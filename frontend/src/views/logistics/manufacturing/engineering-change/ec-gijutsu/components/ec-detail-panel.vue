<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
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
      export-permission="logistics:manufacturing:engineering:change:gijutsu:export"
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-refresh="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :export-disabled="!hasMasterSelection"
      :export-loading="loading"
      :refresh-loading="loading"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
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
      <div v-show="isFieldVisible('ecBomItemText')">
      <a-form-item :label="t('entity.ecdetail.ecbomitemtext')">
        <a-input
          v-model:value="advancedQueryForm.ecBomItemText"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecbomitemtext') })"
          show-count
          :maxlength="40"
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
      <div v-show="isFieldVisible('ecBomSubItemText')">
      <a-form-item :label="t('entity.ecdetail.ecbomsubitemtext')">
        <a-input
          v-model:value="advancedQueryForm.ecBomSubItemText"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecbomsubitemtext') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEndOfLine')">
      <a-form-item :label="t('entity.ecdetail.isendofline')">
        <TaktSelect
          v-model:value="advancedQueryForm.isEndOfLine"
          dict-type="logistics_material_eol_status"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isendofline') })"
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
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldUsage')">
      <a-form-item :label="t('entity.ecdetail.ecoldusage')">
        <a-input-number
          v-model:value="advancedQueryForm.ecOldUsage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldusage') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldPosition')">
      <a-form-item :label="t('entity.ecdetail.ecoldposition')">
        <a-input
          v-model:value="advancedQueryForm.ecOldPosition"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecoldposition') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldStock')">
      <a-form-item :label="t('entity.ecdetail.ecoldstock')">
        <a-input-number
          v-model:value="advancedQueryForm.ecOldStock"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecoldstock') })"
          :min="0"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldWarehouse')">
      <a-form-item :label="t('entity.ecdetail.ecoldwarehouse')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecOldWarehouse"
          api-url="/api/TaktWarehouses/options"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecoldwarehouse') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isOldProcurement')">
      <a-form-item :label="t('entity.ecdetail.isoldprocurement')">
        <TaktSelect
          v-model:value="advancedQueryForm.isOldProcurement"
          dict-type="sys_yes_no"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isoldprocurement') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isOldCheck')">
      <a-form-item :label="t('entity.ecdetail.isoldcheck')">
        <TaktSelect
          v-model:value="advancedQueryForm.isOldCheck"
          dict-type="sys_yes_no"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isoldcheck') })"
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
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewUsage')">
      <a-form-item :label="t('entity.ecdetail.ecnewusage')">
        <a-input-number
          v-model:value="advancedQueryForm.ecNewUsage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewusage') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewPosition')">
      <a-form-item :label="t('entity.ecdetail.ecnewposition')">
        <a-input
          v-model:value="advancedQueryForm.ecNewPosition"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecnewposition') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewStock')">
      <a-form-item :label="t('entity.ecdetail.ecnewstock')">
        <a-input-number
          v-model:value="advancedQueryForm.ecNewStock"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecnewstock') })"
          :min="0"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewWarehouse')">
      <a-form-item :label="t('entity.ecdetail.ecnewwarehouse')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecNewWarehouse"
          api-url="/api/TaktWarehouses/options"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecnewwarehouse') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isNewProcurement')">
      <a-form-item :label="t('entity.ecdetail.isnewprocurement')">
        <TaktSelect
          v-model:value="advancedQueryForm.isNewProcurement"
          dict-type="sys_yes_no"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isnewprocurement') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isNewCheck')">
      <a-form-item :label="t('entity.ecdetail.isnewcheck')">
        <TaktSelect
          v-model:value="advancedQueryForm.isNewCheck"
          dict-type="sys_yes_no"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isnewcheck') })"
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
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import { ref, computed, watch, h } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import TaktDictTag from '@/components/common/takt-dict-tag/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
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
  ecBomItemText: '',
  ecBomSubItem: '',
  ecBomSubItemText: '',
  ecOldItem: '',
  ecOldText: '',
  ecOldUsage: undefined as number | undefined,
  ecOldPosition: '',
  ecOldStock: undefined as number | undefined,
  ecOldWarehouse: '',
  isOldProcurement: undefined as number | undefined,
  isOldCheck: undefined as number | undefined,
  ecNewItem: '',
  ecNewText: '',
  ecNewUsage: undefined as number | undefined,
  ecNewPosition: '',
  ecNewStock: undefined as number | undefined,
  ecNewWarehouse: '',
  isNewProcurement: undefined as number | undefined,
  isNewCheck: undefined as number | undefined,
  isEndOfLine: '',
  ecBomDateStart: '',
  ecBomDateEnd: '',
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
  { key: 'ecBomItemText', label: t('entity.ecdetail.ecbomitemtext') },
  { key: 'ecBomSubItem', label: t('entity.ecdetail.ecbomsubitem') },
  { key: 'ecBomSubItemText', label: t('entity.ecdetail.ecbomsubitemtext') },
  { key: 'isEndOfLine', label: t('entity.ecdetail.isendofline') },
  { key: 'ecOldItem', label: t('entity.ecdetail.ecolditem') },
  { key: 'ecOldText', label: t('entity.ecdetail.ecoldtext') },
  { key: 'ecOldUsage', label: t('entity.ecdetail.ecoldusage') },
  { key: 'ecOldPosition', label: t('entity.ecdetail.ecoldposition') },
  { key: 'ecOldStock', label: t('entity.ecdetail.ecoldstock') },
  { key: 'ecOldWarehouse', label: t('entity.ecdetail.ecoldwarehouse') },
  { key: 'isOldProcurement', label: t('entity.ecdetail.isoldprocurement') },
  { key: 'isOldCheck', label: t('entity.ecdetail.isoldcheck') },
  { key: 'ecNewItem', label: t('entity.ecdetail.ecnewitem') },
  { key: 'ecNewText', label: t('entity.ecdetail.ecnewtext') },
  { key: 'ecNewUsage', label: t('entity.ecdetail.ecnewusage') },
  { key: 'ecNewPosition', label: t('entity.ecdetail.ecnewposition') },
  { key: 'ecNewStock', label: t('entity.ecdetail.ecnewstock') },
  { key: 'ecNewWarehouse', label: t('entity.ecdetail.ecnewwarehouse') },
  { key: 'isNewProcurement', label: t('entity.ecdetail.isnewprocurement') },
  { key: 'isNewCheck', label: t('entity.ecdetail.isnewcheck') },
  { key: 'ecBomDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdetail.ecbomdate')) },
  { key: 'ecBomDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdetail.ecbomdate')) },
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
  ecBomItemText: '',
  ecBomSubItem: '',
  ecBomSubItemText: '',
  ecOldItem: '',
  ecOldText: '',
  ecOldUsage: undefined as number | undefined,
  ecOldPosition: '',
  ecOldStock: undefined as number | undefined,
  ecOldWarehouse: '',
  isOldProcurement: undefined as number | undefined,
  isOldCheck: undefined as number | undefined,
  ecNewItem: '',
  ecNewText: '',
  ecNewUsage: undefined as number | undefined,
  ecNewPosition: '',
  ecNewStock: undefined as number | undefined,
  ecNewWarehouse: '',
  isNewProcurement: undefined as number | undefined,
  isNewCheck: undefined as number | undefined,
  isEndOfLine: '',
  ecBomDateStart: '',
  ecBomDateEnd: '',
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
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.ecGijutsuId)
const masterEcId = computed(() => selectedMasterRow.value?.ecGijutsuId ?? '')
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
    title: t('entity.ecdetail.ecbomitemtext'),
    dataIndex: 'ecBomItemText',
    key: 'ecBomItemText',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecBomItemText') ?? ''),
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
    title: t('entity.ecdetail.ecbomsubitemtext'),
    dataIndex: 'ecBomSubItemText',
    key: 'ecBomSubItemText',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecBomSubItemText') ?? ''),
  },
  {
    title: t('entity.ecdetail.ecoldstock'),
    dataIndex: 'ecOldStock',
    key: 'ecOldStock',
    width: 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecOldStock') ?? ''),
  },
  {
    title: t('entity.ecdetail.ecnewstock'),
    dataIndex: 'ecNewStock',
    key: 'ecNewStock',
    width: 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDetail }) =>
      String(getEcDetailField(record, 'ecNewStock') ?? ''),
  },
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
    } else if (selectedRow.value && getEcDetailId(selectedRow.value) === getEcDetailId(record)) {
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
  assignTrimmed('ecBomItemText', form.ecBomItemText)
  assignTrimmed('ecBomSubItem', form.ecBomSubItem)
  assignTrimmed('ecBomSubItemText', form.ecBomSubItemText)
  if (form.isEndOfLine !== undefined && form.isEndOfLine !== null && String(form.isEndOfLine).trim() !== '') {
    query.isEndOfLine = Number(form.isEndOfLine)
  }
  assignTrimmed('ecOldItem', form.ecOldItem)
  assignTrimmed('ecOldText', form.ecOldText)
  if (form.ecOldUsage !== undefined && form.ecOldUsage !== null) {
    query.ecOldUsage = form.ecOldUsage
  }
  assignTrimmed('ecOldPosition', form.ecOldPosition)
  if (form.ecOldStock !== undefined && form.ecOldStock !== null) {
    query.ecOldStock = form.ecOldStock
  }
  assignTrimmed('ecOldWarehouse', form.ecOldWarehouse)
  if (form.isOldProcurement !== undefined && form.isOldProcurement !== null) {
    query.isOldProcurement = form.isOldProcurement
  }
  if (form.isOldCheck !== undefined && form.isOldCheck !== null) {
    query.isOldCheck = form.isOldCheck
  }
  assignTrimmed('ecNewItem', form.ecNewItem)
  assignTrimmed('ecNewText', form.ecNewText)
  if (form.ecNewUsage !== undefined && form.ecNewUsage !== null) {
    query.ecNewUsage = form.ecNewUsage
  }
  assignTrimmed('ecNewPosition', form.ecNewPosition)
  if (form.ecNewStock !== undefined && form.ecNewStock !== null) {
    query.ecNewStock = form.ecNewStock
  }
  assignTrimmed('ecNewWarehouse', form.ecNewWarehouse)
  if (form.isNewProcurement !== undefined && form.isNewProcurement !== null) {
    query.isNewProcurement = form.isNewProcurement
  }
  if (form.isNewCheck !== undefined && form.isNewCheck !== null) {
    query.isNewCheck = form.isNewCheck
  }
  assignTrimmed('ecBomDateStart', form.ecBomDateStart)
  assignTrimmed('ecBomDateEnd', form.ecBomDateEnd)
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
