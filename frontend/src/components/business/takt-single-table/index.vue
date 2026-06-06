<!-- ========================================
项目名称:Takt.Plat
命名空间:@/components/business/takt-single-table
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:单表格组件,支持虚拟滚动、列宽调整、排序、筛选；按实体基类作用域合并租户/公司/审批默认列

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <div class="takt-single-table">
    <div class="takt-single-table__body">
      <a-table
        class="ant-table-striped"
        :columns="resolvedDisplayColumns"
        :data-source="dataSource"
        :loading="loading"
        :pagination="false"
        :row-key="rowKey"
        :row-class-name="(_record, index) => (index % 2 === 1 ? 'table-striped' : '')"
        :scroll="scrollConfig"
        :virtual="shouldUseVirtual"
        :size="size"
        :bordered="bordered"
        v-bind="{
          ...$attrs,
          ...(effectiveRowSelection ? { 'row-selection': effectiveRowSelection } : {})
        }"
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
import type { TableColumnsType, TableProps } from 'ant-design-vue'
import type {
  ColumnType,
  FilterValue,
  SorterResult,
  TableCurrentDataSource,
  TablePaginationConfig
} from 'ant-design-vue/es/table/interface'
import {
  filterTableColumnsByVisibleKeys,
  getTableColumnKey,
  mergeDefaultColumns,
  type TaktEntityScope,
} from '@/utils/table-columns'
import { useI18n } from 'vue-i18n'

type TableRecord = Record<string, unknown>
type TableSorter = {
  field?: string | number | readonly (string | number)[]
  order?: string
}
type TableFilters = Record<string, FilterValue | null>
type TablePagination = { current?: number; pageSize?: number; total?: number }
type ResizableColumn = { width?: string | number } & Record<string, unknown>

interface Props {
  /** 业务列配置（不含实体基座字段，由 entityScope 自动合并） */
  columns: TableColumnsType
  /** 数据源 */
  dataSource?: TableRecord[]
  /** 加载状态 */
  loading?: boolean
  /** 行键 */
  rowKey?: string | ((record: TableRecord) => string)
  /** 自定义行类名 */
  rowClassName?: string | ((record: TableRecord, index: number) => string) | undefined
  /** 是否启用斑马纹 */
  stripe?: boolean
  /** 是否启用虚拟滚动 */
  virtual?: boolean
  /** 滚动配置 */
  scroll?: { x?: number | string | true; y?: number | string } | undefined
  /** 表格尺寸 */
  size?: TableProps['size']
  /** 是否显示边框 */
  bordered?: boolean
  /** 行选择配置 */
  rowSelection?: TableProps['rowSelection'] | undefined
  /** 是否默认显示行选择列 */
  showRowSelection?: boolean
  /** 是否默认启用列文本省略 */
  defaultEllipsis?: boolean
  /** 是否显示底部分页 */
  showPagination?: boolean
  /** 当前页码 */
  current?: number
  /** 每页条数 */
  pageSize?: number
  /** 总条数 */
  total?: number
  /**
   * 实体基类作用域（与 DTO 继承 TenantDtoBase / CompanyDtoBase / ApprovalDtoBase 对齐）
   * tenant → TaktTenantEntityBase；company → TaktCompanyEntityBase；approval → TaktApprovalEntityBase
   */
  entityScope?: TaktEntityScope
  /** 是否合并审计字段列 */
  includeAuditFields?: boolean
  /** 可见列键（列设置抽屉）；空数组时仅展示业务列 columns */
  visibleColumnKeys?: string[]
  /** ID 列键（默认 id） */
  idColumnKey?: string | number
  /** 操作列键（默认 action） */
  actionColumnKey?: string | number
  /** 大屏默认显示的非固定列数（不含 id、action；未传 visibleColumnKeys 且未启用列设置时不生效） */
  largeScreenColumnCount?: number
  /** 小屏默认显示的非固定列数 */
  smallScreenColumnCount?: number
  /** 大屏断点（px） */
  largeScreenBreakpoint?: number
}

const props = withDefaults(defineProps<Props>(), {
  dataSource: () => [],
  loading: false,
  rowKey: 'id',
  rowClassName: undefined,
  stripe: true,
  virtual: false,
  scroll: undefined,
  size: 'middle',
  bordered: false,
  rowSelection: undefined,
  showRowSelection: true,
  defaultEllipsis: true,
  showPagination: false,
  current: 1,
  pageSize: 20,
  total: 0,
  entityScope: 'company',
  includeAuditFields: true,
  visibleColumnKeys: () => [],
  idColumnKey: 'id',
  actionColumnKey: 'action',
  smallScreenColumnCount: 5,
  largeScreenBreakpoint: 1200,
})

const emit = defineEmits<{
  'change': [pagination: TablePagination, filters: TableFilters, sorter: TableSorter | TableSorter[]]
  'resize-column': [width: number, column: ResizableColumn]
  'update:current': [page: number]
  'update:pageSize': [size: number]
  'pagination-change': [page: number, pageSize: number]
}>()

const { t } = useI18n()

/** 超过此行数时自动启用虚拟滚动（07-overflow-vue） */
const AUTO_VIRTUAL_ROW_THRESHOLD = 50

/** 是否启用虚拟滚动：显式 true 或数据量超阈值 */
const shouldUseVirtual = computed(() => {
  if (props.virtual === true) return true
  const len = props.dataSource?.length ?? 0
  if (props.virtual === false && len <= AUTO_VIRTUAL_ROW_THRESHOLD) return false
  return len > AUTO_VIRTUAL_ROW_THRESHOLD
})

const currentPage = computed({
  get: () => props.current ?? 1,
  set: (val) => emit('update:current', val),
})

const pageSize = computed({
  get: () => props.pageSize ?? 20,
  set: (val) => emit('update:pageSize', val),
})

/** 行选择:默认显示选择列 */
const effectiveRowSelection = computed(() => {
  if (!props.showRowSelection) return undefined
  return props.rowSelection !== undefined && props.rowSelection !== null ? props.rowSelection : {}
})

/** 合并实体基座字段后的完整列 */
const mergedColumns = computed((): TableColumnsType =>
  mergeDefaultColumns(props.columns, t, props.includeAuditFields, props.entityScope),
)

/** 响应式大屏检测 */
const isLargeScreen = ref(window.innerWidth >= props.largeScreenBreakpoint)

const handleWindowResize = () => {
  isLargeScreen.value = window.innerWidth >= props.largeScreenBreakpoint
}

onMounted(() => {
  window.addEventListener('resize', handleWindowResize)
})

onUnmounted(() => {
  window.removeEventListener('resize', handleWindowResize)
})

/**
 * 按大屏/小屏配置截取默认可见列（id + 前 N 个非固定列 + action）
 * @param sourceColumns 已合并基座的列
 */
const pickResponsiveDefaultColumns = (sourceColumns: TableColumnsType): TableColumnsType => {
  const count = props.largeScreenColumnCount
  if (count == null || count <= 0) {
    return props.columns
  }
  const idKey = String(props.idColumnKey)
  const actionKey = String(props.actionColumnKey)
  const limit = isLargeScreen.value ? count : (props.smallScreenColumnCount ?? count)
  const nonFixed = sourceColumns.filter((col) => {
    const key = getTableColumnKey(col as ColumnType<TableRecord>)
    return key != null && key !== idKey && key !== actionKey
  })
  const selected = nonFixed.slice(0, limit)
  const idCol = sourceColumns.find((col) => getTableColumnKey(col as ColumnType<TableRecord>) === idKey)
  const actionCol = sourceColumns.find((col) => getTableColumnKey(col as ColumnType<TableRecord>) === actionKey)
  const result: TableColumnsType = []
  if (idCol) result.push(idCol)
  result.push(...selected)
  if (actionCol) result.push(actionCol)
  return result
}

/** 列设置过滤前的展示列源 */
const displayColumnSource = computed((): TableColumnsType => {
  const keys = props.visibleColumnKeys ?? []
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(mergedColumns.value, keys, props.columns)
  }
  return pickResponsiveDefaultColumns(mergedColumns.value)
})

/**
 * 为列补充默认宽度与 ellipsis
 * @param cols 列配置
 */
const applyColumnPresentation = (cols: TableColumnsType): TableColumnsType => {
  const visibleCount = cols.length
  return cols.map((column) => {
    const processedColumn = { ...column } as ResizableColumn & Record<string, unknown>
    if (!processedColumn.width && visibleCount > 0) {
      const viewportWidth = window.innerWidth - 40
      processedColumn.width = Math.floor(viewportWidth / 9)
    }
    if (props.defaultEllipsis && !('ellipsis' in column)) {
      processedColumn.ellipsis = true
    }
    return processedColumn
  })
}

/** 最终传给 a-table 的列 */
const resolvedDisplayColumns = computed(() => applyColumnPresentation(displayColumnSource.value))

const handleResizeColumn = (w: number, col: ColumnType<unknown>) => {
  const mutableCol = col as ResizableColumn
  mutableCol.width = w
  emit('resize-column', w, mutableCol)
}

const scrollConfig = computed(() => {
  const config: { x?: number | string | true; y?: number | string } = {
    ...props.scroll,
  }
  if (!config.x) {
    const totalWidth = resolvedDisplayColumns.value.reduce((sum: number, col) => {
      const width = (col as ResizableColumn).width
      return sum + (typeof width === 'number' ? width : 0)
    }, 0)
    if (totalWidth > 0 && resolvedDisplayColumns.value.every((col) => (col as ResizableColumn).width)) {
      config.x = totalWidth
    } else {
      config.x = 'max-content'
    }
  }
  if (shouldUseVirtual.value) {
    if (!config.y) {
      config.y = 600
    }
  }
  if (props.showPagination && !config.y) {
    config.y = 'calc(100vh - 320px)'
  }
  return config
})

/**
 * 分页页码变更
 * @param page 页码
 * @param size 每页条数
 */
const handlePaginationChange = (page: number, size: number) => {
  emit('update:current', page)
  emit('update:pageSize', size)
  emit('pagination-change', page, size)
}

/**
 * 分页每页条数变更
 * @param _page 当前页
 * @param size 每页条数
 */
const handlePaginationSizeChange = (_page: number, size: number) => {
  emit('update:current', 1)
  emit('update:pageSize', size)
  emit('pagination-change', 1, size)
}

const handleTableChange = (
  pagination: TablePaginationConfig,
  filters: TableFilters,
  sorter: SorterResult<TableRecord> | SorterResult<TableRecord>[],
  _extra: TableCurrentDataSource<TableRecord>,
) => {
  emit('change', pagination as TablePagination, filters, sorter as TableSorter | TableSorter[])
}

defineExpose({
  mergedColumns,
})
</script>

<style scoped>
.takt-single-table {
  margin: 0 4px 4px 4px;
  width: 100%;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  min-height: 0;
}

.takt-single-table__body {
  flex: 1;
  min-height: 0;
  overflow: hidden;
}
</style>
