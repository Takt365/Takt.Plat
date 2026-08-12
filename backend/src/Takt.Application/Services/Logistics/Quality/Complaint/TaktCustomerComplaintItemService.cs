// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintItemService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉明细应用服务实现
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
/// 客诉明细应用服务
/// </summary>
public class TaktCustomerComplaintItemService : TaktServiceBase, ITaktCustomerComplaintItemService
{
    private readonly ITaktCompanyRepository<TaktCustomerComplaintItem> _customerComplaintItemRepository;
    private readonly ITaktCompanyRepository<TaktCustomerComplaint> _customerComplaintRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerComplaintItemRepository">客诉明细仓储</param>
    /// <param name="customerComplaintRepository">客诉主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerComplaintItemService(
        ITaktCompanyRepository<TaktCustomerComplaintItem> customerComplaintItemRepository,
        ITaktCompanyRepository<TaktCustomerComplaint> customerComplaintRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerComplaintItemRepository = customerComplaintItemRepository;
        _customerComplaintRepository = customerComplaintRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客诉明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerComplaintItemDto>> GetCustomerComplaintItemListAsync(TaktCustomerComplaintItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerComplaintItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerComplaintItemDto>.Create(
            data.Adapt<List<TaktCustomerComplaintItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客诉明细
    /// </summary>
    /// <param name="id">客诉明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintItemDto?> GetCustomerComplaintItemByIdAsync(long id)
    {
        var entity = await _customerComplaintItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCustomerComplaintItemDto>();
    }

    /// <summary>
    /// 获取客诉明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerComplaintItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _customerComplaintItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ImprovementStatus == 1 && x.IsObsolete == 0,
            x => x.ProductName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CustomerComplaintCode,
            DictLabel = e.ProductName ?? e.CustomerComplaintCode,
        }).ToList();
    }

    /// <summary>
    /// 创建客诉明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintItemDto> CreateCustomerComplaintItemAsync(TaktCustomerComplaintItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerComplaintItem>();
        entity.IsObsolete = 0;
        await StampCustomerComplaintItemCustomerComplaintAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_customer_complaint_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _customerComplaintItemRepository,
            x => x.ComplaintId == entity.ComplaintId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_customer_complaint_item_line_unique)
        {
            throw new TaktBusinessException("客诉明细的ComplaintId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _customerComplaintItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ComplaintId == entity.ComplaintId,
                x => x.LineNumber);
            var businessCode = entity.ComplaintId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _customerComplaintItemRepository.CreateAsync(entity);
        return await GetCustomerComplaintItemByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerComplaintItemDto>();
    }

    /// <summary>
    /// 更新客诉明细
    /// </summary>
    /// <param name="id">客诉明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintItemDto> UpdateCustomerComplaintItemAsync(long id, TaktCustomerComplaintItemUpdateDto dto)
    {
        var entity = await _customerComplaintItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉明细不存在");
        }
        dto.Adapt(entity);
        await StampCustomerComplaintItemCustomerComplaintAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_customer_complaint_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _customerComplaintItemRepository,
            x => x.ComplaintId == entity.ComplaintId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_customer_complaint_item_line_unique)
        {
            throw new TaktBusinessException("客诉明细的ComplaintId、LineNumber已存在");
        }
        await _customerComplaintItemRepository.UpdateAsync(entity);
        return await GetCustomerComplaintItemByIdAsync(id) ?? throw new TaktBusinessException("客诉明细不存在");
    }

    /// <summary>
    /// 删除客诉明细
    /// </summary>
    /// <param name="id">客诉明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerComplaintItemByIdAsync(long id)
    {
        var entity = await _customerComplaintItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("客诉明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("客诉明细已作废");
        }
        entity.IsObsolete = 1;
        await _customerComplaintItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除客诉明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerComplaintItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerComplaintItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客诉明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintItemDto> UpdateCustomerComplaintItemStatusAsync(TaktCustomerComplaintItemStatusDto dto)
    {
        var entity = await _customerComplaintItemRepository.GetByIdAsync(dto.CustomerComplaintItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉明细不存在");
        }
        entity.ImprovementStatus = dto.ImprovementStatus;
        await _customerComplaintItemRepository.UpdateAsync(entity);
        return await GetCustomerComplaintItemByIdAsync(dto.CustomerComplaintItemId) ?? throw new TaktBusinessException("客诉明细不存在");
    }

    /// <summary>
    /// 更新客诉明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerComplaintItemDto> UpdateCustomerComplaintItemObsoleteAsync(TaktCustomerComplaintItemObsoleteDto dto)
    {
        var entity = await _customerComplaintItemRepository.GetByIdAsync(dto.CustomerComplaintItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("客诉明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("客诉明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _customerComplaintItemRepository.UpdateAsync(entity);
        return await GetCustomerComplaintItemByIdAsync(dto.CustomerComplaintItemId) ?? throw new TaktBusinessException("客诉明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerComplaintItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerComplaintItemTemplateDto>(
            sheetName ?? "客诉明细导入模板",
            fileName ?? "客诉明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入客诉明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerComplaintItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerComplaintItemImportDto>(fileStream, sheetName ?? "客诉明细导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerComplaintItem>();
                var importDto = rows[i].Adapt<TaktCustomerComplaintItemCreateDto>();
                await StampCustomerComplaintItemCustomerComplaintAsync(entity, importDto);
                var importKey = $"{entity.ComplaintId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ComplaintId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_customer_complaint_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerComplaintItemRepository,
                    x => x.ComplaintId == entity.ComplaintId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_customer_complaint_item_line_unique)
                {
                    throw new TaktBusinessException("客诉明细的ComplaintId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _customerComplaintItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ComplaintId == entity.ComplaintId,
                        x => x.LineNumber);
                    var businessCode = entity.ComplaintId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _customerComplaintItemRepository.CreateAsync(entity);
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
    /// 导出客诉明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerComplaintItemAsync(TaktCustomerComplaintItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCustomerComplaintItemQueryDto());
        var list = await _customerComplaintItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerComplaintItemExportDto>(),
                sheetName ?? "客诉明细数据",
                fileName ?? "客诉明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerComplaintItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客诉明细数据",
            fileName ?? "客诉明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步客诉明细主表外键（ManyToOne → 客诉主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampCustomerComplaintItemCustomerComplaintAsync(TaktCustomerComplaintItem entity, TaktCustomerComplaintItemCreateDto dto)
    {
        if (dto.ComplaintId <= 0)
        {
            return;
        }
        var master = await _customerComplaintRepository.GetByIdAsync(dto.ComplaintId);
        if (master == null)
        {
            throw new TaktBusinessException("客诉主不存在");
        }
        entity.ComplaintId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客诉明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerComplaintItem, bool>> QueryExpression(TaktCustomerComplaintItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerComplaintItem>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ComplaintId).Contains(keywords)
                || (x.CustomerComplaintCode != null && x.CustomerComplaintCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.ProductCode != null && x.ProductCode.Contains(keywords))
                || (x.ProductName != null && x.ProductName.Contains(keywords))
                || (x.BatchCode != null && x.BatchCode.Contains(keywords))
                || SqlFunc.ToString(x.ItemType).Contains(keywords)
                || (x.DefectDescription != null && x.DefectDescription.Contains(keywords))
                || (x.DefectLevel != null && x.DefectLevel.Contains(keywords))
                || SqlFunc.ToString(x.DefectQuantity).Contains(keywords)
                || SqlFunc.ToString(x.DefectRate).Contains(keywords)
                || (x.CauseAnalysis != null && x.CauseAnalysis.Contains(keywords))
                || (x.ImprovementAction != null && x.ImprovementAction.Contains(keywords))
                || (x.ImprovementResponsible != null && x.ImprovementResponsible.Contains(keywords))
                || (x.AttachmentPaths != null && x.AttachmentPaths.Contains(keywords))
                || SqlFunc.ToString(x.ImprovementStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlannedCompletionDate).Contains(keywords)
                || SqlFunc.ToString(x.ActualCompletionDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ComplaintId.HasValue == true)
        {
            exp = exp.And(x => x.ComplaintId == queryDto.ComplaintId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerComplaintCode))
        {
            exp = exp.And(x => x.CustomerComplaintCode != null && x.CustomerComplaintCode.Contains(queryDto.CustomerComplaintCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductCode))
        {
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(queryDto.ProductCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductName))
        {
            exp = exp.And(x => x.ProductName != null && x.ProductName.Contains(queryDto.ProductName));
        }

        if (!string.IsNullOrEmpty(queryDto?.BatchCode))
        {
            exp = exp.And(x => x.BatchCode != null && x.BatchCode.Contains(queryDto.BatchCode));
        }

        if (queryDto?.ItemType.HasValue == true)
        {
            exp = exp.And(x => x.ItemType == queryDto.ItemType);
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectDescription))
        {
            exp = exp.And(x => x.DefectDescription != null && x.DefectDescription.Contains(queryDto.DefectDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.DefectLevel))
        {
            exp = exp.And(x => x.DefectLevel != null && x.DefectLevel.Contains(queryDto.DefectLevel));
        }

        if (queryDto?.DefectQuantity.HasValue == true)
        {
            exp = exp.And(x => x.DefectQuantity == queryDto.DefectQuantity);
        }

        if (queryDto?.DefectRate.HasValue == true)
        {
            exp = exp.And(x => x.DefectRate == queryDto.DefectRate);
        }

        if (!string.IsNullOrEmpty(queryDto?.CauseAnalysis))
        {
            exp = exp.And(x => x.CauseAnalysis != null && x.CauseAnalysis.Contains(queryDto.CauseAnalysis));
        }

        if (!string.IsNullOrEmpty(queryDto?.ImprovementAction))
        {
            exp = exp.And(x => x.ImprovementAction != null && x.ImprovementAction.Contains(queryDto.ImprovementAction));
        }

        if (!string.IsNullOrEmpty(queryDto?.ImprovementResponsible))
        {
            exp = exp.And(x => x.ImprovementResponsible != null && x.ImprovementResponsible.Contains(queryDto.ImprovementResponsible));
        }

        if (!string.IsNullOrEmpty(queryDto?.AttachmentPaths))
        {
            exp = exp.And(x => x.AttachmentPaths != null && x.AttachmentPaths.Contains(queryDto.AttachmentPaths));
        }

        if (queryDto?.ImprovementStatus.HasValue == true)
        {
            exp = exp.And(x => x.ImprovementStatus == queryDto.ImprovementStatus);
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

        if (queryDto?.PlannedCompletionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedCompletionDate >= queryDto.PlannedCompletionDateStart);
        }

        if (queryDto?.PlannedCompletionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedCompletionDate <= queryDto.PlannedCompletionDateEnd);
        }

        if (queryDto?.ActualCompletionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualCompletionDate >= queryDto.ActualCompletionDateStart);
        }

        if (queryDto?.ActualCompletionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualCompletionDate <= queryDto.ActualCompletionDateEnd);
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
