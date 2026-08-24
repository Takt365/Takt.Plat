// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialModelTrendsController.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料机种推移转置分析控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 物料机种推移转置分析控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料机种推移")]
public class TaktMaterialModelTrendsController : TaktControllerBase
{
    private readonly ITaktMaterialModelTrendService _materialModelTrendService;

    public TaktMaterialModelTrendsController(ITaktMaterialModelTrendService materialModelTrendService)
    {
        _materialModelTrendService = materialModelTrendService;
    }

    [TaktPermission("logistics:materials:material:model:trend:list", "物料机种推移工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetMaterialModelTrendPlantOptionsAsync()
    {
        try
        {
            var result = await _materialModelTrendService.GetMaterialModelTrendPlantOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [TaktPermission("logistics:materials:material:model:trend:list", "物料机种推移评估类别选项")]
    [HttpGet("valuation-options")]
    public async Task<IActionResult> GetMaterialModelTrendValuationOptionsAsync([FromQuery] string plantCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode)) return Success(new List<TaktSelectOption>(), "查询成功");
            var result = await _materialModelTrendService.GetMaterialModelTrendValuationOptionsAsync(plantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [TaktPermission("logistics:materials:material:model:trend:list", "物料机种推移物料选项")]
    [HttpGet("material-options")]
    public async Task<IActionResult> GetMaterialModelTrendMaterialOptionsAsync(
        [FromQuery] string plantCode, [FromQuery] string? valuation = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(valuation))
                return Success(new List<TaktSelectOption>(), "查询成功");
            var result = await _materialModelTrendService.GetMaterialModelTrendMaterialOptionsAsync(plantCode, valuation);
            return Success(result, "查询成功");
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [TaktPermission("logistics:materials:material:model:trend:list", "物料机种推移")]
    [HttpGet("trend-analysis")]
    public async Task<IActionResult> GetMaterialModelTrendAnalysisAsync([FromQuery] TaktMaterialModelTrendQueryDto queryDto)
    {
        try
        {
            var result = await _materialModelTrendService.GetMaterialModelTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex) { return HandleException(ex); }
    }

    [TaktPermission("logistics:materials:material:model:trend:export", "清单导出物料机种推移")]
    [HttpGet("trend-analysis/export")]
    public async Task<IActionResult> ExportMaterialModelTrendAnalysisAsync(
        [FromQuery] TaktMaterialModelTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialModelTrendService.ExportMaterialModelTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex) { return HandleException(ex); }
    }
}
