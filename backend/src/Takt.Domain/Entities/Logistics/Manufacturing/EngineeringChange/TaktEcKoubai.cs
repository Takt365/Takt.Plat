// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcKoubai.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变采购课部门执行表（每设变明细一行）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变采购课（D0510）部门执行表
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_koubai", "设变采购执行表")]
[SugarIndex("ix_ec_koubai_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_koubai_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcnDetailId), OrderByType.Asc, true)]
public class TaktEcKoubai : TaktCompanyEntityBase
{
    /// <summary>
    /// 设变明细 ID（TaktEcDetail 主键；关联由 TaktEcDetail.EcKoubai 导航）
    /// </summary>
    [SugarColumn(ColumnName = "ecn_detail_id", ColumnDescription = "设变明细ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcnDetailId { get; set; }
    /// <summary>
    /// 设变单号（冗余，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "ec_no", ColumnDescription = "设变单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string EcNo { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 部门编码（TaktDept.DeptCode，5 位，如 D0510）
    /// </summary>
    [SugarColumn(ColumnName = "dept_code", ColumnDescription = "部门编码", ColumnDataType = "varchar", Length = 5, IsNullable = false)]
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>
    /// 是否实施（0=否 1=是，字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "is_implemented", ColumnDescription = "实施", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsImplemented { get; set; } = 0;
    /// <summary>
    /// 执行内容（各部门通用）
    /// </summary>
    [SugarColumn(ColumnName = "exec_content", ColumnDescription = "执行内容", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ExecContent { get; set; }
    /// <summary>采购订单发行日期</summary>
    [SugarColumn(ColumnName = "purchase_order_issue_date", ColumnDescription = "采购订单发行日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? PurchaseOrderIssueDate { get; set; }
    /// <summary>供应商</summary>
    [SugarColumn(ColumnName = "supplier", ColumnDescription = "供应商", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? Supplier { get; set; }
    /// <summary>采购订单号码</summary>
    [SugarColumn(ColumnName = "purchase_order_no", ColumnDescription = "采购订单号码", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? PurchaseOrderNo { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

}
