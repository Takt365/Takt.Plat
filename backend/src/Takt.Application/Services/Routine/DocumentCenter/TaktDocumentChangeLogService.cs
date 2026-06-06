// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.DocumentCenter
// 文件名称：TaktDocumentChangeLogService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：文管文档变更日志应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.DocumentCenter;
using Takt.Domain.Entities.Routine.DocumentCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.Routine.DocumentCenter;

namespace Takt.Application.Services.Routine.DocumentCenter;

/// <summary>
/// 文管文档变更日志应用服务
/// </summary>
public class TaktDocumentChangeLogService : TaktServiceBase, ITaktDocumentChangeLogService
{
    private readonly ITaktCompanyRepository<TaktDocumentChangeLog> _documentChangeLogRepository;
    private readonly ITaktApprovalRepository<TaktDocument> _documentRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="documentChangeLogRepository">文管文档变更日志仓储</param>
    /// <param name="documentRepository">文管中心仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDocumentChangeLogService(
        ITaktCompanyRepository<TaktDocumentChangeLog> documentChangeLogRepository,
        ITaktApprovalRepository<TaktDocument> documentRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _documentChangeLogRepository = documentChangeLogRepository;
        _documentRepository = documentRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取文管文档变更日志列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDocumentChangeLogDto>> GetDocumentChangeLogListAsync(TaktDocumentChangeLogQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _documentChangeLogRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDocumentChangeLogDto>.Create(
            data.Adapt<List<TaktDocumentChangeLogDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取文管文档变更日志
    /// </summary>
    /// <param name="id">文管文档变更日志ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentChangeLogDto?> GetDocumentChangeLogByIdAsync(long id)
    {
        var entity = await _documentChangeLogRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktDocumentChangeLogDto>();
    }

    /// <summary>
    /// 获取文管文档变更日志选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDocumentChangeLogOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _documentChangeLogRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.DocumentCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DocumentCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建文管文档变更日志
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentChangeLogDto> CreateDocumentChangeLogAsync(TaktDocumentChangeLogCreateDto dto)
    {
        var entity = dto.Adapt<TaktDocumentChangeLog>();
                await StampDocumentChangeLogDocumentAsync(entity, dto);
        entity = await _documentChangeLogRepository.CreateAsync(entity);
        return await GetDocumentChangeLogByIdAsync(entity.Id) ?? entity.Adapt<TaktDocumentChangeLogDto>();
    }

    /// <summary>
    /// 更新文管文档变更日志
    /// </summary>
    /// <param name="id">文管文档变更日志ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentChangeLogDto> UpdateDocumentChangeLogAsync(long id, TaktDocumentChangeLogUpdateDto dto)
    {
        var entity = await _documentChangeLogRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文管文档变更日志不存在");
        }
        dto.Adapt(entity);
                await StampDocumentChangeLogDocumentAsync(entity, dto);
        await _documentChangeLogRepository.UpdateAsync(entity);
        return await GetDocumentChangeLogByIdAsync(id) ?? throw new TaktBusinessException("文管文档变更日志不存在");
    }

    /// <summary>
    /// 删除文管文档变更日志
    /// </summary>
    /// <param name="id">文管文档变更日志ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDocumentChangeLogByIdAsync(long id)
    {
        var deleted = await _documentChangeLogRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("文管文档变更日志不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除文管文档变更日志
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDocumentChangeLogBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteDocumentChangeLogByIdAsync(id);
        }
    }

    /// <summary>
    /// 导出文管文档变更日志
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDocumentChangeLogAsync(TaktDocumentChangeLogQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktDocumentChangeLogQueryDto());
        var list = await _documentChangeLogRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDocumentChangeLogExportDto>(),
                sheetName ?? "文管文档变更日志数据",
                fileName ?? "文管文档变更日志导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDocumentChangeLogExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "文管文档变更日志数据",
            fileName ?? "文管文档变更日志导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步文管文档变更日志主表外键（ManyToOne → 文管中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampDocumentChangeLogDocumentAsync(TaktDocumentChangeLog entity, TaktDocumentChangeLogCreateDto dto)
    {
        if (dto.DocumentId <= 0)
        {
            return;
        }
        var master = await _documentRepository.GetByIdAsync(dto.DocumentId);
        if (master == null)
        {
            throw new TaktBusinessException("文管中心不存在");
        }
        entity.DocumentId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建文管文档变更日志查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDocumentChangeLog, bool>> QueryExpression(TaktDocumentChangeLogQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDocumentChangeLog>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.DocumentId).Contains(keywords)
                || (x.DocumentCode != null && x.DocumentCode.Contains(keywords))
                || (x.DocumentTitle != null && x.DocumentTitle.Contains(keywords))
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

        if (queryDto?.DocumentId.HasValue == true)
        {
            exp = exp.And(x => x.DocumentId == queryDto.DocumentId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentCode))
        {
            exp = exp.And(x => x.DocumentCode != null && x.DocumentCode.Contains(queryDto.DocumentCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentTitle))
        {
            exp = exp.And(x => x.DocumentTitle != null && x.DocumentTitle.Contains(queryDto.DocumentTitle));
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
