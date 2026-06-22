// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktFileAccessHelper.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：文件公开范围（IsPublic）与创建人维度的访问判定；0=公开，1=私有仅创建人
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 文件访问判定（IsPublic 字典 sys_is_public_type：0=公开，1=私有；不含 RBAC，RBAC 由控制器层校验）
/// </summary>
public static class TaktFileAccessHelper
{
    /// <summary>
    /// 当前用户是否可访问该文件（查看、修改、下载等）
    /// </summary>
    /// <param name="isPublic">公开（0=公开，1=私有）</param>
    /// <param name="createdBy">文件创建人用户 ID</param>
    /// <param name="currentUserId">当前登录用户 ID</param>
    /// <returns>公开文件为 true；私有文件仅创建人为 true</returns>
    public static bool CanAccess(int isPublic, long createdBy, long? currentUserId)
    {
        if (isPublic == 0)
        {
            return true;
        }

        return currentUserId is > 0 && createdBy == currentUserId.Value;
    }
}
