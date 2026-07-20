// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostDtos.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Auto Generated)
// 功能描述：BomMaterialCost 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBomMaterialCost 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Bom;

// ========================================
// BomMaterialCost 响应 DTO
// ========================================

/// <summary>
/// BOM 物料成本汇总表（由明细按工厂+机种+产品+核算月聚合写入；UI 机种组→产品行→Item，实体不拆分，无对明细导航）
/// 对应前端 TaktBomMaterialCostDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBomMaterialCostDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BomMaterialCostID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostId { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
    /// </summary>
    public decimal ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
    /// </summary>
    public decimal ProductMonthlyCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）
    /// </summary>
    public DateTime CostingDate { get; set; }

}

// ========================================
// BomMaterialCost 查询 DTO
// ========================================

/// <summary>
/// BomMaterialCost 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBomMaterialCostQueryDto : TaktPagedQuery
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
    /// </summary>
    public decimal? ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
    /// </summary>
    public decimal? ProductMonthlyCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    public string? CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）（范围查询-开始）
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）（范围查询-结束）
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }

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
// 创建BomMaterialCost DTO
// ========================================

/// <summary>
/// 创建BomMaterialCost DTO
/// </summary>
public class TaktBomMaterialCostCreateDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "工厂代码（选项 TaktPlants/options，DictValue=PlantCode）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode）
    /// </summary>
    [Required(ErrorMessage = "机种编码（关联 TaktModelDestination.ModelCode）不能为空")]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
    /// </summary>
    public decimal ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    [Required(ErrorMessage = "产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位不能为空")]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    [Required(ErrorMessage = "产品描述不能为空")]
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
    /// </summary>
    public decimal ProductMonthlyCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    [Required(ErrorMessage = "币种（字典 accounting_currency_code，如 CNY/USD）不能为空")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    [Required(ErrorMessage = "核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）不能为空")]
    public string CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）
    /// </summary>
    public DateTime CostingDate { get; set; }

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
// 更新BomMaterialCost DTO
// ========================================

/// <summary>
/// 更新BomMaterialCost DTO
/// 继承 TaktBomMaterialCostCreateDto，添加 BomMaterialCostId 字段
/// </summary>
public class TaktBomMaterialCostUpdateDto : TaktBomMaterialCostCreateDto
{
    /// <summary>
    /// BomMaterialCostID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// BomMaterialCost 导入模板行 DTO
/// </summary>
public class TaktBomMaterialCostTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
    /// </summary>
    public decimal? ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
    /// </summary>
    public decimal? ProductMonthlyCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    public string? CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）
    /// </summary>
    public DateTime? CostingDate { get; set; }

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
/// BomMaterialCost 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBomMaterialCostImportDto
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
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
    /// </summary>
    public decimal? ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
    /// </summary>
    public decimal? ProductMonthlyCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string? CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    public string? CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）
    /// </summary>
    public DateTime? CostingDate { get; set; }

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
/// BomMaterialCost 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBomMaterialCostExportDto
{
    /// <summary>
    /// BomMaterialCostID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（关联 TaktModelDestination.ModelCode）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
    /// </summary>
    public decimal ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
    /// </summary>
    public decimal ProductMonthlyCost { get; set; }

    /// <summary>
    /// 币种（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）
    /// </summary>
    public DateTime CostingDate { get; set; }

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

/// <summary>
/// BOM 物料成本机种维度展示行（同一物理表按工厂+机种+核算期间去重聚合；非独立实体）
/// </summary>
public class TaktBomMaterialCostModelGroupDto
{
    /// <summary>
    /// 分组键（plant|model|period，供前端 row-key）
    /// </summary>
    public string GroupKey { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种月平均材料成本
    /// </summary>
    public decimal ModelMonthlyAverageCost { get; set; }

    /// <summary>
    /// 币种
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间（yyyy-MM）
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（组内代表行，通常为同月最后核算日）
    /// </summary>
    public DateTime CostingDate { get; set; }

    /// <summary>
    /// 组内产品行数
    /// </summary>
    public int ProductRowCount { get; set; }
}
