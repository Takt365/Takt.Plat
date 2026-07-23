// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeAddressService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：员工地址应用服务实现
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
/// 员工地址应用服务
/// </summary>
public class TaktEmployeeAddressService : TaktServiceBase, ITaktEmployeeAddressService
{
    private readonly ITaktCompanyRepository<TaktEmployeeAddress> _employeeAddressRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeAddressRepository">员工地址仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeAddressService(
        ITaktCompanyRepository<TaktEmployeeAddress> employeeAddressRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeAddressRepository = employeeAddressRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取员工地址列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeAddressDto>> GetEmployeeAddressListAsync(TaktEmployeeAddressQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeAddressRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeAddressDto>.Create(
            data.Adapt<List<TaktEmployeeAddressDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取员工地址
    /// </summary>
    /// <param name="id">员工地址ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeAddressDto?> GetEmployeeAddressByIdAsync(long id)
    {
        var entity = await _employeeAddressRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeAddressDto>();
    }

    /// <summary>
    /// 获取员工地址选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeAddressOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeAddressRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EmployeeName,
            DictLabel = e.EmployeeName,
        }).ToList();
    }

    /// <summary>
    /// 创建员工地址
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeAddressDto> CreateEmployeeAddressAsync(TaktEmployeeAddressCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeAddress>();
        var isUnique_ix_employee_address_type_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeAddressRepository,
            x => x.EmployeeId == entity.EmployeeId
                && x.AddressType == entity.AddressType);
        if (!isUnique_ix_employee_address_type_unique)
        {
            throw new TaktBusinessException("员工地址的EmployeeId、AddressType已存在");
        }
        entity = await _employeeAddressRepository.CreateAsync(entity);
        return await GetEmployeeAddressByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeAddressDto>();
    }

    /// <summary>
    /// 更新员工地址
    /// </summary>
    /// <param name="id">员工地址ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeAddressDto> UpdateEmployeeAddressAsync(long id, TaktEmployeeAddressUpdateDto dto)
    {
        var entity = await _employeeAddressRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("员工地址不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_employee_address_type_unique = await _uniqueValidator.IsUniqueAsync(
            _employeeAddressRepository,
            x => x.EmployeeId == entity.EmployeeId
                && x.AddressType == entity.AddressType,
            id);
        if (!isUnique_ix_employee_address_type_unique)
        {
            throw new TaktBusinessException("员工地址的EmployeeId、AddressType已存在");
        }
        await _employeeAddressRepository.UpdateAsync(entity);
        return await GetEmployeeAddressByIdAsync(id) ?? throw new TaktBusinessException("员工地址不存在");
    }

    /// <summary>
    /// 删除员工地址
    /// </summary>
    /// <param name="id">员工地址ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeAddressByIdAsync(long id)
    {
        var deleted = await _employeeAddressRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("员工地址不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除员工地址
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeAddressBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeAddressByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeAddressTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeAddressTemplateDto>(
            sheetName ?? "员工地址导入模板",
            fileName ?? "员工地址导入模板.xlsx");
    }

    /// <summary>
    /// 导入员工地址
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeAddressAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeAddressImportDto>(fileStream, sheetName ?? "员工地址导入模板");
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
                var entity = rows[i].Adapt<TaktEmployeeAddress>();
                var importKey = $"{entity.EmployeeId}|{entity.AddressType}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EmployeeId、AddressType）");
                }
                var isUnique_ix_employee_address_type_unique = await _uniqueValidator.IsUniqueAsync(
                    _employeeAddressRepository,
                    x => x.EmployeeId == entity.EmployeeId
                        && x.AddressType == entity.AddressType);
                if (!isUnique_ix_employee_address_type_unique)
                {
                    throw new TaktBusinessException("员工地址的EmployeeId、AddressType已存在");
                }
                await _employeeAddressRepository.CreateAsync(entity);
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
    /// 导出员工地址
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeAddressAsync(TaktEmployeeAddressQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeAddressQueryDto());
        var list = await _employeeAddressRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeAddressExportDto>(),
                sheetName ?? "员工地址数据",
                fileName ?? "员工地址导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeAddressExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "员工地址数据",
            fileName ?? "员工地址导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建员工地址查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeAddress, bool>> QueryExpression(TaktEmployeeAddressQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeAddress>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeCode != null && x.EmployeeCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || SqlFunc.ToString(x.AddressType).Contains(keywords)
                || (x.Country != null && x.Country.Contains(keywords))
                || (x.Province != null && x.Province.Contains(keywords))
                || (x.City != null && x.City.Contains(keywords))
                || (x.District != null && x.District.Contains(keywords))
                || (x.Address1 != null && x.Address1.Contains(keywords))
                || (x.Address2 != null && x.Address2.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EmployeeCode))
        {
            exp = exp.And(x => x.EmployeeCode != null && x.EmployeeCode.Contains(queryDto.EmployeeCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EmployeeName))
        {
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(queryDto.EmployeeName));
        }

        if (queryDto?.AddressType.HasValue == true)
        {
            exp = exp.And(x => x.AddressType == queryDto.AddressType);
        }

        if (!string.IsNullOrEmpty(queryDto?.Country))
        {
            exp = exp.And(x => x.Country != null && x.Country.Contains(queryDto.Country));
        }

        if (!string.IsNullOrEmpty(queryDto?.Province))
        {
            exp = exp.And(x => x.Province != null && x.Province.Contains(queryDto.Province));
        }

        if (!string.IsNullOrEmpty(queryDto?.City))
        {
            exp = exp.And(x => x.City != null && x.City.Contains(queryDto.City));
        }

        if (!string.IsNullOrEmpty(queryDto?.District))
        {
            exp = exp.And(x => x.District != null && x.District.Contains(queryDto.District));
        }

        if (!string.IsNullOrEmpty(queryDto?.Address1))
        {
            exp = exp.And(x => x.Address1 != null && x.Address1.Contains(queryDto.Address1));
        }

        if (!string.IsNullOrEmpty(queryDto?.Address2))
        {
            exp = exp.And(x => x.Address2 != null && x.Address2.Contains(queryDto.Address2));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
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
