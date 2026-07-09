// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Planning
// 文件名称：TaktSalesPlanService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：销售计划应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Planning;
using Takt.Domain.Entities.Logistics.Manufacturing.Planning;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Planning;

/// <summary>
/// 销售计划应用服务
/// </summary>
public class TaktSalesPlanService : TaktServiceBase, ITaktSalesPlanService
{
    private readonly ITaktApprovalRepository<TaktSalesPlan> _salesPlanRepository;
    private readonly ITaktCompanyRepository<TaktSalesPlanItem> _salesPlanItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPlanRepository">销售计划仓储</param>
    /// <param name="salesPlanItemRepository">SalesPlanItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPlanService(
        ITaktApprovalRepository<TaktSalesPlan> salesPlanRepository,
        ITaktCompanyRepository<TaktSalesPlanItem> salesPlanItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesPlanRepository = salesPlanRepository;
        _salesPlanItemRepository = salesPlanItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售计划列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesPlanDto>> GetSalesPlanListAsync(TaktSalesPlanQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesPlanRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesPlanDto>.Create(
            data.Adapt<List<TaktSalesPlanDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售计划
    /// </summary>
    /// <param name="id">销售计划ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPlanDto?> GetSalesPlanByIdAsync(long id)
    {
        var entity = await _salesPlanRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesPlanDto>();
        await FillSalesPlanDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售计划选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPlanOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _salesPlanRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PlanStatus == 1,
            x => x.CustomerName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CustomerName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建销售计划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPlanDto> CreateSalesPlanAsync(TaktSalesPlanCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesPlan>();
        var isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPlanRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesPlanCode == entity.SalesPlanCode
                && x.PlanDate == entity.PlanDate);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_unique)
        {
            throw new TaktBusinessException("销售计划的PlantCode、SalesPlanCode、PlanDate已存在");
        }
        entity = await _salesPlanRepository.CreateAsync(entity);
                await SaveSalesPlanChildrenAsync(entity, dto);
        return await GetSalesPlanByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesPlanDto>();
    }

    /// <summary>
    /// 更新销售计划
    /// </summary>
    /// <param name="id">销售计划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPlanDto> UpdateSalesPlanAsync(long id, TaktSalesPlanUpdateDto dto)
    {
        var entity = await _salesPlanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售计划不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_unique = await _uniqueValidator.IsUniqueAsync(
            _salesPlanRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesPlanCode == entity.SalesPlanCode
                && x.PlanDate == entity.PlanDate,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_unique)
        {
            throw new TaktBusinessException("销售计划的PlantCode、SalesPlanCode、PlanDate已存在");
        }
        await _salesPlanRepository.UpdateAsync(entity);
                await SaveSalesPlanChildrenAsync(entity, dto);
        return await GetSalesPlanByIdAsync(id) ?? throw new TaktBusinessException("销售计划不存在");
    }

    /// <summary>
    /// 删除销售计划
    /// </summary>
    /// <param name="id">销售计划ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPlanByIdAsync(long id)
    {
        var entity = await _salesPlanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售计划不存在或已删除");
        }
        await _salesPlanItemRepository.DeleteAsync(x => x.SalesPlanId == entity.Id);
        var deleted = await _salesPlanRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售计划不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售计划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesPlanBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesPlanByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售计划状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesPlanDto> UpdateSalesPlanStatusAsync(TaktSalesPlanStatusDto dto)
    {
        var entity = await _salesPlanRepository.GetByIdAsync(dto.SalesPlanId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售计划不存在");
        }
        entity.PlanStatus = dto.PlanStatus;
        await _salesPlanRepository.UpdateAsync(entity);
        return await GetSalesPlanByIdAsync(dto.SalesPlanId) ?? throw new TaktBusinessException("销售计划不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesPlanTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesPlanTemplateDto>(
            sheetName ?? "销售计划导入模板",
            fileName ?? "销售计划导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售计划
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesPlanAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesPlanImportDto>(fileStream, sheetName ?? "销售计划导入模板");
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
                var entity = rows[i].Adapt<TaktSalesPlan>();
                var importKey = $"{entity.PlantCode}|{entity.SalesPlanCode}|{entity.PlanDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesPlanCode、PlanDate）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesPlanRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesPlanCode == entity.SalesPlanCode
                        && x.PlanDate == entity.PlanDate);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_sales_plan_unique)
                {
                    throw new TaktBusinessException("销售计划的PlantCode、SalesPlanCode、PlanDate已存在");
                }
                await _salesPlanRepository.CreateAsync(entity);
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
    /// 导出销售计划
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPlanAsync(TaktSalesPlanQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalesPlanQueryDto());
        var list = await _salesPlanRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesPlanExportDto>(),
                sheetName ?? "销售计划数据",
                fileName ?? "销售计划导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesPlanExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售计划数据",
            fileName ?? "销售计划导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售计划明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesPlanId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesPlanItemsObsoleteAsync(long salesPlanId)
    {
        if (salesPlanId <= 0)
        {
            return;
        }
        var rows = await _salesPlanItemRepository.GetListAsync(
            x => x.SalesPlanId == salesPlanId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesPlanItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售计划详情（加载 OneToMany 子表：销售计划明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesPlanDetailsAsync(TaktSalesPlanDto dto, TaktSalesPlan entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售计划明细 → dto.Items（含作废行）
        var items = await _salesPlanItemRepository.GetListAsync(x => x.SalesPlanId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesPlanItemDto>>();
    }

    /// <summary>
    /// 保存销售计划子表级联（销售计划明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesPlanChildrenAsync(TaktSalesPlan entity, TaktSalesPlanCreateDto dto)
    {
        // 销售计划明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await MarkSalesPlanItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _salesPlanItemRepository.GetListAsync(x => x.SalesPlanId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesPlanItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < dto.Items.Count; i++)
            {
                var childDto = dto.Items[i];
                childDto.SalesPlanId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("销售计划明细第{i + 1}项与本次提交的其他项重复（CompanyCode、SalesPlanId、LineNumber）");
                }
                if (childDto.SalesPlanItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesPlanItemId, out var target))
                    {
                        throw new TaktBusinessException("销售计划明细不存在（SalesPlanItemId={childDto.SalesPlanItemId}）");
                    }
                    if (target.SalesPlanId != entity.Id)
                    {
                        throw new TaktBusinessException("销售计划明细不属于当前主表（SalesPlanItemId={childDto.SalesPlanItemId}）");
                    }
                    submittedIds.Add(childDto.SalesPlanItemId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_planning_sales_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesPlanItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.SalesPlanId == x.SalesPlanId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.SalesPlanItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_planning_sales_plan_item_line_unique)
                    {
                        throw new TaktBusinessException("销售计划明细的CompanyCode、SalesPlanId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.SalesPlanItemId;
                    target.SalesPlanId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesPlanItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_planning_sales_plan_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesPlanItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.SalesPlanId == x.SalesPlanId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_planning_sales_plan_item_line_unique)
                    {
                        throw new TaktBusinessException("销售计划明细的CompanyCode、SalesPlanId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktSalesPlanItem>();
                    child.Id = 0;
                    child.SalesPlanId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesPlanItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesPlanCode) ? entity.SalesPlanCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _salesPlanItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售计划查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesPlan, bool>> QueryExpression(TaktSalesPlanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesPlan>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesPlanCode != null && x.SalesPlanCode.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.CustomerName != null && x.CustomerName.Contains(keywords))
                || SqlFunc.ToString(x.PlannerId).Contains(keywords)
                || (x.PlanBy != null && x.PlanBy.Contains(keywords))
                || SqlFunc.ToString(x.TotalQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TotalAmount).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedAmount).Contains(keywords)
                || SqlFunc.ToString(x.PlanStatus).Contains(keywords)
                || SqlFunc.ToString(x.ConvertedStatus).Contains(keywords)
                || (x.PlanDescription != null && x.PlanDescription.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlanDate).Contains(keywords)
                || SqlFunc.ToString(x.PlanPeriodStart).Contains(keywords)
                || SqlFunc.ToString(x.PlanPeriodEnd).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesPlanCode))
        {
            exp = exp.And(x => x.SalesPlanCode != null && x.SalesPlanCode.Contains(queryDto.SalesPlanCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerCode))
        {
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(queryDto.CustomerCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerName))
        {
            exp = exp.And(x => x.CustomerName != null && x.CustomerName.Contains(queryDto.CustomerName));
        }

        if (queryDto?.PlannerId.HasValue == true)
        {
            exp = exp.And(x => x.PlannerId == queryDto.PlannerId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlanBy))
        {
            exp = exp.And(x => x.PlanBy != null && x.PlanBy.Contains(queryDto.PlanBy));
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

        if (queryDto?.PlanStatus.HasValue == true)
        {
            exp = exp.And(x => x.PlanStatus == queryDto.PlanStatus);
        }

        if (queryDto?.ConvertedStatus.HasValue == true)
        {
            exp = exp.And(x => x.ConvertedStatus == queryDto.ConvertedStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlanDescription))
        {
            exp = exp.And(x => x.PlanDescription != null && x.PlanDescription.Contains(queryDto.PlanDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PlanDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanDate >= queryDto.PlanDateStart);
        }

        if (queryDto?.PlanDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanDate <= queryDto.PlanDateEnd);
        }

        if (queryDto?.PlanPeriodStartStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanPeriodStart >= queryDto.PlanPeriodStartStart);
        }

        if (queryDto?.PlanPeriodStartEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanPeriodStart <= queryDto.PlanPeriodStartEnd);
        }

        if (queryDto?.PlanPeriodEndStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanPeriodEnd >= queryDto.PlanPeriodEndStart);
        }

        if (queryDto?.PlanPeriodEndEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanPeriodEnd <= queryDto.PlanPeriodEndEnd);
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
