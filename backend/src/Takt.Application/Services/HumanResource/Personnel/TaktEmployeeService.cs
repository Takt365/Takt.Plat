// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：员工应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;
using Takt.Application.Services.Identity;

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 员工应用服务
/// </summary>
public class TaktEmployeeService : TaktServiceBase, ITaktEmployeeService
{
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktRbacService _rbacService;

    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="rbacService">RBAC 关联分配服务</param>

    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeService(
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktRbacService rbacService,

        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeRepository = employeeRepository;
        _rbacService = rbacService;

        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeDto>> GetEmployeeListAsync(TaktEmployeeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeDto>.Create(
            data.Adapt<List<TaktEmployeeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工
    /// </summary>
    /// <param name="id">员工ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDto?> GetEmployeeByIdAsync(long id)
    {
        var entity = await _employeeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktEmployeeDto>();
        dto.EmployeeDepts = await _rbacService.GetEmployeeDeptIdsAsync(entity.Id);
        dto.EmployeePosts = await _rbacService.GetEmployeePostIdsAsync(entity.Id);
        return dto;    }

    /// <summary>
    /// 获取员工选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.Name,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.Name ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建员工
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDto> CreateEmployeeAsync(TaktEmployeeCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployee>();
        entity.IsBuiltIn = TaktYesNo.No;
        var isUnique_ix_employee_no_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeRepository,
            x => x.EmployeeNo == entity.EmployeeNo);
        if (!isUnique_ix_employee_no_unique)
        {
            throw new TaktBusinessException("员工的EmployeeNo已存在");
        }
        entity = await _employeeRepository.CreateAsync(entity);
        if (dto.EmployeeDeptIds != null)
        {
            await _rbacService.AssignEmployeeDeptsAsync(entity.Id, dto.EmployeeDeptIds);
        }
        if (dto.EmployeePostIds != null)
        {
            await _rbacService.AssignEmployeePostsAsync(entity.Id, dto.EmployeePostIds);
        }
        return await GetEmployeeByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeDto>();
    }

    /// <summary>
    /// 更新员工
    /// </summary>
    /// <param name="id">员工ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDto> UpdateEmployeeAsync(long id, TaktEmployeeUpdateDto dto)
    {
        var entity = await _employeeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        var originalEmployeeStatus = entity.EmployeeStatus;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        if (entity.IsBuiltIn == TaktYesNo.Yes && entity.EmployeeStatus != originalEmployeeStatus
            && (entity.EmployeeStatus == 3 || entity.EmployeeStatus == 4))
        {
            throw new TaktBusinessException("不允许将内置员工设为离职或退休");
        }
        var isUnique_ix_employee_no_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeRepository,
            x => x.EmployeeNo == entity.EmployeeNo,
            id);
        if (!isUnique_ix_employee_no_unique)
        {
            throw new TaktBusinessException("员工的EmployeeNo已存在");
        }
        await _employeeRepository.UpdateAsync(entity);
        if (dto.EmployeeDeptIds != null)
        {
            await _rbacService.AssignEmployeeDeptsAsync(id, dto.EmployeeDeptIds);
        }
        if (dto.EmployeePostIds != null)
        {
            await _rbacService.AssignEmployeePostsAsync(id, dto.EmployeePostIds);
        }
        return await GetEmployeeByIdAsync(id) ?? throw new TaktBusinessException("员工不存在");
    }

    /// <summary>
    /// 删除员工
    /// </summary>
    /// <param name="id">员工ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeByIdAsync(long id)
    {
        var entity = await _employeeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工不存在或已删除");
        }
        if (entity.IsBuiltIn == TaktYesNo.Yes)
        {
            throw new TaktBusinessException("内置员工不允许删除");
        }
        await _rbacService.AssignEmployeeDeptsAsync(id, Array.Empty<long>());
        await _rbacService.AssignEmployeePostsAsync(id, Array.Empty<long>());
        var deleted = await _employeeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _employeeRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == TaktYesNo.Yes))
        {
            throw new TaktBusinessException("内置员工不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新员工状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDto> UpdateEmployeeStatusAsync(TaktEmployeeStatusDto dto)
    {
        var entity = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (entity == null)
        {
            throw new TaktBusinessException("员工不存在");
        }
        entity.PoliticalStatus = dto.PoliticalStatus;
        await _employeeRepository.UpdateAsync(entity);
        return await GetEmployeeByIdAsync(dto.EmployeeId) ?? throw new TaktBusinessException("员工不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeTemplateDto>(
            sheetName ?? "员工导入模板",
            fileName ?? "员工导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeImportDto>(fileStream, sheetName ?? "员工导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployee>();
                entity.IsBuiltIn = TaktYesNo.No;
                var importKey = $"{entity.EmployeeNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EmployeeNo）");
                }
                var isUnique_ix_employee_no_unique = await _uniqueValidator.IsUniqueAsync(
                    _employeeRepository,
                    x => x.EmployeeNo == entity.EmployeeNo);
                if (!isUnique_ix_employee_no_unique)
                {
                    throw new TaktBusinessException("员工的EmployeeNo已存在");
                }
                await _employeeRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出员工
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeAsync(TaktEmployeeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeQueryDto());
        var list = await _employeeRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeExportDto>(),
                sheetName ?? "员工数据",
                fileName ?? "员工导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工数据",
            fileName ?? "员工导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployee, bool>> QueryExpression(TaktEmployeeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployee>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EmployeeNo != null && x.EmployeeNo.Contains(keywords))
                || (x.Name != null && x.Name.Contains(keywords))
                || SqlFunc.ToString(x.Gender).Contains(keywords)
                || (x.IdCardNo != null && x.IdCardNo.Contains(keywords))
                || (x.Mobile != null && x.Mobile.Contains(keywords))
                || (x.Email != null && x.Email.Contains(keywords))
                || (x.NativePlace != null && x.NativePlace.Contains(keywords))
                || (x.Ethnicity != null && x.Ethnicity.Contains(keywords))
                || (x.PoliticalStatus != null && x.PoliticalStatus.Contains(keywords))
                || SqlFunc.ToString(x.MaritalStatus).Contains(keywords)
                || SqlFunc.ToString(x.Education).Contains(keywords)
                || (x.GraduateSchool != null && x.GraduateSchool.Contains(keywords))
                || (x.Major != null && x.Major.Contains(keywords))
                || SqlFunc.ToString(x.ResignationType).Contains(keywords)
                || (x.ResignationReason != null && x.ResignationReason.Contains(keywords))
                || SqlFunc.ToString(x.EmployeeStatus).Contains(keywords)
                || SqlFunc.ToString(x.PrimaryDeptId).Contains(keywords)
                || SqlFunc.ToString(x.PrimaryPostId).Contains(keywords)
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || (x.EmergencyContactName != null && x.EmergencyContactName.Contains(keywords))
                || (x.EmergencyContactPhone != null && x.EmergencyContactPhone.Contains(keywords))
                || (x.HomeAddress != null && x.HomeAddress.Contains(keywords))
                || (x.PhotoUrl != null && x.PhotoUrl.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.BirthDate).Contains(keywords)
                || SqlFunc.ToString(x.JoinedDate).Contains(keywords)
                || SqlFunc.ToString(x.ProbationEndDate).Contains(keywords)
                || SqlFunc.ToString(x.RegularDate).Contains(keywords)
                || SqlFunc.ToString(x.TerminationDate).Contains(keywords)
                || SqlFunc.ToString(x.LastWorkDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.EmployeeNo))
        {
            exp = exp.And(x => x.EmployeeNo != null && x.EmployeeNo.Contains(queryDto.EmployeeNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.Name))
        {
            exp = exp.And(x => x.Name != null && x.Name.Contains(queryDto.Name));
        }

        if (queryDto?.Gender.HasValue == true)
        {
            exp = exp.And(x => x.Gender == queryDto.Gender);
        }

        if (!string.IsNullOrEmpty(queryDto?.IdCardNo))
        {
            exp = exp.And(x => x.IdCardNo != null && x.IdCardNo.Contains(queryDto.IdCardNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.Mobile))
        {
            exp = exp.And(x => x.Mobile != null && x.Mobile.Contains(queryDto.Mobile));
        }

        if (!string.IsNullOrEmpty(queryDto?.Email))
        {
            exp = exp.And(x => x.Email != null && x.Email.Contains(queryDto.Email));
        }

        if (!string.IsNullOrEmpty(queryDto?.NativePlace))
        {
            exp = exp.And(x => x.NativePlace != null && x.NativePlace.Contains(queryDto.NativePlace));
        }

        if (!string.IsNullOrEmpty(queryDto?.Ethnicity))
        {
            exp = exp.And(x => x.Ethnicity != null && x.Ethnicity.Contains(queryDto.Ethnicity));
        }

        if (!string.IsNullOrEmpty(queryDto?.PoliticalStatus))
        {
            exp = exp.And(x => x.PoliticalStatus != null && x.PoliticalStatus.Contains(queryDto.PoliticalStatus));
        }

        if (queryDto?.MaritalStatus.HasValue == true)
        {
            exp = exp.And(x => x.MaritalStatus == queryDto.MaritalStatus);
        }

        if (queryDto?.Education.HasValue == true)
        {
            exp = exp.And(x => x.Education == queryDto.Education);
        }

        if (!string.IsNullOrEmpty(queryDto?.GraduateSchool))
        {
            exp = exp.And(x => x.GraduateSchool != null && x.GraduateSchool.Contains(queryDto.GraduateSchool));
        }

        if (!string.IsNullOrEmpty(queryDto?.Major))
        {
            exp = exp.And(x => x.Major != null && x.Major.Contains(queryDto.Major));
        }

        if (queryDto?.ResignationType.HasValue == true)
        {
            exp = exp.And(x => x.ResignationType == queryDto.ResignationType);
        }

        if (!string.IsNullOrEmpty(queryDto?.ResignationReason))
        {
            exp = exp.And(x => x.ResignationReason != null && x.ResignationReason.Contains(queryDto.ResignationReason));
        }

        if (queryDto?.EmployeeStatus.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeStatus == queryDto.EmployeeStatus);
        }

        if (queryDto?.PrimaryDeptId.HasValue == true)
        {
            exp = exp.And(x => x.PrimaryDeptId == queryDto.PrimaryDeptId);
        }

        if (queryDto?.PrimaryPostId.HasValue == true)
        {
            exp = exp.And(x => x.PrimaryPostId == queryDto.PrimaryPostId);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
        }

        if (!string.IsNullOrEmpty(queryDto?.EmergencyContactName))
        {
            exp = exp.And(x => x.EmergencyContactName != null && x.EmergencyContactName.Contains(queryDto.EmergencyContactName));
        }

        if (!string.IsNullOrEmpty(queryDto?.EmergencyContactPhone))
        {
            exp = exp.And(x => x.EmergencyContactPhone != null && x.EmergencyContactPhone.Contains(queryDto.EmergencyContactPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.HomeAddress))
        {
            exp = exp.And(x => x.HomeAddress != null && x.HomeAddress.Contains(queryDto.HomeAddress));
        }

        if (!string.IsNullOrEmpty(queryDto?.PhotoUrl))
        {
            exp = exp.And(x => x.PhotoUrl != null && x.PhotoUrl.Contains(queryDto.PhotoUrl));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.BirthDateStart.HasValue == true)
        {
            exp = exp.And(x => x.BirthDate >= queryDto.BirthDateStart);
        }

        if (queryDto?.BirthDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.BirthDate <= queryDto.BirthDateEnd);
        }

        if (queryDto?.JoinedDateStart.HasValue == true)
        {
            exp = exp.And(x => x.JoinedDate >= queryDto.JoinedDateStart);
        }

        if (queryDto?.JoinedDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.JoinedDate <= queryDto.JoinedDateEnd);
        }

        if (queryDto?.ProbationEndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ProbationEndDate >= queryDto.ProbationEndDateStart);
        }

        if (queryDto?.ProbationEndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ProbationEndDate <= queryDto.ProbationEndDateEnd);
        }

        if (queryDto?.RegularDateStart.HasValue == true)
        {
            exp = exp.And(x => x.RegularDate >= queryDto.RegularDateStart);
        }

        if (queryDto?.RegularDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.RegularDate <= queryDto.RegularDateEnd);
        }

        if (queryDto?.TerminationDateStart.HasValue == true)
        {
            exp = exp.And(x => x.TerminationDate >= queryDto.TerminationDateStart);
        }

        if (queryDto?.TerminationDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.TerminationDate <= queryDto.TerminationDateEnd);
        }

        if (queryDto?.LastWorkDateStart.HasValue == true)
        {
            exp = exp.And(x => x.LastWorkDate >= queryDto.LastWorkDateStart);
        }

        if (queryDto?.LastWorkDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.LastWorkDate <= queryDto.LastWorkDateEnd);
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
