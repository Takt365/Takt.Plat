// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：price-scale-value.d.ts
// 创建时间：2026-07-23
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
 * Takt销售价格价值等级实体（；主子表：TaktSalesPriceItem → ScaleValues；与数量等级仅差 ScaleValue↔ScaleQuantity）
 * 对应前端 TaktSalesPriceScaleValueDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesPriceScaleValue
 * @description 对应后端 TaktSalesPriceScaleValueDto
 */
export interface SalesPriceScaleValue extends CompanyDtoBase {
  /**
   * SalesPriceScaleValueID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesPriceScaleValueId: string;

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
   * 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
   */
  salesPriceSeq: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq: number;

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

}


/**
 * SalesPriceScaleValue 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesPriceScaleValueQuery
 * @description 对应后端 TaktSalesPriceScaleValueQueryDto
 */
export interface SalesPriceScaleValueQuery extends TaktPagedQuery {
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
   * 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
   */
  salesPriceSeq?: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq?: number;

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
 * 创建SalesPriceScaleValue DTO
 * 对应前端 SalesPriceScaleValueCreate
 * @description 对应后端 TaktSalesPriceScaleValueCreateDto
 */
export interface SalesPriceScaleValueCreate {
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
   * 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
   */
  salesPriceSeq: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq: number;

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

}


/**
 * 更新SalesPriceScaleValue DTO
 * 继承 TaktSalesPriceScaleValueCreateDto，添加 SalesPriceScaleValueId 字段
 * 对应前端 SalesPriceScaleValueUpdate
 * @description 对应后端 TaktSalesPriceScaleValueUpdateDto
 */
export interface SalesPriceScaleValueUpdate extends SalesPriceScaleValueCreate {
  /**
   * SalesPriceScaleValueID（标识要更新的实体）
   */
  salesPriceScaleValueId: string;

}


/**
 * SalesPriceScaleValue 作废/撤销作废 DTO
 * 对应前端 SalesPriceScaleValueObsolete
 * @description 对应后端 TaktSalesPriceScaleValueObsoleteDto
 */
export interface SalesPriceScaleValueObsolete {
  /**
   * SalesPriceScaleValueID
   */
  salesPriceScaleValueId: string;

  /**
   * 是否作废（字典 sys_yes_no_type，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * SalesPriceScaleValue 导入模板行 DTO
 * 对应前端 SalesPriceScaleValueTemplate
 * @description 对应后端 TaktSalesPriceScaleValueTemplateDto
 */
export interface SalesPriceScaleValueTemplate {
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
   * 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
   */
  salesPriceSeq?: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq?: number;

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
 * SalesPriceScaleValue 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesPriceScaleValueImport
 * @description 对应后端 TaktSalesPriceScaleValueImportDto
 */
export interface SalesPriceScaleValueImport {
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
   * 定价序号（冗余；与明细 SalesPriceSeq 一致，固定步长=10）
   */
  salesPriceSeq?: number;

  /**
   * 等级序号（KOPOS；同一明细内阶梯序号，固定步长=10）
   */
  salesScaleSeq?: number;

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
 * SalesPriceScaleValue 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesPriceScaleValueExport
 * @description 对应后端 TaktSalesPriceScaleValueExportDto
 */
export interface SalesPriceScaleValueExport {
  /**
   * SalesPriceScaleValueID
   */
  salesPriceScaleValueId: string;

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

