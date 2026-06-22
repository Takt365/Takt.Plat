<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/attendance/overtime/components -->
<!-- 文件名称：overtime-item-panel.vue -->
<!-- 功能描述：加班申请右侧明细 overtimeItem 独立 CRUD（按主表选中 overtimeId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="overtime-item-panel flex flex-col min-h-0 h-full">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.overtimeitem._self') }}
    </div>
    <TaktToolsBar
      create-permission="humanresource:attendance:overtime:create"
      update-permission="humanresource:attendance:overtime:update"
      delete-permission="humanresource:attendance:overtime:delete"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="false"
      :show-export="false"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="false"
      :show-refresh="true"
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
    <div class="takt-master-detail-table-lr__table-body min-h-0 h-full flex-1">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getOvertimeItemId"
        :row-selection="rowSelection"
        :pagination="false"
        scroll-layout="masterDetailLr"
        table-mode="single"
        :show-row-selection="true"
        @change="handleTableChange"
      />
    </div>
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="720px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <OvertimeItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterOvertimeId"
        :loading="formLoading"
      />
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 加班申请子表 overtimeItem 右栏面板
 * @module views/human-resource/attendance/overtime/components
 */
import { ref, computed } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import OvertimeItemForm from './overtime-item-form.vue'
import { useOvertimeMasterContext } from '../composables/use-overtime-master-context'
import {
  getOvertimeItemList,
  getOvertimeItemById,
  createOvertimeItem,
  updateOvertimeItem,
  deleteOvertimeItemById,
  deleteOvertimeItemBatch,
} from '@/api/human-resource/attendance/overtime-item'
import type { OvertimeItem, OvertimeItemQuery } from '@/types/human-resource/attendance/overtime-item'

const { t } = useI18n()
const { selectedMasterRow } = useOvertimeMasterContext()

const loading = ref(false)
const dataSource = ref<OvertimeItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const selectedRow = ref<OvertimeItem | null>(null)
const selectedRows = ref<OvertimeItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<OvertimeItem>>({})
const formLoading = ref(false)
const formRef = ref()

const entityIdName = 'overtimeItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.overtimeId)
const masterOvertimeId = computed(() => selectedMasterRow.value?.overtimeId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getOvertimeItemId(record: OvertimeItem | Record<string, unknown>): string {
  return String((record as OvertimeItem)?.[entityIdName] ?? '')
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('entity.overtimeitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.overtimeitem.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.overtimeitem.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.overtimeitem.plannedhours'),
    dataIndex: 'plannedHours',
    key: 'plannedHours',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.overtimeitem.actualstarttime'),
    dataIndex: 'actualStartTime',
    key: 'actualStartTime',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.overtimeitem.actualendtime'),
    dataIndex: 'actualEndTime',
    key: 'actualEndTime',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.overtimeitem.actualhours'),
    dataIndex: 'actualHours',
    key: 'actualHours',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.overtimeitem.overtime'),
    dataIndex: 'overtime',
    key: 'overtime',
    width: 120,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:attendance:overtime:update',
        onClick: (record: OvertimeItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:attendance:overtime:delete',
        onClick: (record: OvertimeItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: OvertimeItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
}))

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
    const query: OvertimeItemQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      overtimeId: masterOvertimeId.value,
    }
    const res = await getOvertimeItemList(query)
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

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.overtimeitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: OvertimeItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.overtimeitem._self') })
  formLoading.value = true
  try {
    const detail = await getOvertimeItemById(getOvertimeItemId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
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
    const id = formData.value?.overtimeItemId
    if (id) {
      await updateOvertimeItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.overtimeitem._self') }))
    } else {
      await createOvertimeItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.overtimeitem._self') }))
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

async function handleDeleteOne(record: OvertimeItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.overtimeitem._self'),
      name: t('common.tip.this.target', { target: t('entity.overtimeitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteOvertimeItemById(getOvertimeItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.overtimeitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.overtimeitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.overtimeitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getOvertimeItemId(r)).filter(Boolean)
      await deleteOvertimeItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.overtimeitem._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleTableChange() {}

function handlePaginationChange(page: number) {
  currentPage.value = page
  void loadData()
}

function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

defineExpose({ reload, loadData })
</script>
