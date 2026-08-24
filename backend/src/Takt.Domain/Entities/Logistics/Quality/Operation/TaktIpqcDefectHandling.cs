// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Logistics.Quality
// 文件名称：TaktIpqcDefectHandling.cs
// 功能描述：IPQC制程检验不良处理记录实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Operation;

/// <summary>
/// IPQC制程检验不良处理记录实体
/// </summary>
[SugarTable("takt_logistics_quality_ipqc_defect_handling", "制程检验不良处理记录表")]
[SugarIndex("ix_ipqc_defect_handling_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ipqc_defect_handling_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_ipqc_defect_handling_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IpqcOrderItemId), OrderByType.Asc, nameof(DefectCode), OrderByType.Asc, nameof(HandlingMethod), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_ipqc_defect_handling_defect_type", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DefectType), OrderByType.Asc, false)]
public class TaktIpqcDefectHandling : TaktCompanyEntityBase
{    /// <summary>
    /// IPQC不良处理编码
    /// </summary>
    [SugarColumn(ColumnName = "ipqc_defect_handling_code", ColumnDescription = "IPQC不良处理编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string IpqcDefectHandlingCode { get; set; } = string.Empty;
    /// <summary>
    /// IPQC检验单明细 ID（选项 TaktIpqcOrderItems/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "ipqc_order_item_id", ColumnDescription = "IPQC检验单明细ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long IpqcOrderItemId { get; set; }
    /// <summary>
    /// IPQC检验单编码（冗余字段，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "ipqc_order_code", ColumnDescription = "IPQC检验单编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string IpqcOrderCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 不良类型（0=轻微，1=一般，2=严重，3=致命）
    /// </summary>
    [SugarColumn(ColumnName = "defect_type", ColumnDescription = "不良类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DefectType { get; set; } = 0;
    /// <summary>
    /// 不良现象编码
    /// </summary>
    [SugarColumn(ColumnName = "defect_code", ColumnDescription = "不良现象编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string DefectCode { get; set; } = string.Empty;
    /// <summary>
    /// 不良现象描述
    /// </summary>
    [SugarColumn(ColumnName = "defect_description", ColumnDescription = "不良现象描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string DefectDescription { get; set; } = string.Empty;
    /// <summary>
    /// 不良数量
    /// </summary>
    [SugarColumn(ColumnName = "defect_quantity", ColumnDescription = "不良数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DefectQuantity { get; set; } = 0;
    /// <summary>
    /// 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
    /// </summary>
    [SugarColumn(ColumnName = "handling_method", ColumnDescription = "处理方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int HandlingMethod { get; set; } = 0;
    /// <summary>
    /// 处理说明
    /// </summary>
    [SugarColumn(ColumnName = "handling_description", ColumnDescription = "处理说明", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? HandlingDescription { get; set; }
    /// <summary>
    /// 责任部门
    /// </summary>
    [SugarColumn(ColumnName = "responsible_dept", ColumnDescription = "责任部门", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ResponsibleDept { get; set; }
    /// <summary>
    /// 责任人（人员代码）
    /// </summary>
    [SugarColumn(ColumnName = "responsible_by", ColumnDescription = "责任人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? ResponsibleBy { get; set; }
    /// <summary>
    /// 处理人（人员代码）
    /// </summary>
    [SugarColumn(ColumnName = "handler_by", ColumnDescription = "处理人", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? HandlerBy { get; set; }
    /// <summary>
    /// 处理时间
    /// </summary>
    [SugarColumn(ColumnName = "handling_at", ColumnDescription = "处理时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? HandlingAt { get; set; }
    /// <summary>
    /// 预防措施/纠正措施
    /// </summary>
    [SugarColumn(ColumnName = "corrective_action", ColumnDescription = "纠正措施", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? CorrectiveAction { get; set; }
    /// <summary>
    /// 不良图片（JSON格式，存储不良图片URL列表）
    /// </summary>
    [SugarColumn(ColumnName = "defect_images", ColumnDescription = "不良图片", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? DefectImages { get; set; }
    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? Attachments { get; set; }
    /// <summary>
    /// 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
    /// </summary>
    [SugarColumn(ColumnName = "handling_status", ColumnDescription = "处理状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int HandlingStatus { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// IPQC检验单明细（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(IpqcOrderItemId))]
    public TaktIpqcOrderItem? OrderItem { get; set; }
}
