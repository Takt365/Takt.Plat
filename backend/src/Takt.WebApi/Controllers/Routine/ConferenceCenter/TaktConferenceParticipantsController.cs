// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Routine.ConferenceCenter
// 文件名称：TaktConferenceParticipantsController.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：会议参与人控制器
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
/// 会议参与人控制器
/// 提供会议参与人的 REST API
/// </summary>
[ApiModule(TaktModule.Routine, "日常事务")]
[Route("api/[controller]", Name = "会议参与人")]
public class TaktConferenceParticipantsController : TaktControllerBase
{
    private readonly ITaktConferenceParticipantService _conferenceParticipantService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="conferenceParticipantService">会议参与人服务</param>
    public TaktConferenceParticipantsController(ITaktConferenceParticipantService conferenceParticipantService)
    {
        _conferenceParticipantService = conferenceParticipantService;
    }

    /// <summary>
    /// 获取会议参与人列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:conferencecenter:conferenceparticipant:list", "会议参与人列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetConferenceParticipantListAsync([FromQuery] TaktConferenceParticipantQueryDto queryDto)
    {
        try
        {
            var result = await _conferenceParticipantService.GetConferenceParticipantListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会议参与人
    /// </summary>
    /// <param name="id">会议参与人ID</param>
    /// <returns>会议参与人DTO</returns>
    [TaktPermission("routine:conferencecenter:conferenceparticipant:query", "会议参与人详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConferenceParticipantByIdAsync(long id)
    {
        try
        {
            var result = await _conferenceParticipantService.GetConferenceParticipantByIdAsync(id);
            if (result == null)
            {
                return NotFound("会议参与人不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会议参与人选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:conferencecenter:conferenceparticipant:query", "会议参与人选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetConferenceParticipantOptionsAsync()
    {
        try
        {
            var result = await _conferenceParticipantService.GetConferenceParticipantOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会议参与人
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会议参与人DTO</returns>
    [TaktPermission("routine:conferencecenter:conferenceparticipant:create", "创建会议参与人")]
    [HttpPost]
    public async Task<IActionResult> CreateConferenceParticipantAsync([FromBody] TaktConferenceParticipantCreateDto dto)
    {
        try
        {
            var result = await _conferenceParticipantService.CreateConferenceParticipantAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议参与人
    /// </summary>
    /// <param name="id">会议参与人ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会议参与人DTO</returns>
    [TaktPermission("routine:conferencecenter:conferenceparticipant:update", "更新会议参与人")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConferenceParticipantAsync(long id, [FromBody] TaktConferenceParticipantUpdateDto dto)
    {
        try
        {
            var result = await _conferenceParticipantService.UpdateConferenceParticipantAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会议参与人
    /// </summary>
    /// <param name="id">会议参与人ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:conferencecenter:conferenceparticipant:delete", "删除会议参与人")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConferenceParticipantByIdAsync(long id)
    {
        try
        {
            await _conferenceParticipantService.DeleteConferenceParticipantByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会议参与人
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:conferencecenter:conferenceparticipant:delete", "批量删除会议参与人")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteConferenceParticipantBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _conferenceParticipantService.DeleteConferenceParticipantBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议参与人状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>会议参与人DTO</returns>
    [TaktPermission("routine:conferencecenter:conferenceparticipant:update", "更新会议参与人状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateConferenceParticipantStatusAsync([FromBody] TaktConferenceParticipantStatusDto dto)
    {
        try
        {
            var result = await _conferenceParticipantService.UpdateConferenceParticipantStatusAsync(dto);
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
    [TaktPermission("routine:conferencecenter:conferenceparticipant:import", "获取会议参与人导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetConferenceParticipantTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _conferenceParticipantService.GetConferenceParticipantTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入会议参与人
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:conferencecenter:conferenceparticipant:import", "导入会议参与人")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportConferenceParticipantAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _conferenceParticipantService.ImportConferenceParticipantAsync(stream, sheetName);
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
    /// 导出会议参与人
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:conferencecenter:conferenceparticipant:export", "导出会议参与人")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportConferenceParticipantAsync([FromQuery] TaktConferenceParticipantQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _conferenceParticipantService.ExportConferenceParticipantAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
