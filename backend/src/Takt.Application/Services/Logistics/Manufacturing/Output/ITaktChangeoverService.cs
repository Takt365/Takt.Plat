// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：ITaktChangeoverService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：切换记录应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 切换记录应用服务接口
/// </summary>
public interface ITaktChangeoverService
{
    /// <summary>
    /// 获取切换记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktChangeoverDto>> GetChangeoverListAsync(TaktChangeoverQueryDto queryDto);

    /// <summary>
    /// 根据ID获取切换记录
    /// </summary>
    /// <param name="id">切换记录ID</param>
    /// <returns>DTO</returns>
    Task<TaktChangeoverDto?> GetChangeoverByIdAsync(long id);

    /// <summary>
    /// 获取切换记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetChangeoverOptionsAsync();

    /// <summary>
    /// 创建切换记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktChangeoverDto> CreateChangeoverAsync(TaktChangeoverCreateDto dto);

    /// <summary>
    /// 更新切换记录
    /// </summary>
    /// <param name="id">切换记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktChangeoverDto> UpdateChangeoverAsync(long id, TaktChangeoverUpdateDto dto);

    /// <summary>
    /// 删除切换记录
    /// </summary>
    /// <param name="id">切换记录ID</param>
    /// <returns>任务</returns>
    Task DeleteChangeoverByIdAsync(long id);

    /// <summary>
    /// 批量删除切换记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteChangeoverBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetChangeoverTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入切换记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportChangeoverAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出切换记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportChangeoverAsync(TaktChangeoverQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
