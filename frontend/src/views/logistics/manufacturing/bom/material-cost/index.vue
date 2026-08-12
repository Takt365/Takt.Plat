<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：BOM 物料成本汇总页（CRUD + 成本合计/重算/回填机种；合计走 Analyses API） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="logistics:manufacturing:bom:material:cost:create"
      update-permission="logistics:manufacturing:bom:material:cost:update"
      delete-permission="logistics:manufacturing:bom:material:cost:delete"
      import-permission="logistics:manufacturing:bom:material:cost:import"
      export-permission="logistics:manufacturing:bom:material:cost:export"
      :left-actions="costToolbarLeftActions"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'bomMaterialCostId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="getBomMaterialCostId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'materialType'">
          <TaktDictTag
            :value="getBomMaterialCostDictValue(record, 'materialType')"
            dict-type="logistics_material_type"
          />
        </template>
        <template v-else-if="column.key === 'productCode'">
          <TaktDictTag
            :value="getBomMaterialCostDictValue(record, 'productCode')"
            dict-type="logistics_material_type"
          />
        </template>
        <template v-else-if="column.key === 'currencyCode'">
          <TaktDictTag
            :value="getBomMaterialCostDictValue(record, 'currencyCode')"
            dict-type="accounting_currency_code"
          />
        </template>
      </template>

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="50%"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <BomMaterialCostForm
        :key="formData?.bomMaterialCostId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-logistics-manufacturing-bom-material-cost'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="pi.queryLabel('cultureCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="pi.queryPh('cultureCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="pi.queryLabel('plantCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.plantCode"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('plantCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('modelCode')">
      <a-form-item :label="pi.queryLabel('modelCode')">
        <a-input
          v-model:value="advancedQueryForm.modelCode"
          :placeholder="pi.queryPh('modelCode', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('modelMonthlyAverageCost')">
      <a-form-item :label="pi.queryLabel('modelMonthlyAverageCost')">
        <a-input-number
          v-model:value="advancedQueryForm.modelMonthlyAverageCost"
          :placeholder="pi.queryPh('modelMonthlyAverageCost', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialType')">
      <a-form-item :label="pi.queryLabel('materialType')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialType"
          dict-type="logistics_material_type"
          :placeholder="pi.queryPh('materialType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productCode')">
      <a-form-item :label="pi.queryLabel('productCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.productCode"
          dict-type="logistics_material_type"
          :placeholder="pi.queryPh('productCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productDescription')">
      <a-form-item :label="pi.queryLabel('productDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.productDescription"
          :placeholder="pi.queryPh('productDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productMonthlyCost')">
      <a-form-item :label="pi.queryLabel('productMonthlyCost')">
        <a-input-number
          v-model:value="advancedQueryForm.productMonthlyCost"
          :placeholder="pi.queryPh('productMonthlyCost', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="pi.queryLabel('currencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.currencyCode"
          dict-type="accounting_currency_code"
          :placeholder="pi.queryPh('currencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costingPeriod')">
      <a-form-item :label="pi.queryLabel('costingPeriod')">
        <a-date-picker
          v-model:value="advancedQueryForm.costingPeriod"
          :placeholder="pi.queryPh('costingPeriod', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costingDateStart')">
      <a-form-item :label="pi.queryLabel('costingDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.costingDateStart"
          :placeholder="pi.queryPh('costingDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costingDateEnd')">
      <a-form-item :label="pi.queryLabel('costingDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.costingDateEnd"
          :placeholder="pi.queryPh('costingDateEnd', 'select')"
          value-format="YYYY-MM-DD"
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
        :entity-i18n-key="BOMMATERIALCOST_SELF_I18N_KEY"
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
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'bomMaterialCostId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />

    <!-- 成本合计 / 重算 / 回填机种价格 -->
    <TaktModal
      v-model:open="recalculateModalVisible"
      :title="recalculateModalTitle"
      :confirm-loading="recalculatePending || refreshModelFieldsPending"
      :use-viewport-size="false"
      width="480px"
      @ok="handleRecalculateModalOk"
    >
      <a-form layout="vertical">
        <a-form-item
          v-if="recalculateModalMode === 'refreshModelFields'"
          :label="pi.label('plantCode')"
          required
        >
          <a-input
            v-model:value="recalculateForm.plantCode"
            :placeholder="t('logistics.manufacturing.bom.material-cost.page.selectPlantRequired')"
            allow-clear
          />
        </a-form-item>
        <a-form-item
          :label="t('logistics.manufacturing.bom.material-cost.page.costingMonth')"
          required
        >
          <a-date-picker
            v-model:value="recalculateForm.costingMonth"
            picker="month"
            value-format="YYYY-MM"
            class="w-full"
            :placeholder="t('logistics.manufacturing.bom.material-cost.page.costingMonthPlaceholder')"
          />
        </a-form-item>
        <a-form-item
          v-if="recalculateModalMode === 'sum' || recalculateModalMode === 'recalculate'"
          :label="t('logistics.manufacturing.bom.material-cost.page.processRecordCount')"
          :extra="t('logistics.manufacturing.bom.material-cost.page.processRecordCountHint')"
        >
          <a-input-number
            v-model:value="recalculateForm.processRecordCount"
            class="w-full"
            :min="0"
            :precision="0"
          />
        </a-form-item>
      </a-form>
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * BOM 物料成本汇总页（CRUD + Analyses 成本合计/重算/回填机种）
 * @module views/logistics/manufacturing/bom/material-cost
 */
import { ref, computed, onMounted, reactive } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import type { ToolBarAction } from '@/components/business/takt-tools-bar/index.vue'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import BomMaterialCostForm from './components/material-cost-form.vue'
import { getBomMaterialCostList, getBomMaterialCostById, createBomMaterialCost, updateBomMaterialCost, deleteBomMaterialCostById, deleteBomMaterialCostBatch, getBomMaterialCostTemplate, importBomMaterialCost, exportBomMaterialCost } from '@/api/logistics/manufacturing/bom/material-cost'
import {
  recalculateBomMaterialCostItemModelAverage,
  refreshBomMaterialCostModelFields,
} from '@/api/logistics/manufacturing/bom/material-cost-analysis'
import type { BomMaterialCost, BomMaterialCostQuery } from '@/types/logistics/manufacturing/bom/material-cost'
import type { BomMaterialCostItemQuery } from '@/types/logistics/manufacturing/bom/material-cost-item'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine, RiCalculatorLine, RiRefreshLine, RiPriceTag3Line } from '@remixicon/vue'
import {
  formatBomMaterialCostItemRecalculateDuration,
  useBomMaterialCostItemRecalculateSignalR,
} from '@/composables/use-bom-material-cost-item-recalculate-signalr'
import {
  buildDefaultCostingMonth,
  costingMonthToDateQuery,
} from './utils/bom-material-cost-period'

import {
  useBomMaterialCostI18n,
  BOMMATERIALCOST_LIST_FIELDS,
  BOMMATERIALCOST_QUERY_STRING_FIELDS,
  BOMMATERIALCOST_QUERY_FIELDS,
  BOMMATERIALCOST_SELF_I18N_KEY,
} from './composables/use-material-cost-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useBomMaterialCostI18n()
/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type BomMaterialCostRowRecord = BomMaterialCost | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBomMaterialCost')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<BomMaterialCost[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<BomMaterialCostRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<BomMaterialCostRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<BomMaterialCost> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of BOMMATERIALCOST_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.modelMonthlyAverageCost !== undefined && form.modelMonthlyAverageCost !== null) {
    return true
  }
  if (form.productMonthlyCost !== undefined && form.productMonthlyCost !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(BOMMATERIALCOST_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof BOMMATERIALCOST_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    modelMonthlyAverageCost: undefined as number | undefined,
    productMonthlyCost: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  BOMMATERIALCOST_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'bomMaterialCostId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/** 默认处理记录数（0=全部） */
const DEFAULT_PROCESS_RECORD_COUNT = 5000
/** 合计/重算/回填机种弹窗 */
const recalculateModalVisible = ref(false)
/** 弹窗模式 */
const recalculateModalMode = ref<'sum' | 'recalculate' | 'refreshModelFields'>('sum')
/** 合计/重算提交中 */
const recalculatePending = ref(false)
/** 回填机种提交中 */
const refreshModelFieldsPending = ref(false)
/** 弹窗表单 */
const recalculateForm = reactive({
  plantCode: '' as string,
  costingMonth: '' as string,
  processRecordCount: DEFAULT_PROCESS_RECORD_COUNT as number,
})

/** 弹窗标题 */
const recalculateModalTitle = computed(() => {
  if (recalculateModalMode.value === 'recalculate') {
    return t('logistics.manufacturing.bom.material-cost.page.costRecalculate')
  }
  if (recalculateModalMode.value === 'refreshModelFields') {
    return t('logistics.manufacturing.bom.material-cost.page.refreshModelFields')
  }
  return t('logistics.manufacturing.bom.material-cost.page.costSum')
})

/** 工具栏扩展：成本合计 / 重算 / 回填机种 */
const costToolbarLeftActions = computed<ToolBarAction[]>(() => [
  {
    key: 'cost-sum',
    label: t('logistics.manufacturing.bom.material-cost.page.costSum'),
    icon: RiCalculatorLine,
    permission: 'logistics:manufacturing:bom:material:cost:update',
    loading: recalculatePending.value,
    onClick: () => openRecalculateModal('sum'),
  },
  {
    key: 'cost-recalculate',
    label: t('logistics.manufacturing.bom.material-cost.page.costRecalculate'),
    icon: RiRefreshLine,
    permission: 'logistics:manufacturing:bom:material:cost:update',
    loading: recalculatePending.value,
    onClick: () => openRecalculateModal('recalculate'),
  },
  {
    key: 'refresh-model-fields',
    label: t('logistics.manufacturing.bom.material-cost.page.refreshModelFields'),
    icon: RiPriceTag3Line,
    permission: 'logistics:manufacturing:bom:material:cost:update',
    loading: refreshModelFieldsPending.value,
    onClick: () => openRecalculateModal('refreshModelFields'),
  },
])

/**
 * 打开合计/重算/回填机种弹窗
 * @param mode 弹窗模式
 */
function openRecalculateModal(mode: 'sum' | 'recalculate' | 'refreshModelFields') {
  recalculateModalMode.value = mode
  const form = advancedQueryForm.value
  recalculateForm.plantCode = String(form.plantCode ?? selectedRow.value?.plantCode ?? '').trim()
  const period = String(form.costingPeriod ?? selectedRow.value?.costingPeriod ?? '').trim()
  recalculateForm.costingMonth = /^\d{4}-\d{2}$/.test(period) ? period : buildDefaultCostingMonth()
  recalculateForm.processRecordCount = DEFAULT_PROCESS_RECORD_COUNT
  recalculateModalVisible.value = true
}

/**
 * 按弹窗核算月构建明细重算查询
 * @param costingMonth 核算月 yyyy-MM
 * @returns 查询 DTO；月份非法时 null
 */
function buildRecalculateItemQuery(costingMonth: string): BomMaterialCostItemQuery | null {
  const dates = costingMonthToDateQuery(costingMonth)
  if (!dates.costingDateStart || !dates.costingDateEnd) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.costNeedMonth'))
    return null
  }
  const form = advancedQueryForm.value
  const query: BomMaterialCostItemQuery = {
    pageIndex: 1,
    pageSize: 1,
    costingDateStart: dates.costingDateStart,
    costingDateEnd: dates.costingDateEnd,
  }
  const plantCode = String(form.plantCode ?? '').trim() || String(recalculateForm.plantCode ?? '').trim()
  const productCode = String(form.productCode ?? '').trim()
  const modelCode = String(form.modelCode ?? '').trim()
  if (plantCode) query.plantCode = plantCode
  if (productCode) query.productCode = productCode
  if (modelCode) query.modelCode = modelCode
  return query
}

/**
 * 弹窗确认：提交合计/重算或回填机种
 */
async function handleRecalculateModalOk() {
  const month = String(recalculateForm.costingMonth ?? '').trim()
  if (!month) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.costNeedMonth'))
    return
  }
  if (recalculateModalMode.value === 'refreshModelFields') {
    const plant = String(recalculateForm.plantCode ?? '').trim()
    if (!plant) {
      message.warning(t('logistics.manufacturing.bom.material-cost.page.selectPlantRequired'))
      return
    }
    refreshModelFieldsPending.value = true
    try {
      const modelCode = String(advancedQueryForm.value.modelCode ?? '').trim() || undefined
      const result = await refreshBomMaterialCostModelFields({
        plantCode: plant,
        costingPeriod: month,
        modelCode,
      })
      recalculateModalVisible.value = false
      message.success(
        t('logistics.manufacturing.bom.material-cost.page.refreshModelFieldsSuccess', {
          month: result.costingPeriod,
          scanned: result.scannedRowCount,
          modelUpdated: result.modelCodeUpdatedCount,
          averageUpdated: result.averageUpdatedCount,
          groups: result.modelGroupCount,
        }),
      )
      await loadData()
    } catch (error: unknown) {
      const err = error as { message?: string }
      message.error(
        err?.message || t('logistics.manufacturing.bom.material-cost.page.refreshModelFieldsFailed'),
      )
    } finally {
      refreshModelFieldsPending.value = false
    }
    return
  }
  const processRecordCount = Number(recalculateForm.processRecordCount ?? DEFAULT_PROCESS_RECORD_COUNT)
  if (!Number.isFinite(processRecordCount) || processRecordCount < 0) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.processRecordCountInvalid'))
    return
  }
  const query = buildRecalculateItemQuery(month)
  if (!query) return
  const forceRecalculate = recalculateModalMode.value === 'recalculate'
  if (forceRecalculate) {
    const confirmed = await new Promise<boolean>((resolve) => {
      Modal.confirm({
        title: t('logistics.manufacturing.bom.material-cost.page.costRecalculateConfirmTitle'),
        content: t('logistics.manufacturing.bom.material-cost.page.costRecalculateConfirmContent'),
        okText: t('common.page.button.ok'),
        cancelText: t('common.page.button.cancel'),
        onOk: () => resolve(true),
        onCancel: () => resolve(false),
      })
    })
    if (!confirmed) return
  }
  recalculatePending.value = true
  try {
    const submitted = await recalculateBomMaterialCostItemModelAverage(
      query,
      forceRecalculate,
      Math.floor(processRecordCount),
    )
    recalculateModalVisible.value = false
    const msgKey = forceRecalculate
      ? 'logistics.manufacturing.bom.material-cost.page.costRecalculateSubmitted'
      : 'logistics.manufacturing.bom.material-cost.page.costSumSubmitted'
    message.success(t(msgKey, { month: submitted.processedMonth }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('logistics.manufacturing.bom.material-cost.page.costRecalculateFailed'))
  } finally {
    recalculatePending.value = false
  }
}

useBomMaterialCostItemRecalculateSignalR(async (event) => {
  if (event.executeStatus === 1) {
    message.success(
      t('logistics.manufacturing.bom.material-cost.page.costRecalculateCompleted', {
        month: event.processedMonth,
        duration: formatBomMaterialCostItemRecalculateDuration(event.executeDuration),
        refreshed: event.refreshedGroupCount,
        skipped: event.skippedGroupCount,
      }),
    )
    await loadData()
    return
  }
  message.error(
    event.errorMessage || t('logistics.manufacturing.bom.material-cost.page.costRecalculateFailed'),
  )
})

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {BomMaterialCostQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<BomMaterialCostQuery>): BomMaterialCostQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: BomMaterialCostQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof BomMaterialCostQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of BOMMATERIALCOST_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.modelMonthlyAverageCost !== undefined && form.modelMonthlyAverageCost !== null) {
    query.modelMonthlyAverageCost = form.modelMonthlyAverageCost
  }
  if (form.productMonthlyCost !== undefined && form.productMonthlyCost !== null) {
    query.productMonthlyCost = form.productMonthlyCost
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})


/**
 * 构建列表标准文本列
 * @param key 列 key / dataIndex
 * @param title 列标题
 * @param options 宽度与固定列
 */
function buildBomMaterialCostListColumn(
  key: string,
  title: string,
  options?: { width?: number; fixed?: 'left' },
) {
  return {
    title,
    dataIndex: key,
    key,
    width: options?.width ?? 120,
    resizable: true,
    ellipsis: true,
    ...(options?.fixed ? { fixed: options.fixed } : {}),
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  buildBomMaterialCostListColumn('bomMaterialCostId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...BOMMATERIALCOST_LIST_FIELDS.map((key) => buildBomMaterialCostListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:material:cost:update',
        onClick: (record: BomMaterialCostRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:material:cost:delete',
        onClick: (record: BomMaterialCostRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getBomMaterialCostId = (record: BomMaterialCostRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getBomMaterialCostDictValue = (
  record: BomMaterialCostRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}



/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: BomMaterialCostRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: BomMaterialCostRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getBomMaterialCostId(selectedRow.value) === getBomMaterialCostId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: BomMaterialCostRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: BomMaterialCostRowRecord) => ({
  onClick: () => {
    const key = getBomMaterialCostId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getBomMaterialCostId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    if (!hasAnyListQueryFilter()) {
      dataSource.value = []
      total.value = 0
      return
    }
    const res = await getBomMaterialCostList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[BomMaterialCost] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（拉取详情，避免列表列裁剪字段） */
async function handleEdit(record: BomMaterialCostRowRecord) {
  const id = getBomMaterialCostId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getBomMaterialCostById(id)
    formData.value = detail ?? ({ ...record } as Partial<BomMaterialCost>)
    formVisible.value = true
  } catch (error: unknown) {
    message.error(t('common.feedback.load.data.failed'))
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
  }
}
/** 提交新增/编辑表单 */
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
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updateBomMaterialCost(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createBomMaterialCost(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getBomMaterialCostTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importBomMaterialCost(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportBomMaterialCost(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
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
  } catch (error: any) {
    logger.error('[BomMaterialCost] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: BomMaterialCostRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBomMaterialCostById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteBomMaterialCostBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
