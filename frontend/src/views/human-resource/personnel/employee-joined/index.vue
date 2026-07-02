<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee-joined -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工入职上岗办理记录管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="human:resource:personnel:employee:joined:create"
      update-permission="human:resource:personnel:employee:joined:update"
      delete-permission="human:resource:personnel:employee:joined:delete"
      import-permission="human:resource:personnel:employee:joined:import"
      export-permission="human:resource:personnel:employee:joined:export"
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
      <EmployeeJoinedForm
        :key="formData?.employeeJoinedId ?? 'create'"
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
      <a-form-item :label="t('entity.employeejoined.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.employeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('onboardingId')">
      <a-form-item :label="t('entity.employeejoined.onboardingid')">
        <a-input
          v-model:value="advancedQueryForm.onboardingId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.onboardingid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('joinedDateStart')">
      <a-form-item :label="t('entity.employeejoined.joineddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.joinedDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeejoined.joineddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('joinedDateEnd')">
      <a-form-item :label="t('entity.employeejoined.joineddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.joinedDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeejoined.joineddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('probationEndDateStart')">
      <a-form-item :label="t('entity.employeejoined.probationenddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.probationEndDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeejoined.probationenddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('probationEndDateEnd')">
      <a-form-item :label="t('entity.employeejoined.probationenddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.probationEndDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeejoined.probationenddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('regularDateStart')">
      <a-form-item :label="t('entity.employeejoined.regulardatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.regularDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeejoined.regulardatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('regularDateEnd')">
      <a-form-item :label="t('entity.employeejoined.regulardateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.regularDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeejoined.regulardateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="t('entity.employeejoined.deptid')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.deptid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="t('entity.employeejoined.deptname')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.deptname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postId')">
      <a-form-item :label="t('entity.employeejoined.postid')">
        <a-input
          v-model:value="advancedQueryForm.postId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.postid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postName')">
      <a-form-item :label="t('entity.employeejoined.postname')">
        <a-input
          v-model:value="advancedQueryForm.postName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.postname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobTitle')">
      <a-form-item :label="t('entity.employeejoined.jobtitle')">
        <a-input
          v-model:value="advancedQueryForm.jobTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.jobtitle') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workNature')">
      <a-form-item :label="t('entity.employeejoined.worknature')">
        <a-input-number
          v-model:value="advancedQueryForm.workNature"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.worknature') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employmentType')">
      <a-form-item :label="t('entity.employeejoined.employmenttype')">
        <a-input-number
          v-model:value="advancedQueryForm.employmentType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.employmenttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directManagerId')">
      <a-form-item :label="t('entity.employeejoined.directmanagerid')">
        <a-input
          v-model:value="advancedQueryForm.directManagerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.directmanagerid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directManagerName')">
      <a-form-item :label="t('entity.employeejoined.directmanagername')">
        <a-input
          v-model:value="advancedQueryForm.directManagerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.directmanagername') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.employeejoined.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeejoined.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.employeejoined.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.employeejoined.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.employeejoined.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeejoined.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.employeejoined.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.employeejoined.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.employeejoined.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employeejoined.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.employeejoined.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employeejoined.flowinstanceid') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.employeejoined._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.employeejoined._self"
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
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import EmployeeJoinedForm from './components/employee-joined-form.vue'
import { getEmployeeJoinedList, getEmployeeJoinedById, createEmployeeJoined, updateEmployeeJoined, deleteEmployeeJoinedById, deleteEmployeeJoinedBatch, getEmployeeJoinedTemplate, importEmployeeJoined, exportEmployeeJoined } from '@/api/human-resource/personnel/employee-joined'
import type { EmployeeJoined, EmployeeJoinedQuery } from '@/types/human-resource/personnel/employee-joined'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEmployeeJoined')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.employeejoined._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EmployeeJoined[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
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
const formData = ref<Partial<EmployeeJoined> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
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
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'employeeId', label: t('entity.employeejoined.employeeid') },
  { key: 'onboardingId', label: t('entity.employeejoined.onboardingid') },
  { key: 'joinedDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.employeejoined.joineddate')) },
  { key: 'joinedDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.employeejoined.joineddate')) },
  { key: 'probationEndDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.employeejoined.probationenddate')) },
  { key: 'probationEndDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.employeejoined.probationenddate')) },
  { key: 'regularDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.employeejoined.regulardate')) },
  { key: 'regularDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.employeejoined.regulardate')) },
  { key: 'deptId', label: t('entity.employeejoined.deptid') },
  { key: 'deptName', label: t('entity.employeejoined.deptname') },
  { key: 'postId', label: t('entity.employeejoined.postid') },
  { key: 'postName', label: t('entity.employeejoined.postname') },
  { key: 'jobTitle', label: t('entity.employeejoined.jobtitle') },
  { key: 'workNature', label: t('entity.employeejoined.worknature') },
  { key: 'employmentType', label: t('entity.employeejoined.employmenttype') },
  { key: 'directManagerId', label: t('entity.employeejoined.directmanagerid') },
  { key: 'directManagerName', label: t('entity.employeejoined.directmanagername') },
  { key: 'approvalStatus', label: t('entity.employeejoined.approvalstatus') },
  { key: 'initiatorId', label: t('entity.employeejoined.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.employeejoined.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.employeejoined.initiatedatend') },
  { key: 'approvedBy', label: t('entity.employeejoined.approvedby') },
  { key: 'approvedAtStart', label: t('entity.employeejoined.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.employeejoined.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.employeejoined.flowinstanceid') },
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
const entityIdName = 'employeeJoinedId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)



/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EmployeeJoinedQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EmployeeJoinedQuery>): EmployeeJoinedQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EmployeeJoinedQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EmployeeJoinedQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('employeeId', form.employeeId)
  assignTrimmed('onboardingId', form.onboardingId)
  assignTrimmed('joinedDateStart', form.joinedDateStart)
  assignTrimmed('joinedDateEnd', form.joinedDateEnd)
  assignTrimmed('probationEndDateStart', form.probationEndDateStart)
  assignTrimmed('probationEndDateEnd', form.probationEndDateEnd)
  assignTrimmed('regularDateStart', form.regularDateStart)
  assignTrimmed('regularDateEnd', form.regularDateEnd)
  assignTrimmed('deptId', form.deptId)
  assignTrimmed('deptName', form.deptName)
  assignTrimmed('postId', form.postId)
  assignTrimmed('postName', form.postName)
  assignTrimmed('jobTitle', form.jobTitle)
  if (form.workNature !== undefined && form.workNature !== null) {
    query.workNature = form.workNature
  }
  if (form.employmentType !== undefined && form.employmentType !== null) {
    query.employmentType = form.employmentType
  }
  assignTrimmed('directManagerId', form.directManagerId)
  assignTrimmed('directManagerName', form.directManagerName)
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
    dataIndex: 'employeeJoinedId',
    key: 'employeeJoinedId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'employeeJoinedId') ?? ''
  },
  {
    title: t('entity.employeejoined.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.employeejoined.onboardingid'),
    dataIndex: 'onboardingId',
    key: 'onboardingId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'onboardingId') ?? ''
  },
  {
    title: t('entity.employeejoined.joineddate'),
    dataIndex: 'joinedDate',
    key: 'joinedDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'joinedDate') ?? ''
  },
  {
    title: t('entity.employeejoined.probationenddate'),
    dataIndex: 'probationEndDate',
    key: 'probationEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'probationEndDate') ?? ''
  },
  {
    title: t('entity.employeejoined.regulardate'),
    dataIndex: 'regularDate',
    key: 'regularDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'regularDate') ?? ''
  },
  {
    title: t('entity.employeejoined.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.employeejoined.deptname'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'deptName') ?? ''
  },
  {
    title: t('entity.employeejoined.postid'),
    dataIndex: 'postId',
    key: 'postId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'postId') ?? ''
  },
  {
    title: t('entity.employeejoined.postname'),
    dataIndex: 'postName',
    key: 'postName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'postName') ?? ''
  },
  {
    title: t('entity.employeejoined.jobtitle'),
    dataIndex: 'jobTitle',
    key: 'jobTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'jobTitle') ?? ''
  },
  {
    title: t('entity.employeejoined.worknature'),
    dataIndex: 'workNature',
    key: 'workNature',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'workNature') ?? ''
  },
  {
    title: t('entity.employeejoined.employmenttype'),
    dataIndex: 'employmentType',
    key: 'employmentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'employmentType') ?? ''
  },
  {
    title: t('entity.employeejoined.directmanagerid'),
    dataIndex: 'directManagerId',
    key: 'directManagerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeJoinedField(record, 'directManagerId') ?? ''
  },
  {
    title: t('entity.employeejoined.directmanagername'),
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
        permission: 'human:resource:personnel:employee:joined:update',
        onClick: (record: EmployeeJoined) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:personnel:employee:joined:delete',
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
    } else if (selectedRow.value && getEmployeeJoinedId(selectedRow.value) === getEmployeeJoinedId(record)) {
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
    const res = await getEmployeeJoinedList(buildListQuery())
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
  currentPage.value = getTaktDefaultPageIndex()
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.employeejoined._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: EmployeeJoined) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.employeejoined._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.employeejoined._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.employeejoined._self') }))
    } else {
      await createEmployeeJoined(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.employeejoined._self') }))
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
    const exportMeta = await exportEmployeeJoined(
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
    message.success(t('common.feedback.export.success', { target: t('entity.employeejoined._self') }))
  } catch (error: any) {
    logger.error('[EmployeeJoined] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.employeejoined._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EmployeeJoined) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.employeejoined._self'), name: t('common.tip.this.target', { target: t('entity.employeejoined._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEmployeeJoinedById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.employeejoined._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.employeejoined._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.employeejoined._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEmployeeJoinedBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.employeejoined._self') }))
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
