// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData
// 文件名称：TaktMenuI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单导航国际化翻译种子（menu.* 键，与 TaktMenuLevel1~4SeedData I18nKey 对齐）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险.
// ========================================

using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData;

/// <summary>
/// 菜单导航国际化翻译种子（键前缀 menu.*，与菜单种子 I18nKey 一致）
/// 幂等性：存在则更新，不存在则创建
/// TranslationText 为菜单显示名 MenuName；由 generate-menu-i18n-seed.cjs 从 Level1~4 种子生成
/// 与 I18nSeedData.Identity.TaktMenuI18nSeedData（entity.menu.* 实体字段）职责不同
/// </summary>
public class TaktMenuI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>执行顺序（在问候语翻译之前，菜单种子落库之后）</summary>
    public int Order => 47;

    /// <summary>初始化菜单导航国际化翻译种子</summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>插入数与更新数</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化菜单导航国际化翻译种子...");

        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过菜单导航国际化翻译种子");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 menu.* 翻译...", tenantCode);

        foreach (var row in GetMenuTranslations())
        {
            if (!cultureIdByCode.TryGetValue(row.CultureCode, out var cultureId))
            {
                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", row.CultureCode, row.I18nKey);
                continue;
            }

            var item = new TranslationSeedItem(row.I18nKey, row.CultureCode, row.TranslationText, row.ContextNote);
            var (_, i, u) = await CreateOrUpdateTranslationAsync(repository, tenantCode, cultureId, item);
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("菜单导航国际化翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>菜单导航翻译列表（en-US / ja-JP / zh-CN / zh-HK）</summary>
    private static List<(string I18nKey, string CultureCode, string TranslationText, string? ContextNote)> GetMenuTranslations()
    {
        return new List<(string, string, string, string?)>
        {
            // menu.home._self
            ("menu.home._self", "en-US", "主页_us", "菜单导航"),
            // menu.home._self
            ("menu.home._self", "ja-JP", "主页_jp", "菜单导航"),
            // menu.home._self
            ("menu.home._self", "zh-CN", "主页", "菜单导航"),
            // menu.home._self
            ("menu.home._self", "zh-HK", "主页_hk", "菜单导航"),

            // menu.dashboard._self
            ("menu.dashboard._self", "en-US", "仪表盘_us", "菜单导航"),
            // menu.dashboard._self
            ("menu.dashboard._self", "ja-JP", "仪表盘_jp", "菜单导航"),
            // menu.dashboard._self
            ("menu.dashboard._self", "zh-CN", "仪表盘", "菜单导航"),
            // menu.dashboard._self
            ("menu.dashboard._self", "zh-HK", "仪表盘_hk", "菜单导航"),

            // menu.routine._self
            ("menu.routine._self", "en-US", "日常事务_us", "菜单导航"),
            // menu.routine._self
            ("menu.routine._self", "ja-JP", "日常事务_jp", "菜单导航"),
            // menu.routine._self
            ("menu.routine._self", "zh-CN", "日常事务", "菜单导航"),
            // menu.routine._self
            ("menu.routine._self", "zh-HK", "日常事务_hk", "菜单导航"),

            // menu.accounting._self
            ("menu.accounting._self", "en-US", "财务核算_us", "菜单导航"),
            // menu.accounting._self
            ("menu.accounting._self", "ja-JP", "财务核算_jp", "菜单导航"),
            // menu.accounting._self
            ("menu.accounting._self", "zh-CN", "财务核算", "菜单导航"),
            // menu.accounting._self
            ("menu.accounting._self", "zh-HK", "财务核算_hk", "菜单导航"),

            // menu.logistics._self
            ("menu.logistics._self", "en-US", "后勤管理_us", "菜单导航"),
            // menu.logistics._self
            ("menu.logistics._self", "ja-JP", "后勤管理_jp", "菜单导航"),
            // menu.logistics._self
            ("menu.logistics._self", "zh-CN", "后勤管理", "菜单导航"),
            // menu.logistics._self
            ("menu.logistics._self", "zh-HK", "后勤管理_hk", "菜单导航"),

            // menu.humanresource._self
            ("menu.humanresource._self", "en-US", "人力资源_us", "菜单导航"),
            // menu.humanresource._self
            ("menu.humanresource._self", "ja-JP", "人力资源_jp", "菜单导航"),
            // menu.humanresource._self
            ("menu.humanresource._self", "zh-CN", "人力资源", "菜单导航"),
            // menu.humanresource._self
            ("menu.humanresource._self", "zh-HK", "人力资源_hk", "菜单导航"),

            // menu.identity._self
            ("menu.identity._self", "en-US", "身份认证_us", "菜单导航"),
            // menu.identity._self
            ("menu.identity._self", "ja-JP", "身份认证_jp", "菜单导航"),
            // menu.identity._self
            ("menu.identity._self", "zh-CN", "身份认证", "菜单导航"),
            // menu.identity._self
            ("menu.identity._self", "zh-HK", "身份认证_hk", "菜单导航"),

            // menu.workflow._self
            ("menu.workflow._self", "en-US", "工作流_us", "菜单导航"),
            // menu.workflow._self
            ("menu.workflow._self", "ja-JP", "工作流_jp", "菜单导航"),
            // menu.workflow._self
            ("menu.workflow._self", "zh-CN", "工作流", "菜单导航"),
            // menu.workflow._self
            ("menu.workflow._self", "zh-HK", "工作流_hk", "菜单导航"),

            // menu.code._self
            ("menu.code._self", "en-US", "代码管理_us", "菜单导航"),
            // menu.code._self
            ("menu.code._self", "ja-JP", "代码管理_jp", "菜单导航"),
            // menu.code._self
            ("menu.code._self", "zh-CN", "代码管理", "菜单导航"),
            // menu.code._self
            ("menu.code._self", "zh-HK", "代码管理_hk", "菜单导航"),

            // menu.foundation._self
            ("menu.foundation._self", "en-US", "基础设置_us", "菜单导航"),
            // menu.foundation._self
            ("menu.foundation._self", "ja-JP", "基础设置_jp", "菜单导航"),
            // menu.foundation._self
            ("menu.foundation._self", "zh-CN", "基础设置", "菜单导航"),
            // menu.foundation._self
            ("menu.foundation._self", "zh-HK", "基础设置_hk", "菜单导航"),

            // menu.statistics._self
            ("menu.statistics._self", "en-US", "统计看板_us", "菜单导航"),
            // menu.statistics._self
            ("menu.statistics._self", "ja-JP", "统计看板_jp", "菜单导航"),
            // menu.statistics._self
            ("menu.statistics._self", "zh-CN", "统计看板", "菜单导航"),
            // menu.statistics._self
            ("menu.statistics._self", "zh-HK", "统计看板_hk", "菜单导航"),

            // menu.about._self
            ("menu.about._self", "en-US", "关于_us", "菜单导航"),
            // menu.about._self
            ("menu.about._self", "ja-JP", "关于_jp", "菜单导航"),
            // menu.about._self
            ("menu.about._self", "zh-CN", "关于", "菜单导航"),
            // menu.about._self
            ("menu.about._self", "zh-HK", "关于_hk", "菜单导航"),

            // menu.dashboard.workspace
            ("menu.dashboard.workspace", "en-US", "工作台_us", "菜单导航"),
            // menu.dashboard.workspace
            ("menu.dashboard.workspace", "ja-JP", "工作台_jp", "菜单导航"),
            // menu.dashboard.workspace
            ("menu.dashboard.workspace", "zh-CN", "工作台", "菜单导航"),
            // menu.dashboard.workspace
            ("menu.dashboard.workspace", "zh-HK", "工作台_hk", "菜单导航"),

            // menu.dashboard.databoard
            ("menu.dashboard.databoard", "en-US", "数据看板_us", "菜单导航"),
            // menu.dashboard.databoard
            ("menu.dashboard.databoard", "ja-JP", "数据看板_jp", "菜单导航"),
            // menu.dashboard.databoard
            ("menu.dashboard.databoard", "zh-CN", "数据看板", "菜单导航"),
            // menu.dashboard.databoard
            ("menu.dashboard.databoard", "zh-HK", "数据看板_hk", "菜单导航"),

            // menu.routine.announcement
            ("menu.routine.announcement", "en-US", "公告通知_us", "菜单导航"),
            // menu.routine.announcement
            ("menu.routine.announcement", "ja-JP", "公告通知_jp", "菜单导航"),
            // menu.routine.announcement
            ("menu.routine.announcement", "zh-CN", "公告通知", "菜单导航"),
            // menu.routine.announcement
            ("menu.routine.announcement", "zh-HK", "公告通知_hk", "菜单导航"),

            // menu.routine.conferencecenter
            ("menu.routine.conferencecenter", "en-US", "会议中心_us", "菜单导航"),
            // menu.routine.conferencecenter
            ("menu.routine.conferencecenter", "ja-JP", "会议中心_jp", "菜单导航"),
            // menu.routine.conferencecenter
            ("menu.routine.conferencecenter", "zh-CN", "会议中心", "菜单导航"),
            // menu.routine.conferencecenter
            ("menu.routine.conferencecenter", "zh-HK", "会议中心_hk", "菜单导航"),

            // menu.routine.documentcenter
            ("menu.routine.documentcenter", "en-US", "文管中心_us", "菜单导航"),
            // menu.routine.documentcenter
            ("menu.routine.documentcenter", "ja-JP", "文管中心_jp", "菜单导航"),
            // menu.routine.documentcenter
            ("menu.routine.documentcenter", "zh-CN", "文管中心", "菜单导航"),
            // menu.routine.documentcenter
            ("menu.routine.documentcenter", "zh-HK", "文管中心_hk", "菜单导航"),

            // menu.routine.newscenter
            ("menu.routine.newscenter", "en-US", "新闻中心_us", "菜单导航"),
            // menu.routine.newscenter
            ("menu.routine.newscenter", "ja-JP", "新闻中心_jp", "菜单导航"),
            // menu.routine.newscenter
            ("menu.routine.newscenter", "zh-CN", "新闻中心", "菜单导航"),
            // menu.routine.newscenter
            ("menu.routine.newscenter", "zh-HK", "新闻中心_hk", "菜单导航"),

            // menu.routine.helpdesk._self
            ("menu.routine.helpdesk._self", "en-US", "服务台_us", "菜单导航"),
            // menu.routine.helpdesk._self
            ("menu.routine.helpdesk._self", "ja-JP", "服务台_jp", "菜单导航"),
            // menu.routine.helpdesk._self
            ("menu.routine.helpdesk._self", "zh-CN", "服务台", "菜单导航"),
            // menu.routine.helpdesk._self
            ("menu.routine.helpdesk._self", "zh-HK", "服务台_hk", "菜单导航"),

            // menu.routine.visitorcenter
            ("menu.routine.visitorcenter", "en-US", "访客中心_us", "菜单导航"),
            // menu.routine.visitorcenter
            ("menu.routine.visitorcenter", "ja-JP", "访客中心_jp", "菜单导航"),
            // menu.routine.visitorcenter
            ("menu.routine.visitorcenter", "zh-CN", "访客中心", "菜单导航"),
            // menu.routine.visitorcenter
            ("menu.routine.visitorcenter", "zh-HK", "访客中心_hk", "菜单导航"),

            // menu.accounting.financial._self
            ("menu.accounting.financial._self", "en-US", "管理会计_us", "菜单导航"),
            // menu.accounting.financial._self
            ("menu.accounting.financial._self", "ja-JP", "管理会计_jp", "菜单导航"),
            // menu.accounting.financial._self
            ("menu.accounting.financial._self", "zh-CN", "管理会计", "菜单导航"),
            // menu.accounting.financial._self
            ("menu.accounting.financial._self", "zh-HK", "管理会计_hk", "菜单导航"),

            // menu.accounting.controlling._self
            ("menu.accounting.controlling._self", "en-US", "控制会计_us", "菜单导航"),
            // menu.accounting.controlling._self
            ("menu.accounting.controlling._self", "ja-JP", "控制会计_jp", "菜单导航"),
            // menu.accounting.controlling._self
            ("menu.accounting.controlling._self", "zh-CN", "控制会计", "菜单导航"),
            // menu.accounting.controlling._self
            ("menu.accounting.controlling._self", "zh-HK", "控制会计_hk", "菜单导航"),

            // menu.logistics.sales._self
            ("menu.logistics.sales._self", "en-US", "销售管理_us", "菜单导航"),
            // menu.logistics.sales._self
            ("menu.logistics.sales._self", "ja-JP", "销售管理_jp", "菜单导航"),
            // menu.logistics.sales._self
            ("menu.logistics.sales._self", "zh-CN", "销售管理", "菜单导航"),
            // menu.logistics.sales._self
            ("menu.logistics.sales._self", "zh-HK", "销售管理_hk", "菜单导航"),

            // menu.logistics.materials._self
            ("menu.logistics.materials._self", "en-US", "物料管理_us", "菜单导航"),
            // menu.logistics.materials._self
            ("menu.logistics.materials._self", "ja-JP", "物料管理_jp", "菜单导航"),
            // menu.logistics.materials._self
            ("menu.logistics.materials._self", "zh-CN", "物料管理", "菜单导航"),
            // menu.logistics.materials._self
            ("menu.logistics.materials._self", "zh-HK", "物料管理_hk", "菜单导航"),

            // menu.logistics.procurement._self
            ("menu.logistics.procurement._self", "en-US", "采购管理_us", "菜单导航"),
            // menu.logistics.procurement._self
            ("menu.logistics.procurement._self", "ja-JP", "采购管理_jp", "菜单导航"),
            // menu.logistics.procurement._self
            ("menu.logistics.procurement._self", "zh-CN", "采购管理", "菜单导航"),
            // menu.logistics.procurement._self
            ("menu.logistics.procurement._self", "zh-HK", "采购管理_hk", "菜单导航"),

            // menu.logistics.manufacturing._self
            ("menu.logistics.manufacturing._self", "en-US", "生产执行_us", "菜单导航"),
            // menu.logistics.manufacturing._self
            ("menu.logistics.manufacturing._self", "ja-JP", "生产执行_jp", "菜单导航"),
            // menu.logistics.manufacturing._self
            ("menu.logistics.manufacturing._self", "zh-CN", "生产执行", "菜单导航"),
            // menu.logistics.manufacturing._self
            ("menu.logistics.manufacturing._self", "zh-HK", "生产执行_hk", "菜单导航"),

            // menu.logistics.quality._self
            ("menu.logistics.quality._self", "en-US", "质量管理_us", "菜单导航"),
            // menu.logistics.quality._self
            ("menu.logistics.quality._self", "ja-JP", "质量管理_jp", "菜单导航"),
            // menu.logistics.quality._self
            ("menu.logistics.quality._self", "zh-CN", "质量管理", "菜单导航"),
            // menu.logistics.quality._self
            ("menu.logistics.quality._self", "zh-HK", "质量管理_hk", "菜单导航"),

            // menu.logistics.service._self
            ("menu.logistics.service._self", "en-US", "客户服务_us", "菜单导航"),
            // menu.logistics.service._self
            ("menu.logistics.service._self", "ja-JP", "客户服务_jp", "菜单导航"),
            // menu.logistics.service._self
            ("menu.logistics.service._self", "zh-CN", "客户服务", "菜单导航"),
            // menu.logistics.service._self
            ("menu.logistics.service._self", "zh-HK", "客户服务_hk", "菜单导航"),

            // menu.logistics.maintenance._self
            ("menu.logistics.maintenance._self", "en-US", "工厂维护_us", "菜单导航"),
            // menu.logistics.maintenance._self
            ("menu.logistics.maintenance._self", "ja-JP", "工厂维护_jp", "菜单导航"),
            // menu.logistics.maintenance._self
            ("menu.logistics.maintenance._self", "zh-CN", "工厂维护", "菜单导航"),
            // menu.logistics.maintenance._self
            ("menu.logistics.maintenance._self", "zh-HK", "工厂维护_hk", "菜单导航"),

            // menu.logistics.serial._self
            ("menu.logistics.serial._self", "en-US", "序列号管理_us", "菜单导航"),
            // menu.logistics.serial._self
            ("menu.logistics.serial._self", "ja-JP", "序列号管理_jp", "菜单导航"),
            // menu.logistics.serial._self
            ("menu.logistics.serial._self", "zh-CN", "序列号管理", "菜单导航"),
            // menu.logistics.serial._self
            ("menu.logistics.serial._self", "zh-HK", "序列号管理_hk", "菜单导航"),

            // menu.humanresource.organization._self
            ("menu.humanresource.organization._self", "en-US", "组织管理_us", "菜单导航"),
            // menu.humanresource.organization._self
            ("menu.humanresource.organization._self", "ja-JP", "组织管理_jp", "菜单导航"),
            // menu.humanresource.organization._self
            ("menu.humanresource.organization._self", "zh-CN", "组织管理", "菜单导航"),
            // menu.humanresource.organization._self
            ("menu.humanresource.organization._self", "zh-HK", "组织管理_hk", "菜单导航"),

            // menu.humanresource.personnel._self
            ("menu.humanresource.personnel._self", "en-US", "人事管理_us", "菜单导航"),
            // menu.humanresource.personnel._self
            ("menu.humanresource.personnel._self", "ja-JP", "人事管理_jp", "菜单导航"),
            // menu.humanresource.personnel._self
            ("menu.humanresource.personnel._self", "zh-CN", "人事管理", "菜单导航"),
            // menu.humanresource.personnel._self
            ("menu.humanresource.personnel._self", "zh-HK", "人事管理_hk", "菜单导航"),

            // menu.humanresource.attendance._self
            ("menu.humanresource.attendance._self", "en-US", "考勤管理_us", "菜单导航"),
            // menu.humanresource.attendance._self
            ("menu.humanresource.attendance._self", "ja-JP", "考勤管理_jp", "菜单导航"),
            // menu.humanresource.attendance._self
            ("menu.humanresource.attendance._self", "zh-CN", "考勤管理", "菜单导航"),
            // menu.humanresource.attendance._self
            ("menu.humanresource.attendance._self", "zh-HK", "考勤管理_hk", "菜单导航"),

            // menu.humanresource.compensation._self
            ("menu.humanresource.compensation._self", "en-US", "薪酬管理_us", "菜单导航"),
            // menu.humanresource.compensation._self
            ("menu.humanresource.compensation._self", "ja-JP", "薪酬管理_jp", "菜单导航"),
            // menu.humanresource.compensation._self
            ("menu.humanresource.compensation._self", "zh-CN", "薪酬管理", "菜单导航"),
            // menu.humanresource.compensation._self
            ("menu.humanresource.compensation._self", "zh-HK", "薪酬管理_hk", "菜单导航"),

            // menu.humanresource.benefits._self
            ("menu.humanresource.benefits._self", "en-US", "福利管理_us", "菜单导航"),
            // menu.humanresource.benefits._self
            ("menu.humanresource.benefits._self", "ja-JP", "福利管理_jp", "菜单导航"),
            // menu.humanresource.benefits._self
            ("menu.humanresource.benefits._self", "zh-CN", "福利管理", "菜单导航"),
            // menu.humanresource.benefits._self
            ("menu.humanresource.benefits._self", "zh-HK", "福利管理_hk", "菜单导航"),

            // menu.humanresource.performance._self
            ("menu.humanresource.performance._self", "en-US", "绩效管理_us", "菜单导航"),
            // menu.humanresource.performance._self
            ("menu.humanresource.performance._self", "ja-JP", "绩效管理_jp", "菜单导航"),
            // menu.humanresource.performance._self
            ("menu.humanresource.performance._self", "zh-CN", "绩效管理", "菜单导航"),
            // menu.humanresource.performance._self
            ("menu.humanresource.performance._self", "zh-HK", "绩效管理_hk", "菜单导航"),

            // menu.humanresource.training._self
            ("menu.humanresource.training._self", "en-US", "教育培训_us", "菜单导航"),
            // menu.humanresource.training._self
            ("menu.humanresource.training._self", "ja-JP", "教育培训_jp", "菜单导航"),
            // menu.humanresource.training._self
            ("menu.humanresource.training._self", "zh-CN", "教育培训", "菜单导航"),
            // menu.humanresource.training._self
            ("menu.humanresource.training._self", "zh-HK", "教育培训_hk", "菜单导航"),

            // menu.humanresource.talent._self
            ("menu.humanresource.talent._self", "en-US", "人才管理_us", "菜单导航"),
            // menu.humanresource.talent._self
            ("menu.humanresource.talent._self", "ja-JP", "人才管理_jp", "菜单导航"),
            // menu.humanresource.talent._self
            ("menu.humanresource.talent._self", "zh-CN", "人才管理", "菜单导航"),
            // menu.humanresource.talent._self
            ("menu.humanresource.talent._self", "zh-HK", "人才管理_hk", "菜单导航"),

            // menu.identity.tenant
            ("menu.identity.tenant", "en-US", "租户管理_us", "菜单导航"),
            // menu.identity.tenant
            ("menu.identity.tenant", "ja-JP", "租户管理_jp", "菜单导航"),
            // menu.identity.tenant
            ("menu.identity.tenant", "zh-CN", "租户管理", "菜单导航"),
            // menu.identity.tenant
            ("menu.identity.tenant", "zh-HK", "租户管理_hk", "菜单导航"),

            // menu.identity.user
            ("menu.identity.user", "en-US", "用户管理_us", "菜单导航"),
            // menu.identity.user
            ("menu.identity.user", "ja-JP", "用户管理_jp", "菜单导航"),
            // menu.identity.user
            ("menu.identity.user", "zh-CN", "用户管理", "菜单导航"),
            // menu.identity.user
            ("menu.identity.user", "zh-HK", "用户管理_hk", "菜单导航"),

            // menu.identity.menu
            ("menu.identity.menu", "en-US", "菜单管理_us", "菜单导航"),
            // menu.identity.menu
            ("menu.identity.menu", "ja-JP", "菜单管理_jp", "菜单导航"),
            // menu.identity.menu
            ("menu.identity.menu", "zh-CN", "菜单管理", "菜单导航"),
            // menu.identity.menu
            ("menu.identity.menu", "zh-HK", "菜单管理_hk", "菜单导航"),

            // menu.identity.role
            ("menu.identity.role", "en-US", "角色管理_us", "菜单导航"),
            // menu.identity.role
            ("menu.identity.role", "ja-JP", "角色管理_jp", "菜单导航"),
            // menu.identity.role
            ("menu.identity.role", "zh-CN", "角色管理", "菜单导航"),
            // menu.identity.role
            ("menu.identity.role", "zh-HK", "角色管理_hk", "菜单导航"),

            // menu.workflow.todo
            ("menu.workflow.todo", "en-US", "待办事项_us", "菜单导航"),
            // menu.workflow.todo
            ("menu.workflow.todo", "ja-JP", "待办事项_jp", "菜单导航"),
            // menu.workflow.todo
            ("menu.workflow.todo", "zh-CN", "待办事项", "菜单导航"),
            // menu.workflow.todo
            ("menu.workflow.todo", "zh-HK", "待办事项_hk", "菜单导航"),

            // menu.workflow.my
            ("menu.workflow.my", "en-US", "我的流程_us", "菜单导航"),
            // menu.workflow.my
            ("menu.workflow.my", "ja-JP", "我的流程_jp", "菜单导航"),
            // menu.workflow.my
            ("menu.workflow.my", "zh-CN", "我的流程", "菜单导航"),
            // menu.workflow.my
            ("menu.workflow.my", "zh-HK", "我的流程_hk", "菜单导航"),

            // menu.workflow.processed
            ("menu.workflow.processed", "en-US", "已处理_us", "菜单导航"),
            // menu.workflow.processed
            ("menu.workflow.processed", "ja-JP", "已处理_jp", "菜单导航"),
            // menu.workflow.processed
            ("menu.workflow.processed", "zh-CN", "已处理", "菜单导航"),
            // menu.workflow.processed
            ("menu.workflow.processed", "zh-HK", "已处理_hk", "菜单导航"),

            // menu.workflow.instance
            ("menu.workflow.instance", "en-US", "流程实例_us", "菜单导航"),
            // menu.workflow.instance
            ("menu.workflow.instance", "ja-JP", "流程实例_jp", "菜单导航"),
            // menu.workflow.instance
            ("menu.workflow.instance", "zh-CN", "流程实例", "菜单导航"),
            // menu.workflow.instance
            ("menu.workflow.instance", "zh-HK", "流程实例_hk", "菜单导航"),

            // menu.workflow.scheme
            ("menu.workflow.scheme", "en-US", "流程方案_us", "菜单导航"),
            // menu.workflow.scheme
            ("menu.workflow.scheme", "ja-JP", "流程方案_jp", "菜单导航"),
            // menu.workflow.scheme
            ("menu.workflow.scheme", "zh-CN", "流程方案", "菜单导航"),
            // menu.workflow.scheme
            ("menu.workflow.scheme", "zh-HK", "流程方案_hk", "菜单导航"),

            // menu.workflow.form
            ("menu.workflow.form", "en-US", "表单管理_us", "菜单导航"),
            // menu.workflow.form
            ("menu.workflow.form", "ja-JP", "表单管理_jp", "菜单导航"),
            // menu.workflow.form
            ("menu.workflow.form", "zh-CN", "表单管理", "菜单导航"),
            // menu.workflow.form
            ("menu.workflow.form", "zh-HK", "表单管理_hk", "菜单导航"),

            // menu.code.generator
            ("menu.code.generator", "en-US", "代码生成_us", "菜单导航"),
            // menu.code.generator
            ("menu.code.generator", "ja-JP", "代码生成_jp", "菜单导航"),
            // menu.code.generator
            ("menu.code.generator", "zh-CN", "代码生成", "菜单导航"),
            // menu.code.generator
            ("menu.code.generator", "zh-HK", "代码生成_hk", "菜单导航"),

            // menu.code.database.info
            ("menu.code.database.info", "en-US", "数据库信息_us", "菜单导航"),
            // menu.code.database.info
            ("menu.code.database.info", "ja-JP", "数据库信息_jp", "菜单导航"),
            // menu.code.database.info
            ("menu.code.database.info", "zh-CN", "数据库信息", "菜单导航"),
            // menu.code.database.info
            ("menu.code.database.info", "zh-HK", "数据库信息_hk", "菜单导航"),

            // menu.code.database.tableclone
            ("menu.code.database.tableclone", "en-US", "表克隆_us", "菜单导航"),
            // menu.code.database.tableclone
            ("menu.code.database.tableclone", "ja-JP", "表克隆_jp", "菜单导航"),
            // menu.code.database.tableclone
            ("menu.code.database.tableclone", "zh-CN", "表克隆", "菜单导航"),
            // menu.code.database.tableclone
            ("menu.code.database.tableclone", "zh-HK", "表克隆_hk", "菜单导航"),

            // menu.code.database.dataclone
            ("menu.code.database.dataclone", "en-US", "数据克隆_us", "菜单导航"),
            // menu.code.database.dataclone
            ("menu.code.database.dataclone", "ja-JP", "数据克隆_jp", "菜单导航"),
            // menu.code.database.dataclone
            ("menu.code.database.dataclone", "zh-CN", "数据克隆", "菜单导航"),
            // menu.code.database.dataclone
            ("menu.code.database.dataclone", "zh-HK", "数据克隆_hk", "菜单导航"),

            // menu.foundation.numbering
            ("menu.foundation.numbering", "en-US", "编码规则_us", "菜单导航"),
            // menu.foundation.numbering
            ("menu.foundation.numbering", "ja-JP", "编码规则_jp", "菜单导航"),
            // menu.foundation.numbering
            ("menu.foundation.numbering", "zh-CN", "编码规则", "菜单导航"),
            // menu.foundation.numbering
            ("menu.foundation.numbering", "zh-HK", "编码规则_hk", "菜单导航"),

            // menu.foundation.isocode
            ("menu.foundation.isocode", "en-US", "ISO编码_us", "菜单导航"),
            // menu.foundation.isocode
            ("menu.foundation.isocode", "ja-JP", "ISO编码_jp", "菜单导航"),
            // menu.foundation.isocode
            ("menu.foundation.isocode", "zh-CN", "ISO编码", "菜单导航"),
            // menu.foundation.isocode
            ("menu.foundation.isocode", "zh-HK", "ISO编码_hk", "菜单导航"),

            // menu.foundation.dict
            ("menu.foundation.dict", "en-US", "数据字典_us", "菜单导航"),
            // menu.foundation.dict
            ("menu.foundation.dict", "ja-JP", "数据字典_jp", "菜单导航"),
            // menu.foundation.dict
            ("menu.foundation.dict", "zh-CN", "数据字典", "菜单导航"),
            // menu.foundation.dict
            ("menu.foundation.dict", "zh-HK", "数据字典_hk", "菜单导航"),

            // menu.foundation.i18n
            ("menu.foundation.i18n", "en-US", "国际化_us", "菜单导航"),
            // menu.foundation.i18n
            ("menu.foundation.i18n", "ja-JP", "国际化_jp", "菜单导航"),
            // menu.foundation.i18n
            ("menu.foundation.i18n", "zh-CN", "国际化", "菜单导航"),
            // menu.foundation.i18n
            ("menu.foundation.i18n", "zh-HK", "国际化_hk", "菜单导航"),

            // menu.foundation.file
            ("menu.foundation.file", "en-US", "文件管理_us", "菜单导航"),
            // menu.foundation.file
            ("menu.foundation.file", "ja-JP", "文件管理_jp", "菜单导航"),
            // menu.foundation.file
            ("menu.foundation.file", "zh-CN", "文件管理", "菜单导航"),
            // menu.foundation.file
            ("menu.foundation.file", "zh-HK", "文件管理_hk", "菜单导航"),

            // menu.foundation.cache
            ("menu.foundation.cache", "en-US", "缓存管理_us", "菜单导航"),
            // menu.foundation.cache
            ("menu.foundation.cache", "ja-JP", "缓存管理_jp", "菜单导航"),
            // menu.foundation.cache
            ("menu.foundation.cache", "zh-CN", "缓存管理", "菜单导航"),
            // menu.foundation.cache
            ("menu.foundation.cache", "zh-HK", "缓存管理_hk", "菜单导航"),

            // menu.foundation.vocabulary
            ("menu.foundation.vocabulary", "en-US", "敏感词库_us", "菜单导航"),
            // menu.foundation.vocabulary
            ("menu.foundation.vocabulary", "ja-JP", "敏感词库_jp", "菜单导航"),
            // menu.foundation.vocabulary
            ("menu.foundation.vocabulary", "zh-CN", "敏感词库", "菜单导航"),
            // menu.foundation.vocabulary
            ("menu.foundation.vocabulary", "zh-HK", "敏感词库_hk", "菜单导航"),

            // menu.foundation.setting
            ("menu.foundation.setting", "en-US", "系统设置_us", "菜单导航"),
            // menu.foundation.setting
            ("menu.foundation.setting", "ja-JP", "系统设置_jp", "菜单导航"),
            // menu.foundation.setting
            ("menu.foundation.setting", "zh-CN", "系统设置", "菜单导航"),
            // menu.foundation.setting
            ("menu.foundation.setting", "zh-HK", "系统设置_hk", "菜单导航"),

            // menu.foundation.online
            ("menu.foundation.online", "en-US", "在线用户_us", "菜单导航"),
            // menu.foundation.online
            ("menu.foundation.online", "ja-JP", "在线用户_jp", "菜单导航"),
            // menu.foundation.online
            ("menu.foundation.online", "zh-CN", "在线用户", "菜单导航"),
            // menu.foundation.online
            ("menu.foundation.online", "zh-HK", "在线用户_hk", "菜单导航"),

            // menu.foundation.message
            ("menu.foundation.message", "en-US", "在线消息_us", "菜单导航"),
            // menu.foundation.message
            ("menu.foundation.message", "ja-JP", "在线消息_jp", "菜单导航"),
            // menu.foundation.message
            ("menu.foundation.message", "zh-CN", "在线消息", "菜单导航"),
            // menu.foundation.message
            ("menu.foundation.message", "zh-HK", "在线消息_hk", "菜单导航"),

            // menu.statistics.report._self
            ("menu.statistics.report._self", "en-US", "报表管理_us", "菜单导航"),
            // menu.statistics.report._self
            ("menu.statistics.report._self", "ja-JP", "报表管理_jp", "菜单导航"),
            // menu.statistics.report._self
            ("menu.statistics.report._self", "zh-CN", "报表管理", "菜单导航"),
            // menu.statistics.report._self
            ("menu.statistics.report._self", "zh-HK", "报表管理_hk", "菜单导航"),

            // menu.statistics.logging._self
            ("menu.statistics.logging._self", "en-US", "日志管理_us", "菜单导航"),
            // menu.statistics.logging._self
            ("menu.statistics.logging._self", "ja-JP", "日志管理_jp", "菜单导航"),
            // menu.statistics.logging._self
            ("menu.statistics.logging._self", "zh-CN", "日志管理", "菜单导航"),
            // menu.statistics.logging._self
            ("menu.statistics.logging._self", "zh-HK", "日志管理_hk", "菜单导航"),

            // menu.accounting.financial.accounttitle
            ("menu.accounting.financial.accounttitle", "en-US", "会计科目_us", "菜单导航"),
            // menu.accounting.financial.accounttitle
            ("menu.accounting.financial.accounttitle", "ja-JP", "会计科目_jp", "菜单导航"),
            // menu.accounting.financial.accounttitle
            ("menu.accounting.financial.accounttitle", "zh-CN", "会计科目", "菜单导航"),
            // menu.accounting.financial.accounttitle
            ("menu.accounting.financial.accounttitle", "zh-HK", "会计科目_hk", "菜单导航"),

            // menu.accounting.financial.accounttitle.changelog
            ("menu.accounting.financial.accounttitle.changelog", "en-US", "会计科目变更_us", "菜单导航"),
            // menu.accounting.financial.accounttitle.changelog
            ("menu.accounting.financial.accounttitle.changelog", "ja-JP", "会计科目变更_jp", "菜单导航"),
            // menu.accounting.financial.accounttitle.changelog
            ("menu.accounting.financial.accounttitle.changelog", "zh-CN", "会计科目变更", "菜单导航"),
            // menu.accounting.financial.accounttitle.changelog
            ("menu.accounting.financial.accounttitle.changelog", "zh-HK", "会计科目变更_hk", "菜单导航"),

            // menu.accounting.financial.asset
            ("menu.accounting.financial.asset", "en-US", "固定资产_us", "菜单导航"),
            // menu.accounting.financial.asset
            ("menu.accounting.financial.asset", "ja-JP", "固定资产_jp", "菜单导航"),
            // menu.accounting.financial.asset
            ("menu.accounting.financial.asset", "zh-CN", "固定资产", "菜单导航"),
            // menu.accounting.financial.asset
            ("menu.accounting.financial.asset", "zh-HK", "固定资产_hk", "菜单导航"),

            // menu.accounting.financial.asset.changelog
            ("menu.accounting.financial.asset.changelog", "en-US", "固定资产变更_us", "菜单导航"),
            // menu.accounting.financial.asset.changelog
            ("menu.accounting.financial.asset.changelog", "ja-JP", "固定资产变更_jp", "菜单导航"),
            // menu.accounting.financial.asset.changelog
            ("menu.accounting.financial.asset.changelog", "zh-CN", "固定资产变更", "菜单导航"),
            // menu.accounting.financial.asset.changelog
            ("menu.accounting.financial.asset.changelog", "zh-HK", "固定资产变更_hk", "菜单导航"),

            // menu.accounting.financial.countersign
            ("menu.accounting.financial.countersign", "en-US", "会签管理_us", "菜单导航"),
            // menu.accounting.financial.countersign
            ("menu.accounting.financial.countersign", "ja-JP", "会签管理_jp", "菜单导航"),
            // menu.accounting.financial.countersign
            ("menu.accounting.financial.countersign", "zh-CN", "会签管理", "菜单导航"),
            // menu.accounting.financial.countersign
            ("menu.accounting.financial.countersign", "zh-HK", "会签管理_hk", "菜单导航"),

            // menu.accounting.financial.company
            ("menu.accounting.financial.company", "en-US", "公司管理_us", "菜单导航"),
            // menu.accounting.financial.company
            ("menu.accounting.financial.company", "ja-JP", "公司管理_jp", "菜单导航"),
            // menu.accounting.financial.company
            ("menu.accounting.financial.company", "zh-CN", "公司管理", "菜单导航"),
            // menu.accounting.financial.company
            ("menu.accounting.financial.company", "zh-HK", "公司管理_hk", "菜单导航"),

            // menu.accounting.controlling.profitcenter
            ("menu.accounting.controlling.profitcenter", "en-US", "利润中心_us", "菜单导航"),
            // menu.accounting.controlling.profitcenter
            ("menu.accounting.controlling.profitcenter", "ja-JP", "利润中心_jp", "菜单导航"),
            // menu.accounting.controlling.profitcenter
            ("menu.accounting.controlling.profitcenter", "zh-CN", "利润中心", "菜单导航"),
            // menu.accounting.controlling.profitcenter
            ("menu.accounting.controlling.profitcenter", "zh-HK", "利润中心_hk", "菜单导航"),

            // menu.accounting.controlling.profitcenter.changelog
            ("menu.accounting.controlling.profitcenter.changelog", "en-US", "利润中心变更_us", "菜单导航"),
            // menu.accounting.controlling.profitcenter.changelog
            ("menu.accounting.controlling.profitcenter.changelog", "ja-JP", "利润中心变更_jp", "菜单导航"),
            // menu.accounting.controlling.profitcenter.changelog
            ("menu.accounting.controlling.profitcenter.changelog", "zh-CN", "利润中心变更", "菜单导航"),
            // menu.accounting.controlling.profitcenter.changelog
            ("menu.accounting.controlling.profitcenter.changelog", "zh-HK", "利润中心变更_hk", "菜单导航"),

            // menu.accounting.controlling.costcenter
            ("menu.accounting.controlling.costcenter", "en-US", "成本中心_us", "菜单导航"),
            // menu.accounting.controlling.costcenter
            ("menu.accounting.controlling.costcenter", "ja-JP", "成本中心_jp", "菜单导航"),
            // menu.accounting.controlling.costcenter
            ("menu.accounting.controlling.costcenter", "zh-CN", "成本中心", "菜单导航"),
            // menu.accounting.controlling.costcenter
            ("menu.accounting.controlling.costcenter", "zh-HK", "成本中心_hk", "菜单导航"),

            // menu.accounting.controlling.costcenter.changelog
            ("menu.accounting.controlling.costcenter.changelog", "en-US", "成本中心变更_us", "菜单导航"),
            // menu.accounting.controlling.costcenter.changelog
            ("menu.accounting.controlling.costcenter.changelog", "ja-JP", "成本中心变更_jp", "菜单导航"),
            // menu.accounting.controlling.costcenter.changelog
            ("menu.accounting.controlling.costcenter.changelog", "zh-CN", "成本中心变更", "菜单导航"),
            // menu.accounting.controlling.costcenter.changelog
            ("menu.accounting.controlling.costcenter.changelog", "zh-HK", "成本中心变更_hk", "菜单导航"),

            // menu.accounting.controlling.costelement
            ("menu.accounting.controlling.costelement", "en-US", "成本要素_us", "菜单导航"),
            // menu.accounting.controlling.costelement
            ("menu.accounting.controlling.costelement", "ja-JP", "成本要素_jp", "菜单导航"),
            // menu.accounting.controlling.costelement
            ("menu.accounting.controlling.costelement", "zh-CN", "成本要素", "菜单导航"),
            // menu.accounting.controlling.costelement
            ("menu.accounting.controlling.costelement", "zh-HK", "成本要素_hk", "菜单导航"),

            // menu.accounting.controlling.costelement.changelog
            ("menu.accounting.controlling.costelement.changelog", "en-US", "成本要素变更_us", "菜单导航"),
            // menu.accounting.controlling.costelement.changelog
            ("menu.accounting.controlling.costelement.changelog", "ja-JP", "成本要素变更_jp", "菜单导航"),
            // menu.accounting.controlling.costelement.changelog
            ("menu.accounting.controlling.costelement.changelog", "zh-CN", "成本要素变更", "菜单导航"),
            // menu.accounting.controlling.costelement.changelog
            ("menu.accounting.controlling.costelement.changelog", "zh-HK", "成本要素变更_hk", "菜单导航"),

            // menu.accounting.controlling.standardwagerate
            ("menu.accounting.controlling.standardwagerate", "en-US", "标准工资率_us", "菜单导航"),
            // menu.accounting.controlling.standardwagerate
            ("menu.accounting.controlling.standardwagerate", "ja-JP", "标准工资率_jp", "菜单导航"),
            // menu.accounting.controlling.standardwagerate
            ("menu.accounting.controlling.standardwagerate", "zh-CN", "标准工资率", "菜单导航"),
            // menu.accounting.controlling.standardwagerate
            ("menu.accounting.controlling.standardwagerate", "zh-HK", "标准工资率_hk", "菜单导航"),

            // menu.logistics.materials.plant
            ("menu.logistics.materials.plant", "en-US", "工厂信息_us", "菜单导航"),
            // menu.logistics.materials.plant
            ("menu.logistics.materials.plant", "ja-JP", "工厂信息_jp", "菜单导航"),
            // menu.logistics.materials.plant
            ("menu.logistics.materials.plant", "zh-CN", "工厂信息", "菜单导航"),
            // menu.logistics.materials.plant
            ("menu.logistics.materials.plant", "zh-HK", "工厂信息_hk", "菜单导航"),

            // menu.logistics.materials.material
            ("menu.logistics.materials.material", "en-US", "全局物料_us", "菜单导航"),
            // menu.logistics.materials.material
            ("menu.logistics.materials.material", "ja-JP", "全局物料_jp", "菜单导航"),
            // menu.logistics.materials.material
            ("menu.logistics.materials.material", "zh-CN", "全局物料", "菜单导航"),
            // menu.logistics.materials.material
            ("menu.logistics.materials.material", "zh-HK", "全局物料_hk", "菜单导航"),

            // menu.logistics.materials.material.changelog
            ("menu.logistics.materials.material.changelog", "en-US", "全局物料变更_us", "菜单导航"),
            // menu.logistics.materials.material.changelog
            ("menu.logistics.materials.material.changelog", "ja-JP", "全局物料变更_jp", "菜单导航"),
            // menu.logistics.materials.material.changelog
            ("menu.logistics.materials.material.changelog", "zh-CN", "全局物料变更", "菜单导航"),
            // menu.logistics.materials.material.changelog
            ("menu.logistics.materials.material.changelog", "zh-HK", "全局物料变更_hk", "菜单导航"),

            // menu.logistics.materials.materialplant
            ("menu.logistics.materials.materialplant", "en-US", "工厂物料_us", "菜单导航"),
            // menu.logistics.materials.materialplant
            ("menu.logistics.materials.materialplant", "ja-JP", "工厂物料_jp", "菜单导航"),
            // menu.logistics.materials.materialplant
            ("menu.logistics.materials.materialplant", "zh-CN", "工厂物料", "菜单导航"),
            // menu.logistics.materials.materialplant
            ("menu.logistics.materials.materialplant", "zh-HK", "工厂物料_hk", "菜单导航"),

            // menu.logistics.materials.materialplant.changelog
            ("menu.logistics.materials.materialplant.changelog", "en-US", "工厂物料变更_us", "菜单导航"),
            // menu.logistics.materials.materialplant.changelog
            ("menu.logistics.materials.materialplant.changelog", "ja-JP", "工厂物料变更_jp", "菜单导航"),
            // menu.logistics.materials.materialplant.changelog
            ("menu.logistics.materials.materialplant.changelog", "zh-CN", "工厂物料变更", "菜单导航"),
            // menu.logistics.materials.materialplant.changelog
            ("menu.logistics.materials.materialplant.changelog", "zh-HK", "工厂物料变更_hk", "菜单导航"),

            // menu.logistics.materials.warehouse
            ("menu.logistics.materials.warehouse", "en-US", "仓库信息_us", "菜单导航"),
            // menu.logistics.materials.warehouse
            ("menu.logistics.materials.warehouse", "ja-JP", "仓库信息_jp", "菜单导航"),
            // menu.logistics.materials.warehouse
            ("menu.logistics.materials.warehouse", "zh-CN", "仓库信息", "菜单导航"),
            // menu.logistics.materials.warehouse
            ("menu.logistics.materials.warehouse", "zh-HK", "仓库信息_hk", "菜单导航"),

            // menu.logistics.materials.materialgroup
            ("menu.logistics.materials.materialgroup", "en-US", "物料组_us", "菜单导航"),
            // menu.logistics.materials.materialgroup
            ("menu.logistics.materials.materialgroup", "ja-JP", "物料组_jp", "菜单导航"),
            // menu.logistics.materials.materialgroup
            ("menu.logistics.materials.materialgroup", "zh-CN", "物料组", "菜单导航"),
            // menu.logistics.materials.materialgroup
            ("menu.logistics.materials.materialgroup", "zh-HK", "物料组_hk", "菜单导航"),

            // menu.logistics.materials.storagelocation
            ("menu.logistics.materials.storagelocation", "en-US", "库位信息_us", "菜单导航"),
            // menu.logistics.materials.storagelocation
            ("menu.logistics.materials.storagelocation", "ja-JP", "库位信息_jp", "菜单导航"),
            // menu.logistics.materials.storagelocation
            ("menu.logistics.materials.storagelocation", "zh-CN", "库位信息", "菜单导航"),
            // menu.logistics.materials.storagelocation
            ("menu.logistics.materials.storagelocation", "zh-HK", "库位信息_hk", "菜单导航"),

            // menu.logistics.materials.packaging
            ("menu.logistics.materials.packaging", "en-US", "包装物料_us", "菜单导航"),
            // menu.logistics.materials.packaging
            ("menu.logistics.materials.packaging", "ja-JP", "包装物料_jp", "菜单导航"),
            // menu.logistics.materials.packaging
            ("menu.logistics.materials.packaging", "zh-CN", "包装物料", "菜单导航"),
            // menu.logistics.materials.packaging
            ("menu.logistics.materials.packaging", "zh-HK", "包装物料_hk", "菜单导航"),

            // menu.logistics.materials.modeldestination
            ("menu.logistics.materials.modeldestination", "en-US", "机种仕向_us", "菜单导航"),
            // menu.logistics.materials.modeldestination
            ("menu.logistics.materials.modeldestination", "ja-JP", "机种仕向_jp", "菜单导航"),
            // menu.logistics.materials.modeldestination
            ("menu.logistics.materials.modeldestination", "zh-CN", "机种仕向", "菜单导航"),
            // menu.logistics.materials.modeldestination
            ("menu.logistics.materials.modeldestination", "zh-HK", "机种仕向_hk", "菜单导航"),

            // menu.logistics.materials.manufacturer
            ("menu.logistics.materials.manufacturer", "en-US", "制造商物料_us", "菜单导航"),
            // menu.logistics.materials.manufacturer
            ("menu.logistics.materials.manufacturer", "ja-JP", "制造商物料_jp", "菜单导航"),
            // menu.logistics.materials.manufacturer
            ("menu.logistics.materials.manufacturer", "zh-CN", "制造商物料", "菜单导航"),
            // menu.logistics.materials.manufacturer
            ("menu.logistics.materials.manufacturer", "zh-HK", "制造商物料_hk", "菜单导航"),

            // menu.logistics.materials.materialtransaction
            ("menu.logistics.materials.materialtransaction", "en-US", "物料交易_us", "菜单导航"),
            // menu.logistics.materials.materialtransaction
            ("menu.logistics.materials.materialtransaction", "ja-JP", "物料交易_jp", "菜单导航"),
            // menu.logistics.materials.materialtransaction
            ("menu.logistics.materials.materialtransaction", "zh-CN", "物料交易", "菜单导航"),
            // menu.logistics.materials.materialtransaction
            ("menu.logistics.materials.materialtransaction", "zh-HK", "物料交易_hk", "菜单导航"),

            // menu.logistics.procurement.supplier
            ("menu.logistics.procurement.supplier", "en-US", "供应商_us", "菜单导航"),
            // menu.logistics.procurement.supplier
            ("menu.logistics.procurement.supplier", "ja-JP", "供应商_jp", "菜单导航"),
            // menu.logistics.procurement.supplier
            ("menu.logistics.procurement.supplier", "zh-CN", "供应商", "菜单导航"),
            // menu.logistics.procurement.supplier
            ("menu.logistics.procurement.supplier", "zh-HK", "供应商_hk", "菜单导航"),

            // menu.logistics.procurement.vendor
            ("menu.logistics.procurement.vendor", "en-US", "经销商_us", "菜单导航"),
            // menu.logistics.procurement.vendor
            ("menu.logistics.procurement.vendor", "ja-JP", "经销商_jp", "菜单导航"),
            // menu.logistics.procurement.vendor
            ("menu.logistics.procurement.vendor", "zh-CN", "经销商", "菜单导航"),
            // menu.logistics.procurement.vendor
            ("menu.logistics.procurement.vendor", "zh-HK", "经销商_hk", "菜单导航"),

            // menu.logistics.procurement.sourceofsupply
            ("menu.logistics.procurement.sourceofsupply", "en-US", "货源_us", "菜单导航"),
            // menu.logistics.procurement.sourceofsupply
            ("menu.logistics.procurement.sourceofsupply", "ja-JP", "货源_jp", "菜单导航"),
            // menu.logistics.procurement.sourceofsupply
            ("menu.logistics.procurement.sourceofsupply", "zh-CN", "货源", "菜单导航"),
            // menu.logistics.procurement.sourceofsupply
            ("menu.logistics.procurement.sourceofsupply", "zh-HK", "货源_hk", "菜单导航"),

            // menu.logistics.procurement.purchaserequest
            ("menu.logistics.procurement.purchaserequest", "en-US", "采购申请_us", "菜单导航"),
            // menu.logistics.procurement.purchaserequest
            ("menu.logistics.procurement.purchaserequest", "ja-JP", "采购申请_jp", "菜单导航"),
            // menu.logistics.procurement.purchaserequest
            ("menu.logistics.procurement.purchaserequest", "zh-CN", "采购申请", "菜单导航"),
            // menu.logistics.procurement.purchaserequest
            ("menu.logistics.procurement.purchaserequest", "zh-HK", "采购申请_hk", "菜单导航"),

            // menu.logistics.procurement.purchaserequest.changelog
            ("menu.logistics.procurement.purchaserequest.changelog", "en-US", "采购申请变更_us", "菜单导航"),
            // menu.logistics.procurement.purchaserequest.changelog
            ("menu.logistics.procurement.purchaserequest.changelog", "ja-JP", "采购申请变更_jp", "菜单导航"),
            // menu.logistics.procurement.purchaserequest.changelog
            ("menu.logistics.procurement.purchaserequest.changelog", "zh-CN", "采购申请变更", "菜单导航"),
            // menu.logistics.procurement.purchaserequest.changelog
            ("menu.logistics.procurement.purchaserequest.changelog", "zh-HK", "采购申请变更_hk", "菜单导航"),

            // menu.logistics.procurement.purchaseorder
            ("menu.logistics.procurement.purchaseorder", "en-US", "采购订单_us", "菜单导航"),
            // menu.logistics.procurement.purchaseorder
            ("menu.logistics.procurement.purchaseorder", "ja-JP", "采购订单_jp", "菜单导航"),
            // menu.logistics.procurement.purchaseorder
            ("menu.logistics.procurement.purchaseorder", "zh-CN", "采购订单", "菜单导航"),
            // menu.logistics.procurement.purchaseorder
            ("menu.logistics.procurement.purchaseorder", "zh-HK", "采购订单_hk", "菜单导航"),

            // menu.logistics.procurement.purchaseorder.changelog
            ("menu.logistics.procurement.purchaseorder.changelog", "en-US", "采购订单变更_us", "菜单导航"),
            // menu.logistics.procurement.purchaseorder.changelog
            ("menu.logistics.procurement.purchaseorder.changelog", "ja-JP", "采购订单变更_jp", "菜单导航"),
            // menu.logistics.procurement.purchaseorder.changelog
            ("menu.logistics.procurement.purchaseorder.changelog", "zh-CN", "采购订单变更", "菜单导航"),
            // menu.logistics.procurement.purchaseorder.changelog
            ("menu.logistics.procurement.purchaseorder.changelog", "zh-HK", "采购订单变更_hk", "菜单导航"),

            // menu.logistics.procurement.purchaseprice
            ("menu.logistics.procurement.purchaseprice", "en-US", "采购价格_us", "菜单导航"),
            // menu.logistics.procurement.purchaseprice
            ("menu.logistics.procurement.purchaseprice", "ja-JP", "采购价格_jp", "菜单导航"),
            // menu.logistics.procurement.purchaseprice
            ("menu.logistics.procurement.purchaseprice", "zh-CN", "采购价格", "菜单导航"),
            // menu.logistics.procurement.purchaseprice
            ("menu.logistics.procurement.purchaseprice", "zh-HK", "采购价格_hk", "菜单导航"),

            // menu.logistics.procurement.purchaseprice.changelog
            ("menu.logistics.procurement.purchaseprice.changelog", "en-US", "采购价格变更_us", "菜单导航"),
            // menu.logistics.procurement.purchaseprice.changelog
            ("menu.logistics.procurement.purchaseprice.changelog", "ja-JP", "采购价格变更_jp", "菜单导航"),
            // menu.logistics.procurement.purchaseprice.changelog
            ("menu.logistics.procurement.purchaseprice.changelog", "zh-CN", "采购价格变更", "菜单导航"),
            // menu.logistics.procurement.purchaseprice.changelog
            ("menu.logistics.procurement.purchaseprice.changelog", "zh-HK", "采购价格变更_hk", "菜单导航"),

            // menu.logistics.procurement.purchaseinvoice
            ("menu.logistics.procurement.purchaseinvoice", "en-US", "采购发票_us", "菜单导航"),
            // menu.logistics.procurement.purchaseinvoice
            ("menu.logistics.procurement.purchaseinvoice", "ja-JP", "采购发票_jp", "菜单导航"),
            // menu.logistics.procurement.purchaseinvoice
            ("menu.logistics.procurement.purchaseinvoice", "zh-CN", "采购发票", "菜单导航"),
            // menu.logistics.procurement.purchaseinvoice
            ("menu.logistics.procurement.purchaseinvoice", "zh-HK", "采购发票_hk", "菜单导航"),

            // menu.logistics.procurement.purchasegroup
            ("menu.logistics.procurement.purchasegroup", "en-US", "采购组_us", "菜单导航"),
            // menu.logistics.procurement.purchasegroup
            ("menu.logistics.procurement.purchasegroup", "ja-JP", "采购组_jp", "菜单导航"),
            // menu.logistics.procurement.purchasegroup
            ("menu.logistics.procurement.purchasegroup", "zh-CN", "采购组", "菜单导航"),
            // menu.logistics.procurement.purchasegroup
            ("menu.logistics.procurement.purchasegroup", "zh-HK", "采购组_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom._self
            ("menu.logistics.manufacturing.bom._self", "en-US", "BOM管理_us", "菜单导航"),
            // menu.logistics.manufacturing.bom._self
            ("menu.logistics.manufacturing.bom._self", "ja-JP", "BOM管理_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom._self
            ("menu.logistics.manufacturing.bom._self", "zh-CN", "BOM管理", "菜单导航"),
            // menu.logistics.manufacturing.bom._self
            ("menu.logistics.manufacturing.bom._self", "zh-HK", "BOM管理_hk", "菜单导航"),

            // menu.logistics.manufacturing.planning._self
            ("menu.logistics.manufacturing.planning._self", "en-US", "MRP计划_us", "菜单导航"),
            // menu.logistics.manufacturing.planning._self
            ("menu.logistics.manufacturing.planning._self", "ja-JP", "MRP计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.planning._self
            ("menu.logistics.manufacturing.planning._self", "zh-CN", "MRP计划", "菜单导航"),
            // menu.logistics.manufacturing.planning._self
            ("menu.logistics.manufacturing.planning._self", "zh-HK", "MRP计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.scheduling._self
            ("menu.logistics.manufacturing.scheduling._self", "en-US", "生产排程_us", "菜单导航"),
            // menu.logistics.manufacturing.scheduling._self
            ("menu.logistics.manufacturing.scheduling._self", "ja-JP", "生产排程_jp", "菜单导航"),
            // menu.logistics.manufacturing.scheduling._self
            ("menu.logistics.manufacturing.scheduling._self", "zh-CN", "生产排程", "菜单导航"),
            // menu.logistics.manufacturing.scheduling._self
            ("menu.logistics.manufacturing.scheduling._self", "zh-HK", "生产排程_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange._self
            ("menu.logistics.manufacturing.engineeringchange._self", "en-US", "设变_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange._self
            ("menu.logistics.manufacturing.engineeringchange._self", "ja-JP", "设变_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange._self
            ("menu.logistics.manufacturing.engineeringchange._self", "zh-CN", "设变", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange._self
            ("menu.logistics.manufacturing.engineeringchange._self", "zh-HK", "设变_hk", "菜单导航"),

            // menu.logistics.manufacturing.output._self
            ("menu.logistics.manufacturing.output._self", "en-US", "产出管理_us", "菜单导航"),
            // menu.logistics.manufacturing.output._self
            ("menu.logistics.manufacturing.output._self", "ja-JP", "产出管理_jp", "菜单导航"),
            // menu.logistics.manufacturing.output._self
            ("menu.logistics.manufacturing.output._self", "zh-CN", "产出管理", "菜单导航"),
            // menu.logistics.manufacturing.output._self
            ("menu.logistics.manufacturing.output._self", "zh-HK", "产出管理_hk", "菜单导航"),

            // menu.logistics.manufacturing.defect._self
            ("menu.logistics.manufacturing.defect._self", "en-US", "不良_us", "菜单导航"),
            // menu.logistics.manufacturing.defect._self
            ("menu.logistics.manufacturing.defect._self", "ja-JP", "不良_jp", "菜单导航"),
            // menu.logistics.manufacturing.defect._self
            ("menu.logistics.manufacturing.defect._self", "zh-CN", "不良", "菜单导航"),
            // menu.logistics.manufacturing.defect._self
            ("menu.logistics.manufacturing.defect._self", "zh-HK", "不良_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop._self
            ("menu.logistics.manufacturing.sop._self", "en-US", "SOP管理_us", "菜单导航"),
            // menu.logistics.manufacturing.sop._self
            ("menu.logistics.manufacturing.sop._self", "ja-JP", "SOP管理_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop._self
            ("menu.logistics.manufacturing.sop._self", "zh-CN", "SOP管理", "菜单导航"),
            // menu.logistics.manufacturing.sop._self
            ("menu.logistics.manufacturing.sop._self", "zh-HK", "SOP管理_hk", "菜单导航"),

            // menu.logistics.quality.cost._self
            ("menu.logistics.quality.cost._self", "en-US", "品质成本_us", "菜单导航"),
            // menu.logistics.quality.cost._self
            ("menu.logistics.quality.cost._self", "ja-JP", "品质成本_jp", "菜单导航"),
            // menu.logistics.quality.cost._self
            ("menu.logistics.quality.cost._self", "zh-CN", "品质成本", "菜单导航"),
            // menu.logistics.quality.cost._self
            ("menu.logistics.quality.cost._self", "zh-HK", "品质成本_hk", "菜单导航"),

            // menu.logistics.quality.operation._self
            ("menu.logistics.quality.operation._self", "en-US", "质量业务_us", "菜单导航"),
            // menu.logistics.quality.operation._self
            ("menu.logistics.quality.operation._self", "ja-JP", "质量业务_jp", "菜单导航"),
            // menu.logistics.quality.operation._self
            ("menu.logistics.quality.operation._self", "zh-CN", "质量业务", "菜单导航"),
            // menu.logistics.quality.operation._self
            ("menu.logistics.quality.operation._self", "zh-HK", "质量业务_hk", "菜单导航"),

            // menu.logistics.quality.complaint._self
            ("menu.logistics.quality.complaint._self", "en-US", "客诉管理_us", "菜单导航"),
            // menu.logistics.quality.complaint._self
            ("menu.logistics.quality.complaint._self", "ja-JP", "客诉管理_jp", "菜单导航"),
            // menu.logistics.quality.complaint._self
            ("menu.logistics.quality.complaint._self", "zh-CN", "客诉管理", "菜单导航"),
            // menu.logistics.quality.complaint._self
            ("menu.logistics.quality.complaint._self", "zh-HK", "客诉管理_hk", "菜单导航"),

            // menu.logistics.service.request
            ("menu.logistics.service.request", "en-US", "服务请求_us", "菜单导航"),
            // menu.logistics.service.request
            ("menu.logistics.service.request", "ja-JP", "服务请求_jp", "菜单导航"),
            // menu.logistics.service.request
            ("menu.logistics.service.request", "zh-CN", "服务请求", "菜单导航"),
            // menu.logistics.service.request
            ("menu.logistics.service.request", "zh-HK", "服务请求_hk", "菜单导航"),

            // menu.logistics.service.contract
            ("menu.logistics.service.contract", "en-US", "服务合同_us", "菜单导航"),
            // menu.logistics.service.contract
            ("menu.logistics.service.contract", "ja-JP", "服务合同_jp", "菜单导航"),
            // menu.logistics.service.contract
            ("menu.logistics.service.contract", "zh-CN", "服务合同", "菜单导航"),
            // menu.logistics.service.contract
            ("menu.logistics.service.contract", "zh-HK", "服务合同_hk", "菜单导航"),

            // menu.logistics.service.order
            ("menu.logistics.service.order", "en-US", "服务订单_us", "菜单导航"),
            // menu.logistics.service.order
            ("menu.logistics.service.order", "ja-JP", "服务订单_jp", "菜单导航"),
            // menu.logistics.service.order
            ("menu.logistics.service.order", "zh-CN", "服务订单", "菜单导航"),
            // menu.logistics.service.order
            ("menu.logistics.service.order", "zh-HK", "服务订单_hk", "菜单导航"),

            // menu.logistics.service.ticket
            ("menu.logistics.service.ticket", "en-US", "服务工单_us", "菜单导航"),
            // menu.logistics.service.ticket
            ("menu.logistics.service.ticket", "ja-JP", "服务工单_jp", "菜单导航"),
            // menu.logistics.service.ticket
            ("menu.logistics.service.ticket", "zh-CN", "服务工单", "菜单导航"),
            // menu.logistics.service.ticket
            ("menu.logistics.service.ticket", "zh-HK", "服务工单_hk", "菜单导航"),

            // menu.logistics.maintenance.equipment
            ("menu.logistics.maintenance.equipment", "en-US", "设备信息_us", "菜单导航"),
            // menu.logistics.maintenance.equipment
            ("menu.logistics.maintenance.equipment", "ja-JP", "设备信息_jp", "菜单导航"),
            // menu.logistics.maintenance.equipment
            ("menu.logistics.maintenance.equipment", "zh-CN", "设备信息", "菜单导航"),
            // menu.logistics.maintenance.equipment
            ("menu.logistics.maintenance.equipment", "zh-HK", "设备信息_hk", "菜单导航"),

            // menu.logistics.maintenance.notification
            ("menu.logistics.maintenance.notification", "en-US", "维护通知_us", "菜单导航"),
            // menu.logistics.maintenance.notification
            ("menu.logistics.maintenance.notification", "ja-JP", "维护通知_jp", "菜单导航"),
            // menu.logistics.maintenance.notification
            ("menu.logistics.maintenance.notification", "zh-CN", "维护通知", "菜单导航"),
            // menu.logistics.maintenance.notification
            ("menu.logistics.maintenance.notification", "zh-HK", "维护通知_hk", "菜单导航"),

            // menu.logistics.maintenance.workorder
            ("menu.logistics.maintenance.workorder", "en-US", "维护工单_us", "菜单导航"),
            // menu.logistics.maintenance.workorder
            ("menu.logistics.maintenance.workorder", "ja-JP", "维护工单_jp", "菜单导航"),
            // menu.logistics.maintenance.workorder
            ("menu.logistics.maintenance.workorder", "zh-CN", "维护工单", "菜单导航"),
            // menu.logistics.maintenance.workorder
            ("menu.logistics.maintenance.workorder", "zh-HK", "维护工单_hk", "菜单导航"),

            // menu.logistics.maintenance.history
            ("menu.logistics.maintenance.history", "en-US", "维护履历_us", "菜单导航"),
            // menu.logistics.maintenance.history
            ("menu.logistics.maintenance.history", "ja-JP", "维护履历_jp", "菜单导航"),
            // menu.logistics.maintenance.history
            ("menu.logistics.maintenance.history", "zh-CN", "维护履历", "菜单导航"),
            // menu.logistics.maintenance.history
            ("menu.logistics.maintenance.history", "zh-HK", "维护履历_hk", "菜单导航"),

            // menu.logistics.sales.customer
            ("menu.logistics.sales.customer", "en-US", "客户信息_us", "菜单导航"),
            // menu.logistics.sales.customer
            ("menu.logistics.sales.customer", "ja-JP", "客户信息_jp", "菜单导航"),
            // menu.logistics.sales.customer
            ("menu.logistics.sales.customer", "zh-CN", "客户信息", "菜单导航"),
            // menu.logistics.sales.customer
            ("menu.logistics.sales.customer", "zh-HK", "客户信息_hk", "菜单导航"),

            // menu.logistics.sales.client
            ("menu.logistics.sales.client", "en-US", "顾客信息_us", "菜单导航"),
            // menu.logistics.sales.client
            ("menu.logistics.sales.client", "ja-JP", "顾客信息_jp", "菜单导航"),
            // menu.logistics.sales.client
            ("menu.logistics.sales.client", "zh-CN", "顾客信息", "菜单导航"),
            // menu.logistics.sales.client
            ("menu.logistics.sales.client", "zh-HK", "顾客信息_hk", "菜单导航"),

            // menu.logistics.sales.quotation
            ("menu.logistics.sales.quotation", "en-US", "销售报价_us", "菜单导航"),
            // menu.logistics.sales.quotation
            ("menu.logistics.sales.quotation", "ja-JP", "销售报价_jp", "菜单导航"),
            // menu.logistics.sales.quotation
            ("menu.logistics.sales.quotation", "zh-CN", "销售报价", "菜单导航"),
            // menu.logistics.sales.quotation
            ("menu.logistics.sales.quotation", "zh-HK", "销售报价_hk", "菜单导航"),

            // menu.logistics.sales.quotation.changelog
            ("menu.logistics.sales.quotation.changelog", "en-US", "销售报价变更_us", "菜单导航"),
            // menu.logistics.sales.quotation.changelog
            ("menu.logistics.sales.quotation.changelog", "ja-JP", "销售报价变更_jp", "菜单导航"),
            // menu.logistics.sales.quotation.changelog
            ("menu.logistics.sales.quotation.changelog", "zh-CN", "销售报价变更", "菜单导航"),
            // menu.logistics.sales.quotation.changelog
            ("menu.logistics.sales.quotation.changelog", "zh-HK", "销售报价变更_hk", "菜单导航"),

            // menu.logistics.sales.price
            ("menu.logistics.sales.price", "en-US", "销售价格_us", "菜单导航"),
            // menu.logistics.sales.price
            ("menu.logistics.sales.price", "ja-JP", "销售价格_jp", "菜单导航"),
            // menu.logistics.sales.price
            ("menu.logistics.sales.price", "zh-CN", "销售价格", "菜单导航"),
            // menu.logistics.sales.price
            ("menu.logistics.sales.price", "zh-HK", "销售价格_hk", "菜单导航"),

            // menu.logistics.sales.price.changelog
            ("menu.logistics.sales.price.changelog", "en-US", "销售价格变更_us", "菜单导航"),
            // menu.logistics.sales.price.changelog
            ("menu.logistics.sales.price.changelog", "ja-JP", "销售价格变更_jp", "菜单导航"),
            // menu.logistics.sales.price.changelog
            ("menu.logistics.sales.price.changelog", "zh-CN", "销售价格变更", "菜单导航"),
            // menu.logistics.sales.price.changelog
            ("menu.logistics.sales.price.changelog", "zh-HK", "销售价格变更_hk", "菜单导航"),

            // menu.logistics.sales.order
            ("menu.logistics.sales.order", "en-US", "销售订单_us", "菜单导航"),
            // menu.logistics.sales.order
            ("menu.logistics.sales.order", "ja-JP", "销售订单_jp", "菜单导航"),
            // menu.logistics.sales.order
            ("menu.logistics.sales.order", "zh-CN", "销售订单", "菜单导航"),
            // menu.logistics.sales.order
            ("menu.logistics.sales.order", "zh-HK", "销售订单_hk", "菜单导航"),

            // menu.logistics.sales.order.changelog
            ("menu.logistics.sales.order.changelog", "en-US", "销售订单变更_us", "菜单导航"),
            // menu.logistics.sales.order.changelog
            ("menu.logistics.sales.order.changelog", "ja-JP", "销售订单变更_jp", "菜单导航"),
            // menu.logistics.sales.order.changelog
            ("menu.logistics.sales.order.changelog", "zh-CN", "销售订单变更", "菜单导航"),
            // menu.logistics.sales.order.changelog
            ("menu.logistics.sales.order.changelog", "zh-HK", "销售订单变更_hk", "菜单导航"),

            // menu.logistics.sales.invoice
            ("menu.logistics.sales.invoice", "en-US", "销售发票_us", "菜单导航"),
            // menu.logistics.sales.invoice
            ("menu.logistics.sales.invoice", "ja-JP", "销售发票_jp", "菜单导航"),
            // menu.logistics.sales.invoice
            ("menu.logistics.sales.invoice", "zh-CN", "销售发票", "菜单导航"),
            // menu.logistics.sales.invoice
            ("menu.logistics.sales.invoice", "zh-HK", "销售发票_hk", "菜单导航"),

            // menu.logistics.serial.inbound
            ("menu.logistics.serial.inbound", "en-US", "序列号入库_us", "菜单导航"),
            // menu.logistics.serial.inbound
            ("menu.logistics.serial.inbound", "ja-JP", "序列号入库_jp", "菜单导航"),
            // menu.logistics.serial.inbound
            ("menu.logistics.serial.inbound", "zh-CN", "序列号入库", "菜单导航"),
            // menu.logistics.serial.inbound
            ("menu.logistics.serial.inbound", "zh-HK", "序列号入库_hk", "菜单导航"),

            // menu.logistics.serial.outbound
            ("menu.logistics.serial.outbound", "en-US", "序列号出库_us", "菜单导航"),
            // menu.logistics.serial.outbound
            ("menu.logistics.serial.outbound", "ja-JP", "序列号出库_jp", "菜单导航"),
            // menu.logistics.serial.outbound
            ("menu.logistics.serial.outbound", "zh-CN", "序列号出库", "菜单导航"),
            // menu.logistics.serial.outbound
            ("menu.logistics.serial.outbound", "zh-HK", "序列号出库_hk", "菜单导航"),

            // menu.humanresource.organization.dept
            ("menu.humanresource.organization.dept", "en-US", "部门管理_us", "菜单导航"),
            // menu.humanresource.organization.dept
            ("menu.humanresource.organization.dept", "ja-JP", "部门管理_jp", "菜单导航"),
            // menu.humanresource.organization.dept
            ("menu.humanresource.organization.dept", "zh-CN", "部门管理", "菜单导航"),
            // menu.humanresource.organization.dept
            ("menu.humanresource.organization.dept", "zh-HK", "部门管理_hk", "菜单导航"),

            // menu.humanresource.organization.post
            ("menu.humanresource.organization.post", "en-US", "岗位管理_us", "菜单导航"),
            // menu.humanresource.organization.post
            ("menu.humanresource.organization.post", "ja-JP", "岗位管理_jp", "菜单导航"),
            // menu.humanresource.organization.post
            ("menu.humanresource.organization.post", "zh-CN", "岗位管理", "菜单导航"),
            // menu.humanresource.organization.post
            ("menu.humanresource.organization.post", "zh-HK", "岗位管理_hk", "菜单导航"),

            // menu.humanresource.personnel.employee
            ("menu.humanresource.personnel.employee", "en-US", "员工档案_us", "菜单导航"),
            // menu.humanresource.personnel.employee
            ("menu.humanresource.personnel.employee", "ja-JP", "员工档案_jp", "菜单导航"),
            // menu.humanresource.personnel.employee
            ("menu.humanresource.personnel.employee", "zh-CN", "员工档案", "菜单导航"),
            // menu.humanresource.personnel.employee
            ("menu.humanresource.personnel.employee", "zh-HK", "员工档案_hk", "菜单导航"),

            // menu.humanresource.personnel.employeecontract
            ("menu.humanresource.personnel.employeecontract", "en-US", "员工合同_us", "菜单导航"),
            // menu.humanresource.personnel.employeecontract
            ("menu.humanresource.personnel.employeecontract", "ja-JP", "员工合同_jp", "菜单导航"),
            // menu.humanresource.personnel.employeecontract
            ("menu.humanresource.personnel.employeecontract", "zh-CN", "员工合同", "菜单导航"),
            // menu.humanresource.personnel.employeecontract
            ("menu.humanresource.personnel.employeecontract", "zh-HK", "员工合同_hk", "菜单导航"),

            // menu.humanresource.personnel.employeedelegate
            ("menu.humanresource.personnel.employeedelegate", "en-US", "员工代理_us", "菜单导航"),
            // menu.humanresource.personnel.employeedelegate
            ("menu.humanresource.personnel.employeedelegate", "ja-JP", "员工代理_jp", "菜单导航"),
            // menu.humanresource.personnel.employeedelegate
            ("menu.humanresource.personnel.employeedelegate", "zh-CN", "员工代理", "菜单导航"),
            // menu.humanresource.personnel.employeedelegate
            ("menu.humanresource.personnel.employeedelegate", "zh-HK", "员工代理_hk", "菜单导航"),

            // menu.humanresource.personnel.employeereassignment
            ("menu.humanresource.personnel.employeereassignment", "en-US", "员工调动_us", "菜单导航"),
            // menu.humanresource.personnel.employeereassignment
            ("menu.humanresource.personnel.employeereassignment", "ja-JP", "员工调动_jp", "菜单导航"),
            // menu.humanresource.personnel.employeereassignment
            ("menu.humanresource.personnel.employeereassignment", "zh-CN", "员工调动", "菜单导航"),
            // menu.humanresource.personnel.employeereassignment
            ("menu.humanresource.personnel.employeereassignment", "zh-HK", "员工调动_hk", "菜单导航"),

            // menu.humanresource.personnel.employeeonboarding
            ("menu.humanresource.personnel.employeeonboarding", "en-US", "入职待办_us", "菜单导航"),
            // menu.humanresource.personnel.employeeonboarding
            ("menu.humanresource.personnel.employeeonboarding", "ja-JP", "入职待办_jp", "菜单导航"),
            // menu.humanresource.personnel.employeeonboarding
            ("menu.humanresource.personnel.employeeonboarding", "zh-CN", "入职待办", "菜单导航"),
            // menu.humanresource.personnel.employeeonboarding
            ("menu.humanresource.personnel.employeeonboarding", "zh-HK", "入职待办_hk", "菜单导航"),

            // menu.humanresource.attendance.calendar
            ("menu.humanresource.attendance.calendar", "en-US", "工厂日历_us", "菜单导航"),
            // menu.humanresource.attendance.calendar
            ("menu.humanresource.attendance.calendar", "ja-JP", "工厂日历_jp", "菜单导航"),
            // menu.humanresource.attendance.calendar
            ("menu.humanresource.attendance.calendar", "zh-CN", "工厂日历", "菜单导航"),
            // menu.humanresource.attendance.calendar
            ("menu.humanresource.attendance.calendar", "zh-HK", "工厂日历_hk", "菜单导航"),

            // menu.humanresource.attendance.holiday
            ("menu.humanresource.attendance.holiday", "en-US", "假期管理_us", "菜单导航"),
            // menu.humanresource.attendance.holiday
            ("menu.humanresource.attendance.holiday", "ja-JP", "假期管理_jp", "菜单导航"),
            // menu.humanresource.attendance.holiday
            ("menu.humanresource.attendance.holiday", "zh-CN", "假期管理", "菜单导航"),
            // menu.humanresource.attendance.holiday
            ("menu.humanresource.attendance.holiday", "zh-HK", "假期管理_hk", "菜单导航"),

            // menu.humanresource.attendance.shiftschedule
            ("menu.humanresource.attendance.shiftschedule", "en-US", "排班计划_us", "菜单导航"),
            // menu.humanresource.attendance.shiftschedule
            ("menu.humanresource.attendance.shiftschedule", "ja-JP", "排班计划_jp", "菜单导航"),
            // menu.humanresource.attendance.shiftschedule
            ("menu.humanresource.attendance.shiftschedule", "zh-CN", "排班计划", "菜单导航"),
            // menu.humanresource.attendance.shiftschedule
            ("menu.humanresource.attendance.shiftschedule", "zh-HK", "排班计划_hk", "菜单导航"),

            // menu.humanresource.attendance.workshift
            ("menu.humanresource.attendance.workshift", "en-US", "班次管理_us", "菜单导航"),
            // menu.humanresource.attendance.workshift
            ("menu.humanresource.attendance.workshift", "ja-JP", "班次管理_jp", "菜单导航"),
            // menu.humanresource.attendance.workshift
            ("menu.humanresource.attendance.workshift", "zh-CN", "班次管理", "菜单导航"),
            // menu.humanresource.attendance.workshift
            ("menu.humanresource.attendance.workshift", "zh-HK", "班次管理_hk", "菜单导航"),

            // menu.humanresource.attendance.leave
            ("menu.humanresource.attendance.leave", "en-US", "请假管理_us", "菜单导航"),
            // menu.humanresource.attendance.leave
            ("menu.humanresource.attendance.leave", "ja-JP", "请假管理_jp", "菜单导航"),
            // menu.humanresource.attendance.leave
            ("menu.humanresource.attendance.leave", "zh-CN", "请假管理", "菜单导航"),
            // menu.humanresource.attendance.leave
            ("menu.humanresource.attendance.leave", "zh-HK", "请假管理_hk", "菜单导航"),

            // menu.humanresource.attendance.overtime
            ("menu.humanresource.attendance.overtime", "en-US", "加班管理_us", "菜单导航"),
            // menu.humanresource.attendance.overtime
            ("menu.humanresource.attendance.overtime", "ja-JP", "加班管理_jp", "菜单导航"),
            // menu.humanresource.attendance.overtime
            ("menu.humanresource.attendance.overtime", "zh-CN", "加班管理", "菜单导航"),
            // menu.humanresource.attendance.overtime
            ("menu.humanresource.attendance.overtime", "zh-HK", "加班管理_hk", "菜单导航"),

            // menu.humanresource.compensation.salaryitem
            ("menu.humanresource.compensation.salaryitem", "en-US", "薪资项目_us", "菜单导航"),
            // menu.humanresource.compensation.salaryitem
            ("menu.humanresource.compensation.salaryitem", "ja-JP", "薪资项目_jp", "菜单导航"),
            // menu.humanresource.compensation.salaryitem
            ("menu.humanresource.compensation.salaryitem", "zh-CN", "薪资项目", "菜单导航"),
            // menu.humanresource.compensation.salaryitem
            ("menu.humanresource.compensation.salaryitem", "zh-HK", "薪资项目_hk", "菜单导航"),

            // menu.humanresource.compensation.payroll
            ("menu.humanresource.compensation.payroll", "en-US", "薪酬体系_us", "菜单导航"),
            // menu.humanresource.compensation.payroll
            ("menu.humanresource.compensation.payroll", "ja-JP", "薪酬体系_jp", "菜单导航"),
            // menu.humanresource.compensation.payroll
            ("menu.humanresource.compensation.payroll", "zh-CN", "薪酬体系", "菜单导航"),
            // menu.humanresource.compensation.payroll
            ("menu.humanresource.compensation.payroll", "zh-HK", "薪酬体系_hk", "菜单导航"),

            // menu.humanresource.compensation.payscale
            ("menu.humanresource.compensation.payscale", "en-US", "薪级_us", "菜单导航"),
            // menu.humanresource.compensation.payscale
            ("menu.humanresource.compensation.payscale", "ja-JP", "薪级_jp", "菜单导航"),
            // menu.humanresource.compensation.payscale
            ("menu.humanresource.compensation.payscale", "zh-CN", "薪级", "菜单导航"),
            // menu.humanresource.compensation.payscale
            ("menu.humanresource.compensation.payscale", "zh-HK", "薪级_hk", "菜单导航"),

            // menu.humanresource.compensation.empsalary
            ("menu.humanresource.compensation.empsalary", "en-US", "员工定薪_us", "菜单导航"),
            // menu.humanresource.compensation.empsalary
            ("menu.humanresource.compensation.empsalary", "ja-JP", "员工定薪_jp", "菜单导航"),
            // menu.humanresource.compensation.empsalary
            ("menu.humanresource.compensation.empsalary", "zh-CN", "员工定薪", "菜单导航"),
            // menu.humanresource.compensation.empsalary
            ("menu.humanresource.compensation.empsalary", "zh-HK", "员工定薪_hk", "菜单导航"),

            // menu.humanresource.compensation.bonusplan
            ("menu.humanresource.compensation.bonusplan", "en-US", "奖金方案_us", "菜单导航"),
            // menu.humanresource.compensation.bonusplan
            ("menu.humanresource.compensation.bonusplan", "ja-JP", "奖金方案_jp", "菜单导航"),
            // menu.humanresource.compensation.bonusplan
            ("menu.humanresource.compensation.bonusplan", "zh-CN", "奖金方案", "菜单导航"),
            // menu.humanresource.compensation.bonusplan
            ("menu.humanresource.compensation.bonusplan", "zh-HK", "奖金方案_hk", "菜单导航"),

            // menu.humanresource.compensation.salaryformula
            ("menu.humanresource.compensation.salaryformula", "en-US", "薪资计算公式_us", "菜单导航"),
            // menu.humanresource.compensation.salaryformula
            ("menu.humanresource.compensation.salaryformula", "ja-JP", "薪资计算公式_jp", "菜单导航"),
            // menu.humanresource.compensation.salaryformula
            ("menu.humanresource.compensation.salaryformula", "zh-CN", "薪资计算公式", "菜单导航"),
            // menu.humanresource.compensation.salaryformula
            ("menu.humanresource.compensation.salaryformula", "zh-HK", "薪资计算公式_hk", "菜单导航"),

            // menu.humanresource.compensation.payslip
            ("menu.humanresource.compensation.payslip", "en-US", "工资条_us", "菜单导航"),
            // menu.humanresource.compensation.payslip
            ("menu.humanresource.compensation.payslip", "ja-JP", "工资条_jp", "菜单导航"),
            // menu.humanresource.compensation.payslip
            ("menu.humanresource.compensation.payslip", "zh-CN", "工资条", "菜单导航"),
            // menu.humanresource.compensation.payslip
            ("menu.humanresource.compensation.payslip", "zh-HK", "工资条_hk", "菜单导航"),

            // menu.humanresource.benefits.benefititem
            ("menu.humanresource.benefits.benefititem", "en-US", "福利项目_us", "菜单导航"),
            // menu.humanresource.benefits.benefititem
            ("menu.humanresource.benefits.benefititem", "ja-JP", "福利项目_jp", "菜单导航"),
            // menu.humanresource.benefits.benefititem
            ("menu.humanresource.benefits.benefititem", "zh-CN", "福利项目", "菜单导航"),
            // menu.humanresource.benefits.benefititem
            ("menu.humanresource.benefits.benefititem", "zh-HK", "福利项目_hk", "菜单导航"),

            // menu.humanresource.benefits.empbenefitplan
            ("menu.humanresource.benefits.empbenefitplan", "en-US", "员工福利方案_us", "菜单导航"),
            // menu.humanresource.benefits.empbenefitplan
            ("menu.humanresource.benefits.empbenefitplan", "ja-JP", "员工福利方案_jp", "菜单导航"),
            // menu.humanresource.benefits.empbenefitplan
            ("menu.humanresource.benefits.empbenefitplan", "zh-CN", "员工福利方案", "菜单导航"),
            // menu.humanresource.benefits.empbenefitplan
            ("menu.humanresource.benefits.empbenefitplan", "zh-HK", "员工福利方案_hk", "菜单导航"),

            // menu.humanresource.benefits.socialinsurance
            ("menu.humanresource.benefits.socialinsurance", "en-US", "社保公积金_us", "菜单导航"),
            // menu.humanresource.benefits.socialinsurance
            ("menu.humanresource.benefits.socialinsurance", "ja-JP", "社保公积金_jp", "菜单导航"),
            // menu.humanresource.benefits.socialinsurance
            ("menu.humanresource.benefits.socialinsurance", "zh-CN", "社保公积金", "菜单导航"),
            // menu.humanresource.benefits.socialinsurance
            ("menu.humanresource.benefits.socialinsurance", "zh-HK", "社保公积金_hk", "菜单导航"),

            // menu.humanresource.performance.perfcycle
            ("menu.humanresource.performance.perfcycle", "en-US", "绩效周期_us", "菜单导航"),
            // menu.humanresource.performance.perfcycle
            ("menu.humanresource.performance.perfcycle", "ja-JP", "绩效周期_jp", "菜单导航"),
            // menu.humanresource.performance.perfcycle
            ("menu.humanresource.performance.perfcycle", "zh-CN", "绩效周期", "菜单导航"),
            // menu.humanresource.performance.perfcycle
            ("menu.humanresource.performance.perfcycle", "zh-HK", "绩效周期_hk", "菜单导航"),

            // menu.humanresource.performance.perfscheme
            ("menu.humanresource.performance.perfscheme", "en-US", "绩效方案_us", "菜单导航"),
            // menu.humanresource.performance.perfscheme
            ("menu.humanresource.performance.perfscheme", "ja-JP", "绩效方案_jp", "菜单导航"),
            // menu.humanresource.performance.perfscheme
            ("menu.humanresource.performance.perfscheme", "zh-CN", "绩效方案", "菜单导航"),
            // menu.humanresource.performance.perfscheme
            ("menu.humanresource.performance.perfscheme", "zh-HK", "绩效方案_hk", "菜单导航"),

            // menu.humanresource.performance.perfobjective
            ("menu.humanresource.performance.perfobjective", "en-US", "绩效目标_us", "菜单导航"),
            // menu.humanresource.performance.perfobjective
            ("menu.humanresource.performance.perfobjective", "ja-JP", "绩效目标_jp", "菜单导航"),
            // menu.humanresource.performance.perfobjective
            ("menu.humanresource.performance.perfobjective", "zh-CN", "绩效目标", "菜单导航"),
            // menu.humanresource.performance.perfobjective
            ("menu.humanresource.performance.perfobjective", "zh-HK", "绩效目标_hk", "菜单导航"),

            // menu.humanresource.performance.perfassessment
            ("menu.humanresource.performance.perfassessment", "en-US", "绩效考核_us", "菜单导航"),
            // menu.humanresource.performance.perfassessment
            ("menu.humanresource.performance.perfassessment", "ja-JP", "绩效考核_jp", "菜单导航"),
            // menu.humanresource.performance.perfassessment
            ("menu.humanresource.performance.perfassessment", "zh-CN", "绩效考核", "菜单导航"),
            // menu.humanresource.performance.perfassessment
            ("menu.humanresource.performance.perfassessment", "zh-HK", "绩效考核_hk", "菜单导航"),

            // menu.humanresource.performance.perfanalysis
            ("menu.humanresource.performance.perfanalysis", "en-US", "分析改进_us", "菜单导航"),
            // menu.humanresource.performance.perfanalysis
            ("menu.humanresource.performance.perfanalysis", "ja-JP", "分析改进_jp", "菜单导航"),
            // menu.humanresource.performance.perfanalysis
            ("menu.humanresource.performance.perfanalysis", "zh-CN", "分析改进", "菜单导航"),
            // menu.humanresource.performance.perfanalysis
            ("menu.humanresource.performance.perfanalysis", "zh-HK", "分析改进_hk", "菜单导航"),

            // menu.humanresource.training.course
            ("menu.humanresource.training.course", "en-US", "培训课程_us", "菜单导航"),
            // menu.humanresource.training.course
            ("menu.humanresource.training.course", "ja-JP", "培训课程_jp", "菜单导航"),
            // menu.humanresource.training.course
            ("menu.humanresource.training.course", "zh-CN", "培训课程", "菜单导航"),
            // menu.humanresource.training.course
            ("menu.humanresource.training.course", "zh-HK", "培训课程_hk", "菜单导航"),

            // menu.humanresource.training.plan
            ("menu.humanresource.training.plan", "en-US", "年度计划_us", "菜单导航"),
            // menu.humanresource.training.plan
            ("menu.humanresource.training.plan", "ja-JP", "年度计划_jp", "菜单导航"),
            // menu.humanresource.training.plan
            ("menu.humanresource.training.plan", "zh-CN", "年度计划", "菜单导航"),
            // menu.humanresource.training.plan
            ("menu.humanresource.training.plan", "zh-HK", "年度计划_hk", "菜单导航"),

            // menu.humanresource.training.attendee
            ("menu.humanresource.training.attendee", "en-US", "参训记录_us", "菜单导航"),
            // menu.humanresource.training.attendee
            ("menu.humanresource.training.attendee", "ja-JP", "参训记录_jp", "菜单导航"),
            // menu.humanresource.training.attendee
            ("menu.humanresource.training.attendee", "zh-CN", "参训记录", "菜单导航"),
            // menu.humanresource.training.attendee
            ("menu.humanresource.training.attendee", "zh-HK", "参训记录_hk", "菜单导航"),

            // menu.humanresource.talent.staffingrequirement
            ("menu.humanresource.talent.staffingrequirement", "en-US", "用人需求_us", "菜单导航"),
            // menu.humanresource.talent.staffingrequirement
            ("menu.humanresource.talent.staffingrequirement", "ja-JP", "用人需求_jp", "菜单导航"),
            // menu.humanresource.talent.staffingrequirement
            ("menu.humanresource.talent.staffingrequirement", "zh-CN", "用人需求", "菜单导航"),
            // menu.humanresource.talent.staffingrequirement
            ("menu.humanresource.talent.staffingrequirement", "zh-HK", "用人需求_hk", "菜单导航"),

            // menu.humanresource.talent.recruitmentplan
            ("menu.humanresource.talent.recruitmentplan", "en-US", "招聘计划_us", "菜单导航"),
            // menu.humanresource.talent.recruitmentplan
            ("menu.humanresource.talent.recruitmentplan", "ja-JP", "招聘计划_jp", "菜单导航"),
            // menu.humanresource.talent.recruitmentplan
            ("menu.humanresource.talent.recruitmentplan", "zh-CN", "招聘计划", "菜单导航"),
            // menu.humanresource.talent.recruitmentplan
            ("menu.humanresource.talent.recruitmentplan", "zh-HK", "招聘计划_hk", "菜单导航"),

            // menu.humanresource.talent.jobposting
            ("menu.humanresource.talent.jobposting", "en-US", "职位发布_us", "菜单导航"),
            // menu.humanresource.talent.jobposting
            ("menu.humanresource.talent.jobposting", "ja-JP", "职位发布_jp", "菜单导航"),
            // menu.humanresource.talent.jobposting
            ("menu.humanresource.talent.jobposting", "zh-CN", "职位发布", "菜单导航"),
            // menu.humanresource.talent.jobposting
            ("menu.humanresource.talent.jobposting", "zh-HK", "职位发布_hk", "菜单导航"),

            // menu.humanresource.talent.interview
            ("menu.humanresource.talent.interview", "en-US", "面试安排_us", "菜单导航"),
            // menu.humanresource.talent.interview
            ("menu.humanresource.talent.interview", "ja-JP", "面试安排_jp", "菜单导航"),
            // menu.humanresource.talent.interview
            ("menu.humanresource.talent.interview", "zh-CN", "面试安排", "菜单导航"),
            // menu.humanresource.talent.interview
            ("menu.humanresource.talent.interview", "zh-HK", "面试安排_hk", "菜单导航"),

            // menu.humanresource.talent.offer
            ("menu.humanresource.talent.offer", "en-US", "录用_us", "菜单导航"),
            // menu.humanresource.talent.offer
            ("menu.humanresource.talent.offer", "ja-JP", "录用_jp", "菜单导航"),
            // menu.humanresource.talent.offer
            ("menu.humanresource.talent.offer", "zh-CN", "录用", "菜单导航"),
            // menu.humanresource.talent.offer
            ("menu.humanresource.talent.offer", "zh-HK", "录用_hk", "菜单导航"),

            // menu.statistics.report.configurable
            ("menu.statistics.report.configurable", "en-US", "SQVI报表_us", "菜单导航"),
            // menu.statistics.report.configurable
            ("menu.statistics.report.configurable", "ja-JP", "SQVI报表_jp", "菜单导航"),
            // menu.statistics.report.configurable
            ("menu.statistics.report.configurable", "zh-CN", "SQVI报表", "菜单导航"),
            // menu.statistics.report.configurable
            ("menu.statistics.report.configurable", "zh-HK", "SQVI报表_hk", "菜单导航"),

            // menu.statistics.logging.loginlog
            ("menu.statistics.logging.loginlog", "en-US", "登录日志_us", "菜单导航"),
            // menu.statistics.logging.loginlog
            ("menu.statistics.logging.loginlog", "ja-JP", "登录日志_jp", "菜单导航"),
            // menu.statistics.logging.loginlog
            ("menu.statistics.logging.loginlog", "zh-CN", "登录日志", "菜单导航"),
            // menu.statistics.logging.loginlog
            ("menu.statistics.logging.loginlog", "zh-HK", "登录日志_hk", "菜单导航"),

            // menu.statistics.logging.operlog
            ("menu.statistics.logging.operlog", "en-US", "操作日志_us", "菜单导航"),
            // menu.statistics.logging.operlog
            ("menu.statistics.logging.operlog", "ja-JP", "操作日志_jp", "菜单导航"),
            // menu.statistics.logging.operlog
            ("menu.statistics.logging.operlog", "zh-CN", "操作日志", "菜单导航"),
            // menu.statistics.logging.operlog
            ("menu.statistics.logging.operlog", "zh-HK", "操作日志_hk", "菜单导航"),

            // menu.statistics.logging.deltalog
            ("menu.statistics.logging.deltalog", "en-US", "差异日志_us", "菜单导航"),
            // menu.statistics.logging.deltalog
            ("menu.statistics.logging.deltalog", "ja-JP", "差异日志_jp", "菜单导航"),
            // menu.statistics.logging.deltalog
            ("menu.statistics.logging.deltalog", "zh-CN", "差异日志", "菜单导航"),
            // menu.statistics.logging.deltalog
            ("menu.statistics.logging.deltalog", "zh-HK", "差异日志_hk", "菜单导航"),

            // menu.statistics.logging.quartzlog
            ("menu.statistics.logging.quartzlog", "en-US", "任务日志_us", "菜单导航"),
            // menu.statistics.logging.quartzlog
            ("menu.statistics.logging.quartzlog", "ja-JP", "任务日志_jp", "菜单导航"),
            // menu.statistics.logging.quartzlog
            ("menu.statistics.logging.quartzlog", "zh-CN", "任务日志", "菜单导航"),
            // menu.statistics.logging.quartzlog
            ("menu.statistics.logging.quartzlog", "zh-HK", "任务日志_hk", "菜单导航"),

            // menu.statistics.logging.servermonitor
            ("menu.statistics.logging.servermonitor", "en-US", "服务监控_us", "菜单导航"),
            // menu.statistics.logging.servermonitor
            ("menu.statistics.logging.servermonitor", "ja-JP", "服务监控_jp", "菜单导航"),
            // menu.statistics.logging.servermonitor
            ("menu.statistics.logging.servermonitor", "zh-CN", "服务监控", "菜单导航"),
            // menu.statistics.logging.servermonitor
            ("menu.statistics.logging.servermonitor", "zh-HK", "服务监控_hk", "菜单导航"),

            // menu.routine.helpdesk.myticket
            ("menu.routine.helpdesk.myticket", "en-US", "我的工单_us", "菜单导航"),
            // menu.routine.helpdesk.myticket
            ("menu.routine.helpdesk.myticket", "ja-JP", "我的工单_jp", "菜单导航"),
            // menu.routine.helpdesk.myticket
            ("menu.routine.helpdesk.myticket", "zh-CN", "我的工单", "菜单导航"),
            // menu.routine.helpdesk.myticket
            ("menu.routine.helpdesk.myticket", "zh-HK", "我的工单_hk", "菜单导航"),

            // menu.routine.helpdesk.ticket
            ("menu.routine.helpdesk.ticket", "en-US", "工单管理_us", "菜单导航"),
            // menu.routine.helpdesk.ticket
            ("menu.routine.helpdesk.ticket", "ja-JP", "工单管理_jp", "菜单导航"),
            // menu.routine.helpdesk.ticket
            ("menu.routine.helpdesk.ticket", "zh-CN", "工单管理", "菜单导航"),
            // menu.routine.helpdesk.ticket
            ("menu.routine.helpdesk.ticket", "zh-HK", "工单管理_hk", "菜单导航"),

            // menu.routine.helpdesk.ticket.changelog
            ("menu.routine.helpdesk.ticket.changelog", "en-US", "工单变更_us", "菜单导航"),
            // menu.routine.helpdesk.ticket.changelog
            ("menu.routine.helpdesk.ticket.changelog", "ja-JP", "工单变更_jp", "菜单导航"),
            // menu.routine.helpdesk.ticket.changelog
            ("menu.routine.helpdesk.ticket.changelog", "zh-CN", "工单变更", "菜单导航"),
            // menu.routine.helpdesk.ticket.changelog
            ("menu.routine.helpdesk.ticket.changelog", "zh-HK", "工单变更_hk", "菜单导航"),

            // menu.routine.helpdesk.knowledge
            ("menu.routine.helpdesk.knowledge", "en-US", "知识库（FAQ）_us", "菜单导航"),
            // menu.routine.helpdesk.knowledge
            ("menu.routine.helpdesk.knowledge", "ja-JP", "知识库（FAQ）_jp", "菜单导航"),
            // menu.routine.helpdesk.knowledge
            ("menu.routine.helpdesk.knowledge", "zh-CN", "知识库（FAQ）", "菜单导航"),
            // menu.routine.helpdesk.knowledge
            ("menu.routine.helpdesk.knowledge", "zh-HK", "知识库（FAQ）_hk", "菜单导航"),

            // menu.routine.helpdesk.knowledge.changelog
            ("menu.routine.helpdesk.knowledge.changelog", "en-US", "知识库变更_us", "菜单导航"),
            // menu.routine.helpdesk.knowledge.changelog
            ("menu.routine.helpdesk.knowledge.changelog", "ja-JP", "知识库变更_jp", "菜单导航"),
            // menu.routine.helpdesk.knowledge.changelog
            ("menu.routine.helpdesk.knowledge.changelog", "zh-CN", "知识库变更", "菜单导航"),
            // menu.routine.helpdesk.knowledge.changelog
            ("menu.routine.helpdesk.knowledge.changelog", "zh-HK", "知识库变更_hk", "菜单导航"),

            // menu.routine.helpdesk.myasset
            ("menu.routine.helpdesk.myasset", "en-US", "我的资产_us", "菜单导航"),
            // menu.routine.helpdesk.myasset
            ("menu.routine.helpdesk.myasset", "ja-JP", "我的资产_jp", "菜单导航"),
            // menu.routine.helpdesk.myasset
            ("menu.routine.helpdesk.myasset", "zh-CN", "我的资产", "菜单导航"),
            // menu.routine.helpdesk.myasset
            ("menu.routine.helpdesk.myasset", "zh-HK", "我的资产_hk", "菜单导航"),

            // menu.routine.helpdesk.itasset
            ("menu.routine.helpdesk.itasset", "en-US", "IT设备保修_us", "菜单导航"),
            // menu.routine.helpdesk.itasset
            ("menu.routine.helpdesk.itasset", "ja-JP", "IT设备保修_jp", "菜单导航"),
            // menu.routine.helpdesk.itasset
            ("menu.routine.helpdesk.itasset", "zh-CN", "IT设备保修", "菜单导航"),
            // menu.routine.helpdesk.itasset
            ("menu.routine.helpdesk.itasset", "zh-HK", "IT设备保修_hk", "菜单导航"),

            // menu.routine.helpdesk.itasset.changelog
            ("menu.routine.helpdesk.itasset.changelog", "en-US", "IT设备保修变更_us", "菜单导航"),
            // menu.routine.helpdesk.itasset.changelog
            ("menu.routine.helpdesk.itasset.changelog", "ja-JP", "IT设备保修变更_jp", "菜单导航"),
            // menu.routine.helpdesk.itasset.changelog
            ("menu.routine.helpdesk.itasset.changelog", "zh-CN", "IT设备保修变更", "菜单导航"),
            // menu.routine.helpdesk.itasset.changelog
            ("menu.routine.helpdesk.itasset.changelog", "zh-HK", "IT设备保修变更_hk", "菜单导航"),

            // menu.routine.documentcenter.document
            ("menu.routine.documentcenter.document", "en-US", "文档管理_us", "菜单导航"),
            // menu.routine.documentcenter.document
            ("menu.routine.documentcenter.document", "ja-JP", "文档管理_jp", "菜单导航"),
            // menu.routine.documentcenter.document
            ("menu.routine.documentcenter.document", "zh-CN", "文档管理", "菜单导航"),
            // menu.routine.documentcenter.document
            ("menu.routine.documentcenter.document", "zh-HK", "文档管理_hk", "菜单导航"),

            // menu.routine.documentcenter.document.changelog
            ("menu.routine.documentcenter.document.changelog", "en-US", "文档变更_us", "菜单导航"),
            // menu.routine.documentcenter.document.changelog
            ("menu.routine.documentcenter.document.changelog", "ja-JP", "文档变更_jp", "菜单导航"),
            // menu.routine.documentcenter.document.changelog
            ("menu.routine.documentcenter.document.changelog", "zh-CN", "文档变更", "菜单导航"),
            // menu.routine.documentcenter.document.changelog
            ("menu.routine.documentcenter.document.changelog", "zh-HK", "文档变更_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.billofmaterial
            ("menu.logistics.manufacturing.bom.billofmaterial", "en-US", "物料清单_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.billofmaterial
            ("menu.logistics.manufacturing.bom.billofmaterial", "ja-JP", "物料清单_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.billofmaterial
            ("menu.logistics.manufacturing.bom.billofmaterial", "zh-CN", "物料清单", "菜单导航"),
            // menu.logistics.manufacturing.bom.billofmaterial
            ("menu.logistics.manufacturing.bom.billofmaterial", "zh-HK", "物料清单_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.billofmaterial.changelog
            ("menu.logistics.manufacturing.bom.billofmaterial.changelog", "en-US", "物料清单变更_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.billofmaterial.changelog
            ("menu.logistics.manufacturing.bom.billofmaterial.changelog", "ja-JP", "物料清单变更_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.billofmaterial.changelog
            ("menu.logistics.manufacturing.bom.billofmaterial.changelog", "zh-CN", "物料清单变更", "菜单导航"),
            // menu.logistics.manufacturing.bom.billofmaterial.changelog
            ("menu.logistics.manufacturing.bom.billofmaterial.changelog", "zh-HK", "物料清单变更_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.routing
            ("menu.logistics.manufacturing.bom.routing", "en-US", "工艺路线_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.routing
            ("menu.logistics.manufacturing.bom.routing", "ja-JP", "工艺路线_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.routing
            ("menu.logistics.manufacturing.bom.routing", "zh-CN", "工艺路线", "菜单导航"),
            // menu.logistics.manufacturing.bom.routing
            ("menu.logistics.manufacturing.bom.routing", "zh-HK", "工艺路线_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.routing.changelog
            ("menu.logistics.manufacturing.bom.routing.changelog", "en-US", "工艺路线变更_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.routing.changelog
            ("menu.logistics.manufacturing.bom.routing.changelog", "ja-JP", "工艺路线变更_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.routing.changelog
            ("menu.logistics.manufacturing.bom.routing.changelog", "zh-CN", "工艺路线变更", "菜单导航"),
            // menu.logistics.manufacturing.bom.routing.changelog
            ("menu.logistics.manufacturing.bom.routing.changelog", "zh-HK", "工艺路线变更_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.standardoperationtime
            ("menu.logistics.manufacturing.bom.standardoperationtime", "en-US", "标准工序时间_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.standardoperationtime
            ("menu.logistics.manufacturing.bom.standardoperationtime", "ja-JP", "标准工序时间_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.standardoperationtime
            ("menu.logistics.manufacturing.bom.standardoperationtime", "zh-CN", "标准工序时间", "菜单导航"),
            // menu.logistics.manufacturing.bom.standardoperationtime
            ("menu.logistics.manufacturing.bom.standardoperationtime", "zh-HK", "标准工序时间_hk", "菜单导航"),

            // menu.logistics.manufacturing.bom.standardoperationtime.changelog
            ("menu.logistics.manufacturing.bom.standardoperationtime.changelog", "en-US", "标准工序时间变更_us", "菜单导航"),
            // menu.logistics.manufacturing.bom.standardoperationtime.changelog
            ("menu.logistics.manufacturing.bom.standardoperationtime.changelog", "ja-JP", "标准工序时间变更_jp", "菜单导航"),
            // menu.logistics.manufacturing.bom.standardoperationtime.changelog
            ("menu.logistics.manufacturing.bom.standardoperationtime.changelog", "zh-CN", "标准工序时间变更", "菜单导航"),
            // menu.logistics.manufacturing.bom.standardoperationtime.changelog
            ("menu.logistics.manufacturing.bom.standardoperationtime.changelog", "zh-HK", "标准工序时间变更_hk", "菜单导航"),

            // menu.logistics.manufacturing.planning.masterdemandschedule
            ("menu.logistics.manufacturing.planning.masterdemandschedule", "en-US", "主需求计划_us", "菜单导航"),
            // menu.logistics.manufacturing.planning.masterdemandschedule
            ("menu.logistics.manufacturing.planning.masterdemandschedule", "ja-JP", "主需求计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.planning.masterdemandschedule
            ("menu.logistics.manufacturing.planning.masterdemandschedule", "zh-CN", "主需求计划", "菜单导航"),
            // menu.logistics.manufacturing.planning.masterdemandschedule
            ("menu.logistics.manufacturing.planning.masterdemandschedule", "zh-HK", "主需求计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.planning.masterproductionschedule
            ("menu.logistics.manufacturing.planning.masterproductionschedule", "en-US", "主生产计划_us", "菜单导航"),
            // menu.logistics.manufacturing.planning.masterproductionschedule
            ("menu.logistics.manufacturing.planning.masterproductionschedule", "ja-JP", "主生产计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.planning.masterproductionschedule
            ("menu.logistics.manufacturing.planning.masterproductionschedule", "zh-CN", "主生产计划", "菜单导航"),
            // menu.logistics.manufacturing.planning.masterproductionschedule
            ("menu.logistics.manufacturing.planning.masterproductionschedule", "zh-HK", "主生产计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.planning.plannedorder
            ("menu.logistics.manufacturing.planning.plannedorder", "en-US", "计划订单_us", "菜单导航"),
            // menu.logistics.manufacturing.planning.plannedorder
            ("menu.logistics.manufacturing.planning.plannedorder", "ja-JP", "计划订单_jp", "菜单导航"),
            // menu.logistics.manufacturing.planning.plannedorder
            ("menu.logistics.manufacturing.planning.plannedorder", "zh-CN", "计划订单", "菜单导航"),
            // menu.logistics.manufacturing.planning.plannedorder
            ("menu.logistics.manufacturing.planning.plannedorder", "zh-HK", "计划订单_hk", "菜单导航"),

            // menu.logistics.manufacturing.planning.salesplan
            ("menu.logistics.manufacturing.planning.salesplan", "en-US", "销售计划_us", "菜单导航"),
            // menu.logistics.manufacturing.planning.salesplan
            ("menu.logistics.manufacturing.planning.salesplan", "ja-JP", "销售计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.planning.salesplan
            ("menu.logistics.manufacturing.planning.salesplan", "zh-CN", "销售计划", "菜单导航"),
            // menu.logistics.manufacturing.planning.salesplan
            ("menu.logistics.manufacturing.planning.salesplan", "zh-HK", "销售计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.planning.productionplan
            ("menu.logistics.manufacturing.planning.productionplan", "en-US", "生产计划_us", "菜单导航"),
            // menu.logistics.manufacturing.planning.productionplan
            ("menu.logistics.manufacturing.planning.productionplan", "ja-JP", "生产计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.planning.productionplan
            ("menu.logistics.manufacturing.planning.productionplan", "zh-CN", "生产计划", "菜单导航"),
            // menu.logistics.manufacturing.planning.productionplan
            ("menu.logistics.manufacturing.planning.productionplan", "zh-HK", "生产计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.planning.purchaseplan
            ("menu.logistics.manufacturing.planning.purchaseplan", "en-US", "采购计划_us", "菜单导航"),
            // menu.logistics.manufacturing.planning.purchaseplan
            ("menu.logistics.manufacturing.planning.purchaseplan", "ja-JP", "采购计划_jp", "菜单导航"),
            // menu.logistics.manufacturing.planning.purchaseplan
            ("menu.logistics.manufacturing.planning.purchaseplan", "zh-CN", "采购计划", "菜单导航"),
            // menu.logistics.manufacturing.planning.purchaseplan
            ("menu.logistics.manufacturing.planning.purchaseplan", "zh-HK", "采购计划_hk", "菜单导航"),

            // menu.logistics.manufacturing.scheduling.apsschedule
            ("menu.logistics.manufacturing.scheduling.apsschedule", "en-US", "APS排程_us", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.apsschedule
            ("menu.logistics.manufacturing.scheduling.apsschedule", "ja-JP", "APS排程_jp", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.apsschedule
            ("menu.logistics.manufacturing.scheduling.apsschedule", "zh-CN", "APS排程", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.apsschedule
            ("menu.logistics.manufacturing.scheduling.apsschedule", "zh-HK", "APS排程_hk", "菜单导航"),

            // menu.logistics.manufacturing.scheduling.apsschedule.changelog
            ("menu.logistics.manufacturing.scheduling.apsschedule.changelog", "en-US", "APS排程变更_us", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.apsschedule.changelog
            ("menu.logistics.manufacturing.scheduling.apsschedule.changelog", "ja-JP", "APS排程变更_jp", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.apsschedule.changelog
            ("menu.logistics.manufacturing.scheduling.apsschedule.changelog", "zh-CN", "APS排程变更", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.apsschedule.changelog
            ("menu.logistics.manufacturing.scheduling.apsschedule.changelog", "zh-HK", "APS排程变更_hk", "菜单导航"),

            // menu.logistics.manufacturing.scheduling.workcenter
            ("menu.logistics.manufacturing.scheduling.workcenter", "en-US", "工作中心_us", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.workcenter
            ("menu.logistics.manufacturing.scheduling.workcenter", "ja-JP", "工作中心_jp", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.workcenter
            ("menu.logistics.manufacturing.scheduling.workcenter", "zh-CN", "工作中心", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.workcenter
            ("menu.logistics.manufacturing.scheduling.workcenter", "zh-HK", "工作中心_hk", "菜单导航"),

            // menu.logistics.manufacturing.scheduling.changeovermatrix
            ("menu.logistics.manufacturing.scheduling.changeovermatrix", "en-US", "换型矩阵_us", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.changeovermatrix
            ("menu.logistics.manufacturing.scheduling.changeovermatrix", "ja-JP", "换型矩阵_jp", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.changeovermatrix
            ("menu.logistics.manufacturing.scheduling.changeovermatrix", "zh-CN", "换型矩阵", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.changeovermatrix
            ("menu.logistics.manufacturing.scheduling.changeovermatrix", "zh-HK", "换型矩阵_hk", "菜单导航"),

            // menu.logistics.manufacturing.scheduling.apsorder
            ("menu.logistics.manufacturing.scheduling.apsorder", "en-US", "APS订单_us", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.apsorder
            ("menu.logistics.manufacturing.scheduling.apsorder", "ja-JP", "APS订单_jp", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.apsorder
            ("menu.logistics.manufacturing.scheduling.apsorder", "zh-CN", "APS订单", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.apsorder
            ("menu.logistics.manufacturing.scheduling.apsorder", "zh-HK", "APS订单_hk", "菜单导航"),

            // menu.logistics.manufacturing.scheduling.productiondispatch
            ("menu.logistics.manufacturing.scheduling.productiondispatch", "en-US", "生产派工_us", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.productiondispatch
            ("menu.logistics.manufacturing.scheduling.productiondispatch", "ja-JP", "生产派工_jp", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.productiondispatch
            ("menu.logistics.manufacturing.scheduling.productiondispatch", "zh-CN", "生产派工", "菜单导航"),
            // menu.logistics.manufacturing.scheduling.productiondispatch
            ("menu.logistics.manufacturing.scheduling.productiondispatch", "zh-HK", "生产派工_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.kanban
            ("menu.logistics.manufacturing.engineeringchange.kanban", "en-US", "设变看板_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.kanban
            ("menu.logistics.manufacturing.engineeringchange.kanban", "ja-JP", "设变看板_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.kanban
            ("menu.logistics.manufacturing.engineeringchange.kanban", "zh-CN", "设变看板", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.kanban
            ("menu.logistics.manufacturing.engineeringchange.kanban", "zh-HK", "设变看板_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.batch
            ("menu.logistics.manufacturing.engineeringchange.batch", "en-US", "投入批次_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.batch
            ("menu.logistics.manufacturing.engineeringchange.batch", "ja-JP", "投入批次_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.batch
            ("menu.logistics.manufacturing.engineeringchange.batch", "zh-CN", "投入批次", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.batch
            ("menu.logistics.manufacturing.engineeringchange.batch", "zh-HK", "投入批次_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.kakunin
            ("menu.logistics.manufacturing.engineeringchange.kakunin", "en-US", "物料确认_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.kakunin
            ("menu.logistics.manufacturing.engineeringchange.kakunin", "ja-JP", "物料确认_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.kakunin
            ("menu.logistics.manufacturing.engineeringchange.kakunin", "zh-CN", "物料确认", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.kakunin
            ("menu.logistics.manufacturing.engineeringchange.kakunin", "zh-HK", "物料确认_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.ecnotification
            ("menu.logistics.manufacturing.engineeringchange.ecnotification", "en-US", "设变通知_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.ecnotification
            ("menu.logistics.manufacturing.engineeringchange.ecnotification", "ja-JP", "设变通知_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.ecnotification
            ("menu.logistics.manufacturing.engineeringchange.ecnotification", "zh-CN", "设变通知", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.ecnotification
            ("menu.logistics.manufacturing.engineeringchange.ecnotification", "zh-HK", "设变通知_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.gijutsu
            ("menu.logistics.manufacturing.engineeringchange.gijutsu", "en-US", "技术部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.gijutsu
            ("menu.logistics.manufacturing.engineeringchange.gijutsu", "ja-JP", "技术部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.gijutsu
            ("menu.logistics.manufacturing.engineeringchange.gijutsu", "zh-CN", "技术部门", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.gijutsu
            ("menu.logistics.manufacturing.engineeringchange.gijutsu", "zh-HK", "技术部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.koubai
            ("menu.logistics.manufacturing.engineeringchange.koubai", "en-US", "采购部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.koubai
            ("menu.logistics.manufacturing.engineeringchange.koubai", "ja-JP", "采购部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.koubai
            ("menu.logistics.manufacturing.engineeringchange.koubai", "zh-CN", "采购部门", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.koubai
            ("menu.logistics.manufacturing.engineeringchange.koubai", "zh-HK", "采购部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.seikan
            ("menu.logistics.manufacturing.engineeringchange.seikan", "en-US", "生管部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.seikan
            ("menu.logistics.manufacturing.engineeringchange.seikan", "ja-JP", "生管部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.seikan
            ("menu.logistics.manufacturing.engineeringchange.seikan", "zh-CN", "生管部门", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.seikan
            ("menu.logistics.manufacturing.engineeringchange.seikan", "zh-HK", "生管部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.ukeken
            ("menu.logistics.manufacturing.engineeringchange.ukeken", "en-US", "受检部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.ukeken
            ("menu.logistics.manufacturing.engineeringchange.ukeken", "ja-JP", "受检部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.ukeken
            ("menu.logistics.manufacturing.engineeringchange.ukeken", "zh-CN", "受检部门", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.ukeken
            ("menu.logistics.manufacturing.engineeringchange.ukeken", "zh-HK", "受检部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.bukan
            ("menu.logistics.manufacturing.engineeringchange.bukan", "en-US", "部管部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.bukan
            ("menu.logistics.manufacturing.engineeringchange.bukan", "ja-JP", "部管部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.bukan
            ("menu.logistics.manufacturing.engineeringchange.bukan", "zh-CN", "部管部门", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.bukan
            ("menu.logistics.manufacturing.engineeringchange.bukan", "zh-HK", "部管部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.seizounika
            ("menu.logistics.manufacturing.engineeringchange.seizounika", "en-US", "制造二课_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.seizounika
            ("menu.logistics.manufacturing.engineeringchange.seizounika", "ja-JP", "制造二课_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.seizounika
            ("menu.logistics.manufacturing.engineeringchange.seizounika", "zh-CN", "制造二课", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.seizounika
            ("menu.logistics.manufacturing.engineeringchange.seizounika", "zh-HK", "制造二课_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.seizouikka
            ("menu.logistics.manufacturing.engineeringchange.seizouikka", "en-US", "制造一课_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.seizouikka
            ("menu.logistics.manufacturing.engineeringchange.seizouikka", "ja-JP", "制造一课_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.seizouikka
            ("menu.logistics.manufacturing.engineeringchange.seizouikka", "zh-CN", "制造一课", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.seizouikka
            ("menu.logistics.manufacturing.engineeringchange.seizouikka", "zh-HK", "制造一课_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.hinkan
            ("menu.logistics.manufacturing.engineeringchange.hinkan", "en-US", "品管部门_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.hinkan
            ("menu.logistics.manufacturing.engineeringchange.hinkan", "ja-JP", "品管部门_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.hinkan
            ("menu.logistics.manufacturing.engineeringchange.hinkan", "zh-CN", "品管部门", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.hinkan
            ("menu.logistics.manufacturing.engineeringchange.hinkan", "zh-HK", "品管部门_hk", "菜单导航"),

            // menu.logistics.manufacturing.engineeringchange.legacyproduct
            ("menu.logistics.manufacturing.engineeringchange.legacyproduct", "en-US", "旧品管制_us", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.legacyproduct
            ("menu.logistics.manufacturing.engineeringchange.legacyproduct", "ja-JP", "旧品管制_jp", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.legacyproduct
            ("menu.logistics.manufacturing.engineeringchange.legacyproduct", "zh-CN", "旧品管制", "菜单导航"),
            // menu.logistics.manufacturing.engineeringchange.legacyproduct
            ("menu.logistics.manufacturing.engineeringchange.legacyproduct", "zh-HK", "旧品管制_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.productionorder
            ("menu.logistics.manufacturing.output.productionorder", "en-US", "生产工单_us", "菜单导航"),
            // menu.logistics.manufacturing.output.productionorder
            ("menu.logistics.manufacturing.output.productionorder", "ja-JP", "生产工单_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.productionorder
            ("menu.logistics.manufacturing.output.productionorder", "zh-CN", "生产工单", "菜单导航"),
            // menu.logistics.manufacturing.output.productionorder
            ("menu.logistics.manufacturing.output.productionorder", "zh-HK", "生产工单_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.productionorder.changelog
            ("menu.logistics.manufacturing.output.productionorder.changelog", "en-US", "生产工单变更_us", "菜单导航"),
            // menu.logistics.manufacturing.output.productionorder.changelog
            ("menu.logistics.manufacturing.output.productionorder.changelog", "ja-JP", "生产工单变更_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.productionorder.changelog
            ("menu.logistics.manufacturing.output.productionorder.changelog", "zh-CN", "生产工单变更", "菜单导航"),
            // menu.logistics.manufacturing.output.productionorder.changelog
            ("menu.logistics.manufacturing.output.productionorder.changelog", "zh-HK", "生产工单变更_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.pcba
            ("menu.logistics.manufacturing.output.pcba", "en-US", "PCBA日报_us", "菜单导航"),
            // menu.logistics.manufacturing.output.pcba
            ("menu.logistics.manufacturing.output.pcba", "ja-JP", "PCBA日报_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.pcba
            ("menu.logistics.manufacturing.output.pcba", "zh-CN", "PCBA日报", "菜单导航"),
            // menu.logistics.manufacturing.output.pcba
            ("menu.logistics.manufacturing.output.pcba", "zh-HK", "PCBA日报_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.assy
            ("menu.logistics.manufacturing.output.assy", "en-US", "组立日报_us", "菜单导航"),
            // menu.logistics.manufacturing.output.assy
            ("menu.logistics.manufacturing.output.assy", "ja-JP", "组立日报_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.assy
            ("menu.logistics.manufacturing.output.assy", "zh-CN", "组立日报", "菜单导航"),
            // menu.logistics.manufacturing.output.assy
            ("menu.logistics.manufacturing.output.assy", "zh-HK", "组立日报_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.productionchangeover
            ("menu.logistics.manufacturing.output.productionchangeover", "en-US", "生产切换_us", "菜单导航"),
            // menu.logistics.manufacturing.output.productionchangeover
            ("menu.logistics.manufacturing.output.productionchangeover", "ja-JP", "生产切换_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.productionchangeover
            ("menu.logistics.manufacturing.output.productionchangeover", "zh-CN", "生产切换", "菜单导航"),
            // menu.logistics.manufacturing.output.productionchangeover
            ("menu.logistics.manufacturing.output.productionchangeover", "zh-HK", "生产切换_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.equipmentoperationrate
            ("menu.logistics.manufacturing.output.equipmentoperationrate", "en-US", "机器稼动率_us", "菜单导航"),
            // menu.logistics.manufacturing.output.equipmentoperationrate
            ("menu.logistics.manufacturing.output.equipmentoperationrate", "ja-JP", "机器稼动率_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.equipmentoperationrate
            ("menu.logistics.manufacturing.output.equipmentoperationrate", "zh-CN", "机器稼动率", "菜单导航"),
            // menu.logistics.manufacturing.output.equipmentoperationrate
            ("menu.logistics.manufacturing.output.equipmentoperationrate", "zh-HK", "机器稼动率_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.equipmentoperationrate.changelog
            ("menu.logistics.manufacturing.output.equipmentoperationrate.changelog", "en-US", "机器稼动率变更_us", "菜单导航"),
            // menu.logistics.manufacturing.output.equipmentoperationrate.changelog
            ("menu.logistics.manufacturing.output.equipmentoperationrate.changelog", "ja-JP", "机器稼动率变更_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.equipmentoperationrate.changelog
            ("menu.logistics.manufacturing.output.equipmentoperationrate.changelog", "zh-CN", "机器稼动率变更", "菜单导航"),
            // menu.logistics.manufacturing.output.equipmentoperationrate.changelog
            ("menu.logistics.manufacturing.output.equipmentoperationrate.changelog", "zh-HK", "机器稼动率变更_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.personneloperationrate
            ("menu.logistics.manufacturing.output.personneloperationrate", "en-US", "人员稼动率_us", "菜单导航"),
            // menu.logistics.manufacturing.output.personneloperationrate
            ("menu.logistics.manufacturing.output.personneloperationrate", "ja-JP", "人员稼动率_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.personneloperationrate
            ("menu.logistics.manufacturing.output.personneloperationrate", "zh-CN", "人员稼动率", "菜单导航"),
            // menu.logistics.manufacturing.output.personneloperationrate
            ("menu.logistics.manufacturing.output.personneloperationrate", "zh-HK", "人员稼动率_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.personneloperationrate.changelog
            ("menu.logistics.manufacturing.output.personneloperationrate.changelog", "en-US", "人员稼动率变更_us", "菜单导航"),
            // menu.logistics.manufacturing.output.personneloperationrate.changelog
            ("menu.logistics.manufacturing.output.personneloperationrate.changelog", "ja-JP", "人员稼动率变更_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.personneloperationrate.changelog
            ("menu.logistics.manufacturing.output.personneloperationrate.changelog", "zh-CN", "人员稼动率变更", "菜单导航"),
            // menu.logistics.manufacturing.output.personneloperationrate.changelog
            ("menu.logistics.manufacturing.output.personneloperationrate.changelog", "zh-HK", "人员稼动率变更_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.productionteam
            ("menu.logistics.manufacturing.output.productionteam", "en-US", "生产班组_us", "菜单导航"),
            // menu.logistics.manufacturing.output.productionteam
            ("menu.logistics.manufacturing.output.productionteam", "ja-JP", "生产班组_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.productionteam
            ("menu.logistics.manufacturing.output.productionteam", "zh-CN", "生产班组", "菜单导航"),
            // menu.logistics.manufacturing.output.productionteam
            ("menu.logistics.manufacturing.output.productionteam", "zh-HK", "生产班组_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.standardoperationrate
            ("menu.logistics.manufacturing.output.standardoperationrate", "en-US", "标准生产稼动率_us", "菜单导航"),
            // menu.logistics.manufacturing.output.standardoperationrate
            ("menu.logistics.manufacturing.output.standardoperationrate", "ja-JP", "标准生产稼动率_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.standardoperationrate
            ("menu.logistics.manufacturing.output.standardoperationrate", "zh-CN", "标准生产稼动率", "菜单导航"),
            // menu.logistics.manufacturing.output.standardoperationrate
            ("menu.logistics.manufacturing.output.standardoperationrate", "zh-HK", "标准生产稼动率_hk", "菜单导航"),

            // menu.logistics.manufacturing.output.standardoperationrate.changelog
            ("menu.logistics.manufacturing.output.standardoperationrate.changelog", "en-US", "标准生产稼动率变更_us", "菜单导航"),
            // menu.logistics.manufacturing.output.standardoperationrate.changelog
            ("menu.logistics.manufacturing.output.standardoperationrate.changelog", "ja-JP", "标准生产稼动率变更_jp", "菜单导航"),
            // menu.logistics.manufacturing.output.standardoperationrate.changelog
            ("menu.logistics.manufacturing.output.standardoperationrate.changelog", "zh-CN", "标准生产稼动率变更", "菜单导航"),
            // menu.logistics.manufacturing.output.standardoperationrate.changelog
            ("menu.logistics.manufacturing.output.standardoperationrate.changelog", "zh-HK", "标准生产稼动率变更_hk", "菜单导航"),

            // menu.logistics.manufacturing.defect.pcbainspection
            ("menu.logistics.manufacturing.defect.pcbainspection", "en-US", "PCBA检查_us", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcbainspection
            ("menu.logistics.manufacturing.defect.pcbainspection", "ja-JP", "PCBA检查_jp", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcbainspection
            ("menu.logistics.manufacturing.defect.pcbainspection", "zh-CN", "PCBA检查", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcbainspection
            ("menu.logistics.manufacturing.defect.pcbainspection", "zh-HK", "PCBA检查_hk", "菜单导航"),

            // menu.logistics.manufacturing.defect.pcbarepair
            ("menu.logistics.manufacturing.defect.pcbarepair", "en-US", "PCBA改修_us", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcbarepair
            ("menu.logistics.manufacturing.defect.pcbarepair", "ja-JP", "PCBA改修_jp", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcbarepair
            ("menu.logistics.manufacturing.defect.pcbarepair", "zh-CN", "PCBA改修", "菜单导航"),
            // menu.logistics.manufacturing.defect.pcbarepair
            ("menu.logistics.manufacturing.defect.pcbarepair", "zh-HK", "PCBA改修_hk", "菜单导航"),

            // menu.logistics.manufacturing.defect.assy
            ("menu.logistics.manufacturing.defect.assy", "en-US", "组立不良_us", "菜单导航"),
            // menu.logistics.manufacturing.defect.assy
            ("menu.logistics.manufacturing.defect.assy", "ja-JP", "组立不良_jp", "菜单导航"),
            // menu.logistics.manufacturing.defect.assy
            ("menu.logistics.manufacturing.defect.assy", "zh-CN", "组立不良", "菜单导航"),
            // menu.logistics.manufacturing.defect.assy
            ("menu.logistics.manufacturing.defect.assy", "zh-HK", "组立不良_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.workstation
            ("menu.logistics.manufacturing.sop.workstation", "en-US", "工位管理_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.workstation
            ("menu.logistics.manufacturing.sop.workstation", "ja-JP", "工位管理_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.workstation
            ("menu.logistics.manufacturing.sop.workstation", "zh-CN", "工位管理", "菜单导航"),
            // menu.logistics.manufacturing.sop.workstation
            ("menu.logistics.manufacturing.sop.workstation", "zh-HK", "工位管理_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.doc
            ("menu.logistics.manufacturing.sop.doc", "en-US", "SOP文档_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.doc
            ("menu.logistics.manufacturing.sop.doc", "ja-JP", "SOP文档_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.doc
            ("menu.logistics.manufacturing.sop.doc", "zh-CN", "SOP文档", "菜单导航"),
            // menu.logistics.manufacturing.sop.doc
            ("menu.logistics.manufacturing.sop.doc", "zh-HK", "SOP文档_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.revision
            ("menu.logistics.manufacturing.sop.revision", "en-US", "SOP版本_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.revision
            ("menu.logistics.manufacturing.sop.revision", "ja-JP", "SOP版本_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.revision
            ("menu.logistics.manufacturing.sop.revision", "zh-CN", "SOP版本", "菜单导航"),
            // menu.logistics.manufacturing.sop.revision
            ("menu.logistics.manufacturing.sop.revision", "zh-HK", "SOP版本_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.ack
            ("menu.logistics.manufacturing.sop.ack", "en-US", "版本确认_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.ack
            ("menu.logistics.manufacturing.sop.ack", "ja-JP", "版本确认_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.ack
            ("menu.logistics.manufacturing.sop.ack", "zh-CN", "版本确认", "菜单导航"),
            // menu.logistics.manufacturing.sop.ack
            ("menu.logistics.manufacturing.sop.ack", "zh-HK", "版本确认_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.exec
            ("menu.logistics.manufacturing.sop.exec", "en-US", "工位执行_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.exec
            ("menu.logistics.manufacturing.sop.exec", "ja-JP", "工位执行_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.exec
            ("menu.logistics.manufacturing.sop.exec", "zh-CN", "工位执行", "菜单导航"),
            // menu.logistics.manufacturing.sop.exec
            ("menu.logistics.manufacturing.sop.exec", "zh-HK", "工位执行_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.execscan
            ("menu.logistics.manufacturing.sop.execscan", "en-US", "扫码记录_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.execscan
            ("menu.logistics.manufacturing.sop.execscan", "ja-JP", "扫码记录_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.execscan
            ("menu.logistics.manufacturing.sop.execscan", "zh-CN", "扫码记录", "菜单导航"),
            // menu.logistics.manufacturing.sop.execscan
            ("menu.logistics.manufacturing.sop.execscan", "zh-HK", "扫码记录_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.esdcheck
            ("menu.logistics.manufacturing.sop.esdcheck", "en-US", "ESD检查_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.esdcheck
            ("menu.logistics.manufacturing.sop.esdcheck", "ja-JP", "ESD检查_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.esdcheck
            ("menu.logistics.manufacturing.sop.esdcheck", "zh-CN", "ESD检查", "菜单导航"),
            // menu.logistics.manufacturing.sop.esdcheck
            ("menu.logistics.manufacturing.sop.esdcheck", "zh-HK", "ESD检查_hk", "菜单导航"),

            // menu.logistics.manufacturing.sop.call
            ("menu.logistics.manufacturing.sop.call", "en-US", "安灯呼叫_us", "菜单导航"),
            // menu.logistics.manufacturing.sop.call
            ("menu.logistics.manufacturing.sop.call", "ja-JP", "安灯呼叫_jp", "菜单导航"),
            // menu.logistics.manufacturing.sop.call
            ("menu.logistics.manufacturing.sop.call", "zh-CN", "安灯呼叫", "菜单导航"),
            // menu.logistics.manufacturing.sop.call
            ("menu.logistics.manufacturing.sop.call", "zh-HK", "安灯呼叫_hk", "菜单导航"),

            // menu.logistics.quality.cost.assurance
            ("menu.logistics.quality.cost.assurance", "en-US", "品质保证_us", "菜单导航"),
            // menu.logistics.quality.cost.assurance
            ("menu.logistics.quality.cost.assurance", "ja-JP", "品质保证_jp", "菜单导航"),
            // menu.logistics.quality.cost.assurance
            ("menu.logistics.quality.cost.assurance", "zh-CN", "品质保证", "菜单导航"),
            // menu.logistics.quality.cost.assurance
            ("menu.logistics.quality.cost.assurance", "zh-HK", "品质保证_hk", "菜单导航"),

            // menu.logistics.quality.cost.issue
            ("menu.logistics.quality.cost.issue", "en-US", "品质问题_us", "菜单导航"),
            // menu.logistics.quality.cost.issue
            ("menu.logistics.quality.cost.issue", "ja-JP", "品质问题_jp", "菜单导航"),
            // menu.logistics.quality.cost.issue
            ("menu.logistics.quality.cost.issue", "zh-CN", "品质问题", "菜单导航"),
            // menu.logistics.quality.cost.issue
            ("menu.logistics.quality.cost.issue", "zh-HK", "品质问题_hk", "菜单导航"),

            // menu.logistics.quality.cost.incident
            ("menu.logistics.quality.cost.incident", "en-US", "品质事故_us", "菜单导航"),
            // menu.logistics.quality.cost.incident
            ("menu.logistics.quality.cost.incident", "ja-JP", "品质事故_jp", "菜单导航"),
            // menu.logistics.quality.cost.incident
            ("menu.logistics.quality.cost.incident", "zh-CN", "品质事故", "菜单导航"),
            // menu.logistics.quality.cost.incident
            ("menu.logistics.quality.cost.incident", "zh-HK", "品质事故_hk", "菜单导航"),

            // menu.logistics.quality.operation.samplingscheme
            ("menu.logistics.quality.operation.samplingscheme", "en-US", "抽样方案_us", "菜单导航"),
            // menu.logistics.quality.operation.samplingscheme
            ("menu.logistics.quality.operation.samplingscheme", "ja-JP", "抽样方案_jp", "菜单导航"),
            // menu.logistics.quality.operation.samplingscheme
            ("menu.logistics.quality.operation.samplingscheme", "zh-CN", "抽样方案", "菜单导航"),
            // menu.logistics.quality.operation.samplingscheme
            ("menu.logistics.quality.operation.samplingscheme", "zh-HK", "抽样方案_hk", "菜单导航"),

            // menu.logistics.quality.operation.inspectionstandard
            ("menu.logistics.quality.operation.inspectionstandard", "en-US", "检验标准_us", "菜单导航"),
            // menu.logistics.quality.operation.inspectionstandard
            ("menu.logistics.quality.operation.inspectionstandard", "ja-JP", "检验标准_jp", "菜单导航"),
            // menu.logistics.quality.operation.inspectionstandard
            ("menu.logistics.quality.operation.inspectionstandard", "zh-CN", "检验标准", "菜单导航"),
            // menu.logistics.quality.operation.inspectionstandard
            ("menu.logistics.quality.operation.inspectionstandard", "zh-HK", "检验标准_hk", "菜单导航"),

            // menu.logistics.quality.operation.iqcorder
            ("menu.logistics.quality.operation.iqcorder", "en-US", "进货检验_us", "菜单导航"),
            // menu.logistics.quality.operation.iqcorder
            ("menu.logistics.quality.operation.iqcorder", "ja-JP", "进货检验_jp", "菜单导航"),
            // menu.logistics.quality.operation.iqcorder
            ("menu.logistics.quality.operation.iqcorder", "zh-CN", "进货检验", "菜单导航"),
            // menu.logistics.quality.operation.iqcorder
            ("menu.logistics.quality.operation.iqcorder", "zh-HK", "进货检验_hk", "菜单导航"),

            // menu.logistics.quality.operation.iqcorder.changelog
            ("menu.logistics.quality.operation.iqcorder.changelog", "en-US", "进货检验变更_us", "菜单导航"),
            // menu.logistics.quality.operation.iqcorder.changelog
            ("menu.logistics.quality.operation.iqcorder.changelog", "ja-JP", "进货检验变更_jp", "菜单导航"),
            // menu.logistics.quality.operation.iqcorder.changelog
            ("menu.logistics.quality.operation.iqcorder.changelog", "zh-CN", "进货检验变更", "菜单导航"),
            // menu.logistics.quality.operation.iqcorder.changelog
            ("menu.logistics.quality.operation.iqcorder.changelog", "zh-HK", "进货检验变更_hk", "菜单导航"),

            // menu.logistics.quality.operation.ipqcorder
            ("menu.logistics.quality.operation.ipqcorder", "en-US", "制程检验_us", "菜单导航"),
            // menu.logistics.quality.operation.ipqcorder
            ("menu.logistics.quality.operation.ipqcorder", "ja-JP", "制程检验_jp", "菜单导航"),
            // menu.logistics.quality.operation.ipqcorder
            ("menu.logistics.quality.operation.ipqcorder", "zh-CN", "制程检验", "菜单导航"),
            // menu.logistics.quality.operation.ipqcorder
            ("menu.logistics.quality.operation.ipqcorder", "zh-HK", "制程检验_hk", "菜单导航"),

            // menu.logistics.quality.operation.ipqcorder.changelog
            ("menu.logistics.quality.operation.ipqcorder.changelog", "en-US", "制程检验变更_us", "菜单导航"),
            // menu.logistics.quality.operation.ipqcorder.changelog
            ("menu.logistics.quality.operation.ipqcorder.changelog", "ja-JP", "制程检验变更_jp", "菜单导航"),
            // menu.logistics.quality.operation.ipqcorder.changelog
            ("menu.logistics.quality.operation.ipqcorder.changelog", "zh-CN", "制程检验变更", "菜单导航"),
            // menu.logistics.quality.operation.ipqcorder.changelog
            ("menu.logistics.quality.operation.ipqcorder.changelog", "zh-HK", "制程检验变更_hk", "菜单导航"),

            // menu.logistics.quality.operation.fqcorder
            ("menu.logistics.quality.operation.fqcorder", "en-US", "入库检验_us", "菜单导航"),
            // menu.logistics.quality.operation.fqcorder
            ("menu.logistics.quality.operation.fqcorder", "ja-JP", "入库检验_jp", "菜单导航"),
            // menu.logistics.quality.operation.fqcorder
            ("menu.logistics.quality.operation.fqcorder", "zh-CN", "入库检验", "菜单导航"),
            // menu.logistics.quality.operation.fqcorder
            ("menu.logistics.quality.operation.fqcorder", "zh-HK", "入库检验_hk", "菜单导航"),

            // menu.logistics.quality.operation.fqcorder.changelog
            ("menu.logistics.quality.operation.fqcorder.changelog", "en-US", "入库检验变更_us", "菜单导航"),
            // menu.logistics.quality.operation.fqcorder.changelog
            ("menu.logistics.quality.operation.fqcorder.changelog", "ja-JP", "入库检验变更_jp", "菜单导航"),
            // menu.logistics.quality.operation.fqcorder.changelog
            ("menu.logistics.quality.operation.fqcorder.changelog", "zh-CN", "入库检验变更", "菜单导航"),
            // menu.logistics.quality.operation.fqcorder.changelog
            ("menu.logistics.quality.operation.fqcorder.changelog", "zh-HK", "入库检验变更_hk", "菜单导航"),

            // menu.logistics.quality.complaint.customer
            ("menu.logistics.quality.complaint.customer", "en-US", "客诉登记_us", "菜单导航"),
            // menu.logistics.quality.complaint.customer
            ("menu.logistics.quality.complaint.customer", "ja-JP", "客诉登记_jp", "菜单导航"),
            // menu.logistics.quality.complaint.customer
            ("menu.logistics.quality.complaint.customer", "zh-CN", "客诉登记", "菜单导航"),
            // menu.logistics.quality.complaint.customer
            ("menu.logistics.quality.complaint.customer", "zh-HK", "客诉登记_hk", "菜单导航"),

            // menu.logistics.quality.complaint.customercomplainthandling
            ("menu.logistics.quality.complaint.customercomplainthandling", "en-US", "客诉处理_us", "菜单导航"),
            // menu.logistics.quality.complaint.customercomplainthandling
            ("menu.logistics.quality.complaint.customercomplainthandling", "ja-JP", "客诉处理_jp", "菜单导航"),
            // menu.logistics.quality.complaint.customercomplainthandling
            ("menu.logistics.quality.complaint.customercomplainthandling", "zh-CN", "客诉处理", "菜单导航"),
            // menu.logistics.quality.complaint.customercomplainthandling
            ("menu.logistics.quality.complaint.customercomplainthandling", "zh-HK", "客诉处理_hk", "菜单导航"),

            // menu.logistics.quality.complaint.customersatisfactionsurvey
            ("menu.logistics.quality.complaint.customersatisfactionsurvey", "en-US", "客户满意度调查_us", "菜单导航"),
            // menu.logistics.quality.complaint.customersatisfactionsurvey
            ("menu.logistics.quality.complaint.customersatisfactionsurvey", "ja-JP", "客户满意度调查_jp", "菜单导航"),
            // menu.logistics.quality.complaint.customersatisfactionsurvey
            ("menu.logistics.quality.complaint.customersatisfactionsurvey", "zh-CN", "客户满意度调查", "菜单导航"),
            // menu.logistics.quality.complaint.customersatisfactionsurvey
            ("menu.logistics.quality.complaint.customersatisfactionsurvey", "zh-HK", "客户满意度调查_hk", "菜单导航"),

            // menu.logistics.quality.complaint.supplierevaluation
            ("menu.logistics.quality.complaint.supplierevaluation", "en-US", "供应商评价考核_us", "菜单导航"),
            // menu.logistics.quality.complaint.supplierevaluation
            ("menu.logistics.quality.complaint.supplierevaluation", "ja-JP", "供应商评价考核_jp", "菜单导航"),
            // menu.logistics.quality.complaint.supplierevaluation
            ("menu.logistics.quality.complaint.supplierevaluation", "zh-CN", "供应商评价考核", "菜单导航"),
            // menu.logistics.quality.complaint.supplierevaluation
            ("menu.logistics.quality.complaint.supplierevaluation", "zh-HK", "供应商评价考核_hk", "菜单导航"),
        };
    }

    /// <summary>填充 TaktTranslation 全部业务字段（含租户基类字段）</summary>
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
        translation.ResourceGroup = "Foundation";
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

    /// <summary>翻译种子项（CultureId 由 SeedAsync 解析）</summary>
    private sealed record TranslationSeedItem(
        string I18nKey,
        string CultureCode,
        string TranslationText,
        string? ContextNote);
}
