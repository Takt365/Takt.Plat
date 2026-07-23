// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：issue-assy-rework.d.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 品质问题应对明细 - 组装不良改修应对(组装选别・改修费用)
 * 对应前端 TaktQualityIssueAssyReworkDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityIssueAssyRework
 * @description 对应后端 TaktQualityIssueAssyReworkDto
 */
export interface QualityIssueAssyRework extends CompanyDtoBase {
  /**
   * QualityIssueAssyReworkID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityIssueAssyReworkId: string;

  /**
   * 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
   */
  qualityIssueId: string;

  /**
   * 品质问题主表 名称（填充字段）
   */
  qualityIssueName?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityIssueCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost: number;

  /**
   * 组装顾客名
   */
  assyCustomerName1?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 品质问题主表(导航属性) （主表：TaktQualityIssue）
   */
  issue?: QualityIssue;

}


/**
 * QualityIssueAssyRework 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityIssueAssyReworkQuery
 * @description 对应后端 TaktQualityIssueAssyReworkQueryDto
 */
export interface QualityIssueAssyReworkQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
   */
  qualityIssueId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityIssueCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost?: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes?: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes?: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost?: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost?: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses?: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost?: number;

  /**
   * 组装顾客名
   */
  assyCustomerName1?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2?: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建QualityIssueAssyRework DTO
 * 对应前端 QualityIssueAssyReworkCreate
 * @description 对应后端 TaktQualityIssueAssyReworkCreateDto
 */
export interface QualityIssueAssyReworkCreate {
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
   * 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
   */
  qualityIssueId: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityIssueCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost: number;

  /**
   * 组装顾客名
   */
  assyCustomerName1?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
 * 更新QualityIssueAssyRework DTO
 * 继承 TaktQualityIssueAssyReworkCreateDto，添加 QualityIssueAssyReworkId 字段
 * 对应前端 QualityIssueAssyReworkUpdate
 * @description 对应后端 TaktQualityIssueAssyReworkUpdateDto
 */
export interface QualityIssueAssyReworkUpdate extends QualityIssueAssyReworkCreate {
  /**
   * QualityIssueAssyReworkID（标识要更新的实体）
   */
  qualityIssueAssyReworkId: string;

}


/**
 * QualityIssueAssyRework 作废/撤销作废 DTO
 * 对应前端 QualityIssueAssyReworkObsolete
 * @description 对应后端 TaktQualityIssueAssyReworkObsoleteDto
 */
export interface QualityIssueAssyReworkObsolete {
  /**
   * QualityIssueAssyReworkID
   */
  qualityIssueAssyReworkId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * QualityIssueAssyRework 导入模板行 DTO
 * 对应前端 QualityIssueAssyReworkTemplate
 * @description 对应后端 TaktQualityIssueAssyReworkTemplateDto
 */
export interface QualityIssueAssyReworkTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
   */
  qualityIssueId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityIssueCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost?: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes?: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes?: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost?: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost?: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses?: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost?: number;

  /**
   * 组装顾客名
   */
  assyCustomerName1?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2?: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * QualityIssueAssyRework 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityIssueAssyReworkImport
 * @description 对应后端 TaktQualityIssueAssyReworkImportDto
 */
export interface QualityIssueAssyReworkImport {
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
   * 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
   */
  qualityIssueId?: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityIssueCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost?: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes?: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes?: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost?: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost?: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses?: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost?: number;

  /**
   * 组装顾客名
   */
  assyCustomerName1?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2?: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * QualityIssueAssyRework 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityIssueAssyReworkExport
 * @description 对应后端 TaktQualityIssueAssyReworkExportDto
 */
export interface QualityIssueAssyReworkExport {
  /**
   * QualityIssueAssyReworkID
   */
  qualityIssueAssyReworkId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
   */
  qualityIssueId: string;

  /**
   * 品质问题编码（冗余字段，便于查询）
   */
  qualityIssueCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 组装不良内容(Parts/Components)
   */
  assyDefectParts?: string;

  /**
   * 组装选别・改修费用(元)
   */
  assyReworkCost: number;

  /**
   * 组装选别・改修时间(分钟)
   */
  assyReworkTimeMinutes: number;

  /**
   * 组装再检查时间(分钟)
   */
  assyReinspectionTimeMinutes: number;

  /**
   * 组装交通费、旅费(元)
   */
  assyTravelCost: number;

  /**
   * 组装仓库管理费(元)
   */
  assyWarehouseCost: number;

  /**
   * 组装选别・改修其他费用(元)
   */
  assyOtherExpenses: number;

  /**
   * 组装选别・改修备注
   */
  assyReworkNote?: string;

  /**
   * 组装向顾客的费用请求(元)
   */
  assyScrapCost: number;

  /**
   * 组装顾客名
   */
  assyCustomerName1?: string;

  /**
   * 组装 Debit Note No
   */
  assyDebitNoteNo?: string;

  /**
   * 组装其他费用(元)
   */
  assyOtherExpenses2: number;

  /**
   * 组装备注
   */
  assyNote?: string;

  /**
   * 组装不良改修应对记录者
   */
  assyRecorder?: string;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

