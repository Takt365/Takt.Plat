// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecastItemDtos.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：SalesForecastItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSalesForecastItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Mds;

// ========================================
// SalesForecastItem 响应 DTO
// ========================================

/// <summary>
/// Takt销售预测明细实体
/// 对应前端 TaktSalesForecastItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSalesForecastItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SalesForecastItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastItemId { get; set; }

    /// <summary>
    /// 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastId { get; set; }

    /// <summary>
    /// 销售预测名称（填充字段）
    /// </summary>
    public string? SalesForecastName { get; set; }

    /// <summary>
    /// 销售预测编码（冗余字段，便于查询）
    /// </summary>
    public string SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划交货日期
    /// </summary>
    public DateTime? PlannedDeliveryDate { get; set; }

    /// <summary>
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

}

// ========================================
// SalesForecastItem 查询 DTO
// ========================================

/// <summary>
/// SalesForecastItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSalesForecastItemQueryDto : TaktPagedQuery
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
    /// 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 销售预测编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal? PlanQuantity { get; set; }

    /// <summary>
    /// 计划交货日期（范围查询-开始）
    /// </summary>
    public DateTime? PlannedDeliveryDateStart { get; set; }

    /// <summary>
    /// 计划交货日期（范围查询-结束）
    /// </summary>
    public DateTime? PlannedDeliveryDateEnd { get; set; }

    /// <summary>
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal? EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建SalesForecastItem DTO
// ========================================

/// <summary>
/// 创建SalesForecastItem DTO
/// </summary>
public class TaktSalesForecastItemCreateDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastId { get; set; }

    /// <summary>
    /// 销售预测编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "销售预测编码（冗余字段，便于查询）不能为空")]
    public string SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
    /// </summary>
    [Required(ErrorMessage = "物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    [Required(ErrorMessage = "物料名称不能为空")]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [Required(ErrorMessage = "计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）不能为空")]
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划交货日期
    /// </summary>
    public DateTime? PlannedDeliveryDate { get; set; }

    /// <summary>
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新SalesForecastItem DTO
// ========================================

/// <summary>
/// 更新SalesForecastItem DTO
/// 继承 TaktSalesForecastItemCreateDto，添加 SalesForecastItemId 字段
/// </summary>
public class TaktSalesForecastItemUpdateDto : TaktSalesForecastItemCreateDto
{
    /// <summary>
    /// SalesForecastItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastItemId { get; set; }

}

// ========================================
// SalesForecastItem 作废 DTO
// ========================================

/// <summary>
/// SalesForecastItem 作废/撤销作废 DTO
/// </summary>
public class TaktSalesForecastItemObsoleteDto
{
    /// <summary>
    /// SalesForecastItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SalesForecastItem 导入模板行 DTO
/// </summary>
public class TaktSalesForecastItemTemplateDto
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
    /// 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 销售预测编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal? PlanQuantity { get; set; }

    /// <summary>
    /// 计划交货日期
    /// </summary>
    public DateTime? PlannedDeliveryDate { get; set; }

    /// <summary>
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal? EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// SalesForecastItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSalesForecastItemImportDto
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
    /// 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
    /// </summary>
    public string? CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalesForecastId { get; set; }

    /// <summary>
    /// 销售预测编码（冗余字段，便于查询）
    /// </summary>
    public string? SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string? PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal? PlanQuantity { get; set; }

    /// <summary>
    /// 计划交货日期
    /// </summary>
    public DateTime? PlannedDeliveryDate { get; set; }

    /// <summary>
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal? ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal? EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal? EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// SalesForecastItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSalesForecastItemExportDto
{
    /// <summary>
    /// SalesForecastItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SalesForecastId { get; set; }

    /// <summary>
    /// 销售预测编码（冗余字段，便于查询）
    /// </summary>
    public string SalesForecastCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（关联 TaktMaterial.MaterialCode，选项 TaktMaterials/options）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode，与物料机种主数据对齐）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（冗余字段，便于查询展示）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 计划单位（字典 logistics_unit_of_measure_code，DictValue=PC/EA 等；默认 PC）
    /// </summary>
    public string PlanUnit { get; set; } = string.Empty;

    /// <summary>
    /// 计划数量（基本单位数量）
    /// </summary>
    public decimal PlanQuantity { get; set; }

    /// <summary>
    /// 计划交货日期
    /// </summary>
    public DateTime? PlannedDeliveryDate { get; set; }

    /// <summary>
    /// 已转生产/销售数量（基本单位数量）
    /// </summary>
    public decimal ConvertedQuantity { get; set; }

    /// <summary>
    /// 预计单价
    /// </summary>
    public decimal EstimatedUnitPrice { get; set; }

    /// <summary>
    /// 预计金额
    /// </summary>
    public decimal EstimatedAmount { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
