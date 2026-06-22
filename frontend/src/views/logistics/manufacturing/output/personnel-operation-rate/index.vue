<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/personnel-operation-rate -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：人员稼动率实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
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
      create-permission="logistics:manufacturing:output:personnel:operation:rate:create"
      update-permission="logistics:manufacturing:output:personnel:operation:rate:update"
      delete-permission="logistics:manufacturing:output:personnel:operation:rate:delete"
      import-permission="logistics:manufacturing:output:personnel:operation:rate:import"
      export-permission="logistics:manufacturing:output:personnel:operation:rate:export"
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
      entity-scope="company"
      :columns="columns"
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

    <!-- 分页（服务端分页，外置 TaktPagination） -->
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
        :key="formData?.personnelOperationRateId ?? 'create'"
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
      <a-form-item :label="t('entity.personneloperationrate.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('timeCategory')">
      <a-form-item :label="t('entity.personneloperationrate.timecategory')">
        <a-input-number
          v-model:value="advancedQueryForm.timeCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.timecategory') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateStart')">
      <a-form-item :label="t('entity.personneloperationrate.startdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.startdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateEnd')">
      <a-form-item :label="t('entity.personneloperationrate.startdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.startdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateStart')">
      <a-form-item :label="t('entity.personneloperationrate.enddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.enddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateEnd')">
      <a-form-item :label="t('entity.personneloperationrate.enddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.enddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weekNumber')">
      <a-form-item :label="t('entity.personneloperationrate.weeknumber')">
        <a-input-number
          v-model:value="advancedQueryForm.weekNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.weeknumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('monthNumber')">
      <a-form-item :label="t('entity.personneloperationrate.monthnumber')">
        <a-input-number
          v-model:value="advancedQueryForm.monthNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.monthnumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLine')">
      <a-form-item :label="t('entity.personneloperationrate.productionline')">
        <a-input
          v-model:value="advancedQueryForm.productionLine"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.productionline') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLineName')">
      <a-form-item :label="t('entity.personneloperationrate.productionlinename')">
        <a-input
          v-model:value="advancedQueryForm.productionLineName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.productionlinename') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="t('entity.personneloperationrate.shiftno')">
        <a-input-number
          v-model:value="advancedQueryForm.shiftNo"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.shiftno') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedDirectPersonnelCount')">
      <a-form-item :label="t('entity.personneloperationrate.planneddirectpersonnelcount')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedDirectPersonnelCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.planneddirectpersonnelcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualDirectPersonnelCount')">
      <a-form-item :label="t('entity.personneloperationrate.actualdirectpersonnelcount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualDirectPersonnelCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.actualdirectpersonnelcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedIndirectPersonnelCount')">
      <a-form-item :label="t('entity.personneloperationrate.plannedindirectpersonnelcount')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedIndirectPersonnelCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.plannedindirectpersonnelcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualIndirectPersonnelCount')">
      <a-form-item :label="t('entity.personneloperationrate.actualindirectpersonnelcount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualIndirectPersonnelCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.actualindirectpersonnelcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedWorkTime')">
      <a-form-item :label="t('entity.personneloperationrate.plannedworktime')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedWorkTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.plannedworktime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualWorkTime')">
      <a-form-item :label="t('entity.personneloperationrate.actualworktime')">
        <a-input-number
          v-model:value="advancedQueryForm.actualWorkTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.actualworktime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('breakTime')">
      <a-form-item :label="t('entity.personneloperationrate.breaktime')">
        <a-input-number
          v-model:value="advancedQueryForm.breakTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.breaktime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idleTime')">
      <a-form-item :label="t('entity.personneloperationrate.idletime')">
        <a-input-number
          v-model:value="advancedQueryForm.idleTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.idletime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('personnelOperationRate')">
      <a-form-item :label="t('entity.personneloperationrate.personneloperationrate')">
        <a-input-number
          v-model:value="advancedQueryForm.personnelOperationRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.personneloperationrate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedOutput')">
      <a-form-item :label="t('entity.personneloperationrate.plannedoutput')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedOutput"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.plannedoutput') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualOutput')">
      <a-form-item :label="t('entity.personneloperationrate.actualoutput')">
        <a-input-number
          v-model:value="advancedQueryForm.actualOutput"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.actualoutput') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualifiedQuantity')">
      <a-form-item :label="t('entity.personneloperationrate.qualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.qualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.qualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectiveQuantity')">
      <a-form-item :label="t('entity.personneloperationrate.defectivequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.defectiveQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.defectivequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('yieldRate')">
      <a-form-item :label="t('entity.personneloperationrate.yieldrate')">
        <a-input-number
          v-model:value="advancedQueryForm.yieldRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.yieldrate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workEfficiency')">
      <a-form-item :label="t('entity.personneloperationrate.workefficiency')">
        <a-input-number
          v-model:value="advancedQueryForm.workEfficiency"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.workefficiency') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idleReasonType')">
      <a-form-item :label="t('entity.personneloperationrate.idlereasontype')">
        <a-input-number
          v-model:value="advancedQueryForm.idleReasonType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.idlereasontype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('idleReason')">
      <a-form-item :label="t('entity.personneloperationrate.idlereason')">
        <a-input
          v-model:value="advancedQueryForm.idleReason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.idlereason') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overtimeHours')">
      <a-form-item :label="t('entity.personneloperationrate.overtimehours')">
        <a-input-number
          v-model:value="advancedQueryForm.overtimeHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.overtimehours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('teamLeader')">
      <a-form-item :label="t('entity.personneloperationrate.teamleader')">
        <a-input
          v-model:value="advancedQueryForm.teamLeader"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.teamleader') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supervisor')">
      <a-form-item :label="t('entity.personneloperationrate.supervisor')">
        <a-input
          v-model:value="advancedQueryForm.supervisor"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.supervisor') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('status')">
      <a-form-item :label="t('entity.personneloperationrate.status')">
        <a-input-number
          v-model:value="advancedQueryForm.status"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.status') })"
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

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.personneloperationrate._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.personneloperationrate._self"
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
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import PersonnelOperationRateForm from './components/personnel-operation-rate-form.vue'
import { getPersonnelOperationRateList, getPersonnelOperationRateById, createPersonnelOperationRate, updatePersonnelOperationRate, deletePersonnelOperationRateById, deletePersonnelOperationRateBatch, getPersonnelOperationRateTemplate, importPersonnelOperationRate, exportPersonnelOperationRate, updatePersonnelOperationRateStatus } from '@/api/logistics/manufacturing/output/personnel-operation-rate'
import type { PersonnelOperationRate, PersonnelOperationRateQuery } from '@/types/logistics/manufacturing/output/personnel-operation-rate'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPersonnelOperationRate')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.personneloperationrate._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PersonnelOperationRate[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
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
const formData = ref<Partial<PersonnelOperationRate> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
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
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.personneloperationrate.plantcode') },
  { key: 'timeCategory', label: t('entity.personneloperationrate.timecategory') },
  { key: 'startDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.personneloperationrate.startdate')) },
  { key: 'startDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.personneloperationrate.startdate')) },
  { key: 'endDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.personneloperationrate.enddate')) },
  { key: 'endDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.personneloperationrate.enddate')) },
  { key: 'weekNumber', label: t('entity.personneloperationrate.weeknumber') },
  { key: 'monthNumber', label: t('entity.personneloperationrate.monthnumber') },
  { key: 'productionLine', label: t('entity.personneloperationrate.productionline') },
  { key: 'productionLineName', label: t('entity.personneloperationrate.productionlinename') },
  { key: 'shiftNo', label: t('entity.personneloperationrate.shiftno') },
  { key: 'plannedDirectPersonnelCount', label: t('entity.personneloperationrate.planneddirectpersonnelcount') },
  { key: 'actualDirectPersonnelCount', label: t('entity.personneloperationrate.actualdirectpersonnelcount') },
  { key: 'plannedIndirectPersonnelCount', label: t('entity.personneloperationrate.plannedindirectpersonnelcount') },
  { key: 'actualIndirectPersonnelCount', label: t('entity.personneloperationrate.actualindirectpersonnelcount') },
  { key: 'plannedWorkTime', label: t('entity.personneloperationrate.plannedworktime') },
  { key: 'actualWorkTime', label: t('entity.personneloperationrate.actualworktime') },
  { key: 'breakTime', label: t('entity.personneloperationrate.breaktime') },
  { key: 'idleTime', label: t('entity.personneloperationrate.idletime') },
  { key: 'personnelOperationRate', label: t('entity.personneloperationrate.personneloperationrate') },
  { key: 'plannedOutput', label: t('entity.personneloperationrate.plannedoutput') },
  { key: 'actualOutput', label: t('entity.personneloperationrate.actualoutput') },
  { key: 'qualifiedQuantity', label: t('entity.personneloperationrate.qualifiedquantity') },
  { key: 'defectiveQuantity', label: t('entity.personneloperationrate.defectivequantity') },
  { key: 'yieldRate', label: t('entity.personneloperationrate.yieldrate') },
  { key: 'workEfficiency', label: t('entity.personneloperationrate.workefficiency') },
  { key: 'idleReasonType', label: t('entity.personneloperationrate.idlereasontype') },
  { key: 'idleReason', label: t('entity.personneloperationrate.idlereason') },
  { key: 'overtimeHours', label: t('entity.personneloperationrate.overtimehours') },
  { key: 'teamLeader', label: t('entity.personneloperationrate.teamleader') },
  { key: 'supervisor', label: t('entity.personneloperationrate.supervisor') },
  { key: 'status', label: t('entity.personneloperationrate.status') },
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
const entityIdName = 'personnelOperationRateId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)



/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PersonnelOperationRateQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PersonnelOperationRateQuery>): PersonnelOperationRateQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PersonnelOperationRateQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PersonnelOperationRateQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  if (form.timeCategory !== undefined && form.timeCategory !== null) {
    query.timeCategory = form.timeCategory
  }
  assignTrimmed('startDateStart', form.startDateStart)
  assignTrimmed('startDateEnd', form.startDateEnd)
  assignTrimmed('endDateStart', form.endDateStart)
  assignTrimmed('endDateEnd', form.endDateEnd)
  if (form.weekNumber !== undefined && form.weekNumber !== null) {
    query.weekNumber = form.weekNumber
  }
  if (form.monthNumber !== undefined && form.monthNumber !== null) {
    query.monthNumber = form.monthNumber
  }
  assignTrimmed('productionLine', form.productionLine)
  assignTrimmed('productionLineName', form.productionLineName)
  if (form.shiftNo !== undefined && form.shiftNo !== null) {
    query.shiftNo = form.shiftNo
  }
  if (form.plannedDirectPersonnelCount !== undefined && form.plannedDirectPersonnelCount !== null) {
    query.plannedDirectPersonnelCount = form.plannedDirectPersonnelCount
  }
  if (form.actualDirectPersonnelCount !== undefined && form.actualDirectPersonnelCount !== null) {
    query.actualDirectPersonnelCount = form.actualDirectPersonnelCount
  }
  if (form.plannedIndirectPersonnelCount !== undefined && form.plannedIndirectPersonnelCount !== null) {
    query.plannedIndirectPersonnelCount = form.plannedIndirectPersonnelCount
  }
  if (form.actualIndirectPersonnelCount !== undefined && form.actualIndirectPersonnelCount !== null) {
    query.actualIndirectPersonnelCount = form.actualIndirectPersonnelCount
  }
  if (form.plannedWorkTime !== undefined && form.plannedWorkTime !== null) {
    query.plannedWorkTime = form.plannedWorkTime
  }
  if (form.actualWorkTime !== undefined && form.actualWorkTime !== null) {
    query.actualWorkTime = form.actualWorkTime
  }
  if (form.breakTime !== undefined && form.breakTime !== null) {
    query.breakTime = form.breakTime
  }
  if (form.idleTime !== undefined && form.idleTime !== null) {
    query.idleTime = form.idleTime
  }
  if (form.personnelOperationRate !== undefined && form.personnelOperationRate !== null) {
    query.personnelOperationRate = form.personnelOperationRate
  }
  if (form.plannedOutput !== undefined && form.plannedOutput !== null) {
    query.plannedOutput = form.plannedOutput
  }
  if (form.actualOutput !== undefined && form.actualOutput !== null) {
    query.actualOutput = form.actualOutput
  }
  if (form.qualifiedQuantity !== undefined && form.qualifiedQuantity !== null) {
    query.qualifiedQuantity = form.qualifiedQuantity
  }
  if (form.defectiveQuantity !== undefined && form.defectiveQuantity !== null) {
    query.defectiveQuantity = form.defectiveQuantity
  }
  if (form.yieldRate !== undefined && form.yieldRate !== null) {
    query.yieldRate = form.yieldRate
  }
  if (form.workEfficiency !== undefined && form.workEfficiency !== null) {
    query.workEfficiency = form.workEfficiency
  }
  if (form.idleReasonType !== undefined && form.idleReasonType !== null) {
    query.idleReasonType = form.idleReasonType
  }
  assignTrimmed('idleReason', form.idleReason)
  if (form.overtimeHours !== undefined && form.overtimeHours !== null) {
    query.overtimeHours = form.overtimeHours
  }
  assignTrimmed('teamLeader', form.teamLeader)
  assignTrimmed('supervisor', form.supervisor)
  if (form.status !== undefined && form.status !== null) {
    query.status = form.status
  }
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
    title: t('entity.personneloperationrate.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.personneloperationrate.timecategory'),
    dataIndex: 'timeCategory',
    key: 'timeCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'timeCategory') ?? ''
  },
  {
    title: t('entity.personneloperationrate.startdate'),
    dataIndex: 'startDate',
    key: 'startDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'startDate') ?? ''
  },
  {
    title: t('entity.personneloperationrate.enddate'),
    dataIndex: 'endDate',
    key: 'endDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'endDate') ?? ''
  },
  {
    title: t('entity.personneloperationrate.weeknumber'),
    dataIndex: 'weekNumber',
    key: 'weekNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'weekNumber') ?? ''
  },
  {
    title: t('entity.personneloperationrate.monthnumber'),
    dataIndex: 'monthNumber',
    key: 'monthNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'monthNumber') ?? ''
  },
  {
    title: t('entity.personneloperationrate.productionline'),
    dataIndex: 'productionLine',
    key: 'productionLine',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'productionLine') ?? ''
  },
  {
    title: t('entity.personneloperationrate.productionlinename'),
    dataIndex: 'productionLineName',
    key: 'productionLineName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'productionLineName') ?? ''
  },
  {
    title: t('entity.personneloperationrate.shiftno'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'shiftNo') ?? ''
  },
  {
    title: t('entity.personneloperationrate.planneddirectpersonnelcount'),
    dataIndex: 'plannedDirectPersonnelCount',
    key: 'plannedDirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plannedDirectPersonnelCount') ?? ''
  },
  {
    title: t('entity.personneloperationrate.actualdirectpersonnelcount'),
    dataIndex: 'actualDirectPersonnelCount',
    key: 'actualDirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'actualDirectPersonnelCount') ?? ''
  },
  {
    title: t('entity.personneloperationrate.plannedindirectpersonnelcount'),
    dataIndex: 'plannedIndirectPersonnelCount',
    key: 'plannedIndirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plannedIndirectPersonnelCount') ?? ''
  },
  {
    title: t('entity.personneloperationrate.actualindirectpersonnelcount'),
    dataIndex: 'actualIndirectPersonnelCount',
    key: 'actualIndirectPersonnelCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'actualIndirectPersonnelCount') ?? ''
  },
  {
    title: t('entity.personneloperationrate.plannedworktime'),
    dataIndex: 'plannedWorkTime',
    key: 'plannedWorkTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plannedWorkTime') ?? ''
  },
  {
    title: t('entity.personneloperationrate.actualworktime'),
    dataIndex: 'actualWorkTime',
    key: 'actualWorkTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'actualWorkTime') ?? ''
  },
  {
    title: t('entity.personneloperationrate.breaktime'),
    dataIndex: 'breakTime',
    key: 'breakTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'breakTime') ?? ''
  },
  {
    title: t('entity.personneloperationrate.idletime'),
    dataIndex: 'idleTime',
    key: 'idleTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'idleTime') ?? ''
  },
  {
    title: t('entity.personneloperationrate.personneloperationrate'),
    dataIndex: 'personnelOperationRate',
    key: 'personnelOperationRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'personnelOperationRate') ?? ''
  },
  {
    title: t('entity.personneloperationrate.plannedoutput'),
    dataIndex: 'plannedOutput',
    key: 'plannedOutput',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'plannedOutput') ?? ''
  },
  {
    title: t('entity.personneloperationrate.actualoutput'),
    dataIndex: 'actualOutput',
    key: 'actualOutput',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'actualOutput') ?? ''
  },
  {
    title: t('entity.personneloperationrate.qualifiedquantity'),
    dataIndex: 'qualifiedQuantity',
    key: 'qualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'qualifiedQuantity') ?? ''
  },
  {
    title: t('entity.personneloperationrate.defectivequantity'),
    dataIndex: 'defectiveQuantity',
    key: 'defectiveQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'defectiveQuantity') ?? ''
  },
  {
    title: t('entity.personneloperationrate.yieldrate'),
    dataIndex: 'yieldRate',
    key: 'yieldRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'yieldRate') ?? ''
  },
  {
    title: t('entity.personneloperationrate.workefficiency'),
    dataIndex: 'workEfficiency',
    key: 'workEfficiency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'workEfficiency') ?? ''
  },
  {
    title: t('entity.personneloperationrate.idlereasontype'),
    dataIndex: 'idleReasonType',
    key: 'idleReasonType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'idleReasonType') ?? ''
  },
  {
    title: t('entity.personneloperationrate.idlereason'),
    dataIndex: 'idleReason',
    key: 'idleReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'idleReason') ?? ''
  },
  {
    title: t('entity.personneloperationrate.overtimehours'),
    dataIndex: 'overtimeHours',
    key: 'overtimeHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'overtimeHours') ?? ''
  },
  {
    title: t('entity.personneloperationrate.teamleader'),
    dataIndex: 'teamLeader',
    key: 'teamLeader',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'teamLeader') ?? ''
  },
  {
    title: t('entity.personneloperationrate.supervisor'),
    dataIndex: 'supervisor',
    key: 'supervisor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPersonnelOperationRateField(record, 'supervisor') ?? ''
  },
  {
    title: t('entity.personneloperationrate.status'),
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
        permission: 'logistics:manufacturing:output:personnel:operation:rate:update',
        onClick: (record: PersonnelOperationRate) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:personnel:operation:rate:delete',
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
    const res = await getPersonnelOperationRateList(buildListQuery())
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
  currentPage.value = getTaktDefaultPageIndex()
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
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.personneloperationrate._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: PersonnelOperationRate) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.personneloperationrate._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.personneloperationrate._self') }))
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
      message.success(t('common.feedback.updated', { target: t('entity.personneloperationrate._self') }))
    } else {
      await createPersonnelOperationRate(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.personneloperationrate._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
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
    const exportMeta = await exportPersonnelOperationRate(
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
    message.success(t('common.feedback.export.success', { target: t('entity.personneloperationrate._self') }))
  } catch (error: any) {
    logger.error('[PersonnelOperationRate] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.personneloperationrate._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PersonnelOperationRate) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.personneloperationrate._self'), name: t('common.tip.this.target', { target: t('entity.personneloperationrate._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePersonnelOperationRateById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.personneloperationrate._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.personneloperationrate._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.personneloperationrate._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePersonnelOperationRateBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.personneloperationrate._self') }))
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
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
