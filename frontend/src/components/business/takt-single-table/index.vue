<!-- ========================================
项目名称:Takt.Plat
命名空间:@/components/business/takt-single-table
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:单表格组件,支持虚拟滚动、列宽调整、排序、筛选；始终设置 scroll.y（布局高度，与数据有无无关）

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <div
    class="takt-single-table"
    :class="rootExtraClass"
  >
    <div
      ref="tableBodyRef"
      class="takt-single-table__body"
      :class="{ 'takt-single-table__body--fixed-y': hasFixedScrollY }"
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
        :row-class-name="(_record, index) => (index % 2 === 1 ? 'table-striped' : '')"
        :virtual="shouldUseVirtual"
        :size="size"
        :bordered="bordered"
        v-bind="{
          ...tablePassthroughAttrs,
          ...(effectiveRowSelection ? { 'row-selection': effectiveRowSelection } : {})
        }"
        :custom-row="customRow"
        :scroll="scrollConfig"
        :locale="tableLocale"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
        @expand="(expanded, record) => emit('expand', expanded, record)"
      >
        <template #bodyCell="slotData">
          <template v-if="String(slotData.column?.key) === 'approvalStatus' && props.entityScope === 'approval'">
            <TaktDictTag
              dict-type="sys_approval_status"
              :value="slotData.record?.approvalStatus as string | number | undefined"
            />
          </template>
          <slot
            v-else
            name="bodyCell"
            v-bind="slotData"
          />
        </template>
        <template
          v-for="(_, name) in passthroughSlotNames"
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
defineOptions({ inheritAttrs: false })

import type { TableColumnsType, TableProps } from 'ant-design-vue'
import type {
  ColumnType,
  FilterValue,
  SorterResult,
  TableCurrentDataSource,
  TablePaginationConfig
} from 'ant-design-vue/es/table/interface'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
  type TaktEntityScope,
  type TaktTableLayoutMode,
} from '@/utils/table-columns'
import {
  applyTableColumnPresentation,
  resolveTableScrollConfig,
  resolveVerticalScrollY,
  type TaktTableScrollLayout,
} from '@/utils/table-scroll'
import { useTaktTableViewportScrollY } from '@/composables/use-takt-table-viewport-scroll-y'
import { useTaktMasterDetailLrScrollY } from '@/composables/use-takt-master-detail-lr-scroll-y'
import { useI18n } from 'vue-i18n'
import { useAttrs, computed, ref, useSlots } from 'vue'

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
  /** 滚动配置（仅覆盖 x 或显式 y；未传 y 时 innerHeight - 固定 header/footer 与页壳 300px） */
  scroll?: { x?: number | string | true; y?: number | string } | undefined
  /**
   * 表格布局场景（决定视口预留与 scroll.y 计算策略）
   * page：整页列表；treeRight：左树右表；masterDetailLr/Tb*：主子表；editable：表单内嵌可编辑表
   */
  scrollLayout?: TaktTableScrollLayout
  /** 表格尺寸 */
  size?: TableProps['size']
  /** 是否显示边框 */
  bordered?: boolean
  /** 行选择配置 */
  rowSelection?: TableProps['rowSelection'] | undefined
  /** 自定义行属性（点击选中等，对应 a-table customRow） */
  customRow?: TableProps['customRow']
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
   * 实体基类作用域（与 DTO 继承 TenantDtoBase / CompanyDtoBase / ApprovalDtoBase 对齐，必填）
   * tenant → TaktTenantEntityBase；company → TaktCompanyEntityBase；approval → TaktApprovalEntityBase
   */
  entityScope: TaktEntityScope
  /** 是否合并审计字段列 */
  includeAuditFields?: boolean
  /** 可见列键（列设置抽屉）；空数组时仅展示业务列 columns */
  visibleColumnKeys?: string[]
  /** ID 列键（默认 id） */
  idColumnKey?: string | number
  /** 操作列键（默认 action） */
  actionColumnKey?: string | number
  /** 单表 8 个业务列 / 树表 4 个 / 主子表左 2 个 / 主子表右 4 个（默认 single） */
  tableMode?: TaktTableLayoutMode
  /** @deprecated 请使用 tableMode；保留兼容旧页，不再从 merge 列截取 */
  largeScreenColumnCount?: number
  /** @deprecated 请使用 tableMode */
  smallScreenColumnCount?: number
  /** @deprecated 请使用 tableMode */
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
  scrollLayout: 'page',
  size: 'middle',
  bordered: false,
  rowSelection: undefined,
  customRow: undefined,
  showRowSelection: true,
  defaultEllipsis: true,
  showPagination: false,
  current: 1,
  pageSize: 20,
  total: 0,
  includeAuditFields: true,
  visibleColumnKeys: () => [],
  idColumnKey: 'id',
  actionColumnKey: 'action',
  tableMode: 'single',
  smallScreenColumnCount: 5,
  largeScreenBreakpoint: 1200,
})

const emit = defineEmits<{
  'change': [pagination: TablePagination, filters: TableFilters, sorter: TableSorter | TableSorter[]]
  'resize-column': [width: number, column: ResizableColumn]
  'update:current': [page: number]
  'update:pageSize': [size: number]
  'pagination-change': [page: number, pageSize: number]
  'expand': [expanded: boolean, record: TableRecord]
}>()

const { t } = useI18n()
const attrs = useAttrs()
const slots = useSlots()

/** 透传给 a-table 的插槽名（bodyCell 由组件内统一处理 approvalStatus 字典展示） */
const passthroughSlotNames = computed(() =>
  Object.keys(slots).filter((name) => name !== 'bodyCell'),
)

/** 页面传入的 class 挂到根节点（inheritAttrs: false 时不会自动合并） */
const rootExtraClass = computed(() => attrs.class)

/** 透传给 a-table 的属性（排除 class/style/customRow，避免破坏 scroll 布局或重复绑定） */
const tablePassthroughAttrs = computed(() => {
  const { class: _class, style: _style, customRow: _customRow, ...rest } = attrs
  return rest
})

/** 空数据占位（高度由 scroll.y 固定，文案居中显示） */
const tableLocale = computed(() => ({
  emptyText: t('common.status.empty'),
}))

/** 超过此行数时自动启用虚拟滚动（07-overflow-vue）；仅影响 virtual，不影响 scroll.y */
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

/** 页面业务列（解包 Ref/ComputedRef） */
const userColumns = computed((): TableColumnsType => normalizeUserTableColumns(props.columns))

/** 合并实体基座字段后的完整列 */
const mergedColumns = computed((): TableColumnsType =>
  mergeDefaultColumns(userColumns.value, t, props.includeAuditFields, props.entityScope),
)

/** 列设置过滤前的展示列源 */
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

/** 最终传给 a-table 的列 */
const resolvedDisplayColumns = computed(() =>
  applyTableColumnPresentation(displayColumnSource.value, props.defaultEllipsis),
)

const handleResizeColumn = (w: number, col: ColumnType<unknown>) => {
  const mutableCol = col as ResizableColumn & { resizable?: boolean }
  if (mutableCol.resizable !== true) {
    return
  }
  mutableCol.width = w
  emit('resize-column', w, mutableCol)
}

/** 表格体 DOM（masterDetailLr 布局内由父级统一测量 scroll.y） */
const tableBodyRef = ref<HTMLElement | null>(null)

/** 左右主子表：使用 takt-master-detail-table-lr provide 的共享 scroll.y */
const masterDetailLrScrollYPx = useTaktMasterDetailLrScrollY()

/** 视口动态 scroll.y（px）；与 dataSource 行数无关，空表亦保持同一高度 */
const viewportScrollYPx = useTaktTableViewportScrollY(computed(() => props.scrollLayout))

const scrollConfig = computed(() =>
  resolveTableScrollConfig({
    columns: resolvedDisplayColumns.value,
    scroll: props.scroll,
    includeRowSelection: effectiveRowSelection.value != null,
    enableVerticalScroll: true,
    scrollLayout: props.scrollLayout,
    verticalScrollHeight:
      props.scroll?.y != null && props.scroll.y !== ''
        ? props.scroll.y
        : props.scrollLayout === 'masterDetailLr' && masterDetailLrScrollYPx != null
          ? masterDetailLrScrollYPx.value
          : resolveVerticalScrollY(undefined, viewportScrollYPx.value),
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
  min-width: 0;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  min-height: 0;
  flex: 1 1 auto;
}

.takt-single-table.h-full {
  height: 100%;
}

.takt-single-table__body {
  flex: 1 1 auto;
  min-height: 0;
  min-width: 0;
  overflow: hidden;
  height: 100%;
}

.takt-single-table__body :deep(.ant-table-wrapper) {
  width: 100%;
  max-width: 100%;
  min-width: 0;
}

.takt-single-table__body :deep(.ant-table-container) {
  min-width: 0;
  max-width: 100%;
}

.takt-single-table__body :deep(.ant-table-content) {
  overflow-x: auto;
}

/** scroll.y 兜底：空数据/少行时亦保持固定表体高度（不随数据撑开） */
.takt-single-table__body--fixed-y :deep(.ant-table-container) {
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.takt-single-table__body--fixed-y :deep(.ant-table-body) {
  min-height: var(--takt-table-scroll-y);
  max-height: var(--takt-table-scroll-y);
  overflow-y: scroll !important;
  scrollbar-gutter: stable;
  flex: 1 1 auto;
  min-width: 0;
}

.takt-single-table__body--fixed-y :deep(.ant-table-summary) {
  flex-shrink: 0;
}

.takt-single-table__body--fixed-y :deep(.ant-table-placeholder) {
  min-height: calc(var(--takt-table-scroll-y) - 8px);
}
</style>
