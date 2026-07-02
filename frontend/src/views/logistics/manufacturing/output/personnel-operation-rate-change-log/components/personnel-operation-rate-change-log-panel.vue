<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/personnel-operation-rate-change-log/components -->
<!-- 文件名称：personnel-operation-rate-change-log-panel.vue -->
<!-- 功能描述：人员稼动率实体主表实体右侧明细 personnelOperationRateChangeLog 独立 CRUD（按主表选中 personnelOperationRateId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="personnel-operation-rate-change-log-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.personneloperationratechangelog._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:output:personnel:operation:rate:create"
      update-permission="logistics:manufacturing:output:personnel:operation:rate:update"
      delete-permission="logistics:manufacturing:output:personnel:operation:rate:delete"

      export-permission="logistics:manufacturing:output:personnel:operation:rate:export"
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
    <div class="personnel-operation-rate-change-log-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getPersonnelOperationRateChangeLogId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="personnelOperationRateChangeLogId"
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
      <PersonnelOperationRateChangeLogForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterPersonnelOperationRateId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-output-personnel-operation-rate-change-log-personnel-operation-rate-change-log"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('prodTeam')">
      <a-form-item :label="t('entity.personneloperationratechangelog.prodteam')">
        <a-input
          v-model:value="advancedQueryForm.prodTeam"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationratechangelog.prodteam') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeFields')">
      <a-form-item :label="t('entity.personneloperationratechangelog.changefields')">
        <a-input
          v-model:value="advancedQueryForm.changeFields"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationratechangelog.changefields') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeTimeStart')">
      <a-form-item :label="t('entity.personneloperationratechangelog.changetimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.changeTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personneloperationratechangelog.changetimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeTimeEnd')">
      <a-form-item :label="t('entity.personneloperationratechangelog.changetimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.changeTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personneloperationratechangelog.changetimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeBy')">
      <a-form-item :label="t('entity.personneloperationratechangelog.changeby')">
        <a-input
          v-model:value="advancedQueryForm.changeBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationratechangelog.changeby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeReason')">
      <a-form-item :label="t('entity.personneloperationratechangelog.changereason')">
        <a-input
          v-model:value="advancedQueryForm.changeReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationratechangelog.changereason') })"
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
      id-column-key="personnelOperationRateChangeLogId"
      action-column-key="action"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 人员稼动率实体子表 personnelOperationRateChangeLog 右栏面板
 * @module views/logistics/manufacturing/output/personnel-operation-rate-change-log/components
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
import PersonnelOperationRateChangeLogForm from './personnel-operation-rate-change-log-form.vue'
import { usePersonnelOperationRateMasterContext } from '../composables/use-personnel-operation-rate-master-context'
import {
  getPersonnelOperationRateChangeLogList,
  getPersonnelOperationRateChangeLogById,
  createPersonnelOperationRateChangeLog,
  updatePersonnelOperationRateChangeLog,
  deletePersonnelOperationRateChangeLogById,
  deletePersonnelOperationRateChangeLogBatch,
  exportPersonnelOperationRateChangeLog,
} from '@/api/logistics/manufacturing/output/personnel-operation-rate-change-log'
import type { PersonnelOperationRateChangeLog, PersonnelOperationRateChangeLogQuery } from '@/types/logistics/manufacturing/output/personnel-operation-rate-change-log'

const { t } = useI18n()
const { selectedMasterRow } = usePersonnelOperationRateMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPersonnelOperationRateChangeLog')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.personneloperationratechangelog._self') }),
)

const loading = ref(false)
const dataSource = ref<PersonnelOperationRateChangeLog[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<PersonnelOperationRateChangeLog | null>(null)
const selectedRows = ref<PersonnelOperationRateChangeLog[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PersonnelOperationRateChangeLog>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  prodTeam: '',
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
  { key: 'prodTeam', label: t('entity.personneloperationratechangelog.prodteam') },
  { key: 'changeFields', label: t('entity.personneloperationratechangelog.changefields') },
  { key: 'changeTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.personneloperationratechangelog.changetime')) },
  { key: 'changeTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.personneloperationratechangelog.changetime')) },
  { key: 'changeBy', label: t('entity.personneloperationratechangelog.changeby') },
  { key: 'changeReason', label: t('entity.personneloperationratechangelog.changereason') },
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
  prodTeam: '',
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

const entityIdName = 'personnelOperationRateChangeLogId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.personnelOperationRateId)
const masterPersonnelOperationRateId = computed(() => selectedMasterRow.value?.personnelOperationRateId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getPersonnelOperationRateChangeLogId(record: PersonnelOperationRateChangeLog | Record<string, unknown>): string {
  return String((record as PersonnelOperationRateChangeLog)?.[entityIdName] ?? '')
}

function getPersonnelOperationRateChangeLogField(record: PersonnelOperationRateChangeLog | Record<string, unknown>, field: string): unknown {
  return (record as PersonnelOperationRateChangeLog)?.[field as keyof PersonnelOperationRateChangeLog]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'personnelOperationRateChangeLogId',
    key: 'personnelOperationRateChangeLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: PersonnelOperationRateChangeLog }) =>
      String(getPersonnelOperationRateChangeLogField(record, 'personnelOperationRateChangeLogId') ?? ''),
  },
  {
    title: t('entity.personneloperationratechangelog.prodteam'),
    dataIndex: 'prodTeam',
    key: 'prodTeam',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PersonnelOperationRateChangeLog }) =>
      String(getPersonnelOperationRateChangeLogField(record, 'prodTeam') ?? ''),
  },
  {
    title: t('entity.personneloperationratechangelog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PersonnelOperationRateChangeLog }) =>
      String(getPersonnelOperationRateChangeLogField(record, 'changeFields') ?? ''),
  },
  {
    title: t('entity.personneloperationratechangelog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PersonnelOperationRateChangeLog }) =>
      String(getPersonnelOperationRateChangeLogField(record, 'changeTime') ?? ''),
  },
  {
    title: t('entity.personneloperationratechangelog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PersonnelOperationRateChangeLog }) =>
      String(getPersonnelOperationRateChangeLogField(record, 'changeBy') ?? ''),
  },
  {
    title: t('entity.personneloperationratechangelog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PersonnelOperationRateChangeLog }) =>
      String(getPersonnelOperationRateChangeLogField(record, 'changeReason') ?? ''),
  },
  {
    title: t('entity.personneloperationratechangelog.personneloperationrate'),
    dataIndex: 'personnelOperationRate',
    key: 'personnelOperationRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PersonnelOperationRateChangeLog }) =>
      String(getPersonnelOperationRateChangeLogField(record, 'personnelOperationRate') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:personnel:operation:rate:update',
        onClick: (record: PersonnelOperationRateChangeLog) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:personnel:operation:rate:delete',
        onClick: (record: PersonnelOperationRateChangeLog) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PersonnelOperationRateChangeLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PersonnelOperationRateChangeLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getPersonnelOperationRateChangeLogId(selectedRow.value) === getPersonnelOperationRateChangeLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PersonnelOperationRateChangeLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: PersonnelOperationRateChangeLog) {
  const key = getPersonnelOperationRateChangeLogId(record)
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
 * @returns {PersonnelOperationRateChangeLogQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PersonnelOperationRateChangeLogQuery>): PersonnelOperationRateChangeLogQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PersonnelOperationRateChangeLogQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    personnelOperationRateId: masterPersonnelOperationRateId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PersonnelOperationRateChangeLogQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('prodTeam', form.prodTeam)
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
    const res = await getPersonnelOperationRateChangeLogList(buildListQuery())
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
watch(masterPersonnelOperationRateId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.personneloperationratechangelog._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: PersonnelOperationRateChangeLog) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.personneloperationratechangelog._self') })
  formLoading.value = true
  try {
    const detail = await getPersonnelOperationRateChangeLogById(getPersonnelOperationRateChangeLogId(record))
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
      entity: t('entity.personneloperationratechangelog._self'),
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
    const id = formData.value?.personnelOperationRateChangeLogId
    if (id) {
      await updatePersonnelOperationRateChangeLog(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.personneloperationratechangelog._self') }))
    } else {
      await createPersonnelOperationRateChangeLog(payload)
      message.success(t('common.feedback.created', { target: t('entity.personneloperationratechangelog._self') }))
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

async function handleDeleteOne(record: PersonnelOperationRateChangeLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.personneloperationratechangelog._self'),
      name: t('common.tip.this.target', { target: t('entity.personneloperationratechangelog._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePersonnelOperationRateChangeLogById(getPersonnelOperationRateChangeLogId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.personneloperationratechangelog._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.personneloperationratechangelog._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.personneloperationratechangelog._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getPersonnelOperationRateChangeLogId(r)).filter(Boolean)
      await deletePersonnelOperationRateChangeLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.personneloperationratechangelog._self') }))
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
    const exportMeta = await exportPersonnelOperationRateChangeLog(
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
    message.success(t('common.feedback.export.success', { target: t('entity.personneloperationratechangelog._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.personneloperationratechangelog._self') }))
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
