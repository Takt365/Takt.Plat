// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktQuartzLogI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQuartzLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging;

/// <summary>
/// TaktQuartzLog 实体国际化翻译种子（键前缀 entity.quartzLog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQuartzLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQuartzLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 quartzLog 实体翻译...", tenantCode);

        foreach (var item in GetQuartzLogTranslations())
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

        TaktLogger.Information("TaktQuartzLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQuartzLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.quartzLog._self / entity.quartzLog.{{field}}；ResourceGroup=TaktModule.Statistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQuartzLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.quartzLog._self
            new TranslationSeedItem("entity.quartzLog._self", "en-US", "Quartz Log Information", "实体名称"),
            // entity.quartzLog._self
            new TranslationSeedItem("entity.quartzLog._self", "ja-JP", "Quartz 任务执行日志信息", "实体名称"),
            // entity.quartzLog._self
            new TranslationSeedItem("entity.quartzLog._self", "zh-CN", "Quartz 任务执行日志信息", "实体名称"),
            // entity.quartzLog._self
            new TranslationSeedItem("entity.quartzLog._self", "zh-HK", "Quartz 任务执行日志信息", "实体名称"),

            // entity.quartzLog.quartztaskid
            new TranslationSeedItem("entity.quartzLog.quartztaskid", "en-US", "定时任务ID", "关联定时任务 ID"),
            // entity.quartzLog.quartztaskid
            new TranslationSeedItem("entity.quartzLog.quartztaskid", "ja-JP", "定时任务ID", "关联定时任务 ID"),
            // entity.quartzLog.quartztaskid
            new TranslationSeedItem("entity.quartzLog.quartztaskid", "zh-CN", "定时任务ID", "关联定时任务 ID"),
            // entity.quartzLog.quartztaskid
            new TranslationSeedItem("entity.quartzLog.quartztaskid", "zh-HK", "定时任务ID", "关联定时任务 ID"),

            // entity.quartzLog.username
            new TranslationSeedItem("entity.quartzLog.username", "en-US", "触发用户", "触发用户（系统任务为 system）"),
            // entity.quartzLog.username
            new TranslationSeedItem("entity.quartzLog.username", "ja-JP", "触发用户", "触发用户（系统任务为 system）"),
            // entity.quartzLog.username
            new TranslationSeedItem("entity.quartzLog.username", "zh-CN", "触发用户", "触发用户（系统任务为 system）"),
            // entity.quartzLog.username
            new TranslationSeedItem("entity.quartzLog.username", "zh-HK", "触发用户", "触发用户（系统任务为 system）"),

            // entity.quartzLog.jobname
            new TranslationSeedItem("entity.quartzLog.jobname", "en-US", "Job名称", "Job 名称"),
            // entity.quartzLog.jobname
            new TranslationSeedItem("entity.quartzLog.jobname", "ja-JP", "Job名称", "Job 名称"),
            // entity.quartzLog.jobname
            new TranslationSeedItem("entity.quartzLog.jobname", "zh-CN", "Job名称", "Job 名称"),
            // entity.quartzLog.jobname
            new TranslationSeedItem("entity.quartzLog.jobname", "zh-HK", "Job名称", "Job 名称"),

            // entity.quartzLog.jobgroup
            new TranslationSeedItem("entity.quartzLog.jobgroup", "en-US", "Job分组", "Job 分组"),
            // entity.quartzLog.jobgroup
            new TranslationSeedItem("entity.quartzLog.jobgroup", "ja-JP", "Job分组", "Job 分组"),
            // entity.quartzLog.jobgroup
            new TranslationSeedItem("entity.quartzLog.jobgroup", "zh-CN", "Job分组", "Job 分组"),
            // entity.quartzLog.jobgroup
            new TranslationSeedItem("entity.quartzLog.jobgroup", "zh-HK", "Job分组", "Job 分组"),

            // entity.quartzLog.triggername
            new TranslationSeedItem("entity.quartzLog.triggername", "en-US", "Trigger名称", "Trigger 名称"),
            // entity.quartzLog.triggername
            new TranslationSeedItem("entity.quartzLog.triggername", "ja-JP", "Trigger名称", "Trigger 名称"),
            // entity.quartzLog.triggername
            new TranslationSeedItem("entity.quartzLog.triggername", "zh-CN", "Trigger名称", "Trigger 名称"),
            // entity.quartzLog.triggername
            new TranslationSeedItem("entity.quartzLog.triggername", "zh-HK", "Trigger名称", "Trigger 名称"),

            // entity.quartzLog.executestatus
            new TranslationSeedItem("entity.quartzLog.executestatus", "en-US", "执行状态", "执行状态（0=成功，1=失败）"),
            // entity.quartzLog.executestatus
            new TranslationSeedItem("entity.quartzLog.executestatus", "ja-JP", "执行状态", "执行状态（0=成功，1=失败）"),
            // entity.quartzLog.executestatus
            new TranslationSeedItem("entity.quartzLog.executestatus", "zh-CN", "执行状态", "执行状态（0=成功，1=失败）"),
            // entity.quartzLog.executestatus
            new TranslationSeedItem("entity.quartzLog.executestatus", "zh-HK", "执行状态", "执行状态（0=成功，1=失败）"),

            // entity.quartzLog.errormsg
            new TranslationSeedItem("entity.quartzLog.errormsg", "en-US", "错误消息", "错误消息"),
            // entity.quartzLog.errormsg
            new TranslationSeedItem("entity.quartzLog.errormsg", "ja-JP", "错误消息", "错误消息"),
            // entity.quartzLog.errormsg
            new TranslationSeedItem("entity.quartzLog.errormsg", "zh-CN", "错误消息", "错误消息"),
            // entity.quartzLog.errormsg
            new TranslationSeedItem("entity.quartzLog.errormsg", "zh-HK", "错误消息", "错误消息"),

            // entity.quartzLog.executetime
            new TranslationSeedItem("entity.quartzLog.executetime", "en-US", "执行时间", "执行时间"),
            // entity.quartzLog.executetime
            new TranslationSeedItem("entity.quartzLog.executetime", "ja-JP", "执行时间", "执行时间"),
            // entity.quartzLog.executetime
            new TranslationSeedItem("entity.quartzLog.executetime", "zh-CN", "执行时间", "执行时间"),
            // entity.quartzLog.executetime
            new TranslationSeedItem("entity.quartzLog.executetime", "zh-HK", "执行时间", "执行时间"),

            // entity.quartzLog.costtime
            new TranslationSeedItem("entity.quartzLog.costtime", "en-US", "耗时毫秒", "耗时（毫秒）"),
            // entity.quartzLog.costtime
            new TranslationSeedItem("entity.quartzLog.costtime", "ja-JP", "耗时毫秒", "耗时（毫秒）"),
            // entity.quartzLog.costtime
            new TranslationSeedItem("entity.quartzLog.costtime", "zh-CN", "耗时毫秒", "耗时（毫秒）"),
            // entity.quartzLog.costtime
            new TranslationSeedItem("entity.quartzLog.costtime", "zh-HK", "耗时毫秒", "耗时（毫秒）"),
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
        translation.ResourceGroup = TaktModule.Statistics;
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
