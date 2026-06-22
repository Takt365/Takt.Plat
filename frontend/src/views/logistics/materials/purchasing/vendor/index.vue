<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchasing/vendor -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt经销商实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-materials-purchasing-vendor">
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
      create-permission="logistics:materials:vendor:create"
      update-permission="logistics:materials:vendor:update"
      delete-permission="logistics:materials:vendor:delete"
      import-permission="logistics:materials:vendor:import"
      export-permission="logistics:materials:vendor:export"
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
      :id-column-key="'vendorId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getVendorId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'vendorStatus'">
          <TaktDictTag
            :value="getVendorField(record, 'vendorStatus')"
            dict-type="sys_normal_disable_status"
          />
        </template>
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
      <VendorForm
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
      :storage-key="'takt-query-fields-logistics-materials-purchasing-vendor'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.vendor.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorCode')">
      <a-form-item :label="t('entity.vendor.code')">
        <a-input
          v-model:value="advancedQueryForm.vendorCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorName')">
      <a-form-item :label="t('entity.vendor.name')">
        <a-input
          v-model:value="advancedQueryForm.vendorName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.name') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorShortName')">
      <a-form-item :label="t('entity.vendor.shortname')">
        <a-input
          v-model:value="advancedQueryForm.vendorShortName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.shortname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorType')">
      <a-form-item :label="t('entity.vendor.type')">
        <a-input-number
          v-model:value="advancedQueryForm.vendorType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industrySector')">
      <a-form-item :label="t('entity.vendor.industrysector')">
        <a-input
          v-model:value="advancedQueryForm.industrySector"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.industrysector') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorTaxNumber')">
      <a-form-item :label="t('entity.vendor.taxnumber')">
        <a-input
          v-model:value="advancedQueryForm.vendorTaxNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.taxnumber') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationCountry')">
      <a-form-item :label="t('entity.vendor.registrationcountry')">
        <a-input
          v-model:value="advancedQueryForm.registrationCountry"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.registrationcountry') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress1')">
      <a-form-item :label="t('entity.vendor.registrationaddress1')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress1"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.vendor.registrationaddress1') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress2')">
      <a-form-item :label="t('entity.vendor.registrationaddress2')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress2"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.vendor.registrationaddress2') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress3')">
      <a-form-item :label="t('entity.vendor.registrationaddress3')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress3"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.vendor.registrationaddress3') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorPhone')">
      <a-form-item :label="t('entity.vendor.phone')">
        <a-input
          v-model:value="advancedQueryForm.vendorPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.phone') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorFax')">
      <a-form-item :label="t('entity.vendor.fax')">
        <a-input
          v-model:value="advancedQueryForm.vendorFax"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.fax') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorEmail')">
      <a-form-item :label="t('entity.vendor.email')">
        <a-input
          v-model:value="advancedQueryForm.vendorEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.email') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorWebsite')">
      <a-form-item :label="t('entity.vendor.website')">
        <a-input
          v-model:value="advancedQueryForm.vendorWebsite"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.website') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPerson')">
      <a-form-item :label="t('entity.vendor.contactperson')">
        <a-input
          v-model:value="advancedQueryForm.contactPerson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.contactperson') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPhone')">
      <a-form-item :label="t('entity.vendor.contactphone')">
        <a-input
          v-model:value="advancedQueryForm.contactPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.contactphone') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactEmail')">
      <a-form-item :label="t('entity.vendor.contactemail')">
        <a-input
          v-model:value="advancedQueryForm.contactEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.contactemail') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="t('entity.vendor.currencycode')">
        <a-input
          v-model:value="advancedQueryForm.currencyCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.currencycode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentTerms')">
      <a-form-item :label="t('entity.vendor.paymentterms')">
        <a-input-number
          v-model:value="advancedQueryForm.paymentTerms"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.paymentterms') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('creditLevel')">
      <a-form-item :label="t('entity.vendor.creditlevel')">
        <a-input-number
          v-model:value="advancedQueryForm.creditLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.creditlevel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('creditAmount')">
      <a-form-item :label="t('entity.vendor.creditamount')">
        <a-input-number
          v-model:value="advancedQueryForm.creditAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.creditamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('authorizedBrand')">
      <a-form-item :label="t('entity.vendor.authorizedbrand')">
        <a-input
          v-model:value="advancedQueryForm.authorizedBrand"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.authorizedbrand') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('agentRegion')">
      <a-form-item :label="t('entity.vendor.agentregion')">
        <a-input
          v-model:value="advancedQueryForm.agentRegion"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.agentregion') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorLevel')">
      <a-form-item :label="t('entity.vendor.level')">
        <a-input-number
          v-model:value="advancedQueryForm.vendorLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.level') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationScore')">
      <a-form-item :label="t('entity.vendor.evaluationscore')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.evaluationscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isQualified')">
      <a-form-item :label="t('entity.vendor.isqualified')">
        <a-input-number
          v-model:value="advancedQueryForm.isQualified"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.isqualified') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorStatus')">
      <a-form-item :label="t('entity.vendor.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.vendorStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.vendor.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.vendor.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.vendor.sortorder') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.vendor._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.vendor._self"
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
      :id-column-key="'vendorId'"
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
 * Takt经销商实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/purchasing/vendor
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import VendorForm from './components/vendor-form.vue'
import { getVendorList, getVendorById, createVendor, updateVendor, deleteVendorById, deleteVendorBatch, getVendorTemplate, importVendor, exportVendor } from '@/api/logistics/materials/vendor'
import type { Vendor, VendorQuery, VendorCreate, VendorUpdate } from '@/types/logistics/materials/vendor'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktVendor')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.vendor._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Vendor[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Vendor | null>(null)
/** 表格多选行 */
const selectedRows = ref<Vendor[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Vendor>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  vendorCode: '',
  vendorName: '',
  vendorShortName: '',
  vendorType: undefined as number | undefined,
  industrySector: '',
  vendorTaxNumber: '',
  registrationCountry: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  vendorPhone: '',
  vendorFax: '',
  vendorEmail: '',
  vendorWebsite: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  currencyCode: '',
  paymentTerms: undefined as number | undefined,
  creditLevel: undefined as number | undefined,
  creditAmount: undefined as number | undefined,
  authorizedBrand: '',
  agentRegion: '',
  vendorLevel: undefined as number | undefined,
  evaluationScore: undefined as number | undefined,
  isQualified: undefined as number | undefined,
  vendorStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.vendor.plantcode') },
  { key: 'vendorCode', label: t('entity.vendor.code') },
  { key: 'vendorName', label: t('entity.vendor.name') },
  { key: 'vendorShortName', label: t('entity.vendor.shortname') },
  { key: 'vendorType', label: t('entity.vendor.type') },
  { key: 'industrySector', label: t('entity.vendor.industrysector') },
  { key: 'vendorTaxNumber', label: t('entity.vendor.taxnumber') },
  { key: 'registrationCountry', label: t('entity.vendor.registrationcountry') },
  { key: 'registrationAddress1', label: t('entity.vendor.registrationaddress1') },
  { key: 'registrationAddress2', label: t('entity.vendor.registrationaddress2') },
  { key: 'registrationAddress3', label: t('entity.vendor.registrationaddress3') },
  { key: 'vendorPhone', label: t('entity.vendor.phone') },
  { key: 'vendorFax', label: t('entity.vendor.fax') },
  { key: 'vendorEmail', label: t('entity.vendor.email') },
  { key: 'vendorWebsite', label: t('entity.vendor.website') },
  { key: 'contactPerson', label: t('entity.vendor.contactperson') },
  { key: 'contactPhone', label: t('entity.vendor.contactphone') },
  { key: 'contactEmail', label: t('entity.vendor.contactemail') },
  { key: 'currencyCode', label: t('entity.vendor.currencycode') },
  { key: 'paymentTerms', label: t('entity.vendor.paymentterms') },
  { key: 'creditLevel', label: t('entity.vendor.creditlevel') },
  { key: 'creditAmount', label: t('entity.vendor.creditamount') },
  { key: 'authorizedBrand', label: t('entity.vendor.authorizedbrand') },
  { key: 'agentRegion', label: t('entity.vendor.agentregion') },
  { key: 'vendorLevel', label: t('entity.vendor.level') },
  { key: 'evaluationScore', label: t('entity.vendor.evaluationscore') },
  { key: 'isQualified', label: t('entity.vendor.isqualified') },
  { key: 'vendorStatus', label: t('entity.vendor.status') },
  { key: 'sortOrder', label: t('entity.vendor.sortorder') },
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
const entityIdName = 'vendorId'
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
    dataIndex: 'vendorId',
    key: 'vendorId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorId') ?? ''
  },
  {
    title: t('entity.vendor.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.vendor.code'),
    dataIndex: 'vendorCode',
    key: 'vendorCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorCode') ?? ''
  },
  {
    title: t('entity.vendor.name'),
    dataIndex: 'vendorName',
    key: 'vendorName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorName') ?? ''
  },
  {
    title: t('entity.vendor.shortname'),
    dataIndex: 'vendorShortName',
    key: 'vendorShortName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorShortName') ?? ''
  },
  {
    title: t('entity.vendor.type'),
    dataIndex: 'vendorType',
    key: 'vendorType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorType') ?? ''
  },
  {
    title: t('entity.vendor.industrysector'),
    dataIndex: 'industrySector',
    key: 'industrySector',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'industrySector') ?? ''
  },
  {
    title: t('entity.vendor.taxnumber'),
    dataIndex: 'vendorTaxNumber',
    key: 'vendorTaxNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorTaxNumber') ?? ''
  },
  {
    title: t('entity.vendor.registrationcountry'),
    dataIndex: 'registrationCountry',
    key: 'registrationCountry',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'registrationCountry') ?? ''
  },
  {
    title: t('entity.vendor.registrationaddress1'),
    dataIndex: 'registrationAddress1',
    key: 'registrationAddress1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'registrationAddress1') ?? ''
  },
  {
    title: t('entity.vendor.registrationaddress2'),
    dataIndex: 'registrationAddress2',
    key: 'registrationAddress2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'registrationAddress2') ?? ''
  },
  {
    title: t('entity.vendor.registrationaddress3'),
    dataIndex: 'registrationAddress3',
    key: 'registrationAddress3',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'registrationAddress3') ?? ''
  },
  {
    title: t('entity.vendor.phone'),
    dataIndex: 'vendorPhone',
    key: 'vendorPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorPhone') ?? ''
  },
  {
    title: t('entity.vendor.fax'),
    dataIndex: 'vendorFax',
    key: 'vendorFax',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorFax') ?? ''
  },
  {
    title: t('entity.vendor.email'),
    dataIndex: 'vendorEmail',
    key: 'vendorEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorEmail') ?? ''
  },
  {
    title: t('entity.vendor.website'),
    dataIndex: 'vendorWebsite',
    key: 'vendorWebsite',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorWebsite') ?? ''
  },
  {
    title: t('entity.vendor.contactperson'),
    dataIndex: 'contactPerson',
    key: 'contactPerson',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'contactPerson') ?? ''
  },
  {
    title: t('entity.vendor.contactphone'),
    dataIndex: 'contactPhone',
    key: 'contactPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'contactPhone') ?? ''
  },
  {
    title: t('entity.vendor.contactemail'),
    dataIndex: 'contactEmail',
    key: 'contactEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'contactEmail') ?? ''
  },
  {
    title: t('entity.vendor.currencycode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'currencyCode') ?? ''
  },
  {
    title: t('entity.vendor.paymentterms'),
    dataIndex: 'paymentTerms',
    key: 'paymentTerms',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'paymentTerms') ?? ''
  },
  {
    title: t('entity.vendor.creditlevel'),
    dataIndex: 'creditLevel',
    key: 'creditLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'creditLevel') ?? ''
  },
  {
    title: t('entity.vendor.creditamount'),
    dataIndex: 'creditAmount',
    key: 'creditAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'creditAmount') ?? ''
  },
  {
    title: t('entity.vendor.authorizedbrand'),
    dataIndex: 'authorizedBrand',
    key: 'authorizedBrand',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'authorizedBrand') ?? ''
  },
  {
    title: t('entity.vendor.agentregion'),
    dataIndex: 'agentRegion',
    key: 'agentRegion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'agentRegion') ?? ''
  },
  {
    title: t('entity.vendor.level'),
    dataIndex: 'vendorLevel',
    key: 'vendorLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'vendorLevel') ?? ''
  },
  {
    title: t('entity.vendor.evaluationscore'),
    dataIndex: 'evaluationScore',
    key: 'evaluationScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'evaluationScore') ?? ''
  },
  {
    title: t('entity.vendor.isqualified'),
    dataIndex: 'isQualified',
    key: 'isQualified',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getVendorField(record, 'isQualified') ?? ''
  },
  {
    title: t('entity.vendor.status'),
    dataIndex: 'vendorStatus',
    key: 'vendorStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:vendor:update',
        onClick: (record: Vendor) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:vendor:delete',
        onClick: (record: Vendor) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getVendorId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getVendorField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Vendor[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Vendor, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getVendorId(selectedRow.value) === getVendorId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Vendor[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Vendor) => ({
  onClick: () => {
    const key = getVendorId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getVendorId(item)))
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
    const params: VendorQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getVendorList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Vendor] 加载数据失败', { error })
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
  vendorCode: '',
  vendorName: '',
  vendorShortName: '',
  vendorType: undefined as number | undefined,
  industrySector: '',
  vendorTaxNumber: '',
  registrationCountry: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  vendorPhone: '',
  vendorFax: '',
  vendorEmail: '',
  vendorWebsite: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  currencyCode: '',
  paymentTerms: undefined as number | undefined,
  creditLevel: undefined as number | undefined,
  creditAmount: undefined as number | undefined,
  authorizedBrand: '',
  agentRegion: '',
  vendorLevel: undefined as number | undefined,
  evaluationScore: undefined as number | undefined,
  isQualified: undefined as number | undefined,
  vendorStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.vendor._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: Vendor) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.vendor._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.vendor._self') }))
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
      await updateVendor(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.vendor._self') }))
    } else {
      await createVendor(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.vendor._self') }))
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
  const res = await getVendorTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importVendor(file, sheetName)
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
    const exportQuery: VendorQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportVendor(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.vendor._self') }))
  } catch (error: any) {
    logger.error('[Vendor] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.vendor._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Vendor) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.vendor._self'), name: t('common.tip.this.target', { target: t('entity.vendor._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteVendorById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.vendor._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.vendor._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.vendor._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteVendorBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.vendor._self') }))
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
  vendorCode: '',
  vendorName: '',
  vendorShortName: '',
  vendorType: undefined as number | undefined,
  industrySector: '',
  vendorTaxNumber: '',
  registrationCountry: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  vendorPhone: '',
  vendorFax: '',
  vendorEmail: '',
  vendorWebsite: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  currencyCode: '',
  paymentTerms: undefined as number | undefined,
  creditLevel: undefined as number | undefined,
  creditAmount: undefined as number | undefined,
  authorizedBrand: '',
  agentRegion: '',
  vendorLevel: undefined as number | undefined,
  evaluationScore: undefined as number | undefined,
  isQualified: undefined as number | undefined,
  vendorStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
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
.logistics-materials-purchasing-vendor {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
