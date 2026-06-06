// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.VisitorCenter
// 文件名称：TaktVisitorService.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：来访接待应用服务实现
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
/// 来访接待应用服务
/// </summary>
public class TaktVisitorService : TaktServiceBase, ITaktVisitorService
{
    private readonly ITaktCompanyRepository<TaktVisitor> _visitorRepository;
    private readonly ITaktCompanyRepository<TaktVisitorCompanion> _visitorCompanionRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="visitorRepository">来访接待仓储</param>
    /// <param name="visitorCompanionRepository">VisitorCompanion仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktVisitorService(
        ITaktCompanyRepository<TaktVisitor> visitorRepository,
        ITaktCompanyRepository<TaktVisitorCompanion> visitorCompanionRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _visitorRepository = visitorRepository;
        _visitorCompanionRepository = visitorCompanionRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取来访接待列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktVisitorDto>> GetVisitorListAsync(TaktVisitorQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _visitorRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktVisitorDto>.Create(
            data.Adapt<List<TaktVisitorDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取来访接待
    /// </summary>
    /// <param name="id">来访接待ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktVisitorDto?> GetVisitorByIdAsync(long id)
    {
        var entity = await _visitorRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktVisitorDto>();
        await FillVisitorDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取访客中心来访记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetVisitorOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _visitorRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.VisitorCompanyName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.VisitorCompanyName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建来访接待
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVisitorDto> CreateVisitorAsync(TaktVisitorCreateDto dto)
    {
        var entity = dto.Adapt<TaktVisitor>();
        var isUnique_ix_visitor_unique = await _uniqueValidator.IsUniqueAsync(
            _visitorRepository,
            x => x.VisitorCompanyName == entity.VisitorCompanyName
                && x.VisitStartTime == entity.VisitStartTime);
        if (!isUnique_ix_visitor_unique)
        {
            throw new TaktBusinessException("来访接待的VisitorCompanyName、VisitStartTime已存在");
        }
        entity = await _visitorRepository.CreateAsync(entity);
                await SaveVisitorChildrenAsync(entity, dto);
        return await GetVisitorByIdAsync(entity.Id) ?? entity.Adapt<TaktVisitorDto>();
    }

    /// <summary>
    /// 更新来访接待
    /// </summary>
    /// <param name="id">来访接待ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktVisitorDto> UpdateVisitorAsync(long id, TaktVisitorUpdateDto dto)
    {
        var entity = await _visitorRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("来访接待不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_visitor_unique = await _uniqueValidator.IsUniqueAsync(
            _visitorRepository,
            x => x.VisitorCompanyName == entity.VisitorCompanyName
                && x.VisitStartTime == entity.VisitStartTime,
            id);
        if (!isUnique_ix_visitor_unique)
        {
            throw new TaktBusinessException("来访接待的VisitorCompanyName、VisitStartTime已存在");
        }
        await _visitorRepository.UpdateAsync(entity);
                await SaveVisitorChildrenAsync(entity, dto);
        return await GetVisitorByIdAsync(id) ?? throw new TaktBusinessException("来访接待不存在");
    }

    /// <summary>
    /// 删除来访接待
    /// </summary>
    /// <param name="id">来访接待ID</param>
    /// <returns>任务</returns>
    public async Task DeleteVisitorByIdAsync(long id)
    {
        var entity = await _visitorRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("来访接待不存在或已删除");
        }
        await _visitorCompanionRepository.DeleteAsync(x => x.VisitorId == entity.Id);
        var deleted = await _visitorRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("来访接待不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除来访接待
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteVisitorBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteVisitorByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetVisitorTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktVisitorTemplateDto>(
            sheetName ?? "来访接待导入模板",
            fileName ?? "来访接待导入模板.xlsx");
    }

    /// <summary>
    /// 导入来访接待
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportVisitorAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktVisitorImportDto>(fileStream, sheetName ?? "来访接待导入模板");
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
                var entity = rows[i].Adapt<TaktVisitor>();
                var importKey = $"{entity.VisitorCompanyName}|{entity.VisitStartTime}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（VisitorCompanyName、VisitStartTime）");
                }
                var isUnique_ix_visitor_unique = await _uniqueValidator.IsUniqueAsync(
                    _visitorRepository,
                    x => x.VisitorCompanyName == entity.VisitorCompanyName
                        && x.VisitStartTime == entity.VisitStartTime);
                if (!isUnique_ix_visitor_unique)
                {
                    throw new TaktBusinessException("来访接待的VisitorCompanyName、VisitStartTime已存在");
                }
                await _visitorRepository.CreateAsync(entity);
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
    /// 导出来访接待
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportVisitorAsync(TaktVisitorQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktVisitorQueryDto());
        var list = await _visitorRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktVisitorExportDto>(),
                sheetName ?? "来访接待数据",
                fileName ?? "来访接待导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktVisitorExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "来访接待数据",
            fileName ?? "来访接待导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充来访接待详情（加载 OneToMany 子表：来访人员）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillVisitorDetailsAsync(TaktVisitorDto dto, TaktVisitor entity)
    {
        if (dto == null)
        {
            return;
        }
        // 来访人员 → dto.Companions
        var companions = await _visitorCompanionRepository.GetListAsync(x => x.VisitorId == entity.Id);
        dto.Companions = companions.Adapt<List<TaktVisitorCompanionDto>>();
    }

    /// <summary>
    /// 保存来访接待子表级联（来访人员；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveVisitorChildrenAsync(TaktVisitor entity, TaktVisitorCreateDto dto)
    {
        // 来访人员（Companions）
        if (dto.Companions is not { Count: > 0 })
        {
            await _visitorCompanionRepository.DeleteAsync(x => x.VisitorId == entity.Id);
        }
        else
        {
            var companions = dto.Companions.Adapt<List<TaktVisitorCompanion>>();
            foreach (var child in companions)
            {
                child.VisitorId = entity.Id;
            }
            await _visitorCompanionRepository.DeleteAsync(x => x.VisitorId == entity.Id);
            foreach (var child in companions)
            {
            }
            await _visitorCompanionRepository.CreateRangeAsync(companions);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建来访接待查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktVisitor, bool>> QueryExpression(TaktVisitorQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktVisitor>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.VisitorCompanyName != null && x.VisitorCompanyName.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.VisitStartTime).Contains(keywords)
                || SqlFunc.ToString(x.VisitEndTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.VisitorCompanyName))
        {
            exp = exp.And(x => x.VisitorCompanyName != null && x.VisitorCompanyName.Contains(queryDto.VisitorCompanyName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.VisitStartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.VisitStartTime >= queryDto.VisitStartTimeStart);
        }

        if (queryDto?.VisitStartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.VisitStartTime <= queryDto.VisitStartTimeEnd);
        }

        if (queryDto?.VisitEndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.VisitEndTime >= queryDto.VisitEndTimeStart);
        }

        if (queryDto?.VisitEndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.VisitEndTime <= queryDto.VisitEndTimeEnd);
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
