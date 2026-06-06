// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsScheduleItemService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Scheduling;
using Takt.Domain.Entities.Logistics.Manufacturing.Scheduling;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Scheduling;

/// <summary>
/// APS排程明细应用服务
/// </summary>
public class TaktApsScheduleItemService : TaktServiceBase, ITaktApsScheduleItemService
{
    private readonly ITaktCompanyRepository<TaktApsScheduleItem> _apsScheduleItemRepository;
    private readonly ITaktCompanyRepository<TaktApsSchedule> _apsScheduleRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsScheduleItemRepository">APS排程明细仓储</param>
    /// <param name="apsScheduleRepository">APS排程主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktApsScheduleItemService(
        ITaktCompanyRepository<TaktApsScheduleItem> apsScheduleItemRepository,
        ITaktCompanyRepository<TaktApsSchedule> apsScheduleRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _apsScheduleItemRepository = apsScheduleItemRepository;
        _apsScheduleRepository = apsScheduleRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取APS排程明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktApsScheduleItemDto>> GetApsScheduleItemListAsync(TaktApsScheduleItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _apsScheduleItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktApsScheduleItemDto>.Create(
            data.Adapt<List<TaktApsScheduleItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取APS排程明细
    /// </summary>
    /// <param name="id">APS排程明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleItemDto?> GetApsScheduleItemByIdAsync(long id)
    {
        var entity = await _apsScheduleItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktApsScheduleItemDto>();
    }

    /// <summary>
    /// 获取APS排程明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetApsScheduleItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _apsScheduleItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProductName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ProductName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建APS排程明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleItemDto> CreateApsScheduleItemAsync(TaktApsScheduleItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktApsScheduleItem>();
        await StampApsScheduleItemApsScheduleAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _apsScheduleItemRepository,
            x => x.ApsScheduleId == entity.ApsScheduleId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_item_line_unique)
        {
            throw new TaktBusinessException("APS排程明细的ApsScheduleId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _apsScheduleItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ApsScheduleId == entity.ApsScheduleId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.ApsScheduleCode) ? entity.ApsScheduleCode : entity.ApsScheduleId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _apsScheduleItemRepository.CreateAsync(entity);
        return await GetApsScheduleItemByIdAsync(entity.Id) ?? entity.Adapt<TaktApsScheduleItemDto>();
    }

    /// <summary>
    /// 更新APS排程明细
    /// </summary>
    /// <param name="id">APS排程明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleItemDto> UpdateApsScheduleItemAsync(long id, TaktApsScheduleItemUpdateDto dto)
    {
        var entity = await _apsScheduleItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程明细不存在");
        }
        dto.Adapt(entity);
        await StampApsScheduleItemApsScheduleAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _apsScheduleItemRepository,
            x => x.ApsScheduleId == entity.ApsScheduleId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_item_line_unique)
        {
            throw new TaktBusinessException("APS排程明细的ApsScheduleId、LineNumber已存在");
        }
        await _apsScheduleItemRepository.UpdateAsync(entity);
        return await GetApsScheduleItemByIdAsync(id) ?? throw new TaktBusinessException("APS排程明细不存在");
    }

    /// <summary>
    /// 删除APS排程明细
    /// </summary>
    /// <param name="id">APS排程明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteApsScheduleItemByIdAsync(long id)
    {
        var deleted = await _apsScheduleItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("APS排程明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除APS排程明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteApsScheduleItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteApsScheduleItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新APS排程明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleItemDto> UpdateApsScheduleItemStatusAsync(TaktApsScheduleItemStatusDto dto)
    {
        var entity = await _apsScheduleItemRepository.GetByIdAsync(dto.ApsScheduleItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程明细不存在");
        }
        entity.ProcessStatus = dto.ProcessStatus;
        await _apsScheduleItemRepository.UpdateAsync(entity);
        return await GetApsScheduleItemByIdAsync(dto.ApsScheduleItemId) ?? throw new TaktBusinessException("APS排程明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetApsScheduleItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktApsScheduleItemTemplateDto>(
            sheetName ?? "APS排程明细导入模板",
            fileName ?? "APS排程明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入APS排程明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportApsScheduleItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktApsScheduleItemImportDto>(fileStream, sheetName ?? "APS排程明细导入模板");
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
                var entity = rows[i].Adapt<TaktApsScheduleItem>();
                var importDto = rows[i].Adapt<TaktApsScheduleItemCreateDto>();
                await StampApsScheduleItemApsScheduleAsync(entity, importDto);
                var importKey = $"{entity.ApsScheduleId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ApsScheduleId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_scheduling_aps_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _apsScheduleItemRepository,
                    x => x.ApsScheduleId == entity.ApsScheduleId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_scheduling_aps_item_line_unique)
                {
                    throw new TaktBusinessException("APS排程明细的ApsScheduleId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _apsScheduleItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ApsScheduleId == entity.ApsScheduleId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ApsScheduleCode) ? entity.ApsScheduleCode : entity.ApsScheduleId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _apsScheduleItemRepository.CreateAsync(entity);
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
    /// 导出APS排程明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportApsScheduleItemAsync(TaktApsScheduleItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktApsScheduleItemQueryDto());
        var list = await _apsScheduleItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsScheduleItemExportDto>(),
                sheetName ?? "APS排程明细数据",
                fileName ?? "APS排程明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktApsScheduleItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "APS排程明细数据",
            fileName ?? "APS排程明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步APS排程明细主表外键（ManyToOne → APS排程主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampApsScheduleItemApsScheduleAsync(TaktApsScheduleItem entity, TaktApsScheduleItemCreateDto dto)
    {
        if (dto.ApsScheduleId <= 0)
        {
            return;
        }
        var master = await _apsScheduleRepository.GetByIdAsync(dto.ApsScheduleId);
        if (master == null)
        {
            throw new TaktBusinessException("APS排程主不存在");
        }
        entity.ApsScheduleId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建APS排程明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktApsScheduleItem, bool>> QueryExpression(TaktApsScheduleItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktApsScheduleItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ApsScheduleId).Contains(keywords)
                || (x.ApsScheduleCode != null && x.ApsScheduleCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.WorkOrderCode != null && x.WorkOrderCode.Contains(keywords))
                || (x.ProductCode != null && x.ProductCode.Contains(keywords))
                || (x.ProductName != null && x.ProductName.Contains(keywords))
                || (x.WorkCenterCode != null && x.WorkCenterCode.Contains(keywords))
                || (x.WorkCenterName != null && x.WorkCenterName.Contains(keywords))
                || (x.ProcessCode != null && x.ProcessCode.Contains(keywords))
                || (x.ProcessName != null && x.ProcessName.Contains(keywords))
                || SqlFunc.ToString(x.ProcessSequence).Contains(keywords)
                || SqlFunc.ToString(x.ProcessStandardST).Contains(keywords)
                || SqlFunc.ToString(x.ProcessStandardSTUnit).Contains(keywords)
                || SqlFunc.ToString(x.ExtraMinutes).Contains(keywords)
                || SqlFunc.ToString(x.PlanQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ProcessStatus).Contains(keywords)
                || SqlFunc.ToString(x.Priority).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlanStartTime).Contains(keywords)
                || SqlFunc.ToString(x.PlanEndTime).Contains(keywords)
                || SqlFunc.ToString(x.ActualStartTime).Contains(keywords)
                || SqlFunc.ToString(x.ActualEndTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ApsScheduleId.HasValue == true)
        {
            exp = exp.And(x => x.ApsScheduleId == queryDto.ApsScheduleId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ApsScheduleCode))
        {
            exp = exp.And(x => x.ApsScheduleCode != null && x.ApsScheduleCode.Contains(queryDto.ApsScheduleCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkOrderCode))
        {
            exp = exp.And(x => x.WorkOrderCode != null && x.WorkOrderCode.Contains(queryDto.WorkOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductCode))
        {
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(queryDto.ProductCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductName))
        {
            exp = exp.And(x => x.ProductName != null && x.ProductName.Contains(queryDto.ProductName));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenterCode))
        {
            exp = exp.And(x => x.WorkCenterCode != null && x.WorkCenterCode.Contains(queryDto.WorkCenterCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenterName))
        {
            exp = exp.And(x => x.WorkCenterName != null && x.WorkCenterName.Contains(queryDto.WorkCenterName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessCode))
        {
            exp = exp.And(x => x.ProcessCode != null && x.ProcessCode.Contains(queryDto.ProcessCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessName))
        {
            exp = exp.And(x => x.ProcessName != null && x.ProcessName.Contains(queryDto.ProcessName));
        }

        if (queryDto?.ProcessSequence.HasValue == true)
        {
            exp = exp.And(x => x.ProcessSequence == queryDto.ProcessSequence);
        }

        if (queryDto?.ProcessStandardST.HasValue == true)
        {
            exp = exp.And(x => x.ProcessStandardST == queryDto.ProcessStandardST);
        }

        if (queryDto?.ProcessStandardSTUnit.HasValue == true)
        {
            exp = exp.And(x => x.ProcessStandardSTUnit == queryDto.ProcessStandardSTUnit);
        }

        if (queryDto?.ExtraMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ExtraMinutes == queryDto.ExtraMinutes);
        }

        if (queryDto?.PlanQuantity.HasValue == true)
        {
            exp = exp.And(x => x.PlanQuantity == queryDto.PlanQuantity);
        }

        if (queryDto?.ProcessStatus.HasValue == true)
        {
            exp = exp.And(x => x.ProcessStatus == queryDto.ProcessStatus);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            exp = exp.And(x => x.Priority == queryDto.Priority);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PlanStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanStartTime >= queryDto.PlanStartTimeStart);
        }

        if (queryDto?.PlanStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanStartTime <= queryDto.PlanStartTimeEnd);
        }

        if (queryDto?.PlanEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanEndTime >= queryDto.PlanEndTimeStart);
        }

        if (queryDto?.PlanEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanEndTime <= queryDto.PlanEndTimeEnd);
        }

        if (queryDto?.ActualStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualStartTime >= queryDto.ActualStartTimeStart);
        }

        if (queryDto?.ActualStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualStartTime <= queryDto.ActualStartTimeEnd);
        }

        if (queryDto?.ActualEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualEndTime >= queryDto.ActualEndTimeStart);
        }

        if (queryDto?.ActualEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualEndTime <= queryDto.ActualEndTimeEnd);
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
