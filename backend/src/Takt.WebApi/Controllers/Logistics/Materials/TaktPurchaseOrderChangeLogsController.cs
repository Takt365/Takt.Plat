// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktPurchaseOrderChangeLogsController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：采购订单变更记录控制器
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
/// 采购订单变更记录控制器
/// 提供采购订单变更记录的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "采购订单变更记录")]
public class TaktPurchaseOrderChangeLogsController : TaktControllerBase
{
    private readonly ITaktPurchaseOrderChangeLogService _purchaseOrderChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseOrderChangeLogService">采购订单变更记录服务</param>
    public TaktPurchaseOrderChangeLogsController(ITaktPurchaseOrderChangeLogService purchaseOrderChangeLogService)
    {
        _purchaseOrderChangeLogService = purchaseOrderChangeLogService;
    }

    /// <summary>
    /// 获取采购订单变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:purchaseorderchangelog:list", "采购订单变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseOrderChangeLogListAsync([FromQuery] TaktPurchaseOrderChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseOrderChangeLogService.GetPurchaseOrderChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购订单变更记录
    /// </summary>
    /// <param name="id">采购订单变更记录ID</param>
    /// <returns>采购订单变更记录DTO</returns>
    [TaktPermission("logistics:materials:purchaseorderchangelog:query", "采购订单变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchaseOrderChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseOrderChangeLogService.GetPurchaseOrderChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购订单变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购订单变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:purchaseorderchangelog:query", "采购订单变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseOrderChangeLogOptionsAsync()
    {
        try
        {
            var result = await _purchaseOrderChangeLogService.GetPurchaseOrderChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购订单变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购订单变更记录DTO</returns>
    [TaktPermission("logistics:materials:purchaseorderchangelog:create", "创建采购订单变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseOrderChangeLogAsync([FromBody] TaktPurchaseOrderChangeLogCreateDto dto)
    {
        try
        {
            var result = await _purchaseOrderChangeLogService.CreatePurchaseOrderChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购订单变更记录
    /// </summary>
    /// <param name="id">采购订单变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购订单变更记录DTO</returns>
    [TaktPermission("logistics:materials:purchaseorderchangelog:update", "更新采购订单变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseOrderChangeLogAsync(long id, [FromBody] TaktPurchaseOrderChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _purchaseOrderChangeLogService.UpdatePurchaseOrderChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购订单变更记录
    /// </summary>
    /// <param name="id">采购订单变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:purchaseorderchangelog:delete", "删除采购订单变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseOrderChangeLogByIdAsync(long id)
    {
        try
        {
            await _purchaseOrderChangeLogService.DeletePurchaseOrderChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购订单变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:purchaseorderchangelog:delete", "批量删除采购订单变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseOrderChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseOrderChangeLogService.DeletePurchaseOrderChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出采购订单变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:purchaseorderchangelog:export", "导出采购订单变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseOrderChangeLogAsync([FromQuery] TaktPurchaseOrderChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseOrderChangeLogService.ExportPurchaseOrderChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
