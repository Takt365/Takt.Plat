// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktSettingService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：系统设置应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 系统设置应用服务接口
/// </summary>
public interface ITaktSettingService
{
    /// <summary>
    /// 获取系统设置列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSettingDto>> GetSettingListAsync(TaktSettingQueryDto queryDto);

    /// <summary>
    /// 根据ID获取系统设置
    /// </summary>
    /// <param name="id">系统设置ID</param>
    /// <returns>DTO</returns>
    Task<TaktSettingDto?> GetSettingByIdAsync(long id);

    /// <summary>
    /// 获取系统设置选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSettingOptionsAsync();

    /// <summary>
    /// 创建系统设置
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSettingDto> CreateSettingAsync(TaktSettingCreateDto dto);

    /// <summary>
    /// 更新系统设置
    /// </summary>
    /// <param name="id">系统设置ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSettingDto> UpdateSettingAsync(long id, TaktSettingUpdateDto dto);

    /// <summary>
    /// 删除系统设置
    /// </summary>
    /// <param name="id">系统设置ID</param>
    /// <returns>任务</returns>
    Task DeleteSettingByIdAsync(long id);

    /// <summary>
    /// 批量删除系统设置
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSettingBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新系统设置排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSettingDto> UpdateSettingSortAsync(TaktSettingSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSettingTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入系统设置
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSettingAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出系统设置
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSettingAsync(TaktSettingQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
