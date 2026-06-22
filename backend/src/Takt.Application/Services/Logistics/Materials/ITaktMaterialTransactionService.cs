// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktMaterialTransactionService.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：物料交易应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料交易应用服务接口
/// </summary>
public interface ITaktMaterialTransactionService
{
    /// <summary>
    /// 获取物料交易列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktMaterialTransactionDto>> GetMaterialTransactionListAsync(TaktMaterialTransactionQueryDto queryDto);

    /// <summary>
    /// 根据ID获取物料交易
    /// </summary>
    /// <param name="id">物料交易ID</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialTransactionDto?> GetMaterialTransactionByIdAsync(long id);

    /// <summary>
    /// 获取物料交易选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetMaterialTransactionOptionsAsync();

    /// <summary>
    /// 创建物料交易
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialTransactionDto> CreateMaterialTransactionAsync(TaktMaterialTransactionCreateDto dto);

    /// <summary>
    /// 更新物料交易
    /// </summary>
    /// <param name="id">物料交易ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialTransactionDto> UpdateMaterialTransactionAsync(long id, TaktMaterialTransactionUpdateDto dto);

    /// <summary>
    /// 删除物料交易
    /// </summary>
    /// <param name="id">物料交易ID</param>
    /// <returns>任务</returns>
    Task DeleteMaterialTransactionByIdAsync(long id);

    /// <summary>
    /// 批量删除物料交易
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteMaterialTransactionBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新物料交易状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktMaterialTransactionDto> UpdateMaterialTransactionStatusAsync(TaktMaterialTransactionStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetMaterialTransactionTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入物料交易
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportMaterialTransactionAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出物料交易
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportMaterialTransactionAsync(TaktMaterialTransactionQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
