<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-engineering-change-ec">
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
      create-permission="logistics:manufacturing:engineeringchange:ec:create"
      update-permission="logistics:manufacturing:engineeringchange:ec:update"
      delete-permission="logistics:manufacturing:engineeringchange:ec:delete"
      import-permission="logistics:manufacturing:engineeringchange:ec:import"
      export-permission="logistics:manufacturing:engineeringchange:ec:export"
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
      :id-column-key="'ecId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEcId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.ecDetail._self') }}</div>
          <a-table
            v-if="hasEcDetailRows(record)"
            :columns="ecDetailExpandColumns"
            :data-source="getEcDetailRows(record)"
            :row-key="(row: EcDetail, index?: number) => row?.ecDetailId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
          <div class="mb-2 text-sm font-medium">{{ t('entity.ecAttachment._self') }}</div>
          <a-table
            v-if="hasEcAttachmentRows(record)"
            :columns="ecAttachmentExpandColumns"
            :data-source="getEcAttachmentRows(record)"
            :row-key="(row: EcAttachment, index?: number) => row?.ecAttachmentId || String(index ?? 0)"
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
      <EcForm
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
      :storage-key="'takt-query-fields-logistics-manufacturing-engineering-change-ec'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.ec.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNo')">
      <a-form-item :label="t('entity.ec.no')">
        <a-input
          v-model:value="advancedQueryForm.ecNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.no') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecIssueDateStart')">
      <a-form-item :label="t('entity.ec.issuedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecIssueDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.issuedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecIssueDateEnd')">
      <a-form-item :label="t('entity.ec.issuedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecIssueDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.issuedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('changeStatus')">
      <a-form-item :label="t('entity.ec.changestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.changeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.changestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecTitle')">
      <a-form-item :label="t('entity.ec.title')">
        <a-input
          v-model:value="advancedQueryForm.ecTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.title') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecDetailText')">
      <a-form-item :label="t('entity.ec.detailtext')">
        <a-input
          v-model:value="advancedQueryForm.ecDetailText"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.detailtext') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecLeader')">
      <a-form-item :label="t('entity.ec.leader')">
        <a-input
          v-model:value="advancedQueryForm.ecLeader"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.leader') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecLossAmount')">
      <a-form-item :label="t('entity.ec.lossamount')">
        <a-input-number
          v-model:value="advancedQueryForm.ecLossAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.lossamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecDistinction')">
      <a-form-item :label="t('entity.ec.distinction')">
        <a-input
          v-model:value="advancedQueryForm.ecDistinction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.distinction') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="t('entity.ec.effectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.effectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="t('entity.ec.effectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.effectivedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecEntryDateStart')">
      <a-form-item :label="t('entity.ec.entrydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecEntryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.entrydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecEntryDateEnd')">
      <a-form-item :label="t('entity.ec.entrydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecEntryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ec.entrydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.ec.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.flowinstanceid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecStatus')">
      <a-form-item :label="t('entity.ec.status')">
        <a-input-number
          v-model:value="advancedQueryForm.ecStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ec.status') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.ec._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ec._self"
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
      :id-column-key="'ecId'"
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
 * 设变管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import EcForm from './components/ec-form.vue'
import { getEcList, getEcById, createEc, updateEc, deleteEcById, deleteEcBatch, getEcTemplate, importEc, exportEc } from '@/api/logistics/manufacturing/engineering-change/ec'
import * as ecDetailApi from '@/api/logistics/manufacturing/engineering-change/ec-detail'
import * as ecAttachmentApi from '@/api/logistics/manufacturing/engineering-change/ec-attachment'
import type { EcDetail, EcDetailQuery } from '@/types/logistics/manufacturing/engineering-change/ec-detail'
import type { EcAttachment, EcAttachmentQuery } from '@/types/logistics/manufacturing/engineering-change/ec-attachment'
import type { Ec, EcQuery, EcCreate, EcUpdate } from '@/types/logistics/manufacturing/engineering-change/ec'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEc')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ec._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Ec[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Ec | null>(null)
/** 表格多选行 */
const selectedRows = ref<Ec[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Ec>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  ecNo: '',
  ecIssueDateStart: '',
  ecIssueDateEnd: '',
  changeStatus: undefined as number | undefined,
  ecTitle: '',
  ecDetailText: '',
  ecLeader: '',
  ecLossAmount: undefined as number | undefined,
  ecDistinction: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  ecEntryDateStart: '',
  ecEntryDateEnd: '',
  flowInstanceId: '',
  ecStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.ec.plantcode') },
  { key: 'ecNo', label: t('entity.ec.no') },
  { key: 'ecIssueDateStart', label: t('entity.ec.issuedatestart') },
  { key: 'ecIssueDateEnd', label: t('entity.ec.issuedateend') },
  { key: 'changeStatus', label: t('entity.ec.changestatus') },
  { key: 'ecTitle', label: t('entity.ec.title') },
  { key: 'ecDetailText', label: t('entity.ec.detailtext') },
  { key: 'ecLeader', label: t('entity.ec.leader') },
  { key: 'ecLossAmount', label: t('entity.ec.lossamount') },
  { key: 'ecDistinction', label: t('entity.ec.distinction') },
  { key: 'effectiveDateStart', label: t('entity.ec.effectivedatestart') },
  { key: 'effectiveDateEnd', label: t('entity.ec.effectivedateend') },
  { key: 'ecEntryDateStart', label: t('entity.ec.entrydatestart') },
  { key: 'ecEntryDateEnd', label: t('entity.ec.entrydateend') },
  { key: 'flowInstanceId', label: t('entity.ec.flowinstanceid') },
  { key: 'ecStatus', label: t('entity.ec.status') },
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
const entityIdName = 'ecId'
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

/** 展开行预览：ecDetail 列 */
const ecDetailExpandColumns = computed(() => [
  {
    title: t('entity.ecDetail.ecname'),
    dataIndex: 'ecName',
    key: 'ecName',
    ellipsis: true,
  },
  {
    title: t('entity.ecDetail.ecno'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    ellipsis: true,
  },
  {
    title: t('entity.ecDetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.ecDetail.ecmodel'),
    dataIndex: 'ecModel',
    key: 'ecModel',
    ellipsis: true,
  },
  {
    title: t('entity.ecDetail.ecbomitem'),
    dataIndex: 'ecBomItem',
    key: 'ecBomItem',
    ellipsis: true,
  },
  {
    title: t('entity.ecDetail.ecbomsubitem'),
    dataIndex: 'ecBomSubItem',
    key: 'ecBomSubItem',
    ellipsis: true,
  },
  {
    title: t('entity.ecDetail.ecbomno'),
    dataIndex: 'ecBomNo',
    key: 'ecBomNo',
    ellipsis: true,
  },
  {
    title: t('entity.ecDetail.ecchange'),
    dataIndex: 'ecChange',
    key: 'ecChange',
    ellipsis: true,
  },
])

/** 展开行预览：ecAttachment 列 */
const ecAttachmentExpandColumns = computed(() => [
  {
    title: t('entity.ecAttachment.ecname'),
    dataIndex: 'ecName',
    key: 'ecName',
    ellipsis: true,
  },
  {
    title: t('entity.ecAttachment.ecno'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    ellipsis: true,
  },
  {
    title: t('entity.ecAttachment.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.ecAttachment.attachmenttype'),
    dataIndex: 'attachmentType',
    key: 'attachmentType',
    ellipsis: true,
  },
  {
    title: t('entity.ecAttachment.docno'),
    dataIndex: 'docNo',
    key: 'docNo',
    ellipsis: true,
  },
  {
    title: t('entity.ecAttachment.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    ellipsis: true,
  },
  {
    title: t('entity.ecAttachment.accessurl'),
    dataIndex: 'accessUrl',
    key: 'accessUrl',
    ellipsis: true,
  },
  {
    title: t('entity.ecAttachment.ec'),
    dataIndex: 'ec',
    key: 'ec',
    ellipsis: true,
  },
])

/** 读取主表行上的 ecDetail 子表缓存 */
function getEcDetailRows(record: Ec): EcDetail[] {
  return (record as any)?.ecDetails ?? []
}

/** 主表行是否已加载 ecDetail 子表 */
function hasEcDetailRows(record: Ec): boolean {
  return getEcDetailRows(record).length > 0
}

/** 读取主表行上的 ecAttachment 子表缓存 */
function getEcAttachmentRows(record: Ec): EcAttachment[] {
  return (record as any)?.attachments ?? []
}

/** 主表行是否已加载 ecAttachment 子表 */
function hasEcAttachmentRows(record: Ec): boolean {
  return getEcAttachmentRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadEcDetail(record: Ec): Promise<Ec | null> {
  const id = getEcId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getEcById(id)
    const index = dataSource.value.findIndex((row) => getEcId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Ec
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 ecDetail 子表（EcDetailQuery + ecDetailApi，与主表 EcQuery 分离） */
async function loadEcDetailForEc(record: Ec): Promise<EcDetail[]> {
  const masterId = getEcId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: EcDetailQuery = {
      pageIndex: 1,
      pageSize: 500,
      ecId: masterId,
    }
    const result = await ecDetailApi.getEcDetailList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getEcId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, ecDetails: rows } as Ec
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 懒加载 ecAttachment 子表（EcAttachmentQuery + ecAttachmentApi，与主表 EcQuery 分离） */
async function loadEcAttachmentForEc(record: Ec): Promise<EcAttachment[]> {
  const masterId = getEcId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: EcAttachmentQuery = {
      pageIndex: 1,
      pageSize: 500,
      ecId: masterId,
    }
    const result = await ecAttachmentApi.getEcAttachmentList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getEcId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, attachments: rows } as Ec
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureEcChildrenLoaded(record: Ec) {
  if (!hasEcDetailRows(record)) {
    await loadEcDetailForEc(record)
  }
  if (!hasEcAttachmentRows(record)) {
    await loadEcAttachmentForEc(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: Ec) {
  const key = getEcId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureEcChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ecId',
    key: 'ecId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecId') ?? ''
  },
  {
    title: t('entity.ec.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.ec.no'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecNo') ?? ''
  },
  {
    title: t('entity.ec.issuedate'),
    dataIndex: 'ecIssueDate',
    key: 'ecIssueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecIssueDate') ?? ''
  },
  {
    title: t('entity.ec.changestatus'),
    dataIndex: 'changeStatus',
    key: 'changeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'changeStatus') ?? ''
  },
  {
    title: t('entity.ec.title'),
    dataIndex: 'ecTitle',
    key: 'ecTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecTitle') ?? ''
  },
  {
    title: t('entity.ec.detailtext'),
    dataIndex: 'ecDetailText',
    key: 'ecDetailText',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecDetailText') ?? ''
  },
  {
    title: t('entity.ec.leader'),
    dataIndex: 'ecLeader',
    key: 'ecLeader',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecLeader') ?? ''
  },
  {
    title: t('entity.ec.lossamount'),
    dataIndex: 'ecLossAmount',
    key: 'ecLossAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecLossAmount') ?? ''
  },
  {
    title: t('entity.ec.distinction'),
    dataIndex: 'ecDistinction',
    key: 'ecDistinction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecDistinction') ?? ''
  },
  {
    title: t('entity.ec.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'effectiveDate') ?? ''
  },
  {
    title: t('entity.ec.entrydate'),
    dataIndex: 'ecEntryDate',
    key: 'ecEntryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecEntryDate') ?? ''
  },
  {
    title: t('entity.ec.flowinstanceid'),
    dataIndex: 'flowInstanceId',
    key: 'flowInstanceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'flowInstanceId') ?? ''
  },
  {
    title: t('entity.ec.flowinstancename'),
    dataIndex: 'flowInstanceName',
    key: 'flowInstanceName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'flowInstanceName') ?? ''
  },
  {
    title: t('entity.ec.status'),
    dataIndex: 'ecStatus',
    key: 'ecStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcField(record, 'ecStatus') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineeringchange:ec:update',
        onClick: (record: Ec) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:engineeringchange:ec:delete',
        onClick: (record: Ec) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEcId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEcField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Ec[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Ec, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEcId(selectedRow.value) === getEcId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Ec[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Ec) => ({
  onClick: () => {
    const key = getEcId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEcId(item)))
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
    const params: EcQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getEcList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Ec] 加载数据失败', { error })
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
  ecNo: '',
  ecIssueDateStart: '',
  ecIssueDateEnd: '',
  changeStatus: undefined as number | undefined,
  ecTitle: '',
  ecDetailText: '',
  ecLeader: '',
  ecLossAmount: undefined as number | undefined,
  ecDistinction: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  ecEntryDateStart: '',
  ecEntryDateEnd: '',
  flowInstanceId: '',
  ecStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ec._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Ec) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ec._self') })
  formLoading.value = true
  try {
    const detail = await loadEcDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.ec._self') }))
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
      await updateEc(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.ec._self') }))
    } else {
      await createEc(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.ec._self') }))
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
  const res = await getEcTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEc(file, sheetName)
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
    const exportQuery: EcQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportEc(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.ec._self') }))
  } catch (error: any) {
    logger.error('[Ec] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.ec._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Ec) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.ec._self'), name: t('common.tip.this.target', { target: t('entity.ec._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEcById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.ec._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.ec._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.ec._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEcBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ec._self') }))
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
  ecNo: '',
  ecIssueDateStart: '',
  ecIssueDateEnd: '',
  changeStatus: undefined as number | undefined,
  ecTitle: '',
  ecDetailText: '',
  ecLeader: '',
  ecLossAmount: undefined as number | undefined,
  ecDistinction: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  ecEntryDateStart: '',
  ecEntryDateEnd: '',
  flowInstanceId: '',
  ecStatus: undefined as number | undefined,
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
.logistics-manufacturing-engineering-change-ec {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
