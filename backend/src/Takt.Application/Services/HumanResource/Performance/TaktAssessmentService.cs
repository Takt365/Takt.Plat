// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.Performance
// 文件名称：TaktAssessmentService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效考核应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Performance;
using Takt.Domain.Entities.HumanResource.Performance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Performance;

/// <summary>
/// 绩效考核应用服务
/// </summary>
public class TaktAssessmentService : TaktServiceBase, ITaktAssessmentService
{
    private readonly ITaktCompanyRepository<TaktAssessment> _assessmentRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assessmentRepository">绩效考核仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssessmentService(
        ITaktCompanyRepository<TaktAssessment> assessmentRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assessmentRepository = assessmentRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取绩效考核列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAssessmentDto>> GetAssessmentListAsync(TaktAssessmentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _assessmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAssessmentDto>.Create(
            data.Adapt<List<TaktAssessmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取绩效考核
    /// </summary>
    /// <param name="id">绩效考核ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssessmentDto?> GetAssessmentByIdAsync(long id)
    {
        var entity = await _assessmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAssessmentDto>();
    }

    /// <summary>
    /// 获取绩效考核选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAssessmentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _assessmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EmployeeName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建绩效考核
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssessmentDto> CreateAssessmentAsync(TaktAssessmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktAssessment>();
        entity = await _assessmentRepository.CreateAsync(entity);
        return await GetAssessmentByIdAsync(entity.Id) ?? entity.Adapt<TaktAssessmentDto>();
    }

    /// <summary>
    /// 更新绩效考核
    /// </summary>
    /// <param name="id">绩效考核ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssessmentDto> UpdateAssessmentAsync(long id, TaktAssessmentUpdateDto dto)
    {
        var entity = await _assessmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效考核不存在");
        }
        dto.Adapt(entity);
        await _assessmentRepository.UpdateAsync(entity);
        return await GetAssessmentByIdAsync(id) ?? throw new TaktBusinessException("绩效考核不存在");
    }

    /// <summary>
    /// 删除绩效考核
    /// </summary>
    /// <param name="id">绩效考核ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAssessmentByIdAsync(long id)
    {
        var deleted = await _assessmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("绩效考核不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除绩效考核
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAssessmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAssessmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新绩效考核状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAssessmentDto> UpdateAssessmentStatusAsync(TaktAssessmentStatusDto dto)
    {
        var entity = await _assessmentRepository.GetByIdAsync(dto.AssessmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效考核不存在");
        }
        entity.AssessmentStatus = dto.AssessmentStatus;
        await _assessmentRepository.UpdateAsync(entity);
        return await GetAssessmentByIdAsync(dto.AssessmentId) ?? throw new TaktBusinessException("绩效考核不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAssessmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAssessmentTemplateDto>(
            sheetName ?? "绩效考核导入模板",
            fileName ?? "绩效考核导入模板.xlsx");
    }

    /// <summary>
    /// 导入绩效考核
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAssessmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAssessmentImportDto>(fileStream, sheetName ?? "绩效考核导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktAssessment>();
                await _assessmentRepository.CreateAsync(entity);
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
    /// 导出绩效考核
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAssessmentAsync(TaktAssessmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktAssessmentQueryDto());
        var list = await _assessmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAssessmentExportDto>(),
                sheetName ?? "绩效考核数据",
                fileName ?? "绩效考核导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAssessmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "绩效考核数据",
            fileName ?? "绩效考核导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建绩效考核查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAssessment, bool>> QueryExpression(TaktAssessmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAssessment>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.AssessmentPeriod != null && x.AssessmentPeriod.Contains(keywords))
                || SqlFunc.ToString(x.SchemeMetricId).Contains(keywords)
                || SqlFunc.ToString(x.SelfScore).Contains(keywords)
                || (x.SelfEvaluationNotes != null && x.SelfEvaluationNotes.Contains(keywords))
                || SqlFunc.ToString(x.SupervisorScore).Contains(keywords)
                || (x.SupervisorComments != null && x.SupervisorComments.Contains(keywords))
                || SqlFunc.ToString(x.FinalScore).Contains(keywords)
                || (x.PerformanceGrade != null && x.PerformanceGrade.Contains(keywords))
                || SqlFunc.ToString(x.ReviewerId).Contains(keywords)
                || (x.InterviewNotes != null && x.InterviewNotes.Contains(keywords))
                || SqlFunc.ToString(x.AssessmentStatus).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.AssessmentDate).Contains(keywords)
                || SqlFunc.ToString(x.InterviewDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EmployeeName))
        {
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(queryDto.EmployeeName));
        }

        if (!string.IsNullOrEmpty(queryDto?.AssessmentPeriod))
        {
            exp = exp.And(x => x.AssessmentPeriod != null && x.AssessmentPeriod.Contains(queryDto.AssessmentPeriod));
        }

        if (queryDto?.SchemeMetricId.HasValue == true)
        {
            exp = exp.And(x => x.SchemeMetricId == queryDto.SchemeMetricId);
        }

        if (queryDto?.SelfScore.HasValue == true)
        {
            exp = exp.And(x => x.SelfScore == queryDto.SelfScore);
        }

        if (!string.IsNullOrEmpty(queryDto?.SelfEvaluationNotes))
        {
            exp = exp.And(x => x.SelfEvaluationNotes != null && x.SelfEvaluationNotes.Contains(queryDto.SelfEvaluationNotes));
        }

        if (queryDto?.SupervisorScore.HasValue == true)
        {
            exp = exp.And(x => x.SupervisorScore == queryDto.SupervisorScore);
        }

        if (!string.IsNullOrEmpty(queryDto?.SupervisorComments))
        {
            exp = exp.And(x => x.SupervisorComments != null && x.SupervisorComments.Contains(queryDto.SupervisorComments));
        }

        if (queryDto?.FinalScore.HasValue == true)
        {
            exp = exp.And(x => x.FinalScore == queryDto.FinalScore);
        }

        if (!string.IsNullOrEmpty(queryDto?.PerformanceGrade))
        {
            exp = exp.And(x => x.PerformanceGrade != null && x.PerformanceGrade.Contains(queryDto.PerformanceGrade));
        }

        if (queryDto?.ReviewerId.HasValue == true)
        {
            exp = exp.And(x => x.ReviewerId == queryDto.ReviewerId);
        }

        if (!string.IsNullOrEmpty(queryDto?.InterviewNotes))
        {
            exp = exp.And(x => x.InterviewNotes != null && x.InterviewNotes.Contains(queryDto.InterviewNotes));
        }

        if (queryDto?.AssessmentStatus.HasValue == true)
        {
            exp = exp.And(x => x.AssessmentStatus == queryDto.AssessmentStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.RelatedPlant))
        {
            exp = exp.And(x => x.RelatedPlant != null && x.RelatedPlant.Contains(queryDto.RelatedPlant));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.AssessmentDateStart.HasValue == true)
        {
            exp = exp.And(x => x.AssessmentDate >= queryDto.AssessmentDateStart);
        }

        if (queryDto?.AssessmentDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.AssessmentDate <= queryDto.AssessmentDateEnd);
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
