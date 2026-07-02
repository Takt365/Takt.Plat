<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/cost-element-change-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：成本要素实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="accounting:controlling:cost:element:create"
      update-permission="accounting:controlling:cost:element:update"
      delete-permission="accounting:controlling:cost:element:delete"
      import-permission="accounting:controlling:cost:element:import"
      export-permission="accounting:controlling:cost:element:export"
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
      :master-row-key="getCostElementId"
      :master-row-selection="rowSelection"
      master-id-column-key="costElementId"
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
        <template v-if="column.key === 'costElementStatus'">
          <a-switch
            :checked="getCostElementField(record, 'costElementStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleCostElementStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'costElementCategory'">
          <TaktDictTag
            :value="getCostElementDictValue(record, 'costElementCategory')"
            dict-type="accounting_cost_element_category"
          />
        </template>
        <template v-else-if="column.key === 'costElementType'">
          <TaktDictTag
            :value="getCostElementDictValue(record, 'costElementType')"
            dict-type="accounting_cost_element_type"
          />
        </template>
      </template>
      <template #detail>
        <CostElementChangeLogPanel
          ref="costElementChangeLogPanelRef"
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
      <CostElementForm
        :key="formData?.costElementId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-controlling-cost-element-change-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('costElementCode')">
      <a-form-item :label="t('entity.costelement.code')">
        <a-input
          v-model:value="advancedQueryForm.costElementCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.costelement.code') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementName')">
      <a-form-item :label="t('entity.costelement.name')">
        <a-input
          v-model:value="advancedQueryForm.costElementName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.costelement.name') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementCategory')">
      <a-form-item :label="t('entity.costelement.category')">
        <TaktSelect
          v-model:value="advancedQueryForm.costElementCategory"
          dict-type="accounting_cost_element_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.category') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementType')">
      <a-form-item :label="t('entity.costelement.type')">
        <TaktSelect
          v-model:value="advancedQueryForm.costElementType"
          dict-type="accounting_cost_element_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.type') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentId')">
      <a-form-item :label="t('entity.costelement.parentid')">
        <a-input
          v-model:value="advancedQueryForm.parentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.costelement.parentid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementLevel')">
      <a-form-item :label="t('entity.costelement.level')">
        <a-input-number
          v-model:value="advancedQueryForm.costElementLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.costelement.level') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costElementStatus')">
      <a-form-item :label="t('entity.costelement.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.costElementStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromStart')">
      <a-form-item :label="t('entity.costelement.validfromstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.validfromstart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromEnd')">
      <a-form-item :label="t('entity.costelement.validfromend')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.validfromend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToStart')">
      <a-form-item :label="t('entity.costelement.validtostart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.validtostart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToEnd')">
      <a-form-item :label="t('entity.costelement.validtoend')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.costelement.validtoend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.costelement._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.costelement._self"
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
      :id-column-key="'costElementId'"
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
 * 成本要素实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/controlling/cost-element-change-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import CostElementForm from './components/cost-element-form.vue'
import CostElementChangeLogPanel from './components/cost-element-change-log-panel.vue'
import { provideCostElementMasterContext } from './composables/use-cost-element-master-context'
import { getCostElementList, getCostElementById, createCostElement, updateCostElement, deleteCostElementById, deleteCostElementBatch, getCostElementTemplate, importCostElement, exportCostElement, updateCostElementStatus } from '@/api/accounting/controlling/cost-element'
import type { CostElement, CostElementQuery } from '@/types/accounting/controlling/cost-element'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCostElement')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.costelement._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<CostElement[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<CostElement | null>(null)
/** 表格多选行 */
const selectedRows = ref<CostElement[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CostElement> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  costElementCode: '',
  costElementName: '',
  costElementType: undefined as number | undefined,
  costElementCategory: undefined as number | undefined,
  parentId: '',
  costElementLevel: undefined as number | undefined,
  costElementStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'costElementCode', label: t('entity.costelement.code') },
  { key: 'costElementName', label: t('entity.costelement.name') },
  { key: 'costElementType', label: t('entity.costelement.type') },
  { key: 'costElementCategory', label: t('entity.costelement.category') },
  { key: 'parentId', label: t('entity.costelement.parentid') },
  { key: 'costElementLevel', label: t('entity.costelement.level') },
  { key: 'costElementStatus', label: t('entity.costelement.status') },
  { key: 'validFromStart', label: t('entity.costelement.validfromstart') },
  { key: 'validFromEnd', label: t('entity.costelement.validfromend') },
  { key: 'validToStart', label: t('entity.costelement.validtostart') },
  { key: 'validToEnd', label: t('entity.costelement.validtoend') },
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
const entityIdName = 'costElementId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideCostElementMasterContext()
const costElementChangeLogPanelRef = ref<InstanceType<typeof CostElementChangeLogPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {CostElementQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CostElementQuery>): CostElementQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CostElementQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CostElementQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('costElementCode', form.costElementCode)
  assignTrimmed('costElementName', form.costElementName)
  if (form.costElementType !== undefined && form.costElementType !== null) {
    query.costElementType = form.costElementType
  }
  if (form.costElementCategory !== undefined && form.costElementCategory !== null) {
    query.costElementCategory = form.costElementCategory
  }
  assignTrimmed('parentId', form.parentId)
  if (form.costElementLevel !== undefined && form.costElementLevel !== null) {
    query.costElementLevel = form.costElementLevel
  }
  if (form.costElementStatus !== undefined && form.costElementStatus !== null) {
    query.costElementStatus = form.costElementStatus
  }
  assignTrimmed('validFromStart', form.validFromStart)
  assignTrimmed('validFromEnd', form.validFromEnd)
  assignTrimmed('validToStart', form.validToStart)
  assignTrimmed('validToEnd', form.validToEnd)
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
function syncMasterSelection(record: CostElement | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getCostElementId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as CostElement
  const key = getCostElementId(row)
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
async function loadCostElementDetail(record: CostElement): Promise<CostElement | null> {
  const id = getCostElementId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getCostElementById(id)
    const index = dataSource.value.findIndex((row) => getCostElementId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as CostElement
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
    dataIndex: 'costElementId',
    key: 'costElementId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCostElementField(record, 'costElementId') ?? ''
  },
  {
    title: t('entity.costelement.code'),
    dataIndex: 'costElementCode',
    key: 'costElementCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostElementField(record, 'costElementCode') ?? ''
  },
  {
    title: t('entity.costelement.name'),
    dataIndex: 'costElementName',
    key: 'costElementName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostElementField(record, 'costElementName') ?? ''
  },
  {
    title: t('entity.costelement.type'),
    dataIndex: 'costElementType',
    key: 'costElementType',
    width: 140,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.costelement.category'),
    dataIndex: 'costElementCategory',
    key: 'costElementCategory',
    width: 220,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.costelement.parentid'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostElementField(record, 'parentId') ?? ''
  },
  {
    title: t('entity.costelement.level'),
    dataIndex: 'costElementLevel',
    key: 'costElementLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostElementField(record, 'costElementLevel') ?? ''
  },
  {
    title: t('entity.costelement.status'),
    dataIndex: 'costElementStatus',
    key: 'costElementStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.costelement.validfrom'),
    dataIndex: 'validFrom',
    key: 'validFrom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostElementField(record, 'validFrom') ?? ''
  },
  {
    title: t('entity.costelement.validto'),
    dataIndex: 'validTo',
    key: 'validTo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCostElementField(record, 'validTo') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:controlling:cost:element:update',
        onClick: (record: CostElement) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:controlling:cost:element:delete',
        onClick: (record: CostElement) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCostElementId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCostElementField = (record: any, field: string): any => record?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const getCostElementDictValue = (
  record: Record<string, unknown>,
  field: string,
): string | number | undefined => {
  const value = record?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CostElement[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: CostElement, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getCostElementId(selectedRow.value) === getCostElementId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CostElement[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getCostElementList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[CostElement] 加载数据失败', { error })
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
  costElementCode: '',
  costElementName: '',
  costElementType: undefined as number | undefined,
  costElementCategory: undefined as number | undefined,
  parentId: '',
  costElementLevel: undefined as number | undefined,
  costElementStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.costelement._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: CostElement) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.costelement._self') })
  formLoading.value = true
  try {
    const detail = await loadCostElementDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.costelement._self') }))
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
      await updateCostElement(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.costelement._self') }))
    } else {
      await createCostElement(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.costelement._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  costElementChangeLogPanelRef.value?.reload?.()
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
  const res = await getCostElementTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCostElement(file, sheetName)
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
    const exportMeta = await exportCostElement(
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
    message.success(t('common.feedback.export.success', { target: t('entity.costelement._self') }))
  } catch (error: any) {
    logger.error('[CostElement] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.costelement._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CostElement) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.costelement._self'), name: t('common.tip.this.target', { target: t('entity.costelement._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCostElementById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.costelement._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.costelement._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.costelement._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCostElementBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.costelement._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleCostElementStatusChange(record: CostElement, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getCostElementField(record, 'costElementStatus')
  const id = getCostElementId(record)
  const row = dataSource.value.find((item) => getCostElementId(item) === id)
  if (row) {
    row.costElementStatus = newVal
  }
  try {
    await updateCostElementStatus({ costElementId: id, costElementStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.costElementStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
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
  costElementCode: '',
  costElementName: '',
  costElementType: undefined as number | undefined,
  costElementCategory: undefined as number | undefined,
  parentId: '',
  costElementLevel: undefined as number | undefined,
  costElementStatus: undefined as number | undefined,
  validFromStart: '',
  validFromEnd: '',
  validToStart: '',
  validToEnd: '',
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
