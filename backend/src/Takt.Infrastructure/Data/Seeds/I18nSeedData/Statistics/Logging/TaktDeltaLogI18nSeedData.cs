// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktDeltaLogI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktDeltaLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging;

/// <summary>
/// TaktDeltaLog 实体国际化翻译种子（键前缀 entity.deltaLog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktDeltaLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktDeltaLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 deltaLog 实体翻译...", tenantCode);

        foreach (var item in GetDeltaLogTranslations())
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

        TaktLogger.Information("TaktDeltaLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktDeltaLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.deltaLog._self / entity.deltaLog.{{field}}；ResourceGroup=TaktModule.Statistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetDeltaLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.deltaLog._self
            new TranslationSeedItem("entity.deltaLog._self", "en-US", "Delta Log Information", "实体名称"),
            // entity.deltaLog._self
            new TranslationSeedItem("entity.deltaLog._self", "ja-JP", "差异日志信息", "实体名称"),
            // entity.deltaLog._self
            new TranslationSeedItem("entity.deltaLog._self", "zh-CN", "差异日志信息", "实体名称"),
            // entity.deltaLog._self
            new TranslationSeedItem("entity.deltaLog._self", "zh-HK", "差异日志信息", "实体名称"),

            // entity.deltaLog.username
            new TranslationSeedItem("entity.deltaLog.username", "en-US", "用户名", "用户名（登录账号）"),
            // entity.deltaLog.username
            new TranslationSeedItem("entity.deltaLog.username", "ja-JP", "用户名", "用户名（登录账号）"),
            // entity.deltaLog.username
            new TranslationSeedItem("entity.deltaLog.username", "zh-CN", "用户名", "用户名（登录账号）"),
            // entity.deltaLog.username
            new TranslationSeedItem("entity.deltaLog.username", "zh-HK", "用户名", "用户名（登录账号）"),

            // entity.deltaLog.opertype
            new TranslationSeedItem("entity.deltaLog.opertype", "en-US", "操作类型", "操作类型（INSERT、UPDATE、DELETE）"),
            // entity.deltaLog.opertype
            new TranslationSeedItem("entity.deltaLog.opertype", "ja-JP", "操作类型", "操作类型（INSERT、UPDATE、DELETE）"),
            // entity.deltaLog.opertype
            new TranslationSeedItem("entity.deltaLog.opertype", "zh-CN", "操作类型", "操作类型（INSERT、UPDATE、DELETE）"),
            // entity.deltaLog.opertype
            new TranslationSeedItem("entity.deltaLog.opertype", "zh-HK", "操作类型", "操作类型（INSERT、UPDATE、DELETE）"),

            // entity.deltaLog.tablename
            new TranslationSeedItem("entity.deltaLog.tablename", "en-US", "表名", "数据库表名（SugarTable 物理表名）"),
            // entity.deltaLog.tablename
            new TranslationSeedItem("entity.deltaLog.tablename", "ja-JP", "表名", "数据库表名（SugarTable 物理表名）"),
            // entity.deltaLog.tablename
            new TranslationSeedItem("entity.deltaLog.tablename", "zh-CN", "表名", "数据库表名（SugarTable 物理表名）"),
            // entity.deltaLog.tablename
            new TranslationSeedItem("entity.deltaLog.tablename", "zh-HK", "表名", "数据库表名（SugarTable 物理表名）"),

            // entity.deltaLog.primarykeyid
            new TranslationSeedItem("entity.deltaLog.primarykeyid", "en-US", "主键ID", "业务主键 ID"),
            // entity.deltaLog.primarykeyid
            new TranslationSeedItem("entity.deltaLog.primarykeyid", "ja-JP", "主键ID", "业务主键 ID"),
            // entity.deltaLog.primarykeyid
            new TranslationSeedItem("entity.deltaLog.primarykeyid", "zh-CN", "主键ID", "业务主键 ID"),
            // entity.deltaLog.primarykeyid
            new TranslationSeedItem("entity.deltaLog.primarykeyid", "zh-HK", "主键ID", "业务主键 ID"),

            // entity.deltaLog.beforedata
            new TranslationSeedItem("entity.deltaLog.beforedata", "en-US", "修改前数据", "修改前数据 JSON（旧值快照）"),
            // entity.deltaLog.beforedata
            new TranslationSeedItem("entity.deltaLog.beforedata", "ja-JP", "修改前数据", "修改前数据 JSON（旧值快照）"),
            // entity.deltaLog.beforedata
            new TranslationSeedItem("entity.deltaLog.beforedata", "zh-CN", "修改前数据", "修改前数据 JSON（旧值快照）"),
            // entity.deltaLog.beforedata
            new TranslationSeedItem("entity.deltaLog.beforedata", "zh-HK", "修改前数据", "修改前数据 JSON（旧值快照）"),

            // entity.deltaLog.afterdata
            new TranslationSeedItem("entity.deltaLog.afterdata", "en-US", "修改后数据", "修改后数据 JSON（新值快照）"),
            // entity.deltaLog.afterdata
            new TranslationSeedItem("entity.deltaLog.afterdata", "ja-JP", "修改后数据", "修改后数据 JSON（新值快照）"),
            // entity.deltaLog.afterdata
            new TranslationSeedItem("entity.deltaLog.afterdata", "zh-CN", "修改后数据", "修改后数据 JSON（新值快照）"),
            // entity.deltaLog.afterdata
            new TranslationSeedItem("entity.deltaLog.afterdata", "zh-HK", "修改后数据", "修改后数据 JSON（新值快照）"),

            // entity.deltaLog.diffdata
            new TranslationSeedItem("entity.deltaLog.diffdata", "en-US", "差异内容", "差异内容 JSON（变更字段及旧/新值明细）"),
            // entity.deltaLog.diffdata
            new TranslationSeedItem("entity.deltaLog.diffdata", "ja-JP", "差异内容", "差异内容 JSON（变更字段及旧/新值明细）"),
            // entity.deltaLog.diffdata
            new TranslationSeedItem("entity.deltaLog.diffdata", "zh-CN", "差异内容", "差异内容 JSON（变更字段及旧/新值明细）"),
            // entity.deltaLog.diffdata
            new TranslationSeedItem("entity.deltaLog.diffdata", "zh-HK", "差异内容", "差异内容 JSON（变更字段及旧/新值明细）"),

            // entity.deltaLog.sqlstatement
            new TranslationSeedItem("entity.deltaLog.sqlstatement", "en-US", "SQL语句", "执行的 SQL 语句（AOP 捕获，可选）"),
            // entity.deltaLog.sqlstatement
            new TranslationSeedItem("entity.deltaLog.sqlstatement", "ja-JP", "SQL语句", "执行的 SQL 语句（AOP 捕获，可选）"),
            // entity.deltaLog.sqlstatement
            new TranslationSeedItem("entity.deltaLog.sqlstatement", "zh-CN", "SQL语句", "执行的 SQL 语句（AOP 捕获，可选）"),
            // entity.deltaLog.sqlstatement
            new TranslationSeedItem("entity.deltaLog.sqlstatement", "zh-HK", "SQL语句", "执行的 SQL 语句（AOP 捕获，可选）"),

            // entity.deltaLog.operip
            new TranslationSeedItem("entity.deltaLog.operip", "en-US", "操作IP", "操作 IP"),
            // entity.deltaLog.operip
            new TranslationSeedItem("entity.deltaLog.operip", "ja-JP", "操作IP", "操作 IP"),
            // entity.deltaLog.operip
            new TranslationSeedItem("entity.deltaLog.operip", "zh-CN", "操作IP", "操作 IP"),
            // entity.deltaLog.operip
            new TranslationSeedItem("entity.deltaLog.operip", "zh-HK", "操作IP", "操作 IP"),

            // entity.deltaLog.operlocation
            new TranslationSeedItem("entity.deltaLog.operlocation", "en-US", "操作地点", "操作地点（由 <see cref=\"OperIp\"/> 解析，如：中国-广东省-深圳市）"),
            // entity.deltaLog.operlocation
            new TranslationSeedItem("entity.deltaLog.operlocation", "ja-JP", "操作地点", "操作地点（由 <see cref=\"OperIp\"/> 解析，如：中国-广东省-深圳市）"),
            // entity.deltaLog.operlocation
            new TranslationSeedItem("entity.deltaLog.operlocation", "zh-CN", "操作地点", "操作地点（由 <see cref=\"OperIp\"/> 解析，如：中国-广东省-深圳市）"),
            // entity.deltaLog.operlocation
            new TranslationSeedItem("entity.deltaLog.operlocation", "zh-HK", "操作地点", "操作地点（由 <see cref=\"OperIp\"/> 解析，如：中国-广东省-深圳市）"),

            // entity.deltaLog.opertime
            new TranslationSeedItem("entity.deltaLog.opertime", "en-US", "操作时间", "操作时间（数据变更发生时刻）"),
            // entity.deltaLog.opertime
            new TranslationSeedItem("entity.deltaLog.opertime", "ja-JP", "操作时间", "操作时间（数据变更发生时刻）"),
            // entity.deltaLog.opertime
            new TranslationSeedItem("entity.deltaLog.opertime", "zh-CN", "操作时间", "操作时间（数据变更发生时刻）"),
            // entity.deltaLog.opertime
            new TranslationSeedItem("entity.deltaLog.opertime", "zh-HK", "操作时间", "操作时间（数据变更发生时刻）"),

            // entity.deltaLog.elapsedtime
            new TranslationSeedItem("entity.deltaLog.elapsedtime", "en-US", "执行耗时毫秒", "执行耗时（毫秒）"),
            // entity.deltaLog.elapsedtime
            new TranslationSeedItem("entity.deltaLog.elapsedtime", "ja-JP", "执行耗时毫秒", "执行耗时（毫秒）"),
            // entity.deltaLog.elapsedtime
            new TranslationSeedItem("entity.deltaLog.elapsedtime", "zh-CN", "执行耗时毫秒", "执行耗时（毫秒）"),
            // entity.deltaLog.elapsedtime
            new TranslationSeedItem("entity.deltaLog.elapsedtime", "zh-HK", "执行耗时毫秒", "执行耗时（毫秒）"),
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
        translation.ResourceGroup = TaktModule.Statistics;
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
