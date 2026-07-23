// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSalesPriceScaleQuantityService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格数量等级应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格数量等级应用服务接口
/// </summary>
public interface ITaktSalesPriceScaleQuantityService
{
    /// <summary>
    /// 获取销售价格数量等级列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSalesPriceScaleQuantityDto>> GetSalesPriceScaleQuantityListAsync(TaktSalesPriceScaleQuantityQueryDto queryDto);

    /// <summary>
    /// 根据ID获取销售价格数量等级
    /// </summary>
    /// <param name="id">销售价格数量等级ID</param>
    /// <returns>DTO</returns>
    Task<TaktSalesPriceScaleQuantityDto?> GetSalesPriceScaleQuantityByIdAsync(long id);

    /// <summary>
    /// 获取销售价格数量等级选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesPriceScaleQuantityOptionsAsync();

    /// <summary>
    /// 创建销售价格数量等级
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesPriceScaleQuantityDto> CreateSalesPriceScaleQuantityAsync(TaktSalesPriceScaleQuantityCreateDto dto);

    /// <summary>
    /// 更新销售价格数量等级
    /// </summary>
    /// <param name="id">销售价格数量等级ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesPriceScaleQuantityDto> UpdateSalesPriceScaleQuantityAsync(long id, TaktSalesPriceScaleQuantityUpdateDto dto);

    /// <summary>
    /// 删除销售价格数量等级
    /// </summary>
    /// <param name="id">销售价格数量等级ID</param>
    /// <returns>任务</returns>
    Task DeleteSalesPriceScaleQuantityByIdAsync(long id);

    /// <summary>
    /// 批量删除销售价格数量等级
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSalesPriceScaleQuantityBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新销售价格数量等级作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesPriceScaleQuantityDto> UpdateSalesPriceScaleQuantityObsoleteAsync(TaktSalesPriceScaleQuantityObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSalesPriceScaleQuantityTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入销售价格数量等级
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSalesPriceScaleQuantityAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出销售价格数量等级
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalesPriceScaleQuantityAsync(TaktSalesPriceScaleQuantityQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
