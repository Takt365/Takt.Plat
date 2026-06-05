// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktDictTypeI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDictType 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation;

/// <summary>
/// TaktDictType 实体国际化翻译种子（键前缀 entity.dictType.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDictTypeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDictType 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 dictType 实体翻译...", tenantCode);

        foreach (var item in GetDictTypeTranslations())
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

        TaktLogger.Information("TaktDictType 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDictType 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.dictType._self / entity.dictType.{{field}}；ResourceGroup=TaktModule.Foundation；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDictTypeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.dictType._self
            new TranslationSeedItem("entity.dictType._self", "en-US", "Dict Type Information", "实体名称"),
            // entity.dictType._self
            new TranslationSeedItem("entity.dictType._self", "ja-JP", "字典类型信息", "实体名称"),
            // entity.dictType._self
            new TranslationSeedItem("entity.dictType._self", "zh-CN", "字典类型信息", "实体名称"),
            // entity.dictType._self
            new TranslationSeedItem("entity.dictType._self", "zh-HK", "字典类型信息", "实体名称"),

            // entity.dictType.code
            new TranslationSeedItem("entity.dictType.code", "en-US", "字典类型编码", "字典类型编码（唯一索引：租户内唯一，见 ix_dict_type_code_unique；如 order_status, user_type）"),
            // entity.dictType.code
            new TranslationSeedItem("entity.dictType.code", "ja-JP", "字典类型编码", "字典类型编码（唯一索引：租户内唯一，见 ix_dict_type_code_unique；如 order_status, user_type）"),
            // entity.dictType.code
            new TranslationSeedItem("entity.dictType.code", "zh-CN", "字典类型编码", "字典类型编码（唯一索引：租户内唯一，见 ix_dict_type_code_unique；如 order_status, user_type）"),
            // entity.dictType.code
            new TranslationSeedItem("entity.dictType.code", "zh-HK", "字典类型编码", "字典类型编码（唯一索引：租户内唯一，见 ix_dict_type_code_unique；如 order_status, user_type）"),

            // entity.dictType.name
            new TranslationSeedItem("entity.dictType.name", "en-US", "字典类型名称", "字典类型名称（如：订单状态、用户类型）"),
            // entity.dictType.name
            new TranslationSeedItem("entity.dictType.name", "ja-JP", "字典类型名称", "字典类型名称（如：订单状态、用户类型）"),
            // entity.dictType.name
            new TranslationSeedItem("entity.dictType.name", "zh-CN", "字典类型名称", "字典类型名称（如：订单状态、用户类型）"),
            // entity.dictType.name
            new TranslationSeedItem("entity.dictType.name", "zh-HK", "字典类型名称", "字典类型名称（如：订单状态、用户类型）"),

            // entity.dictType.dictscript
            new TranslationSeedItem("entity.dictType.dictscript", "en-US", "动态字典SQL脚本", "动态字典SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder"),
            // entity.dictType.dictscript
            new TranslationSeedItem("entity.dictType.dictscript", "ja-JP", "动态字典SQL脚本", "动态字典SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder"),
            // entity.dictType.dictscript
            new TranslationSeedItem("entity.dictType.dictscript", "zh-CN", "动态字典SQL脚本", "动态字典SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder"),
            // entity.dictType.dictscript
            new TranslationSeedItem("entity.dictType.dictscript", "zh-HK", "动态字典SQL脚本", "动态字典SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder"),

            // entity.dictType.sortorder
            new TranslationSeedItem("entity.dictType.sortorder", "en-US", "排序号", "排序号"),
            // entity.dictType.sortorder
            new TranslationSeedItem("entity.dictType.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.dictType.sortorder
            new TranslationSeedItem("entity.dictType.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.dictType.sortorder
            new TranslationSeedItem("entity.dictType.sortorder", "zh-HK", "排序号", "排序号"),

            // entity.dictType.dictstatus
            new TranslationSeedItem("entity.dictType.dictstatus", "en-US", "状态", "状态（1=启用，0=禁用）"),
            // entity.dictType.dictstatus
            new TranslationSeedItem("entity.dictType.dictstatus", "ja-JP", "状态", "状态（1=启用，0=禁用）"),
            // entity.dictType.dictstatus
            new TranslationSeedItem("entity.dictType.dictstatus", "zh-CN", "状态", "状态（1=启用，0=禁用）"),
            // entity.dictType.dictstatus
            new TranslationSeedItem("entity.dictType.dictstatus", "zh-HK", "状态", "状态（1=启用，0=禁用）"),

            // entity.dictType.dictdatalist
            new TranslationSeedItem("entity.dictType.dictdatalist", "en-US", "dictDataList", "字典数据列表（一对多关联）"),
            // entity.dictType.dictdatalist
            new TranslationSeedItem("entity.dictType.dictdatalist", "ja-JP", "dictDataList", "字典数据列表（一对多关联）"),
            // entity.dictType.dictdatalist
            new TranslationSeedItem("entity.dictType.dictdatalist", "zh-CN", "dictDataList", "字典数据列表（一对多关联）"),
            // entity.dictType.dictdatalist
            new TranslationSeedItem("entity.dictType.dictdatalist", "zh-HK", "dictDataList", "字典数据列表（一对多关联）"),
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
