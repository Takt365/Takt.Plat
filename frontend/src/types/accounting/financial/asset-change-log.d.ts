// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：asset-change-log.d.ts
// 创建时间：2026-06-09
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
 * 资产变更记录实体
 * 对应前端 TaktAssetChangeLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssetChangeLog
 * @description 对应后端 TaktAssetChangeLogDto
 */
export interface AssetChangeLog extends CompanyDtoBase {
  /**
   * AssetChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assetChangeLogId: string;

  /**
   * 资产 ID
   */
  assetId: string;

  /**
   * 资产 名称（填充字段）
   */
  assetName?: string;

  /**
   * 资产编码（冗余）
   */
  assetCode: string;

  /**
   * 变更字段列表 JSON
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

}


/**
 * AssetChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssetChangeLogQuery
 * @description 对应后端 TaktAssetChangeLogQueryDto
 */
export interface AssetChangeLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 资产 ID
   */
  assetId?: string;

  /**
   * 资产编码（冗余）
   */
  assetCode?: string;

  /**
   * 变更字段列表 JSON
   */
  changeFields?: string;

  /**
   * 变更时间（范围查询-开始）
   */
  changeTimeStart?: string;

  /**
   * 变更时间（范围查询-结束）
   */
  changeTimeEnd?: string;

  /**
   * 变更人
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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
 * 创建AssetChangeLog DTO
 * 对应前端 AssetChangeLogCreate
 * @description 对应后端 TaktAssetChangeLogCreateDto
 */
export interface AssetChangeLogCreate {
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
   * 资产 ID
   */
  assetId: string;

  /**
   * 资产编码（冗余）
   */
  assetCode: string;

  /**
   * 变更字段列表 JSON
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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
 * 更新AssetChangeLog DTO
 * 继承 TaktAssetChangeLogCreateDto，添加 AssetChangeLogId 字段
 * 对应前端 AssetChangeLogUpdate
 * @description 对应后端 TaktAssetChangeLogUpdateDto
 */
export interface AssetChangeLogUpdate extends AssetChangeLogCreate {
  /**
   * AssetChangeLogID（标识要更新的实体）
   */
  assetChangeLogId: string;

}


/**
 * AssetChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssetChangeLogExport
 * @description 对应后端 TaktAssetChangeLogExportDto
 */
export interface AssetChangeLogExport {
  /**
   * AssetChangeLogID
   */
  assetChangeLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 资产 ID
   */
  assetId: string;

  /**
   * 资产编码（冗余）
   */
  assetCode: string;

  /**
   * 变更字段列表 JSON
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

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

