// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：pcba-output.d.ts
// 创建时间：2026-07-13
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
   * PcbaOutputID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  pcbaOutputId: string;

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
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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
  batchNo?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty: number;

  /**
   * 序列号（回填：随工单）
   */
  serialNo?: string;

  /**
   * PCBA明细列表 （子表：TaktPcbaOutputDetail）
   */
  pcbaOutputDetails?: PcbaOutputDetail[];

}


/**
 * PcbaOutput 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PcbaOutputQuery
 * @description 对应后端 TaktPcbaOutputQueryDto
 */
export interface PcbaOutputQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（回填：随工单）
   */
  plantCode?: string;

  /**
   * 生产类别（字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 生产日期（范围查询-开始）
   */
  prodDateStart?: string;

  /**
   * 生产日期（范围查询-结束）
   */
  prodDateEnd?: string;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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
  batchNo?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty?: number;

  /**
   * 序列号（回填：随工单）
   */
  serialNo?: string;

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
 * 创建PcbaOutput DTO
 * 对应前端 PcbaOutputCreate
 * @description 对应后端 TaktPcbaOutputCreateDto
 */
export interface PcbaOutputCreate {
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
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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
  batchNo?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty: number;

  /**
   * 序列号（回填：随工单）
   */
  serialNo?: string;

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
 * 更新PcbaOutput DTO
 * 继承 TaktPcbaOutputCreateDto，添加 PcbaOutputId 字段
 * 对应前端 PcbaOutputUpdate
 * @description 对应后端 TaktPcbaOutputUpdateDto
 */
export interface PcbaOutputUpdate extends PcbaOutputCreate {
  /**
   * PcbaOutputID（标识要更新的实体）
   */
  pcbaOutputId: string;

  /**
   * PCBA明细列表（子表，级联保存）
   */
  pcbaOutputDetails?: any;

}


/**
 * PcbaOutput 导入模板行 DTO
 * 对应前端 PcbaOutputTemplate
 * @description 对应后端 TaktPcbaOutputTemplateDto
 */
export interface PcbaOutputTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（回填：随工单）
   */
  plantCode?: string;

  /**
   * 生产类别（字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
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
   * 工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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
  batchNo?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty?: number;

  /**
   * 序列号（回填：随工单）
   */
  serialNo?: string;

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
 * PcbaOutput 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PcbaOutputImport
 * @description 对应后端 TaktPcbaOutputImportDto
 */
export interface PcbaOutputImport {
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
   * 工厂代码（回填：随工单）
   */
  plantCode?: string;

  /**
   * 生产类别（字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
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
   * 工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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
  batchNo?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty?: number;

  /**
   * 序列号（回填：随工单）
   */
  serialNo?: string;

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
   * 生产类别（字典 logistics_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
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
   * 工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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
  batchNo?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty: number;

  /**
   * 序列号（回填：随工单）
   */
  serialNo?: string;

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
 * PCBA 日报新增时按标准工序时间生成的默认明细预览
 */
export interface PcbaOutputDefaultDetail {
  /** 行号 */
  lineNumber: number;
  /** 工作中心（对应明细 timePeriod） */
  workCenter: string;
  /** 工序描述 */
  operationDesc?: string;
  /** 标准点数 */
  standardShorts: number;
}

