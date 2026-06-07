// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.CompensationBenefits
// 文件名称：ITaktTaxCalcService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：个税计算规则应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.CompensationBenefits;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.CompensationBenefits;

/// <summary>
/// 个税计算规则应用服务接口
/// </summary>
public interface ITaktTaxCalcService
{
    /// <summary>
    /// 获取个税计算规则列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktTaxCalcDto>> GetTaxCalcListAsync(TaktTaxCalcQueryDto queryDto);

    /// <summary>
    /// 根据ID获取个税计算规则
    /// </summary>
    /// <param name="id">个税计算规则ID</param>
    /// <returns>DTO</returns>
    Task<TaktTaxCalcDto?> GetTaxCalcByIdAsync(long id);

    /// <summary>
    /// 获取个税计算规则选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetTaxCalcOptionsAsync();

    /// <summary>
    /// 创建个税计算规则
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTaxCalcDto> CreateTaxCalcAsync(TaktTaxCalcCreateDto dto);

    /// <summary>
    /// 更新个税计算规则
    /// </summary>
    /// <param name="id">个税计算规则ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTaxCalcDto> UpdateTaxCalcAsync(long id, TaktTaxCalcUpdateDto dto);

    /// <summary>
    /// 删除个税计算规则
    /// </summary>
    /// <param name="id">个税计算规则ID</param>
    /// <returns>任务</returns>
    Task DeleteTaxCalcByIdAsync(long id);

    /// <summary>
    /// 批量删除个税计算规则
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteTaxCalcBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新个税计算规则状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktTaxCalcDto> UpdateTaxCalcStatusAsync(TaktTaxCalcStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetTaxCalcTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入个税计算规则
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportTaxCalcAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出个税计算规则
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportTaxCalcAsync(TaktTaxCalcQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
