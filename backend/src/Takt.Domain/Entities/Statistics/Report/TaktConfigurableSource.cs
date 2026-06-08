// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Statistics.Report
// 文件名称：TaktConfigurableSource.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：自定义报表数据源表（FROM 子句）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Statistics.Report;

/// <summary>
/// 自定义报表数据源（单表及别名）
/// </summary>
[SugarTable("takt_statistics_report_configurable_source", "自定义报表数据源表")]
[SugarIndex("ix_configurable_source_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_source_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_source_report_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ConfigurableId), OrderByType.Asc, false)]
public class TaktConfigurableSource : TaktCompanyEntityBase
{
    /// <summary>
    /// 关联报表主表 ID（主子表关系）
    /// </summary>
    [SugarColumn(ColumnName = "configurable_id", ColumnDescription = "报表主表ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ConfigurableId { get; set; }

    /// <summary>
    /// 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
    /// </summary>
    [SugarColumn(ColumnName = "source_alias", ColumnDescription = "数据源别名", ColumnDataType = "varchar", Length = 10, IsNullable = false)]
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
    /// </summary>
    [SugarColumn(ColumnName = "table_name", ColumnDescription = "物理表名", ColumnDataType = "varchar", Length = 128, IsNullable = false)]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 是否主表（驱动 FROM 的第一张表）
    /// </summary>
    [SugarColumn(ColumnName = "is_primary", ColumnDescription = "是否主表", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktYesNo IsPrimary { get; set; } = TaktYesNo.No;

    /// <summary>
    /// 排序号（多表 FROM 顺序）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 关联的报表主表
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToOne, nameof(ConfigurableId))]
    public TaktConfigurable? Configurable { get; set; }
}
