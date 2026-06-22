// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeEducationService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：员工教育经历应用服务实现
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

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 员工教育经历应用服务
/// </summary>
public class TaktEmployeeEducationService : TaktServiceBase, ITaktEmployeeEducationService
{
    private readonly ITaktCompanyRepository<TaktEmployeeEducation> _employeeEducationRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeEducationRepository">员工教育经历仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeEducationService(
        ITaktCompanyRepository<TaktEmployeeEducation> employeeEducationRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeEducationRepository = employeeEducationRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工教育经历列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeEducationDto>> GetEmployeeEducationListAsync(TaktEmployeeEducationQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeEducationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeEducationDto>.Create(
            data.Adapt<List<TaktEmployeeEducationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工教育经历
    /// </summary>
    /// <param name="id">员工教育经历ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeEducationDto?> GetEmployeeEducationByIdAsync(long id)
    {
        var entity = await _employeeEducationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeEducationDto>();
    }

    /// <summary>
    /// 获取员工教育经历选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeEducationOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeEducationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SchoolName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SchoolName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建员工教育经历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeEducationDto> CreateEmployeeEducationAsync(TaktEmployeeEducationCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeEducation>();
        entity = await _employeeEducationRepository.CreateAsync(entity);
        return await GetEmployeeEducationByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeEducationDto>();
    }

    /// <summary>
    /// 更新员工教育经历
    /// </summary>
    /// <param name="id">员工教育经历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeEducationDto> UpdateEmployeeEducationAsync(long id, TaktEmployeeEducationUpdateDto dto)
    {
        var entity = await _employeeEducationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工教育经历不存在");
        }
        dto.Adapt(entity);
        await _employeeEducationRepository.UpdateAsync(entity);
        return await GetEmployeeEducationByIdAsync(id) ?? throw new TaktBusinessException("员工教育经历不存在");
    }

    /// <summary>
    /// 删除员工教育经历
    /// </summary>
    /// <param name="id">员工教育经历ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeEducationByIdAsync(long id)
    {
        var deleted = await _employeeEducationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工教育经历不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工教育经历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeEducationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeEducationByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeEducationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeEducationTemplateDto>(
            sheetName ?? "员工教育经历导入模板",
            fileName ?? "员工教育经历导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工教育经历
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeEducationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeEducationImportDto>(fileStream, sheetName ?? "员工教育经历导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeEducation>();
                await _employeeEducationRepository.CreateAsync(entity);
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
    /// 导出员工教育经历
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeEducationAsync(TaktEmployeeEducationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeEducationQueryDto());
        var list = await _employeeEducationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeEducationExportDto>(),
                sheetName ?? "员工教育经历数据",
                fileName ?? "员工教育经历导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeEducationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工教育经历数据",
            fileName ?? "员工教育经历导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工教育经历查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeEducation, bool>> QueryExpression(TaktEmployeeEducationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeEducation>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.SchoolName != null && x.SchoolName.Contains(keywords))
                || SqlFunc.ToString(x.EducationLevel).Contains(keywords)
                || SqlFunc.ToString(x.DegreeLevel).Contains(keywords)
                || (x.MajorName != null && x.MajorName.Contains(keywords))
                || (x.CertificateNo != null && x.CertificateNo.Contains(keywords))
                || SqlFunc.ToString(x.IsHighest).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
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

        if (!string.IsNullOrEmpty(queryDto?.SchoolName))
        {
            exp = exp.And(x => x.SchoolName != null && x.SchoolName.Contains(queryDto.SchoolName));
        }

        if (queryDto?.EducationLevel.HasValue == true)
        {
            exp = exp.And(x => x.EducationLevel == queryDto.EducationLevel);
        }

        if (queryDto?.DegreeLevel.HasValue == true)
        {
            exp = exp.And(x => x.DegreeLevel == queryDto.DegreeLevel);
        }

        if (!string.IsNullOrEmpty(queryDto?.MajorName))
        {
            exp = exp.And(x => x.MajorName != null && x.MajorName.Contains(queryDto.MajorName));
        }

        if (!string.IsNullOrEmpty(queryDto?.CertificateNo))
        {
            exp = exp.And(x => x.CertificateNo != null && x.CertificateNo.Contains(queryDto.CertificateNo));
        }

        if (queryDto?.IsHighest.HasValue == true)
        {
            exp = exp.And(x => x.IsHighest == queryDto.IsHighest);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
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
