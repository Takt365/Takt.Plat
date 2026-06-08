<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/assy-output -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：组立日报管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-output-assy-output">
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
      create-permission="logistics:manufacturing:output:assyoutput:create"
      update-permission="logistics:manufacturing:output:assyoutput:update"
      delete-permission="logistics:manufacturing:output:assyoutput:delete"
      import-permission="logistics:manufacturing:output:assyoutput:import"
      export-permission="logistics:manufacturing:output:assyoutput:export"
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
      :id-column-key="'assyOutputId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getAssyOutputId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.assyOutputDetail._self') }}</div>
          <a-table
            v-if="hasAssyOutputDetailRows(record)"
            :columns="assyOutputDetailExpandColumns"
            :data-source="getAssyOutputDetailRows(record)"
            :row-key="(row: AssyOutputDetail, index?: number) => row?.assyOutputDetailId || String(index ?? 0)"
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
      <AssyOutputForm
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
      :storage-key="'takt-query-fields-logistics-manufacturing-output-assy-output'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.assyOutput.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodCategory')">
      <a-form-item :label="t('entity.assyOutput.prodcategory')">
        <a-input
          v-model:value="advancedQueryForm.prodCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodcategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodDateStart')">
      <a-form-item :label="t('entity.assyOutput.proddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.prodDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assyOutput.proddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodDateEnd')">
      <a-form-item :label="t('entity.assyOutput.proddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.prodDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assyOutput.proddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodLine')">
      <a-form-item :label="t('entity.assyOutput.prodline')">
        <a-input
          v-model:value="advancedQueryForm.prodLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodline') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directLabor')">
      <a-form-item :label="t('entity.assyOutput.directlabor')">
        <a-input-number
          v-model:value="advancedQueryForm.directLabor"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.directlabor') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectLabor')">
      <a-form-item :label="t('entity.assyOutput.indirectlabor')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectLabor"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.indirectlabor') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="t('entity.assyOutput.shiftno')">
        <a-input-number
          v-model:value="advancedQueryForm.shiftNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.shiftno') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodOrderType')">
      <a-form-item :label="t('entity.assyOutput.prodordertype')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodordertype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodOrderCode')">
      <a-form-item :label="t('entity.assyOutput.prodordercode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodordercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('modelCode')">
      <a-form-item :label="t('entity.assyOutput.modelcode')">
        <a-input
          v-model:value="advancedQueryForm.modelCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.modelcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.assyOutput.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.materialcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchNo')">
      <a-form-item :label="t('entity.assyOutput.batchno')">
        <a-input
          v-model:value="advancedQueryForm.batchNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.batchno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodOrderQty')">
      <a-form-item :label="t('entity.assyOutput.prodorderqty')">
        <a-input-number
          v-model:value="advancedQueryForm.prodOrderQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.prodorderqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdMinutes')">
      <a-form-item :label="t('entity.assyOutput.stdminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.stdMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.stdminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdCapacity')">
      <a-form-item :label="t('entity.assyOutput.stdcapacity')">
        <a-input-number
          v-model:value="advancedQueryForm.stdCapacity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.stdcapacity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('status')">
      <a-form-item :label="t('entity.assyOutput.status')">
        <a-input-number
          v-model:value="advancedQueryForm.status"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutput.status') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.assyOutput._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.assyOutput._self"
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
      :id-column-key="'assyOutputId'"
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
 * 组立日报管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/assy-output
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import AssyOutputForm from './components/assy-output-form.vue'
import { getAssyOutputList, getAssyOutputById, createAssyOutput, updateAssyOutput, deleteAssyOutputById, deleteAssyOutputBatch, getAssyOutputTemplate, importAssyOutput, exportAssyOutput } from '@/api/logistics/manufacturing/output/assy-output'
import * as assyOutputDetailApi from '@/api/logistics/manufacturing/output/assy-output-detail'
import type { AssyOutputDetail, AssyOutputDetailQuery } from '@/types/logistics/manufacturing/output/assy-output-detail'
import type { AssyOutput, AssyOutputQuery, AssyOutputCreate, AssyOutputUpdate } from '@/types/logistics/manufacturing/output/assy-output'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAssyOutput')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.assyOutput._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<AssyOutput[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<AssyOutput | null>(null)
/** 表格多选行 */
const selectedRows = ref<AssyOutput[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<AssyOutput>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  prodCategory: '',
  prodDateStart: '',
  prodDateEnd: '',
  prodLine: '',
  directLabor: undefined as number | undefined,
  indirectLabor: undefined as number | undefined,
  shiftNo: undefined as number | undefined,
  prodOrderType: '',
  prodOrderCode: '',
  modelCode: '',
  materialCode: '',
  batchNo: '',
  prodOrderQty: undefined as number | undefined,
  stdMinutes: undefined as number | undefined,
  stdCapacity: undefined as number | undefined,
  status: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.assyOutput.plantcode') },
  { key: 'prodCategory', label: t('entity.assyOutput.prodcategory') },
  { key: 'prodDateStart', label: t('entity.assyOutput.proddatestart') },
  { key: 'prodDateEnd', label: t('entity.assyOutput.proddateend') },
  { key: 'prodLine', label: t('entity.assyOutput.prodline') },
  { key: 'directLabor', label: t('entity.assyOutput.directlabor') },
  { key: 'indirectLabor', label: t('entity.assyOutput.indirectlabor') },
  { key: 'shiftNo', label: t('entity.assyOutput.shiftno') },
  { key: 'prodOrderType', label: t('entity.assyOutput.prodordertype') },
  { key: 'prodOrderCode', label: t('entity.assyOutput.prodordercode') },
  { key: 'modelCode', label: t('entity.assyOutput.modelcode') },
  { key: 'materialCode', label: t('entity.assyOutput.materialcode') },
  { key: 'batchNo', label: t('entity.assyOutput.batchno') },
  { key: 'prodOrderQty', label: t('entity.assyOutput.prodorderqty') },
  { key: 'stdMinutes', label: t('entity.assyOutput.stdminutes') },
  { key: 'stdCapacity', label: t('entity.assyOutput.stdcapacity') },
  { key: 'status', label: t('entity.assyOutput.status') },
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
const entityIdName = 'assyOutputId'
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

/** 展开行预览：assyOutputDetail 列 */
const assyOutputDetailExpandColumns = computed(() => [
  {
    title: t('entity.assyOutputDetail.assyoutputname'),
    dataIndex: 'assyOutputName',
    key: 'assyOutputName',
    ellipsis: true,
  },
  {
    title: t('entity.assyOutputDetail.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    ellipsis: true,
  },
  {
    title: t('entity.assyOutputDetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.assyOutputDetail.timeperiod'),
    dataIndex: 'timePeriod',
    key: 'timePeriod',
    ellipsis: true,
  },
  {
    title: t('entity.assyOutputDetail.prodactualqty'),
    dataIndex: 'prodActualQty',
    key: 'prodActualQty',
    ellipsis: true,
  },
  {
    title: t('entity.assyOutputDetail.downtimeminutes'),
    dataIndex: 'downtimeMinutes',
    key: 'downtimeMinutes',
    ellipsis: true,
  },
  {
    title: t('entity.assyOutputDetail.downtimereason'),
    dataIndex: 'downtimeReason',
    key: 'downtimeReason',
    ellipsis: true,
  },
  {
    title: t('entity.assyOutputDetail.downtimedescription'),
    dataIndex: 'downtimeDescription',
    key: 'downtimeDescription',
    ellipsis: true,
  },
])

/** 读取主表行上的 assyOutputDetail 子表缓存 */
function getAssyOutputDetailRows(record: AssyOutput): AssyOutputDetail[] {
  return (record as any)?.assyOutputDetails ?? []
}

/** 主表行是否已加载 assyOutputDetail 子表 */
function hasAssyOutputDetailRows(record: AssyOutput): boolean {
  return getAssyOutputDetailRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadAssyOutputDetail(record: AssyOutput): Promise<AssyOutput | null> {
  const id = getAssyOutputId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getAssyOutputById(id)
    const index = dataSource.value.findIndex((row) => getAssyOutputId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as AssyOutput
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 assyOutputDetail 子表（AssyOutputDetailQuery + assyOutputDetailApi，与主表 AssyOutputQuery 分离） */
async function loadAssyOutputDetailForAssyOutput(record: AssyOutput): Promise<AssyOutputDetail[]> {
  const masterId = getAssyOutputId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: AssyOutputDetailQuery = {
      pageIndex: 1,
      pageSize: 500,
      assyOutputId: masterId,
    }
    const result = await assyOutputDetailApi.getAssyOutputDetailList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getAssyOutputId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, assyOutputDetails: rows } as AssyOutput
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureAssyOutputChildrenLoaded(record: AssyOutput) {
  if (!hasAssyOutputDetailRows(record)) {
    await loadAssyOutputDetailForAssyOutput(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: AssyOutput) {
  const key = getAssyOutputId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureAssyOutputChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'assyOutputId',
    key: 'assyOutputId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'assyOutputId') ?? ''
  },
  {
    title: t('entity.assyOutput.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.assyOutput.prodcategory'),
    dataIndex: 'prodCategory',
    key: 'prodCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'prodCategory') ?? ''
  },
  {
    title: t('entity.assyOutput.proddate'),
    dataIndex: 'prodDate',
    key: 'prodDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'prodDate') ?? ''
  },
  {
    title: t('entity.assyOutput.prodline'),
    dataIndex: 'prodLine',
    key: 'prodLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'prodLine') ?? ''
  },
  {
    title: t('entity.assyOutput.directlabor'),
    dataIndex: 'directLabor',
    key: 'directLabor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'directLabor') ?? ''
  },
  {
    title: t('entity.assyOutput.indirectlabor'),
    dataIndex: 'indirectLabor',
    key: 'indirectLabor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'indirectLabor') ?? ''
  },
  {
    title: t('entity.assyOutput.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'shiftNo') ?? ''
  },
  {
    title: t('entity.assyOutput.prodordertype'),
    dataIndex: 'prodOrderType',
    key: 'prodOrderType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'prodOrderType') ?? ''
  },
  {
    title: t('entity.assyOutput.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'prodOrderCode') ?? ''
  },
  {
    title: t('entity.assyOutput.modelcode'),
    dataIndex: 'modelCode',
    key: 'modelCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'modelCode') ?? ''
  },
  {
    title: t('entity.assyOutput.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.assyOutput.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'batchNo') ?? ''
  },
  {
    title: t('entity.assyOutput.prodorderqty'),
    dataIndex: 'prodOrderQty',
    key: 'prodOrderQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'prodOrderQty') ?? ''
  },
  {
    title: t('entity.assyOutput.stdminutes'),
    dataIndex: 'stdMinutes',
    key: 'stdMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'stdMinutes') ?? ''
  },
  {
    title: t('entity.assyOutput.stdcapacity'),
    dataIndex: 'stdCapacity',
    key: 'stdCapacity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'stdCapacity') ?? ''
  },
  {
    title: t('entity.assyOutput.status'),
    dataIndex: 'status',
    key: 'status',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'status') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:assyoutput:update',
        onClick: (record: AssyOutput) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:assyoutput:delete',
        onClick: (record: AssyOutput) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getAssyOutputId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getAssyOutputField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AssyOutput[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: AssyOutput, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getAssyOutputId(selectedRow.value) === getAssyOutputId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: AssyOutput[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: AssyOutput) => ({
  onClick: () => {
    const key = getAssyOutputId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getAssyOutputId(item)))
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
    const params: AssyOutputQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getAssyOutputList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[AssyOutput] 加载数据失败', { error })
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
  prodCategory: '',
  prodDateStart: '',
  prodDateEnd: '',
  prodLine: '',
  directLabor: undefined as number | undefined,
  indirectLabor: undefined as number | undefined,
  shiftNo: undefined as number | undefined,
  prodOrderType: '',
  prodOrderCode: '',
  modelCode: '',
  materialCode: '',
  batchNo: '',
  prodOrderQty: undefined as number | undefined,
  stdMinutes: undefined as number | undefined,
  stdCapacity: undefined as number | undefined,
  status: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.assyOutput._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: AssyOutput) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.assyOutput._self') })
  formLoading.value = true
  try {
    const detail = await loadAssyOutputDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.assyOutput._self') }))
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
      await updateAssyOutput(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.assyOutput._self') }))
    } else {
      await createAssyOutput(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.assyOutput._self') }))
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
  const res = await getAssyOutputTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAssyOutput(file, sheetName)
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
    const exportQuery: AssyOutputQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportAssyOutput(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.assyOutput._self') }))
  } catch (error: any) {
    logger.error('[AssyOutput] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.assyOutput._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: AssyOutput) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.assyOutput._self'), name: t('common.tip.this.target', { target: t('entity.assyOutput._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssyOutputById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.assyOutput._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.assyOutput._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.assyOutput._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAssyOutputBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.assyOutput._self') }))
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
  prodCategory: '',
  prodDateStart: '',
  prodDateEnd: '',
  prodLine: '',
  directLabor: undefined as number | undefined,
  indirectLabor: undefined as number | undefined,
  shiftNo: undefined as number | undefined,
  prodOrderType: '',
  prodOrderCode: '',
  modelCode: '',
  materialCode: '',
  batchNo: '',
  prodOrderQty: undefined as number | undefined,
  stdMinutes: undefined as number | undefined,
  stdCapacity: undefined as number | undefined,
  status: undefined as number | undefined,
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
.logistics-manufacturing-output-assy-output {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
