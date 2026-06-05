<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order/components -->
<!-- 文件名称：ipqc-order-form.vue -->
<!-- 功能描述：IPQC制程检验单实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="ipqc-order-form-tabs"
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
                :label="t('entity.ipqcOrder.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.sourcecode')"
                name="sourceCode"
              >
                <a-input
                  v-model:value="formState.sourceCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.sourcecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.inspectiondate')"
                name="inspectionDate"
              >
                <a-date-picker
                  v-model:value="formState.inspectionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.inspectiondate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.code')"
                name="ipqcOrderCode"
              >
                <a-input
                  v-model:value="formState.ipqcOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.processcode')"
                name="processCode"
              >
                <a-input
                  v-model:value="formState.processCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.processname')"
                name="processName"
              >
                <a-input
                  v-model:value="formState.processName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.totalproductionquantity')"
                name="totalProductionQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalProductionQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalproductionquantity') })"
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
                :label="t('entity.ipqcOrder.totalsamplequantity')"
                name="totalSampleQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalSampleQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalsamplequantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.totalqualifiedquantity')"
                name="totalQualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalqualifiedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.totalunqualifiedquantity')"
                name="totalUnqualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalUnqualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalunqualifiedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.totalinspectionreturnquantity')"
                name="totalInspectionReturnQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalInspectionReturnQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.totalinspectionreturnquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.judgestatus')"
                name="judgeStatus"
              >
                <a-input-number
                  v-model:value="formState.judgeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.judgestatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.judgeby')"
                name="judgeBy"
              >
                <a-input
                  v-model:value="formState.judgeBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.judgeby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.judgedate')"
                name="judgeDate"
              >
                <a-date-picker
                  v-model:value="formState.judgeDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.judgedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ipqcOrder.judgedescription')"
                name="judgeDescription"
              >
                <a-textarea
                  v-model:value="formState.judgeDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcOrder.judgedescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.items')"
                name="items"
              >
                <a-input
                  v-model:value="formState.items"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.items') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrder.changelogs')"
                name="changeLogs"
              >
                <a-input
                  v-model:value="formState.changeLogs"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.changelogs') })"
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
 * IPQC制程检验单实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/operation/ipqc-order/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { IpqcOrderCreate } from '@/types/logistics/quality/operation/ipqc-order'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","sourceCode","inspectionDate","ipqcOrderCode","processCode","processName","totalProductionQuantity","totalSampleQuantity","totalQualifiedQuantity","totalUnqualifiedQuantity","totalInspectionReturnQuantity","judgeStatus","judgeBy","judgeDate","judgeDescription","items","changeLogs","extFieldJson","remark"]


interface Props {
  formData?: Partial<IpqcOrderCreate & { ipqcOrderId?: string }> | null
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
    const isCreate = !props.formData?.ipqcOrderId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.plantcode') }),
      trigger: 'blur'
    }
  ],
  sourceCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.sourcecode') }),
      trigger: 'blur'
    }
  ],
  ipqcOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.code') }),
      trigger: 'blur'
    }
  ],
  processCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processcode') }),
      trigger: 'blur'
    }
  ],
  processName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrder.processname') }),
      trigger: 'blur'
    }
  ],
  totalProductionQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.totalproductionquantity') }),
      trigger: 'change'
    }
  ],
  totalSampleQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.totalsamplequantity') }),
      trigger: 'change'
    }
  ],
  totalQualifiedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.totalqualifiedquantity') }),
      trigger: 'change'
    }
  ],
  totalUnqualifiedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.totalunqualifiedquantity') }),
      trigger: 'change'
    }
  ],
  totalInspectionReturnQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.totalinspectionreturnquantity') }),
      trigger: 'change'
    }
  ],
  judgeStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrder.judgestatus') }),
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
