// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：ITaktPurchaseTrendPriceService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格月推移 / 机种推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价格月推移 / 机种推移分析服务
/// </summary>
public interface ITaktPurchaseTrendPriceService
{
    /// <summary>
    /// 采购价格月推移转置分析（工厂×物料×供应商×月份）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktPurchasePriceMonthlyTrendResultDto> GetPurchasePriceMonthlyTrendAnalysisAsync(
        TaktPurchasePriceMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出采购价格月推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPurchasePriceMonthlyTrendAnalysisAsync(
        TaktPurchasePriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 采购机种价格推移转置分析（月推移 + BOM 机种/产品组）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktPurchasePriceModelTrendResultDto> GetPurchasePriceModelTrendAnalysisAsync(
        TaktPurchasePriceMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出采购机种价格推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPurchasePriceModelTrendAnalysisAsync(
        TaktPurchasePriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
