// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktFileEnums.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：Foundation 文件实体相关枚举（与字典 sys_file_category / sys_storage_type / sys_is_public 对齐；状态用 TaktCommonStatus）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 文件分类（字典 sys_file_category）
/// </summary>
public enum TaktFileCategory
{
    /// <summary>
    /// 文档
    /// </summary>
    [Display(Name = "文档")]
    Document = 0,

    /// <summary>
    /// 图片
    /// </summary>
    [Display(Name = "图片")]
    Image = 1,

    /// <summary>
    /// 视频
    /// </summary>
    [Display(Name = "视频")]
    Video = 2,

    /// <summary>
    /// 音频
    /// </summary>
    [Display(Name = "音频")]
    Audio = 3,

    /// <summary>
    /// 压缩包
    /// </summary>
    [Display(Name = "压缩包")]
    Archive = 4,

    /// <summary>
    /// 其他
    /// </summary>
    [Display(Name = "其他")]
    Other = 5,
}

/// <summary>
/// 文件存储方式（字典 sys_storage_type）
/// </summary>
public enum TaktFileStorageType
{
    /// <summary>
    /// 本地存储
    /// </summary>
    [Display(Name = "本地存储")]
    Local = 0,

    /// <summary>
    /// OSS 对象存储
    /// </summary>
    [Display(Name = "OSS对象存储")]
    Oss = 1,

    /// <summary>
    /// FTP
    /// </summary>
    [Display(Name = "FTP")]
    Ftp = 2,

    /// <summary>
    /// 其他
    /// </summary>
    [Display(Name = "其他")]
    Other = 3,
}

/// <summary>
/// 文件公开范围（字典 sys_is_public；列名 is_public）
/// </summary>
public enum TaktFilePublicAccess
{
    /// <summary>
    /// 公开
    /// </summary>
    [Display(Name = "公开")]
    Public = 0,

    /// <summary>
    /// 私有（仅创建人可见、可修改、可下载）
    /// </summary>
    [Display(Name = "私有")]
    Private = 1,
}
