// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderChangeLogsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：进货检验单变更日志控制器
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
/// 进货检验单变更日志控制器
/// 提供进货检验单变更日志的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "进货检验单变更日志")]
public class TaktIqcOrderChangeLogsController : TaktControllerBase
{
    private readonly ITaktIqcOrderChangeLogService _iqcOrderChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="iqcOrderChangeLogService">进货检验单变更日志服务</param>
    public TaktIqcOrderChangeLogsController(ITaktIqcOrderChangeLogService iqcOrderChangeLogService)
    {
        _iqcOrderChangeLogService = iqcOrderChangeLogService;
    }

    /// <summary>
    /// 获取进货检验单变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:iqcorder:list", "进货检验单变更日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetIqcOrderChangeLogListAsync([FromQuery] TaktIqcOrderChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _iqcOrderChangeLogService.GetIqcOrderChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取进货检验单变更日志
    /// </summary>
    /// <param name="id">进货检验单变更日志ID</param>
    /// <returns>进货检验单变更日志DTO</returns>
    [TaktPermission("logistics:quality:operation:iqcorder:query", "进货检验单变更日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetIqcOrderChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _iqcOrderChangeLogService.GetIqcOrderChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("进货检验单变更日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取进货检验单变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:iqcorder:query", "进货检验单变更日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetIqcOrderChangeLogOptionsAsync()
    {
        try
        {
            var result = await _iqcOrderChangeLogService.GetIqcOrderChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建进货检验单变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>进货检验单变更日志DTO</returns>
    [TaktPermission("logistics:quality:operation:iqcorder:create", "创建进货检验单变更日志")]
    [HttpPost]
    public async Task<IActionResult> CreateIqcOrderChangeLogAsync([FromBody] TaktIqcOrderChangeLogCreateDto dto)
    {
        try
        {
            var result = await _iqcOrderChangeLogService.CreateIqcOrderChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新进货检验单变更日志
    /// </summary>
    /// <param name="id">进货检验单变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>进货检验单变更日志DTO</returns>
    [TaktPermission("logistics:quality:operation:iqcorder:update", "更新进货检验单变更日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIqcOrderChangeLogAsync(long id, [FromBody] TaktIqcOrderChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _iqcOrderChangeLogService.UpdateIqcOrderChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除进货检验单变更日志
    /// </summary>
    /// <param name="id">进货检验单变更日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:iqcorder:delete", "删除进货检验单变更日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIqcOrderChangeLogByIdAsync(long id)
    {
        try
        {
            await _iqcOrderChangeLogService.DeleteIqcOrderChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除进货检验单变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:iqcorder:delete", "批量删除进货检验单变更日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteIqcOrderChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _iqcOrderChangeLogService.DeleteIqcOrderChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出进货检验单变更日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:iqcorder:export", "导出进货检验单变更日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportIqcOrderChangeLogAsync([FromQuery] TaktIqcOrderChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _iqcOrderChangeLogService.ExportIqcOrderChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
