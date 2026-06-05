// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Schema
// 文件名称：TaktDatabaseSchemaProvider.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成工作流数据库元数据提供者（按 Database:TenantCodes 连接 introspect）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Code;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Schema;

/// <summary>
/// 代码生成工作流数据库元数据提供者
/// </summary>
public class TaktDatabaseSchemaProvider : ITaktDatabaseSchemaProvider
{
    private static readonly Type[] EntityTypes = typeof(TaktTenantEntityBase).Assembly
        .GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract &&
                    (typeof(TaktTenantEntityBase).IsAssignableFrom(t) ||
                     typeof(TaktCompanyEntityBase).IsAssignableFrom(t) ||
                     typeof(TaktApprovalEntityBase).IsAssignableFrom(t)))
        .OrderBy(t => t.FullName, StringComparer.Ordinal)
        .ToArray();

    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration">应用配置</param>
    public TaktDatabaseSchemaProvider(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<TaktDatabaseInfo>> GetDatabasesAsync()
    {
        var list = ResolveTenantConnections()
            .Select(x => new TaktDatabaseInfo
            {
                TenantCode = x.TenantCode,
                DisplayName = ExtractDatabaseDisplayName(x.ConnectionString, x.TenantCode)
            })
            .ToList();
        return Task.FromResult<IReadOnlyList<TaktDatabaseInfo>>(list);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<DbTableInfo>> GetTablesAsync(string tenantCode)
    {
        using var db = CreateClient(tenantCode);
        var tables = db.DbMaintenance.GetTableInfoList(false) ?? new List<DbTableInfo>();
        return Task.FromResult<IReadOnlyList<DbTableInfo>>(tables);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<TaktDatabaseTableColumnInfo>> GetColumnsAsync(string tenantCode, string tableName)
    {
        using var db = CreateClient(tenantCode);
        var columns = db.DbMaintenance.GetColumnInfosByTableName(tableName, false) ?? new List<DbColumnInfo>();
        var list = columns.Select(MapColumnInfo).ToList();
        return Task.FromResult<IReadOnlyList<TaktDatabaseTableColumnInfo>>(list);
    }

    /// <inheritdoc />
    public Task<string?> GetTableCommentAsync(string tenantCode, string tableName)
    {
        using var db = CreateClient(tenantCode);
        var tables = db.DbMaintenance.GetTableInfoList(false);
        var table = tables?.FirstOrDefault(t =>
            string.Equals(t.Name, tableName, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(table?.Description);
    }

    /// <inheritdoc />
    public Task InitializeTableFromEntityTypeAsync(string tenantCode, string entityTypeFullName)
    {
        var entityType = EntityTypes.FirstOrDefault(t =>
            string.Equals(t.FullName, entityTypeFullName, StringComparison.Ordinal))
            ?? Type.GetType(entityTypeFullName, throwOnError: false)
            ?? throw new InvalidOperationException($"未找到实体类型：{entityTypeFullName}");
        using var db = CreateClient(tenantCode);
        db.DbMaintenance.CreateDatabase();
        db.CodeFirst.InitTables(entityType);
        TaktLogger.Information("[DatabaseSchemaProvider] 已按实体建表: {EntityType}", entityTypeFullName);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<string>> GetAvailableEntityTypeFullNamesAsync()
    {
        var names = EntityTypes
            .Select(t => t.FullName!)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .ToList();
        return Task.FromResult<IReadOnlyList<string>>(names);
    }

    private SqlSugarClient CreateClient(string tenantCode)
    {
        var match = ResolveTenantConnections()
            .FirstOrDefault(x => string.Equals(x.TenantCode, tenantCode, StringComparison.OrdinalIgnoreCase));
        if (string.IsNullOrWhiteSpace(match.ConnectionString))
        {
            throw new InvalidOperationException($"未找到 TenantCode={tenantCode} 对应的租户连接字符串。");
        }
        return new SqlSugarClient(new ConnectionConfig
        {
            ConfigId = tenantCode,
            ConnectionString = match.ConnectionString,
            DbType = DbType.SqlServer,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });
    }

    private List<(string TenantCode, string ConnectionString)> ResolveTenantConnections()
    {
        return _configuration.GetTenantConnections();
    }

    private static string ExtractDatabaseDisplayName(string connectionString, string tenantCode)
    {
        const string key = "Database=";
        var idx = connectionString.IndexOf(key, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
        {
            return $"Takt_{tenantCode}";
        }
        var start = idx + key.Length;
        var end = connectionString.IndexOf(';', start);
        var dbName = end > start
            ? connectionString[start..end]
            : connectionString[start..];
        return string.IsNullOrWhiteSpace(dbName) ? $"Takt_{tenantCode}" : dbName;
    }

    private static TaktDatabaseTableColumnInfo MapColumnInfo(DbColumnInfo col)
    {
        return new TaktDatabaseTableColumnInfo
        {
            DatabaseColumnName = col.DbColumnName ?? string.Empty,
            ColumnComment = col.ColumnDescription,
            DatabaseDataType = col.DataType ?? string.Empty,
            Length = col.Length,
            DecimalDigits = col.DecimalDigits,
            IsPrimaryKey = col.IsPrimarykey,
            IsIdentity = col.IsIdentity,
            IsNullable = col.IsNullable
        };
    }
}
