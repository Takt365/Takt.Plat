// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Routine.ConferenceCenter
// 文件名称：TaktConferencesController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：会议中心控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.ConferenceCenter;
using Takt.Application.Services.Routine.ConferenceCenter;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.ConferenceCenter;

/// <summary>
/// 会议中心控制器
/// 提供会议中心的 REST API
/// </summary>
[ApiModule(TaktModule.Routine, "日常事务")]
[Route("api/[controller]", Name = "会议中心")]
public class TaktConferencesController : TaktControllerBase
{
    private readonly ITaktConferenceService _conferenceService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="conferenceService">会议中心服务</param>
    public TaktConferencesController(ITaktConferenceService conferenceService)
    {
        _conferenceService = conferenceService;
    }

    /// <summary>
    /// 获取会议中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:conferencecenter:conference:list", "会议中心列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetConferenceListAsync([FromQuery] TaktConferenceQueryDto queryDto)
    {
        try
        {
            var result = await _conferenceService.GetConferenceListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>会议中心DTO</returns>
    [TaktPermission("routine:conferencecenter:conference:query", "会议中心详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConferenceByIdAsync(long id)
    {
        try
        {
            var result = await _conferenceService.GetConferenceByIdAsync(id);
            if (result == null)
            {
                return NotFound("会议中心不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会议中心主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:conferencecenter:conference:query", "会议中心选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetConferenceOptionsAsync()
    {
        try
        {
            var result = await _conferenceService.GetConferenceOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会议中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会议中心DTO</returns>
    [TaktPermission("routine:conferencecenter:conference:create", "创建会议中心")]
    [HttpPost]
    public async Task<IActionResult> CreateConferenceAsync([FromBody] TaktConferenceCreateDto dto)
    {
        try
        {
            var result = await _conferenceService.CreateConferenceAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会议中心DTO</returns>
    [TaktPermission("routine:conferencecenter:conference:update", "更新会议中心")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConferenceAsync(long id, [FromBody] TaktConferenceUpdateDto dto)
    {
        try
        {
            var result = await _conferenceService.UpdateConferenceAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:conferencecenter:conference:delete", "删除会议中心")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConferenceByIdAsync(long id)
    {
        try
        {
            await _conferenceService.DeleteConferenceByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会议中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:conferencecenter:conference:delete", "批量删除会议中心")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteConferenceBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _conferenceService.DeleteConferenceBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>会议中心DTO</returns>
    [TaktPermission("routine:conferencecenter:conference:update", "更新会议中心状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateConferenceStatusAsync([FromBody] TaktConferenceStatusDto dto)
    {
        try
        {
            var result = await _conferenceService.UpdateConferenceStatusAsync(dto);
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
    [TaktPermission("routine:conferencecenter:conference:import", "获取会议中心导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetConferenceTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _conferenceService.GetConferenceTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入会议中心
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:conferencecenter:conference:import", "导入会议中心")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportConferenceAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _conferenceService.ImportConferenceAsync(stream, sheetName);
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
    /// 导出会议中心
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:conferencecenter:conference:export", "导出会议中心")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportConferenceAsync([FromQuery] TaktConferenceQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _conferenceService.ExportConferenceAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
