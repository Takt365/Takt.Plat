// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：assy-order-defect.d.ts
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
 * 组立工单不良统计实体（统计维度：生产类别+工单号）
 * 对应前端 TaktAssyOrderDefectDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyOrderDefect
 * @description 对应后端 TaktAssyOrderDefectDto
 */
export interface AssyOrderDefect extends CompanyDtoBase {
  /**
   * AssyOrderDefectID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assyOrderDefectId: string;

  /**
   * 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 生产类别（统计维度，字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 工单号（统计维度，选项 TaktProductionOrders/options）
   */
  prodOrderCode: string;

  /**
   * 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
   */
  prodDateGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode: string;

  /**
   * 物料编码（取最近日报）
   */
  materialCode: string;

  /**
   * 批次（一工单一批次，取最近日报）
   */
  batchNo?: string;

  /**
   * 工单数量（取最近日报）
   */
  prodOrderQty: number;

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
   * 工单状态（字典 logistics_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
   */
  orderStatus: number;

}


/**
 * AssyOrderDefect 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssyOrderDefectQuery
 * @description 对应后端 TaktAssyOrderDefectQueryDto
 */
export interface AssyOrderDefectQuery extends TaktPagedQuery {
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
   * 工单号（统计维度，选项 TaktProductionOrders/options）
   */
  prodOrderCode?: string;

  /**
   * 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
   */
  prodDateGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode?: string;

  /**
   * 物料编码（取最近日报）
   */
  materialCode?: string;

  /**
   * 批次（一工单一批次，取最近日报）
   */
  batchNo?: string;

  /**
   * 工单数量（取最近日报）
   */
  prodOrderQty?: number;

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
   * 工单状态（字典 logistics_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
   */
  orderStatus?: number;

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
 * 创建AssyOrderDefect DTO
 * 对应前端 AssyOrderDefectCreate
 * @description 对应后端 TaktAssyOrderDefectCreateDto
 */
export interface AssyOrderDefectCreate {
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
   * 工单号（统计维度，选项 TaktProductionOrders/options）
   */
  prodOrderCode: string;

  /**
   * 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
   */
  prodDateGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode: string;

  /**
   * 物料编码（取最近日报）
   */
  materialCode: string;

  /**
   * 批次（一工单一批次，取最近日报）
   */
  batchNo?: string;

  /**
   * 工单数量（取最近日报）
   */
  prodOrderQty: number;

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
   * 工单状态（字典 logistics_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
   */
  orderStatus: number;

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
 * 更新AssyOrderDefect DTO
 * 继承 TaktAssyOrderDefectCreateDto，添加 AssyOrderDefectId 字段
 * 对应前端 AssyOrderDefectUpdate
 * @description 对应后端 TaktAssyOrderDefectUpdateDto
 */
export interface AssyOrderDefectUpdate extends AssyOrderDefectCreate {
  /**
   * AssyOrderDefectID（标识要更新的实体）
   */
  assyOrderDefectId: string;

}


/**
 * AssyOrderDefect 状态更新 DTO
 * 对应前端 AssyOrderDefectStatus
 * @description 对应后端 TaktAssyOrderDefectStatusDto
 */
export interface AssyOrderDefectStatus {
  /**
   * AssyOrderDefectID
   */
  assyOrderDefectId: string;

  /**
   * 工单状态（字典 logistics_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
   */
  orderStatus: number;

}


/**
 * AssyOrderDefect 导入模板行 DTO
 * 对应前端 AssyOrderDefectTemplate
 * @description 对应后端 TaktAssyOrderDefectTemplateDto
 */
export interface AssyOrderDefectTemplate {
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
   * 工单号（统计维度，选项 TaktProductionOrders/options）
   */
  prodOrderCode?: string;

  /**
   * 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
   */
  prodDateGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode?: string;

  /**
   * 物料编码（取最近日报）
   */
  materialCode?: string;

  /**
   * 批次（一工单一批次，取最近日报）
   */
  batchNo?: string;

  /**
   * 工单数量（取最近日报）
   */
  prodOrderQty?: number;

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
   * 工单状态（字典 logistics_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
   */
  orderStatus?: number;

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
 * AssyOrderDefect 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssyOrderDefectImport
 * @description 对应后端 TaktAssyOrderDefectImportDto
 */
export interface AssyOrderDefectImport {
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
   * 工单号（统计维度，选项 TaktProductionOrders/options）
   */
  prodOrderCode?: string;

  /**
   * 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
   */
  prodDateGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode?: string;

  /**
   * 物料编码（取最近日报）
   */
  materialCode?: string;

  /**
   * 批次（一工单一批次，取最近日报）
   */
  batchNo?: string;

  /**
   * 工单数量（取最近日报）
   */
  prodOrderQty?: number;

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
   * 工单状态（字典 logistics_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
   */
  orderStatus?: number;

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
 * AssyOrderDefect 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssyOrderDefectExport
 * @description 对应后端 TaktAssyOrderDefectExportDto
 */
export interface AssyOrderDefectExport {
  /**
   * AssyOrderDefectID
   */
  assyOrderDefectId: string;

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
   * 工单号（统计维度，选项 TaktProductionOrders/options）
   */
  prodOrderCode: string;

  /**
   * 生产日期组（汇总日报去重生产日期，yyyy-MM-dd 逗号分隔升序排列）
   */
  prodDateGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode: string;

  /**
   * 物料编码（取最近日报）
   */
  materialCode: string;

  /**
   * 批次（一工单一批次，取最近日报）
   */
  batchNo?: string;

  /**
   * 工单数量（取最近日报）
   */
  prodOrderQty: number;

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
   * 工单状态（字典 logistics_prod_status；1=进行中 2=已完成；工单数量与累计生实实绩相等时为已完成）
   */
  orderStatus: number;

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

