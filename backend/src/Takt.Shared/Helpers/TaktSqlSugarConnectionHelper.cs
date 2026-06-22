// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktSqlSugarConnectionHelper.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SqlSugar 租户连接配置统一工厂（DbType 由调用方一次解析后传入）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Shared.Helpers;

/// <summary>
/// SqlSugar 连接配置工厂（避免各处重复拼装 ConnectionConfig）
/// </summary>
public static class TaktSqlSugarConnectionHelper
{
    /// <summary>
    /// 创建租户 SqlSugar 连接配置
    /// </summary>
    /// <param name="sugarDbType">已解析的 SqlSugar 数据库类型（来自 TaktDatabaseOptions.SugarDbType）</param>
    /// <param name="tenantCode">租户编码（ConfigId）</param>
    /// <param name="connectionString">连接字符串</param>
    /// <param name="registerRepositorySqlFunc">是否注册仓储聚合 SqlFunc 扩展</param>
    /// <returns>ConnectionConfig</returns>
    public static ConnectionConfig CreateConnectionConfig(
        DbType sugarDbType,
        string tenantCode,
        string connectionString,
        bool registerRepositorySqlFunc = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        var config = new ConnectionConfig
        {
            ConfigId = tenantCode.Trim(),
            DbType = sugarDbType,
            ConnectionString = connectionString,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute,
            MoreSettings = new ConnMoreSettings
            {
                SqlServerCodeFirstNvarchar = true,
            },
        };
        if (registerRepositorySqlFunc)
        {
            TaktSqlFuncRegistrationHelper.ApplyRepositorySqlFuncExtensions(config);
        }

        return config;
    }

    /// <summary>
    /// 创建租户 SqlSugar 客户端
    /// </summary>
    /// <param name="sugarDbType">已解析的 SqlSugar 数据库类型</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="connectionString">连接字符串</param>
    /// <param name="registerRepositorySqlFunc">是否注册仓储聚合 SqlFunc 扩展</param>
    /// <returns>SqlSugar 客户端</returns>
    public static SqlSugarClient CreateClient(
        DbType sugarDbType,
        string tenantCode,
        string connectionString,
        bool registerRepositorySqlFunc = true) =>
        new(CreateConnectionConfig(sugarDbType, tenantCode, connectionString, registerRepositorySqlFunc));
}
