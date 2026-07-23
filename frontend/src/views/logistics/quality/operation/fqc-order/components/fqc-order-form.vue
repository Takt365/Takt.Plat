<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/fqc-order/components -->
<!-- 文件名称：fqc-order-form.vue -->
<!-- 功能描述：FQC出货检验单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form fqc-order-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="fqc-order-form-tabs"
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
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  :disabled="!!formData?.fqcOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceCode')"
                name="sourceCode"
              >
                <TaktSelect
                  v-model:value="formState.sourceCode"
                  api-url="TaktSalesOrders/options"
                  :placeholder="pi.ph('sourceCode')"
                  :disabled="!!formData?.fqcOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inspectionDate')"
                name="inspectionDate"
              >
                <a-date-picker
                  v-model:value="formState.inspectionDate"
                  :placeholder="pi.ph('inspectionDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fqcOrderCode')"
                name="fqcOrderCode"
              >
                <a-input
                  v-model:value="formState.fqcOrderCode"
                  :placeholder="pi.ph('fqcOrderCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.fqcOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerCode')"
                name="customerCode"
              >
                <TaktSelect
                  v-model:value="formState.customerCode"
                  api-url="TaktCustomers/options"
                  :placeholder="pi.ph('customerCode')"
                  :disabled="!!formData?.fqcOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalWarehouseQuantity')"
                name="totalWarehouseQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalWarehouseQuantity"
                  :placeholder="pi.ph('totalWarehouseQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalSampleQuantity')"
                name="totalSampleQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalSampleQuantity"
                  :placeholder="pi.ph('totalSampleQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalQualifiedQuantity')"
                name="totalQualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalQualifiedQuantity"
                  :placeholder="pi.ph('totalQualifiedQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalUnqualifiedQuantity')"
                name="totalUnqualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalUnqualifiedQuantity"
                  :placeholder="pi.ph('totalUnqualifiedQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalInspectionReturnQuantity')"
                name="totalInspectionReturnQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalInspectionReturnQuantity"
                  :placeholder="pi.ph('totalInspectionReturnQuantity')"
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
                :label="pi.label('judgeBy')"
                name="judgeBy"
              >
                <TaktSelect
                  v-model:value="formState.judgeBy"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('judgeBy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('judgeDate')"
                name="judgeDate"
              >
                <a-date-picker
                  v-model:value="formState.judgeDate"
                  :placeholder="pi.ph('judgeDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('judgeDescription')"
                name="judgeDescription"
              >
                <a-textarea
                  v-model:value="formState.judgeDescription"
                  :placeholder="pi.ph('judgeDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('judgeStatus')"
                name="judgeStatus"
              >
                <TaktSelect
                  v-model:value="formState.judgeStatus"
                  dict-type="logistics_quality_judge_status"
                  :placeholder="pi.ph('judgeStatus')"
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="fqcOrderItemTableRef"
      v-model="childFqcOrderItemRows"
      :columns="fqcOrderItemFormColumns"
      :title="fqcOrderItemPi.self()"
      :add-button-entity="fqcOrderItemPi.self()"
      id-field="fqcOrderItemId"
      :default-row="createDefaultFqcOrderItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcOrderItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-standardCode="{ record }">
        <TaktSelect
          v-model:value="record.standardCode"
          api-url="TaktInspectionStandards/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcOrderItemPi.queryPh('standardCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-samplingSchemeCode="{ record }">
        <TaktSelect
          v-model:value="record.samplingSchemeCode"
          api-url="TaktSamplingSchemes/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcOrderItemPi.queryPh('samplingSchemeCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-inspectionMethod="{ record }">
        <TaktSelect
          v-model:value="record.inspectionMethod"
          dict-type="logistics_quality_inspection_method"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcOrderItemPi.ph('inspectionMethod')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-inspectorBy="{ record }">
        <TaktSelect
          v-model:value="record.inspectorBy"
          api-url="TaktEmployees/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcOrderItemPi.queryPh('inspectorBy', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-judgeStatus="{ record }">
        <TaktSelect
          v-model:value="record.judgeStatus"
          dict-type="logistics_quality_judge_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcOrderItemPi.ph('judgeStatus')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcOrderItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * FQC出货检验单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/fqc-order/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useFqcOrderI18n } from '../composables/use-fqc-order-i18n'

/** 实体字段 i18n */
const pi = useFqcOrderI18n()

import type { FqcOrderCreate } from '@/types/logistics/quality/operation/fqc-order'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","sourceCode","inspectionDate","fqcOrderCode","customerCode","totalWarehouseQuantity","totalSampleQuantity","totalQualifiedQuantity","totalUnqualifiedQuantity","totalInspectionReturnQuantity","judgeBy","judgeDate","judgeDescription","judgeStatus","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useFqcOrderItemI18n } from '../composables/use-fqc-order-item-i18n'

const fqcOrderItemPi = useFqcOrderItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childFqcOrderItemRows = ref<Record<string, unknown>[]>([])
const fqcOrderItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedFqcOrderItemRow(row: Record<string, unknown>): boolean {
  const id = row.fqcOrderItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextFqcOrderItemLineNumber(): number {
  const rows = fqcOrderItemTableRef.value?.getRows?.() ?? childFqcOrderItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 fqcOrderItem 可编辑列 */
const fqcOrderItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: fqcOrderItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'materialCode',
    title: fqcOrderItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'batchNo',
    title: fqcOrderItemPi.label('batchNo'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: fqcOrderItemPi.ph('batchNo'),
  },
  {
    key: 'warehouseQuantity',
    title: fqcOrderItemPi.label('warehouseQuantity'),
    width: 140,
  },
  {
    key: 'standardCode',
    title: fqcOrderItemPi.label('standardCode'),
    width: 140,
  },
  {
    key: 'samplingSchemeCode',
    title: fqcOrderItemPi.label('samplingSchemeCode'),
    width: 140,
  },
  {
    key: 'inspectionMethod',
    title: fqcOrderItemPi.label('inspectionMethod'),
    width: 140,
  },
  {
    key: 'sampleQuantity',
    title: fqcOrderItemPi.label('sampleQuantity'),
    width: 140,
  },
  {
    key: 'qualifiedQuantity',
    title: fqcOrderItemPi.label('qualifiedQuantity'),
    width: 140,
  },
  {
    key: 'unqualifiedQuantity',
    title: fqcOrderItemPi.label('unqualifiedQuantity'),
    width: 140,
  },
  {
    key: 'inspectionReturnQuantity',
    title: fqcOrderItemPi.label('inspectionReturnQuantity'),
    width: 140,
  },
  {
    key: 'sampleSerialNo',
    title: fqcOrderItemPi.label('sampleSerialNo'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: fqcOrderItemPi.ph('sampleSerialNo'),
  },
  {
    key: 'inspectionDescription',
    title: fqcOrderItemPi.label('inspectionDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: fqcOrderItemPi.ph('inspectionDescription'),
    width: 180,
  },
  {
    key: 'inspectorBy',
    title: fqcOrderItemPi.label('inspectorBy'),
    width: 140,
  },
  {
    key: 'inspectionDate',
    title: fqcOrderItemPi.label('inspectionDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'judgeStatus',
    title: fqcOrderItemPi.label('judgeStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: fqcOrderItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<FqcOrderCreate & { fqcOrderId?: string }> | null | undefined) {
  const rows_fqcOrderItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childFqcOrderItemRows.value = rows_fqcOrderItem
}

function createDefaultFqcOrderItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextFqcOrderItemLineNumber(),
    materialCode: '',
    batchNo: '',
    warehouseQuantity: 0,
    standardCode: '',
    samplingSchemeCode: '',
    inspectionMethod: 0,
    sampleQuantity: 0,
    qualifiedQuantity: 0,
    unqualifiedQuantity: 0,
    inspectionReturnQuantity: 0,
    sampleSerialNo: '',
    inspectionDescription: '',
    inspectorBy: '',
    inspectionDate: '',
    judgeStatus: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.fqcOrderId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: fqcOrderItemTableRef.value?.getRows?.() ?? childFqcOrderItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
        defectHandlings: masterId,
      }
      if (isUpdate && isPersistedFqcOrderItemRow(row)) {
        normalized.fqcOrderItemId = row.fqcOrderItemId
      } else {
        delete normalized.fqcOrderItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<FqcOrderCreate & { fqcOrderId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 fqcOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.fqcOrderId) {
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
    const isCreate = !props.formData?.fqcOrderId
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
  sourceCode: [
    {
      required: true,
      message: pi.ph('sourceCode'),
      trigger: 'change'
    }
  ],
  fqcOrderCode: [
    {
      required: true,
      message: pi.ph('fqcOrderCode'),
      trigger: 'blur'
    }
  ],
  totalWarehouseQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalWarehouseQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalWarehouseQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalSampleQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalSampleQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalSampleQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalQualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalQualifiedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalQualifiedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalUnqualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalUnqualifiedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalUnqualifiedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalInspectionReturnQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalInspectionReturnQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalInspectionReturnQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  judgeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('judgeStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('judgeStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await fqcOrderItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('totalWarehouseQuantity' in payload) {
    const rawtotalWarehouseQuantity = payload.totalWarehouseQuantity
    payload.totalWarehouseQuantity = typeof rawtotalWarehouseQuantity === 'number' ? rawtotalWarehouseQuantity : Number(rawtotalWarehouseQuantity)
  }
  if ('totalSampleQuantity' in payload) {
    const rawtotalSampleQuantity = payload.totalSampleQuantity
    payload.totalSampleQuantity = typeof rawtotalSampleQuantity === 'number' ? rawtotalSampleQuantity : Number(rawtotalSampleQuantity)
  }
  if ('totalQualifiedQuantity' in payload) {
    const rawtotalQualifiedQuantity = payload.totalQualifiedQuantity
    payload.totalQualifiedQuantity = typeof rawtotalQualifiedQuantity === 'number' ? rawtotalQualifiedQuantity : Number(rawtotalQualifiedQuantity)
  }
  if ('totalUnqualifiedQuantity' in payload) {
    const rawtotalUnqualifiedQuantity = payload.totalUnqualifiedQuantity
    payload.totalUnqualifiedQuantity = typeof rawtotalUnqualifiedQuantity === 'number' ? rawtotalUnqualifiedQuantity : Number(rawtotalUnqualifiedQuantity)
  }
  if ('totalInspectionReturnQuantity' in payload) {
    const rawtotalInspectionReturnQuantity = payload.totalInspectionReturnQuantity
    payload.totalInspectionReturnQuantity = typeof rawtotalInspectionReturnQuantity === 'number' ? rawtotalInspectionReturnQuantity : Number(rawtotalInspectionReturnQuantity)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.fqcOrderId)
  childFqcOrderItemRows.value = []
  fqcOrderItemTableRef.value?.resetRows?.()
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
