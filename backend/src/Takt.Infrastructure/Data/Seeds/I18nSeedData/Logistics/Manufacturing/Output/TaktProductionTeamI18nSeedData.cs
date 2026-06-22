// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktProductionTeamI18nSeedData.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktProductionTeam 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktProductionTeam 实体国际化翻译种子（键前缀 entity.productionteam.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktProductionTeamI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktProductionTeam 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionteam 实体翻译...", tenantCode);

        foreach (var item in GetProductionTeamTranslations())
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

        TaktLogger.Information("TaktProductionTeam 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktProductionTeam 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.productionteam._self / entity.productionteam.{{field}}；ResourceGroup=Output；ResourceType=frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionTeamTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionteam._self
            new TranslationSeedItem("entity.productionteam._self", "en-US", "Production Team Information_us", "实体名称"),
            // entity.productionteam._self
            new TranslationSeedItem("entity.productionteam._self", "ja-JP", "生产班组信息_jp", "实体名称"),
            // entity.productionteam._self
            new TranslationSeedItem("entity.productionteam._self", "zh-CN", "生产班组信息", "实体名称"),
            // entity.productionteam._self
            new TranslationSeedItem("entity.productionteam._self", "zh-HK", "生产班组信息_hk", "实体名称"),

            // entity.productionteam.plantcode
            new TranslationSeedItem("entity.productionteam.plantcode", "en-US", "工厂代码_us", "工厂代码"),
            // entity.productionteam.plantcode
            new TranslationSeedItem("entity.productionteam.plantcode", "ja-JP", "工厂代码_jp", "工厂代码"),
            // entity.productionteam.plantcode
            new TranslationSeedItem("entity.productionteam.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.productionteam.plantcode
            new TranslationSeedItem("entity.productionteam.plantcode", "zh-HK", "工厂代码_hk", "工厂代码"),

            // entity.productionteam.teamcode
            new TranslationSeedItem("entity.productionteam.teamcode", "en-US", "班组编码_us", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）"),
            // entity.productionteam.teamcode
            new TranslationSeedItem("entity.productionteam.teamcode", "ja-JP", "班组编码_jp", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）"),
            // entity.productionteam.teamcode
            new TranslationSeedItem("entity.productionteam.teamcode", "zh-CN", "班组编码", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）"),
            // entity.productionteam.teamcode
            new TranslationSeedItem("entity.productionteam.teamcode", "zh-HK", "班组编码_hk", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）"),

            // entity.productionteam.teamname
            new TranslationSeedItem("entity.productionteam.teamname", "en-US", "班组名称_us", "班组名称（显示名称，如：SMT一班、手插二班等）"),
            // entity.productionteam.teamname
            new TranslationSeedItem("entity.productionteam.teamname", "ja-JP", "班组名称_jp", "班组名称（显示名称，如：SMT一班、手插二班等）"),
            // entity.productionteam.teamname
            new TranslationSeedItem("entity.productionteam.teamname", "zh-CN", "班组名称", "班组名称（显示名称，如：SMT一班、手插二班等）"),
            // entity.productionteam.teamname
            new TranslationSeedItem("entity.productionteam.teamname", "zh-HK", "班组名称_hk", "班组名称（显示名称，如：SMT一班、手插二班等）"),

            // entity.productionteam.teamcategory
            new TranslationSeedItem("entity.productionteam.teamcategory", "en-US", "班组分类编码_us", "班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）"),
            // entity.productionteam.teamcategory
            new TranslationSeedItem("entity.productionteam.teamcategory", "ja-JP", "班组分类编码_jp", "班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）"),
            // entity.productionteam.teamcategory
            new TranslationSeedItem("entity.productionteam.teamcategory", "zh-CN", "班组分类编码", "班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）"),
            // entity.productionteam.teamcategory
            new TranslationSeedItem("entity.productionteam.teamcategory", "zh-HK", "班组分类编码_hk", "班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）"),

            // entity.productionteam.teamcategoryname
            new TranslationSeedItem("entity.productionteam.teamcategoryname", "en-US", "班组分类名称_us", "班组分类名称（如：组立、PCBA、SMT、质检等）"),
            // entity.productionteam.teamcategoryname
            new TranslationSeedItem("entity.productionteam.teamcategoryname", "ja-JP", "班组分类名称_jp", "班组分类名称（如：组立、PCBA、SMT、质检等）"),
            // entity.productionteam.teamcategoryname
            new TranslationSeedItem("entity.productionteam.teamcategoryname", "zh-CN", "班组分类名称", "班组分类名称（如：组立、PCBA、SMT、质检等）"),
            // entity.productionteam.teamcategoryname
            new TranslationSeedItem("entity.productionteam.teamcategoryname", "zh-HK", "班组分类名称_hk", "班组分类名称（如：组立、PCBA、SMT、质检等）"),

            // entity.productionteam.productionline
            new TranslationSeedItem("entity.productionteam.productionline", "en-US", "生产线代码_us", "生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）"),
            // entity.productionteam.productionline
            new TranslationSeedItem("entity.productionteam.productionline", "ja-JP", "生产线代码_jp", "生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）"),
            // entity.productionteam.productionline
            new TranslationSeedItem("entity.productionteam.productionline", "zh-CN", "生产线代码", "生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）"),
            // entity.productionteam.productionline
            new TranslationSeedItem("entity.productionteam.productionline", "zh-HK", "生产线代码_hk", "生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）"),

            // entity.productionteam.teamleaderid
            new TranslationSeedItem("entity.productionteam.teamleaderid", "en-US", "班组长员工Id_us", "班组长员工Id"),
            // entity.productionteam.teamleaderid
            new TranslationSeedItem("entity.productionteam.teamleaderid", "ja-JP", "班组长员工Id_jp", "班组长员工Id"),
            // entity.productionteam.teamleaderid
            new TranslationSeedItem("entity.productionteam.teamleaderid", "zh-CN", "班组长员工Id", "班组长员工Id"),
            // entity.productionteam.teamleaderid
            new TranslationSeedItem("entity.productionteam.teamleaderid", "zh-HK", "班组长员工Id_hk", "班组长员工Id"),

            // entity.productionteam.teamleadername
            new TranslationSeedItem("entity.productionteam.teamleadername", "en-US", "班组长姓名_us", "班组长姓名"),
            // entity.productionteam.teamleadername
            new TranslationSeedItem("entity.productionteam.teamleadername", "ja-JP", "班组长姓名_jp", "班组长姓名"),
            // entity.productionteam.teamleadername
            new TranslationSeedItem("entity.productionteam.teamleadername", "zh-CN", "班组长姓名", "班组长姓名"),
            // entity.productionteam.teamleadername
            new TranslationSeedItem("entity.productionteam.teamleadername", "zh-HK", "班组长姓名_hk", "班组长姓名"),

            // entity.productionteam.shiftno
            new TranslationSeedItem("entity.productionteam.shiftno", "en-US", "班次_us", "班次（1=早班，2=中班，3=晚班）"),
            // entity.productionteam.shiftno
            new TranslationSeedItem("entity.productionteam.shiftno", "ja-JP", "班次_jp", "班次（1=早班，2=中班，3=晚班）"),
            // entity.productionteam.shiftno
            new TranslationSeedItem("entity.productionteam.shiftno", "zh-CN", "班次", "班次（1=早班，2=中班，3=晚班）"),
            // entity.productionteam.shiftno
            new TranslationSeedItem("entity.productionteam.shiftno", "zh-HK", "班次_hk", "班次（1=早班，2=中班，3=晚班）"),

            // entity.productionteam.status
            new TranslationSeedItem("entity.productionteam.status", "en-US", "启用状态_us", "启用状态（1=启用，0=禁用）"),
            // entity.productionteam.status
            new TranslationSeedItem("entity.productionteam.status", "ja-JP", "启用状态_jp", "启用状态（1=启用，0=禁用）"),
            // entity.productionteam.status
            new TranslationSeedItem("entity.productionteam.status", "zh-CN", "启用状态", "启用状态（1=启用，0=禁用）"),
            // entity.productionteam.status
            new TranslationSeedItem("entity.productionteam.status", "zh-HK", "启用状态_hk", "启用状态（1=启用，0=禁用）"),
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
        translation.ResourceGroup = "Output";
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
