<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-reassignment -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工调动记录管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-personnel-employee-reassignment">
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
      create-permission="humanresource:personnel:employeereassignment:create"
      update-permission="humanresource:personnel:employeereassignment:update"
      delete-permission="humanresource:personnel:employeereassignment:delete"
      import-permission="humanresource:personnel:employeereassignment:import"
      export-permission="humanresource:personnel:employeereassignment:export"
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
      entity-scope="approval"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'employeeReassignmentId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEmployeeReassignmentId"
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
      <EmployeeReassignmentForm
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
      :storage-key="'takt-query-fields-human-resource-personnel-employee-reassignment'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.employeeReassignment.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.employeeid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reassignmentType')">
      <a-form-item :label="t('entity.employeeReassignment.reassignmenttype')">
        <a-input-number
          v-model:value="advancedQueryForm.reassignmentType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.reassignmenttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fromDeptId')">
      <a-form-item :label="t('entity.employeeReassignment.fromdeptid')">
        <a-input
          v-model:value="advancedQueryForm.fromDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.fromdeptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fromDeptName')">
      <a-form-item :label="t('entity.employeeReassignment.fromdeptname')">
        <a-input
          v-model:value="advancedQueryForm.fromDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.fromdeptname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fromPostId')">
      <a-form-item :label="t('entity.employeeReassignment.frompostid')">
        <a-input
          v-model:value="advancedQueryForm.fromPostId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.frompostid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fromPostName')">
      <a-form-item :label="t('entity.employeeReassignment.frompostname')">
        <a-input
          v-model:value="advancedQueryForm.fromPostName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.frompostname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('toDeptId')">
      <a-form-item :label="t('entity.employeeReassignment.todeptid')">
        <a-input
          v-model:value="advancedQueryForm.toDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.todeptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('toDeptName')">
      <a-form-item :label="t('entity.employeeReassignment.todeptname')">
        <a-input
          v-model:value="advancedQueryForm.toDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.todeptname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('toPostId')">
      <a-form-item :label="t('entity.employeeReassignment.topostid')">
        <a-input
          v-model:value="advancedQueryForm.toPostId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.topostid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('toPostName')">
      <a-form-item :label="t('entity.employeeReassignment.topostname')">
        <a-input
          v-model:value="advancedQueryForm.toPostName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.topostname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="t('entity.employeeReassignment.effectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeReassignment.effectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="t('entity.employeeReassignment.effectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeReassignment.effectivedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reason')">
      <a-form-item :label="t('entity.employeeReassignment.reason')">
        <a-input
          v-model:value="advancedQueryForm.reason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.reason') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.employeeReassignment.approvalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.approvalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.approvalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.employeeReassignment.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.initiatorid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.employeeReassignment.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.initiatedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.employeeReassignment.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeReassignment.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.employeeReassignment.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.approvedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.employeeReassignment.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeReassignment.approvedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.employeeReassignment.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeReassignment.approvedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.employeeReassignment._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.employeeReassignment._self"
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
      :id-column-key="'employeeReassignmentId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 员工调动记录管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/personnel/employee-reassignment
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import EmployeeReassignmentForm from './components/employee-reassignment-form.vue'
import { getEmployeeReassignmentList, getEmployeeReassignmentById, createEmployeeReassignment, updateEmployeeReassignment, deleteEmployeeReassignmentById, deleteEmployeeReassignmentBatch, getEmployeeReassignmentTemplate, importEmployeeReassignment, exportEmployeeReassignment } from '@/api/human-resource/personnel/employee-reassignment'
import type { EmployeeReassignment, EmployeeReassignmentQuery, EmployeeReassignmentCreate, EmployeeReassignmentUpdate } from '@/types/human-resource/personnel/employee-reassignment'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEmployeeReassignment')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.employeeReassignment._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EmployeeReassignment[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<EmployeeReassignment | null>(null)
/** 表格多选行 */
const selectedRows = ref<EmployeeReassignment[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<EmployeeReassignment>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  employeeId: '',
  reassignmentType: undefined as number | undefined,
  fromDeptId: '',
  fromDeptName: '',
  fromPostId: '',
  fromPostName: '',
  toDeptId: '',
  toDeptName: '',
  toPostId: '',
  toPostName: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  reason: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'employeeId', label: t('entity.employeeReassignment.employeeid') },
  { key: 'reassignmentType', label: t('entity.employeeReassignment.reassignmenttype') },
  { key: 'fromDeptId', label: t('entity.employeeReassignment.fromdeptid') },
  { key: 'fromDeptName', label: t('entity.employeeReassignment.fromdeptname') },
  { key: 'fromPostId', label: t('entity.employeeReassignment.frompostid') },
  { key: 'fromPostName', label: t('entity.employeeReassignment.frompostname') },
  { key: 'toDeptId', label: t('entity.employeeReassignment.todeptid') },
  { key: 'toDeptName', label: t('entity.employeeReassignment.todeptname') },
  { key: 'toPostId', label: t('entity.employeeReassignment.topostid') },
  { key: 'toPostName', label: t('entity.employeeReassignment.topostname') },
  { key: 'effectiveDateStart', label: t('entity.employeeReassignment.effectivedatestart') },
  { key: 'effectiveDateEnd', label: t('entity.employeeReassignment.effectivedateend') },
  { key: 'reason', label: t('entity.employeeReassignment.reason') },
  { key: 'approvalStatus', label: t('entity.employeeReassignment.approvalstatus') },
  { key: 'initiatorId', label: t('entity.employeeReassignment.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.employeeReassignment.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.employeeReassignment.initiatedatend') },
  { key: 'approvedBy', label: t('entity.employeeReassignment.approvedby') },
  { key: 'approvedAtStart', label: t('entity.employeeReassignment.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.employeeReassignment.approvedatend') },
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
const entityIdName = 'employeeReassignmentId'
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
    dataIndex: 'employeeReassignmentId',
    key: 'employeeReassignmentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'employeeReassignmentId') ?? ''
  },
  {
    title: t('entity.employeeReassignment.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.employeeReassignment.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.employeeReassignment.reassignmenttype'),
    dataIndex: 'reassignmentType',
    key: 'reassignmentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'reassignmentType') ?? ''
  },
  {
    title: t('entity.employeeReassignment.fromdeptid'),
    dataIndex: 'fromDeptId',
    key: 'fromDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'fromDeptId') ?? ''
  },
  {
    title: t('entity.employeeReassignment.fromdeptname'),
    dataIndex: 'fromDeptName',
    key: 'fromDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'fromDeptName') ?? ''
  },
  {
    title: t('entity.employeeReassignment.frompostid'),
    dataIndex: 'fromPostId',
    key: 'fromPostId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'fromPostId') ?? ''
  },
  {
    title: t('entity.employeeReassignment.frompostname'),
    dataIndex: 'fromPostName',
    key: 'fromPostName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'fromPostName') ?? ''
  },
  {
    title: t('entity.employeeReassignment.todeptid'),
    dataIndex: 'toDeptId',
    key: 'toDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'toDeptId') ?? ''
  },
  {
    title: t('entity.employeeReassignment.todeptname'),
    dataIndex: 'toDeptName',
    key: 'toDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'toDeptName') ?? ''
  },
  {
    title: t('entity.employeeReassignment.topostid'),
    dataIndex: 'toPostId',
    key: 'toPostId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'toPostId') ?? ''
  },
  {
    title: t('entity.employeeReassignment.topostname'),
    dataIndex: 'toPostName',
    key: 'toPostName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'toPostName') ?? ''
  },
  {
    title: t('entity.employeeReassignment.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'effectiveDate') ?? ''
  },
  {
    title: t('entity.employeeReassignment.reason'),
    dataIndex: 'reason',
    key: 'reason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'reason') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:personnel:employeereassignment:update',
        onClick: (record: EmployeeReassignment) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:personnel:employeereassignment:delete',
        onClick: (record: EmployeeReassignment) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEmployeeReassignmentId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEmployeeReassignmentField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EmployeeReassignment[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EmployeeReassignment, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEmployeeReassignmentId(selectedRow.value) === getEmployeeReassignmentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EmployeeReassignment[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: EmployeeReassignment) => ({
  onClick: () => {
    const key = getEmployeeReassignmentId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEmployeeReassignmentId(item)))
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
    const params: EmployeeReassignmentQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getEmployeeReassignmentList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EmployeeReassignment] 加载数据失败', { error })
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
  employeeId: '',
  reassignmentType: undefined as number | undefined,
  fromDeptId: '',
  fromDeptName: '',
  fromPostId: '',
  fromPostName: '',
  toDeptId: '',
  toDeptName: '',
  toPostId: '',
  toPostName: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  reason: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.employeeReassignment._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: EmployeeReassignment) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.employeeReassignment._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.employeeReassignment._self') }))
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
      await updateEmployeeReassignment(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.employeeReassignment._self') }))
    } else {
      await createEmployeeReassignment(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.employeeReassignment._self') }))
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
  const res = await getEmployeeReassignmentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEmployeeReassignment(file, sheetName)
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
    const exportQuery: EmployeeReassignmentQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportEmployeeReassignment(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.employeeReassignment._self') }))
  } catch (error: any) {
    logger.error('[EmployeeReassignment] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.employeeReassignment._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EmployeeReassignment) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.employeeReassignment._self'), name: t('common.tip.this.target', { target: t('entity.employeeReassignment._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEmployeeReassignmentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.employeeReassignment._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.employeeReassignment._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.employeeReassignment._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEmployeeReassignmentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.employeeReassignment._self') }))
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
  reassignmentType: undefined as number | undefined,
  fromDeptId: '',
  fromDeptName: '',
  fromPostId: '',
  fromPostName: '',
  toDeptId: '',
  toDeptName: '',
  toPostId: '',
  toPostName: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  reason: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
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
.human-resource-personnel-employee-reassignment {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
