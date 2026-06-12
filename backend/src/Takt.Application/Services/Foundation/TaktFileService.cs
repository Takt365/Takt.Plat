// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktFileService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：文件应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 文件应用服务（CRUD + 上传/下载运行时）
/// </summary>
public class TaktFileService : TaktServiceBase, ITaktFileService
{
    private readonly ITaktCompanyRepository<TaktFile> _fileRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktFileUploadEngine _fileUploadEngine;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fileRepository">文件仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="fileUploadEngine">通用上传引擎</param>
    /// <param name="httpContextAccessor">HTTP 上下文</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFileService(
        ITaktCompanyRepository<TaktFile> fileRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktFileUploadEngine fileUploadEngine,
        IHttpContextAccessor httpContextAccessor,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        ArgumentNullException.ThrowIfNull(fileRepository);
        ArgumentNullException.ThrowIfNull(uniqueValidator);
        ArgumentNullException.ThrowIfNull(fileUploadEngine);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _fileRepository = fileRepository;
        _uniqueValidator = uniqueValidator;
        _fileUploadEngine = fileUploadEngine;
        _httpContextAccessor = httpContextAccessor;
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
                && (x.IsPublic == 0 || x.CreatedBy == currentUserId),
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
    // 上传 / 下载
    // ========================================

    /// <summary>
    /// 整文件上传（委托引擎落盘并写入 TaktFile 元数据）
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="fileName">原始文件名</param>
    /// <param name="contentType">MIME 类型</param>
    /// <param name="meta">可选业务元数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>已持久化的文件 DTO</returns>
    public async Task<TaktFileUploadResultDto> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string? contentType,
        TaktFileUploadMetaDto? meta = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileStream);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        var enrichedMeta = EnrichUploadMeta(meta);
        var stored = await _fileUploadEngine.UploadFileAsync(
            fileStream,
            fileName,
            contentType,
            BuildUploadScope(enrichedMeta),
            cancellationToken);
        var entity = await PersistStoredFileAsync(stored, enrichedMeta);
        return MapToUploadResultDto(entity);
    }

    /// <summary>
    /// 检查分片是否已上传（秒传/断点续传前置）
    /// </summary>
    /// <param name="dto">分片标识与序号等检查参数</param>
    /// <returns>分片是否已存在于临时存储</returns>
    public async Task<TaktFileChunkCheckResultDto> CheckFileChunkAsync(TaktFileChunkCheckDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var result = await _fileUploadEngine.CheckChunkAsync(new TaktFileChunkCheckRequest
        {
            Identifier = dto.Identifier,
            ChunkNumber = dto.ChunkNumber,
            ChunkSize = dto.ChunkSize,
            TotalSize = dto.TotalSize,
            FileName = dto.FileName,
        });
        return new TaktFileChunkCheckResultDto { Exists = result.Exists };
    }

    /// <summary>
    /// 列出已上传分片序号（断点续传批量恢复）
    /// </summary>
    /// <param name="dto">查询参数</param>
    /// <returns>已上传分片序号</returns>
    public async Task<TaktFileChunkListResultDto> ListFileChunksAsync(TaktFileChunkListDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var result = await _fileUploadEngine.ListUploadedChunksAsync(new TaktFileChunkListRequest
        {
            Identifier = dto.Identifier,
            TotalChunks = dto.TotalChunks,
        });
        return new TaktFileChunkListResultDto
        {
            UploadedChunkNumbers = result.UploadedChunkNumbers,
        };
    }

    /// <summary>
    /// 取消分片上传并清理临时目录
    /// </summary>
    /// <param name="dto">取消参数</param>
    public Task CancelFileChunksAsync(TaktFileChunkCancelDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return _fileUploadEngine.CancelUploadedChunksAsync(dto.Identifier);
    }

    /// <summary>
    /// 上传单个分片至临时存储
    /// </summary>
    /// <param name="chunkStream">分片二进制流</param>
    /// <param name="dto">分片元数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    public Task UploadFileChunkAsync(
        Stream chunkStream,
        TaktFileChunkUploadDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return _fileUploadEngine.UploadChunkAsync(chunkStream, new TaktFileChunkUploadRequest
        {
            Identifier = dto.Identifier,
            ChunkNumber = dto.ChunkNumber,
            TotalChunks = dto.TotalChunks,
            ChunkSize = dto.ChunkSize,
            TotalSize = dto.TotalSize,
            FileName = dto.FileName,
        }, null, cancellationToken);
    }

    /// <summary>
    /// 合并分片为完整文件并写入 TaktFile 元数据
    /// </summary>
    /// <param name="dto">合并参数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>已持久化的文件 DTO</returns>
    public async Task<TaktFileUploadResultDto> MergeFileChunksAsync(
        TaktFileChunkMergeDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var meta = EnrichUploadMeta(new TaktFileUploadMetaDto
        {
            FileDescription = dto.FileDescription,
            FileTags = dto.FileTags,
            IsPublic = dto.IsPublic,
            IpAddress = dto.IpAddress,
            Location = dto.Location,
            FileUploadType = dto.FileUploadType,
            TargetFileName = dto.TargetFileName,
            CategoryPath = dto.CategoryPath,
        });
        var stored = await _fileUploadEngine.MergeChunksAsync(new TaktFileChunkMergeRequest
        {
            Identifier = dto.Identifier,
            FileName = dto.FileName,
            TotalChunks = dto.TotalChunks,
            TotalSize = dto.TotalSize,
        }, BuildUploadScope(meta), cancellationToken);
        var entity = await PersistStoredFileAsync(stored, meta);
        return MapToUploadResultDto(entity);
    }

    /// <summary>
    /// 下载文件：校验租户/公司与启用状态，打开物理流并递增下载次数
    /// </summary>
    /// <param name="fileId">文件主键 ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可读流、原始文件名与 Content-Type</returns>
    public async Task<TaktFileDownloadResultDto> DownloadFileAsync(
        long fileId,
        CancellationToken cancellationToken = default)
    {
        var entity = await GetOwnedFileEntityAsync(fileId);
        if (entity.FileStatus != 1)
        {
            ThrowBusinessException("文件已禁用，无法下载");
        }

        var streamResult = await _fileUploadEngine.OpenReadAsync(
            new TaktFileStorageDescriptor
            {
                FilePath = entity.FilePath,
                StorageType = entity.StorageType,
            },
            entity.FileOriginalName,
            entity.FileType,
            cancellationToken);

        entity.DownloadCount = checked(entity.DownloadCount + 1);
        entity.LastDownloadTime = DateTime.Now;
        await _fileRepository.UpdateAsync(entity);

        return new TaktFileDownloadResultDto
        {
            Stream = streamResult.Stream,
            FileName = streamResult.FileName,
            ContentType = streamResult.ContentType,
        };
    }

    /// <summary>
    /// 更新文件公开范围（TaktFile.IsPublic）
    /// </summary>
    /// <param name="fileId">文件主键 ID</param>
    /// <param name="dto">公开范围 DTO</param>
    /// <returns>更新后的文件 DTO</returns>
    public async Task<TaktFileDto> ChangeFilePublicAccessAsync(long fileId, TaktFilePublicAccessDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var entity = await GetOwnedFileEntityAsync(fileId);
        entity.IsPublic = dto.IsPublic;
        await _fileRepository.UpdateAsync(entity);
        return entity.Adapt<TaktFileDto>();
    }

    /// <summary>
    /// 获取当前租户+公司下的文件实体
    /// </summary>
    /// <param name="fileId">文件 ID</param>
    /// <returns>实体</returns>
    private async Task<TaktFile> GetOwnedFileEntityAsync(long fileId)
    {
        EnsureThreeLayerContext();
        var entity = await _fileRepository.GetByIdAsync(fileId);
        if (entity == null
            || entity.TenantCode != CurrentTenantCode
            || entity.CompanyCode != CurrentCompanyCode
            || entity.IsDeleted != 0)
        {
            ThrowBusinessException("文件不存在");
        }

        if (!TaktFileAccessHelper.CanAccess(entity.IsPublic, entity.CreatedBy, CurrentUserId))
        {
            ThrowBusinessException("无权访问该文件");
        }

        return entity;
    }

    /// <summary>
    /// 将引擎存储结果写入 TaktFile 元数据
    /// </summary>
    /// <param name="stored">存储结果</param>
    /// <param name="meta">业务附加字段</param>
    /// <returns>已持久化实体</returns>
    private async Task<TaktFile> PersistStoredFileAsync(TaktStoredFileResult stored, TaktFileUploadMetaDto meta)
    {
        ArgumentNullException.ThrowIfNull(stored);
        ArgumentNullException.ThrowIfNull(meta);
        EnsureThreeLayerContext();
        ValidateStoredFileResult(stored);
        var entity = new TaktFile
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            FileCode = !string.IsNullOrWhiteSpace(stored.FileCode)
                ? stored.FileCode.Trim()
                : GenerateFileCode(),
            FileName = stored.FileName,
            FileOriginalName = stored.FileOriginalName,
            FilePath = stored.FilePath,
            FileSize = stored.FileSize,
            FileType = NormalizeRequiredString(stored.FileType),
            FileExtension = NormalizeRequiredString(stored.FileExtension),
            FileHash = NormalizeRequiredString(stored.FileHash),
            FileCategory = stored.FileCategory,
            StorageType = stored.StorageType,
            StorageConfig = NormalizeNullableString(stored.StorageConfig),
            AccessUrl = NormalizeRequiredString(stored.AccessUrl),
            DownloadCount = 0,
            FileStatus = 1,
            IsPublic = meta.IsPublic ?? 0,
            FileDescription = NormalizeRequiredString(meta.FileDescription),
            FileTags = NormalizeRequiredString(meta.FileTags),
            IpAddress = NormalizeRequiredString(meta.IpAddress),
            Location = NormalizeRequiredString(meta.Location),
        };
        entity = await _fileRepository.CreateAsync(entity);
        return entity;
    }

    /// <summary>
    /// 校验引擎存储结果是否满足 TaktFile 非空列要求
    /// </summary>
    /// <param name="stored">存储结果</param>
    private static void ValidateStoredFileResult(TaktStoredFileResult stored)
    {
        if (string.IsNullOrWhiteSpace(stored.FileName)
            || string.IsNullOrWhiteSpace(stored.FileOriginalName)
            || string.IsNullOrWhiteSpace(stored.FilePath))
        {
            throw new InvalidOperationException("文件存储结果缺少必填路径或名称字段");
        }

        if (stored.FileSize < 0)
        {
            throw new InvalidOperationException("文件大小无效");
        }
    }

    /// <summary>
    /// 将必填字符串规范为非 null
    /// </summary>
    /// <param name="value">原始值</param>
    /// <returns>去首尾空白后的非 null 字符串</returns>
    private static string NormalizeRequiredString(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

    /// <summary>
    /// 将允许为 null 的字符串规范化
    /// </summary>
    /// <param name="value">原始值</param>
    /// <returns>去首尾空白后的值；空白为 null</returns>
    private static string? NormalizeNullableString(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    /// <summary>
    /// 补全上传元数据中的 IP/地理位置
    /// </summary>
    /// <param name="meta">原始元数据</param>
    /// <returns>补全后的元数据</returns>
    private TaktFileUploadMetaDto EnrichUploadMeta(TaktFileUploadMetaDto? meta)
    {
        meta ??= new TaktFileUploadMetaDto();
        if (string.IsNullOrWhiteSpace(meta.IpAddress))
        {
            meta.IpAddress = ResolveClientIpAddress();
        }

        if (string.IsNullOrWhiteSpace(meta.Location))
        {
            meta.Location = !string.IsNullOrWhiteSpace(meta.IpAddress)
                ? TaktLocationHelper.ResolveIpLocationForLog(meta.IpAddress) ?? string.Empty
                : string.Empty;
        }

        meta.FileDescription = NormalizeRequiredString(meta.FileDescription);
        meta.FileTags = NormalizeRequiredString(meta.FileTags);
        meta.IpAddress = NormalizeRequiredString(meta.IpAddress);
        meta.Location = NormalizeRequiredString(meta.Location);
        return meta;
    }

    /// <summary>
    /// 由上传元数据构建引擎隔离范围（上传类型、目标文件名）
    /// </summary>
    /// <param name="meta">上传元数据</param>
    /// <returns>引擎 scope（租户/公司在引擎内解析）</returns>
    private static TaktFileUploadScope BuildUploadScope(TaktFileUploadMetaDto meta)
    {
        ArgumentNullException.ThrowIfNull(meta);
        return new TaktFileUploadScope
        {
            FileUploadType = meta.FileUploadType,
            TargetFileName = meta.TargetFileName,
            CategoryPath = meta.CategoryPath,
        };
    }

    /// <summary>
    /// 解析当前请求客户端 IP
    /// </summary>
    /// <returns>IP；无 HttpContext 时返回空串</returns>
    private string ResolveClientIpAddress()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            return string.Empty;
        }

        var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwarded))
        {
            return forwarded.Split(',')[0].Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 生成文件业务编码
    /// </summary>
    private static string GenerateFileCode() =>
        $"FILE-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}"[..32];

    /// <summary>
    /// 将已持久化实体映射为上传结果 DTO
    /// </summary>
    /// <param name="entity">文件实体</param>
    /// <returns>上传结果 DTO</returns>
    private static TaktFileUploadResultDto MapToUploadResultDto(TaktFile entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new TaktFileUploadResultDto
        {
            FileId = entity.Id,
            FileCode = entity.FileCode,
            FileName = entity.FileName,
            FileOriginalName = entity.FileOriginalName,
            FilePath = entity.FilePath,
            FileSize = entity.FileSize,
            FileType = entity.FileType,
            FileExtension = entity.FileExtension,
            FileHash = entity.FileHash,
            FileCategory = entity.FileCategory,
            AccessUrl = entity.AccessUrl,
        };
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
