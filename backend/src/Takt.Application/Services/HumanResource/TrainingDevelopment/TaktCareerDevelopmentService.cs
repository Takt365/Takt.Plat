// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.TrainingDevelopment
// 文件名称：TaktCareerDevelopmentService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：职业发展应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.TrainingDevelopment;
using Takt.Domain.Entities.HumanResource.TrainingDevelopment;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.TrainingDevelopment;

/// <summary>
/// 职业发展应用服务
/// </summary>
public class TaktCareerDevelopmentService : TaktServiceBase, ITaktCareerDevelopmentService
{
    private readonly ITaktCompanyRepository<TaktCareerDevelopment> _careerDevelopmentRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="careerDevelopmentRepository">职业发展仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCareerDevelopmentService(
        ITaktCompanyRepository<TaktCareerDevelopment> careerDevelopmentRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _careerDevelopmentRepository = careerDevelopmentRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取职业发展列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCareerDevelopmentDto>> GetCareerDevelopmentListAsync(TaktCareerDevelopmentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _careerDevelopmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCareerDevelopmentDto>.Create(
            data.Adapt<List<TaktCareerDevelopmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取职业发展
    /// </summary>
    /// <param name="id">职业发展ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCareerDevelopmentDto?> GetCareerDevelopmentByIdAsync(long id)
    {
        var entity = await _careerDevelopmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCareerDevelopmentDto>();
    }

    /// <summary>
    /// 获取职业发展选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCareerDevelopmentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _careerDevelopmentRepository.GetListAsync(
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
    /// 创建职业发展
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCareerDevelopmentDto> CreateCareerDevelopmentAsync(TaktCareerDevelopmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktCareerDevelopment>();
        entity = await _careerDevelopmentRepository.CreateAsync(entity);
        return await GetCareerDevelopmentByIdAsync(entity.Id) ?? entity.Adapt<TaktCareerDevelopmentDto>();
    }

    /// <summary>
    /// 更新职业发展
    /// </summary>
    /// <param name="id">职业发展ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCareerDevelopmentDto> UpdateCareerDevelopmentAsync(long id, TaktCareerDevelopmentUpdateDto dto)
    {
        var entity = await _careerDevelopmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("职业发展不存在");
        }
        dto.Adapt(entity);
        await _careerDevelopmentRepository.UpdateAsync(entity);
        return await GetCareerDevelopmentByIdAsync(id) ?? throw new TaktBusinessException("职业发展不存在");
    }

    /// <summary>
    /// 删除职业发展
    /// </summary>
    /// <param name="id">职业发展ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCareerDevelopmentByIdAsync(long id)
    {
        var deleted = await _careerDevelopmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("职业发展不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除职业发展
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCareerDevelopmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCareerDevelopmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新职业发展状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCareerDevelopmentDto> UpdateCareerDevelopmentStatusAsync(TaktCareerDevelopmentStatusDto dto)
    {
        var entity = await _careerDevelopmentRepository.GetByIdAsync(dto.CareerDevelopmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("职业发展不存在");
        }
        entity.CareerDevelopmentStatus = dto.CareerDevelopmentStatus;
        await _careerDevelopmentRepository.UpdateAsync(entity);
        return await GetCareerDevelopmentByIdAsync(dto.CareerDevelopmentId) ?? throw new TaktBusinessException("职业发展不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCareerDevelopmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCareerDevelopmentTemplateDto>(
            sheetName ?? "职业发展导入模板",
            fileName ?? "职业发展导入模板.xlsx");
    }

    /// <summary>
    /// 导入职业发展
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCareerDevelopmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCareerDevelopmentImportDto>(fileStream, sheetName ?? "职业发展导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktCareerDevelopment>();
                await _careerDevelopmentRepository.CreateAsync(entity);
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
    /// 导出职业发展
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCareerDevelopmentAsync(TaktCareerDevelopmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktCareerDevelopmentQueryDto());
        var list = await _careerDevelopmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCareerDevelopmentExportDto>(),
                sheetName ?? "职业发展数据",
                fileName ?? "职业发展导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCareerDevelopmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "职业发展数据",
            fileName ?? "职业发展导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建职业发展查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCareerDevelopment, bool>> QueryExpression(TaktCareerDevelopmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCareerDevelopment>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.SkillCategory != null && x.SkillCategory.Contains(keywords))
                || (x.SkillName != null && x.SkillName.Contains(keywords))
                || (x.AssessmentMethod != null && x.AssessmentMethod.Contains(keywords))
                || SqlFunc.ToString(x.AssessmentScore).Contains(keywords)
                || (x.SkillLevel != null && x.SkillLevel.Contains(keywords))
                || (x.TargetPosition != null && x.TargetPosition.Contains(keywords))
                || (x.DevelopmentPlan != null && x.DevelopmentPlan.Contains(keywords))
                || (x.ImprovementSuggestions != null && x.ImprovementSuggestions.Contains(keywords))
                || SqlFunc.ToString(x.CareerDevelopmentStatus).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.AssessmentDate).Contains(keywords)
                || SqlFunc.ToString(x.NextAssessmentDate).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.SkillCategory))
        {
            exp = exp.And(x => x.SkillCategory != null && x.SkillCategory.Contains(queryDto.SkillCategory));
        }

        if (!string.IsNullOrEmpty(queryDto?.SkillName))
        {
            exp = exp.And(x => x.SkillName != null && x.SkillName.Contains(queryDto.SkillName));
        }

        if (!string.IsNullOrEmpty(queryDto?.AssessmentMethod))
        {
            exp = exp.And(x => x.AssessmentMethod != null && x.AssessmentMethod.Contains(queryDto.AssessmentMethod));
        }

        if (queryDto?.AssessmentScore.HasValue == true)
        {
            exp = exp.And(x => x.AssessmentScore == queryDto.AssessmentScore);
        }

        if (!string.IsNullOrEmpty(queryDto?.SkillLevel))
        {
            exp = exp.And(x => x.SkillLevel != null && x.SkillLevel.Contains(queryDto.SkillLevel));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetPosition))
        {
            exp = exp.And(x => x.TargetPosition != null && x.TargetPosition.Contains(queryDto.TargetPosition));
        }

        if (!string.IsNullOrEmpty(queryDto?.DevelopmentPlan))
        {
            exp = exp.And(x => x.DevelopmentPlan != null && x.DevelopmentPlan.Contains(queryDto.DevelopmentPlan));
        }

        if (!string.IsNullOrEmpty(queryDto?.ImprovementSuggestions))
        {
            exp = exp.And(x => x.ImprovementSuggestions != null && x.ImprovementSuggestions.Contains(queryDto.ImprovementSuggestions));
        }

        if (queryDto?.CareerDevelopmentStatus.HasValue == true)
        {
            exp = exp.And(x => x.CareerDevelopmentStatus == queryDto.CareerDevelopmentStatus);
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

        if (queryDto?.NextAssessmentDateStart.HasValue == true)
        {
            exp = exp.And(x => x.NextAssessmentDate >= queryDto.NextAssessmentDateStart);
        }

        if (queryDto?.NextAssessmentDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.NextAssessmentDate <= queryDto.NextAssessmentDateEnd);
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
