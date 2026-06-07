// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.Performance
// 文件名称：TaktCycleSchedulesController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效周期日程控制器
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
/// 绩效周期日程控制器
/// 提供绩效周期日程的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "绩效周期日程")]
public class TaktCycleSchedulesController : TaktControllerBase
{
    private readonly ITaktCycleScheduleService _cycleScheduleService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cycleScheduleService">绩效周期日程服务</param>
    public TaktCycleSchedulesController(ITaktCycleScheduleService cycleScheduleService)
    {
        _cycleScheduleService = cycleScheduleService;
    }

    /// <summary>
    /// 获取绩效周期日程列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:performance:cycleschedule:list", "绩效周期日程列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCycleScheduleListAsync([FromQuery] TaktCycleScheduleQueryDto queryDto)
    {
        try
        {
            var result = await _cycleScheduleService.GetCycleScheduleListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取绩效周期日程
    /// </summary>
    /// <param name="id">绩效周期日程ID</param>
    /// <returns>绩效周期日程DTO</returns>
    [TaktPermission("humanresource:performance:cycleschedule:query", "绩效周期日程详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCycleScheduleByIdAsync(long id)
    {
        try
        {
            var result = await _cycleScheduleService.GetCycleScheduleByIdAsync(id);
            if (result == null)
            {
                return NotFound("绩效周期日程不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取绩效周期日程选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:performance:cycleschedule:query", "绩效周期日程选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCycleScheduleOptionsAsync()
    {
        try
        {
            var result = await _cycleScheduleService.GetCycleScheduleOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建绩效周期日程
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>绩效周期日程DTO</returns>
    [TaktPermission("humanresource:performance:cycleschedule:create", "创建绩效周期日程")]
    [HttpPost]
    public async Task<IActionResult> CreateCycleScheduleAsync([FromBody] TaktCycleScheduleCreateDto dto)
    {
        try
        {
            var result = await _cycleScheduleService.CreateCycleScheduleAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效周期日程
    /// </summary>
    /// <param name="id">绩效周期日程ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>绩效周期日程DTO</returns>
    [TaktPermission("humanresource:performance:cycleschedule:update", "更新绩效周期日程")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCycleScheduleAsync(long id, [FromBody] TaktCycleScheduleUpdateDto dto)
    {
        try
        {
            var result = await _cycleScheduleService.UpdateCycleScheduleAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除绩效周期日程
    /// </summary>
    /// <param name="id">绩效周期日程ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:performance:cycleschedule:delete", "删除绩效周期日程")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCycleScheduleByIdAsync(long id)
    {
        try
        {
            await _cycleScheduleService.DeleteCycleScheduleByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除绩效周期日程
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:performance:cycleschedule:delete", "批量删除绩效周期日程")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCycleScheduleBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _cycleScheduleService.DeleteCycleScheduleBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效周期日程状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>绩效周期日程DTO</returns>
    [TaktPermission("humanresource:performance:cycleschedule:update", "更新绩效周期日程状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCycleScheduleStatusAsync([FromBody] TaktCycleScheduleStatusDto dto)
    {
        try
        {
            var result = await _cycleScheduleService.UpdateCycleScheduleStatusAsync(dto);
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
    [TaktPermission("humanresource:performance:cycleschedule:import", "获取绩效周期日程导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCycleScheduleTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _cycleScheduleService.GetCycleScheduleTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入绩效周期日程
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:performance:cycleschedule:import", "导入绩效周期日程")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCycleScheduleAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _cycleScheduleService.ImportCycleScheduleAsync(stream, sheetName);
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
    /// 导出绩效周期日程
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:performance:cycleschedule:export", "导出绩效周期日程")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCycleScheduleAsync([FromQuery] TaktCycleScheduleQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _cycleScheduleService.ExportCycleScheduleAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
