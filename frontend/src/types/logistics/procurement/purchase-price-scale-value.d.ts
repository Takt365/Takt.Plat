// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchase-price-scale-value.d.ts
// 创建时间：2026-08-10
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
 * Takt采购价格价值等级实体（；主子表：TaktPurchasePriceItem → ScaleValues；与数量等级仅差 ScaleValue↔ScaleQuantity）
 * 对应前端 TaktPurchasePriceScaleValueDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchasePriceScaleValue
 * @description 对应后端 TaktPurchasePriceScaleValueDto
 */
export interface PurchasePriceScaleValue extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

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
   * 等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）
   */
  scaleValue?: number;

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
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount?: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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
 * PurchasePriceScaleValue 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchasePriceScaleValueExport
 * @description 对应后端 TaktPurchasePriceScaleValueExportDto
 */
export interface PurchasePriceScaleValueExport {
  /**
   * PurchasePriceScaleValueID
   */
  purchasePriceScaleValueId: string;

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
   * 等级值（KSTBW；价值等级门槛；对应数量等级表的 ScaleQuantity）
   */
  scaleValue: number;

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
   * 税费（冗余；含税−未税，打印用）
   */
  taxAmount: number;

  /**
   * 是否作废（字典 sys_yes_no_type；0=否 1=是；编辑移除子行时标记作废）
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

