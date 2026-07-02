<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/serial/outbound/components -->
<!-- 文件名称：outbound-form.vue -->
<!-- 功能描述：序列号出库主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form outbound-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="outbound-form-tabs"
    >
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
                :label="t('entity.serialoutbound.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.serialOutboundId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serialoutbound.outboundno')"
                name="outboundNo"
              >
                <a-input
                  v-model:value="formState.outboundNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.outboundno') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serialoutbound.shippinginvoiceno')"
                name="shippingInvoiceNo"
              >
                <a-input
                  v-model:value="formState.shippingInvoiceNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.shippinginvoiceno') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serialoutbound.outbounddate')"
                name="outboundDate"
              >
                <a-date-picker
                  v-model:value="formState.outboundDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.outbounddate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serialoutbound.destination')"
                name="destination"
              >
                <TaktSelect
                  v-model:value="formState.destination"
                  api-url="/api/TaktModelDestinations/options"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.destination') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serialoutbound.shippingmethod')"
                name="shippingMethod"
              >
                <TaktSelect
                  v-model:value="formState.shippingMethod"
                  dict-type="logistics_shipping_method_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.shippingmethod') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.serialoutbound.destinationport')"
                name="destinationPort"
              >
                <TaktSelect
                  v-model:value="formState.destinationPort"
                  dict-type="logistics_destination_port_code"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.destinationport') })"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.serialoutbound.outboundtype')"
                name="outboundType"
              >
                <TaktSelect
                  v-model:value="formState.outboundType"
                  dict-type="logistics_outbound_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.outboundtype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.serialoutbound.warehousecode')"
                name="warehouseCode"
              >
                <a-input
                  v-model:value="formState.warehouseCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.warehousecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.serialOutboundId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.serialoutbound.locationcode')"
                name="locationCode"
              >
                <a-input
                  v-model:value="formState.locationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.locationcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.serialOutboundId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.serialoutbound.totalquantity')"
                name="totalQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.totalquantity') })"
                  style="width: 100%"
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="serialOutboundItemTableRef"
      v-model="childSerialOutboundItemRows"
      :columns="serialOutboundItemFormColumns"
      :title="t('entity.serialoutbounditem._self')"
      :add-button-entity="t('entity.serialoutbounditem._self')"
      id-field="serialOutboundItemId"
      :default-row="createDefaultSerialOutboundItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 序列号出库主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/serial/outbound/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SerialOutboundCreate } from '@/types/logistics/serial/outbound'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","outboundNo","shippingInvoiceNo","outboundDate","destination","shippingMethod","destinationPort","outboundType","warehouseCode","locationCode","totalQuantity","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childSerialOutboundItemRows = ref<Record<string, unknown>[]>([])
const serialOutboundItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 serialOutboundItem 可编辑列 */
const serialOutboundItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'outboundNo',
    title: t('entity.serialoutbounditem.outboundno'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: t('entity.serialoutbounditem.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'outboundSerialNo',
    title: t('entity.serialoutbounditem.outboundserialno'),
    editor: 'input',
    width: 140, required: true, unique: true,
  },
  {
    key: 'referenceInboundId',
    title: t('entity.serialoutbounditem.referenceinboundid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'referenceInboundNo',
    title: t('entity.serialoutbounditem.referenceinboundno'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'referenceInboundLineNumber',
    title: t('entity.serialoutbounditem.referenceinboundlinenumber'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'outboundTime',
    title: t('entity.serialoutbounditem.outboundtime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
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
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SerialOutboundCreate & { serialOutboundId?: string }> | null | undefined) {
  childSerialOutboundItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultSerialOutboundItemRow(): Record<string, unknown> {
  return {
    outboundNo: '',
    lineNumber: (childSerialOutboundItemRows.value.length + 1) * 10,
    outboundSerialNo: '',
    referenceInboundId: '',
    referenceInboundNo: '',
    referenceInboundLineNumber: 0,
    outboundTime: '',
    extField: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.serialOutboundId ?? ''
  return {
    ...formState,
    items: serialOutboundItemTableRef.value?.getRows?.() ?? childSerialOutboundItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      outboundId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SerialOutboundCreate & { serialOutboundId?: string }> | null
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
  shippingMethod: 0,
  outboundType: 5
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 serialOutboundId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.serialOutboundId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
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
    const isCreate = !props.formData?.serialOutboundId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.plantcode') }),
      trigger: 'blur'
    }
  ],
  outboundNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.outboundno') }),
      trigger: 'blur'
    }
  ],
  shippingInvoiceNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.shippinginvoiceno') }),
      trigger: 'blur'
    }
  ],
  outboundDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.outbounddate') }),
      trigger: 'change'
    }
  ],
  destination: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.destination') }),
      trigger: 'blur'
    }
  ],
  shippingMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.shippingmethod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.shippingmethod') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  destinationPort: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.destinationport') }),
      trigger: 'change'
    }
  ],
  outboundType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.outboundtype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.outboundtype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  warehouseCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.warehousecode') }),
      trigger: 'blur'
    }
  ],
  locationCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.serialoutbound.locationcode') }),
      trigger: 'blur'
    }
  ],
  totalQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.totalquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.serialoutbound.totalquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await serialOutboundItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('shippingMethod' in payload) {
    const rawshippingMethod = payload.shippingMethod
    payload.shippingMethod = typeof rawshippingMethod === 'number' ? rawshippingMethod : Number(rawshippingMethod)
  }
  if ('outboundType' in payload) {
    const rawoutboundType = payload.outboundType
    payload.outboundType = typeof rawoutboundType === 'number' ? rawoutboundType : Number(rawoutboundType)
  }
  if ('totalQuantity' in payload) {
    const rawtotalQuantity = payload.totalQuantity
    payload.totalQuantity = typeof rawtotalQuantity === 'number' ? rawtotalQuantity : Number(rawtotalQuantity)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.serialOutboundId)
  childSerialOutboundItemRows.value = []
  serialOutboundItemTableRef.value?.resetRows?.()
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
