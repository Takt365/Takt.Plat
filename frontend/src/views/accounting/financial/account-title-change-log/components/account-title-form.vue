<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/account-title-change-log/components -->
<!-- 文件名称：account-title-form.vue -->
<!-- 功能描述：会计科目实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form account-title-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="account-title-form-tabs"
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
                :label="t('entity.accounttitle.titlecode')"
                name="titleCode"
              >
                <a-input
                  v-model:value="formState.titleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.titlecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.accountTitleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.titlename')"
                name="titleName"
              >
                <a-input
                  v-model:value="formState.titleName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.titlename') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.parentid')"
                name="parentId"
              >
                <a-input
                  v-model:value="formState.parentId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.parentid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.titletype')"
                name="titleType"
              >
                <a-input-number
                  v-model:value="formState.titleType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.titletype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.balancedirection')"
                name="balanceDirection"
              >
                <a-input-number
                  v-model:value="formState.balanceDirection"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.balancedirection') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.titlelevel')"
                name="titleLevel"
              >
                <a-input-number
                  v-model:value="formState.titleLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.titlelevel') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.isauxiliary')"
                name="isAuxiliary"
              >
                <a-input-number
                  v-model:value="formState.isAuxiliary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isauxiliary') })"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.auxiliarytype')"
                name="auxiliaryType"
              >
                <a-input-number
                  v-model:value="formState.auxiliaryType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.auxiliarytype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.isquantity')"
                name="isQuantity"
              >
                <a-input-number
                  v-model:value="formState.isQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.iscurrency')"
                name="isCurrency"
              >
                <a-input-number
                  v-model:value="formState.isCurrency"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.iscurrency') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.iscash')"
                name="isCash"
              >
                <a-input-number
                  v-model:value="formState.isCash"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.iscash') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.isbank')"
                name="isBank"
              >
                <a-input-number
                  v-model:value="formState.isBank"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.isbank') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accounttitle.relatedplant') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.titlestatus')"
                name="titleStatus"
              >
                <TaktSelect
                  v-model:value="formState.titleStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.titlestatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.validfrom')"
                name="validFrom"
              >
                <a-date-picker
                  v-model:value="formState.validFrom"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validfrom') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accounttitle.validto')"
                name="validTo"
              >
                <a-date-picker
                  v-model:value="formState.validTo"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validto') })"
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
      ref="accountTitleChangeLogTableRef"
      v-model="childAccountTitleChangeLogRows"
      :columns="accountTitleChangeLogFormColumns"
      :title="t('entity.accounttitlechangelog._self')"
      :add-button-entity="t('entity.accounttitlechangelog._self')"
      id-field="accountTitleChangeLogId"
      :default-row="createDefaultAccountTitleChangeLogRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 会计科目实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/account-title-change-log/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { AccountTitleCreate } from '@/types/accounting/financial/account-title'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","titleCode","titleName","parentId","titleType","balanceDirection","titleLevel","isAuxiliary","auxiliaryType","isQuantity","isCurrency","isCash","isBank","relatedPlant","titleStatus","validFrom","validTo","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childAccountTitleChangeLogRows = ref<Record<string, unknown>[]>([])
const accountTitleChangeLogTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 accountTitleChangeLog 可编辑列 */
const accountTitleChangeLogFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'titleCode',
    title: t('entity.accounttitlechangelog.titlecode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'changeFields',
    title: t('entity.accounttitlechangelog.changefields'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.accounttitlechangelog.changefields') }),
  },
  {
    key: 'changeTime',
    title: t('entity.accounttitlechangelog.changetime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'changeBy',
    title: t('entity.accounttitlechangelog.changeby'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.accounttitlechangelog.changeby') }),
  },
  {
    key: 'changeReason',
    title: t('entity.accounttitlechangelog.changereason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.accounttitlechangelog.changereason') }),
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
function syncChildRowsFromFormData(val: Partial<AccountTitleCreate & { accountTitleId?: string }> | null | undefined) {
  childAccountTitleChangeLogRows.value = ((val as any)?.changeLogs ?? []) as Record<string, unknown>[]
}

function createDefaultAccountTitleChangeLogRow(): Record<string, unknown> {
  return {
    titleCode: '',
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
  const masterId = props.formData?.accountTitleId ?? ''
  return {
    ...formState,
    changeLogs: accountTitleChangeLogTableRef.value?.getRows?.() ?? childAccountTitleChangeLogRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      accountTitleId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AccountTitleCreate & { accountTitleId?: string }> | null
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
  titleStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 accountTitleId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.accountTitleId) {
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
    const isCreate = !props.formData?.accountTitleId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  titleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accounttitle.titlecode') }),
      trigger: 'blur'
    }
  ],
  titleName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accounttitle.titlename') }),
      trigger: 'blur'
    }
  ],
  parentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accounttitle.parentid') }),
      trigger: 'blur'
    }
  ],
  titleType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.titletype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.titletype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  balanceDirection: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.balancedirection') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.balancedirection') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  titleLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.titlelevel') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.titlelevel') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isAuxiliary: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.isauxiliary') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.isauxiliary') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  auxiliaryType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.auxiliarytype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.auxiliarytype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.isquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.isquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isCurrency: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.iscurrency') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.iscurrency') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isCash: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.iscash') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.iscash') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isBank: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.isbank') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.isbank') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  titleStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.titlestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.accounttitle.titlestatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  validFrom: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validfrom') }),
      trigger: 'change'
    }
  ],
  validTo: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accounttitle.validto') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await accountTitleChangeLogTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('titleType' in payload) {
    const rawtitleType = payload.titleType
    payload.titleType = typeof rawtitleType === 'number' ? rawtitleType : Number(rawtitleType)
  }
  if ('balanceDirection' in payload) {
    const rawbalanceDirection = payload.balanceDirection
    payload.balanceDirection = typeof rawbalanceDirection === 'number' ? rawbalanceDirection : Number(rawbalanceDirection)
  }
  if ('titleLevel' in payload) {
    const rawtitleLevel = payload.titleLevel
    payload.titleLevel = typeof rawtitleLevel === 'number' ? rawtitleLevel : Number(rawtitleLevel)
  }
  if ('isAuxiliary' in payload) {
    const rawisAuxiliary = payload.isAuxiliary
    payload.isAuxiliary = typeof rawisAuxiliary === 'number' ? rawisAuxiliary : Number(rawisAuxiliary)
  }
  if ('auxiliaryType' in payload) {
    const rawauxiliaryType = payload.auxiliaryType
    payload.auxiliaryType = typeof rawauxiliaryType === 'number' ? rawauxiliaryType : Number(rawauxiliaryType)
  }
  if ('isQuantity' in payload) {
    const rawisQuantity = payload.isQuantity
    payload.isQuantity = typeof rawisQuantity === 'number' ? rawisQuantity : Number(rawisQuantity)
  }
  if ('isCurrency' in payload) {
    const rawisCurrency = payload.isCurrency
    payload.isCurrency = typeof rawisCurrency === 'number' ? rawisCurrency : Number(rawisCurrency)
  }
  if ('isCash' in payload) {
    const rawisCash = payload.isCash
    payload.isCash = typeof rawisCash === 'number' ? rawisCash : Number(rawisCash)
  }
  if ('isBank' in payload) {
    const rawisBank = payload.isBank
    payload.isBank = typeof rawisBank === 'number' ? rawisBank : Number(rawisBank)
  }
  if ('titleStatus' in payload) {
    const rawtitleStatus = payload.titleStatus
    payload.titleStatus = typeof rawtitleStatus === 'number' ? rawtitleStatus : Number(rawtitleStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.accountTitleId)
  childAccountTitleChangeLogRows.value = []
  accountTitleChangeLogTableRef.value?.resetRows?.()
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
