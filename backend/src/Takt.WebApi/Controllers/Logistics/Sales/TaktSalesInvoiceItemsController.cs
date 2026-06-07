// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesInvoiceItemsController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：销售发票明细控制器
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
/// 销售发票明细控制器
/// 提供销售发票明细的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "销售发票明细")]
public class TaktSalesInvoiceItemsController : TaktControllerBase
{
    private readonly ITaktSalesInvoiceItemService _salesInvoiceItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesInvoiceItemService">销售发票明细服务</param>
    public TaktSalesInvoiceItemsController(ITaktSalesInvoiceItemService salesInvoiceItemService)
    {
        _salesInvoiceItemService = salesInvoiceItemService;
    }

    /// <summary>
    /// 获取销售发票明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:salesinvoiceitem:list", "销售发票明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesInvoiceItemListAsync([FromQuery] TaktSalesInvoiceItemQueryDto queryDto)
    {
        try
        {
            var result = await _salesInvoiceItemService.GetSalesInvoiceItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售发票明细
    /// </summary>
    /// <param name="id">销售发票明细ID</param>
    /// <returns>销售发票明细DTO</returns>
    [TaktPermission("logistics:sales:salesinvoiceitem:query", "销售发票明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalesInvoiceItemByIdAsync(long id)
    {
        try
        {
            var result = await _salesInvoiceItemService.GetSalesInvoiceItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售发票明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售发票明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:salesinvoiceitem:query", "销售发票明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesInvoiceItemOptionsAsync()
    {
        try
        {
            var result = await _salesInvoiceItemService.GetSalesInvoiceItemOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售发票明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售发票明细DTO</returns>
    [TaktPermission("logistics:sales:salesinvoiceitem:create", "创建销售发票明细")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesInvoiceItemAsync([FromBody] TaktSalesInvoiceItemCreateDto dto)
    {
        try
        {
            var result = await _salesInvoiceItemService.CreateSalesInvoiceItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售发票明细
    /// </summary>
    /// <param name="id">销售发票明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售发票明细DTO</returns>
    [TaktPermission("logistics:sales:salesinvoiceitem:update", "更新销售发票明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesInvoiceItemAsync(long id, [FromBody] TaktSalesInvoiceItemUpdateDto dto)
    {
        try
        {
            var result = await _salesInvoiceItemService.UpdateSalesInvoiceItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售发票明细
    /// </summary>
    /// <param name="id">销售发票明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:salesinvoiceitem:delete", "删除销售发票明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesInvoiceItemByIdAsync(long id)
    {
        try
        {
            await _salesInvoiceItemService.DeleteSalesInvoiceItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售发票明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:salesinvoiceitem:delete", "批量删除销售发票明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesInvoiceItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesInvoiceItemService.DeleteSalesInvoiceItemBatchAsync(ids);
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
    [TaktPermission("logistics:sales:salesinvoiceitem:import", "获取销售发票明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesInvoiceItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesInvoiceItemService.GetSalesInvoiceItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售发票明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:salesinvoiceitem:import", "导入销售发票明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesInvoiceItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesInvoiceItemService.ImportSalesInvoiceItemAsync(stream, sheetName);
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
    /// 导出销售发票明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:salesinvoiceitem:export", "导出销售发票明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesInvoiceItemAsync([FromQuery] TaktSalesInvoiceItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesInvoiceItemService.ExportSalesInvoiceItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
