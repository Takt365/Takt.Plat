<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：IPQC制程检验单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:quality:operation:ipqc:order:create"
      update-permission="logistics:quality:operation:ipqc:order:update"
      delete-permission="logistics:quality:operation:ipqc:order:delete"
      import-permission="logistics:quality:operation:ipqc:order:import"
      export-permission="logistics:quality:operation:ipqc:order:export"
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
      :master-row-key="getIpqcOrderId"
      :master-row-selection="rowSelection"
      master-id-column-key="ipqcOrderId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #detail>
        <IpqcOrderItemPanel
          ref="ipqcOrderItemPanelRef"
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
      <IpqcOrderForm
        :key="formData?.ipqcOrderId ?? 'create'"
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
      <a-form-item :label="t('entity.ipqcorder.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCode')">
      <a-form-item :label="t('entity.ipqcorder.sourcecode')">
        <a-input
          v-model:value="advancedQueryForm.sourceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.sourcecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateStart')">
      <a-form-item :label="t('entity.ipqcorder.inspectiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcorder.inspectiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateEnd')">
      <a-form-item :label="t('entity.ipqcorder.inspectiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcorder.inspectiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ipqcOrderCode')">
      <a-form-item :label="t('entity.ipqcorder.code')">
        <a-input
          v-model:value="advancedQueryForm.ipqcOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processCode')">
      <a-form-item :label="t('entity.ipqcorder.processcode')">
        <a-input
          v-model:value="advancedQueryForm.processCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.processcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processName')">
      <a-form-item :label="t('entity.ipqcorder.processname')">
        <a-input
          v-model:value="advancedQueryForm.processName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.processname') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalProductionQuantity')">
      <a-form-item :label="t('entity.ipqcorder.totalproductionquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalProductionQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.totalproductionquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalSampleQuantity')">
      <a-form-item :label="t('entity.ipqcorder.totalsamplequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalSampleQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.totalsamplequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQualifiedQuantity')">
      <a-form-item :label="t('entity.ipqcorder.totalqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.totalqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalUnqualifiedQuantity')">
      <a-form-item :label="t('entity.ipqcorder.totalunqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalUnqualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.totalunqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalInspectionReturnQuantity')">
      <a-form-item :label="t('entity.ipqcorder.totalinspectionreturnquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalInspectionReturnQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.totalinspectionreturnquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeStatus')">
      <a-form-item :label="t('entity.ipqcorder.judgestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.judgeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.judgestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeBy')">
      <a-form-item :label="t('entity.ipqcorder.judgeby')">
        <a-input
          v-model:value="advancedQueryForm.judgeBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorder.judgeby') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDateStart')">
      <a-form-item :label="t('entity.ipqcorder.judgedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.judgeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcorder.judgedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDateEnd')">
      <a-form-item :label="t('entity.ipqcorder.judgedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.judgeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcorder.judgedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeDescription')">
      <a-form-item :label="t('entity.ipqcorder.judgedescription')">
        <a-textarea
          v-model:value="advancedQueryForm.judgeDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcorder.judgedescription') })"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('entity.ipqcorder.extfield')">
        <a-textarea
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcorder.extfield') })"
          :rows="2"
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
      :title="t('common.dialog.title.import', { entity: t('entity.ipqcorder._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ipqcorder._self"
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
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import IpqcOrderForm from './components/ipqc-order-form.vue'
import IpqcOrderItemPanel from './components/ipqc-order-item-panel.vue'
import { provideIpqcOrderMasterContext } from './composables/use-ipqc-order-master-context'
import { getIpqcOrderList, getIpqcOrderById, createIpqcOrder, updateIpqcOrder, deleteIpqcOrderById, deleteIpqcOrderBatch, getIpqcOrderTemplate, importIpqcOrder, exportIpqcOrder, updateIpqcOrderStatus } from '@/api/logistics/quality/operation/ipqc-order'
import type { IpqcOrder, IpqcOrderQuery } from '@/types/logistics/quality/operation/ipqc-order'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktIpqcOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ipqcorder._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<IpqcOrder[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
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
const formData = ref<Partial<IpqcOrder> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
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
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.ipqcorder.plantcode') },
  { key: 'sourceCode', label: t('entity.ipqcorder.sourcecode') },
  { key: 'inspectionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ipqcorder.inspectiondate')) },
  { key: 'inspectionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ipqcorder.inspectiondate')) },
  { key: 'ipqcOrderCode', label: t('entity.ipqcorder.code') },
  { key: 'processCode', label: t('entity.ipqcorder.processcode') },
  { key: 'processName', label: t('entity.ipqcorder.processname') },
  { key: 'totalProductionQuantity', label: t('entity.ipqcorder.totalproductionquantity') },
  { key: 'totalSampleQuantity', label: t('entity.ipqcorder.totalsamplequantity') },
  { key: 'totalQualifiedQuantity', label: t('entity.ipqcorder.totalqualifiedquantity') },
  { key: 'totalUnqualifiedQuantity', label: t('entity.ipqcorder.totalunqualifiedquantity') },
  { key: 'totalInspectionReturnQuantity', label: t('entity.ipqcorder.totalinspectionreturnquantity') },
  { key: 'judgeStatus', label: t('entity.ipqcorder.judgestatus') },
  { key: 'judgeBy', label: t('entity.ipqcorder.judgeby') },
  { key: 'judgeDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ipqcorder.judgedate')) },
  { key: 'judgeDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ipqcorder.judgedate')) },
  { key: 'judgeDescription', label: t('entity.ipqcorder.judgedescription') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('entity.ipqcorder.extfield') },
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

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideIpqcOrderMasterContext()
const ipqcOrderItemPanelRef = ref<InstanceType<typeof IpqcOrderItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {IpqcOrderQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<IpqcOrderQuery>): IpqcOrderQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: IpqcOrderQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof IpqcOrderQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('sourceCode', form.sourceCode)
  assignTrimmed('inspectionDateStart', form.inspectionDateStart)
  assignTrimmed('inspectionDateEnd', form.inspectionDateEnd)
  assignTrimmed('ipqcOrderCode', form.ipqcOrderCode)
  assignTrimmed('processCode', form.processCode)
  assignTrimmed('processName', form.processName)
  if (form.totalProductionQuantity !== undefined && form.totalProductionQuantity !== null) {
    query.totalProductionQuantity = form.totalProductionQuantity
  }
  if (form.totalSampleQuantity !== undefined && form.totalSampleQuantity !== null) {
    query.totalSampleQuantity = form.totalSampleQuantity
  }
  if (form.totalQualifiedQuantity !== undefined && form.totalQualifiedQuantity !== null) {
    query.totalQualifiedQuantity = form.totalQualifiedQuantity
  }
  if (form.totalUnqualifiedQuantity !== undefined && form.totalUnqualifiedQuantity !== null) {
    query.totalUnqualifiedQuantity = form.totalUnqualifiedQuantity
  }
  if (form.totalInspectionReturnQuantity !== undefined && form.totalInspectionReturnQuantity !== null) {
    query.totalInspectionReturnQuantity = form.totalInspectionReturnQuantity
  }
  if (form.judgeStatus !== undefined && form.judgeStatus !== null) {
    query.judgeStatus = form.judgeStatus
  }
  assignTrimmed('judgeBy', form.judgeBy)
  assignTrimmed('judgeDateStart', form.judgeDateStart)
  assignTrimmed('judgeDateEnd', form.judgeDateEnd)
  assignTrimmed('judgeDescription', form.judgeDescription)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('ExtField', form.ExtField)
  assignTrimmed('remark', form.remark)
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
function syncMasterSelection(record: IpqcOrder | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getIpqcOrderId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as IpqcOrder
  const key = getIpqcOrderId(row)
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
    title: t('entity.ipqcorder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.ipqcorder.sourcecode'),
    dataIndex: 'sourceCode',
    key: 'sourceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'sourceCode') ?? ''
  },
  {
    title: t('entity.ipqcorder.inspectiondate'),
    dataIndex: 'inspectionDate',
    key: 'inspectionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'inspectionDate') ?? ''
  },
  {
    title: t('entity.ipqcorder.code'),
    dataIndex: 'ipqcOrderCode',
    key: 'ipqcOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'ipqcOrderCode') ?? ''
  },
  {
    title: t('entity.ipqcorder.processcode'),
    dataIndex: 'processCode',
    key: 'processCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'processCode') ?? ''
  },
  {
    title: t('entity.ipqcorder.processname'),
    dataIndex: 'processName',
    key: 'processName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'processName') ?? ''
  },
  {
    title: t('entity.ipqcorder.totalproductionquantity'),
    dataIndex: 'totalProductionQuantity',
    key: 'totalProductionQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalProductionQuantity') ?? ''
  },
  {
    title: t('entity.ipqcorder.totalsamplequantity'),
    dataIndex: 'totalSampleQuantity',
    key: 'totalSampleQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalSampleQuantity') ?? ''
  },
  {
    title: t('entity.ipqcorder.totalqualifiedquantity'),
    dataIndex: 'totalQualifiedQuantity',
    key: 'totalQualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalQualifiedQuantity') ?? ''
  },
  {
    title: t('entity.ipqcorder.totalunqualifiedquantity'),
    dataIndex: 'totalUnqualifiedQuantity',
    key: 'totalUnqualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalUnqualifiedQuantity') ?? ''
  },
  {
    title: t('entity.ipqcorder.totalinspectionreturnquantity'),
    dataIndex: 'totalInspectionReturnQuantity',
    key: 'totalInspectionReturnQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'totalInspectionReturnQuantity') ?? ''
  },
  {
    title: t('entity.ipqcorder.judgestatus'),
    dataIndex: 'judgeStatus',
    key: 'judgeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'judgeStatus') ?? ''
  },
  {
    title: t('entity.ipqcorder.judgeby'),
    dataIndex: 'judgeBy',
    key: 'judgeBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'judgeBy') ?? ''
  },
  {
    title: t('entity.ipqcorder.judgedate'),
    dataIndex: 'judgeDate',
    key: 'judgeDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderField(record, 'judgeDate') ?? ''
  },
  {
    title: t('entity.ipqcorder.judgedescription'),
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
        permission: 'logistics:quality:operation:ipqc:order:update',
        onClick: (record: IpqcOrder) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:ipqc:order:delete',
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
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: IpqcOrder, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (getIpqcOrderId(selectedRow.value) === getIpqcOrderId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: IpqcOrder[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getIpqcOrderList(buildListQuery())
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
  ExtField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ipqcorder._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: IpqcOrder) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ipqcorder._self') })
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.ipqcorder._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.ipqcorder._self') }))
    } else {
      await createIpqcOrder(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.ipqcorder._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  ipqcOrderItemPanelRef.value?.reload?.()
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
    const exportMeta = await exportIpqcOrder(
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
    message.success(t('common.feedback.export.success', { target: t('entity.ipqcorder._self') }))
  } catch (error: any) {
    logger.error('[IpqcOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.ipqcorder._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: IpqcOrder) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.ipqcorder._self'), name: t('common.tip.this.target', { target: t('entity.ipqcorder._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteIpqcOrderById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcorder._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.ipqcorder._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.ipqcorder._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteIpqcOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcorder._self') }))
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
  ExtField: '',
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
