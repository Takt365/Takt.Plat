// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk
// 文件名称：TaktKnowledgeChangeLogI18nSeedData.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktKnowledgeChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk;

/// <summary>
/// TaktKnowledgeChangeLog 实体国际化翻译种子（键前缀 entity.knowledgechangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktKnowledgeChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktKnowledgeChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 knowledgechangelog 实体翻译...", tenantCode);

        foreach (var item in GetKnowledgeChangeLogTranslations())
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

        TaktLogger.Information("TaktKnowledgeChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktKnowledgeChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.knowledgechangelog._self / entity.knowledgechangelog.{{field}}；ResourceGroup=HelpDesk；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetKnowledgeChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.knowledgechangelog._self
            new TranslationSeedItem("entity.knowledgechangelog._self", "en-US", "Knowledge Change Log Information_us", "实体名称"),
            // entity.knowledgechangelog._self
            new TranslationSeedItem("entity.knowledgechangelog._self", "ja-JP", "知识库变更日志信息_jp", "实体名称"),
            // entity.knowledgechangelog._self
            new TranslationSeedItem("entity.knowledgechangelog._self", "zh-CN", "知识库变更日志信息", "实体名称"),
            // entity.knowledgechangelog._self
            new TranslationSeedItem("entity.knowledgechangelog._self", "zh-HK", "知识库变更日志信息_hk", "实体名称"),

            // entity.knowledgechangelog.knowledgeid
            new TranslationSeedItem("entity.knowledgechangelog.knowledgeid", "en-US", "知识ID_us", "知识 ID（关联 TaktKnowledge.Id，选项 TaktKnowledges/options）"),
            // entity.knowledgechangelog.knowledgeid
            new TranslationSeedItem("entity.knowledgechangelog.knowledgeid", "ja-JP", "知识ID_jp", "知识 ID（关联 TaktKnowledge.Id，选项 TaktKnowledges/options）"),
            // entity.knowledgechangelog.knowledgeid
            new TranslationSeedItem("entity.knowledgechangelog.knowledgeid", "zh-CN", "知识ID", "知识 ID（关联 TaktKnowledge.Id，选项 TaktKnowledges/options）"),
            // entity.knowledgechangelog.knowledgeid
            new TranslationSeedItem("entity.knowledgechangelog.knowledgeid", "zh-HK", "知识ID_hk", "知识 ID（关联 TaktKnowledge.Id，选项 TaktKnowledges/options）"),

            // entity.knowledgechangelog.knowledgetitle
            new TranslationSeedItem("entity.knowledgechangelog.knowledgetitle", "en-US", "知识标题_us", "知识标题（冗余）"),
            // entity.knowledgechangelog.knowledgetitle
            new TranslationSeedItem("entity.knowledgechangelog.knowledgetitle", "ja-JP", "知识标题_jp", "知识标题（冗余）"),
            // entity.knowledgechangelog.knowledgetitle
            new TranslationSeedItem("entity.knowledgechangelog.knowledgetitle", "zh-CN", "知识标题", "知识标题（冗余）"),
            // entity.knowledgechangelog.knowledgetitle
            new TranslationSeedItem("entity.knowledgechangelog.knowledgetitle", "zh-HK", "知识标题_hk", "知识标题（冗余）"),

            // entity.knowledgechangelog.changetype
            new TranslationSeedItem("entity.knowledgechangelog.changetype", "en-US", "变更类型_us", "变更类型（字典 sys_entity_change_type；0=创建 1=更新 2=删除 3=状态变更）"),
            // entity.knowledgechangelog.changetype
            new TranslationSeedItem("entity.knowledgechangelog.changetype", "ja-JP", "变更类型_jp", "变更类型（字典 sys_entity_change_type；0=创建 1=更新 2=删除 3=状态变更）"),
            // entity.knowledgechangelog.changetype
            new TranslationSeedItem("entity.knowledgechangelog.changetype", "zh-CN", "变更类型", "变更类型（字典 sys_entity_change_type；0=创建 1=更新 2=删除 3=状态变更）"),
            // entity.knowledgechangelog.changetype
            new TranslationSeedItem("entity.knowledgechangelog.changetype", "zh-HK", "变更类型_hk", "变更类型（字典 sys_entity_change_type；0=创建 1=更新 2=删除 3=状态变更）"),

            // entity.knowledgechangelog.changesummary
            new TranslationSeedItem("entity.knowledgechangelog.changesummary", "en-US", "修改内容摘要_us", "修改内容摘要"),
            // entity.knowledgechangelog.changesummary
            new TranslationSeedItem("entity.knowledgechangelog.changesummary", "ja-JP", "修改内容摘要_jp", "修改内容摘要"),
            // entity.knowledgechangelog.changesummary
            new TranslationSeedItem("entity.knowledgechangelog.changesummary", "zh-CN", "修改内容摘要", "修改内容摘要"),
            // entity.knowledgechangelog.changesummary
            new TranslationSeedItem("entity.knowledgechangelog.changesummary", "zh-HK", "修改内容摘要_hk", "修改内容摘要"),

            // entity.knowledgechangelog.changefields
            new TranslationSeedItem("entity.knowledgechangelog.changefields", "en-US", "变更字段列表_us", "变更字段列表（JSON 数组）"),
            // entity.knowledgechangelog.changefields
            new TranslationSeedItem("entity.knowledgechangelog.changefields", "ja-JP", "变更字段列表_jp", "变更字段列表（JSON 数组）"),
            // entity.knowledgechangelog.changefields
            new TranslationSeedItem("entity.knowledgechangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON 数组）"),
            // entity.knowledgechangelog.changefields
            new TranslationSeedItem("entity.knowledgechangelog.changefields", "zh-HK", "变更字段列表_hk", "变更字段列表（JSON 数组）"),

            // entity.knowledgechangelog.changereason
            new TranslationSeedItem("entity.knowledgechangelog.changereason", "en-US", "变更原因_us", "变更原因或备注"),
            // entity.knowledgechangelog.changereason
            new TranslationSeedItem("entity.knowledgechangelog.changereason", "ja-JP", "变更原因_jp", "变更原因或备注"),
            // entity.knowledgechangelog.changereason
            new TranslationSeedItem("entity.knowledgechangelog.changereason", "zh-CN", "变更原因", "变更原因或备注"),
            // entity.knowledgechangelog.changereason
            new TranslationSeedItem("entity.knowledgechangelog.changereason", "zh-HK", "变更原因_hk", "变更原因或备注"),

            // entity.knowledgechangelog.versionatchange
            new TranslationSeedItem("entity.knowledgechangelog.versionatchange", "en-US", "变更时版本号_us", "变更时知识版本号"),
            // entity.knowledgechangelog.versionatchange
            new TranslationSeedItem("entity.knowledgechangelog.versionatchange", "ja-JP", "变更时版本号_jp", "变更时知识版本号"),
            // entity.knowledgechangelog.versionatchange
            new TranslationSeedItem("entity.knowledgechangelog.versionatchange", "zh-CN", "变更时版本号", "变更时知识版本号"),
            // entity.knowledgechangelog.versionatchange
            new TranslationSeedItem("entity.knowledgechangelog.versionatchange", "zh-HK", "变更时版本号_hk", "变更时知识版本号"),

            // entity.knowledgechangelog.knowledge
            new TranslationSeedItem("entity.knowledgechangelog.knowledge", "en-US", "知识库_us", "知识库（主表）"),
            // entity.knowledgechangelog.knowledge
            new TranslationSeedItem("entity.knowledgechangelog.knowledge", "ja-JP", "知识库_jp", "知识库（主表）"),
            // entity.knowledgechangelog.knowledge
            new TranslationSeedItem("entity.knowledgechangelog.knowledge", "zh-CN", "知识库", "知识库（主表）"),
            // entity.knowledgechangelog.knowledge
            new TranslationSeedItem("entity.knowledgechangelog.knowledge", "zh-HK", "知识库_hk", "知识库（主表）"),
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
        translation.ResourceGroup = "HelpDesk";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
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
