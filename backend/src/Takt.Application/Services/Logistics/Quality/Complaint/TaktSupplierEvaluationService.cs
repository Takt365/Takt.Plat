// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktSupplierEvaluationService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：供应商评价考核应用服务实现
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
using Takt.Domain.Entities.Logistics.Quality.Complaint;

namespace Takt.Application.Services.Logistics.Quality.Complaint;

/// <summary>
/// 供应商评价考核应用服务
/// </summary>
public class TaktSupplierEvaluationService : TaktServiceBase, ITaktSupplierEvaluationService
{
    private readonly ITaktCompanyRepository<TaktSupplierEvaluation> _supplierEvaluationRepository;
    private readonly ITaktCompanyRepository<TaktSupplierEvaluationItem> _supplierEvaluationItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="supplierEvaluationRepository">供应商评价考核仓储</param>
    /// <param name="supplierEvaluationItemRepository">SupplierEvaluationItem仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSupplierEvaluationService(
        ITaktCompanyRepository<TaktSupplierEvaluation> supplierEvaluationRepository,
        ITaktCompanyRepository<TaktSupplierEvaluationItem> supplierEvaluationItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _supplierEvaluationRepository = supplierEvaluationRepository;
        _supplierEvaluationItemRepository = supplierEvaluationItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取供应商评价考核列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSupplierEvaluationDto>> GetSupplierEvaluationListAsync(TaktSupplierEvaluationQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _supplierEvaluationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSupplierEvaluationDto>.Create(
            data.Adapt<List<TaktSupplierEvaluationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取供应商评价考核
    /// </summary>
    /// <param name="id">供应商评价考核ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationDto?> GetSupplierEvaluationByIdAsync(long id)
    {
        var entity = await _supplierEvaluationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSupplierEvaluationDto>();
        await FillSupplierEvaluationDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取供应商评价考核选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSupplierEvaluationOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _supplierEvaluationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SupplierName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SupplierName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建供应商评价考核
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationDto> CreateSupplierEvaluationAsync(TaktSupplierEvaluationCreateDto dto)
    {
        var entity = dto.Adapt<TaktSupplierEvaluation>();
        var isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierEvaluationRepository,
            x => x.SupplierEvaluationCode == entity.SupplierEvaluationCode);
        if (!isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique)
        {
            throw new TaktBusinessException("供应商评价考核的SupplierEvaluationCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _supplierEvaluationRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SupplierId == entity.SupplierId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.SupplierId, maxSort);
        }
        entity = await _supplierEvaluationRepository.CreateAsync(entity);
                await SaveSupplierEvaluationChildrenAsync(entity, dto);
        return await GetSupplierEvaluationByIdAsync(entity.Id) ?? entity.Adapt<TaktSupplierEvaluationDto>();
    }

    /// <summary>
    /// 更新供应商评价考核
    /// </summary>
    /// <param name="id">供应商评价考核ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationDto> UpdateSupplierEvaluationAsync(long id, TaktSupplierEvaluationUpdateDto dto)
    {
        var entity = await _supplierEvaluationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique = await _uniqueValidator.IsUniqueAsync(
            _supplierEvaluationRepository,
            x => x.SupplierEvaluationCode == entity.SupplierEvaluationCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique)
        {
            throw new TaktBusinessException("供应商评价考核的SupplierEvaluationCode已存在");
        }
        await _supplierEvaluationRepository.UpdateAsync(entity);
                await SaveSupplierEvaluationChildrenAsync(entity, dto);
        return await GetSupplierEvaluationByIdAsync(id) ?? throw new TaktBusinessException("供应商评价考核不存在");
    }

    /// <summary>
    /// 删除供应商评价考核
    /// </summary>
    /// <param name="id">供应商评价考核ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSupplierEvaluationByIdAsync(long id)
    {
        var entity = await _supplierEvaluationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核不存在或已删除");
        }
        await _supplierEvaluationItemRepository.DeleteAsync(x => x.EvaluationId == entity.Id);
        var deleted = await _supplierEvaluationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("供应商评价考核不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除供应商评价考核
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSupplierEvaluationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSupplierEvaluationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新供应商评价考核状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationDto> UpdateSupplierEvaluationStatusAsync(TaktSupplierEvaluationStatusDto dto)
    {
        var entity = await _supplierEvaluationRepository.GetByIdAsync(dto.SupplierEvaluationId);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核不存在");
        }
        entity.EvaluationStatus = dto.EvaluationStatus;
        await _supplierEvaluationRepository.UpdateAsync(entity);
        return await GetSupplierEvaluationByIdAsync(dto.SupplierEvaluationId) ?? throw new TaktBusinessException("供应商评价考核不存在");
    }

    /// <summary>
    /// 更新供应商评价考核排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSupplierEvaluationDto> UpdateSupplierEvaluationSortAsync(TaktSupplierEvaluationSortDto dto)
    {
        var entity = await _supplierEvaluationRepository.GetByIdAsync(dto.SupplierEvaluationId);
        if (entity == null)
        {
            throw new TaktBusinessException("供应商评价考核不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _supplierEvaluationRepository.UpdateAsync(entity);
        return await GetSupplierEvaluationByIdAsync(dto.SupplierEvaluationId) ?? throw new TaktBusinessException("供应商评价考核不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSupplierEvaluationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSupplierEvaluationTemplateDto>(
            sheetName ?? "供应商评价考核导入模板",
            fileName ?? "供应商评价考核导入模板.xlsx");
    }

    /// <summary>
    /// 导入供应商评价考核
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSupplierEvaluationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSupplierEvaluationImportDto>(fileStream, sheetName ?? "供应商评价考核导入模板");
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
                var entity = rows[i].Adapt<TaktSupplierEvaluation>();
                var importKey = $"{entity.SupplierEvaluationCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SupplierEvaluationCode）");
                }
                var isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique = await _uniqueValidator.IsUniqueAsync(
                    _supplierEvaluationRepository,
                    x => x.SupplierEvaluationCode == entity.SupplierEvaluationCode);
                if (!isUnique_ix_takt_logistics_quality_supplier_evaluation_evaluation_unique)
                {
                    throw new TaktBusinessException("供应商评价考核的SupplierEvaluationCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _supplierEvaluationRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SupplierId == entity.SupplierId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.SupplierId, maxSort);
                }
                await _supplierEvaluationRepository.CreateAsync(entity);
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
    /// 导出供应商评价考核
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSupplierEvaluationAsync(TaktSupplierEvaluationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSupplierEvaluationQueryDto());
        var list = await _supplierEvaluationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSupplierEvaluationExportDto>(),
                sheetName ?? "供应商评价考核数据",
                fileName ?? "供应商评价考核导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSupplierEvaluationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "供应商评价考核数据",
            fileName ?? "供应商评价考核导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充供应商评价考核详情（加载 OneToMany 子表：供应商评价考核项目明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSupplierEvaluationDetailsAsync(TaktSupplierEvaluationDto dto, TaktSupplierEvaluation entity)
    {
        if (dto == null)
        {
            return;
        }
        // 供应商评价考核项目明细 → dto.Items
        var items = await _supplierEvaluationItemRepository.GetListAsync(x => x.EvaluationId == entity.Id);
        dto.Items = items.Adapt<List<TaktSupplierEvaluationItemDto>>();
    }

    /// <summary>
    /// 保存供应商评价考核子表级联（供应商评价考核项目明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSupplierEvaluationChildrenAsync(TaktSupplierEvaluation entity, TaktSupplierEvaluationCreateDto dto)
    {
        // 供应商评价考核项目明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _supplierEvaluationItemRepository.DeleteAsync(x => x.EvaluationId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktSupplierEvaluationItem>>();
            foreach (var child in items)
            {
                child.EvaluationId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.SupplierEvaluationCode) ? entity.SupplierEvaluationCode : entity.Id.ToString();
                var maxLine = await _supplierEvaluationItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EvaluationId == entity.Id,
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
                            var key = $"{items[i].CompanyCode}|{items[i].EvaluationId}|{items[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"供应商评价考核项目明细第{i + 1}项与本次提交的其他项重复（CompanyCode、EvaluationId、LineNumber）");
                            }
                        }
            await _supplierEvaluationItemRepository.DeleteAsync(x => x.EvaluationId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_quality_supplier_evaluation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                _supplierEvaluationItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.EvaluationId == child.EvaluationId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_supplier_evaluation_item_line_unique)
            {
                throw new TaktBusinessException("供应商评价考核项目明细的CompanyCode、EvaluationId、LineNumber已存在");
            }
            }
            await _supplierEvaluationItemRepository.CreateRangeAsync(items);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建供应商评价考核查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSupplierEvaluation, bool>> QueryExpression(TaktSupplierEvaluationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSupplierEvaluation>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.SupplierEvaluationCode != null && x.SupplierEvaluationCode.Contains(keywords))
                || SqlFunc.ToString(x.SupplierId).Contains(keywords)
                || (x.SupplierName != null && x.SupplierName.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || SqlFunc.ToString(x.EvaluationPeriod).Contains(keywords)
                || SqlFunc.ToString(x.EvaluationType).Contains(keywords)
                || (x.EvaluatorBy != null && x.EvaluatorBy.Contains(keywords))
                || (x.EvaluationDept != null && x.EvaluationDept.Contains(keywords))
                || SqlFunc.ToString(x.OverallRating).Contains(keywords)
                || SqlFunc.ToString(x.TotalScore).Contains(keywords)
                || SqlFunc.ToString(x.QualityScore).Contains(keywords)
                || SqlFunc.ToString(x.DeliveryScore).Contains(keywords)
                || SqlFunc.ToString(x.PriceScore).Contains(keywords)
                || SqlFunc.ToString(x.ServiceScore).Contains(keywords)
                || SqlFunc.ToString(x.TechnicalScore).Contains(keywords)
                || (x.MainStrengths != null && x.MainStrengths.Contains(keywords))
                || (x.MainIssues != null && x.MainIssues.Contains(keywords))
                || (x.ImprovementRequirements != null && x.ImprovementRequirements.Contains(keywords))
                || SqlFunc.ToString(x.EvaluationConclusion).Contains(keywords)
                || SqlFunc.ToString(x.EvaluationStatus).Contains(keywords)
                || SqlFunc.ToString(x.RectificationStatus).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EvaluationDate).Contains(keywords)
                || SqlFunc.ToString(x.RectificationDeadline).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierEvaluationCode))
        {
            exp = exp.And(x => x.SupplierEvaluationCode != null && x.SupplierEvaluationCode.Contains(queryDto.SupplierEvaluationCode));
        }

        if (queryDto?.SupplierId.HasValue == true)
        {
            exp = exp.And(x => x.SupplierId == queryDto.SupplierId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierName))
        {
            exp = exp.And(x => x.SupplierName != null && x.SupplierName.Contains(queryDto.SupplierName));
        }

        if (!string.IsNullOrEmpty(queryDto?.SupplierCode))
        {
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(queryDto.SupplierCode));
        }

        if (queryDto?.EvaluationPeriod.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationPeriod == queryDto.EvaluationPeriod);
        }

        if (queryDto?.EvaluationType.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationType == queryDto.EvaluationType);
        }

        if (!string.IsNullOrEmpty(queryDto?.EvaluatorBy))
        {
            exp = exp.And(x => x.EvaluatorBy != null && x.EvaluatorBy.Contains(queryDto.EvaluatorBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.EvaluationDept))
        {
            exp = exp.And(x => x.EvaluationDept != null && x.EvaluationDept.Contains(queryDto.EvaluationDept));
        }

        if (queryDto?.OverallRating.HasValue == true)
        {
            exp = exp.And(x => x.OverallRating == queryDto.OverallRating);
        }

        if (queryDto?.TotalScore.HasValue == true)
        {
            exp = exp.And(x => x.TotalScore == queryDto.TotalScore);
        }

        if (queryDto?.QualityScore.HasValue == true)
        {
            exp = exp.And(x => x.QualityScore == queryDto.QualityScore);
        }

        if (queryDto?.DeliveryScore.HasValue == true)
        {
            exp = exp.And(x => x.DeliveryScore == queryDto.DeliveryScore);
        }

        if (queryDto?.PriceScore.HasValue == true)
        {
            exp = exp.And(x => x.PriceScore == queryDto.PriceScore);
        }

        if (queryDto?.ServiceScore.HasValue == true)
        {
            exp = exp.And(x => x.ServiceScore == queryDto.ServiceScore);
        }

        if (queryDto?.TechnicalScore.HasValue == true)
        {
            exp = exp.And(x => x.TechnicalScore == queryDto.TechnicalScore);
        }

        if (!string.IsNullOrEmpty(queryDto?.MainStrengths))
        {
            exp = exp.And(x => x.MainStrengths != null && x.MainStrengths.Contains(queryDto.MainStrengths));
        }

        if (!string.IsNullOrEmpty(queryDto?.MainIssues))
        {
            exp = exp.And(x => x.MainIssues != null && x.MainIssues.Contains(queryDto.MainIssues));
        }

        if (!string.IsNullOrEmpty(queryDto?.ImprovementRequirements))
        {
            exp = exp.And(x => x.ImprovementRequirements != null && x.ImprovementRequirements.Contains(queryDto.ImprovementRequirements));
        }

        if (queryDto?.EvaluationConclusion.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationConclusion == queryDto.EvaluationConclusion);
        }

        if (queryDto?.EvaluationStatus.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationStatus == queryDto.EvaluationStatus);
        }

        if (queryDto?.RectificationStatus.HasValue == true)
        {
            exp = exp.And(x => x.RectificationStatus == queryDto.RectificationStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EvaluationDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationDate >= queryDto.EvaluationDateStart);
        }

        if (queryDto?.EvaluationDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationDate <= queryDto.EvaluationDateEnd);
        }

        if (queryDto?.RectificationDeadlineStart.HasValue == true)
        {
            exp = exp.And(x => x.RectificationDeadline >= queryDto.RectificationDeadlineStart);
        }

        if (queryDto?.RectificationDeadlineEnd.HasValue == true)
        {
            exp = exp.And(x => x.RectificationDeadline <= queryDto.RectificationDeadlineEnd);
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
