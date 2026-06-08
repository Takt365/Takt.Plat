<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/assessment -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工绩效考核评估管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-performance-assessment">
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
      create-permission="humanresource:performance:assessment:create"
      update-permission="humanresource:performance:assessment:update"
      delete-permission="humanresource:performance:assessment:delete"
      import-permission="humanresource:performance:assessment:import"
      export-permission="humanresource:performance:assessment:export"
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
      :id-column-key="'assessmentId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getAssessmentId"
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
      <AssessmentForm
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
      :storage-key="'takt-query-fields-human-resource-performance-assessment'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.assessment.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.employeeid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeName')">
      <a-form-item :label="t('entity.assessment.employeename')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.employeename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentPeriod')">
      <a-form-item :label="t('entity.assessment.period')">
        <a-input
          v-model:value="advancedQueryForm.assessmentPeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.period') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentDateStart')">
      <a-form-item :label="t('entity.assessment.datestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.assessmentDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assessment.datestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentDateEnd')">
      <a-form-item :label="t('entity.assessment.dateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.assessmentDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assessment.dateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('schemeMetricId')">
      <a-form-item :label="t('entity.assessment.schememetricid')">
        <a-input
          v-model:value="advancedQueryForm.schemeMetricId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.schememetricid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('selfScore')">
      <a-form-item :label="t('entity.assessment.selfscore')">
        <a-input-number
          v-model:value="advancedQueryForm.selfScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.selfscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('selfEvaluationNotes')">
      <a-form-item :label="t('entity.assessment.selfevaluationnotes')">
        <a-textarea
          v-model:value="advancedQueryForm.selfEvaluationNotes"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assessment.selfevaluationnotes') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supervisorScore')">
      <a-form-item :label="t('entity.assessment.supervisorscore')">
        <a-input-number
          v-model:value="advancedQueryForm.supervisorScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.supervisorscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supervisorComments')">
      <a-form-item :label="t('entity.assessment.supervisorcomments')">
        <a-input
          v-model:value="advancedQueryForm.supervisorComments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.supervisorcomments') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('finalScore')">
      <a-form-item :label="t('entity.assessment.finalscore')">
        <a-input-number
          v-model:value="advancedQueryForm.finalScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.finalscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('performanceGrade')">
      <a-form-item :label="t('entity.assessment.performancegrade')">
        <a-input
          v-model:value="advancedQueryForm.performanceGrade"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.performancegrade') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reviewerId')">
      <a-form-item :label="t('entity.assessment.reviewerid')">
        <a-input
          v-model:value="advancedQueryForm.reviewerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.reviewerid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewDateStart')">
      <a-form-item :label="t('entity.assessment.interviewdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.interviewDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assessment.interviewdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewDateEnd')">
      <a-form-item :label="t('entity.assessment.interviewdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.interviewDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assessment.interviewdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewNotes')">
      <a-form-item :label="t('entity.assessment.interviewnotes')">
        <a-textarea
          v-model:value="advancedQueryForm.interviewNotes"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assessment.interviewnotes') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentStatus')">
      <a-form-item :label="t('entity.assessment.status')">
        <a-input-number
          v-model:value="advancedQueryForm.assessmentStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.assessment.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assessment.relatedplant') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.assessment._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.assessment._self"
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
      :id-column-key="'assessmentId'"
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
 * 员工绩效考核评估管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/performance/assessment
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import AssessmentForm from './components/assessment-form.vue'
import { getAssessmentList, getAssessmentById, createAssessment, updateAssessment, deleteAssessmentById, deleteAssessmentBatch, getAssessmentTemplate, importAssessment, exportAssessment } from '@/api/human-resource/performance/assessment'
import type { Assessment, AssessmentQuery, AssessmentCreate, AssessmentUpdate } from '@/types/human-resource/performance/assessment'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAssessment')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.assessment._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Assessment[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Assessment | null>(null)
/** 表格多选行 */
const selectedRows = ref<Assessment[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Assessment>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  employeeId: '',
  employeeName: '',
  assessmentPeriod: '',
  assessmentDateStart: '',
  assessmentDateEnd: '',
  schemeMetricId: '',
  selfScore: undefined as number | undefined,
  selfEvaluationNotes: '',
  supervisorScore: undefined as number | undefined,
  supervisorComments: '',
  finalScore: undefined as number | undefined,
  performanceGrade: '',
  reviewerId: '',
  interviewDateStart: '',
  interviewDateEnd: '',
  interviewNotes: '',
  assessmentStatus: undefined as number | undefined,
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'employeeId', label: t('entity.assessment.employeeid') },
  { key: 'employeeName', label: t('entity.assessment.employeename') },
  { key: 'assessmentPeriod', label: t('entity.assessment.period') },
  { key: 'assessmentDateStart', label: t('entity.assessment.datestart') },
  { key: 'assessmentDateEnd', label: t('entity.assessment.dateend') },
  { key: 'schemeMetricId', label: t('entity.assessment.schememetricid') },
  { key: 'selfScore', label: t('entity.assessment.selfscore') },
  { key: 'selfEvaluationNotes', label: t('entity.assessment.selfevaluationnotes') },
  { key: 'supervisorScore', label: t('entity.assessment.supervisorscore') },
  { key: 'supervisorComments', label: t('entity.assessment.supervisorcomments') },
  { key: 'finalScore', label: t('entity.assessment.finalscore') },
  { key: 'performanceGrade', label: t('entity.assessment.performancegrade') },
  { key: 'reviewerId', label: t('entity.assessment.reviewerid') },
  { key: 'interviewDateStart', label: t('entity.assessment.interviewdatestart') },
  { key: 'interviewDateEnd', label: t('entity.assessment.interviewdateend') },
  { key: 'interviewNotes', label: t('entity.assessment.interviewnotes') },
  { key: 'assessmentStatus', label: t('entity.assessment.status') },
  { key: 'relatedPlant', label: t('entity.assessment.relatedplant') },
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
const entityIdName = 'assessmentId'
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
    dataIndex: 'assessmentId',
    key: 'assessmentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'assessmentId') ?? ''
  },
  {
    title: t('entity.assessment.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.assessment.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.assessment.period'),
    dataIndex: 'assessmentPeriod',
    key: 'assessmentPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'assessmentPeriod') ?? ''
  },
  {
    title: t('entity.assessment.date'),
    dataIndex: 'assessmentDate',
    key: 'assessmentDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'assessmentDate') ?? ''
  },
  {
    title: t('entity.assessment.schememetricid'),
    dataIndex: 'schemeMetricId',
    key: 'schemeMetricId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'schemeMetricId') ?? ''
  },
  {
    title: t('entity.assessment.schememetricname'),
    dataIndex: 'schemeMetricName',
    key: 'schemeMetricName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'schemeMetricName') ?? ''
  },
  {
    title: t('entity.assessment.selfscore'),
    dataIndex: 'selfScore',
    key: 'selfScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'selfScore') ?? ''
  },
  {
    title: t('entity.assessment.selfevaluationnotes'),
    dataIndex: 'selfEvaluationNotes',
    key: 'selfEvaluationNotes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'selfEvaluationNotes') ?? ''
  },
  {
    title: t('entity.assessment.supervisorscore'),
    dataIndex: 'supervisorScore',
    key: 'supervisorScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'supervisorScore') ?? ''
  },
  {
    title: t('entity.assessment.supervisorcomments'),
    dataIndex: 'supervisorComments',
    key: 'supervisorComments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'supervisorComments') ?? ''
  },
  {
    title: t('entity.assessment.finalscore'),
    dataIndex: 'finalScore',
    key: 'finalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'finalScore') ?? ''
  },
  {
    title: t('entity.assessment.performancegrade'),
    dataIndex: 'performanceGrade',
    key: 'performanceGrade',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'performanceGrade') ?? ''
  },
  {
    title: t('entity.assessment.reviewerid'),
    dataIndex: 'reviewerId',
    key: 'reviewerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'reviewerId') ?? ''
  },
  {
    title: t('entity.assessment.reviewername'),
    dataIndex: 'reviewerName',
    key: 'reviewerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'reviewerName') ?? ''
  },
  {
    title: t('entity.assessment.interviewdate'),
    dataIndex: 'interviewDate',
    key: 'interviewDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'interviewDate') ?? ''
  },
  {
    title: t('entity.assessment.interviewnotes'),
    dataIndex: 'interviewNotes',
    key: 'interviewNotes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'interviewNotes') ?? ''
  },
  {
    title: t('entity.assessment.status'),
    dataIndex: 'assessmentStatus',
    key: 'assessmentStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'assessmentStatus') ?? ''
  },
  {
    title: t('entity.assessment.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssessmentField(record, 'relatedPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:performance:assessment:update',
        onClick: (record: Assessment) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:performance:assessment:delete',
        onClick: (record: Assessment) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getAssessmentId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getAssessmentField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Assessment[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Assessment, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getAssessmentId(selectedRow.value) === getAssessmentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Assessment[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Assessment) => ({
  onClick: () => {
    const key = getAssessmentId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getAssessmentId(item)))
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
    const params: AssessmentQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getAssessmentList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Assessment] 加载数据失败', { error })
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
  employeeId: '',
  employeeName: '',
  assessmentPeriod: '',
  assessmentDateStart: '',
  assessmentDateEnd: '',
  schemeMetricId: '',
  selfScore: undefined as number | undefined,
  selfEvaluationNotes: '',
  supervisorScore: undefined as number | undefined,
  supervisorComments: '',
  finalScore: undefined as number | undefined,
  performanceGrade: '',
  reviewerId: '',
  interviewDateStart: '',
  interviewDateEnd: '',
  interviewNotes: '',
  assessmentStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.assessment._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: Assessment) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.assessment._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.assessment._self') }))
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
      await updateAssessment(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.assessment._self') }))
    } else {
      await createAssessment(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.assessment._self') }))
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
  const res = await getAssessmentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAssessment(file, sheetName)
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
    const exportQuery: AssessmentQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportAssessment(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.assessment._self') }))
  } catch (error: any) {
    logger.error('[Assessment] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.assessment._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Assessment) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.assessment._self'), name: t('common.tip.this.target', { target: t('entity.assessment._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssessmentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.assessment._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.assessment._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.assessment._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAssessmentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.assessment._self') }))
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
  employeeId: '',
  employeeName: '',
  assessmentPeriod: '',
  assessmentDateStart: '',
  assessmentDateEnd: '',
  schemeMetricId: '',
  selfScore: undefined as number | undefined,
  selfEvaluationNotes: '',
  supervisorScore: undefined as number | undefined,
  supervisorComments: '',
  finalScore: undefined as number | undefined,
  performanceGrade: '',
  reviewerId: '',
  interviewDateStart: '',
  interviewDateEnd: '',
  interviewNotes: '',
  assessmentStatus: undefined as number | undefined,
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
.human-resource-performance-assessment {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
