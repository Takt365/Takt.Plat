// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA检查日报应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA检查日报应用服务
/// </summary>
public class TaktPcbaInspectionService : TaktServiceBase, ITaktPcbaInspectionService
{
    private readonly ITaktCompanyRepository<TaktPcbaInspection> _pcbaInspectionRepository;
    private readonly ITaktCompanyRepository<TaktPcbaInspectionDetail> _pcbaInspectionDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaInspectionRepository">PCBA检查日报仓储</param>
    /// <param name="pcbaInspectionDetailRepository">PcbaInspectionDetail仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaInspectionService(
        ITaktCompanyRepository<TaktPcbaInspection> pcbaInspectionRepository,
        ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaInspectionRepository = pcbaInspectionRepository;
        _pcbaInspectionDetailRepository = pcbaInspectionDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA检查日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaInspectionDto>> GetPcbaInspectionListAsync(TaktPcbaInspectionQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaInspectionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaInspectionDto>.Create(
            data.Adapt<List<TaktPcbaInspectionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDto?> GetPcbaInspectionByIdAsync(long id)
    {
        var entity = await _pcbaInspectionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPcbaInspectionDto>();
        await FillPcbaInspectionDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取PCBA检查日报选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaInspectionOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _pcbaInspectionRepository.GetListAsync(
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
    /// 创建PCBA检查日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDto> CreatePcbaInspectionAsync(TaktPcbaInspectionCreateDto dto)
    {
        var entity = dto.Adapt<TaktPcbaInspection>();
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaInspectionRepository,
            x => x.ProdOrderCode == entity.ProdOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique)
        {
            throw new TaktBusinessException("PCBA检查日报的ProdOrderCode已存在");
        }
        entity = await _pcbaInspectionRepository.CreateAsync(entity);
                await SavePcbaInspectionChildrenAsync(entity, dto);
        return await GetPcbaInspectionByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaInspectionDto>();
    }

    /// <summary>
    /// 更新PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDto> UpdatePcbaInspectionAsync(long id, TaktPcbaInspectionUpdateDto dto)
    {
        var entity = await _pcbaInspectionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA检查日报不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaInspectionRepository,
            x => x.ProdOrderCode == entity.ProdOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique)
        {
            throw new TaktBusinessException("PCBA检查日报的ProdOrderCode已存在");
        }
        await _pcbaInspectionRepository.UpdateAsync(entity);
                await SavePcbaInspectionChildrenAsync(entity, dto);
        return await GetPcbaInspectionByIdAsync(id) ?? throw new TaktBusinessException("PCBA检查日报不存在");
    }

    /// <summary>
    /// 删除PCBA检查日报
    /// </summary>
    /// <param name="id">PCBA检查日报ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaInspectionByIdAsync(long id)
    {
        var entity = await _pcbaInspectionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA检查日报不存在或已删除");
        }
        await _pcbaInspectionDetailRepository.DeleteAsync(x => x.PcbaInspectionId == entity.Id);
        var deleted = await _pcbaInspectionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("PCBA检查日报不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除PCBA检查日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaInspectionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaInspectionByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaInspectionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaInspectionTemplateDto>(
            sheetName ?? "PCBA检查日报导入模板",
            fileName ?? "PCBA检查日报导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA检查日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaInspectionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaInspectionImportDto>(fileStream, sheetName ?? "PCBA检查日报导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaInspection>();
                var importKey = entity.ProdOrderCode;
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ProdOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique = await _uniqueValidator.IsUniqueAsync(
                    _pcbaInspectionRepository,
                    x => x.ProdOrderCode == entity.ProdOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_unique)
                {
                    throw new TaktBusinessException("PCBA检查日报的ProdOrderCode已存在");
                }
                await _pcbaInspectionRepository.CreateAsync(entity);
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
    /// 导出PCBA检查日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaInspectionAsync(TaktPcbaInspectionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPcbaInspectionQueryDto());
        var list = await _pcbaInspectionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaInspectionExportDto>(),
                sheetName ?? "PCBA检查日报数据",
                fileName ?? "PCBA检查日报导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaInspectionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA检查日报数据",
            fileName ?? "PCBA检查日报导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废PCBA检查明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="pcbaInspectionId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPcbaInspectionDetailsObsoleteAsync(long pcbaInspectionId)
    {
        if (pcbaInspectionId <= 0)
        {
            return;
        }
        var rows = await _pcbaInspectionDetailRepository.GetListAsync(
            x => x.PcbaInspectionId == pcbaInspectionId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _pcbaInspectionDetailRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充PCBA检查日报详情（加载 OneToMany 子表：PCBA检查明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPcbaInspectionDetailsAsync(TaktPcbaInspectionDto dto, TaktPcbaInspection entity)
    {
        if (dto == null)
        {
            return;
        }
        // PCBA检查明细 → dto.PcbaInspectionDetails（含作废行）
        var pcbainspectiondetails = await _pcbaInspectionDetailRepository.GetListAsync(x => x.PcbaInspectionId == entity.Id);
        dto.PcbaInspectionDetails = pcbainspectiondetails.Adapt<List<TaktPcbaInspectionDetailDto>>();
    }

    /// <summary>
    /// 保存PCBA检查日报子表级联（PCBA检查明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePcbaInspectionChildrenAsync(TaktPcbaInspection entity, TaktPcbaInspectionCreateDto dto)
    {
        // PCBA检查明细（PcbaInspectionDetails）
        if (dto.PcbaInspectionDetails is not { Count: > 0 })
        {
            await MarkPcbaInspectionDetailsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _pcbaInspectionDetailRepository.GetListAsync(x => x.PcbaInspectionId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPcbaInspectionDetail>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < dto.PcbaInspectionDetails.Count; i++)
            {
                var childDto = dto.PcbaInspectionDetails[i];
                childDto.PcbaInspectionId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("PCBA检查明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PcbaInspectionId、LineNumber）");
                }
                if (childDto.PcbaInspectionDetailId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PcbaInspectionDetailId, out var target))
                    {
                        throw new TaktBusinessException("PCBA检查明细不存在（PcbaInspectionDetailId={childDto.PcbaInspectionDetailId}）");
                    }
                    if (target.PcbaInspectionId != entity.Id)
                    {
                        throw new TaktBusinessException("PCBA检查明细不属于当前主表（PcbaInspectionDetailId={childDto.PcbaInspectionDetailId}）");
                    }
                    submittedIds.Add(childDto.PcbaInspectionDetailId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _pcbaInspectionDetailRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.PcbaInspectionId == x.PcbaInspectionId
                && x.LineNumber == x.LineNumber,
                        childDto.PcbaInspectionDetailId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique)
                    {
                        throw new TaktBusinessException("PCBA检查明细的CompanyCode、PcbaInspectionId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.PcbaInspectionDetailId;
                    target.PcbaInspectionId = entity.Id;
                    target.IsObsolete = 0;
                    await _pcbaInspectionDetailRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _pcbaInspectionDetailRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.PcbaInspectionId == x.PcbaInspectionId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique)
                    {
                        throw new TaktBusinessException("PCBA检查明细的CompanyCode、PcbaInspectionId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktPcbaInspectionDetail>();
                    child.Id = 0;
                    child.PcbaInspectionId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _pcbaInspectionDetailRepository.UpdateAsync(removed);
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
                await _pcbaInspectionDetailRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA检查日报查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaInspection, bool>> QueryExpression(TaktPcbaInspectionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaInspection>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProdCategory != null && x.ProdCategory.Contains(keywords))
                || (x.ProdOrderType != null && x.ProdOrderType.Contains(keywords))
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.ProdOrderQty).Contains(keywords)
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.BatchNo != null && x.BatchNo.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
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

        if (queryDto?.ProdOrderQty.HasValue == true)
        {
            exp = exp.And(x => x.ProdOrderQty == queryDto.ProdOrderQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.ModelCode))
        {
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(queryDto.ModelCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.BatchNo))
        {
            exp = exp.And(x => x.BatchNo != null && x.BatchNo.Contains(queryDto.BatchNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
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
