// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktPackagingDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：Packaging 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktPackaging 生成，请按需审阅）
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
// Packaging 响应 DTO
// ========================================

/// <summary>
/// Takt物料包装信息实体
/// 对应前端 TaktPackagingDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktPackagingDto : TaktCompanyDtoBase
{
    /// <summary>
    /// PackagingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PackagingId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联到物料表）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（用于关税和贸易统计）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（用于出口报关和贸易分析）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（包含包装物的总重量，单位：千克）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（不含包装物的净重量，单位：千克）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（如：KG、G、T等）
    /// </summary>
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务量/容积（一个包装单位的体积，单位：立方米）
    /// </summary>
    public decimal? BusinessVolume { get; set; }

    /// <summary>
    /// 体积单位（如：M3、L、ML等）
    /// </summary>
    public string VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 大小/量纲（尺寸量纲或大小规格）
    /// </summary>
    public string? SizeDimension { get; set; } = string.Empty;

    /// <summary>
    /// 包装类型（如：箱、托盘、袋、桶等，VERP=销售包装）
    /// </summary>
    public string PackagingType { get; set; } = string.Empty;

    /// <summary>
    /// 包装单位（CAR=卡通箱；其他如：个、件等）
    /// </summary>
    public string PackingUnit { get; set; } = string.Empty;

    /// <summary>
    /// 每包装数量（一个包装包含的基本单位数量）
    /// </summary>
    public decimal? QuantityPerPacking { get; set; }

    /// <summary>
    /// 包装规格
    /// </summary>
    public string? PackagingSpec { get; set; } = string.Empty;

    /// <summary>
    /// 包装描述
    /// </summary>
    public string? PackagingDescription { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// Packaging 查询 DTO
// ========================================

/// <summary>
/// Packaging 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktPackagingQueryDto : TaktPagedQuery
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
    /// 物料编码（关联到物料表）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（用于关税和贸易统计）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（用于出口报关和贸易分析）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（包含包装物的总重量，单位：千克）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（不含包装物的净重量，单位：千克）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（如：KG、G、T等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务量/容积（一个包装单位的体积，单位：立方米）
    /// </summary>
    public decimal? BusinessVolume { get; set; }

    /// <summary>
    /// 体积单位（如：M3、L、ML等）
    /// </summary>
    public string? VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 大小/量纲（尺寸量纲或大小规格）
    /// </summary>
    public string? SizeDimension { get; set; } = string.Empty;

    /// <summary>
    /// 包装类型（如：箱、托盘、袋、桶等，VERP=销售包装）
    /// </summary>
    public string? PackagingType { get; set; } = string.Empty;

    /// <summary>
    /// 包装单位（CAR=卡通箱；其他如：个、件等）
    /// </summary>
    public string? PackingUnit { get; set; } = string.Empty;

    /// <summary>
    /// 每包装数量（一个包装包含的基本单位数量）
    /// </summary>
    public decimal? QuantityPerPacking { get; set; }

    /// <summary>
    /// 包装规格
    /// </summary>
    public string? PackagingSpec { get; set; } = string.Empty;

    /// <summary>
    /// 包装描述
    /// </summary>
    public string? PackagingDescription { get; set; } = string.Empty;

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
// 创建Packaging DTO
// ========================================

/// <summary>
/// 创建Packaging DTO
/// </summary>
public class TaktPackagingCreateDto
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
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联到物料表）
    /// </summary>
    [Required(ErrorMessage = "物料编码（关联到物料表）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（用于关税和贸易统计）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（用于出口报关和贸易分析）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（包含包装物的总重量，单位：千克）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（不含包装物的净重量，单位：千克）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（如：KG、G、T等）
    /// </summary>
    [Required(ErrorMessage = "重量单位（如：KG、G、T等）不能为空")]
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务量/容积（一个包装单位的体积，单位：立方米）
    /// </summary>
    public decimal? BusinessVolume { get; set; }

    /// <summary>
    /// 体积单位（如：M3、L、ML等）
    /// </summary>
    [Required(ErrorMessage = "体积单位（如：M3、L、ML等）不能为空")]
    public string VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 大小/量纲（尺寸量纲或大小规格）
    /// </summary>
    public string? SizeDimension { get; set; } = string.Empty;

    /// <summary>
    /// 包装类型（如：箱、托盘、袋、桶等，VERP=销售包装）
    /// </summary>
    [Required(ErrorMessage = "包装类型（如：箱、托盘、袋、桶等，VERP=销售包装）不能为空")]
    public string PackagingType { get; set; } = string.Empty;

    /// <summary>
    /// 包装单位（CAR=卡通箱；其他如：个、件等）
    /// </summary>
    [Required(ErrorMessage = "包装单位（CAR=卡通箱；其他如：个、件等）不能为空")]
    public string PackingUnit { get; set; } = string.Empty;

    /// <summary>
    /// 每包装数量（一个包装包含的基本单位数量）
    /// </summary>
    public decimal? QuantityPerPacking { get; set; }

    /// <summary>
    /// 包装规格
    /// </summary>
    public string? PackagingSpec { get; set; } = string.Empty;

    /// <summary>
    /// 包装描述
    /// </summary>
    public string? PackagingDescription { get; set; } = string.Empty;

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

}

// ========================================
// 更新Packaging DTO
// ========================================

/// <summary>
/// 更新Packaging DTO
/// 继承 TaktPackagingCreateDto，添加 PackagingId 字段
/// </summary>
public class TaktPackagingUpdateDto : TaktPackagingCreateDto
{
    /// <summary>
    /// PackagingID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PackagingId { get; set; }

}

// ========================================
// Packaging 排序 DTO
// ========================================

/// <summary>
/// Packaging 排序更新 DTO
/// </summary>
public class TaktPackagingSortDto
{
    /// <summary>
    /// PackagingID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PackagingId { get; set; }

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
/// Packaging 导入模板行 DTO
/// </summary>
public class TaktPackagingTemplateDto
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
    /// 物料编码（关联到物料表）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（用于关税和贸易统计）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（用于出口报关和贸易分析）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 重量单位（如：KG、G、T等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

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
/// Packaging 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktPackagingImportDto
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
    /// 物料编码（关联到物料表）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（用于关税和贸易统计）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（用于出口报关和贸易分析）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 重量单位（如：KG、G、T等）
    /// </summary>
    public string? WeightUnit { get; set; } = string.Empty;

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
/// Packaging 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktPackagingExportDto
{
    /// <summary>
    /// PackagingID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PackagingId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（关联到物料表）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 海关商品编码（HS Code）
    /// </summary>
    public string? HsCode { get; set; } = string.Empty;

    /// <summary>
    /// 商品名称（HS Name）
    /// </summary>
    public string? HsName { get; set; } = string.Empty;

    /// <summary>
    /// 附加编码（如 CIQ 检验检疫附加码，3位，用于满足特定监管要求）
    /// </summary>
    public string? AdditionalCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区编码（用于关税和贸易统计）
    /// </summary>
    public string? OriginCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 原产国/地区名称
    /// </summary>
    public string? OriginCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区编码（用于出口报关和贸易分析）
    /// </summary>
    public string? DestinationCountryRegionCode { get; set; } = string.Empty;

    /// <summary>
    /// 目的国/地区名称
    /// </summary>
    public string? DestinationCountryRegionName { get; set; } = string.Empty;

    /// <summary>
    /// 监管条件代码（如是否需要商检、许可证等，用于触发特定业务流程）
    /// </summary>
    public string? RegulatoryConditionCode { get; set; } = string.Empty;

    /// <summary>
    /// 税率/协定税率标识（记录适用的关税税率类型，便于成本核算）
    /// </summary>
    public string? TariffRateType { get; set; } = string.Empty;

    /// <summary>
    /// 毛重（包含包装物的总重量，单位：千克）
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// 净重（不含包装物的净重量，单位：千克）
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// 重量单位（如：KG、G、T等）
    /// </summary>
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// 业务量/容积（一个包装单位的体积，单位：立方米）
    /// </summary>
    public decimal? BusinessVolume { get; set; }

    /// <summary>
    /// 体积单位（如：M3、L、ML等）
    /// </summary>
    public string VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// 大小/量纲（尺寸量纲或大小规格）
    /// </summary>
    public string? SizeDimension { get; set; } = string.Empty;

    /// <summary>
    /// 包装类型（如：箱、托盘、袋、桶等，VERP=销售包装）
    /// </summary>
    public string PackagingType { get; set; } = string.Empty;

    /// <summary>
    /// 包装单位（CAR=卡通箱；其他如：个、件等）
    /// </summary>
    public string PackingUnit { get; set; } = string.Empty;

    /// <summary>
    /// 每包装数量（一个包装包含的基本单位数量）
    /// </summary>
    public decimal? QuantityPerPacking { get; set; }

    /// <summary>
    /// 包装规格
    /// </summary>
    public string? PackagingSpec { get; set; } = string.Empty;

    /// <summary>
    /// 包装描述
    /// </summary>
    public string? PackagingDescription { get; set; } = string.Empty;

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
