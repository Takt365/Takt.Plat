<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/attendance/overtime -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：加班申请管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-attendance-overtime">
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
      create-permission="humanresource:attendance:overtime:create"
      update-permission="humanresource:attendance:overtime:update"
      delete-permission="humanresource:attendance:overtime:delete"
      import-permission="humanresource:attendance:overtime:import"
      export-permission="humanresource:attendance:overtime:export"
      start-flow-permission="humanresource:attendance:overtime:update"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-start-flow="true"
      :show-expand="true"
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
      :start-flow-disabled="submitApprovalDisabled"
      :start-flow-loading="submitApprovalLoading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @start-flow="handleSubmitApproval"
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
      :id-column-key="'overtimeId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getOvertimeId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'overtimeType'">
          <TaktDictTag
            :value="getOvertimeField(record, 'overtimeType')"
            dict-type="hr_overtime_type"
          />
        </template>
        <template v-else-if="column.key === 'overtimeStatus'">
          <TaktDictTag
            :value="getOvertimeField(record, 'overtimeStatus')"
            dict-type="hr_overtime_status"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.overtimeItem._self') }}</div>
          <a-table
            v-if="hasOvertimeItemRows(record)"
            :columns="overtimeItemExpandColumns"
            :data-source="getOvertimeItemRows(record)"
            :row-key="(row: OvertimeItem, index?: number) => row?.overtimeItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
        </div>
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
      <OvertimeForm
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
      :storage-key="'takt-query-fields-human-resource-attendance-overtime'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="t('entity.overtime.deptid')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.deptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="t('entity.overtime.deptname')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.deptname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimeDateStart')">
      <a-form-item :label="t('entity.overtime.datestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.overtimeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.datestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimeDateEnd')">
      <a-form-item :label="t('entity.overtime.dateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.overtimeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.dateend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeStart')">
      <a-form-item :label="t('entity.overtime.plannedstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.plannedstarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeEnd')">
      <a-form-item :label="t('entity.overtime.plannedstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.plannedstarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeStart')">
      <a-form-item :label="t('entity.overtime.plannedendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.plannedendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeEnd')">
      <a-form-item :label="t('entity.overtime.plannedendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.plannedendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalEmployees')">
      <a-form-item :label="t('entity.overtime.totalemployees')">
        <a-input-number
          v-model:value="advancedQueryForm.totalEmployees"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.totalemployees') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalPlannedHours')">
      <a-form-item :label="t('entity.overtime.totalplannedhours')">
        <a-input-number
          v-model:value="advancedQueryForm.totalPlannedHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.totalplannedhours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalActualHours')">
      <a-form-item :label="t('entity.overtime.totalactualhours')">
        <a-input-number
          v-model:value="advancedQueryForm.totalActualHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.totalactualhours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimeType')">
      <a-form-item :label="t('entity.overtime.type')">
        <TaktSelect
          v-model:value="advancedQueryForm.overtimeType"
          dict-type="hr_overtime_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.type') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reason')">
      <a-form-item :label="t('entity.overtime.reason')">
        <a-input
          v-model:value="advancedQueryForm.reason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.reason') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.overtime.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.relatedplant') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.overtime.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.flowinstanceid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingBy')">
      <a-form-item :label="t('entity.overtime.handlingby')">
        <a-input
          v-model:value="advancedQueryForm.handlingBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtStart')">
      <a-form-item :label="t('entity.overtime.handlingatstart')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtEnd')">
      <a-form-item :label="t('entity.overtime.handlingatend')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingatend') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingComment')">
      <a-form-item :label="t('entity.overtime.handlingcomment')">
        <a-input
          v-model:value="advancedQueryForm.handlingComment"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingcomment') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimeStatus')">
      <a-form-item :label="t('entity.overtime.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.overtimeStatus"
          dict-type="hr_overtime_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.overtime.approvalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.approvalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.approvalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.overtime.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.initiatorid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.overtime.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.initiatedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.overtime.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.overtime.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.approvedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.overtime.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.approvedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.overtime.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.approvedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.overtime._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.overtime._self"
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
      :id-column-key="'overtimeId'"
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
 * 加班申请管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/attendance/overtime
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import OvertimeForm from './components/overtime-form.vue'
import { getOvertimeList, getOvertimeById, createOvertime, updateOvertime, deleteOvertimeById, deleteOvertimeBatch, getOvertimeTemplate, importOvertime, exportOvertime, submitOvertimeForApproval } from '@/api/human-resource/attendance/overtime'
import * as overtimeItemApi from '@/api/human-resource/attendance/overtime-item'
import type { OvertimeItem, OvertimeItemQuery } from '@/types/human-resource/attendance/overtime-item'
import type { Overtime, OvertimeQuery, OvertimeCreate, OvertimeUpdate } from '@/types/human-resource/attendance/overtime'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktOvertime')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.overtime._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Overtime[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Overtime | null>(null)
/** 表格多选行 */
const selectedRows = ref<Overtime[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Overtime>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  deptId: '',
  deptName: '',
  overtimeDateStart: '',
  overtimeDateEnd: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  plannedEndTimeStart: '',
  plannedEndTimeEnd: '',
  totalEmployees: undefined as number | undefined,
  totalPlannedHours: undefined as number | undefined,
  totalActualHours: undefined as number | undefined,
  overtimeType: undefined as number | undefined,
  reason: '',
  relatedPlant: '',
  flowInstanceId: '',
  handlingBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  handlingComment: '',
  overtimeStatus: undefined as number | undefined,
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
  { key: 'deptId', label: t('entity.overtime.deptid') },
  { key: 'deptName', label: t('entity.overtime.deptname') },
  { key: 'overtimeDateStart', label: t('entity.overtime.datestart') },
  { key: 'overtimeDateEnd', label: t('entity.overtime.dateend') },
  { key: 'plannedStartTimeStart', label: t('entity.overtime.plannedstarttimestart') },
  { key: 'plannedStartTimeEnd', label: t('entity.overtime.plannedstarttimeend') },
  { key: 'plannedEndTimeStart', label: t('entity.overtime.plannedendtimestart') },
  { key: 'plannedEndTimeEnd', label: t('entity.overtime.plannedendtimeend') },
  { key: 'totalEmployees', label: t('entity.overtime.totalemployees') },
  { key: 'totalPlannedHours', label: t('entity.overtime.totalplannedhours') },
  { key: 'totalActualHours', label: t('entity.overtime.totalactualhours') },
  { key: 'overtimeType', label: t('entity.overtime.type') },
  { key: 'reason', label: t('entity.overtime.reason') },
  { key: 'relatedPlant', label: t('entity.overtime.relatedplant') },
  { key: 'flowInstanceId', label: t('entity.overtime.flowinstanceid') },
  { key: 'handlingBy', label: t('entity.overtime.handlingby') },
  { key: 'handlingAtStart', label: t('entity.overtime.handlingatstart') },
  { key: 'handlingAtEnd', label: t('entity.overtime.handlingatend') },
  { key: 'handlingComment', label: t('entity.overtime.handlingcomment') },
  { key: 'overtimeStatus', label: t('entity.overtime.status') },
  { key: 'approvalStatus', label: t('entity.overtime.approvalstatus') },
  { key: 'initiatorId', label: t('entity.overtime.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.overtime.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.overtime.initiatedatend') },
  { key: 'approvedBy', label: t('entity.overtime.approvedby') },
  { key: 'approvedAtStart', label: t('entity.overtime.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.overtime.approvedatend') },
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
const entityIdName = 'overtimeId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)
/** 提交审批：须单行且状态为草稿或已驳回 */
const submitApprovalDisabled = computed(() => {
  if (selectedRows.value.length !== 1) {
    return true
  }
  const status = Number(getOvertimeField(selectedRows.value[0], 'overtimeStatus'))
  return status !== 0 && status !== 3
})
/** 提交审批 loading */
const submitApprovalLoading = ref(false)

/** 主子表展开行 keys（手风琴，仅一行展开） */
const expandedRowKeys = ref<string[]>([])

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 展开行预览：overtimeItem 列 */
const overtimeItemExpandColumns = computed(() => [
  {
    title: t('entity.overtimeItem.overtimename'),
    dataIndex: 'overtimeName',
    key: 'overtimeName',
    ellipsis: true,
  },
  {
    title: t('entity.overtimeItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.overtimeItem.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    ellipsis: true,
  },
  {
    title: t('entity.overtimeItem.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    ellipsis: true,
  },
  {
    title: t('entity.overtimeItem.plannedhours'),
    dataIndex: 'plannedHours',
    key: 'plannedHours',
    ellipsis: true,
  },
  {
    title: t('entity.overtimeItem.actualstarttime'),
    dataIndex: 'actualStartTime',
    key: 'actualStartTime',
    ellipsis: true,
  },
  {
    title: t('entity.overtimeItem.actualendtime'),
    dataIndex: 'actualEndTime',
    key: 'actualEndTime',
    ellipsis: true,
  },
  {
    title: t('entity.overtimeItem.actualhours'),
    dataIndex: 'actualHours',
    key: 'actualHours',
    ellipsis: true,
  },
])

/** 读取主表行上的 overtimeItem 子表缓存 */
function getOvertimeItemRows(record: Overtime): OvertimeItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 overtimeItem 子表 */
function hasOvertimeItemRows(record: Overtime): boolean {
  return getOvertimeItemRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadOvertimeDetail(record: Overtime): Promise<Overtime | null> {
  const id = getOvertimeId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getOvertimeById(id)
    const index = dataSource.value.findIndex((row) => getOvertimeId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Overtime
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 overtimeItem 子表（OvertimeItemQuery + overtimeItemApi，与主表 OvertimeQuery 分离） */
async function loadOvertimeItemForOvertime(record: Overtime): Promise<OvertimeItem[]> {
  const masterId = getOvertimeId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: OvertimeItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      overtimeId: masterId,
    }
    const result = await overtimeItemApi.getOvertimeItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getOvertimeId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as Overtime
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureOvertimeChildrenLoaded(record: Overtime) {
  if (!hasOvertimeItemRows(record)) {
    await loadOvertimeItemForOvertime(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: Overtime) {
  const key = getOvertimeId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureOvertimeChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'overtimeId',
    key: 'overtimeId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'overtimeId') ?? ''
  },
  {
    title: t('entity.overtime.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.overtime.deptname'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'deptName') ?? ''
  },
  {
    title: t('entity.overtime.date'),
    dataIndex: 'overtimeDate',
    key: 'overtimeDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'overtimeDate') ?? ''
  },
  {
    title: t('entity.overtime.plannedstarttime'),
    dataIndex: 'plannedStartTime',
    key: 'plannedStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'plannedStartTime') ?? ''
  },
  {
    title: t('entity.overtime.plannedendtime'),
    dataIndex: 'plannedEndTime',
    key: 'plannedEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'plannedEndTime') ?? ''
  },
  {
    title: t('entity.overtime.totalemployees'),
    dataIndex: 'totalEmployees',
    key: 'totalEmployees',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'totalEmployees') ?? ''
  },
  {
    title: t('entity.overtime.totalplannedhours'),
    dataIndex: 'totalPlannedHours',
    key: 'totalPlannedHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'totalPlannedHours') ?? ''
  },
  {
    title: t('entity.overtime.totalactualhours'),
    dataIndex: 'totalActualHours',
    key: 'totalActualHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'totalActualHours') ?? ''
  },
  {
    title: t('entity.overtime.type'),
    dataIndex: 'overtimeType',
    key: 'overtimeType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.overtime.reason'),
    dataIndex: 'reason',
    key: 'reason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'reason') ?? ''
  },
  {
    title: t('entity.overtime.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'relatedPlant') ?? ''
  },
  {
    title: t('entity.overtime.flowinstanceid'),
    dataIndex: 'flowInstanceId',
    key: 'flowInstanceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'flowInstanceId') ?? ''
  },
  {
    title: t('entity.overtime.flowinstancename'),
    dataIndex: 'flowInstanceName',
    key: 'flowInstanceName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'flowInstanceName') ?? ''
  },
  {
    title: t('entity.overtime.handlingby'),
    dataIndex: 'handlingBy',
    key: 'handlingBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'handlingBy') ?? ''
  },
  {
    title: t('entity.overtime.handlingat'),
    dataIndex: 'handlingAt',
    key: 'handlingAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'handlingAt') ?? ''
  },
  {
    title: t('entity.overtime.handlingcomment'),
    dataIndex: 'handlingComment',
    key: 'handlingComment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOvertimeField(record, 'handlingComment') ?? ''
  },
  {
    title: t('entity.overtime.status'),
    dataIndex: 'overtimeStatus',
    key: 'overtimeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:attendance:overtime:update',
        onClick: (record: Overtime) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:attendance:overtime:delete',
        onClick: (record: Overtime) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getOvertimeId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getOvertimeField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Overtime[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Overtime, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getOvertimeId(selectedRow.value) === getOvertimeId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Overtime[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Overtime) => ({
  onClick: () => {
    const key = getOvertimeId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getOvertimeId(item)))
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
    const params: OvertimeQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getOvertimeList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Overtime] 加载数据失败', { error })
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
  deptId: '',
  deptName: '',
  overtimeDateStart: '',
  overtimeDateEnd: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  plannedEndTimeStart: '',
  plannedEndTimeEnd: '',
  totalEmployees: undefined as number | undefined,
  totalPlannedHours: undefined as number | undefined,
  totalActualHours: undefined as number | undefined,
  overtimeType: undefined as number | undefined,
  reason: '',
  relatedPlant: '',
  flowInstanceId: '',
  handlingBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  handlingComment: '',
  overtimeStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.overtime._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Overtime) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.overtime._self') })
  formLoading.value = true
  try {
    const detail = await loadOvertimeDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.overtime._self') }))
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
      await updateOvertime(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.overtime._self') }))
    } else {
      await createOvertime(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.overtime._self') }))
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
  const res = await getOvertimeTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importOvertime(file, sheetName)
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
    const exportQuery: OvertimeQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportOvertime(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.overtime._self') }))
  } catch (error: any) {
    logger.error('[Overtime] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.overtime._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Overtime) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.overtime._self'), name: t('common.tip.this.target', { target: t('entity.overtime._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteOvertimeById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.overtime._self') }))
      loadData()
    }
  })
}
/** 提交加班审批（发起工作流） */
async function handleSubmitApproval() {
  if (submitApprovalDisabled.value || selectedRows.value.length !== 1) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.startflow'), entity: t('entity.overtime._self') }))
    return
  }
  const id = getOvertimeId(selectedRows.value[0])
  if (!id) {
    return
  }
  submitApprovalLoading.value = true
  try {
    await submitOvertimeForApproval(id)
    message.success(t('common.feedback.updated', { target: t('entity.overtime._self') }))
    await loadData()
  } catch (err: unknown) {
    message.error(err instanceof Error ? err.message : t('common.feedback.failed'))
  } finally {
    submitApprovalLoading.value = false
  }
}

/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.overtime._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.overtime._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteOvertimeBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.overtime._self') }))
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
  deptId: '',
  deptName: '',
  overtimeDateStart: '',
  overtimeDateEnd: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  plannedEndTimeStart: '',
  plannedEndTimeEnd: '',
  totalEmployees: undefined as number | undefined,
  totalPlannedHours: undefined as number | undefined,
  totalActualHours: undefined as number | undefined,
  overtimeType: undefined as number | undefined,
  reason: '',
  relatedPlant: '',
  flowInstanceId: '',
  handlingBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  handlingComment: '',
  overtimeStatus: undefined as number | undefined,
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
.human-resource-attendance-overtime {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
