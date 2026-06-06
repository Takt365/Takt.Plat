// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Identity
// 文件名称：TaktUserService.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户管理应用服务实现（完整CRUD + 导入导出 + 状态管理 + 密码重置）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using Microsoft.Extensions.Localization;
using SqlSugar;
using Takt.Application.Dtos.Identity;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 用户管理应用服务
/// </summary>
public class TaktUserService : TaktServiceBase, ITaktUserService
{
    private readonly ITaktTenantRepository<TaktUser> _userRepository;
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktRbacService _rbacService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userRepository">用户仓储</param>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktUserService(
        ITaktTenantRepository<TaktUser> userRepository,
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktRbacService rbacService,
        ITaktUserContext userContext,
        ITaktLocalizationService localizationService)
        : base(userContext, localizationService)
    {
        _userRepository = userRepository;
        _employeeRepository = employeeRepository;
        _rbacService = rbacService;
    }

    // ========================================
    // CRUD 方法
    // ========================================

    /// <summary>
    /// 获取用户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktUserDto>> GetUserListAsync(TaktUserQueryDto queryDto)
    {
        LogInformation("开始查询用户列表");

        try
        {
            var predicate = QueryExpression(queryDto);
            var (data, total) = await _userRepository.GetPagedAsync(
                queryDto.PageIndex,
                queryDto.PageSize,
                predicate);

            var dtos = data.Adapt<List<TaktUserDto>>();

            // 填充关联数据
            foreach (var dto in dtos)
            {
                var user = data.First(u => u.Id == dto.UserId);
                await FillUserDtoAsync(dto, user);
            }

            LogInformation("查询用户列表成功，总数: {Total}", total);

            return TaktPagedResult<TaktUserDto>.Create(dtos, total, queryDto.PageIndex, queryDto.PageSize);
        }
        catch (Exception ex)
        {
            LogError(ex, "查询用户列表失败");
            throw;
        }
    }

    /// <summary>
    /// 根据ID获取用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户DTO</returns>
    public async Task<TaktUserDto?> GetUserByIdAsync(long id)
    {
        var entity = await _userRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }

        var dto = entity.Adapt<TaktUserDto>();
        await FillUserDtoAsync(dto, entity);

        return dto;
    }

    /// <summary>
    /// 获取用户选项列表（用于下拉框等）
    /// </summary>
    /// <returns>用户选项列表</returns>
    public async Task<List<TaktSelectOption>> GetUserOptionsAsync()
    {
        var users = await _userRepository.GetListAsync(u => u.TenantCode == CurrentTenantCode);
        return users.Select(u => new TaktSelectOption
        {
            DictValue = u.Id.ToString(),
            DictLabel = $"{u.Username} ({u.Nickname})"
        }).ToList();
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="dto">创建用户DTO</param>
    /// <returns>用户DTO</returns>
    public async Task<TaktUserDto> CreateUserAsync(TaktCreateUserDto dto)
    {
        // 1. 业务校验：用户名唯一性
        var exists = await _userRepository.FirstAsync(u => 
            u.TenantCode == CurrentTenantCode && u.Username == dto.Username);
        if (exists != null)
        {
            ThrowValidationLocalized(TaktValidationI18nKeys.Duplicate, TaktValidationI18nKeys.EntityUserName, dto.Username);
        }

        // 2. 业务校验：员工ID必须存在
        var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (employee == null || employee.TenantCode != CurrentTenantCode)
        {
            ThrowValidationLocalized(TaktValidationI18nKeys.NotFound, TaktValidationI18nKeys.EntityUserEmployeeId, dto.EmployeeId);
        }

        // 3. 创建实体
        var entity = dto.Adapt<TaktUser>();
        entity.TenantCode = CurrentTenantCode;
        entity.IsBuiltIn = TaktYesNo.No;
        entity.PasswordHash = TaktEncryptHelper.HashPassword(dto.PasswordHash);
        entity.CreatedBy = CurrentUserId ?? 0;
        entity.CreatedAt = DateTime.Now;

        entity = await _userRepository.CreateAsync(entity);

        if (dto.RoleIds != null)
        {
            await _rbacService.AssignUserRolesAsync(entity.Id, dto.RoleIds);
        }
        if (dto.CompanyCodes != null)
        {
            await _rbacService.AssignUserCompaniesAsync(entity.Id, dto.CompanyCodes);
        }

        // 4. 返回结果
        return await GetUserByIdAsync(entity.Id) ?? entity.Adapt<TaktUserDto>();
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <param name="dto">更新用户DTO</param>
    /// <returns>用户DTO</returns>
    public async Task<TaktUserDto> UpdateUserAsync(long id, TaktUpdateUserDto dto)
    {
        // 1. 检查实体存在
        var entity = await _userRepository.GetByIdAsync(id);
        entity = EnsureExists(entity, "用户不存在");

        if (entity.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限修改此用户");
        }

        // 2. 业务规则校验：员工ID必须存在
        if (entity.EmployeeId != dto.EmployeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employee == null || employee.TenantCode != CurrentTenantCode)
            {
                ThrowValidationLocalized(TaktValidationI18nKeys.NotFound, TaktValidationI18nKeys.EntityUserEmployeeId, dto.EmployeeId);
            }
        }

        // 3. 更新实体（保留不参与更新的字段）
        var originalPasswordHash = entity.PasswordHash;
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.PasswordHash = originalPasswordHash;
        entity.IsBuiltIn = originalIsBuiltIn;

        // 4. 更新审计字段
        entity.UpdatedBy = CurrentUserId;
        entity.UpdatedAt = DateTime.Now;

        // 5. 保存
        await _userRepository.UpdateAsync(entity);

        if (dto.RoleIds != null)
        {
            await _rbacService.AssignUserRolesAsync(id, dto.RoleIds);
        }
        if (dto.CompanyCodes != null)
        {
            await _rbacService.AssignUserCompaniesAsync(id, dto.CompanyCodes);
        }

        // 6. 返回结果
        return await GetUserByIdAsync(id) ?? entity.Adapt<TaktUserDto>();
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>任务</returns>
    public async Task DeleteUserByIdAsync(long id)
    {
        LogInformation("开始删除用户，ID: {Id}", id);

        try
        {
            // 1. 查询实体
            var entity = await _userRepository.GetByIdAsync(id);
            if (entity == null)
            {
                ThrowBusinessException("用户不存在");
            }

            // 2. 租户权限校验
            if (entity.TenantCode != CurrentTenantCode)
            {
                ThrowBusinessException("无权限删除此用户");
            }

            // 3. 保护规则校验：内置用户不可删
            if (entity.IsBuiltIn == TaktYesNo.Yes)
            {
                ThrowBusinessException("内置用户不允许删除");
            }

            await _rbacService.AssignUserRolesAsync(id, Array.Empty<long>());
            await _rbacService.AssignUserCompaniesAsync(id, Array.Empty<string>());

            // 4. 软删除：设置 IsDeleted = 1，并同步更新 UserStatus = Disabled
            entity.IsDeleted = 1;
            entity.UserStatus = TaktCommonStatus.Disabled;

            await _userRepository.UpdateAsync(entity);

            LogInformation("删除用户成功，ID: {Id}", id);
        }
        catch (Exception ex)
        {
            LogError(ex, $"删除用户失败，ID: {id}");
            throw;
        }
    }

    /// <summary>
    /// 批量删除用户
    /// </summary>
    /// <param name="ids">用户ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteUserBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids.ToList();
        if (idList.Count == 0) return;

        LogInformation("开始批量删除用户，数量: {Count}", idList.Count);

        try
        {
            // 1. 获取所有要删除的实体（带租户过滤）
            var entities = new List<TaktUser>();
            foreach (var id in idList)
            {
                var entity = await _userRepository.GetByIdAsync(id);
                if (entity != null && entity.TenantCode == CurrentTenantCode)
                {
                    entities.Add(entity);
                }
            }

            if (entities.Count == 0)
            {
                ThrowBusinessException("未找到可删除的用户");
            }

            // 2. 保护规则校验：内置用户不可删
            if (entities.Any(e => e.IsBuiltIn == TaktYesNo.Yes))
            {
                ThrowBusinessException("内置用户不允许删除");
            }

            foreach (var entity in entities)
            {
                await _rbacService.AssignUserRolesAsync(entity.Id, Array.Empty<long>());
                await _rbacService.AssignUserCompaniesAsync(entity.Id, Array.Empty<string>());
            }

            // 3. 批量软删：设置 IsDeleted = 1，并同步更新 UserStatus = Disabled
            foreach (var entity in entities)
            {
                entity.IsDeleted = 1;
                entity.UserStatus = TaktCommonStatus.Disabled;
            }

            await _userRepository.UpdateRangeAsync(entities);

            LogInformation("批量删除用户成功，数量: {Count}", entities.Count);
        }
        catch (Exception ex)
        {
            LogError(ex, "批量删除用户失败");
            throw;
        }
    }

    // ========================================
    // 状态管理方法
    // ========================================

    /// <summary>
    /// 更新用户状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>用户DTO</returns>
    public async Task<TaktUserDto> UpdateUserStatusAsync(TaktUserStatusDto dto)
    {
        var entity = await _userRepository.GetByIdAsync(dto.UserId);
        entity = EnsureExists(entity, "用户不存在");

        if (entity.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限修改此用户状态");
        }

        // 业务规则：禁止禁用内置用户
        if (entity.IsBuiltIn == TaktYesNo.Yes && dto.UserStatus == TaktCommonStatus.Disabled)
        {
            ThrowBusinessException("不允许禁用内置用户");
        }

        entity.UserStatus = dto.UserStatus;
        entity.UpdatedBy = CurrentUserId;
        entity.UpdatedAt = DateTime.Now;

        await _userRepository.UpdateAsync(entity);

        LogInformation("用户状态更新成功: UserId={UserId}, Status={Status}", dto.UserId, dto.UserStatus);

        return await GetUserByIdAsync(entity.Id) ?? entity.Adapt<TaktUserDto>();
    }

    // ========================================
    // 密码管理
    // ========================================

    /// <summary>
    /// 重置用户密码（管理员按 UserId 重置）
    /// </summary>
    /// <param name="dto">重置密码 DTO</param>
    /// <returns>任务</returns>
    public async Task ResetUserPasswordAsync(TaktResetPasswordDto dto)
    {
        if (dto.UserId <= 0)
        {
            ThrowBusinessException("用户ID无效");
        }

        ValidateNewPassword(dto.NewPassword);

        var entity = await _userRepository.GetByIdAsync(dto.UserId);
        EnsureExists(entity, "用户不存在");

        if (entity!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限重置此用户密码");
        }

        if (entity.IsBuiltIn == TaktYesNo.Yes)
        {
            ThrowBusinessException("不允许重置内置用户密码");
        }

        entity.PasswordHash = TaktEncryptHelper.HashPassword(dto.NewPassword);
        entity.LoginFailCount = 0;
        entity.LockedUntil = null;
        entity.UpdatedBy = CurrentUserId;
        entity.UpdatedAt = DateTime.Now;

        await _userRepository.UpdateAsync(entity);

        LogInformation("用户密码重置成功: UserId={UserId}", dto.UserId);
    }

    /// <summary>
    /// 修改密码（当前登录用户）
    /// </summary>
    /// <param name="dto">修改密码 DTO</param>
    /// <returns>任务</returns>
    public async Task ChangePasswordAsync(TaktChangePasswordDto dto)
    {
        if (!IsAuthenticated || !CurrentUserId.HasValue)
        {
            ThrowBusinessException("用户未登录，无法修改密码");
        }

        if (string.IsNullOrWhiteSpace(dto.OldPassword))
        {
            ThrowBusinessException("旧密码不能为空");
        }

        if (dto.NewPassword != dto.ConfirmPassword)
        {
            ThrowBusinessException("两次输入的新密码不一致");
        }

        if (dto.OldPassword == dto.NewPassword)
        {
            ThrowBusinessException("新密码不能与旧密码相同");
        }

        ValidateNewPassword(dto.NewPassword);

        var entity = await _userRepository.GetByIdAsync(CurrentUserId.Value);
        EnsureExists(entity, "用户不存在");

        if (entity!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限修改密码");
        }

        if (!TaktEncryptHelper.VerifyPassword(dto.OldPassword, entity.PasswordHash))
        {
            ThrowBusinessException("旧密码不正确");
        }

        entity.PasswordHash = TaktEncryptHelper.HashPassword(dto.NewPassword);
        entity.UpdatedBy = CurrentUserId;
        entity.UpdatedAt = DateTime.Now;

        await _userRepository.UpdateAsync(entity);

        LogInformation("用户密码修改成功: UserId={UserId}", CurrentUserId.Value);
    }

    /// <summary>
    /// 忘记密码（发送密码重置邮件）
    /// </summary>
    /// <param name="dto">忘记密码 DTO</param>
    /// <returns>结果</returns>
    public async Task<TaktForgotPasswordResultDto> ForgotPasswordAsync(TaktForgotPasswordDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UsernameOrEmail))
        {
            return new TaktForgotPasswordResultDto
            {
                Success = false,
                Message = GetValidationMessage(TaktValidationI18nKeys.Required, TaktValidationI18nKeys.FieldUsernameOrEmail),
            };
        }

        if (string.IsNullOrWhiteSpace(CurrentTenantCode))
        {
            return new TaktForgotPasswordResultDto
            {
                Success = false,
                Message = TaktValidationMessageHelper.Build(
                    k => GetLocalizedMessage(k),
                    TaktValidationI18nKeys.TipSelectFirst,
                    extraTokens: new Dictionary<string, string> { ["target"] = GetLocalizedMessage(TaktValidationI18nKeys.EntityTenantSelf) }),
            };
        }

        var keyword = dto.UsernameOrEmail.Trim();
        var user = await _userRepository.FirstAsync(u =>
            u.TenantCode == CurrentTenantCode && u.Username == keyword);

        if (user == null)
        {
            return new TaktForgotPasswordResultDto
            {
                Success = false,
                Code = "EmailNotFound",
                Message = GetValidationMessage(TaktValidationI18nKeys.NotFound, TaktValidationI18nKeys.EntityUserSelf),
            };
        }

        if (user.IsBuiltIn == TaktYesNo.Yes)
        {
            return new TaktForgotPasswordResultDto
            {
                Success = false,
                Code = "ProtectedUser",
                Message = GetLocalizedMessage(TaktValidationI18nKeys.TipResetPasswordUnavailable),
            };
        }

        // TODO: 发送密码重置邮件
        LogInformation("忘记密码请求已受理: UserId={UserId}, Username={Username}", user.Id, user.Username);

        return new TaktForgotPasswordResultDto { Success = true };
    }

    /// <summary>
    /// 解锁用户
    /// </summary>
    /// <param name="dto">解锁用户 DTO</param>
    /// <returns>用户 DTO</returns>
    public async Task<TaktUserDto> UnlockUserAsync(TaktUserUnlockDto dto)
    {
        if (dto.UserId <= 0)
        {
            ThrowBusinessException("用户ID无效");
        }

        var entity = await _userRepository.GetByIdAsync(dto.UserId);
        EnsureExists(entity, "用户不存在");

        if (entity!.TenantCode != CurrentTenantCode)
        {
            ThrowBusinessException("无权限解锁此用户");
        }

        entity.LoginFailCount = 0;
        entity.LockedUntil = null;
        entity.UpdatedBy = CurrentUserId;
        entity.UpdatedAt = DateTime.Now;

        await _userRepository.UpdateAsync(entity);

        var reasonText = string.IsNullOrWhiteSpace(dto.Reason) ? "无" : dto.Reason;
        LogInformation("用户解锁成功: UserId={UserId}, Reason={Reason}", dto.UserId, reasonText);

        return await GetUserByIdAsync(entity.Id) ?? entity.Adapt<TaktUserDto>();
    }

    /// <summary>
    /// 统计用户总数
    /// </summary>
    /// <returns>用户总数</returns>
    public async Task<long> GetUserCountAsync()
    {
        return await _userRepository.CountAsync(u => u.TenantCode == CurrentTenantCode);
    }

    // ========================================
    // 导入导出方法
    // ========================================

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名（可选）</param>
    /// <param name="fileName">文件名（可选）</param>
    /// <returns>Excel文件信息</returns>
    public async Task<(string fileName, byte[] content)> GetUserTemplateAsync(
        string? sheetName = null, 
        string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktUserTemplateDto>(
            sheetName ?? "用户导入模板",
            fileName ?? "用户导入模板.xlsx");
    }

    /// <summary>
    /// 导入用户
    /// </summary>
    /// <param name="fileStream">Excel流</param>
    /// <param name="sheetName">工作表名称（可选）</param>
    /// <returns>成功数、失败数、错误信息列表</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportUserAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        int success = 0, fail = 0;

        try
        {
            var importData = await TaktExcelHelper.ImportAsync<TaktUserImportDto>(fileStream, sheetName ?? "用户导入模板");

            if (importData == null || importData.Count == 0)
            {
                errors.Add("Excel文件中没有数据");
                return (0, 0, errors);
            }

            var entitiesToInsert = new List<TaktUser>();

            for (int i = 0; i < importData.Count; i++)
            {
                var row = importData[i];
                int rowNumber = i + 2; // Excel 行号（从2开始，1是表头）

                try
                {
                    // 业务校验
                    if (string.IsNullOrWhiteSpace(row.Username))
                    {
                        errors.Add($"第{rowNumber}行：用户名不能为空");
                        fail++;
                        continue;
                    }

                    if (row.Username.Length != 8)
                    {
                        errors.Add($"第{rowNumber}行：用户名必须为8位");
                        fail++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(row.EmployeeCode))
                    {
                        errors.Add($"第{rowNumber}行：员工编号不能为空");
                        fail++;
                        continue;
                    }

                    // 唯一性校验
                    var exists = await _userRepository.FirstAsync(u => 
                        u.TenantCode == CurrentTenantCode && u.Username == row.Username);
                    if (exists != null)
                    {
                        errors.Add($"第{rowNumber}行：用户名{row.Username}已存在");
                        fail++;
                        continue;
                    }

                    // 查找员工
                    var employee = await _employeeRepository.FirstAsync(e => 
                        e.TenantCode == CurrentTenantCode && e.EmployeeNo == row.EmployeeCode);
                    if (employee == null)
                    {
                        errors.Add($"第{rowNumber}行：员工编号{row.EmployeeCode}不存在");
                        fail++;
                        continue;
                    }

                    var plainPassword = string.IsNullOrWhiteSpace(row.PasswordHash)
                        ? "12345678"
                        : row.PasswordHash;
                    try
                    {
                        ValidateNewPassword(plainPassword);
                    }
                    catch (TaktBusinessException ex)
                    {
                        errors.Add($"第{rowNumber}行：{ex.Message}");
                        fail++;
                        continue;
                    }

                    // 添加到待插入列表
                    var entity = new TaktUser
                    {
                        Username = row.Username,
                        Nickname = row.Nickname,
                        UserType = row.UserType,
                        EmployeeId = row.EmployeeId > 0 ? row.EmployeeId : employee.Id,
                        UserStatus = row.UserStatus,
                        PasswordHash = TaktEncryptHelper.HashPassword(plainPassword),
                        TenantCode = CurrentTenantCode,
                        CreatedBy = CurrentUserId ?? 0,
                        CreatedAt = DateTime.Now
                    };
                    entitiesToInsert.Add(entity);
                }
                catch (Exception ex)
                {
                    errors.Add($"第{rowNumber}行：{ex.Message}");
                    fail++;
                }
            }

            // 批量插入
            if (entitiesToInsert.Any())
            {
                await _userRepository.CreateRangeAsync(entitiesToInsert);
                success = entitiesToInsert.Count;
            }
        }
        catch (Exception ex)
        {
            errors.Add($"导入过程发生错误：{ex.Message}");
            fail++;
        }

        return (success, fail, errors);
    }

    /// <summary>
    /// 导出用户
    /// </summary>
    /// <param name="query">查询DTO（可为null）</param>
    /// <param name="sheetName">工作表名（可选）</param>
    /// <param name="fileName">文件名（可选）</param>
    /// <returns>Excel文件信息（文件名与内容）</returns>
    public async Task<(string fileName, byte[] content)> ExportUserAsync(
        TaktUserQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktUserQueryDto());

        var list = await _userRepository.GetListForExportAsync(predicate);

        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktUserExportDto>(),
                sheetName ?? "用户数据",
                fileName ?? "用户导出.xlsx");
        }

        var exportData = new List<TaktUserExportDto>();

        foreach (var user in list)
        {
            var exportDto = user.Adapt<TaktUserExportDto>();
            exportDto.UserTypeName = user.UserType switch
            {
                TaktUserType.Normal => "普通用户",
                TaktUserType.Admin => "管理员",
                TaktUserType.SuperAdmin => "超级管理员",
                _ => "未知"
            };
            exportDto.StatusName = user.UserStatus switch
            {
                TaktCommonStatus.Enabled => "启用",
                TaktCommonStatus.Disabled => "禁用",
                _ => "未知"
            };

            // 填充员工姓名
            if (user.EmployeeId > 0)
            {
                var employee = await _employeeRepository.GetByIdAsync(user.EmployeeId);
                if (employee != null)
                {
                    exportDto.EmployeeName = employee.Name;
                }
            }

            var userRoles = await _rbacService.GetUserRoleIdsAsync(user.Id);
            if (userRoles.Count > 0)
            {
                exportDto.RoleNames = string.Join(", ", userRoles
                    .Select(ur => ur.RoleName)
                    .Where(name => !string.IsNullOrWhiteSpace(name)));
            }

            exportData.Add(exportDto);
        }

        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "用户数据",
            fileName ?? "用户导出.xlsx");
    }

    /// <summary>
    /// 校验新密码基本规则
    /// </summary>
    /// <param name="password">明文密码</param>
    private static void ValidateNewPassword(string password)
    {
        const int minLength = 8;

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new TaktBusinessException("新密码不能为空");
        }

        if (password.Length < minLength)
        {
            throw new TaktBusinessException($"密码长度不能少于{minLength}位");
        }

        const int maxLength = 20;
        if (password.Length > maxLength)
        {
            throw new TaktBusinessException($"密码长度不能超过{maxLength}位");
        }
    }

    // ========================================
    // DTO 填充辅助方法
    // ========================================

    /// <summary>
    /// 填充用户 DTO 关联数据（员工信息、角色信息）
    /// </summary>
    /// <param name="dto">用户DTO</param>
    /// <param name="user">用户实体</param>
    /// <returns>任务</returns>
    private async Task FillUserDtoAsync(TaktUserDto dto, TaktUser user)
    {
        // 填充员工信息
        if (user.EmployeeId > 0)
        {
            var employee = await _employeeRepository.GetByIdAsync(user.EmployeeId);
            if (employee != null)
            {
                dto.EmployeeName = employee.Name;
            }
        }

        var userRoles = await _rbacService.GetUserRoleIdsAsync(user.Id);
        if (userRoles.Count > 0)
        {
            dto.RoleIds = userRoles.Select(ur => ur.RoleId).ToArray();
            dto.RoleNames = userRoles
                .Select(ur => ur.RoleName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name!)
                .ToList();
        }

        var userCompanies = await _rbacService.GetUserCompanyIdsAsync(user.Id);
        if (userCompanies.Count > 0)
        {
            dto.CompanyCodes = userCompanies
                .Select(uc => uc.CompanyCode)
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Select(code => code!)
                .ToArray();
        }
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建用户查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktUser, bool>> QueryExpression(TaktUserQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktUser>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.Username != null && x.Username.Contains(keywords))
                || (x.Nickname != null && x.Nickname.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || (x.DefaultCulture != null && x.DefaultCulture.Contains(keywords))
                || SqlFunc.ToString(x.UserType).Contains(keywords)
                || SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || SqlFunc.ToString(x.UserStatus).Contains(keywords)
                || SqlFunc.ToString(x.CreatedBy).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords));
        }

        if (!string.IsNullOrEmpty(queryDto?.Username))
        {
            exp = exp.And(x => x.Username != null && x.Username.Contains(queryDto.Username));
        }

        if (!string.IsNullOrEmpty(queryDto?.Nickname))
        {
            exp = exp.And(x => x.Nickname != null && x.Nickname.Contains(queryDto.Nickname));
        }

        if (queryDto?.UserType.HasValue == true)
        {
            exp = exp.And(x => x.UserType == queryDto.UserType);
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (queryDto?.UserStatus.HasValue == true)
        {
            exp = exp.And(x => x.UserStatus == queryDto.UserStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.DefaultCulture))
        {
            exp = exp.And(x => x.DefaultCulture != null && x.DefaultCulture.Contains(queryDto.DefaultCulture));
        }

        if (queryDto?.CreatedBy.HasValue == true)
        {
            exp = exp.And(x => x.CreatedBy == queryDto.CreatedBy);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
