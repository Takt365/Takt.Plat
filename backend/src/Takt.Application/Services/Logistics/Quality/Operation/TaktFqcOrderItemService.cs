// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderItemService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：出货检验单明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Quality.Operation;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 出货检验单明细应用服务
/// </summary>
public class TaktFqcOrderItemService : TaktServiceBase, ITaktFqcOrderItemService
{
    private readonly ITaktCompanyRepository<TaktFqcOrderItem> _fqcOrderItemRepository;
    private readonly ITaktCompanyRepository<TaktFqcDefectHandling> _fqcDefectHandlingRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fqcOrderItemRepository">出货检验单明细仓储</param>
    /// <param name="fqcDefectHandlingRepository">FqcDefectHandling仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFqcOrderItemService(
        ITaktCompanyRepository<TaktFqcOrderItem> fqcOrderItemRepository,
        ITaktCompanyRepository<TaktFqcDefectHandling> fqcDefectHandlingRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _fqcOrderItemRepository = fqcOrderItemRepository;
        _fqcDefectHandlingRepository = fqcDefectHandlingRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取出货检验单明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFqcOrderItemDto>> GetFqcOrderItemListAsync(TaktFqcOrderItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _fqcOrderItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFqcOrderItemDto>.Create(
            data.Adapt<List<TaktFqcOrderItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取出货检验单明细
    /// </summary>
    /// <param name="id">出货检验单明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcOrderItemDto?> GetFqcOrderItemByIdAsync(long id)
    {
        var entity = await _fqcOrderItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktFqcOrderItemDto>();
        await FillFqcOrderItemDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取出货检验单明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFqcOrderItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _fqcOrderItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建出货检验单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcOrderItemDto> CreateFqcOrderItemAsync(TaktFqcOrderItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktFqcOrderItem>();
        var isUnique_ix_takt_logistics_quality_fqc_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _fqcOrderItemRepository,
            x => x.FqcOrderId == entity.FqcOrderId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_fqc_order_item_order_line_unique)
        {
            throw new TaktBusinessException("出货检验单明细的FqcOrderId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _fqcOrderItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FqcOrderId == entity.FqcOrderId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.FqcOrderCode) ? entity.FqcOrderCode : entity.FqcOrderId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _fqcOrderItemRepository.CreateAsync(entity);
                await SaveFqcOrderItemChildrenAsync(entity, dto);
        return await GetFqcOrderItemByIdAsync(entity.Id) ?? entity.Adapt<TaktFqcOrderItemDto>();
    }

    /// <summary>
    /// 更新出货检验单明细
    /// </summary>
    /// <param name="id">出货检验单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcOrderItemDto> UpdateFqcOrderItemAsync(long id, TaktFqcOrderItemUpdateDto dto)
    {
        var entity = await _fqcOrderItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("出货检验单明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_fqc_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _fqcOrderItemRepository,
            x => x.FqcOrderId == entity.FqcOrderId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_fqc_order_item_order_line_unique)
        {
            throw new TaktBusinessException("出货检验单明细的FqcOrderId、LineNumber已存在");
        }
        await _fqcOrderItemRepository.UpdateAsync(entity);
                await SaveFqcOrderItemChildrenAsync(entity, dto);
        return await GetFqcOrderItemByIdAsync(id) ?? throw new TaktBusinessException("出货检验单明细不存在");
    }

    /// <summary>
    /// 删除出货检验单明细
    /// </summary>
    /// <param name="id">出货检验单明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFqcOrderItemByIdAsync(long id)
    {
        var entity = await _fqcOrderItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("出货检验单明细不存在或已删除");
        }
        await _fqcDefectHandlingRepository.DeleteAsync(x => x.FqcOrderItemId == entity.Id);
        var deleted = await _fqcOrderItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("出货检验单明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除出货检验单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFqcOrderItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFqcOrderItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新出货检验单明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFqcOrderItemDto> UpdateFqcOrderItemStatusAsync(TaktFqcOrderItemStatusDto dto)
    {
        var entity = await _fqcOrderItemRepository.GetByIdAsync(dto.FqcOrderItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("出货检验单明细不存在");
        }
        entity.JudgeStatus = dto.JudgeStatus;
        await _fqcOrderItemRepository.UpdateAsync(entity);
        return await GetFqcOrderItemByIdAsync(dto.FqcOrderItemId) ?? throw new TaktBusinessException("出货检验单明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFqcOrderItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFqcOrderItemTemplateDto>(
            sheetName ?? "出货检验单明细导入模板",
            fileName ?? "出货检验单明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入出货检验单明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFqcOrderItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFqcOrderItemImportDto>(fileStream, sheetName ?? "出货检验单明细导入模板");
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
                var entity = rows[i].Adapt<TaktFqcOrderItem>();
                var importKey = $"{entity.FqcOrderId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（FqcOrderId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_fqc_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _fqcOrderItemRepository,
                    x => x.FqcOrderId == entity.FqcOrderId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_fqc_order_item_order_line_unique)
                {
                    throw new TaktBusinessException("出货检验单明细的FqcOrderId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _fqcOrderItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FqcOrderId == entity.FqcOrderId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.FqcOrderCode) ? entity.FqcOrderCode : entity.FqcOrderId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _fqcOrderItemRepository.CreateAsync(entity);
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
    /// 导出出货检验单明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFqcOrderItemAsync(TaktFqcOrderItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFqcOrderItemQueryDto());
        var list = await _fqcOrderItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFqcOrderItemExportDto>(),
                sheetName ?? "出货检验单明细数据",
                fileName ?? "出货检验单明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFqcOrderItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "出货检验单明细数据",
            fileName ?? "出货检验单明细导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充出货检验单明细详情（加载 OneToMany 子表：出货检验不良处理记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillFqcOrderItemDetailsAsync(TaktFqcOrderItemDto dto, TaktFqcOrderItem entity)
    {
        if (dto == null)
        {
            return;
        }
        // 出货检验不良处理记录 → dto.DefectHandlings
        var defecthandlings = await _fqcDefectHandlingRepository.GetListAsync(x => x.FqcOrderItemId == entity.Id);
        dto.DefectHandlings = defecthandlings.Adapt<List<TaktFqcDefectHandlingDto>>();
    }

    /// <summary>
    /// 保存出货检验单明细子表级联（出货检验不良处理记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveFqcOrderItemChildrenAsync(TaktFqcOrderItem entity, TaktFqcOrderItemCreateDto dto)
    {
        // 出货检验不良处理记录（DefectHandlings）
        if (dto.DefectHandlings is not { Count: > 0 })
        {
            await _fqcDefectHandlingRepository.DeleteAsync(x => x.FqcOrderItemId == entity.Id);
        }
        else
        {
            var defecthandlings = dto.DefectHandlings.Adapt<List<TaktFqcDefectHandling>>();
            foreach (var child in defecthandlings)
            {
                child.FqcOrderItemId = entity.Id;
            }
            var defecthandlingsNeedLine = defecthandlings.Where(c => c.LineNumber <= 0).ToList();
            if (defecthandlingsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.FqcOrderCode) ? entity.FqcOrderCode : entity.Id.ToString();
                var maxLine = await _fqcDefectHandlingRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FqcOrderItemId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, defecthandlingsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in defecthandlings)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < defecthandlings.Count; i++)
                        {
                            var key = $"{defecthandlings[i].CompanyCode}|{defecthandlings[i].FqcOrderItemId}|{defecthandlings[i].DefectCode}|{defecthandlings[i].HandlingMethod}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"出货检验不良处理记录第{i + 1}项与本次提交的其他项重复（CompanyCode、FqcOrderItemId、DefectCode、HandlingMethod）");
                            }
                        }
            await _fqcDefectHandlingRepository.DeleteAsync(x => x.FqcOrderItemId == entity.Id);
            foreach (var child in defecthandlings)
            {
            var isUnique_ix_takt_logistics_quality_fqc_defect_handling_unique = await _uniqueValidator.IsUniqueAsync(
                _fqcDefectHandlingRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.FqcOrderItemId == child.FqcOrderItemId
                    && x.DefectCode == child.DefectCode
                    && x.HandlingMethod == child.HandlingMethod);
            if (!isUnique_ix_takt_logistics_quality_fqc_defect_handling_unique)
            {
                throw new TaktBusinessException("出货检验不良处理记录的CompanyCode、FqcOrderItemId、DefectCode、HandlingMethod已存在");
            }
            }
            await _fqcDefectHandlingRepository.CreateRangeAsync(defecthandlings);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建出货检验单明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFqcOrderItem, bool>> QueryExpression(TaktFqcOrderItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFqcOrderItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.FqcOrderId).Contains(keywords)
                || (x.FqcOrderCode != null && x.FqcOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialName != null && x.MaterialName.Contains(keywords))
                || (x.BatchNo != null && x.BatchNo.Contains(keywords))
                || SqlFunc.ToString(x.WarehouseQuantity).Contains(keywords)
                || (x.StandardCode != null && x.StandardCode.Contains(keywords))
                || (x.SamplingSchemeCode != null && x.SamplingSchemeCode.Contains(keywords))
                || SqlFunc.ToString(x.InspectionMethod).Contains(keywords)
                || SqlFunc.ToString(x.SampleQuantity).Contains(keywords)
                || SqlFunc.ToString(x.QualifiedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.UnqualifiedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.InspectionReturnQuantity).Contains(keywords)
                || SqlFunc.ToString(x.JudgeStatus).Contains(keywords)
                || (x.SampleSerialNo != null && x.SampleSerialNo.Contains(keywords))
                || (x.InspectionDescription != null && x.InspectionDescription.Contains(keywords))
                || (x.InspectorBy != null && x.InspectorBy.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.InspectionDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.FqcOrderId.HasValue == true)
        {
            exp = exp.And(x => x.FqcOrderId == queryDto.FqcOrderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.FqcOrderCode))
        {
            exp = exp.And(x => x.FqcOrderCode != null && x.FqcOrderCode.Contains(queryDto.FqcOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialName))
        {
            exp = exp.And(x => x.MaterialName != null && x.MaterialName.Contains(queryDto.MaterialName));
        }

        if (!string.IsNullOrEmpty(queryDto?.BatchNo))
        {
            exp = exp.And(x => x.BatchNo != null && x.BatchNo.Contains(queryDto.BatchNo));
        }

        if (queryDto?.WarehouseQuantity.HasValue == true)
        {
            exp = exp.And(x => x.WarehouseQuantity == queryDto.WarehouseQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.StandardCode))
        {
            exp = exp.And(x => x.StandardCode != null && x.StandardCode.Contains(queryDto.StandardCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SamplingSchemeCode))
        {
            exp = exp.And(x => x.SamplingSchemeCode != null && x.SamplingSchemeCode.Contains(queryDto.SamplingSchemeCode));
        }

        if (queryDto?.InspectionMethod.HasValue == true)
        {
            exp = exp.And(x => x.InspectionMethod == queryDto.InspectionMethod);
        }

        if (queryDto?.SampleQuantity.HasValue == true)
        {
            exp = exp.And(x => x.SampleQuantity == queryDto.SampleQuantity);
        }

        if (queryDto?.QualifiedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.QualifiedQuantity == queryDto.QualifiedQuantity);
        }

        if (queryDto?.UnqualifiedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.UnqualifiedQuantity == queryDto.UnqualifiedQuantity);
        }

        if (queryDto?.InspectionReturnQuantity.HasValue == true)
        {
            exp = exp.And(x => x.InspectionReturnQuantity == queryDto.InspectionReturnQuantity);
        }

        if (queryDto?.JudgeStatus.HasValue == true)
        {
            exp = exp.And(x => x.JudgeStatus == queryDto.JudgeStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.SampleSerialNo))
        {
            exp = exp.And(x => x.SampleSerialNo != null && x.SampleSerialNo.Contains(queryDto.SampleSerialNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.InspectionDescription))
        {
            exp = exp.And(x => x.InspectionDescription != null && x.InspectionDescription.Contains(queryDto.InspectionDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.InspectorBy))
        {
            exp = exp.And(x => x.InspectorBy != null && x.InspectorBy.Contains(queryDto.InspectorBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.InspectionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InspectionDate >= queryDto.InspectionDateStart);
        }

        if (queryDto?.InspectionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InspectionDate <= queryDto.InspectionDateEnd);
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
