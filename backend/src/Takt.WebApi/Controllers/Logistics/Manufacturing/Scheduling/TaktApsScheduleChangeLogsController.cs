// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Scheduling
// 文件名称：TaktApsScheduleChangeLogsController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程变更日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Scheduling;
using Takt.Application.Services.Logistics.Manufacturing.Scheduling;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Scheduling;

/// <summary>
/// APS排程变更日志控制器
/// 提供APS排程变更日志的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "APS排程变更日志")]
public class TaktApsScheduleChangeLogsController : TaktControllerBase
{
    private readonly ITaktApsScheduleChangeLogService _apsScheduleChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsScheduleChangeLogService">APS排程变更日志服务</param>
    public TaktApsScheduleChangeLogsController(ITaktApsScheduleChangeLogService apsScheduleChangeLogService)
    {
        _apsScheduleChangeLogService = apsScheduleChangeLogService;
    }

    /// <summary>
    /// 获取APS排程变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:scheduling:apsschedulechangelog:list", "APS排程变更日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetApsScheduleChangeLogListAsync([FromQuery] TaktApsScheduleChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _apsScheduleChangeLogService.GetApsScheduleChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取APS排程变更日志
    /// </summary>
    /// <param name="id">APS排程变更日志ID</param>
    /// <returns>APS排程变更日志DTO</returns>
    [TaktPermission("logistics:manufacturing:scheduling:apsschedulechangelog:query", "APS排程变更日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetApsScheduleChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _apsScheduleChangeLogService.GetApsScheduleChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("APS排程变更日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取APS排程变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:scheduling:apsschedulechangelog:query", "APS排程变更日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetApsScheduleChangeLogOptionsAsync()
    {
        try
        {
            var result = await _apsScheduleChangeLogService.GetApsScheduleChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建APS排程变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>APS排程变更日志DTO</returns>
    [TaktPermission("logistics:manufacturing:scheduling:apsschedulechangelog:create", "创建APS排程变更日志")]
    [HttpPost]
    public async Task<IActionResult> CreateApsScheduleChangeLogAsync([FromBody] TaktApsScheduleChangeLogCreateDto dto)
    {
        try
        {
            var result = await _apsScheduleChangeLogService.CreateApsScheduleChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新APS排程变更日志
    /// </summary>
    /// <param name="id">APS排程变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>APS排程变更日志DTO</returns>
    [TaktPermission("logistics:manufacturing:scheduling:apsschedulechangelog:update", "更新APS排程变更日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApsScheduleChangeLogAsync(long id, [FromBody] TaktApsScheduleChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _apsScheduleChangeLogService.UpdateApsScheduleChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除APS排程变更日志
    /// </summary>
    /// <param name="id">APS排程变更日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:scheduling:apsschedulechangelog:delete", "删除APS排程变更日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApsScheduleChangeLogByIdAsync(long id)
    {
        try
        {
            await _apsScheduleChangeLogService.DeleteApsScheduleChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除APS排程变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:scheduling:apsschedulechangelog:delete", "批量删除APS排程变更日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteApsScheduleChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _apsScheduleChangeLogService.DeleteApsScheduleChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出APS排程变更日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:scheduling:apsschedulechangelog:export", "导出APS排程变更日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportApsScheduleChangeLogAsync([FromQuery] TaktApsScheduleChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _apsScheduleChangeLogService.ExportApsScheduleChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
