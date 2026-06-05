// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailI18nSeedData.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcDetail 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEcDetail 实体国际化翻译种子（键前缀 entity.ecDetail.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcDetailI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcDetail 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecDetail 实体翻译...", tenantCode);

        foreach (var item in GetEcDetailTranslations())
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

        TaktLogger.Information("TaktEcDetail 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcDetail 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecDetail._self / entity.ecDetail.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecDetail._self
            new TranslationSeedItem("entity.ecDetail._self", "en-US", "Ec Detail Information", "实体名称"),
            // entity.ecDetail._self
            new TranslationSeedItem("entity.ecDetail._self", "ja-JP", "设变信息", "实体名称"),
            // entity.ecDetail._self
            new TranslationSeedItem("entity.ecDetail._self", "zh-CN", "设变信息", "实体名称"),
            // entity.ecDetail._self
            new TranslationSeedItem("entity.ecDetail._self", "zh-HK", "设变信息", "实体名称"),

            // entity.ecDetail.ecid
            new TranslationSeedItem("entity.ecDetail.ecid", "en-US", "设变ID", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.ecDetail.ecid
            new TranslationSeedItem("entity.ecDetail.ecid", "ja-JP", "设变ID", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.ecDetail.ecid
            new TranslationSeedItem("entity.ecDetail.ecid", "zh-CN", "设变ID", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.ecDetail.ecid
            new TranslationSeedItem("entity.ecDetail.ecid", "zh-HK", "设变ID", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.ecDetail.ecno
            new TranslationSeedItem("entity.ecDetail.ecno", "en-US", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecDetail.ecno
            new TranslationSeedItem("entity.ecDetail.ecno", "ja-JP", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecDetail.ecno
            new TranslationSeedItem("entity.ecDetail.ecno", "zh-CN", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecDetail.ecno
            new TranslationSeedItem("entity.ecDetail.ecno", "zh-HK", "设变单号", "设变单号（冗余字段,便于查询）"),

            // entity.ecDetail.linenumber
            new TranslationSeedItem("entity.ecDetail.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecDetail.linenumber
            new TranslationSeedItem("entity.ecDetail.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecDetail.linenumber
            new TranslationSeedItem("entity.ecDetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecDetail.linenumber
            new TranslationSeedItem("entity.ecDetail.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.ecDetail.ecmodel
            new TranslationSeedItem("entity.ecDetail.ecmodel", "en-US", "型号", "型号（Ec_model）"),
            // entity.ecDetail.ecmodel
            new TranslationSeedItem("entity.ecDetail.ecmodel", "ja-JP", "型号", "型号（Ec_model）"),
            // entity.ecDetail.ecmodel
            new TranslationSeedItem("entity.ecDetail.ecmodel", "zh-CN", "型号", "型号（Ec_model）"),
            // entity.ecDetail.ecmodel
            new TranslationSeedItem("entity.ecDetail.ecmodel", "zh-HK", "型号", "型号（Ec_model）"),

            // entity.ecDetail.ecbomitem
            new TranslationSeedItem("entity.ecDetail.ecbomitem", "en-US", "BOM主项料号", "BOM 主项料号（Ec_bomitem）"),
            // entity.ecDetail.ecbomitem
            new TranslationSeedItem("entity.ecDetail.ecbomitem", "ja-JP", "BOM主项料号", "BOM 主项料号（Ec_bomitem）"),
            // entity.ecDetail.ecbomitem
            new TranslationSeedItem("entity.ecDetail.ecbomitem", "zh-CN", "BOM主项料号", "BOM 主项料号（Ec_bomitem）"),
            // entity.ecDetail.ecbomitem
            new TranslationSeedItem("entity.ecDetail.ecbomitem", "zh-HK", "BOM主项料号", "BOM 主项料号（Ec_bomitem）"),

            // entity.ecDetail.ecbomsubitem
            new TranslationSeedItem("entity.ecDetail.ecbomsubitem", "en-US", "BOM子项料号", "BOM 子项料号（Ec_bomsubitem）"),
            // entity.ecDetail.ecbomsubitem
            new TranslationSeedItem("entity.ecDetail.ecbomsubitem", "ja-JP", "BOM子项料号", "BOM 子项料号（Ec_bomsubitem）"),
            // entity.ecDetail.ecbomsubitem
            new TranslationSeedItem("entity.ecDetail.ecbomsubitem", "zh-CN", "BOM子项料号", "BOM 子项料号（Ec_bomsubitem）"),
            // entity.ecDetail.ecbomsubitem
            new TranslationSeedItem("entity.ecDetail.ecbomsubitem", "zh-HK", "BOM子项料号", "BOM 子项料号（Ec_bomsubitem）"),

            // entity.ecDetail.ecbomno
            new TranslationSeedItem("entity.ecDetail.ecbomno", "en-US", "BOM编号", "BOM 编号（Ec_bomno）"),
            // entity.ecDetail.ecbomno
            new TranslationSeedItem("entity.ecDetail.ecbomno", "ja-JP", "BOM编号", "BOM 编号（Ec_bomno）"),
            // entity.ecDetail.ecbomno
            new TranslationSeedItem("entity.ecDetail.ecbomno", "zh-CN", "BOM编号", "BOM 编号（Ec_bomno）"),
            // entity.ecDetail.ecbomno
            new TranslationSeedItem("entity.ecDetail.ecbomno", "zh-HK", "BOM编号", "BOM 编号（Ec_bomno）"),

            // entity.ecDetail.ecchange
            new TranslationSeedItem("entity.ecDetail.ecchange", "en-US", "变更内容", "变更内容（Ec_change）"),
            // entity.ecDetail.ecchange
            new TranslationSeedItem("entity.ecDetail.ecchange", "ja-JP", "变更内容", "变更内容（Ec_change）"),
            // entity.ecDetail.ecchange
            new TranslationSeedItem("entity.ecDetail.ecchange", "zh-CN", "变更内容", "变更内容（Ec_change）"),
            // entity.ecDetail.ecchange
            new TranslationSeedItem("entity.ecDetail.ecchange", "zh-HK", "变更内容", "变更内容（Ec_change）"),

            // entity.ecDetail.eclocal
            new TranslationSeedItem("entity.ecDetail.eclocal", "en-US", "本地现场", "本地/现场（Ec_local）"),
            // entity.ecDetail.eclocal
            new TranslationSeedItem("entity.ecDetail.eclocal", "ja-JP", "本地现场", "本地/现场（Ec_local）"),
            // entity.ecDetail.eclocal
            new TranslationSeedItem("entity.ecDetail.eclocal", "zh-CN", "本地现场", "本地/现场（Ec_local）"),
            // entity.ecDetail.eclocal
            new TranslationSeedItem("entity.ecDetail.eclocal", "zh-HK", "本地现场", "本地/现场（Ec_local）"),

            // entity.ecDetail.ecnote
            new TranslationSeedItem("entity.ecDetail.ecnote", "en-US", "备注", "备注（Ec_note）"),
            // entity.ecDetail.ecnote
            new TranslationSeedItem("entity.ecDetail.ecnote", "ja-JP", "备注", "备注（Ec_note）"),
            // entity.ecDetail.ecnote
            new TranslationSeedItem("entity.ecDetail.ecnote", "zh-CN", "备注", "备注（Ec_note）"),
            // entity.ecDetail.ecnote
            new TranslationSeedItem("entity.ecDetail.ecnote", "zh-HK", "备注", "备注（Ec_note）"),

            // entity.ecDetail.ecprocess
            new TranslationSeedItem("entity.ecDetail.ecprocess", "en-US", "工序", "工序（Ec_process）"),
            // entity.ecDetail.ecprocess
            new TranslationSeedItem("entity.ecDetail.ecprocess", "ja-JP", "工序", "工序（Ec_process）"),
            // entity.ecDetail.ecprocess
            new TranslationSeedItem("entity.ecDetail.ecprocess", "zh-CN", "工序", "工序（Ec_process）"),
            // entity.ecDetail.ecprocess
            new TranslationSeedItem("entity.ecDetail.ecprocess", "zh-HK", "工序", "工序（Ec_process）"),

            // entity.ecDetail.ecbomdate
            new TranslationSeedItem("entity.ecDetail.ecbomdate", "en-US", "BOM日期", "BOM 日期（Ec_bomdate）"),
            // entity.ecDetail.ecbomdate
            new TranslationSeedItem("entity.ecDetail.ecbomdate", "ja-JP", "BOM日期", "BOM 日期（Ec_bomdate）"),
            // entity.ecDetail.ecbomdate
            new TranslationSeedItem("entity.ecDetail.ecbomdate", "zh-CN", "BOM日期", "BOM 日期（Ec_bomdate）"),
            // entity.ecDetail.ecbomdate
            new TranslationSeedItem("entity.ecDetail.ecbomdate", "zh-HK", "BOM日期", "BOM 日期（Ec_bomdate）"),

            // entity.ecDetail.ecentrydate
            new TranslationSeedItem("entity.ecDetail.ecentrydate", "en-US", "录入日期", "录入日期（Ec_entrydate）"),
            // entity.ecDetail.ecentrydate
            new TranslationSeedItem("entity.ecDetail.ecentrydate", "ja-JP", "录入日期", "录入日期（Ec_entrydate）"),
            // entity.ecDetail.ecentrydate
            new TranslationSeedItem("entity.ecDetail.ecentrydate", "zh-CN", "录入日期", "录入日期（Ec_entrydate）"),
            // entity.ecDetail.ecentrydate
            new TranslationSeedItem("entity.ecDetail.ecentrydate", "zh-HK", "录入日期", "录入日期（Ec_entrydate）"),

            // entity.ecDetail.ecolditem
            new TranslationSeedItem("entity.ecDetail.ecolditem", "en-US", "旧料号", "旧料号（Ec_olditem）"),
            // entity.ecDetail.ecolditem
            new TranslationSeedItem("entity.ecDetail.ecolditem", "ja-JP", "旧料号", "旧料号（Ec_olditem）"),
            // entity.ecDetail.ecolditem
            new TranslationSeedItem("entity.ecDetail.ecolditem", "zh-CN", "旧料号", "旧料号（Ec_olditem）"),
            // entity.ecDetail.ecolditem
            new TranslationSeedItem("entity.ecDetail.ecolditem", "zh-HK", "旧料号", "旧料号（Ec_olditem）"),

            // entity.ecDetail.ecoldtext
            new TranslationSeedItem("entity.ecDetail.ecoldtext", "en-US", "旧料号描述", "旧料号描述（Ec_oldtext）"),
            // entity.ecDetail.ecoldtext
            new TranslationSeedItem("entity.ecDetail.ecoldtext", "ja-JP", "旧料号描述", "旧料号描述（Ec_oldtext）"),
            // entity.ecDetail.ecoldtext
            new TranslationSeedItem("entity.ecDetail.ecoldtext", "zh-CN", "旧料号描述", "旧料号描述（Ec_oldtext）"),
            // entity.ecDetail.ecoldtext
            new TranslationSeedItem("entity.ecDetail.ecoldtext", "zh-HK", "旧料号描述", "旧料号描述（Ec_oldtext）"),

            // entity.ecDetail.ecoldqty
            new TranslationSeedItem("entity.ecDetail.ecoldqty", "en-US", "旧数量", "旧数量（Ec_oldqty）"),
            // entity.ecDetail.ecoldqty
            new TranslationSeedItem("entity.ecDetail.ecoldqty", "ja-JP", "旧数量", "旧数量（Ec_oldqty）"),
            // entity.ecDetail.ecoldqty
            new TranslationSeedItem("entity.ecDetail.ecoldqty", "zh-CN", "旧数量", "旧数量（Ec_oldqty）"),
            // entity.ecDetail.ecoldqty
            new TranslationSeedItem("entity.ecDetail.ecoldqty", "zh-HK", "旧数量", "旧数量（Ec_oldqty）"),

            // entity.ecDetail.ecoldset
            new TranslationSeedItem("entity.ecDetail.ecoldset", "en-US", "旧单位", "旧单位/设置（Ec_oldset）"),
            // entity.ecDetail.ecoldset
            new TranslationSeedItem("entity.ecDetail.ecoldset", "ja-JP", "旧单位", "旧单位/设置（Ec_oldset）"),
            // entity.ecDetail.ecoldset
            new TranslationSeedItem("entity.ecDetail.ecoldset", "zh-CN", "旧单位", "旧单位/设置（Ec_oldset）"),
            // entity.ecDetail.ecoldset
            new TranslationSeedItem("entity.ecDetail.ecoldset", "zh-HK", "旧单位", "旧单位/设置（Ec_oldset）"),

            // entity.ecDetail.ecnewitem
            new TranslationSeedItem("entity.ecDetail.ecnewitem", "en-US", "新料号", "新料号（Ec_newitem）"),
            // entity.ecDetail.ecnewitem
            new TranslationSeedItem("entity.ecDetail.ecnewitem", "ja-JP", "新料号", "新料号（Ec_newitem）"),
            // entity.ecDetail.ecnewitem
            new TranslationSeedItem("entity.ecDetail.ecnewitem", "zh-CN", "新料号", "新料号（Ec_newitem）"),
            // entity.ecDetail.ecnewitem
            new TranslationSeedItem("entity.ecDetail.ecnewitem", "zh-HK", "新料号", "新料号（Ec_newitem）"),

            // entity.ecDetail.ecnewtext
            new TranslationSeedItem("entity.ecDetail.ecnewtext", "en-US", "新料号描述", "新料号描述（Ec_newtext）"),
            // entity.ecDetail.ecnewtext
            new TranslationSeedItem("entity.ecDetail.ecnewtext", "ja-JP", "新料号描述", "新料号描述（Ec_newtext）"),
            // entity.ecDetail.ecnewtext
            new TranslationSeedItem("entity.ecDetail.ecnewtext", "zh-CN", "新料号描述", "新料号描述（Ec_newtext）"),
            // entity.ecDetail.ecnewtext
            new TranslationSeedItem("entity.ecDetail.ecnewtext", "zh-HK", "新料号描述", "新料号描述（Ec_newtext）"),

            // entity.ecDetail.ecnewqty
            new TranslationSeedItem("entity.ecDetail.ecnewqty", "en-US", "新数量", "新数量（Ec_newqty）"),
            // entity.ecDetail.ecnewqty
            new TranslationSeedItem("entity.ecDetail.ecnewqty", "ja-JP", "新数量", "新数量（Ec_newqty）"),
            // entity.ecDetail.ecnewqty
            new TranslationSeedItem("entity.ecDetail.ecnewqty", "zh-CN", "新数量", "新数量（Ec_newqty）"),
            // entity.ecDetail.ecnewqty
            new TranslationSeedItem("entity.ecDetail.ecnewqty", "zh-HK", "新数量", "新数量（Ec_newqty）"),

            // entity.ecDetail.ecnewset
            new TranslationSeedItem("entity.ecDetail.ecnewset", "en-US", "新单位", "新单位/设置（Ec_newset）"),
            // entity.ecDetail.ecnewset
            new TranslationSeedItem("entity.ecDetail.ecnewset", "ja-JP", "新单位", "新单位/设置（Ec_newset）"),
            // entity.ecDetail.ecnewset
            new TranslationSeedItem("entity.ecDetail.ecnewset", "zh-CN", "新单位", "新单位/设置（Ec_newset）"),
            // entity.ecDetail.ecnewset
            new TranslationSeedItem("entity.ecDetail.ecnewset", "zh-HK", "新单位", "新单位/设置（Ec_newset）"),

            // entity.ecDetail.isprocurement
            new TranslationSeedItem("entity.ecDetail.isprocurement", "en-US", "是否采购", "是否采购（0=否 1=是）"),
            // entity.ecDetail.isprocurement
            new TranslationSeedItem("entity.ecDetail.isprocurement", "ja-JP", "是否采购", "是否采购（0=否 1=是）"),
            // entity.ecDetail.isprocurement
            new TranslationSeedItem("entity.ecDetail.isprocurement", "zh-CN", "是否采购", "是否采购（0=否 1=是）"),
            // entity.ecDetail.isprocurement
            new TranslationSeedItem("entity.ecDetail.isprocurement", "zh-HK", "是否采购", "是否采购（0=否 1=是）"),

            // entity.ecDetail.ischeck
            new TranslationSeedItem("entity.ecDetail.ischeck", "en-US", "是否检查", "是否检查（0=否 1=是）"),
            // entity.ecDetail.ischeck
            new TranslationSeedItem("entity.ecDetail.ischeck", "ja-JP", "是否检查", "是否检查（0=否 1=是）"),
            // entity.ecDetail.ischeck
            new TranslationSeedItem("entity.ecDetail.ischeck", "zh-CN", "是否检查", "是否检查（0=否 1=是）"),
            // entity.ecDetail.ischeck
            new TranslationSeedItem("entity.ecDetail.ischeck", "zh-HK", "是否检查", "是否检查（0=否 1=是）"),

            // entity.ecDetail.ecwarehouse
            new TranslationSeedItem("entity.ecDetail.ecwarehouse", "en-US", "仓库", "仓库（Ec_warehouse）"),
            // entity.ecDetail.ecwarehouse
            new TranslationSeedItem("entity.ecDetail.ecwarehouse", "ja-JP", "仓库", "仓库（Ec_warehouse）"),
            // entity.ecDetail.ecwarehouse
            new TranslationSeedItem("entity.ecDetail.ecwarehouse", "zh-CN", "仓库", "仓库（Ec_warehouse）"),
            // entity.ecDetail.ecwarehouse
            new TranslationSeedItem("entity.ecDetail.ecwarehouse", "zh-HK", "仓库", "仓库（Ec_warehouse）"),

            // entity.ecDetail.isendofline
            new TranslationSeedItem("entity.ecDetail.isendofline", "en-US", "EOL", "EOL（End of Line，0=否 1=是）"),
            // entity.ecDetail.isendofline
            new TranslationSeedItem("entity.ecDetail.isendofline", "ja-JP", "EOL", "EOL（End of Line，0=否 1=是）"),
            // entity.ecDetail.isendofline
            new TranslationSeedItem("entity.ecDetail.isendofline", "zh-CN", "EOL", "EOL（End of Line，0=否 1=是）"),
            // entity.ecDetail.isendofline
            new TranslationSeedItem("entity.ecDetail.isendofline", "zh-HK", "EOL", "EOL（End of Line，0=否 1=是）"),
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
