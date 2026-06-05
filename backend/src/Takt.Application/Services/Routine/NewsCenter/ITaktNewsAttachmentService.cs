// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：ITaktNewsAttachmentService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心附件应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Routine.NewsCenter;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.NewsCenter;

/// <summary>
/// 新闻中心附件应用服务接口
/// </summary>
public interface ITaktNewsAttachmentService
{
    /// <summary>
    /// 获取新闻中心附件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktNewsAttachmentDto>> GetNewsAttachmentListAsync(TaktNewsAttachmentQueryDto queryDto);

    /// <summary>
    /// 根据ID获取新闻中心附件
    /// </summary>
    /// <param name="id">新闻中心附件ID</param>
    /// <returns>DTO</returns>
    Task<TaktNewsAttachmentDto?> GetNewsAttachmentByIdAsync(long id);

    /// <summary>
    /// 获取新闻附件选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetNewsAttachmentOptionsAsync();

    /// <summary>
    /// 创建新闻中心附件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktNewsAttachmentDto> CreateNewsAttachmentAsync(TaktNewsAttachmentCreateDto dto);

    /// <summary>
    /// 更新新闻中心附件
    /// </summary>
    /// <param name="id">新闻中心附件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktNewsAttachmentDto> UpdateNewsAttachmentAsync(long id, TaktNewsAttachmentUpdateDto dto);

    /// <summary>
    /// 删除新闻中心附件
    /// </summary>
    /// <param name="id">新闻中心附件ID</param>
    /// <returns>任务</returns>
    Task DeleteNewsAttachmentByIdAsync(long id);

    /// <summary>
    /// 批量删除新闻中心附件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteNewsAttachmentBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新新闻中心附件排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktNewsAttachmentDto> UpdateNewsAttachmentSortAsync(TaktNewsAttachmentSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetNewsAttachmentTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入新闻中心附件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportNewsAttachmentAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出新闻中心附件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportNewsAttachmentAsync(TaktNewsAttachmentQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
