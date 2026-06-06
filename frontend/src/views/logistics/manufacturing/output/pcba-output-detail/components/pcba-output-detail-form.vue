<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output-detail/components -->
<!-- 文件名称：pcba-output-detail-form.vue -->
<!-- 功能描述：PCBA明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="pcba-output-detail-form-tabs"
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
                :label="t('entity.pcbaOutputDetail.pcbaoutputid')"
                name="pcbaOutputId"
              >
                <a-input
                  v-model:value="formState.pcbaOutputId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.pcbaoutputid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.prodordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.timeperiod')"
                name="timePeriod"
              >
                <a-input
                  v-model:value="formState.timePeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.timeperiod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.shiftno') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.pcbboardtype')"
                name="pcbBoardType"
              >
                <a-input
                  v-model:value="formState.pcbBoardType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.pcbboardtype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.panelside')"
                name="panelSide"
              >
                <a-input
                  v-model:value="formState.panelSide"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.panelside') })"
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
                :label="t('entity.pcbaOutputDetail.batchqty')"
                name="batchQty"
              >
                <a-input-number
                  v-model:value="formState.batchQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.batchqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.dailycompletedqty')"
                name="dailyCompletedQty"
              >
                <a-input-number
                  v-model:value="formState.dailyCompletedQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.dailycompletedqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.totalcompletedqty')"
                name="totalCompletedQty"
              >
                <a-input-number
                  v-model:value="formState.totalCompletedQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.totalcompletedqty') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.completedstatus')"
                name="completedStatus"
              >
                <a-input-number
                  v-model:value="formState.completedStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.completedstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.serialno')"
                name="serialNo"
              >
                <a-input
                  v-model:value="formState.serialNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.serialno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.defectcount')"
                name="defectCount"
              >
                <a-input-number
                  v-model:value="formState.defectCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.defectcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.inputminutes')"
                name="inputMinutes"
              >
                <a-input-number
                  v-model:value="formState.inputMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.inputminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.repairminutes')"
                name="repairMinutes"
              >
                <a-input-number
                  v-model:value="formState.repairMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.repairminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.switchcount')"
                name="switchCount"
              >
                <a-input-number
                  v-model:value="formState.switchCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.switchcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.switchtime')"
                name="switchTime"
              >
                <a-input-number
                  v-model:value="formState.switchTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.switchtime') })"
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
                :label="t('entity.pcbaOutputDetail.stoptime')"
                name="stopTime"
              >
                <a-input-number
                  v-model:value="formState.stopTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.stoptime') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.totalminutes')"
                name="totalMinutes"
              >
                <a-input-number
                  v-model:value="formState.totalMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.totalminutes') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.unachievedreason')"
                name="unachievedReason"
              >
                <a-input
                  v-model:value="formState.unachievedReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.unachievedreason') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.pcbaOutputDetail.unachieveddescription')"
                name="unachievedDescription"
              >
                <a-textarea
                  v-model:value="formState.unachievedDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.pcbaOutputDetail.unachieveddescription') })"
                  :rows="2"
                  size="small"
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
 * PCBA明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/output/pcba-output-detail/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PcbaOutputDetailCreate } from '@/types/logistics/manufacturing/output/pcba-output-detail'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","pcbaOutputId","prodOrderCode","lineNumber","timePeriod","shiftNo","pcbBoardType","panelSide","batchQty","dailyCompletedQty","totalCompletedQty","completedStatus","serialNo","defectCount","inputMinutes","repairMinutes","switchCount","switchTime","stopTime","totalMinutes","unachievedReason","unachievedDescription","extFieldJson","remark"]


interface Props {
  formData?: Partial<PcbaOutputDetailCreate & { pcbaOutputDetailId?: string }> | null
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
    const isCreate = !props.formData?.pcbaOutputDetailId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  pcbaOutputId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.pcbaoutputid') }),
      trigger: 'blur'
    }
  ],
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.prodordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.linenumber') }),
      trigger: 'change'
    }
  ],
  timePeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.timeperiod') }),
      trigger: 'blur'
    }
  ],
  shiftNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.shiftno') }),
      trigger: 'change'
    }
  ],
  pcbBoardType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.pcbboardtype') }),
      trigger: 'blur'
    }
  ],
  panelSide: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.panelside') }),
      trigger: 'blur'
    }
  ],
  batchQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.batchqty') }),
      trigger: 'change'
    }
  ],
  dailyCompletedQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.dailycompletedqty') }),
      trigger: 'change'
    }
  ],
  totalCompletedQty: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.totalcompletedqty') }),
      trigger: 'change'
    }
  ],
  completedStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.completedstatus') }),
      trigger: 'change'
    }
  ],
  serialNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaOutputDetail.serialno') }),
      trigger: 'blur'
    }
  ],
  defectCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.defectcount') }),
      trigger: 'change'
    }
  ],
  inputMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.inputminutes') }),
      trigger: 'change'
    }
  ],
  repairMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.repairminutes') }),
      trigger: 'change'
    }
  ],
  switchCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.switchcount') }),
      trigger: 'change'
    }
  ],
  switchTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.switchtime') }),
      trigger: 'change'
    }
  ],
  stopTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.stoptime') }),
      trigger: 'change'
    }
  ],
  totalMinutes: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaOutputDetail.totalminutes') }),
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
