// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktCountersignService.cs
// 创建时间：2026-06-07
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
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="countersignRepository">会签单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCountersignService(
        ITaktApprovalRepository<TaktCountersign> countersignRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _countersignRepository = countersignRepository;
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
        return entity.Adapt<TaktCountersignDto>();
    }

    /// <summary>
    /// 获取会签单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCountersignOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _countersignRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CountersignCode,
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
        return await GetCountersignByIdAsync(id) ?? throw new TaktBusinessException("会签单不存在");
    }

    /// <summary>
    /// 删除会签单
    /// </summary>
    /// <param name="id">会签单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCountersignByIdAsync(long id)
    {
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
                || (x.CountersignDepts != null && x.CountersignDepts.Contains(keywords))
                || (x.FinanceDept != null && x.FinanceDept.Contains(keywords))
                || (x.BudgetReviewComment != null && x.BudgetReviewComment.Contains(keywords))
                || (x.ExecutiveOffice != null && x.ExecutiveOffice.Contains(keywords))
                || SqlFunc.ToString(x.FlowInstanceId).Contains(keywords)
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
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CountersignCode))
        {
            exp = exp.And(x => x.CountersignCode != null && x.CountersignCode.Contains(queryDto.CountersignCode));
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

        if (queryDto?.FlowInstanceId.HasValue == true)
        {
            exp = exp.And(x => x.FlowInstanceId == queryDto.FlowInstanceId);
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

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
