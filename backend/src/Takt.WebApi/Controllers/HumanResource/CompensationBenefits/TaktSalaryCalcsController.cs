// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.CompensationBenefits
// 文件名称：TaktSalaryCalcsController.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资核算控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.CompensationBenefits;
using Takt.Application.Services.HumanResource.CompensationBenefits;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.CompensationBenefits;

/// <summary>
/// 薪资核算控制器
/// 提供薪资核算的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "薪资核算")]
public class TaktSalaryCalcsController : TaktControllerBase
{
    private readonly ITaktSalaryCalcService _salaryCalcService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salaryCalcService">薪资核算服务</param>
    public TaktSalaryCalcsController(ITaktSalaryCalcService salaryCalcService)
    {
        _salaryCalcService = salaryCalcService;
    }

    /// <summary>
    /// 获取薪资核算列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:compensationbenefits:salarycalc:list", "薪资核算列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetSalaryCalcListAsync([FromQuery] TaktSalaryCalcQueryDto queryDto)
    {
        try
        {
            var result = await _salaryCalcService.GetSalaryCalcListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取薪资核算
    /// </summary>
    /// <param name="id">薪资核算ID</param>
    /// <returns>薪资核算DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:salarycalc:query", "薪资核算详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalaryCalcByIdAsync(long id)
    {
        try
        {
            var result = await _salaryCalcService.GetSalaryCalcByIdAsync(id);
            if (result == null)
            {
                return NotFound("薪资核算不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取薪资核算选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:compensationbenefits:salarycalc:query", "薪资核算选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetSalaryCalcOptionsAsync()
    {
        try
        {
            var result = await _salaryCalcService.GetSalaryCalcOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建薪资核算
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>薪资核算DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:salarycalc:create", "创建薪资核算")]
    [HttpPost]
    public async Task<IActionResult> CreateSalaryCalcAsync([FromBody] TaktSalaryCalcCreateDto dto)
    {
        try
        {
            var result = await _salaryCalcService.CreateSalaryCalcAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪资核算
    /// </summary>
    /// <param name="id">薪资核算ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>薪资核算DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:salarycalc:update", "更新薪资核算")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSalaryCalcAsync(long id, [FromBody] TaktSalaryCalcUpdateDto dto)
    {
        try
        {
            var result = await _salaryCalcService.UpdateSalaryCalcAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除薪资核算
    /// </summary>
    /// <param name="id">薪资核算ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:compensationbenefits:salarycalc:delete", "删除薪资核算")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalaryCalcByIdAsync(long id)
    {
        try
        {
            await _salaryCalcService.DeleteSalaryCalcByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除薪资核算
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:compensationbenefits:salarycalc:delete", "批量删除薪资核算")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteSalaryCalcBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _salaryCalcService.DeleteSalaryCalcBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新薪资核算状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>薪资核算DTO</returns>
    [TaktPermission("humanresource:compensationbenefits:salarycalc:update", "更新薪资核算状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateSalaryCalcStatusAsync([FromBody] TaktSalaryCalcStatusDto dto)
    {
        try
        {
            var result = await _salaryCalcService.UpdateSalaryCalcStatusAsync(dto);
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
    [TaktPermission("humanresource:compensationbenefits:salarycalc:import", "获取薪资核算导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetSalaryCalcTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _salaryCalcService.GetSalaryCalcTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入薪资核算
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:compensationbenefits:salarycalc:import", "导入薪资核算")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportSalaryCalcAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _salaryCalcService.ImportSalaryCalcAsync(stream, sheetName);
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
    /// 导出薪资核算
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:compensationbenefits:salarycalc:export", "导出薪资核算")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportSalaryCalcAsync([FromQuery] TaktSalaryCalcQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _salaryCalcService.ExportSalaryCalcAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
