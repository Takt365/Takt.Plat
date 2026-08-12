// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOperationService.cs
// 创建时间：2026-07-24
// 创建人：Takt365(Cursor AI)
// 功能描述：APS工序排程应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// APS工序排程应用服务
/// </summary>
public class TaktApsOperationService : TaktServiceBase, ITaktApsOperationService
{
    private readonly ITaktCompanyRepository<TaktApsOperation> _apsOperationRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsOperationRepository">APS工序排程仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktApsOperationService(
        ITaktCompanyRepository<TaktApsOperation> apsOperationRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _apsOperationRepository = apsOperationRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取APS工序排程列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktApsOperationDto>> GetApsOperationListAsync(TaktApsOperationQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _apsOperationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktApsOperationDto>.Create(
            data.Adapt<List<TaktApsOperationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取APS工序排程
    /// </summary>
    /// <param name="id">APS工序排程ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOperationDto?> GetApsOperationByIdAsync(long id)
    {
        var entity = await _apsOperationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktApsOperationDto>();
    }

    /// <summary>
    /// 获取APS工序排程选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetApsOperationOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _apsOperationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OperationStatus == 1 && x.IsObsolete == 0,
            x => x.ProcessName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ApsOrderCode,
            DictLabel = e.ProcessName ?? e.ApsOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建APS工序排程
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOperationDto> CreateApsOperationAsync(TaktApsOperationCreateDto dto)
    {
        var entity = dto.Adapt<TaktApsOperation>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_aps_operation_line_unique = await _uniqueValidator.IsUniqueAsync(
            _apsOperationRepository,
            x => x.ApsOrderId == entity.ApsOrderId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_operation_line_unique)
        {
            throw new TaktBusinessException("APS工序排程的ApsOrderId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _apsOperationRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ApsOrderId == entity.ApsOrderId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.ApsOrderCode) ? entity.ApsOrderCode : entity.ApsOrderId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _apsOperationRepository.CreateAsync(entity);
        return await GetApsOperationByIdAsync(entity.Id) ?? entity.Adapt<TaktApsOperationDto>();
    }

    /// <summary>
    /// 更新APS工序排程
    /// </summary>
    /// <param name="id">APS工序排程ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOperationDto> UpdateApsOperationAsync(long id, TaktApsOperationUpdateDto dto)
    {
        var entity = await _apsOperationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS工序排程不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_aps_operation_line_unique = await _uniqueValidator.IsUniqueAsync(
            _apsOperationRepository,
            x => x.ApsOrderId == entity.ApsOrderId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_operation_line_unique)
        {
            throw new TaktBusinessException("APS工序排程的ApsOrderId、LineNumber已存在");
        }
        await _apsOperationRepository.UpdateAsync(entity);
        return await GetApsOperationByIdAsync(id) ?? throw new TaktBusinessException("APS工序排程不存在");
    }

    /// <summary>
    /// 删除APS工序排程
    /// </summary>
    /// <param name="id">APS工序排程ID</param>
    /// <returns>任务</returns>
    public async Task DeleteApsOperationByIdAsync(long id)
    {
        var entity = await _apsOperationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS工序排程不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("APS工序排程不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("APS工序排程已作废");
        }
        entity.IsObsolete = 1;
        await _apsOperationRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除APS工序排程
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteApsOperationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteApsOperationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新APS工序排程状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOperationDto> UpdateApsOperationStatusAsync(TaktApsOperationStatusDto dto)
    {
        var entity = await _apsOperationRepository.GetByIdAsync(dto.ApsOperationId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS工序排程不存在");
        }
        entity.OperationStatus = dto.OperationStatus;
        await _apsOperationRepository.UpdateAsync(entity);
        return await GetApsOperationByIdAsync(dto.ApsOperationId) ?? throw new TaktBusinessException("APS工序排程不存在");
    }

    /// <summary>
    /// 更新APS工序排程作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOperationDto> UpdateApsOperationObsoleteAsync(TaktApsOperationObsoleteDto dto)
    {
        var entity = await _apsOperationRepository.GetByIdAsync(dto.ApsOperationId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS工序排程不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("APS工序排程不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _apsOperationRepository.UpdateAsync(entity);
        return await GetApsOperationByIdAsync(dto.ApsOperationId) ?? throw new TaktBusinessException("APS工序排程不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetApsOperationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktApsOperationTemplateDto>(
            sheetName ?? "APS工序排程导入模板",
            fileName ?? "APS工序排程导入模板.xlsx");
    }

    /// <summary>
    /// 导入APS工序排程
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportApsOperationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktApsOperationImportDto>(fileStream, sheetName ?? "APS工序排程导入模板");
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
                var entity = rows[i].Adapt<TaktApsOperation>();
                var importKey = $"{entity.ApsOrderId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ApsOrderId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_aps_operation_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _apsOperationRepository,
                    x => x.ApsOrderId == entity.ApsOrderId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_aps_operation_line_unique)
                {
                    throw new TaktBusinessException("APS工序排程的ApsOrderId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _apsOperationRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ApsOrderId == entity.ApsOrderId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ApsOrderCode) ? entity.ApsOrderCode : entity.ApsOrderId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _apsOperationRepository.CreateAsync(entity);
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
    /// 导出APS工序排程
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportApsOperationAsync(TaktApsOperationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktApsOperationQueryDto());
        var list = await _apsOperationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsOperationExportDto>(),
                sheetName ?? "APS工序排程数据",
                fileName ?? "APS工序排程导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktApsOperationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "APS工序排程数据",
            fileName ?? "APS工序排程导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建APS工序排程查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktApsOperation, bool>> QueryExpression(TaktApsOperationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktApsOperation>();

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
                SqlFunc.ToString(x.ApsOrderId).Contains(keywords)
                || (x.ApsOrderCode != null && x.ApsOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.RoutingItemId).Contains(keywords)
                || (x.ProcessCode != null && x.ProcessCode.Contains(keywords))
                || (x.ProcessName != null && x.ProcessName.Contains(keywords))
                || (x.WorkCenterCode != null && x.WorkCenterCode.Contains(keywords))
                || SqlFunc.ToString(x.WorkCenterResourceId).Contains(keywords)
                || SqlFunc.ToString(x.PlannedDurationMinutes).Contains(keywords)
                || SqlFunc.ToString(x.ChangeoverMinutes).Contains(keywords)
                || SqlFunc.ToString(x.OperationStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlannedStartTime).Contains(keywords)
                || SqlFunc.ToString(x.PlannedEndTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ApsOrderId.HasValue == true)
        {
            exp = exp.And(x => x.ApsOrderId == queryDto.ApsOrderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ApsOrderCode))
        {
            exp = exp.And(x => x.ApsOrderCode != null && x.ApsOrderCode.Contains(queryDto.ApsOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.RoutingItemId.HasValue == true)
        {
            exp = exp.And(x => x.RoutingItemId == queryDto.RoutingItemId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessCode))
        {
            exp = exp.And(x => x.ProcessCode != null && x.ProcessCode.Contains(queryDto.ProcessCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessName))
        {
            exp = exp.And(x => x.ProcessName != null && x.ProcessName.Contains(queryDto.ProcessName));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenterCode))
        {
            exp = exp.And(x => x.WorkCenterCode != null && x.WorkCenterCode.Contains(queryDto.WorkCenterCode));
        }

        if (queryDto?.WorkCenterResourceId.HasValue == true)
        {
            exp = exp.And(x => x.WorkCenterResourceId == queryDto.WorkCenterResourceId);
        }

        if (queryDto?.PlannedDurationMinutes.HasValue == true)
        {
            exp = exp.And(x => x.PlannedDurationMinutes == queryDto.PlannedDurationMinutes);
        }

        if (queryDto?.ChangeoverMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ChangeoverMinutes == queryDto.ChangeoverMinutes);
        }

        if (queryDto?.OperationStatus.HasValue == true)
        {
            exp = exp.And(x => x.OperationStatus == queryDto.OperationStatus);
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

        if (queryDto?.PlannedStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime >= queryDto.PlannedStartTimeStart);
        }

        if (queryDto?.PlannedStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedStartTime <= queryDto.PlannedStartTimeEnd);
        }

        if (queryDto?.PlannedEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndTime >= queryDto.PlannedEndTimeStart);
        }

        if (queryDto?.PlannedEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedEndTime <= queryDto.PlannedEndTimeEnd);
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
