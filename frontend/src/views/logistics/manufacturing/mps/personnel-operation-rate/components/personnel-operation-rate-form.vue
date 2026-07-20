<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mps/personnel-operation-rate/components -->
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
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.personnelOperationRateId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('timeCategory')"
                name="timeCategory"
              >
                <a-input-number
                  v-model:value="formState.timeCategory"
                  :placeholder="pi.ph('timeCategory')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('startDate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="pi.ph('startDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('endDate')"
                name="endDate"
              >
                <a-date-picker
                  v-model:value="formState.endDate"
                  :placeholder="pi.ph('endDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('weekNumber')"
                name="weekNumber"
              >
                <a-input-number
                  v-model:value="formState.weekNumber"
                  :placeholder="pi.ph('weekNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('monthNumber')"
                name="monthNumber"
              >
                <a-input-number
                  v-model:value="formState.monthNumber"
                  :placeholder="pi.ph('monthNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodTeam')"
                name="prodTeam"
              >
                <TaktSelect
                  v-model:value="formState.prodTeam"
                  api-url="TaktProductionTeams/options"
                  :placeholder="pi.ph('prodTeam')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodTeamName')"
                name="prodTeamName"
              >
                <a-input
                  v-model:value="formState.prodTeamName"
                  :placeholder="pi.ph('prodTeamName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shiftNo')"
                name="shiftNo"
              >
                <TaktSelect
                  v-model:value="formState.shiftNo"
                  dict-type="logistics_shift_category"
                  :placeholder="pi.ph('shiftNo')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedDirectPersonnelCount')"
                name="plannedDirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.plannedDirectPersonnelCount"
                  :placeholder="pi.ph('plannedDirectPersonnelCount')"
                  style="width: 100%"
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
                :label="pi.label('actualDirectPersonnelCount')"
                name="actualDirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.actualDirectPersonnelCount"
                  :placeholder="pi.ph('actualDirectPersonnelCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedIndirectPersonnelCount')"
                name="plannedIndirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.plannedIndirectPersonnelCount"
                  :placeholder="pi.ph('plannedIndirectPersonnelCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualIndirectPersonnelCount')"
                name="actualIndirectPersonnelCount"
              >
                <a-input-number
                  v-model:value="formState.actualIndirectPersonnelCount"
                  :placeholder="pi.ph('actualIndirectPersonnelCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedWorkTime')"
                name="plannedWorkTime"
              >
                <a-input-number
                  v-model:value="formState.plannedWorkTime"
                  :placeholder="pi.ph('plannedWorkTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualWorkTime')"
                name="actualWorkTime"
              >
                <a-input-number
                  v-model:value="formState.actualWorkTime"
                  :placeholder="pi.ph('actualWorkTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('breakTime')"
                name="breakTime"
              >
                <a-input-number
                  v-model:value="formState.breakTime"
                  :placeholder="pi.ph('breakTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('idleTime')"
                name="idleTime"
              >
                <a-input-number
                  v-model:value="formState.idleTime"
                  :placeholder="pi.ph('idleTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('personnelOperationRate')"
                name="personnelOperationRate"
              >
                <a-input-number
                  v-model:value="formState.personnelOperationRate"
                  :placeholder="pi.ph('personnelOperationRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedOutput')"
                name="plannedOutput"
              >
                <a-input-number
                  v-model:value="formState.plannedOutput"
                  :placeholder="pi.ph('plannedOutput')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualOutput')"
                name="actualOutput"
              >
                <a-input-number
                  v-model:value="formState.actualOutput"
                  :placeholder="pi.ph('actualOutput')"
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
                :label="pi.label('qualifiedQuantity')"
                name="qualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.qualifiedQuantity"
                  :placeholder="pi.ph('qualifiedQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectiveQuantity')"
                name="defectiveQuantity"
              >
                <a-input-number
                  v-model:value="formState.defectiveQuantity"
                  :placeholder="pi.ph('defectiveQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('yieldRate')"
                name="yieldRate"
              >
                <a-input-number
                  v-model:value="formState.yieldRate"
                  :placeholder="pi.ph('yieldRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workEfficiency')"
                name="workEfficiency"
              >
                <a-input-number
                  v-model:value="formState.workEfficiency"
                  :placeholder="pi.ph('workEfficiency')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('idleReasonType')"
                name="idleReasonType"
              >
                <a-input-number
                  v-model:value="formState.idleReasonType"
                  :placeholder="pi.ph('idleReasonType')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('idleReason')"
                name="idleReason"
              >
                <a-input
                  v-model:value="formState.idleReason"
                  :placeholder="pi.ph('idleReason')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('overtimeHours')"
                name="overtimeHours"
              >
                <a-input-number
                  v-model:value="formState.overtimeHours"
                  :placeholder="pi.ph('overtimeHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('teamLeader')"
                name="teamLeader"
              >
                <TaktSelect
                  v-model:value="formState.teamLeader"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('teamLeader')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supervisor')"
                name="supervisor"
              >
                <TaktSelect
                  v-model:value="formState.supervisor"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('supervisor')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('rateStatus')"
                name="rateStatus"
              >
                <a-input-number
                  v-model:value="formState.rateStatus"
                  :placeholder="pi.ph('rateStatus')"
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
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyDefaultCulture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="pi.ph('companyDefaultCulture')"
                  show-count
                  :maxlength="20"
                  disabled
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
 * @module views/logistics/manufacturing/mps/personnel-operation-rate/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePersonnelOperationRateI18n } from '../composables/use-personnel-operation-rate-i18n'

/** 实体字段 i18n */
const pi = usePersonnelOperationRateI18n()
import type { PersonnelOperationRateCreate } from '@/types/logistics/manufacturing/mps/personnel-operation-rate'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.companyDefaultCulture) {
    target.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')


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

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

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
    if (!props.formData?.personnelOperationRateId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  timeCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('timeCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('timeCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  startDate: [
    {
      required: true,
      message: pi.ph('startDate'),
      trigger: 'change'
    }
  ],
  endDate: [
    {
      required: true,
      message: pi.ph('endDate'),
      trigger: 'change'
    }
  ],
  prodTeam: [
    {
      required: true,
      message: pi.ph('prodTeam'),
      trigger: 'change'
    }
  ],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('shiftNo'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('shiftNo'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedDirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedDirectPersonnelCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedDirectPersonnelCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualDirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualDirectPersonnelCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualDirectPersonnelCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedIndirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedIndirectPersonnelCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedIndirectPersonnelCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualIndirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualIndirectPersonnelCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualIndirectPersonnelCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedWorkTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedWorkTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedWorkTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualWorkTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualWorkTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualWorkTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  breakTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('breakTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('breakTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  idleTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('idleTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('idleTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  personnelOperationRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('personnelOperationRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('personnelOperationRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedOutput: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedOutput'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedOutput'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualOutput: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualOutput'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualOutput'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  qualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('qualifiedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('qualifiedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectiveQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('defectiveQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('defectiveQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  yieldRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('yieldRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('yieldRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  workEfficiency: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('workEfficiency'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('workEfficiency'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  overtimeHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('overtimeHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('overtimeHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  rateStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('rateStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('rateStatus'))
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
  if ('rateStatus' in payload) {
    const rawrateStatus = payload.rateStatus
    payload.rateStatus = typeof rawrateStatus === 'number' ? rawrateStatus : Number(rawrateStatus)
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
