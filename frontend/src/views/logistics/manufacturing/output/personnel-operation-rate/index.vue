<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/personnel-operation-rate -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：人员稼动率实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-output-personnel-operation-rate">
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
      create-permission="logistics:manufacturing:output:personneloperationrate:create"
      update-permission="logistics:manufacturing:output:personneloperationrate:update"
      delete-permission="logistics:manufacturing:output:personneloperationrate:delete"
      import-permission="logistics:manufacturing:output:personneloperationrate:import"
      export-permission="logistics:manufacturing:output:personneloperationrate:export"
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
      :id-column-key="'personnelOperationRateId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPersonnelOperationRateId"
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
      <PersonnelOperationRateForm
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
      :storage-key="'takt-query-fields-logistics-manufacturing-output-personnel-operation-rate'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.personnelOperationRate.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('timeCategory')">
      <a-form-item :label="t('entity.personnelOperationRate.timecategory')">
        <a-input-number
          v-model:value="advancedQueryForm.timeCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.timecategory') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateStart')">
      <a-form-item :label="t('entity.personnelOperationRate.startdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.startdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateEnd')">
      <a-form-item :label="t('entity.personnelOperationRate.startdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.startdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateStart')">
      <a-form-item :label="t('entity.personnelOperationRate.enddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.enddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateEnd')">
      <a-form-item :label="t('entity.personnelOperationRate.enddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.enddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weekNumber')">
      <a-form-item :label="t('entity.personnelOperationRate.weeknumber')">
        <a-input-number
          v-model:value="advancedQueryForm.weekNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.weeknumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('monthNumber')">
      <a-form-item :label="t('entity.personnelOperationRate.monthnumber')">
        <a-input-number
          v-model:value="advancedQueryForm.monthNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.monthnumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLine')">
      <a-form-item :label="t('entity.personnelOperationRate.productionline')">
        <a-input
          v-model:value="advancedQueryForm.productionLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.productionline') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLineName')">
      <a-form-item :label="t('entity.personnelOperationRate.productionlinename')">
        <a-input
          v-model:value="advancedQueryForm.productionLineName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.productionlinename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="t('entity.personnelOperationRate.shiftno')">
        <a-input-number
          v-model:value="advancedQueryForm.shiftNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.shiftno') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedDirectPersonnelCount')">
      <a-form-item :label="t('entity.personnelOperationRate.planneddirectpersonnelcount')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedDirectPersonnelCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.planneddirectpersonnelcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualDirectPersonnelCount')">
      <a-form-item :label="t('entity.personnelOperationRate.actualdirectpersonnelcount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualDirectPersonnelCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.actualdirectpersonnelcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedIndirectPersonnelCount')">
      <a-form-item :label="t('entity.personnelOperationRate.plannedindirectpersonnelcount')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedIndirectPersonnelCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.plannedindirectpersonnelcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualIndirectPersonnelCount')">
      <a-form-item :label="t('entity.personnelOperationRate.actualindirectpersonnelcount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualIndirectPersonnelCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.actualindirectpersonnelcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedWorkTime')">
      <a-form-item :label="t('entity.personnelOperationRate.plannedworktime')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedWorkTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.plannedworktime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualWorkTime')">
      <a-form-item :label="t('entity.personnelOperationRate.actualworktime')">
        <a-input-number
          v-model:value="advancedQueryForm.actualWorkTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.actualworktime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('breakTime')">
      <a-form-item :label="t('entity.personnelOperationRate.breaktime')">
        <a-input-number
          v-model:value="advancedQueryForm.breakTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.breaktime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idleTime')">
      <a-form-item :label="t('entity.personnelOperationRate.idletime')">
        <a-input-number
          v-model:value="advancedQueryForm.idleTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.idletime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('personnelOperationRate')">
      <a-form-item :label="t('entity.personnelOperationRate.personneloperationrate')">
        <a-input-number
          v-model:value="advancedQueryForm.personnelOperationRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.personneloperationrate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedOutput')">
      <a-form-item :label="t('entity.personnelOperationRate.plannedoutput')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedOutput"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.plannedoutput') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualOutput')">
      <a-form-item :label="t('entity.personnelOperationRate.actualoutput')">
        <a-input-number
          v-model:value="advancedQueryForm.actualOutput"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.actualoutput') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualifiedQuantity')">
      <a-form-item :label="t('entity.personnelOperationRate.qualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.qualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.qualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectiveQuantity')">
      <a-form-item :label="t('entity.personnelOperationRate.defectivequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.defectiveQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.defectivequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('yieldRate')">
      <a-form-item :label="t('entity.personnelOperationRate.yieldrate')">
        <a-input-number
          v-model:value="advancedQueryForm.yieldRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.yieldrate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workEfficiency')">
      <a-form-item :label="t('entity.personnelOperationRate.workefficiency')">
        <a-input-number
          v-model:value="advancedQueryForm.workEfficiency"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.workefficiency') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idleReasonType')">
      <a-form-item :label="t('entity.personnelOperationRate.idlereasontype')">
        <a-input-number
          v-model:value="advancedQueryForm.idleReasonType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.idlereasontype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idleReason')">
      <a-form-item :label="t('entity.personnelOperationRate.idlereason')">
        <a-input
          v-model:value="advancedQueryForm.idleReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.idlereason') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimeHours')">
      <a-form-item :label="t('entity.personnelOperationRate.overtimehours')">
        <a-input-number
          v-model:value="advancedQueryForm.overtimeHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.overtimehours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('teamLeader')">
      <a-form-item :label="t('entity.personnelOperationRate.teamleader')">
        <a-input
          v-model:value="advancedQueryForm.teamLeader"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.teamleader') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supervisor')">
      <a-form-item :label="t('entity.personnelOperationRate.supervisor')">
        <a-input
          v-model:value="advancedQueryForm.supervisor"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.supervisor') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('status')">
      <a-form-item :label="t('entity.personnelOperationRate.status')">
        <a-input-number
          v-model:value="advancedQueryForm.status"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.status') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.personnelOperationRate._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.personnelOperationRate._self"
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
      :id-column-key="'personnelOperationRateId'"
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
 * 人员稼动率实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/personnel-operation-rate
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import PersonnelOperationRateForm from './components/personnel-operation-rate-form.vue'
import { getPersonnelOperationRateList, getPersonnelOperationRateById, createPersonnelOperationRate, updatePersonnelOperationRate, deletePersonnelOperationRateById, deletePersonnelOperationRateBatch, getPersonnelOperationRateTemplate, importPersonnelOperationRate, exportPersonnelOperationRate } from '@/api/logistics/manufacturing/output/personnel-operation-rate'
import type { PersonnelOperationRate, PersonnelOperationRateQuery, PersonnelOperationRateCreate, PersonnelOperationRateUpdate } from '@/types/logistics/manufacturing/output/personnel-operation-rate'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPersonnelOperationRate')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.personnelOperationRate._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PersonnelOperationRate[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<PersonnelOperationRate | null>(null)
/** 表格多选行 */
const selectedRows = ref<PersonnelOperationRate[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PersonnelOperationRate>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  timeCategory: undefined as number | undefined,
  startDateStart: '',
  startDateEnd: '',
  endDateStart: '',
  endDateEnd: '',
  weekNumber: undefined as number | undefined,
  monthNumber: undefined as number | undefined,
  productionLine: '',
  productionLineName: '',
  shiftNo: undefined as number | undefined,
  plannedDirectPersonnelCount: undefined as number | undefined,
  actualDirectPersonnelCount: undefined as number | undefined,
  plannedIndirectPersonnelCount: undefined as number | undefined,
  actualIndirectPersonnelCount: undefined as number | undefined,
  plannedWorkTime: undefined as number | undefined,
  actualWorkTime: undefined as number | undefined,
  breakTime: undefined as number | undefined,
  idleTime: undefined as number | undefined,
  personnelOperationRate: undefined as number | undefined,
  plannedOutput: undefined as number | undefined,
  actualOutput: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  defectiveQuantity: undefined as number | undefined,
  yieldRate: undefined as number | undefined,
  workEfficiency: undefined as number | undefined,
  idleReasonType: undefined as number | undefined,
  idleReason: '',
  overtimeHours: undefined as number | undefined,
  teamLeader: '',
  supervisor: '',
  status: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.personnelOperationRate.plantcode') },
  { key: 'timeCategory', label: t('entity.personnelOperationRate.timecategory') },
  { key: 'startDateStart', label: t('entity.personnelOperationRate.startdatestart') },
  { key: 'startDateEnd', label: t('entity.personnelOperationRate.startdateend') },
  { key: 'endDateStart', label: t('entity.personnelOperationRate.enddatestart') },
  { key: 'endDateEnd', label: t('entity.personnelOperationRate.enddateend') },
  { key: 'weekNumber', label: t('entity.personnelOperationRate.weeknumber') },
  { key: 'monthNumber', label: t('entity.personnelOperationRate.monthnumber') },
  { key: 'productionLine', label: t('entity.personnelOperationRate.productionline') },
  { key: 'productionLineName', label: t('entity.personnelOperationRate.productionlinename') },
  { key: 'shiftNo', label: t('entity.personnelOperationRate.shiftno') },
  { key: 'plannedDirectPersonnelCount', label: t('entity.personnelOperationRate.planneddirectpersonnelcount') },
  { key: 'actualDirectPersonnelCount', label: t('entity.personnelOperationRate.actualdirectpersonnelcount') },
  { key: 'plannedIndirectPersonnelCount', label: t('entity.personnelOperationRate.plannedindirectpersonnelcount') },
  { key: 'actualIndirectPersonnelCount', label: t('entity.personnelOperationRate.actualindirectpersonnelcount') },
  { key: 'plannedWorkTime', label: t('entity.personnelOperationRate.plannedworktime') },
  { key: 'actualWorkTime', label: t('entity.personnelOperationRate.actualworktime') },
  { key: 'breakTime', label: t('entity.personnelOperationRate.breaktime') },
  { key: 'idleTime', label: t('entity.personnelOperationRate.idletime') },
  { key: 'personnelOperationRate', label: t('entity.personnelOperationRate.personneloperationrate') },
  { key: 'plannedOutput', label: t('entity.personnelOperationRate.plannedoutput') },
  { key: 'actualOutput', label: t('entity.personnelOperationRate.actualoutput') },
  { key: 'qualifiedQuantity', label: t('entity.personnelOperationRate.qualifiedquantity') },
  { key: 'defectiveQuantity', label: t('entity.personnelOperationRate.defectivequantity') },
  { key: 'yieldRate', label: t('entity.personnelOperationRate.yieldrate') },
  { key: 'workEfficiency', label: t('entity.personnelOperationRate.workefficiency') },
  { key: 'idleReasonType', label: t('entity.personnelOperationRate.idlereasontype') },
  { key: 'idleReason', label: t('entity.personnelOperationRate.idlereason') },
  { key: 'overtimeHours', label: t('entity.personnelOperationRate.overtimehours') },
  { key: 'teamLeader', label: t('entity.personnelOperationRate.teamleader') },
  { key: 'supervisor', label: t('entity.personnelOperationRate.supervisor') },
  { key: 'status', label: t('entity.personnelOperationRate.status') },
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
const entityIdName = 'personnelOperationRateId'
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
    dataIndex: 'personnelOperationRateId',
    key: 'personnelOperationRateId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'personnelOperationRateId') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.timecategory'),
    dataIndex: 'timeCategory',
    key: 'timeCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'timeCategory') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.startdate'),
    dataIndex: 'startDate',
    key: 'startDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'startDate') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.enddate'),
    dataIndex: 'endDate',
    key: 'endDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'endDate') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.weeknumber'),
    dataIndex: 'weekNumber',
    key: 'weekNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'weekNumber') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.monthnumber'),
    dataIndex: 'monthNumber',
    key: 'monthNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'monthNumber') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.productionline'),
    dataIndex: 'productionLine',
    key: 'productionLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'productionLine') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.productionlinename'),
    dataIndex: 'productionLineName',
    key: 'productionLineName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'productionLineName') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'shiftNo') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.planneddirectpersonnelcount'),
    dataIndex: 'plannedDirectPersonnelCount',
    key: 'plannedDirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plannedDirectPersonnelCount') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.actualdirectpersonnelcount'),
    dataIndex: 'actualDirectPersonnelCount',
    key: 'actualDirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'actualDirectPersonnelCount') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.plannedindirectpersonnelcount'),
    dataIndex: 'plannedIndirectPersonnelCount',
    key: 'plannedIndirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plannedIndirectPersonnelCount') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.actualindirectpersonnelcount'),
    dataIndex: 'actualIndirectPersonnelCount',
    key: 'actualIndirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'actualIndirectPersonnelCount') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.plannedworktime'),
    dataIndex: 'plannedWorkTime',
    key: 'plannedWorkTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plannedWorkTime') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.actualworktime'),
    dataIndex: 'actualWorkTime',
    key: 'actualWorkTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'actualWorkTime') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.breaktime'),
    dataIndex: 'breakTime',
    key: 'breakTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'breakTime') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.idletime'),
    dataIndex: 'idleTime',
    key: 'idleTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'idleTime') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.personneloperationrate'),
    dataIndex: 'personnelOperationRate',
    key: 'personnelOperationRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'personnelOperationRate') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.plannedoutput'),
    dataIndex: 'plannedOutput',
    key: 'plannedOutput',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plannedOutput') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.actualoutput'),
    dataIndex: 'actualOutput',
    key: 'actualOutput',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'actualOutput') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.qualifiedquantity'),
    dataIndex: 'qualifiedQuantity',
    key: 'qualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'qualifiedQuantity') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.defectivequantity'),
    dataIndex: 'defectiveQuantity',
    key: 'defectiveQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'defectiveQuantity') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.yieldrate'),
    dataIndex: 'yieldRate',
    key: 'yieldRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'yieldRate') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.workefficiency'),
    dataIndex: 'workEfficiency',
    key: 'workEfficiency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'workEfficiency') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.idlereasontype'),
    dataIndex: 'idleReasonType',
    key: 'idleReasonType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'idleReasonType') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.idlereason'),
    dataIndex: 'idleReason',
    key: 'idleReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'idleReason') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.overtimehours'),
    dataIndex: 'overtimeHours',
    key: 'overtimeHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'overtimeHours') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.teamleader'),
    dataIndex: 'teamLeader',
    key: 'teamLeader',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'teamLeader') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.supervisor'),
    dataIndex: 'supervisor',
    key: 'supervisor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'supervisor') ?? ''
  },
  {
    title: t('entity.personnelOperationRate.status'),
    dataIndex: 'status',
    key: 'status',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'status') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:personneloperationrate:update',
        onClick: (record: PersonnelOperationRate) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:personneloperationrate:delete',
        onClick: (record: PersonnelOperationRate) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPersonnelOperationRateId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPersonnelOperationRateField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PersonnelOperationRate[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PersonnelOperationRate, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getPersonnelOperationRateId(selectedRow.value) === getPersonnelOperationRateId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PersonnelOperationRate[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: PersonnelOperationRate) => ({
  onClick: () => {
    const key = getPersonnelOperationRateId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPersonnelOperationRateId(item)))
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
    const params: PersonnelOperationRateQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getPersonnelOperationRateList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PersonnelOperationRate] 加载数据失败', { error })
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
  timeCategory: undefined as number | undefined,
  startDateStart: '',
  startDateEnd: '',
  endDateStart: '',
  endDateEnd: '',
  weekNumber: undefined as number | undefined,
  monthNumber: undefined as number | undefined,
  productionLine: '',
  productionLineName: '',
  shiftNo: undefined as number | undefined,
  plannedDirectPersonnelCount: undefined as number | undefined,
  actualDirectPersonnelCount: undefined as number | undefined,
  plannedIndirectPersonnelCount: undefined as number | undefined,
  actualIndirectPersonnelCount: undefined as number | undefined,
  plannedWorkTime: undefined as number | undefined,
  actualWorkTime: undefined as number | undefined,
  breakTime: undefined as number | undefined,
  idleTime: undefined as number | undefined,
  personnelOperationRate: undefined as number | undefined,
  plannedOutput: undefined as number | undefined,
  actualOutput: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  defectiveQuantity: undefined as number | undefined,
  yieldRate: undefined as number | undefined,
  workEfficiency: undefined as number | undefined,
  idleReasonType: undefined as number | undefined,
  idleReason: '',
  overtimeHours: undefined as number | undefined,
  teamLeader: '',
  supervisor: '',
  status: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.personnelOperationRate._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: PersonnelOperationRate) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.personnelOperationRate._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.personnelOperationRate._self') }))
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
      await updatePersonnelOperationRate(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.personnelOperationRate._self') }))
    } else {
      await createPersonnelOperationRate(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.personnelOperationRate._self') }))
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
  const res = await getPersonnelOperationRateTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPersonnelOperationRate(file, sheetName)
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
    const exportQuery: PersonnelOperationRateQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportPersonnelOperationRate(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.personnelOperationRate._self') }))
  } catch (error: any) {
    logger.error('[PersonnelOperationRate] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.personnelOperationRate._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PersonnelOperationRate) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.personnelOperationRate._self'), name: t('common.tip.this.target', { target: t('entity.personnelOperationRate._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePersonnelOperationRateById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.personnelOperationRate._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.personnelOperationRate._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.personnelOperationRate._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePersonnelOperationRateBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.personnelOperationRate._self') }))
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
  timeCategory: undefined as number | undefined,
  startDateStart: '',
  startDateEnd: '',
  endDateStart: '',
  endDateEnd: '',
  weekNumber: undefined as number | undefined,
  monthNumber: undefined as number | undefined,
  productionLine: '',
  productionLineName: '',
  shiftNo: undefined as number | undefined,
  plannedDirectPersonnelCount: undefined as number | undefined,
  actualDirectPersonnelCount: undefined as number | undefined,
  plannedIndirectPersonnelCount: undefined as number | undefined,
  actualIndirectPersonnelCount: undefined as number | undefined,
  plannedWorkTime: undefined as number | undefined,
  actualWorkTime: undefined as number | undefined,
  breakTime: undefined as number | undefined,
  idleTime: undefined as number | undefined,
  personnelOperationRate: undefined as number | undefined,
  plannedOutput: undefined as number | undefined,
  actualOutput: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  defectiveQuantity: undefined as number | undefined,
  yieldRate: undefined as number | undefined,
  workEfficiency: undefined as number | undefined,
  idleReasonType: undefined as number | undefined,
  idleReason: '',
  overtimeHours: undefined as number | undefined,
  teamLeader: '',
  supervisor: '',
  status: undefined as number | undefined,
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
.logistics-manufacturing-output-personnel-operation-rate {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
