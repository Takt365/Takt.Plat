// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/controlling
// 文件名称：cost-element.d.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/controlling 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 成本要素实体
 * 对应前端 TaktCostElementDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CostElement
 * @description 对应后端 TaktCostElementDto
 */
export interface CostElement extends CompanyDtoBase {

  /**
   * 成本要素状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  costElementStatus?: number;

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
 * CostElement 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CostElementExport
 * @description 对应后端 TaktCostElementExportDto
 */
export interface CostElementExport {
  /**
   * CostElementID
   */
  costElementId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 成本要素编码
   */
  costElementCode: string;

  /**
   * 成本要素名称
   */
  costElementName: string;

  /**
   * 成本要素类型（字典 accounting_cost_element_type；0=初级，1=次级；由 KATYP 推导）
   */
  costElementType: number;

  /**
   * 成本要素类别（字典 accounting_cost_element_category；SAP KATYP 整型值）
   */
  costElementCategory: number;

  /**
   * 父级 ID
   */
  parentId: string;

  /**
   * 成本要素层级
   */
  costElementLevel: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 关联工厂
   */
  plantCode: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 成本要素状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  costElementStatus: number;

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

