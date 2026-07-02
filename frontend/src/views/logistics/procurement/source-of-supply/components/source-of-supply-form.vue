<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/source-of-supply/components -->
<!-- 文件名称：source-of-supply-form.vue -->
<!-- 功能描述：Takt货源实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="source-of-supply-form-tabs"
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
                :label="t('entity.sourceofsupply.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.plantcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.sourceOfSupplyId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.code')"
                name="sourceOfSupplyCode"
              >
                <a-input
                  v-model:value="formState.sourceOfSupplyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.code') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.sourceOfSupplyId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.sourceOfSupplyId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.suppliercode')"
                name="supplierCode"
              >
                <a-input
                  v-model:value="formState.supplierCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.suppliercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.sourceOfSupplyId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.purchasegroup')"
                name="purchaseGroup"
              >
                <a-input
                  v-model:value="formState.purchaseGroup"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.purchasegroup') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.sourceOfSupplyId"
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
                :label="t('entity.sourceofsupply.validfrom')"
                name="validFrom"
              >
                <a-date-picker
                  v-model:value="formState.validFrom"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.validfrom') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.validto')"
                name="validTo"
              >
                <a-date-picker
                  v-model:value="formState.validTo"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.validto') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.isfixed')"
                name="isFixed"
              >
                <TaktSelect
                  v-model:value="formState.isFixed"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.isfixed') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.isblocked')"
                name="isBlocked"
              >
                <TaktSelect
                  v-model:value="formState.isBlocked"
                  dict-type="sys_yes_no_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.isblocked') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.purchaseunit')"
                name="purchaseUnit"
              >
                <a-input
                  v-model:value="formState.purchaseUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.purchaseunit') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.minimumorderquantity')"
                name="minimumOrderQuantity"
              >
                <a-input-number
                  v-model:value="formState.minimumOrderQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.minimumorderquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.leadtimedays')"
                name="leadTimeDays"
              >
                <a-input-number
                  v-model:value="formState.leadTimeDays"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.leadtimedays') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.agreementnumber')"
                name="agreementNumber"
              >
                <a-input
                  v-model:value="formState.agreementNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.agreementnumber') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.agreementlinenumber')"
                name="agreementLineNumber"
              >
                <a-input-number
                  v-model:value="formState.agreementLineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.agreementlinenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sourceofsupply.sourcestatus')"
                name="sourceStatus"
              >
                <TaktSelect
                  v-model:value="formState.sourceStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.sourcestatus') })"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt货源实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/source-of-supply/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SourceOfSupplyCreate } from '@/types/logistics/procurement/source-of-supply'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","sourceOfSupplyCode","materialCode","supplierCode","purchaseGroup","validFrom","validTo","isFixed","isBlocked","purchaseUnit","minimumOrderQuantity","leadTimeDays","agreementNumber","agreementLineNumber","sourceStatus","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SourceOfSupplyCreate & { sourceOfSupplyId?: string }> | null
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
  sourceStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 sourceOfSupplyId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.sourceOfSupplyId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
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
    const isCreate = !props.formData?.sourceOfSupplyId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.plantcode') }),
      trigger: 'blur'
    }
  ],
  sourceOfSupplyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.code') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.materialcode') }),
      trigger: 'blur'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.suppliercode') }),
      trigger: 'blur'
    }
  ],
  validFrom: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.validfrom') }),
      trigger: 'change'
    }
  ],
  validTo: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.validto') }),
      trigger: 'change'
    }
  ],
  isFixed: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.isfixed') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.isfixed') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isBlocked: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.isblocked') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.isblocked') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  purchaseUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sourceofsupply.purchaseunit') }),
      trigger: 'blur'
    }
  ],
  minimumOrderQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.minimumorderquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.minimumorderquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  leadTimeDays: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.leadtimedays') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.leadtimedays') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sourceStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.sourcestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sourceofsupply.sourcestatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('isFixed' in payload) {
    const rawisFixed = payload.isFixed
    payload.isFixed = typeof rawisFixed === 'number' ? rawisFixed : Number(rawisFixed)
  }
  if ('isBlocked' in payload) {
    const rawisBlocked = payload.isBlocked
    payload.isBlocked = typeof rawisBlocked === 'number' ? rawisBlocked : Number(rawisBlocked)
  }
  if ('minimumOrderQuantity' in payload) {
    const rawminimumOrderQuantity = payload.minimumOrderQuantity
    payload.minimumOrderQuantity = typeof rawminimumOrderQuantity === 'number' ? rawminimumOrderQuantity : Number(rawminimumOrderQuantity)
  }
  if ('leadTimeDays' in payload) {
    const rawleadTimeDays = payload.leadTimeDays
    payload.leadTimeDays = typeof rawleadTimeDays === 'number' ? rawleadTimeDays : Number(rawleadTimeDays)
  }
  if ('agreementLineNumber' in payload) {
    const rawagreementLineNumber = payload.agreementLineNumber
    payload.agreementLineNumber = typeof rawagreementLineNumber === 'number' ? rawagreementLineNumber : Number(rawagreementLineNumber)
  }
  if ('sourceStatus' in payload) {
    const rawsourceStatus = payload.sourceStatus
    payload.sourceStatus = typeof rawsourceStatus === 'number' ? rawsourceStatus : Number(rawsourceStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.sourceOfSupplyId)

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
