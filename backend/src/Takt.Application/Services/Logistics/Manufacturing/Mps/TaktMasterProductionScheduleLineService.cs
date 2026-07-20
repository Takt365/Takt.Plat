// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：TaktMasterProductionScheduleLineService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：主生产计划MPS行应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mps;

/// <summary>
/// 主生产计划MPS行应用服务
/// </summary>
public class TaktMasterProductionScheduleLineService : TaktServiceBase, ITaktMasterProductionScheduleLineService
{
    private readonly ITaktCompanyRepository<TaktMasterProductionScheduleLine> _masterProductionScheduleLineRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="masterProductionScheduleLineRepository">主生产计划MPS行仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMasterProductionScheduleLineService(
        ITaktCompanyRepository<TaktMasterProductionScheduleLine> masterProductionScheduleLineRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _masterProductionScheduleLineRepository = masterProductionScheduleLineRepository;
        _uniqueValidator = uniqueValidator;
        _lineNumberGenerator = lineNumberGenerator;
    }

    /// <summary>
    /// 获取主生产计划MPS行列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMasterProductionScheduleLineDto>> GetMasterProductionScheduleLineListAsync(TaktMasterProductionScheduleLineQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _masterProductionScheduleLineRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMasterProductionScheduleLineDto>.Create(
            data.Adapt<List<TaktMasterProductionScheduleLineDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleLineDto?> GetMasterProductionScheduleLineByIdAsync(long id)
    {
        var entity = await _masterProductionScheduleLineRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMasterProductionScheduleLineDto>();
    }

    /// <summary>
    /// 获取主生产计划MPS行选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMasterProductionScheduleLineOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _masterProductionScheduleLineRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MpsCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MpsCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建主生产计划MPS行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleLineDto> CreateMasterProductionScheduleLineAsync(TaktMasterProductionScheduleLineCreateDto dto)
    {
        var entity = dto.Adapt<TaktMasterProductionScheduleLine>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_planning_mps_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
            _masterProductionScheduleLineRepository,
            x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                && x.MaterialCode == entity.MaterialCode
                && x.BucketStart == entity.BucketStart);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mps_line_bucket_unique)
        {
            throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、MaterialCode、BucketStart已存在");
        }
        await EnsureMasterProductionScheduleLineNumberAsync(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_mps_line_line_unique = await _uniqueValidator.IsUniqueAsync(
            _masterProductionScheduleLineRepository,
            x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mps_line_line_unique)
        {
            throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、LineNumber、MaterialCode已存在");
        }
        entity = await _masterProductionScheduleLineRepository.CreateAsync(entity);
        return await GetMasterProductionScheduleLineByIdAsync(entity.Id) ?? entity.Adapt<TaktMasterProductionScheduleLineDto>();
    }

    /// <summary>
    /// 更新主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMasterProductionScheduleLineDto> UpdateMasterProductionScheduleLineAsync(long id, TaktMasterProductionScheduleLineUpdateDto dto)
    {
        var entity = await _masterProductionScheduleLineRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("主生产计划MPS行不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_mps_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
            _masterProductionScheduleLineRepository,
            x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                && x.MaterialCode == entity.MaterialCode
                && x.BucketStart == entity.BucketStart,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mps_line_bucket_unique)
        {
            throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、MaterialCode、BucketStart已存在");
        }
        await EnsureMasterProductionScheduleLineNumberAsync(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_mps_line_line_unique = await _uniqueValidator.IsUniqueAsync(
            _masterProductionScheduleLineRepository,
            x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_mps_line_line_unique)
        {
            throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、LineNumber、MaterialCode已存在");
        }
        await _masterProductionScheduleLineRepository.UpdateAsync(entity);
        return await GetMasterProductionScheduleLineByIdAsync(id) ?? throw new TaktBusinessException("主生产计划MPS行不存在");
    }

    /// <summary>
    /// 删除主生产计划MPS行
    /// </summary>
    /// <param name="id">主生产计划MPS行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterProductionScheduleLineByIdAsync(long id)
    {
        var deleted = await _masterProductionScheduleLineRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("主生产计划MPS行不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除主生产计划MPS行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMasterProductionScheduleLineBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMasterProductionScheduleLineByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMasterProductionScheduleLineTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMasterProductionScheduleLineTemplateDto>(
            sheetName ?? "主生产计划MPS行导入模板",
            fileName ?? "主生产计划MPS行导入模板.xlsx");
    }

    /// <summary>
    /// 导入主生产计划MPS行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMasterProductionScheduleLineAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMasterProductionScheduleLineImportDto>(fileStream, sheetName ?? "主生产计划MPS行导入模板");
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
                var entity = rows[i].Adapt<TaktMasterProductionScheduleLine>();
                var importKey = $"{entity.MasterProductionScheduleId}|{entity.MaterialCode}|{entity.BucketStart}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MasterProductionScheduleId、MaterialCode、BucketStart）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_mps_line_bucket_unique = await _uniqueValidator.IsUniqueAsync(
                    _masterProductionScheduleLineRepository,
                    x => x.MasterProductionScheduleId == entity.MasterProductionScheduleId
                        && x.MaterialCode == entity.MaterialCode
                        && x.BucketStart == entity.BucketStart);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_mps_line_bucket_unique)
                {
                    throw new TaktBusinessException("主生产计划MPS行的MasterProductionScheduleId、MaterialCode、BucketStart已存在");
                }
                await _masterProductionScheduleLineRepository.CreateAsync(entity);
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
    /// 导出主生产计划MPS行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMasterProductionScheduleLineAsync(TaktMasterProductionScheduleLineQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMasterProductionScheduleLineQueryDto());
        var list = await _masterProductionScheduleLineRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMasterProductionScheduleLineExportDto>(),
                sheetName ?? "主生产计划MPS行数据",
                fileName ?? "主生产计划MPS行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMasterProductionScheduleLineExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "主生产计划MPS行数据",
            fileName ?? "主生产计划MPS行导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 未传行号时按主表编码生成下一行号
    /// </summary>
    /// <param name="entity">MPS 行实体</param>
    /// <returns>任务</returns>
    private async Task EnsureMasterProductionScheduleLineNumberAsync(TaktMasterProductionScheduleLine entity)
    {
        if (entity.LineNumber > 0)
        {
            return;
        }
        var maxLine = await _masterProductionScheduleLineRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.MasterProductionScheduleId == entity.MasterProductionScheduleId,
            x => x.LineNumber);
        var businessCode = !string.IsNullOrWhiteSpace(entity.MpsCode) ? entity.MpsCode : entity.MasterProductionScheduleId.ToString();
        entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
    }

    /// <summary>
    /// 构建主生产计划MPS行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMasterProductionScheduleLine, bool>> QueryExpression(TaktMasterProductionScheduleLineQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMasterProductionScheduleLine>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.MasterProductionScheduleId).Contains(keywords)
                || (x.MpsCode != null && x.MpsCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.MasterDemandScheduleLineId).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.GrossRequirement).Contains(keywords)
                || SqlFunc.ToString(x.ScheduledReceipts).Contains(keywords)
                || SqlFunc.ToString(x.ProjectedOnHand).Contains(keywords)
                || SqlFunc.ToString(x.NetRequirement).Contains(keywords)
                || SqlFunc.ToString(x.PlannedOrderQuantity).Contains(keywords)
                || SqlFunc.ToString(x.AtpQuantity).Contains(keywords)
                || (x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.IsObsolete).Contains(keywords)
                || SqlFunc.ToString(x.BucketStart).Contains(keywords)
                || SqlFunc.ToString(x.BucketEnd).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.MasterProductionScheduleId.HasValue == true)
        {
            exp = exp.And(x => x.MasterProductionScheduleId == queryDto.MasterProductionScheduleId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MpsCode))
        {
            exp = exp.And(x => x.MpsCode != null && x.MpsCode.Contains(queryDto.MpsCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.MasterDemandScheduleLineId.HasValue == true)
        {
            exp = exp.And(x => x.MasterDemandScheduleLineId == queryDto.MasterDemandScheduleLineId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (queryDto?.GrossRequirement.HasValue == true)
        {
            exp = exp.And(x => x.GrossRequirement == queryDto.GrossRequirement);
        }

        if (queryDto?.ScheduledReceipts.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledReceipts == queryDto.ScheduledReceipts);
        }

        if (queryDto?.ProjectedOnHand.HasValue == true)
        {
            exp = exp.And(x => x.ProjectedOnHand == queryDto.ProjectedOnHand);
        }

        if (queryDto?.NetRequirement.HasValue == true)
        {
            exp = exp.And(x => x.NetRequirement == queryDto.NetRequirement);
        }

        if (queryDto?.PlannedOrderQuantity.HasValue == true)
        {
            exp = exp.And(x => x.PlannedOrderQuantity == queryDto.PlannedOrderQuantity);
        }

        if (queryDto?.AtpQuantity.HasValue == true)
        {
            exp = exp.And(x => x.AtpQuantity == queryDto.AtpQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.UnitOfMeasure))
        {
            exp = exp.And(x => x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(queryDto.UnitOfMeasure));
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

        return exp.ToExpression();
    }
}
