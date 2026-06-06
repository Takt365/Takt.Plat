// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：assy-output-detail.d.ts
// 创建时间：2026-06-06
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
 * 组立日报明细（产出子表）实体
 * 对应前端 TaktAssyOutputDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyOutputDetail
 * @description 对应后端 TaktAssyOutputDetailDto
 */
export interface AssyOutputDetail extends CompanyDtoBase {
  /**
   * AssyOutputDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assyOutputDetailId: string;

  /**
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId: string;

  /**
   * 组立日报名称（填充字段）
   */
  assyOutputName?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产时段
   */
  timePeriod: string;

  /**
   * 实际生产数量
   */
  prodActualQty: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes: number;

  /**
   * 停线原因
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 投入工时(分钟)
   */
  inputMinutes: number;

  /**
   * 生产工时(分钟)
   */
  prodMinutes: number;

  /**
   * 实际工时(分钟)
   */
  actualMinutes: number;

  /**
   * 达成率(%)
   */
  achievementRate: number;

  /**
   * 组立日报（主表） （主表：TaktAssyOutput）
   */
  assyOutput?: AssyOutput;

}


/**
 * AssyOutputDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssyOutputDetailQuery
 * @description 对应后端 TaktAssyOutputDetailQueryDto
 */
export interface AssyOutputDetailQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产时段
   */
  timePeriod?: string;

  /**
   * 实际生产数量
   */
  prodActualQty?: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes?: number;

  /**
   * 停线原因
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 投入工时(分钟)
   */
  inputMinutes?: number;

  /**
   * 生产工时(分钟)
   */
  prodMinutes?: number;

  /**
   * 实际工时(分钟)
   */
  actualMinutes?: number;

  /**
   * 达成率(%)
   */
  achievementRate?: number;

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
 * 创建AssyOutputDetail DTO
 * 对应前端 AssyOutputDetailCreate
 * @description 对应后端 TaktAssyOutputDetailCreateDto
 */
export interface AssyOutputDetailCreate {
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
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产时段
   */
  timePeriod: string;

  /**
   * 实际生产数量
   */
  prodActualQty: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes: number;

  /**
   * 停线原因
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 投入工时(分钟)
   */
  inputMinutes: number;

  /**
   * 生产工时(分钟)
   */
  prodMinutes: number;

  /**
   * 实际工时(分钟)
   */
  actualMinutes: number;

  /**
   * 达成率(%)
   */
  achievementRate: number;

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
 * 更新AssyOutputDetail DTO
 * 继承 TaktAssyOutputDetailCreateDto，添加 AssyOutputDetailId 字段
 * 对应前端 AssyOutputDetailUpdate
 * @description 对应后端 TaktAssyOutputDetailUpdateDto
 */
export interface AssyOutputDetailUpdate extends AssyOutputDetailCreate {
  /**
   * AssyOutputDetailID（标识要更新的实体）
   */
  assyOutputDetailId: string;

}


/**
 * AssyOutputDetail 导入模板行 DTO
 * 对应前端 AssyOutputDetailTemplate
 * @description 对应后端 TaktAssyOutputDetailTemplateDto
 */
export interface AssyOutputDetailTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产时段
   */
  timePeriod?: string;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes?: number;

  /**
   * 停线原因
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

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
 * AssyOutputDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssyOutputDetailImport
 * @description 对应后端 TaktAssyOutputDetailImportDto
 */
export interface AssyOutputDetailImport {
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
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产时段
   */
  timePeriod?: string;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes?: number;

  /**
   * 停线原因
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

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
 * AssyOutputDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssyOutputDetailExport
 * @description 对应后端 TaktAssyOutputDetailExportDto
 */
export interface AssyOutputDetailExport {
  /**
   * AssyOutputDetailID
   */
  assyOutputDetailId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 组立日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyOutputId: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产时段
   */
  timePeriod: string;

  /**
   * 实际生产数量
   */
  prodActualQty: number;

  /**
   * 停线时间(分钟)
   */
  downtimeMinutes: number;

  /**
   * 停线原因
   */
  downtimeReason?: string;

  /**
   * 停线说明
   */
  downtimeDescription?: string;

  /**
   * 未达成原因
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * 投入工时(分钟)
   */
  inputMinutes: number;

  /**
   * 生产工时(分钟)
   */
  prodMinutes: number;

  /**
   * 实际工时(分钟)
   */
  actualMinutes: number;

  /**
   * 达成率(%)
   */
  achievementRate: number;

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

