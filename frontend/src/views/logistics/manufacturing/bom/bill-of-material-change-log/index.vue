<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-change-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt物料清单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:manufacturing:bom:billofmaterial:create"
      update-permission="logistics:manufacturing:bom:billofmaterial:update"
      delete-permission="logistics:manufacturing:bom:billofmaterial:delete"
      import-permission="logistics:manufacturing:bom:billofmaterial:import"
      export-permission="logistics:manufacturing:bom:billofmaterial:export"
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
      :master-row-key="getBillOfMaterialId"
      :master-row-selection="rowSelection"
      master-id-column-key="billOfMaterialId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #detail>
        <BillOfMaterialChangeLogPanel
          ref="billOfMaterialChangeLogPanelRef"
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
      <BillOfMaterialForm
        :key="formData?.billOfMaterialId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-bom-bill-of-material-change-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.billofmaterial.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.plantcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomCode')">
      <a-form-item :label="t('entity.billofmaterial.bomcode')">
        <a-input
          v-model:value="advancedQueryForm.bomCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.bomcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomName')">
      <a-form-item :label="t('entity.billofmaterial.bomname')">
        <a-input
          v-model:value="advancedQueryForm.bomName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.bomname') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMaterialId')">
      <a-form-item :label="t('entity.billofmaterial.parentmaterialid')">
        <a-input
          v-model:value="advancedQueryForm.parentMaterialId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.parentmaterialid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMaterialCode')">
      <a-form-item :label="t('entity.billofmaterial.parentmaterialcode')">
        <a-input
          v-model:value="advancedQueryForm.parentMaterialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.parentmaterialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMaterialName')">
      <a-form-item :label="t('entity.billofmaterial.parentmaterialname')">
        <a-input
          v-model:value="advancedQueryForm.parentMaterialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.parentmaterialname') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomVersion')">
      <a-form-item :label="t('entity.billofmaterial.bomversion')">
        <a-input
          v-model:value="advancedQueryForm.bomVersion"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.bomversion') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomType')">
      <a-form-item :label="t('entity.billofmaterial.bomtype')">
        <a-input-number
          v-model:value="advancedQueryForm.bomType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.bomtype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('alternativeBomNumber')">
      <a-form-item :label="t('entity.billofmaterial.alternativebomnumber')">
        <a-input
          v-model:value="advancedQueryForm.alternativeBomNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.alternativebomnumber') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="t('entity.billofmaterial.effectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterial.effectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="t('entity.billofmaterial.effectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterial.effectivedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateStart')">
      <a-form-item :label="t('entity.billofmaterial.expirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterial.expirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateEnd')">
      <a-form-item :label="t('entity.billofmaterial.expirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterial.expirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMaterialUnit')">
      <a-form-item :label="t('entity.billofmaterial.parentmaterialunit')">
        <a-input
          v-model:value="advancedQueryForm.parentMaterialUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.parentmaterialunit') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMaterialQuantity')">
      <a-form-item :label="t('entity.billofmaterial.parentmaterialquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.parentMaterialQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.parentmaterialquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEnabled')">
      <a-form-item :label="t('entity.billofmaterial.isenabled')">
        <a-input-number
          v-model:value="advancedQueryForm.isEnabled"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.isenabled') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomStatus')">
      <a-form-item :label="t('entity.billofmaterial.bomstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.bomStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterial.bomstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomDescription')">
      <a-form-item :label="t('entity.billofmaterial.bomdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.bomDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.billofmaterial.bomdescription') })"
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
      <a-form-item :label="t('entity.billofmaterial.extfield')">
        <a-textarea
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.billofmaterial.extfield') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.billofmaterial._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.billofmaterial._self"
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
      :id-column-key="'billOfMaterialId'"
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
 * Takt物料清单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/bill-of-material-change-log
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import BillOfMaterialForm from './components/bill-of-material-form.vue'
import BillOfMaterialChangeLogPanel from './components/bill-of-material-change-log-panel.vue'
import { provideBillOfMaterialMasterContext } from './composables/use-bill-of-material-master-context'
import { getBillOfMaterialList, getBillOfMaterialById, createBillOfMaterial, updateBillOfMaterial, deleteBillOfMaterialById, deleteBillOfMaterialBatch, getBillOfMaterialTemplate, importBillOfMaterial, exportBillOfMaterial, updateBillOfMaterialStatus } from '@/api/logistics/manufacturing/bom/bill-of-material'
import type { BillOfMaterial, BillOfMaterialQuery } from '@/types/logistics/manufacturing/bom/bill-of-material'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBillOfMaterial')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.billofmaterial._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<BillOfMaterial[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<BillOfMaterial | null>(null)
/** 表格多选行 */
const selectedRows = ref<BillOfMaterial[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<BillOfMaterial> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  bomCode: '',
  bomName: '',
  parentMaterialId: '',
  parentMaterialCode: '',
  parentMaterialName: '',
  bomVersion: '',
  bomType: undefined as number | undefined,
  alternativeBomNumber: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  parentMaterialUnit: '',
  parentMaterialQuantity: undefined as number | undefined,
  isEnabled: undefined as number | undefined,
  bomStatus: undefined as number | undefined,
  bomDescription: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.billofmaterial.plantcode') },
  { key: 'bomCode', label: t('entity.billofmaterial.bomcode') },
  { key: 'bomName', label: t('entity.billofmaterial.bomname') },
  { key: 'parentMaterialId', label: t('entity.billofmaterial.parentmaterialid') },
  { key: 'parentMaterialCode', label: t('entity.billofmaterial.parentmaterialcode') },
  { key: 'parentMaterialName', label: t('entity.billofmaterial.parentmaterialname') },
  { key: 'bomVersion', label: t('entity.billofmaterial.bomversion') },
  { key: 'bomType', label: t('entity.billofmaterial.bomtype') },
  { key: 'alternativeBomNumber', label: t('entity.billofmaterial.alternativebomnumber') },
  { key: 'effectiveDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.billofmaterial.effectivedate')) },
  { key: 'effectiveDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.billofmaterial.effectivedate')) },
  { key: 'expiryDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.billofmaterial.expirydate')) },
  { key: 'expiryDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.billofmaterial.expirydate')) },
  { key: 'parentMaterialUnit', label: t('entity.billofmaterial.parentmaterialunit') },
  { key: 'parentMaterialQuantity', label: t('entity.billofmaterial.parentmaterialquantity') },
  { key: 'isEnabled', label: t('entity.billofmaterial.isenabled') },
  { key: 'bomStatus', label: t('entity.billofmaterial.bomstatus') },
  { key: 'bomDescription', label: t('entity.billofmaterial.bomdescription') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('entity.billofmaterial.extfield') },
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
const entityIdName = 'billOfMaterialId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideBillOfMaterialMasterContext()
const billOfMaterialChangeLogPanelRef = ref<InstanceType<typeof BillOfMaterialChangeLogPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {BillOfMaterialQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<BillOfMaterialQuery>): BillOfMaterialQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: BillOfMaterialQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof BillOfMaterialQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('bomCode', form.bomCode)
  assignTrimmed('bomName', form.bomName)
  assignTrimmed('parentMaterialId', form.parentMaterialId)
  assignTrimmed('parentMaterialCode', form.parentMaterialCode)
  assignTrimmed('parentMaterialName', form.parentMaterialName)
  assignTrimmed('bomVersion', form.bomVersion)
  if (form.bomType !== undefined && form.bomType !== null) {
    query.bomType = form.bomType
  }
  assignTrimmed('alternativeBomNumber', form.alternativeBomNumber)
  assignTrimmed('effectiveDateStart', form.effectiveDateStart)
  assignTrimmed('effectiveDateEnd', form.effectiveDateEnd)
  assignTrimmed('expiryDateStart', form.expiryDateStart)
  assignTrimmed('expiryDateEnd', form.expiryDateEnd)
  assignTrimmed('parentMaterialUnit', form.parentMaterialUnit)
  if (form.parentMaterialQuantity !== undefined && form.parentMaterialQuantity !== null) {
    query.parentMaterialQuantity = form.parentMaterialQuantity
  }
  if (form.isEnabled !== undefined && form.isEnabled !== null) {
    query.isEnabled = form.isEnabled
  }
  if (form.bomStatus !== undefined && form.bomStatus !== null) {
    query.bomStatus = form.bomStatus
  }
  assignTrimmed('bomDescription', form.bomDescription)
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
function syncMasterSelection(record: BillOfMaterial | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getBillOfMaterialId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as BillOfMaterial
  const key = getBillOfMaterialId(row)
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
async function loadBillOfMaterialDetail(record: BillOfMaterial): Promise<BillOfMaterial | null> {
  const id = getBillOfMaterialId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getBillOfMaterialById(id)
    const index = dataSource.value.findIndex((row) => getBillOfMaterialId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as BillOfMaterial
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
    dataIndex: 'billOfMaterialId',
    key: 'billOfMaterialId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'billOfMaterialId') ?? ''
  },
  {
    title: t('entity.billofmaterial.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.billofmaterial.bomcode'),
    dataIndex: 'bomCode',
    key: 'bomCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomCode') ?? ''
  },
  {
    title: t('entity.billofmaterial.bomname'),
    dataIndex: 'bomName',
    key: 'bomName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomName') ?? ''
  },
  {
    title: t('entity.billofmaterial.parentmaterialid'),
    dataIndex: 'parentMaterialId',
    key: 'parentMaterialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'parentMaterialId') ?? ''
  },
  {
    title: t('entity.billofmaterial.parentmaterialcode'),
    dataIndex: 'parentMaterialCode',
    key: 'parentMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'parentMaterialCode') ?? ''
  },
  {
    title: t('entity.billofmaterial.parentmaterialname'),
    dataIndex: 'parentMaterialName',
    key: 'parentMaterialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'parentMaterialName') ?? ''
  },
  {
    title: t('entity.billofmaterial.bomversion'),
    dataIndex: 'bomVersion',
    key: 'bomVersion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomVersion') ?? ''
  },
  {
    title: t('entity.billofmaterial.bomtype'),
    dataIndex: 'bomType',
    key: 'bomType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomType') ?? ''
  },
  {
    title: t('entity.billofmaterial.alternativebomnumber'),
    dataIndex: 'alternativeBomNumber',
    key: 'alternativeBomNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'alternativeBomNumber') ?? ''
  },
  {
    title: t('entity.billofmaterial.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'effectiveDate') ?? ''
  },
  {
    title: t('entity.billofmaterial.expirydate'),
    dataIndex: 'expiryDate',
    key: 'expiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'expiryDate') ?? ''
  },
  {
    title: t('entity.billofmaterial.parentmaterialunit'),
    dataIndex: 'parentMaterialUnit',
    key: 'parentMaterialUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'parentMaterialUnit') ?? ''
  },
  {
    title: t('entity.billofmaterial.parentmaterialquantity'),
    dataIndex: 'parentMaterialQuantity',
    key: 'parentMaterialQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'parentMaterialQuantity') ?? ''
  },
  {
    title: t('entity.billofmaterial.isenabled'),
    dataIndex: 'isEnabled',
    key: 'isEnabled',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'isEnabled') ?? ''
  },
  {
    title: t('entity.billofmaterial.bomstatus'),
    dataIndex: 'bomStatus',
    key: 'bomStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomStatus') ?? ''
  },
  {
    title: t('entity.billofmaterial.bomdescription'),
    dataIndex: 'bomDescription',
    key: 'bomDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomDescription') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:billofmaterial:update',
        onClick: (record: BillOfMaterial) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:billofmaterial:delete',
        onClick: (record: BillOfMaterial) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getBillOfMaterialId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getBillOfMaterialField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: BillOfMaterial[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: BillOfMaterial, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (getBillOfMaterialId(selectedRow.value) === getBillOfMaterialId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: BillOfMaterial[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getBillOfMaterialList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[BillOfMaterial] 加载数据失败', { error })
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
  bomCode: '',
  bomName: '',
  parentMaterialId: '',
  parentMaterialCode: '',
  parentMaterialName: '',
  bomVersion: '',
  bomType: undefined as number | undefined,
  alternativeBomNumber: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  parentMaterialUnit: '',
  parentMaterialQuantity: undefined as number | undefined,
  isEnabled: undefined as number | undefined,
  bomStatus: undefined as number | undefined,
  bomDescription: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.billofmaterial._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: BillOfMaterial) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.billofmaterial._self') })
  formLoading.value = true
  try {
    const detail = await loadBillOfMaterialDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.billofmaterial._self') }))
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
      await updateBillOfMaterial(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.billofmaterial._self') }))
    } else {
      await createBillOfMaterial(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.billofmaterial._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  billOfMaterialChangeLogPanelRef.value?.reload?.()
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
  const res = await getBillOfMaterialTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importBillOfMaterial(file, sheetName)
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
    const exportMeta = await exportBillOfMaterial(
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
    message.success(t('common.feedback.export.success', { target: t('entity.billofmaterial._self') }))
  } catch (error: any) {
    logger.error('[BillOfMaterial] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.billofmaterial._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: BillOfMaterial) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.billofmaterial._self'), name: t('common.tip.this.target', { target: t('entity.billofmaterial._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBillOfMaterialById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.billofmaterial._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.billofmaterial._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.billofmaterial._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteBillOfMaterialBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.billofmaterial._self') }))
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
  bomCode: '',
  bomName: '',
  parentMaterialId: '',
  parentMaterialCode: '',
  parentMaterialName: '',
  bomVersion: '',
  bomType: undefined as number | undefined,
  alternativeBomNumber: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  parentMaterialUnit: '',
  parentMaterialQuantity: undefined as number | undefined,
  isEnabled: undefined as number | undefined,
  bomStatus: undefined as number | undefined,
  bomDescription: '',
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
