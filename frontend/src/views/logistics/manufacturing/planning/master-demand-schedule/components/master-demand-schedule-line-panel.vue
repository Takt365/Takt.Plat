<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/planning/master-demand-schedule/components -->
<!-- 文件名称：master-demand-schedule-line-panel.vue -->
<!-- 功能描述：主需求计划 MDS 头表主表实体右侧明细 masterDemandScheduleLine 独立 CRUD（按主表选中 masterDemandScheduleId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="master-demand-schedule-line-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.masterdemandscheduleline._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:planning:master:demand:schedule:create"
      update-permission="logistics:manufacturing:planning:master:demand:schedule:update"
      delete-permission="logistics:manufacturing:planning:master:demand:schedule:delete"


      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="false"
      :show-export="false"
      :show-advanced-query="false"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"


      @column-setting="handleColumnSetting"
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @refresh="handleRefresh"
    />
    <div class="master-demand-schedule-line-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getMasterDemandScheduleLineId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="masterDemandScheduleLineId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="single"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      />
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="720px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <MasterDemandScheduleLineForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterMasterDemandScheduleId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="masterDemandScheduleLineId"
      action-column-key="action"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 主需求计划 MDS 头表子表 masterDemandScheduleLine 右栏面板
 * @module views/logistics/manufacturing/planning/master-demand-schedule/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import MasterDemandScheduleLineForm from './master-demand-schedule-line-form.vue'
import { useMasterDemandScheduleMasterContext } from '../composables/use-master-demand-schedule-master-context'
import {
  ,
} from '@/api/logistics/manufacturing/planning/master-demand-schedule-line'
import type { MasterDemandScheduleLine, MasterDemandScheduleLineQuery } from '@/types/logistics/manufacturing/planning/master-demand-schedule-line'

const { t } = useI18n()
const { selectedMasterRow } = useMasterDemandScheduleMasterContext()

/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.masterdemandscheduleline._self') }),
)

const loading = ref(false)
const dataSource = ref<MasterDemandScheduleLine[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<MasterDemandScheduleLine | null>(null)
const selectedRows = ref<MasterDemandScheduleLine[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MasterDemandScheduleLine>>({})
const formLoading = ref(false)
const formRef = ref()

const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

const entityIdName = 'masterDemandScheduleLineId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.masterDemandScheduleId)
const masterMasterDemandScheduleId = computed(() => selectedMasterRow.value?.masterDemandScheduleId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getMasterDemandScheduleLineId(record: MasterDemandScheduleLine | Record<string, unknown>): string {
  return String((record as MasterDemandScheduleLine)?.[entityIdName] ?? '')
}

function getMasterDemandScheduleLineField(record: MasterDemandScheduleLine | Record<string, unknown>, field: string): unknown {
  return (record as MasterDemandScheduleLine)?.[field as keyof MasterDemandScheduleLine]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'masterDemandScheduleLineId',
    key: 'masterDemandScheduleLineId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MasterDemandScheduleLine }) =>
      String(getMasterDemandScheduleLineField(record, 'masterDemandScheduleLineId') ?? ''),
  },

  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:planning:master:demand:schedule:update',
        onClick: (record: MasterDemandScheduleLine) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:planning:master:demand:schedule:delete',
        onClick: (record: MasterDemandScheduleLine) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MasterDemandScheduleLine[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MasterDemandScheduleLine, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getMasterDemandScheduleLineId(selectedRow.value) === getMasterDemandScheduleLineId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MasterDemandScheduleLine[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MasterDemandScheduleLine) {
  const key = getMasterDemandScheduleLineId(record)
  return {
    onClick: () => {
      selectedRowKeys.value = [key]
      selectedRows.value = [record]
      selectedRow.value = record
    },
    class: selectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MasterDemandScheduleLineQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MasterDemandScheduleLineQuery>): MasterDemandScheduleLineQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MasterDemandScheduleLineQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    masterDemandScheduleId: masterMasterDemandScheduleId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MasterDemandScheduleLineQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await (buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(masterMasterDemandScheduleId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.masterdemandscheduleline._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: MasterDemandScheduleLine) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.masterdemandscheduleline._self') })
  formLoading.value = true
  try {
    const detail = await (getMasterDemandScheduleLineId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: t('entity.masterdemandscheduleline._self'),
    }))
  }
}

async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.()
    const id = formData.value?.masterDemandScheduleLineId
    if (id) {
      await (id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.masterdemandscheduleline._self') }))
    } else {
      await (payload)
      message.success(t('common.feedback.created', { target: t('entity.masterdemandscheduleline._self') }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: MasterDemandScheduleLine) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.masterdemandscheduleline._self'),
      name: t('common.tip.this.target', { target: t('entity.masterdemandscheduleline._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await (getMasterDemandScheduleLineId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.masterdemandscheduleline._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.masterdemandscheduleline._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.masterdemandscheduleline._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getMasterDemandScheduleLineId(r)).filter(Boolean)
      await (ids)
      message.success(t('common.feedback.deleted', { target: t('entity.masterdemandscheduleline._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>
