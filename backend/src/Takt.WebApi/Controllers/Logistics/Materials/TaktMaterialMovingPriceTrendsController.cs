// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialMovingPriceTrendsController.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：物料月移动价格推移 / 机种推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 物料月移动价格推移 / 机种推移转置分析控制器（与 TaktMaterialMovingPrices CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料移动价格推移")]
public class TaktMaterialMovingPriceTrendsController : TaktControllerBase
{
    private readonly ITaktMaterialMovingPriceTrendService _materialMovingPriceTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialMovingPriceTrendService">物料移动价格推移服务</param>
    public TaktMaterialMovingPriceTrendsController(ITaktMaterialMovingPriceTrendService materialMovingPriceTrendService)
    {
        _materialMovingPriceTrendService = materialMovingPriceTrendService;
    }

    /// <summary>
    /// 推移查询栏：移动价格本表工厂去重选项
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:moving:trend:list", "物料移动价格推移工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetMaterialMovingPriceTrendPlantOptionsAsync()
    {
        try
        {
            var result = await _materialMovingPriceTrendService.GetMaterialMovingPriceTrendPlantOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂去重评估类别
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:moving:trend:list", "物料移动价格推移评估类别选项")]
    [HttpGet("valuation-options")]
    public async Task<IActionResult> GetMaterialMovingPriceTrendValuationOptionsAsync([FromQuery] string plantCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _materialMovingPriceTrendService.GetMaterialMovingPriceTrendValuationOptionsAsync(plantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 推移查询栏：按工厂+评估类别去重物料
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="valuation">评估类别</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:moving:trend:list", "物料移动价格推移物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetMaterialMovingPriceTrendMaterialOptionsAsync(
        [FromQuery] string plantCode,
        [FromQuery] string? valuation = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(valuation))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _materialMovingPriceTrendService.GetMaterialMovingPriceTrendMaterialOptionsAsync(
                plantCode, valuation);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 物料月移动价格推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分析结果</returns>
    [TaktPermission("logistics:materials:material:moving:trend:list", "物料移动价格推移")]
    [HttpGet("monthly-trend-analysis")]
    public async Task<IActionResult> GetMaterialMovingPriceMonthlyTrendAnalysisAsync(
        [FromQuery] TaktMaterialMovingPriceMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _materialMovingPriceTrendService.GetMaterialMovingPriceMonthlyTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：物料月移动价格推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:materials:material:moving:trend:export", "清单导出物料移动价格推移")]
    [HttpGet("monthly-trend-analysis/export")]
    public async Task<IActionResult> ExportMaterialMovingPriceMonthlyTrendAnalysisAsync(
        [FromQuery] TaktMaterialMovingPriceMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialMovingPriceTrendService.ExportMaterialMovingPriceMonthlyTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 物料-机种-价格推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    [TaktPermission("logistics:materials:model:moving:trend:list", "机种移动推移")]
    [HttpGet("model-trend-analysis")]
    public async Task<IActionResult> GetMaterialMovingPriceModelTrendAnalysisAsync(
        [FromQuery] TaktMaterialMovingPriceMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _materialMovingPriceTrendService.GetMaterialMovingPriceModelTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 清单导出：机种移动推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:materials:model:moving:trend:export", "清单导出机种移动推移")]
    [HttpGet("model-trend-analysis/export")]
    public async Task<IActionResult> ExportMaterialMovingPriceModelTrendAnalysisAsync(
        [FromQuery] TaktMaterialMovingPriceMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialMovingPriceTrendService.ExportMaterialMovingPriceModelTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
