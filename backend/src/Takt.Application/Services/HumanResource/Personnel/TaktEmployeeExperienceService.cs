// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeExperienceService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：员工工作经历应用服务实现
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
/// 员工工作经历应用服务
/// </summary>
public class TaktEmployeeExperienceService : TaktServiceBase, ITaktEmployeeExperienceService
{
    private readonly ITaktCompanyRepository<TaktEmployeeExperience> _employeeExperienceRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeExperienceRepository">员工工作经历仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeExperienceService(
        ITaktCompanyRepository<TaktEmployeeExperience> employeeExperienceRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeExperienceRepository = employeeExperienceRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工工作经历列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeExperienceDto>> GetEmployeeExperienceListAsync(TaktEmployeeExperienceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeExperienceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeExperienceDto>.Create(
            data.Adapt<List<TaktEmployeeExperienceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工工作经历
    /// </summary>
    /// <param name="id">员工工作经历ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeExperienceDto?> GetEmployeeExperienceByIdAsync(long id)
    {
        var entity = await _employeeExperienceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeExperienceDto>();
    }

    /// <summary>
    /// 获取员工工作经历选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeExperienceOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeExperienceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CompanyName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CompanyName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建员工工作经历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeExperienceDto> CreateEmployeeExperienceAsync(TaktEmployeeExperienceCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeExperience>();
        entity = await _employeeExperienceRepository.CreateAsync(entity);
        return await GetEmployeeExperienceByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeExperienceDto>();
    }

    /// <summary>
    /// 更新员工工作经历
    /// </summary>
    /// <param name="id">员工工作经历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeExperienceDto> UpdateEmployeeExperienceAsync(long id, TaktEmployeeExperienceUpdateDto dto)
    {
        var entity = await _employeeExperienceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工工作经历不存在");
        }
        dto.Adapt(entity);
        await _employeeExperienceRepository.UpdateAsync(entity);
        return await GetEmployeeExperienceByIdAsync(id) ?? throw new TaktBusinessException("员工工作经历不存在");
    }

    /// <summary>
    /// 删除员工工作经历
    /// </summary>
    /// <param name="id">员工工作经历ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeExperienceByIdAsync(long id)
    {
        var deleted = await _employeeExperienceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工工作经历不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工工作经历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeExperienceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeExperienceByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeExperienceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeExperienceTemplateDto>(
            sheetName ?? "员工工作经历导入模板",
            fileName ?? "员工工作经历导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工工作经历
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeExperienceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeExperienceImportDto>(fileStream, sheetName ?? "员工工作经历导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeExperience>();
                await _employeeExperienceRepository.CreateAsync(entity);
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
    /// 导出员工工作经历
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeExperienceAsync(TaktEmployeeExperienceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeExperienceQueryDto());
        var list = await _employeeExperienceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeExperienceExportDto>(),
                sheetName ?? "员工工作经历数据",
                fileName ?? "员工工作经历导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeExperienceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工工作经历数据",
            fileName ?? "员工工作经历导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工工作经历查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeExperience, bool>> QueryExpression(TaktEmployeeExperienceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeExperience>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.CompanyName != null && x.CompanyName.Contains(keywords))
                || (x.PositionName != null && x.PositionName.Contains(keywords))
                || (x.JobContent != null && x.JobContent.Contains(keywords))
                || (x.WitnessName != null && x.WitnessName.Contains(keywords))
                || (x.WitnessPhone != null && x.WitnessPhone.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.EndDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CompanyName))
        {
            exp = exp.And(x => x.CompanyName != null && x.CompanyName.Contains(queryDto.CompanyName));
        }

        if (!string.IsNullOrEmpty(queryDto?.PositionName))
        {
            exp = exp.And(x => x.PositionName != null && x.PositionName.Contains(queryDto.PositionName));
        }

        if (!string.IsNullOrEmpty(queryDto?.JobContent))
        {
            exp = exp.And(x => x.JobContent != null && x.JobContent.Contains(queryDto.JobContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.WitnessName))
        {
            exp = exp.And(x => x.WitnessName != null && x.WitnessName.Contains(queryDto.WitnessName));
        }

        if (!string.IsNullOrEmpty(queryDto?.WitnessPhone))
        {
            exp = exp.And(x => x.WitnessPhone != null && x.WitnessPhone.Contains(queryDto.WitnessPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.StartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.StartDate >= queryDto.StartDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartDate <= queryDto.StartDateEnd);
        }

        if (queryDto?.EndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EndDate >= queryDto.EndDateStart);
        }

        if (queryDto?.EndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EndDate <= queryDto.EndDateEnd);
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
