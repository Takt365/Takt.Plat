// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.DocumentCenter
// 文件名称：TaktDocumentVersionI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDocumentVersion 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.DocumentCenter;

/// <summary>
/// TaktDocumentVersion 实体国际化翻译种子（键前缀 entity.documentVersion.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDocumentVersionI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDocumentVersion 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 documentVersion 实体翻译...", tenantCode);

        foreach (var item in GetDocumentVersionTranslations())
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

        TaktLogger.Information("TaktDocumentVersion 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDocumentVersion 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.documentVersion._self / entity.documentVersion.{{field}}；ResourceGroup=TaktModule.Routine；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDocumentVersionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.documentVersion._self
            new TranslationSeedItem("entity.documentVersion._self", "en-US", "Document Version Information", "实体名称"),
            // entity.documentVersion._self
            new TranslationSeedItem("entity.documentVersion._self", "ja-JP", "文管文档版本子信息", "实体名称"),
            // entity.documentVersion._self
            new TranslationSeedItem("entity.documentVersion._self", "zh-CN", "文管文档版本子信息", "实体名称"),
            // entity.documentVersion._self
            new TranslationSeedItem("entity.documentVersion._self", "zh-HK", "文管文档版本子信息", "实体名称"),

            // entity.documentVersion.documentid
            new TranslationSeedItem("entity.documentVersion.documentid", "en-US", "文档ID", "文档 ID"),
            // entity.documentVersion.documentid
            new TranslationSeedItem("entity.documentVersion.documentid", "ja-JP", "文档ID", "文档 ID"),
            // entity.documentVersion.documentid
            new TranslationSeedItem("entity.documentVersion.documentid", "zh-CN", "文档ID", "文档 ID"),
            // entity.documentVersion.documentid
            new TranslationSeedItem("entity.documentVersion.documentid", "zh-HK", "文档ID", "文档 ID"),

            // entity.documentVersion.versionno
            new TranslationSeedItem("entity.documentVersion.versionno", "en-US", "版本号", "版本号"),
            // entity.documentVersion.versionno
            new TranslationSeedItem("entity.documentVersion.versionno", "ja-JP", "版本号", "版本号"),
            // entity.documentVersion.versionno
            new TranslationSeedItem("entity.documentVersion.versionno", "zh-CN", "版本号", "版本号"),
            // entity.documentVersion.versionno
            new TranslationSeedItem("entity.documentVersion.versionno", "zh-HK", "版本号", "版本号"),

            // entity.documentVersion.versionnote
            new TranslationSeedItem("entity.documentVersion.versionnote", "en-US", "版本说明", "版本说明"),
            // entity.documentVersion.versionnote
            new TranslationSeedItem("entity.documentVersion.versionnote", "ja-JP", "版本说明", "版本说明"),
            // entity.documentVersion.versionnote
            new TranslationSeedItem("entity.documentVersion.versionnote", "zh-CN", "版本说明", "版本说明"),
            // entity.documentVersion.versionnote
            new TranslationSeedItem("entity.documentVersion.versionnote", "zh-HK", "版本说明", "版本说明"),

            // entity.documentVersion.fileid
            new TranslationSeedItem("entity.documentVersion.fileid", "en-US", "文件ID", "文件 ID"),
            // entity.documentVersion.fileid
            new TranslationSeedItem("entity.documentVersion.fileid", "ja-JP", "文件ID", "文件 ID"),
            // entity.documentVersion.fileid
            new TranslationSeedItem("entity.documentVersion.fileid", "zh-CN", "文件ID", "文件 ID"),
            // entity.documentVersion.fileid
            new TranslationSeedItem("entity.documentVersion.fileid", "zh-HK", "文件ID", "文件 ID"),

            // entity.documentVersion.filename
            new TranslationSeedItem("entity.documentVersion.filename", "en-US", "文件名称", "文件名称"),
            // entity.documentVersion.filename
            new TranslationSeedItem("entity.documentVersion.filename", "ja-JP", "文件名称", "文件名称"),
            // entity.documentVersion.filename
            new TranslationSeedItem("entity.documentVersion.filename", "zh-CN", "文件名称", "文件名称"),
            // entity.documentVersion.filename
            new TranslationSeedItem("entity.documentVersion.filename", "zh-HK", "文件名称", "文件名称"),

            // entity.documentVersion.filepath
            new TranslationSeedItem("entity.documentVersion.filepath", "en-US", "文件路径", "文件路径"),
            // entity.documentVersion.filepath
            new TranslationSeedItem("entity.documentVersion.filepath", "ja-JP", "文件路径", "文件路径"),
            // entity.documentVersion.filepath
            new TranslationSeedItem("entity.documentVersion.filepath", "zh-CN", "文件路径", "文件路径"),
            // entity.documentVersion.filepath
            new TranslationSeedItem("entity.documentVersion.filepath", "zh-HK", "文件路径", "文件路径"),

            // entity.documentVersion.filesize
            new TranslationSeedItem("entity.documentVersion.filesize", "en-US", "文件大小", "文件大小（字节）"),
            // entity.documentVersion.filesize
            new TranslationSeedItem("entity.documentVersion.filesize", "ja-JP", "文件大小", "文件大小（字节）"),
            // entity.documentVersion.filesize
            new TranslationSeedItem("entity.documentVersion.filesize", "zh-CN", "文件大小", "文件大小（字节）"),
            // entity.documentVersion.filesize
            new TranslationSeedItem("entity.documentVersion.filesize", "zh-HK", "文件大小", "文件大小（字节）"),

            // entity.documentVersion.filetype
            new TranslationSeedItem("entity.documentVersion.filetype", "en-US", "文件类型", "文件类型（MIME）"),
            // entity.documentVersion.filetype
            new TranslationSeedItem("entity.documentVersion.filetype", "ja-JP", "文件类型", "文件类型（MIME）"),
            // entity.documentVersion.filetype
            new TranslationSeedItem("entity.documentVersion.filetype", "zh-CN", "文件类型", "文件类型（MIME）"),
            // entity.documentVersion.filetype
            new TranslationSeedItem("entity.documentVersion.filetype", "zh-HK", "文件类型", "文件类型（MIME）"),

            // entity.documentVersion.fileextension
            new TranslationSeedItem("entity.documentVersion.fileextension", "en-US", "文件扩展名", "文件扩展名"),
            // entity.documentVersion.fileextension
            new TranslationSeedItem("entity.documentVersion.fileextension", "ja-JP", "文件扩展名", "文件扩展名"),
            // entity.documentVersion.fileextension
            new TranslationSeedItem("entity.documentVersion.fileextension", "zh-CN", "文件扩展名", "文件扩展名"),
            // entity.documentVersion.fileextension
            new TranslationSeedItem("entity.documentVersion.fileextension", "zh-HK", "文件扩展名", "文件扩展名"),

            // entity.documentVersion.revisedby
            new TranslationSeedItem("entity.documentVersion.revisedby", "en-US", "修订人ID", "修订人 ID"),
            // entity.documentVersion.revisedby
            new TranslationSeedItem("entity.documentVersion.revisedby", "ja-JP", "修订人ID", "修订人 ID"),
            // entity.documentVersion.revisedby
            new TranslationSeedItem("entity.documentVersion.revisedby", "zh-CN", "修订人ID", "修订人 ID"),
            // entity.documentVersion.revisedby
            new TranslationSeedItem("entity.documentVersion.revisedby", "zh-HK", "修订人ID", "修订人 ID"),

            // entity.documentVersion.revisedbyname
            new TranslationSeedItem("entity.documentVersion.revisedbyname", "en-US", "修订人姓名", "修订人姓名"),
            // entity.documentVersion.revisedbyname
            new TranslationSeedItem("entity.documentVersion.revisedbyname", "ja-JP", "修订人姓名", "修订人姓名"),
            // entity.documentVersion.revisedbyname
            new TranslationSeedItem("entity.documentVersion.revisedbyname", "zh-CN", "修订人姓名", "修订人姓名"),
            // entity.documentVersion.revisedbyname
            new TranslationSeedItem("entity.documentVersion.revisedbyname", "zh-HK", "修订人姓名", "修订人姓名"),

            // entity.documentVersion.revisedat
            new TranslationSeedItem("entity.documentVersion.revisedat", "en-US", "修订时间", "修订时间"),
            // entity.documentVersion.revisedat
            new TranslationSeedItem("entity.documentVersion.revisedat", "ja-JP", "修订时间", "修订时间"),
            // entity.documentVersion.revisedat
            new TranslationSeedItem("entity.documentVersion.revisedat", "zh-CN", "修订时间", "修订时间"),
            // entity.documentVersion.revisedat
            new TranslationSeedItem("entity.documentVersion.revisedat", "zh-HK", "修订时间", "修订时间"),
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
        translation.ResourceGroup = TaktModule.Routine;
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
