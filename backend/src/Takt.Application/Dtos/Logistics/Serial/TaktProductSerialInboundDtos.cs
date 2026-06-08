// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Serial
// 文件名称：TaktProductSerialInboundDtos.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductSerialInbound 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductSerialInbound 生成，请按需审阅）
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
// ProductSerialInbound 响应 DTO
// ========================================

/// <summary>
/// 产品序列号入库主表实体
/// 对应前端 TaktProductSerialInboundDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductSerialInboundDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductSerialInboundID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductSerialInboundId { get; set; }

    /// <summary>
    /// 工厂代码(4位字母数字组合)
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库单号（组合唯一索引：PlantCode + InboundNo）
    /// </summary>
    public string InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime InboundDate { get; set; }

    /// <summary>
    /// 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
    /// </summary>
    public int InboundType { get; set; } = 0;

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 关联公司
    /// </summary>
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 产品序列号入库明细列表(主子表关系)
    /// （子表：TaktProductSerialInboundItem）
    /// </summary>
    public List<TaktProductSerialInboundItemDto>? Items { get; set; }

}

// ========================================
// ProductSerialInbound 查询 DTO
// ========================================

/// <summary>
/// ProductSerialInbound 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductSerialInboundQueryDto : TaktPagedQuery
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
    /// 工厂代码(4位字母数字组合)
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库单号（组合唯一索引：PlantCode + InboundNo）
    /// </summary>
    public string? InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期（范围查询-开始）
    /// </summary>
    public DateTime? InboundDateStart { get; set; }

    /// <summary>
    /// 入库日期（范围查询-结束）
    /// </summary>
    public DateTime? InboundDateEnd { get; set; }

    /// <summary>
    /// 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
    /// </summary>
    public int? InboundType { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int? TotalQuantity { get; set; }

    /// <summary>
    /// 关联公司
    /// </summary>
    public string? RelatedCompany { get; set; } = string.Empty;

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
// 创建ProductSerialInbound DTO
// ========================================

/// <summary>
/// 创建ProductSerialInbound DTO
/// </summary>
public class TaktProductSerialInboundCreateDto
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
    /// 工厂代码(4位字母数字组合)
    /// </summary>
    [Required(ErrorMessage = "工厂代码(4位字母数字组合)不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库单号（组合唯一索引：PlantCode + InboundNo）
    /// </summary>
    [Required(ErrorMessage = "入库单号（组合唯一索引：PlantCode + InboundNo）不能为空")]
    public string InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime InboundDate { get; set; }

    /// <summary>
    /// 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
    /// </summary>
    public int InboundType { get; set; } = 0;

    /// <summary>
    /// 仓库编码
    /// </summary>
    [Required(ErrorMessage = "仓库编码不能为空")]
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    [Required(ErrorMessage = "库位编码不能为空")]
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 关联公司
    /// </summary>
    [Required(ErrorMessage = "关联公司不能为空")]
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 产品序列号入库明细列表(主子表关系)（子表，级联保存）
    /// </summary>
    public List<TaktProductSerialInboundItemCreateDto>? Items { get; set; }

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
// 更新ProductSerialInbound DTO
// ========================================

/// <summary>
/// 更新ProductSerialInbound DTO
/// 继承 TaktProductSerialInboundCreateDto，添加 ProductSerialInboundId 字段
/// </summary>
public class TaktProductSerialInboundUpdateDto : TaktProductSerialInboundCreateDto
{
    /// <summary>
    /// ProductSerialInboundID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductSerialInboundId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductSerialInbound 导入模板行 DTO
/// </summary>
public class TaktProductSerialInboundTemplateDto
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
    /// 工厂代码(4位字母数字组合)
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库单号（组合唯一索引：PlantCode + InboundNo）
    /// </summary>
    public string? InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
    /// </summary>
    public int? InboundType { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int? TotalQuantity { get; set; }

    /// <summary>
    /// 关联公司
    /// </summary>
    public string? RelatedCompany { get; set; } = string.Empty;

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
/// ProductSerialInbound 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductSerialInboundImportDto
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
    /// 工厂代码(4位字母数字组合)
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库单号（组合唯一索引：PlantCode + InboundNo）
    /// </summary>
    public string? InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
    /// </summary>
    public int? InboundType { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int? TotalQuantity { get; set; }

    /// <summary>
    /// 关联公司
    /// </summary>
    public string? RelatedCompany { get; set; } = string.Empty;

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
/// ProductSerialInbound 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductSerialInboundExportDto
{
    /// <summary>
    /// ProductSerialInboundID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductSerialInboundId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码(4位字母数字组合)
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 入库单号（组合唯一索引：PlantCode + InboundNo）
    /// </summary>
    public string InboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库日期
    /// </summary>
    public DateTime InboundDate { get; set; }

    /// <summary>
    /// 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
    /// </summary>
    public int InboundType { get; set; } = 0;

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 关联公司
    /// </summary>
    public string RelatedCompany { get; set; } = string.Empty;

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
