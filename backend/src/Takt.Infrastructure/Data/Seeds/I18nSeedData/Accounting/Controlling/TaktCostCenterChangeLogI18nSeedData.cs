// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling
// 文件名称：TaktCostCenterChangeLogI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCostCenterChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Controlling;

/// <summary>
/// TaktCostCenterChangeLog 实体国际化翻译种子（键前缀 entity.costCenterChangeLog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCostCenterChangeLogI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（实体翻译种子，位于部门翻译之后）
    /// </summary>
    public int Order => 52;

    /// <summary>
    /// 初始化实体字段翻译种子
    /// </summary>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化 TaktCostCenterChangeLog 实体国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 costCenterChangeLog 实体翻译...", tenantCode);

        foreach (var item in GetCostCenterChangeLogTranslations())
        {
            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);
                continue;
            }

            var (translation, i, u) = await CreateOrUpdateTranslationAsync(
                repository,
                tenantCode,
                cultureId,
                item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("TaktCostCenterChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCostCenterChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.costCenterChangeLog._self / entity.costCenterChangeLog.{{field}}；ResourceGroup=TaktModule.Accounting；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCostCenterChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.costCenterChangeLog._self
            new TranslationSeedItem("entity.costCenterChangeLog._self", "en-US", "Cost Center Change Log Information", "实体名称"),
            // entity.costCenterChangeLog._self
            new TranslationSeedItem("entity.costCenterChangeLog._self", "ja-JP", "成本中心变更记录信息", "实体名称"),
            // entity.costCenterChangeLog._self
            new TranslationSeedItem("entity.costCenterChangeLog._self", "zh-CN", "成本中心变更记录信息", "实体名称"),
            // entity.costCenterChangeLog._self
            new TranslationSeedItem("entity.costCenterChangeLog._self", "zh-HK", "成本中心变更记录信息", "实体名称"),

            // entity.costCenterChangeLog.costcenterid
            new TranslationSeedItem("entity.costCenterChangeLog.costcenterid", "en-US", "成本中心ID", "成本中心 ID"),
            // entity.costCenterChangeLog.costcenterid
            new TranslationSeedItem("entity.costCenterChangeLog.costcenterid", "ja-JP", "成本中心ID", "成本中心 ID"),
            // entity.costCenterChangeLog.costcenterid
            new TranslationSeedItem("entity.costCenterChangeLog.costcenterid", "zh-CN", "成本中心ID", "成本中心 ID"),
            // entity.costCenterChangeLog.costcenterid
            new TranslationSeedItem("entity.costCenterChangeLog.costcenterid", "zh-HK", "成本中心ID", "成本中心 ID"),

            // entity.costCenterChangeLog.costcentercode
            new TranslationSeedItem("entity.costCenterChangeLog.costcentercode", "en-US", "成本中心编码", "成本中心编码（冗余）"),
            // entity.costCenterChangeLog.costcentercode
            new TranslationSeedItem("entity.costCenterChangeLog.costcentercode", "ja-JP", "成本中心编码", "成本中心编码（冗余）"),
            // entity.costCenterChangeLog.costcentercode
            new TranslationSeedItem("entity.costCenterChangeLog.costcentercode", "zh-CN", "成本中心编码", "成本中心编码（冗余）"),
            // entity.costCenterChangeLog.costcentercode
            new TranslationSeedItem("entity.costCenterChangeLog.costcentercode", "zh-HK", "成本中心编码", "成本中心编码（冗余）"),

            // entity.costCenterChangeLog.changefields
            new TranslationSeedItem("entity.costCenterChangeLog.changefields", "en-US", "变更字段列表", "变更字段列表 JSON"),
            // entity.costCenterChangeLog.changefields
            new TranslationSeedItem("entity.costCenterChangeLog.changefields", "ja-JP", "变更字段列表", "变更字段列表 JSON"),
            // entity.costCenterChangeLog.changefields
            new TranslationSeedItem("entity.costCenterChangeLog.changefields", "zh-CN", "变更字段列表", "变更字段列表 JSON"),
            // entity.costCenterChangeLog.changefields
            new TranslationSeedItem("entity.costCenterChangeLog.changefields", "zh-HK", "变更字段列表", "变更字段列表 JSON"),

            // entity.costCenterChangeLog.changetime
            new TranslationSeedItem("entity.costCenterChangeLog.changetime", "en-US", "变更时间", "变更时间"),
            // entity.costCenterChangeLog.changetime
            new TranslationSeedItem("entity.costCenterChangeLog.changetime", "ja-JP", "变更时间", "变更时间"),
            // entity.costCenterChangeLog.changetime
            new TranslationSeedItem("entity.costCenterChangeLog.changetime", "zh-CN", "变更时间", "变更时间"),
            // entity.costCenterChangeLog.changetime
            new TranslationSeedItem("entity.costCenterChangeLog.changetime", "zh-HK", "变更时间", "变更时间"),

            // entity.costCenterChangeLog.changeby
            new TranslationSeedItem("entity.costCenterChangeLog.changeby", "en-US", "变更人", "变更人"),
            // entity.costCenterChangeLog.changeby
            new TranslationSeedItem("entity.costCenterChangeLog.changeby", "ja-JP", "变更人", "变更人"),
            // entity.costCenterChangeLog.changeby
            new TranslationSeedItem("entity.costCenterChangeLog.changeby", "zh-CN", "变更人", "变更人"),
            // entity.costCenterChangeLog.changeby
            new TranslationSeedItem("entity.costCenterChangeLog.changeby", "zh-HK", "变更人", "变更人"),

            // entity.costCenterChangeLog.changereason
            new TranslationSeedItem("entity.costCenterChangeLog.changereason", "en-US", "变更原因", "变更原因"),
            // entity.costCenterChangeLog.changereason
            new TranslationSeedItem("entity.costCenterChangeLog.changereason", "ja-JP", "变更原因", "变更原因"),
            // entity.costCenterChangeLog.changereason
            new TranslationSeedItem("entity.costCenterChangeLog.changereason", "zh-CN", "变更原因", "变更原因"),
            // entity.costCenterChangeLog.changereason
            new TranslationSeedItem("entity.costCenterChangeLog.changereason", "zh-HK", "变更原因", "变更原因"),
        };
    }

    /// <summary>
    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）
    /// </summary>
    private static void ApplyTranslationFields(
        TaktTranslation translation,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        translation.TenantCode = tenantCode;
        translation.CultureId = cultureId;
        translation.CultureCode = item.CultureCode;
        translation.I18nKey = item.I18nKey;
        translation.TranslationText = item.TranslationText;
        translation.ResourceGroup = TaktModule.Accounting;
        translation.ResourceType = TaktAppSide.Frontend;
        translation.ContextNote = item.ContextNote;
        translation.ExtFieldJson = null;
        translation.Remark = null;
        translation.IsDeleted = 0;
        translation.DeletedBy = null;
        translation.DeletedAt = null;
    }

    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(
        ITaktTenantSeedRepository<TaktTranslation> repository,
        string tenantCode,
        long cultureId,
        TranslationSeedItem item)
    {
        var translation = await repository.FirstAsync(t =>
            t.TenantCode == tenantCode &&
            t.I18nKey == item.I18nKey &&
            t.CultureCode == item.CultureCode);

        if (translation == null)
        {
            translation = new TaktTranslation();
            ApplyTranslationFields(translation, tenantCode, cultureId, item);
            translation = await repository.CreateAsync(translation);
            return (translation, 1, 0);
        }

        ApplyTranslationFields(translation, tenantCode, cultureId, item);
        await repository.UpdateAsync(translation);
        return (translation, 0, 1);
    }

    /// <summary>
    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）
    /// </summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
