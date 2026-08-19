<!-- ========================================
项目名称:Takt.Plat
命名空间:@/components/business/takt-tree-right-table
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:右表区域,用于树表布局右侧的表格；支持底部分页（树数据需由页面拍平后传入）

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->

<template>
  <div class="takt-tree-right-table">
    <div
      class="takt-tree-right-table__body"
      :class="{ 'takt-tree-right-table__body--fixed-y': hasFixedScrollY }"
      :style="tableBodyStyle"
    >
      <a-table
        class="ant-table-striped"
        table-layout="fixed"
        :columns="resolvedDisplayColumns"
        :data-source="dataSource"
        :loading="loading"
        :pagination="false"
        :row-key="rowKey"
        :row-class-name="rowClassName"
        :virtual="shouldUseVirtual"
        :size="size"
        :bordered="bordered"
        v-bind="{ ...$attrs, ...(effectiveRowSelection ? { 'row-selection': effectiveRowSelection } : {}), 'expanded-row-keys': expandedRowKeys }"
        :scroll="scrollConfig"
        :locale="tableLocale"
        @update:expanded-row-keys="(keys) => emit('update:expandedRowKeys', keys)"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <template #bodyCell="slotData">
          <div
            v-if="isTableColumnEllipsisEnabled(slotData.column)"
            :class="TAKT_TABLE_CELL_ELLIPSIS_INNER_CLASS"
            :title="resolveTableCellEllipsisTitle(slotData.text)"
          >
            <slot
              name="bodyCell"
              v-bind="slotData"
            >
              <TaktTableBodyCellFallback :slot-data="slotData" />
            </slot>
          </div>
          <slot
            v-else
            name="bodyCell"
            v-bind="slotData"
          >
            <TaktTableBodyCellFallback :slot-data="slotData" />
          </slot>
        </template>
        <template
          v-for="name in passthroughSlotNames"
          #[name]="slotData"
        >
          <slot
            :name="name"
            v-bind="slotData"
          />
        </template>
        <template
          v-if="$slots.summary"
          #summary
        >
          <slot name="summary" />
        </template>
      </a-table>
    </div>
    <div
      v-if="showFooterRemark"
      class="takt-tree-right-table__footer-remark shrink-0 px-1 pt-2 text-sm leading-relaxed text-text-secondary"
    >
      <slot name="footerRemark">
        {{ footerRemark }}
      </slot>
    </div>
    <TaktPagination
      v-if="showPagination"
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
  </div>
</template>

<script setup lang="ts">
import type { TableColumnsType, TableProps, TablePaginationConfig } from 'ant-design-vue'
import type { ColumnType, SorterResult, FilterValue, TableCurrentDataSource } from 'ant-design-vue/es/table/interface'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
  type TaktEntityScope,
  type TaktTableLayoutMode,
} from '@/utils/table-columns'
import {
  isTableColumnEllipsisEnabled,
  resolveTableCellEllipsisTitle,
  resolveTableScrollConfig,
  resolveVerticalScrollY,
  shouldUseTableVirtualScroll,
  TAKT_TABLE_CELL_ELLIPSIS_INNER_CLASS,
  type TaktTableScrollLayout,
} from '@/utils/table-scroll'
import TaktTableBodyCellFallback from '@/components/business/takt-table-body-cell-fallback/index'
import { useTaktTableViewportScrollY } from '@/composables/use-takt-table-viewport-scroll-y'
import { useTableColumnResize } from '@/composables/use-table-column-resize'
import { useI18n } from 'vue-i18n'

type TableRecord = Record<string, unknown>
type TableSorter = { field?: string | number | readonly (string | number)[]; order?: string }
type TableFilters = Record<string, unknown>
type TablePagination = { current?: number; pageSize?: number; total?: number }
type ResizableColumn = { width?: number | string }

interface Props {
  /** 业务列配置（不含实体基座字段，由 entityScope 自动合并） */
  columns: TableColumnsType
  /** 实体基类作用域（tenant/company/approval，必填） */
  entityScope: TaktEntityScope
  /** 是否合并审计字段列 */
  includeAuditFields?: boolean
  /** 可见列键（列设置抽屉）；空数组时按 tableMode 默认显隐 */
  visibleColumnKeys?: string[]
  /** ID 列键（默认 id） */
  idColumnKey?: string | number
  /** 操作列键（默认 action） */
  actionColumnKey?: string | number
  /** 左树右表默认 4 个业务列（默认 tree） */
  tableMode?: TaktTableLayoutMode
  /** 数据源 */
  dataSource?: TableRecord[]
  /** 加载状态 */
  loading?: boolean
  /** 行键 */
  rowKey?: string | ((record: TableRecord) => string)
  /** 自定义行类名(斑马纹等) */
  rowClassName?: string | ((record: TableRecord, index: number) => string)
  /** 是否启用斑马纹 */
  stripe?: boolean
  /** 是否启用虚拟滚动 */
  virtual?: boolean
  /** 滚动配置（仅覆盖 x 或显式 y；未传 y 时由 scrollLayout 决定默认高度） */
  scroll?: { x?: number | string | true; y?: number | string }
  /** 表格布局场景（默认 treeRight，与左树右表页面对齐） */
  scrollLayout?: TaktTableScrollLayout
  /** 表格尺寸 */
  size?: TableProps['size']
  /** 是否显示边框 */
  bordered?: boolean
  /** 行选择配置 */
  rowSelection?: TableProps['rowSelection']
  /** 是否默认显示行选择列（默认 true） */
  showRowSelection?: boolean
  /** 展开的行 key 列表（树表展开用，v-model:expanded-row-keys） */
  expandedRowKeys?: (string | number)[]
  /** 是否默认省略 */
  defaultEllipsis?: boolean
  /** 是否显示底部分页（菜单等大数据量树表应开启） */
  showPagination?: boolean
  /** 当前页码（showPagination 时 v-model） */
  current?: number
  /** 每页条数（showPagination 时 v-model） */
  pageSize?: number
  /** 总条数（拍平后的行数） */
  total?: number
  /** 表尾备注说明（合计行 / 表体下方、分页上方） */
  footerRemark?: string
}

const props = withDefaults(defineProps<Props>(), {
  dataSource: () => [],
  loading: false,
  rowKey: 'id',
  stripe: true,
  virtual: true,
  scroll: undefined,
  scrollLayout: 'treeRight',
  size: 'middle',
  bordered: false,
  showRowSelection: true,
  expandedRowKeys: () => [],
  defaultEllipsis: true,
  showPagination: false,
  current: 1,
  pageSize: 20,
  total: 0,
  includeAuditFields: true,
  visibleColumnKeys: () => [],
  idColumnKey: 'id',
  actionColumnKey: 'action',
  tableMode: 'tree',
  footerRemark: '',
})

const { t } = useI18n()
const slots = useSlots()

/** 本组件承接、不向 a-table 透传的插槽 */
const OWNED_SLOT_NAMES = new Set(['bodyCell', 'summary', 'footerRemark'])

/** 透传给 a-table 的插槽名 */
const passthroughSlotNames = computed(() =>
  Object.keys(slots).filter((name) => !OWNED_SLOT_NAMES.has(name)),
)

/** 是否展示表尾备注 */
const showFooterRemark = computed(
  () => !!props.footerRemark?.trim() || !!slots.footerRemark,
)

/** 空数据占位（scroll.y 固定布局高度，与行数无关） */
const tableLocale = computed(() => ({
  emptyText: t('common.status.empty'),
}))

/** 是否启用虚拟滚动：显式 true，或拍平行数超过 5000 */
const shouldUseVirtual = computed(() =>
  shouldUseTableVirtualScroll(props.dataSource?.length ?? 0, props.virtual),
)

const emit = defineEmits<{
  'change': [pagination: TablePagination, filters: TableFilters, sorter: TableSorter]
  'resize-column': [width: number, column: ResizableColumn]
  'update:expandedRowKeys': [keys: (string | number)[]]
  'update:current': [page: number]
  'update:pageSize': [size: number]
}>()

const currentPage = computed({
  get: () => props.current ?? 1,
  set: (val) => emit('update:current', val)
})

const pageSize = computed({
  get: () => props.pageSize ?? 20,
  set: (val) => emit('update:pageSize', val)
})

/** 行选择：默认显示选择列（showRowSelection 为 true 且未传 rowSelection 时用空对象） */
const effectiveRowSelection = computed(() => {
  if (!props.showRowSelection) return undefined
  return props.rowSelection !== undefined && props.rowSelection !== null ? props.rowSelection : {}
})

const rowClassName = computed(() => {
  if (props.rowClassName) return props.rowClassName
  if (props.stripe) {
    return (_record: TableRecord, index: number) => (index % 2 === 1 ? 'table-striped' : '')
  }
  return ''
})

const userColumns = computed((): TableColumnsType => normalizeUserTableColumns(props.columns))

const mergedColumns = computed(() =>
  mergeDefaultColumns(
    userColumns.value,
    t,
    props.includeAuditFields,
    props.entityScope,
    props.idColumnKey,
  ),
)

const displayColumnSource = computed((): TableColumnsType => {
  const keys = props.visibleColumnKeys ?? []
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(mergedColumns.value, keys, mergedColumns.value)
  }
  return filterMergedColumnsByDefaultVisible(mergedColumns.value, userColumns.value, {
    idColumnKey: props.idColumnKey,
    actionColumnKey: props.actionColumnKey,
    tableMode: props.tableMode,
    entityScope: props.entityScope,
  })
})

/** 列宽拖拽：稳定列引用，拖动时就地改 width（不重建列数组） */
const {
  displayColumns: resolvedDisplayColumns,
  rebuildDisplayColumns,
  handleResizeColumn: applyResizeColumnWidth,
} = useTableColumnResize()

watch(
  [displayColumnSource, () => props.defaultEllipsis],
  ([source, defaultEllipsis]) => {
    rebuildDisplayColumns(source, defaultEllipsis)
  },
  { immediate: true },
)

/** 视口动态 scroll.y（px）；与 dataSource 行数无关 */
const viewportScrollYPx = useTaktTableViewportScrollY(computed(() => props.scrollLayout))

const scrollConfig = computed(() =>
  resolveTableScrollConfig({
    columns: resolvedDisplayColumns.value,
    scroll: props.scroll,
    includeRowSelection: effectiveRowSelection.value != null,
    enableVerticalScroll: true,
    scrollLayout: props.scrollLayout,
    verticalScrollHeight: resolveVerticalScrollY(props.scroll?.y, viewportScrollYPx.value),
  }),
)

/** 是否已配置固定纵向滚动高度 */
const hasFixedScrollY = computed(() => {
  const y = scrollConfig.value.y
  return y != null && y !== ''
})

/**
 * 表格体 CSS 变量（兜底固定高度，与 scroll.y 一致）
 * @returns 绑定到 __body 的 style
 */
const tableBodyStyle = computed(() => {
  const y = scrollConfig.value.y
  if (y == null || y === '') {
    return undefined
  }
  const px = typeof y === 'number' ? `${y}px` : String(y)
  return { '--takt-table-scroll-y': px } as Record<string, string>
})

const handleTableChange = (pagination: TablePaginationConfig, filters: Record<string, FilterValue | null>, sorter: SorterResult<any> | SorterResult<any>[], _extra: TableCurrentDataSource<any>) => {
  const finalSorter = Array.isArray(sorter) ? sorter[0] : sorter
  emit('change',
    { current: pagination.current, pageSize: pagination.pageSize, total: pagination.total } as TablePagination,
    filters as TableFilters,
    (finalSorter || {}) as TableSorter
  )
}

/**
 * a-table 列宽拖拽：就地改宽并通知父级
 * @param w 新宽度
 * @param col 当前列
 */
const handleResizeColumn = (w: number, col: ColumnType<any>) => {
  if (!applyResizeColumnWidth(w, col)) {
    return
  }
  emit('resize-column', w, col as any)
}

/**
 * 分页页码变更
 * @param page 页码
 */
const handlePaginationChange = (page: number) => {
  emit('update:current', page)
}

/**
 * 分页每页条数变更
 * @param _page 当前页
 * @param size 每页条数
 */
const handlePaginationSizeChange = (_page: number, size: number) => {
  emit('update:current', 1)
  emit('update:pageSize', size)
}
</script>

<style scoped>
.takt-tree-right-table {
  flex: 1;
  min-width: 0;
  margin: 0 4px 4px 4px;
  width: auto;
  max-width: none;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  min-height: 0;
}

.takt-tree-right-table__body {
  flex: 1;
  min-height: 0;
  min-width: 0;
  overflow: hidden;
  height: 100%;
}

.takt-tree-right-table__body :deep(.ant-table-wrapper) {
  width: 100%;
  min-width: 0;
}

.takt-tree-right-table__body :deep(.ant-table-container) {
  min-width: 0;
}

/** scroll.y 兜底：空数据/少行时亦保持固定表体高度（不随数据撑开） */
.takt-tree-right-table__body--fixed-y :deep(.ant-table-header) {
  overflow-y: scroll !important;
  scrollbar-gutter: stable;
  scrollbar-width: none;
}

.takt-tree-right-table__body--fixed-y :deep(.ant-table-header::-webkit-scrollbar) {
  width: 0;
  height: 0;
}

.takt-tree-right-table__body--fixed-y :deep(.ant-table-body) {
  min-height: var(--takt-table-scroll-y);
  max-height: var(--takt-table-scroll-y);
  overflow-x: auto !important;
  overflow-y: auto !important;
  scrollbar-gutter: stable;
}

.takt-tree-right-table__body--fixed-y :deep(.ant-table-placeholder) {
  min-height: calc(var(--takt-table-scroll-y) - 8px);
}
</style>
