// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailI18nSeedData.cs
// 创建时间：2026-07-09
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEcDetail 实体国际化翻译种子（键前缀 entity.ecdetail.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecdetail 实体翻译...", tenantCode);

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
    /// I18nKey：entity.ecdetail._self / entity.ecdetail.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "en-US", "Ec Detail Information_us", "实体名称"),
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "ja-JP", "设变明细信息_jp", "实体名称"),
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "zh-CN", "设变明细信息", "实体名称"),
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "zh-HK", "设变明细信息_hk", "实体名称"),

            // entity.ecdetail.ecid
            new TranslationSeedItem("entity.ecdetail.ecid", "en-US", "设变ID_us", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.ecdetail.ecid
            new TranslationSeedItem("entity.ecdetail.ecid", "ja-JP", "设变ID_jp", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.ecdetail.ecid
            new TranslationSeedItem("entity.ecdetail.ecid", "zh-CN", "设变ID", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.ecdetail.ecid
            new TranslationSeedItem("entity.ecdetail.ecid", "zh-HK", "设变ID_hk", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.ecdetail.ecno
            new TranslationSeedItem("entity.ecdetail.ecno", "en-US", "设变单号_us", "设变单号（冗余字段,便于查询）"),
            // entity.ecdetail.ecno
            new TranslationSeedItem("entity.ecdetail.ecno", "ja-JP", "设变单号_jp", "设变单号（冗余字段,便于查询）"),
            // entity.ecdetail.ecno
            new TranslationSeedItem("entity.ecdetail.ecno", "zh-CN", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecdetail.ecno
            new TranslationSeedItem("entity.ecdetail.ecno", "zh-HK", "设变单号_hk", "设变单号（冗余字段,便于查询）"),

            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecdetail.ecbomlineno
            new TranslationSeedItem("entity.ecdetail.ecbomlineno", "en-US", "BOM行号_us", "BOM行号（Ec_bom_line_no）"),
            // entity.ecdetail.ecbomlineno
            new TranslationSeedItem("entity.ecdetail.ecbomlineno", "ja-JP", "BOM行号_jp", "BOM行号（Ec_bom_line_no）"),
            // entity.ecdetail.ecbomlineno
            new TranslationSeedItem("entity.ecdetail.ecbomlineno", "zh-CN", "BOM行号", "BOM行号（Ec_bom_line_no）"),
            // entity.ecdetail.ecbomlineno
            new TranslationSeedItem("entity.ecdetail.ecbomlineno", "zh-HK", "BOM行号_hk", "BOM行号（Ec_bom_line_no）"),

            // entity.ecdetail.ecmodel
            new TranslationSeedItem("entity.ecdetail.ecmodel", "en-US", "机种_us", "机种（Ec_model）"),
            // entity.ecdetail.ecmodel
            new TranslationSeedItem("entity.ecdetail.ecmodel", "ja-JP", "机种_jp", "机种（Ec_model）"),
            // entity.ecdetail.ecmodel
            new TranslationSeedItem("entity.ecdetail.ecmodel", "zh-CN", "机种", "机种（Ec_model）"),
            // entity.ecdetail.ecmodel
            new TranslationSeedItem("entity.ecdetail.ecmodel", "zh-HK", "机种_hk", "机种（Ec_model）"),

            // entity.ecdetail.ecbomitem
            new TranslationSeedItem("entity.ecdetail.ecbomitem", "en-US", "完成品_us", "完成品（Ec_bomitem）"),
            // entity.ecdetail.ecbomitem
            new TranslationSeedItem("entity.ecdetail.ecbomitem", "ja-JP", "完成品_jp", "完成品（Ec_bomitem）"),
            // entity.ecdetail.ecbomitem
            new TranslationSeedItem("entity.ecdetail.ecbomitem", "zh-CN", "完成品", "完成品（Ec_bomitem）"),
            // entity.ecdetail.ecbomitem
            new TranslationSeedItem("entity.ecdetail.ecbomitem", "zh-HK", "完成品_hk", "完成品（Ec_bomitem）"),

            // entity.ecdetail.ecbomitemtext
            new TranslationSeedItem("entity.ecdetail.ecbomitemtext", "en-US", "完成品描述_us", "完成品描述（Ec_bomitemtext）"),
            // entity.ecdetail.ecbomitemtext
            new TranslationSeedItem("entity.ecdetail.ecbomitemtext", "ja-JP", "完成品描述_jp", "完成品描述（Ec_bomitemtext）"),
            // entity.ecdetail.ecbomitemtext
            new TranslationSeedItem("entity.ecdetail.ecbomitemtext", "zh-CN", "完成品描述", "完成品描述（Ec_bomitemtext）"),
            // entity.ecdetail.ecbomitemtext
            new TranslationSeedItem("entity.ecdetail.ecbomitemtext", "zh-HK", "完成品描述_hk", "完成品描述（Ec_bomitemtext）"),

            // entity.ecdetail.ecbomsubitem
            new TranslationSeedItem("entity.ecdetail.ecbomsubitem", "en-US", "上阶物料_us", "上阶物料（Ec_bomsubitem）"),
            // entity.ecdetail.ecbomsubitem
            new TranslationSeedItem("entity.ecdetail.ecbomsubitem", "ja-JP", "上阶物料_jp", "上阶物料（Ec_bomsubitem）"),
            // entity.ecdetail.ecbomsubitem
            new TranslationSeedItem("entity.ecdetail.ecbomsubitem", "zh-CN", "上阶物料", "上阶物料（Ec_bomsubitem）"),
            // entity.ecdetail.ecbomsubitem
            new TranslationSeedItem("entity.ecdetail.ecbomsubitem", "zh-HK", "上阶物料_hk", "上阶物料（Ec_bomsubitem）"),

            // entity.ecdetail.ecbomsubitemtext
            new TranslationSeedItem("entity.ecdetail.ecbomsubitemtext", "en-US", "上阶物料描述_us", "上阶物料描述（Ec_bomsubitemtext）"),
            // entity.ecdetail.ecbomsubitemtext
            new TranslationSeedItem("entity.ecdetail.ecbomsubitemtext", "ja-JP", "上阶物料描述_jp", "上阶物料描述（Ec_bomsubitemtext）"),
            // entity.ecdetail.ecbomsubitemtext
            new TranslationSeedItem("entity.ecdetail.ecbomsubitemtext", "zh-CN", "上阶物料描述", "上阶物料描述（Ec_bomsubitemtext）"),
            // entity.ecdetail.ecbomsubitemtext
            new TranslationSeedItem("entity.ecdetail.ecbomsubitemtext", "zh-HK", "上阶物料描述_hk", "上阶物料描述（Ec_bomsubitemtext）"),

            // entity.ecdetail.isendofline
            new TranslationSeedItem("entity.ecdetail.isendofline", "en-US", "完成品EOL_us", "完成品EOL（End of Line，0=否 1=是）"),
            // entity.ecdetail.isendofline
            new TranslationSeedItem("entity.ecdetail.isendofline", "ja-JP", "完成品EOL_jp", "完成品EOL（End of Line，0=否 1=是）"),
            // entity.ecdetail.isendofline
            new TranslationSeedItem("entity.ecdetail.isendofline", "zh-CN", "完成品EOL", "完成品EOL（End of Line，0=否 1=是）"),
            // entity.ecdetail.isendofline
            new TranslationSeedItem("entity.ecdetail.isendofline", "zh-HK", "完成品EOL_hk", "完成品EOL（End of Line，0=否 1=是）"),

            // entity.ecdetail.ecolditem
            new TranslationSeedItem("entity.ecdetail.ecolditem", "en-US", "旧料号_us", "旧料号（Ec_olditem）"),
            // entity.ecdetail.ecolditem
            new TranslationSeedItem("entity.ecdetail.ecolditem", "ja-JP", "旧料号_jp", "旧料号（Ec_olditem）"),
            // entity.ecdetail.ecolditem
            new TranslationSeedItem("entity.ecdetail.ecolditem", "zh-CN", "旧料号", "旧料号（Ec_olditem）"),
            // entity.ecdetail.ecolditem
            new TranslationSeedItem("entity.ecdetail.ecolditem", "zh-HK", "旧料号_hk", "旧料号（Ec_olditem）"),

            // entity.ecdetail.ecoldtext
            new TranslationSeedItem("entity.ecdetail.ecoldtext", "en-US", "旧料号描述_us", "旧料号描述（Ec_oldtext）"),
            // entity.ecdetail.ecoldtext
            new TranslationSeedItem("entity.ecdetail.ecoldtext", "ja-JP", "旧料号描述_jp", "旧料号描述（Ec_oldtext）"),
            // entity.ecdetail.ecoldtext
            new TranslationSeedItem("entity.ecdetail.ecoldtext", "zh-CN", "旧料号描述", "旧料号描述（Ec_oldtext）"),
            // entity.ecdetail.ecoldtext
            new TranslationSeedItem("entity.ecdetail.ecoldtext", "zh-HK", "旧料号描述_hk", "旧料号描述（Ec_oldtext）"),

            // entity.ecdetail.ecoldusage
            new TranslationSeedItem("entity.ecdetail.ecoldusage", "en-US", "旧用量_us", "旧用量（Ec_oldusage）"),
            // entity.ecdetail.ecoldusage
            new TranslationSeedItem("entity.ecdetail.ecoldusage", "ja-JP", "旧用量_jp", "旧用量（Ec_oldusage）"),
            // entity.ecdetail.ecoldusage
            new TranslationSeedItem("entity.ecdetail.ecoldusage", "zh-CN", "旧用量", "旧用量（Ec_oldusage）"),
            // entity.ecdetail.ecoldusage
            new TranslationSeedItem("entity.ecdetail.ecoldusage", "zh-HK", "旧用量_hk", "旧用量（Ec_oldusage）"),

            // entity.ecdetail.ecoldposition
            new TranslationSeedItem("entity.ecdetail.ecoldposition", "en-US", "旧位置_us", "旧位置（Ec_oldposition）"),
            // entity.ecdetail.ecoldposition
            new TranslationSeedItem("entity.ecdetail.ecoldposition", "ja-JP", "旧位置_jp", "旧位置（Ec_oldposition）"),
            // entity.ecdetail.ecoldposition
            new TranslationSeedItem("entity.ecdetail.ecoldposition", "zh-CN", "旧位置", "旧位置（Ec_oldposition）"),
            // entity.ecdetail.ecoldposition
            new TranslationSeedItem("entity.ecdetail.ecoldposition", "zh-HK", "旧位置_hk", "旧位置（Ec_oldposition）"),

            // entity.ecdetail.ecoldstock
            new TranslationSeedItem("entity.ecdetail.ecoldstock", "en-US", "旧在库数量_us", "旧在库数量（Ec_oldstock）"),
            // entity.ecdetail.ecoldstock
            new TranslationSeedItem("entity.ecdetail.ecoldstock", "ja-JP", "旧在库数量_jp", "旧在库数量（Ec_oldstock）"),
            // entity.ecdetail.ecoldstock
            new TranslationSeedItem("entity.ecdetail.ecoldstock", "zh-CN", "旧在库数量", "旧在库数量（Ec_oldstock）"),
            // entity.ecdetail.ecoldstock
            new TranslationSeedItem("entity.ecdetail.ecoldstock", "zh-HK", "旧在库数量_hk", "旧在库数量（Ec_oldstock）"),

            // entity.ecdetail.ecoldwarehouse
            new TranslationSeedItem("entity.ecdetail.ecoldwarehouse", "en-US", "旧品仓库_us", "旧品仓库（Ec_oldwarehouse）"),
            // entity.ecdetail.ecoldwarehouse
            new TranslationSeedItem("entity.ecdetail.ecoldwarehouse", "ja-JP", "旧品仓库_jp", "旧品仓库（Ec_oldwarehouse）"),
            // entity.ecdetail.ecoldwarehouse
            new TranslationSeedItem("entity.ecdetail.ecoldwarehouse", "zh-CN", "旧品仓库", "旧品仓库（Ec_oldwarehouse）"),
            // entity.ecdetail.ecoldwarehouse
            new TranslationSeedItem("entity.ecdetail.ecoldwarehouse", "zh-HK", "旧品仓库_hk", "旧品仓库（Ec_oldwarehouse）"),

            // entity.ecdetail.isoldprocurement
            new TranslationSeedItem("entity.ecdetail.isoldprocurement", "en-US", "旧品是否采购_us", "旧品是否采购（0=否 1=是）"),
            // entity.ecdetail.isoldprocurement
            new TranslationSeedItem("entity.ecdetail.isoldprocurement", "ja-JP", "旧品是否采购_jp", "旧品是否采购（0=否 1=是）"),
            // entity.ecdetail.isoldprocurement
            new TranslationSeedItem("entity.ecdetail.isoldprocurement", "zh-CN", "旧品是否采购", "旧品是否采购（0=否 1=是）"),
            // entity.ecdetail.isoldprocurement
            new TranslationSeedItem("entity.ecdetail.isoldprocurement", "zh-HK", "旧品是否采购_hk", "旧品是否采购（0=否 1=是）"),

            // entity.ecdetail.isoldcheck
            new TranslationSeedItem("entity.ecdetail.isoldcheck", "en-US", "旧品是否检查_us", "旧品是否检查（0=否 1=是）"),
            // entity.ecdetail.isoldcheck
            new TranslationSeedItem("entity.ecdetail.isoldcheck", "ja-JP", "旧品是否检查_jp", "旧品是否检查（0=否 1=是）"),
            // entity.ecdetail.isoldcheck
            new TranslationSeedItem("entity.ecdetail.isoldcheck", "zh-CN", "旧品是否检查", "旧品是否检查（0=否 1=是）"),
            // entity.ecdetail.isoldcheck
            new TranslationSeedItem("entity.ecdetail.isoldcheck", "zh-HK", "旧品是否检查_hk", "旧品是否检查（0=否 1=是）"),

            // entity.ecdetail.ecnewitem
            new TranslationSeedItem("entity.ecdetail.ecnewitem", "en-US", "新料号_us", "新料号（Ec_newitem）"),
            // entity.ecdetail.ecnewitem
            new TranslationSeedItem("entity.ecdetail.ecnewitem", "ja-JP", "新料号_jp", "新料号（Ec_newitem）"),
            // entity.ecdetail.ecnewitem
            new TranslationSeedItem("entity.ecdetail.ecnewitem", "zh-CN", "新料号", "新料号（Ec_newitem）"),
            // entity.ecdetail.ecnewitem
            new TranslationSeedItem("entity.ecdetail.ecnewitem", "zh-HK", "新料号_hk", "新料号（Ec_newitem）"),

            // entity.ecdetail.ecnewtext
            new TranslationSeedItem("entity.ecdetail.ecnewtext", "en-US", "新料号描述_us", "新料号描述（Ec_newtext）"),
            // entity.ecdetail.ecnewtext
            new TranslationSeedItem("entity.ecdetail.ecnewtext", "ja-JP", "新料号描述_jp", "新料号描述（Ec_newtext）"),
            // entity.ecdetail.ecnewtext
            new TranslationSeedItem("entity.ecdetail.ecnewtext", "zh-CN", "新料号描述", "新料号描述（Ec_newtext）"),
            // entity.ecdetail.ecnewtext
            new TranslationSeedItem("entity.ecdetail.ecnewtext", "zh-HK", "新料号描述_hk", "新料号描述（Ec_newtext）"),

            // entity.ecdetail.ecnewusage
            new TranslationSeedItem("entity.ecdetail.ecnewusage", "en-US", "新用量_us", "新用量（Ec_newusage）"),
            // entity.ecdetail.ecnewusage
            new TranslationSeedItem("entity.ecdetail.ecnewusage", "ja-JP", "新用量_jp", "新用量（Ec_newusage）"),
            // entity.ecdetail.ecnewusage
            new TranslationSeedItem("entity.ecdetail.ecnewusage", "zh-CN", "新用量", "新用量（Ec_newusage）"),
            // entity.ecdetail.ecnewusage
            new TranslationSeedItem("entity.ecdetail.ecnewusage", "zh-HK", "新用量_hk", "新用量（Ec_newusage）"),

            // entity.ecdetail.ecnewposition
            new TranslationSeedItem("entity.ecdetail.ecnewposition", "en-US", "新位置_us", "新位置（Ec_newposition）"),
            // entity.ecdetail.ecnewposition
            new TranslationSeedItem("entity.ecdetail.ecnewposition", "ja-JP", "新位置_jp", "新位置（Ec_newposition）"),
            // entity.ecdetail.ecnewposition
            new TranslationSeedItem("entity.ecdetail.ecnewposition", "zh-CN", "新位置", "新位置（Ec_newposition）"),
            // entity.ecdetail.ecnewposition
            new TranslationSeedItem("entity.ecdetail.ecnewposition", "zh-HK", "新位置_hk", "新位置（Ec_newposition）"),

            // entity.ecdetail.ecnewstock
            new TranslationSeedItem("entity.ecdetail.ecnewstock", "en-US", "新在库数量_us", "新在库数量（Ec_newstock）"),
            // entity.ecdetail.ecnewstock
            new TranslationSeedItem("entity.ecdetail.ecnewstock", "ja-JP", "新在库数量_jp", "新在库数量（Ec_newstock）"),
            // entity.ecdetail.ecnewstock
            new TranslationSeedItem("entity.ecdetail.ecnewstock", "zh-CN", "新在库数量", "新在库数量（Ec_newstock）"),
            // entity.ecdetail.ecnewstock
            new TranslationSeedItem("entity.ecdetail.ecnewstock", "zh-HK", "新在库数量_hk", "新在库数量（Ec_newstock）"),

            // entity.ecdetail.ecnewwarehouse
            new TranslationSeedItem("entity.ecdetail.ecnewwarehouse", "en-US", "新品仓库_us", "新品仓库（Ec_newwarehouse）"),
            // entity.ecdetail.ecnewwarehouse
            new TranslationSeedItem("entity.ecdetail.ecnewwarehouse", "ja-JP", "新品仓库_jp", "新品仓库（Ec_newwarehouse）"),
            // entity.ecdetail.ecnewwarehouse
            new TranslationSeedItem("entity.ecdetail.ecnewwarehouse", "zh-CN", "新品仓库", "新品仓库（Ec_newwarehouse）"),
            // entity.ecdetail.ecnewwarehouse
            new TranslationSeedItem("entity.ecdetail.ecnewwarehouse", "zh-HK", "新品仓库_hk", "新品仓库（Ec_newwarehouse）"),

            // entity.ecdetail.isnewprocurement
            new TranslationSeedItem("entity.ecdetail.isnewprocurement", "en-US", "新品是否采购_us", "新品是否采购（0=否 1=是）"),
            // entity.ecdetail.isnewprocurement
            new TranslationSeedItem("entity.ecdetail.isnewprocurement", "ja-JP", "新品是否采购_jp", "新品是否采购（0=否 1=是）"),
            // entity.ecdetail.isnewprocurement
            new TranslationSeedItem("entity.ecdetail.isnewprocurement", "zh-CN", "新品是否采购", "新品是否采购（0=否 1=是）"),
            // entity.ecdetail.isnewprocurement
            new TranslationSeedItem("entity.ecdetail.isnewprocurement", "zh-HK", "新品是否采购_hk", "新品是否采购（0=否 1=是）"),

            // entity.ecdetail.isnewcheck
            new TranslationSeedItem("entity.ecdetail.isnewcheck", "en-US", "新品是否检查_us", "新品是否检查（0=否 1=是）"),
            // entity.ecdetail.isnewcheck
            new TranslationSeedItem("entity.ecdetail.isnewcheck", "ja-JP", "新品是否检查_jp", "新品是否检查（0=否 1=是）"),
            // entity.ecdetail.isnewcheck
            new TranslationSeedItem("entity.ecdetail.isnewcheck", "zh-CN", "新品是否检查", "新品是否检查（0=否 1=是）"),
            // entity.ecdetail.isnewcheck
            new TranslationSeedItem("entity.ecdetail.isnewcheck", "zh-HK", "新品是否检查_hk", "新品是否检查（0=否 1=是）"),

            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "en-US", "BOM生效日期_us", "BOM生效日期（Ec_bomdate）"),
            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "ja-JP", "BOM生效日期_jp", "BOM生效日期（Ec_bomdate）"),
            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "zh-CN", "BOM生效日期", "BOM生效日期（Ec_bomdate）"),
            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "zh-HK", "BOM生效日期_hk", "BOM生效日期（Ec_bomdate）"),

            // entity.ecdetail.eciscompatible
            new TranslationSeedItem("entity.ecdetail.eciscompatible", "en-US", "兼容性_us", "兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）"),
            // entity.ecdetail.eciscompatible
            new TranslationSeedItem("entity.ecdetail.eciscompatible", "ja-JP", "兼容性_jp", "兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）"),
            // entity.ecdetail.eciscompatible
            new TranslationSeedItem("entity.ecdetail.eciscompatible", "zh-CN", "兼容性", "兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）"),
            // entity.ecdetail.eciscompatible
            new TranslationSeedItem("entity.ecdetail.eciscompatible", "zh-HK", "兼容性_hk", "兼容性（字典 logistics_ec_source_compatibility；A=兼容，B=单向兼容（新替旧），C=单向兼容（旧替新），D=不兼容）"),

            // entity.ecdetail.ecseconddistinction
            new TranslationSeedItem("entity.ecdetail.ecseconddistinction", "en-US", "二级区分_us", "二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）"),
            // entity.ecdetail.ecseconddistinction
            new TranslationSeedItem("entity.ecdetail.ecseconddistinction", "ja-JP", "二级区分_jp", "二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）"),
            // entity.ecdetail.ecseconddistinction
            new TranslationSeedItem("entity.ecdetail.ecseconddistinction", "zh-CN", "二级区分", "二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）"),
            // entity.ecdetail.ecseconddistinction
            new TranslationSeedItem("entity.ecdetail.ecseconddistinction", "zh-HK", "二级区分_hk", "二级区分（字典 logistics_ec_source_distinction；1=有，2=优先，3=无）"),

            // entity.ecdetail.ecinstruction
            new TranslationSeedItem("entity.ecdetail.ecinstruction", "en-US", "生产指令_us", "生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）"),
            // entity.ecdetail.ecinstruction
            new TranslationSeedItem("entity.ecdetail.ecinstruction", "ja-JP", "生产指令_jp", "生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）"),
            // entity.ecdetail.ecinstruction
            new TranslationSeedItem("entity.ecdetail.ecinstruction", "zh-CN", "生产指令", "生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）"),
            // entity.ecdetail.ecinstruction
            new TranslationSeedItem("entity.ecdetail.ecinstruction", "zh-HK", "生产指令_hk", "生产指令（字典 logistics_ec_source_instruction；1=已出货成品，2=在线半成品，3=库存零件，4=外协在制品，5=新下达订单，9=未定）"),

            // entity.ecdetail.eclegacypartdisposition
            new TranslationSeedItem("entity.ecdetail.eclegacypartdisposition", "en-US", "旧品处理_us", "旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）"),
            // entity.ecdetail.eclegacypartdisposition
            new TranslationSeedItem("entity.ecdetail.eclegacypartdisposition", "ja-JP", "旧品处理_jp", "旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）"),
            // entity.ecdetail.eclegacypartdisposition
            new TranslationSeedItem("entity.ecdetail.eclegacypartdisposition", "zh-CN", "旧品处理", "旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）"),
            // entity.ecdetail.eclegacypartdisposition
            new TranslationSeedItem("entity.ecdetail.eclegacypartdisposition", "zh-HK", "旧品处理_hk", "旧品处理（字典 logistics_ec_legacy_part_disposition；1=转用，2=废弃，3=返工，4=消耗，5=无处理，9=未定）"),

            // entity.ecdetail.isobsolete
            new TranslationSeedItem("entity.ecdetail.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecdetail.isobsolete
            new TranslationSeedItem("entity.ecdetail.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecdetail.isobsolete
            new TranslationSeedItem("entity.ecdetail.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ecdetail.isobsolete
            new TranslationSeedItem("entity.ecdetail.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）"),

            // entity.ecdetail.ecgijutsu
            new TranslationSeedItem("entity.ecdetail.ecgijutsu", "en-US", "设变技术课主表_us", "设变技术课主表（多对一）"),
            // entity.ecdetail.ecgijutsu
            new TranslationSeedItem("entity.ecdetail.ecgijutsu", "ja-JP", "设变技术课主表_jp", "设变技术课主表（多对一）"),
            // entity.ecdetail.ecgijutsu
            new TranslationSeedItem("entity.ecdetail.ecgijutsu", "zh-CN", "设变技术课主表", "设变技术课主表（多对一）"),
            // entity.ecdetail.ecgijutsu
            new TranslationSeedItem("entity.ecdetail.ecgijutsu", "zh-HK", "设变技术课主表_hk", "设变技术课主表（多对一）"),

            // entity.ecdetail.ecseikan
            new TranslationSeedItem("entity.ecdetail.ecseikan", "en-US", "生管课执行行_us", "生管课执行行（TaktEcSeikan，每明细一行）"),
            // entity.ecdetail.ecseikan
            new TranslationSeedItem("entity.ecdetail.ecseikan", "ja-JP", "生管课执行行_jp", "生管课执行行（TaktEcSeikan，每明细一行）"),
            // entity.ecdetail.ecseikan
            new TranslationSeedItem("entity.ecdetail.ecseikan", "zh-CN", "生管课执行行", "生管课执行行（TaktEcSeikan，每明细一行）"),
            // entity.ecdetail.ecseikan
            new TranslationSeedItem("entity.ecdetail.ecseikan", "zh-HK", "生管课执行行_hk", "生管课执行行（TaktEcSeikan，每明细一行）"),

            // entity.ecdetail.eckoubai
            new TranslationSeedItem("entity.ecdetail.eckoubai", "en-US", "采购课执行行_us", "采购课执行行（TaktEcKoubai，每明细一行）"),
            // entity.ecdetail.eckoubai
            new TranslationSeedItem("entity.ecdetail.eckoubai", "ja-JP", "采购课执行行_jp", "采购课执行行（TaktEcKoubai，每明细一行）"),
            // entity.ecdetail.eckoubai
            new TranslationSeedItem("entity.ecdetail.eckoubai", "zh-CN", "采购课执行行", "采购课执行行（TaktEcKoubai，每明细一行）"),
            // entity.ecdetail.eckoubai
            new TranslationSeedItem("entity.ecdetail.eckoubai", "zh-HK", "采购课执行行_hk", "采购课执行行（TaktEcKoubai，每明细一行）"),

            // entity.ecdetail.ecukeken
            new TranslationSeedItem("entity.ecdetail.ecukeken", "en-US", "受检课执行行_us", "受检课执行行（TaktEcUkeken，每明细一行）"),
            // entity.ecdetail.ecukeken
            new TranslationSeedItem("entity.ecdetail.ecukeken", "ja-JP", "受检课执行行_jp", "受检课执行行（TaktEcUkeken，每明细一行）"),
            // entity.ecdetail.ecukeken
            new TranslationSeedItem("entity.ecdetail.ecukeken", "zh-CN", "受检课执行行", "受检课执行行（TaktEcUkeken，每明细一行）"),
            // entity.ecdetail.ecukeken
            new TranslationSeedItem("entity.ecdetail.ecukeken", "zh-HK", "受检课执行行_hk", "受检课执行行（TaktEcUkeken，每明细一行）"),

            // entity.ecdetail.ecbukan
            new TranslationSeedItem("entity.ecdetail.ecbukan", "en-US", "部管课执行行_us", "部管课执行行（TaktEcBukan，每明细一行）"),
            // entity.ecdetail.ecbukan
            new TranslationSeedItem("entity.ecdetail.ecbukan", "ja-JP", "部管课执行行_jp", "部管课执行行（TaktEcBukan，每明细一行）"),
            // entity.ecdetail.ecbukan
            new TranslationSeedItem("entity.ecdetail.ecbukan", "zh-CN", "部管课执行行", "部管课执行行（TaktEcBukan，每明细一行）"),
            // entity.ecdetail.ecbukan
            new TranslationSeedItem("entity.ecdetail.ecbukan", "zh-HK", "部管课执行行_hk", "部管课执行行（TaktEcBukan，每明细一行）"),

            // entity.ecdetail.ecseizounika
            new TranslationSeedItem("entity.ecdetail.ecseizounika", "en-US", "制二课执行行_us", "制二课执行行（TaktEcSeizounika，每明细一行）"),
            // entity.ecdetail.ecseizounika
            new TranslationSeedItem("entity.ecdetail.ecseizounika", "ja-JP", "制二课执行行_jp", "制二课执行行（TaktEcSeizounika，每明细一行）"),
            // entity.ecdetail.ecseizounika
            new TranslationSeedItem("entity.ecdetail.ecseizounika", "zh-CN", "制二课执行行", "制二课执行行（TaktEcSeizounika，每明细一行）"),
            // entity.ecdetail.ecseizounika
            new TranslationSeedItem("entity.ecdetail.ecseizounika", "zh-HK", "制二课执行行_hk", "制二课执行行（TaktEcSeizounika，每明细一行）"),

            // entity.ecdetail.ecseizouikka
            new TranslationSeedItem("entity.ecdetail.ecseizouikka", "en-US", "制一课执行行_us", "制一课执行行（TaktEcSeizouikka，每明细一行）"),
            // entity.ecdetail.ecseizouikka
            new TranslationSeedItem("entity.ecdetail.ecseizouikka", "ja-JP", "制一课执行行_jp", "制一课执行行（TaktEcSeizouikka，每明细一行）"),
            // entity.ecdetail.ecseizouikka
            new TranslationSeedItem("entity.ecdetail.ecseizouikka", "zh-CN", "制一课执行行", "制一课执行行（TaktEcSeizouikka，每明细一行）"),
            // entity.ecdetail.ecseizouikka
            new TranslationSeedItem("entity.ecdetail.ecseizouikka", "zh-HK", "制一课执行行_hk", "制一课执行行（TaktEcSeizouikka，每明细一行）"),

            // entity.ecdetail.echinkan
            new TranslationSeedItem("entity.ecdetail.echinkan", "en-US", "品管课执行行_us", "品管课执行行（TaktEcHinkan，每明细一行）"),
            // entity.ecdetail.echinkan
            new TranslationSeedItem("entity.ecdetail.echinkan", "ja-JP", "品管课执行行_jp", "品管课执行行（TaktEcHinkan，每明细一行）"),
            // entity.ecdetail.echinkan
            new TranslationSeedItem("entity.ecdetail.echinkan", "zh-CN", "品管课执行行", "品管课执行行（TaktEcHinkan，每明细一行）"),
            // entity.ecdetail.echinkan
            new TranslationSeedItem("entity.ecdetail.echinkan", "zh-HK", "品管课执行行_hk", "品管课执行行（TaktEcHinkan，每明细一行）"),

            // entity.ecdetail.ecseizougijutsu
            new TranslationSeedItem("entity.ecdetail.ecseizougijutsu", "en-US", "制技课执行行_us", "制技课执行行（TaktEcSeizougijutsu，每明细一行）"),
            // entity.ecdetail.ecseizougijutsu
            new TranslationSeedItem("entity.ecdetail.ecseizougijutsu", "ja-JP", "制技课执行行_jp", "制技课执行行（TaktEcSeizougijutsu，每明细一行）"),
            // entity.ecdetail.ecseizougijutsu
            new TranslationSeedItem("entity.ecdetail.ecseizougijutsu", "zh-CN", "制技课执行行", "制技课执行行（TaktEcSeizougijutsu，每明细一行）"),
            // entity.ecdetail.ecseizougijutsu
            new TranslationSeedItem("entity.ecdetail.ecseizougijutsu", "zh-HK", "制技课执行行_hk", "制技课执行行（TaktEcSeizougijutsu，每明细一行）"),
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
        translation.ResourceGroup = "EngineeringChange";
        translation.ResourceType = "frontend";
        translation.ContextNote = item.ContextNote;
        translation.ExtField = null;
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
