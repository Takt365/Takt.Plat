// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Accounting.Controlling
// 文件名称：TaktCostCenterChangeLogsController.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：成本中心变更记录控制器
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
/// 成本中心变更记录控制器
/// 提供成本中心变更记录的 REST API
/// </summary>
[ApiModule(3, "管控会计")]
[Route("api/[controller]", Name = "成本中心变更记录")]
public class TaktCostCenterChangeLogsController : TaktControllerBase
{
    private readonly ITaktCostCenterChangeLogService _costCenterChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="costCenterChangeLogService">成本中心变更记录服务</param>
    public TaktCostCenterChangeLogsController(ITaktCostCenterChangeLogService costCenterChangeLogService)
    {
        _costCenterChangeLogService = costCenterChangeLogService;
    }

    /// <summary>
    /// 获取成本中心变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("accounting:controlling:cost:center:list", "成本中心变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCostCenterChangeLogListAsync([FromQuery] TaktCostCenterChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _costCenterChangeLogService.GetCostCenterChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取成本中心变更记录
    /// </summary>
    /// <param name="id">成本中心变更记录ID</param>
    /// <returns>成本中心变更记录DTO</returns>
    [TaktPermission("accounting:controlling:cost:center:query", "成本中心变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCostCenterChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _costCenterChangeLogService.GetCostCenterChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("成本中心变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取成本中心变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("accounting:controlling:cost:center:query", "成本中心变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCostCenterChangeLogOptionsAsync()
    {
        try
        {
            var result = await _costCenterChangeLogService.GetCostCenterChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建成本中心变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>成本中心变更记录DTO</returns>
    [TaktPermission("accounting:controlling:cost:center:create", "创建成本中心变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateCostCenterChangeLogAsync([FromBody] TaktCostCenterChangeLogCreateDto dto)
    {
        try
        {
            var result = await _costCenterChangeLogService.CreateCostCenterChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新成本中心变更记录
    /// </summary>
    /// <param name="id">成本中心变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>成本中心变更记录DTO</returns>
    [TaktPermission("accounting:controlling:cost:center:update", "更新成本中心变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCostCenterChangeLogAsync(long id, [FromBody] TaktCostCenterChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _costCenterChangeLogService.UpdateCostCenterChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除成本中心变更记录
    /// </summary>
    /// <param name="id">成本中心变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:cost:center:delete", "删除成本中心变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCostCenterChangeLogByIdAsync(long id)
    {
        try
        {
            await _costCenterChangeLogService.DeleteCostCenterChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除成本中心变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("accounting:controlling:cost:center:delete", "批量删除成本中心变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCostCenterChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _costCenterChangeLogService.DeleteCostCenterChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出成本中心变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("accounting:controlling:cost:center:export", "导出成本中心变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCostCenterChangeLogAsync([FromQuery] TaktCostCenterChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _costCenterChangeLogService.ExportCostCenterChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
