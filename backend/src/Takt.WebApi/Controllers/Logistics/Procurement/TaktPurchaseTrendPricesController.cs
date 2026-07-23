// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseTrendPricesController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格月推移 / 机种推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Procurement;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Procurement;

/// <summary>
/// 采购价格月推移 / 机种推移转置分析控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购价格推移")]
public class TaktPurchaseTrendPricesController : TaktControllerBase
{
    private readonly ITaktPurchaseTrendPriceService _purchaseTrendPriceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseTrendPriceService">采购价格推移服务</param>
    public TaktPurchaseTrendPricesController(ITaktPurchaseTrendPriceService purchaseTrendPriceService)
    {
        _purchaseTrendPriceService = purchaseTrendPriceService;
    }

    /// <summary>
    /// 采购价格月推移转置分析（工厂×物料×供应商×月份）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:trend:list", "采购价格推移")]
    [HttpGet("monthly-trend-analysis")]
    public async Task<IActionResult> GetPurchasePriceMonthlyTrendAnalysisAsync(
        [FromQuery] TaktPurchasePriceMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseTrendPriceService.GetPurchasePriceMonthlyTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：采购价格月推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:procurement:purchase:price:trend:export", "清单导出采购价格推移")]
    [HttpGet("monthly-trend-analysis/export")]
    public async Task<IActionResult> ExportPurchasePriceMonthlyTrendAnalysisAsync(
        [FromQuery] TaktPurchasePriceMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseTrendPriceService.ExportPurchasePriceMonthlyTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 采购机种价格推移转置分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:trend:list", "采购机种价格推移")]
    [HttpGet("model-trend-analysis")]
    public async Task<IActionResult> GetPurchasePriceModelTrendAnalysisAsync(
        [FromQuery] TaktPurchasePriceMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseTrendPriceService.GetPurchasePriceModelTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：采购机种价格推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:procurement:purchase:price:trend:export", "清单导出采购机种价格推移")]
    [HttpGet("model-trend-analysis/export")]
    public async Task<IActionResult> ExportPurchasePriceModelTrendAnalysisAsync(
        [FromQuery] TaktPurchasePriceMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseTrendPriceService.ExportPurchasePriceModelTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
