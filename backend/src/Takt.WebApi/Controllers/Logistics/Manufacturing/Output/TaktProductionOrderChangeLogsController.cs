// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktProductionOrderChangeLogsController.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单变更记录控制器
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
/// 生产工单变更记录控制器
/// 提供生产工单变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "生产工单变更记录")]
public class TaktProductionOrderChangeLogsController : TaktControllerBase
{
    private readonly ITaktProductionOrderChangeLogService _productionOrderChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="productionOrderChangeLogService">生产工单变更记录服务</param>
    public TaktProductionOrderChangeLogsController(ITaktProductionOrderChangeLogService productionOrderChangeLogService)
    {
        _productionOrderChangeLogService = productionOrderChangeLogService;
    }

    /// <summary>
    /// 获取生产工单变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:output:production:order:list", "生产工单变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetProductionOrderChangeLogListAsync([FromQuery] TaktProductionOrderChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _productionOrderChangeLogService.GetProductionOrderChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取生产工单变更记录
    /// </summary>
    /// <param name="id">生产工单变更记录ID</param>
    /// <returns>生产工单变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:production:order:query", "生产工单变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductionOrderChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _productionOrderChangeLogService.GetProductionOrderChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("生产工单变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取生产工单变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:manufacturing:output:production:order:query", "生产工单变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetProductionOrderChangeLogOptionsAsync()
    {
        try
        {
            var result = await _productionOrderChangeLogService.GetProductionOrderChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建生产工单变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>生产工单变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:production:order:create", "创建生产工单变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateProductionOrderChangeLogAsync([FromBody] TaktProductionOrderChangeLogCreateDto dto)
    {
        try
        {
            var result = await _productionOrderChangeLogService.CreateProductionOrderChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新生产工单变更记录
    /// </summary>
    /// <param name="id">生产工单变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>生产工单变更记录DTO</returns>
    [TaktPermission("logistics:manufacturing:output:production:order:update", "更新生产工单变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductionOrderChangeLogAsync(long id, [FromBody] TaktProductionOrderChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _productionOrderChangeLogService.UpdateProductionOrderChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除生产工单变更记录
    /// </summary>
    /// <param name="id">生产工单变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:production:order:delete", "删除生产工单变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductionOrderChangeLogByIdAsync(long id)
    {
        try
        {
            await _productionOrderChangeLogService.DeleteProductionOrderChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除生产工单变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:manufacturing:output:production:order:delete", "批量删除生产工单变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteProductionOrderChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _productionOrderChangeLogService.DeleteProductionOrderChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出生产工单变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:manufacturing:output:production:order:export", "导出生产工单变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportProductionOrderChangeLogAsync([FromQuery] TaktProductionOrderChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _productionOrderChangeLogService.ExportProductionOrderChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
