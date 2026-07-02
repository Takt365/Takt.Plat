// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/benefits
// 文件名称：benefit-item.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/benefits 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 福利项目（非直接现金福利主数据；年假请假走考勤模块，培训实施走培训模块，此处仅配置福利项）
 * 对应前端 TaktBenefitItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BenefitItem
 * @description 对应后端 TaktBenefitItemDto
 */
export interface BenefitItem extends CompanyDtoBase {
  /**
   * BenefitItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  benefitItemId: string;

  /**
   * 福利项目编码（租户+公司内唯一）
   */
  itemCode: string;

  /**
   * 福利项目名称
   */
  itemName: string;

  /**
   * 福利大类（字典 hr_benefit_category：保险/补贴/休假/其他）
   */
  benefitCategory: number;

  /**
   * 福利类型（字典 hr_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
   */
  benefitType: number;

  /**
   * 发放周期（字典 hr_benefit_payment_cycle_type）
   */
  paymentCycle: number;

  /**
   * 默认金额或补贴标准（元）
   */
  defaultAmount: number;

  /**
   * 金额上限（元，0 表示不限制）
   */
  maxAmount: number;

  /**
   * 公司承担比例（%，如公积金单位缴存比例）
   */
  employerRatio: number;

  /**
   * 个人承担比例（%，如公积金个人缴存比例）
   */
  employeeRatio: number;

  /**
   * 是否强制福利（字典 sys_yes_no_type）
   */
  isMandatory: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * BenefitItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BenefitItemQuery
 * @description 对应后端 TaktBenefitItemQueryDto
 */
export interface BenefitItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 福利项目编码（租户+公司内唯一）
   */
  itemCode?: string;

  /**
   * 福利项目名称
   */
  itemName?: string;

  /**
   * 福利大类（字典 hr_benefit_category：保险/补贴/休假/其他）
   */
  benefitCategory?: number;

  /**
   * 福利类型（字典 hr_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
   */
  benefitType?: number;

  /**
   * 发放周期（字典 hr_benefit_payment_cycle_type）
   */
  paymentCycle?: number;

  /**
   * 默认金额或补贴标准（元）
   */
  defaultAmount?: number;

  /**
   * 金额上限（元，0 表示不限制）
   */
  maxAmount?: number;

  /**
   * 公司承担比例（%，如公积金单位缴存比例）
   */
  employerRatio?: number;

  /**
   * 个人承担比例（%，如公积金个人缴存比例）
   */
  employeeRatio?: number;

  /**
   * 是否强制福利（字典 sys_yes_no_type）
   */
  isMandatory?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 创建BenefitItem DTO
 * 对应前端 BenefitItemCreate
 * @description 对应后端 TaktBenefitItemCreateDto
 */
export interface BenefitItemCreate {
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
   * 福利项目编码（租户+公司内唯一）
   */
  itemCode: string;

  /**
   * 福利项目名称
   */
  itemName: string;

  /**
   * 福利大类（字典 hr_benefit_category：保险/补贴/休假/其他）
   */
  benefitCategory: number;

  /**
   * 福利类型（字典 hr_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
   */
  benefitType: number;

  /**
   * 发放周期（字典 hr_benefit_payment_cycle_type）
   */
  paymentCycle: number;

  /**
   * 默认金额或补贴标准（元）
   */
  defaultAmount: number;

  /**
   * 金额上限（元，0 表示不限制）
   */
  maxAmount: number;

  /**
   * 公司承担比例（%，如公积金单位缴存比例）
   */
  employerRatio: number;

  /**
   * 个人承担比例（%，如公积金个人缴存比例）
   */
  employeeRatio: number;

  /**
   * 是否强制福利（字典 sys_yes_no_type）
   */
  isMandatory: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * 更新BenefitItem DTO
 * 继承 TaktBenefitItemCreateDto，添加 BenefitItemId 字段
 * 对应前端 BenefitItemUpdate
 * @description 对应后端 TaktBenefitItemUpdateDto
 */
export interface BenefitItemUpdate extends BenefitItemCreate {
  /**
   * BenefitItemID（标识要更新的实体）
   */
  benefitItemId: string;

}


/**
 * BenefitItem 状态更新 DTO
 * 对应前端 BenefitItemStatus
 * @description 对应后端 TaktBenefitItemStatusDto
 */
export interface BenefitItemStatus {
  /**
   * BenefitItemID
   */
  benefitItemId: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus: number;

}


/**
 * BenefitItem 排序更新 DTO
 * 对应前端 BenefitItemSort
 * @description 对应后端 TaktBenefitItemSortDto
 */
export interface BenefitItemSort {
  /**
   * BenefitItemID
   */
  benefitItemId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * BenefitItem 导入模板行 DTO
 * 对应前端 BenefitItemTemplate
 * @description 对应后端 TaktBenefitItemTemplateDto
 */
export interface BenefitItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 福利项目编码（租户+公司内唯一）
   */
  itemCode?: string;

  /**
   * 福利项目名称
   */
  itemName?: string;

  /**
   * 福利大类（字典 hr_benefit_category：保险/补贴/休假/其他）
   */
  benefitCategory?: number;

  /**
   * 福利类型（字典 hr_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
   */
  benefitType?: number;

  /**
   * 发放周期（字典 hr_benefit_payment_cycle_type）
   */
  paymentCycle?: number;

  /**
   * 默认金额或补贴标准（元）
   */
  defaultAmount?: number;

  /**
   * 金额上限（元，0 表示不限制）
   */
  maxAmount?: number;

  /**
   * 公司承担比例（%，如公积金单位缴存比例）
   */
  employerRatio?: number;

  /**
   * 个人承担比例（%，如公积金个人缴存比例）
   */
  employeeRatio?: number;

  /**
   * 是否强制福利（字典 sys_yes_no_type）
   */
  isMandatory?: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * BenefitItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BenefitItemImport
 * @description 对应后端 TaktBenefitItemImportDto
 */
export interface BenefitItemImport {
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
   * 福利项目编码（租户+公司内唯一）
   */
  itemCode?: string;

  /**
   * 福利项目名称
   */
  itemName?: string;

  /**
   * 福利大类（字典 hr_benefit_category：保险/补贴/休假/其他）
   */
  benefitCategory?: number;

  /**
   * 福利类型（字典 hr_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
   */
  benefitType?: number;

  /**
   * 发放周期（字典 hr_benefit_payment_cycle_type）
   */
  paymentCycle?: number;

  /**
   * 默认金额或补贴标准（元）
   */
  defaultAmount?: number;

  /**
   * 金额上限（元，0 表示不限制）
   */
  maxAmount?: number;

  /**
   * 公司承担比例（%，如公积金单位缴存比例）
   */
  employerRatio?: number;

  /**
   * 个人承担比例（%，如公积金个人缴存比例）
   */
  employeeRatio?: number;

  /**
   * 是否强制福利（字典 sys_yes_no_type）
   */
  isMandatory?: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus?: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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
 * BenefitItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BenefitItemExport
 * @description 对应后端 TaktBenefitItemExportDto
 */
export interface BenefitItemExport {
  /**
   * BenefitItemID
   */
  benefitItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 福利项目编码（租户+公司内唯一）
   */
  itemCode: string;

  /**
   * 福利项目名称
   */
  itemName: string;

  /**
   * 福利大类（字典 hr_benefit_category：保险/补贴/休假/其他）
   */
  benefitCategory: number;

  /**
   * 福利类型（字典 hr_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
   */
  benefitType: number;

  /**
   * 发放周期（字典 hr_benefit_payment_cycle_type）
   */
  paymentCycle: number;

  /**
   * 默认金额或补贴标准（元）
   */
  defaultAmount: number;

  /**
   * 金额上限（元，0 表示不限制）
   */
  maxAmount: number;

  /**
   * 公司承担比例（%，如公积金单位缴存比例）
   */
  employerRatio: number;

  /**
   * 个人承担比例（%，如公积金个人缴存比例）
   */
  employeeRatio: number;

  /**
   * 是否强制福利（字典 sys_yes_no_type）
   */
  isMandatory: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  itemStatus: number;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

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

