// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizougijutsuI18nSeedData.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcSeizougijutsu 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcSeizougijutsu 实体国际化翻译种子（键前缀 entity.ecseizougijutsu.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcSeizougijutsuI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcSeizougijutsu 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecseizougijutsu 实体翻译...", tenantCode);

        foreach (var item in GetEcSeizougijutsuTranslations())
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

        TaktLogger.Information("TaktEcSeizougijutsu 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcSeizougijutsu 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecseizougijutsu._self / entity.ecseizougijutsu.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcSeizougijutsuTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecseizougijutsu._self
            new TranslationSeedItem("entity.ecseizougijutsu._self", "en-US", "Ec Seizougijutsu Information_us", "实体名称"),
            // entity.ecseizougijutsu._self
            new TranslationSeedItem("entity.ecseizougijutsu._self", "ja-JP", "设变制造技术课信息_jp", "实体名称"),
            // entity.ecseizougijutsu._self
            new TranslationSeedItem("entity.ecseizougijutsu._self", "zh-CN", "设变制造技术课信息", "实体名称"),
            // entity.ecseizougijutsu._self
            new TranslationSeedItem("entity.ecseizougijutsu._self", "zh-HK", "设变制造技术课信息_hk", "实体名称"),

            // entity.ecseizougijutsu.ecndetailid
            new TranslationSeedItem("entity.ecseizougijutsu.ecndetailid", "en-US", "设变明细ID_us", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）"),
            // entity.ecseizougijutsu.ecndetailid
            new TranslationSeedItem("entity.ecseizougijutsu.ecndetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）"),
            // entity.ecseizougijutsu.ecndetailid
            new TranslationSeedItem("entity.ecseizougijutsu.ecndetailid", "zh-CN", "设变明细ID", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）"),
            // entity.ecseizougijutsu.ecndetailid
            new TranslationSeedItem("entity.ecseizougijutsu.ecndetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizougijutsu 导航）"),

            // entity.ecseizougijutsu.eccode
            new TranslationSeedItem("entity.ecseizougijutsu.eccode", "en-US", "设变单号_us", "设变单号（冗余，便于查询）"),
            // entity.ecseizougijutsu.eccode
            new TranslationSeedItem("entity.ecseizougijutsu.eccode", "ja-JP", "设变单号_jp", "设变单号（冗余，便于查询）"),
            // entity.ecseizougijutsu.eccode
            new TranslationSeedItem("entity.ecseizougijutsu.eccode", "zh-CN", "设变单号", "设变单号（冗余，便于查询）"),
            // entity.ecseizougijutsu.eccode
            new TranslationSeedItem("entity.ecseizougijutsu.eccode", "zh-HK", "设变单号_hk", "设变单号（冗余，便于查询）"),

            // entity.ecseizougijutsu.linenumber
            new TranslationSeedItem("entity.ecseizougijutsu.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizougijutsu.linenumber
            new TranslationSeedItem("entity.ecseizougijutsu.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizougijutsu.linenumber
            new TranslationSeedItem("entity.ecseizougijutsu.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizougijutsu.linenumber
            new TranslationSeedItem("entity.ecseizougijutsu.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecseizougijutsu.deptcode
            new TranslationSeedItem("entity.ecseizougijutsu.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode，5 位，如 D0630）"),
            // entity.ecseizougijutsu.deptcode
            new TranslationSeedItem("entity.ecseizougijutsu.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode，5 位，如 D0630）"),
            // entity.ecseizougijutsu.deptcode
            new TranslationSeedItem("entity.ecseizougijutsu.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode，5 位，如 D0630）"),
            // entity.ecseizougijutsu.deptcode
            new TranslationSeedItem("entity.ecseizougijutsu.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode，5 位，如 D0630）"),

            // entity.ecseizougijutsu.isimplemented
            new TranslationSeedItem("entity.ecseizougijutsu.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizougijutsu.isimplemented
            new TranslationSeedItem("entity.ecseizougijutsu.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizougijutsu.isimplemented
            new TranslationSeedItem("entity.ecseizougijutsu.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizougijutsu.isimplemented
            new TranslationSeedItem("entity.ecseizougijutsu.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecseizougijutsu.execcontent
            new TranslationSeedItem("entity.ecseizougijutsu.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.ecseizougijutsu.execcontent
            new TranslationSeedItem("entity.ecseizougijutsu.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.ecseizougijutsu.execcontent
            new TranslationSeedItem("entity.ecseizougijutsu.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.ecseizougijutsu.execcontent
            new TranslationSeedItem("entity.ecseizougijutsu.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.ecseizougijutsu.confirmationdate
            new TranslationSeedItem("entity.ecseizougijutsu.confirmationdate", "en-US", "确认日期_us", "确认日期"),
            // entity.ecseizougijutsu.confirmationdate
            new TranslationSeedItem("entity.ecseizougijutsu.confirmationdate", "ja-JP", "确认日期_jp", "确认日期"),
            // entity.ecseizougijutsu.confirmationdate
            new TranslationSeedItem("entity.ecseizougijutsu.confirmationdate", "zh-CN", "确认日期", "确认日期"),
            // entity.ecseizougijutsu.confirmationdate
            new TranslationSeedItem("entity.ecseizougijutsu.confirmationdate", "zh-HK", "确认日期_hk", "确认日期"),

            // entity.ecseizougijutsu.issopupdated
            new TranslationSeedItem("entity.ecseizougijutsu.issopupdated", "en-US", "是否更新SOP_us", "是否更新 SOP（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizougijutsu.issopupdated
            new TranslationSeedItem("entity.ecseizougijutsu.issopupdated", "ja-JP", "是否更新SOP_jp", "是否更新 SOP（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizougijutsu.issopupdated
            new TranslationSeedItem("entity.ecseizougijutsu.issopupdated", "zh-CN", "是否更新SOP", "是否更新 SOP（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizougijutsu.issopupdated
            new TranslationSeedItem("entity.ecseizougijutsu.issopupdated", "zh-HK", "是否更新SOP_hk", "是否更新 SOP（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecseizougijutsu.isobsolete
            new TranslationSeedItem("entity.ecseizougijutsu.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizougijutsu.isobsolete
            new TranslationSeedItem("entity.ecseizougijutsu.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizougijutsu.isobsolete
            new TranslationSeedItem("entity.ecseizougijutsu.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizougijutsu.isobsolete
            new TranslationSeedItem("entity.ecseizougijutsu.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
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
        translation.ResourceGroup = "EngineeringChange";
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
