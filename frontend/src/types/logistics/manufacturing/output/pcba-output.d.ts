// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：pcba-output.d.ts
// 创建时间：2026-08-11
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
 * PCBA日报实体 达成率(%) = 明细当日完成数量合计 ÷ 明细人员标准产能合计 × 100%。
 * 对应前端 TaktPcbaOutputDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PcbaOutput
 * @description 对应后端 TaktPcbaOutputDto
 */
export interface PcbaOutput extends CompanyDtoBase {


  /**
   * 生产类别（字典 logistics_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 生产日期
   */
  prodDate?: string;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
   */
  prodOrderCode?: string;

  /**
   * 机种（回填：随工单）
   */
  modelCode?: string;

  /**
   * 物料编码（回填：随工单）
   */
  materialCode?: string;

  /**
   * 批次（回填：随工单）
   */
  batchCode?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty?: number;

  /**
   * 序列号（回填：随工单）
   */
  serialCode?: string;

  /**
   * PCBA明细列表（子表，级联保存）
   */
  pcbaOutputDetails?: PcbaOutputDetailCreate[];

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
 * PcbaOutput 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PcbaOutputExport
 * @description 对应后端 TaktPcbaOutputExportDto
 */
export interface PcbaOutputExport {
  /**
   * PcbaOutputID
   */
  pcbaOutputId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（回填：随工单）
   */
  plantCode: string;

  /**
   * 生产类别（字典 logistics_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
   */
  prodOrderCode: string;

  /**
   * 机种（回填：随工单）
   */
  modelCode: string;

  /**
   * 物料编码（回填：随工单）
   */
  materialCode: string;

  /**
   * 批次（回填：随工单）
   */
  batchCode?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty: number;

  /**
   * 序列号（回填：随工单）
   */
  serialCode?: string;

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

