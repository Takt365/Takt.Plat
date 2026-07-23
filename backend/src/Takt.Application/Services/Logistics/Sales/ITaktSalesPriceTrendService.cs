// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSalesPriceTrendService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格月推移 / 机种推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格月推移 / 机种推移分析服务
/// </summary>
public interface ITaktSalesPriceTrendService
{
    /// <summary>
    /// 销售价格月推移转置分析（工厂×物料×客户×月份）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktSalesPriceMonthlyTrendResultDto> GetSalesPriceMonthlyTrendAnalysisAsync(
        TaktSalesPriceMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出销售价格月推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalesPriceMonthlyTrendAnalysisAsync(
        TaktSalesPriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);

    /// <summary>
    /// 销售机种价格推移转置分析（月推移 + BOM 机种/产品组）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktSalesPriceModelTrendResultDto> GetSalesPriceModelTrendAnalysisAsync(
        TaktSalesPriceMonthlyTrendQueryDto queryDto);

    /// <summary>
    /// 导出销售机种价格推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalesPriceModelTrendAnalysisAsync(
        TaktSalesPriceMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
