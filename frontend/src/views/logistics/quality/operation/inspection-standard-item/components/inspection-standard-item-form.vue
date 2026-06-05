<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/inspection-standard-item/components -->
<!-- 文件名称：inspection-standard-item-form.vue -->
<!-- 功能描述：检验标准明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="inspection-standard-item-form-tabs"
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
                :label="t('entity.inspectionStandardItem.inspectionstandardid')"
                name="inspectionStandardId"
              >
                <a-input
                  v-model:value="formState.inspectionStandardId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.inspectionstandardid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.itemcode')"
                name="itemCode"
              >
                <a-input
                  v-model:value="formState.itemCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.itemcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.itemname')"
                name="itemName"
              >
                <a-input
                  v-model:value="formState.itemName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.itemname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.itemtype')"
                name="itemType"
              >
                <a-input-number
                  v-model:value="formState.itemType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.itemtype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.defectlevel')"
                name="defectLevel"
              >
                <a-input
                  v-model:value="formState.defectLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.defectlevel') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.inspectionmode')"
                name="inspectionMode"
              >
                <a-input-number
                  v-model:value="formState.inspectionMode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.inspectionmode') })"
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
                :label="t('entity.inspectionStandardItem.standardvalue')"
                name="standardValue"
              >
                <a-input
                  v-model:value="formState.standardValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.standardvalue') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.upperlimit')"
                name="upperLimit"
              >
                <a-input
                  v-model:value="formState.upperLimit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.upperlimit') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.lowerlimit')"
                name="lowerLimit"
              >
                <a-input
                  v-model:value="formState.lowerLimit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.lowerlimit') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.inspectiontool')"
                name="inspectionTool"
              >
                <a-input
                  v-model:value="formState.inspectionTool"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.inspectiontool') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.inspectionStandardItem.inspectionmethoddescription')"
                name="inspectionMethodDescription"
              >
                <a-textarea
                  v-model:value="formState.inspectionMethodDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.inspectionStandardItem.inspectionmethoddescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.acceptancecriteria')"
                name="acceptanceCriteria"
              >
                <a-input
                  v-model:value="formState.acceptanceCriteria"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.acceptancecriteria') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.rejectioncriteria')"
                name="rejectionCriteria"
              >
                <a-input
                  v-model:value="formState.rejectionCriteria"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.rejectioncriteria') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.inspectionStandardItem.isqualifiedbasis')"
                name="isQualifiedBasis"
              >
                <a-input-number
                  v-model:value="formState.isQualifiedBasis"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.isqualifiedbasis') })"
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
 * 检验标准明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/operation/inspection-standard-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { InspectionStandardItemCreate } from '@/types/logistics/quality/operation/inspection-standard-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","inspectionStandardId","lineNumber","itemCode","itemName","itemType","defectLevel","inspectionMode","standardValue","upperLimit","lowerLimit","inspectionTool","inspectionMethodDescription","acceptanceCriteria","rejectionCriteria","isQualifiedBasis","extFieldJson","remark"]


interface Props {
  formData?: Partial<InspectionStandardItemCreate & { inspectionStandardItemId?: string }> | null
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
    const isCreate = !props.formData?.inspectionStandardItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  inspectionStandardId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.inspectionstandardid') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.inspectionStandardItem.linenumber') }),
      trigger: 'change'
    }
  ],
  itemCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.itemcode') }),
      trigger: 'blur'
    }
  ],
  itemName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.itemname') }),
      trigger: 'blur'
    }
  ],
  itemType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.inspectionStandardItem.itemtype') }),
      trigger: 'change'
    }
  ],
  defectLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.defectlevel') }),
      trigger: 'blur'
    }
  ],
  inspectionMode: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.inspectionStandardItem.inspectionmode') }),
      trigger: 'change'
    }
  ],
  standardValue: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.standardvalue') }),
      trigger: 'blur'
    }
  ],
  upperLimit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.upperlimit') }),
      trigger: 'blur'
    }
  ],
  lowerLimit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.lowerlimit') }),
      trigger: 'blur'
    }
  ],
  inspectionTool: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.inspectiontool') }),
      trigger: 'blur'
    }
  ],
  inspectionMethodDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.inspectionmethoddescription') }),
      trigger: 'blur'
    }
  ],
  acceptanceCriteria: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.acceptancecriteria') }),
      trigger: 'blur'
    }
  ],
  rejectionCriteria: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.inspectionStandardItem.rejectioncriteria') }),
      trigger: 'blur'
    }
  ],
  isQualifiedBasis: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.inspectionStandardItem.isqualifiedbasis') }),
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
