// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Models.Statistics
// 文件名称：TaktStatQueryModels.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：SQVI 式自定义报表 SQL 编译模型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Shared.Models.Statistics;

/// <summary>
/// 报表 SQL 编译请求（由实体/DTO 映射）
/// </summary>
public sealed class TaktStatQueryBuildRequest
{
    /// <summary>
    /// 租户编码（强制隔离）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（强制隔离）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否 SELECT DISTINCT
    /// </summary>
    public TaktYesNo DistinctRows { get; set; } = TaktYesNo.No;

    /// <summary>
    /// 数据源表
    /// </summary>
    public IReadOnlyList<TaktStatQuerySourceItem> Sources { get; set; } = Array.Empty<TaktStatQuerySourceItem>();

    /// <summary>
    /// 多表关联
    /// </summary>
    public IReadOnlyList<TaktStatQueryJoinItem> Joins { get; set; } = Array.Empty<TaktStatQueryJoinItem>();

    /// <summary>
    /// 输出字段
    /// </summary>
    public IReadOnlyList<TaktStatQueryFieldItem> Fields { get; set; } = Array.Empty<TaktStatQueryFieldItem>();

    /// <summary>
    /// 筛选定义（Selection Screen）
    /// </summary>
    public IReadOnlyList<TaktStatQuerySelectionItem> Selections { get; set; } = Array.Empty<TaktStatQuerySelectionItem>();

    /// <summary>
    /// 分组字段
    /// </summary>
    public IReadOnlyList<TaktStatQueryGroupByItem> GroupBys { get; set; } = Array.Empty<TaktStatQueryGroupByItem>();

    /// <summary>
    /// 排序字段
    /// </summary>
    public IReadOnlyList<TaktStatQueryOrderByItem> OrderBys { get; set; } = Array.Empty<TaktStatQueryOrderByItem>();

    /// <summary>
    /// 运行时筛选值（键为 Selection 的 SortOrder）
    /// </summary>
    public IReadOnlyDictionary<int, TaktStatQuerySelectionValue> RuntimeSelectionValues { get; set; }
        = new Dictionary<int, TaktStatQuerySelectionValue>();
}

/// <summary>
/// 数据源项
/// </summary>
public sealed class TaktStatQuerySourceItem
{
    /// <summary>
    /// 别名
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 是否主表
    /// </summary>
    public TaktYesNo IsPrimary { get; set; } = TaktYesNo.No;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; }
}

/// <summary>
/// 关联项
/// </summary>
public sealed class TaktStatQueryJoinItem
{
    /// <summary>
    /// 关联类型
    /// </summary>
    public TaktConfigurableJoinType JoinType { get; set; } = TaktConfigurableJoinType.Inner;

    /// <summary>
    /// 左表别名
    /// </summary>
    public string LeftSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 左表列
    /// </summary>
    public string LeftColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 右表别名
    /// </summary>
    public string RightSourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 右表列
    /// </summary>
    public string RightColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; }
}

/// <summary>
/// 输出字段项
/// </summary>
public sealed class TaktStatQueryFieldItem
{
    /// <summary>
    /// 数据源别名
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 输出别名
    /// </summary>
    public string? OutputAlias { get; set; }

    /// <summary>
    /// 聚合函数
    /// </summary>
    public TaktConfigurableAggregateFunc AggregateFunc { get; set; } = TaktConfigurableAggregateFunc.None;

    /// <summary>
    /// 是否可见
    /// </summary>
    public TaktYesNo IsVisible { get; set; } = TaktYesNo.Yes;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; }
}

/// <summary>
/// 筛选定义项
/// </summary>
public sealed class TaktStatQuerySelectionItem
{
    /// <summary>
    /// 数据源别名
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 比较符
    /// </summary>
    public TaktConfigurableFilterOperator FilterOperator { get; set; } = TaktConfigurableFilterOperator.Equal;

    /// <summary>
    /// 是否必填
    /// </summary>
    public TaktYesNo IsRequired { get; set; } = TaktYesNo.No;

    /// <summary>
    /// 排序号（运行时取值键）
    /// </summary>
    public int SortOrder { get; set; }
}

/// <summary>
/// 运行时筛选值
/// </summary>
public sealed class TaktStatQuerySelectionValue
{
    /// <summary>
    /// 单值或 IN 列表（逗号分隔）
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// 区间结束值
    /// </summary>
    public string? ValueTo { get; set; }
}

/// <summary>
/// 分组项
/// </summary>
public sealed class TaktStatQueryGroupByItem
{
    /// <summary>
    /// 数据源别名
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; }
}

/// <summary>
/// 排序项
/// </summary>
public sealed class TaktStatQueryOrderByItem
{
    /// <summary>
    /// 数据源别名
    /// </summary>
    public string SourceAlias { get; set; } = string.Empty;

    /// <summary>
    /// 列名
    /// </summary>
    public string ColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向
    /// </summary>
    public TaktConfigurableSortDirection SortDirection { get; set; } = TaktConfigurableSortDirection.Asc;

    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; }
}

/// <summary>
/// SQL 编译结果
/// </summary>
public sealed class TaktStatQueryBuildResult
{
    /// <summary>
    /// 可执行的 SELECT 语句（不含分页）
    /// </summary>
    public string Sql { get; set; } = string.Empty;

    /// <summary>
    /// 命名参数
    /// </summary>
    public Dictionary<string, object?> Parameters { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 结果列键（与 SELECT 别名一致）
    /// </summary>
    public IReadOnlyList<string> OutputKeys { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 结果列显示名（与 OutputKeys 一一对应）
    /// </summary>
    public IReadOnlyList<string> OutputLabels { get; set; } = Array.Empty<string>();
}
