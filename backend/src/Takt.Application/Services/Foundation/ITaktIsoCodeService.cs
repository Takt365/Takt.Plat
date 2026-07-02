// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktIsoCodeService.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Cursor AI)
// 功能描述：ISO编码应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Foundation;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// ISO编码应用服务接口
/// </summary>
public interface ITaktIsoCodeService
{
    /// <summary>
    /// 获取ISO编码列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktIsoCodeDto>> GetIsoCodeListAsync(TaktIsoCodeQueryDto queryDto);

    /// <summary>
    /// 根据ID获取ISO编码
    /// </summary>
    /// <param name="id">ISO编码ID</param>
    /// <returns>DTO</returns>
    Task<TaktIsoCodeDto?> GetIsoCodeByIdAsync(long id);

    /// <summary>
    /// 获取ISO编码选项列表
    /// </summary>
    /// <param name="isoCodeCategory">编码类别（字典 sys_iso_code_category；为空则不过滤）</param>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetIsoCodeOptionsAsync(int? isoCodeCategory = null);

    /// <summary>
    /// 创建ISO编码
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIsoCodeDto> CreateIsoCodeAsync(TaktIsoCodeCreateDto dto);

    /// <summary>
    /// 更新ISO编码
    /// </summary>
    /// <param name="id">ISO编码ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIsoCodeDto> UpdateIsoCodeAsync(long id, TaktIsoCodeUpdateDto dto);

    /// <summary>
    /// 删除ISO编码
    /// </summary>
    /// <param name="id">ISO编码ID</param>
    /// <returns>任务</returns>
    Task DeleteIsoCodeByIdAsync(long id);

    /// <summary>
    /// 批量删除ISO编码
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteIsoCodeBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新ISO编码状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIsoCodeDto> UpdateIsoCodeStatusAsync(TaktIsoCodeStatusDto dto);

    /// <summary>
    /// 更新ISO编码排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    Task<TaktIsoCodeDto> UpdateIsoCodeSortAsync(TaktIsoCodeSortDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetIsoCodeTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入ISO编码
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportIsoCodeAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出ISO编码
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportIsoCodeAsync(TaktIsoCodeQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
