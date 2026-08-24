// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcAttachment.cs
// 功能描述：设变附件实体（技术阶段一 ②）；技术维护联络/EPP/FPP 等文档，与主表、明细一并保存后触发通知自动生成
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变附件实体（技术阶段一 ②，隶属 TaktEcGijutsu）。文件类别见字典 logistics_ec_attachment_type；与主表、明细保存后由系统生成 TaktEcNotification。
/// </summary>
[SugarTable("takt_logistics_manufacturing_ec_attachment", "设变附件表")]
[SugarIndex("ix_ec_attachment_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ec_attachment_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_attachment_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EcId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_attachment_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_attachment_attachment_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(AttachmentType), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_ec_attachment_doc_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DocCode), OrderByType.Asc, false)]
public class TaktEcAttachment : TaktCompanyEntityBase
{
    /// <summary>
    /// 设变主表ID
    /// </summary>
    [SugarColumn(ColumnName = "ec_id", ColumnDescription = "设变ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcId { get; set; }


    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "ec_code", ColumnDescription = "设变单号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string EcCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 文件类别（字典 logistics_ec_attachment_type；TL=联络，EPP=EPP，FPP=FPP，EL=外部联络，TCJ=TCJ，源PDF=源PDF，EC=EC）
    /// </summary>
    [SugarColumn(ColumnName = "attachment_type", ColumnDescription = "文件类别", ColumnDataType = "nvarchar", Length = 8, IsNullable = false)]
    public string AttachmentType { get; set; } = string.Empty;

    /// <summary>
    /// 文件编码（如联络编码等）
    /// </summary>
    [SugarColumn(ColumnName = "doc_code", ColumnDescription = "文件编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string DocCode { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（URL）
    /// </summary>
    [SugarColumn(ColumnName = "access_url", ColumnDescription = "访问地址", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 设变主表（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EcId))]
    public TaktEcGijutsu? EcGijutsu { get; set; }
}
