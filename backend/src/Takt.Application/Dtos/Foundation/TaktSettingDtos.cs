// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktSettingDtos.cs
// 创建时间：2026-06-27
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
    public string? SettingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string SettingGroup { get; set; } = "frontend";

    /// <summary>
    /// 值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string ValueType { get; set; } = "input";

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 只读（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int IsReadonly { get; set; } = 0;

    /// <summary>
    /// 加密（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int IsEncrypted { get; set; } = 0;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int SettingStatus { get; set; } = 1;

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
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
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
    public string? SettingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string? SettingGroup { get; set; }

    /// <summary>
    /// 值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string? ValueType { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 只读（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? IsReadonly { get; set; }

    /// <summary>
    /// 加密（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? IsEncrypted { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int? SettingStatus { get; set; }

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
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;



    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
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
    public string? SettingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string SettingGroup { get; set; } = "frontend";

    /// <summary>
    /// 值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string ValueType { get; set; } = "input";

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 只读（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int IsReadonly { get; set; } = 0;

    /// <summary>
    /// 加密（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int IsEncrypted { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int SettingStatus { get; set; } = 1;

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
// Setting 状态 DTO
// ========================================

/// <summary>
/// Setting 状态更新 DTO
/// </summary>
public class TaktSettingStatusDto
{
    /// <summary>
    /// SettingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SettingId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public int SettingStatus { get; set; } = 1;
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
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
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
    public string? SettingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string? SettingGroup { get; set; }

    /// <summary>
    /// 值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string? ValueType { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 只读（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? IsReadonly { get; set; }

    /// <summary>
    /// 加密（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? IsEncrypted { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int? SettingStatus { get; set; }

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
/// Setting 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSettingImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;



    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
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
    public string? SettingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string? SettingGroup { get; set; }

    /// <summary>
    /// 值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string? ValueType { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 只读（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? IsReadonly { get; set; }

    /// <summary>
    /// 加密（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int? IsEncrypted { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int? SettingStatus { get; set; }

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
    public string? SettingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 设置类别（字典 sys_resource_type；frontend=前端 backend=后端）
    /// </summary>
    public string SettingGroup { get; set; } = "frontend";

    /// <summary>
    /// 值类型（字典 gen_display_type；input=文本框 select=下拉框 switch=开关 等）
    /// </summary>
    public string ValueType { get; set; } = "input";

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 只读（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int IsReadonly { get; set; } = 0;

    /// <summary>
    /// 加密（字典 sys_yes_no_type；0=否 1=是）
    /// </summary>
    public int IsEncrypted { get; set; } = 0;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int SettingStatus { get; set; } = 1;

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
