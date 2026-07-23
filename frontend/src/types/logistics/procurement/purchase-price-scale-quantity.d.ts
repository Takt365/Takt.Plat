// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-price-scale-quantity.d.ts
// 创建时间：2026-07-21
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
 * Takt采购价格数量等级实体（SAP KONM；主子表：TaktPurchasePriceItem → ScaleQuantities；与价值等级仅差 ScaleQuantity↔ScaleValue）
 * 对应前端 TaktPurchasePriceScaleQuantityDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchasePriceScaleQuantity
 * @description 对应后端 TaktPurchasePriceScaleQuantityDto
 */
export interface PurchasePriceScaleQuantity extends CompanyDtoBase {
  /**
   * PurchasePriceScaleQuantityID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  purchasePriceScaleQuantityId: string;

  /**
   * 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
   */
  purchasePriceItemId: string;

  /**
   * 采购价格明细 名称（填充字段）
   */
  purchasePriceItemName?: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode: string;

  /**
   * 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
   */
  purchasePriceSeq: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity: number;

  /**
   * 价格（KBETR）
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchasePriceScaleQuantity 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PurchasePriceScaleQuantityQuery
 * @description 对应后端 TaktPurchasePriceScaleQuantityQueryDto
 */
export interface PurchasePriceScaleQuantityQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
   */
  purchasePriceItemId?: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode?: string;

  /**
   * 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
   */
  purchasePriceSeq?: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq?: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity?: number;

  /**
   * 价格（KBETR）
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

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
 * 创建PurchasePriceScaleQuantity DTO
 * 对应前端 PurchasePriceScaleQuantityCreate
 * @description 对应后端 TaktPurchasePriceScaleQuantityCreateDto
 */
export interface PurchasePriceScaleQuantityCreate {
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
   * 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
   */
  purchasePriceItemId: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode: string;

  /**
   * 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
   */
  purchasePriceSeq: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity: number;

  /**
   * 价格（KBETR）
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

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
 * 更新PurchasePriceScaleQuantity DTO
 * 继承 TaktPurchasePriceScaleQuantityCreateDto，添加 PurchasePriceScaleQuantityId 字段
 * 对应前端 PurchasePriceScaleQuantityUpdate
 * @description 对应后端 TaktPurchasePriceScaleQuantityUpdateDto
 */
export interface PurchasePriceScaleQuantityUpdate extends PurchasePriceScaleQuantityCreate {
  /**
   * PurchasePriceScaleQuantityID（标识要更新的实体）
   */
  purchasePriceScaleQuantityId: string;

}


/**
 * PurchasePriceScaleQuantity 作废/撤销作废 DTO
 * 对应前端 PurchasePriceScaleQuantityObsolete
 * @description 对应后端 TaktPurchasePriceScaleQuantityObsoleteDto
 */
export interface PurchasePriceScaleQuantityObsolete {
  /**
   * PurchasePriceScaleQuantityID
   */
  purchasePriceScaleQuantityId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * PurchasePriceScaleQuantity 导入模板行 DTO
 * 对应前端 PurchasePriceScaleQuantityTemplate
 * @description 对应后端 TaktPurchasePriceScaleQuantityTemplateDto
 */
export interface PurchasePriceScaleQuantityTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司代码（登录或公司切换注入，对应请求头 X-Company-Code）
   */
  companyCode?: string;

  /**
   * 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
   */
  purchasePriceItemId?: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode?: string;

  /**
   * 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
   */
  purchasePriceSeq?: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq?: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity?: number;

  /**
   * 价格（KBETR）
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

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
 * PurchasePriceScaleQuantity 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PurchasePriceScaleQuantityImport
 * @description 对应后端 TaktPurchasePriceScaleQuantityImportDto
 */
export interface PurchasePriceScaleQuantityImport {
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
   * 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
   */
  purchasePriceItemId?: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode?: string;

  /**
   * 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
   */
  purchasePriceSeq?: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq?: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity?: number;

  /**
   * 价格（KBETR）
   */
  price?: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice?: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice?: number;

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
 * PurchasePriceScaleQuantity 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchasePriceScaleQuantityExport
 * @description 对应后端 TaktPurchasePriceScaleQuantityExportDto
 */
export interface PurchasePriceScaleQuantityExport {
  /**
   * PurchasePriceScaleQuantityID
   */
  purchasePriceScaleQuantityId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购价格明细 ID（主子表关系；选项 TaktPurchasePriceItems/options，DictValue=Id）
   */
  purchasePriceItemId: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 PurchasePriceCode 一致，长度 20）
   */
  purchasePriceCode: string;

  /**
   * 定价序号（冗余；与明细 PurchasePriceSeq 一致，固定步长=10）
   */
  purchasePriceSeq: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  purchaseScaleSeq: number;

  /**
   * 等级数量（KSTBM；数量等级门槛；对应价值等级表的 ScaleValue）
   */
  scaleQuantity: number;

  /**
   * 价格（KBETR）
   */
  price: number;

  /**
   * 未税价格（冗余；可由 Price 与税码推算后回写）
   */
  untaxedPrice: number;

  /**
   * 含税价格（冗余；可由 Price 与税码推算后回写）
   */
  taxIncludedPrice: number;

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

