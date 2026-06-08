<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/scheduling/aps-schedule -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：APS排程主表管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-scheduling-aps-schedule">
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
      create-permission="logistics:manufacturing:scheduling:apsschedule:create"
      update-permission="logistics:manufacturing:scheduling:apsschedule:update"
      delete-permission="logistics:manufacturing:scheduling:apsschedule:delete"
      import-permission="logistics:manufacturing:scheduling:apsschedule:import"
      export-permission="logistics:manufacturing:scheduling:apsschedule:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
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
      :id-column-key="'apsScheduleId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getApsScheduleId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.apsScheduleItem._self') }}</div>
          <a-table
            v-if="hasApsScheduleItemRows(record)"
            :columns="apsScheduleItemExpandColumns"
            :data-source="getApsScheduleItemRows(record)"
            :row-key="(row: ApsScheduleItem, index?: number) => row?.apsScheduleItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.apsScheduleChangeLog._self') }}</div>
          <a-table
            v-if="hasApsScheduleChangeLogRows(record)"
            :columns="apsScheduleChangeLogExpandColumns"
            :data-source="getApsScheduleChangeLogRows(record)"
            :row-key="(row: ApsScheduleChangeLog, index?: number) => row?.apsScheduleChangeLogId || String(index ?? 0)"
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
      <ApsScheduleForm
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
      :storage-key="'takt-query-fields-logistics-manufacturing-scheduling-aps-schedule'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.apsSchedule.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleCode')">
      <a-form-item :label="t('entity.apsSchedule.schedulecode')">
        <a-input
          v-model:value="advancedQueryForm.scheduleCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulecode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleName')">
      <a-form-item :label="t('entity.apsSchedule.schedulename')">
        <a-input
          v-model:value="advancedQueryForm.scheduleName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleType')">
      <a-form-item :label="t('entity.apsSchedule.scheduletype')">
        <a-input-number
          v-model:value="advancedQueryForm.scheduleType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.scheduletype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planDateStart')">
      <a-form-item :label="t('entity.apsSchedule.plandatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.planDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.plandatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planDateEnd')">
      <a-form-item :label="t('entity.apsSchedule.plandateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.planDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.plandateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planStartTimeStart')">
      <a-form-item :label="t('entity.apsSchedule.planstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.planStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.planstarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planStartTimeEnd')">
      <a-form-item :label="t('entity.apsSchedule.planstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.planStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.planstarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planEndTimeStart')">
      <a-form-item :label="t('entity.apsSchedule.planendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.planEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.planendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planEndTimeEnd')">
      <a-form-item :label="t('entity.apsSchedule.planendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.planEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.planendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planCycle')">
      <a-form-item :label="t('entity.apsSchedule.plancycle')">
        <a-input-number
          v-model:value="advancedQueryForm.planCycle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plancycle') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workshopCode')">
      <a-form-item :label="t('entity.apsSchedule.workshopcode')">
        <a-input
          v-model:value="advancedQueryForm.workshopCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.workshopcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workshopName')">
      <a-form-item :label="t('entity.apsSchedule.workshopname')">
        <a-input
          v-model:value="advancedQueryForm.workshopName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.workshopname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLineCode')">
      <a-form-item :label="t('entity.apsSchedule.productionlinecode')">
        <a-input
          v-model:value="advancedQueryForm.productionLineCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.productionlinecode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLineName')">
      <a-form-item :label="t('entity.apsSchedule.productionlinename')">
        <a-input
          v-model:value="advancedQueryForm.productionLineName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.productionlinename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleStrategy')">
      <a-form-item :label="t('entity.apsSchedule.schedulestrategy')">
        <a-input-number
          v-model:value="advancedQueryForm.scheduleStrategy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulestrategy') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleAlgorithm')">
      <a-form-item :label="t('entity.apsSchedule.schedulealgorithm')">
        <a-input-number
          v-model:value="advancedQueryForm.scheduleAlgorithm"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulealgorithm') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('optimizationObjective')">
      <a-form-item :label="t('entity.apsSchedule.optimizationobjective')">
        <a-input-number
          v-model:value="advancedQueryForm.optimizationObjective"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.optimizationobjective') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleStatus')">
      <a-form-item :label="t('entity.apsSchedule.schedulestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.scheduleStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannerId')">
      <a-form-item :label="t('entity.apsSchedule.plannerid')">
        <a-input
          v-model:value="advancedQueryForm.plannerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plannerid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannerName')">
      <a-form-item :label="t('entity.apsSchedule.plannername')">
        <a-input
          v-model:value="advancedQueryForm.plannerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plannername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishTimeStart')">
      <a-form-item :label="t('entity.apsSchedule.publishtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.publishtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishTimeEnd')">
      <a-form-item :label="t('entity.apsSchedule.publishtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.publishtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishUserId')">
      <a-form-item :label="t('entity.apsSchedule.publishuserid')">
        <a-input
          v-model:value="advancedQueryForm.publishUserId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.publishuserid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishUserName')">
      <a-form-item :label="t('entity.apsSchedule.publishusername')">
        <a-input
          v-model:value="advancedQueryForm.publishUserName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.publishusername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduleDescription')">
      <a-form-item :label="t('entity.apsSchedule.scheduledescription')">
        <a-textarea
          v-model:value="advancedQueryForm.scheduleDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.apsSchedule.scheduledescription') })"
          :rows="2"
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
      :title="t('common.dialog.title.import', { entity: t('entity.apsSchedule._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.apsSchedule._self"
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
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * APS排程主表管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/scheduling/aps-schedule
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import ApsScheduleForm from './components/aps-schedule-form.vue'
import { getApsScheduleList, getApsScheduleById, createApsSchedule, updateApsSchedule, deleteApsScheduleById, deleteApsScheduleBatch, getApsScheduleTemplate, importApsSchedule, exportApsSchedule } from '@/api/logistics/manufacturing/scheduling/aps-schedule'
import * as apsScheduleItemApi from '@/api/logistics/manufacturing/scheduling/aps-schedule-item'
import * as apsScheduleChangeLogApi from '@/api/logistics/manufacturing/scheduling/aps-schedule-change-log'
import type { ApsScheduleItem, ApsScheduleItemQuery } from '@/types/logistics/manufacturing/scheduling/aps-schedule-item'
import type { ApsScheduleChangeLog, ApsScheduleChangeLogQuery } from '@/types/logistics/manufacturing/scheduling/aps-schedule-change-log'
import type { ApsSchedule, ApsScheduleQuery, ApsScheduleCreate, ApsScheduleUpdate } from '@/types/logistics/manufacturing/scheduling/aps-schedule'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktApsSchedule')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.apsSchedule._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ApsSchedule[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<ApsSchedule | null>(null)
/** 表格多选行 */
const selectedRows = ref<ApsSchedule[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<ApsSchedule>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
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
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.apsSchedule.plantcode') },
  { key: 'scheduleCode', label: t('entity.apsSchedule.schedulecode') },
  { key: 'scheduleName', label: t('entity.apsSchedule.schedulename') },
  { key: 'scheduleType', label: t('entity.apsSchedule.scheduletype') },
  { key: 'planDateStart', label: t('entity.apsSchedule.plandatestart') },
  { key: 'planDateEnd', label: t('entity.apsSchedule.plandateend') },
  { key: 'planStartTimeStart', label: t('entity.apsSchedule.planstarttimestart') },
  { key: 'planStartTimeEnd', label: t('entity.apsSchedule.planstarttimeend') },
  { key: 'planEndTimeStart', label: t('entity.apsSchedule.planendtimestart') },
  { key: 'planEndTimeEnd', label: t('entity.apsSchedule.planendtimeend') },
  { key: 'planCycle', label: t('entity.apsSchedule.plancycle') },
  { key: 'workshopCode', label: t('entity.apsSchedule.workshopcode') },
  { key: 'workshopName', label: t('entity.apsSchedule.workshopname') },
  { key: 'productionLineCode', label: t('entity.apsSchedule.productionlinecode') },
  { key: 'productionLineName', label: t('entity.apsSchedule.productionlinename') },
  { key: 'scheduleStrategy', label: t('entity.apsSchedule.schedulestrategy') },
  { key: 'scheduleAlgorithm', label: t('entity.apsSchedule.schedulealgorithm') },
  { key: 'optimizationObjective', label: t('entity.apsSchedule.optimizationobjective') },
  { key: 'scheduleStatus', label: t('entity.apsSchedule.schedulestatus') },
  { key: 'plannerId', label: t('entity.apsSchedule.plannerid') },
  { key: 'plannerName', label: t('entity.apsSchedule.plannername') },
  { key: 'publishTimeStart', label: t('entity.apsSchedule.publishtimestart') },
  { key: 'publishTimeEnd', label: t('entity.apsSchedule.publishtimeend') },
  { key: 'publishUserId', label: t('entity.apsSchedule.publishuserid') },
  { key: 'publishUserName', label: t('entity.apsSchedule.publishusername') },
  { key: 'scheduleDescription', label: t('entity.apsSchedule.scheduledescription') },
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
const entityIdName = 'apsScheduleId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主子表展开行 keys（手风琴，仅一行展开） */
const expandedRowKeys = ref<string[]>([])

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 展开行预览：apsScheduleItem 列 */
const apsScheduleItemExpandColumns = computed(() => [
  {
    title: t('entity.apsScheduleItem.apsschedulename'),
    dataIndex: 'apsScheduleName',
    key: 'apsScheduleName',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleItem.apsschedulecode'),
    dataIndex: 'apsScheduleCode',
    key: 'apsScheduleCode',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleItem.workordercode'),
    dataIndex: 'workOrderCode',
    key: 'workOrderCode',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleItem.productcode'),
    dataIndex: 'productCode',
    key: 'productCode',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleItem.productname'),
    dataIndex: 'productName',
    key: 'productName',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleItem.workcentercode'),
    dataIndex: 'workCenterCode',
    key: 'workCenterCode',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleItem.workcentername'),
    dataIndex: 'workCenterName',
    key: 'workCenterName',
    ellipsis: true,
  },
])

/** 展开行预览：apsScheduleChangeLog 列 */
const apsScheduleChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.apsScheduleChangeLog.apsschedulename'),
    dataIndex: 'apsScheduleName',
    key: 'apsScheduleName',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    ellipsis: true,
  },
  {
    title: t('entity.apsScheduleChangeLog.schedule'),
    dataIndex: 'schedule',
    key: 'schedule',
    ellipsis: true,
  },
])

/** 读取主表行上的 apsScheduleItem 子表缓存 */
function getApsScheduleItemRows(record: ApsSchedule): ApsScheduleItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 apsScheduleItem 子表 */
function hasApsScheduleItemRows(record: ApsSchedule): boolean {
  return getApsScheduleItemRows(record).length > 0
}

/** 读取主表行上的 apsScheduleChangeLog 子表缓存 */
function getApsScheduleChangeLogRows(record: ApsSchedule): ApsScheduleChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 apsScheduleChangeLog 子表 */
function hasApsScheduleChangeLogRows(record: ApsSchedule): boolean {
  return getApsScheduleChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadApsScheduleDetail(record: ApsSchedule): Promise<ApsSchedule | null> {
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
/** 懒加载 apsScheduleItem 子表（ApsScheduleItemQuery + apsScheduleItemApi，与主表 ApsScheduleQuery 分离） */
async function loadApsScheduleItemForApsSchedule(record: ApsSchedule): Promise<ApsScheduleItem[]> {
  const masterId = getApsScheduleId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ApsScheduleItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      apsScheduleId: masterId,
    }
    const result = await apsScheduleItemApi.getApsScheduleItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getApsScheduleId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as ApsSchedule
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 apsScheduleChangeLog 子表（ApsScheduleChangeLogQuery + apsScheduleChangeLogApi，与主表 ApsScheduleQuery 分离） */
async function loadApsScheduleChangeLogForApsSchedule(record: ApsSchedule): Promise<ApsScheduleChangeLog[]> {
  const masterId = getApsScheduleId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: ApsScheduleChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      apsScheduleId: masterId,
    }
    const result = await apsScheduleChangeLogApi.getApsScheduleChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getApsScheduleId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as ApsSchedule
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureApsScheduleChildrenLoaded(record: ApsSchedule) {
  if (!hasApsScheduleItemRows(record)) {
    await loadApsScheduleItemForApsSchedule(record)
  }
  if (!hasApsScheduleChangeLogRows(record)) {
    await loadApsScheduleChangeLogForApsSchedule(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: ApsSchedule) {
  const key = getApsScheduleId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureApsScheduleChildrenLoaded(record)
  expandedRowKeys.value = [key]
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
    title: t('entity.apsSchedule.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.apsSchedule.schedulecode'),
    dataIndex: 'scheduleCode',
    key: 'scheduleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleCode') ?? ''
  },
  {
    title: t('entity.apsSchedule.schedulename'),
    dataIndex: 'scheduleName',
    key: 'scheduleName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleName') ?? ''
  },
  {
    title: t('entity.apsSchedule.scheduletype'),
    dataIndex: 'scheduleType',
    key: 'scheduleType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleType') ?? ''
  },
  {
    title: t('entity.apsSchedule.plandate'),
    dataIndex: 'planDate',
    key: 'planDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'planDate') ?? ''
  },
  {
    title: t('entity.apsSchedule.planstarttime'),
    dataIndex: 'planStartTime',
    key: 'planStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'planStartTime') ?? ''
  },
  {
    title: t('entity.apsSchedule.planendtime'),
    dataIndex: 'planEndTime',
    key: 'planEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'planEndTime') ?? ''
  },
  {
    title: t('entity.apsSchedule.plancycle'),
    dataIndex: 'planCycle',
    key: 'planCycle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'planCycle') ?? ''
  },
  {
    title: t('entity.apsSchedule.workshopcode'),
    dataIndex: 'workshopCode',
    key: 'workshopCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'workshopCode') ?? ''
  },
  {
    title: t('entity.apsSchedule.workshopname'),
    dataIndex: 'workshopName',
    key: 'workshopName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'workshopName') ?? ''
  },
  {
    title: t('entity.apsSchedule.productionlinecode'),
    dataIndex: 'productionLineCode',
    key: 'productionLineCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'productionLineCode') ?? ''
  },
  {
    title: t('entity.apsSchedule.productionlinename'),
    dataIndex: 'productionLineName',
    key: 'productionLineName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'productionLineName') ?? ''
  },
  {
    title: t('entity.apsSchedule.schedulestrategy'),
    dataIndex: 'scheduleStrategy',
    key: 'scheduleStrategy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleStrategy') ?? ''
  },
  {
    title: t('entity.apsSchedule.schedulealgorithm'),
    dataIndex: 'scheduleAlgorithm',
    key: 'scheduleAlgorithm',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleAlgorithm') ?? ''
  },
  {
    title: t('entity.apsSchedule.optimizationobjective'),
    dataIndex: 'optimizationObjective',
    key: 'optimizationObjective',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'optimizationObjective') ?? ''
  },
  {
    title: t('entity.apsSchedule.schedulestatus'),
    dataIndex: 'scheduleStatus',
    key: 'scheduleStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'scheduleStatus') ?? ''
  },
  {
    title: t('entity.apsSchedule.plannerid'),
    dataIndex: 'plannerId',
    key: 'plannerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'plannerId') ?? ''
  },
  {
    title: t('entity.apsSchedule.plannername'),
    dataIndex: 'plannerName',
    key: 'plannerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'plannerName') ?? ''
  },
  {
    title: t('entity.apsSchedule.publishtime'),
    dataIndex: 'publishTime',
    key: 'publishTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'publishTime') ?? ''
  },
  {
    title: t('entity.apsSchedule.publishuserid'),
    dataIndex: 'publishUserId',
    key: 'publishUserId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'publishUserId') ?? ''
  },
  {
    title: t('entity.apsSchedule.publishusername'),
    dataIndex: 'publishUserName',
    key: 'publishUserName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getApsScheduleField(record, 'publishUserName') ?? ''
  },
  {
    title: t('entity.apsSchedule.scheduledescription'),
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
        permission: 'logistics:manufacturing:scheduling:apsschedule:update',
        onClick: (record: ApsSchedule) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:scheduling:apsschedule:delete',
        onClick: (record: ApsSchedule) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getApsScheduleId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getApsScheduleField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ApsSchedule[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ApsSchedule, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getApsScheduleId(selectedRow.value) === getApsScheduleId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ApsSchedule[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: ApsSchedule) => ({
  onClick: () => {
    const key = getApsScheduleId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getApsScheduleId(item)))
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
    const params: ApsScheduleQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getApsScheduleList(params)
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

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
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
  extFieldJson: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.apsSchedule._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: ApsSchedule) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.apsSchedule._self') })
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.apsSchedule._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.apsSchedule._self') }))
    } else {
      await createApsSchedule(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.apsSchedule._self') }))
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
  const res = await getApsScheduleTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importApsSchedule(file, sheetName)
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
    const exportQuery: ApsScheduleQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportApsSchedule(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.apsSchedule._self') }))
  } catch (error: any) {
    logger.error('[ApsSchedule] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.apsSchedule._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ApsSchedule) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.apsSchedule._self'), name: t('common.tip.this.target', { target: t('entity.apsSchedule._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteApsScheduleById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.apsSchedule._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.apsSchedule._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.apsSchedule._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteApsScheduleBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.apsSchedule._self') }))
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
.logistics-manufacturing-scheduling-aps-schedule {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
