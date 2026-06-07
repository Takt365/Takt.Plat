// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/controlling
// 文件名称：cost-center-change-log.d.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/controlling 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 成本中心变更记录实体
 * 对应前端 TaktCostCenterChangeLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CostCenterChangeLog
 * @description 对应后端 TaktCostCenterChangeLogDto
 */
export interface CostCenterChangeLog extends CompanyDtoBase {
  /**
   * CostCenterChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  costCenterChangeLogId: string;

  /**
   * 成本中心 ID
   */
  costCenterId: string;

  /**
   * 成本中心 名称（填充字段）
   */
  costCenterName?: string;

  /**
   * 成本中心编码（冗余）
   */
  costCenterCode: string;

  /**
   * 变更字段列表 JSON
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

}


/**
 * CostCenterChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CostCenterChangeLogQuery
 * @description 对应后端 TaktCostCenterChangeLogQueryDto
 */
export interface CostCenterChangeLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 成本中心 ID
   */
  costCenterId?: string;

  /**
   * 成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 变更字段列表 JSON
   */
  changeFields?: string;

  /**
   * 变更时间（范围查询-开始）
   */
  changeTimeStart?: string;

  /**
   * 变更时间（范围查询-结束）
   */
  changeTimeEnd?: string;

  /**
   * 变更人
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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
 * 创建CostCenterChangeLog DTO
 * 对应前端 CostCenterChangeLogCreate
 * @description 对应后端 TaktCostCenterChangeLogCreateDto
 */
export interface CostCenterChangeLogCreate {
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
   * 成本中心 ID
   */
  costCenterId: string;

  /**
   * 成本中心编码（冗余）
   */
  costCenterCode: string;

  /**
   * 变更字段列表 JSON
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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
 * 更新CostCenterChangeLog DTO
 * 继承 TaktCostCenterChangeLogCreateDto，添加 CostCenterChangeLogId 字段
 * 对应前端 CostCenterChangeLogUpdate
 * @description 对应后端 TaktCostCenterChangeLogUpdateDto
 */
export interface CostCenterChangeLogUpdate extends CostCenterChangeLogCreate {
  /**
   * CostCenterChangeLogID（标识要更新的实体）
   */
  costCenterChangeLogId: string;

}


/**
 * CostCenterChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CostCenterChangeLogExport
 * @description 对应后端 TaktCostCenterChangeLogExportDto
 */
export interface CostCenterChangeLogExport {
  /**
   * CostCenterChangeLogID
   */
  costCenterChangeLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 成本中心 ID
   */
  costCenterId: string;

  /**
   * 成本中心编码（冗余）
   */
  costCenterCode: string;

  /**
   * 变更字段列表 JSON
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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

