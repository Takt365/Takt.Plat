<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/planning/master-production-schedule -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：主生产计划 MPS 头表管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:manufacturing:planning:master:production:schedule:create"
      update-permission="logistics:manufacturing:planning:master:production:schedule:update"
      delete-permission="logistics:manufacturing:planning:master:production:schedule:delete"
      import-permission="logistics:manufacturing:planning:master:production:schedule:import"
      export-permission="logistics:manufacturing:planning:master:production:schedule:export"
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
      :master-row-key="getMasterProductionScheduleId"
      :master-row-selection="rowSelection"
      master-id-column-key="masterProductionScheduleId"
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
        <template v-if="column.key === 'scheduleStatus'">
          <a-switch
            :checked="getMasterProductionScheduleField(record, 'scheduleStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleScheduleStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'bucketType'">
          <TaktDictTag
            :value="getMasterProductionScheduleField(record, 'bucketType')"
            dict-type="mps_time_bucket_type"
          />
        </template>
      </template>
      <template #detail>
        <MasterProductionScheduleLinePanel
          ref="masterProductionScheduleLinePanelRef"
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
      <MasterProductionScheduleForm
        :key="formData?.masterProductionScheduleId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-planning-master-production-schedule'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.masterproductionschedule.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.plantcode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mpsCode')">
      <a-form-item :label="t('entity.masterproductionschedule.mpscode')">
        <a-input
          v-model:value="advancedQueryForm.mpsCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.mpscode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('masterDemandScheduleId')">
      <a-form-item :label="t('entity.masterproductionschedule.masterdemandscheduleid')">
        <a-input
          v-model:value="advancedQueryForm.masterDemandScheduleId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.masterdemandscheduleid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mdsCode')">
      <a-form-item :label="t('entity.masterproductionschedule.mdscode')">
        <a-input
          v-model:value="advancedQueryForm.mdsCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.mdscode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planPeriodStartStart')">
      <a-form-item :label="t('entity.masterproductionschedule.planperiodstartstart')">
        <a-input
          v-model:value="advancedQueryForm.planPeriodStartStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.planperiodstartstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planPeriodStartEnd')">
      <a-form-item :label="t('entity.masterproductionschedule.planperiodstartend')">
        <a-input
          v-model:value="advancedQueryForm.planPeriodStartEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.planperiodstartend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planPeriodEndStart')">
      <a-form-item :label="t('entity.masterproductionschedule.planperiodendstart')">
        <a-input
          v-model:value="advancedQueryForm.planPeriodEndStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.planperiodendstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planPeriodEndEnd')">
      <a-form-item :label="t('entity.masterproductionschedule.planperiodendend')">
        <a-input
          v-model:value="advancedQueryForm.planPeriodEndEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.planperiodendend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bucketType')">
      <a-form-item :label="t('entity.masterproductionschedule.buckettype')">
        <TaktSelect
          v-model:value="advancedQueryForm.bucketType"
          dict-type="mps_time_bucket_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterproductionschedule.buckettype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleStatus')">
      <a-form-item :label="t('entity.masterproductionschedule.schedulestatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.scheduleStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterproductionschedule.schedulestatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.masterproductionschedule.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterproductionschedule.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.masterproductionschedule.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.masterproductionschedule.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.masterproductionschedule.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterproductionschedule.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.masterproductionschedule.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.masterproductionschedule.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.masterproductionschedule.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterproductionschedule.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.masterproductionschedule.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterproductionschedule.flowinstanceid') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.masterproductionschedule._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.masterproductionschedule._self"
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
      :id-column-key="'masterProductionScheduleId'"
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
 * 主生产计划 MPS 头表管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/planning/master-production-schedule
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import MasterProductionScheduleForm from './components/master-production-schedule-form.vue'
import MasterProductionScheduleLinePanel from './components/master-production-schedule-line-panel.vue'
import { provideMasterProductionScheduleMasterContext } from './composables/use-master-production-schedule-master-context'
import { getMasterProductionScheduleList, getMasterProductionScheduleById, createMasterProductionSchedule, updateMasterProductionSchedule, deleteMasterProductionScheduleById, deleteMasterProductionScheduleBatch, getMasterProductionScheduleTemplate, importMasterProductionSchedule, exportMasterProductionSchedule, updateMasterProductionScheduleStatus } from '@/api/logistics/manufacturing/planning/master-production-schedule'
import type { MasterProductionSchedule, MasterProductionScheduleQuery } from '@/types/logistics/manufacturing/planning/master-production-schedule'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMasterProductionSchedule')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.masterproductionschedule._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<MasterProductionSchedule[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<MasterProductionSchedule | null>(null)
/** 表格多选行 */
const selectedRows = ref<MasterProductionSchedule[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<MasterProductionSchedule> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  mpsCode: '',
  masterDemandScheduleId: '',
  mdsCode: '',
  planPeriodStartStart: '',
  planPeriodStartEnd: '',
  planPeriodEndStart: '',
  planPeriodEndEnd: '',
  bucketType: undefined as number | undefined,
  scheduleStatus: undefined as number | undefined,
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
  { key: 'plantCode', label: t('entity.masterproductionschedule.plantcode') },
  { key: 'mpsCode', label: t('entity.masterproductionschedule.mpscode') },
  { key: 'masterDemandScheduleId', label: t('entity.masterproductionschedule.masterdemandscheduleid') },
  { key: 'mdsCode', label: t('entity.masterproductionschedule.mdscode') },
  { key: 'planPeriodStartStart', label: t('entity.masterproductionschedule.planperiodstartstart') },
  { key: 'planPeriodStartEnd', label: t('entity.masterproductionschedule.planperiodstartend') },
  { key: 'planPeriodEndStart', label: t('entity.masterproductionschedule.planperiodendstart') },
  { key: 'planPeriodEndEnd', label: t('entity.masterproductionschedule.planperiodendend') },
  { key: 'bucketType', label: t('entity.masterproductionschedule.buckettype') },
  { key: 'scheduleStatus', label: t('entity.masterproductionschedule.schedulestatus') },
  { key: 'approvalStatus', label: t('entity.masterproductionschedule.approvalstatus') },
  { key: 'initiatorId', label: t('entity.masterproductionschedule.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.masterproductionschedule.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.masterproductionschedule.initiatedatend') },
  { key: 'approvedBy', label: t('entity.masterproductionschedule.approvedby') },
  { key: 'approvedAtStart', label: t('entity.masterproductionschedule.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.masterproductionschedule.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.masterproductionschedule.flowinstanceid') },
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
const entityIdName = 'masterProductionScheduleId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideMasterProductionScheduleMasterContext()
const masterProductionScheduleLinePanelRef = ref<InstanceType<typeof MasterProductionScheduleLinePanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MasterProductionScheduleQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MasterProductionScheduleQuery>): MasterProductionScheduleQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MasterProductionScheduleQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MasterProductionScheduleQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('mpsCode', form.mpsCode)
  assignTrimmed('masterDemandScheduleId', form.masterDemandScheduleId)
  assignTrimmed('mdsCode', form.mdsCode)
  assignTrimmed('planPeriodStartStart', form.planPeriodStartStart)
  assignTrimmed('planPeriodStartEnd', form.planPeriodStartEnd)
  assignTrimmed('planPeriodEndStart', form.planPeriodEndStart)
  assignTrimmed('planPeriodEndEnd', form.planPeriodEndEnd)
  if (form.bucketType !== undefined && form.bucketType !== null) {
    query.bucketType = form.bucketType
  }
  if (form.scheduleStatus !== undefined && form.scheduleStatus !== null) {
    query.scheduleStatus = form.scheduleStatus
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
function syncMasterSelection(record: MasterProductionSchedule | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getMasterProductionScheduleId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as MasterProductionSchedule
  const key = getMasterProductionScheduleId(row)
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
async function loadMasterProductionScheduleDetail(record: MasterProductionSchedule): Promise<MasterProductionSchedule | null> {
  const id = getMasterProductionScheduleId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getMasterProductionScheduleById(id)
    const index = dataSource.value.findIndex((row) => getMasterProductionScheduleId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as MasterProductionSchedule
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
    dataIndex: 'masterProductionScheduleId',
    key: 'masterProductionScheduleId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMasterProductionScheduleField(record, 'masterProductionScheduleId') ?? ''
  },
  {
    title: t('entity.masterproductionschedule.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMasterProductionScheduleField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.masterproductionschedule.mpscode'),
    dataIndex: 'mpsCode',
    key: 'mpsCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMasterProductionScheduleField(record, 'mpsCode') ?? ''
  },
  {
    title: t('entity.masterproductionschedule.masterdemandscheduleid'),
    dataIndex: 'masterDemandScheduleId',
    key: 'masterDemandScheduleId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMasterProductionScheduleField(record, 'masterDemandScheduleId') ?? ''
  },
  {
    title: t('entity.masterproductionschedule.mdscode'),
    dataIndex: 'mdsCode',
    key: 'mdsCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMasterProductionScheduleField(record, 'mdsCode') ?? ''
  },
  {
    title: t('entity.masterproductionschedule.planperiodstart'),
    dataIndex: 'planPeriodStart',
    key: 'planPeriodStart',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMasterProductionScheduleField(record, 'planPeriodStart') ?? ''
  },
  {
    title: t('entity.masterproductionschedule.planperiodend'),
    dataIndex: 'planPeriodEnd',
    key: 'planPeriodEnd',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMasterProductionScheduleField(record, 'planPeriodEnd') ?? ''
  },
  {
    title: t('entity.masterproductionschedule.buckettype'),
    dataIndex: 'bucketType',
    key: 'bucketType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.masterproductionschedule.schedulestatus'),
    dataIndex: 'scheduleStatus',
    key: 'scheduleStatus',
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
        permission: 'logistics:manufacturing:planning:master:production:schedule:update',
        onClick: (record: MasterProductionSchedule) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:planning:master:production:schedule:delete',
        onClick: (record: MasterProductionSchedule) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMasterProductionScheduleId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMasterProductionScheduleField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MasterProductionSchedule[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: MasterProductionSchedule, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getMasterProductionScheduleId(selectedRow.value) === getMasterProductionScheduleId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MasterProductionSchedule[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getMasterProductionScheduleList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[MasterProductionSchedule] 加载数据失败', { error })
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
  plantCode: '',
  mpsCode: '',
  masterDemandScheduleId: '',
  mdsCode: '',
  planPeriodStartStart: '',
  planPeriodStartEnd: '',
  planPeriodEndStart: '',
  planPeriodEndEnd: '',
  bucketType: undefined as number | undefined,
  scheduleStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.masterproductionschedule._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: MasterProductionSchedule) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.masterproductionschedule._self') })
  formLoading.value = true
  try {
    const detail = await loadMasterProductionScheduleDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.masterproductionschedule._self') }))
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
      await updateMasterProductionSchedule(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.masterproductionschedule._self') }))
    } else {
      await createMasterProductionSchedule(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.masterproductionschedule._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  masterProductionScheduleLinePanelRef.value?.reload?.()
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
  const res = await getMasterProductionScheduleTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMasterProductionSchedule(file, sheetName)
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
    const exportMeta = await exportMasterProductionSchedule(
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
    message.success(t('common.feedback.export.success', { target: t('entity.masterproductionschedule._self') }))
  } catch (error: any) {
    logger.error('[MasterProductionSchedule] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.masterproductionschedule._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MasterProductionSchedule) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.masterproductionschedule._self'), name: t('common.tip.this.target', { target: t('entity.masterproductionschedule._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMasterProductionScheduleById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.masterproductionschedule._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.masterproductionschedule._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.masterproductionschedule._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMasterProductionScheduleBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.masterproductionschedule._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleScheduleStatusChange(record: MasterProductionSchedule, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getMasterProductionScheduleField(record, 'scheduleStatus')
  const id = getMasterProductionScheduleId(record)
  const row = dataSource.value.find((item) => getMasterProductionScheduleId(item) === id)
  if (row) {
    row.scheduleStatus = newVal
  }
  try {
    await updateMasterProductionScheduleStatus({ masterProductionScheduleId: id, scheduleStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.scheduleStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
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
  plantCode: '',
  mpsCode: '',
  masterDemandScheduleId: '',
  mdsCode: '',
  planPeriodStartStart: '',
  planPeriodStartEnd: '',
  planPeriodEndStart: '',
  planPeriodEndEnd: '',
  bucketType: undefined as number | undefined,
  scheduleStatus: undefined as number | undefined,
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
