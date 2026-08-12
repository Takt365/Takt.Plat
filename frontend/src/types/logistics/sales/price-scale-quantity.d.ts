// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：price-scale-quantity.d.ts
// 创建时间：2026-08-06
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
 * Takt销售价格数量等级实体（；主子表：TaktSalesPriceItem → ScaleQuantities；与价值等级仅差 ScaleQuantity↔ScaleValue）
 * 对应前端 TaktSalesPriceScaleQuantityDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesPriceScaleQuantity
 * @description 对应后端 TaktSalesPriceScaleQuantityDto
 */
export interface SalesPriceScaleQuantity extends CompanyDtoBase {
  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode: string

  /**
   * 区域文化编码（登录或公司切换注入）
   */
  cultureCode?: string

  /**
   * 销售价格明细 ID（主子表关系；选项 TaktSalesPriceItems/options，DictValue=Id）
   */
  salesPriceItemId?: string;

  /**
   * 定价记录号（KNUMH；冗余；与主表/明细 SalesPriceCode 一致，长度 20）
   */
  salesPriceCode?: string;

  /**
   * 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
   */
  salesPriceSeq?: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq?: number;

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
   * 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
   */
  salesPriceSeq: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq: number;

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

