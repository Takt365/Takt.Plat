// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.TrainingDevelopment
// 文件名称：TaktTrainingResultsController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：培训结果控制器
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
/// 培训结果控制器
/// 提供培训结果的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "培训结果")]
public class TaktTrainingResultsController : TaktControllerBase
{
    private readonly ITaktTrainingResultService _trainingResultService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="trainingResultService">培训结果服务</param>
    public TaktTrainingResultsController(ITaktTrainingResultService trainingResultService)
    {
        _trainingResultService = trainingResultService;
    }

    /// <summary>
    /// 获取培训结果列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:list", "培训结果列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTrainingResultListAsync([FromQuery] TaktTrainingResultQueryDto queryDto)
    {
        try
        {
            var result = await _trainingResultService.GetTrainingResultListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取培训结果
    /// </summary>
    /// <param name="id">培训结果ID</param>
    /// <returns>培训结果DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:query", "培训结果详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTrainingResultByIdAsync(long id)
    {
        try
        {
            var result = await _trainingResultService.GetTrainingResultByIdAsync(id);
            if (result == null)
            {
                return NotFound("培训结果不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取培训结果选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:query", "培训结果选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTrainingResultOptionsAsync()
    {
        try
        {
            var result = await _trainingResultService.GetTrainingResultOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建培训结果
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>培训结果DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:create", "创建培训结果")]
    [HttpPost]
    public async Task<IActionResult> CreateTrainingResultAsync([FromBody] TaktTrainingResultCreateDto dto)
    {
        try
        {
            var result = await _trainingResultService.CreateTrainingResultAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新培训结果
    /// </summary>
    /// <param name="id">培训结果ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>培训结果DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:update", "更新培训结果")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrainingResultAsync(long id, [FromBody] TaktTrainingResultUpdateDto dto)
    {
        try
        {
            var result = await _trainingResultService.UpdateTrainingResultAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除培训结果
    /// </summary>
    /// <param name="id">培训结果ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:delete", "删除培训结果")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainingResultByIdAsync(long id)
    {
        try
        {
            await _trainingResultService.DeleteTrainingResultByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除培训结果
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:delete", "批量删除培训结果")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTrainingResultBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _trainingResultService.DeleteTrainingResultBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新培训结果状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>培训结果DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:update", "更新培训结果状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTrainingResultStatusAsync([FromBody] TaktTrainingResultStatusDto dto)
    {
        try
        {
            var result = await _trainingResultService.UpdateTrainingResultStatusAsync(dto);
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
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:import", "获取培训结果导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTrainingResultTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _trainingResultService.GetTrainingResultTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入培训结果
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:import", "导入培训结果")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTrainingResultAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _trainingResultService.ImportTrainingResultAsync(stream, sheetName);
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
    /// 导出培训结果
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:trainingdevelopment:trainingresult:export", "导出培训结果")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTrainingResultAsync([FromQuery] TaktTrainingResultQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _trainingResultService.ExportTrainingResultAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
