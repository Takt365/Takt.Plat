// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Talent
// 文件名称：TaktTalentOfferService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：录用信息应用服务实现
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
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Domain.Entities.HumanResource.Personnel;

namespace Takt.Application.Services.HumanResource.Talent;

/// <summary>
/// 录用信息应用服务
/// </summary>
public class TaktTalentOfferService : TaktServiceBase, ITaktTalentOfferService
{
    private readonly ITaktApprovalRepository<TaktTalentOffer> _talentOfferRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeOnboarding> _employeeOnboardingRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="talentOfferRepository">录用信息仓储</param>
    /// <param name="employeeOnboardingRepository">EmployeeOnboarding仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTalentOfferService(
        ITaktApprovalRepository<TaktTalentOffer> talentOfferRepository,
        ITaktCompanyRepository<TaktEmployeeOnboarding> employeeOnboardingRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _talentOfferRepository = talentOfferRepository;
        _employeeOnboardingRepository = employeeOnboardingRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取录用信息列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTalentOfferDto>> GetTalentOfferListAsync(TaktTalentOfferQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _talentOfferRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTalentOfferDto>.Create(
            data.Adapt<List<TaktTalentOfferDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取录用信息
    /// </summary>
    /// <param name="id">录用信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentOfferDto?> GetTalentOfferByIdAsync(long id)
    {
        var entity = await _talentOfferRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktTalentOfferDto>();
        await FillTalentOfferDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取录用信息选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTalentOfferOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _talentOfferRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.DeptName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DeptName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建录用信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentOfferDto> CreateTalentOfferAsync(TaktTalentOfferCreateDto dto)
    {
        var entity = dto.Adapt<TaktTalentOffer>();
        entity = await _talentOfferRepository.CreateAsync(entity);
                await SaveTalentOfferChildrenAsync(entity, dto);
        return await GetTalentOfferByIdAsync(entity.Id) ?? entity.Adapt<TaktTalentOfferDto>();
    }

    /// <summary>
    /// 更新录用信息
    /// </summary>
    /// <param name="id">录用信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTalentOfferDto> UpdateTalentOfferAsync(long id, TaktTalentOfferUpdateDto dto)
    {
        var entity = await _talentOfferRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("录用信息不存在");
        }
        dto.Adapt(entity);
        await _talentOfferRepository.UpdateAsync(entity);
                await SaveTalentOfferChildrenAsync(entity, dto);
        return await GetTalentOfferByIdAsync(id) ?? throw new TaktBusinessException("录用信息不存在");
    }

    /// <summary>
    /// 删除录用信息
    /// </summary>
    /// <param name="id">录用信息ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentOfferByIdAsync(long id)
    {
        var entity = await _talentOfferRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("录用信息不存在或已删除");
        }
        await _employeeOnboardingRepository.DeleteAsync(x => x.OfferId == entity.Id);
        var deleted = await _talentOfferRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("录用信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除录用信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTalentOfferBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTalentOfferByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTalentOfferTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTalentOfferTemplateDto>(
            sheetName ?? "录用信息导入模板",
            fileName ?? "录用信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入录用信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTalentOfferAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTalentOfferImportDto>(fileStream, sheetName ?? "录用信息导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTalentOffer>();
                await _talentOfferRepository.CreateAsync(entity);
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
    /// 导出录用信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTalentOfferAsync(TaktTalentOfferQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTalentOfferQueryDto());
        var list = await _talentOfferRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTalentOfferExportDto>(),
                sheetName ?? "录用信息数据",
                fileName ?? "录用信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTalentOfferExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "录用信息数据",
            fileName ?? "录用信息导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充录用信息详情（加载 OneToMany 子表：入职待办）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillTalentOfferDetailsAsync(TaktTalentOfferDto dto, TaktTalentOffer entity)
    {
        if (dto == null)
        {
            return;
        }
        // 入职待办 → dto.EmployeeOnboardings
        var employeeonboardings = await _employeeOnboardingRepository.GetListAsync(x => x.OfferId == entity.Id);
        dto.EmployeeOnboardings = employeeonboardings.Adapt<List<TaktEmployeeOnboardingDto>>();
    }

    /// <summary>
    /// 保存录用信息子表级联（入职待办；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveTalentOfferChildrenAsync(TaktTalentOffer entity, TaktTalentOfferCreateDto dto)
    {
        // 入职待办（EmployeeOnboardings）
        if (dto.EmployeeOnboardings is not { Count: > 0 })
        {
            await _employeeOnboardingRepository.DeleteAsync(x => x.OfferId == entity.Id);
        }
        else
        {
            var employeeonboardings = dto.EmployeeOnboardings.Adapt<List<TaktEmployeeOnboarding>>();
            foreach (var child in employeeonboardings)
            {
                child.OfferId = entity.Id;
            }
            await _employeeOnboardingRepository.DeleteAsync(x => x.OfferId == entity.Id);
            foreach (var child in employeeonboardings)
            {
            }
            await _employeeOnboardingRepository.CreateRangeAsync(employeeonboardings);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建录用信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTalentOffer, bool>> QueryExpression(TaktTalentOfferQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTalentOffer>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.InterviewId).Contains(keywords)
                || (x.OfferNo != null && x.OfferNo.Contains(keywords))
                || SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || SqlFunc.ToString(x.PostId).Contains(keywords)
                || (x.PostName != null && x.PostName.Contains(keywords))
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.HireDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.InterviewId.HasValue == true)
        {
            exp = exp.And(x => x.InterviewId == queryDto.InterviewId);
        }

        if (!string.IsNullOrEmpty(queryDto?.OfferNo))
        {
            exp = exp.And(x => x.OfferNo != null && x.OfferNo.Contains(queryDto.OfferNo));
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (queryDto?.PostId.HasValue == true)
        {
            exp = exp.And(x => x.PostId == queryDto.PostId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PostName))
        {
            exp = exp.And(x => x.PostName != null && x.PostName.Contains(queryDto.PostName));
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

        if (queryDto?.HireDateStart.HasValue == true)
        {
            exp = exp.And(x => x.HireDate >= queryDto.HireDateStart);
        }

        if (queryDto?.HireDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.HireDate <= queryDto.HireDateEnd);
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
