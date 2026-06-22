// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeContractService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：员工劳动合同应用服务实现
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
/// 员工劳动合同应用服务
/// </summary>
public class TaktEmployeeContractService : TaktServiceBase, ITaktEmployeeContractService
{
    private readonly ITaktCompanyRepository<TaktEmployeeContract> _employeeContractRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeContractRepository">员工劳动合同仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeContractService(
        ITaktCompanyRepository<TaktEmployeeContract> employeeContractRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeContractRepository = employeeContractRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工劳动合同列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeContractDto>> GetEmployeeContractListAsync(TaktEmployeeContractQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeContractRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeContractDto>.Create(
            data.Adapt<List<TaktEmployeeContractDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工劳动合同
    /// </summary>
    /// <param name="id">员工劳动合同ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeContractDto?> GetEmployeeContractByIdAsync(long id)
    {
        var entity = await _employeeContractRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeContractDto>();
    }

    /// <summary>
    /// 获取员工劳动合同选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeContractOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeContractRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ContractNo ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ContractNo ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建员工劳动合同
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeContractDto> CreateEmployeeContractAsync(TaktEmployeeContractCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeContract>();
        var isUnique_ix_employee_contract_no = await _uniqueValidator.IsUniqueAsync(
            _employeeContractRepository,
            x => x.ContractNo == entity.ContractNo);
        if (!isUnique_ix_employee_contract_no)
        {
            throw new TaktBusinessException("员工劳动合同的ContractNo已存在");
        }
        entity = await _employeeContractRepository.CreateAsync(entity);
        return await GetEmployeeContractByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeContractDto>();
    }

    /// <summary>
    /// 更新员工劳动合同
    /// </summary>
    /// <param name="id">员工劳动合同ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeContractDto> UpdateEmployeeContractAsync(long id, TaktEmployeeContractUpdateDto dto)
    {
        var entity = await _employeeContractRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工劳动合同不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_employee_contract_no = await _uniqueValidator.IsUniqueAsync(
            _employeeContractRepository,
            x => x.ContractNo == entity.ContractNo,
            id);
        if (!isUnique_ix_employee_contract_no)
        {
            throw new TaktBusinessException("员工劳动合同的ContractNo已存在");
        }
        await _employeeContractRepository.UpdateAsync(entity);
        return await GetEmployeeContractByIdAsync(id) ?? throw new TaktBusinessException("员工劳动合同不存在");
    }

    /// <summary>
    /// 删除员工劳动合同
    /// </summary>
    /// <param name="id">员工劳动合同ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeContractByIdAsync(long id)
    {
        var deleted = await _employeeContractRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工劳动合同不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工劳动合同
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeContractBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeContractByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新员工劳动合同状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeContractDto> UpdateEmployeeContractStatusAsync(TaktEmployeeContractStatusDto dto)
    {
        var entity = await _employeeContractRepository.GetByIdAsync(dto.EmployeeContractId);
        if (entity == null)
        {
            throw new TaktBusinessException("员工劳动合同不存在");
        }
        entity.ContractStatus = dto.ContractStatus;
        await _employeeContractRepository.UpdateAsync(entity);
        return await GetEmployeeContractByIdAsync(dto.EmployeeContractId) ?? throw new TaktBusinessException("员工劳动合同不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeContractTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeContractTemplateDto>(
            sheetName ?? "员工劳动合同导入模板",
            fileName ?? "员工劳动合同导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工劳动合同
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeContractAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeContractImportDto>(fileStream, sheetName ?? "员工劳动合同导入模板");
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
                var entity = rows[i].Adapt<TaktEmployeeContract>();
                var importKey = $"{entity.ContractNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ContractNo）");
                }
                var isUnique_ix_employee_contract_no = await _uniqueValidator.IsUniqueAsync(
                    _employeeContractRepository,
                    x => x.ContractNo == entity.ContractNo);
                if (!isUnique_ix_employee_contract_no)
                {
                    throw new TaktBusinessException("员工劳动合同的ContractNo已存在");
                }
                await _employeeContractRepository.CreateAsync(entity);
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
    /// 导出员工劳动合同
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeContractAsync(TaktEmployeeContractQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeContractQueryDto());
        var list = await _employeeContractRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeContractExportDto>(),
                sheetName ?? "员工劳动合同数据",
                fileName ?? "员工劳动合同导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeContractExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工劳动合同数据",
            fileName ?? "员工劳动合同导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工劳动合同查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeContract, bool>> QueryExpression(TaktEmployeeContractQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeContract>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.ContractNo != null && x.ContractNo.Contains(keywords))
                || SqlFunc.ToString(x.ContractType).Contains(keywords)
                || SqlFunc.ToString(x.ContractStatus).Contains(keywords)
                || (x.SignCompany != null && x.SignCompany.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.EndDate).Contains(keywords)
                || SqlFunc.ToString(x.ProbationEndDate).Contains(keywords)
                || SqlFunc.ToString(x.SignDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ContractNo))
        {
            exp = exp.And(x => x.ContractNo != null && x.ContractNo.Contains(queryDto.ContractNo));
        }

        if (queryDto?.ContractType.HasValue == true)
        {
            exp = exp.And(x => x.ContractType == queryDto.ContractType);
        }

        if (queryDto?.ContractStatus.HasValue == true)
        {
            exp = exp.And(x => x.ContractStatus == queryDto.ContractStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.SignCompany))
        {
            exp = exp.And(x => x.SignCompany != null && x.SignCompany.Contains(queryDto.SignCompany));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
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

        if (queryDto?.ProbationEndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ProbationEndDate >= queryDto.ProbationEndDateStart);
        }

        if (queryDto?.ProbationEndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ProbationEndDate <= queryDto.ProbationEndDateEnd);
        }

        if (queryDto?.SignDateStart.HasValue == true)
        {
            exp = exp.And(x => x.SignDate >= queryDto.SignDateStart);
        }

        if (queryDto?.SignDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.SignDate <= queryDto.SignDateEnd);
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
