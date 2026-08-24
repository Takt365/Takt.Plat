// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesModelTrendsController.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售机种销售推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Application.Services.Logistics.Sales;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Sales;

/// <summary>
/// 销售机种销售推移转置分析控制器（与 TaktSalesPrices CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "机种销售推移")]
public class TaktSalesModelTrendsController : TaktControllerBase
{
    private readonly ITaktSalesModelTrendService _salesModelTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesModelTrendService">销售机种销售推移服务</param>
    public TaktSalesModelTrendsController(ITaktSalesModelTrendService salesModelTrendService)
    {
        _salesModelTrendService = salesModelTrendService;
    }

    /// <summary>
    /// 推移查询栏：销售价格本表工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:model:trend:list", "机种销售推移工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetSalesModelTrendPlantOptionsAsync()
    {
        try
        {
            var result = await _salesModelTrendService.GetSalesModelTrendPlantOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂去重条件类型
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:model:trend:list", "机种销售推移条件类型选项")]
    [HttpGet("price-type-options")]
    public async Task<IActionResult> GetSalesModelTrendPriceTypeOptionsAsync([FromQuery] string plantCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _salesModelTrendService.GetSalesModelTrendPriceTypeOptionsAsync(plantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重客户
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:model:trend:list", "机种销售推移客户选项")]
    [HttpGet("customer-options")]
    public async Task<IActionResult> GetSalesModelTrendCustomerOptionsAsync(
        [FromQuery] string plantCode,
        [FromQuery] string? priceType = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(priceType))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _salesModelTrendService.GetSalesModelTrendCustomerOptionsAsync(plantCode, priceType);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂+条件类型+客户去重物料
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="customerCode">客户编码</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:model:trend:list", "机种销售推移物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetSalesModelTrendMaterialOptionsAsync(
        [FromQuery] string plantCode,
        [FromQuery] string? priceType = null,
        [FromQuery] string? customerCode = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode)
                || string.IsNullOrWhiteSpace(priceType)
                || string.IsNullOrWhiteSpace(customerCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _salesModelTrendService.GetSalesModelTrendMaterialOptionsAsync(
                plantCode, priceType, customerCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 销售机种销售推移转置分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:sales:model:trend:list", "机种销售推移")]
    [HttpGet("trend-analysis")]
    public async Task<IActionResult> GetSalesModelTrendAnalysisAsync(
        [FromQuery] TaktSalesModelTrendQueryDto queryDto)
    {
        try
        {
            var result = await _salesModelTrendService.GetSalesModelTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：销售机种销售推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:sales:model:trend:export", "清单导出机种销售推移")]
    [HttpGet("trend-analysis/export")]
    public async Task<IActionResult> ExportSalesModelTrendAnalysisAsync(
        [FromQuery] TaktSalesModelTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesModelTrendService.ExportSalesModelTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

