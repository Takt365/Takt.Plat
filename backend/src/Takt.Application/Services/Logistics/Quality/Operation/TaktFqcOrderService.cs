// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：出货检验单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 出货检验单应用服务
/// </summary>
public class TaktFqcOrderService : TaktServiceBase, ITaktFqcOrderService
{
    private readonly ITaktCompanyRepository<TaktFqcOrder> _fqcOrderRepository;
    private readonly ITaktCompanyRepository<TaktFqcOrderItem> _fqcOrderItemRepository;
    private readonly ITaktCompanyRepository<TaktFqcOrderChangeLog> _fqcOrderChangeLogRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fqcOrderRepository">出货检验单仓储</param>
    /// <param name="fqcOrderItemRepository">FqcOrderItem仓储</param>
    /// <param name="fqcOrderChangeLogRepository">FqcOrderChangeLog仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFqcOrderService(
        ITaktCompanyRepository<TaktFqcOrder> fqcOrderRepository,
        ITaktCompanyRepository<TaktFqcOrderItem> fqcOrderItemRepository,
        ITaktCompanyRepository<TaktFqcOrderChangeLog> fqcOrderChangeLogRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _fqcOrderRepository = fqcOrderRepository;
        _fqcOrderItemRepository = fqcOrderItemRepository;
        _fqcOrderChangeLogRepository = fqcOrderChangeLogRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取出货检验单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFqcOrderDto>> GetFqcOrderListAsync(TaktFqcOrderQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _fqcOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFqcOrderDto>.Create(
            data.Adapt<List<TaktFqcOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取出货检验单
    /// </summary>
    /// <param name="id">出货检验单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcOrderDto?> GetFqcOrderByIdAsync(long id)
    {
        var entity = await _fqcOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktFqcOrderDto>();
        await FillFqcOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取出货检验单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFqcOrderOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _fqcOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建出货检验单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcOrderDto> CreateFqcOrderAsync(TaktFqcOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktFqcOrder>();
        var isUnique_ix_takt_logistics_quality_fqc_order_fqc_order_unique = await _uniqueValidator.IsUniqueAsync(
            _fqcOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.FqcOrderCode == entity.FqcOrderCode);
        if (!isUnique_ix_takt_logistics_quality_fqc_order_fqc_order_unique)
        {
            throw new TaktBusinessException("出货检验单的PlantCode、FqcOrderCode已存在");
        }
        entity = await _fqcOrderRepository.CreateAsync(entity);
                await SaveFqcOrderChildrenAsync(entity, dto);
        return await GetFqcOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktFqcOrderDto>();
    }

    /// <summary>
    /// 更新出货检验单
    /// </summary>
    /// <param name="id">出货检验单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcOrderDto> UpdateFqcOrderAsync(long id, TaktFqcOrderUpdateDto dto)
    {
        var entity = await _fqcOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("出货检验单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_fqc_order_fqc_order_unique = await _uniqueValidator.IsUniqueAsync(
            _fqcOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.FqcOrderCode == entity.FqcOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_fqc_order_fqc_order_unique)
        {
            throw new TaktBusinessException("出货检验单的PlantCode、FqcOrderCode已存在");
        }
        await _fqcOrderRepository.UpdateAsync(entity);
                await SaveFqcOrderChildrenAsync(entity, dto);
        return await GetFqcOrderByIdAsync(id) ?? throw new TaktBusinessException("出货检验单不存在");
    }

    /// <summary>
    /// 删除出货检验单
    /// </summary>
    /// <param name="id">出货检验单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFqcOrderByIdAsync(long id)
    {
        var entity = await _fqcOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("出货检验单不存在或已删除");
        }
        await _fqcOrderItemRepository.DeleteAsync(x => x.FqcOrderId == entity.Id);
        await _fqcOrderChangeLogRepository.DeleteAsync(x => x.FqcOrderId == entity.Id);
        var deleted = await _fqcOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("出货检验单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除出货检验单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFqcOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFqcOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新出货检验单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcOrderDto> UpdateFqcOrderStatusAsync(TaktFqcOrderStatusDto dto)
    {
        var entity = await _fqcOrderRepository.GetByIdAsync(dto.FqcOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("出货检验单不存在");
        }
        entity.JudgeStatus = dto.JudgeStatus;
        await _fqcOrderRepository.UpdateAsync(entity);
        return await GetFqcOrderByIdAsync(dto.FqcOrderId) ?? throw new TaktBusinessException("出货检验单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFqcOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFqcOrderTemplateDto>(
            sheetName ?? "出货检验单导入模板",
            fileName ?? "出货检验单导入模板.xlsx");
    }

    /// <summary>
    /// 导入出货检验单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFqcOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFqcOrderImportDto>(fileStream, sheetName ?? "出货检验单导入模板");
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
                var entity = rows[i].Adapt<TaktFqcOrder>();
                var importKey = $"{entity.PlantCode}|{entity.FqcOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、FqcOrderCode）");
                }
                var isUnique_ix_takt_logistics_quality_fqc_order_fqc_order_unique = await _uniqueValidator.IsUniqueAsync(
                    _fqcOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.FqcOrderCode == entity.FqcOrderCode);
                if (!isUnique_ix_takt_logistics_quality_fqc_order_fqc_order_unique)
                {
                    throw new TaktBusinessException("出货检验单的PlantCode、FqcOrderCode已存在");
                }
                await _fqcOrderRepository.CreateAsync(entity);
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
    /// 导出出货检验单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFqcOrderAsync(TaktFqcOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFqcOrderQueryDto());
        var list = await _fqcOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFqcOrderExportDto>(),
                sheetName ?? "出货检验单数据",
                fileName ?? "出货检验单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFqcOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "出货检验单数据",
            fileName ?? "出货检验单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充出货检验单详情（加载 OneToMany 子表：出货检验单明细、出货检验单变更日志）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillFqcOrderDetailsAsync(TaktFqcOrderDto dto, TaktFqcOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // 出货检验单明细 → dto.Items
        var items = await _fqcOrderItemRepository.GetListAsync(x => x.FqcOrderId == entity.Id);
        dto.Items = items.Adapt<List<TaktFqcOrderItemDto>>();
        // 出货检验单变更日志 → dto.ChangeLogs
        var changelogs = await _fqcOrderChangeLogRepository.GetListAsync(x => x.FqcOrderId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktFqcOrderChangeLogDto>>();
    }

    /// <summary>
    /// 保存出货检验单子表级联（出货检验单明细、出货检验单变更日志；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveFqcOrderChildrenAsync(TaktFqcOrder entity, TaktFqcOrderCreateDto dto)
    {
        // 出货检验单明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _fqcOrderItemRepository.DeleteAsync(x => x.FqcOrderId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktFqcOrderItem>>();
            foreach (var child in items)
            {
                child.FqcOrderId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.FqcOrderCode) ? entity.FqcOrderCode : entity.Id.ToString();
                var maxLine = await _fqcOrderItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FqcOrderId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, itemsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in items)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < items.Count; i++)
                        {
                            var key = $"{items[i].CompanyCode}|{items[i].FqcOrderId}|{items[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"出货检验单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、FqcOrderId、LineNumber）");
                            }
                        }
            await _fqcOrderItemRepository.DeleteAsync(x => x.FqcOrderId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_quality_fqc_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                _fqcOrderItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.FqcOrderId == child.FqcOrderId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_fqc_order_item_order_line_unique)
            {
                throw new TaktBusinessException("出货检验单明细的CompanyCode、FqcOrderId、LineNumber已存在");
            }
            }
            await _fqcOrderItemRepository.CreateRangeAsync(items);
        }
        // 出货检验单变更日志（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _fqcOrderChangeLogRepository.DeleteAsync(x => x.FqcOrderId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktFqcOrderChangeLog>>();
            foreach (var child in changelogs)
            {
                child.FqcOrderId = entity.Id;
            }
            await _fqcOrderChangeLogRepository.DeleteAsync(x => x.FqcOrderId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _fqcOrderChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建出货检验单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFqcOrder, bool>> QueryExpression(TaktFqcOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFqcOrder>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SourceCode != null && x.SourceCode.Contains(keywords))
                || (x.FqcOrderCode != null && x.FqcOrderCode.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || SqlFunc.ToString(x.TotalWarehouseQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalSampleQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalQualifiedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalUnqualifiedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalInspectionReturnQuantity).Contains(keywords)
                || SqlFunc.ToString(x.JudgeStatus).Contains(keywords)
                || (x.JudgeBy != null && x.JudgeBy.Contains(keywords))
                || (x.JudgeDescription != null && x.JudgeDescription.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.InspectionDate).Contains(keywords)
                || SqlFunc.ToString(x.JudgeDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceCode))
        {
            exp = exp.And(x => x.SourceCode != null && x.SourceCode.Contains(queryDto.SourceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.FqcOrderCode))
        {
            exp = exp.And(x => x.FqcOrderCode != null && x.FqcOrderCode.Contains(queryDto.FqcOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerCode))
        {
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(queryDto.CustomerCode));
        }

        if (queryDto?.TotalWarehouseQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalWarehouseQuantity == queryDto.TotalWarehouseQuantity);
        }

        if (queryDto?.TotalSampleQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalSampleQuantity == queryDto.TotalSampleQuantity);
        }

        if (queryDto?.TotalQualifiedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalQualifiedQuantity == queryDto.TotalQualifiedQuantity);
        }

        if (queryDto?.TotalUnqualifiedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalUnqualifiedQuantity == queryDto.TotalUnqualifiedQuantity);
        }

        if (queryDto?.TotalInspectionReturnQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalInspectionReturnQuantity == queryDto.TotalInspectionReturnQuantity);
        }

        if (queryDto?.JudgeStatus.HasValue == true)
        {
            exp = exp.And(x => x.JudgeStatus == queryDto.JudgeStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.JudgeBy))
        {
            exp = exp.And(x => x.JudgeBy != null && x.JudgeBy.Contains(queryDto.JudgeBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.JudgeDescription))
        {
            exp = exp.And(x => x.JudgeDescription != null && x.JudgeDescription.Contains(queryDto.JudgeDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.InspectionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InspectionDate >= queryDto.InspectionDateStart);
        }

        if (queryDto?.InspectionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InspectionDate <= queryDto.InspectionDateEnd);
        }

        if (queryDto?.JudgeDateStart.HasValue == true)
        {
            exp = exp.And(x => x.JudgeDate >= queryDto.JudgeDateStart);
        }

        if (queryDto?.JudgeDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.JudgeDate <= queryDto.JudgeDateEnd);
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
