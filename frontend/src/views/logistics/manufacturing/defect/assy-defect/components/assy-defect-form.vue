<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/assy-defect/components -->
<!-- 文件名称：assy-defect-form.vue -->
<!-- 功能描述：组立不良日报实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="assy-defect-form-tabs"
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
                :label="t('entity.assyDefect.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.prodcategory')"
                name="prodCategory"
              >
                <a-input
                  v-model:value="formState.prodCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.prodcategory') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.proddate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.assyDefect.proddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.prodline')"
                name="prodLine"
              >
                <a-input
                  v-model:value="formState.prodLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.prodline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.shiftno') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.prodordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.prodorderqty')"
                name="prodOrderQty"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.prodorderqty') })"
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
                :label="t('entity.assyDefect.modelcode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.modelcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.batchno')"
                name="batchNo"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.batchno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.prodactualqty')"
                name="prodActualQty"
              >
                <a-input-number
                  v-model:value="formState.prodActualQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.prodactualqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.goodquantity')"
                name="goodQuantity"
              >
                <a-input-number
                  v-model:value="formState.goodQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.goodquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.status')"
                name="status"
              >
                <a-input-number
                  v-model:value="formState.status"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.assyDefect.details')"
                name="assyDefectDetails"
              >
                <a-input
                  v-model:value="formState.assyDefectDetails"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.assyDefect.details') })"
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
 * 组立不良日报实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/assy-defect/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { AssyDefectCreate } from '@/types/logistics/manufacturing/defect/assy-defect'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","prodCategory","prodDate","prodLine","shiftNo","prodOrderCode","prodOrderQty","modelCode","batchNo","materialCode","prodActualQty","goodQuantity","status","assyDefectDetails","extFieldJson","remark"]


interface Props {
  formData?: Partial<AssyDefectCreate & { assyDefectId?: string }> | null
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
    const isCreate = !props.formData?.assyDefectId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyDefect.plantcode') }),
      trigger: 'blur'
    }
  ],
  prodCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyDefect.prodcategory') }),
      trigger: 'blur'
    }
  ],
  prodDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyDefect.proddate') }),
      trigger: 'change'
    }
  ],
  prodLine: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyDefect.prodline') }),
      trigger: 'blur'
    }
  ],
  shiftNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyDefect.shiftno') }),
      trigger: 'change'
    }
  ],
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyDefect.prodordercode') }),
      trigger: 'blur'
    }
  ],
  prodOrderQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyDefect.prodorderqty') }),
      trigger: 'change'
    }
  ],
  modelCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyDefect.modelcode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.assyDefect.materialcode') }),
      trigger: 'blur'
    }
  ],
  prodActualQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyDefect.prodactualqty') }),
      trigger: 'change'
    }
  ],
  goodQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyDefect.goodquantity') }),
      trigger: 'change'
    }
  ],
  status: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.assyDefect.status') }),
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
