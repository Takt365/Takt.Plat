// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktIsoCode.cs
// 创建时间：2026-06-14
// 创建人：Takt365(Cursor AI)
// 功能描述：ISO 编码主数据（租户内共享），供编号规则、单据编码等段引用
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// ISO 编码实体
/// 维护租户内标准短码（如 Eng、Pmc、D1000），用于编号规则、单据编码等段引用
/// </summary>
[SugarTable("takt_foundation_iso_code", "ISO编码表")]
[SugarIndex("ix_iso_code_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_iso_code_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_iso_code_category_unique", nameof(TenantCode), OrderByType.Asc, nameof(IsoCodeCategory), OrderByType.Asc, nameof(IsoCode), OrderByType.Asc, true)]
public class TaktIsoCode : TaktTenantEntityBase
{    /// <summary>
    /// 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
    /// </summary>
    [SugarColumn(ColumnName = "iso_code_category", ColumnDescription = "编码类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsoCodeCategory { get; set; } = 1;
    /// <summary>
    /// ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编号规则等段引用，如 Eng、Pmc、D1000）
    /// </summary>
    [SugarColumn(ColumnName = "iso_code", ColumnDescription = "ISO编码", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string IsoCode { get; set; } = string.Empty;
    /// <summary>
    /// ISO 名称（如：技术、生管、总经理室）
    /// </summary>
    [SugarColumn(ColumnName = "iso_name", ColumnDescription = "ISO名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string IsoName { get; set; } = string.Empty;
    /// <summary>
    /// 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;
    /// <summary>
    /// 描述说明
    /// </summary>
    [SugarColumn(ColumnName = "iso_code_description", ColumnDescription = "描述说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? IsoCodeDescription { get; set; }
    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "iso_code_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsoCodeStatus { get; set; } = 1;
}
