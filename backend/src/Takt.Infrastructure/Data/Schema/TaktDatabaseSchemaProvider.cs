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
/// 实现 ITaktDatabaseSchemaProvider，通过 SqlSugar 对租户业务库做 Schema introspect
/// 连接来源：TaktConfigurationExtensions.GetTenantConnections（Database:TenantCodes 配置节）
/// 实体类型扫描：Takt.Domain 中继承 TaktTenantEntityBase、TaktCompanyEntityBase、
/// TaktApprovalEntityBase 的非抽象类，供 CodeFirst 建表与代码生成选型
/// 消费方：TaktDatabaseInfoService、
/// TaktGenWorkflowService
/// </summary>
public class TaktDatabaseSchemaProvider : ITaktDatabaseSchemaProvider
{
    /// <summary>
    /// Domain 程序集中可 CodeFirst 的实体类型（启动时反射扫描，按全名排序）
    /// </summary>
    private static readonly Type[] EntityTypes = typeof(TaktTenantEntityBase).Assembly
        .GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract &&
                    (typeof(TaktTenantEntityBase).IsAssignableFrom(t) ||
                     typeof(TaktCompanyEntityBase).IsAssignableFrom(t) ||
                     typeof(TaktApprovalEntityBase).IsAssignableFrom(t)))
        .OrderBy(t => t.FullName, StringComparer.Ordinal)
        .ToArray();

    /// <summary>应用配置（读取租户连接字符串）</summary>
    private readonly IConfiguration _configuration;
    /// <summary>已解析的 SqlSugar 数据库类型（构造时映射一次）</summary>
    private readonly DbType _sugarDbType;

    /// <summary>
    /// 初始化数据库 Schema 提供者
    /// </summary>
    /// <param name="configuration">应用配置（须包含 Database:TenantCodes 与连接字符串模板）</param>
    /// <exception cref="ArgumentNullException">configuration 为 null 时抛出</exception>
    public TaktDatabaseSchemaProvider(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _sugarDbType = configuration.GetSugarDbType();
    }

    /// <summary>
    /// 获取可 introspect 的租户业务库列表
    /// 从 TaktConfigurationExtensions.GetTenantConnections 解析连接，DisplayName 取自连接串 Database= 段
    /// </summary>
    /// <returns>租户编码与数据库展示名摘要列表</returns>
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

    /// <summary>
    /// 获取指定租户库下所有用户表
    /// 通过 SqlSugar DbMaintenance.GetTableInfoList 读取物理表元数据（不含视图）
    /// </summary>
    /// <param name="tenantCode">租户编码（3 位，须在 Database:TenantCodes 中配置）</param>
    /// <returns>SqlSugar 表元数据列表；无表时返回空列表</returns>
    /// <exception cref="InvalidOperationException">未找到对应租户连接字符串时抛出</exception>
    public Task<IReadOnlyList<DbTableInfo>> GetTablesAsync(string tenantCode)
    {
        using var db = CreateClient(tenantCode);
        var tables = db.DbMaintenance.GetTableInfoList(false) ?? new List<DbTableInfo>();
        return Task.FromResult<IReadOnlyList<DbTableInfo>>(tables);
    }

    /// <summary>
    /// 获取指定表的列元数据
    /// 通过 SqlSugar 读取列定义并映射为 TaktDatabaseTableColumnInfo
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="tableName">物理表名（大小写不敏感由数据库/SqlSugar 决定）</param>
    /// <returns>列名、类型、主键、自增、可空等摘要列表；无列时返回空列表</returns>
    /// <exception cref="InvalidOperationException">未找到对应租户连接字符串时抛出</exception>
    public Task<IReadOnlyList<TaktDatabaseTableColumnInfo>> GetColumnsAsync(string tenantCode, string tableName)
    {
        using var db = CreateClient(tenantCode);
        var columns = db.DbMaintenance.GetColumnInfosByTableName(tableName, false) ?? new List<DbColumnInfo>();
        var list = columns.Select(MapColumnInfo).ToList();
        return Task.FromResult<IReadOnlyList<TaktDatabaseTableColumnInfo>>(list);
    }

    /// <summary>
    /// 获取表注释（SqlSugar 表 Description 字段）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="tableName">物理表名</param>
    /// <returns>表注释；表不存在或无注释时返回 null</returns>
    /// <exception cref="InvalidOperationException">未找到对应租户连接字符串时抛出</exception>
    public Task<string?> GetTableCommentAsync(string tenantCode, string tableName)
    {
        using var db = CreateClient(tenantCode);
        var tables = db.DbMaintenance.GetTableInfoList(false);
        var table = tables?.FirstOrDefault(t =>
            string.Equals(t.Name, tableName, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(table?.Description);
    }

    /// <summary>
    /// 按实体类型在指定租户库 CodeFirst 建表
    /// 优先从启动时扫描的 EntityTypes 解析类型，否则尝试 Type.GetType；建库后 InitTables 单表初始化
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="entityTypeFullName">实体类型全名（如 Takt.Domain.Entities.Identity.TaktUser）</param>
    /// <returns>表示建表完成的任务</returns>
    /// <exception cref="InvalidOperationException">未找到实体类型或租户连接字符串时抛出</exception>
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

    /// <summary>
    /// 获取 Domain 已加载的实体基类派生类型全名
    /// 范围：继承 TaktTenantEntityBase、TaktCompanyEntityBase、
    /// TaktApprovalEntityBase 的非抽象类，按全名字典序
    /// </summary>
    /// <returns>可用于 CodeFirst 建表与代码生成选型的类型全名列表</returns>
    public Task<IReadOnlyList<string>> GetAvailableEntityTypeFullNamesAsync()
    {
        var names = EntityTypes
            .Select(t => t.FullName!)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .ToList();
        return Task.FromResult<IReadOnlyList<string>>(names);
    }

    /// <summary>
    /// 按租户编码创建 SqlSugar 客户端（短生命周期，调用方 using 释放）
    /// </summary>
    /// <param name="tenantCode">租户编码（3 位，与 Database:TenantCodes 项对应）</param>
    /// <returns>已配置连接字符串的 SqlSugar 客户端</returns>
    /// <exception cref="InvalidOperationException">未找到对应租户连接字符串时抛出</exception>
    private SqlSugarClient CreateClient(string tenantCode)
    {
        var match = ResolveTenantConnections()
            .FirstOrDefault(x => string.Equals(x.TenantCode, tenantCode, StringComparison.OrdinalIgnoreCase));
        if (string.IsNullOrWhiteSpace(match.ConnectionString))
        {
            throw new InvalidOperationException($"未找到 TenantCode={tenantCode} 对应的租户连接字符串。");
        }
        return TaktSqlSugarConnectionHelper.CreateClient(_sugarDbType, tenantCode, match.ConnectionString);
    }

    /// <summary>
    /// 从配置解析所有租户业务库连接（租户编码 + 已替换占位符的连接字符串）
    /// </summary>
    /// <returns>租户连接元组列表</returns>
    private List<(string TenantCode, string ConnectionString)> ResolveTenantConnections()
    {
        return _configuration.GetTenantConnections();
    }

    /// <summary>
    /// 从连接字符串提取 Database= 段作为展示名；解析失败时回退为 Takt_{tenantCode}
    /// </summary>
    /// <param name="connectionString">租户业务库连接字符串</param>
    /// <param name="tenantCode">租户编码（用于回退展示名）</param>
    /// <returns>数据库展示名称</returns>
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

    /// <summary>
    /// 将 SqlSugar 列元数据映射为代码生成工作流列摘要 DTO
    /// </summary>
    /// <param name="col">SqlSugar DbColumnInfo</param>
    /// <returns>物理表列 Schema 摘要</returns>
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
