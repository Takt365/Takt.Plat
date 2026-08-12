// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/materials/inventory-impairment-provision/composables
// 文件名称：use-inventory-impairment-provision-i18n.ts
// 功能描述：存货跌价准备实体字段清单 + useInventoryImpairmentProvisionI18n（字段名映射一次，文案由 entity.inventoryimpairmentprovision.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { InventoryImpairmentProvisionQuery } from '@/types/logistics/materials/inventory-impairment-provision'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktInventoryImpairmentProvisionI18nSeedData 一致的实体 slug */
export const INVENTORYIMPAIRMENTPROVISION_ENTITY_SLUG = 'inventoryimpairmentprovision'

/** entity.inventoryimpairmentprovision._self 静态属性（导入组件 entity-i18n-key 等） */
export const INVENTORYIMPAIRMENTPROVISION_SELF_I18N_KEY = buildEntitySelfI18nKey(INVENTORYIMPAIRMENTPROVISION_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const INVENTORYIMPAIRMENTPROVISION_LIST_FIELDS = [
  'plantCode',
  'periodDate',
  'materialCode',
  'materialDescription',
  'valuation',
  'provisionScope',
  'stockQuantity',
  'unitCost',
  'inventoryCost',
  'estimatedSellingPrice',
  'estimatedCompletionCost',
  'estimatedSellingCost',
  'netRealizableValue',
  'unitNetRealizableValue',
  'openingProvision',
  'provisionAmount',
  'reversalAmount',
  'closingProvision',
  'impairmentLoss',
  'carryingAmount',
  'currencyCode',
  'impairmentReason',
  'provisionStatus',
] as const

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const INVENTORYIMPAIRMENTPROVISION_PLACEHOLDER = {
  tenantCode: 'optional',
  companyCode: 'optional',
  companyDefaultCulture: 'optional',
  plantCode: 'select',
  periodDate: 'select',
  materialCode: 'select',
  materialDescription: 'optional',
  valuation: 'select',
  provisionScope: 'select',
  stockQuantity: 'select',
  unitCost: 'select',
  inventoryCost: 'select',
  estimatedSellingPrice: 'select',
  estimatedCompletionCost: 'select',
  estimatedSellingCost: 'select',
  netRealizableValue: 'select',
  unitNetRealizableValue: 'select',
  openingProvision: 'select',
  provisionAmount: 'select',
  reversalAmount: 'select',
  closingProvision: 'select',
  impairmentLoss: 'select',
  carryingAmount: 'select',
  currencyCode: 'select',
  impairmentReason: 'optional',
  provisionStatus: 'select',
  extField: 'optional',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type InventoryImpairmentProvisionField = keyof typeof INVENTORYIMPAIRMENTPROVISION_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const INVENTORYIMPAIRMENTPROVISION_QUERY_STRING_FIELDS = [
  'plantCode',
  'periodDateStart',
  'periodDateEnd',
  'materialCode',
  'materialDescription',
  'valuation',
  'currencyCode',
  'impairmentReason',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof InventoryImpairmentProvisionQuery)[]

export type InventoryImpairmentProvisionQueryField =
  | (typeof INVENTORYIMPAIRMENTPROVISION_QUERY_STRING_FIELDS)[number]
  | 'provisionScope' | 'stockQuantity' | 'unitCost' | 'inventoryCost' | 'estimatedSellingPrice' | 'estimatedCompletionCost' | 'estimatedSellingCost' | 'netRealizableValue' | 'unitNetRealizableValue' | 'openingProvision' | 'provisionAmount' | 'reversalAmount' | 'closingProvision' | 'impairmentLoss' | 'carryingAmount' | 'provisionStatus'

/** 高级查询抽屉全部字段（含数值） */
export const INVENTORYIMPAIRMENTPROVISION_QUERY_FIELDS: readonly InventoryImpairmentProvisionQueryField[] = [
  ...INVENTORYIMPAIRMENTPROVISION_QUERY_STRING_FIELDS,
  'provisionScope',
  'stockQuantity',
  'unitCost',
  'inventoryCost',
  'estimatedSellingPrice',
  'estimatedCompletionCost',
  'estimatedSellingCost',
  'netRealizableValue',
  'unitNetRealizableValue',
  'openingProvision',
  'provisionAmount',
  'reversalAmount',
  'closingProvision',
  'impairmentLoss',
  'carryingAmount',
  'provisionStatus',
]

/**
 * 存货跌价准备实体字段 i18n：index / inventory-impairment-provision-form 统一入口
 */
export function useInventoryImpairmentProvisionI18n() {
  const ef = useEntityFieldI18n(INVENTORYIMPAIRMENTPROVISION_ENTITY_SLUG)

  function ph(field: InventoryImpairmentProvisionField): string {
    return ef.placeholder(field, INVENTORYIMPAIRMENTPROVISION_PLACEHOLDER[field])
  }

  function queryPh(field: InventoryImpairmentProvisionQueryField, kind: EntityFieldPlaceholderKind): string {
    return ef.queryPlaceholder(field, kind)
  }

  return {
    t: ef.t,
    label: ef.label,
    queryLabel: ef.queryLabel,
    queryPh,
    self: ef.self,
    ph,
  }
}
