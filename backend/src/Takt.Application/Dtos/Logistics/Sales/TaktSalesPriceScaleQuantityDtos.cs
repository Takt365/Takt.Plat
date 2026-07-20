// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesPriceScaleQuantityDtos.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPriceScaleQuantity 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesPriceScaleQuantity 生成，请按需审阅）
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
// SalesPriceScaleQuantity 响应 DTO
// ========================================

/// <summary>
/// Takt销售价格数量等级实体（SAP KONM；主子表：TaktSalesPriceItem → ScaleQuantities；与价值等级仅差 ScaleQuantity↔ScaleValue）
/// 对应前端 TaktSalesPriceScaleQuantityDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesPriceScaleQuantityDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesPriceScaleQuantityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleQuantityId { get; set; }

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
    /// 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
    /// </summary>
    public int SalesPriceSeq { get; set; } = 0;

    /// <summary>
    /// 行号（KLFN1；阶梯行序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal ScaleQuantity { get; set; }

    /// <summary>
    /// 金额（KBETR）
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

}

// ========================================
// SalesPriceScaleQuantity 查询 DTO
// ========================================

/// <summary>
/// SalesPriceScaleQuantity 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesPriceScaleQuantityQueryDto : TaktPagedQuery
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
    /// 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
    /// </summary>
    public int? SalesPriceSeq { get; set; }

    /// <summary>
    /// 行号（KLFN1；阶梯行序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 金额（KBETR）
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
// 创建SalesPriceScaleQuantity DTO
// ========================================

/// <summary>
/// 创建SalesPriceScaleQuantity DTO
/// </summary>
public class TaktSalesPriceScaleQuantityCreateDto
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
    /// 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
    /// </summary>
    public int SalesPriceSeq { get; set; } = 0;

    /// <summary>
    /// 行号（KLFN1；阶梯行序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal ScaleQuantity { get; set; }

    /// <summary>
    /// 金额（KBETR）
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
// 更新SalesPriceScaleQuantity DTO
// ========================================

/// <summary>
/// 更新SalesPriceScaleQuantity DTO
/// 继承 TaktSalesPriceScaleQuantityCreateDto，添加 SalesPriceScaleQuantityId 字段
/// </summary>
public class TaktSalesPriceScaleQuantityUpdateDto : TaktSalesPriceScaleQuantityCreateDto
{
    /// <summary>
    /// SalesPriceScaleQuantityID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleQuantityId { get; set; }

}

// ========================================
// SalesPriceScaleQuantity 作废 DTO
// ========================================

/// <summary>
/// SalesPriceScaleQuantity 作废/撤销作废 DTO
/// </summary>
public class TaktSalesPriceScaleQuantityObsoleteDto
{
    /// <summary>
    /// SalesPriceScaleQuantityID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleQuantityId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesPriceScaleQuantity 导入模板行 DTO
/// </summary>
public class TaktSalesPriceScaleQuantityTemplateDto
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
    /// 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
    /// </summary>
    public int? SalesPriceSeq { get; set; }

    /// <summary>
    /// 行号（KLFN1；阶梯行序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 金额（KBETR）
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
/// SalesPriceScaleQuantity 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesPriceScaleQuantityImportDto
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
    /// 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
    /// </summary>
    public int? SalesPriceSeq { get; set; }

    /// <summary>
    /// 行号（KLFN1；阶梯行序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 金额（KBETR）
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
/// SalesPriceScaleQuantity 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesPriceScaleQuantityExportDto
{
    /// <summary>
    /// SalesPriceScaleQuantityID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceScaleQuantityId { get; set; }

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
    /// 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
    /// </summary>
    public int SalesPriceSeq { get; set; } = 0;

    /// <summary>
    /// 行号（KLFN1；阶梯行序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
    /// </summary>
    public decimal ScaleQuantity { get; set; }

    /// <summary>
    /// 金额（KBETR）
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
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
