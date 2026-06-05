// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk
// 文件名称：TaktTicketChangeLogI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTicketChangeLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk;

/// <summary>
/// TaktTicketChangeLog 实体国际化翻译种子（键前缀 entity.ticketChangeLog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTicketChangeLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTicketChangeLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ticketChangeLog 实体翻译...", tenantCode);

        foreach (var item in GetTicketChangeLogTranslations())
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

        TaktLogger.Information("TaktTicketChangeLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTicketChangeLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ticketChangeLog._self / entity.ticketChangeLog.{{field}}；ResourceGroup=TaktModule.Routine；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTicketChangeLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ticketChangeLog._self
            new TranslationSeedItem("entity.ticketChangeLog._self", "en-US", "Ticket Change Log Information", "实体名称"),
            // entity.ticketChangeLog._self
            new TranslationSeedItem("entity.ticketChangeLog._self", "ja-JP", "工单变更日志信息", "实体名称"),
            // entity.ticketChangeLog._self
            new TranslationSeedItem("entity.ticketChangeLog._self", "zh-CN", "工单变更日志信息", "实体名称"),
            // entity.ticketChangeLog._self
            new TranslationSeedItem("entity.ticketChangeLog._self", "zh-HK", "工单变更日志信息", "实体名称"),

            // entity.ticketChangeLog.ticketid
            new TranslationSeedItem("entity.ticketChangeLog.ticketid", "en-US", "工单ID", "工单 ID"),
            // entity.ticketChangeLog.ticketid
            new TranslationSeedItem("entity.ticketChangeLog.ticketid", "ja-JP", "工单ID", "工单 ID"),
            // entity.ticketChangeLog.ticketid
            new TranslationSeedItem("entity.ticketChangeLog.ticketid", "zh-CN", "工单ID", "工单 ID"),
            // entity.ticketChangeLog.ticketid
            new TranslationSeedItem("entity.ticketChangeLog.ticketid", "zh-HK", "工单ID", "工单 ID"),

            // entity.ticketChangeLog.ticketno
            new TranslationSeedItem("entity.ticketChangeLog.ticketno", "en-US", "工单编号", "工单编号（冗余，便于日志列表展示）"),
            // entity.ticketChangeLog.ticketno
            new TranslationSeedItem("entity.ticketChangeLog.ticketno", "ja-JP", "工单编号", "工单编号（冗余，便于日志列表展示）"),
            // entity.ticketChangeLog.ticketno
            new TranslationSeedItem("entity.ticketChangeLog.ticketno", "zh-CN", "工单编号", "工单编号（冗余，便于日志列表展示）"),
            // entity.ticketChangeLog.ticketno
            new TranslationSeedItem("entity.ticketChangeLog.ticketno", "zh-HK", "工单编号", "工单编号（冗余，便于日志列表展示）"),

            // entity.ticketChangeLog.changetype
            new TranslationSeedItem("entity.ticketChangeLog.changetype", "en-US", "变更类型", "变更类型（0=创建，1=更新，2=关闭/删除）"),
            // entity.ticketChangeLog.changetype
            new TranslationSeedItem("entity.ticketChangeLog.changetype", "ja-JP", "变更类型", "变更类型（0=创建，1=更新，2=关闭/删除）"),
            // entity.ticketChangeLog.changetype
            new TranslationSeedItem("entity.ticketChangeLog.changetype", "zh-CN", "变更类型", "变更类型（0=创建，1=更新，2=关闭/删除）"),
            // entity.ticketChangeLog.changetype
            new TranslationSeedItem("entity.ticketChangeLog.changetype", "zh-HK", "变更类型", "变更类型（0=创建，1=更新，2=关闭/删除）"),

            // entity.ticketChangeLog.changesummary
            new TranslationSeedItem("entity.ticketChangeLog.changesummary", "en-US", "修改工单内容摘要", "修改工单内容摘要"),
            // entity.ticketChangeLog.changesummary
            new TranslationSeedItem("entity.ticketChangeLog.changesummary", "ja-JP", "修改工单内容摘要", "修改工单内容摘要"),
            // entity.ticketChangeLog.changesummary
            new TranslationSeedItem("entity.ticketChangeLog.changesummary", "zh-CN", "修改工单内容摘要", "修改工单内容摘要"),
            // entity.ticketChangeLog.changesummary
            new TranslationSeedItem("entity.ticketChangeLog.changesummary", "zh-HK", "修改工单内容摘要", "修改工单内容摘要"),

            // entity.ticketChangeLog.changefields
            new TranslationSeedItem("entity.ticketChangeLog.changefields", "en-US", "变更字段列表", "变更字段列表（JSON 数组）"),
            // entity.ticketChangeLog.changefields
            new TranslationSeedItem("entity.ticketChangeLog.changefields", "ja-JP", "变更字段列表", "变更字段列表（JSON 数组）"),
            // entity.ticketChangeLog.changefields
            new TranslationSeedItem("entity.ticketChangeLog.changefields", "zh-CN", "变更字段列表", "变更字段列表（JSON 数组）"),
            // entity.ticketChangeLog.changefields
            new TranslationSeedItem("entity.ticketChangeLog.changefields", "zh-HK", "变更字段列表", "变更字段列表（JSON 数组）"),

            // entity.ticketChangeLog.changereason
            new TranslationSeedItem("entity.ticketChangeLog.changereason", "en-US", "变更原因", "变更原因或备注"),
            // entity.ticketChangeLog.changereason
            new TranslationSeedItem("entity.ticketChangeLog.changereason", "ja-JP", "变更原因", "变更原因或备注"),
            // entity.ticketChangeLog.changereason
            new TranslationSeedItem("entity.ticketChangeLog.changereason", "zh-CN", "变更原因", "变更原因或备注"),
            // entity.ticketChangeLog.changereason
            new TranslationSeedItem("entity.ticketChangeLog.changereason", "zh-HK", "变更原因", "变更原因或备注"),
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
