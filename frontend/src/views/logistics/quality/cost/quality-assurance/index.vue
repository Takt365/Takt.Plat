<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-operation -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：品质业务主表管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-cost-quality-operation">
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
      create-permission="logistics:quality:cost:qualityoperation:create"
      update-permission="logistics:quality:cost:qualityoperation:update"
      delete-permission="logistics:quality:cost:qualityoperation:delete"
      import-permission="logistics:quality:cost:qualityoperation:import"
      export-permission="logistics:quality:cost:qualityoperation:export"
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
      :id-column-key="'qualityOperationId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getQualityOperationId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityOperationIncoming._self') }}</div>
          <a-table
            v-if="hasQualityOperationIncomingRows(record)"
            :columns="qualityOperationIncomingExpandColumns"
            :data-source="getQualityOperationIncomingRows(record)"
            :row-key="(row: QualityOperationIncoming, index?: number) => row?.qualityOperationIncomingId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityOperationFirstArticle._self') }}</div>
          <a-table
            v-if="hasQualityOperationFirstArticleRows(record)"
            :columns="qualityOperationFirstArticleExpandColumns"
            :data-source="getQualityOperationFirstArticleRows(record)"
            :row-key="(row: QualityOperationFirstArticle, index?: number) => row?.qualityOperationFirstArticleId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityOperationCalibration._self') }}</div>
          <a-table
            v-if="hasQualityOperationCalibrationRows(record)"
            :columns="qualityOperationCalibrationExpandColumns"
            :data-source="getQualityOperationCalibrationRows(record)"
            :row-key="(row: QualityOperationCalibration, index?: number) => row?.qualityOperationCalibrationId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityOperationOther._self') }}</div>
          <a-table
            v-if="hasQualityOperationOtherRows(record)"
            :columns="qualityOperationOtherExpandColumns"
            :data-source="getQualityOperationOtherRows(record)"
            :row-key="(row: QualityOperationOther, index?: number) => row?.qualityOperationOtherId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityOperationOutgoing._self') }}</div>
          <a-table
            v-if="hasQualityOperationOutgoingRows(record)"
            :columns="qualityOperationOutgoingExpandColumns"
            :data-source="getQualityOperationOutgoingRows(record)"
            :row-key="(row: QualityOperationOutgoing, index?: number) => row?.qualityOperationOutgoingId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityOperationReliability._self') }}</div>
          <a-table
            v-if="hasQualityOperationReliabilityRows(record)"
            :columns="qualityOperationReliabilityExpandColumns"
            :data-source="getQualityOperationReliabilityRows(record)"
            :row-key="(row: QualityOperationReliability, index?: number) => row?.qualityOperationReliabilityId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityOperationCustomerResponse._self') }}</div>
          <a-table
            v-if="hasQualityOperationCustomerResponseRows(record)"
            :columns="qualityOperationCustomerResponseExpandColumns"
            :data-source="getQualityOperationCustomerResponseRows(record)"
            :row-key="(row: QualityOperationCustomerResponse, index?: number) => row?.qualityOperationCustomerResponseId || String(index ?? 0)"
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
      <QualityOperationForm
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
      :storage-key="'takt-query-fields-logistics-quality-cost-quality-operation'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.qualityOperation.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityOperationCode')">
      <a-form-item :label="t('entity.qualityOperation.code')">
        <a-input
          v-model:value="advancedQueryForm.qualityOperationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operationMonth')">
      <a-form-item :label="t('entity.qualityOperation.operationmonth')">
        <a-input
          v-model:value="advancedQueryForm.operationMonth"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.operationmonth') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerName')">
      <a-form-item :label="t('entity.qualityOperation.customername')">
        <a-input
          v-model:value="advancedQueryForm.customerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.customername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('debitNoteNo')">
      <a-form-item :label="t('entity.qualityOperation.debitnoteno')">
        <a-textarea
          v-model:value="advancedQueryForm.debitNoteNo"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityOperation.debitnoteno') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('recorder')">
      <a-form-item :label="t('entity.qualityOperation.recorder')">
        <a-input
          v-model:value="advancedQueryForm.recorder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.recorder') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQualityCost')">
      <a-form-item :label="t('entity.qualityOperation.totalqualitycost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQualityCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.totalqualitycost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCurrency')">
      <a-form-item :label="t('entity.qualityOperation.costcurrency')">
        <a-input
          v-model:value="advancedQueryForm.costCurrency"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityOperation.costcurrency') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.qualityOperation._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.qualityOperation._self"
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
      :id-column-key="'qualityOperationId'"
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
 * 品质业务主表管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-operation
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import QualityOperationForm from './components/quality-operation-form.vue'
import { getQualityOperationList, getQualityOperationById, createQualityOperation, updateQualityOperation, deleteQualityOperationById, deleteQualityOperationBatch, getQualityOperationTemplate, importQualityOperation, exportQualityOperation } from '@/api/logistics/quality/cost/quality-operation'
import * as qualityOperationIncomingApi from '@/api/logistics/quality/cost/quality-operation-incoming'
import * as qualityOperationFirstArticleApi from '@/api/logistics/quality/cost/quality-operation-first-article'
import * as qualityOperationCalibrationApi from '@/api/logistics/quality/cost/quality-operation-calibration'
import * as qualityOperationOtherApi from '@/api/logistics/quality/cost/quality-operation-other'
import * as qualityOperationOutgoingApi from '@/api/logistics/quality/cost/quality-operation-outgoing'
import * as qualityOperationReliabilityApi from '@/api/logistics/quality/cost/quality-operation-reliability'
import type { QualityOperationIncoming, QualityOperationIncomingQuery } from '@/types/logistics/quality/cost/quality-operation-incoming'
import type { QualityOperationFirstArticle, QualityOperationFirstArticleQuery } from '@/types/logistics/quality/cost/quality-operation-first-article'
import type { QualityOperationCalibration, QualityOperationCalibrationQuery } from '@/types/logistics/quality/cost/quality-operation-calibration'
import type { QualityOperationOther, QualityOperationOtherQuery } from '@/types/logistics/quality/cost/quality-operation-other'
import type { QualityOperationOutgoing, QualityOperationOutgoingQuery } from '@/types/logistics/quality/cost/quality-operation-outgoing'
import type { QualityOperationReliability, QualityOperationReliabilityQuery } from '@/types/logistics/quality/cost/quality-operation-reliability'
import type { QualityOperationCustomerResponse } from '@/types/logistics/quality/cost/quality-operation-customer-response'
import type { QualityOperation, QualityOperationQuery, QualityOperationCreate, QualityOperationUpdate } from '@/types/logistics/quality/cost/quality-operation'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQualityOperation')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.qualityOperation._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<QualityOperation[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<QualityOperation | null>(null)
/** 表格多选行 */
const selectedRows = ref<QualityOperation[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<QualityOperation>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  qualityOperationCode: '',
  operationMonth: '',
  customerName: '',
  debitNoteNo: '',
  recorder: '',
  totalQualityCost: undefined as number | undefined,
  costCurrency: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.qualityOperation.plantcode') },
  { key: 'qualityOperationCode', label: t('entity.qualityOperation.code') },
  { key: 'operationMonth', label: t('entity.qualityOperation.operationmonth') },
  { key: 'customerName', label: t('entity.qualityOperation.customername') },
  { key: 'debitNoteNo', label: t('entity.qualityOperation.debitnoteno') },
  { key: 'recorder', label: t('entity.qualityOperation.recorder') },
  { key: 'totalQualityCost', label: t('entity.qualityOperation.totalqualitycost') },
  { key: 'costCurrency', label: t('entity.qualityOperation.costcurrency') },
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
const entityIdName = 'qualityOperationId'
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

/** 展开行预览：qualityOperationIncoming 列 */
const qualityOperationIncomingExpandColumns = computed(() => [
  {
    title: t('entity.qualityOperationIncoming.qualityoperationname'),
    dataIndex: 'qualityOperationName',
    key: 'qualityOperationName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationIncoming.qualityoperationcode'),
    dataIndex: 'qualityOperationCode',
    key: 'qualityOperationCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationIncoming.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationIncoming.directmanpowercostperminute'),
    dataIndex: 'directManpowerCostPerMinute',
    key: 'directManpowerCostPerMinute',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationIncoming.incominginspectioncost'),
    dataIndex: 'incomingInspectionCost',
    key: 'incomingInspectionCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationIncoming.inspectiontimeminutes'),
    dataIndex: 'inspectionTimeMinutes',
    key: 'inspectionTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationIncoming.travelcost'),
    dataIndex: 'travelCost',
    key: 'travelCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationIncoming.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    ellipsis: true,
  },
])

/** 展开行预览：qualityOperationFirstArticle 列 */
const qualityOperationFirstArticleExpandColumns = computed(() => [
  {
    title: t('entity.qualityOperationFirstArticle.qualityoperationname'),
    dataIndex: 'qualityOperationName',
    key: 'qualityOperationName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationFirstArticle.qualityoperationcode'),
    dataIndex: 'qualityOperationCode',
    key: 'qualityOperationCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationFirstArticle.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationFirstArticle.qualificationcost'),
    dataIndex: 'qualificationCost',
    key: 'qualificationCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationFirstArticle.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationFirstArticle.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationFirstArticle.qualificationnote'),
    dataIndex: 'qualificationNote',
    key: 'qualificationNote',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationFirstArticle.operation'),
    dataIndex: 'operation',
    key: 'operation',
    ellipsis: true,
  },
])

/** 展开行预览：qualityOperationCalibration 列 */
const qualityOperationCalibrationExpandColumns = computed(() => [
  {
    title: t('entity.qualityOperationCalibration.qualityoperationname'),
    dataIndex: 'qualityOperationName',
    key: 'qualityOperationName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCalibration.qualityoperationcode'),
    dataIndex: 'qualityOperationCode',
    key: 'qualityOperationCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCalibration.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCalibration.calibrationcost'),
    dataIndex: 'calibrationCost',
    key: 'calibrationCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCalibration.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCalibration.externalagentservicefee'),
    dataIndex: 'externalAgentServiceFee',
    key: 'externalAgentServiceFee',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCalibration.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCalibration.calibrationnote'),
    dataIndex: 'calibrationNote',
    key: 'calibrationNote',
    ellipsis: true,
  },
])

/** 展开行预览：qualityOperationOther 列 */
const qualityOperationOtherExpandColumns = computed(() => [
  {
    title: t('entity.qualityOperationOther.qualityoperationname'),
    dataIndex: 'qualityOperationName',
    key: 'qualityOperationName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOther.qualityoperationcode'),
    dataIndex: 'qualityOperationCode',
    key: 'qualityOperationCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOther.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOther.operationscost'),
    dataIndex: 'operationsCost',
    key: 'operationsCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOther.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOther.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOther.othernote'),
    dataIndex: 'otherNote',
    key: 'otherNote',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOther.operation'),
    dataIndex: 'operation',
    key: 'operation',
    ellipsis: true,
  },
])

/** 展开行预览：qualityOperationOutgoing 列 */
const qualityOperationOutgoingExpandColumns = computed(() => [
  {
    title: t('entity.qualityOperationOutgoing.qualityoperationname'),
    dataIndex: 'qualityOperationName',
    key: 'qualityOperationName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOutgoing.qualityoperationcode'),
    dataIndex: 'qualityOperationCode',
    key: 'qualityOperationCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOutgoing.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOutgoing.inspectioncost'),
    dataIndex: 'inspectionCost',
    key: 'inspectionCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOutgoing.inspectiontimeminutes'),
    dataIndex: 'inspectionTimeMinutes',
    key: 'inspectionTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOutgoing.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOutgoing.outgoingnote'),
    dataIndex: 'outgoingNote',
    key: 'outgoingNote',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationOutgoing.operation'),
    dataIndex: 'operation',
    key: 'operation',
    ellipsis: true,
  },
])

/** 展开行预览：qualityOperationReliability 列 */
const qualityOperationReliabilityExpandColumns = computed(() => [
  {
    title: t('entity.qualityOperationReliability.qualityoperationname'),
    dataIndex: 'qualityOperationName',
    key: 'qualityOperationName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationReliability.qualityoperationcode'),
    dataIndex: 'qualityOperationCode',
    key: 'qualityOperationCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationReliability.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationReliability.testcost'),
    dataIndex: 'testCost',
    key: 'testCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationReliability.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationReliability.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationReliability.reliabilitynote'),
    dataIndex: 'reliabilityNote',
    key: 'reliabilityNote',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationReliability.operation'),
    dataIndex: 'operation',
    key: 'operation',
    ellipsis: true,
  },
])

/** 展开行预览：qualityOperationCustomerResponse 列 */
const qualityOperationCustomerResponseExpandColumns = computed(() => [
  {
    title: t('entity.qualityOperationCustomerResponse.qualityoperationname'),
    dataIndex: 'qualityOperationName',
    key: 'qualityOperationName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.qualityoperationcode'),
    dataIndex: 'qualityOperationCode',
    key: 'qualityOperationCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.responsecost'),
    dataIndex: 'responseCost',
    key: 'responseCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.worktimeminutes'),
    dataIndex: 'workTimeMinutes',
    key: 'workTimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.otherexpenses'),
    dataIndex: 'otherExpenses',
    key: 'otherExpenses',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.customerresponsenote'),
    dataIndex: 'customerResponseNote',
    key: 'customerResponseNote',
    ellipsis: true,
  },
  {
    title: t('entity.qualityOperationCustomerResponse.operation'),
    dataIndex: 'operation',
    key: 'operation',
    ellipsis: true,
  },
])

/** 读取主表行上的 qualityOperationIncoming 子表缓存 */
function getQualityOperationIncomingRows(record: QualityOperation): QualityOperationIncoming[] {
  return (record as any)?.incomingItems ?? []
}

/** 主表行是否已加载 qualityOperationIncoming 子表 */
function hasQualityOperationIncomingRows(record: QualityOperation): boolean {
  return getQualityOperationIncomingRows(record).length > 0
}

/** 读取主表行上的 qualityOperationFirstArticle 子表缓存 */
function getQualityOperationFirstArticleRows(record: QualityOperation): QualityOperationFirstArticle[] {
  return (record as any)?.firstArticleItems ?? []
}

/** 主表行是否已加载 qualityOperationFirstArticle 子表 */
function hasQualityOperationFirstArticleRows(record: QualityOperation): boolean {
  return getQualityOperationFirstArticleRows(record).length > 0
}

/** 读取主表行上的 qualityOperationCalibration 子表缓存 */
function getQualityOperationCalibrationRows(record: QualityOperation): QualityOperationCalibration[] {
  return (record as any)?.calibrationItems ?? []
}

/** 主表行是否已加载 qualityOperationCalibration 子表 */
function hasQualityOperationCalibrationRows(record: QualityOperation): boolean {
  return getQualityOperationCalibrationRows(record).length > 0
}

/** 读取主表行上的 qualityOperationOther 子表缓存 */
function getQualityOperationOtherRows(record: QualityOperation): QualityOperationOther[] {
  return (record as any)?.otherItems ?? []
}

/** 主表行是否已加载 qualityOperationOther 子表 */
function hasQualityOperationOtherRows(record: QualityOperation): boolean {
  return getQualityOperationOtherRows(record).length > 0
}

/** 读取主表行上的 qualityOperationOutgoing 子表缓存 */
function getQualityOperationOutgoingRows(record: QualityOperation): QualityOperationOutgoing[] {
  return (record as any)?.outgoingItems ?? []
}

/** 主表行是否已加载 qualityOperationOutgoing 子表 */
function hasQualityOperationOutgoingRows(record: QualityOperation): boolean {
  return getQualityOperationOutgoingRows(record).length > 0
}

/** 读取主表行上的 qualityOperationReliability 子表缓存 */
function getQualityOperationReliabilityRows(record: QualityOperation): QualityOperationReliability[] {
  return (record as any)?.reliabilityItems ?? []
}

/** 主表行是否已加载 qualityOperationReliability 子表 */
function hasQualityOperationReliabilityRows(record: QualityOperation): boolean {
  return getQualityOperationReliabilityRows(record).length > 0
}

/** 读取主表行上的 qualityOperationCustomerResponse 子表缓存 */
function getQualityOperationCustomerResponseRows(record: QualityOperation): QualityOperationCustomerResponse[] {
  return (record as any)?.customerResponseItems ?? []
}

/** 主表行是否已加载 qualityOperationCustomerResponse 子表 */
function hasQualityOperationCustomerResponseRows(record: QualityOperation): boolean {
  return getQualityOperationCustomerResponseRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadQualityOperationDetail(record: QualityOperation): Promise<QualityOperation | null> {
  const id = getQualityOperationId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getQualityOperationById(id)
    const index = dataSource.value.findIndex((row) => getQualityOperationId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as QualityOperation
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 qualityOperationIncoming 子表（QualityOperationIncomingQuery + qualityOperationIncomingApi，与主表 QualityOperationQuery 分离） */
async function loadQualityOperationIncomingForQualityOperation(record: QualityOperation): Promise<QualityOperationIncoming[]> {
  const masterId = getQualityOperationId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: QualityOperationIncomingQuery = {
      pageIndex: 1,
      pageSize: 500,
      qualityOperationId: masterId,
    }
    const result = await qualityOperationIncomingApi.getQualityOperationIncomingList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getQualityOperationId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, incomingItems: rows } as QualityOperation
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 qualityOperationFirstArticle 子表（QualityOperationFirstArticleQuery + qualityOperationFirstArticleApi，与主表 QualityOperationQuery 分离） */
async function loadQualityOperationFirstArticleForQualityOperation(record: QualityOperation): Promise<QualityOperationFirstArticle[]> {
  const masterId = getQualityOperationId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: QualityOperationFirstArticleQuery = {
      pageIndex: 1,
      pageSize: 500,
      qualityOperationId: masterId,
    }
    const result = await qualityOperationFirstArticleApi.getQualityOperationFirstArticleList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getQualityOperationId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, firstArticleItems: rows } as QualityOperation
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 qualityOperationCalibration 子表（QualityOperationCalibrationQuery + qualityOperationCalibrationApi，与主表 QualityOperationQuery 分离） */
async function loadQualityOperationCalibrationForQualityOperation(record: QualityOperation): Promise<QualityOperationCalibration[]> {
  const masterId = getQualityOperationId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: QualityOperationCalibrationQuery = {
      pageIndex: 1,
      pageSize: 500,
      qualityOperationId: masterId,
    }
    const result = await qualityOperationCalibrationApi.getQualityOperationCalibrationList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getQualityOperationId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, calibrationItems: rows } as QualityOperation
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 qualityOperationOther 子表（QualityOperationOtherQuery + qualityOperationOtherApi，与主表 QualityOperationQuery 分离） */
async function loadQualityOperationOtherForQualityOperation(record: QualityOperation): Promise<QualityOperationOther[]> {
  const masterId = getQualityOperationId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: QualityOperationOtherQuery = {
      pageIndex: 1,
      pageSize: 500,
      qualityOperationId: masterId,
    }
    const result = await qualityOperationOtherApi.getQualityOperationOtherList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getQualityOperationId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, otherItems: rows } as QualityOperation
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 qualityOperationOutgoing 子表（QualityOperationOutgoingQuery + qualityOperationOutgoingApi，与主表 QualityOperationQuery 分离） */
async function loadQualityOperationOutgoingForQualityOperation(record: QualityOperation): Promise<QualityOperationOutgoing[]> {
  const masterId = getQualityOperationId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: QualityOperationOutgoingQuery = {
      pageIndex: 1,
      pageSize: 500,
      qualityOperationId: masterId,
    }
    const result = await qualityOperationOutgoingApi.getQualityOperationOutgoingList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getQualityOperationId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, outgoingItems: rows } as QualityOperation
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 qualityOperationReliability 子表（QualityOperationReliabilityQuery + qualityOperationReliabilityApi，与主表 QualityOperationQuery 分离） */
async function loadQualityOperationReliabilityForQualityOperation(record: QualityOperation): Promise<QualityOperationReliability[]> {
  const masterId = getQualityOperationId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: QualityOperationReliabilityQuery = {
      pageIndex: 1,
      pageSize: 500,
      qualityOperationId: masterId,
    }
    const result = await qualityOperationReliabilityApi.getQualityOperationReliabilityList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getQualityOperationId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, reliabilityItems: rows } as QualityOperation
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 通过主表详情接口加载 qualityOperationCustomerResponse 子表 */
async function loadQualityOperationCustomerResponseForQualityOperation(record: QualityOperation): Promise<QualityOperationCustomerResponse[]> {
  const detail = await loadQualityOperationDetail(record)
  return detail?.customerResponseItems ?? []
}

/** 展开前确保各子表已懒加载 */
async function ensureQualityOperationChildrenLoaded(record: QualityOperation) {
  if (!hasQualityOperationIncomingRows(record)) {
    await loadQualityOperationIncomingForQualityOperation(record)
  }
  if (!hasQualityOperationFirstArticleRows(record)) {
    await loadQualityOperationFirstArticleForQualityOperation(record)
  }
  if (!hasQualityOperationCalibrationRows(record)) {
    await loadQualityOperationCalibrationForQualityOperation(record)
  }
  if (!hasQualityOperationOtherRows(record)) {
    await loadQualityOperationOtherForQualityOperation(record)
  }
  if (!hasQualityOperationOutgoingRows(record)) {
    await loadQualityOperationOutgoingForQualityOperation(record)
  }
  if (!hasQualityOperationReliabilityRows(record)) {
    await loadQualityOperationReliabilityForQualityOperation(record)
  }
  if (!hasQualityOperationCustomerResponseRows(record)) {
    await loadQualityOperationCustomerResponseForQualityOperation(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: QualityOperation) {
  const key = getQualityOperationId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureQualityOperationChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'qualityOperationId',
    key: 'qualityOperationId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getQualityOperationField(record, 'qualityOperationId') ?? ''
  },
  {
    title: t('entity.qualityOperation.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityOperationField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.qualityOperation.code'),
    dataIndex: 'qualityOperationCode',
    key: 'qualityOperationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityOperationField(record, 'qualityOperationCode') ?? ''
  },
  {
    title: t('entity.qualityOperation.operationmonth'),
    dataIndex: 'operationMonth',
    key: 'operationMonth',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityOperationField(record, 'operationMonth') ?? ''
  },
  {
    title: t('entity.qualityOperation.customername'),
    dataIndex: 'customerName',
    key: 'customerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityOperationField(record, 'customerName') ?? ''
  },
  {
    title: t('entity.qualityOperation.debitnoteno'),
    dataIndex: 'debitNoteNo',
    key: 'debitNoteNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityOperationField(record, 'debitNoteNo') ?? ''
  },
  {
    title: t('entity.qualityOperation.recorder'),
    dataIndex: 'recorder',
    key: 'recorder',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityOperationField(record, 'recorder') ?? ''
  },
  {
    title: t('entity.qualityOperation.totalqualitycost'),
    dataIndex: 'totalQualityCost',
    key: 'totalQualityCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityOperationField(record, 'totalQualityCost') ?? ''
  },
  {
    title: t('entity.qualityOperation.costcurrency'),
    dataIndex: 'costCurrency',
    key: 'costCurrency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityOperationField(record, 'costCurrency') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:cost:qualityoperation:update',
        onClick: (record: QualityOperation) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:cost:qualityoperation:delete',
        onClick: (record: QualityOperation) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getQualityOperationId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getQualityOperationField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QualityOperation[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QualityOperation, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getQualityOperationId(selectedRow.value) === getQualityOperationId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QualityOperation[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: QualityOperation) => ({
  onClick: () => {
    const key = getQualityOperationId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getQualityOperationId(item)))
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
    const params: QualityOperationQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getQualityOperationList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[QualityOperation] 加载数据失败', { error })
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
  qualityOperationCode: '',
  operationMonth: '',
  customerName: '',
  debitNoteNo: '',
  recorder: '',
  totalQualityCost: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.qualityOperation._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: QualityOperation) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.qualityOperation._self') })
  formLoading.value = true
  try {
    const detail = await loadQualityOperationDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.qualityOperation._self') }))
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
      await updateQualityOperation(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.qualityOperation._self') }))
    } else {
      await createQualityOperation(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.qualityOperation._self') }))
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
  const res = await getQualityOperationTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQualityOperation(file, sheetName)
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
    const exportQuery: QualityOperationQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportQualityOperation(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.qualityOperation._self') }))
  } catch (error: any) {
    logger.error('[QualityOperation] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.qualityOperation._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: QualityOperation) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.qualityOperation._self'), name: t('common.tip.this.target', { target: t('entity.qualityOperation._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQualityOperationById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.qualityOperation._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.qualityOperation._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.qualityOperation._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteQualityOperationBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.qualityOperation._self') }))
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
  qualityOperationCode: '',
  operationMonth: '',
  customerName: '',
  debitNoteNo: '',
  recorder: '',
  totalQualityCost: undefined as number | undefined,
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
.logistics-quality-cost-quality-operation {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
