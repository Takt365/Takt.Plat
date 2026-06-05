// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktMessagesController.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线消息控制器
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// 在线消息控制器
/// 提供在线消息的 REST API
/// </summary>
[ApiModule(TaktModule.Foundation, "基础设置")]
[Route("api/[controller]", Name = "在线消息")]
public class TaktMessagesController : TaktControllerBase
{
    private readonly ITaktMessageService _messageService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="messageService">在线消息服务</param>
    public TaktMessagesController(ITaktMessageService messageService)
    {
        _messageService = messageService;
    }

    /// <summary>
    /// 获取在线消息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("foundation:message:list", "在线消息列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetMessageListAsync([FromQuery] TaktMessageQueryDto queryDto)
    {
        try
        {
            var result = await _messageService.GetMessageListAsync(queryDto);
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 根据ID获取在线消息
    /// </summary>
    /// <param name="id">在线消息ID</param>
    /// <returns>在线消息DTO</returns>
    [TaktPermission("foundation:message:query", "在线消息详情")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessageByIdAsync(long id)
    {
        try
        {
            var result = await _messageService.GetMessageByIdAsync(id);
            if (result == null)
            {
                return NotFound("在线消息不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取在线消息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    [TaktPermission("foundation:message:query", "在线消息选项")]
    [HttpGet("options")]
    public async Task<IActionResult> GetMessageOptionsAsync()
    {
        try
        {
            var result = await _messageService.GetMessageOptionsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 创建在线消息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>在线消息DTO</returns>
    [TaktPermission("foundation:message:create", "创建在线消息")]
    [HttpPost]
    public async Task<IActionResult> CreateMessageAsync([FromBody] TaktMessageCreateDto dto)
    {
        try
        {
            var result = await _messageService.CreateMessageAsync(dto);
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新在线消息
    /// </summary>
    /// <param name="id">在线消息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>在线消息DTO</returns>
    [TaktPermission("foundation:message:update", "更新在线消息")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMessageAsync(long id, [FromBody] TaktMessageUpdateDto dto)
    {
        try
        {
            var result = await _messageService.UpdateMessageAsync(id, dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 删除在线消息
    /// </summary>
    /// <param name="id">在线消息ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:message:delete", "删除在线消息")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessageByIdAsync(long id)
    {
        try
        {
            await _messageService.DeleteMessageByIdAsync(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 批量删除在线消息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("foundation:message:delete", "批量删除在线消息")]
    [HttpDelete("batch")]
    public async Task<IActionResult> DeleteMessageBatchAsync([FromBody] IEnumerable<long> ids)
    {
        try
        {
            await _messageService.DeleteMessageBatchAsync(ids);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 更新在线消息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>在线消息DTO</returns>
    [TaktPermission("foundation:message:update", "更新在线消息状态")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateMessageStatusAsync([FromBody] TaktMessageStatusDto dto)
    {
        try
        {
            var result = await _messageService.UpdateMessageStatusAsync(dto);
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取当前登录用户在线消息统计
    /// </summary>
    /// <returns>统计结果</returns>
    [TaktPermission("foundation:message:query", "当前用户在线消息统计")]
    [HttpGet("statistics")]
    public async Task<IActionResult> GetMessageStatisticsAsync()
    {
        try
        {
            var result = await _messageService.GetMessageStatisticsAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出在线消息
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("foundation:message:export", "导出在线消息")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportMessageAsync([FromQuery] TaktMessageQueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _messageService.ExportMessageAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
