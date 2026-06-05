<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-price-item/components -->
<!-- 文件名称：sales-price-item-form.vue -->
<!-- 功能描述：Takt销售价格明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="sales-price-item-form-tabs"
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
                :label="t('entity.salesPriceItem.salespriceid')"
                name="salesPriceId"
              >
                <a-input
                  v-model:value="formState.salesPriceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.salespriceid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesPriceItem.salespricecode')"
                name="salesPriceCode"
              >
                <a-input
                  v-model:value="formState.salesPriceCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.salespricecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesPriceItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesPriceItem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesPriceItem.salesunit')"
                name="salesUnit"
              >
                <a-input
                  v-model:value="formState.salesUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.salesunit') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesPriceItem.salesprice')"
                name="salesPrice"
              >
                <a-input-number
                  v-model:value="formState.salesPrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.salesprice') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesPriceItem.minorderquantity')"
                name="minOrderQuantity"
              >
                <a-input-number
                  v-model:value="formState.minOrderQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.minorderquantity') })"
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
                :label="t('entity.salesPriceItem.maxorderquantity')"
                name="maxOrderQuantity"
              >
                <a-input-number
                  v-model:value="formState.maxOrderQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.maxorderquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salesPriceItem.scales')"
                name="scales"
              >
                <a-input
                  v-model:value="formState.scales"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.scales') })"
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
 * Takt销售价格明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/sales/sales-price-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SalesPriceItemCreate } from '@/types/logistics/sales/sales-price-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","salesPriceId","salesPriceCode","lineNumber","materialCode","salesUnit","salesPrice","minOrderQuantity","maxOrderQuantity","scales","extFieldJson","remark"]


interface Props {
  formData?: Partial<SalesPriceItemCreate & { salesPriceItemId?: string }> | null
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
    const isCreate = !props.formData?.salesPriceItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  salesPriceId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.salespriceid') }),
      trigger: 'blur'
    }
  ],
  salesPriceCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.salespricecode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesPriceItem.linenumber') }),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.materialcode') }),
      trigger: 'blur'
    }
  ],
  salesUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salesPriceItem.salesunit') }),
      trigger: 'blur'
    }
  ],
  salesPrice: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesPriceItem.salesprice') }),
      trigger: 'change'
    }
  ],
  minOrderQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesPriceItem.minorderquantity') }),
      trigger: 'change'
    }
  ],
  maxOrderQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salesPriceItem.maxorderquantity') }),
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
