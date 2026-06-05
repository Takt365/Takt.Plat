<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/fqc-defect-handling/components -->
<!-- 文件名称：fqc-defect-handling-form.vue -->
<!-- 功能描述：FQC出货检验不良处理记录实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="fqc-defect-handling-form-tabs"
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
                :label="t('entity.fqcDefectHandling.code')"
                name="fqcDefectHandlingCode"
              >
                <a-input
                  v-model:value="formState.fqcDefectHandlingCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.fqcorderitemid')"
                name="fqcOrderItemId"
              >
                <a-input
                  v-model:value="formState.fqcOrderItemId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.fqcorderitemid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.fqcordercode')"
                name="fqcOrderCode"
              >
                <a-input
                  v-model:value="formState.fqcOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.fqcordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.defecttype')"
                name="defectType"
              >
                <a-input-number
                  v-model:value="formState.defectType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.defecttype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.defectcode')"
                name="defectCode"
              >
                <a-input
                  v-model:value="formState.defectCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.defectcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.fqcDefectHandling.defectdescription')"
                name="defectDescription"
              >
                <a-textarea
                  v-model:value="formState.defectDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.fqcDefectHandling.defectdescription') })"
                  :rows="2"
                  size="small"
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
                :label="t('entity.fqcDefectHandling.defectquantity')"
                name="defectQuantity"
              >
                <a-input-number
                  v-model:value="formState.defectQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.defectquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.handlingmethod')"
                name="handlingMethod"
              >
                <a-input-number
                  v-model:value="formState.handlingMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.handlingmethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.fqcDefectHandling.handlingdescription')"
                name="handlingDescription"
              >
                <a-textarea
                  v-model:value="formState.handlingDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.fqcDefectHandling.handlingdescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.responsibledept')"
                name="responsibleDept"
              >
                <a-input
                  v-model:value="formState.responsibleDept"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.responsibledept') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.responsibleby')"
                name="responsibleBy"
              >
                <a-input
                  v-model:value="formState.responsibleBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.responsibleby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.handlerby')"
                name="handlerBy"
              >
                <a-input
                  v-model:value="formState.handlerBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.handlerby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.handlingat')"
                name="handlingAt"
              >
                <a-input
                  v-model:value="formState.handlingAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.handlingat') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.handlingstatus')"
                name="handlingStatus"
              >
                <a-input-number
                  v-model:value="formState.handlingStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.handlingstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.correctiveaction')"
                name="correctiveAction"
              >
                <a-input
                  v-model:value="formState.correctiveAction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.correctiveaction') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcDefectHandling.defectimages')"
                name="defectImages"
              >
                <a-input
                  v-model:value="formState.defectImages"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.defectimages') })"
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
 * FQC出货检验不良处理记录实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/operation/fqc-defect-handling/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { FqcDefectHandlingCreate } from '@/types/logistics/quality/operation/fqc-defect-handling'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","fqcDefectHandlingCode","fqcOrderItemId","fqcOrderCode","lineNumber","defectType","defectCode","defectDescription","defectQuantity","handlingMethod","handlingDescription","responsibleDept","responsibleBy","handlerBy","handlingAt","handlingStatus","correctiveAction","defectImages","extFieldJson","remark"]


interface Props {
  formData?: Partial<FqcDefectHandlingCreate & { fqcDefectHandlingId?: string }> | null
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
    const isCreate = !props.formData?.fqcDefectHandlingId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  fqcDefectHandlingCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.code') }),
      trigger: 'blur'
    }
  ],
  fqcOrderItemId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.fqcorderitemid') }),
      trigger: 'blur'
    }
  ],
  fqcOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.fqcordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.fqcDefectHandling.linenumber') }),
      trigger: 'change'
    }
  ],
  defectType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.fqcDefectHandling.defecttype') }),
      trigger: 'change'
    }
  ],
  defectCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.defectcode') }),
      trigger: 'blur'
    }
  ],
  defectDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcDefectHandling.defectdescription') }),
      trigger: 'blur'
    }
  ],
  defectQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.fqcDefectHandling.defectquantity') }),
      trigger: 'change'
    }
  ],
  handlingMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.fqcDefectHandling.handlingmethod') }),
      trigger: 'change'
    }
  ],
  handlingStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.fqcDefectHandling.handlingstatus') }),
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
