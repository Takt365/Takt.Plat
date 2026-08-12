// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：assy-defect.d.ts
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
 * 组立不良日报实体 不良率(%) = (生实实绩 - 无不良数量) ÷ 生实实绩 × 100%；直行率(%) = 无不良数量 ÷ 生实实绩 × 100%。
 * 对应前端 TaktAssyDefectDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyDefect
 * @description 对应后端 TaktAssyDefectDto
 */
export interface AssyDefect extends CompanyDtoBase {

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
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktAssyOutputs/prod-order-options，来源组立日报；同日同工单已存在不良日报则不再展示）
   */
  prodOrderCode?: string;

  /**
   * 工单数量
   */
  prodOrderQty?: number;

  /**
   * 机种
   */
  modelCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 生实实绩
   */
  prodActualQty?: number;

  /**
   * 无不良数量
   */
  goodQuantity?: number;

  /**
   * 组立不良明细列表（子表，级联保存）
   */
  assyDefectDetails?: AssyDefectDetailCreate[];

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
 * AssyDefect 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssyDefectExport
 * @description 对应后端 TaktAssyDefectExportDto
 */
export interface AssyDefectExport {
  /**
   * AssyDefectID
   */
  assyDefectId: string;

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
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktAssyOutputs/prod-order-options，来源组立日报；同日同工单已存在不良日报则不再展示）
   */
  prodOrderCode: string;

  /**
   * 工单数量
   */
  prodOrderQty: number;

  /**
   * 机种
   */
  modelCode: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 生实实绩
   */
  prodActualQty: number;

  /**
   * 无不良数量
   */
  goodQuantity: number;

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

