// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeDelegationService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：员工代理关系应用服务实现
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
/// 员工代理关系应用服务
/// </summary>
public class TaktEmployeeDelegationService : TaktServiceBase, ITaktEmployeeDelegationService
{
    private readonly ITaktCompanyRepository<TaktEmployeeDelegation> _employeeDelegationRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeDelegationRepository">员工代理关系仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeDelegationService(
        ITaktCompanyRepository<TaktEmployeeDelegation> employeeDelegationRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeDelegationRepository = employeeDelegationRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工代理关系列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeDelegationDto>> GetEmployeeDelegationListAsync(TaktEmployeeDelegationQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeDelegationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeDelegationDto>.Create(
            data.Adapt<List<TaktEmployeeDelegationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工代理关系
    /// </summary>
    /// <param name="id">员工代理关系ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDelegationDto?> GetEmployeeDelegationByIdAsync(long id)
    {
        var entity = await _employeeDelegationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeDelegationDto>();
    }

    /// <summary>
    /// 获取员工代理关系选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeDelegationOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeDelegationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.Reason,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.Reason ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建员工代理关系
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDelegationDto> CreateEmployeeDelegationAsync(TaktEmployeeDelegationCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeDelegation>();
        var isUnique_ix_employee_delegation_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeDelegationRepository,
            x => x.OriginalEmployeeId == entity.OriginalEmployeeId
                && x.ProxyEmployeeId == entity.ProxyEmployeeId
                && x.DelegationType == entity.DelegationType
                && x.StartDate == entity.StartDate);
        if (!isUnique_ix_employee_delegation_unique)
        {
            throw new TaktBusinessException("员工代理关系的OriginalEmployeeId、ProxyEmployeeId、DelegationType、StartDate已存在");
        }
        entity = await _employeeDelegationRepository.CreateAsync(entity);
        return await GetEmployeeDelegationByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeDelegationDto>();
    }

    /// <summary>
    /// 更新员工代理关系
    /// </summary>
    /// <param name="id">员工代理关系ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeDelegationDto> UpdateEmployeeDelegationAsync(long id, TaktEmployeeDelegationUpdateDto dto)
    {
        var entity = await _employeeDelegationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工代理关系不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_employee_delegation_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeDelegationRepository,
            x => x.OriginalEmployeeId == entity.OriginalEmployeeId
                && x.ProxyEmployeeId == entity.ProxyEmployeeId
                && x.DelegationType == entity.DelegationType
                && x.StartDate == entity.StartDate,
            id);
        if (!isUnique_ix_employee_delegation_unique)
        {
            throw new TaktBusinessException("员工代理关系的OriginalEmployeeId、ProxyEmployeeId、DelegationType、StartDate已存在");
        }
        await _employeeDelegationRepository.UpdateAsync(entity);
        return await GetEmployeeDelegationByIdAsync(id) ?? throw new TaktBusinessException("员工代理关系不存在");
    }

    /// <summary>
    /// 删除员工代理关系
    /// </summary>
    /// <param name="id">员工代理关系ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeDelegationByIdAsync(long id)
    {
        var deleted = await _employeeDelegationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工代理关系不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工代理关系
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeDelegationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeDelegationByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeDelegationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeDelegationTemplateDto>(
            sheetName ?? "员工代理关系导入模板",
            fileName ?? "员工代理关系导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工代理关系
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeDelegationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeDelegationImportDto>(fileStream, sheetName ?? "员工代理关系导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeDelegation>();
                var importKey = $"{entity.OriginalEmployeeId}|{entity.ProxyEmployeeId}|{entity.DelegationType}|{entity.StartDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（OriginalEmployeeId、ProxyEmployeeId、DelegationType、StartDate）");
                }
                var isUnique_ix_employee_delegation_unique = await _uniqueValidator.IsUniqueAsync(
                    _employeeDelegationRepository,
                    x => x.OriginalEmployeeId == entity.OriginalEmployeeId
                        && x.ProxyEmployeeId == entity.ProxyEmployeeId
                        && x.DelegationType == entity.DelegationType
                        && x.StartDate == entity.StartDate);
                if (!isUnique_ix_employee_delegation_unique)
                {
                    throw new TaktBusinessException("员工代理关系的OriginalEmployeeId、ProxyEmployeeId、DelegationType、StartDate已存在");
                }
                await _employeeDelegationRepository.CreateAsync(entity);
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
    /// 导出员工代理关系
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeDelegationAsync(TaktEmployeeDelegationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeDelegationQueryDto());
        var list = await _employeeDelegationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeDelegationExportDto>(),
                sheetName ?? "员工代理关系数据",
                fileName ?? "员工代理关系导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeDelegationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工代理关系数据",
            fileName ?? "员工代理关系导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工代理关系查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeDelegation, bool>> QueryExpression(TaktEmployeeDelegationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeDelegation>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ProxyEmployeeId).Contains(keywords)
                || SqlFunc.ToString(x.OriginalEmployeeId).Contains(keywords)
                || SqlFunc.ToString(x.DelegationType).Contains(keywords)
                || SqlFunc.ToString(x.ScopeType).Contains(keywords)
                || SqlFunc.ToString(x.ScopeId).Contains(keywords)
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.EndDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ProxyEmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.ProxyEmployeeId == queryDto.ProxyEmployeeId);
        }

        if (queryDto?.OriginalEmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.OriginalEmployeeId == queryDto.OriginalEmployeeId);
        }

        if (queryDto?.DelegationType.HasValue == true)
        {
            exp = exp.And(x => x.DelegationType == queryDto.DelegationType);
        }

        if (queryDto?.ScopeType.HasValue == true)
        {
            exp = exp.And(x => x.ScopeType == queryDto.ScopeType);
        }

        if (queryDto?.ScopeId.HasValue == true)
        {
            exp = exp.And(x => x.ScopeId == queryDto.ScopeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.Reason))
        {
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(queryDto.Reason));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.StartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.StartDate >= queryDto.StartDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartDate <= queryDto.StartDateEnd);
        }

        if (queryDto?.EndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EndDate >= queryDto.EndDateStart);
        }

        if (queryDto?.EndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EndDate <= queryDto.EndDateEnd);
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
