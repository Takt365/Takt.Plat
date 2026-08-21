// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktPurchasePriceScaleQuantityDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：PurchasePriceScaleQuantity 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPurchasePriceScaleQuantity 生成，请按需审阅）
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
// PurchasePriceScaleQuantity 响应 DTO
// ========================================

/// <summary>
/// Takt采购价格数量等级实体（；主子表：TaktPurchasePriceItem → ScaleQuantities；与价值等级仅差 ScaleQuantity↔ScaleValue）
/// 对应前端 TaktPurchasePriceScaleQuantityDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPurchasePriceScaleQuantityDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PurchasePriceScaleQuantityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceScaleQuantityId { get; set; }

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
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int PurchaseScaleSeq { get; set; } = 0;

    /// <summary>
    /// 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal ScaleQuantity { get; set; }

    /// <summary>
    /// 价格
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
// PurchasePriceScaleQuantity 查询 DTO
// ========================================

/// <summary>
/// PurchasePriceScaleQuantity 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPurchasePriceScaleQuantityQueryDto : TaktPagedQuery
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
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? PurchaseScaleSeq { get; set; }

    /// <summary>
    /// 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 价格
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
// 创建PurchasePriceScaleQuantity DTO
// ========================================

/// <summary>
/// 创建PurchasePriceScaleQuantity DTO
/// </summary>
public class TaktPurchasePriceScaleQuantityCreateDto
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
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int PurchaseScaleSeq { get; set; } = 0;

    /// <summary>
    /// 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal ScaleQuantity { get; set; }

    /// <summary>
    /// 价格
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
// 更新PurchasePriceScaleQuantity DTO
// ========================================

/// <summary>
/// 更新PurchasePriceScaleQuantity DTO
/// 继承 TaktPurchasePriceScaleQuantityCreateDto，添加 PurchasePriceScaleQuantityId 字段
/// </summary>
public class TaktPurchasePriceScaleQuantityUpdateDto : TaktPurchasePriceScaleQuantityCreateDto
{
    /// <summary>
    /// PurchasePriceScaleQuantityID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceScaleQuantityId { get; set; }

}

// ========================================
// PurchasePriceScaleQuantity 作废 DTO
// ========================================

/// <summary>
/// PurchasePriceScaleQuantity 作废/撤销作废 DTO
/// </summary>
public class TaktPurchasePriceScaleQuantityObsoleteDto
{
    /// <summary>
    /// PurchasePriceScaleQuantityID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceScaleQuantityId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// PurchasePriceScaleQuantity 导入模板行 DTO
/// </summary>
public class TaktPurchasePriceScaleQuantityTemplateDto
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
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? PurchaseScaleSeq { get; set; }

    /// <summary>
    /// 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 价格
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
/// PurchasePriceScaleQuantity 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPurchasePriceScaleQuantityImportDto
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
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? PurchaseScaleSeq { get; set; }

    /// <summary>
    /// 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 价格
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
/// PurchasePriceScaleQuantity 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPurchasePriceScaleQuantityExportDto
{
    /// <summary>
    /// PurchasePriceScaleQuantityID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PurchasePriceScaleQuantityId { get; set; }

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
    /// 等级序号（回填：同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int PurchaseScaleSeq { get; set; } = 0;

    /// <summary>
    /// 等级数量（数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal ScaleQuantity { get; set; }

    /// <summary>
    /// 价格
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
