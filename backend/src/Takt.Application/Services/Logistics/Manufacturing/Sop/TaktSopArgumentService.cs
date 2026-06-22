// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopArgumentService.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP作业参数应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP作业参数应用服务
/// </summary>
public class TaktSopArgumentService : TaktServiceBase, ITaktSopArgumentService
{
    private readonly ITaktCompanyRepository<TaktSopArgument> _sopArgumentRepository;
    private readonly ITaktCompanyRepository<TaktSopExec> _sopExecRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopArgumentRepository">SOP作业参数仓储</param>
    /// <param name="sopExecRepository">SOP工位执行仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopArgumentService(
        ITaktCompanyRepository<TaktSopArgument> sopArgumentRepository,
        ITaktCompanyRepository<TaktSopExec> sopExecRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopArgumentRepository = sopArgumentRepository;
        _sopExecRepository = sopExecRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP作业参数列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopArgumentDto>> GetSopArgumentListAsync(TaktSopArgumentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopArgumentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopArgumentDto>.Create(
            data.Adapt<List<TaktSopArgumentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP作业参数
    /// </summary>
    /// <param name="id">SOP作业参数ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopArgumentDto?> GetSopArgumentByIdAsync(long id)
    {
        var entity = await _sopArgumentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopArgumentDto>();
    }

    /// <summary>
    /// 获取SOP作业参数选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopArgumentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _sopArgumentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ParamCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ParamCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP作业参数
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopArgumentDto> CreateSopArgumentAsync(TaktSopArgumentCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopArgument>();
        await StampSopArgumentSopExecAsync(entity, dto);
        entity = await _sopArgumentRepository.CreateAsync(entity);
        return await GetSopArgumentByIdAsync(entity.Id) ?? entity.Adapt<TaktSopArgumentDto>();
    }

    /// <summary>
    /// 更新SOP作业参数
    /// </summary>
    /// <param name="id">SOP作业参数ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopArgumentDto> UpdateSopArgumentAsync(long id, TaktSopArgumentUpdateDto dto)
    {
        var entity = await _sopArgumentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP作业参数不存在");
        }
        dto.Adapt(entity);
        await StampSopArgumentSopExecAsync(entity, dto);
        await _sopArgumentRepository.UpdateAsync(entity);
        return await GetSopArgumentByIdAsync(id) ?? throw new TaktBusinessException("SOP作业参数不存在");
    }

    /// <summary>
    /// 删除SOP作业参数
    /// </summary>
    /// <param name="id">SOP作业参数ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopArgumentByIdAsync(long id)
    {
        var deleted = await _sopArgumentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP作业参数不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP作业参数
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopArgumentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopArgumentByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopArgumentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopArgumentTemplateDto>(
            sheetName ?? "SOP作业参数导入模板",
            fileName ?? "SOP作业参数导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP作业参数
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopArgumentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopArgumentImportDto>(fileStream, sheetName ?? "SOP作业参数导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopArgument>();
                var importDto = rows[i].Adapt<TaktSopArgumentCreateDto>();
                await StampSopArgumentSopExecAsync(entity, importDto);
                await _sopArgumentRepository.CreateAsync(entity);
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
    /// 导出SOP作业参数
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopArgumentAsync(TaktSopArgumentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopArgumentQueryDto());
        var list = await _sopArgumentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopArgumentExportDto>(),
                sheetName ?? "SOP作业参数数据",
                fileName ?? "SOP作业参数导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopArgumentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP作业参数数据",
            fileName ?? "SOP作业参数导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步SOP作业参数主表外键（ManyToOne → SOP工位执行）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopArgumentSopExecAsync(TaktSopArgument entity, TaktSopArgumentCreateDto dto)
    {
        if (dto.ExecId <= 0)
        {
            return;
        }
        var master = await _sopExecRepository.GetByIdAsync(dto.ExecId);
        if (master == null)
        {
            throw new TaktBusinessException("SOP工位执行不存在");
        }
        entity.ExecId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP作业参数查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopArgument, bool>> QueryExpression(TaktSopArgumentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopArgument>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.ExecId).Contains(keywords)
                || SqlFunc.ToString(x.ExecStepId).Contains(keywords)
                || SqlFunc.ToString(x.RoutingItemParameterId).Contains(keywords)
                || (x.ParamCode != null && x.ParamCode.Contains(keywords))
                || SqlFunc.ToString(x.ActualValue).Contains(keywords)
                || SqlFunc.ToString(x.IsOutOfRange).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.RecordedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.ExecId.HasValue == true)
        {
            exp = exp.And(x => x.ExecId == queryDto.ExecId);
        }

        if (queryDto?.ExecStepId.HasValue == true)
        {
            exp = exp.And(x => x.ExecStepId == queryDto.ExecStepId);
        }

        if (queryDto?.RoutingItemParameterId.HasValue == true)
        {
            exp = exp.And(x => x.RoutingItemParameterId == queryDto.RoutingItemParameterId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ParamCode))
        {
            exp = exp.And(x => x.ParamCode != null && x.ParamCode.Contains(queryDto.ParamCode));
        }

        if (queryDto?.ActualValue.HasValue == true)
        {
            exp = exp.And(x => x.ActualValue == queryDto.ActualValue);
        }

        if (queryDto?.IsOutOfRange.HasValue == true)
        {
            exp = exp.And(x => x.IsOutOfRange == queryDto.IsOutOfRange);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.RecordedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.RecordedAt >= queryDto.RecordedAtStart);
        }

        if (queryDto?.RecordedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.RecordedAt <= queryDto.RecordedAtEnd);
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
