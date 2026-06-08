<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey/components -->
<!-- 文件名称：customer-satisfaction-survey-form.vue -->
<!-- 功能描述：客户满意度调查表主表实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="customer-satisfaction-survey-form-tabs"
    >
      <!-- 主表 -->
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
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
                :label="t('entity.customerSatisfactionSurvey.code')"
                name="customerSatisfactionSurveyCode"
              >
                <a-input
                  v-model:value="formState.customerSatisfactionSurveyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.customerid')"
                name="customerId"
              >
                <a-input
                  v-model:value="formState.customerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customerid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.customername')"
                name="customerName"
              >
                <a-input
                  v-model:value="formState.customerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.customercode')"
                name="customerCode"
              >
                <a-input
                  v-model:value="formState.customerCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.surveydate')"
                name="surveyDate"
              >
                <a-date-picker
                  v-model:value="formState.surveyDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.surveydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.surveymethod')"
                name="surveyMethod"
              >
                <a-input-number
                  v-model:value="formState.surveyMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.surveymethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.surveytype')"
                name="surveyType"
              >
                <a-input-number
                  v-model:value="formState.surveyType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.surveytype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.surveyperiod')"
                name="surveyPeriod"
              >
                <a-input-number
                  v-model:value="formState.surveyPeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.surveyperiod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.surveyorby')"
                name="surveyorBy"
              >
                <a-input
                  v-model:value="formState.surveyorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.surveyorby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.customercontact')"
                name="customerContact"
              >
                <a-input
                  v-model:value="formState.customerContact"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customercontact') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.customerphone')"
                name="customerPhone"
              >
                <a-input
                  v-model:value="formState.customerPhone"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customerphone') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.overallsatisfaction')"
                name="overallSatisfaction"
              >
                <a-input-number
                  v-model:value="formState.overallSatisfaction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.overallsatisfaction') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.totalscore')"
                name="totalScore"
              >
                <a-input-number
                  v-model:value="formState.totalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.totalscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.qualityscore')"
                name="qualityScore"
              >
                <a-input-number
                  v-model:value="formState.qualityScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.qualityscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.deliveryscore')"
                name="deliveryScore"
              >
                <a-input-number
                  v-model:value="formState.deliveryScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.deliveryscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.servicescore')"
                name="serviceScore"
              >
                <a-input-number
                  v-model:value="formState.serviceScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.servicescore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.pricescore')"
                name="priceScore"
              >
                <a-input-number
                  v-model:value="formState.priceScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.pricescore') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.technicalscore')"
                name="technicalScore"
              >
                <a-input-number
                  v-model:value="formState.technicalScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.technicalscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.customerpraise')"
                name="customerPraise"
              >
                <a-input
                  v-model:value="formState.customerPraise"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customerpraise') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.customerfeedback')"
                name="customerFeedback"
              >
                <a-input
                  v-model:value="formState.customerFeedback"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customerfeedback') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.improvementplan')"
                name="improvementPlan"
              >
                <a-input
                  v-model:value="formState.improvementPlan"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.improvementplan') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.surveystatus')"
                name="surveyStatus"
              >
                <a-input-number
                  v-model:value="formState.surveyStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.surveystatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.followupstatus')"
                name="followUpStatus"
              >
                <a-input-number
                  v-model:value="formState.followUpStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.followupstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.relatedcomplaintid')"
                name="relatedComplaintId"
              >
                <a-input
                  v-model:value="formState.relatedComplaintId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.relatedcomplaintid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.relatedplant') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.customerSatisfactionSurvey.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.sortorder') })"
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
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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
      <!-- 子表：customerSatisfactionSurveyItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.customerSatisfactionSurveyItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddCustomerSatisfactionSurveyItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.customerSatisfactionSurveyItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="customerSatisfactionSurveyItemFormColumns"
          :data-source="childCustomerSatisfactionSurveyItemRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                size="small"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'surveyId'">
              <a-input
                v-model:value="record.surveyId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.surveyid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'lineNumber'">
              <a-input-number
                v-model:value="record.lineNumber"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'categoryType'">
              <a-input-number
                v-model:value="record.categoryType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.categorytype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'itemName'">
              <a-input
                v-model:value="record.itemName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.itemname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'itemDescription'">
              <a-textarea
                v-model:value="record.itemDescription"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customerSatisfactionSurveyItem.itemdescription') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === 'weight'">
              <a-input-number
                v-model:value="record.weight"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.weight') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'score'">
              <a-input-number
                v-model:value="record.score"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.score') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'satisfactionLevel'">
              <a-input-number
                v-model:value="record.satisfactionLevel"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurveyItem.satisfactionlevel') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveCustomerSatisfactionSurveyItemRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 客户满意度调查表主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-satisfaction-survey/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { CustomerSatisfactionSurveyCreate, CustomerSatisfactionSurveyItemCreate, CustomerSatisfactionSurveyItem } from '@/types/logistics/quality/complaint/customer-satisfaction-survey'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","customerSatisfactionSurveyCode","customerId","customerName","customerCode","surveyDate","surveyMethod","surveyType","surveyPeriod","surveyorBy","customerContact","customerPhone","overallSatisfaction","totalScore","qualityScore","deliveryScore","serviceScore","priceScore","technicalScore","customerPraise","customerFeedback","improvementPlan","surveyStatus","followUpStatus","relatedComplaintId","relatedPlant","sortOrder","extFieldJson","remark"]

/** customerSatisfactionSurveyItem 子表行（表单 Tab 内嵌） */
const childCustomerSatisfactionSurveyItemRows = ref<Record<string, unknown>[]>([])

/** 子表 customerSatisfactionSurveyItem 表单列定义 */
const customerSatisfactionSurveyItemFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.surveyid'),
    dataIndex: 'surveyId',
    key: 'surveyId',
    width: 140,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.categorytype'),
    dataIndex: 'categoryType',
    key: 'categoryType',
    width: 140,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    width: 140,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.itemdescription'),
    dataIndex: 'itemDescription',
    key: 'itemDescription',
    width: 140,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.weight'),
    dataIndex: 'weight',
    key: 'weight',
    width: 140,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.score'),
    dataIndex: 'score',
    key: 'score',
    width: 140,
  },
  {
    title: t('entity.customerSatisfactionSurveyItem.satisfactionlevel'),
    dataIndex: 'satisfactionLevel',
    key: 'satisfactionLevel',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<CustomerSatisfactionSurveyCreate & { customerSatisfactionSurveyId?: string }> | null | undefined) {
  childCustomerSatisfactionSurveyItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.customerSatisfactionSurveyItemId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 customerSatisfactionSurveyItem 行 */
function handleAddCustomerSatisfactionSurveyItemRow() {
  childCustomerSatisfactionSurveyItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      surveyId: '',
      lineNumber: 0,
      categoryType: 0,
      itemName: '',
      itemDescription: '',
      weight: 0,
      score: 0,
      satisfactionLevel: 0,
  })
}

/** 表单 Tab 内删除 customerSatisfactionSurveyItem 行 */
function handleRemoveCustomerSatisfactionSurveyItemRow(index: number) {
  childCustomerSatisfactionSurveyItemRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childCustomerSatisfactionSurveyItemRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CustomerSatisfactionSurveyCreate & { customerSatisfactionSurveyId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

/** 编辑态灌入 formData；新增态 reset */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
    applyScopeDefaults(next)
    Object.assign(formState, next)
    syncChildRowsFromFormData(val)
  },
  { immediate: true, deep: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.customerSatisfactionSurveyId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  customerSatisfactionSurveyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.code') }),
      trigger: 'blur'
    }
  ],
  customerId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customerid') }),
      trigger: 'blur'
    }
  ],
  customerName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.customerSatisfactionSurvey.customername') }),
      trigger: 'blur'
    }
  ],
  surveyDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.surveydate') }),
      trigger: 'change'
    }
  ],
  surveyMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.surveymethod') }),
      trigger: 'change'
    }
  ],
  surveyType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.surveytype') }),
      trigger: 'change'
    }
  ],
  surveyPeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.surveyperiod') }),
      trigger: 'change'
    }
  ],
  overallSatisfaction: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.overallsatisfaction') }),
      trigger: 'change'
    }
  ],
  surveyStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.surveystatus') }),
      trigger: 'change'
    }
  ],
  followUpStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.followupstatus') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.customerSatisfactionSurvey.sortorder') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  return buildSubmitPayload()
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
  childCustomerSatisfactionSurveyItemRows.value = []
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
