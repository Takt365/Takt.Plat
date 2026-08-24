// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：ITaktPurchasePriceScaleQuantityService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格数量等级应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价格数量等级应用服务接口
/// </summary>
public interface ITaktPurchasePriceScaleQuantityService
{
    /// <summary>
    /// 获取采购价格数量等级列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktPurchasePriceScaleQuantityDto>> GetPurchasePriceScaleQuantityListAsync(TaktPurchasePriceScaleQuantityQueryDto queryDto);

    /// <summary>
    /// 根据ID获取采购价格数量等级
    /// </summary>
    /// <param name="id">采购价格数量等级ID</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceScaleQuantityDto?> GetPurchasePriceScaleQuantityByIdAsync(long id);

    /// <summary>
    /// 获取采购价格数量等级选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchasePriceScaleQuantityOptionsAsync();

    /// <summary>
    /// 创建采购价格数量等级
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceScaleQuantityDto> CreatePurchasePriceScaleQuantityAsync(TaktPurchasePriceScaleQuantityCreateDto dto);

    /// <summary>
    /// 更新采购价格数量等级
    /// </summary>
    /// <param name="id">采购价格数量等级ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceScaleQuantityDto> UpdatePurchasePriceScaleQuantityAsync(long id, TaktPurchasePriceScaleQuantityUpdateDto dto);

    /// <summary>
    /// 删除采购价格数量等级
    /// </summary>
    /// <param name="id">采购价格数量等级ID</param>
    /// <returns>任务</returns>
    Task DeletePurchasePriceScaleQuantityByIdAsync(long id);

    /// <summary>
    /// 批量删除采购价格数量等级
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeletePurchasePriceScaleQuantityBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新采购价格数量等级作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceScaleQuantityDto> UpdatePurchasePriceScaleQuantityObsoleteAsync(TaktPurchasePriceScaleQuantityObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetPurchasePriceScaleQuantityTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入采购价格数量等级
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportPurchasePriceScaleQuantityAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出采购价格数量等级
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPurchasePriceScaleQuantityAsync(TaktPurchasePriceScaleQuantityQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
