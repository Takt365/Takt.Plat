// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktCustomerComplaint 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint;

/// <summary>
/// TaktCustomerComplaint 实体国际化翻译种子（键前缀 entity.customerComplaint.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktCustomerComplaintI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktCustomerComplaint 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customerComplaint 实体翻译...", tenantCode);

        foreach (var item in GetCustomerComplaintTranslations())
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

        TaktLogger.Information("TaktCustomerComplaint 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktCustomerComplaint 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.customerComplaint._self / entity.customerComplaint.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerComplaintTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customerComplaint._self
            new TranslationSeedItem("entity.customerComplaint._self", "en-US", "Customer Complaint Information", "实体名称"),
            // entity.customerComplaint._self
            new TranslationSeedItem("entity.customerComplaint._self", "ja-JP", "客诉主表信息", "实体名称"),
            // entity.customerComplaint._self
            new TranslationSeedItem("entity.customerComplaint._self", "zh-CN", "客诉主表信息", "实体名称"),
            // entity.customerComplaint._self
            new TranslationSeedItem("entity.customerComplaint._self", "zh-HK", "客诉主表信息", "实体名称"),

            // entity.customerComplaint.code
            new TranslationSeedItem("entity.customerComplaint.code", "en-US", "客诉单号", "客诉单号（组合唯一索引）"),
            // entity.customerComplaint.code
            new TranslationSeedItem("entity.customerComplaint.code", "ja-JP", "客诉单号", "客诉单号（组合唯一索引）"),
            // entity.customerComplaint.code
            new TranslationSeedItem("entity.customerComplaint.code", "zh-CN", "客诉单号", "客诉单号（组合唯一索引）"),
            // entity.customerComplaint.code
            new TranslationSeedItem("entity.customerComplaint.code", "zh-HK", "客诉单号", "客诉单号（组合唯一索引）"),

            // entity.customerComplaint.customerid
            new TranslationSeedItem("entity.customerComplaint.customerid", "en-US", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaint.customerid
            new TranslationSeedItem("entity.customerComplaint.customerid", "ja-JP", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaint.customerid
            new TranslationSeedItem("entity.customerComplaint.customerid", "zh-CN", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaint.customerid
            new TranslationSeedItem("entity.customerComplaint.customerid", "zh-HK", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerComplaint.customername
            new TranslationSeedItem("entity.customerComplaint.customername", "en-US", "客户名称", "客户名称"),
            // entity.customerComplaint.customername
            new TranslationSeedItem("entity.customerComplaint.customername", "ja-JP", "客户名称", "客户名称"),
            // entity.customerComplaint.customername
            new TranslationSeedItem("entity.customerComplaint.customername", "zh-CN", "客户名称", "客户名称"),
            // entity.customerComplaint.customername
            new TranslationSeedItem("entity.customerComplaint.customername", "zh-HK", "客户名称", "客户名称"),

            // entity.customerComplaint.customercode
            new TranslationSeedItem("entity.customerComplaint.customercode", "en-US", "客户编码", "客户编码"),
            // entity.customerComplaint.customercode
            new TranslationSeedItem("entity.customerComplaint.customercode", "ja-JP", "客户编码", "客户编码"),
            // entity.customerComplaint.customercode
            new TranslationSeedItem("entity.customerComplaint.customercode", "zh-CN", "客户编码", "客户编码"),
            // entity.customerComplaint.customercode
            new TranslationSeedItem("entity.customerComplaint.customercode", "zh-HK", "客户编码", "客户编码"),

            // entity.customerComplaint.complaintdate
            new TranslationSeedItem("entity.customerComplaint.complaintdate", "en-US", "投诉日期", "投诉日期"),
            // entity.customerComplaint.complaintdate
            new TranslationSeedItem("entity.customerComplaint.complaintdate", "ja-JP", "投诉日期", "投诉日期"),
            // entity.customerComplaint.complaintdate
            new TranslationSeedItem("entity.customerComplaint.complaintdate", "zh-CN", "投诉日期", "投诉日期"),
            // entity.customerComplaint.complaintdate
            new TranslationSeedItem("entity.customerComplaint.complaintdate", "zh-HK", "投诉日期", "投诉日期"),

            // entity.customerComplaint.complaintmethod
            new TranslationSeedItem("entity.customerComplaint.complaintmethod", "en-US", "投诉方式", "投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）"),
            // entity.customerComplaint.complaintmethod
            new TranslationSeedItem("entity.customerComplaint.complaintmethod", "ja-JP", "投诉方式", "投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）"),
            // entity.customerComplaint.complaintmethod
            new TranslationSeedItem("entity.customerComplaint.complaintmethod", "zh-CN", "投诉方式", "投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）"),
            // entity.customerComplaint.complaintmethod
            new TranslationSeedItem("entity.customerComplaint.complaintmethod", "zh-HK", "投诉方式", "投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）"),

            // entity.customerComplaint.complainttype
            new TranslationSeedItem("entity.customerComplaint.complainttype", "en-US", "投诉类型", "投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）"),
            // entity.customerComplaint.complainttype
            new TranslationSeedItem("entity.customerComplaint.complainttype", "ja-JP", "投诉类型", "投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）"),
            // entity.customerComplaint.complainttype
            new TranslationSeedItem("entity.customerComplaint.complainttype", "zh-CN", "投诉类型", "投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）"),
            // entity.customerComplaint.complainttype
            new TranslationSeedItem("entity.customerComplaint.complainttype", "zh-HK", "投诉类型", "投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）"),

            // entity.customerComplaint.complaintlevel
            new TranslationSeedItem("entity.customerComplaint.complaintlevel", "en-US", "投诉等级", "投诉等级（0=一般，1=重要，2=紧急，3=严重）"),
            // entity.customerComplaint.complaintlevel
            new TranslationSeedItem("entity.customerComplaint.complaintlevel", "ja-JP", "投诉等级", "投诉等级（0=一般，1=重要，2=紧急，3=严重）"),
            // entity.customerComplaint.complaintlevel
            new TranslationSeedItem("entity.customerComplaint.complaintlevel", "zh-CN", "投诉等级", "投诉等级（0=一般，1=重要，2=紧急，3=严重）"),
            // entity.customerComplaint.complaintlevel
            new TranslationSeedItem("entity.customerComplaint.complaintlevel", "zh-HK", "投诉等级", "投诉等级（0=一般，1=重要，2=紧急，3=严重）"),

            // entity.customerComplaint.responsibledeptid
            new TranslationSeedItem("entity.customerComplaint.responsibledeptid", "en-US", "责任部门ID", "责任部门ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaint.responsibledeptid
            new TranslationSeedItem("entity.customerComplaint.responsibledeptid", "ja-JP", "责任部门ID", "责任部门ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaint.responsibledeptid
            new TranslationSeedItem("entity.customerComplaint.responsibledeptid", "zh-CN", "责任部门ID", "责任部门ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaint.responsibledeptid
            new TranslationSeedItem("entity.customerComplaint.responsibledeptid", "zh-HK", "责任部门ID", "责任部门ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerComplaint.responsibledeptname
            new TranslationSeedItem("entity.customerComplaint.responsibledeptname", "en-US", "责任部门名称", "责任部门名称"),
            // entity.customerComplaint.responsibledeptname
            new TranslationSeedItem("entity.customerComplaint.responsibledeptname", "ja-JP", "责任部门名称", "责任部门名称"),
            // entity.customerComplaint.responsibledeptname
            new TranslationSeedItem("entity.customerComplaint.responsibledeptname", "zh-CN", "责任部门名称", "责任部门名称"),
            // entity.customerComplaint.responsibledeptname
            new TranslationSeedItem("entity.customerComplaint.responsibledeptname", "zh-HK", "责任部门名称", "责任部门名称"),

            // entity.customerComplaint.responsiblepersonid
            new TranslationSeedItem("entity.customerComplaint.responsiblepersonid", "en-US", "责任人ID", "责任人ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaint.responsiblepersonid
            new TranslationSeedItem("entity.customerComplaint.responsiblepersonid", "ja-JP", "责任人ID", "责任人ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaint.responsiblepersonid
            new TranslationSeedItem("entity.customerComplaint.responsiblepersonid", "zh-CN", "责任人ID", "责任人ID（序列化为string以避免Javascript精度问题）"),
            // entity.customerComplaint.responsiblepersonid
            new TranslationSeedItem("entity.customerComplaint.responsiblepersonid", "zh-HK", "责任人ID", "责任人ID（序列化为string以避免Javascript精度问题）"),

            // entity.customerComplaint.responsiblepersonname
            new TranslationSeedItem("entity.customerComplaint.responsiblepersonname", "en-US", "责任人姓名", "责任人姓名"),
            // entity.customerComplaint.responsiblepersonname
            new TranslationSeedItem("entity.customerComplaint.responsiblepersonname", "ja-JP", "责任人姓名", "责任人姓名"),
            // entity.customerComplaint.responsiblepersonname
            new TranslationSeedItem("entity.customerComplaint.responsiblepersonname", "zh-CN", "责任人姓名", "责任人姓名"),
            // entity.customerComplaint.responsiblepersonname
            new TranslationSeedItem("entity.customerComplaint.responsiblepersonname", "zh-HK", "责任人姓名", "责任人姓名"),

            // entity.customerComplaint.requiredreplydate
            new TranslationSeedItem("entity.customerComplaint.requiredreplydate", "en-US", "要求回复日期", "要求回复日期"),
            // entity.customerComplaint.requiredreplydate
            new TranslationSeedItem("entity.customerComplaint.requiredreplydate", "ja-JP", "要求回复日期", "要求回复日期"),
            // entity.customerComplaint.requiredreplydate
            new TranslationSeedItem("entity.customerComplaint.requiredreplydate", "zh-CN", "要求回复日期", "要求回复日期"),
            // entity.customerComplaint.requiredreplydate
            new TranslationSeedItem("entity.customerComplaint.requiredreplydate", "zh-HK", "要求回复日期", "要求回复日期"),

            // entity.customerComplaint.actualreplydate
            new TranslationSeedItem("entity.customerComplaint.actualreplydate", "en-US", "实际回复日期", "实际回复日期"),
            // entity.customerComplaint.actualreplydate
            new TranslationSeedItem("entity.customerComplaint.actualreplydate", "ja-JP", "实际回复日期", "实际回复日期"),
            // entity.customerComplaint.actualreplydate
            new TranslationSeedItem("entity.customerComplaint.actualreplydate", "zh-CN", "实际回复日期", "实际回复日期"),
            // entity.customerComplaint.actualreplydate
            new TranslationSeedItem("entity.customerComplaint.actualreplydate", "zh-HK", "实际回复日期", "实际回复日期"),

            // entity.customerComplaint.complaintstatus
            new TranslationSeedItem("entity.customerComplaint.complaintstatus", "en-US", "客诉状态", "客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）"),
            // entity.customerComplaint.complaintstatus
            new TranslationSeedItem("entity.customerComplaint.complaintstatus", "ja-JP", "客诉状态", "客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）"),
            // entity.customerComplaint.complaintstatus
            new TranslationSeedItem("entity.customerComplaint.complaintstatus", "zh-CN", "客诉状态", "客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）"),
            // entity.customerComplaint.complaintstatus
            new TranslationSeedItem("entity.customerComplaint.complaintstatus", "zh-HK", "客诉状态", "客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）"),

            // entity.customerComplaint.complaintdescription
            new TranslationSeedItem("entity.customerComplaint.complaintdescription", "en-US", "客诉描述", "客诉描述"),
            // entity.customerComplaint.complaintdescription
            new TranslationSeedItem("entity.customerComplaint.complaintdescription", "ja-JP", "客诉描述", "客诉描述"),
            // entity.customerComplaint.complaintdescription
            new TranslationSeedItem("entity.customerComplaint.complaintdescription", "zh-CN", "客诉描述", "客诉描述"),
            // entity.customerComplaint.complaintdescription
            new TranslationSeedItem("entity.customerComplaint.complaintdescription", "zh-HK", "客诉描述", "客诉描述"),

            // entity.customerComplaint.handlingresult
            new TranslationSeedItem("entity.customerComplaint.handlingresult", "en-US", "处理结果", "处理结果/回复内容"),
            // entity.customerComplaint.handlingresult
            new TranslationSeedItem("entity.customerComplaint.handlingresult", "ja-JP", "处理结果", "处理结果/回复内容"),
            // entity.customerComplaint.handlingresult
            new TranslationSeedItem("entity.customerComplaint.handlingresult", "zh-CN", "处理结果", "处理结果/回复内容"),
            // entity.customerComplaint.handlingresult
            new TranslationSeedItem("entity.customerComplaint.handlingresult", "zh-HK", "处理结果", "处理结果/回复内容"),

            // entity.customerComplaint.customersatisfaction
            new TranslationSeedItem("entity.customerComplaint.customersatisfaction", "en-US", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),
            // entity.customerComplaint.customersatisfaction
            new TranslationSeedItem("entity.customerComplaint.customersatisfaction", "ja-JP", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),
            // entity.customerComplaint.customersatisfaction
            new TranslationSeedItem("entity.customerComplaint.customersatisfaction", "zh-CN", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),
            // entity.customerComplaint.customersatisfaction
            new TranslationSeedItem("entity.customerComplaint.customersatisfaction", "zh-HK", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),

            // entity.customerComplaint.relatedplant
            new TranslationSeedItem("entity.customerComplaint.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.customerComplaint.relatedplant
            new TranslationSeedItem("entity.customerComplaint.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.customerComplaint.relatedplant
            new TranslationSeedItem("entity.customerComplaint.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.customerComplaint.relatedplant
            new TranslationSeedItem("entity.customerComplaint.relatedplant", "zh-HK", "关联工厂", "关联工厂"),

            // entity.customerComplaint.sortorder
            new TranslationSeedItem("entity.customerComplaint.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.customerComplaint.sortorder
            new TranslationSeedItem("entity.customerComplaint.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.customerComplaint.sortorder
            new TranslationSeedItem("entity.customerComplaint.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.customerComplaint.sortorder
            new TranslationSeedItem("entity.customerComplaint.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.customerComplaint.items
            new TranslationSeedItem("entity.customerComplaint.items", "en-US", "items", "客诉明细列表（主子表关系）"),
            // entity.customerComplaint.items
            new TranslationSeedItem("entity.customerComplaint.items", "ja-JP", "items", "客诉明细列表（主子表关系）"),
            // entity.customerComplaint.items
            new TranslationSeedItem("entity.customerComplaint.items", "zh-CN", "items", "客诉明细列表（主子表关系）"),
            // entity.customerComplaint.items
            new TranslationSeedItem("entity.customerComplaint.items", "zh-HK", "items", "客诉明细列表（主子表关系）"),
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
