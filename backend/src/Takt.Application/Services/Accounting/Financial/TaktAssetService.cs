// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktAssetService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：资产应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 资产应用服务
/// </summary>
public class TaktAssetService : TaktServiceBase, ITaktAssetService
{
    private readonly ITaktCompanyRepository<TaktAsset> _assetRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assetRepository">资产仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssetService(
        ITaktCompanyRepository<TaktAsset> assetRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assetRepository = assetRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取资产列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssetDto>> GetAssetListAsync(TaktAssetQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assetRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssetDto>.Create(
            data.Adapt<List<TaktAssetDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssetDto?> GetAssetByIdAsync(long id)
    {
        var entity = await _assetRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAssetDto>();
    }

    /// <summary>
    /// 获取固定资产选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssetOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _assetRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.AssetName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.AssetName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建资产
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssetDto> CreateAssetAsync(TaktAssetCreateDto dto)
    {
        var entity = dto.Adapt<TaktAsset>();
        var isUnique_ix_asset_code_unique = await _uniqueValidator.IsUniqueAsync(
            _assetRepository,
            x => x.AssetCode == entity.AssetCode);
        if (!isUnique_ix_asset_code_unique)
        {
            throw new TaktBusinessException("资产的AssetCode已存在");
        }
        entity = await _assetRepository.CreateAsync(entity);
        return await GetAssetByIdAsync(entity.Id) ?? entity.Adapt<TaktAssetDto>();
    }

    /// <summary>
    /// 更新资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssetDto> UpdateAssetAsync(long id, TaktAssetUpdateDto dto)
    {
        var entity = await _assetRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("资产不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_asset_code_unique = await _uniqueValidator.IsUniqueAsync(
            _assetRepository,
            x => x.AssetCode == entity.AssetCode,
            id);
        if (!isUnique_ix_asset_code_unique)
        {
            throw new TaktBusinessException("资产的AssetCode已存在");
        }
        await _assetRepository.UpdateAsync(entity);
        return await GetAssetByIdAsync(id) ?? throw new TaktBusinessException("资产不存在");
    }

    /// <summary>
    /// 删除资产
    /// </summary>
    /// <param name="id">资产ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssetByIdAsync(long id)
    {
        var deleted = await _assetRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("资产不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除资产
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssetBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssetByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新资产状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssetDto> UpdateAssetStatusAsync(TaktAssetStatusDto dto)
    {
        var entity = await _assetRepository.GetByIdAsync(dto.AssetId);
        if (entity == null)
        {
            throw new TaktBusinessException("资产不存在");
        }
        entity.AssetStatus = dto.AssetStatus;
        await _assetRepository.UpdateAsync(entity);
        return await GetAssetByIdAsync(dto.AssetId) ?? throw new TaktBusinessException("资产不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssetTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssetTemplateDto>(
            sheetName ?? "资产导入模板",
            fileName ?? "资产导入模板.xlsx");
    }

    /// <summary>
    /// 导入资产
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssetAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssetImportDto>(fileStream, sheetName ?? "资产导入模板");
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
                var entity = rows[i].Adapt<TaktAsset>();
                var importKey = $"{entity.AssetCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（AssetCode）");
                }
                var isUnique_ix_asset_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _assetRepository,
                    x => x.AssetCode == entity.AssetCode);
                if (!isUnique_ix_asset_code_unique)
                {
                    throw new TaktBusinessException("资产的AssetCode已存在");
                }
                await _assetRepository.CreateAsync(entity);
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
    /// 导出资产
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssetAsync(TaktAssetQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAssetQueryDto());
        var list = await _assetRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssetExportDto>(),
                sheetName ?? "资产数据",
                fileName ?? "资产导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssetExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "资产数据",
            fileName ?? "资产导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建资产查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAsset, bool>> QueryExpression(TaktAssetQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAsset>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.AssetCode != null && x.AssetCode.Contains(keywords))
                || (x.AssetName != null && x.AssetName.Contains(keywords))
                || SqlFunc.ToString(x.AssetCategoryId).Contains(keywords)
                || (x.AssetCategoryName != null && x.AssetCategoryName.Contains(keywords))
                || SqlFunc.ToString(x.AssetType).Contains(keywords)
                || SqlFunc.ToString(x.AssetOriginalValue).Contains(keywords)
                || SqlFunc.ToString(x.AssetNetValue).Contains(keywords)
                || SqlFunc.ToString(x.AccumulatedDepreciation).Contains(keywords)
                || SqlFunc.ToString(x.CostCenterId).Contains(keywords)
                || (x.CostCenterName != null && x.CostCenterName.Contains(keywords))
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || SqlFunc.ToString(x.UserId).Contains(keywords)
                || (x.UserName != null && x.UserName.Contains(keywords))
                || (x.AssetLocation != null && x.AssetLocation.Contains(keywords))
                || SqlFunc.ToString(x.ExpectedLifeMonths).Contains(keywords)
                || SqlFunc.ToString(x.DepreciationMethod).Contains(keywords)
                || SqlFunc.ToString(x.MonthlyDepreciation).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || SqlFunc.ToString(x.AssetStatus).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PurchaseDate).Contains(keywords)
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.ScrapDate).Contains(keywords)
                || SqlFunc.ToString(x.DisposalDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.AssetCode))
        {
            exp = exp.And(x => x.AssetCode != null && x.AssetCode.Contains(queryDto.AssetCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.AssetName))
        {
            exp = exp.And(x => x.AssetName != null && x.AssetName.Contains(queryDto.AssetName));
        }

        if (queryDto?.AssetCategoryId.HasValue == true)
        {
            exp = exp.And(x => x.AssetCategoryId == queryDto.AssetCategoryId);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssetCategoryName))
        {
            exp = exp.And(x => x.AssetCategoryName != null && x.AssetCategoryName.Contains(queryDto.AssetCategoryName));
        }

        if (queryDto?.AssetType.HasValue == true)
        {
            exp = exp.And(x => x.AssetType == queryDto.AssetType);
        }

        if (queryDto?.AssetOriginalValue.HasValue == true)
        {
            exp = exp.And(x => x.AssetOriginalValue == queryDto.AssetOriginalValue);
        }

        if (queryDto?.AssetNetValue.HasValue == true)
        {
            exp = exp.And(x => x.AssetNetValue == queryDto.AssetNetValue);
        }

        if (queryDto?.AccumulatedDepreciation.HasValue == true)
        {
            exp = exp.And(x => x.AccumulatedDepreciation == queryDto.AccumulatedDepreciation);
        }

        if (queryDto?.CostCenterId.HasValue == true)
        {
            exp = exp.And(x => x.CostCenterId == queryDto.CostCenterId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCenterName))
        {
            exp = exp.And(x => x.CostCenterName != null && x.CostCenterName.Contains(queryDto.CostCenterName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (queryDto?.UserId.HasValue == true)
        {
            exp = exp.And(x => x.UserId == queryDto.UserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.UserName))
        {
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(queryDto.UserName));
        }

        if (!string.IsNullOrEmpty(queryDto?.AssetLocation))
        {
            exp = exp.And(x => x.AssetLocation != null && x.AssetLocation.Contains(queryDto.AssetLocation));
        }

        if (queryDto?.ExpectedLifeMonths.HasValue == true)
        {
            exp = exp.And(x => x.ExpectedLifeMonths == queryDto.ExpectedLifeMonths);
        }

        if (queryDto?.DepreciationMethod.HasValue == true)
        {
            exp = exp.And(x => x.DepreciationMethod == queryDto.DepreciationMethod);
        }

        if (queryDto?.MonthlyDepreciation.HasValue == true)
        {
            exp = exp.And(x => x.MonthlyDepreciation == queryDto.MonthlyDepreciation);
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (queryDto?.AssetStatus.HasValue == true)
        {
            exp = exp.And(x => x.AssetStatus == queryDto.AssetStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PurchaseDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseDate >= queryDto.PurchaseDateStart);
        }

        if (queryDto?.PurchaseDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PurchaseDate <= queryDto.PurchaseDateEnd);
        }

        if (queryDto?.StartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.StartDate >= queryDto.StartDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartDate <= queryDto.StartDateEnd);
        }

        if (queryDto?.ScrapDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ScrapDate >= queryDto.ScrapDateStart);
        }

        if (queryDto?.ScrapDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ScrapDate <= queryDto.ScrapDateEnd);
        }

        if (queryDto?.DisposalDateStart.HasValue == true)
        {
            exp = exp.And(x => x.DisposalDate >= queryDto.DisposalDateStart);
        }

        if (queryDto?.DisposalDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.DisposalDate <= queryDto.DisposalDateEnd);
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
