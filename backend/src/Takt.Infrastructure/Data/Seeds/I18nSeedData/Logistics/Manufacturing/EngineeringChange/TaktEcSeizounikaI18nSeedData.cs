// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcSeizounikaI18nSeedData.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcSeizounika 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcSeizounika 实体国际化翻译种子（键前缀 entity.ecseizounika.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcSeizounikaI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcSeizounika 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecseizounika 实体翻译...", tenantCode);

        foreach (var item in GetEcSeizounikaTranslations())
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

        TaktLogger.Information("TaktEcSeizounika 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcSeizounika 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecseizounika._self / entity.ecseizounika.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcSeizounikaTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecseizounika._self
            new TranslationSeedItem("entity.ecseizounika._self", "en-US", "Ec Seizounika Information_us", "实体名称"),
            // entity.ecseizounika._self
            new TranslationSeedItem("entity.ecseizounika._self", "ja-JP", "设变制造2课-间接信息_jp", "实体名称"),
            // entity.ecseizounika._self
            new TranslationSeedItem("entity.ecseizounika._self", "zh-CN", "设变制造2课-间接信息", "实体名称"),
            // entity.ecseizounika._self
            new TranslationSeedItem("entity.ecseizounika._self", "zh-HK", "设变制造2课-间接信息_hk", "实体名称"),

            // entity.ecseizounika.ecndetailid
            new TranslationSeedItem("entity.ecseizounika.ecndetailid", "en-US", "设变明细ID_us", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizounika 导航）"),
            // entity.ecseizounika.ecndetailid
            new TranslationSeedItem("entity.ecseizounika.ecndetailid", "ja-JP", "设变明细ID_jp", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizounika 导航）"),
            // entity.ecseizounika.ecndetailid
            new TranslationSeedItem("entity.ecseizounika.ecndetailid", "zh-CN", "设变明细ID", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizounika 导航）"),
            // entity.ecseizounika.ecndetailid
            new TranslationSeedItem("entity.ecseizounika.ecndetailid", "zh-HK", "设变明细ID_hk", "设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcSeizounika 导航）"),

            // entity.ecseizounika.ecno
            new TranslationSeedItem("entity.ecseizounika.ecno", "en-US", "设变单号_us", "设变单号（冗余，便于查询）"),
            // entity.ecseizounika.ecno
            new TranslationSeedItem("entity.ecseizounika.ecno", "ja-JP", "设变单号_jp", "设变单号（冗余，便于查询）"),
            // entity.ecseizounika.ecno
            new TranslationSeedItem("entity.ecseizounika.ecno", "zh-CN", "设变单号", "设变单号（冗余，便于查询）"),
            // entity.ecseizounika.ecno
            new TranslationSeedItem("entity.ecseizounika.ecno", "zh-HK", "设变单号_hk", "设变单号（冗余，便于查询）"),

            // entity.ecseizounika.linenumber
            new TranslationSeedItem("entity.ecseizounika.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizounika.linenumber
            new TranslationSeedItem("entity.ecseizounika.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizounika.linenumber
            new TranslationSeedItem("entity.ecseizounika.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecseizounika.linenumber
            new TranslationSeedItem("entity.ecseizounika.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecseizounika.deptcode
            new TranslationSeedItem("entity.ecseizounika.deptcode", "en-US", "部门编码_us", "部门编码（TaktDept.DeptCode，5 位，如 D0626）"),
            // entity.ecseizounika.deptcode
            new TranslationSeedItem("entity.ecseizounika.deptcode", "ja-JP", "部门编码_jp", "部门编码（TaktDept.DeptCode，5 位，如 D0626）"),
            // entity.ecseizounika.deptcode
            new TranslationSeedItem("entity.ecseizounika.deptcode", "zh-CN", "部门编码", "部门编码（TaktDept.DeptCode，5 位，如 D0626）"),
            // entity.ecseizounika.deptcode
            new TranslationSeedItem("entity.ecseizounika.deptcode", "zh-HK", "部门编码_hk", "部门编码（TaktDept.DeptCode，5 位，如 D0626）"),

            // entity.ecseizounika.isimplemented
            new TranslationSeedItem("entity.ecseizounika.isimplemented", "en-US", "实施_us", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizounika.isimplemented
            new TranslationSeedItem("entity.ecseizounika.isimplemented", "ja-JP", "实施_jp", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizounika.isimplemented
            new TranslationSeedItem("entity.ecseizounika.isimplemented", "zh-CN", "实施", "是否实施（0=否 1=是，字典 sys_yes_no）"),
            // entity.ecseizounika.isimplemented
            new TranslationSeedItem("entity.ecseizounika.isimplemented", "zh-HK", "实施_hk", "是否实施（0=否 1=是，字典 sys_yes_no）"),

            // entity.ecseizounika.execcontent
            new TranslationSeedItem("entity.ecseizounika.execcontent", "en-US", "执行内容_us", "执行内容（各部门通用）"),
            // entity.ecseizounika.execcontent
            new TranslationSeedItem("entity.ecseizounika.execcontent", "ja-JP", "执行内容_jp", "执行内容（各部门通用）"),
            // entity.ecseizounika.execcontent
            new TranslationSeedItem("entity.ecseizounika.execcontent", "zh-CN", "执行内容", "执行内容（各部门通用）"),
            // entity.ecseizounika.execcontent
            new TranslationSeedItem("entity.ecseizounika.execcontent", "zh-HK", "执行内容_hk", "执行内容（各部门通用）"),

            // entity.ecseizounika.productiondate
            new TranslationSeedItem("entity.ecseizounika.productiondate", "en-US", "生产日期_us", "生产日期"),
            // entity.ecseizounika.productiondate
            new TranslationSeedItem("entity.ecseizounika.productiondate", "ja-JP", "生产日期_jp", "生产日期"),
            // entity.ecseizounika.productiondate
            new TranslationSeedItem("entity.ecseizounika.productiondate", "zh-CN", "生产日期", "生产日期"),
            // entity.ecseizounika.productiondate
            new TranslationSeedItem("entity.ecseizounika.productiondate", "zh-HK", "生产日期_hk", "生产日期"),

            // entity.ecseizounika.productionbatch
            new TranslationSeedItem("entity.ecseizounika.productionbatch", "en-US", "生产批次_us", "生产批次"),
            // entity.ecseizounika.productionbatch
            new TranslationSeedItem("entity.ecseizounika.productionbatch", "ja-JP", "生产批次_jp", "生产批次"),
            // entity.ecseizounika.productionbatch
            new TranslationSeedItem("entity.ecseizounika.productionbatch", "zh-CN", "生产批次", "生产批次"),
            // entity.ecseizounika.productionbatch
            new TranslationSeedItem("entity.ecseizounika.productionbatch", "zh-HK", "生产批次_hk", "生产批次"),

            // entity.ecseizounika.productionteam
            new TranslationSeedItem("entity.ecseizounika.productionteam", "en-US", "生产班组_us", "生产班组"),
            // entity.ecseizounika.productionteam
            new TranslationSeedItem("entity.ecseizounika.productionteam", "ja-JP", "生产班组_jp", "生产班组"),
            // entity.ecseizounika.productionteam
            new TranslationSeedItem("entity.ecseizounika.productionteam", "zh-CN", "生产班组", "生产班组"),
            // entity.ecseizounika.productionteam
            new TranslationSeedItem("entity.ecseizounika.productionteam", "zh-HK", "生产班组_hk", "生产班组"),

            // entity.ecseizounika.outboundorderno
            new TranslationSeedItem("entity.ecseizounika.outboundorderno", "en-US", "出库单号_us", "出库单号"),
            // entity.ecseizounika.outboundorderno
            new TranslationSeedItem("entity.ecseizounika.outboundorderno", "ja-JP", "出库单号_jp", "出库单号"),
            // entity.ecseizounika.outboundorderno
            new TranslationSeedItem("entity.ecseizounika.outboundorderno", "zh-CN", "出库单号", "出库单号"),
            // entity.ecseizounika.outboundorderno
            new TranslationSeedItem("entity.ecseizounika.outboundorderno", "zh-HK", "出库单号_hk", "出库单号"),

            // entity.ecseizounika.isobsolete
            new TranslationSeedItem("entity.ecseizounika.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizounika.isobsolete
            new TranslationSeedItem("entity.ecseizounika.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizounika.isobsolete
            new TranslationSeedItem("entity.ecseizounika.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecseizounika.isobsolete
            new TranslationSeedItem("entity.ecseizounika.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
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
