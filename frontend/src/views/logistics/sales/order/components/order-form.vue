<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-order/components -->
<!-- 文件名称：sales-order-form.vue -->
<!-- 功能描述：Takt销售订单实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="sales-order-form-tabs"
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
                :label="t('entity.salesOrder.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.code')"
                name="salesOrderCode"
              >
                <a-input
                  v-model:value="formState.salesOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.customercode')"
                name="customerCode"
              >
                <a-input
                  v-model:value="formState.customerCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.customercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.customername')"
                name="customerName"
              >
                <a-input
                  v-model:value="formState.customerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.customername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.orderdate')"
                name="orderDate"
              >
                <a-date-picker
                  v-model:value="formState.orderDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.orderdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.requireddeliverydate')"
                name="requiredDeliveryDate"
              >
                <a-date-picker
                  v-model:value="formState.requiredDeliveryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.requireddeliverydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.actualdeliverydate')"
                name="actualDeliveryDate"
              >
                <a-date-picker
                  v-model:value="formState.actualDeliveryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.actualdeliverydate') })"
                  value-format="YYYY-MM-DD"
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
                :label="t('entity.salesOrder.salesby')"
                name="salesBy"
              >
                <a-input
                  v-model:value="formState.salesBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.salesby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.totalquantity')"
                name="totalQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.totalquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.totalamount')"
                name="totalAmount"
              >
                <a-input-number
                  v-model:value="formState.totalAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.totalamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.discountamount')"
                name="discountAmount"
              >
                <a-input-number
                  v-model:value="formState.discountAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.discountamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.taxamount')"
                name="taxAmount"
              >
                <a-input-number
                  v-model:value="formState.taxAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.taxamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.actualamount')"
                name="actualAmount"
              >
                <a-input-number
                  v-model:value="formState.actualAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.actualamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.shippedquantity')"
                name="shippedQuantity"
              >
                <a-input-number
                  v-model:value="formState.shippedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.shippedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.shippedamount')"
                name="shippedAmount"
              >
                <a-input-number
                  v-model:value="formState.shippedAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.shippedamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.receivedamount')"
                name="receivedAmount"
              >
                <a-input-number
                  v-model:value="formState.receivedAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.receivedamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.orderstatus')"
                name="orderStatus"
              >
                <TaktSelect
                  v-model:value="formState.orderStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salesOrder.orderstatus') })"
                  size="small"
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
                :label="t('entity.salesOrder.deliverystatus')"
                name="deliveryStatus"
              >
                <a-input-number
                  v-model:value="formState.deliveryStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.deliverystatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.deliverymethod')"
                name="deliveryMethod"
              >
                <a-input-number
                  v-model:value="formState.deliveryMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.deliverymethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesOrder.paymentmethod')"
                name="paymentMethod"
              >
                <a-input-number
                  v-model:value="formState.paymentMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrder.paymentmethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.salesOrder.deliveryaddress')"
                name="deliveryAddress"
              >
                <a-textarea
                  v-model:value="formState.deliveryAddress"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.salesOrder.deliveryaddress') })"
                  :rows="2"
                  size="small"
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
      <!-- 子表：salesOrderItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.salesOrderItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddSalesOrderItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.salesOrderItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="salesOrderItemFormColumns"
          :data-source="childSalesOrderItemRows"
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
            <template v-else-if="column.key === 'lineNumber'">
              <a-input-number
                v-model:value="record.lineNumber"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'materialCode'">
              <a-input
                v-model:value="record.materialCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderItem.materialcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'materialName'">
              <a-input
                v-model:value="record.materialName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderItem.materialname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'materialSpecification'">
              <a-input
                v-model:value="record.materialSpecification"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderItem.materialspecification') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'salesUnit'">
              <a-input
                v-model:value="record.salesUnit"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderItem.salesunit') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'orderQuantity'">
              <a-input-number
                v-model:value="record.orderQuantity"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderItem.orderquantity') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'shippedQuantity'">
              <a-input-number
                v-model:value="record.shippedQuantity"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderItem.shippedquantity') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'unitPrice'">
              <a-input-number
                v-model:value="record.unitPrice"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderItem.unitprice') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveSalesOrderItemRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：salesOrderChangeLog -->
      <a-tab-pane
        key="child-changeLogs"
        :tab="t('entity.salesOrderChangeLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddSalesOrderChangeLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.salesOrderChangeLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="salesOrderChangeLogFormColumns"
          :data-source="childSalesOrderChangeLogRows"
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
            <template v-else-if="column.key === 'orderCode'">
              <a-input
                v-model:value="record.orderCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderChangeLog.ordercode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeFields'">
              <a-input
                v-model:value="record.changeFields"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderChangeLog.changefields') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeTime'">
              <a-input
                v-model:value="record.changeTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderChangeLog.changetime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeBy'">
              <a-input
                v-model:value="record.changeBy"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderChangeLog.changeby') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeReason'">
              <a-input
                v-model:value="record.changeReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesOrderChangeLog.changereason') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'extFieldJson'">
              <a-input
                v-model:value="record.extFieldJson"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'remark'">
              <a-textarea
                v-model:value="record.remark"
                :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                :rows="2"
                size="small"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveSalesOrderChangeLogRow(index)">
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
 * Takt销售订单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/sales-order/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SalesOrderCreate, SalesOrderItemCreate, SalesOrderItem, SalesOrderChangeLogCreate, SalesOrderChangeLog } from '@/types/logistics/sales/sales-order'
import TaktSelect from '@/components/business/takt-select/index.vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","salesOrderCode","customerCode","customerName","orderDate","requiredDeliveryDate","actualDeliveryDate","salesBy","totalQuantity","totalAmount","discountAmount","taxAmount","actualAmount","shippedQuantity","shippedAmount","receivedAmount","orderStatus","deliveryStatus","deliveryMethod","paymentMethod","deliveryAddress","extFieldJson","remark"]

/** salesOrderItem 子表行（表单 Tab 内嵌） */
const childSalesOrderItemRows = ref<Record<string, unknown>[]>([])
/** salesOrderChangeLog 子表行（表单 Tab 内嵌） */
const childSalesOrderChangeLogRows = ref<Record<string, unknown>[]>([])

/** 子表 salesOrderItem 表单列定义 */
const salesOrderItemFormColumns = computed(() => [
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
    title: t('entity.salesOrderItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.salesOrderItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 140,
  },
  {
    title: t('entity.salesOrderItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 140,
  },
  {
    title: t('entity.salesOrderItem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 140,
  },
  {
    title: t('entity.salesOrderItem.salesunit'),
    dataIndex: 'salesUnit',
    key: 'salesUnit',
    width: 140,
  },
  {
    title: t('entity.salesOrderItem.orderquantity'),
    dataIndex: 'orderQuantity',
    key: 'orderQuantity',
    width: 140,
  },
  {
    title: t('entity.salesOrderItem.shippedquantity'),
    dataIndex: 'shippedQuantity',
    key: 'shippedQuantity',
    width: 140,
  },
  {
    title: t('entity.salesOrderItem.unitprice'),
    dataIndex: 'unitPrice',
    key: 'unitPrice',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 salesOrderChangeLog 表单列定义 */
const salesOrderChangeLogFormColumns = computed(() => [
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
    title: t('entity.salesOrderChangeLog.ordercode'),
    dataIndex: 'orderCode',
    key: 'orderCode',
    width: 140,
  },
  {
    title: t('entity.salesOrderChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 140,
  },
  {
    title: t('entity.salesOrderChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    width: 140,
  },
  {
    title: t('entity.salesOrderChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    width: 140,
  },
  {
    title: t('entity.salesOrderChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 140,
  },
  {
    title: t('common.page.entity.extfieldjson'),
    dataIndex: 'extFieldJson',
    key: 'extFieldJson',
    width: 140,
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
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
function syncChildRowsFromFormData(val: Partial<SalesOrderCreate & { salesOrderId?: string }> | null | undefined) {
  childSalesOrderItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.salesOrderItemId ?? `new-${index}`,
  }))
  childSalesOrderChangeLogRows.value = ((val as any)?.changeLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.salesOrderChangeLogId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 salesOrderItem 行 */
function handleAddSalesOrderItemRow() {
  childSalesOrderItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      materialCode: '',
      materialName: '',
      materialSpecification: '',
      salesUnit: '',
      orderQuantity: 0,
      shippedQuantity: 0,
      unitPrice: 0,
  })
}

/** 表单 Tab 内删除 salesOrderItem 行 */
function handleRemoveSalesOrderItemRow(index: number) {
  childSalesOrderItemRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 salesOrderChangeLog 行 */
function handleAddSalesOrderChangeLogRow() {
  childSalesOrderChangeLogRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      orderCode: '',
      changeFields: '',
      changeTime: '',
      changeBy: '',
      changeReason: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 salesOrderChangeLog 行 */
function handleRemoveSalesOrderChangeLogRow(index: number) {
  childSalesOrderChangeLogRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childSalesOrderItemRows.value.map(({ __rowKey, ...rest }) => rest),
    changeLogs: childSalesOrderChangeLogRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesOrderCreate & { salesOrderId?: string }> | null
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
    delete (next as any).changeLogs
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
    const isCreate = !props.formData?.salesOrderId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  salesOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salesOrder.code') }),
      trigger: 'blur'
    }
  ],
  customerCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salesOrder.customercode') }),
      trigger: 'blur'
    }
  ],
  customerName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salesOrder.customername') }),
      trigger: 'blur'
    }
  ],
  orderDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.orderdate') }),
      trigger: 'change'
    }
  ],
  totalQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.totalquantity') }),
      trigger: 'change'
    }
  ],
  totalAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.totalamount') }),
      trigger: 'change'
    }
  ],
  discountAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.discountamount') }),
      trigger: 'change'
    }
  ],
  taxAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.taxamount') }),
      trigger: 'change'
    }
  ],
  actualAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.actualamount') }),
      trigger: 'change'
    }
  ],
  shippedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.shippedquantity') }),
      trigger: 'change'
    }
  ],
  shippedAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.shippedamount') }),
      trigger: 'change'
    }
  ],
  receivedAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.receivedamount') }),
      trigger: 'change'
    }
  ],
  orderStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.orderstatus') }),
      trigger: 'change'
    }
  ],
  deliveryStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.deliverystatus') }),
      trigger: 'change'
    }
  ],
  deliveryMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.deliverymethod') }),
      trigger: 'change'
    }
  ],
  paymentMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesOrder.paymentmethod') }),
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
  childSalesOrderItemRows.value = []
  childSalesOrderChangeLogRows.value = []
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
