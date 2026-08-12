// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktMaterialMovingPriceTrendService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料月移动价格推移 / 机种推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料月移动价格推移 / 机种推移分析服务（读移动价格本表；与 CRUD 服务分离）
/// </summary>
public interface ITaktMaterialMovingPriceTrendService
{
    /// <summary>
    /// 推移查询栏：移动价格本表工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMaterialMovingPriceTrendPlantOptionsAsync();

    /// <summary>
    /// 推移查询栏：按工厂去重评估类别（级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMaterialMovingPriceTrendValuationOptionsAsync(string plantCode);

    /// <summary>
    /// 推移查询栏：按工厂+评估类别去重物料（级联第 3 级，查询时可空）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="valuation">评估类别</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMaterialMovingPriceTrendMaterialOptionsAsync(
        string plantCode,
        string? valuation = null);

    /// <summary>
    /// 物料月移动价格推移分析（分页）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktMaterialMovingPriceMonthlyTrendResultDto> GetMaterialMovingPriceMonthlyTrendAnalysisAsync(
        TaktMaterialMovingPriceMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出物料月移动价格推移分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMaterialMovingPriceMonthlyTrendAnalysisAsync(
        TaktMaterialMovingPriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 物料-机种-价格推移分析（物料清单 + BOM 机种/产品组）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktMaterialMovingPriceModelTrendResultDto> GetMaterialMovingPriceModelTrendAnalysisAsync(
        TaktMaterialMovingPriceMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出物料-机种-价格推移分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMaterialMovingPriceModelTrendAnalysisAsync(
        TaktMaterialMovingPriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
