// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.DocumentCenter
// 文件名称：TaktDocumentChangeLogI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDocumentChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.DocumentCenter;

/// <summary>
/// TaktDocumentChangeLog 实体国际化翻译种子（键前缀 entity.documentchangelog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDocumentChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDocumentChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 documentchangelog 实体翻译...", tenantCode);

        foreach (var item in GetDocumentChangeLogTranslations())
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

        TaktLogger.Information("TaktDocumentChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDocumentChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.documentchangelog._self / entity.documentchangelog.{{field}}；ResourceGroup=DocumentCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDocumentChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.documentchangelog._self
            new TranslationSeedItem("entity.documentchangelog._self", "en-US", "Document Change Log Information_us", "实体名称"),
            // entity.documentchangelog._self
            new TranslationSeedItem("entity.documentchangelog._self", "ja-JP", "文管文档变更日志信息_jp", "实体名称"),
            // entity.documentchangelog._self
            new TranslationSeedItem("entity.documentchangelog._self", "zh-CN", "文管文档变更日志信息", "实体名称"),
            // entity.documentchangelog._self
            new TranslationSeedItem("entity.documentchangelog._self", "zh-HK", "文管文档变更日志信息_hk", "实体名称"),

            // entity.documentchangelog.documentid
            new TranslationSeedItem("entity.documentchangelog.documentid", "en-US", "文档ID_us", "文档 ID"),
            // entity.documentchangelog.documentid
            new TranslationSeedItem("entity.documentchangelog.documentid", "ja-JP", "文档ID_jp", "文档 ID"),
            // entity.documentchangelog.documentid
            new TranslationSeedItem("entity.documentchangelog.documentid", "zh-CN", "文档ID", "文档 ID"),
            // entity.documentchangelog.documentid
            new TranslationSeedItem("entity.documentchangelog.documentid", "zh-HK", "文档ID_hk", "文档 ID"),

            // entity.documentchangelog.documentcode
            new TranslationSeedItem("entity.documentchangelog.documentcode", "en-US", "文档编码_us", "文档编码（冗余，便于日志列表展示）"),
            // entity.documentchangelog.documentcode
            new TranslationSeedItem("entity.documentchangelog.documentcode", "ja-JP", "文档编码_jp", "文档编码（冗余，便于日志列表展示）"),
            // entity.documentchangelog.documentcode
            new TranslationSeedItem("entity.documentchangelog.documentcode", "zh-CN", "文档编码", "文档编码（冗余，便于日志列表展示）"),
            // entity.documentchangelog.documentcode
            new TranslationSeedItem("entity.documentchangelog.documentcode", "zh-HK", "文档编码_hk", "文档编码（冗余，便于日志列表展示）"),

            // entity.documentchangelog.documenttitle
            new TranslationSeedItem("entity.documentchangelog.documenttitle", "en-US", "文档标题_us", "文档标题（冗余，便于日志列表展示）"),
            // entity.documentchangelog.documenttitle
            new TranslationSeedItem("entity.documentchangelog.documenttitle", "ja-JP", "文档标题_jp", "文档标题（冗余，便于日志列表展示）"),
            // entity.documentchangelog.documenttitle
            new TranslationSeedItem("entity.documentchangelog.documenttitle", "zh-CN", "文档标题", "文档标题（冗余，便于日志列表展示）"),
            // entity.documentchangelog.documenttitle
            new TranslationSeedItem("entity.documentchangelog.documenttitle", "zh-HK", "文档标题_hk", "文档标题（冗余，便于日志列表展示）"),

            // entity.documentchangelog.changetype
            new TranslationSeedItem("entity.documentchangelog.changetype", "en-US", "变更类型_us", "变更类型"),
            // entity.documentchangelog.changetype
            new TranslationSeedItem("entity.documentchangelog.changetype", "ja-JP", "变更类型_jp", "变更类型"),
            // entity.documentchangelog.changetype
            new TranslationSeedItem("entity.documentchangelog.changetype", "zh-CN", "变更类型", "变更类型"),
            // entity.documentchangelog.changetype
            new TranslationSeedItem("entity.documentchangelog.changetype", "zh-HK", "变更类型_hk", "变更类型"),

            // entity.documentchangelog.changesummary
            new TranslationSeedItem("entity.documentchangelog.changesummary", "en-US", "变更内容摘要_us", "变更内容摘要"),
            // entity.documentchangelog.changesummary
            new TranslationSeedItem("entity.documentchangelog.changesummary", "ja-JP", "变更内容摘要_jp", "变更内容摘要"),
            // entity.documentchangelog.changesummary
            new TranslationSeedItem("entity.documentchangelog.changesummary", "zh-CN", "变更内容摘要", "变更内容摘要"),
            // entity.documentchangelog.changesummary
            new TranslationSeedItem("entity.documentchangelog.changesummary", "zh-HK", "变更内容摘要_hk", "变更内容摘要"),

            // entity.documentchangelog.changefields
            new TranslationSeedItem("entity.documentchangelog.changefields", "en-US", "变更字段列表_us", "变更字段列表（JSON 数组）"),
            // entity.documentchangelog.changefields
            new TranslationSeedItem("entity.documentchangelog.changefields", "ja-JP", "变更字段列表_jp", "变更字段列表（JSON 数组）"),
            // entity.documentchangelog.changefields
            new TranslationSeedItem("entity.documentchangelog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON 数组）"),
            // entity.documentchangelog.changefields
            new TranslationSeedItem("entity.documentchangelog.changefields", "zh-HK", "变更字段列表_hk", "变更字段列表（JSON 数组）"),

            // entity.documentchangelog.changereason
            new TranslationSeedItem("entity.documentchangelog.changereason", "en-US", "变更原因_us", "变更原因或备注"),
            // entity.documentchangelog.changereason
            new TranslationSeedItem("entity.documentchangelog.changereason", "ja-JP", "变更原因_jp", "变更原因或备注"),
            // entity.documentchangelog.changereason
            new TranslationSeedItem("entity.documentchangelog.changereason", "zh-CN", "变更原因", "变更原因或备注"),
            // entity.documentchangelog.changereason
            new TranslationSeedItem("entity.documentchangelog.changereason", "zh-HK", "变更原因_hk", "变更原因或备注"),

            // entity.documentchangelog.versionatchange
            new TranslationSeedItem("entity.documentchangelog.versionatchange", "en-US", "变更时文档版本号_us", "变更时文档版本号"),
            // entity.documentchangelog.versionatchange
            new TranslationSeedItem("entity.documentchangelog.versionatchange", "ja-JP", "变更时文档版本号_jp", "变更时文档版本号"),
            // entity.documentchangelog.versionatchange
            new TranslationSeedItem("entity.documentchangelog.versionatchange", "zh-CN", "变更时文档版本号", "变更时文档版本号"),
            // entity.documentchangelog.versionatchange
            new TranslationSeedItem("entity.documentchangelog.versionatchange", "zh-HK", "变更时文档版本号_hk", "变更时文档版本号"),

            // entity.documentchangelog.document
            new TranslationSeedItem("entity.documentchangelog.document", "en-US", "文档_us", "文档（主表）"),
            // entity.documentchangelog.document
            new TranslationSeedItem("entity.documentchangelog.document", "ja-JP", "文档_jp", "文档（主表）"),
            // entity.documentchangelog.document
            new TranslationSeedItem("entity.documentchangelog.document", "zh-CN", "文档", "文档（主表）"),
            // entity.documentchangelog.document
            new TranslationSeedItem("entity.documentchangelog.document", "zh-HK", "文档_hk", "文档（主表）"),
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
        translation.ResourceGroup = "DocumentCenter";
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
