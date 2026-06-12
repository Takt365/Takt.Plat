// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktMessageService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线消息应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 在线消息应用服务接口
/// </summary>
public interface ITaktMessageService
{
    /// <summary>
    /// 获取在线消息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMessageDto>> GetMessageListAsync(TaktMessageQueryDto queryDto);

    /// <summary>
    /// 获取当前登录用户已读消息列表（分页）
    /// </summary>
    /// <param name="queryDto">已读列表查询 DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMessageDto>> GetMessageReadListAsync(TaktMessageInboxListQueryDto queryDto);

    /// <summary>
    /// 获取当前登录用户未读消息列表（分页）
    /// </summary>
    /// <param name="queryDto">未读列表查询 DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMessageDto>> GetMessageUnreadListAsync(TaktMessageInboxListQueryDto queryDto);

    /// <summary>
    /// 根据ID获取在线消息
    /// </summary>
    /// <param name="id">在线消息ID</param>
    /// <returns>DTO</returns>
    Task<TaktMessageDto?> GetMessageByIdAsync(long id);

    /// <summary>
    /// 获取在线消息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMessageOptionsAsync();

    /// <summary>
    /// 创建在线消息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMessageDto> CreateMessageAsync(TaktMessageCreateDto dto);

    /// <summary>
    /// 批量创建在线消息并 SignalR 推送给各接收者（全员或指定用户列表）
    /// </summary>
    /// <param name="dto">批量创建 DTO</param>
    /// <returns>已落库消息列表</returns>
    Task<List<TaktMessageDto>> CreateAndSendMessagesAsync(TaktMessageBatchCreateDto dto);

    /// <summary>
    /// 按消息 ID 经 SignalR 推送给接收者（须已落库）
    /// </summary>
    /// <param name="id">在线消息 ID</param>
    /// <returns>任务</returns>
    Task SendMessageByIdAsync(long id);

    /// <summary>
    /// 删除在线消息
    /// </summary>
    /// <param name="id">在线消息ID</param>
    /// <returns>任务</returns>
    Task DeleteMessageByIdAsync(long id);

    /// <summary>
    /// 批量删除在线消息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMessageBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 标记在线消息为已读
    /// </summary>
    /// <param name="dto">已读 DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMessageDto> MarkMessageReadAsync(TaktMessageReadDto dto);

    /// <summary>
    /// 标记在线消息为未读
    /// </summary>
    /// <param name="dto">未读 DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMessageDto> MarkMessageUnreadAsync(TaktMessageUnreadDto dto);

    /// <summary>
    /// 获取指定用户未读消息数量（SignalR Hub 调用）
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns>未读数量</returns>
    Task<int> GetUnreadMessageCountAsync(string userName);

    /// <summary>
    /// 导出在线消息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMessageAsync(TaktMessageQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取当前登录用户在线消息统计（接收消息：总数/已读/未读）
    /// </summary>
    /// <returns>统计 DTO</returns>
    Task<TaktMessageStatisticsDto> GetMessageStatisticsAsync();

    /// <summary>
    /// 获取指定用户在线消息统计（SignalR 实时推送调用）
    /// </summary>
    /// <param name="userName">用户名（接收者）</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>统计 DTO</returns>
    Task<TaktMessageStatisticsDto> GetMessageStatisticsByUserNameAsync(string userName, long? userId = null);

}
