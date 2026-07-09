// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktCountersignService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：会签单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 会签单应用服务
/// </summary>
public class TaktCountersignService : TaktServiceBase, ITaktCountersignService
{
    private readonly ITaktApprovalRepository<TaktCountersign> _countersignRepository;
    private readonly ITaktCompanyRepository<TaktCountersignDetail> _countersignDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="countersignRepository">会签单仓储</param>
    /// <param name="countersignDetailRepository">CountersignDetail仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCountersignService(
        ITaktApprovalRepository<TaktCountersign> countersignRepository,
        ITaktCompanyRepository<TaktCountersignDetail> countersignDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _countersignRepository = countersignRepository;
        _countersignDetailRepository = countersignDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会签单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCountersignDto>> GetCountersignListAsync(TaktCountersignQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _countersignRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCountersignDto>.Create(
            data.Adapt<List<TaktCountersignDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会签单
    /// </summary>
    /// <param name="id">会签单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDto?> GetCountersignByIdAsync(long id)
    {
        var entity = await _countersignRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCountersignDto>();
        await FillCountersignDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取会签单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCountersignOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _countersignRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CountersignStatus == 1,
            x => x.CountersignCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CountersignCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建会签单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDto> CreateCountersignAsync(TaktCountersignCreateDto dto)
    {
        var entity = dto.Adapt<TaktCountersign>();
        var isUnique_ix_countersign_code_unique = await _uniqueValidator.IsUniqueAsync(
            _countersignRepository,
            x => x.CountersignCode == entity.CountersignCode);
        if (!isUnique_ix_countersign_code_unique)
        {
            throw new TaktBusinessException("会签单的CountersignCode已存在");
        }
        entity = await _countersignRepository.CreateAsync(entity);
                await SaveCountersignChildrenAsync(entity, dto);
        return await GetCountersignByIdAsync(entity.Id) ?? entity.Adapt<TaktCountersignDto>();
    }

    /// <summary>
    /// 更新会签单
    /// </summary>
    /// <param name="id">会签单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDto> UpdateCountersignAsync(long id, TaktCountersignUpdateDto dto)
    {
        var entity = await _countersignRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会签单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_countersign_code_unique = await _uniqueValidator.IsUniqueAsync(
            _countersignRepository,
            x => x.CountersignCode == entity.CountersignCode,
            id);
        if (!isUnique_ix_countersign_code_unique)
        {
            throw new TaktBusinessException("会签单的CountersignCode已存在");
        }
        await _countersignRepository.UpdateAsync(entity);
                await SaveCountersignChildrenAsync(entity, dto);
        return await GetCountersignByIdAsync(id) ?? throw new TaktBusinessException("会签单不存在");
    }

    /// <summary>
    /// 删除会签单
    /// </summary>
    /// <param name="id">会签单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCountersignByIdAsync(long id)
    {
        var entity = await _countersignRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会签单不存在或已删除");
        }
        await _countersignDetailRepository.DeleteAsync(x => x.CountersignId == entity.Id);
        var deleted = await _countersignRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("会签单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除会签单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCountersignBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCountersignByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会签单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDto> UpdateCountersignStatusAsync(TaktCountersignStatusDto dto)
    {
        var entity = await _countersignRepository.GetByIdAsync(dto.CountersignId);
        if (entity == null)
        {
            throw new TaktBusinessException("会签单不存在");
        }
        entity.CountersignStatus = dto.CountersignStatus;
        await _countersignRepository.UpdateAsync(entity);
        return await GetCountersignByIdAsync(dto.CountersignId) ?? throw new TaktBusinessException("会签单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCountersignTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCountersignTemplateDto>(
            sheetName ?? "会签单导入模板",
            fileName ?? "会签单导入模板.xlsx");
    }

    /// <summary>
    /// 导入会签单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCountersignAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCountersignImportDto>(fileStream, sheetName ?? "会签单导入模板");
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
                var entity = rows[i].Adapt<TaktCountersign>();
                var importKey = $"{entity.CountersignCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CountersignCode）");
                }
                var isUnique_ix_countersign_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _countersignRepository,
                    x => x.CountersignCode == entity.CountersignCode);
                if (!isUnique_ix_countersign_code_unique)
                {
                    throw new TaktBusinessException("会签单的CountersignCode已存在");
                }
                await _countersignRepository.CreateAsync(entity);
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
    /// 导出会签单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCountersignAsync(TaktCountersignQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCountersignQueryDto());
        var list = await _countersignRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCountersignExportDto>(),
                sheetName ?? "会签单数据",
                fileName ?? "会签单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCountersignExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会签单数据",
            fileName ?? "会签单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废会签单明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="countersignId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkCountersignDetailsObsoleteAsync(long countersignId)
    {
        if (countersignId <= 0)
        {
            return;
        }
        var rows = await _countersignDetailRepository.GetListAsync(
            x => x.CountersignId == countersignId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _countersignDetailRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充会签单详情（加载 OneToMany 子表：会签单明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillCountersignDetailsAsync(TaktCountersignDto dto, TaktCountersign entity)
    {
        if (dto == null)
        {
            return;
        }
        // 会签单明细 → dto.CountersignDetails（含作废行）
        var countersigndetails = await _countersignDetailRepository.GetListAsync(x => x.CountersignId == entity.Id);
        dto.CountersignDetails = countersigndetails.Adapt<List<TaktCountersignDetailDto>>();
    }

    /// <summary>
    /// 保存会签单子表级联（会签单明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveCountersignChildrenAsync(TaktCountersign entity, TaktCountersignCreateDto dto)
    {
        // 会签单明细（CountersignDetails）
        if (dto.CountersignDetails is not { Count: > 0 })
        {
            await MarkCountersignDetailsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _countersignDetailRepository.GetListAsync(x => x.CountersignId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktCountersignDetail>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < dto.CountersignDetails.Count; i++)
            {
                var childDto = dto.CountersignDetails[i];
                childDto.CountersignId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("会签单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、CountersignId、LineNumber）");
                }
                if (childDto.CountersignDetailId > 0)
                {
                    if (!existingById.TryGetValue(childDto.CountersignDetailId, out var target))
                    {
                        throw new TaktBusinessException("会签单明细不存在（CountersignDetailId={childDto.CountersignDetailId}）");
                    }
                    if (target.CountersignId != entity.Id)
                    {
                        throw new TaktBusinessException("会签单明细不属于当前主表（CountersignDetailId={childDto.CountersignDetailId}）");
                    }
                    submittedIds.Add(childDto.CountersignDetailId);
                    var isUniqueUpdate_ix_countersign_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _countersignDetailRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.CountersignId == x.CountersignId
                && x.LineNumber == x.LineNumber,
                        childDto.CountersignDetailId);
                    if (!isUniqueUpdate_ix_countersign_detail_line_unique)
                    {
                        throw new TaktBusinessException("会签单明细的CompanyCode、CountersignId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.CountersignDetailId;
                    target.CountersignId = entity.Id;
                    target.IsObsolete = 0;
                    await _countersignDetailRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_countersign_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _countersignDetailRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.CountersignId == x.CountersignId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_countersign_detail_line_unique)
                    {
                        throw new TaktBusinessException("会签单明细的CompanyCode、CountersignId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktCountersignDetail>();
                    child.Id = 0;
                    child.CountersignId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _countersignDetailRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.CountersignCode) ? entity.CountersignCode : entity.Id.ToString();
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
                await _countersignDetailRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会签单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCountersign, bool>> QueryExpression(TaktCountersignQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCountersign>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CountersignCode != null && x.CountersignCode.Contains(keywords))
                || SqlFunc.ToString(x.PurchaseInquiryId).Contains(keywords)
                || (x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(keywords))
                || (x.BusinessType != null && x.BusinessType.Contains(keywords))
                || (x.BusinessKey != null && x.BusinessKey.Contains(keywords))
                || SqlFunc.ToString(x.StepNo).Contains(keywords)
                || (x.CountersignDepts != null && x.CountersignDepts.Contains(keywords))
                || (x.FinanceDept != null && x.FinanceDept.Contains(keywords))
                || (x.BudgetReviewComment != null && x.BudgetReviewComment.Contains(keywords))
                || (x.ExecutiveOffice != null && x.ExecutiveOffice.Contains(keywords))
                || SqlFunc.ToString(x.ApplicantBy).Contains(keywords)
                || (x.ApplicationDept != null && x.ApplicationDept.Contains(keywords))
                || (x.CostBearerDept != null && x.CostBearerDept.Contains(keywords))
                || SqlFunc.ToString(x.IsBudget).Contains(keywords)
                || (x.BudgetItem != null && x.BudgetItem.Contains(keywords))
                || SqlFunc.ToString(x.BudgetAmount).Contains(keywords)
                || SqlFunc.ToString(x.ApplicationAmount).Contains(keywords)
                || (x.CountersignTitle != null && x.CountersignTitle.Contains(keywords))
                || (x.ApplicationReason != null && x.ApplicationReason.Contains(keywords))
                || (x.BudgetUsageDescription != null && x.BudgetUsageDescription.Contains(keywords))
                || (x.TargetAndExpectedBenefit != null && x.TargetAndExpectedBenefit.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || SqlFunc.ToString(x.CountersignStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CountersignCode))
        {
            exp = exp.And(x => x.CountersignCode != null && x.CountersignCode.Contains(queryDto.CountersignCode));
        }

        if (queryDto?.PurchaseInquiryId.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseInquiryId == queryDto.PurchaseInquiryId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseInquiryCode))
        {
            exp = exp.And(x => x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(queryDto.PurchaseInquiryCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.BusinessType))
        {
            exp = exp.And(x => x.BusinessType != null && x.BusinessType.Contains(queryDto.BusinessType));
        }

        if (!string.IsNullOrEmpty(queryDto?.BusinessKey))
        {
            exp = exp.And(x => x.BusinessKey != null && x.BusinessKey.Contains(queryDto.BusinessKey));
        }

        if (queryDto?.StepNo.HasValue == true)
        {
            exp = exp.And(x => x.StepNo == queryDto.StepNo);
        }

        if (!string.IsNullOrEmpty(queryDto?.CountersignDepts))
        {
            exp = exp.And(x => x.CountersignDepts != null && x.CountersignDepts.Contains(queryDto.CountersignDepts));
        }

        if (!string.IsNullOrEmpty(queryDto?.FinanceDept))
        {
            exp = exp.And(x => x.FinanceDept != null && x.FinanceDept.Contains(queryDto.FinanceDept));
        }

        if (!string.IsNullOrEmpty(queryDto?.BudgetReviewComment))
        {
            exp = exp.And(x => x.BudgetReviewComment != null && x.BudgetReviewComment.Contains(queryDto.BudgetReviewComment));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExecutiveOffice))
        {
            exp = exp.And(x => x.ExecutiveOffice != null && x.ExecutiveOffice.Contains(queryDto.ExecutiveOffice));
        }

        if (queryDto?.ApplicantBy.HasValue == true)
        {
            exp = exp.And(x => x.ApplicantBy == queryDto.ApplicantBy);
        }

        if (!string.IsNullOrEmpty(queryDto?.ApplicationDept))
        {
            exp = exp.And(x => x.ApplicationDept != null && x.ApplicationDept.Contains(queryDto.ApplicationDept));
        }

        if (!string.IsNullOrEmpty(queryDto?.CostBearerDept))
        {
            exp = exp.And(x => x.CostBearerDept != null && x.CostBearerDept.Contains(queryDto.CostBearerDept));
        }

        if (queryDto?.IsBudget.HasValue == true)
        {
            exp = exp.And(x => x.IsBudget == queryDto.IsBudget);
        }

        if (!string.IsNullOrEmpty(queryDto?.BudgetItem))
        {
            exp = exp.And(x => x.BudgetItem != null && x.BudgetItem.Contains(queryDto.BudgetItem));
        }

        if (queryDto?.BudgetAmount.HasValue == true)
        {
            exp = exp.And(x => x.BudgetAmount == queryDto.BudgetAmount);
        }

        if (queryDto?.ApplicationAmount.HasValue == true)
        {
            exp = exp.And(x => x.ApplicationAmount == queryDto.ApplicationAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.CountersignTitle))
        {
            exp = exp.And(x => x.CountersignTitle != null && x.CountersignTitle.Contains(queryDto.CountersignTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.ApplicationReason))
        {
            exp = exp.And(x => x.ApplicationReason != null && x.ApplicationReason.Contains(queryDto.ApplicationReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.BudgetUsageDescription))
        {
            exp = exp.And(x => x.BudgetUsageDescription != null && x.BudgetUsageDescription.Contains(queryDto.BudgetUsageDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetAndExpectedBenefit))
        {
            exp = exp.And(x => x.TargetAndExpectedBenefit != null && x.TargetAndExpectedBenefit.Contains(queryDto.TargetAndExpectedBenefit));
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (queryDto?.CountersignStatus.HasValue == true)
        {
            exp = exp.And(x => x.CountersignStatus == queryDto.CountersignStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
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
