<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec/components -->
<!-- 文件名称：ec-detail-panel.vue -->
<!-- 功能描述：设变主表实体右侧明细 ecDetail 独立 CRUD（按主表选中 ecId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="ec-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.ecdetail._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:engineering:change:ec:create"
      update-permission="logistics:manufacturing:engineering:change:ec:update"
      delete-permission="logistics:manufacturing:engineering:change:ec:delete"
      import-permission="logistics:manufacturing:engineering:change:ec:import"
      export-permission="logistics:manufacturing:engineering:change:ec:export"
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
    <div class="ec-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getEcDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="ecDetailId"
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
      <EcDetailForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterEcId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-ec-ec-detail"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ecNo')">
      <a-form-item :label="t('entity.ecdetail.ecno')">
        <a-input
          v-model:value="advancedQueryForm.ecNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecno') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.ecdetail.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecModel')">
      <a-form-item :label="t('entity.ecdetail.ecmodel')">
        <a-input
          v-model:value="advancedQueryForm.ecModel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecmodel') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomItem')">
      <a-form-item :label="t('entity.ecdetail.ecbomitem')">
        <a-input
          v-model:value="advancedQueryForm.ecBomItem"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomitem') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomSubItem')">
      <a-form-item :label="t('entity.ecdetail.ecbomsubitem')">
        <a-input
          v-model:value="advancedQueryForm.ecBomSubItem"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomsubitem') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomNo')">
      <a-form-item :label="t('entity.ecdetail.ecbomno')">
        <a-input
          v-model:value="advancedQueryForm.ecBomNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecChange')">
      <a-form-item :label="t('entity.ecdetail.ecchange')">
        <a-input
          v-model:value="advancedQueryForm.ecChange"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecchange') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecLocal')">
      <a-form-item :label="t('entity.ecdetail.eclocal')">
        <a-input
          v-model:value="advancedQueryForm.ecLocal"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.eclocal') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNote')">
      <a-form-item :label="t('entity.ecdetail.ecnote')">
        <a-textarea
          v-model:value="advancedQueryForm.ecNote"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecnote') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecProcess')">
      <a-form-item :label="t('entity.ecdetail.ecprocess')">
        <a-input
          v-model:value="advancedQueryForm.ecProcess"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecprocess') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomDateStart')">
      <a-form-item :label="t('entity.ecdetail.ecbomdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecBomDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecbomdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomDateEnd')">
      <a-form-item :label="t('entity.ecdetail.ecbomdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecBomDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecbomdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecEntryDateStart')">
      <a-form-item :label="t('entity.ecdetail.ecentrydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecEntryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecentrydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecEntryDateEnd')">
      <a-form-item :label="t('entity.ecdetail.ecentrydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecEntryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecentrydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldItem')">
      <a-form-item :label="t('entity.ecdetail.ecolditem')">
        <a-input
          v-model:value="advancedQueryForm.ecOldItem"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecolditem') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldText')">
      <a-form-item :label="t('entity.ecdetail.ecoldtext')">
        <a-input
          v-model:value="advancedQueryForm.ecOldText"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldtext') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldQty')">
      <a-form-item :label="t('entity.ecdetail.ecoldqty')">
        <a-input-number
          v-model:value="advancedQueryForm.ecOldQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldSet')">
      <a-form-item :label="t('entity.ecdetail.ecoldset')">
        <a-input
          v-model:value="advancedQueryForm.ecOldSet"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldset') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewItem')">
      <a-form-item :label="t('entity.ecdetail.ecnewitem')">
        <a-input
          v-model:value="advancedQueryForm.ecNewItem"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewitem') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewText')">
      <a-form-item :label="t('entity.ecdetail.ecnewtext')">
        <a-input
          v-model:value="advancedQueryForm.ecNewText"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewtext') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewQty')">
      <a-form-item :label="t('entity.ecdetail.ecnewqty')">
        <a-input-number
          v-model:value="advancedQueryForm.ecNewQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewSet')">
      <a-form-item :label="t('entity.ecdetail.ecnewset')">
        <a-input
          v-model:value="advancedQueryForm.ecNewSet"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewset') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isProcurement')">
      <a-form-item :label="t('entity.ecdetail.isprocurement')">
        <a-input-number
          v-model:value="advancedQueryForm.isProcurement"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.isprocurement') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCheck')">
      <a-form-item :label="t('entity.ecdetail.ischeck')">
        <a-input-number
          v-model:value="advancedQueryForm.isCheck"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ischeck') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecWarehouse')">
      <a-form-item :label="t('entity.ecdetail.ecwarehouse')">
        <a-input
          v-model:value="advancedQueryForm.ecWarehouse"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecwarehouse') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEndOfLine')">
      <a-form-item :label="t('entity.ecdetail.isendofline')">
        <a-input-number
          v-model:value="advancedQueryForm.isEndOfLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.isendofline') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.ecdetail._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ecdetail._self"
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
      id-column-key="ecDetailId"
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
 * 设变子表 ecDetail 右栏面板
 * @module views/logistics/manufacturing/engineering-change/ec/components
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
import EcDetailForm from './ec-detail-form.vue'
import { useEcMasterContext } from '../composables/use-ec-master-context'
import {
  getEcDetailList,
  getEcDetailById,
  createEcDetail,
  updateEcDetail,
  deleteEcDetailById,
  deleteEcDetailBatch,
  getEcDetailTemplate,
  importEcDetail,
  exportEcDetail,
} from '@/api/logistics/manufacturing/engineering-change/ec-detail'
import type { EcDetail, EcDetailQuery } from '@/types/logistics/manufacturing/engineering-change/ec-detail'

const { t } = useI18n()
const { selectedMasterRow } = useEcMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEcDetail')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ecdetail._self') }),
)

const loading = ref(false)
const dataSource = ref<EcDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<EcDetail | null>(null)
const selectedRows = ref<EcDetail[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<EcDetail>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  ecNo: '',
  lineNumber: undefined as number | undefined,
  ecModel: '',
  ecBomItem: '',
  ecBomSubItem: '',
  ecBomNo: '',
  ecChange: '',
  ecLocal: '',
  ecNote: '',
  ecProcess: '',
  ecBomDateStart: '',
  ecBomDateEnd: '',
  ecEntryDateStart: '',
  ecEntryDateEnd: '',
  ecOldItem: '',
  ecOldText: '',
  ecOldQty: undefined as number | undefined,
  ecOldSet: '',
  ecNewItem: '',
  ecNewText: '',
  ecNewQty: undefined as number | undefined,
  ecNewSet: '',
  isProcurement: undefined as number | undefined,
  isCheck: undefined as number | undefined,
  ecWarehouse: '',
  isEndOfLine: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'ecNo', label: t('entity.ecdetail.ecno') },
  { key: 'lineNumber', label: t('entity.ecdetail.linenumber') },
  { key: 'ecModel', label: t('entity.ecdetail.ecmodel') },
  { key: 'ecBomItem', label: t('entity.ecdetail.ecbomitem') },
  { key: 'ecBomSubItem', label: t('entity.ecdetail.ecbomsubitem') },
  { key: 'ecBomNo', label: t('entity.ecdetail.ecbomno') },
  { key: 'ecChange', label: t('entity.ecdetail.ecchange') },
  { key: 'ecLocal', label: t('entity.ecdetail.eclocal') },
  { key: 'ecNote', label: t('entity.ecdetail.ecnote') },
  { key: 'ecProcess', label: t('entity.ecdetail.ecprocess') },
  { key: 'ecBomDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdetail.ecbomdate')) },
  { key: 'ecBomDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdetail.ecbomdate')) },
  { key: 'ecEntryDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdetail.ecentrydate')) },
  { key: 'ecEntryDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdetail.ecentrydate')) },
  { key: 'ecOldItem', label: t('entity.ecdetail.ecolditem') },
  { key: 'ecOldText', label: t('entity.ecdetail.ecoldtext') },
  { key: 'ecOldQty', label: t('entity.ecdetail.ecoldqty') },
  { key: 'ecOldSet', label: t('entity.ecdetail.ecoldset') },
  { key: 'ecNewItem', label: t('entity.ecdetail.ecnewitem') },
  { key: 'ecNewText', label: t('entity.ecdetail.ecnewtext') },
  { key: 'ecNewQty', label: t('entity.ecdetail.ecnewqty') },
  { key: 'ecNewSet', label: t('entity.ecdetail.ecnewset') },
  { key: 'isProcurement', label: t('entity.ecdetail.isprocurement') },
  { key: 'isCheck', label: t('entity.ecdetail.ischeck') },
  { key: 'ecWarehouse', label: t('entity.ecdetail.ecwarehouse') },
  { key: 'isEndOfLine', label: t('entity.ecdetail.isendofline') },
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
  ecNo: '',
  lineNumber: undefined as number | undefined,
  ecModel: '',
  ecBomItem: '',
  ecBomSubItem: '',
  ecBomNo: '',
  ecChange: '',
  ecLocal: '',
  ecNote: '',
  ecProcess: '',
  ecBomDateStart: '',
  ecBomDateEnd: '',
  ecEntryDateStart: '',
  ecEntryDateEnd: '',
  ecOldItem: '',
  ecOldText: '',
  ecOldQty: undefined as number | undefined,
  ecOldSet: '',
  ecNewItem: '',
  ecNewText: '',
  ecNewQty: undefined as number | undefined,
  ecNewSet: '',
  isProcurement: undefined as number | undefined,
  isCheck: undefined as number | undefined,
  ecWarehouse: '',
  isEndOfLine: undefined as number | undefined,
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

const entityIdName = 'ecDetailId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.ecId)
const masterEcId = computed(() => selectedMasterRow.value?.ecId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getEcDetailId(record: EcDetail | Record<string, unknown>): string {
  return String((record as EcDetail)?.[entityIdName] ?? '')
}

function getEcDetailField(record: EcDetail | Record<string, unknown>, field: string): unknown {
  return (record as EcDetail)?.[field as keyof EcDetail]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ecDetailId',
    key: 'ecDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecDetailId') ?? ''),
  },
  {
    title: t('entity.ecdetail.ecno'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecNo') ?? ''),
  },
  {
    title: t('entity.ecdetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.ecdetail.ecmodel'),
    dataIndex: 'ecModel',
    key: 'ecModel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecModel') ?? ''),
  },
  {
    title: t('entity.ecdetail.ecbomitem'),
    dataIndex: 'ecBomItem',
    key: 'ecBomItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecBomItem') ?? ''),
  },
  {
    title: t('entity.ecdetail.ecbomsubitem'),
    dataIndex: 'ecBomSubItem',
    key: 'ecBomSubItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecBomSubItem') ?? ''),
  },
  {
    title: t('entity.ecdetail.ecbomno'),
    dataIndex: 'ecBomNo',
    key: 'ecBomNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecBomNo') ?? ''),
  },
  {
    title: t('entity.ecdetail.ecchange'),
    dataIndex: 'ecChange',
    key: 'ecChange',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecChange') ?? ''),
  },
  {
    title: t('entity.ecdetail.eclocal'),
    dataIndex: 'ecLocal',
    key: 'ecLocal',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecLocal') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineering:change:ec:update',
        onClick: (record: EcDetail) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:engineering:change:ec:delete',
        onClick: (record: EcDetail) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EcDetail, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEcDetailId(selectedRow.value) === getEcDetailId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EcDetail[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: EcDetail) {
  const key = getEcDetailId(record)
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
 * @returns {EcDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EcDetailQuery>): EcDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EcDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ecId: masterEcId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EcDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ecNo', form.ecNo)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('ecModel', form.ecModel)
  assignTrimmed('ecBomItem', form.ecBomItem)
  assignTrimmed('ecBomSubItem', form.ecBomSubItem)
  assignTrimmed('ecBomNo', form.ecBomNo)
  assignTrimmed('ecChange', form.ecChange)
  assignTrimmed('ecLocal', form.ecLocal)
  assignTrimmed('ecNote', form.ecNote)
  assignTrimmed('ecProcess', form.ecProcess)
  assignTrimmed('ecBomDateStart', form.ecBomDateStart)
  assignTrimmed('ecBomDateEnd', form.ecBomDateEnd)
  assignTrimmed('ecEntryDateStart', form.ecEntryDateStart)
  assignTrimmed('ecEntryDateEnd', form.ecEntryDateEnd)
  assignTrimmed('ecOldItem', form.ecOldItem)
  assignTrimmed('ecOldText', form.ecOldText)
  if (form.ecOldQty !== undefined && form.ecOldQty !== null) {
    query.ecOldQty = form.ecOldQty
  }
  assignTrimmed('ecOldSet', form.ecOldSet)
  assignTrimmed('ecNewItem', form.ecNewItem)
  assignTrimmed('ecNewText', form.ecNewText)
  if (form.ecNewQty !== undefined && form.ecNewQty !== null) {
    query.ecNewQty = form.ecNewQty
  }
  assignTrimmed('ecNewSet', form.ecNewSet)
  if (form.isProcurement !== undefined && form.isProcurement !== null) {
    query.isProcurement = form.isProcurement
  }
  if (form.isCheck !== undefined && form.isCheck !== null) {
    query.isCheck = form.isCheck
  }
  assignTrimmed('ecWarehouse', form.ecWarehouse)
  if (form.isEndOfLine !== undefined && form.isEndOfLine !== null) {
    query.isEndOfLine = form.isEndOfLine
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
    const res = await getEcDetailList(buildListQuery())
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
watch(masterEcId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ecdetail._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: EcDetail) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ecdetail._self') })
  formLoading.value = true
  try {
    const detail = await getEcDetailById(getEcDetailId(record))
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
      entity: t('entity.ecdetail._self'),
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
    const id = formData.value?.ecDetailId
    if (id) {
      await updateEcDetail(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.ecdetail._self') }))
    } else {
      await createEcDetail(payload)
      message.success(t('common.feedback.created', { target: t('entity.ecdetail._self') }))
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

async function handleDeleteOne(record: EcDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.ecdetail._self'),
      name: t('common.tip.this.target', { target: t('entity.ecdetail._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEcDetailById(getEcDetailId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.ecdetail._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.ecdetail._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.ecdetail._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getEcDetailId(r)).filter(Boolean)
      await deleteEcDetailBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ecdetail._self') }))
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
  const res = await getEcDetailTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEcDetail(file, sheetName)
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
    const exportMeta = await exportEcDetail(
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
    message.success(t('common.feedback.export.success', { target: t('entity.ecdetail._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.ecdetail._self') }))
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
