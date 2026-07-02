// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSalesQuotationChangeLogService.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：销售报价变更记录应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售报价变更记录应用服务接口
/// </summary>
public interface ITaktSalesQuotationChangeLogService
{
    /// <summary>
    /// 获取销售报价变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSalesQuotationChangeLogDto>> GetSalesQuotationChangeLogListAsync(TaktSalesQuotationChangeLogQueryDto queryDto);

    /// <summary>
    /// 根据ID获取销售报价变更记录
    /// </summary>
    /// <param name="id">销售报价变更记录ID</param>
    /// <returns>DTO</returns>
    Task<TaktSalesQuotationChangeLogDto?> GetSalesQuotationChangeLogByIdAsync(long id);

    /// <summary>
    /// 获取销售报价变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesQuotationChangeLogOptionsAsync();

    /// <summary>
    /// 创建销售报价变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesQuotationChangeLogDto> CreateSalesQuotationChangeLogAsync(TaktSalesQuotationChangeLogCreateDto dto);

    /// <summary>
    /// 更新销售报价变更记录
    /// </summary>
    /// <param name="id">销售报价变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesQuotationChangeLogDto> UpdateSalesQuotationChangeLogAsync(long id, TaktSalesQuotationChangeLogUpdateDto dto);

    /// <summary>
    /// 删除销售报价变更记录
    /// </summary>
    /// <param name="id">销售报价变更记录ID</param>
    /// <returns>任务</returns>
    Task DeleteSalesQuotationChangeLogByIdAsync(long id);

    /// <summary>
    /// 批量删除销售报价变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSalesQuotationChangeLogBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 导出销售报价变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalesQuotationChangeLogAsync(TaktSalesQuotationChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
