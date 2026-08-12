// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee-skill.d.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 员工技能与证书
 * 对应前端 TaktEmployeeSkillDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeSkill
 * @description 对应后端 TaktEmployeeSkillDto
 */
export interface EmployeeSkill extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 技能名称
   */
  skillName?: string;

  /**
   * 技能等级（字典 hr_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）
   */
  skillLevel?: number;

  /**
   * 证书名称
   */
  certificateName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 取得日期
   */
  obtainedDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

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
 * EmployeeSkill 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeSkillExport
 * @description 对应后端 TaktEmployeeSkillExportDto
 */
export interface EmployeeSkillExport {
  /**
   * EmployeeSkillID
   */
  employeeSkillId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 技能名称
   */
  skillName: string;

  /**
   * 技能等级（字典 hr_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）
   */
  skillLevel: number;

  /**
   * 证书名称
   */
  certificateName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 取得日期
   */
  obtainedDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

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

