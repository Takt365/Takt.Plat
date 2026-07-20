// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：work-center.d.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 工作中心（WC；PlantCode 对齐 TaktCalendar.RelatedPlant）
 * 对应前端 TaktWorkCenterDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 WorkCenter
 * @description 对应后端 TaktWorkCenterDto
 */
export interface WorkCenter extends CompanyDtoBase {
  /**
   * WorkCenterID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  workCenterId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工作中心编码
   */
  workCenterCode: string;

  /**
   * 工作中心名称
   */
  workCenterName: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  workCenterStatus: number;

  /**
   * 工作中心资源列表 （子表：TaktWorkCenterResource）
   */
  resources?: WorkCenterResource[];

}


/**
 * WorkCenter 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 WorkCenterQuery
 * @description 对应后端 TaktWorkCenterQueryDto
 */
export interface WorkCenterQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 工作中心名称
   */
  workCenterName?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  workCenterStatus?: number;

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
 * 创建WorkCenter DTO
 * 对应前端 WorkCenterCreate
 * @description 对应后端 TaktWorkCenterCreateDto
 */
export interface WorkCenterCreate {
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
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工作中心编码
   */
  workCenterCode: string;

  /**
   * 工作中心名称
   */
  workCenterName: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  workCenterStatus: number;

  /**
   * 工作中心资源列表（子表，级联保存）
   */
  resources?: WorkCenterResourceCreate[];

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
 * 更新WorkCenter DTO
 * 继承 TaktWorkCenterCreateDto，添加 WorkCenterId 字段
 * 对应前端 WorkCenterUpdate
 * @description 对应后端 TaktWorkCenterUpdateDto
 */
export interface WorkCenterUpdate extends WorkCenterCreate {
  /**
   * WorkCenterID（标识要更新的实体）
   */
  workCenterId: string;

  /**
   * 工作中心资源列表（子表，级联保存）
   */
  resources?: any;

}


/**
 * WorkCenter 状态更新 DTO
 * 对应前端 WorkCenterStatus
 * @description 对应后端 TaktWorkCenterStatusDto
 */
export interface WorkCenterStatus {
  /**
   * WorkCenterID
   */
  workCenterId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  workCenterStatus: number;

}


/**
 * WorkCenter 导入模板行 DTO
 * 对应前端 WorkCenterTemplate
 * @description 对应后端 TaktWorkCenterTemplateDto
 */
export interface WorkCenterTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 工作中心名称
   */
  workCenterName?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  workCenterStatus?: number;

  /**
   * 工作中心资源列表（子表，级联保存）
   */
  resources?: WorkCenterResourceCreate[];

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
 * WorkCenter 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 WorkCenterImport
 * @description 对应后端 TaktWorkCenterImportDto
 */
export interface WorkCenterImport {
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
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 工作中心名称
   */
  workCenterName?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  workCenterStatus?: number;

  /**
   * 工作中心资源列表（子表，级联保存）
   */
  resources?: WorkCenterResourceCreate[];

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
 * WorkCenter 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 WorkCenterExport
 * @description 对应后端 TaktWorkCenterExportDto
 */
export interface WorkCenterExport {
  /**
   * WorkCenterID
   */
  workCenterId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工作中心编码
   */
  workCenterCode: string;

  /**
   * 工作中心名称
   */
  workCenterName: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  workCenterStatus: number;

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

