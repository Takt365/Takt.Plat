// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：it-asset-change-log.d.ts
// 创建时间：2026-06-10
// 创建人：Takt365(Auto Generated)
// 功能描述：routine/help-desk 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * IT 设备保修扩展变更日志实体
 * 对应前端 TaktItAssetChangeLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ItAssetChangeLog
 * @description 对应后端 TaktItAssetChangeLogDto
 */
export interface ItAssetChangeLog extends CompanyDtoBase {
  /**
   * ItAssetChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  itAssetChangeLogId: string;

  /**
   * IT 设备保修扩展 ID
   */
  itAssetId: string;

  /**
   * IT 设备保修扩展 名称（填充字段）
   */
  itAssetName?: string;

  /**
   * 资产号码（冗余）
   */
  assetCode?: string;

  /**
   * 变更类型（见 TaktHelpDeskChangeType）
   */
  changeType: number;

  /**
   * 修改内容摘要
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组）
   */
  changeFields?: string;

  /**
   * 变更原因或备注
   */
  changeReason?: string;

  /**
   * IT 设备保修扩展（主表） （主表：TaktItAsset）
   */
  itAsset?: ItAsset;

}


/**
 * ItAssetChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ItAssetChangeLogQuery
 * @description 对应后端 TaktItAssetChangeLogQueryDto
 */
export interface ItAssetChangeLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * IT 设备保修扩展 ID
   */
  itAssetId?: string;

  /**
   * 资产号码（冗余）
   */
  assetCode?: string;

  /**
   * 变更类型（见 TaktHelpDeskChangeType）
   */
  changeType?: number;

  /**
   * 修改内容摘要
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组）
   */
  changeFields?: string;

  /**
   * 变更原因或备注
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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建ItAssetChangeLog DTO
 * 对应前端 ItAssetChangeLogCreate
 * @description 对应后端 TaktItAssetChangeLogCreateDto
 */
export interface ItAssetChangeLogCreate {
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
   * IT 设备保修扩展 ID
   */
  itAssetId: string;

  /**
   * 资产号码（冗余）
   */
  assetCode?: string;

  /**
   * 变更类型（见 TaktHelpDeskChangeType）
   */
  changeType: number;

  /**
   * 修改内容摘要
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组）
   */
  changeFields?: string;

  /**
   * 变更原因或备注
   */
  changeReason?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新ItAssetChangeLog DTO
 * 继承 TaktItAssetChangeLogCreateDto，添加 ItAssetChangeLogId 字段
 * 对应前端 ItAssetChangeLogUpdate
 * @description 对应后端 TaktItAssetChangeLogUpdateDto
 */
export interface ItAssetChangeLogUpdate extends ItAssetChangeLogCreate {
  /**
   * ItAssetChangeLogID（标识要更新的实体）
   */
  itAssetChangeLogId: string;

}


/**
 * ItAssetChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ItAssetChangeLogExport
 * @description 对应后端 TaktItAssetChangeLogExportDto
 */
export interface ItAssetChangeLogExport {
  /**
   * ItAssetChangeLogID
   */
  itAssetChangeLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * IT 设备保修扩展 ID
   */
  itAssetId: string;

  /**
   * 资产号码（冗余）
   */
  assetCode?: string;

  /**
   * 变更类型（见 TaktHelpDeskChangeType）
   */
  changeType: number;

  /**
   * 修改内容摘要
   */
  changeSummary?: string;

  /**
   * 变更字段列表（JSON 数组）
   */
  changeFields?: string;

  /**
   * 变更原因或备注
   */
  changeReason?: string;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

