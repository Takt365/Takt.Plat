// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：TaktNewsAttachmentService.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心附件应用服务实现
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
using Takt.Domain.Entities.Routine.NewsCenter;

namespace Takt.Application.Services.Routine.NewsCenter;

/// <summary>
/// 新闻中心附件应用服务
/// </summary>
public class TaktNewsAttachmentService : TaktServiceBase, ITaktNewsAttachmentService
{
    private readonly ITaktCompanyRepository<TaktNewsAttachment> _newsAttachmentRepository;
    private readonly ITaktApprovalRepository<TaktNews> _newsRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsAttachmentRepository">新闻中心附件仓储</param>
    /// <param name="newsRepository">新闻中心仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNewsAttachmentService(
        ITaktCompanyRepository<TaktNewsAttachment> newsAttachmentRepository,
        ITaktApprovalRepository<TaktNews> newsRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _newsAttachmentRepository = newsAttachmentRepository;
        _newsRepository = newsRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取新闻中心附件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNewsAttachmentDto>> GetNewsAttachmentListAsync(TaktNewsAttachmentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _newsAttachmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNewsAttachmentDto>.Create(
            data.Adapt<List<TaktNewsAttachmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取新闻中心附件
    /// </summary>
    /// <param name="id">新闻中心附件ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsAttachmentDto?> GetNewsAttachmentByIdAsync(long id)
    {
        var entity = await _newsAttachmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktNewsAttachmentDto>();
    }

    /// <summary>
    /// 获取新闻附件选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetNewsAttachmentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _newsAttachmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.FileName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FileName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建新闻中心附件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsAttachmentDto> CreateNewsAttachmentAsync(TaktNewsAttachmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktNewsAttachment>();
                await StampNewsAttachmentNewsAsync(entity, dto);
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _newsAttachmentRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.NewsId == entity.NewsId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.NewsId, maxSort);
        }
        entity = await _newsAttachmentRepository.CreateAsync(entity);
        return await GetNewsAttachmentByIdAsync(entity.Id) ?? entity.Adapt<TaktNewsAttachmentDto>();
    }

    /// <summary>
    /// 更新新闻中心附件
    /// </summary>
    /// <param name="id">新闻中心附件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsAttachmentDto> UpdateNewsAttachmentAsync(long id, TaktNewsAttachmentUpdateDto dto)
    {
        var entity = await _newsAttachmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心附件不存在");
        }
        dto.Adapt(entity);
                await StampNewsAttachmentNewsAsync(entity, dto);
        await _newsAttachmentRepository.UpdateAsync(entity);
        return await GetNewsAttachmentByIdAsync(id) ?? throw new TaktBusinessException("新闻中心附件不存在");
    }

    /// <summary>
    /// 删除新闻中心附件
    /// </summary>
    /// <param name="id">新闻中心附件ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsAttachmentByIdAsync(long id)
    {
        var deleted = await _newsAttachmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("新闻中心附件不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除新闻中心附件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsAttachmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteNewsAttachmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新新闻中心附件排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsAttachmentDto> UpdateNewsAttachmentSortAsync(TaktNewsAttachmentSortDto dto)
    {
        var entity = await _newsAttachmentRepository.GetByIdAsync(dto.NewsAttachmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心附件不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _newsAttachmentRepository.UpdateAsync(entity);
        return await GetNewsAttachmentByIdAsync(dto.NewsAttachmentId) ?? throw new TaktBusinessException("新闻中心附件不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNewsAttachmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNewsAttachmentTemplateDto>(
            sheetName ?? "新闻中心附件导入模板",
            fileName ?? "新闻中心附件导入模板.xlsx");
    }

    /// <summary>
    /// 导入新闻中心附件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNewsAttachmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNewsAttachmentImportDto>(fileStream, sheetName ?? "新闻中心附件导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktNewsAttachment>();
                var importDto = rows[i].Adapt<TaktNewsAttachmentCreateDto>();
                await StampNewsAttachmentNewsAsync(entity, importDto);
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _newsAttachmentRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.NewsId == entity.NewsId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.NewsId, maxSort);
                }
                await _newsAttachmentRepository.CreateAsync(entity);
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
    /// 导出新闻中心附件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNewsAttachmentAsync(TaktNewsAttachmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktNewsAttachmentQueryDto());
        var list = await _newsAttachmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsAttachmentExportDto>(),
                sheetName ?? "新闻中心附件数据",
                fileName ?? "新闻中心附件导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNewsAttachmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "新闻中心附件数据",
            fileName ?? "新闻中心附件导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步新闻中心附件主表外键（ManyToOne → 新闻中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampNewsAttachmentNewsAsync(TaktNewsAttachment entity, TaktNewsAttachmentCreateDto dto)
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
    /// 构建新闻中心附件查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNewsAttachment, bool>> QueryExpression(TaktNewsAttachmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNewsAttachment>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.NewsId).Contains(keywords)
                || SqlFunc.ToString(x.FileId).Contains(keywords)
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.FilePath != null && x.FilePath.Contains(keywords))
                || SqlFunc.ToString(x.FileSize).Contains(keywords)
                || (x.FileType != null && x.FileType.Contains(keywords))
                || (x.FileExtension != null && x.FileExtension.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.NewsId.HasValue == true)
        {
            exp = exp.And(x => x.NewsId == queryDto.NewsId);
        }

        if (queryDto?.FileId.HasValue == true)
        {
            exp = exp.And(x => x.FileId == queryDto.FileId);
        }

        if (!string.IsNullOrEmpty(queryDto?.FileName))
        {
            exp = exp.And(x => x.FileName != null && x.FileName.Contains(queryDto.FileName));
        }

        if (!string.IsNullOrEmpty(queryDto?.FilePath))
        {
            exp = exp.And(x => x.FilePath != null && x.FilePath.Contains(queryDto.FilePath));
        }

        if (queryDto?.FileSize.HasValue == true)
        {
            exp = exp.And(x => x.FileSize == queryDto.FileSize);
        }

        if (!string.IsNullOrEmpty(queryDto?.FileType))
        {
            exp = exp.And(x => x.FileType != null && x.FileType.Contains(queryDto.FileType));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileExtension))
        {
            exp = exp.And(x => x.FileExtension != null && x.FileExtension.Contains(queryDto.FileExtension));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
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
