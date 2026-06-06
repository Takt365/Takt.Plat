// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.HumanResource.TrainingDevelopment
// 文件名称：TaktTrainingResultService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：培训结果应用服务实现
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
/// 培训结果应用服务
/// </summary>
public class TaktTrainingResultService : TaktServiceBase, ITaktTrainingResultService
{
    private readonly ITaktCompanyRepository<TaktTrainingResult> _trainingResultRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="trainingResultRepository">培训结果仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTrainingResultService(
        ITaktCompanyRepository<TaktTrainingResult> trainingResultRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _trainingResultRepository = trainingResultRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取培训结果列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTrainingResultDto>> GetTrainingResultListAsync(TaktTrainingResultQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _trainingResultRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTrainingResultDto>.Create(
            data.Adapt<List<TaktTrainingResultDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取培训结果
    /// </summary>
    /// <param name="id">培训结果ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingResultDto?> GetTrainingResultByIdAsync(long id)
    {
        var entity = await _trainingResultRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTrainingResultDto>();
    }

    /// <summary>
    /// 获取培训结果选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTrainingResultOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _trainingResultRepository.GetListAsync(
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
    /// 创建培训结果
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingResultDto> CreateTrainingResultAsync(TaktTrainingResultCreateDto dto)
    {
        var entity = dto.Adapt<TaktTrainingResult>();
        entity = await _trainingResultRepository.CreateAsync(entity);
        return await GetTrainingResultByIdAsync(entity.Id) ?? entity.Adapt<TaktTrainingResultDto>();
    }

    /// <summary>
    /// 更新培训结果
    /// </summary>
    /// <param name="id">培训结果ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingResultDto> UpdateTrainingResultAsync(long id, TaktTrainingResultUpdateDto dto)
    {
        var entity = await _trainingResultRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("培训结果不存在");
        }
        dto.Adapt(entity);
        await _trainingResultRepository.UpdateAsync(entity);
        return await GetTrainingResultByIdAsync(id) ?? throw new TaktBusinessException("培训结果不存在");
    }

    /// <summary>
    /// 删除培训结果
    /// </summary>
    /// <param name="id">培训结果ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTrainingResultByIdAsync(long id)
    {
        var deleted = await _trainingResultRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("培训结果不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除培训结果
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTrainingResultBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTrainingResultByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新培训结果状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingResultDto> UpdateTrainingResultStatusAsync(TaktTrainingResultStatusDto dto)
    {
        var entity = await _trainingResultRepository.GetByIdAsync(dto.TrainingResultId);
        if (entity == null)
        {
            throw new TaktBusinessException("培训结果不存在");
        }
        entity.TrainingResultStatus = dto.TrainingResultStatus;
        await _trainingResultRepository.UpdateAsync(entity);
        return await GetTrainingResultByIdAsync(dto.TrainingResultId) ?? throw new TaktBusinessException("培训结果不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTrainingResultTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTrainingResultTemplateDto>(
            sheetName ?? "培训结果导入模板",
            fileName ?? "培训结果导入模板.xlsx");
    }

    /// <summary>
    /// 导入培训结果
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTrainingResultAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTrainingResultImportDto>(fileStream, sheetName ?? "培训结果导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTrainingResult>();
                await _trainingResultRepository.CreateAsync(entity);
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
    /// 导出培训结果
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTrainingResultAsync(TaktTrainingResultQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTrainingResultQueryDto());
        var list = await _trainingResultRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTrainingResultExportDto>(),
                sheetName ?? "培训结果数据",
                fileName ?? "培训结果导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTrainingResultExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "培训结果数据",
            fileName ?? "培训结果导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建培训结果查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTrainingResult, bool>> QueryExpression(TaktTrainingResultQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTrainingResult>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || SqlFunc.ToString(x.TrainingCourseId).Contains(keywords)
                || (x.CourseName != null && x.CourseName.Contains(keywords))
                || (x.TrainingType != null && x.TrainingType.Contains(keywords))
                || (x.Instructor != null && x.Instructor.Contains(keywords))
                || SqlFunc.ToString(x.TrainingHours).Contains(keywords)
                || SqlFunc.ToString(x.TrainingScore).Contains(keywords)
                || SqlFunc.ToString(x.IsPassed).Contains(keywords)
                || (x.CertificateNo != null && x.CertificateNo.Contains(keywords))
                || (x.TrainingEvaluation != null && x.TrainingEvaluation.Contains(keywords))
                || SqlFunc.ToString(x.TrainingResultStatus).Contains(keywords)
                || (x.RelatedPlant != null && x.RelatedPlant.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.TrainingStartDate).Contains(keywords)
                || SqlFunc.ToString(x.TrainingEndDate).Contains(keywords)
                || SqlFunc.ToString(x.TrainingDate).Contains(keywords)
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

        if (queryDto?.TrainingCourseId.HasValue == true)
        {
            exp = exp.And(x => x.TrainingCourseId == queryDto.TrainingCourseId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CourseName))
        {
            exp = exp.And(x => x.CourseName != null && x.CourseName.Contains(queryDto.CourseName));
        }

        if (!string.IsNullOrEmpty(queryDto?.TrainingType))
        {
            exp = exp.And(x => x.TrainingType != null && x.TrainingType.Contains(queryDto.TrainingType));
        }

        if (!string.IsNullOrEmpty(queryDto?.Instructor))
        {
            exp = exp.And(x => x.Instructor != null && x.Instructor.Contains(queryDto.Instructor));
        }

        if (queryDto?.TrainingHours.HasValue == true)
        {
            exp = exp.And(x => x.TrainingHours == queryDto.TrainingHours);
        }

        if (queryDto?.TrainingScore.HasValue == true)
        {
            exp = exp.And(x => x.TrainingScore == queryDto.TrainingScore);
        }

        if (queryDto?.IsPassed.HasValue == true)
        {
            exp = exp.And(x => x.IsPassed == queryDto.IsPassed);
        }

        if (!string.IsNullOrEmpty(queryDto?.CertificateNo))
        {
            exp = exp.And(x => x.CertificateNo != null && x.CertificateNo.Contains(queryDto.CertificateNo));
        }

        if (!string.IsNullOrEmpty(queryDto?.TrainingEvaluation))
        {
            exp = exp.And(x => x.TrainingEvaluation != null && x.TrainingEvaluation.Contains(queryDto.TrainingEvaluation));
        }

        if (queryDto?.TrainingResultStatus.HasValue == true)
        {
            exp = exp.And(x => x.TrainingResultStatus == queryDto.TrainingResultStatus);
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

        if (queryDto?.TrainingStartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.TrainingStartDate >= queryDto.TrainingStartDateStart);
        }

        if (queryDto?.TrainingStartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.TrainingStartDate <= queryDto.TrainingStartDateEnd);
        }

        if (queryDto?.TrainingEndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.TrainingEndDate >= queryDto.TrainingEndDateStart);
        }

        if (queryDto?.TrainingEndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.TrainingEndDate <= queryDto.TrainingEndDateEnd);
        }

        if (queryDto?.TrainingDateStart.HasValue == true)
        {
            exp = exp.And(x => x.TrainingDate >= queryDto.TrainingDateStart);
        }

        if (queryDto?.TrainingDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.TrainingDate <= queryDto.TrainingDateEnd);
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
