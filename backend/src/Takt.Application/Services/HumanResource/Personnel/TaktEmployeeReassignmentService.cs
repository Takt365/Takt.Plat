// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeReassignmentService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：员工调动应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 员工调动应用服务
/// </summary>
public class TaktEmployeeReassignmentService : TaktServiceBase, ITaktEmployeeReassignmentService
{
    private readonly ITaktApprovalRepository<TaktEmployeeReassignment> _employeeReassignmentRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeReassignmentRepository">员工调动仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeReassignmentService(
        ITaktApprovalRepository<TaktEmployeeReassignment> employeeReassignmentRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeReassignmentRepository = employeeReassignmentRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工调动列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeReassignmentDto>> GetEmployeeReassignmentListAsync(TaktEmployeeReassignmentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeReassignmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeReassignmentDto>.Create(
            data.Adapt<List<TaktEmployeeReassignmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeReassignmentDto?> GetEmployeeReassignmentByIdAsync(long id)
    {
        var entity = await _employeeReassignmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeReassignmentDto>();
    }

    /// <summary>
    /// 获取员工调动选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeReassignmentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeReassignmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.FromDeptName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FromDeptName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建员工调动
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeReassignmentDto> CreateEmployeeReassignmentAsync(TaktEmployeeReassignmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeReassignment>();
        entity = await _employeeReassignmentRepository.CreateAsync(entity);
        return await GetEmployeeReassignmentByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeReassignmentDto>();
    }

    /// <summary>
    /// 更新员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeReassignmentDto> UpdateEmployeeReassignmentAsync(long id, TaktEmployeeReassignmentUpdateDto dto)
    {
        var entity = await _employeeReassignmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工调动不存在");
        }
        dto.Adapt(entity);
        await _employeeReassignmentRepository.UpdateAsync(entity);
        return await GetEmployeeReassignmentByIdAsync(id) ?? throw new TaktBusinessException("员工调动不存在");
    }

    /// <summary>
    /// 删除员工调动
    /// </summary>
    /// <param name="id">员工调动ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeReassignmentByIdAsync(long id)
    {
        var deleted = await _employeeReassignmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工调动不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工调动
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeReassignmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeReassignmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeReassignmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeReassignmentTemplateDto>(
            sheetName ?? "员工调动导入模板",
            fileName ?? "员工调动导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工调动
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeReassignmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeReassignmentImportDto>(fileStream, sheetName ?? "员工调动导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeReassignment>();
                await _employeeReassignmentRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出员工调动
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeReassignmentAsync(TaktEmployeeReassignmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeReassignmentQueryDto());
        var list = await _employeeReassignmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeReassignmentExportDto>(),
                sheetName ?? "员工调动数据",
                fileName ?? "员工调动导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeReassignmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工调动数据",
            fileName ?? "员工调动导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工调动查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeReassignment, bool>> QueryExpression(TaktEmployeeReassignmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeReassignment>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || SqlFunc.ToString(x.ReassignmentType).Contains(keywords)
                || SqlFunc.ToString(x.FromDeptId).Contains(keywords)
                || (x.FromDeptName != null && x.FromDeptName.Contains(keywords))
                || SqlFunc.ToString(x.FromPostId).Contains(keywords)
                || (x.FromPostName != null && x.FromPostName.Contains(keywords))
                || SqlFunc.ToString(x.ToDeptId).Contains(keywords)
                || (x.ToDeptName != null && x.ToDeptName.Contains(keywords))
                || SqlFunc.ToString(x.ToPostId).Contains(keywords)
                || (x.ToPostName != null && x.ToPostName.Contains(keywords))
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EffectiveDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (queryDto?.ReassignmentType.HasValue == true)
        {
            exp = exp.And(x => x.ReassignmentType == queryDto.ReassignmentType);
        }

        if (queryDto?.FromDeptId.HasValue == true)
        {
            exp = exp.And(x => x.FromDeptId == queryDto.FromDeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.FromDeptName))
        {
            exp = exp.And(x => x.FromDeptName != null && x.FromDeptName.Contains(queryDto.FromDeptName));
        }

        if (queryDto?.FromPostId.HasValue == true)
        {
            exp = exp.And(x => x.FromPostId == queryDto.FromPostId);
        }

        if (!string.IsNullOrEmpty(queryDto?.FromPostName))
        {
            exp = exp.And(x => x.FromPostName != null && x.FromPostName.Contains(queryDto.FromPostName));
        }

        if (queryDto?.ToDeptId.HasValue == true)
        {
            exp = exp.And(x => x.ToDeptId == queryDto.ToDeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ToDeptName))
        {
            exp = exp.And(x => x.ToDeptName != null && x.ToDeptName.Contains(queryDto.ToDeptName));
        }

        if (queryDto?.ToPostId.HasValue == true)
        {
            exp = exp.And(x => x.ToPostId == queryDto.ToPostId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ToPostName))
        {
            exp = exp.And(x => x.ToPostName != null && x.ToPostName.Contains(queryDto.ToPostName));
        }

        if (!string.IsNullOrEmpty(queryDto?.Reason))
        {
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(queryDto.Reason));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate >= queryDto.EffectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate <= queryDto.EffectiveDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
