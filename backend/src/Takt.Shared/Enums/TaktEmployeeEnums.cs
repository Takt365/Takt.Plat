// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktEmployeeEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：员工相关枚举
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 性别枚举
/// </summary>
public enum TaktGenderType
{
    /// <summary>
    /// 未知
    /// </summary>
    [Display(Name = "未知")]
    Unknown = 0,

    /// <summary>
    /// 男
    /// </summary>
    [Display(Name = "男")]
    Male = 1,

    /// <summary>
    /// 女
    /// </summary>
    [Display(Name = "女")]
    Female = 2
}

/// <summary>
/// 婚姻状况枚举
/// </summary>
public enum TaktMaritalStatus
{
    /// <summary>
    /// 未婚
    /// </summary>
    [Display(Name = "未婚")]
    Single = 0,

    /// <summary>
    /// 已婚
    /// </summary>
    [Display(Name = "已婚")]
    Married = 1,

    /// <summary>
    /// 离异
    /// </summary>
    [Display(Name = "离异")]
    Divorced = 2,

    /// <summary>
    /// 丧偶
    /// </summary>
    [Display(Name = "丧偶")]
    Widowed = 3
}

/// <summary>
/// 学历枚举
/// </summary>
public enum TaktEducationLevel
{
    /// <summary>
    /// 初中及以下
    /// </summary>
    [Display(Name = "初中及以下")]
    JuniorHigh = 0,

    /// <summary>
    /// 高中/中专
    /// </summary>
    [Display(Name = "高中/中专")]
    HighSchool = 1,

    /// <summary>
    /// 大专
    /// </summary>
    [Display(Name = "大专")]
    College = 2,

    /// <summary>
    /// 本科
    /// </summary>
    [Display(Name = "本科")]
    Bachelor = 3,

    /// <summary>
    /// 硕士
    /// </summary>
    [Display(Name = "硕士")]
    Master = 4,

    /// <summary>
    /// 博士
    /// </summary>
    [Display(Name = "博士")]
    Doctor = 5,

    /// <summary>
    /// 博士后
    /// </summary>
    [Display(Name = "博士后")]
    Postdoctoral = 6
}

/// <summary>
/// 员工状态枚举
/// </summary>
public enum TaktEmployeeStatus
{
    /// <summary>
    /// 试用期
    /// </summary>
    [Display(Name = "试用期")]
    Probation = 0,

    /// <summary>
    /// 正式
    /// </summary>
    [Display(Name = "正式")]
    Regular = 1,

    /// <summary>
    /// 离职
    /// </summary>
    [Display(Name = "离职")]
    Resigned = 2,

    /// <summary>
    /// 停职
    /// </summary>
    [Display(Name = "停职")]
    Suspended = 3,

    /// <summary>
    /// 退休
    /// </summary>
    [Display(Name = "退休")]
    Retired = 4
}
