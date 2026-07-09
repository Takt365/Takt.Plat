<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec/components -->
<!-- 文件名称：source-ec-detail-panel.vue -->
<!-- 功能描述：设变来源主表实体主表实体右侧明细 sourceEcDetail 独立 CRUD（按主表选中 sourceEcId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="source-ec-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.sourceecdetail._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      import-permission="logistics:manufacturing:engineering:change:source:ec:import"
      export-permission="logistics:manufacturing:engineering:change:source:ec:export"
      :show-create="false"
      :show-update="false"
      :show-delete="false"
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
      :refresh-loading="loading"
      @refresh="handleRefresh"
    />
    <div class="source-ec-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getSourceEcDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="sourceEcDetailId"
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
      v-model:open="detailVisible"
      :title="t('common.dialog.title.detail', { entity: t('entity.sourceecdetail._self') })"
      width="800px"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleDetailClose"
    >
      <a-spin :spinning="detailLoading">
        <SourceEcDetailForm
          :form-data="detailData"
          :master-id="masterSourceEcId"
          :loading="detailLoading"
          read-only
        />
      </a-spin>
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-source-ec-source-ec-detail"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('sourceFinishedProduct')">
      <a-form-item :label="t('entity.sourceecdetail.sourcefinishedproduct')">
        <a-input
          v-model:value="advancedQueryForm.sourceFinishedProduct"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcefinishedproduct') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceParentPart')">
      <a-form-item :label="t('entity.sourceecdetail.sourceparentpart')">
        <a-input
          v-model:value="advancedQueryForm.sourceParentPart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourceparentpart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceLegacyPartNo')">
      <a-form-item :label="t('entity.sourceecdetail.sourcelegacypartno')">
        <a-input
          v-model:value="advancedQueryForm.sourceLegacyPartNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcelegacypartno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceLegacyPartName')">
      <a-form-item :label="t('entity.sourceecdetail.sourcelegacypartname')">
        <a-input
          v-model:value="advancedQueryForm.sourceLegacyPartName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcelegacypartname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceLegacyUsage')">
      <a-form-item :label="t('entity.sourceecdetail.sourcelegacyusage')">
        <a-input-number
          v-model:value="advancedQueryForm.sourceLegacyUsage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcelegacyusage') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceLegacyMountingPosition')">
      <a-form-item :label="t('entity.sourceecdetail.sourcelegacymountingposition')">
        <a-input
          v-model:value="advancedQueryForm.sourceLegacyMountingPosition"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcelegacymountingposition') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceReplacementPartNo')">
      <a-form-item :label="t('entity.sourceecdetail.sourcereplacementpartno')">
        <a-input
          v-model:value="advancedQueryForm.sourceReplacementPartNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcereplacementpartno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceReplacementPartName')">
      <a-form-item :label="t('entity.sourceecdetail.sourcereplacementpartname')">
        <a-input
          v-model:value="advancedQueryForm.sourceReplacementPartName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcereplacementpartname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceReplacementUsage')">
      <a-form-item :label="t('entity.sourceecdetail.sourcereplacementusage')">
        <a-input-number
          v-model:value="advancedQueryForm.sourceReplacementUsage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcereplacementusage') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceReplacementMountingPosition')">
      <a-form-item :label="t('entity.sourceecdetail.sourcereplacementmountingposition')">
        <a-input
          v-model:value="advancedQueryForm.sourceReplacementMountingPosition"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcereplacementmountingposition') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceBomNo')">
      <a-form-item :label="t('entity.sourceecdetail.sourcebomno')">
        <a-input
          v-model:value="advancedQueryForm.sourceBomNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceecdetail.sourcebomno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('SourceCompatibility')">
      <a-form-item :label="t('entity.sourceecdetail.SourceCompatibility')">
        <TaktSelect
          v-model:value="advancedQueryForm.SourceCompatibility"
          dict-type="logistics_ec_source_compatibility"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceecdetail.SourceCompatibility') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceDistinction')">
      <a-form-item :label="t('entity.sourceecdetail.sourcedistinction')">
        <TaktSelect
          v-model:value="advancedQueryForm.sourceDistinction"
          dict-type="logistics_ec_source_distinction"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceecdetail.sourcedistinction') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('SourceInstruction')">
      <a-form-item :label="t('entity.sourceecdetail.SourceInstruction')">
        <TaktSelect
          v-model:value="advancedQueryForm.SourceInstruction"
          dict-type="logistics_ec_source_instruction"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceecdetail.SourceInstruction') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceLegacyPartDisposition')">
      <a-form-item :label="t('entity.sourceecdetail.sourcelegacypartdisposition')">
        <TaktSelect
          v-model:value="advancedQueryForm.sourceLegacyPartDisposition"
          dict-type="logistics_ec_legacy_part_disposition"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceecdetail.sourcelegacypartdisposition') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceBomEffectiveDateStart')">
      <a-form-item :label="t('entity.sourceecdetail.sourcebomeffectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.sourceBomEffectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceecdetail.sourcebomeffectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceBomEffectiveDateEnd')">
      <a-form-item :label="t('entity.sourceecdetail.sourcebomeffectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.sourceBomEffectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceecdetail.sourcebomeffectivedateend') })"
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
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.sourceecdetail._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.sourceecdetail._self"
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
      id-column-key="sourceEcDetailId"
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
 * 设变来源主表实体子表 sourceEcDetail 右栏面板
 * @module views/logistics/manufacturing/engineering-change/source-ec/components
 */
import { ref, computed, watch } from 'vue'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEyeLine, RiQuestionLine } from '@remixicon/vue'
import SourceEcDetailForm from './source-ec-detail-form.vue'
import { useSourceEcMasterContext } from '../composables/use-source-ec-master-context'
import {
  SOURCE_EC_DETAIL_ID_COLUMN_KEY,
  buildSourceEcDetailDefaultVisibleColumnKeys,
  buildSourceEcDetailListBusinessColumns,
} from '../composables/use-source-ec-detail-fields'
import {
  getSourceEcDetailList,
  getSourceEcDetailById,
  getSourceEcDetailTemplate,
  importSourceEcDetail,
  exportSourceEcDetail,
} from '@/api/logistics/manufacturing/engineering-change/source-ec-detail'
import type { SourceEcDetail, SourceEcDetailQuery } from '@/types/logistics/manufacturing/engineering-change/source-ec-detail'

const { t } = useI18n()
const { selectedMasterRow } = useSourceEcMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSourceEcDetail')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.sourceecdetail._self') }),
)

const loading = ref(false)
const dataSource = ref<SourceEcDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<SourceEcDetail | null>(null)
const selectedRows = ref<SourceEcDetail[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const detailVisible = ref(false)
const detailLoading = ref(false)
const detailData = ref<Partial<SourceEcDetail>>({})

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  sourceFinishedProduct: '',
  sourceParentPart: '',
  sourceLegacyPartNo: '',
  sourceLegacyPartName: '',
  sourceLegacyUsage: undefined as number | undefined,
  sourceLegacyMountingPosition: '',
  sourceReplacementPartNo: '',
  sourceReplacementPartName: '',
  sourceReplacementUsage: undefined as number | undefined,
  sourceReplacementMountingPosition: '',
  sourceBomNo: '',
  SourceCompatibility: '',
  sourceDistinction: '',
  SourceInstruction: '',
  sourceLegacyPartDisposition: '',
  sourceBomEffectiveDateStart: '',
  sourceBomEffectiveDateEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'sourceFinishedProduct', label: t('entity.sourceecdetail.sourcefinishedproduct') },
  { key: 'sourceParentPart', label: t('entity.sourceecdetail.sourceparentpart') },
  { key: 'sourceLegacyPartNo', label: t('entity.sourceecdetail.sourcelegacypartno') },
  { key: 'sourceLegacyPartName', label: t('entity.sourceecdetail.sourcelegacypartname') },
  { key: 'sourceLegacyUsage', label: t('entity.sourceecdetail.sourcelegacyusage') },
  { key: 'sourceLegacyMountingPosition', label: t('entity.sourceecdetail.sourcelegacymountingposition') },
  { key: 'sourceReplacementPartNo', label: t('entity.sourceecdetail.sourcereplacementpartno') },
  { key: 'sourceReplacementPartName', label: t('entity.sourceecdetail.sourcereplacementpartname') },
  { key: 'sourceReplacementUsage', label: t('entity.sourceecdetail.sourcereplacementusage') },
  { key: 'sourceReplacementMountingPosition', label: t('entity.sourceecdetail.sourcereplacementmountingposition') },
  { key: 'sourceBomNo', label: t('entity.sourceecdetail.sourcebomno') },
  { key: 'SourceCompatibility', label: t('entity.sourceecdetail.SourceCompatibility') },
  { key: 'sourceDistinction', label: t('entity.sourceecdetail.sourcedistinction') },
  { key: 'SourceInstruction', label: t('entity.sourceecdetail.SourceInstruction') },
  { key: 'sourceLegacyPartDisposition', label: t('entity.sourceecdetail.sourcelegacypartdisposition') },
  { key: 'sourceBomEffectiveDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.sourceecdetail.sourcebomeffectivedate')) },
  { key: 'sourceBomEffectiveDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.sourceecdetail.sourcebomeffectivedate')) },
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
  sourceFinishedProduct: '',
  sourceParentPart: '',
  sourceLegacyPartNo: '',
  sourceLegacyPartName: '',
  sourceLegacyUsage: undefined as number | undefined,
  sourceLegacyMountingPosition: '',
  sourceReplacementPartNo: '',
  sourceReplacementPartName: '',
  sourceReplacementUsage: undefined as number | undefined,
  sourceReplacementMountingPosition: '',
  sourceBomNo: '',
  SourceCompatibility: '',
  sourceDistinction: '',
  SourceInstruction: '',
  sourceLegacyPartDisposition: '',
  sourceBomEffectiveDateStart: '',
  sourceBomEffectiveDateEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
/** 默认展示全部 16 个业务列（避免 TaktSingleTable 空 keys 时仅显示 8 列） */
const visibleColumnKeys = ref<string[]>(buildSourceEcDetailDefaultVisibleColumnKeys())

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = buildSourceEcDetailDefaultVisibleColumnKeys()
}
const importVisible = ref(false)

const entityIdName = 'sourceEcDetailId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.sourceEcId)
const masterSourceEcId = computed(() => selectedMasterRow.value?.sourceEcId ?? '')
function getSourceEcDetailId(record: SourceEcDetail | Record<string, unknown>): string {
  return String((record as SourceEcDetail)?.[entityIdName] ?? '')
}

function getSourceEcDetailField(record: SourceEcDetail | Record<string, unknown>, field: string): unknown {
  return (record as SourceEcDetail)?.[field as keyof SourceEcDetail]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: SOURCE_EC_DETAIL_ID_COLUMN_KEY,
    key: SOURCE_EC_DETAIL_ID_COLUMN_KEY,
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SourceEcDetail }) =>
      String(getSourceEcDetailField(record, SOURCE_EC_DETAIL_ID_COLUMN_KEY) ?? ''),
  },
  ...buildSourceEcDetailListBusinessColumns(t, getSourceEcDetailField),
  CreateActionColumn({
    width: 88,
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'logistics:manufacturing:engineering:change:source:ec:query',
        onClick: (record: SourceEcDetail) => void handleShowDetail(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SourceEcDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SourceEcDetail, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getSourceEcDetailId(selectedRow.value) === getSourceEcDetailId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SourceEcDetail[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SourceEcDetail) {
  const key = getSourceEcDetailId(record)
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
 * @returns {SourceEcDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SourceEcDetailQuery>): SourceEcDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SourceEcDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    sourceEcId: masterSourceEcId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SourceEcDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('sourceFinishedProduct', form.sourceFinishedProduct)
  assignTrimmed('sourceParentPart', form.sourceParentPart)
  assignTrimmed('sourceLegacyPartNo', form.sourceLegacyPartNo)
  assignTrimmed('sourceLegacyPartName', form.sourceLegacyPartName)
  if (form.sourceLegacyUsage !== undefined && form.sourceLegacyUsage !== null) {
    query.sourceLegacyUsage = form.sourceLegacyUsage
  }
  assignTrimmed('sourceLegacyMountingPosition', form.sourceLegacyMountingPosition)
  assignTrimmed('sourceReplacementPartNo', form.sourceReplacementPartNo)
  assignTrimmed('sourceReplacementPartName', form.sourceReplacementPartName)
  if (form.sourceReplacementUsage !== undefined && form.sourceReplacementUsage !== null) {
    query.sourceReplacementUsage = form.sourceReplacementUsage
  }
  assignTrimmed('sourceReplacementMountingPosition', form.sourceReplacementMountingPosition)
  assignTrimmed('sourceBomNo', form.sourceBomNo)
  assignTrimmed('SourceCompatibility', form.SourceCompatibility)
  assignTrimmed('sourceDistinction', form.sourceDistinction)
  assignTrimmed('SourceInstruction', form.SourceInstruction)
  assignTrimmed('sourceLegacyPartDisposition', form.sourceLegacyPartDisposition)
  assignTrimmed('sourceBomEffectiveDateStart', form.sourceBomEffectiveDateStart)
  assignTrimmed('sourceBomEffectiveDateEnd', form.sourceBomEffectiveDateEnd)
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
    const res = await getSourceEcDetailList(buildListQuery())
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
watch(masterSourceEcId, () => {
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

/** 打开详情弹窗 */
async function handleShowDetail(record: SourceEcDetail) {
  const id = getSourceEcDetailId(record)
  if (!id) {
    return
  }
  detailVisible.value = true
  detailLoading.value = true
  detailData.value = {}
  try {
    const detail = await getSourceEcDetailById(id)
    detailData.value = detail ? { ...detail } : { ...record }
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    detailVisible.value = false
  } finally {
    detailLoading.value = false
  }
}

/** 关闭详情弹窗 */
function handleDetailClose() {
  detailVisible.value = false
  detailData.value = {}
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
  const res = await getSourceEcDetailTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importSourceEcDetail(file, sheetName)
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
    const exportMeta = await exportSourceEcDetail(
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
    message.success(t('common.feedback.export.success', { target: t('entity.sourceecdetail._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.sourceecdetail._self') }))
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
