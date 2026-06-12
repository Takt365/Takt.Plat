// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Talent
// 文件名称：TaktTalentRecruitmentPlanService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：招聘计划应用服务实现
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
using Takt.Shared.Enums;

namespace Takt.Application.Services.HumanResource.Talent;

/// <summary>
/// 招聘计划应用服务
/// </summary>
public class TaktTalentRecruitmentPlanService : TaktServiceBase, ITaktTalentRecruitmentPlanService
{
    private readonly ITaktApprovalRepository<TaktTalentRecruitmentPlan> _talentRecruitmentPlanRepository;
    private readonly ITaktCompanyRepository<TaktTalentJobPosting> _talentJobPostingRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentRecruitmentPlanRepository">招聘计划仓储</param>
    /// <param name="talentJobPostingRepository">TalentJobPosting仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTalentRecruitmentPlanService(
        ITaktApprovalRepository<TaktTalentRecruitmentPlan> talentRecruitmentPlanRepository,
        ITaktCompanyRepository<TaktTalentJobPosting> talentJobPostingRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _talentRecruitmentPlanRepository = talentRecruitmentPlanRepository;
        _talentJobPostingRepository = talentJobPostingRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取招聘计划列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTalentRecruitmentPlanDto>> GetTalentRecruitmentPlanListAsync(TaktTalentRecruitmentPlanQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _talentRecruitmentPlanRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTalentRecruitmentPlanDto>.Create(
            data.Adapt<List<TaktTalentRecruitmentPlanDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取招聘计划
    /// </summary>
    /// <param name="id">招聘计划ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentRecruitmentPlanDto?> GetTalentRecruitmentPlanByIdAsync(long id)
    {
        var entity = await _talentRecruitmentPlanRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTalentRecruitmentPlanDto>();
        await FillTalentRecruitmentPlanDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取招聘计划选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTalentRecruitmentPlanOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _talentRecruitmentPlanRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PlanNo,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlanNo ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建招聘计划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentRecruitmentPlanDto> CreateTalentRecruitmentPlanAsync(TaktTalentRecruitmentPlanCreateDto dto)
    {
        var entity = dto.Adapt<TaktTalentRecruitmentPlan>();
        entity = await _talentRecruitmentPlanRepository.CreateAsync(entity);
                await SaveTalentRecruitmentPlanChildrenAsync(entity, dto);
        return await GetTalentRecruitmentPlanByIdAsync(entity.Id) ?? entity.Adapt<TaktTalentRecruitmentPlanDto>();
    }

    /// <summary>
    /// 更新招聘计划
    /// </summary>
    /// <param name="id">招聘计划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentRecruitmentPlanDto> UpdateTalentRecruitmentPlanAsync(long id, TaktTalentRecruitmentPlanUpdateDto dto)
    {
        var entity = await _talentRecruitmentPlanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("招聘计划不存在");
        }
        dto.Adapt(entity);
        await _talentRecruitmentPlanRepository.UpdateAsync(entity);
                await SaveTalentRecruitmentPlanChildrenAsync(entity, dto);
        return await GetTalentRecruitmentPlanByIdAsync(id) ?? throw new TaktBusinessException("招聘计划不存在");
    }

    /// <summary>
    /// 删除招聘计划
    /// </summary>
    /// <param name="id">招聘计划ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentRecruitmentPlanByIdAsync(long id)
    {
        var entity = await _talentRecruitmentPlanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("招聘计划不存在或已删除");
        }
        await _talentJobPostingRepository.DeleteAsync(x => x.RecruitmentPlanId == entity.Id);
        var deleted = await _talentRecruitmentPlanRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("招聘计划不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除招聘计划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentRecruitmentPlanBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTalentRecruitmentPlanByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTalentRecruitmentPlanTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTalentRecruitmentPlanTemplateDto>(
            sheetName ?? "招聘计划导入模板",
            fileName ?? "招聘计划导入模板.xlsx");
    }

    /// <summary>
    /// 导入招聘计划
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTalentRecruitmentPlanAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTalentRecruitmentPlanImportDto>(fileStream, sheetName ?? "招聘计划导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTalentRecruitmentPlan>();
                await _talentRecruitmentPlanRepository.CreateAsync(entity);
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
    /// 导出招聘计划
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTalentRecruitmentPlanAsync(TaktTalentRecruitmentPlanQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTalentRecruitmentPlanQueryDto());
        var list = await _talentRecruitmentPlanRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentRecruitmentPlanExportDto>(),
                sheetName ?? "招聘计划数据",
                fileName ?? "招聘计划导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTalentRecruitmentPlanExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "招聘计划数据",
            fileName ?? "招聘计划导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充招聘计划详情（加载 OneToMany 子表：职位发布）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillTalentRecruitmentPlanDetailsAsync(TaktTalentRecruitmentPlanDto dto, TaktTalentRecruitmentPlan entity)
    {
        if (dto == null)
        {
            return;
        }
        // 职位发布 → dto.TalentJobPostings
        var talentjobpostings = await _talentJobPostingRepository.GetListAsync(x => x.RecruitmentPlanId == entity.Id);
        dto.TalentJobPostings = talentjobpostings.Adapt<List<TaktTalentJobPostingDto>>();
    }

    /// <summary>
    /// 保存招聘计划子表级联（职位发布；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveTalentRecruitmentPlanChildrenAsync(TaktTalentRecruitmentPlan entity, TaktTalentRecruitmentPlanCreateDto dto)
    {
        // 职位发布（TalentJobPostings）
        if (dto.TalentJobPostings is not { Count: > 0 })
        {
            await _talentJobPostingRepository.DeleteAsync(x => x.RecruitmentPlanId == entity.Id);
        }
        else
        {
            var talentjobpostings = dto.TalentJobPostings.Adapt<List<TaktTalentJobPosting>>();
            foreach (var child in talentjobpostings)
            {
                child.RecruitmentPlanId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < talentjobpostings.Count; i++)
                        {
                            var key = $"{talentjobpostings[i].CompanyCode}|{talentjobpostings[i].PostingCode}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"职位发布第{i + 1}项与本次提交的其他项重复（CompanyCode、PostingCode）");
                            }
                        }
            await _talentJobPostingRepository.DeleteAsync(x => x.RecruitmentPlanId == entity.Id);
            foreach (var child in talentjobpostings)
            {
            var isUnique_ix_talent_job_posting_code_unique = await _uniqueValidator.IsUniqueAsync(
                _talentJobPostingRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.PostingCode == child.PostingCode);
            if (!isUnique_ix_talent_job_posting_code_unique)
            {
                throw new TaktBusinessException("职位发布的CompanyCode、PostingCode已存在");
            }
            }
            await _talentJobPostingRepository.CreateRangeAsync(talentjobpostings);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建招聘计划查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTalentRecruitmentPlan, bool>> QueryExpression(TaktTalentRecruitmentPlanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTalentRecruitmentPlan>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.StaffingRequirementId).Contains(keywords)
                || (x.PlanNo != null && x.PlanNo.Contains(keywords))
                || SqlFunc.ToString(x.PlanHeadcount).Contains(keywords)
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlanDate).Contains(keywords)
                || SqlFunc.ToString(x.PlanStartDate).Contains(keywords)
                || SqlFunc.ToString(x.PlanEndDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.StaffingRequirementId.HasValue == true)
        {
            exp = exp.And(x => x.StaffingRequirementId == queryDto.StaffingRequirementId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlanNo))
        {
            exp = exp.And(x => x.PlanNo != null && x.PlanNo.Contains(queryDto.PlanNo));
        }

        if (queryDto?.PlanHeadcount.HasValue == true)
        {
            exp = exp.And(x => x.PlanHeadcount == queryDto.PlanHeadcount);
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

        if (queryDto?.PlanDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanDate >= queryDto.PlanDateStart);
        }

        if (queryDto?.PlanDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanDate <= queryDto.PlanDateEnd);
        }

        if (queryDto?.PlanStartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanStartDate >= queryDto.PlanStartDateStart);
        }

        if (queryDto?.PlanStartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanStartDate <= queryDto.PlanStartDateEnd);
        }

        if (queryDto?.PlanEndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanEndDate >= queryDto.PlanEndDateStart);
        }

        if (queryDto?.PlanEndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanEndDate <= queryDto.PlanEndDateEnd);
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
