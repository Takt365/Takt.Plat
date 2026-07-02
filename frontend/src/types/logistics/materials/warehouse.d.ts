// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：warehouse.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt仓库主数据实体（公司级；按工厂划分仓储地点）
 * 对应前端 TaktWarehouseDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Warehouse
 * @description 对应后端 TaktWarehouseDto
 */
export interface Warehouse extends CompanyDtoBase {
  /**
   * WarehouseID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  warehouseId: string;

  /**
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 仓库编码（租户+公司+工厂内唯一；序列号入出库等业务表存此编码）
   */
  warehouseCode: string;

  /**
   * 仓库名称
   */
  warehouseName: string;

  /**
   * 仓库简称
   */
  warehouseShortName?: string;

  /**
   * 仓库地址（address）
   */
  address?: string;

  /**
   * 联系人（contact_person）
   */
  contactPerson?: string;

  /**
   * 联系电话（contact_phone）
   */
  contactPhone?: string;

  /**
   * 仓库负责人用户编码（manager_user_code；关联用户业务编码）
   */
  managerUserCode?: string;

  /**
   * 虚拟仓（is_virtual；字典 sys_yes_no_type；0=实体仓，1=虚拟仓）
   */
  isVirtual: number;

  /**
   * 仓库类型（0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
   */
  warehouseType: number;

  /**
   * 仓库状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  warehouseStatus: number;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 库位列表（主子表关系） （子表：TaktStorageLocation）
   */
  storageLocations?: StorageLocation[];

}


/**
 * Warehouse 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 WarehouseQuery
 * @description 对应后端 TaktWarehouseQueryDto
 */
export interface WarehouseQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 仓库编码（租户+公司+工厂内唯一；序列号入出库等业务表存此编码）
   */
  warehouseCode?: string;

  /**
   * 仓库名称
   */
  warehouseName?: string;

  /**
   * 仓库简称
   */
  warehouseShortName?: string;

  /**
   * 仓库地址（address）
   */
  address?: string;

  /**
   * 联系人（contact_person）
   */
  contactPerson?: string;

  /**
   * 联系电话（contact_phone）
   */
  contactPhone?: string;

  /**
   * 仓库负责人用户编码（manager_user_code；关联用户业务编码）
   */
  managerUserCode?: string;

  /**
   * 虚拟仓（is_virtual；字典 sys_yes_no_type；0=实体仓，1=虚拟仓）
   */
  isVirtual?: number;

  /**
   * 仓库类型（0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
   */
  warehouseType?: number;

  /**
   * 仓库状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  warehouseStatus?: number;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 排序号（越小越靠前）
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
 * 创建Warehouse DTO
 * 对应前端 WarehouseCreate
 * @description 对应后端 TaktWarehouseCreateDto
 */
export interface WarehouseCreate {
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
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 仓库编码（租户+公司+工厂内唯一；序列号入出库等业务表存此编码）
   */
  warehouseCode: string;

  /**
   * 仓库名称
   */
  warehouseName: string;

  /**
   * 仓库简称
   */
  warehouseShortName?: string;

  /**
   * 仓库地址（address）
   */
  address?: string;

  /**
   * 联系人（contact_person）
   */
  contactPerson?: string;

  /**
   * 联系电话（contact_phone）
   */
  contactPhone?: string;

  /**
   * 仓库负责人用户编码（manager_user_code；关联用户业务编码）
   */
  managerUserCode?: string;

  /**
   * 虚拟仓（is_virtual；字典 sys_yes_no_type；0=实体仓，1=虚拟仓）
   */
  isVirtual: number;

  /**
   * 仓库类型（0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
   */
  warehouseType: number;

  /**
   * 仓库状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  warehouseStatus: number;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 库位列表（主子表关系）（子表，级联保存）
   */
  storageLocations?: StorageLocationCreate[];

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
 * 更新Warehouse DTO
 * 继承 TaktWarehouseCreateDto，添加 WarehouseId 字段
 * 对应前端 WarehouseUpdate
 * @description 对应后端 TaktWarehouseUpdateDto
 */
export interface WarehouseUpdate extends WarehouseCreate {
  /**
   * WarehouseID（标识要更新的实体）
   */
  warehouseId: string;

}


/**
 * Warehouse 状态更新 DTO
 * 对应前端 WarehouseStatus
 * @description 对应后端 TaktWarehouseStatusDto
 */
export interface WarehouseStatus {
  /**
   * WarehouseID
   */
  warehouseId: string;

  /**
   * 仓库状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  warehouseStatus: number;

}


/**
 * Warehouse 排序更新 DTO
 * 对应前端 WarehouseSort
 * @description 对应后端 TaktWarehouseSortDto
 */
export interface WarehouseSort {
  /**
   * WarehouseID
   */
  warehouseId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * Warehouse 导入模板行 DTO
 * 对应前端 WarehouseTemplate
 * @description 对应后端 TaktWarehouseTemplateDto
 */
export interface WarehouseTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 仓库编码（租户+公司+工厂内唯一；序列号入出库等业务表存此编码）
   */
  warehouseCode?: string;

  /**
   * 仓库名称
   */
  warehouseName?: string;

  /**
   * 仓库简称
   */
  warehouseShortName?: string;

  /**
   * 仓库地址（address）
   */
  address?: string;

  /**
   * 联系人（contact_person）
   */
  contactPerson?: string;

  /**
   * 联系电话（contact_phone）
   */
  contactPhone?: string;

  /**
   * 仓库负责人用户编码（manager_user_code；关联用户业务编码）
   */
  managerUserCode?: string;

  /**
   * 虚拟仓（is_virtual；字典 sys_yes_no_type；0=实体仓，1=虚拟仓）
   */
  isVirtual?: number;

  /**
   * 仓库类型（0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
   */
  warehouseType?: number;

  /**
   * 仓库状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  warehouseStatus?: number;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 库位列表（主子表关系）（子表，级联保存）
   */
  storageLocations?: StorageLocationCreate[];

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
 * Warehouse 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 WarehouseImport
 * @description 对应后端 TaktWarehouseImportDto
 */
export interface WarehouseImport {
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
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 仓库编码（租户+公司+工厂内唯一；序列号入出库等业务表存此编码）
   */
  warehouseCode?: string;

  /**
   * 仓库名称
   */
  warehouseName?: string;

  /**
   * 仓库简称
   */
  warehouseShortName?: string;

  /**
   * 仓库地址（address）
   */
  address?: string;

  /**
   * 联系人（contact_person）
   */
  contactPerson?: string;

  /**
   * 联系电话（contact_phone）
   */
  contactPhone?: string;

  /**
   * 仓库负责人用户编码（manager_user_code；关联用户业务编码）
   */
  managerUserCode?: string;

  /**
   * 虚拟仓（is_virtual；字典 sys_yes_no_type；0=实体仓，1=虚拟仓）
   */
  isVirtual?: number;

  /**
   * 仓库类型（0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
   */
  warehouseType?: number;

  /**
   * 仓库状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  warehouseStatus?: number;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 库位列表（主子表关系）（子表，级联保存）
   */
  storageLocations?: StorageLocationCreate[];

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
 * Warehouse 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 WarehouseExport
 * @description 对应后端 TaktWarehouseExportDto
 */
export interface WarehouseExport {
  /**
   * WarehouseID
   */
  warehouseId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 仓库编码（租户+公司+工厂内唯一；序列号入出库等业务表存此编码）
   */
  warehouseCode: string;

  /**
   * 仓库名称
   */
  warehouseName: string;

  /**
   * 仓库简称
   */
  warehouseShortName?: string;

  /**
   * 仓库地址（address）
   */
  address?: string;

  /**
   * 联系人（contact_person）
   */
  contactPerson?: string;

  /**
   * 联系电话（contact_phone）
   */
  contactPhone?: string;

  /**
   * 仓库负责人用户编码（manager_user_code；关联用户业务编码）
   */
  managerUserCode?: string;

  /**
   * 虚拟仓（is_virtual；字典 sys_yes_no_type；0=实体仓，1=虚拟仓）
   */
  isVirtual: number;

  /**
   * 仓库类型（0=原材料仓，1=半成品仓，2=成品仓，3=不良品仓，4=外协仓，5=其他）
   */
  warehouseType: number;

  /**
   * 仓库状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  warehouseStatus: number;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 排序号（越小越靠前）
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

