// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNoticeService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 工程变更通知单应用服务
/// </summary>
public class TaktEcNoticeService : TaktServiceBase, ITaktEcNoticeService
{
    private readonly ITaktApprovalRepository<TaktEcNotice> _ecNoticeRepository;
    private readonly ITaktCompanyRepository<TaktEc> _ecRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecNoticeRepository">工程变更通知单仓储</param>
    /// <param name="ecRepository">设变主仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcNoticeService(
        ITaktApprovalRepository<TaktEcNotice> ecNoticeRepository,
        ITaktCompanyRepository<TaktEc> ecRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecNoticeRepository = ecNoticeRepository;
        _ecRepository = ecRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工程变更通知单列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcNoticeDto>> GetEcNoticeListAsync(TaktEcNoticeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecNoticeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcNoticeDto>.Create(
            data.Adapt<List<TaktEcNoticeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcNoticeDto?> GetEcNoticeByIdAsync(long id)
    {
        var entity = await _ecNoticeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEcNoticeDto>();
    }

    /// <summary>
    /// 获取工程变更通知单选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcNoticeOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _ecNoticeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EcNoticeNotifierName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EcNoticeNotifierName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工程变更通知单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcNoticeDto> CreateEcNoticeAsync(TaktEcNoticeCreateDto dto)
    {
        var entity = dto.Adapt<TaktEcNotice>();
        await StampEcNoticeEcAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_notice_no_unique = await _uniqueValidator.IsUniqueAsync(
            _ecNoticeRepository,
            x => x.EcNoticeNo == entity.EcNoticeNo);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_notice_no_unique)
        {
            throw new TaktBusinessException("工程变更通知单的EcNoticeNo已存在");
        }
        entity = await _ecNoticeRepository.CreateAsync(entity);
        return await GetEcNoticeByIdAsync(entity.Id) ?? entity.Adapt<TaktEcNoticeDto>();
    }

    /// <summary>
    /// 更新工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcNoticeDto> UpdateEcNoticeAsync(long id, TaktEcNoticeUpdateDto dto)
    {
        var entity = await _ecNoticeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工程变更通知单不存在");
        }
        dto.Adapt(entity);
        await StampEcNoticeEcAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_ec_notice_no_unique = await _uniqueValidator.IsUniqueAsync(
            _ecNoticeRepository,
            x => x.EcNoticeNo == entity.EcNoticeNo,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_ec_notice_no_unique)
        {
            throw new TaktBusinessException("工程变更通知单的EcNoticeNo已存在");
        }
        await _ecNoticeRepository.UpdateAsync(entity);
        return await GetEcNoticeByIdAsync(id) ?? throw new TaktBusinessException("工程变更通知单不存在");
    }

    /// <summary>
    /// 删除工程变更通知单
    /// </summary>
    /// <param name="id">工程变更通知单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcNoticeByIdAsync(long id)
    {
        var deleted = await _ecNoticeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工程变更通知单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工程变更通知单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcNoticeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcNoticeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工程变更通知单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcNoticeDto> UpdateEcNoticeStatusAsync(TaktEcNoticeStatusDto dto)
    {
        var entity = await _ecNoticeRepository.GetByIdAsync(dto.EcNoticeId);
        if (entity == null)
        {
            throw new TaktBusinessException("工程变更通知单不存在");
        }
        entity.EcNoticeStatus = dto.EcNoticeStatus;
        await _ecNoticeRepository.UpdateAsync(entity);
        return await GetEcNoticeByIdAsync(dto.EcNoticeId) ?? throw new TaktBusinessException("工程变更通知单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcNoticeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcNoticeTemplateDto>(
            sheetName ?? "工程变更通知单导入模板",
            fileName ?? "工程变更通知单导入模板.xlsx");
    }

    /// <summary>
    /// 导入工程变更通知单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcNoticeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcNoticeImportDto>(fileStream, sheetName ?? "工程变更通知单导入模板");
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
                var entity = rows[i].Adapt<TaktEcNotice>();
                var importDto = rows[i].Adapt<TaktEcNoticeCreateDto>();
                await StampEcNoticeEcAsync(entity, importDto);
                var importKey = $"{entity.EcNoticeNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EcNoticeNo）");
                }
                var isUnique_ix_takt_logistics_manufacturing_ec_notice_no_unique = await _uniqueValidator.IsUniqueAsync(
                    _ecNoticeRepository,
                    x => x.EcNoticeNo == entity.EcNoticeNo);
                if (!isUnique_ix_takt_logistics_manufacturing_ec_notice_no_unique)
                {
                    throw new TaktBusinessException("工程变更通知单的EcNoticeNo已存在");
                }
                await _ecNoticeRepository.CreateAsync(entity);
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
    /// 导出工程变更通知单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcNoticeAsync(TaktEcNoticeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEcNoticeQueryDto());
        var list = await _ecNoticeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcNoticeExportDto>(),
                sheetName ?? "工程变更通知单数据",
                fileName ?? "工程变更通知单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcNoticeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工程变更通知单数据",
            fileName ?? "工程变更通知单导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步工程变更通知单主表外键（ManyToOne → 设变主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEcNoticeEcAsync(TaktEcNotice entity, TaktEcNoticeCreateDto dto)
    {
        if (dto.EcId <= 0)
        {
            return;
        }
        var master = await _ecRepository.GetByIdAsync(dto.EcId);
        if (master == null)
        {
            throw new TaktBusinessException("设变主不存在");
        }
        entity.EcId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工程变更通知单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcNotice, bool>> QueryExpression(TaktEcNoticeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcNotice>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EcNoticeNo != null && x.EcNoticeNo.Contains(keywords))
                || SqlFunc.ToString(x.EcId).Contains(keywords)
                || (x.EcNo != null && x.EcNo.Contains(keywords))
                || (x.EcTitle != null && x.EcTitle.Contains(keywords))
                || (x.EcNoticeDeptCodes != null && x.EcNoticeDeptCodes.Contains(keywords))
                || (x.EcNoticeDeptNames != null && x.EcNoticeDeptNames.Contains(keywords))
                || SqlFunc.ToString(x.EcNoticeNotifierId).Contains(keywords)
                || (x.EcNoticeNotifierName != null && x.EcNoticeNotifierName.Contains(keywords))
                || SqlFunc.ToString(x.EcNoticeMethod).Contains(keywords)
                || SqlFunc.ToString(x.EcNoticeStatus).Contains(keywords)
                || SqlFunc.ToString(x.FlowInstanceId).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EcNoticeDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNoticeNo))
        {
            exp = exp.And(x => x.EcNoticeNo != null && x.EcNoticeNo.Contains(queryDto.EcNoticeNo));
        }

        if (queryDto?.EcId.HasValue == true)
        {
            exp = exp.And(x => x.EcId == queryDto.EcId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNo))
        {
            exp = exp.And(x => x.EcNo != null && x.EcNo.Contains(queryDto.EcNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcTitle))
        {
            exp = exp.And(x => x.EcTitle != null && x.EcTitle.Contains(queryDto.EcTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNoticeDeptCodes))
        {
            exp = exp.And(x => x.EcNoticeDeptCodes != null && x.EcNoticeDeptCodes.Contains(queryDto.EcNoticeDeptCodes));
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNoticeDeptNames))
        {
            exp = exp.And(x => x.EcNoticeDeptNames != null && x.EcNoticeDeptNames.Contains(queryDto.EcNoticeDeptNames));
        }

        if (queryDto?.EcNoticeNotifierId.HasValue == true)
        {
            exp = exp.And(x => x.EcNoticeNotifierId == queryDto.EcNoticeNotifierId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EcNoticeNotifierName))
        {
            exp = exp.And(x => x.EcNoticeNotifierName != null && x.EcNoticeNotifierName.Contains(queryDto.EcNoticeNotifierName));
        }

        if (queryDto?.EcNoticeMethod.HasValue == true)
        {
            exp = exp.And(x => x.EcNoticeMethod == queryDto.EcNoticeMethod);
        }

        if (queryDto?.EcNoticeStatus.HasValue == true)
        {
            exp = exp.And(x => x.EcNoticeStatus == queryDto.EcNoticeStatus);
        }

        if (queryDto?.FlowInstanceId.HasValue == true)
        {
            exp = exp.And(x => x.FlowInstanceId == queryDto.FlowInstanceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EcNoticeDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EcNoticeDate >= queryDto.EcNoticeDateStart);
        }

        if (queryDto?.EcNoticeDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EcNoticeDate <= queryDto.EcNoticeDateEnd);
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
