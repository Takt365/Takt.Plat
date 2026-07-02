// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspection.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA检查日报实体，按工厂、生产日期等记录检查主数据
// 计算公式：不良率(%) = 明细不良数量合计 ÷ 明细检查数量合计 × 100%（分母为 0 时取 0）
// 直行率(%) = (明细检查数量合计 - 明细不良数量合计) ÷ 明细检查数量合计 × 100%（数量取自 TaktPcbaInspectionDetail）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA检查日报实体
/// 不良率(%) = 明细不良数量合计 ÷ 明细检查数量合计 × 100%；直行率(%) = (检查数量 - 不良数量) ÷ 检查数量 × 100%。
/// </summary>
[SugarTable("takt_logistics_manufacturing_defect_pcba_inspection", "PCBA检查日报表")]
[SugarIndex("ix_pcba_inspection_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_pcba_inspection_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_inspection_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProdCategory), OrderByType.Asc, nameof(ProdDate), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_inspection_prod_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_defect_pcba_inspection_prod_order_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, false)]
public class TaktPcbaInspection : TaktCompanyEntityBase
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
    /// 生产工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_code", ColumnDescription = "生产工单号", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 订单数量
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_qty", ColumnDescription = "订单数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
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
    /// PCBA检查明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktPcbaInspectionDetail.PcbaInspectionId))]
    public List<TaktPcbaInspectionDetail>? PcbaInspectionDetails { get; set; }
}
