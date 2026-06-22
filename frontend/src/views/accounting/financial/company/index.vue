<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/company -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：公司实体 代表租户下的独立公司/工厂管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="accounting-financial-company">
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
      create-permission="accounting:financial:company:create"
      update-permission="accounting:financial:company:update"
      delete-permission="accounting:financial:company:delete"
      import-permission="accounting:financial:company:import"
      export-permission="accounting:financial:company:export"
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
      entity-scope="tenant"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'companyId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getCompanyId"
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
      <CompanyForm
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
      :storage-key="'takt-query-fields-accounting-financial-company'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('companyName')">
      <a-form-item :label="t('entity.company.name')">
        <a-input
          v-model:value="advancedQueryForm.companyName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.name') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyShortName')">
      <a-form-item :label="t('entity.company.shortname')">
        <a-input
          v-model:value="advancedQueryForm.companyShortName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.shortname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyType')">
      <a-form-item :label="t('entity.company.type')">
        <a-input-number
          v-model:value="advancedQueryForm.companyType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('enterpriseNature')">
      <a-form-item :label="t('entity.company.enterprisenature')">
        <a-input-number
          v-model:value="advancedQueryForm.enterpriseNature"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.enterprisenature') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industryAttribute')">
      <a-form-item :label="t('entity.company.industryattribute')">
        <a-input-number
          v-model:value="advancedQueryForm.industryAttribute"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.industryattribute') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('enterpriseScale')">
      <a-form-item :label="t('entity.company.enterprisescale')">
        <a-input-number
          v-model:value="advancedQueryForm.enterpriseScale"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.enterprisescale') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessScope')">
      <a-form-item :label="t('entity.company.businessscope')">
        <a-textarea
          v-model:value="advancedQueryForm.businessScope"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.businessscope') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress1')">
      <a-form-item :label="t('entity.company.registrationaddress1')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress1"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.registrationaddress1') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress2')">
      <a-form-item :label="t('entity.company.registrationaddress2')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress2"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.registrationaddress2') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress3')">
      <a-form-item :label="t('entity.company.registrationaddress3')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress3"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.registrationaddress3') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationRegion')">
      <a-form-item :label="t('entity.company.registrationregion')">
        <a-input
          v-model:value="advancedQueryForm.registrationRegion"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.registrationregion') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationProvince')">
      <a-form-item :label="t('entity.company.registrationprovince')">
        <a-input
          v-model:value="advancedQueryForm.registrationProvince"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.registrationprovince') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationCity')">
      <a-form-item :label="t('entity.company.registrationcity')">
        <a-input
          v-model:value="advancedQueryForm.registrationCity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.registrationcity') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessRegion')">
      <a-form-item :label="t('entity.company.businessregion')">
        <a-input
          v-model:value="advancedQueryForm.businessRegion"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.businessregion') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessProvince')">
      <a-form-item :label="t('entity.company.businessprovince')">
        <a-input
          v-model:value="advancedQueryForm.businessProvince"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.businessprovince') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessCity')">
      <a-form-item :label="t('entity.company.businesscity')">
        <a-input
          v-model:value="advancedQueryForm.businessCity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.businesscity') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessAddress1')">
      <a-form-item :label="t('entity.company.businessaddress1')">
        <a-textarea
          v-model:value="advancedQueryForm.businessAddress1"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.businessaddress1') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessAddress2')">
      <a-form-item :label="t('entity.company.businessaddress2')">
        <a-textarea
          v-model:value="advancedQueryForm.businessAddress2"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.businessaddress2') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessAddress3')">
      <a-form-item :label="t('entity.company.businessaddress3')">
        <a-textarea
          v-model:value="advancedQueryForm.businessAddress3"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.company.businessaddress3') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyPhone')">
      <a-form-item :label="t('entity.company.phone')">
        <a-input
          v-model:value="advancedQueryForm.companyPhone"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.phone') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyEmail')">
      <a-form-item :label="t('entity.company.email')">
        <a-input
          v-model:value="advancedQueryForm.companyEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.email') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyFax')">
      <a-form-item :label="t('entity.company.fax')">
        <a-input
          v-model:value="advancedQueryForm.companyFax"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.fax') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyWebsite')">
      <a-form-item :label="t('entity.company.website')">
        <a-input
          v-model:value="advancedQueryForm.companyWebsite"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.website') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unifiedSocialCreditCode')">
      <a-form-item :label="t('entity.company.unifiedsocialcreditcode')">
        <a-input
          v-model:value="advancedQueryForm.unifiedSocialCreditCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.unifiedsocialcreditcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxRegistrationNumber')">
      <a-form-item :label="t('entity.company.taxregistrationnumber')">
        <a-input
          v-model:value="advancedQueryForm.taxRegistrationNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.taxregistrationnumber') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('legalRepresentative')">
      <a-form-item :label="t('entity.company.legalrepresentative')">
        <a-input
          v-model:value="advancedQueryForm.legalRepresentative"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.legalrepresentative') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyManager')">
      <a-form-item :label="t('entity.company.manager')">
        <a-input
          v-model:value="advancedQueryForm.companyManager"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.manager') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registeredCapital')">
      <a-form-item :label="t('entity.company.registeredcapital')">
        <a-input-number
          v-model:value="advancedQueryForm.registeredCapital"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.registeredcapital') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('establishmentDateStart')">
      <a-form-item :label="t('entity.company.establishmentdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.establishmentDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.company.establishmentdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('establishmentDateEnd')">
      <a-form-item :label="t('entity.company.establishmentdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.establishmentDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.company.establishmentdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closingDateStart')">
      <a-form-item :label="t('entity.company.closingdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.closingDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.company.closingdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closingDateEnd')">
      <a-form-item :label="t('entity.company.closingdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.closingDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.company.closingdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyExistence')">
      <a-form-item :label="t('entity.company.existence')">
        <a-input-number
          v-model:value="advancedQueryForm.companyExistence"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.existence') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.company.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.relatedplant') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defaultCulture')">
      <a-form-item :label="t('entity.company.defaultculture')">
        <a-input
          v-model:value="advancedQueryForm.defaultCulture"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.defaultculture') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('codeAlias')">
      <a-form-item :label="t('entity.company.codealias')">
        <a-input
          v-model:value="advancedQueryForm.codeAlias"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.codealias') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyStatus')">
      <a-form-item :label="t('entity.company.status')">
        <a-input-number
          v-model:value="advancedQueryForm.companyStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.company.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.company.sortorder') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.company._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.company._self"
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
      :id-column-key="'companyId'"
      :action-column-key="'action'"
      entity-scope="tenant"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
/**
 * 公司实体 代表租户下的独立公司/工厂管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/company
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import CompanyForm from './components/company-form.vue'
import { getCompanyList, getCompanyById, createCompany, updateCompany, deleteCompanyById, deleteCompanyBatch, getCompanyTemplate, importCompany, exportCompany } from '@/api/accounting/financial/company'
import type { Company, CompanyQuery, CompanyCreate, CompanyUpdate } from '@/types/accounting/financial/company'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCompany')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.company._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Company[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Company | null>(null)
/** 表格多选行 */
const selectedRows = ref<Company[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Company>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  companyName: '',
  companyShortName: '',
  companyType: undefined as number | undefined,
  enterpriseNature: undefined as number | undefined,
  industryAttribute: undefined as number | undefined,
  enterpriseScale: undefined as number | undefined,
  businessScope: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  registrationRegion: '',
  registrationProvince: '',
  registrationCity: '',
  businessRegion: '',
  businessProvince: '',
  businessCity: '',
  businessAddress1: '',
  businessAddress2: '',
  businessAddress3: '',
  companyPhone: '',
  companyEmail: '',
  companyFax: '',
  companyWebsite: '',
  unifiedSocialCreditCode: '',
  taxRegistrationNumber: '',
  legalRepresentative: '',
  companyManager: '',
  registeredCapital: undefined as number | undefined,
  establishmentDateStart: '',
  establishmentDateEnd: '',
  closingDateStart: '',
  closingDateEnd: '',
  companyExistence: undefined as number | undefined,
  relatedPlant: '',
  defaultCulture: '',
  codeAlias: '',
  companyStatus: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'companyName', label: t('entity.company.name') },
  { key: 'companyShortName', label: t('entity.company.shortname') },
  { key: 'companyType', label: t('entity.company.type') },
  { key: 'enterpriseNature', label: t('entity.company.enterprisenature') },
  { key: 'industryAttribute', label: t('entity.company.industryattribute') },
  { key: 'enterpriseScale', label: t('entity.company.enterprisescale') },
  { key: 'businessScope', label: t('entity.company.businessscope') },
  { key: 'registrationAddress1', label: t('entity.company.registrationaddress1') },
  { key: 'registrationAddress2', label: t('entity.company.registrationaddress2') },
  { key: 'registrationAddress3', label: t('entity.company.registrationaddress3') },
  { key: 'registrationRegion', label: t('entity.company.registrationregion') },
  { key: 'registrationProvince', label: t('entity.company.registrationprovince') },
  { key: 'registrationCity', label: t('entity.company.registrationcity') },
  { key: 'businessRegion', label: t('entity.company.businessregion') },
  { key: 'businessProvince', label: t('entity.company.businessprovince') },
  { key: 'businessCity', label: t('entity.company.businesscity') },
  { key: 'businessAddress1', label: t('entity.company.businessaddress1') },
  { key: 'businessAddress2', label: t('entity.company.businessaddress2') },
  { key: 'businessAddress3', label: t('entity.company.businessaddress3') },
  { key: 'companyPhone', label: t('entity.company.phone') },
  { key: 'companyEmail', label: t('entity.company.email') },
  { key: 'companyFax', label: t('entity.company.fax') },
  { key: 'companyWebsite', label: t('entity.company.website') },
  { key: 'unifiedSocialCreditCode', label: t('entity.company.unifiedsocialcreditcode') },
  { key: 'taxRegistrationNumber', label: t('entity.company.taxregistrationnumber') },
  { key: 'legalRepresentative', label: t('entity.company.legalrepresentative') },
  { key: 'companyManager', label: t('entity.company.manager') },
  { key: 'registeredCapital', label: t('entity.company.registeredcapital') },
  { key: 'establishmentDateStart', label: t('entity.company.establishmentdatestart') },
  { key: 'establishmentDateEnd', label: t('entity.company.establishmentdateend') },
  { key: 'closingDateStart', label: t('entity.company.closingdatestart') },
  { key: 'closingDateEnd', label: t('entity.company.closingdateend') },
  { key: 'companyExistence', label: t('entity.company.existence') },
  { key: 'relatedPlant', label: t('entity.company.relatedplant') },
  { key: 'defaultCulture', label: t('entity.company.defaultculture') },
  { key: 'codeAlias', label: t('entity.company.codealias') },
  { key: 'companyStatus', label: t('entity.company.status') },
  { key: 'sortOrder', label: t('entity.company.sortorder') },
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
const entityIdName = 'companyId'
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
    dataIndex: 'companyId',
    key: 'companyId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyId') ?? ''
  },
  {
    title: t('entity.company.name'),
    dataIndex: 'companyName',
    key: 'companyName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyName') ?? ''
  },
  {
    title: t('entity.company.shortname'),
    dataIndex: 'companyShortName',
    key: 'companyShortName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyShortName') ?? ''
  },
  {
    title: t('entity.company.type'),
    dataIndex: 'companyType',
    key: 'companyType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyType') ?? ''
  },
  {
    title: t('entity.company.enterprisenature'),
    dataIndex: 'enterpriseNature',
    key: 'enterpriseNature',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'enterpriseNature') ?? ''
  },
  {
    title: t('entity.company.industryattribute'),
    dataIndex: 'industryAttribute',
    key: 'industryAttribute',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'industryAttribute') ?? ''
  },
  {
    title: t('entity.company.enterprisescale'),
    dataIndex: 'enterpriseScale',
    key: 'enterpriseScale',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'enterpriseScale') ?? ''
  },
  {
    title: t('entity.company.businessscope'),
    dataIndex: 'businessScope',
    key: 'businessScope',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'businessScope') ?? ''
  },
  {
    title: t('entity.company.registrationaddress1'),
    dataIndex: 'registrationAddress1',
    key: 'registrationAddress1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationAddress1') ?? ''
  },
  {
    title: t('entity.company.registrationaddress2'),
    dataIndex: 'registrationAddress2',
    key: 'registrationAddress2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationAddress2') ?? ''
  },
  {
    title: t('entity.company.registrationaddress3'),
    dataIndex: 'registrationAddress3',
    key: 'registrationAddress3',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationAddress3') ?? ''
  },
  {
    title: t('entity.company.registrationregion'),
    dataIndex: 'registrationRegion',
    key: 'registrationRegion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationRegion') ?? ''
  },
  {
    title: t('entity.company.registrationprovince'),
    dataIndex: 'registrationProvince',
    key: 'registrationProvince',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationProvince') ?? ''
  },
  {
    title: t('entity.company.registrationcity'),
    dataIndex: 'registrationCity',
    key: 'registrationCity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registrationCity') ?? ''
  },
  {
    title: t('entity.company.businessregion'),
    dataIndex: 'businessRegion',
    key: 'businessRegion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'businessRegion') ?? ''
  },
  {
    title: t('entity.company.businessprovince'),
    dataIndex: 'businessProvince',
    key: 'businessProvince',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'businessProvince') ?? ''
  },
  {
    title: t('entity.company.businesscity'),
    dataIndex: 'businessCity',
    key: 'businessCity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'businessCity') ?? ''
  },
  {
    title: t('entity.company.businessaddress1'),
    dataIndex: 'businessAddress1',
    key: 'businessAddress1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'businessAddress1') ?? ''
  },
  {
    title: t('entity.company.businessaddress2'),
    dataIndex: 'businessAddress2',
    key: 'businessAddress2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'businessAddress2') ?? ''
  },
  {
    title: t('entity.company.businessaddress3'),
    dataIndex: 'businessAddress3',
    key: 'businessAddress3',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'businessAddress3') ?? ''
  },
  {
    title: t('entity.company.phone'),
    dataIndex: 'companyPhone',
    key: 'companyPhone',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyPhone') ?? ''
  },
  {
    title: t('entity.company.email'),
    dataIndex: 'companyEmail',
    key: 'companyEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyEmail') ?? ''
  },
  {
    title: t('entity.company.fax'),
    dataIndex: 'companyFax',
    key: 'companyFax',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyFax') ?? ''
  },
  {
    title: t('entity.company.website'),
    dataIndex: 'companyWebsite',
    key: 'companyWebsite',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyWebsite') ?? ''
  },
  {
    title: t('entity.company.unifiedsocialcreditcode'),
    dataIndex: 'unifiedSocialCreditCode',
    key: 'unifiedSocialCreditCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'unifiedSocialCreditCode') ?? ''
  },
  {
    title: t('entity.company.taxregistrationnumber'),
    dataIndex: 'taxRegistrationNumber',
    key: 'taxRegistrationNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'taxRegistrationNumber') ?? ''
  },
  {
    title: t('entity.company.legalrepresentative'),
    dataIndex: 'legalRepresentative',
    key: 'legalRepresentative',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'legalRepresentative') ?? ''
  },
  {
    title: t('entity.company.manager'),
    dataIndex: 'companyManager',
    key: 'companyManager',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyManager') ?? ''
  },
  {
    title: t('entity.company.registeredcapital'),
    dataIndex: 'registeredCapital',
    key: 'registeredCapital',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'registeredCapital') ?? ''
  },
  {
    title: t('entity.company.establishmentdate'),
    dataIndex: 'establishmentDate',
    key: 'establishmentDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'establishmentDate') ?? ''
  },
  {
    title: t('entity.company.closingdate'),
    dataIndex: 'closingDate',
    key: 'closingDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'closingDate') ?? ''
  },
  {
    title: t('entity.company.existence'),
    dataIndex: 'companyExistence',
    key: 'companyExistence',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyExistence') ?? ''
  },
  {
    title: t('entity.company.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'relatedPlant') ?? ''
  },
  {
    title: t('entity.company.defaultculture'),
    dataIndex: 'defaultCulture',
    key: 'defaultCulture',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'defaultCulture') ?? ''
  },
  {
    title: t('entity.company.codealias'),
    dataIndex: 'codeAlias',
    key: 'codeAlias',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'codeAlias') ?? ''
  },
  {
    title: t('entity.company.status'),
    dataIndex: 'companyStatus',
    key: 'companyStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'companyStatus') ?? ''
  },
  {
    title: t('entity.company.rolecompanies'),
    dataIndex: 'roleCompanies',
    key: 'roleCompanies',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'roleCompanies') ?? ''
  },
  {
    title: t('entity.company.usercompanies'),
    dataIndex: 'userCompanies',
    key: 'userCompanies',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCompanyField(record, 'userCompanies') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:company:update',
        onClick: (record: Company) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:company:delete',
        onClick: (record: Company) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCompanyId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCompanyField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Company[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Company, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getCompanyId(selectedRow.value) === getCompanyId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Company[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Company) => ({
  onClick: () => {
    const key = getCompanyId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getCompanyId(item)))
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
    const params: CompanyQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getCompanyList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Company] 加载数据失败', { error })
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
  companyName: '',
  companyShortName: '',
  companyType: undefined as number | undefined,
  enterpriseNature: undefined as number | undefined,
  industryAttribute: undefined as number | undefined,
  enterpriseScale: undefined as number | undefined,
  businessScope: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  registrationRegion: '',
  registrationProvince: '',
  registrationCity: '',
  businessRegion: '',
  businessProvince: '',
  businessCity: '',
  businessAddress1: '',
  businessAddress2: '',
  businessAddress3: '',
  companyPhone: '',
  companyEmail: '',
  companyFax: '',
  companyWebsite: '',
  unifiedSocialCreditCode: '',
  taxRegistrationNumber: '',
  legalRepresentative: '',
  companyManager: '',
  registeredCapital: undefined as number | undefined,
  establishmentDateStart: '',
  establishmentDateEnd: '',
  closingDateStart: '',
  closingDateEnd: '',
  companyExistence: undefined as number | undefined,
  relatedPlant: '',
  defaultCulture: '',
  codeAlias: '',
  companyStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.company._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: Company) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.company._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.company._self') }))
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
      await updateCompany(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.company._self') }))
    } else {
      await createCompany(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.company._self') }))
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
  const res = await getCompanyTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCompany(file, sheetName)
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
    const exportQuery: CompanyQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportCompany(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.company._self') }))
  } catch (error: any) {
    logger.error('[Company] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.company._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Company) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.company._self'), name: t('common.tip.this.target', { target: t('entity.company._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCompanyById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.company._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.company._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.company._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCompanyBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.company._self') }))
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
  companyName: '',
  companyShortName: '',
  companyType: undefined as number | undefined,
  enterpriseNature: undefined as number | undefined,
  industryAttribute: undefined as number | undefined,
  enterpriseScale: undefined as number | undefined,
  businessScope: '',
  registrationAddress1: '',
  registrationAddress2: '',
  registrationAddress3: '',
  registrationRegion: '',
  registrationProvince: '',
  registrationCity: '',
  businessRegion: '',
  businessProvince: '',
  businessCity: '',
  businessAddress1: '',
  businessAddress2: '',
  businessAddress3: '',
  companyPhone: '',
  companyEmail: '',
  companyFax: '',
  companyWebsite: '',
  unifiedSocialCreditCode: '',
  taxRegistrationNumber: '',
  legalRepresentative: '',
  companyManager: '',
  registeredCapital: undefined as number | undefined,
  establishmentDateStart: '',
  establishmentDateEnd: '',
  closingDateStart: '',
  closingDateEnd: '',
  companyExistence: undefined as number | undefined,
  relatedPlant: '',
  defaultCulture: '',
  codeAlias: '',
  companyStatus: undefined as number | undefined,
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
.accounting-financial-company {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
