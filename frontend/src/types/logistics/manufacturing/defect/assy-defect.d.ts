// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：assy-defect.d.ts
// 创建时间：2026-06-10
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
 * 组立不良日报实体
 * 对应前端 TaktAssyDefectDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyDefect
 * @description 对应后端 TaktAssyDefectDto
 */
export interface AssyDefect extends CompanyDtoBase {
  /**
   * AssyDefectID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assyDefectId: string;

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
   * 生产订单号
   */
  prodOrderCode: string;

  /**
   * 生产订单数量
   */
  prodOrderQty: number;

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
   * 生实实绩
   */
  prodActualQty: number;

  /**
   * 无不良数量
   */
  goodQuantity: number;

  /**
   * 状态(0=正常 1=停用)
   */
  status: number;

  /**
   * 组立不良明细列表 （子表：TaktAssyDefectDetail）
   */
  assyDefectDetails?: AssyDefectDetail[];

}


/**
 * AssyDefect 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssyDefectQuery
 * @description 对应后端 TaktAssyDefectQueryDto
 */
export interface AssyDefectQuery extends TaktPagedQuery {
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
   * 生产订单号
   */
  prodOrderCode?: string;

  /**
   * 生产订单数量
   */
  prodOrderQty?: number;

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
   * 生实实绩
   */
  prodActualQty?: number;

  /**
   * 无不良数量
   */
  goodQuantity?: number;

  /**
   * 状态(0=正常 1=停用)
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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建AssyDefect DTO
 * 对应前端 AssyDefectCreate
 * @description 对应后端 TaktAssyDefectCreateDto
 */
export interface AssyDefectCreate {
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
   * 生产订单号
   */
  prodOrderCode: string;

  /**
   * 生产订单数量
   */
  prodOrderQty: number;

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
   * 生实实绩
   */
  prodActualQty: number;

  /**
   * 无不良数量
   */
  goodQuantity: number;

  /**
   * 状态(0=正常 1=停用)
   */
  status: number;

  /**
   * 组立不良明细列表（子表，级联保存）
   */
  assyDefectDetails?: AssyDefectDetailCreate[];

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
 * 更新AssyDefect DTO
 * 继承 TaktAssyDefectCreateDto，添加 AssyDefectId 字段
 * 对应前端 AssyDefectUpdate
 * @description 对应后端 TaktAssyDefectUpdateDto
 */
export interface AssyDefectUpdate extends AssyDefectCreate {
  /**
   * AssyDefectID（标识要更新的实体）
   */
  assyDefectId: string;

}


/**
 * AssyDefect 状态更新 DTO
 * 对应前端 AssyDefectStatus
 * @description 对应后端 TaktAssyDefectStatusDto
 */
export interface AssyDefectStatus {
  /**
   * AssyDefectID
   */
  assyDefectId: string;

  /**
   * 状态(0=正常 1=停用)
   */
  status: number;

}


/**
 * AssyDefect 导入模板行 DTO
 * 对应前端 AssyDefectTemplate
 * @description 对应后端 TaktAssyDefectTemplateDto
 */
export interface AssyDefectTemplate {
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
   * 生产订单号
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
   * 状态(0=正常 1=停用)
   */
  status?: number;

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
 * AssyDefect 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssyDefectImport
 * @description 对应后端 TaktAssyDefectImportDto
 */
export interface AssyDefectImport {
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
   * 生产订单号
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
   * 状态(0=正常 1=停用)
   */
  status?: number;

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
   * 生产订单号
   */
  prodOrderCode: string;

  /**
   * 生产订单数量
   */
  prodOrderQty: number;

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
   * 生实实绩
   */
  prodActualQty: number;

  /**
   * 无不良数量
   */
  goodQuantity: number;

  /**
   * 状态(0=正常 1=停用)
   */
  status: number;

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

