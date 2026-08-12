<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-invoice/components -->
<!-- 文件名称：invoice-item-form.vue -->
<!-- 功能描述：Takt销售发票主表实体子表 salesInvoiceItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form invoice-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="invoice-item-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('billingDocumentCode')"
                name="billingDocumentCode"
              >
                <a-input
                  v-model:value="formState.billingDocumentCode"
                  :placeholder="pi.ph('billingDocumentCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lineNumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="pi.ph('lineNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('billingQuantity')"
                name="billingQuantity"
              >
                <a-input-number
                  v-model:value="formState.billingQuantity"
                  :placeholder="pi.ph('billingQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesUnit')"
                name="salesUnit"
              >
                <a-input
                  v-model:value="formState.salesUnit"
                  :placeholder="pi.ph('salesUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('baseUnit')"
                name="baseUnit"
              >
                <a-input
                  v-model:value="formState.baseUnit"
                  :placeholder="pi.ph('baseUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleQuantity')"
                name="scaleQuantity"
              >
                <a-input-number
                  v-model:value="formState.scaleQuantity"
                  :placeholder="pi.ph('scaleQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('billingQuantitySku')"
                name="billingQuantitySku"
              >
                <a-input-number
                  v-model:value="formState.billingQuantitySku"
                  :placeholder="pi.ph('billingQuantitySku')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('netWeight')"
                name="netWeight"
              >
                <a-input-number
                  v-model:value="formState.netWeight"
                  :placeholder="pi.ph('netWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grossWeight')"
                name="grossWeight"
              >
                <a-input-number
                  v-model:value="formState.grossWeight"
                  :placeholder="pi.ph('grossWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('weightUnit')"
                name="weightUnit"
              >
                <a-input
                  v-model:value="formState.weightUnit"
                  :placeholder="pi.ph('weightUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessAreaCode')"
                name="businessAreaCode"
              >
                <a-input
                  v-model:value="formState.businessAreaCode"
                  :placeholder="pi.ph('businessAreaCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingDate')"
                name="pricingDate"
              >
                <a-date-picker
                  v-model:value="formState.pricingDate"
                  :placeholder="pi.ph('pricingDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceRenderedDate')"
                name="serviceRenderedDate"
              >
                <a-date-picker
                  v-model:value="formState.serviceRenderedDate"
                  :placeholder="pi.ph('serviceRenderedDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingExchangeRate')"
                name="pricingExchangeRate"
              >
                <a-input-number
                  v-model:value="formState.pricingExchangeRate"
                  :placeholder="pi.ph('pricingExchangeRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('netAmount')"
                name="netAmount"
              >
                <a-input-number
                  v-model:value="formState.netAmount"
                  :placeholder="pi.ph('netAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentCode')"
                name="referenceDocumentCode"
              >
                <a-input
                  v-model:value="formState.referenceDocumentCode"
                  :placeholder="pi.ph('referenceDocumentCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentItem')"
                name="referenceDocumentItem"
              >
                <a-input-number
                  v-model:value="formState.referenceDocumentItem"
                  :placeholder="pi.ph('referenceDocumentItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentCategory')"
                name="referenceDocumentCategory"
              >
                <a-input
                  v-model:value="formState.referenceDocumentCategory"
                  :placeholder="pi.ph('referenceDocumentCategory')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesDocumentCode')"
                name="salesDocumentCode"
              >
                <a-input
                  v-model:value="formState.salesDocumentCode"
                  :placeholder="pi.ph('salesDocumentCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesDocumentItem')"
                name="salesDocumentItem"
              >
                <a-input-number
                  v-model:value="formState.salesDocumentItem"
                  :placeholder="pi.ph('salesDocumentItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesDocumentReferenceFlag')"
                name="salesDocumentReferenceFlag"
              >
                <a-input
                  v-model:value="formState.salesDocumentReferenceFlag"
                  :placeholder="pi.ph('salesDocumentReferenceFlag')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialDescription')"
                name="materialDescription"
              >
                <a-textarea
                  v-model:value="formState.materialDescription"
                  :placeholder="pi.ph('materialDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingReferenceMaterialCode')"
                name="pricingReferenceMaterialCode"
              >
                <a-input
                  v-model:value="formState.pricingReferenceMaterialCode"
                  :placeholder="pi.ph('pricingReferenceMaterialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchCode')"
                name="batchCode"
              >
                <a-input
                  v-model:value="formState.batchCode"
                  :placeholder="pi.ph('batchCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialGroup')"
                name="materialGroup"
              >
                <a-input
                  v-model:value="formState.materialGroup"
                  :placeholder="pi.ph('materialGroup')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesItemCategory')"
                name="salesItemCategory"
              >
                <a-input
                  v-model:value="formState.salesItemCategory"
                  :placeholder="pi.ph('salesItemCategory')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productHierarchy')"
                name="productHierarchy"
              >
                <a-input
                  v-model:value="formState.productHierarchy"
                  :placeholder="pi.ph('productHierarchy')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shippingPoint')"
                name="shippingPoint"
              >
                <a-input
                  v-model:value="formState.shippingPoint"
                  :placeholder="pi.ph('shippingPoint')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('division')"
                name="division"
              >
                <a-input
                  v-model:value="formState.division"
                  :placeholder="pi.ph('division')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('partnerItem')"
                name="partnerItem"
              >
                <a-input-number
                  v-model:value="formState.partnerItem"
                  :placeholder="pi.ph('partnerItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('departureCountry')"
                name="departureCountry"
              >
                <TaktSelect
                  v-model:value="formState.departureCountry"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('departureCountry')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantRegion')"
                name="plantRegion"
              >
                <a-input
                  v-model:value="formState.plantRegion"
                  :placeholder="pi.ph('plantRegion')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingFlag')"
                name="pricingFlag"
              >
                <a-input
                  v-model:value="formState.pricingFlag"
                  :placeholder="pi.ph('pricingFlag')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('warehouseCode')"
                name="warehouseCode"
              >
                <TaktSelect
                  v-model:value="formState.warehouseCode"
                  api-url="TaktWarehouses/options"
                  :placeholder="pi.ph('warehouseCode')"
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costAmount')"
                name="costAmount"
              >
                <a-input-number
                  v-model:value="formState.costAmount"
                  :placeholder="pi.ph('costAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal1')"
                name="subtotal1"
              >
                <a-input-number
                  v-model:value="formState.subtotal1"
                  :placeholder="pi.ph('subtotal1')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal2')"
                name="subtotal2"
              >
                <a-input-number
                  v-model:value="formState.subtotal2"
                  :placeholder="pi.ph('subtotal2')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal3')"
                name="subtotal3"
              >
                <a-input-number
                  v-model:value="formState.subtotal3"
                  :placeholder="pi.ph('subtotal3')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal4')"
                name="subtotal4"
              >
                <a-input-number
                  v-model:value="formState.subtotal4"
                  :placeholder="pi.ph('subtotal4')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal5')"
                name="subtotal5"
              >
                <a-input-number
                  v-model:value="formState.subtotal5"
                  :placeholder="pi.ph('subtotal5')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal6')"
                name="subtotal6"
              >
                <a-input-number
                  v-model:value="formState.subtotal6"
                  :placeholder="pi.ph('subtotal6')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('statisticsExchangeRate')"
                name="statisticsExchangeRate"
              >
                <a-input-number
                  v-model:value="formState.statisticsExchangeRate"
                  :placeholder="pi.ph('statisticsExchangeRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('profitCenterCode')"
                name="profitCenterCode"
              >
                <TaktSelect
                  v-model:value="formState.profitCenterCode"
                  api-url="TaktProfitCenters/options"
                  :placeholder="pi.ph('profitCenterCode')"
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('creditPrice')"
                name="creditPrice"
              >
                <a-input-number
                  v-model:value="formState.creditPrice"
                  :placeholder="pi.ph('creditPrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerGroupSalesOrder')"
                name="customerGroupSalesOrder"
              >
                <a-input
                  v-model:value="formState.customerGroupSalesOrder"
                  :placeholder="pi.ph('customerGroupSalesOrder')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('destinationCountryOrder')"
                name="destinationCountryOrder"
              >
                <TaktSelect
                  v-model:value="formState.destinationCountryOrder"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('destinationCountryOrder')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('regionOrder')"
                name="regionOrder"
              >
                <a-input
                  v-model:value="formState.regionOrder"
                  :placeholder="pi.ph('regionOrder')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesOrganizationOrder')"
                name="salesOrganizationOrder"
              >
                <a-input
                  v-model:value="formState.salesOrganizationOrder"
                  :placeholder="pi.ph('salesOrganizationOrder')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-5"
        :tab="t('common.page.form.tabs.basicinfo') + ' (6/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('distributionChannelOrder')"
                name="distributionChannelOrder"
              >
                <a-input
                  v-model:value="formState.distributionChannelOrder"
                  :placeholder="pi.ph('distributionChannelOrder')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentCategory')"
                name="documentCategory"
              >
                <a-input
                  v-model:value="formState.documentCategory"
                  :placeholder="pi.ph('documentCategory')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxAmount')"
                name="taxAmount"
              >
                <a-input-number
                  v-model:value="formState.taxAmount"
                  :placeholder="pi.ph('taxAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grossAmount')"
                name="grossAmount"
              >
                <a-input-number
                  v-model:value="formState.grossAmount"
                  :placeholder="pi.ph('grossAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('exchangeRateDate')"
                name="exchangeRateDate"
              >
                <a-date-picker
                  v-model:value="formState.exchangeRateDate"
                  :placeholder="pi.ph('exchangeRateDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('postedBy')"
                name="postedBy"
              >
                <TaktSelect
                  v-model:value="formState.postedBy"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('postedBy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isObsolete')"
                name="isObsolete"
              >
                <TaktSelect
                  v-model:value="formState.isObsolete"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isObsolete')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt销售发票主表实体子表 salesInvoiceItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/sales/sales-invoice/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesInvoiceItemI18n } from '../composables/use-invoice-item-i18n'

/** 实体字段 i18n */
const pi = useSalesInvoiceItemI18n()

import type { SalesInvoiceItemCreate } from '@/types/logistics/sales/invoice-item'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["plantCode","billingDocumentCode","lineNumber","billingQuantity","salesUnit","baseUnit","scaleQuantity","billingQuantitySku","netWeight","grossWeight","weightUnit","businessAreaCode","pricingDate","serviceRenderedDate","pricingExchangeRate","netAmount","referenceDocumentCode","referenceDocumentItem","referenceDocumentCategory","salesDocumentCode","salesDocumentItem","salesDocumentReferenceFlag","materialCode","materialDescription","pricingReferenceMaterialCode","batchCode","materialGroup","salesItemCategory","productHierarchy","shippingPoint","division","partnerItem","departureCountry","plantRegion","pricingFlag","warehouseCode","costAmount","subtotal1","subtotal2","subtotal3","subtotal4","subtotal5","subtotal6","statisticsExchangeRate","profitCenterCode","creditPrice","customerGroupSalesOrder","destinationCountryOrder","regionOrder","salesOrganizationOrder","distributionChannelOrder","documentCategory","taxAmount","grossAmount","exchangeRateDate","postedBy","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesInvoiceItemCreate & { salesInvoiceItemId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  departureCountry: "CN",
  destinationCountryOrder: "CN"
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 salesInvoiceItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesInvoiceItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  billingDocumentCode: [
    {
      required: true,
      message: pi.ph('billingDocumentCode'),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('lineNumber'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('lineNumber'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  isObsolete: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isObsolete'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isObsolete'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 salesInvoiceId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('billingQuantity' in payload) {
    const rawbillingQuantity = payload.billingQuantity
    payload.billingQuantity = typeof rawbillingQuantity === 'number' ? rawbillingQuantity : Number(rawbillingQuantity)
  }
  if ('scaleQuantity' in payload) {
    const rawscaleQuantity = payload.scaleQuantity
    payload.scaleQuantity = typeof rawscaleQuantity === 'number' ? rawscaleQuantity : Number(rawscaleQuantity)
  }
  if ('billingQuantitySku' in payload) {
    const rawbillingQuantitySku = payload.billingQuantitySku
    payload.billingQuantitySku = typeof rawbillingQuantitySku === 'number' ? rawbillingQuantitySku : Number(rawbillingQuantitySku)
  }
  if ('netWeight' in payload) {
    const rawnetWeight = payload.netWeight
    payload.netWeight = typeof rawnetWeight === 'number' ? rawnetWeight : Number(rawnetWeight)
  }
  if ('grossWeight' in payload) {
    const rawgrossWeight = payload.grossWeight
    payload.grossWeight = typeof rawgrossWeight === 'number' ? rawgrossWeight : Number(rawgrossWeight)
  }
  if ('pricingExchangeRate' in payload) {
    const rawpricingExchangeRate = payload.pricingExchangeRate
    payload.pricingExchangeRate = typeof rawpricingExchangeRate === 'number' ? rawpricingExchangeRate : Number(rawpricingExchangeRate)
  }
  if ('netAmount' in payload) {
    const rawnetAmount = payload.netAmount
    payload.netAmount = typeof rawnetAmount === 'number' ? rawnetAmount : Number(rawnetAmount)
  }
  if ('referenceDocumentItem' in payload) {
    const rawreferenceDocumentItem = payload.referenceDocumentItem
    payload.referenceDocumentItem = typeof rawreferenceDocumentItem === 'number' ? rawreferenceDocumentItem : Number(rawreferenceDocumentItem)
  }
  if ('salesDocumentItem' in payload) {
    const rawsalesDocumentItem = payload.salesDocumentItem
    payload.salesDocumentItem = typeof rawsalesDocumentItem === 'number' ? rawsalesDocumentItem : Number(rawsalesDocumentItem)
  }
  if ('partnerItem' in payload) {
    const rawpartnerItem = payload.partnerItem
    payload.partnerItem = typeof rawpartnerItem === 'number' ? rawpartnerItem : Number(rawpartnerItem)
  }
  if ('costAmount' in payload) {
    const rawcostAmount = payload.costAmount
    payload.costAmount = typeof rawcostAmount === 'number' ? rawcostAmount : Number(rawcostAmount)
  }
  if ('subtotal1' in payload) {
    const rawsubtotal1 = payload.subtotal1
    payload.subtotal1 = typeof rawsubtotal1 === 'number' ? rawsubtotal1 : Number(rawsubtotal1)
  }
  if ('subtotal2' in payload) {
    const rawsubtotal2 = payload.subtotal2
    payload.subtotal2 = typeof rawsubtotal2 === 'number' ? rawsubtotal2 : Number(rawsubtotal2)
  }
  if ('subtotal3' in payload) {
    const rawsubtotal3 = payload.subtotal3
    payload.subtotal3 = typeof rawsubtotal3 === 'number' ? rawsubtotal3 : Number(rawsubtotal3)
  }
  if ('subtotal4' in payload) {
    const rawsubtotal4 = payload.subtotal4
    payload.subtotal4 = typeof rawsubtotal4 === 'number' ? rawsubtotal4 : Number(rawsubtotal4)
  }
  if ('subtotal5' in payload) {
    const rawsubtotal5 = payload.subtotal5
    payload.subtotal5 = typeof rawsubtotal5 === 'number' ? rawsubtotal5 : Number(rawsubtotal5)
  }
  if ('subtotal6' in payload) {
    const rawsubtotal6 = payload.subtotal6
    payload.subtotal6 = typeof rawsubtotal6 === 'number' ? rawsubtotal6 : Number(rawsubtotal6)
  }
  if ('statisticsExchangeRate' in payload) {
    const rawstatisticsExchangeRate = payload.statisticsExchangeRate
    payload.statisticsExchangeRate = typeof rawstatisticsExchangeRate === 'number' ? rawstatisticsExchangeRate : Number(rawstatisticsExchangeRate)
  }
  if ('creditPrice' in payload) {
    const rawcreditPrice = payload.creditPrice
    payload.creditPrice = typeof rawcreditPrice === 'number' ? rawcreditPrice : Number(rawcreditPrice)
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    payload.taxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
  }
  if ('grossAmount' in payload) {
    const rawgrossAmount = payload.grossAmount
    payload.grossAmount = typeof rawgrossAmount === 'number' ? rawgrossAmount : Number(rawgrossAmount)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.salesInvoiceId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
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
