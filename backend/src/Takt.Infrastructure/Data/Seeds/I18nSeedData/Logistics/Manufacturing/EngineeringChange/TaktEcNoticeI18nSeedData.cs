// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcNoticeI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEcNotice 实体国际化翻译种子（键前缀 entity.ecnotice.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecnotice 实体翻译...", tenantCode);

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
    /// I18nKey：entity.ecnotice._self / entity.ecnotice.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetEcNoticeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecnotice._self
            new TranslationSeedItem("entity.ecnotice._self", "en-US", "Ec Notice Information", "实体名称"),
            // entity.ecnotice._self
            new TranslationSeedItem("entity.ecnotice._self", "ja-JP", "工程变更通知单信息", "实体名称"),
            // entity.ecnotice._self
            new TranslationSeedItem("entity.ecnotice._self", "zh-CN", "工程变更通知单信息", "实体名称"),
            // entity.ecnotice._self
            new TranslationSeedItem("entity.ecnotice._self", "zh-HK", "工程变更通知单信息", "实体名称"),

            // entity.ecnotice.plantcode
            new TranslationSeedItem("entity.ecnotice.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.ecnotice.plantcode
            new TranslationSeedItem("entity.ecnotice.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.ecnotice.plantcode
            new TranslationSeedItem("entity.ecnotice.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.ecnotice.plantcode
            new TranslationSeedItem("entity.ecnotice.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.ecnotice.no
            new TranslationSeedItem("entity.ecnotice.no", "en-US", "通知单号", "通知单号（唯一，如：EC-2026-0001）"),
            // entity.ecnotice.no
            new TranslationSeedItem("entity.ecnotice.no", "ja-JP", "通知单号", "通知单号（唯一，如：EC-2026-0001）"),
            // entity.ecnotice.no
            new TranslationSeedItem("entity.ecnotice.no", "zh-CN", "通知单号", "通知单号（唯一，如：EC-2026-0001）"),
            // entity.ecnotice.no
            new TranslationSeedItem("entity.ecnotice.no", "zh-HK", "通知单号", "通知单号（唯一，如：EC-2026-0001）"),

            // entity.ecnotice.ecid
            new TranslationSeedItem("entity.ecnotice.ecid", "en-US", "设变ID", "关联的设变主表ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecnotice.ecid
            new TranslationSeedItem("entity.ecnotice.ecid", "ja-JP", "设变ID", "关联的设变主表ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecnotice.ecid
            new TranslationSeedItem("entity.ecnotice.ecid", "zh-CN", "设变ID", "关联的设变主表ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecnotice.ecid
            new TranslationSeedItem("entity.ecnotice.ecid", "zh-HK", "设变ID", "关联的设变主表ID（序列化为string以避免Javascript精度问题）"),

            // entity.ecnotice.ecno
            new TranslationSeedItem("entity.ecnotice.ecno", "en-US", "设变单号", "设变单号（冗余字段，便于查询）"),
            // entity.ecnotice.ecno
            new TranslationSeedItem("entity.ecnotice.ecno", "ja-JP", "设变单号", "设变单号（冗余字段，便于查询）"),
            // entity.ecnotice.ecno
            new TranslationSeedItem("entity.ecnotice.ecno", "zh-CN", "设变单号", "设变单号（冗余字段，便于查询）"),
            // entity.ecnotice.ecno
            new TranslationSeedItem("entity.ecnotice.ecno", "zh-HK", "设变单号", "设变单号（冗余字段，便于查询）"),

            // entity.ecnotice.ectitle
            new TranslationSeedItem("entity.ecnotice.ectitle", "en-US", "设变主题", "设变主题（冗余字段）"),
            // entity.ecnotice.ectitle
            new TranslationSeedItem("entity.ecnotice.ectitle", "ja-JP", "设变主题", "设变主题（冗余字段）"),
            // entity.ecnotice.ectitle
            new TranslationSeedItem("entity.ecnotice.ectitle", "zh-CN", "设变主题", "设变主题（冗余字段）"),
            // entity.ecnotice.ectitle
            new TranslationSeedItem("entity.ecnotice.ectitle", "zh-HK", "设变主题", "设变主题（冗余字段）"),

            // entity.ecnotice.date
            new TranslationSeedItem("entity.ecnotice.date", "en-US", "通知日期", "通知日期"),
            // entity.ecnotice.date
            new TranslationSeedItem("entity.ecnotice.date", "ja-JP", "通知日期", "通知日期"),
            // entity.ecnotice.date
            new TranslationSeedItem("entity.ecnotice.date", "zh-CN", "通知日期", "通知日期"),
            // entity.ecnotice.date
            new TranslationSeedItem("entity.ecnotice.date", "zh-HK", "通知日期", "通知日期"),

            // entity.ecnotice.deptcodes
            new TranslationSeedItem("entity.ecnotice.deptcodes", "en-US", "通知部门编码", "通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）"),
            // entity.ecnotice.deptcodes
            new TranslationSeedItem("entity.ecnotice.deptcodes", "ja-JP", "通知部门编码", "通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）"),
            // entity.ecnotice.deptcodes
            new TranslationSeedItem("entity.ecnotice.deptcodes", "zh-CN", "通知部门编码", "通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）"),
            // entity.ecnotice.deptcodes
            new TranslationSeedItem("entity.ecnotice.deptcodes", "zh-HK", "通知部门编码", "通知部门编码（多个部门用逗号分隔，如：Assy,PCBA,QC）"),

            // entity.ecnotice.deptnames
            new TranslationSeedItem("entity.ecnotice.deptnames", "en-US", "通知部门名称", "通知部门名称（多个部门用逗号分隔）"),
            // entity.ecnotice.deptnames
            new TranslationSeedItem("entity.ecnotice.deptnames", "ja-JP", "通知部门名称", "通知部门名称（多个部门用逗号分隔）"),
            // entity.ecnotice.deptnames
            new TranslationSeedItem("entity.ecnotice.deptnames", "zh-CN", "通知部门名称", "通知部门名称（多个部门用逗号分隔）"),
            // entity.ecnotice.deptnames
            new TranslationSeedItem("entity.ecnotice.deptnames", "zh-HK", "通知部门名称", "通知部门名称（多个部门用逗号分隔）"),

            // entity.ecnotice.notifierid
            new TranslationSeedItem("entity.ecnotice.notifierid", "en-US", "通知人ID", "通知人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecnotice.notifierid
            new TranslationSeedItem("entity.ecnotice.notifierid", "ja-JP", "通知人ID", "通知人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecnotice.notifierid
            new TranslationSeedItem("entity.ecnotice.notifierid", "zh-CN", "通知人ID", "通知人ID（序列化为string以避免Javascript精度问题）"),
            // entity.ecnotice.notifierid
            new TranslationSeedItem("entity.ecnotice.notifierid", "zh-HK", "通知人ID", "通知人ID（序列化为string以避免Javascript精度问题）"),

            // entity.ecnotice.notifiername
            new TranslationSeedItem("entity.ecnotice.notifiername", "en-US", "通知人姓名", "通知人姓名"),
            // entity.ecnotice.notifiername
            new TranslationSeedItem("entity.ecnotice.notifiername", "ja-JP", "通知人姓名", "通知人姓名"),
            // entity.ecnotice.notifiername
            new TranslationSeedItem("entity.ecnotice.notifiername", "zh-CN", "通知人姓名", "通知人姓名"),
            // entity.ecnotice.notifiername
            new TranslationSeedItem("entity.ecnotice.notifiername", "zh-HK", "通知人姓名", "通知人姓名"),

            // entity.ecnotice.method
            new TranslationSeedItem("entity.ecnotice.method", "en-US", "通知方式", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),
            // entity.ecnotice.method
            new TranslationSeedItem("entity.ecnotice.method", "ja-JP", "通知方式", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),
            // entity.ecnotice.method
            new TranslationSeedItem("entity.ecnotice.method", "zh-CN", "通知方式", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),
            // entity.ecnotice.method
            new TranslationSeedItem("entity.ecnotice.method", "zh-HK", "通知方式", "通知方式（1=系统通知 2=邮件 3=纸质 4=会议）"),

            // entity.ecnotice.status
            new TranslationSeedItem("entity.ecnotice.status", "en-US", "通知状态", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),
            // entity.ecnotice.status
            new TranslationSeedItem("entity.ecnotice.status", "ja-JP", "通知状态", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),
            // entity.ecnotice.status
            new TranslationSeedItem("entity.ecnotice.status", "zh-CN", "通知状态", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),
            // entity.ecnotice.status
            new TranslationSeedItem("entity.ecnotice.status", "zh-HK", "通知状态", "通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）"),

            // entity.ecnotice.flowinstanceid
            new TranslationSeedItem("entity.ecnotice.flowinstanceid", "en-US", "流程实例ID", "流程实例 ID（TaktFlowInstance；发起审批后由业务写入）"),
            // entity.ecnotice.flowinstanceid
            new TranslationSeedItem("entity.ecnotice.flowinstanceid", "ja-JP", "流程实例ID", "流程实例 ID（TaktFlowInstance；发起审批后由业务写入）"),
            // entity.ecnotice.flowinstanceid
            new TranslationSeedItem("entity.ecnotice.flowinstanceid", "zh-CN", "流程实例ID", "流程实例 ID（TaktFlowInstance；发起审批后由业务写入）"),
            // entity.ecnotice.flowinstanceid
            new TranslationSeedItem("entity.ecnotice.flowinstanceid", "zh-HK", "流程实例ID", "流程实例 ID（TaktFlowInstance；发起审批后由业务写入）"),

            // entity.ecnotice.ec
            new TranslationSeedItem("entity.ecnotice.ec", "en-US", "关联的设变主表", "关联的设变主表"),
            // entity.ecnotice.ec
            new TranslationSeedItem("entity.ecnotice.ec", "ja-JP", "关联的设变主表", "关联的设变主表"),
            // entity.ecnotice.ec
            new TranslationSeedItem("entity.ecnotice.ec", "zh-CN", "关联的设变主表", "关联的设变主表"),
            // entity.ecnotice.ec
            new TranslationSeedItem("entity.ecnotice.ec", "zh-HK", "关联的设变主表", "关联的设变主表"),
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
        translation.ResourceGroup = 4;
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
