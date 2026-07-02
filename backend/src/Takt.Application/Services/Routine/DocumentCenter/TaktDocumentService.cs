// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Routine.DocumentCenter
// 文件名称：TaktDocumentService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：文管中心应用服务实现
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
/// 文管中心应用服务
/// </summary>
public class TaktDocumentService : TaktServiceBase, ITaktDocumentService
{
    private readonly ITaktApprovalRepository<TaktDocument> _documentRepository;
    private readonly ITaktCompanyRepository<TaktDocumentVersion> _documentVersionRepository;
    private readonly ITaktCompanyRepository<TaktDocumentChangeLog> _documentChangeLogRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="documentRepository">文管中心仓储</param>
    /// <param name="documentVersionRepository">DocumentVersion仓储</param>
    /// <param name="documentChangeLogRepository">DocumentChangeLog仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDocumentService(
        ITaktApprovalRepository<TaktDocument> documentRepository,
        ITaktCompanyRepository<TaktDocumentVersion> documentVersionRepository,
        ITaktCompanyRepository<TaktDocumentChangeLog> documentChangeLogRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _documentRepository = documentRepository;
        _documentVersionRepository = documentVersionRepository;
        _documentChangeLogRepository = documentChangeLogRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取文管中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDocumentDto>> GetDocumentListAsync(TaktDocumentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _documentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDocumentDto>.Create(
            data.Adapt<List<TaktDocumentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取文管中心
    /// </summary>
    /// <param name="id">文管中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentDto?> GetDocumentByIdAsync(long id)
    {
        var entity = await _documentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktDocumentDto>();
        await FillDocumentDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取文管中心选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDocumentOptionsAsync()
    {
        EnsureThreeLayerContext();
        var list = await _documentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DocumentStatus == 1,
            x => x.FileName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FileName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建文管中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentDto> CreateDocumentAsync(TaktDocumentCreateDto dto)
    {
        var entity = dto.Adapt<TaktDocument>();
        var isUnique_ix_document_code_unique = await _uniqueValidator.IsUniqueAsync(
            _documentRepository,
            x => x.DocumentCode == entity.DocumentCode);
        if (!isUnique_ix_document_code_unique)
        {
            throw new TaktBusinessException("文管中心的DocumentCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _documentRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FileId == entity.FileId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.FileId.GetValueOrDefault(), maxSort);
        }
        entity = await _documentRepository.CreateAsync(entity);
                await SaveDocumentChildrenAsync(entity, dto);
        return await GetDocumentByIdAsync(entity.Id) ?? entity.Adapt<TaktDocumentDto>();
    }

    /// <summary>
    /// 更新文管中心
    /// </summary>
    /// <param name="id">文管中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentDto> UpdateDocumentAsync(long id, TaktDocumentUpdateDto dto)
    {
        var entity = await _documentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文管中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_document_code_unique = await _uniqueValidator.IsUniqueAsync(
            _documentRepository,
            x => x.DocumentCode == entity.DocumentCode,
            id);
        if (!isUnique_ix_document_code_unique)
        {
            throw new TaktBusinessException("文管中心的DocumentCode已存在");
        }
        await _documentRepository.UpdateAsync(entity);
                await SaveDocumentChildrenAsync(entity, dto);
        return await GetDocumentByIdAsync(id) ?? throw new TaktBusinessException("文管中心不存在");
    }

    /// <summary>
    /// 删除文管中心
    /// </summary>
    /// <param name="id">文管中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDocumentByIdAsync(long id)
    {
        var entity = await _documentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文管中心不存在或已删除");
        }
        await _documentVersionRepository.DeleteAsync(x => x.DocumentId == entity.Id);
        await _documentChangeLogRepository.DeleteAsync(x => x.DocumentId == entity.Id);
        var deleted = await _documentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("文管中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除文管中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDocumentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteDocumentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新文管中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentDto> UpdateDocumentStatusAsync(TaktDocumentStatusDto dto)
    {
        var entity = await _documentRepository.GetByIdAsync(dto.DocumentId);
        if (entity == null)
        {
            throw new TaktBusinessException("文管中心不存在");
        }
        entity.DocumentStatus = dto.DocumentStatus;
        await _documentRepository.UpdateAsync(entity);
        return await GetDocumentByIdAsync(dto.DocumentId) ?? throw new TaktBusinessException("文管中心不存在");
    }

    /// <summary>
    /// 更新文管中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentDto> UpdateDocumentSortAsync(TaktDocumentSortDto dto)
    {
        var entity = await _documentRepository.GetByIdAsync(dto.DocumentId);
        if (entity == null)
        {
            throw new TaktBusinessException("文管中心不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _documentRepository.UpdateAsync(entity);
        return await GetDocumentByIdAsync(dto.DocumentId) ?? throw new TaktBusinessException("文管中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetDocumentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktDocumentTemplateDto>(
            sheetName ?? "文管中心导入模板",
            fileName ?? "文管中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入文管中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportDocumentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktDocumentImportDto>(fileStream, sheetName ?? "文管中心导入模板");
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
                var entity = rows[i].Adapt<TaktDocument>();
                var importKey = $"{entity.DocumentCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DocumentCode）");
                }
                var isUnique_ix_document_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _documentRepository,
                    x => x.DocumentCode == entity.DocumentCode);
                if (!isUnique_ix_document_code_unique)
                {
                    throw new TaktBusinessException("文管中心的DocumentCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _documentRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FileId == entity.FileId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.FileId.GetValueOrDefault(), maxSort);
                }
                await _documentRepository.CreateAsync(entity);
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
    /// 导出文管中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDocumentAsync(TaktDocumentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktDocumentQueryDto());
        var list = await _documentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDocumentExportDto>(),
                sheetName ?? "文管中心数据",
                fileName ?? "文管中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDocumentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "文管中心数据",
            fileName ?? "文管中心导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充文管中心详情（加载 OneToMany 子表：文管文档版本、文管文档变更日志）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillDocumentDetailsAsync(TaktDocumentDto dto, TaktDocument entity)
    {
        if (dto == null)
        {
            return;
        }
        // 文管文档版本 → dto.Versions
        var versions = await _documentVersionRepository.GetListAsync(x => x.DocumentId == entity.Id);
        dto.Versions = versions.Adapt<List<TaktDocumentVersionDto>>();
        // 文管文档变更日志 → dto.ChangeLogs
        var changelogs = await _documentChangeLogRepository.GetListAsync(x => x.DocumentId == entity.Id);
        dto.ChangeLogs = changelogs.Adapt<List<TaktDocumentChangeLogDto>>();
    }

    /// <summary>
    /// 保存文管中心子表级联（文管文档版本、文管文档变更日志；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveDocumentChildrenAsync(TaktDocument entity, TaktDocumentCreateDto dto)
    {
        // 文管文档版本（Versions）
        if (dto.Versions is not { Count: > 0 })
        {
            await _documentVersionRepository.DeleteAsync(x => x.DocumentId == entity.Id);
        }
        else
        {
            var versions = dto.Versions.Adapt<List<TaktDocumentVersion>>();
            foreach (var child in versions)
            {
                child.DocumentId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < versions.Count; i++)
                        {
                            var key = $"{versions[i].CompanyCode}|{versions[i].DocumentId}|{versions[i].VersionNo}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"文管文档版本第{i + 1}项与本次提交的其他项重复（CompanyCode、DocumentId、VersionNo）");
                            }
                        }
            await _documentVersionRepository.DeleteAsync(x => x.DocumentId == entity.Id);
            foreach (var child in versions)
            {
            var isUnique_ix_document_version_unique = await _uniqueValidator.IsUniqueAsync(
                _documentVersionRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.DocumentId == child.DocumentId
                    && x.VersionNo == child.VersionNo);
            if (!isUnique_ix_document_version_unique)
            {
                throw new TaktBusinessException("文管文档版本的CompanyCode、DocumentId、VersionNo已存在");
            }
            }
            await _documentVersionRepository.CreateRangeAsync(versions);
        }
        // 文管文档变更日志（ChangeLogs）
        if (dto.ChangeLogs is not { Count: > 0 })
        {
            await _documentChangeLogRepository.DeleteAsync(x => x.DocumentId == entity.Id);
        }
        else
        {
            var changelogs = dto.ChangeLogs.Adapt<List<TaktDocumentChangeLog>>();
            foreach (var child in changelogs)
            {
                child.DocumentId = entity.Id;
            }
            await _documentChangeLogRepository.DeleteAsync(x => x.DocumentId == entity.Id);
            foreach (var child in changelogs)
            {
            }
            await _documentChangeLogRepository.CreateRangeAsync(changelogs);
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建文管中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDocument, bool>> QueryExpression(TaktDocumentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDocument>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.DocumentCode != null && x.DocumentCode.Contains(keywords))
                || (x.DocumentTitle != null && x.DocumentTitle.Contains(keywords))
                || SqlFunc.ToString(x.DocumentCategory).Contains(keywords)
                || SqlFunc.ToString(x.DocumentStatus).Contains(keywords)
                || SqlFunc.ToString(x.ConfidentialLevel).Contains(keywords)
                || SqlFunc.ToString(x.Version).Contains(keywords)
                || (x.DocumentContent != null && x.DocumentContent.Contains(keywords))
                || (x.DocumentSummary != null && x.DocumentSummary.Contains(keywords))
                || (x.DocumentTags != null && x.DocumentTags.Contains(keywords))
                || SqlFunc.ToString(x.FileId).Contains(keywords)
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.FilePath != null && x.FilePath.Contains(keywords))
                || SqlFunc.ToString(x.FileSize).Contains(keywords)
                || (x.FileType != null && x.FileType.Contains(keywords))
                || (x.FileExtension != null && x.FileExtension.Contains(keywords))
                || SqlFunc.ToString(x.PublisherId).Contains(keywords)
                || (x.PublisherName != null && x.PublisherName.Contains(keywords))
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || SqlFunc.ToString(x.DocumentIsTop).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.DocumentViewCount).Contains(keywords)
                || SqlFunc.ToString(x.DownloadCount).Contains(keywords)
                || (x.TargetScope != null && x.TargetScope.Contains(keywords))
                || (x.TargetDepartments != null && x.TargetDepartments.Contains(keywords))
                || (x.TargetUsers != null && x.TargetUsers.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.DocumentEffectiveTime).Contains(keywords)
                || SqlFunc.ToString(x.DocumentExpireTime).Contains(keywords)
                || SqlFunc.ToString(x.DocumentPublishTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentCode))
        {
            exp = exp.And(x => x.DocumentCode != null && x.DocumentCode.Contains(queryDto.DocumentCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentTitle))
        {
            exp = exp.And(x => x.DocumentTitle != null && x.DocumentTitle.Contains(queryDto.DocumentTitle));
        }

        if (queryDto?.DocumentCategory.HasValue == true)
        {
            exp = exp.And(x => x.DocumentCategory == queryDto.DocumentCategory);
        }

        if (queryDto?.DocumentStatus.HasValue == true)
        {
            exp = exp.And(x => x.DocumentStatus == queryDto.DocumentStatus);
        }

        if (queryDto?.ConfidentialLevel.HasValue == true)
        {
            exp = exp.And(x => x.ConfidentialLevel == queryDto.ConfidentialLevel);
        }

        if (queryDto?.Version.HasValue == true)
        {
            exp = exp.And(x => x.Version == queryDto.Version);
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentContent))
        {
            exp = exp.And(x => x.DocumentContent != null && x.DocumentContent.Contains(queryDto.DocumentContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentSummary))
        {
            exp = exp.And(x => x.DocumentSummary != null && x.DocumentSummary.Contains(queryDto.DocumentSummary));
        }

        if (!string.IsNullOrEmpty(queryDto?.DocumentTags))
        {
            exp = exp.And(x => x.DocumentTags != null && x.DocumentTags.Contains(queryDto.DocumentTags));
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

        if (queryDto?.PublisherId.HasValue == true)
        {
            exp = exp.And(x => x.PublisherId == queryDto.PublisherId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PublisherName))
        {
            exp = exp.And(x => x.PublisherName != null && x.PublisherName.Contains(queryDto.PublisherName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (queryDto?.DocumentIsTop.HasValue == true)
        {
            exp = exp.And(x => x.DocumentIsTop == queryDto.DocumentIsTop);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.DocumentViewCount.HasValue == true)
        {
            exp = exp.And(x => x.DocumentViewCount == queryDto.DocumentViewCount);
        }

        if (queryDto?.DownloadCount.HasValue == true)
        {
            exp = exp.And(x => x.DownloadCount == queryDto.DownloadCount);
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetScope))
        {
            exp = exp.And(x => x.TargetScope != null && x.TargetScope.Contains(queryDto.TargetScope));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetDepartments))
        {
            exp = exp.And(x => x.TargetDepartments != null && x.TargetDepartments.Contains(queryDto.TargetDepartments));
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetUsers))
        {
            exp = exp.And(x => x.TargetUsers != null && x.TargetUsers.Contains(queryDto.TargetUsers));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.EffectiveTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.DocumentEffectiveTime >= queryDto.EffectiveTimeStart);
        }

        if (queryDto?.EffectiveTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.DocumentEffectiveTime <= queryDto.EffectiveTimeEnd);
        }

        if (queryDto?.ExpireTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.DocumentExpireTime >= queryDto.ExpireTimeStart);
        }

        if (queryDto?.ExpireTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.DocumentExpireTime <= queryDto.ExpireTimeEnd);
        }

        if (queryDto?.PublishTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.DocumentPublishTime >= queryDto.PublishTimeStart);
        }

        if (queryDto?.PublishTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.DocumentPublishTime <= queryDto.PublishTimeEnd);
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
