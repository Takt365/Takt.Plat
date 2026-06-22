// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：assy-output.d.ts
// 创建时间：2026-06-20
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
 * 组立日报（产出）主表实体
 * 对应前端 TaktAssyOutputDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyOutput
 * @description 对应后端 TaktAssyOutputDto
 */
export interface AssyOutput extends CompanyDtoBase {
  /**
   * AssyOutputID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assyOutputId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产
   */
  prodCategory: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产线
   */
  prodLine: string;

  /**
   * 直接人员
   */
  directLabor: number;

  /**
   * 间接人员
   */
  indirectLabor: number;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo: number;

  /**
   * 生产订单类型
   */
  prodOrderType?: string;

  /**
   * 生产工单号
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
  batchNo?: string;

  /**
   * 订单数量
   */
  prodOrderQty: number;

  /**
   * 标准工时(分钟)
   */
  stdMinutes: number;

  /**
   * 标准产能
   */
  stdCapacity: number;

  /**
   * 状态
   */
  status: number;

  /**
   * 组立日报明细列表 （子表：TaktAssyOutputDetail）
   */
  assyOutputDetails?: AssyOutputDetail[];

}


/**
 * AssyOutput 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssyOutputQuery
 * @description 对应后端 TaktAssyOutputQueryDto
 */
export interface AssyOutputQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产
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
   * 生产线
   */
  prodLine?: string;

  /**
   * 直接人员
   */
  directLabor?: number;

  /**
   * 间接人员
   */
  indirectLabor?: number;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo?: number;

  /**
   * 生产订单类型
   */
  prodOrderType?: string;

  /**
   * 生产工单号
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
  batchNo?: string;

  /**
   * 订单数量
   */
  prodOrderQty?: number;

  /**
   * 标准工时(分钟)
   */
  stdMinutes?: number;

  /**
   * 标准产能
   */
  stdCapacity?: number;

  /**
   * 状态
   */
  status?: number;

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
 * 创建AssyOutput DTO
 * 对应前端 AssyOutputCreate
 * @description 对应后端 TaktAssyOutputCreateDto
 */
export interface AssyOutputCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产
   */
  prodCategory: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产线
   */
  prodLine: string;

  /**
   * 直接人员
   */
  directLabor: number;

  /**
   * 间接人员
   */
  indirectLabor: number;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo: number;

  /**
   * 生产订单类型
   */
  prodOrderType?: string;

  /**
   * 生产工单号
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
  batchNo?: string;

  /**
   * 订单数量
   */
  prodOrderQty: number;

  /**
   * 标准工时(分钟)
   */
  stdMinutes: number;

  /**
   * 标准产能
   */
  stdCapacity: number;

  /**
   * 状态
   */
  status: number;

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
 * 更新AssyOutput DTO
 * 继承 TaktAssyOutputCreateDto，添加 AssyOutputId 字段
 * 对应前端 AssyOutputUpdate
 * @description 对应后端 TaktAssyOutputUpdateDto
 */
export interface AssyOutputUpdate extends AssyOutputCreate {
  /**
   * AssyOutputID（标识要更新的实体）
   */
  assyOutputId: string;

}


/**
 * AssyOutput 状态更新 DTO
 * 对应前端 AssyOutputStatus
 * @description 对应后端 TaktAssyOutputStatusDto
 */
export interface AssyOutputStatus {
  /**
   * AssyOutputID
   */
  assyOutputId: string;

  /**
   * 状态
   */
  status: number;

}


/**
 * AssyOutput 导入模板行 DTO
 * 对应前端 AssyOutputTemplate
 * @description 对应后端 TaktAssyOutputTemplateDto
 */
export interface AssyOutputTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产
   */
  prodCategory?: string;

  /**
   * 生产线
   */
  prodLine?: string;

  /**
   * 直接人员
   */
  directLabor?: number;

  /**
   * 间接人员
   */
  indirectLabor?: number;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo?: number;

  /**
   * 生产订单类型
   */
  prodOrderType?: string;

  /**
   * 生产工单号
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
  batchNo?: string;

  /**
   * 状态
   */
  status?: number;

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
 * AssyOutput 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssyOutputImport
 * @description 对应后端 TaktAssyOutputImportDto
 */
export interface AssyOutputImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产
   */
  prodCategory?: string;

  /**
   * 生产线
   */
  prodLine?: string;

  /**
   * 直接人员
   */
  directLabor?: number;

  /**
   * 间接人员
   */
  indirectLabor?: number;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo?: number;

  /**
   * 生产订单类型
   */
  prodOrderType?: string;

  /**
   * 生产工单号
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
  batchNo?: string;

  /**
   * 状态
   */
  status?: number;

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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 生产类别 RD: 研发 EVT: 工程验证测试 DVT: 设计验证测试 EPP: 工程试产 PP: 试产 FPP: 正式生产 MP: 大规模生产 RPR: 维修生产 RWR: 返工生产
   */
  prodCategory: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产线
   */
  prodLine: string;

  /**
   * 直接人员
   */
  directLabor: number;

  /**
   * 间接人员
   */
  indirectLabor: number;

  /**
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo: number;

  /**
   * 生产订单类型
   */
  prodOrderType?: string;

  /**
   * 生产工单号
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
  batchNo?: string;

  /**
   * 订单数量
   */
  prodOrderQty: number;

  /**
   * 标准工时(分钟)
   */
  stdMinutes: number;

  /**
   * 标准产能
   */
  stdCapacity: number;

  /**
   * 状态
   */
  status: number;

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

