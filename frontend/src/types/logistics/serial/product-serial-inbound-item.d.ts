// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/serial
// 文件名称：product-serial-inbound-item.d.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/serial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * 产品序列号入库明细实体
 * 对应前端 TaktProductSerialInboundItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductSerialInboundItem
 * @description 对应后端 TaktProductSerialInboundItemDto
 */
export interface ProductSerialInboundItem extends CompanyDtoBase {
  /**
   * ProductSerialInboundItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  productSerialInboundItemId: string;

  /**
   * 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inboundId: string;

  /**
   * 入库名称（填充字段）
   */
  inboundName?: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 入库序列号（唯一索引）
   */
  inboundSerialNo: string;

  /**
   * 入库时间
   */
  inboundTime: string;

  /**
   * 入库主表 （主表：TaktProductSerialInbound）
   */
  inbound?: ProductSerialInbound;

}


/**
 * ProductSerialInboundItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProductSerialInboundItemQuery
 * @description 对应后端 TaktProductSerialInboundItemQueryDto
 */
export interface ProductSerialInboundItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inboundId?: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 入库序列号（唯一索引）
   */
  inboundSerialNo?: string;

  /**
   * 入库时间（范围查询-开始）
   */
  inboundTimeStart?: string;

  /**
   * 入库时间（范围查询-结束）
   */
  inboundTimeEnd?: string;

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
 * 创建ProductSerialInboundItem DTO
 * 对应前端 ProductSerialInboundItemCreate
 * @description 对应后端 TaktProductSerialInboundItemCreateDto
 */
export interface ProductSerialInboundItemCreate {
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
   * 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inboundId: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 入库序列号（唯一索引）
   */
  inboundSerialNo: string;

  /**
   * 入库时间
   */
  inboundTime: string;

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
 * 更新ProductSerialInboundItem DTO
 * 继承 TaktProductSerialInboundItemCreateDto，添加 ProductSerialInboundItemId 字段
 * 对应前端 ProductSerialInboundItemUpdate
 * @description 对应后端 TaktProductSerialInboundItemUpdateDto
 */
export interface ProductSerialInboundItemUpdate extends ProductSerialInboundItemCreate {
  /**
   * ProductSerialInboundItemID（标识要更新的实体）
   */
  productSerialInboundItemId: string;

}


/**
 * ProductSerialInboundItem 导入模板行 DTO
 * 对应前端 ProductSerialInboundItemTemplate
 * @description 对应后端 TaktProductSerialInboundItemTemplateDto
 */
export interface ProductSerialInboundItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inboundId?: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 入库序列号（唯一索引）
   */
  inboundSerialNo?: string;

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
 * ProductSerialInboundItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProductSerialInboundItemImport
 * @description 对应后端 TaktProductSerialInboundItemImportDto
 */
export interface ProductSerialInboundItemImport {
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
   * 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inboundId?: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundNo?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 入库序列号（唯一索引）
   */
  inboundSerialNo?: string;

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
 * ProductSerialInboundItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductSerialInboundItemExport
 * @description 对应后端 TaktProductSerialInboundItemExportDto
 */
export interface ProductSerialInboundItemExport {
  /**
   * ProductSerialInboundItemID
   */
  productSerialInboundItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 入库ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  inboundId: string;

  /**
   * 入库单号（冗余字段，便于查询）
   */
  inboundNo: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 入库序列号（唯一索引）
   */
  inboundSerialNo: string;

  /**
   * 入库时间
   */
  inboundTime: string;

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

