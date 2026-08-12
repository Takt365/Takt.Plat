// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：assy-output.d.ts
// 创建时间：2026-07-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 组立日报（产出）主表实体 达成率(%) = 明细实际生产数量合计 ÷ 主表标准产能合计 × 100%。
 * 对应前端 TaktAssyOutputDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyOutput
 * @description 对应后端 TaktAssyOutputDto
 */
export interface AssyOutput extends CompanyDtoBase {

  /**
   * 生产类别（字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 生产日期
   */
  prodDate?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 直接人员
   */
  directLabor?: number;

  /**
   * 间接人员
   */
  indirectLabor?: number;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 订单类别（选项 TaktProductionOrders/options 的 ExtLabel，随工单回填）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
   */
  prodOrderCode?: string;

  /**
   * 机种
   */
  modelCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 订单数量
   */
  prodOrderQty?: number;

  /**
   * 序列号（回填：随工单）
   */
  serialCode?: string;

  /**
   * 标准工时(分钟)
   */
  stdMinutes?: number;

  /**
   * 标准产能
   */
  stdCapacity?: number;

  /**
   * 组立日报明细列表（子表，级联保存）
   */
  assyOutputDetails?: AssyOutputDetailCreate[];

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
 * AssyOutput 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssyOutputExport
 * @description 对应后端 TaktAssyOutputExportDto
 */
export interface AssyOutputExport {
  /**
   * AssyOutputID
   */
  assyOutputId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（回填：随工单）
   */
  plantCode: string;

  /**
   * 生产类别（字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode: string;

  /**
   * 直接人员
   */
  directLabor: number;

  /**
   * 间接人员
   */
  indirectLabor: number;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 订单类别（选项 TaktProductionOrders/options 的 ExtLabel，随工单回填）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
   */
  prodOrderCode: string;

  /**
   * 机种
   */
  modelCode: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 订单数量
   */
  prodOrderQty: number;

  /**
   * 序列号（回填：随工单）
   */
  serialCode?: string;

  /**
   * 标准工时(分钟)
   */
  stdMinutes: number;

  /**
   * 标准产能
   */
  stdCapacity: number;

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

