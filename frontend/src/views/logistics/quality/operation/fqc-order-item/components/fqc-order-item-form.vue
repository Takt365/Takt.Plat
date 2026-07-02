<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/fqc-order-item/components -->
<!-- 文件名称：fqc-order-item-form.vue -->
<!-- 功能描述：FQC出货检验单明细实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form fqc-order-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="fqc-order-item-form-tabs"
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
                :label="t('entity.fqcorderitem.fqcorderid')"
                name="fqcOrderId"
              >
                <a-input
                  v-model:value="formState.fqcOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.fqcorderid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.fqcordercode')"
                name="fqcOrderCode"
              >
                <a-input
                  v-model:value="formState.fqcOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.fqcordercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.fqcOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.fqcOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.materialname')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.materialname') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.batchno')"
                name="batchNo"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.batchno') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.warehousequantity')"
                name="warehouseQuantity"
              >
                <a-input-number
                  v-model:value="formState.warehouseQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.warehousequantity') })"
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
                :label="t('entity.fqcorderitem.standardcode')"
                name="standardCode"
              >
                <a-input
                  v-model:value="formState.standardCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.standardcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.fqcOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.samplingschemecode')"
                name="samplingSchemeCode"
              >
                <a-input
                  v-model:value="formState.samplingSchemeCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.samplingschemecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.fqcOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.inspectionmethod')"
                name="inspectionMethod"
              >
                <TaktSelect
                  v-model:value="formState.inspectionMethod"
                  dict-type="logistics_quality_inspection_method"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.inspectionmethod') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.samplequantity')"
                name="sampleQuantity"
              >
                <a-input-number
                  v-model:value="formState.sampleQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.samplequantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.qualifiedquantity')"
                name="qualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.qualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.qualifiedquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.unqualifiedquantity')"
                name="unqualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.unqualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.unqualifiedquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.inspectionreturnquantity')"
                name="inspectionReturnQuantity"
              >
                <a-input-number
                  v-model:value="formState.inspectionReturnQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.inspectionreturnquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.sampleserialno')"
                name="sampleSerialNo"
              >
                <a-input
                  v-model:value="formState.sampleSerialNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.sampleserialno') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.fqcorderitem.inspectiondescription')"
                name="inspectionDescription"
              >
                <a-textarea
                  v-model:value="formState.inspectionDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.fqcorderitem.inspectiondescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.fqcorderitem.inspectorby')"
                name="inspectorBy"
              >
                <a-input
                  v-model:value="formState.inspectorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.inspectorby') })"
                  show-count
                  :maxlength="50"
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
                :label="t('entity.fqcorderitem.inspectiondate')"
                name="inspectionDate"
              >
                <a-date-picker
                  v-model:value="formState.inspectionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.inspectiondate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.fqcorderitem.judgestatus')"
                name="judgeStatus"
              >
                <TaktSelect
                  v-model:value="formState.judgeStatus"
                  dict-type="logistics_quality_judge_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.judgestatus') })"
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
    <!-- 下：子表 defectHandlings -->
    <TaktEditableTable
      ref="fqcDefectHandlingTableRef"
      v-model="childFqcDefectHandlingRows"
      :columns="fqcDefectHandlingFormColumns"
      :title="t('entity.fqcdefecthandling._self')"
      :add-button-entity="t('entity.fqcdefecthandling._self')"
      id-field="fqcDefectHandlingId"
      :default-row="createDefaultFqcDefectHandlingRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * FQC出货检验单明细实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/fqc-order-item/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { FqcOrderItemCreate } from '@/types/logistics/quality/operation/fqc-order-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","fqcOrderId","fqcOrderCode","lineNumber","materialCode","materialName","batchNo","warehouseQuantity","standardCode","samplingSchemeCode","inspectionMethod","sampleQuantity","qualifiedQuantity","unqualifiedQuantity","inspectionReturnQuantity","sampleSerialNo","inspectionDescription","inspectorBy","inspectionDate","judgeStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childFqcDefectHandlingRows = ref<Record<string, unknown>[]>([])
const fqcDefectHandlingTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 fqcDefectHandling 可编辑列 */
const fqcDefectHandlingFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'fqcDefectHandlingCode',
    title: t('entity.fqcdefecthandling.code'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'fqcOrderCode',
    title: t('entity.fqcdefecthandling.fqcordercode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: t('entity.fqcdefecthandling.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'defectType',
    title: t('entity.fqcdefecthandling.defecttype'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'defectCode',
    title: t('entity.fqcdefecthandling.defectcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'defectDescription',
    title: t('entity.fqcdefecthandling.defectdescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.required', { field: t('entity.fqcdefecthandling.defectdescription') }),
    width: 140,
  },
  {
    key: 'defectQuantity',
    title: t('entity.fqcdefecthandling.defectquantity'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'handlingMethod',
    title: t('entity.fqcdefecthandling.handlingmethod'),
    editor: 'inputNumber',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<FqcOrderItemCreate & { fqcOrderItemId?: string }> | null | undefined) {
  childFqcDefectHandlingRows.value = ((val as any)?.defectHandlings ?? []) as Record<string, unknown>[]
}

function createDefaultFqcDefectHandlingRow(): Record<string, unknown> {
  return {
    fqcDefectHandlingCode: '',
    fqcOrderCode: '',
    lineNumber: (childFqcDefectHandlingRows.value.length + 1) * 10,
    defectType: 0,
    defectCode: '',
    defectDescription: '',
    defectQuantity: 0,
    handlingMethod: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.fqcOrderItemId ?? ''
  return {
    ...formState,
    defectHandlings: fqcDefectHandlingTableRef.value?.getRows?.() ?? childFqcDefectHandlingRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      fqcOrderItemId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<FqcOrderItemCreate & { fqcOrderItemId?: string }> | null
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
  inspectionMethod: 2,
  judgeStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 fqcOrderItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.fqcOrderItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).defectHandlings
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
    const isCreate = !props.formData?.fqcOrderItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  fqcOrderId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.fqcorderid') }),
      trigger: 'blur'
    }
  ],
  fqcOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.fqcordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.materialname') }),
      trigger: 'blur'
    }
  ],
  warehouseQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.warehousequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.warehousequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  standardCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.standardcode') }),
      trigger: 'blur'
    }
  ],
  samplingSchemeCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.samplingschemecode') }),
      trigger: 'blur'
    }
  ],
  inspectionMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.inspectionmethod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.inspectionmethod') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sampleQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.samplequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.samplequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  qualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.qualifiedquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.qualifiedquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unqualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.unqualifiedquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.unqualifiedquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionReturnQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.inspectionreturnquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.inspectionreturnquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectorBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.fqcorderitem.inspectorby') }),
      trigger: 'blur'
    }
  ],
  inspectionDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.inspectiondate') }),
      trigger: 'change'
    }
  ],
  judgeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.judgestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.fqcorderitem.judgestatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await fqcDefectHandlingTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('warehouseQuantity' in payload) {
    const rawwarehouseQuantity = payload.warehouseQuantity
    payload.warehouseQuantity = typeof rawwarehouseQuantity === 'number' ? rawwarehouseQuantity : Number(rawwarehouseQuantity)
  }
  if ('inspectionMethod' in payload) {
    const rawinspectionMethod = payload.inspectionMethod
    payload.inspectionMethod = typeof rawinspectionMethod === 'number' ? rawinspectionMethod : Number(rawinspectionMethod)
  }
  if ('sampleQuantity' in payload) {
    const rawsampleQuantity = payload.sampleQuantity
    payload.sampleQuantity = typeof rawsampleQuantity === 'number' ? rawsampleQuantity : Number(rawsampleQuantity)
  }
  if ('qualifiedQuantity' in payload) {
    const rawqualifiedQuantity = payload.qualifiedQuantity
    payload.qualifiedQuantity = typeof rawqualifiedQuantity === 'number' ? rawqualifiedQuantity : Number(rawqualifiedQuantity)
  }
  if ('unqualifiedQuantity' in payload) {
    const rawunqualifiedQuantity = payload.unqualifiedQuantity
    payload.unqualifiedQuantity = typeof rawunqualifiedQuantity === 'number' ? rawunqualifiedQuantity : Number(rawunqualifiedQuantity)
  }
  if ('inspectionReturnQuantity' in payload) {
    const rawinspectionReturnQuantity = payload.inspectionReturnQuantity
    payload.inspectionReturnQuantity = typeof rawinspectionReturnQuantity === 'number' ? rawinspectionReturnQuantity : Number(rawinspectionReturnQuantity)
  }
  if ('judgeStatus' in payload) {
    const rawjudgeStatus = payload.judgeStatus
    payload.judgeStatus = typeof rawjudgeStatus === 'number' ? rawjudgeStatus : Number(rawjudgeStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.fqcOrderItemId)
  childFqcDefectHandlingRows.value = []
  fqcDefectHandlingTableRef.value?.resetRows?.()
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
