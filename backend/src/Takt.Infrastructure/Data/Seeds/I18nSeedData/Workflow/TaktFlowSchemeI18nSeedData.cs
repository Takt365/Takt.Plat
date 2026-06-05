// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowSchemeI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowScheme 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFlowScheme 实体国际化翻译种子（键前缀 entity.flowScheme.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowSchemeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowScheme 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowScheme 实体翻译...", tenantCode);

        foreach (var item in GetFlowSchemeTranslations())
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

        TaktLogger.Information("TaktFlowScheme 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowScheme 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowScheme._self / entity.flowScheme.{{field}}；ResourceGroup=TaktModule.Workflow；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowSchemeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowScheme._self
            new TranslationSeedItem("entity.flowScheme._self", "en-US", "Flow Scheme Information", "实体名称"),
            // entity.flowScheme._self
            new TranslationSeedItem("entity.flowScheme._self", "ja-JP", "流程定义信息", "实体名称"),
            // entity.flowScheme._self
            new TranslationSeedItem("entity.flowScheme._self", "zh-CN", "流程定义信息", "实体名称"),
            // entity.flowScheme._self
            new TranslationSeedItem("entity.flowScheme._self", "zh-HK", "流程定义信息", "实体名称"),

            // entity.flowScheme.processkey
            new TranslationSeedItem("entity.flowScheme.processkey", "en-US", "流程键", "流程键（公司内业务唯一标识，如 leave）"),
            // entity.flowScheme.processkey
            new TranslationSeedItem("entity.flowScheme.processkey", "ja-JP", "流程键", "流程键（公司内业务唯一标识，如 leave）"),
            // entity.flowScheme.processkey
            new TranslationSeedItem("entity.flowScheme.processkey", "zh-CN", "流程键", "流程键（公司内业务唯一标识，如 leave）"),
            // entity.flowScheme.processkey
            new TranslationSeedItem("entity.flowScheme.processkey", "zh-HK", "流程键", "流程键（公司内业务唯一标识，如 leave）"),

            // entity.flowScheme.processname
            new TranslationSeedItem("entity.flowScheme.processname", "en-US", "流程名称", "流程名称"),
            // entity.flowScheme.processname
            new TranslationSeedItem("entity.flowScheme.processname", "ja-JP", "流程名称", "流程名称"),
            // entity.flowScheme.processname
            new TranslationSeedItem("entity.flowScheme.processname", "zh-CN", "流程名称", "流程名称"),
            // entity.flowScheme.processname
            new TranslationSeedItem("entity.flowScheme.processname", "zh-HK", "流程名称", "流程名称"),

            // entity.flowScheme.definitionversion
            new TranslationSeedItem("entity.flowScheme.definitionversion", "en-US", "定义版本号", "定义版本号（同流程键可多版本）"),
            // entity.flowScheme.definitionversion
            new TranslationSeedItem("entity.flowScheme.definitionversion", "ja-JP", "定义版本号", "定义版本号（同流程键可多版本）"),
            // entity.flowScheme.definitionversion
            new TranslationSeedItem("entity.flowScheme.definitionversion", "zh-CN", "定义版本号", "定义版本号（同流程键可多版本）"),
            // entity.flowScheme.definitionversion
            new TranslationSeedItem("entity.flowScheme.definitionversion", "zh-HK", "定义版本号", "定义版本号（同流程键可多版本）"),

            // entity.flowScheme.processversion
            new TranslationSeedItem("entity.flowScheme.processversion", "en-US", "版本标签", "版本标签（如 v1.0.0）"),
            // entity.flowScheme.processversion
            new TranslationSeedItem("entity.flowScheme.processversion", "ja-JP", "版本标签", "版本标签（如 v1.0.0）"),
            // entity.flowScheme.processversion
            new TranslationSeedItem("entity.flowScheme.processversion", "zh-CN", "版本标签", "版本标签（如 v1.0.0）"),
            // entity.flowScheme.processversion
            new TranslationSeedItem("entity.flowScheme.processversion", "zh-HK", "版本标签", "版本标签（如 v1.0.0）"),

            // entity.flowScheme.islatest
            new TranslationSeedItem("entity.flowScheme.islatest", "en-US", "是否最新版", "是否当前最新版（同键仅一条为 1）"),
            // entity.flowScheme.islatest
            new TranslationSeedItem("entity.flowScheme.islatest", "ja-JP", "是否最新版", "是否当前最新版（同键仅一条为 1）"),
            // entity.flowScheme.islatest
            new TranslationSeedItem("entity.flowScheme.islatest", "zh-CN", "是否最新版", "是否当前最新版（同键仅一条为 1）"),
            // entity.flowScheme.islatest
            new TranslationSeedItem("entity.flowScheme.islatest", "zh-HK", "是否最新版", "是否当前最新版（同键仅一条为 1）"),

            // entity.flowScheme.processcategory
            new TranslationSeedItem("entity.flowScheme.processcategory", "en-US", "流程分类", "流程分类"),
            // entity.flowScheme.processcategory
            new TranslationSeedItem("entity.flowScheme.processcategory", "ja-JP", "流程分类", "流程分类"),
            // entity.flowScheme.processcategory
            new TranslationSeedItem("entity.flowScheme.processcategory", "zh-CN", "流程分类", "流程分类"),
            // entity.flowScheme.processcategory
            new TranslationSeedItem("entity.flowScheme.processcategory", "zh-HK", "流程分类", "流程分类"),

            // entity.flowScheme.processdescription
            new TranslationSeedItem("entity.flowScheme.processdescription", "en-US", "流程说明", "流程说明"),
            // entity.flowScheme.processdescription
            new TranslationSeedItem("entity.flowScheme.processdescription", "ja-JP", "流程说明", "流程说明"),
            // entity.flowScheme.processdescription
            new TranslationSeedItem("entity.flowScheme.processdescription", "zh-CN", "流程说明", "流程说明"),
            // entity.flowScheme.processdescription
            new TranslationSeedItem("entity.flowScheme.processdescription", "zh-HK", "流程说明", "流程说明"),

            // entity.flowScheme.processstatus
            new TranslationSeedItem("entity.flowScheme.processstatus", "en-US", "发布状态", "发布状态"),
            // entity.flowScheme.processstatus
            new TranslationSeedItem("entity.flowScheme.processstatus", "ja-JP", "发布状态", "发布状态"),
            // entity.flowScheme.processstatus
            new TranslationSeedItem("entity.flowScheme.processstatus", "zh-CN", "发布状态", "发布状态"),
            // entity.flowScheme.processstatus
            new TranslationSeedItem("entity.flowScheme.processstatus", "zh-HK", "发布状态", "发布状态"),

            // entity.flowScheme.processcontent
            new TranslationSeedItem("entity.flowScheme.processcontent", "en-US", "流程设计JSON", "流程设计 JSON（节点、网关、条件、审批人配置）"),
            // entity.flowScheme.processcontent
            new TranslationSeedItem("entity.flowScheme.processcontent", "ja-JP", "流程设计JSON", "流程设计 JSON（节点、网关、条件、审批人配置）"),
            // entity.flowScheme.processcontent
            new TranslationSeedItem("entity.flowScheme.processcontent", "zh-CN", "流程设计JSON", "流程设计 JSON（节点、网关、条件、审批人配置）"),
            // entity.flowScheme.processcontent
            new TranslationSeedItem("entity.flowScheme.processcontent", "zh-HK", "流程设计JSON", "流程设计 JSON（节点、网关、条件、审批人配置）"),

            // entity.flowScheme.deploymentid
            new TranslationSeedItem("entity.flowScheme.deploymentid", "en-US", "部署批次号", "部署批次号"),
            // entity.flowScheme.deploymentid
            new TranslationSeedItem("entity.flowScheme.deploymentid", "ja-JP", "部署批次号", "部署批次号"),
            // entity.flowScheme.deploymentid
            new TranslationSeedItem("entity.flowScheme.deploymentid", "zh-CN", "部署批次号", "部署批次号"),
            // entity.flowScheme.deploymentid
            new TranslationSeedItem("entity.flowScheme.deploymentid", "zh-HK", "部署批次号", "部署批次号"),

            // entity.flowScheme.formid
            new TranslationSeedItem("entity.flowScheme.formid", "en-US", "关联表单ID", "关联表单 ID"),
            // entity.flowScheme.formid
            new TranslationSeedItem("entity.flowScheme.formid", "ja-JP", "关联表单ID", "关联表单 ID"),
            // entity.flowScheme.formid
            new TranslationSeedItem("entity.flowScheme.formid", "zh-CN", "关联表单ID", "关联表单 ID"),
            // entity.flowScheme.formid
            new TranslationSeedItem("entity.flowScheme.formid", "zh-HK", "关联表单ID", "关联表单 ID"),

            // entity.flowScheme.formcode
            new TranslationSeedItem("entity.flowScheme.formcode", "en-US", "关联表单编码", "关联表单编码"),
            // entity.flowScheme.formcode
            new TranslationSeedItem("entity.flowScheme.formcode", "ja-JP", "关联表单编码", "关联表单编码"),
            // entity.flowScheme.formcode
            new TranslationSeedItem("entity.flowScheme.formcode", "zh-CN", "关联表单编码", "关联表单编码"),
            // entity.flowScheme.formcode
            new TranslationSeedItem("entity.flowScheme.formcode", "zh-HK", "关联表单编码", "关联表单编码"),

            // entity.flowScheme.sortorder
            new TranslationSeedItem("entity.flowScheme.sortorder", "en-US", "排序号", "排序号"),
            // entity.flowScheme.sortorder
            new TranslationSeedItem("entity.flowScheme.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.flowScheme.sortorder
            new TranslationSeedItem("entity.flowScheme.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.flowScheme.sortorder
            new TranslationSeedItem("entity.flowScheme.sortorder", "zh-HK", "排序号", "排序号"),
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
