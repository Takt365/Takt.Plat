// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeSkillService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：员工技能应用服务实现
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
/// 员工技能应用服务
/// </summary>
public class TaktEmployeeSkillService : TaktServiceBase, ITaktEmployeeSkillService
{
    private readonly ITaktCompanyRepository<TaktEmployeeSkill> _employeeSkillRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeSkillRepository">员工技能仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeSkillService(
        ITaktCompanyRepository<TaktEmployeeSkill> employeeSkillRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeSkillRepository = employeeSkillRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工技能列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeSkillDto>> GetEmployeeSkillListAsync(TaktEmployeeSkillQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeSkillRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeSkillDto>.Create(
            data.Adapt<List<TaktEmployeeSkillDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工技能
    /// </summary>
    /// <param name="id">员工技能ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeSkillDto?> GetEmployeeSkillByIdAsync(long id)
    {
        var entity = await _employeeSkillRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeSkillDto>();
    }

    /// <summary>
    /// 获取员工技能选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeSkillOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeSkillRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SkillName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SkillName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建员工技能
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeSkillDto> CreateEmployeeSkillAsync(TaktEmployeeSkillCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeSkill>();
        entity = await _employeeSkillRepository.CreateAsync(entity);
        return await GetEmployeeSkillByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeSkillDto>();
    }

    /// <summary>
    /// 更新员工技能
    /// </summary>
    /// <param name="id">员工技能ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeSkillDto> UpdateEmployeeSkillAsync(long id, TaktEmployeeSkillUpdateDto dto)
    {
        var entity = await _employeeSkillRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工技能不存在");
        }
        dto.Adapt(entity);
        await _employeeSkillRepository.UpdateAsync(entity);
        return await GetEmployeeSkillByIdAsync(id) ?? throw new TaktBusinessException("员工技能不存在");
    }

    /// <summary>
    /// 删除员工技能
    /// </summary>
    /// <param name="id">员工技能ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeSkillByIdAsync(long id)
    {
        var deleted = await _employeeSkillRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工技能不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工技能
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeSkillBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeSkillByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeSkillTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeSkillTemplateDto>(
            sheetName ?? "员工技能导入模板",
            fileName ?? "员工技能导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工技能
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeSkillAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeSkillImportDto>(fileStream, sheetName ?? "员工技能导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeSkill>();
                await _employeeSkillRepository.CreateAsync(entity);
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
    /// 导出员工技能
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeSkillAsync(TaktEmployeeSkillQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeSkillQueryDto());
        var list = await _employeeSkillRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeSkillExportDto>(),
                sheetName ?? "员工技能数据",
                fileName ?? "员工技能导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeSkillExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工技能数据",
            fileName ?? "员工技能导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工技能查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeSkill, bool>> QueryExpression(TaktEmployeeSkillQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeSkill>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.SkillName != null && x.SkillName.Contains(keywords))
                || SqlFunc.ToString(x.SkillLevel).Contains(keywords)
                || (x.CertificateName != null && x.CertificateName.Contains(keywords))
                || (x.CertificateNo != null && x.CertificateNo.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ObtainedDate).Contains(keywords)
                || SqlFunc.ToString(x.ExpiryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SkillName))
        {
            exp = exp.And(x => x.SkillName != null && x.SkillName.Contains(queryDto.SkillName));
        }

        if (queryDto?.SkillLevel.HasValue == true)
        {
            exp = exp.And(x => x.SkillLevel == queryDto.SkillLevel);
        }

        if (!string.IsNullOrEmpty(queryDto?.CertificateName))
        {
            exp = exp.And(x => x.CertificateName != null && x.CertificateName.Contains(queryDto.CertificateName));
        }

        if (!string.IsNullOrEmpty(queryDto?.CertificateNo))
        {
            exp = exp.And(x => x.CertificateNo != null && x.CertificateNo.Contains(queryDto.CertificateNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ObtainedDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ObtainedDate >= queryDto.ObtainedDateStart);
        }

        if (queryDto?.ObtainedDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ObtainedDate <= queryDto.ObtainedDateEnd);
        }

        if (queryDto?.ExpiryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate >= queryDto.ExpiryDateStart);
        }

        if (queryDto?.ExpiryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate <= queryDto.ExpiryDateEnd);
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
