// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：account-title.d.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 会计科目实体
 * 对应前端 TaktAccountTitleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AccountTitle
 * @description 对应后端 TaktAccountTitleDto
 */
export interface AccountTitle extends CompanyDtoBase {
  /**
   * AccountTitleID
   */
  accountTitleId: string;
  /**
   * 科目编码
   */
  accountTitleCode: string;
  /**
   * 科目名称
   */
  accountTitleName: string;
  /**
   * 父级 ID
   */
  parentId: string;
  /**
   * 科目类型（字典 accounting_account_title_type）
   */
  accountTitleType: string;
  /**
   * 余额方向（0=借方，1=贷方）
   */
  balanceDirection: number;
  /**
   * 科目层级
   */
  accountTitleLevel: number;
  /**
   * 末级科目（字典 sys_yes_no_type；1=是，0=否）
   */
  isLeaf: number;
  /**
   * 辅助核算（字典 sys_yes_no_type；1=是，0=否）
   */
  isAuxiliary: number;
  /**
   * 辅助核算类型（字典 accounting_auxiliary_type）
   */
  auxiliaryType: string;
  /**
   * 数量核算（字典 sys_yes_no_type；1=是，0=否）
   */
  isQuantity: number;
  /**
   * 外币核算（字典 sys_yes_no_type；1=是，0=否）
   */
  isCurrency: number;
  /**
   * 现金科目（字典 sys_yes_no_type；1=是，0=否）
   */
  isCash: number;
  /**
   * 银行科目（字典 sys_yes_no_type；1=是，0=否）
   */
  isBank: number;
  /**
   * 生效日期
   */
  validFrom: string;
  /**
   * 失效日期
   */
  validTo: string;
  /**
   * 排序号
   */
  sortOrder: number;
  /**
   * 科目状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  accountTitleStatus?: number;
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
 * AccountTitle 树形列表/树选择 DTO（含子节点）
 * 对应 GetAccountTitleTreeAsync 等接口
 * @description 对应后端 TaktAccountTitleTreeDto
 */
export interface AccountTitleTree extends AccountTitle {
  /**
   * 子节点
   */
  children: AccountTitleTree[];
}

/**
 * AccountTitle 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AccountTitleExport
 * @description 对应后端 TaktAccountTitleExportDto
 */
export interface AccountTitleExport {
  /**
   * AccountTitleID
   */
  accountTitleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 科目编码
   */
  accountTitleCode: string;

  /**
   * 科目名称
   */
  accountTitleName: string;

  /**
   * 父级 ID
   */
  parentId: string;

  /**
 * 科目类型（字典 accounting_account_title_type）
   */
  accountTitleType: string;

  /**
   * 余额方向（0=借方，1=贷方）
   */
  balanceDirection: number;

  /**
   * 科目层级
   */
  accountTitleLevel: number;

  /**
   * 末级科目（字典 sys_yes_no_type；1=是，0=否）
   */
  isLeaf: number;

  /**
   * 辅助核算（字典 sys_yes_no_type；1=是，0=否）
   */
  isAuxiliary: number;

  /**
 * 辅助核算类型（字典 accounting_auxiliary_type）
   */
  auxiliaryType: string;

  /**
   * 数量核算（字典 sys_yes_no_type；1=是，0=否）
   */
  isQuantity: number;

  /**
   * 外币核算（字典 sys_yes_no_type；1=是，0=否）
   */
  isCurrency: number;

  /**
   * 现金科目（字典 sys_yes_no_type；1=是，0=否）
   */
  isCash: number;

  /**
   * 银行科目（字典 sys_yes_no_type；1=是，0=否）
   */
  isBank: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 科目状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  accountTitleStatus: number;

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

