// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemsController.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM物料成本明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM物料成本明细控制器
/// 提供BOM物料成本明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM物料成本明细")]
public class TaktBomMaterialCostItemsController : TaktControllerBase
{
    private readonly ITaktBomMaterialCostItemService _bomMaterialCostItemService;
    private readonly ITaktBomMaterialCostItemRecalculateBackgroundService _recalculateBackgroundService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemService">BOM物料成本明细服务</param>
    /// <param name="recalculateBackgroundService">机种月平均重算后台调度</param>
    public TaktBomMaterialCostItemsController(
        ITaktBomMaterialCostItemService bomMaterialCostItemService,
        ITaktBomMaterialCostItemRecalculateBackgroundService recalculateBackgroundService)
    {
        _bomMaterialCostItemService = bomMaterialCostItemService;
        _recalculateBackgroundService = recalculateBackgroundService;
    }

    /// <summary>
    /// 获取BOM物料成本明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:list", "BOM物料成本明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBomMaterialCostItemListAsync([FromQuery] TaktBomMaterialCostItemQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 BOM 物料成本明细转置列表（行=产品，列=月份总成本）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:list", "BOM成本分析转置列表")]
    [HttpGet("transposed")]
    public async Task<IActionResult> GetBomMaterialCostItemTransposedListAsync([FromQuery] TaktBomMaterialCostItemTransposedQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemTransposedListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 BOM 物料成本明细转置报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:export", "导出BOM成本分析转置")]
    [HttpGet("transposed/export")]
    public async Task<IActionResult> ExportBomMaterialCostItemTransposedAsync(
        [FromQuery] TaktBomMaterialCostItemTransposedQueryDto? query = null,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostItemService.ExportBomMaterialCostItemTransposedAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 BOM 物料成本明细差异分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>差异分析结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:query", "BOM成本分析差异")]
    [HttpGet("variance-analysis")]
    public async Task<IActionResult> GetBomMaterialCostItemVarianceAnalysisAsync([FromQuery] TaktBomMaterialCostItemVarianceQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemVarianceAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 BOM 物料成本明细差异分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:export", "导出BOM成本分析差异")]
    [HttpGet("variance-analysis/export")]
    public async Task<IActionResult> ExportBomMaterialCostItemVarianceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostItemVarianceQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostItemService.ExportBomMaterialCostItemVarianceAnalysisAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 BOM 物料成本明细月度涨跌分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>月度涨跌结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:query", "BOM物料成本月度涨跌分析")]
    [HttpGet("monthly-trend-analysis")]
    public async Task<IActionResult> GetBomMaterialCostItemMonthlyTrendAnalysisAsync([FromQuery] TaktBomMaterialCostItemMonthlyTrendQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemMonthlyTrendAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 BOM 物料成本明细月度涨跌分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:export", "导出BOM物料成本月度涨跌")]
    [HttpGet("monthly-trend-analysis/export")]
    public async Task<IActionResult> ExportBomMaterialCostItemMonthlyTrendAnalysisAsync(
        [FromQuery] TaktBomMaterialCostItemMonthlyTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostItemService.ExportBomMaterialCostItemMonthlyTrendAnalysisAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 产品成本分析：单个产品下明细组件 × 月材料成本转置
    /// </summary>
    /// <param name="queryDto">查询 DTO（工厂 + 产品必填）</param>
    /// <returns>明细组件月材料成本结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:trend:list", "产品成本分析")]
    [HttpGet("component-moving-price-analysis")]
    public async Task<IActionResult> GetBomMaterialCostItemComponentMovingPriceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostItemComponentMovingPriceQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemComponentMovingPriceAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出产品成本分析（单个产品明细组件×月材料成本）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:trend:export", "导出产品成本分析")]
    [HttpGet("component-moving-price-analysis/export")]
    public async Task<IActionResult> ExportBomMaterialCostItemComponentMovingPriceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostItemComponentMovingPriceQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostItemService.ExportBomMaterialCostItemComponentMovingPriceAnalysisAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 机种成本推移：机种月材料成本 + 合并键分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>机种月成本与合并键分析行</returns>
    [TaktPermission("logistics:manufacturing:bom:model:moving:price:list", "机种成本推移")]
    [HttpGet("model-moving-price-analysis")]
    public async Task<IActionResult> GetBomMaterialCostItemModelMovingPriceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostItemModelMovingPriceQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemModelMovingPriceAnalysisAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出机种成本推移分析报表
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:model:moving:price:export", "导出机种成本推移")]
    [HttpGet("model-moving-price-analysis/export")]
    public async Task<IActionResult> ExportBomMaterialCostItemModelMovingPriceAnalysisAsync(
        [FromQuery] TaktBomMaterialCostItemModelMovingPriceQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostItemService.ExportBomMaterialCostItemModelMovingPriceAnalysisAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 机种零价格合并清单（X+F、移动平均价=0，按组件合并产品）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>合并分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:list", "BOM零价格合并清单")]
    [HttpGet("zero-moving-price-merged")]
    public async Task<IActionResult> GetBomMaterialCostItemZeroMovingPriceMergedAsync(
        [FromQuery] TaktBomMaterialCostItemZeroMovingPriceQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemZeroMovingPriceMergedAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出机种零价格合并清单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:export", "导出BOM零价格合并清单")]
    [HttpGet("zero-moving-price-merged/export")]
    public async Task<IActionResult> ExportBomMaterialCostItemZeroMovingPriceMergedAsync(
        [FromQuery] TaktBomMaterialCostItemZeroMovingPriceQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostItemService.ExportBomMaterialCostItemZeroMovingPriceMergedAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取 BOM 物料成本机种下拉选项
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:query", "BOM物料成本机种选项")]
    [HttpGet("model-options")]
    public async Task<IActionResult> GetBomMaterialCostItemModelOptionsAsync([FromQuery] string? plantCode = null)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemModelOptionsAsync(plantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 按机种获取 BOM 物料成本产品下拉选项
    /// </summary>
    /// <param name="modelCode">机种编码</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:query", "BOM物料成本产品选项")]
    [HttpGet("product-options-by-model")]
    public async Task<IActionResult> GetBomMaterialCostItemProductOptionsByModelAsync(
        [FromQuery] string? modelCode = null,
        [FromQuery] string? plantCode = null)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemProductOptionsByModelAsync(modelCode, plantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据产品编码反查机种编码
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>机种编码</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:query", "BOM物料成本反查机种")]
    [HttpGet("model-by-product")]
    public async Task<IActionResult> GetBomMaterialCostItemModelCodeByProductAsync(
        [FromQuery] string productCode,
        [FromQuery] string? plantCode = null)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemModelCodeByProductAsync(productCode, plantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>BOM物料成本明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:query", "BOM物料成本明细详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetBomMaterialCostItemByIdAsync(long id)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("BOM物料成本明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取BOM物料成本选项列表（按产品编码去重，可选按工厂过滤）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:query", "BOM物料成本明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetBomMaterialCostItemOptionsAsync([FromQuery] string? plantCode = null)
    {
        try
        {
            var result = await _bomMaterialCostItemService.GetBomMaterialCostItemOptionsAsync(plantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建BOM物料成本明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>BOM物料成本明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:create", "创建BOM物料成本明细")]
    [HttpPost]
    public async Task<IActionResult> CreateBomMaterialCostItemAsync([FromBody] TaktBomMaterialCostItemCreateDto dto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.CreateBomMaterialCostItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>BOM物料成本明细DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:update", "更新BOM物料成本明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBomMaterialCostItemAsync(long id, [FromBody] TaktBomMaterialCostItemUpdateDto dto)
    {
        try
        {
            var result = await _bomMaterialCostItemService.UpdateBomMaterialCostItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除BOM物料成本明细
    /// </summary>
    /// <param name="id">BOM物料成本明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:delete", "删除BOM物料成本明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBomMaterialCostItemByIdAsync(long id)
    {
        try
        {
            await _bomMaterialCostItemService.DeleteBomMaterialCostItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除BOM物料成本明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:delete", "批量删除BOM物料成本明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteBomMaterialCostItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _bomMaterialCostItemService.DeleteBomMaterialCostItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 提交后台重算（按明细 Sync 汇总表；完成后 SignalR 通知触发用户）
    /// </summary>
    /// <param name="queryDto">与明细列表相同的筛选（须单个核算月；忽略分页）</param>
    /// <param name="forceRecalculate">为 true 时按重置成本路径排队</param>
    /// <param name="processRecordCount">处理记录数上限（工厂+产品组；0=全部；默认 5000）</param>
    /// <returns>已提交响应</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:update", "重算BOM物料成本")]
    [HttpPut("recalculate-model-average")]
    public async Task<IActionResult> RecalculateBomMaterialCostItemModelMonthlyAverageAsync(
        [FromQuery] TaktBomMaterialCostItemQueryDto queryDto,
        [FromQuery] bool forceRecalculate = false,
        [FromQuery] int processRecordCount = 5000)
    {
        try
        {
            var prepared = TaktBomMaterialCostItemService.PrepareRecalculateModelAverageQuery(queryDto);
            await _recalculateBackgroundService.EnqueueRecalculateAsync(
                prepared.Query,
                forceRecalculate,
                processRecordCount);
            var submitted = new TaktBomMaterialCostItemRecalculateSubmittedDto
            {
                ProcessedMonth = prepared.ProcessedMonth,
                ForceRecalculate = forceRecalculate,
                ProcessRecordCount = processRecordCount,
            };
            return Success(submitted, "已提交后台重算，完成后将通知您");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:import", "获取BOM物料成本明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetBomMaterialCostItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _bomMaterialCostItemService.GetBomMaterialCostItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入BOM物料成本明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:import", "导入BOM物料成本明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportBomMaterialCostItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _bomMaterialCostItemService.ImportBomMaterialCostItemAsync(stream, sheetName);
            return Success(new
            {
                SuccessCount = success,
                FailCount = fail,
                Errors = errors
            }, $"导入完成：成功{success}条，失败{fail}条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出BOM物料成本明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:export", "导出BOM物料成本明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBomMaterialCostItemAsync([FromQuery] TaktBomMaterialCostItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostItemService.ExportBomMaterialCostItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出 BOM 成本分析明细清单（权限对齐分析页 analysis:export）
    /// </summary>
    /// <param name="query">与明细列表相同的筛选</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:material:cost:analysis:export", "导出BOM成本分析明细清单")]
    [HttpGet("analysis-items/export")]
    public async Task<IActionResult> ExportBomMaterialCostItemAnalysisListAsync(
        [FromQuery] TaktBomMaterialCostItemQueryDto? query = null,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialCostItemService.ExportBomMaterialCostItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
