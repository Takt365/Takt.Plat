<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mps/master-production-schedule/components -->
<!-- 文件名称：master-production-schedule-form.vue -->
<!-- 功能描述：主生产计划 MPS 头表维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form master-production-schedule-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="master-production-schedule-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.masterProductionScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('mpsCode')"
                name="mpsCode"
              >
                <a-input
                  v-model:value="formState.mpsCode"
                  :placeholder="pi.ph('mpsCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.masterProductionScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('masterDemandScheduleId')"
                name="masterDemandScheduleId"
              >
                <a-input
                  v-model:value="formState.masterDemandScheduleId"
                  :placeholder="pi.ph('masterDemandScheduleId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('mdsCode')"
                name="mdsCode"
              >
                <a-input
                  v-model:value="formState.mdsCode"
                  :placeholder="pi.ph('mdsCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.masterProductionScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('planPeriodStart')"
                name="planPeriodStart"
              >
                <a-date-picker
                  v-model:value="formState.planPeriodStart"
                  :placeholder="pi.ph('planPeriodStart')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('planPeriodEnd')"
                name="planPeriodEnd"
              >
                <a-date-picker
                  v-model:value="formState.planPeriodEnd"
                  :placeholder="pi.ph('planPeriodEnd')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('bucketType')"
                name="bucketType"
              >
                <TaktSelect
                  v-model:value="formState.bucketType"
                  dict-type="mps_time_bucket_type"
                  :placeholder="pi.ph('bucketType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('scheduleStatus')"
                name="scheduleStatus"
              >
                <TaktSelect
                  v-model:value="formState.scheduleStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="pi.ph('scheduleStatus')"
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
    <!-- 下：子表 lines -->
    <TaktEditableTable
      ref="masterProductionScheduleLineTableRef"
      v-model="childMasterProductionScheduleLineRows"
      :columns="masterProductionScheduleLineFormColumns"
      :title="masterProductionScheduleLinePi.self()"
      :add-button-entity="masterProductionScheduleLinePi.self()"
      id-field="masterProductionScheduleLineId"
      :default-row="createDefaultMasterProductionScheduleLineRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktMaterials/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="masterProductionScheduleLinePi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-unitOfMeasure="{ record }">
        <TaktSelect
          v-model:value="record.unitOfMeasure"
          dict-type="logistics_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="masterProductionScheduleLinePi.ph('unitOfMeasure')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 主生产计划 MPS 头表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/mps/master-production-schedule/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMasterProductionScheduleI18n } from '../composables/use-master-production-schedule-i18n'

/** 实体字段 i18n */
const pi = useMasterProductionScheduleI18n()

import type { MasterProductionScheduleCreate } from '@/types/logistics/manufacturing/mps/master-production-schedule'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","mpsCode","masterDemandScheduleId","mdsCode","planPeriodStart","planPeriodEnd","bucketType","scheduleStatus","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { useMasterProductionScheduleLineI18n } from '../composables/use-master-production-schedule-line-i18n'

const masterProductionScheduleLinePi = useMasterProductionScheduleLineI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childMasterProductionScheduleLineRows = ref<Record<string, unknown>[]>([])
const masterProductionScheduleLineTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 masterProductionScheduleLine 可编辑列 */
const masterProductionScheduleLineFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'mpsCode',
    title: masterProductionScheduleLinePi.label('mpsCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'masterDemandScheduleLineId',
    title: masterProductionScheduleLinePi.label('masterDemandScheduleLineId'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: masterProductionScheduleLinePi.ph('masterDemandScheduleLineId'),
  },
  {
    key: 'materialCode',
    title: masterProductionScheduleLinePi.label('materialCode'),
    width: 140,
  },
  {
    key: 'bucketStart',
    title: masterProductionScheduleLinePi.label('bucketStart'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'bucketEnd',
    title: masterProductionScheduleLinePi.label('bucketEnd'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'grossRequirement',
    title: masterProductionScheduleLinePi.label('grossRequirement'),
    width: 140,
  },
  {
    key: 'scheduledReceipts',
    title: masterProductionScheduleLinePi.label('scheduledReceipts'),
    width: 140,
  },
  {
    key: 'projectedOnHand',
    title: masterProductionScheduleLinePi.label('projectedOnHand'),
    width: 140,
  },
  {
    key: 'netRequirement',
    title: masterProductionScheduleLinePi.label('netRequirement'),
    width: 140,
  },
  {
    key: 'plannedOrderQuantity',
    title: masterProductionScheduleLinePi.label('plannedOrderQuantity'),
    width: 140,
  },
  {
    key: 'atpQuantity',
    title: masterProductionScheduleLinePi.label('atpQuantity'),
    width: 140,
  },
  {
    key: 'unitOfMeasure',
    title: masterProductionScheduleLinePi.label('unitOfMeasure'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MasterProductionScheduleCreate & { masterProductionScheduleId?: string }> | null | undefined) {
  const rows_masterProductionScheduleLine = ((val as any)?.lines ?? []) as Record<string, unknown>[]
  childMasterProductionScheduleLineRows.value = rows_masterProductionScheduleLine
}

function createDefaultMasterProductionScheduleLineRow(): Record<string, unknown> {
  return {
    mpsCode: '',
    masterDemandScheduleLineId: '',
    materialCode: '',
    bucketStart: '',
    bucketEnd: '',
    grossRequirement: 0,
    scheduledReceipts: 0,
    projectedOnHand: 0,
    netRequirement: 0,
    plannedOrderQuantity: 0,
    atpQuantity: 0,
    unitOfMeasure: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.masterProductionScheduleId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    lines: masterProductionScheduleLineTableRef.value?.getRows?.() ?? childMasterProductionScheduleLineRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      masterProductionScheduleId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MasterProductionScheduleCreate & { masterProductionScheduleId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 masterProductionScheduleId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.masterProductionScheduleId) {
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
    const isCreate = !props.formData?.masterProductionScheduleId
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
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  mpsCode: [
    {
      required: true,
      message: pi.ph('mpsCode'),
      trigger: 'blur'
    }
  ],
  planPeriodStart: [
    {
      required: true,
      message: pi.ph('planPeriodStart'),
      trigger: 'change'
    }
  ],
  planPeriodEnd: [
    {
      required: true,
      message: pi.ph('planPeriodEnd'),
      trigger: 'change'
    }
  ],
  bucketType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('bucketType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('bucketType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scheduleStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scheduleStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scheduleStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await masterProductionScheduleLineTableRef.value?.validate?.()
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.masterProductionScheduleId)
  childMasterProductionScheduleLineRows.value = []
  masterProductionScheduleLineTableRef.value?.resetRows?.()
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
