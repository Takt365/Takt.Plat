// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：bill-of-material-substitute.d.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common';

/**
 * BOM替代料实体（挂载于物料清单明细行，一行主件可维护多条替代物料）
 * 对应前端 TaktBillOfMaterialSubstituteDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BillOfMaterialSubstitute
 * @description 对应后端 TaktBillOfMaterialSubstituteDto
 */
export interface BillOfMaterialSubstitute extends CompanyDtoBase {
  /**
   * BillOfMaterialSubstituteID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  billOfMaterialSubstituteId: string;

  /**
   * 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialItemId: string;

  /**
   * 物料清单明细名称（填充字段）
   */
  billOfMaterialItemName?: string;

  /**
   * 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId: string;

  /**
   * 物料清单名称（填充字段）
   */
  billOfMaterialName?: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode: string;

  /**
   * 主件物料编码（冗余，对应明细行子项物料编码）
   */
  primaryMaterialCode: string;

  /**
   * 替代行号（步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 替代物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  substituteMaterialId: string;

  /**
   * 替代物料名称（填充字段）
   */
  substituteMaterialName?: string;

  /**
   * 替代物料编码（冗余）
   */
  substituteMaterialCode: string;

  /**
   * 替代组号（与明细行 substitute_group 对齐，便于组内检索）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（越小越优先）
   */
  substitutePriority: number;

  /**
   * 替代用量
   */
  usageQuantity: number;

  /**
   * 单位
   */
  materialUnit: string;

  /**
   * 替代比例（相对主件用量，默认1表示等量替代）
   */
  usageRatio: number;

  /**
   * 是否启用（0=否，1=是，字典 sys_yes_no_type）
   */
  isEnabled: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期（为空表示永久有效）
   */
  expiryDate?: string;

  /**
   * 物料清单明细（主表） （主表：TaktBillOfMaterialItem）
   */
  billOfMaterialItem?: BillOfMaterialItem;

  /**
   * 替代物料（工厂物料主数据） （主表：TaktMaterialPlant）
   */
  substituteMaterialPlant?: MaterialPlant;

}


/**
 * BillOfMaterialSubstitute 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BillOfMaterialSubstituteQuery
 * @description 对应后端 TaktBillOfMaterialSubstituteQueryDto
 */
export interface BillOfMaterialSubstituteQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialItemId?: string;

  /**
   * 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId?: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode?: string;

  /**
   * 主件物料编码（冗余，对应明细行子项物料编码）
   */
  primaryMaterialCode?: string;

  /**
   * 替代行号（步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 替代物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  substituteMaterialId?: string;

  /**
   * 替代物料编码（冗余）
   */
  substituteMaterialCode?: string;

  /**
   * 替代组号（与明细行 substitute_group 对齐，便于组内检索）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（越小越优先）
   */
  substitutePriority?: number;

  /**
   * 替代用量
   */
  usageQuantity?: number;

  /**
   * 单位
   */
  materialUnit?: string;

  /**
   * 替代比例（相对主件用量，默认1表示等量替代）
   */
  usageRatio?: number;

  /**
   * 是否启用（0=否，1=是，字典 sys_yes_no_type）
   */
  isEnabled?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 失效日期（为空表示永久有效）（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 失效日期（为空表示永久有效）（范围查询-结束）
   */
  expiryDateEnd?: string;

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
 * 创建BillOfMaterialSubstitute DTO
 * 对应前端 BillOfMaterialSubstituteCreate
 * @description 对应后端 TaktBillOfMaterialSubstituteCreateDto
 */
export interface BillOfMaterialSubstituteCreate {
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
   * 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialItemId: string;

  /**
   * 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode: string;

  /**
   * 主件物料编码（冗余，对应明细行子项物料编码）
   */
  primaryMaterialCode: string;

  /**
   * 替代行号（步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 替代物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  substituteMaterialId: string;

  /**
   * 替代物料编码（冗余）
   */
  substituteMaterialCode: string;

  /**
   * 替代组号（与明细行 substitute_group 对齐，便于组内检索）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（越小越优先）
   */
  substitutePriority: number;

  /**
   * 替代用量
   */
  usageQuantity: number;

  /**
   * 单位
   */
  materialUnit: string;

  /**
   * 替代比例（相对主件用量，默认1表示等量替代）
   */
  usageRatio: number;

  /**
   * 是否启用（0=否，1=是，字典 sys_yes_no_type）
   */
  isEnabled: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期（为空表示永久有效）
   */
  expiryDate?: string;

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
 * 更新BillOfMaterialSubstitute DTO
 * 继承 TaktBillOfMaterialSubstituteCreateDto，添加 BillOfMaterialSubstituteId 字段
 * 对应前端 BillOfMaterialSubstituteUpdate
 * @description 对应后端 TaktBillOfMaterialSubstituteUpdateDto
 */
export interface BillOfMaterialSubstituteUpdate extends BillOfMaterialSubstituteCreate {
  /**
   * BillOfMaterialSubstituteID（标识要更新的实体）
   */
  billOfMaterialSubstituteId: string;

}


/**
 * BillOfMaterialSubstitute 导入模板行 DTO
 * 对应前端 BillOfMaterialSubstituteTemplate
 * @description 对应后端 TaktBillOfMaterialSubstituteTemplateDto
 */
export interface BillOfMaterialSubstituteTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialItemId?: string;

  /**
   * 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId?: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode?: string;

  /**
   * 主件物料编码（冗余，对应明细行子项物料编码）
   */
  primaryMaterialCode?: string;

  /**
   * 替代行号（步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 替代物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  substituteMaterialId?: string;

  /**
   * 替代物料编码（冗余）
   */
  substituteMaterialCode?: string;

  /**
   * 替代组号（与明细行 substitute_group 对齐，便于组内检索）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（越小越优先）
   */
  substitutePriority?: number;

  /**
   * 替代用量
   */
  usageQuantity?: number;

  /**
   * 单位
   */
  materialUnit?: string;

  /**
   * 替代比例（相对主件用量，默认1表示等量替代）
   */
  usageRatio?: number;

  /**
   * 是否启用（0=否，1=是，字典 sys_yes_no_type）
   */
  isEnabled?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期（为空表示永久有效）
   */
  expiryDate?: string;

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
 * BillOfMaterialSubstitute 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BillOfMaterialSubstituteImport
 * @description 对应后端 TaktBillOfMaterialSubstituteImportDto
 */
export interface BillOfMaterialSubstituteImport {
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
   * 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialItemId?: string;

  /**
   * 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId?: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode?: string;

  /**
   * 主件物料编码（冗余，对应明细行子项物料编码）
   */
  primaryMaterialCode?: string;

  /**
   * 替代行号（步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 替代物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  substituteMaterialId?: string;

  /**
   * 替代物料编码（冗余）
   */
  substituteMaterialCode?: string;

  /**
   * 替代组号（与明细行 substitute_group 对齐，便于组内检索）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（越小越优先）
   */
  substitutePriority?: number;

  /**
   * 替代用量
   */
  usageQuantity?: number;

  /**
   * 单位
   */
  materialUnit?: string;

  /**
   * 替代比例（相对主件用量，默认1表示等量替代）
   */
  usageRatio?: number;

  /**
   * 是否启用（0=否，1=是，字典 sys_yes_no_type）
   */
  isEnabled?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期（为空表示永久有效）
   */
  expiryDate?: string;

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
 * BillOfMaterialSubstitute 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BillOfMaterialSubstituteExport
 * @description 对应后端 TaktBillOfMaterialSubstituteExportDto
 */
export interface BillOfMaterialSubstituteExport {
  /**
   * BillOfMaterialSubstituteID
   */
  billOfMaterialSubstituteId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialItemId: string;

  /**
   * 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode: string;

  /**
   * 主件物料编码（冗余，对应明细行子项物料编码）
   */
  primaryMaterialCode: string;

  /**
   * 替代行号（步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 替代物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
   */
  substituteMaterialId: string;

  /**
   * 替代物料编码（冗余）
   */
  substituteMaterialCode: string;

  /**
   * 替代组号（与明细行 substitute_group 对齐，便于组内检索）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（越小越优先）
   */
  substitutePriority: number;

  /**
   * 替代用量
   */
  usageQuantity: number;

  /**
   * 单位
   */
  materialUnit: string;

  /**
   * 替代比例（相对主件用量，默认1表示等量替代）
   */
  usageRatio: number;

  /**
   * 是否启用（0=否，1=是，字典 sys_yes_no_type）
   */
  isEnabled: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期（为空表示永久有效）
   */
  expiryDate?: string;

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

