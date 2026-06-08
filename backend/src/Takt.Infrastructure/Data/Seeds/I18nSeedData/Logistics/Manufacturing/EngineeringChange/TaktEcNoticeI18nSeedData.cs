// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNoticeI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcNotice 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcNotice 实体国际化翻译种子（键前缀 entity.ecNotice.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcNoticeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcNotice 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecNotice 实体翻译...", tenantCode);

        foreach (var item in GetEcNoticeTranslations())
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

        TaktLogger.Information("TaktEcNotice 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcNotice 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecNotice._self / entity.ecNotice.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcNoticeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecNotice._self
            new TranslationSeedItem("entity.ecNotice._self", "en-US", "Ec Notice Information", "实体名称"),
            // entity.ecNotice._self
            new TranslationSeedItem("entity.ecNotice._self", "ja-JP", "工程变更通知单信息", "实体名称"),
            // entity.ecNotice._self
            new TranslationSeedItem("entity.ecNotice._self", "zh-CN", "工程变更通知单信息", "实体名称"),
            // entity.ecNotice._self
            new TranslationSeedItem("entity.ecNotice._self", "zh-HK", "工程变更通知单信息", "实体名称"),

            // entity.ecNotice.plantcode
            new TranslationSeedItem("entity.ecNotice.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.ecNotice.plantcode
            new TranslationSeedItem("entity.ecNotice.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.ecNotice.plantcode
            new TranslationSeedItem("entity.ecNotice.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.ecNotice.plantcode
            new TranslationSeedItem("entity.ecNotice.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.ecNotice.no
            new TranslationSeedItem("entity.ecNotice.no", "en-US", "通知单号", "通知单号（唯一，如：EC-2026-0001）"),
            // entity.ecNotice.no
            new TranslationSeedItem("entity.ecNotice.no", "ja-JP", "通知单号", "通知单号（唯一，如：EC-2026-0001）"),
            // entity.ecNotice.no
            new TranslationSeedItem("entity.ecNotice.no", "zh-CN", "通知单号", "通知单号（唯一，如：EC-2026-0001）"),
            // entity.ecNotice.no
            new TranslationSeedItem("entity.ecNotice.no", "zh-HK", "通知单号", "通知单号（唯一，如：EC-2026-0001）"),

            // entity.ecNotice.ecid
            new TranslationSeedItem("entity.ecNotice.ecid", "en-US", "设变ID", "关联的设变主表ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecNotice.ecid
            new TranslationSeedItem("entity.ecNotice.ecid", "ja-JP", "设变ID", "关联的设变主表ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecNotice.ecid
            new TranslationSeedItem("entity.ecNotice.ecid", "zh-CN", "设变ID", "关联的设变主表ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecNotice.ecid
            new TranslationSeedItem("entity.ecNotice.ecid", "zh-HK", "设变ID", "关联的设变主表ID（序列化为string以避免Javascript精度问题）"),

            // entity.ecNotice.ecno
            new TranslationSeedItem("entity.ecNotice.ecno", "en-US", "设变单号", "设变单号（冗余字段，便于查询）"),
            // entity.ecNotice.ecno
            new TranslationSeedItem("entity.ecNotice.ecno", "ja-JP", "设变单号", "设变单号（冗余字段，便于查询）"),
            // entity.ecNotice.ecno
            new TranslationSeedItem("entity.ecNotice.ecno", "zh-CN", "设变单号", "设变单号（冗余字段，便于查询）"),
            // entity.ecNotice.ecno
            new TranslationSeedItem("entity.ecNotice.ecno", "zh-HK", "设变单号", "设变单号（冗余字段，便于查询）"),

            // entity.ecNotice.ectitle
            new TranslationSeedItem("entity.ecNotice.ectitle", "en-US", "设变主题", "设变主题（冗余字段）"),
            // entity.ecNotice.ectitle
            new TranslationSeedItem("entity.ecNotice.ectitle", "ja-JP", "设变主题", "设变主题（冗余字段）"),
            // entity.ecNotice.ectitle
            new TranslationSeedItem("entity.ecNotice.ectitle", "zh-CN", "设变主题", "设变主题（冗余字段）"),
            // entity.ecNotice.ectitle
            new TranslationSeedItem("entity.ecNotice.ectitle", "zh-HK", "设变主题", "设变主题（冗余字段）"),

            // entity.ecNotice.date
            new TranslationSeedItem("entity.ecNotice.date", "en-US", "通知日期", "通知日期"),
            // entity.ecNotice.date
            new TranslationSeedItem("entity.ecNotice.date", "ja-JP", "通知日期", "通知日期"),
            // entity.ecNotice.date
            new TranslationSeedItem("entity.ecNotice.date", "zh-CN", "通知日期", "通知日期"),
            // entity.ecNotice.date
            new TranslationSeedItem("entity.ecNotice.date", "zh-HK", "通知日期", "通知日期"),

            // entity.ecNotice.deptcodes
            new TranslationSeedItem("entity.ecNotice.deptcodes", "en-US", "通知部门编码", "通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）"),
            // entity.ecNotice.deptcodes
            new TranslationSeedItem("entity.ecNotice.deptcodes", "ja-JP", "通知部门编码", "通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）"),
            // entity.ecNotice.deptcodes
            new TranslationSeedItem("entity.ecNotice.deptcodes", "zh-CN", "通知部门编码", "通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）"),
            // entity.ecNotice.deptcodes
            new TranslationSeedItem("entity.ecNotice.deptcodes", "zh-HK", "通知部门编码", "通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）"),

            // entity.ecNotice.deptnames
            new TranslationSeedItem("entity.ecNotice.deptnames", "en-US", "通知部门名称", "通知部门名称（多个部门用逗号分隔）"),
            // entity.ecNotice.deptnames
            new TranslationSeedItem("entity.ecNotice.deptnames", "ja-JP", "通知部门名称", "通知部门名称（多个部门用逗号分隔）"),
            // entity.ecNotice.deptnames
            new TranslationSeedItem("entity.ecNotice.deptnames", "zh-CN", "通知部门名称", "通知部门名称（多个部门用逗号分隔）"),
            // entity.ecNotice.deptnames
            new TranslationSeedItem("entity.ecNotice.deptnames", "zh-HK", "通知部门名称", "通知部门名称（多个部门用逗号分隔）"),

            // entity.ecNotice.notifierid
            new TranslationSeedItem("entity.ecNotice.notifierid", "en-US", "通知人ID", "通知人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecNotice.notifierid
            new TranslationSeedItem("entity.ecNotice.notifierid", "ja-JP", "通知人ID", "通知人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecNotice.notifierid
            new TranslationSeedItem("entity.ecNotice.notifierid", "zh-CN", "通知人ID", "通知人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecNotice.notifierid
            new TranslationSeedItem("entity.ecNotice.notifierid", "zh-HK", "通知人ID", "通知人ID（序列化为string以避免Javascript精度问题）"),

            // entity.ecNotice.notifiername
            new TranslationSeedItem("entity.ecNotice.notifiername", "en-US", "通知人姓名", "通知人姓名"),
            // entity.ecNotice.notifiername
            new TranslationSeedItem("entity.ecNotice.notifiername", "ja-JP", "通知人姓名", "通知人姓名"),
            // entity.ecNotice.notifiername
            new TranslationSeedItem("entity.ecNotice.notifiername", "zh-CN", "通知人姓名", "通知人姓名"),
            // entity.ecNotice.notifiername
            new TranslationSeedItem("entity.ecNotice.notifiername", "zh-HK", "通知人姓名", "通知人姓名"),

            // entity.ecNotice.method
            new TranslationSeedItem("entity.ecNotice.method", "en-US", "通知方式", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),
            // entity.ecNotice.method
            new TranslationSeedItem("entity.ecNotice.method", "ja-JP", "通知方式", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),
            // entity.ecNotice.method
            new TranslationSeedItem("entity.ecNotice.method", "zh-CN", "通知方式", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),
            // entity.ecNotice.method
            new TranslationSeedItem("entity.ecNotice.method", "zh-HK", "通知方式", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),

            // entity.ecNotice.status
            new TranslationSeedItem("entity.ecNotice.status", "en-US", "通知状态", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),
            // entity.ecNotice.status
            new TranslationSeedItem("entity.ecNotice.status", "ja-JP", "通知状态", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),
            // entity.ecNotice.status
            new TranslationSeedItem("entity.ecNotice.status", "zh-CN", "通知状态", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),
            // entity.ecNotice.status
            new TranslationSeedItem("entity.ecNotice.status", "zh-HK", "通知状态", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),

            // entity.ecNotice.flowinstanceid
            new TranslationSeedItem("entity.ecNotice.flowinstanceid", "en-US", "流程实例ID", "流程实例 ID（<see cref=\"Workflow.TaktFlowInstance\"/>；发起审批后由业务写入）"),
            // entity.ecNotice.flowinstanceid
            new TranslationSeedItem("entity.ecNotice.flowinstanceid", "ja-JP", "流程实例ID", "流程实例 ID（<see cref=\"Workflow.TaktFlowInstance\"/>；发起审批后由业务写入）"),
            // entity.ecNotice.flowinstanceid
            new TranslationSeedItem("entity.ecNotice.flowinstanceid", "zh-CN", "流程实例ID", "流程实例 ID（<see cref=\"Workflow.TaktFlowInstance\"/>；发起审批后由业务写入）"),
            // entity.ecNotice.flowinstanceid
            new TranslationSeedItem("entity.ecNotice.flowinstanceid", "zh-HK", "流程实例ID", "流程实例 ID（<see cref=\"Workflow.TaktFlowInstance\"/>；发起审批后由业务写入）"),

            // entity.ecNotice.ec
            new TranslationSeedItem("entity.ecNotice.ec", "en-US", "关联的设变主表", "关联的设变主表"),
            // entity.ecNotice.ec
            new TranslationSeedItem("entity.ecNotice.ec", "ja-JP", "关联的设变主表", "关联的设变主表"),
            // entity.ecNotice.ec
            new TranslationSeedItem("entity.ecNotice.ec", "zh-CN", "关联的设变主表", "关联的设变主表"),
            // entity.ecNotice.ec
            new TranslationSeedItem("entity.ecNotice.ec", "zh-HK", "关联的设变主表", "关联的设变主表"),
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
