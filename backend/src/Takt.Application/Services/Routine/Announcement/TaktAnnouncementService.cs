// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.Announcement
// 文件名称：TaktAnnouncementService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.Announcement;
using Takt.Application.Services.Workflow.FlowEngine.Business;
using Takt.Domain.Entities.Routine.Announcement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Routine.Announcement;

/// <summary>
/// 公告通知应用服务
/// </summary>
public class TaktAnnouncementService : TaktServiceBase, ITaktAnnouncementService
{
    private readonly ITaktApprovalRepository<TaktAnnouncement> _announcementRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly TaktApprovalFlowSubmitService _approvalFlowSubmitService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="announcementRepository">公告通知仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="approvalFlowSubmitService">通用提交审批服务</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAnnouncementService(
        ITaktApprovalRepository<TaktAnnouncement> announcementRepository,
        ITaktUniqueValidator uniqueValidator,
        TaktApprovalFlowSubmitService approvalFlowSubmitService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _announcementRepository = announcementRepository;
        _uniqueValidator = uniqueValidator;
        _approvalFlowSubmitService = approvalFlowSubmitService;
    }

    /// <summary>
    /// 获取公告通知列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAnnouncementDto>> GetAnnouncementListAsync(TaktAnnouncementQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _announcementRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAnnouncementDto>.Create(
            data.Adapt<List<TaktAnnouncementDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取公告通知
    /// </summary>
    /// <param name="id">公告通知ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAnnouncementDto?> GetAnnouncementByIdAsync(long id)
    {
        var entity = await _announcementRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAnnouncementDto>();
    }

    /// <summary>
    /// 获取公告通知选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAnnouncementOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _announcementRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.Title,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.Title ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建公告通知
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAnnouncementDto> CreateAnnouncementAsync(TaktAnnouncementCreateDto dto)
    {
        var entity = dto.Adapt<TaktAnnouncement>();
        entity = await _announcementRepository.CreateAsync(entity);
        return await GetAnnouncementByIdAsync(entity.Id) ?? entity.Adapt<TaktAnnouncementDto>();
    }

    /// <summary>
    /// 更新公告通知
    /// </summary>
    /// <param name="id">公告通知ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAnnouncementDto> UpdateAnnouncementAsync(long id, TaktAnnouncementUpdateDto dto)
    {
        var entity = await _announcementRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("公告通知不存在");
        }
        dto.Adapt(entity);
        await _announcementRepository.UpdateAsync(entity);
        return await GetAnnouncementByIdAsync(id) ?? throw new TaktBusinessException("公告通知不存在");
    }

    /// <summary>
    /// 删除公告通知
    /// </summary>
    /// <param name="id">公告通知ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAnnouncementByIdAsync(long id)
    {
        var deleted = await _announcementRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("公告通知不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除公告通知
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAnnouncementBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAnnouncementByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新公告通知状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAnnouncementDto> UpdateAnnouncementStatusAsync(TaktAnnouncementStatusDto dto)
    {
        var entity = await _announcementRepository.GetByIdAsync(dto.AnnouncementId);
        if (entity == null)
        {
            throw new TaktBusinessException("公告通知不存在");
        }
        entity.AnnouncementStatus = dto.AnnouncementStatus;
        await _announcementRepository.UpdateAsync(entity);
        return await GetAnnouncementByIdAsync(dto.AnnouncementId) ?? throw new TaktBusinessException("公告通知不存在");
    }

    /// <summary>
    /// 提交公告审批（发起 Announcement 流程）
    /// </summary>
    /// <param name="id">公告 ID</param>
    /// <returns>公告 DTO</returns>
    public async Task<TaktAnnouncementDto> SubmitAnnouncementForApprovalAsync(long id)
    {
        await _approvalFlowSubmitService.SubmitForApprovalByTableAsync("takt_routine_announcement", id);
        return await GetAnnouncementByIdAsync(id) ?? throw new TaktBusinessException("公告通知不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAnnouncementTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAnnouncementTemplateDto>(
            sheetName ?? "公告通知导入模板",
            fileName ?? "公告通知导入模板.xlsx");
    }

    /// <summary>
    /// 导入公告通知
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAnnouncementAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAnnouncementImportDto>(fileStream, sheetName ?? "公告通知导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktAnnouncement>();
                await _announcementRepository.CreateAsync(entity);
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
    /// 导出公告通知
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAnnouncementAsync(TaktAnnouncementQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAnnouncementQueryDto());
        var list = await _announcementRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAnnouncementExportDto>(),
                sheetName ?? "公告通知数据",
                fileName ?? "公告通知导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAnnouncementExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "公告通知数据",
            fileName ?? "公告通知导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建公告通知查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAnnouncement, bool>> QueryExpression(TaktAnnouncementQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAnnouncement>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.Title != null && x.Title.Contains(keywords))
                || SqlFunc.ToString(x.AnnouncementType).Contains(keywords)
                || (x.Content != null && x.Content.Contains(keywords))
                || (x.Summary != null && x.Summary.Contains(keywords))
                || (x.Tags != null && x.Tags.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || SqlFunc.ToString(x.IsScheduled).Contains(keywords)
                || SqlFunc.ToString(x.IsTop).Contains(keywords)
                || SqlFunc.ToString(x.TopPriority).Contains(keywords)
                || SqlFunc.ToString(x.ViewCount).Contains(keywords)
                || (x.TargetScope != null && x.TargetScope.Contains(keywords))
                || (x.TargetDepartments != null && x.TargetDepartments.Contains(keywords))
                || (x.TargetUsers != null && x.TargetUsers.Contains(keywords))
                || SqlFunc.ToString(x.AnnouncementStatus).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PublishTime).Contains(keywords)
                || SqlFunc.ToString(x.ExpireTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.Title))
        {
            exp = exp.And(x => x.Title != null && x.Title.Contains(queryDto.Title));
        }

        if (queryDto?.AnnouncementType.HasValue == true)
        {
            exp = exp.And(x => x.AnnouncementType == queryDto.AnnouncementType);
        }

        if (!string.IsNullOrEmpty(queryDto?.Content))
        {
            exp = exp.And(x => x.Content != null && x.Content.Contains(queryDto.Content));
        }

        if (!string.IsNullOrEmpty(queryDto?.Summary))
        {
            exp = exp.And(x => x.Summary != null && x.Summary.Contains(queryDto.Summary));
        }

        if (!string.IsNullOrEmpty(queryDto?.Tags))
        {
            exp = exp.And(x => x.Tags != null && x.Tags.Contains(queryDto.Tags));
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (queryDto?.IsScheduled.HasValue == true)
        {
            exp = exp.And(x => x.IsScheduled == queryDto.IsScheduled);
        }

        if (queryDto?.IsTop.HasValue == true)
        {
            exp = exp.And(x => x.IsTop == queryDto.IsTop);
        }

        if (queryDto?.TopPriority.HasValue == true)
        {
            exp = exp.And(x => x.TopPriority == queryDto.TopPriority);
        }

        if (queryDto?.ViewCount.HasValue == true)
        {
            exp = exp.And(x => x.ViewCount == queryDto.ViewCount);
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetScope))
        {
            exp = exp.And(x => x.TargetScope != null && x.TargetScope.Contains(queryDto.TargetScope));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetDepartments))
        {
            exp = exp.And(x => x.TargetDepartments != null && x.TargetDepartments.Contains(queryDto.TargetDepartments));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetUsers))
        {
            exp = exp.And(x => x.TargetUsers != null && x.TargetUsers.Contains(queryDto.TargetUsers));
        }

        if (queryDto?.AnnouncementStatus.HasValue == true)
        {
            exp = exp.And(x => x.AnnouncementStatus == queryDto.AnnouncementStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PublishTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PublishTime >= queryDto.PublishTimeStart);
        }

        if (queryDto?.PublishTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PublishTime <= queryDto.PublishTimeEnd);
        }

        if (queryDto?.ExpireTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpireTime >= queryDto.ExpireTimeStart);
        }

        if (queryDto?.ExpireTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpireTime <= queryDto.ExpireTimeEnd);
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
