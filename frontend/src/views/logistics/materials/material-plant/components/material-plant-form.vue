<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-plant/components -->
<!-- 文件名称：material-plant-form.vue -->
<!-- 功能描述：Takt工厂物料实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-plant-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.tenantcode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companycode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companydefaultculture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.plantcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.materialPlantId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialPlantId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.materialname')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialname') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.materialspecification')"
                name="materialSpecification"
              >
                <a-input
                  v-model:value="formState.materialSpecification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialspecification') })"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.materialplant.materialdescription')"
                name="materialDescription"
              >
                <a-textarea
                  v-model:value="formState.materialDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.materialplant.materialdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.industrysector')"
                name="industrySector"
              >
                <a-input
                  v-model:value="formState.industrySector"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.industrysector') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.materialhierarchy')"
                name="materialHierarchy"
              >
                <a-input
                  v-model:value="formState.materialHierarchy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialhierarchy') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.materialgroupcode')"
                name="materialGroupCode"
              >
                <a-input
                  v-model:value="formState.materialGroupCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialgroupcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialPlantId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.materialtype')"
                name="materialType"
              >
                <a-input-number
                  v-model:value="formState.materialType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialtype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.materialmodel')"
                name="materialModel"
              >
                <a-input
                  v-model:value="formState.materialModel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialmodel') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.materialbrand')"
                name="materialBrand"
              >
                <a-input
                  v-model:value="formState.materialBrand"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialbrand') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.baseunit')"
                name="baseUnit"
              >
                <a-input
                  v-model:value="formState.baseUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.baseunit') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.purchasegroup')"
                name="purchaseGroup"
              >
                <a-input
                  v-model:value="formState.purchaseGroup"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.purchasegroup') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.purchasetype')"
                name="purchaseType"
              >
                <a-input-number
                  v-model:value="formState.purchaseType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.purchasetype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.specialprocurement')"
                name="specialProcurement"
              >
                <a-input-number
                  v-model:value="formState.specialProcurement"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.specialprocurement') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.isbulk')"
                name="isBulk"
              >
                <a-input-number
                  v-model:value="formState.isBulk"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.isbulk') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.minorderquantity')"
                name="minOrderQuantity"
              >
                <a-input-number
                  v-model:value="formState.minOrderQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.minorderquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.roundingvalue')"
                name="roundingValue"
              >
                <a-input-number
                  v-model:value="formState.roundingValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.roundingvalue') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.planneddeliverytimedays')"
                name="plannedDeliveryTimeDays"
              >
                <a-input-number
                  v-model:value="formState.plannedDeliveryTimeDays"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.planneddeliverytimedays') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.inhouseproductiondays')"
                name="inHouseProductionDays"
              >
                <a-input-number
                  v-model:value="formState.inHouseProductionDays"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.inhouseproductiondays') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.manufacturer')"
                name="manufacturer"
              >
                <a-input
                  v-model:value="formState.manufacturer"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.manufacturer') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.manufacturerpartnumber')"
                name="manufacturerPartNumber"
              >
                <a-input
                  v-model:value="formState.manufacturerPartNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.manufacturerpartnumber') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.currencycode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.currencycode') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.materialPlantId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.pricecontrol')"
                name="priceControl"
              >
                <a-input-number
                  v-model:value="formState.priceControl"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.pricecontrol') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.priceunit')"
                name="priceUnit"
              >
                <a-input-number
                  v-model:value="formState.priceUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.priceunit') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.valuationcategory')"
                name="valuationCategory"
              >
                <a-input
                  v-model:value="formState.valuationCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.valuationcategory') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.differencecode')"
                name="differenceCode"
              >
                <a-input
                  v-model:value="formState.differenceCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.differencecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.materialPlantId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.profitcenter')"
                name="profitCenter"
              >
                <a-input
                  v-model:value="formState.profitCenter"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.profitcenter') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.latestpurchaseprice')"
                name="latestPurchasePrice"
              >
                <a-input-number
                  v-model:value="formState.latestPurchasePrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.latestpurchaseprice') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.salesprice')"
                name="salesPrice"
              >
                <a-input-number
                  v-model:value="formState.salesPrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.salesprice') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.safetystock')"
                name="safetyStock"
              >
                <a-input-number
                  v-model:value="formState.safetyStock"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.safetystock') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.maxstock')"
                name="maxStock"
              >
                <a-input-number
                  v-model:value="formState.maxStock"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.maxstock') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.minstock')"
                name="minStock"
              >
                <a-input-number
                  v-model:value="formState.minStock"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.minstock') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.currentstock')"
                name="currentStock"
              >
                <a-input-number
                  v-model:value="formState.currentStock"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.currentstock') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.productionlocation')"
                name="productionLocation"
              >
                <a-input
                  v-model:value="formState.productionLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.productionlocation') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.purchasinglocation')"
                name="purchasingLocation"
              >
                <a-input
                  v-model:value="formState.purchasingLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.purchasinglocation') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.inspectionrequired')"
                name="inspectionRequired"
              >
                <a-input-number
                  v-model:value="formState.inspectionRequired"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.inspectionrequired') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.materialplant.isbatch')"
                name="isBatch"
              >
                <a-input-number
                  v-model:value="formState.isBatch"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.isbatch') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.materialplant.isexpiry')"
                name="isExpiry"
              >
                <a-input-number
                  v-model:value="formState.isExpiry"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.isexpiry') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.materialplant.expirydays')"
                name="expiryDays"
              >
                <a-input-number
                  v-model:value="formState.expiryDays"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.expirydays') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.materialplant.materialstatus')"
                name="materialStatus"
              >
                <TaktSelect
                  v-model:value="formState.materialStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.materialstatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.materialplant.materialattributes')"
                name="materialAttributes"
              >
                <a-input
                  v-model:value="formState.materialAttributes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialattributes') })"
                  show-count
                  :maxlength="4000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.materialplant.isendoflife')"
                name="isEndOfLife"
              >
                <a-input
                  v-model:value="formState.isEndOfLife"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.isendoflife') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.materialplant.endoflifedate')"
                name="endOfLifeDate"
              >
                <a-date-picker
                  v-model:value="formState.endOfLifeDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.endoflifedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                    <span>{{ t('common.page.entity.extfield') }}</span>
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
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt工厂物料实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-plant/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { MaterialPlantCreate } from '@/types/logistics/materials/material-plant'
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
  if (formFields.includes('companyDefaultCulture') && (force || !target.companyDefaultCulture)) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","materialCode","materialName","materialSpecification","materialDescription","industrySector","materialHierarchy","materialGroupCode","materialType","materialModel","materialBrand","baseUnit","purchaseGroup","purchaseType","specialProcurement","isBulk","minOrderQuantity","roundingValue","plannedDeliveryTimeDays","inHouseProductionDays","manufacturer","manufacturerPartNumber","currencyCode","priceControl","priceUnit","valuationCategory","differenceCode","profitCenter","latestPurchasePrice","salesPrice","safetyStock","maxStock","minStock","currentStock","productionLocation","purchasingLocation","inspectionRequired","isBatch","isExpiry","expiryDays","materialStatus","materialAttributes","isEndOfLife","endOfLifeDate","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaterialPlantCreate & { materialPlantId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  materialStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 materialPlantId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.materialPlantId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
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
    const isCreate = !props.formData?.materialPlantId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.plantcode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialname') }),
      trigger: 'blur'
    }
  ],
  materialType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.materialtype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.materialtype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  baseUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.baseunit') }),
      trigger: 'blur'
    }
  ],
  purchaseType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.purchasetype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.purchasetype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  specialProcurement: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.specialprocurement') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.specialprocurement') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isBulk: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.isbulk') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.isbulk') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  minOrderQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.minorderquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.minorderquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  roundingValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.roundingvalue') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.roundingvalue') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedDeliveryTimeDays: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.planneddeliverytimedays') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.planneddeliverytimedays') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inHouseProductionDays: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.inhouseproductiondays') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.inhouseproductiondays') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.currencycode') }),
      trigger: 'blur'
    }
  ],
  priceControl: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.pricecontrol') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.pricecontrol') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priceUnit: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.priceunit') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.priceunit') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  latestPurchasePrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.latestpurchaseprice') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.latestpurchaseprice') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  salesPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.salesprice') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.salesprice') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  safetyStock: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.safetystock') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.safetystock') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maxStock: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.maxstock') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.maxstock') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  minStock: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.minstock') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.minstock') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currentStock: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.currentstock') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.currentstock') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionRequired: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.inspectionrequired') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.inspectionrequired') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isBatch: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.isbatch') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.isbatch') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isExpiry: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.isexpiry') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.isexpiry') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  expiryDays: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.expirydays') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.expirydays') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.materialstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.materialstatus') }))
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

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('materialType' in payload) {
    const rawmaterialType = payload.materialType
    payload.materialType = typeof rawmaterialType === 'number' ? rawmaterialType : Number(rawmaterialType)
  }
  if ('purchaseType' in payload) {
    const rawpurchaseType = payload.purchaseType
    payload.purchaseType = typeof rawpurchaseType === 'number' ? rawpurchaseType : Number(rawpurchaseType)
  }
  if ('specialProcurement' in payload) {
    const rawspecialProcurement = payload.specialProcurement
    payload.specialProcurement = typeof rawspecialProcurement === 'number' ? rawspecialProcurement : Number(rawspecialProcurement)
  }
  if ('isBulk' in payload) {
    const rawisBulk = payload.isBulk
    payload.isBulk = typeof rawisBulk === 'number' ? rawisBulk : Number(rawisBulk)
  }
  if ('minOrderQuantity' in payload) {
    const rawminOrderQuantity = payload.minOrderQuantity
    payload.minOrderQuantity = typeof rawminOrderQuantity === 'number' ? rawminOrderQuantity : Number(rawminOrderQuantity)
  }
  if ('roundingValue' in payload) {
    const rawroundingValue = payload.roundingValue
    payload.roundingValue = typeof rawroundingValue === 'number' ? rawroundingValue : Number(rawroundingValue)
  }
  if ('plannedDeliveryTimeDays' in payload) {
    const rawplannedDeliveryTimeDays = payload.plannedDeliveryTimeDays
    payload.plannedDeliveryTimeDays = typeof rawplannedDeliveryTimeDays === 'number' ? rawplannedDeliveryTimeDays : Number(rawplannedDeliveryTimeDays)
  }
  if ('inHouseProductionDays' in payload) {
    const rawinHouseProductionDays = payload.inHouseProductionDays
    payload.inHouseProductionDays = typeof rawinHouseProductionDays === 'number' ? rawinHouseProductionDays : Number(rawinHouseProductionDays)
  }
  if ('priceControl' in payload) {
    const rawpriceControl = payload.priceControl
    payload.priceControl = typeof rawpriceControl === 'number' ? rawpriceControl : Number(rawpriceControl)
  }
  if ('priceUnit' in payload) {
    const rawpriceUnit = payload.priceUnit
    payload.priceUnit = typeof rawpriceUnit === 'number' ? rawpriceUnit : Number(rawpriceUnit)
  }
  if ('latestPurchasePrice' in payload) {
    const rawlatestPurchasePrice = payload.latestPurchasePrice
    payload.latestPurchasePrice = typeof rawlatestPurchasePrice === 'number' ? rawlatestPurchasePrice : Number(rawlatestPurchasePrice)
  }
  if ('salesPrice' in payload) {
    const rawsalesPrice = payload.salesPrice
    payload.salesPrice = typeof rawsalesPrice === 'number' ? rawsalesPrice : Number(rawsalesPrice)
  }
  if ('safetyStock' in payload) {
    const rawsafetyStock = payload.safetyStock
    payload.safetyStock = typeof rawsafetyStock === 'number' ? rawsafetyStock : Number(rawsafetyStock)
  }
  if ('maxStock' in payload) {
    const rawmaxStock = payload.maxStock
    payload.maxStock = typeof rawmaxStock === 'number' ? rawmaxStock : Number(rawmaxStock)
  }
  if ('minStock' in payload) {
    const rawminStock = payload.minStock
    payload.minStock = typeof rawminStock === 'number' ? rawminStock : Number(rawminStock)
  }
  if ('currentStock' in payload) {
    const rawcurrentStock = payload.currentStock
    payload.currentStock = typeof rawcurrentStock === 'number' ? rawcurrentStock : Number(rawcurrentStock)
  }
  if ('inspectionRequired' in payload) {
    const rawinspectionRequired = payload.inspectionRequired
    payload.inspectionRequired = typeof rawinspectionRequired === 'number' ? rawinspectionRequired : Number(rawinspectionRequired)
  }
  if ('isBatch' in payload) {
    const rawisBatch = payload.isBatch
    payload.isBatch = typeof rawisBatch === 'number' ? rawisBatch : Number(rawisBatch)
  }
  if ('isExpiry' in payload) {
    const rawisExpiry = payload.isExpiry
    payload.isExpiry = typeof rawisExpiry === 'number' ? rawisExpiry : Number(rawisExpiry)
  }
  if ('expiryDays' in payload) {
    const rawexpiryDays = payload.expiryDays
    payload.expiryDays = typeof rawexpiryDays === 'number' ? rawexpiryDays : Number(rawexpiryDays)
  }
  if ('materialStatus' in payload) {
    const rawmaterialStatus = payload.materialStatus
    payload.materialStatus = typeof rawmaterialStatus === 'number' ? rawmaterialStatus : Number(rawmaterialStatus)
  }
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.materialPlantId)

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
