<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/fqc-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：FQC出货检验单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-operation-fqc-order">
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
      create-permission="logistics:quality:operation:fqcorder:create"
      update-permission="logistics:quality:operation:fqcorder:update"
      delete-permission="logistics:quality:operation:fqcorder:delete"
      import-permission="logistics:quality:operation:fqcorder:import"
      export-permission="logistics:quality:operation:fqcorder:export"
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
      :id-column-key="'fqcOrderId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getFqcOrderId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.fqcOrderItem._self') }}</div>
          <a-table
            v-if="hasFqcOrderItemRows(record)"
            :columns="fqcOrderItemExpandColumns"
            :data-source="getFqcOrderItemRows(record)"
            :row-key="(row: FqcOrderItem, index?: number) => row?.fqcOrderItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.fqcOrderChangeLog._self') }}</div>
          <a-table
            v-if="hasFqcOrderChangeLogRows(record)"
            :columns="fqcOrderChangeLogExpandColumns"
            :data-source="getFqcOrderChangeLogRows(record)"
            :row-key="(row: FqcOrderChangeLog, index?: number) => row?.fqcOrderChangeLogId || String(index ?? 0)"
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
      <FqcOrderForm
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
      :storage-key="'takt-query-fields-logistics-quality-operation-fqc-order'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.fqcOrder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCode')">
      <a-form-item :label="t('entity.fqcOrder.sourcecode')">
        <a-input
          v-model:value="advancedQueryForm.sourceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.sourcecode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateStart')">
      <a-form-item :label="t('entity.fqcOrder.inspectiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.fqcOrder.inspectiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateEnd')">
      <a-form-item :label="t('entity.fqcOrder.inspectiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.fqcOrder.inspectiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fqcOrderCode')">
      <a-form-item :label="t('entity.fqcOrder.code')">
        <a-input
          v-model:value="advancedQueryForm.fqcOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="t('entity.fqcOrder.customercode')">
        <a-input
          v-model:value="advancedQueryForm.customerCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.customercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalWarehouseQuantity')">
      <a-form-item :label="t('entity.fqcOrder.totalwarehousequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalWarehouseQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.totalwarehousequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalSampleQuantity')">
      <a-form-item :label="t('entity.fqcOrder.totalsamplequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalSampleQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.totalsamplequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQualifiedQuantity')">
      <a-form-item :label="t('entity.fqcOrder.totalqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.totalqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalUnqualifiedQuantity')">
      <a-form-item :label="t('entity.fqcOrder.totalunqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalUnqualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.totalunqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalInspectionReturnQuantity')">
      <a-form-item :label="t('entity.fqcOrder.totalinspectionreturnquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalInspectionReturnQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.totalinspectionreturnquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeStatus')">
      <a-form-item :label="t('entity.fqcOrder.judgestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.judgeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.judgestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeBy')">
      <a-form-item :label="t('entity.fqcOrder.judgeby')">
        <a-input
          v-model:value="advancedQueryForm.judgeBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcOrder.judgeby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDateStart')">
      <a-form-item :label="t('entity.fqcOrder.judgedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.judgeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.fqcOrder.judgedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDateEnd')">
      <a-form-item :label="t('entity.fqcOrder.judgedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.judgeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.fqcOrder.judgedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDescription')">
      <a-form-item :label="t('entity.fqcOrder.judgedescription')">
        <a-textarea
          v-model:value="advancedQueryForm.judgeDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.fqcOrder.judgedescription') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.fqcOrder._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.fqcOrder._self"
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
      :id-column-key="'fqcOrderId'"
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
 * FQC出货检验单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/fqc-order
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import FqcOrderForm from './components/fqc-order-form.vue'
import { getFqcOrderList, getFqcOrderById, createFqcOrder, updateFqcOrder, deleteFqcOrderById, deleteFqcOrderBatch, getFqcOrderTemplate, importFqcOrder, exportFqcOrder } from '@/api/logistics/quality/operation/fqc-order'
import * as fqcOrderItemApi from '@/api/logistics/quality/operation/fqc-order-item'
import * as fqcOrderChangeLogApi from '@/api/logistics/quality/operation/fqc-order-change-log'
import type { FqcOrderItem, FqcOrderItemQuery } from '@/types/logistics/quality/operation/fqc-order-item'
import type { FqcOrderChangeLog, FqcOrderChangeLogQuery } from '@/types/logistics/quality/operation/fqc-order-change-log'
import type { FqcOrder, FqcOrderQuery, FqcOrderCreate, FqcOrderUpdate } from '@/types/logistics/quality/operation/fqc-order'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktFqcOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.fqcOrder._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<FqcOrder[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<FqcOrder | null>(null)
/** 表格多选行 */
const selectedRows = ref<FqcOrder[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<FqcOrder>>({})
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
  fqcOrderCode: '',
  customerCode: '',
  totalWarehouseQuantity: undefined as number | undefined,
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
  { key: 'plantCode', label: t('entity.fqcOrder.plantcode') },
  { key: 'sourceCode', label: t('entity.fqcOrder.sourcecode') },
  { key: 'inspectionDateStart', label: t('entity.fqcOrder.inspectiondatestart') },
  { key: 'inspectionDateEnd', label: t('entity.fqcOrder.inspectiondateend') },
  { key: 'fqcOrderCode', label: t('entity.fqcOrder.code') },
  { key: 'customerCode', label: t('entity.fqcOrder.customercode') },
  { key: 'totalWarehouseQuantity', label: t('entity.fqcOrder.totalwarehousequantity') },
  { key: 'totalSampleQuantity', label: t('entity.fqcOrder.totalsamplequantity') },
  { key: 'totalQualifiedQuantity', label: t('entity.fqcOrder.totalqualifiedquantity') },
  { key: 'totalUnqualifiedQuantity', label: t('entity.fqcOrder.totalunqualifiedquantity') },
  { key: 'totalInspectionReturnQuantity', label: t('entity.fqcOrder.totalinspectionreturnquantity') },
  { key: 'judgeStatus', label: t('entity.fqcOrder.judgestatus') },
  { key: 'judgeBy', label: t('entity.fqcOrder.judgeby') },
  { key: 'judgeDateStart', label: t('entity.fqcOrder.judgedatestart') },
  { key: 'judgeDateEnd', label: t('entity.fqcOrder.judgedateend') },
  { key: 'judgeDescription', label: t('entity.fqcOrder.judgedescription') },
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
const entityIdName = 'fqcOrderId'
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

/** 展开行预览：fqcOrderItem 列 */
const fqcOrderItemExpandColumns = computed(() => [
  {
    title: t('entity.fqcOrderItem.fqcordername'),
    dataIndex: 'fqcOrderName',
    key: 'fqcOrderName',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderItem.fqcordercode'),
    dataIndex: 'fqcOrderCode',
    key: 'fqcOrderCode',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderItem.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderItem.warehousequantity'),
    dataIndex: 'warehouseQuantity',
    key: 'warehouseQuantity',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderItem.standardcode'),
    dataIndex: 'standardCode',
    key: 'standardCode',
    ellipsis: true,
  },
])

/** 展开行预览：fqcOrderChangeLog 列 */
const fqcOrderChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.fqcOrderChangeLog.fqcordername'),
    dataIndex: 'fqcOrderName',
    key: 'fqcOrderName',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    ellipsis: true,
  },
  {
    title: t('entity.fqcOrderChangeLog.order'),
    dataIndex: 'order',
    key: 'order',
    ellipsis: true,
  },
])

/** 读取主表行上的 fqcOrderItem 子表缓存 */
function getFqcOrderItemRows(record: FqcOrder): FqcOrderItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 fqcOrderItem 子表 */
function hasFqcOrderItemRows(record: FqcOrder): boolean {
  return getFqcOrderItemRows(record).length > 0
}

/** 读取主表行上的 fqcOrderChangeLog 子表缓存 */
function getFqcOrderChangeLogRows(record: FqcOrder): FqcOrderChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 fqcOrderChangeLog 子表 */
function hasFqcOrderChangeLogRows(record: FqcOrder): boolean {
  return getFqcOrderChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadFqcOrderDetail(record: FqcOrder): Promise<FqcOrder | null> {
  const id = getFqcOrderId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getFqcOrderById(id)
    const index = dataSource.value.findIndex((row) => getFqcOrderId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as FqcOrder
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 fqcOrderItem 子表（FqcOrderItemQuery + fqcOrderItemApi，与主表 FqcOrderQuery 分离） */
async function loadFqcOrderItemForFqcOrder(record: FqcOrder): Promise<FqcOrderItem[]> {
  const masterId = getFqcOrderId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: FqcOrderItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      fqcOrderId: masterId,
    }
    const result = await fqcOrderItemApi.getFqcOrderItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getFqcOrderId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as FqcOrder
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 fqcOrderChangeLog 子表（FqcOrderChangeLogQuery + fqcOrderChangeLogApi，与主表 FqcOrderQuery 分离） */
async function loadFqcOrderChangeLogForFqcOrder(record: FqcOrder): Promise<FqcOrderChangeLog[]> {
  const masterId = getFqcOrderId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: FqcOrderChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      fqcOrderId: masterId,
    }
    const result = await fqcOrderChangeLogApi.getFqcOrderChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getFqcOrderId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as FqcOrder
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureFqcOrderChildrenLoaded(record: FqcOrder) {
  if (!hasFqcOrderItemRows(record)) {
    await loadFqcOrderItemForFqcOrder(record)
  }
  if (!hasFqcOrderChangeLogRows(record)) {
    await loadFqcOrderChangeLogForFqcOrder(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: FqcOrder) {
  const key = getFqcOrderId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureFqcOrderChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'fqcOrderId',
    key: 'fqcOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'fqcOrderId') ?? ''
  },
  {
    title: t('entity.fqcOrder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.fqcOrder.sourcecode'),
    dataIndex: 'sourceCode',
    key: 'sourceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'sourceCode') ?? ''
  },
  {
    title: t('entity.fqcOrder.inspectiondate'),
    dataIndex: 'inspectionDate',
    key: 'inspectionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'inspectionDate') ?? ''
  },
  {
    title: t('entity.fqcOrder.code'),
    dataIndex: 'fqcOrderCode',
    key: 'fqcOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'fqcOrderCode') ?? ''
  },
  {
    title: t('entity.fqcOrder.customercode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'customerCode') ?? ''
  },
  {
    title: t('entity.fqcOrder.totalwarehousequantity'),
    dataIndex: 'totalWarehouseQuantity',
    key: 'totalWarehouseQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'totalWarehouseQuantity') ?? ''
  },
  {
    title: t('entity.fqcOrder.totalsamplequantity'),
    dataIndex: 'totalSampleQuantity',
    key: 'totalSampleQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'totalSampleQuantity') ?? ''
  },
  {
    title: t('entity.fqcOrder.totalqualifiedquantity'),
    dataIndex: 'totalQualifiedQuantity',
    key: 'totalQualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'totalQualifiedQuantity') ?? ''
  },
  {
    title: t('entity.fqcOrder.totalunqualifiedquantity'),
    dataIndex: 'totalUnqualifiedQuantity',
    key: 'totalUnqualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'totalUnqualifiedQuantity') ?? ''
  },
  {
    title: t('entity.fqcOrder.totalinspectionreturnquantity'),
    dataIndex: 'totalInspectionReturnQuantity',
    key: 'totalInspectionReturnQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'totalInspectionReturnQuantity') ?? ''
  },
  {
    title: t('entity.fqcOrder.judgestatus'),
    dataIndex: 'judgeStatus',
    key: 'judgeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'judgeStatus') ?? ''
  },
  {
    title: t('entity.fqcOrder.judgeby'),
    dataIndex: 'judgeBy',
    key: 'judgeBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'judgeBy') ?? ''
  },
  {
    title: t('entity.fqcOrder.judgedate'),
    dataIndex: 'judgeDate',
    key: 'judgeDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'judgeDate') ?? ''
  },
  {
    title: t('entity.fqcOrder.judgedescription'),
    dataIndex: 'judgeDescription',
    key: 'judgeDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getFqcOrderField(record, 'judgeDescription') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:fqcorder:update',
        onClick: (record: FqcOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:fqcorder:delete',
        onClick: (record: FqcOrder) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getFqcOrderId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getFqcOrderField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: FqcOrder[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: FqcOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getFqcOrderId(selectedRow.value) === getFqcOrderId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: FqcOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: FqcOrder) => ({
  onClick: () => {
    const key = getFqcOrderId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getFqcOrderId(item)))
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
    const params: FqcOrderQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getFqcOrderList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[FqcOrder] 加载数据失败', { error })
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
  sourceCode: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  fqcOrderCode: '',
  customerCode: '',
  totalWarehouseQuantity: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.fqcOrder._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: FqcOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.fqcOrder._self') })
  formLoading.value = true
  try {
    const detail = await loadFqcOrderDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.fqcOrder._self') }))
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
      await updateFqcOrder(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.fqcOrder._self') }))
    } else {
      await createFqcOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.fqcOrder._self') }))
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
  const res = await getFqcOrderTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importFqcOrder(file, sheetName)
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
    const exportQuery: FqcOrderQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportFqcOrder(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.fqcOrder._self') }))
  } catch (error: any) {
    logger.error('[FqcOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.fqcOrder._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: FqcOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.fqcOrder._self'), name: t('common.tip.this.target', { target: t('entity.fqcOrder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteFqcOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.fqcOrder._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.fqcOrder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.fqcOrder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteFqcOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.fqcOrder._self') }))
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
  fqcOrderCode: '',
  customerCode: '',
  totalWarehouseQuantity: undefined as number | undefined,
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
.logistics-quality-operation-fqc-order {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
