<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：IPQC制程检验单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-operation-ipqc-order">
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
      create-permission="logistics:quality:operation:ipqcorder:create"
      update-permission="logistics:quality:operation:ipqcorder:update"
      delete-permission="logistics:quality:operation:ipqcorder:delete"
      import-permission="logistics:quality:operation:ipqcorder:import"
      export-permission="logistics:quality:operation:ipqcorder:export"
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
      :id-column-key="'ipqcOrderId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getIpqcOrderId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.ipqcOrderItem._self') }}</div>
          <a-table
            v-if="hasIpqcOrderItemRows(record)"
            :columns="ipqcOrderItemExpandColumns"
            :data-source="getIpqcOrderItemRows(record)"
            :row-key="(row: IpqcOrderItem, index?: number) => row?.ipqcOrderItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.ipqcOrderChangeLog._self') }}</div>
          <a-table
            v-if="hasIpqcOrderChangeLogRows(record)"
            :columns="ipqcOrderChangeLogExpandColumns"
            :data-source="getIpqcOrderChangeLogRows(record)"
            :row-key="(row: IpqcOrderChangeLog, index?: number) => row?.ipqcOrderChangeLogId || String(index ?? 0)"
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
      <IpqcOrderForm
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
      :storage-key="'takt-query-fields-logistics-quality-operation-ipqc-order'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.ipqcOrder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCode')">
      <a-form-item :label="t('entity.ipqcOrder.sourcecode')">
        <a-input
          v-model:value="advancedQueryForm.sourceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.sourcecode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateStart')">
      <a-form-item :label="t('entity.ipqcOrder.inspectiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.inspectiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateEnd')">
      <a-form-item :label="t('entity.ipqcOrder.inspectiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.inspectiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ipqcOrderCode')">
      <a-form-item :label="t('entity.ipqcOrder.code')">
        <a-input
          v-model:value="advancedQueryForm.ipqcOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processCode')">
      <a-form-item :label="t('entity.ipqcOrder.processcode')">
        <a-input
          v-model:value="advancedQueryForm.processCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processName')">
      <a-form-item :label="t('entity.ipqcOrder.processname')">
        <a-input
          v-model:value="advancedQueryForm.processName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalProductionQuantity')">
      <a-form-item :label="t('entity.ipqcOrder.totalproductionquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalProductionQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalproductionquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalSampleQuantity')">
      <a-form-item :label="t('entity.ipqcOrder.totalsamplequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalSampleQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalsamplequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQualifiedQuantity')">
      <a-form-item :label="t('entity.ipqcOrder.totalqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalUnqualifiedQuantity')">
      <a-form-item :label="t('entity.ipqcOrder.totalunqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalUnqualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalunqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalInspectionReturnQuantity')">
      <a-form-item :label="t('entity.ipqcOrder.totalinspectionreturnquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalInspectionReturnQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalinspectionreturnquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeStatus')">
      <a-form-item :label="t('entity.ipqcOrder.judgestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.judgeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.judgestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeBy')">
      <a-form-item :label="t('entity.ipqcOrder.judgeby')">
        <a-input
          v-model:value="advancedQueryForm.judgeBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.judgeby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDateStart')">
      <a-form-item :label="t('entity.ipqcOrder.judgedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.judgeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.judgedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDateEnd')">
      <a-form-item :label="t('entity.ipqcOrder.judgedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.judgeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.judgedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDescription')">
      <a-form-item :label="t('entity.ipqcOrder.judgedescription')">
        <a-textarea
          v-model:value="advancedQueryForm.judgeDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcOrder.judgedescription') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.ipqcOrder._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ipqcOrder._self"
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
      :id-column-key="'ipqcOrderId'"
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
 * IPQC制程检验单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/ipqc-order
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import IpqcOrderForm from './components/ipqc-order-form.vue'
import { getIpqcOrderList, getIpqcOrderById, createIpqcOrder, updateIpqcOrder, deleteIpqcOrderById, deleteIpqcOrderBatch, getIpqcOrderTemplate, importIpqcOrder, exportIpqcOrder } from '@/api/logistics/quality/operation/ipqc-order'
import * as ipqcOrderItemApi from '@/api/logistics/quality/operation/ipqc-order-item'
import * as ipqcOrderChangeLogApi from '@/api/logistics/quality/operation/ipqc-order-change-log'
import type { IpqcOrderItem, IpqcOrderItemQuery } from '@/types/logistics/quality/operation/ipqc-order-item'
import type { IpqcOrderChangeLog, IpqcOrderChangeLogQuery } from '@/types/logistics/quality/operation/ipqc-order-change-log'
import type { IpqcOrder, IpqcOrderQuery, IpqcOrderCreate, IpqcOrderUpdate } from '@/types/logistics/quality/operation/ipqc-order'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktIpqcOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ipqcOrder._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<IpqcOrder[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<IpqcOrder | null>(null)
/** 表格多选行 */
const selectedRows = ref<IpqcOrder[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<IpqcOrder>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  sourceCode: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  ipqcOrderCode: '',
  processCode: '',
  processName: '',
  totalProductionQuantity: undefined as number | undefined,
  totalSampleQuantity: undefined as number | undefined,
  totalQualifiedQuantity: undefined as number | undefined,
  totalUnqualifiedQuantity: undefined as number | undefined,
  totalInspectionReturnQuantity: undefined as number | undefined,
  judgeStatus: undefined as number | undefined,
  judgeBy: '',
  judgeDateStart: '',
  judgeDateEnd: '',
  judgeDescription: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.ipqcOrder.plantcode') },
  { key: 'sourceCode', label: t('entity.ipqcOrder.sourcecode') },
  { key: 'inspectionDateStart', label: t('entity.ipqcOrder.inspectiondatestart') },
  { key: 'inspectionDateEnd', label: t('entity.ipqcOrder.inspectiondateend') },
  { key: 'ipqcOrderCode', label: t('entity.ipqcOrder.code') },
  { key: 'processCode', label: t('entity.ipqcOrder.processcode') },
  { key: 'processName', label: t('entity.ipqcOrder.processname') },
  { key: 'totalProductionQuantity', label: t('entity.ipqcOrder.totalproductionquantity') },
  { key: 'totalSampleQuantity', label: t('entity.ipqcOrder.totalsamplequantity') },
  { key: 'totalQualifiedQuantity', label: t('entity.ipqcOrder.totalqualifiedquantity') },
  { key: 'totalUnqualifiedQuantity', label: t('entity.ipqcOrder.totalunqualifiedquantity') },
  { key: 'totalInspectionReturnQuantity', label: t('entity.ipqcOrder.totalinspectionreturnquantity') },
  { key: 'judgeStatus', label: t('entity.ipqcOrder.judgestatus') },
  { key: 'judgeBy', label: t('entity.ipqcOrder.judgeby') },
  { key: 'judgeDateStart', label: t('entity.ipqcOrder.judgedatestart') },
  { key: 'judgeDateEnd', label: t('entity.ipqcOrder.judgedateend') },
  { key: 'judgeDescription', label: t('entity.ipqcOrder.judgedescription') },
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
const entityIdName = 'ipqcOrderId'
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

/** 展开行预览：ipqcOrderItem 列 */
const ipqcOrderItemExpandColumns = computed(() => [
  {
    title: t('entity.ipqcOrderItem.ipqcordername'),
    dataIndex: 'ipqcOrderName',
    key: 'ipqcOrderName',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderItem.ipqcordercode'),
    dataIndex: 'ipqcOrderCode',
    key: 'ipqcOrderCode',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderItem.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderItem.productionquantity'),
    dataIndex: 'productionQuantity',
    key: 'productionQuantity',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderItem.standardcode'),
    dataIndex: 'standardCode',
    key: 'standardCode',
    ellipsis: true,
  },
])

/** 展开行预览：ipqcOrderChangeLog 列 */
const ipqcOrderChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.ipqcOrderChangeLog.ipqcordername'),
    dataIndex: 'ipqcOrderName',
    key: 'ipqcOrderName',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    ellipsis: true,
  },
  {
    title: t('entity.ipqcOrderChangeLog.order'),
    dataIndex: 'order',
    key: 'order',
    ellipsis: true,
  },
])

/** 读取主表行上的 ipqcOrderItem 子表缓存 */
function getIpqcOrderItemRows(record: IpqcOrder): IpqcOrderItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 ipqcOrderItem 子表 */
function hasIpqcOrderItemRows(record: IpqcOrder): boolean {
  return getIpqcOrderItemRows(record).length > 0
}

/** 读取主表行上的 ipqcOrderChangeLog 子表缓存 */
function getIpqcOrderChangeLogRows(record: IpqcOrder): IpqcOrderChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 ipqcOrderChangeLog 子表 */
function hasIpqcOrderChangeLogRows(record: IpqcOrder): boolean {
  return getIpqcOrderChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadIpqcOrderDetail(record: IpqcOrder): Promise<IpqcOrder | null> {
  const id = getIpqcOrderId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getIpqcOrderById(id)
    const index = dataSource.value.findIndex((row) => getIpqcOrderId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as IpqcOrder
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 ipqcOrderItem 子表（IpqcOrderItemQuery + ipqcOrderItemApi，与主表 IpqcOrderQuery 分离） */
async function loadIpqcOrderItemForIpqcOrder(record: IpqcOrder): Promise<IpqcOrderItem[]> {
  const masterId = getIpqcOrderId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: IpqcOrderItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      ipqcOrderId: masterId,
    }
    const result = await ipqcOrderItemApi.getIpqcOrderItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getIpqcOrderId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as IpqcOrder
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 ipqcOrderChangeLog 子表（IpqcOrderChangeLogQuery + ipqcOrderChangeLogApi，与主表 IpqcOrderQuery 分离） */
async function loadIpqcOrderChangeLogForIpqcOrder(record: IpqcOrder): Promise<IpqcOrderChangeLog[]> {
  const masterId = getIpqcOrderId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: IpqcOrderChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      ipqcOrderId: masterId,
    }
    const result = await ipqcOrderChangeLogApi.getIpqcOrderChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getIpqcOrderId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as IpqcOrder
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureIpqcOrderChildrenLoaded(record: IpqcOrder) {
  if (!hasIpqcOrderItemRows(record)) {
    await loadIpqcOrderItemForIpqcOrder(record)
  }
  if (!hasIpqcOrderChangeLogRows(record)) {
    await loadIpqcOrderChangeLogForIpqcOrder(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: IpqcOrder) {
  const key = getIpqcOrderId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureIpqcOrderChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ipqcOrderId',
    key: 'ipqcOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'ipqcOrderId') ?? ''
  },
  {
    title: t('entity.ipqcOrder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.ipqcOrder.sourcecode'),
    dataIndex: 'sourceCode',
    key: 'sourceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'sourceCode') ?? ''
  },
  {
    title: t('entity.ipqcOrder.inspectiondate'),
    dataIndex: 'inspectionDate',
    key: 'inspectionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'inspectionDate') ?? ''
  },
  {
    title: t('entity.ipqcOrder.code'),
    dataIndex: 'ipqcOrderCode',
    key: 'ipqcOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'ipqcOrderCode') ?? ''
  },
  {
    title: t('entity.ipqcOrder.processcode'),
    dataIndex: 'processCode',
    key: 'processCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'processCode') ?? ''
  },
  {
    title: t('entity.ipqcOrder.processname'),
    dataIndex: 'processName',
    key: 'processName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'processName') ?? ''
  },
  {
    title: t('entity.ipqcOrder.totalproductionquantity'),
    dataIndex: 'totalProductionQuantity',
    key: 'totalProductionQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalProductionQuantity') ?? ''
  },
  {
    title: t('entity.ipqcOrder.totalsamplequantity'),
    dataIndex: 'totalSampleQuantity',
    key: 'totalSampleQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalSampleQuantity') ?? ''
  },
  {
    title: t('entity.ipqcOrder.totalqualifiedquantity'),
    dataIndex: 'totalQualifiedQuantity',
    key: 'totalQualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalQualifiedQuantity') ?? ''
  },
  {
    title: t('entity.ipqcOrder.totalunqualifiedquantity'),
    dataIndex: 'totalUnqualifiedQuantity',
    key: 'totalUnqualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalUnqualifiedQuantity') ?? ''
  },
  {
    title: t('entity.ipqcOrder.totalinspectionreturnquantity'),
    dataIndex: 'totalInspectionReturnQuantity',
    key: 'totalInspectionReturnQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalInspectionReturnQuantity') ?? ''
  },
  {
    title: t('entity.ipqcOrder.judgestatus'),
    dataIndex: 'judgeStatus',
    key: 'judgeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'judgeStatus') ?? ''
  },
  {
    title: t('entity.ipqcOrder.judgeby'),
    dataIndex: 'judgeBy',
    key: 'judgeBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'judgeBy') ?? ''
  },
  {
    title: t('entity.ipqcOrder.judgedate'),
    dataIndex: 'judgeDate',
    key: 'judgeDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'judgeDate') ?? ''
  },
  {
    title: t('entity.ipqcOrder.judgedescription'),
    dataIndex: 'judgeDescription',
    key: 'judgeDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'judgeDescription') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:ipqcorder:update',
        onClick: (record: IpqcOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:ipqcorder:delete',
        onClick: (record: IpqcOrder) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getIpqcOrderId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getIpqcOrderField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: IpqcOrder[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: IpqcOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getIpqcOrderId(selectedRow.value) === getIpqcOrderId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: IpqcOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: IpqcOrder) => ({
  onClick: () => {
    const key = getIpqcOrderId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getIpqcOrderId(item)))
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
    const params: IpqcOrderQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getIpqcOrderList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[IpqcOrder] 加载数据失败', { error })
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
  sourceCode: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  ipqcOrderCode: '',
  processCode: '',
  processName: '',
  totalProductionQuantity: undefined as number | undefined,
  totalSampleQuantity: undefined as number | undefined,
  totalQualifiedQuantity: undefined as number | undefined,
  totalUnqualifiedQuantity: undefined as number | undefined,
  totalInspectionReturnQuantity: undefined as number | undefined,
  judgeStatus: undefined as number | undefined,
  judgeBy: '',
  judgeDateStart: '',
  judgeDateEnd: '',
  judgeDescription: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ipqcOrder._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: IpqcOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ipqcOrder._self') })
  formLoading.value = true
  try {
    const detail = await loadIpqcOrderDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.ipqcOrder._self') }))
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
      await updateIpqcOrder(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.ipqcOrder._self') }))
    } else {
      await createIpqcOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.ipqcOrder._self') }))
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
  const res = await getIpqcOrderTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importIpqcOrder(file, sheetName)
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
    const exportQuery: IpqcOrderQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportIpqcOrder(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.ipqcOrder._self') }))
  } catch (error: any) {
    logger.error('[IpqcOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.ipqcOrder._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: IpqcOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.ipqcOrder._self'), name: t('common.tip.this.target', { target: t('entity.ipqcOrder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteIpqcOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcOrder._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.ipqcOrder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.ipqcOrder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteIpqcOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcOrder._self') }))
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
  sourceCode: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  ipqcOrderCode: '',
  processCode: '',
  processName: '',
  totalProductionQuantity: undefined as number | undefined,
  totalSampleQuantity: undefined as number | undefined,
  totalQualifiedQuantity: undefined as number | undefined,
  totalUnqualifiedQuantity: undefined as number | undefined,
  totalInspectionReturnQuantity: undefined as number | undefined,
  judgeStatus: undefined as number | undefined,
  judgeBy: '',
  judgeDateStart: '',
  judgeDateEnd: '',
  judgeDescription: '',
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
.logistics-quality-operation-ipqc-order {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
