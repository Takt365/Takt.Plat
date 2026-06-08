<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/statistics/report/logistics/material -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt物料实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="statistics-report-logistics-material">
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
      create-permission="statistics:report:logistics:material:create"
      update-permission="statistics:report:logistics:material:update"
      delete-permission="statistics:report:logistics:material:delete"
      import-permission="statistics:report:logistics:material:import"
      export-permission="statistics:report:logistics:material:export"
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
      :id-column-key="'materialId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getMaterialId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'materialStatus'">
          <TaktDictTag
            :value="getMaterialField(record, 'materialStatus')"
            dict-type="sys_normal_disable"
          />
        </template>
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
      <MaterialForm
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
      :storage-key="'takt-query-fields-statistics-report-logistics-material'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.material.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.plantcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.material.code')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialName')">
      <a-form-item :label="t('entity.material.name')">
        <a-input
          v-model:value="advancedQueryForm.materialName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.name') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="t('entity.material.specification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.specification') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDescription')">
      <a-form-item :label="t('entity.material.description')">
        <a-textarea
          v-model:value="advancedQueryForm.materialDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.material.description') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industrySector')">
      <a-form-item :label="t('entity.material.industrysector')">
        <a-input
          v-model:value="advancedQueryForm.industrySector"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.industrysector') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialHierarchy')">
      <a-form-item :label="t('entity.material.hierarchy')">
        <a-input
          v-model:value="advancedQueryForm.materialHierarchy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.hierarchy') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialGroupCode')">
      <a-form-item :label="t('entity.material.groupcode')">
        <a-input
          v-model:value="advancedQueryForm.materialGroupCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.groupcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialType')">
      <a-form-item :label="t('entity.material.type')">
        <a-input-number
          v-model:value="advancedQueryForm.materialType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.type') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialModel')">
      <a-form-item :label="t('entity.material.model')">
        <a-input
          v-model:value="advancedQueryForm.materialModel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.model') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialBrand')">
      <a-form-item :label="t('entity.material.brand')">
        <a-input
          v-model:value="advancedQueryForm.materialBrand"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.brand') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseUnit')">
      <a-form-item :label="t('entity.material.baseunit')">
        <a-input
          v-model:value="advancedQueryForm.baseUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.baseunit') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseGroup')">
      <a-form-item :label="t('entity.material.purchasegroup')">
        <a-input
          v-model:value="advancedQueryForm.purchaseGroup"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.purchasegroup') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseType')">
      <a-form-item :label="t('entity.material.purchasetype')">
        <a-input-number
          v-model:value="advancedQueryForm.purchaseType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.purchasetype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('specialProcurement')">
      <a-form-item :label="t('entity.material.specialprocurement')">
        <a-input-number
          v-model:value="advancedQueryForm.specialProcurement"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.specialprocurement') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBulk')">
      <a-form-item :label="t('entity.material.isbulk')">
        <a-input-number
          v-model:value="advancedQueryForm.isBulk"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.isbulk') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('minOrderQuantity')">
      <a-form-item :label="t('entity.material.minorderquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.minOrderQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.minorderquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('roundingValue')">
      <a-form-item :label="t('entity.material.roundingvalue')">
        <a-input-number
          v-model:value="advancedQueryForm.roundingValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.roundingvalue') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedDeliveryTimeDays')">
      <a-form-item :label="t('entity.material.planneddeliverytimedays')">
        <a-input-number
          v-model:value="advancedQueryForm.plannedDeliveryTimeDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.planneddeliverytimedays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inHouseProductionDays')">
      <a-form-item :label="t('entity.material.inhouseproductiondays')">
        <a-input-number
          v-model:value="advancedQueryForm.inHouseProductionDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.inhouseproductiondays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturer')">
      <a-form-item :label="t('entity.material.manufacturer')">
        <a-input
          v-model:value="advancedQueryForm.manufacturer"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.manufacturer') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerPartNumber')">
      <a-form-item :label="t('entity.material.manufacturerpartnumber')">
        <a-input
          v-model:value="advancedQueryForm.manufacturerPartNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.manufacturerpartnumber') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="t('entity.material.currencycode')">
        <a-input
          v-model:value="advancedQueryForm.currencyCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.currencycode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceControl')">
      <a-form-item :label="t('entity.material.pricecontrol')">
        <a-input-number
          v-model:value="advancedQueryForm.priceControl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.pricecontrol') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceUnit')">
      <a-form-item :label="t('entity.material.priceunit')">
        <a-input-number
          v-model:value="advancedQueryForm.priceUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.priceunit') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('valuationCategory')">
      <a-form-item :label="t('entity.material.valuationcategory')">
        <a-input
          v-model:value="advancedQueryForm.valuationCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.valuationcategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('differenceCode')">
      <a-form-item :label="t('entity.material.differencecode')">
        <a-input
          v-model:value="advancedQueryForm.differenceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.differencecode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('profitCenter')">
      <a-form-item :label="t('entity.material.profitcenter')">
        <a-input
          v-model:value="advancedQueryForm.profitCenter"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.profitcenter') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('latestPurchasePrice')">
      <a-form-item :label="t('entity.material.latestpurchaseprice')">
        <a-input-number
          v-model:value="advancedQueryForm.latestPurchasePrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.latestpurchaseprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPrice')">
      <a-form-item :label="t('entity.material.salesprice')">
        <a-input-number
          v-model:value="advancedQueryForm.salesPrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.salesprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('safetyStock')">
      <a-form-item :label="t('entity.material.safetystock')">
        <a-input-number
          v-model:value="advancedQueryForm.safetyStock"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.safetystock') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maxStock')">
      <a-form-item :label="t('entity.material.maxstock')">
        <a-input-number
          v-model:value="advancedQueryForm.maxStock"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.maxstock') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('minStock')">
      <a-form-item :label="t('entity.material.minstock')">
        <a-input-number
          v-model:value="advancedQueryForm.minStock"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.minstock') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currentStock')">
      <a-form-item :label="t('entity.material.currentstock')">
        <a-input-number
          v-model:value="advancedQueryForm.currentStock"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.currentstock') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionLocation')">
      <a-form-item :label="t('entity.material.productionlocation')">
        <a-input
          v-model:value="advancedQueryForm.productionLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.productionlocation') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchasingLocation')">
      <a-form-item :label="t('entity.material.purchasinglocation')">
        <a-input
          v-model:value="advancedQueryForm.purchasingLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.purchasinglocation') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionRequired')">
      <a-form-item :label="t('entity.material.inspectionrequired')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionRequired"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.inspectionrequired') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBatch')">
      <a-form-item :label="t('entity.material.isbatch')">
        <a-input-number
          v-model:value="advancedQueryForm.isBatch"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.isbatch') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isExpiry')">
      <a-form-item :label="t('entity.material.isexpiry')">
        <a-input-number
          v-model:value="advancedQueryForm.isExpiry"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.isexpiry') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDays')">
      <a-form-item :label="t('entity.material.expirydays')">
        <a-input-number
          v-model:value="advancedQueryForm.expiryDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.expirydays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialStatus')">
      <a-form-item :label="t('entity.material.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialAttributes')">
      <a-form-item :label="t('entity.material.attributes')">
        <a-input
          v-model:value="advancedQueryForm.materialAttributes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.attributes') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEndOfLife')">
      <a-form-item :label="t('entity.material.isendoflife')">
        <a-input
          v-model:value="advancedQueryForm.isEndOfLife"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.isendoflife') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endOfLifeDateStart')">
      <a-form-item :label="t('entity.material.endoflifedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.endOfLifeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.endoflifedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endOfLifeDateEnd')">
      <a-form-item :label="t('entity.material.endoflifedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.endOfLifeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.endoflifedateend') })"
          value-format="YYYY-MM-DD"
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
      :title="t('common.dialog.title.import', { entity: t('entity.material._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.material._self"
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
      :id-column-key="'materialId'"
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
 * Takt物料实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/statistics/report/logistics/material
 */
import { ref, computed, onMounted } from 'vue'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/takt-action-column/index'
import { useI18n } from 'vue-i18n'
import MaterialForm from './components/material-form.vue'
import { getMaterialList, getMaterialById, createMaterial, updateMaterial, deleteMaterialById, deleteMaterialBatch, getMaterialTemplate, importMaterial, exportMaterial } from '@/api/logistics/materials/material'
import type { Material, MaterialQuery, MaterialCreate, MaterialUpdate } from '@/types/logistics/materials/material'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/export-download-name'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterial')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.material._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Material[]>([])
/** 当前页码 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = ref(20)
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Material | null>(null)
/** 表格多选行 */
const selectedRows = ref<Material[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Material>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  materialDescription: '',
  industrySector: '',
  materialHierarchy: '',
  materialGroupCode: '',
  materialType: undefined as number | undefined,
  materialModel: '',
  materialBrand: '',
  baseUnit: '',
  purchaseGroup: '',
  purchaseType: undefined as number | undefined,
  specialProcurement: undefined as number | undefined,
  isBulk: undefined as number | undefined,
  minOrderQuantity: undefined as number | undefined,
  roundingValue: undefined as number | undefined,
  plannedDeliveryTimeDays: undefined as number | undefined,
  inHouseProductionDays: undefined as number | undefined,
  manufacturer: '',
  manufacturerPartNumber: '',
  currencyCode: '',
  priceControl: undefined as number | undefined,
  priceUnit: undefined as number | undefined,
  valuationCategory: '',
  differenceCode: '',
  profitCenter: '',
  latestPurchasePrice: undefined as number | undefined,
  salesPrice: undefined as number | undefined,
  safetyStock: undefined as number | undefined,
  maxStock: undefined as number | undefined,
  minStock: undefined as number | undefined,
  currentStock: undefined as number | undefined,
  productionLocation: '',
  purchasingLocation: '',
  inspectionRequired: undefined as number | undefined,
  isBatch: undefined as number | undefined,
  isExpiry: undefined as number | undefined,
  expiryDays: undefined as number | undefined,
  materialStatus: undefined as number | undefined,
  materialAttributes: '',
  isEndOfLife: '',
  endOfLifeDateStart: '',
  endOfLifeDateEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extFieldJson: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('entity.material.plantcode') },
  { key: 'materialCode', label: t('entity.material.code') },
  { key: 'materialName', label: t('entity.material.name') },
  { key: 'materialSpecification', label: t('entity.material.specification') },
  { key: 'materialDescription', label: t('entity.material.description') },
  { key: 'industrySector', label: t('entity.material.industrysector') },
  { key: 'materialHierarchy', label: t('entity.material.hierarchy') },
  { key: 'materialGroupCode', label: t('entity.material.groupcode') },
  { key: 'materialType', label: t('entity.material.type') },
  { key: 'materialModel', label: t('entity.material.model') },
  { key: 'materialBrand', label: t('entity.material.brand') },
  { key: 'baseUnit', label: t('entity.material.baseunit') },
  { key: 'purchaseGroup', label: t('entity.material.purchasegroup') },
  { key: 'purchaseType', label: t('entity.material.purchasetype') },
  { key: 'specialProcurement', label: t('entity.material.specialprocurement') },
  { key: 'isBulk', label: t('entity.material.isbulk') },
  { key: 'minOrderQuantity', label: t('entity.material.minorderquantity') },
  { key: 'roundingValue', label: t('entity.material.roundingvalue') },
  { key: 'plannedDeliveryTimeDays', label: t('entity.material.planneddeliverytimedays') },
  { key: 'inHouseProductionDays', label: t('entity.material.inhouseproductiondays') },
  { key: 'manufacturer', label: t('entity.material.manufacturer') },
  { key: 'manufacturerPartNumber', label: t('entity.material.manufacturerpartnumber') },
  { key: 'currencyCode', label: t('entity.material.currencycode') },
  { key: 'priceControl', label: t('entity.material.pricecontrol') },
  { key: 'priceUnit', label: t('entity.material.priceunit') },
  { key: 'valuationCategory', label: t('entity.material.valuationcategory') },
  { key: 'differenceCode', label: t('entity.material.differencecode') },
  { key: 'profitCenter', label: t('entity.material.profitcenter') },
  { key: 'latestPurchasePrice', label: t('entity.material.latestpurchaseprice') },
  { key: 'salesPrice', label: t('entity.material.salesprice') },
  { key: 'safetyStock', label: t('entity.material.safetystock') },
  { key: 'maxStock', label: t('entity.material.maxstock') },
  { key: 'minStock', label: t('entity.material.minstock') },
  { key: 'currentStock', label: t('entity.material.currentstock') },
  { key: 'productionLocation', label: t('entity.material.productionlocation') },
  { key: 'purchasingLocation', label: t('entity.material.purchasinglocation') },
  { key: 'inspectionRequired', label: t('entity.material.inspectionrequired') },
  { key: 'isBatch', label: t('entity.material.isbatch') },
  { key: 'isExpiry', label: t('entity.material.isexpiry') },
  { key: 'expiryDays', label: t('entity.material.expirydays') },
  { key: 'materialStatus', label: t('entity.material.status') },
  { key: 'materialAttributes', label: t('entity.material.attributes') },
  { key: 'isEndOfLife', label: t('entity.material.isendoflife') },
  { key: 'endOfLifeDateStart', label: t('entity.material.endoflifedatestart') },
  { key: 'endOfLifeDateEnd', label: t('entity.material.endoflifedateend') },
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
const entityIdName = 'materialId'
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
    dataIndex: 'materialId',
    key: 'materialId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialId') ?? ''
  },
  {
    title: t('entity.material.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.material.code'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.material.name'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialName') ?? ''
  },
  {
    title: t('entity.material.specification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialSpecification') ?? ''
  },
  {
    title: t('entity.material.description'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialDescription') ?? ''
  },
  {
    title: t('entity.material.industrysector'),
    dataIndex: 'industrySector',
    key: 'industrySector',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'industrySector') ?? ''
  },
  {
    title: t('entity.material.hierarchy'),
    dataIndex: 'materialHierarchy',
    key: 'materialHierarchy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialHierarchy') ?? ''
  },
  {
    title: t('entity.material.groupcode'),
    dataIndex: 'materialGroupCode',
    key: 'materialGroupCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialGroupCode') ?? ''
  },
  {
    title: t('entity.material.type'),
    dataIndex: 'materialType',
    key: 'materialType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialType') ?? ''
  },
  {
    title: t('entity.material.model'),
    dataIndex: 'materialModel',
    key: 'materialModel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialModel') ?? ''
  },
  {
    title: t('entity.material.brand'),
    dataIndex: 'materialBrand',
    key: 'materialBrand',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialBrand') ?? ''
  },
  {
    title: t('entity.material.baseunit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'baseUnit') ?? ''
  },
  {
    title: t('entity.material.purchasegroup'),
    dataIndex: 'purchaseGroup',
    key: 'purchaseGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'purchaseGroup') ?? ''
  },
  {
    title: t('entity.material.purchasetype'),
    dataIndex: 'purchaseType',
    key: 'purchaseType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'purchaseType') ?? ''
  },
  {
    title: t('entity.material.specialprocurement'),
    dataIndex: 'specialProcurement',
    key: 'specialProcurement',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'specialProcurement') ?? ''
  },
  {
    title: t('entity.material.isbulk'),
    dataIndex: 'isBulk',
    key: 'isBulk',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'isBulk') ?? ''
  },
  {
    title: t('entity.material.minorderquantity'),
    dataIndex: 'minOrderQuantity',
    key: 'minOrderQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'minOrderQuantity') ?? ''
  },
  {
    title: t('entity.material.roundingvalue'),
    dataIndex: 'roundingValue',
    key: 'roundingValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'roundingValue') ?? ''
  },
  {
    title: t('entity.material.planneddeliverytimedays'),
    dataIndex: 'plannedDeliveryTimeDays',
    key: 'plannedDeliveryTimeDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'plannedDeliveryTimeDays') ?? ''
  },
  {
    title: t('entity.material.inhouseproductiondays'),
    dataIndex: 'inHouseProductionDays',
    key: 'inHouseProductionDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'inHouseProductionDays') ?? ''
  },
  {
    title: t('entity.material.manufacturer'),
    dataIndex: 'manufacturer',
    key: 'manufacturer',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'manufacturer') ?? ''
  },
  {
    title: t('entity.material.manufacturerpartnumber'),
    dataIndex: 'manufacturerPartNumber',
    key: 'manufacturerPartNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'manufacturerPartNumber') ?? ''
  },
  {
    title: t('entity.material.currencycode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'currencyCode') ?? ''
  },
  {
    title: t('entity.material.pricecontrol'),
    dataIndex: 'priceControl',
    key: 'priceControl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'priceControl') ?? ''
  },
  {
    title: t('entity.material.priceunit'),
    dataIndex: 'priceUnit',
    key: 'priceUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'priceUnit') ?? ''
  },
  {
    title: t('entity.material.valuationcategory'),
    dataIndex: 'valuationCategory',
    key: 'valuationCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'valuationCategory') ?? ''
  },
  {
    title: t('entity.material.differencecode'),
    dataIndex: 'differenceCode',
    key: 'differenceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'differenceCode') ?? ''
  },
  {
    title: t('entity.material.profitcenter'),
    dataIndex: 'profitCenter',
    key: 'profitCenter',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'profitCenter') ?? ''
  },
  {
    title: t('entity.material.latestpurchaseprice'),
    dataIndex: 'latestPurchasePrice',
    key: 'latestPurchasePrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'latestPurchasePrice') ?? ''
  },
  {
    title: t('entity.material.salesprice'),
    dataIndex: 'salesPrice',
    key: 'salesPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'salesPrice') ?? ''
  },
  {
    title: t('entity.material.safetystock'),
    dataIndex: 'safetyStock',
    key: 'safetyStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'safetyStock') ?? ''
  },
  {
    title: t('entity.material.maxstock'),
    dataIndex: 'maxStock',
    key: 'maxStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'maxStock') ?? ''
  },
  {
    title: t('entity.material.minstock'),
    dataIndex: 'minStock',
    key: 'minStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'minStock') ?? ''
  },
  {
    title: t('entity.material.currentstock'),
    dataIndex: 'currentStock',
    key: 'currentStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'currentStock') ?? ''
  },
  {
    title: t('entity.material.productionlocation'),
    dataIndex: 'productionLocation',
    key: 'productionLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'productionLocation') ?? ''
  },
  {
    title: t('entity.material.purchasinglocation'),
    dataIndex: 'purchasingLocation',
    key: 'purchasingLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'purchasingLocation') ?? ''
  },
  {
    title: t('entity.material.inspectionrequired'),
    dataIndex: 'inspectionRequired',
    key: 'inspectionRequired',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'inspectionRequired') ?? ''
  },
  {
    title: t('entity.material.isbatch'),
    dataIndex: 'isBatch',
    key: 'isBatch',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'isBatch') ?? ''
  },
  {
    title: t('entity.material.isexpiry'),
    dataIndex: 'isExpiry',
    key: 'isExpiry',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'isExpiry') ?? ''
  },
  {
    title: t('entity.material.expirydays'),
    dataIndex: 'expiryDays',
    key: 'expiryDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'expiryDays') ?? ''
  },
  {
    title: t('entity.material.status'),
    dataIndex: 'materialStatus',
    key: 'materialStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.material.attributes'),
    dataIndex: 'materialAttributes',
    key: 'materialAttributes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'materialAttributes') ?? ''
  },
  {
    title: t('entity.material.isendoflife'),
    dataIndex: 'isEndOfLife',
    key: 'isEndOfLife',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'isEndOfLife') ?? ''
  },
  {
    title: t('entity.material.endoflifedate'),
    dataIndex: 'endOfLifeDate',
    key: 'endOfLifeDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaterialField(record, 'endOfLifeDate') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'statistics:report:logistics:material:update',
        onClick: (record: Material) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:report:logistics:material:delete',
        onClick: (record: Material) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMaterialId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMaterialField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Material[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Material, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getMaterialId(selectedRow.value) === getMaterialId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Material[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Material) => ({
  onClick: () => {
    const key = getMaterialId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getMaterialId(item)))
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
    const params: MaterialQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getMaterialList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Material] 加载数据失败', { error })
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
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  materialDescription: '',
  industrySector: '',
  materialHierarchy: '',
  materialGroupCode: '',
  materialType: undefined as number | undefined,
  materialModel: '',
  materialBrand: '',
  baseUnit: '',
  purchaseGroup: '',
  purchaseType: undefined as number | undefined,
  specialProcurement: undefined as number | undefined,
  isBulk: undefined as number | undefined,
  minOrderQuantity: undefined as number | undefined,
  roundingValue: undefined as number | undefined,
  plannedDeliveryTimeDays: undefined as number | undefined,
  inHouseProductionDays: undefined as number | undefined,
  manufacturer: '',
  manufacturerPartNumber: '',
  currencyCode: '',
  priceControl: undefined as number | undefined,
  priceUnit: undefined as number | undefined,
  valuationCategory: '',
  differenceCode: '',
  profitCenter: '',
  latestPurchasePrice: undefined as number | undefined,
  salesPrice: undefined as number | undefined,
  safetyStock: undefined as number | undefined,
  maxStock: undefined as number | undefined,
  minStock: undefined as number | undefined,
  currentStock: undefined as number | undefined,
  productionLocation: '',
  purchasingLocation: '',
  inspectionRequired: undefined as number | undefined,
  isBatch: undefined as number | undefined,
  isExpiry: undefined as number | undefined,
  expiryDays: undefined as number | undefined,
  materialStatus: undefined as number | undefined,
  materialAttributes: '',
  isEndOfLife: '',
  endOfLifeDateStart: '',
  endOfLifeDateEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.material._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗 */
function handleEdit(record: Material) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.material._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.material._self') }))
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
      await updateMaterial(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.material._self') }))
    } else {
      await createMaterial(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.material._self') }))
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
  const res = await getMaterialTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaterial(file, sheetName)
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
    const exportQuery: MaterialQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportMaterial(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.material._self') }))
  } catch (error: any) {
    logger.error('[Material] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.material._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Material) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.material._self'), name: t('common.tip.this.target', { target: t('entity.material._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.material._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.material._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.material._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMaterialBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.material._self') }))
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
  materialCode: '',
  materialName: '',
  materialSpecification: '',
  materialDescription: '',
  industrySector: '',
  materialHierarchy: '',
  materialGroupCode: '',
  materialType: undefined as number | undefined,
  materialModel: '',
  materialBrand: '',
  baseUnit: '',
  purchaseGroup: '',
  purchaseType: undefined as number | undefined,
  specialProcurement: undefined as number | undefined,
  isBulk: undefined as number | undefined,
  minOrderQuantity: undefined as number | undefined,
  roundingValue: undefined as number | undefined,
  plannedDeliveryTimeDays: undefined as number | undefined,
  inHouseProductionDays: undefined as number | undefined,
  manufacturer: '',
  manufacturerPartNumber: '',
  currencyCode: '',
  priceControl: undefined as number | undefined,
  priceUnit: undefined as number | undefined,
  valuationCategory: '',
  differenceCode: '',
  profitCenter: '',
  latestPurchasePrice: undefined as number | undefined,
  salesPrice: undefined as number | undefined,
  safetyStock: undefined as number | undefined,
  maxStock: undefined as number | undefined,
  minStock: undefined as number | undefined,
  currentStock: undefined as number | undefined,
  productionLocation: '',
  purchasingLocation: '',
  inspectionRequired: undefined as number | undefined,
  isBatch: undefined as number | undefined,
  isExpiry: undefined as number | undefined,
  expiryDays: undefined as number | undefined,
  materialStatus: undefined as number | undefined,
  materialAttributes: '',
  isEndOfLife: '',
  endOfLifeDateStart: '',
  endOfLifeDateEnd: '',
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
.statistics-report-logistics-material {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>
