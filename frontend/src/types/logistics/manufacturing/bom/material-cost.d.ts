// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：material-cost.d.ts
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
 * BOM 物料成本汇总表（由明细按工厂+机种+产品+核算月聚合写入；UI 机种组→产品行→Item，实体不拆分，无对明细导航）
 * 对应前端 TaktBomMaterialCostDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BomMaterialCost
 * @description 对应后端 TaktBomMaterialCostDto
 */
export interface BomMaterialCost extends CompanyDtoBase {
  /**
   * BomMaterialCostID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  bomMaterialCostId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 机种编码（关联 TaktModelDestination.ModelCode）
   */
  modelCode: string;

  /**
   * 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
   */
  modelMonthlyAverageCost: number;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode: string;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
   * 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
   */
  productMonthlyCost: number;

  /**
   * 币种（字典 accounting_currency_code，如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod: string;

  /**
   * 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）
   */
  costingDate: string;

}


/**
 * BomMaterialCost 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BomMaterialCostQuery
 * @description 对应后端 TaktBomMaterialCostQueryDto
 */
export interface BomMaterialCostQuery extends TaktPagedQuery {
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
   * 机种编码（关联 TaktModelDestination.ModelCode）
   */
  modelCode?: string;

  /**
   * 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
   */
  modelMonthlyAverageCost?: number;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode?: string;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
   * 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
   */
  productMonthlyCost?: number;

  /**
   * 币种（字典 accounting_currency_code，如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod?: string;

  /**
   * 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）（范围查询-开始）
   */
  costingDateStart?: string;

  /**
   * 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）（范围查询-结束）
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
 * 创建BomMaterialCost DTO
 * 对应前端 BomMaterialCostCreate
 * @description 对应后端 TaktBomMaterialCostCreateDto
 */
export interface BomMaterialCostCreate {
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
   * 机种编码（关联 TaktModelDestination.ModelCode）
   */
  modelCode: string;

  /**
   * 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
   */
  modelMonthlyAverageCost: number;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode: string;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
   * 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
   */
  productMonthlyCost: number;

  /**
   * 币种（字典 accounting_currency_code，如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod: string;

  /**
   * 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）
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
 * 更新BomMaterialCost DTO
 * 继承 TaktBomMaterialCostCreateDto，添加 BomMaterialCostId 字段
 * 对应前端 BomMaterialCostUpdate
 * @description 对应后端 TaktBomMaterialCostUpdateDto
 */
export interface BomMaterialCostUpdate extends BomMaterialCostCreate {
  /**
   * BomMaterialCostID（标识要更新的实体）
   */
  bomMaterialCostId: string;

}


/**
 * BomMaterialCost 导入模板行 DTO
 * 对应前端 BomMaterialCostTemplate
 * @description 对应后端 TaktBomMaterialCostTemplateDto
 */
export interface BomMaterialCostTemplate {
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
   * 机种编码（关联 TaktModelDestination.ModelCode）
   */
  modelCode?: string;

  /**
   * 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
   */
  modelMonthlyAverageCost?: number;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode?: string;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
   * 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
   */
  productMonthlyCost?: number;

  /**
   * 币种（字典 accounting_currency_code，如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod?: string;

  /**
   * 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）
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
 * BomMaterialCost 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BomMaterialCostImport
 * @description 对应后端 TaktBomMaterialCostImportDto
 */
export interface BomMaterialCostImport {
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
   * 机种编码（关联 TaktModelDestination.ModelCode）
   */
  modelCode?: string;

  /**
   * 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
   */
  modelMonthlyAverageCost?: number;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode?: string;

  /**
   * 产品描述
   */
  productDescription?: string;

  /**
   * 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
   */
  productMonthlyCost?: number;

  /**
   * 币种（字典 accounting_currency_code，如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod?: string;

  /**
   * 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）
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
 * BomMaterialCost 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BomMaterialCostExport
 * @description 对应后端 TaktBomMaterialCostExportDto
 */
export interface BomMaterialCostExport {
  /**
   * BomMaterialCostID
   */
  bomMaterialCostId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 机种编码（关联 TaktModelDestination.ModelCode）
   */
  modelCode: string;

  /**
   * 机种月平均材料成本（同工厂+机种+核算月份下各成品产品月成本算术平均）
   */
  modelMonthlyAverageCost: number;

  /**
   * 产品编码（父件物料编码，关联 TaktMaterial.MaterialCode）；导入时 18 位纯数字自动归一化为后 10 位
   */
  productCode: string;

  /**
   * 产品描述
   */
  productDescription: string;

  /**
   * 产品月成本（该产品在核算月份下 BOM 材料成本明细汇总）
   */
  productMonthlyCost: number;

  /**
   * 币种（字典 accounting_currency_code，如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 核算期间（yyyy-MM；由核算日期推导；与工厂+机种+产品构成唯一键，同月仅一行）
   */
  costingPeriod: string;

  /**
   * 核算日期（同月最后核算日；明细可能有多日，主表存最后一日的汇总结果）
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
 * BOM 物料成本机种维度展示行（同一物理表按工厂+机种+核算期间去重聚合；非独立实体）
 * 对应前端 BomMaterialCostModelGroup
 * @description 对应后端 TaktBomMaterialCostModelGroupDto
 */
export interface BomMaterialCostModelGroup {
  /**
   * 分组键（plant|model|period，供前端 row-key）
   */
  groupKey: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 机种编码
   */
  modelCode: string;

  /**
   * 机种月平均材料成本
   */
  modelMonthlyAverageCost: number;

  /**
   * 币种
   */
  currencyCode: string;

  /**
   * 核算期间（yyyy-MM）
   */
  costingPeriod: string;

  /**
   * 核算日期（组内代表行，通常为同月最后核算日）
   */
  costingDate: string;

  /**
   * 组内产品行数
   */
  productRowCount: number;

}

