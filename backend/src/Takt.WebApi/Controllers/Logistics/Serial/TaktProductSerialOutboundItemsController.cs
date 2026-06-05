// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Serial
// 文件名称：TaktProductSerialOutboundItemsController.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：产品序列号出库明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Serial;
using Takt.Application.Services.Logistics.Serial;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Serial;

/// <summary>
/// 产品序列号出库明细控制器
/// 提供产品序列号出库明细的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "产品序列号出库明细")]
public class TaktProductSerialOutboundItemsController : TaktControllerBase
{
    private readonly ITaktProductSerialOutboundItemService _productSerialOutboundItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productSerialOutboundItemService">产品序列号出库明细服务</param>
    public TaktProductSerialOutboundItemsController(ITaktProductSerialOutboundItemService productSerialOutboundItemService)
    {
        _productSerialOutboundItemService = productSerialOutboundItemService;
    }

    /// <summary>
    /// 获取产品序列号出库明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:serial:productserialoutbounditem:list", "产品序列号出库明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProductSerialOutboundItemListAsync([FromQuery] TaktProductSerialOutboundItemQueryDto queryDto)
    {
        try
        {
            var result = await _productSerialOutboundItemService.GetProductSerialOutboundItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取产品序列号出库明细
    /// </summary>
    /// <param name="id">产品序列号出库明细ID</param>
    /// <returns>产品序列号出库明细DTO</returns>
    [TaktPermission("logistics:serial:productserialoutbounditem:query", "产品序列号出库明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductSerialOutboundItemByIdAsync(long id)
    {
        try
        {
            var result = await _productSerialOutboundItemService.GetProductSerialOutboundItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("产品序列号出库明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取产品序列号出库明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:serial:productserialoutbounditem:query", "产品序列号出库明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProductSerialOutboundItemOptionsAsync()
    {
        try
        {
            var result = await _productSerialOutboundItemService.GetProductSerialOutboundItemOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建产品序列号出库明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>产品序列号出库明细DTO</returns>
    [TaktPermission("logistics:serial:productserialoutbounditem:create", "创建产品序列号出库明细")]
    [HttpPost]
    public async Task<IActionResult> CreateProductSerialOutboundItemAsync([FromBody] TaktProductSerialOutboundItemCreateDto dto)
    {
        try
        {
            var result = await _productSerialOutboundItemService.CreateProductSerialOutboundItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新产品序列号出库明细
    /// </summary>
    /// <param name="id">产品序列号出库明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>产品序列号出库明细DTO</returns>
    [TaktPermission("logistics:serial:productserialoutbounditem:update", "更新产品序列号出库明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductSerialOutboundItemAsync(long id, [FromBody] TaktProductSerialOutboundItemUpdateDto dto)
    {
        try
        {
            var result = await _productSerialOutboundItemService.UpdateProductSerialOutboundItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除产品序列号出库明细
    /// </summary>
    /// <param name="id">产品序列号出库明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:serial:productserialoutbounditem:delete", "删除产品序列号出库明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductSerialOutboundItemByIdAsync(long id)
    {
        try
        {
            await _productSerialOutboundItemService.DeleteProductSerialOutboundItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除产品序列号出库明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:serial:productserialoutbounditem:delete", "批量删除产品序列号出库明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProductSerialOutboundItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _productSerialOutboundItemService.DeleteProductSerialOutboundItemBatchAsync(ids);
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
    [TaktPermission("logistics:serial:productserialoutbounditem:import", "获取产品序列号出库明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetProductSerialOutboundItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _productSerialOutboundItemService.GetProductSerialOutboundItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入产品序列号出库明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:serial:productserialoutbounditem:import", "导入产品序列号出库明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportProductSerialOutboundItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _productSerialOutboundItemService.ImportProductSerialOutboundItemAsync(stream, sheetName);
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
    /// 导出产品序列号出库明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:serial:productserialoutbounditem:export", "导出产品序列号出库明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProductSerialOutboundItemAsync([FromQuery] TaktProductSerialOutboundItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _productSerialOutboundItemService.ExportProductSerialOutboundItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
