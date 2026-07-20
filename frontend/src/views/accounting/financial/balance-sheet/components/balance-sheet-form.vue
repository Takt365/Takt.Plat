<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/balance-sheet/components -->
<!-- 文件名称：balance-sheet-form.vue -->
<!-- 功能描述：资产负债表行实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="balance-sheet-form-tabs"
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
                :label="pi.label('relatedPlant')"
                name="relatedPlant"
              >
                <TaktSelect
                  v-model:value="formState.relatedPlant"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('relatedPlant')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('periodCode')"
                name="periodCode"
              >
                <a-input
                  v-model:value="formState.periodCode"
                  :placeholder="pi.ph('periodCode')"
                  show-count
                  :maxlength="6"
                  allow-clear
                  :disabled="!!formData?.balanceSheetId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('statementLineCode')"
                name="statementLineCode"
              >
                <a-input
                  v-model:value="formState.statementLineCode"
                  :placeholder="pi.ph('statementLineCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.balanceSheetId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('statementLineName')"
                name="statementLineName"
              >
                <a-input
                  v-model:value="formState.statementLineName"
                  :placeholder="pi.ph('statementLineName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountTitleCode')"
                name="accountTitleCode"
              >
                <TaktSelect
                  v-model:value="formState.accountTitleCode"
                  api-url="TaktAccountTitles/options"
                  :placeholder="pi.ph('accountTitleCode')"
                  :disabled="!!formData?.balanceSheetId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountTitleName')"
                name="accountTitleName"
              >
                <a-input
                  v-model:value="formState.accountTitleName"
                  :placeholder="pi.ph('accountTitleName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lineCategory')"
                name="lineCategory"
              >
                <TaktSelect
                  v-model:value="formState.lineCategory"
                  dict-type="accounting_balance_sheet_line_category"
                  :placeholder="pi.ph('lineCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('balanceDirection')"
                name="balanceDirection"
              >
                <a-input-number
                  v-model:value="formState.balanceDirection"
                  :placeholder="pi.ph('balanceDirection')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isTotalLine')"
                name="isTotalLine"
              >
                <TaktSelect
                  v-model:value="formState.isTotalLine"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isTotalLine')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('openingBalance')"
                name="openingBalance"
              >
                <a-input-number
                  v-model:value="formState.openingBalance"
                  :placeholder="pi.ph('openingBalance')"
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
                :label="pi.label('debitAmount')"
                name="debitAmount"
              >
                <a-input-number
                  v-model:value="formState.debitAmount"
                  :placeholder="pi.ph('debitAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('creditAmount')"
                name="creditAmount"
              >
                <a-input-number
                  v-model:value="formState.creditAmount"
                  :placeholder="pi.ph('creditAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('closingBalance')"
                name="closingBalance"
              >
                <a-input-number
                  v-model:value="formState.closingBalance"
                  :placeholder="pi.ph('closingBalance')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('presentationAmount')"
                name="presentationAmount"
              >
                <a-input-number
                  v-model:value="formState.presentationAmount"
                  :placeholder="pi.ph('presentationAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('priorPeriodAmount')"
                name="priorPeriodAmount"
              >
                <a-input-number
                  v-model:value="formState.priorPeriodAmount"
                  :placeholder="pi.ph('priorPeriodAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('currencyCode')"
                name="currencyCode"
              >
                <TaktSelect
                  v-model:value="formState.currencyCode"
                  dict-type="accounting_currency_code"
                  :placeholder="pi.ph('currencyCode')"
                  :disabled="!!formData?.balanceSheetId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('balanceSheetStatus')"
                name="balanceSheetStatus"
              >
                <TaktSelect
                  v-model:value="formState.balanceSheetStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('balanceSheetStatus')"
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
 * 资产负债表行实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/balance-sheet/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useBalanceSheetI18n } from '../composables/use-balance-sheet-i18n'

/** 实体字段 i18n */
const pi = useBalanceSheetI18n()
import type { BalanceSheetCreate } from '@/types/accounting/financial/balance-sheet'
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
  formData?: Partial<BalanceSheetCreate & { balanceSheetId?: string }> | null
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
  lineCategory: 1,
  currencyCode: "CNY"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 balanceSheetId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.balanceSheetId) {
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
    if (!props.formData?.balanceSheetId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  relatedPlant: [
    {
      required: true,
      message: pi.ph('relatedPlant'),
      trigger: 'change'
    }
  ],
  periodCode: [
    {
      required: true,
      message: pi.ph('periodCode'),
      trigger: 'blur'
    }
  ],
  statementLineCode: [
    {
      required: true,
      message: pi.ph('statementLineCode'),
      trigger: 'blur'
    }
  ],
  statementLineName: [
    {
      required: true,
      message: pi.ph('statementLineName'),
      trigger: 'blur'
    }
  ],
  lineCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('lineCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('lineCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  balanceDirection: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('balanceDirection'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('balanceDirection'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isTotalLine: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isTotalLine'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isTotalLine'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  openingBalance: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('openingBalance'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('openingBalance'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  debitAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('debitAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('debitAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  creditAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('creditAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('creditAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  closingBalance: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('closingBalance'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('closingBalance'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  presentationAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('presentationAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('presentationAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priorPeriodAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('priorPeriodAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('priorPeriodAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'change'
    }
  ],
  balanceSheetStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('balanceSheetStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('balanceSheetStatus'))
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
  if ('lineCategory' in payload) {
    const rawlineCategory = payload.lineCategory
    payload.lineCategory = typeof rawlineCategory === 'number' ? rawlineCategory : Number(rawlineCategory)
  }
  if ('balanceDirection' in payload) {
    const rawbalanceDirection = payload.balanceDirection
    payload.balanceDirection = typeof rawbalanceDirection === 'number' ? rawbalanceDirection : Number(rawbalanceDirection)
  }
  if ('isTotalLine' in payload) {
    const rawisTotalLine = payload.isTotalLine
    payload.isTotalLine = typeof rawisTotalLine === 'number' ? rawisTotalLine : Number(rawisTotalLine)
  }
  if ('openingBalance' in payload) {
    const rawopeningBalance = payload.openingBalance
    payload.openingBalance = typeof rawopeningBalance === 'number' ? rawopeningBalance : Number(rawopeningBalance)
  }
  if ('debitAmount' in payload) {
    const rawdebitAmount = payload.debitAmount
    payload.debitAmount = typeof rawdebitAmount === 'number' ? rawdebitAmount : Number(rawdebitAmount)
  }
  if ('creditAmount' in payload) {
    const rawcreditAmount = payload.creditAmount
    payload.creditAmount = typeof rawcreditAmount === 'number' ? rawcreditAmount : Number(rawcreditAmount)
  }
  if ('closingBalance' in payload) {
    const rawclosingBalance = payload.closingBalance
    payload.closingBalance = typeof rawclosingBalance === 'number' ? rawclosingBalance : Number(rawclosingBalance)
  }
  if ('presentationAmount' in payload) {
    const rawpresentationAmount = payload.presentationAmount
    payload.presentationAmount = typeof rawpresentationAmount === 'number' ? rawpresentationAmount : Number(rawpresentationAmount)
  }
  if ('priorPeriodAmount' in payload) {
    const rawpriorPeriodAmount = payload.priorPeriodAmount
    payload.priorPeriodAmount = typeof rawpriorPeriodAmount === 'number' ? rawpriorPeriodAmount : Number(rawpriorPeriodAmount)
  }
  if ('balanceSheetStatus' in payload) {
    const rawbalanceSheetStatus = payload.balanceSheetStatus
    payload.balanceSheetStatus = typeof rawbalanceSheetStatus === 'number' ? rawbalanceSheetStatus : Number(rawbalanceSheetStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.balanceSheetId)

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
