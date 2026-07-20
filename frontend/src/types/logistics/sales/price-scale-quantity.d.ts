// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：price-scale-quantity.d.ts
// 创建时间：2026-07-20
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
 * Takt销售价格数量等级实体（SAP KONM；主子表：TaktSalesPriceItem → ScaleQuantities；与价值等级仅差 ScaleQuantity↔ScaleValue）
 * 对应前端 TaktSalesPriceScaleQuantityDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesPriceScaleQuantity
 * @description 对应后端 TaktSalesPriceScaleQuantityDto
 */
export interface SalesPriceScaleQuantity extends CompanyDtoBase {
  /**
   * SalesPriceScaleQuantityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPriceScaleQuantityId: string;

  /**
   * 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
   */
  salesPriceItemId: string;

  /**
   * 销售价格明细 名称（填充字段）
   */
  salesPriceItemName?: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode: string;

  /**
   * 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
   */
  salesPriceSeq: number;

  /**
   * 行号（KLFN1；阶梯行序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity: number;

  /**
   * 金额（KBETR）
   */
  amount: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SalesPriceScaleQuantity 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesPriceScaleQuantityQuery
 * @description 对应后端 TaktSalesPriceScaleQuantityQueryDto
 */
export interface SalesPriceScaleQuantityQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
   */
  salesPriceItemId?: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode?: string;

  /**
   * 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
   */
  salesPriceSeq?: number;

  /**
   * 行号（KLFN1；阶梯行序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity?: number;

  /**
   * 金额（KBETR）
   */
  amount?: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建SalesPriceScaleQuantity DTO
 * 对应前端 SalesPriceScaleQuantityCreate
 * @description 对应后端 TaktSalesPriceScaleQuantityCreateDto
 */
export interface SalesPriceScaleQuantityCreate {
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
   * 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
   */
  salesPriceItemId: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode: string;

  /**
   * 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
   */
  salesPriceSeq: number;

  /**
   * 行号（KLFN1；阶梯行序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity: number;

  /**
   * 金额（KBETR）
   */
  amount: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
 * 更新SalesPriceScaleQuantity DTO
 * 继承 TaktSalesPriceScaleQuantityCreateDto，添加 SalesPriceScaleQuantityId 字段
 * 对应前端 SalesPriceScaleQuantityUpdate
 * @description 对应后端 TaktSalesPriceScaleQuantityUpdateDto
 */
export interface SalesPriceScaleQuantityUpdate extends SalesPriceScaleQuantityCreate {
  /**
   * SalesPriceScaleQuantityID（标识要更新的实体）
   */
  salesPriceScaleQuantityId: string;

}


/**
 * SalesPriceScaleQuantity 作废/撤销作废 DTO
 * 对应前端 SalesPriceScaleQuantityObsolete
 * @description 对应后端 TaktSalesPriceScaleQuantityObsoleteDto
 */
export interface SalesPriceScaleQuantityObsolete {
  /**
   * SalesPriceScaleQuantityID
   */
  salesPriceScaleQuantityId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SalesPriceScaleQuantity 导入模板行 DTO
 * 对应前端 SalesPriceScaleQuantityTemplate
 * @description 对应后端 TaktSalesPriceScaleQuantityTemplateDto
 */
export interface SalesPriceScaleQuantityTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
   */
  salesPriceItemId?: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode?: string;

  /**
   * 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
   */
  salesPriceSeq?: number;

  /**
   * 行号（KLFN1；阶梯行序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity?: number;

  /**
   * 金额（KBETR）
   */
  amount?: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * SalesPriceScaleQuantity 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesPriceScaleQuantityImport
 * @description 对应后端 TaktSalesPriceScaleQuantityImportDto
 */
export interface SalesPriceScaleQuantityImport {
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
   * 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
   */
  salesPriceItemId?: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode?: string;

  /**
   * 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
   */
  salesPriceSeq?: number;

  /**
   * 行号（KLFN1；阶梯行序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity?: number;

  /**
   * 金额（KBETR）
   */
  amount?: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * SalesPriceScaleQuantity 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesPriceScaleQuantityExport
 * @description 对应后端 TaktSalesPriceScaleQuantityExportDto
 */
export interface SalesPriceScaleQuantityExport {
  /**
   * SalesPriceScaleQuantityID
   */
  salesPriceScaleQuantityId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
   */
  salesPriceItemId: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode: string;

  /**
   * 条件序列号（KOPOS；冗余；与明细 SalesPriceSeq 一致）
   */
  salesPriceSeq: number;

  /**
   * 行号（KLFN1；阶梯行序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity: number;

  /**
   * 金额（KBETR）
   */
  amount: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

