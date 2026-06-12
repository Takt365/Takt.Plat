<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/iqc-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：IQC进货检验单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-operation-iqc-order">
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
      create-permission="logistics:quality:operation:iqcorder:create"
      update-permission="logistics:quality:operation:iqcorder:update"
      delete-permission="logistics:quality:operation:iqcorder:delete"
      import-permission="logistics:quality:operation:iqcorder:import"
      export-permission="logistics:quality:operation:iqcorder:export"
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
      :id-column-key="'iqcOrderId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getIqcOrderId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.iqcOrderItem._self') }}</div>
          <a-table
            v-if="hasIqcOrderItemRows(record)"
            :columns="iqcOrderItemExpandColumns"
            :data-source="getIqcOrderItemRows(record)"
            :row-key="(row: IqcOrderItem, index?: number) => row?.iqcOrderItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.iqcOrderChangeLog._self') }}</div>
          <a-table
            v-if="hasIqcOrderChangeLogRows(record)"
            :columns="iqcOrderChangeLogExpandColumns"
            :data-source="getIqcOrderChangeLogRows(record)"
            :row-key="(row: IqcOrderChangeLog, index?: number) => row?.iqcOrderChangeLogId || String(index ?? 0)"
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
      <IqcOrderForm
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
      :storage-key="'takt-query-fields-logistics-quality-operation-iqc-order'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.iqcOrder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCode')">
      <a-form-item :label="t('entity.iqcOrder.sourcecode')">
        <a-input
          v-model:value="advancedQueryForm.sourceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.sourcecode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateStart')">
      <a-form-item :label="t('entity.iqcOrder.inspectiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcOrder.inspectiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateEnd')">
      <a-form-item :label="t('entity.iqcOrder.inspectiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcOrder.inspectiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('iqcOrderCode')">
      <a-form-item :label="t('entity.iqcOrder.code')">
        <a-input
          v-model:value="advancedQueryForm.iqcOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="t('entity.iqcOrder.suppliercode')">
        <a-input
          v-model:value="advancedQueryForm.supplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.suppliercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalPurchaseQuantity')">
      <a-form-item :label="t('entity.iqcOrder.totalpurchasequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalPurchaseQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.totalpurchasequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalSampleQuantity')">
      <a-form-item :label="t('entity.iqcOrder.totalsamplequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalSampleQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.totalsamplequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQualifiedQuantity')">
      <a-form-item :label="t('entity.iqcOrder.totalqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.totalqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalUnqualifiedQuantity')">
      <a-form-item :label="t('entity.iqcOrder.totalunqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalUnqualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.totalunqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalInspectionReturnQuantity')">
      <a-form-item :label="t('entity.iqcOrder.totalinspectionreturnquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalInspectionReturnQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.totalinspectionreturnquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeStatus')">
      <a-form-item :label="t('entity.iqcOrder.judgestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.judgeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.judgestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeBy')">
      <a-form-item :label="t('entity.iqcOrder.judgeby')">
        <a-input
          v-model:value="advancedQueryForm.judgeBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrder.judgeby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDateStart')">
      <a-form-item :label="t('entity.iqcOrder.judgedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.judgeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcOrder.judgedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDateEnd')">
      <a-form-item :label="t('entity.iqcOrder.judgedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.judgeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcOrder.judgedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDescription')">
      <a-form-item :label="t('entity.iqcOrder.judgedescription')">
        <a-textarea
          v-model:value="advancedQueryForm.judgeDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.iqcOrder.judgedescription') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.iqcOrder._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.iqcOrder._self"
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
      :id-column-key="'iqcOrderId'"
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
 * IQC进货检验单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/iqc-order
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import IqcOrderForm from './components/iqc-order-form.vue'
import { getIqcOrderList, getIqcOrderById, createIqcOrder, updateIqcOrder, deleteIqcOrderById, deleteIqcOrderBatch, getIqcOrderTemplate, importIqcOrder, exportIqcOrder } from '@/api/logistics/quality/operation/iqc-order'
import * as iqcOrderItemApi from '@/api/logistics/quality/operation/iqc-order-item'
import * as iqcOrderChangeLogApi from '@/api/logistics/quality/operation/iqc-order-change-log'
import type { IqcOrderItem, IqcOrderItemQuery } from '@/types/logistics/quality/operation/iqc-order-item'
import type { IqcOrderChangeLog, IqcOrderChangeLogQuery } from '@/types/logistics/quality/operation/iqc-order-change-log'
import type { IqcOrder, IqcOrderQuery, IqcOrderCreate, IqcOrderUpdate } from '@/types/logistics/quality/operation/iqc-order'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktIqcOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.iqcOrder._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<IqcOrder[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<IqcOrder | null>(null)
/** 表格多选行 */
const selectedRows = ref<IqcOrder[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<IqcOrder>>({})
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
  iqcOrderCode: '',
  supplierCode: '',
  totalPurchaseQuantity: undefined as number | undefined,
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
  { key: 'plantCode', label: t('entity.iqcOrder.plantcode') },
  { key: 'sourceCode', label: t('entity.iqcOrder.sourcecode') },
  { key: 'inspectionDateStart', label: t('entity.iqcOrder.inspectiondatestart') },
  { key: 'inspectionDateEnd', label: t('entity.iqcOrder.inspectiondateend') },
  { key: 'iqcOrderCode', label: t('entity.iqcOrder.code') },
  { key: 'supplierCode', label: t('entity.iqcOrder.suppliercode') },
  { key: 'totalPurchaseQuantity', label: t('entity.iqcOrder.totalpurchasequantity') },
  { key: 'totalSampleQuantity', label: t('entity.iqcOrder.totalsamplequantity') },
  { key: 'totalQualifiedQuantity', label: t('entity.iqcOrder.totalqualifiedquantity') },
  { key: 'totalUnqualifiedQuantity', label: t('entity.iqcOrder.totalunqualifiedquantity') },
  { key: 'totalInspectionReturnQuantity', label: t('entity.iqcOrder.totalinspectionreturnquantity') },
  { key: 'judgeStatus', label: t('entity.iqcOrder.judgestatus') },
  { key: 'judgeBy', label: t('entity.iqcOrder.judgeby') },
  { key: 'judgeDateStart', label: t('entity.iqcOrder.judgedatestart') },
  { key: 'judgeDateEnd', label: t('entity.iqcOrder.judgedateend') },
  { key: 'judgeDescription', label: t('entity.iqcOrder.judgedescription') },
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
const entityIdName = 'iqcOrderId'
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

/** 展开行预览：iqcOrderItem 列 */
const iqcOrderItemExpandColumns = computed(() => [
  {
    title: t('entity.iqcOrderItem.iqcordername'),
    dataIndex: 'iqcOrderName',
    key: 'iqcOrderName',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderItem.iqcordercode'),
    dataIndex: 'iqcOrderCode',
    key: 'iqcOrderCode',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderItem.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderItem.purchasequantity'),
    dataIndex: 'purchaseQuantity',
    key: 'purchaseQuantity',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderItem.standardcode'),
    dataIndex: 'standardCode',
    key: 'standardCode',
    ellipsis: true,
  },
])

/** 展开行预览：iqcOrderChangeLog 列 */
const iqcOrderChangeLogExpandColumns = computed(() => [
  {
    title: t('entity.iqcOrderChangeLog.iqcordername'),
    dataIndex: 'iqcOrderName',
    key: 'iqcOrderName',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    ellipsis: true,
  },
  {
    title: t('entity.iqcOrderChangeLog.order'),
    dataIndex: 'order',
    key: 'order',
    ellipsis: true,
  },
])

/** 读取主表行上的 iqcOrderItem 子表缓存 */
function getIqcOrderItemRows(record: IqcOrder): IqcOrderItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 iqcOrderItem 子表 */
function hasIqcOrderItemRows(record: IqcOrder): boolean {
  return getIqcOrderItemRows(record).length > 0
}

/** 读取主表行上的 iqcOrderChangeLog 子表缓存 */
function getIqcOrderChangeLogRows(record: IqcOrder): IqcOrderChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 iqcOrderChangeLog 子表 */
function hasIqcOrderChangeLogRows(record: IqcOrder): boolean {
  return getIqcOrderChangeLogRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadIqcOrderDetail(record: IqcOrder): Promise<IqcOrder | null> {
  const id = getIqcOrderId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getIqcOrderById(id)
    const index = dataSource.value.findIndex((row) => getIqcOrderId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as IqcOrder
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 iqcOrderItem 子表（IqcOrderItemQuery + iqcOrderItemApi，与主表 IqcOrderQuery 分离） */
async function loadIqcOrderItemForIqcOrder(record: IqcOrder): Promise<IqcOrderItem[]> {
  const masterId = getIqcOrderId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: IqcOrderItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      iqcOrderId: masterId,
    }
    const result = await iqcOrderItemApi.getIqcOrderItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getIqcOrderId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as IqcOrder
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 iqcOrderChangeLog 子表（IqcOrderChangeLogQuery + iqcOrderChangeLogApi，与主表 IqcOrderQuery 分离） */
async function loadIqcOrderChangeLogForIqcOrder(record: IqcOrder): Promise<IqcOrderChangeLog[]> {
  const masterId = getIqcOrderId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: IqcOrderChangeLogQuery = {
      pageIndex: 1,
      pageSize: 500,
      iqcOrderId: masterId,
    }
    const result = await iqcOrderChangeLogApi.getIqcOrderChangeLogList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getIqcOrderId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, changeLogs: rows } as IqcOrder
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureIqcOrderChildrenLoaded(record: IqcOrder) {
  if (!hasIqcOrderItemRows(record)) {
    await loadIqcOrderItemForIqcOrder(record)
  }
  if (!hasIqcOrderChangeLogRows(record)) {
    await loadIqcOrderChangeLogForIqcOrder(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: IqcOrder) {
  const key = getIqcOrderId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureIqcOrderChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'iqcOrderId',
    key: 'iqcOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'iqcOrderId') ?? ''
  },
  {
    title: t('entity.iqcOrder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.iqcOrder.sourcecode'),
    dataIndex: 'sourceCode',
    key: 'sourceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'sourceCode') ?? ''
  },
  {
    title: t('entity.iqcOrder.inspectiondate'),
    dataIndex: 'inspectionDate',
    key: 'inspectionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'inspectionDate') ?? ''
  },
  {
    title: t('entity.iqcOrder.code'),
    dataIndex: 'iqcOrderCode',
    key: 'iqcOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'iqcOrderCode') ?? ''
  },
  {
    title: t('entity.iqcOrder.suppliercode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.iqcOrder.totalpurchasequantity'),
    dataIndex: 'totalPurchaseQuantity',
    key: 'totalPurchaseQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'totalPurchaseQuantity') ?? ''
  },
  {
    title: t('entity.iqcOrder.totalsamplequantity'),
    dataIndex: 'totalSampleQuantity',
    key: 'totalSampleQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'totalSampleQuantity') ?? ''
  },
  {
    title: t('entity.iqcOrder.totalqualifiedquantity'),
    dataIndex: 'totalQualifiedQuantity',
    key: 'totalQualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'totalQualifiedQuantity') ?? ''
  },
  {
    title: t('entity.iqcOrder.totalunqualifiedquantity'),
    dataIndex: 'totalUnqualifiedQuantity',
    key: 'totalUnqualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'totalUnqualifiedQuantity') ?? ''
  },
  {
    title: t('entity.iqcOrder.totalinspectionreturnquantity'),
    dataIndex: 'totalInspectionReturnQuantity',
    key: 'totalInspectionReturnQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'totalInspectionReturnQuantity') ?? ''
  },
  {
    title: t('entity.iqcOrder.judgestatus'),
    dataIndex: 'judgeStatus',
    key: 'judgeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'judgeStatus') ?? ''
  },
  {
    title: t('entity.iqcOrder.judgeby'),
    dataIndex: 'judgeBy',
    key: 'judgeBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'judgeBy') ?? ''
  },
  {
    title: t('entity.iqcOrder.judgedate'),
    dataIndex: 'judgeDate',
    key: 'judgeDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'judgeDate') ?? ''
  },
  {
    title: t('entity.iqcOrder.judgedescription'),
    dataIndex: 'judgeDescription',
    key: 'judgeDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIqcOrderField(record, 'judgeDescription') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:iqcorder:update',
        onClick: (record: IqcOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:iqcorder:delete',
        onClick: (record: IqcOrder) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getIqcOrderId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getIqcOrderField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: IqcOrder[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: IqcOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getIqcOrderId(selectedRow.value) === getIqcOrderId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: IqcOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: IqcOrder) => ({
  onClick: () => {
    const key = getIqcOrderId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getIqcOrderId(item)))
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
    const params: IqcOrderQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getIqcOrderList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[IqcOrder] 加载数据失败', { error })
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
  iqcOrderCode: '',
  supplierCode: '',
  totalPurchaseQuantity: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.iqcOrder._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: IqcOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.iqcOrder._self') })
  formLoading.value = true
  try {
    const detail = await loadIqcOrderDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.iqcOrder._self') }))
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
      await updateIqcOrder(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.iqcOrder._self') }))
    } else {
      await createIqcOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.iqcOrder._self') }))
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
  const res = await getIqcOrderTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importIqcOrder(file, sheetName)
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
    const exportQuery: IqcOrderQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportIqcOrder(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.iqcOrder._self') }))
  } catch (error: any) {
    logger.error('[IqcOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.iqcOrder._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: IqcOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.iqcOrder._self'), name: t('common.tip.this.target', { target: t('entity.iqcOrder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteIqcOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.iqcOrder._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.iqcOrder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.iqcOrder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteIqcOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.iqcOrder._self') }))
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
  iqcOrderCode: '',
  supplierCode: '',
  totalPurchaseQuantity: undefined as number | undefined,
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
.logistics-quality-operation-iqc-order {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
