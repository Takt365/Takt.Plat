// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesPriceItemsController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格明细控制器
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
/// 销售价格明细控制器
/// 提供销售价格明细的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "销售价格明细")]
public class TaktSalesPriceItemsController : TaktControllerBase
{
    private readonly ITaktSalesPriceItemService _salesPriceItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceItemService">销售价格明细服务</param>
    public TaktSalesPriceItemsController(ITaktSalesPriceItemService salesPriceItemService)
    {
        _salesPriceItemService = salesPriceItemService;
    }

    /// <summary>
    /// 获取销售价格明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:salespriceitem:list", "销售价格明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesPriceItemListAsync([FromQuery] TaktSalesPriceItemQueryDto queryDto)
    {
        try
        {
            var result = await _salesPriceItemService.GetSalesPriceItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售价格明细
    /// </summary>
    /// <param name="id">销售价格明细ID</param>
    /// <returns>销售价格明细DTO</returns>
    [TaktPermission("logistics:sales:salespriceitem:query", "销售价格明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalesPriceItemByIdAsync(long id)
    {
        try
        {
            var result = await _salesPriceItemService.GetSalesPriceItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售价格明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售价格明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:salespriceitem:query", "销售价格明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesPriceItemOptionsAsync()
    {
        try
        {
            var result = await _salesPriceItemService.GetSalesPriceItemOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售价格明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售价格明细DTO</returns>
    [TaktPermission("logistics:sales:salespriceitem:create", "创建销售价格明细")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesPriceItemAsync([FromBody] TaktSalesPriceItemCreateDto dto)
    {
        try
        {
            var result = await _salesPriceItemService.CreateSalesPriceItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售价格明细
    /// </summary>
    /// <param name="id">销售价格明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售价格明细DTO</returns>
    [TaktPermission("logistics:sales:salespriceitem:update", "更新销售价格明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesPriceItemAsync(long id, [FromBody] TaktSalesPriceItemUpdateDto dto)
    {
        try
        {
            var result = await _salesPriceItemService.UpdateSalesPriceItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售价格明细
    /// </summary>
    /// <param name="id">销售价格明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:salespriceitem:delete", "删除销售价格明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesPriceItemByIdAsync(long id)
    {
        try
        {
            await _salesPriceItemService.DeleteSalesPriceItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售价格明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:salespriceitem:delete", "批量删除销售价格明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesPriceItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesPriceItemService.DeleteSalesPriceItemBatchAsync(ids);
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
    [TaktPermission("logistics:sales:salespriceitem:import", "获取销售价格明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesPriceItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesPriceItemService.GetSalesPriceItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售价格明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:salespriceitem:import", "导入销售价格明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesPriceItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesPriceItemService.ImportSalesPriceItemAsync(stream, sheetName);
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
    /// 导出销售价格明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:salespriceitem:export", "导出销售价格明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesPriceItemAsync([FromQuery] TaktSalesPriceItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesPriceItemService.ExportSalesPriceItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
