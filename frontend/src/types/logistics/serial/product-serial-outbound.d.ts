// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/serial
// 文件名称：product-serial-outbound.d.ts
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
 * 产品序列号出库主表实体
 * 对应前端 TaktProductSerialOutboundDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductSerialOutbound
 * @description 对应后端 TaktProductSerialOutboundDto
 */
export interface ProductSerialOutbound extends CompanyDtoBase {
  /**
   * ProductSerialOutboundID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  productSerialOutboundId: string;

  /**
   * 工厂代码(4位字母数字组合)
   */
  plantCode: string;

  /**
   * 出库单号(组合唯一索引:PlantCode + OutboundNo)
   */
  outboundNo: string;

  /**
   * 出货发票号
   */
  shippingInvoiceNo: string;

  /**
   * 出库日期
   */
  outboundDate: string;

  /**
   * 仕向地(目的地)
   */
  destination: string;

  /**
   * 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
   */
  shippingMethod: string;

  /**
   * 目的地港
   */
  destinationPort: string;

  /**
   * 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
   */
  outboundType: number;

  /**
   * 仓库编码
   */
  warehouseCode: string;

  /**
   * 库位编码
   */
  locationCode: string;

  /**
   * 关联公司
   */
  relatedCompany: string;

  /**
   * 总数量
   */
  totalQuantity: number;

  /**
   * 产品序列号出库明细列表（主子表关系） （子表：TaktProductSerialOutboundItem）
   */
  items?: ProductSerialOutboundItem[];

}


/**
 * ProductSerialOutbound 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ProductSerialOutboundQuery
 * @description 对应后端 TaktProductSerialOutboundQueryDto
 */
export interface ProductSerialOutboundQuery extends TaktPagedQuery {
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
   * 出库单号(组合唯一索引:PlantCode + OutboundNo)
   */
  outboundNo?: string;

  /**
   * 出货发票号
   */
  shippingInvoiceNo?: string;

  /**
   * 出库日期（范围查询-开始）
   */
  outboundDateStart?: string;

  /**
   * 出库日期（范围查询-结束）
   */
  outboundDateEnd?: string;

  /**
   * 仕向地(目的地)
   */
  destination?: string;

  /**
   * 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
   */
  shippingMethod?: string;

  /**
   * 目的地港
   */
  destinationPort?: string;

  /**
   * 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
   */
  outboundType?: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位编码
   */
  locationCode?: string;

  /**
   * 关联公司
   */
  relatedCompany?: string;

  /**
   * 总数量
   */
  totalQuantity?: number;

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
 * 创建ProductSerialOutbound DTO
 * 对应前端 ProductSerialOutboundCreate
 * @description 对应后端 TaktProductSerialOutboundCreateDto
 */
export interface ProductSerialOutboundCreate {
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
   * 出库单号(组合唯一索引:PlantCode + OutboundNo)
   */
  outboundNo: string;

  /**
   * 出货发票号
   */
  shippingInvoiceNo: string;

  /**
   * 出库日期
   */
  outboundDate: string;

  /**
   * 仕向地(目的地)
   */
  destination: string;

  /**
   * 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
   */
  shippingMethod: string;

  /**
   * 目的地港
   */
  destinationPort: string;

  /**
   * 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
   */
  outboundType: number;

  /**
   * 仓库编码
   */
  warehouseCode: string;

  /**
   * 库位编码
   */
  locationCode: string;

  /**
   * 关联公司
   */
  relatedCompany: string;

  /**
   * 总数量
   */
  totalQuantity: number;

  /**
   * 产品序列号出库明细列表（主子表关系）（子表，级联保存）
   */
  items?: ProductSerialOutboundItemCreate[];

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
 * 更新ProductSerialOutbound DTO
 * 继承 TaktProductSerialOutboundCreateDto，添加 ProductSerialOutboundId 字段
 * 对应前端 ProductSerialOutboundUpdate
 * @description 对应后端 TaktProductSerialOutboundUpdateDto
 */
export interface ProductSerialOutboundUpdate extends ProductSerialOutboundCreate {
  /**
   * ProductSerialOutboundID（标识要更新的实体）
   */
  productSerialOutboundId: string;

}


/**
 * ProductSerialOutbound 导入模板行 DTO
 * 对应前端 ProductSerialOutboundTemplate
 * @description 对应后端 TaktProductSerialOutboundTemplateDto
 */
export interface ProductSerialOutboundTemplate {
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
   * 出库单号(组合唯一索引:PlantCode + OutboundNo)
   */
  outboundNo?: string;

  /**
   * 出货发票号
   */
  shippingInvoiceNo?: string;

  /**
   * 仕向地(目的地)
   */
  destination?: string;

  /**
   * 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
   */
  shippingMethod?: string;

  /**
   * 目的地港
   */
  destinationPort?: string;

  /**
   * 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
   */
  outboundType?: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位编码
   */
  locationCode?: string;

  /**
   * 关联公司
   */
  relatedCompany?: string;

  /**
   * 总数量
   */
  totalQuantity?: number;

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
 * ProductSerialOutbound 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ProductSerialOutboundImport
 * @description 对应后端 TaktProductSerialOutboundImportDto
 */
export interface ProductSerialOutboundImport {
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
   * 出库单号(组合唯一索引:PlantCode + OutboundNo)
   */
  outboundNo?: string;

  /**
   * 出货发票号
   */
  shippingInvoiceNo?: string;

  /**
   * 仕向地(目的地)
   */
  destination?: string;

  /**
   * 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
   */
  shippingMethod?: string;

  /**
   * 目的地港
   */
  destinationPort?: string;

  /**
   * 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
   */
  outboundType?: number;

  /**
   * 仓库编码
   */
  warehouseCode?: string;

  /**
   * 库位编码
   */
  locationCode?: string;

  /**
   * 关联公司
   */
  relatedCompany?: string;

  /**
   * 总数量
   */
  totalQuantity?: number;

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
 * ProductSerialOutbound 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductSerialOutboundExport
 * @description 对应后端 TaktProductSerialOutboundExportDto
 */
export interface ProductSerialOutboundExport {
  /**
   * ProductSerialOutboundID
   */
  productSerialOutboundId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码(4位字母数字组合)
   */
  plantCode: string;

  /**
   * 出库单号(组合唯一索引:PlantCode + OutboundNo)
   */
  outboundNo: string;

  /**
   * 出货发票号
   */
  shippingInvoiceNo: string;

  /**
   * 出库日期
   */
  outboundDate: string;

  /**
   * 仕向地(目的地)
   */
  destination: string;

  /**
   * 运输方式(0=海运,1=空运,2=陆运,3=铁路,4=快递,5=其他)
   */
  shippingMethod: string;

  /**
   * 目的地港
   */
  destinationPort: string;

  /**
   * 出库类型(0=销售出库,1=生产领料,2=退货出库,3=调拨出库,4=报废出库,5=序列号出库,6=其他)
   */
  outboundType: number;

  /**
   * 仓库编码
   */
  warehouseCode: string;

  /**
   * 库位编码
   */
  locationCode: string;

  /**
   * 关联公司
   */
  relatedCompany: string;

  /**
   * 总数量
   */
  totalQuantity: number;

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

