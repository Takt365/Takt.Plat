// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintI18nSeedData.cs
// 创建时间：2026-06-12
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
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Complaint;

/// <summary>
/// TaktCustomerComplaint 实体国际化翻译种子（键前缀 entity.customercomplaint.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 customercomplaint 实体翻译...", tenantCode);

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
    /// I18nKey：entity.customercomplaint._self / entity.customercomplaint.{{field}}；ResourceGroup=4；ResourceType=0
    /// </summary>
    private static List<TranslationSeedItem> GetCustomerComplaintTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.customercomplaint._self
            new TranslationSeedItem("entity.customercomplaint._self", "en-US", "Customer Complaint Information", "实体名称"),
            // entity.customercomplaint._self
            new TranslationSeedItem("entity.customercomplaint._self", "ja-JP", "客诉主表信息", "实体名称"),
            // entity.customercomplaint._self
            new TranslationSeedItem("entity.customercomplaint._self", "zh-CN", "客诉主表信息", "实体名称"),
            // entity.customercomplaint._self
            new TranslationSeedItem("entity.customercomplaint._self", "zh-HK", "客诉主表信息", "实体名称"),

            // entity.customercomplaint.code
            new TranslationSeedItem("entity.customercomplaint.code", "en-US", "客诉单号", "客诉单号（组合唯一索引）"),
            // entity.customercomplaint.code
            new TranslationSeedItem("entity.customercomplaint.code", "ja-JP", "客诉单号", "客诉单号（组合唯一索引）"),
            // entity.customercomplaint.code
            new TranslationSeedItem("entity.customercomplaint.code", "zh-CN", "客诉单号", "客诉单号（组合唯一索引）"),
            // entity.customercomplaint.code
            new TranslationSeedItem("entity.customercomplaint.code", "zh-HK", "客诉单号", "客诉单号（组合唯一索引）"),

            // entity.customercomplaint.customerid
            new TranslationSeedItem("entity.customercomplaint.customerid", "en-US", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),
            // entity.customercomplaint.customerid
            new TranslationSeedItem("entity.customercomplaint.customerid", "ja-JP", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),
            // entity.customercomplaint.customerid
            new TranslationSeedItem("entity.customercomplaint.customerid", "zh-CN", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),
            // entity.customercomplaint.customerid
            new TranslationSeedItem("entity.customercomplaint.customerid", "zh-HK", "客户ID", "客户ID（序列化为string以避免Javascript精度问题）"),

            // entity.customercomplaint.customername
            new TranslationSeedItem("entity.customercomplaint.customername", "en-US", "客户名称", "客户名称"),
            // entity.customercomplaint.customername
            new TranslationSeedItem("entity.customercomplaint.customername", "ja-JP", "客户名称", "客户名称"),
            // entity.customercomplaint.customername
            new TranslationSeedItem("entity.customercomplaint.customername", "zh-CN", "客户名称", "客户名称"),
            // entity.customercomplaint.customername
            new TranslationSeedItem("entity.customercomplaint.customername", "zh-HK", "客户名称", "客户名称"),

            // entity.customercomplaint.customercode
            new TranslationSeedItem("entity.customercomplaint.customercode", "en-US", "客户编码", "客户编码"),
            // entity.customercomplaint.customercode
            new TranslationSeedItem("entity.customercomplaint.customercode", "ja-JP", "客户编码", "客户编码"),
            // entity.customercomplaint.customercode
            new TranslationSeedItem("entity.customercomplaint.customercode", "zh-CN", "客户编码", "客户编码"),
            // entity.customercomplaint.customercode
            new TranslationSeedItem("entity.customercomplaint.customercode", "zh-HK", "客户编码", "客户编码"),

            // entity.customercomplaint.complaintdate
            new TranslationSeedItem("entity.customercomplaint.complaintdate", "en-US", "投诉日期", "投诉日期"),
            // entity.customercomplaint.complaintdate
            new TranslationSeedItem("entity.customercomplaint.complaintdate", "ja-JP", "投诉日期", "投诉日期"),
            // entity.customercomplaint.complaintdate
            new TranslationSeedItem("entity.customercomplaint.complaintdate", "zh-CN", "投诉日期", "投诉日期"),
            // entity.customercomplaint.complaintdate
            new TranslationSeedItem("entity.customercomplaint.complaintdate", "zh-HK", "投诉日期", "投诉日期"),

            // entity.customercomplaint.complaintmethod
            new TranslationSeedItem("entity.customercomplaint.complaintmethod", "en-US", "投诉方式", "投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）"),
            // entity.customercomplaint.complaintmethod
            new TranslationSeedItem("entity.customercomplaint.complaintmethod", "ja-JP", "投诉方式", "投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）"),
            // entity.customercomplaint.complaintmethod
            new TranslationSeedItem("entity.customercomplaint.complaintmethod", "zh-CN", "投诉方式", "投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）"),
            // entity.customercomplaint.complaintmethod
            new TranslationSeedItem("entity.customercomplaint.complaintmethod", "zh-HK", "投诉方式", "投诉方式（0=电话，1=邮件，2=传真，3=现场，4=其他）"),

            // entity.customercomplaint.complainttype
            new TranslationSeedItem("entity.customercomplaint.complainttype", "en-US", "投诉类型", "投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）"),
            // entity.customercomplaint.complainttype
            new TranslationSeedItem("entity.customercomplaint.complainttype", "ja-JP", "投诉类型", "投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）"),
            // entity.customercomplaint.complainttype
            new TranslationSeedItem("entity.customercomplaint.complainttype", "zh-CN", "投诉类型", "投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）"),
            // entity.customercomplaint.complainttype
            new TranslationSeedItem("entity.customercomplaint.complainttype", "zh-HK", "投诉类型", "投诉类型（0=质量，1=交期，2=服务，3=价格，4=其他）"),

            // entity.customercomplaint.complaintlevel
            new TranslationSeedItem("entity.customercomplaint.complaintlevel", "en-US", "投诉等级", "投诉等级（0=一般，1=重要，2=紧急，3=严重）"),
            // entity.customercomplaint.complaintlevel
            new TranslationSeedItem("entity.customercomplaint.complaintlevel", "ja-JP", "投诉等级", "投诉等级（0=一般，1=重要，2=紧急，3=严重）"),
            // entity.customercomplaint.complaintlevel
            new TranslationSeedItem("entity.customercomplaint.complaintlevel", "zh-CN", "投诉等级", "投诉等级（0=一般，1=重要，2=紧急，3=严重）"),
            // entity.customercomplaint.complaintlevel
            new TranslationSeedItem("entity.customercomplaint.complaintlevel", "zh-HK", "投诉等级", "投诉等级（0=一般，1=重要，2=紧急，3=严重）"),

            // entity.customercomplaint.responsibledeptid
            new TranslationSeedItem("entity.customercomplaint.responsibledeptid", "en-US", "责任部门ID", "责任部门ID（序列化为string以避免Javascript精度问题）"),
            // entity.customercomplaint.responsibledeptid
            new TranslationSeedItem("entity.customercomplaint.responsibledeptid", "ja-JP", "责任部门ID", "责任部门ID（序列化为string以避免Javascript精度问题）"),
            // entity.customercomplaint.responsibledeptid
            new TranslationSeedItem("entity.customercomplaint.responsibledeptid", "zh-CN", "责任部门ID", "责任部门ID（序列化为string以避免Javascript精度问题）"),
            // entity.customercomplaint.responsibledeptid
            new TranslationSeedItem("entity.customercomplaint.responsibledeptid", "zh-HK", "责任部门ID", "责任部门ID（序列化为string以避免Javascript精度问题）"),

            // entity.customercomplaint.responsibledeptname
            new TranslationSeedItem("entity.customercomplaint.responsibledeptname", "en-US", "责任部门名称", "责任部门名称"),
            // entity.customercomplaint.responsibledeptname
            new TranslationSeedItem("entity.customercomplaint.responsibledeptname", "ja-JP", "责任部门名称", "责任部门名称"),
            // entity.customercomplaint.responsibledeptname
            new TranslationSeedItem("entity.customercomplaint.responsibledeptname", "zh-CN", "责任部门名称", "责任部门名称"),
            // entity.customercomplaint.responsibledeptname
            new TranslationSeedItem("entity.customercomplaint.responsibledeptname", "zh-HK", "责任部门名称", "责任部门名称"),

            // entity.customercomplaint.responsiblepersonid
            new TranslationSeedItem("entity.customercomplaint.responsiblepersonid", "en-US", "责任人ID", "责任人ID（序列化为string以避免Javascript精度问题）"),
            // entity.customercomplaint.responsiblepersonid
            new TranslationSeedItem("entity.customercomplaint.responsiblepersonid", "ja-JP", "责任人ID", "责任人ID（序列化为string以避免Javascript精度问题）"),
            // entity.customercomplaint.responsiblepersonid
            new TranslationSeedItem("entity.customercomplaint.responsiblepersonid", "zh-CN", "责任人ID", "责任人ID（序列化为string以避免Javascript精度问题）"),
            // entity.customercomplaint.responsiblepersonid
            new TranslationSeedItem("entity.customercomplaint.responsiblepersonid", "zh-HK", "责任人ID", "责任人ID（序列化为string以避免Javascript精度问题）"),

            // entity.customercomplaint.responsiblepersonname
            new TranslationSeedItem("entity.customercomplaint.responsiblepersonname", "en-US", "责任人姓名", "责任人姓名"),
            // entity.customercomplaint.responsiblepersonname
            new TranslationSeedItem("entity.customercomplaint.responsiblepersonname", "ja-JP", "责任人姓名", "责任人姓名"),
            // entity.customercomplaint.responsiblepersonname
            new TranslationSeedItem("entity.customercomplaint.responsiblepersonname", "zh-CN", "责任人姓名", "责任人姓名"),
            // entity.customercomplaint.responsiblepersonname
            new TranslationSeedItem("entity.customercomplaint.responsiblepersonname", "zh-HK", "责任人姓名", "责任人姓名"),

            // entity.customercomplaint.requiredreplydate
            new TranslationSeedItem("entity.customercomplaint.requiredreplydate", "en-US", "要求回复日期", "要求回复日期"),
            // entity.customercomplaint.requiredreplydate
            new TranslationSeedItem("entity.customercomplaint.requiredreplydate", "ja-JP", "要求回复日期", "要求回复日期"),
            // entity.customercomplaint.requiredreplydate
            new TranslationSeedItem("entity.customercomplaint.requiredreplydate", "zh-CN", "要求回复日期", "要求回复日期"),
            // entity.customercomplaint.requiredreplydate
            new TranslationSeedItem("entity.customercomplaint.requiredreplydate", "zh-HK", "要求回复日期", "要求回复日期"),

            // entity.customercomplaint.actualreplydate
            new TranslationSeedItem("entity.customercomplaint.actualreplydate", "en-US", "实际回复日期", "实际回复日期"),
            // entity.customercomplaint.actualreplydate
            new TranslationSeedItem("entity.customercomplaint.actualreplydate", "ja-JP", "实际回复日期", "实际回复日期"),
            // entity.customercomplaint.actualreplydate
            new TranslationSeedItem("entity.customercomplaint.actualreplydate", "zh-CN", "实际回复日期", "实际回复日期"),
            // entity.customercomplaint.actualreplydate
            new TranslationSeedItem("entity.customercomplaint.actualreplydate", "zh-HK", "实际回复日期", "实际回复日期"),

            // entity.customercomplaint.complaintstatus
            new TranslationSeedItem("entity.customercomplaint.complaintstatus", "en-US", "客诉状态", "客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）"),
            // entity.customercomplaint.complaintstatus
            new TranslationSeedItem("entity.customercomplaint.complaintstatus", "ja-JP", "客诉状态", "客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）"),
            // entity.customercomplaint.complaintstatus
            new TranslationSeedItem("entity.customercomplaint.complaintstatus", "zh-CN", "客诉状态", "客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）"),
            // entity.customercomplaint.complaintstatus
            new TranslationSeedItem("entity.customercomplaint.complaintstatus", "zh-HK", "客诉状态", "客诉状态（0=待处理，1=处理中，2=已回复，3=已关闭，4=已驳回）"),

            // entity.customercomplaint.complaintdescription
            new TranslationSeedItem("entity.customercomplaint.complaintdescription", "en-US", "客诉描述", "客诉描述"),
            // entity.customercomplaint.complaintdescription
            new TranslationSeedItem("entity.customercomplaint.complaintdescription", "ja-JP", "客诉描述", "客诉描述"),
            // entity.customercomplaint.complaintdescription
            new TranslationSeedItem("entity.customercomplaint.complaintdescription", "zh-CN", "客诉描述", "客诉描述"),
            // entity.customercomplaint.complaintdescription
            new TranslationSeedItem("entity.customercomplaint.complaintdescription", "zh-HK", "客诉描述", "客诉描述"),

            // entity.customercomplaint.handlingresult
            new TranslationSeedItem("entity.customercomplaint.handlingresult", "en-US", "处理结果", "处理结果/回复内容"),
            // entity.customercomplaint.handlingresult
            new TranslationSeedItem("entity.customercomplaint.handlingresult", "ja-JP", "处理结果", "处理结果/回复内容"),
            // entity.customercomplaint.handlingresult
            new TranslationSeedItem("entity.customercomplaint.handlingresult", "zh-CN", "处理结果", "处理结果/回复内容"),
            // entity.customercomplaint.handlingresult
            new TranslationSeedItem("entity.customercomplaint.handlingresult", "zh-HK", "处理结果", "处理结果/回复内容"),

            // entity.customercomplaint.customersatisfaction
            new TranslationSeedItem("entity.customercomplaint.customersatisfaction", "en-US", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),
            // entity.customercomplaint.customersatisfaction
            new TranslationSeedItem("entity.customercomplaint.customersatisfaction", "ja-JP", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),
            // entity.customercomplaint.customersatisfaction
            new TranslationSeedItem("entity.customercomplaint.customersatisfaction", "zh-CN", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),
            // entity.customercomplaint.customersatisfaction
            new TranslationSeedItem("entity.customercomplaint.customersatisfaction", "zh-HK", "客户满意度", "客户满意度（0=不满意，1=一般，2=满意，3=非常满意）"),

            // entity.customercomplaint.relatedplant
            new TranslationSeedItem("entity.customercomplaint.relatedplant", "en-US", "关联工厂", "关联工厂"),
            // entity.customercomplaint.relatedplant
            new TranslationSeedItem("entity.customercomplaint.relatedplant", "ja-JP", "关联工厂", "关联工厂"),
            // entity.customercomplaint.relatedplant
            new TranslationSeedItem("entity.customercomplaint.relatedplant", "zh-CN", "关联工厂", "关联工厂"),
            // entity.customercomplaint.relatedplant
            new TranslationSeedItem("entity.customercomplaint.relatedplant", "zh-HK", "关联工厂", "关联工厂"),

            // entity.customercomplaint.sortorder
            new TranslationSeedItem("entity.customercomplaint.sortorder", "en-US", "排序号", "排序号（越小越靠前）"),
            // entity.customercomplaint.sortorder
            new TranslationSeedItem("entity.customercomplaint.sortorder", "ja-JP", "排序号", "排序号（越小越靠前）"),
            // entity.customercomplaint.sortorder
            new TranslationSeedItem("entity.customercomplaint.sortorder", "zh-CN", "排序号", "排序号（越小越靠前）"),
            // entity.customercomplaint.sortorder
            new TranslationSeedItem("entity.customercomplaint.sortorder", "zh-HK", "排序号", "排序号（越小越靠前）"),

            // entity.customercomplaint.items
            new TranslationSeedItem("entity.customercomplaint.items", "en-US", "客诉明细列表", "客诉明细列表（主子表关系）"),
            // entity.customercomplaint.items
            new TranslationSeedItem("entity.customercomplaint.items", "ja-JP", "客诉明细列表", "客诉明细列表（主子表关系）"),
            // entity.customercomplaint.items
            new TranslationSeedItem("entity.customercomplaint.items", "zh-CN", "客诉明细列表", "客诉明细列表（主子表关系）"),
            // entity.customercomplaint.items
            new TranslationSeedItem("entity.customercomplaint.items", "zh-HK", "客诉明细列表", "客诉明细列表（主子表关系）"),
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
