// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesPricesController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格控制器
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
/// 销售价格控制器
/// 提供销售价格的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售价格")]
public class TaktSalesPricesController : TaktControllerBase
{
    private readonly ITaktSalesPriceService _salesPriceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceService">销售价格服务</param>
    public TaktSalesPricesController(ITaktSalesPriceService salesPriceService)
    {
        _salesPriceService = salesPriceService;
    }

    /// <summary>
    /// 获取销售价格列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:price:list", "销售价格列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesPriceListAsync([FromQuery] TaktSalesPriceQueryDto queryDto)
    {
        try
        {
            var result = await _salesPriceService.GetSalesPriceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售价格
    /// </summary>
    /// <param name="id">销售价格ID</param>
    /// <returns>销售价格DTO</returns>
    [TaktPermission("logistics:sales:price:query", "销售价格详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalesPriceByIdAsync(long id)
    {
        try
        {
            var result = await _salesPriceService.GetSalesPriceByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售价格不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售价格选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:price:query", "销售价格选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesPriceOptionsAsync()
    {
        try
        {
            var result = await _salesPriceService.GetSalesPriceOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售价格DTO</returns>
    [TaktPermission("logistics:sales:price:create", "创建销售价格")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesPriceAsync([FromBody] TaktSalesPriceCreateDto dto)
    {
        try
        {
            var result = await _salesPriceService.CreateSalesPriceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售价格
    /// </summary>
    /// <param name="id">销售价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售价格DTO</returns>
    [TaktPermission("logistics:sales:price:update", "更新销售价格")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesPriceAsync(long id, [FromBody] TaktSalesPriceUpdateDto dto)
    {
        try
        {
            var result = await _salesPriceService.UpdateSalesPriceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售价格
    /// </summary>
    /// <param name="id">销售价格ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:price:delete", "删除销售价格")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesPriceByIdAsync(long id)
    {
        try
        {
            await _salesPriceService.DeleteSalesPriceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:price:delete", "批量删除销售价格")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesPriceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesPriceService.DeleteSalesPriceBatchAsync(ids);
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
    [TaktPermission("logistics:sales:price:import", "获取销售价格导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesPriceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesPriceService.GetSalesPriceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售价格
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:price:import", "导入销售价格")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesPriceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesPriceService.ImportSalesPriceAsync(stream, sheetName);
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
    /// 导出销售价格
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:price:export", "导出销售价格")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesPriceAsync([FromQuery] TaktSalesPriceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesPriceService.ExportSalesPriceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
