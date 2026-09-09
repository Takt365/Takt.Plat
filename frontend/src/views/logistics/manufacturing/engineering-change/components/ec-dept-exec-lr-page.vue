<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/components -->
<!-- 文件名称：ec-dept-exec-lr-page.vue -->
<!-- 功能描述：执行部门左右主子表壳：左栏部门执行主表，右栏设变明细（视图主从） -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getMasterId"
      :master-row-selection="rowSelection"
      :master-id-column-key="idField"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="queryKeyword"
          :placeholder="t('common.page.form.placeholder.search')"
          :loading="loading"
          @search="handleSearch"
          @reset="handleReset"
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
          :show-advanced-query="false"
          :show-column-setting="true"
          :show-fullscreen="true"
          :show-refresh="true"
          :update-disabled="updateDisabled"
          :update-loading="loading"
          :export-loading="loading"
          :refresh-loading="loading"
          @update="handleUpdate"
          @export="handleExport"
          @column-setting="columnSettingVisible = true"
          @refresh="handleRefresh"
        />
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'discontinuedStatus'">
          <TaktDictTag dict-type="logistics_materials_material_discontinued_status" :value="record.discontinuedStatus" />
        </template>
        <template v-else-if="column.key === 'ecNewPurchaseType'">
          <TaktDictTag dict-type="logistics_procurement_type" :value="record.ecNewPurchaseType" />
        </template>
        <template v-else-if="column.key === 'ecOldPartDisposition'">
          <TaktDictTag dict-type="logistics_manufacturing_ec_old_part_disposition" :value="record.ecOldPartDisposition" />
        </template>
        <template v-else-if="isYesNoField(String(column.key))">
          <TaktDictTag dict-type="sys_yes_no" :value="record[String(column.key)]" />
        </template>
      </template>
      <template #detail>
        <EcDeptExecDetailPanel
          ref="detailPanelRef"
          class="h-full min-h-0 flex-1"
          :id-field="idField"
          :get-master-by-id="getMasterById"
        />
      </template>
    </TaktMasterDetailTableLr>
    <TaktModal
      v-model:open="formVisible"
      :title="t('common.dialog.title.edit', { entity: t(menuI18nKey) })"
      :width="formModalWidthPx"
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
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 执行部门左右主子表页面壳（左栏部门执行主表，右栏设变明细）
 */
import type { Component } from 'vue'
import type { TableColumnsType } from 'ant-design-vue'
import { message, Modal } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useTaktContentModalWidth } from '@/composables/use-takt-content-modal-width'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import type { TaktPagedResult } from '@/types/common'
import type { TaktEcExecCode } from '@/constants/logistics/ec-exec-codes'
import { getEcDeptExecLineFields } from '@/constants/logistics/ec-dept-exec-line-fields'
import { useEcExecSignalRGroup } from '@/composables/use-ec-dept-signalr-group'
import { useEcDeptViewI18n } from '../composables/use-ec-dept-view-i18n'
import {
  provideEcDeptExecMasterContext,
  type EcDeptExecMasterRow,
} from '../composables/use-ec-dept-exec-master-context'
import EcDeptExecDetailPanel from './ec-dept-exec-detail-panel.vue'

/** 主表用字典 sys_yes_no 渲染的字段 */
const YES_NO_FIELDS = new Set(['isImplemented', 'isSopUpdated', 'ecNewRequiresInspection'])

/** 计划物料（在产） */
const PLANNED_MATERIAL_STATUS = 'Z0'
/** 停产按钮默认写入（生产结束） */
const EOL_MATERIAL_STATUS = 'ZQ'

const props = defineProps<{
  /** 更新权限（与控制器 [TaktPermission] 一致，末段 update） */
  updatePermission: string
  /** 导出权限（与控制器 [TaktPermission] 一致，末段 export） */
  exportPermission: string
  /** 菜单 i18n 键 */
  menuI18nKey: string
  /** 执行行主键字段（同明细上视图外键，如 ecBukanId） */
  idField: string
  /** 部门实体 slug（eckoubai / echinkan 等） */
  deptSlug: string
  /** SignalR 部门编码 */
  execCode: TaktEcExecCode
  /** 左栏部门执行主表列表（已含 VisibleExec 去重） */
  getMasterList: (query: any) => Promise<TaktPagedResult<any>>
  /** 按主键取执行行（含 EcDetails，供右栏） */
  getMasterById: (id: string) => Promise<any>
  /** 更新执行行 */
  updateMaster: (id: string, dto: any) => Promise<any>
  /**
   * 更新停产状态（Z0=在产 / ZQ=停产），并触发后端自动填充/清除
   * @param id 执行行主键
   * @param discontinuedStatus 完成品物料状态
   */
  updateDiscontinuedStatus: (id: string, discontinuedStatus: string) => Promise<any>
  /** 导出执行行 */
  exportMaster: (query?: any) => Promise<Blob>
  /** 编辑表单组件 */
  formComponent: Component
  /** 附加查询参数 */
  extraQuery?: Record<string, unknown>
}>()

const { t } = useI18n()
const pi = useEcDeptViewI18n(props.deptSlug)
const { selectedMasterRow } = provideEcDeptExecMasterContext()
const formModalWidthPx = useTaktContentModalWidth()

/** 查询关键词 */
const queryKeyword = ref('')
/** 主表 loading */
const loading = ref(false)
/** 主表数据 */
const dataSource = ref<EcDeptExecMasterRow[]>([])
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数 */
const total = ref(0)
/** 选中行 */
const selectedRows = ref<EcDeptExecMasterRow[]>([])
/** 选中 keys */
const selectedRowKeys = ref<(string | number)[]>([])
/** 主表选中 key */
const selectedMasterKey = ref('')
/** 列设置 */
const columnSettingVisible = ref(false)
/** 表单可见 */
const formVisible = ref(false)
/** 表单 loading */
const formLoading = ref(false)
/** 编辑数据 */
const formData = ref<EcDeptExecMasterRow | null>(null)
/** 表单 ref */
const formRef = ref<{ validate: () => Promise<void>; getValues: () => Record<string, unknown> } | null>(null)
/** 右栏明细面板 */
const detailPanelRef = ref<{ reload?: () => void } | null>(null)

/**
 * 是否用 sys_yes_no 字典展示
 * @param field 列 key
 * @returns {boolean} 是否是/否字段
 */
function isYesNoField(field: string): boolean {
  return YES_NO_FIELDS.has(field)
}

/**
 * 主表列宽
 * @param field DTO camelCase
 * @returns {number} 列宽
 */
function masterColumnWidth(field: string): number {
  if (field === 'execContent' || field === 'supplier') {
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
 * 是否视为停产（非空且非 Z0；无停产列时以执行内容 EOL 兜底）
 * @param record 主表行
 * @returns {boolean} 是否停产
 */
function isEolRow(record: EcDeptExecMasterRow): boolean {
  const status = String(record.discontinuedStatus ?? '').trim()
  if (status) {
    return status.toUpperCase() !== PLANNED_MATERIAL_STATUS
  }
  return String(record.execContent ?? '').trim().toUpperCase() === 'EOL'
}

/**
 * 更新停产状态并刷新
 * @param record 主表行
 * @param discontinuedStatus 目标状态
 * @param confirmKey 确认文案键
 */
function confirmDiscontinuedStatus(
  record: EcDeptExecMasterRow,
  discontinuedStatus: string,
  confirmKey: string,
) {
  const id = getMasterId(record)
  if (!id) {
    return
  }
  Modal.confirm({
    title: t(confirmKey),
    okText: t('common.page.button.ok'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      loading.value = true
      try {
        await props.updateDiscontinuedStatus(id, discontinuedStatus)
        message.success(t('common.feedback.updated', { target: t(props.menuI18nKey) }))
        await loadData()
        detailPanelRef.value?.reload?.()
      } finally {
        loading.value = false
      }
    },
  })
}

/** 主表列（部门执行实体字段 + 停产/在产操作） */
const columns = computed<TableColumnsType>(() => {
  const fieldCols = getEcDeptExecLineFields(props.deptSlug).map((field) => ({
    title: pi.label(field),
    dataIndex: field,
    key: field,
    width: masterColumnWidth(field),
    ellipsis: field === 'execContent',
  }))
  fieldCols.push(
    CreateActionColumn<EcDeptExecMasterRow>({
      width: 168,
      actions: [
        {
          key: 'discontinue',
          label: t('common.page.button.discontinue'),
          shape: 'plain',
          buttonClass: 'takt-button-disable',
          permission: props.updatePermission,
          visible: (record) => !isEolRow(record),
          onClick: (record) =>
            confirmDiscontinuedStatus(
              record,
              EOL_MATERIAL_STATUS,
              'common.page.button.discontinue',
            ),
        },
        {
          key: 'inproduction',
          label: t('common.page.button.inproduction'),
          shape: 'plain',
          buttonClass: 'takt-button-enable',
          permission: props.updatePermission,
          visible: (record) => isEolRow(record),
          onClick: (record) =>
            confirmDiscontinuedStatus(
              record,
              PLANNED_MATERIAL_STATUS,
              'common.page.button.inproduction',
            ),
        },
      ],
    }),
  )
  return fieldCols
})
/** 可见列 */
const visibleColumnKeys = ref<string[]>([])

/** 行选择 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcDeptExecMasterRow[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
  },
}))

/** 更新按钮禁用 */
const updateDisabled = computed(() => selectedRowKeys.value.length !== 1)

/**
 * 主表主键
 * @param record 主表行
 * @returns {string} 主键
 */
function getMasterId(record: EcDeptExecMasterRow | Record<string, unknown>) {
  return String(record[props.idField] ?? '')
}

/**
 * 同步主表选中到右栏
 * @param record 主表行
 */
function syncMasterSelection(record: EcDeptExecMasterRow | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getMasterId(record) : ''
}

/**
 * 主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as EcDeptExecMasterRow
  const key = getMasterId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  syncMasterSelection(row)
}

/** 主表分页 */
function handleMasterPaginationChange() {
  loadData()
}

/** 加载主表 */
async function loadData() {
  loading.value = true
  try {
    const res = await props.getMasterList({
      ...(props.extraQuery ?? {}),
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value || undefined,
    })
    dataSource.value = (res.data ?? []) as EcDeptExecMasterRow[]
    total.value = res.total ?? 0
    const currentKey = selectedMasterKey.value
    if (currentKey) {
      const found = dataSource.value.find((row) => getMasterId(row) === currentKey)
      syncMasterSelection(found ?? null)
      if (!found) {
        selectedRowKeys.value = []
        selectedRows.value = []
      }
    }
  } catch (error: unknown) {
    logger.error('[EcDeptExec] 加载主表失败', { error })
    message.error(t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
    syncMasterSelection(null)
    selectedRowKeys.value = []
    selectedRows.value = []
  } finally {
    loading.value = false
  }
}

/** 搜索 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置 */
function handleReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 刷新 */
function handleRefresh() {
  loadData()
}

/** 表格变化 */
function handleTableChange() {}

/** 列宽 */
function handleResizeColumn() {}

/**
 * 列显隐
 * @param keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列重置 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = [
    ...columns.value.map((c) => String(c.key)).filter((k) => k !== 'action'),
    'action',
  ]
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
    await props.updateMaster(getMasterId(formData.value), dto)
    message.success(t('common.feedback.updated', { target: t(props.menuI18nKey) }))
    formVisible.value = false
    await loadData()
    detailPanelRef.value?.reload?.()
  } finally {
    formLoading.value = false
  }
}

/** 导出 */
async function handleExport() {
  try {
    loading.value = true
    const blob = await props.exportMaster({
      ...(props.extraQuery ?? {}),
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value || undefined,
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

watch(
  () => getEcDeptExecLineFields(props.deptSlug).join(','),
  (keyCsv) => {
    visibleColumnKeys.value = [...keyCsv.split(',').filter(Boolean), 'action']
  },
  { immediate: true },
)

useTableRefresh(loadData)
useEcExecSignalRGroup(props.execCode)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})
</script>
