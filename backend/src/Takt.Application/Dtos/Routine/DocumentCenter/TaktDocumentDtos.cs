// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Routine.DocumentCenter
// 文件名称：TaktDocumentDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：Document 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktDocument 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Routine.DocumentCenter;

// ========================================
// Document 响应 DTO
// ========================================

/// <summary>
/// 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制；需审批通过后发布（草稿→审批→发布）
/// 对应前端 TaktDocumentDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktDocumentDto : TaktApprovalDtoBase
{
    /// <summary>
    /// DocumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 文档编码（租户+公司内唯一）
    /// </summary>
    public string DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类
    /// </summary>
    public TaktDocumentCategory DocumentCategory { get; set; }

    /// <summary>
    /// 文档状态
    /// </summary>
    public TaktDocumentStatus DocumentStatus { get; set; }

    /// <summary>
    /// 密级
    /// </summary>
    public TaktDocumentConfidentialLevel ConfidentialLevel { get; set; }

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 当前文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 当前文件类型（MIME）
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? EffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 归属部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 归属部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 是否置顶
    /// </summary>
    public TaktYesNo IsTop { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int DownloadCount { get; set; } = 0;

    /// <summary>
    /// 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
    /// </summary>
    public string TargetScope { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门编码（多个用逗号分隔）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户 ID（多个用逗号分隔）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 版本历史列表（主子表关系）
    /// （子表：TaktDocumentVersion）
    /// </summary>
    public List<TaktDocumentVersionDto>? Versions { get; set; }

    /// <summary>
    /// 变更日志列表（主子表关系）
    /// （子表：TaktDocumentChangeLog）
    /// </summary>
    public List<TaktDocumentChangeLogDto>? ChangeLogs { get; set; }

}

// ========================================
// Document 查询 DTO
// ========================================

/// <summary>
/// Document 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktDocumentQueryDto : TaktPagedQuery
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
    /// 文档编码（租户+公司内唯一）
    /// </summary>
    public string? DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    public string? Title { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类
    /// </summary>
    public TaktDocumentCategory? DocumentCategory { get; set; }

    /// <summary>
    /// 文档状态
    /// </summary>
    public TaktDocumentStatus? DocumentStatus { get; set; }

    /// <summary>
    /// 密级
    /// </summary>
    public TaktDocumentConfidentialLevel? ConfidentialLevel { get; set; }

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 当前文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileSize { get; set; }

    /// <summary>
    /// 当前文件类型（MIME）
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 生效时间（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveTimeStart { get; set; }

    /// <summary>
    /// 生效时间（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveTimeEnd { get; set; }

    /// <summary>
    /// 失效时间（范围查询-开始）
    /// </summary>
    public DateTime? ExpireTimeStart { get; set; }

    /// <summary>
    /// 失效时间（范围查询-结束）
    /// </summary>
    public DateTime? ExpireTimeEnd { get; set; }

    /// <summary>
    /// 发布时间（范围查询-开始）
    /// </summary>
    public DateTime? PublishTimeStart { get; set; }

    /// <summary>
    /// 发布时间（范围查询-结束）
    /// </summary>
    public DateTime? PublishTimeEnd { get; set; }

    /// <summary>
    /// 发布人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    public string? PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 归属部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 归属部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 是否置顶
    /// </summary>
    public TaktYesNo? IsTop { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int? ViewCount { get; set; }

    /// <summary>
    /// 下载次数
    /// </summary>
    public int? DownloadCount { get; set; }

    /// <summary>
    /// 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
    /// </summary>
    public string? TargetScope { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门编码（多个用逗号分隔）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户 ID（多个用逗号分隔）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 审批状态（TaktApprovalStatus）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

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
// 创建Document DTO
// ========================================

/// <summary>
/// 创建Document DTO
/// </summary>
public class TaktDocumentCreateDto
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
    /// 文档编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "文档编码（租户+公司内唯一）不能为空")]
    public string DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    [Required(ErrorMessage = "文档标题不能为空")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类
    /// </summary>
    public TaktDocumentCategory DocumentCategory { get; set; }

    /// <summary>
    /// 文档状态
    /// </summary>
    public TaktDocumentStatus DocumentStatus { get; set; }

    /// <summary>
    /// 密级
    /// </summary>
    public TaktDocumentConfidentialLevel ConfidentialLevel { get; set; }

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 当前文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 当前文件类型（MIME）
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? EffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    [Required(ErrorMessage = "发布人姓名不能为空")]
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 归属部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 归属部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 是否置顶
    /// </summary>
    public TaktYesNo IsTop { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int DownloadCount { get; set; } = 0;

    /// <summary>
    /// 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
    /// </summary>
    [Required(ErrorMessage = "目标范围（all=全员，company=本公司，department=本部门，custom=自定义）不能为空")]
    public string TargetScope { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门编码（多个用逗号分隔）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户 ID（多个用逗号分隔）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

    /// <summary>
    /// 版本历史列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktDocumentVersionCreateDto>? Versions { get; set; }

    /// <summary>
    /// 变更日志列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktDocumentChangeLogCreateDto>? ChangeLogs { get; set; }

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
// 更新Document DTO
// ========================================

/// <summary>
/// 更新Document DTO
/// 继承 TaktDocumentCreateDto，添加 DocumentId 字段
/// </summary>
public class TaktDocumentUpdateDto : TaktDocumentCreateDto
{
    /// <summary>
    /// DocumentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

}

// ========================================
// Document 状态 DTO
// ========================================

/// <summary>
/// Document 状态更新 DTO
/// </summary>
public class TaktDocumentStatusDto
{
    /// <summary>
    /// DocumentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 文档状态
    /// </summary>
    [Required(ErrorMessage = "文档状态不能为空")]
    public TaktDocumentStatus DocumentStatus { get; set; }
}

// ========================================
// Document 排序 DTO
// ========================================

/// <summary>
/// Document 排序更新 DTO
/// </summary>
public class TaktDocumentSortDto
{
    /// <summary>
    /// DocumentID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [Required(ErrorMessage = "排序号不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Document 导入模板行 DTO
/// </summary>
public class TaktDocumentTemplateDto
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
    /// 文档编码（租户+公司内唯一）
    /// </summary>
    public string? DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    public string? Title { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类
    /// </summary>
    public TaktDocumentCategory? DocumentCategory { get; set; }

    /// <summary>
    /// 文档状态
    /// </summary>
    public TaktDocumentStatus? DocumentStatus { get; set; }

    /// <summary>
    /// 密级
    /// </summary>
    public TaktDocumentConfidentialLevel? ConfidentialLevel { get; set; }

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 当前文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Document 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktDocumentImportDto
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
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 文档编码（租户+公司内唯一）
    /// </summary>
    public string? DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    public string? Title { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类
    /// </summary>
    public TaktDocumentCategory? DocumentCategory { get; set; }

    /// <summary>
    /// 文档状态
    /// </summary>
    public TaktDocumentStatus? DocumentStatus { get; set; }

    /// <summary>
    /// 密级
    /// </summary>
    public TaktDocumentConfidentialLevel? ConfidentialLevel { get; set; }

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int? Version { get; set; }

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 当前文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

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
// 导出 DTO
// ========================================

/// <summary>
/// Document 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktDocumentExportDto
{
    /// <summary>
    /// DocumentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long DocumentId { get; set; }

    /// <summary>
    /// 文档编码（租户+公司内唯一）
    /// </summary>
    public string DocumentCode { get; set; } = string.Empty;

    /// <summary>
    /// 文档标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 文档分类
    /// </summary>
    public TaktDocumentCategory DocumentCategory { get; set; }

    /// <summary>
    /// 文档状态
    /// </summary>
    public TaktDocumentStatus DocumentStatus { get; set; }

    /// <summary>
    /// 密级
    /// </summary>
    public TaktDocumentConfidentialLevel ConfidentialLevel { get; set; }

    /// <summary>
    /// 当前版本号
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// 文档内容（富文本 HTML）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 文档摘要（用于列表展示）
    /// </summary>
    public string? Summary { get; set; } = string.Empty;

    /// <summary>
    /// 标签（逗号分隔或 JSON 数组存储）
    /// </summary>
    public string? Tags { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FileId { get; set; }

    /// <summary>
    /// 当前文件名称
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件路径
    /// </summary>
    public string? FilePath { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件大小（字节）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FileSize { get; set; }

    /// <summary>
    /// 当前文件类型（MIME）
    /// </summary>
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 当前文件扩展名
    /// </summary>
    public string? FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// 生效时间
    /// </summary>
    public DateTime? EffectiveTime { get; set; }

    /// <summary>
    /// 失效时间
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 发布人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PublisherId { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    public string PublisherName { get; set; } = string.Empty;

    /// <summary>
    /// 归属部门 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeptId { get; set; }

    /// <summary>
    /// 归属部门名称
    /// </summary>
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 是否置顶
    /// </summary>
    public TaktYesNo IsTop { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 浏览次数
    /// </summary>
    public int ViewCount { get; set; } = 0;

    /// <summary>
    /// 下载次数
    /// </summary>
    public int DownloadCount { get; set; } = 0;

    /// <summary>
    /// 目标范围（all=全员，company=本公司，department=本部门，custom=自定义）
    /// </summary>
    public string TargetScope { get; set; } = string.Empty;

    /// <summary>
    /// 目标部门编码（多个用逗号分隔）
    /// </summary>
    public string? TargetDepartments { get; set; } = string.Empty;

    /// <summary>
    /// 目标用户 ID（多个用逗号分隔）
    /// </summary>
    public string? TargetUsers { get; set; } = string.Empty;

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
