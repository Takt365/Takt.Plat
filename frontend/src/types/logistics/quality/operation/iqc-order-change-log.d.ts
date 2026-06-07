// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：iqc-order-change-log.d.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * IQC进货检验单变更日志实体
 * 对应前端 TaktIqcOrderChangeLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 IqcOrderChangeLog
 * @description 对应后端 TaktIqcOrderChangeLogDto
 */
export interface IqcOrderChangeLog extends CompanyDtoBase {
  /**
   * IqcOrderChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  iqcOrderChangeLogId: string;

  /**
   * IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderId: string;

  /**
   * IQC检验单名称（填充字段）
   */
  iqcOrderName?: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更）
   */
  changeType: number;

  /**
   * 变更原因
   */
  changeReason?: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * IQC检验单（主表） （主表：TaktIqcOrder）
   */
  order?: IqcOrder;

}


/**
 * IqcOrderChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 IqcOrderChangeLogQuery
 * @description 对应后端 TaktIqcOrderChangeLogQueryDto
 */
export interface IqcOrderChangeLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderId?: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更）
   */
  changeType?: number;

  /**
   * 变更原因
   */
  changeReason?: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更时间（范围查询-开始）
   */
  changeTimeStart?: string;

  /**
   * 变更时间（范围查询-结束）
   */
  changeTimeEnd?: string;

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
 * 创建IqcOrderChangeLog DTO
 * 对应前端 IqcOrderChangeLogCreate
 * @description 对应后端 TaktIqcOrderChangeLogCreateDto
 */
export interface IqcOrderChangeLogCreate {
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
   * IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderId: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更）
   */
  changeType: number;

  /**
   * 变更原因
   */
  changeReason?: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更时间
   */
  changeTime: string;

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
 * 更新IqcOrderChangeLog DTO
 * 继承 TaktIqcOrderChangeLogCreateDto，添加 IqcOrderChangeLogId 字段
 * 对应前端 IqcOrderChangeLogUpdate
 * @description 对应后端 TaktIqcOrderChangeLogUpdateDto
 */
export interface IqcOrderChangeLogUpdate extends IqcOrderChangeLogCreate {
  /**
   * IqcOrderChangeLogID（标识要更新的实体）
   */
  iqcOrderChangeLogId: string;

}


/**
 * IqcOrderChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 IqcOrderChangeLogExport
 * @description 对应后端 TaktIqcOrderChangeLogExportDto
 */
export interface IqcOrderChangeLogExport {
  /**
   * IqcOrderChangeLogID
   */
  iqcOrderChangeLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderId: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更类型（0=新增，1=修改，2=删除，3=状态变更）
   */
  changeType: number;

  /**
   * 变更原因
   */
  changeReason?: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更时间
   */
  changeTime: string;

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

