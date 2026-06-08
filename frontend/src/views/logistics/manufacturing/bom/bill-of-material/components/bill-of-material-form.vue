<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/components -->
<!-- 文件名称：bill-of-material-form.vue -->
<!-- 功能描述：Takt物料清单实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="bill-of-material-form-tabs"
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
                :label="t('entity.billOfMaterial.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.bomcode')"
                name="bomCode"
              >
                <a-input
                  v-model:value="formState.bomCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.bomname')"
                name="bomName"
              >
                <a-input
                  v-model:value="formState.bomName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.parentmaterialid')"
                name="parentMaterialId"
              >
                <a-input
                  v-model:value="formState.parentMaterialId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.parentmaterialcode')"
                name="parentMaterialCode"
              >
                <a-input
                  v-model:value="formState.parentMaterialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.parentmaterialname')"
                name="parentMaterialName"
              >
                <a-input
                  v-model:value="formState.parentMaterialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.bomversion')"
                name="bomVersion"
              >
                <a-input
                  v-model:value="formState.bomVersion"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomversion') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.bomtype')"
                name="bomType"
              >
                <a-input-number
                  v-model:value="formState.bomType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomtype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.alternativebomnumber')"
                name="alternativeBomNumber"
              >
                <a-input
                  v-model:value="formState.alternativeBomNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.alternativebomnumber') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.effectivedate')"
                name="effectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.effectiveDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.effectivedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.expirydate')"
                name="expiryDate"
              >
                <a-date-picker
                  v-model:value="formState.expiryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.expirydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.parentmaterialunit')"
                name="parentMaterialUnit"
              >
                <a-input
                  v-model:value="formState.parentMaterialUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialunit') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.parentmaterialquantity')"
                name="parentMaterialQuantity"
              >
                <a-input-number
                  v-model:value="formState.parentMaterialQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.isenabled')"
                name="isEnabled"
              >
                <a-input-number
                  v-model:value="formState.isEnabled"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.isenabled') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.bomstatus')"
                name="bomStatus"
              >
                <a-input-number
                  v-model:value="formState.bomStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.billOfMaterial.bomdescription')"
                name="bomDescription"
              >
                <a-textarea
                  v-model:value="formState.bomDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.billOfMaterial.bomdescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterial.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.sortorder') })"
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
      <!-- 子表：billOfMaterialItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.billOfMaterialItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddBillOfMaterialItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.billOfMaterialItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="billOfMaterialItemFormColumns"
          :data-source="childBillOfMaterialItemRows"
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
            <template v-else-if="column.key === 'bomCode'">
              <a-input
                v-model:value="record.bomCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.bomcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'lineNumber'">
              <a-input-number
                v-model:value="record.lineNumber"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'materialId'">
              <a-input
                v-model:value="record.materialId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialid') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'materialCode'">
              <a-input
                v-model:value="record.materialCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'usageQuantity'">
              <a-input-number
                v-model:value="record.usageQuantity"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.usagequantity') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'materialUnit'">
              <a-input
                v-model:value="record.materialUnit"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialunit') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'scrapRate'">
              <a-input-number
                v-model:value="record.scrapRate"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.scraprate') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'actualUsageQuantity'">
              <a-input-number
                v-model:value="record.actualUsageQuantity"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.actualusagequantity') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveBillOfMaterialItemRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：billOfMaterialChangeLog -->
      <a-tab-pane
        key="child-changeLogs"
        :tab="t('entity.billOfMaterialChangeLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddBillOfMaterialChangeLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.billOfMaterialChangeLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="billOfMaterialChangeLogFormColumns"
          :data-source="childBillOfMaterialChangeLogRows"
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
            <template v-else-if="column.key === 'bomCode'">
              <a-input
                v-model:value="record.bomCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialChangeLog.bomcode') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeFields'">
              <a-input
                v-model:value="record.changeFields"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialChangeLog.changefields') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeTime'">
              <a-input
                v-model:value="record.changeTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialChangeLog.changetime') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeBy'">
              <a-input
                v-model:value="record.changeBy"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialChangeLog.changeby') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeReason'">
              <a-input
                v-model:value="record.changeReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialChangeLog.changereason') })"
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
              <a-button type="link" danger size="small" @click="handleRemoveBillOfMaterialChangeLogRow(index)">
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
 * Takt物料清单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/bill-of-material/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { BillOfMaterialCreate, BillOfMaterialItemCreate, BillOfMaterialItem, BillOfMaterialChangeLogCreate, BillOfMaterialChangeLog } from '@/types/logistics/manufacturing/bom/bill-of-material'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","bomCode","bomName","parentMaterialId","parentMaterialCode","parentMaterialName","bomVersion","bomType","alternativeBomNumber","effectiveDate","expiryDate","parentMaterialUnit","parentMaterialQuantity","isEnabled","bomStatus","bomDescription","sortOrder","extFieldJson","remark"]

/** billOfMaterialItem 子表行（表单 Tab 内嵌） */
const childBillOfMaterialItemRows = ref<Record<string, unknown>[]>([])
/** billOfMaterialChangeLog 子表行（表单 Tab 内嵌） */
const childBillOfMaterialChangeLogRows = ref<Record<string, unknown>[]>([])

/** 子表 billOfMaterialItem 表单列定义 */
const billOfMaterialItemFormColumns = computed(() => [
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
    title: t('entity.billOfMaterialItem.bomcode'),
    dataIndex: 'bomCode',
    key: 'bomCode',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialItem.materialid'),
    dataIndex: 'materialId',
    key: 'materialId',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialItem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialItem.usagequantity'),
    dataIndex: 'usageQuantity',
    key: 'usageQuantity',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialItem.materialunit'),
    dataIndex: 'materialUnit',
    key: 'materialUnit',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialItem.scraprate'),
    dataIndex: 'scrapRate',
    key: 'scrapRate',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialItem.actualusagequantity'),
    dataIndex: 'actualUsageQuantity',
    key: 'actualUsageQuantity',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 billOfMaterialChangeLog 表单列定义 */
const billOfMaterialChangeLogFormColumns = computed(() => [
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
    title: t('entity.billOfMaterialChangeLog.bomcode'),
    dataIndex: 'bomCode',
    key: 'bomCode',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialChangeLog.changetime'),
    dataIndex: 'changeTime',
    key: 'changeTime',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    width: 140,
  },
  {
    title: t('entity.billOfMaterialChangeLog.changereason'),
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
function syncChildRowsFromFormData(val: Partial<BillOfMaterialCreate & { billOfMaterialId?: string }> | null | undefined) {
  childBillOfMaterialItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.billOfMaterialItemId ?? `new-${index}`,
  }))
  childBillOfMaterialChangeLogRows.value = ((val as any)?.changeLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.billOfMaterialChangeLogId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 billOfMaterialItem 行 */
function handleAddBillOfMaterialItemRow() {
  childBillOfMaterialItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      bomCode: '',
      lineNumber: 0,
      materialId: '',
      materialCode: '',
      usageQuantity: 0,
      materialUnit: '',
      scrapRate: 0,
      actualUsageQuantity: 0,
  })
}

/** 表单 Tab 内删除 billOfMaterialItem 行 */
function handleRemoveBillOfMaterialItemRow(index: number) {
  childBillOfMaterialItemRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 billOfMaterialChangeLog 行 */
function handleAddBillOfMaterialChangeLogRow() {
  childBillOfMaterialChangeLogRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      bomCode: '',
      changeFields: '',
      changeTime: '',
      changeBy: '',
      changeReason: '',
      extFieldJson: '',
      remark: '',
  })
}

/** 表单 Tab 内删除 billOfMaterialChangeLog 行 */
function handleRemoveBillOfMaterialChangeLogRow(index: number) {
  childBillOfMaterialChangeLogRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childBillOfMaterialItemRows.value.map(({ __rowKey, ...rest }) => rest),
    changeLogs: childBillOfMaterialChangeLogRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<BillOfMaterialCreate & { billOfMaterialId?: string }> | null
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
    const isCreate = !props.formData?.billOfMaterialId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.plantcode') }),
      trigger: 'blur'
    }
  ],
  bomCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomcode') }),
      trigger: 'blur'
    }
  ],
  bomName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomname') }),
      trigger: 'blur'
    }
  ],
  parentMaterialId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialid') }),
      trigger: 'blur'
    }
  ],
  parentMaterialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialcode') }),
      trigger: 'blur'
    }
  ],
  parentMaterialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialname') }),
      trigger: 'blur'
    }
  ],
  bomVersion: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.bomversion') }),
      trigger: 'blur'
    }
  ],
  bomType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.bomtype') }),
      trigger: 'change'
    }
  ],
  alternativeBomNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.alternativebomnumber') }),
      trigger: 'blur'
    }
  ],
  effectiveDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.effectivedate') }),
      trigger: 'change'
    }
  ],
  parentMaterialUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterial.parentmaterialunit') }),
      trigger: 'blur'
    }
  ],
  parentMaterialQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.parentmaterialquantity') }),
      trigger: 'change'
    }
  ],
  isEnabled: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.isenabled') }),
      trigger: 'change'
    }
  ],
  bomStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.bomstatus') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterial.sortorder') }),
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
  childBillOfMaterialItemRows.value = []
  childBillOfMaterialChangeLogRows.value = []
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
