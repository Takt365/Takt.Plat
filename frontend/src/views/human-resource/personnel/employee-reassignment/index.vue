<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-reassignment -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工调动记录管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="human:resource:personnel:employee:reassignment:create"
      update-permission="human:resource:personnel:employee:reassignment:update"
      delete-permission="human:resource:personnel:employee:reassignment:delete"
      import-permission="human:resource:personnel:employee:reassignment:import"
      export-permission="human:resource:personnel:employee:reassignment:export"
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
      entity-scope="approval"
      :columns="columns"
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
      <EmployeeReassignmentForm
        :key="formData?.employeeReassignmentId ?? 'create'"
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
      <a-form-item :label="t('entity.employeereassignment.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.employeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reassignmentType')">
      <a-form-item :label="t('entity.employeereassignment.reassignmenttype')">
        <a-input-number
          v-model:value="advancedQueryForm.reassignmentType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.reassignmenttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fromDeptId')">
      <a-form-item :label="t('entity.employeereassignment.fromdeptid')">
        <a-input
          v-model:value="advancedQueryForm.fromDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.fromdeptid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fromDeptName')">
      <a-form-item :label="t('entity.employeereassignment.fromdeptname')">
        <a-input
          v-model:value="advancedQueryForm.fromDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.fromdeptname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fromPostId')">
      <a-form-item :label="t('entity.employeereassignment.frompostid')">
        <a-input
          v-model:value="advancedQueryForm.fromPostId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.frompostid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fromPostName')">
      <a-form-item :label="t('entity.employeereassignment.frompostname')">
        <a-input
          v-model:value="advancedQueryForm.fromPostName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.frompostname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('toDeptId')">
      <a-form-item :label="t('entity.employeereassignment.todeptid')">
        <a-input
          v-model:value="advancedQueryForm.toDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.todeptid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('toDeptName')">
      <a-form-item :label="t('entity.employeereassignment.todeptname')">
        <a-input
          v-model:value="advancedQueryForm.toDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.todeptname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('toPostId')">
      <a-form-item :label="t('entity.employeereassignment.topostid')">
        <a-input
          v-model:value="advancedQueryForm.toPostId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.topostid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('toPostName')">
      <a-form-item :label="t('entity.employeereassignment.topostname')">
        <a-input
          v-model:value="advancedQueryForm.toPostName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.topostname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="t('entity.employeereassignment.effectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeereassignment.effectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="t('entity.employeereassignment.effectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeereassignment.effectivedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reason')">
      <a-form-item :label="t('entity.employeereassignment.reason')">
        <a-input
          v-model:value="advancedQueryForm.reason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.reason') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.employeereassignment.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeereassignment.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.employeereassignment.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.employeereassignment.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.employeereassignment.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeereassignment.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.employeereassignment.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.employeereassignment.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.employeereassignment.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeereassignment.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.employeereassignment.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeereassignment.flowinstanceid') })"
          show-count
          :maxlength="20"
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
      :title="t('common.dialog.title.import', { entity: t('entity.employeereassignment._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.employeereassignment._self"
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
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import EmployeeReassignmentForm from './components/employee-reassignment-form.vue'
import { getEmployeeReassignmentList, getEmployeeReassignmentById, createEmployeeReassignment, updateEmployeeReassignment, deleteEmployeeReassignmentById, deleteEmployeeReassignmentBatch, getEmployeeReassignmentTemplate, importEmployeeReassignment, exportEmployeeReassignment } from '@/api/human-resource/personnel/employee-reassignment'
import type { EmployeeReassignment, EmployeeReassignmentQuery } from '@/types/human-resource/personnel/employee-reassignment'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEmployeeReassignment')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.employeereassignment._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EmployeeReassignment[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
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
const formData = ref<Partial<EmployeeReassignment> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
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
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'employeeId', label: t('entity.employeereassignment.employeeid') },
  { key: 'reassignmentType', label: t('entity.employeereassignment.reassignmenttype') },
  { key: 'fromDeptId', label: t('entity.employeereassignment.fromdeptid') },
  { key: 'fromDeptName', label: t('entity.employeereassignment.fromdeptname') },
  { key: 'fromPostId', label: t('entity.employeereassignment.frompostid') },
  { key: 'fromPostName', label: t('entity.employeereassignment.frompostname') },
  { key: 'toDeptId', label: t('entity.employeereassignment.todeptid') },
  { key: 'toDeptName', label: t('entity.employeereassignment.todeptname') },
  { key: 'toPostId', label: t('entity.employeereassignment.topostid') },
  { key: 'toPostName', label: t('entity.employeereassignment.topostname') },
  { key: 'effectiveDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.employeereassignment.effectivedate')) },
  { key: 'effectiveDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.employeereassignment.effectivedate')) },
  { key: 'reason', label: t('entity.employeereassignment.reason') },
  { key: 'approvalStatus', label: t('entity.employeereassignment.approvalstatus') },
  { key: 'initiatorId', label: t('entity.employeereassignment.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.employeereassignment.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.employeereassignment.initiatedatend') },
  { key: 'approvedBy', label: t('entity.employeereassignment.approvedby') },
  { key: 'approvedAtStart', label: t('entity.employeereassignment.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.employeereassignment.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.employeereassignment.flowinstanceid') },
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
const entityIdName = 'employeeReassignmentId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)



/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EmployeeReassignmentQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EmployeeReassignmentQuery>): EmployeeReassignmentQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EmployeeReassignmentQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EmployeeReassignmentQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('employeeId', form.employeeId)
  if (form.reassignmentType !== undefined && form.reassignmentType !== null) {
    query.reassignmentType = form.reassignmentType
  }
  assignTrimmed('fromDeptId', form.fromDeptId)
  assignTrimmed('fromDeptName', form.fromDeptName)
  assignTrimmed('fromPostId', form.fromPostId)
  assignTrimmed('fromPostName', form.fromPostName)
  assignTrimmed('toDeptId', form.toDeptId)
  assignTrimmed('toDeptName', form.toDeptName)
  assignTrimmed('toPostId', form.toPostId)
  assignTrimmed('toPostName', form.toPostName)
  assignTrimmed('effectiveDateStart', form.effectiveDateStart)
  assignTrimmed('effectiveDateEnd', form.effectiveDateEnd)
  assignTrimmed('reason', form.reason)
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  assignTrimmed('initiatorId', form.initiatorId)
  assignTrimmed('initiatedAtStart', form.initiatedAtStart)
  assignTrimmed('initiatedAtEnd', form.initiatedAtEnd)
  assignTrimmed('approvedBy', form.approvedBy)
  assignTrimmed('approvedAtStart', form.approvedAtStart)
  assignTrimmed('approvedAtEnd', form.approvedAtEnd)
  assignTrimmed('flowInstanceId', form.flowInstanceId)
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
    dataIndex: 'employeeReassignmentId',
    key: 'employeeReassignmentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'employeeReassignmentId') ?? ''
  },
  {
    title: t('entity.employeereassignment.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.employeereassignment.reassignmenttype'),
    dataIndex: 'reassignmentType',
    key: 'reassignmentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'reassignmentType') ?? ''
  },
  {
    title: t('entity.employeereassignment.fromdeptid'),
    dataIndex: 'fromDeptId',
    key: 'fromDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'fromDeptId') ?? ''
  },
  {
    title: t('entity.employeereassignment.fromdeptname'),
    dataIndex: 'fromDeptName',
    key: 'fromDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'fromDeptName') ?? ''
  },
  {
    title: t('entity.employeereassignment.frompostid'),
    dataIndex: 'fromPostId',
    key: 'fromPostId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'fromPostId') ?? ''
  },
  {
    title: t('entity.employeereassignment.frompostname'),
    dataIndex: 'fromPostName',
    key: 'fromPostName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'fromPostName') ?? ''
  },
  {
    title: t('entity.employeereassignment.todeptid'),
    dataIndex: 'toDeptId',
    key: 'toDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'toDeptId') ?? ''
  },
  {
    title: t('entity.employeereassignment.todeptname'),
    dataIndex: 'toDeptName',
    key: 'toDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'toDeptName') ?? ''
  },
  {
    title: t('entity.employeereassignment.topostid'),
    dataIndex: 'toPostId',
    key: 'toPostId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'toPostId') ?? ''
  },
  {
    title: t('entity.employeereassignment.topostname'),
    dataIndex: 'toPostName',
    key: 'toPostName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'toPostName') ?? ''
  },
  {
    title: t('entity.employeereassignment.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeReassignmentField(record, 'effectiveDate') ?? ''
  },
  {
    title: t('entity.employeereassignment.reason'),
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
        permission: 'human:resource:personnel:employee:reassignment:update',
        onClick: (record: EmployeeReassignment) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:personnel:employee:reassignment:delete',
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
    } else if (selectedRow.value && getEmployeeReassignmentId(selectedRow.value) === getEmployeeReassignmentId(record)) {
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
    const res = await getEmployeeReassignmentList(buildListQuery())
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
  currentPage.value = getTaktDefaultPageIndex()
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
  flowInstanceId: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.employeereassignment._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: EmployeeReassignment) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.employeereassignment._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.employeereassignment._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.employeereassignment._self') }))
    } else {
      await createEmployeeReassignment(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.employeereassignment._self') }))
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
    const exportMeta = await exportEmployeeReassignment(
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
    message.success(t('common.feedback.export.success', { target: t('entity.employeereassignment._self') }))
  } catch (error: any) {
    logger.error('[EmployeeReassignment] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.employeereassignment._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EmployeeReassignment) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.employeereassignment._self'), name: t('common.tip.this.target', { target: t('entity.employeereassignment._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEmployeeReassignmentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.employeereassignment._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.employeereassignment._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.employeereassignment._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEmployeeReassignmentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.employeereassignment._self') }))
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
  flowInstanceId: '',
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
