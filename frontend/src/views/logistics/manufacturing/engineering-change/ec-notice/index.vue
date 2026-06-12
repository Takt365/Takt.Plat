<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-notice -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：工程变更通知单实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-engineering-change-ec-notice">
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
      create-permission="logistics:manufacturing:engineeringchange:ecnotice:create"
      update-permission="logistics:manufacturing:engineeringchange:ecnotice:update"
      delete-permission="logistics:manufacturing:engineeringchange:ecnotice:delete"
      import-permission="logistics:manufacturing:engineeringchange:ecnotice:import"
      export-permission="logistics:manufacturing:engineeringchange:ecnotice:export"
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
      entity-scope="approval"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'ecNoticeId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEcNoticeId"
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
      <EcNoticeForm
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
      :storage-key="'takt-query-fields-logistics-manufacturing-engineering-change-ec-notice'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.ecNotice.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNoticeNo')">
      <a-form-item :label="t('entity.ecNotice.no')">
        <a-input
          v-model:value="advancedQueryForm.ecNoticeNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.no') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecId')">
      <a-form-item :label="t('entity.ecNotice.ecid')">
        <a-input
          v-model:value="advancedQueryForm.ecId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.ecid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNo')">
      <a-form-item :label="t('entity.ecNotice.ecno')">
        <a-input
          v-model:value="advancedQueryForm.ecNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.ecno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecTitle')">
      <a-form-item :label="t('entity.ecNotice.ectitle')">
        <a-input
          v-model:value="advancedQueryForm.ecTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.ectitle') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNoticeDateStart')">
      <a-form-item :label="t('entity.ecNotice.datestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecNoticeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecNotice.datestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNoticeDateEnd')">
      <a-form-item :label="t('entity.ecNotice.dateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecNoticeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecNotice.dateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNoticeDeptCodes')">
      <a-form-item :label="t('entity.ecNotice.deptcodes')">
        <a-input
          v-model:value="advancedQueryForm.ecNoticeDeptCodes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.deptcodes') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNoticeDeptNames')">
      <a-form-item :label="t('entity.ecNotice.deptnames')">
        <a-input
          v-model:value="advancedQueryForm.ecNoticeDeptNames"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.deptnames') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNoticeNotifierId')">
      <a-form-item :label="t('entity.ecNotice.notifierid')">
        <a-input
          v-model:value="advancedQueryForm.ecNoticeNotifierId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.notifierid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNoticeNotifierName')">
      <a-form-item :label="t('entity.ecNotice.notifiername')">
        <a-input
          v-model:value="advancedQueryForm.ecNoticeNotifierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.notifiername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNoticeMethod')">
      <a-form-item :label="t('entity.ecNotice.method')">
        <a-input-number
          v-model:value="advancedQueryForm.ecNoticeMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.method') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNoticeStatus')">
      <a-form-item :label="t('entity.ecNotice.status')">
        <a-input-number
          v-model:value="advancedQueryForm.ecNoticeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.status') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.ecNotice.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.flowinstanceid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.ecNotice.approvalstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.approvalStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.approvalstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.ecNotice.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.initiatorid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.ecNotice.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.initiatedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.ecNotice.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecNotice.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.ecNotice.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.approvedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.ecNotice.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.approvedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.ecNotice.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecNotice.approvedatend') })"
          value-format="YYYY-MM-DD"
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
      :title="t('common.dialog.title.import', { entity: t('entity.ecNotice._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ecNotice._self"
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
      :id-column-key="'ecNoticeId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 工程变更通知单实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-notice
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import EcNoticeForm from './components/ec-notice-form.vue'
import { getEcNoticeList, getEcNoticeById, createEcNotice, updateEcNotice, deleteEcNoticeById, deleteEcNoticeBatch, getEcNoticeTemplate, importEcNotice, exportEcNotice } from '@/api/logistics/manufacturing/engineering-change/ec-notice'
import type { EcNotice, EcNoticeQuery, EcNoticeCreate, EcNoticeUpdate } from '@/types/logistics/manufacturing/engineering-change/ec-notice'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEcNotice')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ecNotice._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EcNotice[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<EcNotice | null>(null)
/** 表格多选行 */
const selectedRows = ref<EcNotice[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<EcNotice>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  ecNoticeNo: '',
  ecId: '',
  ecNo: '',
  ecTitle: '',
  ecNoticeDateStart: '',
  ecNoticeDateEnd: '',
  ecNoticeDeptCodes: '',
  ecNoticeDeptNames: '',
  ecNoticeNotifierId: '',
  ecNoticeNotifierName: '',
  ecNoticeMethod: undefined as number | undefined,
  ecNoticeStatus: undefined as number | undefined,
  flowInstanceId: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.ecNotice.plantcode') },
  { key: 'ecNoticeNo', label: t('entity.ecNotice.no') },
  { key: 'ecId', label: t('entity.ecNotice.ecid') },
  { key: 'ecNo', label: t('entity.ecNotice.ecno') },
  { key: 'ecTitle', label: t('entity.ecNotice.ectitle') },
  { key: 'ecNoticeDateStart', label: t('entity.ecNotice.datestart') },
  { key: 'ecNoticeDateEnd', label: t('entity.ecNotice.dateend') },
  { key: 'ecNoticeDeptCodes', label: t('entity.ecNotice.deptcodes') },
  { key: 'ecNoticeDeptNames', label: t('entity.ecNotice.deptnames') },
  { key: 'ecNoticeNotifierId', label: t('entity.ecNotice.notifierid') },
  { key: 'ecNoticeNotifierName', label: t('entity.ecNotice.notifiername') },
  { key: 'ecNoticeMethod', label: t('entity.ecNotice.method') },
  { key: 'ecNoticeStatus', label: t('entity.ecNotice.status') },
  { key: 'flowInstanceId', label: t('entity.ecNotice.flowinstanceid') },
  { key: 'approvalStatus', label: t('entity.ecNotice.approvalstatus') },
  { key: 'initiatorId', label: t('entity.ecNotice.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.ecNotice.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.ecNotice.initiatedatend') },
  { key: 'approvedBy', label: t('entity.ecNotice.approvedby') },
  { key: 'approvedAtStart', label: t('entity.ecNotice.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.ecNotice.approvedatend') },
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
const entityIdName = 'ecNoticeId'
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
    dataIndex: 'ecNoticeId',
    key: 'ecNoticeId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecNoticeId') ?? ''
  },
  {
    title: t('entity.ecNotice.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.ecNotice.no'),
    dataIndex: 'ecNoticeNo',
    key: 'ecNoticeNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecNoticeNo') ?? ''
  },
  {
    title: t('entity.ecNotice.ecid'),
    dataIndex: 'ecId',
    key: 'ecId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecId') ?? ''
  },
  {
    title: t('entity.ecNotice.ecname'),
    dataIndex: 'ecName',
    key: 'ecName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecName') ?? ''
  },
  {
    title: t('entity.ecNotice.ecno'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecNo') ?? ''
  },
  {
    title: t('entity.ecNotice.ectitle'),
    dataIndex: 'ecTitle',
    key: 'ecTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecTitle') ?? ''
  },
  {
    title: t('entity.ecNotice.date'),
    dataIndex: 'ecNoticeDate',
    key: 'ecNoticeDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecNoticeDate') ?? ''
  },
  {
    title: t('entity.ecNotice.deptcodes'),
    dataIndex: 'ecNoticeDeptCodes',
    key: 'ecNoticeDeptCodes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecNoticeDeptCodes') ?? ''
  },
  {
    title: t('entity.ecNotice.deptnames'),
    dataIndex: 'ecNoticeDeptNames',
    key: 'ecNoticeDeptNames',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecNoticeDeptNames') ?? ''
  },
  {
    title: t('entity.ecNotice.notifierid'),
    dataIndex: 'ecNoticeNotifierId',
    key: 'ecNoticeNotifierId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecNoticeNotifierId') ?? ''
  },
  {
    title: t('entity.ecNotice.notifiername'),
    dataIndex: 'ecNoticeNotifierName',
    key: 'ecNoticeNotifierName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecNoticeNotifierName') ?? ''
  },
  {
    title: t('entity.ecNotice.method'),
    dataIndex: 'ecNoticeMethod',
    key: 'ecNoticeMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecNoticeMethod') ?? ''
  },
  {
    title: t('entity.ecNotice.status'),
    dataIndex: 'ecNoticeStatus',
    key: 'ecNoticeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ecNoticeStatus') ?? ''
  },
  {
    title: t('entity.ecNotice.flowinstanceid'),
    dataIndex: 'flowInstanceId',
    key: 'flowInstanceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'flowInstanceId') ?? ''
  },
  {
    title: t('entity.ecNotice.flowinstancename'),
    dataIndex: 'flowInstanceName',
    key: 'flowInstanceName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'flowInstanceName') ?? ''
  },
  {
    title: t('entity.ecNotice.ec'),
    dataIndex: 'ec',
    key: 'ec',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEcNoticeField(record, 'ec') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineeringchange:ecnotice:update',
        onClick: (record: EcNotice) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:engineeringchange:ecnotice:delete',
        onClick: (record: EcNotice) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEcNoticeId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEcNoticeField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcNotice[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EcNotice, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEcNoticeId(selectedRow.value) === getEcNoticeId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EcNotice[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: EcNotice) => ({
  onClick: () => {
    const key = getEcNoticeId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEcNoticeId(item)))
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
    const params: EcNoticeQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getEcNoticeList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EcNotice] 加载数据失败', { error })
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
  ecNoticeNo: '',
  ecId: '',
  ecNo: '',
  ecTitle: '',
  ecNoticeDateStart: '',
  ecNoticeDateEnd: '',
  ecNoticeDeptCodes: '',
  ecNoticeDeptNames: '',
  ecNoticeNotifierId: '',
  ecNoticeNotifierName: '',
  ecNoticeMethod: undefined as number | undefined,
  ecNoticeStatus: undefined as number | undefined,
  flowInstanceId: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ecNotice._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: EcNotice) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ecNotice._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.ecNotice._self') }))
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
      await updateEcNotice(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.ecNotice._self') }))
    } else {
      await createEcNotice(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.ecNotice._self') }))
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
  const res = await getEcNoticeTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEcNotice(file, sheetName)
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
    const exportQuery: EcNoticeQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportEcNotice(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.ecNotice._self') }))
  } catch (error: any) {
    logger.error('[EcNotice] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.ecNotice._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EcNotice) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.ecNotice._self'), name: t('common.tip.this.target', { target: t('entity.ecNotice._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEcNoticeById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.ecNotice._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.ecNotice._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.ecNotice._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEcNoticeBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ecNotice._self') }))
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
  ecNoticeNo: '',
  ecId: '',
  ecNo: '',
  ecTitle: '',
  ecNoticeDateStart: '',
  ecNoticeDateEnd: '',
  ecNoticeDeptCodes: '',
  ecNoticeDeptNames: '',
  ecNoticeNotifierId: '',
  ecNoticeNotifierName: '',
  ecNoticeMethod: undefined as number | undefined,
  ecNoticeStatus: undefined as number | undefined,
  flowInstanceId: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
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
.logistics-manufacturing-engineering-change-ec-notice {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
