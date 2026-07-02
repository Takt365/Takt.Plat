<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-item/components -->
<!-- 文件名称：bill-of-material-substitute-panel.vue -->
<!-- 功能描述：Takt物料清单明细实体主表实体右侧明细 billOfMaterialSubstitute 独立 CRUD（按主表选中 billOfMaterialItemId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="bill-of-material-substitute-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.billofmaterialsubstitute._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:bom:bill:of:material:item:create"
      update-permission="logistics:manufacturing:bom:bill:of:material:item:update"
      delete-permission="logistics:manufacturing:bom:bill:of:material:item:delete"
      import-permission="logistics:manufacturing:bom:bill:of:material:item:import"
      export-permission="logistics:manufacturing:bom:bill:of:material:item:export"
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
    <div class="bill-of-material-substitute-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getBillOfMaterialSubstituteId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="billOfMaterialSubstituteId"
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
      <BillOfMaterialSubstituteForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterBillOfMaterialItemId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-bom-bill-of-material-item-bill-of-material-substitute"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('billOfMaterialId')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.billofmaterialid')">
        <a-input
          v-model:value="advancedQueryForm.billOfMaterialId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.billofmaterialid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomCode')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.bomcode')">
        <a-input
          v-model:value="advancedQueryForm.bomCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.bomcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('primaryMaterialCode')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.primarymaterialcode')">
        <a-input
          v-model:value="advancedQueryForm.primaryMaterialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.primarymaterialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substituteMaterialId')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.substitutematerialid')">
        <a-input
          v-model:value="advancedQueryForm.substituteMaterialId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.substitutematerialid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substituteMaterialCode')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.substitutematerialcode')">
        <a-input
          v-model:value="advancedQueryForm.substituteMaterialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.substitutematerialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substituteGroup')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.substitutegroup')">
        <a-input
          v-model:value="advancedQueryForm.substituteGroup"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.substitutegroup') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substitutePriority')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.substitutepriority')">
        <a-input-number
          v-model:value="advancedQueryForm.substitutePriority"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.substitutepriority') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('usageQuantity')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.usagequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.usageQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.usagequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialUnit')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.materialunit')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialUnit"
          dict-type="logistics_unit_of_measure_code"
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.materialunit') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('usageRatio')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.usageratio')">
        <a-input-number
          v-model:value="advancedQueryForm.usageRatio"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.usageratio') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEnabled')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.isenabled')">
        <TaktSelect
          v-model:value="advancedQueryForm.isEnabled"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.isenabled') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.effectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.effectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.effectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.effectivedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateStart')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.expirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.expirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateEnd')">
      <a-form-item :label="t('entity.billofmaterialsubstitute.expirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.expirydateend') })"
          value-format="YYYY-MM-DD"
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
      :title="t('common.dialog.title.import', { entity: t('entity.billofmaterialsubstitute._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.billofmaterialsubstitute._self"
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
      id-column-key="billOfMaterialSubstituteId"
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
 * Takt物料清单明细实体子表 billOfMaterialSubstitute 右栏面板
 * @module views/logistics/manufacturing/bom/bill-of-material-item/components
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
import BillOfMaterialSubstituteForm from './bill-of-material-substitute-form.vue'
import { useBillOfMaterialItemMasterContext } from '../composables/use-bill-of-material-item-master-context'
import {
  getBillOfMaterialSubstituteList,
  getBillOfMaterialSubstituteById,
  createBillOfMaterialSubstitute,
  updateBillOfMaterialSubstitute,
  deleteBillOfMaterialSubstituteById,
  deleteBillOfMaterialSubstituteBatch,
  getBillOfMaterialSubstituteTemplate,
  importBillOfMaterialSubstitute,
  exportBillOfMaterialSubstitute,
} from '@/api/logistics/manufacturing/bom/bill-of-material-substitute'
import type { BillOfMaterialSubstitute, BillOfMaterialSubstituteQuery } from '@/types/logistics/manufacturing/bom/bill-of-material-substitute'

const { t } = useI18n()
const { selectedMasterRow } = useBillOfMaterialItemMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBillOfMaterialSubstitute')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.billofmaterialsubstitute._self') }),
)

const loading = ref(false)
const dataSource = ref<BillOfMaterialSubstitute[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<BillOfMaterialSubstitute | null>(null)
const selectedRows = ref<BillOfMaterialSubstitute[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<BillOfMaterialSubstitute>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  billOfMaterialId: '',
  bomCode: '',
  primaryMaterialCode: '',
  lineNumber: undefined as number | undefined,
  substituteMaterialId: '',
  substituteMaterialCode: '',
  substituteGroup: '',
  substitutePriority: undefined as number | undefined,
  usageQuantity: undefined as number | undefined,
  materialUnit: '',
  usageRatio: undefined as number | undefined,
  isEnabled: undefined as number | undefined,
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'billOfMaterialId', label: t('entity.billofmaterialsubstitute.billofmaterialid') },
  { key: 'bomCode', label: t('entity.billofmaterialsubstitute.bomcode') },
  { key: 'primaryMaterialCode', label: t('entity.billofmaterialsubstitute.primarymaterialcode') },
  { key: 'lineNumber', label: t('entity.billofmaterialsubstitute.linenumber') },
  { key: 'substituteMaterialId', label: t('entity.billofmaterialsubstitute.substitutematerialid') },
  { key: 'substituteMaterialCode', label: t('entity.billofmaterialsubstitute.substitutematerialcode') },
  { key: 'substituteGroup', label: t('entity.billofmaterialsubstitute.substitutegroup') },
  { key: 'substitutePriority', label: t('entity.billofmaterialsubstitute.substitutepriority') },
  { key: 'usageQuantity', label: t('entity.billofmaterialsubstitute.usagequantity') },
  { key: 'materialUnit', label: t('entity.billofmaterialsubstitute.materialunit') },
  { key: 'usageRatio', label: t('entity.billofmaterialsubstitute.usageratio') },
  { key: 'isEnabled', label: t('entity.billofmaterialsubstitute.isenabled') },
  { key: 'effectiveDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.billofmaterialsubstitute.effectivedate')) },
  { key: 'effectiveDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.billofmaterialsubstitute.effectivedate')) },
  { key: 'expiryDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.billofmaterialsubstitute.expirydate')) },
  { key: 'expiryDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.billofmaterialsubstitute.expirydate')) },
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
  billOfMaterialId: '',
  bomCode: '',
  primaryMaterialCode: '',
  lineNumber: undefined as number | undefined,
  substituteMaterialId: '',
  substituteMaterialCode: '',
  substituteGroup: '',
  substitutePriority: undefined as number | undefined,
  usageQuantity: undefined as number | undefined,
  materialUnit: '',
  usageRatio: undefined as number | undefined,
  isEnabled: undefined as number | undefined,
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
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

const entityIdName = 'billOfMaterialSubstituteId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.billOfMaterialItemId)
const masterBillOfMaterialItemId = computed(() => selectedMasterRow.value?.billOfMaterialItemId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getBillOfMaterialSubstituteId(record: BillOfMaterialSubstitute | Record<string, unknown>): string {
  return String((record as BillOfMaterialSubstitute)?.[entityIdName] ?? '')
}

function getBillOfMaterialSubstituteField(record: BillOfMaterialSubstitute | Record<string, unknown>, field: string): unknown {
  return (record as BillOfMaterialSubstitute)?.[field as keyof BillOfMaterialSubstitute]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'billOfMaterialSubstituteId',
    key: 'billOfMaterialSubstituteId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'billOfMaterialSubstituteId') ?? ''),
  },
  {
    title: t('entity.billofmaterialsubstitute.billofmaterialid'),
    dataIndex: 'billOfMaterialId',
    key: 'billOfMaterialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'billOfMaterialId') ?? ''),
  },
  {
    title: t('entity.billofmaterialsubstitute.bomcode'),
    dataIndex: 'bomCode',
    key: 'bomCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'bomCode') ?? ''),
  },
  {
    title: t('entity.billofmaterialsubstitute.primarymaterialcode'),
    dataIndex: 'primaryMaterialCode',
    key: 'primaryMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'primaryMaterialCode') ?? ''),
  },
  {
    title: t('entity.billofmaterialsubstitute.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.billofmaterialsubstitute.substitutematerialid'),
    dataIndex: 'substituteMaterialId',
    key: 'substituteMaterialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'substituteMaterialId') ?? ''),
  },
  {
    title: t('entity.billofmaterialsubstitute.substitutematerialcode'),
    dataIndex: 'substituteMaterialCode',
    key: 'substituteMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'substituteMaterialCode') ?? ''),
  },
  {
    title: t('entity.billofmaterialsubstitute.substitutegroup'),
    dataIndex: 'substituteGroup',
    key: 'substituteGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'substituteGroup') ?? ''),
  },
  {
    title: t('entity.billofmaterialsubstitute.substitutepriority'),
    dataIndex: 'substitutePriority',
    key: 'substitutePriority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'substitutePriority') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:bill:of:material:item:update',
        onClick: (record: BillOfMaterialSubstitute) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:bill:of:material:item:delete',
        onClick: (record: BillOfMaterialSubstitute) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: BillOfMaterialSubstitute[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: BillOfMaterialSubstitute, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getBillOfMaterialSubstituteId(selectedRow.value) === getBillOfMaterialSubstituteId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: BillOfMaterialSubstitute[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: BillOfMaterialSubstitute) {
  const key = getBillOfMaterialSubstituteId(record)
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
 * @returns {BillOfMaterialSubstituteQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<BillOfMaterialSubstituteQuery>): BillOfMaterialSubstituteQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: BillOfMaterialSubstituteQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    billOfMaterialItemId: masterBillOfMaterialItemId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof BillOfMaterialSubstituteQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('billOfMaterialId', form.billOfMaterialId)
  assignTrimmed('bomCode', form.bomCode)
  assignTrimmed('primaryMaterialCode', form.primaryMaterialCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('substituteMaterialId', form.substituteMaterialId)
  assignTrimmed('substituteMaterialCode', form.substituteMaterialCode)
  assignTrimmed('substituteGroup', form.substituteGroup)
  if (form.substitutePriority !== undefined && form.substitutePriority !== null) {
    query.substitutePriority = form.substitutePriority
  }
  if (form.usageQuantity !== undefined && form.usageQuantity !== null) {
    query.usageQuantity = form.usageQuantity
  }
  assignTrimmed('materialUnit', form.materialUnit)
  if (form.usageRatio !== undefined && form.usageRatio !== null) {
    query.usageRatio = form.usageRatio
  }
  if (form.isEnabled !== undefined && form.isEnabled !== null) {
    query.isEnabled = form.isEnabled
  }
  assignTrimmed('effectiveDateStart', form.effectiveDateStart)
  assignTrimmed('effectiveDateEnd', form.effectiveDateEnd)
  assignTrimmed('expiryDateStart', form.expiryDateStart)
  assignTrimmed('expiryDateEnd', form.expiryDateEnd)
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
    const res = await getBillOfMaterialSubstituteList(buildListQuery())
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
watch(masterBillOfMaterialItemId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.billofmaterialsubstitute._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: BillOfMaterialSubstitute) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.billofmaterialsubstitute._self') })
  formLoading.value = true
  try {
    const detail = await getBillOfMaterialSubstituteById(getBillOfMaterialSubstituteId(record))
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
      entity: t('entity.billofmaterialsubstitute._self'),
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
    const id = formData.value?.billOfMaterialSubstituteId
    if (id) {
      await updateBillOfMaterialSubstitute(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.billofmaterialsubstitute._self') }))
    } else {
      await createBillOfMaterialSubstitute(payload)
      message.success(t('common.feedback.created', { target: t('entity.billofmaterialsubstitute._self') }))
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

async function handleDeleteOne(record: BillOfMaterialSubstitute) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.billofmaterialsubstitute._self'),
      name: t('common.tip.this.target', { target: t('entity.billofmaterialsubstitute._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBillOfMaterialSubstituteById(getBillOfMaterialSubstituteId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.billofmaterialsubstitute._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.billofmaterialsubstitute._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.billofmaterialsubstitute._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getBillOfMaterialSubstituteId(r)).filter(Boolean)
      await deleteBillOfMaterialSubstituteBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.billofmaterialsubstitute._self') }))
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
  const res = await getBillOfMaterialSubstituteTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importBillOfMaterialSubstitute(file, sheetName)
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
    const exportMeta = await exportBillOfMaterialSubstitute(
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
    message.success(t('common.feedback.export.success', { target: t('entity.billofmaterialsubstitute._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.billofmaterialsubstitute._self') }))
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
