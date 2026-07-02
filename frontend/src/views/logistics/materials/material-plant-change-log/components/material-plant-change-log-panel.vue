<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-plant-change-log/components -->
<!-- 文件名称：material-plant-change-log-panel.vue -->
<!-- 功能描述：Takt工厂物料实体主表实体右侧明细 materialPlantChangeLog 独立 CRUD（按主表选中 materialPlantId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="material-plant-change-log-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.materialplantchangelog._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:materials:material:plant:create"
      update-permission="logistics:materials:material:plant:update"
      delete-permission="logistics:materials:material:plant:delete"

      export-permission="logistics:materials:material:plant:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="false"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"

      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
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
    <div class="material-plant-change-log-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getMaterialPlantChangeLogId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="materialPlantChangeLogId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
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
      <MaterialPlantChangeLogForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterMaterialPlantId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-materials-material-plant-change-log-material-plant-change-log"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.materialplantchangelog.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplantchangelog.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.materialplantchangelog.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplantchangelog.plantcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeFields')">
      <a-form-item :label="t('entity.materialplantchangelog.changefields')">
        <a-input
          v-model:value="advancedQueryForm.changeFields"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplantchangelog.changefields') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeTimeStart')">
      <a-form-item :label="t('entity.materialplantchangelog.changetimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.changeTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplantchangelog.changetimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeTimeEnd')">
      <a-form-item :label="t('entity.materialplantchangelog.changetimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.changeTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplantchangelog.changetimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeBy')">
      <a-form-item :label="t('entity.materialplantchangelog.changeby')">
        <a-input
          v-model:value="advancedQueryForm.changeBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplantchangelog.changeby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeReason')">
      <a-form-item :label="t('entity.materialplantchangelog.changereason')">
        <a-input
          v-model:value="advancedQueryForm.changeReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplantchangelog.changereason') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="t('common.page.entity.createdatstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="t('common.page.entity.createdatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ t('common.page.entity.extfield') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="materialPlantChangeLogId"
      action-column-key="action"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt工厂物料实体子表 materialPlantChangeLog 右栏面板
 * @module views/logistics/materials/material-plant-change-log/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import MaterialPlantChangeLogForm from './material-plant-change-log-form.vue'
import { useMaterialPlantMasterContext } from '../composables/use-material-plant-master-context'
import {
  getMaterialPlantChangeLogList,
  getMaterialPlantChangeLogById,
  createMaterialPlantChangeLog,
  updateMaterialPlantChangeLog,
  deleteMaterialPlantChangeLogById,
  deleteMaterialPlantChangeLogBatch,
  exportMaterialPlantChangeLog,
} from '@/api/logistics/materials/material-plant-change-log'
import type { MaterialPlantChangeLog, MaterialPlantChangeLogQuery } from '@/types/logistics/materials/material-plant-change-log'

const { t } = useI18n()
const { selectedMasterRow } = useMaterialPlantMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterialPlantChangeLog')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.materialplantchangelog._self') }),
)

const loading = ref(false)
const dataSource = ref<MaterialPlantChangeLog[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<MaterialPlantChangeLog | null>(null)
const selectedRows = ref<MaterialPlantChangeLog[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MaterialPlantChangeLog>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  materialCode: '',
  plantCode: '',
  changeFields: '',
  changeTimeStart: '',
  changeTimeEnd: '',
  changeBy: '',
  changeReason: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'materialCode', label: t('entity.materialplantchangelog.materialcode') },
  { key: 'plantCode', label: t('entity.materialplantchangelog.plantcode') },
  { key: 'changeFields', label: t('entity.materialplantchangelog.changefields') },
  { key: 'changeTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.materialplantchangelog.changetime')) },
  { key: 'changeTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.materialplantchangelog.changetime')) },
  { key: 'changeBy', label: t('entity.materialplantchangelog.changeby') },
  { key: 'changeReason', label: t('entity.materialplantchangelog.changereason') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])

/**
 * 高级查询字段标签
 * @param key 字段 key
 */
function fieldLabel(key: string): string {
  return queryFieldsMeta.value.find((f) => f.key === key)?.label ?? key
}

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  materialCode: '',
  plantCode: '',
  changeFields: '',
  changeTimeStart: '',
  changeTimeEnd: '',
  changeBy: '',
  changeReason: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key（空数组时按 tableMode=masterDetailDetail 默认 id+4 业务列） */
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

const entityIdName = 'materialPlantChangeLogId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.materialPlantId)
const masterMaterialPlantId = computed(() => selectedMasterRow.value?.materialPlantId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getMaterialPlantChangeLogId(record: MaterialPlantChangeLog | Record<string, unknown>): string {
  return String((record as MaterialPlantChangeLog)?.[entityIdName] ?? '')
}

function getMaterialPlantChangeLogField(record: MaterialPlantChangeLog | Record<string, unknown>, field: string): unknown {
  return (record as MaterialPlantChangeLog)?.[field as keyof MaterialPlantChangeLog]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'materialPlantChangeLogId',
    key: 'materialPlantChangeLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MaterialPlantChangeLog }) =>
      String(getMaterialPlantChangeLogField(record, 'materialPlantChangeLogId') ?? ''),
  },
  {
    title: t('entity.materialplantchangelog.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialPlantChangeLog }) =>
      String(getMaterialPlantChangeLogField(record, 'materialCode') ?? ''),
  },
  {
    title: t('entity.materialplantchangelog.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialPlantChangeLog }) =>
      String(getMaterialPlantChangeLogField(record, 'plantCode') ?? ''),
  },
  {
    title: t('entity.materialplantchangelog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialPlantChangeLog }) =>
      String(getMaterialPlantChangeLogField(record, 'changeFields') ?? ''),
  },
  {
    title: t('entity.materialplantchangelog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialPlantChangeLog }) =>
      String(getMaterialPlantChangeLogField(record, 'changeTime') ?? ''),
  },
  {
    title: t('entity.materialplantchangelog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialPlantChangeLog }) =>
      String(getMaterialPlantChangeLogField(record, 'changeBy') ?? ''),
  },
  {
    title: t('entity.materialplantchangelog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialPlantChangeLog }) =>
      String(getMaterialPlantChangeLogField(record, 'changeReason') ?? ''),
  },
  {
    title: t('entity.materialplantchangelog.materialplant'),
    dataIndex: 'materialPlant',
    key: 'materialPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialPlantChangeLog }) =>
      String(getMaterialPlantChangeLogField(record, 'materialPlant') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:material:plant:update',
        onClick: (record: MaterialPlantChangeLog) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:material:plant:delete',
        onClick: (record: MaterialPlantChangeLog) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaterialPlantChangeLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaterialPlantChangeLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getMaterialPlantChangeLogId(selectedRow.value) === getMaterialPlantChangeLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaterialPlantChangeLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MaterialPlantChangeLog) {
  const key = getMaterialPlantChangeLogId(record)
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
 * @returns {MaterialPlantChangeLogQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaterialPlantChangeLogQuery>): MaterialPlantChangeLogQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaterialPlantChangeLogQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    materialPlantId: masterMaterialPlantId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaterialPlantChangeLogQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('changeFields', form.changeFields)
  assignTrimmed('changeTimeStart', form.changeTimeStart)
  assignTrimmed('changeTimeEnd', form.changeTimeEnd)
  assignTrimmed('changeBy', form.changeBy)
  assignTrimmed('changeReason', form.changeReason)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
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
    const res = await getMaterialPlantChangeLogList(buildListQuery())
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
watch(masterMaterialPlantId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.materialplantchangelog._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: MaterialPlantChangeLog) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.materialplantchangelog._self') })
  formLoading.value = true
  try {
    const detail = await getMaterialPlantChangeLogById(getMaterialPlantChangeLogId(record))
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
      entity: t('entity.materialplantchangelog._self'),
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
    const id = formData.value?.materialPlantChangeLogId
    if (id) {
      await updateMaterialPlantChangeLog(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.materialplantchangelog._self') }))
    } else {
      await createMaterialPlantChangeLog(payload)
      message.success(t('common.feedback.created', { target: t('entity.materialplantchangelog._self') }))
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

async function handleDeleteOne(record: MaterialPlantChangeLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.materialplantchangelog._self'),
      name: t('common.tip.this.target', { target: t('entity.materialplantchangelog._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialPlantChangeLogById(getMaterialPlantChangeLogId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.materialplantchangelog._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.materialplantchangelog._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.materialplantchangelog._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getMaterialPlantChangeLogId(r)).filter(Boolean)
      await deleteMaterialPlantChangeLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.materialplantchangelog._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    const exportMeta = await exportMaterialPlantChangeLog(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.materialplantchangelog._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.materialplantchangelog._self') }))
  } finally {
    loading.value = false
  }
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
