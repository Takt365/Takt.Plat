// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Planning
// 文件名称：TaktSalesPlanI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktSalesPlan 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Planning;

/// <summary>
/// TaktSalesPlan 实体国际化翻译种子（键前缀 entity.salesplan.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktSalesPlanI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktSalesPlan 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 salesplan 实体翻译...", tenantCode);

        foreach (var item in GetSalesPlanTranslations())
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

        TaktLogger.Information("TaktSalesPlan 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktSalesPlan 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.salesplan._self / entity.salesplan.{{field}}；ResourceGroup=Planning；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetSalesPlanTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.salesplan._self
            new TranslationSeedItem("entity.salesplan._self", "en-US", "Sales Plan Information_us", "实体名称"),
            // entity.salesplan._self
            new TranslationSeedItem("entity.salesplan._self", "ja-JP", "Takt销售计划信息_jp", "实体名称"),
            // entity.salesplan._self
            new TranslationSeedItem("entity.salesplan._self", "zh-CN", "Takt销售计划信息", "实体名称"),
            // entity.salesplan._self
            new TranslationSeedItem("entity.salesplan._self", "zh-HK", "Takt销售计划信息_hk", "实体名称"),

            // entity.salesplan.plantcode
            new TranslationSeedItem("entity.salesplan.plantcode", "en-US", "工厂代码_us", "工厂代码（关联 TaktPlant.PlantCode）"),
            // entity.salesplan.plantcode
            new TranslationSeedItem("entity.salesplan.plantcode", "ja-JP", "工厂代码_jp", "工厂代码（关联 TaktPlant.PlantCode）"),
            // entity.salesplan.plantcode
            new TranslationSeedItem("entity.salesplan.plantcode", "zh-CN", "工厂代码", "工厂代码（关联 TaktPlant.PlantCode）"),
            // entity.salesplan.plantcode
            new TranslationSeedItem("entity.salesplan.plantcode", "zh-HK", "工厂代码_hk", "工厂代码（关联 TaktPlant.PlantCode）"),

            // entity.salesplan.code
            new TranslationSeedItem("entity.salesplan.code", "en-US", "销售计划编码_us", "销售计划编码（租户+公司+工厂内业务唯一）"),
            // entity.salesplan.code
            new TranslationSeedItem("entity.salesplan.code", "ja-JP", "销售计划编码_jp", "销售计划编码（租户+公司+工厂内业务唯一）"),
            // entity.salesplan.code
            new TranslationSeedItem("entity.salesplan.code", "zh-CN", "销售计划编码", "销售计划编码（租户+公司+工厂内业务唯一）"),
            // entity.salesplan.code
            new TranslationSeedItem("entity.salesplan.code", "zh-HK", "销售计划编码_hk", "销售计划编码（租户+公司+工厂内业务唯一）"),

            // entity.salesplan.plandate
            new TranslationSeedItem("entity.salesplan.plandate", "en-US", "计划编制日期_us", "计划编制日期"),
            // entity.salesplan.plandate
            new TranslationSeedItem("entity.salesplan.plandate", "ja-JP", "计划编制日期_jp", "计划编制日期"),
            // entity.salesplan.plandate
            new TranslationSeedItem("entity.salesplan.plandate", "zh-CN", "计划编制日期", "计划编制日期"),
            // entity.salesplan.plandate
            new TranslationSeedItem("entity.salesplan.plandate", "zh-HK", "计划编制日期_hk", "计划编制日期"),

            // entity.salesplan.planperiodstart
            new TranslationSeedItem("entity.salesplan.planperiodstart", "en-US", "计划周期开始日期_us", "计划周期开始日期"),
            // entity.salesplan.planperiodstart
            new TranslationSeedItem("entity.salesplan.planperiodstart", "ja-JP", "计划周期开始日期_jp", "计划周期开始日期"),
            // entity.salesplan.planperiodstart
            new TranslationSeedItem("entity.salesplan.planperiodstart", "zh-CN", "计划周期开始日期", "计划周期开始日期"),
            // entity.salesplan.planperiodstart
            new TranslationSeedItem("entity.salesplan.planperiodstart", "zh-HK", "计划周期开始日期_hk", "计划周期开始日期"),

            // entity.salesplan.planperiodend
            new TranslationSeedItem("entity.salesplan.planperiodend", "en-US", "计划周期结束日期_us", "计划周期结束日期"),
            // entity.salesplan.planperiodend
            new TranslationSeedItem("entity.salesplan.planperiodend", "ja-JP", "计划周期结束日期_jp", "计划周期结束日期"),
            // entity.salesplan.planperiodend
            new TranslationSeedItem("entity.salesplan.planperiodend", "zh-CN", "计划周期结束日期", "计划周期结束日期"),
            // entity.salesplan.planperiodend
            new TranslationSeedItem("entity.salesplan.planperiodend", "zh-HK", "计划周期结束日期_hk", "计划周期结束日期"),

            // entity.salesplan.customercode
            new TranslationSeedItem("entity.salesplan.customercode", "en-US", "客户编码_us", "客户编码（可选；汇总计划时为空，关联 TaktCustomer.CustomerCode）"),
            // entity.salesplan.customercode
            new TranslationSeedItem("entity.salesplan.customercode", "ja-JP", "客户编码_jp", "客户编码（可选；汇总计划时为空，关联 TaktCustomer.CustomerCode）"),
            // entity.salesplan.customercode
            new TranslationSeedItem("entity.salesplan.customercode", "zh-CN", "客户编码", "客户编码（可选；汇总计划时为空，关联 TaktCustomer.CustomerCode）"),
            // entity.salesplan.customercode
            new TranslationSeedItem("entity.salesplan.customercode", "zh-HK", "客户编码_hk", "客户编码（可选；汇总计划时为空，关联 TaktCustomer.CustomerCode）"),

            // entity.salesplan.customername
            new TranslationSeedItem("entity.salesplan.customername", "en-US", "客户名称_us", "客户名称（冗余字段，便于查询展示）"),
            // entity.salesplan.customername
            new TranslationSeedItem("entity.salesplan.customername", "ja-JP", "客户名称_jp", "客户名称（冗余字段，便于查询展示）"),
            // entity.salesplan.customername
            new TranslationSeedItem("entity.salesplan.customername", "zh-CN", "客户名称", "客户名称（冗余字段，便于查询展示）"),
            // entity.salesplan.customername
            new TranslationSeedItem("entity.salesplan.customername", "zh-HK", "客户名称_hk", "客户名称（冗余字段，便于查询展示）"),

            // entity.salesplan.plannerid
            new TranslationSeedItem("entity.salesplan.plannerid", "en-US", "计划人员工ID_us", "计划人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesplan.plannerid
            new TranslationSeedItem("entity.salesplan.plannerid", "ja-JP", "计划人员工ID_jp", "计划人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesplan.plannerid
            new TranslationSeedItem("entity.salesplan.plannerid", "zh-CN", "计划人员工ID", "计划人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),
            // entity.salesplan.plannerid
            new TranslationSeedItem("entity.salesplan.plannerid", "zh-HK", "计划人员工ID_hk", "计划人员工ID（关联 TaktEmployee，序列化为 string 以避免 Javascript 精度问题）"),

            // entity.salesplan.planby
            new TranslationSeedItem("entity.salesplan.planby", "en-US", "计划人_us", "计划人（人员代码）"),
            // entity.salesplan.planby
            new TranslationSeedItem("entity.salesplan.planby", "ja-JP", "计划人_jp", "计划人（人员代码）"),
            // entity.salesplan.planby
            new TranslationSeedItem("entity.salesplan.planby", "zh-CN", "计划人", "计划人（人员代码）"),
            // entity.salesplan.planby
            new TranslationSeedItem("entity.salesplan.planby", "zh-HK", "计划人_hk", "计划人（人员代码）"),

            // entity.salesplan.totalquantity
            new TranslationSeedItem("entity.salesplan.totalquantity", "en-US", "计划总数量_us", "计划总数量（基本单位数量）"),
            // entity.salesplan.totalquantity
            new TranslationSeedItem("entity.salesplan.totalquantity", "ja-JP", "计划总数量_jp", "计划总数量（基本单位数量）"),
            // entity.salesplan.totalquantity
            new TranslationSeedItem("entity.salesplan.totalquantity", "zh-CN", "计划总数量", "计划总数量（基本单位数量）"),
            // entity.salesplan.totalquantity
            new TranslationSeedItem("entity.salesplan.totalquantity", "zh-HK", "计划总数量_hk", "计划总数量（基本单位数量）"),

            // entity.salesplan.totalamount
            new TranslationSeedItem("entity.salesplan.totalamount", "en-US", "计划总金额_us", "计划总金额"),
            // entity.salesplan.totalamount
            new TranslationSeedItem("entity.salesplan.totalamount", "ja-JP", "计划总金额_jp", "计划总金额"),
            // entity.salesplan.totalamount
            new TranslationSeedItem("entity.salesplan.totalamount", "zh-CN", "计划总金额", "计划总金额"),
            // entity.salesplan.totalamount
            new TranslationSeedItem("entity.salesplan.totalamount", "zh-HK", "计划总金额_hk", "计划总金额"),

            // entity.salesplan.convertedquantity
            new TranslationSeedItem("entity.salesplan.convertedquantity", "en-US", "已转生产销售数量_us", "已转生产/销售数量（基本单位数量）"),
            // entity.salesplan.convertedquantity
            new TranslationSeedItem("entity.salesplan.convertedquantity", "ja-JP", "已转生产销售数量_jp", "已转生产/销售数量（基本单位数量）"),
            // entity.salesplan.convertedquantity
            new TranslationSeedItem("entity.salesplan.convertedquantity", "zh-CN", "已转生产销售数量", "已转生产/销售数量（基本单位数量）"),
            // entity.salesplan.convertedquantity
            new TranslationSeedItem("entity.salesplan.convertedquantity", "zh-HK", "已转生产销售数量_hk", "已转生产/销售数量（基本单位数量）"),

            // entity.salesplan.convertedamount
            new TranslationSeedItem("entity.salesplan.convertedamount", "en-US", "已转生产销售金额_us", "已转生产/销售金额"),
            // entity.salesplan.convertedamount
            new TranslationSeedItem("entity.salesplan.convertedamount", "ja-JP", "已转生产销售金额_jp", "已转生产/销售金额"),
            // entity.salesplan.convertedamount
            new TranslationSeedItem("entity.salesplan.convertedamount", "zh-CN", "已转生产销售金额", "已转生产/销售金额"),
            // entity.salesplan.convertedamount
            new TranslationSeedItem("entity.salesplan.convertedamount", "zh-HK", "已转生产销售金额_hk", "已转生产/销售金额"),

            // entity.salesplan.planstatus
            new TranslationSeedItem("entity.salesplan.planstatus", "en-US", "计划状态_us", "计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),
            // entity.salesplan.planstatus
            new TranslationSeedItem("entity.salesplan.planstatus", "ja-JP", "计划状态_jp", "计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),
            // entity.salesplan.planstatus
            new TranslationSeedItem("entity.salesplan.planstatus", "zh-CN", "计划状态", "计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),
            // entity.salesplan.planstatus
            new TranslationSeedItem("entity.salesplan.planstatus", "zh-HK", "计划状态_hk", "计划状态（字典 sys_normal_disable_status；1=启用，0=禁用）"),

            // entity.salesplan.convertedstatus
            new TranslationSeedItem("entity.salesplan.convertedstatus", "en-US", "转单状态_us", "转单状态（0=未转单，1=部分转单，2=全部转单）"),
            // entity.salesplan.convertedstatus
            new TranslationSeedItem("entity.salesplan.convertedstatus", "ja-JP", "转单状态_jp", "转单状态（0=未转单，1=部分转单，2=全部转单）"),
            // entity.salesplan.convertedstatus
            new TranslationSeedItem("entity.salesplan.convertedstatus", "zh-CN", "转单状态", "转单状态（0=未转单，1=部分转单，2=全部转单）"),
            // entity.salesplan.convertedstatus
            new TranslationSeedItem("entity.salesplan.convertedstatus", "zh-HK", "转单状态_hk", "转单状态（0=未转单，1=部分转单，2=全部转单）"),

            // entity.salesplan.plandescription
            new TranslationSeedItem("entity.salesplan.plandescription", "en-US", "计划说明_us", "计划说明"),
            // entity.salesplan.plandescription
            new TranslationSeedItem("entity.salesplan.plandescription", "ja-JP", "计划说明_jp", "计划说明"),
            // entity.salesplan.plandescription
            new TranslationSeedItem("entity.salesplan.plandescription", "zh-CN", "计划说明", "计划说明"),
            // entity.salesplan.plandescription
            new TranslationSeedItem("entity.salesplan.plandescription", "zh-HK", "计划说明_hk", "计划说明"),

            // entity.salesplan.items
            new TranslationSeedItem("entity.salesplan.items", "en-US", "销售计划明细列表_us", "销售计划明细列表（主子表关系）"),
            // entity.salesplan.items
            new TranslationSeedItem("entity.salesplan.items", "ja-JP", "销售计划明细列表_jp", "销售计划明细列表（主子表关系）"),
            // entity.salesplan.items
            new TranslationSeedItem("entity.salesplan.items", "zh-CN", "销售计划明细列表", "销售计划明细列表（主子表关系）"),
            // entity.salesplan.items
            new TranslationSeedItem("entity.salesplan.items", "zh-HK", "销售计划明细列表_hk", "销售计划明细列表（主子表关系）"),
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
        translation.ResourceGroup = "Planning";
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
