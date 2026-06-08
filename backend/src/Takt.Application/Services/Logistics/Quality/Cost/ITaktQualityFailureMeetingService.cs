// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：ITaktQualityFailureMeetingService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：质量问题会议调查试验费用明细应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 质量问题会议调查试验费用明细应用服务接口
/// </summary>
public interface ITaktQualityFailureMeetingService
{
    /// <summary>
    /// 获取质量问题会议调查试验费用明细列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktQualityFailureMeetingDto>> GetQualityFailureMeetingListAsync(TaktQualityFailureMeetingQueryDto queryDto);

    /// <summary>
    /// 根据ID获取质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <returns>DTO</returns>
    Task<TaktQualityFailureMeetingDto?> GetQualityFailureMeetingByIdAsync(long id);

    /// <summary>
    /// 获取质量问题会议调查试验费用明细选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetQualityFailureMeetingOptionsAsync();

    /// <summary>
    /// 创建质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktQualityFailureMeetingDto> CreateQualityFailureMeetingAsync(TaktQualityFailureMeetingCreateDto dto);

    /// <summary>
    /// 更新质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktQualityFailureMeetingDto> UpdateQualityFailureMeetingAsync(long id, TaktQualityFailureMeetingUpdateDto dto);

    /// <summary>
    /// 删除质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <returns>任务</returns>
    Task DeleteQualityFailureMeetingByIdAsync(long id);

    /// <summary>
    /// 批量删除质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteQualityFailureMeetingBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetQualityFailureMeetingTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportQualityFailureMeetingAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportQualityFailureMeetingAsync(TaktQualityFailureMeetingQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
