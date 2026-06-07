// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Serial
// 文件名称：TaktProductSerialInboundItemDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductSerialInboundItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductSerialInboundItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Serial;

// ========================================
// ProductSerialInboundItem 响应 DTO
// ========================================

/// <summary>
/// 产品序列号入库明细实体
/// 对应前端 TaktProductSerialInboundItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductSerialInboundItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductSerialInboundItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductSerialInboundItemId { get; set; }

    /// <summary>
    /// 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InboundId { get; set; }

    /// <summary>
    /// 入库名称（填充字段）
    /// </summary>
    public string? InboundName { get; set; }

    /// <summary>
    /// 入库单号（冗余字段，便于查询）
    /// </summary>
    public string InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 入库序列号（唯一索引）
    /// </summary>
    public string InboundSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库时间
    /// </summary>
    public DateTime InboundTime { get; set; }

    /// <summary>
    /// 入库主表
    /// （主表：TaktProductSerialInbound）
    /// </summary>
    public TaktProductSerialInboundDto? Inbound { get; set; }

}

// ========================================
// ProductSerialInboundItem 查询 DTO
// ========================================

/// <summary>
/// ProductSerialInboundItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductSerialInboundItemQueryDto : TaktPagedQuery
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
    /// 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InboundId { get; set; }

    /// <summary>
    /// 入库单号（冗余字段，便于查询）
    /// </summary>
    public string? InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 入库序列号（唯一索引）
    /// </summary>
    public string? InboundSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库时间（范围查询-开始）
    /// </summary>
    public DateTime? InboundTimeStart { get; set; }

    /// <summary>
    /// 入库时间（范围查询-结束）
    /// </summary>
    public DateTime? InboundTimeEnd { get; set; }

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
// 创建ProductSerialInboundItem DTO
// ========================================

/// <summary>
/// 创建ProductSerialInboundItem DTO
/// </summary>
public class TaktProductSerialInboundItemCreateDto
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
    /// 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InboundId { get; set; }

    /// <summary>
    /// 入库单号（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "入库单号（冗余字段，便于查询）不能为空")]
    public string InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 入库序列号（唯一索引）
    /// </summary>
    [Required(ErrorMessage = "入库序列号（唯一索引）不能为空")]
    public string InboundSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库时间
    /// </summary>
    public DateTime InboundTime { get; set; }

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
// 更新ProductSerialInboundItem DTO
// ========================================

/// <summary>
/// 更新ProductSerialInboundItem DTO
/// 继承 TaktProductSerialInboundItemCreateDto，添加 ProductSerialInboundItemId 字段
/// </summary>
public class TaktProductSerialInboundItemUpdateDto : TaktProductSerialInboundItemCreateDto
{
    /// <summary>
    /// ProductSerialInboundItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductSerialInboundItemId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductSerialInboundItem 导入模板行 DTO
/// </summary>
public class TaktProductSerialInboundItemTemplateDto
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
    /// 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InboundId { get; set; }

    /// <summary>
    /// 入库单号（冗余字段，便于查询）
    /// </summary>
    public string? InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 入库序列号（唯一索引）
    /// </summary>
    public string? InboundSerialNo { get; set; } = string.Empty;

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
/// ProductSerialInboundItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductSerialInboundItemImportDto
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
    /// 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InboundId { get; set; }

    /// <summary>
    /// 入库单号（冗余字段，便于查询）
    /// </summary>
    public string? InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 入库序列号（唯一索引）
    /// </summary>
    public string? InboundSerialNo { get; set; } = string.Empty;

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
/// ProductSerialInboundItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductSerialInboundItemExportDto
{
    /// <summary>
    /// ProductSerialInboundItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductSerialInboundItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long InboundId { get; set; }

    /// <summary>
    /// 入库单号（冗余字段，便于查询）
    /// </summary>
    public string InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 入库序列号（唯一索引）
    /// </summary>
    public string InboundSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库时间
    /// </summary>
    public DateTime InboundTime { get; set; }

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
