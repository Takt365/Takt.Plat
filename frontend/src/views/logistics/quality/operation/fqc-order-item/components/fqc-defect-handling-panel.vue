<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/fqc-order-item/components -->
<!-- 文件名称：fqc-defect-handling-panel.vue -->
<!-- 功能描述：FQC出货检验单明细实体主表实体右侧明细 fqcDefectHandling 独立 CRUD（按主表选中 fqcOrderItemId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="fqc-defect-handling-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.fqcdefecthandling._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:operation:fqc:order:create"
      update-permission="logistics:quality:operation:fqc:order:update"
      delete-permission="logistics:quality:operation:fqc:order:delete"
      import-permission="logistics:quality:operation:fqc:order:import"
      export-permission="logistics:quality:operation:fqc:order:export"
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
    <div class="fqc-defect-handling-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getFqcDefectHandlingId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="fqcDefectHandlingId"
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
      <FqcDefectHandlingForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterFqcOrderItemId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-operation-fqc-order-item-fqc-defect-handling"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('fqcDefectHandlingCode')">
      <a-form-item :label="t('entity.fqcdefecthandling.code')">
        <a-input
          v-model:value="advancedQueryForm.fqcDefectHandlingCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.code') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fqcOrderCode')">
      <a-form-item :label="t('entity.fqcdefecthandling.fqcordercode')">
        <a-input
          v-model:value="advancedQueryForm.fqcOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.fqcordercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.fqcdefecthandling.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectType')">
      <a-form-item :label="t('entity.fqcdefecthandling.defecttype')">
        <TaktSelect
          v-model:value="advancedQueryForm.defectType"
          dict-type="logistics_quality_defect_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.fqcdefecthandling.defecttype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectCode')">
      <a-form-item :label="t('entity.fqcdefecthandling.defectcode')">
        <a-input
          v-model:value="advancedQueryForm.defectCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.defectcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectDescription')">
      <a-form-item :label="t('entity.fqcdefecthandling.defectdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.defectDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.fqcdefecthandling.defectdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectQuantity')">
      <a-form-item :label="t('entity.fqcdefecthandling.defectquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.defectQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.defectquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingMethod')">
      <a-form-item :label="t('entity.fqcdefecthandling.handlingmethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.handlingMethod"
          dict-type="logistics_quality_defect_handling_method"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.fqcdefecthandling.handlingmethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingDescription')">
      <a-form-item :label="t('entity.fqcdefecthandling.handlingdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.handlingDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.fqcdefecthandling.handlingdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleDept')">
      <a-form-item :label="t('entity.fqcdefecthandling.responsibledept')">
        <a-input
          v-model:value="advancedQueryForm.responsibleDept"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.responsibledept') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleBy')">
      <a-form-item :label="t('entity.fqcdefecthandling.responsibleby')">
        <a-input
          v-model:value="advancedQueryForm.responsibleBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.responsibleby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlerBy')">
      <a-form-item :label="t('entity.fqcdefecthandling.handlerby')">
        <a-input
          v-model:value="advancedQueryForm.handlerBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.handlerby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtStart')">
      <a-form-item :label="t('entity.fqcdefecthandling.handlingatstart')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.handlingatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtEnd')">
      <a-form-item :label="t('entity.fqcdefecthandling.handlingatend')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.handlingatend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('correctiveAction')">
      <a-form-item :label="t('entity.fqcdefecthandling.correctiveaction')">
        <a-input
          v-model:value="advancedQueryForm.correctiveAction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.correctiveaction') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectImages')">
      <a-form-item :label="t('entity.fqcdefecthandling.defectimages')">
        <a-input
          v-model:value="advancedQueryForm.defectImages"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.defectimages') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachments')">
      <a-form-item :label="t('entity.fqcdefecthandling.attachments')">
        <a-input
          v-model:value="advancedQueryForm.attachments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.attachments') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingStatus')">
      <a-form-item :label="t('entity.fqcdefecthandling.handlingstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.handlingStatus"
          dict-type="logistics_quality_defect_handling_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.fqcdefecthandling.handlingstatus') })"
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
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.fqcdefecthandling._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.fqcdefecthandling._self"
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
      id-column-key="fqcDefectHandlingId"
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
 * FQC出货检验单明细实体子表 fqcDefectHandling 右栏面板
 * @module views/logistics/quality/operation/fqc-order-item/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import FqcDefectHandlingForm from './fqc-defect-handling-form.vue'
import { useFqcOrderItemMasterContext } from '../composables/use-fqc-order-item-master-context'
import {
  getFqcDefectHandlingList,
  getFqcDefectHandlingById,
  createFqcDefectHandling,
  updateFqcDefectHandling,
  deleteFqcDefectHandlingById,
  deleteFqcDefectHandlingBatch,
  getFqcDefectHandlingTemplate,
  importFqcDefectHandling,
  exportFqcDefectHandling,
} from '@/api/logistics/quality/operation/fqc-defect-handling'
import type { FqcDefectHandling, FqcDefectHandlingQuery } from '@/types/logistics/quality/operation/fqc-defect-handling'

const { t } = useI18n()
const { selectedMasterRow } = useFqcOrderItemMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktFqcDefectHandling')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.fqcdefecthandling._self') }),
)

const loading = ref(false)
const dataSource = ref<FqcDefectHandling[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<FqcDefectHandling | null>(null)
const selectedRows = ref<FqcDefectHandling[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<FqcDefectHandling>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  fqcDefectHandlingCode: '',
  fqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  defectType: undefined as number | undefined,
  defectCode: '',
  defectDescription: '',
  defectQuantity: undefined as number | undefined,
  handlingMethod: undefined as number | undefined,
  handlingDescription: '',
  responsibleDept: '',
  responsibleBy: '',
  handlerBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  correctiveAction: '',
  defectImages: '',
  attachments: '',
  handlingStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'fqcDefectHandlingCode', label: t('entity.fqcdefecthandling.code') },
  { key: 'fqcOrderCode', label: t('entity.fqcdefecthandling.fqcordercode') },
  { key: 'lineNumber', label: t('entity.fqcdefecthandling.linenumber') },
  { key: 'defectType', label: t('entity.fqcdefecthandling.defecttype') },
  { key: 'defectCode', label: t('entity.fqcdefecthandling.defectcode') },
  { key: 'defectDescription', label: t('entity.fqcdefecthandling.defectdescription') },
  { key: 'defectQuantity', label: t('entity.fqcdefecthandling.defectquantity') },
  { key: 'handlingMethod', label: t('entity.fqcdefecthandling.handlingmethod') },
  { key: 'handlingDescription', label: t('entity.fqcdefecthandling.handlingdescription') },
  { key: 'responsibleDept', label: t('entity.fqcdefecthandling.responsibledept') },
  { key: 'responsibleBy', label: t('entity.fqcdefecthandling.responsibleby') },
  { key: 'handlerBy', label: t('entity.fqcdefecthandling.handlerby') },
  { key: 'handlingAtStart', label: t('entity.fqcdefecthandling.handlingatstart') },
  { key: 'handlingAtEnd', label: t('entity.fqcdefecthandling.handlingatend') },
  { key: 'correctiveAction', label: t('entity.fqcdefecthandling.correctiveaction') },
  { key: 'defectImages', label: t('entity.fqcdefecthandling.defectimages') },
  { key: 'attachments', label: t('entity.fqcdefecthandling.attachments') },
  { key: 'handlingStatus', label: t('entity.fqcdefecthandling.handlingstatus') },
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
  fqcDefectHandlingCode: '',
  fqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  defectType: undefined as number | undefined,
  defectCode: '',
  defectDescription: '',
  defectQuantity: undefined as number | undefined,
  handlingMethod: undefined as number | undefined,
  handlingDescription: '',
  responsibleDept: '',
  responsibleBy: '',
  handlerBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  correctiveAction: '',
  defectImages: '',
  attachments: '',
  handlingStatus: undefined as number | undefined,
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
const importVisible = ref(false)

const entityIdName = 'fqcDefectHandlingId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.fqcOrderItemId)
const masterFqcOrderItemId = computed(() => selectedMasterRow.value?.fqcOrderItemId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getFqcDefectHandlingId(record: FqcDefectHandling | Record<string, unknown>): string {
  return String((record as FqcDefectHandling)?.[entityIdName] ?? '')
}

function getFqcDefectHandlingField(record: FqcDefectHandling | Record<string, unknown>, field: string): unknown {
  return (record as FqcDefectHandling)?.[field as keyof FqcDefectHandling]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'fqcDefectHandlingId',
    key: 'fqcDefectHandlingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'fqcDefectHandlingId') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.code'),
    dataIndex: 'fqcDefectHandlingCode',
    key: 'fqcDefectHandlingCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'fqcDefectHandlingCode') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.fqcordercode'),
    dataIndex: 'fqcOrderCode',
    key: 'fqcOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'fqcOrderCode') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.defecttype'),
    dataIndex: 'defectType',
    key: 'defectType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'defectType') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.defectcode'),
    dataIndex: 'defectCode',
    key: 'defectCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'defectCode') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.defectdescription'),
    dataIndex: 'defectDescription',
    key: 'defectDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'defectDescription') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.defectquantity'),
    dataIndex: 'defectQuantity',
    key: 'defectQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'defectQuantity') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.handlingmethod'),
    dataIndex: 'handlingMethod',
    key: 'handlingMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'handlingMethod') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.handlingdescription'),
    dataIndex: 'handlingDescription',
    key: 'handlingDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'handlingDescription') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.responsibledept'),
    dataIndex: 'responsibleDept',
    key: 'responsibleDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'responsibleDept') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.responsibleby'),
    dataIndex: 'responsibleBy',
    key: 'responsibleBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'responsibleBy') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.handlerby'),
    dataIndex: 'handlerBy',
    key: 'handlerBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'handlerBy') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.handlingat'),
    dataIndex: 'handlingAt',
    key: 'handlingAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'handlingAt') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.correctiveaction'),
    dataIndex: 'correctiveAction',
    key: 'correctiveAction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'correctiveAction') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.defectimages'),
    dataIndex: 'defectImages',
    key: 'defectImages',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'defectImages') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.attachments'),
    dataIndex: 'attachments',
    key: 'attachments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'attachments') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.handlingstatus'),
    dataIndex: 'handlingStatus',
    key: 'handlingStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'handlingStatus') ?? ''),
  },
  {
    title: t('entity.fqcdefecthandling.orderitem'),
    dataIndex: 'orderItem',
    key: 'orderItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: FqcDefectHandling }) =>
      String(getFqcDefectHandlingField(record, 'orderItem') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:fqc:order:update',
        onClick: (record: FqcDefectHandling) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:fqc:order:delete',
        onClick: (record: FqcDefectHandling) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: FqcDefectHandling[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: FqcDefectHandling, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getFqcDefectHandlingId(selectedRow.value) === getFqcDefectHandlingId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: FqcDefectHandling[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: FqcDefectHandling) {
  const key = getFqcDefectHandlingId(record)
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
 * @returns {FqcDefectHandlingQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<FqcDefectHandlingQuery>): FqcDefectHandlingQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: FqcDefectHandlingQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    fqcOrderItemId: masterFqcOrderItemId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof FqcDefectHandlingQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('fqcDefectHandlingCode', form.fqcDefectHandlingCode)
  assignTrimmed('fqcOrderCode', form.fqcOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.defectType !== undefined && form.defectType !== null) {
    query.defectType = form.defectType
  }
  assignTrimmed('defectCode', form.defectCode)
  assignTrimmed('defectDescription', form.defectDescription)
  if (form.defectQuantity !== undefined && form.defectQuantity !== null) {
    query.defectQuantity = form.defectQuantity
  }
  if (form.handlingMethod !== undefined && form.handlingMethod !== null) {
    query.handlingMethod = form.handlingMethod
  }
  assignTrimmed('handlingDescription', form.handlingDescription)
  assignTrimmed('responsibleDept', form.responsibleDept)
  assignTrimmed('responsibleBy', form.responsibleBy)
  assignTrimmed('handlerBy', form.handlerBy)
  assignTrimmed('handlingAtStart', form.handlingAtStart)
  assignTrimmed('handlingAtEnd', form.handlingAtEnd)
  assignTrimmed('correctiveAction', form.correctiveAction)
  assignTrimmed('defectImages', form.defectImages)
  assignTrimmed('attachments', form.attachments)
  if (form.handlingStatus !== undefined && form.handlingStatus !== null) {
    query.handlingStatus = form.handlingStatus
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
    const res = await getFqcDefectHandlingList(buildListQuery())
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
watch(masterFqcOrderItemId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.fqcdefecthandling._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: FqcDefectHandling) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.fqcdefecthandling._self') })
  formLoading.value = true
  try {
    const detail = await getFqcDefectHandlingById(getFqcDefectHandlingId(record))
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
      entity: t('entity.fqcdefecthandling._self'),
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
    const id = formData.value?.fqcDefectHandlingId
    if (id) {
      await updateFqcDefectHandling(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.fqcdefecthandling._self') }))
    } else {
      await createFqcDefectHandling(payload)
      message.success(t('common.feedback.created', { target: t('entity.fqcdefecthandling._self') }))
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

async function handleDeleteOne(record: FqcDefectHandling) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.fqcdefecthandling._self'),
      name: t('common.tip.this.target', { target: t('entity.fqcdefecthandling._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteFqcDefectHandlingById(getFqcDefectHandlingId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.fqcdefecthandling._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.fqcdefecthandling._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.fqcdefecthandling._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getFqcDefectHandlingId(r)).filter(Boolean)
      await deleteFqcDefectHandlingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.fqcdefecthandling._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

/** 打开导入对话框 */
function handleImport() {
  if (!hasMasterSelection.value) {
      message.warning(t('common.status.empty'))
      return
    }
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getFqcDefectHandlingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importFqcDefectHandling(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  void loadData()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
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
    const exportMeta = await exportFqcDefectHandling(
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
    message.success(t('common.feedback.export.success', { target: t('entity.fqcdefecthandling._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.fqcdefecthandling._self') }))
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
