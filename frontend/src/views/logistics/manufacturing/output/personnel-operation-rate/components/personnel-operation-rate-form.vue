<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/personnel-operation-rate/components -->
<!-- 文件名称：personnel-operation-rate-form.vue -->
<!-- 功能描述：人员稼动率实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="personnel-operation-rate-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
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
                :label="t('entity.personnelOperationRate.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.plantcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.timecategory')"
                name="timeCategory"
              >
                <a-input-number
                  v-model:value="formState.timeCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.timecategory') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.startdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.enddate')"
                name="endDate"
              >
                <a-date-picker
                  v-model:value="formState.endDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.enddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.weeknumber')"
                name="weekNumber"
              >
                <a-input-number
                  v-model:value="formState.weekNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.weeknumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.monthnumber')"
                name="monthNumber"
              >
                <a-input-number
                  v-model:value="formState.monthNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.monthnumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.productionline')"
                name="productionLine"
              >
                <a-input
                  v-model:value="formState.productionLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.productionline') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.productionlinename')"
                name="productionLineName"
              >
                <a-input
                  v-model:value="formState.productionLineName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.productionlinename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.shiftno') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.planneddirectpersonnelcount')"
                name="plannedDirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.plannedDirectPersonnelCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.planneddirectpersonnelcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.actualdirectpersonnelcount')"
                name="actualDirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.actualDirectPersonnelCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.actualdirectpersonnelcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.plannedindirectpersonnelcount')"
                name="plannedIndirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.plannedIndirectPersonnelCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.plannedindirectpersonnelcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.actualindirectpersonnelcount')"
                name="actualIndirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.actualIndirectPersonnelCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.actualindirectpersonnelcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.plannedworktime')"
                name="plannedWorkTime"
              >
                <a-input-number
                  v-model:value="formState.plannedWorkTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.plannedworktime') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.actualworktime')"
                name="actualWorkTime"
              >
                <a-input-number
                  v-model:value="formState.actualWorkTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.actualworktime') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.breaktime')"
                name="breakTime"
              >
                <a-input-number
                  v-model:value="formState.breakTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.breaktime') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.idletime')"
                name="idleTime"
              >
                <a-input-number
                  v-model:value="formState.idleTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.idletime') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.personneloperationrate')"
                name="personnelOperationRate"
              >
                <a-input-number
                  v-model:value="formState.personnelOperationRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.personneloperationrate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.plannedoutput')"
                name="plannedOutput"
              >
                <a-input-number
                  v-model:value="formState.plannedOutput"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.plannedoutput') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.actualoutput')"
                name="actualOutput"
              >
                <a-input-number
                  v-model:value="formState.actualOutput"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.actualoutput') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.qualifiedquantity')"
                name="qualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.qualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.qualifiedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.defectivequantity')"
                name="defectiveQuantity"
              >
                <a-input-number
                  v-model:value="formState.defectiveQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.defectivequantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.yieldrate')"
                name="yieldRate"
              >
                <a-input-number
                  v-model:value="formState.yieldRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.yieldrate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.workefficiency')"
                name="workEfficiency"
              >
                <a-input-number
                  v-model:value="formState.workEfficiency"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.workefficiency') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.idlereasontype')"
                name="idleReasonType"
              >
                <a-input-number
                  v-model:value="formState.idleReasonType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.idlereasontype') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.idlereason')"
                name="idleReason"
              >
                <a-input
                  v-model:value="formState.idleReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.idlereason') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.overtimehours')"
                name="overtimeHours"
              >
                <a-input-number
                  v-model:value="formState.overtimeHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.overtimehours') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.teamleader')"
                name="teamLeader"
              >
                <a-input
                  v-model:value="formState.teamLeader"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.teamleader') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.supervisor')"
                name="supervisor"
              >
                <a-input
                  v-model:value="formState.supervisor"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.supervisor') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personnelOperationRate.status')"
                name="status"
              >
                <a-input-number
                  v-model:value="formState.status"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.status') })"
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
 * 人员稼动率实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/output/personnel-operation-rate/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PersonnelOperationRateCreate } from '@/types/logistics/manufacturing/output/personnel-operation-rate'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","timeCategory","startDate","endDate","weekNumber","monthNumber","productionLine","productionLineName","shiftNo","plannedDirectPersonnelCount","actualDirectPersonnelCount","plannedIndirectPersonnelCount","actualIndirectPersonnelCount","plannedWorkTime","actualWorkTime","breakTime","idleTime","personnelOperationRate","plannedOutput","actualOutput","qualifiedQuantity","defectiveQuantity","yieldRate","workEfficiency","idleReasonType","idleReason","overtimeHours","teamLeader","supervisor","status","extFieldJson","remark"]


interface Props {
  formData?: Partial<PersonnelOperationRateCreate & { personnelOperationRateId?: string }> | null
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
    const isCreate = !props.formData?.personnelOperationRateId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.plantcode') }),
      trigger: 'blur'
    }
  ],
  timeCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.timecategory') }),
      trigger: 'change'
    }
  ],
  startDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.startdate') }),
      trigger: 'change'
    }
  ],
  endDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.enddate') }),
      trigger: 'change'
    }
  ],
  productionLine: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.personnelOperationRate.productionline') }),
      trigger: 'blur'
    }
  ],
  shiftNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.shiftno') }),
      trigger: 'change'
    }
  ],
  plannedDirectPersonnelCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.planneddirectpersonnelcount') }),
      trigger: 'change'
    }
  ],
  actualDirectPersonnelCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.actualdirectpersonnelcount') }),
      trigger: 'change'
    }
  ],
  plannedIndirectPersonnelCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.plannedindirectpersonnelcount') }),
      trigger: 'change'
    }
  ],
  actualIndirectPersonnelCount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.actualindirectpersonnelcount') }),
      trigger: 'change'
    }
  ],
  plannedWorkTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.plannedworktime') }),
      trigger: 'change'
    }
  ],
  actualWorkTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.actualworktime') }),
      trigger: 'change'
    }
  ],
  breakTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.breaktime') }),
      trigger: 'change'
    }
  ],
  idleTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.idletime') }),
      trigger: 'change'
    }
  ],
  personnelOperationRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.personneloperationrate') }),
      trigger: 'change'
    }
  ],
  plannedOutput: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.plannedoutput') }),
      trigger: 'change'
    }
  ],
  actualOutput: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.actualoutput') }),
      trigger: 'change'
    }
  ],
  qualifiedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.qualifiedquantity') }),
      trigger: 'change'
    }
  ],
  defectiveQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.defectivequantity') }),
      trigger: 'change'
    }
  ],
  yieldRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.yieldrate') }),
      trigger: 'change'
    }
  ],
  workEfficiency: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.workefficiency') }),
      trigger: 'change'
    }
  ],
  overtimeHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.overtimehours') }),
      trigger: 'change'
    }
  ],
  status: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personnelOperationRate.status') }),
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
