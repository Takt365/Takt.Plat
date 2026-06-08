// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：TaktNewsLikeService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心点赞记录应用服务实现
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
/// 新闻中心点赞记录应用服务
/// </summary>
public class TaktNewsLikeService : TaktServiceBase, ITaktNewsLikeService
{
    private readonly ITaktCompanyRepository<TaktNewsLike> _newsLikeRepository;
    private readonly ITaktApprovalRepository<TaktNews> _newsRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsLikeRepository">新闻中心点赞记录仓储</param>
    /// <param name="newsRepository">新闻中心仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNewsLikeService(
        ITaktCompanyRepository<TaktNewsLike> newsLikeRepository,
        ITaktApprovalRepository<TaktNews> newsRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _newsLikeRepository = newsLikeRepository;
        _newsRepository = newsRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取新闻中心点赞记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNewsLikeDto>> GetNewsLikeListAsync(TaktNewsLikeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _newsLikeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNewsLikeDto>.Create(
            data.Adapt<List<TaktNewsLikeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取新闻中心点赞记录
    /// </summary>
    /// <param name="id">新闻中心点赞记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsLikeDto?> GetNewsLikeByIdAsync(long id)
    {
        var entity = await _newsLikeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktNewsLikeDto>();
    }

    /// <summary>
    /// 获取新闻点赞记录选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetNewsLikeOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _newsLikeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.UserName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.UserName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建新闻中心点赞记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsLikeDto> CreateNewsLikeAsync(TaktNewsLikeCreateDto dto)
    {
        var entity = dto.Adapt<TaktNewsLike>();
        await StampNewsLikeNewsAsync(entity, dto);
        var isUnique_ix_news_like_unique = await _uniqueValidator.IsUniqueAsync(
            _newsLikeRepository,
            x => x.NewsId == entity.NewsId
                && x.UserId == entity.UserId);
        if (!isUnique_ix_news_like_unique)
        {
            throw new TaktBusinessException("新闻中心点赞记录的NewsId、UserId已存在");
        }
        entity = await _newsLikeRepository.CreateAsync(entity);
        return await GetNewsLikeByIdAsync(entity.Id) ?? entity.Adapt<TaktNewsLikeDto>();
    }

    /// <summary>
    /// 更新新闻中心点赞记录
    /// </summary>
    /// <param name="id">新闻中心点赞记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsLikeDto> UpdateNewsLikeAsync(long id, TaktNewsLikeUpdateDto dto)
    {
        var entity = await _newsLikeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心点赞记录不存在");
        }
        dto.Adapt(entity);
        await StampNewsLikeNewsAsync(entity, dto);
        var isUnique_ix_news_like_unique = await _uniqueValidator.IsUniqueAsync(
            _newsLikeRepository,
            x => x.NewsId == entity.NewsId
                && x.UserId == entity.UserId,
            id);
        if (!isUnique_ix_news_like_unique)
        {
            throw new TaktBusinessException("新闻中心点赞记录的NewsId、UserId已存在");
        }
        await _newsLikeRepository.UpdateAsync(entity);
        return await GetNewsLikeByIdAsync(id) ?? throw new TaktBusinessException("新闻中心点赞记录不存在");
    }

    /// <summary>
    /// 删除新闻中心点赞记录
    /// </summary>
    /// <param name="id">新闻中心点赞记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsLikeByIdAsync(long id)
    {
        var deleted = await _newsLikeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("新闻中心点赞记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除新闻中心点赞记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsLikeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteNewsLikeByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNewsLikeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNewsLikeTemplateDto>(
            sheetName ?? "新闻中心点赞记录导入模板",
            fileName ?? "新闻中心点赞记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入新闻中心点赞记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNewsLikeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNewsLikeImportDto>(fileStream, sheetName ?? "新闻中心点赞记录导入模板");
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
                var entity = rows[i].Adapt<TaktNewsLike>();
                var importDto = rows[i].Adapt<TaktNewsLikeCreateDto>();
                await StampNewsLikeNewsAsync(entity, importDto);
                var importKey = $"{entity.NewsId}|{entity.UserId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（NewsId、UserId）");
                }
                var isUnique_ix_news_like_unique = await _uniqueValidator.IsUniqueAsync(
                    _newsLikeRepository,
                    x => x.NewsId == entity.NewsId
                        && x.UserId == entity.UserId);
                if (!isUnique_ix_news_like_unique)
                {
                    throw new TaktBusinessException("新闻中心点赞记录的NewsId、UserId已存在");
                }
                await _newsLikeRepository.CreateAsync(entity);
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
    /// 导出新闻中心点赞记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNewsLikeAsync(TaktNewsLikeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktNewsLikeQueryDto());
        var list = await _newsLikeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsLikeExportDto>(),
                sheetName ?? "新闻中心点赞记录数据",
                fileName ?? "新闻中心点赞记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNewsLikeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "新闻中心点赞记录数据",
            fileName ?? "新闻中心点赞记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步新闻中心点赞记录主表外键（ManyToOne → 新闻中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampNewsLikeNewsAsync(TaktNewsLike entity, TaktNewsLikeCreateDto dto)
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
    /// 构建新闻中心点赞记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNewsLike, bool>> QueryExpression(TaktNewsLikeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNewsLike>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.NewsId).Contains(keywords)
                || SqlFunc.ToString(x.UserId).Contains(keywords)
                || (x.UserName != null && x.UserName.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.LikeTime).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.LikeTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.LikeTime >= queryDto.LikeTimeStart);
        }

        if (queryDto?.LikeTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.LikeTime <= queryDto.LikeTimeEnd);
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
