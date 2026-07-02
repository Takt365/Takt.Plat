// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktPersonnelOperationRateChangeLogsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：人员稼动率变更记录控制器
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
/// 人员稼动率变更记录控制器
/// 提供人员稼动率变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "人员稼动率变更记录")]
public class TaktPersonnelOperationRateChangeLogsController : TaktControllerBase
{
    private readonly ITaktPersonnelOperationRateChangeLogService _personnelOperationRateChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="personnelOperationRateChangeLogService">人员稼动率变更记录服务</param>
    public TaktPersonnelOperationRateChangeLogsController(ITaktPersonnelOperationRateChangeLogService personnelOperationRateChangeLogService)
    {
        _personnelOperationRateChangeLogService = personnelOperationRateChangeLogService;
    }

    /// <summary>
    /// 获取人员稼动率变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:output:personnel:operation:rate:list", "人员稼动率变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetPersonnelOperationRateChangeLogListAsync([FromQuery] TaktPersonnelOperationRateChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _personnelOperationRateChangeLogService.GetPersonnelOperationRateChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取人员稼动率变更记录
    /// </summary>
    /// <param name="id">人员稼动率变更记录ID</param>
    /// <returns>人员稼动率变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:personnel:operation:rate:query", "人员稼动率变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPersonnelOperationRateChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _personnelOperationRateChangeLogService.GetPersonnelOperationRateChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("人员稼动率变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取人员稼动率变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:personnel:operation:rate:query", "人员稼动率变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetPersonnelOperationRateChangeLogOptionsAsync()
    {
        try
        {
            var result = await _personnelOperationRateChangeLogService.GetPersonnelOperationRateChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建人员稼动率变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>人员稼动率变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:personnel:operation:rate:create", "创建人员稼动率变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreatePersonnelOperationRateChangeLogAsync([FromBody] TaktPersonnelOperationRateChangeLogCreateDto dto)
    {
        try
        {
            var result = await _personnelOperationRateChangeLogService.CreatePersonnelOperationRateChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新人员稼动率变更记录
    /// </summary>
    /// <param name="id">人员稼动率变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>人员稼动率变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:personnel:operation:rate:update", "更新人员稼动率变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePersonnelOperationRateChangeLogAsync(long id, [FromBody] TaktPersonnelOperationRateChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _personnelOperationRateChangeLogService.UpdatePersonnelOperationRateChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除人员稼动率变更记录
    /// </summary>
    /// <param name="id">人员稼动率变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:personnel:operation:rate:delete", "删除人员稼动率变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersonnelOperationRateChangeLogByIdAsync(long id)
    {
        try
        {
            await _personnelOperationRateChangeLogService.DeletePersonnelOperationRateChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除人员稼动率变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:personnel:operation:rate:delete", "批量删除人员稼动率变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeletePersonnelOperationRateChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _personnelOperationRateChangeLogService.DeletePersonnelOperationRateChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出人员稼动率变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:output:personnel:operation:rate:export", "导出人员稼动率变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportPersonnelOperationRateChangeLogAsync([FromQuery] TaktPersonnelOperationRateChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _personnelOperationRateChangeLogService.ExportPersonnelOperationRateChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
