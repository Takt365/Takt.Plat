// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktChangeoverDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：Changeover 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktChangeover 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

// ========================================
// Changeover 响应 DTO
// ========================================

/// <summary>
/// 切换记录实体
/// 对应前端 TaktChangeoverDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktChangeoverDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ChangeoverID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ChangeoverId { get; set; }

    /// <summary>
    /// 生产工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别
    /// </summary>
    public string? ProductionCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 读取SOP时间
    /// </summary>
    public decimal ReadSopTime { get; set; }

    /// <summary>
    /// 人数
    /// </summary>
    public int PersonCount { get; set; } = 0;

    /// <summary>
    /// SOP总时间
    /// </summary>
    public decimal TotalSopTime { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int ChangeoverCount { get; set; } = 0;

    /// <summary>
    /// 切换时间（单次）
    /// </summary>
    public decimal ChangeoverTime { get; set; }

    /// <summary>
    /// 切换总时间
    /// </summary>
    public decimal TotalChangeoverTime { get; set; }

}

// ========================================
// Changeover 查询 DTO
// ========================================

/// <summary>
/// Changeover 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktChangeoverQueryDto : TaktPagedQuery
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
    /// 生产工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别
    /// </summary>
    public string? ProductionCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期（范围查询-开始）
    /// </summary>
    public DateTime? ProductionDateStart { get; set; }

    /// <summary>
    /// 生产日期（范围查询-结束）
    /// </summary>
    public DateTime? ProductionDateEnd { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 读取SOP时间
    /// </summary>
    public decimal? ReadSopTime { get; set; }

    /// <summary>
    /// 人数
    /// </summary>
    public int? PersonCount { get; set; }

    /// <summary>
    /// SOP总时间
    /// </summary>
    public decimal? TotalSopTime { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? ChangeoverCount { get; set; }

    /// <summary>
    /// 切换时间（单次）
    /// </summary>
    public decimal? ChangeoverTime { get; set; }

    /// <summary>
    /// 切换总时间
    /// </summary>
    public decimal? TotalChangeoverTime { get; set; }

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
// 创建Changeover DTO
// ========================================

/// <summary>
/// 创建Changeover DTO
/// </summary>
public class TaktChangeoverCreateDto
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
    /// 生产工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别
    /// </summary>
    public string? ProductionCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 读取SOP时间
    /// </summary>
    public decimal ReadSopTime { get; set; }

    /// <summary>
    /// 人数
    /// </summary>
    public int PersonCount { get; set; } = 0;

    /// <summary>
    /// SOP总时间
    /// </summary>
    public decimal TotalSopTime { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int ChangeoverCount { get; set; } = 0;

    /// <summary>
    /// 切换时间（单次）
    /// </summary>
    public decimal ChangeoverTime { get; set; }

    /// <summary>
    /// 切换总时间
    /// </summary>
    public decimal TotalChangeoverTime { get; set; }

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
// 更新Changeover DTO
// ========================================

/// <summary>
/// 更新Changeover DTO
/// 继承 TaktChangeoverCreateDto，添加 ChangeoverId 字段
/// </summary>
public class TaktChangeoverUpdateDto : TaktChangeoverCreateDto
{
    /// <summary>
    /// ChangeoverID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ChangeoverId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Changeover 导入模板行 DTO
/// </summary>
public class TaktChangeoverTemplateDto
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
    /// 生产工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别
    /// </summary>
    public string? ProductionCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 人数
    /// </summary>
    public int? PersonCount { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? ChangeoverCount { get; set; }

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
/// Changeover 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktChangeoverImportDto
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
    /// 生产工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别
    /// </summary>
    public string? ProductionCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 人数
    /// </summary>
    public int? PersonCount { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? ChangeoverCount { get; set; }

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
/// Changeover 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktChangeoverExportDto
{
    /// <summary>
    /// ChangeoverID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ChangeoverId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工厂
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别
    /// </summary>
    public string? ProductionCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProductionDate { get; set; }

    /// <summary>
    /// 生产线
    /// </summary>
    public string? ProductionLine { get; set; } = string.Empty;

    /// <summary>
    /// 读取SOP时间
    /// </summary>
    public decimal ReadSopTime { get; set; }

    /// <summary>
    /// 人数
    /// </summary>
    public int PersonCount { get; set; } = 0;

    /// <summary>
    /// SOP总时间
    /// </summary>
    public decimal TotalSopTime { get; set; }

    /// <summary>
    /// 切换次数
    /// </summary>
    public int ChangeoverCount { get; set; } = 0;

    /// <summary>
    /// 切换时间（单次）
    /// </summary>
    public decimal ChangeoverTime { get; set; }

    /// <summary>
    /// 切换总时间
    /// </summary>
    public decimal TotalChangeoverTime { get; set; }

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
