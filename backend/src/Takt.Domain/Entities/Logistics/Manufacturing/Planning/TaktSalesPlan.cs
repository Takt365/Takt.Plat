// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Planning
// 文件名称：TaktSalesPlan.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售计划实体，MRP 需求源头（销售计划 → 生产计划 → 采购计划）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Planning;

/// <summary>
/// Takt销售计划实体（公司级；MRP 需求计划源头，可下达生产计划或销售订单）
/// </summary>
[SugarTable("takt_logistics_manufacturing_planning_sales_plan", "销售计划表")]
[SugarIndex("ix_sales_plan_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sales_plan_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_planning_sales_plan_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SalesPlanCode), OrderByType.Asc, nameof(PlanDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_planning_sales_plan_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_planning_sales_plan_plan_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlanDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_planning_sales_plan_customer", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
public class TaktSalesPlan : TaktApprovalEntityBase
{
    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售计划编码（租户+公司+工厂内业务唯一）
    /// </summary>
    [SugarColumn(ColumnName = "sales_plan_code", ColumnDescription = "销售计划编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string SalesPlanCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划编制日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_date", ColumnDescription = "计划编制日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 计划周期开始日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_period_start", ColumnDescription = "计划周期开始日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanPeriodStart { get; set; } = DateTime.Now;

    /// <summary>
    /// 计划周期结束日期
    /// </summary>
    [SugarColumn(ColumnName = "plan_period_end", ColumnDescription = "计划周期结束日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanPeriodEnd { get; set; } = DateTime.Now;

    /// <summary>
    /// 客户编码（关联 TaktCustomer.CustomerCode，选项 TaktCustomers/options；汇总计划时可为空）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 客户名称（冗余字段，便于查询展示）
    /// </summary>
    [SugarColumn(ColumnName = "customer_name", ColumnDescription = "客户名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? CustomerName { get; set; }

    /// <summary>
    /// 计划人员工ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
    /// </summary>
    [SugarColumn(ColumnName = "planner_id", ColumnDescription = "计划人员工ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PlannerId { get; set; }

    /// <summary>
    /// 计划人（关联 TaktEmployee.EmployeeNo，选项 TaktEmployees/options，DictValue=EmployeeNo）
    /// </summary>
    [SugarColumn(ColumnName = "plan_by", ColumnDescription = "计划人", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string PlanBy { get; set; } = string.Empty;

    /// <summary>
    /// 计划总数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "total_quantity", ColumnDescription = "计划总数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 计划总金额
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "计划总金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; } = 0;

    /// <summary>
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    [SugarColumn(ColumnName = "converted_quantity", ColumnDescription = "已转生产销售数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedQuantity { get; set; } = 0;

    /// <summary>
    /// 已转生产/销售金额
    /// </summary>
    [SugarColumn(ColumnName = "converted_amount", ColumnDescription = "已转生产销售金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedAmount { get; set; } = 0;

    /// <summary>
    /// 计划状态（字典 sys_normal_disable_status；1=启用，0=禁用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "plan_status", ColumnDescription = "计划状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PlanStatus { get; set; } = 1;

    /// <summary>
    /// 转单状态（字典 sys_convert_status；0=未转换，1=部分转换，2=全部转换）
    /// </summary>
    [SugarColumn(ColumnName = "converted_status", ColumnDescription = "转单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ConvertedStatus { get; set; } = 0;

    /// <summary>
    /// 计划说明
    /// </summary>
    [SugarColumn(ColumnName = "plan_description", ColumnDescription = "计划说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? PlanDescription { get; set; }

    /// <summary>
    /// 销售计划明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSalesPlanItem.SalesPlanId))]
    public List<TaktSalesPlanItem>? Items { get; set; }
}
