// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：storage-location.d.ts
// 创建时间：2026-06-21
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
 * Takt库位主数据实体（公司级；从属于 TaktWarehouse）
 * 对应前端 TaktStorageLocationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 StorageLocation
 * @description 对应后端 TaktStorageLocationDto
 */
export interface StorageLocation extends CompanyDtoBase {
  /**
   * StorageLocationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  storageLocationId: string;

  /**
   * 仓库ID（主子表关系，关联 TaktWarehouse 主键）
   */
  warehouseId: string;

  /**
   * 仓库名称（填充字段）
   */
  warehouseName?: string;

  /**
   * 工厂代码（冗余字段，便于查询；关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 仓库编码（冗余字段，便于查询；关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode: string;

  /**
   * 库位编码（租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
   */
  locationCode: string;

  /**
   * 库位名称
   */
  locationName: string;

  /**
   * 库位类型（0=存储区，1=拣货区，2=暂存区，3=不良品区，4=其他）
   */
  locationType: number;

  /**
   * 库位状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  locationStatus: number;

  /**
   * 是否内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 所属仓库（主子表关系） （主表：TaktWarehouse）
   */
  warehouse?: Warehouse;

}


/**
 * StorageLocation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 StorageLocationQuery
 * @description 对应后端 TaktStorageLocationQueryDto
 */
export interface StorageLocationQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 仓库ID（主子表关系，关联 TaktWarehouse 主键）
   */
  warehouseId?: string;

  /**
   * 工厂代码（冗余字段，便于查询；关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 仓库编码（冗余字段，便于查询；关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 库位编码（租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
   */
  locationCode?: string;

  /**
   * 库位名称
   */
  locationName?: string;

  /**
   * 库位类型（0=存储区，1=拣货区，2=暂存区，3=不良品区，4=其他）
   */
  locationType?: number;

  /**
   * 库位状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  locationStatus?: number;

  /**
   * 是否内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
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
 * 创建StorageLocation DTO
 * 对应前端 StorageLocationCreate
 * @description 对应后端 TaktStorageLocationCreateDto
 */
export interface StorageLocationCreate {
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
   * 仓库ID（主子表关系，关联 TaktWarehouse 主键）
   */
  warehouseId: string;

  /**
   * 工厂代码（冗余字段，便于查询；关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 仓库编码（冗余字段，便于查询；关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode: string;

  /**
   * 库位编码（租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
   */
  locationCode: string;

  /**
   * 库位名称
   */
  locationName: string;

  /**
   * 库位类型（0=存储区，1=拣货区，2=暂存区，3=不良品区，4=其他）
   */
  locationType: number;

  /**
   * 库位状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  locationStatus: number;

  /**
   * 是否内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

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
 * 更新StorageLocation DTO
 * 继承 TaktStorageLocationCreateDto，添加 StorageLocationId 字段
 * 对应前端 StorageLocationUpdate
 * @description 对应后端 TaktStorageLocationUpdateDto
 */
export interface StorageLocationUpdate extends StorageLocationCreate {
  /**
   * StorageLocationID（标识要更新的实体）
   */
  storageLocationId: string;

}


/**
 * StorageLocation 状态更新 DTO
 * 对应前端 StorageLocationStatus
 * @description 对应后端 TaktStorageLocationStatusDto
 */
export interface StorageLocationStatus {
  /**
   * StorageLocationID
   */
  storageLocationId: string;

  /**
   * 库位状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  locationStatus: number;

}


/**
 * StorageLocation 排序更新 DTO
 * 对应前端 StorageLocationSort
 * @description 对应后端 TaktStorageLocationSortDto
 */
export interface StorageLocationSort {
  /**
   * StorageLocationID
   */
  storageLocationId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * StorageLocation 导入模板行 DTO
 * 对应前端 StorageLocationTemplate
 * @description 对应后端 TaktStorageLocationTemplateDto
 */
export interface StorageLocationTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 仓库ID（主子表关系，关联 TaktWarehouse 主键）
   */
  warehouseId?: string;

  /**
   * 工厂代码（冗余字段，便于查询；关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 仓库编码（冗余字段，便于查询；关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 库位编码（租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
   */
  locationCode?: string;

  /**
   * 库位名称
   */
  locationName?: string;

  /**
   * 库位类型（0=存储区，1=拣货区，2=暂存区，3=不良品区，4=其他）
   */
  locationType?: number;

  /**
   * 库位状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  locationStatus?: number;

  /**
   * 是否内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

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
 * StorageLocation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 StorageLocationImport
 * @description 对应后端 TaktStorageLocationImportDto
 */
export interface StorageLocationImport {
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
   * 仓库ID（主子表关系，关联 TaktWarehouse 主键）
   */
  warehouseId?: string;

  /**
   * 工厂代码（冗余字段，便于查询；关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 仓库编码（冗余字段，便于查询；关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 库位编码（租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
   */
  locationCode?: string;

  /**
   * 库位名称
   */
  locationName?: string;

  /**
   * 库位类型（0=存储区，1=拣货区，2=暂存区，3=不良品区，4=其他）
   */
  locationType?: number;

  /**
   * 库位状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  locationStatus?: number;

  /**
   * 是否内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

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
 * StorageLocation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 StorageLocationExport
 * @description 对应后端 TaktStorageLocationExportDto
 */
export interface StorageLocationExport {
  /**
   * StorageLocationID
   */
  storageLocationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 仓库ID（主子表关系，关联 TaktWarehouse 主键）
   */
  warehouseId: string;

  /**
   * 工厂代码（冗余字段，便于查询；关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 仓库编码（冗余字段，便于查询；关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode: string;

  /**
   * 库位编码（租户+公司+工厂+仓库内唯一；序列号入出库等业务表存此编码）
   */
  locationCode: string;

  /**
   * 库位名称
   */
  locationName: string;

  /**
   * 库位类型（0=存储区，1=拣货区，2=暂存区，3=不良品区，4=其他）
   */
  locationType: number;

  /**
   * 库位状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  locationStatus: number;

  /**
   * 是否内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
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

