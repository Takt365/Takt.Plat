// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：ITaktEmployeeOnboardingTodoService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：入职待办应用服务接口
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 入职待办应用服务接口
/// </summary>
public interface ITaktEmployeeOnboardingTodoService
{
    /// <summary>
    /// 获取入职待办列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktEmployeeOnboardingTodoDto>> GetEmployeeOnboardingTodoListAsync(TaktEmployeeOnboardingTodoQueryDto queryDto);

    /// <summary>
    /// 根据ID获取入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <returns>DTO</returns>
    Task<TaktEmployeeOnboardingTodoDto?> GetEmployeeOnboardingTodoByIdAsync(long id);

    /// <summary>
    /// 获取入职待办选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    Task<List<TaktSelectOption>> GetEmployeeOnboardingTodoOptionsAsync();

    /// <summary>
    /// 创建入职待办
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEmployeeOnboardingTodoDto> CreateEmployeeOnboardingTodoAsync(TaktEmployeeOnboardingTodoCreateDto dto);

    /// <summary>
    /// 更新入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEmployeeOnboardingTodoDto> UpdateEmployeeOnboardingTodoAsync(long id, TaktEmployeeOnboardingTodoUpdateDto dto);

    /// <summary>
    /// 删除入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <returns>任务</returns>
    Task DeleteEmployeeOnboardingTodoByIdAsync(long id);

    /// <summary>
    /// 批量删除入职待办
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    Task DeleteEmployeeOnboardingTodoBatchAsync(IEnumerable<long> ids);

    /// <summary>
    /// 更新入职待办状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    Task<TaktEmployeeOnboardingTodoDto> UpdateEmployeeOnboardingTodoStatusAsync(TaktEmployeeOnboardingTodoStatusDto dto);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] content)> GetEmployeeOnboardingTodoTemplateAsync(string? sheetName = null, string? fileName = null);

    /// <summary>
    /// 导入入职待办
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    Task<(int success, int fail, List<string> errors)> ImportEmployeeOnboardingTodoAsync(Stream fileStream, string? sheetName = null);

    /// <summary>
    /// 导出入职待办
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    Task<(string fileName, byte[] fileContent)> ExportEmployeeOnboardingTodoAsync(TaktEmployeeOnboardingTodoQueryDto? query = null, string? sheetName = null, string? fileName = null);

}
