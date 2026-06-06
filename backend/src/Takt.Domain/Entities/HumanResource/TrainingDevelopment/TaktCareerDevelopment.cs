// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.TrainingDevelopment
// 文件名称：TaktCareerDevelopment.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：职业发展实体，对应菜单 training-development/career
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.TrainingDevelopment;

/// <summary>
/// 员工职业发展规划与技能评估
/// </summary>
[SugarTable("takt_human_resource_training_development_career", "职业发展表")]
[SugarIndex("ix_career_development_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_career_development_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_career_development_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
public class TaktCareerDevelopment : TaktCompanyEntityBase
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
    /// 技能类别
    /// </summary>
    [SugarColumn(ColumnName = "skill_category", ColumnDescription = "技能类别", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SkillCategory { get; set; } = string.Empty;
    /// <summary>
    /// 技能名称
    /// </summary>
    [SugarColumn(ColumnName = "skill_name", ColumnDescription = "技能名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string SkillName { get; set; } = string.Empty;
    /// <summary>
    /// 评估日期
    /// </summary>
    [SugarColumn(ColumnName = "assessment_date", ColumnDescription = "评估日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime AssessmentDate { get; set; }
    /// <summary>
    /// 评估方式
    /// </summary>
    [SugarColumn(ColumnName = "assessment_method", ColumnDescription = "评估方式", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string AssessmentMethod { get; set; } = string.Empty;
    /// <summary>
    /// 评估得分
    /// </summary>
    [SugarColumn(ColumnName = "assessment_score", ColumnDescription = "评估得分", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AssessmentScore { get; set; }
    /// <summary>
    /// 技能等级
    /// </summary>
    [SugarColumn(ColumnName = "skill_level", ColumnDescription = "技能等级", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string SkillLevel { get; set; } = string.Empty;
    /// <summary>
    /// 目标岗位
    /// </summary>
    [SugarColumn(ColumnName = "target_position", ColumnDescription = "目标岗位", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string TargetPosition { get; set; } = string.Empty;
    /// <summary>
    /// 发展计划
    /// </summary>
    [SugarColumn(ColumnName = "development_plan", ColumnDescription = "发展计划", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string DevelopmentPlan { get; set; } = string.Empty;
    /// <summary>
    /// 改进建议
    /// </summary>
    [SugarColumn(ColumnName = "improvement_suggestions", ColumnDescription = "改进建议", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string ImprovementSuggestions { get; set; } = string.Empty;
    /// <summary>
    /// 下次评估日期
    /// </summary>
    [SugarColumn(ColumnName = "next_assessment_date", ColumnDescription = "下次评估日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime NextAssessmentDate { get; set; }
    /// <summary>
    /// 状态（1=进行中 0=已归档）
    /// </summary>
    [SugarColumn(ColumnName = "career_development_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CareerDevelopmentStatus { get; set; } = 1;
    /// <summary>
    /// 关联工厂
    /// </summary>
    [SugarColumn(ColumnName = "related_plant", ColumnDescription = "关联工厂", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? RelatedPlant { get; set; }
}
