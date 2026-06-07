// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktSamplingSchemeI18nSeedData.cs
// 创建时间：2026-06-07
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation;

/// <summary>
/// TaktSamplingScheme 实体国际化翻译种子（键前缀 entity.samplingScheme.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 samplingScheme 实体翻译...", tenantCode);

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
    /// I18nKey：entity.samplingScheme._self / entity.samplingScheme.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSamplingSchemeTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.samplingScheme._self
            new TranslationSeedItem("entity.samplingScheme._self", "en-US", "Sampling Scheme Information", "实体名称"),
            // entity.samplingScheme._self
            new TranslationSeedItem("entity.samplingScheme._self", "ja-JP", "Takt抽样方案信息", "实体名称"),
            // entity.samplingScheme._self
            new TranslationSeedItem("entity.samplingScheme._self", "zh-CN", "Takt抽样方案信息", "实体名称"),
            // entity.samplingScheme._self
            new TranslationSeedItem("entity.samplingScheme._self", "zh-HK", "Takt抽样方案信息", "实体名称"),

            // entity.samplingScheme.plantcode
            new TranslationSeedItem("entity.samplingScheme.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.samplingScheme.plantcode
            new TranslationSeedItem("entity.samplingScheme.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.samplingScheme.plantcode
            new TranslationSeedItem("entity.samplingScheme.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.samplingScheme.plantcode
            new TranslationSeedItem("entity.samplingScheme.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.samplingScheme.code
            new TranslationSeedItem("entity.samplingScheme.code", "en-US", "抽样方案编码", "抽样方案编码（唯一索引）"),
            // entity.samplingScheme.code
            new TranslationSeedItem("entity.samplingScheme.code", "ja-JP", "抽样方案编码", "抽样方案编码（唯一索引）"),
            // entity.samplingScheme.code
            new TranslationSeedItem("entity.samplingScheme.code", "zh-CN", "抽样方案编码", "抽样方案编码（唯一索引）"),
            // entity.samplingScheme.code
            new TranslationSeedItem("entity.samplingScheme.code", "zh-HK", "抽样方案编码", "抽样方案编码（唯一索引）"),

            // entity.samplingScheme.name
            new TranslationSeedItem("entity.samplingScheme.name", "en-US", "抽样方案名称", "抽样方案名称"),
            // entity.samplingScheme.name
            new TranslationSeedItem("entity.samplingScheme.name", "ja-JP", "抽样方案名称", "抽样方案名称"),
            // entity.samplingScheme.name
            new TranslationSeedItem("entity.samplingScheme.name", "zh-CN", "抽样方案名称", "抽样方案名称"),
            // entity.samplingScheme.name
            new TranslationSeedItem("entity.samplingScheme.name", "zh-HK", "抽样方案名称", "抽样方案名称"),

            // entity.samplingScheme.type
            new TranslationSeedItem("entity.samplingScheme.type", "en-US", "抽样方案类型", "抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）"),
            // entity.samplingScheme.type
            new TranslationSeedItem("entity.samplingScheme.type", "ja-JP", "抽样方案类型", "抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）"),
            // entity.samplingScheme.type
            new TranslationSeedItem("entity.samplingScheme.type", "zh-CN", "抽样方案类型", "抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）"),
            // entity.samplingScheme.type
            new TranslationSeedItem("entity.samplingScheme.type", "zh-HK", "抽样方案类型", "抽样方案类型（0=计数型，1=计量型，2=计数调整型，3=计量调整型）"),

            // entity.samplingScheme.samplingstandard
            new TranslationSeedItem("entity.samplingScheme.samplingstandard", "en-US", "抽样标准", "抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）"),
            // entity.samplingScheme.samplingstandard
            new TranslationSeedItem("entity.samplingScheme.samplingstandard", "ja-JP", "抽样标准", "抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）"),
            // entity.samplingScheme.samplingstandard
            new TranslationSeedItem("entity.samplingScheme.samplingstandard", "zh-CN", "抽样标准", "抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）"),
            // entity.samplingScheme.samplingstandard
            new TranslationSeedItem("entity.samplingScheme.samplingstandard", "zh-HK", "抽样标准", "抽样标准（0=GB/T 2828.1，1=GB/T 6378，2=MIL-STD-105E，3=ANSI/ASQ Z1.4，4=ISO 2859-1，5=自定义）"),

            // entity.samplingScheme.inspectionlevel
            new TranslationSeedItem("entity.samplingScheme.inspectionlevel", "en-US", "检验水平", "检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）"),
            // entity.samplingScheme.inspectionlevel
            new TranslationSeedItem("entity.samplingScheme.inspectionlevel", "ja-JP", "检验水平", "检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）"),
            // entity.samplingScheme.inspectionlevel
            new TranslationSeedItem("entity.samplingScheme.inspectionlevel", "zh-CN", "检验水平", "检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）"),
            // entity.samplingScheme.inspectionlevel
            new TranslationSeedItem("entity.samplingScheme.inspectionlevel", "zh-HK", "检验水平", "检验水平（0=I，1=II，2=III，3=S-1，4=S-2，5=S-3，6=S-4）"),

            // entity.samplingScheme.aqlvalue
            new TranslationSeedItem("entity.samplingScheme.aqlvalue", "en-US", "AQL值", "AQL值（可接受质量水平，0.010-1000，存储为小数）"),
            // entity.samplingScheme.aqlvalue
            new TranslationSeedItem("entity.samplingScheme.aqlvalue", "ja-JP", "AQL值", "AQL值（可接受质量水平，0.010-1000，存储为小数）"),
            // entity.samplingScheme.aqlvalue
            new TranslationSeedItem("entity.samplingScheme.aqlvalue", "zh-CN", "AQL值", "AQL值（可接受质量水平，0.010-1000，存储为小数）"),
            // entity.samplingScheme.aqlvalue
            new TranslationSeedItem("entity.samplingScheme.aqlvalue", "zh-HK", "AQL值", "AQL值（可接受质量水平，0.010-1000，存储为小数）"),

            // entity.samplingScheme.lotsizemin
            new TranslationSeedItem("entity.samplingScheme.lotsizemin", "en-US", "批量范围最小值", "批量范围最小值"),
            // entity.samplingScheme.lotsizemin
            new TranslationSeedItem("entity.samplingScheme.lotsizemin", "ja-JP", "批量范围最小值", "批量范围最小值"),
            // entity.samplingScheme.lotsizemin
            new TranslationSeedItem("entity.samplingScheme.lotsizemin", "zh-CN", "批量范围最小值", "批量范围最小值"),
            // entity.samplingScheme.lotsizemin
            new TranslationSeedItem("entity.samplingScheme.lotsizemin", "zh-HK", "批量范围最小值", "批量范围最小值"),

            // entity.samplingScheme.lotsizemax
            new TranslationSeedItem("entity.samplingScheme.lotsizemax", "en-US", "批量范围最大值", "批量范围最大值（0表示无上限）"),
            // entity.samplingScheme.lotsizemax
            new TranslationSeedItem("entity.samplingScheme.lotsizemax", "ja-JP", "批量范围最大值", "批量范围最大值（0表示无上限）"),
            // entity.samplingScheme.lotsizemax
            new TranslationSeedItem("entity.samplingScheme.lotsizemax", "zh-CN", "批量范围最大值", "批量范围最大值（0表示无上限）"),
            // entity.samplingScheme.lotsizemax
            new TranslationSeedItem("entity.samplingScheme.lotsizemax", "zh-HK", "批量范围最大值", "批量范围最大值（0表示无上限）"),

            // entity.samplingScheme.samplesize
            new TranslationSeedItem("entity.samplingScheme.samplesize", "en-US", "样本量", "样本量（抽样数量）"),
            // entity.samplingScheme.samplesize
            new TranslationSeedItem("entity.samplingScheme.samplesize", "ja-JP", "样本量", "样本量（抽样数量）"),
            // entity.samplingScheme.samplesize
            new TranslationSeedItem("entity.samplingScheme.samplesize", "zh-CN", "样本量", "样本量（抽样数量）"),
            // entity.samplingScheme.samplesize
            new TranslationSeedItem("entity.samplingScheme.samplesize", "zh-HK", "样本量", "样本量（抽样数量）"),

            // entity.samplingScheme.acceptancenumber
            new TranslationSeedItem("entity.samplingScheme.acceptancenumber", "en-US", "接收数", "接收数（Ac，Acceptance Number）"),
            // entity.samplingScheme.acceptancenumber
            new TranslationSeedItem("entity.samplingScheme.acceptancenumber", "ja-JP", "接收数", "接收数（Ac，Acceptance Number）"),
            // entity.samplingScheme.acceptancenumber
            new TranslationSeedItem("entity.samplingScheme.acceptancenumber", "zh-CN", "接收数", "接收数（Ac，Acceptance Number）"),
            // entity.samplingScheme.acceptancenumber
            new TranslationSeedItem("entity.samplingScheme.acceptancenumber", "zh-HK", "接收数", "接收数（Ac，Acceptance Number）"),

            // entity.samplingScheme.rejectionnumber
            new TranslationSeedItem("entity.samplingScheme.rejectionnumber", "en-US", "拒收数", "拒收数（Re，Rejection Number）"),
            // entity.samplingScheme.rejectionnumber
            new TranslationSeedItem("entity.samplingScheme.rejectionnumber", "ja-JP", "拒收数", "拒收数（Re，Rejection Number）"),
            // entity.samplingScheme.rejectionnumber
            new TranslationSeedItem("entity.samplingScheme.rejectionnumber", "zh-CN", "拒收数", "拒收数（Re，Rejection Number）"),
            // entity.samplingScheme.rejectionnumber
            new TranslationSeedItem("entity.samplingScheme.rejectionnumber", "zh-HK", "拒收数", "拒收数（Re，Rejection Number）"),

            // entity.samplingScheme.inspectionstrictness
            new TranslationSeedItem("entity.samplingScheme.inspectionstrictness", "en-US", "检验严格度", "检验严格度（0=正常检验，1=加严检验，2=放宽检验）"),
            // entity.samplingScheme.inspectionstrictness
            new TranslationSeedItem("entity.samplingScheme.inspectionstrictness", "ja-JP", "检验严格度", "检验严格度（0=正常检验，1=加严检验，2=放宽检验）"),
            // entity.samplingScheme.inspectionstrictness
            new TranslationSeedItem("entity.samplingScheme.inspectionstrictness", "zh-CN", "检验严格度", "检验严格度（0=正常检验，1=加严检验，2=放宽检验）"),
            // entity.samplingScheme.inspectionstrictness
            new TranslationSeedItem("entity.samplingScheme.inspectionstrictness", "zh-HK", "检验严格度", "检验严格度（0=正常检验，1=加严检验，2=放宽检验）"),

            // entity.samplingScheme.istransferruleenabled
            new TranslationSeedItem("entity.samplingScheme.istransferruleenabled", "en-US", "是否支持转移规则", "是否支持转移规则（0=否，1=是）"),
            // entity.samplingScheme.istransferruleenabled
            new TranslationSeedItem("entity.samplingScheme.istransferruleenabled", "ja-JP", "是否支持转移规则", "是否支持转移规则（0=否，1=是）"),
            // entity.samplingScheme.istransferruleenabled
            new TranslationSeedItem("entity.samplingScheme.istransferruleenabled", "zh-CN", "是否支持转移规则", "是否支持转移规则（0=否，1=是）"),
            // entity.samplingScheme.istransferruleenabled
            new TranslationSeedItem("entity.samplingScheme.istransferruleenabled", "zh-HK", "是否支持转移规则", "是否支持转移规则（0=否，1=是）"),

            // entity.samplingScheme.transferruleconfig
            new TranslationSeedItem("entity.samplingScheme.transferruleconfig", "en-US", "转移规则配置", "转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）"),
            // entity.samplingScheme.transferruleconfig
            new TranslationSeedItem("entity.samplingScheme.transferruleconfig", "ja-JP", "转移规则配置", "转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）"),
            // entity.samplingScheme.transferruleconfig
            new TranslationSeedItem("entity.samplingScheme.transferruleconfig", "zh-CN", "转移规则配置", "转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）"),
            // entity.samplingScheme.transferruleconfig
            new TranslationSeedItem("entity.samplingScheme.transferruleconfig", "zh-HK", "转移规则配置", "转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）"),

            // entity.samplingScheme.status
            new TranslationSeedItem("entity.samplingScheme.status", "en-US", "抽样方案状态", "抽样方案状态（0=草稿，1=已发布，2=已停用）"),
            // entity.samplingScheme.status
            new TranslationSeedItem("entity.samplingScheme.status", "ja-JP", "抽样方案状态", "抽样方案状态（0=草稿，1=已发布，2=已停用）"),
            // entity.samplingScheme.status
            new TranslationSeedItem("entity.samplingScheme.status", "zh-CN", "抽样方案状态", "抽样方案状态（0=草稿，1=已发布，2=已停用）"),
            // entity.samplingScheme.status
            new TranslationSeedItem("entity.samplingScheme.status", "zh-HK", "抽样方案状态", "抽样方案状态（0=草稿，1=已发布，2=已停用）"),

            // entity.samplingScheme.schemedescription
            new TranslationSeedItem("entity.samplingScheme.schemedescription", "en-US", "抽样方案描述", "抽样方案描述"),
            // entity.samplingScheme.schemedescription
            new TranslationSeedItem("entity.samplingScheme.schemedescription", "ja-JP", "抽样方案描述", "抽样方案描述"),
            // entity.samplingScheme.schemedescription
            new TranslationSeedItem("entity.samplingScheme.schemedescription", "zh-CN", "抽样方案描述", "抽样方案描述"),
            // entity.samplingScheme.schemedescription
            new TranslationSeedItem("entity.samplingScheme.schemedescription", "zh-HK", "抽样方案描述", "抽样方案描述"),

            // entity.samplingScheme.inspectionstandards
            new TranslationSeedItem("entity.samplingScheme.inspectionstandards", "en-US", "inspectionStandards", "检验标准列表（主子表关系）"),
            // entity.samplingScheme.inspectionstandards
            new TranslationSeedItem("entity.samplingScheme.inspectionstandards", "ja-JP", "inspectionStandards", "检验标准列表（主子表关系）"),
            // entity.samplingScheme.inspectionstandards
            new TranslationSeedItem("entity.samplingScheme.inspectionstandards", "zh-CN", "inspectionStandards", "检验标准列表（主子表关系）"),
            // entity.samplingScheme.inspectionstandards
            new TranslationSeedItem("entity.samplingScheme.inspectionstandards", "zh-HK", "inspectionStandards", "检验标准列表（主子表关系）"),
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
        translation.ResourceGroup = TaktModule.Logistics;
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
