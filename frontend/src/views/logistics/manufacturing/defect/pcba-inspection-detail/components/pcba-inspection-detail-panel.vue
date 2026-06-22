<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection-detail/components -->
<!-- 文件名称：pcba-inspection-detail-panel.vue -->
<!-- 功能描述：PCBA检查日报实体主表实体右侧明细 pcbaInspectionDetail 独立 CRUD（按主表选中 pcbaInspectionId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="pcba-inspection-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.pcbainspectiondetail._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:defect:pcbainspection:create"
      update-permission="logistics:manufacturing:defect:pcbainspection:update"
      delete-permission="logistics:manufacturing:defect:pcbainspection:delete"
      import-permission="logistics:manufacturing:defect:pcbainspection:import"
      export-permission="logistics:manufacturing:defect:pcbainspection:export"
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
    <div class="pcba-inspection-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getPcbaInspectionDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="pcbaInspectionDetailId"
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
      <PcbaInspectionDetailForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterPcbaInspectionId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-defect-pcba-inspection-detail-pcba-inspection-detail"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('prodOrderCode')">
      <a-form-item :label="t('entity.pcbainspectiondetail.prodordercode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.prodordercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.pcbainspectiondetail.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbaBoardType')">
      <a-form-item :label="t('entity.pcbainspectiondetail.pcbaboardtype')">
        <a-input
          v-model:value="advancedQueryForm.pcbaBoardType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.pcbaboardtype') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('visualInspectionLine')">
      <a-form-item :label="t('entity.pcbainspectiondetail.visualinspectionline')">
        <a-input
          v-model:value="advancedQueryForm.visualInspectionLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.visualinspectionline') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('aoiLine')">
      <a-form-item :label="t('entity.pcbainspectiondetail.aoiline')">
        <a-input
          v-model:value="advancedQueryForm.aoiLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.aoiline') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bSideAssemblyDateStart')">
      <a-form-item :label="t('entity.pcbainspectiondetail.bsideassemblydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.bSideAssemblyDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspectiondetail.bsideassemblydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bSideAssemblyDateEnd')">
      <a-form-item :label="t('entity.pcbainspectiondetail.bsideassemblydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.bSideAssemblyDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspectiondetail.bsideassemblydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tSideAssemblyDateStart')">
      <a-form-item :label="t('entity.pcbainspectiondetail.tsideassemblydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.tSideAssemblyDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspectiondetail.tsideassemblydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tSideAssemblyDateEnd')">
      <a-form-item :label="t('entity.pcbainspectiondetail.tsideassemblydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.tSideAssemblyDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbainspectiondetail.tsideassemblydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="t('entity.pcbainspectiondetail.shiftno')">
        <a-input-number
          v-model:value="advancedQueryForm.shiftNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.shiftno') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectorName')">
      <a-form-item :label="t('entity.pcbainspectiondetail.inspectorname')">
        <a-input
          v-model:value="advancedQueryForm.inspectorName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.inspectorname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dailyCompletedQty')">
      <a-form-item :label="t('entity.pcbainspectiondetail.dailycompletedqty')">
        <a-input-number
          v-model:value="advancedQueryForm.dailyCompletedQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.dailycompletedqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionQty')">
      <a-form-item :label="t('entity.pcbainspectiondetail.inspectionqty')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.inspectionqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionStatus')">
      <a-form-item :label="t('entity.pcbainspectiondetail.inspectionstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.inspectionstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodLine')">
      <a-form-item :label="t('entity.pcbainspectiondetail.prodline')">
        <a-input
          v-model:value="advancedQueryForm.prodLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.prodline') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionWorkHours')">
      <a-form-item :label="t('entity.pcbainspectiondetail.inspectionworkhours')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionWorkHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.inspectionworkhours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('aoiWorkHours')">
      <a-form-item :label="t('entity.pcbainspectiondetail.aoiworkhours')">
        <a-input-number
          v-model:value="advancedQueryForm.aoiWorkHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.aoiworkhours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectQty')">
      <a-form-item :label="t('entity.pcbainspectiondetail.defectqty')">
        <a-input-number
          v-model:value="advancedQueryForm.defectQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.defectqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handPlacement')">
      <a-form-item :label="t('entity.pcbainspectiondetail.handplacement')">
        <a-input
          v-model:value="advancedQueryForm.handPlacement"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.handplacement') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serialNumber')">
      <a-form-item :label="t('entity.pcbainspectiondetail.serialnumber')">
        <a-input
          v-model:value="advancedQueryForm.serialNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.serialnumber') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('content')">
      <a-form-item :label="t('entity.pcbainspectiondetail.content')">
        <a-textarea
          v-model:value="advancedQueryForm.content"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.pcbainspectiondetail.content') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectLocation')">
      <a-form-item :label="t('entity.pcbainspectiondetail.defectlocation')">
        <a-input
          v-model:value="advancedQueryForm.defectLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbainspectiondetail.defectlocation') })"
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
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.pcbainspectiondetail._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.pcbainspectiondetail._self"
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
      id-column-key="pcbaInspectionDetailId"
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
 * PCBA检查日报实体子表 pcbaInspectionDetail 右栏面板
 * @module views/logistics/manufacturing/defect/pcba-inspection-detail/components
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
import PcbaInspectionDetailForm from './pcba-inspection-detail-form.vue'
import { usePcbaInspectionMasterContext } from '../composables/use-pcba-inspection-master-context'
import {
  getPcbaInspectionDetailList,
  getPcbaInspectionDetailById,
  createPcbaInspectionDetail,
  updatePcbaInspectionDetail,
  deletePcbaInspectionDetailById,
  deletePcbaInspectionDetailBatch,
  getPcbaInspectionDetailTemplate,
  importPcbaInspectionDetail,
  exportPcbaInspectionDetail,
} from '@/api/logistics/manufacturing/defect/pcba-inspection-detail'
import type { PcbaInspectionDetail, PcbaInspectionDetailQuery } from '@/types/logistics/manufacturing/defect/pcba-inspection-detail'

const { t } = useI18n()
const { selectedMasterRow } = usePcbaInspectionMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPcbaInspectionDetail')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.pcbainspectiondetail._self') }),
)

const loading = ref(false)
const dataSource = ref<PcbaInspectionDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<PcbaInspectionDetail | null>(null)
const selectedRows = ref<PcbaInspectionDetail[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PcbaInspectionDetail>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  prodOrderCode: '',
  lineNumber: undefined as number | undefined,
  pcbaBoardType: '',
  visualInspectionLine: '',
  aoiLine: '',
  bSideAssemblyDateStart: '',
  bSideAssemblyDateEnd: '',
  tSideAssemblyDateStart: '',
  tSideAssemblyDateEnd: '',
  shiftNo: undefined as number | undefined,
  inspectorName: '',
  dailyCompletedQty: undefined as number | undefined,
  inspectionQty: undefined as number | undefined,
  inspectionStatus: undefined as number | undefined,
  prodLine: '',
  inspectionWorkHours: undefined as number | undefined,
  aoiWorkHours: undefined as number | undefined,
  defectQty: undefined as number | undefined,
  handPlacement: '',
  serialNumber: '',
  content: '',
  defectLocation: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'prodOrderCode', label: t('entity.pcbainspectiondetail.prodordercode') },
  { key: 'lineNumber', label: t('entity.pcbainspectiondetail.linenumber') },
  { key: 'pcbaBoardType', label: t('entity.pcbainspectiondetail.pcbaboardtype') },
  { key: 'visualInspectionLine', label: t('entity.pcbainspectiondetail.visualinspectionline') },
  { key: 'aoiLine', label: t('entity.pcbainspectiondetail.aoiline') },
  { key: 'bSideAssemblyDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.pcbainspectiondetail.bsideassemblydate')) },
  { key: 'bSideAssemblyDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.pcbainspectiondetail.bsideassemblydate')) },
  { key: 'tSideAssemblyDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.pcbainspectiondetail.tsideassemblydate')) },
  { key: 'tSideAssemblyDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.pcbainspectiondetail.tsideassemblydate')) },
  { key: 'shiftNo', label: t('entity.pcbainspectiondetail.shiftno') },
  { key: 'inspectorName', label: t('entity.pcbainspectiondetail.inspectorname') },
  { key: 'dailyCompletedQty', label: t('entity.pcbainspectiondetail.dailycompletedqty') },
  { key: 'inspectionQty', label: t('entity.pcbainspectiondetail.inspectionqty') },
  { key: 'inspectionStatus', label: t('entity.pcbainspectiondetail.inspectionstatus') },
  { key: 'prodLine', label: t('entity.pcbainspectiondetail.prodline') },
  { key: 'inspectionWorkHours', label: t('entity.pcbainspectiondetail.inspectionworkhours') },
  { key: 'aoiWorkHours', label: t('entity.pcbainspectiondetail.aoiworkhours') },
  { key: 'defectQty', label: t('entity.pcbainspectiondetail.defectqty') },
  { key: 'handPlacement', label: t('entity.pcbainspectiondetail.handplacement') },
  { key: 'serialNumber', label: t('entity.pcbainspectiondetail.serialnumber') },
  { key: 'content', label: t('entity.pcbainspectiondetail.content') },
  { key: 'defectLocation', label: t('entity.pcbainspectiondetail.defectlocation') },
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
  prodOrderCode: '',
  lineNumber: undefined as number | undefined,
  pcbaBoardType: '',
  visualInspectionLine: '',
  aoiLine: '',
  bSideAssemblyDateStart: '',
  bSideAssemblyDateEnd: '',
  tSideAssemblyDateStart: '',
  tSideAssemblyDateEnd: '',
  shiftNo: undefined as number | undefined,
  inspectorName: '',
  dailyCompletedQty: undefined as number | undefined,
  inspectionQty: undefined as number | undefined,
  inspectionStatus: undefined as number | undefined,
  prodLine: '',
  inspectionWorkHours: undefined as number | undefined,
  aoiWorkHours: undefined as number | undefined,
  defectQty: undefined as number | undefined,
  handPlacement: '',
  serialNumber: '',
  content: '',
  defectLocation: '',
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

const entityIdName = 'pcbaInspectionDetailId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.pcbaInspectionId)
const masterPcbaInspectionId = computed(() => selectedMasterRow.value?.pcbaInspectionId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getPcbaInspectionDetailId(record: PcbaInspectionDetail | Record<string, unknown>): string {
  return String((record as PcbaInspectionDetail)?.[entityIdName] ?? '')
}

function getPcbaInspectionDetailField(record: PcbaInspectionDetail | Record<string, unknown>, field: string): unknown {
  return (record as PcbaInspectionDetail)?.[field as keyof PcbaInspectionDetail]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'pcbaInspectionDetailId',
    key: 'pcbaInspectionDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: PcbaInspectionDetail }) =>
      String(getPcbaInspectionDetailField(record, 'pcbaInspectionDetailId') ?? ''),
  },
  {
    title: t('entity.pcbainspectiondetail.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaInspectionDetail }) =>
      String(getPcbaInspectionDetailField(record, 'prodOrderCode') ?? ''),
  },
  {
    title: t('entity.pcbainspectiondetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaInspectionDetail }) =>
      String(getPcbaInspectionDetailField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.pcbainspectiondetail.pcbaboardtype'),
    dataIndex: 'pcbaBoardType',
    key: 'pcbaBoardType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaInspectionDetail }) =>
      String(getPcbaInspectionDetailField(record, 'pcbaBoardType') ?? ''),
  },
  {
    title: t('entity.pcbainspectiondetail.visualinspectionline'),
    dataIndex: 'visualInspectionLine',
    key: 'visualInspectionLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaInspectionDetail }) =>
      String(getPcbaInspectionDetailField(record, 'visualInspectionLine') ?? ''),
  },
  {
    title: t('entity.pcbainspectiondetail.aoiline'),
    dataIndex: 'aoiLine',
    key: 'aoiLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaInspectionDetail }) =>
      String(getPcbaInspectionDetailField(record, 'aoiLine') ?? ''),
  },
  {
    title: t('entity.pcbainspectiondetail.bsideassemblydate'),
    dataIndex: 'bSideAssemblyDate',
    key: 'bSideAssemblyDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaInspectionDetail }) =>
      String(getPcbaInspectionDetailField(record, 'bSideAssemblyDate') ?? ''),
  },
  {
    title: t('entity.pcbainspectiondetail.tsideassemblydate'),
    dataIndex: 'tSideAssemblyDate',
    key: 'tSideAssemblyDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaInspectionDetail }) =>
      String(getPcbaInspectionDetailField(record, 'tSideAssemblyDate') ?? ''),
  },
  {
    title: t('entity.pcbainspectiondetail.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaInspectionDetail }) =>
      String(getPcbaInspectionDetailField(record, 'shiftNo') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:defect:pcbainspection:update',
        onClick: (record: PcbaInspectionDetail) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:defect:pcbainspection:delete',
        onClick: (record: PcbaInspectionDetail) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PcbaInspectionDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PcbaInspectionDetail, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPcbaInspectionDetailId(selectedRow.value) === getPcbaInspectionDetailId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PcbaInspectionDetail[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: PcbaInspectionDetail) {
  const key = getPcbaInspectionDetailId(record)
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
 * @returns {PcbaInspectionDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PcbaInspectionDetailQuery>): PcbaInspectionDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PcbaInspectionDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    pcbaInspectionId: masterPcbaInspectionId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PcbaInspectionDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('prodOrderCode', form.prodOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('pcbaBoardType', form.pcbaBoardType)
  assignTrimmed('visualInspectionLine', form.visualInspectionLine)
  assignTrimmed('aoiLine', form.aoiLine)
  assignTrimmed('bSideAssemblyDateStart', form.bSideAssemblyDateStart)
  assignTrimmed('bSideAssemblyDateEnd', form.bSideAssemblyDateEnd)
  assignTrimmed('tSideAssemblyDateStart', form.tSideAssemblyDateStart)
  assignTrimmed('tSideAssemblyDateEnd', form.tSideAssemblyDateEnd)
  if (form.shiftNo !== undefined && form.shiftNo !== null) {
    query.shiftNo = form.shiftNo
  }
  assignTrimmed('inspectorName', form.inspectorName)
  if (form.dailyCompletedQty !== undefined && form.dailyCompletedQty !== null) {
    query.dailyCompletedQty = form.dailyCompletedQty
  }
  if (form.inspectionQty !== undefined && form.inspectionQty !== null) {
    query.inspectionQty = form.inspectionQty
  }
  if (form.inspectionStatus !== undefined && form.inspectionStatus !== null) {
    query.inspectionStatus = form.inspectionStatus
  }
  assignTrimmed('prodLine', form.prodLine)
  if (form.inspectionWorkHours !== undefined && form.inspectionWorkHours !== null) {
    query.inspectionWorkHours = form.inspectionWorkHours
  }
  if (form.aoiWorkHours !== undefined && form.aoiWorkHours !== null) {
    query.aoiWorkHours = form.aoiWorkHours
  }
  if (form.defectQty !== undefined && form.defectQty !== null) {
    query.defectQty = form.defectQty
  }
  assignTrimmed('handPlacement', form.handPlacement)
  assignTrimmed('serialNumber', form.serialNumber)
  assignTrimmed('content', form.content)
  assignTrimmed('defectLocation', form.defectLocation)
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
    const res = await getPcbaInspectionDetailList(buildListQuery())
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
watch(masterPcbaInspectionId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.pcbainspectiondetail._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: PcbaInspectionDetail) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.pcbainspectiondetail._self') })
  formLoading.value = true
  try {
    const detail = await getPcbaInspectionDetailById(getPcbaInspectionDetailId(record))
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
      entity: t('entity.pcbainspectiondetail._self'),
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
    const id = formData.value?.pcbaInspectionDetailId
    if (id) {
      await updatePcbaInspectionDetail(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.pcbainspectiondetail._self') }))
    } else {
      await createPcbaInspectionDetail(payload)
      message.success(t('common.feedback.created', { target: t('entity.pcbainspectiondetail._self') }))
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

async function handleDeleteOne(record: PcbaInspectionDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.pcbainspectiondetail._self'),
      name: t('common.tip.this.target', { target: t('entity.pcbainspectiondetail._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePcbaInspectionDetailById(getPcbaInspectionDetailId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.pcbainspectiondetail._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.pcbainspectiondetail._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.pcbainspectiondetail._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getPcbaInspectionDetailId(r)).filter(Boolean)
      await deletePcbaInspectionDetailBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.pcbainspectiondetail._self') }))
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
  const res = await getPcbaInspectionDetailTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPcbaInspectionDetail(file, sheetName)
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
    const exportMeta = await exportPcbaInspectionDetail(
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
    message.success(t('common.feedback.export.success', { target: t('entity.pcbainspectiondetail._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.pcbainspectiondetail._self') }))
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
