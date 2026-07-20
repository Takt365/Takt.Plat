// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/serial
// 文件名称：upload.d.ts
// 创建时间：2026-07-20
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
 * 序列号上传（公司级；发货票维度的送货/装箱明细行）
 * 对应前端 TaktSerialUploadDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SerialUpload
 * @description 对应后端 TaktSerialUploadDto
 */
export interface SerialUpload extends CompanyDtoBase {
  /**
   * SerialUploadID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  serialUploadId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 出库日期
   */
  outboundDate: string;

  /**
   * 发货单号（固定 9 位）
   */
  shippingInvoiceNo: string;

  /**
   * 序号（同一工厂+发货单号内唯一）
   */
  sequenceNo: number;

  /**
   * 产品物料（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
   */
  materialCode: string;

  /**
   * 合计数量
   */
  totalQuantity: number;

  /**
   * 序列号（固定 7 位）
   */
  serialNo: string;

  /**
   * 装箱数量
   */
  packingQuantity: number;

  /**
   * 运输方式（最长 20）
   */
  transportMode: string;

  /**
   * 物料描述（最长 40）
   */
  materialText: string;

}


/**
 * SerialUpload 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SerialUploadQuery
 * @description 对应后端 TaktSerialUploadQueryDto
 */
export interface SerialUploadQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 出库日期（范围查询-开始）
   */
  outboundDateStart?: string;

  /**
   * 出库日期（范围查询-结束）
   */
  outboundDateEnd?: string;

  /**
   * 发货单号（固定 9 位）
   */
  shippingInvoiceNo?: string;

  /**
   * 序号（同一工厂+发货单号内唯一）
   */
  sequenceNo?: number;

  /**
   * 产品物料（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
   */
  materialCode?: string;

  /**
   * 合计数量
   */
  totalQuantity?: number;

  /**
   * 序列号（固定 7 位）
   */
  serialNo?: string;

  /**
   * 装箱数量
   */
  packingQuantity?: number;

  /**
   * 运输方式（最长 20）
   */
  transportMode?: string;

  /**
   * 物料描述（最长 40）
   */
  materialText?: string;

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
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建SerialUpload DTO
 * 对应前端 SerialUploadCreate
 * @description 对应后端 TaktSerialUploadCreateDto
 */
export interface SerialUploadCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 出库日期
   */
  outboundDate: string;

  /**
   * 发货单号（固定 9 位）
   */
  shippingInvoiceNo: string;

  /**
   * 序号（同一工厂+发货单号内唯一）
   */
  sequenceNo: number;

  /**
   * 产品物料（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
   */
  materialCode: string;

  /**
   * 合计数量
   */
  totalQuantity: number;

  /**
   * 序列号（固定 7 位）
   */
  serialNo: string;

  /**
   * 装箱数量
   */
  packingQuantity: number;

  /**
   * 运输方式（最长 20）
   */
  transportMode: string;

  /**
   * 物料描述（最长 40）
   */
  materialText: string;

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
 * 更新SerialUpload DTO
 * 继承 TaktSerialUploadCreateDto，添加 SerialUploadId 字段
 * 对应前端 SerialUploadUpdate
 * @description 对应后端 TaktSerialUploadUpdateDto
 */
export interface SerialUploadUpdate extends SerialUploadCreate {
  /**
   * SerialUploadID（标识要更新的实体）
   */
  serialUploadId: string;

}


/**
 * SerialUpload 导入模板行 DTO
 * 对应前端 SerialUploadTemplate
 * @description 对应后端 TaktSerialUploadTemplateDto
 */
export interface SerialUploadTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 发货单号（固定 9 位）
   */
  shippingInvoiceNo?: string;

  /**
   * 序号（同一工厂+发货单号内唯一）
   */
  sequenceNo?: number;

  /**
   * 产品物料（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
   */
  materialCode?: string;

  /**
   * 合计数量
   */
  totalQuantity?: number;

  /**
   * 序列号（固定 7 位）
   */
  serialNo?: string;

  /**
   * 装箱数量
   */
  packingQuantity?: number;

  /**
   * 运输方式（最长 20）
   */
  transportMode?: string;

  /**
   * 物料描述（最长 40）
   */
  materialText?: string;

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
 * SerialUpload 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SerialUploadImport
 * @description 对应后端 TaktSerialUploadImportDto
 */
export interface SerialUploadImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  companyDefaultCulture?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 出库日期
   */
  outboundDate?: string;

  /**
   * 发货单号（固定 9 位）
   */
  shippingInvoiceNo?: string;

  /**
   * 序号（同一工厂+发货单号内唯一）
   */
  sequenceNo?: number;

  /**
   * 产品物料（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
   */
  materialCode?: string;

  /**
   * 合计数量
   */
  totalQuantity?: number;

  /**
   * 序列号（固定 7 位）
   */
  serialNo?: string;

  /**
   * 装箱数量
   */
  packingQuantity?: number;

  /**
   * 运输方式（最长 20）
   */
  transportMode?: string;

  /**
   * 物料描述（最长 40）
   */
  materialText?: string;

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
 * SerialUpload 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SerialUploadExport
 * @description 对应后端 TaktSerialUploadExportDto
 */
export interface SerialUploadExport {
  /**
   * SerialUploadID
   */
  serialUploadId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 出库日期
   */
  outboundDate: string;

  /**
   * 发货单号（固定 9 位）
   */
  shippingInvoiceNo: string;

  /**
   * 序号（同一工厂+发货单号内唯一）
   */
  sequenceNo: number;

  /**
   * 产品物料（选项 TaktMaterialPlants/options，DictValue=MaterialCode，ExtValue=PlantCode；最长 20）
   */
  materialCode: string;

  /**
   * 合计数量
   */
  totalQuantity: number;

  /**
   * 序列号（固定 7 位）
   */
  serialNo: string;

  /**
   * 装箱数量
   */
  packingQuantity: number;

  /**
   * 运输方式（最长 20）
   */
  transportMode: string;

  /**
   * 物料描述（最长 40）
   */
  materialText: string;

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

