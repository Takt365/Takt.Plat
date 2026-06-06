// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Serial
// 文件名称：ITaktProductSerialInboundItemService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：产品序列号入库明细应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Serial;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Serial;

/// <summary>
/// 产品序列号入库明细应用服务接口
/// </summary>
public interface ITaktProductSerialInboundItemService
{
    /// <summary>
    /// 获取产品序列号入库明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktProductSerialInboundItemDto>> GetProductSerialInboundItemListAsync(TaktProductSerialInboundItemQueryDto queryDto);

    /// <summary>
    /// 根据ID获取产品序列号入库明细
    /// </summary>
    /// <param name="id">产品序列号入库明细ID</param>
    /// <returns>DTO</returns>
    Task<TaktProductSerialInboundItemDto?> GetProductSerialInboundItemByIdAsync(long id);

    /// <summary>
    /// 获取产品序列号入库明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetProductSerialInboundItemOptionsAsync();

    /// <summary>
    /// 创建产品序列号入库明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktProductSerialInboundItemDto> CreateProductSerialInboundItemAsync(TaktProductSerialInboundItemCreateDto dto);

    /// <summary>
    /// 更新产品序列号入库明细
    /// </summary>
    /// <param name="id">产品序列号入库明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktProductSerialInboundItemDto> UpdateProductSerialInboundItemAsync(long id, TaktProductSerialInboundItemUpdateDto dto);

    /// <summary>
    /// 删除产品序列号入库明细
    /// </summary>
    /// <param name="id">产品序列号入库明细ID</param>
    /// <returns>任务</returns>
    Task DeleteProductSerialInboundItemByIdAsync(long id);

    /// <summary>
    /// 批量删除产品序列号入库明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteProductSerialInboundItemBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetProductSerialInboundItemTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入产品序列号入库明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportProductSerialInboundItemAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出产品序列号入库明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportProductSerialInboundItemAsync(TaktProductSerialInboundItemQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
