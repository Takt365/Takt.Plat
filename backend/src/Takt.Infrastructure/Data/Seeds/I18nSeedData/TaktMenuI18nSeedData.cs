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
            ("menu.logistics.serial._self", "zh-CN", "序列号管理", "二级菜单"),

            // 英文 (en-US)
            ("menu.logistics.sales._self", "en-US", "Sales", "Level 2 Menu"),
            ("menu.logistics.materials._self", "en-US", "Materials", "Level 2 Menu"),
            ("menu.logistics.manufacturing._self", "en-US", "Manufacturing", "Level 2 Menu"),
            ("menu.logistics.quality._self", "en-US", "Quality", "Level 2 Menu"),
            ("menu.logistics.service._self", "en-US", "Service", "Level 2 Menu"),
            ("menu.logistics.maintenance._self", "en-US", "Maintenance", "Level 2 Menu"),
            ("menu.logistics.serial._self", "en-US", "Serial Number", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.sales._self", "ja-JP", "販売管理", "レベル2メニュー"),
            ("menu.logistics.materials._self", "ja-JP", "資材管理", "レベル2メニュー"),
            ("menu.logistics.manufacturing._self", "ja-JP", "製造実行", "レベル2メニュー"),
            ("menu.logistics.quality._self", "ja-JP", "品質管理", "レベル2メニュー"),
            ("menu.logistics.service._self", "ja-JP", "カスタマーサービス", "レベル2メニュー"),
            ("menu.logistics.maintenance._self", "ja-JP", "工場保守", "レベル2メニュー"),
            ("menu.logistics.serial._self", "ja-JP", "シリアル管理", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.sales._self", "zh-HK", "銷售管理", "二级菜单"),
            ("menu.logistics.materials._self", "zh-HK", "物料管理", "二级菜单"),
            ("menu.logistics.manufacturing._self", "zh-HK", "生產執行", "二级菜单"),
            ("menu.logistics.quality._self", "zh-HK", "質量管理", "二级菜单"),
            ("menu.logistics.service._self", "zh-HK", "客户服務", "二级菜单"),
            ("menu.logistics.maintenance._self", "zh-HK", "工廠維護", "二级菜单"),
            ("menu.logistics.serial._self", "zh-HK", "序列號管理", "二级菜单"),

            // ========================================
            // 二级菜单（人力资源）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.organization._self", "zh-CN", "组织管理", "二级菜单"),
            ("menu.humanresource.personnel._self", "zh-CN", "人事管理", "二级菜单"),
            ("menu.humanresource.attendance._self", "zh-CN", "考勤管理", "二级菜单"),
            ("menu.humanresource.compensationbenefits._self", "zh-CN", "薪酬福利", "二级菜单"),
            ("menu.humanresource.performance._self", "zh-CN", "绩效管理", "二级菜单"),
            ("menu.humanresource.trainingdevelopment._self", "zh-CN", "培训发展", "二级菜单"),
            ("menu.humanresource.talent._self", "zh-CN", "人才管理", "二级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.organization._self", "en-US", "Organization", "Level 2 Menu"),
            ("menu.humanresource.personnel._self", "en-US", "Personnel", "Level 2 Menu"),
            ("menu.humanresource.attendance._self", "en-US", "Attendance", "Level 2 Menu"),
            ("menu.humanresource.compensationbenefits._self", "en-US", "Compensation & Benefits", "Level 2 Menu"),
            ("menu.humanresource.performance._self", "en-US", "Performance", "Level 2 Menu"),
            ("menu.humanresource.trainingdevelopment._self", "en-US", "Training & Development", "Level 2 Menu"),
            ("menu.humanresource.talent._self", "en-US", "Talent Management", "Level 2 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.organization._self", "ja-JP", "組織管理", "レベル2メニュー"),
            ("menu.humanresource.personnel._self", "ja-JP", "人事管理", "レベル2メニュー"),
            ("menu.humanresource.attendance._self", "ja-JP", "勤怠管理", "レベル2メニュー"),
            ("menu.humanresource.compensationbenefits._self", "ja-JP", "給与福利", "レベル2メニュー"),
            ("menu.humanresource.performance._self", "ja-JP", "绩效管理", "レベル2メニュー"),
            ("menu.humanresource.trainingdevelopment._self", "ja-JP", "研修開発", "レベル2メニュー"),
            ("menu.humanresource.talent._self", "ja-JP", "人材管理", "レベル2メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.organization._self", "zh-HK", "組織管理", "二级菜单"),
            ("menu.humanresource.personnel._self", "zh-HK", "人事管理", "二级菜单"),
            ("menu.humanresource.attendance._self", "zh-HK", "考勤管理", "二级菜单"),
            ("menu.humanresource.compensationbenefits._self", "zh-HK", "薪酬福利", "二级菜单"),
            ("menu.humanresource.performance._self", "zh-HK", "績效管理", "二级菜单"),
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
            ("menu.logistics.manufacturing.engineeringchange.kanban", "zh-CN", "设变看板", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.kakunin", "zh-CN", "物料确认", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.hinkan", "zh-CN", "品管部门", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.legacyproduct", "zh-CN", "旧品管制", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.manufacturing.engineeringchange.kanban", "en-US", "ECN Board", "Level 3 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.kakunin", "en-US", "Material Confirm", "Level 3 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.hinkan", "en-US", "Quality Dept", "Level 3 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.legacyproduct", "en-US", "Old Product", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.manufacturing.engineeringchange.kanban", "ja-JP", "変更看板", "レベル3メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.kakunin", "ja-JP", "物料確認", "レベル3メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.hinkan", "ja-JP", "品管部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.legacyproduct", "ja-JP", "旧品管制", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.manufacturing.engineeringchange.kanban", "zh-HK", "設變看板", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.kakunin", "zh-HK", "物料確認", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.hinkan", "zh-HK", "品管部門", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.legacyproduct", "zh-HK", "舊品管制", "三级菜单"),

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
            ("menu.logistics.quality.cost.qualityoperation", "zh-CN", "品质业务", "三级菜单"),
            ("menu.logistics.quality.cost.qualityfailure", "zh-CN", "品质问题", "三级菜单"),
            ("menu.logistics.quality.cost.qualityincident", "zh-CN", "品质事故", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.quality.cost.qualityoperation", "en-US", "Quality Operation", "Level 3 Menu"),
            ("menu.logistics.quality.cost.qualityfailure", "en-US", "Quality Failure", "Level 3 Menu"),
            ("menu.logistics.quality.cost.qualityincident", "en-US", "Quality Incident", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.quality.cost.qualityoperation", "ja-JP", "品質業務", "レベル3メニュー"),
            ("menu.logistics.quality.cost.qualityfailure", "ja-JP", "品質問題", "レベル3メニュー"),
            ("menu.logistics.quality.cost.qualityincident", "ja-JP", "品質事故", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.quality.cost.qualityoperation", "zh-HK", "品質業務", "三级菜单"),
            ("menu.logistics.quality.cost.qualityfailure", "zh-HK", "品質問題", "三级菜单"),
            ("menu.logistics.quality.cost.qualityincident", "zh-HK", "品質事故", "三级菜单"),

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
            // 四级菜单（客诉管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.quality.complaint.registration", "zh-CN", "客诉登记", "四级菜单"),
            ("menu.logistics.quality.complaint.customercomplainthandling", "zh-CN", "客诉处理", "四级菜单"),
            ("menu.logistics.quality.complaint.customersatisfactionsurvey", "zh-CN", "客户满意度调查", "四级菜单"),
            ("menu.logistics.quality.complaint.supplierevaluation", "zh-CN", "供应商评价考核", "四级菜单"),

            // 英文 (en-US)
            ("menu.logistics.quality.complaint.registration", "en-US", "Complaint Registration", "Level 4 Menu"),
            ("menu.logistics.quality.complaint.customercomplainthandling", "en-US", "Complaint Handling", "Level 4 Menu"),
            ("menu.logistics.quality.complaint.customersatisfactionsurvey", "en-US", "Satisfaction Survey", "Level 4 Menu"),
            ("menu.logistics.quality.complaint.supplierevaluation", "en-US", "Supplier Evaluation", "Level 4 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.quality.complaint.registration", "ja-JP", "客訴登録", "レベル4メニュー"),
            ("menu.logistics.quality.complaint.customercomplainthandling", "ja-JP", "客訴処理", "レベル4メニュー"),
            ("menu.logistics.quality.complaint.customersatisfactionsurvey", "ja-JP", "顧客満足度調査", "レベル4メニュー"),
            ("menu.logistics.quality.complaint.supplierevaluation", "ja-JP", "サプライヤー評価", "レベル4メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.quality.complaint.registration", "zh-HK", "客訴登記", "四级菜单"),
            ("menu.logistics.quality.complaint.customercomplainthandling", "zh-HK", "客訴處理", "四级菜单"),
            ("menu.logistics.quality.complaint.customersatisfactionsurvey", "zh-HK", "客戶滿意度調查", "四级菜单"),
            ("menu.logistics.quality.complaint.supplierevaluation", "zh-HK", "供應商評價考核", "四级菜单"),

            // ========================================
            // 三级菜单（销售管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.sales.customer", "zh-CN", "客户信息", "三级菜单"),
            ("menu.logistics.sales.client", "zh-CN", "顾客信息", "三级菜单"),
            ("menu.logistics.sales.salesquotation", "zh-CN", "销售报价", "三级菜单"),
            ("menu.logistics.sales.salesprice", "zh-CN", "销售价格", "三级菜单"),
            ("menu.logistics.sales.salesorder", "zh-CN", "销售订单", "三级菜单"),
            ("menu.logistics.sales.salesinvoice", "zh-CN", "销售发票", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.sales.customer", "en-US", "Customer", "Level 3 Menu"),
            ("menu.logistics.sales.client", "en-US", "Client", "Level 3 Menu"),
            ("menu.logistics.sales.salesquotation", "en-US", "Sales Quotation", "Level 3 Menu"),
            ("menu.logistics.sales.salesprice", "en-US", "Sales Price", "Level 3 Menu"),
            ("menu.logistics.sales.salesorder", "en-US", "Sales Order", "Level 3 Menu"),
            ("menu.logistics.sales.salesinvoice", "en-US", "Sales Invoice", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.sales.customer", "ja-JP", "客様情報", "レベル3メニュー"),
            ("menu.logistics.sales.client", "ja-JP", "顧客情報", "レベル3メニュー"),
            ("menu.logistics.sales.salesquotation", "ja-JP", "販売見積", "レベル3メニュー"),
            ("menu.logistics.sales.salesprice", "ja-JP", "販売価格", "レベル3メニュー"),
            ("menu.logistics.sales.salesorder", "ja-JP", "販売オーダ", "レベル3メニュー"),
            ("menu.logistics.sales.salesinvoice", "ja-JP", "販売請求書", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.sales.customer", "zh-HK", "客户信息", "三级菜单"),
            ("menu.logistics.sales.client", "zh-HK", "顧客信息", "三级菜单"),
            ("menu.logistics.sales.salesquotation", "zh-HK", "銷售報價", "三级菜单"),
            ("menu.logistics.sales.salesprice", "zh-HK", "銷售價格", "三级菜单"),
            ("menu.logistics.sales.salesorder", "zh-HK", "銷售訂單", "三级菜单"),
            ("menu.logistics.sales.salesinvoice", "zh-HK", "銷售發票", "三级菜单"),

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
            // 三级菜单（考勤，与 HumanResource/Attendance 实体对齐）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.attendance.workshift", "zh-CN", "班次管理", "三级菜单"),
            ("menu.humanresource.attendance.calendar", "zh-CN", "工厂日历", "三级菜单"),
            ("menu.humanresource.attendance.shiftschedule", "zh-CN", "排班计划", "三级菜单"),
            ("menu.humanresource.attendance.holiday", "zh-CN", "假期管理", "三级菜单"),
            ("menu.humanresource.attendance.leave", "zh-CN", "请假管理", "三级菜单"),
            ("menu.humanresource.attendance.overtime", "zh-CN", "加班管理", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.attendance.workshift", "en-US", "Work Shift", "Level 3 Menu"),
            ("menu.humanresource.attendance.calendar", "en-US", "Factory Calendar", "Level 3 Menu"),
            ("menu.humanresource.attendance.shiftschedule", "en-US", "Shift Schedule", "Level 3 Menu"),
            ("menu.humanresource.attendance.holiday", "en-US", "Holiday", "Level 3 Menu"),
            ("menu.humanresource.attendance.leave", "en-US", "Leave", "Level 3 Menu"),
            ("menu.humanresource.attendance.overtime", "en-US", "Overtime", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.attendance.workshift", "ja-JP", "シフト管理", "レベル3メニュー"),
            ("menu.humanresource.attendance.calendar", "ja-JP", "工場カレンダー", "レベル3メニュー"),
            ("menu.humanresource.attendance.shiftschedule", "ja-JP", "シフト計画", "レベル3メニュー"),
            ("menu.humanresource.attendance.holiday", "ja-JP", "休暇管理", "レベル3メニュー"),
            ("menu.humanresource.attendance.leave", "ja-JP", "休暇申請", "レベル3メニュー"),
            ("menu.humanresource.attendance.overtime", "ja-JP", "残業管理", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.attendance.workshift", "zh-HK", "班次管理", "三级菜单"),
            ("menu.humanresource.attendance.calendar", "zh-HK", "工廠日曆", "三级菜单"),
            ("menu.humanresource.attendance.shiftschedule", "zh-HK", "排班計劃", "三级菜单"),
            ("menu.humanresource.attendance.holiday", "zh-HK", "假期管理", "三级菜单"),
            ("menu.humanresource.attendance.leave", "zh-HK", "請假管理", "三级菜单"),
            ("menu.humanresource.attendance.overtime", "zh-HK", "加班管理", "三级菜单"),

            // ========================================
            // 三级菜单（薪酬福利，与 CompensationBenefits 实体对齐）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.compensationbenefits.benefitplan", "zh-CN", "福利方案", "三级菜单"),
            ("menu.humanresource.compensationbenefits.compensationbenefit", "zh-CN", "薪酬福利", "三级菜单"),
            ("menu.humanresource.compensationbenefits.compensationplan", "zh-CN", "薪酬方案", "三级菜单"),
            ("menu.humanresource.compensationbenefits.employeebenefit", "zh-CN", "员工福利", "三级菜单"),
            ("menu.humanresource.compensationbenefits.salaryadjustment", "zh-CN", "调薪管理", "三级菜单"),
            ("menu.humanresource.compensationbenefits.salarycomponent", "zh-CN", "薪资组成", "三级菜单"),
            ("menu.humanresource.compensationbenefits.salarystructure", "zh-CN", "薪资结构", "三级菜单"),
            ("menu.humanresource.compensationbenefits.taxrule", "zh-CN", "税务规则", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.compensationbenefits.benefitplan", "en-US", "Benefit Plan", "Level 3 Menu"),
            ("menu.humanresource.compensationbenefits.compensationbenefit", "en-US", "Compensation Benefit", "Level 3 Menu"),
            ("menu.humanresource.compensationbenefits.compensationplan", "en-US", "Compensation Plan", "Level 3 Menu"),
            ("menu.humanresource.compensationbenefits.employeebenefit", "en-US", "Employee Benefit", "Level 3 Menu"),
            ("menu.humanresource.compensationbenefits.salaryadjustment", "en-US", "Salary Adjustment", "Level 3 Menu"),
            ("menu.humanresource.compensationbenefits.salarycomponent", "en-US", "Salary Component", "Level 3 Menu"),
            ("menu.humanresource.compensationbenefits.salarystructure", "en-US", "Salary Structure", "Level 3 Menu"),
            ("menu.humanresource.compensationbenefits.taxrule", "en-US", "Tax Rule", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.compensationbenefits.benefitplan", "ja-JP", "福利方案", "レベル3メニュー"),
            ("menu.humanresource.compensationbenefits.compensationbenefit", "ja-JP", "給与福利", "レベル3メニュー"),
            ("menu.humanresource.compensationbenefits.compensationplan", "ja-JP", "給与方案", "レベル3メニュー"),
            ("menu.humanresource.compensationbenefits.employeebenefit", "ja-JP", "従業員福利", "レベル3メニュー"),
            ("menu.humanresource.compensationbenefits.salaryadjustment", "ja-JP", "昇給管理", "レベル3メニュー"),
            ("menu.humanresource.compensationbenefits.salarycomponent", "ja-JP", "給与構成", "レベル3メニュー"),
            ("menu.humanresource.compensationbenefits.salarystructure", "ja-JP", "給与構造", "レベル3メニュー"),
            ("menu.humanresource.compensationbenefits.taxrule", "ja-JP", "税務規則", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.compensationbenefits.benefitplan", "zh-HK", "福利方案", "三级菜单"),
            ("menu.humanresource.compensationbenefits.compensationbenefit", "zh-HK", "薪酬福利", "三级菜单"),
            ("menu.humanresource.compensationbenefits.compensationplan", "zh-HK", "薪酬方案", "三级菜单"),
            ("menu.humanresource.compensationbenefits.employeebenefit", "zh-HK", "員工福利", "三级菜单"),
            ("menu.humanresource.compensationbenefits.salaryadjustment", "zh-HK", "調薪管理", "三级菜单"),
            ("menu.humanresource.compensationbenefits.salarycomponent", "zh-HK", "薪資組成", "三级菜单"),
            ("menu.humanresource.compensationbenefits.salarystructure", "zh-HK", "薪資結構", "三级菜单"),
            ("menu.humanresource.compensationbenefits.taxrule", "zh-HK", "稅務規則", "三级菜单"),

            // ========================================
            // 三级菜单（绩效管理，与 Performance 实体对齐）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.performance.improvementplan", "zh-CN", "改进计划", "三级菜单"),
            ("menu.humanresource.performance.performance", "zh-CN", "绩效记录", "三级菜单"),
            ("menu.humanresource.performance.performancegoal", "zh-CN", "绩效目标", "三级菜单"),
            ("menu.humanresource.performance.performanceindicator", "zh-CN", "绩效指标", "三级菜单"),
            ("menu.humanresource.performance.performanceplan", "zh-CN", "绩效方案", "三级菜单"),
            ("menu.humanresource.performance.performancereview", "zh-CN", "绩效评审", "三级菜单"),
            ("menu.humanresource.performance.reviewcycle", "zh-CN", "评审周期", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.performance.improvementplan", "en-US", "Improvement Plan", "Level 3 Menu"),
            ("menu.humanresource.performance.performance", "en-US", "Performance Record", "Level 3 Menu"),
            ("menu.humanresource.performance.performancegoal", "en-US", "Performance Goal", "Level 3 Menu"),
            ("menu.humanresource.performance.performanceindicator", "en-US", "Performance Indicator", "Level 3 Menu"),
            ("menu.humanresource.performance.performanceplan", "en-US", "Performance Plan", "Level 3 Menu"),
            ("menu.humanresource.performance.performancereview", "en-US", "Performance Review", "Level 3 Menu"),
            ("menu.humanresource.performance.reviewcycle", "en-US", "Review Cycle", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.performance.improvementplan", "ja-JP", "改善計画", "レベル3メニュー"),
            ("menu.humanresource.performance.performance", "ja-JP", "績效記録", "レベル3メニュー"),
            ("menu.humanresource.performance.performancegoal", "ja-JP", "績效目標", "レベル3メニュー"),
            ("menu.humanresource.performance.performanceindicator", "ja-JP", "績效指標", "レベル3メニュー"),
            ("menu.humanresource.performance.performanceplan", "ja-JP", "績效方案", "レベル3メニュー"),
            ("menu.humanresource.performance.performancereview", "ja-JP", "績效評価", "レベル3メニュー"),
            ("menu.humanresource.performance.reviewcycle", "ja-JP", "評価周期", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.performance.improvementplan", "zh-HK", "改進計劃", "三级菜单"),
            ("menu.humanresource.performance.performance", "zh-HK", "績效記錄", "三级菜单"),
            ("menu.humanresource.performance.performancegoal", "zh-HK", "績效目標", "三级菜单"),
            ("menu.humanresource.performance.performanceindicator", "zh-HK", "績效指標", "三级菜单"),
            ("menu.humanresource.performance.performanceplan", "zh-HK", "績效方案", "三级菜单"),
            ("menu.humanresource.performance.performancereview", "zh-HK", "績效評審", "三级菜单"),
            ("menu.humanresource.performance.reviewcycle", "zh-HK", "評審週期", "三级菜单"),

            // ========================================
            // 三级菜单（培训发展，与 TrainingDevelopment 实体对齐）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.humanresource.trainingdevelopment.skillassessment", "zh-CN", "技能评估", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.trainingactivity", "zh-CN", "培训活动", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.trainingcourse", "zh-CN", "培训课程", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.trainingdevelopment", "zh-CN", "培训发展", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.trainingplan", "zh-CN", "培训计划", "三级菜单"),

            // 英文 (en-US)
            ("menu.humanresource.trainingdevelopment.skillassessment", "en-US", "Skill Assessment", "Level 3 Menu"),
            ("menu.humanresource.trainingdevelopment.trainingactivity", "en-US", "Training Activity", "Level 3 Menu"),
            ("menu.humanresource.trainingdevelopment.trainingcourse", "en-US", "Training Course", "Level 3 Menu"),
            ("menu.humanresource.trainingdevelopment.trainingdevelopment", "en-US", "Training Development", "Level 3 Menu"),
            ("menu.humanresource.trainingdevelopment.trainingplan", "en-US", "Training Plan", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.humanresource.trainingdevelopment.skillassessment", "ja-JP", "技能評価", "レベル3メニュー"),
            ("menu.humanresource.trainingdevelopment.trainingactivity", "ja-JP", "研修活動", "レベル3メニュー"),
            ("menu.humanresource.trainingdevelopment.trainingcourse", "ja-JP", "研修課程", "レベル3メニュー"),
            ("menu.humanresource.trainingdevelopment.trainingdevelopment", "ja-JP", "研修開発", "レベル3メニュー"),
            ("menu.humanresource.trainingdevelopment.trainingplan", "ja-JP", "研修計画", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.humanresource.trainingdevelopment.skillassessment", "zh-HK", "技能評估", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.trainingactivity", "zh-HK", "培訓活動", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.trainingcourse", "zh-HK", "培訓課程", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.trainingdevelopment", "zh-HK", "培訓發展", "三级菜单"),
            ("menu.humanresource.trainingdevelopment.trainingplan", "zh-HK", "培訓計劃", "三级菜单"),

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
            // 三级菜单（客户服务与工厂维护）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.service.servicerequest", "zh-CN", "服务请求", "三级菜单"),
            ("menu.logistics.service.servicecontract", "zh-CN", "服务合同", "三级菜单"),
            ("menu.logistics.service.serviceorder", "zh-CN", "服务订单", "三级菜单"),
            ("menu.logistics.service.serviceticket", "zh-CN", "服务工单", "三级菜单"),
            ("menu.logistics.maintenance.repair", "zh-CN", "维修管理", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.service.servicerequest", "en-US", "Service Request", "Level 3 Menu"),
            ("menu.logistics.service.servicecontract", "en-US", "Service Contract", "Level 3 Menu"),
            ("menu.logistics.service.serviceorder", "en-US", "Service Order", "Level 3 Menu"),
            ("menu.logistics.service.serviceticket", "en-US", "Service Ticket", "Level 3 Menu"),
            ("menu.logistics.maintenance.repair", "en-US", "Repair", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.service.servicerequest", "ja-JP", "サービス依頼", "レベル3メニュー"),
            ("menu.logistics.service.servicecontract", "ja-JP", "サービス契約", "レベル3メニュー"),
            ("menu.logistics.service.serviceorder", "ja-JP", "サービス受注", "レベル3メニュー"),
            ("menu.logistics.service.serviceticket", "ja-JP", "サービス工票", "レベル3メニュー"),
            ("menu.logistics.maintenance.repair", "ja-JP", "修理管理", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.service.servicerequest", "zh-HK", "服務請求", "三级菜单"),
            ("menu.logistics.service.servicecontract", "zh-HK", "服務合同", "三级菜单"),
            ("menu.logistics.service.serviceorder", "zh-HK", "服務訂單", "三级菜单"),
            ("menu.logistics.service.serviceticket", "zh-HK", "服務工單", "三级菜单"),
            ("menu.logistics.maintenance.repair", "zh-HK", "維修管理", "三级菜单"),

            // ========================================
            // 三级菜单（序列号管理）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.serial.productserialinbound", "zh-CN", "序列号入库", "三级菜单"),
            ("menu.logistics.serial.productserialoutbound", "zh-CN", "序列号出库", "三级菜单"),

            // 英文 (en-US)
            ("menu.logistics.serial.productserialinbound", "en-US", "Serial Number Inbound", "Level 3 Menu"),
            ("menu.logistics.serial.productserialoutbound", "en-US", "Serial Number Outbound", "Level 3 Menu"),

            // 日文 (ja-JP)
            ("menu.logistics.serial.productserialinbound", "ja-JP", "シリアル番号入庫", "レベル3メニュー"),
            ("menu.logistics.serial.productserialoutbound", "ja-JP", "シリアル番号出庫", "レベル3メニュー"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.serial.productserialinbound", "zh-HK", "序列號入庫", "三级菜单"),
            ("menu.logistics.serial.productserialoutbound", "zh-HK", "序列號出庫", "三级菜单"),

            // ========================================
            // 三级目录与设变子项（与菜单种子 I18nKey 对齐）
            // ========================================

            // 简体中文 (zh-CN)
            ("menu.logistics.materials.purchasing._self", "zh-CN", "采购管理", "三级目录"),
            ("menu.logistics.manufacturing.bom._self", "zh-CN", "BOM管理", "三级目录"),
            ("menu.logistics.manufacturing.scheduling._self", "zh-CN", "生产排程", "三级目录"),
            ("menu.logistics.manufacturing.engineeringchange.batch", "zh-CN", "投入批次", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.gijutsu", "zh-CN", "技术部门", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.koubai", "zh-CN", "采购部门", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.seikan", "zh-CN", "生管部门", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ukeken", "zh-CN", "受检部门", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.bukan", "zh-CN", "部管部门", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.seizonika", "zh-CN", "制造二课", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.seizoikka", "zh-CN", "制造一课", "三级菜单"),
            ("menu.logistics.manufacturing.output._self", "zh-CN", "产出管理", "三级目录"),
            ("menu.logistics.manufacturing.output.pcba._self", "zh-CN", "PCB生产", "三级目录"),
            ("menu.logistics.manufacturing.output.assembly._self", "zh-CN", "组立生产", "三级目录"),
            ("menu.logistics.manufacturing.defect._self", "zh-CN", "不良", "三级目录"),
            ("menu.logistics.manufacturing.defect.pcba._self", "zh-CN", "PCBA不良", "三级目录"),
            ("menu.logistics.manufacturing.defect.assembly._self", "zh-CN", "组立不良", "三级目录"),
            ("menu.logistics.quality.cost._self", "zh-CN", "品质成本", "三级目录"),
            ("menu.logistics.quality.operation._self", "zh-CN", "质量业务", "三级目录"),
            ("menu.logistics.quality.complaint._self", "zh-CN", "客诉管理", "三级目录"),

            // 英文 (en-US)
            ("menu.logistics.materials.purchasing._self", "en-US", "Purchasing", "Level 3 Directory"),
            ("menu.logistics.manufacturing.bom._self", "en-US", "BOM", "Level 3 Directory"),
            ("menu.logistics.manufacturing.scheduling._self", "en-US", "Scheduling", "Level 3 Directory"),
            ("menu.logistics.manufacturing.engineeringchange.batch", "en-US", "Input Batch", "Level 3 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.gijutsu", "en-US", "Technical Dept", "Level 3 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.koubai", "en-US", "Purchasing Dept", "Level 3 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.seikan", "en-US", "Mfg Control Dept", "Level 3 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.ukeken", "en-US", "Inspection Dept", "Level 3 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.bukan", "en-US", "Dept Management", "Level 3 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.seizonika", "en-US", "Mfg Section 2", "Level 3 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.seizoikka", "en-US", "Mfg Section 1", "Level 3 Menu"),
            ("menu.logistics.manufacturing.output._self", "en-US", "OPH", "Level 3 Directory"),
            ("menu.logistics.manufacturing.output.pcba._self", "en-US", "PCB Output", "Level 3 Directory"),
            ("menu.logistics.manufacturing.output.assembly._self", "en-US", "Assembly Output", "Level 3 Directory"),
            ("menu.logistics.manufacturing.defect._self", "en-US", "Defect", "Level 3 Directory"),
            ("menu.logistics.manufacturing.defect.pcba._self", "en-US", "PCBA Defect", "Level 3 Directory"),
            ("menu.logistics.manufacturing.defect.assembly._self", "en-US", "Assembly Defect", "Level 3 Directory"),
            ("menu.logistics.quality.cost._self", "en-US", "Quality Cost", "Level 3 Directory"),
            ("menu.logistics.quality.operation._self", "en-US", "Quality Operation", "Level 3 Directory"),
            ("menu.logistics.quality.complaint._self", "en-US", "Complaint", "Level 3 Directory"),

            // 日文 (ja-JP)
            ("menu.logistics.materials.purchasing._self", "ja-JP", "購買管理", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.bom._self", "ja-JP", "BOM管理", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.scheduling._self", "ja-JP", "生産スケジュール", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.engineeringchange.batch", "ja-JP", "投入ロット", "レベル3メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.gijutsu", "ja-JP", "技術部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.koubai", "ja-JP", "購買部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.seikan", "ja-JP", "生管部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.ukeken", "ja-JP", "受検部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.bukan", "ja-JP", "部管部門", "レベル3メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.seizonika", "ja-JP", "製造二課", "レベル3メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.seizoikka", "ja-JP", "製造一課", "レベル3メニュー"),
            ("menu.logistics.manufacturing.output._self", "ja-JP", "OPH管理", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.output.pcba._self", "ja-JP", "PCB生産", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.output.assembly._self", "ja-JP", "組立生産", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.defect._self", "ja-JP", "不良", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.defect.pcba._self", "ja-JP", "PCBA不良", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.defect.assembly._self", "ja-JP", "組立不良", "レベル3ディレクトリ"),
            ("menu.logistics.quality.cost._self", "ja-JP", "品質コスト", "レベル3ディレクトリ"),
            ("menu.logistics.quality.operation._self", "ja-JP", "品質業務", "レベル3ディレクトリ"),
            ("menu.logistics.quality.complaint._self", "ja-JP", "苦情管理", "レベル3ディレクトリ"),

            // 香港繁体 (zh-HK)
            ("menu.logistics.materials.purchasing._self", "zh-HK", "採購管理", "三级目录"),
            ("menu.logistics.manufacturing.bom._self", "zh-HK", "BOM管理", "三级目录"),
            ("menu.logistics.manufacturing.scheduling._self", "zh-HK", "生產排程", "三级目录"),
            ("menu.logistics.manufacturing.engineeringchange.batch", "zh-HK", "投入批次", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.gijutsu", "zh-HK", "技術部門", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.koubai", "zh-HK", "採購部門", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.seikan", "zh-HK", "生管部門", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ukeken", "zh-HK", "受檢部門", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.bukan", "zh-HK", "部管部門", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.seizonika", "zh-HK", "製造二課", "三级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.seizoikka", "zh-HK", "製造一課", "三级菜单"),
            ("menu.logistics.manufacturing.output._self", "zh-HK", "OPH管理", "三级目录"),
            ("menu.logistics.manufacturing.output.pcba._self", "zh-HK", "PCB生產", "三级目录"),
            ("menu.logistics.manufacturing.output.assembly._self", "zh-HK", "組立生產", "三级目录"),
            ("menu.logistics.manufacturing.defect._self", "zh-HK", "不良", "三级目录"),
            ("menu.logistics.manufacturing.defect.pcba._self", "zh-HK", "PCBA不良", "三级目录"),
            ("menu.logistics.manufacturing.defect.assembly._self", "zh-HK", "組立不良", "三级目录"),
            ("menu.logistics.quality.cost._self", "zh-HK", "品質成本", "三级目录"),
            ("menu.logistics.quality.operation._self", "zh-HK", "質量業務", "三级目录"),
            ("menu.logistics.quality.complaint._self", "zh-HK", "客訴管理", "三级目录"),

            // ========================================
            // 生产执行 CRUD 页面（与控制器/视图对齐）
            // ========================================
            ("menu.logistics.manufacturing.engineeringchange._self", "zh-CN", "设变", "三级目录"),
            ("menu.logistics.manufacturing.bom.billofmaterial", "zh-CN", "物料清单", "四级菜单"),
            ("menu.logistics.manufacturing.bom.billofmaterialitem", "zh-CN", "物料清单明细", "四级菜单"),
            ("menu.logistics.manufacturing.bom.billofmaterialchangelog", "zh-CN", "BOM变更记录", "四级菜单"),
            ("menu.logistics.manufacturing.bom.routing", "zh-CN", "工艺路线", "四级菜单"),
            ("menu.logistics.manufacturing.bom.routingitem", "zh-CN", "工艺路线明细", "四级菜单"),
            ("menu.logistics.manufacturing.bom.routingchangelog", "zh-CN", "工艺路线变更日志", "四级菜单"),
            ("menu.logistics.manufacturing.bom.packaging", "zh-CN", "物料包装", "四级菜单"),
            ("menu.logistics.manufacturing.bom.standardoperationtime", "zh-CN", "标准工序时间", "四级菜单"),
            ("menu.logistics.manufacturing.scheduling.apsschedule", "zh-CN", "APS排程", "四级菜单"),
            ("menu.logistics.manufacturing.scheduling.apsscheduleitem", "zh-CN", "APS排程明细", "四级菜单"),
            ("menu.logistics.manufacturing.scheduling.apsschedulechangelog", "zh-CN", "APS排程变更日志", "四级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ec", "zh-CN", "设变主", "四级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ecdetail", "zh-CN", "设变明细", "四级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ecdept", "zh-CN", "设变部门", "四级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ecattachment", "zh-CN", "设变附件", "四级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ecnotice", "zh-CN", "设变通知", "四级菜单"),
            ("menu.logistics.manufacturing.output.productionorder", "zh-CN", "生产工单", "四级菜单"),
            ("menu.logistics.manufacturing.output.pcbaoutput", "zh-CN", "PCBA日报", "四级菜单"),
            ("menu.logistics.manufacturing.output.pcbaoutputdetail", "zh-CN", "PCBA日报明细", "四级菜单"),
            ("menu.logistics.manufacturing.output.assyoutput", "zh-CN", "组立日报", "四级菜单"),
            ("menu.logistics.manufacturing.output.assyoutputdetail", "zh-CN", "组立日报明细", "四级菜单"),
            ("menu.logistics.manufacturing.output.changeover", "zh-CN", "切换记录", "四级菜单"),
            ("menu.logistics.manufacturing.output.equipmentoperationrate", "zh-CN", "机器稼动率", "四级菜单"),
            ("menu.logistics.manufacturing.output.personneloperationrate", "zh-CN", "人员稼动率", "四级菜单"),
            ("menu.logistics.manufacturing.output.productionteam", "zh-CN", "生产班组", "四级菜单"),
            ("menu.logistics.manufacturing.output.standardoperationrate", "zh-CN", "标准生产稼动率", "四级菜单"),
            ("menu.logistics.manufacturing.defect.pcbainspection", "zh-CN", "PCBA检查", "四级菜单"),
            ("menu.logistics.manufacturing.defect.pcbainspectiondetail", "zh-CN", "PCBA检查明细", "四级菜单"),
            ("menu.logistics.manufacturing.defect.pcbarepair", "zh-CN", "PCBA改修", "四级菜单"),
            ("menu.logistics.manufacturing.defect.pcbarepairdetail", "zh-CN", "PCBA改修明细", "四级菜单"),
            ("menu.logistics.manufacturing.defect.assydefect", "zh-CN", "组立不良", "四级菜单"),
            ("menu.logistics.manufacturing.defect.assydefectdetail", "zh-CN", "组立不良明细", "四级菜单"),

            ("menu.logistics.manufacturing.engineeringchange._self", "en-US", "Engineering Change", "Level 3 Directory"),
            ("menu.logistics.manufacturing.bom.billofmaterial", "en-US", "Bill of Material", "Level 4 Menu"),
            ("menu.logistics.manufacturing.bom.billofmaterialitem", "en-US", "BOM Item", "Level 4 Menu"),
            ("menu.logistics.manufacturing.bom.billofmaterialchangelog", "en-US", "BOM Change Log", "Level 4 Menu"),
            ("menu.logistics.manufacturing.bom.routing", "en-US", "Routing", "Level 4 Menu"),
            ("menu.logistics.manufacturing.bom.routingitem", "en-US", "Routing Item", "Level 4 Menu"),
            ("menu.logistics.manufacturing.bom.routingchangelog", "en-US", "Routing Change Log", "Level 4 Menu"),
            ("menu.logistics.manufacturing.bom.packaging", "en-US", "Packaging", "Level 4 Menu"),
            ("menu.logistics.manufacturing.bom.standardoperationtime", "en-US", "Standard Operation Time", "Level 4 Menu"),
            ("menu.logistics.manufacturing.scheduling.apsschedule", "en-US", "APS Schedule", "Level 4 Menu"),
            ("menu.logistics.manufacturing.scheduling.apsscheduleitem", "en-US", "APS Schedule Item", "Level 4 Menu"),
            ("menu.logistics.manufacturing.scheduling.apsschedulechangelog", "en-US", "APS Schedule Change Log", "Level 4 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.ec", "en-US", "EC Master", "Level 4 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.ecdetail", "en-US", "EC Detail", "Level 4 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.ecdept", "en-US", "EC Department", "Level 4 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.ecattachment", "en-US", "EC Attachment", "Level 4 Menu"),
            ("menu.logistics.manufacturing.engineeringchange.ecnotice", "en-US", "EC Notice", "Level 4 Menu"),
            ("menu.logistics.manufacturing.output.productionorder", "en-US", "Production Order", "Level 4 Menu"),
            ("menu.logistics.manufacturing.output.pcbaoutput", "en-US", "PCBA Output", "Level 4 Menu"),
            ("menu.logistics.manufacturing.output.pcbaoutputdetail", "en-US", "PCBA Output Detail", "Level 4 Menu"),
            ("menu.logistics.manufacturing.output.assyoutput", "en-US", "Assembly Output", "Level 4 Menu"),
            ("menu.logistics.manufacturing.output.assyoutputdetail", "en-US", "Assembly Output Detail", "Level 4 Menu"),
            ("menu.logistics.manufacturing.output.changeover", "en-US", "Changeover", "Level 4 Menu"),
            ("menu.logistics.manufacturing.output.equipmentoperationrate", "en-US", "Equipment Operation Rate", "Level 4 Menu"),
            ("menu.logistics.manufacturing.output.personneloperationrate", "en-US", "Personnel Operation Rate", "Level 4 Menu"),
            ("menu.logistics.manufacturing.output.productionteam", "en-US", "Production Team", "Level 4 Menu"),
            ("menu.logistics.manufacturing.output.standardoperationrate", "en-US", "Standard Operation Rate", "Level 4 Menu"),
            ("menu.logistics.manufacturing.defect.pcbainspection", "en-US", "PCBA Inspection", "Level 4 Menu"),
            ("menu.logistics.manufacturing.defect.pcbainspectiondetail", "en-US", "PCBA Inspection Detail", "Level 4 Menu"),
            ("menu.logistics.manufacturing.defect.pcbarepair", "en-US", "PCBA Repair", "Level 4 Menu"),
            ("menu.logistics.manufacturing.defect.pcbarepairdetail", "en-US", "PCBA Repair Detail", "Level 4 Menu"),
            ("menu.logistics.manufacturing.defect.assydefect", "en-US", "Assembly Defect", "Level 4 Menu"),
            ("menu.logistics.manufacturing.defect.assydefectdetail", "en-US", "Assembly Defect Detail", "Level 4 Menu"),

            ("menu.logistics.manufacturing.engineeringchange._self", "ja-JP", "設変", "レベル3ディレクトリ"),
            ("menu.logistics.manufacturing.bom.billofmaterial", "ja-JP", "部品表", "レベル4メニュー"),
            ("menu.logistics.manufacturing.bom.billofmaterialitem", "ja-JP", "部品表明細", "レベル4メニュー"),
            ("menu.logistics.manufacturing.bom.billofmaterialchangelog", "ja-JP", "BOM変更履歴", "レベル4メニュー"),
            ("menu.logistics.manufacturing.bom.routing", "ja-JP", "工程ルート", "レベル4メニュー"),
            ("menu.logistics.manufacturing.bom.routingitem", "ja-JP", "工程ルート明細", "レベル4メニュー"),
            ("menu.logistics.manufacturing.bom.routingchangelog", "ja-JP", "工程ルート変更履歴", "レベル4メニュー"),
            ("menu.logistics.manufacturing.bom.packaging", "ja-JP", "包装", "レベル4メニュー"),
            ("menu.logistics.manufacturing.bom.standardoperationtime", "ja-JP", "標準工程時間", "レベル4メニュー"),
            ("menu.logistics.manufacturing.scheduling.apsschedule", "ja-JP", "APSスケジュール", "レベル4メニュー"),
            ("menu.logistics.manufacturing.scheduling.apsscheduleitem", "ja-JP", "APSスケジュール明細", "レベル4メニュー"),
            ("menu.logistics.manufacturing.scheduling.apsschedulechangelog", "ja-JP", "APSスケジュール変更履歴", "レベル4メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.ec", "ja-JP", "設変主", "レベル4メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.ecdetail", "ja-JP", "設変明細", "レベル4メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.ecdept", "ja-JP", "設変部門", "レベル4メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.ecattachment", "ja-JP", "設変添付", "レベル4メニュー"),
            ("menu.logistics.manufacturing.engineeringchange.ecnotice", "ja-JP", "設変通知", "レベル4メニュー"),
            ("menu.logistics.manufacturing.output.productionorder", "ja-JP", "生産指図", "レベル4メニュー"),
            ("menu.logistics.manufacturing.output.pcbaoutput", "ja-JP", "PCBA日報", "レベル4メニュー"),
            ("menu.logistics.manufacturing.output.pcbaoutputdetail", "ja-JP", "PCBA日報明細", "レベル4メニュー"),
            ("menu.logistics.manufacturing.output.assyoutput", "ja-JP", "組立日報", "レベル4メニュー"),
            ("menu.logistics.manufacturing.output.assyoutputdetail", "ja-JP", "組立日報明細", "レベル4メニュー"),
            ("menu.logistics.manufacturing.output.changeover", "ja-JP", "切替記録", "レベル4メニュー"),
            ("menu.logistics.manufacturing.output.equipmentoperationrate", "ja-JP", "設備稼働率", "レベル4メニュー"),
            ("menu.logistics.manufacturing.output.personneloperationrate", "ja-JP", "人員稼働率", "レベル4メニュー"),
            ("menu.logistics.manufacturing.output.productionteam", "ja-JP", "生産チーム", "レベル4メニュー"),
            ("menu.logistics.manufacturing.output.standardoperationrate", "ja-JP", "標準稼働率", "レベル4メニュー"),
            ("menu.logistics.manufacturing.defect.pcbainspection", "ja-JP", "PCBA検査", "レベル4メニュー"),
            ("menu.logistics.manufacturing.defect.pcbainspectiondetail", "ja-JP", "PCBA検査明細", "レベル4メニュー"),
            ("menu.logistics.manufacturing.defect.pcbarepair", "ja-JP", "PCBA改修", "レベル4メニュー"),
            ("menu.logistics.manufacturing.defect.pcbarepairdetail", "ja-JP", "PCBA改修明細", "レベル4メニュー"),
            ("menu.logistics.manufacturing.defect.assydefect", "ja-JP", "組立不良", "レベル4メニュー"),
            ("menu.logistics.manufacturing.defect.assydefectdetail", "ja-JP", "組立不良明細", "レベル4メニュー"),

            ("menu.logistics.manufacturing.engineeringchange._self", "zh-HK", "設變", "三级目录"),
            ("menu.logistics.manufacturing.bom.billofmaterial", "zh-HK", "物料清單", "四级菜单"),
            ("menu.logistics.manufacturing.bom.billofmaterialitem", "zh-HK", "物料清單明細", "四级菜单"),
            ("menu.logistics.manufacturing.bom.billofmaterialchangelog", "zh-HK", "BOM變更記錄", "四级菜单"),
            ("menu.logistics.manufacturing.bom.routing", "zh-HK", "工藝路線", "四级菜单"),
            ("menu.logistics.manufacturing.bom.routingitem", "zh-HK", "工藝路線明細", "四级菜单"),
            ("menu.logistics.manufacturing.bom.routingchangelog", "zh-HK", "工藝路線變更日誌", "四级菜单"),
            ("menu.logistics.manufacturing.bom.packaging", "zh-HK", "物料包裝", "四级菜单"),
            ("menu.logistics.manufacturing.bom.standardoperationtime", "zh-HK", "標準工序時間", "四级菜单"),
            ("menu.logistics.manufacturing.scheduling.apsschedule", "zh-HK", "APS排程", "四级菜单"),
            ("menu.logistics.manufacturing.scheduling.apsscheduleitem", "zh-HK", "APS排程明細", "四级菜单"),
            ("menu.logistics.manufacturing.scheduling.apsschedulechangelog", "zh-HK", "APS排程變更日誌", "四级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ec", "zh-HK", "設變主", "四级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ecdetail", "zh-HK", "設變明細", "四级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ecdept", "zh-HK", "設變部門", "四级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ecattachment", "zh-HK", "設變附件", "四级菜单"),
            ("menu.logistics.manufacturing.engineeringchange.ecnotice", "zh-HK", "設變通知", "四级菜单"),
            ("menu.logistics.manufacturing.output.productionorder", "zh-HK", "生產工單", "四级菜单"),
            ("menu.logistics.manufacturing.output.pcbaoutput", "zh-HK", "PCBA日報", "四级菜单"),
            ("menu.logistics.manufacturing.output.pcbaoutputdetail", "zh-HK", "PCBA日報明細", "四级菜单"),
            ("menu.logistics.manufacturing.output.assyoutput", "zh-HK", "組立日報", "四级菜单"),
            ("menu.logistics.manufacturing.output.assyoutputdetail", "zh-HK", "組立日報明細", "四级菜单"),
            ("menu.logistics.manufacturing.output.changeover", "zh-HK", "切換記錄", "四级菜单"),
            ("menu.logistics.manufacturing.output.equipmentoperationrate", "zh-HK", "機器稼動率", "四级菜单"),
            ("menu.logistics.manufacturing.output.personneloperationrate", "zh-HK", "人員稼動率", "四级菜单"),
            ("menu.logistics.manufacturing.output.productionteam", "zh-HK", "生產班組", "四级菜单"),
            ("menu.logistics.manufacturing.output.standardoperationrate", "zh-HK", "標準生產稼動率", "四级菜单"),
            ("menu.logistics.manufacturing.defect.pcbainspection", "zh-HK", "PCBA檢查", "四级菜单"),
            ("menu.logistics.manufacturing.defect.pcbainspectiondetail", "zh-HK", "PCBA檢查明細", "四级菜单"),
            ("menu.logistics.manufacturing.defect.pcbarepair", "zh-HK", "PCBA改修", "四级菜单"),
            ("menu.logistics.manufacturing.defect.pcbarepairdetail", "zh-HK", "PCBA改修明細", "四级菜单"),
            ("menu.logistics.manufacturing.defect.assydefect", "zh-HK", "組立不良", "四级菜单"),
            ("menu.logistics.manufacturing.defect.assydefectdetail", "zh-HK", "組立不良明細", "四级菜单"),
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
