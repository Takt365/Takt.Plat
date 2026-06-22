// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sop-workstation.d.ts
// 创建时间：2026-06-15
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * SOP 工位主数据实体
 * 对应前端 TaktSopWorkstationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopWorkstation
 * @description 对应后端 TaktSopWorkstationDto
 */
export interface SopWorkstation extends CompanyDtoBase {
  /**
   * SopWorkstationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopWorkstationId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 工位编码（工厂内唯一）
   */
  workstationCode: string;

  /**
   * 工位名称
   */
  workstationName: string;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 工位类型（1=装配，2=检验，3=包装，4=测试，5=其他；字典 sys_workstation_type）
   */
  workstationType: number;

  /**
   * 工艺段类型（1=SMT，2=自插，3=手插，4=修正，5=总装；字典 logistics_process_segment_type）
   */
  processSegmentType: number;

  /**
   * 启用状态（字典 sys_normal_disable_status，0=停用，1=启用）
   */
  workstationStatus: number;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * SopWorkstation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopWorkstationQuery
 * @description 对应后端 TaktSopWorkstationQueryDto
 */
export interface SopWorkstationQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 工位编码（工厂内唯一）
   */
  workstationCode?: string;

  /**
   * 工位名称
   */
  workstationName?: string;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 工位类型（1=装配，2=检验，3=包装，4=测试，5=其他；字典 sys_workstation_type）
   */
  workstationType?: number;

  /**
   * 工艺段类型（1=SMT，2=自插，3=手插，4=修正，5=总装；字典 logistics_process_segment_type）
   */
  processSegmentType?: number;

  /**
   * 启用状态（字典 sys_normal_disable_status，0=停用，1=启用）
   */
  workstationStatus?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

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
 * 创建SopWorkstation DTO
 * 对应前端 SopWorkstationCreate
 * @description 对应后端 TaktSopWorkstationCreateDto
 */
export interface SopWorkstationCreate {
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
   * 工厂代码
   */
  plantCode: string;

  /**
   * 工位编码（工厂内唯一）
   */
  workstationCode: string;

  /**
   * 工位名称
   */
  workstationName: string;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 工位类型（1=装配，2=检验，3=包装，4=测试，5=其他；字典 sys_workstation_type）
   */
  workstationType: number;

  /**
   * 工艺段类型（1=SMT，2=自插，3=手插，4=修正，5=总装；字典 logistics_process_segment_type）
   */
  processSegmentType: number;

  /**
   * 启用状态（字典 sys_normal_disable_status，0=停用，1=启用）
   */
  workstationStatus: number;

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
 * 更新SopWorkstation DTO
 * 继承 TaktSopWorkstationCreateDto，添加 SopWorkstationId 字段
 * 对应前端 SopWorkstationUpdate
 * @description 对应后端 TaktSopWorkstationUpdateDto
 */
export interface SopWorkstationUpdate extends SopWorkstationCreate {
  /**
   * SopWorkstationID（标识要更新的实体）
   */
  sopWorkstationId: string;

}


/**
 * SopWorkstation 状态更新 DTO
 * 对应前端 SopWorkstationStatus
 * @description 对应后端 TaktSopWorkstationStatusDto
 */
export interface SopWorkstationStatus {
  /**
   * SopWorkstationID
   */
  sopWorkstationId: string;

  /**
   * 启用状态（字典 sys_normal_disable_status，0=停用，1=启用）
   */
  workstationStatus: number;

}


/**
 * SopWorkstation 排序更新 DTO
 * 对应前端 SopWorkstationSort
 * @description 对应后端 TaktSopWorkstationSortDto
 */
export interface SopWorkstationSort {
  /**
   * SopWorkstationID
   */
  sopWorkstationId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * SopWorkstation 导入模板行 DTO
 * 对应前端 SopWorkstationTemplate
 * @description 对应后端 TaktSopWorkstationTemplateDto
 */
export interface SopWorkstationTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 工位编码（工厂内唯一）
   */
  workstationCode?: string;

  /**
   * 工位名称
   */
  workstationName?: string;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 工位类型（1=装配，2=检验，3=包装，4=测试，5=其他；字典 sys_workstation_type）
   */
  workstationType?: number;

  /**
   * 工艺段类型（1=SMT，2=自插，3=手插，4=修正，5=总装；字典 logistics_process_segment_type）
   */
  processSegmentType?: number;

  /**
   * 启用状态（字典 sys_normal_disable_status，0=停用，1=启用）
   */
  workstationStatus?: number;

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
 * SopWorkstation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopWorkstationImport
 * @description 对应后端 TaktSopWorkstationImportDto
 */
export interface SopWorkstationImport {
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
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 工位编码（工厂内唯一）
   */
  workstationCode?: string;

  /**
   * 工位名称
   */
  workstationName?: string;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 工位类型（1=装配，2=检验，3=包装，4=测试，5=其他；字典 sys_workstation_type）
   */
  workstationType?: number;

  /**
   * 工艺段类型（1=SMT，2=自插，3=手插，4=修正，5=总装；字典 logistics_process_segment_type）
   */
  processSegmentType?: number;

  /**
   * 启用状态（字典 sys_normal_disable_status，0=停用，1=启用）
   */
  workstationStatus?: number;

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
 * SopWorkstation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopWorkstationExport
 * @description 对应后端 TaktSopWorkstationExportDto
 */
export interface SopWorkstationExport {
  /**
   * SopWorkstationID
   */
  sopWorkstationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 工位编码（工厂内唯一）
   */
  workstationCode: string;

  /**
   * 工位名称
   */
  workstationName: string;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 工位类型（1=装配，2=检验，3=包装，4=测试，5=其他；字典 sys_workstation_type）
   */
  workstationType: number;

  /**
   * 工艺段类型（1=SMT，2=自插，3=手插，4=修正，5=总装；字典 logistics_process_segment_type）
   */
  processSegmentType: number;

  /**
   * 启用状态（字典 sys_normal_disable_status，0=停用，1=启用）
   */
  workstationStatus: number;

  /**
   * 排序号
   */
  sortOrder: number;

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

