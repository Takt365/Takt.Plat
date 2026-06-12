<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/equipment-operation-rate -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：机器稼动率实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-output-equipment-operation-rate">
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
      create-permission="logistics:manufacturing:output:equipmentoperationrate:create"
      update-permission="logistics:manufacturing:output:equipmentoperationrate:update"
      delete-permission="logistics:manufacturing:output:equipmentoperationrate:delete"
      import-permission="logistics:manufacturing:output:equipmentoperationrate:import"
      export-permission="logistics:manufacturing:output:equipmentoperationrate:export"
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
      :id-column-key="'equipmentOperationRateId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEquipmentOperationRateId"
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
      <EquipmentOperationRateForm
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
      :storage-key="'takt-query-fields-logistics-manufacturing-output-equipment-operation-rate'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.equipmentOperationRate.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('timeCategory')">
      <a-form-item :label="t('entity.equipmentOperationRate.timecategory')">
        <a-input-number
          v-model:value="advancedQueryForm.timeCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.timecategory') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateStart')">
      <a-form-item :label="t('entity.equipmentOperationRate.startdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.startdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateEnd')">
      <a-form-item :label="t('entity.equipmentOperationRate.startdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.startdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateStart')">
      <a-form-item :label="t('entity.equipmentOperationRate.enddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.enddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateEnd')">
      <a-form-item :label="t('entity.equipmentOperationRate.enddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipmentOperationRate.enddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weekNumber')">
      <a-form-item :label="t('entity.equipmentOperationRate.weeknumber')">
        <a-input-number
          v-model:value="advancedQueryForm.weekNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.weeknumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('monthNumber')">
      <a-form-item :label="t('entity.equipmentOperationRate.monthnumber')">
        <a-input-number
          v-model:value="advancedQueryForm.monthNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.monthnumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentCode')">
      <a-form-item :label="t('entity.equipmentOperationRate.equipmentcode')">
        <a-input
          v-model:value="advancedQueryForm.equipmentCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentName')">
      <a-form-item :label="t('entity.equipmentOperationRate.equipmentname')">
        <a-input
          v-model:value="advancedQueryForm.equipmentName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentname') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentType')">
      <a-form-item :label="t('entity.equipmentOperationRate.equipmenttype')">
        <a-input-number
          v-model:value="advancedQueryForm.equipmentType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmenttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLine')">
      <a-form-item :label="t('entity.equipmentOperationRate.productionline')">
        <a-input
          v-model:value="advancedQueryForm.productionLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.productionline') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="t('entity.equipmentOperationRate.shiftno')">
        <a-input-number
          v-model:value="advancedQueryForm.shiftNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.shiftno') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedRuntime')">
      <a-form-item :label="t('entity.equipmentOperationRate.plannedruntime')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedRuntime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.plannedruntime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualRuntime')">
      <a-form-item :label="t('entity.equipmentOperationRate.actualruntime')">
        <a-input-number
          v-model:value="advancedQueryForm.actualRuntime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.actualruntime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtime')">
      <a-form-item :label="t('entity.equipmentOperationRate.downtime')">
        <a-input-number
          v-model:value="advancedQueryForm.downtime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.downtime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentOperationRate')">
      <a-form-item :label="t('entity.equipmentOperationRate.equipmentoperationrate')">
        <a-input-number
          v-model:value="advancedQueryForm.equipmentOperationRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentoperationrate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedOutput')">
      <a-form-item :label="t('entity.equipmentOperationRate.plannedoutput')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedOutput"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.plannedoutput') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualOutput')">
      <a-form-item :label="t('entity.equipmentOperationRate.actualoutput')">
        <a-input-number
          v-model:value="advancedQueryForm.actualOutput"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.actualoutput') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualifiedQuantity')">
      <a-form-item :label="t('entity.equipmentOperationRate.qualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.qualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.qualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectiveQuantity')">
      <a-form-item :label="t('entity.equipmentOperationRate.defectivequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.defectiveQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.defectivequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('yieldRate')">
      <a-form-item :label="t('entity.equipmentOperationRate.yieldrate')">
        <a-input-number
          v-model:value="advancedQueryForm.yieldRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.yieldrate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeReasonType')">
      <a-form-item :label="t('entity.equipmentOperationRate.downtimereasontype')">
        <a-input-number
          v-model:value="advancedQueryForm.downtimeReasonType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.downtimereasontype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeReason')">
      <a-form-item :label="t('entity.equipmentOperationRate.downtimereason')">
        <a-input
          v-model:value="advancedQueryForm.downtimeReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.downtimereason') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentStatus')">
      <a-form-item :label="t('entity.equipmentOperationRate.equipmentstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.equipmentStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentOperator')">
      <a-form-item :label="t('entity.equipmentOperationRate.equipmentoperator')">
        <a-input
          v-model:value="advancedQueryForm.equipmentOperator"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentoperator') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentMaintainer')">
      <a-form-item :label="t('entity.equipmentOperationRate.equipmentmaintainer')">
        <a-input
          v-model:value="advancedQueryForm.equipmentMaintainer"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.equipmentmaintainer') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('teamLeader')">
      <a-form-item :label="t('entity.equipmentOperationRate.teamleader')">
        <a-input
          v-model:value="advancedQueryForm.teamLeader"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.teamleader') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('status')">
      <a-form-item :label="t('entity.equipmentOperationRate.status')">
        <a-input-number
          v-model:value="advancedQueryForm.status"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipmentOperationRate.status') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.equipmentOperationRate._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.equipmentOperationRate._self"
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
      :id-column-key="'equipmentOperationRateId'"
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
 * 机器稼动率实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/equipment-operation-rate
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import EquipmentOperationRateForm from './components/equipment-operation-rate-form.vue'
import { getEquipmentOperationRateList, getEquipmentOperationRateById, createEquipmentOperationRate, updateEquipmentOperationRate, deleteEquipmentOperationRateById, deleteEquipmentOperationRateBatch, getEquipmentOperationRateTemplate, importEquipmentOperationRate, exportEquipmentOperationRate } from '@/api/logistics/manufacturing/output/equipment-operation-rate'
import type { EquipmentOperationRate, EquipmentOperationRateQuery, EquipmentOperationRateCreate, EquipmentOperationRateUpdate } from '@/types/logistics/manufacturing/output/equipment-operation-rate'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEquipmentOperationRate')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.equipmentOperationRate._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EquipmentOperationRate[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<EquipmentOperationRate | null>(null)
/** 表格多选行 */
const selectedRows = ref<EquipmentOperationRate[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<EquipmentOperationRate>>({})
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
  equipmentCode: '',
  equipmentName: '',
  equipmentType: undefined as number | undefined,
  productionLine: '',
  shiftNo: undefined as number | undefined,
  plannedRuntime: undefined as number | undefined,
  actualRuntime: undefined as number | undefined,
  downtime: undefined as number | undefined,
  equipmentOperationRate: undefined as number | undefined,
  plannedOutput: undefined as number | undefined,
  actualOutput: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  defectiveQuantity: undefined as number | undefined,
  yieldRate: undefined as number | undefined,
  downtimeReasonType: undefined as number | undefined,
  downtimeReason: '',
  equipmentStatus: undefined as number | undefined,
  equipmentOperator: '',
  equipmentMaintainer: '',
  teamLeader: '',
  status: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.equipmentOperationRate.plantcode') },
  { key: 'timeCategory', label: t('entity.equipmentOperationRate.timecategory') },
  { key: 'startDateStart', label: t('entity.equipmentOperationRate.startdatestart') },
  { key: 'startDateEnd', label: t('entity.equipmentOperationRate.startdateend') },
  { key: 'endDateStart', label: t('entity.equipmentOperationRate.enddatestart') },
  { key: 'endDateEnd', label: t('entity.equipmentOperationRate.enddateend') },
  { key: 'weekNumber', label: t('entity.equipmentOperationRate.weeknumber') },
  { key: 'monthNumber', label: t('entity.equipmentOperationRate.monthnumber') },
  { key: 'equipmentCode', label: t('entity.equipmentOperationRate.equipmentcode') },
  { key: 'equipmentName', label: t('entity.equipmentOperationRate.equipmentname') },
  { key: 'equipmentType', label: t('entity.equipmentOperationRate.equipmenttype') },
  { key: 'productionLine', label: t('entity.equipmentOperationRate.productionline') },
  { key: 'shiftNo', label: t('entity.equipmentOperationRate.shiftno') },
  { key: 'plannedRuntime', label: t('entity.equipmentOperationRate.plannedruntime') },
  { key: 'actualRuntime', label: t('entity.equipmentOperationRate.actualruntime') },
  { key: 'downtime', label: t('entity.equipmentOperationRate.downtime') },
  { key: 'equipmentOperationRate', label: t('entity.equipmentOperationRate.equipmentoperationrate') },
  { key: 'plannedOutput', label: t('entity.equipmentOperationRate.plannedoutput') },
  { key: 'actualOutput', label: t('entity.equipmentOperationRate.actualoutput') },
  { key: 'qualifiedQuantity', label: t('entity.equipmentOperationRate.qualifiedquantity') },
  { key: 'defectiveQuantity', label: t('entity.equipmentOperationRate.defectivequantity') },
  { key: 'yieldRate', label: t('entity.equipmentOperationRate.yieldrate') },
  { key: 'downtimeReasonType', label: t('entity.equipmentOperationRate.downtimereasontype') },
  { key: 'downtimeReason', label: t('entity.equipmentOperationRate.downtimereason') },
  { key: 'equipmentStatus', label: t('entity.equipmentOperationRate.equipmentstatus') },
  { key: 'equipmentOperator', label: t('entity.equipmentOperationRate.equipmentoperator') },
  { key: 'equipmentMaintainer', label: t('entity.equipmentOperationRate.equipmentmaintainer') },
  { key: 'teamLeader', label: t('entity.equipmentOperationRate.teamleader') },
  { key: 'status', label: t('entity.equipmentOperationRate.status') },
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
const entityIdName = 'equipmentOperationRateId'
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
    dataIndex: 'equipmentOperationRateId',
    key: 'equipmentOperationRateId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'equipmentOperationRateId') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.timecategory'),
    dataIndex: 'timeCategory',
    key: 'timeCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'timeCategory') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.startdate'),
    dataIndex: 'startDate',
    key: 'startDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'startDate') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.enddate'),
    dataIndex: 'endDate',
    key: 'endDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'endDate') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.weeknumber'),
    dataIndex: 'weekNumber',
    key: 'weekNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'weekNumber') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.monthnumber'),
    dataIndex: 'monthNumber',
    key: 'monthNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'monthNumber') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.equipmentcode'),
    dataIndex: 'equipmentCode',
    key: 'equipmentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'equipmentCode') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.equipmentname'),
    dataIndex: 'equipmentName',
    key: 'equipmentName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'equipmentName') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.equipmenttype'),
    dataIndex: 'equipmentType',
    key: 'equipmentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'equipmentType') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.productionline'),
    dataIndex: 'productionLine',
    key: 'productionLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'productionLine') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'shiftNo') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.plannedruntime'),
    dataIndex: 'plannedRuntime',
    key: 'plannedRuntime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'plannedRuntime') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.actualruntime'),
    dataIndex: 'actualRuntime',
    key: 'actualRuntime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'actualRuntime') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.downtime'),
    dataIndex: 'downtime',
    key: 'downtime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'downtime') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.equipmentoperationrate'),
    dataIndex: 'equipmentOperationRate',
    key: 'equipmentOperationRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'equipmentOperationRate') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.plannedoutput'),
    dataIndex: 'plannedOutput',
    key: 'plannedOutput',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'plannedOutput') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.actualoutput'),
    dataIndex: 'actualOutput',
    key: 'actualOutput',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'actualOutput') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.qualifiedquantity'),
    dataIndex: 'qualifiedQuantity',
    key: 'qualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'qualifiedQuantity') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.defectivequantity'),
    dataIndex: 'defectiveQuantity',
    key: 'defectiveQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'defectiveQuantity') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.yieldrate'),
    dataIndex: 'yieldRate',
    key: 'yieldRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'yieldRate') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.downtimereasontype'),
    dataIndex: 'downtimeReasonType',
    key: 'downtimeReasonType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'downtimeReasonType') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.downtimereason'),
    dataIndex: 'downtimeReason',
    key: 'downtimeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'downtimeReason') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.equipmentstatus'),
    dataIndex: 'equipmentStatus',
    key: 'equipmentStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'equipmentStatus') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.equipmentoperator'),
    dataIndex: 'equipmentOperator',
    key: 'equipmentOperator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'equipmentOperator') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.equipmentmaintainer'),
    dataIndex: 'equipmentMaintainer',
    key: 'equipmentMaintainer',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'equipmentMaintainer') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.teamleader'),
    dataIndex: 'teamLeader',
    key: 'teamLeader',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'teamLeader') ?? ''
  },
  {
    title: t('entity.equipmentOperationRate.status'),
    dataIndex: 'status',
    key: 'status',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentOperationRateField(record, 'status') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:equipmentoperationrate:update',
        onClick: (record: EquipmentOperationRate) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:equipmentoperationrate:delete',
        onClick: (record: EquipmentOperationRate) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEquipmentOperationRateId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEquipmentOperationRateField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EquipmentOperationRate[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EquipmentOperationRate, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEquipmentOperationRateId(selectedRow.value) === getEquipmentOperationRateId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EquipmentOperationRate[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: EquipmentOperationRate) => ({
  onClick: () => {
    const key = getEquipmentOperationRateId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEquipmentOperationRateId(item)))
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
    const params: EquipmentOperationRateQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getEquipmentOperationRateList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[EquipmentOperationRate] 加载数据失败', { error })
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
  equipmentCode: '',
  equipmentName: '',
  equipmentType: undefined as number | undefined,
  productionLine: '',
  shiftNo: undefined as number | undefined,
  plannedRuntime: undefined as number | undefined,
  actualRuntime: undefined as number | undefined,
  downtime: undefined as number | undefined,
  equipmentOperationRate: undefined as number | undefined,
  plannedOutput: undefined as number | undefined,
  actualOutput: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  defectiveQuantity: undefined as number | undefined,
  yieldRate: undefined as number | undefined,
  downtimeReasonType: undefined as number | undefined,
  downtimeReason: '',
  equipmentStatus: undefined as number | undefined,
  equipmentOperator: '',
  equipmentMaintainer: '',
  teamLeader: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.equipmentOperationRate._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: EquipmentOperationRate) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.equipmentOperationRate._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.equipmentOperationRate._self') }))
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
      await updateEquipmentOperationRate(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.equipmentOperationRate._self') }))
    } else {
      await createEquipmentOperationRate(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.equipmentOperationRate._self') }))
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
  const res = await getEquipmentOperationRateTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEquipmentOperationRate(file, sheetName)
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
    const exportQuery: EquipmentOperationRateQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportEquipmentOperationRate(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.equipmentOperationRate._self') }))
  } catch (error: any) {
    logger.error('[EquipmentOperationRate] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.equipmentOperationRate._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: EquipmentOperationRate) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.equipmentOperationRate._self'), name: t('common.tip.this.target', { target: t('entity.equipmentOperationRate._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEquipmentOperationRateById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.equipmentOperationRate._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.equipmentOperationRate._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.equipmentOperationRate._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEquipmentOperationRateBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.equipmentOperationRate._self') }))
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
  equipmentCode: '',
  equipmentName: '',
  equipmentType: undefined as number | undefined,
  productionLine: '',
  shiftNo: undefined as number | undefined,
  plannedRuntime: undefined as number | undefined,
  actualRuntime: undefined as number | undefined,
  downtime: undefined as number | undefined,
  equipmentOperationRate: undefined as number | undefined,
  plannedOutput: undefined as number | undefined,
  actualOutput: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  defectiveQuantity: undefined as number | undefined,
  yieldRate: undefined as number | undefined,
  downtimeReasonType: undefined as number | undefined,
  downtimeReason: '',
  equipmentStatus: undefined as number | undefined,
  equipmentOperator: '',
  equipmentMaintainer: '',
  teamLeader: '',
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
.logistics-manufacturing-output-equipment-operation-rate {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
