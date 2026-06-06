// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowAddSignI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowAddSign 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFlowAddSign 实体国际化翻译种子（键前缀 entity.flowAddSign.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowAddSignI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowAddSign 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowAddSign 实体翻译...", tenantCode);

        foreach (var item in GetFlowAddSignTranslations())
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

        TaktLogger.Information("TaktFlowAddSign 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowAddSign 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowAddSign._self / entity.flowAddSign.{{field}}；ResourceGroup=TaktModule.Workflow；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowAddSignTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowAddSign._self
            new TranslationSeedItem("entity.flowAddSign._self", "en-US", "Flow Add Sign Information", "实体名称"),
            // entity.flowAddSign._self
            new TranslationSeedItem("entity.flowAddSign._self", "ja-JP", "流程加签记录信息", "实体名称"),
            // entity.flowAddSign._self
            new TranslationSeedItem("entity.flowAddSign._self", "zh-CN", "流程加签记录信息", "实体名称"),
            // entity.flowAddSign._self
            new TranslationSeedItem("entity.flowAddSign._self", "zh-HK", "流程加签记录信息", "实体名称"),

            // entity.flowAddSign.instanceid
            new TranslationSeedItem("entity.flowAddSign.instanceid", "en-US", "流程实例ID", "流程实例 ID"),
            // entity.flowAddSign.instanceid
            new TranslationSeedItem("entity.flowAddSign.instanceid", "ja-JP", "流程实例ID", "流程实例 ID"),
            // entity.flowAddSign.instanceid
            new TranslationSeedItem("entity.flowAddSign.instanceid", "zh-CN", "流程实例ID", "流程实例 ID"),
            // entity.flowAddSign.instanceid
            new TranslationSeedItem("entity.flowAddSign.instanceid", "zh-HK", "流程实例ID", "流程实例 ID"),

            // entity.flowAddSign.nodeid
            new TranslationSeedItem("entity.flowAddSign.nodeid", "en-US", "节点ID", "加签节点 ID"),
            // entity.flowAddSign.nodeid
            new TranslationSeedItem("entity.flowAddSign.nodeid", "ja-JP", "节点ID", "加签节点 ID"),
            // entity.flowAddSign.nodeid
            new TranslationSeedItem("entity.flowAddSign.nodeid", "zh-CN", "节点ID", "加签节点 ID"),
            // entity.flowAddSign.nodeid
            new TranslationSeedItem("entity.flowAddSign.nodeid", "zh-HK", "节点ID", "加签节点 ID"),

            // entity.flowAddSign.signuserid
            new TranslationSeedItem("entity.flowAddSign.signuserid", "en-US", "加签人ID", "加签人 ID"),
            // entity.flowAddSign.signuserid
            new TranslationSeedItem("entity.flowAddSign.signuserid", "ja-JP", "加签人ID", "加签人 ID"),
            // entity.flowAddSign.signuserid
            new TranslationSeedItem("entity.flowAddSign.signuserid", "zh-CN", "加签人ID", "加签人 ID"),
            // entity.flowAddSign.signuserid
            new TranslationSeedItem("entity.flowAddSign.signuserid", "zh-HK", "加签人ID", "加签人 ID"),

            // entity.flowAddSign.signusername
            new TranslationSeedItem("entity.flowAddSign.signusername", "en-US", "加签人姓名", "加签人姓名"),
            // entity.flowAddSign.signusername
            new TranslationSeedItem("entity.flowAddSign.signusername", "ja-JP", "加签人姓名", "加签人姓名"),
            // entity.flowAddSign.signusername
            new TranslationSeedItem("entity.flowAddSign.signusername", "zh-CN", "加签人姓名", "加签人姓名"),
            // entity.flowAddSign.signusername
            new TranslationSeedItem("entity.flowAddSign.signusername", "zh-HK", "加签人姓名", "加签人姓名"),

            // entity.flowAddSign.signtype
            new TranslationSeedItem("entity.flowAddSign.signtype", "en-US", "加签方式", "加签方式（sequential / all / one，与前端 approveType 一致）"),
            // entity.flowAddSign.signtype
            new TranslationSeedItem("entity.flowAddSign.signtype", "ja-JP", "加签方式", "加签方式（sequential / all / one，与前端 approveType 一致）"),
            // entity.flowAddSign.signtype
            new TranslationSeedItem("entity.flowAddSign.signtype", "zh-CN", "加签方式", "加签方式（sequential / all / one，与前端 approveType 一致）"),
            // entity.flowAddSign.signtype
            new TranslationSeedItem("entity.flowAddSign.signtype", "zh-HK", "加签方式", "加签方式（sequential / all / one，与前端 approveType 一致）"),

            // entity.flowAddSign.returntosignnode
            new TranslationSeedItem("entity.flowAddSign.returntosignnode", "en-US", "回到加签节点", "完成后是否回到加签节点"),
            // entity.flowAddSign.returntosignnode
            new TranslationSeedItem("entity.flowAddSign.returntosignnode", "ja-JP", "回到加签节点", "完成后是否回到加签节点"),
            // entity.flowAddSign.returntosignnode
            new TranslationSeedItem("entity.flowAddSign.returntosignnode", "zh-CN", "回到加签节点", "完成后是否回到加签节点"),
            // entity.flowAddSign.returntosignnode
            new TranslationSeedItem("entity.flowAddSign.returntosignnode", "zh-HK", "回到加签节点", "完成后是否回到加签节点"),

            // entity.flowAddSign.reason
            new TranslationSeedItem("entity.flowAddSign.reason", "en-US", "加签原因", "加签原因"),
            // entity.flowAddSign.reason
            new TranslationSeedItem("entity.flowAddSign.reason", "ja-JP", "加签原因", "加签原因"),
            // entity.flowAddSign.reason
            new TranslationSeedItem("entity.flowAddSign.reason", "zh-CN", "加签原因", "加签原因"),
            // entity.flowAddSign.reason
            new TranslationSeedItem("entity.flowAddSign.reason", "zh-HK", "加签原因", "加签原因"),

            // entity.flowAddSign.ishandled
            new TranslationSeedItem("entity.flowAddSign.ishandled", "en-US", "是否已处理", "是否已处理（含减签）"),
            // entity.flowAddSign.ishandled
            new TranslationSeedItem("entity.flowAddSign.ishandled", "ja-JP", "是否已处理", "是否已处理（含减签）"),
            // entity.flowAddSign.ishandled
            new TranslationSeedItem("entity.flowAddSign.ishandled", "zh-CN", "是否已处理", "是否已处理（含减签）"),
            // entity.flowAddSign.ishandled
            new TranslationSeedItem("entity.flowAddSign.ishandled", "zh-HK", "是否已处理", "是否已处理（含减签）"),
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
