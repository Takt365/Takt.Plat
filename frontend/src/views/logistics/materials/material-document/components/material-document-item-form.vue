<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-document/components -->
<!-- 文件名称：material-document-item-form.vue -->
<!-- 功能描述：Takt物料凭证主表实体子表 materialDocumentItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-document-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-document-item-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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
                :label="pi.label('lineId')"
                name="lineId"
              >
                <a-input
                  v-model:value="formState.lineId"
                  :placeholder="pi.ph('lineId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('parentLineId')"
                name="parentLineId"
              >
                <a-input
                  v-model:value="formState.parentLineId"
                  :placeholder="pi.ph('parentLineId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lineDepth')"
                name="lineDepth"
              >
                <a-input
                  v-model:value="formState.lineDepth"
                  :placeholder="pi.ph('lineDepth')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('movementType')"
                name="movementType"
              >
                <TaktSelect
                  v-model:value="formState.movementType"
                  dict-type="logistics_movement_type"
                  :placeholder="pi.ph('movementType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('autoCreatedFlag')"
                name="autoCreatedFlag"
              >
                <a-input
                  v-model:value="formState.autoCreatedFlag"
                  :placeholder="pi.ph('autoCreatedFlag')"
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
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.materialDocumentItemId"
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
                  :disabled="!!formData?.materialDocumentItemId"
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
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stockType')"
                name="stockType"
              >
                <TaktSelect
                  v-model:value="formState.stockType"
                  dict-type="logistics_stock_type"
                  :placeholder="pi.ph('stockType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('restrictedStockFlag')"
                name="restrictedStockFlag"
              >
                <a-input
                  v-model:value="formState.restrictedStockFlag"
                  :placeholder="pi.ph('restrictedStockFlag')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('specialStock')"
                name="specialStock"
              >
                <TaktSelect
                  v-model:value="formState.specialStock"
                  dict-type="logistics_special_stock_type"
                  :placeholder="pi.ph('specialStock')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierCode')"
                name="supplierCode"
              >
                <TaktSelect
                  v-model:value="formState.supplierCode"
                  api-url="TaktSuppliers/options"
                  :placeholder="pi.ph('supplierCode')"
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerCode')"
                name="customerCode"
              >
                <TaktSelect
                  v-model:value="formState.customerCode"
                  api-url="TaktCustomers/options"
                  :placeholder="pi.ph('customerCode')"
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('debitCreditIndicator')"
                name="debitCreditIndicator"
              >
                <a-input
                  v-model:value="formState.debitCreditIndicator"
                  :placeholder="pi.ph('debitCreditIndicator')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('currencyCode')"
                name="currencyCode"
              >
                <TaktSelect
                  v-model:value="formState.currencyCode"
                  dict-type="accounting_currency_code"
                  :placeholder="pi.ph('currencyCode')"
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('localCurrencyAmount')"
                name="localCurrencyAmount"
              >
                <a-input-number
                  v-model:value="formState.localCurrencyAmount"
                  :placeholder="pi.ph('localCurrencyAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('alternativeAmount')"
                name="alternativeAmount"
              >
                <a-input-number
                  v-model:value="formState.alternativeAmount"
                  :placeholder="pi.ph('alternativeAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('quantity')"
                name="quantity"
              >
                <a-input-number
                  v-model:value="formState.quantity"
                  :placeholder="pi.ph('quantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('baseUnit')"
                name="baseUnit"
              >
                <TaktSelect
                  v-model:value="formState.baseUnit"
                  dict-type="logistics_unit_of_measure_code"
                  :placeholder="pi.ph('baseUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('entryQuantity')"
                name="entryQuantity"
              >
                <a-input-number
                  v-model:value="formState.entryQuantity"
                  :placeholder="pi.ph('entryQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('entryUnit')"
                name="entryUnit"
              >
                <a-input
                  v-model:value="formState.entryUnit"
                  :placeholder="pi.ph('entryUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('poPriceQuantity')"
                name="poPriceQuantity"
              >
                <a-input-number
                  v-model:value="formState.poPriceQuantity"
                  :placeholder="pi.ph('poPriceQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('poPriceUnit')"
                name="poPriceUnit"
              >
                <a-input
                  v-model:value="formState.poPriceUnit"
                  :placeholder="pi.ph('poPriceUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseOrderCode')"
                name="purchaseOrderCode"
              >
                <a-input
                  v-model:value="formState.purchaseOrderCode"
                  :placeholder="pi.ph('purchaseOrderCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseOrderItem')"
                name="purchaseOrderItem"
              >
                <a-input-number
                  v-model:value="formState.purchaseOrderItem"
                  :placeholder="pi.ph('purchaseOrderItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentYear')"
                name="referenceDocumentYear"
              >
                <a-input
                  v-model:value="formState.referenceDocumentYear"
                  :placeholder="pi.ph('referenceDocumentYear')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                  :disabled="!!formData?.materialDocumentItemId"
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
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('originalMaterialDocumentYear')"
                name="originalMaterialDocumentYear"
              >
                <a-input
                  v-model:value="formState.originalMaterialDocumentYear"
                  :placeholder="pi.ph('originalMaterialDocumentYear')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('originalMaterialDocumentCode')"
                name="originalMaterialDocumentCode"
              >
                <a-input
                  v-model:value="formState.originalMaterialDocumentCode"
                  :placeholder="pi.ph('originalMaterialDocumentCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('originalLineNumber')"
                name="originalLineNumber"
              >
                <a-input-number
                  v-model:value="formState.originalLineNumber"
                  :placeholder="pi.ph('originalLineNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deliveryCompletedFlag')"
                name="deliveryCompletedFlag"
              >
                <a-input
                  v-model:value="formState.deliveryCompletedFlag"
                  :placeholder="pi.ph('deliveryCompletedFlag')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('itemText')"
                name="itemText"
              >
                <a-input
                  v-model:value="formState.itemText"
                  :placeholder="pi.ph('itemText')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('equipmentCode')"
                name="equipmentCode"
              >
                <a-input
                  v-model:value="formState.equipmentCode"
                  :placeholder="pi.ph('equipmentCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('goodsRecipient')"
                name="goodsRecipient"
              >
                <a-input
                  v-model:value="formState.goodsRecipient"
                  :placeholder="pi.ph('goodsRecipient')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unloadingPoint')"
                name="unloadingPoint"
              >
                <a-input
                  v-model:value="formState.unloadingPoint"
                  :placeholder="pi.ph('unloadingPoint')"
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
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('controllingAreaCode')"
                name="controllingAreaCode"
              >
                <a-input
                  v-model:value="formState.controllingAreaCode"
                  :placeholder="pi.ph('controllingAreaCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tradingPartnerBusinessArea')"
                name="tradingPartnerBusinessArea"
              >
                <a-input
                  v-model:value="formState.tradingPartnerBusinessArea"
                  :placeholder="pi.ph('tradingPartnerBusinessArea')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionOrderCode')"
                name="productionOrderCode"
              >
                <a-input
                  v-model:value="formState.productionOrderCode"
                  :placeholder="pi.ph('productionOrderCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assetCode')"
                name="assetCode"
              >
                <a-input
                  v-model:value="formState.assetCode"
                  :placeholder="pi.ph('assetCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assetSubCode')"
                name="assetSubCode"
              >
                <a-input
                  v-model:value="formState.assetSubCode"
                  :placeholder="pi.ph('assetSubCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fiscalYear')"
                name="fiscalYear"
              >
                <a-input
                  v-model:value="formState.fiscalYear"
                  :placeholder="pi.ph('fiscalYear')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('postToPreviousPeriodFlag')"
                name="postToPreviousPeriodFlag"
              >
                <a-input
                  v-model:value="formState.postToPreviousPeriodFlag"
                  :placeholder="pi.ph('postToPreviousPeriodFlag')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('postToPreviousYearFlag')"
                name="postToPreviousYearFlag"
              >
                <a-input
                  v-model:value="formState.postToPreviousYearFlag"
                  :placeholder="pi.ph('postToPreviousYearFlag')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountingDocumentCode')"
                name="accountingDocumentCode"
              >
                <a-input
                  v-model:value="formState.accountingDocumentCode"
                  :placeholder="pi.ph('accountingDocumentCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountingDocumentItem')"
                name="accountingDocumentItem"
              >
                <a-input-number
                  v-model:value="formState.accountingDocumentItem"
                  :placeholder="pi.ph('accountingDocumentItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('revaluationDocumentCode')"
                name="revaluationDocumentCode"
              >
                <a-input
                  v-model:value="formState.revaluationDocumentCode"
                  :placeholder="pi.ph('revaluationDocumentCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-5"
        :tab="t('common.page.form.tabs.basicinfo') + ' (6/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('revaluationDocumentItem')"
                name="revaluationDocumentItem"
              >
                <a-input
                  v-model:value="formState.revaluationDocumentItem"
                  :placeholder="pi.ph('revaluationDocumentItem')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reservationCode')"
                name="reservationCode"
              >
                <a-input
                  v-model:value="formState.reservationCode"
                  :placeholder="pi.ph('reservationCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reservationItem')"
                name="reservationItem"
              >
                <a-input-number
                  v-model:value="formState.reservationItem"
                  :placeholder="pi.ph('reservationItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('finalIssueFlag')"
                name="finalIssueFlag"
              >
                <a-input
                  v-model:value="formState.finalIssueFlag"
                  :placeholder="pi.ph('finalIssueFlag')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reservationQuantity')"
                name="reservationQuantity"
              >
                <a-input-number
                  v-model:value="formState.reservationQuantity"
                  :placeholder="pi.ph('reservationQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('receivingMaterialCode')"
                name="receivingMaterialCode"
              >
                <a-input
                  v-model:value="formState.receivingMaterialCode"
                  :placeholder="pi.ph('receivingMaterialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('receivingPlantCode')"
                name="receivingPlantCode"
              >
                <a-input
                  v-model:value="formState.receivingPlantCode"
                  :placeholder="pi.ph('receivingPlantCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('receivingWarehouseCode')"
                name="receivingWarehouseCode"
              >
                <a-input
                  v-model:value="formState.receivingWarehouseCode"
                  :placeholder="pi.ph('receivingWarehouseCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
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
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('valuatedStockQuantity')"
                name="valuatedStockQuantity"
              >
                <a-input-number
                  v-model:value="formState.valuatedStockQuantity"
                  :placeholder="pi.ph('valuatedStockQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-6"
        :tab="t('common.page.form.tabs.basicinfo') + ' (7/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalValuatedStockValue')"
                name="totalValuatedStockValue"
              >
                <a-input-number
                  v-model:value="formState.totalValuatedStockValue"
                  :placeholder="pi.ph('totalValuatedStockValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('priceControl')"
                name="priceControl"
              >
                <a-input
                  v-model:value="formState.priceControl"
                  :placeholder="pi.ph('priceControl')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerPartMaterialCode')"
                name="manufacturerPartMaterialCode"
              >
                <a-input
                  v-model:value="formState.manufacturerPartMaterialCode"
                  :placeholder="pi.ph('manufacturerPartMaterialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mkpfReferenceCode')"
                name="mkpfReferenceCode"
              >
                <a-input
                  v-model:value="formState.mkpfReferenceCode"
                  :placeholder="pi.ph('mkpfReferenceCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('imDeliveryCode')"
                name="imDeliveryCode"
              >
                <a-input
                  v-model:value="formState.imDeliveryCode"
                  :placeholder="pi.ph('imDeliveryCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialDocumentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('imDeliveryItem')"
                name="imDeliveryItem"
              >
                <a-input-number
                  v-model:value="formState.imDeliveryItem"
                  :placeholder="pi.ph('imDeliveryItem')"
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
 * Takt物料凭证主表实体子表 materialDocumentItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/materials/material-document/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaterialDocumentItemI18n } from '../composables/use-material-document-item-i18n'

/** 实体字段 i18n */
const pi = useMaterialDocumentItemI18n()

import type { MaterialDocumentItemCreate } from '@/types/logistics/materials/material-document-item'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","lineId","parentLineId","lineDepth","movementType","autoCreatedFlag","materialCode","plantCode","warehouseCode","batchCode","stockType","restrictedStockFlag","specialStock","supplierCode","customerCode","debitCreditIndicator","currencyCode","localCurrencyAmount","alternativeAmount","quantity","baseUnit","entryQuantity","entryUnit","poPriceQuantity","poPriceUnit","purchaseOrderCode","purchaseOrderItem","referenceDocumentYear","referenceDocumentCode","referenceDocumentItem","originalMaterialDocumentYear","originalMaterialDocumentCode","originalLineNumber","deliveryCompletedFlag","itemText","equipmentCode","goodsRecipient","unloadingPoint","businessAreaCode","controllingAreaCode","tradingPartnerBusinessArea","productionOrderCode","assetCode","assetSubCode","fiscalYear","postToPreviousPeriodFlag","postToPreviousYearFlag","accountingDocumentCode","accountingDocumentItem","revaluationDocumentCode","revaluationDocumentItem","reservationCode","reservationItem","finalIssueFlag","reservationQuantity","receivingMaterialCode","receivingPlantCode","receivingWarehouseCode","profitCenterCode","valuatedStockQuantity","totalValuatedStockValue","priceControl","manufacturerPartMaterialCode","mkpfReferenceCode","imDeliveryCode","imDeliveryItem","postedBy","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaterialDocumentItemCreate & { materialDocumentItemId?: string }> | null
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
  movementType: "101",
  specialStock: " ",
  currencyCode: "CNY"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 materialDocumentItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.materialDocumentItemId) {
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
  movementType: [
    {
      required: true,
      message: pi.ph('movementType'),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  localCurrencyAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('localCurrencyAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('localCurrencyAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  quantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('quantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('quantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
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

/** 映射为 Create/Update DTO（含主表外键 materialDocumentId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('localCurrencyAmount' in payload) {
    const rawlocalCurrencyAmount = payload.localCurrencyAmount
    payload.localCurrencyAmount = typeof rawlocalCurrencyAmount === 'number' ? rawlocalCurrencyAmount : Number(rawlocalCurrencyAmount)
  }
  if ('alternativeAmount' in payload) {
    const rawalternativeAmount = payload.alternativeAmount
    payload.alternativeAmount = typeof rawalternativeAmount === 'number' ? rawalternativeAmount : Number(rawalternativeAmount)
  }
  if ('quantity' in payload) {
    const rawquantity = payload.quantity
    payload.quantity = typeof rawquantity === 'number' ? rawquantity : Number(rawquantity)
  }
  if ('entryQuantity' in payload) {
    const rawentryQuantity = payload.entryQuantity
    payload.entryQuantity = typeof rawentryQuantity === 'number' ? rawentryQuantity : Number(rawentryQuantity)
  }
  if ('poPriceQuantity' in payload) {
    const rawpoPriceQuantity = payload.poPriceQuantity
    payload.poPriceQuantity = typeof rawpoPriceQuantity === 'number' ? rawpoPriceQuantity : Number(rawpoPriceQuantity)
  }
  if ('purchaseOrderItem' in payload) {
    const rawpurchaseOrderItem = payload.purchaseOrderItem
    payload.purchaseOrderItem = typeof rawpurchaseOrderItem === 'number' ? rawpurchaseOrderItem : Number(rawpurchaseOrderItem)
  }
  if ('referenceDocumentItem' in payload) {
    const rawreferenceDocumentItem = payload.referenceDocumentItem
    payload.referenceDocumentItem = typeof rawreferenceDocumentItem === 'number' ? rawreferenceDocumentItem : Number(rawreferenceDocumentItem)
  }
  if ('originalLineNumber' in payload) {
    const raworiginalLineNumber = payload.originalLineNumber
    payload.originalLineNumber = typeof raworiginalLineNumber === 'number' ? raworiginalLineNumber : Number(raworiginalLineNumber)
  }
  if ('accountingDocumentItem' in payload) {
    const rawaccountingDocumentItem = payload.accountingDocumentItem
    payload.accountingDocumentItem = typeof rawaccountingDocumentItem === 'number' ? rawaccountingDocumentItem : Number(rawaccountingDocumentItem)
  }
  if ('reservationItem' in payload) {
    const rawreservationItem = payload.reservationItem
    payload.reservationItem = typeof rawreservationItem === 'number' ? rawreservationItem : Number(rawreservationItem)
  }
  if ('reservationQuantity' in payload) {
    const rawreservationQuantity = payload.reservationQuantity
    payload.reservationQuantity = typeof rawreservationQuantity === 'number' ? rawreservationQuantity : Number(rawreservationQuantity)
  }
  if ('valuatedStockQuantity' in payload) {
    const rawvaluatedStockQuantity = payload.valuatedStockQuantity
    payload.valuatedStockQuantity = typeof rawvaluatedStockQuantity === 'number' ? rawvaluatedStockQuantity : Number(rawvaluatedStockQuantity)
  }
  if ('totalValuatedStockValue' in payload) {
    const rawtotalValuatedStockValue = payload.totalValuatedStockValue
    payload.totalValuatedStockValue = typeof rawtotalValuatedStockValue === 'number' ? rawtotalValuatedStockValue : Number(rawtotalValuatedStockValue)
  }
  if ('imDeliveryItem' in payload) {
    const rawimDeliveryItem = payload.imDeliveryItem
    payload.imDeliveryItem = typeof rawimDeliveryItem === 'number' ? rawimDeliveryItem : Number(rawimDeliveryItem)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.materialDocumentId = props.masterId
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
