// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktIsoCodeDtos.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Auto Generated)
// 功能描述：IsoCode 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktIsoCode 生成，请按需审阅）
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
// IsoCode 响应 DTO
// ========================================

/// <summary>
/// ISO 编码实体 维护租户内标准短码（如 Eng、Pmc、D1000），用于编码规则、单据编码等段引用
/// 对应前端 TaktIsoCodeDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktIsoCodeDto : TaktTenantDtoBase
{
    /// <summary>
    /// IsoCodeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IsoCodeId { get; set; }

    /// <summary>
    /// 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
    /// </summary>
    public int IsoCodeCategory { get; set; } = 0;

    /// <summary>
    /// ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
    /// </summary>
    public string IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// ISO 名称（如：技术、生管、总经理室）
    /// </summary>
    public string IsoName { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 描述说明
    /// </summary>
    public string? IsoCodeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int IsoCodeStatus { get; set; } = 0;

}

// ========================================
// IsoCode 查询 DTO
// ========================================

/// <summary>
/// IsoCode 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktIsoCodeQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;


    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;
    /// <summary>
    /// 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
    /// </summary>
    public int? IsoCodeCategory { get; set; }

    /// <summary>
    /// ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
    /// </summary>
    public string? IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// ISO 名称（如：技术、生管、总经理室）
    /// </summary>
    public string? IsoName { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 描述说明
    /// </summary>
    public string? IsoCodeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int? IsoCodeStatus { get; set; }

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
// 创建IsoCode DTO
// ========================================

/// <summary>
/// 创建IsoCode DTO
/// </summary>
public class TaktIsoCodeCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;


    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string RelatedPlant { get; set; } = string.Empty;
    /// <summary>
    /// 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
    /// </summary>
    public int IsoCodeCategory { get; set; } = 0;

    /// <summary>
    /// ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
    /// </summary>
    [Required(ErrorMessage = "ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）不能为空")]
    public string IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// ISO 名称（如：技术、生管、总经理室）
    /// </summary>
    [Required(ErrorMessage = "ISO 名称（如：技术、生管、总经理室）不能为空")]
    public string IsoName { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 描述说明
    /// </summary>
    public string? IsoCodeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int IsoCodeStatus { get; set; } = 0;

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
// 更新IsoCode DTO
// ========================================

/// <summary>
/// 更新IsoCode DTO
/// 继承 TaktIsoCodeCreateDto，添加 IsoCodeId 字段
/// </summary>
public class TaktIsoCodeUpdateDto : TaktIsoCodeCreateDto
{
    /// <summary>
    /// IsoCodeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IsoCodeId { get; set; }

}

// ========================================
// IsoCode 状态 DTO
// ========================================

/// <summary>
/// IsoCode 状态更新 DTO
/// </summary>
public class TaktIsoCodeStatusDto
{
    /// <summary>
    /// IsoCodeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IsoCodeId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable_status；1=启用 0=禁用）不能为空")]
    public int IsoCodeStatus { get; set; } = 0;
}

// ========================================
// IsoCode 排序 DTO
// ========================================

/// <summary>
/// IsoCode 排序更新 DTO
/// </summary>
public class TaktIsoCodeSortDto
{
    /// <summary>
    /// IsoCodeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IsoCodeId { get; set; }

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
/// IsoCode 导入模板行 DTO
/// </summary>
public class TaktIsoCodeTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;


    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;
    /// <summary>
    /// 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
    /// </summary>
    public int? IsoCodeCategory { get; set; }

    /// <summary>
    /// ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
    /// </summary>
    public string? IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// ISO 名称（如：技术、生管、总经理室）
    /// </summary>
    public string? IsoName { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 描述说明
    /// </summary>
    public string? IsoCodeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int? IsoCodeStatus { get; set; }

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
/// IsoCode 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktIsoCodeImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;


    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;
    /// <summary>
    /// 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
    /// </summary>
    public int? IsoCodeCategory { get; set; }

    /// <summary>
    /// ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
    /// </summary>
    public string? IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// ISO 名称（如：技术、生管、总经理室）
    /// </summary>
    public string? IsoName { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 描述说明
    /// </summary>
    public string? IsoCodeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int? IsoCodeStatus { get; set; }

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
/// IsoCode 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktIsoCodeExportDto
{
    /// <summary>
    /// IsoCodeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IsoCodeId { get; set; }

    /// <summary>
    /// 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
    /// </summary>
    public int IsoCodeCategory { get; set; } = 0;

    /// <summary>
    /// ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
    /// </summary>
    public string IsoCode { get; set; } = string.Empty;

    /// <summary>
    /// ISO 名称（如：技术、生管、总经理室）
    /// </summary>
    public string IsoName { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 描述说明
    /// </summary>
    public string? IsoCodeDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    public int IsoCodeStatus { get; set; } = 0;

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
