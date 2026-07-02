<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-onboarding -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：入职待办管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
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
      create-permission="human:resource:personnel:employee:onboarding:create"
      update-permission="human:resource:personnel:employee:onboarding:update"
      delete-permission="human:resource:personnel:employee:onboarding:delete"
      import-permission="human:resource:personnel:employee:onboarding:import"
      export-permission="human:resource:personnel:employee:onboarding:export"
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
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'employeeOnboardingId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEmployeeOnboardingId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
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
      <EmployeeOnboardingForm
        :key="formData?.employeeOnboardingId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-personnel-employee-onboarding'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('offerId')">
      <a-form-item :label="t('entity.employeeonboarding.offerid')">
        <a-input
          v-model:value="advancedQueryForm.offerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeonboarding.offerid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('todoNo')">
      <a-form-item :label="t('entity.employeeonboarding.todono')">
        <a-input
          v-model:value="advancedQueryForm.todoNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeonboarding.todono') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('todoStatus')">
      <a-form-item :label="t('entity.employeeonboarding.todostatus')">
        <a-input-number
          v-model:value="advancedQueryForm.todoStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeonboarding.todostatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedJoinedDateStart')">
      <a-form-item :label="t('entity.employeeonboarding.plannedjoineddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedJoinedDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeonboarding.plannedjoineddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedJoinedDateEnd')">
      <a-form-item :label="t('entity.employeeonboarding.plannedjoineddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedJoinedDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeonboarding.plannedjoineddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('candidateName')">
      <a-form-item :label="t('entity.employeeonboarding.candidatename')">
        <a-date-picker
          v-model:value="advancedQueryForm.candidateName"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeonboarding.candidatename') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mobile')">
      <a-form-item :label="t('entity.employeeonboarding.mobile')">
        <a-input
          v-model:value="advancedQueryForm.mobile"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeonboarding.mobile') })"
          show-count
          :maxlength="11"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.employeeonboarding.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeonboarding.employeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeJoinedId')">
      <a-form-item :label="t('entity.employeeonboarding.employeejoinedid')">
        <a-input
          v-model:value="advancedQueryForm.employeeJoinedId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeonboarding.employeejoinedid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reason')">
      <a-form-item :label="t('entity.employeeonboarding.reason')">
        <a-input
          v-model:value="advancedQueryForm.reason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeonboarding.reason') })"
          show-count
          :maxlength="500"
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
      :title="t('common.dialog.title.import', { entity: t('entity.employeeonboarding._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.employeeonboarding._self"
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
      :id-column-key="'employeeOnboardingId'"
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
 * 入职待办管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/personnel/employee-onboarding
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import EmployeeOnboardingForm from './components/employee-onboarding-form.vue'
import { getEmployeeOnboardingList, getEmployeeOnboardingById, createEmployeeOnboarding, updateEmployeeOnboarding, deleteEmployeeOnboardingById, deleteEmployeeOnboardingBatch, getEmployeeOnboardingTemplate, importEmployeeOnboarding, exportEmployeeOnboarding, updateEmployeeOnboardingStatus } from '@/api/human-resource/personnel/employee-onboarding'
import type { EmployeeOnboarding, EmployeeOnboardingQuery } from '@/types/human-resource/personnel/employee-onboarding'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEmployeeOnboarding')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.employeeonboarding._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EmployeeOnboarding[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<EmployeeOnboarding | null>(null)
/** 表格多选行 */
const selectedRows = ref<EmployeeOnboarding[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<EmployeeOnboarding> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  offerId: '',
  todoNo: '',
  todoStatus: undefined as number | undefined,
  plannedJoinedDateStart: '',
  plannedJoinedDateEnd: '',
  candidateName: '',
  mobile: '',
  employeeId: '',
  employeeJoinedId: '',
  reason: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'offerId', label: t('entity.employeeonboarding.offerid') },
  { key: 'todoNo', label: t('entity.employeeonboarding.todono') },
  { key: 'todoStatus', label: t('entity.employeeonboarding.todostatus') },
  { key: 'plannedJoinedDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.employeeonboarding.plannedjoineddate')) },
  { key: 'plannedJoinedDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.employeeonboarding.plannedjoineddate')) },
  { key: 'candidateName', label: t('entity.employeeonboarding.candidatename') },
  { key: 'mobile', label: t('entity.employeeonboarding.mobile') },
  { key: 'employeeId', label: t('entity.employeeonboarding.employeeid') },
  { key: 'employeeJoinedId', label: t('entity.employeeonboarding.employeejoinedid') },
  { key: 'reason', label: t('entity.employeeonboarding.reason') },
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
const entityIdName = 'employeeOnboardingId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)



/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EmployeeOnboardingQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EmployeeOnboardingQuery>): EmployeeOnboardingQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EmployeeOnboardingQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EmployeeOnboardingQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('offerId', form.offerId)
  assignTrimmed('todoNo', form.todoNo)
  if (form.todoStatus !== undefined && form.todoStatus !== null) {
    query.todoStatus = form.todoStatus
  }
  assignTrimmed('plannedJoinedDateStart', form.plannedJoinedDateStart)
  assignTrimmed('plannedJoinedDateEnd', form.plannedJoinedDateEnd)
  assignTrimmed('candidateName', form.candidateName)
  assignTrimmed('mobile', form.mobile)
  assignTrimmed('employeeId', form.employeeId)
  assignTrimmed('employeeJoinedId', form.employeeJoinedId)
  assignTrimmed('reason', form.reason)
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







/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'employeeOnboardingId',
    key: 'employeeOnboardingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'employeeOnboardingId') ?? ''
  },
  {
    title: t('entity.employeeonboarding.offerid'),
    dataIndex: 'offerId',
    key: 'offerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'offerId') ?? ''
  },
  {
    title: t('entity.employeeonboarding.todono'),
    dataIndex: 'todoNo',
    key: 'todoNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'todoNo') ?? ''
  },
  {
    title: t('entity.employeeonboarding.todostatus'),
    dataIndex: 'todoStatus',
    key: 'todoStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'todoStatus') ?? ''
  },
  {
    title: t('entity.employeeonboarding.plannedjoineddate'),
    dataIndex: 'plannedJoinedDate',
    key: 'plannedJoinedDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'plannedJoinedDate') ?? ''
  },
  {
    title: t('entity.employeeonboarding.candidatename'),
    dataIndex: 'candidateName',
    key: 'candidateName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'candidateName') ?? ''
  },
  {
    title: t('entity.employeeonboarding.mobile'),
    dataIndex: 'mobile',
    key: 'mobile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'mobile') ?? ''
  },
  {
    title: t('entity.employeeonboarding.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.employeeonboarding.employeejoinedid'),
    dataIndex: 'employeeJoinedId',
    key: 'employeeJoinedId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'employeeJoinedId') ?? ''
  },
  {
    title: t('entity.employeeonboarding.reason'),
    dataIndex: 'reason',
    key: 'reason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'reason') ?? ''
  },
  {
    title: t('entity.employeeonboarding.offer'),
    dataIndex: 'offer',
    key: 'offer',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'offer') ?? ''
  },
  {
    title: t('entity.employeeonboarding.employeejoined'),
    dataIndex: 'employeeJoined',
    key: 'employeeJoined',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeOnboardingField(record, 'employeeJoined') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:personnel:employee:onboarding:update',
        onClick: (record: EmployeeOnboarding) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:personnel:employee:onboarding:delete',
        onClick: (record: EmployeeOnboarding) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEmployeeOnboardingId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEmployeeOnboardingField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EmployeeOnboarding[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EmployeeOnboarding, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getEmployeeOnboardingId(selectedRow.value) === getEmployeeOnboardingId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EmployeeOnboarding[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: EmployeeOnboarding) => ({
  onClick: () => {
    const key = getEmployeeOnboardingId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEmployeeOnboardingId(item)))
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
    const res = await getEmployeeOnboardingList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EmployeeOnboarding] 加载数据失败', { error })
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
  offerId: '',
  todoNo: '',
  todoStatus: undefined as number | undefined,
  plannedJoinedDateStart: '',
  plannedJoinedDateEnd: '',
  candidateName: '',
  mobile: '',
  employeeId: '',
  employeeJoinedId: '',
  reason: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.employeeonboarding._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: EmployeeOnboarding) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.employeeonboarding._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.employeeonboarding._self') }))
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
      await updateEmployeeOnboarding(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.employeeonboarding._self') }))
    } else {
      await createEmployeeOnboarding(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.employeeonboarding._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
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
  const res = await getEmployeeOnboardingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEmployeeOnboarding(file, sheetName)
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
    const exportMeta = await exportEmployeeOnboarding(
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
    message.success(t('common.feedback.export.success', { target: t('entity.employeeonboarding._self') }))
  } catch (error: any) {
    logger.error('[EmployeeOnboarding] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.employeeonboarding._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EmployeeOnboarding) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.employeeonboarding._self'), name: t('common.tip.this.target', { target: t('entity.employeeonboarding._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEmployeeOnboardingById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.employeeonboarding._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.employeeonboarding._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.employeeonboarding._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEmployeeOnboardingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.employeeonboarding._self') }))
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
  offerId: '',
  todoNo: '',
  todoStatus: undefined as number | undefined,
  plannedJoinedDateStart: '',
  plannedJoinedDateEnd: '',
  candidateName: '',
  mobile: '',
  employeeId: '',
  employeeJoinedId: '',
  reason: '',
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
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
