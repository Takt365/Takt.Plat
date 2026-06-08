// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：ITaktIpqcOrderChangeLogService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：制程检验单变更日志应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 制程检验单变更日志应用服务接口
/// </summary>
public interface ITaktIpqcOrderChangeLogService
{
    /// <summary>
    /// 获取制程检验单变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktIpqcOrderChangeLogDto>> GetIpqcOrderChangeLogListAsync(TaktIpqcOrderChangeLogQueryDto queryDto);

    /// <summary>
    /// 根据ID获取制程检验单变更日志
    /// </summary>
    /// <param name="id">制程检验单变更日志ID</param>
    /// <returns>DTO</returns>
    Task<TaktIpqcOrderChangeLogDto?> GetIpqcOrderChangeLogByIdAsync(long id);

    /// <summary>
    /// 获取制程检验单变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetIpqcOrderChangeLogOptionsAsync();

    /// <summary>
    /// 创建制程检验单变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIpqcOrderChangeLogDto> CreateIpqcOrderChangeLogAsync(TaktIpqcOrderChangeLogCreateDto dto);

    /// <summary>
    /// 更新制程检验单变更日志
    /// </summary>
    /// <param name="id">制程检验单变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIpqcOrderChangeLogDto> UpdateIpqcOrderChangeLogAsync(long id, TaktIpqcOrderChangeLogUpdateDto dto);

    /// <summary>
    /// 删除制程检验单变更日志
    /// </summary>
    /// <param name="id">制程检验单变更日志ID</param>
    /// <returns>任务</returns>
    Task DeleteIpqcOrderChangeLogByIdAsync(long id);

    /// <summary>
    /// 批量删除制程检验单变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteIpqcOrderChangeLogBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 导出制程检验单变更日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportIpqcOrderChangeLogAsync(TaktIpqcOrderChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
