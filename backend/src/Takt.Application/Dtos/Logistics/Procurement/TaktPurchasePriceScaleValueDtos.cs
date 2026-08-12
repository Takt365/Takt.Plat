// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchasePriceScaleValueDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchasePriceScaleValue 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchasePriceScaleValue 生成，请按需审阅）
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
// PurchasePriceScaleValue 响应 DTO
// ========================================

/// <summary>
/// Takt采购价格价值等级实体（；主子表：TaktPurchasePriceItem → ScaleValues；与数量等级仅差 ScaleValue↔ScaleQuantity）
/// 对应前端 TaktPurchasePriceScaleValueDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchasePriceScaleValueDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchasePriceScaleValueID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceScaleValueId { get; set; }

    /// <summary>
    /// 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceItemId { get; set; }

    /// <summary>
    /// 采购价格明细 名称（填充字段）
    /// </summary>
    public string? PurchasePriceItemName { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
    /// </summary>
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
    /// </summary>
    public int PurchasePriceSeq { get; set; } = 0;

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int PurchaseScaleSeq { get; set; } = 0;

    /// <summary>
    /// 等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal ScaleValue { get; set; }

    /// <summary>
    /// 价格（KBETR）
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

}

// ========================================
// PurchasePriceScaleValue 查询 DTO
// ========================================

/// <summary>
/// PurchasePriceScaleValue 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchasePriceScaleValueQueryDto : TaktPagedQuery
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
    /// 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
    /// </summary>
    public string? PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
    /// </summary>
    public int? PurchasePriceSeq { get; set; }

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? PurchaseScaleSeq { get; set; }

    /// <summary>
    /// 等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal? ScaleValue { get; set; }

    /// <summary>
    /// 价格（KBETR）
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建PurchasePriceScaleValue DTO
// ========================================

/// <summary>
/// 创建PurchasePriceScaleValue DTO
/// </summary>
public class TaktPurchasePriceScaleValueCreateDto
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
    /// 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
    /// </summary>
    [Required(ErrorMessage = "定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）不能为空")]
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
    /// </summary>
    public int PurchasePriceSeq { get; set; } = 0;

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int PurchaseScaleSeq { get; set; } = 0;

    /// <summary>
    /// 等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal ScaleValue { get; set; }

    /// <summary>
    /// 价格（KBETR）
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新PurchasePriceScaleValue DTO
// ========================================

/// <summary>
/// 更新PurchasePriceScaleValue DTO
/// 继承 TaktPurchasePriceScaleValueCreateDto，添加 PurchasePriceScaleValueId 字段
/// </summary>
public class TaktPurchasePriceScaleValueUpdateDto : TaktPurchasePriceScaleValueCreateDto
{
    /// <summary>
    /// PurchasePriceScaleValueID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceScaleValueId { get; set; }

}

// ========================================
// PurchasePriceScaleValue 作废 DTO
// ========================================

/// <summary>
/// PurchasePriceScaleValue 作废/撤销作废 DTO
/// </summary>
public class TaktPurchasePriceScaleValueObsoleteDto
{
    /// <summary>
    /// PurchasePriceScaleValueID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceScaleValueId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchasePriceScaleValue 导入模板行 DTO
/// </summary>
public class TaktPurchasePriceScaleValueTemplateDto
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
    /// 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
    /// </summary>
    public string? PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
    /// </summary>
    public int? PurchasePriceSeq { get; set; }

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? PurchaseScaleSeq { get; set; }

    /// <summary>
    /// 等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal? ScaleValue { get; set; }

    /// <summary>
    /// 价格（KBETR）
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// PurchasePriceScaleValue 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchasePriceScaleValueImportDto
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
    /// 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchasePriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
    /// </summary>
    public string? PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
    /// </summary>
    public int? PurchasePriceSeq { get; set; }

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? PurchaseScaleSeq { get; set; }

    /// <summary>
    /// 等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal? ScaleValue { get; set; }

    /// <summary>
    /// 价格（KBETR）
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal? TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// PurchasePriceScaleValue 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchasePriceScaleValueExportDto
{
    /// <summary>
    /// PurchasePriceScaleValueID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceScaleValueId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
    /// </summary>
    public string PurchasePriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
    /// </summary>
    public int PurchasePriceSeq { get; set; } = 0;

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int PurchaseScaleSeq { get; set; } = 0;

    /// <summary>
    /// 等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）
    /// </summary>
    public decimal ScaleValue { get; set; }

    /// <summary>
    /// 价格（KBETR）
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 未税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal UntaxedPrice { get; set; }

    /// <summary>
    /// 含税价格（冗余；可由 Price 与税码推算后回写）
    /// </summary>
    public decimal TaxIncludedPrice { get; set; }

    /// <summary>
    /// 税费（冗余；含税−未税，打印用）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
