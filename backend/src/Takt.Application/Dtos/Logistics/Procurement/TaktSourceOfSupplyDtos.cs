// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktSourceOfSupplyDtos.cs
// 创建时间：2026-06-21
// 创建人：Takt365(Auto Generated)
// 功能描述：SourceOfSupply 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSourceOfSupply 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Procurement;

// ========================================
// SourceOfSupply 响应 DTO
// ========================================

/// <summary>
/// Takt货源实体（公司级；工厂+物料+供货商维度的有效货源记录）
/// 对应前端 TaktSourceOfSupplyDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSourceOfSupplyDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SourceOfSupplyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceOfSupplyId { get; set; }

    /// <summary>
    /// 工厂代码（关联 TaktPlant.PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 货源编码（租户+公司内唯一；业务单据号）
    /// </summary>
    public string SourceOfSupplyCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余字段，便于查询展示）
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称（冗余字段，便于查询展示）
    /// </summary>
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 采购组编码（关联 TaktPurchaseGroup.PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 是否固定（字典 sys_yes_no_type；1=是，0=否；固定货源，MRP/寻源优先选用）
    /// </summary>
    public int IsFixed { get; set; } = 0;

    /// <summary>
    /// 是否冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
    /// </summary>
    public int IsBlocked { get; set; } = 0;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 最小订购量
    /// </summary>
    public decimal MinimumOrderQuantity { get; set; }

    /// <summary>
    /// 计划交货天数（采购提前期）
    /// </summary>
    public int LeadTimeDays { get; set; } = 0;

    /// <summary>
    /// 框架协议号（采购合同/协议编号，可选）
    /// </summary>
    public string? AgreementNumber { get; set; } = string.Empty;

    /// <summary>
    /// 协议行号
    /// </summary>
    public int? AgreementLineNumber { get; set; }

    /// <summary>
    /// 货源状态（字典 sys_normal_disable_status；1=启用，0=禁用）
    /// </summary>
    public int SourceStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前；同物料多货源时的优先级）
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// SourceOfSupply 查询 DTO
// ========================================

/// <summary>
/// SourceOfSupply 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSourceOfSupplyQueryDto : TaktPagedQuery
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
    /// 工厂代码（关联 TaktPlant.PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 货源编码（租户+公司内唯一；业务单据号）
    /// </summary>
    public string? SourceOfSupplyCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余字段，便于查询展示）
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称（冗余字段，便于查询展示）
    /// </summary>
    public string? SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 采购组编码（关联 TaktPurchaseGroup.PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidFromStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidFromEnd { get; set; }

    /// <summary>
    /// 失效日期（范围查询-开始）
    /// </summary>
    public DateTime? ValidToStart { get; set; }

    /// <summary>
    /// 失效日期（范围查询-结束）
    /// </summary>
    public DateTime? ValidToEnd { get; set; }

    /// <summary>
    /// 是否固定（字典 sys_yes_no_type；1=是，0=否；固定货源，MRP/寻源优先选用）
    /// </summary>
    public int? IsFixed { get; set; }

    /// <summary>
    /// 是否冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
    /// </summary>
    public int? IsBlocked { get; set; }

    /// <summary>
    /// 采购单位
    /// </summary>
    public string? PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 最小订购量
    /// </summary>
    public decimal? MinimumOrderQuantity { get; set; }

    /// <summary>
    /// 计划交货天数（采购提前期）
    /// </summary>
    public int? LeadTimeDays { get; set; }

    /// <summary>
    /// 框架协议号（采购合同/协议编号，可选）
    /// </summary>
    public string? AgreementNumber { get; set; } = string.Empty;

    /// <summary>
    /// 协议行号
    /// </summary>
    public int? AgreementLineNumber { get; set; }

    /// <summary>
    /// 货源状态（字典 sys_normal_disable_status；1=启用，0=禁用）
    /// </summary>
    public int? SourceStatus { get; set; }

    /// <summary>
    /// 排序号（越小越靠前；同物料多货源时的优先级）
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
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建SourceOfSupply DTO
// ========================================

/// <summary>
/// 创建SourceOfSupply DTO
/// </summary>
public class TaktSourceOfSupplyCreateDto
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
    /// 工厂代码（关联 TaktPlant.PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（关联 TaktPlant.PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 货源编码（租户+公司内唯一；业务单据号）
    /// </summary>
    [Required(ErrorMessage = "货源编码（租户+公司内唯一；业务单据号）不能为空")]
    public string SourceOfSupplyCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（关联 TaktMaterialPlant.MaterialCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余字段，便于查询展示）
    /// </summary>
    [Required(ErrorMessage = "物料名称（冗余字段，便于查询展示）不能为空")]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    [Required(ErrorMessage = "供货商编码（关联 TaktSupplier.SupplierCode）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称（冗余字段，便于查询展示）
    /// </summary>
    [Required(ErrorMessage = "供货商名称（冗余字段，便于查询展示）不能为空")]
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 采购组编码（关联 TaktPurchaseGroup.PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 是否固定（字典 sys_yes_no_type；1=是，0=否；固定货源，MRP/寻源优先选用）
    /// </summary>
    public int IsFixed { get; set; } = 0;

    /// <summary>
    /// 是否冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
    /// </summary>
    public int IsBlocked { get; set; } = 0;

    /// <summary>
    /// 采购单位
    /// </summary>
    [Required(ErrorMessage = "采购单位不能为空")]
    public string PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 最小订购量
    /// </summary>
    public decimal MinimumOrderQuantity { get; set; }

    /// <summary>
    /// 计划交货天数（采购提前期）
    /// </summary>
    public int LeadTimeDays { get; set; } = 0;

    /// <summary>
    /// 框架协议号（采购合同/协议编号，可选）
    /// </summary>
    public string? AgreementNumber { get; set; } = string.Empty;

    /// <summary>
    /// 协议行号
    /// </summary>
    public int? AgreementLineNumber { get; set; }

    /// <summary>
    /// 货源状态（字典 sys_normal_disable_status；1=启用，0=禁用）
    /// </summary>
    public int SourceStatus { get; set; } = 0;

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
// 更新SourceOfSupply DTO
// ========================================

/// <summary>
/// 更新SourceOfSupply DTO
/// 继承 TaktSourceOfSupplyCreateDto，添加 SourceOfSupplyId 字段
/// </summary>
public class TaktSourceOfSupplyUpdateDto : TaktSourceOfSupplyCreateDto
{
    /// <summary>
    /// SourceOfSupplyID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceOfSupplyId { get; set; }

}

// ========================================
// SourceOfSupply 状态 DTO
// ========================================

/// <summary>
/// SourceOfSupply 状态更新 DTO
/// </summary>
public class TaktSourceOfSupplyStatusDto
{
    /// <summary>
    /// SourceOfSupplyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceOfSupplyId { get; set; }

    /// <summary>
    /// 货源状态（字典 sys_normal_disable_status；1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "货源状态（字典 sys_normal_disable_status；1=启用，0=禁用）不能为空")]
    public int SourceStatus { get; set; } = 0;
}

// ========================================
// SourceOfSupply 排序 DTO
// ========================================

/// <summary>
/// SourceOfSupply 排序更新 DTO
/// </summary>
public class TaktSourceOfSupplySortDto
{
    /// <summary>
    /// SourceOfSupplyID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceOfSupplyId { get; set; }

    /// <summary>
    /// 排序号（越小越靠前；同物料多货源时的优先级）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前；同物料多货源时的优先级）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SourceOfSupply 导入模板行 DTO
/// </summary>
public class TaktSourceOfSupplyTemplateDto
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
    /// 工厂代码（关联 TaktPlant.PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 货源编码（租户+公司内唯一；业务单据号）
    /// </summary>
    public string? SourceOfSupplyCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余字段，便于查询展示）
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称（冗余字段，便于查询展示）
    /// </summary>
    public string? SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 采购组编码（关联 TaktPurchaseGroup.PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否固定（字典 sys_yes_no_type；1=是，0=否；固定货源，MRP/寻源优先选用）
    /// </summary>
    public int? IsFixed { get; set; }

    /// <summary>
    /// 是否冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
    /// </summary>
    public int? IsBlocked { get; set; }

    /// <summary>
    /// 采购单位
    /// </summary>
    public string? PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划交货天数（采购提前期）
    /// </summary>
    public int? LeadTimeDays { get; set; }

    /// <summary>
    /// 框架协议号（采购合同/协议编号，可选）
    /// </summary>
    public string? AgreementNumber { get; set; } = string.Empty;

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
/// SourceOfSupply 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSourceOfSupplyImportDto
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
    /// 工厂代码（关联 TaktPlant.PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 货源编码（租户+公司内唯一；业务单据号）
    /// </summary>
    public string? SourceOfSupplyCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余字段，便于查询展示）
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称（冗余字段，便于查询展示）
    /// </summary>
    public string? SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 采购组编码（关联 TaktPurchaseGroup.PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否固定（字典 sys_yes_no_type；1=是，0=否；固定货源，MRP/寻源优先选用）
    /// </summary>
    public int? IsFixed { get; set; }

    /// <summary>
    /// 是否冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
    /// </summary>
    public int? IsBlocked { get; set; }

    /// <summary>
    /// 采购单位
    /// </summary>
    public string? PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划交货天数（采购提前期）
    /// </summary>
    public int? LeadTimeDays { get; set; }

    /// <summary>
    /// 框架协议号（采购合同/协议编号，可选）
    /// </summary>
    public string? AgreementNumber { get; set; } = string.Empty;

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
/// SourceOfSupply 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSourceOfSupplyExportDto
{
    /// <summary>
    /// SourceOfSupplyID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceOfSupplyId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（关联 TaktPlant.PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 货源编码（租户+公司内唯一；业务单据号）
    /// </summary>
    public string SourceOfSupplyCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联 TaktMaterialPlant.MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（冗余字段，便于查询展示）
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（关联 TaktSupplier.SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商名称（冗余字段，便于查询展示）
    /// </summary>
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// 采购组编码（关联 TaktPurchaseGroup.PurchaseGroupCode）
    /// </summary>
    public string? PurchaseGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    public DateTime ValidTo { get; set; }

    /// <summary>
    /// 是否固定（字典 sys_yes_no_type；1=是，0=否；固定货源，MRP/寻源优先选用）
    /// </summary>
    public int IsFixed { get; set; } = 0;

    /// <summary>
    /// 是否冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
    /// </summary>
    public int IsBlocked { get; set; } = 0;

    /// <summary>
    /// 采购单位
    /// </summary>
    public string PurchaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 最小订购量
    /// </summary>
    public decimal MinimumOrderQuantity { get; set; }

    /// <summary>
    /// 计划交货天数（采购提前期）
    /// </summary>
    public int LeadTimeDays { get; set; } = 0;

    /// <summary>
    /// 框架协议号（采购合同/协议编号，可选）
    /// </summary>
    public string? AgreementNumber { get; set; } = string.Empty;

    /// <summary>
    /// 协议行号
    /// </summary>
    public int? AgreementLineNumber { get; set; }

    /// <summary>
    /// 货源状态（字典 sys_normal_disable_status；1=启用，0=禁用）
    /// </summary>
    public int SourceStatus { get; set; } = 0;

    /// <summary>
    /// 排序号（越小越靠前；同物料多货源时的优先级）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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
