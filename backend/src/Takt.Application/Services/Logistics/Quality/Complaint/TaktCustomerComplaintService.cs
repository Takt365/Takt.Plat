// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Complaint;
using Takt.Domain.Entities.Logistics.Quality.Complaint;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Complaint;

/// <summary>
/// 客诉主应用服务
/// </summary>
public class TaktCustomerComplaintService : TaktServiceBase, ITaktCustomerComplaintService
{
    private readonly ITaktCompanyRepository<TaktCustomerComplaint> _customerComplaintRepository;
    private readonly ITaktCompanyRepository<TaktCustomerComplaintItem> _customerComplaintItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerComplaintRepository">客诉主仓储</param>
    /// <param name="customerComplaintItemRepository">CustomerComplaintItem仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerComplaintService(
        ITaktCompanyRepository<TaktCustomerComplaint> customerComplaintRepository,
        ITaktCompanyRepository<TaktCustomerComplaintItem> customerComplaintItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerComplaintRepository = customerComplaintRepository;
        _customerComplaintItemRepository = customerComplaintItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客诉主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerComplaintDto>> GetCustomerComplaintListAsync(TaktCustomerComplaintQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerComplaintRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerComplaintDto>.Create(
            data.Adapt<List<TaktCustomerComplaintDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客诉主
    /// </summary>
    /// <param name="id">客诉主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintDto?> GetCustomerComplaintByIdAsync(long id)
    {
        var entity = await _customerComplaintRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCustomerComplaintDto>();
        await FillCustomerComplaintDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取客诉主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerComplaintOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _customerComplaintRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ComplaintStatus == 1,
            x => x.CustomerName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CustomerName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建客诉主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintDto> CreateCustomerComplaintAsync(TaktCustomerComplaintCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerComplaint>();
        var isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique = await _uniqueValidator.IsUniqueAsync(
            _customerComplaintRepository,
            x => x.RelatedPlant == entity.RelatedPlant
                && x.CustomerComplaintCode == entity.CustomerComplaintCode);
        if (!isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique)
        {
            throw new TaktBusinessException("客诉主的RelatedPlant、CustomerComplaintCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerComplaintRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CustomerId == entity.CustomerId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.CustomerId, maxSort);
        }
        entity = await _customerComplaintRepository.CreateAsync(entity);
                await SaveCustomerComplaintChildrenAsync(entity, dto);
        return await GetCustomerComplaintByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerComplaintDto>();
    }

    /// <summary>
    /// 更新客诉主
    /// </summary>
    /// <param name="id">客诉主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintDto> UpdateCustomerComplaintAsync(long id, TaktCustomerComplaintUpdateDto dto)
    {
        var entity = await _customerComplaintRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique = await _uniqueValidator.IsUniqueAsync(
            _customerComplaintRepository,
            x => x.RelatedPlant == entity.RelatedPlant
                && x.CustomerComplaintCode == entity.CustomerComplaintCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique)
        {
            throw new TaktBusinessException("客诉主的RelatedPlant、CustomerComplaintCode已存在");
        }
        await _customerComplaintRepository.UpdateAsync(entity);
                await SaveCustomerComplaintChildrenAsync(entity, dto);
        return await GetCustomerComplaintByIdAsync(id) ?? throw new TaktBusinessException("客诉主不存在");
    }

    /// <summary>
    /// 删除客诉主
    /// </summary>
    /// <param name="id">客诉主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerComplaintByIdAsync(long id)
    {
        var entity = await _customerComplaintRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉主不存在或已删除");
        }
        await _customerComplaintItemRepository.DeleteAsync(x => x.ComplaintId == entity.Id);
        var deleted = await _customerComplaintRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("客诉主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除客诉主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerComplaintBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerComplaintByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客诉主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintDto> UpdateCustomerComplaintStatusAsync(TaktCustomerComplaintStatusDto dto)
    {
        var entity = await _customerComplaintRepository.GetByIdAsync(dto.CustomerComplaintId);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉主不存在");
        }
        entity.ComplaintStatus = dto.ComplaintStatus;
        await _customerComplaintRepository.UpdateAsync(entity);
        return await GetCustomerComplaintByIdAsync(dto.CustomerComplaintId) ?? throw new TaktBusinessException("客诉主不存在");
    }

    /// <summary>
    /// 更新客诉主排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintDto> UpdateCustomerComplaintSortAsync(TaktCustomerComplaintSortDto dto)
    {
        var entity = await _customerComplaintRepository.GetByIdAsync(dto.CustomerComplaintId);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉主不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerComplaintRepository.UpdateAsync(entity);
        return await GetCustomerComplaintByIdAsync(dto.CustomerComplaintId) ?? throw new TaktBusinessException("客诉主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerComplaintTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerComplaintTemplateDto>(
            sheetName ?? "客诉主导入模板",
            fileName ?? "客诉主导入模板.xlsx");
    }

    /// <summary>
    /// 导入客诉主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerComplaintAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerComplaintImportDto>(fileStream, sheetName ?? "客诉主导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerComplaint>();
                var importKey = $"{entity.RelatedPlant}|{entity.CustomerComplaintCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（RelatedPlant、CustomerComplaintCode）");
                }
                var isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerComplaintRepository,
                    x => x.RelatedPlant == entity.RelatedPlant
                        && x.CustomerComplaintCode == entity.CustomerComplaintCode);
                if (!isUnique_ix_takt_logistics_quality_customer_complaint_complaint_unique)
                {
                    throw new TaktBusinessException("客诉主的RelatedPlant、CustomerComplaintCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _customerComplaintRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CustomerId == entity.CustomerId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.CustomerId, maxSort);
                }
                await _customerComplaintRepository.CreateAsync(entity);
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
    /// 导出客诉主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerComplaintAsync(TaktCustomerComplaintQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCustomerComplaintQueryDto());
        var list = await _customerComplaintRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerComplaintExportDto>(),
                sheetName ?? "客诉主数据",
                fileName ?? "客诉主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerComplaintExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客诉主数据",
            fileName ?? "客诉主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废客诉明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="complaintId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkCustomerComplaintItemsObsoleteAsync(long complaintId)
    {
        if (complaintId <= 0)
        {
            return;
        }
        var rows = await _customerComplaintItemRepository.GetListAsync(
            x => x.ComplaintId == complaintId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _customerComplaintItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充客诉主详情（加载 OneToMany 子表：客诉明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillCustomerComplaintDetailsAsync(TaktCustomerComplaintDto dto, TaktCustomerComplaint entity)
    {
        if (dto == null)
        {
            return;
        }
        // 客诉明细 → dto.Items（含作废行）
        var items = await _customerComplaintItemRepository.GetListAsync(x => x.ComplaintId == entity.Id);
        dto.Items = items.Adapt<List<TaktCustomerComplaintItemDto>>();
    }

    /// <summary>
    /// 保存客诉主子表级联（客诉明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveCustomerComplaintChildrenAsync(TaktCustomerComplaint entity, TaktCustomerComplaintCreateDto dto)
    {
        // 客诉明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await MarkCustomerComplaintItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _customerComplaintItemRepository.GetListAsync(x => x.ComplaintId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktCustomerComplaintItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < dto.Items.Count; i++)
            {
                var childDto = dto.Items[i];
                childDto.ComplaintId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("客诉明细第{i + 1}项与本次提交的其他项重复（CompanyCode、ComplaintId、LineNumber）");
                }
                if (childDto.CustomerComplaintItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.CustomerComplaintItemId, out var target))
                    {
                        throw new TaktBusinessException("客诉明细不存在（CustomerComplaintItemId={childDto.CustomerComplaintItemId}）");
                    }
                    if (target.ComplaintId != entity.Id)
                    {
                        throw new TaktBusinessException("客诉明细不属于当前主表（CustomerComplaintItemId={childDto.CustomerComplaintItemId}）");
                    }
                    submittedIds.Add(childDto.CustomerComplaintItemId);
                    var isUniqueUpdate_ix_takt_logistics_quality_customer_complaint_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _customerComplaintItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.ComplaintId == x.ComplaintId
                && x.LineNumber == x.LineNumber,
                        childDto.CustomerComplaintItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_customer_complaint_item_line_unique)
                    {
                        throw new TaktBusinessException("客诉明细的CompanyCode、ComplaintId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.CustomerComplaintItemId;
                    target.ComplaintId = entity.Id;
                    target.IsObsolete = 0;
                    await _customerComplaintItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_customer_complaint_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _customerComplaintItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.ComplaintId == x.ComplaintId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_customer_complaint_item_line_unique)
                    {
                        throw new TaktBusinessException("客诉明细的CompanyCode、ComplaintId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktCustomerComplaintItem>();
                    child.Id = 0;
                    child.ComplaintId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _customerComplaintItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.CustomerComplaintCode) ? entity.CustomerComplaintCode : entity.Id.ToString();
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
                await _customerComplaintItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客诉主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerComplaint, bool>> QueryExpression(TaktCustomerComplaintQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerComplaint>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CustomerComplaintCode != null && x.CustomerComplaintCode.Contains(keywords))
                || SqlFunc.ToString(x.CustomerId).Contains(keywords)
                || (x.CustomerName != null && x.CustomerName.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || SqlFunc.ToString(x.ComplaintMethod).Contains(keywords)
                || SqlFunc.ToString(x.ComplaintType).Contains(keywords)
                || SqlFunc.ToString(x.ComplaintLevel).Contains(keywords)
                || SqlFunc.ToString(x.ResponsibleDeptId).Contains(keywords)
                || (x.ResponsibleDeptName != null && x.ResponsibleDeptName.Contains(keywords))
                || SqlFunc.ToString(x.ResponsiblePersonId).Contains(keywords)
                || (x.ResponsiblePersonName != null && x.ResponsiblePersonName.Contains(keywords))
                || (x.ComplaintDescription != null && x.ComplaintDescription.Contains(keywords))
                || (x.HandlingResult != null && x.HandlingResult.Contains(keywords))
                || SqlFunc.ToString(x.CustomerSatisfaction).Contains(keywords)
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ComplaintStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ComplaintDate).Contains(keywords)
                || SqlFunc.ToString(x.RequiredReplyDate).Contains(keywords)
                || SqlFunc.ToString(x.ActualReplyDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerComplaintCode))
        {
            exp = exp.And(x => x.CustomerComplaintCode != null && x.CustomerComplaintCode.Contains(queryDto.CustomerComplaintCode));
        }

        if (queryDto?.CustomerId.HasValue == true)
        {
            exp = exp.And(x => x.CustomerId == queryDto.CustomerId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerName))
        {
            exp = exp.And(x => x.CustomerName != null && x.CustomerName.Contains(queryDto.CustomerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerCode))
        {
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(queryDto.CustomerCode));
        }

        if (queryDto?.ComplaintMethod.HasValue == true)
        {
            exp = exp.And(x => x.ComplaintMethod == queryDto.ComplaintMethod);
        }

        if (queryDto?.ComplaintType.HasValue == true)
        {
            exp = exp.And(x => x.ComplaintType == queryDto.ComplaintType);
        }

        if (queryDto?.ComplaintLevel.HasValue == true)
        {
            exp = exp.And(x => x.ComplaintLevel == queryDto.ComplaintLevel);
        }

        if (queryDto?.ResponsibleDeptId.HasValue == true)
        {
            exp = exp.And(x => x.ResponsibleDeptId == queryDto.ResponsibleDeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ResponsibleDeptName))
        {
            exp = exp.And(x => x.ResponsibleDeptName != null && x.ResponsibleDeptName.Contains(queryDto.ResponsibleDeptName));
        }

        if (queryDto?.ResponsiblePersonId.HasValue == true)
        {
            exp = exp.And(x => x.ResponsiblePersonId == queryDto.ResponsiblePersonId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ResponsiblePersonName))
        {
            exp = exp.And(x => x.ResponsiblePersonName != null && x.ResponsiblePersonName.Contains(queryDto.ResponsiblePersonName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ComplaintDescription))
        {
            exp = exp.And(x => x.ComplaintDescription != null && x.ComplaintDescription.Contains(queryDto.ComplaintDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.HandlingResult))
        {
            exp = exp.And(x => x.HandlingResult != null && x.HandlingResult.Contains(queryDto.HandlingResult));
        }

        if (queryDto?.CustomerSatisfaction.HasValue == true)
        {
            exp = exp.And(x => x.CustomerSatisfaction == queryDto.CustomerSatisfaction);
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ComplaintStatus.HasValue == true)
        {
            exp = exp.And(x => x.ComplaintStatus == queryDto.ComplaintStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ComplaintDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ComplaintDate >= queryDto.ComplaintDateStart);
        }

        if (queryDto?.ComplaintDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ComplaintDate <= queryDto.ComplaintDateEnd);
        }

        if (queryDto?.RequiredReplyDateStart.HasValue == true)
        {
            exp = exp.And(x => x.RequiredReplyDate >= queryDto.RequiredReplyDateStart);
        }

        if (queryDto?.RequiredReplyDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.RequiredReplyDate <= queryDto.RequiredReplyDateEnd);
        }

        if (queryDto?.ActualReplyDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualReplyDate >= queryDto.ActualReplyDateStart);
        }

        if (queryDto?.ActualReplyDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualReplyDate <= queryDto.ActualReplyDateEnd);
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

    /// <inheritdoc />
    public async Task<TaktCustomerComplaintMonthlyTrendResultDto> GetCustomerComplaintMonthlyTrendAnalysisAsync(
        TaktCustomerComplaintMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildCustomerComplaintMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktCustomerComplaintMonthlyTrendResultDto
        {
            Paged = TaktPagedResult<TaktCustomerComplaintMonthlyTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            CustomerCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerComplaintMonthlyTrendAnalysisAsync(
        TaktCustomerComplaintMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildCustomerComplaintMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string> { "plantCode", "customerCode", "customerName" };
        var columnLabels = new List<string> { "工厂代码", "客户编码", "客户名称" };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });
        var exportRows = built.OrderedRows.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["plantCode"] = row.PlantCode,
                ["customerCode"] = row.CustomerCode,
                ["customerName"] = row.CustomerName,
                ["basePeriod"] = row.BasePeriod,
                ["comparePeriod"] = row.ComparePeriod,
                ["varianceAmount"] = row.VarianceAmount,
                ["variancePercent"] = row.VariancePercent.HasValue
                    ? Math.Round(row.VariancePercent.Value, 4, MidpointRounding.AwayFromZero)
                    : null,
                ["trend"] = row.Trend,
            };
            foreach (var period in built.PeriodOrder)
            {
                dict[$"period_{period}"] = row.PeriodValues.TryGetValue(period, out var count) ? count : 0;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "顾客投诉推移表",
            fileName ?? "顾客投诉推移表.xlsx");
    }

    /// <summary>
    /// 构建顾客投诉月度推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析构建结果</returns>
    private async Task<CustomerComplaintMonthlyTrendAnalysisBuilt> BuildCustomerComplaintMonthlyTrendAnalysisAsync(
        TaktCustomerComplaintMonthlyTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var exp = BuildCustomerComplaintTrendExpression(queryDto, plantCode);
        var complaints = await _customerComplaintRepository.GetListAsync(exp);
        if (complaints.Count == 0)
        {
            return CustomerComplaintMonthlyTrendAnalysisBuilt.Empty();
        }
        var (rangeStart, rangeEnd, periodOrder) = ResolveCustomerComplaintTrendRange(queryDto);
        var focusPeriod = ResolveCustomerComplaintFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var allRows = complaints
            .GroupBy(
                c => new CustomerComplaintTrendRowKey(
                    c.RelatedPlant.Trim(),
                    c.CustomerCode?.Trim() ?? string.Empty),
                CustomerComplaintTrendRowKeyComparer.Instance)
            .Select(g => BuildCustomerComplaintMonthlyTrendRow(
                g.Key,
                g.ToList(),
                periodOrder,
                focusPeriod,
                rangeStart,
                rangeEnd))
            .ToList();
        var filtered = FilterCustomerComplaintTrendRows(allRows, queryDto.TrendFilter);
        var ordered = OrderCustomerComplaintTrendRows(filtered);
        return new CustomerComplaintMonthlyTrendAnalysisBuilt
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            BasePeriod = allRows.FirstOrDefault()?.BasePeriod,
            ComparePeriod = focusPeriod,
            UpCount = allRows.Count(r => r.Trend == "up"),
            DownCount = allRows.Count(r => r.Trend == "down"),
            FlatCount = allRows.Count(r => r.Trend == "flat"),
            NoneCount = allRows.Count(r => r.Trend == "none"),
        };
    }

    /// <summary>
    /// 构建顾客投诉推移筛选条件
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktCustomerComplaint, bool>> BuildCustomerComplaintTrendExpression(
        TaktCustomerComplaintMonthlyTrendQueryDto queryDto,
        string plantCode)
    {
        var (rangeStart, rangeEnd, _) = ResolveCustomerComplaintTrendRange(queryDto);
        var exp = Expressionable.Create<TaktCustomerComplaint>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.RelatedPlant == plantCode
            && x.ComplaintDate >= rangeStart
            && x.ComplaintDate <= rangeEnd);
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode.Trim();
            exp = exp.And(x => x.CustomerCode == customerCode);
        }
        if (queryDto.ComplaintType.HasValue)
        {
            exp = exp.And(x => x.ComplaintType == queryDto.ComplaintType.Value);
        }
        if (queryDto.ComplaintLevel.HasValue)
        {
            exp = exp.And(x => x.ComplaintLevel == queryDto.ComplaintLevel.Value);
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 解析顾客投诉推移分析日期区间与期间列
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>区间起止与期间列顺序</returns>
    private static (DateTime RangeStart, DateTime RangeEnd, List<string> PeriodOrder) ResolveCustomerComplaintTrendRange(
        TaktCustomerComplaintMonthlyTrendQueryDto queryDto)
    {
        var (periodStart, periodEnd) = NormalizeCustomerComplaintPeriodBounds(
            queryDto.PeriodDateStart,
            queryDto.PeriodDateEnd);
        if (periodStart.HasValue || periodEnd.HasValue)
        {
            var startMonth = periodStart ?? periodEnd!.Value;
            var endMonth = periodEnd ?? periodStart!.Value;
            if (startMonth > endMonth)
            {
                (startMonth, endMonth) = (endMonth, startMonth);
            }
            var monthCount = ((endMonth.Year - startMonth.Year) * 12) + endMonth.Month - startMonth.Month + 1;
            if (monthCount > TaktPriceTrendAnalysisHelper.MaxTrendMonths)
            {
                throw new ArgumentException($"分析区间不得超过 {TaktPriceTrendAnalysisHelper.MaxTrendMonths} 个月");
            }
            var rangeStart = startMonth;
            var rangeEnd = endMonth.AddMonths(1).AddDays(-1);
            var periodOrder = BuildCustomerComplaintConsecutivePeriodOrder(startMonth, endMonth);
            return (rangeStart, rangeEnd, periodOrder);
        }
        var (resolvedStart, resolvedEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(null, null);
        var start = new DateTime(resolvedStart.Year, resolvedStart.Month, 1);
        var endMonthFirst = new DateTime(resolvedEnd.Year, resolvedEnd.Month, 1);
        return (resolvedStart, resolvedEnd, BuildCustomerComplaintConsecutivePeriodOrder(start, endMonthFirst));
    }

    /// <summary>
    /// 归一化期间起止到月初
    /// </summary>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <returns>月初起止</returns>
    private static (DateTime? Start, DateTime? End) NormalizeCustomerComplaintPeriodBounds(
        DateTime? periodDateStart,
        DateTime? periodDateEnd)
    {
        DateTime? start = periodDateStart.HasValue
            ? new DateTime(periodDateStart.Value.Year, periodDateStart.Value.Month, 1)
            : null;
        DateTime? end = periodDateEnd.HasValue
            ? new DateTime(periodDateEnd.Value.Year, periodDateEnd.Value.Month, 1)
            : null;
        if (start.HasValue && end.HasValue && start > end)
        {
            (start, end) = (end, start);
        }
        return (start, end);
    }

    /// <summary>
    /// 构建连续 yyyy-MM 期间列
    /// </summary>
    /// <param name="periodStart">期间起（月初）</param>
    /// <param name="periodEnd">期间止（月初）</param>
    /// <returns>期间列顺序</returns>
    private static List<string> BuildCustomerComplaintConsecutivePeriodOrder(DateTime periodStart, DateTime periodEnd)
    {
        var order = new List<string>();
        for (var cursor = periodStart; cursor <= periodEnd; cursor = cursor.AddMonths(1))
        {
            order.Add(cursor.ToString("yyyy-MM"));
        }
        return order;
    }

    /// <summary>
    /// 解析关注期间
    /// </summary>
    /// <param name="focusPeriod">关注期间</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>关注期间 yyyy-MM</returns>
    private static string? ResolveCustomerComplaintFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    /// <summary>
    /// 构建单行顾客投诉月推移
    /// </summary>
    /// <param name="key">行键</param>
    /// <param name="groupRows">同键投诉记录</param>
    /// <param name="periodOrder">期间列</param>
    /// <param name="focusPeriod">关注期间</param>
    /// <param name="rangeStart">分析区间起</param>
    /// <param name="rangeEnd">分析区间止</param>
    /// <returns>转置行</returns>
    private static TaktCustomerComplaintMonthlyTrendDto BuildCustomerComplaintMonthlyTrendRow(
        CustomerComplaintTrendRowKey key,
        IReadOnlyList<TaktCustomerComplaint> groupRows,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var row = new TaktCustomerComplaintMonthlyTrendDto
        {
            PlantCode = key.PlantCode,
            CustomerCode = key.CustomerCode,
            CustomerName = groupRows
                .Select(r => r.CustomerName?.Trim())
                .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n)) ?? string.Empty,
            Trend = "none",
        };
        foreach (var period in periodOrder)
        {
            if (!DateTime.TryParseExact(
                    period + "-01",
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out var monthStart))
            {
                continue;
            }
            var monthEnd = monthStart.AddMonths(1).AddTicks(-1);
            if (monthEnd < rangeStart || monthStart > rangeEnd)
            {
                continue;
            }
            var count = groupRows.Count(r =>
                r.ComplaintDate >= monthStart && r.ComplaintDate <= monthEnd);
            if (count > 0)
            {
                row.PeriodValues[period] = count;
            }
        }
        ApplyCustomerComplaintFocusTrend(row, focusPeriod);
        return row;
    }

    /// <summary>
    /// 按关注月计算环比涨跌
    /// </summary>
    /// <param name="row">转置行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    private static void ApplyCustomerComplaintFocusTrend(
        TaktCustomerComplaintMonthlyTrendDto row,
        string? focusPeriod)
    {
        if (string.IsNullOrWhiteSpace(focusPeriod))
        {
            return;
        }
        var comparePeriod = focusPeriod.Trim();
        if (!DateTime.TryParseExact(
                comparePeriod + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var compareMonth))
        {
            return;
        }
        var basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        row.PeriodValues.TryGetValue(basePeriod, out var baseCount);
        row.PeriodValues.TryGetValue(comparePeriod, out var compareCount);
        row.VarianceAmount = compareCount - baseCount;
        if (baseCount != 0)
        {
            row.VariancePercent = Math.Round(
                (decimal)row.VarianceAmount.Value / baseCount,
                4,
                MidpointRounding.AwayFromZero);
        }
        if (compareCount > baseCount)
        {
            row.Trend = "up";
        }
        else if (compareCount < baseCount)
        {
            row.Trend = "down";
        }
        else
        {
            row.Trend = "flat";
        }
    }

    /// <summary>
    /// 涨跌筛选
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <returns>筛选后行</returns>
    private static List<TaktCustomerComplaintMonthlyTrendDto> FilterCustomerComplaintTrendRows(
        IReadOnlyList<TaktCustomerComplaintMonthlyTrendDto> rows,
        string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
        if (filter == "changed")
        {
            return rows.Where(r => r.Trend is "up" or "down").ToList();
        }
        return rows.Where(r => string.Equals(r.Trend, filter, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// 涨跌优先排序
    /// </summary>
    /// <param name="rows">行集合</param>
    /// <returns>排序后行</returns>
    private static List<TaktCustomerComplaintMonthlyTrendDto> OrderCustomerComplaintTrendRows(
        IReadOnlyList<TaktCustomerComplaintMonthlyTrendDto> rows)
    {
        static int TrendRank(string? trend) => trend switch
        {
            "up" => 0,
            "down" => 1,
            "flat" => 2,
            _ => 3,
        };
        return rows
            .OrderBy(r => TrendRank(r.Trend))
            .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0))
            .ThenBy(r => r.CustomerCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 顾客投诉推移行键
    /// </summary>
    /// <param name="PlantCode">工厂代码</param>
    /// <param name="CustomerCode">客户编码</param>
    private sealed record CustomerComplaintTrendRowKey(string PlantCode, string CustomerCode);

    /// <summary>
    /// 顾客投诉推移行键比较器
    /// </summary>
    private sealed class CustomerComplaintTrendRowKeyComparer : IEqualityComparer<CustomerComplaintTrendRowKey>
    {
        /// <summary>单例</summary>
        public static CustomerComplaintTrendRowKeyComparer Instance { get; } = new();

        /// <inheritdoc />
        public bool Equals(CustomerComplaintTrendRowKey? x, CustomerComplaintTrendRowKey? y)
        {
            if (x is null || y is null)
            {
                return x == y;
            }
            return string.Equals(x.PlantCode, y.PlantCode, StringComparison.Ordinal)
                && string.Equals(x.CustomerCode, y.CustomerCode, StringComparison.Ordinal);
        }

        /// <inheritdoc />
        public int GetHashCode(CustomerComplaintTrendRowKey obj) =>
            HashCode.Combine(obj.PlantCode, obj.CustomerCode);
    }

    /// <summary>
    /// 顾客投诉月度推移分析构建结果
    /// </summary>
    private sealed class CustomerComplaintMonthlyTrendAnalysisBuilt
    {
        /// <summary>过滤并排序后的全量行</summary>
        public List<TaktCustomerComplaintMonthlyTrendDto> OrderedRows { get; init; } = new();

        /// <summary>期间列顺序</summary>
        public List<string> PeriodOrder { get; init; } = new();

        /// <summary>环比基准期间</summary>
        public string? BasePeriod { get; init; }

        /// <summary>环比对比期间</summary>
        public string? ComparePeriod { get; init; }

        /// <summary>上涨行数</summary>
        public int UpCount { get; init; }

        /// <summary>下跌行数</summary>
        public int DownCount { get; init; }

        /// <summary>持平行数</summary>
        public int FlatCount { get; init; }

        /// <summary>无法比较行数</summary>
        public int NoneCount { get; init; }

        /// <summary>空结果</summary>
        public static CustomerComplaintMonthlyTrendAnalysisBuilt Empty() => new();
    }
}
