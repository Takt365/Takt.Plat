// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.Talent
// 文件名称：TaktTalentRecruitmentPlansController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：招聘计划控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.HumanResource.Talent;
using Takt.Application.Services.HumanResource.Talent;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.HumanResource.Talent;

/// <summary>
/// 招聘计划控制器
/// 提供招聘计划的 REST API
/// </summary>
[ApiModule(5, "人力资源")]
[Route("api/[controller]", Name = "招聘计划")]
public class TaktTalentRecruitmentPlansController : TaktControllerBase
{
    private readonly ITaktTalentRecruitmentPlanService _talentRecruitmentPlanService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentRecruitmentPlanService">招聘计划服务</param>
    public TaktTalentRecruitmentPlansController(ITaktTalentRecruitmentPlanService talentRecruitmentPlanService)
    {
        _talentRecruitmentPlanService = talentRecruitmentPlanService;
    }

    /// <summary>
    /// 获取招聘计划列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:talent:talentrecruitmentplan:list", "招聘计划列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTalentRecruitmentPlanListAsync([FromQuery] TaktTalentRecruitmentPlanQueryDto queryDto)
    {
        try
        {
            var result = await _talentRecruitmentPlanService.GetTalentRecruitmentPlanListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取招聘计划
    /// </summary>
    /// <param name="id">招聘计划ID</param>
    /// <returns>招聘计划DTO</returns>
    [TaktPermission("humanresource:talent:talentrecruitmentplan:query", "招聘计划详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTalentRecruitmentPlanByIdAsync(long id)
    {
        try
        {
            var result = await _talentRecruitmentPlanService.GetTalentRecruitmentPlanByIdAsync(id);
            if (result == null)
            {
                return NotFound("招聘计划不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取招聘计划选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:talent:talentrecruitmentplan:query", "招聘计划选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTalentRecruitmentPlanOptionsAsync()
    {
        try
        {
            var result = await _talentRecruitmentPlanService.GetTalentRecruitmentPlanOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建招聘计划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>招聘计划DTO</returns>
    [TaktPermission("humanresource:talent:talentrecruitmentplan:create", "创建招聘计划")]
    [HttpPost]
    public async Task<IActionResult> CreateTalentRecruitmentPlanAsync([FromBody] TaktTalentRecruitmentPlanCreateDto dto)
    {
        try
        {
            var result = await _talentRecruitmentPlanService.CreateTalentRecruitmentPlanAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新招聘计划
    /// </summary>
    /// <param name="id">招聘计划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>招聘计划DTO</returns>
    [TaktPermission("humanresource:talent:talentrecruitmentplan:update", "更新招聘计划")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTalentRecruitmentPlanAsync(long id, [FromBody] TaktTalentRecruitmentPlanUpdateDto dto)
    {
        try
        {
            var result = await _talentRecruitmentPlanService.UpdateTalentRecruitmentPlanAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除招聘计划
    /// </summary>
    /// <param name="id">招聘计划ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:talent:talentrecruitmentplan:delete", "删除招聘计划")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTalentRecruitmentPlanByIdAsync(long id)
    {
        try
        {
            await _talentRecruitmentPlanService.DeleteTalentRecruitmentPlanByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除招聘计划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:talent:talentrecruitmentplan:delete", "批量删除招聘计划")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTalentRecruitmentPlanBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _talentRecruitmentPlanService.DeleteTalentRecruitmentPlanBatchAsync(ids);
            return Success("删除成功");
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
    [TaktPermission("humanresource:talent:talentrecruitmentplan:import", "获取招聘计划导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTalentRecruitmentPlanTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _talentRecruitmentPlanService.GetTalentRecruitmentPlanTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入招聘计划
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:talent:talentrecruitmentplan:import", "导入招聘计划")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTalentRecruitmentPlanAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _talentRecruitmentPlanService.ImportTalentRecruitmentPlanAsync(stream, sheetName);
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
    /// 导出招聘计划
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:talent:talentrecruitmentplan:export", "导出招聘计划")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTalentRecruitmentPlanAsync([FromQuery] TaktTalentRecruitmentPlanQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _talentRecruitmentPlanService.ExportTalentRecruitmentPlanAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
