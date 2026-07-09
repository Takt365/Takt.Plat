// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.DocumentCenter
// 文件名称：TaktDocumentVersionI18nSeedData.cs
// 创建时间：2026-07-09
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.DocumentCenter;

/// <summary>
/// TaktDocumentVersion 实体国际化翻译种子（键前缀 entity.documentversion.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 documentversion 实体翻译...", tenantCode);

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
    /// I18nKey：entity.documentversion._self / entity.documentversion.{{field}}；ResourceGroup=DocumentCenter；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDocumentVersionTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.documentversion._self
            new TranslationSeedItem("entity.documentversion._self", "en-US", "Document Version Information_us", "实体名称"),
            // entity.documentversion._self
            new TranslationSeedItem("entity.documentversion._self", "ja-JP", "文管文档版本子信息_jp", "实体名称"),
            // entity.documentversion._self
            new TranslationSeedItem("entity.documentversion._self", "zh-CN", "文管文档版本子信息", "实体名称"),
            // entity.documentversion._self
            new TranslationSeedItem("entity.documentversion._self", "zh-HK", "文管文档版本子信息_hk", "实体名称"),

            // entity.documentversion.documentid
            new TranslationSeedItem("entity.documentversion.documentid", "en-US", "文档ID_us", "文档 ID（关联 TaktDocument.Id，选项 TaktDocuments/options）"),
            // entity.documentversion.documentid
            new TranslationSeedItem("entity.documentversion.documentid", "ja-JP", "文档ID_jp", "文档 ID（关联 TaktDocument.Id，选项 TaktDocuments/options）"),
            // entity.documentversion.documentid
            new TranslationSeedItem("entity.documentversion.documentid", "zh-CN", "文档ID", "文档 ID（关联 TaktDocument.Id，选项 TaktDocuments/options）"),
            // entity.documentversion.documentid
            new TranslationSeedItem("entity.documentversion.documentid", "zh-HK", "文档ID_hk", "文档 ID（关联 TaktDocument.Id，选项 TaktDocuments/options）"),

            // entity.documentversion.versionno
            new TranslationSeedItem("entity.documentversion.versionno", "en-US", "版本号_us", "版本号"),
            // entity.documentversion.versionno
            new TranslationSeedItem("entity.documentversion.versionno", "ja-JP", "版本号_jp", "版本号"),
            // entity.documentversion.versionno
            new TranslationSeedItem("entity.documentversion.versionno", "zh-CN", "版本号", "版本号"),
            // entity.documentversion.versionno
            new TranslationSeedItem("entity.documentversion.versionno", "zh-HK", "版本号_hk", "版本号"),

            // entity.documentversion.versionnote
            new TranslationSeedItem("entity.documentversion.versionnote", "en-US", "版本说明_us", "版本说明"),
            // entity.documentversion.versionnote
            new TranslationSeedItem("entity.documentversion.versionnote", "ja-JP", "版本说明_jp", "版本说明"),
            // entity.documentversion.versionnote
            new TranslationSeedItem("entity.documentversion.versionnote", "zh-CN", "版本说明", "版本说明"),
            // entity.documentversion.versionnote
            new TranslationSeedItem("entity.documentversion.versionnote", "zh-HK", "版本说明_hk", "版本说明"),

            // entity.documentversion.fileid
            new TranslationSeedItem("entity.documentversion.fileid", "en-US", "文件ID_us", "文件 ID（关联 TaktFile.Id，选项 TaktFiles/options）"),
            // entity.documentversion.fileid
            new TranslationSeedItem("entity.documentversion.fileid", "ja-JP", "文件ID_jp", "文件 ID（关联 TaktFile.Id，选项 TaktFiles/options）"),
            // entity.documentversion.fileid
            new TranslationSeedItem("entity.documentversion.fileid", "zh-CN", "文件ID", "文件 ID（关联 TaktFile.Id，选项 TaktFiles/options）"),
            // entity.documentversion.fileid
            new TranslationSeedItem("entity.documentversion.fileid", "zh-HK", "文件ID_hk", "文件 ID（关联 TaktFile.Id，选项 TaktFiles/options）"),

            // entity.documentversion.filename
            new TranslationSeedItem("entity.documentversion.filename", "en-US", "文件名称_us", "文件名称"),
            // entity.documentversion.filename
            new TranslationSeedItem("entity.documentversion.filename", "ja-JP", "文件名称_jp", "文件名称"),
            // entity.documentversion.filename
            new TranslationSeedItem("entity.documentversion.filename", "zh-CN", "文件名称", "文件名称"),
            // entity.documentversion.filename
            new TranslationSeedItem("entity.documentversion.filename", "zh-HK", "文件名称_hk", "文件名称"),

            // entity.documentversion.filepath
            new TranslationSeedItem("entity.documentversion.filepath", "en-US", "文件路径_us", "文件路径"),
            // entity.documentversion.filepath
            new TranslationSeedItem("entity.documentversion.filepath", "ja-JP", "文件路径_jp", "文件路径"),
            // entity.documentversion.filepath
            new TranslationSeedItem("entity.documentversion.filepath", "zh-CN", "文件路径", "文件路径"),
            // entity.documentversion.filepath
            new TranslationSeedItem("entity.documentversion.filepath", "zh-HK", "文件路径_hk", "文件路径"),

            // entity.documentversion.filesize
            new TranslationSeedItem("entity.documentversion.filesize", "en-US", "文件大小_us", "文件大小（字节）"),
            // entity.documentversion.filesize
            new TranslationSeedItem("entity.documentversion.filesize", "ja-JP", "文件大小_jp", "文件大小（字节）"),
            // entity.documentversion.filesize
            new TranslationSeedItem("entity.documentversion.filesize", "zh-CN", "文件大小", "文件大小（字节）"),
            // entity.documentversion.filesize
            new TranslationSeedItem("entity.documentversion.filesize", "zh-HK", "文件大小_hk", "文件大小（字节）"),

            // entity.documentversion.filetype
            new TranslationSeedItem("entity.documentversion.filetype", "en-US", "文件类型_us", "文件类型（MIME）"),
            // entity.documentversion.filetype
            new TranslationSeedItem("entity.documentversion.filetype", "ja-JP", "文件类型_jp", "文件类型（MIME）"),
            // entity.documentversion.filetype
            new TranslationSeedItem("entity.documentversion.filetype", "zh-CN", "文件类型", "文件类型（MIME）"),
            // entity.documentversion.filetype
            new TranslationSeedItem("entity.documentversion.filetype", "zh-HK", "文件类型_hk", "文件类型（MIME）"),

            // entity.documentversion.fileextension
            new TranslationSeedItem("entity.documentversion.fileextension", "en-US", "文件扩展名_us", "文件扩展名"),
            // entity.documentversion.fileextension
            new TranslationSeedItem("entity.documentversion.fileextension", "ja-JP", "文件扩展名_jp", "文件扩展名"),
            // entity.documentversion.fileextension
            new TranslationSeedItem("entity.documentversion.fileextension", "zh-CN", "文件扩展名", "文件扩展名"),
            // entity.documentversion.fileextension
            new TranslationSeedItem("entity.documentversion.fileextension", "zh-HK", "文件扩展名_hk", "文件扩展名"),

            // entity.documentversion.revisedby
            new TranslationSeedItem("entity.documentversion.revisedby", "en-US", "修订人ID_us", "修订人 ID（关联 TaktUser.Id，选项 TaktUsers/options）"),
            // entity.documentversion.revisedby
            new TranslationSeedItem("entity.documentversion.revisedby", "ja-JP", "修订人ID_jp", "修订人 ID（关联 TaktUser.Id，选项 TaktUsers/options）"),
            // entity.documentversion.revisedby
            new TranslationSeedItem("entity.documentversion.revisedby", "zh-CN", "修订人ID", "修订人 ID（关联 TaktUser.Id，选项 TaktUsers/options）"),
            // entity.documentversion.revisedby
            new TranslationSeedItem("entity.documentversion.revisedby", "zh-HK", "修订人ID_hk", "修订人 ID（关联 TaktUser.Id，选项 TaktUsers/options）"),

            // entity.documentversion.revisedbyname
            new TranslationSeedItem("entity.documentversion.revisedbyname", "en-US", "修订人姓名_us", "修订人姓名"),
            // entity.documentversion.revisedbyname
            new TranslationSeedItem("entity.documentversion.revisedbyname", "ja-JP", "修订人姓名_jp", "修订人姓名"),
            // entity.documentversion.revisedbyname
            new TranslationSeedItem("entity.documentversion.revisedbyname", "zh-CN", "修订人姓名", "修订人姓名"),
            // entity.documentversion.revisedbyname
            new TranslationSeedItem("entity.documentversion.revisedbyname", "zh-HK", "修订人姓名_hk", "修订人姓名"),

            // entity.documentversion.revisedat
            new TranslationSeedItem("entity.documentversion.revisedat", "en-US", "修订时间_us", "修订时间"),
            // entity.documentversion.revisedat
            new TranslationSeedItem("entity.documentversion.revisedat", "ja-JP", "修订时间_jp", "修订时间"),
            // entity.documentversion.revisedat
            new TranslationSeedItem("entity.documentversion.revisedat", "zh-CN", "修订时间", "修订时间"),
            // entity.documentversion.revisedat
            new TranslationSeedItem("entity.documentversion.revisedat", "zh-HK", "修订时间_hk", "修订时间"),

            // entity.documentversion.document
            new TranslationSeedItem("entity.documentversion.document", "en-US", "文档_us", "文档（主表）"),
            // entity.documentversion.document
            new TranslationSeedItem("entity.documentversion.document", "ja-JP", "文档_jp", "文档（主表）"),
            // entity.documentversion.document
            new TranslationSeedItem("entity.documentversion.document", "zh-CN", "文档", "文档（主表）"),
            // entity.documentversion.document
            new TranslationSeedItem("entity.documentversion.document", "zh-HK", "文档_hk", "文档（主表）"),
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
