<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/sampling-scheme/components -->
<!-- 文件名称：sampling-scheme-form.vue -->
<!-- 功能描述：Takt抽样方案实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="sampling-scheme-form-tabs"
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
                :label="t('entity.samplingScheme.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.code')"
                name="samplingSchemeCode"
              >
                <a-input
                  v-model:value="formState.samplingSchemeCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.name')"
                name="samplingSchemeName"
              >
                <a-input
                  v-model:value="formState.samplingSchemeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.type')"
                name="samplingSchemeType"
              >
                <a-input-number
                  v-model:value="formState.samplingSchemeType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.type') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.samplingstandard')"
                name="samplingStandard"
              >
                <a-input-number
                  v-model:value="formState.samplingStandard"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.samplingstandard') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.inspectionlevel')"
                name="inspectionLevel"
              >
                <a-input-number
                  v-model:value="formState.inspectionLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.inspectionlevel') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.aqlvalue')"
                name="aqlValue"
              >
                <a-input-number
                  v-model:value="formState.aqlValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.aqlvalue') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.lotsizemin')"
                name="lotSizeMin"
              >
                <a-input-number
                  v-model:value="formState.lotSizeMin"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.lotsizemin') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.lotsizemax')"
                name="lotSizeMax"
              >
                <a-input-number
                  v-model:value="formState.lotSizeMax"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.lotsizemax') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.samplesize')"
                name="sampleSize"
              >
                <a-input-number
                  v-model:value="formState.sampleSize"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.samplesize') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.acceptancenumber')"
                name="acceptanceNumber"
              >
                <a-input-number
                  v-model:value="formState.acceptanceNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.acceptancenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.rejectionnumber')"
                name="rejectionNumber"
              >
                <a-input-number
                  v-model:value="formState.rejectionNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.rejectionnumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.inspectionstrictness')"
                name="inspectionStrictness"
              >
                <a-input-number
                  v-model:value="formState.inspectionStrictness"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.inspectionstrictness') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.istransferruleenabled')"
                name="isTransferRuleEnabled"
              >
                <a-input-number
                  v-model:value="formState.isTransferRuleEnabled"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.istransferruleenabled') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.transferruleconfig')"
                name="transferRuleConfig"
              >
                <a-input
                  v-model:value="formState.transferRuleConfig"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.transferruleconfig') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingScheme.status')"
                name="samplingSchemeStatus"
              >
                <a-input-number
                  v-model:value="formState.samplingSchemeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.samplingScheme.schemedescription')"
                name="schemeDescription"
              >
                <a-textarea
                  v-model:value="formState.schemeDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.samplingScheme.schemedescription') })"
                  :rows="2"
                  size="small"
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

    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt抽样方案实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/operation/sampling-scheme/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SamplingSchemeCreate } from '@/types/logistics/quality/operation/sampling-scheme'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","samplingSchemeCode","samplingSchemeName","samplingSchemeType","samplingStandard","inspectionLevel","aqlValue","lotSizeMin","lotSizeMax","sampleSize","acceptanceNumber","rejectionNumber","inspectionStrictness","isTransferRuleEnabled","transferRuleConfig","samplingSchemeStatus","schemeDescription","extFieldJson","remark"]


interface Props {
  formData?: Partial<SamplingSchemeCreate & { samplingSchemeId?: string }> | null
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
    const isCreate = !props.formData?.samplingSchemeId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.plantcode') }),
      trigger: 'blur'
    }
  ],
  samplingSchemeCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.code') }),
      trigger: 'blur'
    }
  ],
  samplingSchemeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.samplingScheme.name') }),
      trigger: 'blur'
    }
  ],
  samplingSchemeType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.type') }),
      trigger: 'change'
    }
  ],
  samplingStandard: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.samplingstandard') }),
      trigger: 'change'
    }
  ],
  inspectionLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.inspectionlevel') }),
      trigger: 'change'
    }
  ],
  aqlValue: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.aqlvalue') }),
      trigger: 'change'
    }
  ],
  lotSizeMin: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.lotsizemin') }),
      trigger: 'change'
    }
  ],
  lotSizeMax: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.lotsizemax') }),
      trigger: 'change'
    }
  ],
  sampleSize: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.samplesize') }),
      trigger: 'change'
    }
  ],
  acceptanceNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.acceptancenumber') }),
      trigger: 'change'
    }
  ],
  rejectionNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.rejectionnumber') }),
      trigger: 'change'
    }
  ],
  inspectionStrictness: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.inspectionstrictness') }),
      trigger: 'change'
    }
  ],
  isTransferRuleEnabled: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.istransferruleenabled') }),
      trigger: 'change'
    }
  ],
  samplingSchemeStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.samplingScheme.status') }),
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
