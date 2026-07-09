// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：source-of-supply.d.ts
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * Takt货源清单实体（公司级；工厂+物料+供货商维度的有效货源清单记录）
 * 对应前端 TaktSourceOfSupplyDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SourceOfSupply
 * @description 对应后端 TaktSourceOfSupplyDto
 */
export interface SourceOfSupply extends CompanyDtoBase {
  /**
   * SourceOfSupplyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sourceOfSupplyId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 货源清单编码（租户+公司内唯一；业务单据号）
   */
  sourceOfSupplyCode: string;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options，DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 固定（字典 sys_yes_no_type；1=是，0=否；固定货源清单，MRP/寻源优先选用）
   */
  isFixed: number;

  /**
   * 冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
   */
  isBlocked: number;

  /**
   * 采购单位
   */
  purchaseUnit: string;

  /**
   * 最小订购量
   */
  minimumOrderQuantity: number;

  /**
   * 计划交货天数（采购提前期）
   */
  leadTimeDays: number;

  /**
   * 框架协议号（采购合同/协议编号，可选）
   */
  agreementNumber?: string;

  /**
   * 协议行号
   */
  agreementLineNumber?: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 排序号（越小越靠前；同物料多货源清单时的优先级）
   */
  sortOrder: number;

  /**
   * 货源清单状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  sourceStatus: number;

}


/**
 * SourceOfSupply 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SourceOfSupplyQuery
 * @description 对应后端 TaktSourceOfSupplyQueryDto
 */
export interface SourceOfSupplyQuery extends TaktPagedQuery {
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
   * 货源清单编码（租户+公司内唯一；业务单据号）
   */
  sourceOfSupplyCode?: string;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options，DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 固定（字典 sys_yes_no_type；1=是，0=否；固定货源清单，MRP/寻源优先选用）
   */
  isFixed?: number;

  /**
   * 冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
   */
  isBlocked?: number;

  /**
   * 采购单位
   */
  purchaseUnit?: string;

  /**
   * 最小订购量
   */
  minimumOrderQuantity?: number;

  /**
   * 计划交货天数（采购提前期）
   */
  leadTimeDays?: number;

  /**
   * 框架协议号（采购合同/协议编号，可选）
   */
  agreementNumber?: string;

  /**
   * 协议行号
   */
  agreementLineNumber?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  validFromStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  validFromEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  validToStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  validToEnd?: string;

  /**
   * 排序号（越小越靠前；同物料多货源清单时的优先级）
   */
  sortOrder?: number;

  /**
   * 货源清单状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  sourceStatus?: number;

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
 * 创建SourceOfSupply DTO
 * 对应前端 SourceOfSupplyCreate
 * @description 对应后端 TaktSourceOfSupplyCreateDto
 */
export interface SourceOfSupplyCreate {
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
   * 货源清单编码（租户+公司内唯一；业务单据号）
   */
  sourceOfSupplyCode: string;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options，DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 固定（字典 sys_yes_no_type；1=是，0=否；固定货源清单，MRP/寻源优先选用）
   */
  isFixed: number;

  /**
   * 冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
   */
  isBlocked: number;

  /**
   * 采购单位
   */
  purchaseUnit: string;

  /**
   * 最小订购量
   */
  minimumOrderQuantity: number;

  /**
   * 计划交货天数（采购提前期）
   */
  leadTimeDays: number;

  /**
   * 框架协议号（采购合同/协议编号，可选）
   */
  agreementNumber?: string;

  /**
   * 协议行号
   */
  agreementLineNumber?: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 货源清单状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  sourceStatus: number;

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
 * 更新SourceOfSupply DTO
 * 继承 TaktSourceOfSupplyCreateDto，添加 SourceOfSupplyId 字段
 * 对应前端 SourceOfSupplyUpdate
 * @description 对应后端 TaktSourceOfSupplyUpdateDto
 */
export interface SourceOfSupplyUpdate extends SourceOfSupplyCreate {
  /**
   * SourceOfSupplyID（标识要更新的实体）
   */
  sourceOfSupplyId: string;

}


/**
 * SourceOfSupply 状态更新 DTO
 * 对应前端 SourceOfSupplyStatus
 * @description 对应后端 TaktSourceOfSupplyStatusDto
 */
export interface SourceOfSupplyStatus {
  /**
   * SourceOfSupplyID
   */
  sourceOfSupplyId: string;

  /**
   * 货源清单状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  sourceStatus: number;

}


/**
 * SourceOfSupply 排序更新 DTO
 * 对应前端 SourceOfSupplySort
 * @description 对应后端 TaktSourceOfSupplySortDto
 */
export interface SourceOfSupplySort {
  /**
   * SourceOfSupplyID
   */
  sourceOfSupplyId: string;

  /**
   * 排序号（越小越靠前；同物料多货源清单时的优先级）
   */
  sortOrder: number;

}


/**
 * SourceOfSupply 导入模板行 DTO
 * 对应前端 SourceOfSupplyTemplate
 * @description 对应后端 TaktSourceOfSupplyTemplateDto
 */
export interface SourceOfSupplyTemplate {
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
   * 货源清单编码（租户+公司内唯一；业务单据号）
   */
  sourceOfSupplyCode?: string;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options，DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 固定（字典 sys_yes_no_type；1=是，0=否；固定货源清单，MRP/寻源优先选用）
   */
  isFixed?: number;

  /**
   * 冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
   */
  isBlocked?: number;

  /**
   * 采购单位
   */
  purchaseUnit?: string;

  /**
   * 最小订购量
   */
  minimumOrderQuantity?: number;

  /**
   * 计划交货天数（采购提前期）
   */
  leadTimeDays?: number;

  /**
   * 框架协议号（采购合同/协议编号，可选）
   */
  agreementNumber?: string;

  /**
   * 协议行号
   */
  agreementLineNumber?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 货源清单状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  sourceStatus?: number;

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
 * SourceOfSupply 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SourceOfSupplyImport
 * @description 对应后端 TaktSourceOfSupplyImportDto
 */
export interface SourceOfSupplyImport {
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
   * 货源清单编码（租户+公司内唯一；业务单据号）
   */
  sourceOfSupplyCode?: string;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode?: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options，DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 固定（字典 sys_yes_no_type；1=是，0=否；固定货源清单，MRP/寻源优先选用）
   */
  isFixed?: number;

  /**
   * 冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
   */
  isBlocked?: number;

  /**
   * 采购单位
   */
  purchaseUnit?: string;

  /**
   * 最小订购量
   */
  minimumOrderQuantity?: number;

  /**
   * 计划交货天数（采购提前期）
   */
  leadTimeDays?: number;

  /**
   * 框架协议号（采购合同/协议编号，可选）
   */
  agreementNumber?: string;

  /**
   * 协议行号
   */
  agreementLineNumber?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 货源清单状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  sourceStatus?: number;

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
 * SourceOfSupply 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SourceOfSupplyExport
 * @description 对应后端 TaktSourceOfSupplyExportDto
 */
export interface SourceOfSupplyExport {
  /**
   * SourceOfSupplyID
   */
  sourceOfSupplyId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 货源清单编码（租户+公司内唯一；业务单据号）
   */
  sourceOfSupplyCode: string;

  /**
   * 物料编码（选项 TaktMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options，DictValue=SupplierCode）
   */
  supplierCode: string;

  /**
   * 采购组（选项 TaktPurchaseGroups/options，DictValue=PurchaseGroupCode）
   */
  purchaseGroup?: string;

  /**
   * 固定（字典 sys_yes_no_type；1=是，0=否；固定货源清单，MRP/寻源优先选用）
   */
  isFixed: number;

  /**
   * 冻结（字典 sys_yes_no_type；1=是，0=否；冻结后禁止新建采购订单引用）
   */
  isBlocked: number;

  /**
   * 采购单位
   */
  purchaseUnit: string;

  /**
   * 最小订购量
   */
  minimumOrderQuantity: number;

  /**
   * 计划交货天数（采购提前期）
   */
  leadTimeDays: number;

  /**
   * 框架协议号（采购合同/协议编号，可选）
   */
  agreementNumber?: string;

  /**
   * 协议行号
   */
  agreementLineNumber?: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 排序号（越小越靠前；同物料多货源清单时的优先级）
   */
  sortOrder: number;

  /**
   * 货源清单状态（字典 sys_normal_disable_status；1=启用，0=禁用）
   */
  sourceStatus: number;

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

