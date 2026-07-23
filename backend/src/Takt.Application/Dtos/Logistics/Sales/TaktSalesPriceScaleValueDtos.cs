// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesPriceScaleValueDtos.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPriceScaleValue 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesPriceScaleValue 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Sales;

// ========================================
// SalesPriceScaleValue 响应 DTO
// ========================================

/// <summary>
/// Takt销售价格价值等级实体（；主子表：TaktSalesPriceItem → ScaleValues；与数量等级仅差 ScaleValue↔ScaleQuantity）
/// 对应前端 TaktSalesPriceScaleValueDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesPriceScaleValueDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesPriceScaleValueID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleValueId { get; set; }

    /// <summary>
    /// 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

    /// <summary>
    /// 销售价格明细 名称（填充字段）
    /// </summary>
    public string? SalesPriceItemName { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
    /// </summary>
    public int SalesPriceSeq { get; set; } = 0;

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int SalesScaleSeq { get; set; } = 0;

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
// SalesPriceScaleValue 查询 DTO
// ========================================

/// <summary>
/// SalesPriceScaleValue 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesPriceScaleValueQueryDto : TaktPagedQuery
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
    /// 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
    /// </summary>
    public int? SalesPriceSeq { get; set; }

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? SalesScaleSeq { get; set; }

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
// 创建SalesPriceScaleValue DTO
// ========================================

/// <summary>
/// 创建SalesPriceScaleValue DTO
/// </summary>
public class TaktSalesPriceScaleValueCreateDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
    /// </summary>
    [Required(ErrorMessage = "定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）不能为空")]
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
    /// </summary>
    public int SalesPriceSeq { get; set; } = 0;

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int SalesScaleSeq { get; set; } = 0;

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
// 更新SalesPriceScaleValue DTO
// ========================================

/// <summary>
/// 更新SalesPriceScaleValue DTO
/// 继承 TaktSalesPriceScaleValueCreateDto，添加 SalesPriceScaleValueId 字段
/// </summary>
public class TaktSalesPriceScaleValueUpdateDto : TaktSalesPriceScaleValueCreateDto
{
    /// <summary>
    /// SalesPriceScaleValueID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleValueId { get; set; }

}

// ========================================
// SalesPriceScaleValue 作废 DTO
// ========================================

/// <summary>
/// SalesPriceScaleValue 作废/撤销作废 DTO
/// </summary>
public class TaktSalesPriceScaleValueObsoleteDto
{
    /// <summary>
    /// SalesPriceScaleValueID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleValueId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesPriceScaleValue 导入模板行 DTO
/// </summary>
public class TaktSalesPriceScaleValueTemplateDto
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
    /// 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
    /// </summary>
    public int? SalesPriceSeq { get; set; }

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? SalesScaleSeq { get; set; }

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
/// SalesPriceScaleValue 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesPriceScaleValueImportDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
    /// </summary>
    public int? SalesPriceSeq { get; set; }

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int? SalesScaleSeq { get; set; }

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
/// SalesPriceScaleValue 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesPriceScaleValueExportDto
{
    /// <summary>
    /// SalesPriceScaleValueID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleValueId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

    /// <summary>
    /// 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
    /// </summary>
    public int SalesPriceSeq { get; set; } = 0;

    /// <summary>
    /// 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
    /// </summary>
    public int SalesScaleSeq { get; set; } = 0;

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
