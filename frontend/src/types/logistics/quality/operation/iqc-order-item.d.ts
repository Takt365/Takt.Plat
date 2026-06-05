// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：iqc-order-item.d.ts
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
 * IQC进货检验单明细实体
 * 对应前端 TaktIqcOrderItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 IqcOrderItem
 * @description 对应后端 TaktIqcOrderItemDto
 */
export interface IqcOrderItem extends CompanyDtoBase {
  /**
   * IqcOrderItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  iqcOrderItemId: string;

  /**
   * IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderId: string;

  /**
   * IQC检验单名称（填充字段）
   */
  iqcOrderName?: string;

  /**
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 进货数量
   */
  purchaseQuantity: number;

  /**
   * 检验标准编码
   */
  standardCode: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod: number;

  /**
   * 抽样数量
   */
  sampleQuantity: number;

  /**
   * 合格数量
   */
  qualifiedQuantity: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity: number;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
   */
  judgeStatus: number;

  /**
   * 抽检序列号
   */
  sampleSerialNo?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（人员代码）
   */
  inspectorBy: string;

  /**
   * 检验日期
   */
  inspectionDate: string;

  /**
   * IQC检验单（主表） （主表：TaktIqcOrder）
   */
  order?: IqcOrder;

  /**
   * 不良处理记录列表（主子表关系） （子表：TaktIqcDefectHandling）
   */
  defectHandlings?: IqcDefectHandling[];

}


/**
 * IqcOrderItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 IqcOrderItemQuery
 * @description 对应后端 TaktIqcOrderItemQueryDto
 */
export interface IqcOrderItemQuery extends TaktPagedQuery {
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
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 进货数量
   */
  purchaseQuantity?: number;

  /**
   * 检验标准编码
   */
  standardCode?: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode?: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod?: number;

  /**
   * 抽样数量
   */
  sampleQuantity?: number;

  /**
   * 合格数量
   */
  qualifiedQuantity?: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity?: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity?: number;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
   */
  judgeStatus?: number;

  /**
   * 抽检序列号
   */
  sampleSerialNo?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（人员代码）
   */
  inspectorBy?: string;

  /**
   * 检验日期（范围查询-开始）
   */
  inspectionDateStart?: string;

  /**
   * 检验日期（范围查询-结束）
   */
  inspectionDateEnd?: string;

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
 * 创建IqcOrderItem DTO
 * 对应前端 IqcOrderItemCreate
 * @description 对应后端 TaktIqcOrderItemCreateDto
 */
export interface IqcOrderItemCreate {
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
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 进货数量
   */
  purchaseQuantity: number;

  /**
   * 检验标准编码
   */
  standardCode: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod: number;

  /**
   * 抽样数量
   */
  sampleQuantity: number;

  /**
   * 合格数量
   */
  qualifiedQuantity: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity: number;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
   */
  judgeStatus: number;

  /**
   * 抽检序列号
   */
  sampleSerialNo?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（人员代码）
   */
  inspectorBy: string;

  /**
   * 检验日期
   */
  inspectionDate: string;

  /**
   * 不良处理记录列表（主子表关系）（子表，级联保存）
   */
  defectHandlings?: IqcDefectHandlingCreate[];

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
 * 更新IqcOrderItem DTO
 * 继承 TaktIqcOrderItemCreateDto，添加 IqcOrderItemId 字段
 * 对应前端 IqcOrderItemUpdate
 * @description 对应后端 TaktIqcOrderItemUpdateDto
 */
export interface IqcOrderItemUpdate extends IqcOrderItemCreate {
  /**
   * IqcOrderItemID（标识要更新的实体）
   */
  iqcOrderItemId: string;

}


/**
 * IqcOrderItem 状态更新 DTO
 * 对应前端 IqcOrderItemStatus
 * @description 对应后端 TaktIqcOrderItemStatusDto
 */
export interface IqcOrderItemStatus {
  /**
   * IqcOrderItemID
   */
  iqcOrderItemId: string;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
   */
  judgeStatus: number;

}


/**
 * IqcOrderItem 导入模板行 DTO
 * 对应前端 IqcOrderItemTemplate
 * @description 对应后端 TaktIqcOrderItemTemplateDto
 */
export interface IqcOrderItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderId?: string;

  /**
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 检验标准编码
   */
  standardCode?: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode?: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod?: number;

  /**
   * 抽样数量
   */
  sampleQuantity?: number;

  /**
   * 合格数量
   */
  qualifiedQuantity?: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity?: number;

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
 * IqcOrderItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 IqcOrderItemImport
 * @description 对应后端 TaktIqcOrderItemImportDto
 */
export interface IqcOrderItemImport {
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
   * IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderId?: string;

  /**
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料名称
   */
  materialName?: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 检验标准编码
   */
  standardCode?: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode?: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod?: number;

  /**
   * 抽样数量
   */
  sampleQuantity?: number;

  /**
   * 合格数量
   */
  qualifiedQuantity?: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity?: number;

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
 * IqcOrderItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 IqcOrderItemExport
 * @description 对应后端 TaktIqcOrderItemExportDto
 */
export interface IqcOrderItemExport {
  /**
   * IqcOrderItemID
   */
  iqcOrderItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * IQC检验单ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  iqcOrderId: string;

  /**
   * IQC检验单编码（冗余字段，便于查询）
   */
  iqcOrderCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料名称
   */
  materialName: string;

  /**
   * 批次号
   */
  batchNo?: string;

  /**
   * 进货数量
   */
  purchaseQuantity: number;

  /**
   * 检验标准编码
   */
  standardCode: string;

  /**
   * 抽样方案编码
   */
  samplingSchemeCode: string;

  /**
   * 检验方式（0=免检，1=减量，2=正常，3=加严，4=全检）
   */
  inspectionMethod: number;

  /**
   * 抽样数量
   */
  sampleQuantity: number;

  /**
   * 合格数量
   */
  qualifiedQuantity: number;

  /**
   * 不合格数量
   */
  unqualifiedQuantity: number;

  /**
   * 验退数量
   */
  inspectionReturnQuantity: number;

  /**
   * 判定状态（0=待判定，1=合格，2=不合格，3=让步接收，4=退货）
   */
  judgeStatus: number;

  /**
   * 抽检序列号
   */
  sampleSerialNo?: string;

  /**
   * 检验说明
   */
  inspectionDescription?: string;

  /**
   * 检验员（人员代码）
   */
  inspectorBy: string;

  /**
   * 检验日期
   */
  inspectionDate: string;

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

