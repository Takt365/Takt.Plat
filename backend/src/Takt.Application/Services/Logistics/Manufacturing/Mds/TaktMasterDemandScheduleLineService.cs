// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mds
// 文件名称：TaktMasterDemandScheduleLineService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：主需求计划MDS行应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mds;
using Takt.Domain.Entities.Logistics.Manufacturing.Mds;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mds;

/// <summary>
/// 主需求计划MDS行应用服务
/// </summary>
public class TaktMasterDemandScheduleLineService : TaktServiceBase, ITaktMasterDemandScheduleLineService
{
    private readonly ITaktCompanyRepository<TaktMasterDemandScheduleLine> _masterDemandScheduleLineRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="masterDemandScheduleLineRepository">主需求计划MDS行仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMasterDemandScheduleLineService(
        ITaktCompanyRepository<TaktMasterDemandScheduleLine> masterDemandScheduleLineRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _masterDemandScheduleLineRepository = masterDemandScheduleLineRepository;
        _uniqueValidator = uniqueValidator;
        _lineNumberGenerator = lineNumberGenerator;
    }

    /// <summary>
    /// 获取主需求计划MDS行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMasterDemandScheduleLineDto>> GetMasterDemandScheduleLineListAsync(TaktMasterDemandScheduleLineQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _masterDemandScheduleLineRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMasterDemandScheduleLineDto>.Create(
            data.Adapt<List<TaktMasterDemandScheduleLineDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取主需求计划MDS行
    /// </summary>
    /// <param name="id">主需求计划MDS行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleLineDto?> GetMasterDemandScheduleLineByIdAsync(long id)
    {
        var entity = await _masterDemandScheduleLineRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMasterDemandScheduleLineDto>();
    }

    /// <summary>
    /// 获取主需求计划MDS行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMasterDemandScheduleLineOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _masterDemandScheduleLineRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MdsCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MdsCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建主需求计划MDS行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleLineDto> CreateMasterDemandScheduleLineAsync(TaktMasterDemandScheduleLineCreateDto dto)
    {
        var entity = dto.Adapt<TaktMasterDemandScheduleLine>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_planning_mds_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
            _masterDemandScheduleLineRepository,
            x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                && x.MaterialCode == entity.MaterialCode
                && x.BucketStart == entity.BucketStart
                && x.DemandSourceType == entity.DemandSourceType);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mds_line_bucket_unique)
        {
            throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、MaterialCode、BucketStart、DemandSourceType已存在");
        }
        await EnsureMasterDemandScheduleLineNumberAsync(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_mds_line_line_unique = await _uniqueValidator.IsUniqueAsync(
            _masterDemandScheduleLineRepository,
            x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mds_line_line_unique)
        {
            throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、LineNumber、MaterialCode已存在");
        }
        entity = await _masterDemandScheduleLineRepository.CreateAsync(entity);
        return await GetMasterDemandScheduleLineByIdAsync(entity.Id) ?? entity.Adapt<TaktMasterDemandScheduleLineDto>();
    }

    /// <summary>
    /// 更新主需求计划MDS行
    /// </summary>
    /// <param name="id">主需求计划MDS行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterDemandScheduleLineDto> UpdateMasterDemandScheduleLineAsync(long id, TaktMasterDemandScheduleLineUpdateDto dto)
    {
        var entity = await _masterDemandScheduleLineRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("主需求计划MDS行不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_mds_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
            _masterDemandScheduleLineRepository,
            x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                && x.MaterialCode == entity.MaterialCode
                && x.BucketStart == entity.BucketStart
                && x.DemandSourceType == entity.DemandSourceType,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mds_line_bucket_unique)
        {
            throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、MaterialCode、BucketStart、DemandSourceType已存在");
        }
        await EnsureMasterDemandScheduleLineNumberAsync(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_mds_line_line_unique = await _uniqueValidator.IsUniqueAsync(
            _masterDemandScheduleLineRepository,
            x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mds_line_line_unique)
        {
            throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、LineNumber、MaterialCode已存在");
        }
        await _masterDemandScheduleLineRepository.UpdateAsync(entity);
        return await GetMasterDemandScheduleLineByIdAsync(id) ?? throw new TaktBusinessException("主需求计划MDS行不存在");
    }

    /// <summary>
    /// 删除主需求计划MDS行
    /// </summary>
    /// <param name="id">主需求计划MDS行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterDemandScheduleLineByIdAsync(long id)
    {
        var deleted = await _masterDemandScheduleLineRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("主需求计划MDS行不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除主需求计划MDS行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterDemandScheduleLineBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMasterDemandScheduleLineByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMasterDemandScheduleLineTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMasterDemandScheduleLineTemplateDto>(
            sheetName ?? "主需求计划MDS行导入模板",
            fileName ?? "主需求计划MDS行导入模板.xlsx");
    }

    /// <summary>
    /// 导入主需求计划MDS行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMasterDemandScheduleLineAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMasterDemandScheduleLineImportDto>(fileStream, sheetName ?? "主需求计划MDS行导入模板");
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
                var entity = rows[i].Adapt<TaktMasterDemandScheduleLine>();
                var importKey = $"{entity.MasterDemandScheduleId}|{entity.MaterialCode}|{entity.BucketStart}|{entity.DemandSourceType}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MasterDemandScheduleId、MaterialCode、BucketStart、DemandSourceType）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_mds_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
                    _masterDemandScheduleLineRepository,
                    x => x.MasterDemandScheduleId == entity.MasterDemandScheduleId
                        && x.MaterialCode == entity.MaterialCode
                        && x.BucketStart == entity.BucketStart
                        && x.DemandSourceType == entity.DemandSourceType);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_mds_line_bucket_unique)
                {
                    throw new TaktBusinessException("主需求计划MDS行的MasterDemandScheduleId、MaterialCode、BucketStart、DemandSourceType已存在");
                }
                await _masterDemandScheduleLineRepository.CreateAsync(entity);
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
    /// 导出主需求计划MDS行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMasterDemandScheduleLineAsync(TaktMasterDemandScheduleLineQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMasterDemandScheduleLineQueryDto());
        var list = await _masterDemandScheduleLineRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMasterDemandScheduleLineExportDto>(),
                sheetName ?? "主需求计划MDS行数据",
                fileName ?? "主需求计划MDS行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMasterDemandScheduleLineExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "主需求计划MDS行数据",
            fileName ?? "主需求计划MDS行导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 未传行号时按主表编码生成下一行号
    /// </summary>
    /// <param name="entity">MDS 行实体</param>
    /// <returns>任务</returns>
    private async Task EnsureMasterDemandScheduleLineNumberAsync(TaktMasterDemandScheduleLine entity)
    {
        if (entity.LineNumber > 0)
        {
            return;
        }
        var maxLine = await _masterDemandScheduleLineRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.MasterDemandScheduleId == entity.MasterDemandScheduleId,
            x => x.LineNumber);
        var businessCode = !string.IsNullOrWhiteSpace(entity.MdsCode) ? entity.MdsCode : entity.MasterDemandScheduleId.ToString();
        entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
    }

    /// <summary>
    /// 构建主需求计划MDS行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMasterDemandScheduleLine, bool>> QueryExpression(TaktMasterDemandScheduleLineQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMasterDemandScheduleLine>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.MasterDemandScheduleId).Contains(keywords)
                || (x.MdsCode != null && x.MdsCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.DemandSourceType).Contains(keywords)
                || SqlFunc.ToString(x.SalesOrderId).Contains(keywords)
                || SqlFunc.ToString(x.SalesOrderLineNumber).Contains(keywords)
                || SqlFunc.ToString(x.SalesForecastId).Contains(keywords)
                || SqlFunc.ToString(x.SalesForecastLineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.DemandQuantity).Contains(keywords)
                || (x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.IsObsolete).Contains(keywords)
                || SqlFunc.ToString(x.BucketStart).Contains(keywords)
                || SqlFunc.ToString(x.BucketEnd).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.MasterDemandScheduleId.HasValue == true)
        {
            exp = exp.And(x => x.MasterDemandScheduleId == queryDto.MasterDemandScheduleId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MdsCode))
        {
            exp = exp.And(x => x.MdsCode != null && x.MdsCode.Contains(queryDto.MdsCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.DemandSourceType.HasValue == true)
        {
            exp = exp.And(x => x.DemandSourceType == queryDto.DemandSourceType);
        }

        if (queryDto?.SalesOrderId.HasValue == true)
        {
            exp = exp.And(x => x.SalesOrderId == queryDto.SalesOrderId);
        }

        if (queryDto?.SalesOrderLineNumber.HasValue == true)
        {
            exp = exp.And(x => x.SalesOrderLineNumber == queryDto.SalesOrderLineNumber);
        }

        if (queryDto?.SalesForecastId.HasValue == true)
        {
            exp = exp.And(x => x.SalesForecastId == queryDto.SalesForecastId);
        }

        if (queryDto?.SalesForecastLineNumber.HasValue == true)
        {
            exp = exp.And(x => x.SalesForecastLineNumber == queryDto.SalesForecastLineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (queryDto?.DemandQuantity.HasValue == true)
        {
            exp = exp.And(x => x.DemandQuantity == queryDto.DemandQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.UnitOfMeasure))
        {
            exp = exp.And(x => x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(queryDto.UnitOfMeasure));
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

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }

        if (queryDto?.BucketStartStart.HasValue == true)
        {
            exp = exp.And(x => x.BucketStart >= queryDto.BucketStartStart);
        }

        if (queryDto?.BucketStartEnd.HasValue == true)
        {
            exp = exp.And(x => x.BucketStart <= queryDto.BucketStartEnd);
        }

        if (queryDto?.BucketEndStart.HasValue == true)
        {
            exp = exp.And(x => x.BucketEnd >= queryDto.BucketEndStart);
        }

        if (queryDto?.BucketEndEnd.HasValue == true)
        {
            exp = exp.And(x => x.BucketEnd <= queryDto.BucketEndEnd);
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
