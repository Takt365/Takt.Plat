// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Personnel
// 文件名称：TaktEmployeeOnboardingTodoService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：入职待办应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Personnel;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Domain.Entities.HumanResource.Talent;

namespace Takt.Application.Services.HumanResource.Personnel;

/// <summary>
/// 入职待办应用服务
/// </summary>
public class TaktEmployeeOnboardingTodoService : TaktServiceBase, ITaktEmployeeOnboardingTodoService
{
    private readonly ITaktCompanyRepository<TaktEmployeeOnboardingTodo> _employeeOnboardingTodoRepository;
    private readonly ITaktApprovalRepository<TaktTalentOffer> _talentOfferRepository;
    private readonly ITaktApprovalRepository<TaktEmployeeJoined> _employeeJoinedRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeOnboardingTodoRepository">入职待办仓储</param>
    /// <param name="talentOfferRepository">录用信息仓储</param>
    /// <param name="employeeJoinedRepository">员工入职上岗仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEmployeeOnboardingTodoService(
        ITaktCompanyRepository<TaktEmployeeOnboardingTodo> employeeOnboardingTodoRepository,
        ITaktApprovalRepository<TaktTalentOffer> talentOfferRepository,
        ITaktApprovalRepository<TaktEmployeeJoined> employeeJoinedRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _employeeOnboardingTodoRepository = employeeOnboardingTodoRepository;
        _talentOfferRepository = talentOfferRepository;
        _employeeJoinedRepository = employeeJoinedRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取入职待办列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEmployeeOnboardingTodoDto>> GetEmployeeOnboardingTodoListAsync(TaktEmployeeOnboardingTodoQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _employeeOnboardingTodoRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEmployeeOnboardingTodoDto>.Create(
            data.Adapt<List<TaktEmployeeOnboardingTodoDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeOnboardingTodoDto?> GetEmployeeOnboardingTodoByIdAsync(long id)
    {
        var entity = await _employeeOnboardingTodoRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEmployeeOnboardingTodoDto>();
    }

    /// <summary>
    /// 获取入职待办选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEmployeeOnboardingTodoOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _employeeOnboardingTodoRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CandidateName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CandidateName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建入职待办
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeOnboardingTodoDto> CreateEmployeeOnboardingTodoAsync(TaktEmployeeOnboardingTodoCreateDto dto)
    {
        var entity = dto.Adapt<TaktEmployeeOnboardingTodo>();
        await StampEmployeeOnboardingTodoTalentOfferAsync(entity, dto);
        await StampEmployeeOnboardingTodoEmployeeJoinedAsync(entity, dto);
        entity = await _employeeOnboardingTodoRepository.CreateAsync(entity);
        return await GetEmployeeOnboardingTodoByIdAsync(entity.Id) ?? entity.Adapt<TaktEmployeeOnboardingTodoDto>();
    }

    /// <summary>
    /// 更新入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeOnboardingTodoDto> UpdateEmployeeOnboardingTodoAsync(long id, TaktEmployeeOnboardingTodoUpdateDto dto)
    {
        var entity = await _employeeOnboardingTodoRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("入职待办不存在");
        }
        dto.Adapt(entity);
        await StampEmployeeOnboardingTodoTalentOfferAsync(entity, dto);
        await StampEmployeeOnboardingTodoEmployeeJoinedAsync(entity, dto);
        await _employeeOnboardingTodoRepository.UpdateAsync(entity);
        return await GetEmployeeOnboardingTodoByIdAsync(id) ?? throw new TaktBusinessException("入职待办不存在");
    }

    /// <summary>
    /// 删除入职待办
    /// </summary>
    /// <param name="id">入职待办ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeOnboardingTodoByIdAsync(long id)
    {
        var deleted = await _employeeOnboardingTodoRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("入职待办不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除入职待办
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEmployeeOnboardingTodoBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEmployeeOnboardingTodoByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新入职待办状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEmployeeOnboardingTodoDto> UpdateEmployeeOnboardingTodoStatusAsync(TaktEmployeeOnboardingTodoStatusDto dto)
    {
        var entity = await _employeeOnboardingTodoRepository.GetByIdAsync(dto.EmployeeOnboardingTodoId);
        if (entity == null)
        {
            throw new TaktBusinessException("入职待办不存在");
        }
        entity.TodoStatus = dto.TodoStatus;
        await _employeeOnboardingTodoRepository.UpdateAsync(entity);
        return await GetEmployeeOnboardingTodoByIdAsync(dto.EmployeeOnboardingTodoId) ?? throw new TaktBusinessException("入职待办不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEmployeeOnboardingTodoTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEmployeeOnboardingTodoTemplateDto>(
            sheetName ?? "入职待办导入模板",
            fileName ?? "入职待办导入模板.xlsx");
    }

    /// <summary>
    /// 导入入职待办
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEmployeeOnboardingTodoAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEmployeeOnboardingTodoImportDto>(fileStream, sheetName ?? "入职待办导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktEmployeeOnboardingTodo>();
                var importDto = rows[i].Adapt<TaktEmployeeOnboardingTodoCreateDto>();
                await StampEmployeeOnboardingTodoTalentOfferAsync(entity, importDto);
                await StampEmployeeOnboardingTodoEmployeeJoinedAsync(entity, importDto);
                await _employeeOnboardingTodoRepository.CreateAsync(entity);
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
    /// 导出入职待办
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEmployeeOnboardingTodoAsync(TaktEmployeeOnboardingTodoQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEmployeeOnboardingTodoQueryDto());
        var list = await _employeeOnboardingTodoRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEmployeeOnboardingTodoExportDto>(),
                sheetName ?? "入职待办数据",
                fileName ?? "入职待办导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEmployeeOnboardingTodoExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "入职待办数据",
            fileName ?? "入职待办导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步入职待办主表外键（ManyToOne → 录用信息）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeOnboardingTodoTalentOfferAsync(TaktEmployeeOnboardingTodo entity, TaktEmployeeOnboardingTodoCreateDto dto)
    {
        if (dto.OfferId <= 0)
        {
            return;
        }
        var master = await _talentOfferRepository.GetByIdAsync(dto.OfferId);
        if (master == null)
        {
            throw new TaktBusinessException("录用信息不存在");
        }
        entity.OfferId = master.Id;
    }

    /// <summary>
    /// 同步入职待办主表外键（ManyToOne → 员工入职上岗）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEmployeeOnboardingTodoEmployeeJoinedAsync(TaktEmployeeOnboardingTodo entity, TaktEmployeeOnboardingTodoCreateDto dto)
    {
        if (dto.EmployeeJoinedId is not > 0)
        {
            return;
        }
        var master = await _employeeJoinedRepository.GetByIdAsync(dto.EmployeeJoinedId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("员工入职上岗不存在");
        }
        entity.EmployeeJoinedId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建入职待办查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEmployeeOnboardingTodo, bool>> QueryExpression(TaktEmployeeOnboardingTodoQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEmployeeOnboardingTodo>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.OfferId).Contains(keywords)
                || (x.TodoNo != null && x.TodoNo.Contains(keywords))
                || SqlFunc.ToString(x.TodoStatus).Contains(keywords)
                || (x.CandidateName != null && x.CandidateName.Contains(keywords))
                || (x.Mobile != null && x.Mobile.Contains(keywords))
                || SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || SqlFunc.ToString(x.EmployeeJoinedId).Contains(keywords)
                || (x.Reason != null && x.Reason.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlannedJoinedDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.OfferId.HasValue == true)
        {
            exp = exp.And(x => x.OfferId == queryDto.OfferId);
        }

        if (!string.IsNullOrEmpty(queryDto?.TodoNo))
        {
            exp = exp.And(x => x.TodoNo != null && x.TodoNo.Contains(queryDto.TodoNo));
        }

        if (queryDto?.TodoStatus.HasValue == true)
        {
            exp = exp.And(x => x.TodoStatus == queryDto.TodoStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.CandidateName))
        {
            exp = exp.And(x => x.CandidateName != null && x.CandidateName.Contains(queryDto.CandidateName));
        }

        if (!string.IsNullOrEmpty(queryDto?.Mobile))
        {
            exp = exp.And(x => x.Mobile != null && x.Mobile.Contains(queryDto.Mobile));
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (queryDto?.EmployeeJoinedId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeJoinedId == queryDto.EmployeeJoinedId);
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

        if (queryDto?.PlannedJoinedDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlannedJoinedDate >= queryDto.PlannedJoinedDateStart);
        }

        if (queryDto?.PlannedJoinedDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlannedJoinedDate <= queryDto.PlannedJoinedDateEnd);
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
