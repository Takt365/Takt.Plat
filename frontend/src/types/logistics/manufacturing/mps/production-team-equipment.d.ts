// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：production-team-equipment.d.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 生产班组设备组明细（主子表；PCBA 线体生产设备及台数）
 * 对应前端 TaktProductionTeamEquipmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionTeamEquipment
 * @description 对应后端 TaktProductionTeamEquipmentDto
 */
export interface ProductionTeamEquipment extends CompanyDtoBase {
  /**
   * ProductionTeamEquipmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  productionTeamEquipmentId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
   */
  productionTeamId: string;

  /**
   * 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
   */
  productionTeamName?: string;

  /**
   * 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
   */
  teamCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产设备主键（关联 TaktProductionEquipment.Id）
   */
  productionEquipmentId: string;

  /**
   * 生产设备主键（关联 TaktProductionEquipment.Id）
   */
  productionEquipmentName?: string;

  /**
   * 生产设备编码（冗余快照，与 TaktProductionEquipment.ProductionEquipmentCode 一致）
   */
  productionEquipmentCode: string;

  /**
   * 设备台数（同型号多台时 &gt;1）
   */
  equipmentQuantity: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  teamEquipmentStatus: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * ProductionTeamEquipment 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProductionTeamEquipmentQuery
 * @description 对应后端 TaktProductionTeamEquipmentQueryDto
 */
export interface ProductionTeamEquipmentQuery extends TaktPagedQuery {
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
   * 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
   */
  productionTeamId?: string;

  /**
   * 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
   */
  teamCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产设备主键（关联 TaktProductionEquipment.Id）
   */
  productionEquipmentId?: string;

  /**
   * 生产设备编码（冗余快照，与 TaktProductionEquipment.ProductionEquipmentCode 一致）
   */
  productionEquipmentCode?: string;

  /**
   * 设备台数（同型号多台时 &gt;1）
   */
  equipmentQuantity?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  teamEquipmentStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建ProductionTeamEquipment DTO
 * 对应前端 ProductionTeamEquipmentCreate
 * @description 对应后端 TaktProductionTeamEquipmentCreateDto
 */
export interface ProductionTeamEquipmentCreate {
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
   * 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
   */
  productionTeamId: string;

  /**
   * 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
   */
  teamCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产设备主键（关联 TaktProductionEquipment.Id）
   */
  productionEquipmentId: string;

  /**
   * 生产设备编码（冗余快照，与 TaktProductionEquipment.ProductionEquipmentCode 一致）
   */
  productionEquipmentCode: string;

  /**
   * 设备台数（同型号多台时 &gt;1）
   */
  equipmentQuantity: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  teamEquipmentStatus: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
 * 更新ProductionTeamEquipment DTO
 * 继承 TaktProductionTeamEquipmentCreateDto，添加 ProductionTeamEquipmentId 字段
 * 对应前端 ProductionTeamEquipmentUpdate
 * @description 对应后端 TaktProductionTeamEquipmentUpdateDto
 */
export interface ProductionTeamEquipmentUpdate extends ProductionTeamEquipmentCreate {
  /**
   * ProductionTeamEquipmentID（标识要更新的实体）
   */
  productionTeamEquipmentId: string;

}


/**
 * ProductionTeamEquipment 状态更新 DTO
 * 对应前端 ProductionTeamEquipmentStatus
 * @description 对应后端 TaktProductionTeamEquipmentStatusDto
 */
export interface ProductionTeamEquipmentStatus {
  /**
   * ProductionTeamEquipmentID
   */
  productionTeamEquipmentId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  teamEquipmentStatus: number;

}


/**
 * ProductionTeamEquipment 作废/撤销作废 DTO
 * 对应前端 ProductionTeamEquipmentObsolete
 * @description 对应后端 TaktProductionTeamEquipmentObsoleteDto
 */
export interface ProductionTeamEquipmentObsolete {
  /**
   * ProductionTeamEquipmentID
   */
  productionTeamEquipmentId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * ProductionTeamEquipment 导入模板行 DTO
 * 对应前端 ProductionTeamEquipmentTemplate
 * @description 对应后端 TaktProductionTeamEquipmentTemplateDto
 */
export interface ProductionTeamEquipmentTemplate {
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
   * 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
   */
  productionTeamId?: string;

  /**
   * 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
   */
  teamCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产设备主键（关联 TaktProductionEquipment.Id）
   */
  productionEquipmentId?: string;

  /**
   * 生产设备编码（冗余快照，与 TaktProductionEquipment.ProductionEquipmentCode 一致）
   */
  productionEquipmentCode?: string;

  /**
   * 设备台数（同型号多台时 &gt;1）
   */
  equipmentQuantity?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  teamEquipmentStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * ProductionTeamEquipment 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProductionTeamEquipmentImport
 * @description 对应后端 TaktProductionTeamEquipmentImportDto
 */
export interface ProductionTeamEquipmentImport {
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
   * 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
   */
  productionTeamId?: string;

  /**
   * 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
   */
  teamCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产设备主键（关联 TaktProductionEquipment.Id）
   */
  productionEquipmentId?: string;

  /**
   * 生产设备编码（冗余快照，与 TaktProductionEquipment.ProductionEquipmentCode 一致）
   */
  productionEquipmentCode?: string;

  /**
   * 设备台数（同型号多台时 &gt;1）
   */
  equipmentQuantity?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  teamEquipmentStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * ProductionTeamEquipment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionTeamEquipmentExport
 * @description 对应后端 TaktProductionTeamEquipmentExportDto
 */
export interface ProductionTeamEquipmentExport {
  /**
   * ProductionTeamEquipmentID
   */
  productionTeamEquipmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
   */
  productionTeamId: string;

  /**
   * 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
   */
  teamCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产设备主键（关联 TaktProductionEquipment.Id）
   */
  productionEquipmentId: string;

  /**
   * 生产设备编码（冗余快照，与 TaktProductionEquipment.ProductionEquipmentCode 一致）
   */
  productionEquipmentCode: string;

  /**
   * 设备台数（同型号多台时 &gt;1）
   */
  equipmentQuantity: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  teamEquipmentStatus: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

