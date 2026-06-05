<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/scheduling/aps-schedule-item/components -->
<!-- 文件名称：aps-schedule-item-form.vue -->
<!-- 功能描述：APS排程明细维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="aps-schedule-item-form-tabs"
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
                :label="t('entity.apsScheduleItem.apsscheduleid')"
                name="apsScheduleId"
              >
                <a-input
                  v-model:value="formState.apsScheduleId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.apsscheduleid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.apsschedulecode')"
                name="apsScheduleCode"
              >
                <a-input
                  v-model:value="formState.apsScheduleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.apsschedulecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.workordercode')"
                name="workOrderCode"
              >
                <a-input
                  v-model:value="formState.workOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.workordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.productcode')"
                name="productCode"
              >
                <a-input
                  v-model:value="formState.productCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.productcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.productname')"
                name="productName"
              >
                <a-input
                  v-model:value="formState.productName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.productname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.workcentercode')"
                name="workCenterCode"
              >
                <a-input
                  v-model:value="formState.workCenterCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.workcentercode') })"
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
                :label="t('entity.apsScheduleItem.workcentername')"
                name="workCenterName"
              >
                <a-input
                  v-model:value="formState.workCenterName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.workcentername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.processcode')"
                name="processCode"
              >
                <a-input
                  v-model:value="formState.processCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.processcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.processname')"
                name="processName"
              >
                <a-input
                  v-model:value="formState.processName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.processname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.processsequence')"
                name="processSequence"
              >
                <a-input-number
                  v-model:value="formState.processSequence"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.processsequence') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.processstandardst')"
                name="processStandardST"
              >
                <a-input-number
                  v-model:value="formState.processStandardST"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.processstandardst') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.processstandardstunit')"
                name="processStandardSTUnit"
              >
                <a-input-number
                  v-model:value="formState.processStandardSTUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.processstandardstunit') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.extraminutes')"
                name="extraMinutes"
              >
                <a-input-number
                  v-model:value="formState.extraMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.extraminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.planquantity')"
                name="planQuantity"
              >
                <a-input-number
                  v-model:value="formState.planQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.planquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.planstarttime')"
                name="planStartTime"
              >
                <a-input
                  v-model:value="formState.planStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.planstarttime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.planendtime')"
                name="planEndTime"
              >
                <a-input
                  v-model:value="formState.planEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.planendtime') })"
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
                :label="t('entity.apsScheduleItem.actualstarttime')"
                name="actualStartTime"
              >
                <a-input
                  v-model:value="formState.actualStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.actualstarttime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.actualendtime')"
                name="actualEndTime"
              >
                <a-input
                  v-model:value="formState.actualEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.actualendtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.processstatus')"
                name="processStatus"
              >
                <a-input-number
                  v-model:value="formState.processStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.processstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsScheduleItem.priority')"
                name="priority"
              >
                <a-input-number
                  v-model:value="formState.priority"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.priority') })"
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
 * APS排程明细维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/scheduling/aps-schedule-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ApsScheduleItemCreate } from '@/types/logistics/manufacturing/scheduling/aps-schedule-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","apsScheduleId","apsScheduleCode","lineNumber","workOrderCode","productCode","productName","workCenterCode","workCenterName","processCode","processName","processSequence","processStandardST","processStandardSTUnit","extraMinutes","planQuantity","planStartTime","planEndTime","actualStartTime","actualEndTime","processStatus","priority","extFieldJson","remark"]


interface Props {
  formData?: Partial<ApsScheduleItemCreate & { apsScheduleItemId?: string }> | null
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
    const isCreate = !props.formData?.apsScheduleItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  apsScheduleId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.apsscheduleid') }),
      trigger: 'blur'
    }
  ],
  apsScheduleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.apsschedulecode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsScheduleItem.linenumber') }),
      trigger: 'change'
    }
  ],
  workOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.workordercode') }),
      trigger: 'blur'
    }
  ],
  productCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.productcode') }),
      trigger: 'blur'
    }
  ],
  productName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.productname') }),
      trigger: 'blur'
    }
  ],
  processCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.processcode') }),
      trigger: 'blur'
    }
  ],
  processName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.processname') }),
      trigger: 'blur'
    }
  ],
  processSequence: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsScheduleItem.processsequence') }),
      trigger: 'change'
    }
  ],
  processStandardST: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsScheduleItem.processstandardst') }),
      trigger: 'change'
    }
  ],
  processStandardSTUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsScheduleItem.processstandardstunit') }),
      trigger: 'change'
    }
  ],
  extraMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsScheduleItem.extraminutes') }),
      trigger: 'change'
    }
  ],
  planQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsScheduleItem.planquantity') }),
      trigger: 'change'
    }
  ],
  planStartTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.planstarttime') }),
      trigger: 'blur'
    }
  ],
  planEndTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsScheduleItem.planendtime') }),
      trigger: 'blur'
    }
  ],
  processStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsScheduleItem.processstatus') }),
      trigger: 'change'
    }
  ],
  priority: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsScheduleItem.priority') }),
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
