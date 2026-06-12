<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/supplier-evaluation -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：供应商评价考核主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-quality-complaint-supplier-evaluation">
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
      create-permission="logistics:quality:complaint:supplierevaluation:create"
      update-permission="logistics:quality:complaint:supplierevaluation:update"
      delete-permission="logistics:quality:complaint:supplierevaluation:delete"
      import-permission="logistics:quality:complaint:supplierevaluation:import"
      export-permission="logistics:quality:complaint:supplierevaluation:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="true"
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
      :id-column-key="'supplierEvaluationId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSupplierEvaluationId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.supplierEvaluationItem._self') }}</div>
          <a-table
            v-if="hasSupplierEvaluationItemRows(record)"
            :columns="supplierEvaluationItemExpandColumns"
            :data-source="getSupplierEvaluationItemRows(record)"
            :row-key="(row: SupplierEvaluationItem, index?: number) => row?.supplierEvaluationItemId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
        </div>
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
      <SupplierEvaluationForm
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
      <a-form-item :label="t('entity.supplierEvaluation.code')">
        <a-input
          v-model:value="advancedQueryForm.supplierEvaluationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierId')">
      <a-form-item :label="t('entity.supplierEvaluation.supplierid')">
        <a-input
          v-model:value="advancedQueryForm.supplierId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.supplierid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierName')">
      <a-form-item :label="t('entity.supplierEvaluation.suppliername')">
        <a-input
          v-model:value="advancedQueryForm.supplierName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.suppliername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="t('entity.supplierEvaluation.suppliercode')">
        <a-input
          v-model:value="advancedQueryForm.supplierCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.suppliercode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationDateStart')">
      <a-form-item :label="t('entity.supplierEvaluation.evaluationdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.evaluationDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationDateEnd')">
      <a-form-item :label="t('entity.supplierEvaluation.evaluationdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.evaluationDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.supplierEvaluation.evaluationdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationPeriod')">
      <a-form-item :label="t('entity.supplierEvaluation.evaluationperiod')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationPeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationperiod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationType')">
      <a-form-item :label="t('entity.supplierEvaluation.evaluationtype')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationtype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluatorBy')">
      <a-form-item :label="t('entity.supplierEvaluation.evaluatorby')">
        <a-input
          v-model:value="advancedQueryForm.evaluatorBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluatorby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationDept')">
      <a-form-item :label="t('entity.supplierEvaluation.evaluationdept')">
        <a-input
          v-model:value="advancedQueryForm.evaluationDept"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationdept') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overallRating')">
      <a-form-item :label="t('entity.supplierEvaluation.overallrating')">
        <a-input-number
          v-model:value="advancedQueryForm.overallRating"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.overallrating') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalScore')">
      <a-form-item :label="t('entity.supplierEvaluation.totalscore')">
        <a-input-number
          v-model:value="advancedQueryForm.totalScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.totalscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityScore')">
      <a-form-item :label="t('entity.supplierEvaluation.qualityscore')">
        <a-input-number
          v-model:value="advancedQueryForm.qualityScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.qualityscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryScore')">
      <a-form-item :label="t('entity.supplierEvaluation.deliveryscore')">
        <a-input-number
          v-model:value="advancedQueryForm.deliveryScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.deliveryscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceScore')">
      <a-form-item :label="t('entity.supplierEvaluation.pricescore')">
        <a-input-number
          v-model:value="advancedQueryForm.priceScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.pricescore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceScore')">
      <a-form-item :label="t('entity.supplierEvaluation.servicescore')">
        <a-input-number
          v-model:value="advancedQueryForm.serviceScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.servicescore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('technicalScore')">
      <a-form-item :label="t('entity.supplierEvaluation.technicalscore')">
        <a-input-number
          v-model:value="advancedQueryForm.technicalScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.technicalscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mainStrengths')">
      <a-form-item :label="t('entity.supplierEvaluation.mainstrengths')">
        <a-input
          v-model:value="advancedQueryForm.mainStrengths"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.mainstrengths') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mainIssues')">
      <a-form-item :label="t('entity.supplierEvaluation.mainissues')">
        <a-input
          v-model:value="advancedQueryForm.mainIssues"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.mainissues') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementRequirements')">
      <a-form-item :label="t('entity.supplierEvaluation.improvementrequirements')">
        <a-input
          v-model:value="advancedQueryForm.improvementRequirements"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.improvementrequirements') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationConclusion')">
      <a-form-item :label="t('entity.supplierEvaluation.evaluationconclusion')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationConclusion"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationconclusion') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationDeadlineStart')">
      <a-form-item :label="t('entity.supplierEvaluation.rectificationdeadlinestart')">
        <a-input
          v-model:value="advancedQueryForm.rectificationDeadlineStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.rectificationdeadlinestart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationDeadlineEnd')">
      <a-form-item :label="t('entity.supplierEvaluation.rectificationdeadlineend')">
        <a-input
          v-model:value="advancedQueryForm.rectificationDeadlineEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.rectificationdeadlineend') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationStatus')">
      <a-form-item :label="t('entity.supplierEvaluation.evaluationstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.evaluationstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('rectificationStatus')">
      <a-form-item :label="t('entity.supplierEvaluation.rectificationstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.rectificationStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.rectificationstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('relatedPlant')">
      <a-form-item :label="t('entity.supplierEvaluation.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.relatedPlant"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.relatedplant') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.supplierEvaluation.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.supplierEvaluation.sortorder') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.supplierEvaluation._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.supplierEvaluation._self"
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
import SupplierEvaluationForm from './components/supplier-evaluation-form.vue'
import { getSupplierEvaluationList, getSupplierEvaluationById, createSupplierEvaluation, updateSupplierEvaluation, deleteSupplierEvaluationById, deleteSupplierEvaluationBatch, getSupplierEvaluationTemplate, importSupplierEvaluation, exportSupplierEvaluation } from '@/api/logistics/quality/complaint/supplier-evaluation'
import * as supplierEvaluationItemApi from '@/api/logistics/quality/complaint/supplier-evaluation-item'
import type { SupplierEvaluationItem, SupplierEvaluationItemQuery } from '@/types/logistics/quality/complaint/supplier-evaluation-item'
import type { SupplierEvaluation, SupplierEvaluationQuery, SupplierEvaluationCreate, SupplierEvaluationUpdate } from '@/types/logistics/quality/complaint/supplier-evaluation'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSupplierEvaluation')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.supplierEvaluation._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SupplierEvaluation[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
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
const formData = ref<Partial<SupplierEvaluation>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
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
  sortOrder: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'supplierEvaluationCode', label: t('entity.supplierEvaluation.code') },
  { key: 'supplierId', label: t('entity.supplierEvaluation.supplierid') },
  { key: 'supplierName', label: t('entity.supplierEvaluation.suppliername') },
  { key: 'supplierCode', label: t('entity.supplierEvaluation.suppliercode') },
  { key: 'evaluationDateStart', label: t('entity.supplierEvaluation.evaluationdatestart') },
  { key: 'evaluationDateEnd', label: t('entity.supplierEvaluation.evaluationdateend') },
  { key: 'evaluationPeriod', label: t('entity.supplierEvaluation.evaluationperiod') },
  { key: 'evaluationType', label: t('entity.supplierEvaluation.evaluationtype') },
  { key: 'evaluatorBy', label: t('entity.supplierEvaluation.evaluatorby') },
  { key: 'evaluationDept', label: t('entity.supplierEvaluation.evaluationdept') },
  { key: 'overallRating', label: t('entity.supplierEvaluation.overallrating') },
  { key: 'totalScore', label: t('entity.supplierEvaluation.totalscore') },
  { key: 'qualityScore', label: t('entity.supplierEvaluation.qualityscore') },
  { key: 'deliveryScore', label: t('entity.supplierEvaluation.deliveryscore') },
  { key: 'priceScore', label: t('entity.supplierEvaluation.pricescore') },
  { key: 'serviceScore', label: t('entity.supplierEvaluation.servicescore') },
  { key: 'technicalScore', label: t('entity.supplierEvaluation.technicalscore') },
  { key: 'mainStrengths', label: t('entity.supplierEvaluation.mainstrengths') },
  { key: 'mainIssues', label: t('entity.supplierEvaluation.mainissues') },
  { key: 'improvementRequirements', label: t('entity.supplierEvaluation.improvementrequirements') },
  { key: 'evaluationConclusion', label: t('entity.supplierEvaluation.evaluationconclusion') },
  { key: 'rectificationDeadlineStart', label: t('entity.supplierEvaluation.rectificationdeadlinestart') },
  { key: 'rectificationDeadlineEnd', label: t('entity.supplierEvaluation.rectificationdeadlineend') },
  { key: 'evaluationStatus', label: t('entity.supplierEvaluation.evaluationstatus') },
  { key: 'rectificationStatus', label: t('entity.supplierEvaluation.rectificationstatus') },
  { key: 'relatedPlant', label: t('entity.supplierEvaluation.relatedplant') },
  { key: 'sortOrder', label: t('entity.supplierEvaluation.sortorder') },
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
const entityIdName = 'supplierEvaluationId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主子表展开行 keys（手风琴，仅一行展开） */
const expandedRowKeys = ref<string[]>([])

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 展开行预览：supplierEvaluationItem 列 */
const supplierEvaluationItemExpandColumns = computed(() => [
  {
    title: t('entity.supplierEvaluationItem.evaluationid'),
    dataIndex: 'evaluationId',
    key: 'evaluationId',
    ellipsis: true,
  },
  {
    title: t('entity.supplierEvaluationItem.evaluationname'),
    dataIndex: 'evaluationName',
    key: 'evaluationName',
    ellipsis: true,
  },
  {
    title: t('entity.supplierEvaluationItem.supplierevaluationcode'),
    dataIndex: 'supplierEvaluationCode',
    key: 'supplierEvaluationCode',
    ellipsis: true,
  },
  {
    title: t('entity.supplierEvaluationItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.supplierEvaluationItem.categorytype'),
    dataIndex: 'categoryType',
    key: 'categoryType',
    ellipsis: true,
  },
  {
    title: t('entity.supplierEvaluationItem.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    ellipsis: true,
  },
  {
    title: t('entity.supplierEvaluationItem.itemdescription'),
    dataIndex: 'itemDescription',
    key: 'itemDescription',
    ellipsis: true,
  },
  {
    title: t('entity.supplierEvaluationItem.weight'),
    dataIndex: 'weight',
    key: 'weight',
    ellipsis: true,
  },
])

/** 读取主表行上的 supplierEvaluationItem 子表缓存 */
function getSupplierEvaluationItemRows(record: SupplierEvaluation): SupplierEvaluationItem[] {
  return (record as any)?.items ?? []
}

/** 主表行是否已加载 supplierEvaluationItem 子表 */
function hasSupplierEvaluationItemRows(record: SupplierEvaluation): boolean {
  return getSupplierEvaluationItemRows(record).length > 0
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
/** 懒加载 supplierEvaluationItem 子表（SupplierEvaluationItemQuery + supplierEvaluationItemApi，与主表 SupplierEvaluationQuery 分离） */
async function loadSupplierEvaluationItemForSupplierEvaluation(record: SupplierEvaluation): Promise<SupplierEvaluationItem[]> {
  const masterId = getSupplierEvaluationId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: SupplierEvaluationItemQuery = {
      pageIndex: 1,
      pageSize: 500,
      supplierEvaluationCode: masterId,
    }
    const result = await supplierEvaluationItemApi.getSupplierEvaluationItemList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getSupplierEvaluationId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, items: rows } as SupplierEvaluation
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureSupplierEvaluationChildrenLoaded(record: SupplierEvaluation) {
  if (!hasSupplierEvaluationItemRows(record)) {
    await loadSupplierEvaluationItemForSupplierEvaluation(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: SupplierEvaluation) {
  const key = getSupplierEvaluationId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureSupplierEvaluationChildrenLoaded(record)
  expandedRowKeys.value = [key]
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
    title: t('entity.supplierEvaluation.code'),
    dataIndex: 'supplierEvaluationCode',
    key: 'supplierEvaluationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierEvaluationCode') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.supplierid'),
    dataIndex: 'supplierId',
    key: 'supplierId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierId') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.suppliername'),
    dataIndex: 'supplierName',
    key: 'supplierName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierName') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.suppliercode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'supplierCode') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluationdate'),
    dataIndex: 'evaluationDate',
    key: 'evaluationDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationDate') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluationperiod'),
    dataIndex: 'evaluationPeriod',
    key: 'evaluationPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationPeriod') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluationtype'),
    dataIndex: 'evaluationType',
    key: 'evaluationType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationType') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluatorby'),
    dataIndex: 'evaluatorBy',
    key: 'evaluatorBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluatorBy') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluationdept'),
    dataIndex: 'evaluationDept',
    key: 'evaluationDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationDept') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.overallrating'),
    dataIndex: 'overallRating',
    key: 'overallRating',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'overallRating') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.totalscore'),
    dataIndex: 'totalScore',
    key: 'totalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'totalScore') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.qualityscore'),
    dataIndex: 'qualityScore',
    key: 'qualityScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'qualityScore') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.deliveryscore'),
    dataIndex: 'deliveryScore',
    key: 'deliveryScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'deliveryScore') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.pricescore'),
    dataIndex: 'priceScore',
    key: 'priceScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'priceScore') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.servicescore'),
    dataIndex: 'serviceScore',
    key: 'serviceScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'serviceScore') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.technicalscore'),
    dataIndex: 'technicalScore',
    key: 'technicalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'technicalScore') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.mainstrengths'),
    dataIndex: 'mainStrengths',
    key: 'mainStrengths',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'mainStrengths') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.mainissues'),
    dataIndex: 'mainIssues',
    key: 'mainIssues',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'mainIssues') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.improvementrequirements'),
    dataIndex: 'improvementRequirements',
    key: 'improvementRequirements',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'improvementRequirements') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluationconclusion'),
    dataIndex: 'evaluationConclusion',
    key: 'evaluationConclusion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationConclusion') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.rectificationdeadline'),
    dataIndex: 'rectificationDeadline',
    key: 'rectificationDeadline',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'rectificationDeadline') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.evaluationstatus'),
    dataIndex: 'evaluationStatus',
    key: 'evaluationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'evaluationStatus') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.rectificationstatus'),
    dataIndex: 'rectificationStatus',
    key: 'rectificationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSupplierEvaluationField(record, 'rectificationStatus') ?? ''
  },
  {
    title: t('entity.supplierEvaluation.relatedplant'),
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
        permission: 'logistics:quality:complaint:supplierevaluation:update',
        onClick: (record: SupplierEvaluation) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:supplierevaluation:delete',
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
  },
  onSelect: (record: SupplierEvaluation, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSupplierEvaluationId(selectedRow.value) === getSupplierEvaluationId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SupplierEvaluation[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SupplierEvaluation) => ({
  onClick: () => {
    const key = getSupplierEvaluationId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSupplierEvaluationId(item)))
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
    const params: SupplierEvaluationQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getSupplierEvaluationList(params)
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
  currentPage.value = 1
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.supplierEvaluation._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: SupplierEvaluation) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.supplierEvaluation._self') })
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.supplierEvaluation._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.supplierEvaluation._self') }))
    } else {
      await createSupplierEvaluation(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.supplierEvaluation._self') }))
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
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: SupplierEvaluationQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportSupplierEvaluation(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.supplierEvaluation._self') }))
  } catch (error: any) {
    logger.error('[SupplierEvaluation] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.supplierEvaluation._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SupplierEvaluation) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.supplierEvaluation._self'), name: t('common.tip.this.target', { target: t('entity.supplierEvaluation._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSupplierEvaluationById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.supplierEvaluation._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.supplierEvaluation._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.supplierEvaluation._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSupplierEvaluationBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.supplierEvaluation._self') }))
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
.logistics-quality-complaint-supplier-evaluation {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
