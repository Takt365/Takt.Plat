// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Statistics.Report
// 文件名称：ITaktConfigurableService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：自定义报表主应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Statistics.Report;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Statistics.Report;

/// <summary>
/// 自定义报表主应用服务接口
/// </summary>
public interface ITaktConfigurableService
{
    /// <summary>
    /// 获取自定义报表主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktConfigurableDto>> GetConfigurableListAsync(TaktConfigurableQueryDto queryDto);

    /// <summary>
    /// 根据ID获取自定义报表主
    /// </summary>
    /// <param name="id">自定义报表主ID</param>
    /// <returns>DTO</returns>
    Task<TaktConfigurableDto?> GetConfigurableByIdAsync(long id);

    /// <summary>
    /// 获取报表下拉选项
    /// </summary>
    Task<List<TaktSelectOption>> GetConfigurableOptionsAsync();

    /// <summary>
    /// 创建自定义报表主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktConfigurableDto> CreateConfigurableAsync(TaktConfigurableCreateDto dto);

    /// <summary>
    /// 更新自定义报表主
    /// </summary>
    /// <param name="id">自定义报表主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktConfigurableDto> UpdateConfigurableAsync(long id, TaktConfigurableUpdateDto dto);

    /// <summary>
    /// 删除自定义报表主
    /// </summary>
    /// <param name="id">自定义报表主ID</param>
    /// <returns>任务</returns>
    Task DeleteConfigurableByIdAsync(long id);

    /// <summary>
    /// 批量删除自定义报表主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteConfigurableBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新自定义报表主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktConfigurableDto> UpdateConfigurableStatusAsync(TaktConfigurableStatusDto dto);

    /// <summary>
    /// 更新自定义报表主排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktConfigurableDto> UpdateConfigurableSortAsync(TaktConfigurableSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetConfigurableTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入自定义报表主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportConfigurableAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出自定义报表主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportConfigurableAsync(TaktConfigurableQueryDto? query = null, string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 获取 SQVI 运行时筛选条件定义
    /// </summary>
    /// <param name="id">报表主键</param>
    /// <returns>运行时屏幕 DTO</returns>
    Task<TaktConfigurableRuntimeScreenDto> GetConfigurableRuntimeScreenAsync(long id);

    /// <summary>
    /// 执行报表查询（分页）
    /// </summary>
    /// <param name="id">报表主键</param>
    /// <param name="dto">查询参数与筛选值</param>
    /// <returns>查询结果</returns>
    Task<TaktConfigurableQueryResultDto> ExecuteConfigurableQueryAsync(long id, TaktConfigurableExecuteQueryDto dto);

    /// <summary>
    /// 设计态预览查询（未保存报表定义）
    /// </summary>
    /// <param name="dto">报表定义与分页参数</param>
    /// <returns>查询结果</returns>
    Task<TaktConfigurableQueryResultDto> PreviewConfigurableQueryAsync(TaktConfigurablePreviewQueryDto dto);

    /// <summary>
    /// 导出报表数据（Excel）
    /// </summary>
    /// <param name="id">报表主键</param>
    /// <param name="dto">筛选值</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名（不含扩展名）</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> ExportConfigurableDataAsync(
        long id,
        TaktConfigurableExportDataDto dto,
        string? sheetName = null,
        string? fileName = null);
}
