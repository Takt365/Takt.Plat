// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel
// 文件名称：TaktEmployeeAttachmentI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktEmployeeAttachment 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.HumanResource.Personnel;

/// <summary>
/// TaktEmployeeAttachment 实体国际化翻译种子（键前缀 entity.employeeattachment.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktEmployeeAttachmentI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktEmployeeAttachment 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 employeeattachment 实体翻译...", tenantCode);

        foreach (var item in GetEmployeeAttachmentTranslations())
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

        TaktLogger.Information("TaktEmployeeAttachment 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktEmployeeAttachment 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.employeeattachment._self / entity.employeeattachment.{{field}}；ResourceGroup=Personnel；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetEmployeeAttachmentTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.employeeattachment._self
            new TranslationSeedItem("entity.employeeattachment._self", "en-US", "Employee Attachment Information_us", "实体名称"),
            // entity.employeeattachment._self
            new TranslationSeedItem("entity.employeeattachment._self", "ja-JP", "员工档案附件信息_jp", "实体名称"),
            // entity.employeeattachment._self
            new TranslationSeedItem("entity.employeeattachment._self", "zh-CN", "员工档案附件信息", "实体名称"),
            // entity.employeeattachment._self
            new TranslationSeedItem("entity.employeeattachment._self", "zh-HK", "员工档案附件信息_hk", "实体名称"),

            // entity.employeeattachment.employeeid
            new TranslationSeedItem("entity.employeeattachment.employeeid", "en-US", "员工ID_us", "员工ID"),
            // entity.employeeattachment.employeeid
            new TranslationSeedItem("entity.employeeattachment.employeeid", "ja-JP", "员工ID_jp", "员工ID"),
            // entity.employeeattachment.employeeid
            new TranslationSeedItem("entity.employeeattachment.employeeid", "zh-CN", "员工ID", "员工ID"),
            // entity.employeeattachment.employeeid
            new TranslationSeedItem("entity.employeeattachment.employeeid", "zh-HK", "员工ID_hk", "员工ID"),

            // entity.employeeattachment.fileid
            new TranslationSeedItem("entity.employeeattachment.fileid", "en-US", "文件ID_us", "文件ID（关联文件服务）"),
            // entity.employeeattachment.fileid
            new TranslationSeedItem("entity.employeeattachment.fileid", "ja-JP", "文件ID_jp", "文件ID（关联文件服务）"),
            // entity.employeeattachment.fileid
            new TranslationSeedItem("entity.employeeattachment.fileid", "zh-CN", "文件ID", "文件ID（关联文件服务）"),
            // entity.employeeattachment.fileid
            new TranslationSeedItem("entity.employeeattachment.fileid", "zh-HK", "文件ID_hk", "文件ID（关联文件服务）"),

            // entity.employeeattachment.filecode
            new TranslationSeedItem("entity.employeeattachment.filecode", "en-US", "文件编码_us", "文件编码"),
            // entity.employeeattachment.filecode
            new TranslationSeedItem("entity.employeeattachment.filecode", "ja-JP", "文件编码_jp", "文件编码"),
            // entity.employeeattachment.filecode
            new TranslationSeedItem("entity.employeeattachment.filecode", "zh-CN", "文件编码", "文件编码"),
            // entity.employeeattachment.filecode
            new TranslationSeedItem("entity.employeeattachment.filecode", "zh-HK", "文件编码_hk", "文件编码"),

            // entity.employeeattachment.filename
            new TranslationSeedItem("entity.employeeattachment.filename", "en-US", "文件名称_us", "文件名称"),
            // entity.employeeattachment.filename
            new TranslationSeedItem("entity.employeeattachment.filename", "ja-JP", "文件名称_jp", "文件名称"),
            // entity.employeeattachment.filename
            new TranslationSeedItem("entity.employeeattachment.filename", "zh-CN", "文件名称", "文件名称"),
            // entity.employeeattachment.filename
            new TranslationSeedItem("entity.employeeattachment.filename", "zh-HK", "文件名称_hk", "文件名称"),

            // entity.employeeattachment.filepath
            new TranslationSeedItem("entity.employeeattachment.filepath", "en-US", "文件路径_us", "文件路径"),
            // entity.employeeattachment.filepath
            new TranslationSeedItem("entity.employeeattachment.filepath", "ja-JP", "文件路径_jp", "文件路径"),
            // entity.employeeattachment.filepath
            new TranslationSeedItem("entity.employeeattachment.filepath", "zh-CN", "文件路径", "文件路径"),
            // entity.employeeattachment.filepath
            new TranslationSeedItem("entity.employeeattachment.filepath", "zh-HK", "文件路径_hk", "文件路径"),

            // entity.employeeattachment.filesize
            new TranslationSeedItem("entity.employeeattachment.filesize", "en-US", "文件大小_us", "文件大小（字节）"),
            // entity.employeeattachment.filesize
            new TranslationSeedItem("entity.employeeattachment.filesize", "ja-JP", "文件大小_jp", "文件大小（字节）"),
            // entity.employeeattachment.filesize
            new TranslationSeedItem("entity.employeeattachment.filesize", "zh-CN", "文件大小", "文件大小（字节）"),
            // entity.employeeattachment.filesize
            new TranslationSeedItem("entity.employeeattachment.filesize", "zh-HK", "文件大小_hk", "文件大小（字节）"),

            // entity.employeeattachment.filetype
            new TranslationSeedItem("entity.employeeattachment.filetype", "en-US", "文件类型_us", "文件类型/MIME"),
            // entity.employeeattachment.filetype
            new TranslationSeedItem("entity.employeeattachment.filetype", "ja-JP", "文件类型_jp", "文件类型/MIME"),
            // entity.employeeattachment.filetype
            new TranslationSeedItem("entity.employeeattachment.filetype", "zh-CN", "文件类型", "文件类型/MIME"),
            // entity.employeeattachment.filetype
            new TranslationSeedItem("entity.employeeattachment.filetype", "zh-HK", "文件类型_hk", "文件类型/MIME"),

            // entity.employeeattachment.attachmenttype
            new TranslationSeedItem("entity.employeeattachment.attachmenttype", "en-US", "附件类型_us", "附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）"),
            // entity.employeeattachment.attachmenttype
            new TranslationSeedItem("entity.employeeattachment.attachmenttype", "ja-JP", "附件类型_jp", "附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）"),
            // entity.employeeattachment.attachmenttype
            new TranslationSeedItem("entity.employeeattachment.attachmenttype", "zh-CN", "附件类型", "附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）"),
            // entity.employeeattachment.attachmenttype
            new TranslationSeedItem("entity.employeeattachment.attachmenttype", "zh-HK", "附件类型_hk", "附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）"),

            // entity.employeeattachment.attachmentdescription
            new TranslationSeedItem("entity.employeeattachment.attachmentdescription", "en-US", "附件说明_us", "附件说明"),
            // entity.employeeattachment.attachmentdescription
            new TranslationSeedItem("entity.employeeattachment.attachmentdescription", "ja-JP", "附件说明_jp", "附件说明"),
            // entity.employeeattachment.attachmentdescription
            new TranslationSeedItem("entity.employeeattachment.attachmentdescription", "zh-CN", "附件说明", "附件说明"),
            // entity.employeeattachment.attachmentdescription
            new TranslationSeedItem("entity.employeeattachment.attachmentdescription", "zh-HK", "附件说明_hk", "附件说明"),

            // entity.employeeattachment.sortorder
            new TranslationSeedItem("entity.employeeattachment.sortorder", "en-US", "排序号_us", "排序号"),
            // entity.employeeattachment.sortorder
            new TranslationSeedItem("entity.employeeattachment.sortorder", "ja-JP", "排序号_jp", "排序号"),
            // entity.employeeattachment.sortorder
            new TranslationSeedItem("entity.employeeattachment.sortorder", "zh-CN", "排序号", "排序号"),
            // entity.employeeattachment.sortorder
            new TranslationSeedItem("entity.employeeattachment.sortorder", "zh-HK", "排序号_hk", "排序号"),
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
        translation.ResourceGroup = "Personnel";
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
