<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/profit-center/components -->
<!-- 文件名称：profit-center-form.vue -->
<!-- 功能描述：利润中心实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="profit-center-form-tabs"
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
                :label="t('entity.profitCenter.code')"
                name="profitCenterCode"
              >
                <a-input
                  v-model:value="formState.profitCenterCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.name')"
                name="profitCenterName"
              >
                <a-input
                  v-model:value="formState.profitCenterName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.parentid')"
                name="parentId"
              >
                <a-input
                  v-model:value="formState.parentId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.parentid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.managerid')"
                name="managerId"
              >
                <a-input
                  v-model:value="formState.managerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.managerid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.managername')"
                name="managerName"
              >
                <a-input
                  v-model:value="formState.managerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.managername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.deptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.deptname') })"
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
                :label="t('entity.profitCenter.level')"
                name="profitCenterLevel"
              >
                <a-input-number
                  v-model:value="formState.profitCenterLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.level') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.relatedplant') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.status')"
                name="profitCenterStatus"
              >
                <a-input-number
                  v-model:value="formState.profitCenterStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.validfrom')"
                name="validFrom"
              >
                <a-input
                  v-model:value="formState.validFrom"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.validfrom') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.validto')"
                name="validTo"
              >
                <a-input
                  v-model:value="formState.validTo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.validto') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.sortorder') })"
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
 * 利润中心实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/accounting/controlling/profit-center/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ProfitCenterCreate } from '@/types/accounting/controlling/profit-center'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","profitCenterCode","profitCenterName","parentId","managerId","managerName","deptId","deptName","profitCenterLevel","relatedPlant","profitCenterStatus","validFrom","validTo","sortOrder","extFieldJson","remark"]


interface Props {
  formData?: Partial<ProfitCenterCreate & { profitCenterId?: string }> | null
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
    const isCreate = !props.formData?.profitCenterId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  profitCenterCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.profitCenter.code') }),
      trigger: 'blur'
    }
  ],
  profitCenterName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.profitCenter.name') }),
      trigger: 'blur'
    }
  ],
  parentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.profitCenter.parentid') }),
      trigger: 'blur'
    }
  ],
  profitCenterLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.profitCenter.level') }),
      trigger: 'change'
    }
  ],
  profitCenterStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.profitCenter.status') }),
      trigger: 'change'
    }
  ],
  validFrom: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.profitCenter.validfrom') }),
      trigger: 'blur'
    }
  ],
  validTo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.profitCenter.validto') }),
      trigger: 'blur'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.profitCenter.sortorder') }),
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
