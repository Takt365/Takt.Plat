// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Report
// 文件名称：TaktConfigurableI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktConfigurable 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Report;

/// <summary>
/// TaktConfigurable 实体国际化翻译种子（键前缀 entity.configurable.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktConfigurableI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktConfigurable 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 configurable 实体翻译...", tenantCode);

        foreach (var item in GetConfigurableTranslations())
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

        TaktLogger.Information("TaktConfigurable 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktConfigurable 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.configurable._self / entity.configurable.{{field}}；ResourceGroup=9；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetConfigurableTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.configurable._self
            new TranslationSeedItem("entity.configurable._self", "en-US", "Configurable Information", "实体名称"),
            // entity.configurable._self
            new TranslationSeedItem("entity.configurable._self", "ja-JP", "自定义报表主信息", "实体名称"),
            // entity.configurable._self
            new TranslationSeedItem("entity.configurable._self", "zh-CN", "自定义报表主信息", "实体名称"),
            // entity.configurable._self
            new TranslationSeedItem("entity.configurable._self", "zh-HK", "自定义报表主信息", "实体名称"),

            // entity.configurable.reportcode
            new TranslationSeedItem("entity.configurable.reportcode", "en-US", "报表编码", "报表编码（租户+公司内唯一）"),
            // entity.configurable.reportcode
            new TranslationSeedItem("entity.configurable.reportcode", "ja-JP", "报表编码", "报表编码（租户+公司内唯一）"),
            // entity.configurable.reportcode
            new TranslationSeedItem("entity.configurable.reportcode", "zh-CN", "报表编码", "报表编码（租户+公司内唯一）"),
            // entity.configurable.reportcode
            new TranslationSeedItem("entity.configurable.reportcode", "zh-HK", "报表编码", "报表编码（租户+公司内唯一）"),

            // entity.configurable.reportname
            new TranslationSeedItem("entity.configurable.reportname", "en-US", "报表名称", "报表名称"),
            // entity.configurable.reportname
            new TranslationSeedItem("entity.configurable.reportname", "ja-JP", "报表名称", "报表名称"),
            // entity.configurable.reportname
            new TranslationSeedItem("entity.configurable.reportname", "zh-CN", "报表名称", "报表名称"),
            // entity.configurable.reportname
            new TranslationSeedItem("entity.configurable.reportname", "zh-HK", "报表名称", "报表名称"),

            // entity.configurable.reportdomain
            new TranslationSeedItem("entity.configurable.reportdomain", "en-US", "报表业务域", "报表业务域（财务/人力/后勤等）"),
            // entity.configurable.reportdomain
            new TranslationSeedItem("entity.configurable.reportdomain", "ja-JP", "报表业务域", "报表业务域（财务/人力/后勤等）"),
            // entity.configurable.reportdomain
            new TranslationSeedItem("entity.configurable.reportdomain", "zh-CN", "报表业务域", "报表业务域（财务/人力/后勤等）"),
            // entity.configurable.reportdomain
            new TranslationSeedItem("entity.configurable.reportdomain", "zh-HK", "报表业务域", "报表业务域（财务/人力/后勤等）"),

            // entity.configurable.reportsubcategory
            new TranslationSeedItem("entity.configurable.reportsubcategory", "en-US", "报表子分类", "报表子分类（与菜单末级路由段对齐，如 management、controlling、material）"),
            // entity.configurable.reportsubcategory
            new TranslationSeedItem("entity.configurable.reportsubcategory", "ja-JP", "报表子分类", "报表子分类（与菜单末级路由段对齐，如 management、controlling、material）"),
            // entity.configurable.reportsubcategory
            new TranslationSeedItem("entity.configurable.reportsubcategory", "zh-CN", "报表子分类", "报表子分类（与菜单末级路由段对齐，如 management、controlling、material）"),
            // entity.configurable.reportsubcategory
            new TranslationSeedItem("entity.configurable.reportsubcategory", "zh-HK", "报表子分类", "报表子分类（与菜单末级路由段对齐，如 management、controlling、material）"),

            // entity.configurable.distinctrows
            new TranslationSeedItem("entity.configurable.distinctrows", "en-US", "是否去重", "是否去重行（SELECT DISTINCT）"),
            // entity.configurable.distinctrows
            new TranslationSeedItem("entity.configurable.distinctrows", "ja-JP", "是否去重", "是否去重行（SELECT DISTINCT）"),
            // entity.configurable.distinctrows
            new TranslationSeedItem("entity.configurable.distinctrows", "zh-CN", "是否去重", "是否去重行（SELECT DISTINCT）"),
            // entity.configurable.distinctrows
            new TranslationSeedItem("entity.configurable.distinctrows", "zh-HK", "是否去重", "是否去重行（SELECT DISTINCT）"),

            // entity.configurable.maxexportrows
            new TranslationSeedItem("entity.configurable.maxexportrows", "en-US", "导出最大行数", "单次导出最大行数（Excel 上限，防止 OOM）"),
            // entity.configurable.maxexportrows
            new TranslationSeedItem("entity.configurable.maxexportrows", "ja-JP", "导出最大行数", "单次导出最大行数（Excel 上限，防止 OOM）"),
            // entity.configurable.maxexportrows
            new TranslationSeedItem("entity.configurable.maxexportrows", "zh-CN", "导出最大行数", "单次导出最大行数（Excel 上限，防止 OOM）"),
            // entity.configurable.maxexportrows
            new TranslationSeedItem("entity.configurable.maxexportrows", "zh-HK", "导出最大行数", "单次导出最大行数（Excel 上限，防止 OOM）"),

            // entity.configurable.maxqueryrows
            new TranslationSeedItem("entity.configurable.maxqueryrows", "en-US", "查询最大行数", "单次查询最大行数（预览/分页上限）"),
            // entity.configurable.maxqueryrows
            new TranslationSeedItem("entity.configurable.maxqueryrows", "ja-JP", "查询最大行数", "单次查询最大行数（预览/分页上限）"),
            // entity.configurable.maxqueryrows
            new TranslationSeedItem("entity.configurable.maxqueryrows", "zh-CN", "查询最大行数", "单次查询最大行数（预览/分页上限）"),
            // entity.configurable.maxqueryrows
            new TranslationSeedItem("entity.configurable.maxqueryrows", "zh-HK", "查询最大行数", "单次查询最大行数（预览/分页上限）"),

            // entity.configurable.owneruserid
            new TranslationSeedItem("entity.configurable.owneruserid", "en-US", "归属用户ID", "归属用户 ID（为空表示公司级共享报表）"),
            // entity.configurable.owneruserid
            new TranslationSeedItem("entity.configurable.owneruserid", "ja-JP", "归属用户ID", "归属用户 ID（为空表示公司级共享报表）"),
            // entity.configurable.owneruserid
            new TranslationSeedItem("entity.configurable.owneruserid", "zh-CN", "归属用户ID", "归属用户 ID（为空表示公司级共享报表）"),
            // entity.configurable.owneruserid
            new TranslationSeedItem("entity.configurable.owneruserid", "zh-HK", "归属用户ID", "归属用户 ID（为空表示公司级共享报表）"),

            // entity.configurable.isbuiltin
            new TranslationSeedItem("entity.configurable.isbuiltin", "en-US", "是否内置", "是否内置（内置报表禁止删除）"),
            // entity.configurable.isbuiltin
            new TranslationSeedItem("entity.configurable.isbuiltin", "ja-JP", "是否内置", "是否内置（内置报表禁止删除）"),
            // entity.configurable.isbuiltin
            new TranslationSeedItem("entity.configurable.isbuiltin", "zh-CN", "是否内置", "是否内置（内置报表禁止删除）"),
            // entity.configurable.isbuiltin
            new TranslationSeedItem("entity.configurable.isbuiltin", "zh-HK", "是否内置", "是否内置（内置报表禁止删除）"),

            // entity.configurable.sortorder
            new TranslationSeedItem("entity.configurable.sortorder", "en-US", "排序号", "排序号"),
            // entity.configurable.sortorder
            new TranslationSeedItem("entity.configurable.sortorder", "ja-JP", "排序号", "排序号"),
            // entity.configurable.sortorder
            new TranslationSeedItem("entity.configurable.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.configurable.sortorder
            new TranslationSeedItem("entity.configurable.sortorder", "zh-HK", "排序号", "排序号"),

            // entity.configurable.reportstatus
            new TranslationSeedItem("entity.configurable.reportstatus", "en-US", "报表状态", "报表状态（0=禁用 1=启用）"),
            // entity.configurable.reportstatus
            new TranslationSeedItem("entity.configurable.reportstatus", "ja-JP", "报表状态", "报表状态（0=禁用 1=启用）"),
            // entity.configurable.reportstatus
            new TranslationSeedItem("entity.configurable.reportstatus", "zh-CN", "报表状态", "报表状态（0=禁用 1=启用）"),
            // entity.configurable.reportstatus
            new TranslationSeedItem("entity.configurable.reportstatus", "zh-HK", "报表状态", "报表状态（0=禁用 1=启用）"),

            // entity.configurable.description
            new TranslationSeedItem("entity.configurable.description", "en-US", "报表描述", "报表描述"),
            // entity.configurable.description
            new TranslationSeedItem("entity.configurable.description", "ja-JP", "报表描述", "报表描述"),
            // entity.configurable.description
            new TranslationSeedItem("entity.configurable.description", "zh-CN", "报表描述", "报表描述"),
            // entity.configurable.description
            new TranslationSeedItem("entity.configurable.description", "zh-HK", "报表描述", "报表描述"),

            // entity.configurable.sources
            new TranslationSeedItem("entity.configurable.sources", "en-US", "数据源表列表", "数据源表列表（FROM）"),
            // entity.configurable.sources
            new TranslationSeedItem("entity.configurable.sources", "ja-JP", "数据源表列表", "数据源表列表（FROM）"),
            // entity.configurable.sources
            new TranslationSeedItem("entity.configurable.sources", "zh-CN", "数据源表列表", "数据源表列表（FROM）"),
            // entity.configurable.sources
            new TranslationSeedItem("entity.configurable.sources", "zh-HK", "数据源表列表", "数据源表列表（FROM）"),

            // entity.configurable.joins
            new TranslationSeedItem("entity.configurable.joins", "en-US", "多表关联列表", "多表关联列表（JOIN）"),
            // entity.configurable.joins
            new TranslationSeedItem("entity.configurable.joins", "ja-JP", "多表关联列表", "多表关联列表（JOIN）"),
            // entity.configurable.joins
            new TranslationSeedItem("entity.configurable.joins", "zh-CN", "多表关联列表", "多表关联列表（JOIN）"),
            // entity.configurable.joins
            new TranslationSeedItem("entity.configurable.joins", "zh-HK", "多表关联列表", "多表关联列表（JOIN）"),

            // entity.configurable.fields
            new TranslationSeedItem("entity.configurable.fields", "en-US", "输出字段列表", "输出字段列表（SELECT）"),
            // entity.configurable.fields
            new TranslationSeedItem("entity.configurable.fields", "ja-JP", "输出字段列表", "输出字段列表（SELECT）"),
            // entity.configurable.fields
            new TranslationSeedItem("entity.configurable.fields", "zh-CN", "输出字段列表", "输出字段列表（SELECT）"),
            // entity.configurable.fields
            new TranslationSeedItem("entity.configurable.fields", "zh-HK", "输出字段列表", "输出字段列表（SELECT）"),

            // entity.configurable.selections
            new TranslationSeedItem("entity.configurable.selections", "en-US", "筛选条件列表", "筛选条件列表（Selection Screen / WHERE）"),
            // entity.configurable.selections
            new TranslationSeedItem("entity.configurable.selections", "ja-JP", "筛选条件列表", "筛选条件列表（Selection Screen / WHERE）"),
            // entity.configurable.selections
            new TranslationSeedItem("entity.configurable.selections", "zh-CN", "筛选条件列表", "筛选条件列表（Selection Screen / WHERE）"),
            // entity.configurable.selections
            new TranslationSeedItem("entity.configurable.selections", "zh-HK", "筛选条件列表", "筛选条件列表（Selection Screen / WHERE）"),

            // entity.configurable.groupbys
            new TranslationSeedItem("entity.configurable.groupbys", "en-US", "分组字段列表", "分组字段列表（GROUP BY）"),
            // entity.configurable.groupbys
            new TranslationSeedItem("entity.configurable.groupbys", "ja-JP", "分组字段列表", "分组字段列表（GROUP BY）"),
            // entity.configurable.groupbys
            new TranslationSeedItem("entity.configurable.groupbys", "zh-CN", "分组字段列表", "分组字段列表（GROUP BY）"),
            // entity.configurable.groupbys
            new TranslationSeedItem("entity.configurable.groupbys", "zh-HK", "分组字段列表", "分组字段列表（GROUP BY）"),

            // entity.configurable.orderbys
            new TranslationSeedItem("entity.configurable.orderbys", "en-US", "排序字段列表", "排序字段列表（ORDER BY）"),
            // entity.configurable.orderbys
            new TranslationSeedItem("entity.configurable.orderbys", "ja-JP", "排序字段列表", "排序字段列表（ORDER BY）"),
            // entity.configurable.orderbys
            new TranslationSeedItem("entity.configurable.orderbys", "zh-CN", "排序字段列表", "排序字段列表（ORDER BY）"),
            // entity.configurable.orderbys
            new TranslationSeedItem("entity.configurable.orderbys", "zh-HK", "排序字段列表", "排序字段列表（ORDER BY）"),
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
        translation.ResourceGroup = 9;
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
