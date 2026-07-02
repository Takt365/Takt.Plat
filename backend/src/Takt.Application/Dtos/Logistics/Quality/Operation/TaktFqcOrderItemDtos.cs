// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderItemDtos.cs
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：FqcOrderItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktFqcOrderItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Operation;

// ========================================
// FqcOrderItem 响应 DTO
// ========================================

/// <summary>
/// FQC出货检验单明细实体
/// 对应前端 TaktFqcOrderItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktFqcOrderItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// FqcOrderItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderItemId { get; set; }

    /// <summary>
    /// FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderId { get; set; }

    /// <summary>
    /// FQC检验单 名称（填充字段）
    /// </summary>
    public string? FqcOrderName { get; set; }

    /// <summary>
    /// FQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public decimal WarehouseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
    /// </summary>
    public string StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
    /// </summary>
    public string SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_method）
    /// </summary>
    public int InspectionMethod { get; set; } = 0;

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int SampleQuantity { get; set; } = 0;

    /// <summary>
    /// 合格数量
    /// </summary>
    public int QualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int UnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string InspectorBy { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime InspectionDate { get; set; }

    /// <summary>
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// FQC检验单（主表）
    /// （主表：TaktFqcOrder）
    /// </summary>
    public TaktFqcOrderDto? Order { get; set; }

    /// <summary>
    /// 不良处理记录列表（主子表关系）
    /// （子表：TaktFqcDefectHandling）
    /// </summary>
    public List<TaktFqcDefectHandlingDto>? DefectHandlings { get; set; }

}

// ========================================
// FqcOrderItem 查询 DTO
// ========================================

/// <summary>
/// FqcOrderItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktFqcOrderItemQueryDto : TaktPagedQuery
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
    /// FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FqcOrderId { get; set; }

    /// <summary>
    /// FQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public decimal? WarehouseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
    /// </summary>
    public string? StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_method）
    /// </summary>
    public int? InspectionMethod { get; set; }

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int? SampleQuantity { get; set; }

    /// <summary>
    /// 合格数量
    /// </summary>
    public int? QualifiedQuantity { get; set; }

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int? UnqualifiedQuantity { get; set; }

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal? InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string? InspectorBy { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期（范围查询-开始）
    /// </summary>
    public DateTime? InspectionDateStart { get; set; }

    /// <summary>
    /// 检验日期（范围查询-结束）
    /// </summary>
    public DateTime? InspectionDateEnd { get; set; }

    /// <summary>
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    public int? JudgeStatus { get; set; }

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
// 创建FqcOrderItem DTO
// ========================================

/// <summary>
/// 创建FqcOrderItem DTO
/// </summary>
public class TaktFqcOrderItemCreateDto
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
    /// FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderId { get; set; }

    /// <summary>
    /// FQC检验单编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "FQC检验单编码（冗余字段，便于查询）不能为空")]
    public string FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    [Required(ErrorMessage = "物料名称不能为空")]
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public decimal WarehouseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
    /// </summary>
    [Required(ErrorMessage = "检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）不能为空")]
    public string StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
    /// </summary>
    [Required(ErrorMessage = "抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）不能为空")]
    public string SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_method）
    /// </summary>
    public int InspectionMethod { get; set; } = 0;

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int SampleQuantity { get; set; } = 0;

    /// <summary>
    /// 合格数量
    /// </summary>
    public int QualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int UnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    [Required(ErrorMessage = "检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）不能为空")]
    public string InspectorBy { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime InspectionDate { get; set; }

    /// <summary>
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

    /// <summary>
    /// 不良处理记录列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktFqcDefectHandlingCreateDto>? DefectHandlings { get; set; }

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
// 更新FqcOrderItem DTO
// ========================================

/// <summary>
/// 更新FqcOrderItem DTO
/// 继承 TaktFqcOrderItemCreateDto，添加 FqcOrderItemId 字段
/// </summary>
public class TaktFqcOrderItemUpdateDto : TaktFqcOrderItemCreateDto
{
    /// <summary>
    /// FqcOrderItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderItemId { get; set; }

}

// ========================================
// FqcOrderItem 状态 DTO
// ========================================

/// <summary>
/// FqcOrderItem 状态更新 DTO
/// </summary>
public class TaktFqcOrderItemStatusDto
{
    /// <summary>
    /// FqcOrderItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderItemId { get; set; }

    /// <summary>
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    [Required(ErrorMessage = "判定状态（字典 logistics_quality_judge_status）不能为空")]
    public int JudgeStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// FqcOrderItem 导入模板行 DTO
/// </summary>
public class TaktFqcOrderItemTemplateDto
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
    /// FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FqcOrderId { get; set; }

    /// <summary>
    /// FQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public decimal? WarehouseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
    /// </summary>
    public string? StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_method）
    /// </summary>
    public int? InspectionMethod { get; set; }

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int? SampleQuantity { get; set; }

    /// <summary>
    /// 合格数量
    /// </summary>
    public int? QualifiedQuantity { get; set; }

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int? UnqualifiedQuantity { get; set; }

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal? InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string? InspectorBy { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// 不良处理记录列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktFqcDefectHandlingCreateDto>? DefectHandlings { get; set; }

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
/// FqcOrderItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktFqcOrderItemImportDto
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
    /// FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FqcOrderId { get; set; }

    /// <summary>
    /// FQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string? FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public decimal? WarehouseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
    /// </summary>
    public string? StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
    /// </summary>
    public string? SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_method）
    /// </summary>
    public int? InspectionMethod { get; set; }

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int? SampleQuantity { get; set; }

    /// <summary>
    /// 合格数量
    /// </summary>
    public int? QualifiedQuantity { get; set; }

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int? UnqualifiedQuantity { get; set; }

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal? InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string? InspectorBy { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime? InspectionDate { get; set; }

    /// <summary>
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    public int? JudgeStatus { get; set; }

    /// <summary>
    /// 不良处理记录列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktFqcDefectHandlingCreateDto>? DefectHandlings { get; set; }

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
/// FqcOrderItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktFqcOrderItemExportDto
{
    /// <summary>
    /// FqcOrderItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// FQC检验单 ID（关联 TaktFqcOrder.Id，选项 TaktFqcOrders/options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FqcOrderId { get; set; }

    /// <summary>
    /// FQC检验单编码（冗余字段，便于查询）
    /// </summary>
    public string FqcOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string? BatchNo { get; set; } = string.Empty;

    /// <summary>
    /// 入库数量
    /// </summary>
    public decimal WarehouseQuantity { get; set; }

    /// <summary>
    /// 检验标准编码（选项 TaktInspectionStandards/options，DictValue=StandardCode）
    /// </summary>
    public string StandardCode { get; set; } = string.Empty;

    /// <summary>
    /// 抽样方案编码（选项 TaktSamplingSchemes/options，DictValue=SamplingSchemeCode）
    /// </summary>
    public string SamplingSchemeCode { get; set; } = string.Empty;

    /// <summary>
    /// 检验方式（字典 logistics_quality_inspection_method）
    /// </summary>
    public int InspectionMethod { get; set; } = 0;

    /// <summary>
    /// 抽样数量
    /// </summary>
    public int SampleQuantity { get; set; } = 0;

    /// <summary>
    /// 合格数量
    /// </summary>
    public int QualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 不合格数量
    /// </summary>
    public int UnqualifiedQuantity { get; set; } = 0;

    /// <summary>
    /// 验退数量
    /// </summary>
    public decimal InspectionReturnQuantity { get; set; }

    /// <summary>
    /// 抽检序列号
    /// </summary>
    public string? SampleSerialNo { get; set; } = string.Empty;

    /// <summary>
    /// 检验说明
    /// </summary>
    public string? InspectionDescription { get; set; } = string.Empty;

    /// <summary>
    /// 检验员（选项 TaktEmployees/options，DictValue=EmployeeCode）
    /// </summary>
    public string InspectorBy { get; set; } = string.Empty;

    /// <summary>
    /// 检验日期
    /// </summary>
    public DateTime InspectionDate { get; set; }

    /// <summary>
    /// 判定状态（字典 logistics_quality_judge_status）
    /// </summary>
    public int JudgeStatus { get; set; } = 0;

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
