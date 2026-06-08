<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/sampling-scheme -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt抽样方案实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-operation-sampling-scheme">
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
      create-permission="logistics:quality:operation:samplingscheme:create"
      update-permission="logistics:quality:operation:samplingscheme:update"
      delete-permission="logistics:quality:operation:samplingscheme:delete"
      import-permission="logistics:quality:operation:samplingscheme:import"
      export-permission="logistics:quality:operation:samplingscheme:export"
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
      :id-column-key="'samplingSchemeId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSamplingSchemeId"
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
      <SamplingSchemeForm
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
      :storage-key="'takt-query-fields-logistics-quality-operation-sampling-scheme'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.samplingScheme.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingSchemeCode')">
      <a-form-item :label="t('entity.samplingScheme.code')">
        <a-input
          v-model:value="advancedQueryForm.samplingSchemeCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingSchemeName')">
      <a-form-item :label="t('entity.samplingScheme.name')">
        <a-input
          v-model:value="advancedQueryForm.samplingSchemeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.name') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingSchemeType')">
      <a-form-item :label="t('entity.samplingScheme.type')">
        <a-input-number
          v-model:value="advancedQueryForm.samplingSchemeType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingStandard')">
      <a-form-item :label="t('entity.samplingScheme.samplingstandard')">
        <a-input-number
          v-model:value="advancedQueryForm.samplingStandard"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.samplingstandard') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionLevel')">
      <a-form-item :label="t('entity.samplingScheme.inspectionlevel')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.inspectionlevel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('aqlValue')">
      <a-form-item :label="t('entity.samplingScheme.aqlvalue')">
        <a-input-number
          v-model:value="advancedQueryForm.aqlValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.aqlvalue') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lotSizeMin')">
      <a-form-item :label="t('entity.samplingScheme.lotsizemin')">
        <a-input-number
          v-model:value="advancedQueryForm.lotSizeMin"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.lotsizemin') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lotSizeMax')">
      <a-form-item :label="t('entity.samplingScheme.lotsizemax')">
        <a-input-number
          v-model:value="advancedQueryForm.lotSizeMax"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.lotsizemax') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sampleSize')">
      <a-form-item :label="t('entity.samplingScheme.samplesize')">
        <a-input-number
          v-model:value="advancedQueryForm.sampleSize"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.samplesize') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptanceNumber')">
      <a-form-item :label="t('entity.samplingScheme.acceptancenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.acceptanceNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.acceptancenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rejectionNumber')">
      <a-form-item :label="t('entity.samplingScheme.rejectionnumber')">
        <a-input-number
          v-model:value="advancedQueryForm.rejectionNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.rejectionnumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionStrictness')">
      <a-form-item :label="t('entity.samplingScheme.inspectionstrictness')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionStrictness"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.inspectionstrictness') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isTransferRuleEnabled')">
      <a-form-item :label="t('entity.samplingScheme.istransferruleenabled')">
        <a-input-number
          v-model:value="advancedQueryForm.isTransferRuleEnabled"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.istransferruleenabled') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('transferRuleConfig')">
      <a-form-item :label="t('entity.samplingScheme.transferruleconfig')">
        <a-input
          v-model:value="advancedQueryForm.transferRuleConfig"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.transferruleconfig') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingSchemeStatus')">
      <a-form-item :label="t('entity.samplingScheme.status')">
        <a-input-number
          v-model:value="advancedQueryForm.samplingSchemeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('schemeDescription')">
      <a-form-item :label="t('entity.samplingScheme.schemedescription')">
        <a-textarea
          v-model:value="advancedQueryForm.schemeDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.samplingScheme.schemedescription') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.samplingScheme._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.samplingScheme._self"
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
      :id-column-key="'samplingSchemeId'"
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
 * Takt抽样方案实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/sampling-scheme
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import SamplingSchemeForm from './components/sampling-scheme-form.vue'
import { getSamplingSchemeList, getSamplingSchemeById, createSamplingScheme, updateSamplingScheme, deleteSamplingSchemeById, deleteSamplingSchemeBatch, getSamplingSchemeTemplate, importSamplingScheme, exportSamplingScheme } from '@/api/logistics/quality/operation/sampling-scheme'
import type { SamplingScheme, SamplingSchemeQuery, SamplingSchemeCreate, SamplingSchemeUpdate } from '@/types/logistics/quality/operation/sampling-scheme'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSamplingScheme')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.samplingScheme._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SamplingScheme[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SamplingScheme | null>(null)
/** 表格多选行 */
const selectedRows = ref<SamplingScheme[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SamplingScheme>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  samplingSchemeCode: '',
  samplingSchemeName: '',
  samplingSchemeType: undefined as number | undefined,
  samplingStandard: undefined as number | undefined,
  inspectionLevel: undefined as number | undefined,
  aqlValue: undefined as number | undefined,
  lotSizeMin: undefined as number | undefined,
  lotSizeMax: undefined as number | undefined,
  sampleSize: undefined as number | undefined,
  acceptanceNumber: undefined as number | undefined,
  rejectionNumber: undefined as number | undefined,
  inspectionStrictness: undefined as number | undefined,
  isTransferRuleEnabled: undefined as number | undefined,
  transferRuleConfig: '',
  samplingSchemeStatus: undefined as number | undefined,
  schemeDescription: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.samplingScheme.plantcode') },
  { key: 'samplingSchemeCode', label: t('entity.samplingScheme.code') },
  { key: 'samplingSchemeName', label: t('entity.samplingScheme.name') },
  { key: 'samplingSchemeType', label: t('entity.samplingScheme.type') },
  { key: 'samplingStandard', label: t('entity.samplingScheme.samplingstandard') },
  { key: 'inspectionLevel', label: t('entity.samplingScheme.inspectionlevel') },
  { key: 'aqlValue', label: t('entity.samplingScheme.aqlvalue') },
  { key: 'lotSizeMin', label: t('entity.samplingScheme.lotsizemin') },
  { key: 'lotSizeMax', label: t('entity.samplingScheme.lotsizemax') },
  { key: 'sampleSize', label: t('entity.samplingScheme.samplesize') },
  { key: 'acceptanceNumber', label: t('entity.samplingScheme.acceptancenumber') },
  { key: 'rejectionNumber', label: t('entity.samplingScheme.rejectionnumber') },
  { key: 'inspectionStrictness', label: t('entity.samplingScheme.inspectionstrictness') },
  { key: 'isTransferRuleEnabled', label: t('entity.samplingScheme.istransferruleenabled') },
  { key: 'transferRuleConfig', label: t('entity.samplingScheme.transferruleconfig') },
  { key: 'samplingSchemeStatus', label: t('entity.samplingScheme.status') },
  { key: 'schemeDescription', label: t('entity.samplingScheme.schemedescription') },
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
const entityIdName = 'samplingSchemeId'
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
    dataIndex: 'samplingSchemeId',
    key: 'samplingSchemeId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'samplingSchemeId') ?? ''
  },
  {
    title: t('entity.samplingScheme.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.samplingScheme.code'),
    dataIndex: 'samplingSchemeCode',
    key: 'samplingSchemeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'samplingSchemeCode') ?? ''
  },
  {
    title: t('entity.samplingScheme.name'),
    dataIndex: 'samplingSchemeName',
    key: 'samplingSchemeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'samplingSchemeName') ?? ''
  },
  {
    title: t('entity.samplingScheme.type'),
    dataIndex: 'samplingSchemeType',
    key: 'samplingSchemeType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'samplingSchemeType') ?? ''
  },
  {
    title: t('entity.samplingScheme.samplingstandard'),
    dataIndex: 'samplingStandard',
    key: 'samplingStandard',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'samplingStandard') ?? ''
  },
  {
    title: t('entity.samplingScheme.inspectionlevel'),
    dataIndex: 'inspectionLevel',
    key: 'inspectionLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'inspectionLevel') ?? ''
  },
  {
    title: t('entity.samplingScheme.aqlvalue'),
    dataIndex: 'aqlValue',
    key: 'aqlValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'aqlValue') ?? ''
  },
  {
    title: t('entity.samplingScheme.lotsizemin'),
    dataIndex: 'lotSizeMin',
    key: 'lotSizeMin',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'lotSizeMin') ?? ''
  },
  {
    title: t('entity.samplingScheme.lotsizemax'),
    dataIndex: 'lotSizeMax',
    key: 'lotSizeMax',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'lotSizeMax') ?? ''
  },
  {
    title: t('entity.samplingScheme.samplesize'),
    dataIndex: 'sampleSize',
    key: 'sampleSize',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'sampleSize') ?? ''
  },
  {
    title: t('entity.samplingScheme.acceptancenumber'),
    dataIndex: 'acceptanceNumber',
    key: 'acceptanceNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'acceptanceNumber') ?? ''
  },
  {
    title: t('entity.samplingScheme.rejectionnumber'),
    dataIndex: 'rejectionNumber',
    key: 'rejectionNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'rejectionNumber') ?? ''
  },
  {
    title: t('entity.samplingScheme.inspectionstrictness'),
    dataIndex: 'inspectionStrictness',
    key: 'inspectionStrictness',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'inspectionStrictness') ?? ''
  },
  {
    title: t('entity.samplingScheme.istransferruleenabled'),
    dataIndex: 'isTransferRuleEnabled',
    key: 'isTransferRuleEnabled',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'isTransferRuleEnabled') ?? ''
  },
  {
    title: t('entity.samplingScheme.transferruleconfig'),
    dataIndex: 'transferRuleConfig',
    key: 'transferRuleConfig',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'transferRuleConfig') ?? ''
  },
  {
    title: t('entity.samplingScheme.status'),
    dataIndex: 'samplingSchemeStatus',
    key: 'samplingSchemeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'samplingSchemeStatus') ?? ''
  },
  {
    title: t('entity.samplingScheme.schemedescription'),
    dataIndex: 'schemeDescription',
    key: 'schemeDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSamplingSchemeField(record, 'schemeDescription') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:samplingscheme:update',
        onClick: (record: SamplingScheme) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:samplingscheme:delete',
        onClick: (record: SamplingScheme) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSamplingSchemeId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSamplingSchemeField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SamplingScheme[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SamplingScheme, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSamplingSchemeId(selectedRow.value) === getSamplingSchemeId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SamplingScheme[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SamplingScheme) => ({
  onClick: () => {
    const key = getSamplingSchemeId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSamplingSchemeId(item)))
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
    const params: SamplingSchemeQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getSamplingSchemeList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SamplingScheme] 加载数据失败', { error })
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
  samplingSchemeCode: '',
  samplingSchemeName: '',
  samplingSchemeType: undefined as number | undefined,
  samplingStandard: undefined as number | undefined,
  inspectionLevel: undefined as number | undefined,
  aqlValue: undefined as number | undefined,
  lotSizeMin: undefined as number | undefined,
  lotSizeMax: undefined as number | undefined,
  sampleSize: undefined as number | undefined,
  acceptanceNumber: undefined as number | undefined,
  rejectionNumber: undefined as number | undefined,
  inspectionStrictness: undefined as number | undefined,
  isTransferRuleEnabled: undefined as number | undefined,
  transferRuleConfig: '',
  samplingSchemeStatus: undefined as number | undefined,
  schemeDescription: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.samplingScheme._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: SamplingScheme) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.samplingScheme._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.samplingScheme._self') }))
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
      await updateSamplingScheme(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.samplingScheme._self') }))
    } else {
      await createSamplingScheme(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.samplingScheme._self') }))
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
  const res = await getSamplingSchemeTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSamplingScheme(file, sheetName)
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
    const exportQuery: SamplingSchemeQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportSamplingScheme(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.samplingScheme._self') }))
  } catch (error: any) {
    logger.error('[SamplingScheme] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.samplingScheme._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SamplingScheme) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.samplingScheme._self'), name: t('common.tip.this.target', { target: t('entity.samplingScheme._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSamplingSchemeById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.samplingScheme._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.samplingScheme._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.samplingScheme._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSamplingSchemeBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.samplingScheme._self') }))
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
  samplingSchemeCode: '',
  samplingSchemeName: '',
  samplingSchemeType: undefined as number | undefined,
  samplingStandard: undefined as number | undefined,
  inspectionLevel: undefined as number | undefined,
  aqlValue: undefined as number | undefined,
  lotSizeMin: undefined as number | undefined,
  lotSizeMax: undefined as number | undefined,
  sampleSize: undefined as number | undefined,
  acceptanceNumber: undefined as number | undefined,
  rejectionNumber: undefined as number | undefined,
  inspectionStrictness: undefined as number | undefined,
  isTransferRuleEnabled: undefined as number | undefined,
  transferRuleConfig: '',
  samplingSchemeStatus: undefined as number | undefined,
  schemeDescription: '',
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
.logistics-quality-operation-sampling-scheme {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
