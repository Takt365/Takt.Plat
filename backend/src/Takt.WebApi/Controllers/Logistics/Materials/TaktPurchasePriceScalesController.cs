// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktPurchasePriceScalesController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格阶梯控制器
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
/// 采购价格阶梯控制器
/// 提供采购价格阶梯的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "采购价格阶梯")]
public class TaktPurchasePriceScalesController : TaktControllerBase
{
    private readonly ITaktPurchasePriceScaleService _purchasePriceScaleService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceScaleService">采购价格阶梯服务</param>
    public TaktPurchasePriceScalesController(ITaktPurchasePriceScaleService purchasePriceScaleService)
    {
        _purchasePriceScaleService = purchasePriceScaleService;
    }

    /// <summary>
    /// 获取采购价格阶梯列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:purchasepricescale:list", "采购价格阶梯列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchasePriceScaleListAsync([FromQuery] TaktPurchasePriceScaleQueryDto queryDto)
    {
        try
        {
            var result = await _purchasePriceScaleService.GetPurchasePriceScaleListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购价格阶梯
    /// </summary>
    /// <param name="id">采购价格阶梯ID</param>
    /// <returns>采购价格阶梯DTO</returns>
    [TaktPermission("logistics:materials:purchasepricescale:query", "采购价格阶梯详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchasePriceScaleByIdAsync(long id)
    {
        try
        {
            var result = await _purchasePriceScaleService.GetPurchasePriceScaleByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购价格阶梯不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购价格阶梯选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:purchasepricescale:query", "采购价格阶梯选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchasePriceScaleOptionsAsync()
    {
        try
        {
            var result = await _purchasePriceScaleService.GetPurchasePriceScaleOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购价格阶梯
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购价格阶梯DTO</returns>
    [TaktPermission("logistics:materials:purchasepricescale:create", "创建采购价格阶梯")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchasePriceScaleAsync([FromBody] TaktPurchasePriceScaleCreateDto dto)
    {
        try
        {
            var result = await _purchasePriceScaleService.CreatePurchasePriceScaleAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购价格阶梯
    /// </summary>
    /// <param name="id">采购价格阶梯ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购价格阶梯DTO</returns>
    [TaktPermission("logistics:materials:purchasepricescale:update", "更新采购价格阶梯")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchasePriceScaleAsync(long id, [FromBody] TaktPurchasePriceScaleUpdateDto dto)
    {
        try
        {
            var result = await _purchasePriceScaleService.UpdatePurchasePriceScaleAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购价格阶梯
    /// </summary>
    /// <param name="id">采购价格阶梯ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:purchasepricescale:delete", "删除采购价格阶梯")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchasePriceScaleByIdAsync(long id)
    {
        try
        {
            await _purchasePriceScaleService.DeletePurchasePriceScaleByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购价格阶梯
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:purchasepricescale:delete", "批量删除采购价格阶梯")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchasePriceScaleBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchasePriceScaleService.DeletePurchasePriceScaleBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购价格阶梯排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>采购价格阶梯DTO</returns>
    [TaktPermission("logistics:materials:purchasepricescale:update", "更新采购价格阶梯排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdatePurchasePriceScaleSortAsync([FromBody] TaktPurchasePriceScaleSortDto dto)
    {
        try
        {
            var result = await _purchasePriceScaleService.UpdatePurchasePriceScaleSortAsync(dto);
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
    [TaktPermission("logistics:materials:purchasepricescale:import", "获取采购价格阶梯导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetPurchasePriceScaleTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _purchasePriceScaleService.GetPurchasePriceScaleTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入采购价格阶梯
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:purchasepricescale:import", "导入采购价格阶梯")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportPurchasePriceScaleAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _purchasePriceScaleService.ImportPurchasePriceScaleAsync(stream, sheetName);
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
    /// 导出采购价格阶梯
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:purchasepricescale:export", "导出采购价格阶梯")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchasePriceScaleAsync([FromQuery] TaktPurchasePriceScaleQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchasePriceScaleService.ExportPurchasePriceScaleAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
