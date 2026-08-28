// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：asset.d.ts
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
 * 资产实体
 * 对应前端 TaktAssetDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Asset
 * @description 对应后端 TaktAssetDto
 */
export interface Asset extends CompanyDtoBase {

  /**
   * 资产状态（字典 accounting_financial_asset_status）
   */
  assetStatus?: number;

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
 * Asset 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssetExport
 * @description 对应后端 TaktAssetExportDto
 */
export interface AssetExport {
  /**
   * AssetID
   */
  assetId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 资产代码
   */
  assetCode: string;

  /**
   * 资产名称
   */
  assetName: string;

  /**
   * 资产分类（字典 accounting_financial_asset_category）
   */
  assetCategory: string;

  /**
   * 资产类型（字典 accounting_financial_asset_type）
   */
  assetType: string;

  /**
   * 资产原值
   */
  assetOriginalValue: number;

  /**
   * 资产净值
   */
  assetNetValue: number;

  /**
   * 累计折旧
   */
  accumulatedDepreciation: number;

  /**
   * 成本中心（选项 TaktCostCenters/tree-options；DictValue=Id）
   */
  costCenterId?: string;

  /**
   * 成本中心名称（冗余：按 CostCenterId 取 TaktCostCenter.CostCenterName 联动）
   */
  costCenterName?: string;

  /**
   * 部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  deptId?: string;

  /**
   * 部门名称（冗余：按 DeptId 取 TaktDept.DeptName1 联动）
   */
  deptName?: string;

  /**
   * 使用者（选项 TaktUsers/options；DictValue=Id）
   */
  userId?: string;

  /**
   * 使用者名称（冗余：按 UserId 取 TaktUser.UserName 联动）
   */
  userName?: string;

  /**
   * 资产位置
   */
  assetLocation?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 报废日期
   */
  scrapDate?: string;

  /**
   * 处置日期
   */
  disposalDate?: string;

  /**
   * 预计使用月数
   */
  expectedLifeMonths: number;

  /**
   * 折旧方法（字典 accounting_financial_depreciation_method）
   */
  depreciationMethod: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation: number;

  /**
   * 关联工厂
   */
  plantCode: string;

  /**
   * 资产状态（字典 accounting_financial_asset_status）
   */
  assetStatus: number;

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

