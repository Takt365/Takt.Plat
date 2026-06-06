// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Complaint
// 文件名称：TaktCustomerSatisfactionSurveyService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：客户满意度调查应用服务实现
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
/// 客户满意度调查应用服务
/// </summary>
public class TaktCustomerSatisfactionSurveyService : TaktServiceBase, ITaktCustomerSatisfactionSurveyService
{
    private readonly ITaktCompanyRepository<TaktCustomerSatisfactionSurvey> _customerSatisfactionSurveyRepository;
    private readonly ITaktCompanyRepository<TaktCustomerSatisfactionSurveyItem> _customerSatisfactionSurveyItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerSatisfactionSurveyRepository">客户满意度调查仓储</param>
    /// <param name="customerSatisfactionSurveyItemRepository">CustomerSatisfactionSurveyItem仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerSatisfactionSurveyService(
        ITaktCompanyRepository<TaktCustomerSatisfactionSurvey> customerSatisfactionSurveyRepository,
        ITaktCompanyRepository<TaktCustomerSatisfactionSurveyItem> customerSatisfactionSurveyItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerSatisfactionSurveyRepository = customerSatisfactionSurveyRepository;
        _customerSatisfactionSurveyItemRepository = customerSatisfactionSurveyItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客户满意度调查列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerSatisfactionSurveyDto>> GetCustomerSatisfactionSurveyListAsync(TaktCustomerSatisfactionSurveyQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerSatisfactionSurveyRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerSatisfactionSurveyDto>.Create(
            data.Adapt<List<TaktCustomerSatisfactionSurveyDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyDto?> GetCustomerSatisfactionSurveyByIdAsync(long id)
    {
        var entity = await _customerSatisfactionSurveyRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCustomerSatisfactionSurveyDto>();
        await FillCustomerSatisfactionSurveyDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取客户满意度调查选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerSatisfactionSurveyOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _customerSatisfactionSurveyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CustomerName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CustomerName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建客户满意度调查
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyDto> CreateCustomerSatisfactionSurveyAsync(TaktCustomerSatisfactionSurveyCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerSatisfactionSurvey>();
        var isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique = await _uniqueValidator.IsUniqueAsync(
            _customerSatisfactionSurveyRepository,
            x => x.CustomerSatisfactionSurveyCode == entity.CustomerSatisfactionSurveyCode);
        if (!isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique)
        {
            throw new TaktBusinessException("客户满意度调查的CustomerSatisfactionSurveyCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerSatisfactionSurveyRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CustomerId == entity.CustomerId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.CustomerId, maxSort);
        }
        entity = await _customerSatisfactionSurveyRepository.CreateAsync(entity);
                await SaveCustomerSatisfactionSurveyChildrenAsync(entity, dto);
        return await GetCustomerSatisfactionSurveyByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerSatisfactionSurveyDto>();
    }

    /// <summary>
    /// 更新客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyDto> UpdateCustomerSatisfactionSurveyAsync(long id, TaktCustomerSatisfactionSurveyUpdateDto dto)
    {
        var entity = await _customerSatisfactionSurveyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique = await _uniqueValidator.IsUniqueAsync(
            _customerSatisfactionSurveyRepository,
            x => x.CustomerSatisfactionSurveyCode == entity.CustomerSatisfactionSurveyCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique)
        {
            throw new TaktBusinessException("客户满意度调查的CustomerSatisfactionSurveyCode已存在");
        }
        await _customerSatisfactionSurveyRepository.UpdateAsync(entity);
                await SaveCustomerSatisfactionSurveyChildrenAsync(entity, dto);
        return await GetCustomerSatisfactionSurveyByIdAsync(id) ?? throw new TaktBusinessException("客户满意度调查不存在");
    }

    /// <summary>
    /// 删除客户满意度调查
    /// </summary>
    /// <param name="id">客户满意度调查ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerSatisfactionSurveyByIdAsync(long id)
    {
        var entity = await _customerSatisfactionSurveyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查不存在或已删除");
        }
        await _customerSatisfactionSurveyItemRepository.DeleteAsync(x => x.SurveyId == entity.Id);
        var deleted = await _customerSatisfactionSurveyRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("客户满意度调查不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除客户满意度调查
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerSatisfactionSurveyBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerSatisfactionSurveyByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客户满意度调查状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyDto> UpdateCustomerSatisfactionSurveyStatusAsync(TaktCustomerSatisfactionSurveyStatusDto dto)
    {
        var entity = await _customerSatisfactionSurveyRepository.GetByIdAsync(dto.CustomerSatisfactionSurveyId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查不存在");
        }
        entity.SurveyStatus = dto.SurveyStatus;
        await _customerSatisfactionSurveyRepository.UpdateAsync(entity);
        return await GetCustomerSatisfactionSurveyByIdAsync(dto.CustomerSatisfactionSurveyId) ?? throw new TaktBusinessException("客户满意度调查不存在");
    }

    /// <summary>
    /// 更新客户满意度调查排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerSatisfactionSurveyDto> UpdateCustomerSatisfactionSurveySortAsync(TaktCustomerSatisfactionSurveySortDto dto)
    {
        var entity = await _customerSatisfactionSurveyRepository.GetByIdAsync(dto.CustomerSatisfactionSurveyId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户满意度调查不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerSatisfactionSurveyRepository.UpdateAsync(entity);
        return await GetCustomerSatisfactionSurveyByIdAsync(dto.CustomerSatisfactionSurveyId) ?? throw new TaktBusinessException("客户满意度调查不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerSatisfactionSurveyTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerSatisfactionSurveyTemplateDto>(
            sheetName ?? "客户满意度调查导入模板",
            fileName ?? "客户满意度调查导入模板.xlsx");
    }

    /// <summary>
    /// 导入客户满意度调查
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerSatisfactionSurveyAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerSatisfactionSurveyImportDto>(fileStream, sheetName ?? "客户满意度调查导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerSatisfactionSurvey>();
                var importKey = $"{entity.CustomerSatisfactionSurveyCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CustomerSatisfactionSurveyCode）");
                }
                var isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerSatisfactionSurveyRepository,
                    x => x.CustomerSatisfactionSurveyCode == entity.CustomerSatisfactionSurveyCode);
                if (!isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_survey_unique)
                {
                    throw new TaktBusinessException("客户满意度调查的CustomerSatisfactionSurveyCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _customerSatisfactionSurveyRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CustomerId == entity.CustomerId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.CustomerId, maxSort);
                }
                await _customerSatisfactionSurveyRepository.CreateAsync(entity);
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
    /// 导出客户满意度调查
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerSatisfactionSurveyAsync(TaktCustomerSatisfactionSurveyQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCustomerSatisfactionSurveyQueryDto());
        var list = await _customerSatisfactionSurveyRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerSatisfactionSurveyExportDto>(),
                sheetName ?? "客户满意度调查数据",
                fileName ?? "客户满意度调查导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerSatisfactionSurveyExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客户满意度调查数据",
            fileName ?? "客户满意度调查导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充客户满意度调查详情（加载 OneToMany 子表：客户满意度调查项目明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillCustomerSatisfactionSurveyDetailsAsync(TaktCustomerSatisfactionSurveyDto dto, TaktCustomerSatisfactionSurvey entity)
    {
        if (dto == null)
        {
            return;
        }
        // 客户满意度调查项目明细 → dto.Items
        var items = await _customerSatisfactionSurveyItemRepository.GetListAsync(x => x.SurveyId == entity.Id);
        dto.Items = items.Adapt<List<TaktCustomerSatisfactionSurveyItemDto>>();
    }

    /// <summary>
    /// 保存客户满意度调查子表级联（客户满意度调查项目明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveCustomerSatisfactionSurveyChildrenAsync(TaktCustomerSatisfactionSurvey entity, TaktCustomerSatisfactionSurveyCreateDto dto)
    {
        // 客户满意度调查项目明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _customerSatisfactionSurveyItemRepository.DeleteAsync(x => x.SurveyId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktCustomerSatisfactionSurveyItem>>();
            foreach (var child in items)
            {
                child.SurveyId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.CustomerSatisfactionSurveyCode) ? entity.CustomerSatisfactionSurveyCode : entity.Id.ToString();
                var maxLine = await _customerSatisfactionSurveyItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SurveyId == entity.Id,
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
                            var key = $"{items[i].CompanyCode}|{items[i].SurveyId}|{items[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"客户满意度调查项目明细第{i + 1}项与本次提交的其他项重复（CompanyCode、SurveyId、LineNumber）");
                            }
                        }
            await _customerSatisfactionSurveyItemRepository.DeleteAsync(x => x.SurveyId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                _customerSatisfactionSurveyItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.SurveyId == child.SurveyId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_quality_customer_satisfaction_survey_item_line_unique)
            {
                throw new TaktBusinessException("客户满意度调查项目明细的CompanyCode、SurveyId、LineNumber已存在");
            }
            }
            await _customerSatisfactionSurveyItemRepository.CreateRangeAsync(items);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客户满意度调查查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomerSatisfactionSurvey, bool>> QueryExpression(TaktCustomerSatisfactionSurveyQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerSatisfactionSurvey>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CustomerSatisfactionSurveyCode != null && x.CustomerSatisfactionSurveyCode.Contains(keywords))
                || SqlFunc.ToString(x.CustomerId).Contains(keywords)
                || (x.CustomerName != null && x.CustomerName.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || SqlFunc.ToString(x.SurveyMethod).Contains(keywords)
                || SqlFunc.ToString(x.SurveyType).Contains(keywords)
                || SqlFunc.ToString(x.SurveyPeriod).Contains(keywords)
                || (x.SurveyorBy != null && x.SurveyorBy.Contains(keywords))
                || (x.CustomerContact != null && x.CustomerContact.Contains(keywords))
                || (x.CustomerPhone != null && x.CustomerPhone.Contains(keywords))
                || SqlFunc.ToString(x.OverallSatisfaction).Contains(keywords)
                || SqlFunc.ToString(x.TotalScore).Contains(keywords)
                || SqlFunc.ToString(x.QualityScore).Contains(keywords)
                || SqlFunc.ToString(x.DeliveryScore).Contains(keywords)
                || SqlFunc.ToString(x.ServiceScore).Contains(keywords)
                || SqlFunc.ToString(x.PriceScore).Contains(keywords)
                || SqlFunc.ToString(x.TechnicalScore).Contains(keywords)
                || (x.CustomerPraise != null && x.CustomerPraise.Contains(keywords))
                || (x.CustomerFeedback != null && x.CustomerFeedback.Contains(keywords))
                || (x.ImprovementPlan != null && x.ImprovementPlan.Contains(keywords))
                || SqlFunc.ToString(x.SurveyStatus).Contains(keywords)
                || SqlFunc.ToString(x.FollowUpStatus).Contains(keywords)
                || SqlFunc.ToString(x.RelatedComplaintId).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.SurveyDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerSatisfactionSurveyCode))
        {
            exp = exp.And(x => x.CustomerSatisfactionSurveyCode != null && x.CustomerSatisfactionSurveyCode.Contains(queryDto.CustomerSatisfactionSurveyCode));
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

        if (queryDto?.SurveyMethod.HasValue == true)
        {
            exp = exp.And(x => x.SurveyMethod == queryDto.SurveyMethod);
        }

        if (queryDto?.SurveyType.HasValue == true)
        {
            exp = exp.And(x => x.SurveyType == queryDto.SurveyType);
        }

        if (queryDto?.SurveyPeriod.HasValue == true)
        {
            exp = exp.And(x => x.SurveyPeriod == queryDto.SurveyPeriod);
        }

        if (!string.IsNullOrEmpty(queryDto?.SurveyorBy))
        {
            exp = exp.And(x => x.SurveyorBy != null && x.SurveyorBy.Contains(queryDto.SurveyorBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerContact))
        {
            exp = exp.And(x => x.CustomerContact != null && x.CustomerContact.Contains(queryDto.CustomerContact));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerPhone))
        {
            exp = exp.And(x => x.CustomerPhone != null && x.CustomerPhone.Contains(queryDto.CustomerPhone));
        }

        if (queryDto?.OverallSatisfaction.HasValue == true)
        {
            exp = exp.And(x => x.OverallSatisfaction == queryDto.OverallSatisfaction);
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

        if (queryDto?.ServiceScore.HasValue == true)
        {
            exp = exp.And(x => x.ServiceScore == queryDto.ServiceScore);
        }

        if (queryDto?.PriceScore.HasValue == true)
        {
            exp = exp.And(x => x.PriceScore == queryDto.PriceScore);
        }

        if (queryDto?.TechnicalScore.HasValue == true)
        {
            exp = exp.And(x => x.TechnicalScore == queryDto.TechnicalScore);
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerPraise))
        {
            exp = exp.And(x => x.CustomerPraise != null && x.CustomerPraise.Contains(queryDto.CustomerPraise));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerFeedback))
        {
            exp = exp.And(x => x.CustomerFeedback != null && x.CustomerFeedback.Contains(queryDto.CustomerFeedback));
        }

        if (!string.IsNullOrEmpty(queryDto?.ImprovementPlan))
        {
            exp = exp.And(x => x.ImprovementPlan != null && x.ImprovementPlan.Contains(queryDto.ImprovementPlan));
        }

        if (queryDto?.SurveyStatus.HasValue == true)
        {
            exp = exp.And(x => x.SurveyStatus == queryDto.SurveyStatus);
        }

        if (queryDto?.FollowUpStatus.HasValue == true)
        {
            exp = exp.And(x => x.FollowUpStatus == queryDto.FollowUpStatus);
        }

        if (queryDto?.RelatedComplaintId.HasValue == true)
        {
            exp = exp.And(x => x.RelatedComplaintId == queryDto.RelatedComplaintId);
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

        if (queryDto?.SurveyDateStart.HasValue == true)
        {
            exp = exp.And(x => x.SurveyDate >= queryDto.SurveyDateStart);
        }

        if (queryDto?.SurveyDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.SurveyDate <= queryDto.SurveyDateEnd);
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
