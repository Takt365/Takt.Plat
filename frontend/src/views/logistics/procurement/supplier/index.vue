<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/supplier -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt供货商实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
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
      create-permission="logistics:procurement:supplier:create"
      update-permission="logistics:procurement:supplier:update"
      delete-permission="logistics:procurement:supplier:delete"
      import-permission="logistics:procurement:supplier:import"
      export-permission="logistics:procurement:supplier:export"
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
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'supplierId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSupplierId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'supplierStatus'">
          <a-switch
            :checked="getSupplierField(record, 'supplierStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleSupplierStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'supplierType'">
          <TaktDictTag
            :value="getSupplierField(record, 'supplierType')"
            dict-type="logistics_supplier_category"
          />
        </template>
        <template v-else-if="column.key === 'paymentTerms'">
          <TaktDictTag
            :value="getSupplierField(record, 'paymentTerms')"
            dict-type="logistics_payment_terms_param"
          />
        </template>
        <template v-else-if="column.key === 'supplierLevel'">
          <TaktDictTag
            :value="getSupplierField(record, 'supplierLevel')"
            dict-type="logistics_grade_category"
          />
        </template>
      </template>

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
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
      <SupplierForm
        :key="formData?.supplierId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-procurement-supplier'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.supplier.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="t('entity.supplier.code')">
        <a-input
          v-model:value="advancedQueryForm.supplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.code') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierName')">
      <a-form-item :label="t('entity.supplier.name')">
        <a-input
          v-model:value="advancedQueryForm.supplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.name') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierShortName')">
      <a-form-item :label="t('entity.supplier.shortname')">
        <a-input
          v-model:value="advancedQueryForm.supplierShortName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.shortname') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierType')">
      <a-form-item :label="t('entity.supplier.type')">
        <TaktSelect
          v-model:value="advancedQueryForm.supplierType"
          dict-type="logistics_supplier_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplier.type') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industrySector')">
      <a-form-item :label="t('entity.supplier.industrysector')">
        <a-input
          v-model:value="advancedQueryForm.industrySector"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.industrysector') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierTaxNumber')">
      <a-form-item :label="t('entity.supplier.taxnumber')">
        <a-input
          v-model:value="advancedQueryForm.supplierTaxNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.taxnumber') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationCountry')">
      <a-form-item :label="t('entity.supplier.registrationcountry')">
        <a-input
          v-model:value="advancedQueryForm.registrationCountry"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.registrationcountry') })"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress1')">
      <a-form-item :label="t('entity.supplier.registrationaddress1')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress1"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.supplier.registrationaddress1') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress2')">
      <a-form-item :label="t('entity.supplier.registrationaddress2')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress2"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.supplier.registrationaddress2') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress3')">
      <a-form-item :label="t('entity.supplier.registrationaddress3')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress3"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.supplier.registrationaddress3') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierPhone')">
      <a-form-item :label="t('entity.supplier.phone')">
        <a-input
          v-model:value="advancedQueryForm.supplierPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.phone') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierFax')">
      <a-form-item :label="t('entity.supplier.fax')">
        <a-input
          v-model:value="advancedQueryForm.supplierFax"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.fax') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierEmail')">
      <a-form-item :label="t('entity.supplier.email')">
        <a-input
          v-model:value="advancedQueryForm.supplierEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.email') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierWebsite')">
      <a-form-item :label="t('entity.supplier.website')">
        <a-input
          v-model:value="advancedQueryForm.supplierWebsite"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.website') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPerson')">
      <a-form-item :label="t('entity.supplier.contactperson')">
        <a-input
          v-model:value="advancedQueryForm.contactPerson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.contactperson') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPhone')">
      <a-form-item :label="t('entity.supplier.contactphone')">
        <a-input
          v-model:value="advancedQueryForm.contactPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.contactphone') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactEmail')">
      <a-form-item :label="t('entity.supplier.contactemail')">
        <a-input
          v-model:value="advancedQueryForm.contactEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.contactemail') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="t('entity.supplier.currencycode')">
        <a-input
          v-model:value="advancedQueryForm.currencyCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.currencycode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentTerms')">
      <a-form-item :label="t('entity.supplier.paymentterms')">
        <TaktSelect
          v-model:value="advancedQueryForm.paymentTerms"
          dict-type="logistics_payment_terms_param"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplier.paymentterms') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierLevel')">
      <a-form-item :label="t('entity.supplier.level')">
        <TaktSelect
          v-model:value="advancedQueryForm.supplierLevel"
          dict-type="logistics_grade_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplier.level') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationScore')">
      <a-form-item :label="t('entity.supplier.evaluationscore')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.evaluationscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isQualified')">
      <a-form-item :label="t('entity.supplier.isqualified')">
        <a-input-number
          v-model:value="advancedQueryForm.isQualified"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplier.isqualified') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierStatus')">
      <a-form-item :label="t('entity.supplier.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.supplierStatus"
          dict-type="sys_normal_disable_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplier.status') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.supplier._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.supplier._self"
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
      :id-column-key="'supplierId'"
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
 * Takt供货商实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/supplier
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import SupplierForm from './components/supplier-form.vue'
import { getSupplierList, getSupplierById, createSupplier, updateSupplier, deleteSupplierById, deleteSupplierBatch, getSupplierTemplate, importSupplier, exportSupplier, updateSupplierStatus } from '@/api/logistics/procurement/supplier'
import type { Supplier, SupplierQuery } from '@/types/logistics/procurement/supplier'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSupplier')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.supplier._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Supplier[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Supplier | null>(null)
/** 表格多选行 */
const selectedRows = ref<Supplier[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Supplier> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  supplierCode: '',
  supplierName: '',
  supplierShortName: '',
  supplierType: undefined as number | undefined,
  industrySector: '',
  supplierTaxNumber: '',
  registrationCountry: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  supplierPhone: '',
  supplierFax: '',
  supplierEmail: '',
  supplierWebsite: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  currencyCode: '',
  paymentTerms: undefined as number | undefined,
  supplierLevel: undefined as number | undefined,
  evaluationScore: undefined as number | undefined,
  isQualified: undefined as number | undefined,
  supplierStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.supplier.plantcode') },
  { key: 'supplierCode', label: t('entity.supplier.code') },
  { key: 'supplierName', label: t('entity.supplier.name') },
  { key: 'supplierShortName', label: t('entity.supplier.shortname') },
  { key: 'supplierType', label: t('entity.supplier.type') },
  { key: 'industrySector', label: t('entity.supplier.industrysector') },
  { key: 'supplierTaxNumber', label: t('entity.supplier.taxnumber') },
  { key: 'registrationCountry', label: t('entity.supplier.registrationcountry') },
  { key: 'registrationAddress1', label: t('entity.supplier.registrationaddress1') },
  { key: 'registrationAddress2', label: t('entity.supplier.registrationaddress2') },
  { key: 'registrationAddress3', label: t('entity.supplier.registrationaddress3') },
  { key: 'supplierPhone', label: t('entity.supplier.phone') },
  { key: 'supplierFax', label: t('entity.supplier.fax') },
  { key: 'supplierEmail', label: t('entity.supplier.email') },
  { key: 'supplierWebsite', label: t('entity.supplier.website') },
  { key: 'contactPerson', label: t('entity.supplier.contactperson') },
  { key: 'contactPhone', label: t('entity.supplier.contactphone') },
  { key: 'contactEmail', label: t('entity.supplier.contactemail') },
  { key: 'currencyCode', label: t('entity.supplier.currencycode') },
  { key: 'paymentTerms', label: t('entity.supplier.paymentterms') },
  { key: 'supplierLevel', label: t('entity.supplier.level') },
  { key: 'evaluationScore', label: t('entity.supplier.evaluationscore') },
  { key: 'isQualified', label: t('entity.supplier.isqualified') },
  { key: 'supplierStatus', label: t('entity.supplier.status') },
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
const entityIdName = 'supplierId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()


/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SupplierQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SupplierQuery>): SupplierQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SupplierQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SupplierQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('supplierCode', form.supplierCode)
  assignTrimmed('supplierName', form.supplierName)
  assignTrimmed('supplierShortName', form.supplierShortName)
  if (form.supplierType !== undefined && form.supplierType !== null) {
    query.supplierType = form.supplierType
  }
  assignTrimmed('industrySector', form.industrySector)
  assignTrimmed('supplierTaxNumber', form.supplierTaxNumber)
  assignTrimmed('registrationCountry', form.registrationCountry)
  assignTrimmed('registrationAddress1', form.registrationAddress1)
  assignTrimmed('registrationAddress2', form.registrationAddress2)
  assignTrimmed('registrationAddress3', form.registrationAddress3)
  assignTrimmed('supplierPhone', form.supplierPhone)
  assignTrimmed('supplierFax', form.supplierFax)
  assignTrimmed('supplierEmail', form.supplierEmail)
  assignTrimmed('supplierWebsite', form.supplierWebsite)
  assignTrimmed('contactPerson', form.contactPerson)
  assignTrimmed('contactPhone', form.contactPhone)
  assignTrimmed('contactEmail', form.contactEmail)
  assignTrimmed('currencyCode', form.currencyCode)
  if (form.paymentTerms !== undefined && form.paymentTerms !== null) {
    query.paymentTerms = form.paymentTerms
  }
  if (form.supplierLevel !== undefined && form.supplierLevel !== null) {
    query.supplierLevel = form.supplierLevel
  }
  if (form.evaluationScore !== undefined && form.evaluationScore !== null) {
    query.evaluationScore = form.evaluationScore
  }
  if (form.isQualified !== undefined && form.isQualified !== null) {
    query.isQualified = form.isQualified
  }
  if (form.supplierStatus !== undefined && form.supplierStatus !== null) {
    query.supplierStatus = form.supplierStatus
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







/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'supplierId',
    key: 'supplierId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'supplierId') ?? ''
  },
  {
    title: t('entity.supplier.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.supplier.code'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.supplier.name'),
    dataIndex: 'supplierName',
    key: 'supplierName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'supplierName') ?? ''
  },
  {
    title: t('entity.supplier.shortname'),
    dataIndex: 'supplierShortName',
    key: 'supplierShortName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'supplierShortName') ?? ''
  },
  {
    title: t('entity.supplier.type'),
    dataIndex: 'supplierType',
    key: 'supplierType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.supplier.industrysector'),
    dataIndex: 'industrySector',
    key: 'industrySector',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'industrySector') ?? ''
  },
  {
    title: t('entity.supplier.taxnumber'),
    dataIndex: 'supplierTaxNumber',
    key: 'supplierTaxNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'supplierTaxNumber') ?? ''
  },
  {
    title: t('entity.supplier.registrationcountry'),
    dataIndex: 'registrationCountry',
    key: 'registrationCountry',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'registrationCountry') ?? ''
  },
  {
    title: t('entity.supplier.registrationaddress1'),
    dataIndex: 'registrationAddress1',
    key: 'registrationAddress1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'registrationAddress1') ?? ''
  },
  {
    title: t('entity.supplier.registrationaddress2'),
    dataIndex: 'registrationAddress2',
    key: 'registrationAddress2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'registrationAddress2') ?? ''
  },
  {
    title: t('entity.supplier.registrationaddress3'),
    dataIndex: 'registrationAddress3',
    key: 'registrationAddress3',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'registrationAddress3') ?? ''
  },
  {
    title: t('entity.supplier.phone'),
    dataIndex: 'supplierPhone',
    key: 'supplierPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'supplierPhone') ?? ''
  },
  {
    title: t('entity.supplier.fax'),
    dataIndex: 'supplierFax',
    key: 'supplierFax',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'supplierFax') ?? ''
  },
  {
    title: t('entity.supplier.email'),
    dataIndex: 'supplierEmail',
    key: 'supplierEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'supplierEmail') ?? ''
  },
  {
    title: t('entity.supplier.website'),
    dataIndex: 'supplierWebsite',
    key: 'supplierWebsite',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'supplierWebsite') ?? ''
  },
  {
    title: t('entity.supplier.contactperson'),
    dataIndex: 'contactPerson',
    key: 'contactPerson',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'contactPerson') ?? ''
  },
  {
    title: t('entity.supplier.contactphone'),
    dataIndex: 'contactPhone',
    key: 'contactPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'contactPhone') ?? ''
  },
  {
    title: t('entity.supplier.contactemail'),
    dataIndex: 'contactEmail',
    key: 'contactEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'contactEmail') ?? ''
  },
  {
    title: t('entity.supplier.currencycode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'currencyCode') ?? ''
  },
  {
    title: t('entity.supplier.paymentterms'),
    dataIndex: 'paymentTerms',
    key: 'paymentTerms',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.supplier.level'),
    dataIndex: 'supplierLevel',
    key: 'supplierLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.supplier.evaluationscore'),
    dataIndex: 'evaluationScore',
    key: 'evaluationScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'evaluationScore') ?? ''
  },
  {
    title: t('entity.supplier.isqualified'),
    dataIndex: 'isQualified',
    key: 'isQualified',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierField(record, 'isQualified') ?? ''
  },
  {
    title: t('entity.supplier.status'),
    dataIndex: 'supplierStatus',
    key: 'supplierStatus',
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
        permission: 'logistics:procurement:supplier:update',
        onClick: (record: Supplier) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:procurement:supplier:delete',
        onClick: (record: Supplier) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSupplierId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSupplierField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Supplier[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Supplier, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSupplierId(selectedRow.value) === getSupplierId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Supplier[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Supplier) => ({
  onClick: () => {
    const key = getSupplierId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSupplierId(item)))
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
    const res = await getSupplierList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Supplier] 加载数据失败', { error })
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
  supplierCode: '',
  supplierName: '',
  supplierShortName: '',
  supplierType: undefined as number | undefined,
  industrySector: '',
  supplierTaxNumber: '',
  registrationCountry: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  supplierPhone: '',
  supplierFax: '',
  supplierEmail: '',
  supplierWebsite: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  currencyCode: '',
  paymentTerms: undefined as number | undefined,
  supplierLevel: undefined as number | undefined,
  evaluationScore: undefined as number | undefined,
  isQualified: undefined as number | undefined,
  supplierStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.supplier._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: Supplier) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.supplier._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.supplier._self') }))
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
      await updateSupplier(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.supplier._self') }))
    } else {
      await createSupplier(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.supplier._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
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
  const res = await getSupplierTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSupplier(file, sheetName)
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
    const exportMeta = await exportSupplier(
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
    message.success(t('common.feedback.export.success', { target: t('entity.supplier._self') }))
  } catch (error: any) {
    logger.error('[Supplier] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.supplier._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Supplier) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.supplier._self'), name: t('common.tip.this.target', { target: t('entity.supplier._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSupplierById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.supplier._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.supplier._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.supplier._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSupplierBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.supplier._self') }))
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleSupplierStatusChange(record: Supplier, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getSupplierField(record, 'supplierStatus')
  const id = getSupplierId(record)
  const row = dataSource.value.find((item) => getSupplierId(item) === id)
  if (row) {
    row.supplierStatus = newVal
  }
  try {
    await updateSupplierStatus({ supplierId: id, supplierStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.supplierStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
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
  supplierCode: '',
  supplierName: '',
  supplierShortName: '',
  supplierType: undefined as number | undefined,
  industrySector: '',
  supplierTaxNumber: '',
  registrationCountry: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  supplierPhone: '',
  supplierFax: '',
  supplierEmail: '',
  supplierWebsite: '',
  contactPerson: '',
  contactPhone: '',
  contactEmail: '',
  currencyCode: '',
  paymentTerms: undefined as number | undefined,
  supplierLevel: undefined as number | undefined,
  evaluationScore: undefined as number | undefined,
  isQualified: undefined as number | undefined,
  supplierStatus: undefined as number | undefined,
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
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
