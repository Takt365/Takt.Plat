<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/personnel-operation-rate/components -->
<!-- 文件名称：personnel-operation-rate-form.vue -->
<!-- 功能描述：人员稼动率实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
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
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.personnelOperationRateId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.timecategory')"
                name="timeCategory"
              >
                <a-input-number
                  v-model:value="formState.timeCategory"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.timecategory') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.startdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.enddate')"
                name="endDate"
              >
                <a-date-picker
                  v-model:value="formState.endDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.enddate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.weeknumber')"
                name="weekNumber"
              >
                <a-input-number
                  v-model:value="formState.weekNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.weeknumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.monthnumber')"
                name="monthNumber"
              >
                <a-input-number
                  v-model:value="formState.monthNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.monthnumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.productionline')"
                name="productionLine"
              >
                <a-input
                  v-model:value="formState.productionLine"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.productionline') })"
                  show-count
                  :maxlength="20"
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
                :label="t('entity.personneloperationrate.productionlinename')"
                name="productionLineName"
              >
                <a-input
                  v-model:value="formState.productionLineName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.productionlinename') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.shiftno')"
                name="shiftNo"
              >
                <a-input-number
                  v-model:value="formState.shiftNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.shiftno') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.planneddirectpersonnelcount')"
                name="plannedDirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.plannedDirectPersonnelCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.planneddirectpersonnelcount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.actualdirectpersonnelcount')"
                name="actualDirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.actualDirectPersonnelCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.actualdirectpersonnelcount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.plannedindirectpersonnelcount')"
                name="plannedIndirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.plannedIndirectPersonnelCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.plannedindirectpersonnelcount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.actualindirectpersonnelcount')"
                name="actualIndirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.actualIndirectPersonnelCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.actualindirectpersonnelcount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.plannedworktime')"
                name="plannedWorkTime"
              >
                <a-input-number
                  v-model:value="formState.plannedWorkTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.plannedworktime') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.actualworktime')"
                name="actualWorkTime"
              >
                <a-input-number
                  v-model:value="formState.actualWorkTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.actualworktime') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.breaktime')"
                name="breakTime"
              >
                <a-input-number
                  v-model:value="formState.breakTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.breaktime') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.idletime')"
                name="idleTime"
              >
                <a-input-number
                  v-model:value="formState.idleTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.idletime') })"
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
                :label="t('entity.personneloperationrate.personneloperationrate')"
                name="personnelOperationRate"
              >
                <a-input-number
                  v-model:value="formState.personnelOperationRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.personneloperationrate') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.plannedoutput')"
                name="plannedOutput"
              >
                <a-input-number
                  v-model:value="formState.plannedOutput"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.plannedoutput') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.actualoutput')"
                name="actualOutput"
              >
                <a-input-number
                  v-model:value="formState.actualOutput"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.actualoutput') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.qualifiedquantity')"
                name="qualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.qualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.qualifiedquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.defectivequantity')"
                name="defectiveQuantity"
              >
                <a-input-number
                  v-model:value="formState.defectiveQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.defectivequantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.yieldrate')"
                name="yieldRate"
              >
                <a-input-number
                  v-model:value="formState.yieldRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.yieldrate') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.workefficiency')"
                name="workEfficiency"
              >
                <a-input-number
                  v-model:value="formState.workEfficiency"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.workefficiency') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.idlereasontype')"
                name="idleReasonType"
              >
                <a-input-number
                  v-model:value="formState.idleReasonType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.idlereasontype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.idlereason')"
                name="idleReason"
              >
                <a-input
                  v-model:value="formState.idleReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.idlereason') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.personneloperationrate.overtimehours')"
                name="overtimeHours"
              >
                <a-input-number
                  v-model:value="formState.overtimeHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.overtimehours') })"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.personneloperationrate.teamleader')"
                name="teamLeader"
              >
                <a-input
                  v-model:value="formState.teamLeader"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.teamleader') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.personneloperationrate.supervisor')"
                name="supervisor"
              >
                <a-input
                  v-model:value="formState.supervisor"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.supervisor') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.personneloperationrate.status')"
                name="status"
              >
                <a-input-number
                  v-model:value="formState.status"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.status') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t('common.page.entity.extfield') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
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
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
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
 * 人员稼动率实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/personnel-operation-rate/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PersonnelOperationRateCreate } from '@/types/logistics/manufacturing/output/personnel-operation-rate'
import { RiQuestionLine } from '@remixicon/vue'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
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
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","timeCategory","startDate","endDate","weekNumber","monthNumber","productionLine","productionLineName","shiftNo","plannedDirectPersonnelCount","actualDirectPersonnelCount","plannedIndirectPersonnelCount","actualIndirectPersonnelCount","plannedWorkTime","actualWorkTime","breakTime","idleTime","personnelOperationRate","plannedOutput","actualOutput","qualifiedQuantity","defectiveQuantity","yieldRate","workEfficiency","idleReasonType","idleReason","overtimeHours","teamLeader","supervisor","status","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PersonnelOperationRateCreate & { personnelOperationRateId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 personnelOperationRateId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.personnelOperationRateId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.personnelOperationRateId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.plantcode') }),
      trigger: 'blur'
    }
  ],
  timeCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.timecategory') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.timecategory') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  startDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.startdate') }),
      trigger: 'change'
    }
  ],
  endDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.enddate') }),
      trigger: 'change'
    }
  ],
  productionLine: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.personneloperationrate.productionline') }),
      trigger: 'blur'
    }
  ],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.shiftno') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.shiftno') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedDirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.planneddirectpersonnelcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.planneddirectpersonnelcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualDirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.actualdirectpersonnelcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.actualdirectpersonnelcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedIndirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.plannedindirectpersonnelcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.plannedindirectpersonnelcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualIndirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.actualindirectpersonnelcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.actualindirectpersonnelcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedWorkTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.plannedworktime') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.plannedworktime') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualWorkTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.actualworktime') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.actualworktime') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  breakTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.breaktime') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.breaktime') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  idleTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.idletime') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.idletime') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  personnelOperationRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.personneloperationrate') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.personneloperationrate') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedOutput: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.plannedoutput') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.plannedoutput') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualOutput: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.actualoutput') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.actualoutput') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  qualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.qualifiedquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.qualifiedquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectiveQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.defectivequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.defectivequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  yieldRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.yieldrate') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.yieldrate') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  workEfficiency: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.workefficiency') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.workefficiency') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  overtimeHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.overtimehours') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.overtimehours') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  status: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.personneloperationrate.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('timeCategory' in payload) {
    const rawtimeCategory = payload.timeCategory
    payload.timeCategory = typeof rawtimeCategory === 'number' ? rawtimeCategory : Number(rawtimeCategory)
  }
  if ('weekNumber' in payload) {
    const rawweekNumber = payload.weekNumber
    payload.weekNumber = typeof rawweekNumber === 'number' ? rawweekNumber : Number(rawweekNumber)
  }
  if ('monthNumber' in payload) {
    const rawmonthNumber = payload.monthNumber
    payload.monthNumber = typeof rawmonthNumber === 'number' ? rawmonthNumber : Number(rawmonthNumber)
  }
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
  if ('plannedDirectPersonnelCount' in payload) {
    const rawplannedDirectPersonnelCount = payload.plannedDirectPersonnelCount
    payload.plannedDirectPersonnelCount = typeof rawplannedDirectPersonnelCount === 'number' ? rawplannedDirectPersonnelCount : Number(rawplannedDirectPersonnelCount)
  }
  if ('actualDirectPersonnelCount' in payload) {
    const rawactualDirectPersonnelCount = payload.actualDirectPersonnelCount
    payload.actualDirectPersonnelCount = typeof rawactualDirectPersonnelCount === 'number' ? rawactualDirectPersonnelCount : Number(rawactualDirectPersonnelCount)
  }
  if ('plannedIndirectPersonnelCount' in payload) {
    const rawplannedIndirectPersonnelCount = payload.plannedIndirectPersonnelCount
    payload.plannedIndirectPersonnelCount = typeof rawplannedIndirectPersonnelCount === 'number' ? rawplannedIndirectPersonnelCount : Number(rawplannedIndirectPersonnelCount)
  }
  if ('actualIndirectPersonnelCount' in payload) {
    const rawactualIndirectPersonnelCount = payload.actualIndirectPersonnelCount
    payload.actualIndirectPersonnelCount = typeof rawactualIndirectPersonnelCount === 'number' ? rawactualIndirectPersonnelCount : Number(rawactualIndirectPersonnelCount)
  }
  if ('plannedWorkTime' in payload) {
    const rawplannedWorkTime = payload.plannedWorkTime
    payload.plannedWorkTime = typeof rawplannedWorkTime === 'number' ? rawplannedWorkTime : Number(rawplannedWorkTime)
  }
  if ('actualWorkTime' in payload) {
    const rawactualWorkTime = payload.actualWorkTime
    payload.actualWorkTime = typeof rawactualWorkTime === 'number' ? rawactualWorkTime : Number(rawactualWorkTime)
  }
  if ('breakTime' in payload) {
    const rawbreakTime = payload.breakTime
    payload.breakTime = typeof rawbreakTime === 'number' ? rawbreakTime : Number(rawbreakTime)
  }
  if ('idleTime' in payload) {
    const rawidleTime = payload.idleTime
    payload.idleTime = typeof rawidleTime === 'number' ? rawidleTime : Number(rawidleTime)
  }
  if ('personnelOperationRate' in payload) {
    const rawpersonnelOperationRate = payload.personnelOperationRate
    payload.personnelOperationRate = typeof rawpersonnelOperationRate === 'number' ? rawpersonnelOperationRate : Number(rawpersonnelOperationRate)
  }
  if ('plannedOutput' in payload) {
    const rawplannedOutput = payload.plannedOutput
    payload.plannedOutput = typeof rawplannedOutput === 'number' ? rawplannedOutput : Number(rawplannedOutput)
  }
  if ('actualOutput' in payload) {
    const rawactualOutput = payload.actualOutput
    payload.actualOutput = typeof rawactualOutput === 'number' ? rawactualOutput : Number(rawactualOutput)
  }
  if ('qualifiedQuantity' in payload) {
    const rawqualifiedQuantity = payload.qualifiedQuantity
    payload.qualifiedQuantity = typeof rawqualifiedQuantity === 'number' ? rawqualifiedQuantity : Number(rawqualifiedQuantity)
  }
  if ('defectiveQuantity' in payload) {
    const rawdefectiveQuantity = payload.defectiveQuantity
    payload.defectiveQuantity = typeof rawdefectiveQuantity === 'number' ? rawdefectiveQuantity : Number(rawdefectiveQuantity)
  }
  if ('yieldRate' in payload) {
    const rawyieldRate = payload.yieldRate
    payload.yieldRate = typeof rawyieldRate === 'number' ? rawyieldRate : Number(rawyieldRate)
  }
  if ('workEfficiency' in payload) {
    const rawworkEfficiency = payload.workEfficiency
    payload.workEfficiency = typeof rawworkEfficiency === 'number' ? rawworkEfficiency : Number(rawworkEfficiency)
  }
  if ('idleReasonType' in payload) {
    const rawidleReasonType = payload.idleReasonType
    payload.idleReasonType = typeof rawidleReasonType === 'number' ? rawidleReasonType : Number(rawidleReasonType)
  }
  if ('overtimeHours' in payload) {
    const rawovertimeHours = payload.overtimeHours
    payload.overtimeHours = typeof rawovertimeHours === 'number' ? rawovertimeHours : Number(rawovertimeHours)
  }
  if ('status' in payload) {
    const rawstatus = payload.status
    payload.status = typeof rawstatus === 'number' ? rawstatus : Number(rawstatus)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.personnelOperationRateId)

  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
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
