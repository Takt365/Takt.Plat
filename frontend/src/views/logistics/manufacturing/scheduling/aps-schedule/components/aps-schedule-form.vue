<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/scheduling/aps-schedule/components -->
<!-- 文件名称：aps-schedule-form.vue -->
<!-- 功能描述：APS排程主表维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="aps-schedule-form-tabs"
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
                :label="t('entity.apsSchedule.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.schedulecode')"
                name="scheduleCode"
              >
                <a-input
                  v-model:value="formState.scheduleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.schedulename')"
                name="scheduleName"
              >
                <a-input
                  v-model:value="formState.scheduleName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.scheduletype')"
                name="scheduleType"
              >
                <a-input-number
                  v-model:value="formState.scheduleType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.scheduletype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.plandate')"
                name="planDate"
              >
                <a-date-picker
                  v-model:value="formState.planDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.plandate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.planstarttime')"
                name="planStartTime"
              >
                <a-input
                  v-model:value="formState.planStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.planstarttime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.planendtime')"
                name="planEndTime"
              >
                <a-input
                  v-model:value="formState.planEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.planendtime') })"
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
                :label="t('entity.apsSchedule.plancycle')"
                name="planCycle"
              >
                <a-input-number
                  v-model:value="formState.planCycle"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plancycle') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.workshopcode')"
                name="workshopCode"
              >
                <a-input
                  v-model:value="formState.workshopCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.workshopcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.workshopname')"
                name="workshopName"
              >
                <a-input
                  v-model:value="formState.workshopName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.workshopname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.productionlinecode')"
                name="productionLineCode"
              >
                <a-input
                  v-model:value="formState.productionLineCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.productionlinecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.productionlinename')"
                name="productionLineName"
              >
                <a-input
                  v-model:value="formState.productionLineName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.productionlinename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.schedulestrategy')"
                name="scheduleStrategy"
              >
                <a-input-number
                  v-model:value="formState.scheduleStrategy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulestrategy') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.schedulealgorithm')"
                name="scheduleAlgorithm"
              >
                <a-input-number
                  v-model:value="formState.scheduleAlgorithm"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulealgorithm') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.optimizationobjective')"
                name="optimizationObjective"
              >
                <a-input-number
                  v-model:value="formState.optimizationObjective"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.optimizationobjective') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.schedulestatus')"
                name="scheduleStatus"
              >
                <a-input-number
                  v-model:value="formState.scheduleStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulestatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.plannerid')"
                name="plannerId"
              >
                <a-input
                  v-model:value="formState.plannerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plannerid') })"
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
                :label="t('entity.apsSchedule.plannername')"
                name="plannerName"
              >
                <a-input
                  v-model:value="formState.plannerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plannername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.publishtime')"
                name="publishTime"
              >
                <a-input
                  v-model:value="formState.publishTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.publishtime') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.publishuserid')"
                name="publishUserId"
              >
                <a-input
                  v-model:value="formState.publishUserId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.publishuserid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.publishusername')"
                name="publishUserName"
              >
                <a-input
                  v-model:value="formState.publishUserName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.publishusername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.apsSchedule.scheduledescription')"
                name="scheduleDescription"
              >
                <a-textarea
                  v-model:value="formState.scheduleDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.apsSchedule.scheduledescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.items')"
                name="items"
              >
                <a-input
                  v-model:value="formState.items"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.items') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsSchedule.changelogs')"
                name="changeLogs"
              >
                <a-input
                  v-model:value="formState.changeLogs"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.changelogs') })"
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
 * APS排程主表维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/scheduling/aps-schedule/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ApsScheduleCreate } from '@/types/logistics/manufacturing/scheduling/aps-schedule'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","scheduleCode","scheduleName","scheduleType","planDate","planStartTime","planEndTime","planCycle","workshopCode","workshopName","productionLineCode","productionLineName","scheduleStrategy","scheduleAlgorithm","optimizationObjective","scheduleStatus","plannerId","plannerName","publishTime","publishUserId","publishUserName","scheduleDescription","items","changeLogs","extFieldJson","remark"]


interface Props {
  formData?: Partial<ApsScheduleCreate & { apsScheduleId?: string }> | null
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
    const isCreate = !props.formData?.apsScheduleId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.plantcode') }),
      trigger: 'blur'
    }
  ],
  scheduleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulecode') }),
      trigger: 'blur'
    }
  ],
  scheduleName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.schedulename') }),
      trigger: 'blur'
    }
  ],
  scheduleType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.scheduletype') }),
      trigger: 'change'
    }
  ],
  planDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.plandate') }),
      trigger: 'change'
    }
  ],
  planStartTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.planstarttime') }),
      trigger: 'blur'
    }
  ],
  planEndTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsSchedule.planendtime') }),
      trigger: 'blur'
    }
  ],
  planCycle: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.plancycle') }),
      trigger: 'change'
    }
  ],
  scheduleStrategy: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.schedulestrategy') }),
      trigger: 'change'
    }
  ],
  scheduleAlgorithm: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.schedulealgorithm') }),
      trigger: 'change'
    }
  ],
  optimizationObjective: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.optimizationobjective') }),
      trigger: 'change'
    }
  ],
  scheduleStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsSchedule.schedulestatus') }),
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
