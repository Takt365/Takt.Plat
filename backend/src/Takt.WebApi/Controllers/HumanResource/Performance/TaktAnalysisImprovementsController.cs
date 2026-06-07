// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.Performance
// 文件名称：TaktAnalysisImprovementsController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效分析改进控制器
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
/// 绩效分析改进控制器
/// 提供绩效分析改进的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "绩效分析改进")]
public class TaktAnalysisImprovementsController : TaktControllerBase
{
    private readonly ITaktAnalysisImprovementService _analysisImprovementService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="analysisImprovementService">绩效分析改进服务</param>
    public TaktAnalysisImprovementsController(ITaktAnalysisImprovementService analysisImprovementService)
    {
        _analysisImprovementService = analysisImprovementService;
    }

    /// <summary>
    /// 获取绩效分析改进列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:performance:analysisimprovement:list", "绩效分析改进列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAnalysisImprovementListAsync([FromQuery] TaktAnalysisImprovementQueryDto queryDto)
    {
        try
        {
            var result = await _analysisImprovementService.GetAnalysisImprovementListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取绩效分析改进
    /// </summary>
    /// <param name="id">绩效分析改进ID</param>
    /// <returns>绩效分析改进DTO</returns>
    [TaktPermission("humanresource:performance:analysisimprovement:query", "绩效分析改进详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAnalysisImprovementByIdAsync(long id)
    {
        try
        {
            var result = await _analysisImprovementService.GetAnalysisImprovementByIdAsync(id);
            if (result == null)
            {
                return NotFound("绩效分析改进不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取绩效分析改进选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:performance:analysisimprovement:query", "绩效分析改进选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetAnalysisImprovementOptionsAsync()
    {
        try
        {
            var result = await _analysisImprovementService.GetAnalysisImprovementOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建绩效分析改进
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>绩效分析改进DTO</returns>
    [TaktPermission("humanresource:performance:analysisimprovement:create", "创建绩效分析改进")]
    [HttpPost]
    public async Task<IActionResult> CreateAnalysisImprovementAsync([FromBody] TaktAnalysisImprovementCreateDto dto)
    {
        try
        {
            var result = await _analysisImprovementService.CreateAnalysisImprovementAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效分析改进
    /// </summary>
    /// <param name="id">绩效分析改进ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>绩效分析改进DTO</returns>
    [TaktPermission("humanresource:performance:analysisimprovement:update", "更新绩效分析改进")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnalysisImprovementAsync(long id, [FromBody] TaktAnalysisImprovementUpdateDto dto)
    {
        try
        {
            var result = await _analysisImprovementService.UpdateAnalysisImprovementAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除绩效分析改进
    /// </summary>
    /// <param name="id">绩效分析改进ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:performance:analysisimprovement:delete", "删除绩效分析改进")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnalysisImprovementByIdAsync(long id)
    {
        try
        {
            await _analysisImprovementService.DeleteAnalysisImprovementByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除绩效分析改进
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:performance:analysisimprovement:delete", "批量删除绩效分析改进")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteAnalysisImprovementBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _analysisImprovementService.DeleteAnalysisImprovementBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新绩效分析改进状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>绩效分析改进DTO</returns>
    [TaktPermission("humanresource:performance:analysisimprovement:update", "更新绩效分析改进状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateAnalysisImprovementStatusAsync([FromBody] TaktAnalysisImprovementStatusDto dto)
    {
        try
        {
            var result = await _analysisImprovementService.UpdateAnalysisImprovementStatusAsync(dto);
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
    [TaktPermission("humanresource:performance:analysisimprovement:import", "获取绩效分析改进导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetAnalysisImprovementTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _analysisImprovementService.GetAnalysisImprovementTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入绩效分析改进
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:performance:analysisimprovement:import", "导入绩效分析改进")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportAnalysisImprovementAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _analysisImprovementService.ImportAnalysisImprovementAsync(stream, sheetName);
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
    /// 导出绩效分析改进
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:performance:analysisimprovement:export", "导出绩效分析改进")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportAnalysisImprovementAsync([FromQuery] TaktAnalysisImprovementQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _analysisImprovementService.ExportAnalysisImprovementAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
