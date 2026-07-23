// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDetail.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源子表实体，存储旧物料/新物料、用量、安装位置、BOM与实施日期等信息。
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变来源主表
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_source_detail", "设变来源子表")]
[SugarIndex("ix_ec_source_detail_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_source_detail_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_ec_source_detail_ecid", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SourceEcId), OrderByType.Asc, false)]
public class TaktSourceEcDetail : TaktCompanyEntityBase
{
    /// <summary>
    /// 主ID
    /// </summary>
    [SugarColumn(ColumnName = "source_ec_id", ColumnDescription = "主ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 完成品
    /// </summary>
    [SugarColumn(ColumnName = "source_finished_product", ColumnDescription = "完成品", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string SourceFinishedProduct { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料
    /// </summary>
    [SugarColumn(ColumnName = "source_parent_part", ColumnDescription = "上阶物料", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string SourceParentPart { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    [SugarColumn(ColumnName = "source_legacy_part_no", ColumnDescription = "旧物料号", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceLegacyPartNo { get; set; }

    /// <summary>
    /// 旧物料
    /// </summary>
    [SugarColumn(ColumnName = "source_legacy_part_name", ColumnDescription = "旧物料", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceLegacyPartName { get; set; }

    /// <summary>
    /// 旧物料用量
    /// </summary>
    [SugarColumn(ColumnName = "source_legacy_usage", ColumnDescription = "旧物料用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = true)]
    public decimal? SourceLegacyUsage { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    [SugarColumn(ColumnName = "source_legacy_mounting_position", ColumnDescription = "旧物料安装位置", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceLegacyMountingPosition { get; set; }

    /// <summary>
    /// 新物料
    /// </summary>
    [SugarColumn(ColumnName = "source_replacement_part_no", ColumnDescription = "新物料", Length = 20, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceReplacementPartNo { get; set; }

    /// <summary>
    /// 新物料
    /// </summary>
    [SugarColumn(ColumnName = "source_replacement_part_name", ColumnDescription = "新物料", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceReplacementPartName { get; set; }

    /// <summary>
    /// 新物料用量
    /// </summary>
    [SugarColumn(ColumnName = "source_replacement_usage", ColumnDescription = "新物料用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 5, IsNullable = true)]
    public decimal? SourceReplacementUsage { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    [SugarColumn(ColumnName = "source_replacement_mounting_position", ColumnDescription = "新物料安装位置", Length = 40, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceReplacementMountingPosition { get; set; }

    /// <summary>
    /// BOM番号
    /// </summary>
    [SugarColumn(ColumnName = "source_bom_no", ColumnDescription = "BOM番号", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceBomNo { get; set; }

    /// <summary>
    /// 兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）
    /// </summary>
    [SugarColumn(ColumnName = "source_compatibility", ColumnDescription = "兼容性", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceCompatibility { get; set; }

    /// <summary>
    /// 区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）
    /// </summary>
    [SugarColumn(ColumnName = "source_distinction", ColumnDescription = "区分", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceDistinction { get; set; }

    /// <summary>
    /// 安排指示（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）
    /// </summary>
    [SugarColumn(ColumnName = "source_instruction", ColumnDescription = "安排指示", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceInstruction { get; set; }

    /// <summary>
    /// 旧物料处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）
    /// </summary>
    [SugarColumn(ColumnName = "source_legacy_part_disposition", ColumnDescription = "旧物料处理", Length = 4, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? SourceLegacyPartDisposition { get; set; }

    /// <summary>
    /// BOM生效日期
    /// </summary>
    [SugarColumn(ColumnName = "source_bom_effective_date", ColumnDescription = "BOM生效日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? SourceBomEffectiveDate { get; set; }

    /// <summary>
    /// 设变来源主表
    /// </summary>
    // ========================================
    // 导航属性区域
    // ========================================
    [Navigate(NavigateType.ManyToOne, nameof(SourceEcId))]
    public TaktSourceEc? SourceEc { get; set; }
}
