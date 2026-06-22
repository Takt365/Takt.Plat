<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/personnel/employee -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="humanresource:personnel:employee:create"
      update-permission="humanresource:personnel:employee:update"
      delete-permission="humanresource:personnel:employee:delete"
      import-permission="humanresource:personnel:employee:import"
      export-permission="humanresource:personnel:employee:export"
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
      :id-column-key="'employeeId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEmployeeId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'nativePlace'">
          <TaktDictTag
            :value="getEmployeeField(record, 'nativePlace')"
            dict-type="hr_native_place_code"
          />
        </template>
        <template v-else-if="column.key === 'ethnicity'">
          <TaktDictTag
            :value="getEmployeeField(record, 'ethnicity')"
            dict-type="hr_ethnic_code"
          />
        </template>
        <template v-else-if="column.key === 'politicalStatus'">
          <TaktDictTag
            :value="getEmployeeField(record, 'politicalStatus')"
            dict-type="hr_political_status"
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
      <EmployeeForm
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
      :storage-key="'takt-query-fields-human-resource-personnel-employee'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('employeeNo')">
      <a-form-item :label="t('entity.employee.no')">
        <a-input
          v-model:value="advancedQueryForm.employeeNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.no') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('name')">
      <a-form-item :label="t('entity.employee.name')">
        <a-input
          v-model:value="advancedQueryForm.name"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.name') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('gender')">
      <a-form-item :label="t('entity.employee.gender')">
        <a-input-number
          v-model:value="advancedQueryForm.gender"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.gender') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('birthDateStart')">
      <a-form-item :label="t('entity.employee.birthdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.birthDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.birthdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('birthDateEnd')">
      <a-form-item :label="t('entity.employee.birthdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.birthDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.birthdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idCardNo')">
      <a-form-item :label="t('entity.employee.idcardno')">
        <a-input
          v-model:value="advancedQueryForm.idCardNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.idcardno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mobile')">
      <a-form-item :label="t('entity.employee.mobile')">
        <a-input
          v-model:value="advancedQueryForm.mobile"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.mobile') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('email')">
      <a-form-item :label="t('entity.employee.email')">
        <a-input
          v-model:value="advancedQueryForm.email"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.email') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nativePlace')">
      <a-form-item :label="t('entity.employee.nativeplace')">
        <TaktSelect
          v-model:value="advancedQueryForm.nativePlace"
          dict-type="hr_native_place_code"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.nativeplace') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ethnicity')">
      <a-form-item :label="t('entity.employee.ethnicity')">
        <TaktSelect
          v-model:value="advancedQueryForm.ethnicity"
          dict-type="hr_ethnic_code"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.ethnicity') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('politicalStatus')">
      <a-form-item :label="t('entity.employee.politicalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.politicalStatus"
          dict-type="hr_political_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.politicalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maritalStatus')">
      <a-form-item :label="t('entity.employee.maritalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.maritalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.maritalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('education')">
      <a-form-item :label="t('entity.employee.education')">
        <a-input-number
          v-model:value="advancedQueryForm.education"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.education') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('graduateSchool')">
      <a-form-item :label="t('entity.employee.graduateschool')">
        <a-input
          v-model:value="advancedQueryForm.graduateSchool"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.graduateschool') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('major')">
      <a-form-item :label="t('entity.employee.major')">
        <a-input
          v-model:value="advancedQueryForm.major"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.major') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('joinedDateStart')">
      <a-form-item :label="t('entity.employee.joineddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.joinedDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.joineddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('joinedDateEnd')">
      <a-form-item :label="t('entity.employee.joineddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.joinedDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.joineddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('probationEndDateStart')">
      <a-form-item :label="t('entity.employee.probationenddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.probationEndDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.probationenddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('probationEndDateEnd')">
      <a-form-item :label="t('entity.employee.probationenddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.probationEndDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.probationenddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('regularDateStart')">
      <a-form-item :label="t('entity.employee.regulardatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.regularDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.regulardatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('regularDateEnd')">
      <a-form-item :label="t('entity.employee.regulardateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.regularDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.regulardateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('terminationDateStart')">
      <a-form-item :label="t('entity.employee.terminationdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.terminationDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.terminationdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('terminationDateEnd')">
      <a-form-item :label="t('entity.employee.terminationdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.terminationDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.terminationdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastWorkDateStart')">
      <a-form-item :label="t('entity.employee.lastworkdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.lastWorkDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.lastworkdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastWorkDateEnd')">
      <a-form-item :label="t('entity.employee.lastworkdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.lastWorkDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.employee.lastworkdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resignationType')">
      <a-form-item :label="t('entity.employee.resignationtype')">
        <a-input-number
          v-model:value="advancedQueryForm.resignationType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.resignationtype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resignationReason')">
      <a-form-item :label="t('entity.employee.resignationreason')">
        <a-input
          v-model:value="advancedQueryForm.resignationReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.resignationreason') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeStatus')">
      <a-form-item :label="t('entity.employee.status')">
        <a-input-number
          v-model:value="advancedQueryForm.employeeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('primaryDeptId')">
      <a-form-item :label="t('entity.employee.primarydeptid')">
        <a-input
          v-model:value="advancedQueryForm.primaryDeptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.primarydeptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('primaryPostId')">
      <a-form-item :label="t('entity.employee.primarypostid')">
        <a-input
          v-model:value="advancedQueryForm.primaryPostId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.primarypostid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBuiltIn')">
      <a-form-item :label="t('entity.employee.isbuiltin')">
        <a-input-number
          v-model:value="advancedQueryForm.isBuiltIn"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.isbuiltin') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('emergencyContactName')">
      <a-form-item :label="t('entity.employee.emergencycontactname')">
        <a-input
          v-model:value="advancedQueryForm.emergencyContactName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.emergencycontactname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('emergencyContactPhone')">
      <a-form-item :label="t('entity.employee.emergencycontactphone')">
        <a-input
          v-model:value="advancedQueryForm.emergencyContactPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.emergencycontactphone') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('homeAddress')">
      <a-form-item :label="t('entity.employee.homeaddress')">
        <a-textarea
          v-model:value="advancedQueryForm.homeAddress"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.employee.homeaddress') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('photoUrl')">
      <a-form-item :label="t('entity.employee.photourl')">
        <a-input
          v-model:value="advancedQueryForm.photoUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.employee.photourl') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.employee._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.employee._self"
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
      :id-column-key="'employeeId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
/**
 * 员工实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/personnel/employee
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import EmployeeForm from './components/employee-form.vue'
import { getEmployeeList, getEmployeeById, createEmployee, updateEmployee, deleteEmployeeById, deleteEmployeeBatch, getEmployeeTemplate, importEmployee, exportEmployee } from '@/api/human-resource/personnel/employee'
import type { Employee, EmployeeQuery, EmployeeCreate, EmployeeUpdate } from '@/types/human-resource/personnel/employee'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEmployee')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.employee._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Employee[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Employee | null>(null)
/** 表格多选行 */
const selectedRows = ref<Employee[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Employee>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  employeeNo: '',
  name: '',
  gender: undefined as number | undefined,
  birthDateStart: '',
  birthDateEnd: '',
  idCardNo: '',
  mobile: '',
  email: '',
  nativePlace: '',
  ethnicity: undefined as number | undefined,
  politicalStatus: undefined as number | undefined,
  maritalStatus: undefined as number | undefined,
  education: undefined as number | undefined,
  graduateSchool: '',
  major: '',
  joinedDateStart: '',
  joinedDateEnd: '',
  probationEndDateStart: '',
  probationEndDateEnd: '',
  regularDateStart: '',
  regularDateEnd: '',
  terminationDateStart: '',
  terminationDateEnd: '',
  lastWorkDateStart: '',
  lastWorkDateEnd: '',
  resignationType: undefined as number | undefined,
  resignationReason: '',
  employeeStatus: undefined as number | undefined,
  primaryDeptId: '',
  primaryPostId: '',
  isBuiltIn: undefined as number | undefined,
  emergencyContactName: '',
  emergencyContactPhone: '',
  homeAddress: '',
  photoUrl: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'employeeNo', label: t('entity.employee.no') },
  { key: 'name', label: t('entity.employee.name') },
  { key: 'gender', label: t('entity.employee.gender') },
  { key: 'birthDateStart', label: t('entity.employee.birthdatestart') },
  { key: 'birthDateEnd', label: t('entity.employee.birthdateend') },
  { key: 'idCardNo', label: t('entity.employee.idcardno') },
  { key: 'mobile', label: t('entity.employee.mobile') },
  { key: 'email', label: t('entity.employee.email') },
  { key: 'nativePlace', label: t('entity.employee.nativeplace') },
  { key: 'ethnicity', label: t('entity.employee.ethnicity') },
  { key: 'politicalStatus', label: t('entity.employee.politicalstatus') },
  { key: 'maritalStatus', label: t('entity.employee.maritalstatus') },
  { key: 'education', label: t('entity.employee.education') },
  { key: 'graduateSchool', label: t('entity.employee.graduateschool') },
  { key: 'major', label: t('entity.employee.major') },
  { key: 'joinedDateStart', label: t('entity.employee.joineddatestart') },
  { key: 'joinedDateEnd', label: t('entity.employee.joineddateend') },
  { key: 'probationEndDateStart', label: t('entity.employee.probationenddatestart') },
  { key: 'probationEndDateEnd', label: t('entity.employee.probationenddateend') },
  { key: 'regularDateStart', label: t('entity.employee.regulardatestart') },
  { key: 'regularDateEnd', label: t('entity.employee.regulardateend') },
  { key: 'terminationDateStart', label: t('entity.employee.terminationdatestart') },
  { key: 'terminationDateEnd', label: t('entity.employee.terminationdateend') },
  { key: 'lastWorkDateStart', label: t('entity.employee.lastworkdatestart') },
  { key: 'lastWorkDateEnd', label: t('entity.employee.lastworkdateend') },
  { key: 'resignationType', label: t('entity.employee.resignationtype') },
  { key: 'resignationReason', label: t('entity.employee.resignationreason') },
  { key: 'employeeStatus', label: t('entity.employee.status') },
  { key: 'primaryDeptId', label: t('entity.employee.primarydeptid') },
  { key: 'primaryPostId', label: t('entity.employee.primarypostid') },
  { key: 'isBuiltIn', label: t('entity.employee.isbuiltin') },
  { key: 'emergencyContactName', label: t('entity.employee.emergencycontactname') },
  { key: 'emergencyContactPhone', label: t('entity.employee.emergencycontactphone') },
  { key: 'homeAddress', label: t('entity.employee.homeaddress') },
  { key: 'photoUrl', label: t('entity.employee.photourl') },
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
const entityIdName = 'employeeId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

type EmployeeQueryTrimmedKey =
  | 'employeeNo'
  | 'name'
  | 'birthDateStart'
  | 'birthDateEnd'
  | 'idCardNo'
  | 'mobile'
  | 'email'
  | 'nativePlace'
  | 'graduateSchool'
  | 'major'
  | 'joinedDateStart'
  | 'joinedDateEnd'
  | 'probationEndDateStart'
  | 'probationEndDateEnd'
  | 'regularDateStart'
  | 'regularDateEnd'
  | 'terminationDateStart'
  | 'terminationDateEnd'
  | 'lastWorkDateStart'
  | 'lastWorkDateEnd'
  | 'resignationReason'
  | 'primaryDeptId'
  | 'primaryPostId'
  | 'emergencyContactName'
  | 'emergencyContactPhone'
  | 'homeAddress'
  | 'photoUrl'
  | 'createdAtStart'
  | 'createdAtEnd'
  | 'ExtField'
  | 'remark'

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EmployeeQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EmployeeQuery>): EmployeeQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EmployeeQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: EmployeeQueryTrimmedKey, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v
    }
  }
  assignTrimmed('employeeNo', form.employeeNo)
  assignTrimmed('name', form.name)
  assignTrimmed('birthDateStart', form.birthDateStart)
  assignTrimmed('birthDateEnd', form.birthDateEnd)
  assignTrimmed('idCardNo', form.idCardNo)
  assignTrimmed('mobile', form.mobile)
  assignTrimmed('email', form.email)
  assignTrimmed('nativePlace', form.nativePlace)
  assignTrimmed('graduateSchool', form.graduateSchool)
  assignTrimmed('major', form.major)
  assignTrimmed('joinedDateStart', form.joinedDateStart)
  assignTrimmed('joinedDateEnd', form.joinedDateEnd)
  assignTrimmed('probationEndDateStart', form.probationEndDateStart)
  assignTrimmed('probationEndDateEnd', form.probationEndDateEnd)
  assignTrimmed('regularDateStart', form.regularDateStart)
  assignTrimmed('regularDateEnd', form.regularDateEnd)
  assignTrimmed('terminationDateStart', form.terminationDateStart)
  assignTrimmed('terminationDateEnd', form.terminationDateEnd)
  assignTrimmed('lastWorkDateStart', form.lastWorkDateStart)
  assignTrimmed('lastWorkDateEnd', form.lastWorkDateEnd)
  assignTrimmed('resignationReason', form.resignationReason)
  assignTrimmed('primaryDeptId', form.primaryDeptId)
  assignTrimmed('primaryPostId', form.primaryPostId)
  assignTrimmed('emergencyContactName', form.emergencyContactName)
  assignTrimmed('emergencyContactPhone', form.emergencyContactPhone)
  assignTrimmed('homeAddress', form.homeAddress)
  assignTrimmed('photoUrl', form.photoUrl)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('ExtField', form.ExtField)
  assignTrimmed('remark', form.remark)
  if (form.gender !== undefined && form.gender !== null) {
    query.gender = form.gender
  }
  if (form.ethnicity !== undefined && form.ethnicity !== null) {
    query.ethnicity = form.ethnicity
  }
  if (form.politicalStatus !== undefined && form.politicalStatus !== null) {
    query.politicalStatus = form.politicalStatus
  }
  if (form.maritalStatus !== undefined && form.maritalStatus !== null) {
    query.maritalStatus = form.maritalStatus
  }
  if (form.education !== undefined && form.education !== null) {
    query.education = form.education
  }
  if (form.resignationType !== undefined && form.resignationType !== null) {
    query.resignationType = form.resignationType
  }
  if (form.employeeStatus !== undefined && form.employeeStatus !== null) {
    query.employeeStatus = form.employeeStatus
  }
  if (form.isBuiltIn !== undefined && form.isBuiltIn !== null) {
    query.isBuiltIn = form.isBuiltIn
  }
  return query
}

/** 页面挂载：加载分页配置后拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})






/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.employee.no'),
    dataIndex: 'employeeNo',
    key: 'employeeNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'employeeNo') ?? ''
  },
  {
    title: t('entity.employee.name'),
    dataIndex: 'name',
    key: 'name',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'name') ?? ''
  },
  {
    title: t('entity.employee.gender'),
    dataIndex: 'gender',
    key: 'gender',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'gender') ?? ''
  },
  {
    title: t('entity.employee.birthdate'),
    dataIndex: 'birthDate',
    key: 'birthDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'birthDate') ?? ''
  },
  {
    title: t('entity.employee.idcardno'),
    dataIndex: 'idCardNo',
    key: 'idCardNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'idCardNo') ?? ''
  },
  {
    title: t('entity.employee.mobile'),
    dataIndex: 'mobile',
    key: 'mobile',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'mobile') ?? ''
  },
  {
    title: t('entity.employee.email'),
    dataIndex: 'email',
    key: 'email',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'email') ?? ''
  },
  {
    title: t('entity.employee.nativeplace'),
    dataIndex: 'nativePlace',
    key: 'nativePlace',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.employee.ethnicity'),
    dataIndex: 'ethnicity',
    key: 'ethnicity',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.employee.politicalstatus'),
    dataIndex: 'politicalStatus',
    key: 'politicalStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.employee.maritalstatus'),
    dataIndex: 'maritalStatus',
    key: 'maritalStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'maritalStatus') ?? ''
  },
  {
    title: t('entity.employee.education'),
    dataIndex: 'education',
    key: 'education',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'education') ?? ''
  },
  {
    title: t('entity.employee.graduateschool'),
    dataIndex: 'graduateSchool',
    key: 'graduateSchool',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'graduateSchool') ?? ''
  },
  {
    title: t('entity.employee.major'),
    dataIndex: 'major',
    key: 'major',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'major') ?? ''
  },
  {
    title: t('entity.employee.joineddate'),
    dataIndex: 'joinedDate',
    key: 'joinedDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'joinedDate') ?? ''
  },
  {
    title: t('entity.employee.probationenddate'),
    dataIndex: 'probationEndDate',
    key: 'probationEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'probationEndDate') ?? ''
  },
  {
    title: t('entity.employee.regulardate'),
    dataIndex: 'regularDate',
    key: 'regularDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'regularDate') ?? ''
  },
  {
    title: t('entity.employee.terminationdate'),
    dataIndex: 'terminationDate',
    key: 'terminationDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'terminationDate') ?? ''
  },
  {
    title: t('entity.employee.lastworkdate'),
    dataIndex: 'lastWorkDate',
    key: 'lastWorkDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'lastWorkDate') ?? ''
  },
  {
    title: t('entity.employee.resignationtype'),
    dataIndex: 'resignationType',
    key: 'resignationType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'resignationType') ?? ''
  },
  {
    title: t('entity.employee.resignationreason'),
    dataIndex: 'resignationReason',
    key: 'resignationReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'resignationReason') ?? ''
  },
  {
    title: t('entity.employee.status'),
    dataIndex: 'employeeStatus',
    key: 'employeeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'employeeStatus') ?? ''
  },
  {
    title: t('entity.employee.primarydeptid'),
    dataIndex: 'primaryDeptId',
    key: 'primaryDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'primaryDeptId') ?? ''
  },
  {
    title: t('entity.employee.primarydeptname'),
    dataIndex: 'primaryDeptName',
    key: 'primaryDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'primaryDeptName') ?? ''
  },
  {
    title: t('entity.employee.primarypostid'),
    dataIndex: 'primaryPostId',
    key: 'primaryPostId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'primaryPostId') ?? ''
  },
  {
    title: t('entity.employee.primarypostname'),
    dataIndex: 'primaryPostName',
    key: 'primaryPostName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'primaryPostName') ?? ''
  },
  {
    title: t('entity.employee.isbuiltin'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'isBuiltIn') ?? ''
  },
  {
    title: t('entity.employee.emergencycontactname'),
    dataIndex: 'emergencyContactName',
    key: 'emergencyContactName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'emergencyContactName') ?? ''
  },
  {
    title: t('entity.employee.emergencycontactphone'),
    dataIndex: 'emergencyContactPhone',
    key: 'emergencyContactPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'emergencyContactPhone') ?? ''
  },
  {
    title: t('entity.employee.homeaddress'),
    dataIndex: 'homeAddress',
    key: 'homeAddress',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'homeAddress') ?? ''
  },
  {
    title: t('entity.employee.photourl'),
    dataIndex: 'photoUrl',
    key: 'photoUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'photoUrl') ?? ''
  },
  {
    title: t('entity.employee.depts'),
    dataIndex: 'employeeDepts',
    key: 'employeeDepts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'employeeDepts') ?? ''
  },
  {
    title: t('entity.employee.posts'),
    dataIndex: 'employeePosts',
    key: 'employeePosts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEmployeeField(record, 'employeePosts') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:personnel:employee:update',
        onClick: (record: Employee) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:personnel:employee:delete',
        onClick: (record: Employee) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEmployeeId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEmployeeField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Employee[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Employee, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEmployeeId(selectedRow.value) === getEmployeeId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Employee[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Employee) => ({
  onClick: () => {
    const key = getEmployeeId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEmployeeId(item)))
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
    const res = await getEmployeeList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Employee] 加载数据失败', { error })
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
  employeeNo: '',
  name: '',
  gender: undefined as number | undefined,
  birthDateStart: '',
  birthDateEnd: '',
  idCardNo: '',
  mobile: '',
  email: '',
  nativePlace: '',
  ethnicity: undefined as number | undefined,
  politicalStatus: undefined as number | undefined,
  maritalStatus: undefined as number | undefined,
  education: undefined as number | undefined,
  graduateSchool: '',
  major: '',
  joinedDateStart: '',
  joinedDateEnd: '',
  probationEndDateStart: '',
  probationEndDateEnd: '',
  regularDateStart: '',
  regularDateEnd: '',
  terminationDateStart: '',
  terminationDateEnd: '',
  lastWorkDateStart: '',
  lastWorkDateEnd: '',
  resignationType: undefined as number | undefined,
  resignationReason: '',
  employeeStatus: undefined as number | undefined,
  primaryDeptId: '',
  primaryPostId: '',
  isBuiltIn: undefined as number | undefined,
  emergencyContactName: '',
  emergencyContactPhone: '',
  homeAddress: '',
  photoUrl: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.employee._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: Employee) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.employee._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.employee._self') }))
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
      await updateEmployee(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.employee._self') }))
    } else {
      await createEmployee(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.employee._self') }))
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
  const res = await getEmployeeTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEmployee(file, sheetName)
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
    const exportMeta = await exportEmployee(
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
    message.success(t('common.feedback.export.success', { target: t('entity.employee._self') }))
  } catch (error: any) {
    logger.error('[Employee] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.employee._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Employee) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.employee._self'), name: t('common.tip.this.target', { target: t('entity.employee._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEmployeeById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.employee._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.employee._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.employee._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEmployeeBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.employee._self') }))
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
  employeeNo: '',
  name: '',
  gender: undefined as number | undefined,
  birthDateStart: '',
  birthDateEnd: '',
  idCardNo: '',
  mobile: '',
  email: '',
  nativePlace: '',
  ethnicity: undefined as number | undefined,
  politicalStatus: undefined as number | undefined,
  maritalStatus: undefined as number | undefined,
  education: undefined as number | undefined,
  graduateSchool: '',
  major: '',
  joinedDateStart: '',
  joinedDateEnd: '',
  probationEndDateStart: '',
  probationEndDateEnd: '',
  regularDateStart: '',
  regularDateEnd: '',
  terminationDateStart: '',
  terminationDateEnd: '',
  lastWorkDateStart: '',
  lastWorkDateEnd: '',
  resignationType: undefined as number | undefined,
  resignationReason: '',
  employeeStatus: undefined as number | undefined,
  primaryDeptId: '',
  primaryPostId: '',
  isBuiltIn: undefined as number | undefined,
  emergencyContactName: '',
  emergencyContactPhone: '',
  homeAddress: '',
  photoUrl: '',
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
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
