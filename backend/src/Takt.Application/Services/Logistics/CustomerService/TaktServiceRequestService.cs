// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktServiceRequestService.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Cursor AI)
// 功能描述：服务请求应用服务实现
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
/// 服务请求应用服务
/// </summary>
public class TaktServiceRequestService : TaktServiceBase, ITaktServiceRequestService
{
    private readonly ITaktCompanyRepository<TaktServiceRequest> _serviceRequestRepository;
    private readonly ITaktCompanyRepository<TaktServiceTicket> _serviceTicketRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceRequestRepository">服务请求仓储</param>
    /// <param name="serviceTicketRepository">ServiceTicket仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktServiceRequestService(
        ITaktCompanyRepository<TaktServiceRequest> serviceRequestRepository,
        ITaktCompanyRepository<TaktServiceTicket> serviceTicketRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _serviceRequestRepository = serviceRequestRepository;
        _serviceTicketRepository = serviceTicketRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取服务请求列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktServiceRequestDto>> GetServiceRequestListAsync(TaktServiceRequestQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _serviceRequestRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktServiceRequestDto>.Create(
            data.Adapt<List<TaktServiceRequestDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceRequestDto?> GetServiceRequestByIdAsync(long id)
    {
        var entity = await _serviceRequestRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktServiceRequestDto>();
        await FillServiceRequestDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取服务请求选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetServiceRequestOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _serviceRequestRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RequestStatus == 1,
            x => x.ClientName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ClientName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建服务请求
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceRequestDto> CreateServiceRequestAsync(TaktServiceRequestCreateDto dto)
    {
        var entity = dto.Adapt<TaktServiceRequest>();
        var isUnique_ix_takt_logistics_service_request_code_unique = await _uniqueValidator.IsUniqueAsync(
            _serviceRequestRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceRequestCode == entity.ServiceRequestCode);
        if (!isUnique_ix_takt_logistics_service_request_code_unique)
        {
            throw new TaktBusinessException("服务请求的PlantCode、ServiceRequestCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _serviceRequestRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
        }
        entity = await _serviceRequestRepository.CreateAsync(entity);
                await SaveServiceRequestChildrenAsync(entity, dto);
        return await GetServiceRequestByIdAsync(entity.Id) ?? entity.Adapt<TaktServiceRequestDto>();
    }

    /// <summary>
    /// 更新服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceRequestDto> UpdateServiceRequestAsync(long id, TaktServiceRequestUpdateDto dto)
    {
        var entity = await _serviceRequestRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务请求不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_service_request_code_unique = await _uniqueValidator.IsUniqueAsync(
            _serviceRequestRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceRequestCode == entity.ServiceRequestCode,
            id);
        if (!isUnique_ix_takt_logistics_service_request_code_unique)
        {
            throw new TaktBusinessException("服务请求的PlantCode、ServiceRequestCode已存在");
        }
        await _serviceRequestRepository.UpdateAsync(entity);
                await SaveServiceRequestChildrenAsync(entity, dto);
        return await GetServiceRequestByIdAsync(id) ?? throw new TaktBusinessException("服务请求不存在");
    }

    /// <summary>
    /// 删除服务请求
    /// </summary>
    /// <param name="id">服务请求ID</param>
    /// <returns>任务</returns>
    public async Task DeleteServiceRequestByIdAsync(long id)
    {
        var entity = await _serviceRequestRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务请求不存在或已删除");
        }
        await _serviceTicketRepository.DeleteAsync(x => x.ServiceRequestId == entity.Id);
        var deleted = await _serviceRequestRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("服务请求不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除服务请求
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteServiceRequestBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteServiceRequestByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新服务请求状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceRequestDto> UpdateServiceRequestStatusAsync(TaktServiceRequestStatusDto dto)
    {
        var entity = await _serviceRequestRepository.GetByIdAsync(dto.ServiceRequestId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务请求不存在");
        }
        entity.RequestStatus = dto.RequestStatus;
        await _serviceRequestRepository.UpdateAsync(entity);
        return await GetServiceRequestByIdAsync(dto.ServiceRequestId) ?? throw new TaktBusinessException("服务请求不存在");
    }

    /// <summary>
    /// 更新服务请求排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceRequestDto> UpdateServiceRequestSortAsync(TaktServiceRequestSortDto dto)
    {
        var entity = await _serviceRequestRepository.GetByIdAsync(dto.ServiceRequestId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务请求不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _serviceRequestRepository.UpdateAsync(entity);
        return await GetServiceRequestByIdAsync(dto.ServiceRequestId) ?? throw new TaktBusinessException("服务请求不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetServiceRequestTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktServiceRequestTemplateDto>(
            sheetName ?? "服务请求导入模板",
            fileName ?? "服务请求导入模板.xlsx");
    }

    /// <summary>
    /// 导入服务请求
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportServiceRequestAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktServiceRequestImportDto>(fileStream, sheetName ?? "服务请求导入模板");
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
                var entity = rows[i].Adapt<TaktServiceRequest>();
                var importKey = $"{entity.PlantCode}|{entity.ServiceRequestCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ServiceRequestCode）");
                }
                var isUnique_ix_takt_logistics_service_request_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _serviceRequestRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ServiceRequestCode == entity.ServiceRequestCode);
                if (!isUnique_ix_takt_logistics_service_request_code_unique)
                {
                    throw new TaktBusinessException("服务请求的PlantCode、ServiceRequestCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _serviceRequestRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
                }
                await _serviceRequestRepository.CreateAsync(entity);
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
    /// 导出服务请求
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportServiceRequestAsync(TaktServiceRequestQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktServiceRequestQueryDto());
        var list = await _serviceRequestRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktServiceRequestExportDto>(),
                sheetName ?? "服务请求数据",
                fileName ?? "服务请求导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktServiceRequestExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "服务请求数据",
            fileName ?? "服务请求导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充服务请求详情（加载 OneToMany 子表：服务工单）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillServiceRequestDetailsAsync(TaktServiceRequestDto dto, TaktServiceRequest entity)
    {
        if (dto == null)
        {
            return;
        }
        // 服务工单 → dto.Tickets
        var tickets = await _serviceTicketRepository.GetListAsync(x => x.ServiceRequestId == entity.Id);
        dto.Tickets = tickets.Adapt<List<TaktServiceTicketDto>>();
    }

    /// <summary>
    /// 保存服务请求子表级联（服务工单；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveServiceRequestChildrenAsync(TaktServiceRequest entity, TaktServiceRequestCreateDto dto)
    {
        // 服务工单（Tickets）
        if (dto.Tickets is not { Count: > 0 })
        {
            await _serviceTicketRepository.DeleteAsync(x => x.ServiceRequestId == entity.Id);
        }
        else
        {
            var tickets = dto.Tickets.Adapt<List<TaktServiceTicket>>();
            foreach (var child in tickets)
            {
                child.ServiceRequestId = entity.Id;
            }
            var ticketsNeedSort = tickets.Where(c => c.SortOrder <= 0).ToList();
            if (ticketsNeedSort.Count > 0)
            {
                var maxSort = await _serviceTicketRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ServiceRequestId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequence(ticketsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in tickets)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < tickets.Count; i++)
                        {
                            var key = $"{tickets[i].CompanyCode}|{tickets[i].PlantCode}|{tickets[i].ServiceTicketCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"服务工单第{i + 1}项与本次提交的其他项重复（CompanyCode、PlantCode、ServiceTicketCode）");
                            }
                        }
            await _serviceTicketRepository.DeleteAsync(x => x.ServiceRequestId == entity.Id);
            foreach (var child in tickets)
            {
            var isUnique_ix_takt_logistics_service_ticket_code_unique = await _uniqueValidator.IsUniqueAsync(
                _serviceTicketRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.PlantCode == child.PlantCode
                    && x.ServiceTicketCode == child.ServiceTicketCode);
            if (!isUnique_ix_takt_logistics_service_ticket_code_unique)
            {
                throw new TaktBusinessException("服务工单的CompanyCode、PlantCode、ServiceTicketCode已存在");
            }
            }
            await _serviceTicketRepository.CreateRangeAsync(tickets);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建服务请求查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktServiceRequest, bool>> QueryExpression(TaktServiceRequestQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktServiceRequest>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(keywords))
                || SqlFunc.ToString(x.ClientId).Contains(keywords)
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName != null && x.ClientName.Contains(keywords))
                || SqlFunc.ToString(x.ServiceContractId).Contains(keywords)
                || (x.ServiceContractCode != null && x.ServiceContractCode.Contains(keywords))
                || SqlFunc.ToString(x.RequestType).Contains(keywords)
                || SqlFunc.ToString(x.SourceChannel).Contains(keywords)
                || SqlFunc.ToString(x.Priority).Contains(keywords)
                || SqlFunc.ToString(x.RequestStatus).Contains(keywords)
                || (x.RequestSubject != null && x.RequestSubject.Contains(keywords))
                || (x.RequestDescription != null && x.RequestDescription.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ContactEmail != null && x.ContactEmail.Contains(keywords))
                || (x.ServiceAddress != null && x.ServiceAddress.Contains(keywords))
                || SqlFunc.ToString(x.AssignedEmployeeId).Contains(keywords)
                || (x.AssignedEmployeeName != null && x.AssignedEmployeeName.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.RequestDate).Contains(keywords)
                || SqlFunc.ToString(x.ExpectedServiceDate).Contains(keywords)
                || SqlFunc.ToString(x.AssignedAt).Contains(keywords)
                || SqlFunc.ToString(x.ClosedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceRequestCode))
        {
            exp = exp.And(x => x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(queryDto.ServiceRequestCode));
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

        if (queryDto?.ServiceContractId.HasValue == true)
        {
            exp = exp.And(x => x.ServiceContractId == queryDto.ServiceContractId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceContractCode))
        {
            exp = exp.And(x => x.ServiceContractCode != null && x.ServiceContractCode.Contains(queryDto.ServiceContractCode));
        }

        if (queryDto?.RequestType.HasValue == true)
        {
            exp = exp.And(x => x.RequestType == queryDto.RequestType);
        }

        if (queryDto?.SourceChannel.HasValue == true)
        {
            exp = exp.And(x => x.SourceChannel == queryDto.SourceChannel);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            exp = exp.And(x => x.Priority == queryDto.Priority);
        }

        if (queryDto?.RequestStatus.HasValue == true)
        {
            exp = exp.And(x => x.RequestStatus == queryDto.RequestStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.RequestSubject))
        {
            exp = exp.And(x => x.RequestSubject != null && x.RequestSubject.Contains(queryDto.RequestSubject));
        }

        if (!string.IsNullOrEmpty(queryDto?.RequestDescription))
        {
            exp = exp.And(x => x.RequestDescription != null && x.RequestDescription.Contains(queryDto.RequestDescription));
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

        if (!string.IsNullOrEmpty(queryDto?.ServiceAddress))
        {
            exp = exp.And(x => x.ServiceAddress != null && x.ServiceAddress.Contains(queryDto.ServiceAddress));
        }

        if (queryDto?.AssignedEmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.AssignedEmployeeId == queryDto.AssignedEmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssignedEmployeeName))
        {
            exp = exp.And(x => x.AssignedEmployeeName != null && x.AssignedEmployeeName.Contains(queryDto.AssignedEmployeeName));
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

        if (queryDto?.RequestDateStart.HasValue == true)
        {
            exp = exp.And(x => x.RequestDate >= queryDto.RequestDateStart);
        }

        if (queryDto?.RequestDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.RequestDate <= queryDto.RequestDateEnd);
        }

        if (queryDto?.ExpectedServiceDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpectedServiceDate >= queryDto.ExpectedServiceDateStart);
        }

        if (queryDto?.ExpectedServiceDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpectedServiceDate <= queryDto.ExpectedServiceDateEnd);
        }

        if (queryDto?.AssignedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.AssignedAt >= queryDto.AssignedAtStart);
        }

        if (queryDto?.AssignedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.AssignedAt <= queryDto.AssignedAtEnd);
        }

        if (queryDto?.ClosedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.ClosedAt >= queryDto.ClosedAtStart);
        }

        if (queryDto?.ClosedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.ClosedAt <= queryDto.ClosedAtEnd);
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
