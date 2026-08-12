// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos
// 文件名称：TaktDtoBase.cs
// 创建时间：2026-05-20
// 创建人：Takt365(Cursor AI)
// 功能描述：DTO 基类，对齐 TaktTenant/Company/ApprovalEntityBase（租户 RelatedPlant+CultureCode；公司/审批 PlantCode+CultureCode）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos;

// ========================================
// 租户级 DTO 基类（对应 TaktTenantEntityBase）
// ========================================

/// <summary>
/// 租户级 DTO 基类
/// 对应实体基类 TaktTenantEntityBase
/// 包含租户隔离字段和审计字段
/// 适用于用户、角色、菜单等跨公司共享的实体 DTO
/// 注意：不包含 Id，Id 由各具体 DTO 根据需要定义
/// </summary>
public abstract class TaktTenantDtoBase
{
    /// <summary>
    /// 租户编码（第一层数据隔离）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（租户级实体不使用公司隔离，固定为空字符串）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code；行戳记，创建时可由仓储按公司主档注入）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建人ID（非空；无当前用户时仓储填 900001）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间（非空）
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除（软删除标记，0=未删除，1=已删除）
    /// </summary>
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}

// ========================================
// 公司级 DTO 基类（对应 TaktCompanyEntityBase）
// ========================================

/// <summary>
/// 公司级 DTO 基类
/// 对应实体基类 TaktCompanyEntityBase
/// 包含租户+公司双重隔离和审计字段
/// 适用于部门、岗位、员工等业务实体 DTO
/// 注意：不包含 Id，Id 由各具体 DTO 根据需要定义
/// </summary>
public abstract class TaktCompanyDtoBase
{
    /// <summary>
    /// 租户编码（第一层数据隔离）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（第二层数据隔离）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code；与当前公司 CultureCode 一致，创建时由仓储注入）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建人ID（非空；无当前用户时仓储填 900001）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间（非空）
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除（软删除标记，0=未删除，1=已删除）
    /// </summary>
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}

// ========================================
// 审批 DTO 基类（对应 TaktApprovalEntityBase）
// ========================================

/// <summary>
/// 审批级 DTO 基类
/// 对应实体基类 TaktApprovalEntityBase（含 FlowInstanceId；凡审批必走 TaktFlowEngine）
/// 注意：不包含 Id，Id 由各具体 DTO 根据需要定义
/// </summary>
public abstract class TaktApprovalDtoBase
{
    /// <summary>
    /// 租户编码（第一层数据隔离）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（第二层数据隔离）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（字典 sys_culture_code；与当前公司 CultureCode 一致，创建时由仓储注入）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
    /// </summary>
    public int ApprovalStatus { get; set; } = 0;

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间
    /// </summary>
    public DateTime? InitiatedAt { get; set; }

    /// <summary>
    /// 审批意见（支持多级审批时多条意见，用JSON数组存储）
    /// </summary>
    public string? ApprovalOpinion { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// 流程实例 ID（关联工作流流程实例表 takt_workflow_instance；StartFlowInstance 后由业务写入）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 创建人ID（非空；无当前用户时仓储填 900001）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CreatedBy { get; set; }

    /// <summary>
    /// 创建时间（非空）
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否删除（软删除标记，0=未删除，1=已删除）
    /// </summary>
    public int IsDeleted { get; set; } = 0;

    /// <summary>
    /// 删除人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? DeletedBy { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DeletedAt { get; set; }
}
