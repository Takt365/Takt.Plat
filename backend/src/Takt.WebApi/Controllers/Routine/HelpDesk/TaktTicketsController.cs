// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Routine.HelpDesk
// 文件名称：TaktTicketsController.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：工单控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Application.Services.Routine.HelpDesk;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.HelpDesk;

/// <summary>
/// 工单控制器
/// 提供工单的 REST API
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "工单")]
public class TaktTicketsController : TaktControllerBase
{
    private readonly ITaktTicketService _ticketService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketService">工单服务</param>
    public TaktTicketsController(ITaktTicketService ticketService)
    {
        _ticketService = ticketService;
    }

    /// <summary>
    /// 获取工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:help:desk:ticket:list", "工单列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetTicketListAsync([FromQuery] TaktTicketQueryDto queryDto)
    {
        try
        {
            var result = await _ticketService.GetTicketListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取当前用户的工单列表（分页，我的工单门户）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:helpdesk:myticket:list", "我的工单列表")]
    [HttpGet("my-list")]
    public async Task<IActionResult> GetMyTicketListAsync([FromQuery] TaktTicketQueryDto queryDto)
    {
        try
        {
            if (!CurrentUserId.HasValue || CurrentUserId.Value <= 0)
            {
                return Error("无法确定当前用户");
            }
            queryDto.SubmitterId = CurrentUserId.Value;
            var result = await _ticketService.GetTicketListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取当前用户工单关联的资产汇总（分页，按 AssetCode 聚合）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:helpdesk:myasset:list", "我的资产列表")]
    [HttpGet("my-assets")]
    public async Task<IActionResult> GetMyAssetListAsync([FromQuery] TaktTicketMyAssetQueryDto queryDto)
    {
        try
        {
            var result = await _ticketService.GetMyAssetListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:query", "工单详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketByIdAsync(long id)
    {
        try
        {
            var result = await _ticketService.GetTicketByIdAsync(id);
            if (result == null)
            {
                return NotFound("工单不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("routine:help:desk:ticket:query", "工单选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetTicketOptionsAsync()
    {
        try
        {
            var result = await _ticketService.GetTicketOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:create", "创建工单")]
    [HttpPost]
    public async Task<IActionResult> CreateTicketAsync([FromBody] TaktTicketCreateDto dto)
    {
        try
        {
            var result = await _ticketService.CreateTicketAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:update", "更新工单")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicketAsync(long id, [FromBody] TaktTicketUpdateDto dto)
    {
        try
        {
            var result = await _ticketService.UpdateTicketAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:ticket:delete", "删除工单")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicketByIdAsync(long id)
    {
        try
        {
            await _ticketService.DeleteTicketByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("routine:help:desk:ticket:delete", "批量删除工单")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteTicketBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _ticketService.DeleteTicketBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新工单状态
    /// </summary>
    /// <param name="dto">状态 DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:update", "更新工单状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateTicketStatusAsync([FromBody] TaktTicketStatusDto dto)
    {
        try
        {
            var result = await _ticketService.UpdateTicketStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 门户用户提交工单
    /// </summary>
    /// <param name="dto">提交 DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:create", "提交工单")]
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitTicketAsync([FromBody] TaktTicketSubmitDto dto)
    {
        try
        {
            var result = await _ticketService.SubmitTicketAsync(dto);
            return Success(result, "提交成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 邮件/API 渠道建单
    /// </summary>
    /// <param name="dto">渠道建单 DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:create", "渠道建单")]
    [HttpPost("channel")]
    public async Task<IActionResult> CreateTicketFromChannelAsync([FromBody] TaktTicketCreateFromChannelDto dto)
    {
        try
        {
            var result = await _ticketService.CreateTicketFromChannelAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 指派或领取工单
    /// </summary>
    /// <param name="dto">指派 DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:update", "指派工单")]
    [HttpPut("assign")]
    public async Task<IActionResult> AssignTicketAsync([FromBody] TaktTicketAssignDto dto)
    {
        try
        {
            var result = await _ticketService.AssignTicketAsync(dto);
            return Success(result, "操作成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 开始处理工单
    /// </summary>
    /// <param name="dto">动作 DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:update", "开始处理")]
    [HttpPut("start")]
    public async Task<IActionResult> StartTicketProgressAsync([FromBody] TaktTicketWorkflowActionDto dto)
    {
        try
        {
            var result = await _ticketService.StartTicketProgressAsync(dto);
            return Success(result, "操作成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 等待用户回复
    /// </summary>
    /// <param name="dto">动作 DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:update", "等待用户回复")]
    [HttpPut("wait-requester")]
    public async Task<IActionResult> WaitForRequesterAsync([FromBody] TaktTicketWorkflowActionDto dto)
    {
        try
        {
            var result = await _ticketService.WaitForRequesterAsync(dto);
            return Success(result, "操作成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 标记已解决
    /// </summary>
    /// <param name="dto">动作 DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:update", "标记已解决")]
    [HttpPut("resolve")]
    public async Task<IActionResult> ResolveTicketAsync([FromBody] TaktTicketWorkflowActionDto dto)
    {
        try
        {
            var result = await _ticketService.ResolveTicketAsync(dto);
            return Success(result, "操作成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 用户确认关闭
    /// </summary>
    /// <param name="dto">动作 DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:confirm", "确认关闭")]
    [HttpPut("confirm-close")]
    public async Task<IActionResult> ConfirmCloseTicketAsync([FromBody] TaktTicketWorkflowActionDto dto)
    {
        try
        {
            var result = await _ticketService.ConfirmCloseTicketAsync(dto);
            return Success(result, "关闭成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 重新打开工单
    /// </summary>
    /// <param name="dto">动作 DTO</param>
    /// <returns>工单DTO</returns>
    [TaktPermission("routine:help:desk:ticket:update", "重新打开")]
    [HttpPut("reopen")]
    public async Task<IActionResult> ReopenTicketAsync([FromBody] TaktTicketWorkflowActionDto dto)
    {
        try
        {
            var result = await _ticketService.ReopenTicketAsync(dto);
            return Success(result, "操作成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 添加工单回复
    /// </summary>
    /// <param name="dto">回复 DTO</param>
    /// <returns>回复 DTO</returns>
    [TaktPermission("routine:help:desk:ticket:reply", "回复工单")]
    [HttpPost("reply")]
    public async Task<IActionResult> ReplyTicketAsync([FromBody] TaktTicketReplyCreateDto dto)
    {
        try
        {
            var result = await _ticketService.ReplyTicketAsync(dto);
            return Success(result, "回复成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取工单回复列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("routine:help:desk:ticket:query", "工单回复列表")]
    [HttpGet("replies")]
    public async Task<IActionResult> GetTicketReplyListAsync([FromQuery] TaktTicketReplyQueryDto queryDto)
    {
        try
        {
            var result = await _ticketService.GetTicketReplyListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
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
    [TaktPermission("routine:help:desk:ticket:import", "获取工单导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> GetTicketTemplateAsync([FromQuery] string? sheetName = null, [FromQuery] string? templateName = null)
    {
        try
        {
            var (resultFileName, content) = await _ticketService.GetTicketTemplateAsync(sheetName, templateName);
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导入工单
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("routine:help:desk:ticket:import", "导入工单")]
    [HttpPost("import")]
    public async Task<IActionResult> ImportTicketAsync(IFormFile file, [FromQuery] string? sheetName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await _ticketService.ImportTicketAsync(stream, sheetName);
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
    /// 导出工单
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("routine:help:desk:ticket:export", "导出工单")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportTicketAsync([FromQuery] TaktTicketQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _ticketService.ExportTicketAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
