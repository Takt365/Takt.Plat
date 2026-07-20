<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/aps/aps-schedule -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：APS排程主表管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getApsScheduleId"
      :master-row-selection="rowSelection"
      master-id-column-key="apsScheduleId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="queryKeyword"
          :placeholder="searchPlaceholder"
          :loading="loading"
          @search="handleSearch"
          @reset="handleReset"
        />
        <TaktToolsBar
      create-permission="logistics:manufacturing:aps:schedule:create"
      update-permission="logistics:manufacturing:aps:schedule:update"
      delete-permission="logistics:manufacturing:aps:schedule:delete"
      import-permission="logistics:manufacturing:aps:schedule:import"
      export-permission="logistics:manufacturing:aps:schedule:export"
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
      </template>
      <template #detail>
        <ApsScheduleItemPanel
          ref="apsScheduleItemPanelRef"
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
      <ApsScheduleForm
        :key="formData?.apsScheduleId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-aps-aps-schedule'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('materialRequirementsPlanningId')">
      <a-form-item :label="pi.queryLabel('materialRequirementsPlanningId')">
        <a-input
          v-model:value="advancedQueryForm.materialRequirementsPlanningId"
          :placeholder="pi.queryPh('materialRequirementsPlanningId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialRequirementsPlanningCode')">
      <a-form-item :label="pi.queryLabel('materialRequirementsPlanningCode')">
        <a-input
          v-model:value="advancedQueryForm.materialRequirementsPlanningCode"
          :placeholder="pi.queryPh('materialRequirementsPlanningCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="pi.queryLabel('plantCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.plantCode"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('plantCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleCode')">
      <a-form-item :label="pi.queryLabel('scheduleCode')">
        <a-input
          v-model:value="advancedQueryForm.scheduleCode"
          :placeholder="pi.queryPh('scheduleCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleName')">
      <a-form-item :label="pi.queryLabel('scheduleName')">
        <a-input
          v-model:value="advancedQueryForm.scheduleName"
          :placeholder="pi.queryPh('scheduleName', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleType')">
      <a-form-item :label="pi.queryLabel('scheduleType')">
        <a-input-number
          v-model:value="advancedQueryForm.scheduleType"
          :placeholder="pi.queryPh('scheduleType', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planDateStart')">
      <a-form-item :label="pi.queryLabel('planDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.planDateStart"
          :placeholder="pi.queryPh('planDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planDateEnd')">
      <a-form-item :label="pi.queryLabel('planDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.planDateEnd"
          :placeholder="pi.queryPh('planDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planStartTimeStart')">
      <a-form-item :label="pi.queryLabel('planStartTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.planStartTimeStart"
          :placeholder="pi.queryPh('planStartTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planStartTimeEnd')">
      <a-form-item :label="pi.queryLabel('planStartTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.planStartTimeEnd"
          :placeholder="pi.queryPh('planStartTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planEndTimeStart')">
      <a-form-item :label="pi.queryLabel('planEndTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.planEndTimeStart"
          :placeholder="pi.queryPh('planEndTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planEndTimeEnd')">
      <a-form-item :label="pi.queryLabel('planEndTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.planEndTimeEnd"
          :placeholder="pi.queryPh('planEndTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planCycle')">
      <a-form-item :label="pi.queryLabel('planCycle')">
        <a-input-number
          v-model:value="advancedQueryForm.planCycle"
          :placeholder="pi.queryPh('planCycle', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workshopCode')">
      <a-form-item :label="pi.queryLabel('workshopCode')">
        <a-input
          v-model:value="advancedQueryForm.workshopCode"
          :placeholder="pi.queryPh('workshopCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workshopName')">
      <a-form-item :label="pi.queryLabel('workshopName')">
        <a-input
          v-model:value="advancedQueryForm.workshopName"
          :placeholder="pi.queryPh('workshopName', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLineCode')">
      <a-form-item :label="pi.queryLabel('productionLineCode')">
        <a-input
          v-model:value="advancedQueryForm.productionLineCode"
          :placeholder="pi.queryPh('productionLineCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLineName')">
      <a-form-item :label="pi.queryLabel('productionLineName')">
        <a-input
          v-model:value="advancedQueryForm.productionLineName"
          :placeholder="pi.queryPh('productionLineName', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleStrategy')">
      <a-form-item :label="pi.queryLabel('scheduleStrategy')">
        <a-input-number
          v-model:value="advancedQueryForm.scheduleStrategy"
          :placeholder="pi.queryPh('scheduleStrategy', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleAlgorithm')">
      <a-form-item :label="pi.queryLabel('scheduleAlgorithm')">
        <a-input-number
          v-model:value="advancedQueryForm.scheduleAlgorithm"
          :placeholder="pi.queryPh('scheduleAlgorithm', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('optimizationObjective')">
      <a-form-item :label="pi.queryLabel('optimizationObjective')">
        <a-input-number
          v-model:value="advancedQueryForm.optimizationObjective"
          :placeholder="pi.queryPh('optimizationObjective', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleStatus')">
      <a-form-item :label="pi.queryLabel('scheduleStatus')">
        <a-input-number
          v-model:value="advancedQueryForm.scheduleStatus"
          :placeholder="pi.queryPh('scheduleStatus', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannerId')">
      <a-form-item :label="pi.queryLabel('plannerId')">
        <TaktSelect
          v-model:value="advancedQueryForm.plannerId"
          api-url="TaktEmployees/options"
          :placeholder="pi.queryPh('plannerId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannerName')">
      <a-form-item :label="pi.queryLabel('plannerName')">
        <a-input
          v-model:value="advancedQueryForm.plannerName"
          :placeholder="pi.queryPh('plannerName', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishTimeStart')">
      <a-form-item :label="pi.queryLabel('publishTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishTimeStart"
          :placeholder="pi.queryPh('publishTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishTimeEnd')">
      <a-form-item :label="pi.queryLabel('publishTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishTimeEnd"
          :placeholder="pi.queryPh('publishTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishUserId')">
      <a-form-item :label="pi.queryLabel('publishUserId')">
        <TaktSelect
          v-model:value="advancedQueryForm.publishUserId"
          api-url="TaktEmployees/options"
          :placeholder="pi.queryPh('publishUserId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishUserName')">
      <a-form-item :label="pi.queryLabel('publishUserName')">
        <a-input
          v-model:value="advancedQueryForm.publishUserName"
          :placeholder="pi.queryPh('publishUserName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleDescription')">
      <a-form-item :label="pi.queryLabel('scheduleDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.scheduleDescription"
          :placeholder="pi.queryPh('scheduleDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
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
            <span>{{ pi.queryLabel('extField') }}</span>
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
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
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
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="APSSCHEDULE_SELF_I18N_KEY"
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
      :id-column-key="'apsScheduleId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * APS排程主表管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/aps/aps-schedule
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import ApsScheduleForm from './components/schedule-form.vue'
import ApsScheduleItemPanel from './components/schedule-item-panel.vue'
import { provideApsScheduleMasterContext, type ApsScheduleRowRecord } from './composables/use-schedule-master-context'
import { getApsScheduleList, getApsScheduleById, createApsSchedule, updateApsSchedule, deleteApsScheduleById, deleteApsScheduleBatch, getApsScheduleTemplate, importApsSchedule, exportApsSchedule, updateApsScheduleStatus } from '@/api/logistics/manufacturing/aps/schedule'
import type { ApsSchedule, ApsScheduleQuery } from '@/types/logistics/manufacturing/aps/schedule'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useApsScheduleI18n,
  APSSCHEDULE_LIST_FIELDS,
  APSSCHEDULE_QUERY_STRING_FIELDS,
  APSSCHEDULE_QUERY_FIELDS,
  APSSCHEDULE_SELF_I18N_KEY,
} from './composables/use-schedule-i18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useApsScheduleI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktApsSchedule')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ApsSchedule[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<ApsScheduleRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<ApsScheduleRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<ApsSchedule> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(APSSCHEDULE_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof APSSCHEDULE_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    scheduleType: undefined as number | undefined,
    planCycle: undefined as number | undefined,
    scheduleStrategy: undefined as number | undefined,
    scheduleAlgorithm: undefined as number | undefined,
    optimizationObjective: undefined as number | undefined,
    scheduleStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  APSSCHEDULE_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'apsScheduleId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideApsScheduleMasterContext()
const apsScheduleItemPanelRef = ref<InstanceType<typeof ApsScheduleItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {ApsScheduleQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ApsScheduleQuery>): ApsScheduleQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ApsScheduleQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ApsScheduleQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of APSSCHEDULE_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.scheduleType !== undefined && form.scheduleType !== null) {
    query.scheduleType = form.scheduleType
  }
  if (form.planCycle !== undefined && form.planCycle !== null) {
    query.planCycle = form.planCycle
  }
  if (form.scheduleStrategy !== undefined && form.scheduleStrategy !== null) {
    query.scheduleStrategy = form.scheduleStrategy
  }
  if (form.scheduleAlgorithm !== undefined && form.scheduleAlgorithm !== null) {
    query.scheduleAlgorithm = form.scheduleAlgorithm
  }
  if (form.optimizationObjective !== undefined && form.optimizationObjective !== null) {
    query.optimizationObjective = form.optimizationObjective
  }
  if (form.scheduleStatus !== undefined && form.scheduleStatus !== null) {
    query.scheduleStatus = form.scheduleStatus
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: ApsScheduleRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getApsScheduleId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as ApsScheduleRowRecord
  const key = getApsScheduleId(row)
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
async function loadApsScheduleDetail(record: ApsScheduleRowRecord): Promise<ApsSchedule | null> {
  const id = getApsScheduleId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getApsScheduleById(id)
    const index = dataSource.value.findIndex((row) => getApsScheduleId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as ApsSchedule
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
    dataIndex: 'apsScheduleId',
    key: 'apsScheduleId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'apsScheduleId') ?? ''
  },
  {
    title: pi.label('materialRequirementsPlanningId'),
    dataIndex: 'materialRequirementsPlanningId',
    key: 'materialRequirementsPlanningId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'materialRequirementsPlanningId') ?? ''
  },
  {
    title: pi.label('materialRequirementsPlanningCode'),
    dataIndex: 'materialRequirementsPlanningCode',
    key: 'materialRequirementsPlanningCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'materialRequirementsPlanningCode') ?? ''
  },
  {
    title: pi.label('plantCode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'plantCode') ?? ''
  },
  {
    title: pi.label('scheduleCode'),
    dataIndex: 'scheduleCode',
    key: 'scheduleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleCode') ?? ''
  },
  {
    title: pi.label('scheduleName'),
    dataIndex: 'scheduleName',
    key: 'scheduleName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleName') ?? ''
  },
  {
    title: pi.label('scheduleType'),
    dataIndex: 'scheduleType',
    key: 'scheduleType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleType') ?? ''
  },
  {
    title: pi.label('planDate'),
    dataIndex: 'planDate',
    key: 'planDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'planDate') ?? ''
  },
  {
    title: pi.label('planStartTime'),
    dataIndex: 'planStartTime',
    key: 'planStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'planStartTime') ?? ''
  },
  {
    title: pi.label('planEndTime'),
    dataIndex: 'planEndTime',
    key: 'planEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'planEndTime') ?? ''
  },
  {
    title: pi.label('planCycle'),
    dataIndex: 'planCycle',
    key: 'planCycle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'planCycle') ?? ''
  },
  {
    title: pi.label('workshopCode'),
    dataIndex: 'workshopCode',
    key: 'workshopCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'workshopCode') ?? ''
  },
  {
    title: pi.label('workshopName'),
    dataIndex: 'workshopName',
    key: 'workshopName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'workshopName') ?? ''
  },
  {
    title: pi.label('productionLineCode'),
    dataIndex: 'productionLineCode',
    key: 'productionLineCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'productionLineCode') ?? ''
  },
  {
    title: pi.label('productionLineName'),
    dataIndex: 'productionLineName',
    key: 'productionLineName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'productionLineName') ?? ''
  },
  {
    title: pi.label('scheduleStrategy'),
    dataIndex: 'scheduleStrategy',
    key: 'scheduleStrategy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleStrategy') ?? ''
  },
  {
    title: pi.label('scheduleAlgorithm'),
    dataIndex: 'scheduleAlgorithm',
    key: 'scheduleAlgorithm',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleAlgorithm') ?? ''
  },
  {
    title: pi.label('optimizationObjective'),
    dataIndex: 'optimizationObjective',
    key: 'optimizationObjective',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'optimizationObjective') ?? ''
  },
  {
    title: pi.label('scheduleStatus'),
    dataIndex: 'scheduleStatus',
    key: 'scheduleStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleStatus') ?? ''
  },
  {
    title: pi.label('plannerId'),
    dataIndex: 'plannerId',
    key: 'plannerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'plannerId') ?? ''
  },
  {
    title: pi.label('plannerName'),
    dataIndex: 'plannerName',
    key: 'plannerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'plannerName') ?? ''
  },
  {
    title: pi.label('publishTime'),
    dataIndex: 'publishTime',
    key: 'publishTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'publishTime') ?? ''
  },
  {
    title: pi.label('publishUserId'),
    dataIndex: 'publishUserId',
    key: 'publishUserId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'publishUserId') ?? ''
  },
  {
    title: pi.label('publishUserName'),
    dataIndex: 'publishUserName',
    key: 'publishUserName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'publishUserName') ?? ''
  },
  {
    title: pi.label('scheduleDescription'),
    dataIndex: 'scheduleDescription',
    key: 'scheduleDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleDescription') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:aps:schedule:update',
        onClick: (record: ApsScheduleRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:aps:schedule:delete',
        onClick: (record: ApsScheduleRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getApsScheduleId = (record: ApsScheduleRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getApsScheduleField = (record: any, field: string): any => record?.[field]



/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ApsScheduleRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: ApsScheduleRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getApsScheduleId(selectedRow.value) === getApsScheduleId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ApsScheduleRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getApsScheduleList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[ApsSchedule] 加载数据失败', { error })
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
  materialRequirementsPlanningId: '',
  materialRequirementsPlanningCode: '',
  plantCode: '',
  scheduleCode: '',
  scheduleName: '',
  scheduleType: undefined as number | undefined,
  planDateStart: '',
  planDateEnd: '',
  planStartTimeStart: '',
  planStartTimeEnd: '',
  planEndTimeStart: '',
  planEndTimeEnd: '',
  planCycle: undefined as number | undefined,
  workshopCode: '',
  workshopName: '',
  productionLineCode: '',
  productionLineName: '',
  scheduleStrategy: undefined as number | undefined,
  scheduleAlgorithm: undefined as number | undefined,
  optimizationObjective: undefined as number | undefined,
  scheduleStatus: undefined as number | undefined,
  plannerId: '',
  plannerName: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  publishUserId: '',
  publishUserName: '',
  scheduleDescription: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: ApsScheduleRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadApsScheduleDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
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
      await updateApsSchedule(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createApsSchedule(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  apsScheduleItemPanelRef.value?.reload?.()
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
  const res = await getApsScheduleTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importApsSchedule(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    apsScheduleItemPanelRef.value?.reload?.()
      }
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportApsSchedule(
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: any) {
    logger.error('[ApsSchedule] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ApsScheduleRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteApsScheduleById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteApsScheduleBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
  materialRequirementsPlanningId: '',
  materialRequirementsPlanningCode: '',
  plantCode: '',
  scheduleCode: '',
  scheduleName: '',
  scheduleType: undefined as number | undefined,
  planDateStart: '',
  planDateEnd: '',
  planStartTimeStart: '',
  planStartTimeEnd: '',
  planEndTimeStart: '',
  planEndTimeEnd: '',
  planCycle: undefined as number | undefined,
  workshopCode: '',
  workshopName: '',
  productionLineCode: '',
  productionLineName: '',
  scheduleStrategy: undefined as number | undefined,
  scheduleAlgorithm: undefined as number | undefined,
  optimizationObjective: undefined as number | undefined,
  scheduleStatus: undefined as number | undefined,
  plannerId: '',
  plannerName: '',
  publishTimeStart: '',
  publishTimeEnd: '',
  publishUserId: '',
  publishUserName: '',
  scheduleDescription: '',
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
