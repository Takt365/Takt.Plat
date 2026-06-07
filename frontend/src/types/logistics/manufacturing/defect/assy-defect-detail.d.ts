// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：assy-defect-detail.d.ts
// 创建时间：2026-06-07
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
 * 组立不良明细实体
 * 对应前端 TaktAssyDefectDetailDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyDefectDetail
 * @description 对应后端 TaktAssyDefectDetailDto
 */
export interface AssyDefectDetail extends CompanyDtoBase {
  /**
   * AssyDefectDetailID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assyDefectDetailId: string;

  /**
   * 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyDefectId: string;

  /**
   * 组立不良日报名称（填充字段）
   */
  assyDefectName?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 不良区分
   */
  defectCategory?: string;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 累计不良
   */
  cumulativeDefectQty: number;

  /**
   * 随机卡号
   */
  randomCardNo?: string;

  /**
   * 发生工程
   */
  occurrenceEngineering?: string;

  /**
   * 测试步骤
   */
  testStep?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 不良个所
   */
  defectLocation?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 修理员
   */
  repairOperator?: string;

  /**
   * 组立不良日报（主表） （主表：TaktAssyDefect）
   */
  assyDefect?: AssyDefect;

}


/**
 * AssyDefectDetail 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssyDefectDetailQuery
 * @description 对应后端 TaktAssyDefectDetailQueryDto
 */
export interface AssyDefectDetailQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyDefectId?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 不良区分
   */
  defectCategory?: string;

  /**
   * 不良数量
   */
  defectQty?: number;

  /**
   * 累计不良
   */
  cumulativeDefectQty?: number;

  /**
   * 随机卡号
   */
  randomCardNo?: string;

  /**
   * 发生工程
   */
  occurrenceEngineering?: string;

  /**
   * 测试步骤
   */
  testStep?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 不良个所
   */
  defectLocation?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 修理员
   */
  repairOperator?: string;

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
 * 创建AssyDefectDetail DTO
 * 对应前端 AssyDefectDetailCreate
 * @description 对应后端 TaktAssyDefectDetailCreateDto
 */
export interface AssyDefectDetailCreate {
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
   * 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyDefectId: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 不良区分
   */
  defectCategory?: string;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 累计不良
   */
  cumulativeDefectQty: number;

  /**
   * 随机卡号
   */
  randomCardNo?: string;

  /**
   * 发生工程
   */
  occurrenceEngineering?: string;

  /**
   * 测试步骤
   */
  testStep?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 不良个所
   */
  defectLocation?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 修理员
   */
  repairOperator?: string;

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
 * 更新AssyDefectDetail DTO
 * 继承 TaktAssyDefectDetailCreateDto，添加 AssyDefectDetailId 字段
 * 对应前端 AssyDefectDetailUpdate
 * @description 对应后端 TaktAssyDefectDetailUpdateDto
 */
export interface AssyDefectDetailUpdate extends AssyDefectDetailCreate {
  /**
   * AssyDefectDetailID（标识要更新的实体）
   */
  assyDefectDetailId: string;

}


/**
 * AssyDefectDetail 导入模板行 DTO
 * 对应前端 AssyDefectDetailTemplate
 * @description 对应后端 TaktAssyDefectDetailTemplateDto
 */
export interface AssyDefectDetailTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyDefectId?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 不良区分
   */
  defectCategory?: string;

  /**
   * 随机卡号
   */
  randomCardNo?: string;

  /**
   * 发生工程
   */
  occurrenceEngineering?: string;

  /**
   * 测试步骤
   */
  testStep?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 不良个所
   */
  defectLocation?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 修理员
   */
  repairOperator?: string;

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
 * AssyDefectDetail 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssyDefectDetailImport
 * @description 对应后端 TaktAssyDefectDetailImportDto
 */
export interface AssyDefectDetailImport {
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
   * 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyDefectId?: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 不良区分
   */
  defectCategory?: string;

  /**
   * 随机卡号
   */
  randomCardNo?: string;

  /**
   * 发生工程
   */
  occurrenceEngineering?: string;

  /**
   * 测试步骤
   */
  testStep?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 不良个所
   */
  defectLocation?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 修理员
   */
  repairOperator?: string;

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
 * AssyDefectDetail 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssyDefectDetailExport
 * @description 对应后端 TaktAssyDefectDetailExportDto
 */
export interface AssyDefectDetailExport {
  /**
   * AssyDefectDetailID
   */
  assyDefectDetailId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 组立不良日报ID（主表主键,序列化为string以避免Javascript精度问题）
   */
  assyDefectId: string;

  /**
   * 生产工单号（冗余字段,便于查询）
   */
  prodOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 不良区分
   */
  defectCategory?: string;

  /**
   * 不良数量
   */
  defectQty: number;

  /**
   * 累计不良
   */
  cumulativeDefectQty: number;

  /**
   * 随机卡号
   */
  randomCardNo?: string;

  /**
   * 发生工程
   */
  occurrenceEngineering?: string;

  /**
   * 测试步骤
   */
  testStep?: string;

  /**
   * 不良症状
   */
  defectSymptom?: string;

  /**
   * 不良个所
   */
  defectLocation?: string;

  /**
   * 不良原因
   */
  defectReason?: string;

  /**
   * 修理员
   */
  repairOperator?: string;

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

