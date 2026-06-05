// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktPurchaseRequestService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：采购申请应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 采购申请应用服务
/// </summary>
public class TaktPurchaseRequestService : TaktServiceBase, ITaktPurchaseRequestService
{
    private readonly ITaktApprovalRepository<TaktPurchaseRequest> _purchaseRequestRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseRequestItem> _purchaseRequestItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseRequestChangeLog> _purchaseRequestChangeLogRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseRequestRepository">采购申请仓储</param>
    /// <param name="purchaseRequestItemRepository">PurchaseRequestItem仓储</param>
    /// <param name="purchaseRequestChangeLogRepository">PurchaseRequestChangeLog仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseRequestService(
        ITaktApprovalRepository<TaktPurchaseRequest> purchaseRequestRepository,
        ITaktCompanyRepository<TaktPurchaseRequestItem> purchaseRequestItemRepository,
        ITaktCompanyRepository<TaktPurchaseRequestChangeLog> purchaseRequestChangeLogRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseRequestRepository = purchaseRequestRepository;
        _purchaseRequestItemRepository = purchaseRequestItemRepository;
        _purchaseRequestChangeLogRepository = purchaseRequestChangeLogRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购申请列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseRequestDto>> GetPurchaseRequestListAsync(TaktPurchaseRequestQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseRequestRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseRequestDto>.Create(
            data.Adapt<List<TaktPurchaseRequestDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购申请
    /// </summary>
    /// <param name="id">采购申请ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestDto?> GetPurchaseRequestByIdAsync(long id)
    {
        var entity = await _purchaseRequestRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseRequestDto>();
        await FillPurchaseRequestDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购申请选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseRequestOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _purchaseRequestRepository.GetListAsync(
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
    /// 创建采购申请
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestDto> CreatePurchaseRequestAsync(TaktPurchaseRequestCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseRequest>();
        var isUnique_ix_takt_logistics_materials_purchase_request_pr_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseRequestRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseRequestCode == entity.PurchaseRequestCode
                && x.RequestDate == entity.RequestDate);
        if (!isUnique_ix_takt_logistics_materials_purchase_request_pr_unique)
        {
            throw new TaktBusinessException("采购申请的PlantCode、PurchaseRequestCode、RequestDate已存在");
        }
        entity = await _purchaseRequestRepository.CreateAsync(entity);
                await SavePurchaseRequestChildrenAsync(entity, dto);
        return await GetPurchaseRequestByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseRequestDto>();
    }

    /// <summary>
    /// 更新采购申请
    /// </summary>
    /// <param name="id">采购申请ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestDto> UpdatePurchaseRequestAsync(long id, TaktPurchaseRequestUpdateDto dto)
    {
        var entity = await _purchaseRequestRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购申请不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_purchase_request_pr_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseRequestRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseRequestCode == entity.PurchaseRequestCode
                && x.RequestDate == entity.RequestDate,
            id);
        if (!isUnique_ix_takt_logistics_materials_purchase_request_pr_unique)
        {
            throw new TaktBusinessException("采购申请的PlantCode、PurchaseRequestCode、RequestDate已存在");
        }
        await _purchaseRequestRepository.UpdateAsync(entity);
                await SavePurchaseRequestChildrenAsync(entity, dto);
        return await GetPurchaseRequestByIdAsync(id) ?? throw new TaktBusinessException("采购申请不存在");
    }

    /// <summary>
    /// 删除采购申请
    /// </summary>
    /// <param name="id">采购申请ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseRequestByIdAsync(long id)
    {
        var entity = await _purchaseRequestRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购申请不存在或已删除");
        }
        await _purchaseRequestItemRepository.DeleteAsync(x => x.PurchaseRequestId == entity.Id);
        await _purchaseRequestChangeLogRepository.DeleteAsync(x => x.PurchaseRequestId == entity.Id);
        var deleted = await _purchaseRequestRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购申请不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购申请
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseRequestBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseRequestByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购申请状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestDto> UpdatePurchaseRequestStatusAsync(TaktPurchaseRequestStatusDto dto)
    {
        var entity = await _purchaseRequestRepository.GetByIdAsync(dto.PurchaseRequestId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购申请不存在");
        }
        entity.RequestStatus = dto.RequestStatus;
        await _purchaseRequestRepository.UpdateAsync(entity);
        return await GetPurchaseRequestByIdAsync(dto.PurchaseRequestId) ?? throw new TaktBusinessException("采购申请不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseRequestTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseRequestTemplateDto>(
            sheetName ?? "采购申请导入模板",
            fileName ?? "采购申请导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购申请
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseRequestAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseRequestImportDto>(fileStream, sheetName ?? "采购申请导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseRequest>();
                var importKey = $"{entity.PlantCode}|{entity.PurchaseRequestCode}|{entity.RequestDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseRequestCode、RequestDate）");
                }
                var isUnique_ix_takt_logistics_materials_purchase_request_pr_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseRequestRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseRequestCode == entity.PurchaseRequestCode
                        && x.RequestDate == entity.RequestDate);
                if (!isUnique_ix_takt_logistics_materials_purchase_request_pr_unique)
                {
                    throw new TaktBusinessException("采购申请的PlantCode、PurchaseRequestCode、RequestDate已存在");
                }
                await _purchaseRequestRepository.CreateAsync(entity);
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
    /// 导出采购申请
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseRequestAsync(TaktPurchaseRequestQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPurchaseRequestQueryDto());
        var list = await _purchaseRequestRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseRequestExportDto>(),
                sheetName ?? "采购申请数据",
                fileName ?? "采购申请导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseRequestExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购申请数据",
            fileName ?? "采购申请导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充采购申请详情（加载 OneToMany 子表：采购申请明细、采购申请变更记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseRequestDetailsAsync(TaktPurchaseRequestDto dto, TaktPurchaseRequest entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购申请明细 → dto.Items
        var items = await _purchaseRequestItemRepository.GetListAsync(x => x.PurchaseRequestId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseRequestItemDto>>();
        // 采购申请变更记录 → dto.ChangeLogs
        var changelogs = await _purchaseRequestChangeLogRepository.GetListAsync(x => x.PurchaseRequestId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktPurchaseRequestChangeLogDto>>();
    }

    /// <summary>
    /// 保存采购申请子表级联（采购申请明细、采购申请变更记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseRequestChildrenAsync(TaktPurchaseRequest entity, TaktPurchaseRequestCreateDto dto)
    {
        // 采购申请明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _purchaseRequestItemRepository.DeleteAsync(x => x.PurchaseRequestId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktPurchaseRequestItem>>();
            foreach (var child in items)
            {
                child.PurchaseRequestId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseRequestCode) ? entity.PurchaseRequestCode : entity.Id.ToString();
                var maxLine = await _purchaseRequestItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseRequestId == entity.Id,
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
                            var key = $"{items[i].CompanyCode}|{items[i].PurchaseRequestId}|{items[i].LineNumber}|{items[i].MaterialCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"采购申请明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PurchaseRequestId、LineNumber、MaterialCode）");
                            }
                        }
            await _purchaseRequestItemRepository.DeleteAsync(x => x.PurchaseRequestId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_materials_purchase_request_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
                _purchaseRequestItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.PurchaseRequestId == child.PurchaseRequestId
                    && x.LineNumber == child.LineNumber
                    && x.MaterialCode == child.MaterialCode);
            if (!isUnique_ix_takt_logistics_materials_purchase_request_item_request_line_unique)
            {
                throw new TaktBusinessException("采购申请明细的CompanyCode、PurchaseRequestId、LineNumber、MaterialCode已存在");
            }
            }
            await _purchaseRequestItemRepository.CreateRangeAsync(items);
        }
        // 采购申请变更记录（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _purchaseRequestChangeLogRepository.DeleteAsync(x => x.PurchaseRequestId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktPurchaseRequestChangeLog>>();
            foreach (var child in changelogs)
            {
                child.PurchaseRequestId = entity.Id;
            }
            await _purchaseRequestChangeLogRepository.DeleteAsync(x => x.PurchaseRequestId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _purchaseRequestChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购申请查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseRequest, bool>> QueryExpression(TaktPurchaseRequestQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseRequest>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(keywords))
                || SqlFunc.ToString(x.RequestId).Contains(keywords)
                || (x.RequestBy != null && x.RequestBy.Contains(keywords))
                || SqlFunc.ToString(x.TotalQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalAmount).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedAmount).Contains(keywords)
                || SqlFunc.ToString(x.RequestStatus).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedStatus).Contains(keywords)
                || SqlFunc.ToString(x.FlowInstanceId).Contains(keywords)
                || (x.RequestReason != null && x.RequestReason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.RequestDate).Contains(keywords)
                || SqlFunc.ToString(x.RequiredArrivalDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseRequestCode))
        {
            exp = exp.And(x => x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(queryDto.PurchaseRequestCode));
        }

        if (queryDto?.RequestId.HasValue == true)
        {
            exp = exp.And(x => x.RequestId == queryDto.RequestId);
        }

        if (!string.IsNullOrEmpty(queryDto?.RequestBy))
        {
            exp = exp.And(x => x.RequestBy != null && x.RequestBy.Contains(queryDto.RequestBy));
        }

        if (queryDto?.TotalQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalQuantity == queryDto.TotalQuantity);
        }

        if (queryDto?.TotalAmount.HasValue == true)
        {
            exp = exp.And(x => x.TotalAmount == queryDto.TotalAmount);
        }

        if (queryDto?.ConvertedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.ConvertedQuantity == queryDto.ConvertedQuantity);
        }

        if (queryDto?.ConvertedAmount.HasValue == true)
        {
            exp = exp.And(x => x.ConvertedAmount == queryDto.ConvertedAmount);
        }

        if (queryDto?.RequestStatus.HasValue == true)
        {
            exp = exp.And(x => x.RequestStatus == queryDto.RequestStatus);
        }

        if (queryDto?.ConvertedStatus.HasValue == true)
        {
            exp = exp.And(x => x.ConvertedStatus == queryDto.ConvertedStatus);
        }

        if (queryDto?.FlowInstanceId.HasValue == true)
        {
            exp = exp.And(x => x.FlowInstanceId == queryDto.FlowInstanceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.RequestReason))
        {
            exp = exp.And(x => x.RequestReason != null && x.RequestReason.Contains(queryDto.RequestReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.RequestDateStart.HasValue == true)
        {
            exp = exp.And(x => x.RequestDate >= queryDto.RequestDateStart);
        }

        if (queryDto?.RequestDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.RequestDate <= queryDto.RequestDateEnd);
        }

        if (queryDto?.RequiredArrivalDateStart.HasValue == true)
        {
            exp = exp.And(x => x.RequiredArrivalDate >= queryDto.RequiredArrivalDateStart);
        }

        if (queryDto?.RequiredArrivalDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.RequiredArrivalDate <= queryDto.RequiredArrivalDateEnd);
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
