// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktNumbering.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：编号规则实体，定义各类业务单据的编号生成规则
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Shared.Enums;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 编号规则实体
/// 定义系统中各类业务单据的编号生成规则，如：订单号、合同号、发票号等
/// 支持灵活的前缀、日期格式、流水号组合
/// 
/// 编码顺序：单据类型-公司-部门-前缀-日期-流水号
/// 示例：order-1000-DEPT01-SO-20250120-000001
/// </summary>
[SugarTable("takt_foundation_numbering", "编号规则表")]
[SugarIndex("ix_numbering_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_numbering_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_numbering_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RuleCode), OrderByType.Asc, true)]
public class TaktNumbering : TaktCompanyEntityBase
{
    /// <summary>
    /// 规则编码（唯一索引：租户+公司内唯一，见 ix_numbering_code_unique；如 SO, PO, CONTRACT）
    /// </summary>
    [SugarColumn(ColumnName = "rule_code", ColumnDescription = "规则编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（如：销售订单号、采购订单号）
    /// </summary>
    [SugarColumn(ColumnName = "rule_name", ColumnDescription = "规则名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 单据类型
    /// </summary>
    [SugarColumn(ColumnName = "document_type", ColumnDescription = "单据类型", ColumnDataType = "int", IsNullable = false)]
    public TaktDocumentType DocumentType { get; set; }

    /// <summary>
    /// 部门编码（如：DEPT01, DEPT02，不可为空）
    /// 从 TaktDepartment 实体自动获取 DisplayCode
    /// </summary>
    [SugarColumn(ColumnName = "department_code", ColumnDescription = "部门编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string DepartmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 前缀（如：SO-, PO-, INV-）
    /// </summary>
    [SugarColumn(ColumnName = "prefix", ColumnDescription = "前缀", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? Prefix { get; set; }

    /// <summary>
    /// 日期格式（yyyy, yyyyMM, yyyyMMdd, yyyyMMddHH, yyyyMMddHHmm）
    /// 为空表示不使用日期
    /// </summary>
    [SugarColumn(ColumnName = "date_format", ColumnDescription = "日期格式", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? DateFormat { get; set; }

    /// <summary>
    /// 流水号位数（3=001, 4=0001, 5=00001, 6=000001）
    /// </summary>
    [SugarColumn(ColumnName = "sequence_length", ColumnDescription = "流水号位数", ColumnDataType = "int", IsNullable = false, DefaultValue = "6")]
    public int SequenceLength { get; set; } = 6;

    /// <summary>
    /// 流水号步长（每次递增的数值，默认1）
    /// </summary>
    [SugarColumn(ColumnName = "sequence_step", ColumnDescription = "流水号步长", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SequenceStep { get; set; } = 1;

    /// <summary>
    /// 后缀（如：-CN, -USD, -V2）
    /// </summary>
    [SugarColumn(ColumnName = "suffix", ColumnDescription = "后缀", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? Suffix { get; set; }

    /// <summary>
    /// 重置周期（daily=每日重置，monthly=每月重置，yearly=每年重置，none=不重置）
    /// </summary>
    [SugarColumn(ColumnName = "reset_period", ColumnDescription = "重置周期", ColumnDataType = "varchar", Length = 20, IsNullable = false, DefaultValue = "none")]
    public string ResetPeriod { get; set; } = "none";

    /// <summary>
    /// 当前流水号（用于记录下一个流水号值）
    /// </summary>
    [SugarColumn(ColumnName = "current_sequence", ColumnDescription = "当前流水号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CurrentSequence { get; set; } = 0;

    /// <summary>
    /// 示例编码（自动生成，用于预览规则效果）
    /// 如：SO-20250120-000001
    /// </summary>
    [SugarColumn(ColumnName = "example_code", ColumnDescription = "示例编码", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? ExampleCode { get; set; }

    /// <summary>
    /// 分隔符（默认 -，也可用 _ 或 /）
    /// </summary>
    [SugarColumn(ColumnName = "separator", ColumnDescription = "分隔符", ColumnDataType = "varchar", Length = 1, IsNullable = false, DefaultValue = "-")]
    public string Separator { get; set; } = "-";

    /// <summary>
    /// 是否内置（0=否，1=是，系统内置的不可删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "是否内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public TaktYesNo IsBuiltIn { get; set; } = TaktYesNo.No;

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public TaktCommonStatus Status { get; set; } = TaktCommonStatus.Enabled;

    /// <summary>
    /// 描述说明；可选配置编码段顺序，格式：segments:DocumentType,CompanyCode,DepartmentCode,Prefix,DateFormat,Sequence（段名为实体属性名，Sequence 为流水号占位）
    /// </summary>
    [SugarColumn(ColumnName = "description", ColumnDescription = "描述说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Description { get; set; }
}
