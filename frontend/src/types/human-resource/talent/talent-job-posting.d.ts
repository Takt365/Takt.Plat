// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/human-resource/talent
// 文件名称：talent-job-posting.d.ts
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
 * 职位发布（业务发布单，非审批单；状态见 posting_status）
 * 对应前端 TaktTalentJobPostingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TalentJobPosting
 * @description 对应后端 TaktTalentJobPostingDto
 */
export interface TalentJobPosting extends CompanyDtoBase {
  /**
   * TalentJobPostingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  talentJobPostingId: string;

  /**
   * 招聘计划ID
   */
  staffingRequirementId: string;

  /**
   * 招聘计划名称（填充字段）
   */
  staffingRequirementName?: string;

  /**
   * 发布编号（租户+公司内唯一）
   */
  postingCode: string;

  /**
   * 职位标题
   */
  title: string;

  /**
   * 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
   */
  postingStatus: number;

  /**
   * 职位发布日期
   */
  publishDate: string;

  /**
   * 招聘开放日期
   */
  openDate: string;

  /**
   * 招聘关闭日期
   */
  closeDate?: string;

  /**
   * 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
   */
  publishChannel: number;

  /**
   * 发布说明
   */
  reason?: string;

}


/**
 * TalentJobPosting 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TalentJobPostingQuery
 * @description 对应后端 TaktTalentJobPostingQueryDto
 */
export interface TalentJobPostingQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 招聘计划ID
   */
  staffingRequirementId?: string;

  /**
   * 发布编号（租户+公司内唯一）
   */
  postingCode?: string;

  /**
   * 职位标题
   */
  title?: string;

  /**
   * 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
   */
  postingStatus?: number;

  /**
   * 职位发布日期（范围查询-开始）
   */
  publishDateStart?: string;

  /**
   * 职位发布日期（范围查询-结束）
   */
  publishDateEnd?: string;

  /**
   * 招聘开放日期（范围查询-开始）
   */
  openDateStart?: string;

  /**
   * 招聘开放日期（范围查询-结束）
   */
  openDateEnd?: string;

  /**
   * 招聘关闭日期（范围查询-开始）
   */
  closeDateStart?: string;

  /**
   * 招聘关闭日期（范围查询-结束）
   */
  closeDateEnd?: string;

  /**
   * 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
   */
  publishChannel?: number;

  /**
   * 发布说明
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
 * 创建TalentJobPosting DTO
 * 对应前端 TalentJobPostingCreate
 * @description 对应后端 TaktTalentJobPostingCreateDto
 */
export interface TalentJobPostingCreate {
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
   * 招聘计划ID
   */
  staffingRequirementId: string;

  /**
   * 发布编号（租户+公司内唯一）
   */
  postingCode: string;

  /**
   * 职位标题
   */
  title: string;

  /**
   * 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
   */
  postingStatus: number;

  /**
   * 职位发布日期
   */
  publishDate: string;

  /**
   * 招聘开放日期
   */
  openDate: string;

  /**
   * 招聘关闭日期
   */
  closeDate?: string;

  /**
   * 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
   */
  publishChannel: number;

  /**
   * 发布说明
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
 * 更新TalentJobPosting DTO
 * 继承 TaktTalentJobPostingCreateDto，添加 TalentJobPostingId 字段
 * 对应前端 TalentJobPostingUpdate
 * @description 对应后端 TaktTalentJobPostingUpdateDto
 */
export interface TalentJobPostingUpdate extends TalentJobPostingCreate {
  /**
   * TalentJobPostingID（标识要更新的实体）
   */
  talentJobPostingId: string;

}


/**
 * TalentJobPosting 状态更新 DTO
 * 对应前端 TalentJobPostingStatus
 * @description 对应后端 TaktTalentJobPostingStatusDto
 */
export interface TalentJobPostingStatus {
  /**
   * TalentJobPostingID
   */
  talentJobPostingId: string;

  /**
   * 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
   */
  postingStatus: number;

}


/**
 * TalentJobPosting 导入模板行 DTO
 * 对应前端 TalentJobPostingTemplate
 * @description 对应后端 TaktTalentJobPostingTemplateDto
 */
export interface TalentJobPostingTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 招聘计划ID
   */
  staffingRequirementId?: string;

  /**
   * 发布编号（租户+公司内唯一）
   */
  postingCode?: string;

  /**
   * 职位标题
   */
  title?: string;

  /**
   * 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
   */
  postingStatus?: number;

  /**
   * 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
   */
  publishChannel?: number;

  /**
   * 发布说明
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
 * TalentJobPosting 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TalentJobPostingImport
 * @description 对应后端 TaktTalentJobPostingImportDto
 */
export interface TalentJobPostingImport {
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
   * 招聘计划ID
   */
  staffingRequirementId?: string;

  /**
   * 发布编号（租户+公司内唯一）
   */
  postingCode?: string;

  /**
   * 职位标题
   */
  title?: string;

  /**
   * 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
   */
  postingStatus?: number;

  /**
   * 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
   */
  publishChannel?: number;

  /**
   * 发布说明
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
 * TalentJobPosting 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TalentJobPostingExport
 * @description 对应后端 TaktTalentJobPostingExportDto
 */
export interface TalentJobPostingExport {
  /**
   * TalentJobPostingID
   */
  talentJobPostingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 招聘计划ID
   */
  staffingRequirementId: string;

  /**
   * 发布编号（租户+公司内唯一）
   */
  postingCode: string;

  /**
   * 职位标题
   */
  title: string;

  /**
   * 发布状态（0=草稿，1=招聘中，2=已暂停，3=已关闭）
   */
  postingStatus: number;

  /**
   * 职位发布日期
   */
  publishDate: string;

  /**
   * 招聘开放日期
   */
  openDate: string;

  /**
   * 招聘关闭日期
   */
  closeDate?: string;

  /**
   * 发布渠道（0=官网，1=招聘网站，2=内推，3=校园，9=其他）
   */
  publishChannel: number;

  /**
   * 发布说明
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

