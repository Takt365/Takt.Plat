// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktSamplingSchemeI18nSeedData.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSamplingScheme 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation;

/// <summary>
/// TaktSamplingScheme 实体国际化翻译种子（键前缀 entity.samplingscheme.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSamplingSchemeI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSamplingScheme 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 samplingscheme 实体翻译...", tenantCode);

        foreach (var item in GetSamplingSchemeTranslations())
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

        TaktLogger.Information("TaktSamplingScheme 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSamplingScheme 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.samplingscheme._self / entity.samplingscheme.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetSamplingSchemeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.samplingscheme._self
            new TranslationSeedItem("entity.samplingscheme._self", "en-US", "Sampling Scheme Information", "实体名称"),
            // entity.samplingscheme._self
            new TranslationSeedItem("entity.samplingscheme._self", "ja-JP", "Takt抽样方案信息", "实体名称"),
            // entity.samplingscheme._self
            new TranslationSeedItem("entity.samplingscheme._self", "zh-CN", "Takt抽样方案信息", "实体名称"),
            // entity.samplingscheme._self
            new TranslationSeedItem("entity.samplingscheme._self", "zh-HK", "Takt抽样方案信息", "实体名称"),

            // entity.samplingscheme.plantcode
            new TranslationSeedItem("entity.samplingscheme.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.samplingscheme.plantcode
            new TranslationSeedItem("entity.samplingscheme.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.samplingscheme.plantcode
            new TranslationSeedItem("entity.samplingscheme.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.samplingscheme.plantcode
            new TranslationSeedItem("entity.samplingscheme.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.samplingscheme.code
            new TranslationSeedItem("entity.samplingscheme.code", "en-US", "抽样方案编码", "抽样方案编码（唯一索引）"),
            // entity.samplingscheme.code
            new TranslationSeedItem("entity.samplingscheme.code", "ja-JP", "抽样方案编码", "抽样方案编码（唯一索引）"),
            // entity.samplingscheme.code
            new TranslationSeedItem("entity.samplingscheme.code", "zh-CN", "抽样方案编码", "抽样方案编码（唯一索引）"),
            // entity.samplingscheme.code
            new TranslationSeedItem("entity.samplingscheme.code", "zh-HK", "抽样方案编码", "抽样方案编码（唯一索引）"),

            // entity.samplingscheme.name
            new TranslationSeedItem("entity.samplingscheme.name", "en-US", "抽样方案名称", "抽样方案名称"),
            // entity.samplingscheme.name
            new TranslationSeedItem("entity.samplingscheme.name", "ja-JP", "抽样方案名称", "抽样方案名称"),
            // entity.samplingscheme.name
            new TranslationSeedItem("entity.samplingscheme.name", "zh-CN", "抽样方案名称", "抽样方案名称"),
            // entity.samplingscheme.name
            new TranslationSeedItem("entity.samplingscheme.name", "zh-HK", "抽样方案名称", "抽样方案名称"),

            // entity.samplingscheme.type
            new TranslationSeedItem("entity.samplingscheme.type", "en-US", "抽样方案类型", "抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）"),
            // entity.samplingscheme.type
            new TranslationSeedItem("entity.samplingscheme.type", "ja-JP", "抽样方案类型", "抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）"),
            // entity.samplingscheme.type
            new TranslationSeedItem("entity.samplingscheme.type", "zh-CN", "抽样方案类型", "抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）"),
            // entity.samplingscheme.type
            new TranslationSeedItem("entity.samplingscheme.type", "zh-HK", "抽样方案类型", "抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）"),

            // entity.samplingscheme.samplingstandard
            new TranslationSeedItem("entity.samplingscheme.samplingstandard", "en-US", "抽样标准", "抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）"),
            // entity.samplingscheme.samplingstandard
            new TranslationSeedItem("entity.samplingscheme.samplingstandard", "ja-JP", "抽样标准", "抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）"),
            // entity.samplingscheme.samplingstandard
            new TranslationSeedItem("entity.samplingscheme.samplingstandard", "zh-CN", "抽样标准", "抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）"),
            // entity.samplingscheme.samplingstandard
            new TranslationSeedItem("entity.samplingscheme.samplingstandard", "zh-HK", "抽样标准", "抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）"),

            // entity.samplingscheme.inspectionlevel
            new TranslationSeedItem("entity.samplingscheme.inspectionlevel", "en-US", "检验水平", "检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）"),
            // entity.samplingscheme.inspectionlevel
            new TranslationSeedItem("entity.samplingscheme.inspectionlevel", "ja-JP", "检验水平", "检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）"),
            // entity.samplingscheme.inspectionlevel
            new TranslationSeedItem("entity.samplingscheme.inspectionlevel", "zh-CN", "检验水平", "检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）"),
            // entity.samplingscheme.inspectionlevel
            new TranslationSeedItem("entity.samplingscheme.inspectionlevel", "zh-HK", "检验水平", "检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）"),

            // entity.samplingscheme.aqlvalue
            new TranslationSeedItem("entity.samplingscheme.aqlvalue", "en-US", "AQL值", "AQL值（可接受质量水平，0.010-1000，存储为小数）"),
            // entity.samplingscheme.aqlvalue
            new TranslationSeedItem("entity.samplingscheme.aqlvalue", "ja-JP", "AQL值", "AQL值（可接受质量水平，0.010-1000，存储为小数）"),
            // entity.samplingscheme.aqlvalue
            new TranslationSeedItem("entity.samplingscheme.aqlvalue", "zh-CN", "AQL值", "AQL值（可接受质量水平，0.010-1000，存储为小数）"),
            // entity.samplingscheme.aqlvalue
            new TranslationSeedItem("entity.samplingscheme.aqlvalue", "zh-HK", "AQL值", "AQL值（可接受质量水平，0.010-1000，存储为小数）"),

            // entity.samplingscheme.lotsizemin
            new TranslationSeedItem("entity.samplingscheme.lotsizemin", "en-US", "批量范围最小值", "批量范围最小值"),
            // entity.samplingscheme.lotsizemin
            new TranslationSeedItem("entity.samplingscheme.lotsizemin", "ja-JP", "批量范围最小值", "批量范围最小值"),
            // entity.samplingscheme.lotsizemin
            new TranslationSeedItem("entity.samplingscheme.lotsizemin", "zh-CN", "批量范围最小值", "批量范围最小值"),
            // entity.samplingscheme.lotsizemin
            new TranslationSeedItem("entity.samplingscheme.lotsizemin", "zh-HK", "批量范围最小值", "批量范围最小值"),

            // entity.samplingscheme.lotsizemax
            new TranslationSeedItem("entity.samplingscheme.lotsizemax", "en-US", "批量范围最大值", "批量范围最大值（0表示无上限）"),
            // entity.samplingscheme.lotsizemax
            new TranslationSeedItem("entity.samplingscheme.lotsizemax", "ja-JP", "批量范围最大值", "批量范围最大值（0表示无上限）"),
            // entity.samplingscheme.lotsizemax
            new TranslationSeedItem("entity.samplingscheme.lotsizemax", "zh-CN", "批量范围最大值", "批量范围最大值（0表示无上限）"),
            // entity.samplingscheme.lotsizemax
            new TranslationSeedItem("entity.samplingscheme.lotsizemax", "zh-HK", "批量范围最大值", "批量范围最大值（0表示无上限）"),

            // entity.samplingscheme.samplesize
            new TranslationSeedItem("entity.samplingscheme.samplesize", "en-US", "样本量", "样本量（抽样数量）"),
            // entity.samplingscheme.samplesize
            new TranslationSeedItem("entity.samplingscheme.samplesize", "ja-JP", "样本量", "样本量（抽样数量）"),
            // entity.samplingscheme.samplesize
            new TranslationSeedItem("entity.samplingscheme.samplesize", "zh-CN", "样本量", "样本量（抽样数量）"),
            // entity.samplingscheme.samplesize
            new TranslationSeedItem("entity.samplingscheme.samplesize", "zh-HK", "样本量", "样本量（抽样数量）"),

            // entity.samplingscheme.acceptancenumber
            new TranslationSeedItem("entity.samplingscheme.acceptancenumber", "en-US", "接收数", "接收数（Ac，Acceptance Number）"),
            // entity.samplingscheme.acceptancenumber
            new TranslationSeedItem("entity.samplingscheme.acceptancenumber", "ja-JP", "接收数", "接收数（Ac，Acceptance Number）"),
            // entity.samplingscheme.acceptancenumber
            new TranslationSeedItem("entity.samplingscheme.acceptancenumber", "zh-CN", "接收数", "接收数（Ac，Acceptance Number）"),
            // entity.samplingscheme.acceptancenumber
            new TranslationSeedItem("entity.samplingscheme.acceptancenumber", "zh-HK", "接收数", "接收数（Ac，Acceptance Number）"),

            // entity.samplingscheme.rejectionnumber
            new TranslationSeedItem("entity.samplingscheme.rejectionnumber", "en-US", "拒收数", "拒收数（Re，Rejection Number）"),
            // entity.samplingscheme.rejectionnumber
            new TranslationSeedItem("entity.samplingscheme.rejectionnumber", "ja-JP", "拒收数", "拒收数（Re，Rejection Number）"),
            // entity.samplingscheme.rejectionnumber
            new TranslationSeedItem("entity.samplingscheme.rejectionnumber", "zh-CN", "拒收数", "拒收数（Re，Rejection Number）"),
            // entity.samplingscheme.rejectionnumber
            new TranslationSeedItem("entity.samplingscheme.rejectionnumber", "zh-HK", "拒收数", "拒收数（Re，Rejection Number）"),

            // entity.samplingscheme.inspectionstrictness
            new TranslationSeedItem("entity.samplingscheme.inspectionstrictness", "en-US", "检验严格度", "检验严格度（0=正常检验，1=加严检验，2=放宽检验）"),
            // entity.samplingscheme.inspectionstrictness
            new TranslationSeedItem("entity.samplingscheme.inspectionstrictness", "ja-JP", "检验严格度", "检验严格度（0=正常检验，1=加严检验，2=放宽检验）"),
            // entity.samplingscheme.inspectionstrictness
            new TranslationSeedItem("entity.samplingscheme.inspectionstrictness", "zh-CN", "检验严格度", "检验严格度（0=正常检验，1=加严检验，2=放宽检验）"),
            // entity.samplingscheme.inspectionstrictness
            new TranslationSeedItem("entity.samplingscheme.inspectionstrictness", "zh-HK", "检验严格度", "检验严格度（0=正常检验，1=加严检验，2=放宽检验）"),

            // entity.samplingscheme.istransferruleenabled
            new TranslationSeedItem("entity.samplingscheme.istransferruleenabled", "en-US", "是否支持转移规则", "是否支持转移规则（0=否，1=是）"),
            // entity.samplingscheme.istransferruleenabled
            new TranslationSeedItem("entity.samplingscheme.istransferruleenabled", "ja-JP", "是否支持转移规则", "是否支持转移规则（0=否，1=是）"),
            // entity.samplingscheme.istransferruleenabled
            new TranslationSeedItem("entity.samplingscheme.istransferruleenabled", "zh-CN", "是否支持转移规则", "是否支持转移规则（0=否，1=是）"),
            // entity.samplingscheme.istransferruleenabled
            new TranslationSeedItem("entity.samplingscheme.istransferruleenabled", "zh-HK", "是否支持转移规则", "是否支持转移规则（0=否，1=是）"),

            // entity.samplingscheme.transferruleconfig
            new TranslationSeedItem("entity.samplingscheme.transferruleconfig", "en-US", "转移规则配置", "转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）"),
            // entity.samplingscheme.transferruleconfig
            new TranslationSeedItem("entity.samplingscheme.transferruleconfig", "ja-JP", "转移规则配置", "转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）"),
            // entity.samplingscheme.transferruleconfig
            new TranslationSeedItem("entity.samplingscheme.transferruleconfig", "zh-CN", "转移规则配置", "转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）"),
            // entity.samplingscheme.transferruleconfig
            new TranslationSeedItem("entity.samplingscheme.transferruleconfig", "zh-HK", "转移规则配置", "转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）"),

            // entity.samplingscheme.status
            new TranslationSeedItem("entity.samplingscheme.status", "en-US", "抽样方案状态", "抽样方案状态（0=草稿，1=已发布，2=已停用）"),
            // entity.samplingscheme.status
            new TranslationSeedItem("entity.samplingscheme.status", "ja-JP", "抽样方案状态", "抽样方案状态（0=草稿，1=已发布，2=已停用）"),
            // entity.samplingscheme.status
            new TranslationSeedItem("entity.samplingscheme.status", "zh-CN", "抽样方案状态", "抽样方案状态（0=草稿，1=已发布，2=已停用）"),
            // entity.samplingscheme.status
            new TranslationSeedItem("entity.samplingscheme.status", "zh-HK", "抽样方案状态", "抽样方案状态（0=草稿，1=已发布，2=已停用）"),

            // entity.samplingscheme.schemedescription
            new TranslationSeedItem("entity.samplingscheme.schemedescription", "en-US", "抽样方案描述", "抽样方案描述"),
            // entity.samplingscheme.schemedescription
            new TranslationSeedItem("entity.samplingscheme.schemedescription", "ja-JP", "抽样方案描述", "抽样方案描述"),
            // entity.samplingscheme.schemedescription
            new TranslationSeedItem("entity.samplingscheme.schemedescription", "zh-CN", "抽样方案描述", "抽样方案描述"),
            // entity.samplingscheme.schemedescription
            new TranslationSeedItem("entity.samplingscheme.schemedescription", "zh-HK", "抽样方案描述", "抽样方案描述"),

            // entity.samplingscheme.inspectionstandards
            new TranslationSeedItem("entity.samplingscheme.inspectionstandards", "en-US", "检验标准列表", "检验标准列表（主子表关系）"),
            // entity.samplingscheme.inspectionstandards
            new TranslationSeedItem("entity.samplingscheme.inspectionstandards", "ja-JP", "检验标准列表", "检验标准列表（主子表关系）"),
            // entity.samplingscheme.inspectionstandards
            new TranslationSeedItem("entity.samplingscheme.inspectionstandards", "zh-CN", "检验标准列表", "检验标准列表（主子表关系）"),
            // entity.samplingscheme.inspectionstandards
            new TranslationSeedItem("entity.samplingscheme.inspectionstandards", "zh-HK", "检验标准列表", "检验标准列表（主子表关系）"),
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
        translation.ResourceGroup = 4;
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
