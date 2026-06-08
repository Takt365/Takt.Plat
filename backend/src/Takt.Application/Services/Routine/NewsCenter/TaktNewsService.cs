// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：TaktNewsService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心应用服务实现
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
using Takt.Shared.Enums;

namespace Takt.Application.Services.Routine.NewsCenter;

/// <summary>
/// 新闻中心应用服务
/// </summary>
public class TaktNewsService : TaktServiceBase, ITaktNewsService
{
    private readonly ITaktApprovalRepository<TaktNews> _newsRepository;
    private readonly ITaktCompanyRepository<TaktNewsAttachment> _newsAttachmentRepository;
    private readonly ITaktApprovalRepository<TaktNewsComment> _newsCommentRepository;
    private readonly ITaktCompanyRepository<TaktNewsLike> _newsLikeRepository;
    private readonly ITaktCompanyRepository<TaktNewsRead> _newsReadRepository;
    private readonly ITaktCompanyRepository<TaktNewsFavorite> _newsFavoriteRepository;
    private readonly ITaktCompanyRepository<TaktNewsShare> _newsShareRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsRepository">新闻中心仓储</param>
    /// <param name="newsAttachmentRepository">NewsAttachment仓储</param>
    /// <param name="newsCommentRepository">NewsComment仓储</param>
    /// <param name="newsLikeRepository">NewsLike仓储</param>
    /// <param name="newsReadRepository">NewsRead仓储</param>
    /// <param name="newsFavoriteRepository">NewsFavorite仓储</param>
    /// <param name="newsShareRepository">NewsShare仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNewsService(
        ITaktApprovalRepository<TaktNews> newsRepository,
        ITaktCompanyRepository<TaktNewsAttachment> newsAttachmentRepository,
        ITaktApprovalRepository<TaktNewsComment> newsCommentRepository,
        ITaktCompanyRepository<TaktNewsLike> newsLikeRepository,
        ITaktCompanyRepository<TaktNewsRead> newsReadRepository,
        ITaktCompanyRepository<TaktNewsFavorite> newsFavoriteRepository,
        ITaktCompanyRepository<TaktNewsShare> newsShareRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _newsRepository = newsRepository;
        _newsAttachmentRepository = newsAttachmentRepository;
        _newsCommentRepository = newsCommentRepository;
        _newsLikeRepository = newsLikeRepository;
        _newsReadRepository = newsReadRepository;
        _newsFavoriteRepository = newsFavoriteRepository;
        _newsShareRepository = newsShareRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取新闻中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNewsDto>> GetNewsListAsync(TaktNewsQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _newsRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNewsDto>.Create(
            data.Adapt<List<TaktNewsDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取新闻中心
    /// </summary>
    /// <param name="id">新闻中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsDto?> GetNewsByIdAsync(long id)
    {
        var entity = await _newsRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktNewsDto>();
        await FillNewsDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取新闻中心主选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetNewsOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _newsRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.DeptName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.DeptName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建新闻中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsDto> CreateNewsAsync(TaktNewsCreateDto dto)
    {
        var entity = dto.Adapt<TaktNews>();
        var isUnique_ix_news_code_unique = await _uniqueValidator.IsUniqueAsync(
            _newsRepository,
            x => x.NewsCode == entity.NewsCode);
        if (!isUnique_ix_news_code_unique)
        {
            throw new TaktBusinessException("新闻中心的NewsCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _newsRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FlowInstanceId == entity.FlowInstanceId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.FlowInstanceId.GetValueOrDefault(), maxSort);
        }
        entity = await _newsRepository.CreateAsync(entity);
                await SaveNewsChildrenAsync(entity, dto);
        return await GetNewsByIdAsync(entity.Id) ?? entity.Adapt<TaktNewsDto>();
    }

    /// <summary>
    /// 更新新闻中心
    /// </summary>
    /// <param name="id">新闻中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsDto> UpdateNewsAsync(long id, TaktNewsUpdateDto dto)
    {
        var entity = await _newsRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_news_code_unique = await _uniqueValidator.IsUniqueAsync(
            _newsRepository,
            x => x.NewsCode == entity.NewsCode,
            id);
        if (!isUnique_ix_news_code_unique)
        {
            throw new TaktBusinessException("新闻中心的NewsCode已存在");
        }
        await _newsRepository.UpdateAsync(entity);
                await SaveNewsChildrenAsync(entity, dto);
        return await GetNewsByIdAsync(id) ?? throw new TaktBusinessException("新闻中心不存在");
    }

    /// <summary>
    /// 删除新闻中心
    /// </summary>
    /// <param name="id">新闻中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsByIdAsync(long id)
    {
        var entity = await _newsRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心不存在或已删除");
        }
        await _newsAttachmentRepository.DeleteAsync(x => x.NewsId == entity.Id);
        await _newsCommentRepository.DeleteAsync(x => x.NewsId == entity.Id);
        await _newsLikeRepository.DeleteAsync(x => x.NewsId == entity.Id);
        await _newsReadRepository.DeleteAsync(x => x.NewsId == entity.Id);
        await _newsFavoriteRepository.DeleteAsync(x => x.NewsId == entity.Id);
        await _newsShareRepository.DeleteAsync(x => x.NewsId == entity.Id);
        var deleted = await _newsRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("新闻中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除新闻中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteNewsByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新新闻中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsDto> UpdateNewsStatusAsync(TaktNewsStatusDto dto)
    {
        var entity = await _newsRepository.GetByIdAsync(dto.NewsId);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心不存在");
        }
        entity.NewsStatus = dto.NewsStatus;
        await _newsRepository.UpdateAsync(entity);
        return await GetNewsByIdAsync(dto.NewsId) ?? throw new TaktBusinessException("新闻中心不存在");
    }

    /// <summary>
    /// 更新新闻中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsDto> UpdateNewsSortAsync(TaktNewsSortDto dto)
    {
        var entity = await _newsRepository.GetByIdAsync(dto.NewsId);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _newsRepository.UpdateAsync(entity);
        return await GetNewsByIdAsync(dto.NewsId) ?? throw new TaktBusinessException("新闻中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNewsTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNewsTemplateDto>(
            sheetName ?? "新闻中心导入模板",
            fileName ?? "新闻中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入新闻中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNewsAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNewsImportDto>(fileStream, sheetName ?? "新闻中心导入模板");
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
                var entity = rows[i].Adapt<TaktNews>();
                var importKey = $"{entity.NewsCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（NewsCode）");
                }
                var isUnique_ix_news_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _newsRepository,
                    x => x.NewsCode == entity.NewsCode);
                if (!isUnique_ix_news_code_unique)
                {
                    throw new TaktBusinessException("新闻中心的NewsCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _newsRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FlowInstanceId == entity.FlowInstanceId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.FlowInstanceId.GetValueOrDefault(), maxSort);
                }
                await _newsRepository.CreateAsync(entity);
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
    /// 导出新闻中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNewsAsync(TaktNewsQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktNewsQueryDto());
        var list = await _newsRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsExportDto>(),
                sheetName ?? "新闻中心数据",
                fileName ?? "新闻中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNewsExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "新闻中心数据",
            fileName ?? "新闻中心导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充新闻中心详情（加载 OneToMany 子表：新闻中心附件、新闻中心评论、新闻中心点赞记录、新闻中心阅读记录、新闻中心收藏记录、新闻中心分享记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillNewsDetailsAsync(TaktNewsDto dto, TaktNews entity)
    {
        if (dto == null)
        {
            return;
        }
        // 新闻中心附件 → dto.Attachments
        var attachments = await _newsAttachmentRepository.GetListAsync(x => x.NewsId == entity.Id);
        dto.Attachments = attachments.Adapt<List<TaktNewsAttachmentDto>>();
        // 新闻中心评论 → dto.Comments
        var comments = await _newsCommentRepository.GetListAsync(x => x.NewsId == entity.Id);
        dto.Comments = comments.Adapt<List<TaktNewsCommentDto>>();
        // 新闻中心点赞记录 → dto.Likes
        var likes = await _newsLikeRepository.GetListAsync(x => x.NewsId == entity.Id);
        dto.Likes = likes.Adapt<List<TaktNewsLikeDto>>();
        // 新闻中心阅读记录 → dto.Reads
        var reads = await _newsReadRepository.GetListAsync(x => x.NewsId == entity.Id);
        dto.Reads = reads.Adapt<List<TaktNewsReadDto>>();
        // 新闻中心收藏记录 → dto.Favorites
        var favorites = await _newsFavoriteRepository.GetListAsync(x => x.NewsId == entity.Id);
        dto.Favorites = favorites.Adapt<List<TaktNewsFavoriteDto>>();
        // 新闻中心分享记录 → dto.Shares
        var shares = await _newsShareRepository.GetListAsync(x => x.NewsId == entity.Id);
        dto.Shares = shares.Adapt<List<TaktNewsShareDto>>();
    }

    /// <summary>
    /// 保存新闻中心子表级联（新闻中心附件、新闻中心评论、新闻中心点赞记录、新闻中心阅读记录、新闻中心收藏记录、新闻中心分享记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveNewsChildrenAsync(TaktNews entity, TaktNewsCreateDto dto)
    {
        // 新闻中心附件（Attachments）
        if (dto.Attachments is not { Count: > 0 })
        {
            await _newsAttachmentRepository.DeleteAsync(x => x.NewsId == entity.Id);
        }
        else
        {
            var attachments = dto.Attachments.Adapt<List<TaktNewsAttachment>>();
            foreach (var child in attachments)
            {
                child.NewsId = entity.Id;
            }
            var attachmentsNeedSort = attachments.Where(c => c.SortOrder <= 0).ToList();
            if (attachmentsNeedSort.Count > 0)
            {
                var maxSort = await _newsAttachmentRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.NewsId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, attachmentsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in attachments)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _newsAttachmentRepository.DeleteAsync(x => x.NewsId == entity.Id);
            foreach (var child in attachments)
            {
            }
            await _newsAttachmentRepository.CreateRangeAsync(attachments);
        }
        // 新闻中心评论（Comments）
        if (dto.Comments is not { Count: > 0 })
        {
            await _newsCommentRepository.DeleteAsync(x => x.NewsId == entity.Id);
        }
        else
        {
            var comments = dto.Comments.Adapt<List<TaktNewsComment>>();
            foreach (var child in comments)
            {
                child.NewsId = entity.Id;
            }
            await _newsCommentRepository.DeleteAsync(x => x.NewsId == entity.Id);
            foreach (var child in comments)
            {
            }
            await _newsCommentRepository.CreateRangeAsync(comments);
        }
        // 新闻中心点赞记录（Likes）
        if (dto.Likes is not { Count: > 0 })
        {
            await _newsLikeRepository.DeleteAsync(x => x.NewsId == entity.Id);
        }
        else
        {
            var likes = dto.Likes.Adapt<List<TaktNewsLike>>();
            foreach (var child in likes)
            {
                child.NewsId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < likes.Count; i++)
                        {
                            var key = $"{likes[i].CompanyCode}|{likes[i].NewsId}|{likes[i].UserId}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"新闻中心点赞记录第{i + 1}项与本次提交的其他项重复（CompanyCode、NewsId、UserId）");
                            }
                        }
            await _newsLikeRepository.DeleteAsync(x => x.NewsId == entity.Id);
            foreach (var child in likes)
            {
            var isUnique_ix_news_like_unique = await _uniqueValidator.IsUniqueAsync(
                _newsLikeRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.NewsId == child.NewsId
                    && x.UserId == child.UserId);
            if (!isUnique_ix_news_like_unique)
            {
                throw new TaktBusinessException("新闻中心点赞记录的CompanyCode、NewsId、UserId已存在");
            }
            }
            await _newsLikeRepository.CreateRangeAsync(likes);
        }
        // 新闻中心阅读记录（Reads）
        if (dto.Reads is not { Count: > 0 })
        {
            await _newsReadRepository.DeleteAsync(x => x.NewsId == entity.Id);
        }
        else
        {
            var reads = dto.Reads.Adapt<List<TaktNewsRead>>();
            foreach (var child in reads)
            {
                child.NewsId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < reads.Count; i++)
                        {
                            var key = $"{reads[i].CompanyCode}|{reads[i].NewsId}|{reads[i].UserId}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"新闻中心阅读记录第{i + 1}项与本次提交的其他项重复（CompanyCode、NewsId、UserId）");
                            }
                        }
            await _newsReadRepository.DeleteAsync(x => x.NewsId == entity.Id);
            foreach (var child in reads)
            {
            var isUnique_ix_news_read_unique = await _uniqueValidator.IsUniqueAsync(
                _newsReadRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.NewsId == child.NewsId
                    && x.UserId == child.UserId);
            if (!isUnique_ix_news_read_unique)
            {
                throw new TaktBusinessException("新闻中心阅读记录的CompanyCode、NewsId、UserId已存在");
            }
            }
            await _newsReadRepository.CreateRangeAsync(reads);
        }
        // 新闻中心收藏记录（Favorites）
        if (dto.Favorites is not { Count: > 0 })
        {
            await _newsFavoriteRepository.DeleteAsync(x => x.NewsId == entity.Id);
        }
        else
        {
            var favorites = dto.Favorites.Adapt<List<TaktNewsFavorite>>();
            foreach (var child in favorites)
            {
                child.NewsId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < favorites.Count; i++)
                        {
                            var key = $"{favorites[i].CompanyCode}|{favorites[i].NewsId}|{favorites[i].UserId}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"新闻中心收藏记录第{i + 1}项与本次提交的其他项重复（CompanyCode、NewsId、UserId）");
                            }
                        }
            await _newsFavoriteRepository.DeleteAsync(x => x.NewsId == entity.Id);
            foreach (var child in favorites)
            {
            var isUnique_ix_news_favorite_unique = await _uniqueValidator.IsUniqueAsync(
                _newsFavoriteRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.NewsId == child.NewsId
                    && x.UserId == child.UserId);
            if (!isUnique_ix_news_favorite_unique)
            {
                throw new TaktBusinessException("新闻中心收藏记录的CompanyCode、NewsId、UserId已存在");
            }
            }
            await _newsFavoriteRepository.CreateRangeAsync(favorites);
        }
        // 新闻中心分享记录（Shares）
        if (dto.Shares is not { Count: > 0 })
        {
            await _newsShareRepository.DeleteAsync(x => x.NewsId == entity.Id);
        }
        else
        {
            var shares = dto.Shares.Adapt<List<TaktNewsShare>>();
            foreach (var child in shares)
            {
                child.NewsId = entity.Id;
            }
            await _newsShareRepository.DeleteAsync(x => x.NewsId == entity.Id);
            foreach (var child in shares)
            {
            }
            await _newsShareRepository.CreateRangeAsync(shares);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建新闻中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNews, bool>> QueryExpression(TaktNewsQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNews>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.NewsCode != null && x.NewsCode.Contains(keywords))
                || SqlFunc.ToString(x.NewsCategory).Contains(keywords)
                || (x.NewsTitle != null && x.NewsTitle.Contains(keywords))
                || (x.NewsSummary != null && x.NewsSummary.Contains(keywords))
                || (x.Tags != null && x.Tags.Contains(keywords))
                || (x.NewsContent != null && x.NewsContent.Contains(keywords))
                || (x.NewsCoverImage != null && x.NewsCoverImage.Contains(keywords))
                || SqlFunc.ToString(x.IsTop).Contains(keywords)
                || SqlFunc.ToString(x.IsRecommended).Contains(keywords)
                || SqlFunc.ToString(x.ReadCount).Contains(keywords)
                || SqlFunc.ToString(x.LikeCount).Contains(keywords)
                || SqlFunc.ToString(x.CommentCount).Contains(keywords)
                || SqlFunc.ToString(x.FavoriteCount).Contains(keywords)
                || SqlFunc.ToString(x.ShareCount).Contains(keywords)
                || SqlFunc.ToString(x.AttachmentCount).Contains(keywords)
                || SqlFunc.ToString(x.FlowInstanceId).Contains(keywords)
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || SqlFunc.ToString(x.PublisherId).Contains(keywords)
                || (x.PublisherName != null && x.PublisherName.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.NewsStatus).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EffectiveTime).Contains(keywords)
                || SqlFunc.ToString(x.ExpireTime).Contains(keywords)
                || SqlFunc.ToString(x.PublishTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.NewsCode))
        {
            exp = exp.And(x => x.NewsCode != null && x.NewsCode.Contains(queryDto.NewsCode));
        }

        if (queryDto?.NewsCategory.HasValue == true)
        {
            exp = exp.And(x => x.NewsCategory == queryDto.NewsCategory);
        }

        if (!string.IsNullOrEmpty(queryDto?.NewsTitle))
        {
            exp = exp.And(x => x.NewsTitle != null && x.NewsTitle.Contains(queryDto.NewsTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.NewsSummary))
        {
            exp = exp.And(x => x.NewsSummary != null && x.NewsSummary.Contains(queryDto.NewsSummary));
        }

        if (!string.IsNullOrEmpty(queryDto?.Tags))
        {
            exp = exp.And(x => x.Tags != null && x.Tags.Contains(queryDto.Tags));
        }

        if (!string.IsNullOrEmpty(queryDto?.NewsContent))
        {
            exp = exp.And(x => x.NewsContent != null && x.NewsContent.Contains(queryDto.NewsContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.NewsCoverImage))
        {
            exp = exp.And(x => x.NewsCoverImage != null && x.NewsCoverImage.Contains(queryDto.NewsCoverImage));
        }

        if (queryDto?.IsTop.HasValue == true)
        {
            exp = exp.And(x => x.IsTop == queryDto.IsTop);
        }

        if (queryDto?.IsRecommended.HasValue == true)
        {
            exp = exp.And(x => x.IsRecommended == queryDto.IsRecommended);
        }

        if (queryDto?.ReadCount.HasValue == true)
        {
            exp = exp.And(x => x.ReadCount == queryDto.ReadCount);
        }

        if (queryDto?.LikeCount.HasValue == true)
        {
            exp = exp.And(x => x.LikeCount == queryDto.LikeCount);
        }

        if (queryDto?.CommentCount.HasValue == true)
        {
            exp = exp.And(x => x.CommentCount == queryDto.CommentCount);
        }

        if (queryDto?.FavoriteCount.HasValue == true)
        {
            exp = exp.And(x => x.FavoriteCount == queryDto.FavoriteCount);
        }

        if (queryDto?.ShareCount.HasValue == true)
        {
            exp = exp.And(x => x.ShareCount == queryDto.ShareCount);
        }

        if (queryDto?.AttachmentCount.HasValue == true)
        {
            exp = exp.And(x => x.AttachmentCount == queryDto.AttachmentCount);
        }

        if (queryDto?.FlowInstanceId.HasValue == true)
        {
            exp = exp.And(x => x.FlowInstanceId == queryDto.FlowInstanceId);
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (queryDto?.PublisherId.HasValue == true)
        {
            exp = exp.And(x => x.PublisherId == queryDto.PublisherId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PublisherName))
        {
            exp = exp.And(x => x.PublisherName != null && x.PublisherName.Contains(queryDto.PublisherName));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.NewsStatus.HasValue == true)
        {
            exp = exp.And(x => x.NewsStatus == queryDto.NewsStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EffectiveTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveTime >= queryDto.EffectiveTimeStart);
        }

        if (queryDto?.EffectiveTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveTime <= queryDto.EffectiveTimeEnd);
        }

        if (queryDto?.ExpireTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpireTime >= queryDto.ExpireTimeStart);
        }

        if (queryDto?.ExpireTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpireTime <= queryDto.ExpireTimeEnd);
        }

        if (queryDto?.PublishTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.PublishTime >= queryDto.PublishTimeStart);
        }

        if (queryDto?.PublishTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.PublishTime <= queryDto.PublishTimeEnd);
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
