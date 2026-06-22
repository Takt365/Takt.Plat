// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：production-team.d.ts
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 生产班组实体（生产线班组主数据）
 * 对应前端 TaktProductionTeamDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionTeam
 * @description 对应后端 TaktProductionTeamDto
 */
export interface ProductionTeam extends CompanyDtoBase {
  /**
   * ProductionTeamID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  productionTeamId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
   */
  teamCode: string;

  /**
   * 班组名称（显示名称，如：SMT一班、手插二班等）
   */
  teamName: string;

  /**
   * 班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）
   */
  teamCategory?: string;

  /**
   * 班组分类名称（如：组立、PCBA、SMT、质检等）
   */
  teamCategoryName?: string;

  /**
   * 生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）
   */
  productionLine?: string;

  /**
   * 班组长员工Id
   */
  teamLeaderId?: string;

  /**
   * 班组长姓名
   */
  teamLeaderName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo?: number;

  /**
   * 启用状态（1=启用，0=禁用）
   */
  status: number;

}


/**
 * ProductionTeam 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProductionTeamQuery
 * @description 对应后端 TaktProductionTeamQueryDto
 */
export interface ProductionTeamQuery extends TaktPagedQuery {
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
   * 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
   */
  teamCode?: string;

  /**
   * 班组名称（显示名称，如：SMT一班、手插二班等）
   */
  teamName?: string;

  /**
   * 班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）
   */
  teamCategory?: string;

  /**
   * 班组分类名称（如：组立、PCBA、SMT、质检等）
   */
  teamCategoryName?: string;

  /**
   * 生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）
   */
  productionLine?: string;

  /**
   * 班组长员工Id
   */
  teamLeaderId?: string;

  /**
   * 班组长姓名
   */
  teamLeaderName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo?: number;

  /**
   * 启用状态（1=启用，0=禁用）
   */
  status?: number;

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
 * 创建ProductionTeam DTO
 * 对应前端 ProductionTeamCreate
 * @description 对应后端 TaktProductionTeamCreateDto
 */
export interface ProductionTeamCreate {
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
   * 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
   */
  teamCode: string;

  /**
   * 班组名称（显示名称，如：SMT一班、手插二班等）
   */
  teamName: string;

  /**
   * 班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）
   */
  teamCategory?: string;

  /**
   * 班组分类名称（如：组立、PCBA、SMT、质检等）
   */
  teamCategoryName?: string;

  /**
   * 生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）
   */
  productionLine?: string;

  /**
   * 班组长员工Id
   */
  teamLeaderId?: string;

  /**
   * 班组长姓名
   */
  teamLeaderName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo?: number;

  /**
   * 启用状态（1=启用，0=禁用）
   */
  status: number;

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
 * 更新ProductionTeam DTO
 * 继承 TaktProductionTeamCreateDto，添加 ProductionTeamId 字段
 * 对应前端 ProductionTeamUpdate
 * @description 对应后端 TaktProductionTeamUpdateDto
 */
export interface ProductionTeamUpdate extends ProductionTeamCreate {
  /**
   * ProductionTeamID（标识要更新的实体）
   */
  productionTeamId: string;

}


/**
 * ProductionTeam 状态更新 DTO
 * 对应前端 ProductionTeamStatus
 * @description 对应后端 TaktProductionTeamStatusDto
 */
export interface ProductionTeamStatus {
  /**
   * ProductionTeamID
   */
  productionTeamId: string;

  /**
   * 启用状态（1=启用，0=禁用）
   */
  status: number;

}


/**
 * ProductionTeam 导入模板行 DTO
 * 对应前端 ProductionTeamTemplate
 * @description 对应后端 TaktProductionTeamTemplateDto
 */
export interface ProductionTeamTemplate {
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
   * 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
   */
  teamCode?: string;

  /**
   * 班组名称（显示名称，如：SMT一班、手插二班等）
   */
  teamName?: string;

  /**
   * 班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）
   */
  teamCategory?: string;

  /**
   * 班组分类名称（如：组立、PCBA、SMT、质检等）
   */
  teamCategoryName?: string;

  /**
   * 生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）
   */
  productionLine?: string;

  /**
   * 班组长员工Id
   */
  teamLeaderId?: string;

  /**
   * 班组长姓名
   */
  teamLeaderName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo?: number;

  /**
   * 启用状态（1=启用，0=禁用）
   */
  status?: number;

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
 * ProductionTeam 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProductionTeamImport
 * @description 对应后端 TaktProductionTeamImportDto
 */
export interface ProductionTeamImport {
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
   * 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
   */
  teamCode?: string;

  /**
   * 班组名称（显示名称，如：SMT一班、手插二班等）
   */
  teamName?: string;

  /**
   * 班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）
   */
  teamCategory?: string;

  /**
   * 班组分类名称（如：组立、PCBA、SMT、质检等）
   */
  teamCategoryName?: string;

  /**
   * 生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）
   */
  productionLine?: string;

  /**
   * 班组长员工Id
   */
  teamLeaderId?: string;

  /**
   * 班组长姓名
   */
  teamLeaderName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo?: number;

  /**
   * 启用状态（1=启用，0=禁用）
   */
  status?: number;

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
 * ProductionTeam 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionTeamExport
 * @description 对应后端 TaktProductionTeamExportDto
 */
export interface ProductionTeamExport {
  /**
   * ProductionTeamID
   */
  productionTeamId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
   */
  teamCode: string;

  /**
   * 班组名称（显示名称，如：SMT一班、手插二班等）
   */
  teamName: string;

  /**
   * 班组分类编码（M=组立，P=PCBA，S=SMT，Q=质检，O=其他）
   */
  teamCategory?: string;

  /**
   * 班组分类名称（如：组立、PCBA、SMT、质检等）
   */
  teamCategoryName?: string;

  /**
   * 生产线代码（如：SMT1、ASSY1 等，与 TeamCode 区分，TeamCode 可包含班组信息）
   */
  productionLine?: string;

  /**
   * 班组长员工Id
   */
  teamLeaderId?: string;

  /**
   * 班组长姓名
   */
  teamLeaderName?: string;

  /**
   * 班次（1=早班，2=中班，3=晚班）
   */
  shiftNo?: number;

  /**
   * 启用状态（1=启用，0=禁用）
   */
  status: number;

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

