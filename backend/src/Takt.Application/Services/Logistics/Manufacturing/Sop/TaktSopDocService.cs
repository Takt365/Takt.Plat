// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopDocService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP文档头应用服务实现
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
/// SOP文档头应用服务
/// </summary>
public class TaktSopDocService : TaktServiceBase, ITaktSopDocService
{
    private readonly ITaktApprovalRepository<TaktSopDoc> _sopDocRepository;
    private readonly ITaktCompanyRepository<TaktSopRevision> _sopRevisionRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopDocRepository">SOP文档头仓储</param>
    /// <param name="sopRevisionRepository">SopRevision仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopDocService(
        ITaktApprovalRepository<TaktSopDoc> sopDocRepository,
        ITaktCompanyRepository<TaktSopRevision> sopRevisionRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopDocRepository = sopDocRepository;
        _sopRevisionRepository = sopRevisionRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP文档头列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopDocDto>> GetSopDocListAsync(TaktSopDocQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopDocRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopDocDto>.Create(
            data.Adapt<List<TaktSopDocDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP文档头
    /// </summary>
    /// <param name="id">SOP文档头ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopDocDto?> GetSopDocByIdAsync(long id)
    {
        var entity = await _sopDocRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSopDocDto>();
        await FillSopDocDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取SOP文档头选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopDocOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _sopDocRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SopStatus == 1,
            x => x.SopName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SopName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP文档头
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopDocDto> CreateSopDocAsync(TaktSopDocCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopDoc>();
        var isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique = await _uniqueValidator.IsUniqueAsync(
            _sopDocRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SopCode == entity.SopCode);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique)
        {
            throw new TaktBusinessException("SOP文档头的PlantCode、SopCode已存在");
        }
        entity = await _sopDocRepository.CreateAsync(entity);
                await SaveSopDocChildrenAsync(entity, dto);
        return await GetSopDocByIdAsync(entity.Id) ?? entity.Adapt<TaktSopDocDto>();
    }

    /// <summary>
    /// 更新SOP文档头
    /// </summary>
    /// <param name="id">SOP文档头ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopDocDto> UpdateSopDocAsync(long id, TaktSopDocUpdateDto dto)
    {
        var entity = await _sopDocRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP文档头不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique = await _uniqueValidator.IsUniqueAsync(
            _sopDocRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SopCode == entity.SopCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique)
        {
            throw new TaktBusinessException("SOP文档头的PlantCode、SopCode已存在");
        }
        await _sopDocRepository.UpdateAsync(entity);
                await SaveSopDocChildrenAsync(entity, dto);
        return await GetSopDocByIdAsync(id) ?? throw new TaktBusinessException("SOP文档头不存在");
    }

    /// <summary>
    /// 删除SOP文档头
    /// </summary>
    /// <param name="id">SOP文档头ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopDocByIdAsync(long id)
    {
        var entity = await _sopDocRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP文档头不存在或已删除");
        }
        await _sopRevisionRepository.DeleteAsync(x => x.SopId == entity.Id);
        var deleted = await _sopDocRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP文档头不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP文档头
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopDocBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopDocByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新SOP文档头状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopDocDto> UpdateSopDocStatusAsync(TaktSopDocStatusDto dto)
    {
        var entity = await _sopDocRepository.GetByIdAsync(dto.SopDocId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP文档头不存在");
        }
        entity.SopStatus = dto.SopStatus;
        await _sopDocRepository.UpdateAsync(entity);
        return await GetSopDocByIdAsync(dto.SopDocId) ?? throw new TaktBusinessException("SOP文档头不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopDocTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopDocTemplateDto>(
            sheetName ?? "SOP文档头导入模板",
            fileName ?? "SOP文档头导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP文档头
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopDocAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopDocImportDto>(fileStream, sheetName ?? "SOP文档头导入模板");
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
                var entity = rows[i].Adapt<TaktSopDoc>();
                var importKey = $"{entity.PlantCode}|{entity.SopCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SopCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _sopDocRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SopCode == entity.SopCode);
                if (!isUnique_ix_takt_logistics_manufacturing_sop_doc_code_unique)
                {
                    throw new TaktBusinessException("SOP文档头的PlantCode、SopCode已存在");
                }
                await _sopDocRepository.CreateAsync(entity);
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
    /// 导出SOP文档头
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopDocAsync(TaktSopDocQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopDocQueryDto());
        var list = await _sopDocRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopDocExportDto>(),
                sheetName ?? "SOP文档头数据",
                fileName ?? "SOP文档头导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopDocExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP文档头数据",
            fileName ?? "SOP文档头导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充SOP文档头详情（加载 OneToMany 子表：SOP版本）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSopDocDetailsAsync(TaktSopDocDto dto, TaktSopDoc entity)
    {
        if (dto == null)
        {
            return;
        }
        // SOP版本 → dto.Revisions
        var revisions = await _sopRevisionRepository.GetListAsync(x => x.SopId == entity.Id);
        dto.Revisions = revisions.Adapt<List<TaktSopRevisionDto>>();
    }

    /// <summary>
    /// 保存SOP文档头子表级联（SOP版本；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSopDocChildrenAsync(TaktSopDoc entity, TaktSopDocCreateDto dto)
    {
        // SOP版本（Revisions）
        if (dto.Revisions is not { Count: > 0 })
        {
            await _sopRevisionRepository.DeleteAsync(x => x.SopId == entity.Id);
        }
        else
        {
            var revisions = dto.Revisions.Adapt<List<TaktSopRevision>>();
            foreach (var child in revisions)
            {
                child.SopId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < revisions.Count; i++)
                        {
                            var key = $"{revisions[i].CompanyCode}|{revisions[i].SopId}|{revisions[i].Revision}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"SOP版本第{i + 1}项与本次提交的其他项重复（CompanyCode、SopId、Revision）");
                            }
                        }
            await _sopRevisionRepository.DeleteAsync(x => x.SopId == entity.Id);
            foreach (var child in revisions)
            {
            var isUnique_ix_takt_logistics_manufacturing_sop_revision_unique = await _uniqueValidator.IsUniqueAsync(
                _sopRevisionRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.SopId == child.SopId
                    && x.Revision == child.Revision);
            if (!isUnique_ix_takt_logistics_manufacturing_sop_revision_unique)
            {
                throw new TaktBusinessException("SOP版本的CompanyCode、SopId、Revision已存在");
            }
            }
            await _sopRevisionRepository.CreateRangeAsync(revisions);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP文档头查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopDoc, bool>> QueryExpression(TaktSopDocQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopDoc>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SopCode != null && x.SopCode.Contains(keywords))
                || (x.SopName != null && x.SopName.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || SqlFunc.ToString(x.RoutingItemId).Contains(keywords)
                || SqlFunc.ToString(x.WorkstationId).Contains(keywords)
                || SqlFunc.ToString(x.CurrentRevisionId).Contains(keywords)
                || (x.DefaultLang != null && x.DefaultLang.Contains(keywords))
                || SqlFunc.ToString(x.SopStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SopCode))
        {
            exp = exp.And(x => x.SopCode != null && x.SopCode.Contains(queryDto.SopCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SopName))
        {
            exp = exp.And(x => x.SopName != null && x.SopName.Contains(queryDto.SopName));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (queryDto?.RoutingItemId.HasValue == true)
        {
            exp = exp.And(x => x.RoutingItemId == queryDto.RoutingItemId);
        }

        if (queryDto?.WorkstationId.HasValue == true)
        {
            exp = exp.And(x => x.WorkstationId == queryDto.WorkstationId);
        }

        if (queryDto?.CurrentRevisionId.HasValue == true)
        {
            exp = exp.And(x => x.CurrentRevisionId == queryDto.CurrentRevisionId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DefaultLang))
        {
            exp = exp.And(x => x.DefaultLang != null && x.DefaultLang.Contains(queryDto.DefaultLang));
        }

        if (queryDto?.SopStatus.HasValue == true)
        {
            exp = exp.And(x => x.SopStatus == queryDto.SopStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
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
