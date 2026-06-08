// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：ITaktAccountTitleChangeLogService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：会计科目变更记录应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Accounting.Financial;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 会计科目变更记录应用服务接口
/// </summary>
public interface ITaktAccountTitleChangeLogService
{
    /// <summary>
    /// 获取会计科目变更记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktAccountTitleChangeLogDto>> GetAccountTitleChangeLogListAsync(TaktAccountTitleChangeLogQueryDto queryDto);

    /// <summary>
    /// 根据ID获取会计科目变更记录
    /// </summary>
    /// <param name="id">会计科目变更记录ID</param>
    /// <returns>DTO</returns>
    Task<TaktAccountTitleChangeLogDto?> GetAccountTitleChangeLogByIdAsync(long id);

    /// <summary>
    /// 获取会计科目变更记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetAccountTitleChangeLogOptionsAsync();

    /// <summary>
    /// 创建会计科目变更记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAccountTitleChangeLogDto> CreateAccountTitleChangeLogAsync(TaktAccountTitleChangeLogCreateDto dto);

    /// <summary>
    /// 更新会计科目变更记录
    /// </summary>
    /// <param name="id">会计科目变更记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAccountTitleChangeLogDto> UpdateAccountTitleChangeLogAsync(long id, TaktAccountTitleChangeLogUpdateDto dto);

    /// <summary>
    /// 删除会计科目变更记录
    /// </summary>
    /// <param name="id">会计科目变更记录ID</param>
    /// <returns>任务</returns>
    Task DeleteAccountTitleChangeLogByIdAsync(long id);

    /// <summary>
    /// 批量删除会计科目变更记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteAccountTitleChangeLogBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 导出会计科目变更记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportAccountTitleChangeLogAsync(TaktAccountTitleChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
