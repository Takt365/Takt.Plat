// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employee.d.ts
// 创建时间：2026-06-07
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
 * 员工实体（人事主档，公司级档案非审批单） 员工与系统用户分离；子表承载合同、调动、任职、教育、家庭、技能、外部履历、附件等全场景明细 参照 SAP Personnel Number (PERNR) 设计
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
   * 员工编号（租户+公司内唯一）
   */
  employeeNo: string;

  /**
   * 姓名
   */
  name: string;

  /**
   * 性别（0=未知，1=男，2=女）
   */
  gender: number;

  /**
   * 出生日期
   */
  birthDate?: string;

  /**
   * 身份证号
   */
  idCardNo?: string;

  /**
   * 手机号码
   */
  mobile?: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place 编码或文本）
   */
  nativePlace?: string;

  /**
   * 民族（字典 hr_ethnic_group 编码或文本）
   */
  ethnicity?: string;

  /**
   * 政治面貌（字典 hr_political_status 编码或文本）
   */
  politicalStatus?: string;

  /**
   * 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）
   */
  maritalStatus?: number;

  /**
   * 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
   */
  education?: number;

  /**
   * 毕业院校（最高学历摘要）
   */
  graduateSchool?: string;

  /**
   * 专业（最高学历摘要）
   */
  major?: string;

  /**
   * 实际上岗日期（JoinedDate：入职上班；招聘录用见人才管理 TaktTalentOffer）
   */
  joinedDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 转正日期
   */
  regularDate?: string;

  /**
   * 离职日期
   */
  terminationDate?: string;

  /**
   * 最后工作日
   */
  lastWorkDate?: string;

  /**
   * 离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
   */
  resignationType?: number;

  /**
   * 离职原因
   */
  resignationReason?: string;

  /**
   * 员工状态（1=试用期，2=正式，3=离职，4=退休）
   */
  employeeStatus: number;

  /**
   * 当前主部门ID（任职快照，与最新已生效上岗单同步）
   */
  primaryDeptId?: string;

  /**
   * 当前主部门名称（填充字段）
   */
  primaryDeptName?: string;

  /**
   * 当前主岗位ID（任职快照）
   */
  primaryPostId?: string;

  /**
   * 当前主岗位名称（填充字段）
   */
  primaryPostName?: string;

  /**
   * 是否内置（种子员工不可删）
   */
  isBuiltIn: number;

  /**
   * 紧急联系人姓名
   */
  emergencyContactName?: string;

  /**
   * 紧急联系人电话
   */
  emergencyContactPhone?: string;

  /**
   * 家庭住址
   */
  homeAddress?: string;

  /**
   * 照片URL
   */
  photoUrl?: string;

  /**
   * 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept） （子表：TaktEmployeeDept）
   */
  employeeDepts?: EmployeeDept[];

  /**
   * 员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost） （子表：TaktEmployeePost）
   */
  employeePosts?: EmployeePost[];

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
   * 员工编号（租户+公司内唯一）
   */
  employeeNo?: string;

  /**
   * 姓名
   */
  name?: string;

  /**
   * 性别（0=未知，1=男，2=女）
   */
  gender?: number;

  /**
   * 出生日期（范围查询-开始）
   */
  birthDateStart?: string;

  /**
   * 出生日期（范围查询-结束）
   */
  birthDateEnd?: string;

  /**
   * 身份证号
   */
  idCardNo?: string;

  /**
   * 手机号码
   */
  mobile?: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place 编码或文本）
   */
  nativePlace?: string;

  /**
   * 民族（字典 hr_ethnic_group 编码或文本）
   */
  ethnicity?: string;

  /**
   * 政治面貌（字典 hr_political_status 编码或文本）
   */
  politicalStatus?: string;

  /**
   * 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）
   */
  maritalStatus?: number;

  /**
   * 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
   */
  education?: number;

  /**
   * 毕业院校（最高学历摘要）
   */
  graduateSchool?: string;

  /**
   * 专业（最高学历摘要）
   */
  major?: string;

  /**
   * 实际上岗日期（JoinedDate：入职上班；招聘录用见人才管理 TaktTalentOffer）（范围查询-开始）
   */
  joinedDateStart?: string;

  /**
   * 实际上岗日期（JoinedDate：入职上班；招聘录用见人才管理 TaktTalentOffer）（范围查询-结束）
   */
  joinedDateEnd?: string;

  /**
   * 试用期结束日期（范围查询-开始）
   */
  probationEndDateStart?: string;

  /**
   * 试用期结束日期（范围查询-结束）
   */
  probationEndDateEnd?: string;

  /**
   * 转正日期（范围查询-开始）
   */
  regularDateStart?: string;

  /**
   * 转正日期（范围查询-结束）
   */
  regularDateEnd?: string;

  /**
   * 离职日期（范围查询-开始）
   */
  terminationDateStart?: string;

  /**
   * 离职日期（范围查询-结束）
   */
  terminationDateEnd?: string;

  /**
   * 最后工作日（范围查询-开始）
   */
  lastWorkDateStart?: string;

  /**
   * 最后工作日（范围查询-结束）
   */
  lastWorkDateEnd?: string;

  /**
   * 离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
   */
  resignationType?: number;

  /**
   * 离职原因
   */
  resignationReason?: string;

  /**
   * 员工状态（1=试用期，2=正式，3=离职，4=退休）
   */
  employeeStatus?: number;

  /**
   * 当前主部门ID（任职快照，与最新已生效上岗单同步）
   */
  primaryDeptId?: string;

  /**
   * 当前主岗位ID（任职快照）
   */
  primaryPostId?: string;

  /**
   * 是否内置（种子员工不可删）
   */
  isBuiltIn?: number;

  /**
   * 紧急联系人姓名
   */
  emergencyContactName?: string;

  /**
   * 紧急联系人电话
   */
  emergencyContactPhone?: string;

  /**
   * 家庭住址
   */
  homeAddress?: string;

  /**
   * 照片URL
   */
  photoUrl?: string;

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
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 员工编号（租户+公司内唯一）
   */
  employeeNo: string;

  /**
   * 姓名
   */
  name: string;

  /**
   * 性别（0=未知，1=男，2=女）
   */
  gender: number;

  /**
   * 出生日期
   */
  birthDate?: string;

  /**
   * 身份证号
   */
  idCardNo?: string;

  /**
   * 手机号码
   */
  mobile?: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place 编码或文本）
   */
  nativePlace?: string;

  /**
   * 民族（字典 hr_ethnic_group 编码或文本）
   */
  ethnicity?: string;

  /**
   * 政治面貌（字典 hr_political_status 编码或文本）
   */
  politicalStatus?: string;

  /**
   * 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）
   */
  maritalStatus?: number;

  /**
   * 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
   */
  education?: number;

  /**
   * 毕业院校（最高学历摘要）
   */
  graduateSchool?: string;

  /**
   * 专业（最高学历摘要）
   */
  major?: string;

  /**
   * 实际上岗日期（JoinedDate：入职上班；招聘录用见人才管理 TaktTalentOffer）
   */
  joinedDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 转正日期
   */
  regularDate?: string;

  /**
   * 离职日期
   */
  terminationDate?: string;

  /**
   * 最后工作日
   */
  lastWorkDate?: string;

  /**
   * 离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
   */
  resignationType?: number;

  /**
   * 离职原因
   */
  resignationReason?: string;

  /**
   * 员工状态（1=试用期，2=正式，3=离职，4=退休）
   */
  employeeStatus: number;

  /**
   * 当前主部门ID（任职快照，与最新已生效上岗单同步）
   */
  primaryDeptId?: string;

  /**
   * 当前主岗位ID（任职快照）
   */
  primaryPostId?: string;

  /**
   * 是否内置（种子员工不可删）
   */
  isBuiltIn: number;

  /**
   * 紧急联系人姓名
   */
  emergencyContactName?: string;

  /**
   * 紧急联系人电话
   */
  emergencyContactPhone?: string;

  /**
   * 家庭住址
   */
  homeAddress?: string;

  /**
   * 照片URL
   */
  photoUrl?: string;

  /**
   * 员工部门关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  employeeDeptIds?: any;

  /**
   * 员工岗位关联（RBAC 全量覆盖，分配走 ITaktRbacService）
   */
  employeePostIds?: any;

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
   * 政治面貌（字典 hr_political_status 编码或文本）
   */
  politicalStatus: string;

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
   * 员工编号（租户+公司内唯一）
   */
  employeeNo?: string;

  /**
   * 姓名
   */
  name?: string;

  /**
   * 性别（0=未知，1=男，2=女）
   */
  gender?: number;

  /**
   * 身份证号
   */
  idCardNo?: string;

  /**
   * 手机号码
   */
  mobile?: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place 编码或文本）
   */
  nativePlace?: string;

  /**
   * 民族（字典 hr_ethnic_group 编码或文本）
   */
  ethnicity?: string;

  /**
   * 政治面貌（字典 hr_political_status 编码或文本）
   */
  politicalStatus?: string;

  /**
   * 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）
   */
  maritalStatus?: number;

  /**
   * 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
   */
  education?: number;

  /**
   * 毕业院校（最高学历摘要）
   */
  graduateSchool?: string;

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
   * 当前公司默认区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 员工编号（租户+公司内唯一）
   */
  employeeNo?: string;

  /**
   * 姓名
   */
  name?: string;

  /**
   * 性别（0=未知，1=男，2=女）
   */
  gender?: number;

  /**
   * 身份证号
   */
  idCardNo?: string;

  /**
   * 手机号码
   */
  mobile?: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place 编码或文本）
   */
  nativePlace?: string;

  /**
   * 民族（字典 hr_ethnic_group 编码或文本）
   */
  ethnicity?: string;

  /**
   * 政治面貌（字典 hr_political_status 编码或文本）
   */
  politicalStatus?: string;

  /**
   * 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）
   */
  maritalStatus?: number;

  /**
   * 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
   */
  education?: number;

  /**
   * 毕业院校（最高学历摘要）
   */
  graduateSchool?: string;

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
   * 员工编号（租户+公司内唯一）
   */
  employeeNo: string;

  /**
   * 姓名
   */
  name: string;

  /**
   * 性别（0=未知，1=男，2=女）
   */
  gender: number;

  /**
   * 出生日期
   */
  birthDate?: string;

  /**
   * 身份证号
   */
  idCardNo?: string;

  /**
   * 手机号码
   */
  mobile?: string;

  /**
   * 电子邮箱
   */
  email?: string;

  /**
   * 籍贯（字典 hr_native_place 编码或文本）
   */
  nativePlace?: string;

  /**
   * 民族（字典 hr_ethnic_group 编码或文本）
   */
  ethnicity?: string;

  /**
   * 政治面貌（字典 hr_political_status 编码或文本）
   */
  politicalStatus?: string;

  /**
   * 婚姻状况（0=未婚，1=已婚，2=离异，3=丧偶）
   */
  maritalStatus?: number;

  /**
   * 最高学历摘要（1=高中及以下，2=大专，3=本科，4=硕士，5=博士；明细见 EmployeeEducations）
   */
  education?: number;

  /**
   * 毕业院校（最高学历摘要）
   */
  graduateSchool?: string;

  /**
   * 专业（最高学历摘要）
   */
  major?: string;

  /**
   * 实际上岗日期（JoinedDate：入职上班；招聘录用见人才管理 TaktTalentOffer）
   */
  joinedDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 转正日期
   */
  regularDate?: string;

  /**
   * 离职日期
   */
  terminationDate?: string;

  /**
   * 最后工作日
   */
  lastWorkDate?: string;

  /**
   * 离职类型（0=主动辞职，1=公司辞退，2=合同到期，3=退休，9=其他）
   */
  resignationType?: number;

  /**
   * 离职原因
   */
  resignationReason?: string;

  /**
   * 员工状态（1=试用期，2=正式，3=离职，4=退休）
   */
  employeeStatus: number;

  /**
   * 当前主部门ID（任职快照，与最新已生效上岗单同步）
   */
  primaryDeptId?: string;

  /**
   * 当前主岗位ID（任职快照）
   */
  primaryPostId?: string;

  /**
   * 是否内置（种子员工不可删）
   */
  isBuiltIn: number;

  /**
   * 紧急联系人姓名
   */
  emergencyContactName?: string;

  /**
   * 紧急联系人电话
   */
  emergencyContactPhone?: string;

  /**
   * 家庭住址
   */
  homeAddress?: string;

  /**
   * 照片URL
   */
  photoUrl?: string;

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

