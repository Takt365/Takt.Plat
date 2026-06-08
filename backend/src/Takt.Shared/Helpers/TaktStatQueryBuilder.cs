// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktStatQueryBuilder.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：SQVI 式自定义报表只读 SELECT 编译器（JOIN/筛选/分组/排序）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text;
using System.Text.RegularExpressions;
using Takt.Shared.Enums;
using Takt.Shared.Models.Statistics;

namespace Takt.Shared.Helpers;

/// <summary>
/// 自定义报表 SQL 编译器（纯函数，无副作用）
/// </summary>
public static class TaktStatQueryBuilder
{
    private static readonly Regex TableNamePattern = new(@"^takt_[a-z0-9_]+$", RegexOptions.Compiled);
    private static readonly Regex ColumnNamePattern = new(@"^[a-z][a-z0-9_]*$", RegexOptions.Compiled);
    private static readonly Regex AliasPattern = new(@"^[A-Za-z][A-Za-z0-9_]{0,9}$", RegexOptions.Compiled);

    /// <summary>
    /// 将报表定义编译为参数化只读 SELECT 语句
    /// </summary>
    /// <param name="request">编译请求</param>
    /// <returns>SQL 与参数</returns>
    /// <exception cref="ArgumentNullException">request 为空</exception>
    /// <exception cref="ArgumentException">定义非法或缺少必填项</exception>
    public static TaktStatQueryBuildResult Build(TaktStatQueryBuildRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.TenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.CompanyCode);
        if (request.Sources == null || request.Sources.Count == 0)
        {
            throw new ArgumentException("报表至少需要一个数据源表", nameof(request));
        }

        var sources = request.Sources.OrderBy(x => x.SortOrder).ToList();
        var primary = sources.FirstOrDefault(x => x.IsPrimary == TaktYesNo.Yes) ?? sources[0];
        var aliasToTable = BuildAliasMap(sources);
        var parameters = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
        {
            ["p_tenant_code"] = request.TenantCode,
            ["p_company_code"] = request.CompanyCode,
        };

        var visibleFields = (request.Fields ?? Array.Empty<TaktStatQueryFieldItem>())
            .Where(x => x.IsVisible == TaktYesNo.Yes)
            .OrderBy(x => x.SortOrder)
            .ToList();
        if (visibleFields.Count == 0)
        {
            throw new ArgumentException("报表至少需要一个输出字段", nameof(request));
        }

        var groupBys = (request.GroupBys ?? Array.Empty<TaktStatQueryGroupByItem>())
            .OrderBy(x => x.SortOrder)
            .ToList();
        var hasAggregate = visibleFields.Any(x => x.AggregateFunc != TaktConfigurableAggregateFunc.None);
        var useGroupBy = groupBys.Count > 0 || hasAggregate;

        var outputKeys = new List<string>();
        var outputLabels = new List<string>();
        var selectParts = new List<string>();
        foreach (var field in visibleFields)
        {
            ValidateAlias(field.SourceAlias);
            ValidateColumn(field.ColumnName);
            var colExpr = $"{Bracket(field.SourceAlias)}.{Bracket(field.ColumnName)}";
            var expr = field.AggregateFunc switch
            {
                TaktConfigurableAggregateFunc.Count => $"COUNT({colExpr})",
                TaktConfigurableAggregateFunc.Sum => $"SUM({colExpr})",
                TaktConfigurableAggregateFunc.Avg => $"AVG({colExpr})",
                TaktConfigurableAggregateFunc.Min => $"MIN({colExpr})",
                TaktConfigurableAggregateFunc.Max => $"MAX({colExpr})",
                _ => colExpr,
            };
            var outputKey = ResolveOutputKey(field);
            outputKeys.Add(outputKey);
            outputLabels.Add(string.IsNullOrWhiteSpace(field.DisplayName) ? outputKey : field.DisplayName);
            selectParts.Add($"{expr} AS {Bracket(outputKey)}");
        }

        var fromClause = BuildFromClause(primary, sources, request.Joins ?? Array.Empty<TaktStatQueryJoinItem>(), aliasToTable);
        var whereClause = BuildWhereClause(
            primary.SourceAlias,
            request.Selections ?? Array.Empty<TaktStatQuerySelectionItem>(),
            request.RuntimeSelectionValues,
            parameters);

        var sql = new StringBuilder();
        sql.Append("SELECT ");
        if (request.DistinctRows == TaktYesNo.Yes)
        {
            sql.Append("DISTINCT ");
        }
        sql.Append(string.Join(", ", selectParts));
        sql.Append(' ');
        sql.Append(fromClause);
        sql.Append(' ');
        sql.Append(whereClause);

        if (useGroupBy)
        {
            var groupColumns = groupBys.Count > 0
                ? groupBys.Select(x =>
                {
                    ValidateAlias(x.SourceAlias);
                    ValidateColumn(x.ColumnName);
                    return $"{Bracket(x.SourceAlias)}.{Bracket(x.ColumnName)}";
                }).ToList()
                : visibleFields
                    .Where(x => x.AggregateFunc == TaktConfigurableAggregateFunc.None)
                    .Select(x => $"{Bracket(x.SourceAlias)}.{Bracket(x.ColumnName)}")
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
            if (groupColumns.Count == 0)
            {
                throw new ArgumentException("启用聚合时必须配置分组字段或非聚合输出列", nameof(request));
            }
            sql.Append(" GROUP BY ");
            sql.Append(string.Join(", ", groupColumns));
        }

        var orderBys = (request.OrderBys ?? Array.Empty<TaktStatQueryOrderByItem>())
            .OrderBy(x => x.SortOrder)
            .ToList();
        if (orderBys.Count > 0)
        {
            var orderParts = orderBys.Select(x =>
            {
                ValidateAlias(x.SourceAlias);
                ValidateColumn(x.ColumnName);
                var dir = x.SortDirection == TaktConfigurableSortDirection.Desc ? "DESC" : "ASC";
                return $"{Bracket(x.SourceAlias)}.{Bracket(x.ColumnName)} {dir}";
            });
            sql.Append(" ORDER BY ");
            sql.Append(string.Join(", ", orderParts));
        }

        return new TaktStatQueryBuildResult
        {
            Sql = sql.ToString(),
            Parameters = parameters,
            OutputKeys = outputKeys,
            OutputLabels = outputLabels,
        };
    }

    /// <summary>
    /// 包装分页 SQL（SQL Server OFFSET/FETCH）
    /// </summary>
    /// <param name="coreSql">核心 SELECT</param>
    /// <param name="pageIndex">页码（从 1 开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="maxPageSize">最大 pageSize</param>
    /// <returns>分页 SQL</returns>
    public static string WrapPagedSql(string coreSql, int pageIndex, int pageSize, int maxPageSize)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(coreSql);
        pageIndex = Math.Max(1, pageIndex);
        pageSize = Math.Min(maxPageSize, Math.Max(1, pageSize));
        var skip = checked((pageIndex - 1) * pageSize);
        return $"{coreSql} OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
    }

    /// <summary>
    /// 包装计数 SQL
    /// </summary>
    /// <param name="coreSql">核心 SELECT</param>
    /// <returns>COUNT 包装语句</returns>
    public static string WrapCountSql(string coreSql)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(coreSql);
        return $"SELECT COUNT(1) AS takt_total FROM ({coreSql}) AS takt_count_sub";
    }

    /// <summary>
    /// 包装导出行数上限
    /// </summary>
    /// <param name="coreSql">核心 SELECT</param>
    /// <param name="maxRows">最大行数</param>
    /// <returns>带 TOP 的语句</returns>
    public static string WrapTopSql(string coreSql, int maxRows)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(coreSql);
        if (maxRows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxRows), "maxRows 必须大于 0");
        }
        var normalized = coreSql.TrimStart();
        if (normalized.StartsWith("SELECT DISTINCT", StringComparison.OrdinalIgnoreCase))
        {
            return $"SELECT DISTINCT TOP ({maxRows})" + normalized["SELECT DISTINCT".Length..];
        }
        if (normalized.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
        {
            return $"SELECT TOP ({maxRows})" + normalized["SELECT".Length..];
        }
        throw new ArgumentException("仅支持 SELECT 语句", nameof(coreSql));
    }

    private static Dictionary<string, string> BuildAliasMap(IReadOnlyList<TaktStatQuerySourceItem> sources)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var source in sources)
        {
            ValidateAlias(source.SourceAlias);
            ValidateTable(source.TableName);
            if (!map.TryAdd(source.SourceAlias, source.TableName))
            {
                throw new ArgumentException($"数据源别名重复: {source.SourceAlias}");
            }
        }
        return map;
    }

    private static string BuildFromClause(
        TaktStatQuerySourceItem primary,
        IReadOnlyList<TaktStatQuerySourceItem> sources,
        IReadOnlyList<TaktStatQueryJoinItem> joins,
        IReadOnlyDictionary<string, string> aliasToTable)
    {
        ValidateAlias(primary.SourceAlias);
        ValidateTable(primary.TableName);
        var sb = new StringBuilder();
        sb.Append($"FROM {Bracket(primary.TableName)} AS {Bracket(primary.SourceAlias)}");
        foreach (var join in joins.OrderBy(x => x.SortOrder))
        {
            ValidateAlias(join.LeftSourceAlias);
            ValidateAlias(join.RightSourceAlias);
            ValidateColumn(join.LeftColumnName);
            ValidateColumn(join.RightColumnName);
            if (!aliasToTable.ContainsKey(join.LeftSourceAlias) || !aliasToTable.ContainsKey(join.RightSourceAlias))
            {
                throw new ArgumentException($"JOIN 引用了未声明的数据源别名: {join.LeftSourceAlias} / {join.RightSourceAlias}");
            }
            var rightTable = aliasToTable[join.RightSourceAlias];
            ValidateTable(rightTable);
            var joinKeyword = join.JoinType switch
            {
                TaktConfigurableJoinType.Left => "LEFT JOIN",
                TaktConfigurableJoinType.Right => "RIGHT JOIN",
                TaktConfigurableJoinType.Full => "FULL JOIN",
                _ => "INNER JOIN",
            };
            sb.Append(' ');
            sb.Append(joinKeyword);
            sb.Append(' ');
            sb.Append($"{Bracket(rightTable)} AS {Bracket(join.RightSourceAlias)}");
            sb.Append(" ON ");
            sb.Append($"{Bracket(join.LeftSourceAlias)}.{Bracket(join.LeftColumnName)}");
            sb.Append(" = ");
            sb.Append($"{Bracket(join.RightSourceAlias)}.{Bracket(join.RightColumnName)}");
        }
        return sb.ToString();
    }

    private static string BuildWhereClause(
        string primaryAlias,
        IReadOnlyList<TaktStatQuerySelectionItem> selections,
        IReadOnlyDictionary<int, TaktStatQuerySelectionValue> runtimeValues,
        IDictionary<string, object?> parameters)
    {
        var clauses = new List<string>
        {
            $"{Bracket(primaryAlias)}.{Bracket("tenant_code")} = @p_tenant_code",
            $"{Bracket(primaryAlias)}.{Bracket("company_code")} = @p_company_code",
            $"{Bracket(primaryAlias)}.{Bracket("is_deleted")} = 0",
        };
        foreach (var selection in selections.OrderBy(x => x.SortOrder))
        {
            ValidateAlias(selection.SourceAlias);
            ValidateColumn(selection.ColumnName);
            runtimeValues.TryGetValue(selection.SortOrder, out var runtimeValue);
            var value = runtimeValue?.Value;
            var valueTo = runtimeValue?.ValueTo;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (selection.IsRequired == TaktYesNo.Yes
                    && selection.FilterOperator is not (TaktConfigurableFilterOperator.IsNull or TaktConfigurableFilterOperator.IsNotNull))
                {
                    throw new ArgumentException($"筛选条件必填: {selection.SourceAlias}.{selection.ColumnName}");
                }
                continue;
            }
            var columnExpr = $"{Bracket(selection.SourceAlias)}.{Bracket(selection.ColumnName)}";
            var paramBase = $"p_sel_{selection.SortOrder}";
            switch (selection.FilterOperator)
            {
                case TaktConfigurableFilterOperator.Equal:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} = @{paramBase}");
                    break;
                case TaktConfigurableFilterOperator.NotEqual:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} <> @{paramBase}");
                    break;
                case TaktConfigurableFilterOperator.GreaterThan:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} > @{paramBase}");
                    break;
                case TaktConfigurableFilterOperator.GreaterThanOrEqual:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} >= @{paramBase}");
                    break;
                case TaktConfigurableFilterOperator.LessThan:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} < @{paramBase}");
                    break;
                case TaktConfigurableFilterOperator.LessThanOrEqual:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} <= @{paramBase}");
                    break;
                case TaktConfigurableFilterOperator.Contains:
                    parameters[paramBase] = $"%{value}%";
                    clauses.Add($"{columnExpr} LIKE @{paramBase}");
                    break;
                case TaktConfigurableFilterOperator.Between:
                    if (string.IsNullOrWhiteSpace(valueTo))
                    {
                        throw new ArgumentException($"区间筛选缺少结束值: {selection.SourceAlias}.{selection.ColumnName}");
                    }
                    parameters[$"{paramBase}_from"] = value;
                    parameters[$"{paramBase}_to"] = valueTo;
                    clauses.Add($"{columnExpr} BETWEEN @{paramBase}_from AND @{paramBase}_to");
                    break;
                case TaktConfigurableFilterOperator.In:
                    var inParts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    if (inParts.Length == 0)
                    {
                        throw new ArgumentException($"IN 筛选值无效: {selection.SourceAlias}.{selection.ColumnName}");
                    }
                    var inParams = new List<string>();
                    for (var i = 0; i < inParts.Length; i++)
                    {
                        var inKey = $"{paramBase}_{i}";
                        parameters[inKey] = inParts[i];
                        inParams.Add($"@{inKey}");
                    }
                    clauses.Add($"{columnExpr} IN ({string.Join(", ", inParams)})");
                    break;
                case TaktConfigurableFilterOperator.IsNull:
                    clauses.Add($"{columnExpr} IS NULL");
                    break;
                case TaktConfigurableFilterOperator.IsNotNull:
                    clauses.Add($"{columnExpr} IS NOT NULL");
                    break;
                default:
                    throw new ArgumentException($"不支持的筛选运算符: {selection.FilterOperator}");
            }
        }
        return $"WHERE {string.Join(" AND ", clauses)}";
    }

    private static string ResolveOutputKey(TaktStatQueryFieldItem field)
    {
        if (!string.IsNullOrWhiteSpace(field.OutputAlias))
        {
            ValidateOutputAlias(field.OutputAlias);
            return field.OutputAlias;
        }
        var key = $"{field.SourceAlias}_{field.ColumnName}";
        ValidateOutputAlias(key);
        return key;
    }

    private static string Bracket(string identifier)
    {
        ValidateIdentifier(identifier);
        return $"[{identifier}]";
    }

    private static void ValidateTable(string tableName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
        if (!TableNamePattern.IsMatch(tableName))
        {
            throw new ArgumentException($"非法表名: {tableName}");
        }
    }

    private static void ValidateColumn(string columnName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(columnName);
        if (!ColumnNamePattern.IsMatch(columnName))
        {
            throw new ArgumentException($"非法列名: {columnName}");
        }
    }

    private static void ValidateAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);
        if (!AliasPattern.IsMatch(alias))
        {
            throw new ArgumentException($"非法数据源别名: {alias}");
        }
    }

    private static void ValidateOutputAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);
        if (!Regex.IsMatch(alias, @"^[A-Za-z][A-Za-z0-9_]{0,63}$"))
        {
            throw new ArgumentException($"非法输出别名: {alias}");
        }
    }

    private static void ValidateIdentifier(string identifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        if (!Regex.IsMatch(identifier, @"^[A-Za-z][A-Za-z0-9_]*$"))
        {
            throw new ArgumentException($"非法标识符: {identifier}");
        }
    }
}
