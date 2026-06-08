<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/scheme-metric -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：绩效方案指标管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-performance-scheme-metric">
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
      create-permission="humanresource:performance:schememetric:create"
      update-permission="humanresource:performance:schememetric:update"
      delete-permission="humanresource:performance:schememetric:delete"
      import-permission="humanresource:performance:schememetric:import"
      export-permission="humanresource:performance:schememetric:export"
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
      :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'schemeMetricId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSchemeMetricId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

    </TaktSingleTable>

    <!-- 分页组件 -->
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
      <SchemeMetricForm
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
      :storage-key="'takt-query-fields-human-resource-performance-scheme-metric'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('schemeCode')">
      <a-form-item :label="t('entity.schemeMetric.schemecode')">
        <a-input
          v-model:value="advancedQueryForm.schemeCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.schemecode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('schemeName')">
      <a-form-item :label="t('entity.schemeMetric.schemename')">
        <a-input
          v-model:value="advancedQueryForm.schemeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.schemename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicableDepartment')">
      <a-form-item :label="t('entity.schemeMetric.applicabledepartment')">
        <a-input
          v-model:value="advancedQueryForm.applicableDepartment"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.applicabledepartment') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cycleType')">
      <a-form-item :label="t('entity.schemeMetric.cycletype')">
        <a-input
          v-model:value="advancedQueryForm.cycleType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.cycletype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scoringStandard')">
      <a-form-item :label="t('entity.schemeMetric.scoringstandard')">
        <a-input
          v-model:value="advancedQueryForm.scoringStandard"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.scoringstandard') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('selfEvaluationWeight')">
      <a-form-item :label="t('entity.schemeMetric.selfevaluationweight')">
        <a-input-number
          v-model:value="advancedQueryForm.selfEvaluationWeight"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.selfevaluationweight') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supervisorWeight')">
      <a-form-item :label="t('entity.schemeMetric.supervisorweight')">
        <a-input-number
          v-model:value="advancedQueryForm.supervisorWeight"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.supervisorweight') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('metricCode')">
      <a-form-item :label="t('entity.schemeMetric.metriccode')">
        <a-input
          v-model:value="advancedQueryForm.metricCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.metriccode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('metricName')">
      <a-form-item :label="t('entity.schemeMetric.metricname')">
        <a-input
          v-model:value="advancedQueryForm.metricName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.metricname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('category')">
      <a-form-item :label="t('entity.schemeMetric.category')">
        <a-input
          v-model:value="advancedQueryForm.category"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.category') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('metricType')">
      <a-form-item :label="t('entity.schemeMetric.metrictype')">
        <a-input
          v-model:value="advancedQueryForm.metricType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.metrictype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scoringCriteria')">
      <a-form-item :label="t('entity.schemeMetric.scoringcriteria')">
        <a-input
          v-model:value="advancedQueryForm.scoringCriteria"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.scoringcriteria') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardWeight')">
      <a-form-item :label="t('entity.schemeMetric.standardweight')">
        <a-input-number
          v-model:value="advancedQueryForm.standardWeight"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.standardweight') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.schemeMetric.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.sortorder') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('schemeMetricStatus')">
      <a-form-item :label="t('entity.schemeMetric.status')">
        <a-input-number
          v-model:value="advancedQueryForm.schemeMetricStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.schemeMetric.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.schemeMetric.relatedplant') })"
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
      <div v-show="isFieldVisible('extFieldJson')">
      <a-form-item :label="t('common.page.entity.extfieldjson')">
        <a-input
          v-model:value="advancedQueryForm.extFieldJson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.schemeMetric._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.schemeMetric._self"
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
      :id-column-key="'schemeMetricId'"
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
 * 绩效方案指标管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/performance/scheme-metric
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import SchemeMetricForm from './components/scheme-metric-form.vue'
import { getSchemeMetricList, getSchemeMetricById, createSchemeMetric, updateSchemeMetric, deleteSchemeMetricById, deleteSchemeMetricBatch, getSchemeMetricTemplate, importSchemeMetric, exportSchemeMetric } from '@/api/human-resource/performance/scheme-metric'
import type { SchemeMetric, SchemeMetricQuery, SchemeMetricCreate, SchemeMetricUpdate } from '@/types/human-resource/performance/scheme-metric'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSchemeMetric')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.schemeMetric._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SchemeMetric[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SchemeMetric | null>(null)
/** 表格多选行 */
const selectedRows = ref<SchemeMetric[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SchemeMetric>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  schemeCode: '',
  schemeName: '',
  applicableDepartment: '',
  cycleType: '',
  scoringStandard: '',
  selfEvaluationWeight: undefined as number | undefined,
  supervisorWeight: undefined as number | undefined,
  metricCode: '',
  metricName: '',
  category: '',
  metricType: '',
  scoringCriteria: '',
  standardWeight: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  schemeMetricStatus: undefined as number | undefined,
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'schemeCode', label: t('entity.schemeMetric.schemecode') },
  { key: 'schemeName', label: t('entity.schemeMetric.schemename') },
  { key: 'applicableDepartment', label: t('entity.schemeMetric.applicabledepartment') },
  { key: 'cycleType', label: t('entity.schemeMetric.cycletype') },
  { key: 'scoringStandard', label: t('entity.schemeMetric.scoringstandard') },
  { key: 'selfEvaluationWeight', label: t('entity.schemeMetric.selfevaluationweight') },
  { key: 'supervisorWeight', label: t('entity.schemeMetric.supervisorweight') },
  { key: 'metricCode', label: t('entity.schemeMetric.metriccode') },
  { key: 'metricName', label: t('entity.schemeMetric.metricname') },
  { key: 'category', label: t('entity.schemeMetric.category') },
  { key: 'metricType', label: t('entity.schemeMetric.metrictype') },
  { key: 'scoringCriteria', label: t('entity.schemeMetric.scoringcriteria') },
  { key: 'standardWeight', label: t('entity.schemeMetric.standardweight') },
  { key: 'sortOrder', label: t('entity.schemeMetric.sortorder') },
  { key: 'schemeMetricStatus', label: t('entity.schemeMetric.status') },
  { key: 'relatedPlant', label: t('entity.schemeMetric.relatedplant') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extFieldJson', label: t('common.page.entity.extfieldjson') },
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
const entityIdName = 'schemeMetricId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)


/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})






/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'schemeMetricId',
    key: 'schemeMetricId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'schemeMetricId') ?? ''
  },
  {
    title: t('entity.schemeMetric.schemecode'),
    dataIndex: 'schemeCode',
    key: 'schemeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'schemeCode') ?? ''
  },
  {
    title: t('entity.schemeMetric.schemename'),
    dataIndex: 'schemeName',
    key: 'schemeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'schemeName') ?? ''
  },
  {
    title: t('entity.schemeMetric.applicabledepartment'),
    dataIndex: 'applicableDepartment',
    key: 'applicableDepartment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'applicableDepartment') ?? ''
  },
  {
    title: t('entity.schemeMetric.cycletype'),
    dataIndex: 'cycleType',
    key: 'cycleType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'cycleType') ?? ''
  },
  {
    title: t('entity.schemeMetric.scoringstandard'),
    dataIndex: 'scoringStandard',
    key: 'scoringStandard',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'scoringStandard') ?? ''
  },
  {
    title: t('entity.schemeMetric.selfevaluationweight'),
    dataIndex: 'selfEvaluationWeight',
    key: 'selfEvaluationWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'selfEvaluationWeight') ?? ''
  },
  {
    title: t('entity.schemeMetric.supervisorweight'),
    dataIndex: 'supervisorWeight',
    key: 'supervisorWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'supervisorWeight') ?? ''
  },
  {
    title: t('entity.schemeMetric.metriccode'),
    dataIndex: 'metricCode',
    key: 'metricCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'metricCode') ?? ''
  },
  {
    title: t('entity.schemeMetric.metricname'),
    dataIndex: 'metricName',
    key: 'metricName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'metricName') ?? ''
  },
  {
    title: t('entity.schemeMetric.category'),
    dataIndex: 'category',
    key: 'category',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'category') ?? ''
  },
  {
    title: t('entity.schemeMetric.metrictype'),
    dataIndex: 'metricType',
    key: 'metricType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'metricType') ?? ''
  },
  {
    title: t('entity.schemeMetric.scoringcriteria'),
    dataIndex: 'scoringCriteria',
    key: 'scoringCriteria',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'scoringCriteria') ?? ''
  },
  {
    title: t('entity.schemeMetric.standardweight'),
    dataIndex: 'standardWeight',
    key: 'standardWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'standardWeight') ?? ''
  },
  {
    title: t('entity.schemeMetric.status'),
    dataIndex: 'schemeMetricStatus',
    key: 'schemeMetricStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'schemeMetricStatus') ?? ''
  },
  {
    title: t('entity.schemeMetric.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSchemeMetricField(record, 'relatedPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:performance:schememetric:update',
        onClick: (record: SchemeMetric) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:performance:schememetric:delete',
        onClick: (record: SchemeMetric) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSchemeMetricId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSchemeMetricField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SchemeMetric[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SchemeMetric, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSchemeMetricId(selectedRow.value) === getSchemeMetricId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SchemeMetric[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SchemeMetric) => ({
  onClick: () => {
    const key = getSchemeMetricId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSchemeMetricId(item)))
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
    const kw = (queryKeyword.value ?? '').trim()
    const params: SchemeMetricQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getSchemeMetricList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SchemeMetric] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  schemeCode: '',
  schemeName: '',
  applicableDepartment: '',
  cycleType: '',
  scoringStandard: '',
  selfEvaluationWeight: undefined as number | undefined,
  supervisorWeight: undefined as number | undefined,
  metricCode: '',
  metricName: '',
  category: '',
  metricType: '',
  scoringCriteria: '',
  standardWeight: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  schemeMetricStatus: undefined as number | undefined,
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.schemeMetric._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: SchemeMetric) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.schemeMetric._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.schemeMetric._self') }))
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
      await updateSchemeMetric(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.schemeMetric._self') }))
    } else {
      await createSchemeMetric(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.schemeMetric._self') }))
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getSchemeMetricTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSchemeMetric(file, sheetName)
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
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: SchemeMetricQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportSchemeMetric(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.schemeMetric._self') }))
  } catch (error: any) {
    logger.error('[SchemeMetric] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.schemeMetric._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SchemeMetric) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.schemeMetric._self'), name: t('common.tip.this.target', { target: t('entity.schemeMetric._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSchemeMetricById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.schemeMetric._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.schemeMetric._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.schemeMetric._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSchemeMetricBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.schemeMetric._self') }))
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
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  schemeCode: '',
  schemeName: '',
  applicableDepartment: '',
  cycleType: '',
  scoringStandard: '',
  selfEvaluationWeight: undefined as number | undefined,
  supervisorWeight: undefined as number | undefined,
  metricCode: '',
  metricName: '',
  category: '',
  metricType: '',
  scoringCriteria: '',
  standardWeight: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  schemeMetricStatus: undefined as number | undefined,
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
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
/** 分页页码变更 */
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
/** 分页每页条数变更 */
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

<style scoped lang="css">
.human-resource-performance-scheme-metric {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
