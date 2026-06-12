// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesPriceScalesController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格阶梯控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Application.Services.Logistics.Sales;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Sales;

/// <summary>
/// 销售价格阶梯控制器
/// 提供销售价格阶梯的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售价格阶梯")]
public class TaktSalesPriceScalesController : TaktControllerBase
{
    private readonly ITaktSalesPriceScaleService _salesPriceScaleService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceScaleService">销售价格阶梯服务</param>
    public TaktSalesPriceScalesController(ITaktSalesPriceScaleService salesPriceScaleService)
    {
        _salesPriceScaleService = salesPriceScaleService;
    }

    /// <summary>
    /// 获取销售价格阶梯列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:salespricescale:list", "销售价格阶梯列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesPriceScaleListAsync([FromQuery] TaktSalesPriceScaleQueryDto queryDto)
    {
        try
        {
            var result = await _salesPriceScaleService.GetSalesPriceScaleListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售价格阶梯
    /// </summary>
    /// <param name="id">销售价格阶梯ID</param>
    /// <returns>销售价格阶梯DTO</returns>
    [TaktPermission("logistics:sales:salespricescale:query", "销售价格阶梯详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalesPriceScaleByIdAsync(long id)
    {
        try
        {
            var result = await _salesPriceScaleService.GetSalesPriceScaleByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售价格阶梯不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售价格阶梯选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:salespricescale:query", "销售价格阶梯选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesPriceScaleOptionsAsync()
    {
        try
        {
            var result = await _salesPriceScaleService.GetSalesPriceScaleOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售价格阶梯
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售价格阶梯DTO</returns>
    [TaktPermission("logistics:sales:salespricescale:create", "创建销售价格阶梯")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesPriceScaleAsync([FromBody] TaktSalesPriceScaleCreateDto dto)
    {
        try
        {
            var result = await _salesPriceScaleService.CreateSalesPriceScaleAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售价格阶梯
    /// </summary>
    /// <param name="id">销售价格阶梯ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售价格阶梯DTO</returns>
    [TaktPermission("logistics:sales:salespricescale:update", "更新销售价格阶梯")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesPriceScaleAsync(long id, [FromBody] TaktSalesPriceScaleUpdateDto dto)
    {
        try
        {
            var result = await _salesPriceScaleService.UpdateSalesPriceScaleAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售价格阶梯
    /// </summary>
    /// <param name="id">销售价格阶梯ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:salespricescale:delete", "删除销售价格阶梯")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesPriceScaleByIdAsync(long id)
    {
        try
        {
            await _salesPriceScaleService.DeleteSalesPriceScaleByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售价格阶梯
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:salespricescale:delete", "批量删除销售价格阶梯")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesPriceScaleBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesPriceScaleService.DeleteSalesPriceScaleBatchAsync(ids);
            return Success("删除成功");
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
    [TaktPermission("logistics:sales:salespricescale:import", "获取销售价格阶梯导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesPriceScaleTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesPriceScaleService.GetSalesPriceScaleTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售价格阶梯
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:salespricescale:import", "导入销售价格阶梯")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesPriceScaleAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesPriceScaleService.ImportSalesPriceScaleAsync(stream, sheetName);
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
    /// 导出销售价格阶梯
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:salespricescale:export", "导出销售价格阶梯")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesPriceScaleAsync([FromQuery] TaktSalesPriceScaleQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesPriceScaleService.ExportSalesPriceScaleAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
