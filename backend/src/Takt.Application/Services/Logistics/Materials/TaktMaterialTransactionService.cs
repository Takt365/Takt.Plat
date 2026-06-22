// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialTransactionService.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：物料交易应用服务实现
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
/// 物料交易应用服务
/// </summary>
public class TaktMaterialTransactionService : TaktServiceBase, ITaktMaterialTransactionService
{
    private readonly ITaktCompanyRepository<TaktMaterialTransaction> _materialTransactionRepository;
    private readonly ITaktCompanyRepository<TaktMaterialTransactionItem> _materialTransactionItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialTransactionRepository">物料交易仓储</param>
    /// <param name="materialTransactionItemRepository">MaterialTransactionItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialTransactionService(
        ITaktCompanyRepository<TaktMaterialTransaction> materialTransactionRepository,
        ITaktCompanyRepository<TaktMaterialTransactionItem> materialTransactionItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialTransactionRepository = materialTransactionRepository;
        _materialTransactionItemRepository = materialTransactionItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料交易列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialTransactionDto>> GetMaterialTransactionListAsync(TaktMaterialTransactionQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialTransactionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialTransactionDto>.Create(
            data.Adapt<List<TaktMaterialTransactionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料交易
    /// </summary>
    /// <param name="id">物料交易ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialTransactionDto?> GetMaterialTransactionByIdAsync(long id)
    {
        var entity = await _materialTransactionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMaterialTransactionDto>();
        await FillMaterialTransactionDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取物料交易选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialTransactionOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _materialTransactionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TransactionStatus == 1,
            x => x.PartnerName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PartnerName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建物料交易
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialTransactionDto> CreateMaterialTransactionAsync(TaktMaterialTransactionCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialTransaction>();
        var isUnique_ix_takt_logistics_materials_material_transaction_code_unique = await _uniqueValidator.IsUniqueAsync(
            _materialTransactionRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialTransactionCode == entity.MaterialTransactionCode);
        if (!isUnique_ix_takt_logistics_materials_material_transaction_code_unique)
        {
            throw new TaktBusinessException("物料交易的PlantCode、MaterialTransactionCode已存在");
        }
        entity = await _materialTransactionRepository.CreateAsync(entity);
                await SaveMaterialTransactionChildrenAsync(entity, dto);
        return await GetMaterialTransactionByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialTransactionDto>();
    }

    /// <summary>
    /// 更新物料交易
    /// </summary>
    /// <param name="id">物料交易ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialTransactionDto> UpdateMaterialTransactionAsync(long id, TaktMaterialTransactionUpdateDto dto)
    {
        var entity = await _materialTransactionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料交易不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_material_transaction_code_unique = await _uniqueValidator.IsUniqueAsync(
            _materialTransactionRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialTransactionCode == entity.MaterialTransactionCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_transaction_code_unique)
        {
            throw new TaktBusinessException("物料交易的PlantCode、MaterialTransactionCode已存在");
        }
        await _materialTransactionRepository.UpdateAsync(entity);
                await SaveMaterialTransactionChildrenAsync(entity, dto);
        return await GetMaterialTransactionByIdAsync(id) ?? throw new TaktBusinessException("物料交易不存在");
    }

    /// <summary>
    /// 删除物料交易
    /// </summary>
    /// <param name="id">物料交易ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialTransactionByIdAsync(long id)
    {
        var entity = await _materialTransactionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料交易不存在或已删除");
        }
        await _materialTransactionItemRepository.DeleteAsync(x => x.MaterialTransactionId == entity.Id);
        var deleted = await _materialTransactionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料交易不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料交易
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialTransactionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialTransactionByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新物料交易状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialTransactionDto> UpdateMaterialTransactionStatusAsync(TaktMaterialTransactionStatusDto dto)
    {
        var entity = await _materialTransactionRepository.GetByIdAsync(dto.MaterialTransactionId);
        if (entity == null)
        {
            throw new TaktBusinessException("物料交易不存在");
        }
        entity.TransactionStatus = dto.TransactionStatus;
        await _materialTransactionRepository.UpdateAsync(entity);
        return await GetMaterialTransactionByIdAsync(dto.MaterialTransactionId) ?? throw new TaktBusinessException("物料交易不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialTransactionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialTransactionTemplateDto>(
            sheetName ?? "物料交易导入模板",
            fileName ?? "物料交易导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料交易
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialTransactionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialTransactionImportDto>(fileStream, sheetName ?? "物料交易导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialTransaction>();
                var importKey = $"{entity.PlantCode}|{entity.MaterialTransactionCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MaterialTransactionCode）");
                }
                var isUnique_ix_takt_logistics_materials_material_transaction_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialTransactionRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MaterialTransactionCode == entity.MaterialTransactionCode);
                if (!isUnique_ix_takt_logistics_materials_material_transaction_code_unique)
                {
                    throw new TaktBusinessException("物料交易的PlantCode、MaterialTransactionCode已存在");
                }
                await _materialTransactionRepository.CreateAsync(entity);
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
    /// 导出物料交易
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialTransactionAsync(TaktMaterialTransactionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaterialTransactionQueryDto());
        var list = await _materialTransactionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialTransactionExportDto>(),
                sheetName ?? "物料交易数据",
                fileName ?? "物料交易导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialTransactionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料交易数据",
            fileName ?? "物料交易导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充物料交易详情（加载 OneToMany 子表：物料交易明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillMaterialTransactionDetailsAsync(TaktMaterialTransactionDto dto, TaktMaterialTransaction entity)
    {
        if (dto == null)
        {
            return;
        }
        // 物料交易明细 → dto.Items
        var items = await _materialTransactionItemRepository.GetListAsync(x => x.MaterialTransactionId == entity.Id);
        dto.Items = items.Adapt<List<TaktMaterialTransactionItemDto>>();
    }

    /// <summary>
    /// 保存物料交易子表级联（物料交易明细；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveMaterialTransactionChildrenAsync(TaktMaterialTransaction entity, TaktMaterialTransactionCreateDto dto)
    {
        // 物料交易明细（Items）
        if (dto.Items is not { Count: > 0 })
        {
            await _materialTransactionItemRepository.DeleteAsync(x => x.MaterialTransactionId == entity.Id);
        }
        else
        {
            var items = dto.Items.Adapt<List<TaktMaterialTransactionItem>>();
            foreach (var child in items)
            {
                child.MaterialTransactionId = entity.Id;
            }
            var itemsNeedLine = items.Where(c => c.LineNumber <= 0).ToList();
            if (itemsNeedLine.Count > 0)
            {
                var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialTransactionCode) ? entity.MaterialTransactionCode : entity.Id.ToString();
                var maxLine = await _materialTransactionItemRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialTransactionId == entity.Id,
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
                            var key = $"{items[i].CompanyCode}|{items[i].MaterialTransactionId}|{items[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"物料交易明细第{i + 1}项与本次提交的其他项重复（CompanyCode、MaterialTransactionId、LineNumber）");
                            }
                        }
            await _materialTransactionItemRepository.DeleteAsync(x => x.MaterialTransactionId == entity.Id);
            foreach (var child in items)
            {
            var isUnique_ix_takt_logistics_materials_material_transaction_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                _materialTransactionItemRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.MaterialTransactionId == child.MaterialTransactionId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_materials_material_transaction_item_line_unique)
            {
                throw new TaktBusinessException("物料交易明细的CompanyCode、MaterialTransactionId、LineNumber已存在");
            }
            }
            await _materialTransactionItemRepository.CreateRangeAsync(items);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料交易查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialTransaction, bool>> QueryExpression(TaktMaterialTransactionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialTransaction>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialTransactionCode != null && x.MaterialTransactionCode.Contains(keywords))
                || SqlFunc.ToString(x.TransactionDirection).Contains(keywords)
                || SqlFunc.ToString(x.TransactionType).Contains(keywords)
                || SqlFunc.ToString(x.BusinessAction).Contains(keywords)
                || (x.SourceCode != null && x.SourceCode.Contains(keywords))
                || (x.PartnerCode != null && x.PartnerCode.Contains(keywords))
                || (x.PartnerName != null && x.PartnerName.Contains(keywords))
                || (x.WarehouseCode != null && x.WarehouseCode.Contains(keywords))
                || (x.LocationCode != null && x.LocationCode.Contains(keywords))
                || (x.TargetWarehouseCode != null && x.TargetWarehouseCode.Contains(keywords))
                || (x.TargetLocationCode != null && x.TargetLocationCode.Contains(keywords))
                || (x.RelatedCompany != null && x.RelatedCompany.Contains(keywords))
                || SqlFunc.ToString(x.TotalQuantity).Contains(keywords)
                || SqlFunc.ToString(x.TransactionStatus).Contains(keywords)
                || (x.PostedBy != null && x.PostedBy.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.TransactionDate).Contains(keywords)
                || SqlFunc.ToString(x.PostedDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialTransactionCode))
        {
            exp = exp.And(x => x.MaterialTransactionCode != null && x.MaterialTransactionCode.Contains(queryDto.MaterialTransactionCode));
        }

        if (queryDto?.TransactionDirection.HasValue == true)
        {
            exp = exp.And(x => x.TransactionDirection == queryDto.TransactionDirection);
        }

        if (queryDto?.TransactionType.HasValue == true)
        {
            exp = exp.And(x => x.TransactionType == queryDto.TransactionType);
        }

        if (queryDto?.BusinessAction.HasValue == true)
        {
            exp = exp.And(x => x.BusinessAction == queryDto.BusinessAction);
        }

        if (!string.IsNullOrEmpty(queryDto?.SourceCode))
        {
            exp = exp.And(x => x.SourceCode != null && x.SourceCode.Contains(queryDto.SourceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PartnerCode))
        {
            exp = exp.And(x => x.PartnerCode != null && x.PartnerCode.Contains(queryDto.PartnerCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PartnerName))
        {
            exp = exp.And(x => x.PartnerName != null && x.PartnerName.Contains(queryDto.PartnerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.WarehouseCode))
        {
            exp = exp.And(x => x.WarehouseCode != null && x.WarehouseCode.Contains(queryDto.WarehouseCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.LocationCode))
        {
            exp = exp.And(x => x.LocationCode != null && x.LocationCode.Contains(queryDto.LocationCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetWarehouseCode))
        {
            exp = exp.And(x => x.TargetWarehouseCode != null && x.TargetWarehouseCode.Contains(queryDto.TargetWarehouseCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetLocationCode))
        {
            exp = exp.And(x => x.TargetLocationCode != null && x.TargetLocationCode.Contains(queryDto.TargetLocationCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedCompany))
        {
            exp = exp.And(x => x.RelatedCompany != null && x.RelatedCompany.Contains(queryDto.RelatedCompany));
        }

        if (queryDto?.TotalQuantity.HasValue == true)
        {
            exp = exp.And(x => x.TotalQuantity == queryDto.TotalQuantity);
        }

        if (queryDto?.TransactionStatus.HasValue == true)
        {
            exp = exp.And(x => x.TransactionStatus == queryDto.TransactionStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.PostedBy))
        {
            exp = exp.And(x => x.PostedBy != null && x.PostedBy.Contains(queryDto.PostedBy));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.TransactionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.TransactionDate >= queryDto.TransactionDateStart);
        }

        if (queryDto?.TransactionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.TransactionDate <= queryDto.TransactionDateEnd);
        }

        if (queryDto?.PostedDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PostedDate >= queryDto.PostedDateStart);
        }

        if (queryDto?.PostedDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PostedDate <= queryDto.PostedDateEnd);
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
