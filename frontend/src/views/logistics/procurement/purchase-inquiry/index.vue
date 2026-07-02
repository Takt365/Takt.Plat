<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-inquiry -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：采购询价实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:procurement:purchase:inquiry:create"
      update-permission="logistics:procurement:purchase:inquiry:update"
      delete-permission="logistics:procurement:purchase:inquiry:delete"
      import-permission="logistics:procurement:purchase:inquiry:import"
      export-permission="logistics:procurement:purchase:inquiry:export"
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
      :master-row-key="getPurchaseInquiryId"
      :master-row-selection="rowSelection"
      master-id-column-key="purchaseInquiryId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="approval"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'inquiryStatus'">
          <a-switch
            :checked="getPurchaseInquiryField(record, 'inquiryStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleInquiryStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'convertedStatus'">
          <TaktDictTag
            :value="getPurchaseInquiryField(record, 'convertedStatus')"
            dict-type="sys_convert_status"
          />
        </template>
      </template>
      <template #detail>
        <PurchaseInquiryItemPanel
          ref="purchaseInquiryItemPanelRef"
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
      <PurchaseInquiryForm
        :key="formData?.purchaseInquiryId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-procurement-purchase-inquiry'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.purchaseinquiry.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.plantcode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseInquiryCode')">
      <a-form-item :label="t('entity.purchaseinquiry.code')">
        <a-input
          v-model:value="advancedQueryForm.purchaseInquiryCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.code') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inquiryDateStart')">
      <a-form-item :label="t('entity.purchaseinquiry.inquirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.inquiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.inquirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inquiryDateEnd')">
      <a-form-item :label="t('entity.purchaseinquiry.inquirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.inquiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.inquirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quoteDeadlineDateStart')">
      <a-form-item :label="t('entity.purchaseinquiry.quotedeadlinedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.quoteDeadlineDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.quotedeadlinedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quoteDeadlineDateEnd')">
      <a-form-item :label="t('entity.purchaseinquiry.quotedeadlinedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.quoteDeadlineDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.quotedeadlinedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inquiryId')">
      <a-form-item :label="t('entity.purchaseinquiry.inquiryid')">
        <a-input
          v-model:value="advancedQueryForm.inquiryId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.inquiryid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inquiryBy')">
      <a-form-item :label="t('entity.purchaseinquiry.inquiryby')">
        <a-input
          v-model:value="advancedQueryForm.inquiryBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.inquiryby') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="t('entity.purchaseinquiry.suppliercode')">
        <a-input
          v-model:value="advancedQueryForm.supplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.suppliercode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierName')">
      <a-form-item :label="t('entity.purchaseinquiry.suppliername')">
        <a-input
          v-model:value="advancedQueryForm.supplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.suppliername') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQuantity')">
      <a-form-item :label="t('entity.purchaseinquiry.totalquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.totalquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="t('entity.purchaseinquiry.totalamount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.totalamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedQuantity')">
      <a-form-item :label="t('entity.purchaseinquiry.convertedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.convertedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedAmount')">
      <a-form-item :label="t('entity.purchaseinquiry.convertedamount')">
        <a-input-number
          v-model:value="advancedQueryForm.convertedAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.convertedamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inquiryReason')">
      <a-form-item :label="t('entity.purchaseinquiry.inquiryreason')">
        <a-input
          v-model:value="advancedQueryForm.inquiryReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.inquiryreason') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inquiryStatus')">
      <a-form-item :label="t('entity.purchaseinquiry.inquirystatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.inquiryStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.inquirystatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('convertedStatus')">
      <a-form-item :label="t('entity.purchaseinquiry.convertedstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.convertedStatus"
          dict-type="sys_convert_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.convertedstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.purchaseinquiry.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.purchaseinquiry.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.purchaseinquiry.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.purchaseinquiry.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.purchaseinquiry.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.purchaseinquiry.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.purchaseinquiry.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseinquiry.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.purchaseinquiry.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseinquiry.flowinstanceid') })"
          show-count
          :maxlength="20"
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
      :title="t('common.dialog.title.import', { entity: t('entity.purchaseinquiry._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.purchaseinquiry._self"
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
      :id-column-key="'purchaseInquiryId'"
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
 * 采购询价实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/purchase-inquiry
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import PurchaseInquiryForm from './components/purchase-inquiry-form.vue'
import PurchaseInquiryItemPanel from './components/purchase-inquiry-item-panel.vue'
import { providePurchaseInquiryMasterContext } from './composables/use-purchase-inquiry-master-context'
import { getPurchaseInquiryList, getPurchaseInquiryById, createPurchaseInquiry, updatePurchaseInquiry, deletePurchaseInquiryById, deletePurchaseInquiryBatch, getPurchaseInquiryTemplate, importPurchaseInquiry, exportPurchaseInquiry, updatePurchaseInquiryStatus } from '@/api/logistics/procurement/purchase-inquiry'
import type { PurchaseInquiry, PurchaseInquiryQuery } from '@/types/logistics/procurement/purchase-inquiry'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchaseInquiry')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.purchaseinquiry._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PurchaseInquiry[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<PurchaseInquiry | null>(null)
/** 表格多选行 */
const selectedRows = ref<PurchaseInquiry[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PurchaseInquiry> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  purchaseInquiryCode: '',
  inquiryDateStart: '',
  inquiryDateEnd: '',
  quoteDeadlineDateStart: '',
  quoteDeadlineDateEnd: '',
  inquiryId: '',
  inquiryBy: '',
  supplierCode: '',
  supplierName: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  convertedQuantity: undefined as number | undefined,
  convertedAmount: undefined as number | undefined,
  inquiryReason: '',
  inquiryStatus: undefined as number | undefined,
  convertedStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.purchaseinquiry.plantcode') },
  { key: 'purchaseInquiryCode', label: t('entity.purchaseinquiry.code') },
  { key: 'inquiryDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.purchaseinquiry.inquirydate')) },
  { key: 'inquiryDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.purchaseinquiry.inquirydate')) },
  { key: 'quoteDeadlineDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.purchaseinquiry.quotedeadlinedate')) },
  { key: 'quoteDeadlineDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.purchaseinquiry.quotedeadlinedate')) },
  { key: 'inquiryId', label: t('entity.purchaseinquiry.inquiryid') },
  { key: 'inquiryBy', label: t('entity.purchaseinquiry.inquiryby') },
  { key: 'supplierCode', label: t('entity.purchaseinquiry.suppliercode') },
  { key: 'supplierName', label: t('entity.purchaseinquiry.suppliername') },
  { key: 'totalQuantity', label: t('entity.purchaseinquiry.totalquantity') },
  { key: 'totalAmount', label: t('entity.purchaseinquiry.totalamount') },
  { key: 'convertedQuantity', label: t('entity.purchaseinquiry.convertedquantity') },
  { key: 'convertedAmount', label: t('entity.purchaseinquiry.convertedamount') },
  { key: 'inquiryReason', label: t('entity.purchaseinquiry.inquiryreason') },
  { key: 'inquiryStatus', label: t('entity.purchaseinquiry.inquirystatus') },
  { key: 'convertedStatus', label: t('entity.purchaseinquiry.convertedstatus') },
  { key: 'approvalStatus', label: t('entity.purchaseinquiry.approvalstatus') },
  { key: 'initiatorId', label: t('entity.purchaseinquiry.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.purchaseinquiry.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.purchaseinquiry.initiatedatend') },
  { key: 'approvedBy', label: t('entity.purchaseinquiry.approvedby') },
  { key: 'approvedAtStart', label: t('entity.purchaseinquiry.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.purchaseinquiry.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.purchaseinquiry.flowinstanceid') },
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
const entityIdName = 'purchaseInquiryId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = providePurchaseInquiryMasterContext()
const purchaseInquiryItemPanelRef = ref<InstanceType<typeof PurchaseInquiryItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PurchaseInquiryQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PurchaseInquiryQuery>): PurchaseInquiryQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PurchaseInquiryQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PurchaseInquiryQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('purchaseInquiryCode', form.purchaseInquiryCode)
  assignTrimmed('inquiryDateStart', form.inquiryDateStart)
  assignTrimmed('inquiryDateEnd', form.inquiryDateEnd)
  assignTrimmed('quoteDeadlineDateStart', form.quoteDeadlineDateStart)
  assignTrimmed('quoteDeadlineDateEnd', form.quoteDeadlineDateEnd)
  assignTrimmed('inquiryId', form.inquiryId)
  assignTrimmed('inquiryBy', form.inquiryBy)
  assignTrimmed('supplierCode', form.supplierCode)
  assignTrimmed('supplierName', form.supplierName)
  if (form.totalQuantity !== undefined && form.totalQuantity !== null) {
    query.totalQuantity = form.totalQuantity
  }
  if (form.totalAmount !== undefined && form.totalAmount !== null) {
    query.totalAmount = form.totalAmount
  }
  if (form.convertedQuantity !== undefined && form.convertedQuantity !== null) {
    query.convertedQuantity = form.convertedQuantity
  }
  if (form.convertedAmount !== undefined && form.convertedAmount !== null) {
    query.convertedAmount = form.convertedAmount
  }
  assignTrimmed('inquiryReason', form.inquiryReason)
  if (form.inquiryStatus !== undefined && form.inquiryStatus !== null) {
    query.inquiryStatus = form.inquiryStatus
  }
  if (form.convertedStatus !== undefined && form.convertedStatus !== null) {
    query.convertedStatus = form.convertedStatus
  }
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  assignTrimmed('initiatorId', form.initiatorId)
  assignTrimmed('initiatedAtStart', form.initiatedAtStart)
  assignTrimmed('initiatedAtEnd', form.initiatedAtEnd)
  assignTrimmed('approvedBy', form.approvedBy)
  assignTrimmed('approvedAtStart', form.approvedAtStart)
  assignTrimmed('approvedAtEnd', form.approvedAtEnd)
  assignTrimmed('flowInstanceId', form.flowInstanceId)
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
function syncMasterSelection(record: PurchaseInquiry | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getPurchaseInquiryId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as PurchaseInquiry
  const key = getPurchaseInquiryId(row)
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
async function loadPurchaseInquiryDetail(record: PurchaseInquiry): Promise<PurchaseInquiry | null> {
  const id = getPurchaseInquiryId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getPurchaseInquiryById(id)
    const index = dataSource.value.findIndex((row) => getPurchaseInquiryId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as PurchaseInquiry
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
    dataIndex: 'purchaseInquiryId',
    key: 'purchaseInquiryId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'purchaseInquiryId') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.code'),
    dataIndex: 'purchaseInquiryCode',
    key: 'purchaseInquiryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'purchaseInquiryCode') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.inquirydate'),
    dataIndex: 'inquiryDate',
    key: 'inquiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'inquiryDate') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.quotedeadlinedate'),
    dataIndex: 'quoteDeadlineDate',
    key: 'quoteDeadlineDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'quoteDeadlineDate') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.inquiryid'),
    dataIndex: 'inquiryId',
    key: 'inquiryId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'inquiryId') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.inquiryby'),
    dataIndex: 'inquiryBy',
    key: 'inquiryBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'inquiryBy') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.suppliercode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.suppliername'),
    dataIndex: 'supplierName',
    key: 'supplierName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'supplierName') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.totalquantity'),
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'totalQuantity') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.totalamount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'totalAmount') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.convertedquantity'),
    dataIndex: 'convertedQuantity',
    key: 'convertedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'convertedQuantity') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.convertedamount'),
    dataIndex: 'convertedAmount',
    key: 'convertedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'convertedAmount') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.inquiryreason'),
    dataIndex: 'inquiryReason',
    key: 'inquiryReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseInquiryField(record, 'inquiryReason') ?? ''
  },
  {
    title: t('entity.purchaseinquiry.inquirystatus'),
    dataIndex: 'inquiryStatus',
    key: 'inquiryStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.purchaseinquiry.convertedstatus'),
    dataIndex: 'convertedStatus',
    key: 'convertedStatus',
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
        permission: 'logistics:procurement:purchase:inquiry:update',
        onClick: (record: PurchaseInquiry) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:procurement:purchase:inquiry:delete',
        onClick: (record: PurchaseInquiry) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPurchaseInquiryId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPurchaseInquiryField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchaseInquiry[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: PurchaseInquiry, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getPurchaseInquiryId(selectedRow.value) === getPurchaseInquiryId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseInquiry[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getPurchaseInquiryList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PurchaseInquiry] 加载数据失败', { error })
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
  purchaseInquiryCode: '',
  inquiryDateStart: '',
  inquiryDateEnd: '',
  quoteDeadlineDateStart: '',
  quoteDeadlineDateEnd: '',
  inquiryId: '',
  inquiryBy: '',
  supplierCode: '',
  supplierName: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  convertedQuantity: undefined as number | undefined,
  convertedAmount: undefined as number | undefined,
  inquiryReason: '',
  inquiryStatus: undefined as number | undefined,
  convertedStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.purchaseinquiry._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: PurchaseInquiry) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.purchaseinquiry._self') })
  formLoading.value = true
  try {
    const detail = await loadPurchaseInquiryDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.purchaseinquiry._self') }))
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
      await updatePurchaseInquiry(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.purchaseinquiry._self') }))
    } else {
      await createPurchaseInquiry(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.purchaseinquiry._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  purchaseInquiryItemPanelRef.value?.reload?.()
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
  const res = await getPurchaseInquiryTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPurchaseInquiry(file, sheetName)
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
    const exportMeta = await exportPurchaseInquiry(
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
    message.success(t('common.feedback.export.success', { target: t('entity.purchaseinquiry._self') }))
  } catch (error: any) {
    logger.error('[PurchaseInquiry] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.purchaseinquiry._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PurchaseInquiry) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.purchaseinquiry._self'), name: t('common.tip.this.target', { target: t('entity.purchaseinquiry._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseInquiryById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseinquiry._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.purchaseinquiry._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.purchaseinquiry._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePurchaseInquiryBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.purchaseinquiry._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleInquiryStatusChange(record: PurchaseInquiry, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getPurchaseInquiryField(record, 'inquiryStatus')
  const id = getPurchaseInquiryId(record)
  const row = dataSource.value.find((item) => getPurchaseInquiryId(item) === id)
  if (row) {
    row.inquiryStatus = newVal
  }
  try {
    await updatePurchaseInquiryStatus({ purchaseInquiryId: id, inquiryStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.inquiryStatus = oldVal
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
  purchaseInquiryCode: '',
  inquiryDateStart: '',
  inquiryDateEnd: '',
  quoteDeadlineDateStart: '',
  quoteDeadlineDateEnd: '',
  inquiryId: '',
  inquiryBy: '',
  supplierCode: '',
  supplierName: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  convertedQuantity: undefined as number | undefined,
  convertedAmount: undefined as number | undefined,
  inquiryReason: '',
  inquiryStatus: undefined as number | undefined,
  convertedStatus: undefined as number | undefined,
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
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
