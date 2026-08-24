// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSalesModelTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售机种销售推移分析服务接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售机种销售推移分析服务（月推移 + BOM 机种/产品组；与 CRUD 服务分离）
/// </summary>
public interface ITaktSalesModelTrendService
{
    /// <summary>
    /// 推移查询栏：销售价格本表工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesModelTrendPlantOptionsAsync();

    /// <summary>
    /// 推移查询栏：按工厂去重条件类型（级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesModelTrendPriceTypeOptionsAsync(string plantCode);

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重客户（级联第 3 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesModelTrendCustomerOptionsAsync(
        string plantCode,
        string? priceType = null);

    /// <summary>
    /// 推移查询栏：按工厂+条件类型+客户去重物料（级联第 4 级，查询时可空）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="customerCode">客户编码</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesModelTrendMaterialOptionsAsync(
        string plantCode,
        string? priceType = null,
        string? customerCode = null);

    /// <summary>
    /// 销售机种销售推移转置分析（月推移 + BOM 机种/产品组）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    Task<TaktSalesModelTrendResultDto> GetSalesModelTrendAnalysisAsync(
        TaktSalesModelTrendQueryDto queryDto);

    /// <summary>
    /// 导出销售机种销售推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalesModelTrendAnalysisAsync(
        TaktSalesModelTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null);
}
