<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/planning/master-demand-schedule/components -->
<!-- 文件名称：master-demand-schedule-form.vue -->
<!-- 功能描述：主需求计划 MDS 头表维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form master-demand-schedule-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="master-demand-schedule-form-tabs"
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
                :label="t('entity.masterdemandschedule.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandschedule.plantcode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.masterDemandScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandschedule.mdscode')"
                name="mdsCode"
              >
                <a-input
                  v-model:value="formState.mdsCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.masterdemandschedule.mdscode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.masterDemandScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandschedule.planperiodstart')"
                name="planPeriodStart"
              >
                <a-date-picker
                  v-model:value="formState.planPeriodStart"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterdemandschedule.planperiodstart') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandschedule.planperiodend')"
                name="planPeriodEnd"
              >
                <a-date-picker
                  v-model:value="formState.planPeriodEnd"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterdemandschedule.planperiodend') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandschedule.buckettype')"
                name="bucketType"
              >
                <TaktSelect
                  v-model:value="formState.bucketType"
                  dict-type="mps_time_bucket_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterdemandschedule.buckettype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.masterdemandschedule.schedulestatus')"
                name="scheduleStatus"
              >
                <TaktSelect
                  v-model:value="formState.scheduleStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.masterdemandschedule.schedulestatus') })"
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
    <!-- 下：子表 lines -->
    <TaktEditableTable
      ref="masterDemandScheduleLineTableRef"
      v-model="childMasterDemandScheduleLineRows"
      :columns="masterDemandScheduleLineFormColumns"
      :title="t('entity.masterdemandscheduleline._self')"
      :add-button-entity="t('entity.masterdemandscheduleline._self')"
      id-field="masterDemandScheduleLineId"
      :default-row="createDefaultMasterDemandScheduleLineRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 主需求计划 MDS 头表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/planning/master-demand-schedule/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { MasterDemandScheduleCreate } from '@/types/logistics/manufacturing/planning/master-demand-schedule'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","mdsCode","planPeriodStart","planPeriodEnd","bucketType","scheduleStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childMasterDemandScheduleLineRows = ref<Record<string, unknown>[]>([])
const masterDemandScheduleLineTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 masterDemandScheduleLine 可编辑列 */
const masterDemandScheduleLineFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'mdsCode',
    title: t('entity.masterdemandscheduleline.mdscode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'demandSourceType',
    title: t('entity.masterdemandscheduleline.demandsourcetype'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'salesOrderId',
    title: t('entity.masterdemandscheduleline.salesorderid'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.masterdemandscheduleline.salesorderid') }),
  },
  {
    key: 'salesOrderLineNumber',
    title: t('entity.masterdemandscheduleline.salesorderlinenumber'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'salesPlanId',
    title: t('entity.masterdemandscheduleline.salesplanid'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.masterdemandscheduleline.salesplanid') }),
  },
  {
    key: 'salesPlanLineNumber',
    title: t('entity.masterdemandscheduleline.salesplanlinenumber'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'materialCode',
    title: t('entity.masterdemandscheduleline.materialcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'bucketStart',
    title: t('entity.masterdemandscheduleline.bucketstart'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MasterDemandScheduleCreate & { masterDemandScheduleId?: string }> | null | undefined) {
  childMasterDemandScheduleLineRows.value = ((val as any)?.lines ?? []) as Record<string, unknown>[]
}

function createDefaultMasterDemandScheduleLineRow(): Record<string, unknown> {
  return {
    mdsCode: '',
    demandSourceType: 0,
    salesOrderId: '',
    salesOrderLineNumber: 0,
    salesPlanId: '',
    salesPlanLineNumber: 0,
    materialCode: '',
    bucketStart: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.masterDemandScheduleId ?? ''
  return {
    ...formState,
    lines: masterDemandScheduleLineTableRef.value?.getRows?.() ?? childMasterDemandScheduleLineRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      masterDemandScheduleId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MasterDemandScheduleCreate & { masterDemandScheduleId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  scheduleStatus: 1
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 masterDemandScheduleId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.masterDemandScheduleId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).lines
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
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
    const isCreate = !props.formData?.masterDemandScheduleId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.masterdemandschedule.plantcode') }),
      trigger: 'blur'
    }
  ],
  mdsCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.masterdemandschedule.mdscode') }),
      trigger: 'blur'
    }
  ],
  planPeriodStart: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.masterdemandschedule.planperiodstart') }),
      trigger: 'change'
    }
  ],
  planPeriodEnd: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.masterdemandschedule.planperiodend') }),
      trigger: 'change'
    }
  ],
  bucketType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.masterdemandschedule.buckettype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.masterdemandschedule.buckettype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scheduleStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.masterdemandschedule.schedulestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.masterdemandschedule.schedulestatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await masterDemandScheduleLineTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('bucketType' in payload) {
    const rawbucketType = payload.bucketType
    payload.bucketType = typeof rawbucketType === 'number' ? rawbucketType : Number(rawbucketType)
  }
  if ('scheduleStatus' in payload) {
    const rawscheduleStatus = payload.scheduleStatus
    payload.scheduleStatus = typeof rawscheduleStatus === 'number' ? rawscheduleStatus : Number(rawscheduleStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.masterDemandScheduleId)
  childMasterDemandScheduleLineRows.value = []
  masterDemandScheduleLineTableRef.value?.resetRows?.()
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
