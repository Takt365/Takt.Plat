// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Sales
// 文件名称：TaktSalesPriceScaleQuantitiesController.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格数量等级控制器
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
/// 销售价格数量等级控制器
/// 提供销售价格数量等级的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "销售价格数量等级")]
public class TaktSalesPriceScaleQuantitiesController : TaktControllerBase
{
    private readonly ITaktSalesPriceScaleQuantityService _salesPriceScaleQuantityService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesPriceScaleQuantityService">销售价格数量等级服务</param>
    public TaktSalesPriceScaleQuantitiesController(ITaktSalesPriceScaleQuantityService salesPriceScaleQuantityService)
    {
        _salesPriceScaleQuantityService = salesPriceScaleQuantityService;
    }

    /// <summary>
    /// 获取销售价格数量等级列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:sales:price:list", "销售价格数量等级列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalesPriceScaleQuantityListAsync([FromQuery] TaktSalesPriceScaleQuantityQueryDto queryDto)
    {
        try
        {
            var result = await _salesPriceScaleQuantityService.GetSalesPriceScaleQuantityListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取销售价格数量等级
    /// </summary>
    /// <param name="id">销售价格数量等级ID</param>
    /// <returns>销售价格数量等级DTO</returns>
    [TaktPermission("logistics:sales:price:query", "销售价格数量等级详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalesPriceScaleQuantityByIdAsync(long id)
    {
        try
        {
            var result = await _salesPriceScaleQuantityService.GetSalesPriceScaleQuantityByIdAsync(id);
            if (result == null)
            {
                return NotFound("销售价格数量等级不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取销售价格数量等级选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:sales:price:query", "销售价格数量等级选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalesPriceScaleQuantityOptionsAsync()
    {
        try
        {
            var result = await _salesPriceScaleQuantityService.GetSalesPriceScaleQuantityOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建销售价格数量等级
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>销售价格数量等级DTO</returns>
    [TaktPermission("logistics:sales:price:create", "创建销售价格数量等级")]
    [HttpPost]
    public async Task<IActionResult> CreateSalesPriceScaleQuantityAsync([FromBody] TaktSalesPriceScaleQuantityCreateDto dto)
    {
        try
        {
            var result = await _salesPriceScaleQuantityService.CreateSalesPriceScaleQuantityAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售价格数量等级
    /// </summary>
    /// <param name="id">销售价格数量等级ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>销售价格数量等级DTO</returns>
    [TaktPermission("logistics:sales:price:update", "更新销售价格数量等级")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalesPriceScaleQuantityAsync(long id, [FromBody] TaktSalesPriceScaleQuantityUpdateDto dto)
    {
        try
        {
            var result = await _salesPriceScaleQuantityService.UpdateSalesPriceScaleQuantityAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除销售价格数量等级
    /// </summary>
    /// <param name="id">销售价格数量等级ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:price:delete", "删除销售价格数量等级")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalesPriceScaleQuantityByIdAsync(long id)
    {
        try
        {
            await _salesPriceScaleQuantityService.DeleteSalesPriceScaleQuantityByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除销售价格数量等级
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:sales:price:delete", "批量删除销售价格数量等级")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalesPriceScaleQuantityBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salesPriceScaleQuantityService.DeleteSalesPriceScaleQuantityBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新销售价格数量等级作废状态
    /// </summary>
    /// <param name="dto">作废 DTO</param>
    /// <returns>销售价格数量等级DTO</returns>
    [TaktPermission("logistics:sales:price:update", "更新销售价格数量等级作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> UpdateSalesPriceScaleQuantityObsoleteAsync([FromBody] TaktSalesPriceScaleQuantityObsoleteDto dto)
    {
        try
        {
            var result = await _salesPriceScaleQuantityService.UpdateSalesPriceScaleQuantityObsoleteAsync(dto);
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
    [TaktPermission("logistics:sales:price:import", "获取销售价格数量等级导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalesPriceScaleQuantityTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salesPriceScaleQuantityService.GetSalesPriceScaleQuantityTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入销售价格数量等级
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:sales:price:import", "导入销售价格数量等级")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalesPriceScaleQuantityAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salesPriceScaleQuantityService.ImportSalesPriceScaleQuantityAsync(stream, sheetName);
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
    /// 导出销售价格数量等级
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:sales:price:export", "导出销售价格数量等级")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalesPriceScaleQuantityAsync([FromQuery] TaktSalesPriceScaleQuantityQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salesPriceScaleQuantityService.ExportSalesPriceScaleQuantityAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
