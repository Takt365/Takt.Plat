// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output
// 文件名称：TaktProductionTeamI18nSeedData.cs
// 创建时间：2026-06-08
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
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Logistics.Manufacturing.Output;

/// <summary>
/// TaktProductionTeam 实体国际化翻译种子（键前缀 entity.productionTeam.*）
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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 productionTeam 实体翻译...", tenantCode);

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
    /// I18nKey：entity.productionTeam._self / entity.productionTeam.{{field}}；ResourceGroup=TaktModule.Logistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetProductionTeamTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.productionTeam._self
            new TranslationSeedItem("entity.productionTeam._self", "en-US", "Production Team Information", "实体名称"),
            // entity.productionTeam._self
            new TranslationSeedItem("entity.productionTeam._self", "ja-JP", "生产班组信息", "实体名称"),
            // entity.productionTeam._self
            new TranslationSeedItem("entity.productionTeam._self", "zh-CN", "生产班组信息", "实体名称"),
            // entity.productionTeam._self
            new TranslationSeedItem("entity.productionTeam._self", "zh-HK", "生产班组信息", "实体名称"),

            // entity.productionTeam.plantcode
            new TranslationSeedItem("entity.productionTeam.plantcode", "en-US", "工厂代码", "工厂代码"),
            // entity.productionTeam.plantcode
            new TranslationSeedItem("entity.productionTeam.plantcode", "ja-JP", "工厂代码", "工厂代码"),
            // entity.productionTeam.plantcode
            new TranslationSeedItem("entity.productionTeam.plantcode", "zh-CN", "工厂代码", "工厂代码"),
            // entity.productionTeam.plantcode
            new TranslationSeedItem("entity.productionTeam.plantcode", "zh-HK", "工厂代码", "工厂代码"),

            // entity.productionTeam.teamcode
            new TranslationSeedItem("entity.productionTeam.teamcode", "en-US", "班组编码", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）"),
            // entity.productionTeam.teamcode
            new TranslationSeedItem("entity.productionTeam.teamcode", "ja-JP", "班组编码", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）"),
            // entity.productionTeam.teamcode
            new TranslationSeedItem("entity.productionTeam.teamcode", "zh-CN", "班组编码", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）"),
            // entity.productionTeam.teamcode
            new TranslationSeedItem("entity.productionTeam.teamcode", "zh-HK", "班组编码", "班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）"),

            // entity.productionTeam.teamname
            new TranslationSeedItem("entity.productionTeam.teamname", "en-US", "班组名称", "班组名称（显示名称，如：SMT一班、手插二班等）"),
            // entity.productionTeam.teamname
            new TranslationSeedItem("entity.productionTeam.teamname", "ja-JP", "班组名称", "班组名称（显示名称，如：SMT一班、手插二班等）"),
            // entity.productionTeam.teamname
            new TranslationSeedItem("entity.productionTeam.teamname", "zh-CN", "班组名称", "班组名称（显示名称，如：SMT一班、手插二班等）"),
            // entity.productionTeam.teamname
            new TranslationSeedItem("entity.productionTeam.teamname", "zh-HK", "班组名称", "班组名称（显示名称，如：SMT一班、手插二班等）"),

            // entity.productionTeam.teamcategory
            new TranslationSeedItem("entity.productionTeam.teamcategory", "en-US", "班组分类编码", "班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）"),
            // entity.productionTeam.teamcategory
            new TranslationSeedItem("entity.productionTeam.teamcategory", "ja-JP", "班组分类编码", "班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）"),
            // entity.productionTeam.teamcategory
            new TranslationSeedItem("entity.productionTeam.teamcategory", "zh-CN", "班组分类编码", "班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）"),
            // entity.productionTeam.teamcategory
            new TranslationSeedItem("entity.productionTeam.teamcategory", "zh-HK", "班组分类编码", "班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）"),

            // entity.productionTeam.teamcategoryname
            new TranslationSeedItem("entity.productionTeam.teamcategoryname", "en-US", "班组分类名称", "班组分类名称（如：组立、PCBA、SMT、质检等）"),
            // entity.productionTeam.teamcategoryname
            new TranslationSeedItem("entity.productionTeam.teamcategoryname", "ja-JP", "班组分类名称", "班组分类名称（如：组立、PCBA、SMT、质检等）"),
            // entity.productionTeam.teamcategoryname
            new TranslationSeedItem("entity.productionTeam.teamcategoryname", "zh-CN", "班组分类名称", "班组分类名称（如：组立、PCBA、SMT、质检等）"),
            // entity.productionTeam.teamcategoryname
            new TranslationSeedItem("entity.productionTeam.teamcategoryname", "zh-HK", "班组分类名称", "班组分类名称（如：组立、PCBA、SMT、质检等）"),

            // entity.productionTeam.productionline
            new TranslationSeedItem("entity.productionTeam.productionline", "en-US", "生产线代码", "生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）"),
            // entity.productionTeam.productionline
            new TranslationSeedItem("entity.productionTeam.productionline", "ja-JP", "生产线代码", "生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）"),
            // entity.productionTeam.productionline
            new TranslationSeedItem("entity.productionTeam.productionline", "zh-CN", "生产线代码", "生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）"),
            // entity.productionTeam.productionline
            new TranslationSeedItem("entity.productionTeam.productionline", "zh-HK", "生产线代码", "生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）"),

            // entity.productionTeam.teamleaderid
            new TranslationSeedItem("entity.productionTeam.teamleaderid", "en-US", "班组长员工Id", "班组长员工Id"),
            // entity.productionTeam.teamleaderid
            new TranslationSeedItem("entity.productionTeam.teamleaderid", "ja-JP", "班组长员工Id", "班组长员工Id"),
            // entity.productionTeam.teamleaderid
            new TranslationSeedItem("entity.productionTeam.teamleaderid", "zh-CN", "班组长员工Id", "班组长员工Id"),
            // entity.productionTeam.teamleaderid
            new TranslationSeedItem("entity.productionTeam.teamleaderid", "zh-HK", "班组长员工Id", "班组长员工Id"),

            // entity.productionTeam.teamleadername
            new TranslationSeedItem("entity.productionTeam.teamleadername", "en-US", "班组长姓名", "班组长姓名"),
            // entity.productionTeam.teamleadername
            new TranslationSeedItem("entity.productionTeam.teamleadername", "ja-JP", "班组长姓名", "班组长姓名"),
            // entity.productionTeam.teamleadername
            new TranslationSeedItem("entity.productionTeam.teamleadername", "zh-CN", "班组长姓名", "班组长姓名"),
            // entity.productionTeam.teamleadername
            new TranslationSeedItem("entity.productionTeam.teamleadername", "zh-HK", "班组长姓名", "班组长姓名"),

            // entity.productionTeam.shiftno
            new TranslationSeedItem("entity.productionTeam.shiftno", "en-US", "班次", "班次（1=早班，2=中班，3=晚班）"),
            // entity.productionTeam.shiftno
            new TranslationSeedItem("entity.productionTeam.shiftno", "ja-JP", "班次", "班次（1=早班，2=中班，3=晚班）"),
            // entity.productionTeam.shiftno
            new TranslationSeedItem("entity.productionTeam.shiftno", "zh-CN", "班次", "班次（1=早班，2=中班，3=晚班）"),
            // entity.productionTeam.shiftno
            new TranslationSeedItem("entity.productionTeam.shiftno", "zh-HK", "班次", "班次（1=早班，2=中班，3=晚班）"),

            // entity.productionTeam.status
            new TranslationSeedItem("entity.productionTeam.status", "en-US", "启用状态", "启用状态（1=启用，0=禁用）"),
            // entity.productionTeam.status
            new TranslationSeedItem("entity.productionTeam.status", "ja-JP", "启用状态", "启用状态（1=启用，0=禁用）"),
            // entity.productionTeam.status
            new TranslationSeedItem("entity.productionTeam.status", "zh-CN", "启用状态", "启用状态（1=启用，0=禁用）"),
            // entity.productionTeam.status
            new TranslationSeedItem("entity.productionTeam.status", "zh-HK", "启用状态", "启用状态（1=启用，0=禁用）"),
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
