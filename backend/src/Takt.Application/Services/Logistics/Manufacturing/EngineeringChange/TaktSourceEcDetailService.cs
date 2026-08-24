// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDetailService.cs
// 创建时间：2026-08-22
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
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sourceEcDetailRepository">设变来源子仓储</param>
    /// <param name="sourceEcRepository">设变来源主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSourceEcDetailService(
        ITaktCompanyRepository<TaktSourceEcDetail> sourceEcDetailRepository,
        ITaktCompanyRepository<TaktSourceEc> sourceEcRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sourceEcDetailRepository = sourceEcDetailRepository;
        _sourceEcRepository = sourceEcRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变来源子列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSourceEcDetailDto>> GetSourceEcDetailListAsync(TaktSourceEcDetailQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSourceEcDetailDto>.Create(
                new List<TaktSourceEcDetailDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
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
            DictValue = e.SourceLegacyPartCode ?? string.Empty,
            DictLabel = e.SourceLegacyPartName ?? e.SourceLegacyPartCode ?? string.Empty,
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
        entity.IsObsolete = 0;
        await StampSourceEcDetailSourceEcAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_source_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _sourceEcDetailRepository,
            x => x.SourceEcId == entity.SourceEcId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_source_detail_line_unique)
        {
            throw new TaktBusinessException("设变来源子的SourceEcId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _sourceEcDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SourceEcId == entity.SourceEcId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.SourceEcCode) ? entity.SourceEcCode : entity.SourceEcId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
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
        var isUnique_ix_takt_logistics_manufacturing_ec_source_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _sourceEcDetailRepository,
            x => x.SourceEcId == entity.SourceEcId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_source_detail_line_unique)
        {
            throw new TaktBusinessException("设变来源子的SourceEcId、LineNumber已存在");
        }
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
        var entity = await _sourceEcDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变来源子不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变来源子不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("设变来源子已作废");
        }
        entity.IsObsolete = 1;
        await _sourceEcDetailRepository.UpdateAsync(entity);
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
    /// 更新设变来源子作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDetailDto> UpdateSourceEcDetailObsoleteAsync(TaktSourceEcDetailObsoleteDto dto)
    {
        var entity = await _sourceEcDetailRepository.GetByIdAsync(dto.SourceEcDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变来源子不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变来源子不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _sourceEcDetailRepository.UpdateAsync(entity);
        return await GetSourceEcDetailByIdAsync(dto.SourceEcDetailId) ?? throw new TaktBusinessException("设变来源子不存在");
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
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSourceEcDetail>();
                var importDto = rows[i].Adapt<TaktSourceEcDetailCreateDto>();
                await StampSourceEcDetailSourceEcAsync(entity, importDto);
                var importKey = $"{entity.SourceEcId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SourceEcId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_source_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _sourceEcDetailRepository,
                    x => x.SourceEcId == entity.SourceEcId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_source_detail_line_unique)
                {
                    throw new TaktBusinessException("设变来源子的SourceEcId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _sourceEcDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SourceEcId == entity.SourceEcId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SourceEcCode) ? entity.SourceEcCode : entity.SourceEcId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
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
        var queryDto = query ?? new TaktSourceEcDetailQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSourceEcDetailExportDto>(),
                sheetName ?? "设变来源子数据",
                fileName ?? "设变来源子导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
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
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.SourceEcCode))
        {
            entity.SourceEcCode = master.SourceEcCode;
        }
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

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SourceEcCode != null && x.SourceEcCode.Contains(keywords))
                || (x.SourceFinishedProduct != null && x.SourceFinishedProduct.Contains(keywords))
                || (x.SourceParentPart != null && x.SourceParentPart.Contains(keywords))
                || (x.SourceLegacyPartCode != null && x.SourceLegacyPartCode.Contains(keywords))
                || (x.SourceLegacyPartName != null && x.SourceLegacyPartName.Contains(keywords))
                || (x.SourceLegacyMountingPosition != null && x.SourceLegacyMountingPosition.Contains(keywords))
                || (x.SourceReplacementPartCode != null && x.SourceReplacementPartCode.Contains(keywords))
                || (x.SourceReplacementPartName != null && x.SourceReplacementPartName.Contains(keywords))
                || (x.SourceReplacementMountingPosition != null && x.SourceReplacementMountingPosition.Contains(keywords))
                || (x.SourceBomCode != null && x.SourceBomCode.Contains(keywords))
                || (x.SourceCompatibility != null && x.SourceCompatibility.Contains(keywords))
                || (x.SourceDistinction != null && x.SourceDistinction.Contains(keywords))
                || (x.SourceInstruction != null && x.SourceInstruction.Contains(keywords))
                || (x.SourceLegacyPartDisposition != null && x.SourceLegacyPartDisposition.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (queryDto?.SourceEcId.HasValue == true)
        {
            var sourceEcId = queryDto.SourceEcId.Value;
            exp = exp.And(x => x.SourceEcId == sourceEcId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceEcCode))
        {
            var sourceEcCode = queryDto.SourceEcCode;
            exp = exp.And(x => x.SourceEcCode != null && x.SourceEcCode.Contains(sourceEcCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceFinishedProduct))
        {
            var sourceFinishedProduct = queryDto.SourceFinishedProduct;
            exp = exp.And(x => x.SourceFinishedProduct != null && x.SourceFinishedProduct.Contains(sourceFinishedProduct));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceParentPart))
        {
            var sourceParentPart = queryDto.SourceParentPart;
            exp = exp.And(x => x.SourceParentPart != null && x.SourceParentPart.Contains(sourceParentPart));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceLegacyPartCode))
        {
            var sourceLegacyPartCode = queryDto.SourceLegacyPartCode;
            exp = exp.And(x => x.SourceLegacyPartCode != null && x.SourceLegacyPartCode.Contains(sourceLegacyPartCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceLegacyPartName))
        {
            var sourceLegacyPartName = queryDto.SourceLegacyPartName;
            exp = exp.And(x => x.SourceLegacyPartName != null && x.SourceLegacyPartName.Contains(sourceLegacyPartName));
        }

        if (queryDto?.SourceLegacyUsage.HasValue == true)
        {
            var sourceLegacyUsage = queryDto.SourceLegacyUsage.Value;
            exp = exp.And(x => x.SourceLegacyUsage == sourceLegacyUsage);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceLegacyMountingPosition))
        {
            var sourceLegacyMountingPosition = queryDto.SourceLegacyMountingPosition;
            exp = exp.And(x => x.SourceLegacyMountingPosition != null && x.SourceLegacyMountingPosition.Contains(sourceLegacyMountingPosition));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceReplacementPartCode))
        {
            var sourceReplacementPartCode = queryDto.SourceReplacementPartCode;
            exp = exp.And(x => x.SourceReplacementPartCode != null && x.SourceReplacementPartCode.Contains(sourceReplacementPartCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceReplacementPartName))
        {
            var sourceReplacementPartName = queryDto.SourceReplacementPartName;
            exp = exp.And(x => x.SourceReplacementPartName != null && x.SourceReplacementPartName.Contains(sourceReplacementPartName));
        }

        if (queryDto?.SourceReplacementUsage.HasValue == true)
        {
            var sourceReplacementUsage = queryDto.SourceReplacementUsage.Value;
            exp = exp.And(x => x.SourceReplacementUsage == sourceReplacementUsage);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceReplacementMountingPosition))
        {
            var sourceReplacementMountingPosition = queryDto.SourceReplacementMountingPosition;
            exp = exp.And(x => x.SourceReplacementMountingPosition != null && x.SourceReplacementMountingPosition.Contains(sourceReplacementMountingPosition));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceBomCode))
        {
            var sourceBomCode = queryDto.SourceBomCode;
            exp = exp.And(x => x.SourceBomCode != null && x.SourceBomCode.Contains(sourceBomCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceCompatibility))
        {
            var sourceCompatibility = queryDto.SourceCompatibility;
            exp = exp.And(x => x.SourceCompatibility != null && x.SourceCompatibility.Contains(sourceCompatibility));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceDistinction))
        {
            var sourceDistinction = queryDto.SourceDistinction;
            exp = exp.And(x => x.SourceDistinction != null && x.SourceDistinction.Contains(sourceDistinction));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceInstruction))
        {
            var sourceInstruction = queryDto.SourceInstruction;
            exp = exp.And(x => x.SourceInstruction != null && x.SourceInstruction.Contains(sourceInstruction));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceLegacyPartDisposition))
        {
            var sourceLegacyPartDisposition = queryDto.SourceLegacyPartDisposition;
            exp = exp.And(x => x.SourceLegacyPartDisposition != null && x.SourceLegacyPartDisposition.Contains(sourceLegacyPartDisposition));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.SourceBomEffectiveDateStart.HasValue == true)
        {
            var sourceBomEffectiveDateStart = queryDto.SourceBomEffectiveDateStart.Value;
            exp = exp.And(x => x.SourceBomEffectiveDate >= sourceBomEffectiveDateStart);
        }

        if (queryDto?.SourceBomEffectiveDateEnd.HasValue == true)
        {
            var sourceBomEffectiveDateEnd = queryDto.SourceBomEffectiveDateEnd.Value;
            exp = exp.And(x => x.SourceBomEffectiveDate <= sourceBomEffectiveDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart.Value;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd.Value;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktSourceEcDetailQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (queryDto.SourceEcId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceEcCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceFinishedProduct))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceParentPart))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceLegacyPartCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceLegacyPartName))
        {
            return true;
        }
        if (queryDto.SourceLegacyUsage.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceLegacyMountingPosition))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceReplacementPartCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceReplacementPartName))
        {
            return true;
        }
        if (queryDto.SourceReplacementUsage.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceReplacementMountingPosition))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceBomCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceCompatibility))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceDistinction))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceInstruction))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceLegacyPartDisposition))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.IsObsolete.HasValue)
        {
            return true;
        }
        if (queryDto.SourceBomEffectiveDateStart.HasValue || queryDto.SourceBomEffectiveDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
