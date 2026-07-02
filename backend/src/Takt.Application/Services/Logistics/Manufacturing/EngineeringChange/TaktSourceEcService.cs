// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcService.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变来源主应用服务
/// </summary>
public class TaktSourceEcService : TaktServiceBase, ITaktSourceEcService
{
    private readonly ITaktCompanyRepository<TaktSourceEc> _sourceEcRepository;
    private readonly ITaktCompanyRepository<TaktSourceEcDetail> _sourceEcDetailRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sourceEcRepository">设变来源主仓储</param>
    /// <param name="sourceEcDetailRepository">SourceEcDetail仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSourceEcService(
        ITaktCompanyRepository<TaktSourceEc> sourceEcRepository,
        ITaktCompanyRepository<TaktSourceEcDetail> sourceEcDetailRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sourceEcRepository = sourceEcRepository;
        _sourceEcDetailRepository = sourceEcDetailRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变来源主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSourceEcDto>> GetSourceEcListAsync(TaktSourceEcQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sourceEcRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSourceEcDto>.Create(
            data.Adapt<List<TaktSourceEcDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变来源主
    /// </summary>
    /// <param name="id">设变来源主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDto?> GetSourceEcByIdAsync(long id)
    {
        var entity = await _sourceEcRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSourceEcDto>();
        await FillSourceEcDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取设变来源主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSourceEcOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _sourceEcRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SourceEcNo ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SourceEcNo ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变来源主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDto> CreateSourceEcAsync(TaktSourceEcCreateDto dto)
    {
        var entity = dto.Adapt<TaktSourceEc>();
        var isUnique_ix_source_ec_no_unique = await _uniqueValidator.IsUniqueAsync(
            _sourceEcRepository,
            x => x.SourceEcNo == entity.SourceEcNo);
        if (!isUnique_ix_source_ec_no_unique)
        {
            throw new TaktBusinessException("设变来源主的SourceEcNo已存在");
        }
        entity = await _sourceEcRepository.CreateAsync(entity);
                await SaveSourceEcChildrenAsync(entity, dto);
        return await GetSourceEcByIdAsync(entity.Id) ?? entity.Adapt<TaktSourceEcDto>();
    }

    /// <summary>
    /// 更新设变来源主
    /// </summary>
    /// <param name="id">设变来源主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDto> UpdateSourceEcAsync(long id, TaktSourceEcUpdateDto dto)
    {
        var entity = await _sourceEcRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变来源主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_source_ec_no_unique = await _uniqueValidator.IsUniqueAsync(
            _sourceEcRepository,
            x => x.SourceEcNo == entity.SourceEcNo,
            id);
        if (!isUnique_ix_source_ec_no_unique)
        {
            throw new TaktBusinessException("设变来源主的SourceEcNo已存在");
        }
        await _sourceEcRepository.UpdateAsync(entity);
                await SaveSourceEcChildrenAsync(entity, dto);
        return await GetSourceEcByIdAsync(id) ?? throw new TaktBusinessException("设变来源主不存在");
    }

    /// <summary>
    /// 删除设变来源主
    /// </summary>
    /// <param name="id">设变来源主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSourceEcByIdAsync(long id)
    {
        var entity = await _sourceEcRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变来源主不存在或已删除");
        }
        await _sourceEcDetailRepository.DeleteAsync(x => x.SourceEcId == entity.Id);
        var deleted = await _sourceEcRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设变来源主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设变来源主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSourceEcBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSourceEcByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变来源主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDto> UpdateSourceEcStatusAsync(TaktSourceEcStatusDto dto)
    {
        var entity = await _sourceEcRepository.GetByIdAsync(dto.SourceEcId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变来源主不存在");
        }
        entity.SourceStatus = dto.SourceStatus;
        await _sourceEcRepository.UpdateAsync(entity);
        return await GetSourceEcByIdAsync(dto.SourceEcId) ?? throw new TaktBusinessException("设变来源主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSourceEcTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSourceEcTemplateDto>(
            sheetName ?? "设变来源主导入模板",
            fileName ?? "设变来源主导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变来源主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSourceEcAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSourceEcImportDto>(fileStream, sheetName ?? "设变来源主导入模板");
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
                var entity = rows[i].Adapt<TaktSourceEc>();
                var importKey = $"{entity.SourceEcNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SourceEcNo）");
                }
                var isUnique_ix_source_ec_no_unique = await _uniqueValidator.IsUniqueAsync(
                    _sourceEcRepository,
                    x => x.SourceEcNo == entity.SourceEcNo);
                if (!isUnique_ix_source_ec_no_unique)
                {
                    throw new TaktBusinessException("设变来源主的SourceEcNo已存在");
                }
                await _sourceEcRepository.CreateAsync(entity);
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
    /// 导出设变来源主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSourceEcAsync(TaktSourceEcQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSourceEcQueryDto());
        var list = await _sourceEcRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSourceEcExportDto>(),
                sheetName ?? "设变来源主数据",
                fileName ?? "设变来源主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSourceEcExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变来源主数据",
            fileName ?? "设变来源主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充设变来源主详情（加载 OneToMany 子表：设变来源子）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSourceEcDetailsAsync(TaktSourceEcDto dto, TaktSourceEc entity)
    {
        if (dto == null)
        {
            return;
        }
        // 设变来源子 → dto.SourceEcDetails
        var sourceecdetails = await _sourceEcDetailRepository.GetListAsync(x => x.SourceEcId == entity.Id);
        dto.SourceEcDetails = sourceecdetails.Adapt<List<TaktSourceEcDetailDto>>();
    }

    /// <summary>
    /// 保存设变来源主子表级联（设变来源子；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSourceEcChildrenAsync(TaktSourceEc entity, TaktSourceEcCreateDto dto)
    {
        // 设变来源子（SourceEcDetails）
        if (dto.SourceEcDetails is not { Count: > 0 })
        {
            await _sourceEcDetailRepository.DeleteAsync(x => x.SourceEcId == entity.Id);
        }
        else
        {
            var sourceecdetails = dto.SourceEcDetails.Adapt<List<TaktSourceEcDetail>>();
            foreach (var child in sourceecdetails)
            {
                child.SourceEcId = entity.Id;
            }
            await _sourceEcDetailRepository.DeleteAsync(x => x.SourceEcId == entity.Id);
            foreach (var child in sourceecdetails)
            {
            }
            await _sourceEcDetailRepository.CreateRangeAsync(sourceecdetails);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变来源主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSourceEc, bool>> QueryExpression(TaktSourceEcQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSourceEc>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.SourceEcNo != null && x.SourceEcNo.Contains(keywords))
                || (x.SourceModel != null && x.SourceModel.Contains(keywords))
                || (x.SourceTitle != null && x.SourceTitle.Contains(keywords))
                || (x.SourceStatus != null && x.SourceStatus.Contains(keywords))
                || (x.SourceTcjOwner != null && x.SourceTcjOwner.Contains(keywords))
                || (x.SourceTcjDependency != null && x.SourceTcjDependency.Contains(keywords))
                || (x.SourceEcMeeting != null && x.SourceEcMeeting.Contains(keywords))
                || (x.SourcePpNo != null && x.SourcePpNo.Contains(keywords))
                || (x.SourceTechnicalNoticeNo != null && x.SourceTechnicalNoticeNo.Contains(keywords))
                || (x.SourceImplementation != null && x.SourceImplementation.Contains(keywords))
                || (x.SourceMainChangeReason != null && x.SourceMainChangeReason.Contains(keywords))
                || (x.SourceSecondaryChangeReason != null && x.SourceSecondaryChangeReason.Contains(keywords))
                || (x.SourceSafetyRegulation != null && x.SourceSafetyRegulation.Contains(keywords))
                || (x.SourceProgressStatus != null && x.SourceProgressStatus.Contains(keywords))
                || (x.SourceSerialNumberControl != null && x.SourceSerialNumberControl.Contains(keywords))
                || (x.SourceCustomerApproval != null && x.SourceCustomerApproval.Contains(keywords))
                || (x.SourceServiceManualRevision != null && x.SourceServiceManualRevision.Contains(keywords))
                || (x.SourceUserManualRevision != null && x.SourceUserManualRevision.Contains(keywords))
                || (x.SourcePromotionManualRevision != null && x.SourcePromotionManualRevision.Contains(keywords))
                || (x.SourceStandardDocumentRevision != null && x.SourceStandardDocumentRevision.Contains(keywords))
                || (x.SourceInformationRelease != null && x.SourceInformationRelease.Contains(keywords))
                || (x.SourceCostChange != null && x.SourceCostChange.Contains(keywords))
                || SqlFunc.ToString(x.SourceUnitCost).Contains(keywords)
                || SqlFunc.ToString(x.SourceMoldModificationCost).Contains(keywords)
                || (x.SourceRelatedDrawing != null && x.SourceRelatedDrawing.Contains(keywords))
                || (x.SourceEcContent != null && x.SourceEcContent.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.SourceIssueDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceEcNo))
        {
            exp = exp.And(x => x.SourceEcNo != null && x.SourceEcNo.Contains(queryDto.SourceEcNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceModel))
        {
            exp = exp.And(x => x.SourceModel != null && x.SourceModel.Contains(queryDto.SourceModel));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceTitle))
        {
            exp = exp.And(x => x.SourceTitle != null && x.SourceTitle.Contains(queryDto.SourceTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceStatus))
        {
            exp = exp.And(x => x.SourceStatus != null && x.SourceStatus.Contains(queryDto.SourceStatus));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceTcjOwner))
        {
            exp = exp.And(x => x.SourceTcjOwner != null && x.SourceTcjOwner.Contains(queryDto.SourceTcjOwner));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceTcjDependency))
        {
            exp = exp.And(x => x.SourceTcjDependency != null && x.SourceTcjDependency.Contains(queryDto.SourceTcjDependency));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceEcMeeting))
        {
            exp = exp.And(x => x.SourceEcMeeting != null && x.SourceEcMeeting.Contains(queryDto.SourceEcMeeting));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourcePpNo))
        {
            exp = exp.And(x => x.SourcePpNo != null && x.SourcePpNo.Contains(queryDto.SourcePpNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceTechnicalNoticeNo))
        {
            exp = exp.And(x => x.SourceTechnicalNoticeNo != null && x.SourceTechnicalNoticeNo.Contains(queryDto.SourceTechnicalNoticeNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceImplementation))
        {
            exp = exp.And(x => x.SourceImplementation != null && x.SourceImplementation.Contains(queryDto.SourceImplementation));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceMainChangeReason))
        {
            exp = exp.And(x => x.SourceMainChangeReason != null && x.SourceMainChangeReason.Contains(queryDto.SourceMainChangeReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceSecondaryChangeReason))
        {
            exp = exp.And(x => x.SourceSecondaryChangeReason != null && x.SourceSecondaryChangeReason.Contains(queryDto.SourceSecondaryChangeReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceSafetyRegulation))
        {
            exp = exp.And(x => x.SourceSafetyRegulation != null && x.SourceSafetyRegulation.Contains(queryDto.SourceSafetyRegulation));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceProgressStatus))
        {
            exp = exp.And(x => x.SourceProgressStatus != null && x.SourceProgressStatus.Contains(queryDto.SourceProgressStatus));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceSerialNumberControl))
        {
            exp = exp.And(x => x.SourceSerialNumberControl != null && x.SourceSerialNumberControl.Contains(queryDto.SourceSerialNumberControl));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceCustomerApproval))
        {
            exp = exp.And(x => x.SourceCustomerApproval != null && x.SourceCustomerApproval.Contains(queryDto.SourceCustomerApproval));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceServiceManualRevision))
        {
            exp = exp.And(x => x.SourceServiceManualRevision != null && x.SourceServiceManualRevision.Contains(queryDto.SourceServiceManualRevision));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceUserManualRevision))
        {
            exp = exp.And(x => x.SourceUserManualRevision != null && x.SourceUserManualRevision.Contains(queryDto.SourceUserManualRevision));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourcePromotionManualRevision))
        {
            exp = exp.And(x => x.SourcePromotionManualRevision != null && x.SourcePromotionManualRevision.Contains(queryDto.SourcePromotionManualRevision));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceStandardDocumentRevision))
        {
            exp = exp.And(x => x.SourceStandardDocumentRevision != null && x.SourceStandardDocumentRevision.Contains(queryDto.SourceStandardDocumentRevision));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceInformationRelease))
        {
            exp = exp.And(x => x.SourceInformationRelease != null && x.SourceInformationRelease.Contains(queryDto.SourceInformationRelease));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceCostChange))
        {
            exp = exp.And(x => x.SourceCostChange != null && x.SourceCostChange.Contains(queryDto.SourceCostChange));
        }

        if (queryDto?.SourceUnitCost.HasValue == true)
        {
            exp = exp.And(x => x.SourceUnitCost == queryDto.SourceUnitCost);
        }

        if (queryDto?.SourceMoldModificationCost.HasValue == true)
        {
            exp = exp.And(x => x.SourceMoldModificationCost == queryDto.SourceMoldModificationCost);
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceRelatedDrawing))
        {
            exp = exp.And(x => x.SourceRelatedDrawing != null && x.SourceRelatedDrawing.Contains(queryDto.SourceRelatedDrawing));
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceEcContent))
        {
            exp = exp.And(x => x.SourceEcContent != null && x.SourceEcContent.Contains(queryDto.SourceEcContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.SourceIssueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.SourceIssueDate >= queryDto.SourceIssueDateStart);
        }

        if (queryDto?.SourceIssueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.SourceIssueDate <= queryDto.SourceIssueDateEnd);
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
