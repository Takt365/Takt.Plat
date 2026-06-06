// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ec-dept.d.ts
// 创建时间：2026-06-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 设变-部门通用实体。部门顺序（严格）：技术(Eng)、生管(Pmc)、采购(Mp)、Iqc、部管(Mc)、制二(Pcba)、制一(Assy)、Qa、制技(Te)。通过 DeptCode 区分。
 * 对应前端 TaktEcDeptDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcDept
 * @description 对应后端 TaktEcDeptDto
 */
export interface EcDept extends CompanyDtoBase {
  /**
   * EcDeptID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ecDeptId: string;

  /**
   * 设变明细ID（TaktEcDetail 主键）
   */
  ecnDetailId: string;

  /**
   * 设变明细名称（填充字段）
   */
  ecnDetailName?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
   */
  deptCode: string;

  /**
   * 是否实施（0=否 1=是）
   */
  isImplemented: number;

  /**
   * 内容（各部门通用）
   */
  content?: string;

  /**
   * 预计生产日期
   */
  scheduledProductionDate?: string;

  /**
   * 预定批次
   */
  scheduledBatch?: string;

  /**
   * Po残（采购订单残）
   */
  poRemainder?: string;

  /**
   * 结余
   */
  balance?: string;

  /**
   * 旧品处理
   */
  oldProductHandling?: string;

  /**
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderNo?: string;

  /**
   * 受检单号
   */
  iqcOrderNo?: string;

  /**
   * 检验/检查日期
   */
  inspectionDate?: string;

  /**
   * 出库批次
   */
  outboundBatch?: string;

  /**
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 出库单号
   */
  outboundOrderNo?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 实施日期
   */
  implementationDate?: string;

  /**
   * 检验批次
   */
  inspectionBatch?: string;

  /**
   * 抽样号码
   */
  samplingNo?: string;

  /**
   * 是否更新SOP（0=否 1=是）
   */
  isSopUpdated: number;

  /**
   * 设变明细（多对一） （主表：TaktEcDetail）
   */
  ecnDetail?: EcDetail;

}


/**
 * EcDept 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcDeptQuery
 * @description 对应后端 TaktEcDeptQueryDto
 */
export interface EcDeptQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 设变明细ID（TaktEcDetail 主键）
   */
  ecnDetailId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
   */
  deptCode?: string;

  /**
   * 是否实施（0=否 1=是）
   */
  isImplemented?: number;

  /**
   * 内容（各部门通用）
   */
  content?: string;

  /**
   * 预计生产日期（范围查询-开始）
   */
  scheduledProductionDateStart?: string;

  /**
   * 预计生产日期（范围查询-结束）
   */
  scheduledProductionDateEnd?: string;

  /**
   * 预定批次
   */
  scheduledBatch?: string;

  /**
   * Po残（采购订单残）
   */
  poRemainder?: string;

  /**
   * 结余
   */
  balance?: string;

  /**
   * 旧品处理
   */
  oldProductHandling?: string;

  /**
   * 采购订单发行日期（范围查询-开始）
   */
  purchaseOrderIssueDateStart?: string;

  /**
   * 采购订单发行日期（范围查询-结束）
   */
  purchaseOrderIssueDateEnd?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderNo?: string;

  /**
   * 受检单号
   */
  iqcOrderNo?: string;

  /**
   * 检验/检查日期（范围查询-开始）
   */
  inspectionDateStart?: string;

  /**
   * 检验/检查日期（范围查询-结束）
   */
  inspectionDateEnd?: string;

  /**
   * 出库批次
   */
  outboundBatch?: string;

  /**
   * 出库日期（范围查询-开始）
   */
  outboundDateStart?: string;

  /**
   * 出库日期（范围查询-结束）
   */
  outboundDateEnd?: string;

  /**
   * 生产日期（范围查询-开始）
   */
  productionDateStart?: string;

  /**
   * 生产日期（范围查询-结束）
   */
  productionDateEnd?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 出库单号
   */
  outboundOrderNo?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 实施日期（范围查询-开始）
   */
  implementationDateStart?: string;

  /**
   * 实施日期（范围查询-结束）
   */
  implementationDateEnd?: string;

  /**
   * 检验批次
   */
  inspectionBatch?: string;

  /**
   * 抽样号码
   */
  samplingNo?: string;

  /**
   * 是否更新SOP（0=否 1=是）
   */
  isSopUpdated?: number;

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
 * 创建EcDept DTO
 * 对应前端 EcDeptCreate
 * @description 对应后端 TaktEcDeptCreateDto
 */
export interface EcDeptCreate {
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
   * 设变明细ID（TaktEcDetail 主键）
   */
  ecnDetailId: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
   */
  deptCode: string;

  /**
   * 是否实施（0=否 1=是）
   */
  isImplemented: number;

  /**
   * 内容（各部门通用）
   */
  content?: string;

  /**
   * 预计生产日期
   */
  scheduledProductionDate?: string;

  /**
   * 预定批次
   */
  scheduledBatch?: string;

  /**
   * Po残（采购订单残）
   */
  poRemainder?: string;

  /**
   * 结余
   */
  balance?: string;

  /**
   * 旧品处理
   */
  oldProductHandling?: string;

  /**
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderNo?: string;

  /**
   * 受检单号
   */
  iqcOrderNo?: string;

  /**
   * 检验/检查日期
   */
  inspectionDate?: string;

  /**
   * 出库批次
   */
  outboundBatch?: string;

  /**
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 出库单号
   */
  outboundOrderNo?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 实施日期
   */
  implementationDate?: string;

  /**
   * 检验批次
   */
  inspectionBatch?: string;

  /**
   * 抽样号码
   */
  samplingNo?: string;

  /**
   * 是否更新SOP（0=否 1=是）
   */
  isSopUpdated: number;

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
 * 更新EcDept DTO
 * 继承 TaktEcDeptCreateDto，添加 EcDeptId 字段
 * 对应前端 EcDeptUpdate
 * @description 对应后端 TaktEcDeptUpdateDto
 */
export interface EcDeptUpdate extends EcDeptCreate {
  /**
   * EcDeptID（标识要更新的实体）
   */
  ecDeptId: string;

}


/**
 * EcDept 导入模板行 DTO
 * 对应前端 EcDeptTemplate
 * @description 对应后端 TaktEcDeptTemplateDto
 */
export interface EcDeptTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 设变明细ID（TaktEcDetail 主键）
   */
  ecnDetailId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
   */
  deptCode?: string;

  /**
   * 是否实施（0=否 1=是）
   */
  isImplemented?: number;

  /**
   * 内容（各部门通用）
   */
  content?: string;

  /**
   * 预定批次
   */
  scheduledBatch?: string;

  /**
   * Po残（采购订单残）
   */
  poRemainder?: string;

  /**
   * 结余
   */
  balance?: string;

  /**
   * 旧品处理
   */
  oldProductHandling?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderNo?: string;

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
 * EcDept 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcDeptImport
 * @description 对应后端 TaktEcDeptImportDto
 */
export interface EcDeptImport {
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
   * 设变明细ID（TaktEcDetail 主键）
   */
  ecnDetailId?: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
   */
  deptCode?: string;

  /**
   * 是否实施（0=否 1=是）
   */
  isImplemented?: number;

  /**
   * 内容（各部门通用）
   */
  content?: string;

  /**
   * 预定批次
   */
  scheduledBatch?: string;

  /**
   * Po残（采购订单残）
   */
  poRemainder?: string;

  /**
   * 结余
   */
  balance?: string;

  /**
   * 旧品处理
   */
  oldProductHandling?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderNo?: string;

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
 * EcDept 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcDeptExport
 * @description 对应后端 TaktEcDeptExportDto
 */
export interface EcDeptExport {
  /**
   * EcDeptID
   */
  ecDeptId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 设变明细ID（TaktEcDetail 主键）
   */
  ecnDetailId: string;

  /**
   * 设变单号（冗余字段,便于查询）
   */
  ecNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 部门编码。顺序严格为：Eng=技术, Pmc=生管, Mp=采购, Iqc=受检, Mc=部管, Pcba=制二, Assy=制一, Qa=品管, Te=制技。
   */
  deptCode: string;

  /**
   * 是否实施（0=否 1=是）
   */
  isImplemented: number;

  /**
   * 内容（各部门通用）
   */
  content?: string;

  /**
   * 预计生产日期
   */
  scheduledProductionDate?: string;

  /**
   * 预定批次
   */
  scheduledBatch?: string;

  /**
   * Po残（采购订单残）
   */
  poRemainder?: string;

  /**
   * 结余
   */
  balance?: string;

  /**
   * 旧品处理
   */
  oldProductHandling?: string;

  /**
   * 采购订单发行日期
   */
  purchaseOrderIssueDate?: string;

  /**
   * 供应商
   */
  supplier?: string;

  /**
   * 采购订单号码
   */
  purchaseOrderNo?: string;

  /**
   * 受检单号
   */
  iqcOrderNo?: string;

  /**
   * 检验/检查日期
   */
  inspectionDate?: string;

  /**
   * 出库批次
   */
  outboundBatch?: string;

  /**
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 生产日期
   */
  productionDate?: string;

  /**
   * 生产批次
   */
  productionBatch?: string;

  /**
   * 出库单号
   */
  outboundOrderNo?: string;

  /**
   * 生产班组
   */
  productionTeam?: string;

  /**
   * 实施日期
   */
  implementationDate?: string;

  /**
   * 检验批次
   */
  inspectionBatch?: string;

  /**
   * 抽样号码
   */
  samplingNo?: string;

  /**
   * 是否更新SOP（0=否 1=是）
   */
  isSopUpdated: number;

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

