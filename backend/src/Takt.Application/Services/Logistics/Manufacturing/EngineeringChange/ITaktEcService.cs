// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变主应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变主应用服务接口
/// </summary>
public interface ITaktEcService
{
    /// <summary>
    /// 获取设变主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcDto>> GetEcListAsync(TaktEcQueryDto queryDto);

    /// <summary>
    /// 根据ID获取设变主
    /// </summary>
    /// <param name="id">设变主ID</param>
    /// <returns>DTO</returns>
    Task<TaktEcDto?> GetEcByIdAsync(long id);

    /// <summary>
    /// 获取设变主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcOptionsAsync();

    /// <summary>
    /// 创建设变主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcDto> CreateEcAsync(TaktEcCreateDto dto);

    /// <summary>
    /// 更新设变主
    /// </summary>
    /// <param name="id">设变主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcDto> UpdateEcAsync(long id, TaktEcUpdateDto dto);

    /// <summary>
    /// 删除设变主
    /// </summary>
    /// <param name="id">设变主ID</param>
    /// <returns>任务</returns>
    Task DeleteEcByIdAsync(long id);

    /// <summary>
    /// 批量删除设变主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEcBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新设变主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcDto> UpdateEcStatusAsync(TaktEcStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetEcTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入设变主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportEcAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出设变主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcAsync(TaktEcQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
