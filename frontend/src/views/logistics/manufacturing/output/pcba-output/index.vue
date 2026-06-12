<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：PCBA日报实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-output-pcba-output">
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
      create-permission="logistics:manufacturing:output:pcbaoutput:create"
      update-permission="logistics:manufacturing:output:pcbaoutput:update"
      delete-permission="logistics:manufacturing:output:pcbaoutput:delete"
      import-permission="logistics:manufacturing:output:pcbaoutput:import"
      export-permission="logistics:manufacturing:output:pcbaoutput:export"
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
      :id-column-key="'pcbaOutputId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPcbaOutputId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.pcbaOutputDetail._self') }}</div>
          <a-table
            v-if="hasPcbaOutputDetailRows(record)"
            :columns="pcbaOutputDetailExpandColumns"
            :data-source="getPcbaOutputDetailRows(record)"
            :row-key="(row: PcbaOutputDetail, index?: number) => row?.pcbaOutputDetailId || String(index ?? 0)"
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
      <PcbaOutputForm
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
      :storage-key="'takt-query-fields-logistics-manufacturing-output-pcba-output'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.pcbaOutput.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodCategory')">
      <a-form-item :label="t('entity.pcbaOutput.prodcategory')">
        <a-input
          v-model:value="advancedQueryForm.prodCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodcategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodDateStart')">
      <a-form-item :label="t('entity.pcbaOutput.proddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.prodDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbaOutput.proddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodDateEnd')">
      <a-form-item :label="t('entity.pcbaOutput.proddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.prodDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbaOutput.proddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodLine')">
      <a-form-item :label="t('entity.pcbaOutput.prodline')">
        <a-input
          v-model:value="advancedQueryForm.prodLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodline') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="t('entity.pcbaOutput.shiftno')">
        <a-input-number
          v-model:value="advancedQueryForm.shiftNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.shiftno') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodOrderCode')">
      <a-form-item :label="t('entity.pcbaOutput.prodordercode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodordercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('modelCode')">
      <a-form-item :label="t('entity.pcbaOutput.modelcode')">
        <a-input
          v-model:value="advancedQueryForm.modelCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.modelcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchNo')">
      <a-form-item :label="t('entity.pcbaOutput.batchno')">
        <a-input
          v-model:value="advancedQueryForm.batchNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.batchno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.pcbaOutput.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.materialcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodOrderQty')">
      <a-form-item :label="t('entity.pcbaOutput.prodorderqty')">
        <a-input-number
          v-model:value="advancedQueryForm.prodOrderQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.prodorderqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdMinutes')">
      <a-form-item :label="t('entity.pcbaOutput.stdminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.stdMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.stdminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdShorts')">
      <a-form-item :label="t('entity.pcbaOutput.stdshorts')">
        <a-input-number
          v-model:value="advancedQueryForm.stdShorts"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.stdshorts') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdCapacity')">
      <a-form-item :label="t('entity.pcbaOutput.stdcapacity')">
        <a-input-number
          v-model:value="advancedQueryForm.stdCapacity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutput.stdcapacity') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.pcbaOutput._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.pcbaOutput._self"
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
      :id-column-key="'pcbaOutputId'"
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
 * PCBA日报实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/pcba-output
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import PcbaOutputForm from './components/pcba-output-form.vue'
import { getPcbaOutputList, getPcbaOutputById, createPcbaOutput, updatePcbaOutput, deletePcbaOutputById, deletePcbaOutputBatch, getPcbaOutputTemplate, importPcbaOutput, exportPcbaOutput } from '@/api/logistics/manufacturing/output/pcba-output'
import * as pcbaOutputDetailApi from '@/api/logistics/manufacturing/output/pcba-output-detail'
import type { PcbaOutputDetail, PcbaOutputDetailQuery } from '@/types/logistics/manufacturing/output/pcba-output-detail'
import type { PcbaOutput, PcbaOutputQuery, PcbaOutputCreate, PcbaOutputUpdate } from '@/types/logistics/manufacturing/output/pcba-output'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPcbaOutput')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.pcbaOutput._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PcbaOutput[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<PcbaOutput | null>(null)
/** 表格多选行 */
const selectedRows = ref<PcbaOutput[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PcbaOutput>>({})
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
  shiftNo: undefined as number | undefined,
  prodOrderCode: '',
  modelCode: '',
  batchNo: '',
  materialCode: '',
  prodOrderQty: undefined as number | undefined,
  stdMinutes: undefined as number | undefined,
  stdShorts: undefined as number | undefined,
  stdCapacity: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.pcbaOutput.plantcode') },
  { key: 'prodCategory', label: t('entity.pcbaOutput.prodcategory') },
  { key: 'prodDateStart', label: t('entity.pcbaOutput.proddatestart') },
  { key: 'prodDateEnd', label: t('entity.pcbaOutput.proddateend') },
  { key: 'prodLine', label: t('entity.pcbaOutput.prodline') },
  { key: 'shiftNo', label: t('entity.pcbaOutput.shiftno') },
  { key: 'prodOrderCode', label: t('entity.pcbaOutput.prodordercode') },
  { key: 'modelCode', label: t('entity.pcbaOutput.modelcode') },
  { key: 'batchNo', label: t('entity.pcbaOutput.batchno') },
  { key: 'materialCode', label: t('entity.pcbaOutput.materialcode') },
  { key: 'prodOrderQty', label: t('entity.pcbaOutput.prodorderqty') },
  { key: 'stdMinutes', label: t('entity.pcbaOutput.stdminutes') },
  { key: 'stdShorts', label: t('entity.pcbaOutput.stdshorts') },
  { key: 'stdCapacity', label: t('entity.pcbaOutput.stdcapacity') },
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
const entityIdName = 'pcbaOutputId'
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

/** 展开行预览：pcbaOutputDetail 列 */
const pcbaOutputDetailExpandColumns = computed(() => [
  {
    title: t('entity.pcbaOutputDetail.pcbaoutputname'),
    dataIndex: 'pcbaOutputName',
    key: 'pcbaOutputName',
    ellipsis: true,
  },
  {
    title: t('entity.pcbaOutputDetail.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    ellipsis: true,
  },
  {
    title: t('entity.pcbaOutputDetail.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.pcbaOutputDetail.timeperiod'),
    dataIndex: 'timePeriod',
    key: 'timePeriod',
    ellipsis: true,
  },
  {
    title: t('entity.pcbaOutputDetail.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    ellipsis: true,
  },
  {
    title: t('entity.pcbaOutputDetail.pcbboardtype'),
    dataIndex: 'pcbBoardType',
    key: 'pcbBoardType',
    ellipsis: true,
  },
  {
    title: t('entity.pcbaOutputDetail.panelside'),
    dataIndex: 'panelSide',
    key: 'panelSide',
    ellipsis: true,
  },
  {
    title: t('entity.pcbaOutputDetail.batchqty'),
    dataIndex: 'batchQty',
    key: 'batchQty',
    ellipsis: true,
  },
])

/** 读取主表行上的 pcbaOutputDetail 子表缓存 */
function getPcbaOutputDetailRows(record: PcbaOutput): PcbaOutputDetail[] {
  return (record as any)?.pcbaOutputDetails ?? []
}

/** 主表行是否已加载 pcbaOutputDetail 子表 */
function hasPcbaOutputDetailRows(record: PcbaOutput): boolean {
  return getPcbaOutputDetailRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadPcbaOutputDetail(record: PcbaOutput): Promise<PcbaOutput | null> {
  const id = getPcbaOutputId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getPcbaOutputById(id)
    const index = dataSource.value.findIndex((row) => getPcbaOutputId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as PcbaOutput
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 pcbaOutputDetail 子表（PcbaOutputDetailQuery + pcbaOutputDetailApi，与主表 PcbaOutputQuery 分离） */
async function loadPcbaOutputDetailForPcbaOutput(record: PcbaOutput): Promise<PcbaOutputDetail[]> {
  const masterId = getPcbaOutputId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: PcbaOutputDetailQuery = {
      pageIndex: 1,
      pageSize: 500,
      pcbaOutputId: masterId,
    }
    const result = await pcbaOutputDetailApi.getPcbaOutputDetailList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getPcbaOutputId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, pcbaOutputDetails: rows } as PcbaOutput
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensurePcbaOutputChildrenLoaded(record: PcbaOutput) {
  if (!hasPcbaOutputDetailRows(record)) {
    await loadPcbaOutputDetailForPcbaOutput(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: PcbaOutput) {
  const key = getPcbaOutputId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensurePcbaOutputChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'pcbaOutputId',
    key: 'pcbaOutputId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'pcbaOutputId') ?? ''
  },
  {
    title: t('entity.pcbaOutput.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.pcbaOutput.prodcategory'),
    dataIndex: 'prodCategory',
    key: 'prodCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'prodCategory') ?? ''
  },
  {
    title: t('entity.pcbaOutput.proddate'),
    dataIndex: 'prodDate',
    key: 'prodDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'prodDate') ?? ''
  },
  {
    title: t('entity.pcbaOutput.prodline'),
    dataIndex: 'prodLine',
    key: 'prodLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'prodLine') ?? ''
  },
  {
    title: t('entity.pcbaOutput.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'shiftNo') ?? ''
  },
  {
    title: t('entity.pcbaOutput.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'prodOrderCode') ?? ''
  },
  {
    title: t('entity.pcbaOutput.modelcode'),
    dataIndex: 'modelCode',
    key: 'modelCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'modelCode') ?? ''
  },
  {
    title: t('entity.pcbaOutput.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'batchNo') ?? ''
  },
  {
    title: t('entity.pcbaOutput.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.pcbaOutput.prodorderqty'),
    dataIndex: 'prodOrderQty',
    key: 'prodOrderQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'prodOrderQty') ?? ''
  },
  {
    title: t('entity.pcbaOutput.stdminutes'),
    dataIndex: 'stdMinutes',
    key: 'stdMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'stdMinutes') ?? ''
  },
  {
    title: t('entity.pcbaOutput.stdshorts'),
    dataIndex: 'stdShorts',
    key: 'stdShorts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'stdShorts') ?? ''
  },
  {
    title: t('entity.pcbaOutput.stdcapacity'),
    dataIndex: 'stdCapacity',
    key: 'stdCapacity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPcbaOutputField(record, 'stdCapacity') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:pcbaoutput:update',
        onClick: (record: PcbaOutput) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:pcbaoutput:delete',
        onClick: (record: PcbaOutput) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPcbaOutputId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPcbaOutputField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PcbaOutput[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PcbaOutput, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPcbaOutputId(selectedRow.value) === getPcbaOutputId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PcbaOutput[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: PcbaOutput) => ({
  onClick: () => {
    const key = getPcbaOutputId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPcbaOutputId(item)))
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
    const params: PcbaOutputQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPcbaOutputList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PcbaOutput] 加载数据失败', { error })
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
  prodCategory: '',
  prodDateStart: '',
  prodDateEnd: '',
  prodLine: '',
  shiftNo: undefined as number | undefined,
  prodOrderCode: '',
  modelCode: '',
  batchNo: '',
  materialCode: '',
  prodOrderQty: undefined as number | undefined,
  stdMinutes: undefined as number | undefined,
  stdShorts: undefined as number | undefined,
  stdCapacity: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.pcbaOutput._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: PcbaOutput) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.pcbaOutput._self') })
  formLoading.value = true
  try {
    const detail = await loadPcbaOutputDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.pcbaOutput._self') }))
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
      await updatePcbaOutput(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.pcbaOutput._self') }))
    } else {
      await createPcbaOutput(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.pcbaOutput._self') }))
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
  const res = await getPcbaOutputTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPcbaOutput(file, sheetName)
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
    const exportQuery: PcbaOutputQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPcbaOutput(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.pcbaOutput._self') }))
  } catch (error: any) {
    logger.error('[PcbaOutput] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.pcbaOutput._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PcbaOutput) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.pcbaOutput._self'), name: t('common.tip.this.target', { target: t('entity.pcbaOutput._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePcbaOutputById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.pcbaOutput._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.pcbaOutput._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.pcbaOutput._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePcbaOutputBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.pcbaOutput._self') }))
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
  shiftNo: undefined as number | undefined,
  prodOrderCode: '',
  modelCode: '',
  batchNo: '',
  materialCode: '',
  prodOrderQty: undefined as number | undefined,
  stdMinutes: undefined as number | undefined,
  stdShorts: undefined as number | undefined,
  stdCapacity: undefined as number | undefined,
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
.logistics-manufacturing-output-pcba-output {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
