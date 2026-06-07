// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：iqc-defect-handling.d.ts
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
 * IQC进货检验不良处理记录实体
 * 对应前端 TaktIqcDefectHandlingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 IqcDefectHandling
 * @description 对应后端 TaktIqcDefectHandlingDto
 */
export interface IqcDefectHandling extends CompanyDtoBase {
  /**
   * IqcDefectHandlingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  iqcDefectHandlingId: string;

  /**
   * IQC不良处理编码
   */
  iqcDefectHandlingCode: string;

  /**
   * IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderItemId: string;

  /**
   * IQC检验单明细名称（填充字段）
   */
  iqcOrderItemName?: string;

  /**
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType: number;

  /**
   * 不良现象编码
   */
  defectCode: string;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

  /**
   * 处理人（人员代码）
   */
  handlerBy?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus: number;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

  /**
   * IQC检验单明细（主表） （主表：TaktIqcOrderItem）
   */
  orderItem?: IqcOrderItem;

}


/**
 * IqcDefectHandling 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 IqcDefectHandlingQuery
 * @description 对应后端 TaktIqcDefectHandlingQueryDto
 */
export interface IqcDefectHandlingQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * IQC不良处理编码
   */
  iqcDefectHandlingCode?: string;

  /**
   * IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderItemId?: string;

  /**
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType?: number;

  /**
   * 不良现象编码
   */
  defectCode?: string;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

  /**
   * 处理人（人员代码）
   */
  handlerBy?: string;

  /**
   * 处理时间（范围查询-开始）
   */
  handlingAtStart?: string;

  /**
   * 处理时间（范围查询-结束）
   */
  handlingAtEnd?: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus?: number;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

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
 * 创建IqcDefectHandling DTO
 * 对应前端 IqcDefectHandlingCreate
 * @description 对应后端 TaktIqcDefectHandlingCreateDto
 */
export interface IqcDefectHandlingCreate {
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
   * IQC不良处理编码
   */
  iqcDefectHandlingCode: string;

  /**
   * IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderItemId: string;

  /**
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType: number;

  /**
   * 不良现象编码
   */
  defectCode: string;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

  /**
   * 处理人（人员代码）
   */
  handlerBy?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus: number;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

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
 * 更新IqcDefectHandling DTO
 * 继承 TaktIqcDefectHandlingCreateDto，添加 IqcDefectHandlingId 字段
 * 对应前端 IqcDefectHandlingUpdate
 * @description 对应后端 TaktIqcDefectHandlingUpdateDto
 */
export interface IqcDefectHandlingUpdate extends IqcDefectHandlingCreate {
  /**
   * IqcDefectHandlingID（标识要更新的实体）
   */
  iqcDefectHandlingId: string;

}


/**
 * IqcDefectHandling 状态更新 DTO
 * 对应前端 IqcDefectHandlingStatus
 * @description 对应后端 TaktIqcDefectHandlingStatusDto
 */
export interface IqcDefectHandlingStatus {
  /**
   * IqcDefectHandlingID
   */
  iqcDefectHandlingId: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus: number;

}


/**
 * IqcDefectHandling 导入模板行 DTO
 * 对应前端 IqcDefectHandlingTemplate
 * @description 对应后端 TaktIqcDefectHandlingTemplateDto
 */
export interface IqcDefectHandlingTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * IQC不良处理编码
   */
  iqcDefectHandlingCode?: string;

  /**
   * IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderItemId?: string;

  /**
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType?: number;

  /**
   * 不良现象编码
   */
  defectCode?: string;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

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
 * IqcDefectHandling 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 IqcDefectHandlingImport
 * @description 对应后端 TaktIqcDefectHandlingImportDto
 */
export interface IqcDefectHandlingImport {
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
   * IQC不良处理编码
   */
  iqcDefectHandlingCode?: string;

  /**
   * IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderItemId?: string;

  /**
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType?: number;

  /**
   * 不良现象编码
   */
  defectCode?: string;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

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
 * IqcDefectHandling 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 IqcDefectHandlingExport
 * @description 对应后端 TaktIqcDefectHandlingExportDto
 */
export interface IqcDefectHandlingExport {
  /**
   * IqcDefectHandlingID
   */
  iqcDefectHandlingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * IQC不良处理编码
   */
  iqcDefectHandlingCode: string;

  /**
   * IQC检验单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderItemId: string;

  /**
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 不良类型（0=轻微，1=一般，2=严重，3=致命）
   */
  defectType: number;

  /**
   * 不良现象编码
   */
  defectCode: string;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 处理方式（0=返工，1=返修，2=让步接收，3=退货，4=报废，5=挑选使用）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 责任部门
   */
  responsibleDept?: string;

  /**
   * 责任人（人员代码）
   */
  responsibleBy?: string;

  /**
   * 处理人（人员代码）
   */
  handlerBy?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 处理结果（0=待处理，1=处理中，2=已完成，3=已关闭）
   */
  handlingStatus: number;

  /**
   * 预防措施/纠正措施
   */
  correctiveAction?: string;

  /**
   * 不良图片（JSON格式，存储不良图片URL列表）
   */
  defectImages?: string;

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

