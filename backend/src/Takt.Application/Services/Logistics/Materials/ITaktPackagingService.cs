// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：ITaktPackagingService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：物料包装信息应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Materials;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料包装信息应用服务接口
/// </summary>
public interface ITaktPackagingService
{
    /// <summary>
    /// 获取物料包装信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktPackagingDto>> GetPackagingListAsync(TaktPackagingQueryDto queryDto);

    /// <summary>
    /// 根据ID获取物料包装信息
    /// </summary>
    /// <param name="id">物料包装信息ID</param>
    /// <returns>DTO</returns>
    Task<TaktPackagingDto?> GetPackagingByIdAsync(long id);

    /// <summary>
    /// 获取物料包装信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetPackagingOptionsAsync();

    /// <summary>
    /// 创建物料包装信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPackagingDto> CreatePackagingAsync(TaktPackagingCreateDto dto);

    /// <summary>
    /// 更新物料包装信息
    /// </summary>
    /// <param name="id">物料包装信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPackagingDto> UpdatePackagingAsync(long id, TaktPackagingUpdateDto dto);

    /// <summary>
    /// 删除物料包装信息
    /// </summary>
    /// <param name="id">物料包装信息ID</param>
    /// <returns>任务</returns>
    Task DeletePackagingByIdAsync(long id);

    /// <summary>
    /// 批量删除物料包装信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeletePackagingBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新物料包装信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktPackagingDto> UpdatePackagingSortAsync(TaktPackagingSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetPackagingTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入物料包装信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportPackagingAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出物料包装信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportPackagingAsync(TaktPackagingQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
