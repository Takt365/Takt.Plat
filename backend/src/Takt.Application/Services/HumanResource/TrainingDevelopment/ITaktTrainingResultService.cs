// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.TrainingDevelopment
// 文件名称：ITaktTrainingResultService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：培训结果应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.TrainingDevelopment;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.TrainingDevelopment;

/// <summary>
/// 培训结果应用服务接口
/// </summary>
public interface ITaktTrainingResultService
{
    /// <summary>
    /// 获取培训结果列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktTrainingResultDto>> GetTrainingResultListAsync(TaktTrainingResultQueryDto queryDto);

    /// <summary>
    /// 根据ID获取培训结果
    /// </summary>
    /// <param name="id">培训结果ID</param>
    /// <returns>DTO</returns>
    Task<TaktTrainingResultDto?> GetTrainingResultByIdAsync(long id);

    /// <summary>
    /// 获取培训结果选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetTrainingResultOptionsAsync();

    /// <summary>
    /// 创建培训结果
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTrainingResultDto> CreateTrainingResultAsync(TaktTrainingResultCreateDto dto);

    /// <summary>
    /// 更新培训结果
    /// </summary>
    /// <param name="id">培训结果ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTrainingResultDto> UpdateTrainingResultAsync(long id, TaktTrainingResultUpdateDto dto);

    /// <summary>
    /// 删除培训结果
    /// </summary>
    /// <param name="id">培训结果ID</param>
    /// <returns>任务</returns>
    Task DeleteTrainingResultByIdAsync(long id);

    /// <summary>
    /// 批量删除培训结果
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteTrainingResultBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新培训结果状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTrainingResultDto> UpdateTrainingResultStatusAsync(TaktTrainingResultStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetTrainingResultTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入培训结果
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportTrainingResultAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出培训结果
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportTrainingResultAsync(TaktTrainingResultQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
