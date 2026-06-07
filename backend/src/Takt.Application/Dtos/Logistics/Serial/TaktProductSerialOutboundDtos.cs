// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Serial
// 文件名称：TaktProductSerialOutboundDtos.cs
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductSerialOutbound 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductSerialOutbound 生成，请按需审阅）
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
// ProductSerialOutbound 响应 DTO
// ========================================

/// <summary>
/// 产品序列号出库主表实体
/// 对应前端 TaktProductSerialOutboundDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductSerialOutboundDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductSerialOutboundID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductSerialOutboundId { get; set; }

    /// <summary>
    /// 工厂代码(4位字母数字组合)
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号(组合唯一索引:PlantCode + OutboundNo)
    /// </summary>
    public string OutboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 出货发票号
    /// </summary>
    public string ShippingInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime OutboundDate { get; set; }

    /// <summary>
    /// 仕向地(目的地)
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
    /// </summary>
    public string ShippingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港
    /// </summary>
    public string DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
    /// </summary>
    public int OutboundType { get; set; } = 0;

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联公司
    /// </summary>
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 产品序列号出库明细列表（主子表关系）
    /// （子表：TaktProductSerialOutboundItem）
    /// </summary>
    public List<TaktProductSerialOutboundItemDto>? Items { get; set; }

}

// ========================================
// ProductSerialOutbound 查询 DTO
// ========================================

/// <summary>
/// ProductSerialOutbound 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductSerialOutboundQueryDto : TaktPagedQuery
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
    /// 出库单号(组合唯一索引:PlantCode + OutboundNo)
    /// </summary>
    public string? OutboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 出货发票号
    /// </summary>
    public string? ShippingInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期（范围查询-开始）
    /// </summary>
    public DateTime? OutboundDateStart { get; set; }

    /// <summary>
    /// 出库日期（范围查询-结束）
    /// </summary>
    public DateTime? OutboundDateEnd { get; set; }

    /// <summary>
    /// 仕向地(目的地)
    /// </summary>
    public string? Destination { get; set; } = string.Empty;

    /// <summary>
    /// 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
    /// </summary>
    public string? ShippingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港
    /// </summary>
    public string? DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
    /// </summary>
    public int? OutboundType { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联公司
    /// </summary>
    public string? RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int? TotalQuantity { get; set; }

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
// 创建ProductSerialOutbound DTO
// ========================================

/// <summary>
/// 创建ProductSerialOutbound DTO
/// </summary>
public class TaktProductSerialOutboundCreateDto
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
    /// 出库单号(组合唯一索引:PlantCode + OutboundNo)
    /// </summary>
    [Required(ErrorMessage = "出库单号(组合唯一索引:PlantCode + OutboundNo)不能为空")]
    public string OutboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 出货发票号
    /// </summary>
    [Required(ErrorMessage = "出货发票号不能为空")]
    public string ShippingInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime OutboundDate { get; set; }

    /// <summary>
    /// 仕向地(目的地)
    /// </summary>
    [Required(ErrorMessage = "仕向地(目的地)不能为空")]
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
    /// </summary>
    [Required(ErrorMessage = "运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)不能为空")]
    public string ShippingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港
    /// </summary>
    [Required(ErrorMessage = "目的地港不能为空")]
    public string DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
    /// </summary>
    public int OutboundType { get; set; } = 0;

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
    /// 关联公司
    /// </summary>
    [Required(ErrorMessage = "关联公司不能为空")]
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

    /// <summary>
    /// 产品序列号出库明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktProductSerialOutboundItemCreateDto>? Items { get; set; }

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
// 更新ProductSerialOutbound DTO
// ========================================

/// <summary>
/// 更新ProductSerialOutbound DTO
/// 继承 TaktProductSerialOutboundCreateDto，添加 ProductSerialOutboundId 字段
/// </summary>
public class TaktProductSerialOutboundUpdateDto : TaktProductSerialOutboundCreateDto
{
    /// <summary>
    /// ProductSerialOutboundID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductSerialOutboundId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductSerialOutbound 导入模板行 DTO
/// </summary>
public class TaktProductSerialOutboundTemplateDto
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
    /// 出库单号(组合唯一索引:PlantCode + OutboundNo)
    /// </summary>
    public string? OutboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 出货发票号
    /// </summary>
    public string? ShippingInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地(目的地)
    /// </summary>
    public string? Destination { get; set; } = string.Empty;

    /// <summary>
    /// 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
    /// </summary>
    public string? ShippingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港
    /// </summary>
    public string? DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
    /// </summary>
    public int? OutboundType { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联公司
    /// </summary>
    public string? RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int? TotalQuantity { get; set; }

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
/// ProductSerialOutbound 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductSerialOutboundImportDto
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
    /// 出库单号(组合唯一索引:PlantCode + OutboundNo)
    /// </summary>
    public string? OutboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 出货发票号
    /// </summary>
    public string? ShippingInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地(目的地)
    /// </summary>
    public string? Destination { get; set; } = string.Empty;

    /// <summary>
    /// 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
    /// </summary>
    public string? ShippingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港
    /// </summary>
    public string? DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
    /// </summary>
    public int? OutboundType { get; set; }

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联公司
    /// </summary>
    public string? RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int? TotalQuantity { get; set; }

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
/// ProductSerialOutbound 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductSerialOutboundExportDto
{
    /// <summary>
    /// ProductSerialOutboundID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductSerialOutboundId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码(4位字母数字组合)
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 出库单号(组合唯一索引:PlantCode + OutboundNo)
    /// </summary>
    public string OutboundNo { get; set; } = string.Empty;

    /// <summary>
    /// 出货发票号
    /// </summary>
    public string ShippingInvoiceNo { get; set; } = string.Empty;

    /// <summary>
    /// 出库日期
    /// </summary>
    public DateTime OutboundDate { get; set; }

    /// <summary>
    /// 仕向地(目的地)
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
    /// </summary>
    public string ShippingMethod { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港
    /// </summary>
    public string DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
    /// </summary>
    public int OutboundType { get; set; } = 0;

    /// <summary>
    /// 仓库编码
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 库位编码
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联公司
    /// </summary>
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalQuantity { get; set; } = 0;

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
