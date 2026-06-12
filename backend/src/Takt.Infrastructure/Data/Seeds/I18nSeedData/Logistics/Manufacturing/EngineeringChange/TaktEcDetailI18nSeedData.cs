// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDetailI18nSeedData.cs
// 创建时间：2026-06-12
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
    /// I18nKey：entity.ecdetail._self / entity.ecdetail.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetEcDetailTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "en-US", "Ec Detail Information", "实体名称"),
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "ja-JP", "设变信息", "实体名称"),
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "zh-CN", "设变信息", "实体名称"),
            // entity.ecdetail._self
            new TranslationSeedItem("entity.ecdetail._self", "zh-HK", "设变信息", "实体名称"),

            // entity.ecdetail.ecid
            new TranslationSeedItem("entity.ecdetail.ecid", "en-US", "设变ID", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.ecdetail.ecid
            new TranslationSeedItem("entity.ecdetail.ecid", "ja-JP", "设变ID", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.ecdetail.ecid
            new TranslationSeedItem("entity.ecdetail.ecid", "zh-CN", "设变ID", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),
            // entity.ecdetail.ecid
            new TranslationSeedItem("entity.ecdetail.ecid", "zh-HK", "设变ID", "设变主表ID（主表主键,序列化为string以避免Javascript精度问题）"),

            // entity.ecdetail.ecno
            new TranslationSeedItem("entity.ecdetail.ecno", "en-US", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecdetail.ecno
            new TranslationSeedItem("entity.ecdetail.ecno", "ja-JP", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecdetail.ecno
            new TranslationSeedItem("entity.ecdetail.ecno", "zh-CN", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecdetail.ecno
            new TranslationSeedItem("entity.ecdetail.ecno", "zh-HK", "设变单号", "设变单号（冗余字段,便于查询）"),

            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecdetail.linenumber
            new TranslationSeedItem("entity.ecdetail.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.ecdetail.ecmodel
            new TranslationSeedItem("entity.ecdetail.ecmodel", "en-US", "型号", "型号（Ec_model）"),
            // entity.ecdetail.ecmodel
            new TranslationSeedItem("entity.ecdetail.ecmodel", "ja-JP", "型号", "型号（Ec_model）"),
            // entity.ecdetail.ecmodel
            new TranslationSeedItem("entity.ecdetail.ecmodel", "zh-CN", "型号", "型号（Ec_model）"),
            // entity.ecdetail.ecmodel
            new TranslationSeedItem("entity.ecdetail.ecmodel", "zh-HK", "型号", "型号（Ec_model）"),

            // entity.ecdetail.ecbomitem
            new TranslationSeedItem("entity.ecdetail.ecbomitem", "en-US", "BOM主项料号", "BOM 主项料号（Ec_bomitem）"),
            // entity.ecdetail.ecbomitem
            new TranslationSeedItem("entity.ecdetail.ecbomitem", "ja-JP", "BOM主项料号", "BOM 主项料号（Ec_bomitem）"),
            // entity.ecdetail.ecbomitem
            new TranslationSeedItem("entity.ecdetail.ecbomitem", "zh-CN", "BOM主项料号", "BOM 主项料号（Ec_bomitem）"),
            // entity.ecdetail.ecbomitem
            new TranslationSeedItem("entity.ecdetail.ecbomitem", "zh-HK", "BOM主项料号", "BOM 主项料号（Ec_bomitem）"),

            // entity.ecdetail.ecbomsubitem
            new TranslationSeedItem("entity.ecdetail.ecbomsubitem", "en-US", "BOM子项料号", "BOM 子项料号（Ec_bomsubitem）"),
            // entity.ecdetail.ecbomsubitem
            new TranslationSeedItem("entity.ecdetail.ecbomsubitem", "ja-JP", "BOM子项料号", "BOM 子项料号（Ec_bomsubitem）"),
            // entity.ecdetail.ecbomsubitem
            new TranslationSeedItem("entity.ecdetail.ecbomsubitem", "zh-CN", "BOM子项料号", "BOM 子项料号（Ec_bomsubitem）"),
            // entity.ecdetail.ecbomsubitem
            new TranslationSeedItem("entity.ecdetail.ecbomsubitem", "zh-HK", "BOM子项料号", "BOM 子项料号（Ec_bomsubitem）"),

            // entity.ecdetail.ecbomno
            new TranslationSeedItem("entity.ecdetail.ecbomno", "en-US", "BOM编号", "BOM 编号（Ec_bomno）"),
            // entity.ecdetail.ecbomno
            new TranslationSeedItem("entity.ecdetail.ecbomno", "ja-JP", "BOM编号", "BOM 编号（Ec_bomno）"),
            // entity.ecdetail.ecbomno
            new TranslationSeedItem("entity.ecdetail.ecbomno", "zh-CN", "BOM编号", "BOM 编号（Ec_bomno）"),
            // entity.ecdetail.ecbomno
            new TranslationSeedItem("entity.ecdetail.ecbomno", "zh-HK", "BOM编号", "BOM 编号（Ec_bomno）"),

            // entity.ecdetail.ecchange
            new TranslationSeedItem("entity.ecdetail.ecchange", "en-US", "变更内容", "变更内容（Ec_change）"),
            // entity.ecdetail.ecchange
            new TranslationSeedItem("entity.ecdetail.ecchange", "ja-JP", "变更内容", "变更内容（Ec_change）"),
            // entity.ecdetail.ecchange
            new TranslationSeedItem("entity.ecdetail.ecchange", "zh-CN", "变更内容", "变更内容（Ec_change）"),
            // entity.ecdetail.ecchange
            new TranslationSeedItem("entity.ecdetail.ecchange", "zh-HK", "变更内容", "变更内容（Ec_change）"),

            // entity.ecdetail.eclocal
            new TranslationSeedItem("entity.ecdetail.eclocal", "en-US", "本地现场", "本地/现场（Ec_local）"),
            // entity.ecdetail.eclocal
            new TranslationSeedItem("entity.ecdetail.eclocal", "ja-JP", "本地现场", "本地/现场（Ec_local）"),
            // entity.ecdetail.eclocal
            new TranslationSeedItem("entity.ecdetail.eclocal", "zh-CN", "本地现场", "本地/现场（Ec_local）"),
            // entity.ecdetail.eclocal
            new TranslationSeedItem("entity.ecdetail.eclocal", "zh-HK", "本地现场", "本地/现场（Ec_local）"),

            // entity.ecdetail.ecnote
            new TranslationSeedItem("entity.ecdetail.ecnote", "en-US", "备注", "备注（Ec_note）"),
            // entity.ecdetail.ecnote
            new TranslationSeedItem("entity.ecdetail.ecnote", "ja-JP", "备注", "备注（Ec_note）"),
            // entity.ecdetail.ecnote
            new TranslationSeedItem("entity.ecdetail.ecnote", "zh-CN", "备注", "备注（Ec_note）"),
            // entity.ecdetail.ecnote
            new TranslationSeedItem("entity.ecdetail.ecnote", "zh-HK", "备注", "备注（Ec_note）"),

            // entity.ecdetail.ecprocess
            new TranslationSeedItem("entity.ecdetail.ecprocess", "en-US", "工序", "工序（Ec_process）"),
            // entity.ecdetail.ecprocess
            new TranslationSeedItem("entity.ecdetail.ecprocess", "ja-JP", "工序", "工序（Ec_process）"),
            // entity.ecdetail.ecprocess
            new TranslationSeedItem("entity.ecdetail.ecprocess", "zh-CN", "工序", "工序（Ec_process）"),
            // entity.ecdetail.ecprocess
            new TranslationSeedItem("entity.ecdetail.ecprocess", "zh-HK", "工序", "工序（Ec_process）"),

            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "en-US", "BOM日期", "BOM 日期（Ec_bomdate）"),
            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "ja-JP", "BOM日期", "BOM 日期（Ec_bomdate）"),
            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "zh-CN", "BOM日期", "BOM 日期（Ec_bomdate）"),
            // entity.ecdetail.ecbomdate
            new TranslationSeedItem("entity.ecdetail.ecbomdate", "zh-HK", "BOM日期", "BOM 日期（Ec_bomdate）"),

            // entity.ecdetail.ecentrydate
            new TranslationSeedItem("entity.ecdetail.ecentrydate", "en-US", "录入日期", "录入日期（Ec_entrydate）"),
            // entity.ecdetail.ecentrydate
            new TranslationSeedItem("entity.ecdetail.ecentrydate", "ja-JP", "录入日期", "录入日期（Ec_entrydate）"),
            // entity.ecdetail.ecentrydate
            new TranslationSeedItem("entity.ecdetail.ecentrydate", "zh-CN", "录入日期", "录入日期（Ec_entrydate）"),
            // entity.ecdetail.ecentrydate
            new TranslationSeedItem("entity.ecdetail.ecentrydate", "zh-HK", "录入日期", "录入日期（Ec_entrydate）"),

            // entity.ecdetail.ecolditem
            new TranslationSeedItem("entity.ecdetail.ecolditem", "en-US", "旧料号", "旧料号（Ec_olditem）"),
            // entity.ecdetail.ecolditem
            new TranslationSeedItem("entity.ecdetail.ecolditem", "ja-JP", "旧料号", "旧料号（Ec_olditem）"),
            // entity.ecdetail.ecolditem
            new TranslationSeedItem("entity.ecdetail.ecolditem", "zh-CN", "旧料号", "旧料号（Ec_olditem）"),
            // entity.ecdetail.ecolditem
            new TranslationSeedItem("entity.ecdetail.ecolditem", "zh-HK", "旧料号", "旧料号（Ec_olditem）"),

            // entity.ecdetail.ecoldtext
            new TranslationSeedItem("entity.ecdetail.ecoldtext", "en-US", "旧料号描述", "旧料号描述（Ec_oldtext）"),
            // entity.ecdetail.ecoldtext
            new TranslationSeedItem("entity.ecdetail.ecoldtext", "ja-JP", "旧料号描述", "旧料号描述（Ec_oldtext）"),
            // entity.ecdetail.ecoldtext
            new TranslationSeedItem("entity.ecdetail.ecoldtext", "zh-CN", "旧料号描述", "旧料号描述（Ec_oldtext）"),
            // entity.ecdetail.ecoldtext
            new TranslationSeedItem("entity.ecdetail.ecoldtext", "zh-HK", "旧料号描述", "旧料号描述（Ec_oldtext）"),

            // entity.ecdetail.ecoldqty
            new TranslationSeedItem("entity.ecdetail.ecoldqty", "en-US", "旧数量", "旧数量（Ec_oldqty）"),
            // entity.ecdetail.ecoldqty
            new TranslationSeedItem("entity.ecdetail.ecoldqty", "ja-JP", "旧数量", "旧数量（Ec_oldqty）"),
            // entity.ecdetail.ecoldqty
            new TranslationSeedItem("entity.ecdetail.ecoldqty", "zh-CN", "旧数量", "旧数量（Ec_oldqty）"),
            // entity.ecdetail.ecoldqty
            new TranslationSeedItem("entity.ecdetail.ecoldqty", "zh-HK", "旧数量", "旧数量（Ec_oldqty）"),

            // entity.ecdetail.ecoldset
            new TranslationSeedItem("entity.ecdetail.ecoldset", "en-US", "旧单位", "旧单位/设置（Ec_oldset）"),
            // entity.ecdetail.ecoldset
            new TranslationSeedItem("entity.ecdetail.ecoldset", "ja-JP", "旧单位", "旧单位/设置（Ec_oldset）"),
            // entity.ecdetail.ecoldset
            new TranslationSeedItem("entity.ecdetail.ecoldset", "zh-CN", "旧单位", "旧单位/设置（Ec_oldset）"),
            // entity.ecdetail.ecoldset
            new TranslationSeedItem("entity.ecdetail.ecoldset", "zh-HK", "旧单位", "旧单位/设置（Ec_oldset）"),

            // entity.ecdetail.ecnewitem
            new TranslationSeedItem("entity.ecdetail.ecnewitem", "en-US", "新料号", "新料号（Ec_newitem）"),
            // entity.ecdetail.ecnewitem
            new TranslationSeedItem("entity.ecdetail.ecnewitem", "ja-JP", "新料号", "新料号（Ec_newitem）"),
            // entity.ecdetail.ecnewitem
            new TranslationSeedItem("entity.ecdetail.ecnewitem", "zh-CN", "新料号", "新料号（Ec_newitem）"),
            // entity.ecdetail.ecnewitem
            new TranslationSeedItem("entity.ecdetail.ecnewitem", "zh-HK", "新料号", "新料号（Ec_newitem）"),

            // entity.ecdetail.ecnewtext
            new TranslationSeedItem("entity.ecdetail.ecnewtext", "en-US", "新料号描述", "新料号描述（Ec_newtext）"),
            // entity.ecdetail.ecnewtext
            new TranslationSeedItem("entity.ecdetail.ecnewtext", "ja-JP", "新料号描述", "新料号描述（Ec_newtext）"),
            // entity.ecdetail.ecnewtext
            new TranslationSeedItem("entity.ecdetail.ecnewtext", "zh-CN", "新料号描述", "新料号描述（Ec_newtext）"),
            // entity.ecdetail.ecnewtext
            new TranslationSeedItem("entity.ecdetail.ecnewtext", "zh-HK", "新料号描述", "新料号描述（Ec_newtext）"),

            // entity.ecdetail.ecnewqty
            new TranslationSeedItem("entity.ecdetail.ecnewqty", "en-US", "新数量", "新数量（Ec_newqty）"),
            // entity.ecdetail.ecnewqty
            new TranslationSeedItem("entity.ecdetail.ecnewqty", "ja-JP", "新数量", "新数量（Ec_newqty）"),
            // entity.ecdetail.ecnewqty
            new TranslationSeedItem("entity.ecdetail.ecnewqty", "zh-CN", "新数量", "新数量（Ec_newqty）"),
            // entity.ecdetail.ecnewqty
            new TranslationSeedItem("entity.ecdetail.ecnewqty", "zh-HK", "新数量", "新数量（Ec_newqty）"),

            // entity.ecdetail.ecnewset
            new TranslationSeedItem("entity.ecdetail.ecnewset", "en-US", "新单位", "新单位/设置（Ec_newset）"),
            // entity.ecdetail.ecnewset
            new TranslationSeedItem("entity.ecdetail.ecnewset", "ja-JP", "新单位", "新单位/设置（Ec_newset）"),
            // entity.ecdetail.ecnewset
            new TranslationSeedItem("entity.ecdetail.ecnewset", "zh-CN", "新单位", "新单位/设置（Ec_newset）"),
            // entity.ecdetail.ecnewset
            new TranslationSeedItem("entity.ecdetail.ecnewset", "zh-HK", "新单位", "新单位/设置（Ec_newset）"),

            // entity.ecdetail.isprocurement
            new TranslationSeedItem("entity.ecdetail.isprocurement", "en-US", "是否采购", "是否采购（0=否 1=是）"),
            // entity.ecdetail.isprocurement
            new TranslationSeedItem("entity.ecdetail.isprocurement", "ja-JP", "是否采购", "是否采购（0=否 1=是）"),
            // entity.ecdetail.isprocurement
            new TranslationSeedItem("entity.ecdetail.isprocurement", "zh-CN", "是否采购", "是否采购（0=否 1=是）"),
            // entity.ecdetail.isprocurement
            new TranslationSeedItem("entity.ecdetail.isprocurement", "zh-HK", "是否采购", "是否采购（0=否 1=是）"),

            // entity.ecdetail.ischeck
            new TranslationSeedItem("entity.ecdetail.ischeck", "en-US", "是否检查", "是否检查（0=否 1=是）"),
            // entity.ecdetail.ischeck
            new TranslationSeedItem("entity.ecdetail.ischeck", "ja-JP", "是否检查", "是否检查（0=否 1=是）"),
            // entity.ecdetail.ischeck
            new TranslationSeedItem("entity.ecdetail.ischeck", "zh-CN", "是否检查", "是否检查（0=否 1=是）"),
            // entity.ecdetail.ischeck
            new TranslationSeedItem("entity.ecdetail.ischeck", "zh-HK", "是否检查", "是否检查（0=否 1=是）"),

            // entity.ecdetail.ecwarehouse
            new TranslationSeedItem("entity.ecdetail.ecwarehouse", "en-US", "仓库", "仓库（Ec_warehouse）"),
            // entity.ecdetail.ecwarehouse
            new TranslationSeedItem("entity.ecdetail.ecwarehouse", "ja-JP", "仓库", "仓库（Ec_warehouse）"),
            // entity.ecdetail.ecwarehouse
            new TranslationSeedItem("entity.ecdetail.ecwarehouse", "zh-CN", "仓库", "仓库（Ec_warehouse）"),
            // entity.ecdetail.ecwarehouse
            new TranslationSeedItem("entity.ecdetail.ecwarehouse", "zh-HK", "仓库", "仓库（Ec_warehouse）"),

            // entity.ecdetail.isendofline
            new TranslationSeedItem("entity.ecdetail.isendofline", "en-US", "EOL", "EOL（End of Line，0=否 1=是）"),
            // entity.ecdetail.isendofline
            new TranslationSeedItem("entity.ecdetail.isendofline", "ja-JP", "EOL", "EOL（End of Line，0=否 1=是）"),
            // entity.ecdetail.isendofline
            new TranslationSeedItem("entity.ecdetail.isendofline", "zh-CN", "EOL", "EOL（End of Line，0=否 1=是）"),
            // entity.ecdetail.isendofline
            new TranslationSeedItem("entity.ecdetail.isendofline", "zh-HK", "EOL", "EOL（End of Line，0=否 1=是）"),

            // entity.ecdetail.ec
            new TranslationSeedItem("entity.ecdetail.ec", "en-US", "设变主表", "设变主表"),
            // entity.ecdetail.ec
            new TranslationSeedItem("entity.ecdetail.ec", "ja-JP", "设变主表", "设变主表"),
            // entity.ecdetail.ec
            new TranslationSeedItem("entity.ecdetail.ec", "zh-CN", "设变主表", "设变主表"),
            // entity.ecdetail.ec
            new TranslationSeedItem("entity.ecdetail.ec", "zh-HK", "设变主表", "设变主表"),

            // entity.ecdetail.deptrecords
            new TranslationSeedItem("entity.ecdetail.deptrecords", "en-US", "设变明细-部门记录列表", "设变明细-部门记录列表（按 DeptCode 区分部门：Assy/It/Cus/Fins/Gas/Iqc/Mc/Mp/Pcba/Pmc/Qa/Te/Eng）"),
            // entity.ecdetail.deptrecords
            new TranslationSeedItem("entity.ecdetail.deptrecords", "ja-JP", "设变明细-部门记录列表", "设变明细-部门记录列表（按 DeptCode 区分部门：Assy/It/Cus/Fins/Gas/Iqc/Mc/Mp/Pcba/Pmc/Qa/Te/Eng）"),
            // entity.ecdetail.deptrecords
            new TranslationSeedItem("entity.ecdetail.deptrecords", "zh-CN", "设变明细-部门记录列表", "设变明细-部门记录列表（按 DeptCode 区分部门：Assy/It/Cus/Fins/Gas/Iqc/Mc/Mp/Pcba/Pmc/Qa/Te/Eng）"),
            // entity.ecdetail.deptrecords
            new TranslationSeedItem("entity.ecdetail.deptrecords", "zh-HK", "设变明细-部门记录列表", "设变明细-部门记录列表（按 DeptCode 区分部门：Assy/It/Cus/Fins/Gas/Iqc/Mc/Mp/Pcba/Pmc/Qa/Te/Eng）"),
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
