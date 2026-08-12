// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.DocumentCenter
// 文件名称：TaktDocumentVersionDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：DocumentVersion 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDocumentVersion 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.DocumentCenter;

// ========================================
// DocumentVersion 响应 DTO
// ========================================

/// <summary>
/// 文管文档版本子实体
/// 对应前端 TaktDocumentVersionDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktDocumentVersionDto : TaktCompanyDtoBase
{
    /// <summary>
    /// DocumentVersionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentVersionId { get; set; }

    /// <summary>
    /// 文档 ID（选项 TaktDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 文档 名称（填充字段）
    /// </summary>
    public string? DocumentName { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int VersionNo { get; set; } = 0;

    /// <summary>
    /// 版本说明
    /// </summary>
    public string? VersionNote { get; set; } = string.Empty;

    /// <summary>
    /// 文件 ID（选项 TaktFiles/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件类型（MIME）
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 修订人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisedBy { get; set; }

    /// <summary>
    /// 修订人姓名
    /// </summary>
    public string? RevisedByName { get; set; } = string.Empty;

    /// <summary>
    /// 修订时间
    /// </summary>
    public DateTime RevisedAt { get; set; }

    /// <summary>
    /// 文档（主表）
    /// （主表：TaktDocument）
    /// </summary>
    public TaktDocumentDto? Document { get; set; }

}

// ========================================
// DocumentVersion 查询 DTO
// ========================================

/// <summary>
/// DocumentVersion 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDocumentVersionQueryDto : TaktPagedQuery
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
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 文档 ID（选项 TaktDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DocumentId { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int? VersionNo { get; set; }

    /// <summary>
    /// 版本说明
    /// </summary>
    public string? VersionNote { get; set; } = string.Empty;

    /// <summary>
    /// 文件 ID（选项 TaktFiles/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 文件类型（MIME）
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 修订人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisedBy { get; set; }

    /// <summary>
    /// 修订人姓名
    /// </summary>
    public string? RevisedByName { get; set; } = string.Empty;

    /// <summary>
    /// 修订时间（范围查询-开始）
    /// </summary>
    public DateTime? RevisedAtStart { get; set; }

    /// <summary>
    /// 修订时间（范围查询-结束）
    /// </summary>
    public DateTime? RevisedAtEnd { get; set; }

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
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建DocumentVersion DTO
// ========================================

/// <summary>
/// 创建DocumentVersion DTO
/// </summary>
public class TaktDocumentVersionCreateDto
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 文档 ID（选项 TaktDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int VersionNo { get; set; } = 0;

    /// <summary>
    /// 版本说明
    /// </summary>
    public string? VersionNote { get; set; } = string.Empty;

    /// <summary>
    /// 文件 ID（选项 TaktFiles/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    [Required(ErrorMessage = "文件名称不能为空")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    [Required(ErrorMessage = "文件路径不能为空")]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件类型（MIME）
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 修订人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisedBy { get; set; }

    /// <summary>
    /// 修订人姓名
    /// </summary>
    public string? RevisedByName { get; set; } = string.Empty;

    /// <summary>
    /// 修订时间
    /// </summary>
    public DateTime RevisedAt { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新DocumentVersion DTO
// ========================================

/// <summary>
/// 更新DocumentVersion DTO
/// 继承 TaktDocumentVersionCreateDto，添加 DocumentVersionId 字段
/// </summary>
public class TaktDocumentVersionUpdateDto : TaktDocumentVersionCreateDto
{
    /// <summary>
    /// DocumentVersionID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentVersionId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// DocumentVersion 导入模板行 DTO
/// </summary>
public class TaktDocumentVersionTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 文档 ID（选项 TaktDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DocumentId { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int? VersionNo { get; set; }

    /// <summary>
    /// 版本说明
    /// </summary>
    public string? VersionNote { get; set; } = string.Empty;

    /// <summary>
    /// 文件 ID（选项 TaktFiles/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 文件类型（MIME）
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 修订人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisedBy { get; set; }

    /// <summary>
    /// 修订人姓名
    /// </summary>
    public string? RevisedByName { get; set; } = string.Empty;

    /// <summary>
    /// 修订时间
    /// </summary>
    public DateTime? RevisedAt { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// DocumentVersion 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktDocumentVersionImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 文档 ID（选项 TaktDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DocumentId { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int? VersionNo { get; set; }

    /// <summary>
    /// 版本说明
    /// </summary>
    public string? VersionNote { get; set; } = string.Empty;

    /// <summary>
    /// 文件 ID（选项 TaktFiles/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 文件类型（MIME）
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 修订人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisedBy { get; set; }

    /// <summary>
    /// 修订人姓名
    /// </summary>
    public string? RevisedByName { get; set; } = string.Empty;

    /// <summary>
    /// 修订时间
    /// </summary>
    public DateTime? RevisedAt { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// DocumentVersion 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDocumentVersionExportDto
{
    /// <summary>
    /// DocumentVersionID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentVersionId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档 ID（选项 TaktDocuments/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public int VersionNo { get; set; } = 0;

    /// <summary>
    /// 版本说明
    /// </summary>
    public string? VersionNote { get; set; } = string.Empty;

    /// <summary>
    /// 文件 ID（选项 TaktFiles/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileId { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件类型（MIME）
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 修订人 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisedBy { get; set; }

    /// <summary>
    /// 修订人姓名
    /// </summary>
    public string? RevisedByName { get; set; } = string.Empty;

    /// <summary>
    /// 修订时间
    /// </summary>
    public DateTime RevisedAt { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
