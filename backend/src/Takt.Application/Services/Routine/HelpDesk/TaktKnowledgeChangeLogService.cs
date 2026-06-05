// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktKnowledgeChangeLogService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：知识库变更日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Routine.HelpDesk;

namespace Takt.Application.Services.Routine.HelpDesk;

/// <summary>
/// 知识库变更日志应用服务
/// </summary>
public class TaktKnowledgeChangeLogService : TaktServiceBase, ITaktKnowledgeChangeLogService
{
    private readonly ITaktCompanyRepository<TaktKnowledgeChangeLog> _knowledgeChangeLogRepository;
    private readonly ITaktCompanyRepository<TaktKnowledge> _knowledgeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="knowledgeChangeLogRepository">知识库变更日志仓储</param>
    /// <param name="knowledgeRepository">知识库仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktKnowledgeChangeLogService(
        ITaktCompanyRepository<TaktKnowledgeChangeLog> knowledgeChangeLogRepository,
        ITaktCompanyRepository<TaktKnowledge> knowledgeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _knowledgeChangeLogRepository = knowledgeChangeLogRepository;
        _knowledgeRepository = knowledgeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取知识库变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktKnowledgeChangeLogDto>> GetKnowledgeChangeLogListAsync(TaktKnowledgeChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _knowledgeChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktKnowledgeChangeLogDto>.Create(
            data.Adapt<List<TaktKnowledgeChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取知识库变更日志
    /// </summary>
    /// <param name="id">知识库变更日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktKnowledgeChangeLogDto?> GetKnowledgeChangeLogByIdAsync(long id)
    {
        var entity = await _knowledgeChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktKnowledgeChangeLogDto>();
    }

    /// <summary>
    /// 获取知识库变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetKnowledgeChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _knowledgeChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.KnowledgeTitle,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.KnowledgeTitle ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建知识库变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktKnowledgeChangeLogDto> CreateKnowledgeChangeLogAsync(TaktKnowledgeChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktKnowledgeChangeLog>();
                await StampKnowledgeChangeLogKnowledgeAsync(entity, dto);
        entity = await _knowledgeChangeLogRepository.CreateAsync(entity);
        return await GetKnowledgeChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktKnowledgeChangeLogDto>();
    }

    /// <summary>
    /// 更新知识库变更日志
    /// </summary>
    /// <param name="id">知识库变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktKnowledgeChangeLogDto> UpdateKnowledgeChangeLogAsync(long id, TaktKnowledgeChangeLogUpdateDto dto)
    {
        var entity = await _knowledgeChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("知识库变更日志不存在");
        }
        dto.Adapt(entity);
                await StampKnowledgeChangeLogKnowledgeAsync(entity, dto);
        await _knowledgeChangeLogRepository.UpdateAsync(entity);
        return await GetKnowledgeChangeLogByIdAsync(id) ?? throw new TaktBusinessException("知识库变更日志不存在");
    }

    /// <summary>
    /// 删除知识库变更日志
    /// </summary>
    /// <param name="id">知识库变更日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteKnowledgeChangeLogByIdAsync(long id)
    {
        var deleted = await _knowledgeChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("知识库变更日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除知识库变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteKnowledgeChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteKnowledgeChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出知识库变更日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportKnowledgeChangeLogAsync(TaktKnowledgeChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktKnowledgeChangeLogQueryDto());
        var list = await _knowledgeChangeLogRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktKnowledgeChangeLogExportDto>(),
                sheetName ?? "知识库变更日志数据",
                fileName ?? "知识库变更日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktKnowledgeChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "知识库变更日志数据",
            fileName ?? "知识库变更日志导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步知识库变更日志主表外键（ManyToOne → 知识库）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampKnowledgeChangeLogKnowledgeAsync(TaktKnowledgeChangeLog entity, TaktKnowledgeChangeLogCreateDto dto)
    {
        if (dto.KnowledgeId <= 0)
        {
            return;
        }
        var master = await _knowledgeRepository.GetByIdAsync(dto.KnowledgeId);
        if (master == null)
        {
            throw new TaktBusinessException("知识库不存在");
        }
        entity.KnowledgeId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建知识库变更日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktKnowledgeChangeLog, bool>> QueryExpression(TaktKnowledgeChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktKnowledgeChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.KnowledgeId).Contains(keywords)
                || (x.KnowledgeTitle != null && x.KnowledgeTitle.Contains(keywords))
                || SqlFunc.ToString(x.ChangeType).Contains(keywords)
                || (x.ChangeSummary != null && x.ChangeSummary.Contains(keywords))
                || (x.ChangeFields != null && x.ChangeFields.Contains(keywords))
                || (x.ChangeReason != null && x.ChangeReason.Contains(keywords))
                || SqlFunc.ToString(x.VersionAtChange).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.KnowledgeId.HasValue == true)
        {
            exp = exp.And(x => x.KnowledgeId == queryDto.KnowledgeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.KnowledgeTitle))
        {
            exp = exp.And(x => x.KnowledgeTitle != null && x.KnowledgeTitle.Contains(queryDto.KnowledgeTitle));
        }

        if (queryDto?.ChangeType.HasValue == true)
        {
            exp = exp.And(x => x.ChangeType == queryDto.ChangeType);
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeSummary))
        {
            exp = exp.And(x => x.ChangeSummary != null && x.ChangeSummary.Contains(queryDto.ChangeSummary));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeFields))
        {
            exp = exp.And(x => x.ChangeFields != null && x.ChangeFields.Contains(queryDto.ChangeFields));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeReason))
        {
            exp = exp.And(x => x.ChangeReason != null && x.ChangeReason.Contains(queryDto.ChangeReason));
        }

        if (queryDto?.VersionAtChange.HasValue == true)
        {
            exp = exp.And(x => x.VersionAtChange == queryDto.VersionAtChange);
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
