<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/inspection-standard -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：检验标准实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-operation-inspection-standard">
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
      create-permission="logistics:quality:operation:inspectionstandard:create"
      update-permission="logistics:quality:operation:inspectionstandard:update"
      delete-permission="logistics:quality:operation:inspectionstandard:delete"
      import-permission="logistics:quality:operation:inspectionstandard:import"
      export-permission="logistics:quality:operation:inspectionstandard:export"
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
      :id-column-key="'inspectionStandardId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getInspectionStandardId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.inspectionStandardItem._self') }}</div>
          <a-table
            v-if="hasInspectionStandardItemRows(record)"
            :columns="inspectionStandardItemExpandColumns"
            :data-source="getInspectionStandardItemRows(record)"
            :row-key="(row: InspectionStandardItem, index?: number) => row?.inspectionStandardItemId || String(index ?? 0)"
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
      <InspectionStandardForm
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
      :storage-key="'takt-query-fields-logistics-quality-operation-inspection-standard'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.inspectionStandard.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardCode')">
      <a-form-item :label="t('entity.inspectionStandard.standardcode')">
        <a-input
          v-model:value="advancedQueryForm.standardCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.standardcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardName')">
      <a-form-item :label="t('entity.inspectionStandard.standardname')">
        <a-input
          v-model:value="advancedQueryForm.standardName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.standardname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionType')">
      <a-form-item :label="t('entity.inspectionStandard.inspectiontype')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.inspectiontype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCategoryCode')">
      <a-form-item :label="t('entity.inspectionStandard.materialcategorycode')">
        <a-input
          v-model:value="advancedQueryForm.materialCategoryCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.materialcategorycode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCategoryName')">
      <a-form-item :label="t('entity.inspectionStandard.materialcategoryname')">
        <a-input
          v-model:value="advancedQueryForm.materialCategoryName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.materialcategoryname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingSchemeCode')">
      <a-form-item :label="t('entity.inspectionStandard.samplingschemecode')">
        <a-input
          v-model:value="advancedQueryForm.samplingSchemeCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.samplingschemecode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingSchemeName')">
      <a-form-item :label="t('entity.inspectionStandard.samplingschemename')">
        <a-input
          v-model:value="advancedQueryForm.samplingSchemeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.samplingschemename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEnabled')">
      <a-form-item :label="t('entity.inspectionStandard.isenabled')">
        <a-input-number
          v-model:value="advancedQueryForm.isEnabled"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.isenabled') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardStatus')">
      <a-form-item :label="t('entity.inspectionStandard.standardstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.standardStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandard.standardstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardDescription')">
      <a-form-item :label="t('entity.inspectionStandard.standarddescription')">
        <a-textarea
          v-model:value="advancedQueryForm.standardDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.inspectionStandard.standarddescription') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.inspectionStandard._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.inspectionStandard._self"
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
      :id-column-key="'inspectionStandardId'"
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
 * 检验标准实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/inspection-standard
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import InspectionStandardForm from './components/inspection-standard-form.vue'
import { getInspectionStandardList, getInspectionStandardById, createInspectionStandard, updateInspectionStandard, deleteInspectionStandardById, deleteInspectionStandardBatch, getInspectionStandardTemplate, importInspectionStandard, exportInspectionStandard } from '@/api/logistics/quality/operation/inspection-standard'
import * as inspectionStandardItemApi from '@/api/logistics/quality/operation/inspection-standard-item'
import type { InspectionStandardItem, InspectionStandardItemQuery } from '@/types/logistics/quality/operation/inspection-standard-item'
import type { InspectionStandard, InspectionStandardQuery, InspectionStandardCreate, InspectionStandardUpdate } from '@/types/logistics/quality/operation/inspection-standard'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktInspectionStandard')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.inspectionStandard._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<InspectionStandard[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<InspectionStandard | null>(null)
/** 表格多选行 */
const selectedRows = ref<InspectionStandard[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<InspectionStandard>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  standardCode: '',
  standardName: '',
  inspectionType: undefined as number | undefined,
  materialCategoryCode: '',
  materialCategoryName: '',
  samplingSchemeCode: '',
  samplingSchemeName: '',
  isEnabled: undefined as number | undefined,
  standardStatus: undefined as number | undefined,
  standardDescription: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.inspectionStandard.plantcode') },
  { key: 'standardCode', label: t('entity.inspectionStandard.standardcode') },
  { key: 'standardName', label: t('entity.inspectionStandard.standardname') },
  { key: 'inspectionType', label: t('entity.inspectionStandard.inspectiontype') },
  { key: 'materialCategoryCode', label: t('entity.inspectionStandard.materialcategorycode') },
  { key: 'materialCategoryName', label: t('entity.inspectionStandard.materialcategoryname') },
  { key: 'samplingSchemeCode', label: t('entity.inspectionStandard.samplingschemecode') },
  { key: 'samplingSchemeName', label: t('entity.inspectionStandard.samplingschemename') },
  { key: 'isEnabled', label: t('entity.inspectionStandard.isenabled') },
  { key: 'standardStatus', label: t('entity.inspectionStandard.standardstatus') },
  { key: 'standardDescription', label: t('entity.inspectionStandard.standarddescription') },
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
const entityIdName = 'inspectionStandardId'
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

/** 展开行预览：inspectionStandardItem 列 */
const inspectionStandardItemExpandColumns = computed(() => [
  {
    title: t('entity.inspectionStandardItem.inspectionstandardname'),
    dataIndex: 'inspectionStandardName',
    key: 'inspectionStandardName',
    ellipsis: true,
  },
  {
    title: t('entity.inspectionStandardItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.inspectionStandardItem.itemcode'),
    dataIndex: 'itemCode',
    key: 'itemCode',
    ellipsis: true,
  },
  {
    title: t('entity.inspectionStandardItem.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    ellipsis: true,
  },
  {
    title: t('entity.inspectionStandardItem.itemtype'),
    dataIndex: 'itemType',
    key: 'itemType',
    ellipsis: true,
  },
  {
    title: t('entity.inspectionStandardItem.defectlevel'),
    dataIndex: 'defectLevel',
    key: 'defectLevel',
    ellipsis: true,
  },
  {
    title: t('entity.inspectionStandardItem.inspectionmode'),
    dataIndex: 'inspectionMode',
    key: 'inspectionMode',
    ellipsis: true,
  },
  {
    title: t('entity.inspectionStandardItem.standardvalue'),
    dataIndex: 'standardValue',
    key: 'standardValue',
    ellipsis: true,
  },
])

/** 读取主表行上的 inspectionStandardItem 子表缓存 */
function getInspectionStandardItemRows(record: InspectionStandard): InspectionStandardItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 inspectionStandardItem 子表 */
function hasInspectionStandardItemRows(record: InspectionStandard): boolean {
  return getInspectionStandardItemRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadInspectionStandardDetail(record: InspectionStandard): Promise<InspectionStandard | null> {
  const id = getInspectionStandardId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getInspectionStandardById(id)
    const index = dataSource.value.findIndex((row) => getInspectionStandardId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as InspectionStandard
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 inspectionStandardItem 子表（InspectionStandardItemQuery + inspectionStandardItemApi，与主表 InspectionStandardQuery 分离） */
async function loadInspectionStandardItemForInspectionStandard(record: InspectionStandard): Promise<InspectionStandardItem[]> {
  const masterId = getInspectionStandardId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: InspectionStandardItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      inspectionStandardId: masterId,
    }
    const result = await inspectionStandardItemApi.getInspectionStandardItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getInspectionStandardId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as InspectionStandard
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureInspectionStandardChildrenLoaded(record: InspectionStandard) {
  if (!hasInspectionStandardItemRows(record)) {
    await loadInspectionStandardItemForInspectionStandard(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: InspectionStandard) {
  const key = getInspectionStandardId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureInspectionStandardChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'inspectionStandardId',
    key: 'inspectionStandardId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'inspectionStandardId') ?? ''
  },
  {
    title: t('entity.inspectionStandard.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.inspectionStandard.standardcode'),
    dataIndex: 'standardCode',
    key: 'standardCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'standardCode') ?? ''
  },
  {
    title: t('entity.inspectionStandard.standardname'),
    dataIndex: 'standardName',
    key: 'standardName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'standardName') ?? ''
  },
  {
    title: t('entity.inspectionStandard.inspectiontype'),
    dataIndex: 'inspectionType',
    key: 'inspectionType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'inspectionType') ?? ''
  },
  {
    title: t('entity.inspectionStandard.materialcategorycode'),
    dataIndex: 'materialCategoryCode',
    key: 'materialCategoryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'materialCategoryCode') ?? ''
  },
  {
    title: t('entity.inspectionStandard.materialcategoryname'),
    dataIndex: 'materialCategoryName',
    key: 'materialCategoryName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'materialCategoryName') ?? ''
  },
  {
    title: t('entity.inspectionStandard.samplingschemecode'),
    dataIndex: 'samplingSchemeCode',
    key: 'samplingSchemeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'samplingSchemeCode') ?? ''
  },
  {
    title: t('entity.inspectionStandard.samplingschemename'),
    dataIndex: 'samplingSchemeName',
    key: 'samplingSchemeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'samplingSchemeName') ?? ''
  },
  {
    title: t('entity.inspectionStandard.isenabled'),
    dataIndex: 'isEnabled',
    key: 'isEnabled',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'isEnabled') ?? ''
  },
  {
    title: t('entity.inspectionStandard.standardstatus'),
    dataIndex: 'standardStatus',
    key: 'standardStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'standardStatus') ?? ''
  },
  {
    title: t('entity.inspectionStandard.standarddescription'),
    dataIndex: 'standardDescription',
    key: 'standardDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getInspectionStandardField(record, 'standardDescription') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:inspectionstandard:update',
        onClick: (record: InspectionStandard) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:inspectionstandard:delete',
        onClick: (record: InspectionStandard) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getInspectionStandardId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getInspectionStandardField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: InspectionStandard[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: InspectionStandard, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getInspectionStandardId(selectedRow.value) === getInspectionStandardId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: InspectionStandard[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: InspectionStandard) => ({
  onClick: () => {
    const key = getInspectionStandardId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getInspectionStandardId(item)))
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
    const params: InspectionStandardQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getInspectionStandardList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[InspectionStandard] 加载数据失败', { error })
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
  standardCode: '',
  standardName: '',
  inspectionType: undefined as number | undefined,
  materialCategoryCode: '',
  materialCategoryName: '',
  samplingSchemeCode: '',
  samplingSchemeName: '',
  isEnabled: undefined as number | undefined,
  standardStatus: undefined as number | undefined,
  standardDescription: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.inspectionStandard._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: InspectionStandard) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.inspectionStandard._self') })
  formLoading.value = true
  try {
    const detail = await loadInspectionStandardDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.inspectionStandard._self') }))
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
      await updateInspectionStandard(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.inspectionStandard._self') }))
    } else {
      await createInspectionStandard(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.inspectionStandard._self') }))
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
  const res = await getInspectionStandardTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importInspectionStandard(file, sheetName)
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
    const exportQuery: InspectionStandardQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportInspectionStandard(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.inspectionStandard._self') }))
  } catch (error: any) {
    logger.error('[InspectionStandard] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.inspectionStandard._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: InspectionStandard) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.inspectionStandard._self'), name: t('common.tip.this.target', { target: t('entity.inspectionStandard._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteInspectionStandardById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.inspectionStandard._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.inspectionStandard._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.inspectionStandard._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteInspectionStandardBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.inspectionStandard._self') }))
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
  standardCode: '',
  standardName: '',
  inspectionType: undefined as number | undefined,
  materialCategoryCode: '',
  materialCategoryName: '',
  samplingSchemeCode: '',
  samplingSchemeName: '',
  isEnabled: undefined as number | undefined,
  standardStatus: undefined as number | undefined,
  standardDescription: '',
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
.logistics-quality-operation-inspection-standard {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
