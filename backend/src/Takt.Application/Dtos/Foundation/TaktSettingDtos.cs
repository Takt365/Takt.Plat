// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktSettingDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：Setting 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSetting 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// Setting 响应 DTO
// ========================================

/// <summary>
/// 系统设置实体 存储系统的各种配置参数，支持租户级配置隔离
/// 对应前端 TaktSettingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSettingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SettingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SettingId { get; set; }

    /// <summary>
    /// 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    public string SettingKey { get; set; } = string.Empty;

    /// <summary>
    /// 设置值（字符串形式，复杂对象用JSON）
    /// </summary>
    public string? SettingValue { get; set; } = string.Empty;

    /// <summary>
    /// 设置名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    public string SettingName { get; set; } = string.Empty;

    /// <summary>
    /// 设置描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（0=前端，1=后端）
    /// </summary>
    public int SettingGroup { get; set; }

    /// <summary>
    /// 值类型（用于前端渲染不同的输入控件）
    /// </summary>
    public int ValueType { get; set; }

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 是否只读（0=否，1=是，只读设置不可修改）
    /// </summary>
    public int IsReadonly { get; set; }

    /// <summary>
    /// 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
    /// </summary>
    public int IsEncrypted { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// Setting 查询 DTO
// ========================================

/// <summary>
/// Setting 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSettingQueryDto : TaktPagedQuery
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
    /// 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    public string? SettingKey { get; set; } = string.Empty;

    /// <summary>
    /// 设置值（字符串形式，复杂对象用JSON）
    /// </summary>
    public string? SettingValue { get; set; } = string.Empty;

    /// <summary>
    /// 设置名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    public string? SettingName { get; set; } = string.Empty;

    /// <summary>
    /// 设置描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（0=前端，1=后端）
    /// </summary>
    public int? SettingGroup { get; set; }

    /// <summary>
    /// 值类型（用于前端渲染不同的输入控件）
    /// </summary>
    public int? ValueType { get; set; }

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 是否只读（0=否，1=是，只读设置不可修改）
    /// </summary>
    public int? IsReadonly { get; set; }

    /// <summary>
    /// 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
    /// </summary>
    public int? IsEncrypted { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建Setting DTO
// ========================================

/// <summary>
/// 创建Setting DTO
/// </summary>
public class TaktSettingCreateDto
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
    /// 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    [Required(ErrorMessage = "设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）不能为空")]
    public string SettingKey { get; set; } = string.Empty;

    /// <summary>
    /// 设置值（字符串形式，复杂对象用JSON）
    /// </summary>
    public string? SettingValue { get; set; } = string.Empty;

    /// <summary>
    /// 设置名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    [Required(ErrorMessage = "设置名称（显示名称，如：站点名称、最大上传大小）不能为空")]
    public string SettingName { get; set; } = string.Empty;

    /// <summary>
    /// 设置描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（0=前端，1=后端）
    /// </summary>
    public int SettingGroup { get; set; }

    /// <summary>
    /// 值类型（用于前端渲染不同的输入控件）
    /// </summary>
    public int ValueType { get; set; }

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 是否只读（0=否，1=是，只读设置不可修改）
    /// </summary>
    public int IsReadonly { get; set; }

    /// <summary>
    /// 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
    /// </summary>
    public int IsEncrypted { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
// 更新Setting DTO
// ========================================

/// <summary>
/// 更新Setting DTO
/// 继承 TaktSettingCreateDto，添加 SettingId 字段
/// </summary>
public class TaktSettingUpdateDto : TaktSettingCreateDto
{
    /// <summary>
    /// SettingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SettingId { get; set; }

}

// ========================================
// Setting 排序 DTO
// ========================================

/// <summary>
/// Setting 排序更新 DTO
/// </summary>
public class TaktSettingSortDto
{
    /// <summary>
    /// SettingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SettingId { get; set; }

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
/// Setting 导入模板行 DTO
/// </summary>
public class TaktSettingTemplateDto
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
    /// 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    public string? SettingKey { get; set; } = string.Empty;

    /// <summary>
    /// 设置值（字符串形式，复杂对象用JSON）
    /// </summary>
    public string? SettingValue { get; set; } = string.Empty;

    /// <summary>
    /// 设置名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    public string? SettingName { get; set; } = string.Empty;

    /// <summary>
    /// 设置描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（0=前端，1=后端）
    /// </summary>
    public int? SettingGroup { get; set; }

    /// <summary>
    /// 值类型（用于前端渲染不同的输入控件）
    /// </summary>
    public int? ValueType { get; set; }

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 是否只读（0=否，1=是，只读设置不可修改）
    /// </summary>
    public int? IsReadonly { get; set; }

    /// <summary>
    /// 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
    /// </summary>
    public int? IsEncrypted { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

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
/// Setting 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSettingImportDto
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
    /// 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    public string? SettingKey { get; set; } = string.Empty;

    /// <summary>
    /// 设置值（字符串形式，复杂对象用JSON）
    /// </summary>
    public string? SettingValue { get; set; } = string.Empty;

    /// <summary>
    /// 设置名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    public string? SettingName { get; set; } = string.Empty;

    /// <summary>
    /// 设置描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（0=前端，1=后端）
    /// </summary>
    public int? SettingGroup { get; set; }

    /// <summary>
    /// 值类型（用于前端渲染不同的输入控件）
    /// </summary>
    public int? ValueType { get; set; }

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 是否只读（0=否，1=是，只读设置不可修改）
    /// </summary>
    public int? IsReadonly { get; set; }

    /// <summary>
    /// 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
    /// </summary>
    public int? IsEncrypted { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

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
/// Setting 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSettingExportDto
{
    /// <summary>
    /// SettingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SettingId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 设置键（唯一索引：租户+公司内唯一，见 ix_setting_key_unique；如 system.siteName, upload.maxSize）
    /// </summary>
    public string SettingKey { get; set; } = string.Empty;

    /// <summary>
    /// 设置值（字符串形式，复杂对象用JSON）
    /// </summary>
    public string? SettingValue { get; set; } = string.Empty;

    /// <summary>
    /// 设置名称（显示名称，如：站点名称、最大上传大小）
    /// </summary>
    public string SettingName { get; set; } = string.Empty;

    /// <summary>
    /// 设置描述
    /// </summary>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（0=前端，1=后端）
    /// </summary>
    public int SettingGroup { get; set; }

    /// <summary>
    /// 值类型（用于前端渲染不同的输入控件）
    /// </summary>
    public int ValueType { get; set; }

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    public int IsBuiltIn { get; set; }

    /// <summary>
    /// 是否只读（0=否，1=是，只读设置不可修改）
    /// </summary>
    public int IsReadonly { get; set; }

    /// <summary>
    /// 是否加密存储（0=否，1=是，如密码、密钥等敏感信息）
    /// </summary>
    public int IsEncrypted { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
