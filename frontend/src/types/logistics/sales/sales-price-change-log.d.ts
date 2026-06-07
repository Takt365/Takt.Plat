// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：sales-price-change-log.d.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 销售价格变更记录实体
 * 对应前端 TaktSalesPriceChangeLogDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesPriceChangeLog
 * @description 对应后端 TaktSalesPriceChangeLogDto
 */
export interface SalesPriceChangeLog extends CompanyDtoBase {
  /**
   * SalesPriceChangeLogID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPriceChangeLogId: string;

  /**
   * 销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesPriceId: string;

  /**
   * 销售价格名称（填充字段）
   */
  salesPriceName?: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人（人员代码）
   */
  changeBy?: string;

  /**
   * 变更原因
   */
  changeReason?: string;

}


/**
 * SalesPriceChangeLog 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesPriceChangeLogQuery
 * @description 对应后端 TaktSalesPriceChangeLogQueryDto
 */
export interface SalesPriceChangeLogQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesPriceId?: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
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
   * 变更人（人员代码）
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
 * 创建SalesPriceChangeLog DTO
 * 对应前端 SalesPriceChangeLogCreate
 * @description 对应后端 TaktSalesPriceChangeLogCreateDto
 */
export interface SalesPriceChangeLogCreate {
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
   * 销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesPriceId: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人（人员代码）
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
 * 更新SalesPriceChangeLog DTO
 * 继承 TaktSalesPriceChangeLogCreateDto，添加 SalesPriceChangeLogId 字段
 * 对应前端 SalesPriceChangeLogUpdate
 * @description 对应后端 TaktSalesPriceChangeLogUpdateDto
 */
export interface SalesPriceChangeLogUpdate extends SalesPriceChangeLogCreate {
  /**
   * SalesPriceChangeLogID（标识要更新的实体）
   */
  salesPriceChangeLogId: string;

}


/**
 * SalesPriceChangeLog 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesPriceChangeLogExport
 * @description 对应后端 TaktSalesPriceChangeLogExportDto
 */
export interface SalesPriceChangeLogExport {
  /**
   * SalesPriceChangeLogID
   */
  salesPriceChangeLogId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 销售价格ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  salesPriceId: string;

  /**
   * 变更字段列表（JSON数组格式，记录同一时间点修改的所有字段及其旧值、新值） 格式：[{"field":"FieldName","description":"字段描述","oldValue":"旧值","newValue":"新值"}]
   */
  changeFields?: string;

  /**
   * 变更时间
   */
  changeTime: string;

  /**
   * 变更人（人员代码）
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

