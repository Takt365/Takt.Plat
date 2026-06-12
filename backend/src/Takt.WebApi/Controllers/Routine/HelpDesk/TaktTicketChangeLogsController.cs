// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktTicketChangeLogsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：工单变更日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Application.Services.Routine.HelpDesk;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.HelpDesk;

/// <summary>
/// 工单变更日志控制器
/// 提供工单变更日志的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "工单变更日志")]
public class TaktTicketChangeLogsController : TaktControllerBase
{
    private readonly ITaktTicketChangeLogService _ticketChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketChangeLogService">工单变更日志服务</param>
    public TaktTicketChangeLogsController(ITaktTicketChangeLogService ticketChangeLogService)
    {
        _ticketChangeLogService = ticketChangeLogService;
    }

    /// <summary>
    /// 获取工单变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:helpdesk:ticketchangelog:list", "工单变更日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTicketChangeLogListAsync([FromQuery] TaktTicketChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _ticketChangeLogService.GetTicketChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工单变更日志
    /// </summary>
    /// <param name="id">工单变更日志ID</param>
    /// <returns>工单变更日志DTO</returns>
    [TaktPermission("routine:helpdesk:ticketchangelog:query", "工单变更日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _ticketChangeLogService.GetTicketChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("工单变更日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工单变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:helpdesk:ticketchangelog:query", "工单变更日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTicketChangeLogOptionsAsync()
    {
        try
        {
            var result = await _ticketChangeLogService.GetTicketChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工单变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工单变更日志DTO</returns>
    [TaktPermission("routine:helpdesk:ticketchangelog:create", "创建工单变更日志")]
    [HttpPost]
    public async Task<IActionResult> CreateTicketChangeLogAsync([FromBody] TaktTicketChangeLogCreateDto dto)
    {
        try
        {
            var result = await _ticketChangeLogService.CreateTicketChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工单变更日志
    /// </summary>
    /// <param name="id">工单变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工单变更日志DTO</returns>
    [TaktPermission("routine:helpdesk:ticketchangelog:update", "更新工单变更日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicketChangeLogAsync(long id, [FromBody] TaktTicketChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _ticketChangeLogService.UpdateTicketChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工单变更日志
    /// </summary>
    /// <param name="id">工单变更日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:helpdesk:ticketchangelog:delete", "删除工单变更日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicketChangeLogByIdAsync(long id)
    {
        try
        {
            await _ticketChangeLogService.DeleteTicketChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工单变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:helpdesk:ticketchangelog:delete", "批量删除工单变更日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTicketChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ticketChangeLogService.DeleteTicketChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出工单变更日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:helpdesk:ticketchangelog:export", "导出工单变更日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTicketChangeLogAsync([FromQuery] TaktTicketChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ticketChangeLogService.ExportTicketChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
