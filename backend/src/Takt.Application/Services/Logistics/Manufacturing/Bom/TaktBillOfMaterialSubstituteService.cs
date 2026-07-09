// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialSubstituteService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM替代料应用服务实现
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
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM替代料应用服务
/// </summary>
public class TaktBillOfMaterialSubstituteService : TaktServiceBase, ITaktBillOfMaterialSubstituteService
{
    private readonly ITaktCompanyRepository<TaktBillOfMaterialSubstitute> _billOfMaterialSubstituteRepository;
    private readonly ITaktCompanyRepository<TaktBillOfMaterialItem> _billOfMaterialItemRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="billOfMaterialSubstituteRepository">BOM替代料仓储</param>
    /// <param name="billOfMaterialItemRepository">物料清单明细仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBillOfMaterialSubstituteService(
        ITaktCompanyRepository<TaktBillOfMaterialSubstitute> billOfMaterialSubstituteRepository,
        ITaktCompanyRepository<TaktBillOfMaterialItem> billOfMaterialItemRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _billOfMaterialSubstituteRepository = billOfMaterialSubstituteRepository;
        _billOfMaterialItemRepository = billOfMaterialItemRepository;
        _materialPlantRepository = materialPlantRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取BOM替代料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBillOfMaterialSubstituteDto>> GetBillOfMaterialSubstituteListAsync(TaktBillOfMaterialSubstituteQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _billOfMaterialSubstituteRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBillOfMaterialSubstituteDto>.Create(
            data.Adapt<List<TaktBillOfMaterialSubstituteDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialSubstituteDto?> GetBillOfMaterialSubstituteByIdAsync(long id)
    {
        var entity = await _billOfMaterialSubstituteRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBillOfMaterialSubstituteDto>();
    }

    /// <summary>
    /// 获取BOM替代料选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBillOfMaterialSubstituteOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _billOfMaterialSubstituteRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.BomCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.BomCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建BOM替代料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialSubstituteDto> CreateBillOfMaterialSubstituteAsync(TaktBillOfMaterialSubstituteCreateDto dto)
    {
        var entity = dto.Adapt<TaktBillOfMaterialSubstitute>();
        entity.IsObsolete = 0;
        await StampBillOfMaterialSubstituteBillOfMaterialItemAsync(entity, dto);
        await StampBillOfMaterialSubstituteMaterialPlantAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialSubstituteRepository,
            x => x.BillOfMaterialItemId == entity.BillOfMaterialItemId
                && x.SubstituteMaterialId == entity.SubstituteMaterialId);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique)
        {
            throw new TaktBusinessException("BOM替代料的BillOfMaterialItemId、SubstituteMaterialId已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _billOfMaterialSubstituteRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BillOfMaterialItemId == entity.BillOfMaterialItemId,
                x => x.LineNumber);
            var businessCode = entity.BillOfMaterialItemId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _billOfMaterialSubstituteRepository.CreateAsync(entity);
        return await GetBillOfMaterialSubstituteByIdAsync(entity.Id) ?? entity.Adapt<TaktBillOfMaterialSubstituteDto>();
    }

    /// <summary>
    /// 更新BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialSubstituteDto> UpdateBillOfMaterialSubstituteAsync(long id, TaktBillOfMaterialSubstituteUpdateDto dto)
    {
        var entity = await _billOfMaterialSubstituteRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM替代料不存在");
        }
        dto.Adapt(entity);
        await StampBillOfMaterialSubstituteBillOfMaterialItemAsync(entity, dto);
        await StampBillOfMaterialSubstituteMaterialPlantAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique = await _uniqueValidator.IsUniqueAsync(
            _billOfMaterialSubstituteRepository,
            x => x.BillOfMaterialItemId == entity.BillOfMaterialItemId
                && x.SubstituteMaterialId == entity.SubstituteMaterialId,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique)
        {
            throw new TaktBusinessException("BOM替代料的BillOfMaterialItemId、SubstituteMaterialId已存在");
        }
        await _billOfMaterialSubstituteRepository.UpdateAsync(entity);
        return await GetBillOfMaterialSubstituteByIdAsync(id) ?? throw new TaktBusinessException("BOM替代料不存在");
    }

    /// <summary>
    /// 删除BOM替代料
    /// </summary>
    /// <param name="id">BOM替代料ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialSubstituteByIdAsync(long id)
    {
        var entity = await _billOfMaterialSubstituteRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM替代料不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("BOM替代料不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("BOM替代料已作废");
        }
        entity.IsObsolete = 1;
        await _billOfMaterialSubstituteRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除BOM替代料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBillOfMaterialSubstituteBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBillOfMaterialSubstituteByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新BOM替代料作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBillOfMaterialSubstituteDto> UpdateBillOfMaterialSubstituteObsoleteAsync(TaktBillOfMaterialSubstituteObsoleteDto dto)
    {
        var entity = await _billOfMaterialSubstituteRepository.GetByIdAsync(dto.BillOfMaterialSubstituteId);
        if (entity == null)
        {
            throw new TaktBusinessException("BOM替代料不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("BOM替代料不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _billOfMaterialSubstituteRepository.UpdateAsync(entity);
        return await GetBillOfMaterialSubstituteByIdAsync(dto.BillOfMaterialSubstituteId) ?? throw new TaktBusinessException("BOM替代料不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBillOfMaterialSubstituteTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBillOfMaterialSubstituteTemplateDto>(
            sheetName ?? "BOM替代料导入模板",
            fileName ?? "BOM替代料导入模板.xlsx");
    }

    /// <summary>
    /// 导入BOM替代料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBillOfMaterialSubstituteAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBillOfMaterialSubstituteImportDto>(fileStream, sheetName ?? "BOM替代料导入模板");
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
                var entity = rows[i].Adapt<TaktBillOfMaterialSubstitute>();
                var importDto = rows[i].Adapt<TaktBillOfMaterialSubstituteCreateDto>();
                await StampBillOfMaterialSubstituteBillOfMaterialItemAsync(entity, importDto);
                await StampBillOfMaterialSubstituteMaterialPlantAsync(entity, importDto);
                var importKey = $"{entity.BillOfMaterialItemId}|{entity.SubstituteMaterialId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（BillOfMaterialItemId、SubstituteMaterialId）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique = await _uniqueValidator.IsUniqueAsync(
                    _billOfMaterialSubstituteRepository,
                    x => x.BillOfMaterialItemId == entity.BillOfMaterialItemId
                        && x.SubstituteMaterialId == entity.SubstituteMaterialId);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_substitute_item_material_unique)
                {
                    throw new TaktBusinessException("BOM替代料的BillOfMaterialItemId、SubstituteMaterialId已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _billOfMaterialSubstituteRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BillOfMaterialItemId == entity.BillOfMaterialItemId,
                        x => x.LineNumber);
                    var businessCode = entity.BillOfMaterialItemId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _billOfMaterialSubstituteRepository.CreateAsync(entity);
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
    /// 导出BOM替代料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBillOfMaterialSubstituteAsync(TaktBillOfMaterialSubstituteQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktBillOfMaterialSubstituteQueryDto());
        var list = await _billOfMaterialSubstituteRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBillOfMaterialSubstituteExportDto>(),
                sheetName ?? "BOM替代料数据",
                fileName ?? "BOM替代料导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBillOfMaterialSubstituteExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "BOM替代料数据",
            fileName ?? "BOM替代料导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步BOM替代料主表外键（ManyToOne → 物料清单明细）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampBillOfMaterialSubstituteBillOfMaterialItemAsync(TaktBillOfMaterialSubstitute entity, TaktBillOfMaterialSubstituteCreateDto dto)
    {
        if (dto.BillOfMaterialItemId <= 0)
        {
            return;
        }
        var master = await _billOfMaterialItemRepository.GetByIdAsync(dto.BillOfMaterialItemId);
        if (master == null)
        {
            throw new TaktBusinessException("物料清单明细不存在");
        }
        entity.BillOfMaterialItemId = master.Id;
    }

    /// <summary>
    /// 同步BOM替代料主表外键（ManyToOne → 工厂物料）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampBillOfMaterialSubstituteMaterialPlantAsync(TaktBillOfMaterialSubstitute entity, TaktBillOfMaterialSubstituteCreateDto dto)
    {
        if (dto.SubstituteMaterialId <= 0)
        {
            return;
        }
        var master = await _materialPlantRepository.GetByIdAsync(dto.SubstituteMaterialId);
        if (master == null)
        {
            throw new TaktBusinessException("工厂物料不存在");
        }
        entity.SubstituteMaterialId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建BOM替代料查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBillOfMaterialSubstitute, bool>> QueryExpression(TaktBillOfMaterialSubstituteQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBillOfMaterialSubstitute>();

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
                SqlFunc.ToString(x.BillOfMaterialItemId).Contains(keywords)
                || SqlFunc.ToString(x.BillOfMaterialId).Contains(keywords)
                || (x.BomCode != null && x.BomCode.Contains(keywords))
                || (x.PrimaryMaterialCode != null && x.PrimaryMaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || SqlFunc.ToString(x.SubstituteMaterialId).Contains(keywords)
                || (x.SubstituteMaterialCode != null && x.SubstituteMaterialCode.Contains(keywords))
                || (x.SubstituteGroup != null && x.SubstituteGroup.Contains(keywords))
                || SqlFunc.ToString(x.SubstitutePriority).Contains(keywords)
                || SqlFunc.ToString(x.UsageQuantity).Contains(keywords)
                || (x.MaterialUnit != null && x.MaterialUnit.Contains(keywords))
                || SqlFunc.ToString(x.UsageRatio).Contains(keywords)
                || SqlFunc.ToString(x.IsEnabled).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EffectiveDate).Contains(keywords)
                || SqlFunc.ToString(x.ExpiryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.BillOfMaterialItemId.HasValue == true)
        {
            exp = exp.And(x => x.BillOfMaterialItemId == queryDto.BillOfMaterialItemId);
        }

        if (queryDto?.BillOfMaterialId.HasValue == true)
        {
            exp = exp.And(x => x.BillOfMaterialId == queryDto.BillOfMaterialId);
        }

        if (!string.IsNullOrEmpty(queryDto?.BomCode))
        {
            exp = exp.And(x => x.BomCode != null && x.BomCode.Contains(queryDto.BomCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PrimaryMaterialCode))
        {
            exp = exp.And(x => x.PrimaryMaterialCode != null && x.PrimaryMaterialCode.Contains(queryDto.PrimaryMaterialCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (queryDto?.SubstituteMaterialId.HasValue == true)
        {
            exp = exp.And(x => x.SubstituteMaterialId == queryDto.SubstituteMaterialId);
        }

        if (!string.IsNullOrEmpty(queryDto?.SubstituteMaterialCode))
        {
            exp = exp.And(x => x.SubstituteMaterialCode != null && x.SubstituteMaterialCode.Contains(queryDto.SubstituteMaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SubstituteGroup))
        {
            exp = exp.And(x => x.SubstituteGroup != null && x.SubstituteGroup.Contains(queryDto.SubstituteGroup));
        }

        if (queryDto?.SubstitutePriority.HasValue == true)
        {
            exp = exp.And(x => x.SubstitutePriority == queryDto.SubstitutePriority);
        }

        if (queryDto?.UsageQuantity.HasValue == true)
        {
            exp = exp.And(x => x.UsageQuantity == queryDto.UsageQuantity);
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialUnit))
        {
            exp = exp.And(x => x.MaterialUnit != null && x.MaterialUnit.Contains(queryDto.MaterialUnit));
        }

        if (queryDto?.UsageRatio.HasValue == true)
        {
            exp = exp.And(x => x.UsageRatio == queryDto.UsageRatio);
        }

        if (queryDto?.IsEnabled.HasValue == true)
        {
            exp = exp.And(x => x.IsEnabled == queryDto.IsEnabled);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate >= queryDto.EffectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate <= queryDto.EffectiveDateEnd);
        }

        if (queryDto?.ExpiryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate >= queryDto.ExpiryDateStart);
        }

        if (queryDto?.ExpiryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate <= queryDto.ExpiryDateEnd);
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
