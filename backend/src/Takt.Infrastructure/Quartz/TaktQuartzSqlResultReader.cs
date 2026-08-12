// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzSqlResultReader.cs
// 创建时间：2026-07-15
// 创建人：Takt365(Cursor AI)
// 功能描述：执行 Quartz SQL 脚本并解析 QUARTZ_SYNC_SUMMARY 结果集（源/更新前/更新后/增改删）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using SqlSugar;
using Takt.Shared.Constants;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz SQL 执行与汇总结果解析（多结果集；摘要写入 ExecuteMessage / quartz 日志）
/// </summary>
public static class TaktQuartzSqlResultReader
{
    /// <summary>
    /// 执行非查询脚本，读取全部结果集并格式化为执行摘要
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="sql">可执行 SQL</param>
    /// <param name="scriptPath">脚本路径（写入摘要）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>含行数统计的执行摘要</returns>
    public static async Task<string> ExecuteAndFormatSummaryAsync(
        ISqlSugarClient db,
        string sql,
        string? scriptPath,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentException.ThrowIfNullOrWhiteSpace(sql);
        var scopes = new List<SyncScopeCounts>();
        var legacyPairs = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var connection = db.Ado.Connection as DbConnection
            ?? throw new InvalidOperationException("Quartz SQL 执行失败：Ado.Connection 不是 DbConnection");
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = db.Ado.CommandTimeOut > 0 ? db.Ado.CommandTimeOut : 1800;
        if (db.Ado.Transaction is DbTransaction dbTransaction)
        {
            command.Transaction = dbTransaction;
        }
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        do
        {
            await ConsumeResultSetAsync(reader, scopes, legacyPairs, cancellationToken).ConfigureAwait(false);
        }
        while (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false));
        return FormatExecuteMessage(scriptPath, scopes, legacyPairs);
    }

    /// <summary>
    /// 消费单个结果集：优先 QUARTZ_SYNC_SUMMARY，否则兼容两列 metric/value
    /// </summary>
    private static async Task ConsumeResultSetAsync(
        DbDataReader reader,
        List<SyncScopeCounts> scopes,
        Dictionary<string, int> legacyPairs,
        CancellationToken cancellationToken)
    {
        if (reader.FieldCount == 0)
        {
            return;
        }
        var hasSummaryTag = HasColumn(reader, "summary_tag");
        if (hasSummaryTag)
        {
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                var tag = reader["summary_tag"]?.ToString();
                if (!string.Equals(tag, TaktQuartzConstants.SqlSyncSummaryTag, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                scopes.Add(new SyncScopeCounts
                {
                    Scope = ReadString(reader, "scope"),
                    SourceRawCount = HasColumn(reader, "source_raw_count")
                        ? ReadInt(reader, "source_raw_count")
                        : HasColumn(reader, "sap_raw_count")
                            ? ReadInt(reader, "sap_raw_count")
                            : null,
                    SourceCount = ReadInt(reader, "source_count"),
                    SkippedEmptyCount = HasColumn(reader, "skipped_empty_count")
                        ? ReadInt(reader, "skipped_empty_count")
                        : null,
                    ModelBackfilledCount = HasColumn(reader, "model_backfilled_count")
                        ? ReadInt(reader, "model_backfilled_count")
                        : null,
                    MaterialTypeBackfilledCount = HasColumn(reader, "material_type_backfilled_count")
                        ? ReadInt(reader, "material_type_backfilled_count")
                        : null,
                    AverageUpdatedCount = HasColumn(reader, "average_updated_count")
                        ? ReadInt(reader, "average_updated_count")
                        : null,
                    DedupeDroppedCount = HasColumn(reader, "dedupe_dropped")
                        ? ReadInt(reader, "dedupe_dropped")
                        : null,
                    TargetBefore = ReadInt(reader, "target_before"),
                    TargetAfter = ReadInt(reader, "target_after"),
                    TargetPhysical = ReadInt(reader, "target_physical"),
                    SoftDeleted = ReadInt(reader, "soft_deleted"),
                    InsertCount = ReadInt(reader, "insert_count"),
                    UpdateCount = ReadInt(reader, "update_count"),
                    UnchangedCount = HasColumn(reader, "unchanged_count")
                        ? ReadInt(reader, "unchanged_count")
                        : Math.Max(0, ReadInt(reader, "source_count")
                            - ReadInt(reader, "insert_count")
                            - ReadInt(reader, "update_count")),
                    DeleteCount = ReadInt(reader, "delete_count"),
                    SoftDeletedKeys = HasColumn(reader, "soft_deleted_keys")
                        ? ReadString(reader, "soft_deleted_keys")
                        : string.Empty,
                });
            }
            return;
        }
        if (reader.FieldCount >= 2)
        {
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                if (reader.IsDBNull(0) || reader.IsDBNull(1))
                {
                    continue;
                }
                var key = Convert.ToString(reader.GetValue(0), CultureInfo.InvariantCulture);
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }
                if (int.TryParse(
                        Convert.ToString(reader.GetValue(1), CultureInfo.InvariantCulture),
                        NumberStyles.Integer,
                        CultureInfo.InvariantCulture,
                        out var value))
                {
                    legacyPairs[key.Trim()] = value;
                }
            }
        }
    }

    /// <summary>
    /// 格式化执行摘要文案（供落库 ExecuteMessage 与 quartz 日志）
    /// </summary>
    private static string FormatExecuteMessage(
        string? scriptPath,
        IReadOnlyList<SyncScopeCounts> scopes,
        IReadOnlyDictionary<string, int> legacyPairs)
    {
        var path = string.IsNullOrWhiteSpace(scriptPath) ? "(inline)" : scriptPath.Trim();
        if (scopes.Count > 0)
        {
            var sb = new StringBuilder();
            sb.Append("SQL 同步完成，路径=").Append(path);
            foreach (var scope in scopes)
            {
                sb.Append("；");
                if (!string.IsNullOrWhiteSpace(scope.Scope))
                {
                    sb.Append('[').Append(MapScopeLabel(scope.Scope)).Append(']');
                }
                // source_raw=源表物理行；装入=过滤空键+业务键去重后；真正更新=业务字段有差；未变=MERGE 未写
                if (scope.SourceRawCount.HasValue)
                {
                    sb.Append("源表=").Append(scope.SourceRawCount.Value.ToString(CultureInfo.InvariantCulture));
                    sb.Append("，装入=");
                }
                else
                {
                    sb.Append("装入=");
                }
                sb.Append(scope.SourceCount.ToString(CultureInfo.InvariantCulture));
                if (scope.SkippedEmptyCount is > 0)
                {
                    sb.Append("，跳过空键=").Append(scope.SkippedEmptyCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (scope.ModelBackfilledCount is > 0)
                {
                    sb.Append("，回填机种=").Append(scope.ModelBackfilledCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (scope.MaterialTypeBackfilledCount is > 0)
                {
                    sb.Append("，回填物料类型=").Append(scope.MaterialTypeBackfilledCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (scope.AverageUpdatedCount is > 0)
                {
                    sb.Append("，重算月均=").Append(scope.AverageUpdatedCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (scope.DedupeDroppedCount is > 0)
                {
                    sb.Append("，业务键去重=").Append(scope.DedupeDroppedCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                sb.Append("，有效行(更新前)=").Append(scope.TargetBefore.ToString(CultureInfo.InvariantCulture));
                sb.Append("，有效行(更新后)=").Append(scope.TargetAfter.ToString(CultureInfo.InvariantCulture));
                sb.Append("，物理行=").Append(scope.TargetPhysical.ToString(CultureInfo.InvariantCulture));
                sb.Append("，软删合计(is_deleted=1)=").Append(scope.SoftDeleted.ToString(CultureInfo.InvariantCulture));
                sb.Append("，真正新增=").Append(scope.InsertCount.ToString(CultureInfo.InvariantCulture));
                sb.Append("，真正更新=").Append(scope.UpdateCount.ToString(CultureInfo.InvariantCulture));
                sb.Append("，未变=").Append(scope.UnchangedCount.ToString(CultureInfo.InvariantCulture));
                sb.Append("，本轮软删=").Append(scope.DeleteCount.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrWhiteSpace(scope.SoftDeletedKeys))
                {
                    sb.Append("，软删明细(id|工厂/物料/工作中心)=").Append(scope.SoftDeletedKeys);
                }
            }
            return sb.ToString();
        }
        if (legacyPairs.Count > 0)
        {
            var parts = legacyPairs
                .OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
                .Select(x => $"{x.Key}={x.Value.ToString(CultureInfo.InvariantCulture)}");
            return $"SQL 脚本执行成功，路径={path}，{string.Join("，", parts)}";
        }
        return $"SQL 脚本执行成功，路径={path}（无行数汇总结果集；请确认脚本末尾输出 summary_tag={TaktQuartzConstants.SqlSyncSummaryTag}）";
    }

    /// <summary>
    /// 将 scope 码转为中文标签
    /// </summary>
    private static string MapScopeLabel(string scope)
    {
        return scope.Trim().ToLowerInvariant() switch
        {
            "main" => "主表",
            "detail" => "子表",
            _ => scope.Trim(),
        };
    }

    private static bool HasColumn(DbDataReader reader, string name)
    {
        for (var i = 0; i < reader.FieldCount; i++)
        {
            if (string.Equals(reader.GetName(i), name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    private static string ReadString(DbDataReader reader, string column)
    {
        if (!HasColumn(reader, column) || reader[column] is DBNull or null)
        {
            return string.Empty;
        }
        return Convert.ToString(reader[column], CultureInfo.InvariantCulture)?.Trim() ?? string.Empty;
    }

    private static int ReadInt(DbDataReader reader, string column)
    {
        if (!HasColumn(reader, column) || reader[column] is DBNull or null)
        {
            return 0;
        }
        return Convert.ToInt32(reader[column], CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 单 scope 行数汇总
    /// </summary>
    private sealed class SyncScopeCounts
    {
        public string Scope { get; init; } = string.Empty;
        public int? SourceRawCount { get; init; }
        public int SourceCount { get; init; }
        public int? SkippedEmptyCount { get; init; }
        public int? ModelBackfilledCount { get; init; }
        public int? MaterialTypeBackfilledCount { get; init; }
        public int? AverageUpdatedCount { get; init; }
        public int? DedupeDroppedCount { get; init; }
        public int TargetBefore { get; init; }
        public int TargetAfter { get; init; }
        public int TargetPhysical { get; init; }
        public int SoftDeleted { get; init; }
        public int InsertCount { get; init; }
        public int UpdateCount { get; init; }
        public int UnchangedCount { get; init; }
        public int DeleteCount { get; init; }
        public string SoftDeletedKeys { get; init; } = string.Empty;
    }
}
