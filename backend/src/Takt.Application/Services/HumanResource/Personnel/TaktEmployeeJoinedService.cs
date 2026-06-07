// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeJoinedService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：员工入职上岗应用服务实现
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

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 员工入职上岗应用服务
/// </summary>
public class TaktEmployeeJoinedService : TaktServiceBase, ITaktEmployeeJoinedService
{
    private readonly ITaktApprovalRepository<TaktEmployeeJoined> _employeeJoinedRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeJoinedRepository">员工入职上岗仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeJoinedService(
        ITaktApprovalRepository<TaktEmployeeJoined> employeeJoinedRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeJoinedRepository = employeeJoinedRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工入职上岗列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeJoinedDto>> GetEmployeeJoinedListAsync(TaktEmployeeJoinedQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeJoinedRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeJoinedDto>.Create(
            data.Adapt<List<TaktEmployeeJoinedDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工入职上岗
    /// </summary>
    /// <param name="id">员工入职上岗ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeJoinedDto?> GetEmployeeJoinedByIdAsync(long id)
    {
        var entity = await _employeeJoinedRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeJoinedDto>();
    }

    /// <summary>
    /// 获取员工入职上岗选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeJoinedOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeJoinedRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.DeptName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DeptName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建员工入职上岗
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeJoinedDto> CreateEmployeeJoinedAsync(TaktEmployeeJoinedCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeJoined>();
        entity = await _employeeJoinedRepository.CreateAsync(entity);
        return await GetEmployeeJoinedByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeJoinedDto>();
    }

    /// <summary>
    /// 更新员工入职上岗
    /// </summary>
    /// <param name="id">员工入职上岗ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeJoinedDto> UpdateEmployeeJoinedAsync(long id, TaktEmployeeJoinedUpdateDto dto)
    {
        var entity = await _employeeJoinedRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工入职上岗不存在");
        }
        dto.Adapt(entity);
        await _employeeJoinedRepository.UpdateAsync(entity);
        return await GetEmployeeJoinedByIdAsync(id) ?? throw new TaktBusinessException("员工入职上岗不存在");
    }

    /// <summary>
    /// 删除员工入职上岗
    /// </summary>
    /// <param name="id">员工入职上岗ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeJoinedByIdAsync(long id)
    {
        var deleted = await _employeeJoinedRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工入职上岗不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工入职上岗
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeJoinedBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeJoinedByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeJoinedTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeJoinedTemplateDto>(
            sheetName ?? "员工入职上岗导入模板",
            fileName ?? "员工入职上岗导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工入职上岗
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeJoinedAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeJoinedImportDto>(fileStream, sheetName ?? "员工入职上岗导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeJoined>();
                await _employeeJoinedRepository.CreateAsync(entity);
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
    /// 导出员工入职上岗
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeJoinedAsync(TaktEmployeeJoinedQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeJoinedQueryDto());
        var list = await _employeeJoinedRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeJoinedExportDto>(),
                sheetName ?? "员工入职上岗数据",
                fileName ?? "员工入职上岗导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeJoinedExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工入职上岗数据",
            fileName ?? "员工入职上岗导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工入职上岗查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeJoined, bool>> QueryExpression(TaktEmployeeJoinedQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeJoined>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || SqlFunc.ToString(x.OnboardingId).Contains(keywords)
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || SqlFunc.ToString(x.PostId).Contains(keywords)
                || (x.PostName != null && x.PostName.Contains(keywords))
                || (x.JobTitle != null && x.JobTitle.Contains(keywords))
                || SqlFunc.ToString(x.WorkNature).Contains(keywords)
                || SqlFunc.ToString(x.EmploymentType).Contains(keywords)
                || SqlFunc.ToString(x.DirectManagerId).Contains(keywords)
                || (x.DirectManagerName != null && x.DirectManagerName.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.JoinedDate).Contains(keywords)
                || SqlFunc.ToString(x.ProbationEndDate).Contains(keywords)
                || SqlFunc.ToString(x.RegularDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (queryDto?.OnboardingId.HasValue == true)
        {
            exp = exp.And(x => x.OnboardingId == queryDto.OnboardingId);
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (queryDto?.PostId.HasValue == true)
        {
            exp = exp.And(x => x.PostId == queryDto.PostId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PostName))
        {
            exp = exp.And(x => x.PostName != null && x.PostName.Contains(queryDto.PostName));
        }

        if (!string.IsNullOrEmpty(queryDto?.JobTitle))
        {
            exp = exp.And(x => x.JobTitle != null && x.JobTitle.Contains(queryDto.JobTitle));
        }

        if (queryDto?.WorkNature.HasValue == true)
        {
            exp = exp.And(x => x.WorkNature == queryDto.WorkNature);
        }

        if (queryDto?.EmploymentType.HasValue == true)
        {
            exp = exp.And(x => x.EmploymentType == queryDto.EmploymentType);
        }

        if (queryDto?.DirectManagerId.HasValue == true)
        {
            exp = exp.And(x => x.DirectManagerId == queryDto.DirectManagerId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DirectManagerName))
        {
            exp = exp.And(x => x.DirectManagerName != null && x.DirectManagerName.Contains(queryDto.DirectManagerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
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
