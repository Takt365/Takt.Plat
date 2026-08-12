// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/benefits
// 文件名称：emp-benefit-plan.d.ts
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
 * 员工福利方案（非现金福利参与配置）
 * 对应前端 TaktEmpBenefitPlanDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmpBenefitPlan
 * @description 对应后端 TaktEmpBenefitPlanDto
 */
export interface EmpBenefitPlan extends CompanyDtoBase {

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
 * EmpBenefitPlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmpBenefitPlanExport
 * @description 对应后端 TaktEmpBenefitPlanExportDto
 */
export interface EmpBenefitPlanExport {
  /**
   * EmpBenefitPlanID
   */
  empBenefitPlanId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 福利项目 ID
   */
  benefitItemId: string;

  /**
   * 方案编码
   */
  planCode: string;

  /**
   * 参保/参与日期
   */
  enrollmentDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 hr_emp_benefit_plan_status）
   */
  empBenefitStatus: number;

  /**
   * 关联工厂
   */
  plantCode?: string;

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

