<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/supplier-evaluation -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：供应商评价考核主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:quality:complaint:supplier:evaluation:create"
      update-permission="logistics:quality:complaint:supplier:evaluation:update"
      delete-permission="logistics:quality:complaint:supplier:evaluation:delete"
      import-permission="logistics:quality:complaint:supplier:evaluation:import"
      export-permission="logistics:quality:complaint:supplier:evaluation:export"
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
      :master-row-key="getSupplierEvaluationId"
      :master-row-selection="rowSelection"
      master-id-column-key="supplierEvaluationId"
      :master-visible-column-keys="visibleColumnKeys"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #detail>
        <SupplierEvaluationItemPanel
          ref="supplierEvaluationItemPanelRef"
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
      <SupplierEvaluationForm
        :key="formData?.supplierEvaluationId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-quality-complaint-supplier-evaluation'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('supplierEvaluationCode')">
      <a-form-item :label="t('entity.supplierevaluation.code')">
        <a-input
          v-model:value="advancedQueryForm.supplierEvaluationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierId')">
      <a-form-item :label="t('entity.supplierevaluation.supplierid')">
        <a-input
          v-model:value="advancedQueryForm.supplierId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.supplierid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierName')">
      <a-form-item :label="t('entity.supplierevaluation.suppliername')">
        <a-input
          v-model:value="advancedQueryForm.supplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.suppliername') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="t('entity.supplierevaluation.suppliercode')">
        <a-input
          v-model:value="advancedQueryForm.supplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.suppliercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationDateStart')">
      <a-form-item :label="t('entity.supplierevaluation.evaluationdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.evaluationDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationDateEnd')">
      <a-form-item :label="t('entity.supplierevaluation.evaluationdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.evaluationDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.evaluationdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationPeriod')">
      <a-form-item :label="t('entity.supplierevaluation.evaluationperiod')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationPeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluationperiod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationType')">
      <a-form-item :label="t('entity.supplierevaluation.evaluationtype')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluationtype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluatorBy')">
      <a-form-item :label="t('entity.supplierevaluation.evaluatorby')">
        <a-input
          v-model:value="advancedQueryForm.evaluatorBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluatorby') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationDept')">
      <a-form-item :label="t('entity.supplierevaluation.evaluationdept')">
        <a-input
          v-model:value="advancedQueryForm.evaluationDept"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluationdept') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overallRating')">
      <a-form-item :label="t('entity.supplierevaluation.overallrating')">
        <a-input-number
          v-model:value="advancedQueryForm.overallRating"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.overallrating') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalScore')">
      <a-form-item :label="t('entity.supplierevaluation.totalscore')">
        <a-input-number
          v-model:value="advancedQueryForm.totalScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.totalscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityScore')">
      <a-form-item :label="t('entity.supplierevaluation.qualityscore')">
        <a-input-number
          v-model:value="advancedQueryForm.qualityScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.qualityscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryScore')">
      <a-form-item :label="t('entity.supplierevaluation.deliveryscore')">
        <a-input-number
          v-model:value="advancedQueryForm.deliveryScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.deliveryscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceScore')">
      <a-form-item :label="t('entity.supplierevaluation.pricescore')">
        <a-input-number
          v-model:value="advancedQueryForm.priceScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.pricescore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceScore')">
      <a-form-item :label="t('entity.supplierevaluation.servicescore')">
        <a-input-number
          v-model:value="advancedQueryForm.serviceScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.servicescore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('technicalScore')">
      <a-form-item :label="t('entity.supplierevaluation.technicalscore')">
        <a-input-number
          v-model:value="advancedQueryForm.technicalScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.technicalscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mainStrengths')">
      <a-form-item :label="t('entity.supplierevaluation.mainstrengths')">
        <a-input
          v-model:value="advancedQueryForm.mainStrengths"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.mainstrengths') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mainIssues')">
      <a-form-item :label="t('entity.supplierevaluation.mainissues')">
        <a-input
          v-model:value="advancedQueryForm.mainIssues"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.mainissues') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementRequirements')">
      <a-form-item :label="t('entity.supplierevaluation.improvementrequirements')">
        <a-input
          v-model:value="advancedQueryForm.improvementRequirements"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.improvementrequirements') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationConclusion')">
      <a-form-item :label="t('entity.supplierevaluation.evaluationconclusion')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationConclusion"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluationconclusion') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationDeadlineStart')">
      <a-form-item :label="t('entity.supplierevaluation.rectificationdeadlinestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.rectificationDeadlineStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.rectificationdeadlinestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationDeadlineEnd')">
      <a-form-item :label="t('entity.supplierevaluation.rectificationdeadlineend')">
        <a-date-picker
          v-model:value="advancedQueryForm.rectificationDeadlineEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplierevaluation.rectificationdeadlineend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationStatus')">
      <a-form-item :label="t('entity.supplierevaluation.evaluationstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.evaluationstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationStatus')">
      <a-form-item :label="t('entity.supplierevaluation.rectificationstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.rectificationStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.rectificationstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.supplierevaluation.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierevaluation.relatedplant') })"
          show-count
          :maxlength="4"
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
      :title="t('common.dialog.title.import', { entity: t('entity.supplierevaluation._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.supplierevaluation._self"
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
      :id-column-key="'supplierEvaluationId'"
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
 * 供应商评价考核主表实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/supplier-evaluation
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import SupplierEvaluationForm from './components/supplier-evaluation-form.vue'
import SupplierEvaluationItemPanel from './components/supplier-evaluation-item-panel.vue'
import { provideSupplierEvaluationMasterContext } from './composables/use-supplier-evaluation-master-context'
import { getSupplierEvaluationList, getSupplierEvaluationById, createSupplierEvaluation, updateSupplierEvaluation, deleteSupplierEvaluationById, deleteSupplierEvaluationBatch, getSupplierEvaluationTemplate, importSupplierEvaluation, exportSupplierEvaluation, updateSupplierEvaluationStatus } from '@/api/logistics/quality/complaint/supplier-evaluation'
import type { SupplierEvaluation, SupplierEvaluationQuery } from '@/types/logistics/quality/complaint/supplier-evaluation'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSupplierEvaluation')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.supplierevaluation._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SupplierEvaluation[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SupplierEvaluation | null>(null)
/** 表格多选行 */
const selectedRows = ref<SupplierEvaluation[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SupplierEvaluation> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  supplierEvaluationCode: '',
  supplierId: '',
  supplierName: '',
  supplierCode: '',
  evaluationDateStart: '',
  evaluationDateEnd: '',
  evaluationPeriod: undefined as number | undefined,
  evaluationType: undefined as number | undefined,
  evaluatorBy: '',
  evaluationDept: '',
  overallRating: undefined as number | undefined,
  totalScore: undefined as number | undefined,
  qualityScore: undefined as number | undefined,
  deliveryScore: undefined as number | undefined,
  priceScore: undefined as number | undefined,
  serviceScore: undefined as number | undefined,
  technicalScore: undefined as number | undefined,
  mainStrengths: '',
  mainIssues: '',
  improvementRequirements: '',
  evaluationConclusion: undefined as number | undefined,
  rectificationDeadlineStart: '',
  rectificationDeadlineEnd: '',
  evaluationStatus: undefined as number | undefined,
  rectificationStatus: undefined as number | undefined,
  relatedPlant: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'supplierEvaluationCode', label: t('entity.supplierevaluation.code') },
  { key: 'supplierId', label: t('entity.supplierevaluation.supplierid') },
  { key: 'supplierName', label: t('entity.supplierevaluation.suppliername') },
  { key: 'supplierCode', label: t('entity.supplierevaluation.suppliercode') },
  { key: 'evaluationDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.supplierevaluation.evaluationdate')) },
  { key: 'evaluationDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.supplierevaluation.evaluationdate')) },
  { key: 'evaluationPeriod', label: t('entity.supplierevaluation.evaluationperiod') },
  { key: 'evaluationType', label: t('entity.supplierevaluation.evaluationtype') },
  { key: 'evaluatorBy', label: t('entity.supplierevaluation.evaluatorby') },
  { key: 'evaluationDept', label: t('entity.supplierevaluation.evaluationdept') },
  { key: 'overallRating', label: t('entity.supplierevaluation.overallrating') },
  { key: 'totalScore', label: t('entity.supplierevaluation.totalscore') },
  { key: 'qualityScore', label: t('entity.supplierevaluation.qualityscore') },
  { key: 'deliveryScore', label: t('entity.supplierevaluation.deliveryscore') },
  { key: 'priceScore', label: t('entity.supplierevaluation.pricescore') },
  { key: 'serviceScore', label: t('entity.supplierevaluation.servicescore') },
  { key: 'technicalScore', label: t('entity.supplierevaluation.technicalscore') },
  { key: 'mainStrengths', label: t('entity.supplierevaluation.mainstrengths') },
  { key: 'mainIssues', label: t('entity.supplierevaluation.mainissues') },
  { key: 'improvementRequirements', label: t('entity.supplierevaluation.improvementrequirements') },
  { key: 'evaluationConclusion', label: t('entity.supplierevaluation.evaluationconclusion') },
  { key: 'rectificationDeadlineStart', label: t('entity.supplierevaluation.rectificationdeadlinestart') },
  { key: 'rectificationDeadlineEnd', label: t('entity.supplierevaluation.rectificationdeadlineend') },
  { key: 'evaluationStatus', label: t('entity.supplierevaluation.evaluationstatus') },
  { key: 'rectificationStatus', label: t('entity.supplierevaluation.rectificationstatus') },
  { key: 'relatedPlant', label: t('entity.supplierevaluation.relatedplant') },
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
const entityIdName = 'supplierEvaluationId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideSupplierEvaluationMasterContext()
const supplierEvaluationItemPanelRef = ref<InstanceType<typeof SupplierEvaluationItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SupplierEvaluationQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SupplierEvaluationQuery>): SupplierEvaluationQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SupplierEvaluationQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SupplierEvaluationQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('supplierEvaluationCode', form.supplierEvaluationCode)
  assignTrimmed('supplierId', form.supplierId)
  assignTrimmed('supplierName', form.supplierName)
  assignTrimmed('supplierCode', form.supplierCode)
  assignTrimmed('evaluationDateStart', form.evaluationDateStart)
  assignTrimmed('evaluationDateEnd', form.evaluationDateEnd)
  if (form.evaluationPeriod !== undefined && form.evaluationPeriod !== null) {
    query.evaluationPeriod = form.evaluationPeriod
  }
  if (form.evaluationType !== undefined && form.evaluationType !== null) {
    query.evaluationType = form.evaluationType
  }
  assignTrimmed('evaluatorBy', form.evaluatorBy)
  assignTrimmed('evaluationDept', form.evaluationDept)
  if (form.overallRating !== undefined && form.overallRating !== null) {
    query.overallRating = form.overallRating
  }
  if (form.totalScore !== undefined && form.totalScore !== null) {
    query.totalScore = form.totalScore
  }
  if (form.qualityScore !== undefined && form.qualityScore !== null) {
    query.qualityScore = form.qualityScore
  }
  if (form.deliveryScore !== undefined && form.deliveryScore !== null) {
    query.deliveryScore = form.deliveryScore
  }
  if (form.priceScore !== undefined && form.priceScore !== null) {
    query.priceScore = form.priceScore
  }
  if (form.serviceScore !== undefined && form.serviceScore !== null) {
    query.serviceScore = form.serviceScore
  }
  if (form.technicalScore !== undefined && form.technicalScore !== null) {
    query.technicalScore = form.technicalScore
  }
  assignTrimmed('mainStrengths', form.mainStrengths)
  assignTrimmed('mainIssues', form.mainIssues)
  assignTrimmed('improvementRequirements', form.improvementRequirements)
  if (form.evaluationConclusion !== undefined && form.evaluationConclusion !== null) {
    query.evaluationConclusion = form.evaluationConclusion
  }
  assignTrimmed('rectificationDeadlineStart', form.rectificationDeadlineStart)
  assignTrimmed('rectificationDeadlineEnd', form.rectificationDeadlineEnd)
  if (form.evaluationStatus !== undefined && form.evaluationStatus !== null) {
    query.evaluationStatus = form.evaluationStatus
  }
  if (form.rectificationStatus !== undefined && form.rectificationStatus !== null) {
    query.rectificationStatus = form.rectificationStatus
  }
  assignTrimmed('relatedPlant', form.relatedPlant)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})


/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: SupplierEvaluation | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getSupplierEvaluationId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as SupplierEvaluation
  const key = getSupplierEvaluationId(row)
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
async function loadSupplierEvaluationDetail(record: SupplierEvaluation): Promise<SupplierEvaluation | null> {
  const id = getSupplierEvaluationId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getSupplierEvaluationById(id)
    const index = dataSource.value.findIndex((row) => getSupplierEvaluationId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as SupplierEvaluation
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
    dataIndex: 'supplierEvaluationId',
    key: 'supplierEvaluationId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierEvaluationId') ?? ''
  },
  {
    title: t('entity.supplierevaluation.code'),
    dataIndex: 'supplierEvaluationCode',
    key: 'supplierEvaluationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierEvaluationCode') ?? ''
  },
  {
    title: t('entity.supplierevaluation.supplierid'),
    dataIndex: 'supplierId',
    key: 'supplierId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierId') ?? ''
  },
  {
    title: t('entity.supplierevaluation.suppliername'),
    dataIndex: 'supplierName',
    key: 'supplierName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierName') ?? ''
  },
  {
    title: t('entity.supplierevaluation.suppliercode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.supplierevaluation.evaluationdate'),
    dataIndex: 'evaluationDate',
    key: 'evaluationDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationDate') ?? ''
  },
  {
    title: t('entity.supplierevaluation.evaluationperiod'),
    dataIndex: 'evaluationPeriod',
    key: 'evaluationPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationPeriod') ?? ''
  },
  {
    title: t('entity.supplierevaluation.evaluationtype'),
    dataIndex: 'evaluationType',
    key: 'evaluationType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationType') ?? ''
  },
  {
    title: t('entity.supplierevaluation.evaluatorby'),
    dataIndex: 'evaluatorBy',
    key: 'evaluatorBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluatorBy') ?? ''
  },
  {
    title: t('entity.supplierevaluation.evaluationdept'),
    dataIndex: 'evaluationDept',
    key: 'evaluationDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationDept') ?? ''
  },
  {
    title: t('entity.supplierevaluation.overallrating'),
    dataIndex: 'overallRating',
    key: 'overallRating',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'overallRating') ?? ''
  },
  {
    title: t('entity.supplierevaluation.totalscore'),
    dataIndex: 'totalScore',
    key: 'totalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'totalScore') ?? ''
  },
  {
    title: t('entity.supplierevaluation.qualityscore'),
    dataIndex: 'qualityScore',
    key: 'qualityScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'qualityScore') ?? ''
  },
  {
    title: t('entity.supplierevaluation.deliveryscore'),
    dataIndex: 'deliveryScore',
    key: 'deliveryScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'deliveryScore') ?? ''
  },
  {
    title: t('entity.supplierevaluation.pricescore'),
    dataIndex: 'priceScore',
    key: 'priceScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'priceScore') ?? ''
  },
  {
    title: t('entity.supplierevaluation.servicescore'),
    dataIndex: 'serviceScore',
    key: 'serviceScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'serviceScore') ?? ''
  },
  {
    title: t('entity.supplierevaluation.technicalscore'),
    dataIndex: 'technicalScore',
    key: 'technicalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'technicalScore') ?? ''
  },
  {
    title: t('entity.supplierevaluation.mainstrengths'),
    dataIndex: 'mainStrengths',
    key: 'mainStrengths',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'mainStrengths') ?? ''
  },
  {
    title: t('entity.supplierevaluation.mainissues'),
    dataIndex: 'mainIssues',
    key: 'mainIssues',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'mainIssues') ?? ''
  },
  {
    title: t('entity.supplierevaluation.improvementrequirements'),
    dataIndex: 'improvementRequirements',
    key: 'improvementRequirements',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'improvementRequirements') ?? ''
  },
  {
    title: t('entity.supplierevaluation.evaluationconclusion'),
    dataIndex: 'evaluationConclusion',
    key: 'evaluationConclusion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationConclusion') ?? ''
  },
  {
    title: t('entity.supplierevaluation.rectificationdeadline'),
    dataIndex: 'rectificationDeadline',
    key: 'rectificationDeadline',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'rectificationDeadline') ?? ''
  },
  {
    title: t('entity.supplierevaluation.evaluationstatus'),
    dataIndex: 'evaluationStatus',
    key: 'evaluationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationStatus') ?? ''
  },
  {
    title: t('entity.supplierevaluation.rectificationstatus'),
    dataIndex: 'rectificationStatus',
    key: 'rectificationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'rectificationStatus') ?? ''
  },
  {
    title: t('entity.supplierevaluation.relatedplant'),
    dataIndex: 'relatedPlant',
    key: 'relatedPlant',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'relatedPlant') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:supplier:evaluation:update',
        onClick: (record: SupplierEvaluation) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:supplier:evaluation:delete',
        onClick: (record: SupplierEvaluation) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSupplierEvaluationId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSupplierEvaluationField = (record: any, field: string): any => record?.[field]


/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SupplierEvaluation[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: SupplierEvaluation, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getSupplierEvaluationId(selectedRow.value) === getSupplierEvaluationId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SupplierEvaluation[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getSupplierEvaluationList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SupplierEvaluation] 加载数据失败', { error })
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
  supplierEvaluationCode: '',
  supplierId: '',
  supplierName: '',
  supplierCode: '',
  evaluationDateStart: '',
  evaluationDateEnd: '',
  evaluationPeriod: undefined as number | undefined,
  evaluationType: undefined as number | undefined,
  evaluatorBy: '',
  evaluationDept: '',
  overallRating: undefined as number | undefined,
  totalScore: undefined as number | undefined,
  qualityScore: undefined as number | undefined,
  deliveryScore: undefined as number | undefined,
  priceScore: undefined as number | undefined,
  serviceScore: undefined as number | undefined,
  technicalScore: undefined as number | undefined,
  mainStrengths: '',
  mainIssues: '',
  improvementRequirements: '',
  evaluationConclusion: undefined as number | undefined,
  rectificationDeadlineStart: '',
  rectificationDeadlineEnd: '',
  evaluationStatus: undefined as number | undefined,
  rectificationStatus: undefined as number | undefined,
  relatedPlant: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.supplierevaluation._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: SupplierEvaluation) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.supplierevaluation._self') })
  formLoading.value = true
  try {
    const detail = await loadSupplierEvaluationDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.supplierevaluation._self') }))
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
      await updateSupplierEvaluation(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.supplierevaluation._self') }))
    } else {
      await createSupplierEvaluation(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.supplierevaluation._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  supplierEvaluationItemPanelRef.value?.reload?.()
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
  const res = await getSupplierEvaluationTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSupplierEvaluation(file, sheetName)
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
    const exportMeta = await exportSupplierEvaluation(
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
    message.success(t('common.feedback.export.success', { target: t('entity.supplierevaluation._self') }))
  } catch (error: any) {
    logger.error('[SupplierEvaluation] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.supplierevaluation._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SupplierEvaluation) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.supplierevaluation._self'), name: t('common.tip.this.target', { target: t('entity.supplierevaluation._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSupplierEvaluationById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.supplierevaluation._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.supplierevaluation._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.supplierevaluation._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSupplierEvaluationBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.supplierevaluation._self') }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
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
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  supplierEvaluationCode: '',
  supplierId: '',
  supplierName: '',
  supplierCode: '',
  evaluationDateStart: '',
  evaluationDateEnd: '',
  evaluationPeriod: undefined as number | undefined,
  evaluationType: undefined as number | undefined,
  evaluatorBy: '',
  evaluationDept: '',
  overallRating: undefined as number | undefined,
  totalScore: undefined as number | undefined,
  qualityScore: undefined as number | undefined,
  deliveryScore: undefined as number | undefined,
  priceScore: undefined as number | undefined,
  serviceScore: undefined as number | undefined,
  technicalScore: undefined as number | undefined,
  mainStrengths: '',
  mainIssues: '',
  improvementRequirements: '',
  evaluationConclusion: undefined as number | undefined,
  rectificationDeadlineStart: '',
  rectificationDeadlineEnd: '',
  evaluationStatus: undefined as number | undefined,
  rectificationStatus: undefined as number | undefined,
  relatedPlant: '',
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
