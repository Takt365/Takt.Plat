<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order/components -->
<!-- 文件名称：ipqc-order-form.vue -->
<!-- 功能描述：IPQC制程检验单实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="ipqc-order-form-tabs"
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
                :label="t('entity.ipqcOrder.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.sourcecode')"
                name="sourceCode"
              >
                <a-input
                  v-model:value="formState.sourceCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.sourcecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.inspectiondate')"
                name="inspectionDate"
              >
                <a-date-picker
                  v-model:value="formState.inspectionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.inspectiondate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.code')"
                name="ipqcOrderCode"
              >
                <a-input
                  v-model:value="formState.ipqcOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.processcode')"
                name="processCode"
              >
                <a-input
                  v-model:value="formState.processCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.processname')"
                name="processName"
              >
                <a-input
                  v-model:value="formState.processName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.totalproductionquantity')"
                name="totalProductionQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalProductionQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalproductionquantity') })"
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
                :label="t('entity.ipqcOrder.totalsamplequantity')"
                name="totalSampleQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalSampleQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalsamplequantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.totalqualifiedquantity')"
                name="totalQualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalqualifiedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.totalunqualifiedquantity')"
                name="totalUnqualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalUnqualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalunqualifiedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.totalinspectionreturnquantity')"
                name="totalInspectionReturnQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalInspectionReturnQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalinspectionreturnquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.judgestatus')"
                name="judgeStatus"
              >
                <a-input-number
                  v-model:value="formState.judgeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.judgestatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.judgeby')"
                name="judgeBy"
              >
                <a-input
                  v-model:value="formState.judgeBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.judgeby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.judgedate')"
                name="judgeDate"
              >
                <a-date-picker
                  v-model:value="formState.judgeDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.judgedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ipqcOrder.judgedescription')"
                name="judgeDescription"
              >
                <a-textarea
                  v-model:value="formState.judgeDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcOrder.judgedescription') })"
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
      <!-- 子表：ipqcOrderItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.ipqcOrderItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddIpqcOrderItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.ipqcOrderItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="ipqcOrderItemFormColumns"
          :data-source="childIpqcOrderItemRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'materialCode'">
              <a-input
                v-model:value="record.materialCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.materialcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'materialName'">
              <a-input
                v-model:value="record.materialName"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.materialname') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'batchNo'">
              <a-input
                v-model:value="record.batchNo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.batchno') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'productionQuantity'">
              <a-input-number
                v-model:value="record.productionQuantity"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.productionquantity') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'standardCode'">
              <a-input
                v-model:value="record.standardCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.standardcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'samplingSchemeCode'">
              <a-input
                v-model:value="record.samplingSchemeCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.samplingschemecode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'inspectionMethod'">
              <a-input-number
                v-model:value="record.inspectionMethod"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.inspectionmethod') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveIpqcOrderItemRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：ipqcOrderChangeLog -->
      <a-tab-pane
        key="child-changeLogs"
        :tab="t('entity.ipqcOrderChangeLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddIpqcOrderChangeLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.ipqcOrderChangeLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="ipqcOrderChangeLogFormColumns"
          :data-source="childIpqcOrderChangeLogRows"
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
            <template v-else-if="column.key === 'changeFields'">
              <a-input
                v-model:value="record.changeFields"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderChangeLog.changefields') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeType'">
              <a-input-number
                v-model:value="record.changeType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderChangeLog.changetype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'changeReason'">
              <a-input
                v-model:value="record.changeReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderChangeLog.changereason') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeBy'">
              <a-input
                v-model:value="record.changeBy"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderChangeLog.changeby') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeTime'">
              <a-input
                v-model:value="record.changeTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderChangeLog.changetime') })"
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
              <a-button type="link" danger size="small" @click="handleRemoveIpqcOrderChangeLogRow(index)">
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
 * IPQC制程检验单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/ipqc-order/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { IpqcOrderCreate, IpqcOrderItemCreate, IpqcOrderItem, IpqcOrderChangeLogCreate, IpqcOrderChangeLog } from '@/types/logistics/quality/operation/ipqc-order'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","sourceCode","inspectionDate","ipqcOrderCode","processCode","processName","totalProductionQuantity","totalSampleQuantity","totalQualifiedQuantity","totalUnqualifiedQuantity","totalInspectionReturnQuantity","judgeStatus","judgeBy","judgeDate","judgeDescription","extFieldJson","remark"]

/** ipqcOrderItem 子表行（表单 Tab 内嵌） */
const childIpqcOrderItemRows = ref<Record<string, unknown>[]>([])
/** ipqcOrderChangeLog 子表行（表单 Tab 内嵌） */
const childIpqcOrderChangeLogRows = ref<Record<string, unknown>[]>([])

/** 子表 ipqcOrderItem 表单列定义 */
const ipqcOrderItemFormColumns = computed(() => [
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
    title: t('entity.ipqcOrderItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderItem.materialname'),
    dataIndex: 'materialName',
    key: 'materialName',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderItem.batchno'),
    dataIndex: 'batchNo',
    key: 'batchNo',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderItem.productionquantity'),
    dataIndex: 'productionQuantity',
    key: 'productionQuantity',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderItem.standardcode'),
    dataIndex: 'standardCode',
    key: 'standardCode',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderItem.samplingschemecode'),
    dataIndex: 'samplingSchemeCode',
    key: 'samplingSchemeCode',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderItem.inspectionmethod'),
    dataIndex: 'inspectionMethod',
    key: 'inspectionMethod',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 ipqcOrderChangeLog 表单列定义 */
const ipqcOrderChangeLogFormColumns = computed(() => [
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
    title: t('entity.ipqcOrderChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    width: 140,
  },
  {
    title: t('entity.ipqcOrderChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
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
function syncChildRowsFromFormData(val: Partial<IpqcOrderCreate & { ipqcOrderId?: string }> | null | undefined) {
  childIpqcOrderItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.ipqcOrderItemId ?? `new-${index}`,
  }))
  childIpqcOrderChangeLogRows.value = ((val as any)?.changeLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.ipqcOrderChangeLogId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 ipqcOrderItem 行 */
function handleAddIpqcOrderItemRow() {
  childIpqcOrderItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      materialCode: '',
      materialName: '',
      batchNo: '',
      productionQuantity: 0,
      standardCode: '',
      samplingSchemeCode: '',
      inspectionMethod: 0,
  })
}

/** 表单 Tab 内删除 ipqcOrderItem 行 */
function handleRemoveIpqcOrderItemRow(index: number) {
  childIpqcOrderItemRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 ipqcOrderChangeLog 行 */
function handleAddIpqcOrderChangeLogRow() {
  childIpqcOrderChangeLogRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      changeFields: '',
      changeType: 0,
      changeReason: '',
      changeBy: '',
      changeTime: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 ipqcOrderChangeLog 行 */
function handleRemoveIpqcOrderChangeLogRow(index: number) {
  childIpqcOrderChangeLogRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childIpqcOrderItemRows.value.map(({ __rowKey, ...rest }) => rest),
    changeLogs: childIpqcOrderChangeLogRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<IpqcOrderCreate & { ipqcOrderId?: string }> | null
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
    const isCreate = !props.formData?.ipqcOrderId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.plantcode') }),
      trigger: 'blur'
    }
  ],
  sourceCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.sourcecode') }),
      trigger: 'blur'
    }
  ],
  ipqcOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.code') }),
      trigger: 'blur'
    }
  ],
  processCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processcode') }),
      trigger: 'blur'
    }
  ],
  processName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processname') }),
      trigger: 'blur'
    }
  ],
  totalProductionQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.totalproductionquantity') }),
      trigger: 'change'
    }
  ],
  totalSampleQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.totalsamplequantity') }),
      trigger: 'change'
    }
  ],
  totalQualifiedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.totalqualifiedquantity') }),
      trigger: 'change'
    }
  ],
  totalUnqualifiedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.totalunqualifiedquantity') }),
      trigger: 'change'
    }
  ],
  totalInspectionReturnQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.totalinspectionreturnquantity') }),
      trigger: 'change'
    }
  ],
  judgeStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.judgestatus') }),
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
  childIpqcOrderItemRows.value = []
  childIpqcOrderChangeLogRows.value = []
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
