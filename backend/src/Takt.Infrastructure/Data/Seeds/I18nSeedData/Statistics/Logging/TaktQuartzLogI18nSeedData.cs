// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktQuartzLogI18nSeedData.cs
// 创建时间：2026-06-08
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

            // entity.quartzLog.taskname
            new TranslationSeedItem("entity.quartzLog.taskname", "en-US", "任务名称", "任务名称（执行时快照）"),
            // entity.quartzLog.taskname
            new TranslationSeedItem("entity.quartzLog.taskname", "ja-JP", "任务名称", "任务名称（执行时快照）"),
            // entity.quartzLog.taskname
            new TranslationSeedItem("entity.quartzLog.taskname", "zh-CN", "任务名称", "任务名称（执行时快照）"),
            // entity.quartzLog.taskname
            new TranslationSeedItem("entity.quartzLog.taskname", "zh-HK", "任务名称", "任务名称（执行时快照）"),

            // entity.quartzLog.jobgroup
            new TranslationSeedItem("entity.quartzLog.jobgroup", "en-US", "任务组名", "任务组名（执行时快照）"),
            // entity.quartzLog.jobgroup
            new TranslationSeedItem("entity.quartzLog.jobgroup", "ja-JP", "任务组名", "任务组名（执行时快照）"),
            // entity.quartzLog.jobgroup
            new TranslationSeedItem("entity.quartzLog.jobgroup", "zh-CN", "任务组名", "任务组名（执行时快照）"),
            // entity.quartzLog.jobgroup
            new TranslationSeedItem("entity.quartzLog.jobgroup", "zh-HK", "任务组名", "任务组名（执行时快照）"),

            // entity.quartzLog.tasktype
            new TranslationSeedItem("entity.quartzLog.tasktype", "en-US", "任务类型", "任务类型（1=程序集 2=网络请求 3=SQL语句）"),
            // entity.quartzLog.tasktype
            new TranslationSeedItem("entity.quartzLog.tasktype", "ja-JP", "任务类型", "任务类型（1=程序集 2=网络请求 3=SQL语句）"),
            // entity.quartzLog.tasktype
            new TranslationSeedItem("entity.quartzLog.tasktype", "zh-CN", "任务类型", "任务类型（1=程序集 2=网络请求 3=SQL语句）"),
            // entity.quartzLog.tasktype
            new TranslationSeedItem("entity.quartzLog.tasktype", "zh-HK", "任务类型", "任务类型（1=程序集 2=网络请求 3=SQL语句）"),

            // entity.quartzLog.executetime
            new TranslationSeedItem("entity.quartzLog.executetime", "en-US", "执行时间", "执行时间"),
            // entity.quartzLog.executetime
            new TranslationSeedItem("entity.quartzLog.executetime", "ja-JP", "执行时间", "执行时间"),
            // entity.quartzLog.executetime
            new TranslationSeedItem("entity.quartzLog.executetime", "zh-CN", "执行时间", "执行时间"),
            // entity.quartzLog.executetime
            new TranslationSeedItem("entity.quartzLog.executetime", "zh-HK", "执行时间", "执行时间"),

            // entity.quartzLog.executeduration
            new TranslationSeedItem("entity.quartzLog.executeduration", "en-US", "执行耗时（毫秒）", "执行耗时（毫秒）"),
            // entity.quartzLog.executeduration
            new TranslationSeedItem("entity.quartzLog.executeduration", "ja-JP", "执行耗时（毫秒）", "执行耗时（毫秒）"),
            // entity.quartzLog.executeduration
            new TranslationSeedItem("entity.quartzLog.executeduration", "zh-CN", "执行耗时（毫秒）", "执行耗时（毫秒）"),
            // entity.quartzLog.executeduration
            new TranslationSeedItem("entity.quartzLog.executeduration", "zh-HK", "执行耗时（毫秒）", "执行耗时（毫秒）"),

            // entity.quartzLog.executeparams
            new TranslationSeedItem("entity.quartzLog.executeparams", "en-US", "执行参数", "执行参数"),
            // entity.quartzLog.executeparams
            new TranslationSeedItem("entity.quartzLog.executeparams", "ja-JP", "执行参数", "执行参数"),
            // entity.quartzLog.executeparams
            new TranslationSeedItem("entity.quartzLog.executeparams", "zh-CN", "执行参数", "执行参数"),
            // entity.quartzLog.executeparams
            new TranslationSeedItem("entity.quartzLog.executeparams", "zh-HK", "执行参数", "执行参数"),

            // entity.quartzLog.executemessage
            new TranslationSeedItem("entity.quartzLog.executemessage", "en-US", "执行消息", "执行消息"),
            // entity.quartzLog.executemessage
            new TranslationSeedItem("entity.quartzLog.executemessage", "ja-JP", "执行消息", "执行消息"),
            // entity.quartzLog.executemessage
            new TranslationSeedItem("entity.quartzLog.executemessage", "zh-CN", "执行消息", "执行消息"),
            // entity.quartzLog.executemessage
            new TranslationSeedItem("entity.quartzLog.executemessage", "zh-HK", "执行消息", "执行消息"),

            // entity.quartzLog.errorinfo
            new TranslationSeedItem("entity.quartzLog.errorinfo", "en-US", "错误信息", "错误信息"),
            // entity.quartzLog.errorinfo
            new TranslationSeedItem("entity.quartzLog.errorinfo", "ja-JP", "错误信息", "错误信息"),
            // entity.quartzLog.errorinfo
            new TranslationSeedItem("entity.quartzLog.errorinfo", "zh-CN", "错误信息", "错误信息"),
            // entity.quartzLog.errorinfo
            new TranslationSeedItem("entity.quartzLog.errorinfo", "zh-HK", "错误信息", "错误信息"),

            // entity.quartzLog.executeip
            new TranslationSeedItem("entity.quartzLog.executeip", "en-US", "执行机器IP", "执行机器 IP"),
            // entity.quartzLog.executeip
            new TranslationSeedItem("entity.quartzLog.executeip", "ja-JP", "执行机器IP", "执行机器 IP"),
            // entity.quartzLog.executeip
            new TranslationSeedItem("entity.quartzLog.executeip", "zh-CN", "执行机器IP", "执行机器 IP"),
            // entity.quartzLog.executeip
            new TranslationSeedItem("entity.quartzLog.executeip", "zh-HK", "执行机器IP", "执行机器 IP"),

            // entity.quartzLog.executehost
            new TranslationSeedItem("entity.quartzLog.executehost", "en-US", "执行机器名", "执行机器名"),
            // entity.quartzLog.executehost
            new TranslationSeedItem("entity.quartzLog.executehost", "ja-JP", "执行机器名", "执行机器名"),
            // entity.quartzLog.executehost
            new TranslationSeedItem("entity.quartzLog.executehost", "zh-CN", "执行机器名", "执行机器名"),
            // entity.quartzLog.executehost
            new TranslationSeedItem("entity.quartzLog.executehost", "zh-HK", "执行机器名", "执行机器名"),

            // entity.quartzLog.executestatus
            new TranslationSeedItem("entity.quartzLog.executestatus", "en-US", "执行状态", "执行状态（0=失败，1=成功）"),
            // entity.quartzLog.executestatus
            new TranslationSeedItem("entity.quartzLog.executestatus", "ja-JP", "执行状态", "执行状态（0=失败，1=成功）"),
            // entity.quartzLog.executestatus
            new TranslationSeedItem("entity.quartzLog.executestatus", "zh-CN", "执行状态", "执行状态（0=失败，1=成功）"),
            // entity.quartzLog.executestatus
            new TranslationSeedItem("entity.quartzLog.executestatus", "zh-HK", "执行状态", "执行状态（0=失败，1=成功）"),

            // entity.quartzLog.quartztask
            new TranslationSeedItem("entity.quartzLog.quartztask", "en-US", "关联的定时任务", "关联的定时任务"),
            // entity.quartzLog.quartztask
            new TranslationSeedItem("entity.quartzLog.quartztask", "ja-JP", "关联的定时任务", "关联的定时任务"),
            // entity.quartzLog.quartztask
            new TranslationSeedItem("entity.quartzLog.quartztask", "zh-CN", "关联的定时任务", "关联的定时任务"),
            // entity.quartzLog.quartztask
            new TranslationSeedItem("entity.quartzLog.quartztask", "zh-HK", "关联的定时任务", "关联的定时任务"),
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
