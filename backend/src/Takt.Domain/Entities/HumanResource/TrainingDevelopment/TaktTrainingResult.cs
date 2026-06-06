// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.TrainingDevelopment
// 文件名称：TaktTrainingResult.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：培训结果实体，对应菜单 training-development/result
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.TrainingDevelopment;

/// <summary>
/// 员工培训结果记录
/// </summary>
[SugarTable("takt_human_resource_training_development_result", "培训结果表")]
[SugarIndex("ix_training_result_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_training_result_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_training_result_employee_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(TrainingDate), OrderByType.Desc, false)]
public class TaktTrainingResult : TaktCompanyEntityBase
{
    /// <summary>
    /// 员工 ID
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 员工姓名
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string EmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 培训课程 ID
    /// </summary>
    [SugarColumn(ColumnName = "training_course_id", ColumnDescription = "培训课程ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TrainingCourseId { get; set; }
    /// <summary>
    /// 培训课程名称
    /// </summary>
    [SugarColumn(ColumnName = "course_name", ColumnDescription = "培训课程名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string CourseName { get; set; } = string.Empty;
    /// <summary>
    /// 培训类型
    /// </summary>
    [SugarColumn(ColumnName = "training_type", ColumnDescription = "培训类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string TrainingType { get; set; } = string.Empty;
    /// <summary>
    /// 培训讲师
    /// </summary>
    [SugarColumn(ColumnName = "instructor", ColumnDescription = "培训讲师", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string Instructor { get; set; } = string.Empty;
    /// <summary>
    /// 培训开始日期
    /// </summary>
    [SugarColumn(ColumnName = "training_start_date", ColumnDescription = "培训开始日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime TrainingStartDate { get; set; }
    /// <summary>
    /// 培训结束日期
    /// </summary>
    [SugarColumn(ColumnName = "training_end_date", ColumnDescription = "培训结束日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime TrainingEndDate { get; set; }
    /// <summary>
    /// 培训日期
    /// </summary>
    [SugarColumn(ColumnName = "training_date", ColumnDescription = "培训日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime TrainingDate { get; set; }
    /// <summary>
    /// 培训时长（小时）
    /// </summary>
    [SugarColumn(ColumnName = "training_hours", ColumnDescription = "培训时长", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TrainingHours { get; set; }
    /// <summary>
    /// 培训成绩
    /// </summary>
    [SugarColumn(ColumnName = "training_score", ColumnDescription = "培训成绩", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TrainingScore { get; set; }
    /// <summary>
    /// 是否通过（0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_passed", ColumnDescription = "是否通过", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsPassed { get; set; }
    /// <summary>
    /// 证书编号
    /// </summary>
    [SugarColumn(ColumnName = "certificate_no", ColumnDescription = "证书编号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CertificateNo { get; set; } = string.Empty;
    /// <summary>
    /// 培训评价
    /// </summary>
    [SugarColumn(ColumnName = "training_evaluation", ColumnDescription = "培训评价", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string TrainingEvaluation { get; set; } = string.Empty;
    /// <summary>
    /// 状态（1=有效 0=无效）
    /// </summary>
    [SugarColumn(ColumnName = "training_result_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TrainingResultStatus { get; set; } = 1;
    /// <summary>
    /// 关联工厂
    /// </summary>
    [SugarColumn(ColumnName = "related_plant", ColumnDescription = "关联工厂", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? RelatedPlant { get; set; }
}
