<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/standard-wage-rate/components -->
<!-- 文件名称：standard-wage-rate-form.vue -->
<!-- 功能描述：标准工资率实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="standard-wage-rate-form-tabs"
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
                :label="t('entity.standardWageRate.yearmonth')"
                name="yearMonth"
              >
                <a-input
                  v-model:value="formState.yearMonth"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.yearmonth') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.workingdays')"
                name="workingDays"
              >
                <a-input-number
                  v-model:value="formState.workingDays"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.workingdays') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.salesamount')"
                name="salesAmount"
              >
                <a-input-number
                  v-model:value="formState.salesAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.salesamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.directlaborcount')"
                name="directLaborCount"
              >
                <a-input-number
                  v-model:value="formState.directLaborCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.directlaborcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.directlaborwage')"
                name="directLaborWage"
              >
                <a-input-number
                  v-model:value="formState.directLaborWage"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.directlaborwage') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.directovertimehours')"
                name="directOvertimeHours"
              >
                <a-input-number
                  v-model:value="formState.directOvertimeHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.directovertimehours') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.directovertimetotal')"
                name="directOvertimeTotal"
              >
                <a-input-number
                  v-model:value="formState.directOvertimeTotal"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.directovertimetotal') })"
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
                :label="t('entity.standardWageRate.directwagerate')"
                name="directWageRate"
              >
                <a-input-number
                  v-model:value="formState.directWageRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.directwagerate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.indirectlaborcount')"
                name="indirectLaborCount"
              >
                <a-input-number
                  v-model:value="formState.indirectLaborCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.indirectlaborcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.indirectlaborwage')"
                name="indirectLaborWage"
              >
                <a-input-number
                  v-model:value="formState.indirectLaborWage"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.indirectlaborwage') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.indirectovertimehours')"
                name="indirectOvertimeHours"
              >
                <a-input-number
                  v-model:value="formState.indirectOvertimeHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.indirectovertimehours') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.indirectovertimetotal')"
                name="indirectOvertimeTotal"
              >
                <a-input-number
                  v-model:value="formState.indirectOvertimeTotal"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.indirectovertimetotal') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.indirectwagerate')"
                name="indirectWageRate"
              >
                <a-input-number
                  v-model:value="formState.indirectWageRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.indirectwagerate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardWageRate.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.relatedplant') })"
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

    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 标准工资率实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/accounting/controlling/standard-wage-rate/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { StandardWageRateCreate } from '@/types/accounting/controlling/standard-wage-rate'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","yearMonth","workingDays","salesAmount","directLaborCount","directLaborWage","directOvertimeHours","directOvertimeTotal","directWageRate","indirectLaborCount","indirectLaborWage","indirectOvertimeHours","indirectOvertimeTotal","indirectWageRate","relatedPlant","extFieldJson","remark"]


interface Props {
  formData?: Partial<StandardWageRateCreate & { standardWageRateId?: string }> | null
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
    const isCreate = !props.formData?.standardWageRateId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  yearMonth: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.standardWageRate.yearmonth') }),
      trigger: 'blur'
    }
  ],
  workingDays: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.workingdays') }),
      trigger: 'change'
    }
  ],
  salesAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.salesamount') }),
      trigger: 'change'
    }
  ],
  directLaborCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.directlaborcount') }),
      trigger: 'change'
    }
  ],
  directLaborWage: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.directlaborwage') }),
      trigger: 'change'
    }
  ],
  directOvertimeHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.directovertimehours') }),
      trigger: 'change'
    }
  ],
  directOvertimeTotal: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.directovertimetotal') }),
      trigger: 'change'
    }
  ],
  directWageRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.directwagerate') }),
      trigger: 'change'
    }
  ],
  indirectLaborCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.indirectlaborcount') }),
      trigger: 'change'
    }
  ],
  indirectLaborWage: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.indirectlaborwage') }),
      trigger: 'change'
    }
  ],
  indirectOvertimeHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.indirectovertimehours') }),
      trigger: 'change'
    }
  ],
  indirectOvertimeTotal: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.indirectovertimetotal') }),
      trigger: 'change'
    }
  ],
  indirectWageRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardWageRate.indirectwagerate') }),
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
