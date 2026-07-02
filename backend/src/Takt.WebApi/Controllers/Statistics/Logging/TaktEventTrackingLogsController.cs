// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktEventTrackingLogsController.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：交互日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Statistics.Logging;
using Takt.Application.Services.Statistics.Logging;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Statistics.Logging;

/// <summary>
/// 交互日志控制器
/// 提供交互日志的 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "交互日志")]
public class TaktEventTrackingLogsController : TaktControllerBase
{
    private readonly ITaktEventTrackingLogService _eventTrackingLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="eventTrackingLogService">交互日志服务</param>
    public TaktEventTrackingLogsController(ITaktEventTrackingLogService eventTrackingLogService)
    {
        _eventTrackingLogService = eventTrackingLogService;
    }

    /// <summary>
    /// 获取交互日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:logging:event:tracking:log:list", "交互日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEventTrackingLogListAsync([FromQuery] TaktEventTrackingLogQueryDto queryDto)
    {
        try
        {
            var result = await _eventTrackingLogService.GetEventTrackingLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <returns>交互日志DTO</returns>
    [TaktPermission("statistics:logging:event:tracking:log:query", "交互日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventTrackingLogByIdAsync(long id)
    {
        try
        {
            var result = await _eventTrackingLogService.GetEventTrackingLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("交互日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取交互日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:logging:event:tracking:log:query", "交互日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEventTrackingLogOptionsAsync()
    {
        try
        {
            var result = await _eventTrackingLogService.GetEventTrackingLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建交互日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>交互日志DTO</returns>
    [TaktPermission("statistics:logging:event:tracking:log:create", "创建交互日志")]
    [HttpPost]
    public async Task<IActionResult> CreateEventTrackingLogAsync([FromBody] TaktEventTrackingLogCreateDto dto)
    {
        try
        {
            var result = await _eventTrackingLogService.CreateEventTrackingLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>交互日志DTO</returns>
    [TaktPermission("statistics:logging:event:tracking:log:update", "更新交互日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEventTrackingLogAsync(long id, [FromBody] TaktEventTrackingLogUpdateDto dto)
    {
        try
        {
            var result = await _eventTrackingLogService.UpdateEventTrackingLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除交互日志
    /// </summary>
    /// <param name="id">交互日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:event:tracking:log:delete", "删除交互日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEventTrackingLogByIdAsync(long id)
    {
        try
        {
            await _eventTrackingLogService.DeleteEventTrackingLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除交互日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:event:tracking:log:delete", "批量删除交互日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEventTrackingLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _eventTrackingLogService.DeleteEventTrackingLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量上报 Long Task 客户端性能事件（仅需登录，不要求功能权限码）
    /// </summary>
    /// <param name="dto">批量上报 DTO</param>
    /// <returns>写入条数</returns>
    [HttpPost("track-batch")]
    public async Task<IActionResult> TrackEventTrackingLogBatchAsync([FromBody] TaktEventTrackingLogBatchTrackDto dto)
    {
        try
        {
            var clientIp = TaktLocationHelper.ResolveClientIp(HttpContext);
            var count = await _eventTrackingLogService.TrackEventTrackingLogBatchAsync(dto, clientIp);
            return Success(new { count }, "上报成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出交互日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:logging:event:tracking:log:export", "导出交互日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEventTrackingLogAsync([FromQuery] TaktEventTrackingLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _eventTrackingLogService.ExportEventTrackingLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
