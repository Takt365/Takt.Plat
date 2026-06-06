// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktClientService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：客户端信息应用服务实现
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
/// 客户端信息应用服务
/// </summary>
public class TaktClientService : TaktServiceBase, ITaktClientService
{
    private readonly ITaktCompanyRepository<TaktClient> _clientRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="clientRepository">客户端信息仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktClientService(
        ITaktCompanyRepository<TaktClient> clientRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _clientRepository = clientRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取客户端信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktClientDto>> GetClientListAsync(TaktClientQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _clientRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktClientDto>.Create(
            data.Adapt<List<TaktClientDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktClientDto?> GetClientByIdAsync(long id)
    {
        var entity = await _clientRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktClientDto>();
    }

    /// <summary>
    /// 获取客户端信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetClientOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _clientRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ClientName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ClientName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建客户端信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktClientDto> CreateClientAsync(TaktClientCreateDto dto)
    {
        var entity = dto.Adapt<TaktClient>();
        var isUnique_ix_takt_logistics_sales_client_client_code_unique = await _uniqueValidator.IsUniqueAsync(
            _clientRepository,
            x => x.ClientCode == entity.ClientCode);
        if (!isUnique_ix_takt_logistics_sales_client_client_code_unique)
        {
            throw new TaktBusinessException("客户端信息的ClientCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _clientRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _clientRepository.CreateAsync(entity);
        return await GetClientByIdAsync(entity.Id) ?? entity.Adapt<TaktClientDto>();
    }

    /// <summary>
    /// 更新客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktClientDto> UpdateClientAsync(long id, TaktClientUpdateDto dto)
    {
        var entity = await _clientRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("客户端信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_client_client_code_unique = await _uniqueValidator.IsUniqueAsync(
            _clientRepository,
            x => x.ClientCode == entity.ClientCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_client_client_code_unique)
        {
            throw new TaktBusinessException("客户端信息的ClientCode已存在");
        }
        await _clientRepository.UpdateAsync(entity);
        return await GetClientByIdAsync(id) ?? throw new TaktBusinessException("客户端信息不存在");
    }

    /// <summary>
    /// 删除客户端信息
    /// </summary>
    /// <param name="id">客户端信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteClientByIdAsync(long id)
    {
        var deleted = await _clientRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("客户端信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除客户端信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteClientBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteClientByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新客户端信息状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktClientDto> UpdateClientStatusAsync(TaktClientStatusDto dto)
    {
        var entity = await _clientRepository.GetByIdAsync(dto.ClientId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户端信息不存在");
        }
        entity.ClientStatus = dto.ClientStatus;
        await _clientRepository.UpdateAsync(entity);
        return await GetClientByIdAsync(dto.ClientId) ?? throw new TaktBusinessException("客户端信息不存在");
    }

    /// <summary>
    /// 更新客户端信息排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktClientDto> UpdateClientSortAsync(TaktClientSortDto dto)
    {
        var entity = await _clientRepository.GetByIdAsync(dto.ClientId);
        if (entity == null)
        {
            throw new TaktBusinessException("客户端信息不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _clientRepository.UpdateAsync(entity);
        return await GetClientByIdAsync(dto.ClientId) ?? throw new TaktBusinessException("客户端信息不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetClientTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktClientTemplateDto>(
            sheetName ?? "客户端信息导入模板",
            fileName ?? "客户端信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入客户端信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportClientAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktClientImportDto>(fileStream, sheetName ?? "客户端信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _clientRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktClient>();
                var importKey = $"{entity.ClientCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ClientCode）");
                }
                var isUnique_ix_takt_logistics_sales_client_client_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _clientRepository,
                    x => x.ClientCode == entity.ClientCode);
                if (!isUnique_ix_takt_logistics_sales_client_client_code_unique)
                {
                    throw new TaktBusinessException("客户端信息的ClientCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _clientRepository.CreateAsync(entity);
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
    /// 导出客户端信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportClientAsync(TaktClientQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktClientQueryDto());
        var list = await _clientRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktClientExportDto>(),
                sheetName ?? "客户端信息数据",
                fileName ?? "客户端信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktClientExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "客户端信息数据",
            fileName ?? "客户端信息导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建客户端信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktClient, bool>> QueryExpression(TaktClientQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktClient>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName != null && x.ClientName.Contains(keywords))
                || (x.ClientShortName != null && x.ClientShortName.Contains(keywords))
                || SqlFunc.ToString(x.ClientType).Contains(keywords)
                || (x.IndustrySector != null && x.IndustrySector.Contains(keywords))
                || (x.ClientTaxNumber != null && x.ClientTaxNumber.Contains(keywords))
                || (x.RegistrationCountry != null && x.RegistrationCountry.Contains(keywords))
                || (x.RegistrationAddress1 != null && x.RegistrationAddress1.Contains(keywords))
                || (x.RegistrationAddress2 != null && x.RegistrationAddress2.Contains(keywords))
                || (x.RegistrationAddress3 != null && x.RegistrationAddress3.Contains(keywords))
                || (x.ClientPhone != null && x.ClientPhone.Contains(keywords))
                || (x.ClientFax != null && x.ClientFax.Contains(keywords))
                || (x.ClientEmail != null && x.ClientEmail.Contains(keywords))
                || (x.ClientWebsite != null && x.ClientWebsite.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || SqlFunc.ToString(x.PaymentTerms).Contains(keywords)
                || SqlFunc.ToString(x.SalesChannel).Contains(keywords)
                || (x.PlatformName != null && x.PlatformName.Contains(keywords))
                || (x.StoreName != null && x.StoreName.Contains(keywords))
                || SqlFunc.ToString(x.ClientLevel).Contains(keywords)
                || SqlFunc.ToString(x.EvaluationScore).Contains(keywords)
                || SqlFunc.ToString(x.IsQualified).Contains(keywords)
                || SqlFunc.ToString(x.ClientStatus).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.ClientCode))
        {
            exp = exp.And(x => x.ClientCode != null && x.ClientCode.Contains(queryDto.ClientCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientName))
        {
            exp = exp.And(x => x.ClientName != null && x.ClientName.Contains(queryDto.ClientName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientShortName))
        {
            exp = exp.And(x => x.ClientShortName != null && x.ClientShortName.Contains(queryDto.ClientShortName));
        }

        if (queryDto?.ClientType.HasValue == true)
        {
            exp = exp.And(x => x.ClientType == queryDto.ClientType);
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustrySector))
        {
            exp = exp.And(x => x.IndustrySector != null && x.IndustrySector.Contains(queryDto.IndustrySector));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientTaxNumber))
        {
            exp = exp.And(x => x.ClientTaxNumber != null && x.ClientTaxNumber.Contains(queryDto.ClientTaxNumber));
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

        if (!string.IsNullOrEmpty(queryDto?.ClientPhone))
        {
            exp = exp.And(x => x.ClientPhone != null && x.ClientPhone.Contains(queryDto.ClientPhone));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientFax))
        {
            exp = exp.And(x => x.ClientFax != null && x.ClientFax.Contains(queryDto.ClientFax));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientEmail))
        {
            exp = exp.And(x => x.ClientEmail != null && x.ClientEmail.Contains(queryDto.ClientEmail));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientWebsite))
        {
            exp = exp.And(x => x.ClientWebsite != null && x.ClientWebsite.Contains(queryDto.ClientWebsite));
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

        if (queryDto?.SalesChannel.HasValue == true)
        {
            exp = exp.And(x => x.SalesChannel == queryDto.SalesChannel);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlatformName))
        {
            exp = exp.And(x => x.PlatformName != null && x.PlatformName.Contains(queryDto.PlatformName));
        }

        if (!string.IsNullOrEmpty(queryDto?.StoreName))
        {
            exp = exp.And(x => x.StoreName != null && x.StoreName.Contains(queryDto.StoreName));
        }

        if (queryDto?.ClientLevel.HasValue == true)
        {
            exp = exp.And(x => x.ClientLevel == queryDto.ClientLevel);
        }

        if (queryDto?.EvaluationScore.HasValue == true)
        {
            exp = exp.And(x => x.EvaluationScore == queryDto.EvaluationScore);
        }

        if (queryDto?.IsQualified.HasValue == true)
        {
            exp = exp.And(x => x.IsQualified == queryDto.IsQualified);
        }

        if (queryDto?.ClientStatus.HasValue == true)
        {
            exp = exp.And(x => x.ClientStatus == queryDto.ClientStatus);
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
