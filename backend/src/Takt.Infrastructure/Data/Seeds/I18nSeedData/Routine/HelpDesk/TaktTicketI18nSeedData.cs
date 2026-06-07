// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Routine.HelpDesk
// 文件名称：TaktTicketI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktTicket 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktTicket 实体国际化翻译种子（键前缀 entity.ticket.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktTicketI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktTicket 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ticket 实体翻译...", tenantCode);

        foreach (var item in GetTicketTranslations())
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

        TaktLogger.Information("TaktTicket 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktTicket 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ticket._self / entity.ticket.{{field}}；ResourceGroup=TaktModule.Routine；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetTicketTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ticket._self
            new TranslationSeedItem("entity.ticket._self", "en-US", "Ticket Information", "实体名称"),
            // entity.ticket._self
            new TranslationSeedItem("entity.ticket._self", "ja-JP", "Takt工单信息", "实体名称"),
            // entity.ticket._self
            new TranslationSeedItem("entity.ticket._self", "zh-CN", "Takt工单信息", "实体名称"),
            // entity.ticket._self
            new TranslationSeedItem("entity.ticket._self", "zh-HK", "Takt工单信息", "实体名称"),

            // entity.ticket.no
            new TranslationSeedItem("entity.ticket.no", "en-US", "工单编号", "工单编号（唯一）"),
            // entity.ticket.no
            new TranslationSeedItem("entity.ticket.no", "ja-JP", "工单编号", "工单编号（唯一）"),
            // entity.ticket.no
            new TranslationSeedItem("entity.ticket.no", "zh-CN", "工单编号", "工单编号（唯一）"),
            // entity.ticket.no
            new TranslationSeedItem("entity.ticket.no", "zh-HK", "工单编号", "工单编号（唯一）"),

            // entity.ticket.title
            new TranslationSeedItem("entity.ticket.title", "en-US", "工单标题", "工单标题"),
            // entity.ticket.title
            new TranslationSeedItem("entity.ticket.title", "ja-JP", "工单标题", "工单标题"),
            // entity.ticket.title
            new TranslationSeedItem("entity.ticket.title", "zh-CN", "工单标题", "工单标题"),
            // entity.ticket.title
            new TranslationSeedItem("entity.ticket.title", "zh-HK", "工单标题", "工单标题"),

            // entity.ticket.content
            new TranslationSeedItem("entity.ticket.content", "en-US", "工单内容", "工单内容描述"),
            // entity.ticket.content
            new TranslationSeedItem("entity.ticket.content", "ja-JP", "工单内容", "工单内容描述"),
            // entity.ticket.content
            new TranslationSeedItem("entity.ticket.content", "zh-CN", "工单内容", "工单内容描述"),
            // entity.ticket.content
            new TranslationSeedItem("entity.ticket.content", "zh-HK", "工单内容", "工单内容描述"),

            // entity.ticket.attachmentsjson
            new TranslationSeedItem("entity.ticket.attachmentsjson", "en-US", "附件列表JSON", "附件列表 JSON。格式：[{ \"FileId\": 0, \"FileName\": \"\", \"FilePath\": \"\", \"FileSize\": 0, \"FileType\": \"\", \"FileExtension\": \"\", \"SortOrder\": 0 }]"),
            // entity.ticket.attachmentsjson
            new TranslationSeedItem("entity.ticket.attachmentsjson", "ja-JP", "附件列表JSON", "附件列表 JSON。格式：[{ \"FileId\": 0, \"FileName\": \"\", \"FilePath\": \"\", \"FileSize\": 0, \"FileType\": \"\", \"FileExtension\": \"\", \"SortOrder\": 0 }]"),
            // entity.ticket.attachmentsjson
            new TranslationSeedItem("entity.ticket.attachmentsjson", "zh-CN", "附件列表JSON", "附件列表 JSON。格式：[{ \"FileId\": 0, \"FileName\": \"\", \"FilePath\": \"\", \"FileSize\": 0, \"FileType\": \"\", \"FileExtension\": \"\", \"SortOrder\": 0 }]"),
            // entity.ticket.attachmentsjson
            new TranslationSeedItem("entity.ticket.attachmentsjson", "zh-HK", "附件列表JSON", "附件列表 JSON。格式：[{ \"FileId\": 0, \"FileName\": \"\", \"FilePath\": \"\", \"FileSize\": 0, \"FileType\": \"\", \"FileExtension\": \"\", \"SortOrder\": 0 }]"),

            // entity.ticket.status
            new TranslationSeedItem("entity.ticket.status", "en-US", "工单状态", "工单状态（0=待处理，1=处理中，2=已解决，3=已关闭）"),
            // entity.ticket.status
            new TranslationSeedItem("entity.ticket.status", "ja-JP", "工单状态", "工单状态（0=待处理，1=处理中，2=已解决，3=已关闭）"),
            // entity.ticket.status
            new TranslationSeedItem("entity.ticket.status", "zh-CN", "工单状态", "工单状态（0=待处理，1=处理中，2=已解决，3=已关闭）"),
            // entity.ticket.status
            new TranslationSeedItem("entity.ticket.status", "zh-HK", "工单状态", "工单状态（0=待处理，1=处理中，2=已解决，3=已关闭）"),

            // entity.ticket.priority
            new TranslationSeedItem("entity.ticket.priority", "en-US", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),
            // entity.ticket.priority
            new TranslationSeedItem("entity.ticket.priority", "ja-JP", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),
            // entity.ticket.priority
            new TranslationSeedItem("entity.ticket.priority", "zh-CN", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),
            // entity.ticket.priority
            new TranslationSeedItem("entity.ticket.priority", "zh-HK", "优先级", "优先级（0=低，1=中，2=高，3=紧急）"),

            // entity.ticket.categorycode
            new TranslationSeedItem("entity.ticket.categorycode", "en-US", "分类编码", "分类编码（如 incident/request 等）"),
            // entity.ticket.categorycode
            new TranslationSeedItem("entity.ticket.categorycode", "ja-JP", "分类编码", "分类编码（如 incident/request 等）"),
            // entity.ticket.categorycode
            new TranslationSeedItem("entity.ticket.categorycode", "zh-CN", "分类编码", "分类编码（如 incident/request 等）"),
            // entity.ticket.categorycode
            new TranslationSeedItem("entity.ticket.categorycode", "zh-HK", "分类编码", "分类编码（如 incident/request 等）"),

            // entity.ticket.source
            new TranslationSeedItem("entity.ticket.source", "en-US", "工单来源", "工单来源（0=门户网站，1=邮件，2=电话，3=API接入）"),
            // entity.ticket.source
            new TranslationSeedItem("entity.ticket.source", "ja-JP", "工单来源", "工单来源（0=门户网站，1=邮件，2=电话，3=API接入）"),
            // entity.ticket.source
            new TranslationSeedItem("entity.ticket.source", "zh-CN", "工单来源", "工单来源（0=门户网站，1=邮件，2=电话，3=API接入）"),
            // entity.ticket.source
            new TranslationSeedItem("entity.ticket.source", "zh-HK", "工单来源", "工单来源（0=门户网站，1=邮件，2=电话，3=API接入）"),

            // entity.ticket.submitterid
            new TranslationSeedItem("entity.ticket.submitterid", "en-US", "提交人ID", "提交人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ticket.submitterid
            new TranslationSeedItem("entity.ticket.submitterid", "ja-JP", "提交人ID", "提交人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ticket.submitterid
            new TranslationSeedItem("entity.ticket.submitterid", "zh-CN", "提交人ID", "提交人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ticket.submitterid
            new TranslationSeedItem("entity.ticket.submitterid", "zh-HK", "提交人ID", "提交人ID（序列化为string以避免Javascript精度问题）"),

            // entity.ticket.submittername
            new TranslationSeedItem("entity.ticket.submittername", "en-US", "提交人姓名", "提交人姓名"),
            // entity.ticket.submittername
            new TranslationSeedItem("entity.ticket.submittername", "ja-JP", "提交人姓名", "提交人姓名"),
            // entity.ticket.submittername
            new TranslationSeedItem("entity.ticket.submittername", "zh-CN", "提交人姓名", "提交人姓名"),
            // entity.ticket.submittername
            new TranslationSeedItem("entity.ticket.submittername", "zh-HK", "提交人姓名", "提交人姓名"),

            // entity.ticket.assigneeid
            new TranslationSeedItem("entity.ticket.assigneeid", "en-US", "处理人ID", "处理人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ticket.assigneeid
            new TranslationSeedItem("entity.ticket.assigneeid", "ja-JP", "处理人ID", "处理人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ticket.assigneeid
            new TranslationSeedItem("entity.ticket.assigneeid", "zh-CN", "处理人ID", "处理人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ticket.assigneeid
            new TranslationSeedItem("entity.ticket.assigneeid", "zh-HK", "处理人ID", "处理人ID（序列化为string以避免Javascript精度问题）"),

            // entity.ticket.assigneename
            new TranslationSeedItem("entity.ticket.assigneename", "en-US", "处理人姓名", "处理人姓名"),
            // entity.ticket.assigneename
            new TranslationSeedItem("entity.ticket.assigneename", "ja-JP", "处理人姓名", "处理人姓名"),
            // entity.ticket.assigneename
            new TranslationSeedItem("entity.ticket.assigneename", "zh-CN", "处理人姓名", "处理人姓名"),
            // entity.ticket.assigneename
            new TranslationSeedItem("entity.ticket.assigneename", "zh-HK", "处理人姓名", "处理人姓名"),

            // entity.ticket.knowledgeid
            new TranslationSeedItem("entity.ticket.knowledgeid", "en-US", "关联知识ID", "关联知识ID（可选，序列化为string以避免Javascript精度问题）"),
            // entity.ticket.knowledgeid
            new TranslationSeedItem("entity.ticket.knowledgeid", "ja-JP", "关联知识ID", "关联知识ID（可选，序列化为string以避免Javascript精度问题）"),
            // entity.ticket.knowledgeid
            new TranslationSeedItem("entity.ticket.knowledgeid", "zh-CN", "关联知识ID", "关联知识ID（可选，序列化为string以避免Javascript精度问题）"),
            // entity.ticket.knowledgeid
            new TranslationSeedItem("entity.ticket.knowledgeid", "zh-HK", "关联知识ID", "关联知识ID（可选，序列化为string以避免Javascript精度问题）"),

            // entity.ticket.parentticketid
            new TranslationSeedItem("entity.ticket.parentticketid", "en-US", "父工单ID", "父工单ID（为空表示顶级工单；非空表示该工单为子工单，序列化为string以避免Javascript精度问题）"),
            // entity.ticket.parentticketid
            new TranslationSeedItem("entity.ticket.parentticketid", "ja-JP", "父工单ID", "父工单ID（为空表示顶级工单；非空表示该工单为子工单，序列化为string以避免Javascript精度问题）"),
            // entity.ticket.parentticketid
            new TranslationSeedItem("entity.ticket.parentticketid", "zh-CN", "父工单ID", "父工单ID（为空表示顶级工单；非空表示该工单为子工单，序列化为string以避免Javascript精度问题）"),
            // entity.ticket.parentticketid
            new TranslationSeedItem("entity.ticket.parentticketid", "zh-HK", "父工单ID", "父工单ID（为空表示顶级工单；非空表示该工单为子工单，序列化为string以避免Javascript精度问题）"),

            // entity.ticket.firstresponseat
            new TranslationSeedItem("entity.ticket.firstresponseat", "en-US", "首次响应时间", "首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）"),
            // entity.ticket.firstresponseat
            new TranslationSeedItem("entity.ticket.firstresponseat", "ja-JP", "首次响应时间", "首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）"),
            // entity.ticket.firstresponseat
            new TranslationSeedItem("entity.ticket.firstresponseat", "zh-CN", "首次响应时间", "首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）"),
            // entity.ticket.firstresponseat
            new TranslationSeedItem("entity.ticket.firstresponseat", "zh-HK", "首次响应时间", "首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）"),

            // entity.ticket.firstresponsedueby
            new TranslationSeedItem("entity.ticket.firstresponsedueby", "en-US", "首次响应期限", "首次响应期限（根据 SLA 计算出的首次响应截止时间）"),
            // entity.ticket.firstresponsedueby
            new TranslationSeedItem("entity.ticket.firstresponsedueby", "ja-JP", "首次响应期限", "首次响应期限（根据 SLA 计算出的首次响应截止时间）"),
            // entity.ticket.firstresponsedueby
            new TranslationSeedItem("entity.ticket.firstresponsedueby", "zh-CN", "首次响应期限", "首次响应期限（根据 SLA 计算出的首次响应截止时间）"),
            // entity.ticket.firstresponsedueby
            new TranslationSeedItem("entity.ticket.firstresponsedueby", "zh-HK", "首次响应期限", "首次响应期限（根据 SLA 计算出的首次响应截止时间）"),

            // entity.ticket.resolvedat
            new TranslationSeedItem("entity.ticket.resolvedat", "en-US", "解决时间", "解决时间（问题被标记为已解决的时间）"),
            // entity.ticket.resolvedat
            new TranslationSeedItem("entity.ticket.resolvedat", "ja-JP", "解决时间", "解决时间（问题被标记为已解决的时间）"),
            // entity.ticket.resolvedat
            new TranslationSeedItem("entity.ticket.resolvedat", "zh-CN", "解决时间", "解决时间（问题被标记为已解决的时间）"),
            // entity.ticket.resolvedat
            new TranslationSeedItem("entity.ticket.resolvedat", "zh-HK", "解决时间", "解决时间（问题被标记为已解决的时间）"),

            // entity.ticket.resolutiondueby
            new TranslationSeedItem("entity.ticket.resolutiondueby", "en-US", "解决期限", "解决期限（根据 SLA 计算出的解决截止时间）"),
            // entity.ticket.resolutiondueby
            new TranslationSeedItem("entity.ticket.resolutiondueby", "ja-JP", "解决期限", "解决期限（根据 SLA 计算出的解决截止时间）"),
            // entity.ticket.resolutiondueby
            new TranslationSeedItem("entity.ticket.resolutiondueby", "zh-CN", "解决期限", "解决期限（根据 SLA 计算出的解决截止时间）"),
            // entity.ticket.resolutiondueby
            new TranslationSeedItem("entity.ticket.resolutiondueby", "zh-HK", "解决期限", "解决期限（根据 SLA 计算出的解决截止时间）"),

            // entity.ticket.closedat
            new TranslationSeedItem("entity.ticket.closedat", "en-US", "关闭时间", "关闭时间（工单最终关闭的时间）"),
            // entity.ticket.closedat
            new TranslationSeedItem("entity.ticket.closedat", "ja-JP", "关闭时间", "关闭时间（工单最终关闭的时间）"),
            // entity.ticket.closedat
            new TranslationSeedItem("entity.ticket.closedat", "zh-CN", "关闭时间", "关闭时间（工单最终关闭的时间）"),
            // entity.ticket.closedat
            new TranslationSeedItem("entity.ticket.closedat", "zh-HK", "关闭时间", "关闭时间（工单最终关闭的时间）"),

            // entity.ticket.flowinstanceid
            new TranslationSeedItem("entity.ticket.flowinstanceid", "en-US", "流程实例ID", "流程实例ID（关联工作流；流程侧 BusinessType=Ticket、BusinessKey=本表 Id）"),
            // entity.ticket.flowinstanceid
            new TranslationSeedItem("entity.ticket.flowinstanceid", "ja-JP", "流程实例ID", "流程实例ID（关联工作流；流程侧 BusinessType=Ticket、BusinessKey=本表 Id）"),
            // entity.ticket.flowinstanceid
            new TranslationSeedItem("entity.ticket.flowinstanceid", "zh-CN", "流程实例ID", "流程实例ID（关联工作流；流程侧 BusinessType=Ticket、BusinessKey=本表 Id）"),
            // entity.ticket.flowinstanceid
            new TranslationSeedItem("entity.ticket.flowinstanceid", "zh-HK", "流程实例ID", "流程实例ID（关联工作流；流程侧 BusinessType=Ticket、BusinessKey=本表 Id）"),

            // entity.ticket.applicantdeptid
            new TranslationSeedItem("entity.ticket.applicantdeptid", "en-US", "申请部门ID", "申请部门ID"),
            // entity.ticket.applicantdeptid
            new TranslationSeedItem("entity.ticket.applicantdeptid", "ja-JP", "申请部门ID", "申请部门ID"),
            // entity.ticket.applicantdeptid
            new TranslationSeedItem("entity.ticket.applicantdeptid", "zh-CN", "申请部门ID", "申请部门ID"),
            // entity.ticket.applicantdeptid
            new TranslationSeedItem("entity.ticket.applicantdeptid", "zh-HK", "申请部门ID", "申请部门ID"),

            // entity.ticket.applicantdeptname
            new TranslationSeedItem("entity.ticket.applicantdeptname", "en-US", "申请部门名称", "申请部门名称"),
            // entity.ticket.applicantdeptname
            new TranslationSeedItem("entity.ticket.applicantdeptname", "ja-JP", "申请部门名称", "申请部门名称"),
            // entity.ticket.applicantdeptname
            new TranslationSeedItem("entity.ticket.applicantdeptname", "zh-CN", "申请部门名称", "申请部门名称"),
            // entity.ticket.applicantdeptname
            new TranslationSeedItem("entity.ticket.applicantdeptname", "zh-HK", "申请部门名称", "申请部门名称"),

            // entity.ticket.applicantby
            new TranslationSeedItem("entity.ticket.applicantby", "en-US", "申请人", "申请人（实际申请人；代理人代提时填被代理人）"),
            // entity.ticket.applicantby
            new TranslationSeedItem("entity.ticket.applicantby", "ja-JP", "申请人", "申请人（实际申请人；代理人代提时填被代理人）"),
            // entity.ticket.applicantby
            new TranslationSeedItem("entity.ticket.applicantby", "zh-CN", "申请人", "申请人（实际申请人；代理人代提时填被代理人）"),
            // entity.ticket.applicantby
            new TranslationSeedItem("entity.ticket.applicantby", "zh-HK", "申请人", "申请人（实际申请人；代理人代提时填被代理人）"),

            // entity.ticket.childtickets
            new TranslationSeedItem("entity.ticket.childtickets", "en-US", "childTickets", "子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）"),
            // entity.ticket.childtickets
            new TranslationSeedItem("entity.ticket.childtickets", "ja-JP", "childTickets", "子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）"),
            // entity.ticket.childtickets
            new TranslationSeedItem("entity.ticket.childtickets", "zh-CN", "childTickets", "子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）"),
            // entity.ticket.childtickets
            new TranslationSeedItem("entity.ticket.childtickets", "zh-HK", "childTickets", "子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）"),
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
