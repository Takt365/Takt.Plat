<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/purchase-request/components -->
<!-- 文件名称：purchase-request-form.vue -->
<!-- 功能描述：Takt采购申请实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="purchase-request-form-tabs"
    >
      <!-- 主表 -->
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
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
                :label="t('entity.purchaseRequest.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.code')"
                name="purchaseRequestCode"
              >
                <a-input
                  v-model:value="formState.purchaseRequestCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.requestdate')"
                name="requestDate"
              >
                <a-date-picker
                  v-model:value="formState.requestDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.requestdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.requiredarrivaldate')"
                name="requiredArrivalDate"
              >
                <a-date-picker
                  v-model:value="formState.requiredArrivalDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.requiredarrivaldate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.requestid')"
                name="requestId"
              >
                <a-input
                  v-model:value="formState.requestId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.requestid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.requestby')"
                name="requestBy"
              >
                <a-input
                  v-model:value="formState.requestBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.requestby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.totalquantity')"
                name="totalQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.totalquantity') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.totalamount')"
                name="totalAmount"
              >
                <a-input-number
                  v-model:value="formState.totalAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.totalamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.convertedquantity')"
                name="convertedQuantity"
              >
                <a-input-number
                  v-model:value="formState.convertedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.convertedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.convertedamount')"
                name="convertedAmount"
              >
                <a-input-number
                  v-model:value="formState.convertedAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.convertedamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.requeststatus')"
                name="requestStatus"
              >
                <TaktSelect
                  v-model:value="formState.requestStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.requeststatus') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.convertedstatus')"
                name="convertedStatus"
              >
                <a-input-number
                  v-model:value="formState.convertedStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.convertedstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.flowinstanceid')"
                name="flowInstanceId"
              >
                <a-input
                  v-model:value="formState.flowInstanceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.flowinstanceid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.purchaseRequest.requestreason')"
                name="requestReason"
              >
                <a-input
                  v-model:value="formState.requestReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.requestreason') })"
                  size="small"
                  allow-clear
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
      <!-- 子表：purchaseRequestItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.purchaseRequestItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddPurchaseRequestItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.purchaseRequestItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="purchaseRequestItemFormColumns"
          :data-source="childPurchaseRequestItemRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'materialCode'">
              <a-input
                v-model:value="record.materialCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'materialName'">
              <a-input
                v-model:value="record.materialName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'materialSpecification'">
              <a-input
                v-model:value="record.materialSpecification"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.materialspecification') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'requestUnit'">
              <a-input
                v-model:value="record.requestUnit"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.requestunit') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'requestQuantity'">
              <a-input-number
                v-model:value="record.requestQuantity"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.requestquantity') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'convertedQuantity'">
              <a-input-number
                v-model:value="record.convertedQuantity"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.convertedquantity') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'estimatedUnitPrice'">
              <a-input-number
                v-model:value="record.estimatedUnitPrice"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestItem.estimatedunitprice') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemovePurchaseRequestItemRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：purchaseRequestChangeLog -->
      <a-tab-pane
        key="child-changeLogs"
        :tab="t('entity.purchaseRequestChangeLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddPurchaseRequestChangeLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.purchaseRequestChangeLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="purchaseRequestChangeLogFormColumns"
          :data-source="childPurchaseRequestChangeLogRows"
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
            <template v-else-if="column.key === 'requestCode'">
              <a-input
                v-model:value="record.requestCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestChangeLog.requestcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeFields'">
              <a-input
                v-model:value="record.changeFields"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestChangeLog.changefields') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeTime'">
              <a-input
                v-model:value="record.changeTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestChangeLog.changetime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeBy'">
              <a-input
                v-model:value="record.changeBy"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestChangeLog.changeby') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeReason'">
              <a-input
                v-model:value="record.changeReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.purchaseRequestChangeLog.changereason') })"
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
              <a-button type="link" danger size="small" @click="handleRemovePurchaseRequestChangeLogRow(index)">
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
 * Takt采购申请实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/purchase-request/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PurchaseRequestCreate, PurchaseRequestItemCreate, PurchaseRequestItem, PurchaseRequestChangeLogCreate, PurchaseRequestChangeLog } from '@/types/logistics/materials/purchase-request'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","purchaseRequestCode","requestDate","requiredArrivalDate","requestId","requestBy","totalQuantity","totalAmount","convertedQuantity","convertedAmount","requestStatus","convertedStatus","flowInstanceId","requestReason","extFieldJson","remark"]

/** purchaseRequestItem 子表行（表单 Tab 内嵌） */
const childPurchaseRequestItemRows = ref<Record<string, unknown>[]>([])
/** purchaseRequestChangeLog 子表行（表单 Tab 内嵌） */
const childPurchaseRequestChangeLogRows = ref<Record<string, unknown>[]>([])

/** 子表 purchaseRequestItem 表单列定义 */
const purchaseRequestItemFormColumns = computed(() => [
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
    title: t('entity.purchaseRequestItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestItem.materialspecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestItem.requestunit'),
    dataIndex: 'requestUnit',
    key: 'requestUnit',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestItem.requestquantity'),
    dataIndex: 'requestQuantity',
    key: 'requestQuantity',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestItem.convertedquantity'),
    dataIndex: 'convertedQuantity',
    key: 'convertedQuantity',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestItem.estimatedunitprice'),
    dataIndex: 'estimatedUnitPrice',
    key: 'estimatedUnitPrice',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 purchaseRequestChangeLog 表单列定义 */
const purchaseRequestChangeLogFormColumns = computed(() => [
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
    title: t('entity.purchaseRequestChangeLog.requestcode'),
    dataIndex: 'requestCode',
    key: 'requestCode',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    width: 140,
  },
  {
    title: t('entity.purchaseRequestChangeLog.changereason'),
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
function syncChildRowsFromFormData(val: Partial<PurchaseRequestCreate & { purchaseRequestId?: string }> | null | undefined) {
  childPurchaseRequestItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.purchaseRequestItemId ?? `new-${index}`,
  }))
  childPurchaseRequestChangeLogRows.value = ((val as any)?.changeLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.purchaseRequestChangeLogId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 purchaseRequestItem 行 */
function handleAddPurchaseRequestItemRow() {
  childPurchaseRequestItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      materialCode: '',
      materialName: '',
      materialSpecification: '',
      requestUnit: '',
      requestQuantity: 0,
      convertedQuantity: 0,
      estimatedUnitPrice: 0,
  })
}

/** 表单 Tab 内删除 purchaseRequestItem 行 */
function handleRemovePurchaseRequestItemRow(index: number) {
  childPurchaseRequestItemRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 purchaseRequestChangeLog 行 */
function handleAddPurchaseRequestChangeLogRow() {
  childPurchaseRequestChangeLogRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      requestCode: '',
      changeFields: '',
      changeTime: '',
      changeBy: '',
      changeReason: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 purchaseRequestChangeLog 行 */
function handleRemovePurchaseRequestChangeLogRow(index: number) {
  childPurchaseRequestChangeLogRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childPurchaseRequestItemRows.value.map(({ __rowKey, ...rest }) => rest),
    changeLogs: childPurchaseRequestChangeLogRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseRequestCreate & { purchaseRequestId?: string }> | null
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
    const isCreate = !props.formData?.purchaseRequestId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.plantcode') }),
      trigger: 'blur'
    }
  ],
  purchaseRequestCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.code') }),
      trigger: 'blur'
    }
  ],
  requestDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.requestdate') }),
      trigger: 'change'
    }
  ],
  requestBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.purchaseRequest.requestby') }),
      trigger: 'blur'
    }
  ],
  totalQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.totalquantity') }),
      trigger: 'change'
    }
  ],
  totalAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.totalamount') }),
      trigger: 'change'
    }
  ],
  convertedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.convertedquantity') }),
      trigger: 'change'
    }
  ],
  convertedAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.convertedamount') }),
      trigger: 'change'
    }
  ],
  requestStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.requeststatus') }),
      trigger: 'change'
    }
  ],
  convertedStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.purchaseRequest.convertedstatus') }),
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
  childPurchaseRequestItemRows.value = []
  childPurchaseRequestChangeLogRows.value = []
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
