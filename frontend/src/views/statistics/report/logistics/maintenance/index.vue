<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/statistics/report/logistics/maintenance -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt设备维护记录实体管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="statistics-report-logistics-maintenance">
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
      create-permission="statistics:report:logistics:maintenance:create"
      update-permission="statistics:report:logistics:maintenance:update"
      delete-permission="statistics:report:logistics:maintenance:delete"
      import-permission="statistics:report:logistics:maintenance:import"
      export-permission="statistics:report:logistics:maintenance:export"
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
      :id-column-key="'maintenanceId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getMaintenanceId"
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
      <MaintenanceForm
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
      :storage-key="'takt-query-fields-statistics-report-logistics-maintenance'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('equipmentId')">
      <a-form-item :label="t('entity.maintenance.equipmentid')">
        <a-input
          v-model:value="advancedQueryForm.equipmentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.equipmentid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentCode')">
      <a-form-item :label="t('entity.maintenance.equipmentcode')">
        <a-input
          v-model:value="advancedQueryForm.equipmentCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.equipmentcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.maintenance.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceType')">
      <a-form-item :label="t('entity.maintenance.type')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCompany')">
      <a-form-item :label="t('entity.maintenance.company')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceCompany"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.company') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceTechnician')">
      <a-form-item :label="t('entity.maintenance.technician')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceTechnician"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.technician') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceDateStart')">
      <a-form-item :label="t('entity.maintenance.datestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.datestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceDateEnd')">
      <a-form-item :label="t('entity.maintenance.dateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.dateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceStartTimeStart')">
      <a-form-item :label="t('entity.maintenance.starttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.starttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceStartTimeEnd')">
      <a-form-item :label="t('entity.maintenance.starttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.starttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceEndTimeStart')">
      <a-form-item :label="t('entity.maintenance.endtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.endtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceEndTimeEnd')">
      <a-form-item :label="t('entity.maintenance.endtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.endtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceContent')">
      <a-form-item :label="t('entity.maintenance.content')">
        <a-textarea
          v-model:value="advancedQueryForm.maintenanceContent"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenance.content') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('faultDescription')">
      <a-form-item :label="t('entity.maintenance.faultdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.faultDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenance.faultdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('solution')">
      <a-form-item :label="t('entity.maintenance.solution')">
        <a-input
          v-model:value="advancedQueryForm.solution"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.solution') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('usedParts')">
      <a-form-item :label="t('entity.maintenance.usedparts')">
        <a-input
          v-model:value="advancedQueryForm.usedParts"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.usedparts') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCost')">
      <a-form-item :label="t('entity.maintenance.cost')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.cost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceResult')">
      <a-form-item :label="t('entity.maintenance.result')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceResult"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.result') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceStatus')">
      <a-form-item :label="t('entity.maintenance.status')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateStart')">
      <a-form-item :label="t('entity.maintenance.nextmaintenancedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.nextmaintenancedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateEnd')">
      <a-form-item :label="t('entity.maintenance.nextmaintenancedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.nextmaintenancedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCycleDays')">
      <a-form-item :label="t('entity.maintenance.cycledays')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceCycleDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.cycledays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceDocuments')">
      <a-form-item :label="t('entity.maintenance.documents')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceDocuments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.documents') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceImages')">
      <a-form-item :label="t('entity.maintenance.images')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceImages"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.images') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedSummary')">
      <a-form-item :label="t('entity.maintenance.acceptedsummary')">
        <a-input
          v-model:value="advancedQueryForm.acceptedSummary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.acceptedsummary') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedBy')">
      <a-form-item :label="t('entity.maintenance.acceptedby')">
        <a-input
          v-model:value="advancedQueryForm.acceptedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.acceptedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtStart')">
      <a-form-item :label="t('entity.maintenance.acceptedatstart')">
        <a-input
          v-model:value="advancedQueryForm.acceptedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenance.acceptedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtEnd')">
      <a-form-item :label="t('entity.maintenance.acceptedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.acceptedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenance.acceptedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.maintenance._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.maintenance._self"
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
      :id-column-key="'maintenanceId'"
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
 * Takt设备维护记录实体管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/statistics/report/logistics/maintenance
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import MaintenanceForm from './components/maintenance-form.vue'
import { getMaintenanceList, getMaintenanceById, createMaintenance, updateMaintenance, deleteMaintenanceById, deleteMaintenanceBatch, getMaintenanceTemplate, importMaintenance, exportMaintenance } from '@/api/logistics/maintenance/maintenance'
import type { Maintenance, MaintenanceQuery, MaintenanceCreate, MaintenanceUpdate } from '@/types/logistics/maintenance/maintenance'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktMaintenance')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.maintenance._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<Maintenance[]>([])
const currentPage = ref(1)
const pageSize = ref(20)
const total = ref(0)
const selectedRow = ref<Maintenance | null>(null)
const selectedRows = ref<Maintenance[]>([])
const selectedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<Maintenance>>({})
const formLoading = ref(false)
const formRef = ref()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  equipmentId: '',
  equipmentCode: '',
  lineNumber: undefined as number | undefined,
  maintenanceType: undefined as number | undefined,
  maintenanceCompany: '',
  maintenanceTechnician: '',
  maintenanceDateStart: '',
  maintenanceDateEnd: '',
  maintenanceStartTimeStart: '',
  maintenanceStartTimeEnd: '',
  maintenanceEndTimeStart: '',
  maintenanceEndTimeEnd: '',
  maintenanceContent: '',
  faultDescription: '',
  solution: '',
  usedParts: '',
  maintenanceCost: undefined as number | undefined,
  maintenanceResult: undefined as number | undefined,
  maintenanceStatus: undefined as number | undefined,
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  maintenanceCycleDays: undefined as number | undefined,
  maintenanceDocuments: '',
  maintenanceImages: '',
  acceptedSummary: '',
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'equipmentId', label: t('entity.maintenance.equipmentid') },
  { key: 'equipmentCode', label: t('entity.maintenance.equipmentcode') },
  { key: 'lineNumber', label: t('entity.maintenance.linenumber') },
  { key: 'maintenanceType', label: t('entity.maintenance.type') },
  { key: 'maintenanceCompany', label: t('entity.maintenance.company') },
  { key: 'maintenanceTechnician', label: t('entity.maintenance.technician') },
  { key: 'maintenanceDateStart', label: t('entity.maintenance.datestart') },
  { key: 'maintenanceDateEnd', label: t('entity.maintenance.dateend') },
  { key: 'maintenanceStartTimeStart', label: t('entity.maintenance.starttimestart') },
  { key: 'maintenanceStartTimeEnd', label: t('entity.maintenance.starttimeend') },
  { key: 'maintenanceEndTimeStart', label: t('entity.maintenance.endtimestart') },
  { key: 'maintenanceEndTimeEnd', label: t('entity.maintenance.endtimeend') },
  { key: 'maintenanceContent', label: t('entity.maintenance.content') },
  { key: 'faultDescription', label: t('entity.maintenance.faultdescription') },
  { key: 'solution', label: t('entity.maintenance.solution') },
  { key: 'usedParts', label: t('entity.maintenance.usedparts') },
  { key: 'maintenanceCost', label: t('entity.maintenance.cost') },
  { key: 'maintenanceResult', label: t('entity.maintenance.result') },
  { key: 'maintenanceStatus', label: t('entity.maintenance.status') },
  { key: 'nextMaintenanceDateStart', label: t('entity.maintenance.nextmaintenancedatestart') },
  { key: 'nextMaintenanceDateEnd', label: t('entity.maintenance.nextmaintenancedateend') },
  { key: 'maintenanceCycleDays', label: t('entity.maintenance.cycledays') },
  { key: 'maintenanceDocuments', label: t('entity.maintenance.documents') },
  { key: 'maintenanceImages', label: t('entity.maintenance.images') },
  { key: 'acceptedSummary', label: t('entity.maintenance.acceptedsummary') },
  { key: 'acceptedBy', label: t('entity.maintenance.acceptedby') },
  { key: 'acceptedAtStart', label: t('entity.maintenance.acceptedatstart') },
  { key: 'acceptedAtEnd', label: t('entity.maintenance.acceptedatend') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extFieldJson', label: t('common.page.entity.extfieldjson') },
  { key: 'remark', label: t('common.page.entity.remark') },
])
const visibleQueryFieldKeys = ref<string[]>([])
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'maintenanceId'
const updateDisabled = computed(() => selectedRows.value.length !== 1)
const deleteDisabled = computed(() => selectedRows.value.length === 0)

onMounted(() => {
  loadData()
})






const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'maintenanceId',
    key: 'maintenanceId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceId') ?? ''
  },
  {
    title: t('entity.maintenance.equipmentid'),
    dataIndex: 'equipmentId',
    key: 'equipmentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'equipmentId') ?? ''
  },
  {
    title: t('entity.maintenance.equipmentname'),
    dataIndex: 'equipmentName',
    key: 'equipmentName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'equipmentName') ?? ''
  },
  {
    title: t('entity.maintenance.equipmentcode'),
    dataIndex: 'equipmentCode',
    key: 'equipmentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'equipmentCode') ?? ''
  },
  {
    title: t('entity.maintenance.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.maintenance.type'),
    dataIndex: 'maintenanceType',
    key: 'maintenanceType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceType') ?? ''
  },
  {
    title: t('entity.maintenance.company'),
    dataIndex: 'maintenanceCompany',
    key: 'maintenanceCompany',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceCompany') ?? ''
  },
  {
    title: t('entity.maintenance.technician'),
    dataIndex: 'maintenanceTechnician',
    key: 'maintenanceTechnician',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceTechnician') ?? ''
  },
  {
    title: t('entity.maintenance.date'),
    dataIndex: 'maintenanceDate',
    key: 'maintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceDate') ?? ''
  },
  {
    title: t('entity.maintenance.starttime'),
    dataIndex: 'maintenanceStartTime',
    key: 'maintenanceStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceStartTime') ?? ''
  },
  {
    title: t('entity.maintenance.endtime'),
    dataIndex: 'maintenanceEndTime',
    key: 'maintenanceEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceEndTime') ?? ''
  },
  {
    title: t('entity.maintenance.content'),
    dataIndex: 'maintenanceContent',
    key: 'maintenanceContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceContent') ?? ''
  },
  {
    title: t('entity.maintenance.faultdescription'),
    dataIndex: 'faultDescription',
    key: 'faultDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'faultDescription') ?? ''
  },
  {
    title: t('entity.maintenance.solution'),
    dataIndex: 'solution',
    key: 'solution',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'solution') ?? ''
  },
  {
    title: t('entity.maintenance.usedparts'),
    dataIndex: 'usedParts',
    key: 'usedParts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'usedParts') ?? ''
  },
  {
    title: t('entity.maintenance.cost'),
    dataIndex: 'maintenanceCost',
    key: 'maintenanceCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceCost') ?? ''
  },
  {
    title: t('entity.maintenance.result'),
    dataIndex: 'maintenanceResult',
    key: 'maintenanceResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceResult') ?? ''
  },
  {
    title: t('entity.maintenance.status'),
    dataIndex: 'maintenanceStatus',
    key: 'maintenanceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceStatus') ?? ''
  },
  {
    title: t('entity.maintenance.nextmaintenancedate'),
    dataIndex: 'nextMaintenanceDate',
    key: 'nextMaintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'nextMaintenanceDate') ?? ''
  },
  {
    title: t('entity.maintenance.cycledays'),
    dataIndex: 'maintenanceCycleDays',
    key: 'maintenanceCycleDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceCycleDays') ?? ''
  },
  {
    title: t('entity.maintenance.documents'),
    dataIndex: 'maintenanceDocuments',
    key: 'maintenanceDocuments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceDocuments') ?? ''
  },
  {
    title: t('entity.maintenance.images'),
    dataIndex: 'maintenanceImages',
    key: 'maintenanceImages',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'maintenanceImages') ?? ''
  },
  {
    title: t('entity.maintenance.acceptedsummary'),
    dataIndex: 'acceptedSummary',
    key: 'acceptedSummary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'acceptedSummary') ?? ''
  },
  {
    title: t('entity.maintenance.acceptedby'),
    dataIndex: 'acceptedBy',
    key: 'acceptedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'acceptedBy') ?? ''
  },
  {
    title: t('entity.maintenance.acceptedat'),
    dataIndex: 'acceptedAt',
    key: 'acceptedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'acceptedAt') ?? ''
  },
  {
    title: t('entity.maintenance.equipment'),
    dataIndex: 'equipment',
    key: 'equipment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceField(record, 'equipment') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'statistics:report:logistics:maintenance:update',
        onClick: (record: Maintenance) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:report:logistics:maintenance:delete',
        onClick: (record: Maintenance) => handleDeleteOne(record)
      }
    ]
  })
])

const getMaintenanceId = (record: any): string => record?.[entityIdName] ?? ''
const getMaintenanceField = (record: any, field: string): any => record?.[field]

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Maintenance[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Maintenance, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getMaintenanceId(selectedRow.value) === getMaintenanceId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Maintenance[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: Maintenance) => ({
  onClick: () => {
    const key = getMaintenanceId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getMaintenanceId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

async function loadData() {
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: MaintenanceQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getMaintenanceList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Maintenance] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function handleSearch() {
  currentPage.value = 1
  loadData()
}

function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  equipmentId: '',
  equipmentCode: '',
  lineNumber: undefined as number | undefined,
  maintenanceType: undefined as number | undefined,
  maintenanceCompany: '',
  maintenanceTechnician: '',
  maintenanceDateStart: '',
  maintenanceDateEnd: '',
  maintenanceStartTimeStart: '',
  maintenanceStartTimeEnd: '',
  maintenanceEndTimeStart: '',
  maintenanceEndTimeEnd: '',
  maintenanceContent: '',
  faultDescription: '',
  solution: '',
  usedParts: '',
  maintenanceCost: undefined as number | undefined,
  maintenanceResult: undefined as number | undefined,
  maintenanceStatus: undefined as number | undefined,
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  maintenanceCycleDays: undefined as number | undefined,
  maintenanceDocuments: '',
  maintenanceImages: '',
  acceptedSummary: '',
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.maintenance._self') })
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: Maintenance) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.maintenance._self') })
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.maintenance._self') }))
  }
}
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
      await updateMaintenance(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.maintenance._self') }))
    } else {
      await createMaintenance(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.maintenance._self') }))
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}
function handleImport() {
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getMaintenanceTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaintenance(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  try {
    loading.value = true
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: MaintenanceQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportMaintenance(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.maintenance._self') }))
  } catch (error: any) {
    logger.error('[Maintenance] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.maintenance._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: Maintenance) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.maintenance._self'), name: t('common.tip.this.target', { target: t('entity.maintenance._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaintenanceById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.maintenance._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.maintenance._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.maintenance._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMaintenanceBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.maintenance._self') }))
      loadData()
    }
  })
}
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  equipmentId: '',
  equipmentCode: '',
  lineNumber: undefined as number | undefined,
  maintenanceType: undefined as number | undefined,
  maintenanceCompany: '',
  maintenanceTechnician: '',
  maintenanceDateStart: '',
  maintenanceDateEnd: '',
  maintenanceStartTimeStart: '',
  maintenanceStartTimeEnd: '',
  maintenanceEndTimeStart: '',
  maintenanceEndTimeEnd: '',
  maintenanceContent: '',
  faultDescription: '',
  solution: '',
  usedParts: '',
  maintenanceCost: undefined as number | undefined,
  maintenanceResult: undefined as number | undefined,
  maintenanceStatus: undefined as number | undefined,
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  maintenanceCycleDays: undefined as number | undefined,
  maintenanceDocuments: '',
  maintenanceImages: '',
  acceptedSummary: '',
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
  }
}

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

function handleRefresh() {
  loadData()
}

function handleTableChange() {}
function handleResizeColumn() {}
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

<style scoped lang="css">
.statistics-report-logistics-maintenance {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
