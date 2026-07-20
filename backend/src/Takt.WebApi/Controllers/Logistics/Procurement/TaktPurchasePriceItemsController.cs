// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchasePriceItemsController.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格明细控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Procurement;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Procurement;

/// <summary>
/// 采购价格明细控制器
/// 提供采购价格明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购价格明细")]
public class TaktPurchasePriceItemsController : TaktControllerBase
{
    private readonly ITaktPurchasePriceItemService _purchasePriceItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceItemService">采购价格明细服务</param>
    public TaktPurchasePriceItemsController(ITaktPurchasePriceItemService purchasePriceItemService)
    {
        _purchasePriceItemService = purchasePriceItemService;
    }

    /// <summary>
    /// 获取采购价格明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:list", "采购价格明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchasePriceItemListAsync([FromQuery] TaktPurchasePriceItemQueryDto queryDto)
    {
        try
        {
            var result = await _purchasePriceItemService.GetPurchasePriceItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购价格明细
    /// </summary>
    /// <param name="id">采购价格明细ID</param>
    /// <returns>采购价格明细DTO</returns>
    [TaktPermission("logistics:procurement:purchase:price:query", "采购价格明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchasePriceItemByIdAsync(long id)
    {
        try
        {
            var result = await _purchasePriceItemService.GetPurchasePriceItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购价格明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购价格明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:query", "采购价格明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchasePriceItemOptionsAsync()
    {
        try
        {
            var result = await _purchasePriceItemService.GetPurchasePriceItemOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购价格明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购价格明细DTO</returns>
    [TaktPermission("logistics:procurement:purchase:price:create", "创建采购价格明细")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchasePriceItemAsync([FromBody] TaktPurchasePriceItemCreateDto dto)
    {
        try
        {
            var result = await _purchasePriceItemService.CreatePurchasePriceItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购价格明细
    /// </summary>
    /// <param name="id">采购价格明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购价格明细DTO</returns>
    [TaktPermission("logistics:procurement:purchase:price:update", "更新采购价格明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchasePriceItemAsync(long id, [FromBody] TaktPurchasePriceItemUpdateDto dto)
    {
        try
        {
            var result = await _purchasePriceItemService.UpdatePurchasePriceItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购价格明细
    /// </summary>
    /// <param name="id">采购价格明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:delete", "删除采购价格明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchasePriceItemByIdAsync(long id)
    {
        try
        {
            await _purchasePriceItemService.DeletePurchasePriceItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购价格明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:delete", "批量删除采购价格明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchasePriceItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchasePriceItemService.DeletePurchasePriceItemBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购价格明细作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>采购价格明细DTO</returns>
    [TaktPermission("logistics:procurement:purchase:price:update", "更新采购价格明细作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdatePurchasePriceItemObsoleteAsync([FromBody] TaktPurchasePriceItemObsoleteDto dto)
    {
        try
        {
            var result = await _purchasePriceItemService.UpdatePurchasePriceItemObsoleteAsync(dto);
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
    [TaktPermission("logistics:procurement:purchase:price:import", "获取采购价格明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchasePriceItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchasePriceItemService.GetPurchasePriceItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购价格明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:import", "导入采购价格明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchasePriceItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchasePriceItemService.ImportPurchasePriceItemAsync(stream, sheetName);
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
    /// 导出采购价格明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:price:export", "导出采购价格明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchasePriceItemAsync([FromQuery] TaktPurchasePriceItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchasePriceItemService.ExportPurchasePriceItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
