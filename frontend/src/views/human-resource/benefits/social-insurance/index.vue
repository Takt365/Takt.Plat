<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/benefits/social-insurance -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：社保与公积金月度缴纳流水管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-benefits-social-insurance">
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
      create-permission="humanresource:talent:staffingrequirement:create"
      update-permission="humanresource:talent:staffingrequirement:update"
      delete-permission="humanresource:talent:staffingrequirement:delete"
      import-permission="humanresource:talent:staffingrequirement:import"
      export-permission="humanresource:talent:staffingrequirement:export"
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
      :id-column-key="'socialInsuranceId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSocialInsuranceId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'payStatus'">
          <TaktDictTag
            :value="getSocialInsuranceField(record, 'payStatus')"
            dict-type="hr_social_insurance_pay_status"
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
      <SocialInsuranceForm
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
      :storage-key="'takt-query-fields-human-resource-benefits-social-insurance'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('benefitItemId')">
      <a-form-item :label="t('entity.socialinsurance.benefititemid')">
        <a-input
          v-model:value="advancedQueryForm.benefitItemId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.benefititemid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.socialinsurance.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.employeeid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeName')">
      <a-form-item :label="t('entity.socialinsurance.employeename')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.employeename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('payPeriod')">
      <a-form-item :label="t('entity.socialinsurance.payperiod')">
        <a-input
          v-model:value="advancedQueryForm.payPeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.payperiod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('socialSecurityBase')">
      <a-form-item :label="t('entity.socialinsurance.socialsecuritybase')">
        <a-input-number
          v-model:value="advancedQueryForm.socialSecurityBase"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.socialsecuritybase') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pensionAmount')">
      <a-form-item :label="t('entity.socialinsurance.pensionamount')">
        <a-input-number
          v-model:value="advancedQueryForm.pensionAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.pensionamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('medicalAmount')">
      <a-form-item :label="t('entity.socialinsurance.medicalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.medicalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.medicalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unemploymentAmount')">
      <a-form-item :label="t('entity.socialinsurance.unemploymentamount')">
        <a-input-number
          v-model:value="advancedQueryForm.unemploymentAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.unemploymentamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('injuryAmount')">
      <a-form-item :label="t('entity.socialinsurance.injuryamount')">
        <a-input-number
          v-model:value="advancedQueryForm.injuryAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.injuryamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maternityAmount')">
      <a-form-item :label="t('entity.socialinsurance.maternityamount')">
        <a-input-number
          v-model:value="advancedQueryForm.maternityAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.maternityamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('housingFundBase')">
      <a-form-item :label="t('entity.socialinsurance.housingfundbase')">
        <a-input-number
          v-model:value="advancedQueryForm.housingFundBase"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.housingfundbase') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('housingFundAmount')">
      <a-form-item :label="t('entity.socialinsurance.housingfundamount')">
        <a-input-number
          v-model:value="advancedQueryForm.housingFundAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.housingfundamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="t('entity.socialinsurance.totalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.totalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('payStatus')">
      <a-form-item :label="t('entity.socialinsurance.paystatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.payStatus"
          dict-type="hr_social_insurance_pay_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.paystatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.socialinsurance.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.relatedplant') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.socialinsurance._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.socialinsurance._self"
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
      :id-column-key="'socialInsuranceId'"
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
 * 社保与公积金月度缴纳流水管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/benefits/social-insurance
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import SocialInsuranceForm from './components/social-insurance-form.vue'
import { getSocialInsuranceList, getSocialInsuranceById, createSocialInsurance, updateSocialInsurance, deleteSocialInsuranceById, deleteSocialInsuranceBatch, getSocialInsuranceTemplate, importSocialInsurance, exportSocialInsurance } from '@/api/human-resource/benefits/social-insurance'
import type { SocialInsurance, SocialInsuranceQuery, SocialInsuranceCreate, SocialInsuranceUpdate } from '@/types/human-resource/benefits/social-insurance'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSocialInsurance')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.socialinsurance._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SocialInsurance[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SocialInsurance | null>(null)
/** 表格多选行 */
const selectedRows = ref<SocialInsurance[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SocialInsurance>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  benefitItemId: '',
  employeeId: '',
  employeeName: '',
  payPeriod: '',
  socialSecurityBase: undefined as number | undefined,
  pensionAmount: undefined as number | undefined,
  medicalAmount: undefined as number | undefined,
  unemploymentAmount: undefined as number | undefined,
  injuryAmount: undefined as number | undefined,
  maternityAmount: undefined as number | undefined,
  housingFundBase: undefined as number | undefined,
  housingFundAmount: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  payStatus: undefined as number | undefined,
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'benefitItemId', label: t('entity.socialinsurance.benefititemid') },
  { key: 'employeeId', label: t('entity.socialinsurance.employeeid') },
  { key: 'employeeName', label: t('entity.socialinsurance.employeename') },
  { key: 'payPeriod', label: t('entity.socialinsurance.payperiod') },
  { key: 'socialSecurityBase', label: t('entity.socialinsurance.socialsecuritybase') },
  { key: 'pensionAmount', label: t('entity.socialinsurance.pensionamount') },
  { key: 'medicalAmount', label: t('entity.socialinsurance.medicalamount') },
  { key: 'unemploymentAmount', label: t('entity.socialinsurance.unemploymentamount') },
  { key: 'injuryAmount', label: t('entity.socialinsurance.injuryamount') },
  { key: 'maternityAmount', label: t('entity.socialinsurance.maternityamount') },
  { key: 'housingFundBase', label: t('entity.socialinsurance.housingfundbase') },
  { key: 'housingFundAmount', label: t('entity.socialinsurance.housingfundamount') },
  { key: 'totalAmount', label: t('entity.socialinsurance.totalamount') },
  { key: 'payStatus', label: t('entity.socialinsurance.paystatus') },
  { key: 'relatedPlant', label: t('entity.socialinsurance.relatedplant') },
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
const entityIdName = 'socialInsuranceId'
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
    dataIndex: 'socialInsuranceId',
    key: 'socialInsuranceId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'socialInsuranceId') ?? ''
  },
  {
    title: t('entity.socialinsurance.benefititemid'),
    dataIndex: 'benefitItemId',
    key: 'benefitItemId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'benefitItemId') ?? ''
  },
  {
    title: t('entity.socialinsurance.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.socialinsurance.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.socialinsurance.payperiod'),
    dataIndex: 'payPeriod',
    key: 'payPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'payPeriod') ?? ''
  },
  {
    title: t('entity.socialinsurance.socialsecuritybase'),
    dataIndex: 'socialSecurityBase',
    key: 'socialSecurityBase',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'socialSecurityBase') ?? ''
  },
  {
    title: t('entity.socialinsurance.pensionamount'),
    dataIndex: 'pensionAmount',
    key: 'pensionAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'pensionAmount') ?? ''
  },
  {
    title: t('entity.socialinsurance.medicalamount'),
    dataIndex: 'medicalAmount',
    key: 'medicalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'medicalAmount') ?? ''
  },
  {
    title: t('entity.socialinsurance.unemploymentamount'),
    dataIndex: 'unemploymentAmount',
    key: 'unemploymentAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'unemploymentAmount') ?? ''
  },
  {
    title: t('entity.socialinsurance.injuryamount'),
    dataIndex: 'injuryAmount',
    key: 'injuryAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'injuryAmount') ?? ''
  },
  {
    title: t('entity.socialinsurance.maternityamount'),
    dataIndex: 'maternityAmount',
    key: 'maternityAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'maternityAmount') ?? ''
  },
  {
    title: t('entity.socialinsurance.housingfundbase'),
    dataIndex: 'housingFundBase',
    key: 'housingFundBase',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'housingFundBase') ?? ''
  },
  {
    title: t('entity.socialinsurance.housingfundamount'),
    dataIndex: 'housingFundAmount',
    key: 'housingFundAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'housingFundAmount') ?? ''
  },
  {
    title: t('entity.socialinsurance.totalamount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'totalAmount') ?? ''
  },
  {
    title: t('entity.socialinsurance.paystatus'),
    dataIndex: 'payStatus',
    key: 'payStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.socialinsurance.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSocialInsuranceField(record, 'relatedPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'humanresource:talent:staffingrequirement:update',
        onClick: (record: SocialInsurance) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'humanresource:talent:staffingrequirement:delete',
        onClick: (record: SocialInsurance) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSocialInsuranceId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSocialInsuranceField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SocialInsurance[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SocialInsurance, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSocialInsuranceId(selectedRow.value) === getSocialInsuranceId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SocialInsurance[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SocialInsurance) => ({
  onClick: () => {
    const key = getSocialInsuranceId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSocialInsuranceId(item)))
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
    const params: SocialInsuranceQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getSocialInsuranceList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SocialInsurance] 加载数据失败', { error })
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
  benefitItemId: '',
  employeeId: '',
  employeeName: '',
  payPeriod: '',
  socialSecurityBase: undefined as number | undefined,
  pensionAmount: undefined as number | undefined,
  medicalAmount: undefined as number | undefined,
  unemploymentAmount: undefined as number | undefined,
  injuryAmount: undefined as number | undefined,
  maternityAmount: undefined as number | undefined,
  housingFundBase: undefined as number | undefined,
  housingFundAmount: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  payStatus: undefined as number | undefined,
  relatedPlant: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.socialinsurance._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: SocialInsurance) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.socialinsurance._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.socialinsurance._self') }))
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
      await updateSocialInsurance(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.socialinsurance._self') }))
    } else {
      await createSocialInsurance(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.socialinsurance._self') }))
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
  const res = await getSocialInsuranceTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSocialInsurance(file, sheetName)
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
    const exportQuery: SocialInsuranceQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportSocialInsurance(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.socialinsurance._self') }))
  } catch (error: any) {
    logger.error('[SocialInsurance] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.socialinsurance._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SocialInsurance) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.socialinsurance._self'), name: t('common.tip.this.target', { target: t('entity.socialinsurance._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSocialInsuranceById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.socialinsurance._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.socialinsurance._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.socialinsurance._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSocialInsuranceBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.socialinsurance._self') }))
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
  benefitItemId: '',
  employeeId: '',
  employeeName: '',
  payPeriod: '',
  socialSecurityBase: undefined as number | undefined,
  pensionAmount: undefined as number | undefined,
  medicalAmount: undefined as number | undefined,
  unemploymentAmount: undefined as number | undefined,
  injuryAmount: undefined as number | undefined,
  maternityAmount: undefined as number | undefined,
  housingFundBase: undefined as number | undefined,
  housingFundAmount: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  payStatus: undefined as number | undefined,
  relatedPlant: '',
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
.human-resource-benefits-social-insurance {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
