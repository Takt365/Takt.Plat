// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktStandardOperationTimeChangeLogsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：标准工序时间变更记录控制器
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
/// 标准工序时间变更记录控制器
/// 提供标准工序时间变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "标准工序时间变更记录")]
public class TaktStandardOperationTimeChangeLogsController : TaktControllerBase
{
    private readonly ITaktStandardOperationTimeChangeLogService _standardOperationTimeChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="standardOperationTimeChangeLogService">标准工序时间变更记录服务</param>
    public TaktStandardOperationTimeChangeLogsController(ITaktStandardOperationTimeChangeLogService standardOperationTimeChangeLogService)
    {
        _standardOperationTimeChangeLogService = standardOperationTimeChangeLogService;
    }

    /// <summary>
    /// 获取标准工序时间变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:standard:operation:time:list", "标准工序时间变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetStandardOperationTimeChangeLogListAsync([FromQuery] TaktStandardOperationTimeChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _standardOperationTimeChangeLogService.GetStandardOperationTimeChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取标准工序时间变更记录
    /// </summary>
    /// <param name="id">标准工序时间变更记录ID</param>
    /// <returns>标准工序时间变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:standard:operation:time:query", "标准工序时间变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStandardOperationTimeChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _standardOperationTimeChangeLogService.GetStandardOperationTimeChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("标准工序时间变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取标准工序时间变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:bom:standard:operation:time:query", "标准工序时间变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetStandardOperationTimeChangeLogOptionsAsync()
    {
        try
        {
            var result = await _standardOperationTimeChangeLogService.GetStandardOperationTimeChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建标准工序时间变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>标准工序时间变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:standard:operation:time:create", "创建标准工序时间变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateStandardOperationTimeChangeLogAsync([FromBody] TaktStandardOperationTimeChangeLogCreateDto dto)
    {
        try
        {
            var result = await _standardOperationTimeChangeLogService.CreateStandardOperationTimeChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新标准工序时间变更记录
    /// </summary>
    /// <param name="id">标准工序时间变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>标准工序时间变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:bom:standard:operation:time:update", "更新标准工序时间变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStandardOperationTimeChangeLogAsync(long id, [FromBody] TaktStandardOperationTimeChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _standardOperationTimeChangeLogService.UpdateStandardOperationTimeChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除标准工序时间变更记录
    /// </summary>
    /// <param name="id">标准工序时间变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:standard:operation:time:delete", "删除标准工序时间变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStandardOperationTimeChangeLogByIdAsync(long id)
    {
        try
        {
            await _standardOperationTimeChangeLogService.DeleteStandardOperationTimeChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除标准工序时间变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:bom:standard:operation:time:delete", "批量删除标准工序时间变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteStandardOperationTimeChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _standardOperationTimeChangeLogService.DeleteStandardOperationTimeChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出标准工序时间变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:bom:standard:operation:time:export", "导出标准工序时间变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportStandardOperationTimeChangeLogAsync([FromQuery] TaktStandardOperationTimeChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _standardOperationTimeChangeLogService.ExportStandardOperationTimeChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
