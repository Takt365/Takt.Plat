<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/sales/order/components -->
<!-- 文件名称：order-form.vue -->
<!-- 功能描述：APS 排程订单维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form order-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="order-form-tabs"
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
                  :disabled="!!formData?.apsOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('apsOrderCode')"
                name="apsOrderCode"
              >
                <a-input
                  v-model:value="formState.apsOrderCode"
                  :placeholder="pi.ph('apsOrderCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.apsOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedOrderId')"
                name="plannedOrderId"
              >
                <a-input
                  v-model:value="formState.plannedOrderId"
                  :placeholder="pi.ph('plannedOrderId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedOrderCode')"
                name="plannedOrderCode"
              >
                <a-input
                  v-model:value="formState.plannedOrderCode"
                  :placeholder="pi.ph('plannedOrderCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.apsOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterials/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.apsOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderQuantity')"
                name="orderQuantity"
              >
                <a-input-number
                  v-model:value="formState.orderQuantity"
                  :placeholder="pi.ph('orderQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unitOfMeasure')"
                name="unitOfMeasure"
              >
                <TaktSelect
                  v-model:value="formState.unitOfMeasure"
                  dict-type="logistics_unit_of_measure_code"
                  :placeholder="pi.ph('unitOfMeasure')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('routingCode')"
                name="routingCode"
              >
                <TaktSelect
                  v-model:value="formState.routingCode"
                  api-url="TaktRoutings/options"
                  :placeholder="pi.ph('routingCode')"
                  :disabled="!!formData?.apsOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedStartTime')"
                name="plannedStartTime"
              >
                <a-date-picker
                  v-model:value="formState.plannedStartTime"
                  :placeholder="pi.ph('plannedStartTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedEndTime')"
                name="plannedEndTime"
              >
                <a-date-picker
                  v-model:value="formState.plannedEndTime"
                  :placeholder="pi.ph('plannedEndTime')"
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
                :label="pi.label('orderStatus')"
                name="orderStatus"
              >
                <TaktSelect
                  v-model:value="formState.orderStatus"
                  dict-type="aps_order_status"
                  :placeholder="pi.ph('orderStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('apsScheduleId')"
                name="apsScheduleId"
              >
                <a-input
                  v-model:value="formState.apsScheduleId"
                  :placeholder="pi.ph('apsScheduleId')"
                  show-count
                  :maxlength="20"
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
    <!-- 下：子表 operations -->
    <TaktEditableTable
      ref="apsOperationTableRef"
      v-model="childApsOperationRows"
      :columns="apsOperationFormColumns"
      :title="apsOperationPi.self()"
      :add-button-entity="apsOperationPi.self()"
      id-field="apsOperationId"
      :default-row="createDefaultApsOperationRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-routingItemId="{ record }">
        <TaktSelect
          v-model:value="record.routingItemId"
          api-url="TaktRoutingItems/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="apsOperationPi.queryPh('routingItemId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-workCenterCode="{ record }">
        <TaktSelect
          v-model:value="record.workCenterCode"
          api-url="TaktWorkCenters/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="apsOperationPi.queryPh('workCenterCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-workCenterResourceId="{ record }">
        <TaktSelect
          v-model:value="record.workCenterResourceId"
          api-url="TaktWorkCenterResources/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="apsOperationPi.queryPh('workCenterResourceId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-operationStatus="{ record }">
        <TaktSelect
          v-model:value="record.operationStatus"
          dict-type="aps_operation_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="apsOperationPi.ph('operationStatus')"
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
          :placeholder="apsOperationPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * APS 排程订单维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/order/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useApsOrderI18n } from '../composables/use-order-i18n'

/** 实体字段 i18n */
const pi = useApsOrderI18n()

import type { ApsOrderCreate } from '@/types/logistics/manufacturing/aps/order'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","apsOrderCode","plannedOrderId","plannedOrderCode","materialCode","orderQuantity","unitOfMeasure","routingCode","plannedStartTime","plannedEndTime","orderStatus","apsScheduleId","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useApsOperationI18n } from '../composables/use-operation-i18n'

const apsOperationPi = useApsOperationI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childApsOperationRows = ref<Record<string, unknown>[]>([])
const apsOperationTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedApsOperationRow(row: Record<string, unknown>): boolean {
  const id = row.apsOperationId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextApsOperationLineNumber(): number {
  const rows = apsOperationTableRef.value?.getRows?.() ?? childApsOperationRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 apsOperation 可编辑列 */
const apsOperationFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: apsOperationPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'routingItemId',
    title: apsOperationPi.label('routingItemId'),
    width: 140,
  },
  {
    key: 'processCode',
    title: apsOperationPi.label('processCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'processName',
    title: apsOperationPi.label('processName'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: apsOperationPi.ph('processName'),
  },
  {
    key: 'workCenterCode',
    title: apsOperationPi.label('workCenterCode'),
    width: 140,
  },
  {
    key: 'workCenterResourceId',
    title: apsOperationPi.label('workCenterResourceId'),
    width: 140,
  },
  {
    key: 'plannedStartTime',
    title: apsOperationPi.label('plannedStartTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'plannedEndTime',
    title: apsOperationPi.label('plannedEndTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'plannedDurationMinutes',
    title: apsOperationPi.label('plannedDurationMinutes'),
    width: 140,
  },
  {
    key: 'changeoverMinutes',
    title: apsOperationPi.label('changeoverMinutes'),
    width: 140,
  },
  {
    key: 'operationStatus',
    title: apsOperationPi.label('operationStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: apsOperationPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<ApsOrderCreate & { apsOrderId?: string }> | null | undefined) {
  const rows_apsOperation = ((val as any)?.operations ?? []) as Record<string, unknown>[]
  childApsOperationRows.value = rows_apsOperation
}

function createDefaultApsOperationRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextApsOperationLineNumber(),
    routingItemId: '',
    processCode: '',
    processName: '',
    workCenterCode: '',
    workCenterResourceId: '',
    plannedStartTime: '',
    plannedEndTime: '',
    plannedDurationMinutes: 0,
    changeoverMinutes: 0,
    operationStatus: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.apsOrderId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    operations: apsOperationTableRef.value?.getRows?.() ?? childApsOperationRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
        apsOrderId: masterId,
      }
      if (isUpdate && isPersistedApsOperationRow(row)) {
        normalized.apsOperationId = row.apsOperationId
      } else {
        delete normalized.apsOperationId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ApsOrderCreate & { apsOrderId?: string }> | null
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

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 apsOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.apsOrderId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).operations
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
    const isCreate = !props.formData?.apsOrderId
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
  apsOrderCode: [
    {
      required: true,
      message: pi.ph('apsOrderCode'),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  orderQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('orderQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('orderQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unitOfMeasure: [
    {
      required: true,
      message: pi.ph('unitOfMeasure'),
      trigger: 'change'
    }
  ],
  orderStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('orderStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('orderStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await apsOperationTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('orderQuantity' in payload) {
    const raworderQuantity = payload.orderQuantity
    payload.orderQuantity = typeof raworderQuantity === 'number' ? raworderQuantity : Number(raworderQuantity)
  }
  if ('orderStatus' in payload) {
    const raworderStatus = payload.orderStatus
    payload.orderStatus = typeof raworderStatus === 'number' ? raworderStatus : Number(raworderStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.apsOrderId)
  childApsOperationRows.value = []
  apsOperationTableRef.value?.resetRows?.()
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
