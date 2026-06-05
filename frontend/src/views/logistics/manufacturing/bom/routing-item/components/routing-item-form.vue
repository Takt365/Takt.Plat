<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing-item/components -->
<!-- 文件名称：routing-item-form.vue -->
<!-- 功能描述：工艺路线明细表实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
                :label="t('entity.routingItem.routingid')"
                name="routingId"
              >
                <a-input
                  v-model:value="formState.routingId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.routingid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.routingcode')"
                name="routingCode"
              >
                <a-input
                  v-model:value="formState.routingCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.routingcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.baseunit')"
                name="baseUnit"
              >
                <a-input
                  v-model:value="formState.baseUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.baseunit') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.basequantity')"
                name="baseQuantity"
              >
                <a-input-number
                  v-model:value="formState.baseQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.basequantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.standardminutes')"
                name="standardMinutes"
              >
                <a-input-number
                  v-model:value="formState.standardMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.standardminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.timeunit')"
                name="timeUnit"
              >
                <a-input
                  v-model:value="formState.timeUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.timeunit') })"
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
                :label="t('entity.routingItem.standardshorts')"
                name="standardShorts"
              >
                <a-input-number
                  v-model:value="formState.standardShorts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.standardshorts') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.pointsunit')"
                name="pointsUnit"
              >
                <a-input
                  v-model:value="formState.pointsUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.pointsunit') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.pointstominutesrate')"
                name="pointsToMinutesRate"
              >
                <a-input-number
                  v-model:value="formState.pointsToMinutesRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.pointstominutesrate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.convertedminutes')"
                name="convertedMinutes"
              >
                <a-input-number
                  v-model:value="formState.convertedMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.convertedminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.setupminutes')"
                name="setupMinutes"
              >
                <a-input-number
                  v-model:value="formState.setupMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.setupminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.teardownminutes')"
                name="teardownMinutes"
              >
                <a-input-number
                  v-model:value="formState.teardownMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.teardownminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.isqualitycheck')"
                name="isQualityCheck"
              >
                <a-switch v-model:checked="formState.isQualityCheck" size="small" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.routingItem.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.routingItem.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.routingItem.processdescription')"
                name="processDescription"
              >
                <a-textarea
                  v-model:value="formState.processDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.routingItem.processdescription') })"
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

    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 工艺路线明细表实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/routing-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { RoutingItemCreate } from '@/types/logistics/manufacturing/bom/routing-item'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

const { t } = useI18n()

const tenantStore = useTenantStore()
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
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["tenantCode","companyCode","companyDefaultCulture","routingId","routingCode","lineNumber","baseUnit","baseQuantity","standardMinutes","timeUnit","standardShorts","pointsUnit","pointsToMinutesRate","convertedMinutes","setupMinutes","teardownMinutes","isQualityCheck","sortOrder","processDescription","extFieldJson","remark"]


interface Props {
  formData?: Partial<RoutingItemCreate & { routingItemId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})

watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])

    applyScopeDefaults(next)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.routingItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  routingId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingItem.routingid') }),
      trigger: 'blur'
    }
  ],
  routingCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingItem.routingcode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingItem.linenumber') }),
      trigger: 'change'
    }
  ],
  baseUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingItem.baseunit') }),
      trigger: 'blur'
    }
  ],
  baseQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingItem.basequantity') }),
      trigger: 'change'
    }
  ],
  standardMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingItem.standardminutes') }),
      trigger: 'change'
    }
  ],
  timeUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingItem.timeunit') }),
      trigger: 'blur'
    }
  ],
  standardShorts: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingItem.standardshorts') }),
      trigger: 'change'
    }
  ],
  pointsUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingItem.pointsunit') }),
      trigger: 'blur'
    }
  ],
  pointsToMinutesRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingItem.pointstominutesrate') }),
      trigger: 'change'
    }
  ],
  convertedMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingItem.convertedminutes') }),
      trigger: 'change'
    }
  ],
  setupMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingItem.setupminutes') }),
      trigger: 'change'
    }
  ],
  teardownMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingItem.teardownminutes') }),
      trigger: 'change'
    }
  ],
  isQualityCheck: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.routingItem.isqualitycheck') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.routingItem.sortorder') }),
      trigger: 'change'
    }
  ],
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
  return { ...formState }
}

function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])

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
