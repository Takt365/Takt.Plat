// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Shared.Enums
// 文件名称：TaktCommonEnums.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：全项目通用枚举（状态、费用类别等）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Enums;

/// <summary>
/// 是否枚举（布尔值的业务表达）
/// </summary>
public enum TaktYesNo
{
    /// <summary>
    /// 否
    /// </summary>
    [Display(Name = "否")]
    No = 0,

    /// <summary>
    /// 是
    /// </summary>
    [Display(Name = "是")]
    Yes = 1
}

/// <summary>
/// 通用状态枚举（全局通用）
/// </summary>
public enum TaktCommonStatus
{
    /// <summary>
    /// 禁用/停用
    /// </summary>
    [Display(Name = "禁用")]
    Disabled = 0,

    /// <summary>
    /// 启用/正常
    /// </summary>
    [Display(Name = "启用")]
    Enabled = 1
}

/// <summary>
/// 费用类别枚举（全局通用）
/// 用于部门、成本中心等实体的费用分类
/// </summary>
public enum TaktCostCategory
{
    /// <summary>
    /// 直接费用（直接计入产品成本，如：直接材料、直接人工）
    /// </summary>
    [Display(Name = "直接")]
    Direct = 1,

    /// <summary>
    /// 间接费用（需分摊计入产品成本，如：制造费用、管理费用）
    /// </summary>
    [Display(Name = "间接")]
    Indirect = 2
}

/// <summary>
/// 项目模块枚举（全局通用）
/// </summary>
public enum TaktModule
{
    /// <summary>
    /// 仪表盘
    /// </summary>
    [Display(Name = "仪表盘")]
    Dashboard = 0,

    /// <summary>
    /// 身份认证
    /// </summary>
    [Display(Name = "身份认证")]
    Identity = 1,

    /// <summary>
    /// 日常事务
    /// </summary>
    [Display(Name = "日常事务")]
    Routine = 2,

    /// <summary>
    /// 财务核算
    /// </summary>
    [Display(Name = "财务核算")]
    Accounting = 3,

    /// <summary>
    /// 后勤管理
    /// </summary>
    [Display(Name = "后勤管理")]
    Logistics = 4,

    /// <summary>
    /// 人力资源
    /// </summary>
    [Display(Name = "人力资源")]
    HumanResource = 5,

    /// <summary>
    /// 工作流
    /// </summary>
    [Display(Name = "工作流")]
    Workflow = 6,

    /// <summary>
    /// 代码管理
    /// </summary>
    [Display(Name = "代码管理")]
    Code = 7,

    /// <summary>
    /// 基础设置
    /// </summary>
    [Display(Name = "基础设置")]
    Foundation = 8,

    /// <summary>
    /// 统计看板
    /// </summary>
    [Display(Name = "统计看板")]
    Statistics = 9,

    /// <summary>
    /// 实体字段（Domain 实体元数据翻译）
    /// </summary>
    [Display(Name = "实体")]
    Entity = 10
}

/// <summary>
/// 数据权限范围枚举（全局通用）
/// </summary>
public enum TaktDataScope
{
    /// <summary>
    /// 全部数据
    /// </summary>
    [Display(Name = "全部数据")]
    All = 1,

    /// <summary>
    /// 本公司数据
    /// </summary>
    [Display(Name = "本公司数据")]
    Company = 2,

    /// <summary>
    /// 本部门数据
    /// </summary>
    [Display(Name = "本部门数据")]
    Department = 3,

    /// <summary>
    /// 仅本人数据
    /// </summary>
    [Display(Name = "仅本人数据")]
    Self = 4,

    /// <summary>
    /// 自定义范围
    /// </summary>
    [Display(Name = "自定义")]
    Custom = 5
}

/// <summary>
/// 审批状态枚举（全局通用）
/// </summary>
public enum TaktApprovalStatus
{
    /// <summary>
    /// 待审批
    /// </summary>
    [Display(Name = "待审批")]
    Pending = 0,

    /// <summary>
    /// 审批中（处理中）
    /// </summary>
    [Display(Name = "审批中")]
    InProgress = 1,

    /// <summary>
    /// 已通过
    /// </summary>
    [Display(Name = "已通过")]
    Approved = 2,

    /// <summary>
    /// 已驳回
    /// </summary>
    [Display(Name = "已驳回")]
    Rejected = 3,

    /// <summary>
    /// 已撤销（申请人主动撤回）
    /// </summary>
    [Display(Name = "已撤销")]
    Cancelled = 4,

    /// <summary>
    /// 已终止（强制结束，非撤回）
    /// </summary>
    [Display(Name = "已终止")]
    Terminated = 5
}

/// <summary>
/// 紧急程度枚举（全局通用）
/// </summary>
public enum TaktUrgencyLevel
{
    /// <summary>
    /// 普通
    /// </summary>
    [Display(Name = "普通")]
    Normal = 0,

    /// <summary>
    /// 紧急
    /// </summary>
    [Display(Name = "紧急")]
    Urgent = 1,

    /// <summary>
    /// 非常紧急
    /// </summary>
    [Display(Name = "非常紧急")]
    Critical = 2
}

/// <summary>
/// 优先级别枚举（全局通用）
/// </summary>
public enum TaktPriorityLevel
{
    /// <summary>
    /// 低
    /// </summary>
    [Display(Name = "低")]
    Low = 0,

    /// <summary>
    /// 中
    /// </summary>
    [Display(Name = "中")]
    Medium = 1,

    /// <summary>
    /// 高
    /// </summary>
    [Display(Name = "高")]
    High = 2,

    /// <summary>
    /// 最高
    /// </summary>
    [Display(Name = "最高")]
    Highest = 3
}

/// <summary>
/// 数据源枚举（用于字典等基础数据）
/// </summary>
public enum TaktDataSource
{
    /// <summary>
    /// 表数据（通过数据表手动维护）
    /// </summary>
    [Display(Name = "表数据")]
    TableData = 0,

    /// <summary>
    /// SQL脚本（通过 SQL 脚本动态生成）
    /// </summary>
    [Display(Name = "SQL脚本")]
    SqlScript = 1
}

/// <summary>
/// SQL 执行模式（<see cref="TaktSqlExecuteOptions"/>）
/// </summary>
public enum TaktSqlExecuteMode
{
    /// <summary>只读查询（仅 SELECT / WITH，禁止 DML/DDL）</summary>
    [Display(Name = "只读查询")]
    ReadOnly = 0,
}

/// <summary>
/// 应用端枚举（全局通用：前端/后端）
/// </summary>
public enum TaktAppSide
{
    /// <summary>
    /// 前端
    /// </summary>
    [Display(Name = "前端")]
    Frontend = 0,

    /// <summary>
    /// 后端
    /// </summary>
    [Display(Name = "后端")]
    Backend = 1
}

/// <summary>
/// 权限类型枚举（全局通用）
/// </summary>
public enum TaktPermissionType
{
    /// <summary>
    /// 菜单权限（页面访问权限）
    /// </summary>
    [Display(Name = "菜单权限")]
    Menu = 1,

    /// <summary>
    /// 按钮权限（操作权限）
    /// </summary>
    [Display(Name = "按钮权限")]
    Button = 2,

    /// <summary>
    /// 数据权限（数据访问范围）
    /// </summary>
    [Display(Name = "数据权限")]
    Data = 3
}
