// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Materials
// 文件名称：TaktMaterialPlantChangeLogsController.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂物料变更记录控制器
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
/// 工厂物料变更记录控制器
/// 提供工厂物料变更记录的 REST API
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "工厂物料变更记录")]
public class TaktMaterialPlantChangeLogsController : TaktControllerBase
{
    private readonly ITaktMaterialPlantChangeLogService _materialPlantChangeLogService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialPlantChangeLogService">工厂物料变更记录服务</param>
    public TaktMaterialPlantChangeLogsController(ITaktMaterialPlantChangeLogService materialPlantChangeLogService)
    {
        _materialPlantChangeLogService = materialPlantChangeLogService;
    }

    /// <summary>
    /// 获取工厂物料变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:materials:material:plant:list", "工厂物料变更记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaterialPlantChangeLogListAsync([FromQuery] TaktMaterialPlantChangeLogQueryDto queryDto)
    {
        try
        {
            var result = await _materialPlantChangeLogService.GetMaterialPlantChangeLogListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工厂物料变更记录
    /// </summary>
    /// <param name="id">工厂物料变更记录ID</param>
    /// <returns>工厂物料变更记录DTO</returns>
    [TaktPermission("logistics:materials:material:plant:query", "工厂物料变更记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterialPlantChangeLogByIdAsync(long id)
    {
        try
        {
            var result = await _materialPlantChangeLogService.GetMaterialPlantChangeLogByIdAsync(id);
            if (result == null)
            {
                return NotFound("工厂物料变更记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工厂物料变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:materials:material:plant:query", "工厂物料变更记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaterialPlantChangeLogOptionsAsync()
    {
        try
        {
            var result = await _materialPlantChangeLogService.GetMaterialPlantChangeLogOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工厂物料变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工厂物料变更记录DTO</returns>
    [TaktPermission("logistics:materials:material:plant:create", "创建工厂物料变更记录")]
    [HttpPost]
    public async Task<IActionResult> CreateMaterialPlantChangeLogAsync([FromBody] TaktMaterialPlantChangeLogCreateDto dto)
    {
        try
        {
            var result = await _materialPlantChangeLogService.CreateMaterialPlantChangeLogAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工厂物料变更记录
    /// </summary>
    /// <param name="id">工厂物料变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工厂物料变更记录DTO</returns>
    [TaktPermission("logistics:materials:material:plant:update", "更新工厂物料变更记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterialPlantChangeLogAsync(long id, [FromBody] TaktMaterialPlantChangeLogUpdateDto dto)
    {
        try
        {
            var result = await _materialPlantChangeLogService.UpdateMaterialPlantChangeLogAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工厂物料变更记录
    /// </summary>
    /// <param name="id">工厂物料变更记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:plant:delete", "删除工厂物料变更记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterialPlantChangeLogByIdAsync(long id)
    {
        try
        {
            await _materialPlantChangeLogService.DeleteMaterialPlantChangeLogByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工厂物料变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:materials:material:plant:delete", "批量删除工厂物料变更记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaterialPlantChangeLogBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _materialPlantChangeLogService.DeleteMaterialPlantChangeLogBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出工厂物料变更记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:materials:material:plant:export", "导出工厂物料变更记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaterialPlantChangeLogAsync([FromQuery] TaktMaterialPlantChangeLogQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _materialPlantChangeLogService.ExportMaterialPlantChangeLogAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
