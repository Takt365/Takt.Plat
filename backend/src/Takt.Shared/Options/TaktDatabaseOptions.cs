// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Options
// 文件名称：TaktDatabaseOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库配置（按租户分库）及初始化编码解析
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Shared.Options;

/// <summary>
/// 数据库配置（<c>appsettings Database</c> 节；绑定入口 TaktConfigurationExtensions.RequireDatabase）
/// </summary>
public class TaktDatabaseOptions
{
    public const string SectionName = "Database";

    /// <summary>
    /// 数据库类型（0=MySql, 1=SqlServer, 2=Sqlite, 3=Oracle, 4=PostgreSQL, 5=Dm, 6=Kdbndp）
    /// </summary>
    public int DbType { get; set; }

    /// <summary>
    /// 需要初始化的租户编码列表（顺序与 appsettings 一致）
    /// </summary>
    public List<string> TenantCodes { get; set; } = null!;

    /// <summary>
    /// 需要初始化的公司编码列表（各租户内顺序与种子一致）
    /// </summary>
    public List<string> CompanyCodes { get; set; } = null!;

    /// <summary>
    /// 需要初始化的工厂编码列表（各租户内顺序与种子一致）
    /// </summary>
    public List<string> PlantCodes { get; set; } = null!;

    /// <summary>
    /// 连接字符串模板（{TenantCode} 占位）
    /// </summary>
    public string ConnectionStringTemplate { get; set; } = null!;

    /// <summary>
    /// 规范化列表与种子编码后执行 Validate
    /// </summary>
    public void NormalizeAndValidate()
    {
        TenantCodes = NormalizeCodeList(TenantCodes);
        CompanyCodes = NormalizeCodeList(CompanyCodes);
        PlantCodes = NormalizeCodeList(PlantCodes);
        Validate();
    }

    /// <summary>
    /// 默认租户（TenantCodes 首项；启动 DI 引导、演示主公司等）
    /// </summary>
    /// <returns>默认租户编码</returns>
    public string GetSeedTenantCode() => TenantCodes[0];

    /// <summary>
    /// 默认公司（CompanyCodes 首项；演示账号主公司、请假工作流演示范围等）
    /// </summary>
    /// <returns>默认公司编码</returns>
    public string GetSeedCompanyCode() => CompanyCodes[0];

    /// <summary>
    /// 获取 SqlSugar 的 DbType 枚举
    /// </summary>
    public SqlSugar.DbType GetSugarDbType()
    {
        return DbType switch
        {
            0 => SqlSugar.DbType.MySql,
            1 => SqlSugar.DbType.SqlServer,
            2 => SqlSugar.DbType.Sqlite,
            3 => SqlSugar.DbType.Oracle,
            4 => SqlSugar.DbType.PostgreSQL,
            5 => SqlSugar.DbType.Dm,
            6 => SqlSugar.DbType.Kdbndp,
            _ => throw new InvalidOperationException($"不支持的数据库类型: {DbType}")
        };
    }

    /// <summary>
    /// 根据租户编码获取连接字符串
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>连接字符串</returns>
    public string GetConnectionString(string tenantCode)
    {
        return ConnectionStringTemplate.Replace("{TenantCode}", tenantCode);
    }

    /// <summary>
    /// 创建 SqlSugar 客户端配置
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>ConnectionConfig</returns>
    public ConnectionConfig CreateConnectionConfig(string tenantCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        var trimmed = tenantCode.Trim();
        return new ConnectionConfig
        {
            ConfigId = trimmed,
            DbType = GetSugarDbType(),
            ConnectionString = GetConnectionString(trimmed),
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute,
            MoreSettings = new ConnMoreSettings
            {
                SqlServerCodeFirstNvarchar = true,
            },
        };
    }

    /// <summary>
    /// 验证配置（须先 NormalizeAndValidate 或保证列表已规范化）
    /// </summary>
    public void Validate()
    {
        _ = GetSugarDbType();
        if (TenantCodes.Count == 0)
        {
            throw new InvalidOperationException($"{SectionName}:TenantCodes 不能为空");
        }
        if (CompanyCodes.Count == 0)
        {
            throw new InvalidOperationException($"{SectionName}:CompanyCodes 不能为空");
        }
        if (PlantCodes.Count == 0)
        {
            throw new InvalidOperationException($"{SectionName}:PlantCodes 不能为空");
        }
        if (string.IsNullOrWhiteSpace(ConnectionStringTemplate))
        {
            throw new InvalidOperationException($"{SectionName}:ConnectionStringTemplate 未配置");
        }
    }

    /// <summary>
    /// 按配置编码顺序排列实体（仅保留配置中出现的项）
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="configuredCodes">配置编码列表</param>
    /// <param name="items">待排序实体</param>
    /// <param name="codeSelector">从实体读取编码</param>
    /// <returns>按配置顺序排列的实体列表</returns>
    public static IReadOnlyList<T> OrderByConfiguredCodes<T>(
        IReadOnlyList<string> configuredCodes,
        IEnumerable<T> items,
        Func<T, string> codeSelector)
    {
        ArgumentNullException.ThrowIfNull(configuredCodes);
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(codeSelector);
        var itemMap = items.ToDictionary(codeSelector, StringComparer.Ordinal);
        var ordered = new List<T>(configuredCodes.Count);
        foreach (var code in configuredCodes)
        {
            if (itemMap.TryGetValue(code, out var item))
            {
                ordered.Add(item);
            }
        }
        return ordered;
    }

    /// <summary>
    /// 去空白并剔除空项
    /// </summary>
    /// <param name="codes">原始编码列表</param>
    /// <returns>规范化后的列表</returns>
    private static List<string> NormalizeCodeList(List<string>? codes)
    {
        if (codes == null || codes.Count == 0)
        {
            return [];
        }
        return codes
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Select(code => code.Trim())
            .ToList();
    }

}
