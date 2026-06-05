// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktLogEnums.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：日志级别枚举（与前端 TaktLogLevel 数值对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Enums;

/// <summary>
/// 日志级别（数值越大优先级越高，与前端 utils/common.ts 一致）
/// </summary>
public enum TaktLogLevel
{
    /// <summary>
    /// 调试信息
    /// </summary>
    Debug = 0,

    /// <summary>
    /// 一般信息
    /// </summary>
    Info = 1,

    /// <summary>
    /// 警告
    /// </summary>
    Warn = 2,

    /// <summary>
    /// 错误
    /// </summary>
    Error = 3,

    /// <summary>
    /// 致命错误
    /// </summary>
    Fatal = 4
}
