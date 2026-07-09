<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output/components -->
<!-- 文件名称：pcba-output-form.vue -->
<!-- 功能描述：PCBA日报实体 达成率维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-output-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
    :disabled="loading || isMasterProdDateLocked"
  >
    <a-alert
      v-if="isMasterProdDateLocked"
      type="warning"
      show-icon
      class="mb-3 shrink-0"
      :message="prodDateLockedAlertMessage"
    />
    <a-tabs
      v-model:active-key="activeTab"
      class="pcba-output-form-tabs"
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
                :label="t('entity.pcbaoutput.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.plantcode') })"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutput.prodcategory')"
                name="prodCategory"
              >
                <TaktSelect
                  v-model:value="formState.prodCategory"
                  dict-type="logistics_prod_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.prodcategory') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutput.proddate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.proddate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                  :disabled-date="prodDatePickerDisabledDate"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutput.prodteam')"
                name="prodTeam"
              >
                <a-input
                  v-model:value="formState.prodTeam"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.prodteam') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutput.shiftno')"
                name="shiftNo"
              >
                <TaktSelect
                  v-model:value="formState.shiftNo"
                  dict-type="logistics_shift_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.shiftno') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutput.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.prodordercode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.pcbaOutputId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.pcbaoutput.modelcode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.modelcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.pcbaOutputId"
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
                :label="t('entity.pcbaoutput.batchno')"
                name="batchNo"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.batchno') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.pcbaoutput.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.pcbaOutputId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.pcbaoutput.prodorderqty')"
                name="prodOrderQty"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.prodorderqty') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.pcbaoutput.stdminutes')"
                name="stdMinutes"
              >
                <a-input-number
                  v-model:value="formState.stdMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.stdminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.pcbaoutput.stdshorts')"
                name="stdShorts"
              >
                <a-input-number
                  v-model:value="formState.stdShorts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.stdshorts') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.pcbaoutput.stdcapacity')"
                name="stdCapacity"
              >
                <a-input-number
                  v-model:value="formState.stdCapacity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.stdcapacity') })"
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
    <!-- 下：子表 pcbaOutputDetails -->
    <TaktEditableTable
      ref="pcbaOutputDetailTableRef"
      v-model="childPcbaOutputDetailRows"
      :columns="pcbaOutputDetailFormColumns"
      :title="t('entity.pcbaoutputdetail._self')"
      :add-button-entity="t('entity.pcbaoutputdetail._self')"
      id-field="pcbaOutputDetailId"
      :default-row="createDefaultPcbaOutputDetailRow"
      :disabled="loading || isMasterProdDateLocked"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * PCBA日报实体 达成率维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/pcba-output/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PcbaOutputCreate } from '@/types/logistics/manufacturing/output/pcba-output'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { getProductionOrderByCode } from '@/api/logistics/manufacturing/planning/production-order'
import { getModelDestinationByMaterial } from '@/api/logistics/materials/model-destination'
import {
  isOutputProdDateLocked,
  isOutputProdDateSelectable,
  outputProdDatePickerDisabledDate,
  resolveDefaultOutputProdDateYmd,
} from '../../composables/takt-output-prod-date-edit-lock'
import { useOutputProdDateI18n } from '../../composables/use-output-prod-date-i18n'

/** i18n 翻译函数 */
const { t } = useI18n()
const prodDateI18n = useOutputProdDateI18n()

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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","prodCategory","prodDate","prodTeam","shiftNo","prodOrderCode","modelCode","batchNo","materialCode","prodOrderQty","stdMinutes","stdShorts","stdCapacity","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childPcbaOutputDetailRows = ref<Record<string, unknown>[]>([])
const pcbaOutputDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 pcbaOutputDetail 可编辑列 */
const pcbaOutputDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'prodOrderCode',
    title: t('entity.pcbaoutputdetail.prodordercode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: t('entity.pcbaoutputdetail.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'timePeriod',
    title: t('entity.pcbaoutputdetail.timeperiod'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'shiftNo',
    title: t('entity.pcbaoutputdetail.shiftno'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'pcbBoardType',
    title: t('entity.pcbaoutputdetail.pcbboardtype'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'panelSide',
    title: t('entity.pcbaoutputdetail.panelside'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'batchQty',
    title: t('entity.pcbaoutputdetail.batchqty'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'dailyCompletedQty',
    title: t('entity.pcbaoutputdetail.dailycompletedqty'),
    editor: 'inputNumber',
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PcbaOutputCreate & { pcbaOutputId?: string }> | null | undefined) {
  childPcbaOutputDetailRows.value = ((val as any)?.pcbaOutputDetails ?? []) as Record<string, unknown>[]
}

function createDefaultPcbaOutputDetailRow(): Record<string, unknown> {
  return {
    prodOrderCode: '',
    lineNumber: (childPcbaOutputDetailRows.value.length + 1) * 10,
    timePeriod: '',
    shiftNo: 0,
    pcbBoardType: '',
    panelSide: '',
    batchQty: 0,
    dailyCompletedQty: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.pcbaOutputId ?? ''
  return {
    ...formState,
    pcbaOutputDetails: pcbaOutputDetailTableRef.value?.getRows?.() ?? childPcbaOutputDetailRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      pcbaOutputId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaOutputCreate & { pcbaOutputId?: string }> | null
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

/** 主表生产日期是否已锁定 */
const isMasterProdDateLocked = computed(() =>
  isOutputProdDateLocked(String(formState.prodDate ?? '').trim().slice(0, 10)),
)
/** 锁定提示文案 */
const prodDateLockedAlertMessage = computed(() =>
  prodDateI18n.prodDateLockedMessage(String(formState.prodDate ?? '').trim().slice(0, 10)),
)
/** 生产日期不可选已锁定/跨月/未来日期 */
function prodDatePickerDisabledDate(current: Parameters<typeof outputProdDatePickerDisabledDate>[0]) {
  return outputProdDatePickerDisabledDate(current)
}

/** 表单字段默认值 */
function applyFormDefaults(target: Record<string, unknown>) {
  if (!target.prodDate) {
    target.prodDate = resolveDefaultOutputProdDateYmd()
  }
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaOutputId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaOutputId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).pcbaOutputDetails
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

/** 按工单号回填主表字段（仅新增态） */
async function backfillFromProductionOrder() {
  if (props.formData?.pcbaOutputId) {
    return
  }
  const prodOrderCode = String(formState.prodOrderCode ?? '').trim()
  if (!prodOrderCode) {
    return
  }
  try {
    const order = await getProductionOrderByCode(prodOrderCode)
    if (order.plantCode) {
      formState.plantCode = order.plantCode
    }
    formState.materialCode = order.materialCode ?? ''
    formState.prodOrderQty = order.prodOrderQty ?? 0
    formState.batchNo = order.prodBatch ?? ''
    if (order.materialCode) {
      const model = await getModelDestinationByMaterial(order.materialCode)
      if (model?.modelCode) {
        formState.modelCode = model.modelCode
      }
    }
  } catch {
    // 工单不存在时保留用户已填内容
  }
}

watch(
  () => formState.prodOrderCode,
  () => {
    void backfillFromProductionOrder()
  }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.pcbaOutputId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.plantcode') }),
      trigger: 'blur'
    }
  ],
  prodCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.prodcategory') }),
      trigger: 'change'
    }
  ],
  prodDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.proddate') }),
      trigger: 'change'
    },
    {
      validator: async (_rule, value) => {
        const ymd = String(value ?? '').trim().slice(0, 10)
        if (!ymd) {
          return Promise.resolve()
        }
        if (isOutputProdDateLocked(ymd)) {
          return Promise.reject(prodDateI18n.prodDateLockedMessage(ymd))
        }
        if (!isOutputProdDateSelectable(ymd)) {
          return Promise.reject(prodDateI18n.prodDateOutOfRangeMessage())
        }
        return Promise.resolve()
      },
      trigger: 'change',
    },
  ],
  prodTeam: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.prodteam') }),
      trigger: 'blur'
    }
  ],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.shiftno') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.shiftno') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.prodordercode') }),
      trigger: 'blur'
    }
  ],
  modelCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.modelcode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.pcbaoutput.materialcode') }),
      trigger: 'blur'
    }
  ],
  prodOrderQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.prodorderqty') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.prodorderqty') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stdMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.stdminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.stdminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stdShorts: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.stdshorts') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.stdshorts') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stdCapacity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.stdcapacity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.pcbaoutput.stdcapacity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  if (isMasterProdDateLocked.value) {
    throw new Error(prodDateLockedAlertMessage.value)
  }
  await formRef.value?.validate()
  await pcbaOutputDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
  if ('prodOrderQty' in payload) {
    const rawprodOrderQty = payload.prodOrderQty
    payload.prodOrderQty = typeof rawprodOrderQty === 'number' ? rawprodOrderQty : Number(rawprodOrderQty)
  }
  if ('stdMinutes' in payload) {
    const rawstdMinutes = payload.stdMinutes
    payload.stdMinutes = typeof rawstdMinutes === 'number' ? rawstdMinutes : Number(rawstdMinutes)
  }
  if ('stdShorts' in payload) {
    const rawstdShorts = payload.stdShorts
    payload.stdShorts = typeof rawstdShorts === 'number' ? rawstdShorts : Number(rawstdShorts)
  }
  if ('stdCapacity' in payload) {
    const rawstdCapacity = payload.stdCapacity
    payload.stdCapacity = typeof rawstdCapacity === 'number' ? rawstdCapacity : Number(rawstdCapacity)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.pcbaOutputId)
  childPcbaOutputDetailRows.value = []
  pcbaOutputDetailTableRef.value?.resetRows?.()
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
