<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-notice/components -->
<!-- 文件名称：ec-notice-form.vue -->
<!-- 功能描述：工程变更通知单实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="ec-notice-form-tabs"
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
                :label="t('entity.ecNotice.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.no')"
                name="ecNoticeNo"
              >
                <a-input
                  v-model:value="formState.ecNoticeNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.no') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.ecid')"
                name="ecId"
              >
                <a-input
                  v-model:value="formState.ecId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.ecid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.ecno')"
                name="ecNo"
              >
                <a-input
                  v-model:value="formState.ecNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.ecno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.ectitle')"
                name="ecTitle"
              >
                <a-input
                  v-model:value="formState.ecTitle"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.ectitle') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.date')"
                name="ecNoticeDate"
              >
                <a-date-picker
                  v-model:value="formState.ecNoticeDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecNotice.date') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.deptcodes')"
                name="ecNoticeDeptCodes"
              >
                <a-input
                  v-model:value="formState.ecNoticeDeptCodes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.deptcodes') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.deptnames')"
                name="ecNoticeDeptNames"
              >
                <a-input
                  v-model:value="formState.ecNoticeDeptNames"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.deptnames') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.notifierid')"
                name="ecNoticeNotifierId"
              >
                <a-input
                  v-model:value="formState.ecNoticeNotifierId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.notifierid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.notifiername')"
                name="ecNoticeNotifierName"
              >
                <a-input
                  v-model:value="formState.ecNoticeNotifierName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.notifiername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.method')"
                name="ecNoticeMethod"
              >
                <a-input-number
                  v-model:value="formState.ecNoticeMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.method') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.status')"
                name="ecNoticeStatus"
              >
                <a-input-number
                  v-model:value="formState.ecNoticeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.status') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.confirmerid')"
                name="ecNoticeConfirmerId"
              >
                <a-input
                  v-model:value="formState.ecNoticeConfirmerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.confirmerid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.confirmername')"
                name="ecNoticeConfirmerName"
              >
                <a-input
                  v-model:value="formState.ecNoticeConfirmerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.confirmername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.confirmdate')"
                name="ecNoticeConfirmDate"
              >
                <a-date-picker
                  v-model:value="formState.ecNoticeConfirmDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecNotice.confirmdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.confirmcomment')"
                name="ecNoticeConfirmComment"
              >
                <a-input
                  v-model:value="formState.ecNoticeConfirmComment"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.confirmcomment') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecNotice.requirefeedbackdate')"
                name="ecNoticeRequireFeedbackDate"
              >
                <a-date-picker
                  v-model:value="formState.ecNoticeRequireFeedbackDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecNotice.requirefeedbackdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
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
                :label="t('entity.ecNotice.flowinstanceid')"
                name="flowInstanceId"
              >
                <a-input
                  v-model:value="formState.flowInstanceId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecNotice.flowinstanceid') })"
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
 * 工程变更通知单实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-notice/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcNoticeCreate } from '@/types/logistics/manufacturing/engineering-change/ec-notice'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","ecNoticeNo","ecId","ecNo","ecTitle","ecNoticeDate","ecNoticeDeptCodes","ecNoticeDeptNames","ecNoticeNotifierId","ecNoticeNotifierName","ecNoticeMethod","ecNoticeStatus","ecNoticeConfirmerId","ecNoticeConfirmerName","ecNoticeConfirmDate","ecNoticeConfirmComment","ecNoticeRequireFeedbackDate","flowInstanceId","extFieldJson","remark"]


interface Props {
  formData?: Partial<EcNoticeCreate & { ecNoticeId?: string }> | null
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
    const isCreate = !props.formData?.ecNoticeId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecNotice.plantcode') }),
      trigger: 'blur'
    }
  ],
  ecNoticeNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecNotice.no') }),
      trigger: 'blur'
    }
  ],
  ecId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecNotice.ecid') }),
      trigger: 'blur'
    }
  ],
  ecNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecNotice.ecno') }),
      trigger: 'blur'
    }
  ],
  ecNoticeDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecNotice.date') }),
      trigger: 'change'
    }
  ],
  ecNoticeMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecNotice.method') }),
      trigger: 'change'
    }
  ],
  ecNoticeStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ecNotice.status') }),
      trigger: 'change'
    }
  ],
  flowInstanceId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecNotice.flowinstanceid') }),
      trigger: 'blur'
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
