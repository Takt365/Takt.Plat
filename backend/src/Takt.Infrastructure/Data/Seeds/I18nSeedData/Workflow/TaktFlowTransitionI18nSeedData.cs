// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowTransitionI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowTransition 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFlowTransition 实体国际化翻译种子（键前缀 entity.flowTransition.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowTransitionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowTransition 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowTransition 实体翻译...", tenantCode);

        foreach (var item in GetFlowTransitionTranslations())
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

        TaktLogger.Information("TaktFlowTransition 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowTransition 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowTransition._self / entity.flowTransition.{{field}}；ResourceGroup=TaktModule.Workflow；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowTransitionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowTransition._self
            new TranslationSeedItem("entity.flowTransition._self", "en-US", "Flow Transition Information", "实体名称"),
            // entity.flowTransition._self
            new TranslationSeedItem("entity.flowTransition._self", "ja-JP", "流程流转历史信息", "实体名称"),
            // entity.flowTransition._self
            new TranslationSeedItem("entity.flowTransition._self", "zh-CN", "流程流转历史信息", "实体名称"),
            // entity.flowTransition._self
            new TranslationSeedItem("entity.flowTransition._self", "zh-HK", "流程流转历史信息", "实体名称"),

            // entity.flowTransition.instanceid
            new TranslationSeedItem("entity.flowTransition.instanceid", "en-US", "流程实例ID", "流程实例 ID"),
            // entity.flowTransition.instanceid
            new TranslationSeedItem("entity.flowTransition.instanceid", "ja-JP", "流程实例ID", "流程实例 ID"),
            // entity.flowTransition.instanceid
            new TranslationSeedItem("entity.flowTransition.instanceid", "zh-CN", "流程实例ID", "流程实例 ID"),
            // entity.flowTransition.instanceid
            new TranslationSeedItem("entity.flowTransition.instanceid", "zh-HK", "流程实例ID", "流程实例 ID"),

            // entity.flowTransition.activityid
            new TranslationSeedItem("entity.flowTransition.activityid", "en-US", "节点ID", "节点 ID"),
            // entity.flowTransition.activityid
            new TranslationSeedItem("entity.flowTransition.activityid", "ja-JP", "节点ID", "节点 ID"),
            // entity.flowTransition.activityid
            new TranslationSeedItem("entity.flowTransition.activityid", "zh-CN", "节点ID", "节点 ID"),
            // entity.flowTransition.activityid
            new TranslationSeedItem("entity.flowTransition.activityid", "zh-HK", "节点ID", "节点 ID"),

            // entity.flowTransition.activityname
            new TranslationSeedItem("entity.flowTransition.activityname", "en-US", "节点名称", "节点名称"),
            // entity.flowTransition.activityname
            new TranslationSeedItem("entity.flowTransition.activityname", "ja-JP", "节点名称", "节点名称"),
            // entity.flowTransition.activityname
            new TranslationSeedItem("entity.flowTransition.activityname", "zh-CN", "节点名称", "节点名称"),
            // entity.flowTransition.activityname
            new TranslationSeedItem("entity.flowTransition.activityname", "zh-HK", "节点名称", "节点名称"),

            // entity.flowTransition.activitytype
            new TranslationSeedItem("entity.flowTransition.activitytype", "en-US", "节点类型", "节点类型（如 userTask、start、end）"),
            // entity.flowTransition.activitytype
            new TranslationSeedItem("entity.flowTransition.activitytype", "ja-JP", "节点类型", "节点类型（如 userTask、start、end）"),
            // entity.flowTransition.activitytype
            new TranslationSeedItem("entity.flowTransition.activitytype", "zh-CN", "节点类型", "节点类型（如 userTask、start、end）"),
            // entity.flowTransition.activitytype
            new TranslationSeedItem("entity.flowTransition.activitytype", "zh-HK", "节点类型", "节点类型（如 userTask、start、end）"),

            // entity.flowTransition.fromnodeid
            new TranslationSeedItem("entity.flowTransition.fromnodeid", "en-US", "源节点ID", "源节点 ID"),
            // entity.flowTransition.fromnodeid
            new TranslationSeedItem("entity.flowTransition.fromnodeid", "ja-JP", "源节点ID", "源节点 ID"),
            // entity.flowTransition.fromnodeid
            new TranslationSeedItem("entity.flowTransition.fromnodeid", "zh-CN", "源节点ID", "源节点 ID"),
            // entity.flowTransition.fromnodeid
            new TranslationSeedItem("entity.flowTransition.fromnodeid", "zh-HK", "源节点ID", "源节点 ID"),

            // entity.flowTransition.fromnodename
            new TranslationSeedItem("entity.flowTransition.fromnodename", "en-US", "源节点名称", "源节点名称"),
            // entity.flowTransition.fromnodename
            new TranslationSeedItem("entity.flowTransition.fromnodename", "ja-JP", "源节点名称", "源节点名称"),
            // entity.flowTransition.fromnodename
            new TranslationSeedItem("entity.flowTransition.fromnodename", "zh-CN", "源节点名称", "源节点名称"),
            // entity.flowTransition.fromnodename
            new TranslationSeedItem("entity.flowTransition.fromnodename", "zh-HK", "源节点名称", "源节点名称"),

            // entity.flowTransition.tonodeid
            new TranslationSeedItem("entity.flowTransition.tonodeid", "en-US", "目标节点ID", "目标节点 ID"),
            // entity.flowTransition.tonodeid
            new TranslationSeedItem("entity.flowTransition.tonodeid", "ja-JP", "目标节点ID", "目标节点 ID"),
            // entity.flowTransition.tonodeid
            new TranslationSeedItem("entity.flowTransition.tonodeid", "zh-CN", "目标节点ID", "目标节点 ID"),
            // entity.flowTransition.tonodeid
            new TranslationSeedItem("entity.flowTransition.tonodeid", "zh-HK", "目标节点ID", "目标节点 ID"),

            // entity.flowTransition.tonodename
            new TranslationSeedItem("entity.flowTransition.tonodename", "en-US", "目标节点名称", "目标节点名称"),
            // entity.flowTransition.tonodename
            new TranslationSeedItem("entity.flowTransition.tonodename", "ja-JP", "目标节点名称", "目标节点名称"),
            // entity.flowTransition.tonodename
            new TranslationSeedItem("entity.flowTransition.tonodename", "zh-CN", "目标节点名称", "目标节点名称"),
            // entity.flowTransition.tonodename
            new TranslationSeedItem("entity.flowTransition.tonodename", "zh-HK", "目标节点名称", "目标节点名称"),

            // entity.flowTransition.transitionuserid
            new TranslationSeedItem("entity.flowTransition.transitionuserid", "en-US", "操作人ID", "操作人 ID"),
            // entity.flowTransition.transitionuserid
            new TranslationSeedItem("entity.flowTransition.transitionuserid", "ja-JP", "操作人ID", "操作人 ID"),
            // entity.flowTransition.transitionuserid
            new TranslationSeedItem("entity.flowTransition.transitionuserid", "zh-CN", "操作人ID", "操作人 ID"),
            // entity.flowTransition.transitionuserid
            new TranslationSeedItem("entity.flowTransition.transitionuserid", "zh-HK", "操作人ID", "操作人 ID"),

            // entity.flowTransition.transitionusername
            new TranslationSeedItem("entity.flowTransition.transitionusername", "en-US", "操作人姓名", "操作人姓名"),
            // entity.flowTransition.transitionusername
            new TranslationSeedItem("entity.flowTransition.transitionusername", "ja-JP", "操作人姓名", "操作人姓名"),
            // entity.flowTransition.transitionusername
            new TranslationSeedItem("entity.flowTransition.transitionusername", "zh-CN", "操作人姓名", "操作人姓名"),
            // entity.flowTransition.transitionusername
            new TranslationSeedItem("entity.flowTransition.transitionusername", "zh-HK", "操作人姓名", "操作人姓名"),

            // entity.flowTransition.starttime
            new TranslationSeedItem("entity.flowTransition.starttime", "en-US", "开始时间", "开始时间"),
            // entity.flowTransition.starttime
            new TranslationSeedItem("entity.flowTransition.starttime", "ja-JP", "开始时间", "开始时间"),
            // entity.flowTransition.starttime
            new TranslationSeedItem("entity.flowTransition.starttime", "zh-CN", "开始时间", "开始时间"),
            // entity.flowTransition.starttime
            new TranslationSeedItem("entity.flowTransition.starttime", "zh-HK", "开始时间", "开始时间"),

            // entity.flowTransition.transitiontime
            new TranslationSeedItem("entity.flowTransition.transitiontime", "en-US", "结束时间", "结束时间"),
            // entity.flowTransition.transitiontime
            new TranslationSeedItem("entity.flowTransition.transitiontime", "ja-JP", "结束时间", "结束时间"),
            // entity.flowTransition.transitiontime
            new TranslationSeedItem("entity.flowTransition.transitiontime", "zh-CN", "结束时间", "结束时间"),
            // entity.flowTransition.transitiontime
            new TranslationSeedItem("entity.flowTransition.transitiontime", "zh-HK", "结束时间", "结束时间"),

            // entity.flowTransition.durationms
            new TranslationSeedItem("entity.flowTransition.durationms", "en-US", "历时毫秒", "历时毫秒"),
            // entity.flowTransition.durationms
            new TranslationSeedItem("entity.flowTransition.durationms", "ja-JP", "历时毫秒", "历时毫秒"),
            // entity.flowTransition.durationms
            new TranslationSeedItem("entity.flowTransition.durationms", "zh-CN", "历时毫秒", "历时毫秒"),
            // entity.flowTransition.durationms
            new TranslationSeedItem("entity.flowTransition.durationms", "zh-HK", "历时毫秒", "历时毫秒"),

            // entity.flowTransition.transitioncomment
            new TranslationSeedItem("entity.flowTransition.transitioncomment", "en-US", "操作意见", "操作意见"),
            // entity.flowTransition.transitioncomment
            new TranslationSeedItem("entity.flowTransition.transitioncomment", "ja-JP", "操作意见", "操作意见"),
            // entity.flowTransition.transitioncomment
            new TranslationSeedItem("entity.flowTransition.transitioncomment", "zh-CN", "操作意见", "操作意见"),
            // entity.flowTransition.transitioncomment
            new TranslationSeedItem("entity.flowTransition.transitioncomment", "zh-HK", "操作意见", "操作意见"),

            // entity.flowTransition.actiontype
            new TranslationSeedItem("entity.flowTransition.actiontype", "en-US", "动作类型", "动作类型"),
            // entity.flowTransition.actiontype
            new TranslationSeedItem("entity.flowTransition.actiontype", "ja-JP", "动作类型", "动作类型"),
            // entity.flowTransition.actiontype
            new TranslationSeedItem("entity.flowTransition.actiontype", "zh-CN", "动作类型", "动作类型"),
            // entity.flowTransition.actiontype
            new TranslationSeedItem("entity.flowTransition.actiontype", "zh-HK", "动作类型", "动作类型"),
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
