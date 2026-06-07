// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcAttachmentI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcAttachment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEcAttachment 实体国际化翻译种子（键前缀 entity.ecAttachment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcAttachmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcAttachment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecAttachment 实体翻译...", tenantCode);

        foreach (var item in GetEcAttachmentTranslations())
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

        TaktLogger.Information("TaktEcAttachment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcAttachment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecAttachment._self / entity.ecAttachment.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcAttachmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecAttachment._self
            new TranslationSeedItem("entity.ecAttachment._self", "en-US", "Ec Attachment Information", "实体名称"),
            // entity.ecAttachment._self
            new TranslationSeedItem("entity.ecAttachment._self", "ja-JP", "设变附件信息", "实体名称"),
            // entity.ecAttachment._self
            new TranslationSeedItem("entity.ecAttachment._self", "zh-CN", "设变附件信息", "实体名称"),
            // entity.ecAttachment._self
            new TranslationSeedItem("entity.ecAttachment._self", "zh-HK", "设变附件信息", "实体名称"),

            // entity.ecAttachment.ecid
            new TranslationSeedItem("entity.ecAttachment.ecid", "en-US", "设变ID", "设变主表ID"),
            // entity.ecAttachment.ecid
            new TranslationSeedItem("entity.ecAttachment.ecid", "ja-JP", "设变ID", "设变主表ID"),
            // entity.ecAttachment.ecid
            new TranslationSeedItem("entity.ecAttachment.ecid", "zh-CN", "设变ID", "设变主表ID"),
            // entity.ecAttachment.ecid
            new TranslationSeedItem("entity.ecAttachment.ecid", "zh-HK", "设变ID", "设变主表ID"),

            // entity.ecAttachment.ecno
            new TranslationSeedItem("entity.ecAttachment.ecno", "en-US", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecAttachment.ecno
            new TranslationSeedItem("entity.ecAttachment.ecno", "ja-JP", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecAttachment.ecno
            new TranslationSeedItem("entity.ecAttachment.ecno", "zh-CN", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecAttachment.ecno
            new TranslationSeedItem("entity.ecAttachment.ecno", "zh-HK", "设变单号", "设变单号（冗余字段,便于查询）"),

            // entity.ecAttachment.linenumber
            new TranslationSeedItem("entity.ecAttachment.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecAttachment.linenumber
            new TranslationSeedItem("entity.ecAttachment.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecAttachment.linenumber
            new TranslationSeedItem("entity.ecAttachment.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecAttachment.linenumber
            new TranslationSeedItem("entity.ecAttachment.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.ecAttachment.attachmenttype
            new TranslationSeedItem("entity.ecAttachment.attachmenttype", "en-US", "文件类别", "文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等"),
            // entity.ecAttachment.attachmenttype
            new TranslationSeedItem("entity.ecAttachment.attachmenttype", "ja-JP", "文件类别", "文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等"),
            // entity.ecAttachment.attachmenttype
            new TranslationSeedItem("entity.ecAttachment.attachmenttype", "zh-CN", "文件类别", "文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等"),
            // entity.ecAttachment.attachmenttype
            new TranslationSeedItem("entity.ecAttachment.attachmenttype", "zh-HK", "文件类别", "文件类别：Liaison=联络, EPP, FPP, ExternalLiaison=外部联络, TCJ 等"),

            // entity.ecAttachment.docno
            new TranslationSeedItem("entity.ecAttachment.docno", "en-US", "文件编号", "文件编号（如联络编号等）"),
            // entity.ecAttachment.docno
            new TranslationSeedItem("entity.ecAttachment.docno", "ja-JP", "文件编号", "文件编号（如联络编号等）"),
            // entity.ecAttachment.docno
            new TranslationSeedItem("entity.ecAttachment.docno", "zh-CN", "文件编号", "文件编号（如联络编号等）"),
            // entity.ecAttachment.docno
            new TranslationSeedItem("entity.ecAttachment.docno", "zh-HK", "文件编号", "文件编号（如联络编号等）"),

            // entity.ecAttachment.filename
            new TranslationSeedItem("entity.ecAttachment.filename", "en-US", "文件名称", "文件名称"),
            // entity.ecAttachment.filename
            new TranslationSeedItem("entity.ecAttachment.filename", "ja-JP", "文件名称", "文件名称"),
            // entity.ecAttachment.filename
            new TranslationSeedItem("entity.ecAttachment.filename", "zh-CN", "文件名称", "文件名称"),
            // entity.ecAttachment.filename
            new TranslationSeedItem("entity.ecAttachment.filename", "zh-HK", "文件名称", "文件名称"),

            // entity.ecAttachment.accessurl
            new TranslationSeedItem("entity.ecAttachment.accessurl", "en-US", "访问地址", "访问地址（URL）"),
            // entity.ecAttachment.accessurl
            new TranslationSeedItem("entity.ecAttachment.accessurl", "ja-JP", "访问地址", "访问地址（URL）"),
            // entity.ecAttachment.accessurl
            new TranslationSeedItem("entity.ecAttachment.accessurl", "zh-CN", "访问地址", "访问地址（URL）"),
            // entity.ecAttachment.accessurl
            new TranslationSeedItem("entity.ecAttachment.accessurl", "zh-HK", "访问地址", "访问地址（URL）"),
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
        translation.ResourceGroup = TaktModule.Logistics;
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
