<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/supplier-evaluation/components -->
<!-- 文件名称：supplier-evaluation-item-panel.vue -->
<!-- 功能描述：供应商评价考核主表实体主表实体右侧明细 supplierEvaluationItem 独立 CRUD（按主表选中 evaluationId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="supplier-evaluation-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.supplierevaluationitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:complaint:supplierevaluation:create"
      update-permission="logistics:quality:complaint:supplierevaluation:update"
      delete-permission="logistics:quality:complaint:supplierevaluation:delete"
      import-permission="logistics:quality:complaint:supplierevaluation:import"
      export-permission="logistics:quality:complaint:supplierevaluation:export"
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
    <div class="supplier-evaluation-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getSupplierEvaluationItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="supplierEvaluationItemId"
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
      <SupplierEvaluationItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterSupplierEvaluationId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-complaint-supplier-evaluation-supplier-evaluation-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('supplierEvaluationCode')">
      <a-form-item :label="t('entity.supplierevaluationitem.supplierevaluationcode')">
        <a-input
          v-model:value="advancedQueryForm.supplierEvaluationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.supplierevaluationcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.supplierevaluationitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('categoryType')">
      <a-form-item :label="t('entity.supplierevaluationitem.categorytype')">
        <a-input-number
          v-model:value="advancedQueryForm.categoryType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.categorytype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemName')">
      <a-form-item :label="t('entity.supplierevaluationitem.itemname')">
        <a-input
          v-model:value="advancedQueryForm.itemName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.itemname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemDescription')">
      <a-form-item :label="t('entity.supplierevaluationitem.itemdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.itemDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.supplierevaluationitem.itemdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weight')">
      <a-form-item :label="t('entity.supplierevaluationitem.weight')">
        <a-input-number
          v-model:value="advancedQueryForm.weight"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.weight') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scoringStandard')">
      <a-form-item :label="t('entity.supplierevaluationitem.scoringstandard')">
        <a-input
          v-model:value="advancedQueryForm.scoringStandard"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.scoringstandard') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('score')">
      <a-form-item :label="t('entity.supplierevaluationitem.score')">
        <a-input-number
          v-model:value="advancedQueryForm.score"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.score') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ratingLevel')">
      <a-form-item :label="t('entity.supplierevaluationitem.ratinglevel')">
        <a-input-number
          v-model:value="advancedQueryForm.ratingLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.ratinglevel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationComment')">
      <a-form-item :label="t('entity.supplierevaluationitem.evaluationcomment')">
        <a-input
          v-model:value="advancedQueryForm.evaluationComment"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.evaluationcomment') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('existingIssues')">
      <a-form-item :label="t('entity.supplierevaluationitem.existingissues')">
        <a-input
          v-model:value="advancedQueryForm.existingIssues"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.existingissues') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementRequirement')">
      <a-form-item :label="t('entity.supplierevaluationitem.improvementrequirement')">
        <a-input
          v-model:value="advancedQueryForm.improvementRequirement"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.improvementrequirement') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationRequired')">
      <a-form-item :label="t('entity.supplierevaluationitem.rectificationrequired')">
        <a-input-number
          v-model:value="advancedQueryForm.rectificationRequired"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.rectificationrequired') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationDeadlineStart')">
      <a-form-item :label="t('entity.supplierevaluationitem.rectificationdeadlinestart')">
        <a-input
          v-model:value="advancedQueryForm.rectificationDeadlineStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.rectificationdeadlinestart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationDeadlineEnd')">
      <a-form-item :label="t('entity.supplierevaluationitem.rectificationdeadlineend')">
        <a-input
          v-model:value="advancedQueryForm.rectificationDeadlineEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.rectificationdeadlineend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationStatus')">
      <a-form-item :label="t('entity.supplierevaluationitem.rectificationstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.rectificationStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluationitem.rectificationstatus') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.supplierevaluationitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.supplierevaluationitem._self"
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
      id-column-key="supplierEvaluationItemId"
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
 * 供应商评价考核主表实体子表 supplierEvaluationItem 右栏面板
 * @module views/logistics/quality/complaint/supplier-evaluation/components
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
import SupplierEvaluationItemForm from './supplier-evaluation-item-form.vue'
import { useSupplierEvaluationMasterContext } from '../composables/use-supplier-evaluation-master-context'
import {
  getSupplierEvaluationItemList,
  getSupplierEvaluationItemById,
  createSupplierEvaluationItem,
  updateSupplierEvaluationItem,
  deleteSupplierEvaluationItemById,
  deleteSupplierEvaluationItemBatch,
  getSupplierEvaluationItemTemplate,
  importSupplierEvaluationItem,
  exportSupplierEvaluationItem,
} from '@/api/logistics/quality/complaint/supplier-evaluation-item'
import type { SupplierEvaluationItem, SupplierEvaluationItemQuery } from '@/types/logistics/quality/complaint/supplier-evaluation-item'

const { t } = useI18n()
const { selectedMasterRow } = useSupplierEvaluationMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSupplierEvaluationItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.supplierevaluationitem._self') }),
)

const loading = ref(false)
const dataSource = ref<SupplierEvaluationItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<SupplierEvaluationItem | null>(null)
const selectedRows = ref<SupplierEvaluationItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SupplierEvaluationItem>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  supplierEvaluationCode: '',
  lineNumber: undefined as number | undefined,
  categoryType: undefined as number | undefined,
  itemName: '',
  itemDescription: '',
  weight: undefined as number | undefined,
  scoringStandard: '',
  score: undefined as number | undefined,
  ratingLevel: undefined as number | undefined,
  evaluationComment: '',
  existingIssues: '',
  improvementRequirement: '',
  rectificationRequired: undefined as number | undefined,
  rectificationDeadlineStart: '',
  rectificationDeadlineEnd: '',
  rectificationStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'supplierEvaluationCode', label: t('entity.supplierevaluationitem.supplierevaluationcode') },
  { key: 'lineNumber', label: t('entity.supplierevaluationitem.linenumber') },
  { key: 'categoryType', label: t('entity.supplierevaluationitem.categorytype') },
  { key: 'itemName', label: t('entity.supplierevaluationitem.itemname') },
  { key: 'itemDescription', label: t('entity.supplierevaluationitem.itemdescription') },
  { key: 'weight', label: t('entity.supplierevaluationitem.weight') },
  { key: 'scoringStandard', label: t('entity.supplierevaluationitem.scoringstandard') },
  { key: 'score', label: t('entity.supplierevaluationitem.score') },
  { key: 'ratingLevel', label: t('entity.supplierevaluationitem.ratinglevel') },
  { key: 'evaluationComment', label: t('entity.supplierevaluationitem.evaluationcomment') },
  { key: 'existingIssues', label: t('entity.supplierevaluationitem.existingissues') },
  { key: 'improvementRequirement', label: t('entity.supplierevaluationitem.improvementrequirement') },
  { key: 'rectificationRequired', label: t('entity.supplierevaluationitem.rectificationrequired') },
  { key: 'rectificationDeadlineStart', label: t('entity.supplierevaluationitem.rectificationdeadlinestart') },
  { key: 'rectificationDeadlineEnd', label: t('entity.supplierevaluationitem.rectificationdeadlineend') },
  { key: 'rectificationStatus', label: t('entity.supplierevaluationitem.rectificationstatus') },
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
  supplierEvaluationCode: '',
  lineNumber: undefined as number | undefined,
  categoryType: undefined as number | undefined,
  itemName: '',
  itemDescription: '',
  weight: undefined as number | undefined,
  scoringStandard: '',
  score: undefined as number | undefined,
  ratingLevel: undefined as number | undefined,
  evaluationComment: '',
  existingIssues: '',
  improvementRequirement: '',
  rectificationRequired: undefined as number | undefined,
  rectificationDeadlineStart: '',
  rectificationDeadlineEnd: '',
  rectificationStatus: undefined as number | undefined,
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

const entityIdName = 'supplierEvaluationItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.supplierEvaluationId)
const masterSupplierEvaluationId = computed(() => selectedMasterRow.value?.supplierEvaluationId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getSupplierEvaluationItemId(record: SupplierEvaluationItem | Record<string, unknown>): string {
  return String((record as SupplierEvaluationItem)?.[entityIdName] ?? '')
}

function getSupplierEvaluationItemField(record: SupplierEvaluationItem | Record<string, unknown>, field: string): unknown {
  return (record as SupplierEvaluationItem)?.[field as keyof SupplierEvaluationItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'supplierEvaluationItemId',
    key: 'supplierEvaluationItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'supplierEvaluationItemId') ?? ''),
  },
  {
    title: t('entity.supplierevaluationitem.evaluationid'),
    dataIndex: 'evaluationId',
    key: 'evaluationId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'evaluationId') ?? ''),
  },
  {
    title: t('entity.supplierevaluationitem.supplierevaluationcode'),
    dataIndex: 'supplierEvaluationCode',
    key: 'supplierEvaluationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'supplierEvaluationCode') ?? ''),
  },
  {
    title: t('entity.supplierevaluationitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.supplierevaluationitem.categorytype'),
    dataIndex: 'categoryType',
    key: 'categoryType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'categoryType') ?? ''),
  },
  {
    title: t('entity.supplierevaluationitem.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'itemName') ?? ''),
  },
  {
    title: t('entity.supplierevaluationitem.itemdescription'),
    dataIndex: 'itemDescription',
    key: 'itemDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'itemDescription') ?? ''),
  },
  {
    title: t('entity.supplierevaluationitem.weight'),
    dataIndex: 'weight',
    key: 'weight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'weight') ?? ''),
  },
  {
    title: t('entity.supplierevaluationitem.scoringstandard'),
    dataIndex: 'scoringStandard',
    key: 'scoringStandard',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SupplierEvaluationItem }) =>
      String(getSupplierEvaluationItemField(record, 'scoringStandard') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:supplierevaluation:update',
        onClick: (record: SupplierEvaluationItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:supplierevaluation:delete',
        onClick: (record: SupplierEvaluationItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SupplierEvaluationItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SupplierEvaluationItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSupplierEvaluationItemId(selectedRow.value) === getSupplierEvaluationItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SupplierEvaluationItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SupplierEvaluationItem) {
  const key = getSupplierEvaluationItemId(record)
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
 * @returns {SupplierEvaluationItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SupplierEvaluationItemQuery>): SupplierEvaluationItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SupplierEvaluationItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    evaluationId: masterSupplierEvaluationId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SupplierEvaluationItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('supplierEvaluationCode', form.supplierEvaluationCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.categoryType !== undefined && form.categoryType !== null) {
    query.categoryType = form.categoryType
  }
  assignTrimmed('itemName', form.itemName)
  assignTrimmed('itemDescription', form.itemDescription)
  if (form.weight !== undefined && form.weight !== null) {
    query.weight = form.weight
  }
  assignTrimmed('scoringStandard', form.scoringStandard)
  if (form.score !== undefined && form.score !== null) {
    query.score = form.score
  }
  if (form.ratingLevel !== undefined && form.ratingLevel !== null) {
    query.ratingLevel = form.ratingLevel
  }
  assignTrimmed('evaluationComment', form.evaluationComment)
  assignTrimmed('existingIssues', form.existingIssues)
  assignTrimmed('improvementRequirement', form.improvementRequirement)
  if (form.rectificationRequired !== undefined && form.rectificationRequired !== null) {
    query.rectificationRequired = form.rectificationRequired
  }
  assignTrimmed('rectificationDeadlineStart', form.rectificationDeadlineStart)
  assignTrimmed('rectificationDeadlineEnd', form.rectificationDeadlineEnd)
  if (form.rectificationStatus !== undefined && form.rectificationStatus !== null) {
    query.rectificationStatus = form.rectificationStatus
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
    const res = await getSupplierEvaluationItemList(buildListQuery())
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
watch(masterSupplierEvaluationId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.supplierevaluationitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: SupplierEvaluationItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.supplierevaluationitem._self') })
  formLoading.value = true
  try {
    const detail = await getSupplierEvaluationItemById(getSupplierEvaluationItemId(record))
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
      entity: t('entity.supplierevaluationitem._self'),
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
    const id = formData.value?.supplierEvaluationItemId
    if (id) {
      await updateSupplierEvaluationItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.supplierevaluationitem._self') }))
    } else {
      await createSupplierEvaluationItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.supplierevaluationitem._self') }))
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

async function handleDeleteOne(record: SupplierEvaluationItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.supplierevaluationitem._self'),
      name: t('common.tip.this.target', { target: t('entity.supplierevaluationitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSupplierEvaluationItemById(getSupplierEvaluationItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.supplierevaluationitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.supplierevaluationitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.supplierevaluationitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getSupplierEvaluationItemId(r)).filter(Boolean)
      await deleteSupplierEvaluationItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.supplierevaluationitem._self') }))
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
  const res = await getSupplierEvaluationItemTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSupplierEvaluationItem(file, sheetName)
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
    const exportMeta = await exportSupplierEvaluationItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.supplierevaluationitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.supplierevaluationitem._self') }))
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
