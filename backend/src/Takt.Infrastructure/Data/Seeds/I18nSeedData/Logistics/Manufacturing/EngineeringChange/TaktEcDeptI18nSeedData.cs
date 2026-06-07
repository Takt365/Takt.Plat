// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEcDept 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktEcDept 实体国际化翻译种子（键前缀 entity.ecDept.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEcDeptI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEcDept 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecDept 实体翻译...", tenantCode);

        foreach (var item in GetEcDeptTranslations())
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

        TaktLogger.Information("TaktEcDept 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEcDept 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ecDept._self / entity.ecDept.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcDeptTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecDept._self
            new TranslationSeedItem("entity.ecDept._self", "en-US", "Ec Dept Information", "实体名称"),
            // entity.ecDept._self
            new TranslationSeedItem("entity.ecDept._self", "ja-JP", "设变-部门通用信息", "实体名称"),
            // entity.ecDept._self
            new TranslationSeedItem("entity.ecDept._self", "zh-CN", "设变-部门通用信息", "实体名称"),
            // entity.ecDept._self
            new TranslationSeedItem("entity.ecDept._self", "zh-HK", "设变-部门通用信息", "实体名称"),

            // entity.ecDept.ecndetailid
            new TranslationSeedItem("entity.ecDept.ecndetailid", "en-US", "设变明细ID", "设变明细ID（TaktEcDetail 主键）"),
            // entity.ecDept.ecndetailid
            new TranslationSeedItem("entity.ecDept.ecndetailid", "ja-JP", "设变明细ID", "设变明细ID（TaktEcDetail 主键）"),
            // entity.ecDept.ecndetailid
            new TranslationSeedItem("entity.ecDept.ecndetailid", "zh-CN", "设变明细ID", "设变明细ID（TaktEcDetail 主键）"),
            // entity.ecDept.ecndetailid
            new TranslationSeedItem("entity.ecDept.ecndetailid", "zh-HK", "设变明细ID", "设变明细ID（TaktEcDetail 主键）"),

            // entity.ecDept.ecno
            new TranslationSeedItem("entity.ecDept.ecno", "en-US", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecDept.ecno
            new TranslationSeedItem("entity.ecDept.ecno", "ja-JP", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecDept.ecno
            new TranslationSeedItem("entity.ecDept.ecno", "zh-CN", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecDept.ecno
            new TranslationSeedItem("entity.ecDept.ecno", "zh-HK", "设变单号", "设变单号（冗余字段,便于查询）"),

            // entity.ecDept.linenumber
            new TranslationSeedItem("entity.ecDept.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecDept.linenumber
            new TranslationSeedItem("entity.ecDept.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecDept.linenumber
            new TranslationSeedItem("entity.ecDept.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecDept.linenumber
            new TranslationSeedItem("entity.ecDept.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.ecDept.deptcode
            new TranslationSeedItem("entity.ecDept.deptcode", "en-US", "部门编码", "部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。"),
            // entity.ecDept.deptcode
            new TranslationSeedItem("entity.ecDept.deptcode", "ja-JP", "部门编码", "部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。"),
            // entity.ecDept.deptcode
            new TranslationSeedItem("entity.ecDept.deptcode", "zh-CN", "部门编码", "部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。"),
            // entity.ecDept.deptcode
            new TranslationSeedItem("entity.ecDept.deptcode", "zh-HK", "部门编码", "部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。"),

            // entity.ecDept.isimplemented
            new TranslationSeedItem("entity.ecDept.isimplemented", "en-US", "是否实施", "是否实施（0=否 1=是）"),
            // entity.ecDept.isimplemented
            new TranslationSeedItem("entity.ecDept.isimplemented", "ja-JP", "是否实施", "是否实施（0=否 1=是）"),
            // entity.ecDept.isimplemented
            new TranslationSeedItem("entity.ecDept.isimplemented", "zh-CN", "是否实施", "是否实施（0=否 1=是）"),
            // entity.ecDept.isimplemented
            new TranslationSeedItem("entity.ecDept.isimplemented", "zh-HK", "是否实施", "是否实施（0=否 1=是）"),

            // entity.ecDept.content
            new TranslationSeedItem("entity.ecDept.content", "en-US", "内容", "内容（各部门通用）"),
            // entity.ecDept.content
            new TranslationSeedItem("entity.ecDept.content", "ja-JP", "内容", "内容（各部门通用）"),
            // entity.ecDept.content
            new TranslationSeedItem("entity.ecDept.content", "zh-CN", "内容", "内容（各部门通用）"),
            // entity.ecDept.content
            new TranslationSeedItem("entity.ecDept.content", "zh-HK", "内容", "内容（各部门通用）"),

            // entity.ecDept.scheduledproductiondate
            new TranslationSeedItem("entity.ecDept.scheduledproductiondate", "en-US", "预计生产日期", "预计生产日期"),
            // entity.ecDept.scheduledproductiondate
            new TranslationSeedItem("entity.ecDept.scheduledproductiondate", "ja-JP", "预计生产日期", "预计生产日期"),
            // entity.ecDept.scheduledproductiondate
            new TranslationSeedItem("entity.ecDept.scheduledproductiondate", "zh-CN", "预计生产日期", "预计生产日期"),
            // entity.ecDept.scheduledproductiondate
            new TranslationSeedItem("entity.ecDept.scheduledproductiondate", "zh-HK", "预计生产日期", "预计生产日期"),

            // entity.ecDept.scheduledbatch
            new TranslationSeedItem("entity.ecDept.scheduledbatch", "en-US", "预定批次", "预定批次"),
            // entity.ecDept.scheduledbatch
            new TranslationSeedItem("entity.ecDept.scheduledbatch", "ja-JP", "预定批次", "预定批次"),
            // entity.ecDept.scheduledbatch
            new TranslationSeedItem("entity.ecDept.scheduledbatch", "zh-CN", "预定批次", "预定批次"),
            // entity.ecDept.scheduledbatch
            new TranslationSeedItem("entity.ecDept.scheduledbatch", "zh-HK", "预定批次", "预定批次"),

            // entity.ecDept.poremainder
            new TranslationSeedItem("entity.ecDept.poremainder", "en-US", "Po残", "Po残（采购订单残）"),
            // entity.ecDept.poremainder
            new TranslationSeedItem("entity.ecDept.poremainder", "ja-JP", "Po残", "Po残（采购订单残）"),
            // entity.ecDept.poremainder
            new TranslationSeedItem("entity.ecDept.poremainder", "zh-CN", "Po残", "Po残（采购订单残）"),
            // entity.ecDept.poremainder
            new TranslationSeedItem("entity.ecDept.poremainder", "zh-HK", "Po残", "Po残（采购订单残）"),

            // entity.ecDept.balance
            new TranslationSeedItem("entity.ecDept.balance", "en-US", "结余", "结余"),
            // entity.ecDept.balance
            new TranslationSeedItem("entity.ecDept.balance", "ja-JP", "结余", "结余"),
            // entity.ecDept.balance
            new TranslationSeedItem("entity.ecDept.balance", "zh-CN", "结余", "结余"),
            // entity.ecDept.balance
            new TranslationSeedItem("entity.ecDept.balance", "zh-HK", "结余", "结余"),

            // entity.ecDept.oldproducthandling
            new TranslationSeedItem("entity.ecDept.oldproducthandling", "en-US", "旧品处理", "旧品处理"),
            // entity.ecDept.oldproducthandling
            new TranslationSeedItem("entity.ecDept.oldproducthandling", "ja-JP", "旧品处理", "旧品处理"),
            // entity.ecDept.oldproducthandling
            new TranslationSeedItem("entity.ecDept.oldproducthandling", "zh-CN", "旧品处理", "旧品处理"),
            // entity.ecDept.oldproducthandling
            new TranslationSeedItem("entity.ecDept.oldproducthandling", "zh-HK", "旧品处理", "旧品处理"),

            // entity.ecDept.purchaseorderissuedate
            new TranslationSeedItem("entity.ecDept.purchaseorderissuedate", "en-US", "采购订单发行日期", "采购订单发行日期"),
            // entity.ecDept.purchaseorderissuedate
            new TranslationSeedItem("entity.ecDept.purchaseorderissuedate", "ja-JP", "采购订单发行日期", "采购订单发行日期"),
            // entity.ecDept.purchaseorderissuedate
            new TranslationSeedItem("entity.ecDept.purchaseorderissuedate", "zh-CN", "采购订单发行日期", "采购订单发行日期"),
            // entity.ecDept.purchaseorderissuedate
            new TranslationSeedItem("entity.ecDept.purchaseorderissuedate", "zh-HK", "采购订单发行日期", "采购订单发行日期"),

            // entity.ecDept.supplier
            new TranslationSeedItem("entity.ecDept.supplier", "en-US", "供应商", "供应商"),
            // entity.ecDept.supplier
            new TranslationSeedItem("entity.ecDept.supplier", "ja-JP", "供应商", "供应商"),
            // entity.ecDept.supplier
            new TranslationSeedItem("entity.ecDept.supplier", "zh-CN", "供应商", "供应商"),
            // entity.ecDept.supplier
            new TranslationSeedItem("entity.ecDept.supplier", "zh-HK", "供应商", "供应商"),

            // entity.ecDept.purchaseorderno
            new TranslationSeedItem("entity.ecDept.purchaseorderno", "en-US", "采购订单号码", "采购订单号码"),
            // entity.ecDept.purchaseorderno
            new TranslationSeedItem("entity.ecDept.purchaseorderno", "ja-JP", "采购订单号码", "采购订单号码"),
            // entity.ecDept.purchaseorderno
            new TranslationSeedItem("entity.ecDept.purchaseorderno", "zh-CN", "采购订单号码", "采购订单号码"),
            // entity.ecDept.purchaseorderno
            new TranslationSeedItem("entity.ecDept.purchaseorderno", "zh-HK", "采购订单号码", "采购订单号码"),

            // entity.ecDept.iqcorderno
            new TranslationSeedItem("entity.ecDept.iqcorderno", "en-US", "受检单号", "受检单号"),
            // entity.ecDept.iqcorderno
            new TranslationSeedItem("entity.ecDept.iqcorderno", "ja-JP", "受检单号", "受检单号"),
            // entity.ecDept.iqcorderno
            new TranslationSeedItem("entity.ecDept.iqcorderno", "zh-CN", "受检单号", "受检单号"),
            // entity.ecDept.iqcorderno
            new TranslationSeedItem("entity.ecDept.iqcorderno", "zh-HK", "受检单号", "受检单号"),

            // entity.ecDept.inspectiondate
            new TranslationSeedItem("entity.ecDept.inspectiondate", "en-US", "检验日期", "检验/检查日期"),
            // entity.ecDept.inspectiondate
            new TranslationSeedItem("entity.ecDept.inspectiondate", "ja-JP", "检验日期", "检验/检查日期"),
            // entity.ecDept.inspectiondate
            new TranslationSeedItem("entity.ecDept.inspectiondate", "zh-CN", "检验日期", "检验/检查日期"),
            // entity.ecDept.inspectiondate
            new TranslationSeedItem("entity.ecDept.inspectiondate", "zh-HK", "检验日期", "检验/检查日期"),

            // entity.ecDept.outboundbatch
            new TranslationSeedItem("entity.ecDept.outboundbatch", "en-US", "出库批次", "出库批次"),
            // entity.ecDept.outboundbatch
            new TranslationSeedItem("entity.ecDept.outboundbatch", "ja-JP", "出库批次", "出库批次"),
            // entity.ecDept.outboundbatch
            new TranslationSeedItem("entity.ecDept.outboundbatch", "zh-CN", "出库批次", "出库批次"),
            // entity.ecDept.outboundbatch
            new TranslationSeedItem("entity.ecDept.outboundbatch", "zh-HK", "出库批次", "出库批次"),

            // entity.ecDept.outbounddate
            new TranslationSeedItem("entity.ecDept.outbounddate", "en-US", "出库日期", "出库日期"),
            // entity.ecDept.outbounddate
            new TranslationSeedItem("entity.ecDept.outbounddate", "ja-JP", "出库日期", "出库日期"),
            // entity.ecDept.outbounddate
            new TranslationSeedItem("entity.ecDept.outbounddate", "zh-CN", "出库日期", "出库日期"),
            // entity.ecDept.outbounddate
            new TranslationSeedItem("entity.ecDept.outbounddate", "zh-HK", "出库日期", "出库日期"),

            // entity.ecDept.productiondate
            new TranslationSeedItem("entity.ecDept.productiondate", "en-US", "生产日期", "生产日期"),
            // entity.ecDept.productiondate
            new TranslationSeedItem("entity.ecDept.productiondate", "ja-JP", "生产日期", "生产日期"),
            // entity.ecDept.productiondate
            new TranslationSeedItem("entity.ecDept.productiondate", "zh-CN", "生产日期", "生产日期"),
            // entity.ecDept.productiondate
            new TranslationSeedItem("entity.ecDept.productiondate", "zh-HK", "生产日期", "生产日期"),

            // entity.ecDept.productionbatch
            new TranslationSeedItem("entity.ecDept.productionbatch", "en-US", "生产批次", "生产批次"),
            // entity.ecDept.productionbatch
            new TranslationSeedItem("entity.ecDept.productionbatch", "ja-JP", "生产批次", "生产批次"),
            // entity.ecDept.productionbatch
            new TranslationSeedItem("entity.ecDept.productionbatch", "zh-CN", "生产批次", "生产批次"),
            // entity.ecDept.productionbatch
            new TranslationSeedItem("entity.ecDept.productionbatch", "zh-HK", "生产批次", "生产批次"),

            // entity.ecDept.outboundorderno
            new TranslationSeedItem("entity.ecDept.outboundorderno", "en-US", "出库单号", "出库单号"),
            // entity.ecDept.outboundorderno
            new TranslationSeedItem("entity.ecDept.outboundorderno", "ja-JP", "出库单号", "出库单号"),
            // entity.ecDept.outboundorderno
            new TranslationSeedItem("entity.ecDept.outboundorderno", "zh-CN", "出库单号", "出库单号"),
            // entity.ecDept.outboundorderno
            new TranslationSeedItem("entity.ecDept.outboundorderno", "zh-HK", "出库单号", "出库单号"),

            // entity.ecDept.productionteam
            new TranslationSeedItem("entity.ecDept.productionteam", "en-US", "生产班组", "生产班组"),
            // entity.ecDept.productionteam
            new TranslationSeedItem("entity.ecDept.productionteam", "ja-JP", "生产班组", "生产班组"),
            // entity.ecDept.productionteam
            new TranslationSeedItem("entity.ecDept.productionteam", "zh-CN", "生产班组", "生产班组"),
            // entity.ecDept.productionteam
            new TranslationSeedItem("entity.ecDept.productionteam", "zh-HK", "生产班组", "生产班组"),

            // entity.ecDept.implementationdate
            new TranslationSeedItem("entity.ecDept.implementationdate", "en-US", "实施日期", "实施日期"),
            // entity.ecDept.implementationdate
            new TranslationSeedItem("entity.ecDept.implementationdate", "ja-JP", "实施日期", "实施日期"),
            // entity.ecDept.implementationdate
            new TranslationSeedItem("entity.ecDept.implementationdate", "zh-CN", "实施日期", "实施日期"),
            // entity.ecDept.implementationdate
            new TranslationSeedItem("entity.ecDept.implementationdate", "zh-HK", "实施日期", "实施日期"),

            // entity.ecDept.inspectionbatch
            new TranslationSeedItem("entity.ecDept.inspectionbatch", "en-US", "检验批次", "检验批次"),
            // entity.ecDept.inspectionbatch
            new TranslationSeedItem("entity.ecDept.inspectionbatch", "ja-JP", "检验批次", "检验批次"),
            // entity.ecDept.inspectionbatch
            new TranslationSeedItem("entity.ecDept.inspectionbatch", "zh-CN", "检验批次", "检验批次"),
            // entity.ecDept.inspectionbatch
            new TranslationSeedItem("entity.ecDept.inspectionbatch", "zh-HK", "检验批次", "检验批次"),

            // entity.ecDept.samplingno
            new TranslationSeedItem("entity.ecDept.samplingno", "en-US", "抽样号码", "抽样号码"),
            // entity.ecDept.samplingno
            new TranslationSeedItem("entity.ecDept.samplingno", "ja-JP", "抽样号码", "抽样号码"),
            // entity.ecDept.samplingno
            new TranslationSeedItem("entity.ecDept.samplingno", "zh-CN", "抽样号码", "抽样号码"),
            // entity.ecDept.samplingno
            new TranslationSeedItem("entity.ecDept.samplingno", "zh-HK", "抽样号码", "抽样号码"),

            // entity.ecDept.issopupdated
            new TranslationSeedItem("entity.ecDept.issopupdated", "en-US", "是否更新SOP", "是否更新SOP（0=否 1=是）"),
            // entity.ecDept.issopupdated
            new TranslationSeedItem("entity.ecDept.issopupdated", "ja-JP", "是否更新SOP", "是否更新SOP（0=否 1=是）"),
            // entity.ecDept.issopupdated
            new TranslationSeedItem("entity.ecDept.issopupdated", "zh-CN", "是否更新SOP", "是否更新SOP（0=否 1=是）"),
            // entity.ecDept.issopupdated
            new TranslationSeedItem("entity.ecDept.issopupdated", "zh-HK", "是否更新SOP", "是否更新SOP（0=否 1=是）"),
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
