<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing-item -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：工艺路线明细表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
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
      create-permission="logistics:manufacturing:bom:routing:item:create"
      update-permission="logistics:manufacturing:bom:routing:item:update"
      delete-permission="logistics:manufacturing:bom:routing:item:delete"
      import-permission="logistics:manufacturing:bom:routing:item:import"
      export-permission="logistics:manufacturing:bom:routing:item:export"
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

    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getRoutingItemId"
      :master-row-selection="rowSelection"
      master-id-column-key="routingItemId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'processSegmentType'">
          <TaktDictTag
            :value="getRoutingItemField(record, 'processSegmentType')"
            dict-type="logistics_process_segment_type"
          />
        </template>
        <template v-else-if="column.key === 'isInspection'">
          <TaktDictTag
            :value="getRoutingItemField(record, 'isInspection')"
            dict-type="sys_yes_no_type"
          />
        </template>
        <template v-else-if="column.key === 'pointsToMinutesRate'">
          <TaktDictTag
            :value="getRoutingItemField(record, 'pointsToMinutesRate')"
            dict-type="logistics_points_to_minutes_rate"
          />
        </template>
      </template>
      <template #detail>
        <RoutingItemArgumentPanel
          ref="routingItemArgumentPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="1100px"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <RoutingItemForm
        :key="formData?.routingItemId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-bom-routing-item'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('routingId')">
      <a-form-item :label="t('entity.routingitem.routingid')">
        <a-input
          v-model:value="advancedQueryForm.routingId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.routingid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('routingCode')">
      <a-form-item :label="t('entity.routingitem.routingcode')">
        <a-input
          v-model:value="advancedQueryForm.routingCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.routingcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.routingitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseUnit')">
      <a-form-item :label="t('entity.routingitem.baseunit')">
        <a-input
          v-model:value="advancedQueryForm.baseUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.baseunit') })"
          show-count
          :maxlength="5"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseQuantity')">
      <a-form-item :label="t('entity.routingitem.basequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.baseQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.basequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardMinutes')">
      <a-form-item :label="t('entity.routingitem.standardminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.standardMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.standardminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('timeUnit')">
      <a-form-item :label="t('entity.routingitem.timeunit')">
        <a-input
          v-model:value="advancedQueryForm.timeUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.timeunit') })"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardShorts')">
      <a-form-item :label="t('entity.routingitem.standardshorts')">
        <a-input-number
          v-model:value="advancedQueryForm.standardShorts"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.standardshorts') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pointsUnit')">
      <a-form-item :label="t('entity.routingitem.pointsunit')">
        <a-input
          v-model:value="advancedQueryForm.pointsUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.pointsunit') })"
          show-count
          :maxlength="5"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pointsToMinutesRate')">
      <a-form-item :label="t('entity.routingitem.pointstominutesrate')">
        <TaktSelect
          v-model:value="advancedQueryForm.pointsToMinutesRate"
          dict-type="logistics_points_to_minutes_rate"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routingitem.pointstominutesrate') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedMinutes')">
      <a-form-item :label="t('entity.routingitem.convertedminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.convertedminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('setupMinutes')">
      <a-form-item :label="t('entity.routingitem.setupminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.setupMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.setupminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('teardownMinutes')">
      <a-form-item :label="t('entity.routingitem.teardownminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.teardownMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.teardownminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isInspection')">
      <a-form-item :label="t('entity.routingitem.isinspection')">
        <TaktSelect
          v-model:value="advancedQueryForm.isInspection"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routingitem.isinspection') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processDescription')">
      <a-form-item :label="t('entity.routingitem.processdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.processDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.routingitem.processdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processSegmentType')">
      <a-form-item :label="t('entity.routingitem.processsegmenttype')">
        <TaktSelect
          v-model:value="advancedQueryForm.processSegmentType"
          dict-type="logistics_process_segment_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routingitem.processsegmenttype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extJson')">
      <a-form-item :label="t('entity.routingitem.extjson')">
        <a-input
          v-model:value="advancedQueryForm.extJson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.extjson') })"
          show-count
          :maxlength="4000"
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
      :title="t('common.dialog.title.import', { entity: t('entity.routingitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.routingitem._self"
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
      :id-column-key="'routingItemId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 工艺路线明细表实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/routing-item
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import RoutingItemForm from './components/routing-item-form.vue'
import RoutingItemArgumentPanel from './components/routing-item-argument-panel.vue'
import { provideRoutingItemMasterContext } from './composables/use-routing-item-master-context'
import { getRoutingItemList, getRoutingItemById, createRoutingItem, updateRoutingItem, deleteRoutingItemById, deleteRoutingItemBatch, getRoutingItemTemplate, importRoutingItem, exportRoutingItem } from '@/api/logistics/manufacturing/bom/routing-item'
import type { RoutingItem, RoutingItemQuery } from '@/types/logistics/manufacturing/bom/routing-item'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktRoutingItem')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.routingitem._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<RoutingItem[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<RoutingItem | null>(null)
/** 表格多选行 */
const selectedRows = ref<RoutingItem[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<RoutingItem> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  routingId: '',
  routingCode: '',
  lineNumber: undefined as number | undefined,
  baseUnit: '',
  baseQuantity: undefined as number | undefined,
  standardMinutes: undefined as number | undefined,
  timeUnit: '',
  standardShorts: undefined as number | undefined,
  pointsUnit: '',
  pointsToMinutesRate: '' as string,
  convertedMinutes: undefined as number | undefined,
  setupMinutes: undefined as number | undefined,
  teardownMinutes: undefined as number | undefined,
  isInspection: undefined as number | undefined,
  processDescription: '',
  processSegmentType: undefined as number | undefined,
  extJson: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'routingId', label: t('entity.routingitem.routingid') },
  { key: 'routingCode', label: t('entity.routingitem.routingcode') },
  { key: 'lineNumber', label: t('entity.routingitem.linenumber') },
  { key: 'baseUnit', label: t('entity.routingitem.baseunit') },
  { key: 'baseQuantity', label: t('entity.routingitem.basequantity') },
  { key: 'standardMinutes', label: t('entity.routingitem.standardminutes') },
  { key: 'timeUnit', label: t('entity.routingitem.timeunit') },
  { key: 'standardShorts', label: t('entity.routingitem.standardshorts') },
  { key: 'pointsUnit', label: t('entity.routingitem.pointsunit') },
  { key: 'pointsToMinutesRate', label: t('entity.routingitem.pointstominutesrate') },
  { key: 'convertedMinutes', label: t('entity.routingitem.convertedminutes') },
  { key: 'setupMinutes', label: t('entity.routingitem.setupminutes') },
  { key: 'teardownMinutes', label: t('entity.routingitem.teardownminutes') },
  { key: 'isInspection', label: t('entity.routingitem.isinspection') },
  { key: 'processDescription', label: t('entity.routingitem.processdescription') },
  { key: 'processSegmentType', label: t('entity.routingitem.processsegmenttype') },
  { key: 'extJson', label: t('entity.routingitem.extjson') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'routingItemId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideRoutingItemMasterContext()
const routingItemArgumentPanelRef = ref<InstanceType<typeof RoutingItemArgumentPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {RoutingItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<RoutingItemQuery>): RoutingItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: RoutingItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof RoutingItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('routingId', form.routingId)
  assignTrimmed('routingCode', form.routingCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('baseUnit', form.baseUnit)
  if (form.baseQuantity !== undefined && form.baseQuantity !== null) {
    query.baseQuantity = form.baseQuantity
  }
  if (form.standardMinutes !== undefined && form.standardMinutes !== null) {
    query.standardMinutes = form.standardMinutes
  }
  assignTrimmed('timeUnit', form.timeUnit)
  if (form.standardShorts !== undefined && form.standardShorts !== null) {
    query.standardShorts = form.standardShorts
  }
  assignTrimmed('pointsUnit', form.pointsUnit)
  assignTrimmed('pointsToMinutesRate', form.pointsToMinutesRate)
  if (form.convertedMinutes !== undefined && form.convertedMinutes !== null) {
    query.convertedMinutes = form.convertedMinutes
  }
  if (form.setupMinutes !== undefined && form.setupMinutes !== null) {
    query.setupMinutes = form.setupMinutes
  }
  if (form.teardownMinutes !== undefined && form.teardownMinutes !== null) {
    query.teardownMinutes = form.teardownMinutes
  }
  if (form.isInspection !== undefined && form.isInspection !== null) {
    query.isInspection = form.isInspection
  }
  assignTrimmed('processDescription', form.processDescription)
  if (form.processSegmentType !== undefined && form.processSegmentType !== null) {
    query.processSegmentType = form.processSegmentType
  }
  assignTrimmed('extJson', form.extJson)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: RoutingItem | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getRoutingItemId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as RoutingItem
  const key = getRoutingItemId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}

/** 加载主表详情并回填当前页 dataSource */
async function loadRoutingItemDetail(record: RoutingItem): Promise<RoutingItem | null> {
  const id = getRoutingItemId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getRoutingItemById(id)
    const index = dataSource.value.findIndex((row) => getRoutingItemId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as RoutingItem
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'routingItemId',
    key: 'routingItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'routingItemId') ?? ''
  },
  {
    title: t('entity.routingitem.routingid'),
    dataIndex: 'routingId',
    key: 'routingId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'routingId') ?? ''
  },
  {
    title: t('entity.routingitem.routingcode'),
    dataIndex: 'routingCode',
    key: 'routingCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'routingCode') ?? ''
  },
  {
    title: t('entity.routingitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.routingitem.baseunit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'baseUnit') ?? ''
  },
  {
    title: t('entity.routingitem.basequantity'),
    dataIndex: 'baseQuantity',
    key: 'baseQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'baseQuantity') ?? ''
  },
  {
    title: t('entity.routingitem.standardminutes'),
    dataIndex: 'standardMinutes',
    key: 'standardMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'standardMinutes') ?? ''
  },
  {
    title: t('entity.routingitem.timeunit'),
    dataIndex: 'timeUnit',
    key: 'timeUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'timeUnit') ?? ''
  },
  {
    title: t('entity.routingitem.standardshorts'),
    dataIndex: 'standardShorts',
    key: 'standardShorts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'standardShorts') ?? ''
  },
  {
    title: t('entity.routingitem.pointsunit'),
    dataIndex: 'pointsUnit',
    key: 'pointsUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'pointsUnit') ?? ''
  },
  {
    title: t('entity.routingitem.pointstominutesrate'),
    dataIndex: 'pointsToMinutesRate',
    key: 'pointsToMinutesRate',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.routingitem.convertedminutes'),
    dataIndex: 'convertedMinutes',
    key: 'convertedMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'convertedMinutes') ?? ''
  },
  {
    title: t('entity.routingitem.setupminutes'),
    dataIndex: 'setupMinutes',
    key: 'setupMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'setupMinutes') ?? ''
  },
  {
    title: t('entity.routingitem.teardownminutes'),
    dataIndex: 'teardownMinutes',
    key: 'teardownMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'teardownMinutes') ?? ''
  },
  {
    title: t('entity.routingitem.isinspection'),
    dataIndex: 'isInspection',
    key: 'isInspection',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.routingitem.processdescription'),
    dataIndex: 'processDescription',
    key: 'processDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'processDescription') ?? ''
  },
  {
    title: t('entity.routingitem.processsegmenttype'),
    dataIndex: 'processSegmentType',
    key: 'processSegmentType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.routingitem.extjson'),
    dataIndex: 'extJson',
    key: 'extJson',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'extJson') ?? ''
  },
  {
    title: t('entity.routingitem.routing'),
    dataIndex: 'routing',
    key: 'routing',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getRoutingItemField(record, 'routing') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:routing:item:update',
        onClick: (record: RoutingItem) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:routing:item:delete',
        onClick: (record: RoutingItem) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getRoutingItemId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getRoutingItemField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: RoutingItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: RoutingItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getRoutingItemId(selectedRow.value) === getRoutingItemId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: RoutingItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getRoutingItemList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[RoutingItem] 加载数据失败', { error })
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
  advancedQueryForm.value = {
  routingId: '',
  routingCode: '',
  lineNumber: undefined as number | undefined,
  baseUnit: '',
  baseQuantity: undefined as number | undefined,
  standardMinutes: undefined as number | undefined,
  timeUnit: '',
  standardShorts: undefined as number | undefined,
  pointsUnit: '',
  pointsToMinutesRate: '' as string,
  convertedMinutes: undefined as number | undefined,
  setupMinutes: undefined as number | undefined,
  teardownMinutes: undefined as number | undefined,
  isInspection: undefined as number | undefined,
  processDescription: '',
  processSegmentType: undefined as number | undefined,
  extJson: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.routingitem._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: RoutingItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.routingitem._self') })
  formLoading.value = true
  try {
    const detail = await loadRoutingItemDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.routingitem._self') }))
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
      await updateRoutingItem(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.routingitem._self') }))
    } else {
      await createRoutingItem(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.routingitem._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  routingItemArgumentPanelRef.value?.reload?.()
    }
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
  const res = await getRoutingItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importRoutingItem(file, sheetName)
}

/** 导入完成回调：刷新列表并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportRoutingItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.routingitem._self') }))
  } catch (error: any) {
    logger.error('[RoutingItem] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.routingitem._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: RoutingItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.routingitem._self'), name: t('common.tip.this.target', { target: t('entity.routingitem._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteRoutingItemById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.routingitem._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.routingitem._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.routingitem._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteRoutingItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.routingitem._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
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
  advancedQueryForm.value = {
  routingId: '',
  routingCode: '',
  lineNumber: undefined as number | undefined,
  baseUnit: '',
  baseQuantity: undefined as number | undefined,
  standardMinutes: undefined as number | undefined,
  timeUnit: '',
  standardShorts: undefined as number | undefined,
  pointsUnit: '',
  pointsToMinutesRate: '' as string,
  convertedMinutes: undefined as number | undefined,
  setupMinutes: undefined as number | undefined,
  teardownMinutes: undefined as number | undefined,
  isInspection: undefined as number | undefined,
  processDescription: '',
  processSegmentType: undefined as number | undefined,
  extJson: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
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
