// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/serial
// 文件名称：product-serial-inbound.d.ts
// 创建时间：2026-06-05
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
 * 产品序列号入库主表实体
 * 对应前端 TaktProductSerialInboundDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductSerialInbound
 * @description 对应后端 TaktProductSerialInboundDto
 */
export interface ProductSerialInbound extends CompanyDtoBase {
  /**
   * ProductSerialInboundID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  productSerialInboundId: string;

  /**
   * 工厂代码(4位字母数字组合)
   */
  plantCode: string;

  /**
   * 入库单号（组合唯一索引：PlantCode + InboundNo）
   */
  inboundNo: string;

  /**
   * 入库日期
   */
  inboundDate: string;

  /**
   * 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
   */
  inboundType: number;

  /**
   * 仓库编码
   */
  warehouseCode: string;

  /**
   * 库位编码
   */
  locationCode: string;

  /**
   * 总数量
   */
  totalQuantity: number;

  /**
   * 关联公司
   */
  relatedCompany: string;

  /**
   * 产品序列号入库明细列表(主子表关系) （子表：TaktProductSerialInboundItem）
   */
  items?: ProductSerialInboundItem[];

}


/**
 * ProductSerialInbound 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProductSerialInboundQuery
 * @description 对应后端 TaktProductSerialInboundQueryDto
 */
export interface ProductSerialInboundQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码(4位字母数字组合)
   */
  plantCode?: string;

  /**
   * 入库单号（组合唯一索引：PlantCode + InboundNo）
   */
  inboundNo?: string;

  /**
   * 入库日期（范围查询-开始）
   */
  inboundDateStart?: string;

  /**
   * 入库日期（范围查询-结束）
   */
  inboundDateEnd?: string;

  /**
   * 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
   */
  inboundType?: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位编码
   */
  locationCode?: string;

  /**
   * 总数量
   */
  totalQuantity?: number;

  /**
   * 关联公司
   */
  relatedCompany?: string;

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
 * 创建ProductSerialInbound DTO
 * 对应前端 ProductSerialInboundCreate
 * @description 对应后端 TaktProductSerialInboundCreateDto
 */
export interface ProductSerialInboundCreate {
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
   * 工厂代码(4位字母数字组合)
   */
  plantCode: string;

  /**
   * 入库单号（组合唯一索引：PlantCode + InboundNo）
   */
  inboundNo: string;

  /**
   * 入库日期
   */
  inboundDate: string;

  /**
   * 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
   */
  inboundType: number;

  /**
   * 仓库编码
   */
  warehouseCode: string;

  /**
   * 库位编码
   */
  locationCode: string;

  /**
   * 总数量
   */
  totalQuantity: number;

  /**
   * 关联公司
   */
  relatedCompany: string;

  /**
   * 产品序列号入库明细列表(主子表关系)（子表，级联保存）
   */
  items?: ProductSerialInboundItemCreate[];

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
 * 更新ProductSerialInbound DTO
 * 继承 TaktProductSerialInboundCreateDto，添加 ProductSerialInboundId 字段
 * 对应前端 ProductSerialInboundUpdate
 * @description 对应后端 TaktProductSerialInboundUpdateDto
 */
export interface ProductSerialInboundUpdate extends ProductSerialInboundCreate {
  /**
   * ProductSerialInboundID（标识要更新的实体）
   */
  productSerialInboundId: string;

}


/**
 * ProductSerialInbound 导入模板行 DTO
 * 对应前端 ProductSerialInboundTemplate
 * @description 对应后端 TaktProductSerialInboundTemplateDto
 */
export interface ProductSerialInboundTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码(4位字母数字组合)
   */
  plantCode?: string;

  /**
   * 入库单号（组合唯一索引：PlantCode + InboundNo）
   */
  inboundNo?: string;

  /**
   * 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
   */
  inboundType?: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位编码
   */
  locationCode?: string;

  /**
   * 总数量
   */
  totalQuantity?: number;

  /**
   * 关联公司
   */
  relatedCompany?: string;

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
 * ProductSerialInbound 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProductSerialInboundImport
 * @description 对应后端 TaktProductSerialInboundImportDto
 */
export interface ProductSerialInboundImport {
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
   * 工厂代码(4位字母数字组合)
   */
  plantCode?: string;

  /**
   * 入库单号（组合唯一索引：PlantCode + InboundNo）
   */
  inboundNo?: string;

  /**
   * 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
   */
  inboundType?: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位编码
   */
  locationCode?: string;

  /**
   * 总数量
   */
  totalQuantity?: number;

  /**
   * 关联公司
   */
  relatedCompany?: string;

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
 * ProductSerialInbound 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductSerialInboundExport
 * @description 对应后端 TaktProductSerialInboundExportDto
 */
export interface ProductSerialInboundExport {
  /**
   * ProductSerialInboundID
   */
  productSerialInboundId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码(4位字母数字组合)
   */
  plantCode: string;

  /**
   * 入库单号（组合唯一索引：PlantCode + InboundNo）
   */
  inboundNo: string;

  /**
   * 入库日期
   */
  inboundDate: string;

  /**
   * 入库类型(0=采购入库,1=生产入库,2=退货入库,3=调拨入库,4=序列号入库,5=其他)
   */
  inboundType: number;

  /**
   * 仓库编码
   */
  warehouseCode: string;

  /**
   * 库位编码
   */
  locationCode: string;

  /**
   * 总数量
   */
  totalQuantity: number;

  /**
   * 关联公司
   */
  relatedCompany: string;

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

