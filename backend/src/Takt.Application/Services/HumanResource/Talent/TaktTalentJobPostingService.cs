// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Talent
// 文件名称：TaktTalentJobPostingService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：职位发布应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Talent;
using Takt.Domain.Entities.HumanResource.Talent;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Talent;

/// <summary>
/// 职位发布应用服务
/// </summary>
public class TaktTalentJobPostingService : TaktServiceBase, ITaktTalentJobPostingService
{
    private readonly ITaktCompanyRepository<TaktTalentJobPosting> _talentJobPostingRepository;
    private readonly ITaktCompanyRepository<TaktTalentInterview> _talentInterviewRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentJobPostingRepository">职位发布仓储</param>
    /// <param name="talentInterviewRepository">TalentInterview仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTalentJobPostingService(
        ITaktCompanyRepository<TaktTalentJobPosting> talentJobPostingRepository,
        ITaktCompanyRepository<TaktTalentInterview> talentInterviewRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _talentJobPostingRepository = talentJobPostingRepository;
        _talentInterviewRepository = talentInterviewRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取职位发布列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTalentJobPostingDto>> GetTalentJobPostingListAsync(TaktTalentJobPostingQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _talentJobPostingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTalentJobPostingDto>.Create(
            data.Adapt<List<TaktTalentJobPostingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取职位发布
    /// </summary>
    /// <param name="id">职位发布ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentJobPostingDto?> GetTalentJobPostingByIdAsync(long id)
    {
        var entity = await _talentJobPostingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTalentJobPostingDto>();
        await FillTalentJobPostingDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取职位发布选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTalentJobPostingOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _talentJobPostingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PostingCode,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PostingCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建职位发布
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentJobPostingDto> CreateTalentJobPostingAsync(TaktTalentJobPostingCreateDto dto)
    {
        var entity = dto.Adapt<TaktTalentJobPosting>();
        var isUnique_ix_talent_job_posting_code_unique = await _uniqueValidator.IsUniqueAsync(
            _talentJobPostingRepository,
            x => x.PostingCode == entity.PostingCode);
        if (!isUnique_ix_talent_job_posting_code_unique)
        {
            throw new TaktBusinessException("职位发布的PostingCode已存在");
        }
        entity = await _talentJobPostingRepository.CreateAsync(entity);
                await SaveTalentJobPostingChildrenAsync(entity, dto);
        return await GetTalentJobPostingByIdAsync(entity.Id) ?? entity.Adapt<TaktTalentJobPostingDto>();
    }

    /// <summary>
    /// 更新职位发布
    /// </summary>
    /// <param name="id">职位发布ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentJobPostingDto> UpdateTalentJobPostingAsync(long id, TaktTalentJobPostingUpdateDto dto)
    {
        var entity = await _talentJobPostingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("职位发布不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_talent_job_posting_code_unique = await _uniqueValidator.IsUniqueAsync(
            _talentJobPostingRepository,
            x => x.PostingCode == entity.PostingCode,
            id);
        if (!isUnique_ix_talent_job_posting_code_unique)
        {
            throw new TaktBusinessException("职位发布的PostingCode已存在");
        }
        await _talentJobPostingRepository.UpdateAsync(entity);
                await SaveTalentJobPostingChildrenAsync(entity, dto);
        return await GetTalentJobPostingByIdAsync(id) ?? throw new TaktBusinessException("职位发布不存在");
    }

    /// <summary>
    /// 删除职位发布
    /// </summary>
    /// <param name="id">职位发布ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentJobPostingByIdAsync(long id)
    {
        var entity = await _talentJobPostingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("职位发布不存在或已删除");
        }
        await _talentInterviewRepository.DeleteAsync(x => x.JobPostingId == entity.Id);
        var deleted = await _talentJobPostingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("职位发布不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除职位发布
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentJobPostingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTalentJobPostingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新职位发布状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentJobPostingDto> UpdateTalentJobPostingStatusAsync(TaktTalentJobPostingStatusDto dto)
    {
        var entity = await _talentJobPostingRepository.GetByIdAsync(dto.TalentJobPostingId);
        if (entity == null)
        {
            throw new TaktBusinessException("职位发布不存在");
        }
        entity.PostingStatus = dto.PostingStatus;
        await _talentJobPostingRepository.UpdateAsync(entity);
        return await GetTalentJobPostingByIdAsync(dto.TalentJobPostingId) ?? throw new TaktBusinessException("职位发布不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTalentJobPostingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTalentJobPostingTemplateDto>(
            sheetName ?? "职位发布导入模板",
            fileName ?? "职位发布导入模板.xlsx");
    }

    /// <summary>
    /// 导入职位发布
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTalentJobPostingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTalentJobPostingImportDto>(fileStream, sheetName ?? "职位发布导入模板");
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
                var entity = rows[i].Adapt<TaktTalentJobPosting>();
                var importKey = $"{entity.PostingCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PostingCode）");
                }
                var isUnique_ix_talent_job_posting_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _talentJobPostingRepository,
                    x => x.PostingCode == entity.PostingCode);
                if (!isUnique_ix_talent_job_posting_code_unique)
                {
                    throw new TaktBusinessException("职位发布的PostingCode已存在");
                }
                await _talentJobPostingRepository.CreateAsync(entity);
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
    /// 导出职位发布
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTalentJobPostingAsync(TaktTalentJobPostingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTalentJobPostingQueryDto());
        var list = await _talentJobPostingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentJobPostingExportDto>(),
                sheetName ?? "职位发布数据",
                fileName ?? "职位发布导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTalentJobPostingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "职位发布数据",
            fileName ?? "职位发布导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充职位发布详情（加载 OneToMany 子表：面试安排）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillTalentJobPostingDetailsAsync(TaktTalentJobPostingDto dto, TaktTalentJobPosting entity)
    {
        if (dto == null)
        {
            return;
        }
        // 面试安排 → dto.TalentInterviews
        var talentinterviews = await _talentInterviewRepository.GetListAsync(x => x.JobPostingId == entity.Id);
        dto.TalentInterviews = talentinterviews.Adapt<List<TaktTalentInterviewDto>>();
    }

    /// <summary>
    /// 保存职位发布子表级联（面试安排；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveTalentJobPostingChildrenAsync(TaktTalentJobPosting entity, TaktTalentJobPostingCreateDto dto)
    {
        // 面试安排（TalentInterviews）
        if (dto.TalentInterviews is not { Count: > 0 })
        {
            await _talentInterviewRepository.DeleteAsync(x => x.JobPostingId == entity.Id);
        }
        else
        {
            var talentinterviews = dto.TalentInterviews.Adapt<List<TaktTalentInterview>>();
            foreach (var child in talentinterviews)
            {
                child.JobPostingId = entity.Id;
            }
            await _talentInterviewRepository.DeleteAsync(x => x.JobPostingId == entity.Id);
            foreach (var child in talentinterviews)
            {
            }
            await _talentInterviewRepository.CreateRangeAsync(talentinterviews);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建职位发布查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTalentJobPosting, bool>> QueryExpression(TaktTalentJobPostingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTalentJobPosting>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.RecruitmentPlanId).Contains(keywords)
                || (x.PostingCode != null && x.PostingCode.Contains(keywords))
                || (x.Title != null && x.Title.Contains(keywords))
                || SqlFunc.ToString(x.PostingStatus).Contains(keywords)
                || SqlFunc.ToString(x.PublishChannel).Contains(keywords)
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PublishDate).Contains(keywords)
                || SqlFunc.ToString(x.OpenDate).Contains(keywords)
                || SqlFunc.ToString(x.CloseDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.RecruitmentPlanId.HasValue == true)
        {
            exp = exp.And(x => x.RecruitmentPlanId == queryDto.RecruitmentPlanId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PostingCode))
        {
            exp = exp.And(x => x.PostingCode != null && x.PostingCode.Contains(queryDto.PostingCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.Title))
        {
            exp = exp.And(x => x.Title != null && x.Title.Contains(queryDto.Title));
        }

        if (queryDto?.PostingStatus.HasValue == true)
        {
            exp = exp.And(x => x.PostingStatus == queryDto.PostingStatus);
        }

        if (queryDto?.PublishChannel.HasValue == true)
        {
            exp = exp.And(x => x.PublishChannel == queryDto.PublishChannel);
        }

        if (!string.IsNullOrEmpty(queryDto?.Reason))
        {
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(queryDto.Reason));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.PublishDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PublishDate >= queryDto.PublishDateStart);
        }

        if (queryDto?.PublishDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PublishDate <= queryDto.PublishDateEnd);
        }

        if (queryDto?.OpenDateStart.HasValue == true)
        {
            exp = exp.And(x => x.OpenDate >= queryDto.OpenDateStart);
        }

        if (queryDto?.OpenDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.OpenDate <= queryDto.OpenDateEnd);
        }

        if (queryDto?.CloseDateStart.HasValue == true)
        {
            exp = exp.And(x => x.CloseDate >= queryDto.CloseDateStart);
        }

        if (queryDto?.CloseDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.CloseDate <= queryDto.CloseDateEnd);
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
