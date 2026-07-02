// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/talent
// 文件名称：talent-staffing-requirement.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/talent 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common';

import type {
  Dept
} from '@/types/human-resource/organization/dept';
import type {
  Post
} from '@/types/human-resource/organization/post';
import type {
  Employee
} from '@/types/human-resource/personnel/employee';

/**
 * 用人需求（审批单；状态见 TaktApprovalEntityBase.ApprovalStatus）
 * 对应前端 TaktTalentStaffingRequirementDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 TalentStaffingRequirement
 * @description 对应后端 TaktTalentStaffingRequirementDto
 */
export interface TalentStaffingRequirement extends ApprovalDtoBase {
  /**
   * TalentStaffingRequirementID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  talentStaffingRequirementId: string;

  /**
   * 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
   */
  reqNo: string;

  /**
   * 申请部门ID（DeptID，FK→TaktDept）
   */
  deptId: string;

  /**
   * 申请部门名称（填充字段）
   */
  deptName?: string;

  /**
   * 申请岗位ID（PositionID，FK→TaktPost）
   */
  postId: string;

  /**
   * 申请岗位名称（填充字段）
   */
  postName?: string;

  /**
   * 职级（JobGrade/Rank，如专员/主任/工程师）
   */
  jobGrade?: string;

  /**
   * 需求人数（RequestQty，默认 1）
   */
  requestQty: number;

  /**
   * 编制类型（HeadcountType：正式/派遣/实习生/临时）
   */
  headcountType: string;

  /**
   * 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
   */
  reasonCode: string;

  /**
   * 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
   */
  replaceEmployeeId?: string;

  /**
   * 替补员工名称（填充字段）
   */
  replaceEmployeeName?: string;

  /**
   * 期望入职日（ExpectedOnboardDate）
   */
  expectedOnboardDate?: string;

  /**
   * 合同类型（ContractType：固定期/无固定/实习协议）
   */
  contractType?: string;

  /**
   * 工作地点（WorkLocation，如工厂/分公司）
   */
  workLocation?: string;

  /**
   * 岗位职责（JobDesc）
   */
  jobDesc?: string;

  /**
   * 任职要求（Qualification，学历/经验/技能）
   */
  qualification?: string;

  /**
   * 预算年度（BudgetYear，用于 headcount 控制）
   */
  budgetYear?: string;

  /**
   * 申请部门 （主表：TaktDept）
   */
  dept?: Dept;

  /**
   * 申请岗位 （主表：TaktPost）
   */
  post?: Post;

  /**
   * 替补员工 （主表：TaktEmployee）
   */
  replaceEmployee?: Employee;

}


/**
 * TalentStaffingRequirement 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TalentStaffingRequirementQuery
 * @description 对应后端 TaktTalentStaffingRequirementQueryDto
 */
export interface TalentStaffingRequirementQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
   */
  reqNo?: string;

  /**
   * 申请部门ID（DeptID，FK→TaktDept）
   */
  deptId?: string;

  /**
   * 申请岗位ID（PositionID，FK→TaktPost）
   */
  postId?: string;

  /**
   * 职级（JobGrade/Rank，如专员/主任/工程师）
   */
  jobGrade?: string;

  /**
   * 需求人数（RequestQty，默认 1）
   */
  requestQty?: number;

  /**
   * 编制类型（HeadcountType：正式/派遣/实习生/临时）
   */
  headcountType?: string;

  /**
   * 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
   */
  reasonCode?: string;

  /**
   * 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
   */
  replaceEmployeeId?: string;

  /**
   * 期望入职日（ExpectedOnboardDate）（范围查询-开始）
   */
  expectedOnboardDateStart?: string;

  /**
   * 期望入职日（ExpectedOnboardDate）（范围查询-结束）
   */
  expectedOnboardDateEnd?: string;

  /**
   * 合同类型（ContractType：固定期/无固定/实习协议）
   */
  contractType?: string;

  /**
   * 工作地点（WorkLocation，如工厂/分公司）
   */
  workLocation?: string;

  /**
   * 岗位职责（JobDesc）
   */
  jobDesc?: string;

  /**
   * 任职要求（Qualification，学历/经验/技能）
   */
  qualification?: string;

  /**
   * 预算年度（BudgetYear，用于 headcount 控制）
   */
  budgetYear?: string;

  /**
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建TalentStaffingRequirement DTO
 * 对应前端 TalentStaffingRequirementCreate
 * @description 对应后端 TaktTalentStaffingRequirementCreateDto
 */
export interface TalentStaffingRequirementCreate {
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
   * 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
   */
  reqNo: string;

  /**
   * 申请部门ID（DeptID，FK→TaktDept）
   */
  deptId: string;

  /**
   * 申请岗位ID（PositionID，FK→TaktPost）
   */
  postId: string;

  /**
   * 职级（JobGrade/Rank，如专员/主任/工程师）
   */
  jobGrade?: string;

  /**
   * 需求人数（RequestQty，默认 1）
   */
  requestQty: number;

  /**
   * 编制类型（HeadcountType：正式/派遣/实习生/临时）
   */
  headcountType: string;

  /**
   * 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
   */
  reasonCode: string;

  /**
   * 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
   */
  replaceEmployeeId?: string;

  /**
   * 期望入职日（ExpectedOnboardDate）
   */
  expectedOnboardDate?: string;

  /**
   * 合同类型（ContractType：固定期/无固定/实习协议）
   */
  contractType?: string;

  /**
   * 工作地点（WorkLocation，如工厂/分公司）
   */
  workLocation?: string;

  /**
   * 岗位职责（JobDesc）
   */
  jobDesc?: string;

  /**
   * 任职要求（Qualification，学历/经验/技能）
   */
  qualification?: string;

  /**
   * 预算年度（BudgetYear，用于 headcount 控制）
   */
  budgetYear?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新TalentStaffingRequirement DTO
 * 继承 TaktTalentStaffingRequirementCreateDto，添加 TalentStaffingRequirementId 字段
 * 对应前端 TalentStaffingRequirementUpdate
 * @description 对应后端 TaktTalentStaffingRequirementUpdateDto
 */
export interface TalentStaffingRequirementUpdate extends TalentStaffingRequirementCreate {
  /**
   * TalentStaffingRequirementID（标识要更新的实体）
   */
  talentStaffingRequirementId: string;

}


/**
 * TalentStaffingRequirement 导入模板行 DTO
 * 对应前端 TalentStaffingRequirementTemplate
 * @description 对应后端 TaktTalentStaffingRequirementTemplateDto
 */
export interface TalentStaffingRequirementTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
   */
  reqNo?: string;

  /**
   * 申请部门ID（DeptID，FK→TaktDept）
   */
  deptId?: string;

  /**
   * 申请岗位ID（PositionID，FK→TaktPost）
   */
  postId?: string;

  /**
   * 职级（JobGrade/Rank，如专员/主任/工程师）
   */
  jobGrade?: string;

  /**
   * 需求人数（RequestQty，默认 1）
   */
  requestQty?: number;

  /**
   * 编制类型（HeadcountType：正式/派遣/实习生/临时）
   */
  headcountType?: string;

  /**
   * 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
   */
  reasonCode?: string;

  /**
   * 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
   */
  replaceEmployeeId?: string;

  /**
   * 合同类型（ContractType：固定期/无固定/实习协议）
   */
  contractType?: string;

  /**
   * 工作地点（WorkLocation，如工厂/分公司）
   */
  workLocation?: string;

  /**
   * 岗位职责（JobDesc）
   */
  jobDesc?: string;

  /**
   * 任职要求（Qualification，学历/经验/技能）
   */
  qualification?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * TalentStaffingRequirement 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TalentStaffingRequirementImport
 * @description 对应后端 TaktTalentStaffingRequirementImportDto
 */
export interface TalentStaffingRequirementImport {
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
   * 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
   */
  reqNo?: string;

  /**
   * 申请部门ID（DeptID，FK→TaktDept）
   */
  deptId?: string;

  /**
   * 申请岗位ID（PositionID，FK→TaktPost）
   */
  postId?: string;

  /**
   * 职级（JobGrade/Rank，如专员/主任/工程师）
   */
  jobGrade?: string;

  /**
   * 需求人数（RequestQty，默认 1）
   */
  requestQty?: number;

  /**
   * 编制类型（HeadcountType：正式/派遣/实习生/临时）
   */
  headcountType?: string;

  /**
   * 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
   */
  reasonCode?: string;

  /**
   * 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
   */
  replaceEmployeeId?: string;

  /**
   * 合同类型（ContractType：固定期/无固定/实习协议）
   */
  contractType?: string;

  /**
   * 工作地点（WorkLocation，如工厂/分公司）
   */
  workLocation?: string;

  /**
   * 岗位职责（JobDesc）
   */
  jobDesc?: string;

  /**
   * 任职要求（Qualification，学历/经验/技能）
   */
  qualification?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * TalentStaffingRequirement 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TalentStaffingRequirementExport
 * @description 对应后端 TaktTalentStaffingRequirementExportDto
 */
export interface TalentStaffingRequirementExport {
  /**
   * TalentStaffingRequirementID
   */
  talentStaffingRequirementId: string;

  /**
   * 需求单号（ReqNo，租户+公司内唯一；自动生成，如 PR-2026-00123）
   */
  reqNo: string;

  /**
   * 申请部门ID（DeptID，FK→TaktDept）
   */
  deptId: string;

  /**
   * 申请岗位ID（PositionID，FK→TaktPost）
   */
  postId: string;

  /**
   * 职级（JobGrade/Rank，如专员/主任/工程师）
   */
  jobGrade?: string;

  /**
   * 需求人数（RequestQty，默认 1）
   */
  requestQty: number;

  /**
   * 编制类型（HeadcountType：正式/派遣/实习生/临时）
   */
  headcountType: string;

  /**
   * 需求原因（ReasonCode：新增编制/离职补充/业务扩大/替岗）
   */
  reasonCode: string;

  /**
   * 替补员工ID（ReplaceEmpID，离职补充时填原员工，FK→TaktEmployee，可空）
   */
  replaceEmployeeId?: string;

  /**
   * 期望入职日（ExpectedOnboardDate）
   */
  expectedOnboardDate?: string;

  /**
   * 合同类型（ContractType：固定期/无固定/实习协议）
   */
  contractType?: string;

  /**
   * 工作地点（WorkLocation，如工厂/分公司）
   */
  workLocation?: string;

  /**
   * 岗位职责（JobDesc）
   */
  jobDesc?: string;

  /**
   * 任职要求（Qualification，学历/经验/技能）
   */
  qualification?: string;

  /**
   * 预算年度（BudgetYear，用于 headcount 控制）
   */
  budgetYear?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

