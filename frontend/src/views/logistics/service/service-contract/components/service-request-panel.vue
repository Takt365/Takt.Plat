<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-contract/components -->
<!-- 文件名称：service-request-panel.vue -->
<!-- 功能描述：服务合同实体右侧明细 serviceRequest 独立 CRUD（按主表选中 serviceContractId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="service-request-panel flex flex-col min-h-0 h-full">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.servicerequest._self') }}
    </div>
    <TaktToolsBar
      create-permission="logistics:service:contract:create"
      update-permission="logistics:service:contract:update"
      delete-permission="logistics:service:contract:delete"
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
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getServiceRequestId"
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
      <ServiceRequestForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterServiceContractId"
        :loading="formLoading"
      />
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 服务合同实体子表 serviceRequest 右栏面板
 * @module views/logistics/service/service-contract/components
 */
import { ref, computed } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import ServiceRequestForm from './service-request-form.vue'
import { useServiceContractMasterContext } from '../composables/use-service-contract-master-context'
import {
  getServiceRequestList,
  getServiceRequestById,
  createServiceRequest,
  updateServiceRequest,
  deleteServiceRequestById,
  deleteServiceRequestBatch,
} from '@/api/logistics/customer-service/service-request'
import type { ServiceRequest, ServiceRequestQuery } from '@/types/logistics/customer-service/service-request'

const { t } = useI18n()
const { selectedMasterRow } = useServiceContractMasterContext()

const loading = ref(false)
const dataSource = ref<ServiceRequest[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const selectedRow = ref<ServiceRequest | null>(null)
const selectedRows = ref<ServiceRequest[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ServiceRequest>>({})
const formLoading = ref(false)
const formRef = ref()

const entityIdName = 'serviceRequestId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.serviceContractId)
const masterServiceContractId = computed(() => selectedMasterRow.value?.serviceContractId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getServiceRequestId(record: ServiceRequest | Record<string, unknown>): string {
  return String((record as ServiceRequest)?.[entityIdName] ?? '')
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('entity.servicerequest.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.servicerequest.code'),
    dataIndex: 'serviceRequestCode',
    key: 'serviceRequestCode',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.servicerequest.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.servicerequest.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.servicerequest.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.servicerequest.servicecontractcode'),
    dataIndex: 'serviceContractCode',
    key: 'serviceContractCode',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.servicerequest.requestdate'),
    dataIndex: 'requestDate',
    key: 'requestDate',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.servicerequest.expectedservicedate'),
    dataIndex: 'expectedServiceDate',
    key: 'expectedServiceDate',
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
        permission: 'logistics:service:contract:update',
        onClick: (record: ServiceRequest) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:service:contract:delete',
        onClick: (record: ServiceRequest) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ServiceRequest[]) => {
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
    const query: ServiceRequestQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      serviceContractId: masterServiceContractId.value,
    }
    const res = await getServiceRequestList(query)
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.servicerequest._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ServiceRequest) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.servicerequest._self') })
  formLoading.value = true
  try {
    const detail = await getServiceRequestById(getServiceRequestId(record))
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
    const id = formData.value?.serviceRequestId
    if (id) {
      await updateServiceRequest(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.servicerequest._self') }))
    } else {
      await createServiceRequest(payload)
      message.success(t('common.feedback.created', { target: t('entity.servicerequest._self') }))
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

async function handleDeleteOne(record: ServiceRequest) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.servicerequest._self'),
      name: t('common.tip.this.target', { target: t('entity.servicerequest._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteServiceRequestById(getServiceRequestId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.servicerequest._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.servicerequest._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.servicerequest._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getServiceRequestId(r)).filter(Boolean)
      await deleteServiceRequestBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.servicerequest._self') }))
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
