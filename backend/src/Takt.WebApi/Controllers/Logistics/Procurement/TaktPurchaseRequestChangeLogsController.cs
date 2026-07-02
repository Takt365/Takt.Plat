// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Procurement
// 文件名称：TaktPurchaseRequestChangeLogsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：采购申请变更记录控制器
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
/// 采购申请变更记录控制器
/// 提供采购申请变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "采购申请变更记录")]
public class TaktPurchaseRequestChangeLogsController : TaktControllerBase
{
    private readonly ITaktPurchaseRequestChangeLogService _purchaseRequestChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseRequestChangeLogService">采购申请变更记录服务</param>
    public TaktPurchaseRequestChangeLogsController(ITaktPurchaseRequestChangeLogService purchaseRequestChangeLogService)
    {
        _purchaseRequestChangeLogService = purchaseRequestChangeLogService;
    }

    /// <summary>
    /// 获取采购申请变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:procurement:purchase:request:list", "采购申请变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPurchaseRequestChangeLogListAsync([FromQuery] TaktPurchaseRequestChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _purchaseRequestChangeLogService.GetPurchaseRequestChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取采购申请变更记录
    /// </summary>
    /// <param name="id">采购申请变更记录ID</param>
    /// <returns>采购申请变更记录DTO</returns>
    [TaktPermission("logistics:procurement:purchase:request:query", "采购申请变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPurchaseRequestChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _purchaseRequestChangeLogService.GetPurchaseRequestChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("采购申请变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取采购申请变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:procurement:purchase:request:query", "采购申请变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPurchaseRequestChangeLogOptionsAsync()
    {
        try
        {
            var result = await _purchaseRequestChangeLogService.GetPurchaseRequestChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建采购申请变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>采购申请变更记录DTO</returns>
    [TaktPermission("logistics:procurement:purchase:request:create", "创建采购申请变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseRequestChangeLogAsync([FromBody] TaktPurchaseRequestChangeLogCreateDto dto)
    {
        try
        {
            var result = await _purchaseRequestChangeLogService.CreatePurchaseRequestChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新采购申请变更记录
    /// </summary>
    /// <param name="id">采购申请变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>采购申请变更记录DTO</returns>
    [TaktPermission("logistics:procurement:purchase:request:update", "更新采购申请变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePurchaseRequestChangeLogAsync(long id, [FromBody] TaktPurchaseRequestChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _purchaseRequestChangeLogService.UpdatePurchaseRequestChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除采购申请变更记录
    /// </summary>
    /// <param name="id">采购申请变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:request:delete", "删除采购申请变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchaseRequestChangeLogByIdAsync(long id)
    {
        try
        {
            await _purchaseRequestChangeLogService.DeletePurchaseRequestChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除采购申请变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:procurement:purchase:request:delete", "批量删除采购申请变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePurchaseRequestChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _purchaseRequestChangeLogService.DeletePurchaseRequestChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出采购申请变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:procurement:purchase:request:export", "导出采购申请变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPurchaseRequestChangeLogAsync([FromQuery] TaktPurchaseRequestChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _purchaseRequestChangeLogService.ExportPurchaseRequestChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
