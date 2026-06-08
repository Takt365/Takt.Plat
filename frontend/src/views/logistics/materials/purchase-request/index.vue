<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchase-request -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt采购申请实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-materials-purchase-request">
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
      create-permission="logistics:materials:purchaserequest:create"
      update-permission="logistics:materials:purchaserequest:update"
      delete-permission="logistics:materials:purchaserequest:delete"
      import-permission="logistics:materials:purchaserequest:import"
      export-permission="logistics:materials:purchaserequest:export"
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
      entity-scope="approval"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'purchaseRequestId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPurchaseRequestId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'requestStatus'">
          <TaktDictTag
            :value="getPurchaseRequestField(record, 'requestStatus')"
            dict-type="sys_normal_disable"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.purchaseRequestItem._self') }}</div>
          <a-table
            v-if="hasPurchaseRequestItemRows(record)"
            :columns="purchaseRequestItemExpandColumns"
            :data-source="getPurchaseRequestItemRows(record)"
            :row-key="(row: PurchaseRequestItem, index?: number) => row?.purchaseRequestItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.purchaseRequestChangeLog._self') }}</div>
          <a-table
            v-if="hasPurchaseRequestChangeLogRows(record)"
            :columns="purchaseRequestChangeLogExpandColumns"
            :data-source="getPurchaseRequestChangeLogRows(record)"
            :row-key="(row: PurchaseRequestChangeLog, index?: number) => row?.purchaseRequestChangeLogId || String(index ?? 0)"
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
      <PurchaseRequestForm
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
      :storage-key="'takt-query-fields-logistics-materials-purchase-request'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.purchaseRequest.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseRequestCode')">
      <a-form-item :label="t('entity.purchaseRequest.code')">
        <a-input
          v-model:value="advancedQueryForm.purchaseRequestCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestDateStart')">
      <a-form-item :label="t('entity.purchaseRequest.requestdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.requestDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.requestdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestDateEnd')">
      <a-form-item :label="t('entity.purchaseRequest.requestdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.requestDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.requestdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredArrivalDateStart')">
      <a-form-item :label="t('entity.purchaseRequest.requiredarrivaldatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredArrivalDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.requiredarrivaldatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredArrivalDateEnd')">
      <a-form-item :label="t('entity.purchaseRequest.requiredarrivaldateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredArrivalDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.requiredarrivaldateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestId')">
      <a-form-item :label="t('entity.purchaseRequest.requestid')">
        <a-input
          v-model:value="advancedQueryForm.requestId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.requestid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestBy')">
      <a-form-item :label="t('entity.purchaseRequest.requestby')">
        <a-input
          v-model:value="advancedQueryForm.requestBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.requestby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQuantity')">
      <a-form-item :label="t('entity.purchaseRequest.totalquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.totalquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="t('entity.purchaseRequest.totalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.totalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedQuantity')">
      <a-form-item :label="t('entity.purchaseRequest.convertedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.convertedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedAmount')">
      <a-form-item :label="t('entity.purchaseRequest.convertedamount')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.convertedamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestStatus')">
      <a-form-item :label="t('entity.purchaseRequest.requeststatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.requestStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.requeststatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedStatus')">
      <a-form-item :label="t('entity.purchaseRequest.convertedstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.convertedstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.purchaseRequest.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.flowinstanceid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestReason')">
      <a-form-item :label="t('entity.purchaseRequest.requestreason')">
        <a-input
          v-model:value="advancedQueryForm.requestReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.requestreason') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.purchaseRequest.approvalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.approvalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.approvalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.purchaseRequest.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.initiatorid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.purchaseRequest.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.initiatedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.purchaseRequest.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.purchaseRequest.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.approvedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.purchaseRequest.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.approvedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.purchaseRequest.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.approvedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.purchaseRequest._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaseRequest._self"
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
      :id-column-key="'purchaseRequestId'"
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
 * Takt采购申请实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/purchase-request
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import PurchaseRequestForm from './components/purchase-request-form.vue'
import { getPurchaseRequestList, getPurchaseRequestById, createPurchaseRequest, updatePurchaseRequest, deletePurchaseRequestById, deletePurchaseRequestBatch, getPurchaseRequestTemplate, importPurchaseRequest, exportPurchaseRequest } from '@/api/logistics/materials/purchase-request'
import * as purchaseRequestItemApi from '@/api/logistics/materials/purchase-request-item'
import * as purchaseRequestChangeLogApi from '@/api/logistics/materials/purchase-request-change-log'
import type { PurchaseRequestItem, PurchaseRequestItemQuery } from '@/types/logistics/materials/purchase-request-item'
import type { PurchaseRequestChangeLog, PurchaseRequestChangeLogQuery } from '@/types/logistics/materials/purchase-request-change-log'
import type { PurchaseRequest, PurchaseRequestQuery, PurchaseRequestCreate, PurchaseRequestUpdate } from '@/types/logistics/materials/purchase-request'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchaseRequest')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaseRequest._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PurchaseRequest[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<PurchaseRequest | null>(null)
/** 表格多选行 */
const selectedRows = ref<PurchaseRequest[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PurchaseRequest>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  purchaseRequestCode: '',
  requestDateStart: '',
  requestDateEnd: '',
  requiredArrivalDateStart: '',
  requiredArrivalDateEnd: '',
  requestId: '',
  requestBy: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  convertedQuantity: undefined as number | undefined,
  convertedAmount: undefined as number | undefined,
  requestStatus: undefined as number | undefined,
  convertedStatus: undefined as number | undefined,
  flowInstanceId: '',
  requestReason: '',
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
  { key: 'plantCode', label: t('entity.purchaseRequest.plantcode') },
  { key: 'purchaseRequestCode', label: t('entity.purchaseRequest.code') },
  { key: 'requestDateStart', label: t('entity.purchaseRequest.requestdatestart') },
  { key: 'requestDateEnd', label: t('entity.purchaseRequest.requestdateend') },
  { key: 'requiredArrivalDateStart', label: t('entity.purchaseRequest.requiredarrivaldatestart') },
  { key: 'requiredArrivalDateEnd', label: t('entity.purchaseRequest.requiredarrivaldateend') },
  { key: 'requestId', label: t('entity.purchaseRequest.requestid') },
  { key: 'requestBy', label: t('entity.purchaseRequest.requestby') },
  { key: 'totalQuantity', label: t('entity.purchaseRequest.totalquantity') },
  { key: 'totalAmount', label: t('entity.purchaseRequest.totalamount') },
  { key: 'convertedQuantity', label: t('entity.purchaseRequest.convertedquantity') },
  { key: 'convertedAmount', label: t('entity.purchaseRequest.convertedamount') },
  { key: 'requestStatus', label: t('entity.purchaseRequest.requeststatus') },
  { key: 'convertedStatus', label: t('entity.purchaseRequest.convertedstatus') },
  { key: 'flowInstanceId', label: t('entity.purchaseRequest.flowinstanceid') },
  { key: 'requestReason', label: t('entity.purchaseRequest.requestreason') },
  { key: 'approvalStatus', label: t('entity.purchaseRequest.approvalstatus') },
  { key: 'initiatorId', label: t('entity.purchaseRequest.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.purchaseRequest.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.purchaseRequest.initiatedatend') },
  { key: 'approvedBy', label: t('entity.purchaseRequest.approvedby') },
  { key: 'approvedAtStart', label: t('entity.purchaseRequest.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.purchaseRequest.approvedatend') },
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
const entityIdName = 'purchaseRequestId'
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

/** 展开行预览：purchaseRequestItem 列 */
const purchaseRequestItemExpandColumns = computed(() => [
  {
    title: t('entity.purchaseRequestItem.purchaserequestname'),
    dataIndex: 'purchaseRequestName',
    key: 'purchaseRequestName',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestItem.purchaserequestcode'),
    dataIndex: 'purchaseRequestCode',
    key: 'purchaseRequestCode',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestItem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestItem.requestunit'),
    dataIndex: 'requestUnit',
    key: 'requestUnit',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestItem.requestquantity'),
    dataIndex: 'requestQuantity',
    key: 'requestQuantity',
    ellipsis: true,
  },
])

/** 展开行预览：purchaseRequestChangeLog 列 */
const purchaseRequestChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.purchaseRequestChangeLog.purchaserequestname'),
    dataIndex: 'purchaseRequestName',
    key: 'purchaseRequestName',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestChangeLog.requestcode'),
    dataIndex: 'requestCode',
    key: 'requestCode',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequestChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
])

/** 读取主表行上的 purchaseRequestItem 子表缓存 */
function getPurchaseRequestItemRows(record: PurchaseRequest): PurchaseRequestItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 purchaseRequestItem 子表 */
function hasPurchaseRequestItemRows(record: PurchaseRequest): boolean {
  return getPurchaseRequestItemRows(record).length > 0
}

/** 读取主表行上的 purchaseRequestChangeLog 子表缓存 */
function getPurchaseRequestChangeLogRows(record: PurchaseRequest): PurchaseRequestChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 purchaseRequestChangeLog 子表 */
function hasPurchaseRequestChangeLogRows(record: PurchaseRequest): boolean {
  return getPurchaseRequestChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadPurchaseRequestDetail(record: PurchaseRequest): Promise<PurchaseRequest | null> {
  const id = getPurchaseRequestId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getPurchaseRequestById(id)
    const index = dataSource.value.findIndex((row) => getPurchaseRequestId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as PurchaseRequest
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 purchaseRequestItem 子表（PurchaseRequestItemQuery + purchaseRequestItemApi，与主表 PurchaseRequestQuery 分离） */
async function loadPurchaseRequestItemForPurchaseRequest(record: PurchaseRequest): Promise<PurchaseRequestItem[]> {
  const masterId = getPurchaseRequestId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: PurchaseRequestItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      purchaseRequestId: masterId,
    }
    const result = await purchaseRequestItemApi.getPurchaseRequestItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getPurchaseRequestId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as PurchaseRequest
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 purchaseRequestChangeLog 子表（PurchaseRequestChangeLogQuery + purchaseRequestChangeLogApi，与主表 PurchaseRequestQuery 分离） */
async function loadPurchaseRequestChangeLogForPurchaseRequest(record: PurchaseRequest): Promise<PurchaseRequestChangeLog[]> {
  const masterId = getPurchaseRequestId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: PurchaseRequestChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      purchaseRequestId: masterId,
    }
    const result = await purchaseRequestChangeLogApi.getPurchaseRequestChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getPurchaseRequestId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as PurchaseRequest
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensurePurchaseRequestChildrenLoaded(record: PurchaseRequest) {
  if (!hasPurchaseRequestItemRows(record)) {
    await loadPurchaseRequestItemForPurchaseRequest(record)
  }
  if (!hasPurchaseRequestChangeLogRows(record)) {
    await loadPurchaseRequestChangeLogForPurchaseRequest(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: PurchaseRequest) {
  const key = getPurchaseRequestId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensurePurchaseRequestChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchaseRequestId',
    key: 'purchaseRequestId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'purchaseRequestId') ?? ''
  },
  {
    title: t('entity.purchaseRequest.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.purchaseRequest.code'),
    dataIndex: 'purchaseRequestCode',
    key: 'purchaseRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'purchaseRequestCode') ?? ''
  },
  {
    title: t('entity.purchaseRequest.requestdate'),
    dataIndex: 'requestDate',
    key: 'requestDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'requestDate') ?? ''
  },
  {
    title: t('entity.purchaseRequest.requiredarrivaldate'),
    dataIndex: 'requiredArrivalDate',
    key: 'requiredArrivalDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'requiredArrivalDate') ?? ''
  },
  {
    title: t('entity.purchaseRequest.requestid'),
    dataIndex: 'requestId',
    key: 'requestId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'requestId') ?? ''
  },
  {
    title: t('entity.purchaseRequest.requestname'),
    dataIndex: 'requestName',
    key: 'requestName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'requestName') ?? ''
  },
  {
    title: t('entity.purchaseRequest.requestby'),
    dataIndex: 'requestBy',
    key: 'requestBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'requestBy') ?? ''
  },
  {
    title: t('entity.purchaseRequest.totalquantity'),
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'totalQuantity') ?? ''
  },
  {
    title: t('entity.purchaseRequest.totalamount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'totalAmount') ?? ''
  },
  {
    title: t('entity.purchaseRequest.convertedquantity'),
    dataIndex: 'convertedQuantity',
    key: 'convertedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'convertedQuantity') ?? ''
  },
  {
    title: t('entity.purchaseRequest.convertedamount'),
    dataIndex: 'convertedAmount',
    key: 'convertedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'convertedAmount') ?? ''
  },
  {
    title: t('entity.purchaseRequest.requeststatus'),
    dataIndex: 'requestStatus',
    key: 'requestStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.purchaseRequest.convertedstatus'),
    dataIndex: 'convertedStatus',
    key: 'convertedStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'convertedStatus') ?? ''
  },
  {
    title: t('entity.purchaseRequest.flowinstanceid'),
    dataIndex: 'flowInstanceId',
    key: 'flowInstanceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'flowInstanceId') ?? ''
  },
  {
    title: t('entity.purchaseRequest.flowinstancename'),
    dataIndex: 'flowInstanceName',
    key: 'flowInstanceName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'flowInstanceName') ?? ''
  },
  {
    title: t('entity.purchaseRequest.requestreason'),
    dataIndex: 'requestReason',
    key: 'requestReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseRequestField(record, 'requestReason') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:purchaserequest:update',
        onClick: (record: PurchaseRequest) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:purchaserequest:delete',
        onClick: (record: PurchaseRequest) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPurchaseRequestId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPurchaseRequestField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchaseRequest[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchaseRequest, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPurchaseRequestId(selectedRow.value) === getPurchaseRequestId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseRequest[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: PurchaseRequest) => ({
  onClick: () => {
    const key = getPurchaseRequestId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPurchaseRequestId(item)))
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
    const params: PurchaseRequestQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPurchaseRequestList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PurchaseRequest] 加载数据失败', { error })
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
  purchaseRequestCode: '',
  requestDateStart: '',
  requestDateEnd: '',
  requiredArrivalDateStart: '',
  requiredArrivalDateEnd: '',
  requestId: '',
  requestBy: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  convertedQuantity: undefined as number | undefined,
  convertedAmount: undefined as number | undefined,
  requestStatus: undefined as number | undefined,
  convertedStatus: undefined as number | undefined,
  flowInstanceId: '',
  requestReason: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.purchaseRequest._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: PurchaseRequest) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.purchaseRequest._self') })
  formLoading.value = true
  try {
    const detail = await loadPurchaseRequestDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.purchaseRequest._self') }))
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
      await updatePurchaseRequest(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.purchaseRequest._self') }))
    } else {
      await createPurchaseRequest(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.purchaseRequest._self') }))
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
  const res = await getPurchaseRequestTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchaseRequest(file, sheetName)
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
    const exportQuery: PurchaseRequestQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPurchaseRequest(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchaseRequest._self') }))
  } catch (error: any) {
    logger.error('[PurchaseRequest] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.purchaseRequest._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PurchaseRequest) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.purchaseRequest._self'), name: t('common.tip.this.target', { target: t('entity.purchaseRequest._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseRequestById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseRequest._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.purchaseRequest._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.purchaseRequest._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePurchaseRequestBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseRequest._self') }))
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
  purchaseRequestCode: '',
  requestDateStart: '',
  requestDateEnd: '',
  requiredArrivalDateStart: '',
  requiredArrivalDateEnd: '',
  requestId: '',
  requestBy: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  convertedQuantity: undefined as number | undefined,
  convertedAmount: undefined as number | undefined,
  requestStatus: undefined as number | undefined,
  convertedStatus: undefined as number | undefined,
  flowInstanceId: '',
  requestReason: '',
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
.logistics-materials-purchase-request {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
