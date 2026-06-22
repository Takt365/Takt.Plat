// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/talent
// 文件名称：talent-interview.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/talent 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 面试安排（业务过程单，非审批单；状态见 interview_status）
 * 对应前端 TaktTalentInterviewDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TalentInterview
 * @description 对应后端 TaktTalentInterviewDto
 */
export interface TalentInterview extends CompanyDtoBase {
  /**
   * TalentInterviewID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  talentInterviewId: string;

  /**
   * 职位发布ID
   */
  jobPostingId: string;

  /**
   * 职位发布名称（填充字段）
   */
  jobPostingName?: string;

  /**
   * 面试单号（租户+公司内业务编号）
   */
  interviewNo: string;

  /**
   * 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
   */
  interviewStatus: number;

  /**
   * 面试轮次（1=初试，2=复试，3=终试）
   */
  interviewRound: number;

  /**
   * 面试时间
   */
  interviewDate: string;

  /**
   * 面试官姓名
   */
  interviewerName?: string;

  /**
   * 候选人姓名
   */
  candidateName: string;

  /**
   * 候选人手机
   */
  mobile?: string;

  /**
   * 候选人邮箱
   */
  email?: string;

  /**
   * 面试地点
   */
  interviewLocation?: string;

  /**
   * 面试说明
   */
  reason?: string;

  /**
   * 职位发布 （主表：TaktTalentJobPosting）
   */
  jobPosting?: TalentJobPosting;

  /**
   * 录用信息 （子表：TaktTalentOffer）
   */
  talentOffers?: TalentOffer[];

}


/**
 * TalentInterview 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TalentInterviewQuery
 * @description 对应后端 TaktTalentInterviewQueryDto
 */
export interface TalentInterviewQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 职位发布ID
   */
  jobPostingId?: string;

  /**
   * 面试单号（租户+公司内业务编号）
   */
  interviewNo?: string;

  /**
   * 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
   */
  interviewStatus?: number;

  /**
   * 面试轮次（1=初试，2=复试，3=终试）
   */
  interviewRound?: number;

  /**
   * 面试时间（范围查询-开始）
   */
  interviewDateStart?: string;

  /**
   * 面试时间（范围查询-结束）
   */
  interviewDateEnd?: string;

  /**
   * 面试官姓名
   */
  interviewerName?: string;

  /**
   * 候选人姓名
   */
  candidateName?: string;

  /**
   * 候选人手机
   */
  mobile?: string;

  /**
   * 候选人邮箱
   */
  email?: string;

  /**
   * 面试地点
   */
  interviewLocation?: string;

  /**
   * 面试说明
   */
  reason?: string;

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
 * 创建TalentInterview DTO
 * 对应前端 TalentInterviewCreate
 * @description 对应后端 TaktTalentInterviewCreateDto
 */
export interface TalentInterviewCreate {
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
   * 职位发布ID
   */
  jobPostingId: string;

  /**
   * 面试单号（租户+公司内业务编号）
   */
  interviewNo: string;

  /**
   * 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
   */
  interviewStatus: number;

  /**
   * 面试轮次（1=初试，2=复试，3=终试）
   */
  interviewRound: number;

  /**
   * 面试时间
   */
  interviewDate: string;

  /**
   * 面试官姓名
   */
  interviewerName?: string;

  /**
   * 候选人姓名
   */
  candidateName: string;

  /**
   * 候选人手机
   */
  mobile?: string;

  /**
   * 候选人邮箱
   */
  email?: string;

  /**
   * 面试地点
   */
  interviewLocation?: string;

  /**
   * 面试说明
   */
  reason?: string;

  /**
   * 录用信息（子表，级联保存）
   */
  talentOffers?: TalentOfferCreate[];

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
 * 更新TalentInterview DTO
 * 继承 TaktTalentInterviewCreateDto，添加 TalentInterviewId 字段
 * 对应前端 TalentInterviewUpdate
 * @description 对应后端 TaktTalentInterviewUpdateDto
 */
export interface TalentInterviewUpdate extends TalentInterviewCreate {
  /**
   * TalentInterviewID（标识要更新的实体）
   */
  talentInterviewId: string;

}


/**
 * TalentInterview 状态更新 DTO
 * 对应前端 TalentInterviewStatus
 * @description 对应后端 TaktTalentInterviewStatusDto
 */
export interface TalentInterviewStatus {
  /**
   * TalentInterviewID
   */
  talentInterviewId: string;

  /**
   * 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
   */
  interviewStatus: number;

}


/**
 * TalentInterview 导入模板行 DTO
 * 对应前端 TalentInterviewTemplate
 * @description 对应后端 TaktTalentInterviewTemplateDto
 */
export interface TalentInterviewTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 职位发布ID
   */
  jobPostingId?: string;

  /**
   * 面试单号（租户+公司内业务编号）
   */
  interviewNo?: string;

  /**
   * 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
   */
  interviewStatus?: number;

  /**
   * 面试轮次（1=初试，2=复试，3=终试）
   */
  interviewRound?: number;

  /**
   * 面试官姓名
   */
  interviewerName?: string;

  /**
   * 候选人姓名
   */
  candidateName?: string;

  /**
   * 候选人手机
   */
  mobile?: string;

  /**
   * 候选人邮箱
   */
  email?: string;

  /**
   * 面试地点
   */
  interviewLocation?: string;

  /**
   * 面试说明
   */
  reason?: string;

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
 * TalentInterview 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TalentInterviewImport
 * @description 对应后端 TaktTalentInterviewImportDto
 */
export interface TalentInterviewImport {
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
   * 职位发布ID
   */
  jobPostingId?: string;

  /**
   * 面试单号（租户+公司内业务编号）
   */
  interviewNo?: string;

  /**
   * 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
   */
  interviewStatus?: number;

  /**
   * 面试轮次（1=初试，2=复试，3=终试）
   */
  interviewRound?: number;

  /**
   * 面试官姓名
   */
  interviewerName?: string;

  /**
   * 候选人姓名
   */
  candidateName?: string;

  /**
   * 候选人手机
   */
  mobile?: string;

  /**
   * 候选人邮箱
   */
  email?: string;

  /**
   * 面试地点
   */
  interviewLocation?: string;

  /**
   * 面试说明
   */
  reason?: string;

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
 * TalentInterview 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TalentInterviewExport
 * @description 对应后端 TaktTalentInterviewExportDto
 */
export interface TalentInterviewExport {
  /**
   * TalentInterviewID
   */
  talentInterviewId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 职位发布ID
   */
  jobPostingId: string;

  /**
   * 面试单号（租户+公司内业务编号）
   */
  interviewNo: string;

  /**
   * 面试办理状态（0=草稿，1=已安排，2=已完成，3=未通过，4=已取消）
   */
  interviewStatus: number;

  /**
   * 面试轮次（1=初试，2=复试，3=终试）
   */
  interviewRound: number;

  /**
   * 面试时间
   */
  interviewDate: string;

  /**
   * 面试官姓名
   */
  interviewerName?: string;

  /**
   * 候选人姓名
   */
  candidateName: string;

  /**
   * 候选人手机
   */
  mobile?: string;

  /**
   * 候选人邮箱
   */
  email?: string;

  /**
   * 面试地点
   */
  interviewLocation?: string;

  /**
   * 面试说明
   */
  reason?: string;

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

