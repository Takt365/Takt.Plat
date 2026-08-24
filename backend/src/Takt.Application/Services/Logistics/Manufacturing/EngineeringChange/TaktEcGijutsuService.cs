// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGijutsuService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Repositories;
using Takt.Domain.Interfaces;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Constants;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变技术课主应用服务
/// </summary>
public class TaktEcGijutsuService : TaktServiceBase, ITaktEcGijutsuService
{
    private readonly ITaktCompanyRepository<TaktEcGijutsu> _ecEngRepository;
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly ITaktCompanyRepository<TaktEcAttachment> _ecAttachmentRepository;
    private readonly ITaktApprovalRepository<TaktEcNotification> _ecNotificationRepository;
    private readonly ITaktCompanyRepository<TaktSourceEc> _sourceEcRepository;
    private readonly ITaktCompanyRepository<TaktSourceEcDetail> _sourceEcDetailRepository;
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktCompanyRepository<TaktDept> _deptRepository;
    private readonly TaktEcExecPersistence _ecExecPersistence;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecEngRepository">设变技术课主仓储</param>
    /// <param name="ecDetailRepository">EcDetail仓储</param>
    /// <param name="ecAttachmentRepository">EcAttachment仓储</param>
    /// <param name="ecNotificationRepository">EcNotification仓储</param>
    /// <param name="sourceEcRepository">设变来源主仓储</param>
    /// <param name="sourceEcDetailRepository">设变来源子仓储</param>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="deptRepository">部门仓储</param>
    /// <param name="ecExecPersistence">设变部门执行持久化</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="configuration">应用配置</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcGijutsuService(
        ITaktCompanyRepository<TaktEcGijutsu> ecEngRepository,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktCompanyRepository<TaktEcAttachment> ecAttachmentRepository,
        ITaktApprovalRepository<TaktEcNotification> ecNotificationRepository,
        ITaktCompanyRepository<TaktSourceEc> sourceEcRepository,
        ITaktCompanyRepository<TaktSourceEcDetail> sourceEcDetailRepository,
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktCompanyRepository<TaktDept> deptRepository,
        TaktEcExecPersistence ecExecPersistence,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        IConfiguration configuration,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecEngRepository = ecEngRepository;
        _ecDetailRepository = ecDetailRepository;
        _ecAttachmentRepository = ecAttachmentRepository;
        _ecNotificationRepository = ecNotificationRepository;
        _sourceEcRepository = sourceEcRepository;
        _sourceEcDetailRepository = sourceEcDetailRepository;
        _materialPlantRepository = materialPlantRepository;
        _deptRepository = deptRepository;
        _ecExecPersistence = ecExecPersistence;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
        _configuration = configuration;
    }

    /// <summary>
    /// 获取设变技术课主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcGijutsuDto>> GetEcGijutsuListAsync(TaktEcGijutsuQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecEngRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcGijutsuDto>.Create(
            data.Adapt<List<TaktEcGijutsuDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGijutsuDto?> GetEcGijutsuByIdAsync(long id)
    {
        var entity = await _ecEngRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktEcGijutsuDto>();
        await FillEcGijutsuDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取设变技术课主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcGijutsuOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ecEngRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ChangeStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建设变技术课主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGijutsuDto> CreateEcGijutsuAsync(TaktEcGijutsuCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcGijutsu>();
        var isUnique_ix_takt_logistics_manufacturing_ec_gijutsu_plant_ec_code_unique = await _uniqueValidator.IsUniqueAsync(
            _ecEngRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EcCode == entity.EcCode);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_gijutsu_plant_ec_code_unique)
        {
            throw new TaktBusinessException("设变技术课主的PlantCode、EcCode已存在");
        }
        entity = await _ecEngRepository.CreateAsync(entity);
        await SaveEcGijutsuChildrenAsync(entity, dto);
        var hasEcDetails = dto.EcDetails is { Count: > 0 };
        var hasExplicitNotifications = dto.Notifications is { Count: > 0 };
        if (hasEcDetails && !hasExplicitNotifications)
        {
            var notificationDate = dto.EcEntryDate != default ? dto.EcEntryDate : DateTime.Today;
            await FinalizeEcGijutsuPhaseOneAsync(entity, notificationDate);
        }
        return await GetEcGijutsuByIdAsync(entity.Id) ?? entity.Adapt<TaktEcGijutsuDto>();
    }

    /// <summary>
    /// 更新设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGijutsuDto> UpdateEcGijutsuAsync(long id, TaktEcGijutsuUpdateDto dto)
    {
        var entity = await _ecEngRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变技术课主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_ec_gijutsu_plant_ec_code_unique = await _uniqueValidator.IsUniqueAsync(
            _ecEngRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EcCode == entity.EcCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_gijutsu_plant_ec_code_unique)
        {
            throw new TaktBusinessException("设变技术课主的PlantCode、EcCode已存在");
        }
        await _ecEngRepository.UpdateAsync(entity);
                await SaveEcGijutsuChildrenAsync(entity, dto);
        return await GetEcGijutsuByIdAsync(id) ?? throw new TaktBusinessException("设变技术课主不存在");
    }

    /// <summary>
    /// 删除设变技术课主
    /// </summary>
    /// <param name="id">设变技术课主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcGijutsuByIdAsync(long id)
    {
        var entity = await _ecEngRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变技术课主不存在或已删除");
        }
        await _ecDetailRepository.DeleteAsync(x => x.EcId == entity.Id);
        await _ecAttachmentRepository.DeleteAsync(x => x.EcId == entity.Id);
        await _ecNotificationRepository.DeleteAsync(x => x.EcId == entity.Id);
        var deleted = await _ecEngRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设变技术课主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设变技术课主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcGijutsuBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcGijutsuByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变技术课主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcGijutsuDto> UpdateEcGijutsuStatusAsync(TaktEcGijutsuStatusDto dto)
    {
        var entity = await _ecEngRepository.GetByIdAsync(dto.EcGijutsuId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变技术课主不存在");
        }
        entity.ChangeStatus = dto.ChangeStatus;
        await _ecEngRepository.UpdateAsync(entity);
        return await GetEcGijutsuByIdAsync(dto.EcGijutsuId) ?? throw new TaktBusinessException("设变技术课主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcGijutsuTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcGijutsuTemplateDto>(
            sheetName ?? "设变技术课主导入模板",
            fileName ?? "设变技术课主导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变技术课主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcGijutsuAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcGijutsuImportDto>(fileStream, sheetName ?? "设变技术课主导入模板");
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
                var entity = rows[i].Adapt<TaktEcGijutsu>();
                var importKey = $"{entity.PlantCode}|{entity.EcCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、EcCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_gijutsu_plant_ec_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecEngRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.EcCode == entity.EcCode);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_gijutsu_plant_ec_code_unique)
                {
                    throw new TaktBusinessException("设变技术课主的PlantCode、EcCode已存在");
                }
                await _ecEngRepository.CreateAsync(entity);
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
    /// 导出设变技术课主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcGijutsuAsync(TaktEcGijutsuQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcGijutsuQueryDto());
        var list = await _ecEngRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcGijutsuExportDto>(),
                sheetName ?? "设变技术课主数据",
                fileName ?? "设变技术课主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcGijutsuExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变技术课主数据",
            fileName ?? "设变技术课主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充设变技术课主详情（加载 OneToMany 子表：设变明细、设变附件、工程变更通知单）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillEcGijutsuDetailsAsync(TaktEcGijutsuDto dto, TaktEcGijutsu entity)
    {
        if (dto == null)
        {
            return;
        }
        // 设变明细 → dto.EcDetails
        var ecdetails = await _ecDetailRepository.GetListAsync(x => x.EcId == entity.Id);
        dto.EcDetails = ecdetails.Adapt<List<TaktEcDetailDto>>();
        // 设变附件 → dto.Attachments
        var attachments = await _ecAttachmentRepository.GetListAsync(x => x.EcId == entity.Id);
        dto.Attachments = attachments.Adapt<List<TaktEcAttachmentDto>>();
        // 工程变更通知单 → dto.Notifications
        var notifications = await _ecNotificationRepository.GetListAsync(x => x.EcId == entity.Id);
        dto.Notifications = notifications.Adapt<List<TaktEcNotificationDto>>();
    }

    /// <summary>
    /// 保存设变技术课主子表级联（设变明细、设变附件、工程变更通知单；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveEcGijutsuChildrenAsync(TaktEcGijutsu entity, TaktEcGijutsuCreateDto dto)
    {
        // 设变明细（EcDetails）
        if (dto.EcDetails is not { Count: > 0 })
        {
            await _ecDetailRepository.DeleteAsync(x => x.EcId == entity.Id);
        }
        else
        {
            var ecdetails = dto.EcDetails.Adapt<List<TaktEcDetail>>();
            foreach (var child in ecdetails)
            {
                child.EcId = entity.Id;
            }
            var ecdetailsNeedLine = ecdetails.Where(c => c.LineNumber <= 0).ToList();
            if (ecdetailsNeedLine.Count > 0)
            {
                var businessCode = entity.Id.ToString();
                var maxLine = await _ecDetailRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, ecdetailsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in ecdetails)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < ecdetails.Count; i++)
                        {
                            var key = $"{ecdetails[i].CompanyCode}|{ecdetails[i].EcId}|{ecdetails[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"设变明细第{i + 1}项与本次提交的其他项重复（CompanyCode、EcId、LineNumber）");
                            }
                        }
            await _ecDetailRepository.DeleteAsync(x => x.EcId == entity.Id);
            foreach (var child in ecdetails)
            {
            var isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                _ecDetailRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.EcId == child.EcId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_manufacturing_ec_detail_line_unique)
            {
                throw new TaktBusinessException("设变明细的CompanyCode、EcId、LineNumber已存在");
            }
            }
            await _ecDetailRepository.CreateRangeAsync(ecdetails);
        }
        // 设变附件（Attachments）
        if (dto.Attachments is not { Count: > 0 })
        {
            await _ecAttachmentRepository.DeleteAsync(x => x.EcId == entity.Id);
        }
        else
        {
            var attachments = dto.Attachments.Adapt<List<TaktEcAttachment>>();
            foreach (var child in attachments)
            {
                child.EcId = entity.Id;
            }
            var attachmentsNeedLine = attachments.Where(c => c.LineNumber <= 0).ToList();
            if (attachmentsNeedLine.Count > 0)
            {
                var businessCode = entity.Id.ToString();
                var maxLine = await _ecAttachmentRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcId == entity.Id,
                    x => x.LineNumber);
                var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, attachmentsNeedLine.Count, maxLine).ToList();
                var lineIdx = 0;
                foreach (var child in attachments)
                {
                    if (child.LineNumber <= 0)
                    {
                        child.LineNumber = lineSeq[lineIdx++];
                    }
                }
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < attachments.Count; i++)
                        {
                            var key = $"{attachments[i].CompanyCode}|{attachments[i].EcId}|{attachments[i].LineNumber}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"设变附件第{i + 1}项与本次提交的其他项重复（CompanyCode、EcId、LineNumber）");
                            }
                        }
            await _ecAttachmentRepository.DeleteAsync(x => x.EcId == entity.Id);
            foreach (var child in attachments)
            {
            var isUnique_ix_takt_logistics_manufacturing_ec_attachment_line_unique = await _uniqueValidator.IsUniqueAsync(
                _ecAttachmentRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.EcId == child.EcId
                    && x.LineNumber == child.LineNumber);
            if (!isUnique_ix_takt_logistics_manufacturing_ec_attachment_line_unique)
            {
                throw new TaktBusinessException("设变附件的CompanyCode、EcId、LineNumber已存在");
            }
            }
            await _ecAttachmentRepository.CreateRangeAsync(attachments);
        }
        // 工程变更通知单（Notifications）
        if (dto.Notifications is not { Count: > 0 })
        {
            await _ecNotificationRepository.DeleteAsync(x => x.EcId == entity.Id);
        }
        else
        {
            var notifications = dto.Notifications.Adapt<List<TaktEcNotification>>();
            foreach (var child in notifications)
            {
                child.EcId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < notifications.Count; i++)
                        {
                            var key = $"{notifications[i].CompanyCode}|{notifications[i].EcNotificationCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"工程变更通知单第{i + 1}项与本次提交的其他项重复（CompanyCode、EcNotificationCode）");
                            }
                        }
            await _ecNotificationRepository.DeleteAsync(x => x.EcId == entity.Id);
            foreach (var child in notifications)
            {
            var isUnique_ix_takt_logistics_manufacturing_ec_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
                _ecNotificationRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.EcNotificationCode == child.EcNotificationCode);
            if (!isUnique_ix_takt_logistics_manufacturing_ec_notification_code_unique)
            {
                throw new TaktBusinessException("工程变更通知单的CompanyCode、EcNotificationCode已存在");
            }
            }
            await _ecNotificationRepository.CreateRangeAsync(notifications);
        }
    }

    /// <summary>
    /// 获取设变技术课主表统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>设变统计</returns>
    public async Task<TaktEcGijutsuStatDto> GetEcGijutsuStatAsync(TaktEcGijutsuStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.EcEntryDateStart,
            queryDto.EcEntryDateEnd);
        var ecIdsInRange = (await _ecEngRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.EcEntryDate >= start
            && x.EcEntryDate <= end))
            .Select(x => x.Id)
            .ToHashSet();
        var ecDetails = ecIdsInRange.Count == 0
            ? []
            : await _ecDetailRepository.GetListAsync(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && ecIdsInRange.Contains(x.EcId));
        if (!string.IsNullOrEmpty(queryDto.PlantCode))
        {
            var plantEcIds = (await _ecEngRepository.GetListAsync(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == queryDto.PlantCode))
                .Select(x => x.Id)
                .ToHashSet();
            ecDetails = ecDetails.Where(x => plantEcIds.Contains(x.EcId)).ToList();
        }
        return new TaktEcGijutsuStatDto
        {
            StatMonth = statMonth,
            EcCount = ecDetails.Select(x => x.EcId).Distinct().Count(),
            EcDetailCount = ecDetails.Count,
            PlantCode = queryDto.PlantCode,
        };
    }

    // ========================================
    // 来源设变录入
    // ========================================

    /// <summary>
    /// 获取当前公司对应的来源设变目标工厂代码（Database:CompanyCodes 与 PlantCodes 同序映射）
    /// </summary>
    /// <returns>公司代码与映射工厂代码</returns>
    public Task<TaktEcGijutsuSourcePlantCodeDto> GetEcGijutsuSourcePlantCodeAsync()
    {
        EnsureThreeLayerContext();
        var companyCode = CurrentCompanyCode?.Trim() ?? string.Empty;
        if (companyCode.Length == 0)
        {
            throw new TaktBusinessException("当前公司代码不能为空");
        }
        var plantCode = ResolvePlantCodeFromSourceEcCompanyCode(companyCode);
        return Task.FromResult(new TaktEcGijutsuSourcePlantCodeDto
        {
            CompanyCode = companyCode,
            PlantCode = plantCode,
        });
    }

    /// <summary>
    /// 获取尚未导入设变技术课主的来源设变列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcGijutsuSourceEcInputItemDto>> GetUnimportedSourceEcGijutsuListAsync(TaktEcGijutsuSourceEcInputQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var plantCode = ResolvePlantCodeFromSourceEcCompanyCode(CurrentCompanyCode);
        AssertRequestedPlantCodeMatches(queryDto.PlantCode, plantCode, CurrentCompanyCode);
        var importedEcCodes = (await _ecEngRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode))
            .Select(x => x.EcCode)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var predicate = UnimportedSourceEcQueryExpression(queryDto, importedEcCodes);
        var (data, total) = await _sourceEcRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.SourceIssueDate,
            false);
        if (data == null || data.Count == 0)
        {
            return TaktPagedResult<TaktEcGijutsuSourceEcInputItemDto>.Create(
                [],
                total,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var sourceEcIds = data.Select(x => x.Id).ToList();
        var sourceDetails = await _sourceEcDetailRepository.GetListAsync(x => sourceEcIds.Contains(x.SourceEcId));
        var detailCountMap = sourceDetails
            .GroupBy(x => x.SourceEcId)
            .ToDictionary(g => g.Key, g => g.Count());
        var items = data.Select(source => new TaktEcGijutsuSourceEcInputItemDto
        {
            SourceEcId = source.Id,
            SourceEcCode = source.SourceEcCode,
            SourceModel = source.SourceModel,
            SourceTitle = source.SourceTitle,
            SourceIssueDate = source.SourceIssueDate,
            SourceStatus = source.SourceStatus,
            SourceTcjOwner = source.SourceTcjOwner,
            DetailCount = detailCountMap.GetValueOrDefault(source.Id),
        }).ToList();
        return TaktPagedResult<TaktEcGijutsuSourceEcInputItemDto>.Create(
            items,
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 从来源设变导入设变技术课主、明细、附件，并初始化各部门执行行与设变通知
    /// </summary>
    /// <param name="dto">导入 DTO</param>
    /// <returns>导入结果</returns>
    public async Task<TaktEcGijutsuImportFromSourceResultDto> ImportEcGijutsuFromSourceAsync(TaktEcGijutsuImportFromSourceDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        if (dto.SourceEcIds == null || dto.SourceEcIds.Count == 0)
        {
            throw new TaktBusinessException("请至少选择一条来源设变");
        }
        var result = new TaktEcGijutsuImportFromSourceResultDto();
        var today = DateTime.Today;
        foreach (var rawId in dto.SourceEcIds.Distinct(StringComparer.Ordinal))
        {
            if (!long.TryParse(rawId, out var sourceEcId) || sourceEcId <= 0)
            {
                result.FailCount += 1;
                result.Errors.Add($"来源设变ID无效: {rawId}");
                continue;
            }
            try
            {
                var sourceEc = await _sourceEcRepository.GetByIdAsync(sourceEcId);
                if (sourceEc == null
                    || sourceEc.TenantCode != CurrentTenantCode
                    || sourceEc.CompanyCode != CurrentCompanyCode)
                {
                    throw new TaktBusinessException($"来源设变不存在: {sourceEcId}");
                }
                var plantCode = ResolvePlantCodeFromSourceEcCompanyCode(sourceEc.CompanyCode);
                AssertRequestedPlantCodeMatches(dto.PlantCode, plantCode, sourceEc.CompanyCode);
                var alreadyImported = await _ecEngRepository.CountAsync(x =>
                    x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && x.EcCode == sourceEc.SourceEcCode) > 0;
                if (alreadyImported)
                {
                    throw new TaktBusinessException($"设变 {sourceEc.SourceEcCode} 已在工厂 {plantCode} 导入");
                }
                var sourceDetails = await _sourceEcDetailRepository.GetListAsync(x => x.SourceEcId == sourceEc.Id);
                var createDto = await BuildEcGijutsuCreateFromSourceAsync(
                    sourceEc,
                    sourceDetails,
                    plantCode,
                    dto.CultureCode,
                    today);
                var created = await CreateEcGijutsuAsync(createDto);
                result.SuccessCount += 1;
                result.CreatedEcGijutsuIds.Add(created.EcGijutsuId.ToString());
            }
            catch (Exception ex)
            {
                result.FailCount += 1;
                result.Errors.Add(ex.Message);
            }
        }
        return result;
    }

    /// <summary>
    /// 从来源设变构建创建草稿 DTO（不落库；负责人与管理区分须在前端 ec-form 填写后再 create）
    /// </summary>
    /// <param name="dto">草稿请求 DTO</param>
    /// <returns>创建 DTO</returns>
    public async Task<TaktEcGijutsuCreateDto> GetEcGijutsuDraftFromSourceEcAsync(TaktEcGijutsuDraftFromSourceDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        EnsureThreeLayerContext();
        if (!long.TryParse(dto.SourceEcId, out var sourceEcId) || sourceEcId <= 0)
        {
            throw new TaktBusinessException("来源设变ID无效");
        }
        var sourceEc = await _sourceEcRepository.GetByIdAsync(sourceEcId);
        if (sourceEc == null
            || sourceEc.TenantCode != CurrentTenantCode
            || sourceEc.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException($"来源设变不存在: {sourceEcId}");
        }
        var plantCode = ResolvePlantCodeFromSourceEcCompanyCode(sourceEc.CompanyCode);
        AssertRequestedPlantCodeMatches(dto.PlantCode, plantCode, sourceEc.CompanyCode);
        var alreadyImported = await _ecEngRepository.CountAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.EcCode == sourceEc.SourceEcCode) > 0;
        if (alreadyImported)
        {
            throw new TaktBusinessException($"设变 {sourceEc.SourceEcCode} 已在工厂 {plantCode} 导入");
        }
        var sourceDetails = await _sourceEcDetailRepository.GetListAsync(x => x.SourceEcId == sourceEc.Id);
        var createDto = await BuildEcGijutsuCreateFromSourceAsync(
            sourceEc,
            sourceDetails,
            plantCode,
            dto.CultureCode,
            DateTime.Today);
        createDto.EcLeader = string.Empty;
        createDto.EcDistinction = 0;
        return createDto;
    }

    /// <summary>
    /// 由来源设变公司代码解析目标工厂代码（Database:CompanyCodes 与 PlantCodes 同序下标映射）
    /// </summary>
    /// <param name="companyCode">来源设变公司代码</param>
    /// <returns>工厂代码</returns>
    private string ResolvePlantCodeFromSourceEcCompanyCode(string? companyCode)
    {
        var code = companyCode?.Trim() ?? string.Empty;
        if (code.Length == 0)
        {
            throw new TaktBusinessException("来源设变公司代码不能为空，无法解析工厂代码");
        }
        try
        {
            return _configuration.RequireDatabase().GetPlantCodeForCompanyCode(code);
        }
        catch (InvalidOperationException ex)
        {
            throw new TaktBusinessException(ex.Message);
        }
    }

    /// <summary>
    /// 校验前端传入的工厂代码与来源设变公司代码映射结果一致
    /// </summary>
    /// <param name="requestedPlantCode">前端传入的工厂代码（可为空）</param>
    /// <param name="resolvedPlantCode">服务端解析的工厂代码</param>
    /// <param name="companyCode">来源设变公司代码</param>
    private static void AssertRequestedPlantCodeMatches(string? requestedPlantCode, string resolvedPlantCode, string companyCode)
    {
        if (string.IsNullOrWhiteSpace(requestedPlantCode))
        {
            return;
        }
        if (!string.Equals(requestedPlantCode.Trim(), resolvedPlantCode, StringComparison.OrdinalIgnoreCase))
        {
            throw new TaktBusinessException($"工厂代码须与公司代码映射一致（{companyCode}→{resolvedPlantCode}）");
        }
    }

    /// <summary>
    /// 将来源设变主从映射为设变技术课创建 DTO（SourceTcjOwner 不写入 EcLeader，须导入后手工指定负责人）
    /// </summary>
    /// <param name="sourceEc">来源设变主</param>
    /// <param name="sourceDetails">来源设变明细</param>
    /// <param name="plantCode">目标工厂</param>
    /// <param name="companyDefaultCulture">公司默认文化</param>
    /// <param name="entryDate">录入日期</param>
    /// <returns>创建 DTO</returns>
    private async Task<TaktEcGijutsuCreateDto> BuildEcGijutsuCreateFromSourceAsync(
        TaktSourceEc sourceEc,
        List<TaktSourceEcDetail> sourceDetails,
        string plantCode,
        string? companyDefaultCulture,
        DateTime entryDate)
    {
        var ecCode = sourceEc.SourceEcCode ?? string.Empty;
        var materialCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var detail in sourceDetails)
        {
            TaktEcDetailMaterialPlantMapper.CollectMaterialCode(detail.SourceFinishedProduct, materialCodes);
            TaktEcDetailMaterialPlantMapper.CollectMaterialCode(detail.SourceParentPart, materialCodes);
            TaktEcDetailMaterialPlantMapper.CollectMaterialCode(detail.SourceLegacyPartCode, materialCodes);
            TaktEcDetailMaterialPlantMapper.CollectMaterialCode(detail.SourceReplacementPartCode, materialCodes);
        }
        var materialsByCode = await LoadMaterialPlantsByCodesAsync(plantCode, materialCodes);
        var lineNumber = 0;
        var ecDetails = new List<TaktEcDetailCreateDto>(sourceDetails.Count);
        foreach (var detail in sourceDetails.OrderBy(x => x.Id))
        {
            lineNumber += 10;
            var dto = new TaktEcDetailCreateDto
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                CultureCode = companyDefaultCulture ?? string.Empty,
                EcCode = ecCode,
                LineNumber = lineNumber,
                EcModel = sourceEc.SourceModel,
                EcBomItem = detail.SourceFinishedProduct,
                EcBomSubItem = detail.SourceParentPart,
                EcOldItem = detail.SourceLegacyPartCode,
                EcOldText = detail.SourceLegacyPartName,
                EcOldUsage = detail.SourceLegacyUsage,
                EcOldPosition = detail.SourceLegacyMountingPosition,
                EcNewItem = detail.SourceReplacementPartCode,
                EcNewText = detail.SourceReplacementPartName,
                EcNewUsage = detail.SourceReplacementUsage,
                EcNewPosition = detail.SourceReplacementMountingPosition,
                EcBomLineCode = detail.SourceBomCode,
                EcIsCompatible = detail.SourceCompatibility,
                EcSecondDistinction = detail.SourceDistinction,
                EcInstruction = detail.SourceInstruction,
                EcLegacyPartDisposition = detail.SourceLegacyPartDisposition,
                EcBomDate = detail.SourceBomEffectiveDate ?? sourceEc.SourceIssueDate,
            };
            TaktEcDetailMaterialPlantMapper.EnrichCreateDto(dto, materialsByCode);
            ecDetails.Add(dto);
        }
        return new TaktEcGijutsuCreateDto
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            CultureCode = companyDefaultCulture ?? string.Empty,
            PlantCode = plantCode,
            EcCode = ecCode,
            EcIssueDate = sourceEc.SourceIssueDate,
            ChangeStatus = TaktEcSourceStatusMapper.MapToChangeStatusOrThrow(sourceEc.SourceStatus),
            EcTitle = sourceEc.SourceTitle,
            EcContent = sourceEc.SourceEcContent,
            EcLeader = string.Empty,
            EcLossAmount = sourceEc.SourceUnitCost + sourceEc.SourceMoldModificationCost,
            EcDistinction = 4,
            EcEntryDate = entryDate,
            EcStatus = 1,
            EcDetails = ecDetails,
            Attachments = TaktEcSourceAttachmentMapper.MapAttachments(
                sourceEc,
                ecCode,
                CurrentTenantCode,
                CurrentCompanyCode,
                companyDefaultCulture ?? string.Empty).Adapt<List<TaktEcAttachmentCreateDto>>(),
        };
    }

    /// <summary>
    /// 技术阶段一收尾：按来源导入顺序初始化各部门执行行，并自动生成设变通知（无则创建）
    /// </summary>
    /// <param name="entity">设变技术课主</param>
    /// <param name="notificationDate">通知日期</param>
    /// <returns>任务</returns>
    private async Task FinalizeEcGijutsuPhaseOneAsync(TaktEcGijutsu entity, DateTime notificationDate)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (entity.Id <= 0)
        {
            throw new ArgumentException("设变技术课主 ID 无效", nameof(entity));
        }
        var details = await _ecDetailRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.EcId == entity.Id);
        if (details.Count == 0)
        {
            return;
        }
        await _ecExecPersistence.EnsureDeptExecRowsForDetailsInOrderAsync(
            details,
            TaktEcDeptCodes.SourceImportDeptOrder);
        var existingNotification = await _ecNotificationRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.EcId == entity.Id
            && x.IsDeleted == 0);
        if (existingNotification != null)
        {
            return;
        }
        var deptNames = await ResolveDeptDisplayNamesAsync(TaktEcDeptCodes.SourceImportDeptOrder);
        var notificationNo = BuildEcNotificationCode(entity.PlantCode, entity.EcCode);
        var notification = new TaktEcNotification
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            PlantCode = entity.PlantCode,
            EcNotificationCode = notificationNo,
            EcId = entity.Id,
            EcCode = entity.EcCode,
            EcTitle = entity.EcTitle,
            EcNotificationDate = notificationDate.Date,
            EcNotificationDeptCodes = string.Join(",", TaktEcDeptCodes.SourceImportDeptOrder),
            EcNotificationDeptNames = string.Join(",", deptNames),
            EcNotificationMethod = 2,
            EcNotificationStatus = 0,
        };
        var isUnique = await _uniqueValidator.IsUniqueAsync(
            _ecNotificationRepository,
            x => x.CompanyCode == notification.CompanyCode
                && x.EcNotificationCode == notification.EcNotificationCode);
        if (!isUnique)
        {
            throw new TaktBusinessException($"工程变更通知单号 {notificationNo} 已存在");
        }
        await _ecNotificationRepository.CreateAsync(notification);
    }

    /// <summary>
    /// 生成设变通知单号（工厂+设变单号，公司内唯一）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="ecCode">设变单号</param>
    /// <returns>通知单号</returns>
    private static string BuildEcNotificationCode(string plantCode, string ecCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(ecCode);
        return $"{plantCode.Trim()}-ECN-{ecCode.Trim()}";
    }

    /// <summary>
    /// 按部门编码顺序解析部门显示名称（缺失时回退为编码本身）
    /// </summary>
    /// <param name="deptCodes">部门编码列表</param>
    /// <returns>与编码顺序一致的部门名称列表</returns>
    private async Task<IReadOnlyList<string>> ResolveDeptDisplayNamesAsync(IReadOnlyList<string> deptCodes)
    {
        if (deptCodes == null || deptCodes.Count == 0)
        {
            return Array.Empty<string>();
        }
        var codeList = deptCodes
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToList();
        var depts = await _deptRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && codeList.Contains(x.DeptCode));
        var nameByCode = depts.ToDictionary(x => x.DeptCode, x => x.DeptName1, StringComparer.Ordinal);
        var names = new List<string>(deptCodes.Count);
        foreach (var code in deptCodes)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                continue;
            }
            var trimmed = code.Trim();
            names.Add(nameByCode.TryGetValue(trimmed, out var name) && !string.IsNullOrWhiteSpace(name)
                ? name
                : trimmed);
        }
        return names;
    }

    /// <summary>
    /// 按目标工厂与物料编码批量加载工厂物料
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="materialCodes">物料编码集合</param>
    /// <returns>物料编码 → 工厂物料</returns>
    private async Task<Dictionary<string, TaktMaterialPlant>> LoadMaterialPlantsByCodesAsync(
        string plantCode,
        IReadOnlySet<string> materialCodes)
    {
        if (materialCodes.Count == 0)
        {
            return new Dictionary<string, TaktMaterialPlant>(StringComparer.OrdinalIgnoreCase);
        }
        var codeList = materialCodes.ToList();
        var materials = await _materialPlantRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && codeList.Contains(x.MaterialCode));
        return materials.ToDictionary(x => x.MaterialCode, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 构建未导入来源设变查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <param name="importedEcCodes">目标工厂已导入的设变单号集合</param>
    /// <returns>查询表达式</returns>
    private Expression<Func<TaktSourceEc, bool>> UnimportedSourceEcQueryExpression(
        TaktEcGijutsuSourceEcInputQueryDto queryDto,
        IReadOnlySet<string> importedEcCodes)
    {
        var exp = Expressionable.Create<TaktSourceEc>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
        if (importedEcCodes.Count > 0)
        {
            exp = exp.And(x => !importedEcCodes.Contains(x.SourceEcCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceEcCode))
        {
            var sourceEcCode = queryDto.SourceEcCode.Trim();
            exp = exp.And(x => x.SourceEcCode.Contains(sourceEcCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceTitle))
        {
            var sourceTitle = queryDto.SourceTitle.Trim();
            exp = exp.And(x => x.SourceTitle.Contains(sourceTitle));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords.Trim();
            exp = exp.And(x =>
                x.SourceEcCode.Contains(keywords)
                || x.SourceModel.Contains(keywords)
                || x.SourceTitle.Contains(keywords)
                || (x.SourceTcjOwner != null && x.SourceTcjOwner.Contains(keywords))
                || (x.SourceStatus != null && x.SourceStatus.Contains(keywords)));
        }
        return exp.ToExpression();
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变技术课主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcGijutsu, bool>> QueryExpression(TaktEcGijutsuQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcGijutsu>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EcCode != null && x.EcCode.Contains(keywords))
                || SqlFunc.ToString(x.ChangeStatus).Contains(keywords)
                || (x.EcTitle != null && x.EcTitle.Contains(keywords))
                || (x.EcContent != null && x.EcContent.Contains(keywords))
                || (x.EcLeader != null && x.EcLeader.Contains(keywords))
                || SqlFunc.ToString(x.EcLossAmount).Contains(keywords)
                || SqlFunc.ToString(x.EcDistinction).Contains(keywords)
                || SqlFunc.ToString(x.EcStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EcIssueDate).Contains(keywords)
                || SqlFunc.ToString(x.EcEntryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }

        if (queryDto?.ChangeStatus.HasValue == true)
        {
            exp = exp.And(x => x.ChangeStatus == queryDto.ChangeStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcTitle))
        {
            exp = exp.And(x => x.EcTitle != null && x.EcTitle.Contains(queryDto.EcTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcContent))
        {
            exp = exp.And(x => x.EcContent != null && x.EcContent.Contains(queryDto.EcContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcLeader))
        {
            exp = exp.And(x => x.EcLeader != null && x.EcLeader.Contains(queryDto.EcLeader));
        }

        if (queryDto?.EcLossAmount.HasValue == true)
        {
            exp = exp.And(x => x.EcLossAmount == queryDto.EcLossAmount);
        }

        if (queryDto?.EcDistinction.HasValue == true)
        {
            exp = exp.And(x => x.EcDistinction == queryDto.EcDistinction);
        }

        if (queryDto?.EcStatus.HasValue == true)
        {
            exp = exp.And(x => x.EcStatus == queryDto.EcStatus);
        }


        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EcIssueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EcIssueDate >= queryDto.EcIssueDateStart);
        }

        if (queryDto?.EcIssueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EcIssueDate <= queryDto.EcIssueDateEnd);
        }

        if (queryDto?.EcEntryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EcEntryDate >= queryDto.EcEntryDateStart);
        }

        if (queryDto?.EcEntryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EcEntryDate <= queryDto.EcEntryDateEnd);
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
