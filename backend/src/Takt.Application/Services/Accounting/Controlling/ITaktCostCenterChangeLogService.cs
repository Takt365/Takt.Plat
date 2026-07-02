// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Controlling
// 文件名称：ITaktCostCenterChangeLogService.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：成本中心变更记录应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Controlling;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Controlling;

/// <summary>
/// 成本中心变更记录应用服务接口
/// </summary>
public interface ITaktCostCenterChangeLogService
{
    /// <summary>
    /// 获取成本中心变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktCostCenterChangeLogDto>> GetCostCenterChangeLogListAsync(TaktCostCenterChangeLogQueryDto queryDto);

    /// <summary>
    /// 根据ID获取成本中心变更记录
    /// </summary>
    /// <param name="id">成本中心变更记录ID</param>
    /// <returns>DTO</returns>
    Task<TaktCostCenterChangeLogDto?> GetCostCenterChangeLogByIdAsync(long id);

    /// <summary>
    /// 获取成本中心变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetCostCenterChangeLogOptionsAsync();

    /// <summary>
    /// 创建成本中心变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCostCenterChangeLogDto> CreateCostCenterChangeLogAsync(TaktCostCenterChangeLogCreateDto dto);

    /// <summary>
    /// 更新成本中心变更记录
    /// </summary>
    /// <param name="id">成本中心变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktCostCenterChangeLogDto> UpdateCostCenterChangeLogAsync(long id, TaktCostCenterChangeLogUpdateDto dto);

    /// <summary>
    /// 删除成本中心变更记录
    /// </summary>
    /// <param name="id">成本中心变更记录ID</param>
    /// <returns>任务</returns>
    Task DeleteCostCenterChangeLogByIdAsync(long id);

    /// <summary>
    /// 批量删除成本中心变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteCostCenterChangeLogBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 导出成本中心变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportCostCenterChangeLogAsync(TaktCostCenterChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
