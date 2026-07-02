<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-item -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt物料清单明细实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:manufacturing:bom:bill:of:material:item:create"
      update-permission="logistics:manufacturing:bom:bill:of:material:item:update"
      delete-permission="logistics:manufacturing:bom:bill:of:material:item:delete"
      import-permission="logistics:manufacturing:bom:bill:of:material:item:import"
      export-permission="logistics:manufacturing:bom:bill:of:material:item:export"
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
      :master-row-key="getBillOfMaterialItemId"
      :master-row-selection="rowSelection"
      master-id-column-key="billOfMaterialItemId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #detail>
        <BillOfMaterialSubstitutePanel
          ref="billOfMaterialSubstitutePanelRef"
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
      <BillOfMaterialItemForm
        :key="formData?.billOfMaterialItemId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-bom-bill-of-material-item'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('billOfMaterialId')">
      <a-form-item :label="t('entity.billofmaterialitem.billofmaterialid')">
        <a-input
          v-model:value="advancedQueryForm.billOfMaterialId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.billofmaterialid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomCode')">
      <a-form-item :label="t('entity.billofmaterialitem.bomcode')">
        <a-input
          v-model:value="advancedQueryForm.bomCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.bomcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.billofmaterialitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialId')">
      <a-form-item :label="t('entity.billofmaterialitem.materialid')">
        <a-input
          v-model:value="advancedQueryForm.materialId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.materialid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.billofmaterialitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('usageQuantity')">
      <a-form-item :label="t('entity.billofmaterialitem.usagequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.usageQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.usagequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialUnit')">
      <a-form-item :label="t('entity.billofmaterialitem.materialunit')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialUnit"
          dict-type="logistics_unit_of_measure_code"
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.materialunit') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scrapRate')">
      <a-form-item :label="t('entity.billofmaterialitem.scraprate')">
        <a-input-number
          v-model:value="advancedQueryForm.scrapRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.scraprate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualUsageQuantity')">
      <a-form-item :label="t('entity.billofmaterialitem.actualusagequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.actualUsageQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.actualusagequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operationSeq')">
      <a-form-item :label="t('entity.billofmaterialitem.operationseq')">
        <a-input-number
          v-model:value="advancedQueryForm.operationSeq"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.operationseq') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workCenter')">
      <a-form-item :label="t('entity.billofmaterialitem.workcenter')">
        <a-input
          v-model:value="advancedQueryForm.workCenter"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.workcenter') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('position')">
      <a-form-item :label="t('entity.billofmaterialitem.position')">
        <a-input
          v-model:value="advancedQueryForm.position"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.position') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substituteGroup')">
      <a-form-item :label="t('entity.billofmaterialitem.substitutegroup')">
        <a-input
          v-model:value="advancedQueryForm.substituteGroup"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.substitutegroup') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substitutePriority')">
      <a-form-item :label="t('entity.billofmaterialitem.substitutepriority')">
        <a-input-number
          v-model:value="advancedQueryForm.substitutePriority"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialitem.substitutepriority') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isOptional')">
      <a-form-item :label="t('entity.billofmaterialitem.isoptional')">
        <TaktSelect
          v-model:value="advancedQueryForm.isOptional"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.isoptional') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isPhantom')">
      <a-form-item :label="t('entity.billofmaterialitem.isphantom')">
        <TaktSelect
          v-model:value="advancedQueryForm.isPhantom"
          dict-type="sys_yes_no_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialitem.isphantom') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.billofmaterialitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.billofmaterialitem._self"
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
      :id-column-key="'billOfMaterialItemId'"
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
 * Takt物料清单明细实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/bill-of-material-item
 */
import { ref, computed, onMounted, h } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import BillOfMaterialItemForm from './components/bill-of-material-item-form.vue'
import BillOfMaterialSubstitutePanel from './components/bill-of-material-substitute-panel.vue'
import { provideBillOfMaterialItemMasterContext } from './composables/use-bill-of-material-item-master-context'
import { getBillOfMaterialItemList, getBillOfMaterialItemById, createBillOfMaterialItem, updateBillOfMaterialItem, deleteBillOfMaterialItemById, deleteBillOfMaterialItemBatch, getBillOfMaterialItemTemplate, importBillOfMaterialItem, exportBillOfMaterialItem } from '@/api/logistics/manufacturing/bom/bill-of-material-item'
import type { BillOfMaterialItem, BillOfMaterialItemQuery } from '@/types/logistics/manufacturing/bom/bill-of-material-item'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import TaktDictTag from '@/components/common/takt-dict-tag/index.vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBillOfMaterialItem')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.billofmaterialitem._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<BillOfMaterialItem[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<BillOfMaterialItem | null>(null)
/** 表格多选行 */
const selectedRows = ref<BillOfMaterialItem[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<BillOfMaterialItem> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  billOfMaterialId: '',
  bomCode: '',
  lineNumber: undefined as number | undefined,
  materialId: '',
  materialCode: '',
  usageQuantity: undefined as number | undefined,
  materialUnit: '',
  scrapRate: undefined as number | undefined,
  actualUsageQuantity: undefined as number | undefined,
  operationSeq: undefined as number | undefined,
  workCenter: '',
  position: '',
  substituteGroup: '',
  substitutePriority: undefined as number | undefined,
  isOptional: undefined as number | undefined,
  isPhantom: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'billOfMaterialId', label: t('entity.billofmaterialitem.billofmaterialid') },
  { key: 'bomCode', label: t('entity.billofmaterialitem.bomcode') },
  { key: 'lineNumber', label: t('entity.billofmaterialitem.linenumber') },
  { key: 'materialId', label: t('entity.billofmaterialitem.materialid') },
  { key: 'materialCode', label: t('entity.billofmaterialitem.materialcode') },
  { key: 'usageQuantity', label: t('entity.billofmaterialitem.usagequantity') },
  { key: 'materialUnit', label: t('entity.billofmaterialitem.materialunit') },
  { key: 'scrapRate', label: t('entity.billofmaterialitem.scraprate') },
  { key: 'actualUsageQuantity', label: t('entity.billofmaterialitem.actualusagequantity') },
  { key: 'operationSeq', label: t('entity.billofmaterialitem.operationseq') },
  { key: 'workCenter', label: t('entity.billofmaterialitem.workcenter') },
  { key: 'position', label: t('entity.billofmaterialitem.position') },
  { key: 'substituteGroup', label: t('entity.billofmaterialitem.substitutegroup') },
  { key: 'substitutePriority', label: t('entity.billofmaterialitem.substitutepriority') },
  { key: 'isOptional', label: t('entity.billofmaterialitem.isoptional') },
  { key: 'isPhantom', label: t('entity.billofmaterialitem.isphantom') },
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
const entityIdName = 'billOfMaterialItemId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideBillOfMaterialItemMasterContext()
const billOfMaterialSubstitutePanelRef = ref<InstanceType<typeof BillOfMaterialSubstitutePanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {BillOfMaterialItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<BillOfMaterialItemQuery>): BillOfMaterialItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: BillOfMaterialItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof BillOfMaterialItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('billOfMaterialId', form.billOfMaterialId)
  assignTrimmed('bomCode', form.bomCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('materialId', form.materialId)
  assignTrimmed('materialCode', form.materialCode)
  if (form.usageQuantity !== undefined && form.usageQuantity !== null) {
    query.usageQuantity = form.usageQuantity
  }
  assignTrimmed('materialUnit', form.materialUnit)
  if (form.scrapRate !== undefined && form.scrapRate !== null) {
    query.scrapRate = form.scrapRate
  }
  if (form.actualUsageQuantity !== undefined && form.actualUsageQuantity !== null) {
    query.actualUsageQuantity = form.actualUsageQuantity
  }
  if (form.operationSeq !== undefined && form.operationSeq !== null) {
    query.operationSeq = form.operationSeq
  }
  assignTrimmed('workCenter', form.workCenter)
  assignTrimmed('position', form.position)
  assignTrimmed('substituteGroup', form.substituteGroup)
  if (form.substitutePriority !== undefined && form.substitutePriority !== null) {
    query.substitutePriority = form.substitutePriority
  }
  if (form.isOptional !== undefined && form.isOptional !== null) {
    query.isOptional = form.isOptional
  }
  if (form.isPhantom !== undefined && form.isPhantom !== null) {
    query.isPhantom = form.isPhantom
  }
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: BillOfMaterialItem | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getBillOfMaterialItemId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as BillOfMaterialItem
  const key = getBillOfMaterialItemId(row)
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
async function loadBillOfMaterialItemDetail(record: BillOfMaterialItem): Promise<BillOfMaterialItem | null> {
  const id = getBillOfMaterialItemId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getBillOfMaterialItemById(id)
    const index = dataSource.value.findIndex((row) => getBillOfMaterialItemId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as BillOfMaterialItem
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
    dataIndex: 'billOfMaterialItemId',
    key: 'billOfMaterialItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'billOfMaterialItemId') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.billofmaterialid'),
    dataIndex: 'billOfMaterialId',
    key: 'billOfMaterialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'billOfMaterialId') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.bomcode'),
    dataIndex: 'bomCode',
    key: 'bomCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'bomCode') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.materialid'),
    dataIndex: 'materialId',
    key: 'materialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'materialId') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.usagequantity'),
    dataIndex: 'usageQuantity',
    key: 'usageQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'usageQuantity') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.materialunit'),
    dataIndex: 'materialUnit',
    key: 'materialUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => h(TaktDictTag, {
      dictType: 'logistics_unit_of_measure_code',
      value: getBillOfMaterialItemField(record, 'materialUnit'),
    })
  },
  {
    title: t('entity.billofmaterialitem.scraprate'),
    dataIndex: 'scrapRate',
    key: 'scrapRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'scrapRate') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.actualusagequantity'),
    dataIndex: 'actualUsageQuantity',
    key: 'actualUsageQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'actualUsageQuantity') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.operationseq'),
    dataIndex: 'operationSeq',
    key: 'operationSeq',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'operationSeq') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.workcenter'),
    dataIndex: 'workCenter',
    key: 'workCenter',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'workCenter') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.position'),
    dataIndex: 'position',
    key: 'position',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'position') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.substitutegroup'),
    dataIndex: 'substituteGroup',
    key: 'substituteGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'substituteGroup') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.substitutepriority'),
    dataIndex: 'substitutePriority',
    key: 'substitutePriority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'substitutePriority') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.isoptional'),
    dataIndex: 'isOptional',
    key: 'isOptional',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => h(TaktDictTag, {
      dictType: 'sys_yes_no_type',
      value: getBillOfMaterialItemField(record, 'isOptional'),
    })
  },
  {
    title: t('entity.billofmaterialitem.isphantom'),
    dataIndex: 'isPhantom',
    key: 'isPhantom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => h(TaktDictTag, {
      dictType: 'sys_yes_no_type',
      value: getBillOfMaterialItemField(record, 'isPhantom'),
    })
  },
  {
    title: t('entity.billofmaterialitem.bom'),
    dataIndex: 'bom',
    key: 'bom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'bom') ?? ''
  },
  {
    title: t('entity.billofmaterialitem.materialplant'),
    dataIndex: 'materialPlant',
    key: 'materialPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'materialPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:bill:of:material:item:update',
        onClick: (record: BillOfMaterialItem) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:bill:of:material:item:delete',
        onClick: (record: BillOfMaterialItem) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getBillOfMaterialItemId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getBillOfMaterialItemField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: BillOfMaterialItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: BillOfMaterialItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getBillOfMaterialItemId(selectedRow.value) === getBillOfMaterialItemId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: BillOfMaterialItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getBillOfMaterialItemList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[BillOfMaterialItem] 加载数据失败', { error })
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
  billOfMaterialId: '',
  bomCode: '',
  lineNumber: undefined as number | undefined,
  materialId: '',
  materialCode: '',
  usageQuantity: undefined as number | undefined,
  materialUnit: '',
  scrapRate: undefined as number | undefined,
  actualUsageQuantity: undefined as number | undefined,
  operationSeq: undefined as number | undefined,
  workCenter: '',
  position: '',
  substituteGroup: '',
  substitutePriority: undefined as number | undefined,
  isOptional: undefined as number | undefined,
  isPhantom: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.billofmaterialitem._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: BillOfMaterialItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.billofmaterialitem._self') })
  formLoading.value = true
  try {
    const detail = await loadBillOfMaterialItemDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.billofmaterialitem._self') }))
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
      await updateBillOfMaterialItem(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.billofmaterialitem._self') }))
    } else {
      await createBillOfMaterialItem(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.billofmaterialitem._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  billOfMaterialSubstitutePanelRef.value?.reload?.()
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
  const res = await getBillOfMaterialItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importBillOfMaterialItem(file, sheetName)
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
    const exportMeta = await exportBillOfMaterialItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.billofmaterialitem._self') }))
  } catch (error: any) {
    logger.error('[BillOfMaterialItem] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.billofmaterialitem._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: BillOfMaterialItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.billofmaterialitem._self'), name: t('common.tip.this.target', { target: t('entity.billofmaterialitem._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBillOfMaterialItemById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.billofmaterialitem._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.billofmaterialitem._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.billofmaterialitem._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteBillOfMaterialItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.billofmaterialitem._self') }))
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
  billOfMaterialId: '',
  bomCode: '',
  lineNumber: undefined as number | undefined,
  materialId: '',
  materialCode: '',
  usageQuantity: undefined as number | undefined,
  materialUnit: '',
  scrapRate: undefined as number | undefined,
  actualUsageQuantity: undefined as number | undefined,
  operationSeq: undefined as number | undefined,
  workCenter: '',
  position: '',
  substituteGroup: '',
  substitutePriority: undefined as number | undefined,
  isOptional: undefined as number | undefined,
  isPhantom: undefined as number | undefined,
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
