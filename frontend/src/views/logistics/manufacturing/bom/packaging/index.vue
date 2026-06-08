<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/packaging -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt物料包装信息实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-bom-packaging">
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
      create-permission="logistics:manufacturing:bom:packaging:create"
      update-permission="logistics:manufacturing:bom:packaging:update"
      delete-permission="logistics:manufacturing:bom:packaging:delete"
      import-permission="logistics:manufacturing:bom:packaging:import"
      export-permission="logistics:manufacturing:bom:packaging:export"
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
      :id-column-key="'packagingId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPackagingId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

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
      <PackagingForm
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
      :storage-key="'takt-query-fields-logistics-manufacturing-bom-packaging'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.packaging.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.packaging.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.materialcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('hsCode')">
      <a-form-item :label="t('entity.packaging.hscode')">
        <a-input
          v-model:value="advancedQueryForm.hsCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.hscode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('hsName')">
      <a-form-item :label="t('entity.packaging.hsname')">
        <a-input
          v-model:value="advancedQueryForm.hsName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.hsname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('additionalCode')">
      <a-form-item :label="t('entity.packaging.additionalcode')">
        <a-input
          v-model:value="advancedQueryForm.additionalCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.additionalcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('originCountryRegionCode')">
      <a-form-item :label="t('entity.packaging.origincountryregioncode')">
        <a-input
          v-model:value="advancedQueryForm.originCountryRegionCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.origincountryregioncode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('originCountryRegionName')">
      <a-form-item :label="t('entity.packaging.origincountryregionname')">
        <a-input
          v-model:value="advancedQueryForm.originCountryRegionName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.origincountryregionname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('destinationCountryRegionCode')">
      <a-form-item :label="t('entity.packaging.destinationcountryregioncode')">
        <a-input
          v-model:value="advancedQueryForm.destinationCountryRegionCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.destinationcountryregioncode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('destinationCountryRegionName')">
      <a-form-item :label="t('entity.packaging.destinationcountryregionname')">
        <a-input
          v-model:value="advancedQueryForm.destinationCountryRegionName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.destinationcountryregionname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('regulatoryConditionCode')">
      <a-form-item :label="t('entity.packaging.regulatoryconditioncode')">
        <a-input
          v-model:value="advancedQueryForm.regulatoryConditionCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.regulatoryconditioncode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tariffRateType')">
      <a-form-item :label="t('entity.packaging.tariffratetype')">
        <a-input
          v-model:value="advancedQueryForm.tariffRateType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.tariffratetype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('grossWeight')">
      <a-form-item :label="t('entity.packaging.grossweight')">
        <a-input-number
          v-model:value="advancedQueryForm.grossWeight"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.grossweight') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('netWeight')">
      <a-form-item :label="t('entity.packaging.netweight')">
        <a-input-number
          v-model:value="advancedQueryForm.netWeight"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.netweight') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weightUnit')">
      <a-form-item :label="t('entity.packaging.weightunit')">
        <a-input
          v-model:value="advancedQueryForm.weightUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.weightunit') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessVolume')">
      <a-form-item :label="t('entity.packaging.businessvolume')">
        <a-input-number
          v-model:value="advancedQueryForm.businessVolume"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.businessvolume') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('volumeUnit')">
      <a-form-item :label="t('entity.packaging.volumeunit')">
        <a-input
          v-model:value="advancedQueryForm.volumeUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.volumeunit') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sizeDimension')">
      <a-form-item :label="t('entity.packaging.sizedimension')">
        <a-input
          v-model:value="advancedQueryForm.sizeDimension"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.sizedimension') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('packagingType')">
      <a-form-item :label="t('entity.packaging.type')">
        <a-input
          v-model:value="advancedQueryForm.packagingType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.type') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('packingUnit')">
      <a-form-item :label="t('entity.packaging.packingunit')">
        <a-input
          v-model:value="advancedQueryForm.packingUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.packingunit') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quantityPerPacking')">
      <a-form-item :label="t('entity.packaging.quantityperpacking')">
        <a-input-number
          v-model:value="advancedQueryForm.quantityPerPacking"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.quantityperpacking') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('packagingSpec')">
      <a-form-item :label="t('entity.packaging.spec')">
        <a-input
          v-model:value="advancedQueryForm.packagingSpec"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.spec') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('packagingDescription')">
      <a-form-item :label="t('entity.packaging.description')">
        <a-textarea
          v-model:value="advancedQueryForm.packagingDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.packaging.description') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.packaging.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.sortorder') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.packaging._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.packaging._self"
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
      :id-column-key="'packagingId'"
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
 * Takt物料包装信息实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/packaging
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import PackagingForm from './components/packaging-form.vue'
import { getPackagingList, getPackagingById, createPackaging, updatePackaging, deletePackagingById, deletePackagingBatch, getPackagingTemplate, importPackaging, exportPackaging } from '@/api/logistics/manufacturing/bom/packaging'
import type { Packaging, PackagingQuery, PackagingCreate, PackagingUpdate } from '@/types/logistics/manufacturing/bom/packaging'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPackaging')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.packaging._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Packaging[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Packaging | null>(null)
/** 表格多选行 */
const selectedRows = ref<Packaging[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Packaging>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  materialCode: '',
  hsCode: '',
  hsName: '',
  additionalCode: '',
  originCountryRegionCode: '',
  originCountryRegionName: '',
  destinationCountryRegionCode: '',
  destinationCountryRegionName: '',
  regulatoryConditionCode: '',
  tariffRateType: '',
  grossWeight: undefined as number | undefined,
  netWeight: undefined as number | undefined,
  weightUnit: '',
  businessVolume: undefined as number | undefined,
  volumeUnit: '',
  sizeDimension: '',
  packagingType: '',
  packingUnit: '',
  quantityPerPacking: undefined as number | undefined,
  packagingSpec: '',
  packagingDescription: '',
  sortOrder: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.packaging.plantcode') },
  { key: 'materialCode', label: t('entity.packaging.materialcode') },
  { key: 'hsCode', label: t('entity.packaging.hscode') },
  { key: 'hsName', label: t('entity.packaging.hsname') },
  { key: 'additionalCode', label: t('entity.packaging.additionalcode') },
  { key: 'originCountryRegionCode', label: t('entity.packaging.origincountryregioncode') },
  { key: 'originCountryRegionName', label: t('entity.packaging.origincountryregionname') },
  { key: 'destinationCountryRegionCode', label: t('entity.packaging.destinationcountryregioncode') },
  { key: 'destinationCountryRegionName', label: t('entity.packaging.destinationcountryregionname') },
  { key: 'regulatoryConditionCode', label: t('entity.packaging.regulatoryconditioncode') },
  { key: 'tariffRateType', label: t('entity.packaging.tariffratetype') },
  { key: 'grossWeight', label: t('entity.packaging.grossweight') },
  { key: 'netWeight', label: t('entity.packaging.netweight') },
  { key: 'weightUnit', label: t('entity.packaging.weightunit') },
  { key: 'businessVolume', label: t('entity.packaging.businessvolume') },
  { key: 'volumeUnit', label: t('entity.packaging.volumeunit') },
  { key: 'sizeDimension', label: t('entity.packaging.sizedimension') },
  { key: 'packagingType', label: t('entity.packaging.type') },
  { key: 'packingUnit', label: t('entity.packaging.packingunit') },
  { key: 'quantityPerPacking', label: t('entity.packaging.quantityperpacking') },
  { key: 'packagingSpec', label: t('entity.packaging.spec') },
  { key: 'packagingDescription', label: t('entity.packaging.description') },
  { key: 'sortOrder', label: t('entity.packaging.sortorder') },
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
const entityIdName = 'packagingId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)


/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})






/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'packagingId',
    key: 'packagingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'packagingId') ?? ''
  },
  {
    title: t('entity.packaging.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.packaging.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.packaging.hscode'),
    dataIndex: 'hsCode',
    key: 'hsCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'hsCode') ?? ''
  },
  {
    title: t('entity.packaging.hsname'),
    dataIndex: 'hsName',
    key: 'hsName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'hsName') ?? ''
  },
  {
    title: t('entity.packaging.additionalcode'),
    dataIndex: 'additionalCode',
    key: 'additionalCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'additionalCode') ?? ''
  },
  {
    title: t('entity.packaging.origincountryregioncode'),
    dataIndex: 'originCountryRegionCode',
    key: 'originCountryRegionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'originCountryRegionCode') ?? ''
  },
  {
    title: t('entity.packaging.origincountryregionname'),
    dataIndex: 'originCountryRegionName',
    key: 'originCountryRegionName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'originCountryRegionName') ?? ''
  },
  {
    title: t('entity.packaging.destinationcountryregioncode'),
    dataIndex: 'destinationCountryRegionCode',
    key: 'destinationCountryRegionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'destinationCountryRegionCode') ?? ''
  },
  {
    title: t('entity.packaging.destinationcountryregionname'),
    dataIndex: 'destinationCountryRegionName',
    key: 'destinationCountryRegionName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'destinationCountryRegionName') ?? ''
  },
  {
    title: t('entity.packaging.regulatoryconditioncode'),
    dataIndex: 'regulatoryConditionCode',
    key: 'regulatoryConditionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'regulatoryConditionCode') ?? ''
  },
  {
    title: t('entity.packaging.tariffratetype'),
    dataIndex: 'tariffRateType',
    key: 'tariffRateType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'tariffRateType') ?? ''
  },
  {
    title: t('entity.packaging.grossweight'),
    dataIndex: 'grossWeight',
    key: 'grossWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'grossWeight') ?? ''
  },
  {
    title: t('entity.packaging.netweight'),
    dataIndex: 'netWeight',
    key: 'netWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'netWeight') ?? ''
  },
  {
    title: t('entity.packaging.weightunit'),
    dataIndex: 'weightUnit',
    key: 'weightUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'weightUnit') ?? ''
  },
  {
    title: t('entity.packaging.businessvolume'),
    dataIndex: 'businessVolume',
    key: 'businessVolume',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'businessVolume') ?? ''
  },
  {
    title: t('entity.packaging.volumeunit'),
    dataIndex: 'volumeUnit',
    key: 'volumeUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'volumeUnit') ?? ''
  },
  {
    title: t('entity.packaging.sizedimension'),
    dataIndex: 'sizeDimension',
    key: 'sizeDimension',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'sizeDimension') ?? ''
  },
  {
    title: t('entity.packaging.type'),
    dataIndex: 'packagingType',
    key: 'packagingType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'packagingType') ?? ''
  },
  {
    title: t('entity.packaging.packingunit'),
    dataIndex: 'packingUnit',
    key: 'packingUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'packingUnit') ?? ''
  },
  {
    title: t('entity.packaging.quantityperpacking'),
    dataIndex: 'quantityPerPacking',
    key: 'quantityPerPacking',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'quantityPerPacking') ?? ''
  },
  {
    title: t('entity.packaging.spec'),
    dataIndex: 'packagingSpec',
    key: 'packagingSpec',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'packagingSpec') ?? ''
  },
  {
    title: t('entity.packaging.description'),
    dataIndex: 'packagingDescription',
    key: 'packagingDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPackagingField(record, 'packagingDescription') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:packaging:update',
        onClick: (record: Packaging) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:packaging:delete',
        onClick: (record: Packaging) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPackagingId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPackagingField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Packaging[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Packaging, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPackagingId(selectedRow.value) === getPackagingId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Packaging[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Packaging) => ({
  onClick: () => {
    const key = getPackagingId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPackagingId(item)))
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
    const params: PackagingQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPackagingList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Packaging] 加载数据失败', { error })
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
  materialCode: '',
  hsCode: '',
  hsName: '',
  additionalCode: '',
  originCountryRegionCode: '',
  originCountryRegionName: '',
  destinationCountryRegionCode: '',
  destinationCountryRegionName: '',
  regulatoryConditionCode: '',
  tariffRateType: '',
  grossWeight: undefined as number | undefined,
  netWeight: undefined as number | undefined,
  weightUnit: '',
  businessVolume: undefined as number | undefined,
  volumeUnit: '',
  sizeDimension: '',
  packagingType: '',
  packingUnit: '',
  quantityPerPacking: undefined as number | undefined,
  packagingSpec: '',
  packagingDescription: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.packaging._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: Packaging) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.packaging._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.packaging._self') }))
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
      await updatePackaging(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.packaging._self') }))
    } else {
      await createPackaging(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.packaging._self') }))
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
  const res = await getPackagingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPackaging(file, sheetName)
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
    const exportQuery: PackagingQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPackaging(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.packaging._self') }))
  } catch (error: any) {
    logger.error('[Packaging] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.packaging._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Packaging) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.packaging._self'), name: t('common.tip.this.target', { target: t('entity.packaging._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePackagingById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.packaging._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.packaging._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.packaging._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePackagingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.packaging._self') }))
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
  materialCode: '',
  hsCode: '',
  hsName: '',
  additionalCode: '',
  originCountryRegionCode: '',
  originCountryRegionName: '',
  destinationCountryRegionCode: '',
  destinationCountryRegionName: '',
  regulatoryConditionCode: '',
  tariffRateType: '',
  grossWeight: undefined as number | undefined,
  netWeight: undefined as number | undefined,
  weightUnit: '',
  businessVolume: undefined as number | undefined,
  volumeUnit: '',
  sizeDimension: '',
  packagingType: '',
  packingUnit: '',
  quantityPerPacking: undefined as number | undefined,
  packagingSpec: '',
  packagingDescription: '',
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
.logistics-manufacturing-bom-packaging {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
