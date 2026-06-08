<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/training-development/career-development -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工职业发展规划与技能评估管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-training-development-career-development">
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
      create-permission="humanresource:trainingdevelopment:careerdevelopment:create"
      update-permission="humanresource:trainingdevelopment:careerdevelopment:update"
      delete-permission="humanresource:trainingdevelopment:careerdevelopment:delete"
      import-permission="humanresource:trainingdevelopment:careerdevelopment:import"
      export-permission="humanresource:trainingdevelopment:careerdevelopment:export"
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
      :id-column-key="'careerDevelopmentId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getCareerDevelopmentId"
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
      <CareerDevelopmentForm
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
      :storage-key="'takt-query-fields-human-resource-training-development-career-development'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.careerDevelopment.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.employeeid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeName')">
      <a-form-item :label="t('entity.careerDevelopment.employeename')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.employeename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('skillCategory')">
      <a-form-item :label="t('entity.careerDevelopment.skillcategory')">
        <a-input
          v-model:value="advancedQueryForm.skillCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skillcategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('skillName')">
      <a-form-item :label="t('entity.careerDevelopment.skillname')">
        <a-input
          v-model:value="advancedQueryForm.skillName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skillname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentDateStart')">
      <a-form-item :label="t('entity.careerDevelopment.assessmentdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.assessmentDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.careerDevelopment.assessmentdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentDateEnd')">
      <a-form-item :label="t('entity.careerDevelopment.assessmentdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.assessmentDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.careerDevelopment.assessmentdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentMethod')">
      <a-form-item :label="t('entity.careerDevelopment.assessmentmethod')">
        <a-input
          v-model:value="advancedQueryForm.assessmentMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.assessmentmethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentScore')">
      <a-form-item :label="t('entity.careerDevelopment.assessmentscore')">
        <a-input-number
          v-model:value="advancedQueryForm.assessmentScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.assessmentscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('skillLevel')">
      <a-form-item :label="t('entity.careerDevelopment.skilllevel')">
        <a-input
          v-model:value="advancedQueryForm.skillLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.skilllevel') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetPosition')">
      <a-form-item :label="t('entity.careerDevelopment.targetposition')">
        <a-input
          v-model:value="advancedQueryForm.targetPosition"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.targetposition') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('developmentPlan')">
      <a-form-item :label="t('entity.careerDevelopment.developmentplan')">
        <a-input
          v-model:value="advancedQueryForm.developmentPlan"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.developmentplan') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementSuggestions')">
      <a-form-item :label="t('entity.careerDevelopment.improvementsuggestions')">
        <a-input
          v-model:value="advancedQueryForm.improvementSuggestions"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.improvementsuggestions') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextAssessmentDateStart')">
      <a-form-item :label="t('entity.careerDevelopment.nextassessmentdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextAssessmentDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.careerDevelopment.nextassessmentdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextAssessmentDateEnd')">
      <a-form-item :label="t('entity.careerDevelopment.nextassessmentdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextAssessmentDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.careerDevelopment.nextassessmentdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('careerDevelopmentStatus')">
      <a-form-item :label="t('entity.careerDevelopment.status')">
        <a-input-number
          v-model:value="advancedQueryForm.careerDevelopmentStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.careerDevelopment.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.careerDevelopment.relatedplant') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.careerDevelopment._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.careerDevelopment._self"
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
      :id-column-key="'careerDevelopmentId'"
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
 * 员工职业发展规划与技能评估管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/training-development/career-development
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import CareerDevelopmentForm from './components/career-development-form.vue'
import { getCareerDevelopmentList, getCareerDevelopmentById, createCareerDevelopment, updateCareerDevelopment, deleteCareerDevelopmentById, deleteCareerDevelopmentBatch, getCareerDevelopmentTemplate, importCareerDevelopment, exportCareerDevelopment } from '@/api/human-resource/training-development/career-development'
import type { CareerDevelopment, CareerDevelopmentQuery, CareerDevelopmentCreate, CareerDevelopmentUpdate } from '@/types/human-resource/training-development/career-development'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCareerDevelopment')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.careerDevelopment._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<CareerDevelopment[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<CareerDevelopment | null>(null)
/** 表格多选行 */
const selectedRows = ref<CareerDevelopment[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CareerDevelopment>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  employeeId: '',
  employeeName: '',
  skillCategory: '',
  skillName: '',
  assessmentDateStart: '',
  assessmentDateEnd: '',
  assessmentMethod: '',
  assessmentScore: undefined as number | undefined,
  skillLevel: '',
  targetPosition: '',
  developmentPlan: '',
  improvementSuggestions: '',
  nextAssessmentDateStart: '',
  nextAssessmentDateEnd: '',
  careerDevelopmentStatus: undefined as number | undefined,
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'employeeId', label: t('entity.careerDevelopment.employeeid') },
  { key: 'employeeName', label: t('entity.careerDevelopment.employeename') },
  { key: 'skillCategory', label: t('entity.careerDevelopment.skillcategory') },
  { key: 'skillName', label: t('entity.careerDevelopment.skillname') },
  { key: 'assessmentDateStart', label: t('entity.careerDevelopment.assessmentdatestart') },
  { key: 'assessmentDateEnd', label: t('entity.careerDevelopment.assessmentdateend') },
  { key: 'assessmentMethod', label: t('entity.careerDevelopment.assessmentmethod') },
  { key: 'assessmentScore', label: t('entity.careerDevelopment.assessmentscore') },
  { key: 'skillLevel', label: t('entity.careerDevelopment.skilllevel') },
  { key: 'targetPosition', label: t('entity.careerDevelopment.targetposition') },
  { key: 'developmentPlan', label: t('entity.careerDevelopment.developmentplan') },
  { key: 'improvementSuggestions', label: t('entity.careerDevelopment.improvementsuggestions') },
  { key: 'nextAssessmentDateStart', label: t('entity.careerDevelopment.nextassessmentdatestart') },
  { key: 'nextAssessmentDateEnd', label: t('entity.careerDevelopment.nextassessmentdateend') },
  { key: 'careerDevelopmentStatus', label: t('entity.careerDevelopment.status') },
  { key: 'relatedPlant', label: t('entity.careerDevelopment.relatedplant') },
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
const entityIdName = 'careerDevelopmentId'
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
    dataIndex: 'careerDevelopmentId',
    key: 'careerDevelopmentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'careerDevelopmentId') ?? ''
  },
  {
    title: t('entity.careerDevelopment.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.careerDevelopment.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.careerDevelopment.skillcategory'),
    dataIndex: 'skillCategory',
    key: 'skillCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'skillCategory') ?? ''
  },
  {
    title: t('entity.careerDevelopment.skillname'),
    dataIndex: 'skillName',
    key: 'skillName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'skillName') ?? ''
  },
  {
    title: t('entity.careerDevelopment.assessmentdate'),
    dataIndex: 'assessmentDate',
    key: 'assessmentDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'assessmentDate') ?? ''
  },
  {
    title: t('entity.careerDevelopment.assessmentmethod'),
    dataIndex: 'assessmentMethod',
    key: 'assessmentMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'assessmentMethod') ?? ''
  },
  {
    title: t('entity.careerDevelopment.assessmentscore'),
    dataIndex: 'assessmentScore',
    key: 'assessmentScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'assessmentScore') ?? ''
  },
  {
    title: t('entity.careerDevelopment.skilllevel'),
    dataIndex: 'skillLevel',
    key: 'skillLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'skillLevel') ?? ''
  },
  {
    title: t('entity.careerDevelopment.targetposition'),
    dataIndex: 'targetPosition',
    key: 'targetPosition',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'targetPosition') ?? ''
  },
  {
    title: t('entity.careerDevelopment.developmentplan'),
    dataIndex: 'developmentPlan',
    key: 'developmentPlan',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'developmentPlan') ?? ''
  },
  {
    title: t('entity.careerDevelopment.improvementsuggestions'),
    dataIndex: 'improvementSuggestions',
    key: 'improvementSuggestions',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'improvementSuggestions') ?? ''
  },
  {
    title: t('entity.careerDevelopment.nextassessmentdate'),
    dataIndex: 'nextAssessmentDate',
    key: 'nextAssessmentDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'nextAssessmentDate') ?? ''
  },
  {
    title: t('entity.careerDevelopment.status'),
    dataIndex: 'careerDevelopmentStatus',
    key: 'careerDevelopmentStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'careerDevelopmentStatus') ?? ''
  },
  {
    title: t('entity.careerDevelopment.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCareerDevelopmentField(record, 'relatedPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:trainingdevelopment:careerdevelopment:update',
        onClick: (record: CareerDevelopment) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:trainingdevelopment:careerdevelopment:delete',
        onClick: (record: CareerDevelopment) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCareerDevelopmentId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCareerDevelopmentField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CareerDevelopment[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: CareerDevelopment, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getCareerDevelopmentId(selectedRow.value) === getCareerDevelopmentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CareerDevelopment[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: CareerDevelopment) => ({
  onClick: () => {
    const key = getCareerDevelopmentId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getCareerDevelopmentId(item)))
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
    const params: CareerDevelopmentQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getCareerDevelopmentList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[CareerDevelopment] 加载数据失败', { error })
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
  skillCategory: '',
  skillName: '',
  assessmentDateStart: '',
  assessmentDateEnd: '',
  assessmentMethod: '',
  assessmentScore: undefined as number | undefined,
  skillLevel: '',
  targetPosition: '',
  developmentPlan: '',
  improvementSuggestions: '',
  nextAssessmentDateStart: '',
  nextAssessmentDateEnd: '',
  careerDevelopmentStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.careerDevelopment._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: CareerDevelopment) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.careerDevelopment._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.careerDevelopment._self') }))
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
      await updateCareerDevelopment(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.careerDevelopment._self') }))
    } else {
      await createCareerDevelopment(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.careerDevelopment._self') }))
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
  const res = await getCareerDevelopmentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCareerDevelopment(file, sheetName)
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
    const exportQuery: CareerDevelopmentQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportCareerDevelopment(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.careerDevelopment._self') }))
  } catch (error: any) {
    logger.error('[CareerDevelopment] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.careerDevelopment._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CareerDevelopment) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.careerDevelopment._self'), name: t('common.tip.this.target', { target: t('entity.careerDevelopment._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCareerDevelopmentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.careerDevelopment._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.careerDevelopment._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.careerDevelopment._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCareerDevelopmentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.careerDevelopment._self') }))
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
  skillCategory: '',
  skillName: '',
  assessmentDateStart: '',
  assessmentDateEnd: '',
  assessmentMethod: '',
  assessmentScore: undefined as number | undefined,
  skillLevel: '',
  targetPosition: '',
  developmentPlan: '',
  improvementSuggestions: '',
  nextAssessmentDateStart: '',
  nextAssessmentDateEnd: '',
  careerDevelopmentStatus: undefined as number | undefined,
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
.human-resource-training-development-career-development {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
