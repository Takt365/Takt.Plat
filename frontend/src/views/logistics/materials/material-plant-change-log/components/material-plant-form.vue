<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-plant-change-log/components -->
<!-- 文件名称：material-plant-form.vue -->
<!-- 功能描述：Takt工厂物料实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-plant-form flex flex-col min-h-0"
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
                <TaktSelect
                  v-model:value="formState.industrySector"
                  dict-type="logistics_industry_sector"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.industrysector') })"
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
                :label="t('entity.materialplant.materialgroup')"
                name="materialGroup"
              >
                <a-input
                  v-model:value="formState.materialGroup"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialgroup') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.materialtype')"
                name="materialType"
              >
                <TaktSelect
                  v-model:value="formState.materialType"
                  dict-type="logistics_material_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.materialtype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.baseunit')"
                name="baseUnit"
              >
                <TaktSelect
                  v-model:value="formState.baseUnit"
                  dict-type="logistics_unit_of_measure_code"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.baseunit') })"
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
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.purchasetype')"
                name="purchaseType"
              >
                <TaktSelect
                  v-model:value="formState.purchaseType"
                  dict-type="logistics_procurement_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.purchasetype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.specialprocurement')"
                name="specialProcurement"
              >
                <TaktSelect
                  v-model:value="formState.specialProcurement"
                  dict-type="logistics_special_procurement_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.specialprocurement') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.isbulk')"
                name="isBulk"
              >
                <TaktSelect
                  v-model:value="formState.isBulk"
                  dict-type="logistics_bulk_material_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.isbulk') })"
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
                  :min="0"
                  :precision="0"
                  :step="1"
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
                  :min="0"
                  :precision="0"
                  :step="1"
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
                  :min="0"
                  :precision="0"
                  :step="1"
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
                  :min="0"
                  :precision="1"
                  :step="0.5"
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
                :label="t('entity.materialplant.manufacturermaterialcode')"
                name="manufacturerMaterialCode"
              >
                <TaktSelect
                  v-model:value="formState.manufacturerMaterialCode"
                  api-url="TaktManufacturerMaterials/options"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.manufacturermaterialcode') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.currency')"
                name="currency"
              >
                <TaktSelect
                  v-model:value="formState.currency"
                  dict-type="accounting_currency_code"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.currency') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.pricecontrol')"
                name="priceControl"
              >
                <TaktSelect
                  v-model:value="formState.priceControl"
                  dict-type="logistics_price_control_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.pricecontrol') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.priceunit')"
                name="priceUnit"
              >
                <TaktSelect
                  v-model:value="formState.priceUnit"
                  dict-type="logistics_price_unit_param"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.priceunit') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.valuation')"
                name="valuation"
              >
                <TaktSelect
                  v-model:value="formState.valuation"
                  dict-type="logistics_valuation_class_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.valuation') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.movingprice')"
                name="movingPrice"
              >
                <a-input-number
                  v-model:value="formState.movingPrice"
                  :precision="4"
                  :step="0.0001"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.movingprice') })"
                  style="width: 100%"
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
                  :maxlength="6"
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
                  :maxlength="4"
                  allow-clear
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
                  :precision="4"
                  :step="0.0001"
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
                  :maxlength="4"
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
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.storagelocation')"
                name="storageLocation"
              >
                <a-input
                  v-model:value="formState.storageLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.materialplant.storagelocation') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.materialplant.isinspection')"
                name="isInspection"
              >
                <TaktSelect
                  v-model:value="formState.isInspection"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.isinspection') })"
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
                <TaktSelect
                  v-model:value="formState.isBatch"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.isbatch') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.materialplant.isendoflife')"
                name="isEndOfLife"
              >
                <TaktSelect
                  v-model:value="formState.isEndOfLife"
                  dict-type="logistics_material_eol_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.materialplant.isendoflife') })"
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
    <!-- 下：子表 changeLogs -->
    <TaktEditableTable
      ref="materialPlantChangeLogTableRef"
      v-model="childMaterialPlantChangeLogRows"
      :columns="materialPlantChangeLogFormColumns"
      :title="t('entity.materialplantchangelog._self')"
      :add-button-entity="t('entity.materialplantchangelog._self')"
      id-field="materialPlantChangeLogId"
      :default-row="createDefaultMaterialPlantChangeLogRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt工厂物料实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-plant-change-log/components
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","materialCode","materialName","materialSpecification","materialDescription","industrySector","materialHierarchy","materialGroup","materialType","baseUnit","purchaseGroup","purchaseType","specialProcurement","isBulk","minOrderQuantity","roundingValue","plannedDeliveryTimeDays","inHouseProductionDays","manufacturer","manufacturerMaterialCode","currency","priceControl","priceUnit","valuation","movingPrice","differenceCode","profitCenter","currentStock","productionLocation","purchasingLocation","storageLocation","isInspection","isBatch","isEndOfLife","materialStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childMaterialPlantChangeLogRows = ref<Record<string, unknown>[]>([])
const materialPlantChangeLogTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 materialPlantChangeLog 可编辑列 */
const materialPlantChangeLogFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'materialCode',
    title: t('entity.materialplantchangelog.materialcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'plantCode',
    title: t('entity.materialplantchangelog.plantcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'changeFields',
    title: t('entity.materialplantchangelog.changefields'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.materialplantchangelog.changefields') }),
  },
  {
    key: 'changeTime',
    title: t('entity.materialplantchangelog.changetime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'changeBy',
    title: t('entity.materialplantchangelog.changeby'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.materialplantchangelog.changeby') }),
  },
  {
    key: 'changeReason',
    title: t('entity.materialplantchangelog.changereason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.materialplantchangelog.changereason') }),
  },
  {
    key: 'extField',
    title: t('common.page.entity.extfield'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.extfield') }),
    width: 140,
  },
  {
    key: 'remark',
    title: t('common.page.entity.remark'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') }),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaterialPlantCreate & { materialPlantId?: string }> | null | undefined) {
  childMaterialPlantChangeLogRows.value = ((val as any)?.changeLogs ?? []) as Record<string, unknown>[]
}

function createDefaultMaterialPlantChangeLogRow(): Record<string, unknown> {
  return {
    materialCode: '',
    plantCode: '',
    changeFields: '',
    changeTime: '',
    changeBy: '',
    changeReason: '',
    extField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.materialPlantId ?? ''
  return {
    ...formState,
    changeLogs: materialPlantChangeLogTableRef.value?.getRows?.() ?? childMaterialPlantChangeLogRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      materialPlantId: masterId,
    })),
  }
}

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
  materialType: "ROH",
  purchaseType: "f",
  currency: "CNY",
  priceControl: "V",
  priceUnit: 1000,
  movingPrice: 0,
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
    delete (next as any).changeLogs
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
  industrySector: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.materialplant.industrysector') }),
      trigger: 'change'
    }
  ],
  materialGroup: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.materialgroup') }),
      trigger: 'blur'
    }
  ],
  materialType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.materialplant.materialtype') }),
      trigger: 'change'
    }
  ],
  baseUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.materialplant.baseunit') }),
      trigger: 'change'
    }
  ],
  purchaseGroup: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.purchasegroup') }),
      trigger: 'blur'
    }
  ],
  purchaseType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.materialplant.purchasetype') }),
      trigger: 'change'
    }
  ],
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
      if (!Number.isFinite(num) || !Number.isInteger(num) || num < 0) {
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
      if (!Number.isFinite(num) || !Number.isInteger(num) || num < 0) {
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
      if (!Number.isFinite(num) || !Number.isInteger(num) || num < 0) {
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
      if (!Number.isFinite(num) || num < 0) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.inhouseproductiondays') }))
      }
      const scaled = num * 10
      if (Math.abs(scaled - Math.round(scaled)) > 1e-6) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.inhouseproductiondays') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currency: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.materialplant.currency') }),
      trigger: 'change'
    }
  ],
  priceControl: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.materialplant.pricecontrol') }),
      trigger: 'change'
    }
  ],
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
  valuation: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.materialplant.valuation') }),
      trigger: 'change'
    }
  ],
  movingPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.movingprice') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.movingprice') }))
      }
      const scaled = num * 10000
      if (Math.abs(scaled - Math.round(scaled)) > 1e-4) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.movingprice') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  profitCenter: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.profitcenter') }),
      trigger: 'blur'
    }
  ],
  currentStock: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.currentstock') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.currentstock') }))
      }
      const scaled = num * 10000
      if (Math.abs(scaled - Math.round(scaled)) > 1e-4) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.currentstock') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  productionLocation: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.productionlocation') }),
      trigger: 'blur'
    }
  ],
  purchasingLocation: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.purchasinglocation') }),
      trigger: 'blur'
    }
  ],
  storageLocation: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.materialplant.storagelocation') }),
      trigger: 'blur'
    }
  ],
  isInspection: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.isinspection') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.materialplant.isinspection') }))
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
  isEndOfLife: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.materialplant.isendoflife') }),
      trigger: 'change'
    }
  ],
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
  await materialPlantChangeLogTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
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
    const n = typeof rawminOrderQuantity === 'number' ? rawminOrderQuantity : Number(rawminOrderQuantity)
    payload.minOrderQuantity = Number.isFinite(n) ? Math.trunc(n) : 0
  }
  if ('roundingValue' in payload) {
    const rawroundingValue = payload.roundingValue
    const n = typeof rawroundingValue === 'number' ? rawroundingValue : Number(rawroundingValue)
    payload.roundingValue = Number.isFinite(n) ? Math.trunc(n) : 0
  }
  if ('plannedDeliveryTimeDays' in payload) {
    const rawplannedDeliveryTimeDays = payload.plannedDeliveryTimeDays
    const n = typeof rawplannedDeliveryTimeDays === 'number' ? rawplannedDeliveryTimeDays : Number(rawplannedDeliveryTimeDays)
    payload.plannedDeliveryTimeDays = Number.isFinite(n) ? Math.trunc(n) : 0
  }
  if ('inHouseProductionDays' in payload) {
    const rawinHouseProductionDays = payload.inHouseProductionDays
    const n = typeof rawinHouseProductionDays === 'number' ? rawinHouseProductionDays : Number(rawinHouseProductionDays)
    payload.inHouseProductionDays = Number.isFinite(n) ? Math.round(n * 10) / 10 : 0
  }
  if ('priceUnit' in payload) {
    const rawpriceUnit = payload.priceUnit
    payload.priceUnit = typeof rawpriceUnit === 'number' ? rawpriceUnit : Number(rawpriceUnit)
  }
  if ('movingPrice' in payload) {
    const rawmovingPrice = payload.movingPrice
    const n = typeof rawmovingPrice === 'number' ? rawmovingPrice : Number(rawmovingPrice)
    payload.movingPrice = Number.isFinite(n) ? Math.round(n * 10000) / 10000 : 0
  }
  if ('currentStock' in payload) {
    const rawcurrentStock = payload.currentStock
    const n = typeof rawcurrentStock === 'number' ? rawcurrentStock : Number(rawcurrentStock)
    payload.currentStock = Number.isFinite(n) ? Math.round(n * 10000) / 10000 : 0
  }
  if ('isInspection' in payload) {
    const rawisInspection = payload.isInspection
    payload.isInspection = typeof rawisInspection === 'number' ? rawisInspection : Number(rawisInspection)
  }
  if ('isBatch' in payload) {
    const rawisBatch = payload.isBatch
    payload.isBatch = typeof rawisBatch === 'number' ? rawisBatch : Number(rawisBatch)
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
  childMaterialPlantChangeLogRows.value = []
  materialPlantChangeLogTableRef.value?.resetRows?.()
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
