// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialTransactionDtos.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialTransaction 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialTransaction 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// MaterialTransaction 响应 DTO
// ========================================

/// <summary>
/// Takt物料交易主表实体（公司级；覆盖后勤模块收发货、库内作业、领借还与调拨核销等业务）
/// 对应前端 TaktMaterialTransactionDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMaterialTransactionDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MaterialTransactionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionId { get; set; }

    /// <summary>
    /// 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料交易单号（租户+公司+工厂内唯一）
    /// </summary>
    public string MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 交易日期
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// 交易方向（0=入库，1=出库，2=库内/移库）
    /// </summary>
    public int TransactionDirection { get; set; } = 0;

    /// <summary>
    /// 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
    /// </summary>
    public int TransactionType { get; set; } = 0;

    /// <summary>
    /// 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
    /// </summary>
    public int BusinessAction { get; set; } = 0;

    /// <summary>
    /// 来源单号（采购订单、销售订单、生产订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方编码（供应商、客户或部门等业务编码）
    /// </summary>
    public string? PartnerCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方名称
    /// </summary>
    public string? PartnerName { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? TargetWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? TargetLocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联公司
    /// </summary>
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 交易总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 交易状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    public int TransactionStatus { get; set; } = 0;

    /// <summary>
    /// 过账日期
    /// </summary>
    public DateTime? PostedDate { get; set; }

    /// <summary>
    /// 过账人（人员代码）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料交易明细列表（主子表关系）
    /// （子表：TaktMaterialTransactionItem）
    /// </summary>
    public List<TaktMaterialTransactionItemDto>? Items { get; set; }

}

// ========================================
// MaterialTransaction 查询 DTO
// ========================================

/// <summary>
/// MaterialTransaction 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialTransactionQueryDto : TaktPagedQuery
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
    /// 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料交易单号（租户+公司+工厂内唯一）
    /// </summary>
    public string? MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 交易日期（范围查询-开始）
    /// </summary>
    public DateTime? TransactionDateStart { get; set; }

    /// <summary>
    /// 交易日期（范围查询-结束）
    /// </summary>
    public DateTime? TransactionDateEnd { get; set; }

    /// <summary>
    /// 交易方向（0=入库，1=出库，2=库内/移库）
    /// </summary>
    public int? TransactionDirection { get; set; }

    /// <summary>
    /// 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
    /// </summary>
    public int? TransactionType { get; set; }

    /// <summary>
    /// 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
    /// </summary>
    public int? BusinessAction { get; set; }

    /// <summary>
    /// 来源单号（采购订单、销售订单、生产订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方编码（供应商、客户或部门等业务编码）
    /// </summary>
    public string? PartnerCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方名称
    /// </summary>
    public string? PartnerName { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? TargetWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? TargetLocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联公司
    /// </summary>
    public string? RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 交易总数量（基本单位数量）
    /// </summary>
    public decimal? TotalQuantity { get; set; }

    /// <summary>
    /// 交易状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    public int? TransactionStatus { get; set; }

    /// <summary>
    /// 过账日期（范围查询-开始）
    /// </summary>
    public DateTime? PostedDateStart { get; set; }

    /// <summary>
    /// 过账日期（范围查询-结束）
    /// </summary>
    public DateTime? PostedDateEnd { get; set; }

    /// <summary>
    /// 过账人（人员代码）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

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
// 创建MaterialTransaction DTO
// ========================================

/// <summary>
/// 创建MaterialTransaction DTO
/// </summary>
public class TaktMaterialTransactionCreateDto
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
    /// 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料交易单号（租户+公司+工厂内唯一）
    /// </summary>
    [Required(ErrorMessage = "物料交易单号（租户+公司+工厂内唯一）不能为空")]
    public string MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 交易日期
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// 交易方向（0=入库，1=出库，2=库内/移库）
    /// </summary>
    public int TransactionDirection { get; set; } = 0;

    /// <summary>
    /// 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
    /// </summary>
    public int TransactionType { get; set; } = 0;

    /// <summary>
    /// 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
    /// </summary>
    public int BusinessAction { get; set; } = 0;

    /// <summary>
    /// 来源单号（采购订单、销售订单、生产订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方编码（供应商、客户或部门等业务编码）
    /// </summary>
    public string? PartnerCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方名称
    /// </summary>
    public string? PartnerName { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    [Required(ErrorMessage = "源仓库编码（关联 TaktWarehouse.WarehouseCode）不能为空")]
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（关联 TaktStorageLocation.LocationCode）
    /// </summary>
    [Required(ErrorMessage = "源库位编码（关联 TaktStorageLocation.LocationCode）不能为空")]
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? TargetWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? TargetLocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联公司
    /// </summary>
    [Required(ErrorMessage = "关联公司不能为空")]
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 交易总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 交易状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    public int TransactionStatus { get; set; } = 0;

    /// <summary>
    /// 过账日期
    /// </summary>
    public DateTime? PostedDate { get; set; }

    /// <summary>
    /// 过账人（人员代码）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

    /// <summary>
    /// 物料交易明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktMaterialTransactionItemCreateDto>? Items { get; set; }

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
// 更新MaterialTransaction DTO
// ========================================

/// <summary>
/// 更新MaterialTransaction DTO
/// 继承 TaktMaterialTransactionCreateDto，添加 MaterialTransactionId 字段
/// </summary>
public class TaktMaterialTransactionUpdateDto : TaktMaterialTransactionCreateDto
{
    /// <summary>
    /// MaterialTransactionID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionId { get; set; }

}

// ========================================
// MaterialTransaction 状态 DTO
// ========================================

/// <summary>
/// MaterialTransaction 状态更新 DTO
/// </summary>
public class TaktMaterialTransactionStatusDto
{
    /// <summary>
    /// MaterialTransactionID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionId { get; set; }

    /// <summary>
    /// 交易状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    [Required(ErrorMessage = "交易状态（0=草稿，1=已过账，2=已作废）不能为空")]
    public int TransactionStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialTransaction 导入模板行 DTO
/// </summary>
public class TaktMaterialTransactionTemplateDto
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
    /// 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料交易单号（租户+公司+工厂内唯一）
    /// </summary>
    public string? MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 交易方向（0=入库，1=出库，2=库内/移库）
    /// </summary>
    public int? TransactionDirection { get; set; }

    /// <summary>
    /// 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
    /// </summary>
    public int? TransactionType { get; set; }

    /// <summary>
    /// 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
    /// </summary>
    public int? BusinessAction { get; set; }

    /// <summary>
    /// 来源单号（采购订单、销售订单、生产订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方编码（供应商、客户或部门等业务编码）
    /// </summary>
    public string? PartnerCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方名称
    /// </summary>
    public string? PartnerName { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? TargetWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? TargetLocationCode { get; set; } = string.Empty;

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
/// MaterialTransaction 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialTransactionImportDto
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
    /// 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料交易单号（租户+公司+工厂内唯一）
    /// </summary>
    public string? MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 交易方向（0=入库，1=出库，2=库内/移库）
    /// </summary>
    public int? TransactionDirection { get; set; }

    /// <summary>
    /// 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
    /// </summary>
    public int? TransactionType { get; set; }

    /// <summary>
    /// 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
    /// </summary>
    public int? BusinessAction { get; set; }

    /// <summary>
    /// 来源单号（采购订单、销售订单、生产订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方编码（供应商、客户或部门等业务编码）
    /// </summary>
    public string? PartnerCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方名称
    /// </summary>
    public string? PartnerName { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? TargetWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? TargetLocationCode { get; set; } = string.Empty;

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
/// MaterialTransaction 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialTransactionExportDto
{
    /// <summary>
    /// MaterialTransactionID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialTransactionId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料交易单号（租户+公司+工厂内唯一）
    /// </summary>
    public string MaterialTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// 交易日期
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// 交易方向（0=入库，1=出库，2=库内/移库）
    /// </summary>
    public int TransactionDirection { get; set; } = 0;

    /// <summary>
    /// 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
    /// </summary>
    public int TransactionType { get; set; } = 0;

    /// <summary>
    /// 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
    /// </summary>
    public int BusinessAction { get; set; } = 0;

    /// <summary>
    /// 来源单号（采购订单、销售订单、生产订单等业务来源编码）
    /// </summary>
    public string? SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方编码（供应商、客户或部门等业务编码）
    /// </summary>
    public string? PartnerCode { get; set; } = string.Empty;

    /// <summary>
    /// 往来方名称
    /// </summary>
    public string? PartnerName { get; set; } = string.Empty;

    /// <summary>
    /// 源仓库编码（关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string WarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 源库位编码（关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
    /// </summary>
    public string? TargetWarehouseCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
    /// </summary>
    public string? TargetLocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 关联公司
    /// </summary>
    public string RelatedCompany { get; set; } = string.Empty;

    /// <summary>
    /// 交易总数量（基本单位数量）
    /// </summary>
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// 交易状态（0=草稿，1=已过账，2=已作废）
    /// </summary>
    public int TransactionStatus { get; set; } = 0;

    /// <summary>
    /// 过账日期
    /// </summary>
    public DateTime? PostedDate { get; set; }

    /// <summary>
    /// 过账人（人员代码）
    /// </summary>
    public string? PostedBy { get; set; } = string.Empty;

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
