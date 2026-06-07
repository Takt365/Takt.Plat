// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Foundation
// 文件名称：TaktNumberingI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktNumbering 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktNumbering 实体国际化翻译种子（键前缀 entity.numbering.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktNumberingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktNumbering 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 numbering 实体翻译...", tenantCode);

        foreach (var item in GetNumberingTranslations())
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

        TaktLogger.Information("TaktNumbering 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktNumbering 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.numbering._self / entity.numbering.{{field}}；ResourceGroup=TaktModule.Foundation；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetNumberingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.numbering._self
            new TranslationSeedItem("entity.numbering._self", "en-US", "Numbering Information", "实体名称"),
            // entity.numbering._self
            new TranslationSeedItem("entity.numbering._self", "ja-JP", "编号规则信息", "实体名称"),
            // entity.numbering._self
            new TranslationSeedItem("entity.numbering._self", "zh-CN", "编号规则信息", "实体名称"),
            // entity.numbering._self
            new TranslationSeedItem("entity.numbering._self", "zh-HK", "编号规则信息", "实体名称"),

            // entity.numbering.rulecode
            new TranslationSeedItem("entity.numbering.rulecode", "en-US", "规则编码", "规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）"),
            // entity.numbering.rulecode
            new TranslationSeedItem("entity.numbering.rulecode", "ja-JP", "规则编码", "规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）"),
            // entity.numbering.rulecode
            new TranslationSeedItem("entity.numbering.rulecode", "zh-CN", "规则编码", "规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）"),
            // entity.numbering.rulecode
            new TranslationSeedItem("entity.numbering.rulecode", "zh-HK", "规则编码", "规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）"),

            // entity.numbering.rulename
            new TranslationSeedItem("entity.numbering.rulename", "en-US", "规则名称", "规则名称（如：销售订单号、采购订单号）"),
            // entity.numbering.rulename
            new TranslationSeedItem("entity.numbering.rulename", "ja-JP", "规则名称", "规则名称（如：销售订单号、采购订单号）"),
            // entity.numbering.rulename
            new TranslationSeedItem("entity.numbering.rulename", "zh-CN", "规则名称", "规则名称（如：销售订单号、采购订单号）"),
            // entity.numbering.rulename
            new TranslationSeedItem("entity.numbering.rulename", "zh-HK", "规则名称", "规则名称（如：销售订单号、采购订单号）"),

            // entity.numbering.documenttype
            new TranslationSeedItem("entity.numbering.documenttype", "en-US", "单据类型", "单据类型"),
            // entity.numbering.documenttype
            new TranslationSeedItem("entity.numbering.documenttype", "ja-JP", "单据类型", "单据类型"),
            // entity.numbering.documenttype
            new TranslationSeedItem("entity.numbering.documenttype", "zh-CN", "单据类型", "单据类型"),
            // entity.numbering.documenttype
            new TranslationSeedItem("entity.numbering.documenttype", "zh-HK", "单据类型", "单据类型"),

            // entity.numbering.departmentcode
            new TranslationSeedItem("entity.numbering.departmentcode", "en-US", "部门编码", "部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode"),
            // entity.numbering.departmentcode
            new TranslationSeedItem("entity.numbering.departmentcode", "ja-JP", "部门编码", "部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode"),
            // entity.numbering.departmentcode
            new TranslationSeedItem("entity.numbering.departmentcode", "zh-CN", "部门编码", "部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode"),
            // entity.numbering.departmentcode
            new TranslationSeedItem("entity.numbering.departmentcode", "zh-HK", "部门编码", "部门编码（如：DEPT01, DEPT02，不可为空） 从 TaktDepartment 实体自动获取 DisplayCode"),

            // entity.numbering.prefix
            new TranslationSeedItem("entity.numbering.prefix", "en-US", "前缀", "前缀（如：SO-, PO-, INV-）"),
            // entity.numbering.prefix
            new TranslationSeedItem("entity.numbering.prefix", "ja-JP", "前缀", "前缀（如：SO-, PO-, INV-）"),
            // entity.numbering.prefix
            new TranslationSeedItem("entity.numbering.prefix", "zh-CN", "前缀", "前缀（如：SO-, PO-, INV-）"),
            // entity.numbering.prefix
            new TranslationSeedItem("entity.numbering.prefix", "zh-HK", "前缀", "前缀（如：SO-, PO-, INV-）"),

            // entity.numbering.dateformat
            new TranslationSeedItem("entity.numbering.dateformat", "en-US", "日期格式", "日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm） 为空表示不使用日期"),
            // entity.numbering.dateformat
            new TranslationSeedItem("entity.numbering.dateformat", "ja-JP", "日期格式", "日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm） 为空表示不使用日期"),
            // entity.numbering.dateformat
            new TranslationSeedItem("entity.numbering.dateformat", "zh-CN", "日期格式", "日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm） 为空表示不使用日期"),
            // entity.numbering.dateformat
            new TranslationSeedItem("entity.numbering.dateformat", "zh-HK", "日期格式", "日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm） 为空表示不使用日期"),

            // entity.numbering.sequencelength
            new TranslationSeedItem("entity.numbering.sequencelength", "en-US", "流水号位数", "流水号位数（3=001, 4=0001, 5=00001, 6=000001）"),
            // entity.numbering.sequencelength
            new TranslationSeedItem("entity.numbering.sequencelength", "ja-JP", "流水号位数", "流水号位数（3=001, 4=0001, 5=00001, 6=000001）"),
            // entity.numbering.sequencelength
            new TranslationSeedItem("entity.numbering.sequencelength", "zh-CN", "流水号位数", "流水号位数（3=001, 4=0001, 5=00001, 6=000001）"),
            // entity.numbering.sequencelength
            new TranslationSeedItem("entity.numbering.sequencelength", "zh-HK", "流水号位数", "流水号位数（3=001, 4=0001, 5=00001, 6=000001）"),

            // entity.numbering.sequencestep
            new TranslationSeedItem("entity.numbering.sequencestep", "en-US", "流水号步长", "流水号步长（每次递增的数值，默认1）"),
            // entity.numbering.sequencestep
            new TranslationSeedItem("entity.numbering.sequencestep", "ja-JP", "流水号步长", "流水号步长（每次递增的数值，默认1）"),
            // entity.numbering.sequencestep
            new TranslationSeedItem("entity.numbering.sequencestep", "zh-CN", "流水号步长", "流水号步长（每次递增的数值，默认1）"),
            // entity.numbering.sequencestep
            new TranslationSeedItem("entity.numbering.sequencestep", "zh-HK", "流水号步长", "流水号步长（每次递增的数值，默认1）"),

            // entity.numbering.suffix
            new TranslationSeedItem("entity.numbering.suffix", "en-US", "后缀", "后缀（如：-CN, -USD, -V2）"),
            // entity.numbering.suffix
            new TranslationSeedItem("entity.numbering.suffix", "ja-JP", "后缀", "后缀（如：-CN, -USD, -V2）"),
            // entity.numbering.suffix
            new TranslationSeedItem("entity.numbering.suffix", "zh-CN", "后缀", "后缀（如：-CN, -USD, -V2）"),
            // entity.numbering.suffix
            new TranslationSeedItem("entity.numbering.suffix", "zh-HK", "后缀", "后缀（如：-CN, -USD, -V2）"),

            // entity.numbering.resetperiod
            new TranslationSeedItem("entity.numbering.resetperiod", "en-US", "重置周期", "重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）"),
            // entity.numbering.resetperiod
            new TranslationSeedItem("entity.numbering.resetperiod", "ja-JP", "重置周期", "重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）"),
            // entity.numbering.resetperiod
            new TranslationSeedItem("entity.numbering.resetperiod", "zh-CN", "重置周期", "重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）"),
            // entity.numbering.resetperiod
            new TranslationSeedItem("entity.numbering.resetperiod", "zh-HK", "重置周期", "重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）"),

            // entity.numbering.currentsequence
            new TranslationSeedItem("entity.numbering.currentsequence", "en-US", "当前流水号", "当前流水号（用于记录下一个流水号值）"),
            // entity.numbering.currentsequence
            new TranslationSeedItem("entity.numbering.currentsequence", "ja-JP", "当前流水号", "当前流水号（用于记录下一个流水号值）"),
            // entity.numbering.currentsequence
            new TranslationSeedItem("entity.numbering.currentsequence", "zh-CN", "当前流水号", "当前流水号（用于记录下一个流水号值）"),
            // entity.numbering.currentsequence
            new TranslationSeedItem("entity.numbering.currentsequence", "zh-HK", "当前流水号", "当前流水号（用于记录下一个流水号值）"),

            // entity.numbering.examplecode
            new TranslationSeedItem("entity.numbering.examplecode", "en-US", "示例编码", "示例编码（自动生成，用于预览规则效果） 如：SO-20250120-000001"),
            // entity.numbering.examplecode
            new TranslationSeedItem("entity.numbering.examplecode", "ja-JP", "示例编码", "示例编码（自动生成，用于预览规则效果） 如：SO-20250120-000001"),
            // entity.numbering.examplecode
            new TranslationSeedItem("entity.numbering.examplecode", "zh-CN", "示例编码", "示例编码（自动生成，用于预览规则效果） 如：SO-20250120-000001"),
            // entity.numbering.examplecode
            new TranslationSeedItem("entity.numbering.examplecode", "zh-HK", "示例编码", "示例编码（自动生成，用于预览规则效果） 如：SO-20250120-000001"),

            // entity.numbering.separator
            new TranslationSeedItem("entity.numbering.separator", "en-US", "分隔符", "分隔符（默认 -，也可用 _ 或 /）"),
            // entity.numbering.separator
            new TranslationSeedItem("entity.numbering.separator", "ja-JP", "分隔符", "分隔符（默认 -，也可用 _ 或 /）"),
            // entity.numbering.separator
            new TranslationSeedItem("entity.numbering.separator", "zh-CN", "分隔符", "分隔符（默认 -，也可用 _ 或 /）"),
            // entity.numbering.separator
            new TranslationSeedItem("entity.numbering.separator", "zh-HK", "分隔符", "分隔符（默认 -，也可用 _ 或 /）"),

            // entity.numbering.status
            new TranslationSeedItem("entity.numbering.status", "en-US", "状态", "状态（1=启用，0=禁用）"),
            // entity.numbering.status
            new TranslationSeedItem("entity.numbering.status", "ja-JP", "状态", "状态（1=启用，0=禁用）"),
            // entity.numbering.status
            new TranslationSeedItem("entity.numbering.status", "zh-CN", "状态", "状态（1=启用，0=禁用）"),
            // entity.numbering.status
            new TranslationSeedItem("entity.numbering.status", "zh-HK", "状态", "状态（1=启用，0=禁用）"),

            // entity.numbering.description
            new TranslationSeedItem("entity.numbering.description", "en-US", "描述说明", "描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,Prefix,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）"),
            // entity.numbering.description
            new TranslationSeedItem("entity.numbering.description", "ja-JP", "描述说明", "描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,Prefix,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）"),
            // entity.numbering.description
            new TranslationSeedItem("entity.numbering.description", "zh-CN", "描述说明", "描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,Prefix,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）"),
            // entity.numbering.description
            new TranslationSeedItem("entity.numbering.description", "zh-HK", "描述说明", "描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,Prefix,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）"),
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
