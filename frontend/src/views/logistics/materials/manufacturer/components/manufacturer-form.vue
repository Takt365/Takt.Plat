<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/manufacturer/components -->
<!-- 文件名称：manufacturer-form.vue -->
<!-- 功能描述：Takt制造商实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form manufacturer-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="manufacturer-form-tabs"
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
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerCode')"
                name="manufacturerCode"
              >
                <a-input
                  v-model:value="formState.manufacturerCode"
                  :placeholder="pi.ph('manufacturerCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.manufacturerId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerName')"
                name="manufacturerName"
              >
                <a-input
                  v-model:value="formState.manufacturerName"
                  :placeholder="pi.ph('manufacturerName')"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerShortName')"
                name="manufacturerShortName"
              >
                <a-input
                  v-model:value="formState.manufacturerShortName"
                  :placeholder="pi.ph('manufacturerShortName')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerType')"
                name="manufacturerType"
              >
                <TaktSelect
                  v-model:value="formState.manufacturerType"
                  dict-type="logistics_manufacturer_type"
                  :placeholder="pi.ph('manufacturerType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('industrySector')"
                name="industrySector"
              >
                <TaktSelect
                  v-model:value="formState.industrySector"
                  dict-type="logistics_industry_sector"
                  :placeholder="pi.ph('industrySector')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerTaxNumber')"
                name="manufacturerTaxNumber"
              >
                <a-input
                  v-model:value="formState.manufacturerTaxNumber"
                  :placeholder="pi.ph('manufacturerTaxNumber')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('registrationCountry')"
                name="registrationCountry"
              >
                <TaktSelect
                  v-model:value="formState.registrationCountry"
                  api-url="TaktIsoCodes/options"
                  :placeholder="pi.ph('registrationCountry')"
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
                :label="pi.label('registrationAddress1')"
                name="registrationAddress1"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress1"
                  :placeholder="pi.ph('registrationAddress1')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('registrationAddress2')"
                name="registrationAddress2"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress2"
                  :placeholder="pi.ph('registrationAddress2')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('registrationAddress3')"
                name="registrationAddress3"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress3"
                  :placeholder="pi.ph('registrationAddress3')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerPhone')"
                name="manufacturerPhone"
              >
                <a-input
                  v-model:value="formState.manufacturerPhone"
                  :placeholder="pi.ph('manufacturerPhone')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerFax')"
                name="manufacturerFax"
              >
                <a-input
                  v-model:value="formState.manufacturerFax"
                  :placeholder="pi.ph('manufacturerFax')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerEmail')"
                name="manufacturerEmail"
              >
                <a-input
                  v-model:value="formState.manufacturerEmail"
                  :placeholder="pi.ph('manufacturerEmail')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('manufacturerWebsite')"
                name="manufacturerWebsite"
              >
                <a-input
                  v-model:value="formState.manufacturerWebsite"
                  :placeholder="pi.ph('manufacturerWebsite')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactPerson')"
                name="contactPerson"
              >
                <a-input
                  v-model:value="formState.contactPerson"
                  :placeholder="pi.ph('contactPerson')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactPhone')"
                name="contactPhone"
              >
                <a-input
                  v-model:value="formState.contactPhone"
                  :placeholder="pi.ph('contactPhone')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactEmail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="pi.ph('contactEmail')"
                  show-count
                  :maxlength="100"
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
                :label="pi.label('manufacturerLevel')"
                name="manufacturerLevel"
              >
                <TaktSelect
                  v-model:value="formState.manufacturerLevel"
                  dict-type="logistics_grade_category"
                  :placeholder="pi.ph('manufacturerLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('qualityCertification')"
                name="qualityCertification"
              >
                <TaktSelect
                  v-model:value="formState.qualityCertification"
                  dict-type="logistics_quality_certification"
                  :placeholder="pi.ph('qualityCertification')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('evaluationScore')"
                name="evaluationScore"
              >
                <a-input-number
                  v-model:value="formState.evaluationScore"
                  :placeholder="pi.ph('evaluationScore')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('manufacturerStatus')"
                name="manufacturerStatus"
              >
                <TaktSelect
                  v-model:value="formState.manufacturerStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="pi.ph('manufacturerStatus')"
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
    <!-- 下：子表 manufacturerMaterials -->
    <TaktEditableTable
      ref="manufacturerMaterialTableRef"
      v-model="childManufacturerMaterialRows"
      :columns="manufacturerMaterialFormColumns"
      :title="manufacturerMaterialPi.self()"
      :add-button-entity="manufacturerMaterialPi.self()"
      id-field="manufacturerMaterialId"
      :default-row="createDefaultManufacturerMaterialRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt制造商实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/manufacturer/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useManufacturerI18n } from '../composables/use-manufacturer-i18n'

/** 实体字段 i18n */
const pi = useManufacturerI18n()

import type { ManufacturerCreate } from '@/types/logistics/materials/manufacturer'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","manufacturerCode","manufacturerName","manufacturerShortName","manufacturerType","industrySector","manufacturerTaxNumber","registrationCountry","registrationAddress1","registrationAddress2","registrationAddress3","manufacturerPhone","manufacturerFax","manufacturerEmail","manufacturerWebsite","contactPerson","contactPhone","contactEmail","manufacturerLevel","qualityCertification","evaluationScore","manufacturerStatus","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { useManufacturerMaterialI18n } from '../composables/use-manufacturer-material-i18n'

const manufacturerMaterialPi = useManufacturerMaterialI18n()

const childManufacturerMaterialRows = ref<Record<string, unknown>[]>([])
const manufacturerMaterialTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 manufacturerMaterial 可编辑列 */
const manufacturerMaterialFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: manufacturerMaterialPi.label('lineNumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'manufacturerMaterialCode',
    title: manufacturerMaterialPi.label('manufacturerMaterialCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'manufacturerMaterialName',
    title: manufacturerMaterialPi.label('manufacturerMaterialName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'manufacturerMaterialSpecification',
    title: manufacturerMaterialPi.label('manufacturerMaterialSpecification'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: manufacturerMaterialPi.ph('manufacturerMaterialSpecification'),
  },
  {
    key: 'materialCode',
    title: manufacturerMaterialPi.label('materialCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'extField',
    title: manufacturerMaterialPi.label('extField'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.extfield'),
    width: 140,
  },
  {
    key: 'remark',
    title: manufacturerMaterialPi.label('remark'),
    editor: 'textarea',
    rows: 2,
    placeholder: manufacturerMaterialPi.ph('remark'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<ManufacturerCreate & { manufacturerId?: string }> | null | undefined) {
  childManufacturerMaterialRows.value = ((val as any)?.manufacturerMaterials ?? []) as Record<string, unknown>[]
}

function createDefaultManufacturerMaterialRow(): Record<string, unknown> {
  return {
    lineNumber: (childManufacturerMaterialRows.value.length + 1) * 10,
    manufacturerMaterialCode: '',
    manufacturerMaterialName: '',
    manufacturerMaterialSpecification: '',
    materialCode: '',
    extField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.manufacturerId ?? ''
  return {
    ...formState,
    manufacturerMaterials: manufacturerMaterialTableRef.value?.getRows?.() ?? childManufacturerMaterialRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      manufacturerId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ManufacturerCreate & { manufacturerId?: string }> | null
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
  manufacturerType: 0,
  manufacturerLevel: 0,
  qualityCertification: 0,
  manufacturerStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 manufacturerId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.manufacturerId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).manufacturerMaterials
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
    const isCreate = !props.formData?.manufacturerId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  manufacturerCode: [
    {
      required: true,
      message: pi.ph('manufacturerCode'),
      trigger: 'blur'
    }
  ],
  manufacturerName: [
    {
      required: true,
      message: pi.ph('manufacturerName'),
      trigger: 'blur'
    }
  ],
  manufacturerType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('manufacturerType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('manufacturerType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  manufacturerLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('manufacturerLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('manufacturerLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  qualityCertification: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('qualityCertification'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('qualityCertification'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationScore: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('evaluationScore'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('evaluationScore'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  manufacturerStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('manufacturerStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('manufacturerStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await manufacturerMaterialTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('manufacturerType' in payload) {
    const rawmanufacturerType = payload.manufacturerType
    payload.manufacturerType = typeof rawmanufacturerType === 'number' ? rawmanufacturerType : Number(rawmanufacturerType)
  }
  if ('manufacturerLevel' in payload) {
    const rawmanufacturerLevel = payload.manufacturerLevel
    payload.manufacturerLevel = typeof rawmanufacturerLevel === 'number' ? rawmanufacturerLevel : Number(rawmanufacturerLevel)
  }
  if ('qualityCertification' in payload) {
    const rawqualityCertification = payload.qualityCertification
    payload.qualityCertification = typeof rawqualityCertification === 'number' ? rawqualityCertification : Number(rawqualityCertification)
  }
  if ('evaluationScore' in payload) {
    const rawevaluationScore = payload.evaluationScore
    payload.evaluationScore = typeof rawevaluationScore === 'number' ? rawevaluationScore : Number(rawevaluationScore)
  }
  if ('manufacturerStatus' in payload) {
    const rawmanufacturerStatus = payload.manufacturerStatus
    payload.manufacturerStatus = typeof rawmanufacturerStatus === 'number' ? rawmanufacturerStatus : Number(rawmanufacturerStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.manufacturerId)
  childManufacturerMaterialRows.value = []
  manufacturerMaterialTableRef.value?.resetRows?.()
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
