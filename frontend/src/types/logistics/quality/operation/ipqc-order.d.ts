// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：ipqc-order.d.ts
// 创建时间：2026-06-05
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
 * IPQC制程检验单实体
 * 对应前端 TaktIpqcOrderDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 IpqcOrder
 * @description 对应后端 TaktIpqcOrderDto
 */
export interface IpqcOrder extends CompanyDtoBase {
  /**
   * IpqcOrderID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  ipqcOrderId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 来源单号（生产工单编码）
   */
  sourceCode: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * IPQC检验单编码（唯一索引，根据来源单号自动生成）
   */
  ipqcOrderCode: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName: string;

  /**
   * 生产总数
   */
  totalProductionQuantity: number;

  /**
   * 总抽样数量（自动计算 = 各明细抽样数量合计）
   */
  totalSampleQuantity: number;

  /**
   * 总合格数量（自动计算 = 各明细合格数量合计）
   */
  totalQualifiedQuantity: number;

  /**
   * 总不合格数量（自动计算 = 各明细不合格数量合计）
   */
  totalUnqualifiedQuantity: number;

  /**
   * 总验退数量（自动计算 = 各明细验退数量合计）
   */
  totalInspectionReturnQuantity: number;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus: number;

  /**
   * 判定人（人员代码）
   */
  judgeBy?: string;

  /**
   * 判定日期
   */
  judgeDate?: string;

  /**
   * 判定说明
   */
  judgeDescription?: string;

  /**
   * IPQC检验单明细列表（主子表关系） （子表：TaktIpqcOrderItem）
   */
  items?: IpqcOrderItem[];

  /**
   * 变更日志列表（主子表关系） （子表：TaktIpqcOrderChangeLog）
   */
  changeLogs?: IpqcOrderChangeLog[];

}


/**
 * IpqcOrder 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 IpqcOrderQuery
 * @description 对应后端 TaktIpqcOrderQueryDto
 */
export interface IpqcOrderQuery extends TaktPagedQuery {
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
   * 来源单号（生产工单编码）
   */
  sourceCode?: string;

  /**
   * 检验日期（范围查询-开始）
   */
  inspectionDateStart?: string;

  /**
   * 检验日期（范围查询-结束）
   */
  inspectionDateEnd?: string;

  /**
   * IPQC检验单编码（唯一索引，根据来源单号自动生成）
   */
  ipqcOrderCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 生产总数
   */
  totalProductionQuantity?: number;

  /**
   * 总抽样数量（自动计算 = 各明细抽样数量合计）
   */
  totalSampleQuantity?: number;

  /**
   * 总合格数量（自动计算 = 各明细合格数量合计）
   */
  totalQualifiedQuantity?: number;

  /**
   * 总不合格数量（自动计算 = 各明细不合格数量合计）
   */
  totalUnqualifiedQuantity?: number;

  /**
   * 总验退数量（自动计算 = 各明细验退数量合计）
   */
  totalInspectionReturnQuantity?: number;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus?: number;

  /**
   * 判定人（人员代码）
   */
  judgeBy?: string;

  /**
   * 判定日期（范围查询-开始）
   */
  judgeDateStart?: string;

  /**
   * 判定日期（范围查询-结束）
   */
  judgeDateEnd?: string;

  /**
   * 判定说明
   */
  judgeDescription?: string;

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
 * 创建IpqcOrder DTO
 * 对应前端 IpqcOrderCreate
 * @description 对应后端 TaktIpqcOrderCreateDto
 */
export interface IpqcOrderCreate {
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
   * 来源单号（生产工单编码）
   */
  sourceCode: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * IPQC检验单编码（唯一索引，根据来源单号自动生成）
   */
  ipqcOrderCode: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName: string;

  /**
   * 生产总数
   */
  totalProductionQuantity: number;

  /**
   * 总抽样数量（自动计算 = 各明细抽样数量合计）
   */
  totalSampleQuantity: number;

  /**
   * 总合格数量（自动计算 = 各明细合格数量合计）
   */
  totalQualifiedQuantity: number;

  /**
   * 总不合格数量（自动计算 = 各明细不合格数量合计）
   */
  totalUnqualifiedQuantity: number;

  /**
   * 总验退数量（自动计算 = 各明细验退数量合计）
   */
  totalInspectionReturnQuantity: number;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus: number;

  /**
   * 判定人（人员代码）
   */
  judgeBy?: string;

  /**
   * 判定日期
   */
  judgeDate?: string;

  /**
   * 判定说明
   */
  judgeDescription?: string;

  /**
   * IPQC检验单明细列表（主子表关系）（子表，级联保存）
   */
  items?: IpqcOrderItemCreate[];

  /**
   * 变更日志列表（主子表关系）（子表，级联保存）
   */
  changeLogs?: IpqcOrderChangeLogCreate[];

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
 * 更新IpqcOrder DTO
 * 继承 TaktIpqcOrderCreateDto，添加 IpqcOrderId 字段
 * 对应前端 IpqcOrderUpdate
 * @description 对应后端 TaktIpqcOrderUpdateDto
 */
export interface IpqcOrderUpdate extends IpqcOrderCreate {
  /**
   * IpqcOrderID（标识要更新的实体）
   */
  ipqcOrderId: string;

}


/**
 * IpqcOrder 状态更新 DTO
 * 对应前端 IpqcOrderStatus
 * @description 对应后端 TaktIpqcOrderStatusDto
 */
export interface IpqcOrderStatus {
  /**
   * IpqcOrderID
   */
  ipqcOrderId: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus: number;

}


/**
 * IpqcOrder 导入模板行 DTO
 * 对应前端 IpqcOrderTemplate
 * @description 对应后端 TaktIpqcOrderTemplateDto
 */
export interface IpqcOrderTemplate {
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
   * 来源单号（生产工单编码）
   */
  sourceCode?: string;

  /**
   * IPQC检验单编码（唯一索引，根据来源单号自动生成）
   */
  ipqcOrderCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 总抽样数量（自动计算 = 各明细抽样数量合计）
   */
  totalSampleQuantity?: number;

  /**
   * 总合格数量（自动计算 = 各明细合格数量合计）
   */
  totalQualifiedQuantity?: number;

  /**
   * 总不合格数量（自动计算 = 各明细不合格数量合计）
   */
  totalUnqualifiedQuantity?: number;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus?: number;

  /**
   * 判定人（人员代码）
   */
  judgeBy?: string;

  /**
   * 判定说明
   */
  judgeDescription?: string;

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
 * IpqcOrder 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 IpqcOrderImport
 * @description 对应后端 TaktIpqcOrderImportDto
 */
export interface IpqcOrderImport {
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
   * 来源单号（生产工单编码）
   */
  sourceCode?: string;

  /**
   * IPQC检验单编码（唯一索引，根据来源单号自动生成）
   */
  ipqcOrderCode?: string;

  /**
   * 工序编码
   */
  processCode?: string;

  /**
   * 工序名称
   */
  processName?: string;

  /**
   * 总抽样数量（自动计算 = 各明细抽样数量合计）
   */
  totalSampleQuantity?: number;

  /**
   * 总合格数量（自动计算 = 各明细合格数量合计）
   */
  totalQualifiedQuantity?: number;

  /**
   * 总不合格数量（自动计算 = 各明细不合格数量合计）
   */
  totalUnqualifiedQuantity?: number;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus?: number;

  /**
   * 判定人（人员代码）
   */
  judgeBy?: string;

  /**
   * 判定说明
   */
  judgeDescription?: string;

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
 * IpqcOrder 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 IpqcOrderExport
 * @description 对应后端 TaktIpqcOrderExportDto
 */
export interface IpqcOrderExport {
  /**
   * IpqcOrderID
   */
  ipqcOrderId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 来源单号（生产工单编码）
   */
  sourceCode: string;

  /**
   * 检验日期
   */
  inspectionDate?: string;

  /**
   * IPQC检验单编码（唯一索引，根据来源单号自动生成）
   */
  ipqcOrderCode: string;

  /**
   * 工序编码
   */
  processCode: string;

  /**
   * 工序名称
   */
  processName: string;

  /**
   * 生产总数
   */
  totalProductionQuantity: number;

  /**
   * 总抽样数量（自动计算 = 各明细抽样数量合计）
   */
  totalSampleQuantity: number;

  /**
   * 总合格数量（自动计算 = 各明细合格数量合计）
   */
  totalQualifiedQuantity: number;

  /**
   * 总不合格数量（自动计算 = 各明细不合格数量合计）
   */
  totalUnqualifiedQuantity: number;

  /**
   * 总验退数量（自动计算 = 各明细验退数量合计）
   */
  totalInspectionReturnQuantity: number;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=返工）
   */
  judgeStatus: number;

  /**
   * 判定人（人员代码）
   */
  judgeBy?: string;

  /**
   * 判定日期
   */
  judgeDate?: string;

  /**
   * 判定说明
   */
  judgeDescription?: string;

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

