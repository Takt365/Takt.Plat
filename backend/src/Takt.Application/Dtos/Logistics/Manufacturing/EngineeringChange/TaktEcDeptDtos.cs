// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptDtos.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：EcDept 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktEcDept 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

// ========================================
// EcDept 响应 DTO
// ========================================

/// <summary>
/// 设变-部门通用实体。部门顺序（严格）：技术(Eng)、生管(Pmc)、采购(Mp)、Iqc、部管(Mc)、制二(Pcba)、制一(Assy)、Qa、制技(Te)。通过 DeptCode 区分。
/// 对应前端 TaktEcDeptDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktEcDeptDto : TaktCompanyDtoBase
{
    /// <summary>
    /// EcDeptID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDeptId { get; set; }

    /// <summary>
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcnDetailId { get; set; }

    /// <summary>
    /// 设变明细名称（填充字段）
    /// </summary>
    public string? EcnDetailName { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 预计生产日期
    /// </summary>
    public DateTime? ScheduledProductionDate { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理
    /// </summary>
    public string? OldProductHandling { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验/检查日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProductionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; } = string.Empty;

    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; } = string.Empty;

    /// <summary>
    /// 是否更新SOP（0=否 1=是）
    /// </summary>
    public int IsSopUpdated { get; set; } = 0;

    /// <summary>
    /// 设变明细（多对一）
    /// （主表：TaktEcDetail）
    /// </summary>
    public TaktEcDetailDto? EcnDetail { get; set; }

}

// ========================================
// EcDept 查询 DTO
// ========================================

/// <summary>
/// EcDept 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktEcDeptQueryDto : TaktPagedQuery
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
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcnDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 预计生产日期（范围查询-开始）
    /// </summary>
    public DateTime? ScheduledProductionDateStart { get; set; }

    /// <summary>
    /// 预计生产日期（范围查询-结束）
    /// </summary>
    public DateTime? ScheduledProductionDateEnd { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理
    /// </summary>
    public string? OldProductHandling { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单发行日期（范围查询-开始）
    /// </summary>
    public DateTime? PurchaseOrderIssueDateStart { get; set; }

    /// <summary>
    /// 采购订单发行日期（范围查询-结束）
    /// </summary>
    public DateTime? PurchaseOrderIssueDateEnd { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验/检查日期（范围查询-开始）
    /// </summary>
    public DateTime? InspectionDateStart { get; set; }

    /// <summary>
    /// 检验/检查日期（范围查询-结束）
    /// </summary>
    public DateTime? InspectionDateEnd { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期（范围查询-开始）
    /// </summary>
    public DateTime? OutboundDateStart { get; set; }

    /// <summary>
    /// 出库日期（范围查询-结束）
    /// </summary>
    public DateTime? OutboundDateEnd { get; set; }

    /// <summary>
    /// 生产日期（范围查询-开始）
    /// </summary>
    public DateTime? ProductionDateStart { get; set; }

    /// <summary>
    /// 生产日期（范围查询-结束）
    /// </summary>
    public DateTime? ProductionDateEnd { get; set; }

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProductionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; } = string.Empty;

    /// <summary>
    /// 实施日期（范围查询-开始）
    /// </summary>
    public DateTime? ImplementationDateStart { get; set; }

    /// <summary>
    /// 实施日期（范围查询-结束）
    /// </summary>
    public DateTime? ImplementationDateEnd { get; set; }

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; } = string.Empty;

    /// <summary>
    /// 是否更新SOP（0=否 1=是）
    /// </summary>
    public int? IsSopUpdated { get; set; }

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
// 创建EcDept DTO
// ========================================

/// <summary>
/// 创建EcDept DTO
/// </summary>
public class TaktEcDeptCreateDto
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
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcnDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    [Required(ErrorMessage = "设变单号（冗余字段,便于查询）不能为空")]
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
    /// </summary>
    [Required(ErrorMessage = "部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。不能为空")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 预计生产日期
    /// </summary>
    public DateTime? ScheduledProductionDate { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理
    /// </summary>
    public string? OldProductHandling { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验/检查日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProductionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; } = string.Empty;

    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; } = string.Empty;

    /// <summary>
    /// 是否更新SOP（0=否 1=是）
    /// </summary>
    public int IsSopUpdated { get; set; } = 0;

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
// 更新EcDept DTO
// ========================================

/// <summary>
/// 更新EcDept DTO
/// 继承 TaktEcDeptCreateDto，添加 EcDeptId 字段
/// </summary>
public class TaktEcDeptUpdateDto : TaktEcDeptCreateDto
{
    /// <summary>
    /// EcDeptID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDeptId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// EcDept 导入模板行 DTO
/// </summary>
public class TaktEcDeptTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcnDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理
    /// </summary>
    public string? OldProductHandling { get; set; } = string.Empty;

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtFieldJson { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// EcDept 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktEcDeptImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EcnDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string? EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
    /// </summary>
    public string? DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int? IsImplemented { get; set; }

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理
    /// </summary>
    public string? OldProductHandling { get; set; } = string.Empty;

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

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
// 导出 DTO
// ========================================

/// <summary>
/// EcDept 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktEcDeptExportDto
{
    /// <summary>
    /// EcDeptID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcDeptId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 设变明细ID（TaktEcDetail 主键）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EcnDetailId { get; set; }

    /// <summary>
    /// 设变单号（冗余字段,便于查询）
    /// </summary>
    public string EcNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
    /// </summary>
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    /// 是否实施（0=否 1=是）
    /// </summary>
    public int IsImplemented { get; set; } = 0;

    /// <summary>
    /// 内容（各部门通用）
    /// </summary>
    public string? Content { get; set; } = string.Empty;

    /// <summary>
    /// 预计生产日期
    /// </summary>
    public DateTime? ScheduledProductionDate { get; set; }

    /// <summary>
    /// 预定批次
    /// </summary>
    public string? ScheduledBatch { get; set; } = string.Empty;

    /// <summary>
    /// Po残（采购订单残）
    /// </summary>
    public string? PoRemainder { get; set; } = string.Empty;

    /// <summary>
    /// 结余
    /// </summary>
    public string? Balance { get; set; } = string.Empty;

    /// <summary>
    /// 旧品处理
    /// </summary>
    public string? OldProductHandling { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单发行日期
    /// </summary>
    public DateTime? PurchaseOrderIssueDate { get; set; }

    /// <summary>
    /// 供应商
    /// </summary>
    public string? Supplier { get; set; } = string.Empty;

    /// <summary>
    /// 采购订单号码
    /// </summary>
    public string? PurchaseOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 受检单号
    /// </summary>
    public string? IqcOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验/检查日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 出库批次
    /// </summary>
    public string? OutboundBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime? OutboundDate { get; set; }

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// 生产批次
    /// </summary>
    public string? ProductionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号
    /// </summary>
    public string? OutboundOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组
    /// </summary>
    public string? ProductionTeam { get; set; } = string.Empty;

    /// <summary>
    /// 实施日期
    /// </summary>
    public DateTime? ImplementationDate { get; set; }

    /// <summary>
    /// 检验批次
    /// </summary>
    public string? InspectionBatch { get; set; } = string.Empty;

    /// <summary>
    /// 抽样号码
    /// </summary>
    public string? SamplingNo { get; set; } = string.Empty;

    /// <summary>
    /// 是否更新SOP（0=否 1=是）
    /// </summary>
    public int IsSopUpdated { get; set; } = 0;

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
