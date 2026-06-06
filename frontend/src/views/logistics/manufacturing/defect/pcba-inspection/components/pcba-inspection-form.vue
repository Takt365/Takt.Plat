<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection/components -->
<!-- 文件名称：pcba-inspection-form.vue -->
<!-- 功能描述：PCBA检查日报实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="pcba-inspection-form-tabs"
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
                :label="t('entity.pcbaInspection.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspection.prodcategory')"
                name="prodCategory"
              >
                <a-input
                  v-model:value="formState.prodCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.prodcategory') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspection.proddate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbaInspection.proddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspection.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.prodordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspection.prodorderqty')"
                name="prodOrderQty"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.prodorderqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspection.modelcode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.modelcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspection.batchno')"
                name="batchNo"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.batchno') })"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspection.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspection.status')"
                name="status"
              >
                <a-input-number
                  v-model:value="formState.status"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaInspection.details')"
                name="pcbaInspectionDetails"
              >
                <a-input
                  v-model:value="formState.pcbaInspectionDetails"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.details') })"
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
 * PCBA检查日报实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/pcba-inspection/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PcbaInspectionCreate } from '@/types/logistics/manufacturing/defect/pcba-inspection'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","prodCategory","prodDate","prodOrderCode","prodOrderQty","modelCode","batchNo","materialCode","status","pcbaInspectionDetails","extFieldJson","remark"]


interface Props {
  formData?: Partial<PcbaInspectionCreate & { pcbaInspectionId?: string }> | null
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
    const isCreate = !props.formData?.pcbaInspectionId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.plantcode') }),
      trigger: 'blur'
    }
  ],
  prodCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.prodcategory') }),
      trigger: 'blur'
    }
  ],
  prodDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspection.proddate') }),
      trigger: 'change'
    }
  ],
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.prodordercode') }),
      trigger: 'blur'
    }
  ],
  prodOrderQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspection.prodorderqty') }),
      trigger: 'change'
    }
  ],
  modelCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.modelcode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaInspection.materialcode') }),
      trigger: 'blur'
    }
  ],
  status: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaInspection.status') }),
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
