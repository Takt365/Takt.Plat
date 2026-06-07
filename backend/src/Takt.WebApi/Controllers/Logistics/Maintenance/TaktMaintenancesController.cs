// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Logistics.Maintenance
// 文件名称：TaktMaintenancesController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：设备维护记录控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Maintenance;
using Takt.Application.Services.Logistics.Maintenance;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Maintenance;

/// <summary>
/// 设备维护记录控制器
/// 提供设备维护记录的 REST API
/// </summary>
[ApiModule(TaktModule.Logistics, "后勤管理")]
[Route("api/[controller]", Name = "设备维护记录")]
public class TaktMaintenancesController : TaktControllerBase
{
    private readonly ITaktMaintenanceService _maintenanceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceService">设备维护记录服务</param>
    public TaktMaintenancesController(ITaktMaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }

    /// <summary>
    /// 获取设备维护记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:maintenance:maintenance:list", "设备维护记录列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMaintenanceListAsync([FromQuery] TaktMaintenanceQueryDto queryDto)
    {
        try
        {
            var result = await _maintenanceService.GetMaintenanceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取设备维护记录
    /// </summary>
    /// <param name="id">设备维护记录ID</param>
    /// <returns>设备维护记录DTO</returns>
    [TaktPermission("logistics:maintenance:maintenance:query", "设备维护记录详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaintenanceByIdAsync(long id)
    {
        try
        {
            var result = await _maintenanceService.GetMaintenanceByIdAsync(id);
            if (result == null)
            {
                return NotFound("设备维护记录不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取设备维护记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("logistics:maintenance:maintenance:query", "设备维护记录选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMaintenanceOptionsAsync()
    {
        try
        {
            var result = await _maintenanceService.GetMaintenanceOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建设备维护记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>设备维护记录DTO</returns>
    [TaktPermission("logistics:maintenance:maintenance:create", "创建设备维护记录")]
    [HttpPost]
    public async Task<IActionResult> CreateMaintenanceAsync([FromBody] TaktMaintenanceCreateDto dto)
    {
        try
        {
            var result = await _maintenanceService.CreateMaintenanceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设备维护记录
    /// </summary>
    /// <param name="id">设备维护记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>设备维护记录DTO</returns>
    [TaktPermission("logistics:maintenance:maintenance:update", "更新设备维护记录")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaintenanceAsync(long id, [FromBody] TaktMaintenanceUpdateDto dto)
    {
        try
        {
            var result = await _maintenanceService.UpdateMaintenanceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除设备维护记录
    /// </summary>
    /// <param name="id">设备维护记录ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:maintenance:delete", "删除设备维护记录")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaintenanceByIdAsync(long id)
    {
        try
        {
            await _maintenanceService.DeleteMaintenanceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除设备维护记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("logistics:maintenance:maintenance:delete", "批量删除设备维护记录")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMaintenanceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _maintenanceService.DeleteMaintenanceBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新设备维护记录状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>设备维护记录DTO</returns>
    [TaktPermission("logistics:maintenance:maintenance:update", "更新设备维护记录状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMaintenanceStatusAsync([FromBody] TaktMaintenanceStatusDto dto)
    {
        try
        {
            var result = await _maintenanceService.UpdateMaintenanceStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:maintenance:maintenance:import", "获取设备维护记录导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetMaintenanceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _maintenanceService.GetMaintenanceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入设备维护记录
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("logistics:maintenance:maintenance:import", "导入设备维护记录")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportMaintenanceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _maintenanceService.ImportMaintenanceAsync(stream, sheetName);
            return Success(new
            {
                SuccessCount = success,
                FailCount = fail,
                Errors = errors
            }, $"导入完成：成功{success}条，失败{fail}条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出设备维护记录
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("logistics:maintenance:maintenance:export", "导出设备维护记录")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMaintenanceAsync([FromQuery] TaktMaintenanceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _maintenanceService.ExportMaintenanceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
