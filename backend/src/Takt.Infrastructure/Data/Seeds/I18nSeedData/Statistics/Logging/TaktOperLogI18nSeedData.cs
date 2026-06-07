// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Statistics.Logging
// 文件名称：TaktOperLogI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktOperLog 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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
/// TaktOperLog 实体国际化翻译种子（键前缀 entity.operLog.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktOperLogI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktOperLog 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 operLog 实体翻译...", tenantCode);

        foreach (var item in GetOperLogTranslations())
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

        TaktLogger.Information("TaktOperLog 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktOperLog 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.operLog._self / entity.operLog.{{field}}；ResourceGroup=TaktModule.Statistics；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetOperLogTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.operLog._self
            new TranslationSeedItem("entity.operLog._self", "en-US", "Oper Log Information", "实体名称"),
            // entity.operLog._self
            new TranslationSeedItem("entity.operLog._self", "ja-JP", "操作日志信息", "实体名称"),
            // entity.operLog._self
            new TranslationSeedItem("entity.operLog._self", "zh-CN", "操作日志信息", "实体名称"),
            // entity.operLog._self
            new TranslationSeedItem("entity.operLog._self", "zh-HK", "操作日志信息", "实体名称"),

            // entity.operLog.username
            new TranslationSeedItem("entity.operLog.username", "en-US", "用户名", "用户名（登录账号）"),
            // entity.operLog.username
            new TranslationSeedItem("entity.operLog.username", "ja-JP", "用户名", "用户名（登录账号）"),
            // entity.operLog.username
            new TranslationSeedItem("entity.operLog.username", "zh-CN", "用户名", "用户名（登录账号）"),
            // entity.operLog.username
            new TranslationSeedItem("entity.operLog.username", "zh-HK", "用户名", "用户名（登录账号）"),

            // entity.operLog.opermodule
            new TranslationSeedItem("entity.operLog.opermodule", "en-US", "操作模块", "操作模块（如：用户管理、部门管理）"),
            // entity.operLog.opermodule
            new TranslationSeedItem("entity.operLog.opermodule", "ja-JP", "操作模块", "操作模块（如：用户管理、部门管理）"),
            // entity.operLog.opermodule
            new TranslationSeedItem("entity.operLog.opermodule", "zh-CN", "操作模块", "操作模块（如：用户管理、部门管理）"),
            // entity.operLog.opermodule
            new TranslationSeedItem("entity.operLog.opermodule", "zh-HK", "操作模块", "操作模块（如：用户管理、部门管理）"),

            // entity.operLog.opertype
            new TranslationSeedItem("entity.operLog.opertype", "en-US", "操作类型", "操作类型（如：新增、删除、修改、查询、导出）"),
            // entity.operLog.opertype
            new TranslationSeedItem("entity.operLog.opertype", "ja-JP", "操作类型", "操作类型（如：新增、删除、修改、查询、导出）"),
            // entity.operLog.opertype
            new TranslationSeedItem("entity.operLog.opertype", "zh-CN", "操作类型", "操作类型（如：新增、删除、修改、查询、导出）"),
            // entity.operLog.opertype
            new TranslationSeedItem("entity.operLog.opertype", "zh-HK", "操作类型", "操作类型（如：新增、删除、修改、查询、导出）"),

            // entity.operLog.opermethod
            new TranslationSeedItem("entity.operLog.opermethod", "en-US", "操作方法", "操作方法（如：TaktUserService.CreateUserAsync）"),
            // entity.operLog.opermethod
            new TranslationSeedItem("entity.operLog.opermethod", "ja-JP", "操作方法", "操作方法（如：TaktUserService.CreateUserAsync）"),
            // entity.operLog.opermethod
            new TranslationSeedItem("entity.operLog.opermethod", "zh-CN", "操作方法", "操作方法（如：TaktUserService.CreateUserAsync）"),
            // entity.operLog.opermethod
            new TranslationSeedItem("entity.operLog.opermethod", "zh-HK", "操作方法", "操作方法（如：TaktUserService.CreateUserAsync）"),

            // entity.operLog.requestmethod
            new TranslationSeedItem("entity.operLog.requestmethod", "en-US", "请求方式", "请求方式（GET、POST、PUT、DELETE 等）"),
            // entity.operLog.requestmethod
            new TranslationSeedItem("entity.operLog.requestmethod", "ja-JP", "请求方式", "请求方式（GET、POST、PUT、DELETE 等）"),
            // entity.operLog.requestmethod
            new TranslationSeedItem("entity.operLog.requestmethod", "zh-CN", "请求方式", "请求方式（GET、POST、PUT、DELETE 等）"),
            // entity.operLog.requestmethod
            new TranslationSeedItem("entity.operLog.requestmethod", "zh-HK", "请求方式", "请求方式（GET、POST、PUT、DELETE 等）"),

            // entity.operLog.operurl
            new TranslationSeedItem("entity.operLog.operurl", "en-US", "操作URL", "操作 URL（含查询字符串）"),
            // entity.operLog.operurl
            new TranslationSeedItem("entity.operLog.operurl", "ja-JP", "操作URL", "操作 URL（含查询字符串）"),
            // entity.operLog.operurl
            new TranslationSeedItem("entity.operLog.operurl", "zh-CN", "操作URL", "操作 URL（含查询字符串）"),
            // entity.operLog.operurl
            new TranslationSeedItem("entity.operLog.operurl", "zh-HK", "操作URL", "操作 URL（含查询字符串）"),

            // entity.operLog.requestparam
            new TranslationSeedItem("entity.operLog.requestparam", "en-US", "请求参数", "请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）"),
            // entity.operLog.requestparam
            new TranslationSeedItem("entity.operLog.requestparam", "ja-JP", "请求参数", "请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）"),
            // entity.operLog.requestparam
            new TranslationSeedItem("entity.operLog.requestparam", "zh-CN", "请求参数", "请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）"),
            // entity.operLog.requestparam
            new TranslationSeedItem("entity.operLog.requestparam", "zh-HK", "请求参数", "请求参数 JSON（当前操作入参/操作值完整快照；写入方须脱敏密码、Token 等）"),

            // entity.operLog.jsonresult
            new TranslationSeedItem("entity.operLog.jsonresult", "en-US", "返回结果", "返回结果 JSON（当前操作出参/响应摘要）"),
            // entity.operLog.jsonresult
            new TranslationSeedItem("entity.operLog.jsonresult", "ja-JP", "返回结果", "返回结果 JSON（当前操作出参/响应摘要）"),
            // entity.operLog.jsonresult
            new TranslationSeedItem("entity.operLog.jsonresult", "zh-CN", "返回结果", "返回结果 JSON（当前操作出参/响应摘要）"),
            // entity.operLog.jsonresult
            new TranslationSeedItem("entity.operLog.jsonresult", "zh-HK", "返回结果", "返回结果 JSON（当前操作出参/响应摘要）"),

            // entity.operLog.operstatus
            new TranslationSeedItem("entity.operLog.operstatus", "en-US", "操作状态", "操作状态（0=成功，1=失败）"),
            // entity.operLog.operstatus
            new TranslationSeedItem("entity.operLog.operstatus", "ja-JP", "操作状态", "操作状态（0=成功，1=失败）"),
            // entity.operLog.operstatus
            new TranslationSeedItem("entity.operLog.operstatus", "zh-CN", "操作状态", "操作状态（0=成功，1=失败）"),
            // entity.operLog.operstatus
            new TranslationSeedItem("entity.operLog.operstatus", "zh-HK", "操作状态", "操作状态（0=成功，1=失败）"),

            // entity.operLog.errormsg
            new TranslationSeedItem("entity.operLog.errormsg", "en-US", "错误消息", "错误消息（失败时）"),
            // entity.operLog.errormsg
            new TranslationSeedItem("entity.operLog.errormsg", "ja-JP", "错误消息", "错误消息（失败时）"),
            // entity.operLog.errormsg
            new TranslationSeedItem("entity.operLog.errormsg", "zh-CN", "错误消息", "错误消息（失败时）"),
            // entity.operLog.errormsg
            new TranslationSeedItem("entity.operLog.errormsg", "zh-HK", "错误消息", "错误消息（失败时）"),

            // entity.operLog.operip
            new TranslationSeedItem("entity.operLog.operip", "en-US", "操作IP", "操作 IP"),
            // entity.operLog.operip
            new TranslationSeedItem("entity.operLog.operip", "ja-JP", "操作IP", "操作 IP"),
            // entity.operLog.operip
            new TranslationSeedItem("entity.operLog.operip", "zh-CN", "操作IP", "操作 IP"),
            // entity.operLog.operip
            new TranslationSeedItem("entity.operLog.operip", "zh-HK", "操作IP", "操作 IP"),

            // entity.operLog.operlocation
            new TranslationSeedItem("entity.operLog.operlocation", "en-US", "操作地点", "操作地点（由 <see cref=\"OperIp\"/> 解析，如：中国-广东省-深圳市）"),
            // entity.operLog.operlocation
            new TranslationSeedItem("entity.operLog.operlocation", "ja-JP", "操作地点", "操作地点（由 <see cref=\"OperIp\"/> 解析，如：中国-广东省-深圳市）"),
            // entity.operLog.operlocation
            new TranslationSeedItem("entity.operLog.operlocation", "zh-CN", "操作地点", "操作地点（由 <see cref=\"OperIp\"/> 解析，如：中国-广东省-深圳市）"),
            // entity.operLog.operlocation
            new TranslationSeedItem("entity.operLog.operlocation", "zh-HK", "操作地点", "操作地点（由 <see cref=\"OperIp\"/> 解析，如：中国-广东省-深圳市）"),

            // entity.operLog.opertime
            new TranslationSeedItem("entity.operLog.opertime", "en-US", "操作时间", "操作时间（业务操作发生时刻）"),
            // entity.operLog.opertime
            new TranslationSeedItem("entity.operLog.opertime", "ja-JP", "操作时间", "操作时间（业务操作发生时刻）"),
            // entity.operLog.opertime
            new TranslationSeedItem("entity.operLog.opertime", "zh-CN", "操作时间", "操作时间（业务操作发生时刻）"),
            // entity.operLog.opertime
            new TranslationSeedItem("entity.operLog.opertime", "zh-HK", "操作时间", "操作时间（业务操作发生时刻）"),

            // entity.operLog.elapsedtime
            new TranslationSeedItem("entity.operLog.elapsedtime", "en-US", "执行耗时毫秒", "执行耗时（毫秒）"),
            // entity.operLog.elapsedtime
            new TranslationSeedItem("entity.operLog.elapsedtime", "ja-JP", "执行耗时毫秒", "执行耗时（毫秒）"),
            // entity.operLog.elapsedtime
            new TranslationSeedItem("entity.operLog.elapsedtime", "zh-CN", "执行耗时毫秒", "执行耗时（毫秒）"),
            // entity.operLog.elapsedtime
            new TranslationSeedItem("entity.operLog.elapsedtime", "zh-HK", "执行耗时毫秒", "执行耗时（毫秒）"),
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
