// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialDocumentService.cs
// 创建时间：2026-07-15
// 创建人：Takt365(Cursor AI)
// 功能描述：物料凭证应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料凭证应用服务
/// </summary>
public class TaktMaterialDocumentService : TaktServiceBase, ITaktMaterialDocumentService
{
    private readonly ITaktCompanyRepository<TaktMaterialDocument> _materialDocumentRepository;
    private readonly ITaktCompanyRepository<TaktMaterialDocumentItem> _materialDocumentItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialDocumentRepository">物料凭证仓储</param>
    /// <param name="materialDocumentItemRepository">MaterialDocumentItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialDocumentService(
        ITaktCompanyRepository<TaktMaterialDocument> materialDocumentRepository,
        ITaktCompanyRepository<TaktMaterialDocumentItem> materialDocumentItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialDocumentRepository = materialDocumentRepository;
        _materialDocumentItemRepository = materialDocumentItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料凭证列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialDocumentDto>> GetMaterialDocumentListAsync(TaktMaterialDocumentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialDocumentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialDocumentDto>.Create(
            data.Adapt<List<TaktMaterialDocumentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料凭证
    /// </summary>
    /// <param name="id">物料凭证ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentDto?> GetMaterialDocumentByIdAsync(long id)
    {
        var entity = await _materialDocumentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMaterialDocumentDto>();
        await FillMaterialDocumentDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取物料凭证选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialDocumentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _materialDocumentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialDocumentStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建物料凭证
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentDto> CreateMaterialDocumentAsync(TaktMaterialDocumentCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialDocument>();
        var isUnique_ix_takt_logistics_materials_material_document_doc_unique = await _uniqueValidator.IsUniqueAsync(
            _materialDocumentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialDocumentCode == entity.MaterialDocumentCode);
        if (!isUnique_ix_takt_logistics_materials_material_document_doc_unique)
        {
            throw new TaktBusinessException("物料凭证的PlantCode、MaterialDocumentCode已存在");
        }
        entity = await _materialDocumentRepository.CreateAsync(entity);
                await SaveMaterialDocumentChildrenAsync(entity, dto);
        return await GetMaterialDocumentByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialDocumentDto>();
    }

    /// <summary>
    /// 更新物料凭证
    /// </summary>
    /// <param name="id">物料凭证ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentDto> UpdateMaterialDocumentAsync(long id, TaktMaterialDocumentUpdateDto dto)
    {
        var entity = await _materialDocumentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_material_document_doc_unique = await _uniqueValidator.IsUniqueAsync(
            _materialDocumentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialDocumentCode == entity.MaterialDocumentCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_document_doc_unique)
        {
            throw new TaktBusinessException("物料凭证的PlantCode、MaterialDocumentCode已存在");
        }
        await _materialDocumentRepository.UpdateAsync(entity);
                await SaveMaterialDocumentChildrenAsync(entity, dto);
        return await GetMaterialDocumentByIdAsync(id) ?? throw new TaktBusinessException("物料凭证不存在");
    }

    /// <summary>
    /// 删除物料凭证
    /// </summary>
    /// <param name="id">物料凭证ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialDocumentByIdAsync(long id)
    {
        var entity = await _materialDocumentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证不存在或已删除");
        }
        await _materialDocumentItemRepository.DeleteAsync(x => x.MaterialDocumentId == entity.Id);
        var deleted = await _materialDocumentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料凭证不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料凭证
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialDocumentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialDocumentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料凭证状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialDocumentDto> UpdateMaterialDocumentStatusAsync(TaktMaterialDocumentStatusDto dto)
    {
        var entity = await _materialDocumentRepository.GetByIdAsync(dto.MaterialDocumentId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料凭证不存在");
        }
        entity.MaterialDocumentStatus = dto.MaterialDocumentStatus;
        await _materialDocumentRepository.UpdateAsync(entity);
        return await GetMaterialDocumentByIdAsync(dto.MaterialDocumentId) ?? throw new TaktBusinessException("物料凭证不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialDocumentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialDocumentTemplateDto>(
            sheetName ?? "物料凭证导入模板",
            fileName ?? "物料凭证导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料凭证
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialDocumentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialDocumentImportDto>(fileStream, sheetName ?? "物料凭证导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialDocument>();
                var importKey = $"{entity.PlantCode}|{entity.MaterialDocumentCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MaterialDocumentCode）");
                }
                var isUnique_ix_takt_logistics_materials_material_document_doc_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialDocumentRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MaterialDocumentCode == entity.MaterialDocumentCode);
                if (!isUnique_ix_takt_logistics_materials_material_document_doc_unique)
                {
                    throw new TaktBusinessException("物料凭证的PlantCode、MaterialDocumentCode已存在");
                }
                await _materialDocumentRepository.CreateAsync(entity);
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
    /// 导出物料凭证
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialDocumentAsync(TaktMaterialDocumentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaterialDocumentQueryDto());
        var list = await _materialDocumentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialDocumentExportDto>(),
                sheetName ?? "物料凭证数据",
                fileName ?? "物料凭证导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialDocumentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料凭证数据",
            fileName ?? "物料凭证导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废物料凭证行项目标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="materialDocumentId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkMaterialDocumentItemsObsoleteAsync(long materialDocumentId)
    {
        if (materialDocumentId <= 0)
        {
            return;
        }
        var rows = await _materialDocumentItemRepository.GetListAsync(
            x => x.MaterialDocumentId == materialDocumentId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _materialDocumentItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充物料凭证详情（加载 OneToMany 子表：物料凭证行项目）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillMaterialDocumentDetailsAsync(TaktMaterialDocumentDto dto, TaktMaterialDocument entity)
    {
        if (dto == null)
        {
            return;
        }
        // 物料凭证行项目 → dto.Items（含作废行）
        var items = await _materialDocumentItemRepository.GetListAsync(x => x.MaterialDocumentId == entity.Id);
        dto.Items = items.Adapt<List<TaktMaterialDocumentItemDto>>();
    }

    /// <summary>
    /// 保存物料凭证子表级联（物料凭证行项目；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveMaterialDocumentChildrenAsync(TaktMaterialDocument entity, TaktMaterialDocumentCreateDto dto)
    {
        // 物料凭证行项目（Items）
        List<TaktMaterialDocumentItemUpdateDto>? itemsForSave;
        if (dto is TaktMaterialDocumentUpdateDto updateDto && updateDto.Items != null)
        {
            itemsForSave = updateDto.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktMaterialDocumentItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkMaterialDocumentItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _materialDocumentItemRepository.GetListAsync(x => x.MaterialDocumentId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktMaterialDocumentItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.MaterialDocumentId = entity.Id;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("物料凭证行项目第{i + 1}项与本次提交的其他项重复（CompanyCode、MaterialDocumentId、LineNumber）");
                }
                if (childDto.MaterialDocumentItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.MaterialDocumentItemId, out var target))
                    {
                        throw new TaktBusinessException("物料凭证行项目不存在（MaterialDocumentItemId={childDto.MaterialDocumentItemId}）");
                    }
                    if (target.MaterialDocumentId != entity.Id)
                    {
                        throw new TaktBusinessException("物料凭证行项目不属于当前主表（MaterialDocumentItemId={childDto.MaterialDocumentItemId}）");
                    }
                    submittedIds.Add(childDto.MaterialDocumentItemId);
                    var isUniqueUpdate_ix_takt_logistics_materials_material_document_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _materialDocumentItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.MaterialDocumentId == x.MaterialDocumentId
                && x.LineNumber == x.LineNumber,
                        childDto.MaterialDocumentItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_materials_material_document_item_line_unique)
                    {
                        throw new TaktBusinessException("物料凭证行项目的CompanyCode、MaterialDocumentId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.MaterialDocumentItemId;
                    target.MaterialDocumentId = entity.Id;
                    target.IsObsolete = 0;
                    await _materialDocumentItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_materials_material_document_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _materialDocumentItemRepository,
                        x => x.CompanyCode == x.CompanyCode
                && x.MaterialDocumentId == x.MaterialDocumentId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_materials_material_document_item_line_unique)
                    {
                        throw new TaktBusinessException("物料凭证行项目的CompanyCode、MaterialDocumentId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktMaterialDocumentItem>();
                    child.Id = 0;
                    child.MaterialDocumentId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _materialDocumentItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialDocumentCode) ? entity.MaterialDocumentCode : entity.Id.ToString();
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
                await _materialDocumentItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料凭证查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialDocument, bool>> QueryExpression(TaktMaterialDocumentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialDocument>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDocumentCode != null && x.MaterialDocumentCode.Contains(keywords))
                || (x.PostedBy != null && x.PostedBy.Contains(keywords))
                || SqlFunc.ToString(x.MaterialDocumentStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialDocumentCode))
        {
            exp = exp.And(x => x.MaterialDocumentCode != null && x.MaterialDocumentCode.Contains(queryDto.MaterialDocumentCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PostedBy))
        {
            exp = exp.And(x => x.PostedBy != null && x.PostedBy.Contains(queryDto.PostedBy));
        }

        if (queryDto?.MaterialDocumentStatus.HasValue == true)
        {
            exp = exp.And(x => x.MaterialDocumentStatus == queryDto.MaterialDocumentStatus);
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
