// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityIncidentItemDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：QualityIncidentItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktQualityIncidentItem 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Quality.Cost;

// ========================================
// QualityIncidentItem 响应 DTO
// ========================================

/// <summary>
/// 品质事故明细 - 废弃零件明细行
/// 对应前端 TaktQualityIncidentItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktQualityIncidentItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// QualityIncidentItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIncidentItemId { get; set; }

    /// <summary>
    /// 品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIncidentId { get; set; }

    /// <summary>
    /// 品质事故主表 名称（填充字段）
    /// </summary>
    public string? QualityIncidentName { get; set; }

    /// <summary>
    /// 品质事故编码（冗余字段，便于查询）
    /// </summary>
    public string QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 废弃费用(元)
    /// </summary>
    public decimal ScrapCost { get; set; }

    /// <summary>
    /// 废弃数量
    /// </summary>
    public decimal ScrapSize { get; set; }

    /// <summary>
    /// 零件单价(元)
    /// </summary>
    public decimal PartPrice { get; set; }

    /// <summary>
    /// 废弃处理费用(元)
    /// </summary>
    public decimal ScrapReasonCost { get; set; }

    /// <summary>
    /// 运费(元)
    /// </summary>
    public decimal FreightCharges { get; set; }

    /// <summary>
    /// 其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 处理作业时间(分钟)
    /// </summary>
    public int ReasonWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 关税(元)
    /// </summary>
    public decimal Tax { get; set; }

    /// <summary>
    /// 处理发生其他费用(元)
    /// </summary>
    public decimal ReasonOtherExpenses { get; set; }

    /// <summary>
    /// 废弃备注
    /// </summary>
    public string? ScrapNote { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 品质事故主表(导航属性)
    /// （主表：TaktQualityIncident）
    /// </summary>
    public TaktQualityIncidentDto? Incident { get; set; }

}

// ========================================
// QualityIncidentItem 查询 DTO
// ========================================

/// <summary>
/// QualityIncidentItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktQualityIncidentItemQueryDto : TaktPagedQuery
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
    /// 区域文化编码（字典 sys_culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityIncidentId { get; set; }

    /// <summary>
    /// 品质事故编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 废弃费用(元)
    /// </summary>
    public decimal? ScrapCost { get; set; }

    /// <summary>
    /// 废弃数量
    /// </summary>
    public decimal? ScrapSize { get; set; }

    /// <summary>
    /// 零件单价(元)
    /// </summary>
    public decimal? PartPrice { get; set; }

    /// <summary>
    /// 废弃处理费用(元)
    /// </summary>
    public decimal? ScrapReasonCost { get; set; }

    /// <summary>
    /// 运费(元)
    /// </summary>
    public decimal? FreightCharges { get; set; }

    /// <summary>
    /// 其他费用(元)
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 处理作业时间(分钟)
    /// </summary>
    public int? ReasonWorkTimeMinutes { get; set; }

    /// <summary>
    /// 关税(元)
    /// </summary>
    public decimal? Tax { get; set; }

    /// <summary>
    /// 处理发生其他费用(元)
    /// </summary>
    public decimal? ReasonOtherExpenses { get; set; }

    /// <summary>
    /// 废弃备注
    /// </summary>
    public string? ScrapNote { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
// 创建QualityIncidentItem DTO
// ========================================

/// <summary>
/// 创建QualityIncidentItem DTO
/// </summary>
public class TaktQualityIncidentItemCreateDto
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIncidentId { get; set; }

    /// <summary>
    /// 品质事故编码（冗余字段，便于查询）
    /// </summary>
    [Required(ErrorMessage = "品质事故编码（冗余字段，便于查询）不能为空")]
    public string QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    [Required(ErrorMessage = "物料描述（回填：随物料）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 废弃费用(元)
    /// </summary>
    public decimal ScrapCost { get; set; }

    /// <summary>
    /// 废弃数量
    /// </summary>
    public decimal ScrapSize { get; set; }

    /// <summary>
    /// 零件单价(元)
    /// </summary>
    public decimal PartPrice { get; set; }

    /// <summary>
    /// 废弃处理费用(元)
    /// </summary>
    public decimal ScrapReasonCost { get; set; }

    /// <summary>
    /// 运费(元)
    /// </summary>
    public decimal FreightCharges { get; set; }

    /// <summary>
    /// 其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 处理作业时间(分钟)
    /// </summary>
    public int ReasonWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 关税(元)
    /// </summary>
    public decimal Tax { get; set; }

    /// <summary>
    /// 处理发生其他费用(元)
    /// </summary>
    public decimal ReasonOtherExpenses { get; set; }

    /// <summary>
    /// 废弃备注
    /// </summary>
    public string? ScrapNote { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
    /// QualityIncidentItemId
    /// </summary>
    public long QualityIncidentItemId { get; set; }
}

// ========================================
// 更新QualityIncidentItem DTO
// ========================================

/// <summary>
/// 更新QualityIncidentItem DTO
/// 继承 TaktQualityIncidentItemCreateDto，添加 QualityIncidentItemId 字段
/// </summary>
public class TaktQualityIncidentItemUpdateDto : TaktQualityIncidentItemCreateDto
{
    /// <summary>
    /// QualityIncidentItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public new long QualityIncidentItemId { get; set; }

}

// ========================================
// QualityIncidentItem 作废 DTO
// ========================================

/// <summary>
/// QualityIncidentItem 作废/撤销作废 DTO
/// </summary>
public class TaktQualityIncidentItemObsoleteDto
{
    /// <summary>
    /// QualityIncidentItemID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIncidentItemId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// QualityIncidentItem 导入模板行 DTO
/// </summary>
public class TaktQualityIncidentItemTemplateDto
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityIncidentId { get; set; }

    /// <summary>
    /// 品质事故编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 废弃费用(元)
    /// </summary>
    public decimal? ScrapCost { get; set; }

    /// <summary>
    /// 废弃数量
    /// </summary>
    public decimal? ScrapSize { get; set; }

    /// <summary>
    /// 零件单价(元)
    /// </summary>
    public decimal? PartPrice { get; set; }

    /// <summary>
    /// 废弃处理费用(元)
    /// </summary>
    public decimal? ScrapReasonCost { get; set; }

    /// <summary>
    /// 运费(元)
    /// </summary>
    public decimal? FreightCharges { get; set; }

    /// <summary>
    /// 其他费用(元)
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 处理作业时间(分钟)
    /// </summary>
    public int? ReasonWorkTimeMinutes { get; set; }

    /// <summary>
    /// 关税(元)
    /// </summary>
    public decimal? Tax { get; set; }

    /// <summary>
    /// 处理发生其他费用(元)
    /// </summary>
    public decimal? ReasonOtherExpenses { get; set; }

    /// <summary>
    /// 废弃备注
    /// </summary>
    public string? ScrapNote { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
/// QualityIncidentItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktQualityIncidentItemImportDto
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
    /// 区域文化编码（登录或公司切换注入，对应实体基类 CultureCode / 公司 culture_code）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? QualityIncidentId { get; set; }

    /// <summary>
    /// 品质事故编码（冗余字段，便于查询）
    /// </summary>
    public string? QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 废弃费用(元)
    /// </summary>
    public decimal? ScrapCost { get; set; }

    /// <summary>
    /// 废弃数量
    /// </summary>
    public decimal? ScrapSize { get; set; }

    /// <summary>
    /// 零件单价(元)
    /// </summary>
    public decimal? PartPrice { get; set; }

    /// <summary>
    /// 废弃处理费用(元)
    /// </summary>
    public decimal? ScrapReasonCost { get; set; }

    /// <summary>
    /// 运费(元)
    /// </summary>
    public decimal? FreightCharges { get; set; }

    /// <summary>
    /// 其他费用(元)
    /// </summary>
    public decimal? OtherExpenses { get; set; }

    /// <summary>
    /// 处理作业时间(分钟)
    /// </summary>
    public int? ReasonWorkTimeMinutes { get; set; }

    /// <summary>
    /// 关税(元)
    /// </summary>
    public decimal? Tax { get; set; }

    /// <summary>
    /// 处理发生其他费用(元)
    /// </summary>
    public decimal? ReasonOtherExpenses { get; set; }

    /// <summary>
    /// 废弃备注
    /// </summary>
    public string? ScrapNote { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
/// QualityIncidentItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktQualityIncidentItemExportDto
{
    /// <summary>
    /// QualityIncidentItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIncidentItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIncidentId { get; set; }

    /// <summary>
    /// 品质事故编码（冗余字段，便于查询）
    /// </summary>
    public string QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 废弃费用(元)
    /// </summary>
    public decimal ScrapCost { get; set; }

    /// <summary>
    /// 废弃数量
    /// </summary>
    public decimal ScrapSize { get; set; }

    /// <summary>
    /// 零件单价(元)
    /// </summary>
    public decimal PartPrice { get; set; }

    /// <summary>
    /// 废弃处理费用(元)
    /// </summary>
    public decimal ScrapReasonCost { get; set; }

    /// <summary>
    /// 运费(元)
    /// </summary>
    public decimal FreightCharges { get; set; }

    /// <summary>
    /// 其他费用(元)
    /// </summary>
    public decimal OtherExpenses { get; set; }

    /// <summary>
    /// 处理作业时间(分钟)
    /// </summary>
    public int ReasonWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 关税(元)
    /// </summary>
    public decimal Tax { get; set; }

    /// <summary>
    /// 处理发生其他费用(元)
    /// </summary>
    public decimal ReasonOtherExpenses { get; set; }

    /// <summary>
    /// 废弃备注
    /// </summary>
    public string? ScrapNote { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
