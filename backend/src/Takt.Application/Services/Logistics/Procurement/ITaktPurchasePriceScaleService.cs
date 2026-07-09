// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：ITaktPurchasePriceScaleService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格阶梯应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价格阶梯应用服务接口
/// </summary>
public interface ITaktPurchasePriceScaleService
{
    /// <summary>
    /// 获取采购价格阶梯列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktPurchasePriceScaleDto>> GetPurchasePriceScaleListAsync(TaktPurchasePriceScaleQueryDto queryDto);

    /// <summary>
    /// 根据ID获取采购价格阶梯
    /// </summary>
    /// <param name="id">采购价格阶梯ID</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceScaleDto?> GetPurchasePriceScaleByIdAsync(long id);

    /// <summary>
    /// 获取采购价格阶梯选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPurchasePriceScaleOptionsAsync();

    /// <summary>
    /// 创建采购价格阶梯
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceScaleDto> CreatePurchasePriceScaleAsync(TaktPurchasePriceScaleCreateDto dto);

    /// <summary>
    /// 更新采购价格阶梯
    /// </summary>
    /// <param name="id">采购价格阶梯ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceScaleDto> UpdatePurchasePriceScaleAsync(long id, TaktPurchasePriceScaleUpdateDto dto);

    /// <summary>
    /// 删除采购价格阶梯
    /// </summary>
    /// <param name="id">采购价格阶梯ID</param>
    /// <returns>任务</returns>
    Task DeletePurchasePriceScaleByIdAsync(long id);

    /// <summary>
    /// 批量删除采购价格阶梯
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeletePurchasePriceScaleBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新采购价格阶梯排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceScaleDto> UpdatePurchasePriceScaleSortAsync(TaktPurchasePriceScaleSortDto dto);

    /// <summary>
    /// 更新采购价格阶梯作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPurchasePriceScaleDto> UpdatePurchasePriceScaleObsoleteAsync(TaktPurchasePriceScaleObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetPurchasePriceScaleTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入采购价格阶梯
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportPurchasePriceScaleAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出采购价格阶梯
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPurchasePriceScaleAsync(TaktPurchasePriceScaleQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
