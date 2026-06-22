<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-detail/components -->
<!-- 文件名称：ec-dept-panel.vue -->
<!-- 功能描述：设变主表实体右侧明细 ecDept 独立 CRUD（按主表选中 ecDetailId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="ec-dept-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.ecdept._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:engineering:change:ec:detail:create"
      update-permission="logistics:manufacturing:engineering:change:ec:detail:update"
      delete-permission="logistics:manufacturing:engineering:change:ec:detail:delete"
      import-permission="logistics:manufacturing:engineering:change:ec:detail:import"
      export-permission="logistics:manufacturing:engineering:change:ec:detail:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @refresh="handleRefresh"
    />
    <div class="ec-dept-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getEcDeptId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="ecDeptId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="single"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      />
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      width="720px"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <EcDeptForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterEcDetailId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-ec-detail-ec-dept"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ecnDetailId')">
      <a-form-item :label="t('entity.ecdept.ecndetailid')">
        <a-input
          v-model:value="advancedQueryForm.ecnDetailId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.ecndetailid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNo')">
      <a-form-item :label="t('entity.ecdept.ecno')">
        <a-input
          v-model:value="advancedQueryForm.ecNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.ecno') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.ecdept.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptCode')">
      <a-form-item :label="t('entity.ecdept.deptcode')">
        <a-input
          v-model:value="advancedQueryForm.deptCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.deptcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isImplemented')">
      <a-form-item :label="t('entity.ecdept.isimplemented')">
        <a-input-number
          v-model:value="advancedQueryForm.isImplemented"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.isimplemented') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('content')">
      <a-form-item :label="t('entity.ecdept.content')">
        <a-textarea
          v-model:value="advancedQueryForm.content"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdept.content') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledProductionDateStart')">
      <a-form-item :label="t('entity.ecdept.scheduledproductiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.scheduledProductionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.scheduledproductiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledProductionDateEnd')">
      <a-form-item :label="t('entity.ecdept.scheduledproductiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.scheduledProductionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.scheduledproductiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scheduledBatch')">
      <a-form-item :label="t('entity.ecdept.scheduledbatch')">
        <a-input
          v-model:value="advancedQueryForm.scheduledBatch"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.scheduledbatch') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('poRemainder')">
      <a-form-item :label="t('entity.ecdept.poremainder')">
        <a-input
          v-model:value="advancedQueryForm.poRemainder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.poremainder') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('balance')">
      <a-form-item :label="t('entity.ecdept.balance')">
        <a-input
          v-model:value="advancedQueryForm.balance"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.balance') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('oldProductHandling')">
      <a-form-item :label="t('entity.ecdept.oldproducthandling')">
        <a-input
          v-model:value="advancedQueryForm.oldProductHandling"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.oldproducthandling') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderIssueDateStart')">
      <a-form-item :label="t('entity.ecdept.purchaseorderissuedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.purchaseOrderIssueDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.purchaseorderissuedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderIssueDateEnd')">
      <a-form-item :label="t('entity.ecdept.purchaseorderissuedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.purchaseOrderIssueDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.purchaseorderissuedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplier')">
      <a-form-item :label="t('entity.ecdept.supplier')">
        <a-input
          v-model:value="advancedQueryForm.supplier"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.supplier') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderNo')">
      <a-form-item :label="t('entity.ecdept.purchaseorderno')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrderNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.purchaseorderno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('iqcOrderNo')">
      <a-form-item :label="t('entity.ecdept.iqcorderno')">
        <a-input
          v-model:value="advancedQueryForm.iqcOrderNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.iqcorderno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateStart')">
      <a-form-item :label="t('entity.ecdept.inspectiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.inspectiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateEnd')">
      <a-form-item :label="t('entity.ecdept.inspectiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.inspectiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundBatch')">
      <a-form-item :label="t('entity.ecdept.outboundbatch')">
        <a-input
          v-model:value="advancedQueryForm.outboundBatch"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.outboundbatch') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundDateStart')">
      <a-form-item :label="t('entity.ecdept.outbounddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.outboundDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.outbounddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundDateEnd')">
      <a-form-item :label="t('entity.ecdept.outbounddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.outboundDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.outbounddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionDateStart')">
      <a-form-item :label="t('entity.ecdept.productiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.productionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.productiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionDateEnd')">
      <a-form-item :label="t('entity.ecdept.productiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.productionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.productiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionBatch')">
      <a-form-item :label="t('entity.ecdept.productionbatch')">
        <a-input
          v-model:value="advancedQueryForm.productionBatch"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.productionbatch') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outboundOrderNo')">
      <a-form-item :label="t('entity.ecdept.outboundorderno')">
        <a-input
          v-model:value="advancedQueryForm.outboundOrderNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.outboundorderno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionTeam')">
      <a-form-item :label="t('entity.ecdept.productionteam')">
        <a-input
          v-model:value="advancedQueryForm.productionTeam"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.productionteam') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('implementationDateStart')">
      <a-form-item :label="t('entity.ecdept.implementationdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.implementationDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.implementationdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('implementationDateEnd')">
      <a-form-item :label="t('entity.ecdept.implementationdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.implementationDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdept.implementationdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionBatch')">
      <a-form-item :label="t('entity.ecdept.inspectionbatch')">
        <a-input
          v-model:value="advancedQueryForm.inspectionBatch"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.inspectionbatch') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingNo')">
      <a-form-item :label="t('entity.ecdept.samplingno')">
        <a-input
          v-model:value="advancedQueryForm.samplingNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.samplingno') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isSopUpdated')">
      <a-form-item :label="t('entity.ecdept.issopupdated')">
        <a-input-number
          v-model:value="advancedQueryForm.isSopUpdated"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdept.issopupdated') })"
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
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.ecdept._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ecdept._self"
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
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="ecDeptId"
      action-column-key="action"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变子表 ecDept 右栏面板
 * @module views/logistics/manufacturing/engineering-change/ec-detail/components
 */
import { ref, computed, watch } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import EcDeptForm from './ec-dept-form.vue'
import { useEcDetailMasterContext } from '../composables/use-ec-detail-master-context'
import {
  getEcDeptList,
  getEcDeptById,
  createEcDept,
  updateEcDept,
  deleteEcDeptById,
  deleteEcDeptBatch,
  getEcDeptTemplate,
  importEcDept,
  exportEcDept,
} from '@/api/logistics/manufacturing/engineering-change/ec-dept'
import type { EcDept, EcDeptQuery } from '@/types/logistics/manufacturing/engineering-change/ec-dept'

const { t } = useI18n()
const { selectedMasterRow } = useEcDetailMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEcDept')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ecdept._self') }),
)

const loading = ref(false)
const dataSource = ref<EcDept[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<EcDept | null>(null)
const selectedRows = ref<EcDept[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<EcDept>>({})
const formLoading = ref(false)
const formRef = ref()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  ecnDetailId: '',
  ecNo: '',
  lineNumber: undefined as number | undefined,
  deptCode: '',
  isImplemented: undefined as number | undefined,
  content: '',
  scheduledProductionDateStart: '',
  scheduledProductionDateEnd: '',
  scheduledBatch: '',
  poRemainder: '',
  balance: '',
  oldProductHandling: '',
  purchaseOrderIssueDateStart: '',
  purchaseOrderIssueDateEnd: '',
  supplier: '',
  purchaseOrderNo: '',
  iqcOrderNo: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  outboundBatch: '',
  outboundDateStart: '',
  outboundDateEnd: '',
  productionDateStart: '',
  productionDateEnd: '',
  productionBatch: '',
  outboundOrderNo: '',
  productionTeam: '',
  implementationDateStart: '',
  implementationDateEnd: '',
  inspectionBatch: '',
  samplingNo: '',
  isSopUpdated: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'ecnDetailId', label: t('entity.ecdept.ecndetailid') },
  { key: 'ecNo', label: t('entity.ecdept.ecno') },
  { key: 'lineNumber', label: t('entity.ecdept.linenumber') },
  { key: 'deptCode', label: t('entity.ecdept.deptcode') },
  { key: 'isImplemented', label: t('entity.ecdept.isimplemented') },
  { key: 'content', label: t('entity.ecdept.content') },
  { key: 'scheduledProductionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdept.scheduledproductiondate')) },
  { key: 'scheduledProductionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdept.scheduledproductiondate')) },
  { key: 'scheduledBatch', label: t('entity.ecdept.scheduledbatch') },
  { key: 'poRemainder', label: t('entity.ecdept.poremainder') },
  { key: 'balance', label: t('entity.ecdept.balance') },
  { key: 'oldProductHandling', label: t('entity.ecdept.oldproducthandling') },
  { key: 'purchaseOrderIssueDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdept.purchaseorderissuedate')) },
  { key: 'purchaseOrderIssueDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdept.purchaseorderissuedate')) },
  { key: 'supplier', label: t('entity.ecdept.supplier') },
  { key: 'purchaseOrderNo', label: t('entity.ecdept.purchaseorderno') },
  { key: 'iqcOrderNo', label: t('entity.ecdept.iqcorderno') },
  { key: 'inspectionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdept.inspectiondate')) },
  { key: 'inspectionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdept.inspectiondate')) },
  { key: 'outboundBatch', label: t('entity.ecdept.outboundbatch') },
  { key: 'outboundDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdept.outbounddate')) },
  { key: 'outboundDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdept.outbounddate')) },
  { key: 'productionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdept.productiondate')) },
  { key: 'productionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdept.productiondate')) },
  { key: 'productionBatch', label: t('entity.ecdept.productionbatch') },
  { key: 'outboundOrderNo', label: t('entity.ecdept.outboundorderno') },
  { key: 'productionTeam', label: t('entity.ecdept.productionteam') },
  { key: 'implementationDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ecdept.implementationdate')) },
  { key: 'implementationDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ecdept.implementationdate')) },
  { key: 'inspectionBatch', label: t('entity.ecdept.inspectionbatch') },
  { key: 'samplingNo', label: t('entity.ecdept.samplingno') },
  { key: 'isSopUpdated', label: t('entity.ecdept.issopupdated') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])

/**
 * 高级查询字段标签
 * @param key 字段 key
 */
function fieldLabel(key: string): string {
  return queryFieldsMeta.value.find((f) => f.key === key)?.label ?? key
}

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  ecnDetailId: '',
  ecNo: '',
  lineNumber: undefined as number | undefined,
  deptCode: '',
  isImplemented: undefined as number | undefined,
  content: '',
  scheduledProductionDateStart: '',
  scheduledProductionDateEnd: '',
  scheduledBatch: '',
  poRemainder: '',
  balance: '',
  oldProductHandling: '',
  purchaseOrderIssueDateStart: '',
  purchaseOrderIssueDateEnd: '',
  supplier: '',
  purchaseOrderNo: '',
  iqcOrderNo: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  outboundBatch: '',
  outboundDateStart: '',
  outboundDateEnd: '',
  productionDateStart: '',
  productionDateEnd: '',
  productionBatch: '',
  outboundOrderNo: '',
  productionTeam: '',
  implementationDateStart: '',
  implementationDateEnd: '',
  inspectionBatch: '',
  samplingNo: '',
  isSopUpdated: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}
const importVisible = ref(false)

const entityIdName = 'ecDeptId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.ecDetailId)
const masterEcDetailId = computed(() => selectedMasterRow.value?.ecDetailId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getEcDeptId(record: EcDept | Record<string, unknown>): string {
  return String((record as EcDept)?.[entityIdName] ?? '')
}

function getEcDeptField(record: EcDept | Record<string, unknown>, field: string): unknown {
  return (record as EcDept)?.[field as keyof EcDept]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ecDeptId',
    key: 'ecDeptId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: EcDept }) =>
      String(getEcDeptField(record, 'ecDeptId') ?? ''),
  },
  {
    title: t('entity.ecdept.ecndetailid'),
    dataIndex: 'ecnDetailId',
    key: 'ecnDetailId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDept }) =>
      String(getEcDeptField(record, 'ecnDetailId') ?? ''),
  },
  {
    title: t('entity.ecdept.ecno'),
    dataIndex: 'ecNo',
    key: 'ecNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDept }) =>
      String(getEcDeptField(record, 'ecNo') ?? ''),
  },
  {
    title: t('entity.ecdept.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDept }) =>
      String(getEcDeptField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.ecdept.deptcode'),
    dataIndex: 'deptCode',
    key: 'deptCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDept }) =>
      String(getEcDeptField(record, 'deptCode') ?? ''),
  },
  {
    title: t('entity.ecdept.isimplemented'),
    dataIndex: 'isImplemented',
    key: 'isImplemented',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDept }) =>
      String(getEcDeptField(record, 'isImplemented') ?? ''),
  },
  {
    title: t('entity.ecdept.content'),
    dataIndex: 'content',
    key: 'content',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDept }) =>
      String(getEcDeptField(record, 'content') ?? ''),
  },
  {
    title: t('entity.ecdept.scheduledproductiondate'),
    dataIndex: 'scheduledProductionDate',
    key: 'scheduledProductionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDept }) =>
      String(getEcDeptField(record, 'scheduledProductionDate') ?? ''),
  },
  {
    title: t('entity.ecdept.scheduledbatch'),
    dataIndex: 'scheduledBatch',
    key: 'scheduledBatch',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcDept }) =>
      String(getEcDeptField(record, 'scheduledBatch') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineering:change:ec:detail:update',
        onClick: (record: EcDept) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:engineering:change:ec:detail:delete',
        onClick: (record: EcDept) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcDept[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EcDept, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEcDeptId(selectedRow.value) === getEcDeptId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EcDept[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: EcDept) {
  const key = getEcDeptId(record)
  return {
    onClick: () => {
      selectedRowKeys.value = [key]
      selectedRows.value = [record]
      selectedRow.value = record
    },
    class: selectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EcDeptQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EcDeptQuery>): EcDeptQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EcDeptQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ecDetailId: masterEcDetailId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EcDeptQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ecnDetailId', form.ecnDetailId)
  assignTrimmed('ecNo', form.ecNo)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('deptCode', form.deptCode)
  if (form.isImplemented !== undefined && form.isImplemented !== null) {
    query.isImplemented = form.isImplemented
  }
  assignTrimmed('content', form.content)
  assignTrimmed('scheduledProductionDateStart', form.scheduledProductionDateStart)
  assignTrimmed('scheduledProductionDateEnd', form.scheduledProductionDateEnd)
  assignTrimmed('scheduledBatch', form.scheduledBatch)
  assignTrimmed('poRemainder', form.poRemainder)
  assignTrimmed('balance', form.balance)
  assignTrimmed('oldProductHandling', form.oldProductHandling)
  assignTrimmed('purchaseOrderIssueDateStart', form.purchaseOrderIssueDateStart)
  assignTrimmed('purchaseOrderIssueDateEnd', form.purchaseOrderIssueDateEnd)
  assignTrimmed('supplier', form.supplier)
  assignTrimmed('purchaseOrderNo', form.purchaseOrderNo)
  assignTrimmed('iqcOrderNo', form.iqcOrderNo)
  assignTrimmed('inspectionDateStart', form.inspectionDateStart)
  assignTrimmed('inspectionDateEnd', form.inspectionDateEnd)
  assignTrimmed('outboundBatch', form.outboundBatch)
  assignTrimmed('outboundDateStart', form.outboundDateStart)
  assignTrimmed('outboundDateEnd', form.outboundDateEnd)
  assignTrimmed('productionDateStart', form.productionDateStart)
  assignTrimmed('productionDateEnd', form.productionDateEnd)
  assignTrimmed('productionBatch', form.productionBatch)
  assignTrimmed('outboundOrderNo', form.outboundOrderNo)
  assignTrimmed('productionTeam', form.productionTeam)
  assignTrimmed('implementationDateStart', form.implementationDateStart)
  assignTrimmed('implementationDateEnd', form.implementationDateEnd)
  assignTrimmed('inspectionBatch', form.inspectionBatch)
  assignTrimmed('samplingNo', form.samplingNo)
  if (form.isSopUpdated !== undefined && form.isSopUpdated !== null) {
    query.isSopUpdated = form.isSopUpdated
  }
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getEcDeptList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(masterEcDetailId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ecdept._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: EcDept) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ecdept._self') })
  formLoading.value = true
  try {
    const detail = await getEcDeptById(getEcDeptId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: t('entity.ecdept._self'),
    }))
  }
}

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
    const payload = refInst.getValues?.()
    const id = formData.value?.ecDeptId
    if (id) {
      await updateEcDept(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.ecdept._self') }))
    } else {
      await createEcDept(payload)
      message.success(t('common.feedback.created', { target: t('entity.ecdept._self') }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: EcDept) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.ecdept._self'),
      name: t('common.tip.this.target', { target: t('entity.ecdept._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEcDeptById(getEcDeptId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.ecdept._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.ecdept._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.ecdept._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getEcDeptId(r)).filter(Boolean)
      await deleteEcDeptBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ecdept._self') }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleImport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getEcDeptTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEcDept(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    const exportMeta = await exportEcDept(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: t('entity.ecdept._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.ecdept._self') }))
  } finally {
    loading.value = false
  }
}
function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>
