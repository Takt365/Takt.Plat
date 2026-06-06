// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.CompensationBenefits
// 文件名称：ITaktSalaryCalcService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资核算应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.CompensationBenefits;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.CompensationBenefits;

/// <summary>
/// 薪资核算应用服务接口
/// </summary>
public interface ITaktSalaryCalcService
{
    /// <summary>
    /// 获取薪资核算列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktSalaryCalcDto>> GetSalaryCalcListAsync(TaktSalaryCalcQueryDto queryDto);

    /// <summary>
    /// 根据ID获取薪资核算
    /// </summary>
    /// <param name="id">薪资核算ID</param>
    /// <returns>DTO</returns>
    Task<TaktSalaryCalcDto?> GetSalaryCalcByIdAsync(long id);

    /// <summary>
    /// 获取薪资核算选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetSalaryCalcOptionsAsync();

    /// <summary>
    /// 创建薪资核算
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalaryCalcDto> CreateSalaryCalcAsync(TaktSalaryCalcCreateDto dto);

    /// <summary>
    /// 更新薪资核算
    /// </summary>
    /// <param name="id">薪资核算ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalaryCalcDto> UpdateSalaryCalcAsync(long id, TaktSalaryCalcUpdateDto dto);

    /// <summary>
    /// 删除薪资核算
    /// </summary>
    /// <param name="id">薪资核算ID</param>
    /// <returns>任务</returns>
    Task DeleteSalaryCalcByIdAsync(long id);

    /// <summary>
    /// 批量删除薪资核算
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteSalaryCalcBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新薪资核算状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktSalaryCalcDto> UpdateSalaryCalcStatusAsync(TaktSalaryCalcStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetSalaryCalcTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入薪资核算
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportSalaryCalcAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出薪资核算
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportSalaryCalcAsync(TaktSalaryCalcQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
