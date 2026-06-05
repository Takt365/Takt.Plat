// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：ITaktEmployeeTransferService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：员工调动应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 员工调动应用服务接口
/// </summary>
public interface ITaktEmployeeTransferService
{
    /// <summary>
    /// 获取员工调动列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEmployeeTransferDto>> GetEmployeeTransferListAsync(TaktEmployeeTransferQueryDto queryDto);

    /// <summary>
    /// 根据ID获取员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <returns>DTO</returns>
    Task<TaktEmployeeTransferDto?> GetEmployeeTransferByIdAsync(long id);

    /// <summary>
    /// 获取员工调动选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEmployeeTransferOptionsAsync();

    /// <summary>
    /// 创建员工调动
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEmployeeTransferDto> CreateEmployeeTransferAsync(TaktEmployeeTransferCreateDto dto);

    /// <summary>
    /// 更新员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEmployeeTransferDto> UpdateEmployeeTransferAsync(long id, TaktEmployeeTransferUpdateDto dto);

    /// <summary>
    /// 删除员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <returns>任务</returns>
    Task DeleteEmployeeTransferByIdAsync(long id);

    /// <summary>
    /// 批量删除员工调动
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEmployeeTransferBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetEmployeeTransferTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入员工调动
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportEmployeeTransferAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出员工调动
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEmployeeTransferAsync(TaktEmployeeTransferQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
