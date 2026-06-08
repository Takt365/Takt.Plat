<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/equipment -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt工厂设备实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-maintenance-equipment">
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
      create-permission="logistics:maintenance:equipment:create"
      update-permission="logistics:maintenance:equipment:update"
      delete-permission="logistics:maintenance:equipment:delete"
      import-permission="logistics:maintenance:equipment:import"
      export-permission="logistics:maintenance:equipment:export"
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
      :id-column-key="'equipmentId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEquipmentId"
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
          <div class="mb-2 text-sm font-medium">{{ t('entity.maintenance._self') }}</div>
          <a-table
            v-if="hasMaintenanceRows(record)"
            :columns="maintenanceExpandColumns"
            :data-source="getMaintenanceRows(record)"
            :row-key="(row: Maintenance, index?: number) => row?.maintenanceId || String(index ?? 0)"
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
      <EquipmentForm
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
      :storage-key="'takt-query-fields-logistics-maintenance-equipment'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.equipment.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentCode')">
      <a-form-item :label="t('entity.equipment.code')">
        <a-input
          v-model:value="advancedQueryForm.equipmentCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentName')">
      <a-form-item :label="t('entity.equipment.name')">
        <a-input
          v-model:value="advancedQueryForm.equipmentName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.name') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentType')">
      <a-form-item :label="t('entity.equipment.type')">
        <a-input-number
          v-model:value="advancedQueryForm.equipmentType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentModel')">
      <a-form-item :label="t('entity.equipment.model')">
        <a-input
          v-model:value="advancedQueryForm.equipmentModel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.model') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentSpecification')">
      <a-form-item :label="t('entity.equipment.specification')">
        <a-input
          v-model:value="advancedQueryForm.equipmentSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.specification') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentBrand')">
      <a-form-item :label="t('entity.equipment.brand')">
        <a-input
          v-model:value="advancedQueryForm.equipmentBrand"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.brand') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturer')">
      <a-form-item :label="t('entity.equipment.manufacturer')">
        <a-input
          v-model:value="advancedQueryForm.manufacturer"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.manufacturer') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dealerBy')">
      <a-form-item :label="t('entity.equipment.dealerby')">
        <a-input
          v-model:value="advancedQueryForm.dealerBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.dealerby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serialNumber')">
      <a-form-item :label="t('entity.equipment.serialnumber')">
        <a-input
          v-model:value="advancedQueryForm.serialNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.serialnumber') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workshopBy')">
      <a-form-item :label="t('entity.equipment.workshopby')">
        <a-input
          v-model:value="advancedQueryForm.workshopBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.workshopby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLineBy')">
      <a-form-item :label="t('entity.equipment.productionlineby')">
        <a-input
          v-model:value="advancedQueryForm.productionLineBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.productionlineby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workstationBy')">
      <a-form-item :label="t('entity.equipment.workstationby')">
        <a-input
          v-model:value="advancedQueryForm.workstationBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.workstationby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptBy')">
      <a-form-item :label="t('entity.equipment.deptby')">
        <a-input
          v-model:value="advancedQueryForm.deptBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.deptby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentLocation')">
      <a-form-item :label="t('entity.equipment.location')">
        <a-input
          v-model:value="advancedQueryForm.equipmentLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.location') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleUserBy')">
      <a-form-item :label="t('entity.equipment.responsibleuserby')">
        <a-input
          v-model:value="advancedQueryForm.responsibleUserBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.responsibleuserby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operatorBy')">
      <a-form-item :label="t('entity.equipment.operatorby')">
        <a-input
          v-model:value="advancedQueryForm.operatorBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.operatorby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseDateStart')">
      <a-form-item :label="t('entity.equipment.purchasedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.purchaseDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.purchasedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseDateEnd')">
      <a-form-item :label="t('entity.equipment.purchasedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.purchaseDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.purchasedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('installationDateStart')">
      <a-form-item :label="t('entity.equipment.installationdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.installationDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.installationdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('installationDateEnd')">
      <a-form-item :label="t('entity.equipment.installationdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.installationDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.installationdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateStart')">
      <a-form-item :label="t('entity.equipment.startdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.startdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateEnd')">
      <a-form-item :label="t('entity.equipment.startdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.startdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyStartDateStart')">
      <a-form-item :label="t('entity.equipment.warrantystartdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyStartDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantystartdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyStartDateEnd')">
      <a-form-item :label="t('entity.equipment.warrantystartdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyStartDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantystartdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyEndDateStart')">
      <a-form-item :label="t('entity.equipment.warrantyenddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyEndDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantyenddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyEndDateEnd')">
      <a-form-item :label="t('entity.equipment.warrantyenddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyEndDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.warrantyenddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentOriginalValue')">
      <a-form-item :label="t('entity.equipment.originalvalue')">
        <a-input-number
          v-model:value="advancedQueryForm.equipmentOriginalValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.originalvalue') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('technicalParameters')">
      <a-form-item :label="t('entity.equipment.technicalparameters')">
        <a-input
          v-model:value="advancedQueryForm.technicalParameters"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.technicalparameters') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentImages')">
      <a-form-item :label="t('entity.equipment.images')">
        <a-input
          v-model:value="advancedQueryForm.equipmentImages"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.images') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentDocuments')">
      <a-form-item :label="t('entity.equipment.documents')">
        <a-input
          v-model:value="advancedQueryForm.equipmentDocuments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.documents') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCritical')">
      <a-form-item :label="t('entity.equipment.iscritical')">
        <a-input-number
          v-model:value="advancedQueryForm.isCritical"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.iscritical') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyStatus')">
      <a-form-item :label="t('entity.equipment.warrantystatus')">
        <a-input-number
          v-model:value="advancedQueryForm.warrantyStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.warrantystatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentStatus')">
      <a-form-item :label="t('entity.equipment.status')">
        <a-input-number
          v-model:value="advancedQueryForm.equipmentStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.status') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.equipment._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.equipment._self"
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
      :id-column-key="'equipmentId'"
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
 * Takt工厂设备实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/equipment
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import EquipmentForm from './components/equipment-form.vue'
import { getEquipmentList, getEquipmentById, createEquipment, updateEquipment, deleteEquipmentById, deleteEquipmentBatch, getEquipmentTemplate, importEquipment, exportEquipment } from '@/api/logistics/maintenance/equipment'
import * as maintenanceApi from '@/api/logistics/maintenance/maintenance'
import type { Maintenance, MaintenanceQuery } from '@/types/logistics/maintenance/maintenance'
import type { Equipment, EquipmentQuery, EquipmentCreate, EquipmentUpdate } from '@/types/logistics/maintenance/equipment'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEquipment')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.equipment._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Equipment[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Equipment | null>(null)
/** 表格多选行 */
const selectedRows = ref<Equipment[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Equipment>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  equipmentCode: '',
  equipmentName: '',
  equipmentType: undefined as number | undefined,
  equipmentModel: '',
  equipmentSpecification: '',
  equipmentBrand: '',
  manufacturer: '',
  dealerBy: '',
  serialNumber: '',
  workshopBy: '',
  productionLineBy: '',
  workstationBy: '',
  deptBy: '',
  equipmentLocation: '',
  responsibleUserBy: '',
  operatorBy: '',
  purchaseDateStart: '',
  purchaseDateEnd: '',
  installationDateStart: '',
  installationDateEnd: '',
  startDateStart: '',
  startDateEnd: '',
  warrantyStartDateStart: '',
  warrantyStartDateEnd: '',
  warrantyEndDateStart: '',
  warrantyEndDateEnd: '',
  equipmentOriginalValue: undefined as number | undefined,
  technicalParameters: '',
  equipmentImages: '',
  equipmentDocuments: '',
  isCritical: undefined as number | undefined,
  warrantyStatus: undefined as number | undefined,
  equipmentStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.equipment.plantcode') },
  { key: 'equipmentCode', label: t('entity.equipment.code') },
  { key: 'equipmentName', label: t('entity.equipment.name') },
  { key: 'equipmentType', label: t('entity.equipment.type') },
  { key: 'equipmentModel', label: t('entity.equipment.model') },
  { key: 'equipmentSpecification', label: t('entity.equipment.specification') },
  { key: 'equipmentBrand', label: t('entity.equipment.brand') },
  { key: 'manufacturer', label: t('entity.equipment.manufacturer') },
  { key: 'dealerBy', label: t('entity.equipment.dealerby') },
  { key: 'serialNumber', label: t('entity.equipment.serialnumber') },
  { key: 'workshopBy', label: t('entity.equipment.workshopby') },
  { key: 'productionLineBy', label: t('entity.equipment.productionlineby') },
  { key: 'workstationBy', label: t('entity.equipment.workstationby') },
  { key: 'deptBy', label: t('entity.equipment.deptby') },
  { key: 'equipmentLocation', label: t('entity.equipment.location') },
  { key: 'responsibleUserBy', label: t('entity.equipment.responsibleuserby') },
  { key: 'operatorBy', label: t('entity.equipment.operatorby') },
  { key: 'purchaseDateStart', label: t('entity.equipment.purchasedatestart') },
  { key: 'purchaseDateEnd', label: t('entity.equipment.purchasedateend') },
  { key: 'installationDateStart', label: t('entity.equipment.installationdatestart') },
  { key: 'installationDateEnd', label: t('entity.equipment.installationdateend') },
  { key: 'startDateStart', label: t('entity.equipment.startdatestart') },
  { key: 'startDateEnd', label: t('entity.equipment.startdateend') },
  { key: 'warrantyStartDateStart', label: t('entity.equipment.warrantystartdatestart') },
  { key: 'warrantyStartDateEnd', label: t('entity.equipment.warrantystartdateend') },
  { key: 'warrantyEndDateStart', label: t('entity.equipment.warrantyenddatestart') },
  { key: 'warrantyEndDateEnd', label: t('entity.equipment.warrantyenddateend') },
  { key: 'equipmentOriginalValue', label: t('entity.equipment.originalvalue') },
  { key: 'technicalParameters', label: t('entity.equipment.technicalparameters') },
  { key: 'equipmentImages', label: t('entity.equipment.images') },
  { key: 'equipmentDocuments', label: t('entity.equipment.documents') },
  { key: 'isCritical', label: t('entity.equipment.iscritical') },
  { key: 'warrantyStatus', label: t('entity.equipment.warrantystatus') },
  { key: 'equipmentStatus', label: t('entity.equipment.status') },
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
const entityIdName = 'equipmentId'
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

/** 展开行预览：maintenance 列 */
const maintenanceExpandColumns = computed(() => [
  {
    title: t('entity.maintenance.equipmentname'),
    dataIndex: 'equipmentName',
    key: 'equipmentName',
    ellipsis: true,
  },
  {
    title: t('entity.maintenance.equipmentcode'),
    dataIndex: 'equipmentCode',
    key: 'equipmentCode',
    ellipsis: true,
  },
  {
    title: t('entity.maintenance.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    ellipsis: true,
  },
  {
    title: t('entity.maintenance.type'),
    dataIndex: 'maintenanceType',
    key: 'maintenanceType',
    ellipsis: true,
  },
  {
    title: t('entity.maintenance.company'),
    dataIndex: 'maintenanceCompany',
    key: 'maintenanceCompany',
    ellipsis: true,
  },
  {
    title: t('entity.maintenance.technician'),
    dataIndex: 'maintenanceTechnician',
    key: 'maintenanceTechnician',
    ellipsis: true,
  },
  {
    title: t('entity.maintenance.date'),
    dataIndex: 'maintenanceDate',
    key: 'maintenanceDate',
    ellipsis: true,
  },
  {
    title: t('entity.maintenance.starttime'),
    dataIndex: 'maintenanceStartTime',
    key: 'maintenanceStartTime',
    ellipsis: true,
  },
])

/** 读取主表行上的 maintenance 子表缓存 */
function getMaintenanceRows(record: Equipment): Maintenance[] {
  return (record as any)?.maintenanceRecords ?? []
}

/** 主表行是否已加载 maintenance 子表 */
function hasMaintenanceRows(record: Equipment): boolean {
  return getMaintenanceRows(record).length > 0
}


/** 加载主表详情并回填当前页 dataSource */
async function loadEquipmentDetail(record: Equipment): Promise<Equipment | null> {
  const id = getEquipmentId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getEquipmentById(id)
    const index = dataSource.value.findIndex((row) => getEquipmentId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Equipment
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 懒加载 maintenance 子表（MaintenanceQuery + maintenanceApi，与主表 EquipmentQuery 分离） */
async function loadMaintenanceForEquipment(record: Equipment): Promise<Maintenance[]> {
  const masterId = getEquipmentId(record)
  if (!masterId) {
    return []
  }
  try {
    const childQuery: MaintenanceQuery = {
      pageIndex: 1,
      pageSize: 500,
      equipmentId: masterId,
    }
    const result = await maintenanceApi.getMaintenanceList(childQuery)
    const rows = result?.data ?? []
    const index = dataSource.value.findIndex((row) => getEquipmentId(row) === masterId)
    if (index !== -1) {
      const row = dataSource.value[index]
      dataSource.value[index] = { ...row, maintenanceRecords: rows } as Equipment
    }
    return rows
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return []
  }
}

/** 展开前确保各子表已懒加载 */
async function ensureEquipmentChildrenLoaded(record: Equipment) {
  if (!hasMaintenanceRows(record)) {
    await loadMaintenanceForEquipment(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: Equipment) {
  const key = getEquipmentId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureEquipmentChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'equipmentId',
    key: 'equipmentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentId') ?? ''
  },
  {
    title: t('entity.equipment.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.equipment.code'),
    dataIndex: 'equipmentCode',
    key: 'equipmentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentCode') ?? ''
  },
  {
    title: t('entity.equipment.name'),
    dataIndex: 'equipmentName',
    key: 'equipmentName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentName') ?? ''
  },
  {
    title: t('entity.equipment.type'),
    dataIndex: 'equipmentType',
    key: 'equipmentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentType') ?? ''
  },
  {
    title: t('entity.equipment.model'),
    dataIndex: 'equipmentModel',
    key: 'equipmentModel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentModel') ?? ''
  },
  {
    title: t('entity.equipment.specification'),
    dataIndex: 'equipmentSpecification',
    key: 'equipmentSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentSpecification') ?? ''
  },
  {
    title: t('entity.equipment.brand'),
    dataIndex: 'equipmentBrand',
    key: 'equipmentBrand',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentBrand') ?? ''
  },
  {
    title: t('entity.equipment.manufacturer'),
    dataIndex: 'manufacturer',
    key: 'manufacturer',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'manufacturer') ?? ''
  },
  {
    title: t('entity.equipment.dealerby'),
    dataIndex: 'dealerBy',
    key: 'dealerBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'dealerBy') ?? ''
  },
  {
    title: t('entity.equipment.serialnumber'),
    dataIndex: 'serialNumber',
    key: 'serialNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'serialNumber') ?? ''
  },
  {
    title: t('entity.equipment.workshopby'),
    dataIndex: 'workshopBy',
    key: 'workshopBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'workshopBy') ?? ''
  },
  {
    title: t('entity.equipment.productionlineby'),
    dataIndex: 'productionLineBy',
    key: 'productionLineBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'productionLineBy') ?? ''
  },
  {
    title: t('entity.equipment.workstationby'),
    dataIndex: 'workstationBy',
    key: 'workstationBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'workstationBy') ?? ''
  },
  {
    title: t('entity.equipment.deptby'),
    dataIndex: 'deptBy',
    key: 'deptBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'deptBy') ?? ''
  },
  {
    title: t('entity.equipment.location'),
    dataIndex: 'equipmentLocation',
    key: 'equipmentLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentLocation') ?? ''
  },
  {
    title: t('entity.equipment.responsibleuserby'),
    dataIndex: 'responsibleUserBy',
    key: 'responsibleUserBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'responsibleUserBy') ?? ''
  },
  {
    title: t('entity.equipment.operatorby'),
    dataIndex: 'operatorBy',
    key: 'operatorBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'operatorBy') ?? ''
  },
  {
    title: t('entity.equipment.purchasedate'),
    dataIndex: 'purchaseDate',
    key: 'purchaseDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'purchaseDate') ?? ''
  },
  {
    title: t('entity.equipment.installationdate'),
    dataIndex: 'installationDate',
    key: 'installationDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'installationDate') ?? ''
  },
  {
    title: t('entity.equipment.startdate'),
    dataIndex: 'startDate',
    key: 'startDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'startDate') ?? ''
  },
  {
    title: t('entity.equipment.warrantystartdate'),
    dataIndex: 'warrantyStartDate',
    key: 'warrantyStartDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'warrantyStartDate') ?? ''
  },
  {
    title: t('entity.equipment.warrantyenddate'),
    dataIndex: 'warrantyEndDate',
    key: 'warrantyEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'warrantyEndDate') ?? ''
  },
  {
    title: t('entity.equipment.originalvalue'),
    dataIndex: 'equipmentOriginalValue',
    key: 'equipmentOriginalValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentOriginalValue') ?? ''
  },
  {
    title: t('entity.equipment.technicalparameters'),
    dataIndex: 'technicalParameters',
    key: 'technicalParameters',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'technicalParameters') ?? ''
  },
  {
    title: t('entity.equipment.images'),
    dataIndex: 'equipmentImages',
    key: 'equipmentImages',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentImages') ?? ''
  },
  {
    title: t('entity.equipment.documents'),
    dataIndex: 'equipmentDocuments',
    key: 'equipmentDocuments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentDocuments') ?? ''
  },
  {
    title: t('entity.equipment.iscritical'),
    dataIndex: 'isCritical',
    key: 'isCritical',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'isCritical') ?? ''
  },
  {
    title: t('entity.equipment.warrantystatus'),
    dataIndex: 'warrantyStatus',
    key: 'warrantyStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'warrantyStatus') ?? ''
  },
  {
    title: t('entity.equipment.status'),
    dataIndex: 'equipmentStatus',
    key: 'equipmentStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'equipmentStatus') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:maintenance:equipment:update',
        onClick: (record: Equipment) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:maintenance:equipment:delete',
        onClick: (record: Equipment) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getEquipmentId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getEquipmentField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Equipment[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Equipment, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getEquipmentId(selectedRow.value) === getEquipmentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Equipment[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Equipment) => ({
  onClick: () => {
    const key = getEquipmentId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getEquipmentId(item)))
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
    const params: EquipmentQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getEquipmentList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Equipment] 加载数据失败', { error })
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
  equipmentCode: '',
  equipmentName: '',
  equipmentType: undefined as number | undefined,
  equipmentModel: '',
  equipmentSpecification: '',
  equipmentBrand: '',
  manufacturer: '',
  dealerBy: '',
  serialNumber: '',
  workshopBy: '',
  productionLineBy: '',
  workstationBy: '',
  deptBy: '',
  equipmentLocation: '',
  responsibleUserBy: '',
  operatorBy: '',
  purchaseDateStart: '',
  purchaseDateEnd: '',
  installationDateStart: '',
  installationDateEnd: '',
  startDateStart: '',
  startDateEnd: '',
  warrantyStartDateStart: '',
  warrantyStartDateEnd: '',
  warrantyEndDateStart: '',
  warrantyEndDateEnd: '',
  equipmentOriginalValue: undefined as number | undefined,
  technicalParameters: '',
  equipmentImages: '',
  equipmentDocuments: '',
  isCritical: undefined as number | undefined,
  warrantyStatus: undefined as number | undefined,
  equipmentStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.equipment._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Equipment) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.equipment._self') })
  formLoading.value = true
  try {
    const detail = await loadEquipmentDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.equipment._self') }))
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
      await updateEquipment(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.equipment._self') }))
    } else {
      await createEquipment(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.equipment._self') }))
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
  const res = await getEquipmentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEquipment(file, sheetName)
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
    const exportQuery: EquipmentQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportEquipment(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.equipment._self') }))
  } catch (error: any) {
    logger.error('[Equipment] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.equipment._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Equipment) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.equipment._self'), name: t('common.tip.this.target', { target: t('entity.equipment._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEquipmentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.equipment._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.equipment._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.equipment._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteEquipmentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.equipment._self') }))
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
  equipmentCode: '',
  equipmentName: '',
  equipmentType: undefined as number | undefined,
  equipmentModel: '',
  equipmentSpecification: '',
  equipmentBrand: '',
  manufacturer: '',
  dealerBy: '',
  serialNumber: '',
  workshopBy: '',
  productionLineBy: '',
  workstationBy: '',
  deptBy: '',
  equipmentLocation: '',
  responsibleUserBy: '',
  operatorBy: '',
  purchaseDateStart: '',
  purchaseDateEnd: '',
  installationDateStart: '',
  installationDateEnd: '',
  startDateStart: '',
  startDateEnd: '',
  warrantyStartDateStart: '',
  warrantyStartDateEnd: '',
  warrantyEndDateStart: '',
  warrantyEndDateEnd: '',
  equipmentOriginalValue: undefined as number | undefined,
  technicalParameters: '',
  equipmentImages: '',
  equipmentDocuments: '',
  isCritical: undefined as number | undefined,
  warrantyStatus: undefined as number | undefined,
  equipmentStatus: undefined as number | undefined,
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
.logistics-maintenance-equipment {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
