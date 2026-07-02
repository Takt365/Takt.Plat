// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeFamilyService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：员工家庭成员应用服务实现
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
/// 员工家庭成员应用服务
/// </summary>
public class TaktEmployeeFamilyService : TaktServiceBase, ITaktEmployeeFamilyService
{
    private readonly ITaktCompanyRepository<TaktEmployeeFamily> _employeeFamilyRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeFamilyRepository">员工家庭成员仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeFamilyService(
        ITaktCompanyRepository<TaktEmployeeFamily> employeeFamilyRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeFamilyRepository = employeeFamilyRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工家庭成员列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeFamilyDto>> GetEmployeeFamilyListAsync(TaktEmployeeFamilyQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeFamilyRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeFamilyDto>.Create(
            data.Adapt<List<TaktEmployeeFamilyDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工家庭成员
    /// </summary>
    /// <param name="id">员工家庭成员ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeFamilyDto?> GetEmployeeFamilyByIdAsync(long id)
    {
        var entity = await _employeeFamilyRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeFamilyDto>();
    }

    /// <summary>
    /// 获取员工家庭成员选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeFamilyOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeFamilyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MemberName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.MemberName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建员工家庭成员
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeFamilyDto> CreateEmployeeFamilyAsync(TaktEmployeeFamilyCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeFamily>();
        entity = await _employeeFamilyRepository.CreateAsync(entity);
        return await GetEmployeeFamilyByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeFamilyDto>();
    }

    /// <summary>
    /// 更新员工家庭成员
    /// </summary>
    /// <param name="id">员工家庭成员ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeFamilyDto> UpdateEmployeeFamilyAsync(long id, TaktEmployeeFamilyUpdateDto dto)
    {
        var entity = await _employeeFamilyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工家庭成员不存在");
        }
        dto.Adapt(entity);
        await _employeeFamilyRepository.UpdateAsync(entity);
        return await GetEmployeeFamilyByIdAsync(id) ?? throw new TaktBusinessException("员工家庭成员不存在");
    }

    /// <summary>
    /// 删除员工家庭成员
    /// </summary>
    /// <param name="id">员工家庭成员ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeFamilyByIdAsync(long id)
    {
        var deleted = await _employeeFamilyRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工家庭成员不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工家庭成员
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeFamilyBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeFamilyByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeFamilyTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeFamilyTemplateDto>(
            sheetName ?? "员工家庭成员导入模板",
            fileName ?? "员工家庭成员导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工家庭成员
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeFamilyAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeFamilyImportDto>(fileStream, sheetName ?? "员工家庭成员导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeFamily>();
                await _employeeFamilyRepository.CreateAsync(entity);
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
    /// 导出员工家庭成员
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeFamilyAsync(TaktEmployeeFamilyQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeFamilyQueryDto());
        var list = await _employeeFamilyRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeFamilyExportDto>(),
                sheetName ?? "员工家庭成员数据",
                fileName ?? "员工家庭成员导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeFamilyExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工家庭成员数据",
            fileName ?? "员工家庭成员导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工家庭成员查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeFamily, bool>> QueryExpression(TaktEmployeeFamilyQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeFamily>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.MemberName != null && x.MemberName.Contains(keywords))
                || SqlFunc.ToString(x.RelationType).Contains(keywords)
                || (x.PhoneNumber != null && x.PhoneNumber.Contains(keywords))
                || (x.WorkUnit != null && x.WorkUnit.Contains(keywords))
                || (x.JobTitle != null && x.JobTitle.Contains(keywords))
                || SqlFunc.ToString(x.IsEmergencyContact).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.BirthDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.MemberName))
        {
            exp = exp.And(x => x.MemberName != null && x.MemberName.Contains(queryDto.MemberName));
        }

        if (queryDto?.RelationType.HasValue == true)
        {
            exp = exp.And(x => x.RelationType == queryDto.RelationType);
        }

        if (!string.IsNullOrEmpty(queryDto?.PhoneNumber))
        {
            exp = exp.And(x => x.PhoneNumber != null && x.PhoneNumber.Contains(queryDto.PhoneNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkUnit))
        {
            exp = exp.And(x => x.WorkUnit != null && x.WorkUnit.Contains(queryDto.WorkUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.JobTitle))
        {
            exp = exp.And(x => x.JobTitle != null && x.JobTitle.Contains(queryDto.JobTitle));
        }

        if (queryDto?.IsEmergencyContact.HasValue == true)
        {
            exp = exp.And(x => x.IsEmergencyContact == queryDto.IsEmergencyContact);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.BirthDateStart.HasValue == true)
        {
            exp = exp.And(x => x.BirthDate >= queryDto.BirthDateStart);
        }

        if (queryDto?.BirthDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.BirthDate <= queryDto.BirthDateEnd);
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
