// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：account-title.d.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 会计科目实体
 * 对应前端 TaktAccountTitleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AccountTitle
 * @description 对应后端 TaktAccountTitleDto
 */
export interface AccountTitle extends CompanyDtoBase {
  /**
   * AccountTitleID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  accountTitleId: string;

  /**
   * 科目编码
   */
  titleCode: string;

  /**
   * 科目名称
   */
  titleName: string;

  /**
   * 父级 ID
   */
  parentId: string;

  /**
   * 科目类型
   */
  titleType: number;

  /**
   * 余额方向（0=借方，1=贷方）
   */
  balanceDirection: number;

  /**
   * 科目层级
   */
  titleLevel: number;

  /**
   * 是否末级科目
   */
  isLeaf: number;

  /**
   * 是否辅助核算
   */
  isAuxiliary: number;

  /**
   * 辅助核算类型
   */
  auxiliaryType: number;

  /**
   * 是否数量核算
   */
  isQuantity: number;

  /**
   * 是否外币核算
   */
  isCurrency: number;

  /**
   * 是否现金科目
   */
  isCash: number;

  /**
   * 是否银行科目
   */
  isBank: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 科目状态（1=启用，0=禁用）
   */
  titleStatus: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * AccountTitle 树形列表/树选择 DTO（含子节点）
 * 对应 GetAccountTitleTreeAsync 等接口
 * 对应前端 AccountTitleTree
 * @description 对应后端 TaktAccountTitleTreeDto
 */
export interface AccountTitleTree extends AccountTitle {
  /**
   * 子节点
   */
  children: AccountTitleTree[];

}


/**
 * AccountTitle 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AccountTitleQuery
 * @description 对应后端 TaktAccountTitleQueryDto
 */
export interface AccountTitleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 科目编码
   */
  titleCode?: string;

  /**
   * 科目名称
   */
  titleName?: string;

  /**
   * 父级 ID
   */
  parentId?: string;

  /**
   * 科目类型
   */
  titleType?: number;

  /**
   * 余额方向（0=借方，1=贷方）
   */
  balanceDirection?: number;

  /**
   * 科目层级
   */
  titleLevel?: number;

  /**
   * 是否末级科目
   */
  isLeaf?: number;

  /**
   * 是否辅助核算
   */
  isAuxiliary?: number;

  /**
   * 辅助核算类型
   */
  auxiliaryType?: number;

  /**
   * 是否数量核算
   */
  isQuantity?: number;

  /**
   * 是否外币核算
   */
  isCurrency?: number;

  /**
   * 是否现金科目
   */
  isCash?: number;

  /**
   * 是否银行科目
   */
  isBank?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 科目状态（1=启用，0=禁用）
   */
  titleStatus?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  validFromStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  validFromEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  validToStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  validToEnd?: string;

  /**
   * 排序号
   */
  sortOrder?: number;

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
 * 创建AccountTitle DTO
 * 对应前端 AccountTitleCreate
 * @description 对应后端 TaktAccountTitleCreateDto
 */
export interface AccountTitleCreate {
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
   * 科目编码
   */
  titleCode: string;

  /**
   * 科目名称
   */
  titleName: string;

  /**
   * 父级 ID
   */
  parentId: string;

  /**
   * 科目类型
   */
  titleType: number;

  /**
   * 余额方向（0=借方，1=贷方）
   */
  balanceDirection: number;

  /**
   * 科目层级
   */
  titleLevel: number;

  /**
   * 是否辅助核算
   */
  isAuxiliary: number;

  /**
   * 辅助核算类型
   */
  auxiliaryType: number;

  /**
   * 是否数量核算
   */
  isQuantity: number;

  /**
   * 是否外币核算
   */
  isCurrency: number;

  /**
   * 是否现金科目
   */
  isCash: number;

  /**
   * 是否银行科目
   */
  isBank: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 科目状态（1=启用，0=禁用）
   */
  titleStatus: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 排序号
   */
  sortOrder: number;

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
 * 更新AccountTitle DTO
 * 继承 TaktAccountTitleCreateDto，添加 AccountTitleId 字段
 * 对应前端 AccountTitleUpdate
 * @description 对应后端 TaktAccountTitleUpdateDto
 */
export interface AccountTitleUpdate extends AccountTitleCreate {
  /**
   * AccountTitleID（标识要更新的实体）
   */
  accountTitleId: string;

}


/**
 * AccountTitle 状态更新 DTO
 * 对应前端 AccountTitleStatus
 * @description 对应后端 TaktAccountTitleStatusDto
 */
export interface AccountTitleStatus {
  /**
   * AccountTitleID
   */
  accountTitleId: string;

  /**
   * 科目状态（1=启用，0=禁用）
   */
  titleStatus: number;

}


/**
 * AccountTitle 排序更新 DTO
 * 对应前端 AccountTitleSort
 * @description 对应后端 TaktAccountTitleSortDto
 */
export interface AccountTitleSort {
  /**
   * AccountTitleID
   */
  accountTitleId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * AccountTitle 导入模板行 DTO
 * 对应前端 AccountTitleTemplate
 * @description 对应后端 TaktAccountTitleTemplateDto
 */
export interface AccountTitleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 科目编码
   */
  titleCode?: string;

  /**
   * 科目名称
   */
  titleName?: string;

  /**
   * 父级 ID
   */
  parentId?: string;

  /**
   * 科目类型
   */
  titleType?: number;

  /**
   * 余额方向（0=借方，1=贷方）
   */
  balanceDirection?: number;

  /**
   * 科目层级
   */
  titleLevel?: number;

  /**
   * 是否辅助核算
   */
  isAuxiliary?: number;

  /**
   * 辅助核算类型
   */
  auxiliaryType?: number;

  /**
   * 是否数量核算
   */
  isQuantity?: number;

  /**
   * 是否外币核算
   */
  isCurrency?: number;

  /**
   * 是否现金科目
   */
  isCash?: number;

  /**
   * 是否银行科目
   */
  isBank?: number;

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
 * AccountTitle 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AccountTitleImport
 * @description 对应后端 TaktAccountTitleImportDto
 */
export interface AccountTitleImport {
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
   * 科目编码
   */
  titleCode?: string;

  /**
   * 科目名称
   */
  titleName?: string;

  /**
   * 父级 ID
   */
  parentId?: string;

  /**
   * 科目类型
   */
  titleType?: number;

  /**
   * 余额方向（0=借方，1=贷方）
   */
  balanceDirection?: number;

  /**
   * 科目层级
   */
  titleLevel?: number;

  /**
   * 是否辅助核算
   */
  isAuxiliary?: number;

  /**
   * 辅助核算类型
   */
  auxiliaryType?: number;

  /**
   * 是否数量核算
   */
  isQuantity?: number;

  /**
   * 是否外币核算
   */
  isCurrency?: number;

  /**
   * 是否现金科目
   */
  isCash?: number;

  /**
   * 是否银行科目
   */
  isBank?: number;

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
 * AccountTitle 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AccountTitleExport
 * @description 对应后端 TaktAccountTitleExportDto
 */
export interface AccountTitleExport {
  /**
   * AccountTitleID
   */
  accountTitleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 科目编码
   */
  titleCode: string;

  /**
   * 科目名称
   */
  titleName: string;

  /**
   * 父级 ID
   */
  parentId: string;

  /**
   * 科目类型
   */
  titleType: number;

  /**
   * 余额方向（0=借方，1=贷方）
   */
  balanceDirection: number;

  /**
   * 科目层级
   */
  titleLevel: number;

  /**
   * 是否末级科目
   */
  isLeaf: number;

  /**
   * 是否辅助核算
   */
  isAuxiliary: number;

  /**
   * 辅助核算类型
   */
  auxiliaryType: number;

  /**
   * 是否数量核算
   */
  isQuantity: number;

  /**
   * 是否外币核算
   */
  isCurrency: number;

  /**
   * 是否现金科目
   */
  isCash: number;

  /**
   * 是否银行科目
   */
  isBank: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

  /**
   * 科目状态（1=启用，0=禁用）
   */
  titleStatus: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 排序号
   */
  sortOrder: number;

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

