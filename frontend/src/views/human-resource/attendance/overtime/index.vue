<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/attendance/overtime -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：加班申请管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
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
      create-permission="human:resource:attendance:overtime:create"
      update-permission="human:resource:attendance:overtime:update"
      delete-permission="human:resource:attendance:overtime:delete"
      import-permission="human:resource:attendance:overtime:import"
      export-permission="human:resource:attendance:overtime:export"
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

    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getOvertimeId"
      :master-row-selection="rowSelection"
      master-id-column-key="overtimeId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="approval"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
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
            dict-type="sys_approval_status"
          />
        </template>
      </template>
      <template #detail>
        <OvertimeItemPanel
          ref="overtimeItemPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="1100px"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <OvertimeForm
        :key="formData?.overtimeId ?? 'create'"
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
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="t('entity.overtime.deptname')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.deptname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimeDateStart')">
      <a-form-item :label="t('entity.overtime.datestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.overtimeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.datestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimeDateEnd')">
      <a-form-item :label="t('entity.overtime.dateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.overtimeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.dateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeStart')">
      <a-form-item :label="t('entity.overtime.plannedstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.plannedstarttimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeEnd')">
      <a-form-item :label="t('entity.overtime.plannedstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.plannedstarttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeStart')">
      <a-form-item :label="t('entity.overtime.plannedendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.plannedendtimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedEndTimeEnd')">
      <a-form-item :label="t('entity.overtime.plannedendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.plannedendtimeend') })"
          value-format="YYYY-MM-DD"
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
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.overtime.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.relatedplant') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingBy')">
      <a-form-item :label="t('entity.overtime.handlingby')">
        <a-input
          v-model:value="advancedQueryForm.handlingBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtStart')">
      <a-form-item :label="t('entity.overtime.handlingatstart')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtEnd')">
      <a-form-item :label="t('entity.overtime.handlingatend')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingatend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingComment')">
      <a-form-item :label="t('entity.overtime.handlingcomment')">
        <a-input
          v-model:value="advancedQueryForm.handlingComment"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingcomment') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimeStatus')">
      <a-form-item :label="t('entity.overtime.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.overtimeStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.overtime.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.overtime.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.overtime.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.initiatedatstart') })"
          show-count
          :maxlength="20"
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
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.overtime.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.approvedatstart') })"
          show-count
          :maxlength="20"
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
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.overtime.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.flowinstanceid') })"
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
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import OvertimeForm from './components/overtime-form.vue'
import OvertimeItemPanel from './components/overtime-item-panel.vue'
import { provideOvertimeMasterContext } from './composables/use-overtime-master-context'
import { getOvertimeList, getOvertimeById, createOvertime, updateOvertime, deleteOvertimeById, deleteOvertimeBatch, getOvertimeTemplate, importOvertime, exportOvertime, updateOvertimeStatus } from '@/api/human-resource/attendance/overtime'
import type { Overtime, OvertimeQuery } from '@/types/human-resource/attendance/overtime'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

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
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
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
const formData = ref<Partial<Overtime> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
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
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'deptId', label: t('entity.overtime.deptid') },
  { key: 'deptName', label: t('entity.overtime.deptname') },
  { key: 'overtimeDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.overtime.date')) },
  { key: 'overtimeDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.overtime.date')) },
  { key: 'plannedStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.overtime.plannedstarttime')) },
  { key: 'plannedStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.overtime.plannedstarttime')) },
  { key: 'plannedEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.overtime.plannedendtime')) },
  { key: 'plannedEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.overtime.plannedendtime')) },
  { key: 'totalEmployees', label: t('entity.overtime.totalemployees') },
  { key: 'totalPlannedHours', label: t('entity.overtime.totalplannedhours') },
  { key: 'totalActualHours', label: t('entity.overtime.totalactualhours') },
  { key: 'overtimeType', label: t('entity.overtime.type') },
  { key: 'reason', label: t('entity.overtime.reason') },
  { key: 'relatedPlant', label: t('entity.overtime.relatedplant') },
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
  { key: 'flowInstanceId', label: t('entity.overtime.flowinstanceid') },
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
const entityIdName = 'overtimeId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideOvertimeMasterContext()
const overtimeItemPanelRef = ref<InstanceType<typeof OvertimeItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {OvertimeQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<OvertimeQuery>): OvertimeQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: OvertimeQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof OvertimeQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('deptId', form.deptId)
  assignTrimmed('deptName', form.deptName)
  assignTrimmed('overtimeDateStart', form.overtimeDateStart)
  assignTrimmed('overtimeDateEnd', form.overtimeDateEnd)
  assignTrimmed('plannedStartTimeStart', form.plannedStartTimeStart)
  assignTrimmed('plannedStartTimeEnd', form.plannedStartTimeEnd)
  assignTrimmed('plannedEndTimeStart', form.plannedEndTimeStart)
  assignTrimmed('plannedEndTimeEnd', form.plannedEndTimeEnd)
  if (form.totalEmployees !== undefined && form.totalEmployees !== null) {
    query.totalEmployees = form.totalEmployees
  }
  if (form.totalPlannedHours !== undefined && form.totalPlannedHours !== null) {
    query.totalPlannedHours = form.totalPlannedHours
  }
  if (form.totalActualHours !== undefined && form.totalActualHours !== null) {
    query.totalActualHours = form.totalActualHours
  }
  if (form.overtimeType !== undefined && form.overtimeType !== null) {
    query.overtimeType = form.overtimeType
  }
  assignTrimmed('reason', form.reason)
  assignTrimmed('relatedPlant', form.relatedPlant)
  assignTrimmed('handlingBy', form.handlingBy)
  assignTrimmed('handlingAtStart', form.handlingAtStart)
  assignTrimmed('handlingAtEnd', form.handlingAtEnd)
  assignTrimmed('handlingComment', form.handlingComment)
  if (form.overtimeStatus !== undefined && form.overtimeStatus !== null) {
    query.overtimeStatus = form.overtimeStatus
  }
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
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: Overtime | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getOvertimeId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as Overtime
  const key = getOvertimeId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
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
        permission: 'human:resource:attendance:overtime:update',
        onClick: (record: Overtime) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:attendance:overtime:delete',
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
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: Overtime, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getOvertimeId(selectedRow.value) === getOvertimeId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Overtime[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getOvertimeList(buildListQuery())
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
  currentPage.value = getTaktDefaultPageIndex()
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.overtime._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
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
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  overtimeItemPanelRef.value?.reload?.()
    }
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
    const exportMeta = await exportOvertime(
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
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
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
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
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
</script>
