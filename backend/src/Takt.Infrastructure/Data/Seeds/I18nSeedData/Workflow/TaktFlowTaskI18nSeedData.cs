// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowTaskI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowTask 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow;

/// <summary>
/// TaktFlowTask 实体国际化翻译种子（键前缀 entity.flowTask.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowTaskI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowTask 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowTask 实体翻译...", tenantCode);

        foreach (var item in GetFlowTaskTranslations())
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

        TaktLogger.Information("TaktFlowTask 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowTask 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowTask._self / entity.flowTask.{{field}}；ResourceGroup=TaktModule.Workflow；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowTaskTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowTask._self
            new TranslationSeedItem("entity.flowTask._self", "en-US", "Flow Task Information", "实体名称"),
            // entity.flowTask._self
            new TranslationSeedItem("entity.flowTask._self", "ja-JP", "流程用户任务信息", "实体名称"),
            // entity.flowTask._self
            new TranslationSeedItem("entity.flowTask._self", "zh-CN", "流程用户任务信息", "实体名称"),
            // entity.flowTask._self
            new TranslationSeedItem("entity.flowTask._self", "zh-HK", "流程用户任务信息", "实体名称"),

            // entity.flowTask.instanceid
            new TranslationSeedItem("entity.flowTask.instanceid", "en-US", "流程实例ID", "流程实例 ID"),
            // entity.flowTask.instanceid
            new TranslationSeedItem("entity.flowTask.instanceid", "ja-JP", "流程实例ID", "流程实例 ID"),
            // entity.flowTask.instanceid
            new TranslationSeedItem("entity.flowTask.instanceid", "zh-CN", "流程实例ID", "流程实例 ID"),
            // entity.flowTask.instanceid
            new TranslationSeedItem("entity.flowTask.instanceid", "zh-HK", "流程实例ID", "流程实例 ID"),

            // entity.flowTask.taskdefinitionkey
            new TranslationSeedItem("entity.flowTask.taskdefinitionkey", "en-US", "任务定义键", "任务定义键（设计器节点 nodeId）"),
            // entity.flowTask.taskdefinitionkey
            new TranslationSeedItem("entity.flowTask.taskdefinitionkey", "ja-JP", "任务定义键", "任务定义键（设计器节点 nodeId）"),
            // entity.flowTask.taskdefinitionkey
            new TranslationSeedItem("entity.flowTask.taskdefinitionkey", "zh-CN", "任务定义键", "任务定义键（设计器节点 nodeId）"),
            // entity.flowTask.taskdefinitionkey
            new TranslationSeedItem("entity.flowTask.taskdefinitionkey", "zh-HK", "任务定义键", "任务定义键（设计器节点 nodeId）"),

            // entity.flowTask.taskname
            new TranslationSeedItem("entity.flowTask.taskname", "en-US", "任务名称", "任务名称"),
            // entity.flowTask.taskname
            new TranslationSeedItem("entity.flowTask.taskname", "ja-JP", "任务名称", "任务名称"),
            // entity.flowTask.taskname
            new TranslationSeedItem("entity.flowTask.taskname", "zh-CN", "任务名称", "任务名称"),
            // entity.flowTask.taskname
            new TranslationSeedItem("entity.flowTask.taskname", "zh-HK", "任务名称", "任务名称"),

            // entity.flowTask.assigneeuserid
            new TranslationSeedItem("entity.flowTask.assigneeuserid", "en-US", "办理人ID", "办理人 ID"),
            // entity.flowTask.assigneeuserid
            new TranslationSeedItem("entity.flowTask.assigneeuserid", "ja-JP", "办理人ID", "办理人 ID"),
            // entity.flowTask.assigneeuserid
            new TranslationSeedItem("entity.flowTask.assigneeuserid", "zh-CN", "办理人ID", "办理人 ID"),
            // entity.flowTask.assigneeuserid
            new TranslationSeedItem("entity.flowTask.assigneeuserid", "zh-HK", "办理人ID", "办理人 ID"),

            // entity.flowTask.assigneeusername
            new TranslationSeedItem("entity.flowTask.assigneeusername", "en-US", "办理人姓名", "办理人姓名"),
            // entity.flowTask.assigneeusername
            new TranslationSeedItem("entity.flowTask.assigneeusername", "ja-JP", "办理人姓名", "办理人姓名"),
            // entity.flowTask.assigneeusername
            new TranslationSeedItem("entity.flowTask.assigneeusername", "zh-CN", "办理人姓名", "办理人姓名"),
            // entity.flowTask.assigneeusername
            new TranslationSeedItem("entity.flowTask.assigneeusername", "zh-HK", "办理人姓名", "办理人姓名"),

            // entity.flowTask.owneruserid
            new TranslationSeedItem("entity.flowTask.owneruserid", "en-US", "任务所有者ID", "任务所有者 ID（转办前原办理人）"),
            // entity.flowTask.owneruserid
            new TranslationSeedItem("entity.flowTask.owneruserid", "ja-JP", "任务所有者ID", "任务所有者 ID（转办前原办理人）"),
            // entity.flowTask.owneruserid
            new TranslationSeedItem("entity.flowTask.owneruserid", "zh-CN", "任务所有者ID", "任务所有者 ID（转办前原办理人）"),
            // entity.flowTask.owneruserid
            new TranslationSeedItem("entity.flowTask.owneruserid", "zh-HK", "任务所有者ID", "任务所有者 ID（转办前原办理人）"),

            // entity.flowTask.taskstatus
            new TranslationSeedItem("entity.flowTask.taskstatus", "en-US", "任务状态", "任务状态"),
            // entity.flowTask.taskstatus
            new TranslationSeedItem("entity.flowTask.taskstatus", "ja-JP", "任务状态", "任务状态"),
            // entity.flowTask.taskstatus
            new TranslationSeedItem("entity.flowTask.taskstatus", "zh-CN", "任务状态", "任务状态"),
            // entity.flowTask.taskstatus
            new TranslationSeedItem("entity.flowTask.taskstatus", "zh-HK", "任务状态", "任务状态"),

            // entity.flowTask.signtype
            new TranslationSeedItem("entity.flowTask.signtype", "en-US", "会签类型", "会签类型"),
            // entity.flowTask.signtype
            new TranslationSeedItem("entity.flowTask.signtype", "ja-JP", "会签类型", "会签类型"),
            // entity.flowTask.signtype
            new TranslationSeedItem("entity.flowTask.signtype", "zh-CN", "会签类型", "会签类型"),
            // entity.flowTask.signtype
            new TranslationSeedItem("entity.flowTask.signtype", "zh-HK", "会签类型", "会签类型"),

            // entity.flowTask.priority
            new TranslationSeedItem("entity.flowTask.priority", "en-US", "优先级", "优先级"),
            // entity.flowTask.priority
            new TranslationSeedItem("entity.flowTask.priority", "ja-JP", "优先级", "优先级"),
            // entity.flowTask.priority
            new TranslationSeedItem("entity.flowTask.priority", "zh-CN", "优先级", "优先级"),
            // entity.flowTask.priority
            new TranslationSeedItem("entity.flowTask.priority", "zh-HK", "优先级", "优先级"),

            // entity.flowTask.duedate
            new TranslationSeedItem("entity.flowTask.duedate", "en-US", "到期时间", "到期时间"),
            // entity.flowTask.duedate
            new TranslationSeedItem("entity.flowTask.duedate", "ja-JP", "到期时间", "到期时间"),
            // entity.flowTask.duedate
            new TranslationSeedItem("entity.flowTask.duedate", "zh-CN", "到期时间", "到期时间"),
            // entity.flowTask.duedate
            new TranslationSeedItem("entity.flowTask.duedate", "zh-HK", "到期时间", "到期时间"),

            // entity.flowTask.claimtime
            new TranslationSeedItem("entity.flowTask.claimtime", "en-US", "认领时间", "认领时间"),
            // entity.flowTask.claimtime
            new TranslationSeedItem("entity.flowTask.claimtime", "ja-JP", "认领时间", "认领时间"),
            // entity.flowTask.claimtime
            new TranslationSeedItem("entity.flowTask.claimtime", "zh-CN", "认领时间", "认领时间"),
            // entity.flowTask.claimtime
            new TranslationSeedItem("entity.flowTask.claimtime", "zh-HK", "认领时间", "认领时间"),

            // entity.flowTask.completedat
            new TranslationSeedItem("entity.flowTask.completedat", "en-US", "办结时间", "办结时间"),
            // entity.flowTask.completedat
            new TranslationSeedItem("entity.flowTask.completedat", "ja-JP", "办结时间", "办结时间"),
            // entity.flowTask.completedat
            new TranslationSeedItem("entity.flowTask.completedat", "zh-CN", "办结时间", "办结时间"),
            // entity.flowTask.completedat
            new TranslationSeedItem("entity.flowTask.completedat", "zh-HK", "办结时间", "办结时间"),

            // entity.flowTask.isaddsign
            new TranslationSeedItem("entity.flowTask.isaddsign", "en-US", "是否加签", "是否加签任务"),
            // entity.flowTask.isaddsign
            new TranslationSeedItem("entity.flowTask.isaddsign", "ja-JP", "是否加签", "是否加签任务"),
            // entity.flowTask.isaddsign
            new TranslationSeedItem("entity.flowTask.isaddsign", "zh-CN", "是否加签", "是否加签任务"),
            // entity.flowTask.isaddsign
            new TranslationSeedItem("entity.flowTask.isaddsign", "zh-HK", "是否加签", "是否加签任务"),

            // entity.flowTask.addsignid
            new TranslationSeedItem("entity.flowTask.addsignid", "en-US", "加签记录ID", "加签记录 ID（<see cref=\"TaktFlowAddSign\"/>）"),
            // entity.flowTask.addsignid
            new TranslationSeedItem("entity.flowTask.addsignid", "ja-JP", "加签记录ID", "加签记录 ID（<see cref=\"TaktFlowAddSign\"/>）"),
            // entity.flowTask.addsignid
            new TranslationSeedItem("entity.flowTask.addsignid", "zh-CN", "加签记录ID", "加签记录 ID（<see cref=\"TaktFlowAddSign\"/>）"),
            // entity.flowTask.addsignid
            new TranslationSeedItem("entity.flowTask.addsignid", "zh-HK", "加签记录ID", "加签记录 ID（<see cref=\"TaktFlowAddSign\"/>）"),

            // entity.flowTask.sortorder
            new TranslationSeedItem("entity.flowTask.sortorder", "en-US", "序号", "多实例序号"),
            // entity.flowTask.sortorder
            new TranslationSeedItem("entity.flowTask.sortorder", "ja-JP", "序号", "多实例序号"),
            // entity.flowTask.sortorder
            new TranslationSeedItem("entity.flowTask.sortorder", "zh-CN", "序号", "多实例序号"),
            // entity.flowTask.sortorder
            new TranslationSeedItem("entity.flowTask.sortorder", "zh-HK", "序号", "多实例序号"),

            // entity.flowTask.comment
            new TranslationSeedItem("entity.flowTask.comment", "en-US", "审批意见", "审批意见"),
            // entity.flowTask.comment
            new TranslationSeedItem("entity.flowTask.comment", "ja-JP", "审批意见", "审批意见"),
            // entity.flowTask.comment
            new TranslationSeedItem("entity.flowTask.comment", "zh-CN", "审批意见", "审批意见"),
            // entity.flowTask.comment
            new TranslationSeedItem("entity.flowTask.comment", "zh-HK", "审批意见", "审批意见"),

            // entity.flowTask.instance
            new TranslationSeedItem("entity.flowTask.instance", "en-US", "所属流程实例", "所属流程实例"),
            // entity.flowTask.instance
            new TranslationSeedItem("entity.flowTask.instance", "ja-JP", "所属流程实例", "所属流程实例"),
            // entity.flowTask.instance
            new TranslationSeedItem("entity.flowTask.instance", "zh-CN", "所属流程实例", "所属流程实例"),
            // entity.flowTask.instance
            new TranslationSeedItem("entity.flowTask.instance", "zh-HK", "所属流程实例", "所属流程实例"),
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
        translation.ResourceGroup = TaktModule.Workflow;
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
