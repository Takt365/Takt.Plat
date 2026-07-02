<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing-item/components -->
<!-- 文件名称：routing-item-argument-panel.vue -->
<!-- 功能描述：工艺路线明细表实体主表实体右侧明细 routingItemArgument 独立 CRUD（按主表选中 routingItemId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="routing-item-argument-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.routingitemargument._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:bom:routing:item:create"
      update-permission="logistics:manufacturing:bom:routing:item:update"
      delete-permission="logistics:manufacturing:bom:routing:item:delete"
      import-permission="logistics:manufacturing:bom:routing:item:import"
      export-permission="logistics:manufacturing:bom:routing:item:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
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
    <div class="routing-item-argument-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getRoutingItemArgumentId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="routingItemArgumentId"
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
      <RoutingItemArgumentForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterRoutingItemId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-bom-routing-item-routing-item-argument"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('paramCode')">
      <a-form-item :label="t('entity.routingitemargument.paramcode')">
        <a-input
          v-model:value="advancedQueryForm.paramCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitemargument.paramcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paramName')">
      <a-form-item :label="t('entity.routingitemargument.paramname')">
        <a-input
          v-model:value="advancedQueryForm.paramName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitemargument.paramname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paramUnit')">
      <a-form-item :label="t('entity.routingitemargument.paramunit')">
        <a-input
          v-model:value="advancedQueryForm.paramUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitemargument.paramunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardValue')">
      <a-form-item :label="t('entity.routingitemargument.standardvalue')">
        <a-input-number
          v-model:value="advancedQueryForm.standardValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitemargument.standardvalue') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lowerLimit')">
      <a-form-item :label="t('entity.routingitemargument.lowerlimit')">
        <a-input-number
          v-model:value="advancedQueryForm.lowerLimit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitemargument.lowerlimit') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('upperLimit')">
      <a-form-item :label="t('entity.routingitemargument.upperlimit')">
        <a-input-number
          v-model:value="advancedQueryForm.upperLimit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitemargument.upperlimit') })"
          style="width: 100%"
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
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.routingitemargument._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.routingitemargument._self"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="routingItemArgumentId"
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
 * 工艺路线明细表实体子表 routingItemArgument 右栏面板
 * @module views/logistics/manufacturing/bom/routing-item/components
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
import RoutingItemArgumentForm from './routing-item-argument-form.vue'
import { useRoutingItemMasterContext } from '../composables/use-routing-item-master-context'
import {
  getRoutingItemArgumentList,
  getRoutingItemArgumentById,
  createRoutingItemArgument,
  updateRoutingItemArgument,
  deleteRoutingItemArgumentById,
  deleteRoutingItemArgumentBatch,
  getRoutingItemArgumentTemplate,
  importRoutingItemArgument,
  exportRoutingItemArgument,
} from '@/api/logistics/manufacturing/bom/routing-item-argument'
import type { RoutingItemArgument, RoutingItemArgumentQuery } from '@/types/logistics/manufacturing/bom/routing-item-argument'

const { t } = useI18n()
const { selectedMasterRow } = useRoutingItemMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktRoutingItemArgument')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.routingitemargument._self') }),
)

const loading = ref(false)
const dataSource = ref<RoutingItemArgument[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<RoutingItemArgument | null>(null)
const selectedRows = ref<RoutingItemArgument[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<RoutingItemArgument>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  paramCode: '',
  paramName: '',
  paramUnit: '',
  standardValue: undefined as number | undefined,
  lowerLimit: undefined as number | undefined,
  upperLimit: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'paramCode', label: t('entity.routingitemargument.paramcode') },
  { key: 'paramName', label: t('entity.routingitemargument.paramname') },
  { key: 'paramUnit', label: t('entity.routingitemargument.paramunit') },
  { key: 'standardValue', label: t('entity.routingitemargument.standardvalue') },
  { key: 'lowerLimit', label: t('entity.routingitemargument.lowerlimit') },
  { key: 'upperLimit', label: t('entity.routingitemargument.upperlimit') },
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
  paramCode: '',
  paramName: '',
  paramUnit: '',
  standardValue: undefined as number | undefined,
  lowerLimit: undefined as number | undefined,
  upperLimit: undefined as number | undefined,
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
const importVisible = ref(false)

const entityIdName = 'routingItemArgumentId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.routingItemId)
const masterRoutingItemId = computed(() => selectedMasterRow.value?.routingItemId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getRoutingItemArgumentId(record: RoutingItemArgument | Record<string, unknown>): string {
  return String((record as RoutingItemArgument)?.[entityIdName] ?? '')
}

function getRoutingItemArgumentField(record: RoutingItemArgument | Record<string, unknown>, field: string): unknown {
  return (record as RoutingItemArgument)?.[field as keyof RoutingItemArgument]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'routingItemArgumentId',
    key: 'routingItemArgumentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: RoutingItemArgument }) =>
      String(getRoutingItemArgumentField(record, 'routingItemArgumentId') ?? ''),
  },
  {
    title: t('entity.routingitemargument.paramcode'),
    dataIndex: 'paramCode',
    key: 'paramCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItemArgument }) =>
      String(getRoutingItemArgumentField(record, 'paramCode') ?? ''),
  },
  {
    title: t('entity.routingitemargument.paramname'),
    dataIndex: 'paramName',
    key: 'paramName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItemArgument }) =>
      String(getRoutingItemArgumentField(record, 'paramName') ?? ''),
  },
  {
    title: t('entity.routingitemargument.paramunit'),
    dataIndex: 'paramUnit',
    key: 'paramUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItemArgument }) =>
      String(getRoutingItemArgumentField(record, 'paramUnit') ?? ''),
  },
  {
    title: t('entity.routingitemargument.standardvalue'),
    dataIndex: 'standardValue',
    key: 'standardValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItemArgument }) =>
      String(getRoutingItemArgumentField(record, 'standardValue') ?? ''),
  },
  {
    title: t('entity.routingitemargument.lowerlimit'),
    dataIndex: 'lowerLimit',
    key: 'lowerLimit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItemArgument }) =>
      String(getRoutingItemArgumentField(record, 'lowerLimit') ?? ''),
  },
  {
    title: t('entity.routingitemargument.upperlimit'),
    dataIndex: 'upperLimit',
    key: 'upperLimit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItemArgument }) =>
      String(getRoutingItemArgumentField(record, 'upperLimit') ?? ''),
  },
  {
    title: t('entity.routingitemargument.routingitem'),
    dataIndex: 'routingItem',
    key: 'routingItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: RoutingItemArgument }) =>
      String(getRoutingItemArgumentField(record, 'routingItem') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:routing:item:update',
        onClick: (record: RoutingItemArgument) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:routing:item:delete',
        onClick: (record: RoutingItemArgument) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: RoutingItemArgument[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: RoutingItemArgument, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getRoutingItemArgumentId(selectedRow.value) === getRoutingItemArgumentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: RoutingItemArgument[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: RoutingItemArgument) {
  const key = getRoutingItemArgumentId(record)
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
 * @returns {RoutingItemArgumentQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<RoutingItemArgumentQuery>): RoutingItemArgumentQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: RoutingItemArgumentQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    routingItemId: masterRoutingItemId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof RoutingItemArgumentQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('paramCode', form.paramCode)
  assignTrimmed('paramName', form.paramName)
  assignTrimmed('paramUnit', form.paramUnit)
  if (form.standardValue !== undefined && form.standardValue !== null) {
    query.standardValue = form.standardValue
  }
  if (form.lowerLimit !== undefined && form.lowerLimit !== null) {
    query.lowerLimit = form.lowerLimit
  }
  if (form.upperLimit !== undefined && form.upperLimit !== null) {
    query.upperLimit = form.upperLimit
  }
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
    const res = await getRoutingItemArgumentList(buildListQuery())
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
watch(masterRoutingItemId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.routingitemargument._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: RoutingItemArgument) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.routingitemargument._self') })
  formLoading.value = true
  try {
    const detail = await getRoutingItemArgumentById(getRoutingItemArgumentId(record))
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
      entity: t('entity.routingitemargument._self'),
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
    const id = formData.value?.routingItemArgumentId
    if (id) {
      await updateRoutingItemArgument(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.routingitemargument._self') }))
    } else {
      await createRoutingItemArgument(payload)
      message.success(t('common.feedback.created', { target: t('entity.routingitemargument._self') }))
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

async function handleDeleteOne(record: RoutingItemArgument) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.routingitemargument._self'),
      name: t('common.tip.this.target', { target: t('entity.routingitemargument._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteRoutingItemArgumentById(getRoutingItemArgumentId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.routingitemargument._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.routingitemargument._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.routingitemargument._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getRoutingItemArgumentId(r)).filter(Boolean)
      await deleteRoutingItemArgumentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.routingitemargument._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleImport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getRoutingItemArgumentTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importRoutingItemArgument(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    const exportMeta = await exportRoutingItemArgument(
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
    message.success(t('common.feedback.export.success', { target: t('entity.routingitemargument._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.routingitemargument._self') }))
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
