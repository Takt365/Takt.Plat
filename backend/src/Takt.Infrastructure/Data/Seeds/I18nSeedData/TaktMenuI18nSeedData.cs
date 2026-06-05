// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData
// 文件名称：TaktMenuI18nSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：菜单国际化翻译种子数据初始化（英、日、中、港繁四语）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData;

/// <summary>
/// 菜单国际化翻译种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// </summary>
public class TaktMenuI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在字典数据之后，RBAC关联之前）
    /// </summary>
    public int Order => 47;

    /// <summary>
    /// 初始化菜单国际化翻译种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化菜单国际化翻译种子数据...");

        // 参数验证
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过菜单国际化翻译种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化菜单国际化翻译数据...", tenantCode);

        foreach (var row in GetStandardMenuTranslations())
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

        TaktLogger.Information("菜单国际化翻译种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取标准菜单翻译列表
    /// 包含英文、日文、简体中文、香港繁体四种语言的菜单翻译
    /// </summary>
    private static List<(string I18nKey, string CultureCode, string TranslationText, string? ContextNote)> GetStandardMenuTranslations()
    {
        return new List<(string, string, string, string?)>
        {
            // ========================================
            // 一级菜单（顶级目录）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.home._self", "zh-CN", "主页", "一级菜单"),
            ("menu.dashboard._self", "zh-CN", "仪表盘", "一级菜单"),
            ("menu.routine._self", "zh-CN", "日常事务", "一级菜单"),
            ("menu.accounting._self", "zh-CN", "财务核算", "一级菜单"),
            ("menu.logistics._self", "zh-CN", "后勤管理", "一级菜单"),
            ("menu.humanresource._self", "zh-CN", "人力资源", "一级菜单"),
            ("menu.identity._self", "zh-CN", "身份认证", "一级菜单"),
            ("menu.workflow._self", "zh-CN", "工作流", "一级菜单"),
            ("menu.code._self", "zh-CN", "代码管理", "一级菜单"),
            ("menu.foundation._self", "zh-CN", "基础设置", "一级菜单"),
            ("menu.statistics._self", "zh-CN", "统计看板", "一级菜单"),
            ("menu.about._self", "zh-CN", "关于", "一级菜单"),

            // 英文 (en-US)
            ("menu.home._self", "en-US", "Home", "Level 1 Menu"),
            ("menu.dashboard._self", "en-US", "Dashboard", "Level 1 Menu"),
            ("menu.routine._self", "en-US", "Routine", "Level 1 Menu"),
            ("menu.accounting._self", "en-US", "Accounting", "Level 1 Menu"),
            ("menu.logistics._self", "en-US", "Logistics", "Level 1 Menu"),
            ("menu.humanresource._self", "en-US", "Human Resource", "Level 1 Menu"),
            ("menu.identity._self", "en-US", "Identity", "Level 1 Menu"),
            ("menu.workflow._self", "en-US", "Workflow", "Level 1 Menu"),
            ("menu.code._self", "en-US", "Code", "Level 1 Menu"),
            ("menu.foundation._self", "en-US", "Foundation", "Level 1 Menu"),
            ("menu.statistics._self", "en-US", "Statistics", "Level 1 Menu"),
            ("menu.about._self", "en-US", "About", "Level 1 Menu"),

            // 日文 (ja-JP)
            ("menu.home._self", "ja-JP", "ホーム", "レベル1メニュー"),
            ("menu.dashboard._self", "ja-JP", "ダッシュボード", "レベル1メニュー"),
            ("menu.routine._self", "ja-JP", "日常業務", "レベル1メニュー"),
            ("menu.accounting._self", "ja-JP", "財務会計", "レベル1メニュー"),
            ("menu.logistics._self", "ja-JP", "后勤管理", "レベル1メニュー"),
            ("menu.humanresource._self", "ja-JP", "人事管理", "レベル1メニュー"),
            ("menu.identity._self", "ja-JP", "認証", "レベル1メニュー"),
            ("menu.workflow._self", "ja-JP", "ワークフロー", "レベル1メニュー"),
            ("menu.code._self", "ja-JP", "コード", "レベル1メニュー"),
            ("menu.foundation._self", "ja-JP", "基本設定", "レベル1メニュー"),
            ("menu.statistics._self", "ja-JP", "統計", "レベル1メニュー"),
            ("menu.about._self", "ja-JP", "について", "レベル1メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.home._self", "zh-HK", "主頁", "一级菜单"),
            ("menu.dashboard._self", "zh-HK", "儀表盤", "一级菜单"),
            ("menu.routine._self", "zh-HK", "日常事務", "一级菜单"),
            ("menu.accounting._self", "zh-HK", "財務核算", "一级菜单"),
            ("menu.logistics._self", "zh-HK", "後勤管理", "一级菜单"),
            ("menu.humanresource._self", "zh-HK", "人力資源", "一级菜单"),
            ("menu.identity._self", "zh-HK", "身份認證", "一级菜单"),
            ("menu.workflow._self", "zh-HK", "工作流", "一级菜单"),
            ("menu.code._self", "zh-HK", "代碼管理", "一级菜单"),
            ("menu.foundation._self", "zh-HK", "基礎設置", "一级菜单"),
            ("menu.statistics._self", "zh-HK", "統計看板", "一级菜单"),
            ("menu.about._self", "zh-HK", "關於", "一级菜单"),

            // ========================================
            // 二级菜单（仪表盘）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.dashboard.workspace", "zh-CN", "工作台", "二级菜单"),
            ("menu.dashboard.databoard", "zh-CN", "数据看板", "二级菜单"),

            // 英文 (en-US)
            ("menu.dashboard.workspace", "en-US", "Workspace", "Level 2 Menu"),
            ("menu.dashboard.databoard", "en-US", "Data Board", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.dashboard.workspace", "ja-JP", "ワークスペース", "レベル2メニュー"),
            ("menu.dashboard.databoard", "ja-JP", "データボード", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.dashboard.workspace", "zh-HK", "工作台", "二级菜单"),
            ("menu.dashboard.databoard", "zh-HK", "數據看板", "二级菜单"),

            // ========================================
            // 二级菜单（日常事务）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.routine.announcement", "zh-CN", "公告通知", "二级菜单"),
            ("menu.routine.conferencecenter", "zh-CN", "会议中心", "二级菜单"),
            ("menu.routine.documentcenter", "zh-CN", "文管中心", "二级菜单"),
            ("menu.routine.newscenter", "zh-CN", "新闻中心", "二级菜单"),
            ("menu.routine.helpdesk", "zh-CN", "服务台", "二级菜单"),
            ("menu.routine.visitorcenter", "zh-CN", "访客中心", "二级菜单"),

            // 英文 (en-US)
            ("menu.routine.announcement", "en-US", "Announcement", "Level 2 Menu"),
            ("menu.routine.conferencecenter", "en-US", "Conference Center", "Level 2 Menu"),
            ("menu.routine.documentcenter", "en-US", "Document Center", "Level 2 Menu"),
            ("menu.routine.newscenter", "en-US", "News Center", "Level 2 Menu"),
            ("menu.routine.helpdesk", "en-US", "Help Desk", "Level 2 Menu"),
            ("menu.routine.visitorcenter", "en-US", "Visitor Center", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.routine.announcement", "ja-JP", "お知らせ", "レベル2メニュー"),
            ("menu.routine.conferencecenter", "ja-JP", "会議センター", "レベル2メニュー"),
            ("menu.routine.documentcenter", "ja-JP", "文書管理センター", "レベル2メニュー"),
            ("menu.routine.newscenter", "ja-JP", "ニュースセンター", "レベル2メニュー"),
            ("menu.routine.helpdesk", "ja-JP", "ヘルプデスク", "レベル2メニュー"),
            ("menu.routine.visitorcenter", "ja-JP", "来訪者センター", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.routine.announcement", "zh-HK", "公告通知", "二级菜单"),
            ("menu.routine.conferencecenter", "zh-HK", "會議中心", "二级菜单"),
            ("menu.routine.documentcenter", "zh-HK", "文管中心", "二级菜单"),
            ("menu.routine.newscenter", "zh-HK", "新聞中心", "二级菜单"),
            ("menu.routine.helpdesk", "zh-HK", "服務枱", "二级菜单"),
            ("menu.routine.visitorcenter", "zh-HK", "訪客中心", "二级菜单"),

            // ========================================
            // 二级菜单（财务核算）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.accounting.financial._self", "zh-CN", "管理会计", "二级菜单"),
            ("menu.accounting.controlling._self", "zh-CN", "控制会计", "二级菜单"),

            // 英文 (en-US)
            ("menu.accounting.financial._self", "en-US", "Financial Accounting", "Level 2 Menu"),
            ("menu.accounting.controlling._self", "en-US", "Controlling", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.accounting.financial._self", "ja-JP", "管理会計", "レベル2メニュー"),
            ("menu.accounting.controlling._self", "ja-JP", "管理会計", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.accounting.financial._self", "zh-HK", "管理會計", "二级菜单"),
            ("menu.accounting.controlling._self", "zh-HK", "控制會計", "二级菜单"),

            // ========================================
            // 二级菜单（后勤管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.sales._self", "zh-CN", "销售管理", "二级菜单"),
            ("menu.logistics.materials._self", "zh-CN", "物料管理", "二级菜单"),
            ("menu.logistics.manufacturing._self", "zh-CN", "生产执行", "二级菜单"),
            ("menu.logistics.quality._self", "zh-CN", "质量管理", "二级菜单"),
            ("menu.logistics.service._self", "zh-CN", "客户服务", "二级菜单"),
            ("menu.logistics.maintenance._self", "zh-CN", "工厂维护", "二级菜单"),

            // 英文 (en-US)
            ("menu.logistics.sales._self", "en-US", "Sales", "Level 2 Menu"),
            ("menu.logistics.materials._self", "en-US", "Materials", "Level 2 Menu"),
            ("menu.logistics.manufacturing._self", "en-US", "Manufacturing", "Level 2 Menu"),
            ("menu.logistics.quality._self", "en-US", "Quality", "Level 2 Menu"),
            ("menu.logistics.service._self", "en-US", "Service", "Level 2 Menu"),
            ("menu.logistics.maintenance._self", "en-US", "Maintenance", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.sales._self", "ja-JP", "販売管理", "レベル2メニュー"),
            ("menu.logistics.materials._self", "ja-JP", "資材管理", "レベル2メニュー"),
            ("menu.logistics.manufacturing._self", "ja-JP", "製造実行", "レベル2メニュー"),
            ("menu.logistics.quality._self", "ja-JP", "品質管理", "レベル2メニュー"),
            ("menu.logistics.service._self", "ja-JP", "カスタマーサービス", "レベル2メニュー"),
            ("menu.logistics.maintenance._self", "ja-JP", "工場保守", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.sales._self", "zh-HK", "銷售管理", "二级菜单"),
            ("menu.logistics.materials._self", "zh-HK", "物料管理", "二级菜单"),
            ("menu.logistics.manufacturing._self", "zh-HK", "生產執行", "二级菜单"),
            ("menu.logistics.quality._self", "zh-HK", "質量管理", "二级菜单"),
            ("menu.logistics.service._self", "zh-HK", "客户服務", "二级菜单"),
            ("menu.logistics.maintenance._self", "zh-HK", "工廠維護", "二级菜单"),

            // ========================================
            // 二级菜单（人力资源）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.organization._self", "zh-CN", "组织管理", "二级菜单"),
            ("menu.humanresource.personnel._self", "zh-CN", "人事管理", "二级菜单"),
            ("menu.humanresource.attendanceleave._self", "zh-CN", "考勤假期", "二级菜单"),
            ("menu.humanresource.performance._self", "zh-CN", "绩效管理", "二级菜单"),
            ("menu.humanresource.compensationbenefits._self", "zh-CN", "薪酬福利", "二级菜单"),
            ("menu.humanresource.trainingdevelopment._self", "zh-CN", "培训发展", "二级菜单"),
            ("menu.humanresource.talent._self", "zh-CN", "人才管理", "二级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.organization._self", "en-US", "Organization", "Level 2 Menu"),
            ("menu.humanresource.personnel._self", "en-US", "Personnel", "Level 2 Menu"),
            ("menu.humanresource.attendanceleave._self", "en-US", "Attendance & Leave", "Level 2 Menu"),
            ("menu.humanresource.performance._self", "en-US", "Performance", "Level 2 Menu"),
            ("menu.humanresource.compensationbenefits._self", "en-US", "Compensation & Benefits", "Level 2 Menu"),
            ("menu.humanresource.trainingdevelopment._self", "en-US", "Training & Development", "Level 2 Menu"),
            ("menu.humanresource.talent._self", "en-US", "Talent Management", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.organization._self", "ja-JP", "組織管理", "レベル2メニュー"),
            ("menu.humanresource.personnel._self", "ja-JP", "人事管理", "レベル2メニュー"),
            ("menu.humanresource.attendanceleave._self", "ja-JP", "勤怠休暇", "レベル2メニュー"),
            ("menu.humanresource.performance._self", "ja-JP", "绩效管理", "レベル2メニュー"),
            ("menu.humanresource.compensationbenefits._self", "ja-JP", "給与福利", "レベル2メニュー"),
            ("menu.humanresource.trainingdevelopment._self", "ja-JP", "研修開発", "レベル2メニュー"),
            ("menu.humanresource.talent._self", "ja-JP", "人材管理", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.organization._self", "zh-HK", "組織管理", "二级菜单"),
            ("menu.humanresource.personnel._self", "zh-HK", "人事管理", "二级菜单"),
            ("menu.humanresource.attendanceleave._self", "zh-HK", "考勤假期", "二级菜单"),
            ("menu.humanresource.performance._self", "zh-HK", "績效管理", "二级菜单"),
            ("menu.humanresource.compensationbenefits._self", "zh-HK", "薪酬福利", "二级菜单"),
            ("menu.humanresource.trainingdevelopment._self", "zh-HK", "培訓發展", "二级菜单"),
            ("menu.humanresource.talent._self", "zh-HK", "人才管理", "二级菜单"),

            // ========================================
            // 二级菜单（身份认证）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.identity.tenant", "zh-CN", "租户管理", "二级菜单"),
            ("menu.identity.user", "zh-CN", "用户管理", "二级菜单"),
            ("menu.identity.menu", "zh-CN", "菜单管理", "二级菜单"),
            ("menu.identity.role", "zh-CN", "角色管理", "二级菜单"),

            // 英文 (en-US)
            ("menu.identity.tenant", "en-US", "Tenant", "Level 2 Menu"),
            ("menu.identity.user", "en-US", "User", "Level 2 Menu"),
            ("menu.identity.menu", "en-US", "Menu", "Level 2 Menu"),
            ("menu.identity.role", "en-US", "Role", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.identity.tenant", "ja-JP", "テナント", "レベル2メニュー"),
            ("menu.identity.user", "ja-JP", "ユーザー", "レベル2メニュー"),
            ("menu.identity.menu", "ja-JP", "メニュー", "レベル2メニュー"),
            ("menu.identity.role", "ja-JP", "ロール", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.identity.tenant", "zh-HK", "租户管理", "二级菜单"),
            ("menu.identity.user", "zh-HK", "用户管理", "二级菜单"),
            ("menu.identity.menu", "zh-HK", "菜單管理", "二级菜单"),
            ("menu.identity.role", "zh-HK", "角色管理", "二级菜单"),

            // ========================================
            // 二级菜单（工作流）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.workflow.todo", "zh-CN", "待办事项", "二级菜单"),
            ("menu.workflow.my", "zh-CN", "我的流程", "二级菜单"),
            ("menu.workflow.processed", "zh-CN", "已处理", "二级菜单"),
            ("menu.workflow.instance", "zh-CN", "流程实例", "二级菜单"),
            ("menu.workflow.scheme", "zh-CN", "流程方案", "二级菜单"),
            ("menu.workflow.form", "zh-CN", "表单管理", "二级菜单"),

            // 英文 (en-US)
            ("menu.workflow.todo", "en-US", "Todo", "Level 2 Menu"),
            ("menu.workflow.my", "en-US", "My Process", "Level 2 Menu"),
            ("menu.workflow.processed", "en-US", "Processed", "Level 2 Menu"),
            ("menu.workflow.instance", "en-US", "Process Instance", "Level 2 Menu"),
            ("menu.workflow.scheme", "en-US", "Process Scheme", "Level 2 Menu"),
            ("menu.workflow.form", "en-US", "Form", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.workflow.todo", "ja-JP", "TODO", "レベル2メニュー"),
            ("menu.workflow.my", "ja-JP", "マイプロセス", "レベル2メニュー"),
            ("menu.workflow.processed", "ja-JP", "処理済み", "レベル2メニュー"),
            ("menu.workflow.instance", "ja-JP", "プロセスインスタンス", "レベル2メニュー"),
            ("menu.workflow.scheme", "ja-JP", "プロセス方案", "レベル2メニュー"),
            ("menu.workflow.form", "ja-JP", "フォーム", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.workflow.todo", "zh-HK", "待辦事項", "二级菜单"),
            ("menu.workflow.my", "zh-HK", "我的流程", "二级菜单"),
            ("menu.workflow.processed", "zh-HK", "已處理", "二级菜单"),
            ("menu.workflow.instance", "zh-HK", "流程實例", "二级菜单"),
            ("menu.workflow.scheme", "zh-HK", "流程方案", "二级菜单"),
            ("menu.workflow.form", "zh-HK", "表單管理", "二级菜单"),

            // ========================================
            // 二级菜单（代码管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.code.generator", "zh-CN", "代码生成", "二级菜单"),

            // 英文 (en-US)
            ("menu.code.generator", "en-US", "Code Generator", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.code.generator", "ja-JP", "コード生成", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.code.generator", "zh-HK", "代碼生成", "二级菜单"),

            // ========================================
            // 二级菜单（基础设置）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.foundation.numbering", "zh-CN", "编码规则", "二级菜单"),
            ("menu.foundation.dict", "zh-CN", "数据字典", "二级菜单"),
            ("menu.foundation.i18n", "zh-CN", "国际化", "二级菜单"),
            ("menu.foundation.file", "zh-CN", "文件管理", "二级菜单"),
            ("menu.foundation.device", "zh-CN", "系统设备", "二级菜单"),
            ("menu.foundation.cache", "zh-CN", "缓存管理", "二级菜单"),
            ("menu.foundation.vocabulary", "zh-CN", "敏感词库", "二级菜单"),
            ("menu.foundation.setting", "zh-CN", "系统设置", "二级菜单"),
            ("menu.foundation.online", "zh-CN", "在线用户", "二级菜单"),
            ("menu.foundation.message", "zh-CN", "在线消息", "二级菜单"),

            // 英文 (en-US)
            ("menu.foundation.numbering", "en-US", "Numbering", "Level 2 Menu"),
            ("menu.foundation.dict", "en-US", "Dictionary", "Level 2 Menu"),
            ("menu.foundation.i18n", "en-US", "Internationalization", "Level 2 Menu"),
            ("menu.foundation.file", "en-US", "File", "Level 2 Menu"),
            ("menu.foundation.device", "en-US", "Device", "Level 2 Menu"),
            ("menu.foundation.cache", "en-US", "Cache", "Level 2 Menu"),
            ("menu.foundation.vocabulary", "en-US", "Sensitive Vocabulary", "Level 2 Menu"),
            ("menu.foundation.setting", "en-US", "Setting", "Level 2 Menu"),
            ("menu.foundation.online", "en-US", "Online Users", "Level 2 Menu"),
            ("menu.foundation.message", "en-US", "Online Messages", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.foundation.numbering", "ja-JP", "番号規則", "レベル2メニュー"),
            ("menu.foundation.dict", "ja-JP", "データディクショナリ", "レベル2メニュー"),
            ("menu.foundation.i18n", "ja-JP", "国際化", "レベル2メニュー"),
            ("menu.foundation.file", "ja-JP", "ファイル", "レベル2メニュー"),
            ("menu.foundation.device", "ja-JP", "デバイス", "レベル2メニュー"),
            ("menu.foundation.cache", "ja-JP", "キャッシュ", "レベル2メニュー"),
            ("menu.foundation.vocabulary", "ja-JP", "敏感語彙", "レベル2メニュー"),
            ("menu.foundation.setting", "ja-JP", "システム設定", "レベル2メニュー"),
            ("menu.foundation.online", "ja-JP", "オンラインユーザー", "レベル2メニュー"),
            ("menu.foundation.message", "ja-JP", "オンラインメッセージ", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.foundation.numbering", "zh-HK", "編碼規則", "二级菜单"),
            ("menu.foundation.dict", "zh-HK", "數據字典", "二级菜单"),
            ("menu.foundation.i18n", "zh-HK", "國際化", "二级菜单"),
            ("menu.foundation.file", "zh-HK", "文件管理", "二级菜单"),
            ("menu.foundation.device", "zh-HK", "系統設備", "二级菜单"),
            ("menu.foundation.cache", "zh-HK", "緩存管理", "二级菜单"),
            ("menu.foundation.vocabulary", "zh-HK", "敏感詞庫", "二级菜单"),
            ("menu.foundation.setting", "zh-HK", "系統設置", "二级菜单"),
            ("menu.foundation.online", "zh-HK", "在線用户", "二级菜单"),
            ("menu.foundation.message", "zh-HK", "在線消息", "二级菜单"),

            // ========================================
            // 二级菜单（统计看板）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.statistics.report._self", "zh-CN", "报表管理", "二级菜单"),
            ("menu.statistics.logging._self", "zh-CN", "日志管理", "二级菜单"),

            // 英文 (en-US)
            ("menu.statistics.report._self", "en-US", "Report Management", "Level 2 Menu"),
            ("menu.statistics.logging._self", "en-US", "Log Management", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.statistics.report._self", "ja-JP", "レポート管理", "レベル2メニュー"),
            ("menu.statistics.logging._self", "ja-JP", "ログ管理", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.statistics.report._self", "zh-HK", "報表管理", "二级菜单"),
            ("menu.statistics.logging._self", "zh-HK", "日誌管理", "二级菜单"),

            // ========================================
            // 三级菜单（管理会计）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.accounting.financial.accounttitle", "zh-CN", "会计科目", "三级菜单"),
            ("menu.accounting.financial.asset", "zh-CN", "固定资产", "三级菜单"),
            ("menu.accounting.financial.countersign", "zh-CN", "会签管理", "三级菜单"),
            ("menu.accounting.financial.company", "zh-CN", "公司管理", "三级菜单"),

            // 英文 (en-US)
            ("menu.accounting.financial.accounttitle", "en-US", "Account Title", "Level 3 Menu"),
            ("menu.accounting.financial.asset", "en-US", "Fixed Asset", "Level 3 Menu"),
            ("menu.accounting.financial.countersign", "en-US", "Countersign", "Level 3 Menu"),
            ("menu.accounting.financial.company", "en-US", "Company", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.accounting.financial.accounttitle", "ja-JP", "会計科目", "レベル3メニュー"),
            ("menu.accounting.financial.asset", "ja-JP", "固定資産", "レベル3メニュー"),
            ("menu.accounting.financial.countersign", "ja-JP", "会签管理", "レベル3メニュー"),
            ("menu.accounting.financial.company", "ja-JP", "会社管理", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.accounting.financial.accounttitle", "zh-HK", "會計科目", "三级菜单"),
            ("menu.accounting.financial.asset", "zh-HK", "固定資產", "三级菜单"),
            ("menu.accounting.financial.countersign", "zh-HK", "會籤管理", "三级菜单"),
            ("menu.accounting.financial.company", "zh-HK", "公司管理", "三级菜单"),

            // ========================================
            // 三级菜单（控制会计）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.accounting.controlling.profitcenter", "zh-CN", "利润中心", "三级菜单"),
            ("menu.accounting.controlling.costcenter", "zh-CN", "成本中心", "三级菜单"),
            ("menu.accounting.controlling.costelement", "zh-CN", "成本要素", "三级菜单"),
            ("menu.accounting.controlling.wagerate", "zh-CN", "工资率", "三级菜单"),

            // 英文 (en-US)
            ("menu.accounting.controlling.profitcenter", "en-US", "Profit Center", "Level 3 Menu"),
            ("menu.accounting.controlling.costcenter", "en-US", "Cost Center", "Level 3 Menu"),
            ("menu.accounting.controlling.costelement", "en-US", "Cost Element", "Level 3 Menu"),
            ("menu.accounting.controlling.wagerate", "en-US", "Wage Rate", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.accounting.controlling.profitcenter", "ja-JP", "利益センター", "レベル3メニュー"),
            ("menu.accounting.controlling.costcenter", "ja-JP", "原価センター", "レベル3メニュー"),
            ("menu.accounting.controlling.costelement", "ja-JP", "原価要素", "レベル3メニュー"),
            ("menu.accounting.controlling.wagerate", "ja-JP", "賃金率", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.accounting.controlling.profitcenter", "zh-HK", "利潤中心", "三级菜单"),
            ("menu.accounting.controlling.costcenter", "zh-HK", "成本中心", "三级菜单"),
            ("menu.accounting.controlling.costelement", "zh-HK", "成本要素", "三级菜单"),
            ("menu.accounting.controlling.wagerate", "zh-HK", "工資率", "三级菜单"),

            // ========================================
            // 三级菜单（Materials 物料管理-工厂/物料）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.materials.plant", "zh-CN", "工厂信息", "三级菜单"),
            ("menu.logistics.materials.material", "zh-CN", "物料", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.materials.plant", "en-US", "Plant", "Level 3 Menu"),
            ("menu.logistics.materials.material", "en-US", "Material", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.materials.plant", "ja-JP", "工場情報", "レベル3メニュー"),
            ("menu.logistics.materials.material", "ja-JP", "資材", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.materials.plant", "zh-HK", "工廠信息", "三级菜单"),
            ("menu.logistics.materials.material", "zh-HK", "物料", "三级菜单"),

            // ========================================
            // 三级菜单（Materials 物料管理-采购）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.materials.purchasing.supplier", "zh-CN", "供应商", "三级菜单"),
            ("menu.logistics.materials.purchasing.vendor", "zh-CN", "经销商", "三级菜单"),
            ("menu.logistics.materials.purchasing.info", "zh-CN", "采购信息", "三级菜单"),
            ("menu.logistics.materials.purchasing.source", "zh-CN", "货源信息", "三级菜单"),
            ("menu.logistics.materials.purchasing.request", "zh-CN", "采购申请", "三级菜单"),
            ("menu.logistics.materials.purchasing.order", "zh-CN", "采购订单", "三级菜单"),
            ("menu.logistics.materials.purchasing.invoice", "zh-CN", "采购发票", "三级菜单"),
            ("menu.logistics.materials.purchasing.plan", "zh-CN", "采购计划", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.materials.purchasing.supplier", "en-US", "Supplier", "Level 3 Menu"),
            ("menu.logistics.materials.purchasing.vendor", "en-US", "Vendor", "Level 3 Menu"),
            ("menu.logistics.materials.purchasing.info", "en-US", "Purchasing Info", "Level 3 Menu"),
            ("menu.logistics.materials.purchasing.source", "en-US", "Source Info", "Level 3 Menu"),
            ("menu.logistics.materials.purchasing.request", "en-US", "Purchase Request", "Level 3 Menu"),
            ("menu.logistics.materials.purchasing.order", "en-US", "Purchase Order", "Level 3 Menu"),
            ("menu.logistics.materials.purchasing.invoice", "en-US", "Purchase Invoice", "Level 3 Menu"),
            ("menu.logistics.materials.purchasing.plan", "en-US", "Purchase Plan", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.materials.purchasing.supplier", "ja-JP", "サプライヤー", "レベル3メニュー"),
            ("menu.logistics.materials.purchasing.vendor", "ja-JP", "ベンダー", "レベル3メニュー"),
            ("menu.logistics.materials.purchasing.info", "ja-JP", "購買情報", "レベル3メニュー"),
            ("menu.logistics.materials.purchasing.source", "ja-JP", "ソース情報", "レベル3メニュー"),
            ("menu.logistics.materials.purchasing.request", "ja-JP", "購買申請", "レベル3メニュー"),
            ("menu.logistics.materials.purchasing.order", "ja-JP", "購買オーダ", "レベル3メニュー"),
            ("menu.logistics.materials.purchasing.invoice", "ja-JP", "購買請求書", "レベル3メニュー"),
            ("menu.logistics.materials.purchasing.plan", "ja-JP", "購買計画", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.materials.purchasing.supplier", "zh-HK", "供應商", "三级菜单"),
            ("menu.logistics.materials.purchasing.vendor", "zh-HK", "經銷商", "三级菜单"),
            ("menu.logistics.materials.purchasing.info", "zh-HK", "採購信息", "三级菜单"),
            ("menu.logistics.materials.purchasing.source", "zh-HK", "貨源信息", "三级菜单"),
            ("menu.logistics.materials.purchasing.request", "zh-HK", "採購申請", "三级菜单"),
            ("menu.logistics.materials.purchasing.order", "zh-HK", "採購訂單", "三级菜单"),
            ("menu.logistics.materials.purchasing.invoice", "zh-HK", "採購發票", "三级菜单"),
            ("menu.logistics.materials.purchasing.plan", "zh-HK", "採購計劃", "三级菜单"),

            // ========================================
            // 三级菜单（生产执行-BOM）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.manufacturing.bom.modeldestination", "zh-CN", "机种仕向", "三级菜单"),
            ("menu.logistics.manufacturing.bom.list", "zh-CN", "物料清单", "三级菜单"),
            ("menu.logistics.manufacturing.bom.routin", "zh-CN", "工艺路线", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.manufacturing.bom.modeldestination", "en-US", "Model Destination", "Level 3 Menu"),
            ("menu.logistics.manufacturing.bom.list", "en-US", "BOM List", "Level 3 Menu"),
            ("menu.logistics.manufacturing.bom.routin", "en-US", "Routing", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.manufacturing.bom.modeldestination", "ja-JP", "機種仕向", "レベル3メニュー"),
            ("menu.logistics.manufacturing.bom.list", "ja-JP", "部品表", "レベル3メニュー"),
            ("menu.logistics.manufacturing.bom.routin", "ja-JP", "工程ルート", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.manufacturing.bom.modeldestination", "zh-HK", "機種仕向", "三级菜单"),
            ("menu.logistics.manufacturing.bom.list", "zh-HK", "物料清單", "三级菜单"),
            ("menu.logistics.manufacturing.bom.routin", "zh-HK", "工藝路線", "三级菜单"),

            // ========================================
            // 三级菜单（生产执行-工单）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.manufacturing.workorder", "zh-CN", "工单管理", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.manufacturing.workorder", "en-US", "Work Order", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.manufacturing.workorder", "ja-JP", "作業指図", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.manufacturing.workorder", "zh-HK", "工單管理", "三级菜单"),

            // ========================================
            // 三级菜单（生产执行-排程）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.manufacturing.scheduling.weekly", "zh-CN", "周排程", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.manufacturing.scheduling.weekly", "en-US", "Weekly Schedule", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.manufacturing.scheduling.weekly", "ja-JP", "週間スケジュール", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.manufacturing.scheduling.weekly", "zh-HK", "周排程", "三级菜单"),

            // ========================================
            // 三级菜单（生产执行-设变）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.manufacturing.ecn.kanban", "zh-CN", "设变看板", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.kakunin", "zh-CN", "物料确认", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.hinkan", "zh-CN", "品管部门", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.legacyproduct", "zh-CN", "旧品管制", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.manufacturing.ecn.kanban", "en-US", "ECN Board", "Level 3 Menu"),
            ("menu.logistics.manufacturing.ecn.kakunin", "en-US", "Material Confirm", "Level 3 Menu"),
            ("menu.logistics.manufacturing.ecn.hinkan", "en-US", "Quality Dept", "Level 3 Menu"),
            ("menu.logistics.manufacturing.ecn.legacyproduct", "en-US", "Old Product", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.manufacturing.ecn.kanban", "ja-JP", "変更看板", "レベル3メニュー"),
            ("menu.logistics.manufacturing.ecn.kakunin", "ja-JP", "物料確認", "レベル3メニュー"),
            ("menu.logistics.manufacturing.ecn.hinkan", "ja-JP", "品管部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.ecn.legacyproduct", "ja-JP", "旧品管制", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.manufacturing.ecn.kanban", "zh-HK", "設變看板", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.kakunin", "zh-HK", "物料確認", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.hinkan", "zh-HK", "品管部門", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.legacyproduct", "zh-HK", "舊品管制", "三级菜单"),

            // ========================================
            // 三级菜单（生产执行-产出-PCBA）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.manufacturing.output.pcba.production", "zh-CN", "PCBA日报", "三级菜单"),
            ("menu.logistics.manufacturing.output.pcba.repair", "zh-CN", "PCBA改修", "三级菜单"),
            ("menu.logistics.manufacturing.output.pcba.rework", "zh-CN", "PCBA返工", "三级菜单"),
            ("menu.logistics.manufacturing.output.pcba.epp", "zh-CN", "PCBA EPP", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.manufacturing.output.pcba.production", "en-US", "PCBA Daily", "Level 3 Menu"),
            ("menu.logistics.manufacturing.output.pcba.repair", "en-US", "PCBA Repair", "Level 3 Menu"),
            ("menu.logistics.manufacturing.output.pcba.rework", "en-US", "PCBA Rework", "Level 3 Menu"),
            ("menu.logistics.manufacturing.output.pcba.epp", "en-US", "PCBA EPP", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.manufacturing.output.pcba.production", "ja-JP", "PCBA日報", "レベル3メニュー"),
            ("menu.logistics.manufacturing.output.pcba.repair", "ja-JP", "PCBA改修", "レベル3メニュー"),
            ("menu.logistics.manufacturing.output.pcba.rework", "ja-JP", "PCBA返工", "レベル3メニュー"),
            ("menu.logistics.manufacturing.output.pcba.epp", "ja-JP", "PCBA EPP", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.manufacturing.output.pcba.production", "zh-HK", "PCBA日報", "三级菜单"),
            ("menu.logistics.manufacturing.output.pcba.repair", "zh-HK", "PCBA改修", "三级菜单"),
            ("menu.logistics.manufacturing.output.pcba.rework", "zh-HK", "PCBA返工", "三级菜单"),
            ("menu.logistics.manufacturing.output.pcba.epp", "zh-HK", "PCBA EPP", "三级菜单"),

            // ========================================
            // 三级菜单（生产执行-产出-组立）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.manufacturing.output.assembly.production", "zh-CN", "组立日报", "三级菜单"),
            ("menu.logistics.manufacturing.output.assembly.repair", "zh-CN", "组立改修", "三级菜单"),
            ("menu.logistics.manufacturing.output.assembly.rework", "zh-CN", "组立返工", "三级菜单"),
            ("menu.logistics.manufacturing.output.assembly.epp", "zh-CN", "组立EPP", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.manufacturing.output.assembly.production", "en-US", "Assembly Daily", "Level 3 Menu"),
            ("menu.logistics.manufacturing.output.assembly.repair", "en-US", "Assembly Repair", "Level 3 Menu"),
            ("menu.logistics.manufacturing.output.assembly.rework", "en-US", "Assembly Rework", "Level 3 Menu"),
            ("menu.logistics.manufacturing.output.assembly.epp", "en-US", "Assembly EPP", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.manufacturing.output.assembly.production", "ja-JP", "組立日報", "レベル3メニュー"),
            ("menu.logistics.manufacturing.output.assembly.repair", "ja-JP", "組立改修", "レベル3メニュー"),
            ("menu.logistics.manufacturing.output.assembly.rework", "ja-JP", "組立返工", "レベル3メニュー"),
            ("menu.logistics.manufacturing.output.assembly.epp", "ja-JP", "組立EPP", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.manufacturing.output.assembly.production", "zh-HK", "組立日報", "三级菜单"),
            ("menu.logistics.manufacturing.output.assembly.repair", "zh-HK", "組立改修", "三级菜单"),
            ("menu.logistics.manufacturing.output.assembly.rework", "zh-HK", "組立返工", "三级菜单"),
            ("menu.logistics.manufacturing.output.assembly.epp", "zh-HK", "組立EPP", "三级菜单"),

            // ========================================
            // 三级菜单（生产执行-不良-PCBA）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.manufacturing.defect.pcba.smt", "zh-CN", "SMT检查", "三级菜单"),
            ("menu.logistics.manufacturing.defect.pcba.repair", "zh-CN", "PCBA修理", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.manufacturing.defect.pcba.smt", "en-US", "SMT Check", "Level 3 Menu"),
            ("menu.logistics.manufacturing.defect.pcba.repair", "en-US", "PCBA Repair", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.manufacturing.defect.pcba.smt", "ja-JP", "SMT検査", "レベル3メニュー"),
            ("menu.logistics.manufacturing.defect.pcba.repair", "ja-JP", "PCBA修理", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.manufacturing.defect.pcba.smt", "zh-HK", "SMT檢查", "三级菜单"),
            ("menu.logistics.manufacturing.defect.pcba.repair", "zh-HK", "PCBA修理", "三级菜单"),

            // ========================================
            // 三级菜单（生产执行-不良-组立）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.manufacturing.defect.assembly.production", "zh-CN", "组立生产不良", "三级菜单"),
            ("menu.logistics.manufacturing.defect.assembly.repair", "zh-CN", "组立改修不良", "三级菜单"),
            ("menu.logistics.manufacturing.defect.assembly.rework", "zh-CN", "组立返工不良", "三级菜单"),
            ("menu.logistics.manufacturing.defect.assembly.epp", "zh-CN", "组立EPP不良", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.manufacturing.defect.assembly.production", "en-US", "Assembly Defect", "Level 3 Menu"),
            ("menu.logistics.manufacturing.defect.assembly.repair", "en-US", "Assembly Repair Defect", "Level 3 Menu"),
            ("menu.logistics.manufacturing.defect.assembly.rework", "en-US", "Assembly Rework Defect", "Level 3 Menu"),
            ("menu.logistics.manufacturing.defect.assembly.epp", "en-US", "Assembly EPP Defect", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.manufacturing.defect.assembly.production", "ja-JP", "組立生産不良", "レベル3メニュー"),
            ("menu.logistics.manufacturing.defect.assembly.repair", "ja-JP", "組立改修不良", "レベル3メニュー"),
            ("menu.logistics.manufacturing.defect.assembly.rework", "ja-JP", "組立返工不良", "レベル3メニュー"),
            ("menu.logistics.manufacturing.defect.assembly.epp", "ja-JP", "組立EPP不良", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.manufacturing.defect.assembly.production", "zh-HK", "組立生產不良", "三级菜单"),
            ("menu.logistics.manufacturing.defect.assembly.repair", "zh-HK", "組立改修不良", "三级菜单"),
            ("menu.logistics.manufacturing.defect.assembly.rework", "zh-HK", "組立返工不良", "三级菜单"),
            ("menu.logistics.manufacturing.defect.assembly.epp", "zh-HK", "組立EPP不良", "三级菜单"),

            // ========================================
            // 三级菜单（品质成本）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.quality.cost.operation", "zh-CN", "品质业务", "三级菜单"),
            ("menu.logistics.quality.cost.issue", "zh-CN", "品质问题", "三级菜单"),
            ("menu.logistics.quality.cost.scrap", "zh-CN", "品质事故", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.quality.cost.operation", "en-US", "Quality Operation", "Level 3 Menu"),
            ("menu.logistics.quality.cost.issue", "en-US", "Quality Issue", "Level 3 Menu"),
            ("menu.logistics.quality.cost.scrap", "en-US", "Quality Scrap", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.quality.cost.operation", "ja-JP", "品質業務", "レベル3メニュー"),
            ("menu.logistics.quality.cost.issue", "ja-JP", "品質問題", "レベル3メニュー"),
            ("menu.logistics.quality.cost.scrap", "ja-JP", "品質事故", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.quality.cost.operation", "zh-HK", "品質業務", "三级菜单"),
            ("menu.logistics.quality.cost.issue", "zh-HK", "品質問題", "三级菜单"),
            ("menu.logistics.quality.cost.scrap", "zh-HK", "品質事故", "三级菜单"),

            // ========================================
            // 三级菜单（质量业务）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.quality.operation.samplingscheme", "zh-CN", "抽样方案", "三级菜单"),
            ("menu.logistics.quality.operation.inspectionstandard", "zh-CN", "检验标准", "三级菜单"),
            ("menu.logistics.quality.operation.iqcorder", "zh-CN", "进货检验", "三级菜单"),
            ("menu.logistics.quality.operation.ipqcorder", "zh-CN", "制程检验", "三级菜单"),
            ("menu.logistics.quality.operation.fqcorder", "zh-CN", "入库检验", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.quality.operation.samplingscheme", "en-US", "Sampling Scheme", "Level 3 Menu"),
            ("menu.logistics.quality.operation.inspectionstandard", "en-US", "Inspection Standard", "Level 3 Menu"),
            ("menu.logistics.quality.operation.iqcorder", "en-US", "IQC Order", "Level 3 Menu"),
            ("menu.logistics.quality.operation.ipqcorder", "en-US", "IPQC Order", "Level 3 Menu"),
            ("menu.logistics.quality.operation.fqcorder", "en-US", "FQC Order", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.quality.operation.samplingscheme", "ja-JP", "抽出方案", "レベル3メニュー"),
            ("menu.logistics.quality.operation.inspectionstandard", "ja-JP", "検査基準", "レベル3メニュー"),
            ("menu.logistics.quality.operation.iqcorder", "ja-JP", "入荷検査", "レベル3メニュー"),
            ("menu.logistics.quality.operation.ipqcorder", "ja-JP", "工程検査", "レベル3メニュー"),
            ("menu.logistics.quality.operation.fqcorder", "ja-JP", "入庫検査", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.quality.operation.samplingscheme", "zh-HK", "抽樣方案", "三级菜单"),
            ("menu.logistics.quality.operation.inspectionstandard", "zh-HK", "檢驗標準", "三级菜单"),
            ("menu.logistics.quality.operation.iqcorder", "zh-HK", "進貨檢驗", "三级菜单"),
            ("menu.logistics.quality.operation.ipqcorder", "zh-HK", "製程檢驗", "三级菜单"),
            ("menu.logistics.quality.operation.fqcorder", "zh-HK", "入庫檢驗", "三级菜单"),

            // ========================================
            // 三级菜单（销售管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.sales.customer", "zh-CN", "客户信息", "三级菜单"),
            ("menu.logistics.sales.client", "zh-CN", "顾客信息", "三级菜单"),
            ("menu.logistics.sales.quotation", "zh-CN", "销售报价", "三级菜单"),
            ("menu.logistics.sales.price", "zh-CN", "销售价格", "三级菜单"),
            ("menu.logistics.sales.order", "zh-CN", "销售订单", "三级菜单"),
            ("menu.logistics.sales.invoice", "zh-CN", "销售发票", "三级菜单"),
            ("menu.logistics.sales.forecast", "zh-CN", "销售预测", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.sales.customer", "en-US", "Customer", "Level 3 Menu"),
            ("menu.logistics.sales.client", "en-US", "Client", "Level 3 Menu"),
            ("menu.logistics.sales.quotation", "en-US", "Quotation", "Level 3 Menu"),
            ("menu.logistics.sales.price", "en-US", "Price", "Level 3 Menu"),
            ("menu.logistics.sales.order", "en-US", "Sales Order", "Level 3 Menu"),
            ("menu.logistics.sales.invoice", "en-US", "Sales Invoice", "Level 3 Menu"),
            ("menu.logistics.sales.forecast", "en-US", "Forecast", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.sales.customer", "ja-JP", "客様情報", "レベル3メニュー"),
            ("menu.logistics.sales.client", "ja-JP", "顧客情報", "レベル3メニュー"),
            ("menu.logistics.sales.quotation", "ja-JP", "販売見積", "レベル3メニュー"),
            ("menu.logistics.sales.price", "ja-JP", "販売価格", "レベル3メニュー"),
            ("menu.logistics.sales.order", "ja-JP", "販売オーダ", "レベル3メニュー"),
            ("menu.logistics.sales.invoice", "ja-JP", "販売請求書", "レベル3メニュー"),
            ("menu.logistics.sales.forecast", "ja-JP", "販売予測", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.sales.customer", "zh-HK", "客户信息", "三级菜单"),
            ("menu.logistics.sales.client", "zh-HK", "顧客信息", "三级菜单"),
            ("menu.logistics.sales.quotation", "zh-HK", "銷售報價", "三级菜单"),
            ("menu.logistics.sales.price", "zh-HK", "銷售價格", "三级菜单"),
            ("menu.logistics.sales.order", "zh-HK", "銷售訂單", "三级菜单"),
            ("menu.logistics.sales.invoice", "zh-HK", "銷售發票", "三级菜单"),
            ("menu.logistics.sales.forecast", "zh-HK", "銷售預測", "三级菜单"),

            // ========================================
            // 三级菜单（组织管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.organization.dept", "zh-CN", "部门管理", "三级菜单"),
            ("menu.humanresource.organization.post", "zh-CN", "岗位管理", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.organization.dept", "en-US", "Department", "Level 3 Menu"),
            ("menu.humanresource.organization.post", "en-US", "Post", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.organization.dept", "ja-JP", "部門管理", "レベル3メニュー"),
            ("menu.humanresource.organization.post", "ja-JP", "岗位管理", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.organization.dept", "zh-HK", "部門管理", "三级菜单"),
            ("menu.humanresource.organization.post", "zh-HK", "崗位管理", "三级菜单"),

            // ========================================
            // 三级菜单（人事管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.personnel.employee", "zh-CN", "员工档案", "三级菜单"),
            ("menu.humanresource.personnel.employeecontract", "zh-CN", "员工合同", "三级菜单"),
            ("menu.humanresource.personnel.employeedelegate", "zh-CN", "员工代理", "三级菜单"),
            ("menu.humanresource.personnel.employeetransfer", "zh-CN", "员工调动", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.personnel.employee", "en-US", "Employee", "Level 3 Menu"),
            ("menu.humanresource.personnel.employeecontract", "en-US", "Employee Contract", "Level 3 Menu"),
            ("menu.humanresource.personnel.employeedelegate", "en-US", "Employee Delegate", "Level 3 Menu"),
            ("menu.humanresource.personnel.employeetransfer", "en-US", "Employee Transfer", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.personnel.employee", "ja-JP", "従業員档案", "レベル3メニュー"),
            ("menu.humanresource.personnel.employeecontract", "ja-JP", "従業員契約", "レベル3メニュー"),
            ("menu.humanresource.personnel.employeedelegate", "ja-JP", "従業員代理", "レベル3メニュー"),
            ("menu.humanresource.personnel.employeetransfer", "ja-JP", "従業員異動", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.personnel.employee", "zh-HK", "員工檔案", "三级菜单"),
            ("menu.humanresource.personnel.employeecontract", "zh-HK", "員工合同", "三级菜单"),
            ("menu.humanresource.personnel.employeedelegate", "zh-HK", "員工代理", "三级菜单"),
            ("menu.humanresource.personnel.employeetransfer", "zh-HK", "員工調動", "三级菜单"),

            // ========================================
            // 三级菜单（考勤假期）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.attendanceleave.holiday", "zh-CN", "假期管理", "三级菜单"),
            ("menu.humanresource.attendanceleave.leave", "zh-CN", "请假管理", "三级菜单"),
            ("menu.humanresource.attendanceleave.overtime", "zh-CN", "加班管理", "三级菜单"),
            ("menu.humanresource.attendanceleave.attendancecorrection", "zh-CN", "补卡管理", "三级菜单"),
            ("menu.humanresource.attendanceleave.attendancesettings", "zh-CN", "考勤设置", "三级菜单"),
            ("menu.humanresource.attendanceleave.schedule", "zh-CN", "排班管理", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.attendanceleave.holiday", "en-US", "Holiday", "Level 3 Menu"),
            ("menu.humanresource.attendanceleave.leave", "en-US", "Leave", "Level 3 Menu"),
            ("menu.humanresource.attendanceleave.overtime", "en-US", "Overtime", "Level 3 Menu"),
            ("menu.humanresource.attendanceleave.attendancecorrection", "en-US", "Attendance Correction", "Level 3 Menu"),
            ("menu.humanresource.attendanceleave.attendancesettings", "en-US", "Attendance Settings", "Level 3 Menu"),
            ("menu.humanresource.attendanceleave.schedule", "en-US", "Schedule", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.attendanceleave.holiday", "ja-JP", "休暇管理", "レベル3メニュー"),
            ("menu.humanresource.attendanceleave.leave", "ja-JP", "请假管理", "レベル3メニュー"),
            ("menu.humanresource.attendanceleave.overtime", "ja-JP", "残業管理", "レベル3メニュー"),
            ("menu.humanresource.attendanceleave.attendancecorrection", "ja-JP", "補卡管理", "レベル3メニュー"),
            ("menu.humanresource.attendanceleave.attendancesettings", "ja-JP", "勤怠設定", "レベル3メニュー"),
            ("menu.humanresource.attendanceleave.schedule", "ja-JP", "排班管理", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.attendanceleave.holiday", "zh-HK", "假期管理", "三级菜单"),
            ("menu.humanresource.attendanceleave.leave", "zh-HK", "請假管理", "三级菜单"),
            ("menu.humanresource.attendanceleave.overtime", "zh-HK", "加班管理", "三级菜单"),
            ("menu.humanresource.attendanceleave.attendancecorrection", "zh-HK", "補卡管理", "三级菜单"),
            ("menu.humanresource.attendanceleave.attendancesettings", "zh-HK", "考勤設置", "三级菜单"),
            ("menu.humanresource.attendanceleave.schedule", "zh-HK", "排班管理", "三级菜单"),

            // ========================================
            // 三级菜单（薪酬福利）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.compensationbenefits.salarycalc", "zh-CN", "薪资核算", "三级菜单"),
            ("menu.humanresource.compensationbenefits.taxcalc", "zh-CN", "个税计算", "三级菜单"),
            ("menu.humanresource.compensationbenefits.socialsecurity", "zh-CN", "社保缴纳", "三级菜单"),
            ("menu.humanresource.compensationbenefits.payslip", "zh-CN", "薪资条发放", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.compensationbenefits.salarycalc", "en-US", "Salary Calculation", "Level 3 Menu"),
            ("menu.humanresource.compensationbenefits.taxcalc", "en-US", "Tax Calculation", "Level 3 Menu"),
            ("menu.humanresource.compensationbenefits.socialsecurity", "en-US", "Social Security", "Level 3 Menu"),
            ("menu.humanresource.compensationbenefits.payslip", "en-US", "Payslip", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.compensationbenefits.salarycalc", "ja-JP", "給与計算", "レベル3メニュー"),
            ("menu.humanresource.compensationbenefits.taxcalc", "ja-JP", "税額計算", "レベル3メニュー"),
            ("menu.humanresource.compensationbenefits.socialsecurity", "ja-JP", "社会保険", "レベル3メニュー"),
            ("menu.humanresource.compensationbenefits.payslip", "ja-JP", "給与明細", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.compensationbenefits.salarycalc", "zh-HK", "薪資核算", "三级菜单"),
            ("menu.humanresource.compensationbenefits.taxcalc", "zh-HK", "個税計算", "三级菜单"),
            ("menu.humanresource.compensationbenefits.socialsecurity", "zh-HK", "社保繳納", "三级菜单"),
            ("menu.humanresource.compensationbenefits.payslip", "zh-HK", "薪資條發放", "三级菜单"),

            // ========================================
            // 三级菜单（绩效管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.performance.schememetric", "zh-CN", "方案指标", "三级菜单"),
            ("menu.humanresource.performance.cycleschedule", "zh-CN", "周期日程", "三级菜单"),
            ("menu.humanresource.performance.objective", "zh-CN", "目标管理", "三级菜单"),
            ("menu.humanresource.performance.assessment", "zh-CN", "考核评估", "三级菜单"),
            ("menu.humanresource.performance.analysisimprovement", "zh-CN", "分析改进", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.performance.schememetric", "en-US", "Scheme Metric", "Level 3 Menu"),
            ("menu.humanresource.performance.cycleschedule", "en-US", "Cycle Schedule", "Level 3 Menu"),
            ("menu.humanresource.performance.objective", "en-US", "Objective", "Level 3 Menu"),
            ("menu.humanresource.performance.assessment", "en-US", "Assessment", "Level 3 Menu"),
            ("menu.humanresource.performance.analysisimprovement", "en-US", "Analysis & Improvement", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.performance.schememetric", "ja-JP", "方案指標", "レベル3メニュー"),
            ("menu.humanresource.performance.cycleschedule", "ja-JP", "周期日程", "レベル3メニュー"),
            ("menu.humanresource.performance.objective", "ja-JP", "目標管理", "レベル3メニュー"),
            ("menu.humanresource.performance.assessment", "ja-JP", "考核評価", "レベル3メニュー"),
            ("menu.humanresource.performance.analysisimprovement", "ja-JP", "分析改善", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.performance.schememetric", "zh-HK", "方案指標", "三级菜单"),
            ("menu.humanresource.performance.cycleschedule", "zh-HK", "週期日程", "三级菜单"),
            ("menu.humanresource.performance.objective", "zh-HK", "目標管理", "三级菜单"),
            ("menu.humanresource.performance.assessment", "zh-HK", "考核評估", "三级菜单"),
            ("menu.humanresource.performance.analysisimprovement", "zh-HK", "分析改進", "三级菜单"),

            // ========================================
            // 三级菜单（培训发展）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.trainingdevelopment.plan", "zh-CN", "培训计划", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.course", "zh-CN", "培训课程", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.result", "zh-CN", "培训结果", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.career", "zh-CN", "职业发展", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.trainingdevelopment.plan", "en-US", "Training Plan", "Level 3 Menu"),
            ("menu.humanresource.trainingdevelopment.course", "en-US", "Training Course", "Level 3 Menu"),
            ("menu.humanresource.trainingdevelopment.result", "en-US", "Training Result", "Level 3 Menu"),
            ("menu.humanresource.trainingdevelopment.career", "en-US", "Career Development", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.trainingdevelopment.plan", "ja-JP", "研修計画", "レベル3メニュー"),
            ("menu.humanresource.trainingdevelopment.course", "ja-JP", "研修課程", "レベル3メニュー"),
            ("menu.humanresource.trainingdevelopment.result", "ja-JP", "研修結果", "レベル3メニュー"),
            ("menu.humanresource.trainingdevelopment.career", "ja-JP", "職業発展", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.trainingdevelopment.plan", "zh-HK", "培訓計劃", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.course", "zh-HK", "培訓課程", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.result", "zh-HK", "培訓結果", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.career", "zh-HK", "職業發展", "三级菜单"),

            // ========================================
            // 三级菜单（人才管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.talent.staffingrequirement", "zh-CN", "用人需求", "三级菜单"),
            ("menu.humanresource.talent.recruitmentplan", "zh-CN", "招聘计划", "三级菜单"),
            ("menu.humanresource.talent.jobposting", "zh-CN", "职位发布", "三级菜单"),
            ("menu.humanresource.talent.resumefilter", "zh-CN", "简历筛选", "三级菜单"),
            ("menu.humanresource.talent.interview", "zh-CN", "面试安排", "三级菜单"),
            ("menu.humanresource.talent.offer", "zh-CN", "录用", "三级菜单"),
            ("menu.humanresource.personnel.employeeonboardingtodo", "zh-CN", "入职待办", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.talent.staffingrequirement", "en-US", "Staffing Requirement", "Level 3 Menu"),
            ("menu.humanresource.talent.recruitmentplan", "en-US", "Recruitment Plan", "Level 3 Menu"),
            ("menu.humanresource.talent.jobposting", "en-US", "Job Posting", "Level 3 Menu"),
            ("menu.humanresource.talent.resumefilter", "en-US", "Resume Filter", "Level 3 Menu"),
            ("menu.humanresource.talent.interview", "en-US", "Interview", "Level 3 Menu"),
            ("menu.humanresource.talent.offer", "en-US", "Offer", "Level 3 Menu"),
            ("menu.humanresource.personnel.employeeonboardingtodo", "en-US", "Onboarding Todo", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.talent.staffingrequirement", "ja-JP", "用人需求", "レベル3メニュー"),
            ("menu.humanresource.talent.recruitmentplan", "ja-JP", "採用計画", "レベル3メニュー"),
            ("menu.humanresource.talent.jobposting", "ja-JP", "職位発布", "レベル3メニュー"),
            ("menu.humanresource.talent.resumefilter", "ja-JP", "履歴書筛选", "レベル3メニュー"),
            ("menu.humanresource.talent.interview", "ja-JP", "面接安排", "レベル3メニュー"),
            ("menu.humanresource.talent.offer", "ja-JP", "採用", "レベル3メニュー"),
            ("menu.humanresource.personnel.employeeonboardingtodo", "ja-JP", "入社待办", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.talent.staffingrequirement", "zh-HK", "用人需求", "三级菜单"),
            ("menu.humanresource.talent.recruitmentplan", "zh-HK", "招聘計劃", "三级菜单"),
            ("menu.humanresource.talent.jobposting", "zh-HK", "職位發佈", "三级菜单"),
            ("menu.humanresource.talent.resumefilter", "zh-HK", "簡歷篩選", "三级菜单"),
            ("menu.humanresource.talent.interview", "zh-HK", "面試安排", "三级菜单"),
            ("menu.humanresource.talent.offer", "zh-HK", "錄用", "三级菜单"),
            ("menu.humanresource.personnel.employeeonboardingtodo", "zh-HK", "入職待辦", "三级菜单"),

            // ========================================
            // 三级菜单（统计看板-报表管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.statistics.report.financial._self", "zh-CN", "财务统计", "三级菜单"),
            ("menu.statistics.report.humanresource._self", "zh-CN", "人力统计", "三级菜单"),
            ("menu.statistics.report.logistics._self", "zh-CN", "后勤统计", "三级菜单"),
            ("menu.statistics.logging.loginlog", "zh-CN", "登录日志", "三级菜单"),
            ("menu.statistics.logging.operlog", "zh-CN", "操作日志", "三级菜单"),
            ("menu.statistics.logging.deltalog", "zh-CN", "差异日志", "三级菜单"),
            ("menu.statistics.logging.quartzlog", "zh-CN", "任务日志", "三级菜单"),
            ("menu.statistics.logging.servermonitor", "zh-CN", "服务监控", "三级菜单"),

            // 英文 (en-US)
            ("menu.statistics.report.financial._self", "en-US", "Financial Statistics", "Level 3 Menu"),
            ("menu.statistics.report.humanresource._self", "en-US", "HR Statistics", "Level 3 Menu"),
            ("menu.statistics.report.logistics._self", "en-US", "Logistics Statistics", "Level 3 Menu"),
            ("menu.statistics.logging.loginlog", "en-US", "Login Log", "Level 3 Menu"),
            ("menu.statistics.logging.operlog", "en-US", "Operation Log", "Level 3 Menu"),
            ("menu.statistics.logging.deltalog", "en-US", "Delta Log", "Level 3 Menu"),
            ("menu.statistics.logging.quartzlog", "en-US", "Job Log", "Level 3 Menu"),
            ("menu.statistics.logging.servermonitor", "en-US", "Server Monitor", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.statistics.report.financial._self", "ja-JP", "財務統計", "レベル3メニュー"),
            ("menu.statistics.report.humanresource._self", "ja-JP", "人事統計", "レベル3メニュー"),
            ("menu.statistics.report.logistics._self", "ja-JP", "后勤統計", "レベル3メニュー"),
            ("menu.statistics.logging.loginlog", "ja-JP", "ログインログ", "レベル3メニュー"),
            ("menu.statistics.logging.operlog", "ja-JP", "操作ログ", "レベル3メニュー"),
            ("menu.statistics.logging.deltalog", "ja-JP", "差分ログ", "レベル3メニュー"),
            ("menu.statistics.logging.quartzlog", "ja-JP", "タスクログ", "レベル3メニュー"),
            ("menu.statistics.logging.servermonitor", "ja-JP", "サービス監視", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.statistics.report.financial._self", "zh-HK", "財務統計", "三级菜单"),
            ("menu.statistics.report.humanresource._self", "zh-HK", "人力統計", "三级菜单"),
            ("menu.statistics.report.logistics._self", "zh-HK", "後勤統計", "三级菜单"),
            ("menu.statistics.logging.loginlog", "zh-HK", "登錄日誌", "三级菜单"),
            ("menu.statistics.logging.operlog", "zh-HK", "操作日誌", "三级菜单"),
            ("menu.statistics.logging.deltalog", "zh-HK", "差異日誌", "三级菜单"),
            ("menu.statistics.logging.quartzlog", "zh-HK", "任務日誌", "三级菜单"),
            ("menu.statistics.logging.servermonitor", "zh-HK", "服務監控", "三级菜单"),

            // ========================================
            // 四级菜单（统计看板-报表管理-财务统计）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.statistics.report.financial.management", "zh-CN", "管理统计", "四级菜单"),
            ("menu.statistics.report.financial.controlling", "zh-CN", "控制统计", "四级菜单"),

            // 英文 (en-US)
            ("menu.statistics.report.financial.management", "en-US", "Management Statistics", "Level 4 Menu"),
            ("menu.statistics.report.financial.controlling", "en-US", "Controlling Statistics", "Level 4 Menu"),

            // 日文 (ja-JP)
            ("menu.statistics.report.financial.management", "ja-JP", "管理統計", "レベル4メニュー"),
            ("menu.statistics.report.financial.controlling", "ja-JP", "コントロール統計", "レベル4メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.statistics.report.financial.management", "zh-HK", "管理統計", "四级菜单"),
            ("menu.statistics.report.financial.controlling", "zh-HK", "控制統計", "四级菜单"),

            // ========================================
            // 四级菜单（统计看板-报表管理-人力统计）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.statistics.report.humanresource.attendance", "zh-CN", "考勤统计", "四级菜单"),
            ("menu.statistics.report.humanresource.personnel", "zh-CN", "人事统计", "四级菜单"),
            ("menu.statistics.report.humanresource.talent", "zh-CN", "人才统计", "四级菜单"),

            // 英文 (en-US)
            ("menu.statistics.report.humanresource.attendance", "en-US", "Attendance Statistics", "Level 4 Menu"),
            ("menu.statistics.report.humanresource.personnel", "en-US", "Personnel Statistics", "Level 4 Menu"),
            ("menu.statistics.report.humanresource.talent", "en-US", "Talent Statistics", "Level 4 Menu"),

            // 日文 (ja-JP)
            ("menu.statistics.report.humanresource.attendance", "ja-JP", "勤怠統計", "レベル4メニュー"),
            ("menu.statistics.report.humanresource.personnel", "ja-JP", "人事統計", "レベル4メニュー"),
            ("menu.statistics.report.humanresource.talent", "ja-JP", "人材統計", "レベル4メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.statistics.report.humanresource.attendance", "zh-HK", "考勤統計", "四级菜单"),
            ("menu.statistics.report.humanresource.personnel", "zh-HK", "人事統計", "四级菜单"),
            ("menu.statistics.report.humanresource.talent", "zh-HK", "人才統計", "四级菜单"),

            // ========================================
            // 四级菜单（统计看板-报表管理-后勤统计）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.statistics.report.logistics.maintenance", "zh-CN", "维护统计", "四级菜单"),
            ("menu.statistics.report.logistics.manufacturing", "zh-CN", "生产统计", "四级菜单"),
            ("menu.statistics.report.logistics.material", "zh-CN", "物料统计", "四级菜单"),
            ("menu.statistics.report.logistics.quality", "zh-CN", "质量统计", "四级菜单"),
            ("menu.statistics.report.logistics.sales", "zh-CN", "销售统计", "四级菜单"),
            ("menu.statistics.report.logistics.serial", "zh-CN", "序列号统计", "四级菜单"),

            // 英文 (en-US)
            ("menu.statistics.report.logistics.maintenance", "en-US", "Maintenance Statistics", "Level 4 Menu"),
            ("menu.statistics.report.logistics.manufacturing", "en-US", "Production Statistics", "Level 4 Menu"),
            ("menu.statistics.report.logistics.material", "en-US", "Material Statistics", "Level 4 Menu"),
            ("menu.statistics.report.logistics.quality", "en-US", "Quality Statistics", "Level 4 Menu"),
            ("menu.statistics.report.logistics.sales", "en-US", "Sales Statistics", "Level 4 Menu"),
            ("menu.statistics.report.logistics.serial", "en-US", "Serial Number Statistics", "Level 4 Menu"),

            // 日文 (ja-JP)
            ("menu.statistics.report.logistics.maintenance", "ja-JP", "保守統計", "レベル4メニュー"),
            ("menu.statistics.report.logistics.manufacturing", "ja-JP", "生産統計", "レベル4メニュー"),
            ("menu.statistics.report.logistics.material", "ja-JP", "資材統計", "レベル4メニュー"),
            ("menu.statistics.report.logistics.quality", "ja-JP", "品質統計", "レベル4メニュー"),
            ("menu.statistics.report.logistics.sales", "ja-JP", "販売統計", "レベル4メニュー"),
            ("menu.statistics.report.logistics.serial", "ja-JP", "シリアル番号統計", "レベル4メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.statistics.report.logistics.maintenance", "zh-HK", "維護統計", "四级菜单"),
            ("menu.statistics.report.logistics.manufacturing", "zh-HK", "生產統計", "四级菜单"),
            ("menu.statistics.report.logistics.material", "zh-HK", "物料統計", "四级菜单"),
            ("menu.statistics.report.logistics.quality", "zh-HK", "質量統計", "四级菜单"),
            ("menu.statistics.report.logistics.sales", "zh-HK", "銷售統計", "四级菜单"),
            ("menu.statistics.report.logistics.serial", "zh-HK", "序列號統計", "四级菜单"),

            // ========================================
            // 三级菜单（后勤服务-客诉与维修）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.service.complaint", "zh-CN", "客诉管理", "三级菜单"),
            ("menu.logistics.maintenance.repair", "zh-CN", "维修管理", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.service.complaint", "en-US", "Complaint", "Level 3 Menu"),
            ("menu.logistics.maintenance.repair", "en-US", "Repair", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.service.complaint", "ja-JP", "苦情管理", "レベル3メニュー"),
            ("menu.logistics.maintenance.repair", "ja-JP", "修理管理", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.service.complaint", "zh-HK", "客訴管理", "三级菜单"),
            ("menu.logistics.maintenance.repair", "zh-HK", "維修管理", "三级菜单"),

            // ========================================
            // 三级目录与设变子项（与菜单种子 I18nKey 对齐）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.materials.purchasing._self", "zh-CN", "采购管理", "三级目录"),
            ("menu.logistics.manufacturing.bom._self", "zh-CN", "BOM管理", "三级目录"),
            ("menu.logistics.manufacturing.scheduling._self", "zh-CN", "生产排程", "三级目录"),
            ("menu.logistics.manufacturing.ecn._self", "zh-CN", "设变", "三级目录"),
            ("menu.logistics.manufacturing.ecn.batch", "zh-CN", "投入批次", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.gijutsu", "zh-CN", "技术部门", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.koubai", "zh-CN", "采购部门", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.seikan", "zh-CN", "生管部门", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.ukeken", "zh-CN", "受检部门", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.bukan", "zh-CN", "部管部门", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.seizonika", "zh-CN", "制造二课", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.seizoikka", "zh-CN", "制造一课", "三级菜单"),
            ("menu.logistics.manufacturing.output._self", "zh-CN", "OPH管理", "三级目录"),
            ("menu.logistics.manufacturing.output.pcba._self", "zh-CN", "PCB生产", "三级目录"),
            ("menu.logistics.manufacturing.output.assembly._self", "zh-CN", "组立生产", "三级目录"),
            ("menu.logistics.manufacturing.defect._self", "zh-CN", "不良", "三级目录"),
            ("menu.logistics.manufacturing.defect.pcba._self", "zh-CN", "PCBA不良", "三级目录"),
            ("menu.logistics.manufacturing.defect.assembly._self", "zh-CN", "组立不良", "三级目录"),
            ("menu.logistics.quality.cost._self", "zh-CN", "品质成本", "三级目录"),
            ("menu.logistics.quality.operation._self", "zh-CN", "质量业务", "三级目录"),

            // 英文 (en-US)
            ("menu.logistics.materials.purchasing._self", "en-US", "Purchasing", "Level 3 Directory"),
            ("menu.logistics.manufacturing.bom._self", "en-US", "BOM", "Level 3 Directory"),
            ("menu.logistics.manufacturing.scheduling._self", "en-US", "Scheduling", "Level 3 Directory"),
            ("menu.logistics.manufacturing.ecn._self", "en-US", "ECN", "Level 3 Directory"),
            ("menu.logistics.manufacturing.ecn.batch", "en-US", "Input Batch", "Level 3 Menu"),
            ("menu.logistics.manufacturing.ecn.gijutsu", "en-US", "Technical Dept", "Level 3 Menu"),
            ("menu.logistics.manufacturing.ecn.koubai", "en-US", "Purchasing Dept", "Level 3 Menu"),
            ("menu.logistics.manufacturing.ecn.seikan", "en-US", "Mfg Control Dept", "Level 3 Menu"),
            ("menu.logistics.manufacturing.ecn.ukeken", "en-US", "Inspection Dept", "Level 3 Menu"),
            ("menu.logistics.manufacturing.ecn.bukan", "en-US", "Dept Management", "Level 3 Menu"),
            ("menu.logistics.manufacturing.ecn.seizonika", "en-US", "Mfg Section 2", "Level 3 Menu"),
            ("menu.logistics.manufacturing.ecn.seizoikka", "en-US", "Mfg Section 1", "Level 3 Menu"),
            ("menu.logistics.manufacturing.output._self", "en-US", "OPH", "Level 3 Directory"),
            ("menu.logistics.manufacturing.output.pcba._self", "en-US", "PCB Output", "Level 3 Directory"),
            ("menu.logistics.manufacturing.output.assembly._self", "en-US", "Assembly Output", "Level 3 Directory"),
            ("menu.logistics.manufacturing.defect._self", "en-US", "Defect", "Level 3 Directory"),
            ("menu.logistics.manufacturing.defect.pcba._self", "en-US", "PCBA Defect", "Level 3 Directory"),
            ("menu.logistics.manufacturing.defect.assembly._self", "en-US", "Assembly Defect", "Level 3 Directory"),
            ("menu.logistics.quality.cost._self", "en-US", "Quality Cost", "Level 3 Directory"),
            ("menu.logistics.quality.operation._self", "en-US", "Quality Operation", "Level 3 Directory"),

            // 日文 (ja-JP)
            ("menu.logistics.materials.purchasing._self", "ja-JP", "購買管理", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.bom._self", "ja-JP", "BOM管理", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.scheduling._self", "ja-JP", "生産スケジュール", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.ecn._self", "ja-JP", "設変", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.ecn.batch", "ja-JP", "投入ロット", "レベル3メニュー"),
            ("menu.logistics.manufacturing.ecn.gijutsu", "ja-JP", "技術部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.ecn.koubai", "ja-JP", "購買部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.ecn.seikan", "ja-JP", "生管部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.ecn.ukeken", "ja-JP", "受検部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.ecn.bukan", "ja-JP", "部管部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.ecn.seizonika", "ja-JP", "製造二課", "レベル3メニュー"),
            ("menu.logistics.manufacturing.ecn.seizoikka", "ja-JP", "製造一課", "レベル3メニュー"),
            ("menu.logistics.manufacturing.output._self", "ja-JP", "OPH管理", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.output.pcba._self", "ja-JP", "PCB生産", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.output.assembly._self", "ja-JP", "組立生産", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.defect._self", "ja-JP", "不良", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.defect.pcba._self", "ja-JP", "PCBA不良", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.defect.assembly._self", "ja-JP", "組立不良", "レベル3ディレクトリ"),
            ("menu.logistics.quality.cost._self", "ja-JP", "品質コスト", "レベル3ディレクトリ"),
            ("menu.logistics.quality.operation._self", "ja-JP", "品質業務", "レベル3ディレクトリ"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.materials.purchasing._self", "zh-HK", "採購管理", "三级目录"),
            ("menu.logistics.manufacturing.bom._self", "zh-HK", "BOM管理", "三级目录"),
            ("menu.logistics.manufacturing.scheduling._self", "zh-HK", "生產排程", "三级目录"),
            ("menu.logistics.manufacturing.ecn._self", "zh-HK", "設變", "三级目录"),
            ("menu.logistics.manufacturing.ecn.batch", "zh-HK", "投入批次", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.gijutsu", "zh-HK", "技術部門", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.koubai", "zh-HK", "採購部門", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.seikan", "zh-HK", "生管部門", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.ukeken", "zh-HK", "受檢部門", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.bukan", "zh-HK", "部管部門", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.seizonika", "zh-HK", "製造二課", "三级菜单"),
            ("menu.logistics.manufacturing.ecn.seizoikka", "zh-HK", "製造一課", "三级菜单"),
            ("menu.logistics.manufacturing.output._self", "zh-HK", "OPH管理", "三级目录"),
            ("menu.logistics.manufacturing.output.pcba._self", "zh-HK", "PCB生產", "三级目录"),
            ("menu.logistics.manufacturing.output.assembly._self", "zh-HK", "組立生產", "三级目录"),
            ("menu.logistics.manufacturing.defect._self", "zh-HK", "不良", "三级目录"),
            ("menu.logistics.manufacturing.defect.pcba._self", "zh-HK", "PCBA不良", "三级目录"),
            ("menu.logistics.manufacturing.defect.assembly._self", "zh-HK", "組立不良", "三级目录"),
            ("menu.logistics.quality.cost._self", "zh-HK", "品質成本", "三级目录"),
            ("menu.logistics.quality.operation._self", "zh-HK", "質量業務", "三级目录"),
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
        translation.ResourceGroup = TaktModule.Foundation;
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
