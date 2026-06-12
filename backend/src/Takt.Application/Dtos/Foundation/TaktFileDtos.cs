// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktFileDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：File 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFile 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// File 响应 DTO
// ========================================

/// <summary>
/// 文件实体 公司级实体：文件元数据按租户+公司隔离；字段与前端 entity.file.* 及业务附件 JSON 结构对齐
/// 对应前端 TaktFileDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFileDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FileID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
    /// </summary>
    public string FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（存储文件名）
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    public string FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（相对路径或完整路径）
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    public string FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    public string FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类（字典 sys_file_category）
    /// </summary>
    public int FileCategory { get; set; }

    /// <summary>
    /// 存储方式（字典 sys_storage_type）
    /// </summary>
    public int StorageType { get; set; }

    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    public string? StorageConfig { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int DownloadCount { get; set; } = 0;

    /// <summary>
    /// 最后下载时间
    /// </summary>
    public DateTime? LastDownloadTime { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int FileStatus { get; set; }

    /// <summary>
    /// 是否公开（字典 sys_is_public；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    public int IsPublic { get; set; }

    /// <summary>
    /// 文件描述
    /// </summary>
    public string FileDescription { get; set; } = string.Empty;

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    public string FileTags { get; set; } = string.Empty;

    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    public string Location { get; set; } = string.Empty;

}

// ========================================
// File 查询 DTO
// ========================================

/// <summary>
/// File 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFileQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
    /// </summary>
    public string? FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（存储文件名）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    public string? FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（相对路径或完整路径）
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    public string? FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类（字典 sys_file_category）
    /// </summary>
    public int? FileCategory { get; set; }

    /// <summary>
    /// 存储方式（字典 sys_storage_type）
    /// </summary>
    public int? StorageType { get; set; }

    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    public string? StorageConfig { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int? DownloadCount { get; set; }

    /// <summary>
    /// 最后下载时间（范围查询-开始）
    /// </summary>
    public DateTime? LastDownloadTimeStart { get; set; }

    /// <summary>
    /// 最后下载时间（范围查询-结束）
    /// </summary>
    public DateTime? LastDownloadTimeEnd { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int? FileStatus { get; set; }

    /// <summary>
    /// 是否公开（字典 sys_is_public；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    public int? IsPublic { get; set; }

    /// <summary>
    /// 文件描述
    /// </summary>
    public string? FileDescription { get; set; } = string.Empty;

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    public string? FileTags { get; set; } = string.Empty;

    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    public string? IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    public string? Location { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建File DTO
// ========================================

/// <summary>
/// 创建File DTO
/// </summary>
public class TaktFileCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
    /// </summary>
    [Required(ErrorMessage = "文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）不能为空")]
    public string FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（存储文件名）
    /// </summary>
    [Required(ErrorMessage = "文件名称（存储文件名）不能为空")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    [Required(ErrorMessage = "文件原始名称（上传时的原始文件名）不能为空")]
    public string FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（相对路径或完整路径）
    /// </summary>
    [Required(ErrorMessage = "文件路径（相对路径或完整路径）不能为空")]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    [Required(ErrorMessage = "文件 MIME 类型不能为空")]
    public string FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    [Required(ErrorMessage = "文件扩展名不能为空")]
    public string FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    public string FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类（字典 sys_file_category）
    /// </summary>
    public int FileCategory { get; set; }

    /// <summary>
    /// 存储方式（字典 sys_storage_type）
    /// </summary>
    public int StorageType { get; set; }

    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    public string? StorageConfig { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    [Required(ErrorMessage = "访问地址（文件 URL）不能为空")]
    public string AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int DownloadCount { get; set; } = 0;

    /// <summary>
    /// 最后下载时间
    /// </summary>
    public DateTime? LastDownloadTime { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int FileStatus { get; set; }

    /// <summary>
    /// 是否公开（字典 sys_is_public；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    public int IsPublic { get; set; }

    /// <summary>
    /// 文件描述
    /// </summary>
    [Required(ErrorMessage = "文件描述不能为空")]
    public string FileDescription { get; set; } = string.Empty;

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    [Required(ErrorMessage = "文件标签（多个标签用逗号分隔）不能为空")]
    public string FileTags { get; set; } = string.Empty;

    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    [Required(ErrorMessage = "IP 地址（上传或访问来源）不能为空")]
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    [Required(ErrorMessage = "位置（IP 对应地理位置）不能为空")]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新File DTO
// ========================================

/// <summary>
/// 更新File DTO
/// 继承 TaktFileCreateDto，添加 FileId 字段
/// </summary>
public class TaktFileUpdateDto : TaktFileCreateDto
{
    /// <summary>
    /// FileID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

}

// ========================================
// File 状态 DTO
// ========================================

/// <summary>
/// File 状态更新 DTO
/// </summary>
public class TaktFileStatusDto
{
    /// <summary>
    /// FileID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（1=启用，0=禁用）不能为空")]
    public int FileStatus { get; set; }
}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// File 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFileExportDto
{
    /// <summary>
    /// FileID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（唯一索引：租户+公司内唯一，见 ix_file_code_unique）
    /// </summary>
    public string FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（存储文件名）
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称（上传时的原始文件名）
    /// </summary>
    public string FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（相对路径或完整路径）
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件 MIME 类型
    /// </summary>
    public string FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 文件哈希值（MD5 或 SHA256，用于去重与校验）
    /// </summary>
    public string FileHash { get; set; } = string.Empty;

    /// <summary>
    /// 文件分类（字典 sys_file_category）
    /// </summary>
    public int FileCategory { get; set; }

    /// <summary>
    /// 存储方式（字典 sys_storage_type）
    /// </summary>
    public int StorageType { get; set; }

    /// <summary>
    /// 存储配置（JSON，OSS/FTP 等扩展配置）
    /// </summary>
    public string? StorageConfig { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件 URL）
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int DownloadCount { get; set; } = 0;

    /// <summary>
    /// 最后下载时间
    /// </summary>
    public DateTime? LastDownloadTime { get; set; }

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int FileStatus { get; set; }

    /// <summary>
    /// 是否公开（字典 sys_is_public；0=公开同公司可见，1=私有仅创建人可见/可改/可下载）
    /// </summary>
    public int IsPublic { get; set; }

    /// <summary>
    /// 文件描述
    /// </summary>
    public string FileDescription { get; set; } = string.Empty;

    /// <summary>
    /// 文件标签（多个标签用逗号分隔）
    /// </summary>
    public string FileTags { get; set; } = string.Empty;

    /// <summary>
    /// IP 地址（上传或访问来源）
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 位置（IP 对应地理位置）
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

// ========================================
// 上传 / 下载 DTO
// ========================================

/// <summary>
/// 文件上传结果 DTO（Foundation 上传/合并接口响应；引擎落盘 + 元数据落库后的核心字段）
/// </summary>
/// <remarks>
/// 与 Routine.Tasks 的 <c>FileUploadResultDto</c> / <c>TaktUploadEngineService</c> 无关；
/// 本 DTO 由 TaktFileService 在 TaktFile 持久化后映射，含主键 FileId。
/// </remarks>
public class TaktFileUploadResultDto
{
    /// <summary>
    /// FileID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 文件编码
    /// </summary>
    public string FileCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（存储文件名）
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件原始名称
    /// </summary>
    public string FileOriginalName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径（相对路径）
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件类型（MIME 类型）
    /// </summary>
    public string? FileType { get; set; }

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; }

    /// <summary>
    /// 文件哈希值
    /// </summary>
    public string? FileHash { get; set; }

    /// <summary>
    /// 文件分类（字典 sys_file_category）
    /// </summary>
    public int FileCategory { get; set; }

    /// <summary>
    /// 访问地址
    /// </summary>
    public string AccessUrl { get; set; } = string.Empty;
}

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
/// 已上传分片列表查询（断点续传）
/// </summary>
public class TaktFileChunkListDto
{
    /// <summary>
    /// 文件唯一标识（通常为 MD5）
    /// </summary>
    [Required]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// 总分片数（可选；大于 0 时过滤非法序号）
    /// </summary>
    [Range(0, int.MaxValue)]
    public int TotalChunks { get; set; }
}

/// <summary>
/// 已上传分片列表结果
/// </summary>
public class TaktFileChunkListResultDto
{
    /// <summary>
    /// 已上传的分片序号（从 1 开始）
    /// </summary>
    public List<int> UploadedChunkNumbers { get; set; } = [];
}

/// <summary>
/// 取消分片上传请求
/// </summary>
public class TaktFileChunkCancelDto
{
    /// <summary>
    /// 文件唯一标识
    /// </summary>
    [Required]
    public string Identifier { get; set; } = string.Empty;
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
    public int? IsPublic { get; set; }

    /// <summary>
    /// IP 地址（上传来源；未传时由服务从 HttpContext 解析）→ <c>TaktFile.IpAddress</c>
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// 位置（未传时由服务根据 IP 解析）→ <c>TaktFile.Location</c>
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// 上传类型（引擎路由：普通/分片/直传 OSS；存储子目录由 CategoryPath 传入）
    /// </summary>
    public TaktFileUploadType FileUploadType { get; set; } = TaktFileUploadType.Normal;

    /// <summary>
    /// 目标文件名（可选；未指定时使用引擎生成的 fileCode.扩展名）
    /// </summary>
    public string? TargetFileName { get; set; }

    /// <summary>
    /// 存储子目录（业务/字典传入，映射磁盘路径 CategoryPath 段）
    /// </summary>
    public string? CategoryPath { get; set; }
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
    public int? IsPublic { get; set; }

    /// <summary>
    /// IP 地址（未传时由服务从 HttpContext 解析）→ <c>TaktFile.IpAddress</c>
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// 位置（未传时由服务根据 IP 解析）→ <c>TaktFile.Location</c>
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// 上传类型（引擎路由：普通/分片/直传 OSS；存储子目录由 CategoryPath 传入）
    /// </summary>
    public TaktFileUploadType FileUploadType { get; set; } = TaktFileUploadType.Normal;

    /// <summary>
    /// 目标文件名（可选；未指定时使用引擎生成的 fileCode.扩展名）
    /// </summary>
    public string? TargetFileName { get; set; }

    /// <summary>
    /// 存储子目录（业务/字典传入，映射磁盘路径 CategoryPath 段）
    /// </summary>
    public string? CategoryPath { get; set; }
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
    public int IsPublic { get; set; }
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
