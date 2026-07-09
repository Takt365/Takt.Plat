// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialDtos.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：Material 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// Material 响应 DTO
// ========================================

/// <summary>
/// Takt全局物料实体（租户内共享主数据；工厂维度扩展见 TaktMaterialPlant）
/// 对应前端 TaktMaterialDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktMaterialDto : TaktTenantDtoBase
{
    /// <summary>
    /// MaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编号）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    public string? MaterialAttributes { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string IsEndOfLife { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;
}

// ========================================
// Material 查询 DTO
// ========================================

/// <summary>
/// Material 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编号）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    public string? MaterialAttributes { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string? IsEndOfLife { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? MaterialStatus { get; set; }

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
// 创建Material DTO
// ========================================

/// <summary>
/// 创建Material DTO
/// </summary>
public class TaktMaterialCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    [Required(ErrorMessage = "物料编码（租户内唯一）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    [Required(ErrorMessage = "物料名称不能为空")]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    [Required(ErrorMessage = "行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）不能为空")]
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
    /// </summary>
    [Required(ErrorMessage = "物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）不能为空")]
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    [Required(ErrorMessage = "物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）不能为空")]
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）不能为空")]
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编号）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    public string? MaterialAttributes { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    [Required(ErrorMessage = "停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）不能为空")]
    public string IsEndOfLife { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新Material DTO
// ========================================

/// <summary>
/// 更新Material DTO
/// 继承 TaktMaterialCreateDto，添加 MaterialId 字段
/// </summary>
public class TaktMaterialUpdateDto : TaktMaterialCreateDto
{
    /// <summary>
    /// MaterialID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

}

// ========================================
// Material 状态 DTO
// ========================================

/// <summary>
/// Material 状态更新 DTO
/// </summary>
public class TaktMaterialStatusDto
{
    /// <summary>
    /// MaterialID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    [Required(ErrorMessage = "物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）不能为空")]
    public int MaterialStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Material 导入模板行 DTO
/// </summary>
public class TaktMaterialTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编号）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    public string? MaterialAttributes { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string? IsEndOfLife { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? MaterialStatus { get; set; }    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Material 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编号）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    public string? MaterialAttributes { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string? IsEndOfLife { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int? MaterialStatus { get; set; }    /// <summary>
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
/// Material 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialExportDto
{
    /// <summary>
    /// MaterialID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料编码（租户内唯一）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 行业领域（字典 logistics_industry_sector；A=工厂工程/装备制造，C=化工，M=机械工程，P=制药/医药）
    /// </summary>
    public string IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 物料层级
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（关联 TaktMaterialGroup.MaterialGroupCode，选项 TaktMaterialGroups/options，DictValue=MaterialGroupCode）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_material_type，DictValue=ROH/HALB 等；默认 ROH）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编号）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    public string? MaterialAttributes { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（字典 logistics_material_eol_status，DictValue=01/Z0 等；默认 Z0=计划物料）
    /// </summary>
    public string IsEndOfLife { get; set; } = string.Empty;

    /// <summary>
    /// 物料状态（字典 sys_normal_disable_status；0=禁用，1=启用，2=锁定）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;

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
