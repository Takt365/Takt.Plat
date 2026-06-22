// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.DocumentCenter
// 文件名称：TaktDocumentVersionService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：文管文档版本应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.DocumentCenter;
using Takt.Domain.Entities.Routine.DocumentCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.DocumentCenter;

/// <summary>
/// 文管文档版本应用服务
/// </summary>
public class TaktDocumentVersionService : TaktServiceBase, ITaktDocumentVersionService
{
    private readonly ITaktCompanyRepository<TaktDocumentVersion> _documentVersionRepository;
    private readonly ITaktApprovalRepository<TaktDocument> _documentRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="documentVersionRepository">文管文档版本仓储</param>
    /// <param name="documentRepository">文管中心仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDocumentVersionService(
        ITaktCompanyRepository<TaktDocumentVersion> documentVersionRepository,
        ITaktApprovalRepository<TaktDocument> documentRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _documentVersionRepository = documentVersionRepository;
        _documentRepository = documentRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取文管文档版本列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDocumentVersionDto>> GetDocumentVersionListAsync(TaktDocumentVersionQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _documentVersionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDocumentVersionDto>.Create(
            data.Adapt<List<TaktDocumentVersionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentVersionDto?> GetDocumentVersionByIdAsync(long id)
    {
        var entity = await _documentVersionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktDocumentVersionDto>();
    }

    /// <summary>
    /// 获取文管文档版本选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDocumentVersionOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _documentVersionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.FileName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FileName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建文管文档版本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentVersionDto> CreateDocumentVersionAsync(TaktDocumentVersionCreateDto dto)
    {
        var entity = dto.Adapt<TaktDocumentVersion>();
        await StampDocumentVersionDocumentAsync(entity, dto);
        var isUnique_ix_document_version_unique = await _uniqueValidator.IsUniqueAsync(
            _documentVersionRepository,
            x => x.DocumentId == entity.DocumentId
                && x.VersionNo == entity.VersionNo);
        if (!isUnique_ix_document_version_unique)
        {
            throw new TaktBusinessException("文管文档版本的DocumentId、VersionNo已存在");
        }
        entity = await _documentVersionRepository.CreateAsync(entity);
        return await GetDocumentVersionByIdAsync(entity.Id) ?? entity.Adapt<TaktDocumentVersionDto>();
    }

    /// <summary>
    /// 更新文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentVersionDto> UpdateDocumentVersionAsync(long id, TaktDocumentVersionUpdateDto dto)
    {
        var entity = await _documentVersionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文管文档版本不存在");
        }
        dto.Adapt(entity);
        await StampDocumentVersionDocumentAsync(entity, dto);
        var isUnique_ix_document_version_unique = await _uniqueValidator.IsUniqueAsync(
            _documentVersionRepository,
            x => x.DocumentId == entity.DocumentId
                && x.VersionNo == entity.VersionNo,
            id);
        if (!isUnique_ix_document_version_unique)
        {
            throw new TaktBusinessException("文管文档版本的DocumentId、VersionNo已存在");
        }
        await _documentVersionRepository.UpdateAsync(entity);
        return await GetDocumentVersionByIdAsync(id) ?? throw new TaktBusinessException("文管文档版本不存在");
    }

    /// <summary>
    /// 删除文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDocumentVersionByIdAsync(long id)
    {
        var deleted = await _documentVersionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("文管文档版本不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除文管文档版本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDocumentVersionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteDocumentVersionByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetDocumentVersionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktDocumentVersionTemplateDto>(
            sheetName ?? "文管文档版本导入模板",
            fileName ?? "文管文档版本导入模板.xlsx");
    }

    /// <summary>
    /// 导入文管文档版本
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportDocumentVersionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktDocumentVersionImportDto>(fileStream, sheetName ?? "文管文档版本导入模板");
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
                var entity = rows[i].Adapt<TaktDocumentVersion>();
                var importDto = rows[i].Adapt<TaktDocumentVersionCreateDto>();
                await StampDocumentVersionDocumentAsync(entity, importDto);
                var importKey = $"{entity.DocumentId}|{entity.VersionNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DocumentId、VersionNo）");
                }
                var isUnique_ix_document_version_unique = await _uniqueValidator.IsUniqueAsync(
                    _documentVersionRepository,
                    x => x.DocumentId == entity.DocumentId
                        && x.VersionNo == entity.VersionNo);
                if (!isUnique_ix_document_version_unique)
                {
                    throw new TaktBusinessException("文管文档版本的DocumentId、VersionNo已存在");
                }
                await _documentVersionRepository.CreateAsync(entity);
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
    /// 导出文管文档版本
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDocumentVersionAsync(TaktDocumentVersionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktDocumentVersionQueryDto());
        var list = await _documentVersionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDocumentVersionExportDto>(),
                sheetName ?? "文管文档版本数据",
                fileName ?? "文管文档版本导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDocumentVersionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "文管文档版本数据",
            fileName ?? "文管文档版本导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步文管文档版本主表外键（ManyToOne → 文管中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampDocumentVersionDocumentAsync(TaktDocumentVersion entity, TaktDocumentVersionCreateDto dto)
    {
        if (dto.DocumentId <= 0)
        {
            return;
        }
        var master = await _documentRepository.GetByIdAsync(dto.DocumentId);
        if (master == null)
        {
            throw new TaktBusinessException("文管中心不存在");
        }
        entity.DocumentId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建文管文档版本查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDocumentVersion, bool>> QueryExpression(TaktDocumentVersionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDocumentVersion>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.DocumentId).Contains(keywords)
                || SqlFunc.ToString(x.VersionNo).Contains(keywords)
                || (x.VersionNote != null && x.VersionNote.Contains(keywords))
                || SqlFunc.ToString(x.FileId).Contains(keywords)
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.FilePath != null && x.FilePath.Contains(keywords))
                || SqlFunc.ToString(x.FileSize).Contains(keywords)
                || (x.FileType != null && x.FileType.Contains(keywords))
                || (x.FileExtension != null && x.FileExtension.Contains(keywords))
                || SqlFunc.ToString(x.RevisedBy).Contains(keywords)
                || (x.RevisedByName != null && x.RevisedByName.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.RevisedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.DocumentId.HasValue == true)
        {
            exp = exp.And(x => x.DocumentId == queryDto.DocumentId);
        }

        if (queryDto?.VersionNo.HasValue == true)
        {
            exp = exp.And(x => x.VersionNo == queryDto.VersionNo);
        }

        if (!string.IsNullOrEmpty(queryDto?.VersionNote))
        {
            exp = exp.And(x => x.VersionNote != null && x.VersionNote.Contains(queryDto.VersionNote));
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

        if (queryDto?.RevisedBy.HasValue == true)
        {
            exp = exp.And(x => x.RevisedBy == queryDto.RevisedBy);
        }

        if (!string.IsNullOrEmpty(queryDto?.RevisedByName))
        {
            exp = exp.And(x => x.RevisedByName != null && x.RevisedByName.Contains(queryDto.RevisedByName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.RevisedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.RevisedAt >= queryDto.RevisedAtStart);
        }

        if (queryDto?.RevisedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.RevisedAt <= queryDto.RevisedAtEnd);
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
