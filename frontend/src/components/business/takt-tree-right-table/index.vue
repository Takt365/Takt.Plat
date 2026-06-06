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
    <div class="takt-tree-right-table__body">
      <a-table
        class="ant-table-striped"
        :columns="resolvedDisplayColumns"
        :data-source="dataSource"
        :loading="loading"
        :pagination="false"
        :row-key="rowKey"
        :row-class-name="rowClassName"
        :scroll="scrollConfig"
        :virtual="shouldUseVirtual"
        :size="size"
        :bordered="bordered"
        v-bind="{ ...$attrs, ...(effectiveRowSelection ? { 'row-selection': effectiveRowSelection } : {}), 'expanded-row-keys': expandedRowKeys }"
        @update:expanded-row-keys="(keys) => emit('update:expandedRowKeys', keys)"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <template
          v-for="(_, name) in $slots"
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
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  type TaktEntityScope,
} from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'

type TableRecord = Record<string, unknown>
type TableSorter = { field?: string | number | readonly (string | number)[]; order?: string }
type TableFilters = Record<string, unknown>
type TablePagination = { current?: number; pageSize?: number; total?: number }
type ResizableColumn = { width?: number | string }

interface Props {
  /** 业务列配置（不含实体基座字段，由 entityScope 自动合并） */
  columns: TableColumnsType
  /** 实体基类作用域（默认 company，对齐 CompanyDtoBase） */
  entityScope?: TaktEntityScope
  /** 是否合并审计字段列 */
  includeAuditFields?: boolean
  /** 可见列键（列设置抽屉）；空数组时仅展示业务列 */
  visibleColumnKeys?: string[]
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
  /** 滚动配置 */
  scroll?: { x?: number | string | true; y?: number | string }
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
}

const props = withDefaults(defineProps<Props>(), {
  dataSource: () => [],
  loading: false,
  rowKey: 'id',
  stripe: true,
  virtual: true,
  size: 'middle',
  bordered: false,
  showRowSelection: true,
  expandedRowKeys: () => [],
  defaultEllipsis: true,
  showPagination: false,
  current: 1,
  pageSize: 20,
  total: 0,
  entityScope: 'company',
  includeAuditFields: true,
  visibleColumnKeys: () => [],
})

const { t } = useI18n()

/** 超过此行数时自动启用虚拟滚动（07-overflow-vue） */
const AUTO_VIRTUAL_ROW_THRESHOLD = 50

/** 是否启用虚拟滚动 */
const shouldUseVirtual = computed(() => {
  if (props.virtual === true) return true
  const len = props.dataSource?.length ?? 0
  if (props.virtual === false && len <= AUTO_VIRTUAL_ROW_THRESHOLD) return false
  return len > AUTO_VIRTUAL_ROW_THRESHOLD
})

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

const mergedColumns = computed(() =>
  mergeDefaultColumns(props.columns, t, props.includeAuditFields, props.entityScope),
)

const displayColumnSource = computed((): TableColumnsType => {
  const keys = props.visibleColumnKeys ?? []
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(mergedColumns.value, keys, props.columns)
  }
  return props.columns
})

const resolvedDisplayColumns = computed<TableColumnsType>(() => {
  const cols = displayColumnSource.value
  const visibleCount = cols.length
  return cols.map((column) => {
    const processedColumn = { ...column } as ColumnType<TableRecord>
    if (!processedColumn.width && visibleCount > 0) {
      const viewportWidth = window.innerWidth - 40
      processedColumn.width = Math.floor(viewportWidth / 9)
    }
    if (props.defaultEllipsis && column.ellipsis === undefined) {
      processedColumn.ellipsis = true
    }
    return processedColumn
  })
})

const scrollConfig = computed(() => {
  const config: { x?: number | string | true; y?: number | string } = { ...props.scroll }
  if (!config.x) {
    const totalWidth = resolvedDisplayColumns.value.reduce((sum: number, col) => {
      const width = (col as ResizableColumn).width
      return sum + (typeof width === 'number' ? width : 0)
    }, 0)
    config.x = totalWidth > 0 && resolvedDisplayColumns.value.every((col) => !!(col as ResizableColumn).width) ? totalWidth : 'max-content'
  }
  if (shouldUseVirtual.value && !config.y) config.y = 600
  if (props.showPagination && !config.y) {
    config.y = 'calc(100vh - 320px)'
  }
  return config
})

const handleTableChange = (pagination: TablePaginationConfig, filters: Record<string, FilterValue | null>, sorter: SorterResult<any> | SorterResult<any>[], _extra: TableCurrentDataSource<any>) => {
  const finalSorter = Array.isArray(sorter) ? sorter[0] : sorter
  emit('change',
    { current: pagination.current, pageSize: pagination.pageSize, total: pagination.total } as TablePagination,
    filters as TableFilters,
    (finalSorter || {}) as TableSorter
  )
}

const handleResizeColumn = (w: number, col: ColumnType<any>) => {
  ;(col as any).width = w
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
  overflow: hidden;
}
</style>
