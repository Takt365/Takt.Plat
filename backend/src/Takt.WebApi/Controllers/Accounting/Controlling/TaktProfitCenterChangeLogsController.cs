// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Accounting.Controlling
// 文件名称：TaktProfitCenterChangeLogsController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：利润中心变更记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Accounting.Controlling;
using Takt.Application.Services.Accounting.Controlling;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Accounting.Controlling;

/// <summary>
/// 利润中心变更记录控制器
/// 提供利润中心变更记录的 REST API
/// </summary>
[ApiModule(TaktModule.Accounting, "管控会计")]
[Route("api/[controller]", Name = "利润中心变更记录")]
public class TaktProfitCenterChangeLogsController : TaktControllerBase
{
    private readonly ITaktProfitCenterChangeLogService _profitCenterChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="profitCenterChangeLogService">利润中心变更记录服务</param>
    public TaktProfitCenterChangeLogsController(ITaktProfitCenterChangeLogService profitCenterChangeLogService)
    {
        _profitCenterChangeLogService = profitCenterChangeLogService;
    }

    /// <summary>
    /// 获取利润中心变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:controlling:profitcenterchangelog:list", "利润中心变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProfitCenterChangeLogListAsync([FromQuery] TaktProfitCenterChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _profitCenterChangeLogService.GetProfitCenterChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取利润中心变更记录
    /// </summary>
    /// <param name="id">利润中心变更记录ID</param>
    /// <returns>利润中心变更记录DTO</returns>
    [TaktPermission("accounting:controlling:profitcenterchangelog:query", "利润中心变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfitCenterChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _profitCenterChangeLogService.GetProfitCenterChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("利润中心变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取利润中心变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:controlling:profitcenterchangelog:query", "利润中心变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProfitCenterChangeLogOptionsAsync()
    {
        try
        {
            var result = await _profitCenterChangeLogService.GetProfitCenterChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建利润中心变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>利润中心变更记录DTO</returns>
    [TaktPermission("accounting:controlling:profitcenterchangelog:create", "创建利润中心变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateProfitCenterChangeLogAsync([FromBody] TaktProfitCenterChangeLogCreateDto dto)
    {
        try
        {
            var result = await _profitCenterChangeLogService.CreateProfitCenterChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新利润中心变更记录
    /// </summary>
    /// <param name="id">利润中心变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>利润中心变更记录DTO</returns>
    [TaktPermission("accounting:controlling:profitcenterchangelog:update", "更新利润中心变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfitCenterChangeLogAsync(long id, [FromBody] TaktProfitCenterChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _profitCenterChangeLogService.UpdateProfitCenterChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除利润中心变更记录
    /// </summary>
    /// <param name="id">利润中心变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:profitcenterchangelog:delete", "删除利润中心变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfitCenterChangeLogByIdAsync(long id)
    {
        try
        {
            await _profitCenterChangeLogService.DeleteProfitCenterChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除利润中心变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:profitcenterchangelog:delete", "批量删除利润中心变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProfitCenterChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _profitCenterChangeLogService.DeleteProfitCenterChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出利润中心变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:controlling:profitcenterchangelog:export", "导出利润中心变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProfitCenterChangeLogAsync([FromQuery] TaktProfitCenterChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _profitCenterChangeLogService.ExportProfitCenterChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
