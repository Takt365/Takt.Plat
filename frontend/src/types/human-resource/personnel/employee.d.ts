// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee.d.ts
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
 * 员工实体（人事主档，公司级档案非审批单） 仅保留身份与档案基本属性；明细见导航子表： 教育→Education；地址→Address；家庭/紧急联系人→Family； 上岗日期/试用/转正/主部门岗位→Joined；离职→Resignation； 合同→Contract；调动→Reassignment；技能→Skill；履历→Experience； 附件→Attachment；代理→Delegation；入职待办→Onboarding 参照 SAP Personnel Number (PERNR) 设计
 * 对应前端 TaktEmployeeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Employee
 * @description 对应后端 TaktEmployeeDto
 */
export interface Employee extends CompanyDtoBase {
  /**
   * EmployeeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeId: string;

  /**
   * 员工编码（租户+公司内唯一）
   */
  employeeCode: string;

  /**
   * 姓名
   */
  employeeName: string;

  /**
   * 性别（字典 sys_user_gender_category；0=未知 1=男 2=女）
   */
  gender: number;

  /**
   * 出生日期（人事档案必填）
   */
  birthDate: string;

  /**
   * 身份证号（人事档案必填）
   */
  idCardNo: string;

  /**
   * 手机号码（人事档案必填）
   */
  mobile: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
   */
  nativePlace: string;

  /**
   * 民族（字典 hr_ethnic_code；DictValue 1～56）
   */
  ethnicity: number;

  /**
   * 政治面貌（字典 hr_political_affiliation；0～12；人事档案必填）
   */
  politicalAffiliation: number;

  /**
   * 婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
   */
  maritalStatus: number;

  /**
   * 员工状态（字典 hr_employee_status；1=试用期 2=正式 3=离职 4=退休）
   */
  employeeStatus: number;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是；种子员工不可删）
   */
  isBuiltIn: number;

  /**
   * 头像URL（展示用；档案附件明细见 EmployeeAttachments）
   */
  avatar?: string;

  /**
   * 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept） （子表：TaktEmployeeDept）
   */
  employeeDepts?: EmployeeDept[];

  /**
   * 员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost） （子表：TaktEmployeePost）
   */
  employeePosts?: EmployeePost[];

  /**
   * 员工地址（家庭/工作/常住） （子表：TaktEmployeeAddress）
   */
  employeeAddresses?: EmployeeAddress[];

  /**
   * 教育经历（含最高学历 IsHighest） （子表：TaktEmployeeEducation）
   */
  employeeEducations?: EmployeeEducation[];

  /**
   * 家庭成员（含紧急联系人 IsEmergencyContact） （子表：TaktEmployeeFamily）
   */
  employeeFamilies?: EmployeeFamily[];

  /**
   * 外部工作经历 （子表：TaktEmployeeExperience）
   */
  employeeExperiences?: EmployeeExperience[];

  /**
   * 技能与证书 （子表：TaktEmployeeSkill）
   */
  employeeSkills?: EmployeeSkill[];

  /**
   * 劳动合同 （子表：TaktEmployeeContract）
   */
  employeeContracts?: EmployeeContract[];

  /**
   * 入职上岗办理（实际上岗日/试用/转正/部门岗位） （子表：TaktEmployeeJoined）
   */
  employeeJoineds?: EmployeeJoined[];

  /**
   * 入职待办 （子表：TaktEmployeeOnboarding）
   */
  employeeOnboardings?: EmployeeOnboarding[];

  /**
   * 调动记录 （子表：TaktEmployeeReassignment）
   */
  employeeReassignments?: EmployeeReassignment[];

  /**
   * 离职办理 （子表：TaktEmployeeResignation）
   */
  employeeResignations?: EmployeeResignation[];

  /**
   * 档案附件 （子表：TaktEmployeeAttachment）
   */
  employeeAttachments?: EmployeeAttachment[];

}


/**
 * Employee 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeQuery
 * @description 对应后端 TaktEmployeeQueryDto
 */
export interface EmployeeQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 员工编码（租户+公司内唯一）
   */
  employeeCode?: string;

  /**
   * 姓名
   */
  employeeName?: string;

  /**
   * 性别（字典 sys_user_gender_category；0=未知 1=男 2=女）
   */
  gender?: number;

  /**
   * 出生日期（人事档案必填）（范围查询-开始）
   */
  birthDateStart?: string;

  /**
   * 出生日期（人事档案必填）（范围查询-结束）
   */
  birthDateEnd?: string;

  /**
   * 身份证号（人事档案必填）
   */
  idCardNo?: string;

  /**
   * 手机号码（人事档案必填）
   */
  mobile?: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
   */
  nativePlace?: string;

  /**
   * 民族（字典 hr_ethnic_code；DictValue 1～56）
   */
  ethnicity?: number;

  /**
   * 政治面貌（字典 hr_political_affiliation；0～12；人事档案必填）
   */
  politicalAffiliation?: number;

  /**
   * 婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
   */
  maritalStatus?: number;

  /**
   * 员工状态（字典 hr_employee_status；1=试用期 2=正式 3=离职 4=退休）
   */
  employeeStatus?: number;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是；种子员工不可删）
   */
  isBuiltIn?: number;

  /**
   * 头像URL（展示用；档案附件明细见 EmployeeAttachments）
   */
  avatar?: string;

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
 * 创建Employee DTO
 * 对应前端 EmployeeCreate
 * @description 对应后端 TaktEmployeeCreateDto
 */
export interface EmployeeCreate {
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
   * 员工编码（租户+公司内唯一）
   */
  employeeCode: string;

  /**
   * 姓名
   */
  employeeName: string;

  /**
   * 性别（字典 sys_user_gender_category；0=未知 1=男 2=女）
   */
  gender: number;

  /**
   * 出生日期（人事档案必填）
   */
  birthDate: string;

  /**
   * 身份证号（人事档案必填）
   */
  idCardNo: string;

  /**
   * 手机号码（人事档案必填）
   */
  mobile: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
   */
  nativePlace: string;

  /**
   * 民族（字典 hr_ethnic_code；DictValue 1～56）
   */
  ethnicity: number;

  /**
   * 政治面貌（字典 hr_political_affiliation；0～12；人事档案必填）
   */
  politicalAffiliation: number;

  /**
   * 婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
   */
  maritalStatus: number;

  /**
   * 员工状态（字典 hr_employee_status；1=试用期 2=正式 3=离职 4=退休）
   */
  employeeStatus: number;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是；种子员工不可删）
   */
  isBuiltIn: number;

  /**
   * 头像URL（展示用；档案附件明细见 EmployeeAttachments）
   */
  avatar?: string;

  /**
   * 员工部门关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  employeeDeptIds?: any;

  /**
   * 员工岗位关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  employeePostIds?: any;

  /**
   * 员工地址（家庭/工作/常住）（子表，级联保存）
   */
  employeeAddresses?: EmployeeAddressCreate[];

  /**
   * 教育经历（含最高学历 IsHighest）（子表，级联保存）
   */
  employeeEducations?: EmployeeEducationCreate[];

  /**
   * 家庭成员（含紧急联系人 IsEmergencyContact）（子表，级联保存）
   */
  employeeFamilies?: EmployeeFamilyCreate[];

  /**
   * 外部工作经历（子表，级联保存）
   */
  employeeExperiences?: EmployeeExperienceCreate[];

  /**
   * 技能与证书（子表，级联保存）
   */
  employeeSkills?: EmployeeSkillCreate[];

  /**
   * 劳动合同（子表，级联保存）
   */
  employeeContracts?: EmployeeContractCreate[];

  /**
   * 入职上岗办理（实际上岗日/试用/转正/部门岗位）（子表，级联保存）
   */
  employeeJoineds?: EmployeeJoinedCreate[];

  /**
   * 入职待办（子表，级联保存）
   */
  employeeOnboardings?: EmployeeOnboardingCreate[];

  /**
   * 调动记录（子表，级联保存）
   */
  employeeReassignments?: EmployeeReassignmentCreate[];

  /**
   * 离职办理（子表，级联保存）
   */
  employeeResignations?: EmployeeResignationCreate[];

  /**
   * 档案附件（子表，级联保存）
   */
  employeeAttachments?: EmployeeAttachmentCreate[];

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
 * 更新Employee DTO
 * 继承 TaktEmployeeCreateDto，添加 EmployeeId 字段
 * 对应前端 EmployeeUpdate
 * @description 对应后端 TaktEmployeeUpdateDto
 */
export interface EmployeeUpdate extends EmployeeCreate {
  /**
   * EmployeeID（标识要更新的实体）
   */
  employeeId: string;

  /**
   * 员工地址（家庭/工作/常住）（子表，级联保存）
   */
  employeeAddresses?: any;

  /**
   * 教育经历（含最高学历 IsHighest）（子表，级联保存）
   */
  employeeEducations?: any;

  /**
   * 家庭成员（含紧急联系人 IsEmergencyContact）（子表，级联保存）
   */
  employeeFamilies?: any;

  /**
   * 外部工作经历（子表，级联保存）
   */
  employeeExperiences?: any;

  /**
   * 技能与证书（子表，级联保存）
   */
  employeeSkills?: any;

  /**
   * 劳动合同（子表，级联保存）
   */
  employeeContracts?: any;

  /**
   * 入职上岗办理（实际上岗日/试用/转正/部门岗位）（子表，级联保存）
   */
  employeeJoineds?: any;

  /**
   * 入职待办（子表，级联保存）
   */
  employeeOnboardings?: any;

  /**
   * 调动记录（子表，级联保存）
   */
  employeeReassignments?: any;

  /**
   * 离职办理（子表，级联保存）
   */
  employeeResignations?: any;

  /**
   * 档案附件（子表，级联保存）
   */
  employeeAttachments?: any;

}


/**
 * Employee 状态更新 DTO
 * 对应前端 EmployeeStatus
 * @description 对应后端 TaktEmployeeStatusDto
 */
export interface EmployeeStatus {
  /**
   * EmployeeID
   */
  employeeId: string;

  /**
   * 婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
   */
  maritalStatus: number;

}


/**
 * Employee 导入模板行 DTO
 * 对应前端 EmployeeTemplate
 * @description 对应后端 TaktEmployeeTemplateDto
 */
export interface EmployeeTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 员工编码（租户+公司内唯一）
   */
  employeeCode?: string;

  /**
   * 姓名
   */
  employeeName?: string;

  /**
   * 性别（字典 sys_user_gender_category；0=未知 1=男 2=女）
   */
  gender?: number;

  /**
   * 出生日期（人事档案必填）
   */
  birthDate?: string;

  /**
   * 身份证号（人事档案必填）
   */
  idCardNo?: string;

  /**
   * 手机号码（人事档案必填）
   */
  mobile?: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
   */
  nativePlace?: string;

  /**
   * 民族（字典 hr_ethnic_code；DictValue 1～56）
   */
  ethnicity?: number;

  /**
   * 政治面貌（字典 hr_political_affiliation；0～12；人事档案必填）
   */
  politicalAffiliation?: number;

  /**
   * 婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
   */
  maritalStatus?: number;

  /**
   * 员工状态（字典 hr_employee_status；1=试用期 2=正式 3=离职 4=退休）
   */
  employeeStatus?: number;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是；种子员工不可删）
   */
  isBuiltIn?: number;

  /**
   * 头像URL（展示用；档案附件明细见 EmployeeAttachments）
   */
  avatar?: string;

  /**
   * 员工部门关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  employeeDeptIds?: any;

  /**
   * 员工岗位关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  employeePostIds?: any;

  /**
   * 员工地址（家庭/工作/常住）（子表，级联保存）
   */
  employeeAddresses?: EmployeeAddressCreate[];

  /**
   * 教育经历（含最高学历 IsHighest）（子表，级联保存）
   */
  employeeEducations?: EmployeeEducationCreate[];

  /**
   * 家庭成员（含紧急联系人 IsEmergencyContact）（子表，级联保存）
   */
  employeeFamilies?: EmployeeFamilyCreate[];

  /**
   * 外部工作经历（子表，级联保存）
   */
  employeeExperiences?: EmployeeExperienceCreate[];

  /**
   * 技能与证书（子表，级联保存）
   */
  employeeSkills?: EmployeeSkillCreate[];

  /**
   * 劳动合同（子表，级联保存）
   */
  employeeContracts?: EmployeeContractCreate[];

  /**
   * 入职上岗办理（实际上岗日/试用/转正/部门岗位）（子表，级联保存）
   */
  employeeJoineds?: EmployeeJoinedCreate[];

  /**
   * 入职待办（子表，级联保存）
   */
  employeeOnboardings?: EmployeeOnboardingCreate[];

  /**
   * 调动记录（子表，级联保存）
   */
  employeeReassignments?: EmployeeReassignmentCreate[];

  /**
   * 离职办理（子表，级联保存）
   */
  employeeResignations?: EmployeeResignationCreate[];

  /**
   * 档案附件（子表，级联保存）
   */
  employeeAttachments?: EmployeeAttachmentCreate[];

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
 * Employee 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeImport
 * @description 对应后端 TaktEmployeeImportDto
 */
export interface EmployeeImport {
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
   * 员工编码（租户+公司内唯一）
   */
  employeeCode?: string;

  /**
   * 姓名
   */
  employeeName?: string;

  /**
   * 性别（字典 sys_user_gender_category；0=未知 1=男 2=女）
   */
  gender?: number;

  /**
   * 出生日期（人事档案必填）
   */
  birthDate?: string;

  /**
   * 身份证号（人事档案必填）
   */
  idCardNo?: string;

  /**
   * 手机号码（人事档案必填）
   */
  mobile?: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
   */
  nativePlace?: string;

  /**
   * 民族（字典 hr_ethnic_code；DictValue 1～56）
   */
  ethnicity?: number;

  /**
   * 政治面貌（字典 hr_political_affiliation；0～12；人事档案必填）
   */
  politicalAffiliation?: number;

  /**
   * 婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
   */
  maritalStatus?: number;

  /**
   * 员工状态（字典 hr_employee_status；1=试用期 2=正式 3=离职 4=退休）
   */
  employeeStatus?: number;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是；种子员工不可删）
   */
  isBuiltIn?: number;

  /**
   * 头像URL（展示用；档案附件明细见 EmployeeAttachments）
   */
  avatar?: string;

  /**
   * 员工部门关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  employeeDeptIds?: any;

  /**
   * 员工岗位关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  employeePostIds?: any;

  /**
   * 员工地址（家庭/工作/常住）（子表，级联保存）
   */
  employeeAddresses?: EmployeeAddressCreate[];

  /**
   * 教育经历（含最高学历 IsHighest）（子表，级联保存）
   */
  employeeEducations?: EmployeeEducationCreate[];

  /**
   * 家庭成员（含紧急联系人 IsEmergencyContact）（子表，级联保存）
   */
  employeeFamilies?: EmployeeFamilyCreate[];

  /**
   * 外部工作经历（子表，级联保存）
   */
  employeeExperiences?: EmployeeExperienceCreate[];

  /**
   * 技能与证书（子表，级联保存）
   */
  employeeSkills?: EmployeeSkillCreate[];

  /**
   * 劳动合同（子表，级联保存）
   */
  employeeContracts?: EmployeeContractCreate[];

  /**
   * 入职上岗办理（实际上岗日/试用/转正/部门岗位）（子表，级联保存）
   */
  employeeJoineds?: EmployeeJoinedCreate[];

  /**
   * 入职待办（子表，级联保存）
   */
  employeeOnboardings?: EmployeeOnboardingCreate[];

  /**
   * 调动记录（子表，级联保存）
   */
  employeeReassignments?: EmployeeReassignmentCreate[];

  /**
   * 离职办理（子表，级联保存）
   */
  employeeResignations?: EmployeeResignationCreate[];

  /**
   * 档案附件（子表，级联保存）
   */
  employeeAttachments?: EmployeeAttachmentCreate[];

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
 * Employee 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeExport
 * @description 对应后端 TaktEmployeeExportDto
 */
export interface EmployeeExport {
  /**
   * EmployeeID
   */
  employeeId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 员工编码（租户+公司内唯一）
   */
  employeeCode: string;

  /**
   * 姓名
   */
  employeeName: string;

  /**
   * 性别（字典 sys_user_gender_category；0=未知 1=男 2=女）
   */
  gender: number;

  /**
   * 出生日期（人事档案必填）
   */
  birthDate: string;

  /**
   * 身份证号（人事档案必填）
   */
  idCardNo: string;

  /**
   * 手机号码（人事档案必填）
   */
  mobile: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
   */
  nativePlace: string;

  /**
   * 民族（字典 hr_ethnic_code；DictValue 1～56）
   */
  ethnicity: number;

  /**
   * 政治面貌（字典 hr_political_affiliation；0～12；人事档案必填）
   */
  politicalAffiliation: number;

  /**
   * 婚姻状况（字典 hr_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
   */
  maritalStatus: number;

  /**
   * 员工状态（字典 hr_employee_status；1=试用期 2=正式 3=离职 4=退休）
   */
  employeeStatus: number;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是；种子员工不可删）
   */
  isBuiltIn: number;

  /**
   * 头像URL（展示用；档案附件明细见 EmployeeAttachments）
   */
  avatar?: string;

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

