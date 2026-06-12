// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Statistics.Report
// 文件名称：TaktConfigurableGroupBiesController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：自定义报表分组控制器
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
/// 自定义报表分组控制器
/// 提供自定义报表分组的 REST API
/// </summary>
[ApiModule(9, "统计看板")]
[Route("api/[controller]", Name = "自定义报表分组")]
public class TaktConfigurableGroupBiesController : TaktControllerBase
{
    private readonly ITaktConfigurableGroupByService _configurableGroupByService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configurableGroupByService">自定义报表分组服务</param>
    public TaktConfigurableGroupBiesController(ITaktConfigurableGroupByService configurableGroupByService)
    {
        _configurableGroupByService = configurableGroupByService;
    }

    /// <summary>
    /// 获取自定义报表分组列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("statistics:report:configurablegroupby:list", "自定义报表分组列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetConfigurableGroupByListAsync([FromQuery] TaktConfigurableGroupByQueryDto queryDto)
    {
        try
        {
            var result = await _configurableGroupByService.GetConfigurableGroupByListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取自定义报表分组
    /// </summary>
    /// <param name="id">自定义报表分组ID</param>
    /// <returns>自定义报表分组DTO</returns>
    [TaktPermission("statistics:report:configurablegroupby:query", "自定义报表分组详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConfigurableGroupByByIdAsync(long id)
    {
        try
        {
            var result = await _configurableGroupByService.GetConfigurableGroupByByIdAsync(id);
            if (result == null)
            {
                return NotFound("自定义报表分组不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取自定义报表分组选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("statistics:report:configurablegroupby:query", "自定义报表分组选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetConfigurableGroupByOptionsAsync()
    {
        try
        {
            var result = await _configurableGroupByService.GetConfigurableGroupByOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建自定义报表分组
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>自定义报表分组DTO</returns>
    [TaktPermission("statistics:report:configurablegroupby:create", "创建自定义报表分组")]
    [HttpPost]
    public async Task<IActionResult> CreateConfigurableGroupByAsync([FromBody] TaktConfigurableGroupByCreateDto dto)
    {
        try
        {
            var result = await _configurableGroupByService.CreateConfigurableGroupByAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新自定义报表分组
    /// </summary>
    /// <param name="id">自定义报表分组ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>自定义报表分组DTO</returns>
    [TaktPermission("statistics:report:configurablegroupby:update", "更新自定义报表分组")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConfigurableGroupByAsync(long id, [FromBody] TaktConfigurableGroupByUpdateDto dto)
    {
        try
        {
            var result = await _configurableGroupByService.UpdateConfigurableGroupByAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除自定义报表分组
    /// </summary>
    /// <param name="id">自定义报表分组ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:report:configurablegroupby:delete", "删除自定义报表分组")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConfigurableGroupByByIdAsync(long id)
    {
        try
        {
            await _configurableGroupByService.DeleteConfigurableGroupByByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除自定义报表分组
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:report:configurablegroupby:delete", "批量删除自定义报表分组")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteConfigurableGroupByBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _configurableGroupByService.DeleteConfigurableGroupByBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新自定义报表分组排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>自定义报表分组DTO</returns>
    [TaktPermission("statistics:report:configurablegroupby:update", "更新自定义报表分组排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateConfigurableGroupBySortAsync([FromBody] TaktConfigurableGroupBySortDto dto)
    {
        try
        {
            var result = await _configurableGroupByService.UpdateConfigurableGroupBySortAsync(dto);
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
    [TaktPermission("statistics:report:configurablegroupby:import", "获取自定义报表分组导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetConfigurableGroupByTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _configurableGroupByService.GetConfigurableGroupByTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入自定义报表分组
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("statistics:report:configurablegroupby:import", "导入自定义报表分组")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportConfigurableGroupByAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _configurableGroupByService.ImportConfigurableGroupByAsync(stream, sheetName);
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
    /// 导出自定义报表分组
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("statistics:report:configurablegroupby:export", "导出自定义报表分组")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportConfigurableGroupByAsync([FromQuery] TaktConfigurableGroupByQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _configurableGroupByService.ExportConfigurableGroupByAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
