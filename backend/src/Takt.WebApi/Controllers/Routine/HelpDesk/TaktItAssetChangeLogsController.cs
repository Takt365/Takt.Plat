// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktItAssetChangeLogsController.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：IT设备保修变更日志控制器
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
/// IT设备保修变更日志控制器
/// 提供IT设备保修变更日志的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "IT设备保修变更日志")]
public class TaktItAssetChangeLogsController : TaktControllerBase
{
    private readonly ITaktItAssetChangeLogService _itAssetChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="itAssetChangeLogService">IT设备保修变更日志服务</param>
    public TaktItAssetChangeLogsController(ITaktItAssetChangeLogService itAssetChangeLogService)
    {
        _itAssetChangeLogService = itAssetChangeLogService;
    }

    /// <summary>
    /// 获取IT设备保修变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:helpdesk:itassetchangelog:list", "IT设备保修变更日志列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetItAssetChangeLogListAsync([FromQuery] TaktItAssetChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _itAssetChangeLogService.GetItAssetChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取IT设备保修变更日志
    /// </summary>
    /// <param name="id">IT设备保修变更日志ID</param>
    /// <returns>IT设备保修变更日志DTO</returns>
    [TaktPermission("routine:helpdesk:itassetchangelog:query", "IT设备保修变更日志详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItAssetChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _itAssetChangeLogService.GetItAssetChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("IT设备保修变更日志不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取IT设备保修变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:helpdesk:itassetchangelog:query", "IT设备保修变更日志选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetItAssetChangeLogOptionsAsync()
    {
        try
        {
            var result = await _itAssetChangeLogService.GetItAssetChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建IT设备保修变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>IT设备保修变更日志DTO</returns>
    [TaktPermission("routine:helpdesk:itassetchangelog:create", "创建IT设备保修变更日志")]
    [HttpPost]
    public async Task<IActionResult> CreateItAssetChangeLogAsync([FromBody] TaktItAssetChangeLogCreateDto dto)
    {
        try
        {
            var result = await _itAssetChangeLogService.CreateItAssetChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新IT设备保修变更日志
    /// </summary>
    /// <param name="id">IT设备保修变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>IT设备保修变更日志DTO</returns>
    [TaktPermission("routine:helpdesk:itassetchangelog:update", "更新IT设备保修变更日志")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItAssetChangeLogAsync(long id, [FromBody] TaktItAssetChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _itAssetChangeLogService.UpdateItAssetChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除IT设备保修变更日志
    /// </summary>
    /// <param name="id">IT设备保修变更日志ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:helpdesk:itassetchangelog:delete", "删除IT设备保修变更日志")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItAssetChangeLogByIdAsync(long id)
    {
        try
        {
            await _itAssetChangeLogService.DeleteItAssetChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除IT设备保修变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:helpdesk:itassetchangelog:delete", "批量删除IT设备保修变更日志")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteItAssetChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _itAssetChangeLogService.DeleteItAssetChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出IT设备保修变更日志
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:helpdesk:itassetchangelog:export", "导出IT设备保修变更日志")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportItAssetChangeLogAsync([FromQuery] TaktItAssetChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _itAssetChangeLogService.ExportItAssetChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
