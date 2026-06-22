// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktServiceContractService.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：服务合同应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.CustomerService;
using Takt.Domain.Entities.Logistics.CustomerService;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.CustomerService;

/// <summary>
/// 服务合同应用服务
/// </summary>
public class TaktServiceContractService : TaktServiceBase, ITaktServiceContractService
{
    private readonly ITaktCompanyRepository<TaktServiceContract> _serviceContractRepository;
    private readonly ITaktCompanyRepository<TaktServiceOrder> _serviceOrderRepository;
    private readonly ITaktCompanyRepository<TaktServiceRequest> _serviceRequestRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceContractRepository">服务合同仓储</param>
    /// <param name="serviceOrderRepository">ServiceOrder仓储</param>
    /// <param name="serviceRequestRepository">ServiceRequest仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktServiceContractService(
        ITaktCompanyRepository<TaktServiceContract> serviceContractRepository,
        ITaktCompanyRepository<TaktServiceOrder> serviceOrderRepository,
        ITaktCompanyRepository<TaktServiceRequest> serviceRequestRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _serviceContractRepository = serviceContractRepository;
        _serviceOrderRepository = serviceOrderRepository;
        _serviceRequestRepository = serviceRequestRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取服务合同列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktServiceContractDto>> GetServiceContractListAsync(TaktServiceContractQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _serviceContractRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktServiceContractDto>.Create(
            data.Adapt<List<TaktServiceContractDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceContractDto?> GetServiceContractByIdAsync(long id)
    {
        var entity = await _serviceContractRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktServiceContractDto>();
        await FillServiceContractDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取服务合同选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetServiceContractOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _serviceContractRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ContractStatus == 1,
            x => x.ContractName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ContractName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建服务合同
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceContractDto> CreateServiceContractAsync(TaktServiceContractCreateDto dto)
    {
        var entity = dto.Adapt<TaktServiceContract>();
        var isUnique_ix_takt_logistics_service_contract_code_unique = await _uniqueValidator.IsUniqueAsync(
            _serviceContractRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceContractCode == entity.ServiceContractCode);
        if (!isUnique_ix_takt_logistics_service_contract_code_unique)
        {
            throw new TaktBusinessException("服务合同的PlantCode、ServiceContractCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _serviceContractRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
        }
        entity = await _serviceContractRepository.CreateAsync(entity);
                await SaveServiceContractChildrenAsync(entity, dto);
        return await GetServiceContractByIdAsync(entity.Id) ?? entity.Adapt<TaktServiceContractDto>();
    }

    /// <summary>
    /// 更新服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceContractDto> UpdateServiceContractAsync(long id, TaktServiceContractUpdateDto dto)
    {
        var entity = await _serviceContractRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_service_contract_code_unique = await _uniqueValidator.IsUniqueAsync(
            _serviceContractRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceContractCode == entity.ServiceContractCode,
            id);
        if (!isUnique_ix_takt_logistics_service_contract_code_unique)
        {
            throw new TaktBusinessException("服务合同的PlantCode、ServiceContractCode已存在");
        }
        await _serviceContractRepository.UpdateAsync(entity);
                await SaveServiceContractChildrenAsync(entity, dto);
        return await GetServiceContractByIdAsync(id) ?? throw new TaktBusinessException("服务合同不存在");
    }

    /// <summary>
    /// 删除服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <returns>任务</returns>
    public async Task DeleteServiceContractByIdAsync(long id)
    {
        var entity = await _serviceContractRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在或已删除");
        }
        await _serviceOrderRepository.DeleteAsync(x => x.ServiceContractId == entity.Id);
        await _serviceRequestRepository.DeleteAsync(x => x.ServiceContractId == entity.Id);
        var deleted = await _serviceContractRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("服务合同不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除服务合同
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteServiceContractBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteServiceContractByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新服务合同状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceContractDto> UpdateServiceContractStatusAsync(TaktServiceContractStatusDto dto)
    {
        var entity = await _serviceContractRepository.GetByIdAsync(dto.ServiceContractId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        entity.ContractStatus = dto.ContractStatus;
        await _serviceContractRepository.UpdateAsync(entity);
        return await GetServiceContractByIdAsync(dto.ServiceContractId) ?? throw new TaktBusinessException("服务合同不存在");
    }

    /// <summary>
    /// 更新服务合同排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceContractDto> UpdateServiceContractSortAsync(TaktServiceContractSortDto dto)
    {
        var entity = await _serviceContractRepository.GetByIdAsync(dto.ServiceContractId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _serviceContractRepository.UpdateAsync(entity);
        return await GetServiceContractByIdAsync(dto.ServiceContractId) ?? throw new TaktBusinessException("服务合同不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetServiceContractTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktServiceContractTemplateDto>(
            sheetName ?? "服务合同导入模板",
            fileName ?? "服务合同导入模板.xlsx");
    }

    /// <summary>
    /// 导入服务合同
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportServiceContractAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktServiceContractImportDto>(fileStream, sheetName ?? "服务合同导入模板");
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
                var entity = rows[i].Adapt<TaktServiceContract>();
                var importKey = $"{entity.PlantCode}|{entity.ServiceContractCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ServiceContractCode）");
                }
                var isUnique_ix_takt_logistics_service_contract_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _serviceContractRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ServiceContractCode == entity.ServiceContractCode);
                if (!isUnique_ix_takt_logistics_service_contract_code_unique)
                {
                    throw new TaktBusinessException("服务合同的PlantCode、ServiceContractCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _serviceContractRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
                }
                await _serviceContractRepository.CreateAsync(entity);
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
    /// 导出服务合同
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportServiceContractAsync(TaktServiceContractQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktServiceContractQueryDto());
        var list = await _serviceContractRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktServiceContractExportDto>(),
                sheetName ?? "服务合同数据",
                fileName ?? "服务合同导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktServiceContractExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "服务合同数据",
            fileName ?? "服务合同导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充服务合同详情（加载 OneToMany 子表：服务订单、服务请求）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillServiceContractDetailsAsync(TaktServiceContractDto dto, TaktServiceContract entity)
    {
        if (dto == null)
        {
            return;
        }
        // 服务订单 → dto.ServiceOrders
        var serviceorders = await _serviceOrderRepository.GetListAsync(x => x.ServiceContractId == entity.Id);
        dto.ServiceOrders = serviceorders.Adapt<List<TaktServiceOrderDto>>();
        // 服务请求 → dto.ServiceRequests
        var servicerequests = await _serviceRequestRepository.GetListAsync(x => x.ServiceContractId == entity.Id);
        dto.ServiceRequests = servicerequests.Adapt<List<TaktServiceRequestDto>>();
    }

    /// <summary>
    /// 保存服务合同子表级联（服务订单、服务请求；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveServiceContractChildrenAsync(TaktServiceContract entity, TaktServiceContractCreateDto dto)
    {
        // 服务订单（ServiceOrders）
        if (dto.ServiceOrders is not { Count: > 0 })
        {
            await _serviceOrderRepository.DeleteAsync(x => x.ServiceContractId == entity.Id);
        }
        else
        {
            var serviceorders = dto.ServiceOrders.Adapt<List<TaktServiceOrder>>();
            foreach (var child in serviceorders)
            {
                child.ServiceContractId = entity.Id;
            }
            var serviceordersNeedSort = serviceorders.Where(c => c.SortOrder <= 0).ToList();
            if (serviceordersNeedSort.Count > 0)
            {
                var maxSort = await _serviceOrderRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ServiceContractId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequence(serviceordersNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in serviceorders)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < serviceorders.Count; i++)
                        {
                            var key = $"{serviceorders[i].CompanyCode}|{serviceorders[i].PlantCode}|{serviceorders[i].ServiceOrderCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"服务订单第{i + 1}项与本次提交的其他项重复（CompanyCode、PlantCode、ServiceOrderCode）");
                            }
                        }
            await _serviceOrderRepository.DeleteAsync(x => x.ServiceContractId == entity.Id);
            foreach (var child in serviceorders)
            {
            var isUnique_ix_takt_logistics_service_order_code_unique = await _uniqueValidator.IsUniqueAsync(
                _serviceOrderRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.PlantCode == child.PlantCode
                    && x.ServiceOrderCode == child.ServiceOrderCode);
            if (!isUnique_ix_takt_logistics_service_order_code_unique)
            {
                throw new TaktBusinessException("服务订单的CompanyCode、PlantCode、ServiceOrderCode已存在");
            }
            }
            await _serviceOrderRepository.CreateRangeAsync(serviceorders);
        }
        // 服务请求（ServiceRequests）
        if (dto.ServiceRequests is not { Count: > 0 })
        {
            await _serviceRequestRepository.DeleteAsync(x => x.ServiceContractId == entity.Id);
        }
        else
        {
            var servicerequests = dto.ServiceRequests.Adapt<List<TaktServiceRequest>>();
            foreach (var child in servicerequests)
            {
                child.ServiceContractId = entity.Id;
            }
            var servicerequestsNeedSort = servicerequests.Where(c => c.SortOrder <= 0).ToList();
            if (servicerequestsNeedSort.Count > 0)
            {
                var maxSort = await _serviceRequestRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ServiceContractId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequence(servicerequestsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in servicerequests)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < servicerequests.Count; i++)
                        {
                            var key = $"{servicerequests[i].CompanyCode}|{servicerequests[i].PlantCode}|{servicerequests[i].ServiceRequestCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"服务请求第{i + 1}项与本次提交的其他项重复（CompanyCode、PlantCode、ServiceRequestCode）");
                            }
                        }
            await _serviceRequestRepository.DeleteAsync(x => x.ServiceContractId == entity.Id);
            foreach (var child in servicerequests)
            {
            var isUnique_ix_takt_logistics_service_request_code_unique = await _uniqueValidator.IsUniqueAsync(
                _serviceRequestRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.PlantCode == child.PlantCode
                    && x.ServiceRequestCode == child.ServiceRequestCode);
            if (!isUnique_ix_takt_logistics_service_request_code_unique)
            {
                throw new TaktBusinessException("服务请求的CompanyCode、PlantCode、ServiceRequestCode已存在");
            }
            }
            await _serviceRequestRepository.CreateRangeAsync(servicerequests);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建服务合同查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktServiceContract, bool>> QueryExpression(TaktServiceContractQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktServiceContract>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ServiceContractCode != null && x.ServiceContractCode.Contains(keywords))
                || (x.ContractName != null && x.ContractName.Contains(keywords))
                || SqlFunc.ToString(x.ClientId).Contains(keywords)
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName != null && x.ClientName.Contains(keywords))
                || SqlFunc.ToString(x.ContractType).Contains(keywords)
                || SqlFunc.ToString(x.ContractStatus).Contains(keywords)
                || SqlFunc.ToString(x.ContractAmount).Contains(keywords)
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || SqlFunc.ToString(x.PaymentTerms).Contains(keywords)
                || (x.ServiceScope != null && x.ServiceScope.Contains(keywords))
                || SqlFunc.ToString(x.SlaResponseHours).Contains(keywords)
                || SqlFunc.ToString(x.SlaResolveHours).Contains(keywords)
                || (x.AccountManager != null && x.AccountManager.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.SignDate).Contains(keywords)
                || SqlFunc.ToString(x.EffectiveDate).Contains(keywords)
                || SqlFunc.ToString(x.ExpiryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceContractCode))
        {
            exp = exp.And(x => x.ServiceContractCode != null && x.ServiceContractCode.Contains(queryDto.ServiceContractCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ContractName))
        {
            exp = exp.And(x => x.ContractName != null && x.ContractName.Contains(queryDto.ContractName));
        }

        if (queryDto?.ClientId.HasValue == true)
        {
            exp = exp.And(x => x.ClientId == queryDto.ClientId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientCode))
        {
            exp = exp.And(x => x.ClientCode != null && x.ClientCode.Contains(queryDto.ClientCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ClientName))
        {
            exp = exp.And(x => x.ClientName != null && x.ClientName.Contains(queryDto.ClientName));
        }

        if (queryDto?.ContractType.HasValue == true)
        {
            exp = exp.And(x => x.ContractType == queryDto.ContractType);
        }

        if (queryDto?.ContractStatus.HasValue == true)
        {
            exp = exp.And(x => x.ContractStatus == queryDto.ContractStatus);
        }

        if (queryDto?.ContractAmount.HasValue == true)
        {
            exp = exp.And(x => x.ContractAmount == queryDto.ContractAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (queryDto?.PaymentTerms.HasValue == true)
        {
            exp = exp.And(x => x.PaymentTerms == queryDto.PaymentTerms);
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceScope))
        {
            exp = exp.And(x => x.ServiceScope != null && x.ServiceScope.Contains(queryDto.ServiceScope));
        }

        if (queryDto?.SlaResponseHours.HasValue == true)
        {
            exp = exp.And(x => x.SlaResponseHours == queryDto.SlaResponseHours);
        }

        if (queryDto?.SlaResolveHours.HasValue == true)
        {
            exp = exp.And(x => x.SlaResolveHours == queryDto.SlaResolveHours);
        }

        if (!string.IsNullOrEmpty(queryDto?.AccountManager))
        {
            exp = exp.And(x => x.AccountManager != null && x.AccountManager.Contains(queryDto.AccountManager));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.SignDateStart.HasValue == true)
        {
            exp = exp.And(x => x.SignDate >= queryDto.SignDateStart);
        }

        if (queryDto?.SignDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.SignDate <= queryDto.SignDateEnd);
        }

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate >= queryDto.EffectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate <= queryDto.EffectiveDateEnd);
        }

        if (queryDto?.ExpiryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate >= queryDto.ExpiryDateStart);
        }

        if (queryDto?.ExpiryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate <= queryDto.ExpiryDateEnd);
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
