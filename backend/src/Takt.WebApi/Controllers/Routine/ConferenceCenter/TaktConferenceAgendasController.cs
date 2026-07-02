// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Routine.ConferenceCenter
// 文件名称：TaktConferenceAgendasController.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：会议议程纪要控制器
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
/// 会议议程纪要控制器
/// 提供会议议程纪要的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "会议议程纪要")]
public class TaktConferenceAgendasController : TaktControllerBase
{
    private readonly ITaktConferenceAgendaService _conferenceAgendaService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="conferenceAgendaService">会议议程纪要服务</param>
    public TaktConferenceAgendasController(ITaktConferenceAgendaService conferenceAgendaService)
    {
        _conferenceAgendaService = conferenceAgendaService;
    }

    /// <summary>
    /// 获取会议议程纪要列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:conference:center:agenda:list", "会议议程纪要列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetConferenceAgendaListAsync([FromQuery] TaktConferenceAgendaQueryDto queryDto)
    {
        try
        {
            var result = await _conferenceAgendaService.GetConferenceAgendaListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取会议议程纪要
    /// </summary>
    /// <param name="id">会议议程纪要ID</param>
    /// <returns>会议议程纪要DTO</returns>
    [TaktPermission("routine:conference:center:agenda:query", "会议议程纪要详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConferenceAgendaByIdAsync(long id)
    {
        try
        {
            var result = await _conferenceAgendaService.GetConferenceAgendaByIdAsync(id);
            if (result == null)
            {
                return NotFound("会议议程纪要不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取会议议程纪要选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:conference:center:agenda:query", "会议议程纪要选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetConferenceAgendaOptionsAsync()
    {
        try
        {
            var result = await _conferenceAgendaService.GetConferenceAgendaOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建会议议程纪要
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>会议议程纪要DTO</returns>
    [TaktPermission("routine:conference:center:agenda:create", "创建会议议程纪要")]
    [HttpPost]
    public async Task<IActionResult> CreateConferenceAgendaAsync([FromBody] TaktConferenceAgendaCreateDto dto)
    {
        try
        {
            var result = await _conferenceAgendaService.CreateConferenceAgendaAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新会议议程纪要
    /// </summary>
    /// <param name="id">会议议程纪要ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>会议议程纪要DTO</returns>
    [TaktPermission("routine:conference:center:agenda:update", "更新会议议程纪要")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConferenceAgendaAsync(long id, [FromBody] TaktConferenceAgendaUpdateDto dto)
    {
        try
        {
            var result = await _conferenceAgendaService.UpdateConferenceAgendaAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除会议议程纪要
    /// </summary>
    /// <param name="id">会议议程纪要ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:conference:center:agenda:delete", "删除会议议程纪要")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConferenceAgendaByIdAsync(long id)
    {
        try
        {
            await _conferenceAgendaService.DeleteConferenceAgendaByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除会议议程纪要
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:conference:center:agenda:delete", "批量删除会议议程纪要")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteConferenceAgendaBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _conferenceAgendaService.DeleteConferenceAgendaBatchAsync(ids);
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
    [TaktPermission("routine:conference:center:agenda:import", "获取会议议程纪要导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetConferenceAgendaTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _conferenceAgendaService.GetConferenceAgendaTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入会议议程纪要
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:conference:center:agenda:import", "导入会议议程纪要")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportConferenceAgendaAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _conferenceAgendaService.ImportConferenceAgendaAsync(stream, sheetName);
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
    /// 导出会议议程纪要
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:conference:center:agenda:export", "导出会议议程纪要")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportConferenceAgendaAsync([FromQuery] TaktConferenceAgendaQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _conferenceAgendaService.ExportConferenceAgendaAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
