// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcDeptService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门应用服务接口
/// </summary>
public interface ITaktEcDeptService
{
    /// <summary>
    /// 获取设变部门列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEcDeptDto>> GetEcDeptListAsync(TaktEcDeptQueryDto queryDto);

    /// <summary>
    /// 根据ID获取设变部门
    /// </summary>
    /// <param name="id">设变部门ID</param>
    /// <returns>DTO</returns>
    Task<TaktEcDeptDto?> GetEcDeptByIdAsync(long id);

    /// <summary>
    /// 获取设变部门选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEcDeptOptionsAsync();

    /// <summary>
    /// 创建设变部门
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcDeptDto> CreateEcDeptAsync(TaktEcDeptCreateDto dto);

    /// <summary>
    /// 更新设变部门
    /// </summary>
    /// <param name="id">设变部门ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEcDeptDto> UpdateEcDeptAsync(long id, TaktEcDeptUpdateDto dto);

    /// <summary>
    /// 删除设变部门
    /// </summary>
    /// <param name="id">设变部门ID</param>
    /// <returns>任务</returns>
    Task DeleteEcDeptByIdAsync(long id);

    /// <summary>
    /// 批量删除设变部门
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEcDeptBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetEcDeptTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入设变部门
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportEcDeptAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出设变部门
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEcDeptAsync(TaktEcDeptQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
