// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine
// 文件名称：TaktDocumentChangeLogI18nSeedData.cs
// 创建时间：2026-06-04
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine;

/// <summary>
/// TaktDocumentChangeLog 实体国际化翻译种子（键前缀 entity.documentChangeLog.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 documentChangeLog 实体翻译...", tenantCode);

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
    /// I18nKey：entity.documentChangeLog._self / entity.documentChangeLog.{{field}}；ResourceGroup=2；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetDocumentChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.documentChangeLog._self
            new TranslationSeedItem("entity.documentChangeLog._self", "en-US", "Document Change Log Information", "实体名称"),
            // entity.documentChangeLog._self
            new TranslationSeedItem("entity.documentChangeLog._self", "ja-JP", "文管文档变更日志信息", "实体名称"),
            // entity.documentChangeLog._self
            new TranslationSeedItem("entity.documentChangeLog._self", "zh-CN", "文管文档变更日志信息", "实体名称"),
            // entity.documentChangeLog._self
            new TranslationSeedItem("entity.documentChangeLog._self", "zh-HK", "文管文档变更日志信息", "实体名称"),

            // entity.documentChangeLog.documentid
            new TranslationSeedItem("entity.documentChangeLog.documentid", "en-US", "文档ID", "文档 ID"),
            // entity.documentChangeLog.documentid
            new TranslationSeedItem("entity.documentChangeLog.documentid", "ja-JP", "文档ID", "文档 ID"),
            // entity.documentChangeLog.documentid
            new TranslationSeedItem("entity.documentChangeLog.documentid", "zh-CN", "文档ID", "文档 ID"),
            // entity.documentChangeLog.documentid
            new TranslationSeedItem("entity.documentChangeLog.documentid", "zh-HK", "文档ID", "文档 ID"),

            // entity.documentChangeLog.documentcode
            new TranslationSeedItem("entity.documentChangeLog.documentcode", "en-US", "文档编码", "文档编码（冗余，便于日志列表展示）"),
            // entity.documentChangeLog.documentcode
            new TranslationSeedItem("entity.documentChangeLog.documentcode", "ja-JP", "文档编码", "文档编码（冗余，便于日志列表展示）"),
            // entity.documentChangeLog.documentcode
            new TranslationSeedItem("entity.documentChangeLog.documentcode", "zh-CN", "文档编码", "文档编码（冗余，便于日志列表展示）"),
            // entity.documentChangeLog.documentcode
            new TranslationSeedItem("entity.documentChangeLog.documentcode", "zh-HK", "文档编码", "文档编码（冗余，便于日志列表展示）"),

            // entity.documentChangeLog.documenttitle
            new TranslationSeedItem("entity.documentChangeLog.documenttitle", "en-US", "文档标题", "文档标题（冗余，便于日志列表展示）"),
            // entity.documentChangeLog.documenttitle
            new TranslationSeedItem("entity.documentChangeLog.documenttitle", "ja-JP", "文档标题", "文档标题（冗余，便于日志列表展示）"),
            // entity.documentChangeLog.documenttitle
            new TranslationSeedItem("entity.documentChangeLog.documenttitle", "zh-CN", "文档标题", "文档标题（冗余，便于日志列表展示）"),
            // entity.documentChangeLog.documenttitle
            new TranslationSeedItem("entity.documentChangeLog.documenttitle", "zh-HK", "文档标题", "文档标题（冗余，便于日志列表展示）"),

            // entity.documentChangeLog.changetype
            new TranslationSeedItem("entity.documentChangeLog.changetype", "en-US", "变更类型", "变更类型"),
            // entity.documentChangeLog.changetype
            new TranslationSeedItem("entity.documentChangeLog.changetype", "ja-JP", "变更类型", "变更类型"),
            // entity.documentChangeLog.changetype
            new TranslationSeedItem("entity.documentChangeLog.changetype", "zh-CN", "变更类型", "变更类型"),
            // entity.documentChangeLog.changetype
            new TranslationSeedItem("entity.documentChangeLog.changetype", "zh-HK", "变更类型", "变更类型"),

            // entity.documentChangeLog.changesummary
            new TranslationSeedItem("entity.documentChangeLog.changesummary", "en-US", "变更内容摘要", "变更内容摘要（如「发布文档」「修订版本」「归档文档」等）"),
            // entity.documentChangeLog.changesummary
            new TranslationSeedItem("entity.documentChangeLog.changesummary", "ja-JP", "变更内容摘要", "变更内容摘要（如「发布文档」「修订版本」「归档文档」等）"),
            // entity.documentChangeLog.changesummary
            new TranslationSeedItem("entity.documentChangeLog.changesummary", "zh-CN", "变更内容摘要", "变更内容摘要（如「发布文档」「修订版本」「归档文档」等）"),
            // entity.documentChangeLog.changesummary
            new TranslationSeedItem("entity.documentChangeLog.changesummary", "zh-HK", "变更内容摘要", "变更内容摘要（如「发布文档」「修订版本」「归档文档」等）"),

            // entity.documentChangeLog.changefields
            new TranslationSeedItem("entity.documentChangeLog.changefields", "en-US", "变更字段列表", "变更字段列表（JSON 数组，记录字段旧值与新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.documentChangeLog.changefields
            new TranslationSeedItem("entity.documentChangeLog.changefields", "ja-JP", "变更字段列表", "变更字段列表（JSON 数组，记录字段旧值与新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.documentChangeLog.changefields
            new TranslationSeedItem("entity.documentChangeLog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON 数组，记录字段旧值与新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),
            // entity.documentChangeLog.changefields
            new TranslationSeedItem("entity.documentChangeLog.changefields", "zh-HK", "变更字段列表", "变更字段列表（JSON 数组，记录字段旧值与新值） 格式：[{\"field\":\"FieldName\",\"description\":\"字段描述\",\"oldValue\":\"旧值\",\"newValue\":\"新值\"}]"),

            // entity.documentChangeLog.changereason
            new TranslationSeedItem("entity.documentChangeLog.changereason", "en-US", "变更原因", "变更原因或备注（变更时间、变更人由基类 CreatedAt/CreatedBy 表示）"),
            // entity.documentChangeLog.changereason
            new TranslationSeedItem("entity.documentChangeLog.changereason", "ja-JP", "变更原因", "变更原因或备注（变更时间、变更人由基类 CreatedAt/CreatedBy 表示）"),
            // entity.documentChangeLog.changereason
            new TranslationSeedItem("entity.documentChangeLog.changereason", "zh-CN", "变更原因", "变更原因或备注（变更时间、变更人由基类 CreatedAt/CreatedBy 表示）"),
            // entity.documentChangeLog.changereason
            new TranslationSeedItem("entity.documentChangeLog.changereason", "zh-HK", "变更原因", "变更原因或备注（变更时间、变更人由基类 CreatedAt/CreatedBy 表示）"),

            // entity.documentChangeLog.versionatchange
            new TranslationSeedItem("entity.documentChangeLog.versionatchange", "en-US", "变更时版本号", "变更时文档版本号（与 TaktDocument.Version 对应）"),
            // entity.documentChangeLog.versionatchange
            new TranslationSeedItem("entity.documentChangeLog.versionatchange", "ja-JP", "变更时版本号", "变更时文档版本号（与 TaktDocument.Version 对应）"),
            // entity.documentChangeLog.versionatchange
            new TranslationSeedItem("entity.documentChangeLog.versionatchange", "zh-CN", "变更时版本号", "变更时文档版本号（与 TaktDocument.Version 对应）"),
            // entity.documentChangeLog.versionatchange
            new TranslationSeedItem("entity.documentChangeLog.versionatchange", "zh-HK", "变更时版本号", "变更时文档版本号（与 TaktDocument.Version 对应）"),
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
        translation.ResourceGroup = 2;
        translation.ResourceType = 0;
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
