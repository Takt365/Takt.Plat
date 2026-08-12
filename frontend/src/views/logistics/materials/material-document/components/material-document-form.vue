<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-document/components -->
<!-- 文件名称：material-document-form.vue -->
<!-- 功能描述：Takt物料凭证主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-document-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-document-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
              <a-col :span="12">
                <a-form-item
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
                  />
                </a-form-item>
              </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('extField') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="materialDocumentItemTableRef"
      v-model="childMaterialDocumentItemRows"
      :columns="materialDocumentItemFormColumns"
      :title="materialDocumentItemPi.self()"
      :add-button-entity="materialDocumentItemPi.self()"
      id-field="materialDocumentItemId"
      :default-row="createDefaultMaterialDocumentItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-movementType="{ record }">
        <TaktSelect
          v-model:value="record.movementType"
          dict-type="logistics_movement_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('movementType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-plantCode="{ record }">
        <TaktSelect
          v-model:value="record.plantCode"
          api-url="TaktPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('plantCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-warehouseCode="{ record }">
        <TaktSelect
          v-model:value="record.warehouseCode"
          api-url="TaktWarehouses/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('warehouseCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-stockType="{ record }">
        <TaktSelect
          v-model:value="record.stockType"
          dict-type="logistics_stock_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('stockType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-specialStock="{ record }">
        <TaktSelect
          v-model:value="record.specialStock"
          dict-type="logistics_special_stock_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('specialStock')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-supplierCode="{ record }">
        <TaktSelect
          v-model:value="record.supplierCode"
          api-url="TaktSuppliers/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('supplierCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-customerCode="{ record }">
        <TaktSelect
          v-model:value="record.customerCode"
          api-url="TaktCustomers/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('customerCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-currencyCode="{ record }">
        <TaktSelect
          v-model:value="record.currencyCode"
          dict-type="accounting_currency_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('currencyCode')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-baseUnit="{ record }">
        <TaktSelect
          v-model:value="record.baseUnit"
          dict-type="logistics_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('baseUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-profitCenterCode="{ record }">
        <TaktSelect
          v-model:value="record.profitCenterCode"
          api-url="TaktProfitCenters/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('profitCenterCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-postedBy="{ record }">
        <TaktSelect
          v-model:value="record.postedBy"
          api-url="TaktEmployees/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.queryPh('postedBy', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialDocumentItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt物料凭证主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-document/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaterialDocumentI18n } from '../composables/use-material-document-i18n'

/** 实体字段 i18n */
const pi = useMaterialDocumentI18n()

import type { MaterialDocumentCreate } from '@/types/logistics/materials/material-document'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","materialDocumentCode","materialDocumentYear","transactionEventType","documentType","revaluationType","documentDate","postingDate","referenceCode","headerText","billOfLadingCode","deliveryCode","transactionCode","postedBy","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useMaterialDocumentItemI18n } from '../composables/use-material-document-item-i18n'

const materialDocumentItemPi = useMaterialDocumentItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childMaterialDocumentItemRows = ref<Record<string, unknown>[]>([])
const materialDocumentItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedMaterialDocumentItemRow(row: Record<string, unknown>): boolean {
  const id = row.materialDocumentItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextMaterialDocumentItemLineNumber(): number {
  const rows = materialDocumentItemTableRef.value?.getRows?.() ?? childMaterialDocumentItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 materialDocumentItem 可编辑列 */
const materialDocumentItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: materialDocumentItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'lineId',
    title: materialDocumentItemPi.label('lineId'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('lineId'),
  },
  {
    key: 'parentLineId',
    title: materialDocumentItemPi.label('parentLineId'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('parentLineId'),
  },
  {
    key: 'lineDepth',
    title: materialDocumentItemPi.label('lineDepth'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('lineDepth'),
  },
  {
    key: 'movementType',
    title: materialDocumentItemPi.label('movementType'),
    width: 140,
  },
  {
    key: 'autoCreatedFlag',
    title: materialDocumentItemPi.label('autoCreatedFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('autoCreatedFlag'),
  },
  {
    key: 'materialCode',
    title: materialDocumentItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'plantCode',
    title: materialDocumentItemPi.label('plantCode'),
    width: 140,
  },
  {
    key: 'warehouseCode',
    title: materialDocumentItemPi.label('warehouseCode'),
    width: 140,
  },
  {
    key: 'batchCode',
    title: materialDocumentItemPi.label('batchCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('batchCode'),
  },
  {
    key: 'stockType',
    title: materialDocumentItemPi.label('stockType'),
    width: 140,
  },
  {
    key: 'restrictedStockFlag',
    title: materialDocumentItemPi.label('restrictedStockFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('restrictedStockFlag'),
  },
  {
    key: 'specialStock',
    title: materialDocumentItemPi.label('specialStock'),
    width: 140,
  },
  {
    key: 'supplierCode',
    title: materialDocumentItemPi.label('supplierCode'),
    width: 140,
  },
  {
    key: 'customerCode',
    title: materialDocumentItemPi.label('customerCode'),
    width: 140,
  },
  {
    key: 'debitCreditIndicator',
    title: materialDocumentItemPi.label('debitCreditIndicator'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('debitCreditIndicator'),
  },
  {
    key: 'currencyCode',
    title: materialDocumentItemPi.label('currencyCode'),
    width: 140,
  },
  {
    key: 'localCurrencyAmount',
    title: materialDocumentItemPi.label('localCurrencyAmount'),
    width: 140,
  },
  {
    key: 'alternativeAmount',
    title: materialDocumentItemPi.label('alternativeAmount'),
    width: 140,
  },
  {
    key: 'quantity',
    title: materialDocumentItemPi.label('quantity'),
    width: 140,
  },
  {
    key: 'baseUnit',
    title: materialDocumentItemPi.label('baseUnit'),
    width: 140,
  },
  {
    key: 'entryQuantity',
    title: materialDocumentItemPi.label('entryQuantity'),
    width: 140,
  },
  {
    key: 'entryUnit',
    title: materialDocumentItemPi.label('entryUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('entryUnit'),
  },
  {
    key: 'poPriceQuantity',
    title: materialDocumentItemPi.label('poPriceQuantity'),
    width: 140,
  },
  {
    key: 'poPriceUnit',
    title: materialDocumentItemPi.label('poPriceUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('poPriceUnit'),
  },
  {
    key: 'purchaseOrderCode',
    title: materialDocumentItemPi.label('purchaseOrderCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('purchaseOrderCode'),
  },
  {
    key: 'purchaseOrderItem',
    title: materialDocumentItemPi.label('purchaseOrderItem'),
    width: 140,
  },
  {
    key: 'referenceDocumentYear',
    title: materialDocumentItemPi.label('referenceDocumentYear'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('referenceDocumentYear'),
  },
  {
    key: 'referenceDocumentCode',
    title: materialDocumentItemPi.label('referenceDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('referenceDocumentCode'),
  },
  {
    key: 'referenceDocumentItem',
    title: materialDocumentItemPi.label('referenceDocumentItem'),
    width: 140,
  },
  {
    key: 'originalMaterialDocumentYear',
    title: materialDocumentItemPi.label('originalMaterialDocumentYear'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('originalMaterialDocumentYear'),
  },
  {
    key: 'originalMaterialDocumentCode',
    title: materialDocumentItemPi.label('originalMaterialDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('originalMaterialDocumentCode'),
  },
  {
    key: 'originalLineNumber',
    title: materialDocumentItemPi.label('originalLineNumber'),
    width: 140,
  },
  {
    key: 'deliveryCompletedFlag',
    title: materialDocumentItemPi.label('deliveryCompletedFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('deliveryCompletedFlag'),
  },
  {
    key: 'itemText',
    title: materialDocumentItemPi.label('itemText'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('itemText'),
  },
  {
    key: 'equipmentCode',
    title: materialDocumentItemPi.label('equipmentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('equipmentCode'),
  },
  {
    key: 'goodsRecipient',
    title: materialDocumentItemPi.label('goodsRecipient'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('goodsRecipient'),
  },
  {
    key: 'unloadingPoint',
    title: materialDocumentItemPi.label('unloadingPoint'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('unloadingPoint'),
  },
  {
    key: 'businessAreaCode',
    title: materialDocumentItemPi.label('businessAreaCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('businessAreaCode'),
  },
  {
    key: 'controllingAreaCode',
    title: materialDocumentItemPi.label('controllingAreaCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('controllingAreaCode'),
  },
  {
    key: 'tradingPartnerBusinessArea',
    title: materialDocumentItemPi.label('tradingPartnerBusinessArea'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('tradingPartnerBusinessArea'),
  },
  {
    key: 'productionOrderCode',
    title: materialDocumentItemPi.label('productionOrderCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('productionOrderCode'),
  },
  {
    key: 'assetCode',
    title: materialDocumentItemPi.label('assetCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('assetCode'),
  },
  {
    key: 'assetSubCode',
    title: materialDocumentItemPi.label('assetSubCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('assetSubCode'),
  },
  {
    key: 'fiscalYear',
    title: materialDocumentItemPi.label('fiscalYear'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('fiscalYear'),
  },
  {
    key: 'postToPreviousPeriodFlag',
    title: materialDocumentItemPi.label('postToPreviousPeriodFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('postToPreviousPeriodFlag'),
  },
  {
    key: 'postToPreviousYearFlag',
    title: materialDocumentItemPi.label('postToPreviousYearFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('postToPreviousYearFlag'),
  },
  {
    key: 'accountingDocumentCode',
    title: materialDocumentItemPi.label('accountingDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('accountingDocumentCode'),
  },
  {
    key: 'accountingDocumentItem',
    title: materialDocumentItemPi.label('accountingDocumentItem'),
    width: 140,
  },
  {
    key: 'revaluationDocumentCode',
    title: materialDocumentItemPi.label('revaluationDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('revaluationDocumentCode'),
  },
  {
    key: 'revaluationDocumentItem',
    title: materialDocumentItemPi.label('revaluationDocumentItem'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('revaluationDocumentItem'),
  },
  {
    key: 'reservationCode',
    title: materialDocumentItemPi.label('reservationCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('reservationCode'),
  },
  {
    key: 'reservationItem',
    title: materialDocumentItemPi.label('reservationItem'),
    width: 140,
  },
  {
    key: 'finalIssueFlag',
    title: materialDocumentItemPi.label('finalIssueFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('finalIssueFlag'),
  },
  {
    key: 'reservationQuantity',
    title: materialDocumentItemPi.label('reservationQuantity'),
    width: 140,
  },
  {
    key: 'receivingMaterialCode',
    title: materialDocumentItemPi.label('receivingMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('receivingMaterialCode'),
  },
  {
    key: 'receivingPlantCode',
    title: materialDocumentItemPi.label('receivingPlantCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('receivingPlantCode'),
  },
  {
    key: 'receivingWarehouseCode',
    title: materialDocumentItemPi.label('receivingWarehouseCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('receivingWarehouseCode'),
  },
  {
    key: 'profitCenterCode',
    title: materialDocumentItemPi.label('profitCenterCode'),
    width: 140,
  },
  {
    key: 'valuatedStockQuantity',
    title: materialDocumentItemPi.label('valuatedStockQuantity'),
    width: 140,
  },
  {
    key: 'totalValuatedStockValue',
    title: materialDocumentItemPi.label('totalValuatedStockValue'),
    width: 140,
  },
  {
    key: 'priceControl',
    title: materialDocumentItemPi.label('priceControl'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('priceControl'),
  },
  {
    key: 'manufacturerPartMaterialCode',
    title: materialDocumentItemPi.label('manufacturerPartMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('manufacturerPartMaterialCode'),
  },
  {
    key: 'mkpfReferenceCode',
    title: materialDocumentItemPi.label('mkpfReferenceCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('mkpfReferenceCode'),
  },
  {
    key: 'imDeliveryCode',
    title: materialDocumentItemPi.label('imDeliveryCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialDocumentItemPi.ph('imDeliveryCode'),
  },
  {
    key: 'imDeliveryItem',
    title: materialDocumentItemPi.label('imDeliveryItem'),
    width: 140,
  },
  {
    key: 'postedBy',
    title: materialDocumentItemPi.label('postedBy'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: materialDocumentItemPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaterialDocumentCreate & { materialDocumentId?: string }> | null | undefined) {
  const rows_materialDocumentItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childMaterialDocumentItemRows.value = rows_materialDocumentItem
}

function createDefaultMaterialDocumentItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextMaterialDocumentItemLineNumber(),
    lineId: '',
    parentLineId: '',
    lineDepth: '',
    movementType: '',
    autoCreatedFlag: '',
    materialCode: '',
    plantCode: '',
    warehouseCode: '',
    batchCode: '',
    stockType: '',
    restrictedStockFlag: '',
    specialStock: '',
    supplierCode: '',
    customerCode: '',
    debitCreditIndicator: '',
    currencyCode: '',
    localCurrencyAmount: 0,
    alternativeAmount: 0,
    quantity: 0,
    baseUnit: '',
    entryQuantity: 0,
    entryUnit: '',
    poPriceQuantity: 0,
    poPriceUnit: '',
    purchaseOrderCode: '',
    purchaseOrderItem: 0,
    referenceDocumentYear: '',
    referenceDocumentCode: '',
    referenceDocumentItem: 0,
    originalMaterialDocumentYear: '',
    originalMaterialDocumentCode: '',
    originalLineNumber: 0,
    deliveryCompletedFlag: '',
    itemText: '',
    equipmentCode: '',
    goodsRecipient: '',
    unloadingPoint: '',
    businessAreaCode: '',
    controllingAreaCode: '',
    tradingPartnerBusinessArea: '',
    productionOrderCode: '',
    assetCode: '',
    assetSubCode: '',
    fiscalYear: '',
    postToPreviousPeriodFlag: '',
    postToPreviousYearFlag: '',
    accountingDocumentCode: '',
    accountingDocumentItem: 0,
    revaluationDocumentCode: '',
    revaluationDocumentItem: '',
    reservationCode: '',
    reservationItem: 0,
    finalIssueFlag: '',
    reservationQuantity: 0,
    receivingMaterialCode: '',
    receivingPlantCode: '',
    receivingWarehouseCode: '',
    profitCenterCode: '',
    valuatedStockQuantity: 0,
    totalValuatedStockValue: 0,
    priceControl: '',
    manufacturerPartMaterialCode: '',
    mkpfReferenceCode: '',
    imDeliveryCode: '',
    imDeliveryItem: 0,
    postedBy: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.materialDocumentId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: materialDocumentItemTableRef.value?.getRows?.() ?? childMaterialDocumentItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        materialDocumentId: masterId,
      }
      if (isUpdate && isPersistedMaterialDocumentItemRow(row)) {
        normalized.materialDocumentItemId = row.materialDocumentItemId
      } else {
        delete normalized.materialDocumentItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaterialDocumentCreate & { materialDocumentId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 materialDocumentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.materialDocumentId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.materialDocumentId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  materialDocumentCode: [
    {
      required: true,
      message: pi.ph('materialDocumentCode'),
      trigger: 'blur'
    }
  ],
  materialDocumentYear: [
    {
      required: true,
      message: pi.ph('materialDocumentYear'),
      trigger: 'blur'
    }
  ],
  documentDate: [
    {
      required: true,
      message: pi.ph('documentDate'),
      trigger: 'change'
    }
  ],
  postingDate: [
    {
      required: true,
      message: pi.ph('postingDate'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await materialDocumentItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.materialDocumentId)
  childMaterialDocumentItemRows.value = []
  materialDocumentItemTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
