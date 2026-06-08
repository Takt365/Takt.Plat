// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktPurchasePriceItemService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格明细应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 采购价格明细应用服务接口
/// </summary>
public interface ITaktPurchasePriceItemService
{
    /// <summary>
    /// 获取采购价格明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktPurchasePriceItemDto>> GetPurchasePriceItemListAsync(TaktPurchasePriceItemQueryDto queryDto);

    /// <summary>
    /// 根据ID获取采购价格明细
    /// </summary>
    /// <param name="id">采购价格明细ID</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceItemDto?> GetPurchasePriceItemByIdAsync(long id);

    /// <summary>
    /// 获取采购价格明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchasePriceItemOptionsAsync();

    /// <summary>
    /// 创建采购价格明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceItemDto> CreatePurchasePriceItemAsync(TaktPurchasePriceItemCreateDto dto);

    /// <summary>
    /// 更新采购价格明细
    /// </summary>
    /// <param name="id">采购价格明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceItemDto> UpdatePurchasePriceItemAsync(long id, TaktPurchasePriceItemUpdateDto dto);

    /// <summary>
    /// 删除采购价格明细
    /// </summary>
    /// <param name="id">采购价格明细ID</param>
    /// <returns>任务</returns>
    Task DeletePurchasePriceItemByIdAsync(long id);

    /// <summary>
    /// 批量删除采购价格明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeletePurchasePriceItemBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新采购价格明细排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceItemDto> UpdatePurchasePriceItemSortAsync(TaktPurchasePriceItemSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetPurchasePriceItemTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入采购价格明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportPurchasePriceItemAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出采购价格明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPurchasePriceItemAsync(TaktPurchasePriceItemQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
