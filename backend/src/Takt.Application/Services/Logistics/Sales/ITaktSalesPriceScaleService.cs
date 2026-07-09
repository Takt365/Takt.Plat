// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSalesPriceScaleService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格阶梯应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格阶梯应用服务接口
/// </summary>
public interface ITaktSalesPriceScaleService
{
    /// <summary>
    /// 获取销售价格阶梯列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSalesPriceScaleDto>> GetSalesPriceScaleListAsync(TaktSalesPriceScaleQueryDto queryDto);

    /// <summary>
    /// 根据ID获取销售价格阶梯
    /// </summary>
    /// <param name="id">销售价格阶梯ID</param>
    /// <returns>DTO</returns>
    Task<TaktSalesPriceScaleDto?> GetSalesPriceScaleByIdAsync(long id);

    /// <summary>
    /// 获取销售价格阶梯选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalesPriceScaleOptionsAsync();

    /// <summary>
    /// 创建销售价格阶梯
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesPriceScaleDto> CreateSalesPriceScaleAsync(TaktSalesPriceScaleCreateDto dto);

    /// <summary>
    /// 更新销售价格阶梯
    /// </summary>
    /// <param name="id">销售价格阶梯ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesPriceScaleDto> UpdateSalesPriceScaleAsync(long id, TaktSalesPriceScaleUpdateDto dto);

    /// <summary>
    /// 删除销售价格阶梯
    /// </summary>
    /// <param name="id">销售价格阶梯ID</param>
    /// <returns>任务</returns>
    Task DeleteSalesPriceScaleByIdAsync(long id);

    /// <summary>
    /// 批量删除销售价格阶梯
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSalesPriceScaleBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新销售价格阶梯作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalesPriceScaleDto> UpdateSalesPriceScaleObsoleteAsync(TaktSalesPriceScaleObsoleteDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSalesPriceScaleTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入销售价格阶梯
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSalesPriceScaleAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出销售价格阶梯
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalesPriceScaleAsync(TaktSalesPriceScaleQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
