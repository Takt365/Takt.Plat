// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Talent
// 文件名称：TaktTalentInterviewService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：面试安排应用服务实现
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
using Takt.Domain.Entities.HumanResource.Talent;

namespace Takt.Application.Services.HumanResource.Talent;

/// <summary>
/// 面试安排应用服务
/// </summary>
public class TaktTalentInterviewService : TaktServiceBase, ITaktTalentInterviewService
{
    private readonly ITaktCompanyRepository<TaktTalentInterview> _talentInterviewRepository;
    private readonly ITaktApprovalRepository<TaktTalentOffer> _talentOfferRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentInterviewRepository">面试安排仓储</param>
    /// <param name="talentOfferRepository">TalentOffer仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTalentInterviewService(
        ITaktCompanyRepository<TaktTalentInterview> talentInterviewRepository,
        ITaktApprovalRepository<TaktTalentOffer> talentOfferRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _talentInterviewRepository = talentInterviewRepository;
        _talentOfferRepository = talentOfferRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取面试安排列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTalentInterviewDto>> GetTalentInterviewListAsync(TaktTalentInterviewQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _talentInterviewRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTalentInterviewDto>.Create(
            data.Adapt<List<TaktTalentInterviewDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取面试安排
    /// </summary>
    /// <param name="id">面试安排ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentInterviewDto?> GetTalentInterviewByIdAsync(long id)
    {
        var entity = await _talentInterviewRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTalentInterviewDto>();
        await FillTalentInterviewDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取面试安排选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTalentInterviewOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _talentInterviewRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.InterviewerName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.InterviewerName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建面试安排
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentInterviewDto> CreateTalentInterviewAsync(TaktTalentInterviewCreateDto dto)
    {
        var entity = dto.Adapt<TaktTalentInterview>();
        entity = await _talentInterviewRepository.CreateAsync(entity);
                await SaveTalentInterviewChildrenAsync(entity, dto);
        return await GetTalentInterviewByIdAsync(entity.Id) ?? entity.Adapt<TaktTalentInterviewDto>();
    }

    /// <summary>
    /// 更新面试安排
    /// </summary>
    /// <param name="id">面试安排ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentInterviewDto> UpdateTalentInterviewAsync(long id, TaktTalentInterviewUpdateDto dto)
    {
        var entity = await _talentInterviewRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("面试安排不存在");
        }
        dto.Adapt(entity);
        await _talentInterviewRepository.UpdateAsync(entity);
                await SaveTalentInterviewChildrenAsync(entity, dto);
        return await GetTalentInterviewByIdAsync(id) ?? throw new TaktBusinessException("面试安排不存在");
    }

    /// <summary>
    /// 删除面试安排
    /// </summary>
    /// <param name="id">面试安排ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentInterviewByIdAsync(long id)
    {
        var entity = await _talentInterviewRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("面试安排不存在或已删除");
        }
        await _talentOfferRepository.DeleteAsync(x => x.InterviewId == entity.Id);
        var deleted = await _talentInterviewRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("面试安排不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除面试安排
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentInterviewBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTalentInterviewByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新面试安排状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentInterviewDto> UpdateTalentInterviewStatusAsync(TaktTalentInterviewStatusDto dto)
    {
        var entity = await _talentInterviewRepository.GetByIdAsync(dto.TalentInterviewId);
        if (entity == null)
        {
            throw new TaktBusinessException("面试安排不存在");
        }
        entity.InterviewStatus = dto.InterviewStatus;
        await _talentInterviewRepository.UpdateAsync(entity);
        return await GetTalentInterviewByIdAsync(dto.TalentInterviewId) ?? throw new TaktBusinessException("面试安排不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTalentInterviewTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTalentInterviewTemplateDto>(
            sheetName ?? "面试安排导入模板",
            fileName ?? "面试安排导入模板.xlsx");
    }

    /// <summary>
    /// 导入面试安排
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTalentInterviewAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTalentInterviewImportDto>(fileStream, sheetName ?? "面试安排导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTalentInterview>();
                await _talentInterviewRepository.CreateAsync(entity);
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
    /// 导出面试安排
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTalentInterviewAsync(TaktTalentInterviewQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTalentInterviewQueryDto());
        var list = await _talentInterviewRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentInterviewExportDto>(),
                sheetName ?? "面试安排数据",
                fileName ?? "面试安排导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTalentInterviewExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "面试安排数据",
            fileName ?? "面试安排导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充面试安排详情（加载 OneToMany 子表：录用信息）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillTalentInterviewDetailsAsync(TaktTalentInterviewDto dto, TaktTalentInterview entity)
    {
        if (dto == null)
        {
            return;
        }
        // 录用信息 → dto.TalentOffers
        var talentoffers = await _talentOfferRepository.GetListAsync(x => x.InterviewId == entity.Id);
        dto.TalentOffers = talentoffers.Adapt<List<TaktTalentOfferDto>>();
    }

    /// <summary>
    /// 保存面试安排子表级联（录用信息；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveTalentInterviewChildrenAsync(TaktTalentInterview entity, TaktTalentInterviewCreateDto dto)
    {
        // 录用信息（TalentOffers）
        if (dto.TalentOffers is not { Count: > 0 })
        {
            await _talentOfferRepository.DeleteAsync(x => x.InterviewId == entity.Id);
        }
        else
        {
            var talentoffers = dto.TalentOffers.Adapt<List<TaktTalentOffer>>();
            foreach (var child in talentoffers)
            {
                child.InterviewId = entity.Id;
            }
            await _talentOfferRepository.DeleteAsync(x => x.InterviewId == entity.Id);
            foreach (var child in talentoffers)
            {
            }
            await _talentOfferRepository.CreateRangeAsync(talentoffers);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建面试安排查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTalentInterview, bool>> QueryExpression(TaktTalentInterviewQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTalentInterview>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.JobPostingId).Contains(keywords)
                || (x.InterviewNo != null && x.InterviewNo.Contains(keywords))
                || SqlFunc.ToString(x.InterviewStatus).Contains(keywords)
                || SqlFunc.ToString(x.InterviewRound).Contains(keywords)
                || (x.InterviewerName != null && x.InterviewerName.Contains(keywords))
                || (x.CandidateName != null && x.CandidateName.Contains(keywords))
                || (x.Mobile != null && x.Mobile.Contains(keywords))
                || (x.Email != null && x.Email.Contains(keywords))
                || (x.InterviewLocation != null && x.InterviewLocation.Contains(keywords))
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.InterviewDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.JobPostingId.HasValue == true)
        {
            exp = exp.And(x => x.JobPostingId == queryDto.JobPostingId);
        }

        if (!string.IsNullOrEmpty(queryDto?.InterviewNo))
        {
            exp = exp.And(x => x.InterviewNo != null && x.InterviewNo.Contains(queryDto.InterviewNo));
        }

        if (queryDto?.InterviewStatus.HasValue == true)
        {
            exp = exp.And(x => x.InterviewStatus == queryDto.InterviewStatus);
        }

        if (queryDto?.InterviewRound.HasValue == true)
        {
            exp = exp.And(x => x.InterviewRound == queryDto.InterviewRound);
        }

        if (!string.IsNullOrEmpty(queryDto?.InterviewerName))
        {
            exp = exp.And(x => x.InterviewerName != null && x.InterviewerName.Contains(queryDto.InterviewerName));
        }

        if (!string.IsNullOrEmpty(queryDto?.CandidateName))
        {
            exp = exp.And(x => x.CandidateName != null && x.CandidateName.Contains(queryDto.CandidateName));
        }

        if (!string.IsNullOrEmpty(queryDto?.Mobile))
        {
            exp = exp.And(x => x.Mobile != null && x.Mobile.Contains(queryDto.Mobile));
        }

        if (!string.IsNullOrEmpty(queryDto?.Email))
        {
            exp = exp.And(x => x.Email != null && x.Email.Contains(queryDto.Email));
        }

        if (!string.IsNullOrEmpty(queryDto?.InterviewLocation))
        {
            exp = exp.And(x => x.InterviewLocation != null && x.InterviewLocation.Contains(queryDto.InterviewLocation));
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

        if (queryDto?.InterviewDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InterviewDate >= queryDto.InterviewDateStart);
        }

        if (queryDto?.InterviewDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InterviewDate <= queryDto.InterviewDateEnd);
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
