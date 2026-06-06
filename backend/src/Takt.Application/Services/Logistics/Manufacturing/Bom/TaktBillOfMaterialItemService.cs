// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialItemService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：物料清单明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 物料清单明细应用服务
/// </summary>
public class TaktBillOfMaterialItemService : TaktServiceBase, ITaktBillOfMaterialItemService
{
    private readonly ITaktCompanyRepository<TaktBillOfMaterialItem> _billOfMaterialItemRepository;
    private readonly ITaktCompanyRepository<TaktBillOfMaterial> _billOfMaterialRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialItemRepository">物料清单明细仓储</param>
    /// <param name="billOfMaterialRepository">物料清单仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBillOfMaterialItemService(
        ITaktCompanyRepository<TaktBillOfMaterialItem> billOfMaterialItemRepository,
        ITaktCompanyRepository<TaktBillOfMaterial> billOfMaterialRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _billOfMaterialItemRepository = billOfMaterialItemRepository;
        _billOfMaterialRepository = billOfMaterialRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取物料清单明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBillOfMaterialItemDto>> GetBillOfMaterialItemListAsync(TaktBillOfMaterialItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _billOfMaterialItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBillOfMaterialItemDto>.Create(
            data.Adapt<List<TaktBillOfMaterialItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取物料清单明细
    /// </summary>
    /// <param name="id">物料清单明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialItemDto?> GetBillOfMaterialItemByIdAsync(long id)
    {
        var entity = await _billOfMaterialItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBillOfMaterialItemDto>();
    }

    /// <summary>
    /// 获取物料清单明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBillOfMaterialItemOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _billOfMaterialItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MaterialCode,
        }).ToList();
    }

    /// <summary>
    /// 创建物料清单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialItemDto> CreateBillOfMaterialItemAsync(TaktBillOfMaterialItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktBillOfMaterialItem>();
                await StampBillOfMaterialItemBillOfMaterialAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialItemRepository,
            x => x.BillOfMaterialId == entity.BillOfMaterialId
                && x.LineNumber == entity.LineNumber
                && x.MaterialId == entity.MaterialId);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique)
        {
            throw new TaktBusinessException("物料清单明细的BillOfMaterialId、LineNumber、MaterialId已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _billOfMaterialItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BillOfMaterialId == entity.BillOfMaterialId,
                x => x.LineNumber);
            var businessCode = entity.BillOfMaterialId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _billOfMaterialItemRepository.CreateAsync(entity);
        return await GetBillOfMaterialItemByIdAsync(entity.Id) ?? entity.Adapt<TaktBillOfMaterialItemDto>();
    }

    /// <summary>
    /// 更新物料清单明细
    /// </summary>
    /// <param name="id">物料清单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialItemDto> UpdateBillOfMaterialItemAsync(long id, TaktBillOfMaterialItemUpdateDto dto)
    {
        var entity = await _billOfMaterialItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("物料清单明细不存在");
        }
        dto.Adapt(entity);
                await StampBillOfMaterialItemBillOfMaterialAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialItemRepository,
            x => x.BillOfMaterialId == entity.BillOfMaterialId
                && x.LineNumber == entity.LineNumber
                && x.MaterialId == entity.MaterialId,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique)
        {
            throw new TaktBusinessException("物料清单明细的BillOfMaterialId、LineNumber、MaterialId已存在");
        }
        await _billOfMaterialItemRepository.UpdateAsync(entity);
        return await GetBillOfMaterialItemByIdAsync(id) ?? throw new TaktBusinessException("物料清单明细不存在");
    }

    /// <summary>
    /// 删除物料清单明细
    /// </summary>
    /// <param name="id">物料清单明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialItemByIdAsync(long id)
    {
        var deleted = await _billOfMaterialItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("物料清单明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除物料清单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBillOfMaterialItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBillOfMaterialItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBillOfMaterialItemTemplateDto>(
            sheetName ?? "物料清单明细导入模板",
            fileName ?? "物料清单明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入物料清单明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBillOfMaterialItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBillOfMaterialItemImportDto>(fileStream, sheetName ?? "物料清单明细导入模板");
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
                var entity = rows[i].Adapt<TaktBillOfMaterialItem>();
                var importDto = rows[i].Adapt<TaktBillOfMaterialItemCreateDto>();
                await StampBillOfMaterialItemBillOfMaterialAsync(entity, importDto);
                var importKey = $"{entity.BillOfMaterialId}|{entity.LineNumber}|{entity.MaterialId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（BillOfMaterialId、LineNumber、MaterialId）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _billOfMaterialItemRepository,
                    x => x.BillOfMaterialId == entity.BillOfMaterialId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialId == entity.MaterialId);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_item_bom_line_unique)
                {
                    throw new TaktBusinessException("物料清单明细的BillOfMaterialId、LineNumber、MaterialId已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _billOfMaterialItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BillOfMaterialId == entity.BillOfMaterialId,
                        x => x.LineNumber);
                    var businessCode = entity.BillOfMaterialId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _billOfMaterialItemRepository.CreateAsync(entity);
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
    /// 导出物料清单明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBillOfMaterialItemAsync(TaktBillOfMaterialItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktBillOfMaterialItemQueryDto());
        var list = await _billOfMaterialItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBillOfMaterialItemExportDto>(),
                sheetName ?? "物料清单明细数据",
                fileName ?? "物料清单明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBillOfMaterialItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "物料清单明细数据",
            fileName ?? "物料清单明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步物料清单明细主表外键（ManyToOne → 物料清单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampBillOfMaterialItemBillOfMaterialAsync(TaktBillOfMaterialItem entity, TaktBillOfMaterialItemCreateDto dto)
    {
        if (dto.BillOfMaterialId <= 0)
        {
            return;
        }
        var master = await _billOfMaterialRepository.GetByIdAsync(dto.BillOfMaterialId);
        if (master == null)
        {
            throw new TaktBusinessException("物料清单不存在");
        }
        entity.BillOfMaterialId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建物料清单明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBillOfMaterialItem, bool>> QueryExpression(TaktBillOfMaterialItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBillOfMaterialItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.BillOfMaterialId).Contains(keywords)
                || (x.BomCode != null && x.BomCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.MaterialId).Contains(keywords)
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.UsageQuantity).Contains(keywords)
                || (x.MaterialUnit != null && x.MaterialUnit.Contains(keywords))
                || SqlFunc.ToString(x.ScrapRate).Contains(keywords)
                || SqlFunc.ToString(x.ActualUsageQuantity).Contains(keywords)
                || SqlFunc.ToString(x.OperationSeq).Contains(keywords)
                || (x.WorkCenter != null && x.WorkCenter.Contains(keywords))
                || (x.Position != null && x.Position.Contains(keywords))
                || (x.SubstituteGroup != null && x.SubstituteGroup.Contains(keywords))
                || SqlFunc.ToString(x.SubstitutePriority).Contains(keywords)
                || SqlFunc.ToString(x.IsOptional).Contains(keywords)
                || SqlFunc.ToString(x.IsPhantom).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.BillOfMaterialId.HasValue == true)
        {
            exp = exp.And(x => x.BillOfMaterialId == queryDto.BillOfMaterialId);
        }

        if (!string.IsNullOrEmpty(queryDto?.BomCode))
        {
            exp = exp.And(x => x.BomCode != null && x.BomCode.Contains(queryDto.BomCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.MaterialId.HasValue == true)
        {
            exp = exp.And(x => x.MaterialId == queryDto.MaterialId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (queryDto?.UsageQuantity.HasValue == true)
        {
            exp = exp.And(x => x.UsageQuantity == queryDto.UsageQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialUnit))
        {
            exp = exp.And(x => x.MaterialUnit != null && x.MaterialUnit.Contains(queryDto.MaterialUnit));
        }

        if (queryDto?.ScrapRate.HasValue == true)
        {
            exp = exp.And(x => x.ScrapRate == queryDto.ScrapRate);
        }

        if (queryDto?.ActualUsageQuantity.HasValue == true)
        {
            exp = exp.And(x => x.ActualUsageQuantity == queryDto.ActualUsageQuantity);
        }

        if (queryDto?.OperationSeq.HasValue == true)
        {
            exp = exp.And(x => x.OperationSeq == queryDto.OperationSeq);
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkCenter))
        {
            exp = exp.And(x => x.WorkCenter != null && x.WorkCenter.Contains(queryDto.WorkCenter));
        }

        if (!string.IsNullOrEmpty(queryDto?.Position))
        {
            exp = exp.And(x => x.Position != null && x.Position.Contains(queryDto.Position));
        }

        if (!string.IsNullOrEmpty(queryDto?.SubstituteGroup))
        {
            exp = exp.And(x => x.SubstituteGroup != null && x.SubstituteGroup.Contains(queryDto.SubstituteGroup));
        }

        if (queryDto?.SubstitutePriority.HasValue == true)
        {
            exp = exp.And(x => x.SubstitutePriority == queryDto.SubstitutePriority);
        }

        if (queryDto?.IsOptional.HasValue == true)
        {
            exp = exp.And(x => x.IsOptional == queryDto.IsOptional);
        }

        if (queryDto?.IsPhantom.HasValue == true)
        {
            exp = exp.And(x => x.IsPhantom == queryDto.IsPhantom);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
