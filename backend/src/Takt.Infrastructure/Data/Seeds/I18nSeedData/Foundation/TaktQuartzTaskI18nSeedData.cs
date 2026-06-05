// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktQuartzTaskI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktQuartzTask 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktQuartzTask 实体国际化翻译种子（键前缀 entity.quartzTask.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktQuartzTaskI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktQuartzTask 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 quartzTask 实体翻译...", tenantCode);

        foreach (var item in GetQuartzTaskTranslations())
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

        TaktLogger.Information("TaktQuartzTask 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktQuartzTask 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.quartzTask._self / entity.quartzTask.{{field}}；ResourceGroup=TaktModule.Foundation；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetQuartzTaskTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.quartzTask._self
            new TranslationSeedItem("entity.quartzTask._self", "en-US", "Quartz Task Information", "实体名称"),
            // entity.quartzTask._self
            new TranslationSeedItem("entity.quartzTask._self", "ja-JP", "Quartz 定时任务信息", "实体名称"),
            // entity.quartzTask._self
            new TranslationSeedItem("entity.quartzTask._self", "zh-CN", "Quartz 定时任务信息", "实体名称"),
            // entity.quartzTask._self
            new TranslationSeedItem("entity.quartzTask._self", "zh-HK", "Quartz 定时任务信息", "实体名称"),

            // entity.quartzTask.taskcode
            new TranslationSeedItem("entity.quartzTask.taskcode", "en-US", "任务编码", "任务编码（租户+公司内唯一）"),
            // entity.quartzTask.taskcode
            new TranslationSeedItem("entity.quartzTask.taskcode", "ja-JP", "任务编码", "任务编码（租户+公司内唯一）"),
            // entity.quartzTask.taskcode
            new TranslationSeedItem("entity.quartzTask.taskcode", "zh-CN", "任务编码", "任务编码（租户+公司内唯一）"),
            // entity.quartzTask.taskcode
            new TranslationSeedItem("entity.quartzTask.taskcode", "zh-HK", "任务编码", "任务编码（租户+公司内唯一）"),

            // entity.quartzTask.taskname
            new TranslationSeedItem("entity.quartzTask.taskname", "en-US", "任务名称", "任务名称"),
            // entity.quartzTask.taskname
            new TranslationSeedItem("entity.quartzTask.taskname", "ja-JP", "任务名称", "任务名称"),
            // entity.quartzTask.taskname
            new TranslationSeedItem("entity.quartzTask.taskname", "zh-CN", "任务名称", "任务名称"),
            // entity.quartzTask.taskname
            new TranslationSeedItem("entity.quartzTask.taskname", "zh-HK", "任务名称", "任务名称"),

            // entity.quartzTask.jobname
            new TranslationSeedItem("entity.quartzTask.jobname", "en-US", "Job名称", "Quartz Job 名称"),
            // entity.quartzTask.jobname
            new TranslationSeedItem("entity.quartzTask.jobname", "ja-JP", "Job名称", "Quartz Job 名称"),
            // entity.quartzTask.jobname
            new TranslationSeedItem("entity.quartzTask.jobname", "zh-CN", "Job名称", "Quartz Job 名称"),
            // entity.quartzTask.jobname
            new TranslationSeedItem("entity.quartzTask.jobname", "zh-HK", "Job名称", "Quartz Job 名称"),

            // entity.quartzTask.jobgroup
            new TranslationSeedItem("entity.quartzTask.jobgroup", "en-US", "Job分组", "Quartz Job 分组"),
            // entity.quartzTask.jobgroup
            new TranslationSeedItem("entity.quartzTask.jobgroup", "ja-JP", "Job分组", "Quartz Job 分组"),
            // entity.quartzTask.jobgroup
            new TranslationSeedItem("entity.quartzTask.jobgroup", "zh-CN", "Job分组", "Quartz Job 分组"),
            // entity.quartzTask.jobgroup
            new TranslationSeedItem("entity.quartzTask.jobgroup", "zh-HK", "Job分组", "Quartz Job 分组"),

            // entity.quartzTask.cronexpression
            new TranslationSeedItem("entity.quartzTask.cronexpression", "en-US", "Cron表达式", "Cron 表达式"),
            // entity.quartzTask.cronexpression
            new TranslationSeedItem("entity.quartzTask.cronexpression", "ja-JP", "Cron表达式", "Cron 表达式"),
            // entity.quartzTask.cronexpression
            new TranslationSeedItem("entity.quartzTask.cronexpression", "zh-CN", "Cron表达式", "Cron 表达式"),
            // entity.quartzTask.cronexpression
            new TranslationSeedItem("entity.quartzTask.cronexpression", "zh-HK", "Cron表达式", "Cron 表达式"),

            // entity.quartzTask.jobtype
            new TranslationSeedItem("entity.quartzTask.jobtype", "en-US", "任务处理器", "任务处理器类型（DI 注册键或完整类型名）"),
            // entity.quartzTask.jobtype
            new TranslationSeedItem("entity.quartzTask.jobtype", "ja-JP", "任务处理器", "任务处理器类型（DI 注册键或完整类型名）"),
            // entity.quartzTask.jobtype
            new TranslationSeedItem("entity.quartzTask.jobtype", "zh-CN", "任务处理器", "任务处理器类型（DI 注册键或完整类型名）"),
            // entity.quartzTask.jobtype
            new TranslationSeedItem("entity.quartzTask.jobtype", "zh-HK", "任务处理器", "任务处理器类型（DI 注册键或完整类型名）"),

            // entity.quartzTask.jobparams
            new TranslationSeedItem("entity.quartzTask.jobparams", "en-US", "任务参数JSON", "任务参数 JSON"),
            // entity.quartzTask.jobparams
            new TranslationSeedItem("entity.quartzTask.jobparams", "ja-JP", "任务参数JSON", "任务参数 JSON"),
            // entity.quartzTask.jobparams
            new TranslationSeedItem("entity.quartzTask.jobparams", "zh-CN", "任务参数JSON", "任务参数 JSON"),
            // entity.quartzTask.jobparams
            new TranslationSeedItem("entity.quartzTask.jobparams", "zh-HK", "任务参数JSON", "任务参数 JSON"),

            // entity.quartzTask.taskstatus
            new TranslationSeedItem("entity.quartzTask.taskstatus", "en-US", "任务状态", "任务状态"),
            // entity.quartzTask.taskstatus
            new TranslationSeedItem("entity.quartzTask.taskstatus", "ja-JP", "任务状态", "任务状态"),
            // entity.quartzTask.taskstatus
            new TranslationSeedItem("entity.quartzTask.taskstatus", "zh-CN", "任务状态", "任务状态"),
            // entity.quartzTask.taskstatus
            new TranslationSeedItem("entity.quartzTask.taskstatus", "zh-HK", "任务状态", "任务状态"),

            // entity.quartzTask.lastrunat
            new TranslationSeedItem("entity.quartzTask.lastrunat", "en-US", "上次执行时间", "上次执行时间"),
            // entity.quartzTask.lastrunat
            new TranslationSeedItem("entity.quartzTask.lastrunat", "ja-JP", "上次执行时间", "上次执行时间"),
            // entity.quartzTask.lastrunat
            new TranslationSeedItem("entity.quartzTask.lastrunat", "zh-CN", "上次执行时间", "上次执行时间"),
            // entity.quartzTask.lastrunat
            new TranslationSeedItem("entity.quartzTask.lastrunat", "zh-HK", "上次执行时间", "上次执行时间"),

            // entity.quartzTask.nextrunat
            new TranslationSeedItem("entity.quartzTask.nextrunat", "en-US", "下次执行时间", "下次执行时间"),
            // entity.quartzTask.nextrunat
            new TranslationSeedItem("entity.quartzTask.nextrunat", "ja-JP", "下次执行时间", "下次执行时间"),
            // entity.quartzTask.nextrunat
            new TranslationSeedItem("entity.quartzTask.nextrunat", "zh-CN", "下次执行时间", "下次执行时间"),
            // entity.quartzTask.nextrunat
            new TranslationSeedItem("entity.quartzTask.nextrunat", "zh-HK", "下次执行时间", "下次执行时间"),

            // entity.quartzTask.description
            new TranslationSeedItem("entity.quartzTask.description", "en-US", "任务描述", "任务描述"),
            // entity.quartzTask.description
            new TranslationSeedItem("entity.quartzTask.description", "ja-JP", "任务描述", "任务描述"),
            // entity.quartzTask.description
            new TranslationSeedItem("entity.quartzTask.description", "zh-CN", "任务描述", "任务描述"),
            // entity.quartzTask.description
            new TranslationSeedItem("entity.quartzTask.description", "zh-HK", "任务描述", "任务描述"),
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
        translation.ResourceGroup = TaktModule.Foundation;
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
