// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktAssetChangeLogDtos.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：AssetChangeLog 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktAssetChangeLog 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// AssetChangeLog 响应 DTO
// ========================================

/// <summary>
/// 资产变更记录实体
/// 对应前端 TaktAssetChangeLogDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktAssetChangeLogDto : TaktCompanyDtoBase
{
    /// <summary>
    /// AssetChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssetChangeLogId { get; set; }

    /// <summary>
    /// 资产 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssetId { get; set; }

    /// <summary>
    /// 资产 名称（填充字段）
    /// </summary>
    public string? AssetName { get; set; }

    /// <summary>
    /// 资产编码（冗余）
    /// </summary>
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表 JSON
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人
    /// </summary>
    public string? ChangeBy { get; set; } = string.Empty;

    /// <summary>
    /// 变更原因
    /// </summary>
    public string? ChangeReason { get; set; } = string.Empty;

}

// ========================================
// AssetChangeLog 查询 DTO
// ========================================

/// <summary>
/// AssetChangeLog 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktAssetChangeLogQueryDto : TaktPagedQuery
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
    /// 资产 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AssetId { get; set; }

    /// <summary>
    /// 资产编码（冗余）
    /// </summary>
    public string? AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表 JSON
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
    /// 变更人
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
// 创建AssetChangeLog DTO
// ========================================

/// <summary>
/// 创建AssetChangeLog DTO
/// </summary>
public class TaktAssetChangeLogCreateDto
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
    /// 资产 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssetId { get; set; }

    /// <summary>
    /// 资产编码（冗余）
    /// </summary>
    [Required(ErrorMessage = "资产编码（冗余）不能为空")]
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表 JSON
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人
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
// 更新AssetChangeLog DTO
// ========================================

/// <summary>
/// 更新AssetChangeLog DTO
/// 继承 TaktAssetChangeLogCreateDto，添加 AssetChangeLogId 字段
/// </summary>
public class TaktAssetChangeLogUpdateDto : TaktAssetChangeLogCreateDto
{
    /// <summary>
    /// AssetChangeLogID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssetChangeLogId { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// AssetChangeLog 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktAssetChangeLogExportDto
{
    /// <summary>
    /// AssetChangeLogID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssetChangeLogId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 资产 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AssetId { get; set; }

    /// <summary>
    /// 资产编码（冗余）
    /// </summary>
    public string AssetCode { get; set; } = string.Empty;

    /// <summary>
    /// 变更字段列表 JSON
    /// </summary>
    public string? ChangeFields { get; set; } = string.Empty;

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更人
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
