// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopRevisionService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP版本应用服务实现
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
/// SOP版本应用服务
/// </summary>
public class TaktSopRevisionService : TaktServiceBase, ITaktSopRevisionService
{
    private readonly ITaktCompanyRepository<TaktSopRevision> _sopRevisionRepository;
    private readonly ITaktCompanyRepository<TaktSopContent> _sopContentRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopRevisionRepository">SOP版本仓储</param>
    /// <param name="sopContentRepository">SopContent仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopRevisionService(
        ITaktCompanyRepository<TaktSopRevision> sopRevisionRepository,
        ITaktCompanyRepository<TaktSopContent> sopContentRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopRevisionRepository = sopRevisionRepository;
        _sopContentRepository = sopContentRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP版本列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopRevisionDto>> GetSopRevisionListAsync(TaktSopRevisionQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopRevisionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopRevisionDto>.Create(
            data.Adapt<List<TaktSopRevisionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP版本
    /// </summary>
    /// <param name="id">SOP版本ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopRevisionDto?> GetSopRevisionByIdAsync(long id)
    {
        var entity = await _sopRevisionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSopRevisionDto>();
        await FillSopRevisionDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取SOP版本选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopRevisionOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _sopRevisionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RevisionStatus == 1,
            x => x.Revision ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.Revision ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP版本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopRevisionDto> CreateSopRevisionAsync(TaktSopRevisionCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopRevision>();
        var isUnique_ix_takt_logistics_manufacturing_sop_revision_unique = await _uniqueValidator.IsUniqueAsync(
            _sopRevisionRepository,
            x => x.SopId == entity.SopId
                && x.Revision == entity.Revision);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_revision_unique)
        {
            throw new TaktBusinessException("SOP版本的SopId、Revision已存在");
        }
        entity = await _sopRevisionRepository.CreateAsync(entity);
                await SaveSopRevisionChildrenAsync(entity, dto);
        return await GetSopRevisionByIdAsync(entity.Id) ?? entity.Adapt<TaktSopRevisionDto>();
    }

    /// <summary>
    /// 更新SOP版本
    /// </summary>
    /// <param name="id">SOP版本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopRevisionDto> UpdateSopRevisionAsync(long id, TaktSopRevisionUpdateDto dto)
    {
        var entity = await _sopRevisionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP版本不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_sop_revision_unique = await _uniqueValidator.IsUniqueAsync(
            _sopRevisionRepository,
            x => x.SopId == entity.SopId
                && x.Revision == entity.Revision,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_revision_unique)
        {
            throw new TaktBusinessException("SOP版本的SopId、Revision已存在");
        }
        await _sopRevisionRepository.UpdateAsync(entity);
                await SaveSopRevisionChildrenAsync(entity, dto);
        return await GetSopRevisionByIdAsync(id) ?? throw new TaktBusinessException("SOP版本不存在");
    }

    /// <summary>
    /// 删除SOP版本
    /// </summary>
    /// <param name="id">SOP版本ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopRevisionByIdAsync(long id)
    {
        var entity = await _sopRevisionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP版本不存在或已删除");
        }
        await _sopContentRepository.DeleteAsync(x => x.RevisionId == entity.Id);
        var deleted = await _sopRevisionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP版本不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP版本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopRevisionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopRevisionByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新SOP版本状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopRevisionDto> UpdateSopRevisionStatusAsync(TaktSopRevisionStatusDto dto)
    {
        var entity = await _sopRevisionRepository.GetByIdAsync(dto.SopRevisionId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP版本不存在");
        }
        entity.RevisionStatus = dto.RevisionStatus;
        await _sopRevisionRepository.UpdateAsync(entity);
        return await GetSopRevisionByIdAsync(dto.SopRevisionId) ?? throw new TaktBusinessException("SOP版本不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopRevisionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopRevisionTemplateDto>(
            sheetName ?? "SOP版本导入模板",
            fileName ?? "SOP版本导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP版本
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopRevisionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopRevisionImportDto>(fileStream, sheetName ?? "SOP版本导入模板");
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
                var entity = rows[i].Adapt<TaktSopRevision>();
                var importKey = $"{entity.SopId}|{entity.Revision}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SopId、Revision）");
                }
                var isUnique_ix_takt_logistics_manufacturing_sop_revision_unique = await _uniqueValidator.IsUniqueAsync(
                    _sopRevisionRepository,
                    x => x.SopId == entity.SopId
                        && x.Revision == entity.Revision);
                if (!isUnique_ix_takt_logistics_manufacturing_sop_revision_unique)
                {
                    throw new TaktBusinessException("SOP版本的SopId、Revision已存在");
                }
                await _sopRevisionRepository.CreateAsync(entity);
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
    /// 导出SOP版本
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopRevisionAsync(TaktSopRevisionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopRevisionQueryDto());
        var list = await _sopRevisionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopRevisionExportDto>(),
                sheetName ?? "SOP版本数据",
                fileName ?? "SOP版本导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopRevisionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP版本数据",
            fileName ?? "SOP版本导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充SOP版本详情（加载 OneToMany 子表：SOP多语言正文）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSopRevisionDetailsAsync(TaktSopRevisionDto dto, TaktSopRevision entity)
    {
        if (dto == null)
        {
            return;
        }
        // SOP多语言正文 → dto.Contents
        var contents = await _sopContentRepository.GetListAsync(x => x.RevisionId == entity.Id);
        dto.Contents = contents.Adapt<List<TaktSopContentDto>>();
    }

    /// <summary>
    /// 保存SOP版本子表级联（SOP多语言正文；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSopRevisionChildrenAsync(TaktSopRevision entity, TaktSopRevisionCreateDto dto)
    {
        // SOP多语言正文（Contents）
        if (dto.Contents is not { Count: > 0 })
        {
            await _sopContentRepository.DeleteAsync(x => x.RevisionId == entity.Id);
        }
        else
        {
            var contents = dto.Contents.Adapt<List<TaktSopContent>>();
            foreach (var child in contents)
            {
                child.RevisionId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < contents.Count; i++)
                        {
                            var key = $"{contents[i].CompanyCode}|{contents[i].RevisionId}|{contents[i].ContentLang}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"SOP多语言正文第{i + 1}项与本次提交的其他项重复（CompanyCode、RevisionId、ContentLang）");
                            }
                        }
            await _sopContentRepository.DeleteAsync(x => x.RevisionId == entity.Id);
            foreach (var child in contents)
            {
            var isUnique_ix_takt_logistics_manufacturing_sop_content_lang_unique = await _uniqueValidator.IsUniqueAsync(
                _sopContentRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.RevisionId == child.RevisionId
                    && x.ContentLang == child.ContentLang);
            if (!isUnique_ix_takt_logistics_manufacturing_sop_content_lang_unique)
            {
                throw new TaktBusinessException("SOP多语言正文的CompanyCode、RevisionId、ContentLang已存在");
            }
            }
            await _sopContentRepository.CreateRangeAsync(contents);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP版本查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopRevision, bool>> QueryExpression(TaktSopRevisionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopRevision>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.SopId).Contains(keywords)
                || (x.Revision != null && x.Revision.Contains(keywords))
                || (x.FileUrl != null && x.FileUrl.Contains(keywords))
                || (x.ChangeDesc != null && x.ChangeDesc.Contains(keywords))
                || SqlFunc.ToString(x.EcnId).Contains(keywords)
                || SqlFunc.ToString(x.IsLocked).Contains(keywords)
                || SqlFunc.ToString(x.ForceLeaderAck).Contains(keywords)
                || SqlFunc.ToString(x.RevisionStatus).Contains(keywords)
                || SqlFunc.ToString(x.EffectiveRule).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.SopId.HasValue == true)
        {
            exp = exp.And(x => x.SopId == queryDto.SopId);
        }

        if (!string.IsNullOrEmpty(queryDto?.Revision))
        {
            exp = exp.And(x => x.Revision != null && x.Revision.Contains(queryDto.Revision));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileUrl))
        {
            exp = exp.And(x => x.FileUrl != null && x.FileUrl.Contains(queryDto.FileUrl));
        }

        if (!string.IsNullOrEmpty(queryDto?.ChangeDesc))
        {
            exp = exp.And(x => x.ChangeDesc != null && x.ChangeDesc.Contains(queryDto.ChangeDesc));
        }

        if (queryDto?.EcnId.HasValue == true)
        {
            exp = exp.And(x => x.EcnId == queryDto.EcnId);
        }

        if (queryDto?.IsLocked.HasValue == true)
        {
            exp = exp.And(x => x.IsLocked == queryDto.IsLocked);
        }

        if (queryDto?.ForceLeaderAck.HasValue == true)
        {
            exp = exp.And(x => x.ForceLeaderAck == queryDto.ForceLeaderAck);
        }

        if (queryDto?.RevisionStatus.HasValue == true)
        {
            exp = exp.And(x => x.RevisionStatus == queryDto.RevisionStatus);
        }

        if (queryDto?.EffectiveRule.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveRule == queryDto.EffectiveRule);
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
