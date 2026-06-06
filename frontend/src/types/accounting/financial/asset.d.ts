// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：asset.d.ts
// 创建时间：2026-06-06
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
   * AssetID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assetId: string;

  /**
   * 资产代码
   */
  assetCode: string;

  /**
   * 资产名称
   */
  assetName: string;

  /**
   * 资产分类ID
   */
  assetCategoryId: string;

  /**
   * 资产分类名称
   */
  assetCategoryName?: string;

  /**
   * 资产类型
   */
  assetType: number;

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
   * 成本中心ID
   */
  costCenterId?: string;

  /**
   * 成本中心名称
   */
  costCenterName?: string;

  /**
   * 部门ID
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 使用者ID
   */
  userId?: string;

  /**
   * 使用者名称
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
   * 折旧方法
   */
  depreciationMethod: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation: number;

  /**
   * 关联生产线
   */
  relatedPlant?: string;

  /**
   * 资产状态
   */
  assetStatus: number;

}


/**
 * Asset 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssetQuery
 * @description 对应后端 TaktAssetQueryDto
 */
export interface AssetQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 资产代码
   */
  assetCode?: string;

  /**
   * 资产名称
   */
  assetName?: string;

  /**
   * 资产分类ID
   */
  assetCategoryId?: string;

  /**
   * 资产分类名称
   */
  assetCategoryName?: string;

  /**
   * 资产类型
   */
  assetType?: number;

  /**
   * 资产原值
   */
  assetOriginalValue?: number;

  /**
   * 资产净值
   */
  assetNetValue?: number;

  /**
   * 累计折旧
   */
  accumulatedDepreciation?: number;

  /**
   * 成本中心ID
   */
  costCenterId?: string;

  /**
   * 成本中心名称
   */
  costCenterName?: string;

  /**
   * 部门ID
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 使用者ID
   */
  userId?: string;

  /**
   * 使用者名称
   */
  userName?: string;

  /**
   * 资产位置
   */
  assetLocation?: string;

  /**
   * 购买日期（范围查询-开始）
   */
  purchaseDateStart?: string;

  /**
   * 购买日期（范围查询-结束）
   */
  purchaseDateEnd?: string;

  /**
   * 启用日期（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 启用日期（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 报废日期（范围查询-开始）
   */
  scrapDateStart?: string;

  /**
   * 报废日期（范围查询-结束）
   */
  scrapDateEnd?: string;

  /**
   * 处置日期（范围查询-开始）
   */
  disposalDateStart?: string;

  /**
   * 处置日期（范围查询-结束）
   */
  disposalDateEnd?: string;

  /**
   * 预计使用月数
   */
  expectedLifeMonths?: number;

  /**
   * 折旧方法
   */
  depreciationMethod?: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation?: number;

  /**
   * 关联生产线
   */
  relatedPlant?: string;

  /**
   * 资产状态
   */
  assetStatus?: number;

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
  extFieldJson?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Asset DTO
 * 对应前端 AssetCreate
 * @description 对应后端 TaktAssetCreateDto
 */
export interface AssetCreate {
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
   * 资产代码
   */
  assetCode: string;

  /**
   * 资产名称
   */
  assetName: string;

  /**
   * 资产分类ID
   */
  assetCategoryId: string;

  /**
   * 资产分类名称
   */
  assetCategoryName?: string;

  /**
   * 资产类型
   */
  assetType: number;

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
   * 成本中心ID
   */
  costCenterId?: string;

  /**
   * 成本中心名称
   */
  costCenterName?: string;

  /**
   * 部门ID
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 使用者ID
   */
  userId?: string;

  /**
   * 使用者名称
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
   * 折旧方法
   */
  depreciationMethod: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation: number;

  /**
   * 关联生产线
   */
  relatedPlant?: string;

  /**
   * 资产状态
   */
  assetStatus: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新Asset DTO
 * 继承 TaktAssetCreateDto，添加 AssetId 字段
 * 对应前端 AssetUpdate
 * @description 对应后端 TaktAssetUpdateDto
 */
export interface AssetUpdate extends AssetCreate {
  /**
   * AssetID（标识要更新的实体）
   */
  assetId: string;

}


/**
 * Asset 状态更新 DTO
 * 对应前端 AssetStatus
 * @description 对应后端 TaktAssetStatusDto
 */
export interface AssetStatus {
  /**
   * AssetID
   */
  assetId: string;

  /**
   * 资产状态
   */
  assetStatus: number;

}


/**
 * Asset 导入模板行 DTO
 * 对应前端 AssetTemplate
 * @description 对应后端 TaktAssetTemplateDto
 */
export interface AssetTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 资产代码
   */
  assetCode?: string;

  /**
   * 资产名称
   */
  assetName?: string;

  /**
   * 资产分类ID
   */
  assetCategoryId?: string;

  /**
   * 资产分类名称
   */
  assetCategoryName?: string;

  /**
   * 资产类型
   */
  assetType?: number;

  /**
   * 成本中心ID
   */
  costCenterId?: string;

  /**
   * 成本中心名称
   */
  costCenterName?: string;

  /**
   * 部门ID
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 使用者ID
   */
  userId?: string;

  /**
   * 使用者名称
   */
  userName?: string;

  /**
   * 资产位置
   */
  assetLocation?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Asset 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssetImport
 * @description 对应后端 TaktAssetImportDto
 */
export interface AssetImport {
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
   * 资产代码
   */
  assetCode?: string;

  /**
   * 资产名称
   */
  assetName?: string;

  /**
   * 资产分类ID
   */
  assetCategoryId?: string;

  /**
   * 资产分类名称
   */
  assetCategoryName?: string;

  /**
   * 资产类型
   */
  assetType?: number;

  /**
   * 成本中心ID
   */
  costCenterId?: string;

  /**
   * 成本中心名称
   */
  costCenterName?: string;

  /**
   * 部门ID
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 使用者ID
   */
  userId?: string;

  /**
   * 使用者名称
   */
  userName?: string;

  /**
   * 资产位置
   */
  assetLocation?: string;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

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
   * 资产分类ID
   */
  assetCategoryId: string;

  /**
   * 资产分类名称
   */
  assetCategoryName?: string;

  /**
   * 资产类型
   */
  assetType: number;

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
   * 成本中心ID
   */
  costCenterId?: string;

  /**
   * 成本中心名称
   */
  costCenterName?: string;

  /**
   * 部门ID
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 使用者ID
   */
  userId?: string;

  /**
   * 使用者名称
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
   * 折旧方法
   */
  depreciationMethod: number;

  /**
   * 每月折旧金额
   */
  monthlyDepreciation: number;

  /**
   * 关联生产线
   */
  relatedPlant?: string;

  /**
   * 资产状态
   */
  assetStatus: number;

  /**
   * 扩展字段JSON
   */
  extFieldJson?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

