<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/work-order/components -->
<!-- 文件名称：work-order-form.vue -->
<!-- 功能描述：维护工单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form work-order-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="work-order-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
              <a-col :span="12">
                <a-form-item
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
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
    <!-- 下：子表 materials -->
    <TaktEditableTable
      ref="maintenanceWorkOrderMaterialTableRef"
      v-model="childMaintenanceWorkOrderMaterialRows"
      :columns="maintenanceWorkOrderMaterialFormColumns"
      :title="maintenanceWorkOrderMaterialPi.self()"
      :add-button-entity="maintenanceWorkOrderMaterialPi.self()"
      id-field="maintenanceWorkOrderMaterialId"
      :default-row="createDefaultMaintenanceWorkOrderMaterialRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="maintenanceWorkOrderMaterialPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 维护工单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/work-order/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaintenanceWorkOrderI18n } from '../composables/use-work-order-i18n'

/** 实体字段 i18n */
const pi = useMaintenanceWorkOrderI18n()

import type { MaintenanceWorkOrderCreate } from '@/types/logistics/maintenance/work-order'
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
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","workOrderCode","maintenanceNotificationId","notificationCode","equipmentId","EquipCode","equipmentName","maintenanceCategory","maintenanceType","workOrderStatus","priority","workCenter","assignedTechnician","maintenanceCompany","plannedStartTime","plannedEndTime","actualStartTime","actualEndTime","faultDescription","maintenanceContent","solution","costCenterId","costCenterCode","costElementId","costElementCode","totalMaterialCost","totalLaborCost","totalOtherCost","totalCost","settlementStatus","settlementTime","completedAt","acceptedBy","acceptedAt","maintenanceResult","nextMaintenanceDate","maintenanceCycleDays","maintenanceImages","maintenanceDocuments","acceptedSummary","isHistoryArchived","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useMaintenanceWorkOrderMaterialI18n } from '../composables/use-work-order-material-i18n'

const maintenanceWorkOrderMaterialPi = useMaintenanceWorkOrderMaterialI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childMaintenanceWorkOrderMaterialRows = ref<Record<string, unknown>[]>([])
const maintenanceWorkOrderMaterialTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedMaintenanceWorkOrderMaterialRow(row: Record<string, unknown>): boolean {
  const id = row.maintenanceWorkOrderMaterialId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextMaintenanceWorkOrderMaterialLineNumber(): number {
  const rows = maintenanceWorkOrderMaterialTableRef.value?.getRows?.() ?? childMaintenanceWorkOrderMaterialRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 maintenanceWorkOrderMaterial 可编辑列 */
const maintenanceWorkOrderMaterialFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'workOrderCode',
    title: maintenanceWorkOrderMaterialPi.label('workOrderCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: maintenanceWorkOrderMaterialPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'materialId',
    title: maintenanceWorkOrderMaterialPi.label('materialId'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialCode',
    title: maintenanceWorkOrderMaterialPi.label('materialCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialDescription',
    title: maintenanceWorkOrderMaterialPi.label('materialDescription'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'requiredQuantity',
    title: maintenanceWorkOrderMaterialPi.label('requiredQuantity'),
    width: 140,
  },
  {
    key: 'issuedQuantity',
    title: maintenanceWorkOrderMaterialPi.label('issuedQuantity'),
    width: 140,
  },
  {
    key: 'materialUnit',
    title: maintenanceWorkOrderMaterialPi.label('materialUnit'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'unitPrice',
    title: maintenanceWorkOrderMaterialPi.label('unitPrice'),
    width: 140,
  },
  {
    key: 'amount',
    title: maintenanceWorkOrderMaterialPi.label('amount'),
    width: 140,
  },
  {
    key: 'warehouseCode',
    title: maintenanceWorkOrderMaterialPi.label('warehouseCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: maintenanceWorkOrderMaterialPi.ph('warehouseCode'),
  },
  {
    key: 'storageLocation',
    title: maintenanceWorkOrderMaterialPi.label('storageLocation'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: maintenanceWorkOrderMaterialPi.ph('storageLocation'),
  },
  {
    key: 'issueStatus',
    title: maintenanceWorkOrderMaterialPi.label('issueStatus'),
    width: 140,
  },
  {
    key: 'issueTime',
    title: maintenanceWorkOrderMaterialPi.label('issueTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'isObsolete',
    title: maintenanceWorkOrderMaterialPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaintenanceWorkOrderCreate & { maintenanceWorkOrderId?: string }> | null | undefined) {
  const rows_maintenanceWorkOrderMaterial = ((val as any)?.materials ?? []) as Record<string, unknown>[]
  childMaintenanceWorkOrderMaterialRows.value = rows_maintenanceWorkOrderMaterial
}

function createDefaultMaintenanceWorkOrderMaterialRow(): Record<string, unknown> {
  return {
    workOrderCode: '',
    lineNumber: allocateNextMaintenanceWorkOrderMaterialLineNumber(),
    materialId: '',
    materialCode: '',
    materialDescription: '',
    requiredQuantity: 0,
    issuedQuantity: 0,
    materialUnit: '',
    unitPrice: 0,
    amount: 0,
    warehouseCode: '',
    storageLocation: '',
    issueStatus: 0,
    issueTime: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.maintenanceWorkOrderId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    materials: maintenanceWorkOrderMaterialTableRef.value?.getRows?.() ?? childMaintenanceWorkOrderMaterialRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        maintenanceWorkOrderId: masterId,
      }
      if (isUpdate && isPersistedMaintenanceWorkOrderMaterialRow(row)) {
        normalized.maintenanceWorkOrderMaterialId = row.maintenanceWorkOrderMaterialId
      } else {
        delete normalized.maintenanceWorkOrderMaterialId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaintenanceWorkOrderCreate & { maintenanceWorkOrderId?: string }> | null
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
  workOrderStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 maintenanceWorkOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.maintenanceWorkOrderId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).materials
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
    const isCreate = !props.formData?.maintenanceWorkOrderId
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
      trigger: 'blur'
    }
  ],
  workOrderCode: [
    {
      required: true,
      message: pi.ph('workOrderCode'),
      trigger: 'blur'
    }
  ],
  equipmentId: [
    {
      required: true,
      message: pi.ph('equipmentId'),
      trigger: 'blur'
    }
  ],
  EquipCode: [
    {
      required: true,
      message: pi.ph('EquipCode'),
      trigger: 'blur'
    }
  ],
  equipmentName: [
    {
      required: true,
      message: pi.ph('equipmentName'),
      trigger: 'blur'
    }
  ],
  maintenanceCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maintenanceCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maintenanceCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maintenanceType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maintenanceType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  workOrderStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('workOrderStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('workOrderStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('priority'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('priority'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalMaterialCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalMaterialCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalMaterialCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalLaborCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalLaborCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalLaborCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalOtherCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalOtherCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalOtherCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  settlementStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('settlementStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('settlementStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceResult: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maintenanceResult'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maintenanceResult'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceCycleDays: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maintenanceCycleDays'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maintenanceCycleDays'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isHistoryArchived: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isHistoryArchived'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isHistoryArchived'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await maintenanceWorkOrderMaterialTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('maintenanceCategory' in payload) {
    const rawmaintenanceCategory = payload.maintenanceCategory
    payload.maintenanceCategory = typeof rawmaintenanceCategory === 'number' ? rawmaintenanceCategory : Number(rawmaintenanceCategory)
  }
  if ('maintenanceType' in payload) {
    const rawmaintenanceType = payload.maintenanceType
    payload.maintenanceType = typeof rawmaintenanceType === 'number' ? rawmaintenanceType : Number(rawmaintenanceType)
  }
  if ('workOrderStatus' in payload) {
    const rawworkOrderStatus = payload.workOrderStatus
    payload.workOrderStatus = typeof rawworkOrderStatus === 'number' ? rawworkOrderStatus : Number(rawworkOrderStatus)
  }
  if ('priority' in payload) {
    const rawpriority = payload.priority
    payload.priority = typeof rawpriority === 'number' ? rawpriority : Number(rawpriority)
  }
  if ('totalMaterialCost' in payload) {
    const rawtotalMaterialCost = payload.totalMaterialCost
    payload.totalMaterialCost = typeof rawtotalMaterialCost === 'number' ? rawtotalMaterialCost : Number(rawtotalMaterialCost)
  }
  if ('totalLaborCost' in payload) {
    const rawtotalLaborCost = payload.totalLaborCost
    payload.totalLaborCost = typeof rawtotalLaborCost === 'number' ? rawtotalLaborCost : Number(rawtotalLaborCost)
  }
  if ('totalOtherCost' in payload) {
    const rawtotalOtherCost = payload.totalOtherCost
    payload.totalOtherCost = typeof rawtotalOtherCost === 'number' ? rawtotalOtherCost : Number(rawtotalOtherCost)
  }
  if ('totalCost' in payload) {
    const rawtotalCost = payload.totalCost
    payload.totalCost = typeof rawtotalCost === 'number' ? rawtotalCost : Number(rawtotalCost)
  }
  if ('settlementStatus' in payload) {
    const rawsettlementStatus = payload.settlementStatus
    payload.settlementStatus = typeof rawsettlementStatus === 'number' ? rawsettlementStatus : Number(rawsettlementStatus)
  }
  if ('maintenanceResult' in payload) {
    const rawmaintenanceResult = payload.maintenanceResult
    payload.maintenanceResult = typeof rawmaintenanceResult === 'number' ? rawmaintenanceResult : Number(rawmaintenanceResult)
  }
  if ('maintenanceCycleDays' in payload) {
    const rawmaintenanceCycleDays = payload.maintenanceCycleDays
    payload.maintenanceCycleDays = typeof rawmaintenanceCycleDays === 'number' ? rawmaintenanceCycleDays : Number(rawmaintenanceCycleDays)
  }
  if ('isHistoryArchived' in payload) {
    const rawisHistoryArchived = payload.isHistoryArchived
    payload.isHistoryArchived = typeof rawisHistoryArchived === 'number' ? rawisHistoryArchived : Number(rawisHistoryArchived)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.maintenanceWorkOrderId)
  childMaintenanceWorkOrderMaterialRows.value = []
  maintenanceWorkOrderMaterialTableRef.value?.resetRows?.()
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
