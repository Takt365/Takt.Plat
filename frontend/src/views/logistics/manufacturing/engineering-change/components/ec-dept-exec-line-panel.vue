<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/components -->
<!-- 文件名称：ec-dept-exec-line-panel.vue -->
<!-- 功能描述：执行部门右栏：按部门实体字段出列；按选中明细加载 1:1 执行行；defineExpose reload -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="ec-dept-exec-line-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="t('common.page.form.placeholder.search')"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      :update-permission="updatePermission"
      :export-permission="exportPermission"
      :show-create="false"
      :show-update="true"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-refresh="true"
      :show-advanced-query="false"
      :show-column-setting="true"
      :show-fullscreen="true"
      :update-disabled="updateDisabled"
      :export-disabled="!hasMasterSelection"
      :update-loading="loading"
      :export-loading="loading"
      :refresh-loading="loading"
      @update="handleUpdate"
      @export="handleExport"
      @column-setting="columnSettingVisible = true"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="ec-dept-exec-line-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        entity-scope="company"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="idField"
        table-mode="masterDetailDetail"
        scroll-layout="masterDetailLr"
        :scroll="{ y: detailTableScrollY }"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getLineId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        @change="handleTableChange"
        @pagination-change="handlePaginationChange"
        @resize-column="handleResizeColumn"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'discontinuedStatus'">
            <TaktDictTag dict-type="logistics_materials_material_discontinued_status" :value="record.discontinuedStatus" />
          </template>
          <template v-else-if="isYesNoField(String(column.key))">
            <TaktDictTag dict-type="sys_yes_no" :value="record[String(column.key)]" />
          </template>
        </template>
      </TaktSingleTable>
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="t('common.dialog.title.edit', { entity: t(menuI18nKey) })"
      width="900px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
    >
      <component
        :is="formComponent"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="idField"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 执行部门右栏执行行面板
 */
import type { Component } from 'vue'
import type { TableColumnsType } from 'ant-design-vue'
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/use-takt-master-detail-lr-scroll-y'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import type { TaktPagedResult } from '@/types/common'
import { getEcDeptExecLineFields } from '@/constants/logistics/ec-dept-exec-line-fields'
import { useEcDeptViewI18n } from '../composables/use-ec-dept-view-i18n'
import { useEcDeptExecMasterContext } from '../composables/use-ec-dept-exec-master-context'

/** 子表用字典 sys_yes_no 渲染的字段 */
const YES_NO_FIELDS = new Set(['isImplemented', 'isSopUpdated'])

/** 执行行（各部门 DTO 公共字段） */
type EcDeptExecLine = Record<string, unknown>

const props = defineProps<{
  /** 更新权限（与控制器 [TaktPermission] 一致，末段 update） */
  updatePermission: string
  /** 导出权限（与控制器 [TaktPermission] 一致，末段 export） */
  exportPermission: string
  /** 菜单 i18n 键 */
  menuI18nKey: string
  /** 执行行主键字段 */
  idField: string
  /** 部门实体 slug（eckoubai / echinkan 等） */
  deptSlug: string
  /** 执行行分页列表 */
  getLineList: (query: any) => Promise<TaktPagedResult<any>>
  /** 更新执行行 */
  updateLine: (id: string, dto: any) => Promise<any>
  /** 导出执行行 */
  exportLines: (query?: any) => Promise<Blob>
  /** 编辑表单组件 */
  formComponent: Component
  /** 附加查询参数（如制二页签 pcbaTab） */
  extraQuery?: Record<string, unknown>
}>()

const { t } = useI18n()
const pi = useEcDeptViewI18n(props.deptSlug)
const { selectedMasterRow } = useEcDeptExecMasterContext()

/**
 * 当前选中明细主键（对应各部门执行表 EcnDetailId）
 * @returns {string} 明细 Id
 */
function selectedEcnDetailId(): string {
  return String(selectedMasterRow.value?.ecDetailId ?? '').trim()
}

/** 是否已选主表明细行 */
const hasMasterSelection = computed(() => !!selectedEcnDetailId())
/** 列表 loading */
const loading = ref(false)
/** 数据源 */
const dataSource = ref<EcDeptExecLine[]>([])
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数 */
const total = ref(0)
/** 关键词 */
const queryKeyword = ref('')
/** 选中 keys */
const selectedRowKeys = ref<(string | number)[]>([])
/** 选中行 */
const selectedRows = ref<EcDeptExecLine[]>([])
/** 表单可见 */
const formVisible = ref(false)
/** 表单 loading */
const formLoading = ref(false)
/** 编辑数据 */
const formData = ref<EcDeptExecLine | null>(null)
/** 表单 ref */
const formRef = ref<{ validate: () => Promise<void>; getValues: () => Record<string, unknown> } | null>(null)
/** 列设置 */
const columnSettingVisible = ref(false)
/** 子表滚动容器 */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/**
 * 子表列宽（按字段语义）
 * @param field DTO camelCase
 * @returns {number} 列宽
 */
function lineColumnWidth(field: string): number {
  if (field === 'execContent' || field === 'oldProductHandling' || field === 'supplier') {
    return 180
  }
  if (field === 'ecFinishedGoodsDescription' || field === 'ecParentMaterialDescription') {
    return 160
  }
  if (YES_NO_FIELDS.has(field) || field === 'productionTeam' || field === 'lineNumber' || field === 'deptCode') {
    return 100
  }
  if (field.endsWith('Date') || field.endsWith('Code') || field.endsWith('Batch') || field === 'ecFinishedGoods') {
    return 140
  }
  return 120
}

/**
 * 是否用 sys_yes_no 字典展示
 * @param field 列 key
 * @returns {boolean} 是否是/否字段
 */
function isYesNoField(field: string): boolean {
  return YES_NO_FIELDS.has(field)
}

/** 列定义（与 TaktEcKoubai 等部门执行实体属性声明顺序一致） */
const columns = computed<TableColumnsType>(() =>
  getEcDeptExecLineFields(props.deptSlug).map((field) => ({
    title: pi.label(field),
    dataIndex: field,
    key: field,
    width: lineColumnWidth(field),
    ellipsis: field === 'execContent' || field === 'oldProductHandling',
  })),
)
/** 可见列 */
const visibleColumnKeys = ref<string[]>([])
/** 行选择 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcDeptExecLine[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
  },
}))
/** 更新按钮禁用 */
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRowKeys.value.length !== 1)

/**
 * 行主键
 * @param record 执行行
 * @returns {string} 主键
 */
function getLineId(record: Record<string, unknown>) {
  return String(record[props.idField] ?? '')
}

/**
 * 重算子表 scroll.y
 */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap)
}

/**
 * 加载执行行
 */
async function loadData() {
  const ecnDetailId = selectedEcnDetailId()
  if (!ecnDetailId) {
    dataSource.value = []
    total.value = 0
    selectedRowKeys.value = []
    selectedRows.value = []
    return
  }
  loading.value = true
  try {
    const res = await props.getLineList({
      ...(props.extraQuery ?? {}),
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value || undefined,
      ecnDetailId,
    })
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch {
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 刷新（重置到第一页） */
function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  selectedRowKeys.value = []
  selectedRows.value = []
  void loadData()
}

/** 搜索 */
function handleSearch() {
  reload()
}

/** 重置 */
function handleQueryReset() {
  queryKeyword.value = ''
  reload()
}

/** 刷新当前页 */
function handleRefresh() {
  void loadData()
}

/** 分页 */
function handlePaginationChange() {
  void loadData()
}

/** 表格变化 */
function handleTableChange() {}

/** 列宽 */
function handleResizeColumn() {}

/**
 * 行点击
 * @param record 行
 * @returns 行事件
 */
function onClickRow(record: Record<string, unknown>) {
  return {
    onClick: () => {
      const id = getLineId(record)
      selectedRowKeys.value = [id]
      selectedRows.value = [record]
    },
  }
}

/** 编辑 */
function handleUpdate() {
  const row = selectedRows.value[0]
  if (!row) {
    return
  }
  formData.value = { ...row }
  formVisible.value = true
}

/** 提交 */
async function handleFormSubmit() {
  if (!formRef.value || !formData.value) {
    return
  }
  await formRef.value.validate()
  const dto = formRef.value.getValues()
  formLoading.value = true
  try {
    await props.updateLine(getLineId(formData.value), dto)
    message.success(t('common.feedback.updated', { target: t(props.menuI18nKey) }))
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

/** 导出 */
async function handleExport() {
  const ecnDetailId = selectedEcnDetailId()
  if (!ecnDetailId) {
    return
  }
  try {
    loading.value = true
    const blob = await props.exportLines({
      ...(props.extraQuery ?? {}),
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value || undefined,
      ecnDetailId,
    })
    const url = window.URL.createObjectURL(blob as Blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `${t(props.menuI18nKey)}.xlsx`
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
  } finally {
    loading.value = false
  }
}

/**
 * 列显隐
 * @param keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列重置 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = columns.value.map((c) => String(c.key))
}

watch(
  () => getEcDeptExecLineFields(props.deptSlug).join(','),
  (keyCsv) => {
    visibleColumnKeys.value = keyCsv.split(',').filter(Boolean)
  },
  { immediate: true },
)

watch(
  () => selectedMasterRow.value?.ecDetailId,
  () => {
    reload()
  },
)

onMounted(() => {
  nextTick(() => {
    recalcDetailTableScrollY()
    const wrap = detailTableWrapRef.value
    if (wrap && typeof ResizeObserver !== 'undefined') {
      detailTableScrollResizeObserver = new ResizeObserver(() => recalcDetailTableScrollY())
      detailTableScrollResizeObserver.observe(wrap)
    }
  })
})

onBeforeUnmount(() => {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
})

defineExpose({ reload, loadData })
</script>
