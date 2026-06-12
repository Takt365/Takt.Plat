// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Statistics.Report
// 文件名称：TaktConfigurableFieldsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：自定义报表输出字段控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Statistics.Report;
using Takt.Application.Services.Statistics.Report;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Statistics.Report;

/// <summary>
/// 自定义报表输出字段控制器
/// 提供自定义报表输出字段的 REST API
/// </summary>
[ApiModule(9, "统计看板")]
[Route("api/[controller]", Name = "自定义报表输出字段")]
public class TaktConfigurableFieldsController : TaktControllerBase
{
    private readonly ITaktConfigurableFieldService _configurableFieldService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configurableFieldService">自定义报表输出字段服务</param>
    public TaktConfigurableFieldsController(ITaktConfigurableFieldService configurableFieldService)
    {
        _configurableFieldService = configurableFieldService;
    }

    /// <summary>
    /// 获取自定义报表输出字段列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:report:configurablefield:list", "自定义报表输出字段列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetConfigurableFieldListAsync([FromQuery] TaktConfigurableFieldQueryDto queryDto)
    {
        try
        {
            var result = await _configurableFieldService.GetConfigurableFieldListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取自定义报表输出字段
    /// </summary>
    /// <param name="id">自定义报表输出字段ID</param>
    /// <returns>自定义报表输出字段DTO</returns>
    [TaktPermission("statistics:report:configurablefield:query", "自定义报表输出字段详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConfigurableFieldByIdAsync(long id)
    {
        try
        {
            var result = await _configurableFieldService.GetConfigurableFieldByIdAsync(id);
            if (result == null)
            {
                return NotFound("自定义报表输出字段不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取自定义报表输出字段选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:report:configurablefield:query", "自定义报表输出字段选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetConfigurableFieldOptionsAsync()
    {
        try
        {
            var result = await _configurableFieldService.GetConfigurableFieldOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建自定义报表输出字段
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>自定义报表输出字段DTO</returns>
    [TaktPermission("statistics:report:configurablefield:create", "创建自定义报表输出字段")]
    [HttpPost]
    public async Task<IActionResult> CreateConfigurableFieldAsync([FromBody] TaktConfigurableFieldCreateDto dto)
    {
        try
        {
            var result = await _configurableFieldService.CreateConfigurableFieldAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新自定义报表输出字段
    /// </summary>
    /// <param name="id">自定义报表输出字段ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>自定义报表输出字段DTO</returns>
    [TaktPermission("statistics:report:configurablefield:update", "更新自定义报表输出字段")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConfigurableFieldAsync(long id, [FromBody] TaktConfigurableFieldUpdateDto dto)
    {
        try
        {
            var result = await _configurableFieldService.UpdateConfigurableFieldAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除自定义报表输出字段
    /// </summary>
    /// <param name="id">自定义报表输出字段ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:report:configurablefield:delete", "删除自定义报表输出字段")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConfigurableFieldByIdAsync(long id)
    {
        try
        {
            await _configurableFieldService.DeleteConfigurableFieldByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除自定义报表输出字段
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:report:configurablefield:delete", "批量删除自定义报表输出字段")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteConfigurableFieldBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _configurableFieldService.DeleteConfigurableFieldBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新自定义报表输出字段排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>自定义报表输出字段DTO</returns>
    [TaktPermission("statistics:report:configurablefield:update", "更新自定义报表输出字段排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateConfigurableFieldSortAsync([FromBody] TaktConfigurableFieldSortDto dto)
    {
        try
        {
            var result = await _configurableFieldService.UpdateConfigurableFieldSortAsync(dto);
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
    [TaktPermission("statistics:report:configurablefield:import", "获取自定义报表输出字段导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetConfigurableFieldTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _configurableFieldService.GetConfigurableFieldTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入自定义报表输出字段
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("statistics:report:configurablefield:import", "导入自定义报表输出字段")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportConfigurableFieldAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _configurableFieldService.ImportConfigurableFieldAsync(stream, sheetName);
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
    /// 导出自定义报表输出字段
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:report:configurablefield:export", "导出自定义报表输出字段")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportConfigurableFieldAsync([FromQuery] TaktConfigurableFieldQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _configurableFieldService.ExportConfigurableFieldAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
