// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.CustomerService
// 文件名称：TaktServiceTicketService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：服务工单应用服务实现
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
/// 服务工单应用服务
/// </summary>
public class TaktServiceTicketService : TaktServiceBase, ITaktServiceTicketService
{
    private readonly ITaktCompanyRepository<TaktServiceTicket> _serviceTicketRepository;
    private readonly ITaktCompanyRepository<TaktServiceRequest> _serviceRequestRepository;
    private readonly ITaktCompanyRepository<TaktServiceOrder> _serviceOrderRepository;
    private readonly ITaktCompanyRepository<TaktServiceContract> _serviceContractRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceTicketRepository">服务工单仓储</param>
    /// <param name="serviceRequestRepository">服务请求仓储</param>
    /// <param name="serviceOrderRepository">服务订单仓储</param>
    /// <param name="serviceContractRepository">服务合同仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktServiceTicketService(
        ITaktCompanyRepository<TaktServiceTicket> serviceTicketRepository,
        ITaktCompanyRepository<TaktServiceRequest> serviceRequestRepository,
        ITaktCompanyRepository<TaktServiceOrder> serviceOrderRepository,
        ITaktCompanyRepository<TaktServiceContract> serviceContractRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _serviceTicketRepository = serviceTicketRepository;
        _serviceRequestRepository = serviceRequestRepository;
        _serviceOrderRepository = serviceOrderRepository;
        _serviceContractRepository = serviceContractRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取服务工单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktServiceTicketDto>> GetServiceTicketListAsync(TaktServiceTicketQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _serviceTicketRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktServiceTicketDto>.Create(
            data.Adapt<List<TaktServiceTicketDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取服务工单
    /// </summary>
    /// <param name="id">服务工单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceTicketDto?> GetServiceTicketByIdAsync(long id)
    {
        var entity = await _serviceTicketRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktServiceTicketDto>();
    }

    /// <summary>
    /// 获取服务工单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetServiceTicketOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _serviceTicketRepository.GetListAsync(
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
    /// 创建服务工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceTicketDto> CreateServiceTicketAsync(TaktServiceTicketCreateDto dto)
    {
        var entity = dto.Adapt<TaktServiceTicket>();
        await StampServiceTicketServiceRequestAsync(entity, dto);
        await StampServiceTicketServiceOrderAsync(entity, dto);
        await StampServiceTicketServiceContractAsync(entity, dto);
        var isUnique_ix_takt_logistics_service_ticket_code_unique = await _uniqueValidator.IsUniqueAsync(
            _serviceTicketRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceTicketCode == entity.ServiceTicketCode);
        if (!isUnique_ix_takt_logistics_service_ticket_code_unique)
        {
            throw new TaktBusinessException("服务工单的PlantCode、ServiceTicketCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _serviceTicketRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
        }
        entity = await _serviceTicketRepository.CreateAsync(entity);
        return await GetServiceTicketByIdAsync(entity.Id) ?? entity.Adapt<TaktServiceTicketDto>();
    }

    /// <summary>
    /// 更新服务工单
    /// </summary>
    /// <param name="id">服务工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceTicketDto> UpdateServiceTicketAsync(long id, TaktServiceTicketUpdateDto dto)
    {
        var entity = await _serviceTicketRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("服务工单不存在");
        }
        dto.Adapt(entity);
        await StampServiceTicketServiceRequestAsync(entity, dto);
        await StampServiceTicketServiceOrderAsync(entity, dto);
        await StampServiceTicketServiceContractAsync(entity, dto);
        var isUnique_ix_takt_logistics_service_ticket_code_unique = await _uniqueValidator.IsUniqueAsync(
            _serviceTicketRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ServiceTicketCode == entity.ServiceTicketCode,
            id);
        if (!isUnique_ix_takt_logistics_service_ticket_code_unique)
        {
            throw new TaktBusinessException("服务工单的PlantCode、ServiceTicketCode已存在");
        }
        await _serviceTicketRepository.UpdateAsync(entity);
        return await GetServiceTicketByIdAsync(id) ?? throw new TaktBusinessException("服务工单不存在");
    }

    /// <summary>
    /// 删除服务工单
    /// </summary>
    /// <param name="id">服务工单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteServiceTicketByIdAsync(long id)
    {
        var deleted = await _serviceTicketRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("服务工单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除服务工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteServiceTicketBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteServiceTicketByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新服务工单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceTicketDto> UpdateServiceTicketStatusAsync(TaktServiceTicketStatusDto dto)
    {
        var entity = await _serviceTicketRepository.GetByIdAsync(dto.ServiceTicketId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务工单不存在");
        }
        entity.TicketStatus = dto.TicketStatus;
        await _serviceTicketRepository.UpdateAsync(entity);
        return await GetServiceTicketByIdAsync(dto.ServiceTicketId) ?? throw new TaktBusinessException("服务工单不存在");
    }

    /// <summary>
    /// 更新服务工单排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktServiceTicketDto> UpdateServiceTicketSortAsync(TaktServiceTicketSortDto dto)
    {
        var entity = await _serviceTicketRepository.GetByIdAsync(dto.ServiceTicketId);
        if (entity == null)
        {
            throw new TaktBusinessException("服务工单不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _serviceTicketRepository.UpdateAsync(entity);
        return await GetServiceTicketByIdAsync(dto.ServiceTicketId) ?? throw new TaktBusinessException("服务工单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetServiceTicketTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktServiceTicketTemplateDto>(
            sheetName ?? "服务工单导入模板",
            fileName ?? "服务工单导入模板.xlsx");
    }

    /// <summary>
    /// 导入服务工单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportServiceTicketAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktServiceTicketImportDto>(fileStream, sheetName ?? "服务工单导入模板");
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
                var entity = rows[i].Adapt<TaktServiceTicket>();
                var importDto = rows[i].Adapt<TaktServiceTicketCreateDto>();
                await StampServiceTicketServiceRequestAsync(entity, importDto);
                await StampServiceTicketServiceOrderAsync(entity, importDto);
                await StampServiceTicketServiceContractAsync(entity, importDto);
                var importKey = $"{entity.PlantCode}|{entity.ServiceTicketCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ServiceTicketCode）");
                }
                var isUnique_ix_takt_logistics_service_ticket_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _serviceTicketRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ServiceTicketCode == entity.ServiceTicketCode);
                if (!isUnique_ix_takt_logistics_service_ticket_code_unique)
                {
                    throw new TaktBusinessException("服务工单的PlantCode、ServiceTicketCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _serviceTicketRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ClientId == entity.ClientId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.ClientId, maxSort);
                }
                await _serviceTicketRepository.CreateAsync(entity);
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
    /// 导出服务工单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportServiceTicketAsync(TaktServiceTicketQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktServiceTicketQueryDto());
        var list = await _serviceTicketRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktServiceTicketExportDto>(),
                sheetName ?? "服务工单数据",
                fileName ?? "服务工单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktServiceTicketExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "服务工单数据",
            fileName ?? "服务工单导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步服务工单主表外键（ManyToOne → 服务请求）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampServiceTicketServiceRequestAsync(TaktServiceTicket entity, TaktServiceTicketCreateDto dto)
    {
        if (dto.ServiceRequestId is not > 0)
        {
            return;
        }
        var master = await _serviceRequestRepository.GetByIdAsync(dto.ServiceRequestId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("服务请求不存在");
        }
        entity.ServiceRequestId = master.Id;
    }

    /// <summary>
    /// 同步服务工单主表外键（ManyToOne → 服务订单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampServiceTicketServiceOrderAsync(TaktServiceTicket entity, TaktServiceTicketCreateDto dto)
    {
        if (dto.ServiceOrderId is not > 0)
        {
            return;
        }
        var master = await _serviceOrderRepository.GetByIdAsync(dto.ServiceOrderId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("服务订单不存在");
        }
        entity.ServiceOrderId = master.Id;
    }

    /// <summary>
    /// 同步服务工单主表外键（ManyToOne → 服务合同）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampServiceTicketServiceContractAsync(TaktServiceTicket entity, TaktServiceTicketCreateDto dto)
    {
        if (dto.ServiceContractId is not > 0)
        {
            return;
        }
        var master = await _serviceContractRepository.GetByIdAsync(dto.ServiceContractId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("服务合同不存在");
        }
        entity.ServiceContractId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建服务工单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktServiceTicket, bool>> QueryExpression(TaktServiceTicketQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktServiceTicket>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ServiceTicketCode != null && x.ServiceTicketCode.Contains(keywords))
                || SqlFunc.ToString(x.ClientId).Contains(keywords)
                || (x.ClientCode != null && x.ClientCode.Contains(keywords))
                || (x.ClientName != null && x.ClientName.Contains(keywords))
                || SqlFunc.ToString(x.ServiceRequestId).Contains(keywords)
                || (x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(keywords))
                || SqlFunc.ToString(x.ServiceOrderId).Contains(keywords)
                || (x.ServiceOrderCode != null && x.ServiceOrderCode.Contains(keywords))
                || SqlFunc.ToString(x.ServiceContractId).Contains(keywords)
                || (x.ServiceContractCode != null && x.ServiceContractCode.Contains(keywords))
                || SqlFunc.ToString(x.TicketType).Contains(keywords)
                || SqlFunc.ToString(x.Priority).Contains(keywords)
                || SqlFunc.ToString(x.TicketStatus).Contains(keywords)
                || (x.TicketSubject != null && x.TicketSubject.Contains(keywords))
                || (x.FaultDescription != null && x.FaultDescription.Contains(keywords))
                || (x.SolutionDescription != null && x.SolutionDescription.Contains(keywords))
                || (x.ServiceLocation != null && x.ServiceLocation.Contains(keywords))
                || SqlFunc.ToString(x.AssignedEmployeeId).Contains(keywords)
                || (x.AssignedEmployeeName != null && x.AssignedEmployeeName.Contains(keywords))
                || SqlFunc.ToString(x.AcceptanceResult).Contains(keywords)
                || (x.AcceptedBy != null && x.AcceptedBy.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ScheduledStartTime).Contains(keywords)
                || SqlFunc.ToString(x.ScheduledEndTime).Contains(keywords)
                || SqlFunc.ToString(x.ActualStartTime).Contains(keywords)
                || SqlFunc.ToString(x.ActualEndTime).Contains(keywords)
                || SqlFunc.ToString(x.AcceptedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceTicketCode))
        {
            exp = exp.And(x => x.ServiceTicketCode != null && x.ServiceTicketCode.Contains(queryDto.ServiceTicketCode));
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

        if (queryDto?.ServiceRequestId.HasValue == true)
        {
            exp = exp.And(x => x.ServiceRequestId == queryDto.ServiceRequestId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceRequestCode))
        {
            exp = exp.And(x => x.ServiceRequestCode != null && x.ServiceRequestCode.Contains(queryDto.ServiceRequestCode));
        }

        if (queryDto?.ServiceOrderId.HasValue == true)
        {
            exp = exp.And(x => x.ServiceOrderId == queryDto.ServiceOrderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceOrderCode))
        {
            exp = exp.And(x => x.ServiceOrderCode != null && x.ServiceOrderCode.Contains(queryDto.ServiceOrderCode));
        }

        if (queryDto?.ServiceContractId.HasValue == true)
        {
            exp = exp.And(x => x.ServiceContractId == queryDto.ServiceContractId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceContractCode))
        {
            exp = exp.And(x => x.ServiceContractCode != null && x.ServiceContractCode.Contains(queryDto.ServiceContractCode));
        }

        if (queryDto?.TicketType.HasValue == true)
        {
            exp = exp.And(x => x.TicketType == queryDto.TicketType);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            exp = exp.And(x => x.Priority == queryDto.Priority);
        }

        if (queryDto?.TicketStatus.HasValue == true)
        {
            exp = exp.And(x => x.TicketStatus == queryDto.TicketStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.TicketSubject))
        {
            exp = exp.And(x => x.TicketSubject != null && x.TicketSubject.Contains(queryDto.TicketSubject));
        }

        if (!string.IsNullOrEmpty(queryDto?.FaultDescription))
        {
            exp = exp.And(x => x.FaultDescription != null && x.FaultDescription.Contains(queryDto.FaultDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.SolutionDescription))
        {
            exp = exp.And(x => x.SolutionDescription != null && x.SolutionDescription.Contains(queryDto.SolutionDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.ServiceLocation))
        {
            exp = exp.And(x => x.ServiceLocation != null && x.ServiceLocation.Contains(queryDto.ServiceLocation));
        }

        if (queryDto?.AssignedEmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.AssignedEmployeeId == queryDto.AssignedEmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.AssignedEmployeeName))
        {
            exp = exp.And(x => x.AssignedEmployeeName != null && x.AssignedEmployeeName.Contains(queryDto.AssignedEmployeeName));
        }

        if (queryDto?.AcceptanceResult.HasValue == true)
        {
            exp = exp.And(x => x.AcceptanceResult == queryDto.AcceptanceResult);
        }

        if (!string.IsNullOrEmpty(queryDto?.AcceptedBy))
        {
            exp = exp.And(x => x.AcceptedBy != null && x.AcceptedBy.Contains(queryDto.AcceptedBy));
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

        if (queryDto?.ScheduledStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledStartTime >= queryDto.ScheduledStartTimeStart);
        }

        if (queryDto?.ScheduledStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledStartTime <= queryDto.ScheduledStartTimeEnd);
        }

        if (queryDto?.ScheduledEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledEndTime >= queryDto.ScheduledEndTimeStart);
        }

        if (queryDto?.ScheduledEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ScheduledEndTime <= queryDto.ScheduledEndTimeEnd);
        }

        if (queryDto?.ActualStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualStartTime >= queryDto.ActualStartTimeStart);
        }

        if (queryDto?.ActualStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualStartTime <= queryDto.ActualStartTimeEnd);
        }

        if (queryDto?.ActualEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ActualEndTime >= queryDto.ActualEndTimeStart);
        }

        if (queryDto?.ActualEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ActualEndTime <= queryDto.ActualEndTimeEnd);
        }

        if (queryDto?.AcceptedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.AcceptedAt >= queryDto.AcceptedAtStart);
        }

        if (queryDto?.AcceptedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.AcceptedAt <= queryDto.AcceptedAtEnd);
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
