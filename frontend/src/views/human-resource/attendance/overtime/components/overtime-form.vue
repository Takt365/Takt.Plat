<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/attendance/overtime/components -->
<!-- 文件名称：overtime-form.vue -->
<!-- 功能描述：加班申请维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form overtime-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="overtime-form-tabs"
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
                :label="t('entity.overtime.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.deptid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.deptname') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.date')"
                name="overtimeDate"
              >
                <a-date-picker
                  v-model:value="formState.overtimeDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.date') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.plannedstarttime')"
                name="plannedStartTime"
              >
                <a-input
                  v-model:value="formState.plannedStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.plannedstarttime') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.plannedendtime')"
                name="plannedEndTime"
              >
                <a-input
                  v-model:value="formState.plannedEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.plannedendtime') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.totalemployees')"
                name="totalEmployees"
              >
                <a-input-number
                  v-model:value="formState.totalEmployees"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.totalemployees') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.totalplannedhours')"
                name="totalPlannedHours"
              >
                <a-input-number
                  v-model:value="formState.totalPlannedHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.totalplannedhours') })"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.totalactualhours')"
                name="totalActualHours"
              >
                <a-input-number
                  v-model:value="formState.totalActualHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.totalactualhours') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.type')"
                name="overtimeType"
              >
                <TaktSelect
                  v-model:value="formState.overtimeType"
                  dict-type="hr_overtime_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.type') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.reason')"
                name="reason"
              >
                <a-input
                  v-model:value="formState.reason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.reason') })"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.relatedplant') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.handlingby')"
                name="handlingBy"
              >
                <a-input
                  v-model:value="formState.handlingBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingby') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.handlingat')"
                name="handlingAt"
              >
                <a-input
                  v-model:value="formState.handlingAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.handlingcomment')"
                name="handlingComment"
              >
                <a-input
                  v-model:value="formState.handlingComment"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingcomment') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.overtime.status')"
                name="overtimeStatus"
              >
                <TaktSelect
                  v-model:value="formState.overtimeStatus"
                  dict-type="sys_approval_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.overtime.status') })"
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="overtimeItemTableRef"
      v-model="childOvertimeItemRows"
      :columns="overtimeItemFormColumns"
      :title="t('entity.overtimeitem._self')"
      :add-button-entity="t('entity.overtimeitem._self')"
      id-field="overtimeItemId"
      :default-row="createDefaultOvertimeItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 加班申请维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/attendance/overtime/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { OvertimeCreate } from '@/types/human-resource/attendance/overtime'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","deptId","deptName","overtimeDate","plannedStartTime","plannedEndTime","totalEmployees","totalPlannedHours","totalActualHours","overtimeType","reason","relatedPlant","handlingBy","handlingAt","handlingComment","overtimeStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childOvertimeItemRows = ref<Record<string, unknown>[]>([])
const overtimeItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 overtimeItem 可编辑列 */
const overtimeItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: t('entity.overtimeitem.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'employeeId',
    title: t('entity.overtimeitem.employeeid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'employeeName',
    title: t('entity.overtimeitem.employeename'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'plannedHours',
    title: t('entity.overtimeitem.plannedhours'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'actualStartTime',
    title: t('entity.overtimeitem.actualstarttime'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.overtimeitem.actualstarttime') }),
  },
  {
    key: 'actualEndTime',
    title: t('entity.overtimeitem.actualendtime'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.overtimeitem.actualendtime') }),
  },
  {
    key: 'actualHours',
    title: t('entity.overtimeitem.actualhours'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'ExtField',
    title: t('entity.overtimeitem.extfield'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.optional', { field: t('entity.overtimeitem.extfield') }),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<OvertimeCreate & { overtimeId?: string }> | null | undefined) {
  childOvertimeItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultOvertimeItemRow(): Record<string, unknown> {
  return {
    lineNumber: (childOvertimeItemRows.value.length + 1) * 10,
    employeeId: '',
    employeeName: '',
    plannedHours: 0,
    actualStartTime: '',
    actualEndTime: '',
    actualHours: 0,
    ExtField: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.overtimeId ?? ''
  return {
    ...formState,
    items: overtimeItemTableRef.value?.getRows?.() ?? childOvertimeItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      overtimeId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<OvertimeCreate & { overtimeId?: string }> | null
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
  overtimeStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 overtimeId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.overtimeId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
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
    const isCreate = !props.formData?.overtimeId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  deptId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtime.deptid') }),
      trigger: 'blur'
    }
  ],
  overtimeDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.overtime.date') }),
      trigger: 'change'
    }
  ],
  plannedStartTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtime.plannedstarttime') }),
      trigger: 'blur'
    }
  ],
  plannedEndTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtime.plannedendtime') }),
      trigger: 'blur'
    }
  ],
  totalEmployees: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.overtime.totalemployees') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.overtime.totalemployees') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalPlannedHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.overtime.totalplannedhours') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.overtime.totalplannedhours') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalActualHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.overtime.totalactualhours') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.overtime.totalactualhours') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  overtimeType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.overtime.type') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.overtime.type') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  handlingBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.overtime.handlingby') }),
      trigger: 'blur'
    }
  ],
  overtimeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.overtime.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.overtime.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await overtimeItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('totalEmployees' in payload) {
    const rawtotalEmployees = payload.totalEmployees
    payload.totalEmployees = typeof rawtotalEmployees === 'number' ? rawtotalEmployees : Number(rawtotalEmployees)
  }
  if ('totalPlannedHours' in payload) {
    const rawtotalPlannedHours = payload.totalPlannedHours
    payload.totalPlannedHours = typeof rawtotalPlannedHours === 'number' ? rawtotalPlannedHours : Number(rawtotalPlannedHours)
  }
  if ('totalActualHours' in payload) {
    const rawtotalActualHours = payload.totalActualHours
    payload.totalActualHours = typeof rawtotalActualHours === 'number' ? rawtotalActualHours : Number(rawtotalActualHours)
  }
  if ('overtimeType' in payload) {
    const rawovertimeType = payload.overtimeType
    payload.overtimeType = typeof rawovertimeType === 'number' ? rawovertimeType : Number(rawovertimeType)
  }
  if ('overtimeStatus' in payload) {
    const rawovertimeStatus = payload.overtimeStatus
    payload.overtimeStatus = typeof rawovertimeStatus === 'number' ? rawovertimeStatus : Number(rawovertimeStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.overtimeId)
  childOvertimeItemRows.value = []
  overtimeItemTableRef.value?.resetRows?.()
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
