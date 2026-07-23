// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktCustomerServiceContractService.cs
// 创建时间：2026-07-23
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
public class TaktCustomerServiceContractService : TaktServiceBase, ITaktCustomerServiceContractService
{
    private readonly ITaktCompanyRepository<TaktCustomerServiceContract> _customerServiceContractRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceOrder> _customerServiceOrderRepository;
    private readonly ITaktCompanyRepository<TaktCustomerServiceRequest> _customerServiceRequestRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="customerServiceContractRepository">服务合同仓储</param>
    /// <param name="customerServiceOrderRepository">CustomerServiceOrder仓储</param>
    /// <param name="customerServiceRequestRepository">CustomerServiceRequest仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCustomerServiceContractService(
        ITaktCompanyRepository<TaktCustomerServiceContract> customerServiceContractRepository,
        ITaktCompanyRepository<TaktCustomerServiceOrder> customerServiceOrderRepository,
        ITaktCompanyRepository<TaktCustomerServiceRequest> customerServiceRequestRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _customerServiceContractRepository = customerServiceContractRepository;
        _customerServiceOrderRepository = customerServiceOrderRepository;
        _customerServiceRequestRepository = customerServiceRequestRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取服务合同列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCustomerServiceContractDto>> GetCustomerServiceContractListAsync(TaktCustomerServiceContractQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _customerServiceContractRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCustomerServiceContractDto>.Create(
            data.Adapt<List<TaktCustomerServiceContractDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceContractDto?> GetCustomerServiceContractByIdAsync(long id)
    {
        var entity = await _customerServiceContractRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCustomerServiceContractDto>();
        await FillCustomerServiceContractDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取服务合同选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCustomerServiceContractOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _customerServiceContractRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ContractStatus == 1,
            x => x.ContractName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ServiceContractCode,
            DictLabel = e.ContractName ?? e.ServiceContractCode,
        }).ToList();
    }

    /// <summary>
    /// 创建服务合同
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceContractDto> CreateCustomerServiceContractAsync(TaktCustomerServiceContractCreateDto dto)
    {
        var entity = dto.Adapt<TaktCustomerServiceContract>();
        var isUnique_ix_takt_logistics_customer_service_contract_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceContractRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceContractCode == entity.ServiceContractCode);
        if (!isUnique_ix_takt_logistics_customer_service_contract_code_unique)
        {
            throw new TaktBusinessException("服务合同的PlantCode、ServiceContractCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _customerServiceContractRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
        }
        entity = await _customerServiceContractRepository.CreateAsync(entity);
                await SaveCustomerServiceContractChildrenAsync(entity, dto);
        return await GetCustomerServiceContractByIdAsync(entity.Id) ?? entity.Adapt<TaktCustomerServiceContractDto>();
    }

    /// <summary>
    /// 更新服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceContractDto> UpdateCustomerServiceContractAsync(long id, TaktCustomerServiceContractUpdateDto dto)
    {
        var entity = await _customerServiceContractRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_customer_service_contract_code_unique = await _uniqueValidator.IsUniqueAsync(
            _customerServiceContractRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceContractCode == entity.ServiceContractCode,
            id);
        if (!isUnique_ix_takt_logistics_customer_service_contract_code_unique)
        {
            throw new TaktBusinessException("服务合同的PlantCode、ServiceContractCode已存在");
        }
        await _customerServiceContractRepository.UpdateAsync(entity);
                await SaveCustomerServiceContractChildrenAsync(entity, dto);
        return await GetCustomerServiceContractByIdAsync(id) ?? throw new TaktBusinessException("服务合同不存在");
    }

    /// <summary>
    /// 删除服务合同
    /// </summary>
    /// <param name="id">服务合同ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCustomerServiceContractByIdAsync(long id)
    {
        var entity = await _customerServiceContractRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在或已删除");
        }
        await _customerServiceOrderRepository.DeleteAsync(x => x.ServiceContractId == entity.Id);
        await _customerServiceRequestRepository.DeleteAsync(x => x.ServiceContractId == entity.Id);
        var deleted = await _customerServiceContractRepository.DeleteAsync(id);
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
    public async Task DeleteCustomerServiceContractBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCustomerServiceContractByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新服务合同状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceContractDto> UpdateCustomerServiceContractStatusAsync(TaktCustomerServiceContractStatusDto dto)
    {
        var entity = await _customerServiceContractRepository.GetByIdAsync(dto.CustomerServiceContractId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        entity.ContractStatus = dto.ContractStatus;
        await _customerServiceContractRepository.UpdateAsync(entity);
        return await GetCustomerServiceContractByIdAsync(dto.CustomerServiceContractId) ?? throw new TaktBusinessException("服务合同不存在");
    }

    /// <summary>
    /// 更新服务合同排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCustomerServiceContractDto> UpdateCustomerServiceContractSortAsync(TaktCustomerServiceContractSortDto dto)
    {
        var entity = await _customerServiceContractRepository.GetByIdAsync(dto.CustomerServiceContractId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _customerServiceContractRepository.UpdateAsync(entity);
        return await GetCustomerServiceContractByIdAsync(dto.CustomerServiceContractId) ?? throw new TaktBusinessException("服务合同不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCustomerServiceContractTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCustomerServiceContractTemplateDto>(
            sheetName ?? "服务合同导入模板",
            fileName ?? "服务合同导入模板.xlsx");
    }

    /// <summary>
    /// 导入服务合同
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCustomerServiceContractAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCustomerServiceContractImportDto>(fileStream, sheetName ?? "服务合同导入模板");
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
                var entity = rows[i].Adapt<TaktCustomerServiceContract>();
                var importKey = $"{entity.PlantCode}|{entity.ServiceContractCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ServiceContractCode）");
                }
                var isUnique_ix_takt_logistics_customer_service_contract_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _customerServiceContractRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ServiceContractCode == entity.ServiceContractCode);
                if (!isUnique_ix_takt_logistics_customer_service_contract_code_unique)
                {
                    throw new TaktBusinessException("服务合同的PlantCode、ServiceContractCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _customerServiceContractRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
                }
                await _customerServiceContractRepository.CreateAsync(entity);
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
    public async Task<(string fileName, byte[] fileContent)> ExportCustomerServiceContractAsync(TaktCustomerServiceContractQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCustomerServiceContractQueryDto());
        var list = await _customerServiceContractRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCustomerServiceContractExportDto>(),
                sheetName ?? "服务合同数据",
                fileName ?? "服务合同导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCustomerServiceContractExportDto>>();
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
    private async Task FillCustomerServiceContractDetailsAsync(TaktCustomerServiceContractDto dto, TaktCustomerServiceContract entity)
    {
        if (dto == null)
        {
            return;
        }
        // 服务订单 → dto.ServiceOrders
        var serviceorders = await _customerServiceOrderRepository.GetListAsync(x => x.ServiceContractId == entity.Id);
        dto.ServiceOrders = serviceorders.Adapt<List<TaktCustomerServiceOrderDto>>();
        // 服务请求 → dto.ServiceRequests
        var servicerequests = await _customerServiceRequestRepository.GetListAsync(x => x.ServiceContractId == entity.Id);
        dto.ServiceRequests = servicerequests.Adapt<List<TaktCustomerServiceRequestDto>>();
    }

    /// <summary>
    /// 保存服务合同子表级联（服务订单、服务请求；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveCustomerServiceContractChildrenAsync(TaktCustomerServiceContract entity, TaktCustomerServiceContractCreateDto dto)
    {
        // 服务订单（ServiceOrders）
        List<TaktCustomerServiceOrderUpdateDto>? serviceOrdersForSave;
        if (dto is TaktCustomerServiceContractUpdateDto updateDtoForServiceOrders && updateDtoForServiceOrders.ServiceOrders != null)
        {
            serviceOrdersForSave = updateDtoForServiceOrders.ServiceOrders;
        }
        else if (dto.ServiceOrders != null)
        {
            serviceOrdersForSave = dto.ServiceOrders.Adapt<List<TaktCustomerServiceOrderUpdateDto>>();
        }
        else
        {
            serviceOrdersForSave = null;
        }
        if (serviceOrdersForSave is not { Count: > 0 })
        {
            await _customerServiceOrderRepository.DeleteAsync(x => x.ServiceContractId == entity.Id);
        }
        else
        {
            var existingList = await _customerServiceOrderRepository.GetListAsync(x => x.ServiceContractId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktCustomerServiceOrder>();
            for (var i = 0; i < serviceOrdersForSave.Count; i++)
            {
                var childDto = serviceOrdersForSave[i];
                childDto.ServiceContractId = entity.Id;
                if (childDto.CustomerServiceOrderId > 0)
                {
                    if (!existingById.TryGetValue(childDto.CustomerServiceOrderId, out var target))
                    {
                        throw new TaktBusinessException("服务订单不存在（CustomerServiceOrderId={childDto.CustomerServiceOrderId}）");
                    }
                    if (target.ServiceContractId != entity.Id)
                    {
                        throw new TaktBusinessException("服务订单不属于当前主表（CustomerServiceOrderId={childDto.CustomerServiceOrderId}）");
                    }
                    submittedIds.Add(childDto.CustomerServiceOrderId);
                    childDto.Adapt(target);
                    target.Id = childDto.CustomerServiceOrderId;
                    target.ServiceContractId = entity.Id;
                    await _customerServiceOrderRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktCustomerServiceOrder>();
                    child.Id = 0;
                    child.ServiceContractId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _customerServiceOrderRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _customerServiceOrderRepository.CreateRangeAsync(toCreate);
            }
        }
        // 服务请求（ServiceRequests）
        List<TaktCustomerServiceRequestUpdateDto>? serviceRequestsForSave;
        if (dto is TaktCustomerServiceContractUpdateDto updateDtoForServiceRequests && updateDtoForServiceRequests.ServiceRequests != null)
        {
            serviceRequestsForSave = updateDtoForServiceRequests.ServiceRequests;
        }
        else if (dto.ServiceRequests != null)
        {
            serviceRequestsForSave = dto.ServiceRequests.Adapt<List<TaktCustomerServiceRequestUpdateDto>>();
        }
        else
        {
            serviceRequestsForSave = null;
        }
        if (serviceRequestsForSave is not { Count: > 0 })
        {
            await _customerServiceRequestRepository.DeleteAsync(x => x.ServiceContractId == entity.Id);
        }
        else
        {
            var existingList = await _customerServiceRequestRepository.GetListAsync(x => x.ServiceContractId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktCustomerServiceRequest>();
            for (var i = 0; i < serviceRequestsForSave.Count; i++)
            {
                var childDto = serviceRequestsForSave[i];
                childDto.ServiceContractId = entity.Id;
                if (childDto.CustomerServiceRequestId > 0)
                {
                    if (!existingById.TryGetValue(childDto.CustomerServiceRequestId, out var target))
                    {
                        throw new TaktBusinessException("服务请求不存在（CustomerServiceRequestId={childDto.CustomerServiceRequestId}）");
                    }
                    if (target.ServiceContractId != entity.Id)
                    {
                        throw new TaktBusinessException("服务请求不属于当前主表（CustomerServiceRequestId={childDto.CustomerServiceRequestId}）");
                    }
                    submittedIds.Add(childDto.CustomerServiceRequestId);
                    childDto.Adapt(target);
                    target.Id = childDto.CustomerServiceRequestId;
                    target.ServiceContractId = entity.Id;
                    await _customerServiceRequestRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktCustomerServiceRequest>();
                    child.Id = 0;
                    child.ServiceContractId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _customerServiceRequestRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _customerServiceRequestRepository.CreateRangeAsync(toCreate);
            }
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
    private static Expression<Func<TaktCustomerServiceContract, bool>> QueryExpression(TaktCustomerServiceContractQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCustomerServiceContract>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ServiceContractCode != null && x.ServiceContractCode.Contains(keywords))
                || (x.ContractName != null && x.ContractName.Contains(keywords))
                || SqlFunc.ToString(x.ClientId).Contains(keywords)
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName1 != null && x.ClientName1.Contains(keywords))
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

        if (!string.IsNullOrEmpty(queryDto?.ClientName1))
        {
            exp = exp.And(x => x.ClientName1 != null && x.ClientName1.Contains(queryDto.ClientName1));
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
