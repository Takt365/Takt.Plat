// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/labor-hour
// 文件名称：pcba-smt-labor-hour.d.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/labor-hour 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * PCBA SMT工数统计实体
 * 对应前端 TaktPcbaSmtLaborHourDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PcbaSmtLaborHour
 * @description 对应后端 TaktPcbaSmtLaborHourDto
 */
export interface PcbaSmtLaborHour extends CompanyDtoBase {
  /**
   * PcbaSmtLaborHourID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  pcbaSmtLaborHourId: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
   */
  stdCapacity: number;

  /**
   * 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
   */
  prodActualQty: number;

  /**
   * 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
   */
  inputMinutes: number;

  /**
   * 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
   */
  downtimeMinutes: number;

  /**
   * 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
   */
  confirmMinutes: number;

  /**
   * 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
   */
  actualMinutes: number;

}


/**
 * PcbaSmtLaborHour 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PcbaSmtLaborHourQuery
 * @description 对应后端 TaktPcbaSmtLaborHourQueryDto
 */
export interface PcbaSmtLaborHourQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 生产日期（范围查询-开始）
   */
  prodDateStart?: string;

  /**
   * 生产日期（范围查询-结束）
   */
  prodDateEnd?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam?: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
   */
  stdCapacity?: number;

  /**
   * 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
   */
  prodActualQty?: number;

  /**
   * 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
   */
  inputMinutes?: number;

  /**
   * 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
   */
  downtimeMinutes?: number;

  /**
   * 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
   */
  confirmMinutes?: number;

  /**
   * 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
   */
  actualMinutes?: number;

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
 * 创建PcbaSmtLaborHour DTO
 * 对应前端 PcbaSmtLaborHourCreate
 * @description 对应后端 TaktPcbaSmtLaborHourCreateDto
 */
export interface PcbaSmtLaborHourCreate {
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
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
   */
  stdCapacity: number;

  /**
   * 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
   */
  prodActualQty: number;

  /**
   * 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
   */
  inputMinutes: number;

  /**
   * 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
   */
  downtimeMinutes: number;

  /**
   * 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
   */
  confirmMinutes: number;

  /**
   * 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
   */
  actualMinutes: number;

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
 * 更新PcbaSmtLaborHour DTO
 * 继承 TaktPcbaSmtLaborHourCreateDto，添加 PcbaSmtLaborHourId 字段
 * 对应前端 PcbaSmtLaborHourUpdate
 * @description 对应后端 TaktPcbaSmtLaborHourUpdateDto
 */
export interface PcbaSmtLaborHourUpdate extends PcbaSmtLaborHourCreate {
  /**
   * PcbaSmtLaborHourID（标识要更新的实体）
   */
  pcbaSmtLaborHourId: string;

}


/**
 * PcbaSmtLaborHour 导入模板行 DTO
 * 对应前端 PcbaSmtLaborHourTemplate
 * @description 对应后端 TaktPcbaSmtLaborHourTemplateDto
 */
export interface PcbaSmtLaborHourTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 生产日期
   */
  prodDate?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam?: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
   */
  stdCapacity?: number;

  /**
   * 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
   */
  prodActualQty?: number;

  /**
   * 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
   */
  inputMinutes?: number;

  /**
   * 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
   */
  downtimeMinutes?: number;

  /**
   * 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
   */
  confirmMinutes?: number;

  /**
   * 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
   */
  actualMinutes?: number;

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
 * PcbaSmtLaborHour 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PcbaSmtLaborHourImport
 * @description 对应后端 TaktPcbaSmtLaborHourImportDto
 */
export interface PcbaSmtLaborHourImport {
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
   * 生产日期
   */
  prodDate?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam?: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
   */
  stdCapacity?: number;

  /**
   * 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
   */
  prodActualQty?: number;

  /**
   * 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
   */
  inputMinutes?: number;

  /**
   * 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
   */
  downtimeMinutes?: number;

  /**
   * 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
   */
  confirmMinutes?: number;

  /**
   * 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
   */
  actualMinutes?: number;

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
 * PcbaSmtLaborHour 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PcbaSmtLaborHourExport
 * @description 对应后端 TaktPcbaSmtLaborHourExportDto
 */
export interface PcbaSmtLaborHourExport {
  /**
   * PcbaSmtLaborHourID
   */
  pcbaSmtLaborHourId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  prodTeam: string;

  /**
   * 班次（字典 logistics_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
   */
  stdCapacity: number;

  /**
   * 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
   */
  prodActualQty: number;

  /**
   * 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
   */
  inputMinutes: number;

  /**
   * 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
   */
  downtimeMinutes: number;

  /**
   * 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
   */
  confirmMinutes: number;

  /**
   * 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
   */
  actualMinutes: number;

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

