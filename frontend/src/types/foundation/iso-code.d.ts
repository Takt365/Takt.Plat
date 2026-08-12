// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/foundation
// 文件名称：iso-code.d.ts
// 创建时间：2026-06-27
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantDtoBase
} from '@/types/common';

/**
 * ISO 编码实体 维护租户内标准短码（如 Eng、Pmc、D1000），用于编码规则、单据编码等段引用
 * 对应前端 TaktIsoCodeDto
 * 继承 TaktTenantDtoBase
 * 对应前端 IsoCode
 * @description 对应后端 TaktIsoCodeDto
 */
export interface IsoCode extends TenantDtoBase {
  /**
   * IsoCodeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  isoCodeId: string;

  /**
   * 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
   */
  isoCodeCategory: number;

  /**
   * ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
   */
  isoCode: string;

  /**
   * ISO 名称（如：技术、生管、总经理室）
   */
  isoName: string;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
   */
  isBuiltIn: number;

  /**
   * 描述说明
   */
  isoCodeDescription?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  isoCodeStatus: number;

}

/**
 * IsoCode 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 IsoCodeQuery
 * @description 对应后端 TaktIsoCodeQueryDto
 */
export interface IsoCodeQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
   */
  isoCodeCategory?: number;

  /**
   * ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
   */
  isoCode?: string;

  /**
   * ISO 名称（如：技术、生管、总经理室）
   */
  isoName?: string;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
   */
  isBuiltIn?: number;

  /**
   * 描述说明
   */
  isoCodeDescription?: string;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  isoCodeStatus?: number;

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
 * 创建IsoCode DTO
 * 对应前端 IsoCodeCreate
 * @description 对应后端 TaktIsoCodeCreateDto
 */
export interface IsoCodeCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
   */
  isoCodeCategory: number;

  /**
   * ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
   */
  isoCode: string;

  /**
   * ISO 名称（如：技术、生管、总经理室）
   */
  isoName: string;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
   */
  isBuiltIn: number;

  /**
   * 描述说明
   */
  isoCodeDescription?: string;

  /**
   * 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  isoCodeStatus: number;

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
 * 更新IsoCode DTO
 * 继承 TaktIsoCodeCreateDto，添加 IsoCodeId 字段
 * 对应前端 IsoCodeUpdate
 * @description 对应后端 TaktIsoCodeUpdateDto
 */
export interface IsoCodeUpdate extends IsoCodeCreate {
  /**
   * IsoCodeID（标识要更新的实体）
   */
  isoCodeId: string;

}

/**
 * IsoCode 状态更新 DTO
 * 对应前端 IsoCodeStatus
 * @description 对应后端 TaktIsoCodeStatusDto
 */
export interface IsoCodeStatus {
  /**
   * IsoCodeID
   */
  isoCodeId: string;

  /**
   * 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  isoCodeStatus: number;

}

/**
 * IsoCode 排序更新 DTO
 * 对应前端 IsoCodeSort
 * @description 对应后端 TaktIsoCodeSortDto
 */
export interface IsoCodeSort {
  /**
   * IsoCodeID
   */
  isoCodeId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}

/**
 * IsoCode 导入模板行 DTO
 * 对应前端 IsoCodeTemplate
 * @description 对应后端 TaktIsoCodeTemplateDto
 */
export interface IsoCodeTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
   */
  isoCodeCategory?: number;

  /**
   * ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
   */
  isoCode?: string;

  /**
   * ISO 名称（如：技术、生管、总经理室）
   */
  isoName?: string;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
   */
  isBuiltIn?: number;

  /**
   * 描述说明
   */
  isoCodeDescription?: string;

  /**
   * 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  isoCodeStatus?: number;

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
 * IsoCode 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 IsoCodeImport
 * @description 对应后端 TaktIsoCodeImportDto
 */
export interface IsoCodeImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
   */
  isoCodeCategory?: number;

  /**
   * ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
   */
  isoCode?: string;

  /**
   * ISO 名称（如：技术、生管、总经理室）
   */
  isoName?: string;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
   */
  isBuiltIn?: number;

  /**
   * 描述说明
   */
  isoCodeDescription?: string;

  /**
   * 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  isoCodeStatus?: number;

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
 * IsoCode 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 IsoCodeExport
 * @description 对应后端 TaktIsoCodeExportDto
 */
export interface IsoCodeExport {
  /**
   * IsoCodeID
   */
  isoCodeId: string;

  /**
   * 编码类别（字典 sys_iso_code_category；0=不使用，1=部门）
   */
  isoCodeCategory: number;

  /**
   * ISO 编码（唯一索引：租户+类别内唯一，见 ix_iso_code_category_unique；编码规则等段引用，如 Eng、Pmc、D1000）
   */
  isoCode: string;

  /**
   * ISO 名称（如：技术、生管、总经理室）
   */
  isoName: string;

  /**
   * 内置（字典 sys_yes_no_type；0=否 1=是，内置项不可删除）
   */
  isBuiltIn: number;

  /**
   * 描述说明
   */
  isoCodeDescription?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable_status；1=启用 0=禁用）
   */
  isoCodeStatus: number;

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

