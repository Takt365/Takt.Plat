// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeAttachment.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：员工档案附件实体（人事-附件管理）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工档案附件（主档子表，公司级非审批单）
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_attachment", "员工附件表")]
[SugarIndex("ix_employee_attachment_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_attachment_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
public class TaktEmployeeAttachment : TaktCompanyEntityBase
{
    /// <summary>
    /// 员工ID
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 文件ID（关联文件服务）
    /// </summary>
    [SugarColumn(ColumnName = "file_id", ColumnDescription = "文件ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? FileId { get; set; }

    /// <summary>
    /// 文件编码
    /// </summary>
    [SugarColumn(ColumnName = "file_code", ColumnDescription = "文件编码", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? FileCode { get; set; }

    /// <summary>
    /// 文件名称
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", ColumnDataType = "nvarchar", Length = 255, IsNullable = false)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    [SugarColumn(ColumnName = "file_path", ColumnDescription = "文件路径", ColumnDataType = "varchar", Length = 500, IsNullable = true)]
    public string? FilePath { get; set; }

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [SugarColumn(ColumnName = "file_size", ColumnDescription = "文件大小", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件类型/MIME
    /// </summary>
    [SugarColumn(ColumnName = "file_type", ColumnDescription = "文件类型", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? FileType { get; set; }

    /// <summary>
    /// 附件类型（0=身份证，1=学历证，2=合同，3=照片，4=离职证明，5=其他）
    /// </summary>
    [SugarColumn(ColumnName = "attachment_type", ColumnDescription = "附件类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "5")]
    public int AttachmentType { get; set; }

    /// <summary>
    /// 附件说明
    /// </summary>
    [SugarColumn(ColumnName = "attachment_description", ColumnDescription = "附件说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? AttachmentDescription { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
}
