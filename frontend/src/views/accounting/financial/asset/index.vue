<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/asset -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：资产实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="accounting:financial:asset:create"
      update-permission="accounting:financial:asset:update"
      delete-permission="accounting:financial:asset:delete"
      import-permission="accounting:financial:asset:import"
      export-permission="accounting:financial:asset:export"
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
      :id-column-key="'assetId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getAssetId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'assetCategory'">
          <TaktDictTag
            :value="getAssetField(record, 'assetCategory')"
            dict-type="accounting_asset_category"
          />
        </template>
        <template v-else-if="column.key === 'assetType'">
          <TaktDictTag
            :value="getAssetField(record, 'assetType')"
            dict-type="accounting_asset_type"
          />
        </template>
        <template v-else-if="column.key === 'assetStatus'">
          <TaktDictTag
            :value="getAssetField(record, 'assetStatus')"
            dict-type="accounting_asset_status"
          />
        </template>
        <template v-else-if="column.key === 'depreciationMethod'">
          <TaktDictTag
            :value="getAssetField(record, 'depreciationMethod')"
            dict-type="accounting_depreciation_method"
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
      <AssetForm
        :key="formData?.assetId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-financial-asset'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('assetCode')">
      <a-form-item :label="t('entity.asset.code')">
        <a-input
          v-model:value="advancedQueryForm.assetCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.code') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assetName')">
      <a-form-item :label="t('entity.asset.name')">
        <a-input
          v-model:value="advancedQueryForm.assetName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.name') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assetCategory')">
      <a-form-item :label="t('entity.asset.category')">
        <TaktSelect
          v-model:value="advancedQueryForm.assetCategory"
          dict-type="accounting_asset_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.category') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assetType')">
      <a-form-item :label="t('entity.asset.type')">
        <TaktSelect
          v-model:value="advancedQueryForm.assetType"
          dict-type="accounting_asset_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.type') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assetOriginalValue')">
      <a-form-item :label="t('entity.asset.originalvalue')">
        <a-input-number
          v-model:value="advancedQueryForm.assetOriginalValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.originalvalue') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assetNetValue')">
      <a-form-item :label="t('entity.asset.netvalue')">
        <a-input-number
          v-model:value="advancedQueryForm.assetNetValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.netvalue') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accumulatedDepreciation')">
      <a-form-item :label="t('entity.asset.accumulateddepreciation')">
        <a-input-number
          v-model:value="advancedQueryForm.accumulatedDepreciation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.accumulateddepreciation') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterId')">
      <a-form-item :label="t('entity.asset.costcenterid')">
        <a-input
          v-model:value="advancedQueryForm.costCenterId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.costcenterid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterName')">
      <a-form-item :label="t('entity.asset.costcentername')">
        <a-input
          v-model:value="advancedQueryForm.costCenterName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.costcentername') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="t('entity.asset.deptid')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.deptid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="t('entity.asset.deptname')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.deptname') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('userId')">
      <a-form-item :label="t('entity.asset.userid')">
        <a-input
          v-model:value="advancedQueryForm.userId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.userid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('userName')">
      <a-form-item :label="t('entity.asset.username')">
        <a-input
          v-model:value="advancedQueryForm.userName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.username') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assetLocation')">
      <a-form-item :label="t('entity.asset.location')">
        <a-input
          v-model:value="advancedQueryForm.assetLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.location') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseDateStart')">
      <a-form-item :label="t('entity.asset.purchasedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.purchaseDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.purchasedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseDateEnd')">
      <a-form-item :label="t('entity.asset.purchasedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.purchaseDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.purchasedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateStart')">
      <a-form-item :label="t('entity.asset.startdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.startdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateEnd')">
      <a-form-item :label="t('entity.asset.startdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.startdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scrapDateStart')">
      <a-form-item :label="t('entity.asset.scrapdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.scrapDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.scrapdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scrapDateEnd')">
      <a-form-item :label="t('entity.asset.scrapdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.scrapDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.scrapdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('disposalDateStart')">
      <a-form-item :label="t('entity.asset.disposaldatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.disposalDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.disposaldatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('disposalDateEnd')">
      <a-form-item :label="t('entity.asset.disposaldateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.disposalDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.disposaldateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expectedLifeMonths')">
      <a-form-item :label="t('entity.asset.expectedlifemonths')">
        <a-input-number
          v-model:value="advancedQueryForm.expectedLifeMonths"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.expectedlifemonths') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('depreciationMethod')">
      <a-form-item :label="t('entity.asset.depreciationmethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.depreciationMethod"
          dict-type="accounting_depreciation_method"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.depreciationmethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('monthlyDepreciation')">
      <a-form-item :label="t('entity.asset.monthlydepreciation')">
        <a-input-number
          v-model:value="advancedQueryForm.monthlyDepreciation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.monthlydepreciation') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.asset.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.relatedplant') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assetStatus')">
      <a-form-item :label="t('entity.asset.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.assetStatus"
          dict-type="accounting_asset_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.status') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.asset._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.asset._self"
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
      :id-column-key="'assetId'"
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
 * 资产实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/asset
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/takt-paged'
import AssetForm from './components/asset-form.vue'
import { getAssetList, getAssetById, createAsset, updateAsset, deleteAssetById, deleteAssetBatch, getAssetTemplate, importAsset, exportAsset, updateAssetStatus } from '@/api/accounting/financial/asset'
import type { Asset, AssetQuery } from '@/types/accounting/financial/asset'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：字典缓存 */
const dictDataStore = useDictDataStore()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAsset')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.asset._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Asset[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Asset | null>(null)
/** 表格多选行 */
const selectedRows = ref<Asset[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Asset> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  assetCode: '',
  assetName: '',
  assetCategory: '',
  assetType: undefined as string | undefined,
  assetOriginalValue: undefined as number | undefined,
  assetNetValue: undefined as number | undefined,
  accumulatedDepreciation: undefined as number | undefined,
  costCenterId: '',
  costCenterName: '',
  deptId: '',
  deptName: '',
  userId: '',
  userName: '',
  assetLocation: '',
  purchaseDateStart: '',
  purchaseDateEnd: '',
  startDateStart: '',
  startDateEnd: '',
  scrapDateStart: '',
  scrapDateEnd: '',
  disposalDateStart: '',
  disposalDateEnd: '',
  expectedLifeMonths: undefined as number | undefined,
  depreciationMethod: undefined as number | undefined,
  monthlyDepreciation: undefined as number | undefined,
  plantCode: '',
  assetStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'assetCode', label: t('entity.asset.code') },
  { key: 'assetName', label: t('entity.asset.name') },
  { key: 'assetCategory', label: t('entity.asset.category') },
  { key: 'assetType', label: t('entity.asset.type') },
  { key: 'assetOriginalValue', label: t('entity.asset.originalvalue') },
  { key: 'assetNetValue', label: t('entity.asset.netvalue') },
  { key: 'accumulatedDepreciation', label: t('entity.asset.accumulateddepreciation') },
  { key: 'costCenterId', label: t('entity.asset.costcenterid') },
  { key: 'costCenterName', label: t('entity.asset.costcentername') },
  { key: 'deptId', label: t('entity.asset.deptid') },
  { key: 'deptName', label: t('entity.asset.deptname') },
  { key: 'userId', label: t('entity.asset.userid') },
  { key: 'userName', label: t('entity.asset.username') },
  { key: 'assetLocation', label: t('entity.asset.location') },
  { key: 'purchaseDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.asset.purchasedate')) },
  { key: 'purchaseDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.asset.purchasedate')) },
  { key: 'startDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.asset.startdate')) },
  { key: 'startDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.asset.startdate')) },
  { key: 'scrapDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.asset.scrapdate')) },
  { key: 'scrapDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.asset.scrapdate')) },
  { key: 'disposalDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.asset.disposaldate')) },
  { key: 'disposalDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.asset.disposaldate')) },
  { key: 'expectedLifeMonths', label: t('entity.asset.expectedlifemonths') },
  { key: 'depreciationMethod', label: t('entity.asset.depreciationmethod') },
  { key: 'monthlyDepreciation', label: t('entity.asset.monthlydepreciation') },
  { key: 'plantCode', label: t('entity.asset.relatedplant') },
  { key: 'assetStatus', label: t('entity.asset.status') },
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
const entityIdName = 'assetId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {AssetQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<AssetQuery>): AssetQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: AssetQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof AssetQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('assetCode', form.assetCode)
  assignTrimmed('assetName', form.assetName)
  assignTrimmed('assetCategory', form.assetCategory)
  if (form.assetType !== undefined && form.assetType !== null) {
    query.assetType = form.assetType
  }
  if (form.assetOriginalValue !== undefined && form.assetOriginalValue !== null) {
    query.assetOriginalValue = form.assetOriginalValue
  }
  if (form.assetNetValue !== undefined && form.assetNetValue !== null) {
    query.assetNetValue = form.assetNetValue
  }
  if (form.accumulatedDepreciation !== undefined && form.accumulatedDepreciation !== null) {
    query.accumulatedDepreciation = form.accumulatedDepreciation
  }
  assignTrimmed('costCenterId', form.costCenterId)
  assignTrimmed('costCenterName', form.costCenterName)
  assignTrimmed('deptId', form.deptId)
  assignTrimmed('deptName', form.deptName)
  assignTrimmed('userId', form.userId)
  assignTrimmed('userName', form.userName)
  assignTrimmed('assetLocation', form.assetLocation)
  assignTrimmed('purchaseDateStart', form.purchaseDateStart)
  assignTrimmed('purchaseDateEnd', form.purchaseDateEnd)
  assignTrimmed('startDateStart', form.startDateStart)
  assignTrimmed('startDateEnd', form.startDateEnd)
  assignTrimmed('scrapDateStart', form.scrapDateStart)
  assignTrimmed('scrapDateEnd', form.scrapDateEnd)
  assignTrimmed('disposalDateStart', form.disposalDateStart)
  assignTrimmed('disposalDateEnd', form.disposalDateEnd)
  if (form.expectedLifeMonths !== undefined && form.expectedLifeMonths !== null) {
    query.expectedLifeMonths = form.expectedLifeMonths
  }
  if (form.depreciationMethod !== undefined && form.depreciationMethod !== null) {
    query.depreciationMethod = form.depreciationMethod
  }
  if (form.monthlyDepreciation !== undefined && form.monthlyDepreciation !== null) {
    query.monthlyDepreciation = form.monthlyDepreciation
  }
  assignTrimmed('plantCode', form.plantCode)
  if (form.assetStatus !== undefined && form.assetStatus !== null) {
    query.assetStatus = form.assetStatus
  }
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  void dictDataStore.loadAllDictDataAsync()
  await ensureTaktPaginationConfigAsync()
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'assetId',
    key: 'assetId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetId') ?? ''
  },
  {
    title: t('entity.asset.code'),
    dataIndex: 'assetCode',
    key: 'assetCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetCode') ?? ''
  },
  {
    title: t('entity.asset.name'),
    dataIndex: 'assetName',
    key: 'assetName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetName') ?? ''
  },
  {
    title: t('entity.asset.category'),
    dataIndex: 'assetCategory',
    key: 'assetCategory',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.asset.type'),
    dataIndex: 'assetType',
    key: 'assetType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetType') ?? ''
  },
  {
    title: t('entity.asset.originalvalue'),
    dataIndex: 'assetOriginalValue',
    key: 'assetOriginalValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetOriginalValue') ?? ''
  },
  {
    title: t('entity.asset.netvalue'),
    dataIndex: 'assetNetValue',
    key: 'assetNetValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetNetValue') ?? ''
  },
  {
    title: t('entity.asset.accumulateddepreciation'),
    dataIndex: 'accumulatedDepreciation',
    key: 'accumulatedDepreciation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'accumulatedDepreciation') ?? ''
  },
  {
    title: t('entity.asset.costcenterid'),
    dataIndex: 'costCenterId',
    key: 'costCenterId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'costCenterId') ?? ''
  },
  {
    title: t('entity.asset.costcentername'),
    dataIndex: 'costCenterName',
    key: 'costCenterName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'costCenterName') ?? ''
  },
  {
    title: t('entity.asset.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.asset.deptname'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'deptName') ?? ''
  },
  {
    title: t('entity.asset.userid'),
    dataIndex: 'userId',
    key: 'userId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'userId') ?? ''
  },
  {
    title: t('entity.asset.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'userName') ?? ''
  },
  {
    title: t('entity.asset.location'),
    dataIndex: 'assetLocation',
    key: 'assetLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetLocation') ?? ''
  },
  {
    title: t('entity.asset.purchasedate'),
    dataIndex: 'purchaseDate',
    key: 'purchaseDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'purchaseDate') ?? ''
  },
  {
    title: t('entity.asset.startdate'),
    dataIndex: 'startDate',
    key: 'startDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'startDate') ?? ''
  },
  {
    title: t('entity.asset.scrapdate'),
    dataIndex: 'scrapDate',
    key: 'scrapDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'scrapDate') ?? ''
  },
  {
    title: t('entity.asset.disposaldate'),
    dataIndex: 'disposalDate',
    key: 'disposalDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'disposalDate') ?? ''
  },
  {
    title: t('entity.asset.expectedlifemonths'),
    dataIndex: 'expectedLifeMonths',
    key: 'expectedLifeMonths',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'expectedLifeMonths') ?? ''
  },
  {
    title: t('entity.asset.depreciationmethod'),
    dataIndex: 'depreciationMethod',
    key: 'depreciationMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'depreciationMethod') ?? ''
  },
  {
    title: t('entity.asset.monthlydepreciation'),
    dataIndex: 'monthlyDepreciation',
    key: 'monthlyDepreciation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'monthlyDepreciation') ?? ''
  },
  {
    title: t('entity.asset.relatedplant'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.asset.status'),
    dataIndex: 'assetStatus',
    key: 'assetStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssetField(record, 'assetStatus') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:asset:update',
        onClick: (record: Asset) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:asset:delete',
        onClick: (record: Asset) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getAssetId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getAssetField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Asset[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Asset, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getAssetId(selectedRow.value) === getAssetId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Asset[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Asset) => ({
  onClick: () => {
    const key = getAssetId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getAssetId(item)))
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
    const res = await getAssetList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Asset] 加载数据失败', { error })
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
  assetCode: '',
  assetName: '',
  assetCategory: '',
  assetType: undefined as string | undefined,
  assetOriginalValue: undefined as number | undefined,
  assetNetValue: undefined as number | undefined,
  accumulatedDepreciation: undefined as number | undefined,
  costCenterId: '',
  costCenterName: '',
  deptId: '',
  deptName: '',
  userId: '',
  userName: '',
  assetLocation: '',
  purchaseDateStart: '',
  purchaseDateEnd: '',
  startDateStart: '',
  startDateEnd: '',
  scrapDateStart: '',
  scrapDateEnd: '',
  disposalDateStart: '',
  disposalDateEnd: '',
  expectedLifeMonths: undefined as number | undefined,
  depreciationMethod: undefined as number | undefined,
  monthlyDepreciation: undefined as number | undefined,
  plantCode: '',
  assetStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.asset._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: Asset) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.asset._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.asset._self') }))
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
      await updateAsset(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.asset._self') }))
    } else {
      await createAsset(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.asset._self') }))
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
  const res = await getAssetTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importAsset(file, sheetName)
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
    const exportMeta = await exportAsset(
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
    message.success(t('common.feedback.export.success', { target: t('entity.asset._self') }))
  } catch (error: any) {
    logger.error('[Asset] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.asset._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Asset) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.asset._self'), name: t('common.tip.this.target', { target: t('entity.asset._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssetById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.asset._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.asset._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.asset._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAssetBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.asset._self') }))
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
  assetCode: '',
  assetName: '',
  assetCategory: '',
  assetType: undefined as string | undefined,
  assetOriginalValue: undefined as number | undefined,
  assetNetValue: undefined as number | undefined,
  accumulatedDepreciation: undefined as number | undefined,
  costCenterId: '',
  costCenterName: '',
  deptId: '',
  deptName: '',
  userId: '',
  userName: '',
  assetLocation: '',
  purchaseDateStart: '',
  purchaseDateEnd: '',
  startDateStart: '',
  startDateEnd: '',
  scrapDateStart: '',
  scrapDateEnd: '',
  disposalDateStart: '',
  disposalDateEnd: '',
  expectedLifeMonths: undefined as number | undefined,
  depreciationMethod: undefined as number | undefined,
  monthlyDepreciation: undefined as number | undefined,
  plantCode: '',
  assetStatus: undefined as number | undefined,
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
