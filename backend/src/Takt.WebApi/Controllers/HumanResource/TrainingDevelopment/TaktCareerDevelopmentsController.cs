// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.TrainingDevelopment
// 文件名称：TaktCareerDevelopmentsController.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：职业发展控制器
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
/// 职业发展控制器
/// 提供职业发展的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "职业发展")]
public class TaktCareerDevelopmentsController : TaktControllerBase
{
    private readonly ITaktCareerDevelopmentService _careerDevelopmentService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="careerDevelopmentService">职业发展服务</param>
    public TaktCareerDevelopmentsController(ITaktCareerDevelopmentService careerDevelopmentService)
    {
        _careerDevelopmentService = careerDevelopmentService;
    }

    /// <summary>
    /// 获取职业发展列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:list", "职业发展列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetCareerDevelopmentListAsync([FromQuery] TaktCareerDevelopmentQueryDto queryDto)
    {
        try
        {
            var result = await _careerDevelopmentService.GetCareerDevelopmentListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取职业发展
    /// </summary>
    /// <param name="id">职业发展ID</param>
    /// <returns>职业发展DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:query", "职业发展详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCareerDevelopmentByIdAsync(long id)
    {
        try
        {
            var result = await _careerDevelopmentService.GetCareerDevelopmentByIdAsync(id);
            if (result == null)
            {
                return NotFound("职业发展不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取职业发展选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:query", "职业发展选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetCareerDevelopmentOptionsAsync()
    {
        try
        {
            var result = await _careerDevelopmentService.GetCareerDevelopmentOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建职业发展
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>职业发展DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:create", "创建职业发展")]
    [HttpPost]
    public async Task<IActionResult> CreateCareerDevelopmentAsync([FromBody] TaktCareerDevelopmentCreateDto dto)
    {
        try
        {
            var result = await _careerDevelopmentService.CreateCareerDevelopmentAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新职业发展
    /// </summary>
    /// <param name="id">职业发展ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>职业发展DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:update", "更新职业发展")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCareerDevelopmentAsync(long id, [FromBody] TaktCareerDevelopmentUpdateDto dto)
    {
        try
        {
            var result = await _careerDevelopmentService.UpdateCareerDevelopmentAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除职业发展
    /// </summary>
    /// <param name="id">职业发展ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:delete", "删除职业发展")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCareerDevelopmentByIdAsync(long id)
    {
        try
        {
            await _careerDevelopmentService.DeleteCareerDevelopmentByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除职业发展
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:delete", "批量删除职业发展")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteCareerDevelopmentBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _careerDevelopmentService.DeleteCareerDevelopmentBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新职业发展状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>职业发展DTO</returns>
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:update", "更新职业发展状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateCareerDevelopmentStatusAsync([FromBody] TaktCareerDevelopmentStatusDto dto)
    {
        try
        {
            var result = await _careerDevelopmentService.UpdateCareerDevelopmentStatusAsync(dto);
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
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:import", "获取职业发展导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetCareerDevelopmentTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _careerDevelopmentService.GetCareerDevelopmentTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入职业发展
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:import", "导入职业发展")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportCareerDevelopmentAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _careerDevelopmentService.ImportCareerDevelopmentAsync(stream, sheetName);
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
    /// 导出职业发展
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:trainingdevelopment:careerdevelopment:export", "导出职业发展")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportCareerDevelopmentAsync([FromQuery] TaktCareerDevelopmentQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _careerDevelopmentService.ExportCareerDevelopmentAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
