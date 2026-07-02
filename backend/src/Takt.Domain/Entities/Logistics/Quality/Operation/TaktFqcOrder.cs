// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Quality
// 文件名称：TaktFqcOrder.cs
// 功能描述：FQC出货检验单实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Operation;

/// <summary>
/// FQC出货检验单实体
/// </summary>
[SugarTable("takt_logistics_quality_fqc_order", "出货检验单表")]
[SugarIndex("ix_fqc_order_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_fqc_order_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_fqc_order_fqc_order_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(FqcOrderCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_fqc_order_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_fqc_order_customer_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
public class TaktFqcOrder : TaktCompanyEntityBase
{    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "plant_code", ColumnDescription = "工厂代码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 来源单号（选项 TaktSalesOrders/options，DictValue=SalesOrderCode）
    /// </summary>
    [SugarColumn(ColumnName = "source_code", ColumnDescription = "来源单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SourceCode { get; set; } = string.Empty;
    /// <summary>
    /// 检验日期
    /// </summary>
    [SugarColumn(ColumnName = "inspection_date", ColumnDescription = "检验日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? InspectionDate { get; set; }
    /// <summary>
    /// FQC检验单编码（唯一索引，根据来源单号自动生成）
    /// </summary>
    [SugarColumn(ColumnName = "fqc_order_code", ColumnDescription = "FQC检验单编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string FqcOrderCode { get; set; } = string.Empty;
    /// <summary>
    /// 客户编码（选项 TaktCustomers/options，DictValue=CustomerCode）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 总入库数
    /// </summary>
    [SugarColumn(ColumnName = "total_warehouse_quantity", ColumnDescription = "总入库数", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TotalWarehouseQuantity { get; set; } = 0;
    /// <summary>
    /// 总抽样数量（自动计算 = 各明细抽样数量合计）
    /// </summary>
    [SugarColumn(ColumnName = "total_sample_quantity", ColumnDescription = "总抽样数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalSampleQuantity { get; set; } = 0;
    /// <summary>
    /// 总合格数量（自动计算 = 各明细合格数量合计）
    /// </summary>
    [SugarColumn(ColumnName = "total_qualified_quantity", ColumnDescription = "总合格数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalQualifiedQuantity { get; set; } = 0;
    /// <summary>
    /// 总不合格数量（自动计算 = 各明细不合格数量合计）
    /// </summary>
    [SugarColumn(ColumnName = "total_unqualified_quantity", ColumnDescription = "总不合格数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TotalUnqualifiedQuantity { get; set; } = 0;
    /// <summary>
    /// 总验退数量（自动计算 = 各明细验退数量合计）
    /// </summary>
    [SugarColumn(ColumnName = "total_inspection_return_quantity", ColumnDescription = "总验退数量", ColumnDataType = "decimal", Length = 16, DecimalDigits = 6, IsNullable = false, DefaultValue = "0")]
    public decimal TotalInspectionReturnQuantity { get; set; } = 0;
    /// <summary>
    /// 判定人（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    [SugarColumn(ColumnName = "judge_by", ColumnDescription = "判定人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? JudgeBy { get; set; }
    /// <summary>
    /// 判定日期
    /// </summary>
    [SugarColumn(ColumnName = "judge_date", ColumnDescription = "判定日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? JudgeDate { get; set; }
    /// <summary>
    /// 判定说明
    /// </summary>
    [SugarColumn(ColumnName = "judge_description", ColumnDescription = "判定说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? JudgeDescription { get; set; }
    /// <summary>
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    [SugarColumn(ColumnName = "judge_status", ColumnDescription = "判定状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// FQC检验单明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktFqcOrderItem.FqcOrderId))]
    public List<TaktFqcOrderItem>? Items { get; set; }

    /// <summary>
    /// 变更日志列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktFqcOrderChangeLog.FqcOrderId))]
    public List<TaktFqcOrderChangeLog>? ChangeLogs { get; set; }
}
