// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：ITaktPurchaseModelTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购机种价格推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购机种价格推移分析服务（月推移 + BOM 机种/产品组；与 CRUD 服务分离）
/// </summary>
public interface ITaktPurchaseModelTrendService
{
    /// <summary>
    /// 推移查询栏：采购价格本表工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchaseModelTrendPlantOptionsAsync();

    /// <summary>
    /// 推移查询栏：按工厂去重条件类型（级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchaseModelTrendPriceTypeOptionsAsync(string plantCode);

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重供应商（级联第 3 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchaseModelTrendSupplierOptionsAsync(
        string plantCode,
        string? priceType = null);

    /// <summary>
    /// 推移查询栏：按工厂+条件类型+供应商去重物料（级联第 4 级，查询时可空）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="supplierCode">供应商编码</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchaseModelTrendMaterialOptionsAsync(
        string plantCode,
        string? priceType = null,
        string? supplierCode = null);

    /// <summary>
    /// 采购机种价格推移转置分析（月推移 + BOM 机种/产品组）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktPurchaseModelTrendResultDto> GetPurchaseModelTrendAnalysisAsync(
        TaktPurchaseModelTrendQueryDto queryDto);

    /// <summary>
    /// 导出采购机种价格推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPurchaseModelTrendAnalysisAsync(
        TaktPurchaseModelTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
