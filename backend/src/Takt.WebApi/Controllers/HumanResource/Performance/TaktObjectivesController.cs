// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.Performance
// 文件名称：TaktObjectivesController.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效目标控制器
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
/// 绩效目标控制器
/// 提供绩效目标的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "绩效目标")]
public class TaktObjectivesController : TaktControllerBase
{
    private readonly ITaktObjectiveService _objectiveService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="objectiveService">绩效目标服务</param>
    public TaktObjectivesController(ITaktObjectiveService objectiveService)
    {
        _objectiveService = objectiveService;
    }

    /// <summary>
    /// 获取绩效目标列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:performance:objective:list", "绩效目标列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetObjectiveListAsync([FromQuery] TaktObjectiveQueryDto queryDto)
    {
        try
        {
            var result = await _objectiveService.GetObjectiveListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <returns>绩效目标DTO</returns>
    [TaktPermission("humanresource:performance:objective:query", "绩效目标详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetObjectiveByIdAsync(long id)
    {
        try
        {
            var result = await _objectiveService.GetObjectiveByIdAsync(id);
            if (result == null)
            {
                return NotFound("绩效目标不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取绩效目标选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:performance:objective:query", "绩效目标选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetObjectiveOptionsAsync()
    {
        try
        {
            var result = await _objectiveService.GetObjectiveOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建绩效目标
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>绩效目标DTO</returns>
    [TaktPermission("humanresource:performance:objective:create", "创建绩效目标")]
    [HttpPost]
    public async Task<IActionResult> CreateObjectiveAsync([FromBody] TaktObjectiveCreateDto dto)
    {
        try
        {
            var result = await _objectiveService.CreateObjectiveAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>绩效目标DTO</returns>
    [TaktPermission("humanresource:performance:objective:update", "更新绩效目标")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateObjectiveAsync(long id, [FromBody] TaktObjectiveUpdateDto dto)
    {
        try
        {
            var result = await _objectiveService.UpdateObjectiveAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:performance:objective:delete", "删除绩效目标")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteObjectiveByIdAsync(long id)
    {
        try
        {
            await _objectiveService.DeleteObjectiveByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除绩效目标
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:performance:objective:delete", "批量删除绩效目标")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteObjectiveBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _objectiveService.DeleteObjectiveBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效目标状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>绩效目标DTO</returns>
    [TaktPermission("humanresource:performance:objective:update", "更新绩效目标状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateObjectiveStatusAsync([FromBody] TaktObjectiveStatusDto dto)
    {
        try
        {
            var result = await _objectiveService.UpdateObjectiveStatusAsync(dto);
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
    [TaktPermission("humanresource:performance:objective:import", "获取绩效目标导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetObjectiveTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _objectiveService.GetObjectiveTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入绩效目标
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:performance:objective:import", "导入绩效目标")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportObjectiveAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _objectiveService.ImportObjectiveAsync(stream, sheetName);
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
    /// 导出绩效目标
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:performance:objective:export", "导出绩效目标")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportObjectiveAsync([FromQuery] TaktObjectiveQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _objectiveService.ExportObjectiveAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
