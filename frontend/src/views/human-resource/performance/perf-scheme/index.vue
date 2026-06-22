<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/perf-scheme -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：绩效方案指标管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-performance-perf-scheme">
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
      create-permission="humanresource:talent:jobposting:create"
      update-permission="humanresource:talent:jobposting:update"
      delete-permission="humanresource:talent:jobposting:delete"
      import-permission="humanresource:talent:jobposting:import"
      export-permission="humanresource:talent:jobposting:export"
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
      :id-column-key="'perfSchemeId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPerfSchemeId"
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
      <PerfSchemeForm
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
      :storage-key="'takt-query-fields-human-resource-performance-perf-scheme'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('schemeCode')">
      <a-form-item :label="t('entity.perfscheme.schemecode')">
        <a-input
          v-model:value="advancedQueryForm.schemeCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.schemecode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('schemeName')">
      <a-form-item :label="t('entity.perfscheme.schemename')">
        <a-input
          v-model:value="advancedQueryForm.schemeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.schemename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicableDepartment')">
      <a-form-item :label="t('entity.perfscheme.applicabledepartment')">
        <a-input
          v-model:value="advancedQueryForm.applicableDepartment"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.applicabledepartment') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cycleType')">
      <a-form-item :label="t('entity.perfscheme.cycletype')">
        <a-input
          v-model:value="advancedQueryForm.cycleType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.cycletype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scoringStandard')">
      <a-form-item :label="t('entity.perfscheme.scoringstandard')">
        <a-input
          v-model:value="advancedQueryForm.scoringStandard"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.scoringstandard') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('selfEvaluationWeight')">
      <a-form-item :label="t('entity.perfscheme.selfevaluationweight')">
        <a-input-number
          v-model:value="advancedQueryForm.selfEvaluationWeight"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.selfevaluationweight') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supervisorWeight')">
      <a-form-item :label="t('entity.perfscheme.supervisorweight')">
        <a-input-number
          v-model:value="advancedQueryForm.supervisorWeight"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.supervisorweight') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('metricCode')">
      <a-form-item :label="t('entity.perfscheme.metriccode')">
        <a-input
          v-model:value="advancedQueryForm.metricCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.metriccode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('metricName')">
      <a-form-item :label="t('entity.perfscheme.metricname')">
        <a-input
          v-model:value="advancedQueryForm.metricName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.metricname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('category')">
      <a-form-item :label="t('entity.perfscheme.category')">
        <a-input
          v-model:value="advancedQueryForm.category"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.category') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('metricType')">
      <a-form-item :label="t('entity.perfscheme.metrictype')">
        <a-input
          v-model:value="advancedQueryForm.metricType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.metrictype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scoringCriteria')">
      <a-form-item :label="t('entity.perfscheme.scoringcriteria')">
        <a-input
          v-model:value="advancedQueryForm.scoringCriteria"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.scoringcriteria') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardWeight')">
      <a-form-item :label="t('entity.perfscheme.standardweight')">
        <a-input-number
          v-model:value="advancedQueryForm.standardWeight"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.standardweight') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.perfscheme.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.sortorder') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('schemeMetricStatus')">
      <a-form-item :label="t('entity.perfscheme.schememetricstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.schemeMetricStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.schememetricstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.perfscheme.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.relatedplant') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('common.page.entity.ExtField')">
        <a-input
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.perfscheme._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.perfscheme._self"
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
      :id-column-key="'perfSchemeId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
/**
 * 绩效方案指标管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/performance/perf-scheme
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import PerfSchemeForm from './components/perf-scheme-form.vue'
import { getPerfSchemeList, getPerfSchemeById, createPerfScheme, updatePerfScheme, deletePerfSchemeById, deletePerfSchemeBatch, getPerfSchemeTemplate, importPerfScheme, exportPerfScheme } from '@/api/human-resource/performance/perf-scheme'
import type { PerfScheme, PerfSchemeQuery, PerfSchemeCreate, PerfSchemeUpdate } from '@/types/human-resource/performance/perf-scheme'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPerfScheme')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.perfscheme._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PerfScheme[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<PerfScheme | null>(null)
/** 表格多选行 */
const selectedRows = ref<PerfScheme[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PerfScheme>>({})
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
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'schemeCode', label: t('entity.perfscheme.schemecode') },
  { key: 'schemeName', label: t('entity.perfscheme.schemename') },
  { key: 'applicableDepartment', label: t('entity.perfscheme.applicabledepartment') },
  { key: 'cycleType', label: t('entity.perfscheme.cycletype') },
  { key: 'scoringStandard', label: t('entity.perfscheme.scoringstandard') },
  { key: 'selfEvaluationWeight', label: t('entity.perfscheme.selfevaluationweight') },
  { key: 'supervisorWeight', label: t('entity.perfscheme.supervisorweight') },
  { key: 'metricCode', label: t('entity.perfscheme.metriccode') },
  { key: 'metricName', label: t('entity.perfscheme.metricname') },
  { key: 'category', label: t('entity.perfscheme.category') },
  { key: 'metricType', label: t('entity.perfscheme.metrictype') },
  { key: 'scoringCriteria', label: t('entity.perfscheme.scoringcriteria') },
  { key: 'standardWeight', label: t('entity.perfscheme.standardweight') },
  { key: 'sortOrder', label: t('entity.perfscheme.sortorder') },
  { key: 'schemeMetricStatus', label: t('entity.perfscheme.schememetricstatus') },
  { key: 'relatedPlant', label: t('entity.perfscheme.relatedplant') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.ExtField') },
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
const entityIdName = 'perfSchemeId'
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
    dataIndex: 'perfSchemeId',
    key: 'perfSchemeId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'perfSchemeId') ?? ''
  },
  {
    title: t('entity.perfscheme.schemecode'),
    dataIndex: 'schemeCode',
    key: 'schemeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'schemeCode') ?? ''
  },
  {
    title: t('entity.perfscheme.schemename'),
    dataIndex: 'schemeName',
    key: 'schemeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'schemeName') ?? ''
  },
  {
    title: t('entity.perfscheme.applicabledepartment'),
    dataIndex: 'applicableDepartment',
    key: 'applicableDepartment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'applicableDepartment') ?? ''
  },
  {
    title: t('entity.perfscheme.cycletype'),
    dataIndex: 'cycleType',
    key: 'cycleType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'cycleType') ?? ''
  },
  {
    title: t('entity.perfscheme.scoringstandard'),
    dataIndex: 'scoringStandard',
    key: 'scoringStandard',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'scoringStandard') ?? ''
  },
  {
    title: t('entity.perfscheme.selfevaluationweight'),
    dataIndex: 'selfEvaluationWeight',
    key: 'selfEvaluationWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'selfEvaluationWeight') ?? ''
  },
  {
    title: t('entity.perfscheme.supervisorweight'),
    dataIndex: 'supervisorWeight',
    key: 'supervisorWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'supervisorWeight') ?? ''
  },
  {
    title: t('entity.perfscheme.metriccode'),
    dataIndex: 'metricCode',
    key: 'metricCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'metricCode') ?? ''
  },
  {
    title: t('entity.perfscheme.metricname'),
    dataIndex: 'metricName',
    key: 'metricName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'metricName') ?? ''
  },
  {
    title: t('entity.perfscheme.category'),
    dataIndex: 'category',
    key: 'category',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'category') ?? ''
  },
  {
    title: t('entity.perfscheme.metrictype'),
    dataIndex: 'metricType',
    key: 'metricType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'metricType') ?? ''
  },
  {
    title: t('entity.perfscheme.scoringcriteria'),
    dataIndex: 'scoringCriteria',
    key: 'scoringCriteria',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'scoringCriteria') ?? ''
  },
  {
    title: t('entity.perfscheme.standardweight'),
    dataIndex: 'standardWeight',
    key: 'standardWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'standardWeight') ?? ''
  },
  {
    title: t('entity.perfscheme.schememetricstatus'),
    dataIndex: 'schemeMetricStatus',
    key: 'schemeMetricStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'schemeMetricStatus') ?? ''
  },
  {
    title: t('entity.perfscheme.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfSchemeField(record, 'relatedPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:talent:jobposting:update',
        onClick: (record: PerfScheme) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:talent:jobposting:delete',
        onClick: (record: PerfScheme) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPerfSchemeId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPerfSchemeField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PerfScheme[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PerfScheme, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPerfSchemeId(selectedRow.value) === getPerfSchemeId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PerfScheme[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: PerfScheme) => ({
  onClick: () => {
    const key = getPerfSchemeId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPerfSchemeId(item)))
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
    const params: PerfSchemeQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPerfSchemeList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PerfScheme] 加载数据失败', { error })
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
  ExtField: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.perfscheme._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: PerfScheme) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.perfscheme._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.perfscheme._self') }))
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
      await updatePerfScheme(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.perfscheme._self') }))
    } else {
      await createPerfScheme(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.perfscheme._self') }))
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
  const res = await getPerfSchemeTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPerfScheme(file, sheetName)
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
    const exportQuery: PerfSchemeQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPerfScheme(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.perfscheme._self') }))
  } catch (error: any) {
    logger.error('[PerfScheme] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.perfscheme._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PerfScheme) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.perfscheme._self'), name: t('common.tip.this.target', { target: t('entity.perfscheme._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePerfSchemeById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.perfscheme._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.perfscheme._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.perfscheme._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePerfSchemeBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.perfscheme._self') }))
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
  ExtField: '',
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
.human-resource-performance-perf-scheme {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
