// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow
// 文件名称：TaktFlowFormI18nSeedData.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFlowForm 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Workflow;

/// <summary>
/// TaktFlowForm 实体国际化翻译种子（键前缀 entity.flowForm.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFlowFormI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFlowForm 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 flowForm 实体翻译...", tenantCode);

        foreach (var item in GetFlowFormTranslations())
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

        TaktLogger.Information("TaktFlowForm 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFlowForm 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.flowForm._self / entity.flowForm.{{field}}；ResourceGroup=TaktModule.Workflow；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFlowFormTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.flowForm._self
            new TranslationSeedItem("entity.flowForm._self", "en-US", "Flow Form Information", "实体名称"),
            // entity.flowForm._self
            new TranslationSeedItem("entity.flowForm._self", "ja-JP", "流程表单定义信息", "实体名称"),
            // entity.flowForm._self
            new TranslationSeedItem("entity.flowForm._self", "zh-CN", "流程表单定义信息", "实体名称"),
            // entity.flowForm._self
            new TranslationSeedItem("entity.flowForm._self", "zh-HK", "流程表单定义信息", "实体名称"),

            // entity.flowForm.formcode
            new TranslationSeedItem("entity.flowForm.formcode", "en-US", "表单编码", "表单编码（公司内唯一）"),
            // entity.flowForm.formcode
            new TranslationSeedItem("entity.flowForm.formcode", "ja-JP", "表单编码", "表单编码（公司内唯一）"),
            // entity.flowForm.formcode
            new TranslationSeedItem("entity.flowForm.formcode", "zh-CN", "表单编码", "表单编码（公司内唯一）"),
            // entity.flowForm.formcode
            new TranslationSeedItem("entity.flowForm.formcode", "zh-HK", "表单编码", "表单编码（公司内唯一）"),

            // entity.flowForm.formname
            new TranslationSeedItem("entity.flowForm.formname", "en-US", "表单名称", "表单名称"),
            // entity.flowForm.formname
            new TranslationSeedItem("entity.flowForm.formname", "ja-JP", "表单名称", "表单名称"),
            // entity.flowForm.formname
            new TranslationSeedItem("entity.flowForm.formname", "zh-CN", "表单名称", "表单名称"),
            // entity.flowForm.formname
            new TranslationSeedItem("entity.flowForm.formname", "zh-HK", "表单名称", "表单名称"),

            // entity.flowForm.formcategory
            new TranslationSeedItem("entity.flowForm.formcategory", "en-US", "表单分类", "表单分类（字典 sys_form_category）"),
            // entity.flowForm.formcategory
            new TranslationSeedItem("entity.flowForm.formcategory", "ja-JP", "表单分类", "表单分类（字典 sys_form_category）"),
            // entity.flowForm.formcategory
            new TranslationSeedItem("entity.flowForm.formcategory", "zh-CN", "表单分类", "表单分类（字典 sys_form_category）"),
            // entity.flowForm.formcategory
            new TranslationSeedItem("entity.flowForm.formcategory", "zh-HK", "表单分类", "表单分类（字典 sys_form_category）"),

            // entity.flowForm.formtype
            new TranslationSeedItem("entity.flowForm.formtype", "en-US", "表单类型", "表单类型（字典 sys_form_type）"),
            // entity.flowForm.formtype
            new TranslationSeedItem("entity.flowForm.formtype", "ja-JP", "表单类型", "表单类型（字典 sys_form_type）"),
            // entity.flowForm.formtype
            new TranslationSeedItem("entity.flowForm.formtype", "zh-CN", "表单类型", "表单类型（字典 sys_form_type）"),
            // entity.flowForm.formtype
            new TranslationSeedItem("entity.flowForm.formtype", "zh-HK", "表单类型", "表单类型（字典 sys_form_type）"),

            // entity.flowForm.formconfig
            new TranslationSeedItem("entity.flowForm.formconfig", "en-US", "表单配置JSON", "表单设计 JSON"),
            // entity.flowForm.formconfig
            new TranslationSeedItem("entity.flowForm.formconfig", "ja-JP", "表单配置JSON", "表单设计 JSON"),
            // entity.flowForm.formconfig
            new TranslationSeedItem("entity.flowForm.formconfig", "zh-CN", "表单配置JSON", "表单设计 JSON"),
            // entity.flowForm.formconfig
            new TranslationSeedItem("entity.flowForm.formconfig", "zh-HK", "表单配置JSON", "表单设计 JSON"),

            // entity.flowForm.formtemplate
            new TranslationSeedItem("entity.flowForm.formtemplate", "en-US", "表单模板JSON", "表单模板 JSON"),
            // entity.flowForm.formtemplate
            new TranslationSeedItem("entity.flowForm.formtemplate", "ja-JP", "表单模板JSON", "表单模板 JSON"),
            // entity.flowForm.formtemplate
            new TranslationSeedItem("entity.flowForm.formtemplate", "zh-CN", "表单模板JSON", "表单模板 JSON"),
            // entity.flowForm.formtemplate
            new TranslationSeedItem("entity.flowForm.formtemplate", "zh-HK", "表单模板JSON", "表单模板 JSON"),

            // entity.flowForm.formversion
            new TranslationSeedItem("entity.flowForm.formversion", "en-US", "表单版本", "表单版本标签"),
            // entity.flowForm.formversion
            new TranslationSeedItem("entity.flowForm.formversion", "ja-JP", "表单版本", "表单版本标签"),
            // entity.flowForm.formversion
            new TranslationSeedItem("entity.flowForm.formversion", "zh-CN", "表单版本", "表单版本标签"),
            // entity.flowForm.formversion
            new TranslationSeedItem("entity.flowForm.formversion", "zh-HK", "表单版本", "表单版本标签"),

            // entity.flowForm.isdatasource
            new TranslationSeedItem("entity.flowForm.isdatasource", "en-US", "是否数据源表单", "是否绑定数据源"),
            // entity.flowForm.isdatasource
            new TranslationSeedItem("entity.flowForm.isdatasource", "ja-JP", "是否数据源表单", "是否绑定数据源"),
            // entity.flowForm.isdatasource
            new TranslationSeedItem("entity.flowForm.isdatasource", "zh-CN", "是否数据源表单", "是否绑定数据源"),
            // entity.flowForm.isdatasource
            new TranslationSeedItem("entity.flowForm.isdatasource", "zh-HK", "是否数据源表单", "是否绑定数据源"),

            // entity.flowForm.relateddatabasename
            new TranslationSeedItem("entity.flowForm.relateddatabasename", "en-US", "关联数据库名", "关联数据库名"),
            // entity.flowForm.relateddatabasename
            new TranslationSeedItem("entity.flowForm.relateddatabasename", "ja-JP", "关联数据库名", "关联数据库名"),
            // entity.flowForm.relateddatabasename
            new TranslationSeedItem("entity.flowForm.relateddatabasename", "zh-CN", "关联数据库名", "关联数据库名"),
            // entity.flowForm.relateddatabasename
            new TranslationSeedItem("entity.flowForm.relateddatabasename", "zh-HK", "关联数据库名", "关联数据库名"),

            // entity.flowForm.relatedtablename
            new TranslationSeedItem("entity.flowForm.relatedtablename", "en-US", "关联表名", "关联表名"),
            // entity.flowForm.relatedtablename
            new TranslationSeedItem("entity.flowForm.relatedtablename", "ja-JP", "关联表名", "关联表名"),
            // entity.flowForm.relatedtablename
            new TranslationSeedItem("entity.flowForm.relatedtablename", "zh-CN", "关联表名", "关联表名"),
            // entity.flowForm.relatedtablename
            new TranslationSeedItem("entity.flowForm.relatedtablename", "zh-HK", "关联表名", "关联表名"),

            // entity.flowForm.relatedformfield
            new TranslationSeedItem("entity.flowForm.relatedformfield", "en-US", "关联字段映射", "关联字段映射 JSON"),
            // entity.flowForm.relatedformfield
            new TranslationSeedItem("entity.flowForm.relatedformfield", "ja-JP", "关联字段映射", "关联字段映射 JSON"),
            // entity.flowForm.relatedformfield
            new TranslationSeedItem("entity.flowForm.relatedformfield", "zh-CN", "关联字段映射", "关联字段映射 JSON"),
            // entity.flowForm.relatedformfield
            new TranslationSeedItem("entity.flowForm.relatedformfield", "zh-HK", "关联字段映射", "关联字段映射 JSON"),

            // entity.flowForm.sortorder
            new TranslationSeedItem("entity.flowForm.sortorder", "en-US", "排序号", "排序号"),
            // entity.flowForm.sortorder
            new TranslationSeedItem("entity.flowForm.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.flowForm.sortorder
            new TranslationSeedItem("entity.flowForm.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.flowForm.sortorder
            new TranslationSeedItem("entity.flowForm.sortorder", "zh-HK", "排序号", "排序号"),

            // entity.flowForm.formstatus
            new TranslationSeedItem("entity.flowForm.formstatus", "en-US", "表单状态", "表单状态"),
            // entity.flowForm.formstatus
            new TranslationSeedItem("entity.flowForm.formstatus", "ja-JP", "表单状态", "表单状态"),
            // entity.flowForm.formstatus
            new TranslationSeedItem("entity.flowForm.formstatus", "zh-CN", "表单状态", "表单状态"),
            // entity.flowForm.formstatus
            new TranslationSeedItem("entity.flowForm.formstatus", "zh-HK", "表单状态", "表单状态"),
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
        translation.ResourceGroup = TaktModule.Workflow;
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
