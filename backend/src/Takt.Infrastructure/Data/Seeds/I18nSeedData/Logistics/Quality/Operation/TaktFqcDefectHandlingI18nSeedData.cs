// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktFqcDefectHandlingI18nSeedData.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktFqcDefectHandling 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktFqcDefectHandling 实体国际化翻译种子（键前缀 entity.fqcDefectHandling.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktFqcDefectHandlingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktFqcDefectHandling 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 fqcDefectHandling 实体翻译...", tenantCode);

        foreach (var item in GetFqcDefectHandlingTranslations())
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

        TaktLogger.Information("TaktFqcDefectHandling 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktFqcDefectHandling 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.fqcDefectHandling._self / entity.fqcDefectHandling.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetFqcDefectHandlingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.fqcDefectHandling._self
            new TranslationSeedItem("entity.fqcDefectHandling._self", "en-US", "Fqc Defect Handling Information", "实体名称"),
            // entity.fqcDefectHandling._self
            new TranslationSeedItem("entity.fqcDefectHandling._self", "ja-JP", "FQC出货检验不良处理记录信息", "实体名称"),
            // entity.fqcDefectHandling._self
            new TranslationSeedItem("entity.fqcDefectHandling._self", "zh-CN", "FQC出货检验不良处理记录信息", "实体名称"),
            // entity.fqcDefectHandling._self
            new TranslationSeedItem("entity.fqcDefectHandling._self", "zh-HK", "FQC出货检验不良处理记录信息", "实体名称"),

            // entity.fqcDefectHandling.code
            new TranslationSeedItem("entity.fqcDefectHandling.code", "en-US", "FQC不良处理编码", "FQC不良处理编码"),
            // entity.fqcDefectHandling.code
            new TranslationSeedItem("entity.fqcDefectHandling.code", "ja-JP", "FQC不良处理编码", "FQC不良处理编码"),
            // entity.fqcDefectHandling.code
            new TranslationSeedItem("entity.fqcDefectHandling.code", "zh-CN", "FQC不良处理编码", "FQC不良处理编码"),
            // entity.fqcDefectHandling.code
            new TranslationSeedItem("entity.fqcDefectHandling.code", "zh-HK", "FQC不良处理编码", "FQC不良处理编码"),

            // entity.fqcDefectHandling.fqcorderitemid
            new TranslationSeedItem("entity.fqcDefectHandling.fqcorderitemid", "en-US", "FQC检验单明细ID", "FQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.fqcDefectHandling.fqcorderitemid
            new TranslationSeedItem("entity.fqcDefectHandling.fqcorderitemid", "ja-JP", "FQC检验单明细ID", "FQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.fqcDefectHandling.fqcorderitemid
            new TranslationSeedItem("entity.fqcDefectHandling.fqcorderitemid", "zh-CN", "FQC检验单明细ID", "FQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.fqcDefectHandling.fqcorderitemid
            new TranslationSeedItem("entity.fqcDefectHandling.fqcorderitemid", "zh-HK", "FQC检验单明细ID", "FQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.fqcDefectHandling.fqcordercode
            new TranslationSeedItem("entity.fqcDefectHandling.fqcordercode", "en-US", "FQC检验单编码", "FQC检验单编码（冗余字段，便于查询）"),
            // entity.fqcDefectHandling.fqcordercode
            new TranslationSeedItem("entity.fqcDefectHandling.fqcordercode", "ja-JP", "FQC检验单编码", "FQC检验单编码（冗余字段，便于查询）"),
            // entity.fqcDefectHandling.fqcordercode
            new TranslationSeedItem("entity.fqcDefectHandling.fqcordercode", "zh-CN", "FQC检验单编码", "FQC检验单编码（冗余字段，便于查询）"),
            // entity.fqcDefectHandling.fqcordercode
            new TranslationSeedItem("entity.fqcDefectHandling.fqcordercode", "zh-HK", "FQC检验单编码", "FQC检验单编码（冗余字段，便于查询）"),

            // entity.fqcDefectHandling.linenumber
            new TranslationSeedItem("entity.fqcDefectHandling.linenumber", "en-US", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.fqcDefectHandling.linenumber
            new TranslationSeedItem("entity.fqcDefectHandling.linenumber", "ja-JP", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.fqcDefectHandling.linenumber
            new TranslationSeedItem("entity.fqcDefectHandling.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.fqcDefectHandling.linenumber
            new TranslationSeedItem("entity.fqcDefectHandling.linenumber", "zh-HK", "行号", "行号（项号/序号，固定步长=10）"),

            // entity.fqcDefectHandling.defecttype
            new TranslationSeedItem("entity.fqcDefectHandling.defecttype", "en-US", "不良类型", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),
            // entity.fqcDefectHandling.defecttype
            new TranslationSeedItem("entity.fqcDefectHandling.defecttype", "ja-JP", "不良类型", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),
            // entity.fqcDefectHandling.defecttype
            new TranslationSeedItem("entity.fqcDefectHandling.defecttype", "zh-CN", "不良类型", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),
            // entity.fqcDefectHandling.defecttype
            new TranslationSeedItem("entity.fqcDefectHandling.defecttype", "zh-HK", "不良类型", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),

            // entity.fqcDefectHandling.defectcode
            new TranslationSeedItem("entity.fqcDefectHandling.defectcode", "en-US", "不良现象编码", "不良现象编码"),
            // entity.fqcDefectHandling.defectcode
            new TranslationSeedItem("entity.fqcDefectHandling.defectcode", "ja-JP", "不良现象编码", "不良现象编码"),
            // entity.fqcDefectHandling.defectcode
            new TranslationSeedItem("entity.fqcDefectHandling.defectcode", "zh-CN", "不良现象编码", "不良现象编码"),
            // entity.fqcDefectHandling.defectcode
            new TranslationSeedItem("entity.fqcDefectHandling.defectcode", "zh-HK", "不良现象编码", "不良现象编码"),

            // entity.fqcDefectHandling.defectdescription
            new TranslationSeedItem("entity.fqcDefectHandling.defectdescription", "en-US", "不良现象描述", "不良现象描述"),
            // entity.fqcDefectHandling.defectdescription
            new TranslationSeedItem("entity.fqcDefectHandling.defectdescription", "ja-JP", "不良现象描述", "不良现象描述"),
            // entity.fqcDefectHandling.defectdescription
            new TranslationSeedItem("entity.fqcDefectHandling.defectdescription", "zh-CN", "不良现象描述", "不良现象描述"),
            // entity.fqcDefectHandling.defectdescription
            new TranslationSeedItem("entity.fqcDefectHandling.defectdescription", "zh-HK", "不良现象描述", "不良现象描述"),

            // entity.fqcDefectHandling.defectquantity
            new TranslationSeedItem("entity.fqcDefectHandling.defectquantity", "en-US", "不良数量", "不良数量"),
            // entity.fqcDefectHandling.defectquantity
            new TranslationSeedItem("entity.fqcDefectHandling.defectquantity", "ja-JP", "不良数量", "不良数量"),
            // entity.fqcDefectHandling.defectquantity
            new TranslationSeedItem("entity.fqcDefectHandling.defectquantity", "zh-CN", "不良数量", "不良数量"),
            // entity.fqcDefectHandling.defectquantity
            new TranslationSeedItem("entity.fqcDefectHandling.defectquantity", "zh-HK", "不良数量", "不良数量"),

            // entity.fqcDefectHandling.handlingmethod
            new TranslationSeedItem("entity.fqcDefectHandling.handlingmethod", "en-US", "处理方式", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),
            // entity.fqcDefectHandling.handlingmethod
            new TranslationSeedItem("entity.fqcDefectHandling.handlingmethod", "ja-JP", "处理方式", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),
            // entity.fqcDefectHandling.handlingmethod
            new TranslationSeedItem("entity.fqcDefectHandling.handlingmethod", "zh-CN", "处理方式", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),
            // entity.fqcDefectHandling.handlingmethod
            new TranslationSeedItem("entity.fqcDefectHandling.handlingmethod", "zh-HK", "处理方式", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),

            // entity.fqcDefectHandling.handlingdescription
            new TranslationSeedItem("entity.fqcDefectHandling.handlingdescription", "en-US", "处理说明", "处理说明"),
            // entity.fqcDefectHandling.handlingdescription
            new TranslationSeedItem("entity.fqcDefectHandling.handlingdescription", "ja-JP", "处理说明", "处理说明"),
            // entity.fqcDefectHandling.handlingdescription
            new TranslationSeedItem("entity.fqcDefectHandling.handlingdescription", "zh-CN", "处理说明", "处理说明"),
            // entity.fqcDefectHandling.handlingdescription
            new TranslationSeedItem("entity.fqcDefectHandling.handlingdescription", "zh-HK", "处理说明", "处理说明"),

            // entity.fqcDefectHandling.responsibledept
            new TranslationSeedItem("entity.fqcDefectHandling.responsibledept", "en-US", "责任部门", "责任部门"),
            // entity.fqcDefectHandling.responsibledept
            new TranslationSeedItem("entity.fqcDefectHandling.responsibledept", "ja-JP", "责任部门", "责任部门"),
            // entity.fqcDefectHandling.responsibledept
            new TranslationSeedItem("entity.fqcDefectHandling.responsibledept", "zh-CN", "责任部门", "责任部门"),
            // entity.fqcDefectHandling.responsibledept
            new TranslationSeedItem("entity.fqcDefectHandling.responsibledept", "zh-HK", "责任部门", "责任部门"),

            // entity.fqcDefectHandling.responsibleby
            new TranslationSeedItem("entity.fqcDefectHandling.responsibleby", "en-US", "责任人", "责任人（人员代码）"),
            // entity.fqcDefectHandling.responsibleby
            new TranslationSeedItem("entity.fqcDefectHandling.responsibleby", "ja-JP", "责任人", "责任人（人员代码）"),
            // entity.fqcDefectHandling.responsibleby
            new TranslationSeedItem("entity.fqcDefectHandling.responsibleby", "zh-CN", "责任人", "责任人（人员代码）"),
            // entity.fqcDefectHandling.responsibleby
            new TranslationSeedItem("entity.fqcDefectHandling.responsibleby", "zh-HK", "责任人", "责任人（人员代码）"),

            // entity.fqcDefectHandling.handlerby
            new TranslationSeedItem("entity.fqcDefectHandling.handlerby", "en-US", "处理人", "处理人（人员代码）"),
            // entity.fqcDefectHandling.handlerby
            new TranslationSeedItem("entity.fqcDefectHandling.handlerby", "ja-JP", "处理人", "处理人（人员代码）"),
            // entity.fqcDefectHandling.handlerby
            new TranslationSeedItem("entity.fqcDefectHandling.handlerby", "zh-CN", "处理人", "处理人（人员代码）"),
            // entity.fqcDefectHandling.handlerby
            new TranslationSeedItem("entity.fqcDefectHandling.handlerby", "zh-HK", "处理人", "处理人（人员代码）"),

            // entity.fqcDefectHandling.handlingat
            new TranslationSeedItem("entity.fqcDefectHandling.handlingat", "en-US", "处理时间", "处理时间"),
            // entity.fqcDefectHandling.handlingat
            new TranslationSeedItem("entity.fqcDefectHandling.handlingat", "ja-JP", "处理时间", "处理时间"),
            // entity.fqcDefectHandling.handlingat
            new TranslationSeedItem("entity.fqcDefectHandling.handlingat", "zh-CN", "处理时间", "处理时间"),
            // entity.fqcDefectHandling.handlingat
            new TranslationSeedItem("entity.fqcDefectHandling.handlingat", "zh-HK", "处理时间", "处理时间"),

            // entity.fqcDefectHandling.handlingstatus
            new TranslationSeedItem("entity.fqcDefectHandling.handlingstatus", "en-US", "处理状态", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),
            // entity.fqcDefectHandling.handlingstatus
            new TranslationSeedItem("entity.fqcDefectHandling.handlingstatus", "ja-JP", "处理状态", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),
            // entity.fqcDefectHandling.handlingstatus
            new TranslationSeedItem("entity.fqcDefectHandling.handlingstatus", "zh-CN", "处理状态", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),
            // entity.fqcDefectHandling.handlingstatus
            new TranslationSeedItem("entity.fqcDefectHandling.handlingstatus", "zh-HK", "处理状态", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),

            // entity.fqcDefectHandling.correctiveaction
            new TranslationSeedItem("entity.fqcDefectHandling.correctiveaction", "en-US", "纠正措施", "预防措施/纠正措施"),
            // entity.fqcDefectHandling.correctiveaction
            new TranslationSeedItem("entity.fqcDefectHandling.correctiveaction", "ja-JP", "纠正措施", "预防措施/纠正措施"),
            // entity.fqcDefectHandling.correctiveaction
            new TranslationSeedItem("entity.fqcDefectHandling.correctiveaction", "zh-CN", "纠正措施", "预防措施/纠正措施"),
            // entity.fqcDefectHandling.correctiveaction
            new TranslationSeedItem("entity.fqcDefectHandling.correctiveaction", "zh-HK", "纠正措施", "预防措施/纠正措施"),

            // entity.fqcDefectHandling.defectimages
            new TranslationSeedItem("entity.fqcDefectHandling.defectimages", "en-US", "不良图片", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.fqcDefectHandling.defectimages
            new TranslationSeedItem("entity.fqcDefectHandling.defectimages", "ja-JP", "不良图片", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.fqcDefectHandling.defectimages
            new TranslationSeedItem("entity.fqcDefectHandling.defectimages", "zh-CN", "不良图片", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.fqcDefectHandling.defectimages
            new TranslationSeedItem("entity.fqcDefectHandling.defectimages", "zh-HK", "不良图片", "不良图片（JSON格式，存储不良图片URL列表）"),

            // entity.fqcDefectHandling.orderitem
            new TranslationSeedItem("entity.fqcDefectHandling.orderitem", "en-US", "FQC检验单明细", "FQC检验单明细（主表）"),
            // entity.fqcDefectHandling.orderitem
            new TranslationSeedItem("entity.fqcDefectHandling.orderitem", "ja-JP", "FQC检验单明细", "FQC检验单明细（主表）"),
            // entity.fqcDefectHandling.orderitem
            new TranslationSeedItem("entity.fqcDefectHandling.orderitem", "zh-CN", "FQC检验单明细", "FQC检验单明细（主表）"),
            // entity.fqcDefectHandling.orderitem
            new TranslationSeedItem("entity.fqcDefectHandling.orderitem", "zh-HK", "FQC检验单明细", "FQC检验单明细（主表）"),
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
