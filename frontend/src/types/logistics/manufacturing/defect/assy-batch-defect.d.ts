// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：assy-batch-defect.d.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/defect 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 组立批量不良统计实体（统计维度：生产类别+批次）
 * 对应前端 TaktAssyBatchDefectDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyBatchDefect
 * @description 对应后端 TaktAssyBatchDefectDto
 */
export interface AssyBatchDefect extends CompanyDtoBase {
  /**
   * AssyBatchDefectID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assyBatchDefectId: string;

  /**
   * 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 生产类别（统计维度，字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 批次（统计维度）
   */
  batchNo: string;

  /**
   * 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
   */
  prodDateGroup?: string;

  /**
   * 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
   */
  prodOrderGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode: string;

  /**
   * 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
   */
  materialGroup?: string;

  /**
   * 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
   */
  batchOrderQty: number;

  /**
   * 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
   */
  prodOrderQtyGroup?: string;

  /**
   * 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
   */
  prodActualQty: number;

  /**
   * 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
   */
  goodQuantity: number;

  /**
   * 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
   */
  defectQty: number;

  /**
   * 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
   */
  defectRatePercent: number;

  /**
   * 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
   */
  yieldRatePercent: number;

  /**
   * 最近生产日期（关联日报最大 ProdDate）
   */
  lastProdDate?: string;

  /**
   * 关联组立不良日报笔数
   */
  reportCount: number;

  /**
   * 批次状态（字典 logistics_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
   */
  batchStatus: number;

}


/**
 * AssyBatchDefect 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssyBatchDefectQuery
 * @description 对应后端 TaktAssyBatchDefectQueryDto
 */
export interface AssyBatchDefectQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 生产类别（统计维度，字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 批次（统计维度）
   */
  batchNo?: string;

  /**
   * 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
   */
  prodDateGroup?: string;

  /**
   * 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
   */
  prodOrderGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode?: string;

  /**
   * 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
   */
  materialGroup?: string;

  /**
   * 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
   */
  batchOrderQty?: number;

  /**
   * 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
   */
  prodOrderQtyGroup?: string;

  /**
   * 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
   */
  prodActualQty?: number;

  /**
   * 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
   */
  goodQuantity?: number;

  /**
   * 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
   */
  defectQty?: number;

  /**
   * 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
   */
  defectRatePercent?: number;

  /**
   * 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
   */
  yieldRatePercent?: number;

  /**
   * 最近生产日期（关联日报最大 ProdDate）（范围查询-开始）
   */
  lastProdDateStart?: string;

  /**
   * 最近生产日期（关联日报最大 ProdDate）（范围查询-结束）
   */
  lastProdDateEnd?: string;

  /**
   * 关联组立不良日报笔数
   */
  reportCount?: number;

  /**
   * 批次状态（字典 logistics_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
   */
  batchStatus?: number;

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
 * 创建AssyBatchDefect DTO
 * 对应前端 AssyBatchDefectCreate
 * @description 对应后端 TaktAssyBatchDefectCreateDto
 */
export interface AssyBatchDefectCreate {
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
   * 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 生产类别（统计维度，字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 批次（统计维度）
   */
  batchNo: string;

  /**
   * 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
   */
  prodDateGroup?: string;

  /**
   * 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
   */
  prodOrderGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode: string;

  /**
   * 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
   */
  materialGroup?: string;

  /**
   * 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
   */
  batchOrderQty: number;

  /**
   * 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
   */
  prodOrderQtyGroup?: string;

  /**
   * 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
   */
  prodActualQty: number;

  /**
   * 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
   */
  goodQuantity: number;

  /**
   * 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
   */
  defectQty: number;

  /**
   * 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
   */
  defectRatePercent: number;

  /**
   * 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
   */
  yieldRatePercent: number;

  /**
   * 最近生产日期（关联日报最大 ProdDate）
   */
  lastProdDate?: string;

  /**
   * 关联组立不良日报笔数
   */
  reportCount: number;

  /**
   * 批次状态（字典 logistics_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
   */
  batchStatus: number;

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
 * 更新AssyBatchDefect DTO
 * 继承 TaktAssyBatchDefectCreateDto，添加 AssyBatchDefectId 字段
 * 对应前端 AssyBatchDefectUpdate
 * @description 对应后端 TaktAssyBatchDefectUpdateDto
 */
export interface AssyBatchDefectUpdate extends AssyBatchDefectCreate {
  /**
   * AssyBatchDefectID（标识要更新的实体）
   */
  assyBatchDefectId: string;

}


/**
 * AssyBatchDefect 状态更新 DTO
 * 对应前端 AssyBatchDefectStatus
 * @description 对应后端 TaktAssyBatchDefectStatusDto
 */
export interface AssyBatchDefectStatus {
  /**
   * AssyBatchDefectID
   */
  assyBatchDefectId: string;

  /**
   * 批次状态（字典 logistics_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
   */
  batchStatus: number;

}


/**
 * AssyBatchDefect 导入模板行 DTO
 * 对应前端 AssyBatchDefectTemplate
 * @description 对应后端 TaktAssyBatchDefectTemplateDto
 */
export interface AssyBatchDefectTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 生产类别（统计维度，字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 批次（统计维度）
   */
  batchNo?: string;

  /**
   * 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
   */
  prodDateGroup?: string;

  /**
   * 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
   */
  prodOrderGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode?: string;

  /**
   * 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
   */
  materialGroup?: string;

  /**
   * 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
   */
  batchOrderQty?: number;

  /**
   * 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
   */
  prodOrderQtyGroup?: string;

  /**
   * 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
   */
  prodActualQty?: number;

  /**
   * 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
   */
  goodQuantity?: number;

  /**
   * 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
   */
  defectQty?: number;

  /**
   * 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
   */
  defectRatePercent?: number;

  /**
   * 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
   */
  yieldRatePercent?: number;

  /**
   * 最近生产日期（关联日报最大 ProdDate）
   */
  lastProdDate?: string;

  /**
   * 关联组立不良日报笔数
   */
  reportCount?: number;

  /**
   * 批次状态（字典 logistics_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
   */
  batchStatus?: number;

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
 * AssyBatchDefect 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssyBatchDefectImport
 * @description 对应后端 TaktAssyBatchDefectImportDto
 */
export interface AssyBatchDefectImport {
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
   * 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 生产类别（统计维度，字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 批次（统计维度）
   */
  batchNo?: string;

  /**
   * 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
   */
  prodDateGroup?: string;

  /**
   * 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
   */
  prodOrderGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode?: string;

  /**
   * 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
   */
  materialGroup?: string;

  /**
   * 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
   */
  batchOrderQty?: number;

  /**
   * 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
   */
  prodOrderQtyGroup?: string;

  /**
   * 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
   */
  prodActualQty?: number;

  /**
   * 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
   */
  goodQuantity?: number;

  /**
   * 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
   */
  defectQty?: number;

  /**
   * 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
   */
  defectRatePercent?: number;

  /**
   * 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
   */
  yieldRatePercent?: number;

  /**
   * 最近生产日期（关联日报最大 ProdDate）
   */
  lastProdDate?: string;

  /**
   * 关联组立不良日报笔数
   */
  reportCount?: number;

  /**
   * 批次状态（字典 logistics_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
   */
  batchStatus?: number;

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
 * AssyBatchDefect 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssyBatchDefectExport
 * @description 对应后端 TaktAssyBatchDefectExportDto
 */
export interface AssyBatchDefectExport {
  /**
   * AssyBatchDefectID
   */
  assyBatchDefectId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 生产类别（统计维度，字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 批次（统计维度）
   */
  batchNo: string;

  /**
   * 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
   */
  prodDateGroup?: string;

  /**
   * 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
   */
  prodOrderGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode: string;

  /**
   * 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
   */
  materialGroup?: string;

  /**
   * 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
   */
  batchOrderQty: number;

  /**
   * 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
   */
  prodOrderQtyGroup?: string;

  /**
   * 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
   */
  prodActualQty: number;

  /**
   * 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
   */
  goodQuantity: number;

  /**
   * 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
   */
  defectQty: number;

  /**
   * 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
   */
  defectRatePercent: number;

  /**
   * 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
   */
  yieldRatePercent: number;

  /**
   * 最近生产日期（关联日报最大 ProdDate）
   */
  lastProdDate?: string;

  /**
   * 关联组立不良日报笔数
   */
  reportCount: number;

  /**
   * 批次状态（字典 logistics_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
   */
  batchStatus: number;

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

