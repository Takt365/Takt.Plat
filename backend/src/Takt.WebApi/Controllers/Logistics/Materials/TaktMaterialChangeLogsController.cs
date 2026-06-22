// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialChangeLogsController.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Cursor AI)
// 功能描述：全局物料变更记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Application.Services.Logistics.Materials;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Materials;

/// <summary>
/// 全局物料变更记录控制器
/// 提供全局物料变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "全局物料变更记录")]
public class TaktMaterialChangeLogsController : TaktControllerBase
{
    private readonly ITaktMaterialChangeLogService _materialChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialChangeLogService">全局物料变更记录服务</param>
    public TaktMaterialChangeLogsController(ITaktMaterialChangeLogService materialChangeLogService)
    {
        _materialChangeLogService = materialChangeLogService;
    }

    /// <summary>
    /// 获取全局物料变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:material:change:log:list", "全局物料变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialChangeLogListAsync([FromQuery] TaktMaterialChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _materialChangeLogService.GetMaterialChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取全局物料变更记录
    /// </summary>
    /// <param name="id">全局物料变更记录ID</param>
    /// <returns>全局物料变更记录DTO</returns>
    [TaktPermission("logistics:materials:material:change:log:query", "全局物料变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _materialChangeLogService.GetMaterialChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("全局物料变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取全局物料变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:change:log:query", "全局物料变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialChangeLogOptionsAsync()
    {
        try
        {
            var result = await _materialChangeLogService.GetMaterialChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建全局物料变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>全局物料变更记录DTO</returns>
    [TaktPermission("logistics:materials:material:change:log:create", "创建全局物料变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialChangeLogAsync([FromBody] TaktMaterialChangeLogCreateDto dto)
    {
        try
        {
            var result = await _materialChangeLogService.CreateMaterialChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新全局物料变更记录
    /// </summary>
    /// <param name="id">全局物料变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>全局物料变更记录DTO</returns>
    [TaktPermission("logistics:materials:material:change:log:update", "更新全局物料变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialChangeLogAsync(long id, [FromBody] TaktMaterialChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _materialChangeLogService.UpdateMaterialChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除全局物料变更记录
    /// </summary>
    /// <param name="id">全局物料变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:change:log:delete", "删除全局物料变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialChangeLogByIdAsync(long id)
    {
        try
        {
            await _materialChangeLogService.DeleteMaterialChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除全局物料变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:change:log:delete", "批量删除全局物料变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialChangeLogService.DeleteMaterialChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出全局物料变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:material:change:log:export", "导出全局物料变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialChangeLogAsync([FromQuery] TaktMaterialChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialChangeLogService.ExportMaterialChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
