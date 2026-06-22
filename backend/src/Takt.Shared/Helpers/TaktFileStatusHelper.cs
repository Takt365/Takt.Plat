// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktFileStatusHelper.cs
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：文件状态（字典 sys_normal_disable_status）常量与判定；1=启用，0=禁用，2=锁定
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 文件状态工具（与 TaktFile.FileStatus、字典 sys_normal_disable_status 对齐）
/// </summary>
public static class TaktFileStatusHelper
{
    /// <summary>禁用（字典 sys_normal_disable_status=0）</summary>
    public const int Disabled = 0;

    /// <summary>启用（字典 sys_normal_disable_status=1）</summary>
    public const int Enabled = 1;

    /// <summary>锁定（字典 sys_normal_disable_status=2）</summary>
    public const int Locked = 2;

    /// <summary>
    /// 是否为启用态（仅 1 可下载）
    /// </summary>
    /// <param name="fileStatus">文件状态</param>
    /// <returns>启用返回 true</returns>
    public static bool IsEnabled(int fileStatus) => fileStatus == Enabled;

    /// <summary>
    /// 是否为合法字典值（0/1/2）
    /// </summary>
    /// <param name="fileStatus">文件状态</param>
    /// <returns>合法返回 true</returns>
    public static bool IsValidStatus(int fileStatus) =>
        fileStatus is Disabled or Enabled or Locked;

    /// <summary>
    /// 解析上传/表单传入的状态；非法或未传时默认启用
    /// </summary>
    /// <param name="fileStatus">可选状态</param>
    /// <returns>有效状态值</returns>
    public static int NormalizeOrDefault(int? fileStatus)
    {
        if (!fileStatus.HasValue || !IsValidStatus(fileStatus.Value))
        {
            return Enabled;
        }

        return fileStatus.Value;
    }
}
