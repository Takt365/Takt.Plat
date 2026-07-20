// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报应用服务
/// </summary>
public class TaktPcbaOutputService : TaktServiceBase, ITaktPcbaOutputService
{
    private readonly ITaktCompanyRepository<TaktPcbaOutput> _pcbaOutputRepository;
    private readonly ITaktCompanyRepository<TaktPcbaOutputDetail> _pcbaOutputDetailRepository;
    private readonly ITaktApprovalRepository<TaktStandardOperationTime> _standardOperationTimeRepository;
    private readonly ITaktCompanyRepository<TaktProductionOrder> _productionOrderRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaOutputRepository">PCBA日报仓储</param>
    /// <param name="pcbaOutputDetailRepository">PcbaOutputDetail仓储</param>
    /// <param name="standardOperationTimeRepository">标准工序时间仓储</param>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaOutputService(
        ITaktCompanyRepository<TaktPcbaOutput> pcbaOutputRepository,
        ITaktCompanyRepository<TaktPcbaOutputDetail> pcbaOutputDetailRepository,
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaOutputRepository = pcbaOutputRepository;
        _pcbaOutputDetailRepository = pcbaOutputDetailRepository;
        _standardOperationTimeRepository = standardOperationTimeRepository;
        _productionOrderRepository = productionOrderRepository;
        _modelDestinationRepository = modelDestinationRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaOutputDto>> GetPcbaOutputListAsync(TaktPcbaOutputQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaOutputRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaOutputDto>.Create(
            data.Adapt<List<TaktPcbaOutputDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDto?> GetPcbaOutputByIdAsync(long id)
    {
        var entity = await _pcbaOutputRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPcbaOutputDto>();
        await FillPcbaOutputDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取PCBA日报选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaOutputOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _pcbaOutputRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建PCBA日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDto> CreatePcbaOutputAsync(TaktPcbaOutputCreateDto dto)
    {
        EnsureThreeLayerContext();
        var entity = dto.Adapt<TaktPcbaOutput>();
        await TaktPcbaOutputBackfillHelper.ApplyMasterFromProductionOrderAsync(
            _productionOrderRepository,
            _modelDestinationRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            entity.ProdOrderCode,
            entity);
        var operationTimes = await ResolveStandardOperationTimesForMasterAsync(entity);
        TaktPcbaOutputDetailSeedHelper.EnsureDefaultDetailsOnCreate(dto, operationTimes);
        var isUnique_ix_takt_logistics_manufacturing_output_pcba_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaOutputRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProdCategory == entity.ProdCategory
                && x.ProdDate == entity.ProdDate
                && x.ProdOrderCode == entity.ProdOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_output_pcba_unique)
        {
            throw new TaktBusinessException("PCBA日报的PlantCode、ProdCategory、ProdDate、ProdOrderCode已存在");
        }
        entity = await _pcbaOutputRepository.CreateAsync(entity);
                await SavePcbaOutputChildrenAsync(entity, dto);
        return await GetPcbaOutputByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaOutputDto>();
    }

    /// <summary>
    /// 更新PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDto> UpdatePcbaOutputAsync(long id, TaktPcbaOutputUpdateDto dto)
    {
        var entity = await _pcbaOutputRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA日报不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_output_pcba_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaOutputRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProdCategory == entity.ProdCategory
                && x.ProdDate == entity.ProdDate
                && x.ProdOrderCode == entity.ProdOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_output_pcba_unique)
        {
            throw new TaktBusinessException("PCBA日报的PlantCode、ProdCategory、ProdDate、ProdOrderCode已存在");
        }
        await _pcbaOutputRepository.UpdateAsync(entity);
                await SavePcbaOutputChildrenAsync(entity, dto);
        return await GetPcbaOutputByIdAsync(id) ?? throw new TaktBusinessException("PCBA日报不存在");
    }

    /// <summary>
    /// 删除PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaOutputByIdAsync(long id)
    {
        var entity = await _pcbaOutputRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA日报不存在或已删除");
        }
        await _pcbaOutputDetailRepository.DeleteAsync(x => x.PcbaOutputId == entity.Id);
        var deleted = await _pcbaOutputRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("PCBA日报不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除PCBA日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaOutputBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaOutputByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaOutputTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaOutputTemplateDto>(
            sheetName ?? "PCBA日报导入模板",
            fileName ?? "PCBA日报导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaOutputAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaOutputImportDto>(fileStream, sheetName ?? "PCBA日报导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaOutput>();
                var importKey = $"{entity.PlantCode}|{entity.ProdCategory}|{entity.ProdDate}|{entity.ProdOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ProdCategory、ProdDate、ProdOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_output_pcba_unique = await _uniqueValidator.IsUniqueAsync(
                    _pcbaOutputRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ProdCategory == entity.ProdCategory
                        && x.ProdDate == entity.ProdDate
                        && x.ProdOrderCode == entity.ProdOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_output_pcba_unique)
                {
                    throw new TaktBusinessException("PCBA日报的PlantCode、ProdCategory、ProdDate、ProdOrderCode已存在");
                }
                await _pcbaOutputRepository.CreateAsync(entity);
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
    /// 导出PCBA日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaOutputAsync(TaktPcbaOutputQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPcbaOutputQueryDto());
        var list = await _pcbaOutputRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaOutputExportDto>(),
                sheetName ?? "PCBA日报数据",
                fileName ?? "PCBA日报导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaOutputExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA日报数据",
            fileName ?? "PCBA日报导出.xlsx");
    }

    /// <summary>
    /// 按物料编码获取 PCBA 日报默认明细预览
    /// </summary>
    /// <param name="materialCode">物料编码</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodDate">生产日期</param>
    /// <returns>默认明细预览列表</returns>
    public async Task<List<TaktPcbaOutputDefaultDetailDto>> GetPcbaOutputDefaultDetailsByMaterialAsync(
        string materialCode,
        string plantCode,
        DateTime prodDate)
    {
        EnsureThreeLayerContext();
        ArgumentException.ThrowIfNullOrWhiteSpace(materialCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        var operationTimes = await TaktAssyOutputDerivedFieldsHelper.ResolveStandardOperationTimesByMaterialAsync(
            _standardOperationTimeRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            materialCode.Trim(),
            plantCode.Trim(),
            prodDate);
        return TaktPcbaOutputDetailSeedHelper.BuildDefaultDetailPreview(operationTimes);
    }

    /// <summary>
    /// 按主表物料/工厂/生产日期解析标准工序时间
    /// </summary>
    /// <param name="entity">PCBA 日报主表</param>
    /// <returns>标准工序时间列表</returns>
    private async Task<IReadOnlyList<TaktStandardOperationTime>> ResolveStandardOperationTimesForMasterAsync(TaktPcbaOutput entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (string.IsNullOrWhiteSpace(entity.MaterialCode) || string.IsNullOrWhiteSpace(entity.PlantCode))
        {
            return [];
        }
        return await TaktAssyOutputDerivedFieldsHelper.ResolveStandardOperationTimesByMaterialAsync(
            _standardOperationTimeRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            entity.MaterialCode.Trim(),
            entity.PlantCode.Trim(),
            entity.ProdDate);
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废PCBA日报明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="pcbaOutputId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPcbaOutputDetailsObsoleteAsync(long pcbaOutputId)
    {
        if (pcbaOutputId <= 0)
        {
            return;
        }
        var rows = await _pcbaOutputDetailRepository.GetListAsync(
            x => x.PcbaOutputId == pcbaOutputId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _pcbaOutputDetailRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充PCBA日报详情（加载 OneToMany 子表：PCBA日报明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPcbaOutputDetailsAsync(TaktPcbaOutputDto dto, TaktPcbaOutput entity)
    {
        if (dto == null)
        {
            return;
        }
        // PCBA日报明细 → dto.PcbaOutputDetails（含作废行）
        var pcbaoutputdetails = await _pcbaOutputDetailRepository.GetListAsync(x => x.PcbaOutputId == entity.Id);
        dto.PcbaOutputDetails = pcbaoutputdetails.Adapt<List<TaktPcbaOutputDetailDto>>();
    }

    /// <summary>
    /// 保存PCBA日报子表级联（PCBA日报明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePcbaOutputChildrenAsync(TaktPcbaOutput entity, TaktPcbaOutputCreateDto dto)
    {
        // PCBA日报明细（PcbaOutputDetails）
        List<TaktPcbaOutputDetailUpdateDto>? pcbaOutputDetailsForSave;
        if (dto is TaktPcbaOutputUpdateDto updateDto && updateDto.PcbaOutputDetails != null)
        {
            pcbaOutputDetailsForSave = updateDto.PcbaOutputDetails;
        }
        else if (dto.PcbaOutputDetails != null)
        {
            pcbaOutputDetailsForSave = dto.PcbaOutputDetails.Adapt<List<TaktPcbaOutputDetailUpdateDto>>();
        }
        else
        {
            pcbaOutputDetailsForSave = null;
        }
        if (pcbaOutputDetailsForSave is not { Count: > 0 })
        {
            await MarkPcbaOutputDetailsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _pcbaOutputDetailRepository.GetListAsync(x => x.PcbaOutputId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPcbaOutputDetail>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < pcbaOutputDetailsForSave.Count; i++)
            {
                var childDto = pcbaOutputDetailsForSave[i];
                childDto.PcbaOutputId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("PCBA日报明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PcbaOutputId、LineNumber）");
                }
                if (childDto.PcbaOutputDetailId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PcbaOutputDetailId, out var target))
                    {
                        throw new TaktBusinessException("PCBA日报明细不存在（PcbaOutputDetailId={childDto.PcbaOutputDetailId}）");
                    }
                    if (target.PcbaOutputId != entity.Id)
                    {
                        throw new TaktBusinessException("PCBA日报明细不属于当前主表（PcbaOutputDetailId={childDto.PcbaOutputDetailId}）");
                    }
                    submittedIds.Add(childDto.PcbaOutputDetailId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_output_pcba_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _pcbaOutputDetailRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.PcbaOutputId == x.PcbaOutputId
                && x.LineNumber == x.LineNumber,
                        childDto.PcbaOutputDetailId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_output_pcba_detail_line_unique)
                    {
                        throw new TaktBusinessException("PCBA日报明细的CompanyCode、PcbaOutputId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.PcbaOutputDetailId;
                    target.PcbaOutputId = entity.Id;
                    target.IsObsolete = 0;
                    await _pcbaOutputDetailRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_output_pcba_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _pcbaOutputDetailRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.PcbaOutputId == x.PcbaOutputId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_output_pcba_detail_line_unique)
                    {
                        throw new TaktBusinessException("PCBA日报明细的CompanyCode、PcbaOutputId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktPcbaOutputDetail>();
                    child.Id = 0;
                    child.PcbaOutputId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _pcbaOutputDetailRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ProdOrderCode) ? entity.ProdOrderCode : entity.Id.ToString();
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
                await _pcbaOutputDetailRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA日报查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaOutput, bool>> QueryExpression(TaktPcbaOutputQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaOutput>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProdCategory != null && x.ProdCategory.Contains(keywords))
                || (x.ProdOrderType != null && x.ProdOrderType.Contains(keywords))
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.BatchNo != null && x.BatchNo.Contains(keywords))
                || SqlFunc.ToString(x.ProdOrderQty).Contains(keywords)
                || (x.SerialNo != null && x.SerialNo.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ProdDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdCategory))
        {
            exp = exp.And(x => x.ProdCategory != null && x.ProdCategory.Contains(queryDto.ProdCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderType))
        {
            exp = exp.And(x => x.ProdOrderType != null && x.ProdOrderType.Contains(queryDto.ProdOrderType));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProdOrderCode))
        {
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(queryDto.ProdOrderCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelCode))
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(queryDto.ModelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.BatchNo))
        {
            exp = exp.And(x => x.BatchNo != null && x.BatchNo.Contains(queryDto.BatchNo));
        }

        if (queryDto?.ProdOrderQty.HasValue == true)
        {
            exp = exp.And(x => x.ProdOrderQty == queryDto.ProdOrderQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.SerialNo))
        {
            exp = exp.And(x => x.SerialNo != null && x.SerialNo.Contains(queryDto.SerialNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ProdDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ProdDate >= queryDto.ProdDateStart);
        }

        if (queryDto?.ProdDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ProdDate <= queryDto.ProdDateEnd);
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
