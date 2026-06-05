<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/changeover/components -->
<!-- 文件名称：changeover-form.vue -->
<!-- 功能描述：切换记录实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="changeover-form-tabs"
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
                :label="t('entity.changeover.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.changeover.productioncategory')"
                name="productionCategory"
              >
                <a-input
                  v-model:value="formState.productionCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.productioncategory') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.changeover.productiondate')"
                name="productionDate"
              >
                <a-date-picker
                  v-model:value="formState.productionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.changeover.productiondate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.changeover.productionline')"
                name="productionLine"
              >
                <a-input
                  v-model:value="formState.productionLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.productionline') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.changeover.readsoptime')"
                name="readSopTime"
              >
                <a-input-number
                  v-model:value="formState.readSopTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.readsoptime') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.changeover.personcount')"
                name="personCount"
              >
                <a-input-number
                  v-model:value="formState.personCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.personcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.changeover.totalsoptime')"
                name="totalSopTime"
              >
                <a-input-number
                  v-model:value="formState.totalSopTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.totalsoptime') })"
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
                :label="t('entity.changeover.count')"
                name="changeoverCount"
              >
                <a-input-number
                  v-model:value="formState.changeoverCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.count') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.changeover.time')"
                name="changeoverTime"
              >
                <a-input-number
                  v-model:value="formState.changeoverTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.time') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.changeover.totalchangeovertime')"
                name="totalChangeoverTime"
              >
                <a-input-number
                  v-model:value="formState.totalChangeoverTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.changeover.totalchangeovertime') })"
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
 * 切换记录实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/output/changeover/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ChangeoverCreate } from '@/types/logistics/manufacturing/output/changeover'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","productionCategory","productionDate","productionLine","readSopTime","personCount","totalSopTime","changeoverCount","changeoverTime","totalChangeoverTime","extFieldJson","remark"]


interface Props {
  formData?: Partial<ChangeoverCreate & { changeoverId?: string }> | null
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
    const isCreate = !props.formData?.changeoverId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  productionDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.changeover.productiondate') }),
      trigger: 'change'
    }
  ],
  readSopTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.changeover.readsoptime') }),
      trigger: 'change'
    }
  ],
  personCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.changeover.personcount') }),
      trigger: 'change'
    }
  ],
  totalSopTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.changeover.totalsoptime') }),
      trigger: 'change'
    }
  ],
  changeoverCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.changeover.count') }),
      trigger: 'change'
    }
  ],
  changeoverTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.changeover.time') }),
      trigger: 'change'
    }
  ],
  totalChangeoverTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.changeover.totalchangeovertime') }),
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
