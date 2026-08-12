// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-group.d.ts
// 创建时间：2026-08-06
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
   * 采购组负责人用户 ID（选项 TaktUsers/options；DictValue=Id）
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
  groupStatus?: number;

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
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
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
   * 采购组负责人用户 ID（选项 TaktUsers/options；DictValue=Id）
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
  groupStatus: number;

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

