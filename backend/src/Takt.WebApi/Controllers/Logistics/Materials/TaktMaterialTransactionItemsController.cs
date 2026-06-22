// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialTransactionItemsController.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：物料交易明细控制器
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
/// 物料交易明细控制器
/// 提供物料交易明细的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料交易明细")]
public class TaktMaterialTransactionItemsController : TaktControllerBase
{
    private readonly ITaktMaterialTransactionItemService _materialTransactionItemService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialTransactionItemService">物料交易明细服务</param>
    public TaktMaterialTransactionItemsController(ITaktMaterialTransactionItemService materialTransactionItemService)
    {
        _materialTransactionItemService = materialTransactionItemService;
    }

    /// <summary>
    /// 获取物料交易明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:materialtransaction:list", "物料交易明细列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialTransactionItemListAsync([FromQuery] TaktMaterialTransactionItemQueryDto queryDto)
    {
        try
        {
            var result = await _materialTransactionItemService.GetMaterialTransactionItemListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料交易明细
    /// </summary>
    /// <param name="id">物料交易明细ID</param>
    /// <returns>物料交易明细DTO</returns>
    [TaktPermission("logistics:materials:materialtransaction:query", "物料交易明细详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialTransactionItemByIdAsync(long id)
    {
        try
        {
            var result = await _materialTransactionItemService.GetMaterialTransactionItemByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料交易明细不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料交易明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:materialtransaction:query", "物料交易明细选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialTransactionItemOptionsAsync()
    {
        try
        {
            var result = await _materialTransactionItemService.GetMaterialTransactionItemOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料交易明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料交易明细DTO</returns>
    [TaktPermission("logistics:materials:materialtransaction:create", "创建物料交易明细")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialTransactionItemAsync([FromBody] TaktMaterialTransactionItemCreateDto dto)
    {
        try
        {
            var result = await _materialTransactionItemService.CreateMaterialTransactionItemAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料交易明细
    /// </summary>
    /// <param name="id">物料交易明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料交易明细DTO</returns>
    [TaktPermission("logistics:materials:materialtransaction:update", "更新物料交易明细")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialTransactionItemAsync(long id, [FromBody] TaktMaterialTransactionItemUpdateDto dto)
    {
        try
        {
            var result = await _materialTransactionItemService.UpdateMaterialTransactionItemAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料交易明细
    /// </summary>
    /// <param name="id">物料交易明细ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:materialtransaction:delete", "删除物料交易明细")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialTransactionItemByIdAsync(long id)
    {
        try
        {
            await _materialTransactionItemService.DeleteMaterialTransactionItemByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料交易明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:materialtransaction:delete", "批量删除物料交易明细")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialTransactionItemBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialTransactionItemService.DeleteMaterialTransactionItemBatchAsync(ids);
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
    [TaktPermission("logistics:materials:materialtransaction:import", "获取物料交易明细导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaterialTransactionItemTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _materialTransactionItemService.GetMaterialTransactionItemTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料交易明细
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:materialtransaction:import", "导入物料交易明细")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaterialTransactionItemAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _materialTransactionItemService.ImportMaterialTransactionItemAsync(stream, sheetName);
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
    /// 导出物料交易明细
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:materialtransaction:export", "导出物料交易明细")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialTransactionItemAsync([FromQuery] TaktMaterialTransactionItemQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialTransactionItemService.ExportMaterialTransactionItemAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
