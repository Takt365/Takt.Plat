// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial
// 文件名称：TaktAccountTitleI18nSeedData.cs
// 创建时间：2026-06-06
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Accounting.Financial;

/// <summary>
/// TaktAccountTitle 实体国际化翻译种子（键前缀 entity.accountTitle.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 accountTitle 实体翻译...", tenantCode);

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
    /// I18nKey：entity.accountTitle._self / entity.accountTitle.{{field}}；ResourceGroup=TaktModule.Accounting；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetAccountTitleTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.accountTitle._self
            new TranslationSeedItem("entity.accountTitle._self", "en-US", "Account Title Information", "实体名称"),
            // entity.accountTitle._self
            new TranslationSeedItem("entity.accountTitle._self", "ja-JP", "会计科目信息", "实体名称"),
            // entity.accountTitle._self
            new TranslationSeedItem("entity.accountTitle._self", "zh-CN", "会计科目信息", "实体名称"),
            // entity.accountTitle._self
            new TranslationSeedItem("entity.accountTitle._self", "zh-HK", "会计科目信息", "实体名称"),

            // entity.accountTitle.titlecode
            new TranslationSeedItem("entity.accountTitle.titlecode", "en-US", "科目编码", "科目编码"),
            // entity.accountTitle.titlecode
            new TranslationSeedItem("entity.accountTitle.titlecode", "ja-JP", "科目编码", "科目编码"),
            // entity.accountTitle.titlecode
            new TranslationSeedItem("entity.accountTitle.titlecode", "zh-CN", "科目编码", "科目编码"),
            // entity.accountTitle.titlecode
            new TranslationSeedItem("entity.accountTitle.titlecode", "zh-HK", "科目编码", "科目编码"),

            // entity.accountTitle.titlename
            new TranslationSeedItem("entity.accountTitle.titlename", "en-US", "科目名称", "科目名称"),
            // entity.accountTitle.titlename
            new TranslationSeedItem("entity.accountTitle.titlename", "ja-JP", "科目名称", "科目名称"),
            // entity.accountTitle.titlename
            new TranslationSeedItem("entity.accountTitle.titlename", "zh-CN", "科目名称", "科目名称"),
            // entity.accountTitle.titlename
            new TranslationSeedItem("entity.accountTitle.titlename", "zh-HK", "科目名称", "科目名称"),

            // entity.accountTitle.parentid
            new TranslationSeedItem("entity.accountTitle.parentid", "en-US", "父级ID", "父级 ID"),
            // entity.accountTitle.parentid
            new TranslationSeedItem("entity.accountTitle.parentid", "ja-JP", "父级ID", "父级 ID"),
            // entity.accountTitle.parentid
            new TranslationSeedItem("entity.accountTitle.parentid", "zh-CN", "父级ID", "父级 ID"),
            // entity.accountTitle.parentid
            new TranslationSeedItem("entity.accountTitle.parentid", "zh-HK", "父级ID", "父级 ID"),

            // entity.accountTitle.titletype
            new TranslationSeedItem("entity.accountTitle.titletype", "en-US", "科目类型", "科目类型"),
            // entity.accountTitle.titletype
            new TranslationSeedItem("entity.accountTitle.titletype", "ja-JP", "科目类型", "科目类型"),
            // entity.accountTitle.titletype
            new TranslationSeedItem("entity.accountTitle.titletype", "zh-CN", "科目类型", "科目类型"),
            // entity.accountTitle.titletype
            new TranslationSeedItem("entity.accountTitle.titletype", "zh-HK", "科目类型", "科目类型"),

            // entity.accountTitle.balancedirection
            new TranslationSeedItem("entity.accountTitle.balancedirection", "en-US", "余额方向", "余额方向（0=借方，1=贷方）"),
            // entity.accountTitle.balancedirection
            new TranslationSeedItem("entity.accountTitle.balancedirection", "ja-JP", "余额方向", "余额方向（0=借方，1=贷方）"),
            // entity.accountTitle.balancedirection
            new TranslationSeedItem("entity.accountTitle.balancedirection", "zh-CN", "余额方向", "余额方向（0=借方，1=贷方）"),
            // entity.accountTitle.balancedirection
            new TranslationSeedItem("entity.accountTitle.balancedirection", "zh-HK", "余额方向", "余额方向（0=借方，1=贷方）"),

            // entity.accountTitle.titlelevel
            new TranslationSeedItem("entity.accountTitle.titlelevel", "en-US", "科目层级", "科目层级"),
            // entity.accountTitle.titlelevel
            new TranslationSeedItem("entity.accountTitle.titlelevel", "ja-JP", "科目层级", "科目层级"),
            // entity.accountTitle.titlelevel
            new TranslationSeedItem("entity.accountTitle.titlelevel", "zh-CN", "科目层级", "科目层级"),
            // entity.accountTitle.titlelevel
            new TranslationSeedItem("entity.accountTitle.titlelevel", "zh-HK", "科目层级", "科目层级"),

            // entity.accountTitle.isleaf
            new TranslationSeedItem("entity.accountTitle.isleaf", "en-US", "是否末级科目", "是否末级科目"),
            // entity.accountTitle.isleaf
            new TranslationSeedItem("entity.accountTitle.isleaf", "ja-JP", "是否末级科目", "是否末级科目"),
            // entity.accountTitle.isleaf
            new TranslationSeedItem("entity.accountTitle.isleaf", "zh-CN", "是否末级科目", "是否末级科目"),
            // entity.accountTitle.isleaf
            new TranslationSeedItem("entity.accountTitle.isleaf", "zh-HK", "是否末级科目", "是否末级科目"),

            // entity.accountTitle.isauxiliary
            new TranslationSeedItem("entity.accountTitle.isauxiliary", "en-US", "是否辅助核算", "是否辅助核算"),
            // entity.accountTitle.isauxiliary
            new TranslationSeedItem("entity.accountTitle.isauxiliary", "ja-JP", "是否辅助核算", "是否辅助核算"),
            // entity.accountTitle.isauxiliary
            new TranslationSeedItem("entity.accountTitle.isauxiliary", "zh-CN", "是否辅助核算", "是否辅助核算"),
            // entity.accountTitle.isauxiliary
            new TranslationSeedItem("entity.accountTitle.isauxiliary", "zh-HK", "是否辅助核算", "是否辅助核算"),

            // entity.accountTitle.auxiliarytype
            new TranslationSeedItem("entity.accountTitle.auxiliarytype", "en-US", "辅助核算类型", "辅助核算类型"),
            // entity.accountTitle.auxiliarytype
            new TranslationSeedItem("entity.accountTitle.auxiliarytype", "ja-JP", "辅助核算类型", "辅助核算类型"),
            // entity.accountTitle.auxiliarytype
            new TranslationSeedItem("entity.accountTitle.auxiliarytype", "zh-CN", "辅助核算类型", "辅助核算类型"),
            // entity.accountTitle.auxiliarytype
            new TranslationSeedItem("entity.accountTitle.auxiliarytype", "zh-HK", "辅助核算类型", "辅助核算类型"),

            // entity.accountTitle.isquantity
            new TranslationSeedItem("entity.accountTitle.isquantity", "en-US", "是否数量核算", "是否数量核算"),
            // entity.accountTitle.isquantity
            new TranslationSeedItem("entity.accountTitle.isquantity", "ja-JP", "是否数量核算", "是否数量核算"),
            // entity.accountTitle.isquantity
            new TranslationSeedItem("entity.accountTitle.isquantity", "zh-CN", "是否数量核算", "是否数量核算"),
            // entity.accountTitle.isquantity
            new TranslationSeedItem("entity.accountTitle.isquantity", "zh-HK", "是否数量核算", "是否数量核算"),

            // entity.accountTitle.iscurrency
            new TranslationSeedItem("entity.accountTitle.iscurrency", "en-US", "是否外币核算", "是否外币核算"),
            // entity.accountTitle.iscurrency
            new TranslationSeedItem("entity.accountTitle.iscurrency", "ja-JP", "是否外币核算", "是否外币核算"),
            // entity.accountTitle.iscurrency
            new TranslationSeedItem("entity.accountTitle.iscurrency", "zh-CN", "是否外币核算", "是否外币核算"),
            // entity.accountTitle.iscurrency
            new TranslationSeedItem("entity.accountTitle.iscurrency", "zh-HK", "是否外币核算", "是否外币核算"),

            // entity.accountTitle.iscash
            new TranslationSeedItem("entity.accountTitle.iscash", "en-US", "是否现金科目", "是否现金科目"),
            // entity.accountTitle.iscash
            new TranslationSeedItem("entity.accountTitle.iscash", "ja-JP", "是否现金科目", "是否现金科目"),
            // entity.accountTitle.iscash
            new TranslationSeedItem("entity.accountTitle.iscash", "zh-CN", "是否现金科目", "是否现金科目"),
            // entity.accountTitle.iscash
            new TranslationSeedItem("entity.accountTitle.iscash", "zh-HK", "是否现金科目", "是否现金科目"),

            // entity.accountTitle.isbank
            new TranslationSeedItem("entity.accountTitle.isbank", "en-US", "是否银行科目", "是否银行科目"),
            // entity.accountTitle.isbank
            new TranslationSeedItem("entity.accountTitle.isbank", "ja-JP", "是否银行科目", "是否银行科目"),
            // entity.accountTitle.isbank
            new TranslationSeedItem("entity.accountTitle.isbank", "zh-CN", "是否银行科目", "是否银行科目"),
            // entity.accountTitle.isbank
            new TranslationSeedItem("entity.accountTitle.isbank", "zh-HK", "是否银行科目", "是否银行科目"),

            // entity.accountTitle.relatedplant
            new TranslationSeedItem("entity.accountTitle.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.accountTitle.relatedplant
            new TranslationSeedItem("entity.accountTitle.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.accountTitle.relatedplant
            new TranslationSeedItem("entity.accountTitle.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.accountTitle.relatedplant
            new TranslationSeedItem("entity.accountTitle.relatedplant", "zh-HK", "关联工厂", "关联工厂"),

            // entity.accountTitle.titlestatus
            new TranslationSeedItem("entity.accountTitle.titlestatus", "en-US", "科目状态", "科目状态（1=启用，0=禁用）"),
            // entity.accountTitle.titlestatus
            new TranslationSeedItem("entity.accountTitle.titlestatus", "ja-JP", "科目状态", "科目状态（1=启用，0=禁用）"),
            // entity.accountTitle.titlestatus
            new TranslationSeedItem("entity.accountTitle.titlestatus", "zh-CN", "科目状态", "科目状态（1=启用，0=禁用）"),
            // entity.accountTitle.titlestatus
            new TranslationSeedItem("entity.accountTitle.titlestatus", "zh-HK", "科目状态", "科目状态（1=启用，0=禁用）"),

            // entity.accountTitle.validfrom
            new TranslationSeedItem("entity.accountTitle.validfrom", "en-US", "生效日期", "生效日期"),
            // entity.accountTitle.validfrom
            new TranslationSeedItem("entity.accountTitle.validfrom", "ja-JP", "生效日期", "生效日期"),
            // entity.accountTitle.validfrom
            new TranslationSeedItem("entity.accountTitle.validfrom", "zh-CN", "生效日期", "生效日期"),
            // entity.accountTitle.validfrom
            new TranslationSeedItem("entity.accountTitle.validfrom", "zh-HK", "生效日期", "生效日期"),

            // entity.accountTitle.validto
            new TranslationSeedItem("entity.accountTitle.validto", "en-US", "失效日期", "失效日期"),
            // entity.accountTitle.validto
            new TranslationSeedItem("entity.accountTitle.validto", "ja-JP", "失效日期", "失效日期"),
            // entity.accountTitle.validto
            new TranslationSeedItem("entity.accountTitle.validto", "zh-CN", "失效日期", "失效日期"),
            // entity.accountTitle.validto
            new TranslationSeedItem("entity.accountTitle.validto", "zh-HK", "失效日期", "失效日期"),

            // entity.accountTitle.sortorder
            new TranslationSeedItem("entity.accountTitle.sortorder", "en-US", "排序号", "排序号"),
            // entity.accountTitle.sortorder
            new TranslationSeedItem("entity.accountTitle.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.accountTitle.sortorder
            new TranslationSeedItem("entity.accountTitle.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.accountTitle.sortorder
            new TranslationSeedItem("entity.accountTitle.sortorder", "zh-HK", "排序号", "排序号"),
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
        translation.ResourceGroup = TaktModule.Accounting;
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
