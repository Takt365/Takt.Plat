// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktExpenseService.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：费用单应用服务实现
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
/// 费用单应用服务
/// </summary>
public class TaktExpenseService : TaktServiceBase, ITaktExpenseService
{
    private readonly ITaktApprovalRepository<TaktExpense> _expenseRepository;
    private readonly ITaktCompanyRepository<TaktExpenseDetail> _expenseDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="expenseRepository">费用单仓储</param>
    /// <param name="expenseDetailRepository">ExpenseDetail仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktExpenseService(
        ITaktApprovalRepository<TaktExpense> expenseRepository,
        ITaktCompanyRepository<TaktExpenseDetail> expenseDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _expenseRepository = expenseRepository;
        _expenseDetailRepository = expenseDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取费用单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktExpenseDto>> GetExpenseListAsync(TaktExpenseQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _expenseRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktExpenseDto>.Create(
            data.Adapt<List<TaktExpenseDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取费用单
    /// </summary>
    /// <param name="id">费用单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDto?> GetExpenseByIdAsync(long id)
    {
        var entity = await _expenseRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktExpenseDto>();
        await FillExpenseDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取费用单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetExpenseOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _expenseRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ExpenseStatus == 1,
            x => x.ExpenseCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ExpenseCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建费用单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDto> CreateExpenseAsync(TaktExpenseCreateDto dto)
    {
        var entity = dto.Adapt<TaktExpense>();
        var isUnique_ix_expense_code_unique = await _uniqueValidator.IsUniqueAsync(
            _expenseRepository,
            x => x.ExpenseCode == entity.ExpenseCode);
        if (!isUnique_ix_expense_code_unique)
        {
            throw new TaktBusinessException("费用单的ExpenseCode已存在");
        }
        entity = await _expenseRepository.CreateAsync(entity);
                await SaveExpenseChildrenAsync(entity, dto);
        return await GetExpenseByIdAsync(entity.Id) ?? entity.Adapt<TaktExpenseDto>();
    }

    /// <summary>
    /// 更新费用单
    /// </summary>
    /// <param name="id">费用单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDto> UpdateExpenseAsync(long id, TaktExpenseUpdateDto dto)
    {
        var entity = await _expenseRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("费用单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_expense_code_unique = await _uniqueValidator.IsUniqueAsync(
            _expenseRepository,
            x => x.ExpenseCode == entity.ExpenseCode,
            id);
        if (!isUnique_ix_expense_code_unique)
        {
            throw new TaktBusinessException("费用单的ExpenseCode已存在");
        }
        await _expenseRepository.UpdateAsync(entity);
                await SaveExpenseChildrenAsync(entity, dto);
        return await GetExpenseByIdAsync(id) ?? throw new TaktBusinessException("费用单不存在");
    }

    /// <summary>
    /// 删除费用单
    /// </summary>
    /// <param name="id">费用单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteExpenseByIdAsync(long id)
    {
        var entity = await _expenseRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("费用单不存在或已删除");
        }
        await _expenseDetailRepository.DeleteAsync(x => x.ExpenseId == entity.Id);
        var deleted = await _expenseRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("费用单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除费用单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteExpenseBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteExpenseByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新费用单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDto> UpdateExpenseStatusAsync(TaktExpenseStatusDto dto)
    {
        var entity = await _expenseRepository.GetByIdAsync(dto.ExpenseId);
        if (entity == null)
        {
            throw new TaktBusinessException("费用单不存在");
        }
        entity.ExpenseStatus = dto.ExpenseStatus;
        await _expenseRepository.UpdateAsync(entity);
        return await GetExpenseByIdAsync(dto.ExpenseId) ?? throw new TaktBusinessException("费用单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetExpenseTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktExpenseTemplateDto>(
            sheetName ?? "费用单导入模板",
            fileName ?? "费用单导入模板.xlsx");
    }

    /// <summary>
    /// 导入费用单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportExpenseAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktExpenseImportDto>(fileStream, sheetName ?? "费用单导入模板");
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
                var entity = rows[i].Adapt<TaktExpense>();
                var importKey = $"{entity.ExpenseCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ExpenseCode）");
                }
                var isUnique_ix_expense_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _expenseRepository,
                    x => x.ExpenseCode == entity.ExpenseCode);
                if (!isUnique_ix_expense_code_unique)
                {
                    throw new TaktBusinessException("费用单的ExpenseCode已存在");
                }
                await _expenseRepository.CreateAsync(entity);
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
    /// 导出费用单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportExpenseAsync(TaktExpenseQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktExpenseQueryDto());
        var list = await _expenseRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktExpenseExportDto>(),
                sheetName ?? "费用单数据",
                fileName ?? "费用单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktExpenseExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "费用单数据",
            fileName ?? "费用单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充费用单详情（加载 OneToMany 子表：费用单明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillExpenseDetailsAsync(TaktExpenseDto dto, TaktExpense entity)
    {
        if (dto == null)
        {
            return;
        }
        // 费用单明细 → dto.ExpenseDetails
        var expensedetails = await _expenseDetailRepository.GetListAsync(x => x.ExpenseId == entity.Id);
        dto.ExpenseDetails = expensedetails.Adapt<List<TaktExpenseDetailDto>>();
    }

    /// <summary>
    /// 保存费用单子表级联（费用单明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveExpenseChildrenAsync(TaktExpense entity, TaktExpenseCreateDto dto)
    {
        // 费用单明细（ExpenseDetails）
        if (dto.ExpenseDetails is not { Count: > 0 })
        {
            await _expenseDetailRepository.DeleteAsync(x => x.ExpenseId == entity.Id);
        }
        else
        {
            var expensedetails = dto.ExpenseDetails.Adapt<List<TaktExpenseDetail>>();
            foreach (var child in expensedetails)
            {
                child.ExpenseId = entity.Id;
            }
            var expensedetailsNeedLine = expensedetails.Where(c => c.LineNumber <= 0).ToList();
            if (expensedetailsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.ExpenseCode) ? entity.ExpenseCode : entity.Id.ToString();
                var maxLine = await _expenseDetailRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ExpenseId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, expensedetailsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in expensedetails)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < expensedetails.Count; i++)
                        {
                            var key = $"{expensedetails[i].CompanyCode}|{expensedetails[i].ExpenseId}|{expensedetails[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"费用单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、ExpenseId、LineNumber）");
                            }
                        }
            await _expenseDetailRepository.DeleteAsync(x => x.ExpenseId == entity.Id);
            foreach (var child in expensedetails)
            {
            var isUnique_ix_expense_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                _expenseDetailRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.ExpenseId == child.ExpenseId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_expense_detail_line_unique)
            {
                throw new TaktBusinessException("费用单明细的CompanyCode、ExpenseId、LineNumber已存在");
            }
            }
            await _expenseDetailRepository.CreateRangeAsync(expensedetails);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建费用单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktExpense, bool>> QueryExpression(TaktExpenseQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktExpense>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ExpenseCode != null && x.ExpenseCode.Contains(keywords))
                || (x.ExpenseTitle != null && x.ExpenseTitle.Contains(keywords))
                || SqlFunc.ToString(x.ExpenseType).Contains(keywords)
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.SupplierName != null && x.SupplierName.Contains(keywords))
                || SqlFunc.ToString(x.ApplicantBy).Contains(keywords)
                || (x.ApplicationDept != null && x.ApplicationDept.Contains(keywords))
                || (x.CostBearerDept != null && x.CostBearerDept.Contains(keywords))
                || (x.CostCenter != null && x.CostCenter.Contains(keywords))
                || SqlFunc.ToString(x.CountersignId).Contains(keywords)
                || (x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(keywords))
                || (x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(keywords))
                || SqlFunc.ToString(x.ExpenseAmount).Contains(keywords)
                || SqlFunc.ToString(x.TaxRate).Contains(keywords)
                || SqlFunc.ToString(x.TaxAmount).Contains(keywords)
                || (x.ApplicationReason != null && x.ApplicationReason.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || SqlFunc.ToString(x.ExpenseStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ExpenseDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ExpenseCode))
        {
            exp = exp.And(x => x.ExpenseCode != null && x.ExpenseCode.Contains(queryDto.ExpenseCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExpenseTitle))
        {
            exp = exp.And(x => x.ExpenseTitle != null && x.ExpenseTitle.Contains(queryDto.ExpenseTitle));
        }

        if (queryDto?.ExpenseType.HasValue == true)
        {
            exp = exp.And(x => x.ExpenseType == queryDto.ExpenseType);
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierCode))
        {
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(queryDto.SupplierCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierName))
        {
            exp = exp.And(x => x.SupplierName != null && x.SupplierName.Contains(queryDto.SupplierName));
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

        if (!string.IsNullOrEmpty(queryDto?.CostCenter))
        {
            exp = exp.And(x => x.CostCenter != null && x.CostCenter.Contains(queryDto.CostCenter));
        }

        if (queryDto?.CountersignId.HasValue == true)
        {
            exp = exp.And(x => x.CountersignId == queryDto.CountersignId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseOrderCode))
        {
            exp = exp.And(x => x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(queryDto.PurchaseOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseRequestCode))
        {
            exp = exp.And(x => x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(queryDto.PurchaseRequestCode));
        }

        if (queryDto?.ExpenseAmount.HasValue == true)
        {
            exp = exp.And(x => x.ExpenseAmount == queryDto.ExpenseAmount);
        }

        if (queryDto?.TaxRate.HasValue == true)
        {
            exp = exp.And(x => x.TaxRate == queryDto.TaxRate);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            exp = exp.And(x => x.TaxAmount == queryDto.TaxAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.ApplicationReason))
        {
            exp = exp.And(x => x.ApplicationReason != null && x.ApplicationReason.Contains(queryDto.ApplicationReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (queryDto?.ExpenseStatus.HasValue == true)
        {
            exp = exp.And(x => x.ExpenseStatus == queryDto.ExpenseStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ExpenseDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpenseDate >= queryDto.ExpenseDateStart);
        }

        if (queryDto?.ExpenseDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpenseDate <= queryDto.ExpenseDateEnd);
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
