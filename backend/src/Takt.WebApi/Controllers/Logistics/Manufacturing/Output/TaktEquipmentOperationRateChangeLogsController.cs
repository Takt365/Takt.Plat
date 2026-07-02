// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktEquipmentOperationRateChangeLogsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：机器稼动率变更记录控制器
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
/// 机器稼动率变更记录控制器
/// 提供机器稼动率变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "机器稼动率变更记录")]
public class TaktEquipmentOperationRateChangeLogsController : TaktControllerBase
{
    private readonly ITaktEquipmentOperationRateChangeLogService _equipmentOperationRateChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="equipmentOperationRateChangeLogService">机器稼动率变更记录服务</param>
    public TaktEquipmentOperationRateChangeLogsController(ITaktEquipmentOperationRateChangeLogService equipmentOperationRateChangeLogService)
    {
        _equipmentOperationRateChangeLogService = equipmentOperationRateChangeLogService;
    }

    /// <summary>
    /// 获取机器稼动率变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:output:equipment:operation:rate:list", "机器稼动率变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEquipmentOperationRateChangeLogListAsync([FromQuery] TaktEquipmentOperationRateChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _equipmentOperationRateChangeLogService.GetEquipmentOperationRateChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取机器稼动率变更记录
    /// </summary>
    /// <param name="id">机器稼动率变更记录ID</param>
    /// <returns>机器稼动率变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:equipment:operation:rate:query", "机器稼动率变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEquipmentOperationRateChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _equipmentOperationRateChangeLogService.GetEquipmentOperationRateChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("机器稼动率变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取机器稼动率变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:equipment:operation:rate:query", "机器稼动率变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEquipmentOperationRateChangeLogOptionsAsync()
    {
        try
        {
            var result = await _equipmentOperationRateChangeLogService.GetEquipmentOperationRateChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建机器稼动率变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>机器稼动率变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:equipment:operation:rate:create", "创建机器稼动率变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateEquipmentOperationRateChangeLogAsync([FromBody] TaktEquipmentOperationRateChangeLogCreateDto dto)
    {
        try
        {
            var result = await _equipmentOperationRateChangeLogService.CreateEquipmentOperationRateChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新机器稼动率变更记录
    /// </summary>
    /// <param name="id">机器稼动率变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>机器稼动率变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:equipment:operation:rate:update", "更新机器稼动率变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEquipmentOperationRateChangeLogAsync(long id, [FromBody] TaktEquipmentOperationRateChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _equipmentOperationRateChangeLogService.UpdateEquipmentOperationRateChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除机器稼动率变更记录
    /// </summary>
    /// <param name="id">机器稼动率变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:equipment:operation:rate:delete", "删除机器稼动率变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipmentOperationRateChangeLogByIdAsync(long id)
    {
        try
        {
            await _equipmentOperationRateChangeLogService.DeleteEquipmentOperationRateChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除机器稼动率变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:equipment:operation:rate:delete", "批量删除机器稼动率变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEquipmentOperationRateChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _equipmentOperationRateChangeLogService.DeleteEquipmentOperationRateChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出机器稼动率变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:output:equipment:operation:rate:export", "导出机器稼动率变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEquipmentOperationRateChangeLogAsync([FromQuery] TaktEquipmentOperationRateChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _equipmentOperationRateChangeLogService.ExportEquipmentOperationRateChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
