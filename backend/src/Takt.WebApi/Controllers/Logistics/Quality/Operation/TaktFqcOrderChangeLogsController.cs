// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderChangeLogsController.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：出货检验单变更日志控制器
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
/// 出货检验单变更日志控制器
/// 提供出货检验单变更日志的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "出货检验单变更日志")]
public class TaktFqcOrderChangeLogsController : TaktControllerBase
{
    private readonly ITaktFqcOrderChangeLogService _fqcOrderChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fqcOrderChangeLogService">出货检验单变更日志服务</param>
    public TaktFqcOrderChangeLogsController(ITaktFqcOrderChangeLogService fqcOrderChangeLogService)
    {
        _fqcOrderChangeLogService = fqcOrderChangeLogService;
    }

    /// <summary>
    /// 获取出货检验单变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:list", "出货检验单变更日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetFqcOrderChangeLogListAsync([FromQuery] TaktFqcOrderChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _fqcOrderChangeLogService.GetFqcOrderChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取出货检验单变更日志
    /// </summary>
    /// <param name="id">出货检验单变更日志ID</param>
    /// <returns>出货检验单变更日志DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:query", "出货检验单变更日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFqcOrderChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _fqcOrderChangeLogService.GetFqcOrderChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("出货检验单变更日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取出货检验单变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:query", "出货检验单变更日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetFqcOrderChangeLogOptionsAsync()
    {
        try
        {
            var result = await _fqcOrderChangeLogService.GetFqcOrderChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建出货检验单变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>出货检验单变更日志DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:create", "创建出货检验单变更日志")]
    [HttpPost]
    public async Task<IActionResult> CreateFqcOrderChangeLogAsync([FromBody] TaktFqcOrderChangeLogCreateDto dto)
    {
        try
        {
            var result = await _fqcOrderChangeLogService.CreateFqcOrderChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新出货检验单变更日志
    /// </summary>
    /// <param name="id">出货检验单变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>出货检验单变更日志DTO</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:update", "更新出货检验单变更日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFqcOrderChangeLogAsync(long id, [FromBody] TaktFqcOrderChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _fqcOrderChangeLogService.UpdateFqcOrderChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除出货检验单变更日志
    /// </summary>
    /// <param name="id">出货检验单变更日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:delete", "删除出货检验单变更日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFqcOrderChangeLogByIdAsync(long id)
    {
        try
        {
            await _fqcOrderChangeLogService.DeleteFqcOrderChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除出货检验单变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:delete", "批量删除出货检验单变更日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteFqcOrderChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _fqcOrderChangeLogService.DeleteFqcOrderChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出出货检验单变更日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:quality:operation:fqc:order:export", "导出出货检验单变更日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportFqcOrderChangeLogAsync([FromQuery] TaktFqcOrderChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _fqcOrderChangeLogService.ExportFqcOrderChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
