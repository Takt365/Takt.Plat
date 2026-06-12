<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-failure -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：品质问题应对主表管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-cost-quality-failure">
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
      create-permission="logistics:quality:cost:qualityfailure:create"
      update-permission="logistics:quality:cost:qualityfailure:update"
      delete-permission="logistics:quality:cost:qualityfailure:delete"
      import-permission="logistics:quality:cost:qualityfailure:import"
      export-permission="logistics:quality:cost:qualityfailure:export"
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
      :id-column-key="'qualityFailureId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getQualityFailureId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityFailureMeeting._self') }}</div>
          <a-table
            v-if="hasQualityFailureMeetingRows(record)"
            :columns="qualityFailureMeetingExpandColumns"
            :data-source="getQualityFailureMeetingRows(record)"
            :row-key="(row: QualityFailureMeeting, index?: number) => row?.qualityFailureMeetingId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityFailureAssyRework._self') }}</div>
          <a-table
            v-if="hasQualityFailureAssyReworkRows(record)"
            :columns="qualityFailureAssyReworkExpandColumns"
            :data-source="getQualityFailureAssyReworkRows(record)"
            :row-key="(row: QualityFailureAssyRework, index?: number) => row?.qualityFailureAssyReworkId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityFailurePcbaRework._self') }}</div>
          <a-table
            v-if="hasQualityFailurePcbaReworkRows(record)"
            :columns="qualityFailurePcbaReworkExpandColumns"
            :data-source="getQualityFailurePcbaReworkRows(record)"
            :row-key="(row: QualityFailurePcbaRework, index?: number) => row?.qualityFailurePcbaReworkId || String(index ?? 0)"
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
      <QualityFailureForm
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
      :storage-key="'takt-query-fields-logistics-quality-cost-quality-failure'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.qualityFailure.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityFailureCode')">
      <a-form-item :label="t('entity.qualityFailure.code')">
        <a-input
          v-model:value="advancedQueryForm.qualityFailureCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('failureDateStart')">
      <a-form-item :label="t('entity.qualityFailure.failuredatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.failureDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.qualityFailure.failuredatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('failureDateEnd')">
      <a-form-item :label="t('entity.qualityFailure.failuredateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.failureDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.qualityFailure.failuredateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('model')">
      <a-form-item :label="t('entity.qualityFailure.model')">
        <a-input
          v-model:value="advancedQueryForm.model"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.model') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lot')">
      <a-form-item :label="t('entity.qualityFailure.lot')">
        <a-input
          v-model:value="advancedQueryForm.lot"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.lot') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityProblemsResponse')">
      <a-form-item :label="t('entity.qualityFailure.qualityproblemsresponse')">
        <a-input
          v-model:value="advancedQueryForm.qualityProblemsResponse"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.qualityproblemsresponse') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reworkDueToDefects')">
      <a-form-item :label="t('entity.qualityFailure.reworkduetodefects')">
        <a-input
          v-model:value="advancedQueryForm.reworkDueToDefects"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.reworkduetodefects') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('needRework')">
      <a-form-item :label="t('entity.qualityFailure.needrework')">
        <a-input
          v-model:value="advancedQueryForm.needRework"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.needrework') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalTimeMinutes')">
      <a-form-item :label="t('entity.qualityFailure.totaltimeminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.totalTimeMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.totaltimeminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalCost')">
      <a-form-item :label="t('entity.qualityFailure.totalcost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.totalcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCurrency')">
      <a-form-item :label="t('entity.qualityFailure.costcurrency')">
        <a-input
          v-model:value="advancedQueryForm.costCurrency"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityFailure.costcurrency') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.qualityFailure._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.qualityFailure._self"
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
      :id-column-key="'qualityFailureId'"
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
 * 品质问题应对主表管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-failure
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import QualityFailureForm from './components/quality-failure-form.vue'
import { getQualityFailureList, getQualityFailureById, createQualityFailure, updateQualityFailure, deleteQualityFailureById, deleteQualityFailureBatch, getQualityFailureTemplate, importQualityFailure, exportQualityFailure } from '@/api/logistics/quality/cost/quality-failure'
import * as qualityFailureMeetingApi from '@/api/logistics/quality/cost/quality-failure-meeting'
import * as qualityFailureAssyReworkApi from '@/api/logistics/quality/cost/quality-failure-assy-rework'
import * as qualityFailurePcbaReworkApi from '@/api/logistics/quality/cost/quality-failure-pcba-rework'
import type { QualityFailureMeeting, QualityFailureMeetingQuery } from '@/types/logistics/quality/cost/quality-failure-meeting'
import type { QualityFailureAssyRework, QualityFailureAssyReworkQuery } from '@/types/logistics/quality/cost/quality-failure-assy-rework'
import type { QualityFailurePcbaRework, QualityFailurePcbaReworkQuery } from '@/types/logistics/quality/cost/quality-failure-pcba-rework'
import type { QualityFailure, QualityFailureQuery, QualityFailureCreate, QualityFailureUpdate } from '@/types/logistics/quality/cost/quality-failure'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQualityFailure')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.qualityFailure._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<QualityFailure[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<QualityFailure | null>(null)
/** 表格多选行 */
const selectedRows = ref<QualityFailure[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<QualityFailure>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  qualityFailureCode: '',
  failureDateStart: '',
  failureDateEnd: '',
  model: '',
  lot: '',
  qualityProblemsResponse: '',
  reworkDueToDefects: '',
  needRework: '',
  totalTimeMinutes: undefined as number | undefined,
  totalCost: undefined as number | undefined,
  costCurrency: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.qualityFailure.plantcode') },
  { key: 'qualityFailureCode', label: t('entity.qualityFailure.code') },
  { key: 'failureDateStart', label: t('entity.qualityFailure.failuredatestart') },
  { key: 'failureDateEnd', label: t('entity.qualityFailure.failuredateend') },
  { key: 'model', label: t('entity.qualityFailure.model') },
  { key: 'lot', label: t('entity.qualityFailure.lot') },
  { key: 'qualityProblemsResponse', label: t('entity.qualityFailure.qualityproblemsresponse') },
  { key: 'reworkDueToDefects', label: t('entity.qualityFailure.reworkduetodefects') },
  { key: 'needRework', label: t('entity.qualityFailure.needrework') },
  { key: 'totalTimeMinutes', label: t('entity.qualityFailure.totaltimeminutes') },
  { key: 'totalCost', label: t('entity.qualityFailure.totalcost') },
  { key: 'costCurrency', label: t('entity.qualityFailure.costcurrency') },
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
const entityIdName = 'qualityFailureId'
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

/** 展开行预览：qualityFailureMeeting 列 */
const qualityFailureMeetingExpandColumns = computed(() => [
  {
    title: t('entity.qualityFailureMeeting.qualityfailurename'),
    dataIndex: 'qualityFailureName',
    key: 'qualityFailureName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureMeeting.qualityfailurecode'),
    dataIndex: 'qualityFailureCode',
    key: 'qualityFailureCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureMeeting.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureMeeting.directmanpowercostperminute'),
    dataIndex: 'directManpowerCostPerMinute',
    key: 'directManpowerCostPerMinute',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureMeeting.indirectmanpowercostperminute'),
    dataIndex: 'indirectManpowerCostPerMinute',
    key: 'indirectManpowerCostPerMinute',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureMeeting.meetinginvestigationcontent'),
    dataIndex: 'meetingInvestigationContent',
    key: 'meetingInvestigationContent',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureMeeting.meetinginvestigationcost'),
    dataIndex: 'meetingInvestigationCost',
    key: 'meetingInvestigationCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureMeeting.meetingtimeminutes'),
    dataIndex: 'meetingTimeMinutes',
    key: 'meetingTimeMinutes',
    ellipsis: true,
  },
])

/** 展开行预览：qualityFailureAssyRework 列 */
const qualityFailureAssyReworkExpandColumns = computed(() => [
  {
    title: t('entity.qualityFailureAssyRework.qualityfailurename'),
    dataIndex: 'qualityFailureName',
    key: 'qualityFailureName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureAssyRework.qualityfailurecode'),
    dataIndex: 'qualityFailureCode',
    key: 'qualityFailureCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureAssyRework.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureAssyRework.assydefectparts'),
    dataIndex: 'assyDefectParts',
    key: 'assyDefectParts',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureAssyRework.assyreworkcost'),
    dataIndex: 'assyReworkCost',
    key: 'assyReworkCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureAssyRework.assyreworktimeminutes'),
    dataIndex: 'assyReworkTimeMinutes',
    key: 'assyReworkTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureAssyRework.assyreinspectiontimeminutes'),
    dataIndex: 'assyReinspectionTimeMinutes',
    key: 'assyReinspectionTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailureAssyRework.assytravelcost'),
    dataIndex: 'assyTravelCost',
    key: 'assyTravelCost',
    ellipsis: true,
  },
])

/** 展开行预览：qualityFailurePcbaRework 列 */
const qualityFailurePcbaReworkExpandColumns = computed(() => [
  {
    title: t('entity.qualityFailurePcbaRework.qualityfailurename'),
    dataIndex: 'qualityFailureName',
    key: 'qualityFailureName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailurePcbaRework.qualityfailurecode'),
    dataIndex: 'qualityFailureCode',
    key: 'qualityFailureCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailurePcbaRework.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbadefectparts'),
    dataIndex: 'pcbaDefectParts',
    key: 'pcbaDefectParts',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbareworkcost'),
    dataIndex: 'pcbaReworkCost',
    key: 'pcbaReworkCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbareworktimeminutes'),
    dataIndex: 'pcbaReworkTimeMinutes',
    key: 'pcbaReworkTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbareinspectiontimeminutes'),
    dataIndex: 'pcbaReinspectionTimeMinutes',
    key: 'pcbaReinspectionTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityFailurePcbaRework.pcbatravelcost'),
    dataIndex: 'pcbaTravelCost',
    key: 'pcbaTravelCost',
    ellipsis: true,
  },
])

/** 读取主表行上的 qualityFailureMeeting 子表缓存 */
function getQualityFailureMeetingRows(record: QualityFailure): QualityFailureMeeting[] {
  return (record as any)?.meetingItems ?? []
}

/** 主表行是否已加载 qualityFailureMeeting 子表 */
function hasQualityFailureMeetingRows(record: QualityFailure): boolean {
  return getQualityFailureMeetingRows(record).length > 0
}

/** 读取主表行上的 qualityFailureAssyRework 子表缓存 */
function getQualityFailureAssyReworkRows(record: QualityFailure): QualityFailureAssyRework[] {
  return (record as any)?.assyReworkItems ?? []
}

/** 主表行是否已加载 qualityFailureAssyRework 子表 */
function hasQualityFailureAssyReworkRows(record: QualityFailure): boolean {
  return getQualityFailureAssyReworkRows(record).length > 0
}

/** 读取主表行上的 qualityFailurePcbaRework 子表缓存 */
function getQualityFailurePcbaReworkRows(record: QualityFailure): QualityFailurePcbaRework[] {
  return (record as any)?.pcbaReworkItems ?? []
}

/** 主表行是否已加载 qualityFailurePcbaRework 子表 */
function hasQualityFailurePcbaReworkRows(record: QualityFailure): boolean {
  return getQualityFailurePcbaReworkRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadQualityFailureDetail(record: QualityFailure): Promise<QualityFailure | null> {
  const id = getQualityFailureId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getQualityFailureById(id)
    const index = dataSource.value.findIndex((row) => getQualityFailureId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as QualityFailure
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 qualityFailureMeeting 子表（QualityFailureMeetingQuery + qualityFailureMeetingApi，与主表 QualityFailureQuery 分离） */
async function loadQualityFailureMeetingForQualityFailure(record: QualityFailure): Promise<QualityFailureMeeting[]> {
  const masterId = getQualityFailureId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: QualityFailureMeetingQuery = {
      pageIndex: 1,
      pageSize: 500,
      qualityFailureId: masterId,
    }
    const result = await qualityFailureMeetingApi.getQualityFailureMeetingList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getQualityFailureId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, meetingItems: rows } as QualityFailure
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 qualityFailureAssyRework 子表（QualityFailureAssyReworkQuery + qualityFailureAssyReworkApi，与主表 QualityFailureQuery 分离） */
async function loadQualityFailureAssyReworkForQualityFailure(record: QualityFailure): Promise<QualityFailureAssyRework[]> {
  const masterId = getQualityFailureId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: QualityFailureAssyReworkQuery = {
      pageIndex: 1,
      pageSize: 500,
      qualityFailureId: masterId,
    }
    const result = await qualityFailureAssyReworkApi.getQualityFailureAssyReworkList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getQualityFailureId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, assyReworkItems: rows } as QualityFailure
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 qualityFailurePcbaRework 子表（QualityFailurePcbaReworkQuery + qualityFailurePcbaReworkApi，与主表 QualityFailureQuery 分离） */
async function loadQualityFailurePcbaReworkForQualityFailure(record: QualityFailure): Promise<QualityFailurePcbaRework[]> {
  const masterId = getQualityFailureId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: QualityFailurePcbaReworkQuery = {
      pageIndex: 1,
      pageSize: 500,
      qualityFailureId: masterId,
    }
    const result = await qualityFailurePcbaReworkApi.getQualityFailurePcbaReworkList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getQualityFailureId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, pcbaReworkItems: rows } as QualityFailure
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureQualityFailureChildrenLoaded(record: QualityFailure) {
  if (!hasQualityFailureMeetingRows(record)) {
    await loadQualityFailureMeetingForQualityFailure(record)
  }
  if (!hasQualityFailureAssyReworkRows(record)) {
    await loadQualityFailureAssyReworkForQualityFailure(record)
  }
  if (!hasQualityFailurePcbaReworkRows(record)) {
    await loadQualityFailurePcbaReworkForQualityFailure(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: QualityFailure) {
  const key = getQualityFailureId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureQualityFailureChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'qualityFailureId',
    key: 'qualityFailureId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'qualityFailureId') ?? ''
  },
  {
    title: t('entity.qualityFailure.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.qualityFailure.code'),
    dataIndex: 'qualityFailureCode',
    key: 'qualityFailureCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'qualityFailureCode') ?? ''
  },
  {
    title: t('entity.qualityFailure.failuredate'),
    dataIndex: 'failureDate',
    key: 'failureDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'failureDate') ?? ''
  },
  {
    title: t('entity.qualityFailure.model'),
    dataIndex: 'model',
    key: 'model',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'model') ?? ''
  },
  {
    title: t('entity.qualityFailure.lot'),
    dataIndex: 'lot',
    key: 'lot',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'lot') ?? ''
  },
  {
    title: t('entity.qualityFailure.qualityproblemsresponse'),
    dataIndex: 'qualityProblemsResponse',
    key: 'qualityProblemsResponse',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'qualityProblemsResponse') ?? ''
  },
  {
    title: t('entity.qualityFailure.reworkduetodefects'),
    dataIndex: 'reworkDueToDefects',
    key: 'reworkDueToDefects',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'reworkDueToDefects') ?? ''
  },
  {
    title: t('entity.qualityFailure.needrework'),
    dataIndex: 'needRework',
    key: 'needRework',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'needRework') ?? ''
  },
  {
    title: t('entity.qualityFailure.totaltimeminutes'),
    dataIndex: 'totalTimeMinutes',
    key: 'totalTimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'totalTimeMinutes') ?? ''
  },
  {
    title: t('entity.qualityFailure.totalcost'),
    dataIndex: 'totalCost',
    key: 'totalCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'totalCost') ?? ''
  },
  {
    title: t('entity.qualityFailure.costcurrency'),
    dataIndex: 'costCurrency',
    key: 'costCurrency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityFailureField(record, 'costCurrency') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:cost:qualityfailure:update',
        onClick: (record: QualityFailure) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:cost:qualityfailure:delete',
        onClick: (record: QualityFailure) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getQualityFailureId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getQualityFailureField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QualityFailure[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QualityFailure, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getQualityFailureId(selectedRow.value) === getQualityFailureId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QualityFailure[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: QualityFailure) => ({
  onClick: () => {
    const key = getQualityFailureId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getQualityFailureId(item)))
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
    const params: QualityFailureQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getQualityFailureList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[QualityFailure] 加载数据失败', { error })
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
  plantCode: '',
  qualityFailureCode: '',
  failureDateStart: '',
  failureDateEnd: '',
  model: '',
  lot: '',
  qualityProblemsResponse: '',
  reworkDueToDefects: '',
  needRework: '',
  totalTimeMinutes: undefined as number | undefined,
  totalCost: undefined as number | undefined,
  costCurrency: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.qualityFailure._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: QualityFailure) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.qualityFailure._self') })
  formLoading.value = true
  try {
    const detail = await loadQualityFailureDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.qualityFailure._self') }))
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
      await updateQualityFailure(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.qualityFailure._self') }))
    } else {
      await createQualityFailure(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.qualityFailure._self') }))
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
  const res = await getQualityFailureTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQualityFailure(file, sheetName)
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
    const exportQuery: QualityFailureQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportQualityFailure(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.qualityFailure._self') }))
  } catch (error: any) {
    logger.error('[QualityFailure] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.qualityFailure._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: QualityFailure) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.qualityFailure._self'), name: t('common.tip.this.target', { target: t('entity.qualityFailure._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQualityFailureById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.qualityFailure._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.qualityFailure._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.qualityFailure._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteQualityFailureBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.qualityFailure._self') }))
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
  qualityFailureCode: '',
  failureDateStart: '',
  failureDateEnd: '',
  model: '',
  lot: '',
  qualityProblemsResponse: '',
  reworkDueToDefects: '',
  needRework: '',
  totalTimeMinutes: undefined as number | undefined,
  totalCost: undefined as number | undefined,
  costCurrency: '',
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
.logistics-quality-cost-quality-failure {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
