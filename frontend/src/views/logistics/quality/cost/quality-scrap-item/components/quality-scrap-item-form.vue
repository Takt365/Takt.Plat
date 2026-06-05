<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/quality-scrap-item/components -->
<!-- 文件名称：quality-scrap-item-form.vue -->
<!-- 功能描述：品质废弃明细 - 废弃零件明细行维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="quality-scrap-item-form-tabs"
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
                :label="t('entity.qualityScrapItem.qualityscrapid')"
                name="qualityScrapId"
              >
                <a-input
                  v-model:value="formState.qualityScrapId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.qualityscrapid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.qualityscrapcode')"
                name="qualityScrapCode"
              >
                <a-input
                  v-model:value="formState.qualityScrapCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.qualityscrapcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.materialname')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.materialname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.scrapcost')"
                name="scrapCost"
              >
                <a-input-number
                  v-model:value="formState.scrapCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.scrapcost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.scrapsize')"
                name="scrapSize"
              >
                <a-input-number
                  v-model:value="formState.scrapSize"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.scrapsize') })"
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
                :label="t('entity.qualityScrapItem.partprice')"
                name="partPrice"
              >
                <a-input-number
                  v-model:value="formState.partPrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.partprice') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.scrapreasoncost')"
                name="scrapReasonCost"
              >
                <a-input-number
                  v-model:value="formState.scrapReasonCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.scrapreasoncost') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.freightcharges')"
                name="freightCharges"
              >
                <a-input-number
                  v-model:value="formState.freightCharges"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.freightcharges') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.otherexpenses')"
                name="otherExpenses"
              >
                <a-input-number
                  v-model:value="formState.otherExpenses"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.otherexpenses') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.reasonworktimeminutes')"
                name="reasonWorkTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.reasonWorkTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.reasonworktimeminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.tax')"
                name="tax"
              >
                <a-input-number
                  v-model:value="formState.tax"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.tax') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityScrapItem.reasonotherexpenses')"
                name="reasonOtherExpenses"
              >
                <a-input-number
                  v-model:value="formState.reasonOtherExpenses"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.reasonotherexpenses') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityScrapItem.scrapnote')"
                name="scrapNote"
              >
                <a-textarea
                  v-model:value="formState.scrapNote"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityScrapItem.scrapnote') })"
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

    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 品质废弃明细 - 废弃零件明细行维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/cost/quality-scrap-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityScrapItemCreate } from '@/types/logistics/quality/cost/quality-scrap-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","qualityScrapId","qualityScrapCode","lineNumber","materialCode","materialName","scrapCost","scrapSize","partPrice","scrapReasonCost","freightCharges","otherExpenses","reasonWorkTimeMinutes","tax","reasonOtherExpenses","scrapNote","extFieldJson","remark"]


interface Props {
  formData?: Partial<QualityScrapItemCreate & { qualityScrapItemId?: string }> | null
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
    const isCreate = !props.formData?.qualityScrapItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  qualityScrapId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.qualityscrapid') }),
      trigger: 'blur'
    }
  ],
  qualityScrapCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.qualityscrapcode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityScrapItem.linenumber') }),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityScrapItem.materialname') }),
      trigger: 'blur'
    }
  ],
  scrapCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityScrapItem.scrapcost') }),
      trigger: 'change'
    }
  ],
  scrapSize: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityScrapItem.scrapsize') }),
      trigger: 'change'
    }
  ],
  partPrice: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityScrapItem.partprice') }),
      trigger: 'change'
    }
  ],
  scrapReasonCost: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityScrapItem.scrapreasoncost') }),
      trigger: 'change'
    }
  ],
  freightCharges: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityScrapItem.freightcharges') }),
      trigger: 'change'
    }
  ],
  otherExpenses: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityScrapItem.otherexpenses') }),
      trigger: 'change'
    }
  ],
  reasonWorkTimeMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityScrapItem.reasonworktimeminutes') }),
      trigger: 'change'
    }
  ],
  tax: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityScrapItem.tax') }),
      trigger: 'change'
    }
  ],
  reasonOtherExpenses: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityScrapItem.reasonotherexpenses') }),
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
