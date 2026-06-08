// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktFileUploadDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：文件上传引擎 DTO（整文件/分片/合并/下载，与 frontend upload.ts 协议对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Foundation;

/// <summary>
/// 分片存在性检查请求（对应 upload.ts checkChunk）
/// </summary>
public class TaktFileChunkCheckDto
{
    /// <summary>
    /// 文件唯一标识（通常为 MD5）
    /// </summary>
    [Required]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 分片序号（从 1 开始）
    /// </summary>
    [Range(1, int.MaxValue)]
    public int ChunkNumber { get; set; }

    /// <summary>
    /// 当前分片大小（字节）
    /// </summary>
    public long ChunkSize { get; set; }

    /// <summary>
    /// 文件总大小（字节）
    /// </summary>
    public long TotalSize { get; set; }

    /// <summary>
    /// 原始文件名
    /// </summary>
    public string? FileName { get; set; }
}

/// <summary>
/// 分片存在性检查结果
/// </summary>
public class TaktFileChunkCheckResultDto
{
    /// <summary>
    /// 分片是否已存在
    /// </summary>
    public bool Exists { get; set; }
}

/// <summary>
/// 分片上传元数据（multipart 表单字段，不含 file 流）
/// </summary>
public class TaktFileChunkUploadDto
{
    /// <summary>
    /// 文件唯一标识
    /// </summary>
    [Required]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 分片序号（从 1 开始）
    /// </summary>
    [Range(1, int.MaxValue)]
    public int ChunkNumber { get; set; }

    /// <summary>
    /// 总分片数
    /// </summary>
    [Range(1, int.MaxValue)]
    public int TotalChunks { get; set; }

    /// <summary>
    /// 当前分片大小（字节）
    /// </summary>
    public long ChunkSize { get; set; }

    /// <summary>
    /// 文件总大小（字节）
    /// </summary>
    public long TotalSize { get; set; }

    /// <summary>
    /// 原始文件名
    /// </summary>
    [Required]
    public string FileName { get; set; } = string.Empty;
}

/// <summary>
/// 分片合并请求（对应 upload.ts mergeChunks）
/// </summary>
public class TaktFileChunkMergeDto
{
    /// <summary>
    /// 文件唯一标识
    /// </summary>
    [Required]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 原始文件名
    /// </summary>
    [Required]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 总分片数
    /// </summary>
    [Range(1, int.MaxValue)]
    public int TotalChunks { get; set; }

    /// <summary>
    /// 文件总大小（字节）
    /// </summary>
    public long TotalSize { get; set; }

    /// <summary>
    /// 文件描述（可选）
    /// </summary>
    public string? FileDescription { get; set; }

    /// <summary>
    /// 文件标签（可选）
    /// </summary>
    public string? FileTags { get; set; }

    /// <summary>
    /// 是否公开（默认公开）→ <c>TaktFile.IsPublic</c>
    /// </summary>
    public TaktFilePublicAccess? IsPublic { get; set; }

    /// <summary>
    /// IP 地址（上传来源；未传时由服务从 HttpContext 解析）→ <c>TaktFile.IpAddress</c>
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// 位置（未传时由服务根据 IP 解析）→ <c>TaktFile.Location</c>
    /// </summary>
    public string? Location { get; set; }
}

/// <summary>
/// 整文件上传附加元数据（multipart 可选字段）
/// </summary>
public class TaktFileUploadMetaDto
{
    /// <summary>
    /// 文件描述 → <c>TaktFile.FileDescription</c>
    /// </summary>
    public string? FileDescription { get; set; }

    /// <summary>
    /// 文件标签 → <c>TaktFile.FileTags</c>
    /// </summary>
    public string? FileTags { get; set; }

    /// <summary>
    /// 是否公开 → <c>TaktFile.IsPublic</c>
    /// </summary>
    public TaktFilePublicAccess? IsPublic { get; set; }

    /// <summary>
    /// IP 地址（未传时由服务从 HttpContext 解析）→ <c>TaktFile.IpAddress</c>
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// 位置（未传时由服务根据 IP 解析）→ <c>TaktFile.Location</c>
    /// </summary>
    public string? Location { get; set; }
}

/// <summary>
/// 文件公开范围更新 DTO
/// </summary>
public class TaktFilePublicAccessDto
{
    /// <summary>
    /// 是否公开
    /// </summary>
    [Required]
    public TaktFilePublicAccess IsPublic { get; set; }
}

/// <summary>
/// 文件下载结果（引擎 → 控制器）
/// </summary>
public sealed class TaktFileDownloadResultDto
{
    /// <summary>
    /// 可读流（调用方负责释放）
    /// </summary>
    public required Stream Stream { get; init; }

    /// <summary>
    /// 下载文件名（原始名）
    /// </summary>
    public required string FileName { get; init; }

    /// <summary>
    /// MIME 类型
    /// </summary>
    public required string ContentType { get; init; }
}
