// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktAccountTitleI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktAccountTitle 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial;

/// <summary>
/// TaktAccountTitle 实体国际化翻译种子（键前缀 entity.accounttitle.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktAccountTitleI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktAccountTitle 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 accounttitle 实体翻译...", tenantCode);

        foreach (var item in GetAccountTitleTranslations())
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

        TaktLogger.Information("TaktAccountTitle 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktAccountTitle 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.accounttitle._self / entity.accounttitle.{{field}}；ResourceGroup=3；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetAccountTitleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.accounttitle._self
            new TranslationSeedItem("entity.accounttitle._self", "en-US", "Account Title Information", "实体名称"),
            // entity.accounttitle._self
            new TranslationSeedItem("entity.accounttitle._self", "ja-JP", "会计科目信息", "实体名称"),
            // entity.accounttitle._self
            new TranslationSeedItem("entity.accounttitle._self", "zh-CN", "会计科目信息", "实体名称"),
            // entity.accounttitle._self
            new TranslationSeedItem("entity.accounttitle._self", "zh-HK", "会计科目信息", "实体名称"),

            // entity.accounttitle.titlecode
            new TranslationSeedItem("entity.accounttitle.titlecode", "en-US", "科目编码", "科目编码"),
            // entity.accounttitle.titlecode
            new TranslationSeedItem("entity.accounttitle.titlecode", "ja-JP", "科目编码", "科目编码"),
            // entity.accounttitle.titlecode
            new TranslationSeedItem("entity.accounttitle.titlecode", "zh-CN", "科目编码", "科目编码"),
            // entity.accounttitle.titlecode
            new TranslationSeedItem("entity.accounttitle.titlecode", "zh-HK", "科目编码", "科目编码"),

            // entity.accounttitle.titlename
            new TranslationSeedItem("entity.accounttitle.titlename", "en-US", "科目名称", "科目名称"),
            // entity.accounttitle.titlename
            new TranslationSeedItem("entity.accounttitle.titlename", "ja-JP", "科目名称", "科目名称"),
            // entity.accounttitle.titlename
            new TranslationSeedItem("entity.accounttitle.titlename", "zh-CN", "科目名称", "科目名称"),
            // entity.accounttitle.titlename
            new TranslationSeedItem("entity.accounttitle.titlename", "zh-HK", "科目名称", "科目名称"),

            // entity.accounttitle.parentid
            new TranslationSeedItem("entity.accounttitle.parentid", "en-US", "父级ID", "父级 ID"),
            // entity.accounttitle.parentid
            new TranslationSeedItem("entity.accounttitle.parentid", "ja-JP", "父级ID", "父级 ID"),
            // entity.accounttitle.parentid
            new TranslationSeedItem("entity.accounttitle.parentid", "zh-CN", "父级ID", "父级 ID"),
            // entity.accounttitle.parentid
            new TranslationSeedItem("entity.accounttitle.parentid", "zh-HK", "父级ID", "父级 ID"),

            // entity.accounttitle.titletype
            new TranslationSeedItem("entity.accounttitle.titletype", "en-US", "科目类型", "科目类型"),
            // entity.accounttitle.titletype
            new TranslationSeedItem("entity.accounttitle.titletype", "ja-JP", "科目类型", "科目类型"),
            // entity.accounttitle.titletype
            new TranslationSeedItem("entity.accounttitle.titletype", "zh-CN", "科目类型", "科目类型"),
            // entity.accounttitle.titletype
            new TranslationSeedItem("entity.accounttitle.titletype", "zh-HK", "科目类型", "科目类型"),

            // entity.accounttitle.balancedirection
            new TranslationSeedItem("entity.accounttitle.balancedirection", "en-US", "余额方向", "余额方向（0=借方，1=贷方）"),
            // entity.accounttitle.balancedirection
            new TranslationSeedItem("entity.accounttitle.balancedirection", "ja-JP", "余额方向", "余额方向（0=借方，1=贷方）"),
            // entity.accounttitle.balancedirection
            new TranslationSeedItem("entity.accounttitle.balancedirection", "zh-CN", "余额方向", "余额方向（0=借方，1=贷方）"),
            // entity.accounttitle.balancedirection
            new TranslationSeedItem("entity.accounttitle.balancedirection", "zh-HK", "余额方向", "余额方向（0=借方，1=贷方）"),

            // entity.accounttitle.titlelevel
            new TranslationSeedItem("entity.accounttitle.titlelevel", "en-US", "科目层级", "科目层级"),
            // entity.accounttitle.titlelevel
            new TranslationSeedItem("entity.accounttitle.titlelevel", "ja-JP", "科目层级", "科目层级"),
            // entity.accounttitle.titlelevel
            new TranslationSeedItem("entity.accounttitle.titlelevel", "zh-CN", "科目层级", "科目层级"),
            // entity.accounttitle.titlelevel
            new TranslationSeedItem("entity.accounttitle.titlelevel", "zh-HK", "科目层级", "科目层级"),

            // entity.accounttitle.isleaf
            new TranslationSeedItem("entity.accounttitle.isleaf", "en-US", "是否末级科目", "是否末级科目"),
            // entity.accounttitle.isleaf
            new TranslationSeedItem("entity.accounttitle.isleaf", "ja-JP", "是否末级科目", "是否末级科目"),
            // entity.accounttitle.isleaf
            new TranslationSeedItem("entity.accounttitle.isleaf", "zh-CN", "是否末级科目", "是否末级科目"),
            // entity.accounttitle.isleaf
            new TranslationSeedItem("entity.accounttitle.isleaf", "zh-HK", "是否末级科目", "是否末级科目"),

            // entity.accounttitle.isauxiliary
            new TranslationSeedItem("entity.accounttitle.isauxiliary", "en-US", "是否辅助核算", "是否辅助核算"),
            // entity.accounttitle.isauxiliary
            new TranslationSeedItem("entity.accounttitle.isauxiliary", "ja-JP", "是否辅助核算", "是否辅助核算"),
            // entity.accounttitle.isauxiliary
            new TranslationSeedItem("entity.accounttitle.isauxiliary", "zh-CN", "是否辅助核算", "是否辅助核算"),
            // entity.accounttitle.isauxiliary
            new TranslationSeedItem("entity.accounttitle.isauxiliary", "zh-HK", "是否辅助核算", "是否辅助核算"),

            // entity.accounttitle.auxiliarytype
            new TranslationSeedItem("entity.accounttitle.auxiliarytype", "en-US", "辅助核算类型", "辅助核算类型"),
            // entity.accounttitle.auxiliarytype
            new TranslationSeedItem("entity.accounttitle.auxiliarytype", "ja-JP", "辅助核算类型", "辅助核算类型"),
            // entity.accounttitle.auxiliarytype
            new TranslationSeedItem("entity.accounttitle.auxiliarytype", "zh-CN", "辅助核算类型", "辅助核算类型"),
            // entity.accounttitle.auxiliarytype
            new TranslationSeedItem("entity.accounttitle.auxiliarytype", "zh-HK", "辅助核算类型", "辅助核算类型"),

            // entity.accounttitle.isquantity
            new TranslationSeedItem("entity.accounttitle.isquantity", "en-US", "是否数量核算", "是否数量核算"),
            // entity.accounttitle.isquantity
            new TranslationSeedItem("entity.accounttitle.isquantity", "ja-JP", "是否数量核算", "是否数量核算"),
            // entity.accounttitle.isquantity
            new TranslationSeedItem("entity.accounttitle.isquantity", "zh-CN", "是否数量核算", "是否数量核算"),
            // entity.accounttitle.isquantity
            new TranslationSeedItem("entity.accounttitle.isquantity", "zh-HK", "是否数量核算", "是否数量核算"),

            // entity.accounttitle.iscurrency
            new TranslationSeedItem("entity.accounttitle.iscurrency", "en-US", "是否外币核算", "是否外币核算"),
            // entity.accounttitle.iscurrency
            new TranslationSeedItem("entity.accounttitle.iscurrency", "ja-JP", "是否外币核算", "是否外币核算"),
            // entity.accounttitle.iscurrency
            new TranslationSeedItem("entity.accounttitle.iscurrency", "zh-CN", "是否外币核算", "是否外币核算"),
            // entity.accounttitle.iscurrency
            new TranslationSeedItem("entity.accounttitle.iscurrency", "zh-HK", "是否外币核算", "是否外币核算"),

            // entity.accounttitle.iscash
            new TranslationSeedItem("entity.accounttitle.iscash", "en-US", "是否现金科目", "是否现金科目"),
            // entity.accounttitle.iscash
            new TranslationSeedItem("entity.accounttitle.iscash", "ja-JP", "是否现金科目", "是否现金科目"),
            // entity.accounttitle.iscash
            new TranslationSeedItem("entity.accounttitle.iscash", "zh-CN", "是否现金科目", "是否现金科目"),
            // entity.accounttitle.iscash
            new TranslationSeedItem("entity.accounttitle.iscash", "zh-HK", "是否现金科目", "是否现金科目"),

            // entity.accounttitle.isbank
            new TranslationSeedItem("entity.accounttitle.isbank", "en-US", "是否银行科目", "是否银行科目"),
            // entity.accounttitle.isbank
            new TranslationSeedItem("entity.accounttitle.isbank", "ja-JP", "是否银行科目", "是否银行科目"),
            // entity.accounttitle.isbank
            new TranslationSeedItem("entity.accounttitle.isbank", "zh-CN", "是否银行科目", "是否银行科目"),
            // entity.accounttitle.isbank
            new TranslationSeedItem("entity.accounttitle.isbank", "zh-HK", "是否银行科目", "是否银行科目"),

            // entity.accounttitle.relatedplant
            new TranslationSeedItem("entity.accounttitle.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.accounttitle.relatedplant
            new TranslationSeedItem("entity.accounttitle.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.accounttitle.relatedplant
            new TranslationSeedItem("entity.accounttitle.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.accounttitle.relatedplant
            new TranslationSeedItem("entity.accounttitle.relatedplant", "zh-HK", "关联工厂", "关联工厂"),

            // entity.accounttitle.titlestatus
            new TranslationSeedItem("entity.accounttitle.titlestatus", "en-US", "科目状态", "科目状态（1=启用，0=禁用）"),
            // entity.accounttitle.titlestatus
            new TranslationSeedItem("entity.accounttitle.titlestatus", "ja-JP", "科目状态", "科目状态（1=启用，0=禁用）"),
            // entity.accounttitle.titlestatus
            new TranslationSeedItem("entity.accounttitle.titlestatus", "zh-CN", "科目状态", "科目状态（1=启用，0=禁用）"),
            // entity.accounttitle.titlestatus
            new TranslationSeedItem("entity.accounttitle.titlestatus", "zh-HK", "科目状态", "科目状态（1=启用，0=禁用）"),

            // entity.accounttitle.validfrom
            new TranslationSeedItem("entity.accounttitle.validfrom", "en-US", "生效日期", "生效日期"),
            // entity.accounttitle.validfrom
            new TranslationSeedItem("entity.accounttitle.validfrom", "ja-JP", "生效日期", "生效日期"),
            // entity.accounttitle.validfrom
            new TranslationSeedItem("entity.accounttitle.validfrom", "zh-CN", "生效日期", "生效日期"),
            // entity.accounttitle.validfrom
            new TranslationSeedItem("entity.accounttitle.validfrom", "zh-HK", "生效日期", "生效日期"),

            // entity.accounttitle.validto
            new TranslationSeedItem("entity.accounttitle.validto", "en-US", "失效日期", "失效日期"),
            // entity.accounttitle.validto
            new TranslationSeedItem("entity.accounttitle.validto", "ja-JP", "失效日期", "失效日期"),
            // entity.accounttitle.validto
            new TranslationSeedItem("entity.accounttitle.validto", "zh-CN", "失效日期", "失效日期"),
            // entity.accounttitle.validto
            new TranslationSeedItem("entity.accounttitle.validto", "zh-HK", "失效日期", "失效日期"),

            // entity.accounttitle.sortorder
            new TranslationSeedItem("entity.accounttitle.sortorder", "en-US", "排序号", "排序号"),
            // entity.accounttitle.sortorder
            new TranslationSeedItem("entity.accounttitle.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.accounttitle.sortorder
            new TranslationSeedItem("entity.accounttitle.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.accounttitle.sortorder
            new TranslationSeedItem("entity.accounttitle.sortorder", "zh-HK", "排序号", "排序号"),
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
        translation.ResourceGroup = 3;
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
