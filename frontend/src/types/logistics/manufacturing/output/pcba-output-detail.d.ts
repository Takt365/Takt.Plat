// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：pcba-output-detail.d.ts
// 创建时间：2026-06-09
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
 * PCBA明细实体
 * 对应前端 TaktPcbaOutputDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PcbaOutputDetail
 * @description 对应后端 TaktPcbaOutputDetailDto
 */
export interface PcbaOutputDetail extends CompanyDtoBase {
  /**
   * PcbaOutputDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  pcbaOutputDetailId: string;

  /**
   * PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaOutputId: string;

  /**
   * PCBA日报名称（填充字段）
   */
  pcbaOutputName?: string;

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
   * 班组
   */
  shiftNo: number;

  /**
   * 板别（PCB板别）
   */
  pcbBoardType: string;

  /**
   * 面板别
   */
  panelSide: string;

  /**
   * 批次数量
   */
  batchQty: number;

  /**
   * 当日完成数
   */
  dailyCompletedQty: number;

  /**
   * 累计完成数
   */
  totalCompletedQty: number;

  /**
   * 完成状态（0=未完成 1=部分完成 2=已完成）
   */
  completedStatus: number;

  /**
   * 序列号
   */
  serialNo: string;

  /**
   * 不良台数
   */
  defectCount: number;

  /**
   * 投入工数(分钟)
   */
  inputMinutes: number;

  /**
   * 修工数(分钟)
   */
  repairMinutes: number;

  /**
   * 切换次数
   */
  switchCount: number;

  /**
   * 切换时间(分钟)
   */
  switchTime: number;

  /**
   * 切停机时间(分钟)
   */
  stopTime: number;

  /**
   * 总工数(分钟)
   */
  totalMinutes: number;

  /**
   * 未达成原因
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

  /**
   * PCBA日报（主表） （主表：TaktPcbaOutput）
   */
  pcbaOutput?: PcbaOutput;

}


/**
 * PcbaOutputDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PcbaOutputDetailQuery
 * @description 对应后端 TaktPcbaOutputDetailQueryDto
 */
export interface PcbaOutputDetailQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaOutputId?: string;

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
   * 班组
   */
  shiftNo?: number;

  /**
   * 板别（PCB板别）
   */
  pcbBoardType?: string;

  /**
   * 面板别
   */
  panelSide?: string;

  /**
   * 批次数量
   */
  batchQty?: number;

  /**
   * 当日完成数
   */
  dailyCompletedQty?: number;

  /**
   * 累计完成数
   */
  totalCompletedQty?: number;

  /**
   * 完成状态（0=未完成 1=部分完成 2=已完成）
   */
  completedStatus?: number;

  /**
   * 序列号
   */
  serialNo?: string;

  /**
   * 不良台数
   */
  defectCount?: number;

  /**
   * 投入工数(分钟)
   */
  inputMinutes?: number;

  /**
   * 修工数(分钟)
   */
  repairMinutes?: number;

  /**
   * 切换次数
   */
  switchCount?: number;

  /**
   * 切换时间(分钟)
   */
  switchTime?: number;

  /**
   * 切停机时间(分钟)
   */
  stopTime?: number;

  /**
   * 总工数(分钟)
   */
  totalMinutes?: number;

  /**
   * 未达成原因
   */
  unachievedReason?: string;

  /**
   * 未达成说明
   */
  unachievedDescription?: string;

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
 * 创建PcbaOutputDetail DTO
 * 对应前端 PcbaOutputDetailCreate
 * @description 对应后端 TaktPcbaOutputDetailCreateDto
 */
export interface PcbaOutputDetailCreate {
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
   * PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaOutputId: string;

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
   * 班组
   */
  shiftNo: number;

  /**
   * 板别（PCB板别）
   */
  pcbBoardType: string;

  /**
   * 面板别
   */
  panelSide: string;

  /**
   * 批次数量
   */
  batchQty: number;

  /**
   * 当日完成数
   */
  dailyCompletedQty: number;

  /**
   * 累计完成数
   */
  totalCompletedQty: number;

  /**
   * 完成状态（0=未完成 1=部分完成 2=已完成）
   */
  completedStatus: number;

  /**
   * 序列号
   */
  serialNo: string;

  /**
   * 不良台数
   */
  defectCount: number;

  /**
   * 投入工数(分钟)
   */
  inputMinutes: number;

  /**
   * 修工数(分钟)
   */
  repairMinutes: number;

  /**
   * 切换次数
   */
  switchCount: number;

  /**
   * 切换时间(分钟)
   */
  switchTime: number;

  /**
   * 切停机时间(分钟)
   */
  stopTime: number;

  /**
   * 总工数(分钟)
   */
  totalMinutes: number;

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
 * 更新PcbaOutputDetail DTO
 * 继承 TaktPcbaOutputDetailCreateDto，添加 PcbaOutputDetailId 字段
 * 对应前端 PcbaOutputDetailUpdate
 * @description 对应后端 TaktPcbaOutputDetailUpdateDto
 */
export interface PcbaOutputDetailUpdate extends PcbaOutputDetailCreate {
  /**
   * PcbaOutputDetailID（标识要更新的实体）
   */
  pcbaOutputDetailId: string;

}


/**
 * PcbaOutputDetail 状态更新 DTO
 * 对应前端 PcbaOutputDetailStatus
 * @description 对应后端 TaktPcbaOutputDetailStatusDto
 */
export interface PcbaOutputDetailStatus {
  /**
   * PcbaOutputDetailID
   */
  pcbaOutputDetailId: string;

  /**
   * 完成状态（0=未完成 1=部分完成 2=已完成）
   */
  completedStatus: number;

}


/**
 * PcbaOutputDetail 导入模板行 DTO
 * 对应前端 PcbaOutputDetailTemplate
 * @description 对应后端 TaktPcbaOutputDetailTemplateDto
 */
export interface PcbaOutputDetailTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaOutputId?: string;

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
   * 班组
   */
  shiftNo?: number;

  /**
   * 板别（PCB板别）
   */
  pcbBoardType?: string;

  /**
   * 面板别
   */
  panelSide?: string;

  /**
   * 完成状态（0=未完成 1=部分完成 2=已完成）
   */
  completedStatus?: number;

  /**
   * 序列号
   */
  serialNo?: string;

  /**
   * 不良台数
   */
  defectCount?: number;

  /**
   * 切换次数
   */
  switchCount?: number;

  /**
   * 未达成原因
   */
  unachievedReason?: string;

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
 * PcbaOutputDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PcbaOutputDetailImport
 * @description 对应后端 TaktPcbaOutputDetailImportDto
 */
export interface PcbaOutputDetailImport {
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
   * PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaOutputId?: string;

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
   * 班组
   */
  shiftNo?: number;

  /**
   * 板别（PCB板别）
   */
  pcbBoardType?: string;

  /**
   * 面板别
   */
  panelSide?: string;

  /**
   * 完成状态（0=未完成 1=部分完成 2=已完成）
   */
  completedStatus?: number;

  /**
   * 序列号
   */
  serialNo?: string;

  /**
   * 不良台数
   */
  defectCount?: number;

  /**
   * 切换次数
   */
  switchCount?: number;

  /**
   * 未达成原因
   */
  unachievedReason?: string;

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
 * PcbaOutputDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PcbaOutputDetailExport
 * @description 对应后端 TaktPcbaOutputDetailExportDto
 */
export interface PcbaOutputDetailExport {
  /**
   * PcbaOutputDetailID
   */
  pcbaOutputDetailId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * PCBA日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  pcbaOutputId: string;

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
   * 班组
   */
  shiftNo: number;

  /**
   * 板别（PCB板别）
   */
  pcbBoardType: string;

  /**
   * 面板别
   */
  panelSide: string;

  /**
   * 批次数量
   */
  batchQty: number;

  /**
   * 当日完成数
   */
  dailyCompletedQty: number;

  /**
   * 累计完成数
   */
  totalCompletedQty: number;

  /**
   * 完成状态（0=未完成 1=部分完成 2=已完成）
   */
  completedStatus: number;

  /**
   * 序列号
   */
  serialNo: string;

  /**
   * 不良台数
   */
  defectCount: number;

  /**
   * 投入工数(分钟)
   */
  inputMinutes: number;

  /**
   * 修工数(分钟)
   */
  repairMinutes: number;

  /**
   * 切换次数
   */
  switchCount: number;

  /**
   * 切换时间(分钟)
   */
  switchTime: number;

  /**
   * 切停机时间(分钟)
   */
  stopTime: number;

  /**
   * 总工数(分钟)
   */
  totalMinutes: number;

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

  /**
   * 创建时间
   */
  createdAt: string;

}

