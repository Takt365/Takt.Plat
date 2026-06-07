// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktPurchaseOrderItemsController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：采购订单明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 采购订单明细控制器
/// 提供采购订单明细的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "采购订单明细")]
public class TaktPurchaseOrderItemsController : TaktControllerBase
{
    private readonly ITaktPurchaseOrderItemService _purchaseOrderItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseOrderItemService">采购订单明细服务</param>
    public TaktPurchaseOrderItemsController(ITaktPurchaseOrderItemService purchaseOrderItemService)
    {
        _purchaseOrderItemService = purchaseOrderItemService;
    }

    /// <summary>
    /// 获取采购订单明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:purchaseorderitem:list", "采购订单明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseOrderItemListAsync([FromQuery] TaktPurchaseOrderItemQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseOrderItemService.GetPurchaseOrderItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购订单明细
    /// </summary>
    /// <param name="id">采购订单明细ID</param>
    /// <returns>采购订单明细DTO</returns>
    [TaktPermission("logistics:materials:purchaseorderitem:query", "采购订单明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchaseOrderItemByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseOrderItemService.GetPurchaseOrderItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购订单明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购订单明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:purchaseorderitem:query", "采购订单明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseOrderItemOptionsAsync()
    {
        try
        {
            var result = await _purchaseOrderItemService.GetPurchaseOrderItemOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购订单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购订单明细DTO</returns>
    [TaktPermission("logistics:materials:purchaseorderitem:create", "创建采购订单明细")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseOrderItemAsync([FromBody] TaktPurchaseOrderItemCreateDto dto)
    {
        try
        {
            var result = await _purchaseOrderItemService.CreatePurchaseOrderItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购订单明细
    /// </summary>
    /// <param name="id">采购订单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购订单明细DTO</returns>
    [TaktPermission("logistics:materials:purchaseorderitem:update", "更新采购订单明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseOrderItemAsync(long id, [FromBody] TaktPurchaseOrderItemUpdateDto dto)
    {
        try
        {
            var result = await _purchaseOrderItemService.UpdatePurchaseOrderItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购订单明细
    /// </summary>
    /// <param name="id">采购订单明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:purchaseorderitem:delete", "删除采购订单明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseOrderItemByIdAsync(long id)
    {
        try
        {
            await _purchaseOrderItemService.DeletePurchaseOrderItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购订单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:purchaseorderitem:delete", "批量删除采购订单明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseOrderItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseOrderItemService.DeletePurchaseOrderItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购订单明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>采购订单明细DTO</returns>
    [TaktPermission("logistics:materials:purchaseorderitem:update", "更新采购订单明细状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePurchaseOrderItemStatusAsync([FromBody] TaktPurchaseOrderItemStatusDto dto)
    {
        try
        {
            var result = await _purchaseOrderItemService.UpdatePurchaseOrderItemStatusAsync(dto);
            return Success(result, "更新成功");
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
    [TaktPermission("logistics:materials:purchaseorderitem:import", "获取采购订单明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchaseOrderItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchaseOrderItemService.GetPurchaseOrderItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购订单明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:purchaseorderitem:import", "导入采购订单明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchaseOrderItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchaseOrderItemService.ImportPurchaseOrderItemAsync(stream, sheetName);
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
    /// 导出采购订单明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:purchaseorderitem:export", "导出采购订单明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseOrderItemAsync([FromQuery] TaktPurchaseOrderItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseOrderItemService.ExportPurchaseOrderItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
