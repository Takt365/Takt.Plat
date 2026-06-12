// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktFileUploadEngine.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：通用文件上传下载引擎实现（本地存储、分片合并；OSS/FTP 读删待扩展）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Takt.Application.Services;
using Takt.Domain.Interfaces;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using static Takt.Shared.Helpers.TaktFileHelper;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktFileUploadEngine 实现：物理文件 I/O，与业务表无关，供各模块应用服务复用。
/// </summary>
/// <remarks>
/// 当前仅支持 0（wwwroot 下相对路径存储）。
/// 隔离范围按租户/公司分目录；分片临时文件合并后删除。
/// 配置来源 TaktFileUploadOptions（appsettings FileUpload 节）。
/// </remarks>
public sealed class TaktFileUploadEngine : TaktServiceBase, ITaktFileUploadEngine
{
    /// <summary>
    /// Web 宿主环境（解析 ContentRoot 与 wwwroot）
    /// </summary>
    private readonly IWebHostEnvironment _webHostEnvironment;

    /// <summary>
    /// 上传配置（大小上限、扩展名白名单、相对路径等）
    /// </summary>
    private readonly TaktFileUploadOptions _uploadOptions;

    /// <summary>
    /// 初始化文件上传引擎
    /// </summary>
    /// <param name="webHostEnvironment">Web 宿主环境</param>
    /// <param name="uploadOptions">上传配置</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFileUploadEngine(
        IWebHostEnvironment webHostEnvironment,
        IOptions<TaktFileUploadOptions> uploadOptions,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        ArgumentNullException.ThrowIfNull(webHostEnvironment);
        ArgumentNullException.ThrowIfNull(uploadOptions);
        _webHostEnvironment = webHostEnvironment;
        _uploadOptions = uploadOptions.Value;
    }

    /// <summary>
    /// 整文件上传至本地存储
    /// </summary>
    /// <param name="fileStream">文件流；不可为 null</param>
    /// <param name="fileName">原始文件名（仅取 Path.GetFileName 安全段）</param>
    /// <param name="contentType">MIME 类型；为空时按扩展名推断</param>
    /// <param name="scope">租户/公司隔离范围；为空时从当前用户上下文解析</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存储结果（相对路径、哈希、访问 URL 等，不含业务表 Id）</returns>
    /// <exception cref="ArgumentNullException">fileStream 为 null</exception>
    /// <exception cref="ArgumentException">fileName 为空或 identifier 类参数非法</exception>
    public async Task<TaktStoredFileResult> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string? contentType,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileStream);
        if (fileStream.CanSeek && fileStream.Length == 0)
        {
            ThrowBusinessException("文件流不能为空");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        var resolvedScope = ResolveScope(scope);
        var safeOriginalName = Path.GetFileName(fileName);
        ValidateFileName(safeOriginalName);
        var fileExtension = GetNormalizedExtension(safeOriginalName);
        if (fileStream.CanSeek && fileStream.Length > _uploadOptions.MaxFileSizeBytes)
        {
            ThrowBusinessException($"文件大小超过上限（{_uploadOptions.MaxFileSizeBytes} 字节）");
        }

        var fileCode = Guid.NewGuid().ToString("N");
        var fileMimeType = string.IsNullOrWhiteSpace(contentType) ? GetMimeType(safeOriginalName) : contentType.Trim();
        var fileHash = await ComputeStreamHashAsync(fileStream);
        if (fileStream.CanSeek)
        {
            fileStream.Position = 0;
        }

        var finalFileName = ResolveFinalFileName(fileCode, fileExtension, resolvedScope.TargetFileName);
        var relativePath = BuildStoredRelativePath(resolvedScope, finalFileName);
        var absolutePath = GetAbsoluteUploadPath(relativePath);
        EnsureDirectoryExists(Path.GetDirectoryName(absolutePath)!);
        await using (var fileWriteStream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await fileStream.CopyToAsync(fileWriteStream, cancellationToken);
        }

        var fileSize = GetFileSize(absolutePath);
        EnsureFileSizeWithinLimit(fileSize);
        return BuildStoredFileResult(
            fileCode,
            safeOriginalName,
            finalFileName,
            relativePath,
            fileSize,
            fileMimeType,
            fileExtension,
            fileHash,
            MapFileCategory(GetFileCategory(safeOriginalName)));
    }

    /// <summary>
    /// 检查指定分片 part 文件是否已存在（断点续传）
    /// </summary>
    /// <param name="request">检查参数（identifier、chunkNumber、totalSize 等）</param>
    /// <param name="scope">租户/公司隔离范围；为空时从当前用户上下文解析</param>
    /// <returns>分片是否存在</returns>
    /// <exception cref="ArgumentNullException">request 为 null</exception>
    /// <exception cref="ArgumentException">identifier 非法或过长</exception>
    public Task<TaktFileChunkCheckResult> CheckChunkAsync(
        TaktFileChunkCheckRequest request,
        TaktFileUploadScope? scope = null)
    {
        ArgumentNullException.ThrowIfNull(request);
        var resolvedScope = ResolveScope(scope);
        ValidateIdentifier(request.Identifier);
        EnsureFileSizeWithinLimit(request.TotalSize);
        var chunkPath = GetChunkPartPath(resolvedScope, request.Identifier, request.ChunkNumber);
        return Task.FromResult(new TaktFileChunkCheckResult
        {
            Exists = FileExists(chunkPath),
        });
    }

    /// <summary>
    /// 列出指定 identifier 下已落盘的分片序号（断点续传批量恢复）
    /// </summary>
    /// <param name="request">查询参数</param>
    /// <param name="scope">租户/公司隔离范围</param>
    /// <returns>已上传分片序号（升序）</returns>
    public Task<TaktFileChunkListResult> ListUploadedChunksAsync(
        TaktFileChunkListRequest request,
        TaktFileUploadScope? scope = null)
    {
        ArgumentNullException.ThrowIfNull(request);
        var resolvedScope = ResolveScope(scope);
        ValidateIdentifier(request.Identifier);
        var dir = GetChunkDirectory(resolvedScope, request.Identifier);
        var uploaded = new List<int>();
        if (Directory.Exists(dir))
        {
            foreach (var partPath in Directory.GetFiles(dir, "*.part"))
            {
                var chunkName = Path.GetFileNameWithoutExtension(partPath);
                if (!int.TryParse(chunkName, out var chunkNumber) || chunkNumber < 1)
                {
                    continue;
                }

                if (request.TotalChunks > 0 && chunkNumber > request.TotalChunks)
                {
                    continue;
                }

                uploaded.Add(chunkNumber);
            }
        }

        uploaded.Sort();
        return Task.FromResult(new TaktFileChunkListResult
        {
            UploadedChunkNumbers = uploaded,
        });
    }

    /// <summary>
    /// 取消分片上传并删除临时目录
    /// </summary>
    /// <param name="identifier">上传会话标识</param>
    /// <param name="scope">租户/公司隔离范围</param>
    public Task CancelUploadedChunksAsync(string identifier, TaktFileUploadScope? scope = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        var resolvedScope = ResolveScope(scope);
        ValidateIdentifier(identifier);
        DeleteChunkDirectory(resolvedScope, identifier);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 上传单个分片至临时目录（{chunkRelativePath}/{tenant}/{company}/{identifier}/{n}.part）
    /// </summary>
    /// <param name="chunkStream">分片二进制流</param>
    /// <param name="request">分片元数据（序号、总数、文件大小等）</param>
    /// <param name="scope">租户/公司隔离范围；为空时从当前用户上下文解析</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <exception cref="ArgumentNullException">chunkStream 或 request 为 null</exception>
    /// <exception cref="ArgumentException">identifier 或文件名非法</exception>
    public async Task UploadChunkAsync(
        Stream chunkStream,
        TaktFileChunkUploadRequest request,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(chunkStream);
        ArgumentNullException.ThrowIfNull(request);
        var resolvedScope = ResolveScope(scope);
        ValidateIdentifier(request.Identifier);
        ValidateFileName(request.FileName);
        EnsureFileSizeWithinLimit(request.TotalSize);
        EnsureChunkCountWithinLimit(request.TotalChunks);
        if (request.ChunkNumber < 1 || request.ChunkNumber > request.TotalChunks)
        {
            ThrowBusinessException("分片序号无效");
        }

        var chunkPath = GetChunkPartPath(resolvedScope, request.Identifier, request.ChunkNumber);
        await WriteFileFromStreamAsync(chunkPath, chunkStream, createDirectory: true, cancellationToken);
    }

    /// <summary>
    /// 按序合并分片为完整文件，校验总大小后写入正式存储路径并清理分片临时目录
    /// </summary>
    /// <param name="request">合并参数（identifier、fileName、totalChunks、totalSize）</param>
    /// <param name="scope">租户/公司隔离范围；为空时从当前用户上下文解析</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>与整文件上传一致的存储结果</returns>
    /// <exception cref="ArgumentNullException">request 为 null</exception>
    /// <exception cref="ArgumentException">identifier 或文件名非法</exception>
    public async Task<TaktStoredFileResult> MergeChunksAsync(
        TaktFileChunkMergeRequest request,
        TaktFileUploadScope? scope = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var resolvedScope = ResolveScope(scope);
        ValidateIdentifier(request.Identifier);
        ValidateFileName(request.FileName);
        EnsureFileSizeWithinLimit(request.TotalSize);
        EnsureChunkCountWithinLimit(request.TotalChunks);

        var safeOriginalName = Path.GetFileName(request.FileName);
        ValidateFileName(safeOriginalName);
        var fileExtension = GetNormalizedExtension(safeOriginalName);
        var fileCode = Guid.NewGuid().ToString("N");
        var finalFileName = ResolveFinalFileName(fileCode, fileExtension, resolvedScope.TargetFileName);
        var relativePath = BuildStoredRelativePath(resolvedScope, finalFileName);
        var absolutePath = GetAbsoluteUploadPath(relativePath);
        EnsureDirectoryExists(Path.GetDirectoryName(absolutePath)!);

        await using (var output = new FileStream(absolutePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            long written = 0;
            for (var i = 1; i <= request.TotalChunks; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var chunkPath = GetChunkPartPath(resolvedScope, request.Identifier, i);
                if (!FileExists(chunkPath))
                {
                    ThrowBusinessException($"分片 {i} 不存在，无法合并");
                }

                await using var chunkStream = ReadFileStream(chunkPath);
                await chunkStream.CopyToAsync(output, cancellationToken);
                written = checked(written + chunkStream.Length);
            }

            if (request.TotalSize > 0 && written != request.TotalSize)
            {
                DeleteFile(absolutePath, throwIfNotExists: false);
                ThrowBusinessException("合并后文件大小与声明不一致");
            }
        }

        EnsureFileSizeWithinLimit(GetFileSize(absolutePath));
        DeleteChunkDirectory(resolvedScope, request.Identifier);
        return await BuildStoredFileResultAsync(
            safeOriginalName,
            finalFileName,
            relativePath,
            null,
            fileCode);
    }

    /// <summary>
    /// 按存储描述符打开本地只读文件流（调用方负责释放 Stream）
    /// </summary>
    /// <param name="descriptor">存储定位（FilePath + StorageType）</param>
    /// <param name="downloadFileName">下载展示文件名；为空时使用路径末段</param>
    /// <param name="contentType">MIME；为空时按扩展名推断</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可读流、文件名与 ContentType</returns>
    /// <exception cref="ArgumentNullException">descriptor 为 null</exception>
    /// <exception cref="ArgumentException">FilePath 为空</exception>
    public Task<TaktFileDownloadStreamResult> OpenReadAsync(
        TaktFileStorageDescriptor descriptor,
        string? downloadFileName = null,
        string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        ArgumentException.ThrowIfNullOrWhiteSpace(descriptor.FilePath);
        if (descriptor.StorageType != 0)
        {
            ThrowBusinessException("当前存储方式暂不支持引擎读流，请使用访问地址或扩展存储驱动");
        }

        var absolutePath = GetAbsoluteUploadPath(descriptor.FilePath);
        if (!FileExists(absolutePath))
        {
            ThrowBusinessException("物理文件不存在");
        }

        var name = string.IsNullOrWhiteSpace(downloadFileName)
            ? Path.GetFileName(descriptor.FilePath)
            : downloadFileName;
        var mime = string.IsNullOrWhiteSpace(contentType) ? GetMimeType(name) : contentType;
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(new TaktFileDownloadStreamResult
        {
            Stream = ReadFileStream(absolutePath),
            FileName = name,
            ContentType = mime,
        });
    }

    /// <summary>
    /// 删除本地物理文件（文件不存在时不抛异常）
    /// </summary>
    /// <param name="descriptor">存储定位</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <exception cref="ArgumentNullException">descriptor 为 null</exception>
    public Task DeleteStoredFileAsync(
        TaktFileStorageDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        if (descriptor.StorageType != 0)
        {
            ThrowBusinessException("当前存储方式暂不支持引擎删除物理文件");
        }

        var absolutePath = GetAbsoluteUploadPath(descriptor.FilePath);
        cancellationToken.ThrowIfCancellationRequested();
        DeleteFile(absolutePath, throwIfNotExists: false);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 解析租户/公司隔离范围；显式 scope 不完整时回退到当前三层上下文
    /// </summary>
    /// <param name="scope">显式范围；TenantCode/CompanyCode 均有效时直接使用</param>
    /// <returns>含 TenantCode、CompanyCode 与可选 CategoryPath 的有效范围</returns>
    private TaktFileUploadScope ResolveScope(TaktFileUploadScope? scope)
    {
        if (scope != null
            && !string.IsNullOrWhiteSpace(scope.TenantCode)
            && !string.IsNullOrWhiteSpace(scope.CompanyCode))
        {
            return scope;
        }

        EnsureThreeLayerContext();
        return new TaktFileUploadScope
        {
            TenantCode = CurrentTenantCode,
            CompanyCode = CurrentCompanyCode,
            CategoryPath = scope?.CategoryPath,
            FileUploadType = scope?.FileUploadType ?? TaktFileUploadType.Normal,
            TargetFileName = scope?.TargetFileName,
        };
    }

    /// <summary>
    /// 构建存储结果（含大小、哈希、MIME、分类与访问 URL）
    /// </summary>
    /// <param name="originalName">原始文件名</param>
    /// <param name="storedFileName">磁盘存储文件名</param>
    /// <param name="relativePath">相对 wwwroot 的存储路径</param>
    /// <param name="contentType">MIME；为空时按扩展名推断</param>
    /// <param name="fileCode">文件编码；为空时不填充</param>
    /// <param name="precomputedHash">预计算 MD5；为空时从磁盘文件计算</param>
    /// <returns>与 TaktFile 存储列对齐的结果 DTO</returns>
    private async Task<TaktStoredFileResult> BuildStoredFileResultAsync(
        string originalName,
        string storedFileName,
        string relativePath,
        string? contentType,
        string? fileCode = null,
        string? precomputedHash = null)
    {
        var absolutePath = GetAbsoluteUploadPath(relativePath);
        var fileSize = GetFileSize(absolutePath);
        EnsureFileSizeWithinLimit(fileSize);
        var extension = GetNormalizedExtension(originalName);
        var fileHash = precomputedHash ?? await ComputeFileHashAsync(absolutePath);
        return BuildStoredFileResult(
            fileCode ?? string.Empty,
            originalName,
            storedFileName,
            relativePath,
            fileSize,
            string.IsNullOrWhiteSpace(contentType) ? GetMimeType(originalName) : contentType.Trim(),
            extension,
            fileHash,
            MapFileCategory(GetFileCategory(originalName)));
    }

    /// <summary>
    /// 组装 TaktStoredFileResult（字段与 Routine.Tasks <c>FileUploadResultDto</c> 对齐）
    /// </summary>
    /// <param name="fileCode">文件编码</param>
    /// <param name="originalName">原始文件名</param>
    /// <param name="storedFileName">存储文件名</param>
    /// <param name="relativePath">相对路径</param>
    /// <param name="fileSize">文件大小</param>
    /// <param name="fileMimeType">MIME 类型</param>
    /// <param name="fileExtension">扩展名（不含点）</param>
    /// <param name="fileHash">MD5 哈希</param>
    /// <param name="fileCategory">文件分类</param>
    /// <returns>存储结果</returns>
    private static TaktStoredFileResult BuildStoredFileResult(
        string fileCode,
        string originalName,
        string storedFileName,
        string relativePath,
        long fileSize,
        string fileMimeType,
        string fileExtension,
        string fileHash,
        int fileCategory) =>
        new()
        {
            FileCode = fileCode,
            FilePath = relativePath,
            FileName = storedFileName,
            FileOriginalName = originalName,
            FileSize = fileSize,
            FileType = fileMimeType,
            FileExtension = fileExtension,
            FileHash = fileHash,
            AccessUrl = BuildAccessUrl(relativePath),
            StorageType = 0,
            StorageConfig = null,
            FileCategory = fileCategory,
        };

    /// <summary>
    /// 将 TaktFileHelper.FileCategory 映射为 TaktFileCategory 枚举
    /// </summary>
    /// <param name="category">Helper 侧文件分类</param>
    /// <returns>实体 FileCategory 值</returns>
    private static int MapFileCategory(FileCategory category) =>
        (int)category;

    /// <summary>
    /// 构建正式存储相对路径：{UploadRelativePath}/{tenant}/{company}/[{CategoryPath}/]{yyyy/MM/dd}/{storedFileName}
    /// </summary>
    /// <param name="scope">租户/公司与可选业务子路径（CategoryPath 由业务/字典传入）</param>
    /// <param name="storedFileName">唯一存储文件名</param>
    /// <returns>以 / 分隔的相对路径</returns>
    private string BuildStoredRelativePath(TaktFileUploadScope scope, string storedFileName)
    {
        var dateSegment = GenerateDatePath(DateTime.Now);
        var segments = new List<string>
        {
            _uploadOptions.UploadRelativePath,
            scope.TenantCode,
            scope.CompanyCode,
        };
        if (!string.IsNullOrWhiteSpace(scope.CategoryPath))
        {
            segments.Add(NormalizeCategoryPath(scope.CategoryPath));
        }

        segments.Add(dateSegment);
        segments.Add(storedFileName);
        return string.Join('/', segments);
    }

    /// <summary>
    /// 解析最终存储文件名（<c>targetFileName</c> 或 <c>{fileCode}.{ext}</c>）
    /// </summary>
    /// <param name="fileCode">文件编码</param>
    /// <param name="fileExtension">扩展名（不含点）</param>
    /// <param name="targetFileName">目标文件名（可选）</param>
    /// <returns>磁盘文件名</returns>
    private static string ResolveFinalFileName(string fileCode, string fileExtension, string? targetFileName)
    {
        if (!string.IsNullOrWhiteSpace(targetFileName))
        {
            var trimmed = Path.GetFileName(targetFileName.Trim());
            if (string.IsNullOrEmpty(Path.GetExtension(trimmed)) && !string.IsNullOrEmpty(fileExtension))
            {
                return $"{trimmed}.{fileExtension}";
            }

            return trimmed;
        }

        return string.IsNullOrEmpty(fileExtension)
            ? fileCode
            : $"{fileCode}.{fileExtension}";
    }

    /// <summary>
    /// 获取规范化扩展名（小写、不含点）
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns>扩展名；无扩展名时返回空串</returns>
    private static string GetNormalizedExtension(string fileName) =>
        Path.GetExtension(fileName)?.TrimStart('.')?.ToLowerInvariant() ?? string.Empty;

    /// <summary>
    /// 拒绝配置中的禁止扩展名（<see cref="TaktFileUploadOptions.DeniedExtensions"/>）
    /// </summary>
    /// <param name="fileExtension">扩展名（不含点）</param>
    private void ValidateDeniedExtension(string fileExtension)
    {
        if (string.IsNullOrWhiteSpace(fileExtension) || _uploadOptions.DeniedExtensions.Length == 0)
        {
            return;
        }

        if (_uploadOptions.DeniedExtensions.Any(x => string.Equals(x, fileExtension, StringComparison.OrdinalIgnoreCase)))
        {
            ThrowBusinessException($"不允许上传 .{fileExtension} 格式文件");
        }
    }

    /// <summary>
    /// 规范化业务子路径（统一斜杠、去首尾 /、禁止 .. 穿越）
    /// </summary>
    /// <param name="categoryPath">业务传入的子目录</param>
    /// <returns>安全相对路径段</returns>
    /// <exception cref="ArgumentException">含 .. 等非法段</exception>
    private static string NormalizeCategoryPath(string categoryPath)
    {
        var normalized = categoryPath.Replace('\\', '/').Trim('/');
        if (normalized.Contains("..", StringComparison.Ordinal))
        {
            throw new ArgumentException("CategoryPath 非法");
        }

        return normalized;
    }

    /// <summary>
    /// 构建对外 HTTP 访问 URL（根相对路径）
    /// </summary>
    /// <param name="relativePath">存储相对路径</param>
    /// <returns>以 / 开头的 URL 路径</returns>
    private static string BuildAccessUrl(string relativePath) =>
        $"/{relativePath.Replace('\\', '/')}";

    /// <summary>
    /// 将相对路径解析为 wwwroot 下绝对路径（Path.GetFullPath 规范化）
    /// </summary>
    /// <param name="relativePath">相对 wwwroot 的路径</param>
    /// <returns>本地绝对文件路径</returns>
    private string GetAbsoluteUploadPath(string relativePath)
    {
        var wwwroot = GetWwwRootPath(_webHostEnvironment.ContentRootPath);
        var normalized = relativePath.Replace('/', Path.DirectorySeparatorChar);
        return Path.GetFullPath(Path.Combine(wwwroot, normalized));
    }

    /// <summary>
    /// 获取分片临时目录绝对路径
    /// </summary>
    /// <param name="scope">租户/公司隔离范围</param>
    /// <param name="identifier">上传会话标识</param>
    /// <returns>分片目录绝对路径</returns>
    private string GetChunkDirectory(TaktFileUploadScope scope, string identifier)
    {
        var wwwroot = GetWwwRootPath(_webHostEnvironment.ContentRootPath);
        return Path.Combine(
            wwwroot,
            _uploadOptions.ChunkRelativePath.Replace('/', Path.DirectorySeparatorChar),
            scope.TenantCode,
            scope.CompanyCode,
            SanitizeIdentifier(identifier));
    }

    /// <summary>
    /// 获取分片 part 文件的绝对路径
    /// </summary>
    /// <param name="scope">租户/公司隔离范围</param>
    /// <param name="identifier">上传会话标识（通常为文件 MD5）</param>
    /// <param name="chunkNumber">分片序号（从 1 开始）</param>
    /// <returns>{chunkDir}/{chunkNumber}.part 绝对路径</returns>
    private string GetChunkPartPath(TaktFileUploadScope scope, string identifier, int chunkNumber)
    {
        return Path.Combine(GetChunkDirectory(scope, identifier), $"{chunkNumber}.part");
    }

    /// <summary>
    /// 递归删除指定 identifier 的分片临时目录
    /// </summary>
    /// <param name="scope">租户/公司隔离范围</param>
    /// <param name="identifier">上传会话标识</param>
    private void DeleteChunkDirectory(TaktFileUploadScope scope, string identifier)
    {
        var dir = GetChunkDirectory(scope, identifier);
        if (Directory.Exists(dir))
        {
            Directory.Delete(dir, recursive: true);
        }
    }

    /// <summary>
    /// 校验上传会话 identifier（非空、长度 ≤128、仅字母数字与 - _）
    /// </summary>
    /// <param name="identifier">客户端生成的文件唯一标识</param>
    /// <exception cref="ArgumentException">为空、过长或含非法字符</exception>
    private static void ValidateIdentifier(string identifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        if (identifier.Length > 128)
        {
            throw new ArgumentException("identifier 过长");
        }

        foreach (var ch in identifier)
        {
            if (!char.IsLetterOrDigit(ch) && ch != '-' && ch != '_')
            {
                throw new ArgumentException("identifier 含非法字符");
            }
        }
    }

    /// <summary>
    /// 规范化 identifier（去除首尾空白）
    /// </summary>
    /// <param name="identifier">原始 identifier</param>
    /// <returns>Trim 后的 identifier</returns>
    private static string SanitizeIdentifier(string identifier) => identifier.Trim();

    /// <summary>
    /// 校验文件名有效性与扩展名白名单（TaktFileUploadOptions.AllowedExtensions）
    /// </summary>
    /// <param name="fileName">原始或客户端文件名</param>
    private void ValidateFileName(string fileName)
    {
        var safeName = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(safeName))
        {
            ThrowBusinessException("文件名无效");
        }

        var ext = GetNormalizedExtension(safeName);
        ValidateDeniedExtension(ext);
        if (_uploadOptions.AllowedExtensions.Length == 0)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(ext)
            || !_uploadOptions.AllowedExtensions.Any(x => string.Equals(x, ext, StringComparison.OrdinalIgnoreCase)))
        {
            ThrowBusinessException("不允许的文件扩展名");
        }
    }

    /// <summary>
    /// 校验字节数不超过 TaktFileUploadOptions.MaxFileSizeBytes
    /// </summary>
    /// <param name="sizeBytes">待校验大小</param>
    private void EnsureFileSizeWithinLimit(long sizeBytes)
    {
        if (sizeBytes < 0 || sizeBytes > _uploadOptions.MaxFileSizeBytes)
        {
            ThrowBusinessException($"文件大小超过上限（{_uploadOptions.MaxFileSizeBytes} 字节）");
        }
    }

    /// <summary>
    /// 校验分片总数在 [1, TaktFileUploadOptions.MaxChunkCount] 范围内
    /// </summary>
    /// <param name="totalChunks">声明的总分片数</param>
    private void EnsureChunkCountWithinLimit(int totalChunks)
    {
        if (totalChunks < 1 || totalChunks > _uploadOptions.MaxChunkCount)
        {
            ThrowBusinessException($"分片数量超过上限（{_uploadOptions.MaxChunkCount}）");
        }
    }
}
