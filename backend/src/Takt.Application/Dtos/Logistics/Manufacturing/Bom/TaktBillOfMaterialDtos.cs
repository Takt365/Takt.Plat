// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：BillOfMaterial 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBillOfMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// BillOfMaterial 响应 DTO
// ========================================

/// <summary>
/// Takt物料清单实体（规范化：每个父件在工厂下维护一张BOM抬头，多层结构通过子件物料递归关联其自身BOM实现）
/// 对应前端 TaktBillOfMaterialDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBillOfMaterialDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BillOfMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM编码（业务单据号，便于检索，非唯一键）
    /// </summary>
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM名称
    /// </summary>
    public string BomName { get; set; } = string.Empty;

    /// <summary>
    /// 父物料ID（成品/半成品，关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentMaterialId { get; set; }

    /// <summary>
    /// 父物料编码（父项物料编码 item_code，冗余）
    /// </summary>
    public string ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 父物料名称（冗余）
    /// </summary>
    public string ParentMaterialName { get; set; } = string.Empty;

    /// <summary>
    /// BOM版本号
    /// </summary>
    public string BomVersion { get; set; } = string.Empty;

    /// <summary>
    /// BOM类型/用途（0=标准BOM，1=工程BOM，2=制造BOM，3=成本BOM，4=销售BOM，对应SAP BOM Usage）
    /// </summary>
    public int BomType { get; set; } = 0;

    /// <summary>
    /// 备选BOM编号（对应SAP Alternative BOM，如01/02）
    /// </summary>
    public string AlternativeBomNumber { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 父物料单位
    /// </summary>
    public string ParentMaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本数量（BOM基数，对应SAP Base quantity）
    /// </summary>
    public decimal ParentMaterialQuantity { get; set; }

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int IsEnabled { get; set; } = 0;

    /// <summary>
    /// BOM状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int BomStatus { get; set; } = 0;

    /// <summary>
    /// BOM描述
    /// </summary>
    public string? BomDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）
    /// （子表：TaktBillOfMaterialItem）
    /// </summary>
    public List<TaktBillOfMaterialItemDto>? Items { get; set; }

    /// <summary>
    /// BOM变更记录列表（外键在子表 TaktBillOfMaterialChangeLog.BillOfMaterialId）
    /// （子表：TaktBillOfMaterialChangeLog）
    /// </summary>
    public List<TaktBillOfMaterialChangeLogDto>? ChangeLogs { get; set; }

}

// ========================================
// BillOfMaterial 查询 DTO
// ========================================

/// <summary>
/// BillOfMaterial 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBillOfMaterialQueryDto : TaktPagedQuery
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM编码（业务单据号，便于检索，非唯一键）
    /// </summary>
    public string? BomCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM名称
    /// </summary>
    public string? BomName { get; set; } = string.Empty;

    /// <summary>
    /// 父物料ID（成品/半成品，关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentMaterialId { get; set; }

    /// <summary>
    /// 父物料编码（父项物料编码 item_code，冗余）
    /// </summary>
    public string? ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 父物料名称（冗余）
    /// </summary>
    public string? ParentMaterialName { get; set; } = string.Empty;

    /// <summary>
    /// BOM版本号
    /// </summary>
    public string? BomVersion { get; set; } = string.Empty;

    /// <summary>
    /// BOM类型/用途（0=标准BOM，1=工程BOM，2=制造BOM，3=成本BOM，4=销售BOM，对应SAP BOM Usage）
    /// </summary>
    public int? BomType { get; set; }

    /// <summary>
    /// 备选BOM编号（对应SAP Alternative BOM，如01/02）
    /// </summary>
    public string? AlternativeBomNumber { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期（范围查询-开始）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围查询-结束）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）（范围查询-开始）
    /// </summary>
    public DateTime? ExpiryDateStart { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）（范围查询-结束）
    /// </summary>
    public DateTime? ExpiryDateEnd { get; set; }

    /// <summary>
    /// 父物料单位
    /// </summary>
    public string? ParentMaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本数量（BOM基数，对应SAP Base quantity）
    /// </summary>
    public decimal? ParentMaterialQuantity { get; set; }

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int? IsEnabled { get; set; }

    /// <summary>
    /// BOM状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int? BomStatus { get; set; }

    /// <summary>
    /// BOM描述
    /// </summary>
    public string? BomDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
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
// 创建BillOfMaterial DTO
// ========================================

/// <summary>
/// 创建BillOfMaterial DTO
/// </summary>
public class TaktBillOfMaterialCreateDto
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
    /// 工厂代码
    /// </summary>
    [Required(ErrorMessage = "工厂代码不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM编码（业务单据号，便于检索，非唯一键）
    /// </summary>
    [Required(ErrorMessage = "BOM编码（业务单据号，便于检索，非唯一键）不能为空")]
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM名称
    /// </summary>
    [Required(ErrorMessage = "BOM名称不能为空")]
    public string BomName { get; set; } = string.Empty;

    /// <summary>
    /// 父物料ID（成品/半成品，关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentMaterialId { get; set; }

    /// <summary>
    /// 父物料编码（父项物料编码 item_code，冗余）
    /// </summary>
    [Required(ErrorMessage = "父物料编码（父项物料编码 item_code，冗余）不能为空")]
    public string ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 父物料名称（冗余）
    /// </summary>
    [Required(ErrorMessage = "父物料名称（冗余）不能为空")]
    public string ParentMaterialName { get; set; } = string.Empty;

    /// <summary>
    /// BOM版本号
    /// </summary>
    [Required(ErrorMessage = "BOM版本号不能为空")]
    public string BomVersion { get; set; } = string.Empty;

    /// <summary>
    /// BOM类型/用途（0=标准BOM，1=工程BOM，2=制造BOM，3=成本BOM，4=销售BOM，对应SAP BOM Usage）
    /// </summary>
    public int BomType { get; set; } = 0;

    /// <summary>
    /// 备选BOM编号（对应SAP Alternative BOM，如01/02）
    /// </summary>
    [Required(ErrorMessage = "备选BOM编号（对应SAP Alternative BOM，如01/02）不能为空")]
    public string AlternativeBomNumber { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 父物料单位
    /// </summary>
    [Required(ErrorMessage = "父物料单位不能为空")]
    public string ParentMaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本数量（BOM基数，对应SAP Base quantity）
    /// </summary>
    public decimal ParentMaterialQuantity { get; set; }

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int IsEnabled { get; set; } = 0;

    /// <summary>
    /// BOM状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int BomStatus { get; set; } = 0;

    /// <summary>
    /// BOM描述
    /// </summary>
    public string? BomDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）（子表，级联保存）
    /// </summary>
    public List<TaktBillOfMaterialItemCreateDto>? Items { get; set; }

    /// <summary>
    /// BOM变更记录列表（外键在子表 TaktBillOfMaterialChangeLog.BillOfMaterialId）（子表，级联保存）
    /// </summary>
    public List<TaktBillOfMaterialChangeLogCreateDto>? ChangeLogs { get; set; }

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
// 更新BillOfMaterial DTO
// ========================================

/// <summary>
/// 更新BillOfMaterial DTO
/// 继承 TaktBillOfMaterialCreateDto，添加 BillOfMaterialId 字段
/// </summary>
public class TaktBillOfMaterialUpdateDto : TaktBillOfMaterialCreateDto
{
    /// <summary>
    /// BillOfMaterialID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

}

// ========================================
// BillOfMaterial 状态 DTO
// ========================================

/// <summary>
/// BillOfMaterial 状态更新 DTO
/// </summary>
public class TaktBillOfMaterialStatusDto
{
    /// <summary>
    /// BillOfMaterialID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    [Required(ErrorMessage = "BOM状态（0=草稿，1=已发布，2=已停用）不能为空")]
    public int BomStatus { get; set; } = 0;
}

// ========================================
// BillOfMaterial 排序 DTO
// ========================================

/// <summary>
/// BillOfMaterial 排序更新 DTO
/// </summary>
public class TaktBillOfMaterialSortDto
{
    /// <summary>
    /// BillOfMaterialID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// BillOfMaterial 导入模板行 DTO
/// </summary>
public class TaktBillOfMaterialTemplateDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM编码（业务单据号，便于检索，非唯一键）
    /// </summary>
    public string? BomCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM名称
    /// </summary>
    public string? BomName { get; set; } = string.Empty;

    /// <summary>
    /// 父物料ID（成品/半成品，关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentMaterialId { get; set; }

    /// <summary>
    /// 父物料编码（父项物料编码 item_code，冗余）
    /// </summary>
    public string? ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 父物料名称（冗余）
    /// </summary>
    public string? ParentMaterialName { get; set; } = string.Empty;

    /// <summary>
    /// BOM版本号
    /// </summary>
    public string? BomVersion { get; set; } = string.Empty;

    /// <summary>
    /// BOM类型/用途（0=标准BOM，1=工程BOM，2=制造BOM，3=成本BOM，4=销售BOM，对应SAP BOM Usage）
    /// </summary>
    public int? BomType { get; set; }

    /// <summary>
    /// 备选BOM编号（对应SAP Alternative BOM，如01/02）
    /// </summary>
    public string? AlternativeBomNumber { get; set; } = string.Empty;

    /// <summary>
    /// 父物料单位
    /// </summary>
    public string? ParentMaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int? IsEnabled { get; set; }

    /// <summary>
    /// BOM状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int? BomStatus { get; set; }

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
/// BillOfMaterial 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBillOfMaterialImportDto
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
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM编码（业务单据号，便于检索，非唯一键）
    /// </summary>
    public string? BomCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM名称
    /// </summary>
    public string? BomName { get; set; } = string.Empty;

    /// <summary>
    /// 父物料ID（成品/半成品，关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentMaterialId { get; set; }

    /// <summary>
    /// 父物料编码（父项物料编码 item_code，冗余）
    /// </summary>
    public string? ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 父物料名称（冗余）
    /// </summary>
    public string? ParentMaterialName { get; set; } = string.Empty;

    /// <summary>
    /// BOM版本号
    /// </summary>
    public string? BomVersion { get; set; } = string.Empty;

    /// <summary>
    /// BOM类型/用途（0=标准BOM，1=工程BOM，2=制造BOM，3=成本BOM，4=销售BOM，对应SAP BOM Usage）
    /// </summary>
    public int? BomType { get; set; }

    /// <summary>
    /// 备选BOM编号（对应SAP Alternative BOM，如01/02）
    /// </summary>
    public string? AlternativeBomNumber { get; set; } = string.Empty;

    /// <summary>
    /// 父物料单位
    /// </summary>
    public string? ParentMaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int? IsEnabled { get; set; }

    /// <summary>
    /// BOM状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int? BomStatus { get; set; }

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
/// BillOfMaterial 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBillOfMaterialExportDto
{
    /// <summary>
    /// BillOfMaterialID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM编码（业务单据号，便于检索，非唯一键）
    /// </summary>
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// BOM名称
    /// </summary>
    public string BomName { get; set; } = string.Empty;

    /// <summary>
    /// 父物料ID（成品/半成品，关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentMaterialId { get; set; }

    /// <summary>
    /// 父物料编码（父项物料编码 item_code，冗余）
    /// </summary>
    public string ParentMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 父物料名称（冗余）
    /// </summary>
    public string ParentMaterialName { get; set; } = string.Empty;

    /// <summary>
    /// BOM版本号
    /// </summary>
    public string BomVersion { get; set; } = string.Empty;

    /// <summary>
    /// BOM类型/用途（0=标准BOM，1=工程BOM，2=制造BOM，3=成本BOM，4=销售BOM，对应SAP BOM Usage）
    /// </summary>
    public int BomType { get; set; } = 0;

    /// <summary>
    /// 备选BOM编号（对应SAP Alternative BOM，如01/02）
    /// </summary>
    public string AlternativeBomNumber { get; set; } = string.Empty;

    /// <summary>
    /// 生效日期
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 父物料单位
    /// </summary>
    public string ParentMaterialUnit { get; set; } = string.Empty;

    /// <summary>
    /// 基本数量（BOM基数，对应SAP Base quantity）
    /// </summary>
    public decimal ParentMaterialQuantity { get; set; }

    /// <summary>
    /// 是否启用（0=否，1=是）
    /// </summary>
    public int IsEnabled { get; set; } = 0;

    /// <summary>
    /// BOM状态（0=草稿，1=已发布，2=已停用）
    /// </summary>
    public int BomStatus { get; set; } = 0;

    /// <summary>
    /// BOM描述
    /// </summary>
    public string? BomDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
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
