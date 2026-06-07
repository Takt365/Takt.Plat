// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialChangeLogDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：BillOfMaterialChangeLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBillOfMaterialChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// BillOfMaterialChangeLog 响应 DTO
// ========================================

/// <summary>
/// BOM变更记录实体
/// 对应前端 TaktBillOfMaterialChangeLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBillOfMaterialChangeLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BillOfMaterialChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialChangeLogId { get; set; }

    /// <summary>
    /// 物料清单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// 物料清单名称（填充字段）
    /// </summary>
    public string? BillOfMaterialName { get; set; }

    /// <summary>
    /// BOM编码
    /// </summary>
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// BOM（主表）
    /// （主表：TaktBillOfMaterial）
    /// </summary>
    public TaktBillOfMaterialDto? Bom { get; set; }

}

// ========================================
// BillOfMaterialChangeLog 查询 DTO
// ========================================

/// <summary>
/// BillOfMaterialChangeLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBillOfMaterialChangeLogQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料清单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码
    /// </summary>
    public string? BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间（范围查询-开始）
    /// </summary>
    public DateTime? ChangeTimeStart { get; set; }

    /// <summary>
    /// 变更时间（范围查询-结束）
    /// </summary>
    public DateTime? ChangeTimeEnd { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建BillOfMaterialChangeLog DTO
// ========================================

/// <summary>
/// 创建BillOfMaterialChangeLog DTO
/// </summary>
public class TaktBillOfMaterialChangeLogCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 物料清单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码
    /// </summary>
    [Required(ErrorMessage = "BOM编码不能为空")]
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新BillOfMaterialChangeLog DTO
// ========================================

/// <summary>
/// 更新BillOfMaterialChangeLog DTO
/// 继承 TaktBillOfMaterialChangeLogCreateDto，添加 BillOfMaterialChangeLogId 字段
/// </summary>
public class TaktBillOfMaterialChangeLogUpdateDto : TaktBillOfMaterialChangeLogCreateDto
{
    /// <summary>
    /// BillOfMaterialChangeLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialChangeLogId { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// BillOfMaterialChangeLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBillOfMaterialChangeLogExportDto
{
    /// <summary>
    /// BillOfMaterialChangeLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialChangeLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料清单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码
    /// </summary>
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人（人员代码）
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
