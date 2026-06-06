// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktPurchasePricesController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格控制器
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
/// 采购价格控制器
/// 提供采购价格的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "采购价格")]
public class TaktPurchasePricesController : TaktControllerBase
{
    private readonly ITaktPurchasePriceService _purchasePriceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceService">采购价格服务</param>
    public TaktPurchasePricesController(ITaktPurchasePriceService purchasePriceService)
    {
        _purchasePriceService = purchasePriceService;
    }

    /// <summary>
    /// 获取采购价格列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:purchaseprice:list", "采购价格列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchasePriceListAsync([FromQuery] TaktPurchasePriceQueryDto queryDto)
    {
        try
        {
            var result = await _purchasePriceService.GetPurchasePriceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购价格
    /// </summary>
    /// <param name="id">采购价格ID</param>
    /// <returns>采购价格DTO</returns>
    [TaktPermission("logistics:materials:purchaseprice:query", "采购价格详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchasePriceByIdAsync(long id)
    {
        try
        {
            var result = await _purchasePriceService.GetPurchasePriceByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购价格不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购价格选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:purchaseprice:query", "采购价格选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchasePriceOptionsAsync()
    {
        try
        {
            var result = await _purchasePriceService.GetPurchasePriceOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购价格
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购价格DTO</returns>
    [TaktPermission("logistics:materials:purchaseprice:create", "创建采购价格")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchasePriceAsync([FromBody] TaktPurchasePriceCreateDto dto)
    {
        try
        {
            var result = await _purchasePriceService.CreatePurchasePriceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购价格
    /// </summary>
    /// <param name="id">采购价格ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购价格DTO</returns>
    [TaktPermission("logistics:materials:purchaseprice:update", "更新采购价格")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchasePriceAsync(long id, [FromBody] TaktPurchasePriceUpdateDto dto)
    {
        try
        {
            var result = await _purchasePriceService.UpdatePurchasePriceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购价格
    /// </summary>
    /// <param name="id">采购价格ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:purchaseprice:delete", "删除采购价格")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchasePriceByIdAsync(long id)
    {
        try
        {
            await _purchasePriceService.DeletePurchasePriceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购价格
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:purchaseprice:delete", "批量删除采购价格")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchasePriceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchasePriceService.DeletePurchasePriceBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购价格状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>采购价格DTO</returns>
    [TaktPermission("logistics:materials:purchaseprice:update", "更新采购价格状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdatePurchasePriceStatusAsync([FromBody] TaktPurchasePriceStatusDto dto)
    {
        try
        {
            var result = await _purchasePriceService.UpdatePurchasePriceStatusAsync(dto);
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
    [TaktPermission("logistics:materials:purchaseprice:import", "获取采购价格导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchasePriceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchasePriceService.GetPurchasePriceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购价格
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:purchaseprice:import", "导入采购价格")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchasePriceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchasePriceService.ImportPurchasePriceAsync(stream, sheetName);
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
    /// 导出采购价格
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:purchaseprice:export", "导出采购价格")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchasePriceAsync([FromQuery] TaktPurchasePriceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchasePriceService.ExportPurchasePriceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
