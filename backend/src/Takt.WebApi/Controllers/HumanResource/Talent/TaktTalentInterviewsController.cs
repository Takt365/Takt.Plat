// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.HumanResource.Talent
// 文件名称：TaktTalentInterviewsController.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：面试安排控制器
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
/// 面试安排控制器
/// 提供面试安排的 REST API
/// </summary>
[ApiModule(TaktModule.HumanResource, "人力资源")]
[Route("api/[controller]", Name = "面试安排")]
public class TaktTalentInterviewsController : TaktControllerBase
{
    private readonly ITaktTalentInterviewService _talentInterviewService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentInterviewService">面试安排服务</param>
    public TaktTalentInterviewsController(ITaktTalentInterviewService talentInterviewService)
    {
        _talentInterviewService = talentInterviewService;
    }

    /// <summary>
    /// 获取面试安排列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("humanresource:talent:talentinterview:list", "面试安排列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTalentInterviewListAsync([FromQuery] TaktTalentInterviewQueryDto queryDto)
    {
        try
        {
            var result = await _talentInterviewService.GetTalentInterviewListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取面试安排
    /// </summary>
    /// <param name="id">面试安排ID</param>
    /// <returns>面试安排DTO</returns>
    [TaktPermission("humanresource:talent:talentinterview:query", "面试安排详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTalentInterviewByIdAsync(long id)
    {
        try
        {
            var result = await _talentInterviewService.GetTalentInterviewByIdAsync(id);
            if (result == null)
            {
                return NotFound("面试安排不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取面试安排选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("humanresource:talent:talentinterview:query", "面试安排选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTalentInterviewOptionsAsync()
    {
        try
        {
            var result = await _talentInterviewService.GetTalentInterviewOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建面试安排
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>面试安排DTO</returns>
    [TaktPermission("humanresource:talent:talentinterview:create", "创建面试安排")]
    [HttpPost]
    public async Task<IActionResult> CreateTalentInterviewAsync([FromBody] TaktTalentInterviewCreateDto dto)
    {
        try
        {
            var result = await _talentInterviewService.CreateTalentInterviewAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新面试安排
    /// </summary>
    /// <param name="id">面试安排ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>面试安排DTO</returns>
    [TaktPermission("humanresource:talent:talentinterview:update", "更新面试安排")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTalentInterviewAsync(long id, [FromBody] TaktTalentInterviewUpdateDto dto)
    {
        try
        {
            var result = await _talentInterviewService.UpdateTalentInterviewAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除面试安排
    /// </summary>
    /// <param name="id">面试安排ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:talent:talentinterview:delete", "删除面试安排")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTalentInterviewByIdAsync(long id)
    {
        try
        {
            await _talentInterviewService.DeleteTalentInterviewByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除面试安排
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("humanresource:talent:talentinterview:delete", "批量删除面试安排")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTalentInterviewBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _talentInterviewService.DeleteTalentInterviewBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新面试安排状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>面试安排DTO</returns>
    [TaktPermission("humanresource:talent:talentinterview:update", "更新面试安排状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTalentInterviewStatusAsync([FromBody] TaktTalentInterviewStatusDto dto)
    {
        try
        {
            var result = await _talentInterviewService.UpdateTalentInterviewStatusAsync(dto);
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
    [TaktPermission("humanresource:talent:talentinterview:import", "获取面试安排导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTalentInterviewTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _talentInterviewService.GetTalentInterviewTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入面试安排
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("humanresource:talent:talentinterview:import", "导入面试安排")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTalentInterviewAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _talentInterviewService.ImportTalentInterviewAsync(stream, sheetName);
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
    /// 导出面试安排
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("humanresource:talent:talentinterview:export", "导出面试安排")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTalentInterviewAsync([FromQuery] TaktTalentInterviewQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _talentInterviewService.ExportTalentInterviewAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
