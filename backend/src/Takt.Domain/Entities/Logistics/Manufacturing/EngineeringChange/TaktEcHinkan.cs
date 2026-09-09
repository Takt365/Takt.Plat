// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcHinkan.cs
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变品管课部门执行表（按设变明细ID+完成品唯一）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Interfaces;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变品管课（D0820）部门执行表（按设变明细 + 完成品唯一）
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_hinkan", "设变品管执行表")]
[SugarIndex("ix_ec_hinkan_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_hinkan_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcDetailId), OrderByType.Asc, nameof(EcFinishedGoods), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_hinkan_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktEcHinkan : TaktCompanyEntityBase, ITaktEcDeptExecEntity
{
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 设变单号（冗余：来自 TaktEcDetail.EcCode）
    /// </summary>
    [SugarColumn(ColumnName = "ec_code", ColumnDescription = "设变单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string EcCode { get; set; } = string.Empty;
    /// <summary>
    /// 停产状态（冗余：来自 TaktEcDetail.DiscontinuedStatus）
    /// </summary>
    [SugarColumn(ColumnName = "discontinued_status", ColumnDescription = "停产状态", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string DiscontinuedStatus { get; set; } = string.Empty;
    /// <summary>
    /// 机种编码（冗余：来自 TaktEcDetail.EcModelCode）
    /// </summary>
    [SugarColumn(ColumnName = "ec_model_code", ColumnDescription = "机种编码", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcModelCode { get; set; } = string.Empty;
    /// <summary>
    /// 完成品（冗余：来自 TaktEcDetail.EcFinishedGoods）
    /// </summary>
    [SugarColumn(ColumnName = "ec_finished_goods", ColumnDescription = "完成品", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcFinishedGoods { get; set; } = string.Empty;

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
    /// <summary>
    /// 生产班组
    /// </summary>
    [SugarColumn(ColumnName = "production_team", ColumnDescription = "生产班组", ColumnDataType = "nvarchar", Length = 8, IsNullable = true)]
    public string? ProductionTeam { get; set; }
    /// <summary>
    /// 检验日期
    /// </summary>
    [SugarColumn(ColumnName = "inspection_date", ColumnDescription = "检验日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? InspectionDate { get; set; }
    /// <summary>
    /// 检验批次
    /// </summary>
    [SugarColumn(ColumnName = "inspection_batch", ColumnDescription = "检验批次", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? InspectionBatch { get; set; }
    /// <summary>
    /// 抽样号码
    /// </summary>
    [SugarColumn(ColumnName = "sampling_code", ColumnDescription = "抽样号码", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? SamplingCode { get; set; }
    /// <summary>
    /// 完成品描述（冗余：来自 TaktEcDetail.EcFinishedGoodsDescription）
    /// </summary>
    [SugarColumn(ColumnName = "ec_finished_goods_description", ColumnDescription = "完成品描述", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string EcFinishedGoodsDescription { get; set; } = string.Empty;

    /// <summary>
    /// 部门编码（TaktDept.DeptCode；本表固定课别）
    /// </summary>
    [SugarColumn(ColumnName = "dept_code", ColumnDescription = "部门编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string DeptCode { get; set; } = string.Empty;
    /// <summary>
    /// 部门名称（冗余：按 DeptCode 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 管理区分（冗余：来自 TaktEcDetail.EcDistinction）
    /// </summary>
    [SugarColumn(ColumnName = "ec_distinction", ColumnDescription = "管理区分", ColumnDataType = "int", IsNullable = false)]
    public int EcDistinction { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变明细 ID（TaktEcDetail 主键；去重组内代表/种子明细 Id；同组多明细按业务键 FanOut）
    /// </summary>
    [SugarColumn(ColumnName = "ec_detail_id", ColumnDescription = "设变明细ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDetailId { get; set; }

    /// <summary>
    /// 设变明细（数据主从：本表由明细派生；多对一，外键 EcDetailId → TaktEcDetail.Id）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EcDetailId))]
    public TaktEcDetail? EcDetail { get; set; }
}
