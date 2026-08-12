// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDetailService.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源子应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变来源子应用服务
/// </summary>
public class TaktSourceEcDetailService : TaktServiceBase, ITaktSourceEcDetailService
{
    private readonly ITaktCompanyRepository<TaktSourceEcDetail> _sourceEcDetailRepository;
    private readonly ITaktCompanyRepository<TaktSourceEc> _sourceEcRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sourceEcDetailRepository">设变来源子仓储</param>
    /// <param name="sourceEcRepository">设变来源主仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSourceEcDetailService(
        ITaktCompanyRepository<TaktSourceEcDetail> sourceEcDetailRepository,
        ITaktCompanyRepository<TaktSourceEc> sourceEcRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sourceEcDetailRepository = sourceEcDetailRepository;
        _sourceEcRepository = sourceEcRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变来源子列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSourceEcDetailDto>> GetSourceEcDetailListAsync(TaktSourceEcDetailQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sourceEcDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSourceEcDetailDto>.Create(
            data.Adapt<List<TaktSourceEcDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变来源子
    /// </summary>
    /// <param name="id">设变来源子ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDetailDto?> GetSourceEcDetailByIdAsync(long id)
    {
        var entity = await _sourceEcDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSourceEcDetailDto>();
    }

    /// <summary>
    /// 获取设变来源子选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSourceEcDetailOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _sourceEcDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SourceLegacyPartName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SourceLegacyPartName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变来源子
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDetailDto> CreateSourceEcDetailAsync(TaktSourceEcDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktSourceEcDetail>();
        await StampSourceEcDetailSourceEcAsync(entity, dto);
        entity = await _sourceEcDetailRepository.CreateAsync(entity);
        return await GetSourceEcDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktSourceEcDetailDto>();
    }

    /// <summary>
    /// 更新设变来源子
    /// </summary>
    /// <param name="id">设变来源子ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDetailDto> UpdateSourceEcDetailAsync(long id, TaktSourceEcDetailUpdateDto dto)
    {
        var entity = await _sourceEcDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变来源子不存在");
        }
        dto.Adapt(entity);
        await StampSourceEcDetailSourceEcAsync(entity, dto);
        await _sourceEcDetailRepository.UpdateAsync(entity);
        return await GetSourceEcDetailByIdAsync(id) ?? throw new TaktBusinessException("设变来源子不存在");
    }

    /// <summary>
    /// 删除设变来源子
    /// </summary>
    /// <param name="id">设变来源子ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSourceEcDetailByIdAsync(long id)
    {
        var deleted = await _sourceEcDetailRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设变来源子不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设变来源子
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSourceEcDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSourceEcDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSourceEcDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSourceEcDetailTemplateDto>(
            sheetName ?? "设变来源子导入模板",
            fileName ?? "设变来源子导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变来源子
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSourceEcDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSourceEcDetailImportDto>(fileStream, sheetName ?? "设变来源子导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSourceEcDetail>();
                var importDto = rows[i].Adapt<TaktSourceEcDetailCreateDto>();
                await StampSourceEcDetailSourceEcAsync(entity, importDto);
                await _sourceEcDetailRepository.CreateAsync(entity);
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
    /// 导出设变来源子
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSourceEcDetailAsync(TaktSourceEcDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSourceEcDetailQueryDto());
        var list = await _sourceEcDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSourceEcDetailExportDto>(),
                sheetName ?? "设变来源子数据",
                fileName ?? "设变来源子导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSourceEcDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变来源子数据",
            fileName ?? "设变来源子导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步设变来源子主表外键（ManyToOne → 设变来源主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSourceEcDetailSourceEcAsync(TaktSourceEcDetail entity, TaktSourceEcDetailCreateDto dto)
    {
        if (dto.SourceEcId <= 0)
        {
            return;
        }
        var master = await _sourceEcRepository.GetByIdAsync(dto.SourceEcId);
        if (master == null)
        {
            throw new TaktBusinessException("设变来源主不存在");
        }
        entity.SourceEcId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变来源子查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSourceEcDetail, bool>> QueryExpression(TaktSourceEcDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSourceEcDetail>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.SourceEcId).Contains(keywords)
                || (x.SourceFinishedProduct != null && x.SourceFinishedProduct.Contains(keywords))
                || (x.SourceParentPart != null && x.SourceParentPart.Contains(keywords))
                || (x.SourceLegacyPartCode != null && x.SourceLegacyPartCode.Contains(keywords))
                || (x.SourceLegacyPartName != null && x.SourceLegacyPartName.Contains(keywords))
                || SqlFunc.ToString(x.SourceLegacyUsage).Contains(keywords)
                || (x.SourceLegacyMountingPosition != null && x.SourceLegacyMountingPosition.Contains(keywords))
                || (x.SourceReplacementPartCode != null && x.SourceReplacementPartCode.Contains(keywords))
                || (x.SourceReplacementPartName != null && x.SourceReplacementPartName.Contains(keywords))
                || SqlFunc.ToString(x.SourceReplacementUsage).Contains(keywords)
                || (x.SourceReplacementMountingPosition != null && x.SourceReplacementMountingPosition.Contains(keywords))
                || (x.SourceBomCode != null && x.SourceBomCode.Contains(keywords))
                || (x.SourceCompatibility != null && x.SourceCompatibility.Contains(keywords))
                || (x.SourceDistinction != null && x.SourceDistinction.Contains(keywords))
                || (x.SourceInstruction != null && x.SourceInstruction.Contains(keywords))
                || (x.SourceLegacyPartDisposition != null && x.SourceLegacyPartDisposition.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.SourceBomEffectiveDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.SourceEcId.HasValue == true)
        {
            exp = exp.And(x => x.SourceEcId == queryDto.SourceEcId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceFinishedProduct))
        {
            exp = exp.And(x => x.SourceFinishedProduct != null && x.SourceFinishedProduct.Contains(queryDto.SourceFinishedProduct));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceParentPart))
        {
            exp = exp.And(x => x.SourceParentPart != null && x.SourceParentPart.Contains(queryDto.SourceParentPart));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceLegacyPartCode))
        {
            exp = exp.And(x => x.SourceLegacyPartCode != null && x.SourceLegacyPartCode.Contains(queryDto.SourceLegacyPartCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceLegacyPartName))
        {
            exp = exp.And(x => x.SourceLegacyPartName != null && x.SourceLegacyPartName.Contains(queryDto.SourceLegacyPartName));
        }

        if (queryDto?.SourceLegacyUsage.HasValue == true)
        {
            exp = exp.And(x => x.SourceLegacyUsage == queryDto.SourceLegacyUsage);
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceLegacyMountingPosition))
        {
            exp = exp.And(x => x.SourceLegacyMountingPosition != null && x.SourceLegacyMountingPosition.Contains(queryDto.SourceLegacyMountingPosition));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceReplacementPartCode))
        {
            exp = exp.And(x => x.SourceReplacementPartCode != null && x.SourceReplacementPartCode.Contains(queryDto.SourceReplacementPartCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceReplacementPartName))
        {
            exp = exp.And(x => x.SourceReplacementPartName != null && x.SourceReplacementPartName.Contains(queryDto.SourceReplacementPartName));
        }

        if (queryDto?.SourceReplacementUsage.HasValue == true)
        {
            exp = exp.And(x => x.SourceReplacementUsage == queryDto.SourceReplacementUsage);
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceReplacementMountingPosition))
        {
            exp = exp.And(x => x.SourceReplacementMountingPosition != null && x.SourceReplacementMountingPosition.Contains(queryDto.SourceReplacementMountingPosition));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceBomCode))
        {
            exp = exp.And(x => x.SourceBomCode != null && x.SourceBomCode.Contains(queryDto.SourceBomCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceCompatibility))
        {
            exp = exp.And(x => x.SourceCompatibility != null && x.SourceCompatibility.Contains(queryDto.SourceCompatibility));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceDistinction))
        {
            exp = exp.And(x => x.SourceDistinction != null && x.SourceDistinction.Contains(queryDto.SourceDistinction));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceInstruction))
        {
            exp = exp.And(x => x.SourceInstruction != null && x.SourceInstruction.Contains(queryDto.SourceInstruction));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceLegacyPartDisposition))
        {
            exp = exp.And(x => x.SourceLegacyPartDisposition != null && x.SourceLegacyPartDisposition.Contains(queryDto.SourceLegacyPartDisposition));
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.SourceBomEffectiveDateStart.HasValue == true)
        {
            exp = exp.And(x => x.SourceBomEffectiveDate >= queryDto.SourceBomEffectiveDateStart);
        }

        if (queryDto?.SourceBomEffectiveDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.SourceBomEffectiveDate <= queryDto.SourceBomEffectiveDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }
}
