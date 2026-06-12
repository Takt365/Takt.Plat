// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktSettingsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：系统设置控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 系统设置控制器
/// 提供系统设置的 REST API
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "系统设置")]
public class TaktSettingsController : TaktControllerBase
{
    private readonly ITaktSettingService _settingService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="settingService">系统设置服务</param>
    public TaktSettingsController(ITaktSettingService settingService)
    {
        _settingService = settingService;
    }

    /// <summary>
    /// 获取系统设置列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:setting:list", "系统设置列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSettingListAsync([FromQuery] TaktSettingQueryDto queryDto)
    {
        try
        {
            var result = await _settingService.GetSettingListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取系统设置
    /// </summary>
    /// <param name="id">系统设置ID</param>
    /// <returns>系统设置DTO</returns>
    [TaktPermission("foundation:setting:query", "系统设置详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSettingByIdAsync(long id)
    {
        try
        {
            var result = await _settingService.GetSettingByIdAsync(id);
            if (result == null)
            {
                return NotFound("系统设置不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取系统设置选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:setting:query", "系统设置选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSettingOptionsAsync()
    {
        try
        {
            var result = await _settingService.GetSettingOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建系统设置
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>系统设置DTO</returns>
    [TaktPermission("foundation:setting:create", "创建系统设置")]
    [HttpPost]
    public async Task<IActionResult> CreateSettingAsync([FromBody] TaktSettingCreateDto dto)
    {
        try
        {
            var result = await _settingService.CreateSettingAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新系统设置
    /// </summary>
    /// <param name="id">系统设置ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>系统设置DTO</returns>
    [TaktPermission("foundation:setting:update", "更新系统设置")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSettingAsync(long id, [FromBody] TaktSettingUpdateDto dto)
    {
        try
        {
            var result = await _settingService.UpdateSettingAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除系统设置
    /// </summary>
    /// <param name="id">系统设置ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:setting:delete", "删除系统设置")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSettingByIdAsync(long id)
    {
        try
        {
            await _settingService.DeleteSettingByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除系统设置
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:setting:delete", "批量删除系统设置")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSettingBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _settingService.DeleteSettingBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新系统设置排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>系统设置DTO</returns>
    [TaktPermission("foundation:setting:update", "更新系统设置排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSettingSortAsync([FromBody] TaktSettingSortDto dto)
    {
        try
        {
            var result = await _settingService.UpdateSettingSortAsync(dto);
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
    [TaktPermission("foundation:setting:import", "获取系统设置导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSettingTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _settingService.GetSettingTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入系统设置
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("foundation:setting:import", "导入系统设置")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSettingAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _settingService.ImportSettingAsync(stream, sheetName);
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
    /// 导出系统设置
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:setting:export", "导出系统设置")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSettingAsync([FromQuery] TaktSettingQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _settingService.ExportSettingAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
