<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt物料清单管理页：上主下从（底部明细独立CRUD）+ 弹窗级联（主表与子表 Tab 一次保存） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-bom-bill-of-material flex flex-col min-h-0 h-full">
    <div class="master-section flex flex-col min-h-0 flex-1">
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

    <!-- 表格 -->
    <TaktSingleTable
      :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'billOfMaterialId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getBillOfMaterialId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    />

    <!-- 分页组件 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
    </div>

    <!-- 底部：BOM 明细独立 CRUD（上主下从） -->
    <BillOfMaterialItemPanel ref="itemPanelRef" />

    <!-- 新增/编辑对话框（主表 + 子表级联 Save） -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="1100px"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <a-alert
        type="info"
        show-icon
        class="mb-3"
        :message="t('logistics.manufacturing.bom.bill-of-material.page.modalCascadeHint')"
      />
      <BillOfMaterialForm
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
      :storage-key="'takt-query-fields-logistics-manufacturing-bom-bill-of-material'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.billOfMaterial.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomCode')">
      <a-form-item :label="t('entity.billOfMaterial.bomcode')">
        <a-input
          v-model:value="advancedQueryForm.bomCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomName')">
      <a-form-item :label="t('entity.billOfMaterial.bomname')">
        <a-input
          v-model:value="advancedQueryForm.bomName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMaterialId')">
      <a-form-item :label="t('entity.billOfMaterial.parentmaterialid')">
        <a-input
          v-model:value="advancedQueryForm.parentMaterialId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMaterialCode')">
      <a-form-item :label="t('entity.billOfMaterial.parentmaterialcode')">
        <a-input
          v-model:value="advancedQueryForm.parentMaterialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMaterialName')">
      <a-form-item :label="t('entity.billOfMaterial.parentmaterialname')">
        <a-input
          v-model:value="advancedQueryForm.parentMaterialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomVersion')">
      <a-form-item :label="t('entity.billOfMaterial.bomversion')">
        <a-input
          v-model:value="advancedQueryForm.bomVersion"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomversion') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomType')">
      <a-form-item :label="t('entity.billOfMaterial.bomtype')">
        <a-input-number
          v-model:value="advancedQueryForm.bomType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomtype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('alternativeBomNumber')">
      <a-form-item :label="t('entity.billOfMaterial.alternativebomnumber')">
        <a-input
          v-model:value="advancedQueryForm.alternativeBomNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.alternativebomnumber') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="t('entity.billOfMaterial.effectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.effectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="t('entity.billOfMaterial.effectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.effectivedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateStart')">
      <a-form-item :label="t('entity.billOfMaterial.expirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.expirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateEnd')">
      <a-form-item :label="t('entity.billOfMaterial.expirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.expirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMaterialUnit')">
      <a-form-item :label="t('entity.billOfMaterial.parentmaterialunit')">
        <a-input
          v-model:value="advancedQueryForm.parentMaterialUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialunit') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentMaterialQuantity')">
      <a-form-item :label="t('entity.billOfMaterial.parentmaterialquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.parentMaterialQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEnabled')">
      <a-form-item :label="t('entity.billOfMaterial.isenabled')">
        <a-input-number
          v-model:value="advancedQueryForm.isEnabled"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.isenabled') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomStatus')">
      <a-form-item :label="t('entity.billOfMaterial.bomstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.bomStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomDescription')">
      <a-form-item :label="t('entity.billOfMaterial.bomdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.bomDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.billOfMaterial.bomdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.billOfMaterial.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.sortorder') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.billOfMaterial._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.billOfMaterial._self"
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
 * @module views/logistics/manufacturing/bom/bill-of-material
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import BillOfMaterialForm from './components/bill-of-material-form.vue'
import BillOfMaterialItemPanel from './components/bill-of-material-item-panel.vue'
import { provideBillOfMaterialMasterContext } from './composables/use-bill-of-material-master-context'
import { getBillOfMaterialList, getBillOfMaterialById, createBillOfMaterial, updateBillOfMaterial, deleteBillOfMaterialById, deleteBillOfMaterialBatch, getBillOfMaterialTemplate, importBillOfMaterial, exportBillOfMaterial } from '@/api/logistics/manufacturing/bom/bill-of-material'
import type { BillOfMaterial, BillOfMaterialQuery, BillOfMaterialCreate, BillOfMaterialUpdate } from '@/types/logistics/manufacturing/bom/bill-of-material'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 主表选中行上下文（底部明细面板读取） */
const { selectedMasterRow } = provideBillOfMaterialMasterContext()
/** 底部 BOM 明细面板 ref */
const itemPanelRef = ref<InstanceType<typeof BillOfMaterialItemPanel> | null>(null)
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBillOfMaterial')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.billOfMaterial._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<BillOfMaterial[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
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
const formData = ref<Partial<BillOfMaterial>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
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
  sortOrder: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.billOfMaterial.plantcode') },
  { key: 'bomCode', label: t('entity.billOfMaterial.bomcode') },
  { key: 'bomName', label: t('entity.billOfMaterial.bomname') },
  { key: 'parentMaterialId', label: t('entity.billOfMaterial.parentmaterialid') },
  { key: 'parentMaterialCode', label: t('entity.billOfMaterial.parentmaterialcode') },
  { key: 'parentMaterialName', label: t('entity.billOfMaterial.parentmaterialname') },
  { key: 'bomVersion', label: t('entity.billOfMaterial.bomversion') },
  { key: 'bomType', label: t('entity.billOfMaterial.bomtype') },
  { key: 'alternativeBomNumber', label: t('entity.billOfMaterial.alternativebomnumber') },
  { key: 'effectiveDateStart', label: t('entity.billOfMaterial.effectivedatestart') },
  { key: 'effectiveDateEnd', label: t('entity.billOfMaterial.effectivedateend') },
  { key: 'expiryDateStart', label: t('entity.billOfMaterial.expirydatestart') },
  { key: 'expiryDateEnd', label: t('entity.billOfMaterial.expirydateend') },
  { key: 'parentMaterialUnit', label: t('entity.billOfMaterial.parentmaterialunit') },
  { key: 'parentMaterialQuantity', label: t('entity.billOfMaterial.parentmaterialquantity') },
  { key: 'isEnabled', label: t('entity.billOfMaterial.isenabled') },
  { key: 'bomStatus', label: t('entity.billOfMaterial.bomstatus') },
  { key: 'bomDescription', label: t('entity.billOfMaterial.bomdescription') },
  { key: 'sortOrder', label: t('entity.billOfMaterial.sortorder') },
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
const entityIdName = 'billOfMaterialId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 同步主表选中行到底部明细上下文并刷新子表 */
function syncMasterSelection(record: BillOfMaterial | null) {
  selectedMasterRow.value = record
  itemPanelRef.value?.reload()
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
    title: t('entity.billOfMaterial.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.billOfMaterial.bomcode'),
    dataIndex: 'bomCode',
    key: 'bomCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomCode') ?? ''
  },
  {
    title: t('entity.billOfMaterial.bomname'),
    dataIndex: 'bomName',
    key: 'bomName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomName') ?? ''
  },
  {
    title: t('entity.billOfMaterial.parentmaterialid'),
    dataIndex: 'parentMaterialId',
    key: 'parentMaterialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'parentMaterialId') ?? ''
  },
  {
    title: t('entity.billOfMaterial.parentmaterialcode'),
    dataIndex: 'parentMaterialCode',
    key: 'parentMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'parentMaterialCode') ?? ''
  },
  {
    title: t('entity.billOfMaterial.parentmaterialname'),
    dataIndex: 'parentMaterialName',
    key: 'parentMaterialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'parentMaterialName') ?? ''
  },
  {
    title: t('entity.billOfMaterial.bomversion'),
    dataIndex: 'bomVersion',
    key: 'bomVersion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomVersion') ?? ''
  },
  {
    title: t('entity.billOfMaterial.bomtype'),
    dataIndex: 'bomType',
    key: 'bomType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomType') ?? ''
  },
  {
    title: t('entity.billOfMaterial.alternativebomnumber'),
    dataIndex: 'alternativeBomNumber',
    key: 'alternativeBomNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'alternativeBomNumber') ?? ''
  },
  {
    title: t('entity.billOfMaterial.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'effectiveDate') ?? ''
  },
  {
    title: t('entity.billOfMaterial.expirydate'),
    dataIndex: 'expiryDate',
    key: 'expiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'expiryDate') ?? ''
  },
  {
    title: t('entity.billOfMaterial.parentmaterialunit'),
    dataIndex: 'parentMaterialUnit',
    key: 'parentMaterialUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'parentMaterialUnit') ?? ''
  },
  {
    title: t('entity.billOfMaterial.parentmaterialquantity'),
    dataIndex: 'parentMaterialQuantity',
    key: 'parentMaterialQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'parentMaterialQuantity') ?? ''
  },
  {
    title: t('entity.billOfMaterial.isenabled'),
    dataIndex: 'isEnabled',
    key: 'isEnabled',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'isEnabled') ?? ''
  },
  {
    title: t('entity.billOfMaterial.bomstatus'),
    dataIndex: 'bomStatus',
    key: 'bomStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialField(record, 'bomStatus') ?? ''
  },
  {
    title: t('entity.billOfMaterial.bomdescription'),
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
    syncMasterSelection(rows.length === 1 ? (rows[0] ?? null) : null)
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

/** 行点击：单选当前行并加载底部明细（对齐 Vue.NetCore MES_Bom_Main.rowClick） */
const onClickRow = (record: BillOfMaterial) => ({
  onClick: () => {
    const key = getBillOfMaterialId(record)
    selectedRowKeys.value = [key]
    selectedRows.value = [record]
    selectedRow.value = record
    syncMasterSelection(record)
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: BillOfMaterialQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getBillOfMaterialList(params)
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
  currentPage.value = 1
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
  sortOrder: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.billOfMaterial._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: BillOfMaterial) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.billOfMaterial._self') })
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.billOfMaterial._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.billOfMaterial._self') }))
    } else {
      await createBillOfMaterial(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.billOfMaterial._self') }))
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
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: BillOfMaterialQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportBillOfMaterial(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.billOfMaterial._self') }))
  } catch (error: any) {
    logger.error('[BillOfMaterial] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.billOfMaterial._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: BillOfMaterial) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.billOfMaterial._self'), name: t('common.tip.this.target', { target: t('entity.billOfMaterial._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBillOfMaterialById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.billOfMaterial._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.billOfMaterial._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.billOfMaterial._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteBillOfMaterialBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.billOfMaterial._self') }))
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
  sortOrder: undefined as number | undefined,
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
.logistics-manufacturing-bom-bill-of-material {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}
.master-section {
  min-height: 280px;
}
</style>
