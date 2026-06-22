<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/compensation/salary-formula -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：薪资计算公式管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-compensation-salary-formula">
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
      create-permission="humanresource:performance:analysis:create"
      update-permission="humanresource:performance:analysis:update"
      delete-permission="humanresource:performance:analysis:delete"
      import-permission="humanresource:performance:analysis:import"
      export-permission="humanresource:performance:analysis:export"
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
      :id-column-key="'salaryFormulaId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSalaryFormulaId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'formulaStep'">
          <TaktDictTag
            :value="getSalaryFormulaField(record, 'formulaStep')"
            dict-type="hr_salary_formula_step_type"
          />
        </template>
        <template v-else-if="column.key === 'formulaStatus'">
          <TaktDictTag
            :value="getSalaryFormulaField(record, 'formulaStatus')"
            dict-type="sys_normal_disable_status"
          />
        </template>
      </template>

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
      <SalaryFormulaForm
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
      :storage-key="'takt-query-fields-human-resource-compensation-salary-formula'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('setCode')">
      <a-form-item :label="t('entity.salaryformula.setcode')">
        <a-input
          v-model:value="advancedQueryForm.setCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.setcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('setName')">
      <a-form-item :label="t('entity.salaryformula.setname')">
        <a-input
          v-model:value="advancedQueryForm.setName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.setname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('PayrollId')">
      <a-form-item :label="t('entity.salaryformula.PayrollId')">
        <a-input
          v-model:value="advancedQueryForm.PayrollId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.PayrollId') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formulaCode')">
      <a-form-item :label="t('entity.salaryformula.formulacode')">
        <a-input
          v-model:value="advancedQueryForm.formulaCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.formulacode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formulaName')">
      <a-form-item :label="t('entity.salaryformula.formulaname')">
        <a-input
          v-model:value="advancedQueryForm.formulaName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.formulaname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formulaStep')">
      <a-form-item :label="t('entity.salaryformula.formulastep')">
        <TaktSelect
          v-model:value="advancedQueryForm.formulaStep"
          dict-type="hr_salary_formula_step_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.formulastep') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.salaryformula.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.sortorder') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetField')">
      <a-form-item :label="t('entity.salaryformula.targetfield')">
        <a-input
          v-model:value="advancedQueryForm.targetField"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.targetfield') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formulaExpression')">
      <a-form-item :label="t('entity.salaryformula.formulaexpression')">
        <a-input
          v-model:value="advancedQueryForm.formulaExpression"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.formulaexpression') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stepDescription')">
      <a-form-item :label="t('entity.salaryformula.stepdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.stepDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.salaryformula.stepdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="t('entity.salaryformula.effectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.effectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="t('entity.salaryformula.effectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.effectivedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateStart')">
      <a-form-item :label="t('entity.salaryformula.expirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.expirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateEnd')">
      <a-form-item :label="t('entity.salaryformula.expirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.expirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formulaStatus')">
      <a-form-item :label="t('entity.salaryformula.formulastatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.formulaStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.formulastatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.salaryformula.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.relatedplant') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.salaryformula._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.salaryformula._self"
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
      :id-column-key="'salaryFormulaId'"
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
 * 薪资计算公式管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/compensation/salary-formula
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import SalaryFormulaForm from './components/salary-formula-form.vue'
import { getSalaryFormulaList, getSalaryFormulaById, createSalaryFormula, updateSalaryFormula, deleteSalaryFormulaById, deleteSalaryFormulaBatch, getSalaryFormulaTemplate, importSalaryFormula, exportSalaryFormula } from '@/api/human-resource/compensation/salary-formula'
import type { SalaryFormula, SalaryFormulaQuery, SalaryFormulaCreate, SalaryFormulaUpdate } from '@/types/human-resource/compensation/salary-formula'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalaryFormula')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.salaryformula._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SalaryFormula[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SalaryFormula | null>(null)
/** 表格多选行 */
const selectedRows = ref<SalaryFormula[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SalaryFormula>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  setCode: '',
  setName: '',
  PayrollId: '',
  formulaCode: '',
  formulaName: '',
  formulaStep: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  targetField: '',
  formulaExpression: '',
  stepDescription: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  formulaStatus: undefined as number | undefined,
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'setCode', label: t('entity.salaryformula.setcode') },
  { key: 'setName', label: t('entity.salaryformula.setname') },
  { key: 'PayrollId', label: t('entity.salaryformula.PayrollId') },
  { key: 'formulaCode', label: t('entity.salaryformula.formulacode') },
  { key: 'formulaName', label: t('entity.salaryformula.formulaname') },
  { key: 'formulaStep', label: t('entity.salaryformula.formulastep') },
  { key: 'sortOrder', label: t('entity.salaryformula.sortorder') },
  { key: 'targetField', label: t('entity.salaryformula.targetfield') },
  { key: 'formulaExpression', label: t('entity.salaryformula.formulaexpression') },
  { key: 'stepDescription', label: t('entity.salaryformula.stepdescription') },
  { key: 'effectiveDateStart', label: t('entity.salaryformula.effectivedatestart') },
  { key: 'effectiveDateEnd', label: t('entity.salaryformula.effectivedateend') },
  { key: 'expiryDateStart', label: t('entity.salaryformula.expirydatestart') },
  { key: 'expiryDateEnd', label: t('entity.salaryformula.expirydateend') },
  { key: 'formulaStatus', label: t('entity.salaryformula.formulastatus') },
  { key: 'relatedPlant', label: t('entity.salaryformula.relatedplant') },
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
const entityIdName = 'salaryFormulaId'
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
    dataIndex: 'salaryFormulaId',
    key: 'salaryFormulaId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'salaryFormulaId') ?? ''
  },
  {
    title: t('entity.salaryformula.setcode'),
    dataIndex: 'setCode',
    key: 'setCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'setCode') ?? ''
  },
  {
    title: t('entity.salaryformula.setname'),
    dataIndex: 'setName',
    key: 'setName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'setName') ?? ''
  },
  {
    title: t('entity.salaryformula.PayrollId'),
    dataIndex: 'PayrollId',
    key: 'PayrollId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'PayrollId') ?? ''
  },
  {
    title: t('entity.salaryformula.formulacode'),
    dataIndex: 'formulaCode',
    key: 'formulaCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'formulaCode') ?? ''
  },
  {
    title: t('entity.salaryformula.formulaname'),
    dataIndex: 'formulaName',
    key: 'formulaName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'formulaName') ?? ''
  },
  {
    title: t('entity.salaryformula.formulastep'),
    dataIndex: 'formulaStep',
    key: 'formulaStep',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryformula.targetfield'),
    dataIndex: 'targetField',
    key: 'targetField',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'targetField') ?? ''
  },
  {
    title: t('entity.salaryformula.formulaexpression'),
    dataIndex: 'formulaExpression',
    key: 'formulaExpression',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'formulaExpression') ?? ''
  },
  {
    title: t('entity.salaryformula.stepdescription'),
    dataIndex: 'stepDescription',
    key: 'stepDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'stepDescription') ?? ''
  },
  {
    title: t('entity.salaryformula.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'effectiveDate') ?? ''
  },
  {
    title: t('entity.salaryformula.expirydate'),
    dataIndex: 'expiryDate',
    key: 'expiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'expiryDate') ?? ''
  },
  {
    title: t('entity.salaryformula.formulastatus'),
    dataIndex: 'formulaStatus',
    key: 'formulaStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryformula.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'relatedPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:performance:analysis:update',
        onClick: (record: SalaryFormula) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:performance:analysis:delete',
        onClick: (record: SalaryFormula) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSalaryFormulaId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSalaryFormulaField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SalaryFormula[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SalaryFormula, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSalaryFormulaId(selectedRow.value) === getSalaryFormulaId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalaryFormula[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SalaryFormula) => ({
  onClick: () => {
    const key = getSalaryFormulaId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSalaryFormulaId(item)))
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
    const params: SalaryFormulaQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getSalaryFormulaList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SalaryFormula] 加载数据失败', { error })
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
  setCode: '',
  setName: '',
  PayrollId: '',
  formulaCode: '',
  formulaName: '',
  formulaStep: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  targetField: '',
  formulaExpression: '',
  stepDescription: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  formulaStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.salaryformula._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: SalaryFormula) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.salaryformula._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.salaryformula._self') }))
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
      await updateSalaryFormula(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.salaryformula._self') }))
    } else {
      await createSalaryFormula(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.salaryformula._self') }))
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
  const res = await getSalaryFormulaTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSalaryFormula(file, sheetName)
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
    const exportQuery: SalaryFormulaQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportSalaryFormula(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.salaryformula._self') }))
  } catch (error: any) {
    logger.error('[SalaryFormula] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.salaryformula._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SalaryFormula) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.salaryformula._self'), name: t('common.tip.this.target', { target: t('entity.salaryformula._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalaryFormulaById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.salaryformula._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.salaryformula._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.salaryformula._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSalaryFormulaBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.salaryformula._self') }))
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
  setCode: '',
  setName: '',
  PayrollId: '',
  formulaCode: '',
  formulaName: '',
  formulaStep: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  targetField: '',
  formulaExpression: '',
  stepDescription: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  formulaStatus: undefined as number | undefined,
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
.human-resource-compensation-salary-formula {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
