<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/standard-operation-time-change-log/components -->
<!-- 文件名称：standard-operation-time-form.vue -->
<!-- 功能描述：标准工序时间实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form standard-operation-time-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="standard-operation-time-form-tabs"
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
                :label="t('entity.standardoperationtime.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.plantcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.standardOperationTimeId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardoperationtime.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.standardOperationTimeId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardoperationtime.workcenter')"
                name="workCenter"
              >
                <a-input
                  v-model:value="formState.workCenter"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.workcenter') })"
                  show-count
                  :maxlength="8"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardoperationtime.operationdesc')"
                name="operationDesc"
              >
                <a-input
                  v-model:value="formState.operationDesc"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.operationdesc') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardoperationtime.standardminutes')"
                name="standardMinutes"
              >
                <a-input-number
                  v-model:value="formState.standardMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.standardminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardoperationtime.timeunit')"
                name="timeUnit"
              >
                <a-input
                  v-model:value="formState.timeUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.timeunit') })"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.standardoperationtime.standardshorts')"
                name="standardShorts"
              >
                <a-input-number
                  v-model:value="formState.standardShorts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.standardshorts') })"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.standardoperationtime.pointsunit')"
                name="pointsUnit"
              >
                <a-input
                  v-model:value="formState.pointsUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.pointsunit') })"
                  show-count
                  :maxlength="5"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.standardoperationtime.pointstominutesrate')"
                name="pointsToMinutesRate"
              >
                <TaktSelect
                  v-model:value="formState.pointsToMinutesRate"
                  dict-type="logistics_points_to_minutes_rate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.pointstominutesrate') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.standardoperationtime.convertedminutes')"
                name="convertedMinutes"
              >
                <a-input-number
                  v-model:value="formState.convertedMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.convertedminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.standardoperationtime.effectivedate')"
                name="effectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.effectiveDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.effectivedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.standardoperationtime.expirydate')"
                name="expiryDate"
              >
                <a-date-picker
                  v-model:value="formState.expiryDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.expirydate') })"
                  value-format="YYYY-MM-DD"
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
    <!-- 下：子表 changeLogs -->
    <TaktEditableTable
      ref="standardOperationTimeChangeLogTableRef"
      v-model="childStandardOperationTimeChangeLogRows"
      :columns="standardOperationTimeChangeLogFormColumns"
      :title="t('entity.standardoperationtimechangelog._self')"
      :add-button-entity="t('entity.standardoperationtimechangelog._self')"
      id-field="standardOperationTimeChangeLogId"
      :default-row="createDefaultStandardOperationTimeChangeLogRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 标准工序时间实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/standard-operation-time-change-log/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { StandardOperationTimeCreate } from '@/types/logistics/manufacturing/bom/standard-operation-time'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","materialCode","workCenter","operationDesc","standardMinutes","timeUnit","standardShorts","pointsUnit","pointsToMinutesRate","convertedMinutes","effectiveDate","expiryDate","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childStandardOperationTimeChangeLogRows = ref<Record<string, unknown>[]>([])
const standardOperationTimeChangeLogTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 standardOperationTimeChangeLog 可编辑列 */
const standardOperationTimeChangeLogFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'materialCode',
    title: t('entity.standardoperationtimechangelog.materialcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'changeFields',
    title: t('entity.standardoperationtimechangelog.changefields'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.standardoperationtimechangelog.changefields') }),
  },
  {
    key: 'changeTime',
    title: t('entity.standardoperationtimechangelog.changetime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'changeBy',
    title: t('entity.standardoperationtimechangelog.changeby'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.standardoperationtimechangelog.changeby') }),
  },
  {
    key: 'changeReason',
    title: t('entity.standardoperationtimechangelog.changereason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.standardoperationtimechangelog.changereason') }),
  },
  {
    key: 'extField',
    title: t('common.page.entity.extfield'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.extfield') }),
    width: 140,
  },
  {
    key: 'remark',
    title: t('common.page.entity.remark'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') }),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<StandardOperationTimeCreate & { standardOperationTimeId?: string }> | null | undefined) {
  childStandardOperationTimeChangeLogRows.value = ((val as any)?.changeLogs ?? []) as Record<string, unknown>[]
}

function createDefaultStandardOperationTimeChangeLogRow(): Record<string, unknown> {
  return {
    materialCode: '',
    changeFields: '',
    changeTime: '',
    changeBy: '',
    changeReason: '',
    extField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.standardOperationTimeId ?? ''
  return {
    ...formState,
    changeLogs: standardOperationTimeChangeLogTableRef.value?.getRows?.() ?? childStandardOperationTimeChangeLogRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      standardOperationTimeId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<StandardOperationTimeCreate & { standardOperationTimeId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 standardOperationTimeId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.standardOperationTimeId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).changeLogs
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
    const isCreate = !props.formData?.standardOperationTimeId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.plantcode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.materialcode') }),
      trigger: 'blur'
    }
  ],
  workCenter: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.workcenter') }),
      trigger: 'blur'
    }
  ],
  standardMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.standardminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.standardminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  timeUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.timeunit') }),
      trigger: 'blur'
    }
  ],
  standardShorts: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.standardshorts') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.standardshorts') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pointsUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.standardoperationtime.pointsunit') }),
      trigger: 'blur'
    }
  ],
  pointsToMinutesRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.pointstominutesrate') }),
      trigger: 'change'
    }
  ],
  convertedMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.convertedminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.convertedminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  effectiveDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.standardoperationtime.effectivedate') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await standardOperationTimeChangeLogTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('standardMinutes' in payload) {
    const rawstandardMinutes = payload.standardMinutes
    payload.standardMinutes = typeof rawstandardMinutes === 'number' ? rawstandardMinutes : Number(rawstandardMinutes)
  }
  if ('standardShorts' in payload) {
    const rawstandardShorts = payload.standardShorts
    payload.standardShorts = typeof rawstandardShorts === 'number' ? rawstandardShorts : Number(rawstandardShorts)
  }
  if ('convertedMinutes' in payload) {
    const rawconvertedMinutes = payload.convertedMinutes
    payload.convertedMinutes = typeof rawconvertedMinutes === 'number' ? rawconvertedMinutes : Number(rawconvertedMinutes)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.standardOperationTimeId)
  childStandardOperationTimeChangeLogRows.value = []
  standardOperationTimeChangeLogTableRef.value?.resetRows?.()
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
