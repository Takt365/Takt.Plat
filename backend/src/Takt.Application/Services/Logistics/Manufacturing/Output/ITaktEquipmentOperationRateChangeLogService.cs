// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：ITaktEquipmentOperationRateChangeLogService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：机器稼动率变更记录应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 机器稼动率变更记录应用服务接口
/// </summary>
public interface ITaktEquipmentOperationRateChangeLogService
{
    /// <summary>
    /// 获取机器稼动率变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEquipmentOperationRateChangeLogDto>> GetEquipmentOperationRateChangeLogListAsync(TaktEquipmentOperationRateChangeLogQueryDto queryDto);

    /// <summary>
    /// 根据ID获取机器稼动率变更记录
    /// </summary>
    /// <param name="id">机器稼动率变更记录ID</param>
    /// <returns>DTO</returns>
    Task<TaktEquipmentOperationRateChangeLogDto?> GetEquipmentOperationRateChangeLogByIdAsync(long id);

    /// <summary>
    /// 获取机器稼动率变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEquipmentOperationRateChangeLogOptionsAsync();

    /// <summary>
    /// 创建机器稼动率变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEquipmentOperationRateChangeLogDto> CreateEquipmentOperationRateChangeLogAsync(TaktEquipmentOperationRateChangeLogCreateDto dto);

    /// <summary>
    /// 更新机器稼动率变更记录
    /// </summary>
    /// <param name="id">机器稼动率变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEquipmentOperationRateChangeLogDto> UpdateEquipmentOperationRateChangeLogAsync(long id, TaktEquipmentOperationRateChangeLogUpdateDto dto);

    /// <summary>
    /// 删除机器稼动率变更记录
    /// </summary>
    /// <param name="id">机器稼动率变更记录ID</param>
    /// <returns>任务</returns>
    Task DeleteEquipmentOperationRateChangeLogByIdAsync(long id);

    /// <summary>
    /// 批量删除机器稼动率变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEquipmentOperationRateChangeLogBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 导出机器稼动率变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEquipmentOperationRateChangeLogAsync(TaktEquipmentOperationRateChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
