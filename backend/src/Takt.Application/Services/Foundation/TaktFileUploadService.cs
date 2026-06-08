// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktFileUploadService.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：文件上传应用服务（编排通用引擎与 TaktFile 元数据落库）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Mapster;
using Microsoft.AspNetCore.Http;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 文件上传应用服务（运行时上传/下载；物理 I/O 委托 <see cref="ITaktFileUploadEngine"/>）
/// </summary>
public sealed class TaktFileUploadService : TaktServiceBase, ITaktFileUploadService
{
    private readonly ITaktCompanyRepository<TaktFile> _fileRepository;
    private readonly ITaktFileUploadEngine _fileUploadEngine;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 初始化文件上传应用服务
    /// </summary>
    /// <param name="fileRepository">文件仓储</param>
    /// <param name="fileUploadEngine">通用上传引擎</param>
    /// <param name="httpContextAccessor">HTTP 上下文</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFileUploadService(
        ITaktCompanyRepository<TaktFile> fileRepository,
        ITaktFileUploadEngine fileUploadEngine,
        IHttpContextAccessor httpContextAccessor,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        ArgumentNullException.ThrowIfNull(fileRepository);
        ArgumentNullException.ThrowIfNull(fileUploadEngine);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _fileRepository = fileRepository;
        _fileUploadEngine = fileUploadEngine;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 整文件上传（委托引擎落盘并写入 <see cref="TaktFile"/> 元数据）
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="fileName">原始文件名</param>
    /// <param name="contentType">MIME 类型</param>
    /// <param name="meta">可选业务元数据（描述、标签、公开范围等）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>已持久化的文件 DTO</returns>
    public async Task<TaktFileDto> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string? contentType,
        TaktFileUploadMetaDto? meta = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileStream);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        var stored = await _fileUploadEngine.UploadFileAsync(fileStream, fileName, contentType, null, cancellationToken);
        var entity = await PersistStoredFileAsync(stored, EnrichUploadMeta(meta));
        return entity.Adapt<TaktFileDto>();
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
    /// 上传单个分片至临时存储
    /// </summary>
    /// <param name="chunkStream">分片二进制流</param>
    /// <param name="dto">分片元数据（identifier、chunkNumber、totalChunks 等）</param>
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
    /// 合并分片为完整文件并写入 <see cref="TaktFile"/> 元数据
    /// </summary>
    /// <param name="dto">合并参数（identifier、fileName、totalChunks 等）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>已持久化的文件 DTO</returns>
    public async Task<TaktFileDto> MergeFileChunksAsync(
        TaktFileChunkMergeDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var stored = await _fileUploadEngine.MergeChunksAsync(new TaktFileChunkMergeRequest
        {
            Identifier = dto.Identifier,
            FileName = dto.FileName,
            TotalChunks = dto.TotalChunks,
            TotalSize = dto.TotalSize,
        }, null, cancellationToken);
        var meta = EnrichUploadMeta(new TaktFileUploadMetaDto
        {
            FileDescription = dto.FileDescription,
            FileTags = dto.FileTags,
            IsPublic = dto.IsPublic,
            IpAddress = dto.IpAddress,
            Location = dto.Location,
        });
        var entity = await PersistStoredFileAsync(stored, meta);
        return entity.Adapt<TaktFileDto>();
    }

    /// <summary>
    /// 下载文件：校验租户/公司与启用状态，打开物理流并递增下载次数
    /// </summary>
    /// <param name="fileId">文件主键 ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可读流、原始文件名与 Content-Type（调用方负责释放流）</returns>
    public async Task<TaktFileDownloadResultDto> DownloadFileAsync(
        long fileId,
        CancellationToken cancellationToken = default)
    {
        var entity = await GetOwnedFileEntityAsync(fileId);
        if (entity.FileStatus != TaktCommonStatus.Enabled)
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
    /// 更新文件公开范围（<see cref="TaktFile.IsPublic"/>）
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
    /// 将引擎存储结果写入 <see cref="TaktFile"/> 元数据（字段 1:1 对齐）
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
            FileCode = GenerateFileCode(),
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
            FileStatus = TaktCommonStatus.Enabled,
            IsPublic = meta.IsPublic ?? TaktFilePublicAccess.Public,
            FileDescription = NormalizeRequiredString(meta.FileDescription),
            FileTags = NormalizeRequiredString(meta.FileTags),
            IpAddress = NormalizeRequiredString(meta.IpAddress),
            Location = NormalizeRequiredString(meta.Location),
        };
        entity = await _fileRepository.CreateAsync(entity);
        return entity;
    }

    /// <summary>
    /// 校验引擎存储结果是否满足 <see cref="TaktFile"/> 非空列要求
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
    /// 将必填字符串规范为非 null（空白视为 <see cref="string.Empty"/>）
    /// </summary>
    /// <param name="value">原始值</param>
    /// <returns>去首尾空白后的非 null 字符串</returns>
    private static string NormalizeRequiredString(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

    /// <summary>
    /// 将允许为 null 的字符串规范化（空白视为 null，供 StorageConfig）
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
}
