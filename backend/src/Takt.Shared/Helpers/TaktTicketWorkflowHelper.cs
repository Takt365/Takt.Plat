// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktTicketWorkflowHelper.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：工单 ITSM 状态机流转校验（纯函数，无 I/O）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 工单工作流状态机辅助类
/// </summary>
public static class TaktTicketWorkflowHelper
{
    /// <summary>
    /// 编号规则编码（对接 TaktNumbering.RuleCode）
    /// </summary>
    public const string TicketNumberRuleCode = "HD-TICKET";

    /// <summary>
    /// 将库内 int 规范化为合法工单状态
    /// </summary>
    /// <param name="status">库内状态值</param>
    /// <returns>合法状态 int</returns>
    public static int NormalizeLegacyStatus(int status) => status is >= 0 and <= 6 ? status : 0;

    /// <summary>
    /// 导入/迁移旧版 4 态（0→Open，1→InProgress，2→Resolved，3→Closed）
    /// </summary>
    /// <param name="legacyStatus">旧版状态 int</param>
    /// <returns>新状态 int</returns>
    public static int MapLegacyImportStatus(int legacyStatus)
    {
        return legacyStatus switch
        {
            0 => 0,
            1 => 2,
            2 => 4,
            3 => 5,
            _ => NormalizeLegacyStatus(legacyStatus),
        };
    }

    /// <summary>
    /// 校验状态流转是否合法
    /// </summary>
    /// <param name="current">当前状态</param>
    /// <param name="target">目标状态</param>
    /// <returns>是否允许流转</returns>
    public static bool CanTransition(int current, int target)
    {
        if (current == target)
        {
            return false;
        }
        return (current, target) switch
        {
            (0, 1) => true,
            (0, 2) => true,
            (6, 1) => true,
            (6, 2) => true,
            (1, 2) => true,
            (2, 3) => true,
            (2, 4) => true,
            (3, 2) => true,
            (4, 5) => true,
            (4, 6) => true,
            (5, 6) => true,
            _ => false,
        };
    }

    /// <summary>
    /// 是否允许客服领取/指派
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可领取</returns>
    public static bool CanPickOrAssign(int status)
    {
        return status is 0 or 6;
    }

    /// <summary>
    /// 是否允许开始处理
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可开始</returns>
    public static bool CanStartProgress(int status)
    {
        return status == 1;
    }

    /// <summary>
    /// 是否允许请求用户补充信息
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可等待用户</returns>
    public static bool CanWaitForRequester(int status)
    {
        return status == 2;
    }

    /// <summary>
    /// 是否允许标记已解决
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可解决</returns>
    public static bool CanResolve(int status)
    {
        return status == 2;
    }

    /// <summary>
    /// 是否允许用户确认关闭
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可确认关闭</returns>
    public static bool CanConfirmClose(int status)
    {
        return status == 4;
    }

    /// <summary>
    /// 是否允许重新打开
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可重开</returns>
    public static bool CanReopen(int status)
    {
        return status is 4 or 5;
    }

    /// <summary>
    /// 用户回复后是否应回到处理中
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <param name="authorType">回复作者类型</param>
    /// <returns>是否自动回到处理中</returns>
    public static bool ShouldResumeAfterReply(int status, int authorType)
    {
        return status == 3 && authorType == 1;
    }
}
