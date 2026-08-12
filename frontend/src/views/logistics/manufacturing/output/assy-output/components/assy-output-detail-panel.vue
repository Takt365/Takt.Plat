<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/assy-output/components -->
<!-- 文件名称：assy-output-detail-panel.vue -->
<!-- 功能描述：组立日报主表右侧明细 assyOutputDetail 列表（按主表 assyOutputId 分页）；支持编辑，不提供新增/删除 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="assy-output-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      update-permission="logistics:manufacturing:output:assy:update"
      import-permission="logistics:manufacturing:output:assy:import"
      export-permission="logistics:manufacturing:output:assy:export"
      :show-create="false"
      :show-update="true"
      :show-delete="false"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection || masterProdDateLocked"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :refresh-loading="loading"
      @update="handleUpdate"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="assy-output-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getAssyOutputDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="assyOutputDetailId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      >
        <template #summary>
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell :index="0" />
              <a-table-summary-cell
                v-for="cell in summaryCells"
                :key="cell.key"
                :index="cell.index"
              >
                <span class="text-sm font-medium">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </template>
      </TaktSingleTable>
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="720px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <AssyOutputDetailForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterAssyOutputId"
        :master-context="masterDerivedContext"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-output-assy-output-assy-output-detail"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('prodOrderCode')">
      <a-form-item :label="pi.queryLabel('prodOrderCode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="pi.queryPh('prodOrderCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="pi.queryLabel('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="pi.queryPh('lineNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('timePeriod')">
      <a-form-item :label="pi.queryLabel('timePeriod')">
        <a-input
          v-model:value="advancedQueryForm.timePeriod"
          :placeholder="pi.queryPh('timePeriod', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdCapacity')">
      <a-form-item :label="pi.queryLabel('stdCapacity')">
        <a-input-number
          v-model:value="advancedQueryForm.stdCapacity"
          :placeholder="pi.queryPh('stdCapacity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodActualQty')">
      <a-form-item :label="pi.queryLabel('prodActualQty')">
        <a-input-number
          v-model:value="advancedQueryForm.prodActualQty"
          :placeholder="pi.queryPh('prodActualQty', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeMinutes')">
      <a-form-item :label="pi.queryLabel('downtimeMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.downtimeMinutes"
          :placeholder="pi.queryPh('downtimeMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeReason')">
      <a-form-item :label="pi.queryLabel('downtimeReason')">
        <TaktSelect
          v-model:value="advancedQueryForm.downtimeReason"
          dict-type="logistics_stop_reason_category"
          :placeholder="pi.queryPh('downtimeReason', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeDescription')">
      <a-form-item :label="pi.queryLabel('downtimeDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.downtimeDescription"
          :placeholder="pi.queryPh('downtimeDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unachievedReason')">
      <a-form-item :label="pi.queryLabel('unachievedReason')">
        <TaktSelect
          v-model:value="advancedQueryForm.unachievedReason"
          dict-type="logistics_nonachievement_reason_category"
          :placeholder="pi.queryPh('unachievedReason', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unachievedDescription')">
      <a-form-item :label="pi.queryLabel('unachievedDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.unachievedDescription"
          :placeholder="pi.queryPh('unachievedDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inputMinutes')">
      <a-form-item :label="pi.queryLabel('inputMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.inputMinutes"
          :placeholder="pi.queryPh('inputMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualMinutes')">
      <a-form-item :label="pi.queryLabel('actualMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.actualMinutes"
          :placeholder="pi.queryPh('actualMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectMinutes')">
      <a-form-item :label="pi.queryLabel('indirectMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectMinutes"
          :placeholder="pi.queryPh('indirectMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('confirmMinutes')">
      <a-form-item :label="pi.queryLabel('confirmMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.confirmMinutes"
          :placeholder="pi.queryPh('confirmMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mixedProd')">
      <a-form-item :label="pi.queryLabel('mixedProd')">
        <a-input-number
          v-model:value="advancedQueryForm.mixedProd"
          :placeholder="pi.queryPh('mixedProd', 'required')"
          style="width: 100%"
          :min="0"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('achievementRate')">
      <a-form-item :label="pi.queryLabel('achievementRate')">
        <a-input-number
          v-model:value="advancedQueryForm.achievementRate"
          :placeholder="pi.queryPh('achievementRate', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
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
            <span>{{ pi.queryLabel('extField') }}</span>
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
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
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
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="ASSYOUTPUTDETAIL_SELF_I18N_KEY"
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
      id-column-key="assyOutputDetailId"
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
 * 组立日报子表 assyOutputDetail 右栏面板
 * @module views/logistics/manufacturing/output/assy-output/components
 */
import { ref, computed, watch, onMounted, onBeforeUnmount, nextTick, h } from 'vue'
import { message, Tooltip } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/use-takt-master-detail-lr-scroll-y'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
} from '@/utils/table-columns'
import { formatSummaryValue } from '@/components/business/takt-editable-table/editable-table-utils'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiQuestionLine } from '@remixicon/vue'
import AssyOutputDetailForm from './assy-output-detail-form.vue'
import { useAssyOutputMasterContext } from '../composables/use-assy-output-master-context'
import { resolvePersonnelOperationRatePercent } from '../composables/use-assy-output-derived-calc'
import { useAssyOutputI18n } from '../composables/use-assy-output-i18n'
import {
  getAssyOutputProdDateYmdFromRecord,
  isAssyOutputProdDateLocked,
} from '../composables/takt-assy-output-prod-date-edit-lock'
import {
  getAssyOutputDetailList,
  getAssyOutputDetailById,
  updateAssyOutputDetail,
  getAssyOutputDetailTemplate,
  importAssyOutputDetail,
  exportAssyOutputDetail,
} from '@/api/logistics/manufacturing/output/assy-output-detail'
import type { AssyOutputDetail, AssyOutputDetailQuery } from '@/types/logistics/manufacturing/output/assy-output-detail'

import {
  useAssyOutputDetailI18n,
  ASSYOUTPUTDETAIL_LIST_FIELDS,
  ASSYOUTPUTDETAIL_QUERY_STRING_FIELDS,
  ASSYOUTPUTDETAIL_QUERY_FIELDS,
  ASSYOUTPUTDETAIL_SELF_I18N_KEY,
  ASSYOUTPUTDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS,
  ASSYOUTPUTDETAIL_SUMMARY_SUM_FIELDS,
} from '../composables/use-assy-output-detail-i18n'
import {
  ASSY_DETAIL_DOWNTIME_REASON_DICT,
  ASSY_DETAIL_UNACHIEVED_REASON_DICT,
} from '../composables/assy-output-detail-dict-multi'
import { useAssyOutputDetailDictMultiFormat } from '../composables/use-assy-output-detail-dict-multi-format'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useAssyOutputDetailI18n()
const masterPi = useAssyOutputI18n()
const { resolveQueryLabel } = useAssyOutputDetailDictMultiFormat()

const { t } = useI18n()
const { selectedMasterRow } = useAssyOutputMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAssyOutputDetail')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)
/** 子表滚动区容器（扣除查询/工具栏后剩余高度） */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（按 __table-wrap 实测，避免沿用主表共享高度导致双滚动条） */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 按子表容器重算 scroll.y（扣除表头 + 汇总行，避免合计被裁切或双滚动条） */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap, { reserveSummaryRow: true })
}

/** 监听子表容器尺寸变化 */
function startDetailTableScrollObserve(): void {
  stopDetailTableScrollObserve()
  recalcDetailTableScrollY()
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollResizeObserver = new ResizeObserver(() => {
    recalcDetailTableScrollY()
  })
  detailTableScrollResizeObserver.observe(wrap)
}

/** 停止监听子表容器尺寸 */
function stopDetailTableScrollObserve(): void {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
}

const dataSource = ref<AssyOutputDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<AssyOutputDetail | null>(null)
const selectedRows = ref<AssyOutputDetail[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<AssyOutputDetail>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(ASSYOUTPUTDETAIL_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof ASSYOUTPUTDETAIL_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    prodActualQty: undefined as number | undefined,
    downtimeMinutes: undefined as number | undefined,
    inputMinutes: undefined as number | undefined,
    actualMinutes: undefined as number | undefined,
    indirectMinutes: undefined as number | undefined,
    confirmMinutes: undefined as number | undefined,
    mixedProd: undefined as number | undefined,
    stdCapacity: undefined as number | undefined,
    achievementRate: undefined as number | undefined,
  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  ASSYOUTPUTDETAIL_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...ASSYOUTPUTDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...ASSYOUTPUTDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'assyOutputDetailId'
const masterAssyOutputId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['assyOutputId']
  return id != null ? String(id) : ''
})
/** 明细标准产能折算用标准生产稼动率(%) */
const masterOperationRatePercent = ref(0)
watch(
  () => {
    const row = selectedMasterRow.value as Record<string, unknown> | null
    return [row?.plantCode, row?.prodDate] as const
  },
  async ([plantCode, prodDate]) => {
    const plant = String(plantCode ?? '').trim()
    const dateText = String(prodDate ?? '').trim()
    if (!plant || !dateText) {
      masterOperationRatePercent.value = 0
      return
    }
    try {
      masterOperationRatePercent.value = await resolvePersonnelOperationRatePercent(plant, dateText)
    } catch {
      masterOperationRatePercent.value = 0
    }
  },
  { immediate: true }
)
/** 主表派生字段快照（供明细表单计算投入/实际工时、达成率） */
const masterDerivedContext = computed(() => {
  const row = selectedMasterRow.value as Record<string, unknown> | null
  if (!row) {
    return null
  }
  return {
    directLabor: Number(row.directLabor) || 0,
    indirectLabor: Number(row.indirectLabor) || 0,
    stdCapacity: Number(row.stdCapacity) || 0,
    stdMinutes: Number(row.stdMinutes) || 0,
    operationRatePercent: masterOperationRatePercent.value,
  }
})
const hasMasterSelection = computed(() => masterAssyOutputId.value !== '')
/** 主表生产日期是否已锁定 */
const masterProdDateLocked = computed(() => {
  const row = selectedMasterRow.value as Record<string, unknown> | null
  const ymd = getAssyOutputProdDateYmdFromRecord(row)
  return ymd !== '' && isAssyOutputProdDateLocked(ymd)
})
const updateDisabled = computed(() =>
  !hasMasterSelection.value || selectedRows.value.length !== 1 || masterProdDateLocked.value,
)

function getAssyOutputDetailId(record: AssyOutputDetail | Record<string, unknown>): string {
  return String((record as AssyOutputDetail)?.[entityIdName] ?? '')
}

function getAssyOutputDetailField(record: AssyOutputDetail | Record<string, unknown>, field: string): unknown {
  return (record as AssyOutputDetail)?.[field as keyof AssyOutputDetail]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'assyOutputDetailId',
    key: 'assyOutputDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'assyOutputDetailId') ?? ''),
  },
  {
    title: pi.label('prodOrderCode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'prodOrderCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('timePeriod'),
    dataIndex: 'timePeriod',
    key: 'timePeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'timePeriod') ?? ''),
  },
  {
    title: () =>
      h('span', { class: 'inline-flex items-center gap-1 align-middle' }, [
        h(
          Tooltip,
          { title: pi.stdCapacityHint(), placement: 'top' },
          {
            default: () =>
              h('span', { class: 'takt-form-label-hint-icon inline-flex cursor-help' }, [
                h(RiQuestionLine, { class: 'takt-remix-icon' })]),
          },
        ),
        h('span', null, pi.label('stdCapacity'))]),
    taktColumnSettingLabel: pi.label('stdCapacity'),
    dataIndex: 'stdCapacity',
    key: 'stdCapacity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'stdCapacity') ?? ''),
  },
  {
    title: pi.label('prodActualQty'),
    dataIndex: 'prodActualQty',
    key: 'prodActualQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'prodActualQty') ?? ''),
  },
  {
    title: pi.label('downtimeMinutes'),
    dataIndex: 'downtimeMinutes',
    key: 'downtimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'downtimeMinutes') ?? ''),
  },
  {
    title: pi.label('downtimeReason'),
    dataIndex: 'downtimeReason',
    key: 'downtimeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'downtimeReason') ?? ''),
  },
  {
    title: pi.label('downtimeDescription'),
    dataIndex: 'downtimeDescription',
    key: 'downtimeDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'downtimeDescription') ?? ''),
  },
  {
    title: pi.label('unachievedReason'),
    dataIndex: 'unachievedReason',
    key: 'unachievedReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'unachievedReason') ?? ''),
  },
  {
    title: pi.label('unachievedDescription'),
    dataIndex: 'unachievedDescription',
    key: 'unachievedDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'unachievedDescription') ?? ''),
  },
  {
    title: pi.label('inputMinutes'),
    dataIndex: 'inputMinutes',
    key: 'inputMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'inputMinutes') ?? ''),
  },
  {
    title: pi.label('actualMinutes'),
    dataIndex: 'actualMinutes',
    key: 'actualMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'actualMinutes') ?? ''),
  },
  {
    title: pi.label('indirectMinutes'),
    dataIndex: 'indirectMinutes',
    key: 'indirectMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'indirectMinutes') ?? ''),
  },
  {
    title: () =>
      h('span', { class: 'inline-flex items-center gap-1 align-middle' }, [
        h(
          Tooltip,
          { title: pi.confirmMinutesHint(), placement: 'top' },
          {
            default: () =>
              h('span', { class: 'takt-form-label-hint-icon inline-flex cursor-help' }, [
                h(RiQuestionLine, { class: 'takt-remix-icon' })]),
          },
        ),
        h('span', null, pi.label('confirmMinutes'))]),
    taktColumnSettingLabel: pi.label('confirmMinutes'),
    dataIndex: 'confirmMinutes',
    key: 'confirmMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'confirmMinutes') ?? ''),
  },
  {
    title: pi.label('mixedProd'),
    dataIndex: 'mixedProd',
    key: 'mixedProd',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'mixedProd') ?? ''),
  },
  {
    title: pi.label('achievementRate'),
    dataIndex: 'achievementRate',
    key: 'achievementRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'achievementRate') ?? ''),
  },
  {
    title: pi.label('assyOutputId'),
    dataIndex: 'assyOutputId',
    key: 'assyOutputId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: AssyOutputDetail }) =>
      String(getAssyOutputDetailField(record, 'assyOutputId') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:assy:update',
        disabled: () => masterProdDateLocked.value,
        onClick: (record: AssyOutputDetail) => void handleEdit(record),
      }],
  })])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'company')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'assyOutputDetailId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(ASSYOUTPUTDETAIL_SUMMARY_SUM_FIELDS)

/** 汇总行首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 汇总行单元格（index 与 a-table 列序一致：0=行选择，1..n=展示列） */
const summaryCells = computed(() => {
  const cells: Array<{ key: string; text: string; index: number }> = []
  resolvedSummaryColumns.value.forEach((col, columnIndex) => {
    const key = String(col.key ?? columnIndex)
    let text = ''
    if (columnIndex === 0) {
      text = summaryLabel.value
    } else if (isSummarySumField(key)) {
      text = formatSummaryFieldTotal(key)
    }
    cells.push({
      key,
      text,
      index: columnIndex + 1,
    })
  })
  return cells
})

/** 是否参与当前页合计 */
function isSummarySumField(field: string): boolean {
  return summarySumFieldSet.has(field)
}

/** 当前页 dataSource 各合计列求和 */
const summaryFieldTotals = computed(() => {
  const totals = Object.fromEntries(
    ASSYOUTPUTDETAIL_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof ASSYOUTPUTDETAIL_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of ASSYOUTPUTDETAIL_SUMMARY_SUM_FIELDS) {
      const num = Number(getAssyOutputDetailField(row, field))
      if (Number.isFinite(num)) {
        totals[field] += num
      }
    }
  }
  return totals
})

/** 格式化合计单元格展示值 */
function formatSummaryFieldTotal(field: string): string {
  if (!isSummarySumField(field)) {
    return ''
  }
  return formatSummaryValue(summaryFieldTotals.value[field as keyof typeof summaryFieldTotals.value])
}

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AssyOutputDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: AssyOutputDetail, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getAssyOutputDetailId(selectedRow.value) === getAssyOutputDetailId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: AssyOutputDetail[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: AssyOutputDetail) {
  const key = getAssyOutputDetailId(record)
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
 * @returns {AssyOutputDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<AssyOutputDetailQuery>): AssyOutputDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: AssyOutputDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    assyOutputId: masterAssyOutputId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof AssyOutputDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of ASSYOUTPUTDETAIL_QUERY_STRING_FIELDS) {
    if (key === 'downtimeReason') {
      const label = resolveQueryLabel(form.downtimeReason, ASSY_DETAIL_DOWNTIME_REASON_DICT)
      if (label) {
        query.downtimeReason = label
      }
      continue
    }
    if (key === 'unachievedReason') {
      const label = resolveQueryLabel(form.unachievedReason, ASSY_DETAIL_UNACHIEVED_REASON_DICT)
      if (label) {
        query.unachievedReason = label
      }
      continue
    }
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.prodActualQty !== undefined && form.prodActualQty !== null) {
    query.prodActualQty = form.prodActualQty
  }
  if (form.downtimeMinutes !== undefined && form.downtimeMinutes !== null) {
    query.downtimeMinutes = form.downtimeMinutes
  }
  if (form.inputMinutes !== undefined && form.inputMinutes !== null) {
    query.inputMinutes = form.inputMinutes
  }
  if (form.actualMinutes !== undefined && form.actualMinutes !== null) {
    query.actualMinutes = form.actualMinutes
  }
  if (form.indirectMinutes !== undefined && form.indirectMinutes !== null) {
    query.indirectMinutes = form.indirectMinutes
  }
  if (form.confirmMinutes !== undefined && form.confirmMinutes !== null) {
    query.confirmMinutes = form.confirmMinutes
  }
  if (form.mixedProd !== undefined && form.mixedProd !== null) {
    query.mixedProd = form.mixedProd
  }
  if (form.stdCapacity !== undefined && form.stdCapacity !== null) {
    query.stdCapacity = form.stdCapacity
  }
  if (form.achievementRate !== undefined && form.achievementRate !== null) {
    query.achievementRate = form.achievementRate
  }
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
    const res = await getAssyOutputDetailList(buildListQuery())
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
watch(masterAssyOutputId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

onMounted(() => {
  startDetailTableScrollObserve()
})

onBeforeUnmount(() => {
  stopDetailTableScrollObserve()
})

watch(
  () => loading.value,
  (isLoading) => {
    if (!isLoading) {
      void nextTick(() => recalcDetailTableScrollY())
    }
  },
)

watch(
  () => [dataSource.value.length, visibleColumnKeys.value.join(',')],
  () => {
    void nextTick(() => recalcDetailTableScrollY())
  },
)

watch(hasMasterSelection, (selected) => {
  if (selected) {
    void nextTick(() => startDetailTableScrollObserve())
  }
})

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

async function handleEdit(record: AssyOutputDetail) {
  if (masterProdDateLocked.value) {
    const ymd = getAssyOutputProdDateYmdFromRecord(selectedMasterRow.value as Record<string, unknown> | null)
    message.warning(masterPi.prodDateLockedMessage(ymd))
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getAssyOutputDetailById(getAssyOutputDetailId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (masterProdDateLocked.value) {
    const ymd = getAssyOutputProdDateYmdFromRecord(selectedMasterRow.value as Record<string, unknown> | null)
    message.warning(masterPi.prodDateLockedMessage(ymd))
    return
  }
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: pi.self(),
    }))
  }
}

async function handleFormSubmit() {
  if (masterProdDateLocked.value) {
    const ymd = getAssyOutputProdDateYmdFromRecord(selectedMasterRow.value as Record<string, unknown> | null)
    message.warning(masterPi.prodDateLockedMessage(ymd))
    return
  }
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
    const id = formData.value?.assyOutputDetailId
    if (!id) {
      return
    }
    await updateAssyOutputDetail(id, payload)
    message.success(t('common.feedback.updated', { target: pi.self() }))
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
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
  if (masterProdDateLocked.value) {
    const ymd = getAssyOutputProdDateYmdFromRecord(selectedMasterRow.value as Record<string, unknown> | null)
    message.warning(masterPi.prodDateLockedMessage(ymd))
    return
  }
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getAssyOutputDetailTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importAssyOutputDetail(file, sheetName)
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
    const exportMeta = await exportAssyOutputDetail(
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
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
