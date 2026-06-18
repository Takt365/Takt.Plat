<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-incident -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：品质事故主表管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-cost-quality-incident">
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
      create-permission="logistics:quality:cost:qualityincident:create"
      update-permission="logistics:quality:cost:qualityincident:update"
      delete-permission="logistics:quality:cost:qualityincident:delete"
      import-permission="logistics:quality:cost:qualityincident:import"
      export-permission="logistics:quality:cost:qualityincident:export"
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
      :id-column-key="'qualityIncidentId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getQualityIncidentId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.qualityIncidentItem._self') }}</div>
          <a-table
            v-if="hasQualityIncidentItemRows(record)"
            :columns="qualityIncidentItemExpandColumns"
            :data-source="getQualityIncidentItemRows(record)"
            :row-key="(row: QualityIncidentItem, index?: number) => row?.qualityIncidentItemId || String(index ?? 0)"
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
      <QualityIncidentForm
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
      :storage-key="'takt-query-fields-logistics-quality-cost-quality-incident'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.qualityIncident.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIncident.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityIncidentCode')">
      <a-form-item :label="t('entity.qualityIncident.code')">
        <a-input
          v-model:value="advancedQueryForm.qualityIncidentCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIncident.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('incidentDateStart')">
      <a-form-item :label="t('entity.qualityIncident.incidentdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.incidentDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.qualityIncident.incidentdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('incidentDateEnd')">
      <a-form-item :label="t('entity.qualityIncident.incidentdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.incidentDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.qualityIncident.incidentdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectManpowerCostPerMinute')">
      <a-form-item :label="t('entity.qualityIncident.indirectmanpowercostperminute')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectManpowerCostPerMinute"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIncident.indirectmanpowercostperminute') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('model')">
      <a-form-item :label="t('entity.qualityIncident.model')">
        <a-input
          v-model:value="advancedQueryForm.model"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIncident.model') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('incidentReason')">
      <a-form-item :label="t('entity.qualityIncident.incidentreason')">
        <a-input
          v-model:value="advancedQueryForm.incidentReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIncident.incidentreason') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalScrapQuantity')">
      <a-form-item :label="t('entity.qualityIncident.totalscrapquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalScrapQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIncident.totalscrapquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalScrapCost')">
      <a-form-item :label="t('entity.qualityIncident.totalscrapcost')">
        <a-input-number
          v-model:value="advancedQueryForm.totalScrapCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIncident.totalscrapcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCurrency')">
      <a-form-item :label="t('entity.qualityIncident.costcurrency')">
        <a-input
          v-model:value="advancedQueryForm.costCurrency"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityIncident.costcurrency') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.qualityIncident._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.qualityIncident._self"
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
      :id-column-key="'qualityIncidentId'"
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
 * 品质事故主表管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-incident
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import QualityIncidentForm from './components/quality-incident-form.vue'
import { getQualityIncidentList, getQualityIncidentById, createQualityIncident, updateQualityIncident, deleteQualityIncidentById, deleteQualityIncidentBatch, getQualityIncidentTemplate, importQualityIncident, exportQualityIncident } from '@/api/logistics/quality/cost/quality-incident'
import * as qualityIncidentItemApi from '@/api/logistics/quality/cost/quality-incident-item'
import type { QualityIncidentItem, QualityIncidentItemQuery } from '@/types/logistics/quality/cost/quality-incident-item'
import type { QualityIncident, QualityIncidentQuery, QualityIncidentCreate, QualityIncidentUpdate } from '@/types/logistics/quality/cost/quality-incident'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktQualityIncident')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.qualityIncident._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<QualityIncident[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<QualityIncident | null>(null)
/** 表格多选行 */
const selectedRows = ref<QualityIncident[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<QualityIncident>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  qualityIncidentCode: '',
  incidentDateStart: '',
  incidentDateEnd: '',
  indirectManpowerCostPerMinute: undefined as number | undefined,
  model: '',
  incidentReason: '',
  totalScrapQuantity: undefined as number | undefined,
  totalScrapCost: undefined as number | undefined,
  costCurrency: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.qualityIncident.plantcode') },
  { key: 'qualityIncidentCode', label: t('entity.qualityIncident.code') },
  { key: 'incidentDateStart', label: t('entity.qualityIncident.incidentdatestart') },
  { key: 'incidentDateEnd', label: t('entity.qualityIncident.incidentdateend') },
  { key: 'indirectManpowerCostPerMinute', label: t('entity.qualityIncident.indirectmanpowercostperminute') },
  { key: 'model', label: t('entity.qualityIncident.model') },
  { key: 'incidentReason', label: t('entity.qualityIncident.incidentreason') },
  { key: 'totalScrapQuantity', label: t('entity.qualityIncident.totalscrapquantity') },
  { key: 'totalScrapCost', label: t('entity.qualityIncident.totalscrapcost') },
  { key: 'costCurrency', label: t('entity.qualityIncident.costcurrency') },
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
const entityIdName = 'qualityIncidentId'
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

/** 展开行预览：qualityIncidentItem 列 */
const qualityIncidentItemExpandColumns = computed(() => [
  {
    title: t('entity.qualityIncidentItem.qualityincidentname'),
    dataIndex: 'qualityIncidentName',
    key: 'qualityIncidentName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityIncidentItem.qualityincidentcode'),
    dataIndex: 'qualityIncidentCode',
    key: 'qualityIncidentCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityIncidentItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.qualityIncidentItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    ellipsis: true,
  },
  {
    title: t('entity.qualityIncidentItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    ellipsis: true,
  },
  {
    title: t('entity.qualityIncidentItem.scrapcost'),
    dataIndex: 'scrapCost',
    key: 'scrapCost',
    ellipsis: true,
  },
  {
    title: t('entity.qualityIncidentItem.scrapsize'),
    dataIndex: 'scrapSize',
    key: 'scrapSize',
    ellipsis: true,
  },
  {
    title: t('entity.qualityIncidentItem.partprice'),
    dataIndex: 'partPrice',
    key: 'partPrice',
    ellipsis: true,
  },
])

/** 读取主表行上的 qualityIncidentItem 子表缓存 */
function getQualityIncidentItemRows(record: QualityIncident): QualityIncidentItem[] {
  return (record as any)?.incidentItems ?? []
}

/** 主表行是否已加载 qualityIncidentItem 子表 */
function hasQualityIncidentItemRows(record: QualityIncident): boolean {
  return getQualityIncidentItemRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadQualityIncidentDetail(record: QualityIncident): Promise<QualityIncident | null> {
  const id = getQualityIncidentId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getQualityIncidentById(id)
    const index = dataSource.value.findIndex((row) => getQualityIncidentId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as QualityIncident
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 qualityIncidentItem 子表（QualityIncidentItemQuery + qualityIncidentItemApi，与主表 QualityIncidentQuery 分离） */
async function loadQualityIncidentItemForQualityIncident(record: QualityIncident): Promise<QualityIncidentItem[]> {
  const masterId = getQualityIncidentId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: QualityIncidentItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      qualityIncidentId: masterId,
    }
    const result = await qualityIncidentItemApi.getQualityIncidentItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getQualityIncidentId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, incidentItems: rows } as QualityIncident
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureQualityIncidentChildrenLoaded(record: QualityIncident) {
  if (!hasQualityIncidentItemRows(record)) {
    await loadQualityIncidentItemForQualityIncident(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: QualityIncident) {
  const key = getQualityIncidentId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureQualityIncidentChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'qualityIncidentId',
    key: 'qualityIncidentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getQualityIncidentField(record, 'qualityIncidentId') ?? ''
  },
  {
    title: t('entity.qualityIncident.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIncidentField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.qualityIncident.code'),
    dataIndex: 'qualityIncidentCode',
    key: 'qualityIncidentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIncidentField(record, 'qualityIncidentCode') ?? ''
  },
  {
    title: t('entity.qualityIncident.incidentdate'),
    dataIndex: 'incidentDate',
    key: 'incidentDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIncidentField(record, 'incidentDate') ?? ''
  },
  {
    title: t('entity.qualityIncident.indirectmanpowercostperminute'),
    dataIndex: 'indirectManpowerCostPerMinute',
    key: 'indirectManpowerCostPerMinute',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIncidentField(record, 'indirectManpowerCostPerMinute') ?? ''
  },
  {
    title: t('entity.qualityIncident.model'),
    dataIndex: 'model',
    key: 'model',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIncidentField(record, 'model') ?? ''
  },
  {
    title: t('entity.qualityIncident.incidentreason'),
    dataIndex: 'incidentReason',
    key: 'incidentReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIncidentField(record, 'incidentReason') ?? ''
  },
  {
    title: t('entity.qualityIncident.totalscrapquantity'),
    dataIndex: 'totalScrapQuantity',
    key: 'totalScrapQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIncidentField(record, 'totalScrapQuantity') ?? ''
  },
  {
    title: t('entity.qualityIncident.totalscrapcost'),
    dataIndex: 'totalScrapCost',
    key: 'totalScrapCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIncidentField(record, 'totalScrapCost') ?? ''
  },
  {
    title: t('entity.qualityIncident.costcurrency'),
    dataIndex: 'costCurrency',
    key: 'costCurrency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getQualityIncidentField(record, 'costCurrency') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:cost:qualityincident:update',
        onClick: (record: QualityIncident) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:cost:qualityincident:delete',
        onClick: (record: QualityIncident) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getQualityIncidentId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getQualityIncidentField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: QualityIncident[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: QualityIncident, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getQualityIncidentId(selectedRow.value) === getQualityIncidentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: QualityIncident[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: QualityIncident) => ({
  onClick: () => {
    const key = getQualityIncidentId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getQualityIncidentId(item)))
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
    const params: QualityIncidentQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getQualityIncidentList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[QualityIncident] 加载数据失败', { error })
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
  qualityIncidentCode: '',
  incidentDateStart: '',
  incidentDateEnd: '',
  indirectManpowerCostPerMinute: undefined as number | undefined,
  model: '',
  incidentReason: '',
  totalScrapQuantity: undefined as number | undefined,
  totalScrapCost: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.qualityIncident._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: QualityIncident) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.qualityIncident._self') })
  formLoading.value = true
  try {
    const detail = await loadQualityIncidentDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.qualityIncident._self') }))
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
      await updateQualityIncident(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.qualityIncident._self') }))
    } else {
      await createQualityIncident(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.qualityIncident._self') }))
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
  const res = await getQualityIncidentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importQualityIncident(file, sheetName)
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
    const exportQuery: QualityIncidentQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportQualityIncident(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.qualityIncident._self') }))
  } catch (error: any) {
    logger.error('[QualityIncident] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.qualityIncident._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: QualityIncident) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.qualityIncident._self'), name: t('common.tip.this.target', { target: t('entity.qualityIncident._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteQualityIncidentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.qualityIncident._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.qualityIncident._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.qualityIncident._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteQualityIncidentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.qualityIncident._self') }))
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
  qualityIncidentCode: '',
  incidentDateStart: '',
  incidentDateEnd: '',
  indirectManpowerCostPerMinute: undefined as number | undefined,
  model: '',
  incidentReason: '',
  totalScrapQuantity: undefined as number | undefined,
  totalScrapCost: undefined as number | undefined,
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
.logistics-quality-cost-quality-incident {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
