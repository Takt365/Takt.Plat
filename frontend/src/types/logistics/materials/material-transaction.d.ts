// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：material-transaction.d.ts
// 创建时间：2026-06-20
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt物料交易主表实体（公司级；覆盖后勤模块收发货、库内作业、领借还与调拨核销等业务）
 * 对应前端 TaktMaterialTransactionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialTransaction
 * @description 对应后端 TaktMaterialTransactionDto
 */
export interface MaterialTransaction extends CompanyDtoBase {
  /**
   * MaterialTransactionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialTransactionId: string;

  /**
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 物料交易单号（租户+公司+工厂内唯一）
   */
  materialTransactionCode: string;

  /**
   * 交易日期
   */
  transactionDate: string;

  /**
   * 交易方向（0=入库，1=出库，2=库内/移库）
   */
  transactionDirection: number;

  /**
   * 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
   */
  transactionType: number;

  /**
   * 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
   */
  businessAction: number;

  /**
   * 来源单号（采购订单、销售订单、生产订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 往来方编码（供应商、客户或部门等业务编码）
   */
  partnerCode?: string;

  /**
   * 往来方名称
   */
  partnerName?: string;

  /**
   * 源仓库编码（关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode: string;

  /**
   * 源库位编码（关联 TaktStorageLocation.LocationCode）
   */
  locationCode: string;

  /**
   * 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
   */
  targetWarehouseCode?: string;

  /**
   * 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
   */
  targetLocationCode?: string;

  /**
   * 关联公司
   */
  relatedCompany: string;

  /**
   * 交易总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 交易状态（0=草稿，1=已过账，2=已作废）
   */
  transactionStatus: number;

  /**
   * 过账日期
   */
  postedDate?: string;

  /**
   * 过账人（人员代码）
   */
  postedBy?: string;

  /**
   * 物料交易明细列表（主子表关系） （子表：TaktMaterialTransactionItem）
   */
  items?: MaterialTransactionItem[];

}


/**
 * MaterialTransaction 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialTransactionQuery
 * @description 对应后端 TaktMaterialTransactionQueryDto
 */
export interface MaterialTransactionQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 物料交易单号（租户+公司+工厂内唯一）
   */
  materialTransactionCode?: string;

  /**
   * 交易日期（范围查询-开始）
   */
  transactionDateStart?: string;

  /**
   * 交易日期（范围查询-结束）
   */
  transactionDateEnd?: string;

  /**
   * 交易方向（0=入库，1=出库，2=库内/移库）
   */
  transactionDirection?: number;

  /**
   * 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
   */
  transactionType?: number;

  /**
   * 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
   */
  businessAction?: number;

  /**
   * 来源单号（采购订单、销售订单、生产订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 往来方编码（供应商、客户或部门等业务编码）
   */
  partnerCode?: string;

  /**
   * 往来方名称
   */
  partnerName?: string;

  /**
   * 源仓库编码（关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 源库位编码（关联 TaktStorageLocation.LocationCode）
   */
  locationCode?: string;

  /**
   * 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
   */
  targetWarehouseCode?: string;

  /**
   * 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
   */
  targetLocationCode?: string;

  /**
   * 关联公司
   */
  relatedCompany?: string;

  /**
   * 交易总数量（基本单位数量）
   */
  totalQuantity?: number;

  /**
   * 交易状态（0=草稿，1=已过账，2=已作废）
   */
  transactionStatus?: number;

  /**
   * 过账日期（范围查询-开始）
   */
  postedDateStart?: string;

  /**
   * 过账日期（范围查询-结束）
   */
  postedDateEnd?: string;

  /**
   * 过账人（人员代码）
   */
  postedBy?: string;

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
 * 创建MaterialTransaction DTO
 * 对应前端 MaterialTransactionCreate
 * @description 对应后端 TaktMaterialTransactionCreateDto
 */
export interface MaterialTransactionCreate {
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
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 物料交易单号（租户+公司+工厂内唯一）
   */
  materialTransactionCode: string;

  /**
   * 交易日期
   */
  transactionDate: string;

  /**
   * 交易方向（0=入库，1=出库，2=库内/移库）
   */
  transactionDirection: number;

  /**
   * 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
   */
  transactionType: number;

  /**
   * 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
   */
  businessAction: number;

  /**
   * 来源单号（采购订单、销售订单、生产订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 往来方编码（供应商、客户或部门等业务编码）
   */
  partnerCode?: string;

  /**
   * 往来方名称
   */
  partnerName?: string;

  /**
   * 源仓库编码（关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode: string;

  /**
   * 源库位编码（关联 TaktStorageLocation.LocationCode）
   */
  locationCode: string;

  /**
   * 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
   */
  targetWarehouseCode?: string;

  /**
   * 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
   */
  targetLocationCode?: string;

  /**
   * 关联公司
   */
  relatedCompany: string;

  /**
   * 交易总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 交易状态（0=草稿，1=已过账，2=已作废）
   */
  transactionStatus: number;

  /**
   * 过账日期
   */
  postedDate?: string;

  /**
   * 过账人（人员代码）
   */
  postedBy?: string;

  /**
   * 物料交易明细列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialTransactionItemCreate[];

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
 * 更新MaterialTransaction DTO
 * 继承 TaktMaterialTransactionCreateDto，添加 MaterialTransactionId 字段
 * 对应前端 MaterialTransactionUpdate
 * @description 对应后端 TaktMaterialTransactionUpdateDto
 */
export interface MaterialTransactionUpdate extends MaterialTransactionCreate {
  /**
   * MaterialTransactionID（标识要更新的实体）
   */
  materialTransactionId: string;

}


/**
 * MaterialTransaction 状态更新 DTO
 * 对应前端 MaterialTransactionStatus
 * @description 对应后端 TaktMaterialTransactionStatusDto
 */
export interface MaterialTransactionStatus {
  /**
   * MaterialTransactionID
   */
  materialTransactionId: string;

  /**
   * 交易状态（0=草稿，1=已过账，2=已作废）
   */
  transactionStatus: number;

}


/**
 * MaterialTransaction 导入模板行 DTO
 * 对应前端 MaterialTransactionTemplate
 * @description 对应后端 TaktMaterialTransactionTemplateDto
 */
export interface MaterialTransactionTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 物料交易单号（租户+公司+工厂内唯一）
   */
  materialTransactionCode?: string;

  /**
   * 交易方向（0=入库，1=出库，2=库内/移库）
   */
  transactionDirection?: number;

  /**
   * 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
   */
  transactionType?: number;

  /**
   * 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
   */
  businessAction?: number;

  /**
   * 来源单号（采购订单、销售订单、生产订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 往来方编码（供应商、客户或部门等业务编码）
   */
  partnerCode?: string;

  /**
   * 往来方名称
   */
  partnerName?: string;

  /**
   * 源仓库编码（关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 源库位编码（关联 TaktStorageLocation.LocationCode）
   */
  locationCode?: string;

  /**
   * 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
   */
  targetWarehouseCode?: string;

  /**
   * 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
   */
  targetLocationCode?: string;

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
 * MaterialTransaction 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialTransactionImport
 * @description 对应后端 TaktMaterialTransactionImportDto
 */
export interface MaterialTransactionImport {
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
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode?: string;

  /**
   * 物料交易单号（租户+公司+工厂内唯一）
   */
  materialTransactionCode?: string;

  /**
   * 交易方向（0=入库，1=出库，2=库内/移库）
   */
  transactionDirection?: number;

  /**
   * 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
   */
  transactionType?: number;

  /**
   * 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
   */
  businessAction?: number;

  /**
   * 来源单号（采购订单、销售订单、生产订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 往来方编码（供应商、客户或部门等业务编码）
   */
  partnerCode?: string;

  /**
   * 往来方名称
   */
  partnerName?: string;

  /**
   * 源仓库编码（关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode?: string;

  /**
   * 源库位编码（关联 TaktStorageLocation.LocationCode）
   */
  locationCode?: string;

  /**
   * 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
   */
  targetWarehouseCode?: string;

  /**
   * 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
   */
  targetLocationCode?: string;

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
 * MaterialTransaction 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialTransactionExport
 * @description 对应后端 TaktMaterialTransactionExportDto
 */
export interface MaterialTransactionExport {
  /**
   * MaterialTransactionID
   */
  materialTransactionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（4位字母数字组合，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 物料交易单号（租户+公司+工厂内唯一）
   */
  materialTransactionCode: string;

  /**
   * 交易日期
   */
  transactionDate: string;

  /**
   * 交易方向（0=入库，1=出库，2=库内/移库）
   */
  transactionDirection: number;

  /**
   * 交易类型（direction=0 时字典 logistics_inbound_type；direction=1 时字典 logistics_outbound_type；direction=2 时 0=移库，1=盘点，2=调整，3=报废，4=调拨，5=核销，6=其他）
   */
  transactionType: number;

  /**
   * 业务动作（与后勤扩展按钮权限后缀对齐：0=receive，1=shipping，2=returns，3=transfer，4=stocktake，5=adjust，6=scrap，7=requisition，8=secondment，9=restore，10=lossreport，11=allot，12=writeoff，13=其他）
   */
  businessAction: number;

  /**
   * 来源单号（采购订单、销售订单、生产订单等业务来源编码）
   */
  sourceCode?: string;

  /**
   * 往来方编码（供应商、客户或部门等业务编码）
   */
  partnerCode?: string;

  /**
   * 往来方名称
   */
  partnerName?: string;

  /**
   * 源仓库编码（关联 TaktWarehouse.WarehouseCode）
   */
  warehouseCode: string;

  /**
   * 源库位编码（关联 TaktStorageLocation.LocationCode）
   */
  locationCode: string;

  /**
   * 目标仓库编码（移库/调拨时使用，关联 TaktWarehouse.WarehouseCode）
   */
  targetWarehouseCode?: string;

  /**
   * 目标库位编码（移库/调拨时使用，关联 TaktStorageLocation.LocationCode）
   */
  targetLocationCode?: string;

  /**
   * 关联公司
   */
  relatedCompany: string;

  /**
   * 交易总数量（基本单位数量）
   */
  totalQuantity: number;

  /**
   * 交易状态（0=草稿，1=已过账，2=已作废）
   */
  transactionStatus: number;

  /**
   * 过账日期
   */
  postedDate?: string;

  /**
   * 过账人（人员代码）
   */
  postedBy?: string;

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

