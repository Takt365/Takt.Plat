// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostAnalysesController.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析控制器（转置 / 差异 / 月度涨跌；成本合计/重算/机种月均；与 CostItem CRUD 分离）
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
/// BOM 成本分析控制器（转置 / 差异 / 月度涨跌；成本合计/重算/机种月均；与 CostItem CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM物料成本分析")]
public class TaktBomMaterialCostAnalysesController : TaktControllerBase
{
    private readonly ITaktBomMaterialCostAnalysisService _bomMaterialCostAnalysisService;
    private readonly ITaktBomMaterialCostItemRecalculateBackgroundService _recalculateBackgroundService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostAnalysisService">BOM 成本分析服务</param>
    /// <param name="recalculateBackgroundService">成本合计/重算后台调度</param>
    public TaktBomMaterialCostAnalysesController(
        ITaktBomMaterialCostAnalysisService bomMaterialCostAnalysisService,
        ITaktBomMaterialCostItemRecalculateBackgroundService recalculateBackgroundService)
    {
        _bomMaterialCostAnalysisService = bomMaterialCostAnalysisService;
        _recalculateBackgroundService = recalculateBackgroundService;
    }

    /// <summary>
    /// 三页共用：工厂选项（级联第 1 级；仅当前公司 RelatedPlant 且存在于本表）
    /// </summary>
    /// <returns>下拉选项（通常仅一项）</returns>
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetBomMaterialCostAnalysisPlantOptionsAsync()
    {
        try
        {
            var result = await _bomMaterialCostAnalysisService.GetBomMaterialCostAnalysisPlantOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 三页共用：本表物料类型去重选项（分析视图专用）
    /// <para>数据源仅 takt_bom_material_cost.MaterialType；❌ 非字典 logistics_material_type（CRUD 表单用）。</para>
    /// </summary>
    /// <param name="queryDto">须 PlantCode</param>
    /// <returns>DictValue/DictLabel=MaterialType</returns>
    [HttpGet("material-type-options")]
    public async Task<IActionResult> GetBomMaterialCostAnalysisMaterialTypeOptionsAsync(
        [FromQuery] TaktBomMaterialCostAnalysisMaterialTypeOptionsQueryDto queryDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(queryDto?.PlantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _bomMaterialCostAnalysisService.GetBomMaterialCostAnalysisMaterialTypeOptionsAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 三页共用：本表机种去重选项（分析视图；须工厂 + MaterialType）
    /// <para>数据源仅 takt_logistics_manufacturing_bom_material_cost.ModelCode；❌ 非 CRUD 主数据 TaktModelDestination。</para>
    /// </summary>
    /// <param name="queryDto">机种选项查询</param>
    /// <returns>下拉选项</returns>
    [HttpGet("model-options")]
    public async Task<IActionResult> GetBomMaterialCostAnalysisModelOptionsAsync(
        [FromQuery] TaktBomMaterialCostAnalysisModelOptionsQueryDto queryDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(queryDto?.PlantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _bomMaterialCostAnalysisService.GetBomMaterialCostAnalysisModelOptionsAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 三页共用：本表物料/产品去重选项（级联第 3 级）
    /// <para>数据源仅 takt_logistics_manufacturing_bom_material_cost.ProductCode（按工厂+物料类型+可选机种去重），非 MaterialPlant、非字典。</para>
    /// </summary>
    /// <param name="queryDto">须 PlantCode；建议 MaterialType；ModelCode 可空</param>
    /// <returns>DictValue=ProductCode；DictLabel=编码+描述；ExtValue=ModelCode；ExtLabel=MaterialType</returns>
    [HttpGet("product-options")]
    public async Task<IActionResult> GetBomMaterialCostAnalysisProductOptionsAsync(
        [FromQuery] TaktBomMaterialCostAnalysisProductOptionsQueryDto queryDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(queryDto?.PlantCode))
            {
                return Success(new List<TaktSelectOption>(), "查询成功");
            }
            var result = await _bomMaterialCostAnalysisService.GetBomMaterialCostAnalysisProductOptionsAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 BOM 物料成本转置列表（行=产品，列=月份总成本）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:list", "BOM成本分析转置列表")]
    [HttpGet("transposed")]
    public async Task<IActionResult> GetBomMaterialCostAnalysisTransposedListAsync(
        [FromQuery] TaktBomMaterialCostAnalysisTransposedQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostAnalysisService.GetBomMaterialCostAnalysisTransposedListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 BOM 物料成本转置报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:export", "导出BOM成本分析转置")]
    [HttpGet("transposed/export")]
    public async Task<IActionResult> ExportBomMaterialCostAnalysisTransposedAsync(
        [FromQuery] TaktBomMaterialCostAnalysisTransposedQueryDto? query = null,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostAnalysisService.ExportBomMaterialCostAnalysisTransposedAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 BOM 物料成本差异分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>差异分析结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:query", "BOM成本分析差异")]
    [HttpGet("variance-analysis")]
    public async Task<IActionResult> GetBomMaterialCostAnalysisVarianceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostAnalysisVarianceQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostAnalysisService.GetBomMaterialCostAnalysisVarianceAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 BOM 物料成本差异分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:export", "导出BOM成本分析差异")]
    [HttpGet("variance-analysis/export")]
    public async Task<IActionResult> ExportBomMaterialCostAnalysisVarianceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostAnalysisVarianceQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostAnalysisService.ExportBomMaterialCostAnalysisVarianceAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 BOM 物料成本月度涨跌分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>月度涨跌结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:query", "BOM成本分析月度涨跌")]
    [HttpGet("monthly-trend-analysis")]
    public async Task<IActionResult> GetBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        [FromQuery] TaktBomMaterialCostAnalysisMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostAnalysisService.GetBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 BOM 物料成本月度涨跌分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:export", "导出BOM成本分析月度涨跌")]
    [HttpGet("monthly-trend-analysis/export")]
    public async Task<IActionResult> ExportBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        [FromQuery] TaktBomMaterialCostAnalysisMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostAnalysisService.ExportBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 提交后台成本合计/重算（明细 Sync 主表；完成后 SignalR 通知触发用户）
    /// </summary>
    /// <param name="queryDto">与明细列表相同的筛选（须单个核算月；忽略分页）</param>
    /// <param name="forceRecalculate">为 true 时按重置成本路径排队</param>
    /// <param name="processRecordCount">处理工厂+产品组上限（0=全部；默认 5000）</param>
    /// <returns>已提交回执</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:update", "合计或重算BOM物料成本")]
    [HttpPut("recalculate-model-average")]
    public async Task<IActionResult> RecalculateBomMaterialCostItemModelMonthlyAverageAsync(
        [FromQuery] TaktBomMaterialCostItemQueryDto queryDto,
        [FromQuery] bool forceRecalculate = false,
        [FromQuery] int processRecordCount = 5000)
    {
        try
        {
            var prepared = TaktBomMaterialCostAnalysisService.PrepareRecalculateModelAverageQuery(queryDto);
            await _recalculateBackgroundService.EnqueueRecalculateAsync(
                prepared.Query,
                forceRecalculate,
                processRecordCount);
            var submitted = new TaktBomMaterialCostItemRecalculateSubmittedDto
            {
                ProcessedMonth = prepared.ProcessedMonth,
                ForceRecalculate = forceRecalculate,
            };
            return Success(submitted, "已提交后台重算，完成后将通知您");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 同步刷新主表机种编码 / 物料类型 / 机种月均（不改产品月成本、不扫明细）
    /// </summary>
    /// <param name="queryDto">工厂 + 核算期间；机种可选</param>
    /// <returns>刷新结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:update", "回填BOM机种价格")]
    [HttpPost("refresh-model-fields")]
    public async Task<IActionResult> RefreshBomMaterialCostModelFieldsAsync(
        [FromBody] TaktBomMaterialCostRefreshModelQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostAnalysisService.RefreshBomMaterialCostModelFieldsAsync(queryDto);
            return Success(result, "机种字段刷新完成");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
