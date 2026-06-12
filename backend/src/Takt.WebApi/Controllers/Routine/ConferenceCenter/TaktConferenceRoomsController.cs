// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Routine.ConferenceCenter
// 文件名称：TaktConferenceRoomsController.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：会议室控制器
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
/// 会议室控制器
/// 提供会议室的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "会议室")]
public class TaktConferenceRoomsController : TaktControllerBase
{
    private readonly ITaktConferenceRoomService _conferenceRoomService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="conferenceRoomService">会议室服务</param>
    public TaktConferenceRoomsController(ITaktConferenceRoomService conferenceRoomService)
    {
        _conferenceRoomService = conferenceRoomService;
    }

    /// <summary>
    /// 获取会议室列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:list", "会议室列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetConferenceRoomListAsync([FromQuery] TaktConferenceRoomQueryDto queryDto)
    {
        try
        {
            var result = await _conferenceRoomService.GetConferenceRoomListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会议室
    /// </summary>
    /// <param name="id">会议室ID</param>
    /// <returns>会议室DTO</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:query", "会议室详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConferenceRoomByIdAsync(long id)
    {
        try
        {
            var result = await _conferenceRoomService.GetConferenceRoomByIdAsync(id);
            if (result == null)
            {
                return NotFound("会议室不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会议室选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:query", "会议室选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetConferenceRoomOptionsAsync()
    {
        try
        {
            var result = await _conferenceRoomService.GetConferenceRoomOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会议室
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会议室DTO</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:create", "创建会议室")]
    [HttpPost]
    public async Task<IActionResult> CreateConferenceRoomAsync([FromBody] TaktConferenceRoomCreateDto dto)
    {
        try
        {
            var result = await _conferenceRoomService.CreateConferenceRoomAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议室
    /// </summary>
    /// <param name="id">会议室ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会议室DTO</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:update", "更新会议室")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConferenceRoomAsync(long id, [FromBody] TaktConferenceRoomUpdateDto dto)
    {
        try
        {
            var result = await _conferenceRoomService.UpdateConferenceRoomAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会议室
    /// </summary>
    /// <param name="id">会议室ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:delete", "删除会议室")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConferenceRoomByIdAsync(long id)
    {
        try
        {
            await _conferenceRoomService.DeleteConferenceRoomByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会议室
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:delete", "批量删除会议室")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteConferenceRoomBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _conferenceRoomService.DeleteConferenceRoomBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议室状态
    /// </summary>
    /// <param name="dto">状态 DTO（TaktConferenceRoomStatus 枚举）</param>
    /// <returns>会议室DTO</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:update", "更新会议室状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateConferenceRoomStatusAsync([FromBody] TaktConferenceRoomStatusDto dto)
    {
        try
        {
            var result = await _conferenceRoomService.UpdateConferenceRoomStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议室排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>会议室DTO</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:update", "更新会议室排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> UpdateConferenceRoomSortAsync([FromBody] TaktConferenceRoomSortDto dto)
    {
        try
        {
            var result = await _conferenceRoomService.UpdateConferenceRoomSortAsync(dto);
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
    [TaktPermission("routine:conferencecenter:conferenceroom:import", "获取会议室导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetConferenceRoomTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _conferenceRoomService.GetConferenceRoomTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入会议室
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:import", "导入会议室")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportConferenceRoomAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _conferenceRoomService.ImportConferenceRoomAsync(stream, sheetName);
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
    /// 导出会议室
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:conferencecenter:conferenceroom:export", "导出会议室")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportConferenceRoomAsync([FromQuery] TaktConferenceRoomQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _conferenceRoomService.ExportConferenceRoomAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
