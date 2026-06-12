// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine
// 文件名称：TaktNewsAttachmentI18nSeedData.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNewsAttachment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktNewsAttachment 实体国际化翻译种子（键前缀 entity.newsAttachment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNewsAttachmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNewsAttachment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 newsAttachment 实体翻译...", tenantCode);

        foreach (var item in GetNewsAttachmentTranslations())
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

        TaktLogger.Information("TaktNewsAttachment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNewsAttachment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.newsAttachment._self / entity.newsAttachment.{{field}}；ResourceGroup=2；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetNewsAttachmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.newsAttachment._self
            new TranslationSeedItem("entity.newsAttachment._self", "en-US", "News Attachment Information", "实体名称"),
            // entity.newsAttachment._self
            new TranslationSeedItem("entity.newsAttachment._self", "ja-JP", "新闻附件信息", "实体名称"),
            // entity.newsAttachment._self
            new TranslationSeedItem("entity.newsAttachment._self", "zh-CN", "新闻附件信息", "实体名称"),
            // entity.newsAttachment._self
            new TranslationSeedItem("entity.newsAttachment._self", "zh-HK", "新闻附件信息", "实体名称"),

            // entity.newsAttachment.newsid
            new TranslationSeedItem("entity.newsAttachment.newsid", "en-US", "新闻ID", "新闻 ID"),
            // entity.newsAttachment.newsid
            new TranslationSeedItem("entity.newsAttachment.newsid", "ja-JP", "新闻ID", "新闻 ID"),
            // entity.newsAttachment.newsid
            new TranslationSeedItem("entity.newsAttachment.newsid", "zh-CN", "新闻ID", "新闻 ID"),
            // entity.newsAttachment.newsid
            new TranslationSeedItem("entity.newsAttachment.newsid", "zh-HK", "新闻ID", "新闻 ID"),

            // entity.newsAttachment.fileid
            new TranslationSeedItem("entity.newsAttachment.fileid", "en-US", "文件ID", "文件 ID"),
            // entity.newsAttachment.fileid
            new TranslationSeedItem("entity.newsAttachment.fileid", "ja-JP", "文件ID", "文件 ID"),
            // entity.newsAttachment.fileid
            new TranslationSeedItem("entity.newsAttachment.fileid", "zh-CN", "文件ID", "文件 ID"),
            // entity.newsAttachment.fileid
            new TranslationSeedItem("entity.newsAttachment.fileid", "zh-HK", "文件ID", "文件 ID"),

            // entity.newsAttachment.filename
            new TranslationSeedItem("entity.newsAttachment.filename", "en-US", "文件名称", "文件名称"),
            // entity.newsAttachment.filename
            new TranslationSeedItem("entity.newsAttachment.filename", "ja-JP", "文件名称", "文件名称"),
            // entity.newsAttachment.filename
            new TranslationSeedItem("entity.newsAttachment.filename", "zh-CN", "文件名称", "文件名称"),
            // entity.newsAttachment.filename
            new TranslationSeedItem("entity.newsAttachment.filename", "zh-HK", "文件名称", "文件名称"),

            // entity.newsAttachment.filepath
            new TranslationSeedItem("entity.newsAttachment.filepath", "en-US", "文件路径", "文件路径"),
            // entity.newsAttachment.filepath
            new TranslationSeedItem("entity.newsAttachment.filepath", "ja-JP", "文件路径", "文件路径"),
            // entity.newsAttachment.filepath
            new TranslationSeedItem("entity.newsAttachment.filepath", "zh-CN", "文件路径", "文件路径"),
            // entity.newsAttachment.filepath
            new TranslationSeedItem("entity.newsAttachment.filepath", "zh-HK", "文件路径", "文件路径"),

            // entity.newsAttachment.filesize
            new TranslationSeedItem("entity.newsAttachment.filesize", "en-US", "文件大小（字节）", "文件大小（字节）"),
            // entity.newsAttachment.filesize
            new TranslationSeedItem("entity.newsAttachment.filesize", "ja-JP", "文件大小（字节）", "文件大小（字节）"),
            // entity.newsAttachment.filesize
            new TranslationSeedItem("entity.newsAttachment.filesize", "zh-CN", "文件大小（字节）", "文件大小（字节）"),
            // entity.newsAttachment.filesize
            new TranslationSeedItem("entity.newsAttachment.filesize", "zh-HK", "文件大小（字节）", "文件大小（字节）"),

            // entity.newsAttachment.filetype
            new TranslationSeedItem("entity.newsAttachment.filetype", "en-US", "文件类型", "文件类型（MIME）"),
            // entity.newsAttachment.filetype
            new TranslationSeedItem("entity.newsAttachment.filetype", "ja-JP", "文件类型", "文件类型（MIME）"),
            // entity.newsAttachment.filetype
            new TranslationSeedItem("entity.newsAttachment.filetype", "zh-CN", "文件类型", "文件类型（MIME）"),
            // entity.newsAttachment.filetype
            new TranslationSeedItem("entity.newsAttachment.filetype", "zh-HK", "文件类型", "文件类型（MIME）"),

            // entity.newsAttachment.fileextension
            new TranslationSeedItem("entity.newsAttachment.fileextension", "en-US", "文件扩展名", "文件扩展名"),
            // entity.newsAttachment.fileextension
            new TranslationSeedItem("entity.newsAttachment.fileextension", "ja-JP", "文件扩展名", "文件扩展名"),
            // entity.newsAttachment.fileextension
            new TranslationSeedItem("entity.newsAttachment.fileextension", "zh-CN", "文件扩展名", "文件扩展名"),
            // entity.newsAttachment.fileextension
            new TranslationSeedItem("entity.newsAttachment.fileextension", "zh-HK", "文件扩展名", "文件扩展名"),

            // entity.newsAttachment.sortorder
            new TranslationSeedItem("entity.newsAttachment.sortorder", "en-US", "排序号", "排序号"),
            // entity.newsAttachment.sortorder
            new TranslationSeedItem("entity.newsAttachment.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.newsAttachment.sortorder
            new TranslationSeedItem("entity.newsAttachment.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.newsAttachment.sortorder
            new TranslationSeedItem("entity.newsAttachment.sortorder", "zh-HK", "排序号", "排序号"),
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
