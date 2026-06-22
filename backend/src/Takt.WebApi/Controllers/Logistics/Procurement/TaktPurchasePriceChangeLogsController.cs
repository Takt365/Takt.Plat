// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchasePriceChangeLogsController.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格变更记录控制器
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
/// 采购价格变更记录控制器
/// 提供采购价格变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购价格变更记录")]
public class TaktPurchasePriceChangeLogsController : TaktControllerBase
{
    private readonly ITaktPurchasePriceChangeLogService _purchasePriceChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchasePriceChangeLogService">采购价格变更记录服务</param>
    public TaktPurchasePriceChangeLogsController(ITaktPurchasePriceChangeLogService purchasePriceChangeLogService)
    {
        _purchasePriceChangeLogService = purchasePriceChangeLogService;
    }

    /// <summary>
    /// 获取采购价格变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchaseprice:list", "采购价格变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchasePriceChangeLogListAsync([FromQuery] TaktPurchasePriceChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _purchasePriceChangeLogService.GetPurchasePriceChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购价格变更记录
    /// </summary>
    /// <param name="id">采购价格变更记录ID</param>
    /// <returns>采购价格变更记录DTO</returns>
    [TaktPermission("logistics:procurement:purchaseprice:query", "采购价格变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchasePriceChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _purchasePriceChangeLogService.GetPurchasePriceChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购价格变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购价格变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchaseprice:query", "采购价格变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchasePriceChangeLogOptionsAsync()
    {
        try
        {
            var result = await _purchasePriceChangeLogService.GetPurchasePriceChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购价格变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购价格变更记录DTO</returns>
    [TaktPermission("logistics:procurement:purchaseprice:create", "创建采购价格变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchasePriceChangeLogAsync([FromBody] TaktPurchasePriceChangeLogCreateDto dto)
    {
        try
        {
            var result = await _purchasePriceChangeLogService.CreatePurchasePriceChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购价格变更记录
    /// </summary>
    /// <param name="id">采购价格变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购价格变更记录DTO</returns>
    [TaktPermission("logistics:procurement:purchaseprice:update", "更新采购价格变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchasePriceChangeLogAsync(long id, [FromBody] TaktPurchasePriceChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _purchasePriceChangeLogService.UpdatePurchasePriceChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购价格变更记录
    /// </summary>
    /// <param name="id">采购价格变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchaseprice:delete", "删除采购价格变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchasePriceChangeLogByIdAsync(long id)
    {
        try
        {
            await _purchasePriceChangeLogService.DeletePurchasePriceChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购价格变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchaseprice:delete", "批量删除采购价格变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchasePriceChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchasePriceChangeLogService.DeletePurchasePriceChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出采购价格变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchaseprice:export", "导出采购价格变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchasePriceChangeLogAsync([FromQuery] TaktPurchasePriceChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchasePriceChangeLogService.ExportPurchasePriceChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
