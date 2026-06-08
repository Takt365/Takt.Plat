<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing/components -->
<!-- 文件名称：routing-form.vue -->
<!-- 功能描述：工艺路线主表实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="routing-form-tabs"
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
                :label="t('entity.routing.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routing.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routing.workcenter')"
                name="workCenter"
              >
                <a-input
                  v-model:value="formState.workCenter"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routing.workcenter') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routing.code')"
                name="routingCode"
              >
                <a-input
                  v-model:value="formState.routingCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routing.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routing.name')"
                name="routingName"
              >
                <a-input
                  v-model:value="formState.routingName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routing.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routing.purpose')"
                name="purpose"
              >
                <a-input-number
                  v-model:value="formState.purpose"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routing.purpose') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routing.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routing.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routing.version')"
                name="version"
              >
                <a-input
                  v-model:value="formState.version"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routing.version') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routing.status')"
                name="routingStatus"
              >
                <a-input-number
                  v-model:value="formState.routingStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routing.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routing.effectivedate')"
                name="effectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.effectiveDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routing.effectivedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routing.expirydate')"
                name="expiryDate"
              >
                <a-date-picker
                  v-model:value="formState.expiryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routing.expirydate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.routing.description')"
                name="routingDescription"
              >
                <a-textarea
                  v-model:value="formState.routingDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.routing.description') })"
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
      <!-- 子表：routingItem -->
      <a-tab-pane
        key="child-items"
        :tab="t('entity.routingItem._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddRoutingItemRow">
            {{ t('common.page.button.create') }}{{ t('entity.routingItem._self') }}
          </a-button>
        </div>
        <a-table
          :columns="routingItemFormColumns"
          :data-source="childRoutingItemRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.linenumber') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'baseUnit'">
              <a-input
                v-model:value="record.baseUnit"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.baseunit') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'baseQuantity'">
              <a-input-number
                v-model:value="record.baseQuantity"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.basequantity') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'standardMinutes'">
              <a-input-number
                v-model:value="record.standardMinutes"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.standardminutes') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'timeUnit'">
              <a-input
                v-model:value="record.timeUnit"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.timeunit') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'standardShorts'">
              <a-input-number
                v-model:value="record.standardShorts"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.standardshorts') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'pointsUnit'">
              <a-input
                v-model:value="record.pointsUnit"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.pointsunit') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'pointsToMinutesRate'">
              <a-input-number
                v-model:value="record.pointsToMinutesRate"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.pointstominutesrate') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveRoutingItemRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：routingChangeLog -->
      <a-tab-pane
        key="child-changeLogs"
        :tab="t('entity.routingChangeLog._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddRoutingChangeLogRow">
            {{ t('common.page.button.create') }}{{ t('entity.routingChangeLog._self') }}
          </a-button>
        </div>
        <a-table
          :columns="routingChangeLogFormColumns"
          :data-source="childRoutingChangeLogRows"
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
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingChangeLog.changefields') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeType'">
              <a-input-number
                v-model:value="record.changeType"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingChangeLog.changetype') })"
                size="small"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'changeReason'">
              <a-input
                v-model:value="record.changeReason"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingChangeLog.changereason') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeBy'">
              <a-input
                v-model:value="record.changeBy"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingChangeLog.changeby') })"
                size="small"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'changeTime'">
              <a-input
                v-model:value="record.changeTime"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingChangeLog.changetime') })"
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
              <a-button type="link" danger size="small" @click="handleRemoveRoutingChangeLogRow(index)">
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
 * 工艺路线主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/routing/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { RoutingCreate, RoutingItemCreate, RoutingItem, RoutingChangeLogCreate, RoutingChangeLog } from '@/types/logistics/manufacturing/bom/routing'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","workCenter","routingCode","routingName","purpose","materialCode","version","routingStatus","effectiveDate","expiryDate","routingDescription","extFieldJson","remark"]

/** routingItem 子表行（表单 Tab 内嵌） */
const childRoutingItemRows = ref<Record<string, unknown>[]>([])
/** routingChangeLog 子表行（表单 Tab 内嵌） */
const childRoutingChangeLogRows = ref<Record<string, unknown>[]>([])

/** 子表 routingItem 表单列定义 */
const routingItemFormColumns = computed(() => [
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
    title: t('entity.routingItem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 140,
  },
  {
    title: t('entity.routingItem.baseunit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 140,
  },
  {
    title: t('entity.routingItem.basequantity'),
    dataIndex: 'baseQuantity',
    key: 'baseQuantity',
    width: 140,
  },
  {
    title: t('entity.routingItem.standardminutes'),
    dataIndex: 'standardMinutes',
    key: 'standardMinutes',
    width: 140,
  },
  {
    title: t('entity.routingItem.timeunit'),
    dataIndex: 'timeUnit',
    key: 'timeUnit',
    width: 140,
  },
  {
    title: t('entity.routingItem.standardshorts'),
    dataIndex: 'standardShorts',
    key: 'standardShorts',
    width: 140,
  },
  {
    title: t('entity.routingItem.pointsunit'),
    dataIndex: 'pointsUnit',
    key: 'pointsUnit',
    width: 140,
  },
  {
    title: t('entity.routingItem.pointstominutesrate'),
    dataIndex: 'pointsToMinutesRate',
    key: 'pointsToMinutesRate',
    width: 140,
  },
  {
    title: t('common.page.entity.action'),
    key: '__action',
    width: 80,
    fixed: 'right',
  },
])

/** 子表 routingChangeLog 表单列定义 */
const routingChangeLogFormColumns = computed(() => [
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
    title: t('entity.routingChangeLog.changefields'),
    dataIndex: 'changeFields',
    key: 'changeFields',
    width: 140,
  },
  {
    title: t('entity.routingChangeLog.changetype'),
    dataIndex: 'changeType',
    key: 'changeType',
    width: 140,
  },
  {
    title: t('entity.routingChangeLog.changereason'),
    dataIndex: 'changeReason',
    key: 'changeReason',
    width: 140,
  },
  {
    title: t('entity.routingChangeLog.changeby'),
    dataIndex: 'changeBy',
    key: 'changeBy',
    width: 140,
  },
  {
    title: t('entity.routingChangeLog.changetime'),
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
function syncChildRowsFromFormData(val: Partial<RoutingCreate & { routingId?: string }> | null | undefined) {
  childRoutingItemRows.value = ((val as any)?.items ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.routingItemId ?? `new-${index}`,
  }))
  childRoutingChangeLogRows.value = ((val as any)?.changeLogs ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.routingChangeLogId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 routingItem 行 */
function handleAddRoutingItemRow() {
  childRoutingItemRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      lineNumber: 0,
      baseUnit: '',
      baseQuantity: 0,
      standardMinutes: 0,
      timeUnit: '',
      standardShorts: 0,
      pointsUnit: '',
      pointsToMinutesRate: 0,
  })
}

/** 表单 Tab 内删除 routingItem 行 */
function handleRemoveRoutingItemRow(index: number) {
  childRoutingItemRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 routingChangeLog 行 */
function handleAddRoutingChangeLogRow() {
  childRoutingChangeLogRows.value.push({
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

/** 表单 Tab 内删除 routingChangeLog 行 */
function handleRemoveRoutingChangeLogRow(index: number) {
  childRoutingChangeLogRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    items: childRoutingItemRows.value.map(({ __rowKey, ...rest }) => rest),
    changeLogs: childRoutingChangeLogRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<RoutingCreate & { routingId?: string }> | null
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
    const isCreate = !props.formData?.routingId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.routing.plantcode') }),
      trigger: 'blur'
    }
  ],
  workCenter: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routing.workcenter') }),
      trigger: 'blur'
    }
  ],
  routingCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routing.code') }),
      trigger: 'blur'
    }
  ],
  routingName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routing.name') }),
      trigger: 'blur'
    }
  ],
  purpose: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routing.purpose') }),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routing.materialcode') }),
      trigger: 'blur'
    }
  ],
  version: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routing.version') }),
      trigger: 'blur'
    }
  ],
  routingStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routing.status') }),
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
  childRoutingItemRows.value = []
  childRoutingChangeLogRows.value = []
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
