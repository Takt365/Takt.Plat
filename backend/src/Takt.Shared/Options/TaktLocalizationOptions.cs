// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Options
// 文件名称：TaktLocalizationOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：本地化配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 本地化配置选项
/// </summary>
public class TaktLocalizationOptions
{
    public const string SectionName = "Localization";

    /// <summary>
    /// 默认语言（appsettings Localization:DefaultCulture；租户无启用语言时兜底）
    /// </summary>
    public string DefaultCulture { get; set; } = null!;

    /// <summary>
    /// 资源文件路径
    /// </summary>
    public string ResourcesPath { get; set; } = null!;

    /// <summary>
    /// 是否从数据库加载翻译（true=数据库，false=resx文件）
    /// </summary>
    public bool UseDatabaseLocalization { get; set; }

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(DefaultCulture))
        {
            throw new InvalidOperationException($"{SectionName}:DefaultCulture 不能为空");
        }

        if (string.IsNullOrWhiteSpace(ResourcesPath))
        {
            throw new InvalidOperationException($"{SectionName}:ResourcesPath 不能为空");
        }
    }
}
