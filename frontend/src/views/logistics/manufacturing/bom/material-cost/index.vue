<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：工厂/机种/核算单月查询；左机种→中产品→右明细三栏（不拆实体）；合计/重算/导入/导出 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="material-cost-lcr-page flex h-full min-h-0 flex-col p-4">
    <!-- 工厂 / 机种 / 核算单月（无高级查询） -->
    <material-cost-query-form
      v-model:plant-code="queryForm.plantCode"
      v-model:model-code="queryForm.modelCode"
      v-model:costing-month="costingMonth"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="false"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :refresh-loading="loading"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    >
      <template #left>
        <a-space>
          <a-button
            v-permission="'logistics:manufacturing:bom:material:cost:update'"
            class="takt-button-query"
            :loading="recalculatePending"
            @click="openRecalculateModal(false)"
          >
            {{ t('logistics.manufacturing.bom.material-cost.page.costSum') }}
          </a-button>
          <a-button
            v-permission="'logistics:manufacturing:bom:material:cost:update'"
            class="takt-button-reset"
            :loading="recalculatePending"
            @click="openRecalculateModal(true)"
          >
            {{ t('logistics.manufacturing.bom.material-cost.page.costRecalculate') }}
          </a-button>
          <a-button
            v-permission="'logistics:manufacturing:bom:material:cost:import'"
            class="takt-button-import"
            @click="handleImport"
          >
            <template #icon>
              <RiImportLine class="takt-remix-icon" />
            </template>
            {{ t('common.page.button.import') }}
          </a-button>
          <a-button
            v-permission="'logistics:manufacturing:bom:material:cost:export'"
            class="takt-button-export"
            :loading="loading"
            @click="handleExport"
          >
            <template #icon>
              <RiExportLine class="takt-remix-icon" />
            </template>
            {{ t('common.page.button.export') }}
          </a-button>
          <a-button
            v-permission="'logistics:manufacturing:bom:material:cost:list'"
            class="takt-button-query"
            @click="openZeroPriceModal"
          >
            {{ t('logistics.manufacturing.bom.material-cost.page.zeroPrice.button') }}
          </a-button>
        </a-space>
      </template>
    </TaktToolsBar>
    <!-- 左机种 / 中产品 / 右明细：等宽三栏，均为 TaktSingleTable + 外置分页 -->
    <div class="flex min-h-0 flex-1 flex-row overflow-hidden">
      <div class="flex h-full min-h-0 w-1/3 min-w-0 shrink-0 flex-col border-r border-border pr-3">
        <div
          ref="leftTableWrapRef"
          class="min-h-0 flex-1 overflow-hidden"
        >
          <TaktSingleTable
            class="h-full min-h-0"
            entity-scope="company"
            table-mode="single"
            :stripe="true"
            :columns="columns"
            :data-source="dataSource"
            :loading="loading"
            :row-key="getModelGroupKey"
            :row-selection="rowSelection"
            :custom-row="onMasterClickRow"
            id-column-key="groupKey"
            :visible-column-keys="visibleColumnKeys"
            :pagination="false"
            :scroll="{ y: leftTableScrollY }"
            @change="handleTableChange"
            @resize-column="handleResizeColumn"
          >
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'currencyCode'">
                <TaktDictTag
                  :value="getBomMaterialCostDictValue(record, 'currencyCode')"
                  dict-type="accounting_currency_code"
                />
              </template>
            </template>
          </TaktSingleTable>
        </div>
        <TaktPagination
          v-model:current="currentPage"
          v-model:page-size="pageSize"
          :total="total"
          :disabled="loading"
          @change="handleMasterPaginationChange"
        />
      </div>
      <BomMaterialCostProductPanel
        ref="bomMaterialCostProductPanelRef"
        class="h-full min-h-0 w-2/3 min-w-0 flex-1"
      />
    </div>

    <!-- 计算/重置成本：选择核算月与处理记录数 -->
    <TaktModal
      v-model:open="recalculateModalVisible"
      :title="recalculateModalForce
        ? t('logistics.manufacturing.bom.material-cost.page.costRecalculate')
        : t('logistics.manufacturing.bom.material-cost.page.costSum')"
      :width="480"
      :use-viewport-size="false"
      :confirm-loading="recalculatePending"
      :ok-text="t('common.page.button.ok')"
      :cancel-text="t('common.page.button.cancel')"
      @ok="handleRecalculateModalOk"
    >
      <a-form layout="vertical" class="pt-1">
        <a-form-item
          :label="t('entity.bommaterialcost.plantcode')"
          required
        >
          <TaktSelect
            v-model:value="recalculateForm.plantCode"
            api-url="TaktPlants/options"
            class="w-full"
            allow-clear
            :placeholder="t('logistics.manufacturing.bom.material-cost.page.selectPlantRequired')"
          />
        </a-form-item>
        <a-form-item
          :label="t('logistics.manufacturing.bom.material-cost.page.costingMonth')"
          required
        >
          <a-date-picker
            v-model:value="recalculateForm.costingMonth"
            picker="month"
            format="YYYY-MM"
            value-format="YYYY-MM"
            class="w-full"
            :placeholder="t('logistics.manufacturing.bom.material-cost.page.costingMonthPlaceholder')"
          />
        </a-form-item>
        <a-form-item
          :label="t('logistics.manufacturing.bom.material-cost.page.processRecordCount')"
          :extra="t('logistics.manufacturing.bom.material-cost.page.processRecordCountHint')"
        >
          <a-input-number
            v-model:value="recalculateForm.processRecordCount"
            :min="0"
            :precision="0"
            class="w-full"
          />
        </a-form-item>
      </a-form>
    </TaktModal>

    <!-- 导入对话框（导入明细源数据，Sync 后刷新汇总） -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t(BOMMATERIALCOSTITEM_SELF_I18N_KEY) })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="BOMMATERIALCOSTITEM_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="itemExcelNames.sheet"
        :template-file-name="itemExcelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <!-- 零价格：先选工厂 / 机种 / 核算月 -->
    <TaktModal
      v-model:open="zeroPriceMonthModalVisible"
      :title="t('logistics.manufacturing.bom.material-cost.page.zeroPrice.monthTitle')"
      :width="480"
      :use-viewport-size="false"
      :ok-text="t('common.page.button.ok')"
      :cancel-text="t('common.page.button.cancel')"
      @ok="handleZeroPriceMonthOk"
    >
      <a-form layout="vertical" class="pt-1">
        <a-form-item
          :label="t('entity.bommaterialcost.plantcode')"
          required
        >
          <TaktSelect
            v-model:value="zeroPricePlantCode"
            api-url="TaktPlants/options"
            class="w-full"
            allow-clear
            :placeholder="t('logistics.manufacturing.bom.material-cost.page.selectPlantRequired')"
            @change="handleZeroPricePlantChange"
          />
        </a-form-item>
        <a-form-item
          :label="t('entity.bommaterialcost.modelcode')"
          required
        >
          <TaktSelect
            :key="zeroPriceModelSelectKey"
            v-model:value="zeroPriceModelCode"
            api-url="TaktBomMaterialCosts/model-options"
            :api-params="zeroPriceModelApiParams"
            class="w-full"
            allow-clear
            :disabled="!zeroPricePlantCode"
            :placeholder="t('logistics.manufacturing.bom.material-cost.page.selectModelRequired')"
          />
        </a-form-item>
        <a-form-item
          :label="t('logistics.manufacturing.bom.material-cost.page.costingMonth')"
          required
        >
          <a-date-picker
            v-model:value="zeroPriceCostingMonth"
            picker="month"
            format="YYYY-MM"
            value-format="YYYY-MM"
            class="w-full"
            :placeholder="t('logistics.manufacturing.bom.material-cost.page.costingMonthPlaceholder')"
          />
        </a-form-item>
      </a-form>
    </TaktModal>

    <material-cost-zero-price-modal
      v-model:open="zeroPriceModalVisible"
      :plant-code="zeroPricePlantCode"
      :model-code="zeroPriceModelCode"
      :costing-month="zeroPriceCostingMonth"
    />

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'groupKey'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * BOM 物料成本汇总页（工具栏：合计成本 / 重算成本 / 导入明细 / 导出汇总）
 * @module views/logistics/manufacturing/bom/material-cost
 */
import { ref, reactive, computed, onMounted, onBeforeUnmount, nextTick } from 'vue'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/use-takt-master-detail-lr-scroll-y'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/table-scroll'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import BomMaterialCostProductPanel from './components/material-cost-product-panel.vue'
import MaterialCostQueryForm from './components/material-cost-query-form.vue'
import MaterialCostZeroPriceModal from './components/material-cost-zero-price-modal.vue'
import {
  provideBomMaterialCostMasterContext,
  type BomMaterialCostModelGroupRecord,
} from './composables/use-material-cost-master-context'
import {
  getBomMaterialCostModelGroupList,
  exportBomMaterialCost,
} from '@/api/logistics/manufacturing/bom/material-cost'
import {
  getBomMaterialCostItemTemplate,
  importBomMaterialCostItem,
  recalculateBomMaterialCostItemModelAverage,
} from '@/api/logistics/manufacturing/bom/material-cost-item'
import type {
  BomMaterialCostModelGroup,
  BomMaterialCostQuery,
} from '@/types/logistics/manufacturing/bom/material-cost'
import type { BomMaterialCostItemQuery } from '@/types/logistics/manufacturing/bom/material-cost-item'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiImportLine, RiExportLine } from '@remixicon/vue'
import {
  formatBomMaterialCostItemRecalculateDuration,
  useBomMaterialCostItemRecalculateSignalR,
} from '@/composables/use-bom-material-cost-item-recalculate-signalr'
import { BOMMATERIALCOSTITEM_SELF_I18N_KEY } from './composables/use-material-cost-item-i18n'
import { useBomMaterialCostI18n } from './composables/use-material-cost-i18n'
import {
  buildDefaultCostingMonth,
  costingMonthToDateQuery,
} from './utils/bom-material-cost-period'
import { formatBomMaterialCostAmount } from './utils/bom-material-cost-item-line-cost'
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/use-company-related-plant'
import { useTenantStore } from '@/stores/identity/tenant'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useBomMaterialCostI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
const tenantStore = useTenantStore()
/** 汇总导出默认 sheet / 文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBomMaterialCost')
/** 明细导入默认 sheet / 文件名前缀 */
const itemExcelNames = taktExcelEntityNames('TaktBomMaterialCostItem')
/** 查询条件：工厂 / 机种 */
const queryForm = reactive({
  plantCode: undefined as string | undefined,
  modelCode: undefined as string | undefined,
})
/** 核算单月 yyyy-MM */
const costingMonth = ref<string | null>(null)
/** 重算任务提交中 */
const recalculatePending = ref(false)
/** 零价格：月份选择弹窗 */
const zeroPriceMonthModalVisible = ref(false)
/** 零价格：已选工厂 */
const zeroPricePlantCode = ref<string | undefined>(undefined)
/** 零价格：已选机种 */
const zeroPriceModelCode = ref<string | undefined>(undefined)
/** 零价格：机种下拉刷新键 */
const zeroPriceModelSelectKey = ref(0)
/** 零价格：已选核算月 yyyy-MM */
const zeroPriceCostingMonth = ref('')
/** 零价格清单弹窗 */
const zeroPriceModalVisible = ref(false)

/** 零价机种下拉参数 */
const zeroPriceModelApiParams = computed(() => ({
  plantCode: zeroPricePlantCode.value || undefined,
}))
/** 计算/重置弹窗 */
const recalculateModalVisible = ref(false)
/** 弹窗是否为强制重置 */
const recalculateModalForce = ref(false)
/** 默认处理记录数（0=全部） */
const DEFAULT_PROCESS_RECORD_COUNT = 5000
/** 弹窗表单：工厂 + 核算月 + 处理记录数 */
const recalculateForm = reactive({
  plantCode: undefined as string | undefined,
  costingMonth: '' as string,
  processRecordCount: DEFAULT_PROCESS_RECORD_COUNT as number,
})

/** 列表 loading */
const loading = ref(false)
/** 机种聚合主表分页数据 */
const dataSource = ref<BomMaterialCostModelGroup[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<BomMaterialCostModelGroupRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<BomMaterialCostModelGroupRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 三层选中上下文 */
const { selectedModelGroup, selectedProductRow } = provideBomMaterialCostMasterContext()
/** 右侧产品+Item 面板 */
const bomMaterialCostProductPanelRef = ref<InstanceType<typeof BomMaterialCostProductPanel> | null>(null)

/**
 * 构建列表/导出查询参数（工厂 + 机种 + 核算单月）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {BomMaterialCostQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<BomMaterialCostQuery>): BomMaterialCostQuery {
  const dateQuery = costingMonthToDateQuery(costingMonth.value)
  const query: BomMaterialCostQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  const plantCode = queryForm.plantCode?.trim()
  const modelCode = queryForm.modelCode?.trim()
  if (plantCode) query.plantCode = plantCode
  if (modelCode) query.modelCode = modelCode
  if (dateQuery.costingDateStart) query.costingDateStart = dateQuery.costingDateStart
  if (dateQuery.costingDateEnd) query.costingDateEnd = dateQuery.costingDateEnd
  return query
}
/** 左栏表格容器（实测 scroll.y） */
const leftTableWrapRef = ref<HTMLElement | null>(null)
/** 左栏 scroll.y */
const leftTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
/** 左栏 ResizeObserver */
let leftTableScrollResizeObserver: ResizeObserver | null = null

/** 按左栏容器重算 scroll.y */
function recalcLeftTableScrollY(): void {
  const wrap = leftTableWrapRef.value
  if (!wrap) {
    return
  }
  leftTableScrollY.value = measureMasterDetailLrTableScrollY(wrap)
}

/** 监听左栏容器尺寸 */
function startLeftTableScrollObserve(): void {
  stopLeftTableScrollObserve()
  recalcLeftTableScrollY()
  const wrap = leftTableWrapRef.value
  if (!wrap) {
    return
  }
  leftTableScrollResizeObserver = new ResizeObserver(() => {
    recalcLeftTableScrollY()
  })
  leftTableScrollResizeObserver.observe(wrap)
}

/** 停止左栏容器监听 */
function stopLeftTableScrollObserve(): void {
  leftTableScrollResizeObserver?.disconnect()
  leftTableScrollResizeObserver = null
}

/**
 * 默认选中当前登录公司关联工厂
 * @returns {Promise<void>}
 */
async function applyDefaultPlantFromCompany(): Promise<void> {
  const plant = await resolveCurrentCompanyRelatedPlantCode()
  queryForm.plantCode = plant || undefined
}

/** 默认核算单月：当月 */
function applyDefaultCostingMonth() {
  costingMonth.value = buildDefaultCostingMonth()
}

/** 页面挂载：分页配置 + 字典 + 默认工厂/核算月；列表须查询后加载 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  applyDefaultCostingMonth()
  await applyDefaultPlantFromCompany()
  await nextTick()
  startLeftTableScrollObserve()
})

watch(
  () => tenantStore.companyCode,
  () => {
    void applyDefaultPlantFromCompany()
  },
)

onBeforeUnmount(() => {
  stopLeftTableScrollObserve()
})

/** 汇总行点击选中 key（左栏高亮） */
const selectedMasterKey = ref('')

/**
 * 同步机种主表选中（切换时清空产品选中）
 * @param record 机种聚合行
 */
function syncMasterSelection(record: BomMaterialCostModelGroupRecord | null) {
  selectedModelGroup.value = record
  selectedMasterKey.value = record ? getModelGroupKey(record) : ''
  selectedProductRow.value = null
}

/**
 * 选中机种主表行后加载产品子表
 * @param record 机种聚合行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as BomMaterialCostModelGroupRecord
  const key = getModelGroupKey(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
  syncMasterSelection(row)
}

/**
 * 左栏行点击（高亮 + 同步选中）
 * @param record 机种聚合行
 * @returns customRow 属性
 */
function onMasterClickRow(record: BomMaterialCostModelGroupRecord) {
  const key = getModelGroupKey(record)
  return {
    onClick: () => {
      handleMasterSelect(record as unknown as Record<string, unknown>)
    },
    class:
      selectedMasterKey.value === key
        ? 'takt-master-detail-table-row-selected cursor-pointer'
        : 'cursor-pointer',
  }
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}

/** 机种主表列（工厂/机种/机种月均/币种/核算期间） */
const columns = computed<TableColumnsType>(() => [
  {
    title: pi.label('plantCode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 100,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getBomMaterialCostField(record, 'plantCode') ?? '',
  },
  {
    title: pi.label('modelCode'),
    dataIndex: 'modelCode',
    key: 'modelCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBomMaterialCostField(record, 'modelCode') ?? '',
  },
  {
    title: pi.label('modelMonthlyAverageCost'),
    dataIndex: 'modelMonthlyAverageCost',
    key: 'modelMonthlyAverageCost',
    width: 140,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) =>
      formatBomMaterialCostAmount(getBomMaterialCostField(record, 'modelMonthlyAverageCost')),
  },
  {
    title: pi.label('currencyCode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 100,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('costingPeriod'),
    dataIndex: 'costingPeriod',
    key: 'costingPeriod',
    width: 110,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBomMaterialCostField(record, 'costingPeriod') ?? '',
  },
  {
    title: t('logistics.manufacturing.bom.material-cost.page.productRowCount'),
    dataIndex: 'productRowCount',
    key: 'productRowCount',
    width: 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBomMaterialCostField(record, 'productRowCount') ?? '',
  },
])

/**
 * 机种聚合行 key
 * @param record 机种行
 * @returns {string} groupKey
 */
const getModelGroupKey = (record: BomMaterialCostModelGroupRecord): string => {
  const g = record as Record<string, unknown>
  if (g.groupKey != null && String(g.groupKey).length > 0) {
    return String(g.groupKey)
  }
  return `${String(g.plantCode ?? '')}|${String(g.modelCode ?? '')}|${String(g.costingPeriod ?? '')}`
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getBomMaterialCostField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getBomMaterialCostDictValue = (
  record: BomMaterialCostModelGroupRecord,
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
  onChange: (keys: (string | number)[], rows: BomMaterialCostModelGroupRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: BomMaterialCostModelGroupRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (
      selectedRow.value
      && getModelGroupKey(selectedRow.value) === getModelGroupKey(record)
    ) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: BomMaterialCostModelGroupRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  },
}))

/** 加载机种聚合主表分页（须先选工厂） */
async function loadData() {
  if (!queryForm.plantCode?.trim()) {
    dataSource.value = []
    total.value = 0
    syncMasterSelection(null)
    return
  }
  loading.value = true
  try {
    const res = await getBomMaterialCostModelGroupList(buildListQuery())
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

/** 租户/公司切换：先同步公司关联工厂，再重载列表 */
useTableRefresh(async () => {
  await applyDefaultPlantFromCompany()
  await loadData()
})

/** 查询（须工厂） */
function handleSearch() {
  if (!queryForm.plantCode?.trim()) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.selectPlantRequired'))
    return
  }
  currentPage.value = getTaktDefaultPageIndex()
  syncMasterSelection(null)
  selectedRowKeys.value = []
  selectedRows.value = []
  selectedRow.value = null
  void loadData()
}

/** 重置工厂/机种/核算月（工厂恢复公司默认；核算月恢复当月） */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  queryForm.modelCode = undefined
  applyDefaultCostingMonth()
  currentPage.value = getTaktDefaultPageIndex()
  dataSource.value = []
  total.value = 0
  syncMasterSelection(null)
  selectedRowKeys.value = []
  selectedRows.value = []
  selectedRow.value = null
}

/**
 * 由核算期间 yyyy-MM 生成月起止日期
 * @param period 核算期间
 * @returns 起止 YYYY-MM-DD；非法时 null
 */
function resolveMonthRangeFromPeriod(period: string): { start: string; end: string } | null {
  const match = /^(\d{4})-(\d{2})$/.exec(period.trim())
  if (!match) return null
  const year = Number(match[1])
  const month = Number(match[2])
  if (!Number.isFinite(year) || month < 1 || month > 12) return null
  const pad = (n: number) => String(n).padStart(2, '0')
  const lastDay = new Date(year, month, 0).getDate()
  return {
    start: `${year}-${pad(month)}-01`,
    end: `${year}-${pad(month)}-${pad(lastDay)}`,
  }
}

/**
 * 解析弹窗默认核算月（选中行 → 查询单月 → 当前月）
 * @returns {string} yyyy-MM
 */
function resolveDefaultCostingMonth(): string {
  const row = selectedModelGroup.value as Record<string, unknown> | null
  const fromRow = String(row?.costingPeriod ?? '').trim()
  if (resolveMonthRangeFromPeriod(fromRow)) return fromRow
  const fromQueryMonth = costingMonth.value?.trim()
  if (fromQueryMonth && resolveMonthRangeFromPeriod(fromQueryMonth)) return fromQueryMonth
  return buildDefaultCostingMonth()
}

/**
 * 按弹窗核算月构建明细重算查询（工厂必填：弹窗所选）
 * @param {string} costingMonth 核算月 yyyy-MM
 * @returns 查询 DTO；月份或工厂非法时 null
 */
function buildRecalculateItemQuery(costingMonth: string): BomMaterialCostItemQuery | null {
  const plantCode = String(recalculateForm.plantCode ?? '').trim()
  if (!plantCode) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.selectPlantRequired'))
    return null
  }
  const range = resolveMonthRangeFromPeriod(costingMonth)
  if (!range) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.costNeedMonth'))
    return null
  }
  const query: BomMaterialCostItemQuery = {
    pageIndex: 1,
    pageSize: 1,
    plantCode,
    costingDateStart: range.start,
    costingDateEnd: range.end,
  }
  const modelFromQuery = queryForm.modelCode?.trim()
  if (modelFromQuery) {
    query.modelCode = modelFromQuery
  }
  if (selectedModelGroup.value && !query.modelCode) {
    const row = selectedModelGroup.value as Record<string, unknown>
    const m = String(row.modelCode ?? '').trim()
    if (m) {
      query.modelCode = m
    }
  }
  return query
}

/**
 * 打开零价格：弹窗内选工厂 + 机种 + 核算月
 */
function openZeroPriceModal() {
  zeroPricePlantCode.value = queryForm.plantCode?.trim() || undefined
  zeroPriceModelCode.value = queryForm.modelCode?.trim() || undefined
  if (!zeroPriceModelCode.value && selectedModelGroup.value) {
    const m = String((selectedModelGroup.value as Record<string, unknown>).modelCode ?? '').trim()
    if (m) {
      zeroPriceModelCode.value = m
    }
  }
  zeroPriceModelSelectKey.value += 1
  zeroPriceCostingMonth.value = resolveDefaultCostingMonth()
  zeroPriceMonthModalVisible.value = true
}

/**
 * 工厂变更：清空机种并刷新下拉
 */
function handleZeroPricePlantChange() {
  zeroPriceModelCode.value = undefined
  zeroPriceModelSelectKey.value += 1
}

/**
 * 确认工厂 / 机种 / 核算月后打开零价格合并清单
 */
function handleZeroPriceMonthOk() {
  const plant = String(zeroPricePlantCode.value ?? '').trim()
  if (!plant) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.selectPlantRequired'))
    return
  }
  const model = String(zeroPriceModelCode.value ?? '').trim()
  if (!model) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.selectModelRequired'))
    return
  }
  const month = String(zeroPriceCostingMonth.value ?? '').trim()
  if (!month) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.costNeedMonth'))
    return
  }
  zeroPricePlantCode.value = plant
  zeroPriceModelCode.value = model
  zeroPriceMonthModalVisible.value = false
  zeroPriceModalVisible.value = true
}

/**
 * 打开计算/重置成本弹窗（弹窗内选工厂 + 核算月）
 * @param {boolean} forceRecalculate true=重置成本
 */
function openRecalculateModal(forceRecalculate: boolean) {
  recalculateModalForce.value = forceRecalculate
  recalculateForm.plantCode = queryForm.plantCode?.trim() || undefined
  if (!recalculateForm.plantCode && selectedModelGroup.value) {
    const p = String((selectedModelGroup.value as Record<string, unknown>).plantCode ?? '').trim()
    if (p) {
      recalculateForm.plantCode = p
    }
  }
  recalculateForm.costingMonth = resolveDefaultCostingMonth()
  recalculateForm.processRecordCount = DEFAULT_PROCESS_RECORD_COUNT
  recalculateModalVisible.value = true
}

/**
 * 弹窗确认：提交后台重算
 */
async function handleRecalculateModalOk() {
  const plant = String(recalculateForm.plantCode ?? '').trim()
  if (!plant) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.selectPlantRequired'))
    return
  }
  const month = String(recalculateForm.costingMonth ?? '').trim()
  if (!month) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.costNeedMonth'))
    return
  }
  const processRecordCount = Number(recalculateForm.processRecordCount ?? DEFAULT_PROCESS_RECORD_COUNT)
  if (!Number.isFinite(processRecordCount) || processRecordCount < 0) {
    message.warning(t('logistics.manufacturing.bom.material-cost.page.processRecordCountInvalid'))
    return
  }
  const query = buildRecalculateItemQuery(month)
  if (!query) return
  recalculatePending.value = true
  try {
    const submitted = await recalculateBomMaterialCostItemModelAverage(
      query,
      recalculateModalForce.value,
      Math.floor(processRecordCount),
    )
    recalculateModalVisible.value = false
    message.success(
      t(
        recalculateModalForce.value
          ? 'logistics.manufacturing.bom.material-cost.page.costRecalculateSubmitted'
          : 'logistics.manufacturing.bom.material-cost.page.costSumSubmitted',
        { month: submitted.processedMonth },
      ),
    )
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('logistics.manufacturing.bom.material-cost.page.costRecalculateFailed'))
  } finally {
    recalculatePending.value = false
  }
}

useBomMaterialCostItemRecalculateSignalR(async (event) => {
  recalculatePending.value = false
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
    bomMaterialCostProductPanelRef.value?.reload?.()
    return
  }
  message.error(
    event.errorMessage
      || t('logistics.manufacturing.bom.material-cost.page.costRecalculateFailed'),
  )
})

/** 打开导入对话框（明细源数据） */
function handleImport() {
  importVisible.value = true
}

/** 下载明细导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getBomMaterialCostItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入明细 Excel */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importBomMaterialCostItem(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新汇总与右侧明细 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()
  bomMaterialCostProductPanelRef.value?.reload?.()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}

/** 导出当前查询条件下的汇总 Excel */
async function handleExport() {
  try {
    loading.value = true
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
</script>
