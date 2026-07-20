// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：material-cost-item.d.ts
// 创建时间：2026-07-14
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * BOM 物料成本明细行（业务源数据：先导入/维护明细，再按工厂+产品+核算月聚合写入 TaktBomMaterialCost；无线上主表外键）
 * 对应前端 TaktBomMaterialCostItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BomMaterialCostItem
 * @description 对应后端 TaktBomMaterialCostItemDto
 */
export interface BomMaterialCostItem extends CompanyDtoBase {
  /**
   * BomMaterialCostItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  bomMaterialCostItemId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode: string;

  /**
   * 序号（展开行序号，如 0010）
   */
  sequenceNo: string;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemNo: string;

  /**
   * 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode: string;

  /**
   * 组件描述
   */
  componentDescription: string;

  /**
   * 组件数量
   */
  componentQuantity: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
   */
  profitCenterCode: string;

  /**
   * 移动平均价
   */
  movingAveragePrice: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit: number;

  /**
   * 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
   */
  movingPriceCurrency: string;

  /**
   * 采购组织
   */
  purchaseOrganization: string;

  /**
   * 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
   */
  purchaseGroup: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 净价（采购价格）
   */
  netPurchasePrice: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit: number;

  /**
   * 采购货币（字典 accounting_currency_code，如 CNY/USD）
   */
  purchaseCurrency: string;

  /**
   * 核算日期
   */
  costingDate: string;

}


/**
 * BomMaterialCostItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BomMaterialCostItemQuery
 * @description 对应后端 TaktBomMaterialCostItemQueryDto
 */
export interface BomMaterialCostItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 机种编码（成本合计按主表过滤；明细实体本身无此列）
   */
  modelCode?: string;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode?: string;

  /**
   * 序号（展开行序号，如 0010）
   */
  sequenceNo?: string;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel?: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemNo?: string;

  /**
   * 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode?: string;

  /**
   * 组件描述
   */
  componentDescription?: string;

  /**
   * 组件数量
   */
  componentQuantity?: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType?: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
   */
  profitCenterCode?: string;

  /**
   * 移动平均价
   */
  movingAveragePrice?: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit?: number;

  /**
   * 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
   */
  movingPriceCurrency?: string;

  /**
   * 采购组织
   */
  purchaseOrganization?: string;

  /**
   * 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
   */
  purchaseGroup?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 净价（采购价格）
   */
  netPurchasePrice?: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit?: number;

  /**
   * 采购货币（字典 accounting_currency_code，如 CNY/USD）
   */
  purchaseCurrency?: string;

  /**
   * 核算日期（范围查询-开始）
   */
  costingDateStart?: string;

  /**
   * 核算日期（范围查询-结束）
   */
  costingDateEnd?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建BomMaterialCostItem DTO
 * 对应前端 BomMaterialCostItemCreate
 * @description 对应后端 TaktBomMaterialCostItemCreateDto
 */
export interface BomMaterialCostItemCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode: string;

  /**
   * 序号（展开行序号，如 0010）
   */
  sequenceNo: string;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemNo: string;

  /**
   * 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode: string;

  /**
   * 组件描述
   */
  componentDescription: string;

  /**
   * 组件数量
   */
  componentQuantity: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
   */
  profitCenterCode: string;

  /**
   * 移动平均价
   */
  movingAveragePrice: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit: number;

  /**
   * 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
   */
  movingPriceCurrency: string;

  /**
   * 采购组织
   */
  purchaseOrganization: string;

  /**
   * 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
   */
  purchaseGroup: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 净价（采购价格）
   */
  netPurchasePrice: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit: number;

  /**
   * 采购货币（字典 accounting_currency_code，如 CNY/USD）
   */
  purchaseCurrency: string;

  /**
   * 核算日期
   */
  costingDate: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新BomMaterialCostItem DTO
 * 继承 TaktBomMaterialCostItemCreateDto，添加 BomMaterialCostItemId 字段
 * 对应前端 BomMaterialCostItemUpdate
 * @description 对应后端 TaktBomMaterialCostItemUpdateDto
 */
export interface BomMaterialCostItemUpdate extends BomMaterialCostItemCreate {
  /**
   * BomMaterialCostItemID（标识要更新的实体）
   */
  bomMaterialCostItemId: string;

}


/**
 * BomMaterialCostItem 导入模板行 DTO
 * 对应前端 BomMaterialCostItemTemplate
 * @description 对应后端 TaktBomMaterialCostItemTemplateDto
 */
export interface BomMaterialCostItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode?: string;

  /**
   * 序号（展开行序号，如 0010）
   */
  sequenceNo?: string;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel?: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemNo?: string;

  /**
   * 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode?: string;

  /**
   * 组件描述
   */
  componentDescription?: string;

  /**
   * 组件数量
   */
  componentQuantity?: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType?: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
   */
  profitCenterCode?: string;

  /**
   * 移动平均价
   */
  movingAveragePrice?: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit?: number;

  /**
   * 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
   */
  movingPriceCurrency?: string;

  /**
   * 采购组织
   */
  purchaseOrganization?: string;

  /**
   * 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
   */
  purchaseGroup?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 净价（采购价格）
   */
  netPurchasePrice?: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit?: number;

  /**
   * 采购货币（字典 accounting_currency_code，如 CNY/USD）
   */
  purchaseCurrency?: string;

  /**
   * 核算日期
   */
  costingDate?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * BomMaterialCostItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BomMaterialCostItemImport
 * @description 对应后端 TaktBomMaterialCostItemImportDto
 */
export interface BomMaterialCostItemImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode?: string;

  /**
   * 序号（展开行序号，如 0010）
   */
  sequenceNo?: string;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel?: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemNo?: string;

  /**
   * 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode?: string;

  /**
   * 组件描述
   */
  componentDescription?: string;

  /**
   * 组件数量
   */
  componentQuantity?: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType?: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
   */
  profitCenterCode?: string;

  /**
   * 移动平均价
   */
  movingAveragePrice?: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit?: number;

  /**
   * 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
   */
  movingPriceCurrency?: string;

  /**
   * 采购组织
   */
  purchaseOrganization?: string;

  /**
   * 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
   */
  purchaseGroup?: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 净价（采购价格）
   */
  netPurchasePrice?: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit?: number;

  /**
   * 采购货币（字典 accounting_currency_code，如 CNY/USD）
   */
  purchaseCurrency?: string;

  /**
   * 核算日期
   */
  costingDate?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * BomMaterialCostItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BomMaterialCostItemExport
 * @description 对应后端 TaktBomMaterialCostItemExportDto
 */
export interface BomMaterialCostItemExport {
  /**
   * BomMaterialCostItemID
   */
  bomMaterialCostItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode: string;

  /**
   * 序号（展开行序号，如 0010）
   */
  sequenceNo: string;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
   * 层级（BOM 展开层级，如 01/02）
   */
  bomLevel: string;

  /**
   * BOM 项目号（子件行项目号，如 0010）
   */
  bomItemNo: string;

  /**
   * 组件编码（子件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  componentCode: string;

  /**
   * 组件描述
   */
  componentDescription: string;

  /**
   * 组件数量
   */
  componentQuantity: number;

  /**
   * 批量标识（空或 X）
   */
  batchIndicator?: string;

  /**
   * 生产相关（空或 X）
   */
  productionRelated?: string;

  /**
   * 采购类型（F=外部采购，E=自制生产）；仅生产相关=X 且 F 行参与产品 BOM 材料成本汇总，行成本=组件数量×(移动平均价÷移动价格单位) 保留 5 位小数
   */
  purchaseType: string;

  /**
   * 特殊采购类（空或业务码，最长 50）
   */
  specialProcurementType?: string;

  /**
   * 利润中心（关联 TaktProfitCenter.ProfitCenterCode，选项 TaktProfitCenters/options）
   */
  profitCenterCode: string;

  /**
   * 移动平均价
   */
  movingAveragePrice: number;

  /**
   * 移动价格单位
   */
  movingPriceUnit: number;

  /**
   * 移动价格货币（字典 accounting_currency_code，如 CNY/USD）
   */
  movingPriceCurrency: string;

  /**
   * 采购组织
   */
  purchaseOrganization: string;

  /**
   * 采购组（关联 TaktPurchaseGroup.PurchaseGroupCode，选项 TaktPurchaseGroups/options）
   */
  purchaseGroup: string;

  /**
   * 供应商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 净价（采购价格）
   */
  netPurchasePrice: number;

  /**
   * 采购价格单位
   */
  purchasePriceUnit: number;

  /**
   * 采购货币（字典 accounting_currency_code，如 CNY/USD）
   */
  purchaseCurrency: string;

  /**
   * 核算日期
   */
  costingDate: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}


/**
 * 机种月平均重算：规范化后的查询与月份标签
 * 对应前端 BomMaterialCostItemRecalculatePreparedQuery
 * @description 对应后端 TaktBomMaterialCostItemRecalculatePreparedQueryDto
 */
export interface BomMaterialCostItemRecalculatePreparedQuery {
  /**
   * 规范化后的明细查询（单核算月日期范围）
   */
  query: BomMaterialCostItemQuery;

  /**
   * 核算月份（yyyy-MM）
   */
  processedMonth: string;

}


/**
 * 机种月平均重算任务已提交回执
 * 对应前端 BomMaterialCostItemRecalculateSubmitted
 * @description 对应后端 TaktBomMaterialCostItemRecalculateSubmittedDto
 */
export interface BomMaterialCostItemRecalculateSubmitted {
  /**
   * 核算月份（yyyy-MM）
   */
  processedMonth: string;

  /**
   * 是否强制重算（重置）
   */
  forceRecalculate: boolean;

  /**
   * 处理记录数上限（工厂+产品组；0=全部；默认 5000）
   */
  processRecordCount: number;
}


/**
 * 机种月平均重算执行结果（同步或后台完成后）
 * 对应前端 BomMaterialCostItemRecalculateModelAverageResult
 * @description 对应后端 TaktBomMaterialCostItemRecalculateModelAverageResultDto
 */
export interface BomMaterialCostItemRecalculateModelAverageResult {
  /**
   * 扫描明细行数
   */
  scannedRowCount: number;

  /**
   * 刷新汇总组数
   */
  refreshedGroupCount: number;

  /**
   * 跳过组数
   */
  skippedGroupCount: number;

  /**
   * 强制重置涉及组数
   */
  resetGroupCount: number;

  /**
   * 处理月份数
   */
  processedMonthCount: number;

  /**
   * 核算月份（yyyy-MM）
   */
  processedMonth: string;

}

