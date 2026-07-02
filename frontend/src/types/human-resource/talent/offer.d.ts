// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/talent
// 文件名称：offer.d.ts
// 创建时间：2026-06-23
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

/**
 * 录用信息（审批单，状态见 TaktApprovalEntityBase.ApprovalStatus）
 * 对应前端 TaktTalentOfferDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 TalentOffer
 * @description 对应后端 TaktTalentOfferDto
 */
export interface TalentOffer extends ApprovalDtoBase {
  /**
   * TalentOfferID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  talentOfferId: string;

  /**
   * 职位发布ID（用人需求→职位发布→录用）
   */
  jobPostingId: string;

  /**
   * 职位发布名称（填充字段）
   */
  jobPostingName?: string;

  /**
   * 录用编号（租户+公司内业务编号）
   */
  offerNo: string;

  /**
   * 录用日期（HireDate：确认录用/发 offer）
   */
  hireDate: string;

  /**
   * 关联员工ID（录用通过并建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 关联员工名称（填充字段）
   */
  employeeName?: string;

  /**
   * 拟录用部门ID
   */
  deptId: string;

  /**
   * 拟录用部门名称
   */
  deptName: string;

  /**
   * 拟录用岗位ID
   */
  postId?: string;

  /**
   * 拟录用岗位名称
   */
  postName?: string;

  /**
   * 录用说明
   */
  reason?: string;

  /**
   * 职位发布 （主表：TaktTalentJobPosting）
   */
  jobPosting?: TalentJobPosting;

  /**
   * 入职待办 （子表：TaktEmployeeOnboarding）
   */
  employeeOnboardings?: EmployeeOnboarding[];

}


/**
 * TalentOffer 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TalentOfferQuery
 * @description 对应后端 TaktTalentOfferQueryDto
 */
export interface TalentOfferQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 职位发布ID（用人需求→职位发布→录用）
   */
  jobPostingId?: string;

  /**
   * 录用编号（租户+公司内业务编号）
   */
  offerNo?: string;

  /**
   * 录用日期（HireDate：确认录用/发 offer）（范围查询-开始）
   */
  hireDateStart?: string;

  /**
   * 录用日期（HireDate：确认录用/发 offer）（范围查询-结束）
   */
  hireDateEnd?: string;

  /**
   * 关联员工ID（录用通过并建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 拟录用部门ID
   */
  deptId?: string;

  /**
   * 拟录用部门名称
   */
  deptName?: string;

  /**
   * 拟录用岗位ID
   */
  postId?: string;

  /**
   * 拟录用岗位名称
   */
  postName?: string;

  /**
   * 录用说明
   */
  reason?: string;

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
   * 流程实例 ID
   */
  flowInstanceId?: string;

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
 * 创建TalentOffer DTO
 * 对应前端 TalentOfferCreate
 * @description 对应后端 TaktTalentOfferCreateDto
 */
export interface TalentOfferCreate {
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
   * 职位发布ID（用人需求→职位发布→录用）
   */
  jobPostingId: string;

  /**
   * 录用编号（租户+公司内业务编号）
   */
  offerNo: string;

  /**
   * 录用日期（HireDate：确认录用/发 offer）
   */
  hireDate: string;

  /**
   * 关联员工ID（录用通过并建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 拟录用部门ID
   */
  deptId: string;

  /**
   * 拟录用部门名称
   */
  deptName: string;

  /**
   * 拟录用岗位ID
   */
  postId?: string;

  /**
   * 拟录用岗位名称
   */
  postName?: string;

  /**
   * 录用说明
   */
  reason?: string;

  /**
   * 入职待办（子表，级联保存）
   */
  employeeOnboardings?: EmployeeOnboardingCreate[];

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
 * 更新TalentOffer DTO
 * 继承 TaktTalentOfferCreateDto，添加 TalentOfferId 字段
 * 对应前端 TalentOfferUpdate
 * @description 对应后端 TaktTalentOfferUpdateDto
 */
export interface TalentOfferUpdate extends TalentOfferCreate {
  /**
   * TalentOfferID（标识要更新的实体）
   */
  talentOfferId: string;

}


/**
 * TalentOffer 导入模板行 DTO
 * 对应前端 TalentOfferTemplate
 * @description 对应后端 TaktTalentOfferTemplateDto
 */
export interface TalentOfferTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 职位发布ID（用人需求→职位发布→录用）
   */
  jobPostingId?: string;

  /**
   * 录用编号（租户+公司内业务编号）
   */
  offerNo?: string;

  /**
   * 录用日期（HireDate：确认录用/发 offer）
   */
  hireDate?: string;

  /**
   * 关联员工ID（录用通过并建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 拟录用部门ID
   */
  deptId?: string;

  /**
   * 拟录用部门名称
   */
  deptName?: string;

  /**
   * 拟录用岗位ID
   */
  postId?: string;

  /**
   * 拟录用岗位名称
   */
  postName?: string;

  /**
   * 录用说明
   */
  reason?: string;

  /**
   * 入职待办（子表，级联保存）
   */
  employeeOnboardings?: EmployeeOnboardingCreate[];

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
 * TalentOffer 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TalentOfferImport
 * @description 对应后端 TaktTalentOfferImportDto
 */
export interface TalentOfferImport {
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
   * 职位发布ID（用人需求→职位发布→录用）
   */
  jobPostingId?: string;

  /**
   * 录用编号（租户+公司内业务编号）
   */
  offerNo?: string;

  /**
   * 录用日期（HireDate：确认录用/发 offer）
   */
  hireDate?: string;

  /**
   * 关联员工ID（录用通过并建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 拟录用部门ID
   */
  deptId?: string;

  /**
   * 拟录用部门名称
   */
  deptName?: string;

  /**
   * 拟录用岗位ID
   */
  postId?: string;

  /**
   * 拟录用岗位名称
   */
  postName?: string;

  /**
   * 录用说明
   */
  reason?: string;

  /**
   * 入职待办（子表，级联保存）
   */
  employeeOnboardings?: EmployeeOnboardingCreate[];

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
 * TalentOffer 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TalentOfferExport
 * @description 对应后端 TaktTalentOfferExportDto
 */
export interface TalentOfferExport {
  /**
   * TalentOfferID
   */
  talentOfferId: string;

  /**
   * 职位发布ID（用人需求→职位发布→录用）
   */
  jobPostingId: string;

  /**
   * 录用编号（租户+公司内业务编号）
   */
  offerNo: string;

  /**
   * 录用日期（HireDate：确认录用/发 offer）
   */
  hireDate: string;

  /**
   * 关联员工ID（录用通过并建档后回填，可空）
   */
  employeeId?: string;

  /**
   * 拟录用部门ID
   */
  deptId: string;

  /**
   * 拟录用部门名称
   */
  deptName: string;

  /**
   * 拟录用岗位ID
   */
  postId?: string;

  /**
   * 拟录用岗位名称
   */
  postName?: string;

  /**
   * 录用说明
   */
  reason?: string;

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

