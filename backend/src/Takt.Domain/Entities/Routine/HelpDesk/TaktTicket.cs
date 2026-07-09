// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities.Routine.HelpDesk
// 文件名称：TaktTicket.cs
// 创建时间：2025-02-26
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt工单实体，服务台工单领域模型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.HelpDesk;

/// <summary>
/// 服务台工单实体
/// </summary>
[SugarTable("takt_routine_help_desk_ticket", "工单表")]
[SugarIndex("ix_ticket_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_no_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TicketNo), OrderByType.Asc, true)]
[SugarIndex("ix_ticket_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TicketStatus), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_parent_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ParentTicketId), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_source", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TicketSource), OrderByType.Asc, false)]
[SugarIndex("ix_ticket_flow_instance_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktTicket : TaktCompanyEntityBase
{
    /// <summary>
    /// 工单编号（唯一）
    /// </summary>
    [SugarColumn(ColumnName = "ticket_no", ColumnDescription = "工单编号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string TicketNo { get; set; } = string.Empty;
    /// <summary>
    /// 工单标题
    /// </summary>
    [SugarColumn(ColumnName = "ticket_title", ColumnDescription = "工单标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string TicketTitle { get; set; } = string.Empty;
    /// <summary>
    /// 工单内容描述
    /// </summary>
    [SugarColumn(ColumnName = "ticket_content", ColumnDescription = "工单内容", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? TicketContent { get; set; }
    /// <summary>
    /// 附件（JSON 列表形式，由 TaktFile 统一上传到服务器）
    /// </summary>
    [SugarColumn(ColumnName = "attachments", ColumnDescription = "附件列表JSON", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? attachments { get; set; }
    /// <summary>
    /// 优先级（字典 sys_priority_level_category；1=最高 2=高 3=普通 4=低）
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "3")]
    public int Priority { get; set; } = 3;
    /// <summary>
    /// 紧急度（字典 sys_urgency_level_category；1=高 2=中 3=低）
    /// </summary>
    [SugarColumn(ColumnName = "urgency", ColumnDescription = "紧急度", ColumnDataType = "int", IsNullable = false, DefaultValue = "3")]
    public int Urgency { get; set; } = 3;
    /// <summary>
    /// 影响范围（字典 sys_impact_level_category；1=高 2=中 3=低）
    /// </summary>
    [SugarColumn(ColumnName = "impact", ColumnDescription = "影响范围", ColumnDataType = "int", IsNullable = false, DefaultValue = "3")]
    public int Impact { get; set; } = 3;
    /// <summary>
    /// 分类编码（如 incident/request 等，与 TaktTicketCategoryAssign.CategoryCode 对应）
    /// </summary>
    [SugarColumn(ColumnName = "category_code", ColumnDescription = "分类编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? CategoryCode { get; set; }
    /// <summary>
    /// 工单来源（字典 routine_ticket_source_type；0=门户 1=邮件 2=电话 3=API）
    /// </summary>
    [SugarColumn(ColumnName = "ticket_source", ColumnDescription = "工单来源", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TicketSource { get; set; } = 0;
    /// <summary>
    /// 提交人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [SugarColumn(ColumnName = "submitter_id", ColumnDescription = "提交人ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SubmitterId { get; set; }
    /// <summary>
    /// 提交人姓名
    /// </summary>
    [SugarColumn(ColumnName = "submitter_name", ColumnDescription = "提交人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? SubmitterName { get; set; }
    /// <summary>
    /// 处理人 ID（关联 TaktUser.Id，选项 TaktUsers/options）
    /// </summary>
    [SugarColumn(ColumnName = "assignee_id", ColumnDescription = "处理人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssigneeId { get; set; }
    /// <summary>
    /// 处理人姓名
    /// </summary>
    [SugarColumn(ColumnName = "assignee_name", ColumnDescription = "处理人姓名", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? AssigneeName { get; set; }
    /// <summary>
    /// 关联知识 ID（关联 TaktKnowledge.Id，选项 TaktKnowledges/options）
    /// </summary>
    [SugarColumn(ColumnName = "knowledge_id", ColumnDescription = "关联知识ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? KnowledgeId { get; set; }
    /// <summary>
    /// 父工单 ID（关联 TaktTicket.Id，选项 TaktTickets/options；为空表示顶级工单）
    /// </summary>
    [SugarColumn(ColumnName = "parent_ticket_id", ColumnDescription = "父工单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentTicketId { get; set; }
    /// <summary>
    /// 首次响应时间（支持人员首次回复用户的时间，SLA/OLA 时间追踪）
    /// </summary>
    [SugarColumn(ColumnName = "first_response_at", ColumnDescription = "首次响应时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? FirstResponseAt { get; set; }
    /// <summary>
    /// 首次响应期限（根据 SLA 计算出的首次响应截止时间）
    /// </summary>
    [SugarColumn(ColumnName = "first_response_due_by", ColumnDescription = "首次响应期限", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? FirstResponseDueBy { get; set; }
    /// <summary>
    /// 解决时间（问题被标记为已解决的时间）
    /// </summary>
    [SugarColumn(ColumnName = "resolved_at", ColumnDescription = "解决时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ResolvedAt { get; set; }
    /// <summary>
    /// 解决期限（根据 SLA 计算出的解决截止时间）
    /// </summary>
    [SugarColumn(ColumnName = "resolution_due_by", ColumnDescription = "解决期限", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ResolutionDueBy { get; set; }
    /// <summary>
    /// 关闭时间（工单最终关闭的时间）
    /// </summary>
    [SugarColumn(ColumnName = "closed_at", ColumnDescription = "关闭时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ClosedAt { get; set; }
    /// <summary>
    /// IT 设备保修扩展 ID（关联 TaktItAsset.Id，选项 TaktItAssets/options）
    /// </summary>
    [SugarColumn(ColumnName = "it_asset_id", ColumnDescription = "IT设备ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ItAssetId { get; set; }
    /// <summary>
    /// 资产号码（冗余；与 TaktItAsset.AssetCode 一致）
    /// </summary>
    [SugarColumn(ColumnName = "asset_code", ColumnDescription = "资产号码", ColumnDataType = "varchar", Length = 40, IsNullable = true)]
    public string? AssetCode { get; set; }
    /// <summary>
    /// 流程实例 ID（关联 TaktFlowInstance.Id；流程侧 BusinessType=Ticket、BusinessKey=本表 Id）
    /// </summary>
    [SugarColumn(ColumnName = "flow_instance_id", ColumnDescription = "流程实例ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }
    /// <summary>
    /// 申请部门 ID（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [SugarColumn(ColumnName = "applicant_dept_id", ColumnDescription = "申请部门ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantDeptId { get; set; }
    /// <summary>
    /// 申请部门名称
    /// </summary>
    [SugarColumn(ColumnName = "applicant_dept_name", ColumnDescription = "申请部门名称", Length = 100, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? ApplicantDeptName { get; set; }
    /// <summary>
    /// 申请人 ID（关联 TaktUser.Id，选项 TaktUsers/options；代理人代提时填被代理人）
    /// </summary>
    [SugarColumn(ColumnName = "applicant_by", ColumnDescription = "申请人", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApplicantBy { get; set; }
    /// <summary>
    /// 工单状态（字典 sys_ticket_status；0=新建 1=已分配 2=处理中 3=待确认 4=已完成 5=已关闭 6=已取消 7=重新打开）
    /// </summary>
    [SugarColumn(ColumnName = "ticket_status", ColumnDescription = "工单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TicketStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 子工单列表（父工单时有效；外键：本表 Id = 子工单 ParentTicketId）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ParentTicketId))]
    public List<TaktTicket>? ChildTickets { get; set; }
    /// <summary>
    /// 服务评价（工单关闭后的评价，一对一）
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(TaktTicketEvaluation.TicketId))]
    public TaktTicketEvaluation? Evaluation { get; set; }
}
