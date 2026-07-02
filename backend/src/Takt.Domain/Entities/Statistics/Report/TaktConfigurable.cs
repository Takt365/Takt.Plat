// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Statistics.Report
// 文件名称：TaktConfigurable.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：自研 SQVI 式自定义报表主表（查询定义头）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Shared.Constants;

namespace Takt.Domain.Entities.Statistics.Report;

/// <summary>
/// 自定义报表主实体（SQVI 查询定义）
/// </summary>
/// <remarks>
/// 主子表结构承载多表 JOIN、SQVI 筛选条件、分组与排序定义；
/// 运行时由应用服务编译为 SqlSugar Queryable 并通过 <c>ITaktStatQueryExecutor</c> 执行，Excel 导出走 <c>TaktExcelHelper</c>。
/// </remarks>
[SugarTable("takt_statistics_report_configurable", "自定义报表主表")]
[SugarIndex("ix_configurable_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_configurable_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ReportCode), OrderByType.Asc, true)]
[SugarIndex("ix_configurable_domain", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ReportDomain), OrderByType.Asc, false)]
public class TaktConfigurable : TaktCompanyEntityBase
{    /// <summary>
    /// 报表编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "report_code", ColumnDescription = "报表编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string ReportCode { get; set; } = string.Empty;
    /// <summary>
    /// 报表名称
    /// </summary>
    [SugarColumn(ColumnName = "report_name", ColumnDescription = "报表名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ReportName { get; set; } = string.Empty;
    /// <summary>
    /// 报表业务域（TaktModule 整型，与一级目录菜单 MenuCode 映射；展示名取自菜单 i18n）
    /// </summary>
    [SugarColumn(ColumnName = "report_domain", ColumnDescription = "报表业务域", ColumnDataType = "int", IsNullable = false, DefaultValue = "9")]
    public int ReportDomain { get; set; } = 9;
    /// <summary>
    /// 报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
    /// </summary>
    [SugarColumn(ColumnName = "report_sub_category", ColumnDescription = "报表子分类", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? ReportSubCategory { get; set; }
    /// <summary>
    /// 是否去重行（SELECT DISTINCT）
    /// </summary>
    [SugarColumn(ColumnName = "distinct_rows", ColumnDescription = "是否去重", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DistinctRows { get; set; } = 0;
    /// <summary>
    /// 单次导出最大行数（Excel 上限，防止 OOM）
    /// </summary>
    [SugarColumn(ColumnName = "max_export_rows", ColumnDescription = "导出最大行数", ColumnDataType = "int", IsNullable = false, DefaultValue = "500")]
    public int MaxExportRows { get; set; } = TaktConfigurableConstants.DefaultRowLimit;
    /// <summary>
    /// 单次查询最大行数（预览/分页上限）
    /// </summary>
    [SugarColumn(ColumnName = "max_query_rows", ColumnDescription = "查询最大行数", ColumnDataType = "int", IsNullable = false, DefaultValue = "500")]
    public int MaxQueryRows { get; set; } = TaktConfigurableConstants.DefaultRowLimit;
    /// <summary>
    /// 公开（字典 sys_is_public_type；0=公开，1=私有）
    /// </summary>
    [SugarColumn(ColumnName = "is_public", ColumnDescription = "公开", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsPublic { get; set; } = 0;
    /// <summary>
    /// 报表描述
    /// </summary>
    [SugarColumn(ColumnName = "configurable_description", ColumnDescription = "报表描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ConfigurableDescription { get; set; }
    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 报表状态（0=禁用 1=启用）
    /// </summary>
    [SugarColumn(ColumnName = "report_status", ColumnDescription = "报表状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ReportStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域（主子表 OneToMany）
    // ========================================

    /// <summary>
    /// 数据源表列表（FROM）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableSource.ConfigurableId))]
    public List<TaktConfigurableSource>? Sources { get; set; }

    /// <summary>
    /// 多表关联列表（JOIN）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableJoin.ConfigurableId))]
    public List<TaktConfigurableJoin>? Joins { get; set; }

    /// <summary>
    /// 输出字段列表（SELECT）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableField.ConfigurableId))]
    public List<TaktConfigurableField>? Fields { get; set; }

    /// <summary>
    /// 筛选条件列表（SQVI WHERE）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableSelection.ConfigurableId))]
    public List<TaktConfigurableSelection>? Selections { get; set; }

    /// <summary>
    /// 分组字段列表（GROUP BY）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableGroupBy.ConfigurableId))]
    public List<TaktConfigurableGroupBy>? GroupBys { get; set; }

    /// <summary>
    /// 排序字段列表（ORDER BY）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktConfigurableOrderBy.ConfigurableId))]
    public List<TaktConfigurableOrderBy>? OrderBys { get; set; }
}
