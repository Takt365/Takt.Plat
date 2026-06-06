// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialDtos.cs
// 创建时间：2026-06-06
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
/// Takt物料实体
/// 对应前端 TaktMaterialDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（唯一索引）
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
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 品目阶层
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 品目组代码
    /// </summary>
    public string? MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
    /// </summary>
    public int MaterialType { get; set; } = 0;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（主单位）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购组
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）
    /// </summary>
    public int PurchaseType { get; set; } = 0;

    /// <summary>
    /// 特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）
    /// </summary>
    public int SpecialProcurement { get; set; } = 0;

    /// <summary>
    /// 是否散装（0=否，1=是）
    /// </summary>
    public int IsBulk { get; set; } = 0;

    /// <summary>
    /// 最小起订量（基本单位数量）
    /// </summary>
    public decimal MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入）
    /// </summary>
    public decimal RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 自制生产天数（内部生产所需天数）
    /// </summary>
    public int InHouseProductionDays { get; set; } = 0;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件编号
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 币种代码
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格控制（0=标准价格，1=移动平均价格，2=其他）
    /// </summary>
    public int PriceControl { get; set; } = 0;

    /// <summary>
    /// 价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）
    /// </summary>
    public decimal PriceUnit { get; set; }

    /// <summary>
    /// 评估类别代码
    /// </summary>
    public string? ValuationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 差异码
    /// </summary>
    public string? DifferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心
    /// </summary>
    public string? ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// 最新采购价（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal LatestPurchasePrice { get; set; }

    /// <summary>
    /// 销售价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal SalesPrice { get; set; }

    /// <summary>
    /// 安全库存（基本单位数量）
    /// </summary>
    public decimal SafetyStock { get; set; }

    /// <summary>
    /// 最大库存（基本单位数量）
    /// </summary>
    public decimal MaxStock { get; set; }

    /// <summary>
    /// 最小库存（基本单位数量）
    /// </summary>
    public decimal MinStock { get; set; }

    /// <summary>
    /// 当前库存（基本单位数量）
    /// </summary>
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// 生产地点
    /// </summary>
    public string? ProductionLocation { get; set; } = string.Empty;

    /// <summary>
    /// 采购地点
    /// </summary>
    public string? PurchasingLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否检验（0=否，1=是）
    /// </summary>
    public int InspectionRequired { get; set; } = 0;

    /// <summary>
    /// 是否批次管理（0=否，1=是）
    /// </summary>
    public int IsBatch { get; set; } = 0;

    /// <summary>
    /// 是否保质期管理（0=否，1=是）
    /// </summary>
    public int IsExpiry { get; set; } = 0;

    /// <summary>
    /// 保质期天数（如果启用保质期管理）
    /// </summary>
    public int ExpiryDays { get; set; } = 0;

    /// <summary>
    /// 物料状态（1=启用，0=禁用）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;

    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    public string? MaterialAttributes { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）
    /// </summary>
    public string? IsEndOfLife { get; set; } = string.Empty;

    /// <summary>
    /// 停产日期
    /// </summary>
    public DateTime? EndOfLifeDate { get; set; }

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
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（唯一索引）
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
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 品目阶层
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 品目组代码
    /// </summary>
    public string? MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
    /// </summary>
    public int? MaterialType { get; set; }

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（主单位）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购组
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）
    /// </summary>
    public int? PurchaseType { get; set; }

    /// <summary>
    /// 特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）
    /// </summary>
    public int? SpecialProcurement { get; set; }

    /// <summary>
    /// 是否散装（0=否，1=是）
    /// </summary>
    public int? IsBulk { get; set; }

    /// <summary>
    /// 最小起订量（基本单位数量）
    /// </summary>
    public decimal? MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入）
    /// </summary>
    public decimal? RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数）
    /// </summary>
    public int? PlannedDeliveryTimeDays { get; set; }

    /// <summary>
    /// 自制生产天数（内部生产所需天数）
    /// </summary>
    public int? InHouseProductionDays { get; set; }

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件编号
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 币种代码
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格控制（0=标准价格，1=移动平均价格，2=其他）
    /// </summary>
    public int? PriceControl { get; set; }

    /// <summary>
    /// 价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）
    /// </summary>
    public decimal? PriceUnit { get; set; }

    /// <summary>
    /// 评估类别代码
    /// </summary>
    public string? ValuationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 差异码
    /// </summary>
    public string? DifferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心
    /// </summary>
    public string? ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// 最新采购价（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? LatestPurchasePrice { get; set; }

    /// <summary>
    /// 销售价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal? SalesPrice { get; set; }

    /// <summary>
    /// 安全库存（基本单位数量）
    /// </summary>
    public decimal? SafetyStock { get; set; }

    /// <summary>
    /// 最大库存（基本单位数量）
    /// </summary>
    public decimal? MaxStock { get; set; }

    /// <summary>
    /// 最小库存（基本单位数量）
    /// </summary>
    public decimal? MinStock { get; set; }

    /// <summary>
    /// 当前库存（基本单位数量）
    /// </summary>
    public decimal? CurrentStock { get; set; }

    /// <summary>
    /// 生产地点
    /// </summary>
    public string? ProductionLocation { get; set; } = string.Empty;

    /// <summary>
    /// 采购地点
    /// </summary>
    public string? PurchasingLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否检验（0=否，1=是）
    /// </summary>
    public int? InspectionRequired { get; set; }

    /// <summary>
    /// 是否批次管理（0=否，1=是）
    /// </summary>
    public int? IsBatch { get; set; }

    /// <summary>
    /// 是否保质期管理（0=否，1=是）
    /// </summary>
    public int? IsExpiry { get; set; }

    /// <summary>
    /// 保质期天数（如果启用保质期管理）
    /// </summary>
    public int? ExpiryDays { get; set; }

    /// <summary>
    /// 物料状态（1=启用，0=禁用）
    /// </summary>
    public int? MaterialStatus { get; set; }

    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    public string? MaterialAttributes { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）
    /// </summary>
    public string? IsEndOfLife { get; set; } = string.Empty;

    /// <summary>
    /// 停产日期（范围查询-开始）
    /// </summary>
    public DateTime? EndOfLifeDateStart { get; set; }

    /// <summary>
    /// 停产日期（范围查询-结束）
    /// </summary>
    public DateTime? EndOfLifeDateEnd { get; set; }

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
    /// 物料编码（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "物料编码（唯一索引）不能为空")]
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
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 品目阶层
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 品目组代码
    /// </summary>
    public string? MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
    /// </summary>
    public int MaterialType { get; set; } = 0;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（主单位）
    /// </summary>
    [Required(ErrorMessage = "基本单位（主单位）不能为空")]
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购组
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）
    /// </summary>
    public int PurchaseType { get; set; } = 0;

    /// <summary>
    /// 特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）
    /// </summary>
    public int SpecialProcurement { get; set; } = 0;

    /// <summary>
    /// 是否散装（0=否，1=是）
    /// </summary>
    public int IsBulk { get; set; } = 0;

    /// <summary>
    /// 最小起订量（基本单位数量）
    /// </summary>
    public decimal MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入）
    /// </summary>
    public decimal RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 自制生产天数（内部生产所需天数）
    /// </summary>
    public int InHouseProductionDays { get; set; } = 0;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件编号
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 币种代码
    /// </summary>
    [Required(ErrorMessage = "币种代码不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格控制（0=标准价格，1=移动平均价格，2=其他）
    /// </summary>
    public int PriceControl { get; set; } = 0;

    /// <summary>
    /// 价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）
    /// </summary>
    public decimal PriceUnit { get; set; }

    /// <summary>
    /// 评估类别代码
    /// </summary>
    public string? ValuationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 差异码
    /// </summary>
    public string? DifferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心
    /// </summary>
    public string? ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// 最新采购价（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal LatestPurchasePrice { get; set; }

    /// <summary>
    /// 销售价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal SalesPrice { get; set; }

    /// <summary>
    /// 安全库存（基本单位数量）
    /// </summary>
    public decimal SafetyStock { get; set; }

    /// <summary>
    /// 最大库存（基本单位数量）
    /// </summary>
    public decimal MaxStock { get; set; }

    /// <summary>
    /// 最小库存（基本单位数量）
    /// </summary>
    public decimal MinStock { get; set; }

    /// <summary>
    /// 当前库存（基本单位数量）
    /// </summary>
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// 生产地点
    /// </summary>
    public string? ProductionLocation { get; set; } = string.Empty;

    /// <summary>
    /// 采购地点
    /// </summary>
    public string? PurchasingLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否检验（0=否，1=是）
    /// </summary>
    public int InspectionRequired { get; set; } = 0;

    /// <summary>
    /// 是否批次管理（0=否，1=是）
    /// </summary>
    public int IsBatch { get; set; } = 0;

    /// <summary>
    /// 是否保质期管理（0=否，1=是）
    /// </summary>
    public int IsExpiry { get; set; } = 0;

    /// <summary>
    /// 保质期天数（如果启用保质期管理）
    /// </summary>
    public int ExpiryDays { get; set; } = 0;

    /// <summary>
    /// 物料状态（1=启用，0=禁用）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;

    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    public string? MaterialAttributes { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）
    /// </summary>
    public string? IsEndOfLife { get; set; } = string.Empty;

    /// <summary>
    /// 停产日期
    /// </summary>
    public DateTime? EndOfLifeDate { get; set; }

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
    /// 物料状态（1=启用，0=禁用）
    /// </summary>
    [Required(ErrorMessage = "物料状态（1=启用，0=禁用）不能为空")]
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
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（唯一索引）
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
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 品目阶层
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 品目组代码
    /// </summary>
    public string? MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
    /// </summary>
    public int? MaterialType { get; set; }

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（主单位）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

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
/// Material 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialImportDto
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
    /// 物料编码（唯一索引）
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
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 品目阶层
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 品目组代码
    /// </summary>
    public string? MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
    /// </summary>
    public int? MaterialType { get; set; }

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（主单位）
    /// </summary>
    public string? BaseUnit { get; set; } = string.Empty;

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
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（唯一索引）
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
    /// 行业领域
    /// </summary>
    public string? IndustrySector { get; set; } = string.Empty;

    /// <summary>
    /// 品目阶层
    /// </summary>
    public string? MaterialHierarchy { get; set; } = string.Empty;

    /// <summary>
    /// 品目组代码
    /// </summary>
    public string? MaterialGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（0=原材料，1=半成品，2=成品，3=辅料，4=包装材料，5=其他）
    /// </summary>
    public int MaterialType { get; set; } = 0;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料品牌
    /// </summary>
    public string? MaterialBrand { get; set; } = string.Empty;

    /// <summary>
    /// 基本单位（主单位）
    /// </summary>
    public string BaseUnit { get; set; } = string.Empty;

    /// <summary>
    /// 采购组
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（0=内部采购，1=外部采购，2=委外加工，3=其他）
    /// </summary>
    public int PurchaseType { get; set; } = 0;

    /// <summary>
    /// 特殊采购（0=标准采购，1=寄售，2=库存转移，3=其他）
    /// </summary>
    public int SpecialProcurement { get; set; } = 0;

    /// <summary>
    /// 是否散装（0=否，1=是）
    /// </summary>
    public int IsBulk { get; set; } = 0;

    /// <summary>
    /// 最小起订量（基本单位数量）
    /// </summary>
    public decimal MinOrderQuantity { get; set; }

    /// <summary>
    /// 舍入值（基本单位数量，用于数量舍入）
    /// </summary>
    public decimal RoundingValue { get; set; }

    /// <summary>
    /// 计划交货时间（天数）
    /// </summary>
    public int PlannedDeliveryTimeDays { get; set; } = 0;

    /// <summary>
    /// 自制生产天数（内部生产所需天数）
    /// </summary>
    public int InHouseProductionDays { get; set; } = 0;

    /// <summary>
    /// 制造商
    /// </summary>
    public string? Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 制造商零件编号
    /// </summary>
    public string? ManufacturerPartNumber { get; set; } = string.Empty;

    /// <summary>
    /// 币种代码
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 价格控制（0=标准价格，1=移动平均价格，2=其他）
    /// </summary>
    public int PriceControl { get; set; } = 0;

    /// <summary>
    /// 价格单位（价格对应的单位数量，如：1表示每1个，10表示每10个）
    /// </summary>
    public decimal PriceUnit { get; set; }

    /// <summary>
    /// 评估类别代码
    /// </summary>
    public string? ValuationCategory { get; set; } = string.Empty;

    /// <summary>
    /// 差异码
    /// </summary>
    public string? DifferenceCode { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心
    /// </summary>
    public string? ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// 最新采购价（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal LatestPurchasePrice { get; set; }

    /// <summary>
    /// 销售价格（精确到分，存储为整数，单位为分）
    /// </summary>
    public decimal SalesPrice { get; set; }

    /// <summary>
    /// 安全库存（基本单位数量）
    /// </summary>
    public decimal SafetyStock { get; set; }

    /// <summary>
    /// 最大库存（基本单位数量）
    /// </summary>
    public decimal MaxStock { get; set; }

    /// <summary>
    /// 最小库存（基本单位数量）
    /// </summary>
    public decimal MinStock { get; set; }

    /// <summary>
    /// 当前库存（基本单位数量）
    /// </summary>
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// 生产地点
    /// </summary>
    public string? ProductionLocation { get; set; } = string.Empty;

    /// <summary>
    /// 采购地点
    /// </summary>
    public string? PurchasingLocation { get; set; } = string.Empty;

    /// <summary>
    /// 是否检验（0=否，1=是）
    /// </summary>
    public int InspectionRequired { get; set; } = 0;

    /// <summary>
    /// 是否批次管理（0=否，1=是）
    /// </summary>
    public int IsBatch { get; set; } = 0;

    /// <summary>
    /// 是否保质期管理（0=否，1=是）
    /// </summary>
    public int IsExpiry { get; set; } = 0;

    /// <summary>
    /// 保质期天数（如果启用保质期管理）
    /// </summary>
    public int ExpiryDays { get; set; } = 0;

    /// <summary>
    /// 物料状态（1=启用，0=禁用）
    /// </summary>
    public int MaterialStatus { get; set; } = 0;

    /// <summary>
    /// 物料属性（JSON格式，存储物料自定义属性）
    /// </summary>
    public string? MaterialAttributes { get; set; } = string.Empty;

    /// <summary>
    /// 停产状态（EOL，End Of Life）（01=采购/仓库已锁定，02=任务清单/BOM已锁定，Z0=计划物料，ZM=当前库存需确认，ZP=制造中止，ZQ=生产结束（产品），ZW=PC MRP对象外，ZX=PC 中介专用品，ZY=PC 断开连接(MRP对象外)，ZZ=PC 有替代物料）
    /// </summary>
    public string? IsEndOfLife { get; set; } = string.Empty;

    /// <summary>
    /// 停产日期
    /// </summary>
    public DateTime? EndOfLifeDate { get; set; }

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
