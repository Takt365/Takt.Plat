// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables
// 文件名称：use-ec-detail-i18n.ts
// 功能描述：EcDetail 字段清单、列宽/默认可见列、表格列构建 + useEcDetailI18n（与 TaktEcDetail 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { h } from 'vue'
import type { TableColumnsType } from 'ant-design-vue'
import type { EcDetailQuery } from '@/types/logistics/manufacturing/engineering-change/ec-detail'
import TaktDictTag from '@/components/common/takt-dict-tag/index.vue'
import { buildEntitySelfI18nKey } from '@/utils/takt-entity-i18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/use-entity-field-i18n'

/** 与 TaktEcDetailI18nSeedData 一致的实体 slug */
export const ECDETAIL_ENTITY_SLUG = 'ecdetail'

/** entity.ecdetail._self 静态属性（导入组件 entity-i18n-key 等） */
export const ECDETAIL_SELF_I18N_KEY = buildEntitySelfI18nKey(ECDETAIL_ENTITY_SLUG)

/** 设变明细业务列（与 TaktEcDetail 实体属性 camelCase 一致；不含主键） */
export const ECDETAIL_LIST_FIELDS = [
  'ecCode',
  'lineNumber',
  'ecBomLineCode',
  'ecModelCode',
  'ecFinishedGoods',
  'ecFinishedGoodsDescription',
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'discontinuedStatus',
  'ecOldMaterialCode',
  'ecOldMaterialDescription',
  'ecOldUsageQuantity',
  'ecOldItemPosition',
  'ecOldStock',
  'ecOldWarehouse',
  'ecOldPurchaseType',
  'ecOldRequiresInspection',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewUsageQuantity',
  'ecNewItemPosition',
  'ecNewStock',
  'ecNewWarehouse',
  'ecNewPurchaseType',
  'ecNewRequiresInspection',
  'ecBomDate',
  'ecIsCompatible',
  'ecSecondDistinction',
  'ecInstruction',
  'ecOldPartDisposition',
  'isObsolete',
  'remark',
] as const

/** ec-form 来源导入/编辑内嵌明细 Tab 展示列（含工厂；plantCode 标签走 common.page.entity.plantcode） */
export const ECDETAIL_FORM_SUBTABLE_FIELDS = [
  'plantCode',
  ...ECDETAIL_LIST_FIELDS,
] as const

/** ec-form 内嵌明细 Tab 可见列（须显式传入 TaktSingleTable，否则默认仅 8 列） */
export const ECDETAIL_FORM_SUBTABLE_VISIBLE_COLUMN_KEYS = [
  ...ECDETAIL_FORM_SUBTABLE_FIELDS,
] as const

/**
 * 执行部门左栏主表默认可见列（完整列仍为 ECDETAIL_FORM_SUBTABLE_FIELDS，经列设置打开）
 * 识别行 + 完成品/上阶 + 兼容/二级区分/指令/旧品处理 + 新旧料 + 新品仓库/采购/检验 + BOM 日期
 */
export const ECDETAIL_DEPT_MASTER_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'plantCode',
  'ecCode',
  'lineNumber',
  'ecModelCode',
  'ecFinishedGoods',
  'ecFinishedGoodsDescription',
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'discontinuedStatus',
  'ecOldMaterialCode',
  'ecOldMaterialDescription',
  'ecIsCompatible',
  'ecSecondDistinction',
  'ecInstruction',
  'ecOldPartDisposition',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewPurchaseType',
  'ecNewWarehouse',
  'ecNewRequiresInspection',
  'ecBomDate',
] as const

/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const ECDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS = [
  'ecCode',
  'lineNumber',
  'ecBomLineCode',
  'ecModelCode',
  'ecFinishedGoods',
  'ecFinishedGoodsDescription',
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'discontinuedStatus',
  'ecOldMaterialCode',
  'ecOldMaterialDescription',
  'ecOldUsageQuantity',
  'ecOldItemPosition',
  'ecOldStock',
  'ecOldWarehouse',
  'ecOldPurchaseType',
  'ecOldRequiresInspection',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewUsageQuantity',
  'ecNewItemPosition',
  'ecNewStock',
  'ecNewWarehouse',
  'ecNewPurchaseType',
  'ecNewRequiresInspection',
  'ecBomDate',
  'ecIsCompatible',
  'ecSecondDistinction',
  'ecInstruction',
  'ecOldPartDisposition',
  'isObsolete',
  'action',
] as const

/** 内嵌明细 Tab 字典列（字段 → DictTypeCode） */
export const ECDETAIL_DICT_TYPE_BY_FIELD: Partial<Record<EcDetailListField | 'plantCode', string>> = {
  discontinuedStatus: 'logistics_materials_material_discontinued_status',
  ecOldRequiresInspection: 'sys_yes_no',
  ecNewRequiresInspection: 'sys_yes_no',
  ecSecondDistinction: 'logistics_manufacturing_ec_source_distinction',
  ecInstruction: 'logistics_manufacturing_ec_source_instruction',
  ecOldPartDisposition: 'logistics_manufacturing_ec_old_part_disposition',
  isObsolete: 'sys_yes_no',
}

export type EcDetailListField = (typeof ECDETAIL_LIST_FIELDS)[number]
export type EcDetailFormSubtableField = (typeof ECDETAIL_FORM_SUBTABLE_FIELDS)[number]

/** 设变明细表格列宽（ec-form 内嵌 Tab / 执行部门左栏主表共用） */
export const ECDETAIL_COLUMN_WIDTH: Partial<Record<EcDetailFormSubtableField, number>> = {
  plantCode: 80,
  ecCode: 100,
  lineNumber: 80,
  ecBomLineCode: 90,
  ecModelCode: 120,
  ecFinishedGoods: 120,
  ecFinishedGoodsDescription: 160,
  ecParentMaterialCode: 120,
  ecParentMaterialDescription: 160,
  discontinuedStatus: 100,
  ecOldMaterialCode: 120,
  ecOldMaterialDescription: 160,
  ecOldUsageQuantity: 100,
  ecOldItemPosition: 100,
  ecOldStock: 100,
  ecOldWarehouse: 90,
  ecOldPurchaseType: 90,
  ecOldRequiresInspection: 100,
  ecNewMaterialCode: 120,
  ecNewMaterialDescription: 160,
  ecNewUsageQuantity: 100,
  ecNewItemPosition: 100,
  ecNewStock: 100,
  ecNewWarehouse: 90,
  ecNewPurchaseType: 90,
  ecNewRequiresInspection: 100,
  ecBomDate: 120,
  ecIsCompatible: 100,
  ecSecondDistinction: 100,
  ecInstruction: 100,
  ecOldPartDisposition: 100,
  isObsolete: 90,
  remark: 140,
}

/**
 * 空值显示为短横
 * @param value 单元格原值
 * @returns {string} 展示文本
 */
function formatEcDetailCellValue(value: unknown): string {
  if (value === undefined || value === null || value === '') {
    return '-'
  }
  return String(value)
}

/**
 * 构建设变明细表格列（完整 TaktEcDetail 业务字段；字典列用 TaktDictTag）
 * @param label 列标题（entity.ecdetail.* / plantCode）
 * @returns {TableColumnsType} 全量业务列
 */
export function buildEcDetailTableColumns(
  label: (field: EcDetailFormSubtableField) => string,
): TableColumnsType {
  return ECDETAIL_FORM_SUBTABLE_FIELDS.map((field) => {
    const dictType = ECDETAIL_DICT_TYPE_BY_FIELD[field]
    return {
      title: label(field),
      dataIndex: field,
      key: field,
      width: ECDETAIL_COLUMN_WIDTH[field] ?? 120,
      ellipsis: true,
      resizable: true,
      customRender: ({ record }: { record: Record<string, unknown> }) => {
        const value = record[field]
        if (dictType) {
          return h(TaktDictTag, { dictType, value: String(value ?? '') })
        }
        return formatEcDetailCellValue(value)
      },
    }
  })
}

/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ECDETAIL_PLACEHOLDER = {
  ecCode: 'required',
  lineNumber: 'required',
  ecModelCode: 'required',
  ecFinishedGoods: 'optional',
  ecFinishedGoodsDescription: 'optional',
  ecParentMaterialCode: 'optional',
  ecParentMaterialDescription: 'optional',
  discontinuedStatus: 'select',
  ecOldMaterialCode: 'optional',
  ecOldMaterialDescription: 'optional',
  ecOldUsageQuantity: 'optional',
  ecOldItemPosition: 'optional',
  ecOldStock: 'optional',
  ecOldWarehouse: 'select',
  ecOldPurchaseType: 'select',
  ecOldRequiresInspection: 'select',
  ecNewMaterialCode: 'optional',
  ecNewMaterialDescription: 'optional',
  ecNewUsageQuantity: 'optional',
  ecNewItemPosition: 'optional',
  ecNewStock: 'optional',
  ecNewWarehouse: 'select',
  ecNewPurchaseType: 'select',
  ecNewRequiresInspection: 'select',
  ecBomDate: 'select',
  ecIsCompatible: 'optional',
  ecSecondDistinction: 'select',
  ecInstruction: 'select',
  ecOldPartDisposition: 'select',
  isObsolete: 'select',
  remark: 'optional',
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致） */
export type EcDetailField = keyof typeof ECDETAIL_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ECDETAIL_QUERY_STRING_FIELDS = [
  'plantCode',
  'ecId',
  'ecCode',
  'ecBomLineCode',
  'ecModelCode',
  'ecFinishedGoods',
  'ecFinishedGoodsDescription',
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'discontinuedStatus',
  'ecOldMaterialCode',
  'ecOldMaterialDescription',
  'ecOldItemPosition',
  'ecOldWarehouse',
  'ecOldPurchaseType',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewItemPosition',
  'ecNewWarehouse',
  'ecNewPurchaseType',
  'ecBomDateStart',
  'ecBomDateEnd',
  'ecIsCompatible',
  'ecSecondDistinction',
  'ecInstruction',
  'ecOldPartDisposition',
  'createdAtStart',
  'createdAtEnd',
  'extField',
  'remark',
] as const satisfies readonly (keyof EcDetailQuery)[]

export type EcDetailQueryField =
  | (typeof ECDETAIL_QUERY_STRING_FIELDS)[number]
  | 'lineNumber'
  | 'ecOldUsageQuantity'
  | 'ecOldStock'
  | 'ecOldRequiresInspection'
  | 'ecNewUsageQuantity'
  | 'ecNewStock'
  | 'ecNewRequiresInspection'
  | 'isObsolete'

/** 高级查询抽屉全部字段（含数值） */
export const ECDETAIL_QUERY_FIELDS: readonly EcDetailQueryField[] = [
  ...ECDETAIL_QUERY_STRING_FIELDS,
  'lineNumber',
  'ecOldUsageQuantity',
  'ecOldStock',
  'ecOldRequiresInspection',
  'ecNewUsageQuantity',
  'ecNewStock',
  'ecNewRequiresInspection',
  'isObsolete',
]

/**
 * EcDetail 字段 i18n：ec-form / ec-detail-panel / ec-detail-form 统一入口
 */
export function useEcDetailI18n() {
  const ef = useEntityFieldI18n(ECDETAIL_ENTITY_SLUG)

  function ph(field: EcDetailField): string {
    return ef.placeholder(field, ECDETAIL_PLACEHOLDER[field])
  }

  function queryPh(field: EcDetailQueryField, kind: EntityFieldPlaceholderKind): string {
    return ef.queryPlaceholder(field, kind)
  }

  /** plantCode 走 common.page.entity.plantcode */
  function columnLabel(field: EcDetailFormSubtableField): string {
    return ef.label(field)
  }

  return {
    t: ef.t,
    label: ef.label,
    columnLabel,
    queryLabel: ef.queryLabel,
    queryPh,
    self: ef.self,
    ph,
  }
}
