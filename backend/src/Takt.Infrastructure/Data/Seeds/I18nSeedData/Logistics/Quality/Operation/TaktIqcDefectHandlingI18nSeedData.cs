// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktIqcDefectHandlingI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktIqcDefectHandling 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktIqcDefectHandling 实体国际化翻译种子（键前缀 entity.iqcDefectHandling.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktIqcDefectHandlingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktIqcDefectHandling 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 iqcDefectHandling 实体翻译...", tenantCode);

        foreach (var item in GetIqcDefectHandlingTranslations())
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

        TaktLogger.Information("TaktIqcDefectHandling 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktIqcDefectHandling 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.iqcDefectHandling._self / entity.iqcDefectHandling.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetIqcDefectHandlingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.iqcDefectHandling._self
            new TranslationSeedItem("entity.iqcDefectHandling._self", "en-US", "Iqc Defect Handling Information", "实体名称"),
            // entity.iqcDefectHandling._self
            new TranslationSeedItem("entity.iqcDefectHandling._self", "ja-JP", "IQC进货检验不良处理记录信息", "实体名称"),
            // entity.iqcDefectHandling._self
            new TranslationSeedItem("entity.iqcDefectHandling._self", "zh-CN", "IQC进货检验不良处理记录信息", "实体名称"),
            // entity.iqcDefectHandling._self
            new TranslationSeedItem("entity.iqcDefectHandling._self", "zh-HK", "IQC进货检验不良处理记录信息", "实体名称"),

            // entity.iqcDefectHandling.code
            new TranslationSeedItem("entity.iqcDefectHandling.code", "en-US", "IQC不良处理编码", "IQC不良处理编码"),
            // entity.iqcDefectHandling.code
            new TranslationSeedItem("entity.iqcDefectHandling.code", "ja-JP", "IQC不良处理编码", "IQC不良处理编码"),
            // entity.iqcDefectHandling.code
            new TranslationSeedItem("entity.iqcDefectHandling.code", "zh-CN", "IQC不良处理编码", "IQC不良处理编码"),
            // entity.iqcDefectHandling.code
            new TranslationSeedItem("entity.iqcDefectHandling.code", "zh-HK", "IQC不良处理编码", "IQC不良处理编码"),

            // entity.iqcDefectHandling.iqcorderitemid
            new TranslationSeedItem("entity.iqcDefectHandling.iqcorderitemid", "en-US", "IQC检验单明细ID", "IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.iqcDefectHandling.iqcorderitemid
            new TranslationSeedItem("entity.iqcDefectHandling.iqcorderitemid", "ja-JP", "IQC检验单明细ID", "IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.iqcDefectHandling.iqcorderitemid
            new TranslationSeedItem("entity.iqcDefectHandling.iqcorderitemid", "zh-CN", "IQC检验单明细ID", "IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),
            // entity.iqcDefectHandling.iqcorderitemid
            new TranslationSeedItem("entity.iqcDefectHandling.iqcorderitemid", "zh-HK", "IQC检验单明细ID", "IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）"),

            // entity.iqcDefectHandling.iqcordercode
            new TranslationSeedItem("entity.iqcDefectHandling.iqcordercode", "en-US", "IQC检验单编码", "IQC检验单编码（冗余字段，便于查询）"),
            // entity.iqcDefectHandling.iqcordercode
            new TranslationSeedItem("entity.iqcDefectHandling.iqcordercode", "ja-JP", "IQC检验单编码", "IQC检验单编码（冗余字段，便于查询）"),
            // entity.iqcDefectHandling.iqcordercode
            new TranslationSeedItem("entity.iqcDefectHandling.iqcordercode", "zh-CN", "IQC检验单编码", "IQC检验单编码（冗余字段，便于查询）"),
            // entity.iqcDefectHandling.iqcordercode
            new TranslationSeedItem("entity.iqcDefectHandling.iqcordercode", "zh-HK", "IQC检验单编码", "IQC检验单编码（冗余字段，便于查询）"),

            // entity.iqcDefectHandling.linenumber
            new TranslationSeedItem("entity.iqcDefectHandling.linenumber", "en-US", "检验单行号", "行号（项号/序号，固定步长=10）"),
            // entity.iqcDefectHandling.linenumber
            new TranslationSeedItem("entity.iqcDefectHandling.linenumber", "ja-JP", "检验单行号", "行号（项号/序号，固定步长=10）"),
            // entity.iqcDefectHandling.linenumber
            new TranslationSeedItem("entity.iqcDefectHandling.linenumber", "zh-CN", "检验单行号", "行号（项号/序号，固定步长=10）"),
            // entity.iqcDefectHandling.linenumber
            new TranslationSeedItem("entity.iqcDefectHandling.linenumber", "zh-HK", "检验单行号", "行号（项号/序号，固定步长=10）"),

            // entity.iqcDefectHandling.defecttype
            new TranslationSeedItem("entity.iqcDefectHandling.defecttype", "en-US", "不良类型", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),
            // entity.iqcDefectHandling.defecttype
            new TranslationSeedItem("entity.iqcDefectHandling.defecttype", "ja-JP", "不良类型", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),
            // entity.iqcDefectHandling.defecttype
            new TranslationSeedItem("entity.iqcDefectHandling.defecttype", "zh-CN", "不良类型", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),
            // entity.iqcDefectHandling.defecttype
            new TranslationSeedItem("entity.iqcDefectHandling.defecttype", "zh-HK", "不良类型", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),

            // entity.iqcDefectHandling.defectcode
            new TranslationSeedItem("entity.iqcDefectHandling.defectcode", "en-US", "不良现象编码", "不良现象编码"),
            // entity.iqcDefectHandling.defectcode
            new TranslationSeedItem("entity.iqcDefectHandling.defectcode", "ja-JP", "不良现象编码", "不良现象编码"),
            // entity.iqcDefectHandling.defectcode
            new TranslationSeedItem("entity.iqcDefectHandling.defectcode", "zh-CN", "不良现象编码", "不良现象编码"),
            // entity.iqcDefectHandling.defectcode
            new TranslationSeedItem("entity.iqcDefectHandling.defectcode", "zh-HK", "不良现象编码", "不良现象编码"),

            // entity.iqcDefectHandling.defectdescription
            new TranslationSeedItem("entity.iqcDefectHandling.defectdescription", "en-US", "不良现象描述", "不良现象描述"),
            // entity.iqcDefectHandling.defectdescription
            new TranslationSeedItem("entity.iqcDefectHandling.defectdescription", "ja-JP", "不良现象描述", "不良现象描述"),
            // entity.iqcDefectHandling.defectdescription
            new TranslationSeedItem("entity.iqcDefectHandling.defectdescription", "zh-CN", "不良现象描述", "不良现象描述"),
            // entity.iqcDefectHandling.defectdescription
            new TranslationSeedItem("entity.iqcDefectHandling.defectdescription", "zh-HK", "不良现象描述", "不良现象描述"),

            // entity.iqcDefectHandling.defectquantity
            new TranslationSeedItem("entity.iqcDefectHandling.defectquantity", "en-US", "不良数量", "不良数量"),
            // entity.iqcDefectHandling.defectquantity
            new TranslationSeedItem("entity.iqcDefectHandling.defectquantity", "ja-JP", "不良数量", "不良数量"),
            // entity.iqcDefectHandling.defectquantity
            new TranslationSeedItem("entity.iqcDefectHandling.defectquantity", "zh-CN", "不良数量", "不良数量"),
            // entity.iqcDefectHandling.defectquantity
            new TranslationSeedItem("entity.iqcDefectHandling.defectquantity", "zh-HK", "不良数量", "不良数量"),

            // entity.iqcDefectHandling.handlingmethod
            new TranslationSeedItem("entity.iqcDefectHandling.handlingmethod", "en-US", "处理方式", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),
            // entity.iqcDefectHandling.handlingmethod
            new TranslationSeedItem("entity.iqcDefectHandling.handlingmethod", "ja-JP", "处理方式", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),
            // entity.iqcDefectHandling.handlingmethod
            new TranslationSeedItem("entity.iqcDefectHandling.handlingmethod", "zh-CN", "处理方式", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),
            // entity.iqcDefectHandling.handlingmethod
            new TranslationSeedItem("entity.iqcDefectHandling.handlingmethod", "zh-HK", "处理方式", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),

            // entity.iqcDefectHandling.handlingdescription
            new TranslationSeedItem("entity.iqcDefectHandling.handlingdescription", "en-US", "处理说明", "处理说明"),
            // entity.iqcDefectHandling.handlingdescription
            new TranslationSeedItem("entity.iqcDefectHandling.handlingdescription", "ja-JP", "处理说明", "处理说明"),
            // entity.iqcDefectHandling.handlingdescription
            new TranslationSeedItem("entity.iqcDefectHandling.handlingdescription", "zh-CN", "处理说明", "处理说明"),
            // entity.iqcDefectHandling.handlingdescription
            new TranslationSeedItem("entity.iqcDefectHandling.handlingdescription", "zh-HK", "处理说明", "处理说明"),

            // entity.iqcDefectHandling.responsibledept
            new TranslationSeedItem("entity.iqcDefectHandling.responsibledept", "en-US", "责任部门", "责任部门"),
            // entity.iqcDefectHandling.responsibledept
            new TranslationSeedItem("entity.iqcDefectHandling.responsibledept", "ja-JP", "责任部门", "责任部门"),
            // entity.iqcDefectHandling.responsibledept
            new TranslationSeedItem("entity.iqcDefectHandling.responsibledept", "zh-CN", "责任部门", "责任部门"),
            // entity.iqcDefectHandling.responsibledept
            new TranslationSeedItem("entity.iqcDefectHandling.responsibledept", "zh-HK", "责任部门", "责任部门"),

            // entity.iqcDefectHandling.responsibleby
            new TranslationSeedItem("entity.iqcDefectHandling.responsibleby", "en-US", "责任人", "责任人（人员代码）"),
            // entity.iqcDefectHandling.responsibleby
            new TranslationSeedItem("entity.iqcDefectHandling.responsibleby", "ja-JP", "责任人", "责任人（人员代码）"),
            // entity.iqcDefectHandling.responsibleby
            new TranslationSeedItem("entity.iqcDefectHandling.responsibleby", "zh-CN", "责任人", "责任人（人员代码）"),
            // entity.iqcDefectHandling.responsibleby
            new TranslationSeedItem("entity.iqcDefectHandling.responsibleby", "zh-HK", "责任人", "责任人（人员代码）"),

            // entity.iqcDefectHandling.handlerby
            new TranslationSeedItem("entity.iqcDefectHandling.handlerby", "en-US", "处理人", "处理人（人员代码）"),
            // entity.iqcDefectHandling.handlerby
            new TranslationSeedItem("entity.iqcDefectHandling.handlerby", "ja-JP", "处理人", "处理人（人员代码）"),
            // entity.iqcDefectHandling.handlerby
            new TranslationSeedItem("entity.iqcDefectHandling.handlerby", "zh-CN", "处理人", "处理人（人员代码）"),
            // entity.iqcDefectHandling.handlerby
            new TranslationSeedItem("entity.iqcDefectHandling.handlerby", "zh-HK", "处理人", "处理人（人员代码）"),

            // entity.iqcDefectHandling.handlingat
            new TranslationSeedItem("entity.iqcDefectHandling.handlingat", "en-US", "处理时间", "处理时间"),
            // entity.iqcDefectHandling.handlingat
            new TranslationSeedItem("entity.iqcDefectHandling.handlingat", "ja-JP", "处理时间", "处理时间"),
            // entity.iqcDefectHandling.handlingat
            new TranslationSeedItem("entity.iqcDefectHandling.handlingat", "zh-CN", "处理时间", "处理时间"),
            // entity.iqcDefectHandling.handlingat
            new TranslationSeedItem("entity.iqcDefectHandling.handlingat", "zh-HK", "处理时间", "处理时间"),

            // entity.iqcDefectHandling.handlingstatus
            new TranslationSeedItem("entity.iqcDefectHandling.handlingstatus", "en-US", "处理状态", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),
            // entity.iqcDefectHandling.handlingstatus
            new TranslationSeedItem("entity.iqcDefectHandling.handlingstatus", "ja-JP", "处理状态", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),
            // entity.iqcDefectHandling.handlingstatus
            new TranslationSeedItem("entity.iqcDefectHandling.handlingstatus", "zh-CN", "处理状态", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),
            // entity.iqcDefectHandling.handlingstatus
            new TranslationSeedItem("entity.iqcDefectHandling.handlingstatus", "zh-HK", "处理状态", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),

            // entity.iqcDefectHandling.correctiveaction
            new TranslationSeedItem("entity.iqcDefectHandling.correctiveaction", "en-US", "纠正措施", "预防措施/纠正措施"),
            // entity.iqcDefectHandling.correctiveaction
            new TranslationSeedItem("entity.iqcDefectHandling.correctiveaction", "ja-JP", "纠正措施", "预防措施/纠正措施"),
            // entity.iqcDefectHandling.correctiveaction
            new TranslationSeedItem("entity.iqcDefectHandling.correctiveaction", "zh-CN", "纠正措施", "预防措施/纠正措施"),
            // entity.iqcDefectHandling.correctiveaction
            new TranslationSeedItem("entity.iqcDefectHandling.correctiveaction", "zh-HK", "纠正措施", "预防措施/纠正措施"),

            // entity.iqcDefectHandling.defectimages
            new TranslationSeedItem("entity.iqcDefectHandling.defectimages", "en-US", "不良图片", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.iqcDefectHandling.defectimages
            new TranslationSeedItem("entity.iqcDefectHandling.defectimages", "ja-JP", "不良图片", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.iqcDefectHandling.defectimages
            new TranslationSeedItem("entity.iqcDefectHandling.defectimages", "zh-CN", "不良图片", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.iqcDefectHandling.defectimages
            new TranslationSeedItem("entity.iqcDefectHandling.defectimages", "zh-HK", "不良图片", "不良图片（JSON格式，存储不良图片URL列表）"),
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
