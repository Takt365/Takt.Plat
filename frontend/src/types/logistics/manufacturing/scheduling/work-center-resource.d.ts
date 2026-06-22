// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/scheduling
// 文件名称：work-center-resource.d.ts
// 创建时间：2026-06-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/scheduling 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 工作中心资源（设备/人员/模具等）
 * 对应前端 TaktWorkCenterResourceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 WorkCenterResource
 * @description 对应后端 TaktWorkCenterResourceDto
 */
export interface WorkCenterResource extends CompanyDtoBase {
  /**
   * WorkCenterResourceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  workCenterResourceId: string;

  /**
   * 工作中心 ID（主子表关系）
   */
  workCenterId: string;

  /**
   * 工作中心 名称（填充字段）
   */
  workCenterName?: string;

  /**
   * 工作中心编码（冗余）
   */
  workCenterCode: string;

  /**
   * 资源编码
   */
  resourceCode: string;

  /**
   * 资源名称
   */
  resourceName: string;

  /**
   * 资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）
   */
  resourceType: number;

  /**
   * 并行能力（可同时加工任务数）
   */
  parallelCapacity: number;

  /**
   * 效率系数（1.0=标准）
   */
  efficiencyRate: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  resourceStatus: number;

}


/**
 * WorkCenterResource 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 WorkCenterResourceQuery
 * @description 对应后端 TaktWorkCenterResourceQueryDto
 */
export interface WorkCenterResourceQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工作中心 ID（主子表关系）
   */
  workCenterId?: string;

  /**
   * 工作中心编码（冗余）
   */
  workCenterCode?: string;

  /**
   * 资源编码
   */
  resourceCode?: string;

  /**
   * 资源名称
   */
  resourceName?: string;

  /**
   * 资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）
   */
  resourceType?: number;

  /**
   * 并行能力（可同时加工任务数）
   */
  parallelCapacity?: number;

  /**
   * 效率系数（1.0=标准）
   */
  efficiencyRate?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  resourceStatus?: number;

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
 * 创建WorkCenterResource DTO
 * 对应前端 WorkCenterResourceCreate
 * @description 对应后端 TaktWorkCenterResourceCreateDto
 */
export interface WorkCenterResourceCreate {
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
   * 工作中心 ID（主子表关系）
   */
  workCenterId: string;

  /**
   * 工作中心编码（冗余）
   */
  workCenterCode: string;

  /**
   * 资源编码
   */
  resourceCode: string;

  /**
   * 资源名称
   */
  resourceName: string;

  /**
   * 资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）
   */
  resourceType: number;

  /**
   * 并行能力（可同时加工任务数）
   */
  parallelCapacity: number;

  /**
   * 效率系数（1.0=标准）
   */
  efficiencyRate: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  resourceStatus: number;

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
 * 更新WorkCenterResource DTO
 * 继承 TaktWorkCenterResourceCreateDto，添加 WorkCenterResourceId 字段
 * 对应前端 WorkCenterResourceUpdate
 * @description 对应后端 TaktWorkCenterResourceUpdateDto
 */
export interface WorkCenterResourceUpdate extends WorkCenterResourceCreate {
  /**
   * WorkCenterResourceID（标识要更新的实体）
   */
  workCenterResourceId: string;

}


/**
 * WorkCenterResource 状态更新 DTO
 * 对应前端 WorkCenterResourceStatus
 * @description 对应后端 TaktWorkCenterResourceStatusDto
 */
export interface WorkCenterResourceStatus {
  /**
   * WorkCenterResourceID
   */
  workCenterResourceId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  resourceStatus: number;

}


/**
 * WorkCenterResource 导入模板行 DTO
 * 对应前端 WorkCenterResourceTemplate
 * @description 对应后端 TaktWorkCenterResourceTemplateDto
 */
export interface WorkCenterResourceTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工作中心 ID（主子表关系）
   */
  workCenterId?: string;

  /**
   * 工作中心编码（冗余）
   */
  workCenterCode?: string;

  /**
   * 资源编码
   */
  resourceCode?: string;

  /**
   * 资源名称
   */
  resourceName?: string;

  /**
   * 资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）
   */
  resourceType?: number;

  /**
   * 并行能力（可同时加工任务数）
   */
  parallelCapacity?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  resourceStatus?: number;

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
 * WorkCenterResource 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 WorkCenterResourceImport
 * @description 对应后端 TaktWorkCenterResourceImportDto
 */
export interface WorkCenterResourceImport {
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
   * 工作中心 ID（主子表关系）
   */
  workCenterId?: string;

  /**
   * 工作中心编码（冗余）
   */
  workCenterCode?: string;

  /**
   * 资源编码
   */
  resourceCode?: string;

  /**
   * 资源名称
   */
  resourceName?: string;

  /**
   * 资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）
   */
  resourceType?: number;

  /**
   * 并行能力（可同时加工任务数）
   */
  parallelCapacity?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  resourceStatus?: number;

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
 * WorkCenterResource 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 WorkCenterResourceExport
 * @description 对应后端 TaktWorkCenterResourceExportDto
 */
export interface WorkCenterResourceExport {
  /**
   * WorkCenterResourceID
   */
  workCenterResourceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工作中心 ID（主子表关系）
   */
  workCenterId: string;

  /**
   * 工作中心编码（冗余）
   */
  workCenterCode: string;

  /**
   * 资源编码
   */
  resourceCode: string;

  /**
   * 资源名称
   */
  resourceName: string;

  /**
   * 资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）
   */
  resourceType: number;

  /**
   * 并行能力（可同时加工任务数）
   */
  parallelCapacity: number;

  /**
   * 效率系数（1.0=标准）
   */
  efficiencyRate: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  resourceStatus: number;

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

