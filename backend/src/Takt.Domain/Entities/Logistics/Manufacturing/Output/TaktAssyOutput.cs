// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutput.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报（产出）主表实体，按工厂、生产日期、生产线等记录生产订单与标准产能
// 计算公式：达成率(%) = 明细实际生产数量合计 ÷ 主表标准产能合计 × 100%（见 TaktProductionStatHelper.CalculateAchievementRatePercent）
// 明细参考：明细达成率(%) = 明细实际生产数量 ÷ 主表标准产能 × 100%（AchievementRate 存于 TaktAssyOutputDetail；标准产能为 0 时取 0）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报（产出）主表实体
/// 达成率(%) = 明细实际生产数量合计 ÷ 主表标准产能合计 × 100%。
/// </summary>
[SugarTable("takt_logistics_manufacturing_output_assy", "组立日报表")]
[SugarIndex("ix_assy_output_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_assy_output_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_assy_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProdCategory), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, nameof(ProdTeam), OrderByType.Asc, nameof(ShiftNo), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_output_assy_prod_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_assy_prod_team", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdTeam), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_assy_prod_order_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, false)]
public class TaktAssyOutput : TaktCompanyEntityBase
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
    /// 直接人员
    /// </summary>
    [SugarColumn(ColumnName = "direct_labor", ColumnDescription = "直接人员", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    [SugarColumn(ColumnName = "indirect_labor", ColumnDescription = "间接人员", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    [SugarColumn(ColumnName = "shift_no", ColumnDescription = "班次", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ShiftNo { get; set; } = 1;

    /// <summary>
    /// 生产订单类型（选项 TaktProductionOrders/options 的 ExtLabel，随工单回填）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_type", ColumnDescription = "生产订单类型", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? ProdOrderType { get; set; }

    /// <summary>
    /// 生产工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_code", ColumnDescription = "生产工单号", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种
    /// </summary>
    [SugarColumn(ColumnName = "model_code", ColumnDescription = "机种", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次
    /// </summary>
    [SugarColumn(ColumnName = "batch_no", ColumnDescription = "批次", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? BatchNo { get; set; }

    /// <summary>
    /// 订单数量
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_qty", ColumnDescription = "订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal ProdOrderQty { get; set; } = 0;

    /// <summary>
    /// 标准工时(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "std_minutes", ColumnDescription = "标准工时(分钟)", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StdMinutes { get; set; } = 0;

    /// <summary>
    /// 标准产能
    /// </summary>
    [SugarColumn(ColumnName = "std_capacity", ColumnDescription = "标准产能", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal StdCapacity { get; set; } = 0;

    /// <summary>
    /// 组立日报明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktAssyOutputDetail.AssyOutputId))]
    public List<TaktAssyOutputDetail>? AssyOutputDetails { get; set; }
}
