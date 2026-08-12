// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomVarianceCostTrendsController.cs
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 差异成本推移分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 差异成本推移分析控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM差异成本推移")]
public class TaktBomVarianceCostTrendsController : TaktControllerBase
{
    private readonly ITaktBomVarianceCostTrendService _bomVarianceCostTrendService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomVarianceCostTrendService">差异成本推移服务</param>
    public TaktBomVarianceCostTrendsController(ITaktBomVarianceCostTrendService bomVarianceCostTrendService)
    {
        _bomVarianceCostTrendService = bomVarianceCostTrendService;
    }

    /// <summary>
    /// 机种选项（工厂 + 期间最后月）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:variance:cost:trend:query", "差异成本推移机种选项")]
    [HttpGet("model-options")]
    public async Task<IActionResult> GetBomVarianceCostTrendModelOptionsAsync(
        [FromQuery] TaktBomVarianceCostTrendOptionsQueryDto queryDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(queryDto.PlantCode) || string.IsNullOrWhiteSpace(queryDto.FocusPeriod))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _bomVarianceCostTrendService.GetBomVarianceCostTrendModelOptionsAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 产品选项（工厂 + 期间最后月 + 已选机种；与机种联动）
    /// </summary>
    /// <param name="queryDto">查询（须含机种）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:variance:cost:trend:query", "差异成本推移产品选项")]
    [HttpGet("product-options")]
    public async Task<IActionResult> GetBomVarianceCostTrendProductOptionsAsync(
        [FromQuery] TaktBomVarianceCostTrendOptionsQueryDto queryDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(queryDto.PlantCode)
                || string.IsNullOrWhiteSpace(queryDto.FocusPeriod)
                || (string.IsNullOrWhiteSpace(queryDto.ModelCodes) && string.IsNullOrWhiteSpace(queryDto.ModelCode)))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _bomVarianceCostTrendService.GetBomVarianceCostTrendProductOptionsAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 差异成本推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    [TaktPermission("logistics:manufacturing:bom:variance:cost:trend:list", "差异成本推移")]
    [HttpGet("variance-cost-trend-analysis")]
    public async Task<IActionResult> GetBomVarianceCostTrendAnalysisAsync(
        [FromQuery] TaktBomVarianceCostTrendQueryDto queryDto)
    {
        try
        {
            var result = await _bomVarianceCostTrendService.GetBomVarianceCostTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出差异成本推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:variance:cost:trend:export", "导出差异成本推移")]
    [HttpGet("variance-cost-trend-analysis/export")]
    public async Task<IActionResult> ExportBomVarianceCostTrendAnalysisAsync(
        [FromQuery] TaktBomVarianceCostTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomVarianceCostTrendService.ExportBomVarianceCostTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
