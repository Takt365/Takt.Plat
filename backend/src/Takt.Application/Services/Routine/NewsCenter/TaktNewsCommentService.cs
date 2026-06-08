// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：TaktNewsCommentService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心评论应用服务实现
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
/// 新闻中心评论应用服务
/// </summary>
public class TaktNewsCommentService : TaktServiceBase, ITaktNewsCommentService
{
    private readonly ITaktApprovalRepository<TaktNewsComment> _newsCommentRepository;
    private readonly ITaktCompanyRepository<TaktNewsCommentLike> _newsCommentLikeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsCommentRepository">新闻中心评论仓储</param>
    /// <param name="newsCommentLikeRepository">NewsCommentLike仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNewsCommentService(
        ITaktApprovalRepository<TaktNewsComment> newsCommentRepository,
        ITaktCompanyRepository<TaktNewsCommentLike> newsCommentLikeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _newsCommentRepository = newsCommentRepository;
        _newsCommentLikeRepository = newsCommentLikeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取新闻中心评论列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNewsCommentDto>> GetNewsCommentListAsync(TaktNewsCommentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _newsCommentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNewsCommentDto>.Create(
            data.Adapt<List<TaktNewsCommentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentDto?> GetNewsCommentByIdAsync(long id)
    {
        var entity = await _newsCommentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktNewsCommentDto>();
        await FillNewsCommentDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取新闻评论树形选项列表
    /// </summary>
    /// <returns>树形选项</returns>
    public async Task<List<TaktTreeSelectOption>> GetNewsCommentTreeOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _newsCommentRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
        return BuildNewsCommentTreeOptions(list, 0);
    }

    /// <summary>
    /// 在内存中构建新闻中心评论树形选项（递归，按 ParentId）
    /// </summary>
    private List<TaktTreeSelectOption> BuildNewsCommentTreeOptions(List<TaktNewsComment> all, long parentId)
    {
        var result = new List<TaktTreeSelectOption>();
        foreach (var item in all.Where(x => x.ParentId == parentId).OrderBy(x => x.Id))
        {
            var option = new TaktTreeSelectOption
            {
                DictValue = item.Id,
                DictLabel = item.UserName ?? item.Id.ToString(),
                SortOrder = 0,
            };
            var children = BuildNewsCommentTreeOptions(all, item.Id);
            if (children.Count > 0)
            {
                option.Children = children;
            }
            result.Add(option);
        }
        return result;
    }

    /// <summary>
    /// 获取新闻中心评论树形列表
    /// </summary>
    /// <param name="parentId">父级ID</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表</returns>
    public async Task<List<TaktNewsCommentTreeDto>> GetNewsCommentTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        EnsureThreeLayerContext();
        var list = await _newsCommentRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
        var filtered = includeDisabled
            ? list
            : list.Where(x => x.CommentStatus == TaktNewsCommentStatus.Normal).ToList();
        return BuildNewsCommentTree(filtered, parentId);
    }

    /// <summary>
    /// 在内存中构建新闻中心评论树（递归，按 ParentId）
    /// </summary>
    private List<TaktNewsCommentTreeDto> BuildNewsCommentTree(List<TaktNewsComment> allRecords, long parentId)
    {
        var children = allRecords
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.Id)
            .ToList();
        var treeList = new List<TaktNewsCommentTreeDto>();
        foreach (var item in children)
        {
            var treeDto = item.Adapt<TaktNewsCommentTreeDto>();
            var childTree = BuildNewsCommentTree(allRecords, item.Id);
            if (childTree.Count > 0)
            {
                treeDto.Children = childTree;
            }
            treeList.Add(treeDto);
        }
        return treeList;
    }

    /// <summary>
    /// 创建新闻中心评论
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentDto> CreateNewsCommentAsync(TaktNewsCommentCreateDto dto)
    {
        var entity = dto.Adapt<TaktNewsComment>();
        entity = await _newsCommentRepository.CreateAsync(entity);
                await SaveNewsCommentChildrenAsync(entity, dto);
        return await GetNewsCommentByIdAsync(entity.Id) ?? entity.Adapt<TaktNewsCommentDto>();
    }

    /// <summary>
    /// 更新新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentDto> UpdateNewsCommentAsync(long id, TaktNewsCommentUpdateDto dto)
    {
        var entity = await _newsCommentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心评论不存在");
        }
        dto.Adapt(entity);
        await _newsCommentRepository.UpdateAsync(entity);
                await SaveNewsCommentChildrenAsync(entity, dto);
        return await GetNewsCommentByIdAsync(id) ?? throw new TaktBusinessException("新闻中心评论不存在");
    }

    /// <summary>
    /// 删除新闻中心评论
    /// </summary>
    /// <param name="id">新闻中心评论ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsCommentByIdAsync(long id)
    {
        var entity = await _newsCommentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心评论不存在或已删除");
        }
        await _newsCommentLikeRepository.DeleteAsync(x => x.CommentId == entity.Id);
        var deleted = await _newsCommentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("新闻中心评论不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除新闻中心评论
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsCommentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteNewsCommentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新新闻中心评论状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentDto> UpdateNewsCommentStatusAsync(TaktNewsCommentStatusDto dto)
    {
        var entity = await _newsCommentRepository.GetByIdAsync(dto.NewsCommentId);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心评论不存在");
        }
        entity.CommentStatus = dto.CommentStatus;
        await _newsCommentRepository.UpdateAsync(entity);
        return await GetNewsCommentByIdAsync(dto.NewsCommentId) ?? throw new TaktBusinessException("新闻中心评论不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNewsCommentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNewsCommentTemplateDto>(
            sheetName ?? "新闻中心评论导入模板",
            fileName ?? "新闻中心评论导入模板.xlsx");
    }

    /// <summary>
    /// 导入新闻中心评论
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNewsCommentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNewsCommentImportDto>(fileStream, sheetName ?? "新闻中心评论导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktNewsComment>();
                await _newsCommentRepository.CreateAsync(entity);
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
    /// 导出新闻中心评论
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNewsCommentAsync(TaktNewsCommentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktNewsCommentQueryDto());
        var list = await _newsCommentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsCommentExportDto>(),
                sheetName ?? "新闻中心评论数据",
                fileName ?? "新闻中心评论导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNewsCommentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "新闻中心评论数据",
            fileName ?? "新闻中心评论导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充新闻中心评论详情（加载 OneToMany 子表：新闻中心评论点赞记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillNewsCommentDetailsAsync(TaktNewsCommentDto dto, TaktNewsComment entity)
    {
        if (dto == null)
        {
            return;
        }
        // 新闻中心评论点赞记录 → dto.Likes
        var likes = await _newsCommentLikeRepository.GetListAsync(x => x.CommentId == entity.Id);
        dto.Likes = likes.Adapt<List<TaktNewsCommentLikeDto>>();
    }

    /// <summary>
    /// 保存新闻中心评论子表级联（新闻中心评论点赞记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveNewsCommentChildrenAsync(TaktNewsComment entity, TaktNewsCommentCreateDto dto)
    {
        // 新闻中心评论点赞记录（Likes）
        if (dto.Likes is not { Count: > 0 })
        {
            await _newsCommentLikeRepository.DeleteAsync(x => x.CommentId == entity.Id);
        }
        else
        {
            var likes = dto.Likes.Adapt<List<TaktNewsCommentLike>>();
            foreach (var child in likes)
            {
                child.CommentId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < likes.Count; i++)
                        {
                            var key = $"{likes[i].CompanyCode}|{likes[i].CommentId}|{likes[i].UserId}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"新闻中心评论点赞记录第{i + 1}项与本次提交的其他项重复（CompanyCode、CommentId、UserId）");
                            }
                        }
            await _newsCommentLikeRepository.DeleteAsync(x => x.CommentId == entity.Id);
            foreach (var child in likes)
            {
            var isUnique_ix_news_comment_like_unique = await _uniqueValidator.IsUniqueAsync(
                _newsCommentLikeRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.CommentId == child.CommentId
                    && x.UserId == child.UserId);
            if (!isUnique_ix_news_comment_like_unique)
            {
                throw new TaktBusinessException("新闻中心评论点赞记录的CompanyCode、CommentId、UserId已存在");
            }
            }
            await _newsCommentLikeRepository.CreateRangeAsync(likes);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建新闻中心评论查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNewsComment, bool>> QueryExpression(TaktNewsCommentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNewsComment>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.NewsId).Contains(keywords)
                || SqlFunc.ToString(x.ParentId).Contains(keywords)
                || SqlFunc.ToString(x.UserId).Contains(keywords)
                || (x.UserName != null && x.UserName.Contains(keywords))
                || (x.UserAvatar != null && x.UserAvatar.Contains(keywords))
                || SqlFunc.ToString(x.ReplyToUserId).Contains(keywords)
                || (x.ReplyToUserName != null && x.ReplyToUserName.Contains(keywords))
                || (x.CommentContent != null && x.CommentContent.Contains(keywords))
                || SqlFunc.ToString(x.LikeCount).Contains(keywords)
                || SqlFunc.ToString(x.ReplyCount).Contains(keywords)
                || SqlFunc.ToString(x.CommentLevel).Contains(keywords)
                || SqlFunc.ToString(x.FlowInstanceId).Contains(keywords)
                || SqlFunc.ToString(x.CommentStatus).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CommentTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.NewsId.HasValue == true)
        {
            exp = exp.And(x => x.NewsId == queryDto.NewsId);
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            exp = exp.And(x => x.ParentId == queryDto.ParentId);
        }

        if (queryDto?.UserId.HasValue == true)
        {
            exp = exp.And(x => x.UserId == queryDto.UserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.UserName))
        {
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(queryDto.UserName));
        }

        if (!string.IsNullOrEmpty(queryDto?.UserAvatar))
        {
            exp = exp.And(x => x.UserAvatar != null && x.UserAvatar.Contains(queryDto.UserAvatar));
        }

        if (queryDto?.ReplyToUserId.HasValue == true)
        {
            exp = exp.And(x => x.ReplyToUserId == queryDto.ReplyToUserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ReplyToUserName))
        {
            exp = exp.And(x => x.ReplyToUserName != null && x.ReplyToUserName.Contains(queryDto.ReplyToUserName));
        }

        if (!string.IsNullOrEmpty(queryDto?.CommentContent))
        {
            exp = exp.And(x => x.CommentContent != null && x.CommentContent.Contains(queryDto.CommentContent));
        }

        if (queryDto?.LikeCount.HasValue == true)
        {
            exp = exp.And(x => x.LikeCount == queryDto.LikeCount);
        }

        if (queryDto?.ReplyCount.HasValue == true)
        {
            exp = exp.And(x => x.ReplyCount == queryDto.ReplyCount);
        }

        if (queryDto?.CommentLevel.HasValue == true)
        {
            exp = exp.And(x => x.CommentLevel == queryDto.CommentLevel);
        }

        if (queryDto?.FlowInstanceId.HasValue == true)
        {
            exp = exp.And(x => x.FlowInstanceId == queryDto.FlowInstanceId);
        }

        if (queryDto?.CommentStatus.HasValue == true)
        {
            exp = exp.And(x => x.CommentStatus == queryDto.CommentStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.CommentTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.CommentTime >= queryDto.CommentTimeStart);
        }

        if (queryDto?.CommentTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.CommentTime <= queryDto.CommentTimeEnd);
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
