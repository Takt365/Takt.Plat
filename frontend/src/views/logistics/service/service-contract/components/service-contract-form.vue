<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/service/service-contract/components -->
<!-- 文件名称：service-contract-form.vue -->
<!-- 功能描述：服务合同实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="service-contract-form-tabs"
    >
      <!-- 主表 -->
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
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
                :label="t('entity.serviceContract.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.code')"
                name="serviceContractCode"
              >
                <a-input
                  v-model:value="formState.serviceContractCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.contractname')"
                name="contractName"
              >
                <a-input
                  v-model:value="formState.contractName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.clientid')"
                name="clientId"
              >
                <a-input
                  v-model:value="formState.clientId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.clientcode')"
                name="clientCode"
              >
                <a-input
                  v-model:value="formState.clientCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.clientname')"
                name="clientName"
              >
                <a-input
                  v-model:value="formState.clientName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.contracttype')"
                name="contractType"
              >
                <a-input-number
                  v-model:value="formState.contractType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contracttype') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.contractstatus')"
                name="contractStatus"
              >
                <a-input-number
                  v-model:value="formState.contractStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.signdate')"
                name="signDate"
              >
                <a-date-picker
                  v-model:value="formState.signDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.signdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.effectivedate')"
                name="effectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.effectiveDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.effectivedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.expirydate')"
                name="expiryDate"
              >
                <a-date-picker
                  v-model:value="formState.expiryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceContract.expirydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.contractamount')"
                name="contractAmount"
              >
                <a-input-number
                  v-model:value="formState.contractAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.currencycode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.currencycode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.paymentterms')"
                name="paymentTerms"
              >
                <a-input-number
                  v-model:value="formState.paymentTerms"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.paymentterms') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.serviceContract.servicescope')"
                name="serviceScope"
              >
                <a-textarea
                  v-model:value="formState.serviceScope"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.serviceContract.servicescope') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.slaresponsehours')"
                name="slaResponseHours"
              >
                <a-input-number
                  v-model:value="formState.slaResponseHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.slaresponsehours') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.slaresolvehours')"
                name="slaResolveHours"
              >
                <a-input-number
                  v-model:value="formState.slaResolveHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.slaresolvehours') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.accountmanager')"
                name="accountManager"
              >
                <a-input
                  v-model:value="formState.accountManager"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.accountmanager') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serviceContract.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceContract.sortorder') })"
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
      <!-- 子表：serviceOrder -->
      <a-tab-pane
        key="child-serviceOrders"
        :tab="t('entity.serviceOrder._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddServiceOrderRow">
            {{ t('common.page.button.create') }}{{ t('entity.serviceOrder._self') }}
          </a-button>
        </div>
        <a-table
          :columns="serviceOrderFormColumns"
          :data-source="childServiceOrderRows"
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
            <template v-else-if="column.key === 'plantCode'">
              <a-input
                v-model:value="record.plantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceOrder.plantcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'serviceOrderCode'">
              <a-input
                v-model:value="record.serviceOrderCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceOrder.code') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'clientId'">
              <a-input
                v-model:value="record.clientId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceOrder.clientid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'clientCode'">
              <a-input
                v-model:value="record.clientCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceOrder.clientcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'clientName'">
              <a-input
                v-model:value="record.clientName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceOrder.clientname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'serviceRequestId'">
              <a-input
                v-model:value="record.serviceRequestId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceOrder.servicerequestid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'serviceRequestCode'">
              <a-input
                v-model:value="record.serviceRequestCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceOrder.servicerequestcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'orderDate'">
              <a-date-picker
                v-model:value="record.orderDate"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceOrder.orderdate') })"
                value-format="YYYY-MM-DD"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveServiceOrderRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：serviceRequest -->
      <a-tab-pane
        key="child-serviceRequests"
        :tab="t('entity.serviceRequest._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddServiceRequestRow">
            {{ t('common.page.button.create') }}{{ t('entity.serviceRequest._self') }}
          </a-button>
        </div>
        <a-table
          :columns="serviceRequestFormColumns"
          :data-source="childServiceRequestRows"
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
            <template v-else-if="column.key === 'plantCode'">
              <a-input
                v-model:value="record.plantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceRequest.plantcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'serviceRequestCode'">
              <a-input
                v-model:value="record.serviceRequestCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceRequest.code') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'clientId'">
              <a-input
                v-model:value="record.clientId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceRequest.clientid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'clientCode'">
              <a-input
                v-model:value="record.clientCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceRequest.clientcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'clientName'">
              <a-input
                v-model:value="record.clientName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceRequest.clientname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'requestDate'">
              <a-date-picker
                v-model:value="record.requestDate"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceRequest.requestdate') })"
                value-format="YYYY-MM-DD"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'expectedServiceDate'">
              <a-date-picker
                v-model:value="record.expectedServiceDate"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serviceRequest.expectedservicedate') })"
                value-format="YYYY-MM-DD"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'requestType'">
              <a-input-number
                v-model:value="record.requestType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serviceRequest.requesttype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveServiceRequestRow(index)">
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
 * 服务合同实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/service/service-contract/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ServiceContractCreate, ServiceOrderCreate, ServiceOrder, ServiceRequestCreate, ServiceRequest } from '@/types/logistics/customer-service/service-contract'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","serviceContractCode","contractName","clientId","clientCode","clientName","contractType","contractStatus","signDate","effectiveDate","expiryDate","contractAmount","currencyCode","paymentTerms","serviceScope","slaResponseHours","slaResolveHours","accountManager","sortOrder","extFieldJson","remark"]

/** serviceOrder 子表行（表单 Tab 内嵌） */
const childServiceOrderRows = ref<Record<string, unknown>[]>([])
/** serviceRequest 子表行（表单 Tab 内嵌） */
const childServiceRequestRows = ref<Record<string, unknown>[]>([])

/** 子表 serviceOrder 表单列定义 */
const serviceOrderFormColumns = computed(() => [
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
    title: t('entity.serviceOrder.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 140,
  },
  {
    title: t('entity.serviceOrder.code'),
    dataIndex: 'serviceOrderCode',
    key: 'serviceOrderCode',
    width: 140,
  },
  {
    title: t('entity.serviceOrder.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    width: 140,
  },
  {
    title: t('entity.serviceOrder.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    width: 140,
  },
  {
    title: t('entity.serviceOrder.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    width: 140,
  },
  {
    title: t('entity.serviceOrder.servicerequestid'),
    dataIndex: 'serviceRequestId',
    key: 'serviceRequestId',
    width: 140,
  },
  {
    title: t('entity.serviceOrder.servicerequestcode'),
    dataIndex: 'serviceRequestCode',
    key: 'serviceRequestCode',
    width: 140,
  },
  {
    title: t('entity.serviceOrder.orderdate'),
    dataIndex: 'orderDate',
    key: 'orderDate',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 serviceRequest 表单列定义 */
const serviceRequestFormColumns = computed(() => [
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
    title: t('entity.serviceRequest.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 140,
  },
  {
    title: t('entity.serviceRequest.code'),
    dataIndex: 'serviceRequestCode',
    key: 'serviceRequestCode',
    width: 140,
  },
  {
    title: t('entity.serviceRequest.clientid'),
    dataIndex: 'clientId',
    key: 'clientId',
    width: 140,
  },
  {
    title: t('entity.serviceRequest.clientcode'),
    dataIndex: 'clientCode',
    key: 'clientCode',
    width: 140,
  },
  {
    title: t('entity.serviceRequest.clientname'),
    dataIndex: 'clientName',
    key: 'clientName',
    width: 140,
  },
  {
    title: t('entity.serviceRequest.requestdate'),
    dataIndex: 'requestDate',
    key: 'requestDate',
    width: 140,
  },
  {
    title: t('entity.serviceRequest.expectedservicedate'),
    dataIndex: 'expectedServiceDate',
    key: 'expectedServiceDate',
    width: 140,
  },
  {
    title: t('entity.serviceRequest.requesttype'),
    dataIndex: 'requestType',
    key: 'requestType',
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
function syncChildRowsFromFormData(val: Partial<ServiceContractCreate & { serviceContractId?: string }> | null | undefined) {
  childServiceOrderRows.value = ((val as any)?.serviceOrders ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.serviceOrderId ?? `new-${index}`,
  }))
  childServiceRequestRows.value = ((val as any)?.serviceRequests ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.serviceRequestId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 serviceOrder 行 */
function handleAddServiceOrderRow() {
  childServiceOrderRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      plantCode: '',
      serviceOrderCode: '',
      clientId: '',
      clientCode: '',
      clientName: '',
      serviceRequestId: '',
      serviceRequestCode: '',
      orderDate: '',
  })
}

/** 表单 Tab 内删除 serviceOrder 行 */
function handleRemoveServiceOrderRow(index: number) {
  childServiceOrderRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 serviceRequest 行 */
function handleAddServiceRequestRow() {
  childServiceRequestRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      plantCode: '',
      serviceRequestCode: '',
      clientId: '',
      clientCode: '',
      clientName: '',
      requestDate: '',
      expectedServiceDate: '',
      requestType: 0,
  })
}

/** 表单 Tab 内删除 serviceRequest 行 */
function handleRemoveServiceRequestRow(index: number) {
  childServiceRequestRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    serviceOrders: childServiceOrderRows.value.map(({ __rowKey, ...rest }) => rest),
    serviceRequests: childServiceRequestRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ServiceContractCreate & { serviceContractId?: string }> | null
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
    delete (next as any).serviceOrders
    delete (next as any).serviceRequests
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
    const isCreate = !props.formData?.serviceContractId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.plantcode') }),
      trigger: 'blur'
    }
  ],
  serviceContractCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.code') }),
      trigger: 'blur'
    }
  ],
  contractName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.contractname') }),
      trigger: 'blur'
    }
  ],
  clientId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientid') }),
      trigger: 'blur'
    }
  ],
  clientCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientcode') }),
      trigger: 'blur'
    }
  ],
  clientName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.clientname') }),
      trigger: 'blur'
    }
  ],
  contractType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.contracttype') }),
      trigger: 'change'
    }
  ],
  contractStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.contractstatus') }),
      trigger: 'change'
    }
  ],
  effectiveDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.effectivedate') }),
      trigger: 'change'
    }
  ],
  contractAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.contractamount') }),
      trigger: 'change'
    }
  ],
  currencyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serviceContract.currencycode') }),
      trigger: 'blur'
    }
  ],
  paymentTerms: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.paymentterms') }),
      trigger: 'change'
    }
  ],
  slaResponseHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.slaresponsehours') }),
      trigger: 'change'
    }
  ],
  slaResolveHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.slaresolvehours') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serviceContract.sortorder') }),
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
  childServiceOrderRows.value = []
  childServiceRequestRows.value = []
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
