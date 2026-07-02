// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepMediaService.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工步多媒体应用服务实现
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
/// SOP工步多媒体应用服务
/// </summary>
public class TaktSopStepMediaService : TaktServiceBase, ITaktSopStepMediaService
{
    private readonly ITaktCompanyRepository<TaktSopStepMedia> _sopStepMediaRepository;
    private readonly ITaktCompanyRepository<TaktSopStep> _sopStepRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopStepMediaRepository">SOP工步多媒体仓储</param>
    /// <param name="sopStepRepository">SOP工步仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopStepMediaService(
        ITaktCompanyRepository<TaktSopStepMedia> sopStepMediaRepository,
        ITaktCompanyRepository<TaktSopStep> sopStepRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopStepMediaRepository = sopStepMediaRepository;
        _sopStepRepository = sopStepRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP工步多媒体列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopStepMediaDto>> GetSopStepMediaListAsync(TaktSopStepMediaQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopStepMediaRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopStepMediaDto>.Create(
            data.Adapt<List<TaktSopStepMediaDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP工步多媒体
    /// </summary>
    /// <param name="id">SOP工步多媒体ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepMediaDto?> GetSopStepMediaByIdAsync(long id)
    {
        var entity = await _sopStepMediaRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopStepMediaDto>();
    }

    /// <summary>
    /// 获取SOP工步多媒体选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopStepMediaOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _sopStepMediaRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.FileUrl ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FileUrl ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP工步多媒体
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepMediaDto> CreateSopStepMediaAsync(TaktSopStepMediaCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopStepMedia>();
        await StampSopStepMediaSopStepAsync(entity, dto);
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _sopStepMediaRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.StepId == entity.StepId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.StepId, maxSort);
        }
        entity = await _sopStepMediaRepository.CreateAsync(entity);
        return await GetSopStepMediaByIdAsync(entity.Id) ?? entity.Adapt<TaktSopStepMediaDto>();
    }

    /// <summary>
    /// 更新SOP工步多媒体
    /// </summary>
    /// <param name="id">SOP工步多媒体ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepMediaDto> UpdateSopStepMediaAsync(long id, TaktSopStepMediaUpdateDto dto)
    {
        var entity = await _sopStepMediaRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工步多媒体不存在");
        }
        dto.Adapt(entity);
        await StampSopStepMediaSopStepAsync(entity, dto);
        await _sopStepMediaRepository.UpdateAsync(entity);
        return await GetSopStepMediaByIdAsync(id) ?? throw new TaktBusinessException("SOP工步多媒体不存在");
    }

    /// <summary>
    /// 删除SOP工步多媒体
    /// </summary>
    /// <param name="id">SOP工步多媒体ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopStepMediaByIdAsync(long id)
    {
        var deleted = await _sopStepMediaRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP工步多媒体不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP工步多媒体
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopStepMediaBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopStepMediaByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新SOP工步多媒体排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopStepMediaDto> UpdateSopStepMediaSortAsync(TaktSopStepMediaSortDto dto)
    {
        var entity = await _sopStepMediaRepository.GetByIdAsync(dto.SopStepMediaId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工步多媒体不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _sopStepMediaRepository.UpdateAsync(entity);
        return await GetSopStepMediaByIdAsync(dto.SopStepMediaId) ?? throw new TaktBusinessException("SOP工步多媒体不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopStepMediaTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopStepMediaTemplateDto>(
            sheetName ?? "SOP工步多媒体导入模板",
            fileName ?? "SOP工步多媒体导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP工步多媒体
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopStepMediaAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopStepMediaImportDto>(fileStream, sheetName ?? "SOP工步多媒体导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopStepMedia>();
                var importDto = rows[i].Adapt<TaktSopStepMediaCreateDto>();
                await StampSopStepMediaSopStepAsync(entity, importDto);
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _sopStepMediaRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.StepId == entity.StepId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.StepId, maxSort);
                }
                await _sopStepMediaRepository.CreateAsync(entity);
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
    /// 导出SOP工步多媒体
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopStepMediaAsync(TaktSopStepMediaQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopStepMediaQueryDto());
        var list = await _sopStepMediaRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopStepMediaExportDto>(),
                sheetName ?? "SOP工步多媒体数据",
                fileName ?? "SOP工步多媒体导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopStepMediaExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP工步多媒体数据",
            fileName ?? "SOP工步多媒体导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步SOP工步多媒体主表外键（ManyToOne → SOP工步）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopStepMediaSopStepAsync(TaktSopStepMedia entity, TaktSopStepMediaCreateDto dto)
    {
        if (dto.StepId <= 0)
        {
            return;
        }
        var master = await _sopStepRepository.GetByIdAsync(dto.StepId);
        if (master == null)
        {
            throw new TaktBusinessException("SOP工步不存在");
        }
        entity.StepId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP工步多媒体查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopStepMedia, bool>> QueryExpression(TaktSopStepMediaQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopStepMedia>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.StepId).Contains(keywords)
                || SqlFunc.ToString(x.MediaType).Contains(keywords)
                || (x.FileUrl != null && x.FileUrl.Contains(keywords))
                || (x.FileExt != null && x.FileExt.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.StepId.HasValue == true)
        {
            exp = exp.And(x => x.StepId == queryDto.StepId);
        }

        if (queryDto?.MediaType.HasValue == true)
        {
            exp = exp.And(x => x.MediaType == queryDto.MediaType);
        }

        if (!string.IsNullOrEmpty(queryDto?.FileUrl))
        {
            exp = exp.And(x => x.FileUrl != null && x.FileUrl.Contains(queryDto.FileUrl));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileExt))
        {
            exp = exp.And(x => x.FileExt != null && x.FileExt.Contains(queryDto.FileExt));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
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
