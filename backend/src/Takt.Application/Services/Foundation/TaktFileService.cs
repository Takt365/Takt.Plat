// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktFileService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：文件应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 文件应用服务
/// </summary>
public class TaktFileService : TaktServiceBase, ITaktFileService
{
    private readonly ITaktCompanyRepository<TaktFile> _fileRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fileRepository">文件仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFileService(
        ITaktCompanyRepository<TaktFile> fileRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _fileRepository = fileRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取文件列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFileDto>> GetFileListAsync(TaktFileQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _fileRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFileDto>.Create(
            data.Adapt<List<TaktFileDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFileDto?> GetFileByIdAsync(long id)
    {
        var entity = await _fileRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFileDto>();
    }

    /// <summary>
    /// 获取文件选项列表
    /// </summary>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFileOptionsAsync()
    {
        EnsureThreeLayerContext();
        var currentUserId = CurrentUserId ?? 0;
        var list = await _fileRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && (x.IsPublic == TaktFilePublicAccess.Public || x.CreatedBy == currentUserId),
            x => x.FileName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FileName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFileDto> CreateFileAsync(TaktFileCreateDto dto)
    {
        var entity = dto.Adapt<TaktFile>();
        var isUnique_ix_file_code_unique = await _uniqueValidator.IsUniqueAsync(
            _fileRepository,
            x => x.FileCode == entity.FileCode);
        if (!isUnique_ix_file_code_unique)
        {
            throw new TaktBusinessException("文件的FileCode已存在");
        }
        entity = await _fileRepository.CreateAsync(entity);
        return await GetFileByIdAsync(entity.Id) ?? entity.Adapt<TaktFileDto>();
    }

    /// <summary>
    /// 更新文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFileDto> UpdateFileAsync(long id, TaktFileUpdateDto dto)
    {
        var entity = await _fileRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文件不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_file_code_unique = await _uniqueValidator.IsUniqueAsync(
            _fileRepository,
            x => x.FileCode == entity.FileCode,
            id);
        if (!isUnique_ix_file_code_unique)
        {
            throw new TaktBusinessException("文件的FileCode已存在");
        }
        await _fileRepository.UpdateAsync(entity);
        return await GetFileByIdAsync(id) ?? throw new TaktBusinessException("文件不存在");
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="id">文件ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFileByIdAsync(long id)
    {
        var deleted = await _fileRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("文件不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除文件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFileBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFileByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新文件状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFileDto> UpdateFileStatusAsync(TaktFileStatusDto dto)
    {
        var entity = await _fileRepository.GetByIdAsync(dto.FileId);
        if (entity == null)
        {
            throw new TaktBusinessException("文件不存在");
        }
        entity.FileStatus = dto.FileStatus;
        await _fileRepository.UpdateAsync(entity);
        return await GetFileByIdAsync(dto.FileId) ?? throw new TaktBusinessException("文件不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFileTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFileTemplateDto>(
            sheetName ?? "文件导入模板",
            fileName ?? "文件导入模板.xlsx");
    }

    /// <summary>
    /// 导入文件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFileAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFileImportDto>(fileStream, sheetName ?? "文件导入模板");
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
                var entity = rows[i].Adapt<TaktFile>();
                var importKey = $"{entity.FileCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（FileCode）");
                }
                var isUnique_ix_file_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _fileRepository,
                    x => x.FileCode == entity.FileCode);
                if (!isUnique_ix_file_code_unique)
                {
                    throw new TaktBusinessException("文件的FileCode已存在");
                }
                await _fileRepository.CreateAsync(entity);
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
    /// 导出文件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFileAsync(TaktFileQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFileQueryDto());
        var list = await _fileRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFileExportDto>(),
                sheetName ?? "文件数据",
                fileName ?? "文件导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFileExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "文件数据",
            fileName ?? "文件导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建文件查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFile, bool>> QueryExpression(TaktFileQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFile>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.FileCode != null && x.FileCode.Contains(keywords))
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.FileOriginalName != null && x.FileOriginalName.Contains(keywords))
                || (x.FilePath != null && x.FilePath.Contains(keywords))
                || SqlFunc.ToString(x.FileSize).Contains(keywords)
                || (x.FileType != null && x.FileType.Contains(keywords))
                || (x.FileExtension != null && x.FileExtension.Contains(keywords))
                || (x.FileHash != null && x.FileHash.Contains(keywords))
                || SqlFunc.ToString(x.FileCategory).Contains(keywords)
                || SqlFunc.ToString(x.StorageType).Contains(keywords)
                || (x.StorageConfig != null && x.StorageConfig.Contains(keywords))
                || (x.AccessUrl != null && x.AccessUrl.Contains(keywords))
                || SqlFunc.ToString(x.DownloadCount).Contains(keywords)
                || SqlFunc.ToString(x.FileStatus).Contains(keywords)
                || SqlFunc.ToString(x.IsPublic).Contains(keywords)
                || (x.FileDescription != null && x.FileDescription.Contains(keywords))
                || (x.FileTags != null && x.FileTags.Contains(keywords))
                || (x.IpAddress != null && x.IpAddress.Contains(keywords))
                || (x.Location != null && x.Location.Contains(keywords))
                || (x.ExtFieldJson != null && x.ExtFieldJson.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.LastDownloadTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.FileCode))
        {
            exp = exp.And(x => x.FileCode != null && x.FileCode.Contains(queryDto.FileCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileName))
        {
            exp = exp.And(x => x.FileName != null && x.FileName.Contains(queryDto.FileName));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileOriginalName))
        {
            exp = exp.And(x => x.FileOriginalName != null && x.FileOriginalName.Contains(queryDto.FileOriginalName));
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

        if (!string.IsNullOrEmpty(queryDto?.FileHash))
        {
            exp = exp.And(x => x.FileHash != null && x.FileHash.Contains(queryDto.FileHash));
        }

        if (queryDto?.FileCategory.HasValue == true)
        {
            exp = exp.And(x => x.FileCategory == queryDto.FileCategory);
        }

        if (queryDto?.StorageType.HasValue == true)
        {
            exp = exp.And(x => x.StorageType == queryDto.StorageType);
        }

        if (!string.IsNullOrEmpty(queryDto?.StorageConfig))
        {
            exp = exp.And(x => x.StorageConfig != null && x.StorageConfig.Contains(queryDto.StorageConfig));
        }

        if (!string.IsNullOrEmpty(queryDto?.AccessUrl))
        {
            exp = exp.And(x => x.AccessUrl != null && x.AccessUrl.Contains(queryDto.AccessUrl));
        }

        if (queryDto?.DownloadCount.HasValue == true)
        {
            exp = exp.And(x => x.DownloadCount == queryDto.DownloadCount);
        }

        if (queryDto?.FileStatus.HasValue == true)
        {
            exp = exp.And(x => x.FileStatus == queryDto.FileStatus);
        }

        if (queryDto?.IsPublic.HasValue == true)
        {
            exp = exp.And(x => x.IsPublic == queryDto.IsPublic);
        }

        if (!string.IsNullOrEmpty(queryDto?.FileDescription))
        {
            exp = exp.And(x => x.FileDescription != null && x.FileDescription.Contains(queryDto.FileDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileTags))
        {
            exp = exp.And(x => x.FileTags != null && x.FileTags.Contains(queryDto.FileTags));
        }

        if (!string.IsNullOrEmpty(queryDto?.IpAddress))
        {
            exp = exp.And(x => x.IpAddress != null && x.IpAddress.Contains(queryDto.IpAddress));
        }

        if (!string.IsNullOrEmpty(queryDto?.Location))
        {
            exp = exp.And(x => x.Location != null && x.Location.Contains(queryDto.Location));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtFieldJson))
        {
            exp = exp.And(x => x.ExtFieldJson != null && x.ExtFieldJson.Contains(queryDto.ExtFieldJson));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.LastDownloadTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.LastDownloadTime >= queryDto.LastDownloadTimeStart);
        }

        if (queryDto?.LastDownloadTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.LastDownloadTime <= queryDto.LastDownloadTimeEnd);
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
