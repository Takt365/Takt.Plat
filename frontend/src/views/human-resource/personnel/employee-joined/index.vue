<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-joined -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工入职上岗办理记录管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-personnel-employee-joined">
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
      create-permission="humanresource:personnel:employeejoined:create"
      update-permission="humanresource:personnel:employeejoined:update"
      delete-permission="humanresource:personnel:employeejoined:delete"
      import-permission="humanresource:personnel:employeejoined:import"
      export-permission="humanresource:personnel:employeejoined:export"
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
      :id-column-key="'employeeJoinedId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEmployeeJoinedId"
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
      <EmployeeJoinedForm
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
      :storage-key="'takt-query-fields-human-resource-personnel-employee-joined'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.employeeJoined.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.employeeid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('onboardingId')">
      <a-form-item :label="t('entity.employeeJoined.onboardingid')">
        <a-input
          v-model:value="advancedQueryForm.onboardingId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.onboardingid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('joinedDateStart')">
      <a-form-item :label="t('entity.employeeJoined.joineddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.joinedDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeJoined.joineddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('joinedDateEnd')">
      <a-form-item :label="t('entity.employeeJoined.joineddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.joinedDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeJoined.joineddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('probationEndDateStart')">
      <a-form-item :label="t('entity.employeeJoined.probationenddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.probationEndDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeJoined.probationenddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('probationEndDateEnd')">
      <a-form-item :label="t('entity.employeeJoined.probationenddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.probationEndDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeJoined.probationenddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('regularDateStart')">
      <a-form-item :label="t('entity.employeeJoined.regulardatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.regularDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeJoined.regulardatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('regularDateEnd')">
      <a-form-item :label="t('entity.employeeJoined.regulardateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.regularDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeJoined.regulardateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="t('entity.employeeJoined.deptid')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.deptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="t('entity.employeeJoined.deptname')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.deptname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postId')">
      <a-form-item :label="t('entity.employeeJoined.postid')">
        <a-input
          v-model:value="advancedQueryForm.postId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.postid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postName')">
      <a-form-item :label="t('entity.employeeJoined.postname')">
        <a-input
          v-model:value="advancedQueryForm.postName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.postname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobTitle')">
      <a-form-item :label="t('entity.employeeJoined.jobtitle')">
        <a-input
          v-model:value="advancedQueryForm.jobTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.jobtitle') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workNature')">
      <a-form-item :label="t('entity.employeeJoined.worknature')">
        <a-input-number
          v-model:value="advancedQueryForm.workNature"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.worknature') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employmentType')">
      <a-form-item :label="t('entity.employeeJoined.employmenttype')">
        <a-input-number
          v-model:value="advancedQueryForm.employmentType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.employmenttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directManagerId')">
      <a-form-item :label="t('entity.employeeJoined.directmanagerid')">
        <a-input
          v-model:value="advancedQueryForm.directManagerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.directmanagerid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directManagerName')">
      <a-form-item :label="t('entity.employeeJoined.directmanagername')">
        <a-input
          v-model:value="advancedQueryForm.directManagerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.directmanagername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.employeeJoined.approvalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.approvalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.approvalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.employeeJoined.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.initiatorid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.employeeJoined.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.initiatedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.employeeJoined.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeJoined.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.employeeJoined.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.approvedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.employeeJoined.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeeJoined.approvedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.employeeJoined.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeeJoined.approvedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.employeeJoined._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.employeeJoined._self"
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
      :id-column-key="'employeeJoinedId'"
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
 * 员工入职上岗办理记录管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/personnel/employee-joined
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import EmployeeJoinedForm from './components/employee-joined-form.vue'
import { getEmployeeJoinedList, getEmployeeJoinedById, createEmployeeJoined, updateEmployeeJoined, deleteEmployeeJoinedById, deleteEmployeeJoinedBatch, getEmployeeJoinedTemplate, importEmployeeJoined, exportEmployeeJoined } from '@/api/human-resource/personnel/employee-joined'
import type { EmployeeJoined, EmployeeJoinedQuery, EmployeeJoinedCreate, EmployeeJoinedUpdate } from '@/types/human-resource/personnel/employee-joined'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEmployeeJoined')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.employeeJoined._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EmployeeJoined[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<EmployeeJoined | null>(null)
/** 表格多选行 */
const selectedRows = ref<EmployeeJoined[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<EmployeeJoined>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  employeeId: '',
  onboardingId: '',
  joinedDateStart: '',
  joinedDateEnd: '',
  probationEndDateStart: '',
  probationEndDateEnd: '',
  regularDateStart: '',
  regularDateEnd: '',
  deptId: '',
  deptName: '',
  postId: '',
  postName: '',
  jobTitle: '',
  workNature: undefined as number | undefined,
  employmentType: undefined as number | undefined,
  directManagerId: '',
  directManagerName: '',
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
  { key: 'employeeId', label: t('entity.employeeJoined.employeeid') },
  { key: 'onboardingId', label: t('entity.employeeJoined.onboardingid') },
  { key: 'joinedDateStart', label: t('entity.employeeJoined.joineddatestart') },
  { key: 'joinedDateEnd', label: t('entity.employeeJoined.joineddateend') },
  { key: 'probationEndDateStart', label: t('entity.employeeJoined.probationenddatestart') },
  { key: 'probationEndDateEnd', label: t('entity.employeeJoined.probationenddateend') },
  { key: 'regularDateStart', label: t('entity.employeeJoined.regulardatestart') },
  { key: 'regularDateEnd', label: t('entity.employeeJoined.regulardateend') },
  { key: 'deptId', label: t('entity.employeeJoined.deptid') },
  { key: 'deptName', label: t('entity.employeeJoined.deptname') },
  { key: 'postId', label: t('entity.employeeJoined.postid') },
  { key: 'postName', label: t('entity.employeeJoined.postname') },
  { key: 'jobTitle', label: t('entity.employeeJoined.jobtitle') },
  { key: 'workNature', label: t('entity.employeeJoined.worknature') },
  { key: 'employmentType', label: t('entity.employeeJoined.employmenttype') },
  { key: 'directManagerId', label: t('entity.employeeJoined.directmanagerid') },
  { key: 'directManagerName', label: t('entity.employeeJoined.directmanagername') },
  { key: 'approvalStatus', label: t('entity.employeeJoined.approvalstatus') },
  { key: 'initiatorId', label: t('entity.employeeJoined.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.employeeJoined.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.employeeJoined.initiatedatend') },
  { key: 'approvedBy', label: t('entity.employeeJoined.approvedby') },
  { key: 'approvedAtStart', label: t('entity.employeeJoined.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.employeeJoined.approvedatend') },
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
const entityIdName = 'employeeJoinedId'
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
    dataIndex: 'employeeJoinedId',
    key: 'employeeJoinedId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'employeeJoinedId') ?? ''
  },
  {
    title: t('entity.employeeJoined.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.employeeJoined.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.employeeJoined.onboardingid'),
    dataIndex: 'onboardingId',
    key: 'onboardingId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'onboardingId') ?? ''
  },
  {
    title: t('entity.employeeJoined.onboardingname'),
    dataIndex: 'onboardingName',
    key: 'onboardingName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'onboardingName') ?? ''
  },
  {
    title: t('entity.employeeJoined.joineddate'),
    dataIndex: 'joinedDate',
    key: 'joinedDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'joinedDate') ?? ''
  },
  {
    title: t('entity.employeeJoined.probationenddate'),
    dataIndex: 'probationEndDate',
    key: 'probationEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'probationEndDate') ?? ''
  },
  {
    title: t('entity.employeeJoined.regulardate'),
    dataIndex: 'regularDate',
    key: 'regularDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'regularDate') ?? ''
  },
  {
    title: t('entity.employeeJoined.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.employeeJoined.deptname'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'deptName') ?? ''
  },
  {
    title: t('entity.employeeJoined.postid'),
    dataIndex: 'postId',
    key: 'postId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'postId') ?? ''
  },
  {
    title: t('entity.employeeJoined.postname'),
    dataIndex: 'postName',
    key: 'postName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'postName') ?? ''
  },
  {
    title: t('entity.employeeJoined.jobtitle'),
    dataIndex: 'jobTitle',
    key: 'jobTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'jobTitle') ?? ''
  },
  {
    title: t('entity.employeeJoined.worknature'),
    dataIndex: 'workNature',
    key: 'workNature',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'workNature') ?? ''
  },
  {
    title: t('entity.employeeJoined.employmenttype'),
    dataIndex: 'employmentType',
    key: 'employmentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'employmentType') ?? ''
  },
  {
    title: t('entity.employeeJoined.directmanagerid'),
    dataIndex: 'directManagerId',
    key: 'directManagerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'directManagerId') ?? ''
  },
  {
    title: t('entity.employeeJoined.directmanagername'),
    dataIndex: 'directManagerName',
    key: 'directManagerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'directManagerName') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:personnel:employeejoined:update',
        onClick: (record: EmployeeJoined) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:personnel:employeejoined:delete',
        onClick: (record: EmployeeJoined) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEmployeeJoinedId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEmployeeJoinedField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EmployeeJoined[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EmployeeJoined, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEmployeeJoinedId(selectedRow.value) === getEmployeeJoinedId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EmployeeJoined[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: EmployeeJoined) => ({
  onClick: () => {
    const key = getEmployeeJoinedId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEmployeeJoinedId(item)))
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
    const params: EmployeeJoinedQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getEmployeeJoinedList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EmployeeJoined] 加载数据失败', { error })
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
  onboardingId: '',
  joinedDateStart: '',
  joinedDateEnd: '',
  probationEndDateStart: '',
  probationEndDateEnd: '',
  regularDateStart: '',
  regularDateEnd: '',
  deptId: '',
  deptName: '',
  postId: '',
  postName: '',
  jobTitle: '',
  workNature: undefined as number | undefined,
  employmentType: undefined as number | undefined,
  directManagerId: '',
  directManagerName: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.employeeJoined._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: EmployeeJoined) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.employeeJoined._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.employeeJoined._self') }))
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
      await updateEmployeeJoined(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.employeeJoined._self') }))
    } else {
      await createEmployeeJoined(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.employeeJoined._self') }))
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
  const res = await getEmployeeJoinedTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEmployeeJoined(file, sheetName)
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
    const exportQuery: EmployeeJoinedQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportEmployeeJoined(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.employeeJoined._self') }))
  } catch (error: any) {
    logger.error('[EmployeeJoined] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.employeeJoined._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EmployeeJoined) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.employeeJoined._self'), name: t('common.tip.this.target', { target: t('entity.employeeJoined._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEmployeeJoinedById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.employeeJoined._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.employeeJoined._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.employeeJoined._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEmployeeJoinedBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.employeeJoined._self') }))
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
  onboardingId: '',
  joinedDateStart: '',
  joinedDateEnd: '',
  probationEndDateStart: '',
  probationEndDateEnd: '',
  regularDateStart: '',
  regularDateEnd: '',
  deptId: '',
  deptName: '',
  postId: '',
  postName: '',
  jobTitle: '',
  workNature: undefined as number | undefined,
  employmentType: undefined as number | undefined,
  directManagerId: '',
  directManagerName: '',
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
.human-resource-personnel-employee-joined {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
