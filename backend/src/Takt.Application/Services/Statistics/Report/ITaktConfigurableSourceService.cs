// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Statistics.Report
// 文件名称：ITaktConfigurableSourceService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：自定义报表数据源应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Statistics.Report;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.Report;

/// <summary>
/// 自定义报表数据源应用服务接口
/// </summary>
public interface ITaktConfigurableSourceService
{
    /// <summary>
    /// 获取自定义报表数据源列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktConfigurableSourceDto>> GetConfigurableSourceListAsync(TaktConfigurableSourceQueryDto queryDto);

    /// <summary>
    /// 根据ID获取自定义报表数据源
    /// </summary>
    /// <param name="id">自定义报表数据源ID</param>
    /// <returns>DTO</returns>
    Task<TaktConfigurableSourceDto?> GetConfigurableSourceByIdAsync(long id);

    /// <summary>
    /// 获取自定义报表数据源选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetConfigurableSourceOptionsAsync();

    /// <summary>
    /// 创建自定义报表数据源
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktConfigurableSourceDto> CreateConfigurableSourceAsync(TaktConfigurableSourceCreateDto dto);

    /// <summary>
    /// 更新自定义报表数据源
    /// </summary>
    /// <param name="id">自定义报表数据源ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktConfigurableSourceDto> UpdateConfigurableSourceAsync(long id, TaktConfigurableSourceUpdateDto dto);

    /// <summary>
    /// 删除自定义报表数据源
    /// </summary>
    /// <param name="id">自定义报表数据源ID</param>
    /// <returns>任务</returns>
    Task DeleteConfigurableSourceByIdAsync(long id);

    /// <summary>
    /// 批量删除自定义报表数据源
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteConfigurableSourceBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新自定义报表数据源排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktConfigurableSourceDto> UpdateConfigurableSourceSortAsync(TaktConfigurableSourceSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetConfigurableSourceTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入自定义报表数据源
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportConfigurableSourceAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出自定义报表数据源
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportConfigurableSourceAsync(TaktConfigurableSourceQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
