// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.Performance
// 文件名称：TaktSchemeMetricsController.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效方案指标控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Performance;
using Takt.Application.Services.HumanResource.Performance;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Performance;

/// <summary>
/// 绩效方案指标控制器
/// 提供绩效方案指标的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "绩效方案指标")]
public class TaktSchemeMetricsController : TaktControllerBase
{
    private readonly ITaktSchemeMetricService _schemeMetricService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="schemeMetricService">绩效方案指标服务</param>
    public TaktSchemeMetricsController(ITaktSchemeMetricService schemeMetricService)
    {
        _schemeMetricService = schemeMetricService;
    }

    /// <summary>
    /// 获取绩效方案指标列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:performance:schememetric:list", "绩效方案指标列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSchemeMetricListAsync([FromQuery] TaktSchemeMetricQueryDto queryDto)
    {
        try
        {
            var result = await _schemeMetricService.GetSchemeMetricListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取绩效方案指标
    /// </summary>
    /// <param name="id">绩效方案指标ID</param>
    /// <returns>绩效方案指标DTO</returns>
    [TaktPermission("humanresource:performance:schememetric:query", "绩效方案指标详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSchemeMetricByIdAsync(long id)
    {
        try
        {
            var result = await _schemeMetricService.GetSchemeMetricByIdAsync(id);
            if (result == null)
            {
                return NotFound("绩效方案指标不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取绩效方案指标选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:performance:schememetric:query", "绩效方案指标选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSchemeMetricOptionsAsync()
    {
        try
        {
            var result = await _schemeMetricService.GetSchemeMetricOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建绩效方案指标
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>绩效方案指标DTO</returns>
    [TaktPermission("humanresource:performance:schememetric:create", "创建绩效方案指标")]
    [HttpPost]
    public async Task<IActionResult> CreateSchemeMetricAsync([FromBody] TaktSchemeMetricCreateDto dto)
    {
        try
        {
            var result = await _schemeMetricService.CreateSchemeMetricAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效方案指标
    /// </summary>
    /// <param name="id">绩效方案指标ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>绩效方案指标DTO</returns>
    [TaktPermission("humanresource:performance:schememetric:update", "更新绩效方案指标")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchemeMetricAsync(long id, [FromBody] TaktSchemeMetricUpdateDto dto)
    {
        try
        {
            var result = await _schemeMetricService.UpdateSchemeMetricAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除绩效方案指标
    /// </summary>
    /// <param name="id">绩效方案指标ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:performance:schememetric:delete", "删除绩效方案指标")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchemeMetricByIdAsync(long id)
    {
        try
        {
            await _schemeMetricService.DeleteSchemeMetricByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除绩效方案指标
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:performance:schememetric:delete", "批量删除绩效方案指标")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSchemeMetricBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _schemeMetricService.DeleteSchemeMetricBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效方案指标状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>绩效方案指标DTO</returns>
    [TaktPermission("humanresource:performance:schememetric:update", "更新绩效方案指标状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSchemeMetricStatusAsync([FromBody] TaktSchemeMetricStatusDto dto)
    {
        try
        {
            var result = await _schemeMetricService.UpdateSchemeMetricStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效方案指标排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>绩效方案指标DTO</returns>
    [TaktPermission("humanresource:performance:schememetric:update", "更新绩效方案指标排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateSchemeMetricSortAsync([FromBody] TaktSchemeMetricSortDto dto)
    {
        try
        {
            var result = await _schemeMetricService.UpdateSchemeMetricSortAsync(dto);
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
    [TaktPermission("humanresource:performance:schememetric:import", "获取绩效方案指标导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSchemeMetricTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _schemeMetricService.GetSchemeMetricTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入绩效方案指标
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:performance:schememetric:import", "导入绩效方案指标")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSchemeMetricAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _schemeMetricService.ImportSchemeMetricAsync(stream, sheetName);
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
    /// 导出绩效方案指标
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:performance:schememetric:export", "导出绩效方案指标")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSchemeMetricAsync([FromQuery] TaktSchemeMetricQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _schemeMetricService.ExportSchemeMetricAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
