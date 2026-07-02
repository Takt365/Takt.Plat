<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/assy-defect -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：组立不良日报实体 不良率管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:manufacturing:defect:assy:create"
      update-permission="logistics:manufacturing:defect:assy:update"
      delete-permission="logistics:manufacturing:defect:assy:delete"
      import-permission="logistics:manufacturing:defect:assy:import"
      export-permission="logistics:manufacturing:defect:assy:export"
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
      :master-row-key="getAssyDefectId"
      :master-row-selection="rowSelection"
      master-id-column-key="assyDefectId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'prodCategory'">
          <TaktDictTag
            :value="getAssyDefectField(record, 'prodCategory')"
            dict-type="logistics_prod_category"
          />
        </template>
        <template v-else-if="column.key === 'shiftNo'">
          <TaktDictTag
            :value="getAssyDefectField(record, 'shiftNo')"
            dict-type="logistics_shift_category"
          />
        </template>
      </template>
      <template #detail>
        <AssyDefectDetailPanel
          ref="assyDefectDetailPanelRef"
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
      <AssyDefectForm
        :key="formData?.assyDefectId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-defect-assy-defect'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.assydefect.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefect.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodCategory')">
      <a-form-item :label="t('entity.assydefect.prodcategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.prodCategory"
          dict-type="logistics_prod_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assydefect.prodcategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodDateStart')">
      <a-form-item :label="t('entity.assydefect.proddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.prodDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assydefect.proddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodDateEnd')">
      <a-form-item :label="t('entity.assydefect.proddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.prodDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assydefect.proddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodTeam')">
      <a-form-item :label="t('entity.assydefect.prodteam')">
        <a-input
          v-model:value="advancedQueryForm.prodTeam"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefect.prodteam') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="t('entity.assydefect.shiftno')">
        <TaktSelect
          v-model:value="advancedQueryForm.shiftNo"
          dict-type="logistics_shift_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assydefect.shiftno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodOrderCode')">
      <a-form-item :label="t('entity.assydefect.prodordercode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefect.prodordercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodOrderQty')">
      <a-form-item :label="t('entity.assydefect.prodorderqty')">
        <a-input-number
          v-model:value="advancedQueryForm.prodOrderQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefect.prodorderqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('modelCode')">
      <a-form-item :label="t('entity.assydefect.modelcode')">
        <a-input
          v-model:value="advancedQueryForm.modelCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefect.modelcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchNo')">
      <a-form-item :label="t('entity.assydefect.batchno')">
        <a-input
          v-model:value="advancedQueryForm.batchNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefect.batchno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.assydefect.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefect.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodActualQty')">
      <a-form-item :label="t('entity.assydefect.prodactualqty')">
        <a-input-number
          v-model:value="advancedQueryForm.prodActualQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefect.prodactualqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('goodQuantity')">
      <a-form-item :label="t('entity.assydefect.goodquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.goodQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assydefect.goodquantity') })"
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
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ t('common.page.entity.extfield') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
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
      :title="t('common.dialog.title.import', { entity: t('entity.assydefect._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.assydefect._self"
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
      :id-column-key="'assyDefectId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 组立不良日报实体 不良率管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/assy-defect
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import AssyDefectForm from './components/assy-defect-form.vue'
import AssyDefectDetailPanel from './components/assy-defect-detail-panel.vue'
import { provideAssyDefectMasterContext } from './composables/use-assy-defect-master-context'
import { getAssyDefectList, getAssyDefectById, createAssyDefect, updateAssyDefect, deleteAssyDefectById, deleteAssyDefectBatch, getAssyDefectTemplate, importAssyDefect, exportAssyDefect } from '@/api/logistics/manufacturing/defect/assy-defect'
import type { AssyDefect, AssyDefectQuery } from '@/types/logistics/manufacturing/defect/assy-defect'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { normalizeImportResult, type TaktImportResult } from '@/utils/takt-import-result'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAssyDefect')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.assydefect._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<AssyDefect[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<AssyDefect | null>(null)
/** 表格多选行 */
const selectedRows = ref<AssyDefect[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<AssyDefect> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  prodCategory: '',
  prodDateStart: '',
  prodDateEnd: '',
  prodTeam: '',
  shiftNo: undefined as number | undefined,
  prodOrderCode: '',
  prodOrderQty: undefined as number | undefined,
  modelCode: '',
  batchNo: '',
  materialCode: '',
  prodActualQty: undefined as number | undefined,
  goodQuantity: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.assydefect.plantcode') },
  { key: 'prodCategory', label: t('entity.assydefect.prodcategory') },
  { key: 'prodDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.assydefect.proddate')) },
  { key: 'prodDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.assydefect.proddate')) },
  { key: 'prodTeam', label: t('entity.assydefect.prodteam') },
  { key: 'shiftNo', label: t('entity.assydefect.shiftno') },
  { key: 'prodOrderCode', label: t('entity.assydefect.prodordercode') },
  { key: 'prodOrderQty', label: t('entity.assydefect.prodorderqty') },
  { key: 'modelCode', label: t('entity.assydefect.modelcode') },
  { key: 'batchNo', label: t('entity.assydefect.batchno') },
  { key: 'materialCode', label: t('entity.assydefect.materialcode') },
  { key: 'prodActualQty', label: t('entity.assydefect.prodactualqty') },
  { key: 'goodQuantity', label: t('entity.assydefect.goodquantity') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
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
const entityIdName = 'assyDefectId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideAssyDefectMasterContext()
const assyDefectDetailPanelRef = ref<InstanceType<typeof AssyDefectDetailPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {AssyDefectQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<AssyDefectQuery>): AssyDefectQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: AssyDefectQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof AssyDefectQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('prodCategory', form.prodCategory)
  assignTrimmed('prodDateStart', form.prodDateStart)
  assignTrimmed('prodDateEnd', form.prodDateEnd)
  assignTrimmed('prodTeam', form.prodTeam)
  if (form.shiftNo !== undefined && form.shiftNo !== null) {
    query.shiftNo = form.shiftNo
  }
  assignTrimmed('prodOrderCode', form.prodOrderCode)
  if (form.prodOrderQty !== undefined && form.prodOrderQty !== null) {
    query.prodOrderQty = form.prodOrderQty
  }
  assignTrimmed('modelCode', form.modelCode)
  assignTrimmed('batchNo', form.batchNo)
  assignTrimmed('materialCode', form.materialCode)
  if (form.prodActualQty !== undefined && form.prodActualQty !== null) {
    query.prodActualQty = form.prodActualQty
  }
  if (form.goodQuantity !== undefined && form.goodQuantity !== null) {
    query.goodQuantity = form.goodQuantity
  }
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: AssyDefect | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getAssyDefectId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as AssyDefect
  const key = getAssyDefectId(row)
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
async function loadAssyDefectDetail(record: AssyDefect): Promise<AssyDefect | null> {
  const id = getAssyDefectId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getAssyDefectById(id)
    const index = dataSource.value.findIndex((row) => getAssyDefectId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as AssyDefect
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
    dataIndex: 'assyDefectId',
    key: 'assyDefectId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'assyDefectId') ?? ''
  },
  {
    title: t('entity.assydefect.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.assydefect.prodcategory'),
    dataIndex: 'prodCategory',
    key: 'prodCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.assydefect.proddate'),
    dataIndex: 'prodDate',
    key: 'prodDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'prodDate') ?? ''
  },
  {
    title: t('entity.assydefect.prodteam'),
    dataIndex: 'prodTeam',
    key: 'prodTeam',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'prodTeam') ?? ''
  },
  {
    title: t('entity.assydefect.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.assydefect.prodordercode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'prodOrderCode') ?? ''
  },
  {
    title: t('entity.assydefect.prodorderqty'),
    dataIndex: 'prodOrderQty',
    key: 'prodOrderQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'prodOrderQty') ?? ''
  },
  {
    title: t('entity.assydefect.modelcode'),
    dataIndex: 'modelCode',
    key: 'modelCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'modelCode') ?? ''
  },
  {
    title: t('entity.assydefect.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'batchNo') ?? ''
  },
  {
    title: t('entity.assydefect.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.assydefect.prodactualqty'),
    dataIndex: 'prodActualQty',
    key: 'prodActualQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'prodActualQty') ?? ''
  },
  {
    title: t('entity.assydefect.goodquantity'),
    dataIndex: 'goodQuantity',
    key: 'goodQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyDefectField(record, 'goodQuantity') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:defect:assy:update',
        onClick: (record: AssyDefect) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:defect:assy:delete',
        onClick: (record: AssyDefect) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getAssyDefectId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getAssyDefectField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AssyDefect[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: AssyDefect, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getAssyDefectId(selectedRow.value) === getAssyDefectId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: AssyDefect[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getAssyDefectList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[AssyDefect] 加载数据失败', { error })
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
  prodCategory: '',
  prodDateStart: '',
  prodDateEnd: '',
  prodTeam: '',
  shiftNo: undefined as number | undefined,
  prodOrderCode: '',
  prodOrderQty: undefined as number | undefined,
  modelCode: '',
  batchNo: '',
  materialCode: '',
  prodActualQty: undefined as number | undefined,
  goodQuantity: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.assydefect._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: AssyDefect) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.assydefect._self') })
  formLoading.value = true
  try {
    const detail = await loadAssyDefectDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.assydefect._self') }))
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
      await updateAssyDefect(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.assydefect._self') }))
    } else {
      await createAssyDefect(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.assydefect._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  assyDefectDetailPanelRef.value?.reload?.()
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
  const res = await getAssyDefectTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importAssyDefect(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    assyDefectDetailPanelRef.value?.reload?.()
      }
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportAssyDefect(
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
    message.success(t('common.feedback.export.success', { target: t('entity.assydefect._self') }))
  } catch (error: any) {
    logger.error('[AssyDefect] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.assydefect._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: AssyDefect) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.assydefect._self'), name: t('common.tip.this.target', { target: t('entity.assydefect._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssyDefectById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.assydefect._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.assydefect._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.assydefect._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAssyDefectBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.assydefect._self') }))
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
  prodCategory: '',
  prodDateStart: '',
  prodDateEnd: '',
  prodTeam: '',
  shiftNo: undefined as number | undefined,
  prodOrderCode: '',
  prodOrderQty: undefined as number | undefined,
  modelCode: '',
  batchNo: '',
  materialCode: '',
  prodActualQty: undefined as number | undefined,
  goodQuantity: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
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
