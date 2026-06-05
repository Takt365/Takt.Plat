// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktMenuButtonSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt 按钮菜单种子数据。
//           遍历所有页面菜单（MenuType=1），按权限前缀匹配按钮模板，
//           为每个菜单生成一组按钮子项（MenuType=2），并写入权限标识与排序。
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Takt 按钮菜单种子数据。
/// <para>
/// 在各级页面菜单（Level1～Level5）种子执行完毕后，由 <see cref="TaktMenuSeedData"/> 调用。
/// 仅处理 <c>MenuType == 1</c> 且未删除的菜单；要求 <c>Permission</c> 非空且以 <c>:list</c> 结尾。
/// 由 TaktMenuSeedData 统一协调调用，不直接注册为 ITaktSeedDataCoordinator。
/// </para>
/// </summary>
public class TaktMenuButtonSeedData
{
    /// <summary>
    /// 初始化按钮菜单种子数据。
    /// <para>
    /// 对每个符合条件的页面菜单，根据其权限字符串首段（模块前缀）选择预置按钮组：
    /// 通用 CRUD 为基线，Accounting / Code / Foundation / HumanResource / Identity / Routine / Workflow / Statistics
    /// 等模块仅追加特有操作按钮，生成或更新子按钮记录。
    /// </para>
    /// </summary>
    /// <param name="serviceProvider">服务提供者，用于解析 <see cref="ITaktRepository{TaktMenu}"/>。</param>
    /// <param name="specifiedTenantCode">租户编码（由协调器传入）。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，分别为本次新增与更新的菜单（按钮）条数。</returns>
    /// <exception cref="InvalidOperationException">当某页面菜单的 Permission 为空或不以 :list 结尾时抛出。</exception>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? specifiedTenantCode = null)
    {
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();

        // 按钮菜单:基于所有页面菜单(MenuType=1)
        // 注意:菜单为租户级实体,由协调器指定租户,必须传入租户编码
        if (string.IsNullOrWhiteSpace(specifiedTenantCode))
        {
            TaktLogger.Warning("未指定租户编码,跳过按钮菜单种子数据初始化");
            return (0, 0);
        }
        
        var tenantCode = specifiedTenantCode;

        int insertCount = 0;
        int updateCount = 0;

        var menus = await menuRepository.GetListAsync(m => m.TenantCode == tenantCode && m.MenuType == 1);

        foreach (var menu in menus)
        {
            if (string.IsNullOrEmpty(menu.Permission))
            {
                throw new InvalidOperationException(
                    $"菜单 {menu.MenuCode} ({menu.MenuName}) 的 MenuType=1，但 Permission 为空。必须设置 Permission，且必须以 \":list\" 结尾。");
            }

            if (!menu.Permission.EndsWith(":list", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"菜单 {menu.MenuCode} ({menu.MenuName}) 的 Permission={menu.Permission} 必须以 \":list\" 结尾。");
            }

            // 从 Permission 中解析模块前缀（第一段），用于选择预置按钮组；若以 takt: 开头则走默认组
            var modulePrefix = GetModulePrefix(menu.Permission);
            // 为该菜单创建按钮子项(所有 MenuType=1 的菜单都需要按钮)
            var (insert, update) = await CreateButtonsForMenuAsync(menuRepository, menu, modulePrefix, tenantCode);
            insertCount += insert;
            updateCount += update;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 为单个页面菜单创建或更新其下全部按钮子项。
    /// </summary>
    /// <param name="menuRepository">菜单仓储。</param>
    /// <param name="menu">父级页面菜单实体（MenuType=1）。</param>
    /// <param name="modulePrefix">
    /// 从 <see cref="TaktMenu.Permission"/> 解析出的模块前缀（冒号分隔第一段的小写形式）。
    /// 若以 <c>takt</c> 开头则视为空前缀，走默认通用按钮组。
    /// </param>
    /// <returns>元组：(InsertCount, UpdateCount)，本菜单下按钮新增与更新条数。</returns>
    private static async Task<(int InsertCount, int UpdateCount)> CreateButtonsForMenuAsync(
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        TaktMenu menu,
        string modulePrefix,
        string tenantCode)
    {
        int insertCount = 0;
        int updateCount = 0;

        // 获取按钮配置（按模块前缀匹配：通用 / 身份 / 代码生成 / 工作流 / 日常 / 人力 / 会计等）
        var (buttonNames, buttonPerms) = GetButtonConfig(modulePrefix);
        if (buttonNames == null || buttonPerms == null)
            return (0, 0);

        // 从菜单的权限标识中解析"菜单段"，用于拼接按钮权限；若无法解析出有效段，则使用 MenuCode 的小写形式
        var menuPerm = GetMenuPerm(menu.Permission);
        if (string.IsNullOrEmpty(menuPerm) && !string.IsNullOrEmpty(menu.MenuCode))
            menuPerm = menu.MenuCode.ToLowerInvariant();

        // 生成各按钮
        for (int i = 0; i < buttonNames.Length; i++)
        {
            var buttonName = buttonNames[i];
            var buttonPerm = buttonPerms[i];

            // 生成按钮编码：优先使用 MenuCode_buttonPerm；超过列长度（50）时自动收缩并附加稳定哈希后缀
            var buttonCode = BuildButtonCode(menu.MenuCode, buttonPerm);

            // 生成权限字符串：顶级目录 + … + 业务实体 + 操作
            // 格式一般为 modulePrefix:menuPerm:buttonPerm；若 modulePrefix 为空则为 menuPerm:buttonPerm；若 menuPerm 也为空则为 buttonPerm
            var permission = string.IsNullOrEmpty(modulePrefix)
                ? (string.IsNullOrEmpty(menuPerm)
                    ? buttonPerm.ToLowerInvariant()
                    : $"{menuPerm}:{buttonPerm.ToLowerInvariant()}")
                : $"{modulePrefix.ToLower()}:{menuPerm}:{buttonPerm.ToLowerInvariant()}";

            // 生成本地化键：common.button.{操作后缀}
            var menuL10nKey = $"common.button.{buttonPerm.ToLowerInvariant()}";

            var (insert, update) = await CreateOrUpdateButtonAsync(
                menuRepository, tenantCode, menu.Id, buttonCode, buttonName, permission, menuL10nKey, i + 1);
            insertCount += insert;
            updateCount += update;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 从完整权限字符串中提取模块前缀（第一段），用于 <see cref="GetButtonConfig"/> 分支。
    /// </summary>
    /// <param name="permission">菜单权限字符串，例如 <c>identity:user:list</c>。</param>
    /// <returns>
    /// 第一段的小写形式；若以 <c>takt</c> 开头则返回空字符串，表示使用默认按钮模板。
    /// </returns>
    private static string GetModulePrefix(string permission)
    {
        if (string.IsNullOrEmpty(permission))
            return string.Empty;

        var parts = permission.Split(':');
        if (parts.Length == 0)
            return string.Empty;

        if (parts[0].Equals("takt", StringComparison.OrdinalIgnoreCase))
            return string.Empty;

        return parts[0].ToLowerInvariant();
    }

    /// <summary>
    /// 从权限字符串中解析"菜单段"用于拼接按钮权限：去掉首段模块名与末段 <c>list</c>，保留中间段。
    /// </summary>
    /// <param name="permission">菜单权限字符串。</param>
    /// <returns>用于拼接按钮权限的中间路径（小写），例如 <c>user</c> 或 <c>a:b</c>。</returns>
    private static string GetMenuPerm(string? permission)
    {
        if (string.IsNullOrEmpty(permission))
            return string.Empty;

        var parts = permission.Split(':');
        if (parts.Length > 2)
            // 截取第一个冒号之后、最后一个冒号之前的部分（支持多级路径）
            return string.Join(":", parts[1..^1]).ToLower();

        if (parts.Length == 2)
            // 仅两段时取第二段（如 module:entity:list 中的 entity）
            return parts[1].ToLower();

        if (parts.Length == 1)
            return parts[0].ToLower();

        return string.Empty;
    }

    /// <summary>
    /// 通用 CRUD 按钮显示名称（含树表全场景、主子表明细全场景；末项固定为「批量」）。
    /// </summary>
    private static readonly string[] GenericButtonNames =
    {
        "查询", "新增", "修改", "删除", "详情", "预览", "打印", "导入", "导出", "模板", "审批", "撤销", "树", "选项", "主题",
        "展开", "收缩", "新增根", "新增下级", "同级新增", "插入", "编辑节点", "复制节点",
        "删除当前", "删除本级及子级", "移动", "上移", "下移", "置顶", "置底", "提升层级", "降级层级",
        "新增行", "插入行", "编辑行", "修改行", "删除行", "复制行", "克隆行",
        "行上移", "行下移", "清空明细", "导入明细", "导出明细", "明细批量",
        "批量"
    };

    /// <summary>通用 CRUD 按钮权限后缀（与 <see cref="GenericButtonNames"/> 一一对应）。</summary>
    private static readonly string[] GenericButtonPerms =
    {
        "query", "create", "update", "delete", "detail", "preview", "print", "import", "export", "template", "approve", "revoke", "tree", "options", "theme",
        "expand", "collapse", "createroot", "createchild", "createsibling", "insert", "editnode", "clonenode",
        "deletecurrent", "deletesubtree", "move", "moveup", "movedown", "movetop", "movebottom", "promote", "demote",
        "createrow", "insertrow", "editrow", "updaterow", "deleterow", "copyrow", "clonerow",
        "moverowup", "moverowdown", "cleardetail", "importdetail", "exportdetail", "detailbatch",
        "batch"
    };

    /// <summary>会计模块扩展按钮（继承通用，仅追加财务操作）。</summary>
    private static readonly string[] AccountingExtraNames =
    {
        "核算", "记账", "结账", "对账", "支付", "折旧", "报废", "报销", "冲销", "计提", "账期", "结转", "作废"
    };

    private static readonly string[] AccountingExtraPerms =
    {
        "calculate", "book", "closing", "reconcile", "payment", "depreciation", "scrap", "reimburse", "reverse", "accrual", "period", "carryforward", "void"
    };

    /// <summary>代码生成模块扩展按钮。</summary>
    private static readonly string[] CodeExtraNames =
    {
        "生成", "下载", "同步", "字段", "表", "数据库", "初始化", "克隆", "清空", "截断"
    };

    private static readonly string[] CodeExtraPerms =
    {
        "generate", "download", "sync", "columns", "tables", "databases", "initialize", "clone", "empty", "truncate"
    };

    /// <summary>基础设置模块扩展按钮（敏感词过滤/替换）。</summary>
    private static readonly string[] FoundationExtraNames = { "过滤", "替换" };
    private static readonly string[] FoundationExtraPerms = { "filter", "replace" };

    /// <summary>人力资源模块扩展按钮。</summary>
    private static readonly string[] HumanResourceExtraNames = { "核算" };
    private static readonly string[] HumanResourceExtraPerms = { "calculate" };

    /// <summary>身份认证模块扩展按钮。</summary>
    private static readonly string[] IdentityExtraNames =
    {
        "授权", "分配", "重置密码", "变更密码", "重置", "变更", "清空", "截断", "解锁", "禁用"
    };

    private static readonly string[] IdentityExtraPerms =
    {
        "authorize", "allocate", "resetpwd", "changepwd", "reset", "change", "empty", "truncate", "unlock", "disable"
    };

    /// <summary>日常事务模块扩展按钮（文档/社交/文件/系统等）。</summary>
    private static readonly string[] RoutineExtraNames =
    {
        "克隆", "复制",
        "保存草稿", "删除草稿", "发送", "撤回", "转发", "回复", "已读", "未读", "传阅", "签收", "催办", "确认",
        "点赞", "取消点赞", "收藏", "取消收藏", "分享", "取消分享", "评论", "取消评论", "举报", "取消举报", "关注", "取消关注",
        "上传", "下载", "归档", "销毁", "版本",
        "运行", "停止", "重启", "刷新", "重置", "清空",
        "转置"
    };

    private static readonly string[] RoutineExtraPerms =
    {
        "clone", "copy",
        "draft", "deletedraft", "send", "withdraw", "forward", "reply", "read", "unread", "circulate", "sign", "urge", "confirm",
        "like", "unlike", "favorite", "unfavorite", "share", "unshare", "comment", "uncomment", "flagging", "unflagging", "follow", "unfollow",
        "upload", "download", "archive", "destroy", "version",
        "run", "stop", "restart", "refresh", "reset", "empty",
        "transpose"
    };

    /// <summary>工作流模块扩展按钮（流程定义/实例/表单等）。</summary>
    private static readonly string[] WorkflowExtraNames =
    {
        "复制", "克隆",
        "暂停", "恢复", "提交", "撤回", "转办", "委托", "退回", "催办", "加签", "减签", "进度", "历史",
        "发布", "停用", "启用", "版本", "设计", "配置", "验证",
        "启动", "终止",
        "字段管理", "权限设置", "数据源配置", "主题设置", "表单数据",
        "流转归档", "流转清理"
    };

    private static readonly string[] WorkflowExtraPerms =
    {
        "copy", "clone",
        "suspend", "resume", "submit", "withdraw", "transfer", "delegate", "return", "urge", "addsign", "reducesign", "progress", "history",
        "publish", "disable", "enable", "version", "design", "config", "validate",
        "start", "terminate",
        "field", "permission", "datasource", "theme", "data",
        "archive", "clean"
    };

    /// <summary>统计看板模块扩展按钮（日志清理/归档、服务监控、报表统计等）。</summary>
    private static readonly string[] StatisticsExtraNames =
    {
        "刷新", "清空", "清空7天", "清空30天", "清空全部", "截断", "归档", "销毁", "清理",
        "下载", "同步", "核算", "转置", "重置", "运行", "停止", "重启"
    };

    private static readonly string[] StatisticsExtraPerms =
    {
        "refresh", "empty", "empty7d", "empty30d", "emptyall", "truncate", "archive", "destroy", "clean",
        "download", "sync", "calculate", "transpose", "reset", "run", "stop", "restart"
    };

    /// <summary>后勤/物料模块扩展按钮（未列入主模块顺序，仍继承通用）。</summary>
    private static readonly string[] LogisticsExtraNames =
    {
        "收货", "发货", "退货", "移库", "盘点", "调整", "报废"
    };

    private static readonly string[] LogisticsExtraPerms =
    {
        "receive", "issue", "return", "relocate", "count", "adjust", "scrap"
    };

    /// <summary>
    /// 合并通用按钮与模块扩展：扩展项插入在「批量」之前，保持通用段顺序一致。
    /// </summary>
    /// <param name="extraNames">模块特有按钮显示名称。</param>
    /// <param name="extraPerms">模块特有按钮权限后缀。</param>
    /// <returns>合并后的名称与权限后缀数组。</returns>
    private static (string[] names, string[] perms) MergeModuleButtons(string[] extraNames, string[] extraPerms)
    {
        if (extraNames.Length == 0)
            return (GenericButtonNames, GenericButtonPerms);
        if (extraNames.Length != extraPerms.Length)
            throw new InvalidOperationException("模块扩展按钮名称与权限后缀数量不一致。");
        var names = new string[GenericButtonNames.Length + extraNames.Length];
        var perms = new string[GenericButtonPerms.Length + extraPerms.Length];
        var insertIndex = GenericButtonNames.Length - 1;
        Array.Copy(GenericButtonNames, 0, names, 0, insertIndex);
        Array.Copy(extraNames, 0, names, insertIndex, extraNames.Length);
        names[^1] = GenericButtonNames[^1];
        Array.Copy(GenericButtonPerms, 0, perms, 0, insertIndex);
        Array.Copy(extraPerms, 0, perms, insertIndex, extraPerms.Length);
        perms[^1] = GenericButtonPerms[^1];
        return (names, perms);
    }

    /// <summary>
    /// 按模块前缀返回按钮显示名称数组与权限后缀（英文）数组；两者长度一致、一一对应。
    /// 各模块继承 <see cref="GenericButtonNames"/>，仅追加特有操作；匹配顺序：
    /// Accounting → Code → Foundation → HumanResource → Identity → Routine → Workflow → Statistics。
    /// </summary>
    /// <param name="modulePrefix">
    /// 模块前缀，如 <c>identity</c>、<c>workflow</c>、<c>routine</c> 等；
    /// 空或未知前缀时使用通用 CRUD 按钮组。
    /// </param>
    /// <returns>名称与权限后缀数组。</returns>
    private static (string[] names, string[] perms) GetButtonConfig(string modulePrefix)
    {
        return modulePrefix.ToLower() switch
        {
            "accounting" or "finance" => MergeModuleButtons(AccountingExtraNames, AccountingExtraPerms),
            "code" or "generator" => MergeModuleButtons(CodeExtraNames, CodeExtraPerms),
            "foundation" => MergeModuleButtons(FoundationExtraNames, FoundationExtraPerms),
            "humanresource" or "hr" => MergeModuleButtons(HumanResourceExtraNames, HumanResourceExtraPerms),
            "identity" => MergeModuleButtons(IdentityExtraNames, IdentityExtraPerms),
            "routine" => MergeModuleButtons(RoutineExtraNames, RoutineExtraPerms),
            "workflow" => MergeModuleButtons(WorkflowExtraNames, WorkflowExtraPerms),
            "statistics" => MergeModuleButtons(StatisticsExtraNames, StatisticsExtraPerms),
            "logistics" or "material" => MergeModuleButtons(LogisticsExtraNames, LogisticsExtraPerms),
            _ => (GenericButtonNames, GenericButtonPerms)
        };
    }

    /// <summary>
    /// 生成按钮编码：MenuCode + _ + buttonPerm，超过 50 字符时自动截断并附加哈希。
    /// </summary>
    private static string BuildButtonCode(string menuCode, string buttonPerm)
    {
        var code = $"{menuCode}_{buttonPerm.ToUpperInvariant()}";
        if (code.Length <= 50)
            return code;

        // 超过长度限制：截断 + 稳定哈希
        var hash = ComputeStableHash(code);
        return code.Substring(0, 43) + "_" + hash;
    }

    /// <summary>
    /// 计算字符串的稳定哈希（8 位十六进制）。
    /// </summary>
    private static string ComputeStableHash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes)[..8];
    }

    /// <summary>
    /// 创建或更新单个按钮菜单。
    /// </summary>
    /// <param name="menuRepository">菜单仓储。</param>
    /// <param name="parentId">父级页面菜单 ID。</param>
    /// <param name="buttonCode">按钮编码（业务键）。</param>
    /// <param name="buttonName">按钮显示名称。</param>
    /// <param name="permission">按钮权限字符串。</param>
    /// <param name="menuL10nKey">按钮本地化键。</param>
    /// <param name="sortOrder">按钮排序。</param>
    /// <returns>元组：(InsertCount, UpdateCount)，本条按钮新增或更新条数（0或1）。</returns>
    private static async Task<(int InsertCount, int UpdateCount)> CreateOrUpdateButtonAsync(
        ITaktTenantSeedRepository<TaktMenu> menuRepository,
        string tenantCode,
        long parentId,
        string buttonCode,
        string buttonName,
        string permission,
        string menuL10nKey,
        int sortOrder)
    {
        var button = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.MenuCode == buttonCode);

        if (button == null)
        {
            button = new TaktMenu
            {
                // 必须先设置 TenantCode（租户级实体）
                TenantCode = tenantCode,
                MenuName = buttonName,
                MenuCode = buttonCode,
                I18nKey = menuL10nKey,
                ParentId = parentId,
                MenuType = 2,  // Button
                Permission = permission,
                MenuStatus = 1,
                IsVisible = 1,
                SortOrder = sortOrder,
                IsCached = 0,
                IsExternal = 0,
                Level = 0,  // 稍后根据父级计算
                IsLeaf = 1,  // 按钮永远是叶子节点
                IsBuiltIn = TaktYesNo.Yes,
                CreatedBy = 900001,
                CreatedAt = DateTime.Now
            };

            button = await menuRepository.CreateAsync(button);

            // 更新 MenuPath 和 Level
            if (button.ParentId > 0)
            {
                var parentMenu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.Id == button.ParentId);
                if (parentMenu != null)
                {
                    button.MenuPath = $"{parentMenu.MenuPath}{button.Id}/";
                    button.Level = parentMenu.Level + 1;
                    
                    // 更新父级 IsLeaf 为非叶子（虽然按钮父级应该已经是非叶子）
                    if (parentMenu.IsLeaf == 1)
                    {
                        parentMenu.IsLeaf = 0;
                        await menuRepository.UpdateAsync(parentMenu);
                    }
                }
            }
            else
            {
                button.MenuPath = $"/{button.Id}/";
                button.Level = 1;
            }

            await menuRepository.UpdateAsync(button);
            return (1, 0);
        }
        else
        {
            button.MenuName = buttonName;
            button.I18nKey = menuL10nKey;
            button.Permission = permission;
            button.MenuStatus = 1;
            button.IsVisible = 1;
            button.SortOrder = sortOrder;
            button.IsBuiltIn = TaktYesNo.Yes;
            
            // 重新计算 Level 和 MenuPath（如果 ParentId 发生变化）
            if (button.ParentId != parentId)
            {
                button.ParentId = parentId;
                
                if (button.ParentId > 0)
                {
                    var parentMenu = await menuRepository.FirstAsync(m => m.TenantCode == tenantCode && m.Id == button.ParentId);
                    if (parentMenu != null)
                    {
                        button.MenuPath = $"{parentMenu.MenuPath}{button.Id}/";
                        button.Level = parentMenu.Level + 1;
                        
                        // 更新父级 IsLeaf 为非叶子
                        if (parentMenu.IsLeaf == 1)
                        {
                            parentMenu.IsLeaf = 0;
                            await menuRepository.UpdateAsync(parentMenu);
                        }
                    }
                }
                else
                {
                    button.MenuPath = $"/{button.Id}/";
                    button.Level = 1;
                }
            }
            
            button.UpdatedAt = DateTime.Now;
            button.UpdatedBy = 900001;

            await menuRepository.UpdateAsync(button);
            return (0, 1);
        }
    }
}
