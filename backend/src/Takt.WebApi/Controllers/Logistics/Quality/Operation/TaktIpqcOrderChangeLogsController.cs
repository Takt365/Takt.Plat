// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktIpqcOrderChangeLogsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：制程检验单变更日志控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Application.Services.Logistics.Quality.Operation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Quality.Operation;

/// <summary>
/// 制程检验单变更日志控制器
/// 提供制程检验单变更日志的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "制程检验单变更日志")]
public class TaktIpqcOrderChangeLogsController : TaktControllerBase
{
    private readonly ITaktIpqcOrderChangeLogService _ipqcOrderChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ipqcOrderChangeLogService">制程检验单变更日志服务</param>
    public TaktIpqcOrderChangeLogsController(ITaktIpqcOrderChangeLogService ipqcOrderChangeLogService)
    {
        _ipqcOrderChangeLogService = ipqcOrderChangeLogService;
    }

    /// <summary>
    /// 获取制程检验单变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:ipqcorder:list", "制程检验单变更日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetIpqcOrderChangeLogListAsync([FromQuery] TaktIpqcOrderChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _ipqcOrderChangeLogService.GetIpqcOrderChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取制程检验单变更日志
    /// </summary>
    /// <param name="id">制程检验单变更日志ID</param>
    /// <returns>制程检验单变更日志DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqcorder:query", "制程检验单变更日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetIpqcOrderChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _ipqcOrderChangeLogService.GetIpqcOrderChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("制程检验单变更日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取制程检验单变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:ipqcorder:query", "制程检验单变更日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetIpqcOrderChangeLogOptionsAsync()
    {
        try
        {
            var result = await _ipqcOrderChangeLogService.GetIpqcOrderChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建制程检验单变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>制程检验单变更日志DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqcorder:create", "创建制程检验单变更日志")]
    [HttpPost]
    public async Task<IActionResult> CreateIpqcOrderChangeLogAsync([FromBody] TaktIpqcOrderChangeLogCreateDto dto)
    {
        try
        {
            var result = await _ipqcOrderChangeLogService.CreateIpqcOrderChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新制程检验单变更日志
    /// </summary>
    /// <param name="id">制程检验单变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>制程检验单变更日志DTO</returns>
    [TaktPermission("logistics:quality:operation:ipqcorder:update", "更新制程检验单变更日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIpqcOrderChangeLogAsync(long id, [FromBody] TaktIpqcOrderChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _ipqcOrderChangeLogService.UpdateIpqcOrderChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除制程检验单变更日志
    /// </summary>
    /// <param name="id">制程检验单变更日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:ipqcorder:delete", "删除制程检验单变更日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIpqcOrderChangeLogByIdAsync(long id)
    {
        try
        {
            await _ipqcOrderChangeLogService.DeleteIpqcOrderChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除制程检验单变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:ipqcorder:delete", "批量删除制程检验单变更日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteIpqcOrderChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ipqcOrderChangeLogService.DeleteIpqcOrderChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出制程检验单变更日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:ipqcorder:export", "导出制程检验单变更日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportIpqcOrderChangeLogAsync([FromQuery] TaktIpqcOrderChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ipqcOrderChangeLogService.ExportIpqcOrderChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
