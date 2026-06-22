<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/components/business/takt-editable-table -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：主子表表单内嵌可编辑子表；增删行、行校验、汇总、空数据提示；defineExpose validate/getRows -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    class="takt-editable-table flex flex-col min-h-0"
    :class="sectionBorder ? 'pt-3 mt-3 border-t border-border' : undefined"
  >
    <div
      v-if="showToolbar"
      class="mb-2 flex items-center justify-between gap-2"
    >
      <span
        v-if="title"
        class="text-sm font-medium text-text"
      >{{ title }}</span>
      <slot name="toolbar" />
      <a-button
        v-if="showAdd"
        class="takt-button-create-row"
        :disabled="disabled || loading"
        @click="handleAddRow"
      >
        <template #icon>
          <RiInsertRowBottom class="takt-remix-icon" />
        </template>
        {{ resolvedAddButtonLabel }}
      </a-button>
    </div>
    <a-table
      :columns="tableColumns"
      :data-source="innerRows"
      :pagination="false"
      :scroll="scrollConfig"
      :row-key="rowKeyResolver"
      :size="size"
      :bordered="bordered"
      :loading="loading"
      :locale="tableLocale"
    >
      <template #emptyText>
        <div class="py-8 text-center text-sm text-text-secondary">
          <slot name="empty">
            {{ resolvedEmptyDescription }}
          </slot>
        </div>
      </template>
      <template
        v-if="hasSummaryRow"
        #summary
      >
        <slot
          name="summary"
          :summary-map="summaryValueMap"
          :rows="innerRows"
        >
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell
                v-for="(cell, cellIndex) in summaryCells"
                :key="cell.key"
                :index="cellIndex"
                :col-span="cell.colSpan"
              >
                <span class="text-sm font-medium">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </slot>
      </template>
      <template #bodyCell="{ column, record, index }">
        <template v-if="column.key === ACTION_COLUMN_KEY">
          <a-tooltip :title="t('common.page.button.deleterow')">
            <a-button
              class="takt-button-delete-row takt-button-plain-borderless takt-button-plain-icon-only"
              :disabled="disabled || loading"
              @click="handleRemoveRow(index)"
            >
              <template #icon>
                <RiDeleteRow class="takt-remix-icon" />
              </template>
            </a-button>
          </a-tooltip>
        </template>
        <template v-else-if="$slots[`cell-${String(column.key)}`]">
          <slot
            :name="`cell-${String(column.key)}`"
            :record="record"
            :index="index"
            :column="resolveColumn(String(column.key))"
            :error="getCellError(record, String(column.key))"
          />
        </template>
        <template v-else>
          <div class="flex min-w-0 flex-col gap-0.5">
            <template v-if="isReadonlyColumn(String(column.key))">
              <span class="text-sm">{{ record[resolveDataIndex(String(column.key))] }}</span>
            </template>
            <template v-else-if="resolveEditor(String(column.key)) === 'inputNumber'">
              <a-input-number
                v-model:value="record[resolveDataIndex(String(column.key))]"
                v-bind="resolveInputNumberProps(String(column.key))"
                :status="resolveCellStatus(record, String(column.key))"
                @change="handleCellChange(record, String(column.key))"
              />
            </template>
            <template v-else-if="resolveEditor(String(column.key)) === 'textarea'">
              <a-textarea
                v-model:value="record[resolveDataIndex(String(column.key))]"
                v-bind="resolveTextareaProps(String(column.key))"
                :status="resolveCellStatus(record, String(column.key))"
                @change="handleCellChange(record, String(column.key))"
              />
            </template>
            <template v-else-if="resolveEditor(String(column.key)) === 'datePicker'">
              <a-date-picker
                v-model:value="record[resolveDataIndex(String(column.key))]"
                v-bind="resolveDatePickerProps(String(column.key))"
                :status="resolveCellStatus(record, String(column.key))"
                @change="handleCellChange(record, String(column.key))"
              />
            </template>
            <template v-else>
              <a-input
                v-model:value="record[resolveDataIndex(String(column.key))]"
                v-bind="resolveInputProps(String(column.key))"
                :status="resolveCellStatus(record, String(column.key))"
                @change="handleCellChange(record, String(column.key))"
              />
            </template>
            <span
              v-if="getCellError(record, String(column.key))"
              class="text-xs text-red-500"
            >{{ getCellError(record, String(column.key)) }}</span>
          </div>
        </template>
      </template>
    </a-table>
  </div>
</template>

<script setup lang="ts">
/**
 * 主子表表单内嵌可编辑子表
 * @module components/business/takt-editable-table
 */
import { computed, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { RiDeleteRow, RiInsertRowBottom } from '@remixicon/vue'
import type { TableColumnsType } from 'ant-design-vue'
import {
  computeEditableSummaryMap,
  formatSummaryValue,
  mapEditableErrorsToCells,
  validateEditableRows,
  type TaktEditableValidateError,
} from './editable-table-utils'
import {
  resolveTableScrollConfig,
  resolveVerticalScrollY,
  type TaktTableScrollLayout,
} from '@/utils/table-scroll'
import { useTaktTableViewportScrollY } from '@/composables/use-takt-table-viewport-scroll-y'
import {
  TAKT_EDITABLE_ROW_KEY,
  attachEditableRowKeys,
  createEditableRowKey,
  detachEditableRowKeys,
  type TaktEditableEditorType,
  type TaktEditableRow,
  type TaktEditableTableColumn,
} from './types'

/** 操作列 key（内置） */
const ACTION_COLUMN_KEY = '__action'

/** i18n 翻译函数 */
const { t } = useI18n()

/** 组件 Props */
interface Props {
  /** 子表行（可含或不含 __rowKey） */
  modelValue?: TaktEditableRow[]
  /** 列配置（不含操作列，由 showDelete 自动追加） */
  columns: TaktEditableTableColumn[]
  /** 区块标题 */
  title?: string
  /** 新增按钮文案；省略时用 addButtonEntity 或 title */
  addButtonLabel?: string
  /** 新增按钮实体后缀（拼在 common.page.button.create 后） */
  addButtonEntity?: string
  /** 是否显示工具栏新增按钮 */
  showAdd?: boolean
  /** 是否显示行内删除列 */
  showDelete?: boolean
  /** 是否显示工具栏（标题 + 新增） */
  showToolbar?: boolean
  /** 滚动配置（仅覆盖 x 或显式 y；未传 y 时由 scrollLayout 决定默认高度） */
  scroll?: { x?: number | string | true; y?: number | string }
  /** 表格布局场景（默认 editable） */
  scrollLayout?: TaktTableScrollLayout
  /** @deprecated 请用 scroll.y；显式覆盖纵向滚动高度（px） */
  scrollY?: number
  /** 表格 bordered */
  bordered?: boolean
  /** 表格尺寸 */
  size?: 'small' | 'middle' | 'large'
  /** 表格 loading */
  loading?: boolean
  /** 禁用编辑与增删 */
  disabled?: boolean
  /** 持久化主键字段（用于 row-key 与 attach） */
  idField?: string
  /** 新增行默认字段工厂 */
  defaultRow?: () => Record<string, unknown>
  /** 顶部 border-t 分区（表单上主下从） */
  sectionBorder?: boolean
  /** 空数据提示文案；省略时用 common.status.empty */
  emptyDescription?: string
  /** 是否显示汇总行（省略时按列 summary 自动推断） */
  showSummary?: boolean
  /** 汇总行首列标签 */
  summaryLabel?: string
  /** validate 最少行数 */
  minRows?: number
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: () => [],
  title: '',
  addButtonLabel: '',
  addButtonEntity: '',
  showAdd: true,
  showDelete: true,
  showToolbar: true,
  scroll: undefined,
  scrollLayout: 'editable',
  bordered: true,
  size: 'middle',
  loading: false,
  disabled: false,
  idField: '',
  defaultRow: undefined,
  sectionBorder: false,
  emptyDescription: '',
  showSummary: undefined,
  summaryLabel: '',
  minRows: 0,
})

const emit = defineEmits<{
  'update:modelValue': [rows: TaktEditableRow[]]
  add: [row: TaktEditableRow]
  remove: [payload: { index: number; row: TaktEditableRow }]
}>()

/** 内部行（始终带 __rowKey） */
const innerRows = ref<TaktEditableRow[]>([])
/** 单元格校验错误 `${rowKey}:${field}` → message */
const cellErrors = ref<Record<string, string>>({})

/** 同步外部 modelValue → innerRows */
watch(
  () => props.modelValue,
  (val) => {
    innerRows.value = attachEditableRowKeys(val ?? [], props.idField || undefined)
    cellErrors.value = {}
  },
  { immediate: true, deep: true },
)

/** 空数据提示 */
const resolvedEmptyDescription = computed(() => {
  if (props.emptyDescription) {
    return props.emptyDescription
  }
  return t('common.status.empty')
})

/** a-table locale（空态由 #emptyText 渲染） */
const tableLocale = computed(() => ({
  emptyText: resolvedEmptyDescription.value,
}))

/** 新增按钮文案 */
const resolvedAddButtonLabel = computed(() => {
  if (props.addButtonLabel) {
    return props.addButtonLabel
  }
  const entity = props.addButtonEntity || props.title
  if (entity) {
    return `${t('common.page.button.createrow')}${entity}`
  }
  return t('common.page.button.createrow')
})

/** 是否展示汇总行 */
const hasSummaryRow = computed(() => {
  if (props.showSummary === false) {
    return false
  }
  if (props.showSummary === true) {
    return true
  }
  return props.columns.some((col) => !!col.summary)
})

/** 汇总值映射 */
const summaryValueMap = computed(() => computeEditableSummaryMap(innerRows.value, props.columns))

/** 汇总行首列文案 */
const resolvedSummaryLabel = computed(() => {
  if (props.summaryLabel) {
    return props.summaryLabel
  }
  return t('components.business.page.editabletable.summarylabel')
})

/** 汇总行单元格 */
const summaryCells = computed(() => {
  const cells: Array<{ key: string; text: string; colSpan?: number }> = [
    {
      key: '__summary_label',
      text: resolvedSummaryLabel.value,
    },
  ]
  for (const column of props.columns) {
    if (!column.summary) {
      cells.push({
        key: column.key,
        text: '',
      })
      continue
    }
    const raw = summaryValueMap.value[column.key]
    cells.push({
      key: column.key,
      text: formatSummaryValue(raw, column.summaryPrecision ?? 2),
    })
  }
  if (props.showDelete) {
    cells.push({
      key: ACTION_COLUMN_KEY,
      text: '',
    })
  }
  return cells
})

/** 合并视图传入的 scroll 与兼容 scrollY */
const mergedScroll = computed(() => {
  const config = props.scroll ? { ...props.scroll } : {}
  if (config.y == null && props.scrollY != null && props.scrollY > 0) {
    config.y = props.scrollY
  }
  return Object.keys(config).length > 0 ? config : undefined
})

/** 视口动态 scroll.y（px）；editable 场景默认固定 240，仍走统一计算入口 */
const viewportScrollYPx = useTaktTableViewportScrollY(() => props.scrollLayout)

/** 表格 scroll */
const scrollConfig = computed(() =>
  resolveTableScrollConfig({
    columns: tableColumns.value,
    scroll: mergedScroll.value,
    enableVerticalScroll: true,
    scrollLayout: props.scrollLayout,
    verticalScrollHeight: resolveVerticalScrollY(mergedScroll.value?.y, viewportScrollYPx.value),
  }),
)

/** a-table columns */
const tableColumns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = props.columns.map((col) => ({
    title: col.title,
    dataIndex: col.dataIndex ?? col.key,
    key: col.key,
    width: col.width,
    fixed: col.fixed,
    ellipsis: true,
  }))
  if (props.showDelete) {
    cols.push({
      title: t('common.table.actions'),
      key: ACTION_COLUMN_KEY,
      width: 56,
      align: 'center',
      fixed: 'right',
    })
  }
  return cols
})

/**
 * row-key 解析
 * @param row 行数据
 * @returns {string} row-key
 */
function rowKeyResolver(row: TaktEditableRow): string {
  return String(row[TAKT_EDITABLE_ROW_KEY] ?? '')
}

/**
 * 单元格错误 key
 * @param row 行
 * @param columnKey 列 key
 * @returns {string} `${rowKey}:${field}`
 */
function buildCellErrorKey(row: TaktEditableRow, columnKey: string): string {
  const field = resolveDataIndex(columnKey)
  return `${rowKeyResolver(row)}:${field}`
}

/**
 * 读取单元格错误
 * @param row 行
 * @param columnKey 列 key
 * @returns {string | undefined} 错误文案
 */
function getCellError(row: TaktEditableRow, columnKey: string): string | undefined {
  return cellErrors.value[buildCellErrorKey(row, columnKey)]
}

/**
 * 解析单元格校验态
 * @param row 行
 * @param columnKey 列 key
 * @returns {'error' | undefined} a-input status
 */
function resolveCellStatus(row: TaktEditableRow, columnKey: string): 'error' | undefined {
  return getCellError(row, columnKey) ? 'error' : undefined
}

/**
 * 单元格变更时清除对应错误
 * @param row 行
 * @param columnKey 列 key
 */
function handleCellChange(row: TaktEditableRow, columnKey: string) {
  const key = buildCellErrorKey(row, columnKey)
  if (!cellErrors.value[key]) {
    return
  }
  const next = { ...cellErrors.value }
  delete next[key]
  cellErrors.value = next
}

/**
 * 按 key 查找列配置
 * @param key 列 key
 * @returns {TaktEditableTableColumn | undefined} 列配置
 */
function resolveColumn(key: string): TaktEditableTableColumn | undefined {
  return props.columns.find((col) => col.key === key)
}

/**
 * 解析绑定字段名
 * @param key 列 key
 * @returns {string} dataIndex
 */
function resolveDataIndex(key: string): string {
  return resolveColumn(key)?.dataIndex ?? key
}

/**
 * 是否只读列
 * @param key 列 key
 * @returns {boolean} 只读
 */
function isReadonlyColumn(key: string): boolean {
  const col = resolveColumn(key)
  if (!col) {
    return true
  }
  return col.readonly === true || col.editor === 'readonly'
}

/**
 * 解析列编辑器类型
 * @param key 列 key
 * @returns {TaktEditableEditorType} 编辑器
 */
function resolveEditor(key: string): TaktEditableEditorType {
  const col = resolveColumn(key)
  if (!col || col.readonly) {
    return 'readonly'
  }
  return col.editor ?? 'input'
}

/**
 * 解析列占位符
 * @param col 列配置
 * @returns {string} placeholder
 */
function resolvePlaceholder(col: TaktEditableTableColumn): string {
  if (col.placeholder) {
    return col.placeholder
  }
  const field = col.title
  if (col.editor === 'datePicker') {
    return t('common.page.form.placeholder.select', { field })
  }
  if (col.editor === 'textarea') {
    return t('common.page.form.placeholder.optional', { field })
  }
  return t('common.page.form.placeholder.required', { field })
}

/**
 * 行号 + 字段标签（校验文案）
 * @param rowIndex 行索引
 * @param column 列配置
 * @returns {string} 标签
 */
function buildRowFieldLabel(rowIndex: number, column: TaktEditableTableColumn): string {
  return t('components.business.page.editabletable.rowfield', {
    row: rowIndex + 1,
    field: column.title,
  })
}

/**
 * 解析 a-input props
 * @param key 列 key
 * @returns {Record<string, unknown>} props
 */
function resolveInputProps(key: string): Record<string, unknown> {
  const col = resolveColumn(key)
  return {
    placeholder: col ? resolvePlaceholder(col) : '',
    size: props.size,
    disabled: props.disabled || props.loading,
    allowClear: col?.allowClear !== false,
  }
}

/**
 * 解析 a-input-number props
 * @param key 列 key
 * @returns {Record<string, unknown>} props
 */
function resolveInputNumberProps(key: string): Record<string, unknown> {
  const col = resolveColumn(key)
  return {
    min: col?.min ?? 0,
    placeholder: col ? resolvePlaceholder(col) : '',
    size: props.size,
    disabled: props.disabled || props.loading,
    style: { width: '100%' },
  }
}

/**
 * 解析 a-textarea props
 * @param key 列 key
 * @returns {Record<string, unknown>} props
 */
function resolveTextareaProps(key: string): Record<string, unknown> {
  const col = resolveColumn(key)
  return {
    placeholder: col ? resolvePlaceholder(col) : '',
    rows: col?.rows ?? 1,
    size: props.size,
    disabled: props.disabled || props.loading,
  }
}

/**
 * 解析 a-date-picker props
 * @param key 列 key
 * @returns {Record<string, unknown>} props
 */
function resolveDatePickerProps(key: string): Record<string, unknown> {
  const col = resolveColumn(key)
  return {
    placeholder: col ? resolvePlaceholder(col) : '',
    valueFormat: col?.valueFormat ?? 'YYYY-MM-DD HH:mm:ss',
    showTime: col?.showTime ?? true,
    size: props.size,
    disabled: props.disabled || props.loading,
    style: { width: '100%' },
  }
}

/**
 * 向父级 emit 行变更
 * @param rows 内部行
 */
function emitRows(rows: TaktEditableRow[]) {
  innerRows.value = rows
  emit('update:modelValue', rows)
}

/** 新增一行 */
function handleAddRow() {
  const base = props.defaultRow?.() ?? {}
  const row: TaktEditableRow = {
    ...base,
    [TAKT_EDITABLE_ROW_KEY]: createEditableRowKey(),
  }
  const next = [...innerRows.value, row]
  emitRows(next)
  emit('add', row)
}

/**
 * 删除指定行
 * @param index 行索引
 */
function handleRemoveRow(index: number) {
  const row = innerRows.value[index]
  if (!row) {
    return
  }
  const next = innerRows.value.filter((_, i) => i !== index)
  emitRows(next)
  emit('remove', { index, row })
}

/**
 * 剥离 __rowKey 后返回提交用行
 * @returns {Record<string, unknown>[]} DTO 行数组
 */
function getRows(): Record<string, unknown>[] {
  return detachEditableRowKeys(innerRows.value)
}

/**
 * 返回各列汇总值
 * @returns {Record<string, unknown>} 汇总映射
 */
function getSummaryValues(): Record<string, unknown> {
  return { ...summaryValueMap.value }
}

/** 清空子表 */
function resetRows() {
  cellErrors.value = {}
  emitRows([])
}

/** 清除校验态 */
function clearValidate() {
  cellErrors.value = {}
}

/**
 * 校验子表行（失败 throw Error，供父级 validate 捕获）
 * @returns {Promise<TaktEditableValidateError[]>} 错误列表（成功为空数组）
 */
async function validate(): Promise<TaktEditableValidateError[]> {
  const entity = props.addButtonEntity || props.title
  const minRowsMessage =
    props.minRows > 0
      ? t('components.business.page.editabletable.minrows', {
          min: props.minRows,
          entity: entity || t('components.business.page.editabletable.row'),
        })
      : ''
  const rawErrors = await validateEditableRows(innerRows.value, props.columns, {
    minRows: props.minRows,
    minRowsMessage,
    rowFieldLabel: (rowIndex, column) =>
      t('common.validation.required', { field: buildRowFieldLabel(rowIndex, column) }),
    uniqueMessage: (rowIndex, column) =>
      t('components.business.page.editabletable.uniquefield', {
        row: rowIndex + 1,
        field: column.title,
      }),
  })
  const normalizedErrors = rawErrors.map((item) => {
    if (item.rowIndex >= 0) {
      return item
    }
    return {
      ...item,
      message: item.message || minRowsMessage,
    }
  })
  cellErrors.value = mapEditableErrorsToCells(normalizedErrors)
  if (normalizedErrors.length) {
    const first = normalizedErrors[0]
    throw new Error(first?.message || t('common.validation.invalid', { field: props.title || '' }))
  }
  return []
}

defineExpose({
  addRow: handleAddRow,
  removeRow: handleRemoveRow,
  getRows,
  getSummaryValues,
  resetRows,
  validate,
  clearValidate,
})
</script>
