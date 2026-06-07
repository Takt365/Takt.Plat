// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktDictDataI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDictData 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktDictData 实体国际化翻译种子（键前缀 entity.dictData.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDictDataI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDictData 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 dictData 实体翻译...", tenantCode);

        foreach (var item in GetDictDataTranslations())
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

        TaktLogger.Information("TaktDictData 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDictData 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.dictData._self / entity.dictData.{{field}}；ResourceGroup=TaktModule.Foundation；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDictDataTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.dictData._self
            new TranslationSeedItem("entity.dictData._self", "en-US", "Dict Data Information", "实体名称"),
            // entity.dictData._self
            new TranslationSeedItem("entity.dictData._self", "ja-JP", "字典数据信息", "实体名称"),
            // entity.dictData._self
            new TranslationSeedItem("entity.dictData._self", "zh-CN", "字典数据信息", "实体名称"),
            // entity.dictData._self
            new TranslationSeedItem("entity.dictData._self", "zh-HK", "字典数据信息", "实体名称"),

            // entity.dictData.dicttypeid
            new TranslationSeedItem("entity.dictData.dicttypeid", "en-US", "字典类型ID", "字典类型ID（关联 TaktDictType.Id；唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique）"),
            // entity.dictData.dicttypeid
            new TranslationSeedItem("entity.dictData.dicttypeid", "ja-JP", "字典类型ID", "字典类型ID（关联 TaktDictType.Id；唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique）"),
            // entity.dictData.dicttypeid
            new TranslationSeedItem("entity.dictData.dicttypeid", "zh-CN", "字典类型ID", "字典类型ID（关联 TaktDictType.Id；唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique）"),
            // entity.dictData.dicttypeid
            new TranslationSeedItem("entity.dictData.dicttypeid", "zh-HK", "字典类型ID", "字典类型ID（关联 TaktDictType.Id；唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique）"),

            // entity.dictData.dicttypecode
            new TranslationSeedItem("entity.dictData.dicttypecode", "en-US", "字典类型编码", "字典类型编码（关联 TaktDictType.DictTypeCode）"),
            // entity.dictData.dicttypecode
            new TranslationSeedItem("entity.dictData.dicttypecode", "ja-JP", "字典类型编码", "字典类型编码（关联 TaktDictType.DictTypeCode）"),
            // entity.dictData.dicttypecode
            new TranslationSeedItem("entity.dictData.dicttypecode", "zh-CN", "字典类型编码", "字典类型编码（关联 TaktDictType.DictTypeCode）"),
            // entity.dictData.dicttypecode
            new TranslationSeedItem("entity.dictData.dicttypecode", "zh-HK", "字典类型编码", "字典类型编码（关联 TaktDictType.DictTypeCode）"),

            // entity.dictData.dictlabel
            new TranslationSeedItem("entity.dictData.dictlabel", "en-US", "字典项标签", "字典项标签（唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique；如：待支付、已完成）"),
            // entity.dictData.dictlabel
            new TranslationSeedItem("entity.dictData.dictlabel", "ja-JP", "字典项标签", "字典项标签（唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique；如：待支付、已完成）"),
            // entity.dictData.dictlabel
            new TranslationSeedItem("entity.dictData.dictlabel", "zh-CN", "字典项标签", "字典项标签（唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique；如：待支付、已完成）"),
            // entity.dictData.dictlabel
            new TranslationSeedItem("entity.dictData.dictlabel", "zh-HK", "字典项标签", "字典项标签（唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique；如：待支付、已完成）"),

            // entity.dictData.dictvalue
            new TranslationSeedItem("entity.dictData.dictvalue", "en-US", "字典项值", "字典项值（实际存储值，如：0, 1, 2）"),
            // entity.dictData.dictvalue
            new TranslationSeedItem("entity.dictData.dictvalue", "ja-JP", "字典项值", "字典项值（实际存储值，如：0, 1, 2）"),
            // entity.dictData.dictvalue
            new TranslationSeedItem("entity.dictData.dictvalue", "zh-CN", "字典项值", "字典项值（实际存储值，如：0, 1, 2）"),
            // entity.dictData.dictvalue
            new TranslationSeedItem("entity.dictData.dictvalue", "zh-HK", "字典项值", "字典项值（实际存储值，如：0, 1, 2）"),

            // entity.dictData.i18nkey
            new TranslationSeedItem("entity.dictData.i18nkey", "en-US", "国际化翻译键", "国际化翻译键（唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique；如：dict.user_type.admin）"),
            // entity.dictData.i18nkey
            new TranslationSeedItem("entity.dictData.i18nkey", "ja-JP", "国际化翻译键", "国际化翻译键（唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique；如：dict.user_type.admin）"),
            // entity.dictData.i18nkey
            new TranslationSeedItem("entity.dictData.i18nkey", "zh-CN", "国际化翻译键", "国际化翻译键（唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique；如：dict.user_type.admin）"),
            // entity.dictData.i18nkey
            new TranslationSeedItem("entity.dictData.i18nkey", "zh-HK", "国际化翻译键", "国际化翻译键（唯一索引：租户内 DictTypeId+DictLabel+I18nKey 唯一，见 ix_dict_data_type_label_i18n_unique；如：dict.user_type.admin）"),

            // entity.dictData.extlabel
            new TranslationSeedItem("entity.dictData.extlabel", "en-US", "扩展标签", "扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）"),
            // entity.dictData.extlabel
            new TranslationSeedItem("entity.dictData.extlabel", "ja-JP", "扩展标签", "扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）"),
            // entity.dictData.extlabel
            new TranslationSeedItem("entity.dictData.extlabel", "zh-CN", "扩展标签", "扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）"),
            // entity.dictData.extlabel
            new TranslationSeedItem("entity.dictData.extlabel", "zh-HK", "扩展标签", "扩展标签（用于存储额外的显示文本，如：副标题、简短描述等）"),

            // entity.dictData.extvalue
            new TranslationSeedItem("entity.dictData.extvalue", "en-US", "扩展值", "扩展值（用于存储额外的业务数据，如：编码、标识符等）"),
            // entity.dictData.extvalue
            new TranslationSeedItem("entity.dictData.extvalue", "ja-JP", "扩展值", "扩展值（用于存储额外的业务数据，如：编码、标识符等）"),
            // entity.dictData.extvalue
            new TranslationSeedItem("entity.dictData.extvalue", "zh-CN", "扩展值", "扩展值（用于存储额外的业务数据，如：编码、标识符等）"),
            // entity.dictData.extvalue
            new TranslationSeedItem("entity.dictData.extvalue", "zh-HK", "扩展值", "扩展值（用于存储额外的业务数据，如：编码、标识符等）"),

            // entity.dictData.listclass
            new TranslationSeedItem("entity.dictData.listclass", "en-US", "列表样式类", "列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识"),
            // entity.dictData.listclass
            new TranslationSeedItem("entity.dictData.listclass", "ja-JP", "列表样式类", "列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识"),
            // entity.dictData.listclass
            new TranslationSeedItem("entity.dictData.listclass", "zh-CN", "列表样式类", "列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识"),
            // entity.dictData.listclass
            new TranslationSeedItem("entity.dictData.listclass", "zh-HK", "列表样式类", "列表样式类（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于下拉列表选项中显示的颜色标识"),

            // entity.dictData.cssclass
            new TranslationSeedItem("entity.dictData.cssclass", "en-US", "CSS类名", "CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签"),
            // entity.dictData.cssclass
            new TranslationSeedItem("entity.dictData.cssclass", "ja-JP", "CSS类名", "CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签"),
            // entity.dictData.cssclass
            new TranslationSeedItem("entity.dictData.cssclass", "zh-CN", "CSS类名", "CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签"),
            // entity.dictData.cssclass
            new TranslationSeedItem("entity.dictData.cssclass", "zh-HK", "CSS类名", "CSS 类名（0=默认, 1=primary, 2=success, 3=warning, 4=danger, 5=info） 用于数据表格中字典值显示的颜色标签"),

            // entity.dictData.sortorder
            new TranslationSeedItem("entity.dictData.sortorder", "en-US", "排序号", "排序号"),
            // entity.dictData.sortorder
            new TranslationSeedItem("entity.dictData.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.dictData.sortorder
            new TranslationSeedItem("entity.dictData.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.dictData.sortorder
            new TranslationSeedItem("entity.dictData.sortorder", "zh-HK", "排序号", "排序号"),

            // entity.dictData.dicttype
            new TranslationSeedItem("entity.dictData.dicttype", "en-US", "dictType", "字典类型（多对一关联）"),
            // entity.dictData.dicttype
            new TranslationSeedItem("entity.dictData.dicttype", "ja-JP", "dictType", "字典类型（多对一关联）"),
            // entity.dictData.dicttype
            new TranslationSeedItem("entity.dictData.dicttype", "zh-CN", "dictType", "字典类型（多对一关联）"),
            // entity.dictData.dicttype
            new TranslationSeedItem("entity.dictData.dicttype", "zh-HK", "dictType", "字典类型（多对一关联）"),
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
