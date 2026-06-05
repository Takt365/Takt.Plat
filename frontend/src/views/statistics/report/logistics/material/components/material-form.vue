<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/statistics/report/logistics/material/components -->
<!-- 文件名称：material-form.vue -->
<!-- 功能描述：Takt物料实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-form-tabs"
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
                  size="small"
                  readonly
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
                  size="small"
                  readonly
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
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.code')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.name')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.specification')"
                name="materialSpecification"
              >
                <a-input
                  v-model:value="formState.materialSpecification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.specification') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.material.description')"
                name="materialDescription"
              >
                <a-textarea
                  v-model:value="formState.materialDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.material.description') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.industrysector')"
                name="industrySector"
              >
                <a-input
                  v-model:value="formState.industrySector"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.industrysector') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.hierarchy')"
                name="materialHierarchy"
              >
                <a-input
                  v-model:value="formState.materialHierarchy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.hierarchy') })"
                  size="small"
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
                :label="t('entity.material.groupcode')"
                name="materialGroupCode"
              >
                <a-input
                  v-model:value="formState.materialGroupCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.groupcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.type')"
                name="materialType"
              >
                <a-input-number
                  v-model:value="formState.materialType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.type') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.model')"
                name="materialModel"
              >
                <a-input
                  v-model:value="formState.materialModel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.model') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.brand')"
                name="materialBrand"
              >
                <a-input
                  v-model:value="formState.materialBrand"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.brand') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.baseunit')"
                name="baseUnit"
              >
                <a-input
                  v-model:value="formState.baseUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.baseunit') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.purchasegroup')"
                name="purchaseGroup"
              >
                <a-input
                  v-model:value="formState.purchaseGroup"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.purchasegroup') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.purchasetype')"
                name="purchaseType"
              >
                <a-input-number
                  v-model:value="formState.purchaseType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.purchasetype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.specialprocurement')"
                name="specialProcurement"
              >
                <a-input-number
                  v-model:value="formState.specialProcurement"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.specialprocurement') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.isbulk')"
                name="isBulk"
              >
                <a-input-number
                  v-model:value="formState.isBulk"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.isbulk') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.minorderquantity')"
                name="minOrderQuantity"
              >
                <a-input-number
                  v-model:value="formState.minOrderQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.minorderquantity') })"
                  size="small"
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
                :label="t('entity.material.roundingvalue')"
                name="roundingValue"
              >
                <a-input-number
                  v-model:value="formState.roundingValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.roundingvalue') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.planneddeliverytimedays')"
                name="plannedDeliveryTimeDays"
              >
                <a-input-number
                  v-model:value="formState.plannedDeliveryTimeDays"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.planneddeliverytimedays') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.inhouseproductiondays')"
                name="inHouseProductionDays"
              >
                <a-input-number
                  v-model:value="formState.inHouseProductionDays"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.inhouseproductiondays') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.manufacturer')"
                name="manufacturer"
              >
                <a-input
                  v-model:value="formState.manufacturer"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.manufacturer') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.manufacturerpartnumber')"
                name="manufacturerPartNumber"
              >
                <a-input
                  v-model:value="formState.manufacturerPartNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.manufacturerpartnumber') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.currencycode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.currencycode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.pricecontrol')"
                name="priceControl"
              >
                <a-input-number
                  v-model:value="formState.priceControl"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.pricecontrol') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.priceunit')"
                name="priceUnit"
              >
                <a-input-number
                  v-model:value="formState.priceUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.priceunit') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.valuationcategory')"
                name="valuationCategory"
              >
                <a-input
                  v-model:value="formState.valuationCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.valuationcategory') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.differencecode')"
                name="differenceCode"
              >
                <a-input
                  v-model:value="formState.differenceCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.differencecode') })"
                  size="small"
                  allow-clear
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
                :label="t('entity.material.profitcenter')"
                name="profitCenter"
              >
                <a-input
                  v-model:value="formState.profitCenter"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.profitcenter') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.latestpurchaseprice')"
                name="latestPurchasePrice"
              >
                <a-input-number
                  v-model:value="formState.latestPurchasePrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.latestpurchaseprice') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.salesprice')"
                name="salesPrice"
              >
                <a-input-number
                  v-model:value="formState.salesPrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.salesprice') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.safetystock')"
                name="safetyStock"
              >
                <a-input-number
                  v-model:value="formState.safetyStock"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.safetystock') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.maxstock')"
                name="maxStock"
              >
                <a-input-number
                  v-model:value="formState.maxStock"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.maxstock') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.minstock')"
                name="minStock"
              >
                <a-input-number
                  v-model:value="formState.minStock"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.minstock') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.currentstock')"
                name="currentStock"
              >
                <a-input-number
                  v-model:value="formState.currentStock"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.currentstock') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.productionlocation')"
                name="productionLocation"
              >
                <a-input
                  v-model:value="formState.productionLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.productionlocation') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.purchasinglocation')"
                name="purchasingLocation"
              >
                <a-input
                  v-model:value="formState.purchasingLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.purchasinglocation') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.inspectionrequired')"
                name="inspectionRequired"
              >
                <a-input-number
                  v-model:value="formState.inspectionRequired"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.inspectionrequired') })"
                  size="small"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.isbatch')"
                name="isBatch"
              >
                <a-input-number
                  v-model:value="formState.isBatch"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.isbatch') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.isexpiry')"
                name="isExpiry"
              >
                <a-input-number
                  v-model:value="formState.isExpiry"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.isexpiry') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.expirydays')"
                name="expiryDays"
              >
                <a-input-number
                  v-model:value="formState.expiryDays"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.expirydays') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.status')"
                name="materialStatus"
              >
                <a-input-number
                  v-model:value="formState.materialStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.attributes')"
                name="materialAttributes"
              >
                <a-input
                  v-model:value="formState.materialAttributes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.attributes') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.isendoflife')"
                name="isEndOfLife"
              >
                <a-input
                  v-model:value="formState.isEndOfLife"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.isendoflife') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.endoflifedate')"
                name="endOfLifeDate"
              >
                <a-date-picker
                  v-model:value="formState.endOfLifeDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.endoflifedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.extfieldjson')"
                name="extFieldJson"
              >
                <a-input
                  v-model:value="formState.extFieldJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                  size="small"
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
                  :rows="2"
                  size="small"
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
 * Takt物料实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/statistics/report/logistics/material/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { MaterialCreate } from '@/types/logistics/materials/material'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

const { t } = useI18n()

const tenantStore = useTenantStore()
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
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","materialCode","materialName","materialSpecification","materialDescription","industrySector","materialHierarchy","materialGroupCode","materialType","materialModel","materialBrand","baseUnit","purchaseGroup","purchaseType","specialProcurement","isBulk","minOrderQuantity","roundingValue","plannedDeliveryTimeDays","inHouseProductionDays","manufacturer","manufacturerPartNumber","currencyCode","priceControl","priceUnit","valuationCategory","differenceCode","profitCenter","latestPurchasePrice","salesPrice","safetyStock","maxStock","minStock","currentStock","productionLocation","purchasingLocation","inspectionRequired","isBatch","isExpiry","expiryDays","materialStatus","materialAttributes","isEndOfLife","endOfLifeDate","extFieldJson","remark"]


interface Props {
  formData?: Partial<MaterialCreate & { materialId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})

watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])

    applyScopeDefaults(next)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.materialId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.material.plantcode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.material.code') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.material.name') }),
      trigger: 'blur'
    }
  ],
  materialType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.type') }),
      trigger: 'change'
    }
  ],
  baseUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.material.baseunit') }),
      trigger: 'blur'
    }
  ],
  purchaseType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.purchasetype') }),
      trigger: 'change'
    }
  ],
  specialProcurement: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.specialprocurement') }),
      trigger: 'change'
    }
  ],
  isBulk: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.isbulk') }),
      trigger: 'change'
    }
  ],
  minOrderQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.minorderquantity') }),
      trigger: 'change'
    }
  ],
  roundingValue: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.roundingvalue') }),
      trigger: 'change'
    }
  ],
  plannedDeliveryTimeDays: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.planneddeliverytimedays') }),
      trigger: 'change'
    }
  ],
  inHouseProductionDays: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.inhouseproductiondays') }),
      trigger: 'change'
    }
  ],
  currencyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.material.currencycode') }),
      trigger: 'blur'
    }
  ],
  priceControl: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.pricecontrol') }),
      trigger: 'change'
    }
  ],
  priceUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.priceunit') }),
      trigger: 'change'
    }
  ],
  latestPurchasePrice: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.latestpurchaseprice') }),
      trigger: 'change'
    }
  ],
  salesPrice: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.salesprice') }),
      trigger: 'change'
    }
  ],
  safetyStock: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.safetystock') }),
      trigger: 'change'
    }
  ],
  maxStock: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.maxstock') }),
      trigger: 'change'
    }
  ],
  minStock: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.minstock') }),
      trigger: 'change'
    }
  ],
  currentStock: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.currentstock') }),
      trigger: 'change'
    }
  ],
  inspectionRequired: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.inspectionrequired') }),
      trigger: 'change'
    }
  ],
  isBatch: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.isbatch') }),
      trigger: 'change'
    }
  ],
  isExpiry: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.isexpiry') }),
      trigger: 'change'
    }
  ],
  expiryDays: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.expirydays') }),
      trigger: 'change'
    }
  ],
  materialStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.status') }),
      trigger: 'change'
    }
  ],
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
  return { ...formState }
}

function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])

  activeTab.value = 'tab-0'
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
