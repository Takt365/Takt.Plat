// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptI18nSeedData.cs
// 创建时间：2026-06-22
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// TaktEcDept 实体国际化翻译种子（键前缀 entity.ecdept.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ecdept 实体翻译...", tenantCode);

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
    /// I18nKey：entity.ecdept._self / entity.ecdept.{{field}}；ResourceGroup=EngineeringChange；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEcDeptTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ecdept._self
            new TranslationSeedItem("entity.ecdept._self", "en-US", "Ec Dept Information_us", "实体名称"),
            // entity.ecdept._self
            new TranslationSeedItem("entity.ecdept._self", "ja-JP", "设变-部门通用信息_jp", "实体名称"),
            // entity.ecdept._self
            new TranslationSeedItem("entity.ecdept._self", "zh-CN", "设变-部门通用信息", "实体名称"),
            // entity.ecdept._self
            new TranslationSeedItem("entity.ecdept._self", "zh-HK", "设变-部门通用信息_hk", "实体名称"),

            // entity.ecdept.ecndetailid
            new TranslationSeedItem("entity.ecdept.ecndetailid", "en-US", "设变明细ID_us", "设变明细ID（TaktEcDetail 主键）"),
            // entity.ecdept.ecndetailid
            new TranslationSeedItem("entity.ecdept.ecndetailid", "ja-JP", "设变明细ID_jp", "设变明细ID（TaktEcDetail 主键）"),
            // entity.ecdept.ecndetailid
            new TranslationSeedItem("entity.ecdept.ecndetailid", "zh-CN", "设变明细ID", "设变明细ID（TaktEcDetail 主键）"),
            // entity.ecdept.ecndetailid
            new TranslationSeedItem("entity.ecdept.ecndetailid", "zh-HK", "设变明细ID_hk", "设变明细ID（TaktEcDetail 主键）"),

            // entity.ecdept.ecno
            new TranslationSeedItem("entity.ecdept.ecno", "en-US", "设变单号_us", "设变单号（冗余字段,便于查询）"),
            // entity.ecdept.ecno
            new TranslationSeedItem("entity.ecdept.ecno", "ja-JP", "设变单号_jp", "设变单号（冗余字段,便于查询）"),
            // entity.ecdept.ecno
            new TranslationSeedItem("entity.ecdept.ecno", "zh-CN", "设变单号", "设变单号（冗余字段,便于查询）"),
            // entity.ecdept.ecno
            new TranslationSeedItem("entity.ecdept.ecno", "zh-HK", "设变单号_hk", "设变单号（冗余字段,便于查询）"),

            // entity.ecdept.linenumber
            new TranslationSeedItem("entity.ecdept.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ecdept.linenumber
            new TranslationSeedItem("entity.ecdept.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ecdept.linenumber
            new TranslationSeedItem("entity.ecdept.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ecdept.linenumber
            new TranslationSeedItem("entity.ecdept.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ecdept.deptcode
            new TranslationSeedItem("entity.ecdept.deptcode", "en-US", "部门编码_us", "部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。"),
            // entity.ecdept.deptcode
            new TranslationSeedItem("entity.ecdept.deptcode", "ja-JP", "部门编码_jp", "部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。"),
            // entity.ecdept.deptcode
            new TranslationSeedItem("entity.ecdept.deptcode", "zh-CN", "部门编码", "部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。"),
            // entity.ecdept.deptcode
            new TranslationSeedItem("entity.ecdept.deptcode", "zh-HK", "部门编码_hk", "部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。"),

            // entity.ecdept.isimplemented
            new TranslationSeedItem("entity.ecdept.isimplemented", "en-US", "是否实施_us", "是否实施（0=否 1=是）"),
            // entity.ecdept.isimplemented
            new TranslationSeedItem("entity.ecdept.isimplemented", "ja-JP", "是否实施_jp", "是否实施（0=否 1=是）"),
            // entity.ecdept.isimplemented
            new TranslationSeedItem("entity.ecdept.isimplemented", "zh-CN", "是否实施", "是否实施（0=否 1=是）"),
            // entity.ecdept.isimplemented
            new TranslationSeedItem("entity.ecdept.isimplemented", "zh-HK", "是否实施_hk", "是否实施（0=否 1=是）"),

            // entity.ecdept.content
            new TranslationSeedItem("entity.ecdept.content", "en-US", "内容_us", "内容（各部门通用）"),
            // entity.ecdept.content
            new TranslationSeedItem("entity.ecdept.content", "ja-JP", "内容_jp", "内容（各部门通用）"),
            // entity.ecdept.content
            new TranslationSeedItem("entity.ecdept.content", "zh-CN", "内容", "内容（各部门通用）"),
            // entity.ecdept.content
            new TranslationSeedItem("entity.ecdept.content", "zh-HK", "内容_hk", "内容（各部门通用）"),

            // entity.ecdept.scheduledproductiondate
            new TranslationSeedItem("entity.ecdept.scheduledproductiondate", "en-US", "预计生产日期_us", "预计生产日期"),
            // entity.ecdept.scheduledproductiondate
            new TranslationSeedItem("entity.ecdept.scheduledproductiondate", "ja-JP", "预计生产日期_jp", "预计生产日期"),
            // entity.ecdept.scheduledproductiondate
            new TranslationSeedItem("entity.ecdept.scheduledproductiondate", "zh-CN", "预计生产日期", "预计生产日期"),
            // entity.ecdept.scheduledproductiondate
            new TranslationSeedItem("entity.ecdept.scheduledproductiondate", "zh-HK", "预计生产日期_hk", "预计生产日期"),

            // entity.ecdept.scheduledbatch
            new TranslationSeedItem("entity.ecdept.scheduledbatch", "en-US", "预定批次_us", "预定批次"),
            // entity.ecdept.scheduledbatch
            new TranslationSeedItem("entity.ecdept.scheduledbatch", "ja-JP", "预定批次_jp", "预定批次"),
            // entity.ecdept.scheduledbatch
            new TranslationSeedItem("entity.ecdept.scheduledbatch", "zh-CN", "预定批次", "预定批次"),
            // entity.ecdept.scheduledbatch
            new TranslationSeedItem("entity.ecdept.scheduledbatch", "zh-HK", "预定批次_hk", "预定批次"),

            // entity.ecdept.poremainder
            new TranslationSeedItem("entity.ecdept.poremainder", "en-US", "Po残_us", "Po残（采购订单残）"),
            // entity.ecdept.poremainder
            new TranslationSeedItem("entity.ecdept.poremainder", "ja-JP", "Po残_jp", "Po残（采购订单残）"),
            // entity.ecdept.poremainder
            new TranslationSeedItem("entity.ecdept.poremainder", "zh-CN", "Po残", "Po残（采购订单残）"),
            // entity.ecdept.poremainder
            new TranslationSeedItem("entity.ecdept.poremainder", "zh-HK", "Po残_hk", "Po残（采购订单残）"),

            // entity.ecdept.balance
            new TranslationSeedItem("entity.ecdept.balance", "en-US", "结余_us", "结余"),
            // entity.ecdept.balance
            new TranslationSeedItem("entity.ecdept.balance", "ja-JP", "结余_jp", "结余"),
            // entity.ecdept.balance
            new TranslationSeedItem("entity.ecdept.balance", "zh-CN", "结余", "结余"),
            // entity.ecdept.balance
            new TranslationSeedItem("entity.ecdept.balance", "zh-HK", "结余_hk", "结余"),

            // entity.ecdept.oldproducthandling
            new TranslationSeedItem("entity.ecdept.oldproducthandling", "en-US", "旧品处理_us", "旧品处理"),
            // entity.ecdept.oldproducthandling
            new TranslationSeedItem("entity.ecdept.oldproducthandling", "ja-JP", "旧品处理_jp", "旧品处理"),
            // entity.ecdept.oldproducthandling
            new TranslationSeedItem("entity.ecdept.oldproducthandling", "zh-CN", "旧品处理", "旧品处理"),
            // entity.ecdept.oldproducthandling
            new TranslationSeedItem("entity.ecdept.oldproducthandling", "zh-HK", "旧品处理_hk", "旧品处理"),

            // entity.ecdept.purchaseorderissuedate
            new TranslationSeedItem("entity.ecdept.purchaseorderissuedate", "en-US", "采购订单发行日期_us", "采购订单发行日期"),
            // entity.ecdept.purchaseorderissuedate
            new TranslationSeedItem("entity.ecdept.purchaseorderissuedate", "ja-JP", "采购订单发行日期_jp", "采购订单发行日期"),
            // entity.ecdept.purchaseorderissuedate
            new TranslationSeedItem("entity.ecdept.purchaseorderissuedate", "zh-CN", "采购订单发行日期", "采购订单发行日期"),
            // entity.ecdept.purchaseorderissuedate
            new TranslationSeedItem("entity.ecdept.purchaseorderissuedate", "zh-HK", "采购订单发行日期_hk", "采购订单发行日期"),

            // entity.ecdept.supplier
            new TranslationSeedItem("entity.ecdept.supplier", "en-US", "供应商_us", "供应商"),
            // entity.ecdept.supplier
            new TranslationSeedItem("entity.ecdept.supplier", "ja-JP", "供应商_jp", "供应商"),
            // entity.ecdept.supplier
            new TranslationSeedItem("entity.ecdept.supplier", "zh-CN", "供应商", "供应商"),
            // entity.ecdept.supplier
            new TranslationSeedItem("entity.ecdept.supplier", "zh-HK", "供应商_hk", "供应商"),

            // entity.ecdept.purchaseorderno
            new TranslationSeedItem("entity.ecdept.purchaseorderno", "en-US", "采购订单号码_us", "采购订单号码"),
            // entity.ecdept.purchaseorderno
            new TranslationSeedItem("entity.ecdept.purchaseorderno", "ja-JP", "采购订单号码_jp", "采购订单号码"),
            // entity.ecdept.purchaseorderno
            new TranslationSeedItem("entity.ecdept.purchaseorderno", "zh-CN", "采购订单号码", "采购订单号码"),
            // entity.ecdept.purchaseorderno
            new TranslationSeedItem("entity.ecdept.purchaseorderno", "zh-HK", "采购订单号码_hk", "采购订单号码"),

            // entity.ecdept.iqcorderno
            new TranslationSeedItem("entity.ecdept.iqcorderno", "en-US", "受检单号_us", "受检单号"),
            // entity.ecdept.iqcorderno
            new TranslationSeedItem("entity.ecdept.iqcorderno", "ja-JP", "受检单号_jp", "受检单号"),
            // entity.ecdept.iqcorderno
            new TranslationSeedItem("entity.ecdept.iqcorderno", "zh-CN", "受检单号", "受检单号"),
            // entity.ecdept.iqcorderno
            new TranslationSeedItem("entity.ecdept.iqcorderno", "zh-HK", "受检单号_hk", "受检单号"),

            // entity.ecdept.inspectiondate
            new TranslationSeedItem("entity.ecdept.inspectiondate", "en-US", "检验日期_us", "检验/检查日期"),
            // entity.ecdept.inspectiondate
            new TranslationSeedItem("entity.ecdept.inspectiondate", "ja-JP", "检验日期_jp", "检验/检查日期"),
            // entity.ecdept.inspectiondate
            new TranslationSeedItem("entity.ecdept.inspectiondate", "zh-CN", "检验日期", "检验/检查日期"),
            // entity.ecdept.inspectiondate
            new TranslationSeedItem("entity.ecdept.inspectiondate", "zh-HK", "检验日期_hk", "检验/检查日期"),

            // entity.ecdept.outboundbatch
            new TranslationSeedItem("entity.ecdept.outboundbatch", "en-US", "出库批次_us", "出库批次"),
            // entity.ecdept.outboundbatch
            new TranslationSeedItem("entity.ecdept.outboundbatch", "ja-JP", "出库批次_jp", "出库批次"),
            // entity.ecdept.outboundbatch
            new TranslationSeedItem("entity.ecdept.outboundbatch", "zh-CN", "出库批次", "出库批次"),
            // entity.ecdept.outboundbatch
            new TranslationSeedItem("entity.ecdept.outboundbatch", "zh-HK", "出库批次_hk", "出库批次"),

            // entity.ecdept.outbounddate
            new TranslationSeedItem("entity.ecdept.outbounddate", "en-US", "出库日期_us", "出库日期"),
            // entity.ecdept.outbounddate
            new TranslationSeedItem("entity.ecdept.outbounddate", "ja-JP", "出库日期_jp", "出库日期"),
            // entity.ecdept.outbounddate
            new TranslationSeedItem("entity.ecdept.outbounddate", "zh-CN", "出库日期", "出库日期"),
            // entity.ecdept.outbounddate
            new TranslationSeedItem("entity.ecdept.outbounddate", "zh-HK", "出库日期_hk", "出库日期"),

            // entity.ecdept.productiondate
            new TranslationSeedItem("entity.ecdept.productiondate", "en-US", "生产日期_us", "生产日期"),
            // entity.ecdept.productiondate
            new TranslationSeedItem("entity.ecdept.productiondate", "ja-JP", "生产日期_jp", "生产日期"),
            // entity.ecdept.productiondate
            new TranslationSeedItem("entity.ecdept.productiondate", "zh-CN", "生产日期", "生产日期"),
            // entity.ecdept.productiondate
            new TranslationSeedItem("entity.ecdept.productiondate", "zh-HK", "生产日期_hk", "生产日期"),

            // entity.ecdept.productionbatch
            new TranslationSeedItem("entity.ecdept.productionbatch", "en-US", "生产批次_us", "生产批次"),
            // entity.ecdept.productionbatch
            new TranslationSeedItem("entity.ecdept.productionbatch", "ja-JP", "生产批次_jp", "生产批次"),
            // entity.ecdept.productionbatch
            new TranslationSeedItem("entity.ecdept.productionbatch", "zh-CN", "生产批次", "生产批次"),
            // entity.ecdept.productionbatch
            new TranslationSeedItem("entity.ecdept.productionbatch", "zh-HK", "生产批次_hk", "生产批次"),

            // entity.ecdept.outboundorderno
            new TranslationSeedItem("entity.ecdept.outboundorderno", "en-US", "出库单号_us", "出库单号"),
            // entity.ecdept.outboundorderno
            new TranslationSeedItem("entity.ecdept.outboundorderno", "ja-JP", "出库单号_jp", "出库单号"),
            // entity.ecdept.outboundorderno
            new TranslationSeedItem("entity.ecdept.outboundorderno", "zh-CN", "出库单号", "出库单号"),
            // entity.ecdept.outboundorderno
            new TranslationSeedItem("entity.ecdept.outboundorderno", "zh-HK", "出库单号_hk", "出库单号"),

            // entity.ecdept.productionteam
            new TranslationSeedItem("entity.ecdept.productionteam", "en-US", "生产班组_us", "生产班组"),
            // entity.ecdept.productionteam
            new TranslationSeedItem("entity.ecdept.productionteam", "ja-JP", "生产班组_jp", "生产班组"),
            // entity.ecdept.productionteam
            new TranslationSeedItem("entity.ecdept.productionteam", "zh-CN", "生产班组", "生产班组"),
            // entity.ecdept.productionteam
            new TranslationSeedItem("entity.ecdept.productionteam", "zh-HK", "生产班组_hk", "生产班组"),

            // entity.ecdept.implementationdate
            new TranslationSeedItem("entity.ecdept.implementationdate", "en-US", "实施日期_us", "实施日期"),
            // entity.ecdept.implementationdate
            new TranslationSeedItem("entity.ecdept.implementationdate", "ja-JP", "实施日期_jp", "实施日期"),
            // entity.ecdept.implementationdate
            new TranslationSeedItem("entity.ecdept.implementationdate", "zh-CN", "实施日期", "实施日期"),
            // entity.ecdept.implementationdate
            new TranslationSeedItem("entity.ecdept.implementationdate", "zh-HK", "实施日期_hk", "实施日期"),

            // entity.ecdept.inspectionbatch
            new TranslationSeedItem("entity.ecdept.inspectionbatch", "en-US", "检验批次_us", "检验批次"),
            // entity.ecdept.inspectionbatch
            new TranslationSeedItem("entity.ecdept.inspectionbatch", "ja-JP", "检验批次_jp", "检验批次"),
            // entity.ecdept.inspectionbatch
            new TranslationSeedItem("entity.ecdept.inspectionbatch", "zh-CN", "检验批次", "检验批次"),
            // entity.ecdept.inspectionbatch
            new TranslationSeedItem("entity.ecdept.inspectionbatch", "zh-HK", "检验批次_hk", "检验批次"),

            // entity.ecdept.samplingno
            new TranslationSeedItem("entity.ecdept.samplingno", "en-US", "抽样号码_us", "抽样号码"),
            // entity.ecdept.samplingno
            new TranslationSeedItem("entity.ecdept.samplingno", "ja-JP", "抽样号码_jp", "抽样号码"),
            // entity.ecdept.samplingno
            new TranslationSeedItem("entity.ecdept.samplingno", "zh-CN", "抽样号码", "抽样号码"),
            // entity.ecdept.samplingno
            new TranslationSeedItem("entity.ecdept.samplingno", "zh-HK", "抽样号码_hk", "抽样号码"),

            // entity.ecdept.issopupdated
            new TranslationSeedItem("entity.ecdept.issopupdated", "en-US", "是否更新SOP_us", "是否更新SOP（0=否 1=是）"),
            // entity.ecdept.issopupdated
            new TranslationSeedItem("entity.ecdept.issopupdated", "ja-JP", "是否更新SOP_jp", "是否更新SOP（0=否 1=是）"),
            // entity.ecdept.issopupdated
            new TranslationSeedItem("entity.ecdept.issopupdated", "zh-CN", "是否更新SOP", "是否更新SOP（0=否 1=是）"),
            // entity.ecdept.issopupdated
            new TranslationSeedItem("entity.ecdept.issopupdated", "zh-HK", "是否更新SOP_hk", "是否更新SOP（0=否 1=是）"),

            // entity.ecdept.ecndetail
            new TranslationSeedItem("entity.ecdept.ecndetail", "en-US", "设变明细_us", "设变明细（多对一）"),
            // entity.ecdept.ecndetail
            new TranslationSeedItem("entity.ecdept.ecndetail", "ja-JP", "设变明细_jp", "设变明细（多对一）"),
            // entity.ecdept.ecndetail
            new TranslationSeedItem("entity.ecdept.ecndetail", "zh-CN", "设变明细", "设变明细（多对一）"),
            // entity.ecdept.ecndetail
            new TranslationSeedItem("entity.ecdept.ecndetail", "zh-HK", "设变明细_hk", "设变明细（多对一）"),
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
