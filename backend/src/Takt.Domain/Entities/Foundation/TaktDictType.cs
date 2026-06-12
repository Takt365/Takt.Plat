// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktDictType.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：字典类型实体，如：订单状态、用户类型、审批状态等
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 字典类型实体
/// 用于定义系统中使用的各种字典分类，如：订单状态、用户类型、审批状态等
/// 租户级实体：字典类型在租户内共享，不需要公司隔离
/// </summary>
[SugarTable("takt_foundation_dict_type", "字典类型表")]
[SugarIndex("ix_dict_type_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_dict_type_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_dict_type_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(DictTypeCode), OrderByType.Asc, true)]
public class TaktDictType : TaktTenantEntityBase
{
    /// <summary>
    /// 字典类型编码（唯一索引：租户内唯一，见 ix_dict_type_code_unique；如 order_status, user_type）
    /// </summary>
    [SugarColumn(ColumnName = "dict_type_code", ColumnDescription = "字典类型编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型名称（如：订单状态、用户类型）
    /// </summary>
    [SugarColumn(ColumnName = "dict_type_name", ColumnDescription = "字典类型名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string DictTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（0=表数据，1=SQL脚本）
    /// </summary>
    [SugarColumn(ColumnName = "data_source", ColumnDescription = "数据源", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DataSource { get; set; } = 0;

    /// <summary>
    /// 动态字典SQL脚本（仅当DataSource=SqlScript时使用）
    /// SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
    /// </summary>
    [SugarColumn(ColumnName = "dict_script", ColumnDescription = "动态字典SQL脚本", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? DictScript { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 是否内置（1=是，0=否）
    /// 内置字典不允许删除和修改核心字段
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "是否内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "dict_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int DictStatus { get; set; } = 1;


    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 字典数据列表（一对多关联）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktDictData.DictTypeCode))]
    public List<TaktDictData>? DictDataList { get; set; }
}
