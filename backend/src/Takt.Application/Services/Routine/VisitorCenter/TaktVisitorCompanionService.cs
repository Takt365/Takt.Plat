// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.VisitorCenter
// 文件名称：TaktVisitorCompanionService.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：来访人员应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.VisitorCenter;
using Takt.Domain.Entities.Routine.VisitorCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.VisitorCenter;

/// <summary>
/// 来访人员应用服务
/// </summary>
public class TaktVisitorCompanionService : TaktServiceBase, ITaktVisitorCompanionService
{
    private readonly ITaktCompanyRepository<TaktVisitorCompanion> _visitorCompanionRepository;
    private readonly ITaktCompanyRepository<TaktVisitor> _visitorRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="visitorCompanionRepository">来访人员仓储</param>
    /// <param name="visitorRepository">来访接待仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktVisitorCompanionService(
        ITaktCompanyRepository<TaktVisitorCompanion> visitorCompanionRepository,
        ITaktCompanyRepository<TaktVisitor> visitorRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _visitorCompanionRepository = visitorCompanionRepository;
        _visitorRepository = visitorRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取来访人员列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktVisitorCompanionDto>> GetVisitorCompanionListAsync(TaktVisitorCompanionQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _visitorCompanionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktVisitorCompanionDto>.Create(
            data.Adapt<List<TaktVisitorCompanionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取来访人员
    /// </summary>
    /// <param name="id">来访人员ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktVisitorCompanionDto?> GetVisitorCompanionByIdAsync(long id)
    {
        var entity = await _visitorCompanionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktVisitorCompanionDto>();
    }

    /// <summary>
    /// 获取来访人员选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetVisitorCompanionOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _visitorCompanionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CompanionName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CompanionName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建来访人员
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVisitorCompanionDto> CreateVisitorCompanionAsync(TaktVisitorCompanionCreateDto dto)
    {
        var entity = dto.Adapt<TaktVisitorCompanion>();
        await StampVisitorCompanionVisitorAsync(entity, dto);
        entity = await _visitorCompanionRepository.CreateAsync(entity);
        return await GetVisitorCompanionByIdAsync(entity.Id) ?? entity.Adapt<TaktVisitorCompanionDto>();
    }

    /// <summary>
    /// 更新来访人员
    /// </summary>
    /// <param name="id">来访人员ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVisitorCompanionDto> UpdateVisitorCompanionAsync(long id, TaktVisitorCompanionUpdateDto dto)
    {
        var entity = await _visitorCompanionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("来访人员不存在");
        }
        dto.Adapt(entity);
        await StampVisitorCompanionVisitorAsync(entity, dto);
        await _visitorCompanionRepository.UpdateAsync(entity);
        return await GetVisitorCompanionByIdAsync(id) ?? throw new TaktBusinessException("来访人员不存在");
    }

    /// <summary>
    /// 删除来访人员
    /// </summary>
    /// <param name="id">来访人员ID</param>
    /// <returns>任务</returns>
    public async Task DeleteVisitorCompanionByIdAsync(long id)
    {
        var deleted = await _visitorCompanionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("来访人员不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除来访人员
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteVisitorCompanionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteVisitorCompanionByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetVisitorCompanionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktVisitorCompanionTemplateDto>(
            sheetName ?? "来访人员导入模板",
            fileName ?? "来访人员导入模板.xlsx");
    }

    /// <summary>
    /// 导入来访人员
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportVisitorCompanionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktVisitorCompanionImportDto>(fileStream, sheetName ?? "来访人员导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktVisitorCompanion>();
                var importDto = rows[i].Adapt<TaktVisitorCompanionCreateDto>();
                await StampVisitorCompanionVisitorAsync(entity, importDto);
                await _visitorCompanionRepository.CreateAsync(entity);
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
    /// 导出来访人员
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportVisitorCompanionAsync(TaktVisitorCompanionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktVisitorCompanionQueryDto());
        var list = await _visitorCompanionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktVisitorCompanionExportDto>(),
                sheetName ?? "来访人员数据",
                fileName ?? "来访人员导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktVisitorCompanionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "来访人员数据",
            fileName ?? "来访人员导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步来访人员主表外键（ManyToOne → 来访接待）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampVisitorCompanionVisitorAsync(TaktVisitorCompanion entity, TaktVisitorCompanionCreateDto dto)
    {
        if (dto.VisitorId <= 0)
        {
            return;
        }
        var master = await _visitorRepository.GetByIdAsync(dto.VisitorId);
        if (master == null)
        {
            throw new TaktBusinessException("来访接待不存在");
        }
        entity.VisitorId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建来访人员查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktVisitorCompanion, bool>> QueryExpression(TaktVisitorCompanionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktVisitorCompanion>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.VisitorId).Contains(keywords)
                || (x.Department != null && x.Department.Contains(keywords))
                || (x.JobTitle != null && x.JobTitle.Contains(keywords))
                || (x.CompanionName != null && x.CompanionName.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.VisitorId.HasValue == true)
        {
            exp = exp.And(x => x.VisitorId == queryDto.VisitorId);
        }

        if (!string.IsNullOrEmpty(queryDto?.Department))
        {
            exp = exp.And(x => x.Department != null && x.Department.Contains(queryDto.Department));
        }

        if (!string.IsNullOrEmpty(queryDto?.JobTitle))
        {
            exp = exp.And(x => x.JobTitle != null && x.JobTitle.Contains(queryDto.JobTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.CompanionName))
        {
            exp = exp.And(x => x.CompanionName != null && x.CompanionName.Contains(queryDto.CompanionName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
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
