// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：TaktNewsFavoriteService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心收藏记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.NewsCenter;
using Takt.Domain.Entities.Routine.NewsCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.NewsCenter;

/// <summary>
/// 新闻中心收藏记录应用服务
/// </summary>
public class TaktNewsFavoriteService : TaktServiceBase, ITaktNewsFavoriteService
{
    private readonly ITaktCompanyRepository<TaktNewsFavorite> _newsFavoriteRepository;
    private readonly ITaktApprovalRepository<TaktNews> _newsRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsFavoriteRepository">新闻中心收藏记录仓储</param>
    /// <param name="newsRepository">新闻中心仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNewsFavoriteService(
        ITaktCompanyRepository<TaktNewsFavorite> newsFavoriteRepository,
        ITaktApprovalRepository<TaktNews> newsRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _newsFavoriteRepository = newsFavoriteRepository;
        _newsRepository = newsRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取新闻中心收藏记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNewsFavoriteDto>> GetNewsFavoriteListAsync(TaktNewsFavoriteQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _newsFavoriteRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNewsFavoriteDto>.Create(
            data.Adapt<List<TaktNewsFavoriteDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取新闻中心收藏记录
    /// </summary>
    /// <param name="id">新闻中心收藏记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsFavoriteDto?> GetNewsFavoriteByIdAsync(long id)
    {
        var entity = await _newsFavoriteRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktNewsFavoriteDto>();
    }

    /// <summary>
    /// 获取新闻收藏记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetNewsFavoriteOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _newsFavoriteRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.UserName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.UserName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建新闻中心收藏记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsFavoriteDto> CreateNewsFavoriteAsync(TaktNewsFavoriteCreateDto dto)
    {
        var entity = dto.Adapt<TaktNewsFavorite>();
        await StampNewsFavoriteNewsAsync(entity, dto);
        var isUnique_ix_news_favorite_unique = await _uniqueValidator.IsUniqueAsync(
            _newsFavoriteRepository,
            x => x.NewsId == entity.NewsId
                && x.UserId == entity.UserId);
        if (!isUnique_ix_news_favorite_unique)
        {
            throw new TaktBusinessException("新闻中心收藏记录的NewsId、UserId已存在");
        }
        entity = await _newsFavoriteRepository.CreateAsync(entity);
        return await GetNewsFavoriteByIdAsync(entity.Id) ?? entity.Adapt<TaktNewsFavoriteDto>();
    }

    /// <summary>
    /// 更新新闻中心收藏记录
    /// </summary>
    /// <param name="id">新闻中心收藏记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsFavoriteDto> UpdateNewsFavoriteAsync(long id, TaktNewsFavoriteUpdateDto dto)
    {
        var entity = await _newsFavoriteRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心收藏记录不存在");
        }
        dto.Adapt(entity);
        await StampNewsFavoriteNewsAsync(entity, dto);
        var isUnique_ix_news_favorite_unique = await _uniqueValidator.IsUniqueAsync(
            _newsFavoriteRepository,
            x => x.NewsId == entity.NewsId
                && x.UserId == entity.UserId,
            id);
        if (!isUnique_ix_news_favorite_unique)
        {
            throw new TaktBusinessException("新闻中心收藏记录的NewsId、UserId已存在");
        }
        await _newsFavoriteRepository.UpdateAsync(entity);
        return await GetNewsFavoriteByIdAsync(id) ?? throw new TaktBusinessException("新闻中心收藏记录不存在");
    }

    /// <summary>
    /// 删除新闻中心收藏记录
    /// </summary>
    /// <param name="id">新闻中心收藏记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsFavoriteByIdAsync(long id)
    {
        var deleted = await _newsFavoriteRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("新闻中心收藏记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除新闻中心收藏记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsFavoriteBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteNewsFavoriteByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNewsFavoriteTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNewsFavoriteTemplateDto>(
            sheetName ?? "新闻中心收藏记录导入模板",
            fileName ?? "新闻中心收藏记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入新闻中心收藏记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNewsFavoriteAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNewsFavoriteImportDto>(fileStream, sheetName ?? "新闻中心收藏记录导入模板");
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
                var entity = rows[i].Adapt<TaktNewsFavorite>();
                var importDto = rows[i].Adapt<TaktNewsFavoriteCreateDto>();
                await StampNewsFavoriteNewsAsync(entity, importDto);
                var importKey = $"{entity.NewsId}|{entity.UserId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（NewsId、UserId）");
                }
                var isUnique_ix_news_favorite_unique = await _uniqueValidator.IsUniqueAsync(
                    _newsFavoriteRepository,
                    x => x.NewsId == entity.NewsId
                        && x.UserId == entity.UserId);
                if (!isUnique_ix_news_favorite_unique)
                {
                    throw new TaktBusinessException("新闻中心收藏记录的NewsId、UserId已存在");
                }
                await _newsFavoriteRepository.CreateAsync(entity);
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
    /// 导出新闻中心收藏记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNewsFavoriteAsync(TaktNewsFavoriteQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktNewsFavoriteQueryDto());
        var list = await _newsFavoriteRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsFavoriteExportDto>(),
                sheetName ?? "新闻中心收藏记录数据",
                fileName ?? "新闻中心收藏记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNewsFavoriteExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "新闻中心收藏记录数据",
            fileName ?? "新闻中心收藏记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步新闻中心收藏记录主表外键（ManyToOne → 新闻中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampNewsFavoriteNewsAsync(TaktNewsFavorite entity, TaktNewsFavoriteCreateDto dto)
    {
        if (dto.NewsId <= 0)
        {
            return;
        }
        var master = await _newsRepository.GetByIdAsync(dto.NewsId);
        if (master == null)
        {
            throw new TaktBusinessException("新闻中心不存在");
        }
        entity.NewsId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建新闻中心收藏记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNewsFavorite, bool>> QueryExpression(TaktNewsFavoriteQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNewsFavorite>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.NewsId).Contains(keywords)
                || SqlFunc.ToString(x.UserId).Contains(keywords)
                || (x.UserName != null && x.UserName.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.FavoriteTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.NewsId.HasValue == true)
        {
            exp = exp.And(x => x.NewsId == queryDto.NewsId);
        }

        if (queryDto?.UserId.HasValue == true)
        {
            exp = exp.And(x => x.UserId == queryDto.UserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.UserName))
        {
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(queryDto.UserName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.FavoriteTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.FavoriteTime >= queryDto.FavoriteTimeStart);
        }

        if (queryDto?.FavoriteTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.FavoriteTime <= queryDto.FavoriteTimeEnd);
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
