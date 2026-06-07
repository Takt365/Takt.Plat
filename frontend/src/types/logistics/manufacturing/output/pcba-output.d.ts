// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：pcba-output.d.ts
// 创建时间：2026-06-07
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
 * PCBA日报实体
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
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo: number;

  /**
   * 生产工单号
   */
  prodOrderCode: string;

  /**
   * 机种
   */
  modelCode: string;

  /**
   * 批次
   */
  batchNo?: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 订单数量
   */
  prodOrderQty: number;

  /**
   * 标准工时(分钟)
   */
  stdMinutes: number;

  /**
   * 标准点数
   */
  stdShorts: number;

  /**
   * 标准产能
   */
  stdCapacity: number;

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
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo?: number;

  /**
   * 生产工单号
   */
  prodOrderCode?: string;

  /**
   * 机种
   */
  modelCode?: string;

  /**
   * 批次
   */
  batchNo?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 订单数量
   */
  prodOrderQty?: number;

  /**
   * 标准工时(分钟)
   */
  stdMinutes?: number;

  /**
   * 标准点数
   */
  stdShorts?: number;

  /**
   * 标准产能
   */
  stdCapacity?: number;

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
  extFieldJson?: string;

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
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo: number;

  /**
   * 生产工单号
   */
  prodOrderCode: string;

  /**
   * 机种
   */
  modelCode: string;

  /**
   * 批次
   */
  batchNo?: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 订单数量
   */
  prodOrderQty: number;

  /**
   * 标准工时(分钟)
   */
  stdMinutes: number;

  /**
   * 标准点数
   */
  stdShorts: number;

  /**
   * 标准产能
   */
  stdCapacity: number;

  /**
   * PCBA明细列表（子表，级联保存）
   */
  pcbaOutputDetails?: PcbaOutputDetailCreate[];

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

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
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo?: number;

  /**
   * 生产工单号
   */
  prodOrderCode?: string;

  /**
   * 机种
   */
  modelCode?: string;

  /**
   * 批次
   */
  batchNo?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 标准点数
   */
  stdShorts?: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

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
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo?: number;

  /**
   * 生产工单号
   */
  prodOrderCode?: string;

  /**
   * 机种
   */
  modelCode?: string;

  /**
   * 批次
   */
  batchNo?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 标准点数
   */
  stdShorts?: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

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
   * 班次(1=早班 2=中班 3=晚班)
   */
  shiftNo: number;

  /**
   * 生产工单号
   */
  prodOrderCode: string;

  /**
   * 机种
   */
  modelCode: string;

  /**
   * 批次
   */
  batchNo?: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 订单数量
   */
  prodOrderQty: number;

  /**
   * 标准工时(分钟)
   */
  stdMinutes: number;

  /**
   * 标准点数
   */
  stdShorts: number;

  /**
   * 标准产能
   */
  stdCapacity: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

