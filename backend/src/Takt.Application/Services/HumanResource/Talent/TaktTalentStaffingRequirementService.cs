// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Talent
// 文件名称：TaktTalentStaffingRequirementService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：用人需求应用服务实现
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
/// 用人需求应用服务
/// </summary>
public class TaktTalentStaffingRequirementService : TaktServiceBase, ITaktTalentStaffingRequirementService
{
    private readonly ITaktApprovalRepository<TaktTalentStaffingRequirement> _talentStaffingRequirementRepository;
    private readonly ITaktCompanyRepository<TaktTalentJobPosting> _talentJobPostingRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentStaffingRequirementRepository">用人需求仓储</param>
    /// <param name="talentJobPostingRepository">TalentJobPosting仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTalentStaffingRequirementService(
        ITaktApprovalRepository<TaktTalentStaffingRequirement> talentStaffingRequirementRepository,
        ITaktCompanyRepository<TaktTalentJobPosting> talentJobPostingRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _talentStaffingRequirementRepository = talentStaffingRequirementRepository;
        _talentJobPostingRepository = talentJobPostingRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取用人需求列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTalentStaffingRequirementDto>> GetTalentStaffingRequirementListAsync(TaktTalentStaffingRequirementQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _talentStaffingRequirementRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTalentStaffingRequirementDto>.Create(
            data.Adapt<List<TaktTalentStaffingRequirementDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取用人需求
    /// </summary>
    /// <param name="id">用人需求ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentStaffingRequirementDto?> GetTalentStaffingRequirementByIdAsync(long id)
    {
        var entity = await _talentStaffingRequirementRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTalentStaffingRequirementDto>();
        await FillTalentStaffingRequirementDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取用人需求选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTalentStaffingRequirementOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _talentStaffingRequirementRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ReasonCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ReasonCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建用人需求
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentStaffingRequirementDto> CreateTalentStaffingRequirementAsync(TaktTalentStaffingRequirementCreateDto dto)
    {
        var entity = dto.Adapt<TaktTalentStaffingRequirement>();
        var isUnique_ix_talent_staffing_requirement_req_no_unique = await _uniqueValidator.IsUniqueAsync(
            _talentStaffingRequirementRepository,
            x => x.ReqNo == entity.ReqNo);
        if (!isUnique_ix_talent_staffing_requirement_req_no_unique)
        {
            throw new TaktBusinessException("用人需求的ReqNo已存在");
        }
        entity = await _talentStaffingRequirementRepository.CreateAsync(entity);
                await SaveTalentStaffingRequirementChildrenAsync(entity, dto);
        return await GetTalentStaffingRequirementByIdAsync(entity.Id) ?? entity.Adapt<TaktTalentStaffingRequirementDto>();
    }

    /// <summary>
    /// 更新用人需求
    /// </summary>
    /// <param name="id">用人需求ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentStaffingRequirementDto> UpdateTalentStaffingRequirementAsync(long id, TaktTalentStaffingRequirementUpdateDto dto)
    {
        var entity = await _talentStaffingRequirementRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("用人需求不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_talent_staffing_requirement_req_no_unique = await _uniqueValidator.IsUniqueAsync(
            _talentStaffingRequirementRepository,
            x => x.ReqNo == entity.ReqNo,
            id);
        if (!isUnique_ix_talent_staffing_requirement_req_no_unique)
        {
            throw new TaktBusinessException("用人需求的ReqNo已存在");
        }
        await _talentStaffingRequirementRepository.UpdateAsync(entity);
                await SaveTalentStaffingRequirementChildrenAsync(entity, dto);
        return await GetTalentStaffingRequirementByIdAsync(id) ?? throw new TaktBusinessException("用人需求不存在");
    }

    /// <summary>
    /// 删除用人需求
    /// </summary>
    /// <param name="id">用人需求ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentStaffingRequirementByIdAsync(long id)
    {
        var entity = await _talentStaffingRequirementRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("用人需求不存在或已删除");
        }
        await _talentJobPostingRepository.DeleteAsync(x => x.StaffingRequirementId == entity.Id);
        var deleted = await _talentStaffingRequirementRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("用人需求不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除用人需求
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentStaffingRequirementBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTalentStaffingRequirementByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTalentStaffingRequirementTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTalentStaffingRequirementTemplateDto>(
            sheetName ?? "用人需求导入模板",
            fileName ?? "用人需求导入模板.xlsx");
    }

    /// <summary>
    /// 导入用人需求
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTalentStaffingRequirementAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTalentStaffingRequirementImportDto>(fileStream, sheetName ?? "用人需求导入模板");
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
                var entity = rows[i].Adapt<TaktTalentStaffingRequirement>();
                var importKey = $"{entity.ReqNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ReqNo）");
                }
                var isUnique_ix_talent_staffing_requirement_req_no_unique = await _uniqueValidator.IsUniqueAsync(
                    _talentStaffingRequirementRepository,
                    x => x.ReqNo == entity.ReqNo);
                if (!isUnique_ix_talent_staffing_requirement_req_no_unique)
                {
                    throw new TaktBusinessException("用人需求的ReqNo已存在");
                }
                await _talentStaffingRequirementRepository.CreateAsync(entity);
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
    /// 导出用人需求
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTalentStaffingRequirementAsync(TaktTalentStaffingRequirementQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTalentStaffingRequirementQueryDto());
        var list = await _talentStaffingRequirementRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentStaffingRequirementExportDto>(),
                sheetName ?? "用人需求数据",
                fileName ?? "用人需求导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTalentStaffingRequirementExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "用人需求数据",
            fileName ?? "用人需求导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充用人需求详情（加载 OneToMany 子表：职位发布）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillTalentStaffingRequirementDetailsAsync(TaktTalentStaffingRequirementDto dto, TaktTalentStaffingRequirement entity)
    {
        if (dto == null)
        {
            return;
        }
        // 职位发布 → dto.TalentJobPostings
        var talentjobpostings = await _talentJobPostingRepository.GetListAsync(x => x.StaffingRequirementId == entity.Id);
        dto.TalentJobPostings = talentjobpostings.Adapt<List<TaktTalentJobPostingDto>>();
    }

    /// <summary>
    /// 保存用人需求子表级联（职位发布；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveTalentStaffingRequirementChildrenAsync(TaktTalentStaffingRequirement entity, TaktTalentStaffingRequirementCreateDto dto)
    {
        // 职位发布（TalentJobPostings）
        if (dto.TalentJobPostings is not { Count: > 0 })
        {
            await _talentJobPostingRepository.DeleteAsync(x => x.StaffingRequirementId == entity.Id);
        }
        else
        {
            var talentjobpostings = dto.TalentJobPostings.Adapt<List<TaktTalentJobPosting>>();
            foreach (var child in talentjobpostings)
            {
                child.StaffingRequirementId = entity.Id;
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
            await _talentJobPostingRepository.DeleteAsync(x => x.StaffingRequirementId == entity.Id);
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
    /// 构建用人需求查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTalentStaffingRequirement, bool>> QueryExpression(TaktTalentStaffingRequirementQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTalentStaffingRequirement>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ReqNo != null && x.ReqNo.Contains(keywords))
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || SqlFunc.ToString(x.PostId).Contains(keywords)
                || (x.JobGrade != null && x.JobGrade.Contains(keywords))
                || SqlFunc.ToString(x.RequestQty).Contains(keywords)
                || (x.HeadcountType != null && x.HeadcountType.Contains(keywords))
                || (x.ReasonCode != null && x.ReasonCode.Contains(keywords))
                || SqlFunc.ToString(x.ReplaceEmployeeId).Contains(keywords)
                || (x.ContractType != null && x.ContractType.Contains(keywords))
                || (x.WorkLocation != null && x.WorkLocation.Contains(keywords))
                || (x.JobDesc != null && x.JobDesc.Contains(keywords))
                || (x.Qualification != null && x.Qualification.Contains(keywords))
                || (x.BudgetYear != null && x.BudgetYear.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ExpectedOnboardDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ReqNo))
        {
            exp = exp.And(x => x.ReqNo != null && x.ReqNo.Contains(queryDto.ReqNo));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (queryDto?.PostId.HasValue == true)
        {
            exp = exp.And(x => x.PostId == queryDto.PostId);
        }

        if (!string.IsNullOrEmpty(queryDto?.JobGrade))
        {
            exp = exp.And(x => x.JobGrade != null && x.JobGrade.Contains(queryDto.JobGrade));
        }

        if (queryDto?.RequestQty.HasValue == true)
        {
            exp = exp.And(x => x.RequestQty == queryDto.RequestQty);
        }

        if (!string.IsNullOrEmpty(queryDto?.HeadcountType))
        {
            exp = exp.And(x => x.HeadcountType != null && x.HeadcountType.Contains(queryDto.HeadcountType));
        }

        if (!string.IsNullOrEmpty(queryDto?.ReasonCode))
        {
            exp = exp.And(x => x.ReasonCode != null && x.ReasonCode.Contains(queryDto.ReasonCode));
        }

        if (queryDto?.ReplaceEmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.ReplaceEmployeeId == queryDto.ReplaceEmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ContractType))
        {
            exp = exp.And(x => x.ContractType != null && x.ContractType.Contains(queryDto.ContractType));
        }

        if (!string.IsNullOrEmpty(queryDto?.WorkLocation))
        {
            exp = exp.And(x => x.WorkLocation != null && x.WorkLocation.Contains(queryDto.WorkLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.JobDesc))
        {
            exp = exp.And(x => x.JobDesc != null && x.JobDesc.Contains(queryDto.JobDesc));
        }

        if (!string.IsNullOrEmpty(queryDto?.Qualification))
        {
            exp = exp.And(x => x.Qualification != null && x.Qualification.Contains(queryDto.Qualification));
        }

        if (!string.IsNullOrEmpty(queryDto?.BudgetYear))
        {
            exp = exp.And(x => x.BudgetYear != null && x.BudgetYear.Contains(queryDto.BudgetYear));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ExpectedOnboardDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpectedOnboardDate >= queryDto.ExpectedOnboardDateStart);
        }

        if (queryDto?.ExpectedOnboardDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpectedOnboardDate <= queryDto.ExpectedOnboardDateEnd);
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
