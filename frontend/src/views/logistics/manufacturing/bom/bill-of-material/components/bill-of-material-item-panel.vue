<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/components -->
<!-- 文件名称：bill-of-material-item-panel.vue -->
<!-- 功能描述：BOM 明细底部独立 CRUD 面板（上主下从）；按主表选中行 billOfMaterialId 分页查询 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="bill-of-material-item-panel flex flex-col min-h-[240px] pt-3 border-t-8 border-border">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('logistics.manufacturing.bom.bill-of-material.page.detailPanelTitle') }}
    </div>
    <TaktToolsBar
      create-permission="logistics:manufacturing:bom:billofmaterialitem:create"
      update-permission="logistics:manufacturing:bom:billofmaterialitem:update"
      delete-permission="logistics:manufacturing:bom:billofmaterialitem:delete"
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
    <TaktSingleTable
      :columns="columns"
      entity-scope="company"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="(record: any) => getBillOfMaterialItemId(record)"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :pagination="false"
      @change="handleTableChange"
    />
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
      width="640px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <BillOfMaterialItemForm
        ref="formRef"
        :form-data="formData"
        :master-bill-of-material-id="masterBillOfMaterialId"
        :master-bom-code="masterBomCode"
        :loading="formLoading"
      />
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * BOM 明细底部面板 · 独立子表 CRUD（对齐 Vue.NetCore MES_Bom_Detail）
 * @module views/logistics/manufacturing/bom/bill-of-material/components
 */
import { ref, computed } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import BillOfMaterialItemForm from './bill-of-material-item-form.vue'
import { useBillOfMaterialMasterContext } from '../composables/use-bill-of-material-master-context'
import {
  getBillOfMaterialItemList,
  getBillOfMaterialItemById,
  createBillOfMaterialItem,
  updateBillOfMaterialItem,
  deleteBillOfMaterialItemById,
  deleteBillOfMaterialItemBatch,
} from '@/api/logistics/manufacturing/bom/bill-of-material-item'
import type {
  BillOfMaterialItem,
  BillOfMaterialItemQuery,
} from '@/types/logistics/manufacturing/bom/bill-of-material-item'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 主表选中行上下文 */
const { selectedMasterRow } = useBillOfMaterialMasterContext()

/** 列表 loading */
const loading = ref(false)
/** 明细分页数据 */
const dataSource = ref<BillOfMaterialItem[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选行 */
const selectedRow = ref<BillOfMaterialItem | null>(null)
/** 表格多选行 */
const selectedRows = ref<BillOfMaterialItem[]>([])
/** 表格多选 keys */
const selectedRowKeys = ref<(string | number)[]>([])
/** 明细弹窗可见 */
const formVisible = ref(false)
/** 明细弹窗标题 */
const formTitle = ref('')
/** 明细表单数据 */
const formData = ref<Partial<BillOfMaterialItem>>({})
/** 明细提交 loading */
const formLoading = ref(false)
/** 明细表单 ref */
const formRef = ref()

/** 是否已选中主表行 */
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.billOfMaterialId)
/** 主表 BOM 头 ID */
const masterBillOfMaterialId = computed(() => selectedMasterRow.value?.billOfMaterialId ?? '')
/** 主表 BOM 编码 */
const masterBomCode = computed(() => selectedMasterRow.value?.bomCode ?? '')
/** 工具栏编辑禁用 */
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
/** 工具栏删除禁用 */
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

/** 实体主键字段 */
const entityIdName = 'billOfMaterialItemId'

/**
 * 读取明细行主键
 * @param record 行数据
 * @returns {string} 主键 string
 */
function getBillOfMaterialItemId(record: BillOfMaterialItem | Record<string, unknown>): string {
  return String((record as BillOfMaterialItem)?.[entityIdName] ?? '')
}

/** 表格列 */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('entity.billOfMaterialItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 80,
    ellipsis: true,
  },
  {
    title: t('entity.billOfMaterialItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.billOfMaterialItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 140,
    ellipsis: true,
  },
  {
    title: t('entity.billOfMaterialItem.usagequantity'),
    dataIndex: 'usageQuantity',
    key: 'usageQuantity',
    width: 100,
    ellipsis: true,
  },
  {
    title: t('entity.billOfMaterialItem.materialunit'),
    dataIndex: 'materialUnit',
    key: 'materialUnit',
    width: 80,
    ellipsis: true,
  },
  {
    title: t('entity.billOfMaterialItem.scraprate'),
    dataIndex: 'scrapRate',
    key: 'scrapRate',
    width: 90,
    ellipsis: true,
  },
  {
    title: t('entity.billOfMaterialItem.actualusagequantity'),
    dataIndex: 'actualUsageQuantity',
    key: 'actualUsageQuantity',
    width: 110,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:billofmaterialitem:update',
        onClick: (record: BillOfMaterialItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:billofmaterialitem:delete',
        onClick: (record: BillOfMaterialItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: BillOfMaterialItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
}))

/**
 * 行点击切换选中
 * @param record 行数据
 */
function onClickRow(record: BillOfMaterialItem) {
  return {
    onClick: () => {
      const key = getBillOfMaterialItemId(record)
      selectedRowKeys.value = [key]
      selectedRows.value = [record]
      selectedRow.value = record
    },
  }
}

/**
 * 加载明细分页（须先选中主表）
 */
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
    const query: BillOfMaterialItemQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      billOfMaterialId: masterBillOfMaterialId.value,
    }
    const res = await getBillOfMaterialItemList(query)
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

/** 对外暴露：主表行变更后刷新明细 */
function reload() {
  currentPage.value = 1
  void loadData()
}

/** 打开新增明细 */
function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('logistics.manufacturing.bom.bill-of-material.page.selectMasterFirst'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.billOfMaterialItem._self') })
  formData.value = {}
  formVisible.value = true
}

/** 打开编辑明细 */
async function handleEdit(record: BillOfMaterialItem) {
  if (!hasMasterSelection.value) {
    message.warning(t('logistics.manufacturing.bom.bill-of-material.page.selectMasterFirst'))
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.billOfMaterialItem._self') })
  formLoading.value = true
  try {
    const detail = await getBillOfMaterialItemById(getBillOfMaterialItemId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  }
}

/** 提交明细表单 */
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
    const id = formData.value?.billOfMaterialItemId
    if (id) {
      await updateBillOfMaterialItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.billOfMaterialItem._self') }))
    } else {
      await createBillOfMaterialItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.billOfMaterialItem._self') }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭明细弹窗 */
function handleFormCancel() {
  formVisible.value = false
}

/** 删除单行明细 */
async function handleDeleteOne(record: BillOfMaterialItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.billOfMaterialItem._self'),
      name: t('common.tip.this.target', { target: t('entity.billOfMaterialItem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBillOfMaterialItemById(getBillOfMaterialItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.billOfMaterialItem._self') }))
      await loadData()
    },
  })
}

/** 批量删除明细 */
async function handleDelete() {
  if (!hasMasterSelection.value) {
    message.warning(t('logistics.manufacturing.bom.bill-of-material.page.selectMasterFirst'))
    return
  }
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.billOfMaterialItem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.billOfMaterialItem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getBillOfMaterialItemId(r)).filter(Boolean)
      await deleteBillOfMaterialItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.billOfMaterialItem._self') }))
      await loadData()
    },
  })
}

/** 刷新明细列表 */
function handleRefresh() {
  void loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}

/** 分页页码变更 */
function handlePaginationChange(page: number) {
  currentPage.value = page
  void loadData()
}

/** 分页条数变更 */
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  void loadData()
}

defineExpose({ reload, loadData })
</script>
