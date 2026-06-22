// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktStandardOperationRateChangeLogsController.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Cursor AI)
// 功能描述：标准生产稼动率变更记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// 标准生产稼动率变更记录控制器
/// 提供标准生产稼动率变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "标准生产稼动率变更记录")]
public class TaktStandardOperationRateChangeLogsController : TaktControllerBase
{
    private readonly ITaktStandardOperationRateChangeLogService _standardOperationRateChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="standardOperationRateChangeLogService">标准生产稼动率变更记录服务</param>
    public TaktStandardOperationRateChangeLogsController(ITaktStandardOperationRateChangeLogService standardOperationRateChangeLogService)
    {
        _standardOperationRateChangeLogService = standardOperationRateChangeLogService;
    }

    /// <summary>
    /// 获取标准生产稼动率变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:output:standardoperationrate:list", "标准生产稼动率变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetStandardOperationRateChangeLogListAsync([FromQuery] TaktStandardOperationRateChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _standardOperationRateChangeLogService.GetStandardOperationRateChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取标准生产稼动率变更记录
    /// </summary>
    /// <param name="id">标准生产稼动率变更记录ID</param>
    /// <returns>标准生产稼动率变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:standardoperationrate:query", "标准生产稼动率变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStandardOperationRateChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _standardOperationRateChangeLogService.GetStandardOperationRateChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("标准生产稼动率变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取标准生产稼动率变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:standardoperationrate:query", "标准生产稼动率变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetStandardOperationRateChangeLogOptionsAsync()
    {
        try
        {
            var result = await _standardOperationRateChangeLogService.GetStandardOperationRateChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建标准生产稼动率变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>标准生产稼动率变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:standardoperationrate:create", "创建标准生产稼动率变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateStandardOperationRateChangeLogAsync([FromBody] TaktStandardOperationRateChangeLogCreateDto dto)
    {
        try
        {
            var result = await _standardOperationRateChangeLogService.CreateStandardOperationRateChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新标准生产稼动率变更记录
    /// </summary>
    /// <param name="id">标准生产稼动率变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>标准生产稼动率变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:standardoperationrate:update", "更新标准生产稼动率变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStandardOperationRateChangeLogAsync(long id, [FromBody] TaktStandardOperationRateChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _standardOperationRateChangeLogService.UpdateStandardOperationRateChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除标准生产稼动率变更记录
    /// </summary>
    /// <param name="id">标准生产稼动率变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:standardoperationrate:delete", "删除标准生产稼动率变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStandardOperationRateChangeLogByIdAsync(long id)
    {
        try
        {
            await _standardOperationRateChangeLogService.DeleteStandardOperationRateChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除标准生产稼动率变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:standardoperationrate:delete", "批量删除标准生产稼动率变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteStandardOperationRateChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _standardOperationRateChangeLogService.DeleteStandardOperationRateChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出标准生产稼动率变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:output:standardoperationrate:export", "导出标准生产稼动率变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportStandardOperationRateChangeLogAsync([FromQuery] TaktStandardOperationRateChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _standardOperationRateChangeLogService.ExportStandardOperationRateChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
