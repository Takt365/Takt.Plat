<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/assy-defect/components -->
<!-- 文件名称：assy-defect-detail-panel.vue -->
<!-- 功能描述：组立不良明细右侧分栏独立 CRUD；按主表选中 assyDefectId 分页查询 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="assy-defect-detail-panel flex flex-col min-h-0 h-full">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('logistics.manufacturing.defect.assy-defect.page.detailpanetitle') }}
    </div>
    <TaktToolsBar
      create-permission="logistics:manufacturing:defect:assydefectdetail:create"
      update-permission="logistics:manufacturing:defect:assydefectdetail:update"
      delete-permission="logistics:manufacturing:defect:assydefectdetail:delete"
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
      :row-key="getAssyDefectDetailId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :pagination="false"
      table-mode="single"
      :show-row-selection="true"
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
      width="720px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <AssyDefectDetailForm
        ref="formRef"
        :form-data="formData"
        :master-assy-defect-id="masterAssyDefectId"
        :master-prod-order-code="masterProdOrderCode"
        :loading="formLoading"
      />
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 组立不良明细右栏面板 · 独立子表 CRUD
 * @module views/logistics/manufacturing/defect/assy-defect/components
 */
import { ref, computed } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import AssyDefectDetailForm from './assy-defect-detail-form.vue'
import { useAssyDefectMasterContext } from '../composables/use-assy-defect-master-context'
import {
  getAssyDefectDetailList,
  getAssyDefectDetailById,
  createAssyDefectDetail,
  updateAssyDefectDetail,
  deleteAssyDefectDetailById,
  deleteAssyDefectDetailBatch,
} from '@/api/logistics/manufacturing/defect/assy-defect-detail'
import type {
  AssyDefectDetail,
  AssyDefectDetailQuery,
} from '@/types/logistics/manufacturing/defect/assy-defect-detail'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 主表选中行上下文 */
const { selectedMasterRow } = useAssyDefectMasterContext()

/** 列表 loading */
const loading = ref(false)
/** 明细分页数据 */
const dataSource = ref<AssyDefectDetail[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选行 */
const selectedRow = ref<AssyDefectDetail | null>(null)
/** 表格多选行 */
const selectedRows = ref<AssyDefectDetail[]>([])
/** 表格多选 keys */
const selectedRowKeys = ref<(string | number)[]>([])
/** 明细弹窗可见 */
const formVisible = ref(false)
/** 明细弹窗标题 */
const formTitle = ref('')
/** 明细表单数据 */
const formData = ref<Partial<AssyDefectDetail>>({})
/** 明细提交 loading */
const formLoading = ref(false)
/** 明细表单 ref */
const formRef = ref()

/** 是否已选中主表行 */
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.assyDefectId)
/** 主表 ID */
const masterAssyDefectId = computed(() => selectedMasterRow.value?.assyDefectId ?? '')
/** 主表生产订单号 */
const masterProdOrderCode = computed(() => selectedMasterRow.value?.prodOrderCode ?? '')
/** 工具栏编辑禁用 */
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
/** 工具栏删除禁用 */
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

/** 实体主键字段 */
const entityIdName = 'assyDefectDetailId'

/**
 * 读取明细行主键（与 TaktSingleTable rowKey 签名对齐）
 * @param record 行数据
 * @returns {string} 主键 string
 */
function getAssyDefectDetailId(record: any): string {
  return String(record?.[entityIdName] ?? '')
}

/** 表格列 */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('entity.assydefectdetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 72,
    ellipsis: true,
  },
  {
    title: t('entity.assydefectdetail.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.assydefectdetail.defectcategory'),
    dataIndex: 'defectCategory',
    key: 'defectCategory',
    width: 100,
    ellipsis: true,
  },
  {
    title: t('entity.assydefectdetail.defectqty'),
    dataIndex: 'defectQty',
    key: 'defectQty',
    width: 88,
    ellipsis: true,
  },
  {
    title: t('entity.assydefectdetail.cumulativedefectqty'),
    dataIndex: 'cumulativeDefectQty',
    key: 'cumulativeDefectQty',
    width: 88,
    ellipsis: true,
  },
  {
    title: t('entity.assydefectdetail.randomcardno'),
    dataIndex: 'randomCardNo',
    key: 'randomCardNo',
    width: 100,
    ellipsis: true,
  },
  {
    title: t('entity.assydefectdetail.occurrenceengineering'),
    dataIndex: 'occurrenceEngineering',
    key: 'occurrenceEngineering',
    width: 120,
    ellipsis: true,
  },
  {
    title: t('entity.assydefectdetail.teststep'),
    dataIndex: 'testStep',
    key: 'testStep',
    width: 100,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:defect:assydefectdetail:update',
        onClick: (record: AssyDefectDetail) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:defect:assydefectdetail:delete',
        onClick: (record: AssyDefectDetail) => void handleDeleteOne(record),
      },
    ],
  }),
])

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AssyDefectDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
}))

/**
 * 行点击切换选中
 * @param record 行数据
 */
function onClickRow(record: AssyDefectDetail) {
  return {
    onClick: () => {
      const key = getAssyDefectDetailId(record)
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
    const query: AssyDefectDetailQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      assyDefectId: masterAssyDefectId.value,
    }
    const res = await getAssyDefectDetailList(query)
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
    message.warning(t('logistics.manufacturing.defect.assy-defect.page.selectmasterfirst'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.assydefectdetail._self') })
  formData.value = {}
  formVisible.value = true
}

/** 打开编辑明细 */
async function handleEdit(record: AssyDefectDetail) {
  if (!hasMasterSelection.value) {
    message.warning(t('logistics.manufacturing.defect.assy-defect.page.selectmasterfirst'))
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.assydefectdetail._self') })
  formLoading.value = true
  try {
    const detail = await getAssyDefectDetailById(getAssyDefectDetailId(record))
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
    const id = formData.value?.assyDefectDetailId
    if (id) {
      await updateAssyDefectDetail(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.assydefectdetail._self') }))
    } else {
      await createAssyDefectDetail(payload)
      message.success(t('common.feedback.created', { target: t('entity.assydefectdetail._self') }))
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
async function handleDeleteOne(record: AssyDefectDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.assydefectdetail._self'),
      name: t('common.tip.this.target', { target: t('entity.assydefectdetail._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssyDefectDetailById(getAssyDefectDetailId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.assydefectdetail._self') }))
      await loadData()
    },
  })
}

/** 批量删除明细 */
async function handleDelete() {
  if (!hasMasterSelection.value) {
    message.warning(t('logistics.manufacturing.defect.assy-defect.page.selectmasterfirst'))
    return
  }
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.assydefectdetail._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.assydefectdetail._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getAssyDefectDetailId(r)).filter(Boolean)
      await deleteAssyDefectDetailBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.assydefectdetail._self') }))
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
