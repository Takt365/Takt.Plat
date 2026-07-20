// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemDtos.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Auto Generated)
// 功能描述：BomMaterialCostItem 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktBomMaterialCostItem 生成，请按需审阅）
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
// BomMaterialCostItem 响应 DTO
// ========================================

/// <summary>
/// BOM 物料成本明细行（业务源数据：先导入/维护明细，再按工厂+产品+核算月聚合写入 TaktBomMaterialCost；无线上主表外键）
/// 对应前端 TaktBomMaterialCostItemDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktBomMaterialCostItemDto : TaktCompanyDtoBase
{
    /// <summary>
    /// BomMaterialCostItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostItemId { get; set; }

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（展开行序号，如 0010）
    /// </summary>
    public string SequenceNo { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    public string BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    public string BomItemNo { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
    /// </summary>
    public string ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价
    /// </summary>
    public decimal MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int MovingPriceUnit { get; set; } = 0;

    /// <summary>
    /// 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string MovingPriceCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    public string PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
    /// </summary>
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格）
    /// </summary>
    public decimal NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int PurchasePriceUnit { get; set; } = 0;

    /// <summary>
    /// 采购货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string PurchaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
    /// </summary>
    public DateTime CostingDate { get; set; }

}

// ========================================
// BomMaterialCostItem 查询 DTO
// ========================================

/// <summary>
/// BomMaterialCostItem 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktBomMaterialCostItemQueryDto : TaktPagedQuery
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
    /// 机种编码（成本合计按主表过滤用；明细行本身无此列）
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 产品编码集合（机种展开后内部过滤；有值时 ProductCode 精确匹配集合）
    /// </summary>
    public List<string>? ProductCodes { get; set; }

    /// <summary>
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（展开行序号，如 0010）
    /// </summary>
    public string? SequenceNo { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    public string? BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    public string? BomItemNo { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string? ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal? ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    public string? PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价
    /// </summary>
    public decimal? MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int? MovingPriceUnit { get; set; }

    /// <summary>
    /// 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string? MovingPriceCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    public string? PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格）
    /// </summary>
    public decimal? NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int? PurchasePriceUnit { get; set; }

    /// <summary>
    /// 采购货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string? PurchaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期（范围查询-开始）
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期（范围查询-结束）
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
// 创建BomMaterialCostItem DTO
// ========================================

/// <summary>
/// 创建BomMaterialCostItem DTO
/// </summary>
public class TaktBomMaterialCostItemCreateDto
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
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    [Required(ErrorMessage = "产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位不能为空")]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（展开行序号，如 0010）
    /// </summary>
    [Required(ErrorMessage = "序号（展开行序号，如 0010）不能为空")]
    public string SequenceNo { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    [Required(ErrorMessage = "产品描述不能为空")]
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    [Required(ErrorMessage = "层级（BOM 展开层级，如 01/02）不能为空")]
    public string BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    [Required(ErrorMessage = "BOM 项目号（子件行项目号，如 0010）不能为空")]
    public string BomItemNo { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    [Required(ErrorMessage = "组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位不能为空")]
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    [Required(ErrorMessage = "组件描述不能为空")]
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    [Required(ErrorMessage = "采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数不能为空")]
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
    /// </summary>
    [Required(ErrorMessage = "利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）不能为空")]
    public string ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价
    /// </summary>
    public decimal MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int MovingPriceUnit { get; set; } = 0;

    /// <summary>
    /// 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    [Required(ErrorMessage = "移动价格货币（字典 accounting_currency_code，如 CNY/USD）不能为空")]
    public string MovingPriceCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    [Required(ErrorMessage = "采购组织不能为空")]
    public string PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
    /// </summary>
    [Required(ErrorMessage = "采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）不能为空")]
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    [Required(ErrorMessage = "供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）不能为空")]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格）
    /// </summary>
    public decimal NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int PurchasePriceUnit { get; set; } = 0;

    /// <summary>
    /// 采购货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    [Required(ErrorMessage = "采购货币（字典 accounting_currency_code，如 CNY/USD）不能为空")]
    public string PurchaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
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
// 更新BomMaterialCostItem DTO
// ========================================

/// <summary>
/// 更新BomMaterialCostItem DTO
/// 继承 TaktBomMaterialCostItemCreateDto，添加 BomMaterialCostItemId 字段
/// </summary>
public class TaktBomMaterialCostItemUpdateDto : TaktBomMaterialCostItemCreateDto
{
    /// <summary>
    /// BomMaterialCostItemID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostItemId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// BomMaterialCostItem 导入模板行 DTO
/// </summary>
public class TaktBomMaterialCostItemTemplateDto
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
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（展开行序号，如 0010）
    /// </summary>
    public string? SequenceNo { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    public string? BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    public string? BomItemNo { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string? ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal? ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    public string? PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价
    /// </summary>
    public decimal? MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int? MovingPriceUnit { get; set; }

    /// <summary>
    /// 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string? MovingPriceCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    public string? PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格）
    /// </summary>
    public decimal? NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int? PurchasePriceUnit { get; set; }

    /// <summary>
    /// 采购货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string? PurchaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
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
/// BomMaterialCostItem 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktBomMaterialCostItemImportDto
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
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（展开行序号，如 0010）
    /// </summary>
    public string? SequenceNo { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string? ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    public string? BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    public string? BomItemNo { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string? ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string? ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal? ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    public string? PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
    /// </summary>
    public string? ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价
    /// </summary>
    public decimal? MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int? MovingPriceUnit { get; set; }

    /// <summary>
    /// 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string? MovingPriceCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    public string? PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
    /// </summary>
    public string? PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格）
    /// </summary>
    public decimal? NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int? PurchasePriceUnit { get; set; }

    /// <summary>
    /// 采购货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string? PurchaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
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
/// BomMaterialCostItem 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktBomMaterialCostItemExportDto
{
    /// <summary>
    /// BomMaterialCostItemID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BomMaterialCostItemId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 序号（展开行序号，如 0010）
    /// </summary>
    public string SequenceNo { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 层级（BOM 展开层级，如 01/02）
    /// </summary>
    public string BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（子件行项目号，如 0010）
    /// </summary>
    public string BomItemNo { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量
    /// </summary>
    public decimal ComponentQuantity { get; set; }

    /// <summary>
    /// 批量标识（空或 X）
    /// </summary>
    public string? BatchIndicator { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关（空或 X）
    /// </summary>
    public string? ProductionRelated { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 特殊采购类（空或业务码，最长 50）
    /// </summary>
    public string? SpecialProcurementType { get; set; } = string.Empty;

    /// <summary>
    /// 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
    /// </summary>
    public string ProfitCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 移动平均价
    /// </summary>
    public decimal MovingAveragePrice { get; set; }

    /// <summary>
    /// 移动价格单位
    /// </summary>
    public int MovingPriceUnit { get; set; } = 0;

    /// <summary>
    /// 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string MovingPriceCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 采购组织
    /// </summary>
    public string PurchaseOrganization { get; set; } = string.Empty;

    /// <summary>
    /// 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
    /// </summary>
    public string PurchaseGroup { get; set; } = string.Empty;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
    /// </summary>
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 净价（采购价格）
    /// </summary>
    public decimal NetPurchasePrice { get; set; }

    /// <summary>
    /// 采购价格单位
    /// </summary>
    public int PurchasePriceUnit { get; set; } = 0;

    /// <summary>
    /// 采购货币（字典 accounting_currency_code，如 CNY/USD）
    /// </summary>
    public string PurchaseCurrency { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期
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
/// 机种月平均重算：规范化后的查询与月份标签
/// </summary>
public class TaktBomMaterialCostItemRecalculatePreparedQueryDto
{
    /// <summary>
    /// 规范化后的明细查询（单核算月日期范围）
    /// </summary>
    public TaktBomMaterialCostItemQueryDto Query { get; set; } = new();

    /// <summary>
    /// 核算月份（yyyy-MM）
    /// </summary>
    public string ProcessedMonth { get; set; } = string.Empty;
}

/// <summary>
/// 机种月平均重算任务已提交回执
/// </summary>
public class TaktBomMaterialCostItemRecalculateSubmittedDto
{
    /// <summary>
    /// 核算月份（yyyy-MM）
    /// </summary>
    public string ProcessedMonth { get; set; } = string.Empty;

    /// <summary>
    /// 是否强制重算（重置）
    /// </summary>
    public bool ForceRecalculate { get; set; }

    /// <summary>
    /// 处理记录数上限（按工厂+产品组；0=全部；默认 5000）
    /// </summary>
    public int ProcessRecordCount { get; set; }
}

/// <summary>
/// 机种月平均重算执行结果（同步或后台完成后）
/// </summary>
public class TaktBomMaterialCostItemRecalculateModelAverageResultDto
{
    /// <summary>
    /// 扫描明细行数
    /// </summary>
    public int ScannedRowCount { get; set; }

    /// <summary>
    /// 刷新汇总组数
    /// </summary>
    public int RefreshedGroupCount { get; set; }

    /// <summary>
    /// 跳过组数
    /// </summary>
    public int SkippedGroupCount { get; set; }

    /// <summary>
    /// 强制重置涉及组数
    /// </summary>
    public int ResetGroupCount { get; set; }

    /// <summary>
    /// 处理月份数
    /// </summary>
    public int ProcessedMonthCount { get; set; }

    /// <summary>
    /// 核算月份（yyyy-MM）
    /// </summary>
    public string ProcessedMonth { get; set; } = string.Empty;
}

// ========================================
// BOM 物料成本明细转置/差异/涨跌分析 DTO
// ========================================

/// <summary>
/// 成本转置查询（行=产品，列=月份期间）
/// </summary>
public class TaktBomMaterialCostItemTransposedQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string? PlantCode { get; set; }

    /// <summary>
    /// 机种编码
    /// </summary>
    public string? ModelCode { get; set; }

    /// <summary>
    /// 产品编码
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 核算日期起
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期止
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }

    /// <summary>
    /// 关注期间（yyyy-MM，可选）；设置后按该月相对上月计算各行环比涨跌
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 成本转置行（产品各月总成本）
/// </summary>
public class TaktBomMaterialCostItemTransposedDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（取自主表 TaktBomMaterialCost）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 各期间总成本（键 yyyy-MM）
    /// </summary>
    public Dictionary<string, decimal> PeriodCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 币种（取该产品最新核算行）
    /// </summary>
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 环比涨跌：none / up / down / flat（FocusPeriod 设置且存在上月成本时有效）
    /// </summary>
    public string Trend { get; set; } = "none";

    /// <summary>
    /// 环比基准期间（yyyy-MM）
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间（yyyy-MM，通常为 FocusPeriod）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 环比差额（对比月 - 基准月）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 机种材料成本汇总（转置页未选单物料时展示）
/// </summary>
public class TaktBomMaterialCostItemModelSummaryDto
{
    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 机种下成品数量
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// 各月平均材料成本（键 yyyy-MM）
    /// </summary>
    public Dictionary<string, decimal> AveragePeriodCosts { get; set; } = new(StringComparer.Ordinal);
}

/// <summary>
/// 成本转置分页结果（含动态列顺序）
/// </summary>
public class TaktBomMaterialCostItemTransposedResultDto
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public TaktPagedResult<TaktBomMaterialCostItemTransposedDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序（yyyy-MM）
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 机种汇总（未选产品时：全量成品平均成本）
    /// </summary>
    public TaktBomMaterialCostItemModelSummaryDto? ModelSummary { get; set; }

    /// <summary>
    /// 全量行各期间成本合计（分页前、已应用涨跌筛选）
    /// </summary>
    public Dictionary<string, decimal> PeriodCostTotals { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 全量行环比差额合计（分页前、已应用涨跌筛选）
    /// </summary>
    public decimal? VarianceAmountTotal { get; set; }
}

/// <summary>
/// 成本差异分析查询（两期间组件级对比）
/// </summary>
public class TaktBomMaterialCostItemVarianceQueryDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    [Required]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 基准期间（yyyy-MM）
    /// </summary>
    [Required]
    public string BasePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 对比期间（yyyy-MM）
    /// </summary>
    [Required]
    public string ComparePeriod { get; set; } = string.Empty;
}

/// <summary>
/// 成本差异分析行（组件级）
/// </summary>
public class TaktBomMaterialCostItemVarianceLineDto
{
    /// <summary>
    /// BOM 项目号
    /// </summary>
    public string BomItemNo { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 采购类型（F/E）
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 货币
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// 基准行成本
    /// </summary>
    public decimal BaseCost { get; set; }

    /// <summary>
    /// 对比行成本
    /// </summary>
    public decimal CompareCost { get; set; }

    /// <summary>
    /// 成本差异（对比 - 基准）
    /// </summary>
    public decimal VarianceAmount { get; set; }

    /// <summary>
    /// 成本差异率（%）
    /// </summary>
    public decimal? VariancePercent { get; set; }

    /// <summary>
    /// 基准单价
    /// </summary>
    public decimal BaseUnitPrice { get; set; }

    /// <summary>
    /// 对比单价
    /// </summary>
    public decimal CompareUnitPrice { get; set; }

    /// <summary>
    /// 单价差异
    /// </summary>
    public decimal UnitPriceVariance { get; set; }

    /// <summary>
    /// 基准数量
    /// </summary>
    public decimal BaseQuantity { get; set; }

    /// <summary>
    /// 对比数量
    /// </summary>
    public decimal CompareQuantity { get; set; }

    /// <summary>
    /// 数量差异
    /// </summary>
    public decimal QuantityVariance { get; set; }

    /// <summary>
    /// 价格因素影响额
    /// </summary>
    public decimal PriceEffectAmount { get; set; }

    /// <summary>
    /// 数量因素影响额
    /// </summary>
    public decimal QuantityEffectAmount { get; set; }

    /// <summary>
    /// 变动类型：new / removed / price / quantity / mixed / unchanged
    /// </summary>
    public string ChangeType { get; set; } = string.Empty;
}

/// <summary>
/// 成本差异分析结果
/// </summary>
public class TaktBomMaterialCostItemVarianceResultDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 基准期间
    /// </summary>
    public string BasePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 对比期间
    /// </summary>
    public string ComparePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 基准总成本
    /// </summary>
    public decimal BaseTotalCost { get; set; }

    /// <summary>
    /// 对比总成本
    /// </summary>
    public decimal CompareTotalCost { get; set; }

    /// <summary>
    /// 总成本差异
    /// </summary>
    public decimal TotalVariance { get; set; }

    /// <summary>
    /// 组件差异明细
    /// </summary>
    public List<TaktBomMaterialCostItemVarianceLineDto> Lines { get; set; } = new();
}

/// <summary>
/// 成本月度涨跌分析查询
/// </summary>
public class TaktBomMaterialCostItemMonthlyTrendQueryDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    [Required]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    [Required]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（选定单物料时必填；为空表示机种下全部物料）
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 起始年月 yyyy-MM
    /// </summary>
    public string? PeriodStart { get; set; }

    /// <summary>
    /// 结束年月 yyyy-MM
    /// </summary>
    public string? PeriodEnd { get; set; }
}

/// <summary>
/// 成本月度涨跌分析行
/// </summary>
public class TaktBomMaterialCostItemMonthlyTrendLineDto
{
    /// <summary>
    /// 期间 yyyy-MM
    /// </summary>
    public string Period { get; set; } = string.Empty;

    /// <summary>
    /// 材料总成本
    /// </summary>
    public decimal TotalCost { get; set; }

    /// <summary>
    /// 对比基准月
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 基准月成本
    /// </summary>
    public decimal? BaseTotalCost { get; set; }

    /// <summary>
    /// 环比差额
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? VariancePercent { get; set; }

    /// <summary>
    /// 涨跌：none / up / down / flat
    /// </summary>
    public string Trend { get; set; } = string.Empty;
}

/// <summary>
/// 成本月度涨跌分析结果
/// </summary>
public class TaktBomMaterialCostItemMonthlyTrendResultDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 是否为机种下全部物料汇总
    /// </summary>
    public bool AllMaterialsUnderModel { get; set; }

    /// <summary>
    /// 月度涨跌行
    /// </summary>
    public List<TaktBomMaterialCostItemMonthlyTrendLineDto> Lines { get; set; } = new();
}

// ========================================
// 机种合并组件 × 移动价格期间转置分析 DTO
// ========================================

/// <summary>
/// BOM 成本推移：按单个产品汇总月材料成本转置查询（不按机种合并组件）
/// </summary>
public class TaktBomMaterialCostItemComponentMovingPriceQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（可选；仅缩小产品范围）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（必填；仅分析该单个产品）
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算期间起（CostingDate 月初语义；传任意日由服务归一到月初）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 核算期间止（CostingDate 月初语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；按产品月材料成本环比
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 评估类别（明细表无此字段；保留兼容，服务忽略）
    /// </summary>
    public string? Valuation { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// BOM 成本推移明细行：单个产品下组件（明细表）各核算月材料成本转置；缺月不回填
/// </summary>
public class TaktBomMaterialCostItemComponentMovingPriceDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品描述
    /// </summary>
    public string ProductDescription { get; set; } = string.Empty;

    /// <summary>
    /// 序号（明细表）
    /// </summary>
    public string SequenceNo { get; set; } = string.Empty;

    /// <summary>
    /// BOM 层级（明细表）
    /// </summary>
    public string BomLevel { get; set; } = string.Empty;

    /// <summary>
    /// BOM 项目号（明细表）
    /// </summary>
    public string BomItemNo { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码（明细表）
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述（明细表）
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 组件数量（明细表）
    /// </summary>
    public decimal ComponentQuantity { get; set; }

    /// <summary>
    /// 生产相关（明细表）
    /// </summary>
    public string? ProductionRelated { get; set; }

    /// <summary>
    /// 采购类型（明细表）
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 币种
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// 各核算月材料成本（键 yyyy-MM；按明细行键对齐后汇总 CalculateLineCost；缺月无键）
    /// </summary>
    public Dictionary<string, decimal> PeriodMaterialCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 兼容旧字段：同 PeriodMaterialCosts
    /// </summary>
    public Dictionary<string, decimal> PeriodUnitPrices
    {
        get => PeriodMaterialCosts;
        set => PeriodMaterialCosts = value ?? new Dictionary<string, decimal>(StringComparer.Ordinal);
    }

    /// <summary>
    /// 环比涨跌：none / up / down / flat
    /// </summary>
    public string Trend { get; set; } = "none";

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 环比差额（对比月材料成本 - 基准月材料成本）
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// BOM 成本推移（单个产品明细组件×月材料成本）分析结果
/// </summary>
public class TaktBomMaterialCostItemComponentMovingPriceResultDto
{
    /// <summary>
    /// 分页明细组件行
    /// </summary>
    public TaktPagedResult<TaktBomMaterialCostItemComponentMovingPriceDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 产品编码列表（通常仅 1 个）
    /// </summary>
    public List<string> ProductCodes { get; set; } = new();

    /// <summary>
    /// 明细组件行总数（分页前，已应用涨跌筛选）
    /// </summary>
    public int ComponentCount { get; set; }

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间（关注月）
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨价产品数（筛选前全量统计）
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌价产品数（筛选前全量统计）
    /// </summary>
    public int DownCount { get; set; }

    /// <summary>
    /// 持平产品数（筛选前全量统计）
    /// </summary>
    public int FlatCount { get; set; }

    /// <summary>
    /// 无法比较产品数（筛选前全量统计）
    /// </summary>
    public int NoneCount { get; set; }

    /// <summary>
    /// 全量行各期间材料成本合计（分页前、已应用涨跌筛选）
    /// </summary>
    public Dictionary<string, decimal> PeriodCostTotals { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 全量行环比差额合计（分页前、已应用涨跌筛选）
    /// </summary>
    public decimal? VarianceAmountTotal { get; set; }
}

/// <summary>
/// 机种成本推移查询
/// </summary>
public class TaktBomMaterialCostItemModelMovingPriceQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（可选；空=工厂期间全量产品；空且提供 ProductCode 时按产品反查机种）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（可选；有值时仅汇总该产品下明细，否则机种或工厂下全部产品）
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 核算期间起（CostingDate 月初语义）
    /// </summary>
    public DateTime? PeriodDateStart { get; set; }

    /// <summary>
    /// 核算期间止（CostingDate 月初语义）
    /// </summary>
    public DateTime? PeriodDateEnd { get; set; }

    /// <summary>
    /// 关注期间 yyyy-MM（可选）；环比对比月
    /// </summary>
    public string? FocusPeriod { get; set; }

    /// <summary>
    /// 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
    /// </summary>
    public string? TrendFilter { get; set; }
}

/// <summary>
/// 机种成本推移分析行（键=Plant+ComponentCode+ProductionRelated+PurchaseType，跨产品组合并；列为月材料成本）
/// </summary>
public class TaktBomMaterialCostItemModelMovingPriceDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 生产相关
    /// </summary>
    public string? ProductionRelated { get; set; }

    /// <summary>
    /// 采购类型
    /// </summary>
    public string PurchaseType { get; set; } = string.Empty;

    /// <summary>
    /// 产品组（机种下共用该键的产品编码，逗号分隔）
    /// </summary>
    public string ProductCodes { get; set; } = string.Empty;

    /// <summary>
    /// 产品组内产品数
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// 币种
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// 各核算月材料成本（键 yyyy-MM；同键行成本求和；缺月无键）
    /// </summary>
    public Dictionary<string, decimal> PeriodMaterialCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 兼容旧字段：同 PeriodMaterialCosts
    /// </summary>
    public Dictionary<string, decimal> PeriodUnitPrices
    {
        get => PeriodMaterialCosts;
        set => PeriodMaterialCosts = value ?? new Dictionary<string, decimal>(StringComparer.Ordinal);
    }

    /// <summary>
    /// 环比涨跌：none / up / down / flat
    /// </summary>
    public string Trend { get; set; } = "none";

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 环比差额
    /// </summary>
    public decimal? VarianceAmount { get; set; }

    /// <summary>
    /// 环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? VariancePercent { get; set; }
}

/// <summary>
/// 机种成本推移分析结果
/// </summary>
public class TaktBomMaterialCostItemModelMovingPriceResultDto
{
    /// <summary>
    /// 分页分析行（组件合并键）
    /// </summary>
    public TaktPagedResult<TaktBomMaterialCostItemModelMovingPriceDto> Paged { get; set; } = null!;

    /// <summary>
    /// 期间列顺序 yyyy-MM
    /// </summary>
    public List<string> PeriodOrder { get; set; } = new();

    /// <summary>
    /// 机种下产品编码列表（产品组）
    /// </summary>
    public List<string> ProductCodes { get; set; } = new();

    /// <summary>
    /// 机种各月材料成本（产品月成本算术平均，与主表机种月平均口径一致）
    /// </summary>
    public Dictionary<string, decimal> ModelPeriodMaterialCosts { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 机种月材料成本环比涨跌
    /// </summary>
    public string ModelTrend { get; set; } = "none";

    /// <summary>
    /// 机种环比基准月
    /// </summary>
    public string? ModelBasePeriod { get; set; }

    /// <summary>
    /// 机种环比对比月
    /// </summary>
    public string? ModelComparePeriod { get; set; }

    /// <summary>
    /// 机种环比差额
    /// </summary>
    public decimal? ModelVarianceAmount { get; set; }

    /// <summary>
    /// 机种环比变动率（百分点，如 -0.34 表示 -0.34%；导出 Excel 时 ÷100）
    /// </summary>
    public decimal? ModelVariancePercent { get; set; }

    /// <summary>
    /// 合并分析行总数（分页前，已应用涨跌筛选）
    /// </summary>
    public int ComponentCount { get; set; }

    /// <summary>
    /// 环比基准期间（分析行）
    /// </summary>
    public string? BasePeriod { get; set; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; set; }

    /// <summary>
    /// 涨价分析行数（筛选前全量统计）
    /// </summary>
    public int UpCount { get; set; }

    /// <summary>
    /// 跌价分析行数
    /// </summary>
    public int DownCount { get; set; }

    /// <summary>
    /// 持平行数
    /// </summary>
    public int FlatCount { get; set; }

    /// <summary>
    /// 无法比较行数
    /// </summary>
    public int NoneCount { get; set; }

    /// <summary>
    /// 全量分析行各期间材料成本合计（分页前、已应用涨跌筛选）
    /// </summary>
    public Dictionary<string, decimal> PeriodCostTotals { get; set; } = new(StringComparer.Ordinal);

    /// <summary>
    /// 全量分析行环比差额合计（分页前、已应用涨跌筛选）
    /// </summary>
    public decimal? VarianceAmountTotal { get; set; }
}

/// <summary>
/// 机种下 X+F 且移动平均价=0 的 BOM 行按组件合并查询（核算月必填）
/// </summary>
public class TaktBomMaterialCostItemZeroMovingPriceQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 工厂代码（必填）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（必填；从 TaktBomMaterialCost 取下属全部产品）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 核算日期起（须与止同属一个月；服务归一到月初～月末）
    /// </summary>
    public DateTime? CostingDateStart { get; set; }

    /// <summary>
    /// 核算日期止
    /// </summary>
    public DateTime? CostingDateEnd { get; set; }
}

/// <summary>
/// 零价格合并行：机种 + 组件 + 共用产品列表
/// </summary>
public class TaktBomMaterialCostItemZeroMovingPriceDto
{
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件编码
    /// </summary>
    public string ComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 组件描述
    /// </summary>
    public string ComponentDescription { get; set; } = string.Empty;

    /// <summary>
    /// 共用该组件且零价的产品编码（逗号分隔，如 C,D,E,F）
    /// </summary>
    public string ProductCodes { get; set; } = string.Empty;

    /// <summary>
    /// 共用产品数
    /// </summary>
    public int ProductCount { get; set; }

    /// <summary>
    /// 移动平均价（本清单恒为 0）
    /// </summary>
    public decimal MovingAveragePrice { get; set; }

    /// <summary>
    /// 建议代替组件：末字母依次前推（如 A00001D→A00001C）且同月移动价&gt;0 的首个编码；无则为空
    /// </summary>
    public string SuggestedComponentCode { get; set; } = string.Empty;

    /// <summary>
    /// 建议代替组件的移动价格（相对价格单位；无建议则为 null）
    /// </summary>
    public decimal? SuggestedMovingPrice { get; set; }

    /// <summary>
    /// 核算月 yyyy-MM
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;
}

/// <summary>
/// 零价格合并分析结果
/// </summary>
public class TaktBomMaterialCostItemZeroMovingPriceResultDto
{
    /// <summary>
    /// 分页合并行
    /// </summary>
    public TaktPagedResult<TaktBomMaterialCostItemZeroMovingPriceDto> Paged { get; set; } = null!;

    /// <summary>
    /// 机种下产品编码列表
    /// </summary>
    public List<string> ProductCodes { get; set; } = new();

    /// <summary>
    /// 合并后零价组件总数（分页前）
    /// </summary>
    public int ComponentCount { get; set; }

    /// <summary>
    /// 核算月 yyyy-MM
    /// </summary>
    public string CostingPeriod { get; set; } = string.Empty;
}
