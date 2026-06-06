<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/production-team/components -->
<!-- 文件名称：production-team-form.vue -->
<!-- 功能描述：生产班组实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="production-team-form-tabs"
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
                :label="t('entity.productionTeam.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionTeam.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionTeam.teamcode')"
                name="teamCode"
              >
                <a-input
                  v-model:value="formState.teamCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionTeam.teamcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionTeam.teamname')"
                name="teamName"
              >
                <a-input
                  v-model:value="formState.teamName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionTeam.teamname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionTeam.teamcategory')"
                name="teamCategory"
              >
                <a-input
                  v-model:value="formState.teamCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionTeam.teamcategory') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionTeam.teamcategoryname')"
                name="teamCategoryName"
              >
                <a-input
                  v-model:value="formState.teamCategoryName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionTeam.teamcategoryname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionTeam.productionline')"
                name="productionLine"
              >
                <a-input
                  v-model:value="formState.productionLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionTeam.productionline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionTeam.teamleaderid')"
                name="teamLeaderId"
              >
                <a-input
                  v-model:value="formState.teamLeaderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionTeam.teamleaderid') })"
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
                :label="t('entity.productionTeam.teamleadername')"
                name="teamLeaderName"
              >
                <a-input
                  v-model:value="formState.teamLeaderName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionTeam.teamleadername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionTeam.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionTeam.shiftno') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionTeam.status')"
                name="status"
              >
                <a-input-number
                  v-model:value="formState.status"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionTeam.status') })"
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
 * 生产班组实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/output/production-team/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ProductionTeamCreate } from '@/types/logistics/manufacturing/output/production-team'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","teamCode","teamName","teamCategory","teamCategoryName","productionLine","teamLeaderId","teamLeaderName","shiftNo","status","extFieldJson","remark"]


interface Props {
  formData?: Partial<ProductionTeamCreate & { productionTeamId?: string }> | null
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
    const isCreate = !props.formData?.productionTeamId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productionTeam.plantcode') }),
      trigger: 'blur'
    }
  ],
  teamCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productionTeam.teamcode') }),
      trigger: 'blur'
    }
  ],
  teamName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productionTeam.teamname') }),
      trigger: 'blur'
    }
  ],
  status: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.productionTeam.status') }),
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
