// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Materials
// 文件名称：TaktMaterialMovingPrice.cs
// 创建时间：2026-07-15
// 创建人：Takt365(Cursor AI)
// 功能描述：移动价格实体（按工厂/物料/评估类别/期间维护库存与移动平均价）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Materials;

/// <summary>
/// 移动价格实体
/// 唯一键：租户 + 公司 + 工厂 + 期间 + 物料 + 评估类别（期间存当月首日表示年月）
/// </summary>
[SugarTable("takt_logistics_materials_material_moving_price", "移动价格表")]
[SugarIndex("ix_material_moving_price_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_moving_price_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_material_moving_price_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PeriodDate), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, nameof(Valuation), OrderByType.Asc, true)]
[SugarIndex("ix_material_moving_price_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_material_moving_price_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PeriodDate), OrderByType.Asc, false)]
public class TaktMaterialMovingPrice : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "varchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 期间（日期类型，业务存当月首日，表示年月，如 2026-07-01 → 2026年7月）
    /// </summary>
    [SugarColumn(ColumnName = "period_date", ColumnDescription = "期间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PeriodDate { get; set; }
    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;
    /// <summary>
    /// 评估类别（字典 logistics_valuation_class_category；Z792=成品，Z790=半成品，Z300=原材料）
    /// </summary>
    [SugarColumn(ColumnName = "valuation", ColumnDescription = "评估类别", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string Valuation { get; set; } = string.Empty;
    /// <summary>
    /// 库存数量（基本单位，4 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "stock_quantity", ColumnDescription = "库存数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal StockQuantity { get; set; } = 0;
    /// <summary>
    /// 库存金额（与币种一致，2 位小数）
    /// </summary>
    [SugarColumn(ColumnName = "stock_amount", ColumnDescription = "库存金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StockAmount { get; set; } = 0;
    /// <summary>
    /// 价格控制（字典 logistics_price_control_type；S=标准价格，V=移动平均价格/周期单价；默认 V）
    /// </summary>
    [SugarColumn(ColumnName = "price_control", ColumnDescription = "价格控制", ColumnDataType = "nvarchar", Length = 1, IsNullable = false, DefaultValue = "V")]
    public string PriceControl { get; set; } = "V";
    /// <summary>
    /// 移动价格（decimal，5 位小数；相对价格单位）
    /// </summary>
    [SugarColumn(ColumnName = "moving_price", ColumnDescription = "移动价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = false, DefaultValue = "0")]
    public decimal MovingPrice { get; set; } = 0;
    /// <summary>
    /// 价格单位（字典 logistics_price_unit_param；1/10/100/1000；默认 1000）
    /// </summary>
    [SugarColumn(ColumnName = "price_unit", ColumnDescription = "价格单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "1000")]
    public int PriceUnit { get; set; } = 1000;
    /// <summary>
    /// 币种（字典 accounting_currency_code，DictValue=CNY/USD 等）
    /// </summary>
    [SugarColumn(ColumnName = "currency", ColumnDescription = "币种", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "CNY")]
    public string Currency { get; set; } = "CNY";
}
