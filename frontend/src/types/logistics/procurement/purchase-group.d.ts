// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-group.d.ts
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt采购组主数据实体（公司级；采购业务组织分组）
 * 对应前端 TaktPurchaseGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseGroup
 * @description 对应后端 TaktPurchaseGroupDto
 */
export interface PurchaseGroup extends CompanyDtoBase {
  /**
   * PurchaseGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseGroupId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 采购组编码（3）
   */
  purchaseGroupCode: string;

  /**
   * 采购组名称
   */
  purchaseGroupName: string;

  /**
   * 采购组描述
   */
  purchaseGroupDescription?: string;

  /**
   * 采购组负责人用户 ID（关联 TaktUser.Id，选项 TaktUsers/options）
   */
  responsibleUserId?: string;

  /**
   * 采购组负责人用户 名称（填充字段）
   */
  responsibleUserName?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 采购组状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  purchaseGroupStatus: number;

}


/**
 * PurchaseGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchaseGroupQuery
 * @description 对应后端 TaktPurchaseGroupQueryDto
 */
export interface PurchaseGroupQuery extends TaktPagedQuery {
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
   * 采购组编码（3）
   */
  purchaseGroupCode?: string;

  /**
   * 采购组名称
   */
  purchaseGroupName?: string;

  /**
   * 采购组描述
   */
  purchaseGroupDescription?: string;

  /**
   * 采购组负责人用户 ID（关联 TaktUser.Id，选项 TaktUsers/options）
   */
  responsibleUserId?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 采购组状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  purchaseGroupStatus?: number;

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
 * 创建PurchaseGroup DTO
 * 对应前端 PurchaseGroupCreate
 * @description 对应后端 TaktPurchaseGroupCreateDto
 */
export interface PurchaseGroupCreate {
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
   * 采购组编码（3）
   */
  purchaseGroupCode: string;

  /**
   * 采购组名称
   */
  purchaseGroupName: string;

  /**
   * 采购组描述
   */
  purchaseGroupDescription?: string;

  /**
   * 采购组负责人用户 ID（关联 TaktUser.Id，选项 TaktUsers/options）
   */
  responsibleUserId?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 采购组状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  purchaseGroupStatus: number;

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
 * 更新PurchaseGroup DTO
 * 继承 TaktPurchaseGroupCreateDto，添加 PurchaseGroupId 字段
 * 对应前端 PurchaseGroupUpdate
 * @description 对应后端 TaktPurchaseGroupUpdateDto
 */
export interface PurchaseGroupUpdate extends PurchaseGroupCreate {
  /**
   * PurchaseGroupID（标识要更新的实体）
   */
  purchaseGroupId: string;

}


/**
 * PurchaseGroup 状态更新 DTO
 * 对应前端 PurchaseGroupStatus
 * @description 对应后端 TaktPurchaseGroupStatusDto
 */
export interface PurchaseGroupStatus {
  /**
   * PurchaseGroupID
   */
  purchaseGroupId: string;

  /**
   * 采购组状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  purchaseGroupStatus: number;

}


/**
 * PurchaseGroup 排序更新 DTO
 * 对应前端 PurchaseGroupSort
 * @description 对应后端 TaktPurchaseGroupSortDto
 */
export interface PurchaseGroupSort {
  /**
   * PurchaseGroupID
   */
  purchaseGroupId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}


/**
 * PurchaseGroup 导入模板行 DTO
 * 对应前端 PurchaseGroupTemplate
 * @description 对应后端 TaktPurchaseGroupTemplateDto
 */
export interface PurchaseGroupTemplate {
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
   * 采购组编码（3）
   */
  purchaseGroupCode?: string;

  /**
   * 采购组名称
   */
  purchaseGroupName?: string;

  /**
   * 采购组描述
   */
  purchaseGroupDescription?: string;

  /**
   * 采购组负责人用户 ID（关联 TaktUser.Id，选项 TaktUsers/options）
   */
  responsibleUserId?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 采购组状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  purchaseGroupStatus?: number;

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
 * PurchaseGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchaseGroupImport
 * @description 对应后端 TaktPurchaseGroupImportDto
 */
export interface PurchaseGroupImport {
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
   * 采购组编码（3）
   */
  purchaseGroupCode?: string;

  /**
   * 采购组名称
   */
  purchaseGroupName?: string;

  /**
   * 采购组描述
   */
  purchaseGroupDescription?: string;

  /**
   * 采购组负责人用户 ID（关联 TaktUser.Id，选项 TaktUsers/options）
   */
  responsibleUserId?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 采购组状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  purchaseGroupStatus?: number;

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
 * PurchaseGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseGroupExport
 * @description 对应后端 TaktPurchaseGroupExportDto
 */
export interface PurchaseGroupExport {
  /**
   * PurchaseGroupID
   */
  purchaseGroupId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 采购组编码（3）
   */
  purchaseGroupCode: string;

  /**
   * 采购组名称
   */
  purchaseGroupName: string;

  /**
   * 采购组描述
   */
  purchaseGroupDescription?: string;

  /**
   * 采购组负责人用户 ID（关联 TaktUser.Id，选项 TaktUsers/options）
   */
  responsibleUserId?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no_type；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 采购组状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  purchaseGroupStatus: number;

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

