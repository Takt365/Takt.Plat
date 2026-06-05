<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/assy-output-detail/components -->
<!-- 文件名称：assy-output-detail-form.vue -->
<!-- 功能描述：组立日报明细维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="assy-output-detail-form-tabs"
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
                :label="t('entity.assyOutputDetail.assyoutputid')"
                name="assyOutputId"
              >
                <a-input
                  v-model:value="formState.assyOutputId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.assyoutputid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.prodordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.timeperiod')"
                name="timePeriod"
              >
                <a-input
                  v-model:value="formState.timePeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.timeperiod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.prodactualqty')"
                name="prodActualQty"
              >
                <a-input-number
                  v-model:value="formState.prodActualQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.prodactualqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.downtimeminutes')"
                name="downtimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.downtimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.downtimeminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.downtimereason')"
                name="downtimeReason"
              >
                <a-input
                  v-model:value="formState.downtimeReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.downtimereason') })"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.assyOutputDetail.downtimedescription')"
                name="downtimeDescription"
              >
                <a-textarea
                  v-model:value="formState.downtimeDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assyOutputDetail.downtimedescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.unachievedreason')"
                name="unachievedReason"
              >
                <a-input
                  v-model:value="formState.unachievedReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.unachievedreason') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.assyOutputDetail.unachieveddescription')"
                name="unachievedDescription"
              >
                <a-textarea
                  v-model:value="formState.unachievedDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.assyOutputDetail.unachieveddescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.inputminutes')"
                name="inputMinutes"
              >
                <a-input-number
                  v-model:value="formState.inputMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.inputminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.prodminutes')"
                name="prodMinutes"
              >
                <a-input-number
                  v-model:value="formState.prodMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.prodminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.actualminutes')"
                name="actualMinutes"
              >
                <a-input-number
                  v-model:value="formState.actualMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.actualminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyOutputDetail.achievementrate')"
                name="achievementRate"
              >
                <a-input-number
                  v-model:value="formState.achievementRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.achievementrate') })"
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

    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 组立日报明细维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/output/assy-output-detail/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { AssyOutputDetailCreate } from '@/types/logistics/manufacturing/output/assy-output-detail'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","assyOutputId","prodOrderCode","lineNumber","timePeriod","prodActualQty","downtimeMinutes","downtimeReason","downtimeDescription","unachievedReason","unachievedDescription","inputMinutes","prodMinutes","actualMinutes","achievementRate","extFieldJson","remark"]


interface Props {
  formData?: Partial<AssyOutputDetailCreate & { assyOutputDetailId?: string }> | null
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
    const isCreate = !props.formData?.assyOutputDetailId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  assyOutputId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.assyoutputid') }),
      trigger: 'blur'
    }
  ],
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.prodordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutputDetail.linenumber') }),
      trigger: 'change'
    }
  ],
  timePeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyOutputDetail.timeperiod') }),
      trigger: 'blur'
    }
  ],
  prodActualQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutputDetail.prodactualqty') }),
      trigger: 'change'
    }
  ],
  downtimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutputDetail.downtimeminutes') }),
      trigger: 'change'
    }
  ],
  inputMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutputDetail.inputminutes') }),
      trigger: 'change'
    }
  ],
  prodMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutputDetail.prodminutes') }),
      trigger: 'change'
    }
  ],
  actualMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutputDetail.actualminutes') }),
      trigger: 'change'
    }
  ],
  achievementRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyOutputDetail.achievementrate') }),
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
