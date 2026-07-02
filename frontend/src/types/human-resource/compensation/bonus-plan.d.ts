// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：bonus-plan.d.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 奖金方案（现金奖金）
 * 对应前端 TaktBonusPlanDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BonusPlan
 * @description 对应后端 TaktBonusPlanDto
 */
export interface BonusPlan extends CompanyDtoBase {
  /**
   * BonusPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  bonusPlanId: string;

  /**
   * 方案编码（租户+公司内唯一）
   */
  planCode: string;

  /**
   * 方案名称
   */
  planName: string;

  /**
   * 奖金类型（字典 hr_comp_bonus_type）
   */
  bonusType: number;

  /**
   * 计算方式（字典 hr_comp_bonus_calc_method_type）
   */
  calcMethod: number;

  /**
   * 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
   */
  salaryFormulaId?: string;

  /**
   * 关联计算公式 名称（填充字段）
   */
  salaryFormulaName?: string;

  /**
   * 默认奖金金额或基数（元）
   */
  defaultAmount: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  planStatus: number;

  /**
   * 方案说明
   */
  bonusPlanDescription?: string;

  /**
   * 关联工厂
   */
  relatedPlant?: string;

}


/**
 * BonusPlan 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BonusPlanQuery
 * @description 对应后端 TaktBonusPlanQueryDto
 */
export interface BonusPlanQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 方案编码（租户+公司内唯一）
   */
  planCode?: string;

  /**
   * 方案名称
   */
  planName?: string;

  /**
   * 奖金类型（字典 hr_comp_bonus_type）
   */
  bonusType?: number;

  /**
   * 计算方式（字典 hr_comp_bonus_calc_method_type）
   */
  calcMethod?: number;

  /**
   * 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
   */
  salaryFormulaId?: string;

  /**
   * 默认奖金金额或基数（元）
   */
  defaultAmount?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  planStatus?: number;

  /**
   * 方案说明
   */
  bonusPlanDescription?: string;

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
 * 创建BonusPlan DTO
 * 对应前端 BonusPlanCreate
 * @description 对应后端 TaktBonusPlanCreateDto
 */
export interface BonusPlanCreate {
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
   * 方案编码（租户+公司内唯一）
   */
  planCode: string;

  /**
   * 方案名称
   */
  planName: string;

  /**
   * 奖金类型（字典 hr_comp_bonus_type）
   */
  bonusType: number;

  /**
   * 计算方式（字典 hr_comp_bonus_calc_method_type）
   */
  calcMethod: number;

  /**
   * 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
   */
  salaryFormulaId?: string;

  /**
   * 默认奖金金额或基数（元）
   */
  defaultAmount: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  planStatus: number;

  /**
   * 方案说明
   */
  bonusPlanDescription?: string;

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
 * 更新BonusPlan DTO
 * 继承 TaktBonusPlanCreateDto，添加 BonusPlanId 字段
 * 对应前端 BonusPlanUpdate
 * @description 对应后端 TaktBonusPlanUpdateDto
 */
export interface BonusPlanUpdate extends BonusPlanCreate {
  /**
   * BonusPlanID（标识要更新的实体）
   */
  bonusPlanId: string;

}


/**
 * BonusPlan 状态更新 DTO
 * 对应前端 BonusPlanStatus
 * @description 对应后端 TaktBonusPlanStatusDto
 */
export interface BonusPlanStatus {
  /**
   * BonusPlanID
   */
  bonusPlanId: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  planStatus: number;

}


/**
 * BonusPlan 导入模板行 DTO
 * 对应前端 BonusPlanTemplate
 * @description 对应后端 TaktBonusPlanTemplateDto
 */
export interface BonusPlanTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 方案编码（租户+公司内唯一）
   */
  planCode?: string;

  /**
   * 方案名称
   */
  planName?: string;

  /**
   * 奖金类型（字典 hr_comp_bonus_type）
   */
  bonusType?: number;

  /**
   * 计算方式（字典 hr_comp_bonus_calc_method_type）
   */
  calcMethod?: number;

  /**
   * 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
   */
  salaryFormulaId?: string;

  /**
   * 默认奖金金额或基数（元）
   */
  defaultAmount?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  planStatus?: number;

  /**
   * 方案说明
   */
  bonusPlanDescription?: string;

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
 * BonusPlan 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BonusPlanImport
 * @description 对应后端 TaktBonusPlanImportDto
 */
export interface BonusPlanImport {
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
   * 方案编码（租户+公司内唯一）
   */
  planCode?: string;

  /**
   * 方案名称
   */
  planName?: string;

  /**
   * 奖金类型（字典 hr_comp_bonus_type）
   */
  bonusType?: number;

  /**
   * 计算方式（字典 hr_comp_bonus_calc_method_type）
   */
  calcMethod?: number;

  /**
   * 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
   */
  salaryFormulaId?: string;

  /**
   * 默认奖金金额或基数（元）
   */
  defaultAmount?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  planStatus?: number;

  /**
   * 方案说明
   */
  bonusPlanDescription?: string;

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
 * BonusPlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BonusPlanExport
 * @description 对应后端 TaktBonusPlanExportDto
 */
export interface BonusPlanExport {
  /**
   * BonusPlanID
   */
  bonusPlanId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 方案编码（租户+公司内唯一）
   */
  planCode: string;

  /**
   * 方案名称
   */
  planName: string;

  /**
   * 奖金类型（字典 hr_comp_bonus_type）
   */
  bonusType: number;

  /**
   * 计算方式（字典 hr_comp_bonus_calc_method_type）
   */
  calcMethod: number;

  /**
   * 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
   */
  salaryFormulaId?: string;

  /**
   * 默认奖金金额或基数（元）
   */
  defaultAmount: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（字典 sys_normal_disable_status）
   */
  planStatus: number;

  /**
   * 方案说明
   */
  bonusPlanDescription?: string;

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

