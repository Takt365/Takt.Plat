// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktCustomerService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：客户信息应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 客户信息应用服务
/// </summary>
public class TaktCustomerService : TaktServiceBase, ITaktCustomerService
{
    private readonly ITaktCompanyRepository<TaktCustomer> _customerRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerRepository">客户信息仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerService(
        ITaktCompanyRepository<TaktCustomer> customerRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerRepository = customerRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客户信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerDto>> GetCustomerListAsync(TaktCustomerQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerDto>.Create(
            data.Adapt<List<TaktCustomerDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客户信息
    /// </summary>
    /// <param name="id">客户信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerDto?> GetCustomerByIdAsync(long id)
    {
        var entity = await _customerRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCustomerDto>();
    }

    /// <summary>
    /// 获取客户信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _customerRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CustomerName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CustomerName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建客户信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerDto> CreateCustomerAsync(TaktCustomerCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomer>();
        var isUnique_ix_takt_logistics_sales_customer_customer_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerRepository,
            x => x.CustomerCode == entity.CustomerCode);
        if (!isUnique_ix_takt_logistics_sales_customer_customer_code_unique)
        {
            throw new TaktBusinessException("客户信息的CustomerCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _customerRepository.CreateAsync(entity);
        return await GetCustomerByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerDto>();
    }

    /// <summary>
    /// 更新客户信息
    /// </summary>
    /// <param name="id">客户信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerDto> UpdateCustomerAsync(long id, TaktCustomerUpdateDto dto)
    {
        var entity = await _customerRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_customer_customer_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerRepository,
            x => x.CustomerCode == entity.CustomerCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_customer_customer_code_unique)
        {
            throw new TaktBusinessException("客户信息的CustomerCode已存在");
        }
        await _customerRepository.UpdateAsync(entity);
        return await GetCustomerByIdAsync(id) ?? throw new TaktBusinessException("客户信息不存在");
    }

    /// <summary>
    /// 删除客户信息
    /// </summary>
    /// <param name="id">客户信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerByIdAsync(long id)
    {
        var deleted = await _customerRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("客户信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除客户信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客户信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerDto> UpdateCustomerStatusAsync(TaktCustomerStatusDto dto)
    {
        var entity = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户信息不存在");
        }
        entity.CustomerStatus = dto.CustomerStatus;
        await _customerRepository.UpdateAsync(entity);
        return await GetCustomerByIdAsync(dto.CustomerId) ?? throw new TaktBusinessException("客户信息不存在");
    }

    /// <summary>
    /// 更新客户信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerDto> UpdateCustomerSortAsync(TaktCustomerSortDto dto)
    {
        var entity = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户信息不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerRepository.UpdateAsync(entity);
        return await GetCustomerByIdAsync(dto.CustomerId) ?? throw new TaktBusinessException("客户信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerTemplateDto>(
            sheetName ?? "客户信息导入模板",
            fileName ?? "客户信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入客户信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerImportDto>(fileStream, sheetName ?? "客户信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _customerRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktCustomer>();
                var importKey = $"{entity.CustomerCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CustomerCode）");
                }
                var isUnique_ix_takt_logistics_sales_customer_customer_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerRepository,
                    x => x.CustomerCode == entity.CustomerCode);
                if (!isUnique_ix_takt_logistics_sales_customer_customer_code_unique)
                {
                    throw new TaktBusinessException("客户信息的CustomerCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _customerRepository.CreateAsync(entity);
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
    /// 导出客户信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerAsync(TaktCustomerQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCustomerQueryDto());
        var list = await _customerRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerExportDto>(),
                sheetName ?? "客户信息数据",
                fileName ?? "客户信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客户信息数据",
            fileName ?? "客户信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客户信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCustomer, bool>> QueryExpression(TaktCustomerQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomer>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.CustomerName != null && x.CustomerName.Contains(keywords))
                || (x.CustomerShortName != null && x.CustomerShortName.Contains(keywords))
                || SqlFunc.ToString(x.CustomerType).Contains(keywords)
                || (x.IndustrySector != null && x.IndustrySector.Contains(keywords))
                || (x.CustomerTaxNumber != null && x.CustomerTaxNumber.Contains(keywords))
                || (x.RegistrationCountry != null && x.RegistrationCountry.Contains(keywords))
                || (x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(keywords))
                || (x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(keywords))
                || (x.RegistrationAddress3 != null && x.RegistrationAddress3.Contains(keywords))
                || (x.CustomerPhone != null && x.CustomerPhone.Contains(keywords))
                || (x.CustomerFax != null && x.CustomerFax.Contains(keywords))
                || (x.CustomerEmail != null && x.CustomerEmail.Contains(keywords))
                || (x.CustomerWebsite != null && x.CustomerWebsite.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || SqlFunc.ToString(x.PaymentTerms).Contains(keywords)
                || SqlFunc.ToString(x.CreditLevel).Contains(keywords)
                || SqlFunc.ToString(x.CreditAmount).Contains(keywords)
                || SqlFunc.ToString(x.DiscountRate).Contains(keywords)
                || (x.SalesBy != null && x.SalesBy.Contains(keywords))
                || SqlFunc.ToString(x.CustomerLevel).Contains(keywords)
                || SqlFunc.ToString(x.EvaluationScore).Contains(keywords)
                || SqlFunc.ToString(x.IsQualified).Contains(keywords)
                || SqlFunc.ToString(x.CustomerStatus).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerCode))
        {
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(queryDto.CustomerCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerName))
        {
            exp = exp.And(x => x.CustomerName != null && x.CustomerName.Contains(queryDto.CustomerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerShortName))
        {
            exp = exp.And(x => x.CustomerShortName != null && x.CustomerShortName.Contains(queryDto.CustomerShortName));
        }

        if (queryDto?.CustomerType.HasValue == true)
        {
            exp = exp.And(x => x.CustomerType == queryDto.CustomerType);
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustrySector))
        {
            exp = exp.And(x => x.IndustrySector != null && x.IndustrySector.Contains(queryDto.IndustrySector));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerTaxNumber))
        {
            exp = exp.And(x => x.CustomerTaxNumber != null && x.CustomerTaxNumber.Contains(queryDto.CustomerTaxNumber));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationCountry))
        {
            exp = exp.And(x => x.RegistrationCountry != null && x.RegistrationCountry.Contains(queryDto.RegistrationCountry));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationAddress1))
        {
            exp = exp.And(x => x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(queryDto.RegistrationAddress1));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationAddress2))
        {
            exp = exp.And(x => x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(queryDto.RegistrationAddress2));
        }

        if (!string.IsNullOrEmpty(queryDto?.RegistrationAddress3))
        {
            exp = exp.And(x => x.RegistrationAddress3 != null && x.RegistrationAddress3.Contains(queryDto.RegistrationAddress3));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerPhone))
        {
            exp = exp.And(x => x.CustomerPhone != null && x.CustomerPhone.Contains(queryDto.CustomerPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerFax))
        {
            exp = exp.And(x => x.CustomerFax != null && x.CustomerFax.Contains(queryDto.CustomerFax));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerEmail))
        {
            exp = exp.And(x => x.CustomerEmail != null && x.CustomerEmail.Contains(queryDto.CustomerEmail));
        }

        if (!string.IsNullOrEmpty(queryDto?.CustomerWebsite))
        {
            exp = exp.And(x => x.CustomerWebsite != null && x.CustomerWebsite.Contains(queryDto.CustomerWebsite));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactPerson))
        {
            exp = exp.And(x => x.ContactPerson != null && x.ContactPerson.Contains(queryDto.ContactPerson));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactPhone))
        {
            exp = exp.And(x => x.ContactPhone != null && x.ContactPhone.Contains(queryDto.ContactPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContactEmail))
        {
            exp = exp.And(x => x.ContactEmail != null && x.ContactEmail.Contains(queryDto.ContactEmail));
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (queryDto?.PaymentTerms.HasValue == true)
        {
            exp = exp.And(x => x.PaymentTerms == queryDto.PaymentTerms);
        }

        if (queryDto?.CreditLevel.HasValue == true)
        {
            exp = exp.And(x => x.CreditLevel == queryDto.CreditLevel);
        }

        if (queryDto?.CreditAmount.HasValue == true)
        {
            exp = exp.And(x => x.CreditAmount == queryDto.CreditAmount);
        }

        if (queryDto?.DiscountRate.HasValue == true)
        {
            exp = exp.And(x => x.DiscountRate == queryDto.DiscountRate);
        }

        if (!string.IsNullOrEmpty(queryDto?.SalesBy))
        {
            exp = exp.And(x => x.SalesBy != null && x.SalesBy.Contains(queryDto.SalesBy));
        }

        if (queryDto?.CustomerLevel.HasValue == true)
        {
            exp = exp.And(x => x.CustomerLevel == queryDto.CustomerLevel);
        }

        if (queryDto?.EvaluationScore.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationScore == queryDto.EvaluationScore);
        }

        if (queryDto?.IsQualified.HasValue == true)
        {
            exp = exp.And(x => x.IsQualified == queryDto.IsQualified);
        }

        if (queryDto?.CustomerStatus.HasValue == true)
        {
            exp = exp.And(x => x.CustomerStatus == queryDto.CustomerStatus);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
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
