// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingChangeLogsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线变更日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线变更日志控制器
/// 提供工艺路线变更日志的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工艺路线变更日志")]
public class TaktRoutingChangeLogsController : TaktControllerBase
{
    private readonly ITaktRoutingChangeLogService _routingChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routingChangeLogService">工艺路线变更日志服务</param>
    public TaktRoutingChangeLogsController(ITaktRoutingChangeLogService routingChangeLogService)
    {
        _routingChangeLogService = routingChangeLogService;
    }

    /// <summary>
    /// 获取工艺路线变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routingchangelog:list", "工艺路线变更日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetRoutingChangeLogListAsync([FromQuery] TaktRoutingChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _routingChangeLogService.GetRoutingChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工艺路线变更日志
    /// </summary>
    /// <param name="id">工艺路线变更日志ID</param>
    /// <returns>工艺路线变更日志DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routingchangelog:query", "工艺路线变更日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoutingChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _routingChangeLogService.GetRoutingChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("工艺路线变更日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工艺路线变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:routingchangelog:query", "工艺路线变更日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetRoutingChangeLogOptionsAsync()
    {
        try
        {
            var result = await _routingChangeLogService.GetRoutingChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工艺路线变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工艺路线变更日志DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routingchangelog:create", "创建工艺路线变更日志")]
    [HttpPost]
    public async Task<IActionResult> CreateRoutingChangeLogAsync([FromBody] TaktRoutingChangeLogCreateDto dto)
    {
        try
        {
            var result = await _routingChangeLogService.CreateRoutingChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工艺路线变更日志
    /// </summary>
    /// <param name="id">工艺路线变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工艺路线变更日志DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:routingchangelog:update", "更新工艺路线变更日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoutingChangeLogAsync(long id, [FromBody] TaktRoutingChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _routingChangeLogService.UpdateRoutingChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工艺路线变更日志
    /// </summary>
    /// <param name="id">工艺路线变更日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routingchangelog:delete", "删除工艺路线变更日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoutingChangeLogByIdAsync(long id)
    {
        try
        {
            await _routingChangeLogService.DeleteRoutingChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工艺路线变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:routingchangelog:delete", "批量删除工艺路线变更日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteRoutingChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _routingChangeLogService.DeleteRoutingChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出工艺路线变更日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:routingchangelog:export", "导出工艺路线变更日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportRoutingChangeLogAsync([FromQuery] TaktRoutingChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _routingChangeLogService.ExportRoutingChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
