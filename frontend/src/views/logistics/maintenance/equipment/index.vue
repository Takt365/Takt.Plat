<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/equipment -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt工厂设备实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      :id-column-key="'equipmentId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEquipmentId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'equipmentStatus'">
          <TaktDictTag
            :value="getEquipmentField(record, 'equipmentStatus')"
            dict-type="sys_equipment_status"
          />
        </template>
      </template>

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
      <EquipmentForm
        :key="formData?.equipmentId ?? 'create'"
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
      <a-form-item :label="t('common.page.entity.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('EquipCode')">
      <a-form-item :label="t('entity.equipment.code')">
        <a-input
          v-model:value="advancedQueryForm.EquipCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentName')">
      <a-form-item :label="t('entity.equipment.name')">
        <a-input
          v-model:value="advancedQueryForm.equipmentName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.name') })"
          show-count
          :maxlength="200"
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
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('EquipSpecification')">
      <a-form-item :label="t('entity.equipment.specification')">
        <a-input
          v-model:value="advancedQueryForm.EquipSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.specification') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('EquipBrand')">
      <a-form-item :label="t('entity.equipment.brand')">
        <a-input
          v-model:value="advancedQueryForm.EquipBrand"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.brand') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturer')">
      <a-form-item :label="t('entity.equipment.manufacturer')">
        <a-input
          v-model:value="advancedQueryForm.manufacturer"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.manufacturer') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dealerBy')">
      <a-form-item :label="t('entity.equipment.dealerby')">
        <a-input
          v-model:value="advancedQueryForm.dealerBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.dealerby') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serialNumber')">
      <a-form-item :label="t('entity.equipment.serialnumber')">
        <a-input
          v-model:value="advancedQueryForm.serialNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.serialnumber') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workshopBy')">
      <a-form-item :label="t('entity.equipment.workshopby')">
        <a-input
          v-model:value="advancedQueryForm.workshopBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.workshopby') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLineBy')">
      <a-form-item :label="t('entity.equipment.productionlineby')">
        <a-input
          v-model:value="advancedQueryForm.productionLineBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.productionlineby') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workstationBy')">
      <a-form-item :label="t('entity.equipment.workstationby')">
        <a-input
          v-model:value="advancedQueryForm.workstationBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.workstationby') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptBy')">
      <a-form-item :label="t('entity.equipment.deptby')">
        <a-input
          v-model:value="advancedQueryForm.deptBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.deptby') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentLocation')">
      <a-form-item :label="t('entity.equipment.location')">
        <a-input
          v-model:value="advancedQueryForm.equipmentLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.location') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleUserBy')">
      <a-form-item :label="t('entity.equipment.responsibleuserby')">
        <a-input
          v-model:value="advancedQueryForm.responsibleUserBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.responsibleuserby') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operatorBy')">
      <a-form-item :label="t('entity.equipment.operatorby')">
        <a-input
          v-model:value="advancedQueryForm.operatorBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.operatorby') })"
          show-count
          :maxlength="50"
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
          show-count
          :maxlength="4000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentImages')">
      <a-form-item :label="t('entity.equipment.images')">
        <a-input
          v-model:value="advancedQueryForm.equipmentImages"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.images') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentDocuments')">
      <a-form-item :label="t('entity.equipment.documents')">
        <a-input
          v-model:value="advancedQueryForm.equipmentDocuments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.equipment.documents') })"
          show-count
          :maxlength="2000"
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
        <TaktSelect
          v-model:value="advancedQueryForm.equipmentStatus"
          dict-type="sys_equipment_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.equipment.status') })"
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
 * Takt工厂设备实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/equipment
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import EquipmentForm from './components/equipment-form.vue'
import { getEquipmentList, getEquipmentById, createEquipment, updateEquipment, deleteEquipmentById, deleteEquipmentBatch, getEquipmentTemplate, importEquipment, exportEquipment, updateEquipmentStatus } from '@/api/logistics/maintenance/equipment'
import type { Equipment, EquipmentQuery } from '@/types/logistics/maintenance/equipment'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

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
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
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
const formData = ref<Partial<Equipment> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  EquipCode: '',
  equipmentName: '',
  equipmentType: undefined as number | undefined,
  equipmentModel: '',
  EquipSpecification: '',
  EquipBrand: '',
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
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('common.page.entity.plantcode') },
  { key: 'EquipCode', label: t('entity.equipment.code') },
  { key: 'equipmentName', label: t('entity.equipment.name') },
  { key: 'equipmentType', label: t('entity.equipment.type') },
  { key: 'equipmentModel', label: t('entity.equipment.model') },
  { key: 'EquipSpecification', label: t('entity.equipment.specification') },
  { key: 'EquipBrand', label: t('entity.equipment.brand') },
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
  { key: 'purchaseDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.equipment.purchasedate')) },
  { key: 'purchaseDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.equipment.purchasedate')) },
  { key: 'installationDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.equipment.installationdate')) },
  { key: 'installationDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.equipment.installationdate')) },
  { key: 'startDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.equipment.startdate')) },
  { key: 'startDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.equipment.startdate')) },
  { key: 'warrantyStartDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.equipment.warrantystartdate')) },
  { key: 'warrantyStartDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.equipment.warrantystartdate')) },
  { key: 'warrantyEndDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.equipment.warrantyenddate')) },
  { key: 'warrantyEndDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.equipment.warrantyenddate')) },
  { key: 'equipmentOriginalValue', label: t('entity.equipment.originalvalue') },
  { key: 'technicalParameters', label: t('entity.equipment.technicalparameters') },
  { key: 'equipmentImages', label: t('entity.equipment.images') },
  { key: 'equipmentDocuments', label: t('entity.equipment.documents') },
  { key: 'isCritical', label: t('entity.equipment.iscritical') },
  { key: 'warrantyStatus', label: t('entity.equipment.warrantystatus') },
  { key: 'equipmentStatus', label: t('entity.equipment.status') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])
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

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EquipmentQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EquipmentQuery>): EquipmentQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EquipmentQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EquipmentQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('EquipCode', form.EquipCode)
  assignTrimmed('equipmentName', form.equipmentName)
  if (form.equipmentType !== undefined && form.equipmentType !== null) {
    query.equipmentType = form.equipmentType
  }
  assignTrimmed('equipmentModel', form.equipmentModel)
  assignTrimmed('EquipSpecification', form.EquipSpecification)
  assignTrimmed('EquipBrand', form.EquipBrand)
  assignTrimmed('manufacturer', form.manufacturer)
  assignTrimmed('dealerBy', form.dealerBy)
  assignTrimmed('serialNumber', form.serialNumber)
  assignTrimmed('workshopBy', form.workshopBy)
  assignTrimmed('productionLineBy', form.productionLineBy)
  assignTrimmed('workstationBy', form.workstationBy)
  assignTrimmed('deptBy', form.deptBy)
  assignTrimmed('equipmentLocation', form.equipmentLocation)
  assignTrimmed('responsibleUserBy', form.responsibleUserBy)
  assignTrimmed('operatorBy', form.operatorBy)
  assignTrimmed('purchaseDateStart', form.purchaseDateStart)
  assignTrimmed('purchaseDateEnd', form.purchaseDateEnd)
  assignTrimmed('installationDateStart', form.installationDateStart)
  assignTrimmed('installationDateEnd', form.installationDateEnd)
  assignTrimmed('startDateStart', form.startDateStart)
  assignTrimmed('startDateEnd', form.startDateEnd)
  assignTrimmed('warrantyStartDateStart', form.warrantyStartDateStart)
  assignTrimmed('warrantyStartDateEnd', form.warrantyStartDateEnd)
  assignTrimmed('warrantyEndDateStart', form.warrantyEndDateStart)
  assignTrimmed('warrantyEndDateEnd', form.warrantyEndDateEnd)
  if (form.equipmentOriginalValue !== undefined && form.equipmentOriginalValue !== null) {
    query.equipmentOriginalValue = form.equipmentOriginalValue
  }
  assignTrimmed('technicalParameters', form.technicalParameters)
  assignTrimmed('equipmentImages', form.equipmentImages)
  assignTrimmed('equipmentDocuments', form.equipmentDocuments)
  if (form.isCritical !== undefined && form.isCritical !== null) {
    query.isCritical = form.isCritical
  }
  if (form.warrantyStatus !== undefined && form.warrantyStatus !== null) {
    query.warrantyStatus = form.warrantyStatus
  }
  if (form.equipmentStatus !== undefined && form.equipmentStatus !== null) {
    query.equipmentStatus = form.equipmentStatus
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
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

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
    title: t('common.page.entity.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.equipment.code'),
    dataIndex: 'EquipCode',
    key: 'EquipCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'EquipCode') ?? ''
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
    dataIndex: 'EquipSpecification',
    key: 'EquipSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'EquipSpecification') ?? ''
  },
  {
    title: t('entity.equipment.brand'),
    dataIndex: 'EquipBrand',
    key: 'EquipBrand',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getEquipmentField(record, 'EquipBrand') ?? ''
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
    } else if (selectedRow.value && getEquipmentId(selectedRow.value) === getEquipmentId(record)) {
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
    const res = await getEquipmentList(buildListQuery())
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
  EquipCode: '',
  equipmentName: '',
  equipmentType: undefined as number | undefined,
  equipmentModel: '',
  EquipSpecification: '',
  EquipBrand: '',
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
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.equipment._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: Equipment) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.equipment._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
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
    const exportMeta = await exportEquipment(
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
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  plantCode: '',
  EquipCode: '',
  equipmentName: '',
  equipmentType: undefined as number | undefined,
  equipmentModel: '',
  EquipSpecification: '',
  EquipBrand: '',
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
