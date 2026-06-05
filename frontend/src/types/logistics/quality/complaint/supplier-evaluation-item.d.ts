// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：supplier-evaluation-item.d.ts
// 创建时间：2026-06-05
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 供应商评价考核项目明细实体
 * 对应前端 TaktSupplierEvaluationItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SupplierEvaluationItem
 * @description 对应后端 TaktSupplierEvaluationItemDto
 */
export interface SupplierEvaluationItem extends CompanyDtoBase {
  /**
   * SupplierEvaluationItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  supplierEvaluationItemId: string;

  /**
   * 评价表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  evaluationId: string;

  /**
   * 评价表名称（填充字段）
   */
  evaluationName?: string;

  /**
   * 评价表编号（冗余字段，便于查询）
   */
  supplierEvaluationCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 评价类别类型（0=质量管理，1=交付能力，2=价格水平，3=服务水平，4=技术能力，5=管理体系，6=其他）
   */
  categoryType: number;

  /**
   * 评价项目名称
   */
  itemName: string;

  /**
   * 评价项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight: number;

  /**
   * 评分标准
   */
  scoringStandard?: string;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  ratingLevel?: number;

  /**
   * 评价说明/事实依据
   */
  evaluationComment?: string;

  /**
   * 存在问题
   */
  existingIssues?: string;

  /**
   * 改进要求
   */
  improvementRequirement?: string;

  /**
   * 整改要求（0=无需整改，1=限期整改，2=重点整改）
   */
  rectificationRequired: number;

  /**
   * 整改期限
   */
  rectificationDeadline?: string;

  /**
   * 整改状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus: number;

  /**
   * 评价表主表 （主表：TaktSupplierEvaluation）
   */
  evaluation?: SupplierEvaluation;

}


/**
 * SupplierEvaluationItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SupplierEvaluationItemQuery
 * @description 对应后端 TaktSupplierEvaluationItemQueryDto
 */
export interface SupplierEvaluationItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 评价表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  evaluationId?: string;

  /**
   * 评价表编号（冗余字段，便于查询）
   */
  supplierEvaluationCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 评价类别类型（0=质量管理，1=交付能力，2=价格水平，3=服务水平，4=技术能力，5=管理体系，6=其他）
   */
  categoryType?: number;

  /**
   * 评价项目名称
   */
  itemName?: string;

  /**
   * 评价项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight?: number;

  /**
   * 评分标准
   */
  scoringStandard?: string;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  ratingLevel?: number;

  /**
   * 评价说明/事实依据
   */
  evaluationComment?: string;

  /**
   * 存在问题
   */
  existingIssues?: string;

  /**
   * 改进要求
   */
  improvementRequirement?: string;

  /**
   * 整改要求（0=无需整改，1=限期整改，2=重点整改）
   */
  rectificationRequired?: number;

  /**
   * 整改期限（范围查询-开始）
   */
  rectificationDeadlineStart?: string;

  /**
   * 整改期限（范围查询-结束）
   */
  rectificationDeadlineEnd?: string;

  /**
   * 整改状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus?: number;

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
 * 创建SupplierEvaluationItem DTO
 * 对应前端 SupplierEvaluationItemCreate
 * @description 对应后端 TaktSupplierEvaluationItemCreateDto
 */
export interface SupplierEvaluationItemCreate {
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
   * 评价表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  evaluationId: string;

  /**
   * 评价表编号（冗余字段，便于查询）
   */
  supplierEvaluationCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 评价类别类型（0=质量管理，1=交付能力，2=价格水平，3=服务水平，4=技术能力，5=管理体系，6=其他）
   */
  categoryType: number;

  /**
   * 评价项目名称
   */
  itemName: string;

  /**
   * 评价项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight: number;

  /**
   * 评分标准
   */
  scoringStandard?: string;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  ratingLevel?: number;

  /**
   * 评价说明/事实依据
   */
  evaluationComment?: string;

  /**
   * 存在问题
   */
  existingIssues?: string;

  /**
   * 改进要求
   */
  improvementRequirement?: string;

  /**
   * 整改要求（0=无需整改，1=限期整改，2=重点整改）
   */
  rectificationRequired: number;

  /**
   * 整改期限
   */
  rectificationDeadline?: string;

  /**
   * 整改状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus: number;

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
 * 更新SupplierEvaluationItem DTO
 * 继承 TaktSupplierEvaluationItemCreateDto，添加 SupplierEvaluationItemId 字段
 * 对应前端 SupplierEvaluationItemUpdate
 * @description 对应后端 TaktSupplierEvaluationItemUpdateDto
 */
export interface SupplierEvaluationItemUpdate extends SupplierEvaluationItemCreate {
  /**
   * SupplierEvaluationItemID（标识要更新的实体）
   */
  supplierEvaluationItemId: string;

}


/**
 * SupplierEvaluationItem 状态更新 DTO
 * 对应前端 SupplierEvaluationItemStatus
 * @description 对应后端 TaktSupplierEvaluationItemStatusDto
 */
export interface SupplierEvaluationItemStatus {
  /**
   * SupplierEvaluationItemID
   */
  supplierEvaluationItemId: string;

  /**
   * 整改状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus: number;

}


/**
 * SupplierEvaluationItem 导入模板行 DTO
 * 对应前端 SupplierEvaluationItemTemplate
 * @description 对应后端 TaktSupplierEvaluationItemTemplateDto
 */
export interface SupplierEvaluationItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 评价表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  evaluationId?: string;

  /**
   * 评价表编号（冗余字段，便于查询）
   */
  supplierEvaluationCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 评价类别类型（0=质量管理，1=交付能力，2=价格水平，3=服务水平，4=技术能力，5=管理体系，6=其他）
   */
  categoryType?: number;

  /**
   * 评价项目名称
   */
  itemName?: string;

  /**
   * 评价项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight?: number;

  /**
   * 评分标准
   */
  scoringStandard?: string;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  ratingLevel?: number;

  /**
   * 评价说明/事实依据
   */
  evaluationComment?: string;

  /**
   * 存在问题
   */
  existingIssues?: string;

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
 * SupplierEvaluationItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SupplierEvaluationItemImport
 * @description 对应后端 TaktSupplierEvaluationItemImportDto
 */
export interface SupplierEvaluationItemImport {
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
   * 评价表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  evaluationId?: string;

  /**
   * 评价表编号（冗余字段，便于查询）
   */
  supplierEvaluationCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 评价类别类型（0=质量管理，1=交付能力，2=价格水平，3=服务水平，4=技术能力，5=管理体系，6=其他）
   */
  categoryType?: number;

  /**
   * 评价项目名称
   */
  itemName?: string;

  /**
   * 评价项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight?: number;

  /**
   * 评分标准
   */
  scoringStandard?: string;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  ratingLevel?: number;

  /**
   * 评价说明/事实依据
   */
  evaluationComment?: string;

  /**
   * 存在问题
   */
  existingIssues?: string;

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
 * SupplierEvaluationItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SupplierEvaluationItemExport
 * @description 对应后端 TaktSupplierEvaluationItemExportDto
 */
export interface SupplierEvaluationItemExport {
  /**
   * SupplierEvaluationItemID
   */
  supplierEvaluationItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 评价表ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  evaluationId: string;

  /**
   * 评价表编号（冗余字段，便于查询）
   */
  supplierEvaluationCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 评价类别类型（0=质量管理，1=交付能力，2=价格水平，3=服务水平，4=技术能力，5=管理体系，6=其他）
   */
  categoryType: number;

  /**
   * 评价项目名称
   */
  itemName: string;

  /**
   * 评价项目说明
   */
  itemDescription?: string;

  /**
   * 权重（%）
   */
  weight: number;

  /**
   * 评分标准
   */
  scoringStandard?: string;

  /**
   * 评分（0-100分）
   */
  score?: number;

  /**
   * 评级（0=D级-不合格，1=C级-合格，2=B级-良好，3=A级-优秀）
   */
  ratingLevel?: number;

  /**
   * 评价说明/事实依据
   */
  evaluationComment?: string;

  /**
   * 存在问题
   */
  existingIssues?: string;

  /**
   * 改进要求
   */
  improvementRequirement?: string;

  /**
   * 整改要求（0=无需整改，1=限期整改，2=重点整改）
   */
  rectificationRequired: number;

  /**
   * 整改期限
   */
  rectificationDeadline?: string;

  /**
   * 整改状态（0=无需整改，1=待整改，2=整改中，3=已完成，4=未通过）
   */
  rectificationStatus: number;

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

