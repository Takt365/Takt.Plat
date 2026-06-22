<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/it-asset -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务台 IT 设备保修扩展实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="routine-help-desk-it-asset">
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
      create-permission="routine:help:desk:it:asset:create"
      update-permission="routine:help:desk:it:asset:update"
      delete-permission="routine:help:desk:it:asset:delete"
      import-permission="routine:help:desk:it:asset:import"
      export-permission="routine:help:desk:it:asset:export"
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
      :id-column-key="'itAssetId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getItAssetId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.itAssetChangeLog._self') }}</div>
          <a-table
            v-if="hasItAssetChangeLogRows(record)"
            :columns="itAssetChangeLogExpandColumns"
            :data-source="getItAssetChangeLogRows(record)"
            :row-key="(row: ItAssetChangeLog, index?: number) => row?.itAssetChangeLogId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.ticket._self') }}</div>
          <a-table
            v-if="hasTicketRows(record)"
            :columns="ticketExpandColumns"
            :data-source="getTicketRows(record)"
            :row-key="(row: Ticket, index?: number) => row?.ticketId || String(index ?? 0)"
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
      <ItAssetForm
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
      :storage-key="'takt-query-fields-routine-help-desk-it-asset'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('assetCode')">
      <a-form-item :label="t('entity.itAsset.assetcode')">
        <a-input
          v-model:value="advancedQueryForm.assetCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.assetcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyType')">
      <a-form-item :label="t('entity.itAsset.warrantytype')">
        <a-input-number
          v-model:value="advancedQueryForm.warrantyType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.warrantytype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyStartDateStart')">
      <a-form-item :label="t('entity.itAsset.warrantystartdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyStartDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.warrantystartdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyStartDateEnd')">
      <a-form-item :label="t('entity.itAsset.warrantystartdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyStartDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.warrantystartdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyExpiryDateStart')">
      <a-form-item :label="t('entity.itAsset.warrantyexpirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyExpiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.warrantyexpirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyExpiryDateEnd')">
      <a-form-item :label="t('entity.itAsset.warrantyexpirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyExpiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.warrantyexpirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyProvider')">
      <a-form-item :label="t('entity.itAsset.warrantyprovider')">
        <a-input
          v-model:value="advancedQueryForm.warrantyProvider"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.warrantyprovider') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyContractNo')">
      <a-form-item :label="t('entity.itAsset.warrantycontractno')">
        <a-input
          v-model:value="advancedQueryForm.warrantyContractNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.warrantycontractno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceHotline')">
      <a-form-item :label="t('entity.itAsset.servicehotline')">
        <a-input
          v-model:value="advancedQueryForm.serviceHotline"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.servicehotline') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceEmail')">
      <a-form-item :label="t('entity.itAsset.serviceemail')">
        <a-input
          v-model:value="advancedQueryForm.serviceEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itAsset.serviceemail') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceExpiryDateStart')">
      <a-form-item :label="t('entity.itAsset.maintenanceexpirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceExpiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.maintenanceexpirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceExpiryDateEnd')">
      <a-form-item :label="t('entity.itAsset.maintenanceexpirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceExpiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.maintenanceexpirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastMaintenanceDateStart')">
      <a-form-item :label="t('entity.itAsset.lastmaintenancedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.lastMaintenanceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.lastmaintenancedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastMaintenanceDateEnd')">
      <a-form-item :label="t('entity.itAsset.lastmaintenancedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.lastMaintenanceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.lastmaintenancedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateStart')">
      <a-form-item :label="t('entity.itAsset.nextmaintenancedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.nextmaintenancedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateEnd')">
      <a-form-item :label="t('entity.itAsset.nextmaintenancedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itAsset.nextmaintenancedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyRemark')">
      <a-form-item :label="t('entity.itAsset.warrantyremark')">
        <a-textarea
          v-model:value="advancedQueryForm.warrantyRemark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.itAsset.warrantyremark') })"
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
      <a-form-item :label="t('common.page.entity.ExtField')">
        <a-input
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.ExtField') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.itAsset._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.itAsset._self"
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
      :id-column-key="'itAssetId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
/**
 * 服务台 IT 设备保修扩展实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/it-asset
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import ItAssetForm from './components/it-asset-form.vue'
import { getItAssetList, getItAssetById, createItAsset, updateItAsset, deleteItAssetById, deleteItAssetBatch, getItAssetTemplate, importItAsset, exportItAsset } from '@/api/routine/help-desk/it-asset'
import * as ticketApi from '@/api/routine/help-desk/ticket'
import type { ItAssetChangeLog } from '@/types/routine/help-desk/it-asset-change-log'
import type { Ticket, TicketQuery } from '@/types/routine/help-desk/ticket'
import type { ItAsset, ItAssetQuery, ItAssetCreate, ItAssetUpdate } from '@/types/routine/help-desk/it-asset'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktItAsset')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.itAsset._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ItAsset[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<ItAsset | null>(null)
/** 表格多选行 */
const selectedRows = ref<ItAsset[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<ItAsset>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  assetCode: '',
  warrantyType: undefined as number | undefined,
  warrantyStartDateStart: '',
  warrantyStartDateEnd: '',
  warrantyExpiryDateStart: '',
  warrantyExpiryDateEnd: '',
  warrantyProvider: '',
  warrantyContractNo: '',
  serviceHotline: '',
  serviceEmail: '',
  maintenanceExpiryDateStart: '',
  maintenanceExpiryDateEnd: '',
  lastMaintenanceDateStart: '',
  lastMaintenanceDateEnd: '',
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  warrantyRemark: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'assetCode', label: t('entity.itAsset.assetcode') },
  { key: 'warrantyType', label: t('entity.itAsset.warrantytype') },
  { key: 'warrantyStartDateStart', label: t('entity.itAsset.warrantystartdatestart') },
  { key: 'warrantyStartDateEnd', label: t('entity.itAsset.warrantystartdateend') },
  { key: 'warrantyExpiryDateStart', label: t('entity.itAsset.warrantyexpirydatestart') },
  { key: 'warrantyExpiryDateEnd', label: t('entity.itAsset.warrantyexpirydateend') },
  { key: 'warrantyProvider', label: t('entity.itAsset.warrantyprovider') },
  { key: 'warrantyContractNo', label: t('entity.itAsset.warrantycontractno') },
  { key: 'serviceHotline', label: t('entity.itAsset.servicehotline') },
  { key: 'serviceEmail', label: t('entity.itAsset.serviceemail') },
  { key: 'maintenanceExpiryDateStart', label: t('entity.itAsset.maintenanceexpirydatestart') },
  { key: 'maintenanceExpiryDateEnd', label: t('entity.itAsset.maintenanceexpirydateend') },
  { key: 'lastMaintenanceDateStart', label: t('entity.itAsset.lastmaintenancedatestart') },
  { key: 'lastMaintenanceDateEnd', label: t('entity.itAsset.lastmaintenancedateend') },
  { key: 'nextMaintenanceDateStart', label: t('entity.itAsset.nextmaintenancedatestart') },
  { key: 'nextMaintenanceDateEnd', label: t('entity.itAsset.nextmaintenancedateend') },
  { key: 'warrantyRemark', label: t('entity.itAsset.warrantyremark') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.ExtField') },
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
const entityIdName = 'itAssetId'
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

/** 展开行预览：itAssetChangeLog 列 */
const itAssetChangeLogExpandColumns = computed(() => [

])

/** 展开行预览：ticket 列 */
const ticketExpandColumns = computed(() => [
  {
    title: t('entity.ticket.no'),
    dataIndex: 'ticketNo',
    key: 'ticketNo',
    ellipsis: true,
  },
  {
    title: t('entity.ticket.title'),
    dataIndex: 'title',
    key: 'title',
    ellipsis: true,
  },
  {
    title: t('entity.ticket.content'),
    dataIndex: 'content',
    key: 'content',
    ellipsis: true,
  },
  {
    title: t('entity.ticket.attachmentsjson'),
    dataIndex: 'attachmentsJson',
    key: 'attachmentsJson',
    ellipsis: true,
  },
  {
    title: t('entity.ticket.status'),
    dataIndex: 'ticketStatus',
    key: 'ticketStatus',
    ellipsis: true,
  },
  {
    title: t('entity.ticket.priority'),
    dataIndex: 'priority',
    key: 'priority',
    ellipsis: true,
  },
  {
    title: t('entity.ticket.categorycode'),
    dataIndex: 'categoryCode',
    key: 'categoryCode',
    ellipsis: true,
  },
  {
    title: t('entity.ticket.assetcode'),
    dataIndex: 'assetCode',
    key: 'assetCode',
    ellipsis: true,
  },
])

/** 读取主表行上的 itAssetChangeLog 子表缓存 */
function getItAssetChangeLogRows(record: ItAsset): ItAssetChangeLog[] {
  return (record as any)?.changeLogs ?? []
}

/** 主表行是否已加载 itAssetChangeLog 子表 */
function hasItAssetChangeLogRows(record: ItAsset): boolean {
  return getItAssetChangeLogRows(record).length > 0
}

/** 读取主表行上的 ticket 子表缓存 */
function getTicketRows(record: ItAsset): Ticket[] {
  return (record as any)?.tickets ?? []
}

/** 主表行是否已加载 ticket 子表 */
function hasTicketRows(record: ItAsset): boolean {
  return getTicketRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadItAssetDetail(record: ItAsset): Promise<ItAsset | null> {
  const id = getItAssetId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getItAssetById(id)
    const index = dataSource.value.findIndex((row) => getItAssetId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as ItAsset
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 通过主表详情接口加载 itAssetChangeLog 子表 */
async function loadItAssetChangeLogForItAsset(record: ItAsset): Promise<ItAssetChangeLog[]> {
  const detail = await loadItAssetDetail(record)
  return detail?.changeLogs ?? []
}

/** 懒加载 ticket 子表（TicketQuery + ticketApi，与主表 ItAssetQuery 分离） */
async function loadTicketForItAsset(record: ItAsset): Promise<Ticket[]> {
  const masterId = getItAssetId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: TicketQuery = {
      pageIndex: 1,
      pageSize: 500,
      itAssetId: masterId,
    }
    const result = await ticketApi.getTicketList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getItAssetId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, tickets: rows } as ItAsset
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureItAssetChildrenLoaded(record: ItAsset) {
  if (!hasItAssetChangeLogRows(record)) {
    await loadItAssetChangeLogForItAsset(record)
  }
  if (!hasTicketRows(record)) {
    await loadTicketForItAsset(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: ItAsset) {
  const key = getItAssetId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureItAssetChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'itAssetId',
    key: 'itAssetId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'itAssetId') ?? ''
  },
  {
    title: t('entity.itAsset.assetcode'),
    dataIndex: 'assetCode',
    key: 'assetCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'assetCode') ?? ''
  },
  {
    title: t('entity.itAsset.warrantytype'),
    dataIndex: 'warrantyType',
    key: 'warrantyType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyType') ?? ''
  },
  {
    title: t('entity.itAsset.warrantystartdate'),
    dataIndex: 'warrantyStartDate',
    key: 'warrantyStartDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyStartDate') ?? ''
  },
  {
    title: t('entity.itAsset.warrantyexpirydate'),
    dataIndex: 'warrantyExpiryDate',
    key: 'warrantyExpiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyExpiryDate') ?? ''
  },
  {
    title: t('entity.itAsset.warrantyprovider'),
    dataIndex: 'warrantyProvider',
    key: 'warrantyProvider',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyProvider') ?? ''
  },
  {
    title: t('entity.itAsset.warrantycontractno'),
    dataIndex: 'warrantyContractNo',
    key: 'warrantyContractNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyContractNo') ?? ''
  },
  {
    title: t('entity.itAsset.servicehotline'),
    dataIndex: 'serviceHotline',
    key: 'serviceHotline',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'serviceHotline') ?? ''
  },
  {
    title: t('entity.itAsset.serviceemail'),
    dataIndex: 'serviceEmail',
    key: 'serviceEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'serviceEmail') ?? ''
  },
  {
    title: t('entity.itAsset.maintenanceexpirydate'),
    dataIndex: 'maintenanceExpiryDate',
    key: 'maintenanceExpiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'maintenanceExpiryDate') ?? ''
  },
  {
    title: t('entity.itAsset.lastmaintenancedate'),
    dataIndex: 'lastMaintenanceDate',
    key: 'lastMaintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'lastMaintenanceDate') ?? ''
  },
  {
    title: t('entity.itAsset.nextmaintenancedate'),
    dataIndex: 'nextMaintenanceDate',
    key: 'nextMaintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'nextMaintenanceDate') ?? ''
  },
  {
    title: t('entity.itAsset.warrantyremark'),
    dataIndex: 'warrantyRemark',
    key: 'warrantyRemark',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyRemark') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:help:desk:it:asset:update',
        onClick: (record: ItAsset) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:help:desk:it:asset:delete',
        onClick: (record: ItAsset) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getItAssetId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getItAssetField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ItAsset[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ItAsset, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getItAssetId(selectedRow.value) === getItAssetId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ItAsset[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: ItAsset) => ({
  onClick: () => {
    const key = getItAssetId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getItAssetId(item)))
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
    const params: ItAssetQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getItAssetList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[ItAsset] 加载数据失败', { error })
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
  assetCode: '',
  warrantyType: undefined as number | undefined,
  warrantyStartDateStart: '',
  warrantyStartDateEnd: '',
  warrantyExpiryDateStart: '',
  warrantyExpiryDateEnd: '',
  warrantyProvider: '',
  warrantyContractNo: '',
  serviceHotline: '',
  serviceEmail: '',
  maintenanceExpiryDateStart: '',
  maintenanceExpiryDateEnd: '',
  lastMaintenanceDateStart: '',
  lastMaintenanceDateEnd: '',
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  warrantyRemark: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.itAsset._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: ItAsset) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.itAsset._self') })
  formLoading.value = true
  try {
    const detail = await loadItAssetDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.itAsset._self') }))
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
      await updateItAsset(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.itAsset._self') }))
    } else {
      await createItAsset(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.itAsset._self') }))
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
  const res = await getItAssetTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importItAsset(file, sheetName)
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
    const exportQuery: ItAssetQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportItAsset(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.itAsset._self') }))
  } catch (error: any) {
    logger.error('[ItAsset] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.itAsset._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ItAsset) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.itAsset._self'), name: t('common.tip.this.target', { target: t('entity.itAsset._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteItAssetById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.itAsset._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.itAsset._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.itAsset._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteItAssetBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.itAsset._self') }))
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
  assetCode: '',
  warrantyType: undefined as number | undefined,
  warrantyStartDateStart: '',
  warrantyStartDateEnd: '',
  warrantyExpiryDateStart: '',
  warrantyExpiryDateEnd: '',
  warrantyProvider: '',
  warrantyContractNo: '',
  serviceHotline: '',
  serviceEmail: '',
  maintenanceExpiryDateStart: '',
  maintenanceExpiryDateEnd: '',
  lastMaintenanceDateStart: '',
  lastMaintenanceDateEnd: '',
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  warrantyRemark: '',
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
.routine-help-desk-it-asset {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
