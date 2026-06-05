// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData
// 文件名称：TaktDeptI18nSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：部门国际化翻译种子数据初始化（基于TaktDeptSeedData完整组织架构）
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
/// 部门国际化翻译种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// 基于 TaktDeptSeedData.cs 完整组织架构的英日中港繁四语翻译
/// </summary>
public class TaktDeptI18nSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在名言警句之后）
    /// </summary>
    public int Order => 51;

    /// <summary>
    /// 初始化部门国际化翻译种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化部门国际化翻译种子数据...");

        // 参数验证
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过部门国际化翻译种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();
        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();
        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))
            .ToDictionary(c => c.CultureCode, c => c.Id);
        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化部门翻译数据...", tenantCode);

        foreach (var row in GetStandardDeptTranslations())
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

        TaktLogger.Information("部门国际化翻译种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取标准部门翻译列表
    /// 基于 TaktDeptSeedData.cs 完整组织架构的英日中港繁四语翻译
    /// 包含：总公司、DTA组织架构及所有课室
    /// </summary>
    private static List<(string I18nKey, string CultureCode, string TranslationText, string? ContextNote)> GetStandardDeptTranslations()
    {
        return new List<(string, string, string, string?)>
        {
            // ========================================
            // 总公司层级 (org.dept.*)
            // ========================================
            ("org.dept.headoffice", "zh-CN", "TEAC", "总公司"),
            ("org.dept.headoffice", "en-US", "TEAC", "Head Office"),
            ("org.dept.headoffice", "ja-JP", "TEAC", "本社"),
            ("org.dept.headoffice", "zh-HK", "TEAC", "总公司"),
            
            ("org.dept.d0000", "zh-CN", "DTA", "组织架构根"),
            ("org.dept.d0000", "en-US", "DTA", "Organization Root"),
            ("org.dept.d0000", "ja-JP", "DTA", "組織ルート"),
            ("org.dept.d0000", "zh-HK", "DTA", "组织架构根"),

            // ========================================
            // DTA 一级部门（D1000～D0900）
            // ========================================
            ("org.dept.d1000", "zh-CN", "总经理室", "一级部门"),
            ("org.dept.d1000", "en-US", "General Manager Office", "Level 1 Dept"),
            ("org.dept.d1000", "ja-JP", "総経理室", "レベル1部門"),
            ("org.dept.d1000", "zh-HK", "總經理室", "一级部门"),
            
            ("org.dept.d0100", "zh-CN", "总务部", "一级部门"),
            ("org.dept.d0100", "en-US", "General Affairs Dept", "Level 1 Dept"),
            ("org.dept.d0100", "ja-JP", "総務部", "レベル1部門"),
            ("org.dept.d0100", "zh-HK", "總務部", "一级部门"),
            
            ("org.dept.d0200", "zh-CN", "财务部", "一级部门"),
            ("org.dept.d0200", "en-US", "Finance Dept", "Level 1 Dept"),
            ("org.dept.d0200", "ja-JP", "財務部", "レベル1部門"),
            ("org.dept.d0200", "zh-HK", "財務部", "一级部门"),
            
            ("org.dept.d0300", "zh-CN", "IT部", "一级部门"),
            ("org.dept.d0300", "en-US", "IT Dept", "Level 1 Dept"),
            ("org.dept.d0300", "ja-JP", "IT部", "レベル1部門"),
            ("org.dept.d0300", "zh-HK", "IT部", "一级部门"),
            
            ("org.dept.d0400", "zh-CN", "管理部", "一级部门"),
            ("org.dept.d0400", "en-US", "Management Dept", "Level 1 Dept"),
            ("org.dept.d0400", "ja-JP", "管理部", "レベル1部門"),
            ("org.dept.d0400", "zh-HK", "管理部", "一级部门"),
            
            ("org.dept.d0500", "zh-CN", "资材部", "一级部门"),
            ("org.dept.d0500", "en-US", "Materials Dept", "Level 1 Dept"),
            ("org.dept.d0500", "ja-JP", "資材部", "レベル1部門"),
            ("org.dept.d0500", "zh-HK", "資材部", "一级部门"),
            
            ("org.dept.d0600", "zh-CN", "生产部", "一级部门"),
            ("org.dept.d0600", "en-US", "Production Dept", "Level 1 Dept"),
            ("org.dept.d0600", "ja-JP", "生産部", "レベル1部門"),
            ("org.dept.d0600", "zh-HK", "生產部", "一级部门"),
            
            ("org.dept.d0700", "zh-CN", "技术部", "一级部门"),
            ("org.dept.d0700", "en-US", "Technology Dept", "Level 1 Dept"),
            ("org.dept.d0700", "ja-JP", "技術部", "レベル1部門"),
            ("org.dept.d0700", "zh-HK", "技術部", "一级部门"),
            
            ("org.dept.d0800", "zh-CN", "品保部", "一级部门"),
            ("org.dept.d0800", "en-US", "Quality Assurance Dept", "Level 1 Dept"),
            ("org.dept.d0800", "ja-JP", "品質保証部", "レベル1部門"),
            ("org.dept.d0800", "zh-HK", "品保部", "一级部门"),
            
            ("org.dept.d0900", "zh-CN", "OEM部", "一级部门"),
            ("org.dept.d0900", "en-US", "OEM Dept", "Level 1 Dept"),
            ("org.dept.d0900", "ja-JP", "OEM部", "レベル1部門"),
            ("org.dept.d0900", "zh-HK", "OEM部", "一级部门"),

            // ========================================
            // 总务部下级（D0110）
            // ========================================
            ("org.dept.d0110", "zh-CN", "总务课", "二级部门"),
            ("org.dept.d0110", "en-US", "General Affairs Section", "Level 2 Dept"),
            ("org.dept.d0110", "ja-JP", "総務課", "レベル2部門"),
            ("org.dept.d0110", "zh-HK", "總務課", "二级部门"),

            // ========================================
            // 财务部下级（D0210）
            // ========================================
            ("org.dept.d0210", "zh-CN", "财务课", "二级部门"),
            ("org.dept.d0210", "en-US", "Finance Section", "Level 2 Dept"),
            ("org.dept.d0210", "ja-JP", "財務課", "レベル2部門"),
            ("org.dept.d0210", "zh-HK", "財務課", "二级部门"),

            // ========================================
            // IT部下级（D0310）
            // ========================================
            ("org.dept.d0310", "zh-CN", "电脑课", "二级部门"),
            ("org.dept.d0310", "en-US", "Computer Section", "Level 2 Dept"),
            ("org.dept.d0310", "ja-JP", "電算課", "レベル2部門"),
            ("org.dept.d0310", "zh-HK", "電腦課", "二级部门"),

            // ========================================
            // 管理部下级（D0410～D0430）
            // ========================================
            ("org.dept.d0410", "zh-CN", "报关课", "二级部门"),
            ("org.dept.d0410", "en-US", "Customs Section", "Level 2 Dept"),
            ("org.dept.d0410", "ja-JP", "通関課", "レベル2部門"),
            ("org.dept.d0410", "zh-HK", "報關課", "二级部门"),
            
            ("org.dept.d0420", "zh-CN", "生管课", "二级部门"),
            ("org.dept.d0420", "en-US", "Production Control Section", "Level 2 Dept"),
            ("org.dept.d0420", "ja-JP", "生産管理課", "レベル2部門"),
            ("org.dept.d0420", "zh-HK", "生管課", "二级部门"),
            
            ("org.dept.d0430", "zh-CN", "部管课", "二级部门"),
            ("org.dept.d0430", "en-US", "Department Management Section", "Level 2 Dept"),
            ("org.dept.d0430", "ja-JP", "部門管理課", "レベル2部門"),
            ("org.dept.d0430", "zh-HK", "部管課", "二级部门"),

            // ========================================
            // 资材部下级（D0510）
            // ========================================
            ("org.dept.d0510", "zh-CN", "采购课", "二级部门"),
            ("org.dept.d0510", "en-US", "Purchasing Section", "Level 2 Dept"),
            ("org.dept.d0510", "ja-JP", "購買課", "レベル2部門"),
            ("org.dept.d0510", "zh-HK", "採購課", "二级部门"),

            // ========================================
            // 生产部下级（D0610～D0630）
            // ========================================
            ("org.dept.d0610", "zh-CN", "制造1课", "二级部门"),
            ("org.dept.d0610", "en-US", "Manufacturing Section 1", "Level 2 Dept"),
            ("org.dept.d0610", "ja-JP", "製造1課", "レベル2部門"),
            ("org.dept.d0610", "zh-HK", "製造1課", "二级部门"),
            
            ("org.dept.d0620", "zh-CN", "制造2课", "二级部门"),
            ("org.dept.d0620", "en-US", "Manufacturing Section 2", "Level 2 Dept"),
            ("org.dept.d0620", "ja-JP", "製造2課", "レベル2部門"),
            ("org.dept.d0620", "zh-HK", "製造2課", "二级部门"),
            
            ("org.dept.d0630", "zh-CN", "制造技术课", "二级部门"),
            ("org.dept.d0630", "en-US", "Manufacturing Technology Section", "Level 2 Dept"),
            ("org.dept.d0630", "ja-JP", "製造技術課", "レベル2部門"),
            ("org.dept.d0630", "zh-HK", "製造技術課", "二级部门"),

            // ========================================
            // 制造2课下级（D0621～D0626）
            // ========================================
            ("org.dept.d0621", "zh-CN", "SMT", "三级部门"),
            ("org.dept.d0621", "en-US", "SMT", "Level 3 Dept"),
            ("org.dept.d0621", "ja-JP", "SMT", "レベル3部門"),
            ("org.dept.d0621", "zh-HK", "SMT", "三级部门"),
            
            ("org.dept.d0622", "zh-CN", "自插", "三级部门"),
            ("org.dept.d0622", "en-US", "Auto Insertion", "Level 3 Dept"),
            ("org.dept.d0622", "ja-JP", "自動挿入", "レベル3部門"),
            ("org.dept.d0622", "zh-HK", "自插", "三级部门"),
            
            ("org.dept.d0623", "zh-CN", "修正", "三级部门"),
            ("org.dept.d0623", "en-US", "Rework", "Level 3 Dept"),
            ("org.dept.d0623", "ja-JP", "修正", "レベル3部門"),
            ("org.dept.d0623", "zh-HK", "修正", "三级部门"),
            
            ("org.dept.d0624", "zh-CN", "手插", "三级部门"),
            ("org.dept.d0624", "en-US", "Manual Insertion", "Level 3 Dept"),
            ("org.dept.d0624", "ja-JP", "手動挿入", "レベル3部門"),
            ("org.dept.d0624", "zh-HK", "手插", "三级部门"),
            
            ("org.dept.d0625", "zh-CN", "物料", "三级部门"),
            ("org.dept.d0625", "en-US", "Materials", "Level 3 Dept"),
            ("org.dept.d0625", "ja-JP", "資材", "レベル3部門"),
            ("org.dept.d0625", "zh-HK", "物料", "三级部门"),
            
            ("org.dept.d0626", "zh-CN", "制造2课-间接", "三级部门"),
            ("org.dept.d0626", "en-US", "Manufacturing Section 2 - Indirect", "Level 3 Dept"),
            ("org.dept.d0626", "ja-JP", "製造2課-間接", "レベル3部門"),
            ("org.dept.d0626", "zh-HK", "製造2課-間接", "三级部门"),

            // ========================================
            // 技术部下级（D0710）
            // ========================================
            ("org.dept.d0710", "zh-CN", "技术课", "二级部门"),
            ("org.dept.d0710", "en-US", "Technology Section", "Level 2 Dept"),
            ("org.dept.d0710", "ja-JP", "技術課", "レベル2部門"),
            ("org.dept.d0710", "zh-HK", "技術課", "二级部门"),

            // ========================================
            // 品保部下级（D0810～D0820）
            // ========================================
            ("org.dept.d0810", "zh-CN", "受检课", "二级部门"),
            ("org.dept.d0810", "en-US", "Incoming Inspection Section", "Level 2 Dept"),
            ("org.dept.d0810", "ja-JP", "受入検査課", "レベル2部門"),
            ("org.dept.d0810", "zh-HK", "受檢課", "二级部门"),
            
            ("org.dept.d0820", "zh-CN", "品管课", "二级部门"),
            ("org.dept.d0820", "en-US", "Quality Control Section", "Level 2 Dept"),
            ("org.dept.d0820", "ja-JP", "品質管理課", "レベル2部門"),
            ("org.dept.d0820", "zh-HK", "品管課", "二级部门"),

            // ========================================
            // OEM部下级（D0910～D0920）
            // ========================================
            ("org.dept.d0910", "zh-CN", "OEM QA课", "二级部门"),
            ("org.dept.d0910", "en-US", "OEM QA Section", "Level 2 Dept"),
            ("org.dept.d0910", "ja-JP", "OEM QA課", "レベル2部門"),
            ("org.dept.d0910", "zh-HK", "OEM QA課", "二级部门"),
            
            ("org.dept.d0920", "zh-CN", "OEM管理课", "二级部门"),
            ("org.dept.d0920", "en-US", "OEM Management Section", "Level 2 Dept"),
            ("org.dept.d0920", "ja-JP", "OEM管理課", "レベル2部門"),
            ("org.dept.d0920", "zh-HK", "OEM管理課", "二级部门"),
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
        translation.ResourceGroup = TaktModule.HumanResource;
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
