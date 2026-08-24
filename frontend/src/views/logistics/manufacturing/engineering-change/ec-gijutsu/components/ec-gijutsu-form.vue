<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：ec-gijutsu-form.vue -->
<!-- 功能描述：设变技术课维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ec-gijutsu-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="ec-gijutsu-form-tabs"
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
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecCode')"
                name="ecCode"
              >
                <a-input
                  v-model:value="formState.ecCode"
                  :placeholder="pi.ph('ecCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.ecGijutsuId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecIssueDate')"
                name="ecIssueDate"
              >
                <a-date-picker
                  v-model:value="formState.ecIssueDate"
                  :placeholder="pi.ph('ecIssueDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('changeStatus')"
                name="changeStatus"
              >
                <a-input-number
                  v-model:value="formState.changeStatus"
                  :placeholder="pi.ph('changeStatus')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecTitle')"
                name="ecTitle"
              >
                <a-input
                  v-model:value="formState.ecTitle"
                  :placeholder="pi.ph('ecTitle')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('ecContent')"
                name="ecContent"
              >
                <takt-rich-editor
                  v-model:value="formState.ecContent"
                  :placeholder="pi.ph('ecContent')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecLeader')"
                name="ecLeader"
              >
                <TaktSelect
                  v-model:value="formState.ecLeader"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('ecLeader')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecLossAmount')"
                name="ecLossAmount"
              >
                <a-input-number
                  v-model:value="formState.ecLossAmount"
                  :placeholder="pi.ph('ecLossAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecDistinction')"
                name="ecDistinction"
              >
                <TaktSelect
                  v-model:value="formState.ecDistinction"
                  dict-type="logistics_ec_distinction_category"
                  :placeholder="pi.ph('ecDistinction')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecEntryDate')"
                name="ecEntryDate"
              >
                <a-date-picker
                  v-model:value="formState.ecEntryDate"
                  :placeholder="pi.ph('ecEntryDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
            <a-col :span="24">
              <a-form-item
                :label="pi.label('ecStatus')"
                name="ecStatus"
              >
                <TaktSelect
                  v-model:value="formState.ecStatus"
                  dict-type="logistics_ec_gijutsu_status"
                  :placeholder="pi.ph('ecStatus')"
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
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
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
    <!-- 下：子表 ecDetails -->
    <TaktEditableTable
      ref="ecDetailTableRef"
      v-model="childEcDetailRows"
      :columns="ecDetailFormColumns"
      :title="ecDetailPi.self()"
      :add-button-entity="ecDetailPi.self()"
      id-field="ecDetailId"
      :default-row="createDefaultEcDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 设变技术课维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useEcGijutsuI18n } from '../composables/use-ec-gijutsu-i18n'

/** 实体字段 i18n */
const pi = useEcGijutsuI18n()

import type { EcGijutsuCreate } from '@/types/logistics/manufacturing/engineering-change/ec-gijutsu'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文（当前公司 CultureCode 注入源） */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
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
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","ecCode","ecIssueDate","changeStatus","ecTitle","ecContent","ecLeader","ecLossAmount","ecDistinction","ecEntryDate","ecStatus","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { useEcDetailI18n } from '../composables/use-ec-detail-i18n'

const ecDetailPi = useEcDetailI18n()

const childEcDetailRows = ref<Record<string, unknown>[]>([])
const ecDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 ecDetail 可编辑列 */
const ecDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
,
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<EcGijutsuCreate & { ecGijutsuId?: string }> | null | undefined) {
  const rows_ecDetail = ((val as any)?.ecDetails ?? []) as Record<string, unknown>[]
  childEcDetailRows.value = rows_ecDetail
}

function createDefaultEcDetailRow(): Record<string, unknown> {
  return {

  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.ecGijutsuId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    ecDetails: ecDetailTableRef.value?.getRows?.() ?? childEcDetailRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      ecGijutsuId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EcGijutsuCreate & { ecGijutsuId?: string }> | null
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
  ecStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 ecGijutsuId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ecGijutsuId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).ecDetails
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
    if (!props.formData?.ecGijutsuId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  ecCode: [
    {
      required: true,
      message: pi.ph('ecCode'),
      trigger: 'blur'
    }
  ],
  ecIssueDate: [
    {
      required: true,
      message: pi.ph('ecIssueDate'),
      trigger: 'change'
    }
  ],
  changeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('changeStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('changeStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecTitle: [
    {
      required: true,
      message: pi.ph('ecTitle'),
      trigger: 'blur'
    }
  ],
  ecContent: [
    {
      required: true,
      message: pi.ph('ecContent'),
      trigger: 'blur'
    }
  ],
  ecLeader: [
    {
      required: true,
      message: pi.ph('ecLeader'),
      trigger: 'change'
    }
  ],
  ecLossAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('ecLossAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('ecLossAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecDistinction: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('ecDistinction'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('ecDistinction'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecEntryDate: [
    {
      required: true,
      message: pi.ph('ecEntryDate'),
      trigger: 'change'
    }
  ],
  ecStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('ecStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('ecStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await ecDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('changeStatus' in payload) {
    const rawchangeStatus = payload.changeStatus
    if (rawchangeStatus === undefined || rawchangeStatus === null || rawchangeStatus === '') {
      delete payload.changeStatus
    } else {
      const numchangeStatus = typeof rawchangeStatus === 'number' ? rawchangeStatus : Number(rawchangeStatus)
      if (Number.isFinite(numchangeStatus)) payload.changeStatus = numchangeStatus
      else delete payload.changeStatus
    }
  }
  if ('ecLossAmount' in payload) {
    const rawecLossAmount = payload.ecLossAmount
    if (rawecLossAmount === undefined || rawecLossAmount === null || rawecLossAmount === '') {
      delete payload.ecLossAmount
    } else {
      const numecLossAmount = typeof rawecLossAmount === 'number' ? rawecLossAmount : Number(rawecLossAmount)
      if (Number.isFinite(numecLossAmount)) payload.ecLossAmount = numecLossAmount
      else delete payload.ecLossAmount
    }
  }
  if ('ecDistinction' in payload) {
    const rawecDistinction = payload.ecDistinction
    if (rawecDistinction === undefined || rawecDistinction === null || rawecDistinction === '') {
      delete payload.ecDistinction
    } else {
      const numecDistinction = typeof rawecDistinction === 'number' ? rawecDistinction : Number(rawecDistinction)
      if (Number.isFinite(numecDistinction)) payload.ecDistinction = numecDistinction
      else delete payload.ecDistinction
    }
  }
  if ('ecStatus' in payload) {
    const rawecStatus = payload.ecStatus
    if (rawecStatus === undefined || rawecStatus === null || rawecStatus === '') {
      delete payload.ecStatus
    } else {
      const numecStatus = typeof rawecStatus === 'number' ? rawecStatus : Number(rawecStatus)
      if (Number.isFinite(numecStatus)) payload.ecStatus = numecStatus
      else delete payload.ecStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.ecGijutsuId) {
    payload.ecGijutsuId = props.formData.ecGijutsuId
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ecGijutsuId)
  childEcDetailRows.value = []
  ecDetailTableRef.value?.resetRows?.()
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
