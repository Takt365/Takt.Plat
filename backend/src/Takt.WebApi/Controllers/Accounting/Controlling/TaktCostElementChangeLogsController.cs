// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Accounting.Controlling
// 文件名称：TaktCostElementChangeLogsController.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：成本要素变更记录控制器
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
/// 成本要素变更记录控制器
/// 提供成本要素变更记录的 REST API
/// </summary>
[ApiModule(TaktModule.Accounting, "管控会计")]
[Route("api/[controller]", Name = "成本要素变更记录")]
public class TaktCostElementChangeLogsController : TaktControllerBase
{
    private readonly ITaktCostElementChangeLogService _costElementChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="costElementChangeLogService">成本要素变更记录服务</param>
    public TaktCostElementChangeLogsController(ITaktCostElementChangeLogService costElementChangeLogService)
    {
        _costElementChangeLogService = costElementChangeLogService;
    }

    /// <summary>
    /// 获取成本要素变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:controlling:costelementchangelog:list", "成本要素变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCostElementChangeLogListAsync([FromQuery] TaktCostElementChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _costElementChangeLogService.GetCostElementChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取成本要素变更记录
    /// </summary>
    /// <param name="id">成本要素变更记录ID</param>
    /// <returns>成本要素变更记录DTO</returns>
    [TaktPermission("accounting:controlling:costelementchangelog:query", "成本要素变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCostElementChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _costElementChangeLogService.GetCostElementChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("成本要素变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取成本要素变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:controlling:costelementchangelog:query", "成本要素变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCostElementChangeLogOptionsAsync()
    {
        try
        {
            var result = await _costElementChangeLogService.GetCostElementChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建成本要素变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>成本要素变更记录DTO</returns>
    [TaktPermission("accounting:controlling:costelementchangelog:create", "创建成本要素变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateCostElementChangeLogAsync([FromBody] TaktCostElementChangeLogCreateDto dto)
    {
        try
        {
            var result = await _costElementChangeLogService.CreateCostElementChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本要素变更记录
    /// </summary>
    /// <param name="id">成本要素变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>成本要素变更记录DTO</returns>
    [TaktPermission("accounting:controlling:costelementchangelog:update", "更新成本要素变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCostElementChangeLogAsync(long id, [FromBody] TaktCostElementChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _costElementChangeLogService.UpdateCostElementChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除成本要素变更记录
    /// </summary>
    /// <param name="id">成本要素变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:costelementchangelog:delete", "删除成本要素变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCostElementChangeLogByIdAsync(long id)
    {
        try
        {
            await _costElementChangeLogService.DeleteCostElementChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除成本要素变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:costelementchangelog:delete", "批量删除成本要素变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCostElementChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _costElementChangeLogService.DeleteCostElementChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出成本要素变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:controlling:costelementchangelog:export", "导出成本要素变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCostElementChangeLogAsync([FromQuery] TaktCostElementChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _costElementChangeLogService.ExportCostElementChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
