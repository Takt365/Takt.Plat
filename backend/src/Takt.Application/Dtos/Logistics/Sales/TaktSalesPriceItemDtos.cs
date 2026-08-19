// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesPriceItemDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesPriceItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesPriceItem 生成，请按需审阅）
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
// SalesPriceItem 响应 DTO
// ========================================

/// <summary>
/// Takt销售价格明细实体（定价记录条件行；主子表：TaktSalesPrice → Items → ScaleQuantities / ScaleValues）
/// 对应前端 TaktSalesPriceItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesPriceItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesPriceItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

    /// <summary>
    /// 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

    /// <summary>
    /// 销售价格 名称（填充字段）
    /// </summary>
    public string? SalesPriceName { get; set; }

    /// <summary>
    /// 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（项号/序号，固定步长=10）
    /// </summary>
    public int SalesPriceSeq { get; set; } = 0;

    /// <summary>
    /// 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
    /// </summary>
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
    /// </summary>
    public string? ScaleType { get; set; } = string.Empty;

    /// <summary>
    /// 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
    /// </summary>
    public string? ScaleBasis { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal ScaleQuantity { get; set; }

    /// <summary>
    /// 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? ScaleUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级值
    /// </summary>
    public decimal ScaleValue { get; set; }

    /// <summary>
    /// 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? ScaleCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 计算类型（字典 logistics_calculation_type；默认 A=百分数）
    /// </summary>
    public string CalculationType { get; set; } = string.Empty;

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
    /// 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
    /// </summary>
    public string ConditionCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 最小起订量（计量单位数量，整数）
    /// </summary>
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int RoundingValue { get; set; } = 0;

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 数量等级行列表（；主子表关系）
    /// （子表：TaktSalesPriceScaleQuantity）
    /// </summary>
    public List<TaktSalesPriceScaleQuantityDto>? ScaleQuantities { get; set; }

    /// <summary>
    /// 价值等级行列表（；主子表关系）
    /// （子表：TaktSalesPriceScaleValue）
    /// </summary>
    public List<TaktSalesPriceScaleValueDto>? ScaleValues { get; set; }

}

// ========================================
// SalesPriceItem 查询 DTO
// ========================================

/// <summary>
/// SalesPriceItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesPriceItemQueryDto : TaktPagedQuery
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
    /// 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPriceId { get; set; }

    /// <summary>
    /// 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（项号/序号，固定步长=10）
    /// </summary>
    public int? SalesPriceSeq { get; set; }

    /// <summary>
    /// 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
    /// </summary>
    public string? PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
    /// </summary>
    public string? ScaleType { get; set; } = string.Empty;

    /// <summary>
    /// 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
    /// </summary>
    public string? ScaleBasis { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? ScaleUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级值
    /// </summary>
    public decimal? ScaleValue { get; set; }

    /// <summary>
    /// 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? ScaleCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 计算类型（字典 logistics_calculation_type；默认 A=百分数）
    /// </summary>
    public string? CalculationType { get; set; } = string.Empty;

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
    /// 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
    /// </summary>
    public string? ConditionCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 最小起订量（计量单位数量，整数）
    /// </summary>
    public int? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int? RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

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
// 创建SalesPriceItem DTO
// ========================================

/// <summary>
/// 创建SalesPriceItem DTO
/// </summary>
public class TaktSalesPriceItemCreateDto
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
    /// 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

    /// <summary>
    /// 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
    /// </summary>
    [Required(ErrorMessage = "定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）不能为空")]
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（项号/序号，固定步长=10）
    /// </summary>
    public int SalesPriceSeq { get; set; } = 0;

    /// <summary>
    /// 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
    /// </summary>
    [Required(ErrorMessage = "条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）不能为空")]
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
    /// </summary>
    public string? ScaleType { get; set; } = string.Empty;

    /// <summary>
    /// 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
    /// </summary>
    public string? ScaleBasis { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal ScaleQuantity { get; set; }

    /// <summary>
    /// 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? ScaleUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级值
    /// </summary>
    public decimal ScaleValue { get; set; }

    /// <summary>
    /// 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? ScaleCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 计算类型（字典 logistics_calculation_type；默认 A=百分数）
    /// </summary>
    [Required(ErrorMessage = "计算类型（字典 logistics_calculation_type；默认 A=百分数）不能为空")]
    public string CalculationType { get; set; } = string.Empty;

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
    /// 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
    /// </summary>
    [Required(ErrorMessage = "条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）不能为空")]
    public string ConditionCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）不能为空")]
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 最小起订量（计量单位数量，整数）
    /// </summary>
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int RoundingValue { get; set; } = 0;

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 数量等级行列表（；主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceScaleQuantityCreateDto>? ScaleQuantities { get; set; }

    /// <summary>
    /// 价值等级行列表（；主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceScaleValueCreateDto>? ScaleValues { get; set; }

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
// 更新SalesPriceItem DTO
// ========================================

/// <summary>
/// 更新SalesPriceItem DTO
/// 继承 TaktSalesPriceItemCreateDto，添加 SalesPriceItemId 字段
/// </summary>
public class TaktSalesPriceItemUpdateDto : TaktSalesPriceItemCreateDto
{
    /// <summary>
    /// SalesPriceItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

    /// <summary>
    /// 数量等级行列表（；主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktSalesPriceScaleQuantityUpdateDto>? ScaleQuantities { get; set; }

    /// <summary>
    /// 价值等级行列表（；主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktSalesPriceScaleValueUpdateDto>? ScaleValues { get; set; }

}

// ========================================
// SalesPriceItem 作废 DTO
// ========================================

/// <summary>
/// SalesPriceItem 作废/撤销作废 DTO
/// </summary>
public class TaktSalesPriceItemObsoleteDto
{
    /// <summary>
    /// SalesPriceItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesPriceItem 导入模板行 DTO
/// </summary>
public class TaktSalesPriceItemTemplateDto
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
    /// 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPriceId { get; set; }

    /// <summary>
    /// 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（项号/序号，固定步长=10）
    /// </summary>
    public int? SalesPriceSeq { get; set; }

    /// <summary>
    /// 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
    /// </summary>
    public string? PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
    /// </summary>
    public string? ScaleType { get; set; } = string.Empty;

    /// <summary>
    /// 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
    /// </summary>
    public string? ScaleBasis { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? ScaleUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级值
    /// </summary>
    public decimal? ScaleValue { get; set; }

    /// <summary>
    /// 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? ScaleCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 计算类型（字典 logistics_calculation_type；默认 A=百分数）
    /// </summary>
    public string? CalculationType { get; set; } = string.Empty;

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
    /// 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
    /// </summary>
    public string? ConditionCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 最小起订量（计量单位数量，整数）
    /// </summary>
    public int? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int? RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 数量等级行列表（；主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceScaleQuantityCreateDto>? ScaleQuantities { get; set; }

    /// <summary>
    /// 价值等级行列表（；主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceScaleValueCreateDto>? ScaleValues { get; set; }

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
/// SalesPriceItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesPriceItemImportDto
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
    /// 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesPriceId { get; set; }

    /// <summary>
    /// 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
    /// </summary>
    public string? SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（项号/序号，固定步长=10）
    /// </summary>
    public int? SalesPriceSeq { get; set; }

    /// <summary>
    /// 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
    /// </summary>
    public string? PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
    /// </summary>
    public string? ScaleType { get; set; } = string.Empty;

    /// <summary>
    /// 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
    /// </summary>
    public string? ScaleBasis { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal? ScaleQuantity { get; set; }

    /// <summary>
    /// 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? ScaleUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级值
    /// </summary>
    public decimal? ScaleValue { get; set; }

    /// <summary>
    /// 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? ScaleCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 计算类型（字典 logistics_calculation_type；默认 A=百分数）
    /// </summary>
    public string? CalculationType { get; set; } = string.Empty;

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
    /// 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
    /// </summary>
    public string? ConditionCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 最小起订量（计量单位数量，整数）
    /// </summary>
    public int? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int? RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

    /// <summary>
    /// 数量等级行列表（；主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceScaleQuantityCreateDto>? ScaleQuantities { get; set; }

    /// <summary>
    /// 价值等级行列表（；主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktSalesPriceScaleValueCreateDto>? ScaleValues { get; set; }

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
/// SalesPriceItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesPriceItemExportDto
{
    /// <summary>
    /// SalesPriceItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售价格 ID（主子表关系；选项 TaktSalesPrices/options，DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesPriceId { get; set; }

    /// <summary>
    /// 定价记录号（冗余；与主表 SalesPriceCode 一致，长度 20）
    /// </summary>
    public string SalesPriceCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价序号（项号/序号，固定步长=10）
    /// </summary>
    public int SalesPriceSeq { get; set; } = 0;

    /// <summary>
    /// 条件类型（冗余；字典 logistics_price_type；与主表 PriceType 一致，PB00/PR00/MWST/MWRK/NLXV）
    /// </summary>
    public string PriceType { get; set; } = string.Empty;

    /// <summary>
    /// 等级类型（字典 logistics_scale_type；A=基础等级，B=到等级，C=未使用，D=累进间隔等级）
    /// </summary>
    public string? ScaleType { get; set; } = string.Empty;

    /// <summary>
    /// 等级基础（字典 logistics_scale_basis；B=价值等级，C=数量规模，…）
    /// </summary>
    public string? ScaleBasis { get; set; } = string.Empty;

    /// <summary>
    /// 等级数量
    /// </summary>
    public decimal ScaleQuantity { get; set; }

    /// <summary>
    /// 等级单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等）
    /// </summary>
    public string? ScaleUnit { get; set; } = string.Empty;

    /// <summary>
    /// 等级值
    /// </summary>
    public decimal ScaleValue { get; set; }

    /// <summary>
    /// 等级货币（字典 accounting_currency_code；DictValue=CNY/USD 等）
    /// </summary>
    public string? ScaleCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 计算类型（字典 logistics_calculation_type；默认 A=百分数）
    /// </summary>
    public string CalculationType { get; set; } = string.Empty;

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
    /// 条件货币（字典 accounting_currency_code；DictValue=CNY/USD 等；默认 CNY）
    /// </summary>
    public string ConditionCurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 定价单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 计量单位（字典 logistics_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// 最小起订量（计量单位数量，整数）
    /// </summary>
    public int MinOrderQuantity { get; set; } = 0;

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入，整数）
    /// </summary>
    public int RoundingValue { get; set; } = 0;

    /// <summary>
    /// 计划交货时间（天数，整数）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

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
