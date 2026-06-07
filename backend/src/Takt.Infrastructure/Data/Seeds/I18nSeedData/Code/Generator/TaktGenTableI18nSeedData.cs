// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Generator
// 文件名称：TaktGenTableI18nSeedData.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：TaktGenTable 实体字段国际化种子（无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary）
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

namespace Takt.Infrastructure.Data.Seeds.I18nSeedData.Code.Generator;

/// <summary>
/// TaktGenTable 实体国际化翻译种子（键前缀 entity.genTable.*）
/// 幂等性：存在则更新，不存在则创建
/// </summary>
public class TaktGenTableI18nSeedData : ITaktSeedDataCoordinator
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
        TaktLogger.Information("开始初始化 TaktGenTable 实体国际化翻译种子...");

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

        TaktLogger.Information("正在为租户 {TenantCode} 初始化 genTable 实体翻译...", tenantCode);

        foreach (var item in GetGenTableTranslations())
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

        TaktLogger.Information("TaktGenTable 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// TaktGenTable 实体翻译列表（en-US / ja-JP / zh-CN / zh-HK）
    /// I18nKey：entity.genTable._self / entity.genTable.{{field}}；ResourceGroup=TaktModule.Code；ResourceType=TaktAppSide.Frontend
    /// </summary>
    private static List<TranslationSeedItem> GetGenTableTranslations()
    {
        return new List<TranslationSeedItem>
        {
            // entity.genTable._self
            new TranslationSeedItem("entity.genTable._self", "en-US", "Gen Table Information", "实体名称"),
            // entity.genTable._self
            new TranslationSeedItem("entity.genTable._self", "ja-JP", "Takt代码生成表配置信息", "实体名称"),
            // entity.genTable._self
            new TranslationSeedItem("entity.genTable._self", "zh-CN", "Takt代码生成表配置信息", "实体名称"),
            // entity.genTable._self
            new TranslationSeedItem("entity.genTable._self", "zh-HK", "Takt代码生成表配置信息", "实体名称"),

            // entity.genTable.datasource
            new TranslationSeedItem("entity.genTable.datasource", "en-US", "数据源", "数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）"),
            // entity.genTable.datasource
            new TranslationSeedItem("entity.genTable.datasource", "ja-JP", "数据源", "数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）"),
            // entity.genTable.datasource
            new TranslationSeedItem("entity.genTable.datasource", "zh-CN", "数据源", "数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）"),
            // entity.genTable.datasource
            new TranslationSeedItem("entity.genTable.datasource", "zh-HK", "数据源", "数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000，不可空）"),

            // entity.genTable.tablename
            new TranslationSeedItem("entity.genTable.tablename", "en-US", "表名称", "数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）"),
            // entity.genTable.tablename
            new TranslationSeedItem("entity.genTable.tablename", "ja-JP", "表名称", "数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）"),
            // entity.genTable.tablename
            new TranslationSeedItem("entity.genTable.tablename", "zh-CN", "表名称", "数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）"),
            // entity.genTable.tablename
            new TranslationSeedItem("entity.genTable.tablename", "zh-HK", "表名称", "数据表名称（唯一索引：租户内数据源+表名唯一，见 ix_gen_table_datasource_table_unique）"),

            // entity.genTable.tablecomment
            new TranslationSeedItem("entity.genTable.tablecomment", "en-US", "表描述", "表描述（表注释）"),
            // entity.genTable.tablecomment
            new TranslationSeedItem("entity.genTable.tablecomment", "ja-JP", "表描述", "表描述（表注释）"),
            // entity.genTable.tablecomment
            new TranslationSeedItem("entity.genTable.tablecomment", "zh-CN", "表描述", "表描述（表注释）"),
            // entity.genTable.tablecomment
            new TranslationSeedItem("entity.genTable.tablecomment", "zh-HK", "表描述", "表描述（表注释）"),

            // entity.genTable.subtablename
            new TranslationSeedItem("entity.genTable.subtablename", "en-US", "关联父表", "关联父表名（用于主子表）"),
            // entity.genTable.subtablename
            new TranslationSeedItem("entity.genTable.subtablename", "ja-JP", "关联父表", "关联父表名（用于主子表）"),
            // entity.genTable.subtablename
            new TranslationSeedItem("entity.genTable.subtablename", "zh-CN", "关联父表", "关联父表名（用于主子表）"),
            // entity.genTable.subtablename
            new TranslationSeedItem("entity.genTable.subtablename", "zh-HK", "关联父表", "关联父表名（用于主子表）"),

            // entity.genTable.subtablefkname
            new TranslationSeedItem("entity.genTable.subtablefkname", "en-US", "关联外键", "本表关联父表的外键名（用于主子表）"),
            // entity.genTable.subtablefkname
            new TranslationSeedItem("entity.genTable.subtablefkname", "ja-JP", "关联外键", "本表关联父表的外键名（用于主子表）"),
            // entity.genTable.subtablefkname
            new TranslationSeedItem("entity.genTable.subtablefkname", "zh-CN", "关联外键", "本表关联父表的外键名（用于主子表）"),
            // entity.genTable.subtablefkname
            new TranslationSeedItem("entity.genTable.subtablefkname", "zh-HK", "关联外键", "本表关联父表的外键名（用于主子表）"),

            // entity.genTable.treecode
            new TranslationSeedItem("entity.genTable.treecode", "en-US", "树编码", "树编码字段（用于树形结构）"),
            // entity.genTable.treecode
            new TranslationSeedItem("entity.genTable.treecode", "ja-JP", "树编码", "树编码字段（用于树形结构）"),
            // entity.genTable.treecode
            new TranslationSeedItem("entity.genTable.treecode", "zh-CN", "树编码", "树编码字段（用于树形结构）"),
            // entity.genTable.treecode
            new TranslationSeedItem("entity.genTable.treecode", "zh-HK", "树编码", "树编码字段（用于树形结构）"),

            // entity.genTable.treeparentcode
            new TranslationSeedItem("entity.genTable.treeparentcode", "en-US", "树父编码", "树父编码字段（用于树形结构）"),
            // entity.genTable.treeparentcode
            new TranslationSeedItem("entity.genTable.treeparentcode", "ja-JP", "树父编码", "树父编码字段（用于树形结构）"),
            // entity.genTable.treeparentcode
            new TranslationSeedItem("entity.genTable.treeparentcode", "zh-CN", "树父编码", "树父编码字段（用于树形结构）"),
            // entity.genTable.treeparentcode
            new TranslationSeedItem("entity.genTable.treeparentcode", "zh-HK", "树父编码", "树父编码字段（用于树形结构）"),

            // entity.genTable.treename
            new TranslationSeedItem("entity.genTable.treename", "en-US", "树名称", "树名称字段（用于树形结构）"),
            // entity.genTable.treename
            new TranslationSeedItem("entity.genTable.treename", "ja-JP", "树名称", "树名称字段（用于树形结构）"),
            // entity.genTable.treename
            new TranslationSeedItem("entity.genTable.treename", "zh-CN", "树名称", "树名称字段（用于树形结构）"),
            // entity.genTable.treename
            new TranslationSeedItem("entity.genTable.treename", "zh-HK", "树名称", "树名称字段（用于树形结构）"),

            // entity.genTable.indatabase
            new TranslationSeedItem("entity.genTable.indatabase", "en-US", "库表标识", "是否在数据库中（1=是库表，0=不是库表）"),
            // entity.genTable.indatabase
            new TranslationSeedItem("entity.genTable.indatabase", "ja-JP", "库表标识", "是否在数据库中（1=是库表，0=不是库表）"),
            // entity.genTable.indatabase
            new TranslationSeedItem("entity.genTable.indatabase", "zh-CN", "库表标识", "是否在数据库中（1=是库表，0=不是库表）"),
            // entity.genTable.indatabase
            new TranslationSeedItem("entity.genTable.indatabase", "zh-HK", "库表标识", "是否在数据库中（1=是库表，0=不是库表）"),

            // entity.genTable.gentemplatecategory
            new TranslationSeedItem("entity.genTable.gentemplatecategory", "en-US", "生成模板类型", "生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）"),
            // entity.genTable.gentemplatecategory
            new TranslationSeedItem("entity.genTable.gentemplatecategory", "ja-JP", "生成模板类型", "生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）"),
            // entity.genTable.gentemplatecategory
            new TranslationSeedItem("entity.genTable.gentemplatecategory", "zh-CN", "生成模板类型", "生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）"),
            // entity.genTable.gentemplatecategory
            new TranslationSeedItem("entity.genTable.gentemplatecategory", "zh-HK", "生成模板类型", "生成模板类型（crud=单表操作，tree=树表操作，sub=主子表操作）"),

            // entity.genTable.genmodulename
            new TranslationSeedItem("entity.genTable.genmodulename", "en-US", "模块名", "模块名（功能模块名称）"),
            // entity.genTable.genmodulename
            new TranslationSeedItem("entity.genTable.genmodulename", "ja-JP", "模块名", "模块名（功能模块名称）"),
            // entity.genTable.genmodulename
            new TranslationSeedItem("entity.genTable.genmodulename", "zh-CN", "模块名", "模块名（功能模块名称）"),
            // entity.genTable.genmodulename
            new TranslationSeedItem("entity.genTable.genmodulename", "zh-HK", "模块名", "模块名（功能模块名称）"),

            // entity.genTable.genbusinessname
            new TranslationSeedItem("entity.genTable.genbusinessname", "en-US", "业务名", "业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）"),
            // entity.genTable.genbusinessname
            new TranslationSeedItem("entity.genTable.genbusinessname", "ja-JP", "业务名", "业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）"),
            // entity.genTable.genbusinessname
            new TranslationSeedItem("entity.genTable.genbusinessname", "zh-CN", "业务名", "业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）"),
            // entity.genTable.genbusinessname
            new TranslationSeedItem("entity.genTable.genbusinessname", "zh-HK", "业务名", "业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）"),

            // entity.genTable.genfunctionname
            new TranslationSeedItem("entity.genTable.genfunctionname", "en-US", "功能名", "功能名（用于接口与注释的中文名称，如：公司、部门）"),
            // entity.genTable.genfunctionname
            new TranslationSeedItem("entity.genTable.genfunctionname", "ja-JP", "功能名", "功能名（用于接口与注释的中文名称，如：公司、部门）"),
            // entity.genTable.genfunctionname
            new TranslationSeedItem("entity.genTable.genfunctionname", "zh-CN", "功能名", "功能名（用于接口与注释的中文名称，如：公司、部门）"),
            // entity.genTable.genfunctionname
            new TranslationSeedItem("entity.genTable.genfunctionname", "zh-HK", "功能名", "功能名（用于接口与注释的中文名称，如：公司、部门）"),

            // entity.genTable.permsprefix
            new TranslationSeedItem("entity.genTable.permsprefix", "en-US", "权限前缀", "权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。"),
            // entity.genTable.permsprefix
            new TranslationSeedItem("entity.genTable.permsprefix", "ja-JP", "权限前缀", "权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。"),
            // entity.genTable.permsprefix
            new TranslationSeedItem("entity.genTable.permsprefix", "zh-CN", "权限前缀", "权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。"),
            // entity.genTable.permsprefix
            new TranslationSeedItem("entity.genTable.permsprefix", "zh-HK", "权限前缀", "权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。"),

            // entity.genTable.menubuttongroup
            new TranslationSeedItem("entity.genTable.menubuttongroup", "en-US", "菜单权限组", "菜单权限组"),
            // entity.genTable.menubuttongroup
            new TranslationSeedItem("entity.genTable.menubuttongroup", "ja-JP", "菜单权限组", "菜单权限组"),
            // entity.genTable.menubuttongroup
            new TranslationSeedItem("entity.genTable.menubuttongroup", "zh-CN", "菜单权限组", "菜单权限组"),
            // entity.genTable.menubuttongroup
            new TranslationSeedItem("entity.genTable.menubuttongroup", "zh-HK", "菜单权限组", "菜单权限组"),

            // entity.genTable.nameprefix
            new TranslationSeedItem("entity.genTable.nameprefix", "en-US", "命名空间前缀", "命名空间前缀（用于生成类名、方法名等的前缀）"),
            // entity.genTable.nameprefix
            new TranslationSeedItem("entity.genTable.nameprefix", "ja-JP", "命名空间前缀", "命名空间前缀（用于生成类名、方法名等的前缀）"),
            // entity.genTable.nameprefix
            new TranslationSeedItem("entity.genTable.nameprefix", "zh-CN", "命名空间前缀", "命名空间前缀（用于生成类名、方法名等的前缀）"),
            // entity.genTable.nameprefix
            new TranslationSeedItem("entity.genTable.nameprefix", "zh-HK", "命名空间前缀", "命名空间前缀（用于生成类名、方法名等的前缀）"),

            // entity.genTable.entitynamespace
            new TranslationSeedItem("entity.genTable.entitynamespace", "en-US", "实体命名空间", "实体命名空间（默认当前项目：Takt.Domain.Entities）"),
            // entity.genTable.entitynamespace
            new TranslationSeedItem("entity.genTable.entitynamespace", "ja-JP", "实体命名空间", "实体命名空间（默认当前项目：Takt.Domain.Entities）"),
            // entity.genTable.entitynamespace
            new TranslationSeedItem("entity.genTable.entitynamespace", "zh-CN", "实体命名空间", "实体命名空间（默认当前项目：Takt.Domain.Entities）"),
            // entity.genTable.entitynamespace
            new TranslationSeedItem("entity.genTable.entitynamespace", "zh-HK", "实体命名空间", "实体命名空间（默认当前项目：Takt.Domain.Entities）"),

            // entity.genTable.entityclassname
            new TranslationSeedItem("entity.genTable.entityclassname", "en-US", "实体类名称", "实体类名称（首字母大写，驼峰命名）"),
            // entity.genTable.entityclassname
            new TranslationSeedItem("entity.genTable.entityclassname", "ja-JP", "实体类名称", "实体类名称（首字母大写，驼峰命名）"),
            // entity.genTable.entityclassname
            new TranslationSeedItem("entity.genTable.entityclassname", "zh-CN", "实体类名称", "实体类名称（首字母大写，驼峰命名）"),
            // entity.genTable.entityclassname
            new TranslationSeedItem("entity.genTable.entityclassname", "zh-HK", "实体类名称", "实体类名称（首字母大写，驼峰命名）"),

            // entity.genTable.dtonamespace
            new TranslationSeedItem("entity.genTable.dtonamespace", "en-US", "传输对象Dto命名空间", "传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）"),
            // entity.genTable.dtonamespace
            new TranslationSeedItem("entity.genTable.dtonamespace", "ja-JP", "传输对象Dto命名空间", "传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）"),
            // entity.genTable.dtonamespace
            new TranslationSeedItem("entity.genTable.dtonamespace", "zh-CN", "传输对象Dto命名空间", "传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）"),
            // entity.genTable.dtonamespace
            new TranslationSeedItem("entity.genTable.dtonamespace", "zh-HK", "传输对象Dto命名空间", "传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）"),

            // entity.genTable.dtoclassname
            new TranslationSeedItem("entity.genTable.dtoclassname", "en-US", "传输对象Dto类名", "传输对象 Dto 类名"),
            // entity.genTable.dtoclassname
            new TranslationSeedItem("entity.genTable.dtoclassname", "ja-JP", "传输对象Dto类名", "传输对象 Dto 类名"),
            // entity.genTable.dtoclassname
            new TranslationSeedItem("entity.genTable.dtoclassname", "zh-CN", "传输对象Dto类名", "传输对象 Dto 类名"),
            // entity.genTable.dtoclassname
            new TranslationSeedItem("entity.genTable.dtoclassname", "zh-HK", "传输对象Dto类名", "传输对象 Dto 类名"),

            // entity.genTable.servicenamespace
            new TranslationSeedItem("entity.genTable.servicenamespace", "en-US", "服务命名空间", "服务命名空间（默认当前项目：Takt.Application.Services）"),
            // entity.genTable.servicenamespace
            new TranslationSeedItem("entity.genTable.servicenamespace", "ja-JP", "服务命名空间", "服务命名空间（默认当前项目：Takt.Application.Services）"),
            // entity.genTable.servicenamespace
            new TranslationSeedItem("entity.genTable.servicenamespace", "zh-CN", "服务命名空间", "服务命名空间（默认当前项目：Takt.Application.Services）"),
            // entity.genTable.servicenamespace
            new TranslationSeedItem("entity.genTable.servicenamespace", "zh-HK", "服务命名空间", "服务命名空间（默认当前项目：Takt.Application.Services）"),

            // entity.genTable.iserviceclassname
            new TranslationSeedItem("entity.genTable.iserviceclassname", "en-US", "服务接口类名称", "服务接口类名称"),
            // entity.genTable.iserviceclassname
            new TranslationSeedItem("entity.genTable.iserviceclassname", "ja-JP", "服务接口类名称", "服务接口类名称"),
            // entity.genTable.iserviceclassname
            new TranslationSeedItem("entity.genTable.iserviceclassname", "zh-CN", "服务接口类名称", "服务接口类名称"),
            // entity.genTable.iserviceclassname
            new TranslationSeedItem("entity.genTable.iserviceclassname", "zh-HK", "服务接口类名称", "服务接口类名称"),

            // entity.genTable.serviceclassname
            new TranslationSeedItem("entity.genTable.serviceclassname", "en-US", "服务类名称", "服务类名称"),
            // entity.genTable.serviceclassname
            new TranslationSeedItem("entity.genTable.serviceclassname", "ja-JP", "服务类名称", "服务类名称"),
            // entity.genTable.serviceclassname
            new TranslationSeedItem("entity.genTable.serviceclassname", "zh-CN", "服务类名称", "服务类名称"),
            // entity.genTable.serviceclassname
            new TranslationSeedItem("entity.genTable.serviceclassname", "zh-HK", "服务类名称", "服务类名称"),

            // entity.genTable.controllernamespace
            new TranslationSeedItem("entity.genTable.controllernamespace", "en-US", "控制器命名空间", "控制器命名空间（默认当前项目：Takt.WebApi.Controllers）"),
            // entity.genTable.controllernamespace
            new TranslationSeedItem("entity.genTable.controllernamespace", "ja-JP", "控制器命名空间", "控制器命名空间（默认当前项目：Takt.WebApi.Controllers）"),
            // entity.genTable.controllernamespace
            new TranslationSeedItem("entity.genTable.controllernamespace", "zh-CN", "控制器命名空间", "控制器命名空间（默认当前项目：Takt.WebApi.Controllers）"),
            // entity.genTable.controllernamespace
            new TranslationSeedItem("entity.genTable.controllernamespace", "zh-HK", "控制器命名空间", "控制器命名空间（默认当前项目：Takt.WebApi.Controllers）"),

            // entity.genTable.controllerclassname
            new TranslationSeedItem("entity.genTable.controllerclassname", "en-US", "控制器类名称", "控制器类名称"),
            // entity.genTable.controllerclassname
            new TranslationSeedItem("entity.genTable.controllerclassname", "ja-JP", "控制器类名称", "控制器类名称"),
            // entity.genTable.controllerclassname
            new TranslationSeedItem("entity.genTable.controllerclassname", "zh-CN", "控制器类名称", "控制器类名称"),
            // entity.genTable.controllerclassname
            new TranslationSeedItem("entity.genTable.controllerclassname", "zh-HK", "控制器类名称", "控制器类名称"),

            // entity.genTable.isrepository
            new TranslationSeedItem("entity.genTable.isrepository", "en-US", "仓储层", "是否生成仓储层（1=是，0=否）"),
            // entity.genTable.isrepository
            new TranslationSeedItem("entity.genTable.isrepository", "ja-JP", "仓储层", "是否生成仓储层（1=是，0=否）"),
            // entity.genTable.isrepository
            new TranslationSeedItem("entity.genTable.isrepository", "zh-CN", "仓储层", "是否生成仓储层（1=是，0=否）"),
            // entity.genTable.isrepository
            new TranslationSeedItem("entity.genTable.isrepository", "zh-HK", "仓储层", "是否生成仓储层（1=是，0=否）"),

            // entity.genTable.repositoryinterfacenamespace
            new TranslationSeedItem("entity.genTable.repositoryinterfacenamespace", "en-US", "仓储接口命名空间", "仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）"),
            // entity.genTable.repositoryinterfacenamespace
            new TranslationSeedItem("entity.genTable.repositoryinterfacenamespace", "ja-JP", "仓储接口命名空间", "仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）"),
            // entity.genTable.repositoryinterfacenamespace
            new TranslationSeedItem("entity.genTable.repositoryinterfacenamespace", "zh-CN", "仓储接口命名空间", "仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）"),
            // entity.genTable.repositoryinterfacenamespace
            new TranslationSeedItem("entity.genTable.repositoryinterfacenamespace", "zh-HK", "仓储接口命名空间", "仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）"),

            // entity.genTable.irepositoryclassname
            new TranslationSeedItem("entity.genTable.irepositoryclassname", "en-US", "仓储接口类名称", "仓储接口类名称"),
            // entity.genTable.irepositoryclassname
            new TranslationSeedItem("entity.genTable.irepositoryclassname", "ja-JP", "仓储接口类名称", "仓储接口类名称"),
            // entity.genTable.irepositoryclassname
            new TranslationSeedItem("entity.genTable.irepositoryclassname", "zh-CN", "仓储接口类名称", "仓储接口类名称"),
            // entity.genTable.irepositoryclassname
            new TranslationSeedItem("entity.genTable.irepositoryclassname", "zh-HK", "仓储接口类名称", "仓储接口类名称"),

            // entity.genTable.repositorynamespace
            new TranslationSeedItem("entity.genTable.repositorynamespace", "en-US", "仓储命名空间", "仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）"),
            // entity.genTable.repositorynamespace
            new TranslationSeedItem("entity.genTable.repositorynamespace", "ja-JP", "仓储命名空间", "仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）"),
            // entity.genTable.repositorynamespace
            new TranslationSeedItem("entity.genTable.repositorynamespace", "zh-CN", "仓储命名空间", "仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）"),
            // entity.genTable.repositorynamespace
            new TranslationSeedItem("entity.genTable.repositorynamespace", "zh-HK", "仓储命名空间", "仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）"),

            // entity.genTable.repositoryclassname
            new TranslationSeedItem("entity.genTable.repositoryclassname", "en-US", "仓储类名称", "仓储类名称"),
            // entity.genTable.repositoryclassname
            new TranslationSeedItem("entity.genTable.repositoryclassname", "ja-JP", "仓储类名称", "仓储类名称"),
            // entity.genTable.repositoryclassname
            new TranslationSeedItem("entity.genTable.repositoryclassname", "zh-CN", "仓储类名称", "仓储类名称"),
            // entity.genTable.repositoryclassname
            new TranslationSeedItem("entity.genTable.repositoryclassname", "zh-HK", "仓储类名称", "仓储类名称"),

            // entity.genTable.genfunction
            new TranslationSeedItem("entity.genTable.genfunction", "en-US", "生成功能", "生成功能，JSON 格式。对象形式：{\"查看\":\"View\",\"新增\":\"Create\",\"更新\":\"Update\",\"删除\":\"Delete\",...}，键为中文功能名、值为英文标识；也支持数组 [\"查询\",\"新增\",...] 或逗号分隔。 <para><b>核心设计</b>：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下：</para> <para>- Query → QueryDto（查询传输对象）</para> <para>- Create → CreateDto（创建传输对象）</para> <para>- Update → UpdateDto（更新传输对象）</para> <para>- Status → StatusDto（状态传输对象）</para> <para>- Sort → SortDto（排序传输对象）</para> <para>- Import → TemplateDto + ImportDto（模板+导入传输对象）</para> <para>- Export → ExportDto（导出传输对象）</para> <para>- 所有功能 → Dto（基础传输对象，包含所有字段）</para>"),
            // entity.genTable.genfunction
            new TranslationSeedItem("entity.genTable.genfunction", "ja-JP", "生成功能", "生成功能，JSON 格式。对象形式：{\"查看\":\"View\",\"新增\":\"Create\",\"更新\":\"Update\",\"删除\":\"Delete\",...}，键为中文功能名、值为英文标识；也支持数组 [\"查询\",\"新增\",...] 或逗号分隔。 <para><b>核心设计</b>：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下：</para> <para>- Query → QueryDto（查询传输对象）</para> <para>- Create → CreateDto（创建传输对象）</para> <para>- Update → UpdateDto（更新传输对象）</para> <para>- Status → StatusDto（状态传输对象）</para> <para>- Sort → SortDto（排序传输对象）</para> <para>- Import → TemplateDto + ImportDto（模板+导入传输对象）</para> <para>- Export → ExportDto（导出传输对象）</para> <para>- 所有功能 → Dto（基础传输对象，包含所有字段）</para>"),
            // entity.genTable.genfunction
            new TranslationSeedItem("entity.genTable.genfunction", "zh-CN", "生成功能", "生成功能，JSON 格式。对象形式：{\"查看\":\"View\",\"新增\":\"Create\",\"更新\":\"Update\",\"删除\":\"Delete\",...}，键为中文功能名、值为英文标识；也支持数组 [\"查询\",\"新增\",...] 或逗号分隔。 <para><b>核心设计</b>：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下：</para> <para>- Query → QueryDto（查询传输对象）</para> <para>- Create → CreateDto（创建传输对象）</para> <para>- Update → UpdateDto（更新传输对象）</para> <para>- Status → StatusDto（状态传输对象）</para> <para>- Sort → SortDto（排序传输对象）</para> <para>- Import → TemplateDto + ImportDto（模板+导入传输对象）</para> <para>- Export → ExportDto（导出传输对象）</para> <para>- 所有功能 → Dto（基础传输对象，包含所有字段）</para>"),
            // entity.genTable.genfunction
            new TranslationSeedItem("entity.genTable.genfunction", "zh-HK", "生成功能", "生成功能，JSON 格式。对象形式：{\"查看\":\"View\",\"新增\":\"Create\",\"更新\":\"Update\",\"删除\":\"Delete\",...}，键为中文功能名、值为英文标识；也支持数组 [\"查询\",\"新增\",...] 或逗号分隔。 <para><b>核心设计</b>：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下：</para> <para>- Query → QueryDto（查询传输对象）</para> <para>- Create → CreateDto（创建传输对象）</para> <para>- Update → UpdateDto（更新传输对象）</para> <para>- Status → StatusDto（状态传输对象）</para> <para>- Sort → SortDto（排序传输对象）</para> <para>- Import → TemplateDto + ImportDto（模板+导入传输对象）</para> <para>- Export → ExportDto（导出传输对象）</para> <para>- 所有功能 → Dto（基础传输对象，包含所有字段）</para>"),

            // entity.genTable.genmethod
            new TranslationSeedItem("entity.genTable.genmethod", "en-US", "生成方式", "生成代码方式（0=zip 压缩包，1=自定义路径，2=当前项目）"),
            // entity.genTable.genmethod
            new TranslationSeedItem("entity.genTable.genmethod", "ja-JP", "生成方式", "生成代码方式（0=zip 压缩包，1=自定义路径，2=当前项目）"),
            // entity.genTable.genmethod
            new TranslationSeedItem("entity.genTable.genmethod", "zh-CN", "生成方式", "生成代码方式（0=zip 压缩包，1=自定义路径，2=当前项目）"),
            // entity.genTable.genmethod
            new TranslationSeedItem("entity.genTable.genmethod", "zh-HK", "生成方式", "生成代码方式（0=zip 压缩包，1=自定义路径，2=当前项目）"),

            // entity.genTable.genpath
            new TranslationSeedItem("entity.genTable.genpath", "en-US", "生成路径", "生成路径（默认为项目根目录）"),
            // entity.genTable.genpath
            new TranslationSeedItem("entity.genTable.genpath", "ja-JP", "生成路径", "生成路径（默认为项目根目录）"),
            // entity.genTable.genpath
            new TranslationSeedItem("entity.genTable.genpath", "zh-CN", "生成路径", "生成路径（默认为项目根目录）"),
            // entity.genTable.genpath
            new TranslationSeedItem("entity.genTable.genpath", "zh-HK", "生成路径", "生成路径（默认为项目根目录）"),

            // entity.genTable.isgenmenu
            new TranslationSeedItem("entity.genTable.isgenmenu", "en-US", "生成菜单", "是否生成菜单（1=是，0=否）"),
            // entity.genTable.isgenmenu
            new TranslationSeedItem("entity.genTable.isgenmenu", "ja-JP", "生成菜单", "是否生成菜单（1=是，0=否）"),
            // entity.genTable.isgenmenu
            new TranslationSeedItem("entity.genTable.isgenmenu", "zh-CN", "生成菜单", "是否生成菜单（1=是，0=否）"),
            // entity.genTable.isgenmenu
            new TranslationSeedItem("entity.genTable.isgenmenu", "zh-HK", "生成菜单", "是否生成菜单（1=是，0=否）"),

            // entity.genTable.parentmenuid
            new TranslationSeedItem("entity.genTable.parentmenuid", "en-US", "上级菜单ID", "上级菜单ID"),
            // entity.genTable.parentmenuid
            new TranslationSeedItem("entity.genTable.parentmenuid", "ja-JP", "上级菜单ID", "上级菜单ID"),
            // entity.genTable.parentmenuid
            new TranslationSeedItem("entity.genTable.parentmenuid", "zh-CN", "上级菜单ID", "上级菜单ID"),
            // entity.genTable.parentmenuid
            new TranslationSeedItem("entity.genTable.parentmenuid", "zh-HK", "上级菜单ID", "上级菜单ID"),

            // entity.genTable.isgentranslation
            new TranslationSeedItem("entity.genTable.isgentranslation", "en-US", "生成翻译", "是否生成翻译（1=是，0=否）"),
            // entity.genTable.isgentranslation
            new TranslationSeedItem("entity.genTable.isgentranslation", "ja-JP", "生成翻译", "是否生成翻译（1=是，0=否）"),
            // entity.genTable.isgentranslation
            new TranslationSeedItem("entity.genTable.isgentranslation", "zh-CN", "生成翻译", "是否生成翻译（1=是，0=否）"),
            // entity.genTable.isgentranslation
            new TranslationSeedItem("entity.genTable.isgentranslation", "zh-HK", "生成翻译", "是否生成翻译（1=是，0=否）"),

            // entity.genTable.sortfield
            new TranslationSeedItem("entity.genTable.sortfield", "en-US", "排序字段", "排序字段"),
            // entity.genTable.sortfield
            new TranslationSeedItem("entity.genTable.sortfield", "ja-JP", "排序字段", "排序字段"),
            // entity.genTable.sortfield
            new TranslationSeedItem("entity.genTable.sortfield", "zh-CN", "排序字段", "排序字段"),
            // entity.genTable.sortfield
            new TranslationSeedItem("entity.genTable.sortfield", "zh-HK", "排序字段", "排序字段"),

            // entity.genTable.sorttype
            new TranslationSeedItem("entity.genTable.sorttype", "en-US", "排序类型", "排序类型（asc=升序，desc=降序）"),
            // entity.genTable.sorttype
            new TranslationSeedItem("entity.genTable.sorttype", "ja-JP", "排序类型", "排序类型（asc=升序，desc=降序）"),
            // entity.genTable.sorttype
            new TranslationSeedItem("entity.genTable.sorttype", "zh-CN", "排序类型", "排序类型（asc=升序，desc=降序）"),
            // entity.genTable.sorttype
            new TranslationSeedItem("entity.genTable.sorttype", "zh-HK", "排序类型", "排序类型（asc=升序，desc=降序）"),

            // entity.genTable.frontui
            new TranslationSeedItem("entity.genTable.frontui", "en-US", "前端UI框架", "前端UI框架（1=element plus，2=ant design vue）"),
            // entity.genTable.frontui
            new TranslationSeedItem("entity.genTable.frontui", "ja-JP", "前端UI框架", "前端UI框架（1=element plus，2=ant design vue）"),
            // entity.genTable.frontui
            new TranslationSeedItem("entity.genTable.frontui", "zh-CN", "前端UI框架", "前端UI框架（1=element plus，2=ant design vue）"),
            // entity.genTable.frontui
            new TranslationSeedItem("entity.genTable.frontui", "zh-HK", "前端UI框架", "前端UI框架（1=element plus，2=ant design vue）"),

            // entity.genTable.frontformlayout
            new TranslationSeedItem("entity.genTable.frontformlayout", "en-US", "前端表单布局", "前端表单布局（12=一行一列，24=一行两列）"),
            // entity.genTable.frontformlayout
            new TranslationSeedItem("entity.genTable.frontformlayout", "ja-JP", "前端表单布局", "前端表单布局（12=一行一列，24=一行两列）"),
            // entity.genTable.frontformlayout
            new TranslationSeedItem("entity.genTable.frontformlayout", "zh-CN", "前端表单布局", "前端表单布局（12=一行一列，24=一行两列）"),
            // entity.genTable.frontformlayout
            new TranslationSeedItem("entity.genTable.frontformlayout", "zh-HK", "前端表单布局", "前端表单布局（12=一行一列，24=一行两列）"),

            // entity.genTable.frontbtnstyle
            new TranslationSeedItem("entity.genTable.frontbtnstyle", "en-US", "前端按钮样式", "前端操作按钮样式（0=文本，1=标准）"),
            // entity.genTable.frontbtnstyle
            new TranslationSeedItem("entity.genTable.frontbtnstyle", "ja-JP", "前端按钮样式", "前端操作按钮样式（0=文本，1=标准）"),
            // entity.genTable.frontbtnstyle
            new TranslationSeedItem("entity.genTable.frontbtnstyle", "zh-CN", "前端按钮样式", "前端操作按钮样式（0=文本，1=标准）"),
            // entity.genTable.frontbtnstyle
            new TranslationSeedItem("entity.genTable.frontbtnstyle", "zh-HK", "前端按钮样式", "前端操作按钮样式（0=文本，1=标准）"),

            // entity.genTable.isgencode
            new TranslationSeedItem("entity.genTable.isgencode", "en-US", "是否生成", "是否生成代码（1=是，0=否）"),
            // entity.genTable.isgencode
            new TranslationSeedItem("entity.genTable.isgencode", "ja-JP", "是否生成", "是否生成代码（1=是，0=否）"),
            // entity.genTable.isgencode
            new TranslationSeedItem("entity.genTable.isgencode", "zh-CN", "是否生成", "是否生成代码（1=是，0=否）"),
            // entity.genTable.isgencode
            new TranslationSeedItem("entity.genTable.isgencode", "zh-HK", "是否生成", "是否生成代码（1=是，0=否）"),

            // entity.genTable.gencodecount
            new TranslationSeedItem("entity.genTable.gencodecount", "en-US", "代码生成次数", "代码生成次数（每次生成成功后自增）"),
            // entity.genTable.gencodecount
            new TranslationSeedItem("entity.genTable.gencodecount", "ja-JP", "代码生成次数", "代码生成次数（每次生成成功后自增）"),
            // entity.genTable.gencodecount
            new TranslationSeedItem("entity.genTable.gencodecount", "zh-CN", "代码生成次数", "代码生成次数（每次生成成功后自增）"),
            // entity.genTable.gencodecount
            new TranslationSeedItem("entity.genTable.gencodecount", "zh-HK", "代码生成次数", "代码生成次数（每次生成成功后自增）"),

            // entity.genTable.isusetabs
            new TranslationSeedItem("entity.genTable.isusetabs", "en-US", "使用tabs", "是否使用tabs（1=是，0=否）"),
            // entity.genTable.isusetabs
            new TranslationSeedItem("entity.genTable.isusetabs", "ja-JP", "使用tabs", "是否使用tabs（1=是，0=否）"),
            // entity.genTable.isusetabs
            new TranslationSeedItem("entity.genTable.isusetabs", "zh-CN", "使用tabs", "是否使用tabs（1=是，0=否）"),
            // entity.genTable.isusetabs
            new TranslationSeedItem("entity.genTable.isusetabs", "zh-HK", "使用tabs", "是否使用tabs（1=是，0=否）"),

            // entity.genTable.tabsfieldcount
            new TranslationSeedItem("entity.genTable.tabsfieldcount", "en-US", "tabs标签字段", "tabs标签中字段的数量"),
            // entity.genTable.tabsfieldcount
            new TranslationSeedItem("entity.genTable.tabsfieldcount", "ja-JP", "tabs标签字段", "tabs标签中字段的数量"),
            // entity.genTable.tabsfieldcount
            new TranslationSeedItem("entity.genTable.tabsfieldcount", "zh-CN", "tabs标签字段", "tabs标签中字段的数量"),
            // entity.genTable.tabsfieldcount
            new TranslationSeedItem("entity.genTable.tabsfieldcount", "zh-HK", "tabs标签字段", "tabs标签中字段的数量"),

            // entity.genTable.genauthor
            new TranslationSeedItem("entity.genTable.genauthor", "en-US", "作者", "作者"),
            // entity.genTable.genauthor
            new TranslationSeedItem("entity.genTable.genauthor", "ja-JP", "作者", "作者"),
            // entity.genTable.genauthor
            new TranslationSeedItem("entity.genTable.genauthor", "zh-CN", "作者", "作者"),
            // entity.genTable.genauthor
            new TranslationSeedItem("entity.genTable.genauthor", "zh-HK", "作者", "作者"),

            // entity.genTable.othergenoptions
            new TranslationSeedItem("entity.genTable.othergenoptions", "en-US", "其他生成选项", "其他生成选项（JSON格式，存储其他生成配置）"),
            // entity.genTable.othergenoptions
            new TranslationSeedItem("entity.genTable.othergenoptions", "ja-JP", "其他生成选项", "其他生成选项（JSON格式，存储其他生成配置）"),
            // entity.genTable.othergenoptions
            new TranslationSeedItem("entity.genTable.othergenoptions", "zh-CN", "其他生成选项", "其他生成选项（JSON格式，存储其他生成配置）"),
            // entity.genTable.othergenoptions
            new TranslationSeedItem("entity.genTable.othergenoptions", "zh-HK", "其他生成选项", "其他生成选项（JSON格式，存储其他生成配置）"),

            // entity.genTable.columns
            new TranslationSeedItem("entity.genTable.columns", "en-US", "columns", "字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）"),
            // entity.genTable.columns
            new TranslationSeedItem("entity.genTable.columns", "ja-JP", "columns", "字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）"),
            // entity.genTable.columns
            new TranslationSeedItem("entity.genTable.columns", "zh-CN", "columns", "字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）"),
            // entity.genTable.columns
            new TranslationSeedItem("entity.genTable.columns", "zh-HK", "columns", "字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）"),
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
        translation.ResourceGroup = TaktModule.Code;
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
