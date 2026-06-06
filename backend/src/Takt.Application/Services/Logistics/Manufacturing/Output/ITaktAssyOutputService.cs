// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：ITaktAssyOutputService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报应用服务接口
/// </summary>
public interface ITaktAssyOutputService
{
    /// <summary>
    /// 获取组立日报列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktAssyOutputDto>> GetAssyOutputListAsync(TaktAssyOutputQueryDto queryDto);

    /// <summary>
    /// 根据ID获取组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <returns>DTO</returns>
    Task<TaktAssyOutputDto?> GetAssyOutputByIdAsync(long id);

    /// <summary>
    /// 获取组立日报选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetAssyOutputOptionsAsync();

    /// <summary>
    /// 创建组立日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAssyOutputDto> CreateAssyOutputAsync(TaktAssyOutputCreateDto dto);

    /// <summary>
    /// 更新组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAssyOutputDto> UpdateAssyOutputAsync(long id, TaktAssyOutputUpdateDto dto);

    /// <summary>
    /// 删除组立日报
    /// </summary>
    /// <param name="id">组立日报ID</param>
    /// <returns>任务</returns>
    Task DeleteAssyOutputByIdAsync(long id);

    /// <summary>
    /// 批量删除组立日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteAssyOutputBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新组立日报状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktAssyOutputDto> UpdateAssyOutputStatusAsync(TaktAssyOutputStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetAssyOutputTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入组立日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportAssyOutputAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出组立日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportAssyOutputAsync(TaktAssyOutputQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
