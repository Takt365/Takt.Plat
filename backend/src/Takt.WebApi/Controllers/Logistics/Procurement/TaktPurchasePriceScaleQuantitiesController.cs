// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchasePriceScaleQuantitiesController.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格数量等级控制器
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
/// 采购价格数量等级控制器
/// 提供采购价格数量等级的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购价格数量等级")]
public class TaktPurchasePriceScaleQuantitiesController : TaktControllerBase
{
    private readonly ITaktPurchasePriceScaleQuantityService _purchasePriceScaleQuantityService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceScaleQuantityService">采购价格数量等级服务</param>
    public TaktPurchasePriceScaleQuantitiesController(ITaktPurchasePriceScaleQuantityService purchasePriceScaleQuantityService)
    {
        _purchasePriceScaleQuantityService = purchasePriceScaleQuantityService;
    }

    /// <summary>
    /// 获取采购价格数量等级列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:list", "采购价格数量等级列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchasePriceScaleQuantityListAsync([FromQuery] TaktPurchasePriceScaleQuantityQueryDto queryDto)
    {
        try
        {
            var result = await _purchasePriceScaleQuantityService.GetPurchasePriceScaleQuantityListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购价格数量等级
    /// </summary>
    /// <param name="id">采购价格数量等级ID</param>
    /// <returns>采购价格数量等级DTO</returns>
    [TaktPermission("logistics:procurement:purchase:price:query", "采购价格数量等级详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchasePriceScaleQuantityByIdAsync(long id)
    {
        try
        {
            var result = await _purchasePriceScaleQuantityService.GetPurchasePriceScaleQuantityByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购价格数量等级不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购价格数量等级选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:price:query", "采购价格数量等级选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchasePriceScaleQuantityOptionsAsync()
    {
        try
        {
            var result = await _purchasePriceScaleQuantityService.GetPurchasePriceScaleQuantityOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购价格数量等级
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购价格数量等级DTO</returns>
    [TaktPermission("logistics:procurement:purchase:price:create", "创建采购价格数量等级")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchasePriceScaleQuantityAsync([FromBody] TaktPurchasePriceScaleQuantityCreateDto dto)
    {
        try
        {
            var result = await _purchasePriceScaleQuantityService.CreatePurchasePriceScaleQuantityAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购价格数量等级
    /// </summary>
    /// <param name="id">采购价格数量等级ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购价格数量等级DTO</returns>
    [TaktPermission("logistics:procurement:purchase:price:update", "更新采购价格数量等级")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchasePriceScaleQuantityAsync(long id, [FromBody] TaktPurchasePriceScaleQuantityUpdateDto dto)
    {
        try
        {
            var result = await _purchasePriceScaleQuantityService.UpdatePurchasePriceScaleQuantityAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购价格数量等级
    /// </summary>
    /// <param name="id">采购价格数量等级ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:delete", "删除采购价格数量等级")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchasePriceScaleQuantityByIdAsync(long id)
    {
        try
        {
            await _purchasePriceScaleQuantityService.DeletePurchasePriceScaleQuantityByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购价格数量等级
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:delete", "批量删除采购价格数量等级")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchasePriceScaleQuantityBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchasePriceScaleQuantityService.DeletePurchasePriceScaleQuantityBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购价格数量等级作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>采购价格数量等级DTO</returns>
    [TaktPermission("logistics:procurement:purchase:price:update", "更新采购价格数量等级作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdatePurchasePriceScaleQuantityObsoleteAsync([FromBody] TaktPurchasePriceScaleQuantityObsoleteDto dto)
    {
        try
        {
            var result = await _purchasePriceScaleQuantityService.UpdatePurchasePriceScaleQuantityObsoleteAsync(dto);
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
    [TaktPermission("logistics:procurement:purchase:price:import", "获取采购价格数量等级导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchasePriceScaleQuantityTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchasePriceScaleQuantityService.GetPurchasePriceScaleQuantityTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购价格数量等级
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:procurement:purchase:price:import", "导入采购价格数量等级")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchasePriceScaleQuantityAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchasePriceScaleQuantityService.ImportPurchasePriceScaleQuantityAsync(stream, sheetName);
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
    /// 导出采购价格数量等级
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:price:export", "导出采购价格数量等级")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchasePriceScaleQuantityAsync([FromQuery] TaktPurchasePriceScaleQuantityQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchasePriceScaleQuantityService.ExportPurchasePriceScaleQuantityAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
