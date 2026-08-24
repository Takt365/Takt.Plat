// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Operation
// 文件名称：TaktQualityGroup.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：质量组主数据实体，按检查类别（IQC/QA/IPQC）定义质量业务组织分组
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Operation;

/// <summary>
/// 质量组主数据实体（公司级；按检查类别区分的质量业务组织分组）
/// </summary>
[SugarTable("takt_logistics_quality_operation_quality_group", "质量组主数据表")]
[SugarIndex("ix_quality_group_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quality_group_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_operation_quality_group_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(InspectionCategory), OrderByType.Asc, nameof(QualityGroupCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_operation_quality_group_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_operation_quality_group_inspection_category", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InspectionCategory), OrderByType.Asc, false)]
public class TaktQualityGroup : TaktCompanyEntityBase
{
    /// <summary>
    /// 检查类别（字典 logistics_quality_group_inspection_category；0=IQC，1=QA，2=IPQC）
    /// </summary>
    [SugarColumn(ColumnName = "inspection_category", ColumnDescription = "检查类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int InspectionCategory { get; set; } = 0;
    /// <summary>
    /// 质量组编码（3）
    /// </summary>
    [SugarColumn(ColumnName = "quality_group_code", ColumnDescription = "质量组编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string QualityGroupCode { get; set; } = string.Empty;
    /// <summary>
    /// 质量组名称
    /// </summary>
    [SugarColumn(ColumnName = "quality_group_name", ColumnDescription = "质量组名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string QualityGroupName { get; set; } = string.Empty;
    /// <summary>
    /// 质量组描述
    /// </summary>
    [SugarColumn(ColumnName = "quality_group_description", ColumnDescription = "质量组描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? QualityGroupDescription { get; set; }
    /// <summary>
    /// 质量组负责人用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_user_id", ColumnDescription = "负责人用户ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ResponsibleUserId { get; set; }
    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "联系电话", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ContactPhone { get; set; }
    /// <summary>
    /// 联系邮箱
    /// </summary>
    [SugarColumn(ColumnName = "contact_email", ColumnDescription = "联系邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ContactEmail { get; set; }
    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;
    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
    /// <summary>
    /// 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "group_status", ColumnDescription = "质量组状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int GroupStatus { get; set; } = 1;
}
