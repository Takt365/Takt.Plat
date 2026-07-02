<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing-item/components -->
<!-- 文件名称：routing-item-form.vue -->
<!-- 功能描述：工艺路线明细表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form routing-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="routing-item-form-tabs"
    >
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
                :label="t('entity.routingitem.routingid')"
                name="routingId"
              >
                <a-input
                  v-model:value="formState.routingId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.routingid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.routingcode')"
                name="routingCode"
              >
                <a-input
                  v-model:value="formState.routingCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.routingcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.routingItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.baseunit')"
                name="baseUnit"
              >
                <a-input
                  v-model:value="formState.baseUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.baseunit') })"
                  show-count
                  :maxlength="5"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.basequantity')"
                name="baseQuantity"
              >
                <a-input-number
                  v-model:value="formState.baseQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.basequantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.standardminutes')"
                name="standardMinutes"
              >
                <a-input-number
                  v-model:value="formState.standardMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.standardminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.timeunit')"
                name="timeUnit"
              >
                <a-input
                  v-model:value="formState.timeUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.timeunit') })"
                  show-count
                  :maxlength="3"
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
                :label="t('entity.routingitem.standardshorts')"
                name="standardShorts"
              >
                <a-input-number
                  v-model:value="formState.standardShorts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.standardshorts') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.pointsunit')"
                name="pointsUnit"
              >
                <a-input
                  v-model:value="formState.pointsUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.pointsunit') })"
                  show-count
                  :maxlength="5"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.pointstominutesrate')"
                name="pointsToMinutesRate"
              >
                <TaktSelect
                  v-model:value="formState.pointsToMinutesRate"
                  dict-type="logistics_points_to_minutes_rate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routingitem.pointstominutesrate') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.convertedminutes')"
                name="convertedMinutes"
              >
                <a-input-number
                  v-model:value="formState.convertedMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.convertedminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.setupminutes')"
                name="setupMinutes"
              >
                <a-input-number
                  v-model:value="formState.setupMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.setupminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.teardownminutes')"
                name="teardownMinutes"
              >
                <a-input-number
                  v-model:value="formState.teardownMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.teardownminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.isinspection')"
                name="isInspection"
              >
                <TaktSelect
                  v-model:value="formState.isInspection"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routingitem.isinspection') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.routingitem.processdescription')"
                name="processDescription"
              >
                <a-textarea
                  v-model:value="formState.processDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.routingitem.processdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.processsegmenttype')"
                name="processSegmentType"
              >
                <TaktSelect
                  v-model:value="formState.processSegmentType"
                  dict-type="logistics_process_segment_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.routingitem.processsegmenttype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingitem.extjson')"
                name="extJson"
              >
                <a-input
                  v-model:value="formState.extJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingitem.extjson') })"
                  show-count
                  :maxlength="4000"
                  allow-clear
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
    <!-- 下：子表 arguments -->
    <TaktEditableTable
      ref="routingItemArgumentTableRef"
      v-model="childRoutingItemArgumentRows"
      :columns="routingItemArgumentFormColumns"
      :title="t('entity.routingitemargument._self')"
      :add-button-entity="t('entity.routingitemargument._self')"
      id-field="routingItemArgumentId"
      :default-row="createDefaultRoutingItemArgumentRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 工艺路线明细表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/routing-item/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { RoutingItemCreate } from '@/types/logistics/manufacturing/bom/routing-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","routingId","routingCode","lineNumber","baseUnit","baseQuantity","standardMinutes","timeUnit","standardShorts","pointsUnit","pointsToMinutesRate","convertedMinutes","setupMinutes","teardownMinutes","isInspection","processDescription","processSegmentType","extJson","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childRoutingItemArgumentRows = ref<Record<string, unknown>[]>([])
const routingItemArgumentTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 routingItemArgument 可编辑列 */
const routingItemArgumentFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'paramCode',
    title: t('entity.routingitemargument.paramcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'paramName',
    title: t('entity.routingitemargument.paramname'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'paramUnit',
    title: t('entity.routingitemargument.paramunit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.routingitemargument.paramunit') }),
  },
  {
    key: 'standardValue',
    title: t('entity.routingitemargument.standardvalue'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'lowerLimit',
    title: t('entity.routingitemargument.lowerlimit'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'upperLimit',
    title: t('entity.routingitemargument.upperlimit'),
    editor: 'inputNumber',
    width: 140,
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
function syncChildRowsFromFormData(val: Partial<RoutingItemCreate & { routingItemId?: string }> | null | undefined) {
  childRoutingItemArgumentRows.value = ((val as any)?.arguments ?? []) as Record<string, unknown>[]
}

function createDefaultRoutingItemArgumentRow(): Record<string, unknown> {
  return {
    paramCode: '',
    paramName: '',
    paramUnit: '',
    standardValue: 0,
    lowerLimit: 0,
    upperLimit: 0,
    extField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.routingItemId ?? ''
  return {
    ...formState,
    arguments: routingItemArgumentTableRef.value?.getRows?.() ?? childRoutingItemArgumentRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      routingItemId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<RoutingItemCreate & { routingItemId?: string }> | null
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
  processSegmentType: 1,
  isInspection: 0,
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 routingItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.routingItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).arguments
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
    const isCreate = !props.formData?.routingItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  routingId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingitem.routingid') }),
      trigger: 'blur'
    }
  ],
  routingCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingitem.routingcode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  baseUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingitem.baseunit') }),
      trigger: 'blur'
    }
  ],
  baseQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.basequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.basequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  standardMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.standardminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.standardminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  timeUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingitem.timeunit') }),
      trigger: 'blur'
    }
  ],
  standardShorts: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.standardshorts') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.standardshorts') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pointsUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingitem.pointsunit') }),
      trigger: 'blur'
    }
  ],
  pointsToMinutesRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingitem.pointstominutesrate') }),
      trigger: 'change'
    }
  ],
  convertedMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.convertedminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.convertedminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  setupMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.setupminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.setupminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  teardownMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.teardownminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.teardownminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isInspection: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingitem.isinspection') }),
      trigger: 'change'
    }
  ],
  processSegmentType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.processsegmenttype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.routingitem.processsegmenttype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await routingItemArgumentTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('baseQuantity' in payload) {
    const rawbaseQuantity = payload.baseQuantity
    payload.baseQuantity = typeof rawbaseQuantity === 'number' ? rawbaseQuantity : Number(rawbaseQuantity)
  }
  if ('standardMinutes' in payload) {
    const rawstandardMinutes = payload.standardMinutes
    payload.standardMinutes = typeof rawstandardMinutes === 'number' ? rawstandardMinutes : Number(rawstandardMinutes)
  }
  if ('standardShorts' in payload) {
    const rawstandardShorts = payload.standardShorts
    payload.standardShorts = typeof rawstandardShorts === 'number' ? rawstandardShorts : Number(rawstandardShorts)
  }
  if ('convertedMinutes' in payload) {
    const rawconvertedMinutes = payload.convertedMinutes
    payload.convertedMinutes = typeof rawconvertedMinutes === 'number' ? rawconvertedMinutes : Number(rawconvertedMinutes)
  }
  if ('setupMinutes' in payload) {
    const rawsetupMinutes = payload.setupMinutes
    payload.setupMinutes = typeof rawsetupMinutes === 'number' ? rawsetupMinutes : Number(rawsetupMinutes)
  }
  if ('teardownMinutes' in payload) {
    const rawteardownMinutes = payload.teardownMinutes
    payload.teardownMinutes = typeof rawteardownMinutes === 'number' ? rawteardownMinutes : Number(rawteardownMinutes)
  }
  if ('processSegmentType' in payload) {
    const rawprocessSegmentType = payload.processSegmentType
    payload.processSegmentType = typeof rawprocessSegmentType === 'number' ? rawprocessSegmentType : Number(rawprocessSegmentType)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.routingItemId)
  childRoutingItemArgumentRows.value = []
  routingItemArgumentTableRef.value?.resetRows?.()
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
