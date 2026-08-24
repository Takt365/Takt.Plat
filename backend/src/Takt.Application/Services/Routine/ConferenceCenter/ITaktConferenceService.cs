// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.ConferenceCenter
// 文件名称：ITaktConferenceService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：会议中心应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Routine.ConferenceCenter;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.ConferenceCenter;

/// <summary>
/// 会议中心应用服务接口
/// </summary>
public interface ITaktConferenceService
{
    /// <summary>
    /// 获取会议中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktConferenceDto>> GetConferenceListAsync(TaktConferenceQueryDto queryDto);

    /// <summary>
    /// 根据ID获取会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>DTO</returns>
    Task<TaktConferenceDto?> GetConferenceByIdAsync(long id);

    /// <summary>
    /// 获取会议中心主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetConferenceOptionsAsync();

    /// <summary>
    /// 创建会议中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktConferenceDto> CreateConferenceAsync(TaktConferenceCreateDto dto);

    /// <summary>
    /// 更新会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktConferenceDto> UpdateConferenceAsync(long id, TaktConferenceUpdateDto dto);

    /// <summary>
    /// 删除会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>任务</returns>
    Task DeleteConferenceByIdAsync(long id);

    /// <summary>
    /// 批量删除会议中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteConferenceBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新会议中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktConferenceDto> UpdateConferenceStatusAsync(TaktConferenceStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetConferenceTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入会议中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportConferenceAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出会议中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportConferenceAsync(TaktConferenceQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
