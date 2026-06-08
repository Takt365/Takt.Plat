// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktStatisticsEnums.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：统计报表域枚举（自研 SQVI 式自定义报表）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 自定义报表业务域（与 statistics/report 菜单层级对齐）
/// </summary>
public enum TaktConfigurableDomain
{
    /// <summary>
    /// 通用
    /// </summary>
    [Display(Name = "通用")]
    General = 0,

    /// <summary>
    /// 财务统计
    /// </summary>
    [Display(Name = "财务统计")]
    Financial = 1,

    /// <summary>
    /// 人力统计
    /// </summary>
    [Display(Name = "人力统计")]
    HumanResource = 2,

    /// <summary>
    /// 后勤统计
    /// </summary>
    [Display(Name = "后勤统计")]
    Logistics = 3,
}

/// <summary>
/// 报表多表关联类型（JOIN）
/// </summary>
public enum TaktConfigurableJoinType
{
    /// <summary>
    /// 内连接
    /// </summary>
    [Display(Name = "内连接")]
    Inner = 1,

    /// <summary>
    /// 左连接
    /// </summary>
    [Display(Name = "左连接")]
    Left = 2,

    /// <summary>
    /// 右连接
    /// </summary>
    [Display(Name = "右连接")]
    Right = 3,

    /// <summary>
    /// 全连接
    /// </summary>
    [Display(Name = "全连接")]
    Full = 4,
}

/// <summary>
/// 报表筛选比较符（Selection Screen）
/// </summary>
public enum TaktConfigurableFilterOperator
{
    /// <summary>
    /// 等于
    /// </summary>
    [Display(Name = "等于")]
    Equal = 1,

    /// <summary>
    /// 不等于
    /// </summary>
    [Display(Name = "不等于")]
    NotEqual = 2,

    /// <summary>
    /// 大于
    /// </summary>
    [Display(Name = "大于")]
    GreaterThan = 3,

    /// <summary>
    /// 大于等于
    /// </summary>
    [Display(Name = "大于等于")]
    GreaterThanOrEqual = 4,

    /// <summary>
    /// 小于
    /// </summary>
    [Display(Name = "小于")]
    LessThan = 5,

    /// <summary>
    /// 小于等于
    /// </summary>
    [Display(Name = "小于等于")]
    LessThanOrEqual = 6,

    /// <summary>
    /// 包含（LIKE）
    /// </summary>
    [Display(Name = "包含")]
    Contains = 7,

    /// <summary>
    /// 区间（BETWEEN）
    /// </summary>
    [Display(Name = "区间")]
    Between = 8,

    /// <summary>
    /// 在列表中（IN）
    /// </summary>
    [Display(Name = "在列表中")]
    In = 9,

    /// <summary>
    /// 为空
    /// </summary>
    [Display(Name = "为空")]
    IsNull = 10,

    /// <summary>
    /// 不为空
    /// </summary>
    [Display(Name = "不为空")]
    IsNotNull = 11,
}

/// <summary>
/// 报表输出字段聚合函数（配合 GROUP BY）
/// </summary>
public enum TaktConfigurableAggregateFunc
{
    /// <summary>
    /// 无聚合（原值输出）
    /// </summary>
    [Display(Name = "无")]
    None = 0,

    /// <summary>
    /// 计数
    /// </summary>
    [Display(Name = "计数")]
    Count = 1,

    /// <summary>
    /// 求和
    /// </summary>
    [Display(Name = "求和")]
    Sum = 2,

    /// <summary>
    /// 平均值
    /// </summary>
    [Display(Name = "平均值")]
    Avg = 3,

    /// <summary>
    /// 最小值
    /// </summary>
    [Display(Name = "最小值")]
    Min = 4,

    /// <summary>
    /// 最大值
    /// </summary>
    [Display(Name = "最大值")]
    Max = 5,
}

/// <summary>
/// 报表排序方向
/// </summary>
public enum TaktConfigurableSortDirection
{
    /// <summary>
    /// 升序
    /// </summary>
    [Display(Name = "升序")]
    Asc = 1,

    /// <summary>
    /// 降序
    /// </summary>
    [Display(Name = "降序")]
    Desc = 2,
}
