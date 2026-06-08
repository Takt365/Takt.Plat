// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.TrainingDevelopment
// 文件名称：TaktTrainingPlansController.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：培训计划控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.TrainingDevelopment;
using Takt.Application.Services.HumanResource.TrainingDevelopment;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.TrainingDevelopment;

/// <summary>
/// 培训计划控制器
/// 提供培训计划的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "培训计划")]
public class TaktTrainingPlansController : TaktControllerBase
{
    private readonly ITaktTrainingPlanService _trainingPlanService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="trainingPlanService">培训计划服务</param>
    public TaktTrainingPlansController(ITaktTrainingPlanService trainingPlanService)
    {
        _trainingPlanService = trainingPlanService;
    }

    /// <summary>
    /// 获取培训计划列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:list", "培训计划列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTrainingPlanListAsync([FromQuery] TaktTrainingPlanQueryDto queryDto)
    {
        try
        {
            var result = await _trainingPlanService.GetTrainingPlanListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取培训计划
    /// </summary>
    /// <param name="id">培训计划ID</param>
    /// <returns>培训计划DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:query", "培训计划详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrainingPlanByIdAsync(long id)
    {
        try
        {
            var result = await _trainingPlanService.GetTrainingPlanByIdAsync(id);
            if (result == null)
            {
                return NotFound("培训计划不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取培训计划选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:query", "培训计划选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTrainingPlanOptionsAsync()
    {
        try
        {
            var result = await _trainingPlanService.GetTrainingPlanOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建培训计划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>培训计划DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:create", "创建培训计划")]
    [HttpPost]
    public async Task<IActionResult> CreateTrainingPlanAsync([FromBody] TaktTrainingPlanCreateDto dto)
    {
        try
        {
            var result = await _trainingPlanService.CreateTrainingPlanAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新培训计划
    /// </summary>
    /// <param name="id">培训计划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>培训计划DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:update", "更新培训计划")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrainingPlanAsync(long id, [FromBody] TaktTrainingPlanUpdateDto dto)
    {
        try
        {
            var result = await _trainingPlanService.UpdateTrainingPlanAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除培训计划
    /// </summary>
    /// <param name="id">培训计划ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:delete", "删除培训计划")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainingPlanByIdAsync(long id)
    {
        try
        {
            await _trainingPlanService.DeleteTrainingPlanByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除培训计划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:delete", "批量删除培训计划")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTrainingPlanBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _trainingPlanService.DeleteTrainingPlanBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新培训计划状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktCommonStatus 枚举）</param>
    /// <returns>培训计划DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:update", "更新培训计划状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTrainingPlanStatusAsync([FromBody] TaktTrainingPlanStatusDto dto)
    {
        try
        {
            var result = await _trainingPlanService.UpdateTrainingPlanStatusAsync(dto);
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
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:import", "获取培训计划导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTrainingPlanTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _trainingPlanService.GetTrainingPlanTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入培训计划
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:import", "导入培训计划")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTrainingPlanAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _trainingPlanService.ImportTrainingPlanAsync(stream, sheetName);
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
    /// 导出培训计划
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingplan:export", "导出培训计划")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTrainingPlanAsync([FromQuery] TaktTrainingPlanQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _trainingPlanService.ExportTrainingPlanAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
