<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-document-item -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt物料交易主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:materials:materialdocument:create"
      update-permission="logistics:materials:materialdocument:update"
      delete-permission="logistics:materials:materialdocument:delete"
      import-permission="logistics:materials:materialdocument:import"
      export-permission="logistics:materials:materialdocument:export"
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
      :master-row-key="getMaterialDocumentId"
      :master-row-selection="rowSelection"
      master-id-column-key="materialDocumentId"
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
        <template v-if="column.key === 'movementType'">
          <TaktDictTag
            :value="getMaterialTransactionField(record, 'movementType')"
            dict-type="logistics_movement_type"
          />
        </template>
      </template>
      <template #detail>
        <MaterialTransactionItemPanel
          ref="materialDocumentItemPanelRef"
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
      <MaterialTransactionForm
        :key="formData?.materialDocumentId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-materials-material-document-item'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.materialdocument.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDocumentCode')">
      <a-form-item :label="t('entity.materialdocument.code')">
        <a-input
          v-model:value="advancedQueryForm.materialDocumentCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('transactionDateStart')">
      <a-form-item :label="t('entity.materialdocument.transactiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.transactionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialdocument.transactiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('transactionDateEnd')">
      <a-form-item :label="t('entity.materialdocument.transactiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.transactionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialdocument.transactiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('movementType')">
      <a-form-item :label="t('entity.materialdocument.movementtype')">
        <TaktSelect
          v-model:value="advancedQueryForm.movementType"
          dict-type="logistics_movement_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialdocument.movementtype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCode')">
      <a-form-item :label="t('entity.materialdocument.sourcecode')">
        <a-input
          v-model:value="advancedQueryForm.sourceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.sourcecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('partnerCode')">
      <a-form-item :label="t('entity.materialdocument.partnercode')">
        <a-input
          v-model:value="advancedQueryForm.partnerCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.partnercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('partnerName')">
      <a-form-item :label="t('entity.materialdocument.partnername')">
        <a-input
          v-model:value="advancedQueryForm.partnerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.partnername') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warehouseCode')">
      <a-form-item :label="t('entity.materialdocument.warehousecode')">
        <a-input
          v-model:value="advancedQueryForm.warehouseCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.warehousecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('locationCode')">
      <a-form-item :label="t('entity.materialdocument.locationcode')">
        <a-input
          v-model:value="advancedQueryForm.locationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.locationcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetWarehouseCode')">
      <a-form-item :label="t('entity.materialdocument.targetwarehousecode')">
        <a-input
          v-model:value="advancedQueryForm.targetWarehouseCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.targetwarehousecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetLocationCode')">
      <a-form-item :label="t('entity.materialdocument.targetlocationcode')">
        <a-input
          v-model:value="advancedQueryForm.targetLocationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.targetlocationcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedCompany')">
      <a-form-item :label="t('entity.materialdocument.relatedcompany')">
        <a-input
          v-model:value="advancedQueryForm.relatedCompany"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.relatedcompany') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQuantity')">
      <a-form-item :label="t('entity.materialdocument.totalquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.totalquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('transactionStatus')">
      <a-form-item :label="t('entity.materialdocument.transactionstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.transactionStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.transactionstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postedDateStart')">
      <a-form-item :label="t('entity.materialdocument.posteddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.postedDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialdocument.posteddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postedDateEnd')">
      <a-form-item :label="t('entity.materialdocument.posteddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.postedDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialdocument.posteddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postedBy')">
      <a-form-item :label="t('entity.materialdocument.postedby')">
        <a-input
          v-model:value="advancedQueryForm.postedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialdocument.postedby') })"
          show-count
          :maxlength="50"
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
      :title="t('common.dialog.title.import', { entity: t('entity.materialdocument._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.materialdocument._self"
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
      :id-column-key="'materialDocumentId'"
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
 * Takt物料交易主表实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-document-item
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import MaterialTransactionForm from './components/material-document-form.vue'
import MaterialTransactionItemPanel from './components/material-document-item-panel.vue'
import { provideMaterialTransactionMasterContext } from './composables/use-material-document-master-context'
import { getMaterialTransactionList, getMaterialTransactionById, createMaterialTransaction, updateMaterialTransaction, deleteMaterialTransactionById, deleteMaterialTransactionBatch, getMaterialTransactionTemplate, importMaterialTransaction, exportMaterialTransaction, updateMaterialTransactionStatus } from '@/api/logistics/materials/material-document'
import type { MaterialTransaction, MaterialTransactionQuery } from '@/types/logistics/materials/material-document'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterialDocument')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.materialdocument._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<MaterialTransaction[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<MaterialTransaction | null>(null)
/** 表格多选行 */
const selectedRows = ref<MaterialTransaction[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<MaterialTransaction> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  materialDocumentCode: '',
  transactionDateStart: '',
  transactionDateEnd: '',
  movementType: undefined as string | undefined,
  sourceCode: '',
  partnerCode: '',
  partnerName: '',
  warehouseCode: '',
  locationCode: '',
  targetWarehouseCode: '',
  targetLocationCode: '',
  relatedCompany: '',
  totalQuantity: undefined as number | undefined,
  transactionStatus: undefined as number | undefined,
  postedDateStart: '',
  postedDateEnd: '',
  postedBy: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.materialdocument.plantcode') },
  { key: 'materialDocumentCode', label: t('entity.materialdocument.code') },
  { key: 'transactionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.materialdocument.transactiondate')) },
  { key: 'transactionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.materialdocument.transactiondate')) },
  { key: 'movementType', label: t('entity.materialdocument.movementtype') },
  { key: 'sourceCode', label: t('entity.materialdocument.sourcecode') },
  { key: 'partnerCode', label: t('entity.materialdocument.partnercode') },
  { key: 'partnerName', label: t('entity.materialdocument.partnername') },
  { key: 'warehouseCode', label: t('entity.materialdocument.warehousecode') },
  { key: 'locationCode', label: t('entity.materialdocument.locationcode') },
  { key: 'targetWarehouseCode', label: t('entity.materialdocument.targetwarehousecode') },
  { key: 'targetLocationCode', label: t('entity.materialdocument.targetlocationcode') },
  { key: 'relatedCompany', label: t('entity.materialdocument.relatedcompany') },
  { key: 'totalQuantity', label: t('entity.materialdocument.totalquantity') },
  { key: 'transactionStatus', label: t('entity.materialdocument.transactionstatus') },
  { key: 'postedDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.materialdocument.posteddate')) },
  { key: 'postedDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.materialdocument.posteddate')) },
  { key: 'postedBy', label: t('entity.materialdocument.postedby') },
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
const entityIdName = 'materialDocumentId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideMaterialTransactionMasterContext()
const materialDocumentItemPanelRef = ref<InstanceType<typeof MaterialTransactionItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MaterialTransactionQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaterialTransactionQuery>): MaterialTransactionQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaterialTransactionQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaterialTransactionQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('materialDocumentCode', form.materialDocumentCode)
  assignTrimmed('transactionDateStart', form.transactionDateStart)
  assignTrimmed('transactionDateEnd', form.transactionDateEnd)
  if (form.transactionDirection !== undefined && form.transactionDirection !== null) {
    query.transactionDirection = form.transactionDirection
  }
  if (form.transactionType !== undefined && form.transactionType !== null) {
    query.transactionType = form.transactionType
  }
  if (form.businessAction !== undefined && form.businessAction !== null) {
    query.businessAction = form.businessAction
  }
  assignTrimmed('sourceCode', form.sourceCode)
  assignTrimmed('partnerCode', form.partnerCode)
  assignTrimmed('partnerName', form.partnerName)
  assignTrimmed('warehouseCode', form.warehouseCode)
  assignTrimmed('locationCode', form.locationCode)
  assignTrimmed('targetWarehouseCode', form.targetWarehouseCode)
  assignTrimmed('targetLocationCode', form.targetLocationCode)
  assignTrimmed('relatedCompany', form.relatedCompany)
  if (form.totalQuantity !== undefined && form.totalQuantity !== null) {
    query.totalQuantity = form.totalQuantity
  }
  if (form.transactionStatus !== undefined && form.transactionStatus !== null) {
    query.transactionStatus = form.transactionStatus
  }
  assignTrimmed('postedDateStart', form.postedDateStart)
  assignTrimmed('postedDateEnd', form.postedDateEnd)
  assignTrimmed('postedBy', form.postedBy)
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
function syncMasterSelection(record: MaterialTransaction | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getMaterialDocumentId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as MaterialTransaction
  const key = getMaterialDocumentId(row)
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
async function loadMaterialTransactionDetail(record: MaterialTransaction): Promise<MaterialTransaction | null> {
  const id = getMaterialDocumentId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getMaterialTransactionById(id)
    const index = dataSource.value.findIndex((row) => getMaterialDocumentId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as MaterialTransaction
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
    dataIndex: 'materialDocumentId',
    key: 'materialDocumentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'materialDocumentId') ?? ''
  },
  {
    title: t('entity.materialdocument.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.materialdocument.code'),
    dataIndex: 'materialDocumentCode',
    key: 'materialDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'materialDocumentCode') ?? ''
  },
  {
    title: t('entity.materialdocument.transactiondate'),
    dataIndex: 'transactionDate',
    key: 'transactionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'transactionDate') ?? ''
  },
  {
    title: t('entity.materialdocument.transactiondirection'),
    dataIndex: 'transactionDirection',
    key: 'transactionDirection',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'transactionDirection') ?? ''
  },
  {
    title: t('entity.materialdocument.transactiontype'),
    dataIndex: 'transactionType',
    key: 'transactionType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.materialdocument.businessaction'),
    dataIndex: 'businessAction',
    key: 'businessAction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'businessAction') ?? ''
  },
  {
    title: t('entity.materialdocument.sourcecode'),
    dataIndex: 'sourceCode',
    key: 'sourceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'sourceCode') ?? ''
  },
  {
    title: t('entity.materialdocument.partnercode'),
    dataIndex: 'partnerCode',
    key: 'partnerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'partnerCode') ?? ''
  },
  {
    title: t('entity.materialdocument.partnername'),
    dataIndex: 'partnerName',
    key: 'partnerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'partnerName') ?? ''
  },
  {
    title: t('entity.materialdocument.warehousecode'),
    dataIndex: 'warehouseCode',
    key: 'warehouseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'warehouseCode') ?? ''
  },
  {
    title: t('entity.materialdocument.locationcode'),
    dataIndex: 'locationCode',
    key: 'locationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'locationCode') ?? ''
  },
  {
    title: t('entity.materialdocument.targetwarehousecode'),
    dataIndex: 'targetWarehouseCode',
    key: 'targetWarehouseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'targetWarehouseCode') ?? ''
  },
  {
    title: t('entity.materialdocument.targetlocationcode'),
    dataIndex: 'targetLocationCode',
    key: 'targetLocationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'targetLocationCode') ?? ''
  },
  {
    title: t('entity.materialdocument.relatedcompany'),
    dataIndex: 'relatedCompany',
    key: 'relatedCompany',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'relatedCompany') ?? ''
  },
  {
    title: t('entity.materialdocument.totalquantity'),
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'totalQuantity') ?? ''
  },
  {
    title: t('entity.materialdocument.transactionstatus'),
    dataIndex: 'transactionStatus',
    key: 'transactionStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'transactionStatus') ?? ''
  },
  {
    title: t('entity.materialdocument.posteddate'),
    dataIndex: 'postedDate',
    key: 'postedDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'postedDate') ?? ''
  },
  {
    title: t('entity.materialdocument.postedby'),
    dataIndex: 'postedBy',
    key: 'postedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialTransactionField(record, 'postedBy') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:materialdocument:update',
        onClick: (record: MaterialTransaction) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:materialdocument:delete',
        onClick: (record: MaterialTransaction) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMaterialDocumentId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMaterialTransactionField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaterialTransaction[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: MaterialTransaction, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (getMaterialDocumentId(selectedRow.value) === getMaterialDocumentId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaterialTransaction[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getMaterialTransactionList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[MaterialTransaction] 加载数据失败', { error })
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
  plantCode: '',
  materialDocumentCode: '',
  transactionDateStart: '',
  transactionDateEnd: '',
  movementType: undefined as string | undefined,
  sourceCode: '',
  partnerCode: '',
  partnerName: '',
  warehouseCode: '',
  locationCode: '',
  targetWarehouseCode: '',
  targetLocationCode: '',
  relatedCompany: '',
  totalQuantity: undefined as number | undefined,
  transactionStatus: undefined as number | undefined,
  postedDateStart: '',
  postedDateEnd: '',
  postedBy: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.materialdocument._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: MaterialTransaction) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.materialdocument._self') })
  formLoading.value = true
  try {
    const detail = await loadMaterialTransactionDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.materialdocument._self') }))
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
      await updateMaterialTransaction(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.materialdocument._self') }))
    } else {
      await createMaterialTransaction(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.materialdocument._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  materialDocumentItemPanelRef.value?.reload?.()
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
  const res = await getMaterialTransactionTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaterialTransaction(file, sheetName)
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
    const exportMeta = await exportMaterialTransaction(
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
    message.success(t('common.feedback.export.success', { target: t('entity.materialdocument._self') }))
  } catch (error: any) {
    logger.error('[MaterialTransaction] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.materialdocument._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MaterialTransaction) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.materialdocument._self'), name: t('common.tip.this.target', { target: t('entity.materialdocument._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialTransactionById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.materialdocument._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.materialdocument._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.materialdocument._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMaterialTransactionBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.materialdocument._self') }))
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
  plantCode: '',
  materialDocumentCode: '',
  transactionDateStart: '',
  transactionDateEnd: '',
  movementType: undefined as string | undefined,
  sourceCode: '',
  partnerCode: '',
  partnerName: '',
  warehouseCode: '',
  locationCode: '',
  targetWarehouseCode: '',
  targetLocationCode: '',
  relatedCompany: '',
  totalQuantity: undefined as number | undefined,
  transactionStatus: undefined as number | undefined,
  postedDateStart: '',
  postedDateEnd: '',
  postedBy: '',
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
