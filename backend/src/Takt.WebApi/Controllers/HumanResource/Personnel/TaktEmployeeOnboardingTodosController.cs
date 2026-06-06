// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.Personnel
// 文件名称：TaktEmployeeOnboardingTodosController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：入职待办控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Application.Services.HumanResource.Personnel;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Personnel;

/// <summary>
/// 入职待办控制器
/// 提供入职待办的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人事管理")]
[Route("api/[controller]", Name = "入职待办")]
public class TaktEmployeeOnboardingTodosController : TaktControllerBase
{
    private readonly ITaktEmployeeOnboardingTodoService _employeeOnboardingTodoService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeOnboardingTodoService">入职待办服务</param>
    public TaktEmployeeOnboardingTodosController(ITaktEmployeeOnboardingTodoService employeeOnboardingTodoService)
    {
        _employeeOnboardingTodoService = employeeOnboardingTodoService;
    }

    /// <summary>
    /// 获取入职待办列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:list", "入职待办列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEmployeeOnboardingTodoListAsync([FromQuery] TaktEmployeeOnboardingTodoQueryDto queryDto)
    {
        try
        {
            var result = await _employeeOnboardingTodoService.GetEmployeeOnboardingTodoListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <returns>入职待办DTO</returns>
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:query", "入职待办详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeOnboardingTodoByIdAsync(long id)
    {
        try
        {
            var result = await _employeeOnboardingTodoService.GetEmployeeOnboardingTodoByIdAsync(id);
            if (result == null)
            {
                return NotFound("入职待办不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取入职待办选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:query", "入职待办选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetEmployeeOnboardingTodoOptionsAsync()
    {
        try
        {
            var result = await _employeeOnboardingTodoService.GetEmployeeOnboardingTodoOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建入职待办
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>入职待办DTO</returns>
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:create", "创建入职待办")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeOnboardingTodoAsync([FromBody] TaktEmployeeOnboardingTodoCreateDto dto)
    {
        try
        {
            var result = await _employeeOnboardingTodoService.CreateEmployeeOnboardingTodoAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>入职待办DTO</returns>
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:update", "更新入职待办")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeOnboardingTodoAsync(long id, [FromBody] TaktEmployeeOnboardingTodoUpdateDto dto)
    {
        try
        {
            var result = await _employeeOnboardingTodoService.UpdateEmployeeOnboardingTodoAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:delete", "删除入职待办")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeOnboardingTodoByIdAsync(long id)
    {
        try
        {
            await _employeeOnboardingTodoService.DeleteEmployeeOnboardingTodoByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除入职待办
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:delete", "批量删除入职待办")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteEmployeeOnboardingTodoBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _employeeOnboardingTodoService.DeleteEmployeeOnboardingTodoBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新入职待办状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>入职待办DTO</returns>
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:update", "更新入职待办状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateEmployeeOnboardingTodoStatusAsync([FromBody] TaktEmployeeOnboardingTodoStatusDto dto)
    {
        try
        {
            var result = await _employeeOnboardingTodoService.UpdateEmployeeOnboardingTodoStatusAsync(dto);
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
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:import", "获取入职待办导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetEmployeeOnboardingTodoTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _employeeOnboardingTodoService.GetEmployeeOnboardingTodoTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入入职待办
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:import", "导入入职待办")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportEmployeeOnboardingTodoAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _employeeOnboardingTodoService.ImportEmployeeOnboardingTodoAsync(stream, sheetName);
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
    /// 导出入职待办
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:personnel:employeeonboardingtodo:export", "导出入职待办")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEmployeeOnboardingTodoAsync([FromQuery] TaktEmployeeOnboardingTodoQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _employeeOnboardingTodoService.ExportEmployeeOnboardingTodoAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
