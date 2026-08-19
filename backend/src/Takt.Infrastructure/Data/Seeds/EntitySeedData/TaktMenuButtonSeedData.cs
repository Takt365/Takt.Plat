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
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// Takt 按钮菜单种子数据。
/// <para>
/// 在各级页面菜单（Level1～Level5）种子执行完毕后，由 TaktMenuSeedData 调用。
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
    /// <param name="serviceProvider">服务提供者，用于解析 ITaktRepository{TaktMenu}。</param>
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
    /// 从 TaktMenu.Permission 解析出的模块前缀（冒号分隔第一段的小写形式）。
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

            // 生成本地化键：common.page.button.{操作后缀}（与 TaktCommonI18nSeedData、前端 t() 一致）
            var menuL10nKey = TaktCommonI18nKeys.MenuButton(buttonPerm);

            var (insert, update) = await CreateOrUpdateButtonAsync(
                menuRepository, tenantCode, menu.Id, buttonCode, buttonName, permission, menuL10nKey, i + 1);
            insertCount += insert;
            updateCount += update;
        }

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 从完整权限字符串中提取模块前缀（第一段），用于 GetButtonConfig 分支。
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

        if (parts.Length >= 2
            && parts[0].Equals("human", StringComparison.OrdinalIgnoreCase)
            && parts[1].Equals("resource", StringComparison.OrdinalIgnoreCase))
            return "human:resource";

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
        if (parts.Length >= 2
            && parts[0].Equals("human", StringComparison.OrdinalIgnoreCase)
            && parts[1].Equals("resource", StringComparison.OrdinalIgnoreCase))
        {
            if (parts.Length > 3)
                return string.Join(":", parts[2..^1]).ToLowerInvariant();
            if (parts.Length == 3)
                return parts[2].ToLowerInvariant();
            return string.Empty;
        }

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
    /// 通用 CRUD 按钮显示名称（分组：CRUD → 导入导出 → 审批流 → 复制克隆 → 树表 → 明细行 → 其他；末项固定「批量」）。
    /// 「撤销」(revoke) 与「撤回」(withdraw) 为全项目共用后缀，模块扩展不得重复定义。
    /// </summary>
    private static readonly string[] GenericButtonNames =
    {
        "查询", "新增", "修改", "删除", "详情", "预览", "打印",
        "导入", "导出", "模板",
        "审批", "撤销", "撤回",
        "复制", "克隆",
        "树", "选项", "展开", "收缩", "插入", "移动", "上移", "下移",
        "新增行", "插入行", "编辑行", "修改行", "删除行", "复制行", "克隆行", "行上移", "行下移",
        "主题设置",
        "批量"
    };

    /// <summary>通用 CRUD 按钮权限后缀（与 GenericButtonNames 一一对应；I18nKey 为 common.page.button.*）。</summary>
    private static readonly string[] GenericButtonPerms =
    {
        "query", "create", "update", "delete", "detail", "preview", "print",
        "import", "export", "template",
        "approve", "revoke", "withdraw",
        "copy", "clone",
        "tree", "options", "expand", "collapse", "insert", "move", "moveup", "movedown",
        "createrow", "insertrow", "editrow", "updaterow", "deleterow", "copyrow", "clonerow", "moverowup", "moverowdown",
        "theme",
        "batch"
    };

    /// <summary>会计模块扩展按钮（分组：账期结账 → 核算记账 → 对账支付 → 报销 → 资产 → 冲销作废）。</summary>
    private static readonly string[] AccountingExtraNames =
    {
        "账期", "结账", "结转",
        "核算", "记账", "计提",
        "对账", "支付",
        "报销",
        "折旧", "报废",
        "冲销", "作废"
    };

    private static readonly string[] AccountingExtraPerms =
    {
        "period", "closing", "carryforward",
        "calculate", "book", "accrual",
        "reconcile", "payment",
        "reimburse",
        "depreciation", "scrap",
        "reverse", "void"
    };

    /// <summary>代码生成模块扩展按钮（分组：生成下载 → 同步元数据 → 初始化克隆 → 数据表归档 → 备份调度 → 清空截断）。</summary>
    private static readonly string[] CodeExtraNames =
    {
        "生成", "下载",
        "同步", "字段", "表", "数据库",
        "初始化", "克隆",
        "归档",
        "立即备份", "后台备份",
        "清空", "截断"
    };

    private static readonly string[] CodeExtraPerms =
    {
        "generate", "download",
        "sync", "columns", "tables", "databases",
        "initialize", "clone",
        "archive",
        "run", "schedule",
        "empty", "truncate"
    };

    /// <summary>基础设置模块扩展按钮（分组：敏感词 → 在线/SignalR → 消息 → Quartz → 文件 → 重置/转置）。</summary>
    private static readonly string[] FoundationExtraNames =
    {
        "过滤", "替换",
        "强退", "统计",
        "发送", "广播", "已读", "未读",
        "执行", "启动", "暂停", "停止",
        "上传", "分片", "合并", "检查", "下载",
        "重置", "转置"
    };

    private static readonly string[] FoundationExtraPerms =
    {
        "filter", "replace",
        "kick", "stats",
        "send", "broadcast", "read", "unread",
        "execute", "start", "pause", "stop",
        "upload", "chunk", "merge", "check", "download",
        "reset", "transpose"
    };

    /// <summary>人力资源模块扩展按钮（员工生命周期：入职 → 转正 → 调动 → 晋升 → 离职 → 返聘）。</summary>
    private static readonly string[] HumanResourceExtraNames =
    {
        "入职", "转正", "调转", "晋升", "离职", "返聘"
    };

    private static readonly string[] HumanResourceExtraPerms =
    {
        "onboard", "regularize", "transfer", "promote", "terminate", "rehire"
    };

    /// <summary>身份认证模块扩展按钮（分组：授权分配 → 密码 → 账号状态 → 重置/变更）。</summary>
    private static readonly string[] IdentityExtraNames =
    {
        "授权", "分配",
        "重置密码", "变更密码",
        "解锁", "禁用",
        "重置", "变更"
    };

    private static readonly string[] IdentityExtraPerms =
    {
        "authorize", "allocate",
        "resetpwd", "changepwd",
        "unlock", "disable",
        "reset", "change"
    };

    /// <summary>日常事务模块扩展按钮（分组：复制 → 文档流转 → 社交互动 → 文件 → 排序 → 系统 → 转置）。</summary>
    private static readonly string[] RoutineExtraNames =
    {
        "克隆", "复制",
        "草稿", "删除草稿", "发送", "转发", "回复", "已读", "未读", "传阅", "签收", "催办", "确认",
        "点赞", "取消点赞", "收藏", "取消收藏", "分享", "取消分享", "评论", "取消评论", "举报", "取消举报", "关注", "取消关注",
        "上传", "下载", "归档", "销毁", "版本",
        "置顶", "置底",
        "运行", "停止", "重启", "刷新", "重置", "清空",
        "转置"
    };

    private static readonly string[] RoutineExtraPerms =
    {
        "clone", "copy",
        "draft", "deletedraft", "send", "forward", "reply", "read", "unread", "circulate", "sign", "urge", "confirm",
        "like", "unlike", "favorite", "unfavorite", "share", "unshare", "comment", "uncomment", "flagging", "unflagging", "follow", "unfollow",
        "upload", "download", "archive", "destroy", "version",
        "movetop", "movebottom",
        "run", "stop", "restart", "refresh", "reset", "empty",
        "transpose"
    };

    /// <summary>
    /// 工作流模块扩展按钮（分组：实例发起 → 待办操作 → 实例管控 → 方案定义 → 规划项）。
    /// 运行时与 <c>TaktFlowEngineController</c> 对齐；「撤销」(revoke)/「撤回」(withdraw) 仅 Generic 定义。
    /// 规划项（claim/cc/return/delegate 等）引擎未实现前仅种子授权，见 09-workflow §十。
    /// </summary>
    private static readonly string[] WorkflowExtraNames =
    {
        "启动", "发起", "草稿", "提交",
        "转办", "加签", "减签",
        "挂起", "恢复", "终止",
        "设计", "发布", "验证", "部署",
        "认领", "抄送", "退回", "委托", "跳转", "跟踪", "释放", "发起人", "切换"
    };

    /// <summary>工作流模块扩展按钮权限后缀（与 WorkflowExtraNames 一一对应）。</summary>
    private static readonly string[] WorkflowExtraPerms =
    {
        "start", "initiate", "draft", "submit",
        "transfer", "addsign", "reducesign",
        "suspend", "resume", "terminate",
        "design", "publish", "validate", "deploy",
        "claim", "cc", "return", "delegate", "jump", "trace", "release", "initiator", "toggle"
    };

    /// <summary>统计看板模块扩展按钮（分组：刷新 → 日志清理 → 归档销毁 → 导出同步 → 核算转置 → 系统控制）。</summary>
    private static readonly string[] StatisticsExtraNames =
    {
        "刷新",
        "清空", "清理", "截断",
        "归档", "销毁",
        "下载", "同步",
        "核算", "转置",
        "重置", "运行", "停止", "重启"
    };

    private static readonly string[] StatisticsExtraPerms =
    {
        "refresh",
        "empty", "clean", "truncate",
        "archive", "destroy",
        "download", "sync",
        "calculate", "transpose",
        "reset", "run", "stop", "restart"
    };

    /// <summary>
    /// 后勤/物料/制造模块扩展按钮（分组：收发货 → 库内作业 → 领借还 → 调拨核销 → 制造编排通用操作）。
    /// 权限末段用通用操作：generate / run / publish / release / schedule / convertto。
    /// </summary>
    private static readonly string[] LogisticsExtraNames =
    {
        "收货", "发货", "退货",
        "移库", "盘点", "调整", "报废",
        "领用", "借调", "归还", "报损",
        "调拨", "核销",
        "生成", "运行", "发布", "释放", "排程", "转换"
    };

    private static readonly string[] LogisticsExtraPerms =
    {
        "receive", "shipping", "returns",
        "transfer", "stocktake", "adjust", "scrap",
        "requisition", "secondment", "restore", "lossreport",
        "allot", "writeoff",
        "generate", "run", "publish", "release", "schedule", "convertto"
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
    /// 各模块继承 GenericButtonNames，仅追加特有操作；匹配顺序：
    /// Accounting → Code → Foundation → HumanResource → Identity → Routine → Workflow → Statistics → Logistics。
    /// 禁止再按单个 list 菜单特立 Append* 追加权限后缀。
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
            "human:resource" or "humanresource" or "hr" => MergeModuleButtons(HumanResourceExtraNames, HumanResourceExtraPerms),
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
        // 父级 MenuCode 变更后旧按钮仍挂同一 ParentId，按权限末段回收，避免重复插入
        if (button == null)
        {
            var permSuffix = permission.Contains(':')
                ? permission[permission.LastIndexOf(':')..]
                : $":{permission}";
            button = await menuRepository.FirstAsync(m =>
                m.TenantCode == tenantCode
                && m.ParentId == parentId
                && m.MenuType == 2
                && m.Permission != null
                && m.Permission.EndsWith(permSuffix));
        }

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
                IsBuiltIn = 1,
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
            button.MenuCode = buttonCode;
            button.I18nKey = menuL10nKey;
            button.Permission = permission;
            button.MenuStatus = 1;
            button.IsVisible = 1;
            button.SortOrder = sortOrder;
            button.IsBuiltIn = 1;
            
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
