// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcDetailDtos.cs
// 创建时间：2026-06-27
// 创建人：Takt365(Auto Generated)
// 功能描述：SourceEcDetail 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSourceEcDetail 生成，请按需审阅）
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
// SourceEcDetail 响应 DTO
// ========================================

/// <summary>
/// 设变来源子表实体。
/// 对应前端 TaktSourceEcDetailDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSourceEcDetailDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SourceEcDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcDetailId { get; set; }

    /// <summary>
    /// 主ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 主名称（填充字段）
    /// </summary>
    public string? SourceEcName { get; set; }

    /// <summary>
    /// 完成品
    /// </summary>
    public string SourceFinishedProduct { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料
    /// </summary>
    public string SourceParentPart { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? SourceLegacyPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料
    /// </summary>
    public string? SourceLegacyPartName { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceLegacyUsage { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceLegacyMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartName { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceReplacementUsage { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceReplacementMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 互换性
    /// </summary>
    public string? SourceInterchangeability { get; set; } = string.Empty;

    /// <summary>
    /// 区分
    /// </summary>
    public string? SourceDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示
    /// </summary>
    public string? SourceArrangementInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理
    /// </summary>
    public string? SourceLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? SourceBomEffectiveDate { get; set; }

    /// <summary>
    /// 设变来源主表
    /// （主表：TaktSourceEc）
    /// </summary>
    public TaktSourceEcDto? SourceEc { get; set; }

}

// ========================================
// SourceEcDetail 查询 DTO
// ========================================

/// <summary>
/// SourceEcDetail 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSourceEcDetailQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 主ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SourceEcId { get; set; }

    /// <summary>
    /// 完成品
    /// </summary>
    public string? SourceFinishedProduct { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料
    /// </summary>
    public string? SourceParentPart { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? SourceLegacyPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料
    /// </summary>
    public string? SourceLegacyPartName { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceLegacyUsage { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceLegacyMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartName { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceReplacementUsage { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceReplacementMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 互换性
    /// </summary>
    public string? SourceInterchangeability { get; set; } = string.Empty;

    /// <summary>
    /// 区分
    /// </summary>
    public string? SourceDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示
    /// </summary>
    public string? SourceArrangementInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理
    /// </summary>
    public string? SourceLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期（范围查询-开始）
    /// </summary>
    public DateTime? SourceBomEffectiveDateStart { get; set; }

    /// <summary>
    /// BOM生效日期（范围查询-结束）
    /// </summary>
    public DateTime? SourceBomEffectiveDateEnd { get; set; }

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
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建SourceEcDetail DTO
// ========================================

/// <summary>
/// 创建SourceEcDetail DTO
/// </summary>
public class TaktSourceEcDetailCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 主ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 完成品
    /// </summary>
    [Required(ErrorMessage = "完成品不能为空")]
    public string SourceFinishedProduct { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料
    /// </summary>
    [Required(ErrorMessage = "上阶物料不能为空")]
    public string SourceParentPart { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? SourceLegacyPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料
    /// </summary>
    public string? SourceLegacyPartName { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceLegacyUsage { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceLegacyMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartName { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceReplacementUsage { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceReplacementMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 互换性
    /// </summary>
    public string? SourceInterchangeability { get; set; } = string.Empty;

    /// <summary>
    /// 区分
    /// </summary>
    public string? SourceDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示
    /// </summary>
    public string? SourceArrangementInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理
    /// </summary>
    public string? SourceLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? SourceBomEffectiveDate { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新SourceEcDetail DTO
// ========================================

/// <summary>
/// 更新SourceEcDetail DTO
/// 继承 TaktSourceEcDetailCreateDto，添加 SourceEcDetailId 字段
/// </summary>
public class TaktSourceEcDetailUpdateDto : TaktSourceEcDetailCreateDto
{
    /// <summary>
    /// SourceEcDetailID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcDetailId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SourceEcDetail 导入模板行 DTO
/// </summary>
public class TaktSourceEcDetailTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 主ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SourceEcId { get; set; }

    /// <summary>
    /// 完成品
    /// </summary>
    public string? SourceFinishedProduct { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料
    /// </summary>
    public string? SourceParentPart { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? SourceLegacyPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料
    /// </summary>
    public string? SourceLegacyPartName { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceLegacyUsage { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceLegacyMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartName { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceReplacementUsage { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceReplacementMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 互换性
    /// </summary>
    public string? SourceInterchangeability { get; set; } = string.Empty;

    /// <summary>
    /// 区分
    /// </summary>
    public string? SourceDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示
    /// </summary>
    public string? SourceArrangementInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理
    /// </summary>
    public string? SourceLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? SourceBomEffectiveDate { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// SourceEcDetail 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSourceEcDetailImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 主ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SourceEcId { get; set; }

    /// <summary>
    /// 完成品
    /// </summary>
    public string? SourceFinishedProduct { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料
    /// </summary>
    public string? SourceParentPart { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? SourceLegacyPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料
    /// </summary>
    public string? SourceLegacyPartName { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceLegacyUsage { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceLegacyMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartName { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceReplacementUsage { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceReplacementMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 互换性
    /// </summary>
    public string? SourceInterchangeability { get; set; } = string.Empty;

    /// <summary>
    /// 区分
    /// </summary>
    public string? SourceDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示
    /// </summary>
    public string? SourceArrangementInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理
    /// </summary>
    public string? SourceLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? SourceBomEffectiveDate { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// SourceEcDetail 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSourceEcDetailExportDto
{
    /// <summary>
    /// SourceEcDetailID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcDetailId { get; set; }

    /// <summary>
    /// 主ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SourceEcId { get; set; }

    /// <summary>
    /// 完成品
    /// </summary>
    public string SourceFinishedProduct { get; set; } = string.Empty;

    /// <summary>
    /// 上阶物料
    /// </summary>
    public string SourceParentPart { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料号
    /// </summary>
    public string? SourceLegacyPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料
    /// </summary>
    public string? SourceLegacyPartName { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料用量
    /// </summary>
    public decimal? SourceLegacyUsage { get; set; }

    /// <summary>
    /// 旧物料安装位置
    /// </summary>
    public string? SourceLegacyMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartNo { get; set; } = string.Empty;

    /// <summary>
    /// 新物料
    /// </summary>
    public string? SourceReplacementPartName { get; set; } = string.Empty;

    /// <summary>
    /// 新物料用量
    /// </summary>
    public decimal? SourceReplacementUsage { get; set; }

    /// <summary>
    /// 新物料安装位置
    /// </summary>
    public string? SourceReplacementMountingPosition { get; set; } = string.Empty;

    /// <summary>
    /// BOM番号
    /// </summary>
    public string? SourceBomNo { get; set; } = string.Empty;

    /// <summary>
    /// 互换性
    /// </summary>
    public string? SourceInterchangeability { get; set; } = string.Empty;

    /// <summary>
    /// 区分
    /// </summary>
    public string? SourceDistinction { get; set; } = string.Empty;

    /// <summary>
    /// 安排指示
    /// </summary>
    public string? SourceArrangementInstruction { get; set; } = string.Empty;

    /// <summary>
    /// 旧物料处理
    /// </summary>
    public string? SourceLegacyPartDisposition { get; set; } = string.Empty;

    /// <summary>
    /// BOM生效日期
    /// </summary>
    public DateTime? SourceBomEffectiveDate { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
