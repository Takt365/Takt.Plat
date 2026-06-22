// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialTransactionsController.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：物料交易控制器
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
/// 物料交易控制器
/// 提供物料交易的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料交易")]
public class TaktMaterialTransactionsController : TaktControllerBase
{
    private readonly ITaktMaterialTransactionService _materialTransactionService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialTransactionService">物料交易服务</param>
    public TaktMaterialTransactionsController(ITaktMaterialTransactionService materialTransactionService)
    {
        _materialTransactionService = materialTransactionService;
    }

    /// <summary>
    /// 获取物料交易列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:material:transaction:list", "物料交易列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialTransactionListAsync([FromQuery] TaktMaterialTransactionQueryDto queryDto)
    {
        try
        {
            var result = await _materialTransactionService.GetMaterialTransactionListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取物料交易
    /// </summary>
    /// <param name="id">物料交易ID</param>
    /// <returns>物料交易DTO</returns>
    [TaktPermission("logistics:materials:material:transaction:query", "物料交易详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialTransactionByIdAsync(long id)
    {
        try
        {
            var result = await _materialTransactionService.GetMaterialTransactionByIdAsync(id);
            if (result == null)
            {
                return NotFound("物料交易不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取物料交易选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:transaction:query", "物料交易选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialTransactionOptionsAsync()
    {
        try
        {
            var result = await _materialTransactionService.GetMaterialTransactionOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建物料交易
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>物料交易DTO</returns>
    [TaktPermission("logistics:materials:material:transaction:create", "创建物料交易")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialTransactionAsync([FromBody] TaktMaterialTransactionCreateDto dto)
    {
        try
        {
            var result = await _materialTransactionService.CreateMaterialTransactionAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料交易
    /// </summary>
    /// <param name="id">物料交易ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>物料交易DTO</returns>
    [TaktPermission("logistics:materials:material:transaction:update", "更新物料交易")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialTransactionAsync(long id, [FromBody] TaktMaterialTransactionUpdateDto dto)
    {
        try
        {
            var result = await _materialTransactionService.UpdateMaterialTransactionAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除物料交易
    /// </summary>
    /// <param name="id">物料交易ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:transaction:delete", "删除物料交易")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialTransactionByIdAsync(long id)
    {
        try
        {
            await _materialTransactionService.DeleteMaterialTransactionByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除物料交易
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:transaction:delete", "批量删除物料交易")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialTransactionBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialTransactionService.DeleteMaterialTransactionBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新物料交易状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>物料交易DTO</returns>
    [TaktPermission("logistics:materials:material:transaction:update", "更新物料交易状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMaterialTransactionStatusAsync([FromBody] TaktMaterialTransactionStatusDto dto)
    {
        try
        {
            var result = await _materialTransactionService.UpdateMaterialTransactionStatusAsync(dto);
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
    [TaktPermission("logistics:materials:material:transaction:import", "获取物料交易导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaterialTransactionTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _materialTransactionService.GetMaterialTransactionTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入物料交易
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:materials:material:transaction:import", "导入物料交易")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaterialTransactionAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _materialTransactionService.ImportMaterialTransactionAsync(stream, sheetName);
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
    /// 导出物料交易
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:material:transaction:export", "导出物料交易")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialTransactionAsync([FromQuery] TaktMaterialTransactionQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialTransactionService.ExportMaterialTransactionAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
