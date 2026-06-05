// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktPostEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：岗位相关枚举
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 岗位类型枚举
/// </summary>
public enum TaktPostType
{
    /// <summary>
    /// 管理岗
    /// </summary>
    [Display(Name = "管理岗")]
    Management = 0,

    /// <summary>
    /// 技术岗
    /// </summary>
    [Display(Name = "技术岗")]
    Technical = 1,

    /// <summary>
    /// 业务岗
    /// </summary>
    [Display(Name = "业务岗")]
    Business = 2,

    /// <summary>
    /// 职能岗
    /// </summary>
    [Display(Name = "职能岗")]
    Functional = 3,

    /// <summary>
    /// 操作岗
    /// </summary>
    [Display(Name = "操作岗")]
    Operation = 4,

    /// <summary>
    /// 品质岗
    /// </summary>
    [Display(Name = "品质岗")]
    Quality = 5,

    /// <summary>
    /// 保安岗
    /// </summary>
    [Display(Name = "保安岗")]
    Security = 6,

    /// <summary>
    /// 后勤岗
    /// </summary>
    [Display(Name = "后勤岗")]
    Logistics = 7
}

/// <summary>
/// 岗位职级枚举
/// 制造业三层职级体系
/// </summary>
public enum TaktPostLevel
{
    /// <summary>
    /// 一线/基层
    /// 普工/作业员、多能工/熟手、质检员（QC）/仓管员
    /// </summary>
    [Display(Name = "一线/基层")]
    Frontline = 0,

    /// <summary>
    /// 技术/骨干层
    /// 技术员、工程师（IE/PE/ME）、高级工程师
    /// </summary>
    [Display(Name = "技术/骨干层")]
    Technical = 1,

    /// <summary>
    /// 管理/决策层
    /// 班组长/拉长、主管（课长）、经理、厂长/总监
    /// </summary>
    [Display(Name = "管理/决策层")]
    Management = 2
}
