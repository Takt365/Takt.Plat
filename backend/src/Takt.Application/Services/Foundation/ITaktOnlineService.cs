// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktOnlineService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线用户应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 在线用户应用服务接口
/// </summary>
public interface ITaktOnlineService
{
    /// <summary>
    /// 获取在线用户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktOnlineDto>> GetOnlineListAsync(TaktOnlineQueryDto queryDto);

    /// <summary>
    /// 根据ID获取在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>DTO</returns>
    Task<TaktOnlineDto?> GetOnlineByIdAsync(long id);

    /// <summary>
    /// 获取在线用户选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetOnlineOptionsAsync();

    /// <summary>
    /// 创建在线用户
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktOnlineDto> CreateOnlineAsync(TaktOnlineCreateDto dto);

    /// <summary>
    /// 注册 SignalR 在线会话（同租户+公司+用户复用主记录；其它仍 Online 的会话标为离线）
    /// </summary>
    /// <param name="dto">连接信息</param>
    /// <returns>在线用户 DTO</returns>
    Task<TaktOnlineDto> RegisterOnlineSessionAsync(TaktOnlineCreateDto dto);

    /// <summary>
    /// 更新在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktOnlineDto> UpdateOnlineAsync(long id, TaktOnlineUpdateDto dto);

    /// <summary>
    /// 删除在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>任务</returns>
    Task DeleteOnlineByIdAsync(long id);

    /// <summary>
    /// 批量删除在线用户
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteOnlineBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新在线用户状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktOnlineDto> UpdateOnlineStatusAsync(TaktOnlineStatusDto dto);

    /// <summary>
    /// 导出在线用户
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportOnlineAsync(TaktOnlineQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取当前登录用户在线统计（在线连接数、在线时长：当前/当天/当月）
    /// </summary>
    /// <returns>统计 DTO</returns>
    Task<TaktOnlineStatisticsDto> GetOnlineStatisticsAsync();

    /// <summary>
    /// 获取指定用户在线统计（SignalR 实时推送调用）
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>统计 DTO</returns>
    Task<TaktOnlineStatisticsDto> GetOnlineStatisticsByUserNameAsync(string userName, long? userId = null);

}
