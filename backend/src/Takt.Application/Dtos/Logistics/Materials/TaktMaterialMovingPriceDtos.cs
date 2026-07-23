// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialMovingPriceDtos.cs
// 创建时间：2026-07-16
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialMovingPrice 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialMovingPrice 生成，请按需审阅）
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
// MaterialMovingPrice 响应 DTO
// ========================================

/// <summary>
/// 移动价格实体 唯一键：租户 + 公司 + 工厂 + 期间 + 物料 + 评估类别（期间存当月首日表示年月）
/// 对应前端 TaktMaterialMovingPriceDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialMovingPriceDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialMovingPriceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialMovingPriceId { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（日期类型，业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    public DateTime PeriodDate { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode / TaktMaterialPlant.MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_price_control_type；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 币种（字典 accounting_currency_code，DictValue=CNY/USD 等）
    /// </summary>
    public string Currency { get; set; } = string.Empty;

}

// ========================================
// MaterialMovingPrice 查询 DTO
// ========================================

/// <summary>
/// MaterialMovingPrice 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialMovingPriceQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（日期类型，业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）（范围查询-开始）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 期间（日期类型，业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）（范围查询-结束）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode / TaktMaterialPlant.MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal? StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal? StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_price_control_type；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal? MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 币种（字典 accounting_currency_code，DictValue=CNY/USD 等）
    /// </summary>
    public string? Currency { get; set; } = string.Empty;

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
// 创建MaterialMovingPrice DTO
// ========================================

/// <summary>
/// 创建MaterialMovingPrice DTO
/// </summary>
public class TaktMaterialMovingPriceCreateDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（日期类型，业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    public DateTime PeriodDate { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode / TaktMaterialPlant.MaterialCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（关联 TaktMaterial.MaterialCode / TaktMaterialPlant.MaterialCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    [Required(ErrorMessage = "评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）不能为空")]
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_price_control_type；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    [Required(ErrorMessage = "价格控制（字典 logistics_price_control_type；S=标准价格，V=移动平均价格/周期单价；默认 V）不能为空")]
    public string PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 币种（字典 accounting_currency_code，DictValue=CNY/USD 等）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_currency_code，DictValue=CNY/USD 等）不能为空")]
    public string Currency { get; set; } = string.Empty;

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
// 更新MaterialMovingPrice DTO
// ========================================

/// <summary>
/// 更新MaterialMovingPrice DTO
/// 继承 TaktMaterialMovingPriceCreateDto，添加 MaterialMovingPriceId 字段
/// </summary>
public class TaktMaterialMovingPriceUpdateDto : TaktMaterialMovingPriceCreateDto
{
    /// <summary>
    /// MaterialMovingPriceID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialMovingPriceId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialMovingPrice 导入模板行 DTO
/// </summary>
public class TaktMaterialMovingPriceTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（日期类型，业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    public DateTime? PeriodDate { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode / TaktMaterialPlant.MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal? StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal? StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_price_control_type；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal? MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 币种（字典 accounting_currency_code，DictValue=CNY/USD 等）
    /// </summary>
    public string? Currency { get; set; } = string.Empty;

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
/// MaterialMovingPrice 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialMovingPriceImportDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（日期类型，业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    public DateTime? PeriodDate { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode / TaktMaterialPlant.MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string? Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal? StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal? StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_price_control_type；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string? PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal? MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// 币种（字典 accounting_currency_code，DictValue=CNY/USD 等）
    /// </summary>
    public string? Currency { get; set; } = string.Empty;

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
/// MaterialMovingPrice 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialMovingPriceExportDto
{
    /// <summary>
    /// MaterialMovingPriceID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialMovingPriceId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 期间（日期类型，业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    public DateTime PeriodDate { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode / TaktMaterialPlant.MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    public decimal StockQuantity { get; set; }

    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    public decimal StockAmount { get; set; }

    /// <summary>
    /// 价格控制（字典 logistics_price_control_type；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    public string PriceControl { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    public decimal MovingPrice { get; set; }

    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    public int PriceUnit { get; set; } = 0;

    /// <summary>
    /// 币种（字典 accounting_currency_code，DictValue=CNY/USD 等）
    /// </summary>
    public string Currency { get; set; } = string.Empty;

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

// ========================================
// 物料月移动价格推移分析 DTO
// ========================================

/// <summary>
/// 物料 × 月份移动单价转置分析查询
/// </summary>
public class TaktMaterialMovingPriceMonthlyTrendQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动价格期间起（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 移动价格期间止（当月首日语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；缺省取期间末月，相对上月算环比
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 评估类别（可选；为空时按物料+估值分行）
    /// </summary>
    public string? Valuation { get; set; }

    /// <summary>
    /// 物料编码（可选，模糊匹配）
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 涨跌筛选：空=物料价格推移全部 / 机种推移默认领涨领跌各 50；leading=领涨领跌各 50；all=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 物料月移动价格转置行（行=物料+估值，列=各月单价 MovingPrice÷PriceUnit）
/// </summary>
public class TaktMaterialMovingPriceMonthlyTrendDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称（回填：随物料）
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 评估类别
    /// </summary>
    public string Valuation { get; set; } = string.Empty;

    /// <summary>
    /// 币种
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// 各期间单价（键 yyyy-MM；无当月数据时沿用最近有价期间）
    /// </summary>
    public Dictionary<string, decimal> PeriodUnitPrices { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 各期间单价来源月（键=展示 yyyy-MM，值=实际取价 yyyy-MM；值≠键表示回填）
    /// </summary>
    public Dictionary<string, string> PeriodPriceSourcePeriods { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 环比涨跌：none / up / down / flat
    /// </summary>
    public string Trend { get; set; } = "none";

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 环比差额（对比单价 - 基准单价）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（小数比率，保留 4 位；如 0.2978 表示 29.78%，便于 Excel 百分比格式）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 物料月移动价格转置分析结果
/// </summary>
public class TaktMaterialMovingPriceMonthlyTrendResultDto
{
    /// <summary>
    /// 分页物料行
    /// </summary>
    public TaktPagedResult<TaktMaterialMovingPriceMonthlyTrendDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 物料行总数（分页前，已应用涨跌筛选）
    /// </summary>
    public int MaterialCount { get; set; }

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间（关注月）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨价行数（筛选前全量统计）
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌价行数（筛选前全量统计）
    /// </summary>
    public int DownCount { get; set; }

    /// <summary>
    /// 持平行数（筛选前全量统计）
    /// </summary>
    public int FlatCount { get; set; }

    /// <summary>
    /// 无法比较行数（筛选前全量统计）
    /// </summary>
    public int NoneCount { get; set; }
}

/// <summary>
/// 物料-机种-价格推移行（物料×机种组×产品组 + 各月单价）
/// </summary>
public class TaktMaterialMovingPriceModelTrendDto : TaktMaterialMovingPriceMonthlyTrendDto
{
    /// <summary>
    /// 机种组展示（逗号分隔 ModelCode；来源 BOM 成本汇总）
    /// </summary>
    public string ModelGroup { get; set; } = string.Empty;

    /// <summary>
    /// 产品组展示（逗号分隔 ProductCode；来源 BOM 成本明细组件行）
    /// </summary>
    public string ProductGroup { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码列表（去重排序）
    /// </summary>
    public List<string> ModelCodes { get; set; } = new();

    /// <summary>
    /// 产品编码列表（去重排序）
    /// </summary>
    public List<string> ProductCodes { get; set; } = new();

    /// <summary>
    /// 物料描述（优先工厂物料名称，否则 BOM 组件描述）
    /// </summary>
    public string MaterialText { get; set; } = string.Empty;
}

/// <summary>
/// 物料-机种-价格推移分析结果
/// </summary>
public class TaktMaterialMovingPriceModelTrendResultDto
{
    /// <summary>
    /// 分页行
    /// </summary>
    public TaktPagedResult<TaktMaterialMovingPriceModelTrendDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 物料行总数（分页前）
    /// </summary>
    public int MaterialCount { get; set; }

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨价行数
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌价行数
    /// </summary>
    public int DownCount { get; set; }

    /// <summary>
    /// 持平行数
    /// </summary>
    public int FlatCount { get; set; }

    /// <summary>
    /// 无法比较行数
    /// </summary>
    public int NoneCount { get; set; }
}
