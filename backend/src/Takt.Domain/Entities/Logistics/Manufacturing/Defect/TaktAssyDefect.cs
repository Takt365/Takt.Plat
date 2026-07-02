// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefect.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：组立不良日报实体，按工厂、生产日期、生产线等记录不良主数据
// 计算公式：不良率(%) = (生实实绩 - 无不良数量) ÷ 生实实绩 × 100%（或明细不良数量合计 ÷ 生实实绩 × 100%）
// 直行率(%) = 无不良数量 ÷ 生实实绩 × 100%（生实实绩为 0 时取 0；不良率 + 直行率 = 100%）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Defect;

/// <summary>
/// 组立不良日报实体
/// 不良率(%) = (生实实绩 - 无不良数量) ÷ 生实实绩 × 100%；直行率(%) = 无不良数量 ÷ 生实实绩 × 100%。
/// </summary>
[SugarTable("takt_logistics_manufacturing_defect_assy", "组立不良日报表")]
[SugarIndex("ix_assy_defect_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_assy_defect_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_assy_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProdCategory), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, nameof(ProdTeam), OrderByType.Asc, nameof(ShiftNo), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_assy_prod_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_assy_prod_team", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdTeam), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_assy_prod_order_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, false)]
public class TaktAssyDefect : TaktCompanyEntityBase
{
    /// <summary>
    /// 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", Length = 4, ColumnDataType = "nvarchar", IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_prod_category，存 DictValue：RD/EVT/DVT/EPP/PP/FPP/MP/RPR/RWR）
    /// </summary>
    [SugarColumn(ColumnName = "prod_category", ColumnDescription = "生产类别", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    [SugarColumn(ColumnName = "prod_date", ColumnDescription = "生产日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
    /// </summary>
    [SugarColumn(ColumnName = "prod_team", ColumnDescription = "生产班组", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdTeam { get; set; } = string.Empty;

    /// <summary>
    /// 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    [SugarColumn(ColumnName = "shift_no", ColumnDescription = "班次", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ShiftNo { get; set; } = 1;

    /// <summary>
    /// 生产工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_code", ColumnDescription = "生产工单号", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产订单数量
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_qty", ColumnDescription = "生产订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal ProdOrderQty { get; set; } = 0;

    /// <summary>
    /// 机种
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "机种", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    [SugarColumn(ColumnName = "batch_no", ColumnDescription = "批次", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? BatchNo { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 生实实绩
    /// </summary>
    [SugarColumn(ColumnName = "prod_actual_qty", ColumnDescription = "生实实绩", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal ProdActualQty { get; set; } = 0;

    /// <summary>
    /// 无不良数量
    /// </summary>
    [SugarColumn(ColumnName = "good_quantity", ColumnDescription = "无不良数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal GoodQuantity { get; set; } = 0;

    /// <summary>
    /// 组立不良明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktAssyDefectDetail.AssyDefectId))]
    public List<TaktAssyDefectDetail>? AssyDefectDetails { get; set; }
}
