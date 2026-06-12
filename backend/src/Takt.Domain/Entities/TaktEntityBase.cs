// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Domain.Entities
// 文件名称：TaktCompanyEntityBase.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：实体基类体系，包含租户级、公司级、审批级三层基类
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities;

// ========================================
// 租户级实体基类（对应 TaktTenantEntityBase）
// ========================================

/// <summary>
/// 租户级实体基类
/// 仅包含租户隔离(TenantCode),不包含公司隔离(CompanyCode)
/// 适用于用户、角色、菜单等跨公司共享的实体
/// </summary>
public abstract class TaktTenantEntityBase
{
    /// <summary>
    /// 主键ID(雪花ID,序列化为string以避免Javascript精度问题)
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long Id { get; set; }

    /// <summary>
    /// 租户编码(第一层数据隔离)
    /// </summary>
    [SugarColumn(ColumnName = "tenant_code", ColumnDescription = "租户编码", ColumnDataType = "varchar", Length = 3, IsNullable = false)]
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    [SugarColumn(ColumnName = "ext_field_json", ColumnDescription = "扩展字段JSON", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Remark { get; set; }

    /// <summary>
    /// 创建人ID(非空;无当前用户时仓储填 999)
    /// </summary>
    [SugarColumn(ColumnName = "created_by", ColumnDescription = "创建人ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间(非空)
    /// </summary>
    [SugarColumn(ColumnName = "created_at", ColumnDescription = "创建时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新人ID
    /// </summary>
    [SugarColumn(ColumnName = "updated_by", ColumnDescription = "更新人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnName = "updated_at", ColumnDescription = "更新时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除(软删除标记,0=未删除,1=已删除)
    /// </summary>
    [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "是否删除", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [SugarColumn(ColumnName = "deleted_by", ColumnDescription = "删除人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [SugarColumn(ColumnName = "deleted_at", ColumnDescription = "删除时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? DeletedAt { get; set; }
}

// ========================================
// 公司级实体基类（对应 TaktCompanyEntityBase）
// ========================================

/// <summary>
/// 公司级实体基类(包含租户+公司双重隔离)
/// 适用于部门、岗位、员工等业务实体
/// </summary>
public abstract class TaktCompanyEntityBase
{
    /// <summary>
    /// 主键ID(雪花ID,序列化为string以避免Javascript精度问题)
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long Id { get; set; }

    /// <summary>
    /// 租户编码(第一层数据隔离)
    /// </summary>
    [SugarColumn(ColumnName = "tenant_code", ColumnDescription = "租户编码", ColumnDataType = "varchar", Length = 3, IsNullable = false)]
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码(第二层数据隔离)
    /// </summary>
    [SugarColumn(ColumnName = "company_code", ColumnDescription = "公司代码", ColumnDataType = "varchar", Length = 4, IsNullable = false)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    [SugarColumn(ColumnName = "ext_field_json", ColumnDescription = "扩展字段JSON", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Remark { get; set; }

    /// <summary>
    /// 创建人ID(非空;无当前用户时仓储填 999)
    /// </summary>
    [SugarColumn(ColumnName = "created_by", ColumnDescription = "创建人ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间(非空)
    /// </summary>
    [SugarColumn(ColumnName = "created_at", ColumnDescription = "创建时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新人ID
    /// </summary>
    [SugarColumn(ColumnName = "updated_by", ColumnDescription = "更新人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnName = "updated_at", ColumnDescription = "更新时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除(软删除标记,0=未删除,1=已删除)
    /// </summary>
    [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "是否删除", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [SugarColumn(ColumnName = "deleted_by", ColumnDescription = "删除人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [SugarColumn(ColumnName = "deleted_at", ColumnDescription = "删除时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? DeletedAt { get; set; }
}

// ========================================
// 审批级实体基类（对应 TaktApprovalEntityBase）
// ========================================

/// <summary>
/// 审批级实体基类
/// 包含租户+公司双重隔离+审批流程相关字段
/// 适用于需要审批的业务实体，如：请假单、报销单、采购单、合同等
/// 
/// 注意：此基类仅适用于简单审批（单级审批）
/// 如果是多级审批或复杂工作流，应使用独立的审批记录表（TaktApprovalRecord）
/// </summary>
public abstract class TaktApprovalEntityBase
{
    /// <summary>
    /// 主键ID(雪花ID,序列化为string以避免Javascript精度问题)
    /// </summary>
    [SugarColumn(ColumnName = "id", ColumnDescription = "主键ID", ColumnDataType = "bigint", IsPrimaryKey = true, IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long Id { get; set; }

    /// <summary>
    /// 租户编码(第一层数据隔离)
    /// </summary>
    [SugarColumn(ColumnName = "tenant_code", ColumnDescription = "租户编码", ColumnDataType = "varchar", Length = 3, IsNullable = false)]
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码(第二层数据隔离)
    /// </summary>
    [SugarColumn(ColumnName = "company_code", ColumnDescription = "公司代码", ColumnDataType = "varchar", Length = 4, IsNullable = false)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    [SugarColumn(ColumnName = "ext_field_json", ColumnDescription = "扩展字段JSON", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Remark { get; set; }

    /// <summary>
    /// 审批状态（0=待审批，1=审批中，2=已通过，3=已驳回，4=已撤销，5=已终止）
    /// </summary>
    [SugarColumn(ColumnName = "approval_status", ColumnDescription = "审批状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ApprovalStatus { get; set; } = 0;

    /// <summary>
    /// 发起人ID
    /// </summary>
    [SugarColumn(ColumnName = "initiator_id", ColumnDescription = "发起人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间
    /// </summary>
    [SugarColumn(ColumnName = "initiated_at", ColumnDescription = "发起时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? InitiatedAt { get; set; }

    /// <summary>
    /// 审批意见（支持多级审批时多条意见，用JSON数组存储）
    /// </summary>
    [SugarColumn(ColumnName = "approval_opinion", ColumnDescription = "审批意见", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ApprovalOpinion { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [SugarColumn(ColumnName = "approved_by", ColumnDescription = "最终审批人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间
    /// </summary>
    [SugarColumn(ColumnName = "approved_at", ColumnDescription = "最终审批时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// 创建人ID(非空;无当前用户时仓储填 999)
    /// </summary>
    [SugarColumn(ColumnName = "created_by", ColumnDescription = "创建人ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间(非空)
    /// </summary>
    [SugarColumn(ColumnName = "created_at", ColumnDescription = "创建时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新人ID
    /// </summary>
    [SugarColumn(ColumnName = "updated_by", ColumnDescription = "更新人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnName = "updated_at", ColumnDescription = "更新时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除(软删除标记,0=未删除,1=已删除)
    /// </summary>
    [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "是否删除", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [SugarColumn(ColumnName = "deleted_by", ColumnDescription = "删除人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    [SugarColumn(ColumnName = "deleted_at", ColumnDescription = "删除时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? DeletedAt { get; set; }
}
