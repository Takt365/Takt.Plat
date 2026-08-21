// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation
// 文件名称：TaktIpqcDefectHandlingI18nSeedData.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktIpqcDefectHandling 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Quality.Operation;

/// <summary>
/// TaktIpqcDefectHandling 实体国际化翻译种子（键前缀 entity.ipqcdefecthandling.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktIpqcDefectHandlingI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktIpqcDefectHandling 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ipqcdefecthandling 实体翻译...", tenantCode);

        foreach (var item in GetIpqcDefectHandlingTranslations())
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

        TaktLogger.Information("TaktIpqcDefectHandling 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktIpqcDefectHandling 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.ipqcdefecthandling._self / entity.ipqcdefecthandling.{{field}}；ResourceGroup=Operation；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetIpqcDefectHandlingTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.ipqcdefecthandling._self
            new TranslationSeedItem("entity.ipqcdefecthandling._self", "en-US", "Ipqc Defect Handling Information_us", "实体名称"),
            // entity.ipqcdefecthandling._self
            new TranslationSeedItem("entity.ipqcdefecthandling._self", "ja-JP", "IPQC制程检验不良处理记录信息_jp", "实体名称"),
            // entity.ipqcdefecthandling._self
            new TranslationSeedItem("entity.ipqcdefecthandling._self", "zh-CN", "IPQC制程检验不良处理记录信息", "实体名称"),
            // entity.ipqcdefecthandling._self
            new TranslationSeedItem("entity.ipqcdefecthandling._self", "zh-HK", "IPQC制程检验不良处理记录信息_hk", "实体名称"),

            // entity.ipqcdefecthandling.code
            new TranslationSeedItem("entity.ipqcdefecthandling.code", "en-US", "IPQC不良处理编码_us", "IPQC不良处理编码"),
            // entity.ipqcdefecthandling.code
            new TranslationSeedItem("entity.ipqcdefecthandling.code", "ja-JP", "IPQC不良处理编码_jp", "IPQC不良处理编码"),
            // entity.ipqcdefecthandling.code
            new TranslationSeedItem("entity.ipqcdefecthandling.code", "zh-CN", "IPQC不良处理编码", "IPQC不良处理编码"),
            // entity.ipqcdefecthandling.code
            new TranslationSeedItem("entity.ipqcdefecthandling.code", "zh-HK", "IPQC不良处理编码_hk", "IPQC不良处理编码"),

            // entity.ipqcdefecthandling.ipqcorderitemid
            new TranslationSeedItem("entity.ipqcdefecthandling.ipqcorderitemid", "en-US", "IPQC检验单明细ID_us", "IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）"),
            // entity.ipqcdefecthandling.ipqcorderitemid
            new TranslationSeedItem("entity.ipqcdefecthandling.ipqcorderitemid", "ja-JP", "IPQC检验单明细ID_jp", "IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）"),
            // entity.ipqcdefecthandling.ipqcorderitemid
            new TranslationSeedItem("entity.ipqcdefecthandling.ipqcorderitemid", "zh-CN", "IPQC检验单明细ID", "IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）"),
            // entity.ipqcdefecthandling.ipqcorderitemid
            new TranslationSeedItem("entity.ipqcdefecthandling.ipqcorderitemid", "zh-HK", "IPQC检验单明细ID_hk", "IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）"),

            // entity.ipqcdefecthandling.ipqcordercode
            new TranslationSeedItem("entity.ipqcdefecthandling.ipqcordercode", "en-US", "IPQC检验单编码_us", "IPQC检验单编码（冗余字段，便于查询）"),
            // entity.ipqcdefecthandling.ipqcordercode
            new TranslationSeedItem("entity.ipqcdefecthandling.ipqcordercode", "ja-JP", "IPQC检验单编码_jp", "IPQC检验单编码（冗余字段，便于查询）"),
            // entity.ipqcdefecthandling.ipqcordercode
            new TranslationSeedItem("entity.ipqcdefecthandling.ipqcordercode", "zh-CN", "IPQC检验单编码", "IPQC检验单编码（冗余字段，便于查询）"),
            // entity.ipqcdefecthandling.ipqcordercode
            new TranslationSeedItem("entity.ipqcdefecthandling.ipqcordercode", "zh-HK", "IPQC检验单编码_hk", "IPQC检验单编码（冗余字段，便于查询）"),

            // entity.ipqcdefecthandling.linenumber
            new TranslationSeedItem("entity.ipqcdefecthandling.linenumber", "en-US", "行号_us", "行号（项号/序号，固定步长=10）"),
            // entity.ipqcdefecthandling.linenumber
            new TranslationSeedItem("entity.ipqcdefecthandling.linenumber", "ja-JP", "行号_jp", "行号（项号/序号，固定步长=10）"),
            // entity.ipqcdefecthandling.linenumber
            new TranslationSeedItem("entity.ipqcdefecthandling.linenumber", "zh-CN", "行号", "行号（项号/序号，固定步长=10）"),
            // entity.ipqcdefecthandling.linenumber
            new TranslationSeedItem("entity.ipqcdefecthandling.linenumber", "zh-HK", "行号_hk", "行号（项号/序号，固定步长=10）"),

            // entity.ipqcdefecthandling.defecttype
            new TranslationSeedItem("entity.ipqcdefecthandling.defecttype", "en-US", "不良类型_us", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),
            // entity.ipqcdefecthandling.defecttype
            new TranslationSeedItem("entity.ipqcdefecthandling.defecttype", "ja-JP", "不良类型_jp", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),
            // entity.ipqcdefecthandling.defecttype
            new TranslationSeedItem("entity.ipqcdefecthandling.defecttype", "zh-CN", "不良类型", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),
            // entity.ipqcdefecthandling.defecttype
            new TranslationSeedItem("entity.ipqcdefecthandling.defecttype", "zh-HK", "不良类型_hk", "不良类型（0=轻微，1=一般，2=严重，3=致命）"),

            // entity.ipqcdefecthandling.defectcode
            new TranslationSeedItem("entity.ipqcdefecthandling.defectcode", "en-US", "不良现象编码_us", "不良现象编码"),
            // entity.ipqcdefecthandling.defectcode
            new TranslationSeedItem("entity.ipqcdefecthandling.defectcode", "ja-JP", "不良现象编码_jp", "不良现象编码"),
            // entity.ipqcdefecthandling.defectcode
            new TranslationSeedItem("entity.ipqcdefecthandling.defectcode", "zh-CN", "不良现象编码", "不良现象编码"),
            // entity.ipqcdefecthandling.defectcode
            new TranslationSeedItem("entity.ipqcdefecthandling.defectcode", "zh-HK", "不良现象编码_hk", "不良现象编码"),

            // entity.ipqcdefecthandling.defectdescription
            new TranslationSeedItem("entity.ipqcdefecthandling.defectdescription", "en-US", "不良现象描述_us", "不良现象描述"),
            // entity.ipqcdefecthandling.defectdescription
            new TranslationSeedItem("entity.ipqcdefecthandling.defectdescription", "ja-JP", "不良现象描述_jp", "不良现象描述"),
            // entity.ipqcdefecthandling.defectdescription
            new TranslationSeedItem("entity.ipqcdefecthandling.defectdescription", "zh-CN", "不良现象描述", "不良现象描述"),
            // entity.ipqcdefecthandling.defectdescription
            new TranslationSeedItem("entity.ipqcdefecthandling.defectdescription", "zh-HK", "不良现象描述_hk", "不良现象描述"),

            // entity.ipqcdefecthandling.defectquantity
            new TranslationSeedItem("entity.ipqcdefecthandling.defectquantity", "en-US", "不良数量_us", "不良数量"),
            // entity.ipqcdefecthandling.defectquantity
            new TranslationSeedItem("entity.ipqcdefecthandling.defectquantity", "ja-JP", "不良数量_jp", "不良数量"),
            // entity.ipqcdefecthandling.defectquantity
            new TranslationSeedItem("entity.ipqcdefecthandling.defectquantity", "zh-CN", "不良数量", "不良数量"),
            // entity.ipqcdefecthandling.defectquantity
            new TranslationSeedItem("entity.ipqcdefecthandling.defectquantity", "zh-HK", "不良数量_hk", "不良数量"),

            // entity.ipqcdefecthandling.handlingmethod
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingmethod", "en-US", "处理方式_us", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),
            // entity.ipqcdefecthandling.handlingmethod
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingmethod", "ja-JP", "处理方式_jp", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),
            // entity.ipqcdefecthandling.handlingmethod
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingmethod", "zh-CN", "处理方式", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),
            // entity.ipqcdefecthandling.handlingmethod
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingmethod", "zh-HK", "处理方式_hk", "处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）"),

            // entity.ipqcdefecthandling.handlingdescription
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingdescription", "en-US", "处理说明_us", "处理说明"),
            // entity.ipqcdefecthandling.handlingdescription
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingdescription", "ja-JP", "处理说明_jp", "处理说明"),
            // entity.ipqcdefecthandling.handlingdescription
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingdescription", "zh-CN", "处理说明", "处理说明"),
            // entity.ipqcdefecthandling.handlingdescription
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingdescription", "zh-HK", "处理说明_hk", "处理说明"),

            // entity.ipqcdefecthandling.responsibledept
            new TranslationSeedItem("entity.ipqcdefecthandling.responsibledept", "en-US", "责任部门_us", "责任部门"),
            // entity.ipqcdefecthandling.responsibledept
            new TranslationSeedItem("entity.ipqcdefecthandling.responsibledept", "ja-JP", "责任部门_jp", "责任部门"),
            // entity.ipqcdefecthandling.responsibledept
            new TranslationSeedItem("entity.ipqcdefecthandling.responsibledept", "zh-CN", "责任部门", "责任部门"),
            // entity.ipqcdefecthandling.responsibledept
            new TranslationSeedItem("entity.ipqcdefecthandling.responsibledept", "zh-HK", "责任部门_hk", "责任部门"),

            // entity.ipqcdefecthandling.responsibleby
            new TranslationSeedItem("entity.ipqcdefecthandling.responsibleby", "en-US", "责任人_us", "责任人（人员代码）"),
            // entity.ipqcdefecthandling.responsibleby
            new TranslationSeedItem("entity.ipqcdefecthandling.responsibleby", "ja-JP", "责任人_jp", "责任人（人员代码）"),
            // entity.ipqcdefecthandling.responsibleby
            new TranslationSeedItem("entity.ipqcdefecthandling.responsibleby", "zh-CN", "责任人", "责任人（人员代码）"),
            // entity.ipqcdefecthandling.responsibleby
            new TranslationSeedItem("entity.ipqcdefecthandling.responsibleby", "zh-HK", "责任人_hk", "责任人（人员代码）"),

            // entity.ipqcdefecthandling.handlerby
            new TranslationSeedItem("entity.ipqcdefecthandling.handlerby", "en-US", "处理人_us", "处理人（人员代码）"),
            // entity.ipqcdefecthandling.handlerby
            new TranslationSeedItem("entity.ipqcdefecthandling.handlerby", "ja-JP", "处理人_jp", "处理人（人员代码）"),
            // entity.ipqcdefecthandling.handlerby
            new TranslationSeedItem("entity.ipqcdefecthandling.handlerby", "zh-CN", "处理人", "处理人（人员代码）"),
            // entity.ipqcdefecthandling.handlerby
            new TranslationSeedItem("entity.ipqcdefecthandling.handlerby", "zh-HK", "处理人_hk", "处理人（人员代码）"),

            // entity.ipqcdefecthandling.handlingat
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingat", "en-US", "处理时间_us", "处理时间"),
            // entity.ipqcdefecthandling.handlingat
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingat", "ja-JP", "处理时间_jp", "处理时间"),
            // entity.ipqcdefecthandling.handlingat
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingat", "zh-CN", "处理时间", "处理时间"),
            // entity.ipqcdefecthandling.handlingat
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingat", "zh-HK", "处理时间_hk", "处理时间"),

            // entity.ipqcdefecthandling.correctiveaction
            new TranslationSeedItem("entity.ipqcdefecthandling.correctiveaction", "en-US", "纠正措施_us", "预防措施/纠正措施"),
            // entity.ipqcdefecthandling.correctiveaction
            new TranslationSeedItem("entity.ipqcdefecthandling.correctiveaction", "ja-JP", "纠正措施_jp", "预防措施/纠正措施"),
            // entity.ipqcdefecthandling.correctiveaction
            new TranslationSeedItem("entity.ipqcdefecthandling.correctiveaction", "zh-CN", "纠正措施", "预防措施/纠正措施"),
            // entity.ipqcdefecthandling.correctiveaction
            new TranslationSeedItem("entity.ipqcdefecthandling.correctiveaction", "zh-HK", "纠正措施_hk", "预防措施/纠正措施"),

            // entity.ipqcdefecthandling.defectimages
            new TranslationSeedItem("entity.ipqcdefecthandling.defectimages", "en-US", "不良图片_us", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.ipqcdefecthandling.defectimages
            new TranslationSeedItem("entity.ipqcdefecthandling.defectimages", "ja-JP", "不良图片_jp", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.ipqcdefecthandling.defectimages
            new TranslationSeedItem("entity.ipqcdefecthandling.defectimages", "zh-CN", "不良图片", "不良图片（JSON格式，存储不良图片URL列表）"),
            // entity.ipqcdefecthandling.defectimages
            new TranslationSeedItem("entity.ipqcdefecthandling.defectimages", "zh-HK", "不良图片_hk", "不良图片（JSON格式，存储不良图片URL列表）"),

            // entity.ipqcdefecthandling.attachments
            new TranslationSeedItem("entity.ipqcdefecthandling.attachments", "en-US", "附件JSON_us", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.ipqcdefecthandling.attachments
            new TranslationSeedItem("entity.ipqcdefecthandling.attachments", "ja-JP", "附件JSON_jp", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.ipqcdefecthandling.attachments
            new TranslationSeedItem("entity.ipqcdefecthandling.attachments", "zh-CN", "附件JSON", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),
            // entity.ipqcdefecthandling.attachments
            new TranslationSeedItem("entity.ipqcdefecthandling.attachments", "zh-HK", "附件JSON_hk", "附件 （JSON列表形式，由TaktFile 统一上传到服务器）"),

            // entity.ipqcdefecthandling.handlingstatus
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingstatus", "en-US", "处理状态_us", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),
            // entity.ipqcdefecthandling.handlingstatus
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingstatus", "ja-JP", "处理状态_jp", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),
            // entity.ipqcdefecthandling.handlingstatus
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingstatus", "zh-CN", "处理状态", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),
            // entity.ipqcdefecthandling.handlingstatus
            new TranslationSeedItem("entity.ipqcdefecthandling.handlingstatus", "zh-HK", "处理状态_hk", "处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）"),

            // entity.ipqcdefecthandling.isobsolete
            new TranslationSeedItem("entity.ipqcdefecthandling.isobsolete", "en-US", "是否作废_us", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ipqcdefecthandling.isobsolete
            new TranslationSeedItem("entity.ipqcdefecthandling.isobsolete", "ja-JP", "是否作废_jp", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ipqcdefecthandling.isobsolete
            new TranslationSeedItem("entity.ipqcdefecthandling.isobsolete", "zh-CN", "是否作废", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),
            // entity.ipqcdefecthandling.isobsolete
            new TranslationSeedItem("entity.ipqcdefecthandling.isobsolete", "zh-HK", "是否作废_hk", "是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）"),

            // entity.ipqcdefecthandling.orderitem
            new TranslationSeedItem("entity.ipqcdefecthandling.orderitem", "en-US", "IPQC检验单明细_us", "IPQC检验单明细（主表）"),
            // entity.ipqcdefecthandling.orderitem
            new TranslationSeedItem("entity.ipqcdefecthandling.orderitem", "ja-JP", "IPQC检验单明细_jp", "IPQC检验单明细（主表）"),
            // entity.ipqcdefecthandling.orderitem
            new TranslationSeedItem("entity.ipqcdefecthandling.orderitem", "zh-CN", "IPQC检验单明细", "IPQC检验单明细（主表）"),
            // entity.ipqcdefecthandling.orderitem
            new TranslationSeedItem("entity.ipqcdefecthandling.orderitem", "zh-HK", "IPQC检验单明细_hk", "IPQC检验单明细（主表）"),
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
        translation.ResourceGroup = "Operation";
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
